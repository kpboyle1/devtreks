using System;
using System.Xml.Linq;
using Errors = DevTreks.Exceptions.DevTreksErrors;
namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Helper functions for running agricultural calculators
    ///Author:		www.devtreks.org
    ///Date:		2014, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class AgBudgetingHelpers 
    {
        //constructor
        public AgBudgetingHelpers() { }
        public AgBudgetingHelpers(CalculatorParameters calcParameters)
        {
            this.ABCalculatorParams = calcParameters;
        }
        
        //properties
        //parameters needed by publishers
        public CalculatorParameters ABCalculatorParams { get; set; }

        public enum CALCULATOR_TYPES
        {
            none                = 0,
            agmachinery         = 1,
            capitalservices     = 2,
            irrpower            = 3,
            gencapital          = 4,
            lifecycle           = 5
        }
        public static CALCULATOR_TYPES GetCalculatorType(string calcType)
        {
            CALCULATOR_TYPES eCalculatorType = CALCULATOR_TYPES.none;
            if (calcType == CALCULATOR_TYPES.agmachinery.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.agmachinery;
            }
            else if (calcType == CALCULATOR_TYPES.irrpower.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.irrpower;
            }
            else if (calcType == CALCULATOR_TYPES.lifecycle.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.lifecycle;
            }
            else if (calcType == CALCULATOR_TYPES.gencapital.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.gencapital;
            }
            else if (calcType == CALCULATOR_TYPES.capitalservices.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.capitalservices;
            }
            return eCalculatorType;
        }
        //constants specific to this extension
        public enum CONSTANTS_TYPES
        {
            none = 0,
            machineryconstant = 1,
            priceconstant = 2,
            randmconstant = 3
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
            else if (calcParameters.StepNumber
                == ContractHelpers.EXTENSION_STEPS.stepthree.ToString())
            {
                //price constants were added to caldoc when the calculator 
                //form elements were added; nothing more needs to be done
                bHasConstants = true;
            }
            return bHasConstants;
        }
        public static bool SetLinkedListsState(CalculatorParameters calcParameters)
        {
            bool bIsDone = false;
            //step two passes in constants and other lists of data
            if (calcParameters.LinkedViewElement != null)
            {
                AddConstants(CONSTANTS_TYPES.machineryconstant, 
                    calcParameters);
                AddConstants(CONSTANTS_TYPES.randmconstant, 
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
            string sConstantsNodeQry = string.Empty;
            switch (constantType)
            {
                case CONSTANTS_TYPES.machineryconstant:
                    sConstantsIdAttName = Constants.MACHINERY_CONSTANTS_ID;
                    break;
                case CONSTANTS_TYPES.randmconstant:
                    sConstantsIdAttName = Constants.RANDM_ID;
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
        
        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            //these calculators use a basic calculatorpatterns
            this.ABCalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
            CALCULATOR_TYPES eCalculatorType
               = GetCalculatorType(this.ABCalculatorParams.CalculatorType);
            //both calculators and analyzers both calculate a file in this path:
            this.ABCalculatorParams.AnalyzerParms.ObservationsPath
                = this.ABCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            ABIOSubscriber subInput
                = new ABIOSubscriber(this.ABCalculatorParams, eCalculatorType);
            //note that running descendant calculations inserts a calculator 
            //with all of its attributes into the descendant, but the descendant 
            //may still need to have a calculation run
            switch (eCalculatorType)
            {
                case CALCULATOR_TYPES.agmachinery:
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.ABCalculatorParams, subInput.GCCalculatorParams);
                    break;
                case CALCULATOR_TYPES.irrpower:
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.ABCalculatorParams, subInput.GCCalculatorParams);
                    break;
                case CALCULATOR_TYPES.lifecycle:
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.ABCalculatorParams, subInput.GCCalculatorParams);
                    break;
                case CALCULATOR_TYPES.gencapital:
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.ABCalculatorParams, subInput.GCCalculatorParams);
                    break;
                case CALCULATOR_TYPES.capitalservices:
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.ABCalculatorParams, subInput.GCCalculatorParams);
                    break;
                default:
                    break;
            }
            subInput = null;
            //set parameters/attributes needed to update db and display this analysis
            SetCalculatorParameters();
            return bHasCalculations;
        }
        public bool RunDevPacksCalculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //these calculators use a basic calculatorpatterns
            calcParameters.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
            //both calculators and analyzers both calculate a file in this path:
            this.ABCalculatorParams.AnalyzerParms.ObservationsPath
                = calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            //prepare the event subscriber
            ABDevPacksSubscriber subDevPacks
                = new ABDevPacksSubscriber(calcParameters);
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
            //0.9.1 moved stylesheet management into linkedview attributes
            SetStylesheetNamespace();
        }
        private void SetStylesheetNamespace()
        {
            string sStylesheetExtObjNamespace
                = GetCalculatorStyleSheetExtObjNamespace();
            string sExistingStylesheetExtObjNamespace
                = CalculatorHelpers.GetAttribute(this.ABCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.ABCalculatorParams.LinkedViewElement,
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
