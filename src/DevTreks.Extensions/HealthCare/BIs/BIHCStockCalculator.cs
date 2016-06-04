using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run health care cost and benefit stock calculations for operating 
    ///             and capital budgets
    ///Author:		www.devtreks.org
    ///Date:		2013, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. 
    /// </summary>
    public class BIHCStockCalculator : BudgetInvestmentCalculator
    {
        public BIHCStockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set machstock
            Init();
        }
        public BIHCStockCalculator()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            //collections of calculated inputs and outputs
            this.InputHCStock = new InputHCStock();
            this.InputHCStock.HealthCareCost1s
                = new Dictionary<int, List<HealthCareCost1Calculator>>();
            this.OutputHCStock = new OutputHCStock();
            this.OutputHCStock.HealthCareBenefit1s
                = new Dictionary<int, List<HealthBenefit1Calculator>>();
            //init indexing
            this.TimePeriodStartingFileIndex = 0;
            this.BudgetStartingFileIndex = 0;
        }

        //constants used by this calculator
        public const string ZERO = "0";
        //stateful health input stock
        public InputHCStock InputHCStock { get; set; }
        //stateful health output stock
        public OutputHCStock OutputHCStock { get; set; }

        //these objects hold collections of descendants for optimizing total benefits and costs
        public HealthCareCost1Calculator HealthCareCost1Input { get; set; }
        public HealthBenefit1Calculator HealthBenefit1Output { get; set; }

        //time period starting index for calculating opOrComp mach totals
        //set every time a time period accumulates descendant opOrComp mach Totals
        public int TimePeriodStartingFileIndex { get; set; }
        //budget starting index for calculating opOrComp mach totals
        //set every time a budget accumulates descendant opOrComp mach Totals
        public int BudgetStartingFileIndex { get; set; }
        //these objects hold collections of descendants for optimizing total costs
        public BudgetInvestmentGroup BudgetGroup { get; set; }
        public BudgetInvestment Budget { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public OperationComponent OpComp { get; set; }
        public Outcome Outcome { get; set; }

        public bool AddStock1CalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //input healthcare1 facts are aggregated by each ancestor
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetTotalHCStockCalculations(
                   ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                bHasCalculations = SetOutcomeHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                bHasCalculations = SetOutputHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = SetOpOrCompHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetInputHCStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }

        public bool SetTotalHCStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroug)
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.BudgetGroup == null)
            {
                this.BudgetGroup = new BudgetInvestmentGroup();
            }
            //when this method is called from opcomp or input group
            if (this.Budget == null)
            {
                this.Budget = new BudgetInvestment();
            }
            if (currentElement.Name.LocalName.EndsWith("group"))
            {
                this.BudgetGroup.SetBudgetInvestmentGroupProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            }
            else
            {
                this.Budget.SetBudgetInvestmentProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            }
            //init hcStock props
            this.InputHCStock.SetCalculatorProperties(currentCalculationsElement);
            //the hcStock.stocks dictionary can now be summed to derive totals
            //bimachstockcalculator handles calculations
            double dbMultiplier = 1;
            //set the health care stock totals from hcStock collection
            bHasCalculations = SetTotalHCInputStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new health care stock totals
            this.InputHCStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.InputHCStock.SetTotalInputHCStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddInputHCStockTotals(this.InputHCStock, ref currentCalculationsElement);
            if (currentElement.Name.LocalName
                == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                //set the output stock totals
                bHasCalculations = SetOutputHCStockTotalsCalculations(ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                //operation group won't return true but budget will
                SetOutputHCStockTotalsCalculations(ref currentCalculationsElement, ref currentElement);
            }
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //add to collection
            if (this.BudgetGroup.BudgetInvestments == null)
            {
                this.BudgetGroup.BudgetInvestments = new List<BudgetInvestment>();
            }
            this.BudgetGroup.BudgetInvestments.Add(this.Budget);
            //reset for next collection
            this.Budget = null;
            return bHasCalculations;
        }
        public bool SetBIHCStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroug)
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.BudgetGroup == null)
            {
                this.BudgetGroup = new BudgetInvestmentGroup();
            }
            //when this method is called from opcomp or input group
            if (this.Budget == null)
            {
                this.Budget = new BudgetInvestment();
            }
            if (currentCalculationsElement == null)
            {
                //budget resource stock analyzers always show tp totals which are stored in
                //currentCalcElement; if null (uses set NeedsChildren to false) set it to 
                //current linkedview
                currentCalculationsElement = this.GCCalculatorParams.LinkedViewElement;
            }
            this.Budget.SetBudgetInvestmentProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            //init hcStock props
            this.InputHCStock.SetCalculatorProperties(currentCalculationsElement);
            //the hcStock.machstocks dictionary can now be summed to derive totals
            double dbMultiplier = 1;
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new health care stock totals
            this.InputHCStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the output stock totals (before SetTotalHCInput changes fileposition)
            SetOutputHCStockTotalsCalculations(ref currentCalculationsElement, ref currentElement);
            //set the health care stock totals from hcStock collection
            bHasCalculations = SetTotalHCInputStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            this.InputHCStock.SetTotalInputHCStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddInputHCStockTotals(this.InputHCStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.InputHCStock.InitTotalInputHCStockProperties();
            ////set the output stock totals
            //SetOutputHCStockTotalsCalculations(ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.BudgetGroup.BudgetInvestments == null)
            {
                this.BudgetGroup.BudgetInvestments = new List<BudgetInvestment>();
            }
            this.BudgetGroup.BudgetInvestments.Add(this.Budget);
            //reset for next collection
            this.Budget = null;
            return bHasCalculations;
        }
        public bool SetTimePeriodHCStockCalculations(ref XElement currentCalculationsElement,
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
            if (currentCalculationsElement == null)
            {
                //budget resource stock analyzers always show tp totals which are stored in
                //currentCalcElement; if null (uses set NeedsChildren to false) set it to 
                //current linkedview
                currentCalculationsElement = this.GCCalculatorParams.LinkedViewElement;
            }
            //note that the healthCareCostInput calculator can not change TimePeriod properties
            //but needs properties from the TimePeriod (i.e. Amount)
            this.TimePeriod.SetTimePeriodProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init hcStock props
            this.InputHCStock.SetCalculatorProperties(currentCalculationsElement);
            //don't double count the tp multiplier -each op comp already used it to set penalties
            double dbMultiplier = 1;
            string sAttNameExtension = string.Empty;
            //set new health care stock totals
            this.InputHCStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //August, 2012 needs to be set before file index changes so set before SetTotalHCInput
            //set the output stock totals
            SetOutputHCStockTotalsCalculations(ref currentCalculationsElement, ref currentElement);
            //the hcStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalHCInputStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            this.InputHCStock.SetTotalInputHCStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddInputHCStockTotals(this.InputHCStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next tp)
            this.InputHCStock.InitTotalInputHCStockProperties();
            //August, 2012 needs to be set before file index changes so set in SetTotalHCInput
            //set the output stock totals
            //SetOutputHCStockTotalsCalculations(ref currentCalculationsElement, ref currentElement);
            //reset hcStock.machstocks fileposition index
            //next tp's machstock will be inserted in next index
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.Budget.TimePeriods == null)
            {
                this.Budget.TimePeriods = new List<TimePeriod>();
            }
            this.Budget.TimePeriods.Add(this.TimePeriod);
            //reset for next collection
            this.TimePeriod = null;
            return bHasCalculations;
        }
        public bool SetOpOrCompHCStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent tp for holding collection of opcomps
            if (this.TimePeriod == null)
            {
                this.TimePeriod = new TimePeriod();
            }
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            //note that the healthCareCostInput calculator can not change Operation properties
            //but needs several properties from the Operation (i.e. Id, Amount)
            this.OpComp.SetOperationComponentProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init hcStock props
            this.InputHCStock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the hcStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalHCInputStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new health care stock totals
            this.InputHCStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.InputHCStock.SetTotalInputHCStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddInputHCStockTotals(this.InputHCStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.InputHCStock.InitTotalInputHCStockProperties();
            //reset hcStock.machstocks fileposition index
            //next opOrComp's machstock will be inserted in next index
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.TimePeriod.OperationComponents == null)
            {
                this.TimePeriod.OperationComponents = new List<OperationComponent>();
            }
            this.TimePeriod.OperationComponents.Add(this.OpComp);
            //reset for next collection
            this.OpComp = null;
            return bHasCalculations;
        }
        public bool SetOutcomeHCStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent tp for holding collection of outcomes
            if (this.TimePeriod == null)
            {
                this.TimePeriod = new TimePeriod();
            }
            if (this.Outcome == null)
            {
                this.Outcome = new Outcome();
            }
            //note that the healthCareCostOutput calculator can not change Outcome properties
            //but needs several properties from the Outcome (i.e. Id, Amount)
            this.Outcome.SetOutcomeProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init hcStock props
            this.OutputHCStock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the hcStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalHCOutputStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new health care stock totals
            this.OutputHCStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.OutputHCStock.SetTotalOutputHCStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddOutputHCStockTotals(this.OutputHCStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.OutputHCStock.InitTotalOutputHCStockProperties();
            //reset hcStock.machstocks fileposition index
            //next opOrComp's machstock will be inserted in next index
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.TimePeriod.Outcomes == null)
            {
                this.TimePeriod.Outcomes = new List<Outcome>();
            }
            this.TimePeriod.Outcomes.Add(this.Outcome);
            //reset for next collection
            this.Outcome = null;
            return bHasCalculations;
        }
        public bool SetOutputHCStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent outcome for holding a collection of healthCareOutputs
            if (this.Outcome == null)
            {
                this.Outcome = new Outcome();
            }
            if (currentCalculationsElement != null)
            {
                //note that the healthCareCostInput calculator can not change Output properties
                //when running from opOrComps or budgets
                //but needs several properties from the Output (i.e. Id, CompositionAmount)
                this.HealthBenefit1Output = new HealthBenefit1Calculator();
                //deserialize xml to object
                this.HealthBenefit1Output.SetHealthBenefit1Properties(this.GCCalculatorParams,
                   currentCalculationsElement, currentElement);
                //init analyzer props
                this.HealthBenefit1Output.SetCalculatorProperties(currentCalculationsElement);
                //all stocks analyzers put full benefits in outputs (easier to manipulate collections)
                double dbMultiplier = GetOutputFullCostMultiplier(this.HealthBenefit1Output, this.GCCalculatorParams);
                //change benefits by output.compositionamount * output.amount 
                ChangeHCOutputByOutputMultipliers(this.HealthBenefit1Output, dbMultiplier);
                //serialize calculator object back to xml
                string sAttNameExtension = string.Empty;
                //set new input totals
                this.HealthBenefit1Output.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                this.HealthBenefit1Output.SetNewOutputAttributes(this.GCCalculatorParams, ref currentCalculationsElement);
                this.HealthBenefit1Output.SetHealthBenefit1Attributes(sAttNameExtension,
                    ref currentCalculationsElement);
                //set the totalR 
                AddHealthBenefit1CalculatorTotals(this.HealthBenefit1Output, ref currentCalculationsElement);
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the healthCareCostOutput to the hcStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.OutputHCStock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.HealthBenefit1Output);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.OutputHCStock
                    .AddOutputHCStocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.HealthBenefit1Output);
                //add to collection
                if (this.Outcome.Outputs == null)
                {
                    this.Outcome.Outputs = new List<Output>();
                }
                //note that healthCareBenefitOutput can be retrieved by converting the output to the 
                //HealthCareBenefit1Calculator type (healthCareCostOutput = (HealthCareBenefit1Calculator) output)
                if (this.HealthBenefit1Output != null)
                {
                    this.Outcome.Outputs.Add(this.HealthBenefit1Output);
                }
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        private bool SetOutputHCStockTotalsCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //this has to be called from the inputtpc method
            bool bHasCalculations = false;
            //tp.amount was multiplied in the outputs
            double dbMultiplier = 1;
            //the hcStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalHCOutputStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new output stock totals
            this.OutputHCStock.SetTotalOutputHCStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //reset machstock totals to zero (for next tp)
            this.OutputHCStock.InitTotalOutputHCStockProperties();
            return bHasCalculations;
        }
        public bool SetTotalHCOutputStockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.OutputHCStock.HealthCareBenefit1s != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<HealthBenefit1Calculator>> kvp
                    in this.OutputHCStock.HealthCareBenefit1s)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (HealthBenefit1Calculator healthCareOutput in kvp.Value)
                        {
                            bool bAdjustTotals = true;
                            bHasCalculations
                                = AddHCOutputToStock(this.OutputHCStock, 
                                multiplier, healthCareOutput, currentNodeName, bAdjustTotals);
                            iNodeCount += 1;
                        }
                    }
                }
                //let the input stocks keep count of filepositions and indexes
            }
            return bHasCalculations;
        }

        public static bool AddHCOutputToStock(OutputHCStock outputHCStock, double multiplier, 
            HealthBenefit1Calculator healthOutput, string currentNodeName, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (input.times, input.ocamount, out.compositionamount, oc.amount, tp.amount)
                //are not used with per hourly costs, but are used with total op and aoh costs (fuelcost, capitalrecovery)
                if (currentNodeName.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i.e. healthCareCostInput.cost = output.composamount * output.amount
                    ChangeHCOutputByOutputMultipliers(healthOutput, multiplier);
                }
                else
                {
                    //if timeperiod multiplier = tp.amount)
                    ChangeHCOutputByMultiplier(healthOutput, multiplier);
                }
            }
            //total economic measures (adjusted above by multipliers)
            outputHCStock.TotalAverageBenefitRating += (healthOutput.AverageBenefitRating);
            outputHCStock.TotalQALY += (healthOutput.QALY);
            outputHCStock.TotalICERQALY += (healthOutput.ICERQALY);
            outputHCStock.TotalTTOQALY += (healthOutput.TTOQALY);
            outputHCStock.TotalOutputCost += (healthOutput.OutputCost);
            outputHCStock.TotalAdjustedBenefit += (healthOutput.AdjustedBenefit);
            //remainder
            outputHCStock.TotalBenefitAdjustment += (healthOutput.BenefitAdjustment);
            outputHCStock.TotalOutputEffect1Amount += (healthOutput.OutputEffect1Amount);
            outputHCStock.TotalOutputEffect1Price += (healthOutput.OutputEffect1Price);
            outputHCStock.TotalOutputEffect1Cost += (healthOutput.OutputEffect1Cost);
            //qol measures
            outputHCStock.TotalPhysicalHealthRating += (healthOutput.PhysicalHealthRating);
            outputHCStock.TotalEmotionalHealthRating += (healthOutput.EmotionalHealthRating);
            outputHCStock.TotalSocialHealthRating += (healthOutput.SocialHealthRating);
            outputHCStock.TotalEconomicHealthRating += (healthOutput.EconomicHealthRating);
            outputHCStock.TotalHealthCareDeliveryRating += (healthOutput.HealthCareDeliveryRating);
            outputHCStock.TotalBeforeQOLRating += (healthOutput.BeforeQOLRating);
            outputHCStock.TotalAfterQOLRating += (healthOutput.AfterQOLRating);
            outputHCStock.TotalBeforeYears += (healthOutput.BeforeYears);
            outputHCStock.TotalAfterYears += (healthOutput.AfterYears);
            outputHCStock.TotalAfterYearsProb += (healthOutput.AfterYearsProb);
            outputHCStock.TotalTimeTradeoffYears += (healthOutput.TimeTradeoffYears);
            outputHCStock.TotalEquityMultiplier += (healthOutput.EquityMultiplier);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetInputHCStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of healthCareCostInputs
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            if (currentCalculationsElement != null)
            {
                //note that the healthCareCostInput calculator can not change Input properties
                //when running from opOrComps or budgets
                //but needs several properties from the Input (i.e. Id, Times)
                this.HealthCareCost1Input = new HealthCareCost1Calculator();
                //deserialize xml to object
                this.HealthCareCost1Input.SetHealthCareCost1Properties(this.GCCalculatorParams,
                   currentCalculationsElement, currentElement);
                //init analyzer props
                this.HealthCareCost1Input.SetCalculatorProperties(currentCalculationsElement);
                //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                double dbMultiplier = GetInputFullCostMultiplier(this.HealthCareCost1Input, this.GCCalculatorParams);
                //change fuel cost, repair cost, by input.times * input.ocamount or input.aohamount
                ChangeHCInputByInputMultipliers(this.HealthCareCost1Input, dbMultiplier);
                //serialize calculator object back to xml
                //(calculator doesn't change opOrComp, so don't serialize it)
                string sAttNameExtension = string.Empty;
                //set new  input totals
                this.HealthCareCost1Input.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                this.HealthCareCost1Input.SetNewInputAttributes(this.GCCalculatorParams, ref currentCalculationsElement);
                this.HealthCareCost1Input.SetHealthCareCost1Attributes(sAttNameExtension,
                    ref currentCalculationsElement);
                //set the totaloc and totalaoh
                AddHealthCareCost1CalculatorTotals(this.HealthCareCost1Input, ref currentCalculationsElement);
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the healthCareCostInput to the hcStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.InputHCStock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.HealthCareCost1Input);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.InputHCStock
                    .AddInputHCStocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.HealthCareCost1Input);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                //note that healthCareCostInput can be retrieved by converting the input to the 
                //HealthCareCost1Calculator type (healthCareCostInput = (HealthCareCost1Calculator) input)
                if (this.HealthCareCost1Input != null)
                {
                    this.OpComp.Inputs.Add(this.HealthCareCost1Input);
                }
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        
        public bool SetTotalHCInputStockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.InputHCStock.HealthCareCost1s != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<HealthCareCost1Calculator>> kvp
                    in this.InputHCStock.HealthCareCost1s)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (HealthCareCost1Calculator healthCareCostInput in kvp.Value)
                        {
                            bool bAdjustTotals = true;
                            bHasCalculations
                                = AddHCInputToStock(this.InputHCStock, multiplier, 
                                healthCareCostInput, currentNodeName, bAdjustTotals);
                            iNodeCount += 1;
                        }
                    }
                }
                if (currentNodeName
                    == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                    || currentNodeName
                    == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                {
                    //set the stateful budget index 
                    this.BudgetStartingFileIndex = iFilePosition;
                }
                else if (currentNodeName
                    == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || currentNodeName
                    == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //set the stateful time period index 
                    this.TimePeriodStartingFileIndex = iFilePosition;
                }
            }
            return bHasCalculations;
        }

        public static bool AddHCInputToStock(InputHCStock hcStock, double multiplier,
            HealthCareCost1Calculator healthCareCostInput, string currentNodeName, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
                //don't change per hour health care costs, only total costs
                if (currentNodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    //i.e. healthCareCostInput.cost = healthCareCostInput.cost * multiplier * input.ocamount
                    //multiplier = input.times * oc.amount * tp.amount
                    ChangeHCInputByInputMultipliers(healthCareCostInput, multiplier);
                }
                else
                {
                    //i.e. healthCareCostInput.cost = healthCareCostInput.cost * multiplier (1 in stock analyzers)
                    ChangeHCInputByMultiplier(healthCareCostInput, multiplier);
                }
            }
            //multiplier adjusted costs
            hcStock.TotalInsuranceProviderCost += (healthCareCostInput.InsuranceProviderCost);
            hcStock.TotalIncentivesCost += (healthCareCostInput.IncentivesCost);
            hcStock.TotalReceiverCost += (healthCareCostInput.ReceiverCost);
            hcStock.TotalHealthCareProviderCost += (healthCareCostInput.HealthCareProviderCost);
            //no multiplier used with remaining attributes
            hcStock.TotalBasePrice += (healthCareCostInput.BasePrice);
            hcStock.TotalBasePriceAdjustment += (healthCareCostInput.BasePriceAdjustment);
            hcStock.TotalAdjustedPrice += (healthCareCostInput.AdjustedPrice);
            hcStock.TotalContractedPrice += (healthCareCostInput.ContractedPrice);
            hcStock.TotalListPrice += (healthCareCostInput.ListPrice);
            hcStock.TotalMarketPrice += (healthCareCostInput.MarketPrice);
            hcStock.TotalProductionCostPrice += (healthCareCostInput.ProductionCostPrice);
            hcStock.TotalAnnualPremium1 += (healthCareCostInput.AnnualPremium1);
            hcStock.TotalAnnualPremium2 += (healthCareCostInput.AnnualPremium2);
            hcStock.TotalAssignedPremiumCost += (healthCareCostInput.AssignedPremiumCost);
            hcStock.TotalAdditionalPrice1 += (healthCareCostInput.AdditionalPrice1);
            hcStock.TotalAdditionalAmount1 += (healthCareCostInput.AdditionalAmount1);
            hcStock.TotalAdditionalCost1 += (healthCareCostInput.AdditionalCost1);
            hcStock.TotalAdditionalPrice2 += (healthCareCostInput.AdditionalPrice2);
            hcStock.TotalAdditionalAmount2 += (healthCareCostInput.AdditionalAmount2);
            hcStock.TotalAdditionalCost2 += (healthCareCostInput.AdditionalCost2);
            hcStock.TotalAdditionalCost += (healthCareCostInput.AdditionalCost);
            hcStock.TotalKnowledgeTransferRating += (healthCareCostInput.KnowledgeTransferRating);
            hcStock.TotalConstrainedChoiceRating += (healthCareCostInput.ConstrainedChoiceRating);

            //health care received payments
            hcStock.TotalCoPay1Amount += (healthCareCostInput.CoPay1Amount);
            hcStock.TotalCoPay1Rate += (healthCareCostInput.CoPay1Rate);
            hcStock.TotalCoPay2Amount += (healthCareCostInput.CoPay2Amount);
            hcStock.TotalCoPay2Rate += (healthCareCostInput.CoPay2Rate);
            hcStock.TotalIncentive1Amount += (healthCareCostInput.Incentive1Amount);
            hcStock.TotalIncentive1Rate += (healthCareCostInput.Incentive1Rate);
            hcStock.TotalIncentive2Amount += (healthCareCostInput.Incentive2Amount);
            hcStock.TotalIncentive2Rate += (healthCareCostInput.Incentive2Rate);
            hcStock.TotalDiagnosisQualityRating += (healthCareCostInput.DiagnosisQualityRating);
            hcStock.TotalTreatmentQualityRating += (healthCareCostInput.TreatmentQualityRating);
            hcStock.TotalTreatmentBenefitRating += (healthCareCostInput.TreatmentBenefitRating);
            hcStock.TotalTreatmentCostRating += (healthCareCostInput.TreatmentCostRating);
            hcStock.TotalInsuranceCoverageRating += (healthCareCostInput.InsuranceCoverageRating);
            hcStock.TotalCostRating += (healthCareCostInput.CostRating);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddInputHCStockToStock(InputHCStock totalsHC1Stock,
            double multiplier, InputHCStock currentHC1Stock)
        {
            bool bHasCalculations = false;
            totalsHC1Stock.TotalInsuranceProviderCost += (currentHC1Stock.InsuranceProviderCost);
            totalsHC1Stock.TotalIncentivesCost += (currentHC1Stock.IncentivesCost);
            totalsHC1Stock.TotalReceiverCost += (currentHC1Stock.ReceiverCost);
            totalsHC1Stock.TotalHealthCareProviderCost += (currentHC1Stock.HealthCareProviderCost);

            totalsHC1Stock.TotalBasePrice += (currentHC1Stock.BasePrice);
            totalsHC1Stock.TotalBasePriceAdjustment += (currentHC1Stock.BasePriceAdjustment);
            totalsHC1Stock.TotalAdjustedPrice += (currentHC1Stock.AdjustedPrice);
            totalsHC1Stock.TotalContractedPrice += (currentHC1Stock.ContractedPrice);
            totalsHC1Stock.TotalListPrice += (currentHC1Stock.ListPrice);
            totalsHC1Stock.TotalMarketPrice += (currentHC1Stock.MarketPrice);
            totalsHC1Stock.TotalProductionCostPrice += (currentHC1Stock.ProductionCostPrice);
            totalsHC1Stock.TotalAnnualPremium1 += (currentHC1Stock.AnnualPremium1);
            totalsHC1Stock.TotalAnnualPremium2 += (currentHC1Stock.AnnualPremium2);
            totalsHC1Stock.TotalAssignedPremiumCost += (currentHC1Stock.AssignedPremiumCost);
            totalsHC1Stock.TotalAdditionalPrice1 += (currentHC1Stock.AdditionalPrice1);
            totalsHC1Stock.TotalAdditionalAmount1 += (currentHC1Stock.AdditionalAmount1);
            totalsHC1Stock.TotalAdditionalCost1 += (currentHC1Stock.AdditionalCost1);
            totalsHC1Stock.TotalAdditionalPrice2 += (currentHC1Stock.AdditionalPrice2);
            totalsHC1Stock.TotalAdditionalAmount2 += (currentHC1Stock.AdditionalAmount2);
            totalsHC1Stock.TotalAdditionalCost2 += (currentHC1Stock.AdditionalCost2);
            totalsHC1Stock.TotalAdditionalCost += (currentHC1Stock.AdditionalCost);
            totalsHC1Stock.TotalKnowledgeTransferRating += (currentHC1Stock.KnowledgeTransferRating);
            totalsHC1Stock.TotalConstrainedChoiceRating += (currentHC1Stock.ConstrainedChoiceRating);
            //health care received payments
            totalsHC1Stock.TotalCoPay1Amount += (currentHC1Stock.CoPay1Amount);
            totalsHC1Stock.TotalCoPay1Rate += (currentHC1Stock.CoPay1Rate);
            totalsHC1Stock.TotalCoPay2Amount += (currentHC1Stock.CoPay2Amount);
            totalsHC1Stock.TotalCoPay2Rate += (currentHC1Stock.CoPay2Rate);
            totalsHC1Stock.TotalIncentive1Amount += (currentHC1Stock.Incentive1Amount);
            totalsHC1Stock.TotalIncentive1Rate += (currentHC1Stock.Incentive1Rate);
            totalsHC1Stock.TotalIncentive2Amount += (currentHC1Stock.Incentive2Amount);
            totalsHC1Stock.TotalIncentive2Rate += (currentHC1Stock.Incentive2Rate);
            totalsHC1Stock.TotalDiagnosisQualityRating += (currentHC1Stock.DiagnosisQualityRating);
            totalsHC1Stock.TotalTreatmentQualityRating += (currentHC1Stock.TreatmentQualityRating);
            totalsHC1Stock.TotalTreatmentBenefitRating += (currentHC1Stock.TreatmentBenefitRating);
            totalsHC1Stock.TotalTreatmentCostRating += (currentHC1Stock.TreatmentCostRating);
            totalsHC1Stock.TotalInsuranceCoverageRating += (currentHC1Stock.InsuranceCoverageRating);
            totalsHC1Stock.TotalCostRating += (currentHC1Stock.CostRating);
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool NeedsFilePosition(int filePosition, string currentNodeName)
        {
            bool bNeedsFilePositionFacts = true;
            if (currentNodeName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentNodeName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
                || currentNodeName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //each opOrComp is a file position member
                //aggregate only using the current file position
                if (this.GCCalculatorParams.AnalyzerParms.FilePositionIndex != filePosition)
                {
                    bNeedsFilePositionFacts = false;
                }
            }
            else if (currentNodeName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentNodeName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                //set the stateful budget index 
                if (this.BudgetStartingFileIndex == 0)
                    return true;
                if (filePosition > this.BudgetStartingFileIndex)
                {
                    bNeedsFilePositionFacts = true;
                }
                else
                {
                    bNeedsFilePositionFacts = false;
                }
            }
            else if (currentNodeName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentNodeName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //set the stateful time period index 
                if (this.TimePeriodStartingFileIndex == 0)
                    return true;
                if (filePosition > this.TimePeriodStartingFileIndex)
                {
                    bNeedsFilePositionFacts = true;
                }
                else
                {
                    bNeedsFilePositionFacts = false;
                }
            }
            return bNeedsFilePositionFacts;
        }
        public static double GetOutputFullCostMultiplier(HealthBenefit1Calculator healthBenefitOutput,
            CalculatorParameters calcParams)
        {
            //to keep all health care stock analysis consistent, put the totals 
            //in the outputs, then all can be compared consistently
            //output.compositionamount * output.times
            double dbOutputMultiplier = IOHCStockSubscriber
                .GetMultiplierForTechOutput(healthBenefitOutput);
            double dbOutcomeMultiplier = OutcomeHCStockSubscriber
               .GetMultiplierForOutcome(calcParams.ParentOutcome);
            double dbTPAmount = BIHCStockSubscriber.GetMultiplierForTimePeriod(
                calcParams.ParentTimePeriod);
            double dbMultiplier = dbOutputMultiplier * dbOutcomeMultiplier * dbTPAmount;
            return dbMultiplier;
        }
        public static double GetInputFullCostMultiplier(HealthCareCost1Calculator healthCareCostInput,
            CalculatorParameters calcParams)
        {
            //to keep all health care stock analysis consistent, put the totals 
            //in the inputs, then all can be compared consistently
            //input.times
            double dbInputMultiplier = IOHCStockSubscriber
                .GetMultiplierForTechInput(healthCareCostInput);
            //oc.amount
            double dbOCMultiplier = OCHCStockSubscriber
                .GetMultiplierForOperation(calcParams.ParentOperationComponent);
            double dbTPAmount = BIHCStockSubscriber.GetMultiplierForTimePeriod(
                calcParams.ParentTimePeriod);
            double dbMultiplier = dbInputMultiplier * dbOCMultiplier * dbTPAmount;
            return dbMultiplier;
        }
        
        public static void ChangeHCOutputByMultiplier(HealthBenefit1Calculator healthOutput,
            double multiplier)
        {
            healthOutput.AverageBenefitRating = (healthOutput.AverageBenefitRating * multiplier);
            healthOutput.QALY = (healthOutput.QALY * multiplier);
            healthOutput.ICERQALY = (healthOutput.ICERQALY * multiplier);
            healthOutput.TTOQALY = (healthOutput.TTOQALY * multiplier);
            healthOutput.OutputCost = (healthOutput.OutputCost * multiplier);
            healthOutput.AdjustedBenefit = (healthOutput.AdjustedBenefit * multiplier);
        }
        public static void ChangeHCOutputByOutputMultipliers(HealthBenefit1Calculator healthOutput,
            double multiplier)
        {
            healthOutput.AverageBenefitRating = (healthOutput.AverageBenefitRating * healthOutput.Amount * multiplier);
            healthOutput.QALY = (healthOutput.QALY * healthOutput.Amount * multiplier);
            healthOutput.ICERQALY = (healthOutput.ICERQALY * healthOutput.Amount * multiplier);
            healthOutput.TTOQALY = (healthOutput.TTOQALY * healthOutput.Amount * multiplier);
            healthOutput.OutputCost = (healthOutput.OutputCost * healthOutput.Amount * multiplier);
            healthOutput.AdjustedBenefit = (healthOutput.AdjustedBenefit * healthOutput.Amount * multiplier);
        }
        public static void ChangeHCInputByMultiplier(HealthCareCost1Calculator healthCareCostInput,
            double multiplier)
        {
            healthCareCostInput.HealthCareProviderCost = (healthCareCostInput.HealthCareProviderCost * multiplier);
            healthCareCostInput.InsuranceProviderCost = (healthCareCostInput.InsuranceProviderCost * multiplier);
            healthCareCostInput.IncentivesCost = (healthCareCostInput.IncentivesCost * multiplier);
            healthCareCostInput.ReceiverCost = (healthCareCostInput.ReceiverCost * multiplier);
        }
        public static void ChangeHCInputByInputMultipliers(HealthCareCost1Calculator healthCareCostInput,
            double multiplier)
        {
            healthCareCostInput.HealthCareProviderCost = (healthCareCostInput.HealthCareProviderCost * healthCareCostInput.OCAmount * multiplier);
            healthCareCostInput.InsuranceProviderCost = (healthCareCostInput.InsuranceProviderCost * healthCareCostInput.OCAmount * multiplier);
            healthCareCostInput.IncentivesCost = (healthCareCostInput.IncentivesCost * healthCareCostInput.OCAmount * multiplier);
            healthCareCostInput.ReceiverCost = (healthCareCostInput.ReceiverCost * healthCareCostInput.OCAmount * multiplier);
        }
        public static void AddHealthBenefit1CalculatorTotals(HealthBenefit1Calculator healthBenefitOutput,
            ref XElement healthCareOutputCalcElement)
        {
            
        }
        public static void AddHealthCareCost1CalculatorTotals(HealthCareCost1Calculator healthCareCostInput,
            ref XElement healthCareCostInputCalcElement)
        {
           
        }
        public static void AddInputHCStockTotals(InputHCStock hcStock,
            ref XElement hcStockCalcElement)
        {
            
        }
        public static void AddOutputHCStockTotals(OutputHCStock hcStock,
            ref XElement hcStockCalcElement)
        {
            
        }
    }
}
