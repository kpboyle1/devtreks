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
    ///Purpose:		Helper functions for the NPV calculators extension
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class NPVCalculatorHelper 
    {
        //constructor
        public NPVCalculatorHelper() { }
        public NPVCalculatorHelper(CalculatorParameters calcParameters)
        {
            //this is by ref and needs to pass back the new linkedview, and updates
            this.NPVCalculatorParams = calcParameters;
        }
        
        //properties
        //parameters needed by publishers
        public CalculatorParameters NPVCalculatorParams { get; set; }
        //constants specific to this extension (these are strictly examples 
        //showing how to use these types of constants)
        public enum CONSTANTS_TYPES
        {
            none                = 0,
            machineryconstant   = 1,
            priceconstant       = 2,
            randmconstant       = 3
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
        public static bool RunSchedulingCalculations(CalculatorParameters calcParameters)
        {
            bool bIsDone = false;
            //step two of operation and component npv calculator
            //has parameters that are used in labor and capital stock planning
            //clean up those parameters here
            TimelinessOpComp1 npvOC = new TimelinessOpComp1();
            //set the object's properties from the calcdoc
            npvOC.SetTimelinessOC1Properties(calcParameters.LinkedViewElement);
            //set the calcdoc from the object
            npvOC.SetTimelinessOC1Attributes(string.Empty, calcParameters.LinkedViewElement);
            bIsDone = true;
            return bIsDone;
        }
        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            //these calculators use a basic calculatorpatterns
            this.NPVCalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.basic;
            CalculatorHelpers.CALCULATOR_TYPES eCalculatorType
               = CalculatorHelpers.GetCalculatorType(this.NPVCalculatorParams.CalculatorType);
            //both calculators and analyzers both calculate a file in this path:
            this.NPVCalculatorParams.AnalyzerParms.ObservationsPath
                = this.NPVCalculatorParams.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            switch (eCalculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.input:
                    //prepare the event subscriber
                    NPVIOSubscriber subInput
                        = new NPVIOSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subInput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subInput.GCCalculatorParams);
                    subInput = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.output:
                    //prepare the event subscriber
                    NPVIOSubscriber subOutput
                        = new NPVIOSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subOutput.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subOutput.GCCalculatorParams);
                    subOutput = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.outcome:
                    //prepare the event subscriber
                    NPVOutcomeSubscriber subOutcome
                        = new NPVOutcomeSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subOutcome.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subOutcome.GCCalculatorParams);
                    subOutcome = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.operation:
                    //prepare the event subscriber
                    NPVOCSubscriber subOperation
                        = new NPVOCSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subOperation.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subOperation.GCCalculatorParams);
                    subOperation = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.operation2:
                    //prepare the event subscriber
                    NPVOCSubscriber subOperation2
                        = new NPVOCSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subOperation2.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subOperation2.GCCalculatorParams);
                    subOperation2 = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.component:
                    //prepare the event subscriber
                    NPVOCSubscriber subComponent
                        = new NPVOCSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subComponent.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subComponent.GCCalculatorParams);
                    subComponent = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.component2:
                    //prepare the event subscriber
                    NPVOCSubscriber subComponent2
                        = new NPVOCSubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subComponent2.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subComponent2.GCCalculatorParams);
                    subComponent2 = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.budget:
                    //prepare the event subscriber
                    NPVBISubscriber subBudget
                        = new NPVBISubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subBudget.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subBudget.GCCalculatorParams);
                    subBudget = null;
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.investment:
                    //prepare the event subscriber
                    NPVBISubscriber subInvestment
                        = new NPVBISubscriber(this.NPVCalculatorParams, eCalculatorType);
                    //run the analyses (raising the publisher's events for each node)
                    bHasCalculations = subInvestment.RunCalculator();
                    CalculatorHelpers.UpdateCalculatorParams(this.NPVCalculatorParams,
                        subInvestment.GCCalculatorParams);
                    subInvestment = null;
                    break;
                default:
                    break;
            }
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
            calcParameters.AnalyzerParms.ObservationsPath
                = calcParameters.ExtensionDocToCalcURI.URIDataManager.TempDocPath;
            //prepare the event subscriber
            NPVDevPacksSubscriber subDevPacks
                = new NPVDevPacksSubscriber(calcParameters);
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
            string sStylesheetName = GetCalculatorStyleSheet();
            //sStylesheetName will be used to find the Stylesheet and, if its a first time view, 
            //to set two more params: StylesheetResourceURIPattern and StylesheetDocPath 
            string sExistingStylesheetName
                = CalculatorHelpers.GetAttribute(this.NPVCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ResourceFileName);
            if (string.IsNullOrEmpty(sStylesheetName))
                sStylesheetName = sExistingStylesheetName;
            if ((string.IsNullOrEmpty(sExistingStylesheetName))
                || (!sExistingStylesheetName.Equals(sStylesheetName)))
            {
                CalculatorHelpers.SetAttribute(this.NPVCalculatorParams.LinkedViewElement,
                    Calculator1.cStylesheet2ResourceFileName, sStylesheetName);
            }
            //set the ss's extension object's namespace
            SetStylesheetNamespace();
        }
        private void SetStylesheetNamespace()
        {
            string sStylesheetExtObjNamespace
                = GetCalculatorStyleSheetExtObjNamespace();
            string sExistingStylesheetExtObjNamespace
                = CalculatorHelpers.GetAttribute(this.NPVCalculatorParams.LinkedViewElement,
                Calculator1.cStylesheet2ObjectNS);
            if ((string.IsNullOrEmpty(sExistingStylesheetExtObjNamespace))
                || (!sExistingStylesheetExtObjNamespace.Equals(sStylesheetExtObjNamespace)))
            {
                CalculatorHelpers.SetAttribute(this.NPVCalculatorParams.LinkedViewElement,
                    Calculator1.cStylesheet2ObjectNS,
                    sStylesheetExtObjNamespace);
            }
        }
        private string GetCalculatorStyleSheetExtObjNamespace()
        {
            string sStylesheetExtObjNamespace = "displaydevpacks";
            return sStylesheetExtObjNamespace;
        }
        private string GetCalculatorStyleSheet()
        {
            string sCalculatorStyleSheet = string.Empty;
            if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.none.ToString())
            {
                sCalculatorStyleSheet = string.Empty;
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString())
            {
                sCalculatorStyleSheet = "Budgets1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.component.ToString())
            {
                sCalculatorStyleSheet = "Components1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.component2.ToString())
            {
                sCalculatorStyleSheet = "Components1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.input.ToString())
            {
                sCalculatorStyleSheet = "Inputs1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
            {
                sCalculatorStyleSheet = "Investments1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.operation.ToString())
            {
                sCalculatorStyleSheet = "Operations1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.operation2.ToString())
            {
                sCalculatorStyleSheet = "Operations1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.output.ToString())
            {
                sCalculatorStyleSheet = "Outputs1.xslt";
            }
            else if (this.NPVCalculatorParams.CalculatorType == CalculatorHelpers.CALCULATOR_TYPES.outcome.ToString())
            {
                sCalculatorStyleSheet = "Outcomes1.xslt";
            }
            return sCalculatorStyleSheet;
        }
    }
}
