using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run monitoring and evaluation indicator stock calculations for operations 
    ///             and components.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OCLCA1StockCalculator
    {
        public OCLCA1StockCalculator(CalculatorParameters calcParameters)
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
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || currentElement.Name.LocalName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                //the object model uses a BudgetGroup but all that's needed is id and name 
                bHasCalculations = BILCA1Calculator.SetOCGroupLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BILCA1Calculator.SetOpOrCompLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                //resource stock calcs come from calculator results
                bHasCalculations = BILCA1Calculator.SetTechInputLCA1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        
    }
}
