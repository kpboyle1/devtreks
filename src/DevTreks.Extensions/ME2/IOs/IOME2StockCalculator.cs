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
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class IOME2StockCalculator
    {
        public IOME2StockCalculator(CalculatorParameters calcParameters)
        {
            BIME2Calculator = new BIME2StockCalculator(calcParameters);
        }
        //stateful health care stock
        public BIME2StockCalculator BIME2Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BIME2Calculator.SetInputGroupME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BIME2Calculator.SetOutputGroupME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BIME2Calculator.SetInputME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BIME2Calculator.SetOutputME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.inputseries.ToString()))
            {
                bHasCalculations = BIME2Calculator.SetInputSeriesME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.outputseries.ToString()))
            {
                bHasCalculations = BIME2Calculator.SetOutputSeriesME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
    }
}