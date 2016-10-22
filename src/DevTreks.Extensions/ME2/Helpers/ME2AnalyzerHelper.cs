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
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///             1. Other modules demonstrate how to implement the analyzer patterns. 
    /// </summary>
    public class ME2AnalyzerHelper
    {
        //constructor
        public ME2AnalyzerHelper() { }
        public ME2AnalyzerHelper(CalculatorParameters calcParameters)
        {
            //note that this is byref
            this.ME2CalculatorParams = calcParameters;
        }

        //properties
        //parameters needed by publishers
        public CalculatorParameters ME2CalculatorParams { get; set; }
        
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }

        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none                    = 0,
            //totals (i.e. total fat used by an operation's food input)
            metotal1               = 1,
            //stats
            mestat1                = 2,
            //change by year
            mechangeyr             = 3,
            //change by id
            mechangeid             = 4,
            //change by alternativetype
            mechangealt            = 5,
            //progress   
            meprogress1            = 6,
            //unit cost (cost per unit output)
            meeff01a1              = 7,
            //unit cost (cost per dollar revenue)
            meeff01b               = 8,
            //unit resource (resource per unit output)
            meeff02a               = 9,
            //unit resource (resource per dollar revenue)
            meeff02b               = 10
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.ME2CalculatorParams.LinkedViewElement);
            eAnalyzerType = GetAnalyzerType(sAnalyzerType);
            return eAnalyzerType;
        }
        public ANALYZER_TYPES GetAnalyzerType(string analyzerType)
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            if (analyzerType == ANALYZER_TYPES.metotal1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.metotal1;
            }
            else if (analyzerType == ANALYZER_TYPES.mechangeyr.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mechangeyr;
            }
            else if (analyzerType == ANALYZER_TYPES.mechangeid.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mechangeid;
            }
            else if (analyzerType == ANALYZER_TYPES.mechangealt.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mechangealt;
            }
            else if (analyzerType == ANALYZER_TYPES.meprogress1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.meprogress1;
            }
            else if (analyzerType == ANALYZER_TYPES.mestat1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.mestat1;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.ME2CalculatorParams.ErrorMessage
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
            this.ME2CalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.ME2CalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.ME2CalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.ME2CalculatorParams.ExtensionDocToCalcURI,
                this.ME2CalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.ME2CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                IOME2StockSubscriber subInput
                    = new IOME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subInput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer inputAnalyzer = new BIME2StockAnalyzer(
                        subInput.GCCalculatorParams);
                    inputAnalyzer.InputGroup = subInput.ME2StockCalculator.BIME2Calculator.InputGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = inputAnalyzer.SetInputME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = inputAnalyzer.SaveME2StockTotals();
                        inputAnalyzer = null;
                    }
                }
                subInput = null;
            }
            else if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                IOME2StockSubscriber subOutput
                    = new IOME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subOutput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer outputAnalyzer = new BIME2StockAnalyzer(
                        subOutput.GCCalculatorParams);
                    outputAnalyzer.OutputGroup = subOutput.ME2StockCalculator.BIME2Calculator.OutputGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outputAnalyzer.SetOutputME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outputAnalyzer.SaveME2StockTotals();
                        outputAnalyzer = null;
                    }
                }
                subOutput = null;
            }
            else if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                //build an object collection
                OCME2StockSubscriber subOperation
                    = new OCME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subOperation.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer opAnalyzer = new BIME2StockAnalyzer(
                        subOperation.GCCalculatorParams);
                    opAnalyzer.OCGroup = subOperation.ME2StockCalculator.BIME2Calculator.OCGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = opAnalyzer.SetOCME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = opAnalyzer.SaveME2StockTotals();
                        opAnalyzer = null;
                    }
                }
                subOperation = null;
            }
            else if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                //build an object collection
                OCME2StockSubscriber subComponent
                    = new OCME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subComponent.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer compAnalyzer = new BIME2StockAnalyzer(
                        subComponent.GCCalculatorParams);
                    compAnalyzer.OCGroup = subComponent.ME2StockCalculator.BIME2Calculator.OCGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = compAnalyzer.SetOCME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = compAnalyzer.SaveME2StockTotals();
                        compAnalyzer = null;
                    }
                }
                subComponent = null;
            }
            else if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                OutcomeME2StockSubscriber subOutcome
                        = new OutcomeME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subOutcome.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer outcomeAnalyzer = new BIME2StockAnalyzer(
                        subOutcome.GCCalculatorParams);
                    outcomeAnalyzer.OutcomeGroup = subOutcome.ME2StockCalculator.BIME2Calculator.OutcomeGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outcomeAnalyzer.SetOutcomeME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outcomeAnalyzer.SaveME2StockTotals();
                        outcomeAnalyzer = null;
                    }
                }
                subOutcome = null;
            }
            else if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                BIME2StockSubscriber subBudget
                    = new BIME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subBudget.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer biAnalyzer = new BIME2StockAnalyzer(
                        subBudget.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subBudget.ME2StockCalculator.BudgetGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBIME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveME2StockTotals();
                        biAnalyzer = null;
                    }
                }
                subBudget = null;
            }
            else if (this.ME2CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.investments)
            {
                BIME2StockSubscriber subInvestment
                    = new BIME2StockSubscriber(this.ME2CalculatorParams);
                bHasAnalysis = subInvestment.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BIME2StockAnalyzer biAnalyzer = new BIME2StockAnalyzer(
                        subInvestment.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subInvestment.ME2StockCalculator.BudgetGroup;
                    //2. Aggregate the base ME collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBIME2StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveME2StockTotals();
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
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.ME2CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.ME2CalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.ME2CalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.ME2CalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.ME2CalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.ME2CalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.ME2CalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = RunAnalysis();
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.ME2CalculatorParams);
                        }
                        //reset subapptype
                        this.ME2CalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.ME2CalculatorParams.ErrorMessage += this.ME2CalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.ME2CalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.ME2CalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.ME2CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.ME2CalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.ME2CalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.ME2CalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.ME2CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.ME2CalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.ME2CalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.ME2CalculatorParams.LinkedViewElement);
            this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.ME2CalculatorParams.LinkedViewElement);
            this.ME2CalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.ME2CalculatorParams.LinkedViewElement);
            //new in v1.4.5 (user can choose whether or not to display full calcs)
            this.ME2CalculatorParams.NeedsFullView
                = AnalyzerHelper.GetDisplayFullViewOption(this.ME2CalculatorParams.LinkedViewElement);
            this.ME2CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.ME2CalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            this.ME2CalculatorParams.StartingDocToCalcNodeName
                = GetNodeNameFromFileExtensionType(this.ME2CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.ME2CalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.ME2CalculatorParams.StartingDocToCalcNodeName);
        }
        private string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
            if (this.ME2CalculatorParams.StartingDocToCalcNodeName != string.Empty 
                && this.ME2CalculatorParams.StartingDocToCalcNodeName != Constants.NONE)
            {
                sStartingNodeToCalc = this.ME2CalculatorParams.StartingDocToCalcNodeName;
            }
            else
            {
                if (this.ME2CalculatorParams.DocToCalcNodeName != string.Empty
                    && this.ME2CalculatorParams.DocToCalcNodeName != Constants.NONE)
                {
                    sStartingNodeToCalc = this.ME2CalculatorParams.DocToCalcNodeName;
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
                = CalculatorHelpers.GetAttribute(this.ME2CalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.ME2CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.ME2CalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.ME2CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(this.ME2CalculatorParams,
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
            return sFileExtensionType;
        }
        //sets linkedview (analysisdoc) props needed by client to load and display doc
        public void SetLinkedViewParams()
        {
            string sStylesheetName = GetAnalysisStyleSheet();
            //sStylesheetName will be used to find the Stylesheet and, if its a first time view, 
            //to set two more params: StylesheetResourceURIPattern and StylesheetDocPath 
            string sExistingStylesheetName
                = CalculatorHelpers.GetAttribute(this.ME2CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.ME2CalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.ME2CalculatorParams.LinkedViewElement,
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
                = CalculatorHelpers.GetAttribute(this.ME2CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.ME2CalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.ME2CalculatorParams.LinkedViewElement,
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
            else if (this.AnalyzerType == ANALYZER_TYPES.mestat1)
            {
                if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsInvests1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsBuds1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsOps1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsComps1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsOutcomes1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsIns1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Stats2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2StatsOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.mechangeyr
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt)
            {
                if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeInvests1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeBuds1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeOps1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeComps1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeOutcomes1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeIns1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Change2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ChangeOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.meprogress1)
            {
                if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgInvests1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgBuds1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgOps1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgComps1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgOutcomes1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgIns1.xslt";
                    }
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "ME2Prog2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "ME2ProgOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.metotal1)
            {
                if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sAnalysisStyleSheet = "ME2StocksInvests1.xslt";
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sAnalysisStyleSheet = "ME2StocksBuds1.xslt";
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    sAnalysisStyleSheet = "ME2StocksOps1.xslt";
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    sAnalysisStyleSheet = "ME2StocksComps1.xslt";
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    sAnalysisStyleSheet = "ME2StocksOutcomes1.xslt";
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    sAnalysisStyleSheet = "ME2StocksIns1.xslt";
                }
                else if (this.ME2CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    sAnalysisStyleSheet = "ME2StocksOuts1.xslt";
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.ME2CalculatorParams.AnalyzerParms.ComparisonType
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
               == ANALYZER_TYPES.meeff01a1
                || analyzerType
               == ANALYZER_TYPES.meeff01b
                || analyzerType
               == ANALYZER_TYPES.meeff02a
                || analyzerType
               == ANALYZER_TYPES.meeff02b)
            {
                bIsEffectivenessAnalysis = true;
            }
            return bIsEffectivenessAnalysis;
        }
        public static bool IsChangeTypeAnalysis(string analyzerType)
        {
            bool bIsChangeTypeAnalysis = false;
            if (analyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || analyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString()
                || analyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || analyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                bIsChangeTypeAnalysis = true;
            }
            return bIsChangeTypeAnalysis;
        }
        
        public static void AddCalculators(int alternative2, List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                foreach (Calculator1 calc in calcs)
                {
                    //important because all analysis agg indicators by obs (by alt2)
                    calc.Alternative2 = alternative2;
                    newcalcs.Add(calc);
                }
            }
        }
        //all stock copying should use these five methods and nothing else
        public static void AddCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                foreach (Calculator1 calc in calcs)
                {
                    newcalcs.Add(calc);
                }
            }
        }
        
        public static void CopyStockCalculator(CalculatorParameters calcParams,
            List<Calculator1> oldcalcs, List<Calculator1> newcalcs)
        {
            if (oldcalcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in oldcalcs)
                {
                    //don't init newstock with this.GCCalcParam, use the oldcalc.CalcParams
                    ME2Stock newStock = new ME2Stock(calcParams);
                    bool bHasCopy = CopyStockCalculator(calc, newStock);
                    if (bHasCopy)
                    {
                        newcalcs.Add(newStock);
                    }
                }
            }
        }
        public static void CopyStockCalculator(List<Calculator1> calcs, ME2Stock newStock)
        {
            if (calcs != null)
            {
                foreach (Calculator1 calc in calcs)
                {
                    bool bHasCopy = CopyStockCalculator(calc, newStock);
                    if (bHasCopy)
                    {
                        break;
                    }
                }
            }
        }
       public static ME2Stock GetNewME2Stock(CalculatorParameters calcParams,
            Calculator1 baseElement, List<Calculator1> calcs, ME2Stock descendant)
        {
            ME2Stock newStock = new ME2Stock(calcParams, calcParams.AnalyzerParms.AnalyzerType);
            if (calcs == null)
                calcs = new List<Calculator1>();
            if (calcs.Count > 0)
            {
                ME2AnalyzerHelper.CopyandInitStockCalculator(calcs
                    .FirstOrDefault(), newStock);
            }
            else
            {
                //need the options
                newStock.CopyCalculatorProperties(descendant);
            }
            BIME2StockAnalyzer.CopyBaseElementProperties(baseElement, newStock);
            return newStock;
        }
        public static bool CopyandInitStockCalculator(Calculator1 calc, ME2Stock newStock)
        {
            bool bHasCopy = false;
            if (calc != null)
            {
                if (calc.GetType().Equals(newStock.GetType()))
                {
                    ME2Stock oldStock = (ME2Stock)calc;
                    if (oldStock != null)
                    {
                        //initiate analyzer objects
                        newStock.InitTotalME2StocksProperties();
                        //but keep calc props
                        newStock.CopyCalculatorProperties(oldStock);
                        if (oldStock.CalcParameters != null)
                        {
                            newStock.CalcParameters = new CalculatorParameters(oldStock.CalcParameters);
                        }
                        bHasCopy = true;
                    }
                }
            }
            return bHasCopy;
        }
        public static bool CopyStockCalculator(Calculator1 calc, ME2Stock newStock)
        {
            bool bHasCopy = false;
            if (calc != null)
            {
                if (calc.GetType().Equals(newStock.GetType()))
                {
                    ME2Stock oldStock = (ME2Stock)calc;
                    if (oldStock != null)
                    {
                        //only one me2stock is initialized in object model
                        bHasCopy = CopyStockCalculator(oldStock, newStock);
                    }
                }
            }
            return bHasCopy;
        }
        public static bool CopyStockCalculator(ME2Stock oldStock, ME2Stock newStock)
        {
            bool bHasCopy = false;
            if (oldStock != null && newStock != null)
            {
                //the copy will also copy calcprops and calcparams for newstock and newstock.Stocks
                newStock.CopyTotalME2StocksProperties(oldStock);
                bHasCopy = true;
            }
            return bHasCopy;
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
            if (calcParams.AnalyzerParms.AnalyzerType == ANALYZER_TYPES.metotal1.ToString())
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
    }
}
