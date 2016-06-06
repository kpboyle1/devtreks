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
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class OCHCStockSubscriber : OperationComponentCalculator
    {
        //constructors
        public OCHCStockSubscriber() { }
        //constructor sets class (base) properties
        public OCHCStockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorType
                == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
            {
                this.HCStockCalculator = new OCHCStockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public OCHCStockCalculator HCStockCalculator { get; set; }
        //methods
        //subscribe to the events raised by the base class
        public bool RunCalculator()
        {
            bool bHasAnalysis = false;
            //subscribe to the event raised by the publisher delegate
            this.SetAncestorObjects += AddAncestorObjects;
            this.RunCalculation += AddCalculations;
            //run the calculation (raising the events
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveCalculation();
            return bHasAnalysis;
        }
        //define the actions to take when the event is raised
        public void AddAncestorObjects(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement statElement = new XElement(e.CurrentElement);
            //run the stats and add them to statelement
            AddAncestorObjects(statElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            e.CurrentElement = new XElement(statElement);
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
            e.HasCalculations = RunOperationComponentCalculation(
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
        private bool RunOperationComponentCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.CalculatorType.StartsWith(
                CalculatorHelpers.CALCULATOR_TYPES.operation.ToString()))
            {
                if (this.GCCalculatorParams.RelatedCalculatorType
                    == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                {
                    //assume resources01 is default
                    bHasCalculations
                        = this.HCStockCalculator.AddCalculationsToCurrentElement(
                            ref currentCalculationsElement, ref currentElement);
                }
            }
            else if (this.GCCalculatorParams.CalculatorType.StartsWith(
                CalculatorHelpers.CALCULATOR_TYPES.component.ToString()))
            {
                if (this.GCCalculatorParams.RelatedCalculatorType
                    == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                {
                    //assume resources01 is default
                    bHasCalculations
                        = this.HCStockCalculator.AddCalculationsToCurrentElement(
                            ref currentCalculationsElement, ref currentElement);
                }
            }
            bHasCalculations = TransferErrors(bHasCalculations);
            //some calcparams have to be passed back to this.calcparams
            UpdateCalculatorParams(currentElement.Name.LocalName);
            return bHasCalculations;
        }
        private void AddAncestorObjects(XElement currentElement)
        {
            XElement currentCalculationsElement = null;
            currentCalculationsElement
                    = CalculatorHelpers.GetCalculator(this.GCCalculatorParams,
                    currentElement);
            if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //need the oc.amount multiplier at the input level (so set the parentopcomp here)
                this.GCCalculatorParams.ParentOperationComponent
                 = new OperationComponent();
                this.GCCalculatorParams.ParentOperationComponent
                    .SetOperationComponentProperties(
                    this.GCCalculatorParams, currentCalculationsElement,
                    currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentOperationComponent.InitTotalCostsProperties();
                //all calculators need it
                if (this.HCStockCalculator != null)
                {
                    this.HCStockCalculator.BIHC1Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
            }
        }
        public static double GetMultiplierForOperation(OperationComponent opComp)
        {
            //ops, comps, budgets and investments use the times multiplier
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
            if (this.HCStockCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.HCStockCalculator.BIHC1Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.HCStockCalculator.BIHC1Calculator.GCCalculatorParams.ErrorMessage;
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
