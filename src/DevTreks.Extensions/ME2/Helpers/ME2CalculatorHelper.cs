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
    ///Purpose:		Helper functions for running resource stock calculators
    ///Author:		www.devtreks.org
    ///Date:		2014, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1.
    /// </summary>
    public class ME2CalculatorHelper
    {
        //constructors
        public ME2CalculatorHelper() { }
        public ME2CalculatorHelper(CalculatorParameters calcParameters)
        {
            this.ME2CalculatorParams = calcParameters;
        }

        //parameters needed by publishers
        public CalculatorParameters ME2CalculatorParams { get; set; }

        public enum CALCULATOR_TYPES
        {
            none            = 0,
            //benefit and costs being analyzed
            me2             = 1
        }
        public static CALCULATOR_TYPES GetCalculatorType(string calculatorType)
        {
            CALCULATOR_TYPES eCalculatorType = CALCULATOR_TYPES.none;
            if (calculatorType == CALCULATOR_TYPES.me2.ToString())
            {
                eCalculatorType = CALCULATOR_TYPES.me2;
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
        public bool RunME2CalculatorCalculations()
        {
            bool bHasCalculations = false;
            CALCULATOR_TYPES eCalculatorType
               = GetCalculatorType(this.ME2CalculatorParams.CalculatorType);
            this.ME2CalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
            this.ME2CalculatorParams.AnalyzerParms.ObservationsPath
                = this.ME2CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            //note that running descendant calculations inserts a calculator 
            //with all of its attributes into the descendant, but the descendant 
            //may still need to have a calculation run
            switch (eCalculatorType)
            {
                case CALCULATOR_TYPES.me2:
                    if (this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.budgets
                        || this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        BIME2StockSubscriber subME2Calculator
                            = new BIME2StockSubscriber(this.ME2CalculatorParams);
                        bHasCalculations = subME2Calculator.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.ME2CalculatorParams, subME2Calculator.GCCalculatorParams);
                    }
                    else if (this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        OutcomeME2StockSubscriber subME2Calculator
                            = new OutcomeME2StockSubscriber(this.ME2CalculatorParams);
                        bHasCalculations = subME2Calculator.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.ME2CalculatorParams, subME2Calculator.GCCalculatorParams);
                    }
                    else if (this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.operationprices
                        || this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.componentprices)
                    {
                        OCME2StockSubscriber subME2Calculator
                             = new OCME2StockSubscriber(this.ME2CalculatorParams);
                        //build the me2s object collections
                        bHasCalculations = subME2Calculator.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.ME2CalculatorParams, subME2Calculator.GCCalculatorParams);
                    }
                    else if (this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices
                        || this.ME2CalculatorParams.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.outputprices)
                    {
                        IOME2StockSubscriber subME2Calculator
                            = new IOME2StockSubscriber(this.ME2CalculatorParams);
                        bHasCalculations = subME2Calculator.RunCalculator();
                        CalculatorHelpers.UpdateCalculatorParams(this.ME2CalculatorParams, subME2Calculator.GCCalculatorParams);

                    }
                    break;
                default:
                    break;
            }
            //set parameters/attributes needed to update db and display this analysis
            SetCalculatorParameters();
            return bHasCalculations;
        }
        //public bool RunME2CalculatorCalculations()
        //{
        //    bool bHasCalculations = false;
        //    CALCULATOR_TYPES eCalculatorType
        //       = GetCalculatorType(this.ME2CalculatorParams.CalculatorType);
        //    this.ME2CalculatorParams.RunCalculatorType
        //        = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
        //    this.ME2CalculatorParams.AnalyzerParms.ObservationsPath
        //        = this.ME2CalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
        //    //note that running descendant calculations inserts a calculator 
        //    //with all of its attributes into the descendant, but the descendant 
        //    //may still need to have a calculation run
        //    IOME2StockSubscriber subME2Calculator
        //        = new IOME2StockSubscriber(this.ME2CalculatorParams);
        //    //switch (eCalculatorType)
        //    //{
        //    //    case CALCULATOR_TYPES.buildcost1:
        //    //        bHasCalculations = subME2Calculator.RunCalculator();
        //    //        break;
        //    //    case CALCULATOR_TYPES.buildbenefit1:
        //    //        bHasCalculations = subME2Calculator.RunCalculator();
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //    CalculatorHelpers.UpdateCalculatorParams(this.ME2CalculatorParams, subME2Calculator.GCCalculatorParams);
        //    //set parameters/attributes needed to update db and display this analysis
        //    SetCalculatorParameters();
        //    return bHasCalculations;
        //}

       
        public bool RunDevPacksCalculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //these calculators use a mixed calculatorpatterns
            calcParameters.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects;
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
            DevPacksME2Subscriber subDevPacks
                = new DevPacksME2Subscriber(calcParameters);
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
                = CalculatorHelpers.GetAttribute(this.ME2CalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.ME2CalculatorParams.LinkedViewElement,
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