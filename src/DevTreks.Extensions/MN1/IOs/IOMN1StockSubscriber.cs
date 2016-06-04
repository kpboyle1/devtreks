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
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This calculator didn't need the SetAncestorObjects event raised 
    ///             by the base class.
    ///             2. This calculator doesn't use cumulative totals, so analyzers using 
    ///             this class need to carry out all aggregations.
    /// </summary>
    public class IOMN1StockSubscriber : InputOutputCalculator
    {
        //constructors
        public IOMN1StockSubscriber() { }
        //constructor sets class (base) properties
        public IOMN1StockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorsType
                == MN1CalculatorHelper.CALCULATOR_TYPES.mn01.ToString())
            {
                this.MN1StockCalculator = new IOMN1StockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public IOMN1StockCalculator MN1StockCalculator { get; set; }
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
            MN1CalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = MN1CalculatorHelper.GetCalculatorType(
                this.GCCalculatorParams.CalculatorType);
            //run the stats and add them to statelement
            if (eCalculatorType == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01
                || eCalculatorType == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02)
            {
                e.HasCalculations = RunMN1Calculation(eCalculatorType,
                    ref statElement, ref linkedViewElement);
            }
            else
            {
                //run normally and save the same statelement and linkedviewelement
                e.HasCalculations = RunMN1Analysis(
                    ref statElement, ref linkedViewElement);
                //if (!this.HasTotals)
                //{
                //    //run normally and save the same statelement and linkedviewelement
                //    e.HasCalculations = RunMN1Analysis(
                //        ref statElement, ref linkedViewElement);
                //}
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
        private bool RunMN1Calculation(MN1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            ref XElement currentElement, ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            switch (calculatorType)
            {
                case MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01:
                    //serialize, run calcs, and deserialize
                    MNC1Calculator mnc1 = new MNC1Calculator();
                    bHasCalculations = mnc1.SetMNC1Calculations(calculatorType, this.GCCalculatorParams,
                        ref currentCalculationsElement, ref currentElement);
                    break;
                case MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02:
                    //serialize, run calcs, and deserialize
                    MNB1Calculator mnb1 = new MNB1Calculator();
                    bHasCalculations = mnb1.SetMNB1Calculations(calculatorType, this.GCCalculatorParams,
                        ref currentCalculationsElement, ref currentElement);
                    break;
                default:
                    //should be running an analysis
                    break;
            }
            return bHasCalculations;
        }
        private bool RunMN1Analysis(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.RelatedCalculatorsType
               == MN1CalculatorHelper.CALCULATOR_TYPES.mn01.ToString())
            {
                bHasCalculations
                    = this.MN1StockCalculator.AddCalculationsToCurrentElement(
                        ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        
        private bool TransferErrors(bool hasCalculations)
        {
            bool bHasGoodCalculations = hasCalculations;
            if (this.MN1StockCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.MN1StockCalculator.BIMN1Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.MN1StockCalculator.BIMN1Calculator.GCCalculatorParams.ErrorMessage;
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
       
    }
}
