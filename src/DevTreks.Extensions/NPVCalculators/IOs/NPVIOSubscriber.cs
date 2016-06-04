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
    ///Date:		2011, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This calculator didn't need the SetAncestorObjects event raised 
    ///             by the base class.
    ///             2. This calculator doesn't use cumulative totals, so analyzers using 
    ///             this class need to carry out all aggregations.
    /// </summary>
    public class NPVIOSubscriber : InputOutputCalculator
    {
        //constructors
        public NPVIOSubscriber() { }
        //constructor sets class (base) properties
        public NPVIOSubscriber(CalculatorParameters calcParameters,
            CalculatorHelpers.CALCULATOR_TYPES calculatorType)
            : base(calcParameters)
        {
            this.CalculatorType = calculatorType;
        }

        //properties
        //calculators specific to this extension
        public CalculatorHelpers.CALCULATOR_TYPES CalculatorType { get; set; }

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
                ref statElement, ref linkedViewElement, this.GCCalculatorParams.Updates);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            if (statElement != null)
                e.CurrentElement = new XElement(statElement);
            if (linkedViewElement != null)
                e.LinkedViewElement = new XElement(linkedViewElement);
        }
        
        private bool RunInputOrOutputCalculation(ref XElement currentElement,
            ref XElement linkedViewElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            CalculatorHelpers.CALCULATOR_TYPES eCalculatorType
                = CalculatorHelpers.GetCalculatorType(
                this.GCCalculatorParams.CalculatorType);
            IOCalculator ioCalculator = new IOCalculator();
            switch (eCalculatorType)
            {
                //these two apps are expected to need separate calcs in the future
                case CalculatorHelpers.CALCULATOR_TYPES.input:
                    bHasCalculations
                        = ioCalculator.SetInputOutputCalculations(
                        eCalculatorType, this.GCCalculatorParams, ref linkedViewElement,
                        ref currentElement, updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.output:
                    bHasCalculations
                        = ioCalculator.SetInputOutputCalculations(
                        eCalculatorType, this.GCCalculatorParams, ref linkedViewElement,
                        ref currentElement, updates);
                    break;
                default:
                    break;
            }
            return bHasCalculations;
        }
    }
}
