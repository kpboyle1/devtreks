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
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Resource stock calculators take the data found in related 
    ///             calculators, such as the agricultural machinery calculators, and 
    ///             run additional calculations and statistics on that data. 
    ///             Typical uses are to run resource stock budgets, where the 
    ///             stock goes up and down each time period depending on additions 
    ///             and subtractions. The stock can be capital (i.e. machinery and 
    ///             real property), natural resources (i.e. water and nutrients), 
    ///             human (i.e. knowledge and experience) and social (i.e. cohesion 
    ///             and empathy). Imagination will be needed in this project.
    /// </summary>
    public class ARSCalculatorHelper
    {
        //constructors
        public ARSCalculatorHelper() { }
        public ARSCalculatorHelper(CalculatorParameters calcParameters)
        {
            this.ARSCalculatorParams = calcParameters;
            //reset calculatortype (uses filestoanalyze attribute in calculator)
            calcParameters.CalculatorType 
                = calcParameters.AnalyzerParms.FilesToAnalyzeExtensionType;
        }

        //parameters needed by publishers 
        public CalculatorParameters ARSCalculatorParams { get; set; }

        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            //these calculators use a mix of calculator and analyzer patterns
            this.ARSCalculatorParams.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology;
            //urisToAnalyze has the summary calculated results path in position 0.
            //This calculator uses that path to derive the full calculated 
            //results path. The full path document is used to run the calculations 
            //(it has the input and output calculated results that are 
            //the basis for most resource stock calculations).
            this.ARSCalculatorParams.AnalyzerParms.ObservationsPath
                = CalculatorHelpers.GetFullCalculatorResultsPath(
                this.ARSCalculatorParams);
            if (!CalculatorHelpers.URIAbsoluteExists(this.ARSCalculatorParams.ExtensionDocToCalcURI,
                this.ARSCalculatorParams.AnalyzerParms.ObservationsPath))
            {
                this.ARSCalculatorParams.ErrorMessage 
                    = Errors.MakeStandardErrorMsg("ANALYZER_BASECALCS_MISSING");
                return false;
            }
            //can run npv calcs or base input calculators
            if (this.ARSCalculatorParams.CalculatorType 
                == CalculatorHelpers.CALCULATOR_TYPES.input.ToString()
                || this.ARSCalculatorParams.CalculatorType 
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString()
                || this.ARSCalculatorParams.CalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString()
                || this.ARSCalculatorParams.CalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
                IOMachineryStockSubscriber subInput
                        = new IOMachineryStockSubscriber(this.ARSCalculatorParams);
                bHasCalculations = subInput.RunCalculator();
                CalculatorHelpers.UpdateCalculatorParams(this.ARSCalculatorParams, 
                    subInput.GCCalculatorParams);
                subInput = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType.StartsWith(
                CalculatorHelpers.CALCULATOR_TYPES.operation.ToString())
                || this.ARSCalculatorParams.CalculatorType.StartsWith(
                CalculatorHelpers.CALCULATOR_TYPES.component.ToString()))
            {
                OCMachineryStockSubscriber subOperation
                    = new OCMachineryStockSubscriber(this.ARSCalculatorParams);
                bHasCalculations = subOperation.RunCalculator();
                CalculatorHelpers.UpdateCalculatorParams(this.ARSCalculatorParams,
                    subOperation.GCCalculatorParams);
                subOperation = null;
            }
            else if (this.ARSCalculatorParams.CalculatorType 
                == CalculatorHelpers.CALCULATOR_TYPES.budget.ToString()
                || this.ARSCalculatorParams.CalculatorType 
                == CalculatorHelpers.CALCULATOR_TYPES.investment.ToString())
            {
                BIMachineryStockSubscriber subBudget
                        = new BIMachineryStockSubscriber(this.ARSCalculatorParams);
                bHasCalculations = subBudget.RunCalculator();
                CalculatorHelpers.UpdateCalculatorParams(this.ARSCalculatorParams,
                    subBudget.GCCalculatorParams);
                subBudget = null;
            }
            return bHasCalculations;
        }
        public bool RunAnalysis(IList<string> urisToAnalyze)
        {
            bool bHasAnalysis = false;
            //set the files needing analysis
            CalculatorHelpers.SetFileOrFoldersToAnalyze(this.ARSCalculatorParams.AnalyzerParms.FilesToAnalyzeExtensionType,
                this.ARSCalculatorParams, urisToAnalyze);
            //run the analysis
            if (this.ARSCalculatorParams.AnalyzerParms.FileOrFolderPaths != null)
            {
                if (this.ARSCalculatorParams.AnalyzerParms.FileOrFolderPaths.Count > 0)
                {
                    //build the base file needed by the regular analysis
                    this.ARSCalculatorParams.AnalyzerParms.ObservationsPath
                        = CalculatorHelpers.GetFullCalculatorResultsPath(this.ARSCalculatorParams);
                    bHasAnalysis = CalculatorHelpers.AddFilesToBaseDocument(this.ARSCalculatorParams);
                    if (bHasAnalysis)
                    {
                        //run the regular analysis
                        bHasAnalysis = RunCalculations();
                        if (bHasAnalysis)
                        {
                            //v170 set devpack params
                            CalculatorHelpers.UpdateDevPackAnalyzerParams(this.ARSCalculatorParams);
                        }
                        //reset subapptype
                        this.ARSCalculatorParams.SubApplicationType = Constants.SUBAPPLICATION_TYPES.devpacks;
                    }
                    this.ARSCalculatorParams.ErrorMessage += this.ARSCalculatorParams.ErrorMessage;
                }
                else
                {
                    if (this.ARSCalculatorParams.DocToCalcNodeName
                        == Constants.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    {
                        //setting default analyzer attribute values
                    }
                    else
                    {
                        this.ARSCalculatorParams.ErrorMessage
                            = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
                    }
                }
            }
            else
            {
                this.ARSCalculatorParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_NOFILES");
            }
            return bHasAnalysis;
        }
        public bool RunDevPacksCalculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //these calculators use a mixed calculatorpatterns
            calcParameters.RunCalculatorType
                = CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology;
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
        public static void GetRelatedCalculatorAndType(CalculatorParameters calcParameters, 
            out string calcTypeAttributeName, out string calcTypeAttributeValue)
        {
            calcTypeAttributeName = Constants.NONE;
            calcTypeAttributeValue = Constants.NONE;
            //rule 1 if a relatedcalctype has been filled in, that must be used
            if (string.IsNullOrEmpty(calcParameters.RelatedCalculatorType) == false
                && calcParameters.RelatedCalculatorType != Constants.NONE)
            {
                calcTypeAttributeName = Calculator1.cRelatedCalculatorType;
                calcTypeAttributeValue = calcParameters.RelatedCalculatorType;
            }
            else
            {
                //second option is to use related calculators type
                calcTypeAttributeName = Calculator1.cRelatedCalculatorsType;
                calcTypeAttributeValue = calcParameters.RelatedCalculatorsType;
            }
        }
    }
}
