using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Calculate general capital stocks for inputs.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    /// </summary>
    public class IOGeneralCapitalStockCalculator
    {
        public IOGeneralCapitalStockCalculator(CalculatorParameters calcParameters)
        {
            BIGC1Calculator = new BIGeneralCapitalStockCalculator(calcParameters);
        }

        //stateful calculator
        BIGeneralCapitalStockCalculator BIGC1Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                && calcParameters.ExtensionDocToCalcURI.URINodeName
                != Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                bHasCalculations = BIGC1Calculator.SetTotalMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                if (currentCalculationsElement != null)
                {
                    GeneralCapital1Input machInput = new GeneralCapital1Input();
                    machInput.SetGeneralCapital1InputProperties(calcParameters,
                        currentCalculationsElement, currentElement);
                    //init analyzer props
                    machInput.SetCalculatorProperties(currentCalculationsElement);
                    //run the calculations
                    bHasCalculations = RunGeneralCapitalStockCalculations(machInput,
                        calcParameters);
                    //serialize object back to xml (note that the base calc is not run here)
                    string sAttNameExtension = string.Empty;
                    //update the calculator attributes
                    machInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    machInput.SetGeneralCapital1InputAttributes(calcParameters,
                    ref currentCalculationsElement, ref currentElement);
                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    //input groups only aggregate inputs (not input series)
                    if (currentElement.Name.LocalName
                        == Input.INPUT_PRICE_TYPES.input.ToString())
                    {
                        //the count is 1-based, while iNodePosition is 0-based
                        //so the count is the correct next index position
                        int iNodePosition = BIGC1Calculator.GeneralCapitalStock
                            .GetNodePositionCount(calcParameters.AnalyzerParms.FilePositionIndex,
                            machInput);
                        if (iNodePosition < 0)
                            iNodePosition = 0;
                        bHasCalculations = BIGC1Calculator.GeneralCapitalStock
                            .AddGeneralCapital1StocksToDictionary(
                            calcParameters.AnalyzerParms.FilePositionIndex, iNodePosition,
                            machInput);
                    }
                }
            }
            return bHasCalculations;
        }
        public bool RunGeneralCapitalStockCalculations(GeneralCapital1Input machInput,
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
                bHasCalculations = SetGeneralCapitalStockCalculations(multiplier,
                    calcParameters, machInput);
            }
            else
            {
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        private void TransferCorrespondingDbProperties(
           ref GeneralCapital1Input machInput)
        {
            //calculators use aliases to change db properties
            if (machInput.MarketValue > 0)
            {
                machInput.CAPPrice = machInput.MarketValue;
            }
        }
        public bool SetGeneralCapitalStockCalculations(double multiplier,
            CalculatorParameters calcParameters, GeneralCapital1Input machInput)
        {
            bool bHasCalculations = false;
            if (machInput != null)
            {
                //don't adjust machinery per hour costs by any multiplier
                //pattern is fine but not appropriate here
                //in this extension, calculations are carried out in the 
                //budget/investment calculators
                //BIGeneralCapitalStockCalculator biCalculator = new BIGeneralCapitalStockCalculator();
                //biCalculator.ChangeGeneralCapitalInputByMultiplier(machInput, multiplier);
                //bHasCalculations = true;
            }
            else
            {
                calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            }
            return bHasCalculations;
        }
    }
}
