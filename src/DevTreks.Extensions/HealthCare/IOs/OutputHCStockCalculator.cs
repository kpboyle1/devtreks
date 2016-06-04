using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Calculate health output stocks.
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    /// </summary>
    public class OutputHCStockCalculator
    {
        public OutputHCStockCalculator() { }
        public OutputHCStockCalculator(CalculatorParameters calcParameters)
        {
            BIHC1Calculator = new BIHCStockCalculator(calcParameters);
        }
        //stateful health input stock
        BIHCStockCalculator BIHC1Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            CalculatorParameters calcParameters, 
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()
                && calcParameters.ExtensionDocToCalcURI.URINodeName
                != Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                bHasCalculations = BIHC1Calculator.SetTotalHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                if (currentCalculationsElement != null)
                {
                    HealthBenefit1Calculator healthCostOutput = new HealthBenefit1Calculator();
                    //deserialize xml to object
                    healthCostOutput.SetHealthBenefit1Properties(calcParameters,
                        currentCalculationsElement, currentElement);
                    //init analyzer props
                    healthCostOutput.SetCalculatorProperties(currentCalculationsElement);
                    //run the calculations
                    bHasCalculations = RunHCStockCalculations(healthCostOutput,
                        calcParameters);
                    //serialize object back to xml
                    string sAttNameExtension = string.Empty;
                    //bool bRemoveAtts = false;
                    //note that unlike other IOAnalyzers, this runs the input calc too
                    //and must update input props to calculated results (OCAmount and OCPrice calcs)
                    //also note that if input analyzers are needed, probably want to use BIHCStockCalcor
                    //so that does not update input db props and keeps consistent pattern
                    healthCostOutput.SetOutputAttributes(calcParameters,
                        ref currentElement, updates);
                    //update the calculator attributes
                    healthCostOutput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    healthCostOutput.SetNewOutputAttributes(calcParameters, ref currentCalculationsElement);
                    healthCostOutput.SetHealthBenefit1Attributes(sAttNameExtension,
                        ref currentCalculationsElement);

                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    //input groups only aggregate inputs (not input series)
                    if (currentElement.Name.LocalName
                        .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        //add the machinery to the machstock.machstocks dictionary
                        //the count is 1-based, while iNodePosition is 0-based
                        //so the count is the correct next index position
                        int iNodePosition = BIHC1Calculator.OutputHCStock
                            .GetNodePositionCount(calcParameters.AnalyzerParms.FilePositionIndex,
                            healthCostOutput);
                        if (iNodePosition < 0)
                            iNodePosition = 0;
                        bHasCalculations = BIHC1Calculator.OutputHCStock
                            .AddOutputHCStocksToDictionary(
                            calcParameters.AnalyzerParms.FilePositionIndex, iNodePosition,
                            healthCostOutput);
                    }
                }
            }
            return bHasCalculations;
        }
        public bool RunHCStockCalculations(HealthBenefit1Calculator healthCostOutput,
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //see if any db props are being changed by calculator
            TransferCorrespondingDbProperties(ref healthCostOutput);
            //set the multiplier (the multiplier in most inputs is 1,
            //this is kept here to keep a uniform pattern when the multiplier 
            //can be changed -see the food nutrition calculator)
            double multiplier = GetMultiplierForHealthCareCost1(healthCostOutput);
            bHasCalculations = healthCostOutput.RunHCCalculations(calcParameters, healthCostOutput);
            return bHasCalculations;
        }
        public static double GetMultiplierForHealthCareCost1(HealthBenefit1Calculator hcCost1)
        {
            //health output only multiplier (IOHCStockSubscriber uses input.times instead)
            double multiplier = 0;
            //double multiplier = (hcCost1.ActualServingsPerContainer == 0)
            //    ? 1 : hcCost1.ServingsPerContainer / hcCost1.ActualServingsPerContainer;
            return multiplier;
        }
        private void TransferCorrespondingDbProperties(
            ref HealthBenefit1Calculator healthCostOutput)
        {
            //nothing yet
        }


    }
}
