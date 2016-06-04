using System.Collections.Generic;
using System.Xml.Linq;

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
    /// </summary>
    public class ABIOSubscriber : InputOutputCalculator
    {
        //constructors
        public ABIOSubscriber() { }
        //constructor sets class (base) properties
        public ABIOSubscriber(CalculatorParameters calcParameters,
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType)
            : base(calcParameters)
        {
            this.CalculatorType = calculatorType;
        }

        //properties
        //calculators specific to this extension
        public AgBudgetingHelpers.CALCULATOR_TYPES CalculatorType { get; set; }

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
                ref statElement, ref linkedViewElement, 
                this.GCCalculatorParams.Updates);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            if (statElement != null)
                e.CurrentElement = new XElement(statElement);
            if (linkedViewElement != null)
                e.LinkedViewElement = new XElement(linkedViewElement);
        }
        
        private bool RunInputOrOutputCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            AgBudgetingHelpers.CALCULATOR_TYPES eCalculatorType
                = AgBudgetingHelpers.GetCalculatorType(
                this.GCCalculatorParams.CalculatorType);
            switch (eCalculatorType)
            {
                case AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery:
                    bHasCalculations
                        = Machinery1InputCalculator.SetMachinery1Calculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, updates);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.irrpower:
                    bHasCalculations
                            = Machinery1InputCalculator.SetMachinery1Calculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, updates);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.lifecycle:
                    bHasCalculations
                        = Machinery1InputCalculator.SetMachinery1Calculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, updates);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.gencapital:
                    bHasCalculations
                        = Machinery1InputCalculator.SetMachinery1Calculations(
                        eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                        ref currentElement, updates);
                    break;
                default:
                    break;
            }
            return bHasCalculations;
        }
    }
}
