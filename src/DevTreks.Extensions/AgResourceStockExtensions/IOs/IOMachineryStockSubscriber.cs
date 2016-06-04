using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		This class derives from the CalculatorContracts.InputOutputCalculator 
    ///             class. It subscribes to the RunCalculation events raised by that class. 
    ///             It runs calculations on the nodes returned by that class, 
    ///             and returns a calculated xelement to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This calculator didn't need the SetAncestorObjects event raised 
    ///             by the base class.
    ///             2. This calculator doesn't use cumulative totals, so analyzers using 
    ///             this class need to carry out all aggregations.
    /// </summary>
    public class IOMachineryStockSubscriber : InputOutputCalculator
    {
        //constructors
        public IOMachineryStockSubscriber() { }
        //constructor sets class (base) properties
        public IOMachineryStockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
                this.MachineryStockCalculator = new IOMachineryStockCalculator(calcParameters);
            }
            else if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
            {
                this.IrrPowerStockCalculator = new IOIrrPowerStockCalculator(calcParameters);
            }
            else if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
                this.GeneralCapitalStockCalculator = new IOGeneralCapitalStockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public IOMachineryStockCalculator MachineryStockCalculator { get; set; }
        public IOIrrPowerStockCalculator IrrPowerStockCalculator { get; set; }
        public IOGeneralCapitalStockCalculator GeneralCapitalStockCalculator { get; set; }
        //methods
        //subscribe to the events raised by the base class
        public bool RunCalculator()
        {
            bool bHasAnalysis = false;
            //subscribe to the event raised by the publisher delegate
            this.RunCalculation += AddCalculations;
            //run the calculation (raising the events
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveCalculation();
            return bHasAnalysis;
        }
        //define the actions to take when the event is raised
        public void AddCalculations(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement statElement = null;
            XElement linkedViewElement = null;
            if (e.CurrentElement != null)
                statElement = new XElement(e.CurrentElement);
            if (e.LinkedViewElement != null)
                linkedViewElement = new XElement(e.LinkedViewElement);
            //run the stats and add them to statelement
            e.HasCalculations = RunInputOrOutputCalculation(
                ref statElement, ref linkedViewElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            if (statElement != null)
                e.CurrentElement = new XElement(statElement);
            if (linkedViewElement != null)
                e.LinkedViewElement = new XElement(linkedViewElement);
        }
        
        private bool RunInputOrOutputCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
                bHasCalculations
                    = this.MachineryStockCalculator.AddCalculationsToCurrentElement(
                        this.GCCalculatorParams, 
                        ref currentCalculationsElement, ref currentElement);
            }
            else if (this.GCCalculatorParams.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
            {
                bHasCalculations
                    = this.IrrPowerStockCalculator.AddCalculationsToCurrentElement(
                        this.GCCalculatorParams, 
                        ref currentCalculationsElement, ref currentElement);
            }
            else if (this.GCCalculatorParams.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
                bHasCalculations
                    = this.GeneralCapitalStockCalculator.AddCalculationsToCurrentElement(
                        this.GCCalculatorParams, 
                        ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        public static double GetMultiplierForInput(Input input)
        {
            //input apps don't have a multipler (ops, comps, invests, and budgets do)
            double multiplier = 1;
            return multiplier;
        }
        public static double GetMultiplierForTechInput(Input input)
        {
            double multiplier = 1;
            if (input != null)
            {
                multiplier = input.Times;
                if (multiplier == 0)
                {
                    multiplier = 1;
                }
            }
            return multiplier;
        }
       
    }
}
