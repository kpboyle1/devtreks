using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run monitoring and evaluation indicator stock calculations for outcomes
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OutcomeLCA1StockCalculator
    {
        public OutcomeLCA1StockCalculator(CalculatorParameters calcParameters)
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
                == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                //the operation group can be used to insert calculators into 
                //descendant operations and run totals for each operation
                bHasCalculations = BILCA1Calculator.SetOutcomeGroupLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BILCA1Calculator.SetOutcomeLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                bHasCalculations = BILCA1Calculator.SetTechOutputLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        
    }
}
