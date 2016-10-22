using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		1. Build the object models needed to carry out analyses.
    ///Date:		2015, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// NOTES        1. Each base element holds an ME2Stock. Analysis object (Total1, Stat1) 
    ///             holds all calcs and analysis.
    ///             Initial MandE calculators are stored as follows:
    ///             ME2Stock.Total1.ME2Indicators
    ///             Final analyses are stored as follows: 
    ///             ME2Stock.Total1.ME2IndicatorStocks{x].ME2Calculator.ME2Indicators
    /// </summary>
    public class BIME2StockCalculator : BudgetInvestmentCalculator
    {
        public BIME2StockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set Indic1Stock
            Init();
        }
        public BIME2StockCalculator()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //set ME2DescendentStock state so that descendant stock totals will have good base properties
            //the base.Analyzer is set when the Save... methods are run
            this.ME2DescendentStock = new ME2Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (this.GCCalculatorParams.LinkedViewElement != null)
            {
                this.ME2DescendentStock.SetDescendantME2StockProperties(
                    this.GCCalculatorParams.LinkedViewElement);
            }
        }
        //stateful analyzer used to set base.Analyzer properties in descendants (name, id)
        //and to hold descendent input and output stocks for analysis
        public ME2Stock ME2DescendentStock { get; set; }

        //these objects hold collections of ME2s for running totals.
        public BudgetInvestmentGroup BudgetGroup { get; set; }
        public OperationComponentGroup OCGroup { get; set; }
        public OutcomeGroup OutcomeGroup { get; set; }
        public InputGroup InputGroup { get; set; }
        public OutputGroup OutputGroup { get; set; }
        public BudgetInvestment Budget { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public OperationComponent OpComp { get; set; }
        public Outcome Outcome { get; set; }
        public Output Output { get; set; }
        public Input Input { get; set; }

        public bool AddME2CalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //multiple analyzers with rcalctype = me can confuse subscriber
            ChangeCalculator(currentElement, ref currentCalculationsElement);
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetBIGroupME2Calculations(
                   ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                bHasCalculations = SetOutcomeME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                bHasCalculations = SetTechOutputME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = SetOpOrCompME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetTechInputME2Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }

        private void SetIds(Calculator1 baseElement, ME2Stock stock)
        {
            SetCalculatorId(baseElement, stock);
            //alt2 is stored with the base element for some elements (ocs and outcomes, tps)
            if (baseElement.Alternative2 != 0)
            {
                //the CalculatorId is used to set the Id (Id is the base element_
                stock.Alternative2 = baseElement.Alternative2;
            }
        }
        private void SetCalculatorId(Calculator1 baseElement, ME2Stock stock)
        {
            //the initial collections don't store initial stock calculators
            //related calculator.Id are stored with the base element
            if (baseElement.CalculatorId != 0)
            {
                //the CalculatorId is used to set the Id (Id is the base element_
                stock.CalculatorId = baseElement.CalculatorId;
                stock.Id = baseElement.CalculatorId;
            }
        }
        public bool SetBIGroupME2Calculations(ref XElement currentCalculationsElement,
           ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.BudgetGroup == null)
            {
                this.BudgetGroup = new BudgetInvestmentGroup();
            }
            this.BudgetGroup.SetBudgetInvestmentGroupProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //easier to set multipliers with base els; copy into me2; subsequent CopyStockCalc handles automatically
            this.BudgetGroup.Multiplier = 1;
            //set this.ME2 props
            ME2Stock me2 = SetME2Properties(this.BudgetGroup, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.BudgetGroup.Calculators == null)
            {
                this.BudgetGroup.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.BudgetGroup.Calculators.Add(me2);
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetBIME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroug)
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.BudgetGroup == null)
            {
                this.BudgetGroup = new BudgetInvestmentGroup();
            }
            if (this.Budget == null)
            {
                this.Budget = new BudgetInvestment();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.Budget.SetBudgetInvestmentProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            this.Budget.Multiplier = 1;
            ME2Stock me2 = SetME2Properties(this.Budget, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Budget.Calculators == null)
            {
                this.Budget.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.Budget.Calculators.Add(me2);
            }
            if (this.BudgetGroup.BudgetInvestments == null)
            {
                this.BudgetGroup.BudgetInvestments = new List<BudgetInvestment>();
            }
            //calculators start with inputs and outputs
            //don't use byref collections (or this.Budget = null will set byref members to null)
            AddNewBudgetToCollection(this.Budget, this.BudgetGroup.BudgetInvestments);
            //reset for next collection
            this.Budget = null;
            bHasCalculations = true;
            return bHasCalculations;
        }
        private void AddNewBudgetToCollection(BudgetInvestment budI, List<BudgetInvestment> bis)
        {
            BudgetInvestment bi = new BudgetInvestment(this.GCCalculatorParams, budI);
            bi.Calculators = new List<Calculator1>();
            if (budI.Calculators != null)
            {
                ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, budI.Calculators, bi.Calculators);
            }
            //calculators start with inputs and outputs
            bi.TimePeriods = new List<Extensions.TimePeriod>();
            if (budI.TimePeriods != null)
            {
                foreach (TimePeriod tp in budI.TimePeriods)
                {
                    AddNewTPToCollection(tp, bi.TimePeriods);
                }
            }
            bis.Add(bi);
        }

        public bool SetTimePeriodME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget for holding collection of timeperiods
            if (this.Budget == null)
            {
                this.Budget = new BudgetInvestment();
            }
            if (this.TimePeriod == null)
            {
                this.TimePeriod = new TimePeriod();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.TimePeriod.SetTimePeriodProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.TimePeriod.Multiplier = this.TimePeriod.Amount;
            ME2Stock me2 = SetME2Properties(this.TimePeriod, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.TimePeriod.Calculators == null)
            {
                this.TimePeriod.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.TimePeriod.Calculators.Add(me2);
            }
            //add to collection
            if (this.Budget.TimePeriods == null)
            {
                this.Budget.TimePeriods = new List<TimePeriod>();
            }
            //calculators start with inputs and outputs
            AddNewTPToCollection(this.TimePeriod, this.Budget.TimePeriods);
            //if startingnode is a tp, won't have a good starting budgetgroup 
            //(stops running calcs at tp level)
            AddNewBudgetToTP();
            //reset for next collection
            this.TimePeriod = null;
            bHasCalculations = true;
            return bHasCalculations;
        }
        private void AddNewTPToCollection(TimePeriod timeP, List<TimePeriod> tps)
        {
            TimePeriod tp = new TimePeriod(timeP);
            tp.Calculators = new List<Calculator1>();
            if (timeP.Calculators != null)
            {
                ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, timeP.Calculators, tp.Calculators);
            }
            //calculators start with inputs and outputs
            tp.OperationComponents = new List<Extensions.OperationComponent>();
            if (timeP.OperationComponents != null)
            {
                foreach (OperationComponent oc in timeP.OperationComponents)
                {
                    AddNewOCToCollection(oc, tp.OperationComponents);
                }
            }
            tp.Outcomes = new List<Extensions.Outcome>();
            if (timeP.Outcomes != null)
            {
                foreach (Outcome outcome in timeP.Outcomes)
                {
                    AddNewOutcomeToCollection(outcome, tp.Outcomes);
                }
            }
            tps.Add(tp);
        }
        private void AddNewBudgetToTP()
        {
            //if startingnode is a tp, won't have a good starting budgetgroup (stops running calcs at tp level)
            if (this.GCCalculatorParams.StartingDocToCalcNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || this.GCCalculatorParams.StartingDocToCalcNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //even when analyses are run at tp, still want group-based object model
                if (this.BudgetGroup == null)
                {
                    this.BudgetGroup = new BudgetInvestmentGroup();
                }
                if (this.BudgetGroup.BudgetInvestments == null)
                {
                    this.BudgetGroup.BudgetInvestments = new List<BudgetInvestment>();
                }
                if (this.Budget != null)
                {
                    AddNewBudgetToCollection(this.Budget, this.BudgetGroup.BudgetInvestments);
                    this.Budget = null;
                }
            }
        }

        public bool SetOCGroupME2Calculations(ref XElement currentCalculationsElement,
          ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.OCGroup == null)
            {
                this.OCGroup = new OperationComponentGroup();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.OCGroup.SetOperationComponentGroupProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.OCGroup.Multiplier = 1;
            //set me2 props
            ME2Stock me2 = SetME2Properties(this.OCGroup, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OCGroup.Calculators == null)
            {
                this.OCGroup.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.OCGroup.Calculators.Add(me2);
            }
            //add to collection
            if (this.OCGroup.OperationComponents == null)
            {
                this.OCGroup.OperationComponents = new List<OperationComponent>();
            }
            //calculators start with inputs and outputs
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetOpOrCompME2Calculations(ref XElement currentCalculationsElement,
           ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent tp for holding collection of opcomps
            if (this.GCCalculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.operationprices
                || this.GCCalculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                if (this.OCGroup == null)
                {
                    this.OCGroup = new OperationComponentGroup();
                }
            }
            else
            {
                if (this.TimePeriod == null)
                {
                    this.TimePeriod = new TimePeriod();
                }
            }
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.OpComp.SetOperationComponentProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.OpComp.Multiplier = this.OpComp.Amount;
            ME2Stock me2 = SetME2Properties(this.OpComp, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OpComp.Calculators == null)
            {
                this.OpComp.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.OpComp.Calculators.Add(me2);
            }
            //calculators start with inputs and outputs
            //add to collection
            if (this.GCCalculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.operationprices
                || this.GCCalculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                if (this.OCGroup.OperationComponents == null)
                {
                    this.OCGroup.OperationComponents = new List<OperationComponent>();
                }
                AddNewOCToCollection(this.OpComp, this.OCGroup.OperationComponents);
            }
            else
            {
                if (this.TimePeriod.OperationComponents == null)
                {
                    this.TimePeriod.OperationComponents = new List<OperationComponent>();
                }
                AddNewOCToCollection(this.OpComp, this.TimePeriod.OperationComponents);
            }
            //reset for next collection
            this.OpComp = null;
            bHasCalculations = true;
            return bHasCalculations;
        }
        private void AddNewOCToCollection(OperationComponent opComp, List<OperationComponent> ocs)
        {
            OperationComponent oc = new OperationComponent(this.GCCalculatorParams, opComp);
            oc.Calculators = new List<Calculator1>();
            if (opComp.Calculators != null)
            {
                ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, opComp.Calculators, oc.Calculators);
            }
            oc.Inputs = new List<Extensions.Input>();
            if (opComp.Inputs != null)
            {
                foreach (Input input in opComp.Inputs)
                {
                    Input i = new Input(input);
                    i.Calculators = new List<Calculator1>();
                    if (input.Calculators != null)
                    {
                        ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, input.Calculators, i.Calculators);
                    }
                    oc.Inputs.Add(i);
                }
            }
            ocs.Add(oc);
        }

        public bool SetOutcomeGroupME2Calculations(ref XElement currentCalculationsElement,
          ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.OutcomeGroup == null)
            {
                this.OutcomeGroup = new OutcomeGroup();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.OutcomeGroup.SetOutcomeGroupProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.OutcomeGroup.Multiplier = 1;
            //set me2 props
            ME2Stock me2 = SetME2Properties(this.OutcomeGroup, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OutcomeGroup.Calculators == null)
            {
                this.OutcomeGroup.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.OutcomeGroup.Calculators.Add(me2);
            }
            //add to collection
            if (this.OutcomeGroup.Outcomes == null)
            {
                this.OutcomeGroup.Outcomes = new List<Outcome>();
            }
            //calculators start with inputs and outputs
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetOutcomeME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent tp for holding collection of outcomes
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                if (this.OutcomeGroup == null)
                {
                    this.OutcomeGroup = new OutcomeGroup();
                }
            }
            else
            {
                if (this.TimePeriod == null)
                {
                    this.TimePeriod = new TimePeriod();
                }
            }
            if (this.Outcome == null)
            {
                this.Outcome = new Outcome();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.Outcome.SetOutcomeProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.Outcome.Multiplier = this.Outcome.Amount;
            ME2Stock me2 = SetME2Properties(this.Outcome, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Outcome.Calculators == null)
            {
                this.Outcome.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.Outcome.Calculators.Add(me2);
            }
            //calculators start with inputs and outputs
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices)
            {
                if (this.OutcomeGroup.Outcomes == null)
                {
                    this.OutcomeGroup.Outcomes = new List<Outcome>();
                }
                AddNewOutcomeToCollection(this.Outcome, this.OutcomeGroup.Outcomes);
            }
            else
            {
                if (this.TimePeriod.Outcomes == null)
                {
                    this.TimePeriod.Outcomes = new List<Outcome>();
                }
                AddNewOutcomeToCollection(this.Outcome, this.TimePeriod.Outcomes);
            }
            //reset for next collection
            this.Outcome = null;
            bHasCalculations = true;
            return bHasCalculations;
        }
        private void AddNewOutcomeToCollection(Outcome oc, List<Outcome> outcomes)
        {
            Outcome outcome = new Outcome(this.GCCalculatorParams, oc);
            outcome.Calculators = new List<Calculator1>();
            if (oc.Calculators != null)
            {
                ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, oc.Calculators, outcome.Calculators);
            }
            outcome.Outputs = new List<Extensions.Output>();
            if (oc.Outputs != null)
            {
                foreach (Output output in oc.Outputs)
                {
                    Output o = new Output(output);
                    o.Calculators = new List<Calculator1>();
                    if (output.Calculators != null)
                    {
                        ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, output.Calculators, o.Calculators);
                    }
                    outcome.Outputs.Add(o);
                }
            }
            outcomes.Add(outcome);
        }

        public bool SetInputGroupME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.InputGroup == null)
            {
                this.InputGroup = new InputGroup();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.InputGroup.SetInputGroupProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.InputGroup.Multiplier = 1;
            ME2Stock me2 = SetME2Properties(this.InputGroup, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.InputGroup.Calculators == null)
            {
                this.InputGroup.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.InputGroup.Calculators.Add(me2);
            }
            if (this.InputGroup.Inputs == null)
            {
                this.InputGroup.Inputs = new List<Input>();
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetInputME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostInputs
            if (this.InputGroup == null)
            {
                this.InputGroup = new InputGroup();
            }
            //might have inputseries
            if (this.Input == null)
            {
                this.Input = new Input();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.Input.SetInputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.Input.Multiplier = this.Input.OCAmount;
            ME2Stock me2 = SetME2Properties(this.Input, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Input.Calculators == null)
            {
                this.Input.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.Input.Calculators.Add(me2);
            }
            if (this.InputGroup.Inputs == null)
            {
                this.InputGroup.Inputs = new List<Input>();
            }
            AddNewInput(this.InputGroup, this.Input);
            bHasCalculations = true;
            //reset for next series collection
            this.Input = null;
            return bHasCalculations;
        }

        private void AddNewInput(InputGroup inGroup, Input input)
        {
            Input i = new Input(input);
            i.Calculators = new List<Calculator1>();
            if (input.Calculators != null)
            {
                ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, input.Calculators, i.Calculators);
            }
            i.Inputs = new List<Extensions.Input>();
            if (input.Inputs != null)
            {
                foreach (Input inputseries in input.Inputs)
                {
                    Input inputseries2 = new Input(inputseries);
                    inputseries2.Calculators = new List<Calculator1>();
                    if (inputseries.Calculators != null)
                    {
                        ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, inputseries.Calculators, inputseries2.Calculators);
                    }
                    i.Inputs.Add(inputseries2);
                }
            }
            inGroup.Inputs.Add(i);
        }
        public bool SetInputSeriesME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostInputs
            if (this.Input == null)
            {
                this.Input = new Input();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            Input inputserie = new Input();
            inputserie.SetInputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            inputserie.Multiplier = inputserie.OCAmount;
            ME2Stock me2 = SetME2Properties(inputserie, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (inputserie.Calculators == null)
            {
                inputserie.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                inputserie.Calculators.Add(me2);
            }
            if (this.Input.Inputs == null)
            {
                this.Input.Inputs = new List<Input>();
            }
            this.Input.Inputs.Add(inputserie);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetTechInputME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostInputs
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.Input = new Input();
            this.Input.SetInputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            if (this.GCCalculatorParams.SubApplicationType 
                == Constants.SUBAPPLICATION_TYPES.investments
                || this.GCCalculatorParams.SubApplicationType
                == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                this.Input.Multiplier = this.Input.CAPAmount * this.Input.Times;
            }
            else
            {
                this.Input.Multiplier = this.Input.OCAmount * this.Input.Times;
            }
            ME2Stock me2 = SetME2Properties(this.Input, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Input.Calculators == null)
            {
                this.Input.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.Input.Calculators.Add(me2);
            }
            //add to collection
            if (this.OpComp.Inputs == null)
            {
                this.OpComp.Inputs = new List<Input>();
            }
            this.OpComp.Inputs.Add(this.Input);
            bHasCalculations = true;
            return bHasCalculations;
        }

        public bool SetOutputGroupME2Calculations(ref XElement currentCalculationsElement,
             ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.OutputGroup == null)
            {
                this.OutputGroup = new OutputGroup();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.OutputGroup.SetOutputGroupProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.OutputGroup.Multiplier = 1;
            ME2Stock me2 = SetME2Properties(this.OutputGroup, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OutputGroup.Calculators == null)
            {
                this.OutputGroup.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.OutputGroup.Calculators.Add(me2);
            }
            if (this.OutputGroup.Outputs == null)
            {
                this.OutputGroup.Outputs = new List<Output>();
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }
        public bool SetOutputME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostOutputs
            if (this.OutputGroup == null)
            {
                this.OutputGroup = new OutputGroup();
            }
            //might have inputseries
            if (this.Output == null)
            {
                this.Output = new Output();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.Output.SetOutputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.Output.Multiplier = this.Output.Amount;
            ME2Stock me2 = SetME2Properties(this.Output, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Output.Calculators == null)
            {
                this.Output.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.Output.Calculators.Add(me2);
            }
            if (this.OutputGroup.Outputs == null)
            {
                this.OutputGroup.Outputs = new List<Output>();
            }
            AddNewOutput(this.OutputGroup, this.Output);
            bHasCalculations = true;
            //reset for next series collection
            this.Output = null;
            return bHasCalculations;
        }

        private void AddNewOutput(OutputGroup outGroup, Output output)
        {
            Output o = new Output(output);
            o.Calculators = new List<Calculator1>();
            if (output.Calculators != null)
            {
                ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, output.Calculators, o.Calculators);
            }
            o.Outputs = new List<Extensions.Output>();
            if (output.Outputs != null)
            {
                foreach (Output outputseries in output.Outputs)
                {
                    Output outputseries2 = new Output(outputseries);
                    outputseries2.Calculators = new List<Calculator1>();
                    if (outputseries.Calculators != null)
                    {
                        ME2AnalyzerHelper.CopyStockCalculator(this.GCCalculatorParams, outputseries.Calculators, outputseries2.Calculators);
                    }
                    o.Outputs.Add(outputseries2);
                }
            }
            outGroup.Outputs.Add(o);
        }
        public bool SetOutputSeriesME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostOutputs
            if (this.Output == null)
            {
                this.Output = new Output();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            Output outputserie = new Output();
            outputserie.SetOutputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            outputserie.Multiplier = outputserie.Amount;
            ME2Stock me2 = SetME2Properties(outputserie, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (outputserie.Calculators == null)
            {
                outputserie.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                outputserie.Calculators.Add(me2);
            }
            if (this.Output.Outputs == null)
            {
                this.Output.Outputs = new List<Output>();
            }
            this.Output.Outputs.Add(outputserie);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetTechOutputME2Calculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostOutputs
            if (this.Outcome == null)
            {
                this.Outcome = new Outcome();
            }
            //this also sets the labels, groupid, and typeid for aggregation
            this.Output = new Output();
            this.Output.SetOutputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.Output.Multiplier = this.Output.Amount * this.Output.Times * this.Output.CompositionAmount;
            ME2Stock me2 = SetME2Properties(this.Output, currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Output.Calculators == null)
            {
                this.Output.Calculators = new List<Calculator1>();
            }
            if (me2 != null)
            {
                this.Output.Calculators.Add(me2);
            }
            //add to collection
            if (this.Outcome.Outputs == null)
            {
                this.Outcome.Outputs = new List<Output>();
            }
            this.Outcome.Outputs.Add(this.Output);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public ME2Stock SetME2Properties(CostBenefitCalculator baseElement, 
            XElement currentCalculationsElement, XElement currentElement)
        {
            ME2Stock me2 = new ME2Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //204 not used
            //if (this.ME2DescendentStock != null)
            //{
            //    //only property set by analyzer
            //    me2.TotalME2Stage = this.ME2DescendentStock.TotalME2Stage;
            //}
            ME2Calculator me2Calc = new ME2Calculator(this.GCCalculatorParams);
            me2.CalcParameters.CurrentElementNodeName = currentElement.Name.LocalName;
            bool bHasCalcs = false;
            if (currentCalculationsElement != null)
            {
                //have to make sure its not a stockanalyzer
                string sCalculatorType = CalculatorHelpers.GetAttribute(
                    currentCalculationsElement, Calculator1.cCalculatorType);
                if (sCalculatorType == ME2CalculatorHelper.CALCULATOR_TYPES.me2.ToString())
                {
                    //deserialize xml to object
                    me2Calc.SetME2Properties(currentCalculationsElement, currentElement);
                    //only need alttype and targettype (alts are groupedby using base elements)
                    baseElement.AlternativeType = me2Calc.AlternativeType;
                    baseElement.TargetType = me2Calc.TargetType;
                    bHasCalcs = true;
                }
            }
            if (!bHasCalcs)
            {
                //see if a sibling holds the calculations (currentCalcs could be the analyzer)
                XElement lv = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                    Calculator1.cCalculatorType, ME2CalculatorHelper.CALCULATOR_TYPES.me2.ToString());
                if (lv != null)
                {
                    //deserialize xml to object
                    me2Calc.SetME2Properties(lv, currentElement);
                    //only need alttype and targettype (alts are groupedby using base elements)
                    baseElement.AlternativeType = me2Calc.AlternativeType;
                    baseElement.TargetType = me2Calc.TargetType;
                    bHasCalcs = true;
                }
            }
            if (!bHasCalcs)
            {
                //else don't need the ids from calculator
                me2.CopyCalculatorProperties(baseElement);
            }
            //all calcs and analyses are stored in the appropriate analysis object (i.e. me2.Total, me2.Stat)
            me2.InitTotalME2StocksProperties();
            //copy the initial calculator to the appropriate analysis object.ME2Indicators collection
            me2.CopyME2CalculatorToME2Stock(me2Calc);
            me2.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
            //label and date comes from baseelement
            me2.Label = baseElement.Label;
            me2.Date = baseElement.Date;
            //kiss with the multipliers
            me2.Multiplier = baseElement.Multiplier;
            me2.CalculatorId = me2Calc.Id;
            //adjust id if children analyzers are being inserted/updated
            ChangeCalculatorIdForUpdatedChildren(currentElement, ref currentCalculationsElement, me2);
            return me2;
        }
        
        private void ChangeCalculatorIdForUpdatedChildren(XElement currentElement,
            ref XElement calculator, ME2Stock me2)
        {
            //alternatives are set using ME calculators (or input/output calculators)
            bool bIsChildrenUpdate
                = CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, currentElement.Name.LocalName);
            if (bIsChildrenUpdate)
            {
                //don't change the starting calculator
                if (this.GCCalculatorParams.StartingDocToCalcNodeName
                    == currentElement.Name.LocalName)
                {
                    return;
                }
                XElement lv = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                    Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType.ToString());
                //init with random calculator id
                if (this.GCCalculatorParams.RndGenerator == null)
                    this.GCCalculatorParams.RndGenerator = new Random();
                int iCalcId = this.GCCalculatorParams.RndGenerator.Next();
                if (lv != null)
                {
                    //switch to a calc that needs to be updated
                    iCalcId = CalculatorHelpers.GetAttributeInt(lv, Calculator1.cId);
                }
                else
                {
                    if (calculator != null)
                    {
                        //use the existing calc (gencalcsubscriber will replace with a new one anyway)
                        lv = new XElement(calculator);
                    }
                }
                if (lv != null)
                {
                    //change analyzer so that it can be inserted or updated (instead of the existing me calc)
                    CalculatorHelpers.SetAttributeInt(lv, Calculator1.cId, iCalcId);
                    CalculatorHelpers.SetAttributeInt(lv, Calculator1.cCalculatorId, iCalcId);
                    //important for updates that this get set to string.empty
                    CalculatorHelpers.SetAttribute(lv, Calculator1.cCalculatorType, string.Empty);
                    CalculatorHelpers.SetAttribute(lv, Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //tells subscriber to update/insert child
                    calculator = new XElement(lv);
                }
                if (iCalcId != 0 )
                {
                    me2.Id = iCalcId;
                    me2.CalculatorId = iCalcId;
                    if (me2.Total1 != null)
                    {
                        me2.Total1.Id = iCalcId;
                        me2.Total1.CalculatorId = iCalcId;
                    }
                    if (me2.Stat1 != null)
                    {
                        me2.Stat1.Id = iCalcId;
                        me2.Stat1.CalculatorId = iCalcId;
                    }
                    if (me2.Change1 != null)
                    {
                        me2.Change1.Id = iCalcId;
                        me2.Change1.CalculatorId = iCalcId;
                    }
                    if (me2.Progress1 != null)
                    {
                        me2.Progress1.Id = iCalcId;
                        me2.Progress1.CalculatorId = iCalcId;
                    }
                }
            }
        }
        public void ChangeCalculator(XElement currentElement,
            ref XElement calculator)
        {
            //these analyzers use atts from calculators only
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString()
                || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                //don't change the starting calculator
                if (this.GCCalculatorParams.StartingDocToCalcNodeName
                    == currentElement.Name.LocalName)
                {
                    return;
                }
                bool bIsChildren
                    = CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, currentElement.Name.LocalName);
                bool bIsTPGrandChild = 
                    (currentElement.Name.LocalName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || currentElement.Name.LocalName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    ? true : false;
                //need the alt or target type from mecalcs
                if (bIsChildren || bIsTPGrandChild)
                {
                    bool bIsMECalculator = false;
                    string sCurrentCalcType = string.Empty;
                    if (calculator != null)
                    {
                        sCurrentCalcType = CalculatorHelpers.GetAttribute(calculator, Calculator1.cCalculatorType);
                    }
                    if (!string.IsNullOrEmpty(sCurrentCalcType))
                    {
                        //see if it matches a real me calctype
                        CalculatorHelpers.CALCULATOR_TYPES calcType 
                            = CalculatorHelpers.GetCalculatorType(sCurrentCalcType);
                        if (calcType != CalculatorHelpers.CALCULATOR_TYPES.none)
                        {
                            bIsMECalculator = true;
                        }
                    }
                    if (calculator == null
                        || bIsMECalculator == false)
                    {
                        calculator = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                            Calculator1.cCalculatorType, currentElement.Name.LocalName);
                    }
                }
            }
        }
    }
}