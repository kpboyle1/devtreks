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
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class IOLCA1StockCalculator
    {
        public IOLCA1StockCalculator(CalculatorParameters calcParameters)
        {
            BILCA1Calculator = new BILCA1StockCalculator(calcParameters);
        }
        //stateful health care stock
        public BILCA1StockCalculator BILCA1Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BILCA1Calculator.SetInputGroupLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BILCA1Calculator.SetOutputGroupLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BILCA1Calculator.SetInputLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BILCA1Calculator.SetOutputLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.inputseries.ToString()))
            {
                bHasCalculations = BILCA1Calculator.SetInputSeriesLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.outputseries.ToString()))
            {
                bHasCalculations = BILCA1Calculator.SetOutputSeriesLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        
    }
}