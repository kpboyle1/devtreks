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
    ///Purpose:		Run resource stock calculations and analysis for related 
    ///             agricultural resource calculators (i.e. machinery, nutrient, 
    ///             water, energy, carbon)
    ///Author:		www.devtreks.org
    ///Date:		2015, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES: 
    ///         1. Resource stock budgets can focus on the machinery 
    ///         stock used in an enterprise, the nutrient stocks of soil, 
    ///         the water budgets for construction projects, energy budgets 
    ///         for households...
    ///         2. These stock budgets are calculated and analyzed using the data 
    ///         from calculators found in other projects (but used in this extension), 
    ///         such as machinery, irrigation, nutrient, and carbon calculators.
    ///         3. The resources01 app in this extension uses a combination of the 
    ///         calculator and analyzer patterns. Some analyzers don't run stats, 
    ///         but run additional calculations on linked view data. In the case 
    ///         of resources01, these calcs are totals.
    ///         4. Resource stock budgeting has a long and fruitful history. 
    ///         Common features of stock budgets, such as increasing and 
    ///         decreasing stocks over time, will appear in future analyzers.
    /// </summary>

    //custom exportattribute which inherits from DoStepsHostView 
    //(note that these attributes will change in the future)
    [DoStepsAttribute(CONTRACT_TYPE = CALULATOR_CONTRACT_TYPES.defaultcalculatormanager,
        CalculatorsExtensionName = "AgResourceStockExtensions")]
    public class AgResourceStockExtensions : DoStepsHostView
    {
        public override async Task<bool> RunAnalyzerStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            bool bHasAnalysis = false;
            CalculatorParameters arsCalcParams
                = CalculatorHelpers.SetCalculatorParameters(
                extDocToCalcURI, extCalcDocURI, stepNumber, urisToAnalyze,
                updates);
            ARSAnalyzerHelper arsAnalyzerHelper = new ARSAnalyzerHelper(arsCalcParams);
            //check to make sure the analyzer can be run
            arsAnalyzerHelper.SetOptions();
            if (arsAnalyzerHelper.ARSCalculatorParams.ErrorMessage != string.Empty)
            {
                extDocToCalcURI.ErrorMessage += arsAnalyzerHelper.ARSCalculatorParams.ErrorMessage;
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
                    CalculatorHelpers.RefreshUpdatesUsingDocToCalc(arsCalcParams, 
                        stepNumber, updates);
                    if (arsAnalyzerHelper.AnalyzerType 
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources01
                        || arsAnalyzerHelper.AnalyzerType.ToString().StartsWith(ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString()))
                    {
                        //linked view insertions needs some analysis parameters
                        SetAnalyzerParameters(arsAnalyzerHelper, arsCalcParams);
                        //the calculator pattern handles children linked view insertions
                        //better than the analyzer pattern
                        ARSCalculatorHelper arsCalculatorHelper
                            = new ARSCalculatorHelper(arsCalcParams);
                        if (arsCalcParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.devpacks)
                        {
                            bHasAnalysis = arsCalculatorHelper.RunAnalysis(arsCalcParams.UrisToAnalyze);
                        }
                        else
                        {
                            bHasAnalysis = arsCalculatorHelper
                                .RunCalculations();
                        }
                    }
                    extDocToCalcURI.ErrorMessage = arsAnalyzerHelper.ARSCalculatorParams.ErrorMessage;
                    if (!bHasAnalysis)
                    {
                        extDocToCalcURI.ErrorMessage = (extDocToCalcURI.ErrorMessage == string.Empty) ?
                            Errors.MakeStandardErrorMsg("ANALYSES_URI_MISMATCH")
                            : extDocToCalcURI.ErrorMessage;
                        return bHasAnalysis;
                    }
                    if (string.IsNullOrEmpty(extDocToCalcURI.ErrorMessage))
                    {
                        //two step analyzers need to be saved now
                        ARSAnalyzerHelper.ANALYZER_TYPES eAnalyzerType
                            = arsAnalyzerHelper.GetAnalyzerType(
                            arsAnalyzerHelper.ARSCalculatorParams.AnalyzerParms.AnalyzerType);
                        //when 3+ step analyzers start being used
                        CheckForLastStepAnalyzer(eAnalyzerType,
                            eStepNumber, extDocToCalcURI);
                        if (!CalculatorHelpers.IsSaveAction(extCalcDocURI))
                        {
                            //replace the old calculator with the new one
                            //and save the new calculations document
                            bool bHasReplacedCalcDoc
                                = CalculatorHelpers.SaveNewCalculationsDocument(
                                arsAnalyzerHelper.ARSCalculatorParams);
                            if (bHasReplacedCalcDoc)
                            {
                                //the resource01 app uses the calculator, rather
                                //than analyzer, pattern (i.e. children linked views 
                                //can be inserted)
                                if (arsAnalyzerHelper.AnalyzerType
                                    != ARSAnalyzerHelper.ANALYZER_TYPES.resources01
                                    && (!arsAnalyzerHelper.AnalyzerType.ToString().StartsWith(ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())))
                                {
                                    //and add xmldoc params to update collection
                                    //only doctocalc gets updated
                                    CalculatorHelpers.AddXmlDocAndXmlDocIdsToUpdates(
                                        arsAnalyzerHelper.ARSCalculatorParams);
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
                    if (arsAnalyzerHelper.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources01)
                    {
                        extDocToCalcURI.URIDataManager.NeedsFullView = true;
                        extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                        extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(arsCalcParams.LinkedViewElement, 
                            Calculator1.cFileExtensionType);
                    }
                    else if (arsAnalyzerHelper.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a
                        || arsAnalyzerHelper.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources02)
                    {
                        extDocToCalcURI.URIDataManager.NeedsFullView = true;
                        extDocToCalcURI.URIDataManager.NeedsSummaryView = false;
                        extCalcDocURI.URIFileExtensionType = CalculatorHelpers.GetAttribute(arsCalcParams.LinkedViewElement,
                            Calculator1.cFileExtensionType);
                    }
                    if (extDocToCalcURI.URIDataManager.TempDocSaveMethod
                        == Constants.SAVECALCS_METHOD.saveastext.ToString())
                    {
                        //text files can be filtered by subscribers
                        ARSAnalyzerHelper.ANALYZER_TYPES eAnalyzerType
                            = arsAnalyzerHelper.GetAnalyzerType(
                            arsAnalyzerHelper.ARSCalculatorParams.AnalyzerParms.AnalyzerType);
                        ARSTextSubscriber npvTextSubscriber
                            = new ARSTextSubscriber(arsCalcParams, eAnalyzerType);
                        bHasAnalysis = npvTextSubscriber.BuildObservations(urisToAnalyze);
                        if (!bHasAnalysis
                            || (!string.IsNullOrEmpty(npvTextSubscriber.ObsCalculatorParams.ErrorMessage)))
                        {
                            extDocToCalcURI.ErrorMessage = npvTextSubscriber.ObsCalculatorParams.ErrorMessage;
                            CalculatorHelpers.SetTempDocSaveNoneProperty(extDocToCalcURI);
                        }
                        else
                        {
                            //tells addinhelper to save calcs
                            CalculatorHelpers.SetTempDocSaveAnalysesProperty(extDocToCalcURI);
                        }
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
            extDocToCalcURI.ErrorMessage += arsCalcParams.ErrorMessage;
            extDocToCalcURI.ErrorMessage += arsAnalyzerHelper.ARSCalculatorParams.ErrorMessage;
            return bHasAnalysis;
        }
        private static void CheckForLastStepAnalyzer(
            ARSAnalyzerHelper.ANALYZER_TYPES analyzerType,
            ContractHelpers.EXTENSION_STEPS stepNumber,
            ExtensionContentURI extDocToCalcURI)
        {
            //this release only includes three step analyzers
        }
        private static void SetAnalyzerParameters(
            ARSAnalyzerHelper arsAnalyzerHelper, CalculatorParameters arsCalcParams)
        {
            arsAnalyzerHelper.SetAnalysisParameters();
            arsCalcParams.FileExtensionType
                = arsAnalyzerHelper.ARSCalculatorParams.FileExtensionType;
            arsCalcParams.Stylesheet2Name
                = arsAnalyzerHelper.ARSCalculatorParams.Stylesheet2Name;
            arsCalcParams.Stylesheet2ObjectNS
                = arsAnalyzerHelper.ARSCalculatorParams.Stylesheet2ObjectNS;
        }
        //implement the interface, but this is not a calculators addin
        public override async Task<bool> RunCalculatorStep(
            ExtensionContentURI extDocToCalcURI, ExtensionContentURI extCalcDocURI,
            string stepNumber, IList<string> urisToAnalyze,
            IDictionary<string, string> updates, CancellationToken cancellationToken)
        {
            return false;
        }
    }
}
