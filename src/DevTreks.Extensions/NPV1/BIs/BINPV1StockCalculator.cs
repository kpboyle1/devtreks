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
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// NOTES        1. Each base element gets converted to a calculator. 
    ///             The calc is a NPV1Stock which inherits from CostBenefitCalculator.
    ///             The collection of calcs is used to run all analyses.
    /// </summary>
    public class BINPV1StockCalculator : BudgetInvestmentCalculator
    {
        public BINPV1StockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set Indic1Stock
            Init();
        }
        public BINPV1StockCalculator()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //set NPV1DescendentStock state so that descendant stock totals will have good base properties
            //the base.Analyzer is set when the Save... methods are run
            this.NPV1DescendentStock = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            if (this.GCCalculatorParams.LinkedViewElement != null)
            {
                this.NPV1DescendentStock.SetDescendantNPV1StockProperties(
                    this.GCCalculatorParams.LinkedViewElement);
            }
        }
        //stateful analyzer used to set base.Analyzer properties in descendants (name, id)
        //and to hold descendent input and output stocks for analysis
        public NPV1Stock NPV1DescendentStock { get; set; }

        //these objects hold collections of NPV1s for running totals.
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

        public bool AddNPV1CalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //multiple analyzers with rcalctype = npv can confuse subscriber
            ChangeCalculator(currentElement, ref currentCalculationsElement);
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetBIGroupNPV1Calculations(
                   ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBINPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                bHasCalculations = SetOutcomeNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                bHasCalculations = SetTechOutputNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = SetOpOrCompNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetTechInputNPV1Calculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }

        private void SetIds(Calculator1 baseElement, NPV1Stock stock)
        {
            SetCalculatorId(baseElement, stock);
            //alt2 is stored with the base element for some elements (ocs and outcomes, tps)
            if (baseElement.Alternative2 != 0)
            {
                //the CalculatorId is used to set the Id (Id is the base element_
                stock.Alternative2 = baseElement.Alternative2;
            }
        }
        private void SetCalculatorId(Calculator1 baseElement, NPV1Stock stock)
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
        public bool SetBIGroupNPV1Calculations(ref XElement currentCalculationsElement,
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
            //npv calcs already multiplied
            this.BudgetGroup.Multiplier = 1;
            //set this.NPV1 props
            NPV1Stock npv1 = SetNPV1Properties(this.BudgetGroup, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.BudgetGroup.Calculators == null)
            {
                this.BudgetGroup.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.BudgetGroup.Calculators.Add(npv1);
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetBINPV1Calculations(ref XElement currentCalculationsElement,
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
            //npv calcs already multiplied
            this.Budget.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1Properties(this.Budget, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Budget.Calculators == null)
            {
                this.Budget.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.Budget.Calculators.Add(npv1);
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
                NPV1AnalyzerHelper.CopyStockCalculator(budI.Calculators, bi.Calculators);
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

        public bool SetTimePeriodNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.TimePeriod.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1Properties(this.TimePeriod, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.TimePeriod.Calculators == null)
            {
                this.TimePeriod.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.TimePeriod.Calculators.Add(npv1);
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
                NPV1AnalyzerHelper.CopyStockCalculator(timeP.Calculators, tp.Calculators);
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

        public bool SetOCGroupNPV1Calculations(ref XElement currentCalculationsElement,
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
            //set npv1 props
            NPV1Stock npv1 = SetNPV1Properties(this.OCGroup, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OCGroup.Calculators == null)
            {
                this.OCGroup.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.OCGroup.Calculators.Add(npv1);
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

        public bool SetOpOrCompNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.OpComp.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1Properties(this.OpComp, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OpComp.Calculators == null)
            {
                this.OpComp.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.OpComp.Calculators.Add(npv1);
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
                NPV1AnalyzerHelper.CopyStockCalculator(opComp.Calculators, oc.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(input.Calculators, i.Calculators);
                        ////add any base calculators
                        //NPV1AnalyzerHelper.AddInputCalculators(input.Calculators, i.Calculators);
                        ////add any stock analyzers
                        //AddStockInputCalculators(input.Calculators, i.Calculators);
                    }
                    oc.Inputs.Add(i);
                }
            }
            ocs.Add(oc);
        }

        public bool SetOutcomeGroupNPV1Calculations(ref XElement currentCalculationsElement,
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
            //set npv1 props
            NPV1Stock npv1 = SetNPV1Properties(this.OutcomeGroup, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OutcomeGroup.Calculators == null)
            {
                this.OutcomeGroup.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.OutcomeGroup.Calculators.Add(npv1);
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

        public bool SetOutcomeNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.Outcome.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1Properties(this.Outcome, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Outcome.Calculators == null)
            {
                this.Outcome.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.Outcome.Calculators.Add(npv1);
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
                NPV1AnalyzerHelper.CopyStockCalculator(oc.Calculators, outcome.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, o.Calculators);
                        ////add base benefit calculators
                        //NPV1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
                        ////add any stock analyzers
                        //AddStockOutputCalculators(output.Calculators, o.Calculators);
                    }
                    outcome.Outputs.Add(o);
                }
            }
            outcomes.Add(outcome);
        }

        public bool SetInputGroupNPV1Calculations(ref XElement currentCalculationsElement,
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
            NPV1Stock npv1 = SetNPV1Properties(this.InputGroup, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.InputGroup.Calculators == null)
            {
                this.InputGroup.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.InputGroup.Calculators.Add(npv1);
            }
            if (this.InputGroup.Inputs == null)
            {
                this.InputGroup.Inputs = new List<Input>();
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }

        public bool SetInputNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.Input.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1InputProperties(this.Input, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Input.Calculators == null)
            {
                this.Input.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.Input.Calculators.Add(npv1);
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
                NPV1AnalyzerHelper.CopyStockCalculator(input.Calculators, i.Calculators);
                //NPV1AnalyzerHelper.AddInputCalculators(input.Calculators, i.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(inputseries.Calculators, inputseries2.Calculators);
                        //NPV1AnalyzerHelper.AddInputCalculators(inputseries.Calculators, inputseries2.Calculators);
                    }
                    i.Inputs.Add(inputseries2);
                }
            }
            inGroup.Inputs.Add(i);
        }
        public bool SetInputSeriesNPV1Calculations(ref XElement currentCalculationsElement,
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
            inputserie.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1InputProperties(inputserie, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (inputserie.Calculators == null)
            {
                inputserie.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                inputserie.Calculators.Add(npv1);
            }
            if (this.Input.Inputs == null)
            {
                this.Input.Inputs = new List<Input>();
            }
            this.Input.Inputs.Add(inputserie);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetTechInputNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.Input.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1InputProperties(this.Input, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Input.Calculators == null)
            {
                this.Input.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.Input.Calculators.Add(npv1);
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

        public bool SetOutputGroupNPV1Calculations(ref XElement currentCalculationsElement,
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
            NPV1Stock npv1 = SetNPV1Properties(this.OutputGroup, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.OutputGroup.Calculators == null)
            {
                this.OutputGroup.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.OutputGroup.Calculators.Add(npv1);
            }
            if (this.OutputGroup.Outputs == null)
            {
                this.OutputGroup.Outputs = new List<Output>();
            }
            bHasCalculations = true;
            //don't reset for next collection
            return bHasCalculations;
        }
        public bool SetOutputNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.Output.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1OutputProperties(this.Output, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Output.Calculators == null)
            {
                this.Output.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.Output.Calculators.Add(npv1);
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
                NPV1AnalyzerHelper.CopyStockCalculator(output.Calculators, o.Calculators);
                //NPV1AnalyzerHelper.AddOutputCalculators(output.Calculators, o.Calculators);
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
                        NPV1AnalyzerHelper.CopyStockCalculator(outputseries.Calculators, outputseries2.Calculators);
                        //NPV1AnalyzerHelper.AddOutputCalculators(outputseries.Calculators, outputseries2.Calculators);
                    }
                    o.Outputs.Add(outputseries2);
                }
            }
            outGroup.Outputs.Add(o);
        }
        public bool SetOutputSeriesNPV1Calculations(ref XElement currentCalculationsElement,
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
            outputserie.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1OutputProperties(outputserie, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (outputserie.Calculators == null)
            {
                outputserie.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                outputserie.Calculators.Add(npv1);
            }
            if (this.Output.Outputs == null)
            {
                this.Output.Outputs = new List<Output>();
            }
            this.Output.Outputs.Add(outputserie);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetTechOutputNPV1Calculations(ref XElement currentCalculationsElement,
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
            this.Output.Multiplier = 1;
            NPV1Stock npv1 = SetNPV1OutputProperties(this.Output, ref currentCalculationsElement, currentElement);
            //add the calculations to the base element
            if (this.Output.Calculators == null)
            {
                this.Output.Calculators = new List<Calculator1>();
            }
            if (npv1 != null)
            {
                this.Output.Calculators.Add(npv1);
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
        public NPV1Stock SetNPV1Properties(CostBenefitCalculator baseElement,
            ref XElement currentCalculationsElement, XElement currentElement)
        {
            //base stock gets totals and id props
            NPV1Stock npv1 = new NPV1Stock(this.GCCalculatorParams, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            npv1.CalcParameters.CurrentElementNodeName = currentElement.Name.LocalName;
            if (currentCalculationsElement != null)
            {
                //if it finds a calculator, need the ids for updating
                npv1.SetCalculatorProperties(currentCalculationsElement);
                //only need alttype and targettype set from the calculator
                baseElement.AlternativeType = npv1.AlternativeType;
                baseElement.TargetType = npv1.TargetType;
            }
            else
            {
                //else don't need the ids from calculator
                npv1.CopyCalculatorProperties(baseElement);
            }
            npv1.AnalyzerType = this.GCCalculatorParams.AnalyzerParms.AnalyzerType;
            //label and date comes from baseelement
            npv1.Label = baseElement.Label;
            npv1.Date = baseElement.Date;
            npv1.Multiplier = baseElement.Multiplier;
            //copy the totals costs and benefits to the aggregating analyzer
            //this prevents overwriting base and analyzer props during stock construction
            if (NPV1AnalyzerHelper.IsChangeTypeAnalysis(this.GCCalculatorParams.AnalyzerParms.AnalyzerType))
            {
                //change analyzers combine amount and composition amount for display
                baseElement.TotalRAmount = baseElement.TotalRAmount * baseElement.TotalRCompositionAmount;
            }
            npv1.InitTotalNPV1StocksProperties(baseElement);
            //adjust id if children analyzers are being inserted/updated
            ChangeCalculatorIdForUpdatedChildren(currentElement, ref currentCalculationsElement, npv1);
            return npv1;
        }
        public NPV1Stock SetNPV1OutputProperties(Output output,
            ref XElement currentCalculationsElement, XElement currentElement)
        {
            //set specific output props that will be copied into .Total1, .Stat1 ...
            if (output.Times == 0)
                output.Times = 1;
            if (output.CompositionAmount == 0)
                output.CompositionAmount = 1;
            output.TotalRAmount = output.Amount;
            output.TotalRPrice = output.Price;
            output.TotalRName = output.Name;
            output.TotalRUnit = output.Unit;
            output.TotalRCompositionAmount = output.CompositionAmount * output.Times;
            output.TotalRCompositionUnit = output.CompositionUnit;
            //only if npv, mach and other discount calcs run (npv should be run)
            if (output.TotalR != 0)
            {
                output.TotalR = output.TotalR + output.TotalR_INT;
            }
            else
            {
                //base outputs
                output.TotalR = (output.Amount * output.Price * output.CompositionAmount) + output.TotalR_INT;
            }
            //base will be zero
            output.TotalR_INT = output.TotalR_INT;
            output.TotalRINCENT = output.TotalRINCENT;
            if (output.TotalAMR != 0)
            {
                output.TotalAMR = output.TotalAMR;
            }
            else
            {
                //base outputs (no amortized outputs)
                output.TotalAMR = output.TotalR;
            }
            if (output.TotalAMRINCENT != 0)
            {
                output.TotalAMRINCENT = output.TotalAMRINCENT;
            }
            else
            {
                //base outputs
                output.TotalAMRINCENT = output.TotalRINCENT;
            }

            //uniform stock pattern (all stocks subsequently copied identically)
            //accounts for potential future calcors that set alt and target types
            NPV1Stock npv1 = SetNPV1Properties(output, ref currentCalculationsElement, currentElement);
            return npv1;
        }
        public NPV1Stock SetNPV1InputProperties(Input input,
            ref XElement currentCalculationsElement, XElement currentElement)
        {
            //set specific input props that need totals in order to be copied into .Total1, .Stat1 ...
            if (input.Times == 0)
                input.Times = 1;
            input.TotalOCPrice = input.OCPrice;
            input.TotalOCAmount = input.OCAmount;
            //only if mach and other discount calcs run
            if (input.TotalOC != 0)
            {
                input.TotalOC = input.TotalOC + input.TotalOC_INT;
            }
            else
            {
                input.TotalOC = (input.OCPrice * input.OCAmount * input.Times) + input.TotalOC_INT;
            }
            input.TotalAOHPrice = input.AOHPrice;
            //only if mach and other discount calcs run
            if (input.TotalAOH != 0)
            {
                input.TotalAOHAmount = input.AOHAmount;
                input.TotalAOH = input.TotalAOH + input.TotalAOH_INT;
            }
            else
            {
                //assume base inputs have one uniform amount
                input.TotalAOHAmount = input.OCAmount;
                input.TotalAOH = (input.AOHPrice * input.TotalAOHAmount * input.Times) + input.TotalAOH_INT;
            }
            input.TotalCAPPrice = input.CAPPrice;
            //only if mach and other discount calcs run
            if (input.TotalCAP != 0)
            {
                input.TotalCAPAmount = input.CAPAmount;
                input.TotalCAP = input.TotalCAP + input.TotalCAP_INT;
            }
            else
            {
                input.TotalCAPAmount = input.OCAmount;
                input.TotalCAP = (input.CAPPrice * input.TotalCAPAmount * input.Times) + input.TotalCAP_INT;
            }
            //these can be zero
            input.TotalINCENT = input.TotalINCENT;
            input.TotalOC_INT = input.TotalOC_INT;
            input.TotalAOH_INT = input.TotalAOH_INT;
            input.TotalCAP_INT = input.TotalCAP_INT;
            //strictly for outputprice analysis
            if (input.TotalAMOC != 0)
            {
                input.TotalAMOC = input.TotalAMOC;
            }
            else
            {
                //base outputs (no amortized inputs)
                input.TotalAMOC = input.TotalOC;
            }
            if (input.TotalAMAOH != 0)
            {
                input.TotalAMAOH = input.TotalAMAOH;
            }
            else
            {
                //base outputs
                input.TotalAMAOH = input.TotalAOH;
            }
            if (input.TotalAMCAP != 0)
            {
                input.TotalAMCAP = input.TotalAMCAP;
            }
            else
            {
                //base outputs
                input.TotalAMCAP = input.TotalCAP;
            }
            input.TotalAMTOTAL = input.TotalAMOC + input.TotalAMAOH + input.TotalAMCAP;
            if (input.TotalAMINCENT != 0)
            {
                input.TotalAMINCENT = input.TotalAMINCENT;
            }
            else
            {
                //base outputs
                input.TotalAMINCENT = input.TotalINCENT;
            }
            //display
            input.TotalOCName = input.Name;
            input.TotalOCUnit = input.OCUnit;
            //uniform stock pattern (all stocks subsequently copied identically)
            //accounts for potential future calcors that set alt and target types
            NPV1Stock npv1 = SetNPV1Properties(input, ref currentCalculationsElement, currentElement);
            return npv1;
        }
        private void ChangeCalculatorIdForUpdatedChildren(XElement currentElement,
            ref XElement calculator, NPV1Stock npv1)
        {
            //alternatives are set using NPV calculators (or input/output calculators)
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
                    //change analyzer so that it can be inserted or updated (instead of the existing npv calc)
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
                    npv1.Id = iCalcId;
                    npv1.CalculatorId = iCalcId;
                    if (npv1.Total1 != null)
                    {
                        npv1.Total1.Id = iCalcId;
                        npv1.Total1.CalculatorId = iCalcId;
                    }
                    if (npv1.Stat1 != null)
                    {
                        npv1.Stat1.Id = iCalcId;
                        npv1.Stat1.CalculatorId = iCalcId;
                    }
                    if (npv1.Change1 != null)
                    {
                        npv1.Change1.Id = iCalcId;
                        npv1.Change1.CalculatorId = iCalcId;
                    }
                    if (npv1.Progress1 != null)
                    {
                        npv1.Progress1.Id = iCalcId;
                        npv1.Progress1.CalculatorId = iCalcId;
                    }
                }
            }
        }
        public void ChangeCalculator(XElement currentElement,
            ref XElement calculator)
        {
            //these analyzers use atts from calculators only
            if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                //need the alt or target type from npvcalcs
                if (bIsChildren || bIsTPGrandChild)
                {
                    bool bIsNPVCalculator = false;
                    string sCurrentCalcType = string.Empty;
                    if (calculator != null)
                    {
                        sCurrentCalcType = CalculatorHelpers.GetAttribute(calculator, Calculator1.cCalculatorType);
                    }
                    if (!string.IsNullOrEmpty(sCurrentCalcType))
                    {
                        //see if it matches a real npv calctype
                        CalculatorHelpers.CALCULATOR_TYPES calcType 
                            = CalculatorHelpers.GetCalculatorType(sCurrentCalcType);
                        if (calcType != CalculatorHelpers.CALCULATOR_TYPES.none)
                        {
                            bIsNPVCalculator = true;
                        }
                    }
                    if (calculator == null
                        || bIsNPVCalculator == false)
                    {
                        calculator = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                            Calculator1.cCalculatorType, currentElement.Name.LocalName);
                    }
                }
            }
        }
    }
}