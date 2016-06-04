using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Calculate health input stocks.
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    /// </summary>
    public class InputHCStockCalculator
    {
        public InputHCStockCalculator() { }
        public InputHCStockCalculator(CalculatorParameters calcParameters)
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
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
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
                    HealthCareCost1Calculator healthCostInput = new HealthCareCost1Calculator();
                    //deserialize xml to object
                    healthCostInput.SetHealthCareCost1Properties(calcParameters,
                        currentCalculationsElement, currentElement);
                    //init analyzer props
                    healthCostInput.SetCalculatorProperties(currentCalculationsElement);
                    //run the calculations
                    bHasCalculations = RunHCStockCalculations(healthCostInput,
                        calcParameters);
                    //serialize object back to xml
                    string sAttNameExtension = string.Empty;
                    //bool bRemoveAtts = false;
                    //note that unlike other IOAnalyzers, this runs the input calc too
                    //and must update input props to calculated results (OCAmount and OCPrice calcs)
                    //also note that if input analyzers are needed, probably want to use BIHCStockCalcor
                    //so that does not update input db props and keeps consistent pattern
                    healthCostInput.SetInputAttributes(calcParameters,
                        ref currentElement, updates);
                    //update the calculator attributes
                    healthCostInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    healthCostInput.SetNewInputAttributes(calcParameters, ref currentCalculationsElement);
                    healthCostInput.SetHealthCareCost1Attributes(sAttNameExtension,
                        ref currentCalculationsElement);

                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    //input groups only aggregate inputs (not input series)
                    if (currentElement.Name.LocalName
                        .Contains(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        //add the machinery to the machstock.machstocks dictionary
                        //the count is 1-based, while iNodePosition is 0-based
                        //so the count is the correct next index position
                        int iNodePosition = BIHC1Calculator.InputHCStock
                            .GetNodePositionCount(calcParameters.AnalyzerParms.FilePositionIndex,
                            healthCostInput);
                        if (iNodePosition < 0)
                            iNodePosition = 0;
                        bHasCalculations = BIHC1Calculator.InputHCStock
                            .AddInputHCStocksToDictionary(
                            calcParameters.AnalyzerParms.FilePositionIndex, iNodePosition,
                            healthCostInput);
                    }
                }
            }
            return bHasCalculations;
        }
        public bool RunHCStockCalculations(HealthCareCost1Calculator healthCostInput,
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //see if any db props are being changed by calculator
            TransferCorrespondingDbProperties(ref healthCostInput);
            //set the multiplier (the multiplier in most inputs is 1,
            //this is kept here to keep a uniform pattern when the multiplier 
            //can be changed -see the food nutrition calculator)
            double multiplier = GetMultiplierForHealthCareCost1(healthCostInput);
            bHasCalculations = healthCostInput.RunHCCalculations(calcParameters, healthCostInput);
            return bHasCalculations;
        }
        public static double GetMultiplierForHealthCareCost1(HealthCareCost1Calculator hcCost1)
        {
            //health input only multiplier (IOHCStockSubscriber uses input.times instead)
            double multiplier = 0;
            //double multiplier = (hcCost1.ActualServingsPerContainer == 0)
            //    ? 1 : hcCost1.ServingsPerContainer / hcCost1.ActualServingsPerContainer;
            return multiplier;
        }
        private void TransferCorrespondingDbProperties(
            ref HealthCareCost1Calculator healthCostInput)
        {
            //calculators can use aliases to change db properties
            //but this calc gets its MV from input.CapPrice
            healthCostInput.BasePrice = healthCostInput.CAPPrice;
        }

        
    }
}
