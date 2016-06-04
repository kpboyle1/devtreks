using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run machinery stock calculations for operations and components.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OCMachinery2StockCalculator
    {
        public OCMachinery2StockCalculator(CalculatorParameters calcParameters)
        {
            BIM2Calculator = new BIMachinery2StockCalculator(calcParameters);
            BIM2aCalculator = new BIMachinery2aStockCalculator(calcParameters);
        }
        //state management is handled by bi calculators
        public BIMachinery2StockCalculator BIM2Calculator { get; set; }
        public BIMachinery2aStockCalculator BIM2aCalculator { get; set; }


        public bool AddCalculationsToCurrentElement(ref XElement currentCalculationsElement, 
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || currentElement.Name.LocalName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                if (BIM2Calculator.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                {
                    //fill in the collections with base timeliness penalties
                    bHasCalculations = BIM2Calculator.SetTotalMachinery2StockJointCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
                else if (BIM2Calculator.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    //fill in the collections with feasible machinery selections
                    //if further work is needed use this.TimelinessTimePeriod.TimelinessOpComps collection
                    bHasCalculations = BIM2aCalculator.SetTotalMachinery2aStockCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
                
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                if (BIM2Calculator.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                {
                    //fill in the collections with base timeliness penalties
                    bHasCalculations = BIM2Calculator.SetOpOrCompMachinery2StockJointCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
                else if (BIM2Calculator.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    //fill in the collections with optimum machinery selections
                    bHasCalculations = BIM2aCalculator.SetOpOrCompMachinery2aStockCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
            }
            else if (currentElement.Name.LocalName
               .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                if (BIM2Calculator.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                {
                    //fill in the collections with base timeliness penalties
                    bHasCalculations = BIM2Calculator.SetInputMachinery2StockJointCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
                else if (BIM2Calculator.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    //fill in the collections with optimum machinery selections
                    bHasCalculations = BIM2aCalculator.SetInputMachinery2aStockCalculations(
                        ref currentCalculationsElement, ref currentElement);
                }
            }
            return bHasCalculations;
        }
    }
}