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
    ///Purpose:		This class derives from the CalculatorContracts.OperationComponentCalculator 
    ///             class. It subscribes to the RunCalculation and SetAncestorObjects events 
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class OCMachineryStockSubscriber : OperationComponentCalculator
    {
        //constructors
        public OCMachineryStockSubscriber() { }
        //constructor sets class (base) properties
        public OCMachineryStockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                {
                    this.Machinery2StockCalculator = new OCMachinery2StockCalculator(calcParameters);
                }
                else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    this.Machinery2StockCalculator = new OCMachinery2StockCalculator(calcParameters);
                }
                else
                {
                    this.MachineryStockCalculator = new OCMachineryStockCalculator(calcParameters);
                }
            }
            else if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
            {
                this.IrrPowerStockCalculator = new OCIrrPowerStockCalculator(calcParameters);
            }
            else if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
                this.GeneralCapitalStockCalculator = new OCGeneralCapitalStockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculators that can be run by this extension
        public OCMachineryStockCalculator MachineryStockCalculator { get; set; }
        public OCMachinery2StockCalculator Machinery2StockCalculator { get; set; }
        public OCIrrPowerStockCalculator IrrPowerStockCalculator { get; set; }
        public OCGeneralCapitalStockCalculator GeneralCapitalStockCalculator { get; set; }
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
            if(this.GCCalculatorParams.CalculatorType.StartsWith(
                CalculatorHelpers.CALCULATOR_TYPES.operation.ToString()))
            {
                if (this.GCCalculatorParams.RelatedCalculatorType
                    == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                {
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                    {
                        bHasCalculations
                            = this.Machinery2StockCalculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                    {
                        bHasCalculations
                            = this.Machinery2StockCalculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    else 
                    {
                        //assume resources01 is default
                        bHasCalculations
                            = this.MachineryStockCalculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
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
            }
            else if(this.GCCalculatorParams.CalculatorType.StartsWith(
                CalculatorHelpers.CALCULATOR_TYPES.component.ToString()))
            {
                if (this.GCCalculatorParams.RelatedCalculatorType
                    == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                {
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                    {
                        bHasCalculations
                            = this.Machinery2StockCalculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                    {
                        bHasCalculations
                            = this.Machinery2StockCalculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    else 
                    {
                        //assume resources01 is default
                        bHasCalculations
                            = this.MachineryStockCalculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
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
                if (this.MachineryStockCalculator != null)
                {
                    this.MachineryStockCalculator.BIM1Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.Machinery2StockCalculator != null)
                {
                    this.Machinery2StockCalculator.BIM2Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                    this.Machinery2StockCalculator.BIM2aCalculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.IrrPowerStockCalculator != null)
                {
                    this.IrrPowerStockCalculator.BIIrrCalculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.GeneralCapitalStockCalculator != null)
                {
                    this.GeneralCapitalStockCalculator.BIGCCalculator.GCCalculatorParams.ParentOperationComponent
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
            //the bicalculators have their own set of calcs and analysis params
            if (this.Machinery2StockCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.Machinery2StockCalculator.BIM2Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.Machinery2StockCalculator.BIM2Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.Machinery2StockCalculator.BIM2aCalculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.Machinery2StockCalculator.BIM2aCalculator.GCCalculatorParams.ErrorMessage;
            }
            if (this.MachineryStockCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.MachineryStockCalculator.BIM1Calculator.GCCalculatorParams.ErrorMessage;
                this.GCCalculatorParams.ErrorMessage += this.MachineryStockCalculator.BIM1Calculator.GCCalculatorParams.ErrorMessage;
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
            //note that 0.8.8 did not end up using Use Same Calculator Pack in Children
            //for resources 02 or resources 02a analyses
            //but keep this pattern for future use
            if (this.GCCalculatorParams.NeedsCalculators
                && (this.GCCalculatorParams.StartingDocToCalcNodeName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString()
                || this.GCCalculatorParams.StartingDocToCalcNodeName
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()))
            {
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString()
                    || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    if (currentNodeName
                        == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()
                        || currentNodeName
                        == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
                    {
                        if (this.Machinery2StockCalculator != null)
                        {
                            //unless NeedsXmlDocOnly = false
                            //won't be able to insert a new calculator in children 
                            //(note that children replace the base npv operation calcor 
                            //passed into them)
                            //this.GCCalculatorParams.NeedsXmlDocOnly
                            //    = this.Machinery2StockCalculator.BIM2Calculator.GCCalculatorParams.NeedsXmlDocOnly;
                        }
                    }
                }
            }
        }
    }
}
