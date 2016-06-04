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
    ///Date:		2014, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///             1. Other modules demonstrate how to implement the analyzer patterns. 
    /// </summary>
    public class NPV1AnalyzerHelper
    {
        //constructor
        public NPV1AnalyzerHelper() { }
        public NPV1AnalyzerHelper(CalculatorParameters calcParameters)
        {
            //note that this is byref
            this.NPV1CalculatorParams = calcParameters;
        }

        //properties
        //parameters needed by publishers
        public CalculatorParameters NPV1CalculatorParams { get; set; }
        
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }

        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none                    = 0,
            //totals (i.e. total fat used by an operation's food input)
            npvtotal1               = 1,
            //stats
            npvstat1                = 2,
            //change by year
            npvchangeyr             = 3,
            //change by id
            npvchangeid             = 4,
            //change by alternativetype
            npvchangealt            = 5,
            //progress   
            npvprogress1            = 6,
            //unit cost (cost per unit output)
            npveff01a1              = 7,
            //unit cost (cost per dollar revenue)
            npveff01b               = 8,
            //unit resource (resource per unit output)
            npveff02a               = 9,
            //unit resource (resource per dollar revenue)
            npveff02b               = 10
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.NPV1CalculatorParams.LinkedViewElement);
            eAnalyzerType = GetAnalyzerType(sAnalyzerType);
            return eAnalyzerType;
        }
        public ANALYZER_TYPES GetAnalyzerType(string analyzerType)
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            if (analyzerType == ANALYZER_TYPES.npvtotal1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.npvtotal1;
            }
            else if (analyzerType == ANALYZER_TYPES.npvchangeyr.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.npvchangeyr;
            }
            else if (analyzerType == ANALYZER_TYPES.npvchangeid.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.npvchangeid;
            }
            else if (analyzerType == ANALYZER_TYPES.npvchangealt.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.npvchangealt;
            }
            else if (analyzerType == ANALYZER_TYPES.npvprogress1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.npvprogress1;
            }
            else if (analyzerType == ANALYZER_TYPES.npvstat1.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.npvstat1;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.NPV1CalculatorParams.ErrorMessage
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
            this.NPV1CalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.NPV1CalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.NPV1CalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.NPV1CalculatorParams.ExtensionDocToCalcURI,
                this.NPV1CalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.NPV1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                IONPV1StockSubscriber subInput
                    = new IONPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subInput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer inputAnalyzer = new BINPV1StockAnalyzer(
                        subInput.GCCalculatorParams);
                    inputAnalyzer.InputGroup = subInput.NPV1StockCalculator.BINPV1Calculator.InputGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = inputAnalyzer.SetInputNPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = inputAnalyzer.SaveNPV1StockTotals();
                        inputAnalyzer = null;
                    }
                }
                subInput = null;
            }
            else if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                IONPV1StockSubscriber subOutput
                    = new IONPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subOutput.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer outputAnalyzer = new BINPV1StockAnalyzer(
                        subOutput.GCCalculatorParams);
                    outputAnalyzer.OutputGroup = subOutput.NPV1StockCalculator.BINPV1Calculator.OutputGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outputAnalyzer.SetOutputNPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outputAnalyzer.SaveNPV1StockTotals();
                        outputAnalyzer = null;
                    }
                }
                subOutput = null;
            }
            else if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                //build an object collection
                OCNPV1StockSubscriber subOperation
                    = new OCNPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subOperation.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer opAnalyzer = new BINPV1StockAnalyzer(
                        subOperation.GCCalculatorParams);
                    opAnalyzer.OCGroup = subOperation.NPV1StockCalculator.BINPV1Calculator.OCGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = opAnalyzer.SetOCNPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = opAnalyzer.SaveNPV1StockTotals();
                        opAnalyzer = null;
                    }
                }
                subOperation = null;
            }
            else if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                //build an object collection
                OCNPV1StockSubscriber subComponent
                    = new OCNPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subComponent.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer compAnalyzer = new BINPV1StockAnalyzer(
                        subComponent.GCCalculatorParams);
                    compAnalyzer.OCGroup = subComponent.NPV1StockCalculator.BINPV1Calculator.OCGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = compAnalyzer.SetOCNPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = compAnalyzer.SaveNPV1StockTotals();
                        compAnalyzer = null;
                    }
                }
                subComponent = null;
            }
            else if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                OutcomeNPV1StockSubscriber subOutcome
                        = new OutcomeNPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subOutcome.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer outcomeAnalyzer = new BINPV1StockAnalyzer(
                        subOutcome.GCCalculatorParams);
                    outcomeAnalyzer.OutcomeGroup = subOutcome.NPV1StockCalculator.BINPV1Calculator.OutcomeGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = outcomeAnalyzer.SetOutcomeNPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = outcomeAnalyzer.SaveNPV1StockTotals();
                        outcomeAnalyzer = null;
                    }
                }
                subOutcome = null;
            }
            else if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.budgets)
            {
                BINPV1StockSubscriber subBudget
                    = new BINPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subBudget.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer biAnalyzer = new BINPV1StockAnalyzer(
                        subBudget.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subBudget.NPV1StockCalculator.BudgetGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBINPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveNPV1StockTotals();
                        biAnalyzer = null;
                    }
                }
                subBudget = null;
            }
            else if (this.NPV1CalculatorParams.SubApplicationType
               == Constants.SUBAPPLICATION_TYPES.investments)
            {
                BINPV1StockSubscriber subInvestment
                    = new BINPV1StockSubscriber(this.NPV1CalculatorParams);
                bHasAnalysis = subInvestment.RunCalculator();
                if (bHasAnalysis)
                {
                    //transfer object model to Analyzer
                    BINPV1StockAnalyzer biAnalyzer = new BINPV1StockAnalyzer(
                        subInvestment.GCCalculatorParams);
                    biAnalyzer.BudgetGroup = subInvestment.NPV1StockCalculator.BudgetGroup;
                    //2. Aggregate the base NPV collections and add Stock totals to collections
                    //aggregation is for full collection including descendants
                    bHasAggregation = biAnalyzer.SetBINPV1StockTotals();
                    if (bHasAggregation)
                    {
                        //Save the results as xml
                        bHasAnalysis = biAnalyzer.SaveNPV1StockTotals();
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
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.NPV1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.NPV1CalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.NPV1CalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.NPV1CalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.NPV1CalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.NPV1CalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.NPV1CalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = RunAnalysis();
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.NPV1CalculatorParams);
                        }
                        //reset subapptype
                        this.NPV1CalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.NPV1CalculatorParams.ErrorMessage += this.NPV1CalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.NPV1CalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.NPV1CalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.NPV1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.NPV1CalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.NPV1CalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.NPV1CalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.NPV1CalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.NPV1CalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.NPV1CalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.NPV1CalculatorParams.LinkedViewElement);
            this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.NPV1CalculatorParams.LinkedViewElement);
            this.NPV1CalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.NPV1CalculatorParams.LinkedViewElement);
            //new in v1.4.5 (user can choose whether or not to display full calcs)
            this.NPV1CalculatorParams.NeedsFullView
                = AnalyzerHelper.GetDisplayFullViewOption(this.NPV1CalculatorParams.LinkedViewElement);
            this.NPV1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.NPV1CalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            this.NPV1CalculatorParams.StartingDocToCalcNodeName
                = GetNodeNameFromFileExtensionType(this.NPV1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.NPV1CalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.NPV1CalculatorParams.StartingDocToCalcNodeName);
        }
        private string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
            if (this.NPV1CalculatorParams.StartingDocToCalcNodeName != string.Empty 
                && this.NPV1CalculatorParams.StartingDocToCalcNodeName != Constants.NONE)
            {
                sStartingNodeToCalc = this.NPV1CalculatorParams.StartingDocToCalcNodeName;
            }
            else
            {
                if (this.NPV1CalculatorParams.DocToCalcNodeName != string.Empty
                    && this.NPV1CalculatorParams.DocToCalcNodeName != Constants.NONE)
                {
                    sStartingNodeToCalc = this.NPV1CalculatorParams.DocToCalcNodeName;
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
                = CalculatorHelpers.GetAttribute(this.NPV1CalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.NPV1CalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.NPV1CalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.NPV1CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(this.NPV1CalculatorParams,
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
                = CalculatorHelpers.GetAttribute(this.NPV1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.NPV1CalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.NPV1CalculatorParams.LinkedViewElement,
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
                = CalculatorHelpers.GetAttribute(this.NPV1CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.NPV1CalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.NPV1CalculatorParams.LinkedViewElement,
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
            else if (this.AnalyzerType == ANALYZER_TYPES.npvstat1)
            {
                if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsInvests1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsBuds1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsOps1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsComps1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsOutcomes1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsIns1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Stats2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1StatsOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.npvchangeyr
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt)
            {
                if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeInvests1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeBuds1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeOps1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeComps1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeOutcomes1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeIns1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Change2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ChangeOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.npvprogress1)
            {
                if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Invests1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgInvests1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Buds1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgBuds1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Ops1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgOps1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Comps1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgComps1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Outcomes1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgOutcomes1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Ins1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgIns1.xslt";
                    }
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet = "NPV1Prog2Outs1.xslt";
                    }
                    else
                    {
                        sAnalysisStyleSheet = "NPV1ProgOuts1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.npvtotal1)
            {
                if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sAnalysisStyleSheet = "NPV1StocksInvests1.xslt";
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sAnalysisStyleSheet = "NPV1StocksBuds1.xslt";
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.operationprices)
                {
                    sAnalysisStyleSheet = "NPV1StocksOps1.xslt";
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.componentprices)
                {
                    sAnalysisStyleSheet = "NPV1StocksComps1.xslt";
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                {
                    sAnalysisStyleSheet = "NPV1StocksOutcomes1.xslt";
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    sAnalysisStyleSheet = "NPV1StocksIns1.xslt";
                }
                else if (this.NPV1CalculatorParams.SubApplicationType
                     == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    sAnalysisStyleSheet = "NPV1StocksOuts1.xslt";
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.NPV1CalculatorParams.AnalyzerParms.ComparisonType
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
               == ANALYZER_TYPES.npveff01a1
                || analyzerType
               == ANALYZER_TYPES.npveff01b
                || analyzerType
               == ANALYZER_TYPES.npveff02a
                || analyzerType
               == ANALYZER_TYPES.npveff02b)
            {
                bIsEffectivenessAnalysis = true;
            }
            return bIsEffectivenessAnalysis;
        }
        public static bool IsChangeTypeAnalysis(string analyzerType)
        {
            bool bIsChangeTypeAnalysis = false;
            if (analyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || analyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                || analyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || analyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
        
        public static void CopyStockCalculator(List<Calculator1> oldcalcs, List<Calculator1> newcalcs)
        {
            if (oldcalcs != null)
            {
                if (newcalcs == null)
                    newcalcs = new List<Calculator1>();
                foreach (Calculator1 calc in oldcalcs)
                {
                    //don't init newstock with this.GCCalcParam, use the oldcalc.CalcParams
                    NPV1Stock newStock = new NPV1Stock();
                    //NPV1Stock newStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    bool bHasCopy = CopyStockCalculator(calc, newStock);
                    if (bHasCopy)
                    {
                        newcalcs.Add(newStock);
                    }
                }
            }
        }
        public static void CopyStockCalculator(List<Calculator1> calcs, NPV1Stock newStock)
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
        public static bool CopyandInitStockCalculator(Calculator1 calc, NPV1Stock newStock)
        {
            bool bHasCopy = false;
            if (calc != null)
            {
                if (calc.GetType().Equals(newStock.GetType()))
                {
                    NPV1Stock oldStock = (NPV1Stock)calc;
                    if (oldStock != null)
                    {
                        //copy initial totals
                        newStock.InitTotalNPV1StocksProperties(oldStock);
                        //but keep calc props
                        newStock.CopyCalculatorProperties(oldStock);
                        bHasCopy = true;
                    }
                }
            }
            return bHasCopy;
        }
        public static bool CopyStockCalculator(Calculator1 calc, NPV1Stock newStock)
        {
            bool bHasCopy = false;
            if (calc != null)
            {
                if (calc.GetType().Equals(newStock.GetType()))
                {
                    NPV1Stock oldStock = (NPV1Stock)calc;
                    if (oldStock != null)
                    {
                        //only one npv1stock is initialized in object model
                        bHasCopy = CopyStockCalculator(oldStock, newStock);
                    }
                }
            }
            return bHasCopy;
        }
        public static bool CopyStockCalculator(NPV1Stock oldStock, NPV1Stock newStock)
        {
            bool bHasCopy = false;
            if (oldStock != null && newStock != null)
            {
                //copying stocks should all take place uniformly here
                newStock.CopyTotalNPV1StocksProperties(oldStock);
                //newStock.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
                bHasCopy = true;
            }
            return bHasCopy;
        }
        
    }
}
