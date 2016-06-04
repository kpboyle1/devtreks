using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Run both calculators and analyzers for net present value analysis data
    ///Author:		www.devtreks.org
    ///Date:		2015, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES: 
    ///         1. This release includes NPV calculators and analyzers
    /// </summary>

    //custom exportattribute which inherits from DoStepsHostView 
    //(note that these attributes will change in the future)
    [DoStepsAttribute(CONTRACT_TYPE = CALULATOR_CONTRACT_TYPES.defaultcalculatormanager,
        CalculatorsExtensionName = "NPV1")]
    public class NPV1 : DoStepsHostView
    {
        public override async Task<bool> RunCalculatorStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasCalculation = false;
            CalculatorHelpers eCalcHelpers = new CalculatorHelpers();
            CalculatorParameters NPV1CalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            NPV1CalculatorHelper npv1BudgetHelper
                = new NPV1CalculatorHelper(NPV1CalcParams);
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
                    if (NPV1CalcParams != null)
                    {
                        //set constants for this step
                        bHasCalculation
                            = NPV1CalculatorHelper.SetConstants(NPV1CalcParams);
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepthree:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(NPV1CalcParams, stepNumber, updates);
                    if (NPV1CalcParams != null)
                    {
                        //no constants; just an xml edit of NRImpacts
                        bHasCalculation = true;
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfour:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(NPV1CalcParams, stepNumber, updates);
                    if (NPV1CalcParams != null)
                    {
                        if (NPV1CalcParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                            != Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
                        {
                            //run the calculations 
                            bHasCalculation = npv1BudgetHelper.RunNPV1CalculatorCalculations();
                        }
                        else
                        {
                            //run custom document calculations
                            bHasCalculation
                                = npv1BudgetHelper.RunDevPacksCalculations(NPV1CalcParams);
                        }
                        extDocToCalcURI.ErrorMessage = NPV1CalcParams.ErrorMessage;
                        extDocToCalcURI.ErrorMessage += NPV1CalcParams.ExtensionDocToCalcURI.ErrorMessage;
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
                            CheckForLastStepCalculator(NPV1CalcParams,
                                eStepNumber, extDocToCalcURI);
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bHasCalculation
                                = CalculatorHelpers.SaveNewCalculationsDocument(NPV1CalcParams);
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
                    extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(NPV1CalcParams.LinkedViewElement,
                            Calculator1.cFileExtensionType);
                    bHasCalculation = true;
                    //tells addinhelper to save calcs
                    CalculatorHelpers.SetTempDocSaveCalcsProperty(extDocToCalcURI);
                    break;
                default:
                    //as many steps as needed can be added to this addin
                    break;
            }
            extDocToCalcURI.ErrorMessage += NPV1CalcParams.ErrorMessage;
            return bHasCalculation;
        }
        private static void CheckForLastStepCalculator(
            CalculatorParameters npv1CalcParams,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            NPV1CalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = NPV1CalculatorHelper.GetCalculatorType(
                npv1CalcParams.CalculatorType);
            //other projects have code for handling different
            //numbers of steps in calculators
        }
        public override async Task<bool> RunAnalyzerStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasAnalysis = false;
            CalculatorParameters npv1CalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            NPV1AnalyzerHelper npv1AnalyzerHelper = new NPV1AnalyzerHelper(npv1CalcParams);
            //check to make sure the analyzer can be run
            npv1AnalyzerHelper.SetOptions();
            if (npv1AnalyzerHelper.NPV1CalculatorParams.ErrorMessage != string.Empty)
            {
                extDocToCalcURI.ErrorMessage += npv1AnalyzerHelper.NPV1CalculatorParams.ErrorMessage;
                return false;
            }
            ContractHelpers.EXTENSION_STEPS eStepNumber
                = ContractHelpers.GetEnumStepNumber(stepNumber);
            switch (eStepNumber)
            {
                case ContractHelpers.EXTENSION_STEPS.stepzero:
                    bHasAnalysis = true;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepone:
                    bHasAnalysis = true;
                    break;
                case ContractHelpers.EXTENSION_STEPS.steptwo:
                    //clear updates collection
                    updates.Clear();
                    //just need the html form edits in this step
                    bHasAnalysis = true;
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepthree:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(npv1CalcParams, stepNumber, updates);
                    //linked view insertions needs some analysis parameters
                    SetAnalyzerParameters(npv1AnalyzerHelper, npv1CalcParams);
                    if (npv1CalcParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.devpacks)
                    {
                        bHasAnalysis = npv1AnalyzerHelper.RunAnalysis(npv1CalcParams.UrisToAnalyze);
                    }
                    else
                    {
                        bHasAnalysis = npv1AnalyzerHelper.RunAnalysis();
                    }
                    extDocToCalcURI.ErrorMessage = npv1CalcParams.ErrorMessage;
                    extDocToCalcURI.ErrorMessage += npv1AnalyzerHelper.NPV1CalculatorParams.ErrorMessage;
                    if (!bHasAnalysis)
                    {
                        extDocToCalcURI.ErrorMessage = (extDocToCalcURI.ErrorMessage == string.Empty) ?
                            Errors.MakeStandardErrorMsg("CALCULATORS_URI_MISMATCH")
                            : extDocToCalcURI.ErrorMessage;
                        return bHasAnalysis;
                    }
                    if (string.IsNullOrEmpty(extDocToCalcURI.ErrorMessage))
                    {
                        //two step analyzers need to be saved now
                        NPV1AnalyzerHelper.ANALYZER_TYPES eAnalyzerType
                            = npv1AnalyzerHelper.GetAnalyzerType(
                            npv1AnalyzerHelper.NPV1CalculatorParams.AnalyzerParms.AnalyzerType);
                        //when 3+ step analyzers start being used
                        CheckForLastStepAnalyzer(eAnalyzerType,
                            eStepNumber, extDocToCalcURI);
                        if (!CalculatorHelpers.IsSaveAction(extDocToCalcURI))
                        {
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bool bHasReplacedCalcDoc
                                = CalculatorHelpers.SaveNewCalculationsDocument(
                                npv1AnalyzerHelper.NPV1CalculatorParams);
                            if (bHasReplacedCalcDoc)
                            {
                                bHasAnalysis = true;
                            }
                            else
                            {
                                extDocToCalcURI.ErrorMessage = Errors.MakeStandardErrorMsg("ANALYSES_ID_MISMATCH");
                            }
                        }
                    }
                    else
                    {
                        bHasAnalysis = false;
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = true;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfour:
                    bHasAnalysis = true;
                    if (npv1AnalyzerHelper.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1
                        || npv1AnalyzerHelper.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1
                        || npv1AnalyzerHelper.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr
                        || npv1AnalyzerHelper.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid
                        || npv1AnalyzerHelper.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt
                        || npv1AnalyzerHelper.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1)
                    {
                        if (npv1CalcParams.NeedsFullView)
                        {
                            extDocToCalcURI.URIDataManager.NeedsFullView = true;
                            extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                        }
                        else
                        {
                            extDocToCalcURI.URIDataManager.NeedsFullView = false;
                            extDocToCalcURI.URIDataManager.NeedsSummaryView = true;
                        }
                        extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(npv1CalcParams.LinkedViewElement,
                            Calculator1.cFileExtensionType);
                    }
                    if (extDocToCalcURI.URIDataManager.TempDocSaveMethod
                        == Constants.SAVECALCS_METHOD.saveastext.ToString())
                    {
                        //analyzers aren't yet available
                    }
                    else
                    {
                        //tells addinhelper to save calcs
                        CalculatorHelpers.SetTempDocSaveAnalysesProperty(extDocToCalcURI);
                    }
                    break;
                default:
                    //as many steps as needed can be added to this addin
                    break;
            }
            extDocToCalcURI.ErrorMessage += npv1AnalyzerHelper.NPV1CalculatorParams.ErrorMessage;
            return bHasAnalysis;
        }
        private static void CheckForLastStepAnalyzer(
            NPV1AnalyzerHelper.ANALYZER_TYPES analyzerType,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            //this release only includes three step analyzers
        }
        private static void SetAnalyzerParameters(
            NPV1AnalyzerHelper npv1AnalyzerHelper, CalculatorParameters npv1CalcParams)
        {
            npv1AnalyzerHelper.SetAnalysisParameters();
            npv1CalcParams.FileExtensionType
                = npv1AnalyzerHelper.NPV1CalculatorParams.FileExtensionType;
            npv1CalcParams.Stylesheet2Name
                = npv1AnalyzerHelper.NPV1CalculatorParams.Stylesheet2Name;
            npv1CalcParams.Stylesheet2ObjectNS
                = npv1AnalyzerHelper.NPV1CalculatorParams.Stylesheet2ObjectNS;
        }
    }
}
