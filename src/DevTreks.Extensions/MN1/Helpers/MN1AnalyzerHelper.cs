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
    ///Date:		2014, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///             1. Other modules demonstrate how to implement the analyzer patterns. 
    /// </summary>
    public class MN1AnalyzerHelper
    {
        //constructor
        public MN1AnalyzerHelper() { }
        public MN1AnalyzerHelper(CalculatorParameters calcParameters)
        {
            this.MN1CalculatorParams = calcParameters;
        }

        //properties
        //parameters needed by publishers
        public CalculatorParameters MN1CalculatorParams { get; set; }
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }

        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none                    = 0,
            //totals (i.e. total fat used by an operation's food input)
            mntotal1               = 1,
            //stats
            mnstat1                = 2,
            //change by year
            mnchangeyr             = 3,
            //change by id
            mnchangeid             = 4,
            //change by alternativetype
            mnchangealt            = 5,
            //progress   
            mnprogress1            = 6
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.MN1CalculatorParams.LinkedViewElement);
            eAnalyzerType = GetAnalyzerType(sAnalyzerType);
            return eAnalyzerType;
        }
        public ANALYZER_TYPES GetAnalyzerType(string analyzerType)
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            if (analyzerType == ANALYZER_TYPES.mntotal1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mntotal1;
            }
            else if (analyzerType == ANALYZER_TYPES.mnchangeyr.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mnchangeyr;
            }
            else if (analyzerType == ANALYZER_TYPES.mnchangeid.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mnchangeid;
            }
            else if (analyzerType == ANALYZER_TYPES.mnchangealt.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mnchangealt;
            }
            else if (analyzerType == ANALYZER_TYPES.mnprogress1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mnprogress1;
            }
            else if (analyzerType == ANALYZER_TYPES.mnstat1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mnstat1;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.MN1CalculatorParams.ErrorMessage
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
            this.MN1CalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.MN1CalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.MN1CalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.MN1CalculatorParams.ExtensionDocToCalcURI,
                this.MN1CalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.MN1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            //10 chosen to analyze
            SetNutrientsToAnalyze();
            if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                IOMN1StockSubscriber subInput
                    = new IOMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subInput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer inputAnalyzer = new BIMN1StockAnalyzer(
                        subInput.GCCalculatorParams);
                    inputAnalyzer.InputGroup = subInput.MN1StockCalculator.BIMN1Calculator.InputGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = inputAnalyzer.SetInputMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = inputAnalyzer.SaveMN1StockTotals();
                        inputAnalyzer = null;
                    }
                }
                subInput = null;
            }
            else if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                IOMN1StockSubscriber subOutput
                    = new IOMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subOutput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer outputAnalyzer = new BIMN1StockAnalyzer(
                        subOutput.GCCalculatorParams);
                    outputAnalyzer.OutputGroup = subOutput.MN1StockCalculator.BIMN1Calculator.OutputGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outputAnalyzer.SetOutputMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outputAnalyzer.SaveMN1StockTotals();
                        outputAnalyzer = null;
                    }
                }
                subOutput = null;
            }
            else if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                //build an object collection
                OCMN1StockSubscriber subOperation
                    = new OCMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subOperation.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer opAnalyzer = new BIMN1StockAnalyzer(
                        subOperation.GCCalculatorParams);
                    opAnalyzer.OCGroup = subOperation.MN1StockCalculator.BIMN1Calculator.OCGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = opAnalyzer.SetOCMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = opAnalyzer.SaveMN1StockTotals();
                        opAnalyzer = null;
                    }
                }
                subOperation = null;
            }
            else if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                //build an object collection
                OCMN1StockSubscriber subComponent
                    = new OCMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subComponent.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer compAnalyzer = new BIMN1StockAnalyzer(
                        subComponent.GCCalculatorParams);
                    compAnalyzer.OCGroup = subComponent.MN1StockCalculator.BIMN1Calculator.OCGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = compAnalyzer.SetOCMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = compAnalyzer.SaveMN1StockTotals();
                        compAnalyzer = null;
                    }
                }
                subComponent = null;
            }
            else if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                OutcomeMN1StockSubscriber subOutcome
                        = new OutcomeMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subOutcome.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer outcomeAnalyzer = new BIMN1StockAnalyzer(
                        subOutcome.GCCalculatorParams);
                    outcomeAnalyzer.OutcomeGroup = subOutcome.MN1StockCalculator.BIMN1Calculator.OutcomeGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outcomeAnalyzer.SetOutcomeMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outcomeAnalyzer.SaveMN1StockTotals();
                        outcomeAnalyzer = null;
                    }
                }
                subOutcome = null;
            }
            else if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                BIMN1StockSubscriber subBudget
                    = new BIMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subBudget.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer biAnalyzer = new BIMN1StockAnalyzer(
                        subBudget.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subBudget.MN1StockCalculator.BudgetGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBIMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveMN1StockTotals();
                        biAnalyzer = null;
                    }
                }
                subBudget = null;
            }
            else if (this.MN1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.investments)
            {
                BIMN1StockSubscriber subInvestment
                    = new BIMN1StockSubscriber(this.MN1CalculatorParams);
                bHasAnalysis = subInvestment.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIMN1StockAnalyzer biAnalyzer = new BIMN1StockAnalyzer(
                        subInvestment.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subInvestment.MN1StockCalculator.BudgetGroup;
                    //2. Aggregate the base MN collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBIMN1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveMN1StockTotals();
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
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.MN1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.MN1CalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.MN1CalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.MN1CalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.MN1CalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.MN1CalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.MN1CalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = RunAnalysis();
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.MN1CalculatorParams);
                        }
                        //reset subapptype
                        this.MN1CalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.MN1CalculatorParams.ErrorMessage += this.MN1CalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.MN1CalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.MN1CalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.MN1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.MN1CalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.MN1CalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.MN1CalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.MN1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.MN1CalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.MN1CalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.MN1CalculatorParams.LinkedViewElement);
            this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.MN1CalculatorParams.LinkedViewElement);
            this.MN1CalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.MN1CalculatorParams.LinkedViewElement);
            //new in v1.6.5 (user can choose whether or not to display full calcs)
            this.MN1CalculatorParams.NeedsFullView
                = AnalyzerHelper.GetDisplayFullViewOption(this.MN1CalculatorParams.LinkedViewElement);
            ////these analyzers do not use default labels aggregation
            //if (IsChangeTypeAnalysis(this.AnalyzerType.ToString())
            //    || this.AnalyzerType == ANALYZER_TYPES.mntotal1)
            //{
            //    //version upgrades won't update types if they are no longer in stylesheet
            //    this.MN1AnalyzerParams.AggregationType = AnalyzerHelper.AGGREGATION_OPTIONS.none;
            //}
            this.MN1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.MN1CalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            //version 1.3.7 stopped this pattern so that desc analyzers can be updated easily
            //this.MN1CalculatorParams.CalculatorType
            //    = this.MN1AnalyzerParams.FilesToAnalyzeExtensionType;
            this.MN1CalculatorParams.StartingDocToCalcNodeName
                = GetNodeNameFromFileExtensionType(this.MN1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.MN1CalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.MN1CalculatorParams.StartingDocToCalcNodeName);
        }
        private string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
            if (this.MN1CalculatorParams.StartingDocToCalcNodeName != string.Empty 
                && this.MN1CalculatorParams.StartingDocToCalcNodeName != Constants.NONE)
            {
                sStartingNodeToCalc = this.MN1CalculatorParams.StartingDocToCalcNodeName;
            }
            else
            {
                if (this.MN1CalculatorParams.DocToCalcNodeName != string.Empty
                    && this.MN1CalculatorParams.DocToCalcNodeName != Constants.NONE)
                {
                    sStartingNodeToCalc = this.MN1CalculatorParams.DocToCalcNodeName;
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
                        MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                    {
                        sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
                    }
                    else if (fileExtensionType ==
                        MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02.ToString())
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
                = CalculatorHelpers.GetAttribute(this.MN1CalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.MN1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.MN1CalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.MN1CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(this.MN1CalculatorParams,
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
                = CalculatorHelpers.GetAttribute(this.MN1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.MN1CalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.MN1CalculatorParams.LinkedViewElement,
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
                = CalculatorHelpers.GetAttribute(this.MN1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.MN1CalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.MN1CalculatorParams.LinkedViewElement,
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
            else if (this.AnalyzerType == ANALYZER_TYPES.mnstat1)
            {
                if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsInvests1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsBuds1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsOps1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsComps1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsOutcomes1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsIns1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Stats2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1StatsOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.mnchangeyr
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt)
            {
                if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeInvests1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeBuds1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeOps1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeComps1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeOutcomes1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeIns1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Change2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ChangeOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.mnprogress1)
            {
                if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgInvests1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgBuds1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgOps1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgComps1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgOutcomes1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgIns1.xslt";
                    }
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "MN1Prog2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "MN1ProgOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.mntotal1)
            {
                if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sAnalysisStyleSheet = "MN1StocksInvests1.xslt";
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sAnalysisStyleSheet = "MN1StocksBuds1.xslt";
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    sAnalysisStyleSheet = "MN1StocksOps1.xslt";
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    sAnalysisStyleSheet = "MN1StocksComps1.xslt";
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    sAnalysisStyleSheet = "MN1StocksOutcomes1.xslt";
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    sAnalysisStyleSheet = "MN1StocksIns1.xslt";
                }
                else if (this.MN1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    sAnalysisStyleSheet = "MN1StocksOuts1.xslt";
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.MN1CalculatorParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                sStylesheetExtObjNamespace = "displaycomps";
            }
            else
            {
                sStylesheetExtObjNamespace = "displaydevpacks";
                //if (IsEffectivenessAnalysis(
                //    this.AnalyzerType))
                //{
                //    sStylesheetExtObjNamespace = "displaycomps";
                //}
            }
            return sStylesheetExtObjNamespace;
        }
        
        public static bool IsChangeTypeAnalysis(string analyzerType)
        {
            bool bIsChangeTypeAnalysis = false;
            if (analyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || analyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                || analyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || analyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                bIsChangeTypeAnalysis = true;
            }
            return bIsChangeTypeAnalysis;
        }
        public void SetNutrientsToAnalyze()
        {
            if (this.MN1CalculatorParams.UrisToAnalyze == null)
                this.MN1CalculatorParams.UrisToAnalyze = new List<string>();
            if (this.AnalyzerType != ANALYZER_TYPES.mntotal1)
            {
                //10 nutrient limit
                int iCount = this.MN1CalculatorParams.UrisToAnalyze.Count + 10;
                AddNutrientToList(MNSR1.cContainerPrice, iCount);
                AddNutrientToList(MNSR1.cContainerSizeInSSUnits, iCount);
                AddNutrientToList(MNSR1.cServingCost, iCount);
                AddNutrientToList(MNSR1.cActualServingSize, iCount);
                AddNutrientToList(MNSR1.cTypicalServingSize, iCount);
                AddNutrientToList(MNSR1.cTypicalServingsPerContainer, iCount);
                AddNutrientToList(MNSR1.cActualServingsPerContainer, iCount);
                AddNutrientToList(MNSR1.cWater_g, iCount);
                AddNutrientToList(MNSR1.cEnerg_Kcal, iCount);
                AddNutrientToList(MNSR1.cProtein_g, iCount);
                AddNutrientToList(MNSR1.cLipid_Tot_g, iCount);
                AddNutrientToList(MNSR1.cAsh_g, iCount);
                AddNutrientToList(MNSR1.cCarbohydrt_g, iCount);
                AddNutrientToList(MNSR1.cFiber_TD_g, iCount);
                AddNutrientToList(MNSR1.cSugar_Tot_g, iCount);
                AddNutrientToList(MNSR1.cCalcium_mg, iCount);
                AddNutrientToList(MNSR1.cIron_mg, iCount);
                AddNutrientToList(MNSR1.cMagnesium_mg, iCount);
                AddNutrientToList(MNSR1.cPhosphorus_mg, iCount);
                AddNutrientToList(MNSR1.cPotassium_mg, iCount);
                AddNutrientToList(MNSR1.cSodium_mg, iCount);
                AddNutrientToList(MNSR1.cZinc_mg, iCount);
                AddNutrientToList(MNSR1.cCopper_mg, iCount);
                AddNutrientToList(MNSR1.cManganese_mg, iCount);
                AddNutrientToList(MNSR1.cSelenium_pg, iCount);
                AddNutrientToList(MNSR1.cVit_C_mg, iCount);
                AddNutrientToList(MNSR1.cThiamin_mg, iCount);
                AddNutrientToList(MNSR1.cRiboflavin_mg, iCount);
                AddNutrientToList(MNSR1.cNiacin_mg, iCount);
                AddNutrientToList(MNSR1.cPanto_Acid_mg, iCount);
                AddNutrientToList(MNSR1.cVit_B6_mg, iCount);
                AddNutrientToList(MNSR1.cFolate_Tot_pg, iCount);
                AddNutrientToList(MNSR1.cFolic_Acid_pg, iCount);
                AddNutrientToList(MNSR1.cFood_Folate_pg, iCount);
                AddNutrientToList(MNSR1.cFolate_DFE_pg, iCount);
                AddNutrientToList(MNSR1.cCholine_Tot_mg, iCount);
                AddNutrientToList(MNSR1.cVit_B12_pg, iCount);
                AddNutrientToList(MNSR1.cVit_A_IU, iCount);
                AddNutrientToList(MNSR1.cVit_A_RAE, iCount);
                AddNutrientToList(MNSR1.cRetinol_pg, iCount);
                AddNutrientToList(MNSR1.cAlpha_Carot_pg, iCount);
                AddNutrientToList(MNSR1.cBeta_Carot_pg, iCount);
                AddNutrientToList(MNSR1.cBeta_Crypt_pg, iCount);
                AddNutrientToList(MNSR1.cLycopene_pg, iCount);
                AddNutrientToList(MNSR1.cLut_Zea_pg, iCount);
                AddNutrientToList(MNSR1.cVit_E_mg, iCount);
                AddNutrientToList(MNSR1.cVit_D_pg, iCount);
                AddNutrientToList(MNSR1.cViVit_D_IU, iCount);
                AddNutrientToList(MNSR1.cVit_K_pg, iCount);
                AddNutrientToList(MNSR1.cFA_Sat_g, iCount);
                AddNutrientToList(MNSR1.cFA_Mono_g, iCount);
                AddNutrientToList(MNSR1.cFA_Poly_g, iCount);
                AddNutrientToList(MNSR1.cCholestrl_mg, iCount);
                AddNutrientToList(MNSR1.cExtra1, iCount);
                AddNutrientToList(MNSR1.cExtra2, iCount);
            }
        }
        private void AddNutrientToList(string nutName, int iCount)
        {
            string sValue = CalculatorHelpers.GetAttribute(this.MN1CalculatorParams.LinkedViewElement,
                    nutName);
            //checkbox attributes are dynamically set in js as true or false
            if (sValue == "true")
            {
                if (this.MN1CalculatorParams.UrisToAnalyze.Count <= iCount)
                    this.MN1CalculatorParams.UrisToAnalyze.Add(nutName);
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
                        == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                    {
                        MNC1Calculator mnc = new MNC1Calculator();
                        if (calc.GetType().Equals(mnc.GetType()))
                        {
                            MNC1Calculator mncInput = (MNC1Calculator)calc;
                            mnc.CopyMNC1Properties(mncInput);
                            //which observation?
                            mnc.Alternative2 = alternative2;
                            newcalcs.Add(mnc);
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
                        == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                    {
                        MNC1Calculator mnc = new MNC1Calculator();
                        if (calc.GetType().Equals(mnc.GetType()))
                        {
                            MNC1Calculator mncInput = (MNC1Calculator)calc;
                            mnc.CopyMNC1Properties(mncInput);
                            newcalcs.Add(mnc);
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
                        == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02.ToString())
                    {
                        MNB1Calculator mnb = new MNB1Calculator();
                        if (calc.GetType().Equals(mnb.GetType()))
                        {
                            MNB1Calculator mnbOutput = (MNB1Calculator)calc;
                            mnb.CopyMNB1Properties(mnbOutput);
                            //which observation is it in?
                            mnb.Alternative2 = alternative2;
                            newcalcs.Add(mnb);
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
                        == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02.ToString())
                    {
                        MNB1Calculator mnb = new MNB1Calculator();
                        if (calc.GetType().Equals(mnb.GetType()))
                        {
                            MNB1Calculator mnbOutput = (MNB1Calculator)calc;
                            mnb.CopyMNB1Properties(mnbOutput);
                            newcalcs.Add(mnb);
                        }
                    }
                }
            }
        }
        
    }
}
