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
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class ARSAnalyzerHelper 
    {
        //constructor
        public ARSAnalyzerHelper() { }
        public ARSAnalyzerHelper(CalculatorParameters calcParameters)
        {
            this.ARSCalculatorParams = calcParameters;
        }
        
        //properties
        //parameters needed by publishers
        public CalculatorParameters ARSCalculatorParams { get; set; }
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }
        
        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none                = 0,
            statistics01        = 1,
            //totals (i.e. total fuel used by an operation's machinery)
            resources01         = 2,
            //unit cost (cost per unit output)
            effectiveness01a    = 3,
            //unit cost (cost per dollar revenue)
            effectiveness01b    = 4,
            //unit resource (resource per unit output)
            effectiveness02a    = 5,
            //unit resource (resource per dollar revenue)
            effectiveness02b    = 6,
            //scheduling and timeliness cost totals
            resources02         = 7,
            //scheduling and timeliness optimums
            resources02a        = 8
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.ARSCalculatorParams.LinkedViewElement);
            eAnalyzerType = GetAnalyzerType(sAnalyzerType);
            return eAnalyzerType;
        }
        public ANALYZER_TYPES GetAnalyzerType(string analyzerType)
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            if (analyzerType == ANALYZER_TYPES.effectiveness01a.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.effectiveness01a;
            }
            else if (analyzerType == ANALYZER_TYPES.effectiveness01b.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.effectiveness01b;
            }
            else if (analyzerType == ANALYZER_TYPES.effectiveness02a.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.effectiveness02a;
            }
            else if (analyzerType == ANALYZER_TYPES.effectiveness02b.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.effectiveness02b;
            }
            else if (analyzerType == ANALYZER_TYPES.none.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.none;
            }
            else if (analyzerType == ANALYZER_TYPES.resources01.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.resources01;
            }
            else if (analyzerType == ANALYZER_TYPES.resources02.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.resources02;
            }
            else if (analyzerType == ANALYZER_TYPES.resources02a.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.resources02a;
            }
            else if (analyzerType == ANALYZER_TYPES.statistics01.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.statistics01;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.ARSCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            return eAnalyzerType;
        }
        
        public static AnalyzerHelper.ANALYZER_GENERAL_TYPES GetBaseAnalyzerType(
            ANALYZER_TYPES analyzerType)
        {
            AnalyzerHelper.ANALYZER_GENERAL_TYPES eAnalyzerGeneralType
                = AnalyzerHelper.ANALYZER_GENERAL_TYPES.none;
            if (analyzerType
                == ANALYZER_TYPES.statistics01)
            {
                eAnalyzerGeneralType
                    = AnalyzerHelper.ANALYZER_GENERAL_TYPES.basic;
            }
            else if (analyzerType
                == ANALYZER_TYPES.effectiveness01a
                || analyzerType
                == ANALYZER_TYPES.effectiveness01b
                || analyzerType
                == ANALYZER_TYPES.effectiveness02a
                || analyzerType
                == ANALYZER_TYPES.effectiveness02b)
            {
                eAnalyzerGeneralType
                    = AnalyzerHelper.ANALYZER_GENERAL_TYPES.outputbased;
            }
            else if (analyzerType
                == ANALYZER_TYPES.resources01)
            {
                eAnalyzerGeneralType
                    = AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased;
            }
            else if (analyzerType
                == ANALYZER_TYPES.resources02)
            {
                eAnalyzerGeneralType
                    = AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased;
            }
            else if (analyzerType
                == ANALYZER_TYPES.resources02a)
            {
                eAnalyzerGeneralType
                    = AnalyzerHelper.ANALYZER_GENERAL_TYPES.inputbased;
            }
            return eAnalyzerGeneralType;
        }
        public bool RunAnalysis()
        {
            bool bHasCalculations = false;
            if (this.ARSCalculatorParams.ErrorMessage == string.Empty)
            {
                //run the analysis and fill in the updates collection
                SetAnalysis();
                //set parameters/attributes needed to update db and display this analysis
                SetAnalysisParameters();
            }
            if (this.ARSCalculatorParams.ErrorMessage == string.Empty)
            {
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public bool RunAnalysis(IList<string> urisToAnalyze)
        {
            bool bHasAnalysis = false;
            //set the files needing analysis
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.ARSCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.ARSCalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.ARSCalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.ARSCalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.ARSCalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.ARSCalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.ARSCalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = RunAnalysis();
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.ARSCalculatorParams);
                        }
                        //reset subapptype
                        this.ARSCalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.ARSCalculatorParams.ErrorMessage += this.ARSCalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.ARSCalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.ARSCalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.ARSCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.ARSCalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.ARSCalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.ARSCalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.ARSCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.ARSCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.ARSCalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.ARSCalculatorParams.LinkedViewElement);
            this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.ARSCalculatorParams.LinkedViewElement);
            this.ARSCalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.ARSCalculatorParams.LinkedViewElement);
            this.ARSCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.ARSCalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            this.ARSCalculatorParams.CalculatorType 
                = this.ARSCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType;
            if (this.AnalyzerType != ANALYZER_TYPES.resources01
                && (!this.AnalyzerType.ToString().StartsWith(ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())))
            {
                //resources01 uses the calculator pattern, which does not change startingdoctocalc
                this.ARSCalculatorParams.StartingDocToCalcNodeName
                    = GetNodeNameFromFileExtensionType(this.ARSCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            }
            this.ARSCalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.ARSCalculatorParams.StartingDocToCalcNodeName);
        }
        public void SetAnalysis()
        {
            //set the files needing analysis
            string sFileExtension = GetFilesToAnalyzeExtension(this.ARSCalculatorParams,
                this.AnalyzerType);
            CalculatorHelpers.SetFileOrFoldersToAnalyze(sFileExtension,
                this.ARSCalculatorParams, this.ARSCalculatorParams.UrisToAnalyze);


            if (string.IsNullOrEmpty(this.ARSCalculatorParams.ErrorMessage))
            {
                if (this.ARSCalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
                {
                    if (this.ARSCalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                    {
                        //1. generate an observations file
                        bool bHasObservations = BuildObservations();
                        if (bHasObservations)
                        {
                            string sObservationsFilePath 
                                = ObservationBuilder.GetObservationFilePath(
                                this.ARSCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
                            this.ARSCalculatorParams.AnalyzerParms.ObservationsPath
                                = sObservationsFilePath;
                            //2. use the observations to produce a statistical result file
                            bool bHasAnalysis
                                = RunStatisticsFromObservations();
                        }
                    }
                    else
                    {
                        if (this.ARSCalculatorParams.DocToCalcNodeName
                            == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                        {
                            //setting default analyzer attribute values
                        }
                        else
                        {
                            this.ARSCalculatorParams.ErrorMessage 
                                = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                        }
                    }
                }
                else
                {
                    this.ARSCalculatorParams.ErrorMessage 
                        = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                }
            }
        }
        public bool BuildObservations()
        {
            bool bHasAnalysis = false;
            //prepare the event subscriber
            ARSObservationsSubscriber subObserver
                = new ARSObservationsSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
            //build the observations document
            bHasAnalysis = subObserver.BuildObservations();
            subObserver = null;
            //analyzers always aggregate calculator data
            //if a future calculator needs a custom observation aggregation
            //use an if else statement conditional on calculatortype:
            //See RunStatisticsFromObservations()
            return bHasAnalysis;
        }
        public bool RunStatisticsFromObservations()
        {
            bool bHasAnalysis = false;
            //analyzers always aggregate calculator data (this.ARSCalculatorParams.CalculatorType)
            //those calculators are referenced by the project they are in
            if (this.ARSCalculatorParams.CalculatorType
                 == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
            {
                //prepare the event subscriber
                BIAnalyzerSubscriber subBudget
                    = new BIAnalyzerSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
                //run the analyses
                bHasAnalysis = subBudget.RunAnalyzer();
                UpdateCalcParameters(this.ARSCalculatorParams,
                    subBudget.GCCalculatorParams);
                subBudget = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
            {
                //prepare the event subscriber
                BIAnalyzerSubscriber subInvestment
                    = new BIAnalyzerSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
                //run the analyses
                bHasAnalysis = subInvestment.RunAnalyzer();
                UpdateCalcParameters(this.ARSCalculatorParams,
                    subInvestment.GCCalculatorParams);
                subInvestment = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == CalculatorHelpers.CALCULATOR_TYPES.operation.ToString())
            {
                //prepare the event subscriber
                OCAnalyzerSubscriber subOperation
                    = new OCAnalyzerSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
                //run the analyses
                bHasAnalysis = subOperation.RunAnalyzer();
                UpdateCalcParameters(this.ARSCalculatorParams,
                    subOperation.GCCalculatorParams);
                subOperation = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == CalculatorHelpers.CALCULATOR_TYPES.component.ToString())
            {
                //prepare the event subscriber
                OCAnalyzerSubscriber subComponent
                    = new OCAnalyzerSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
                //run the analyses
                bHasAnalysis = subComponent.RunAnalyzer();
                UpdateCalcParameters(this.ARSCalculatorParams,
                    subComponent.GCCalculatorParams);
                subComponent = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == CalculatorHelpers.CALCULATOR_TYPES.output.ToString())
            {
                //prepare the event subscriber
                IOAnalyzerSubscriber subOutput
                    = new IOAnalyzerSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
                //run the analyses
                bHasAnalysis = subOutput.RunAnalyzer();
                UpdateCalcParameters(this.ARSCalculatorParams,
                    subOutput.GCCalculatorParams);
                subOutput = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == CalculatorHelpers.CALCULATOR_TYPES.input.ToString())
            {
                //prepare the event subscriber
                IOAnalyzerSubscriber subInput
                    = new IOAnalyzerSubscriber(this.ARSCalculatorParams, this.AnalyzerType);
                //run the analyses
                bHasAnalysis = subInput.RunAnalyzer();
                UpdateCalcParameters(this.ARSCalculatorParams,
                    subInput.GCCalculatorParams);
                subInput = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices.ToString())
            {
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
            }
            else if (this.ARSCalculatorParams.CalculatorType
                 == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
            {
            }
            return bHasAnalysis;
        }
        
        private void UpdateCalcParameters(CalculatorParameters calcParams,
            CalculatorParameters newCalcParams)
        {
            calcParams.ErrorMessage = newCalcParams.ErrorMessage;
            calcParams.StartingDocToCalcNodeName = newCalcParams.StartingDocToCalcNodeName;
        }
        public static void SetExtensionAttributes(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement)
        {
            //set any attributes specific to this extension needed by specific calculators
            //the input calculators remove all attributes from the linked view and these get lost
            if (!string.IsNullOrEmpty(calcParameters.AnalyzerParms.AnalyzerType)
                && calcParameters.AnalyzerParms.AnalyzerType != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement, AnalyzerHelper.ANALYZER_TYPE,
                    calcParameters.AnalyzerParms.AnalyzerType);
            }
            if (!string.IsNullOrEmpty(calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType)
                && calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement, AnalyzerHelper.FILESTOANALYZE_EXTENSION_TYPE,
                    calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType);
            }
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
                = CalculatorHelpers.GetAttribute(this.ARSCalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType 
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.ARSCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.ARSCalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.ARSCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath); 
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(this.ARSCalculatorParams,
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
            //i.e. filetoanalyzeExttype = budget; analyzertype = stats01
            //the none comparision type has been deprecated
            //= budgetstats01none
            return sFileExtensionType;
        }
        //sets linkedview (analysisdoc) props needed by client to load and display doc
        public void SetLinkedViewParams()
        {
            string sStylesheetName = GetAnalysisStyleSheet();
            //sStylesheetName will be used to find the Stylesheet and, if its a first time view, 
            //to set two more params: StylesheetResourceURIPattern and StylesheetDocPath 
            string sExistingStylesheetName
                = CalculatorHelpers.GetAttribute(this.ARSCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.ARSCalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.ARSCalculatorParams.LinkedViewElement,
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
                = CalculatorHelpers.GetAttribute(this.ARSCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.ARSCalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.ARSCalculatorParams.LinkedViewElement,
                    Calculator1.cStylesheet2ObjectNS,
                    sStylesheetExtObjNamespace);
            }
        }
        private string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
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
                AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
                sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
            }
            else if (fileExtensionType ==
                AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices.ToString())
            {
                sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
            }
            else if (fileExtensionType ==
                AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
                sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
            }
            else if (fileExtensionType ==
                AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
            {
                sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
            }
            else if (fileExtensionType.StartsWith(CalculatorHelpers.CALCULATOR_TYPES.operation.ToString()))
            {
                sStartingNodeToCalc = OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString();
            }
            else if (fileExtensionType.StartsWith(CalculatorHelpers.CALCULATOR_TYPES.component.ToString()))
            {
                sStartingNodeToCalc = OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString();
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
            return sStartingNodeToCalc;
        }
        private string GetAnalysisStyleSheet()
        {
            string sAnalysisStyleSheet = string.Empty;
            if (this.AnalyzerType == ANALYZER_TYPES.none)
            {
                sAnalysisStyleSheet = string.Empty;
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.statistics01
                || IsEffectivenessAnalysis(this.AnalyzerType))
            {
                if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Invests.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Profits.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Operations.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Components.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Inputs.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Outputs.xslt";
                    }
                }
                else
                {
                    if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Invests.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Profits.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Operations.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Components.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Inputs.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Outputs.xslt";
                    }
                }
                if (this.AnalyzerType == ANALYZER_TYPES.effectiveness01a)
                {
                    sAnalysisStyleSheet 
                        = sAnalysisStyleSheet.Replace("Stats01", "Units01a");
                    //or if comps
                    if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet 
                            = sAnalysisStyleSheet.Replace("Stats02", "Units02a");
                    }
                }
                else if (this.AnalyzerType == ANALYZER_TYPES.effectiveness01b)
                {
                    sAnalysisStyleSheet 
                        = sAnalysisStyleSheet.Replace("Stats01", "Units01b");
                    //or if comps
                    if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet 
                            = sAnalysisStyleSheet.Replace("Stats02", "Units02b");
                    }
                }
                else if (this.AnalyzerType == ANALYZER_TYPES.effectiveness02a)
                {
                    sAnalysisStyleSheet 
                        = sAnalysisStyleSheet.Replace("Stats01", "UnitResources01a");
                    //or if comps
                    if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet 
                            = sAnalysisStyleSheet.Replace("Stats02", "UnitResources02a");
                    }
                }
                else if (this.AnalyzerType == ANALYZER_TYPES.effectiveness02b)
                {
                    sAnalysisStyleSheet = sAnalysisStyleSheet.Replace("Stats01", "UnitResources01b");
                    //or if comps
                    if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet 
                            = sAnalysisStyleSheet.Replace("Stats02", "UnitResources02b");
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.resources01)
            {
                if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources2Invests1.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources2Profits1.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources2Operations1.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName 
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources2Components1.xslt";
                    }
                }
                else
                {
                    if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                    {
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksIns1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksIns1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksIns1.xslt";
                        }
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.operation.ToString())
                    {
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksOps1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksOps1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksOps1.xslt";
                        }
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
                    {
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksBuds1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksBuds1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksBuds1.xslt";
                        }
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.component.ToString())
                    {
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksComps1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksComps1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksComps1.xslt";
                        }
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
                    {
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksInvests1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksInvests1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksInvests1.xslt";
                        }
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.resources02)
            {
                if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04Invests.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04Profits.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04Operations.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04Components.xslt";
                    }
                }
                else
                {
                    if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                    {
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksIns1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksIns1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksIns1.xslt";
                        }
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.operation2.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksOps1.xslt";
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksBuds1.xslt";
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.component2.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksComps1.xslt";
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksInvests1.xslt";
                    }
                }
            }
            else if (this.AnalyzerType == ANALYZER_TYPES.resources02a)
            {
                if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04aInvests.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04aProfits.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04aOperations.xslt";
                    }
                    else if (this.ARSCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Resources04aComponents.xslt";
                    }
                }
                else
                {
                    if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString()
                        || this.ARSCalculatorParams.CalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                    {
                        //v120 : former 2a styles are the same as the 2 styles
                        if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                        {
                            sAnalysisStyleSheet = "AgMach1StocksIns1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                        {
                            sAnalysisStyleSheet = "IrrPower1StocksIns1.xslt";
                        }
                        else if (this.ARSCalculatorParams.RelatedCalculatorType
                            == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                        {
                            sAnalysisStyleSheet = "GenCap1StocksIns1.xslt";
                        }
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.operation2.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksOps1.xslt";
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksBuds1.xslt";
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.component2.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksComps1.xslt";
                    }
                    else if (this.ARSCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
                    {
                        sAnalysisStyleSheet = "AgMach2StocksInvests1.xslt";
                    }
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.ARSCalculatorParams.AnalyzerParms.ComparisonType
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
        public static string GetFilesToAnalyzeExtension(CalculatorParameters calcParams,
            ANALYZER_TYPES analysisType)
        {
            string sFileExtension = string.Empty;
            //get analysis of calculated docs file extension
            if (calcParams.DocToCalcNodeName.EndsWith("group"))
            {
                if (calcParams.AnalyzerParms.SubFolderType
                    == AnalyzerHelper.SUBFOLDER_OPTIONS.yes)
                {
                    //going after db budgets and investments
                    sFileExtension = string.Concat(Constants.FILENAME_DELIMITER,
                        "full");
                }
                else
                {
                    //going after terminal files
                    sFileExtension = string.Empty;
                }
            }
            else
            {
                sFileExtension = string.Empty;
            }
            //get analysis of analyzed docs file extension
            //refactor: as analyses come on board
            if (calcParams.AnalyzerParms.SubFolderType
                == AnalyzerHelper.SUBFOLDER_OPTIONS.yes
                && analysisType
                == ANALYZER_TYPES.statistics01)
            {
                sFileExtension = string.Concat(calcParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                    ANALYZER_TYPES.statistics01, AnalyzerHelper.COMPARISON_OPTIONS.none.ToString());
                //sFileExtension = MakeAnalysisFileExtensionType(this.NPVAnalyzerParams.FilesToAnalyzeExtensionType);
            }
            else
            {
                sFileExtension = string.Concat(calcParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                    sFileExtension);
            }
            return sFileExtension;
        }
        
        //public static bool NeedsStatsCalculations(string analysisDocPath, 
        //    string statsAnalysisFileName, ANALYZER_TYPES analysisType, 
        //    string analysisId, out string statsDocPath)
        //{
        //    bool bNeedsStatsCalculations = false;
        //    //returns effectiveness_01_100
        //    string sThisAnalysisFileName = string.Empty;
        //    //replaces effectiveness_01_100 with statistics01_125
        //    statsDocPath = analysisDocPath.Replace(sThisAnalysisFileName, statsAnalysisFileName);
        //    if (CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
        //        statsDocPath) == false)
        //    {
        //        bNeedsStatsCalculations = true;
        //    }
        //    return bNeedsStatsCalculations;
        //}
        //public static bool NeedsStatsCalculations(string statsAnalysisFilePath)
        //{
        //    bool bNeedsStatsCalculations = false;
        //    if (CalculatorHelpers.URIAbsoluteExists(this.GCCalculatorParams.ExtensionDocToCalcURI,
        //        statsAnalysisFilePath) == false)
        //    {
        //        bNeedsStatsCalculations = true;
        //    }
        //    return bNeedsStatsCalculations;
        //}
        public static bool IsEffectivenessAnalysis(
            ANALYZER_TYPES analyzerType)
        {
            bool bIsEffectivenessAnalysis = false;
            if (analyzerType
               == ANALYZER_TYPES.effectiveness01a
                || analyzerType
               == ANALYZER_TYPES.effectiveness01b
                || analyzerType
               == ANALYZER_TYPES.effectiveness02a
                || analyzerType
               == ANALYZER_TYPES.effectiveness02b)
            {
                bIsEffectivenessAnalysis = true;
            }
            return bIsEffectivenessAnalysis;
        }
        public void AddDisplayParameters(ExtensionContentURI extDocToCalcURI)
        {
            //some analyzers display linkedview data using two form els:
            StringBuilder strB = new StringBuilder();
            strB.Append("&");
            strB.Append(Calculator1.WHATIF_TAGNAME_FORM);
            strB.Append("=");
            strB.Append(this.ARSCalculatorParams.WhatIfScenario);
            strB.Append("&");
            strB.Append(Calculator1.RELATEDCALCULATORSTYPE_FORM);
            strB.Append("=");
            strB.Append(this.ARSCalculatorParams.RelatedCalculatorsType);
            strB.Append(extDocToCalcURI.URIDataManager.CalcParams);
        }
    }
}
