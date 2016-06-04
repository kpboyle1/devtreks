using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run monitoring and evaluation indicator stock calculations for inputs 
    ///             and outputs.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class IONPV1StockCalculator
    {
        public IONPV1StockCalculator(CalculatorParameters calcParameters)
        {
            BINPV1Calculator = new BINPV1StockCalculator(calcParameters);
        }
        //stateful health care stock
        public BINPV1StockCalculator BINPV1Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BINPV1Calculator.SetInputGroupNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BINPV1Calculator.SetOutputGroupNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BINPV1Calculator.SetInputNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BINPV1Calculator.SetOutputNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.inputseries.ToString()))
            {
                bHasCalculations = BINPV1Calculator.SetInputSeriesNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.outputseries.ToString()))
            {
                bHasCalculations = BINPV1Calculator.SetOutputSeriesNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
    }
}