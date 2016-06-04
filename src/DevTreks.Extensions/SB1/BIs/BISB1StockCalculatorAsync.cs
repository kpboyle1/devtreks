using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		1. Build the object models needed to carry out analyses.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. Only input and output calculators are added to the object model. 
    ///             In some instances, other calculators and analyzers may be needed 
    ///             to be added to the object model.
    /// </summary>
    public class BISB1StockCalculatorAsync : BudgetInvestmentCalculatorAsync
    {
        public BISB1StockCalculatorAsync(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set Indic1Stock
            Init();
        }
        public BISB1StockCalculatorAsync()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //set SB1DescendentStock state so that descendant stock totals will have good base properties
            //the base.Analyzer is set when the Save... methods are run
            this.SB1DescendentStock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (this.GCCalculatorParams.LinkedViewElement != null)
            {
                this.SB1DescendentStock.SetDescendantSB1StockProperties(
                    this.GCCalculatorParams.LinkedViewElement);
            }
        }
        //stateful Stock costs
        public SBC1Calculator SB1C1 { get; set; }
        //stateful Stock benefits
        public SBB1Calculator SB1B1 { get; set; }
        //some analyzers use other calculators and analyzers (Alternatives use NPV)
        public SB1Stock SB1 { get; set; }
        //stateful analyzer used to set base.Analyzer properties in descendants (name, id)
        //and to hold descendent input and output stocks for analysis
        public SB1Stock SB1DescendentStock { get; set; }

        //these objects hold collections of SB1s for running totals.
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

        public bool AddSB1CalculationsToCurrentElement(
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasCalculations = false;
            //don't use changecalc here -these don't use npv rcalcs types
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetBIGroupSB1Calculations(
                   currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBISB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                bHasCalculations = SetOutcomeSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                bHasCalculations = SetTechOutputSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = SetOpOrCompSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetTechInputSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            return bHasCalculations;
        }

        private void SetIds(Calculator1 baseElement, SB1Stock stock)
        {
            SetCalculatorId(baseElement, stock);
            //alt2 is stored with the base element for some elements (ocs and outcomes, tps)
            if (baseElement.Alternative2 != 0)
            {
                //the CalculatorId is used to set the Id (Id is the base element_
                stock.Alternative2 = baseElement.Alternative2;
            }
        }
        private void SetCalculatorId(Calculator1 baseElement, SB1Stock stock)
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
        public bool SetBIGroupSB1Calculations(XElement currentCalculationsElement,
           XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.BudgetGroup == null)
            {
                this.BudgetGroup = new BudgetInvestmentGroup();
            }
            this.BudgetGroup.SetBudgetInvestmentGroupProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            this.BudgetGroup.Multiplier = 1;
            //calculators start with inputs and outputs
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetBISB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
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
            SetSB1Properties(this.Budget, currentCalculationsElement, currentElement);
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

        public bool SetTimePeriodSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
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
            SetSB1Properties(this.TimePeriod, currentCalculationsElement, currentElement);
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

        public bool SetOCGroupSB1Calculations(XElement currentCalculationsElement,
          XElement currentElement)
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

        public bool SetOpOrCompSB1Calculations(XElement currentCalculationsElement,
           XElement currentElement)
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
            SetSB1Properties(this.OpComp, currentCalculationsElement, currentElement);
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
                AddStockCalculators(opComp.Calculators, oc.Calculators);
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
                        //add any base calculators
                        SB1AnalyzerHelper.AddInputCalculators(input.Calculators, i.Calculators);
                        //add any stock analyzers
                        AddStockInputCalculators(input.Calculators, i.Calculators);
                    }
                    oc.Inputs.Add(i);
                }
            }
            ocs.Add(oc);
        }

        public bool SetOutcomeGroupSB1Calculations(XElement currentCalculationsElement,
          XElement currentElement)
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

        public bool SetOutcomeSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
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
            SetSB1Properties(this.Outcome, currentCalculationsElement, currentElement);
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
                AddStockCalculators(oc.Calculators, outcome.Calculators);
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
                        //add base benefit calculators
                        SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
                        //add any stock analyzers
                        AddStockOutputCalculators(output.Calculators, o.Calculators);
                    }
                    outcome.Outputs.Add(o);
                }
            }
            outcomes.Add(outcome);
        }

        public bool SetInputGroupSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
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
            SetSB1IOProperties(this.InputGroup, currentCalculationsElement, currentElement);
            //add to collection
            if (this.InputGroup.Inputs == null)
            {
                this.InputGroup.Inputs = new List<Input>();
            }
            //add to collection
            if (this.SB1C1 != null)
            {
                //add the calculations to the base element
                if (this.InputGroup.Calculators == null)
                {
                    this.InputGroup.Calculators = new List<Calculator1>();
                }
                this.InputGroup.Calculators.Add(this.SB1C1);
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetInputSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostInputs
            if (this.InputGroup == null)
            {
                this.InputGroup = new InputGroup();
            }
            //this analysis only needs me1s calcs, if no currentcalcs, don't process further
            if (currentCalculationsElement != null)
            {
                //might have inputseries
                if (this.Input == null)
                {
                    this.Input = new Input();
                }
                //this also sets the labels, groupid, and typeid for aggregation
                this.Input.SetInputProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
                //base inputs only have ocamount
                this.Input.Multiplier = this.Input.OCAmount;
                SetSB1IOProperties(this.Input, currentCalculationsElement, currentElement);
                if (this.InputGroup.Inputs == null)
                {
                    this.InputGroup.Inputs = new List<Input>();
                }
                if (this.SB1C1 != null)
                {
                    //add the calculations to the base element
                    if (this.Input.Calculators == null)
                    {
                        this.Input.Calculators = new List<Calculator1>();
                    }
                    this.Input.Calculators.Add(this.SB1C1);
                }
                AddNewInput(this.InputGroup, this.Input);
                bHasCalculations = true;
            }
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
                SB1AnalyzerHelper.AddInputCalculators(input.Calculators, i.Calculators);
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
                        SB1AnalyzerHelper.AddInputCalculators(inputseries.Calculators, inputseries2.Calculators);
                    }
                    i.Inputs.Add(inputseries2);
                }
            }
            inGroup.Inputs.Add(i);
        }
        public bool SetInputSeriesSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostInputs
            if (this.Input == null)
            {
                this.Input = new Input();
            }
            //this analysis only needs me1s calcs, if no currentcalcs, don't process further
            if (currentCalculationsElement != null)
            {
                //this also sets the labels, groupid, and typeid for aggregation
                Input inputserie = new Input();
                inputserie.SetInputProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
                //base inputs only have ocamount
                inputserie.Multiplier = inputserie.OCAmount;
                SetSB1IOProperties(inputserie, currentCalculationsElement, currentElement);
                //add to collection
                if (this.Input.Inputs == null)
                {
                    this.Input.Inputs = new List<Input>();
                }
                if (this.SB1C1 != null)
                {
                    //add the calculations to the base element
                    if (inputserie.Calculators == null)
                    {
                        inputserie.Calculators = new List<Calculator1>();
                    }
                    inputserie.Calculators.Add(this.SB1C1);
                }
                this.Input.Inputs.Add(inputserie);
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public bool SetTechInputSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostInputs
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            if (currentCalculationsElement != null)
            {
                //this also sets the labels, groupid, and typeid for aggregation
                this.Input = new Input();
                this.Input.SetInputProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
                //base input calculator handles ocamount, aohamount, capamount
                //rules for this calculator:
                if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices
                    || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    this.Input.Multiplier = this.Input.Times * this.Input.CAPAmount;
                }
                else
                {
                    this.Input.Multiplier = this.Input.Times * this.Input.OCAmount;
                }
                SetSB1IOProperties(this.Input, currentCalculationsElement, currentElement);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                if (this.SB1C1 != null)
                {
                    //add the calculations to the base element
                    if (this.Input.Calculators == null)
                    {
                        this.Input.Calculators = new List<Calculator1>();
                    }
                    this.Input.Calculators.Add(this.SB1C1);
                }
                this.OpComp.Inputs.Add(this.Input);
                bHasCalculations = true;
            }
            return bHasCalculations;
        }

        public bool SetOutputGroupSB1Calculations(XElement currentCalculationsElement,
             XElement currentElement)
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
            SetSB1IOProperties(this.OutputGroup, currentCalculationsElement, currentElement);
            //add to collection
            if (this.OutputGroup.Outputs == null)
            {
                this.OutputGroup.Outputs = new List<Output>();
            }
            //add to collection
            if (this.SB1B1 != null)
            {
                //add the calculations to the base element
                if (this.OutputGroup.Calculators == null)
                {
                    this.OutputGroup.Calculators = new List<Calculator1>();
                }
                this.OutputGroup.Calculators.Add(this.SB1B1);
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetOutputSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostOutputs
            if (this.OutputGroup == null)
            {
                this.OutputGroup = new OutputGroup();
            }
            //this analysis only needs me1s calcs, if no currentcalcs, don't process further
            if (currentCalculationsElement != null)
            {
                //might have inputseries
                if (this.Output == null)
                {
                    this.Output = new Output();
                }
                //this also sets the labels, groupid, and typeid for aggregation
                this.Output.SetOutputProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
                this.Output.Multiplier = this.Output.Amount;
                SetSB1IOProperties(this.Output, currentCalculationsElement, currentElement);
                if (this.OutputGroup.Outputs == null)
                {
                    this.OutputGroup.Outputs = new List<Output>();
                }
                if (this.SB1B1 != null)
                {
                    //add the calculations to the base element
                    if (this.Output.Calculators == null)
                    {
                        this.Output.Calculators = new List<Calculator1>();
                    }
                    this.Output.Calculators.Add(this.SB1B1);
                }
                AddNewOutput(this.OutputGroup, this.Output);
                bHasCalculations = true;
            }
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
                SB1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
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
                        SB1AnalyzerHelper.AddOutputCalculators(outputseries.Calculators, outputseries2.Calculators);
                    }
                    o.Outputs.Add(outputseries2);
                }
            }
            outGroup.Outputs.Add(o);
        }
        public bool SetOutputSeriesSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostOutputs
            if (this.Output == null)
            {
                this.Output = new Output();
            }
            //this analysis only needs me1s calcs, if no currentcalcs, don't process further
            if (currentCalculationsElement != null)
            {
                //this also sets the labels, groupid, and typeid for aggregation
                Output outputserie = new Output();
                outputserie.SetOutputProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
                outputserie.Multiplier = outputserie.Amount;
                SetSB1IOProperties(outputserie, currentCalculationsElement, currentElement);
                //add to collection
                if (this.Output.Outputs == null)
                {
                    this.Output.Outputs = new List<Output>();
                }
                if (this.SB1B1 != null)
                {
                    //add the calculations to the base element
                    if (outputserie.Calculators == null)
                    {
                        outputserie.Calculators = new List<Calculator1>();
                    }
                    outputserie.Calculators.Add(this.SB1B1);
                }
                this.Output.Outputs.Add(outputserie);
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public bool SetTechOutputSB1Calculations(XElement currentCalculationsElement,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of mandECostOutputs
            if (this.Outcome == null)
            {
                this.Outcome = new Outcome();
            }
            if (currentCalculationsElement != null)
            {
                //this also sets the labels, groupid, and typeid for aggregation
                this.Output = new Output();
                this.Output.SetOutputProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
                //rules for outputs
                this.Output.Multiplier = this.Output.Amount * this.Output.CompositionAmount * this.Output.Times;
                SetSB1IOProperties(this.Output, currentCalculationsElement, currentElement);
                //add to collection
                if (this.Outcome.Outputs == null)
                {
                    this.Outcome.Outputs = new List<Output>();
                }
                if (this.SB1B1 != null)
                {
                    //add the calculations to the base element
                    if (this.Output.Calculators == null)
                    {
                        this.Output.Calculators = new List<Calculator1>();
                    }
                    this.Output.Calculators.Add(this.SB1B1);
                }
                this.Outcome.Outputs.Add(this.Output);
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public void SetSB1IOProperties(Calculator1 baseElement,
            XElement currentCalculationsElement, XElement currentElement)
        {
            if (currentCalculationsElement != null)
            {
                //have to make sure its not a stockanalyzer
                string sCalculatorType = CalculatorHelpers.GetAttribute(
                    currentCalculationsElement, Calculator1.cCalculatorType);
                if (sCalculatorType == SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
                {
                    this.SB1C1 = new SBC1Calculator();
                    //need ocamount, aohamount ... and local
                    this.SB1C1.SB1CInput.SetInputProperties(this.GCCalculatorParams,
                        currentCalculationsElement, currentElement);
                    //deserialize xml to object
                    this.SB1C1.SetSB1C1Properties(currentCalculationsElement, currentElement);
                }
                else if (sCalculatorType == SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
                {
                    this.SB1B1 = new SBB1Calculator();
                    //deserialize xml to object
                    this.SB1B1.SB1BOutput.SetOutputProperties(this.GCCalculatorParams,
                        currentCalculationsElement, currentElement);
                    this.SB1B1.SetSB1B1Properties(currentCalculationsElement, currentElement);
                }
                else
                {
                    XElement lv = null;
                    //look in currentElement (currentCalcsEl could be a stock analyzer (i.e. totals, stats)
                    if (currentElement.Name.LocalName.Contains(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        lv = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                            Calculator1.cCalculatorType, SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString());
                    }
                    else if (currentElement.Name.LocalName.Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        lv = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                            Calculator1.cCalculatorType, SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString());
                    }
                    if (lv == null)
                    {
                        //can't use base Inputs and Outputs alone; 
                        this.SB1C1 = null;
                        this.SB1B1 = null;
                    }
                    else
                    {
                        //recurse
                        SetSB1IOProperties(baseElement, lv, currentElement);
                    }
                }
            }
        }
        public void SetSB1Properties(Calculator1 baseElement,
            XElement currentCalculationsElement, XElement currentElement)
        {
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                //alternatives are set using NPV calculators (or input/output calculators)
                XElement lv = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                    Calculator1.cRelatedCalculatorsType, CalculatorHelpers.CALCULATOR_TYPES.npv.ToString());
                if (lv != null)
                {
                    this.SB1 = new SB1Stock();
                    this.SB1.SetCalculatorProperties(lv);
                    //only need alttype and targettype (alts are groupedby using base elements)
                    baseElement.AlternativeType = this.SB1.AlternativeType;
                    baseElement.TargetType = this.SB1.TargetType;
                    //label and date comes from baseelement
                    this.SB1.Label = baseElement.Label;
                    this.SB1.Date = baseElement.Date;
                }
            }
        }

        private bool SetTPStockTotals(List<TimePeriod> tps,
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasTotals = false;
            int iElementId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            if (tps != null)
            {
                foreach (var tp in tps)
                {
                    if (iElementId == tp.Id)
                    {
                        //ok to keep element
                        bHasTotals = true;
                        //but change the currentcalcs element to new Budget.Calcs
                        string sAttNameExt = string.Empty;
                        AddSB1StockTotals(sAttNameExt, tp.Calculators,
                                currentElement, currentCalculationsElement);
                    }
                }
            }
            return bHasTotals;
        }


        private bool SetBIStockTotals(List<BudgetInvestment> bis,
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasTotals = false;
            int iElementId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            if (bis != null)
            {
                foreach (var bi in bis)
                {
                    if (iElementId == bi.Id)
                    {
                        //ok to keep element
                        bHasTotals = true;
                        //but change the currentcalcs element to new BudgetGroup.Calcs
                        string sAttNameExt = string.Empty;
                        AddSB1StockTotals(sAttNameExt, bi.Calculators,
                            currentElement, currentCalculationsElement);
                    }
                }
            }
            return bHasTotals;
        }


        private bool SetOCStockTotals(List<OperationComponent> ocs,
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasTotals = false;
            int iElementId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            if (ocs != null)
            {
                foreach (var oc in ocs)
                {
                    if (iElementId == oc.Id)
                    {
                        //ok to keep element
                        bHasTotals = true;
                        //but change the currentcalcs element to new OCGroup.Calcs
                        string sAttNameExt = string.Empty;
                        AddSB1StockTotals(sAttNameExt, oc.Calculators,
                                currentElement, currentCalculationsElement);
                    }
                }
            }
            return bHasTotals;
        }

        private bool SetInputStockTotals(List<Input> inputs,
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasTotals = false;
            int iElementId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            if (inputs != null)
            {
                foreach (var input in inputs)
                {
                    if (iElementId == input.Id)
                    {
                        //ok to keep element
                        bHasTotals = true;
                        //but change the currentcalcs element to new OCGroup.Calcs
                        string sAttNameExt = string.Empty;
                        AddSB1StockTotals(sAttNameExt, input.Calculators,
                                currentElement, currentCalculationsElement);
                    }
                }
            }
            return bHasTotals;
        }


        private bool SetOutcomeStockTotals(List<Outcome> ocs,
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasTotals = false;
            int iElementId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            if (ocs != null)
            {
                foreach (var oc in ocs)
                {
                    if (iElementId == oc.Id)
                    {
                        //ok to keep element
                        bHasTotals = true;
                        //but change the currentcalcs element to new OutcomeGroup.Calcs
                        string sAttNameExt = string.Empty;
                        AddSB1StockTotals(sAttNameExt, oc.Calculators,
                                currentElement, currentCalculationsElement);
                    }
                }
            }
            return bHasTotals;
        }
        private bool SetOutputStockTotals(List<Output> outputs,
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasTotals = false;
            int iElementId = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            if (outputs != null)
            {
                foreach (var output in outputs)
                {
                    if (iElementId == output.Id)
                    {
                        //ok to keep element
                        bHasTotals = true;
                        //but change the currentcalcs element to new OutcomeGroup.Calcs
                        string sAttNameExt = string.Empty;
                        AddSB1StockTotals(sAttNameExt, output.Calculators,
                                currentElement, currentCalculationsElement);
                    }
                }
            }
            return bHasTotals;
        }

        //deprecated in favor of the XmlWriter techniques
        private void AddSB1StockTotals(string attNameExt, List<Calculator1> stocks,
            XElement currentElement, XElement currentCalculationsElement)
        {
            if (stocks != null)
            {
                //each stock can contain as many ptstocks as totaled
                //so only one SB1Stock should be used (or inserting/updating els becomes difficult)
                SB1Stock stock = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                if (stocks != null)
                {
                    foreach (var calc in stocks)
                    {
                        if (calc.GetType().Equals(stock.GetType()))
                        {
                            //convert basecalc to stock
                            stock = (SB1Stock)calc;
                            //the stock totals are added to currentelement as a child linked view
                            AddSB1StockTotal(attNameExt, stock,
                                currentElement, currentCalculationsElement);
                        }
                    }
                }
            }
        }

        private void AddSB1StockTotal(string attNameExt, SB1Stock calc,
            XElement currentElement, XElement currentCalculationsElement)
        {
            bool bHasUpdatedCurrentCalcsElement = false;
            //don't keep replacing current calcs element
            bHasUpdatedCurrentCalcsElement = UpdateCurrentCalcsElement(attNameExt, calc,
                currentElement, currentCalculationsElement);
            if (!bHasUpdatedCurrentCalcsElement)
            {
                //completely replace current calcs element 
                //(this analyzer will only display me1stocks, so nothing else is needed)
                XElement lv
                    = new XElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                //try to get the analyzerprops from stateful stock object
                if (this.SB1DescendentStock != null)
                {
                    //copy the name, description ... but no totals
                    calc.CopyCalculatorProperties(this.SB1DescendentStock);
                    //but give it a unique id so a new lv will be added to document
                    //don't replace existing currentcalcsel in case it needs to be used for future analyses
                    int iRandomId = CalculatorHelpers.GetRandomInteger(this.GCCalculatorParams);
                    calc.Id = iRandomId;
                    calc.CalculatorId = iRandomId;
                }
                //this also sets base analyzer properties
                calc.SetDescendantSB1StockAttributes(attNameExt, lv);
                //replace currentcalcsel
                currentCalculationsElement = new XElement(lv);
            }
        }

        private bool UpdateCurrentCalcsElement(string attNameExt, SB1Stock calc,
            XElement currentElement, XElement currentCalculationsElement)
        {
            bool bHasUpdated = false;
            if (currentCalculationsElement != null)
            {
                //if the currentcalcsel has AnalyzerType='progress01' and RelatedCalcType='lifecyle'
                //it is the sb1stock analyzer being run
                string sAnalyzerType = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    Calculator1.cAnalyzerType);
                string sRelatedCalculatorType = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    Calculator1.cRelatedCalculatorType);
                string sRelatedCalculatorsType = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    Calculator1.cRelatedCalculatorsType);
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType == sAnalyzerType
                   && (this.GCCalculatorParams.RelatedCalculatorType == sRelatedCalculatorType
                    || this.GCCalculatorParams.RelatedCalculatorsType == sRelatedCalculatorsType))
                {
                    //don't limit to just the starting analyzer, update children analyzers too
                    //this is the correct stock calculator (won't change the totals)
                    calc.SetCalculatorProperties(currentCalculationsElement);
                    //init label, groupid and typeid for aggregation
                    calc.SetSharedObjectProperties(currentElement);
                    bHasUpdated = true;
                    //keep the id because its the database id
                    int iId = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
                        Calculator1.cId);
                    calc.Id = iId;
                    calc.CalculatorId = iId;
                    //will save the base analyzer and totals
                    calc.SetDescendantSB1StockAttributes(attNameExt, currentCalculationsElement);
                }
                else
                {
                    //the lcastock.total1 updates the existing input or output lv
                    //so that more base calculator props can be displayed (i.e. pcyears)
                    if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                        == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString())
                    {
                        //keep the id because its the database id
                        int iId = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
                            Calculator1.cId);
                        calc.Id = iId;
                        calc.CalculatorId = iId;
                        if (currentElement.Name.LocalName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                        {
                            calc.SetDescendantSB1StockInputAttributes(attNameExt, this.GCCalculatorParams,
                                currentCalculationsElement, currentElement);
                            bHasUpdated = true;
                        }
                        else if (currentElement.Name.LocalName.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                        {
                            calc.SetDescendantSB1StockOutputAttributes(attNameExt, this.GCCalculatorParams,
                                currentCalculationsElement, currentElement);
                            bHasUpdated = true;
                        }
                    }
                }
            }
            if (!bHasUpdated)
            {
                //could look in currentelement, but it shouldn't be necessary
            }
            return bHasUpdated;
        }
        private void AddStockCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                SB1Stock teststock = new SB1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            SB1Stock me = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalSB1StockProperties(stock);
                            newcalcs.Add(me);
                        }
                    }
                }
            }
        }
        private void AddStockInputCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                SB1Stock teststock = new SB1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            SB1Stock me = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalSB1StockProperties(stock);
                            //some analyzers display the results of the initial cost and benefit calcs
                            if (stock.SB11Stock != null)
                            {
                                if (stock.SB11Stock.SB1Calcs != null)
                                {
                                    if (stock.SB11Stock.SB1Calcs.Count > 0)
                                    {
                                        //these are sbc cost calculators not generic subp1s
                                        me.SB11Stock = new SB101Stock();
                                        me.SB11Stock.SB1Calcs = new List<SBC1Calculator>();
                                        stock.SB11Stock.AddSB1CalcsToStock(me.SB11Stock.SB1Calcs);
                                        //SB1AnalyzerHelper.AddInputCalculators(stock.SB11Stock.SB1Calcs, me.SB11Stock.SB1Calcs);
                                    }
                                }
                            }
                            newcalcs.Add(me);
                        }
                    }
                }
            }
        }
        private void AddStockOutputCalculators(List<Calculator1> calcs, List<Calculator1> newcalcs)
        {
            if (calcs != null)
            {
                SB1Stock teststock = new SB1Stock();
                foreach (Calculator1 calc in calcs)
                {
                    if (calc.GetType().Equals(teststock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            SB1Stock me = new SB1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                            me.CopyTotalSB1StockProperties(stock);
                            //some analyzers display the results of the initial cost and benefit calcs
                            if (stock.SB12Stock != null)
                            {
                                if (stock.SB12Stock.SB2Calcs != null)
                                {
                                    if (stock.SB12Stock.SB2Calcs.Count > 0)
                                    {
                                        //these are sbc cost calculators not generic subp1s
                                        me.SB12Stock = new SB102Stock();
                                        me.SB12Stock.SB2Calcs = new List<SBB1Calculator>();
                                        stock.SB12Stock.AddSB2CalcsToStock(me.SB12Stock.SB2Calcs);
                                        //SB1AnalyzerHelper.AddOutputCalculators(stock.SB12Stock.SB1Calcs, me.SB12Stock.SB1Calcs);
                                    }

                                }
                            }
                            newcalcs.Add(me);
                        }
                    }
                }
            }
        }
    }
}
