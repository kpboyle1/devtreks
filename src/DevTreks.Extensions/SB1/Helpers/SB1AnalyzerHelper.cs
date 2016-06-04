using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Helper functions for the resource stocks analyzer extension
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. Other modules demonstrate how to implement the analyzer patterns. 
    /// </summary>
    public class SB1AnalyzerHelper
    {
        //constructor
        public SB1AnalyzerHelper() { }
        public SB1AnalyzerHelper(CalculatorParameters calcParameters)
        {
            this.SB1CalculatorParams = calcParameters;
        }

        //properties
        //parameters needed by publishers
        public CalculatorParameters SB1CalculatorParams { get; set; }
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }

        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none                    = 0,
            //totals (i.e. total fat used by an operation's resource stock input)
            sbtotal1               = 1,
            //stats
            sbstat1                = 2,
            //change by year
            sbchangeyr             = 3,
            //change by id
            sbchangeid             = 4,
            //change by alternativetype
            sbchangealt            = 5,
            //progress   
            sbprogress1            = 6
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.SB1CalculatorParams.LinkedViewElement);
            eAnalyzerType = GetAnalyzerType(sAnalyzerType);
            return eAnalyzerType;
        }
        public ANALYZER_TYPES GetAnalyzerType(string analyzerType)
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            if (analyzerType == ANALYZER_TYPES.sbtotal1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.sbtotal1;
            }
            else if (analyzerType == ANALYZER_TYPES.sbchangeyr.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.sbchangeyr;
            }
            else if (analyzerType == ANALYZER_TYPES.sbchangeid.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.sbchangeid;
            }
            else if (analyzerType == ANALYZER_TYPES.sbchangealt.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.sbchangealt;
            }
            else if (analyzerType == ANALYZER_TYPES.sbprogress1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.sbprogress1;
            }
            else if (analyzerType == ANALYZER_TYPES.sbstat1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.sbstat1;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.SB1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            return eAnalyzerType;
        }

        public static AnalyzerHelper.ANALYZER_GENERAL_TYPES GetBaseAnalyzerType(
            ANALYZER_TYPES analyzerType)
        {
            AnalyzerHelper.ANALYZER_GENERAL_TYPES eAnalyzerGeneralType
                = AnalyzerHelper.ANALYZER_GENERAL_TYPES.none;
            eAnalyzerGeneralType
                   = AnalyzerHelper.ANALYZER_GENERAL_TYPES.basic;

            return eAnalyzerGeneralType;
        }
        public async Task<bool> RunAnalysis()
        {
            bool bHasAnalysis = false;
            bool bHasAggregation = false;
            //must use analyzeobjects in order to get complete collections
            //otherwise ancestor nodes can't be serialized (no id spells trouble)
            this.SB1CalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.SB1CalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.SB1CalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.SB1CalculatorParams.ExtensionDocToCalcURI,
                this.SB1CalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.SB1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            //10 chosen to analyze
            SetStocksToAnalyze();
            if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                IOSB1CalculatorAsync subInputAsync
                    = new IOSB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subInputAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync inputAnalyzer = new BISB1StockAnalyzerAsync(
                        subInputAsync.GCCalculatorParams);
                    inputAnalyzer.InputGroup = subInputAsync.BISB1CalculatorAsync.InputGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await inputAnalyzer.SetInputSB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(inputAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await inputAnalyzer.SaveSB1StockTotalsAsync();
                        inputAnalyzer = null;
                    }
                }
                subInputAsync = null;
            }
            else if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                IOSB1CalculatorAsync subOutputAsync
                    = new IOSB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subOutputAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync outputAnalyzer = new BISB1StockAnalyzerAsync(
                        subOutputAsync.GCCalculatorParams);
                    outputAnalyzer.OutputGroup = subOutputAsync.BISB1CalculatorAsync.OutputGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await outputAnalyzer.SetOutputSB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(outputAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await outputAnalyzer.SaveSB1StockTotalsAsync();
                        outputAnalyzer = null;
                    }
                }
                subOutputAsync = null;
            }
            else if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                //build an object collection
                OCSB1CalculatorAsync subOperationAsync
                    = new OCSB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subOperationAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync opAnalyzer = new BISB1StockAnalyzerAsync(
                        subOperationAsync.GCCalculatorParams);
                    opAnalyzer.OCGroup = subOperationAsync.BISB1CalculatorAsync.OCGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await opAnalyzer.SetOCSB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(opAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await opAnalyzer.SaveSB1StockTotalsAsync();
                        opAnalyzer = null;
                    }
                }
                subOperationAsync = null;
            }
            else if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                //build an object collection
                OCSB1CalculatorAsync subComponentAsync
                    = new OCSB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subComponentAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync compAnalyzer = new BISB1StockAnalyzerAsync(
                        subComponentAsync.GCCalculatorParams);
                    compAnalyzer.OCGroup = subComponentAsync.BISB1CalculatorAsync.OCGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await compAnalyzer.SetOCSB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(compAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await compAnalyzer.SaveSB1StockTotalsAsync();
                        compAnalyzer = null;
                    }
                }
                subComponentAsync = null;
            }
            else if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                OutcomeSB1CalculatorAsync subOutcomeAsync
                        = new OutcomeSB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subOutcomeAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync outcomeAnalyzer = new BISB1StockAnalyzerAsync(
                        subOutcomeAsync.GCCalculatorParams);
                    outcomeAnalyzer.OutcomeGroup = subOutcomeAsync.BISB1CalculatorAsync.OutcomeGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await outcomeAnalyzer.SetOutcomeSB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(outcomeAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await outcomeAnalyzer.SaveSB1StockTotalsAsync();
                        outcomeAnalyzer = null;
                    }
                }
                subOutcomeAsync = null;
            }
            else if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                BISB1CalculatorAsync subBudgetAsync
                    = new BISB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subBudgetAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync biAnalyzer = new BISB1StockAnalyzerAsync(
                        subBudgetAsync.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subBudgetAsync.BISB1CalculatorAsync.BudgetGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await biAnalyzer.SetBISB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(biAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await biAnalyzer.SaveSB1StockTotalsAsync();
                        biAnalyzer = null;
                    }
                }
                subBudgetAsync = null;
            }
            else if (this.SB1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.investments)
            {
                BISB1CalculatorAsync subInvestmentAsync
                    = new BISB1CalculatorAsync(this.SB1CalculatorParams);
                bHasAnalysis = await subInvestmentAsync.RunCalculatorAsync();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BISB1StockAnalyzerAsync biAnalyzer = new BISB1StockAnalyzerAsync(
                        subInvestmentAsync.GCCalculatorParams);
                    //object is budget but xml is investment
                    biAnalyzer.BudgetGroup = subInvestmentAsync.BISB1CalculatorAsync.BudgetGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = await biAnalyzer.SetBISB1StockTotals();
                    if (bHasAggregation)
                    {
                        //188 the analyzer object holds the current node properties to update
                        SetPostAnalysisParameters(biAnalyzer.GCCalculatorParams);
                        //Save the results as xml
                        bHasAnalysis = await biAnalyzer.SaveSB1StockTotalsAsync();
                        biAnalyzer = null;
                    }
                }
                subInvestmentAsync = null;
            }
            if (!bHasAggregation)
            {
                bHasAnalysis = false;
            }
            bool bIsPostAnalysis = true;
            //this is run pre and post analysis because some props (i.e. devpacks with subapptypes) aren't set until analyses are run
            SetAnalysisParameters(bIsPostAnalysis);
            //stylesheet set in analyzerhelper
            return bHasAnalysis;
        }
        public async Task<bool> RunAnalysis(IList<string> urisToAnalyze)
        {
            bool bHasAnalysis = false;
            //set the files needing analysis
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.SB1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.SB1CalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.SB1CalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.SB1CalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.SB1CalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.SB1CalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.SB1CalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = await RunAnalysis();
                        //188: still needed because devpacks can be run without using algos which means RunAnalysis did not update Updates collection
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.SB1CalculatorParams);
                        }
                        //reset subapptype
                        this.SB1CalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.SB1CalculatorParams.ErrorMessage += this.SB1CalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.SB1CalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.SB1CalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.SB1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.SB1CalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.SB1CalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.SB1CalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.SB1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.SB1CalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.SB1CalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.SB1CalculatorParams.LinkedViewElement);
            this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.SB1CalculatorParams.LinkedViewElement);
            this.SB1CalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.SB1CalculatorParams.LinkedViewElement);
            //new in v1.6.5 (user can choose whether or not to display full calcs)
            this.SB1CalculatorParams.NeedsFullView
                = AnalyzerHelper.GetDisplayFullViewOption(this.SB1CalculatorParams.LinkedViewElement);
            this.SB1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            this.SB1CalculatorParams.StartingDocToCalcNodeName
                = GetNodeNameFromFileExtensionType(this.SB1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.SB1CalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.SB1CalculatorParams.StartingDocToCalcNodeName);
        }
        private string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
            if (this.SB1CalculatorParams.StartingDocToCalcNodeName != string.Empty 
                && this.SB1CalculatorParams.StartingDocToCalcNodeName != Constants.NONE)
            {
                sStartingNodeToCalc = this.SB1CalculatorParams.StartingDocToCalcNodeName;
            }
            else
            {
                if (this.SB1CalculatorParams.DocToCalcNodeName != string.Empty
                    && this.SB1CalculatorParams.DocToCalcNodeName != Constants.NONE)
                {
                    sStartingNodeToCalc = this.SB1CalculatorParams.DocToCalcNodeName;
                }
                else
                {
                    if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.input.ToString())
                    {
                        sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.output.ToString())
                    {
                        sStartingNodeToCalc = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.component.ToString())
                    {
                        sStartingNodeToCalc = OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.operation.ToString())
                    {
                        sStartingNodeToCalc = OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.outcome.ToString())
                    {
                        sStartingNodeToCalc = Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString();
                    }
                    else if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
                    {
                        sStartingNodeToCalc = BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
                    {
                        sStartingNodeToCalc = BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
                    {
                        sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
                    {
                        sStartingNodeToCalc = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                    }
                }
            }

            return sStartingNodeToCalc;
        }
        public void SetPostAnalysisParameters(CalculatorParameters calcParams)
        {
            //188 added to handle algos in analyzers
            if (!string.IsNullOrEmpty(calcParams.MathResult)
                && calcParams.MathResult != Constants.NONE)
            {
                //update devpacks will set this in lv
                this.SB1CalculatorParams.MathResult = calcParams.MathResult;
                if (calcParams.LinkedViewElement != null)
                {
                    //this may actually be a byref and redundant, but keep
                    this.SB1CalculatorParams.LinkedViewElement = calcParams.LinkedViewElement;
                }
                //188 made sure its in db lv (devpacks will overwrite this using UpdateDevPackAnalyzerParams)
                CalculatorHelpers.AddXmlDocToUpdates(this.SB1CalculatorParams,
                    Calculator1.cMathResult, calcParams.MathResult);
            }
        }

        //this is called before the analysis is run and updates lv with properties needing db save
        public void SetAnalysisParameters(bool isPostAnalysis)
        {
            //unlike calculators, only the doctocalcuripattern gets updated in db
            //set the file extension for this analysis (so other analyses can find it)
            SetFileExtensionType();
            //set the linkedview params needed to load and display the analysis
            SetLinkedViewParams(isPostAnalysis);
        }
        private void SetFileExtensionType()
        {
            //analyzers find the results of other analyzers using this attribute
            string sFileExtensionType
                = CalculatorHelpers.GetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.SB1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.SB1CalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.SB1CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                //devpacks handle removing this from calcparam.updates 
                CalculatorHelpers.SetFileExtensionType(this.SB1CalculatorParams,
                    sNeededFileExtensionType);
            }
        }
        private string MakeAnalysisFileExtensionType(
            string filesToAnalyzeExtensionType)
        {
            //i.e. filetoanalyzeExttype = budget; analyzertype = stats01, comparisontype = none
            //= budgetstats01none
            string sFileExtensionType = MakeAnalysisFileExtensionTypeForDbUpdate(
                filesToAnalyzeExtensionType);
            return sFileExtensionType;
        }
        private string MakeAnalysisFileExtensionTypeForDbUpdate(
            string filesToAnalyzeExtensionType)
        {
            //version 1.3.5 simplification
            string sFileExtensionType = string.Concat(filesToAnalyzeExtensionType,
               this.AnalyzerType.ToString());
            ////i.e. filetoanalyzeExttype = budget; analyzertype = stats01, comparisontype = none
            ////= budgetstats01none
            return sFileExtensionType;
        }
        //sets linkedview (analysisdoc) props needed by client to load and display doc
        public void SetLinkedViewParams(bool isPostAnalysis)
        {
            string sStylesheetName = GetAnalysisStyleSheet();
            //sStylesheetName will be used to find the Stylesheet and, if its a first time view, 
            //to set two more params: StylesheetResourceURIPattern and StylesheetDocPath 
            string sExistingStylesheetName
                = CalculatorHelpers.GetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.SB1CalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if (!string.IsNullOrEmpty(sStylesheetName))
            {
                if ((string.IsNullOrEmpty(sExistingStylesheetName))
                    || (!sExistingStylesheetName.Equals(sStylesheetName)))
                {
                    CalculatorHelpers.SetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                        Calculator1.cStylesheet2ResourceFileName, sStylesheetName);
                    if (isPostAnalysis)
                    {
                        //188 made sure its in db lv -this is hit under some analyses
                        CalculatorHelpers.AddXmlDocToUpdates(this.SB1CalculatorParams,
                            Calculator1.cStylesheet2ResourceFileName, sStylesheetName);
                    }
                }
            }
            //set the ss's extension object's namespace
            SetStylesheetNamespace(isPostAnalysis);
        }
        private void SetStylesheetNamespace(bool isPostAnalysis)
        {
            string sStylesheetExtObjNamespace
                = GetAnalysisStyleSheetExtObjNamespace();
            string sExistingStylesheetExtObjNamespace
                = CalculatorHelpers.GetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.SB1CalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if (!string.IsNullOrEmpty(sStylesheetExtObjNamespace))
            {
                if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                    || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
                {
                    CalculatorHelpers.SetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                        Calculator1.cStylesheet2ObjectNS,
                        sStylesheetExtObjNamespace);
                    if (isPostAnalysis)
                    {
                        //188 made sure its in db lv
                        CalculatorHelpers.AddXmlDocToUpdates(this.SB1CalculatorParams, Calculator1.cStylesheet2ObjectNS, sStylesheetExtObjNamespace);
                    }
                }
            }
        }
        private string GetAnalysisStyleSheet()
        {
            string sAnalysisStyleSheet = string.Empty;
            if (this.AnalyzerType == ANALYZER_TYPES.none)
            {
                sAnalysisStyleSheet = string.Empty;
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.sbstat1)
            {
                if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsInvests1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsBuds1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsOps1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsComps1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsOutcomes1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsIns1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Stats2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1StatsOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.sbchangeyr
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt)
            {
                if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeInvests1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeBuds1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeOps1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeComps1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeOutcomes1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeIns1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Change2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ChangeOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.sbprogress1)
            {
                if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgInvests1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgBuds1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgOps1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgComps1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgOutcomes1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgIns1.xslt";
                    }
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "SB1Prog2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "SB1ProgOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.sbtotal1)
            {
                if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sAnalysisStyleSheet = "SB1StocksInvests1.xslt";
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sAnalysisStyleSheet = "SB1StocksBuds1.xslt";
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    sAnalysisStyleSheet = "SB1StocksOps1.xslt";
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    sAnalysisStyleSheet = "SB1StocksComps1.xslt";
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    sAnalysisStyleSheet = "SB1StocksOutcomes1.xslt";
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    sAnalysisStyleSheet = "SB1StocksIns1.xslt";
                }
                else if (this.SB1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    sAnalysisStyleSheet = "SB1StocksOuts1.xslt";
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.SB1CalculatorParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                sStylesheetExtObjNamespace = "displaycomps";
            }
            else
            {
                sStylesheetExtObjNamespace = "displaydevpacks";
            }
            return sStylesheetExtObjNamespace;
        }
        
        public static bool IsChangeTypeAnalysis(string analyzerType)
        {
            bool bIsChangeTypeAnalysis = false;
            if (analyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || analyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                || analyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || analyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                bIsChangeTypeAnalysis = true;
            }
            return bIsChangeTypeAnalysis;
        }
        public static bool NeedsAnalyzerIndicator(string indicLabel, CalculatorParameters calcParams)
        {
            //not fully implemented yet
            bool bNeedsInd = true;
            if (calcParams.ExtensionCalcDocURI == null)
            {
                return bNeedsInd;
            }
            if (calcParams.ExtensionCalcDocURI.URIDataManager == null)
            {
                return bNeedsInd;
            }
            if (calcParams.ExtensionCalcDocURI.URIDataManager.HostName.ToLower() ==
                Constants.ANALYZER_HOSTNAME)
            {
                //analyzers can only run up to 15 indicators at one ime
                bNeedsInd = false;
            }
            if (calcParams.AnalyzerParms.AnalyzerType == ANALYZER_TYPES.sbtotal1.ToString())
            {
                bNeedsInd = true;
            }
            if (!bNeedsInd)
            {
                if (calcParams.UrisToAnalyze != null)
                {
                    foreach (var sb in calcParams.UrisToAnalyze)
                    {
                        if (sb == indicLabel)
                        {
                            return true;
                        }
                    }
                }
            }
            return bNeedsInd;
        }
        public void SetStocksToAnalyze()
        {
            if (this.SB1CalculatorParams.UrisToAnalyze == null)
                this.SB1CalculatorParams.UrisToAnalyze = new List<string>();
            if (this.AnalyzerType != ANALYZER_TYPES.sbtotal1)
            {
                //10 stock limit
                int iCount = 10;
                //label aggregators used 
                AddStockToList(SB1Base.cSB1Label1, iCount);
                AddStockToList(SB1Base.cSB1Label2, iCount);
                AddStockToList(SB1Base.cSB1Label3, iCount);
                AddStockToList(SB1Base.cSB1Label4, iCount);
                AddStockToList(SB1Base.cSB1Label5, iCount);
                AddStockToList(SB1Base.cSB1Label6, iCount);
                AddStockToList(SB1Base.cSB1Label7, iCount);
                AddStockToList(SB1Base.cSB1Label8, iCount);
                AddStockToList(SB1Base.cSB1Label9, iCount);
                AddStockToList(SB1Base.cSB1Label10, iCount);
                AddStockToList(SB1Base.cSB1Label11, iCount);
                AddStockToList(SB1Base.cSB1Label12, iCount);
                AddStockToList(SB1Base.cSB1Label13, iCount);
                AddStockToList(SB1Base.cSB1Label14, iCount);
                AddStockToList(SB1Base.cSB1Label15, iCount);
                AddStockToList(SB1Base.cSB1Label16, iCount);
                AddStockToList(SB1Base.cSB1Label17, iCount);
                AddStockToList(SB1Base.cSB1Label18, iCount);
                AddStockToList(SB1Base.cSB1Label19, iCount);
                AddStockToList(SB1Base.cSB1Label20, iCount);
            }
        }
        private void AddStockToList(string stockLabel, int iCount)
        {
            string sValue = CalculatorHelpers.GetAttribute(this.SB1CalculatorParams.LinkedViewElement,
                    stockLabel);
            //checkbox attributes are dynamically set in js as true or false
            if (sValue == "true")
            {
                if (this.SB1CalculatorParams.UrisToAnalyze.Count <= iCount)
                    this.SB1CalculatorParams.UrisToAnalyze.Add(stockLabel);
            }
        }
        public static void AddCalculators(int alternative2, List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in calcs)
                {
                    calc.Alternative2 = alternative2;
                    newcalcs.Add(calc);
                }
            }
        }
        public static void AddCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in calcs)
                {
                    newcalcs.Add(calc);
                }
            }
        }
        public static void AddInputObsCalculators(int alternative2, List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.CalculatorType
                        == SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
                    {
                        SBC1Calculator sbc = new SBC1Calculator();
                        if (calc.GetType().Equals(sbc.GetType()))
                        {
                            SBC1Calculator sbcInput = (SBC1Calculator)calc;
                            sbc.CopySB1C1Properties(sbcInput);
                            //which observation?
                            sbc.Alternative2 = alternative2;
                            newcalcs.Add(sbc);
                        }
                    }
                }
            }
        }
        public static void AddInputCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.CalculatorType
                        == SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
                    {
                        SBC1Calculator sbc = new SBC1Calculator();
                        if (calc.GetType().Equals(sbc.GetType()))
                        {
                            SBC1Calculator sbcInput = (SBC1Calculator)calc;
                            sbc.CopySB1C1Properties(sbcInput);
                            newcalcs.Add(sbc);
                        }
                    }
                }
            }
        }
        
        
        public static void AddOutputObsCalculators(int alternative2, List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.CalculatorType
                        == SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
                    {
                        SBB1Calculator sbb = new SBB1Calculator();
                        if (calc.GetType().Equals(sbb.GetType()))
                        {
                            SBB1Calculator sbbOutput = (SBB1Calculator)calc;
                            sbb.CopySB1B1Properties(sbbOutput);
                            //which observation is it in?
                            sbb.Alternative2 = alternative2;
                            newcalcs.Add(sbb);
                        }
                    }
                }
            }
        }
        public static void AddOutputCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.CalculatorType
                        == SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
                    {
                        SBB1Calculator sbb = new SBB1Calculator();
                        if (calc.GetType().Equals(sbb.GetType()))
                        {
                            SBB1Calculator sbbOutput = (SBB1Calculator)calc;
                            sbb.CopySB1B1Properties(sbbOutput);
                            newcalcs.Add(sbb);
                        }
                    }
                }
            }
        }
        public static void InitCalculatorMath(SB1Stock sb1StockMember)
        {
            sb1StockMember.MathResult = string.Empty;
        }
        public static void AddCalculatorDataToStock(List<Calculator1> calcs, SB1Stock sb1StockMember)
        {
            foreach (var c in calcs)
            {
                if (c.DataToAnalyze != null)
                {
                    if (c.DataToAnalyze.Count > 0)
                    {
                        sb1StockMember.CopyData(c.DataToAnalyze);
                        //convention in SB1Base.ProcessAlgosforAnalyzer is to use 1st calc for column names
                        if (string.IsNullOrEmpty(sb1StockMember.DataColNames))
                        {
                            sb1StockMember.DataColNames = c.DataColNames;
                        }
                    }
                }
            }
        }
    }
}
