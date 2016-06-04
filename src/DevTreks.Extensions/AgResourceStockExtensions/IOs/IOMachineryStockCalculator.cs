using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Calculate machinery stocks for inputs.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    /// </summary>
    public class IOMachineryStockCalculator
    {
        public IOMachineryStockCalculator(CalculatorParameters calcParameters)
        {
            BIM1Calculator = new BIMachineryStockCalculator(calcParameters);
        }
        //stateful machinery stock
        BIMachineryStockCalculator BIM1Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            CalculatorParameters calcParameters, 
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                && calcParameters.ExtensionDocToCalcURI.URINodeName
                != Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                bHasCalculations = BIM1Calculator.SetTotalMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                if (currentCalculationsElement != null)
                {
                    Machinery1Input machInput = new Machinery1Input();
                    //deserialize xml to object
                    machInput.SetMachinery1InputProperties(calcParameters,
                        currentCalculationsElement, currentElement);
                    //init analyzer props
                    machInput.SetCalculatorProperties(currentCalculationsElement);
                    //run the calculations
                    bHasCalculations = RunMachineryStockCalculations(machInput,
                        calcParameters);
                    //serialize object back to xml (note that the base calc is not run here)
                    string sAttNameExtension = string.Empty;
                    //set new machinery input totals
                    machInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    machInput.SetMachinery1InputAttributes(calcParameters,
                        ref currentCalculationsElement, ref currentElement);
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
                        int iNodePosition = BIM1Calculator.MachineryStock
                            .GetNodePositionCount(calcParameters.AnalyzerParms.FilePositionIndex,
                            machInput);
                        if (iNodePosition < 0)
                            iNodePosition = 0;
                        bHasCalculations = BIM1Calculator.MachineryStock
                            .AddMachinery1StocksToDictionary(
                            calcParameters.AnalyzerParms.FilePositionIndex, iNodePosition,
                            machInput);
                    }
                }
            }
            return bHasCalculations;
        }
        
        public bool RunMachineryStockCalculations(Machinery1Input machInput,
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //see if any db props are being changed by calculator
            TransferCorrespondingDbProperties(ref machInput);
            //set the multiplier (the multiplier in most inputs is 1,
            //this is kept here to keep a uniform pattern when the multiplier 
            //can be changed -see the food nutrition calculator)
            double multiplier = IOMachineryStockSubscriber
                .GetMultiplierForInput(machInput);
            if (multiplier != 1)
            {
                bHasCalculations = SetMachineryStockCalculations(multiplier,
                    calcParameters, machInput);
            }
            else
            {
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public static double GetMultiplierForInput(Input input)
        {
            //ops, comps, budgets and investments use the times multiplier
            double multiplier = input.Times;
            return multiplier;
        }
        private void TransferCorrespondingDbProperties(
            ref Machinery1Input machInput)
        {
            //calculators use aliases to change db properties
            if (machInput.MarketValue > 0)
            {
                machInput.CAPPrice = machInput.MarketValue;
            }
        }
        public bool SetMachineryStockCalculations(double multiplier,
            CalculatorParameters calcParameters, Machinery1Input machInput)
        {
            bool bHasCalculations = true;
            //don't adjust machinery per hour costs by any multiplier
            //pattern is fine but not appropriate here
            //if (machInput != null)
            //{
            //    //in this extension, calculations are carried out in the 
            //    //budget/investment calculators
            //    BIMachineryStockCalculator.ChangeMachineryInputByMultiplier(machInput, multiplier);
            //    bHasCalculations = true;
            //}
            //else
            //{
            //    calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            //}
            return bHasCalculations;
        }
    }
}
