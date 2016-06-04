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
    ///Purpose:		Run both calculators and analyzers for food nutrition data
    ///Author:		www.devtreks.org
    ///Date:		2015, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES: 
    ///         1. This release introduces health care calculators and analyzers
    /// </summary>

    //custom exportattribute which inherits from DoStepsHostView 
    //(note that these attributes will change in the future)
    [DoStepsAttribute(CONTRACT_TYPE = CALULATOR_CONTRACT_TYPES.defaultcalculatormanager,
        CalculatorsExtensionName = "HealthCare")]
    public class HealthCare : DoStepsHostView
    {
        public override async Task<bool> RunCalculatorStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasCalculation = false;
            CalculatorHelpers eCalcHelpers = new CalculatorHelpers();
            CalculatorParameters HCCalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            HCCalculatorHelper hcBudgetHelper
                = new HCCalculatorHelper(HCCalcParams);
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
                    if (HCCalcParams != null)
                    {
                        //set constants for this step
                        bHasCalculation
                            = HCCalculatorHelper.SetConstants(HCCalcParams);
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepthree:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(HCCalcParams, stepNumber, updates);
                    //just save this step's edits
                    bHasCalculation = true;
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfour:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(HCCalcParams, stepNumber, updates);
                    //just save this step's edits
                    bHasCalculation = true;
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfive:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(HCCalcParams, stepNumber, updates);
                    if (HCCalcParams != null)
                    {
                        if (HCCalcParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                            != Constants.SUBAPPLICATION_TYPES.devpacks.ToString())
                        {
                            //run the calculations 
                            bHasCalculation = hcBudgetHelper.RunInputOrOutputCalculations();
                        }
                        else
                        {
                            //run custom document calculations
                            bHasCalculation
                                = hcBudgetHelper.RunDevPacksCalculations(HCCalcParams);
                        }
                        extDocToCalcURI.ErrorMessage = HCCalcParams.ErrorMessage;
                        extDocToCalcURI.ErrorMessage += HCCalcParams.ExtensionDocToCalcURI.ErrorMessage;
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
                            CheckForLastStepCalculator(HCCalcParams,
                                eStepNumber, extDocToCalcURI);
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bHasCalculation
                                = CalculatorHelpers.SaveNewCalculationsDocument(HCCalcParams);
                        }
                        else
                        {
                            bHasCalculation = false;
                        }
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = true;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepsix:
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = true;
                    extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(HCCalcParams.LinkedViewElement,
                            Calculator1.cFileExtensionType);
                    bHasCalculation = true;
                    //tells addinhelper to save calcs
                    CalculatorHelpers.SetTempDocSaveCalcsProperty(extDocToCalcURI);
                    break;
                default:
                    //as many steps as needed can be added to this addin
                    break;
            }
            extDocToCalcURI.ErrorMessage += HCCalcParams.ErrorMessage;
            return bHasCalculation;
        }
        private static void CheckForLastStepCalculator(
            CalculatorParameters hcCalcParams,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            HCCalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = HCCalculatorHelper.GetCalculatorType(
                hcCalcParams.CalculatorType);
            //other projects have code for handling different
            //numbers of steps in calculators
        }
        public override async Task<bool> RunAnalyzerStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasAnalysis = false;
            CalculatorParameters hcCalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            HCAnalyzerHelper hcAnalyzerHelper = new HCAnalyzerHelper(hcCalcParams);
            //check to make sure the analyzer can be run
            hcAnalyzerHelper.SetOptions();
            if (hcAnalyzerHelper.HCCalculatorParams.ErrorMessage != string.Empty)
            {
                extDocToCalcURI.ErrorMessage += hcAnalyzerHelper.HCCalculatorParams.ErrorMessage;
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
                    CalculatorHelpers.RefreshUpdates(hcCalcParams, stepNumber, updates);
                    if (hcAnalyzerHelper.AnalyzerType
                        == HCAnalyzerHelper.ANALYZER_TYPES.resources01)
                    {
                        //linked view insertions needs some analysis parameters
                        SetAnalyzerParameters(hcAnalyzerHelper, hcCalcParams);
                        //the calculator pattern handles children linked view insertions
                        //better than the analyzer pattern
                        HCCalculatorHelper hcCalculatorHelper
                            = new HCCalculatorHelper(hcCalcParams);
                        bHasAnalysis = hcCalculatorHelper
                            .RunCalculations();
                    }
                    else
                    {
                        //run the analysis (when analyses are available)
                        //bHasAnalysis = hcAnalyzerHelper
                        //    .RunAnalysis(urisToAnalyze);
                    }
                    extDocToCalcURI.ErrorMessage = hcCalcParams.ErrorMessage;
                    extDocToCalcURI.ErrorMessage += hcAnalyzerHelper.HCCalculatorParams.ErrorMessage;
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
                        HCAnalyzerHelper.ANALYZER_TYPES eAnalyzerType
                            = hcAnalyzerHelper.GetAnalyzerType(
                            hcAnalyzerHelper.HCCalculatorParams.AnalyzerParms.AnalyzerType);
                        //when 3+ step analyzers start being used
                        CheckForLastStepAnalyzer(eAnalyzerType,
                            eStepNumber, extDocToCalcURI);
                        if (!CalculatorHelpers.IsSaveAction(extDocToCalcURI))
                        {
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bool bHasReplacedCalcDoc
                                = CalculatorHelpers.SaveNewCalculationsDocument(
                                hcAnalyzerHelper.HCCalculatorParams);
                            if (bHasReplacedCalcDoc)
                            {
                                //the resource01 app uses the calculator, rather
                                //than analyzer, pattern (i.e. children linked views 
                                //can be inserted)
                                if (hcAnalyzerHelper.AnalyzerType
                                    != HCAnalyzerHelper.ANALYZER_TYPES.resources01)
                                {
                                    //and add xmldoc params to update collection
                                    //only doctocalc gets updated
                                    CalculatorHelpers.AddXmlDocAndXmlDocIdsToUpdates(
                                        hcAnalyzerHelper.HCCalculatorParams);
                                }
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
                    if (hcAnalyzerHelper.AnalyzerType
                        == HCAnalyzerHelper.ANALYZER_TYPES.resources01)
                    {
                        extDocToCalcURI.URIDataManager.NeedsFullView = false;
                        extDocToCalcURI.URIDataManager.NeedsSummaryView = true;
                        extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(hcCalcParams.LinkedViewElement,
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
            extDocToCalcURI.ErrorMessage += hcAnalyzerHelper.HCCalculatorParams.ErrorMessage;
            return bHasAnalysis;
        }
        private static void CheckForLastStepAnalyzer(
            HCAnalyzerHelper.ANALYZER_TYPES analyzerType,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            //this release only includes three step analyzers
        }
        private static void SetAnalyzerParameters(
            HCAnalyzerHelper hcAnalyzerHelper, CalculatorParameters hcCalcParams)
        {
            //only resources01 analyzers are available yet
            hcAnalyzerHelper.SetAnalysisParameters();
            hcCalcParams.FileExtensionType
                = hcAnalyzerHelper.HCCalculatorParams.FileExtensionType;
            hcCalcParams.Stylesheet2Name
                = hcAnalyzerHelper.HCCalculatorParams.Stylesheet2Name;
            hcCalcParams.Stylesheet2ObjectNS
                = hcAnalyzerHelper.HCCalculatorParams.Stylesheet2ObjectNS;
        }
    }
}
