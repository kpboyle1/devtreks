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
    ///Purpose:		This class derives from the CalculatorContracts.InputOutputCalculator 
    ///             class. It subscribes to the RunCalculation events raised by that class. 
    ///             It runs calculations on the nodes returned by that class, 
    ///             and returns a calculated xelement to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2014, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This calculator didn't need the SetAncestorObjects event raised 
    ///             by the base class.
    ///             2. This calculator doesn't use cumulative totals, so analyzers using 
    ///             this class need to carry out all aggregations.
    /// </summary>
    public class IOME2StockSubscriber : InputOutputCalculator
    {
        //constructors
        public IOME2StockSubscriber() { }
        //constructor sets class (base) properties
        public IOME2StockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorsType
                == ME2CalculatorHelper.CALCULATOR_TYPES.me2.ToString())
            {
                this.ME2StockCalculator = new IOME2StockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public IOME2StockCalculator ME2StockCalculator { get; set; }
        //if true, saves the totals using new object model
        public bool HasTotals { get; set; }
        //methods
        //subscribe to the events raised by the base class
        public bool RunCalculator()
        {
            bool bHasAnalysis = false;
            //subscribe to the event raised by the publisher delegate
            //does not use addancestors event
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
            ME2CalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = ME2CalculatorHelper.GetCalculatorType(
                this.GCCalculatorParams.CalculatorType);
            if (eCalculatorType == ME2CalculatorHelper.CALCULATOR_TYPES.me2)
            {
                e.HasCalculations = RunME2Calculation(eCalculatorType,
                    ref statElement, ref linkedViewElement);
            }
            else
            {
                //run normally and save the same statelement and linkedviewelement
                e.HasCalculations = RunME2Analysis(
                    ref statElement, ref linkedViewElement);
            }
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
        private bool RunME2Calculation(ME2CalculatorHelper.CALCULATOR_TYPES calculatorType,
            ref XElement currentElement, ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            switch (calculatorType)
            {
                case ME2CalculatorHelper.CALCULATOR_TYPES.me2:
                    //serialize, run calcs, and deserialize
                    ME2Calculator me2 = new ME2Calculator();
                    bHasCalculations = me2.SetME2Calculations(calculatorType, this.GCCalculatorParams,
                        ref currentCalculationsElement, ref currentElement);
                    break;
                default:
                    //should be running an analysis
                    break;
            }
            return bHasCalculations;
        }
        private bool RunME2Analysis(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.RelatedCalculatorsType
               == ME2CalculatorHelper.CALCULATOR_TYPES.me2.ToString())
            {
                bHasCalculations
                    = this.ME2StockCalculator.AddCalculationsToCurrentElement(
                        ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
       
        private bool TransferErrors(bool hasCalculations)
        {
            bool bHasGoodCalculations = hasCalculations;
            if (this.ME2StockCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.ME2StockCalculator.BIME2Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.ME2StockCalculator.BIME2Calculator.GCCalculatorParams.ErrorMessage;
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
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                || this.GCCalculatorParams.StartingDocToCalcNodeName
                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString()))
            {
                //see the corresponding machinery stock pattern
            }
        }
        public static double GetMultiplierForInput(Input input)
        {
            //input apps don't have a multipler (ops, comps, invests, and budgets do)
            double multiplier = 1;
            return multiplier;
        }
        public static double GetMultiplierForOutput(Output output)
        {
            //output apps don't have a multipler (ops, comps, invests, and budgets do)
            double multiplier = 1;
            return multiplier;
        }
        public static double GetMultiplierForTechOutput(Output output)
        {
            double multiplier = 1;
            if (output != null)
            {
                //outputs times, and compositionamount multipliers
                //output.Amount used in analyzers
                multiplier = output.CompositionAmount * output.Times;
                if (multiplier == 0)
                {
                    multiplier = 1;
                }
            }
            return multiplier;
        }
        public static double GetMultiplierForTechInput(Input input)
        {
            double multiplier = 1;
            if (input != null)
            {
                //inputs have time multipliers
                //the OCAmount, AOHAmount, and CAPAmount are used in analyzers
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
