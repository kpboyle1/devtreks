using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Run both calculators and analyzers for Stock data
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES: 
    ///         1. This release introduces Stock calculators and analyzers
    /// </summary>

    //custom exportattribute which inherits from DoStepsHostView 
    //(note that these attributes will change in the future)
    [DoStepsAttribute(CONTRACT_TYPE = CALULATOR_CONTRACT_TYPES.defaultcalculatormanager,
        CalculatorsExtensionName = "SB1")]
    public class SB1 : DoStepsHostView
    {
        public override async Task<bool> RunCalculatorStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasCalculation = false;
            CalculatorParameters SB1CalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            SB1CalculatorHelper sb1BudgetHelper
                = new SB1CalculatorHelper(SB1CalcParams);
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
                    if (SB1CalcParams != null)
                    {
                        //set constants for this step
                        bHasCalculation
                            = SB1CalculatorHelper.SetConstants(SB1CalcParams);
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepthree:
                    //get rid of any update member that was added after running the same step 2x
                    CalculatorHelpers.RefreshUpdates(SB1CalcParams, stepNumber, updates);
                    if (SB1CalcParams != null)
                    {
                        //run the calculations 
                        bHasCalculation = await sb1BudgetHelper.RunSB1CalculatorCalculationsAsync();

                        extDocToCalcURI.ErrorMessage = SB1CalcParams.ErrorMessage;
                        extDocToCalcURI.ErrorMessage += SB1CalcParams.ExtensionDocToCalcURI.ErrorMessage;
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
                            CheckForLastStepCalculator(SB1CalcParams,
                                eStepNumber, extDocToCalcURI);
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bHasCalculation
                                = CalculatorHelpers.SaveNewCalculationsDocument(SB1CalcParams);
                        }
                        else
                        {
                            bHasCalculation = false;
                        }
                    }
                    extDocToCalcURI.URIDataManager.NeedsFullView = true;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                    break;
                case ContractHelpers.EXTENSION_STEPS.stepfour:
                    extDocToCalcURI.URIDataManager.NeedsFullView = false;
                    extDocToCalcURI.URIDataManager.NeedsSummaryView = true;
                    extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(SB1CalcParams.LinkedViewElement,
                            Calculator1.cFileExtensionType);
                    bHasCalculation = true;
                    //tells addinhelper to save calcs
                    CalculatorHelpers.SetTempDocSaveCalcsProperty(extDocToCalcURI);
                    break;
                default:
                    //as many steps as needed can be added to this addin
                    break;
            }
            extDocToCalcURI.ErrorMessage += SB1CalcParams.ErrorMessage;
            return bHasCalculation;
        }
        private static void CheckForLastStepCalculator(
            CalculatorParameters sb1CalcParams,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            SB1CalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = SB1CalculatorHelper.GetCalculatorType(
                sb1CalcParams.CalculatorType);
            //other projects have code for handling different
            //numbers of steps in calculators
        }
        public override async Task<bool> RunAnalyzerStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasAnalysis = false;
            CalculatorParameters sb1CalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            SB1AnalyzerHelper sb1AnalyzerHelper = new SB1AnalyzerHelper(sb1CalcParams);
            //check to make sure the analyzer can be run
            sb1AnalyzerHelper.SetOptions();
            if (sb1AnalyzerHelper.SB1CalculatorParams.ErrorMessage != string.Empty)
            {
                extDocToCalcURI.ErrorMessage += sb1AnalyzerHelper.SB1CalculatorParams.ErrorMessage;
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
                    CalculatorHelpers.RefreshUpdates(sb1CalcParams, stepNumber, updates);
                    //linked view insertions needs some analysis parameters
                    SetPreAnalyzerParameters(sb1AnalyzerHelper, sb1CalcParams);
                    if (sb1CalcParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.devpacks)
                    {
                        bHasAnalysis = await sb1AnalyzerHelper.RunAnalysis(sb1CalcParams.UrisToAnalyze);
                    }
                    else
                    {
                        bHasAnalysis = await sb1AnalyzerHelper.RunAnalysis();
                    }
                    extDocToCalcURI.ErrorMessage = sb1CalcParams.ErrorMessage;
                    extDocToCalcURI.ErrorMessage += sb1AnalyzerHelper.SB1CalculatorParams.ErrorMessage;
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
                        SB1AnalyzerHelper.ANALYZER_TYPES eAnalyzerType
                            = sb1AnalyzerHelper.GetAnalyzerType(
                            sb1AnalyzerHelper.SB1CalculatorParams.AnalyzerParms.AnalyzerType);
                        //when 3+ step analyzers start being used
                        CheckForLastStepAnalyzer(eAnalyzerType,
                            eStepNumber, extDocToCalcURI);
                        if (!CalculatorHelpers.IsSaveAction(extDocToCalcURI))
                        {
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bool bHasReplacedCalcDoc
                                = CalculatorHelpers.SaveNewCalculationsDocument(
                                sb1AnalyzerHelper.SB1CalculatorParams);
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
                    if (sb1AnalyzerHelper.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1
                        || sb1AnalyzerHelper.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1
                        || sb1AnalyzerHelper.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr
                        || sb1AnalyzerHelper.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid
                        || sb1AnalyzerHelper.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt
                        || sb1AnalyzerHelper.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1)
                    {
                        if (sb1CalcParams.NeedsFullView)
                        {
                            extDocToCalcURI.URIDataManager.NeedsFullView = true;
                            extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                        }
                        else
                        {
                            extDocToCalcURI.URIDataManager.NeedsFullView = false;
                            extDocToCalcURI.URIDataManager.NeedsSummaryView = true;
                        }
                        extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(sb1CalcParams.LinkedViewElement,
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
            extDocToCalcURI.ErrorMessage += sb1AnalyzerHelper.SB1CalculatorParams.ErrorMessage;
            return bHasAnalysis;
        }
        private static void CheckForLastStepAnalyzer(
            SB1AnalyzerHelper.ANALYZER_TYPES analyzerType,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            //this release only includes three step analyzers
        }
        private static void SetPreAnalyzerParameters(
            SB1AnalyzerHelper sb1AnalyzerHelper, CalculatorParameters sb1CalcParams)
        {
            bool bIsPostAnalysis = false;
            sb1AnalyzerHelper.SetAnalysisParameters(bIsPostAnalysis);
            sb1CalcParams.FileExtensionType
                = sb1AnalyzerHelper.SB1CalculatorParams.FileExtensionType;
            sb1CalcParams.Stylesheet2Name
                = sb1AnalyzerHelper.SB1CalculatorParams.Stylesheet2Name;
            sb1CalcParams.Stylesheet2ObjectNS
                = sb1AnalyzerHelper.SB1CalculatorParams.Stylesheet2ObjectNS;
        }
    }
}