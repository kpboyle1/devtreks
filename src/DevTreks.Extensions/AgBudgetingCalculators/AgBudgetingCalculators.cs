using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;
namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		economics calculators target agricultural sector
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///Notes:       Must implement the RunCalculatorStep and RunAnalyzerStep interface 
    ///             methods from the inherted DoStepsHostView class. That class defines 
    ///             the host's view of Extensions. 
    /// </summary>
    ///NOTES: 
    ///         1. These calculators generate basic totals for their respective 
    ///         (i.e. inputs, outputs) uris. These basic totals include variables, 
    ///         such as fuel consumed, that are subsequently
    ///         used by analyzers (multiple uri analyses). The analyzers find these 
    ///         base calculation uris by looking for a common 
    ///         uri.URIFileExtensionType attribute in the file name. Remote uris 
    ///         haven't been addressed yet.
    ///         In this addin, the parameter is first hardcoded in the base calculators, 
    ///         then added to the db update list, updated in the database, and subsequently 
    ///         used in the uri.urifileextensiontype property.
    [DoStepsAttribute(CONTRACT_TYPE = CALULATOR_CONTRACT_TYPES.defaultcalculatormanager,
        CalculatorsExtensionName = "AgBudgetingCalculators")]
    public class AgBudgetingCalculators : DoStepsHostView
    {
        public override async Task<bool> RunCalculatorStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasCalculation = false;
            CalculatorHelpers eCalcHelpers = new CalculatorHelpers();
            CalculatorParameters ABCalcParams 
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze, 
                updates);
            AgBudgetingHelpers agBudgetHelpers
                = new AgBudgetingHelpers(ABCalcParams);
            ContractHelpers.EXTENSION_STEPS eStepNumber
                = ContractHelpers.GetEnumStepNumber(stepNumber);
            switch (eStepNumber)
            {
                case ContractHelpers.EXTENSION_STEPS.stepzero:
                    bHasCalculation = true;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepone:
                    bHasCalculation = true;
                    break;
                case ContractHelpers.EXTENSION_STEPS.steptwo:
                    //clear updates collection
                    updates.Clear();
                    if (ABCalcParams != null)
                    {
                        //set constants for this step
                        bHasCalculation 
                            = AgBudgetingHelpers.SetConstants(ABCalcParams);
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepthree:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(ABCalcParams, stepNumber, updates);
                    if (ABCalcParams != null)
                    {
                        bHasCalculation 
                            = AgBudgetingHelpers.SetConstants(ABCalcParams);
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfour:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(ABCalcParams, stepNumber, updates);
                    if (ABCalcParams != null)
                    {
                        if (ABCalcParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                            != Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
                        {
                            //run the calculations 
                            bHasCalculation = agBudgetHelpers.RunCalculations();
                        }
                        else
                        {
                            //run custom document calculations
                            bHasCalculation 
                                = agBudgetHelpers.RunDevPacksCalculations(ABCalcParams);
                        }
                        if (!bHasCalculation)
                        {
                            extDocToCalcURI.ErrorMessage = (extDocToCalcURI.ErrorMessage == string.Empty) ?
                                Errors.MakeStandardErrorMsg("CALCULATORS_URI_MISMATCH")
                                : extDocToCalcURI.ErrorMessage;
                            return bHasCalculation;
                        }
                        if (string.IsNullOrEmpty(extDocToCalcURI.ErrorMessage))
                        {
                            //two step calculators need to be saved now
                            AgBudgetingHelpers.CALCULATOR_TYPES eCalculatorType
                                = AgBudgetingHelpers.GetCalculatorType(
                                ABCalcParams.CalculatorType);
                            CheckForLastStepCalculator(eCalculatorType,
                                eStepNumber, extDocToCalcURI);
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bHasCalculation
                                = CalculatorHelpers.SaveNewCalculationsDocument(ABCalcParams);
                        }
                        else
                        {
                            bHasCalculation = false;
                        }
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = true;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfive:
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = true;
                    extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(ABCalcParams.LinkedViewElement,
                            Calculator1.cFileExtensionType);
                    bHasCalculation = true;
                    //tells addinhelper to save calcs
                    CalculatorHelpers.SetTempDocSaveCalcsProperty(extDocToCalcURI);
                    break;
                default:
                    //as many steps as needed can be added to this addin
                    break;
            }
            extDocToCalcURI.ErrorMessage += ABCalcParams.ErrorMessage;
            return bHasCalculation;
        }
        private static void CheckForLastStepCalculator(
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType, 
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            //if (calculatorType
            //    == AgBudgetingHelpers.CALCULATOR_TYPES.locals)
            //{
            //    if (stepNumber == ContractHelpers.EXTENSION_STEPS.stepthree)
            //    {
            //        //locals use 2 steps
            //        saveTempDocInDb = true;
            //    }
            //}
            //else -remaining calculators in this extension use 4 steps
        }
        //implement the interface, but this is not an analyzers addin
        public override async Task<bool> RunAnalyzerStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            return false;
        }
    }
}
