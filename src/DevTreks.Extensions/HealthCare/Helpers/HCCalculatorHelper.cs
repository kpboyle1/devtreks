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
    ///Purpose:		Helper functions for running resource stock calculators
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1.
    /// </summary>
    public class HCCalculatorHelper
    {
        //constructors
        public HCCalculatorHelper() { }
        public HCCalculatorHelper(CalculatorParameters calcParameters)
        {
            this.HCCalculatorParams = calcParameters;
            //step 1 of analyzers set this property based on selection
            if (!string.IsNullOrEmpty(calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType))
            {
                //reset calculatortype (uses filestoanalyze attribute in calculator)
                calcParameters.CalculatorType = calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType;
            }
        }

        //parameters needed by publishers
        public CalculatorParameters HCCalculatorParams { get; set; }
        //some analyzers in this project use the calculator pattern but still

        public enum CALCULATOR_TYPES
        {
            none = 0,
            //typical health insurance calculator (copays, deductibles) for inputs
            healthcost1         = 1,
            //health benefit calculator
            hcbenefit1 = 2
        }
        public static CALCULATOR_TYPES GetCalculatorType(string calculatorType)
        {
            CALCULATOR_TYPES eCalculatorType = CALCULATOR_TYPES.none;
            if (calculatorType == CALCULATOR_TYPES.healthcost1.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.healthcost1;
            }
            else if (calculatorType == CALCULATOR_TYPES.hcbenefit1.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.hcbenefit1;
            }
            return eCalculatorType;
        }
        //constants specific to this extension
        //these constants are here as examples
        public enum CONSTANTS_TYPES
        {
            none = 0,
            priceconstant = 1
        }
        public static CONSTANTS_TYPES GetConstantsType(string constantsType)
        {
            CONSTANTS_TYPES eConstantsType
                = (!string.IsNullOrEmpty(constantsType))
                ? (CONSTANTS_TYPES)Enum.Parse(
                typeof(CONSTANTS_TYPES), constantsType)
                : CONSTANTS_TYPES.none;
            return eConstantsType;
        }
        public static string GetNodeNameFromFileExtensionType(
            string fileExtensionType)
        {
            string sStartingNodeToCalc = string.Empty;
            if (fileExtensionType ==
                CALCULATOR_TYPES.healthcost1.ToString())
            {
                sStartingNodeToCalc = Input.INPUT_PRICE_TYPES.inputgroup.ToString();
            }
            else if (fileExtensionType ==
                CALCULATOR_TYPES.hcbenefit1.ToString())
            {
                sStartingNodeToCalc = Output.OUTPUT_PRICE_TYPES.outputgroup.ToString();
            }
            return sStartingNodeToCalc;
        }
        public static bool SetConstants(CalculatorParameters calcParameters)
        {
            bool bHasConstants = false;
            if (calcParameters.StepNumber
                == ContractHelpers.EXTENSION_STEPS.steptwo.ToString())
            {
                //load locals (can check the node to make sure its needed)
                bHasConstants = CalculatorHelpers.SetLinkedLocalsListsState(
                    calcParameters);
                //constants and other linked lists specific to this extension
                bHasConstants = SetLinkedListsState(calcParameters);
            }
            return bHasConstants;
        }
        public static bool SetLinkedListsState(CalculatorParameters calcParameters)
        {
            bool bIsDone = false;
            //step two passes in constants and other lists of data
            if (calcParameters.LinkedViewElement != null)
            {
                AddConstants(CONSTANTS_TYPES.priceconstant,
                    calcParameters);
                //save the tempdoc with changes and bIsDone = true
                bIsDone = CalculatorHelpers.SaveNewCalculationsDocument(
                    calcParameters);
            }
            else
            {
                calcParameters.ErrorMessage
                    = Errors.MakeStandardErrorMsg("CALCSHELPER_FILE_NOTFOUND");
            }
            return bIsDone;
        }
        public static void AddConstants(CONSTANTS_TYPES constantType,
            CalculatorParameters calcParameters)
        {
            string sConstantsIdAttName = string.Empty;
            string sConstantsFullDocPath = string.Empty;
            string sConstantsId = string.Empty;
            switch (constantType)
            {
                case CONSTANTS_TYPES.priceconstant:
                    sConstantsIdAttName = Constants.PRICE_CONSTANTS_ID;
                    break;
                default:
                    break;
            }
            sConstantsFullDocPath = CalculatorHelpers.GetLinkedListPath(
                calcParameters, constantType.ToString());
            //use the calculator node constant id fields to find the correct node
            sConstantsId
                = CalculatorHelpers.GetAttribute(
                    calcParameters.LinkedViewElement, sConstantsIdAttName);
            if (!string.IsNullOrEmpty(sConstantsId))
            {
                string sErrorMsg = string.Empty;
                CalculatorHelpers.GetResourceURIPath(calcParameters.ExtensionDocToCalcURI,
                    sConstantsFullDocPath, sErrorMsg);
                calcParameters.ErrorMessage = sErrorMsg;
                if (string.IsNullOrEmpty(sErrorMsg))
                {
                    XElement rootConstants = CalculatorHelpers.LoadXElement(calcParameters.ExtensionDocToCalcURI,
                        sConstantsFullDocPath);
                    if (rootConstants != null)
                    {
                        //add the remaining constants attributes
                        CalculatorHelpers.AddAttributesWithoutIdNameDesc(
                            constantType.ToString(), sConstantsId,
                            rootConstants, calcParameters.LinkedViewElement);
                    }
                }
            }
        }
        public bool RunInputOrOutputCalculations()
        {
            bool bHasCalculations = false;
            CALCULATOR_TYPES eCalculatorType
               = GetCalculatorType(this.HCCalculatorParams.CalculatorType);
            this.HCCalculatorParams.RunCalculatorType
               = CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology;
            if (this.HCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.inputprices.ToString()
                || this.HCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
            {
                this.HCCalculatorParams.RunCalculatorType
                    = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
            }
            this.HCCalculatorParams.AnalyzerParms.ObservationsPath
                = this.HCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            //note that running descendant calculations inserts a calculator 
            //with all of its attributes into the descendant, but the descendant 
            //may still need to have a calculation run
            switch (eCalculatorType)
            {
                case CALCULATOR_TYPES.healthcost1:
                    IOHCStockSubscriber subInput
                         = new IOHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams,
                        subInput.GCCalculatorParams);
                    break;
                case CALCULATOR_TYPES.hcbenefit1:
                    IOHCStockSubscriber subOutput
                         = new IOHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subOutput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams,
                        subOutput.GCCalculatorParams);
                    break;
                default:
                    break;
            }
            //set parameters/attributes needed to update db and display this analysis
            SetCalculatorParameters();
            return bHasCalculations;
        }

        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            //these calculators use a mix of calculator and analyzer patterns
            this.HCCalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.HCCalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.HCCalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.HCCalculatorParams.ExtensionDocToCalcURI,
                this.HCCalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.HCCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            if (this.HCCalculatorParams.CalculatorType
                == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                || this.HCCalculatorParams.CalculatorType
                    == CALCULATOR_TYPES.healthcost1.ToString()
                || this.HCCalculatorParams.CalculatorType
                    == CalculatorHelpers.CALCULATOR_TYPES.output.ToString()
                || this.HCCalculatorParams.CalculatorType
                    == CALCULATOR_TYPES.hcbenefit1.ToString())
            {
                this.HCCalculatorParams.RunCalculatorType
                    = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
                if (this.HCCalculatorParams.RelatedCalculatorType
                    == CALCULATOR_TYPES.healthcost1.ToString())
                {
                    IOHCStockSubscriber subInput
                        = new IOHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subInput.GCCalculatorParams);
                    subInput = null;
                }
                else if (this.HCCalculatorParams.RelatedCalculatorType
                    == CALCULATOR_TYPES.hcbenefit1.ToString())
                {
                    IOHCStockSubscriber subOutput
                        = new IOHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subOutput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subOutput.GCCalculatorParams);
                    subOutput = null;
                }
                else
                {
                    //don't rely on just related calcs type
                    if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                        || this.HCCalculatorParams.CalculatorType
                        == CALCULATOR_TYPES.healthcost1.ToString())
                    {
                        IOHCStockSubscriber subInput
                        = new IOHCStockSubscriber(this.HCCalculatorParams);
                        bHasCalculations = subInput.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subInput.GCCalculatorParams);
                        subInput = null;
                    }
                    else if (this.HCCalculatorParams.CalculatorType
                        == CalculatorHelpers.CALCULATOR_TYPES.output.ToString()
                        || this.HCCalculatorParams.CalculatorType
                        == CALCULATOR_TYPES.hcbenefit1.ToString())
                    {
                        IOHCStockSubscriber subOutput
                        = new IOHCStockSubscriber(this.HCCalculatorParams);
                        bHasCalculations = subOutput.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subOutput.GCCalculatorParams);
                        subOutput = null;
                    }
                }
            }
            else if (this.HCCalculatorParams.CalculatorType
                == CalculatorHelpers.CALCULATOR_TYPES.outcome.ToString())
            {
                if (this.HCCalculatorParams.RelatedCalculatorType != string.Empty
                    && this.HCCalculatorParams.RelatedCalculatorType
                    != Constants.NONE)
                {
                    if (this.HCCalculatorParams.RelatedCalculatorType
                       == CALCULATOR_TYPES.hcbenefit1.ToString())
                    {
                        OutcomeHCStockSubscriber subOutcome
                            = new OutcomeHCStockSubscriber(this.HCCalculatorParams);
                        bHasCalculations = subOutcome.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subOutcome.GCCalculatorParams);
                        subOutcome = null;
                    }
                }
                else
                {
                    OutcomeHCStockSubscriber subOutcome
                            = new OutcomeHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subOutcome.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subOutcome.GCCalculatorParams);
                    subOutcome = null;
                }
            }
            else if (this.HCCalculatorParams.CalculatorType
                == CalculatorHelpers.CALCULATOR_TYPES.operation.ToString()
                || this.HCCalculatorParams.CalculatorType
                == CalculatorHelpers.CALCULATOR_TYPES.component.ToString())
            {
                if (this.HCCalculatorParams.RelatedCalculatorType != string.Empty
                    && this.HCCalculatorParams.RelatedCalculatorType
                    != Constants.NONE)
                {
                    if (this.HCCalculatorParams.RelatedCalculatorType
                       == CALCULATOR_TYPES.healthcost1.ToString())
                    {
                        OCHCStockSubscriber subOperation
                            = new OCHCStockSubscriber(this.HCCalculatorParams);
                        bHasCalculations = subOperation.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subOperation.GCCalculatorParams);
                        subOperation = null;
                    }
                }
                else
                {
                    OCHCStockSubscriber subOperation
                            = new OCHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subOperation.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subOperation.GCCalculatorParams);
                    subOperation = null;
                }
            }
            else if (this.HCCalculatorParams.CalculatorType
                == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString()
                || this.HCCalculatorParams.CalculatorType
                == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
            {
                if (this.HCCalculatorParams.RelatedCalculatorType
                    == CALCULATOR_TYPES.healthcost1.ToString())
                {
                    BIHCStockSubscriber subBudget
                        = new BIHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subBudget.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subBudget.GCCalculatorParams);
                    subBudget = null;
                }
                else if (this.HCCalculatorParams.RelatedCalculatorType
                    == CALCULATOR_TYPES.hcbenefit1.ToString())
                {
                    BIHCStockSubscriber subBudget
                        = new BIHCStockSubscriber(this.HCCalculatorParams);
                    bHasCalculations = subBudget.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subBudget.GCCalculatorParams);
                    subBudget = null;
                }
                else
                {
                    if (this.HCCalculatorParams.RelatedCalculatorsType != string.Empty)
                    {
                        //set by analyzer or by user 
                        BIHCStockSubscriber subBudget
                       = new BIHCStockSubscriber(this.HCCalculatorParams);
                        bHasCalculations = subBudget.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subBudget.GCCalculatorParams);
                        subBudget = null;
                    }
                    else
                    {
                        //set by analyzer or by user 
                        BIHCStockSubscriber subBudget
                       = new BIHCStockSubscriber(this.HCCalculatorParams);
                        bHasCalculations = subBudget.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.HCCalculatorParams, subBudget.GCCalculatorParams);
                        subBudget = null;
                    }
                }
            }
            //stylesheet set in analyzerhelper
            return bHasCalculations;
        }

        public bool RunDevPacksCalculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //these calculators use a mixed calculatorpatterns
            calcParameters.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology;
            if (calcParameters.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.inputprices.ToString()
                || calcParameters.ExtensionDocToCalcURI.URIDataManager.SubAppType
                == Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
            {
                calcParameters.RunCalculatorType
                    = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
            }
            //both calculators and analyzers both calculate a file in this path:
            calcParameters.AnalyzerParms.ObservationsPath
                = calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            //prepare the event subscriber
            HC1DevPacksSubscriber subDevPacks
                = new HC1DevPacksSubscriber(calcParameters);
            //run the analyses (raising the publisher's events for each node)
            bHasCalculations = subDevPacks.RunDevPackCalculator();
            CalculatorHelpers.UpdateCalculatorParams(
                calcParameters, subDevPacks.GCCalculatorParams);
            subDevPacks = null;
            return bHasCalculations;
        }
        //0.8.8a: hardcoding stylesheet2 name is more reliable than strictly relying 
        //on the baselinkedview attribute (especially with interrelated calculators)
        private void SetCalculatorParameters()
        {
            //set the linkedview params needed to load and display the analysis
            SetLinkedViewParams();
        }

        //sets linkedview (analysisdoc) props needed by client to load and display doc
        public void SetLinkedViewParams()
        {
            //set the ss's extension object's namespace
            SetStylesheetNamespace();
        }
        private void SetStylesheetNamespace()
        {
            string sStylesheetExtObjNamespace
                = GetCalculatorStyleSheetExtObjNamespace();
            string sExistingStylesheetExtObjNamespace
                = CalculatorHelpers.GetAttribute(this.HCCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.HCCalculatorParams.LinkedViewElement,
                    Calculator1.cStylesheet2ObjectNS,
                    sStylesheetExtObjNamespace);
            }
        }
        private string GetCalculatorStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = "displaydevpacks";
            return sStylesheetExtObjNamespace;
        }
    }
}