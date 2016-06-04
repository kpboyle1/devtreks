using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run irrigation power stock calculations for operations and components.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OCIrrPowerStockCalculator
    {
        public OCIrrPowerStockCalculator(CalculatorParameters calcParameters)
        {
            BIIrrCalculator = new BIIrrPowerStockCalculator(calcParameters);
        }

        //stateful machinery stock
        public BIIrrPowerStockCalculator BIIrrCalculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || currentElement.Name.LocalName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                //the operation group can be used to insert calculators into 
                //descendant operations and run totals for each operation
                bHasCalculations = BIIrrCalculator.SetTotalMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = BIIrrCalculator.SetOpOrCompMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
               .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                //resource stock calcs come from calculator results
                if (currentCalculationsElement != null)
                {
                    bHasCalculations = BIIrrCalculator.SetInputMachineryStockCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
            }
            return bHasCalculations;
        }
    }
}
