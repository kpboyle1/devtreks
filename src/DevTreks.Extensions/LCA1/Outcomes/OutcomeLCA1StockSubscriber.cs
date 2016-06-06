using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		This class derives from the CalculatorContracts.OutcomeCalculator 
    ///             class. It subscribes to the RunCalculation and SetAncestorObjects events 
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class OutcomeLCA1StockSubscriber : OutcomeCalculator
    {
        //constructors
        public OutcomeLCA1StockSubscriber() { }
        //constructor sets class (base) properties
        public OutcomeLCA1StockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorsType
                == LCA1CalculatorHelper.CALCULATOR_TYPES.lifecycle1.ToString())
            {
                this.LCA1StockCalculator = new OutcomeLCA1StockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public OutcomeLCA1StockCalculator LCA1StockCalculator { get; set; }
        //if true, saves the totals using new object model
        public bool HasTotals { get; set; }
        //methods
        //subscribe to the events raised by the base class
        public bool RunCalculator()
        {
            bool bHasAnalysis = false;
            //subscribe to the event raised by the publisher delegate
            //this.SetAncestorObjects += AddAncestorObjects;
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
            LCA1CalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = LCA1CalculatorHelper.GetCalculatorType(
                this.GCCalculatorParams.CalculatorType);
            //run normally and save the same statelement and linkedviewelement
            e.HasCalculations = RunLCA1Analysis(
                ref statElement, ref linkedViewElement);
            if (e.HasCalculations)
            {
                //pass the new statelement back to the publisher
                //by setting the CalculatedElement property of CustomEventArgs
                if (statElement != null)
                    e.CurrentElement = new XElement(statElement);
                if (linkedViewElement != null)
                    e.LinkedViewElement = new XElement(linkedViewElement);
            }
        }
        
        private bool RunLCA1Analysis(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.SubApplicationType 
                == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                if (this.GCCalculatorParams.RelatedCalculatorsType
                    == LCA1CalculatorHelper.CALCULATOR_TYPES.lifecycle1.ToString())
                {
                    //assume progress01 is default
                    bHasCalculations
                        = this.LCA1StockCalculator.AddCalculationsToCurrentElement(
                            ref currentCalculationsElement, ref currentElement);
                }
            }
            bHasCalculations = TransferErrors(bHasCalculations);
            //some calcparams have to be passed back to this.calcparams
            UpdateCalculatorParams(currentElement.Name.LocalName);
            return bHasCalculations;
        }

        public static double GetMultiplierForOutcome(Outcome outcome)
        {
            //this subscriber does not use these multipliers
            double multiplier = 1;
            if (outcome != null)
            {
                multiplier = outcome.Amount;
                if (multiplier == 0)
                {
                    multiplier = 1;
                }
            }
            return multiplier;
        }
        private bool TransferErrors(bool hasCalculations)
        {
            bool bHasGoodCalculations = hasCalculations;
            if (this.LCA1StockCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.LCA1StockCalculator.BILCA1Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.LCA1StockCalculator.BILCA1Calculator.GCCalculatorParams.ErrorMessage;
            }
            if (!string.IsNullOrEmpty(this.GCCalculatorParams.ErrorMessage))
            {
                this.GCCalculatorParams.ErrorMessage += this.GCCalculatorParams.ErrorMessage;
                bHasGoodCalculations = false;
            }
            return bHasGoodCalculations;
        }
        private void UpdateCalculatorParams(string currentNodeName)
        {
            //pass back NeedsXmlDocOnly in calcparams
            if (this.GCCalculatorParams.NeedsCalculators
                && (this.GCCalculatorParams.StartingDocToCalcNodeName
                == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString()))
            {
                //see the corresponding machinery stock pattern
            }
        }
    }
}
