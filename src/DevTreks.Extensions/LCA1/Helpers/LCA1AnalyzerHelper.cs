using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Helper functions for the resource stocks analyzer extension
    ///Author:		www.devtreks.org
    ///Date:		2014, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///             1. Other modules demonstrate how to implement the analyzer patterns. 
    /// </summary>
    public class LCA1AnalyzerHelper
    {
        //constructor
        public LCA1AnalyzerHelper() { }
        public LCA1AnalyzerHelper(CalculatorParameters calcParameters)
        {
            this.LCA1CalculatorParams = calcParameters;
        }

        //properties
        //parameters needed by publishers
        public CalculatorParameters LCA1CalculatorParams { get; set; }
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }

        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none                    = 0,
            //totals (i.e. total fat used by an operation's food input)
            lcatotal1               = 1,
            //stats
            lcastat1                = 2,
            //change by year
            lcachangeyr             = 3,
            //change by id
            lcachangeid             = 4,
            //change by alternativetype
            lcachangealt            = 5,
            //progress   
            lcaprogress1            = 6,
            //unit cost (cost per unit output)
            lcaeff01a1              = 7,
            //unit cost (cost per dollar revenue)
            lcaeff01b               = 8,
            //unit resource (resource per unit output)
            lcaeff02a               = 9,
            //unit resource (resource per dollar revenue)
            lcaeff02b               = 10
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.LCA1CalculatorParams.LinkedViewElement);
            eAnalyzerType = GetAnalyzerType(sAnalyzerType);
            return eAnalyzerType;
        }
        public ANALYZER_TYPES GetAnalyzerType(string analyzerType)
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            if (analyzerType == ANALYZER_TYPES.lcatotal1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.lcatotal1;
            }
            else if (analyzerType == ANALYZER_TYPES.lcachangeyr.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.lcachangeyr;
            }
            else if (analyzerType == ANALYZER_TYPES.lcachangeid.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.lcachangeid;
            }
            else if (analyzerType == ANALYZER_TYPES.lcachangealt.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.lcachangealt;
            }
            else if (analyzerType == ANALYZER_TYPES.lcaprogress1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.lcaprogress1;
            }
            else if (analyzerType == ANALYZER_TYPES.lcastat1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.lcastat1;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.LCA1CalculatorParams.ErrorMessage
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
        public bool RunAnalysis()
        {
            bool bHasAnalysis = false;
            bool bHasAggregation = false;
            //must use analyzeobjects in order to get complete collections
            //otherwise ancestor nodes can't be serialized (no id spells trouble)
            this.LCA1CalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.LCA1CalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.LCA1CalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.LCA1CalculatorParams.ExtensionDocToCalcURI,
                this.LCA1CalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.LCA1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                IOLCA1StockSubscriber subInput
                    = new IOLCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subInput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer inputAnalyzer = new BILCA1StockAnalyzer(
                        subInput.GCCalculatorParams);
                    inputAnalyzer.InputGroup = subInput.LCA1StockCalculator.BILCA1Calculator.InputGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = inputAnalyzer.SetInputLCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = inputAnalyzer.SaveLCA1StockTotals();
                        inputAnalyzer = null;
                    }
                }
                subInput = null;
            }
            else if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                IOLCA1StockSubscriber subOutput
                    = new IOLCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subOutput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer outputAnalyzer = new BILCA1StockAnalyzer(
                        subOutput.GCCalculatorParams);
                    outputAnalyzer.OutputGroup = subOutput.LCA1StockCalculator.BILCA1Calculator.OutputGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outputAnalyzer.SetOutputLCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outputAnalyzer.SaveLCA1StockTotals();
                        outputAnalyzer = null;
                    }
                }
                subOutput = null;
            }
            else if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                //build an object collection
                OCLCA1StockSubscriber subOperation
                    = new OCLCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subOperation.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer opAnalyzer = new BILCA1StockAnalyzer(
                        subOperation.GCCalculatorParams);
                    opAnalyzer.OCGroup = subOperation.LCA1StockCalculator.BILCA1Calculator.OCGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = opAnalyzer.SetOCLCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = opAnalyzer.SaveLCA1StockTotals();
                        opAnalyzer = null;
                    }
                }
                subOperation = null;
            }
            else if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                //build an object collection
                OCLCA1StockSubscriber subComponent
                    = new OCLCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subComponent.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer compAnalyzer = new BILCA1StockAnalyzer(
                        subComponent.GCCalculatorParams);
                    compAnalyzer.OCGroup = subComponent.LCA1StockCalculator.BILCA1Calculator.OCGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = compAnalyzer.SetOCLCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = compAnalyzer.SaveLCA1StockTotals();
                        compAnalyzer = null;
                    }
                }
                subComponent = null;
            }
            else if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                OutcomeLCA1StockSubscriber subOutcome
                        = new OutcomeLCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subOutcome.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer outcomeAnalyzer = new BILCA1StockAnalyzer(
                        subOutcome.GCCalculatorParams);
                    outcomeAnalyzer.OutcomeGroup = subOutcome.LCA1StockCalculator.BILCA1Calculator.OutcomeGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outcomeAnalyzer.SetOutcomeLCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outcomeAnalyzer.SaveLCA1StockTotals();
                        outcomeAnalyzer = null;
                    }
                }
                subOutcome = null;
            }
            else if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                BILCA1StockSubscriber subBudget
                    = new BILCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subBudget.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer biAnalyzer = new BILCA1StockAnalyzer(
                        subBudget.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subBudget.LCA1StockCalculator.BudgetGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBILCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveLCA1StockTotals();
                        biAnalyzer = null;
                    }
                }
                subBudget = null;
            }
            else if (this.LCA1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.investments)
            {
                BILCA1StockSubscriber subInvestment
                    = new BILCA1StockSubscriber(this.LCA1CalculatorParams);
                bHasAnalysis = subInvestment.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BILCA1StockAnalyzer biAnalyzer = new BILCA1StockAnalyzer(
                        subInvestment.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subInvestment.LCA1StockCalculator.BudgetGroup;
                    //2. Aggregate the base LCA collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBILCA1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveLCA1StockTotals();
                        biAnalyzer = null;
                    }
                }
                subInvestment = null;
            }
            if (!bHasAggregation)
            {
                bHasAnalysis = false;
            }
            //set the db parameters needed for saving
            SetAnalysisParameters();
            //stylesheet set in analyzerhelper
            return bHasAnalysis;
        }
        public bool RunAnalysis(IList<string> urisToAnalyze)
        {
            bool bHasAnalysis = false;
            //set the files needing analysis
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.LCA1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.LCA1CalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.LCA1CalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.LCA1CalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.LCA1CalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.LCA1CalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.LCA1CalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = RunAnalysis();
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.LCA1CalculatorParams);
                        }
                        //reset subapptype
                        this.LCA1CalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.LCA1CalculatorParams.ErrorMessage += this.LCA1CalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.LCA1CalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.LCA1CalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.LCA1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.LCA1CalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.LCA1CalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.LCA1CalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.LCA1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.LCA1CalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.LCA1CalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.LCA1CalculatorParams.LinkedViewElement);
            this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.LCA1CalculatorParams.LinkedViewElement);
            this.LCA1CalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.LCA1CalculatorParams.LinkedViewElement);
            ////these analyzers do not use default labels aggregation
            //if (IsChangeTypeAnalysis(this.AnalyzerType.ToString())
            //    || this.AnalyzerType == ANALYZER_TYPES.lcatotal1)
            //{
            //    //version upgrades won't update types if they are no longer in stylesheet
            //    this.LCA1AnalyzerParams.AggregationType = AnalyzerHelper.AGGREGATION_OPTIONS.none;
            //}
            this.LCA1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.LCA1CalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            //version 1.3.7 stopped this pattern so that desc analyzers can be updated easily
            //this.LCA1CalculatorParams.CalculatorType
            //    = this.LCA1AnalyzerParams.FilesToAnalyzeExtensionType;
            this.LCA1CalculatorParams.StartingDocToCalcNodeName
                = GetNodeNameFromFileExtensionType(this.LCA1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.LCA1CalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.LCA1CalculatorParams.StartingDocToCalcNodeName);
        }
        private string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
            if (this.LCA1CalculatorParams.StartingDocToCalcNodeName != string.Empty 
                && this.LCA1CalculatorParams.StartingDocToCalcNodeName != Constants.NONE)
            {
                sStartingNodeToCalc = this.LCA1CalculatorParams.StartingDocToCalcNodeName;
            }
            else
            {
                if (this.LCA1CalculatorParams.DocToCalcNodeName != string.Empty
                    && this.LCA1CalculatorParams.DocToCalcNodeName != Constants.NONE)
                {
                    sStartingNodeToCalc = this.LCA1CalculatorParams.DocToCalcNodeName;
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
                        LCA1CalculatorHelper.CALCULATOR_TYPES.buildcost1.ToString())
                    {
                        sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        LCA1CalculatorHelper.CALCULATOR_TYPES.buildbenefit1.ToString())
                    {
                        sStartingNodeToCalc = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
                    }
                }
            }

            return sStartingNodeToCalc;
        }
        public void SetAnalysisParameters()
        {
            //unlike calculators, only the doctocalcuripattern gets updated in db
            //set the file extension for this analysis (so other analyses can find it)
            SetFileExtensionType();
            //set the linkedview params needed to load and display the analysis
            SetLinkedViewParams();
        }
        private void SetFileExtensionType()
        {
            //analyzers find the results of other analyzers using this attribute
            string sFileExtensionType
                = CalculatorHelpers.GetAttribute(this.LCA1CalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.LCA1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.LCA1CalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.LCA1CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(this.LCA1CalculatorParams,
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
        public void SetLinkedViewParams()
        {
            string sStylesheetName = GetAnalysisStyleSheet();
            //sStylesheetName will be used to find the Stylesheet and, if its a first time view, 
            //to set two more params: StylesheetResourceURIPattern and StylesheetDocPath 
            string sExistingStylesheetName
                = CalculatorHelpers.GetAttribute(this.LCA1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.LCA1CalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.LCA1CalculatorParams.LinkedViewElement,
                    Calculator1.cStylesheet2ResourceFileName, sStylesheetName);
            }
            //set the ss's extension object's namespace
            SetStylesheetNamespace();
        }
        private void SetStylesheetNamespace()
        {
            string sStylesheetExtObjNamespace
                = GetAnalysisStyleSheetExtObjNamespace();
            string sExistingStylesheetExtObjNamespace
                = CalculatorHelpers.GetAttribute(this.LCA1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.LCA1CalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.LCA1CalculatorParams.LinkedViewElement,
                    Calculator1.cStylesheet2ObjectNS,
                    sStylesheetExtObjNamespace);
            }
        }
        private string GetAnalysisStyleSheet()
        {
            string sAnalysisStyleSheet = string.Empty;
            if (this.AnalyzerType == ANALYZER_TYPES.none)
            {
                sAnalysisStyleSheet = string.Empty;
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.lcastat1)
            {
                if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsInvests1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsBuds1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsOps1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsComps1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsOutcomes1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsIns1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Stats2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1StatsOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.lcachangeyr
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt)
            {
                if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeInvests1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeBuds1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeOps1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeComps1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeOutcomes1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeIns1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Change2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ChangeOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.lcaprogress1)
            {
                if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgInvests1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgBuds1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgOps1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgComps1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgOutcomes1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgIns1.xslt";
                    }
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "LC1Prog2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "LC1ProgOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.lcatotal1)
            {
                if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sAnalysisStyleSheet = "LC1StocksInvests1.xslt";
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sAnalysisStyleSheet = "LC1StocksBuds1.xslt";
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    sAnalysisStyleSheet = "LC1StocksOps1.xslt";
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    sAnalysisStyleSheet = "LC1StocksComps1.xslt";
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    sAnalysisStyleSheet = "LC1StocksOutcomes1.xslt";
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    sAnalysisStyleSheet = "LC1StocksIns1.xslt";
                }
                else if (this.LCA1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    sAnalysisStyleSheet = "LC1StocksOuts1.xslt";
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.LCA1CalculatorParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                sStylesheetExtObjNamespace = "displaycomps";
            }
            else
            {
                sStylesheetExtObjNamespace = "displaydevpacks";
                if (IsEffectivenessAnalysis(
                    this.AnalyzerType))
                {
                    sStylesheetExtObjNamespace = "displaycomps";
                }
            }
            return sStylesheetExtObjNamespace;
        }
        public static bool IsEffectivenessAnalysis(
            ANALYZER_TYPES analyzerType)
        {
            bool bIsEffectivenessAnalysis = false;
            if (analyzerType
               == ANALYZER_TYPES.lcaeff01a1
                || analyzerType
               == ANALYZER_TYPES.lcaeff01b
                || analyzerType
               == ANALYZER_TYPES.lcaeff02a
                || analyzerType
               == ANALYZER_TYPES.lcaeff02b)
            {
                bIsEffectivenessAnalysis = true;
            }
            return bIsEffectivenessAnalysis;
        }
        public static bool IsChangeTypeAnalysis(string analyzerType)
        {
            bool bIsChangeTypeAnalysis = false;
            if (analyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || analyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString()
                || analyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || analyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                bIsChangeTypeAnalysis = true;
            }
            return bIsChangeTypeAnalysis;
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
                        == LCA1CalculatorHelper.CALCULATOR_TYPES.buildcost1.ToString())
                    {
                        LCC1Calculator lcc = new LCC1Calculator();
                        if (calc.GetType().Equals(lcc.GetType()))
                        {
                            LCC1Calculator lccInput = (LCC1Calculator)calc;
                            lcc.CopyLCC1Properties(lccInput);
                            //which observation?
                            lcc.Alternative2 = alternative2;
                            newcalcs.Add(lcc);
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
                        == LCA1CalculatorHelper.CALCULATOR_TYPES.buildcost1.ToString())
                    {
                        LCC1Calculator lcc = new LCC1Calculator();
                        if (calc.GetType().Equals(lcc.GetType()))
                        {
                            LCC1Calculator lccInput = (LCC1Calculator)calc;
                            lcc.CopyLCC1Properties(lccInput);
                            newcalcs.Add(lcc);
                        }
                    }
                }
            }
        }
        public static void AddInputCalculators(List<SubPrice1> calcs, List<SubPrice1> newcalcs)
        {
            if (calcs != null)
            {
                foreach (Calculator1 calc in calcs)
                {
                    AddInputCalculator(calc, newcalcs);
                }
            }
        }
        public static void AddInputCalculators(List<Calculator1> calcs, List<SubPrice1> newcalcs)
        {
            if (calcs != null)
            {
                foreach (Calculator1 calc in calcs)
                {
                    AddInputCalculator(calc, newcalcs);
                }
            }
        }
        public static void AddInputCalculator(Calculator1 calc, List<SubPrice1> newcalcs)
        {
            if (calc != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<SubPrice1>();
                if (calc.CalculatorType
                    == LCA1CalculatorHelper.CALCULATOR_TYPES.buildcost1.ToString())
                {
                    LCC1Calculator lcc = new LCC1Calculator();
                    if (calc.GetType().Equals(lcc.GetType()))
                    {
                        LCC1Calculator lccInput = (LCC1Calculator)calc;
                        lcc.CopyLCC1Properties(lccInput);
                        newcalcs.Add(lcc);
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
                        == LCA1CalculatorHelper.CALCULATOR_TYPES.buildbenefit1.ToString())
                    {
                        LCB1Calculator lcb = new LCB1Calculator();
                        if (calc.GetType().Equals(lcb.GetType()))
                        {
                            LCB1Calculator lcbOutput = (LCB1Calculator)calc;
                            lcb.CopyLCB1Properties(lcbOutput);
                            //which observation is it in?
                            lcb.Alternative2 = alternative2;
                            newcalcs.Add(lcb);
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
                        == LCA1CalculatorHelper.CALCULATOR_TYPES.buildbenefit1.ToString())
                    {
                        LCB1Calculator lcb = new LCB1Calculator();
                        if (calc.GetType().Equals(lcb.GetType()))
                        {
                            LCB1Calculator lcbOutput = (LCB1Calculator)calc;
                            lcb.CopyLCB1Properties(lcbOutput);
                            newcalcs.Add(lcb);
                        }
                    }
                }
            }
        }
        public static void AddOutputCalculators(List<SubPrice1> calcs, List<SubPrice1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<SubPrice1>();
                foreach (Calculator1 calc in calcs)
                {
                    AddOutputCalculator(calc, newcalcs);
                }
            }
        }
        public static void AddOutputCalculators(List<Calculator1> calcs, List<SubPrice1> newcalcs)
        {
            if (calcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<SubPrice1>();
                foreach (Calculator1 calc in calcs)
                {
                    AddOutputCalculator(calc, newcalcs);
                }
            }
        }
        public static void AddOutputCalculator(Calculator1 calc, List<SubPrice1> newcalcs)
        {
            if (calc != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<SubPrice1>();
                if (calc.CalculatorType
                    == LCA1CalculatorHelper.CALCULATOR_TYPES.buildbenefit1.ToString())
                {
                    LCB1Calculator lcb = new LCB1Calculator();
                    if (calc.GetType().Equals(lcb.GetType()))
                    {
                        LCB1Calculator lcbOutput = (LCB1Calculator)calc;
                        lcb.CopyLCB1Properties(lcbOutput);
                        newcalcs.Add(lcb);
                    }
                }
            }
        }
    }
}
