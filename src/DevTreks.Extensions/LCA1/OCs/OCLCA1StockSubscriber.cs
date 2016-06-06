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
    ///Purpose:		This class derives from the CalculatorContracts.OperationComponentCalculator 
    ///             class. It subscribes to the RunCalculation and SetAncestorObjects events 
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class OCLCA1StockSubscriber : OperationComponentCalculator
    {
        //constructors
        public OCLCA1StockSubscriber() { }
        //constructor sets class (base) properties
        public OCLCA1StockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorsType
               == LCA1CalculatorHelper.CALCULATOR_TYPES.lifecycle1.ToString())
            {
                this.LCA1StockCalculator = new OCLCA1StockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public OCLCA1StockCalculator LCA1StockCalculator { get; set; }
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
            //run the stats and add them to statelement
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
                == Constants.SUBAPPLICATION_TYPES.operationprices)
            {
                if (this.GCCalculatorParams.RelatedCalculatorsType
                    == LCA1CalculatorHelper.CALCULATOR_TYPES.lifecycle1.ToString())
                {
                    bHasCalculations
                        = this.LCA1StockCalculator.AddCalculationsToCurrentElement(
                            ref currentCalculationsElement, ref currentElement);
                }
            }
            else if (this.GCCalculatorParams.SubApplicationType 
                == Constants.SUBAPPLICATION_TYPES.componentprices)
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
            return bHasCalculations;
        }
       
        public static double GetMultiplierForOperation(OperationComponent opComp)
        {
            //this subscriber does not use these multipliers
            double multiplier = 1;
            if (opComp != null)
            {
                multiplier = opComp.Amount;
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
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString()
                || this.GCCalculatorParams.StartingDocToCalcNodeName
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()))
            {
                //see the corresponding machinery stock pattern
            }
        }
    }
}
