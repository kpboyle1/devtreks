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
    ///Purpose:		This class derives from the CalculatorContracts.BudgetInvestmentCalculator 
    ///             class. It subscribes to the RunCalculation and SetAncestorObjects events 
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2012, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class NPVBISubscriber : BudgetInvestmentCalculator
    {
        //constructors
        public NPVBISubscriber() { }
        //constructor sets class (base) properties
        public NPVBISubscriber(CalculatorParameters calcParameters,
            CalculatorHelpers.CALCULATOR_TYPES calculatorType)
            : base(calcParameters) 
        {
            this.CalculatorType = calculatorType;
        }

        //properties
        //calculators specific to this extension
        public CalculatorHelpers.CALCULATOR_TYPES CalculatorType { get; set; }
        //parameters needed by customdocs, in particular 
        //(this.GCCalculatorParams has the parameters passed back by 
        //the event publisher, still need original ones)
        public CalculatorParameters NPVCalculatorParams { get; set; }

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
            e.HasCalculations = RunBudgetInvestmentCalculation(
                ref statElement, ref linkedViewElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            if (statElement != null)
                e.CurrentElement = new XElement(statElement);
            if (linkedViewElement != null)
                e.LinkedViewElement = new XElement(linkedViewElement);
        }
    
        //descendants need properties from ancestor objects (i.e. realrate)
        //to run their calculations; the ancestors are kept statefully in this.GCCalculatorParams
        private void AddAncestorObjects(XElement currentElement)
        {
            XElement currentCalculationsElement = null;
            currentCalculationsElement
                    = CalculatorHelpers.GetCalculator(this.GCCalculatorParams,
                    currentElement);
            if (currentElement.Name.LocalName 
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName 
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                //set the stateful ancestor object
                this.GCCalculatorParams.ParentBudgetInvestmentGroup 
                    = new BudgetInvestmentGroup();
                //deserialize xml to object
                this.GCCalculatorParams.ParentBudgetInvestmentGroup.SetBudgetInvestmentGroupProperties(
                    this.GCCalculatorParams, currentCalculationsElement, currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentBudgetInvestmentGroup.InitTotalBenefitsProperties();
                this.GCCalculatorParams.ParentBudgetInvestmentGroup.InitTotalCostsProperties();
            }
            else if (currentElement.Name.LocalName 
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName 
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                //set the stateful ancestor object
                this.GCCalculatorParams.ParentBudgetInvestment 
                    = new BudgetInvestment();
                //deserialize xml to object
                this.GCCalculatorParams.ParentBudgetInvestment.SetBudgetInvestmentProperties(
                    this.GCCalculatorParams, currentCalculationsElement, currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentBudgetInvestment.InitTotalBenefitsProperties();
                this.GCCalculatorParams.ParentBudgetInvestment.InitTotalCostsProperties();

                //anyway to setpreprod annuities later rather than running here?
                SetAncestorBudgetInvestment(currentCalculationsElement, currentElement);

                //run preproduction and other annuity calculations
                BICalculator biCalculator = new BICalculator();
                biCalculator.SetPreProductionCalculations(
                    this.GCCalculatorParams, currentElement);
                //reset annuities collection
                this.GCCalculatorParams.ParentBudgetInvestment.AnnEquivalents = null;
            }
            else if (currentElement.Name.LocalName 
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName 
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //set the stateful ancestor object
                //last time period date's are used in some calculations
                DateTime dtLastTimePeriodDate = new DateTime(1000, 12, 1);
                bool bHasLastDate = false;
                if (this.GCCalculatorParams.ParentTimePeriod != null)
                {
                    bHasLastDate = true;
                    dtLastTimePeriodDate = this.GCCalculatorParams.ParentTimePeriod.Date;
                }
                this.GCCalculatorParams.ParentTimePeriod = new TimePeriod();
                //deserialize xml to object
                this.GCCalculatorParams.ParentTimePeriod.SetTimePeriodProperties(
                    this.GCCalculatorParams, currentCalculationsElement, currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentTimePeriod.InitTotalBenefitsProperties();
                this.GCCalculatorParams.ParentTimePeriod.InitTotalCostsProperties();
                if (this.GCCalculatorParams.ParentBudgetInvestment.PreProdPeriods == 0
                    && this.GCCalculatorParams.ParentBudgetInvestment.ProdPeriods <= 1)
                {
                    //no preproduction annuities
                    this.GCCalculatorParams.ParentBudgetInvestment.InitEOPDate
                        = this.GCCalculatorParams.ParentTimePeriod.Date;
                }
                //copy some of the last tps properties to current tp 
                if (!bHasLastDate) dtLastTimePeriodDate
                    = this.GCCalculatorParams.ParentTimePeriod.Date;
                this.GCCalculatorParams.ParentTimePeriod.LastPeriodDate
                    = dtLastTimePeriodDate;
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                this.GCCalculatorParams.ParentOutcome = new Outcome();
                this.GCCalculatorParams.ParentOutcome.SetOutcomeProperties(
                    this.GCCalculatorParams, currentCalculationsElement, currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentOutcome.InitTotalBenefitsProperties();
                SetAncestorOutcome(currentCalculationsElement, currentElement);
                //run joint output calculations 
                //these will be copied to descendant output nodes as calculations 
                //are run on those nodes
                NPVOutcomeCalculator outcomeCalculator = new NPVOutcomeCalculator();
                outcomeCalculator.SetJointOutputCalculations(
                    this.GCCalculatorParams);
            }
            else if (currentElement.Name.LocalName 
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName 
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                this.GCCalculatorParams.ParentOperationComponent = new OperationComponent();
                this.GCCalculatorParams.ParentOperationComponent.SetOperationComponentProperties(
                    this.GCCalculatorParams, currentCalculationsElement, currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentOperationComponent.InitTotalCostsProperties();
                SetAncestorOperationComponent(currentCalculationsElement, currentElement);
                //run joint input calculations (i.e. fuel amount, fuel cost)
                //these will be copied to descendant input nodes as calculations 
                //are run on those nodes
                OCCalculator ocCalculator = new OCCalculator();
                ocCalculator.SetJointInputCalculations(
                    this.GCCalculatorParams);
            }
        }
        private void SetAncestorBudgetInvestment(XElement currentCalculationsElement,
            XElement currentElement)
        {
            this.GCCalculatorParams.ParentBudgetInvestment.TimePeriods
                = new List<TimePeriod>();
            //use streaming techniques to process children
            XElement rootTps = this.GCCalculatorParams.GetTimePeriodsForBudgetInvestmentNode(
                currentElement);
            if (rootTps != null)
            {
                if (rootTps.HasElements)
                {
                    foreach (XElement tp in rootTps.Elements())
                    {
                        TimePeriod timePeriod = new TimePeriod();
                        //always use the original calculations
                        XElement timeXmlDoc
                           = new XElement(tp.Name.LocalName);
                        if (tp.Elements(Constants.ROOT_PATH).Any())
                        {
                            foreach (XElement xmlDoc in tp.Elements(Constants.ROOT_PATH))
                            {
                                timeXmlDoc.Add(xmlDoc);
                            }
                        }
                        else
                        {
                            timeXmlDoc = currentCalculationsElement;
                        }
                        timePeriod.SetTimePeriodProperties(
                            this.GCCalculatorParams, timeXmlDoc, tp);
                        //don't process previously calculated nodes
                        bool bIsAnnuity = TimePeriod.IsAnnuity(currentElement);
                        if (!bIsAnnuity)
                        {
                            this.GCCalculatorParams.ParentBudgetInvestment
                                .TimePeriods.Add(timePeriod);
                        }
                        timeXmlDoc = null;
                    }
                }
            }
        }
        private void SetAncestorOutcome(XElement currentCalculationsElement,
            XElement currentElement)
        {
            this.GCCalculatorParams.ParentOutcome.Outputs
                = new List<Output>();
            //use streaming techniques to process children
            XElement rootOutputs = this.GCCalculatorParams.GetOutputForOutcomeNode(
                currentElement);
            if (rootOutputs != null)
            {
                if (rootOutputs.HasElements)
                {
                    foreach (XElement output in rootOutputs.Elements())
                    {
                        Output oOutput = new Output();
                        //always use the original calculations
                        XElement outputXmlDoc = new XElement(output.Name.LocalName);
                        if (output.Elements(Constants.ROOT_PATH).Any())
                        {
                            foreach (XElement xmlDoc in output.Elements(Constants.ROOT_PATH))
                            {
                                outputXmlDoc.Add(xmlDoc);
                            }
                        }
                        else
                        {
                            outputXmlDoc = currentCalculationsElement;
                        }
                        oOutput.SetOutputProperties(
                            this.GCCalculatorParams, outputXmlDoc, output);
                        this.GCCalculatorParams.ParentOutcome.Outputs.Add(oOutput);
                        outputXmlDoc = null;
                    }
                }
            }
        }
        
        //stateful ancestor stores input calculations that have to be set 
        //using all of an operation/component's inputs (unlike the 
        //single node calculations run above)
        private void SetAncestorOperationComponent(XElement currentCalculationsElement,
            XElement currentElement)
        {
            this.GCCalculatorParams.ParentOperationComponent.Inputs 
                = new List<Input>();
            //use streaming techniques to process children
            XElement rootInputs = this.GCCalculatorParams.GetInputForOpOrCompNode(
                currentElement);
            if (rootInputs != null)
            {
                if (rootInputs.HasElements)
                {
                    foreach (XElement input in rootInputs.Elements())
                    {
                        Input oInput = new Input();
                        //always use the original calculations
                        XElement inputXmlDoc = new XElement(input.Name.LocalName);
                        if (input.Elements(Constants.ROOT_PATH).Any())
                        {
                            foreach (XElement xmlDoc in input.Elements(Constants.ROOT_PATH))
                            {
                                inputXmlDoc.Add(xmlDoc);
                            }
                        }
                        else
                        {
                            inputXmlDoc = currentCalculationsElement;
                        }
                        oInput.SetInputProperties(
                            this.GCCalculatorParams, inputXmlDoc, input);
                        this.GCCalculatorParams.ParentOperationComponent.Inputs.Add(oInput);
                        inputXmlDoc = null;
                    }
                }
            }
        }
        
        private bool RunBudgetInvestmentCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            CalculatorHelpers.CALCULATOR_TYPES eCalculatorType
                = CalculatorHelpers.GetCalculatorType(this.GCCalculatorParams.CalculatorType);
            BICalculator biCalculator = new BICalculator();
            switch (eCalculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.budget:
                    bHasCalculations
                        = biCalculator.SetBudgetAndInvestmentCalculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, this.GCCalculatorParams.Updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.investment:
                    bHasCalculations
                        = biCalculator.SetBudgetAndInvestmentCalculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, this.GCCalculatorParams.Updates);
                    break;
                default:
                    break;
            }
            return bHasCalculations;
        }
    }
}
