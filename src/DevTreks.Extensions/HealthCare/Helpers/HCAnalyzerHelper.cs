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
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///             1. Other modules demonstrate how to implement the analyzer patterns. 
    ///             Its not available for food nutritional analysis yet.
    /// </summary>
    public class HCAnalyzerHelper
    {
        //constructor
        public HCAnalyzerHelper() { }
        public HCAnalyzerHelper(CalculatorParameters calcParameters)
        {
            this.HCCalculatorParams = calcParameters;
        }

        //properties
        //parameters needed by publishers
        public CalculatorParameters HCCalculatorParams { get; set; }
        //analyzers specific to this extension
        public ANALYZER_TYPES AnalyzerType { get; set; }

        //calculations are run using ANALYZER_TYPE attribute with this enum
        //common what-if scenarios run by also using WHATIF_TAG_NAME attribute 
        //(i.e. descendant xmldoc calc nodes are loaded using: //linkedview[@WhatIfTagName='lowyield_highprice'])
        public enum ANALYZER_TYPES
        {
            none = 0,
            statistics01 = 1,
            //totals (i.e. total fat used by an operation's food input)
            resources01 = 2,
            //unit cost (cost per unit output)
            effectiveness01a = 3,
            //unit cost (cost per dollar revenue)
            effectiveness01b = 4,
            //unit resource (resource per unit output)
            effectiveness02a = 5,
            //unit resource (resource per dollar revenue)
            effectiveness02b = 6
        }
        //methods
        public ANALYZER_TYPES GetAnalyzerType()
        {
            ANALYZER_TYPES eAnalyzerType = ANALYZER_TYPES.none;
            string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(
                this.HCCalculatorParams.LinkedViewElement);
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
            else if (analyzerType == ANALYZER_TYPES.statistics01.ToString())
            {
                eAnalyzerType = ANALYZER_TYPES.statistics01;
            }
            if (eAnalyzerType == ANALYZER_TYPES.none)
            {
                this.HCCalculatorParams.ErrorMessage
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
            return eAnalyzerGeneralType;
        }

        public void SetOptions()
        {
            this.AnalyzerType = GetAnalyzerType();
            this.HCCalculatorParams.AnalyzerParms.AnalyzerType = this.AnalyzerType.ToString();
            if (this.HCCalculatorParams.AnalyzerParms.AnalyzerType == string.Empty
                || this.HCCalculatorParams.AnalyzerParms.AnalyzerType == Constants.NONE)
            {
                this.HCCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            this.HCCalculatorParams.AnalyzerParms.AnalyzerGeneralType
                    = GetBaseAnalyzerType(this.AnalyzerType);
            this.HCCalculatorParams.AnalyzerParms.SubFolderType
                = AnalyzerHelper.GetSubFolderType(this.HCCalculatorParams.LinkedViewElement);
            this.HCCalculatorParams.AnalyzerParms.ComparisonType
                = AnalyzerHelper.GetComparisonType(this.HCCalculatorParams.LinkedViewElement);
            this.HCCalculatorParams.AnalyzerParms.AggregationType
                = AnalyzerHelper.GetAggregationType(this.HCCalculatorParams.LinkedViewElement);
            this.HCCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType
                = CalculatorHelpers.GetAttribute(this.HCCalculatorParams.LinkedViewElement,
                Calculator1.cFilesToAnalyzeExtensionType);
            this.HCCalculatorParams.CalculatorType
                = this.HCCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType;
            if (this.AnalyzerType != ANALYZER_TYPES.resources01)
            {
                //resources01 uses the calculator pattern, which does not change startingdoctocalc
                this.HCCalculatorParams.StartingDocToCalcNodeName
                    = GetNodeNameFromFileExtensionType(this.HCCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            }
            this.HCCalculatorParams.SubApplicationType
                = CalculatorHelpers.GetSubAppTypeFromNodeName2(
                 this.HCCalculatorParams.StartingDocToCalcNodeName);
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
                HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
            {
                sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
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
                = CalculatorHelpers.GetAttribute(this.HCCalculatorParams.LinkedViewElement,
                Calculator1.cFileExtensionType);
            //test 1 (note that this file extension type can be different than the one used to analyze)
            string sNeededFileExtensionType
                = MakeAnalysisFileExtensionTypeForDbUpdate(
                    this.HCCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType);
            this.HCCalculatorParams.FileExtensionType
                = sNeededFileExtensionType;
            //test 2 (file extension type will need to change doctocalcpath during save)
            string sDocToCalcFileName
                = Path.GetFileName(this.HCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath);
            if (!sFileExtensionType.Equals(sNeededFileExtensionType)
                || !sDocToCalcFileName.Contains(sNeededFileExtensionType))
            {
                CalculatorHelpers.SetFileExtensionType(this.HCCalculatorParams,
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
            if (this.HCCalculatorParams.AnalyzerParms.SubFolderType
                == AnalyzerHelper.SUBFOLDER_OPTIONS.yes)
            {
                //need to find a stats01, no comparisons analyzer's results: 
                sFileExtensionType = string.Concat(filesToAnalyzeExtensionType,
                    ANALYZER_TYPES.statistics01,
                    AnalyzerHelper.COMPARISON_OPTIONS.none.ToString());
            }
            return sFileExtensionType;
        }
        private string MakeAnalysisFileExtensionTypeForDbUpdate(
            string filesToAnalyzeExtensionType)
        {
            //i.e. filetoanalyzeExttype = budget; analyzertype = stats01, comparisontype = none
            //= budgetstats01none
            string sFileExtensionType = string.Concat(filesToAnalyzeExtensionType,
               this.AnalyzerType.ToString(),
               this.HCCalculatorParams.AnalyzerParms.ComparisonType.ToString());
            return sFileExtensionType;
        }
        //sets linkedview (analysisdoc) props needed by client to load and display doc
        public void SetLinkedViewParams()
        {
            string sStylesheetName = GetAnalysisStyleSheet();
            //sStylesheetName will be used to find the Stylesheet and, if its a first time view, 
            //to set two more params: StylesheetResourceURIPattern and StylesheetDocPath 
            string sExistingStylesheetName
                = CalculatorHelpers.GetAttribute(this.HCCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            this.HCCalculatorParams.Stylesheet2Name
                = sStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.HCCalculatorParams.LinkedViewElement,
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
                = CalculatorHelpers.GetAttribute(this.HCCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            this.HCCalculatorParams.Stylesheet2ObjectNS
                = sStylesheetExtObjNamespace;
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.HCCalculatorParams.LinkedViewElement,
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
            else if (this.AnalyzerType == ANALYZER_TYPES.statistics01
                || IsEffectivenessAnalysis(this.AnalyzerType))
            {
                if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Invests.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Profits.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Operations.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Components.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Inputs.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats02Outputs.xslt";
                    }
                }
                else
                {
                    if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Invests.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Profits.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Operations.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Components.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Inputs.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
                    {
                        sAnalysisStyleSheet = "Stats01Outputs.xslt";
                    }
                }
                if (this.AnalyzerType
                    == ANALYZER_TYPES.effectiveness01a)
                {
                    sAnalysisStyleSheet
                        = sAnalysisStyleSheet.Replace("Stats01", "Units01a");
                    //or if comps
                    if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
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
                    if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet
                            = sAnalysisStyleSheet.Replace("Stats02", "Units02b");
                    }
                }
                else if (this.AnalyzerType
                    == ANALYZER_TYPES.effectiveness02a)
                {
                    sAnalysisStyleSheet
                        = sAnalysisStyleSheet.Replace("Stats01", "UnitHCAnalysis01a");
                    //or if comps
                    if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet
                            = sAnalysisStyleSheet.Replace("Stats02", "UnitHCAnalysis02a");
                    }
                }
                else if (this.AnalyzerType == ANALYZER_TYPES.effectiveness02b)
                {
                    sAnalysisStyleSheet = sAnalysisStyleSheet.Replace("Stats01", "UnitHCAnalysis01b");
                    //or if comps
                    if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
                        == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                    {
                        sAnalysisStyleSheet
                            = sAnalysisStyleSheet.Replace("Stats02", "UnitHCAnalysis02b");
                    }
                }
            }
            else if (this.AnalyzerType
                == ANALYZER_TYPES.resources01)
            {
                if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "HCAnalysis02Invests.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
                    {
                        sAnalysisStyleSheet = "HCAnalysis02Profits.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString())
                    {
                        sAnalysisStyleSheet = "HCAnalysis02Operations.xslt";
                    }
                    else if (this.HCCalculatorParams.StartingDocToCalcNodeName
                        == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
                    {
                        sAnalysisStyleSheet = "HCAnalysis02Components.xslt";
                    }
                }
                else
                {
                    if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                        || this.HCCalculatorParams.CalculatorType
                        == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksIns1.xslt";
                        }
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.output.ToString()
                        || this.HCCalculatorParams.CalculatorType
                        == HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksOuts1.xslt";
                        }
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.outcome.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksOutcomes1.xslt";
                        }
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.operation.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksOps1.xslt";
                        }
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksBuds1.xslt";
                        }
                        else
                        {
                            //using std parameters
                            if (this.HCCalculatorParams.RelatedCalculatorsType
                                != string.Empty)
                            {
                                sAnalysisStyleSheet = "HC1StocksBuds1.xslt";
                            }
                            else
                            {
                                //default to initial calculators
                                sAnalysisStyleSheet = "HC1StocksBuds1.xslt";
                            }
                        }
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.component.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksComps1.xslt";
                        }
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
                    {
                        if (this.HCCalculatorParams.RelatedCalculatorType
                            == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                        {
                            sAnalysisStyleSheet = "HC1StocksInvests1.xslt";
                        }
                        else
                        {
                            //using std parameters
                            if (this.HCCalculatorParams.RelatedCalculatorsType
                                != string.Empty)
                            {
                                sAnalysisStyleSheet = "HC1StocksInvests1.xslt";
                            }
                        }
                    }
                }
            }
            return sAnalysisStyleSheet;
        }
        private string GetAnalysisStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = string.Empty;
            if (this.HCCalculatorParams.AnalyzerParms.ComparisonType
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
    }
}
