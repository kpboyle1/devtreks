using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run machinery stock calculations for operating 
    ///             and capital budgets
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. 
    /// </summary>
    public class BIMachineryStockCalculator : BudgetInvestmentCalculator
    {
        public BIMachineryStockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters) 
        {
            //base sets calc and analysis
            //this needs to set machstock
            Init();
        }
        public BIMachineryStockCalculator()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            //collections of calculated machinery
            this.MachineryStock = new Machinery1Stock();
            this.MachineryStock.MachineryStocks
                = new Dictionary<int, List<Machinery1Input>>();
            //collections of calculated timeliness penalties
            this.Machinery2Stock = new Machinery2Stock();
            this.Machinery2Stock.Machinery2Stocks
                = new Dictionary<int, List<TimelinessOpComp1>>();
            //init indexing
            this.TimePeriodStartingFileIndex = 0;
            this.BudgetStartingFileIndex = 0;
        }
        
        //constants used by this calculator
        public const string ZERO = "0";
        //stateful machinery stock
        public Machinery1Stock MachineryStock { get; set; }
        public Machinery2Stock Machinery2Stock { get; set; }

        //these objects hold collections of descendants for optimizing total costs
        public Machinery1Input Mach1Input { get; set; }

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

        public bool AddStock1CalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //input machinery1 facts are aggregated by each ancestor
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetTotalMachineryStockCalculations(
                   ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                //outputs are not used in this analyzer
                //bHasCalculations = true;
            }
            else if (currentElement.Name.LocalName
             .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //outcomes are not used in this analyzer
                //bHasCalculations = true;
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = SetOpOrCompMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetInputMachineryStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }

        public bool SetTotalMachineryStockCalculations(ref XElement currentCalculationsElement, 
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
            //init machStock props
            this.MachineryStock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            //bimachstockcalculator handles calculations
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.MachineryStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.MachineryStock, ref currentCalculationsElement);
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
        public bool SetBIMachineryStockCalculations(ref XElement currentCalculationsElement,
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
            //init machStock props
            this.MachineryStock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.MachineryStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.MachineryStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.MachineryStock.InitTotalMachinery1StockProperties();
            this.MachineryStock.InitTotalMachinery1ConstantProperties();
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
        public bool SetTimePeriodMachineryStockCalculations(ref XElement currentCalculationsElement,
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
            //note that the machinput calculator can not change TimePeriod properties
            //but needs properties from the TimePeriod (i.e. Amount)
            this.TimePeriod.SetTimePeriodProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init machStock props
            this.MachineryStock.SetCalculatorProperties(currentCalculationsElement);
            //don't double count the tp multiplier -each op comp already used it to set penalties
            double dbMultiplier = 1; 
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.MachineryStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.MachineryStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next tp)
            this.MachineryStock.InitTotalMachinery1StockProperties();
            this.MachineryStock.InitTotalMachinery1ConstantProperties();
            //reset machStock.machstocks fileposition index
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
       
        public bool SetOpOrCompMachineryStockCalculations(ref XElement currentCalculationsElement,
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
            //note that the machinput calculator can not change Operation properties
            //but needs several properties from the Operation (i.e. Id, Amount)
            this.OpComp.SetOperationComponentProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init machStock props
            this.MachineryStock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.MachineryStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.MachineryStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.MachineryStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.MachineryStock.InitTotalMachinery1StockProperties();
            this.MachineryStock.InitTotalMachinery1ConstantProperties();
            //reset machStock.machstocks fileposition index
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
        public bool SetInputMachineryStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of machinputs
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            if (currentCalculationsElement != null)
            {
                //note that the machinput calculator can not change Input properties
                //when running from opOrComps or budgets
                //but needs several properties from the Input (i.e. Id, Times)
                this.Mach1Input = new Machinery1Input();
                //deserialize xml to object
                this.Mach1Input.SetMachinery1InputProperties(this.GCCalculatorParams,
                     currentCalculationsElement, currentElement);
                //init analyzer props
                this.MachineryStock.SetCalculatorProperties(currentCalculationsElement);
                //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                double dbMultiplier = GetInputFullCostMultiplier(this.Mach1Input, this.GCCalculatorParams);
                //change fuel cost, repair cost, by input.times * input.ocamount or input.aohamount
                ChangeMachineryInputByInputMultipliers(this.Mach1Input, dbMultiplier);
                //serialize calculator object back to xml
                //(calculator doesn't change opOrComp, so don't serialize it)
                string sAttNameExtension = string.Empty;
                //set new machinery input totals
                this.Mach1Input.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                this.Mach1Input.SetMachinery1InputAttributes(this.GCCalculatorParams,
                    ref currentCalculationsElement, ref currentElement);
                //set the totaloc and totalaoh
                AddMachinery1InputTotals(this.Mach1Input, ref currentCalculationsElement);
                //163: this.Mach1Input is sometimes used to build feasible input combos and needs input name when serialized
                if (this.Mach1Input.Name == string.Empty)   
                {
                    string sInputName = CalculatorHelpers.GetAttribute(currentElement, Calculator1.cName);
                    this.Mach1Input.Name = sInputName;
                }
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the machinput to the machStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.MachineryStock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.Mach1Input);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.MachineryStock
                    .AddMachinery1StocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.Mach1Input);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                //note that machinput can be retrieved by converting the input to the 
                //Machinery1Input type (machinput = (Machinery1Input) input)
                if (this.Mach1Input != null)
                {
                    this.OpComp.Inputs.Add(this.Mach1Input);
                }
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        
        public bool SetTotalMachineryStockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.MachineryStock.MachineryStocks != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<Machinery1Input>> kvp
                    in this.MachineryStock.MachineryStocks)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (Machinery1Input machinput in kvp.Value)
                        {
                            bool bAdjustTotals = true;
                            bHasCalculations
                                = AddMachineryInputToStock(multiplier, machinput, currentNodeName, bAdjustTotals);
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

        public bool AddMachineryInputToStock(double multiplier, Machinery1Input machinput, 
            string currentNodeName, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (input.times, input.ocamount, out.compositionamount, oc.amount, tp.amount)
                //are not used with per hourly costs, but are used with total op and aoh costs (fuelcost, capitalrecovery)
                if (currentNodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    //i.e. machinput.cost = input.times * input.ocamount
                    ChangeMachineryInputByInputMultipliers(machinput, multiplier);
                }
                else
                {
                    //i.e. budget adjustment = if operation multiplier = op.amount, 
                    //if timeperiod multiplier = tp.amount)
                    ChangeMachineryInputByMultiplier(machinput, multiplier);
                }
            }
            this.MachineryStock.TotalMarketValue += (machinput.MarketValue);
            this.MachineryStock.TotalSalvageValue += (machinput.SalvageValue);
            this.MachineryStock.TotalFuelAmount += (machinput.FuelAmount);
            this.MachineryStock.TotalFuelPrice += AgBudgetingRules.GetFuelPrice(machinput);
            this.MachineryStock.TotalFuelCost += (machinput.FuelCost);
            this.MachineryStock.TotalLubeOilAmount += (machinput.LubeOilAmount);
            this.MachineryStock.TotalLubeOilPrice += (machinput.Constants.PriceOil);
            this.MachineryStock.TotalLubeOilCost += (machinput.LubeOilCost);
            this.MachineryStock.TotalRepairCost += (machinput.RepairCost);
            this.MachineryStock.TotalLaborAmount += (machinput.LaborAmount);
            this.MachineryStock.TotalLaborPrice += AgBudgetingRules.GetLaborPrice(machinput);
            this.MachineryStock.TotalLaborCost += (machinput.LaborCost);
            this.MachineryStock.TotalCapitalRecoveryCost += (machinput.CapitalRecoveryCost);
            this.MachineryStock.TotalTaxesHousingInsuranceCost += (machinput.TaxesHousingInsuranceCost);
            this.MachineryStock.TotalPriceGas += (machinput.Constants.PriceGas);
            this.MachineryStock.TotalPriceDiesel += (machinput.Constants.PriceDiesel);
            this.MachineryStock.TotalPriceLP += (machinput.Constants.PriceLP);
            this.MachineryStock.TotalPriceElectric += (machinput.Constants.PriceElectric);
            this.MachineryStock.TotalPriceNG += (machinput.Constants.PriceNG);
            this.MachineryStock.TotalPriceOil += (machinput.Constants.PriceOil);
            this.MachineryStock.TotalPriceRegularLabor += (machinput.Constants.PriceRegularLabor);
            this.MachineryStock.TotalPriceMachineryLabor += (machinput.Constants.PriceMachineryLabor);
            this.MachineryStock.TotalPriceSupervisorLabor += (machinput.Constants.PriceSupervisorLabor);
            this.MachineryStock.TotalStartingHrs += (machinput.Constants.StartingHrs);
            this.MachineryStock.TotalPlannedUseHrs += (machinput.Constants.PlannedUseHrs);
            this.MachineryStock.TotalUsefulLifeHrs += (machinput.Constants.UsefulLifeHrs);
            this.MachineryStock.TotalHousingPercent += (machinput.Constants.HousingPercent);
            this.MachineryStock.TotalTaxPercent += (machinput.Constants.TaxPercent);
            this.MachineryStock.TotalInsurePercent += (machinput.Constants.InsurePercent);
            this.MachineryStock.TotalSpeed += (machinput.Constants.FieldSpeedTypical);
            this.MachineryStock.TotalWidth += (machinput.Constants.Width);
            this.MachineryStock.TotalHorsepower += (machinput.Constants.HP);
            this.MachineryStock.TotalHPPTOEquiv += (machinput.Constants.HPPTOEquiv);
            this.MachineryStock.TotalFieldEffTypical += (machinput.Constants.FieldEffTypical);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddMachineryInputToStock(Machinery1Stock machStock, double multiplier,
            Machinery1Input machinput, string currentNodeName, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
                //don't change per hour machinery costs, only total costs
                if (currentNodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    //i.e. machinput.cost = machinput.cost * multiplier * input.ocamount
                    //multiplier = input.times * oc.amount * tp.amount
                    ChangeMachineryInputByInputMultipliers(machinput, multiplier);
                }
                else
                {
                    //i.e. machinput.cost = machinput.cost * multiplier (1 in stock analyzers)
                    ChangeMachineryInputByMultiplier(machinput, multiplier);
                }
            }
            machStock.TotalMarketValue += (machinput.MarketValue);
            machStock.TotalSalvageValue += (machinput.SalvageValue);
            machStock.TotalFuelAmount += (machinput.FuelAmount);
            machStock.TotalFuelPrice += AgBudgetingRules.GetFuelPrice(machinput);
            machStock.TotalFuelCost += (machinput.FuelCost);
            machStock.TotalLubeOilAmount += (machinput.LubeOilAmount);
            machStock.TotalLubeOilPrice += (machinput.Constants.PriceOil);
            machStock.TotalLubeOilCost += (machinput.LubeOilCost);
            machStock.TotalRepairCost += (machinput.RepairCost);
            machStock.TotalLaborAmount += (machinput.LaborAmount);
            machStock.TotalLaborPrice += AgBudgetingRules.GetLaborPrice(machinput);
            machStock.TotalLaborCost += (machinput.LaborCost);
            machStock.TotalCapitalRecoveryCost += (machinput.CapitalRecoveryCost);
            machStock.TotalTaxesHousingInsuranceCost += (machinput.TaxesHousingInsuranceCost);
            machStock.TotalPriceGas += (machinput.Constants.PriceGas);
            machStock.TotalPriceDiesel += (machinput.Constants.PriceDiesel);
            machStock.TotalPriceLP += (machinput.Constants.PriceLP);
            machStock.TotalPriceElectric += (machinput.Constants.PriceElectric);
            machStock.TotalPriceNG += (machinput.Constants.PriceNG);
            machStock.TotalPriceOil += (machinput.Constants.PriceOil);
            machStock.TotalPriceRegularLabor += (machinput.Constants.PriceRegularLabor);
            machStock.TotalPriceMachineryLabor += (machinput.Constants.PriceMachineryLabor);
            machStock.TotalPriceSupervisorLabor += (machinput.Constants.PriceSupervisorLabor);
            machStock.TotalStartingHrs += (machinput.Constants.StartingHrs);
            machStock.TotalPlannedUseHrs += (machinput.Constants.PlannedUseHrs);
            machStock.TotalUsefulLifeHrs += (machinput.Constants.UsefulLifeHrs);
            machStock.TotalHousingPercent += (machinput.Constants.HousingPercent);
            machStock.TotalTaxPercent += (machinput.Constants.TaxPercent);
            machStock.TotalInsurePercent += (machinput.Constants.InsurePercent);
            machStock.TotalSpeed += (machinput.Constants.FieldSpeedTypical);
            machStock.TotalWidth += (machinput.Constants.Width);
            machStock.TotalHorsepower += (machinput.Constants.HP);
            machStock.TotalHPPTOEquiv += (machinput.Constants.HPPTOEquiv);
            machStock.TotalFieldEffTypical += (machinput.Constants.FieldEffTypical);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddMachinery1StockToStock(Machinery1Stock totalsMach1Stock,
            double multiplier, Machinery1Stock currentMach1Stock)
        {
            bool bHasCalculations = false;
            //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
            //don't change per hour machinery costs, only total costs
            totalsMach1Stock.TotalMarketValue += (currentMach1Stock.TotalMarketValue);
            totalsMach1Stock.TotalSalvageValue += (currentMach1Stock.TotalSalvageValue);
            totalsMach1Stock.TotalFuelAmount += (currentMach1Stock.TotalFuelAmount * multiplier);
            totalsMach1Stock.TotalFuelPrice += AgBudgetingRules.GetFuelPrice(currentMach1Stock);
            totalsMach1Stock.TotalFuelCost += (currentMach1Stock.TotalFuelCost * multiplier);
            totalsMach1Stock.TotalLubeOilAmount += (currentMach1Stock.TotalLubeOilAmount * multiplier);
            totalsMach1Stock.TotalLubeOilPrice += (currentMach1Stock.Constants.PriceOil);
            totalsMach1Stock.TotalLubeOilCost += (currentMach1Stock.TotalLubeOilCost * multiplier);
            totalsMach1Stock.TotalRepairCost += (currentMach1Stock.TotalRepairCost * multiplier);
            totalsMach1Stock.TotalLaborAmount += (currentMach1Stock.TotalLaborAmount * multiplier);
            totalsMach1Stock.TotalLaborPrice += AgBudgetingRules.GetLaborPrice(currentMach1Stock);
            totalsMach1Stock.TotalLaborCost += (currentMach1Stock.TotalLaborCost * multiplier);
            totalsMach1Stock.TotalCapitalRecoveryCost += (currentMach1Stock.TotalCapitalRecoveryCost * multiplier);
            totalsMach1Stock.TotalTaxesHousingInsuranceCost += (currentMach1Stock.TotalTaxesHousingInsuranceCost * multiplier);
            totalsMach1Stock.TotalPriceGas += (currentMach1Stock.Constants.PriceGas);
            totalsMach1Stock.TotalPriceDiesel += (currentMach1Stock.Constants.PriceDiesel);
            totalsMach1Stock.TotalPriceLP += (currentMach1Stock.Constants.PriceLP);
            totalsMach1Stock.TotalPriceElectric += (currentMach1Stock.Constants.PriceElectric);
            totalsMach1Stock.TotalPriceNG += (currentMach1Stock.Constants.PriceNG);
            totalsMach1Stock.TotalPriceOil += (currentMach1Stock.Constants.PriceOil);
            totalsMach1Stock.TotalPriceRegularLabor += (currentMach1Stock.Constants.PriceRegularLabor);
            totalsMach1Stock.TotalPriceMachineryLabor += (currentMach1Stock.Constants.PriceMachineryLabor);
            totalsMach1Stock.TotalPriceSupervisorLabor += (currentMach1Stock.Constants.PriceSupervisorLabor);
            totalsMach1Stock.TotalStartingHrs += (currentMach1Stock.Constants.StartingHrs);
            totalsMach1Stock.TotalPlannedUseHrs += (currentMach1Stock.Constants.PlannedUseHrs);
            totalsMach1Stock.TotalUsefulLifeHrs += (currentMach1Stock.Constants.UsefulLifeHrs);
            totalsMach1Stock.TotalHousingPercent += (currentMach1Stock.Constants.HousingPercent);
            totalsMach1Stock.TotalTaxPercent += (currentMach1Stock.Constants.TaxPercent);
            totalsMach1Stock.TotalInsurePercent += (currentMach1Stock.Constants.InsurePercent);
            totalsMach1Stock.TotalSpeed += (currentMach1Stock.Constants.FieldSpeedTypical);
            totalsMach1Stock.TotalWidth += (currentMach1Stock.Constants.Width);
            totalsMach1Stock.TotalHorsepower += (currentMach1Stock.Constants.HP);
            totalsMach1Stock.TotalHPPTOEquiv += (currentMach1Stock.Constants.HPPTOEquiv);
            totalsMach1Stock.TotalFieldEffTypical += (currentMach1Stock.Constants.FieldEffTypical);
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool NeedsFilePosition(int filePosition, string currentNodeName)
        {
            bool bNeedsFilePositionFacts = true;
            if (currentNodeName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentNodeName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
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
        public static double GetInputFullCostMultiplier(Machinery1Input machInput,
            CalculatorParameters calcParams)
        {
            //to keep all machinery stock analysis consistent, put the totals 
            //in the inputs, then all can be compared consistently
            //input.times
            double dbInputMultiplier = IOMachineryStockSubscriber
                .GetMultiplierForTechInput(machInput);
            //oc.amount
            double dbOCMultiplier = OCMachineryStockSubscriber
                .GetMultiplierForOperation(calcParams.ParentOperationComponent);
            double dbTPAmount = BIMachineryStockSubscriber.GetMultiplierForTimePeriod(
                calcParams.ParentTimePeriod);
            double dbMultiplier = dbInputMultiplier * dbOCMultiplier * dbTPAmount;
            return dbMultiplier;
        }
        
        public static void ChangeMachineryInputByMultiplier(Machinery1Input machInput,
            double multiplier)
        {
            //cost per hour * hrs/acre * input.times
            machInput.FuelCost = (machInput.FuelCost * multiplier);
            machInput.FuelAmount = (machInput.FuelAmount * multiplier);
            machInput.LubeOilCost = (machInput.LubeOilCost * multiplier);
            machInput.LubeOilAmount = (machInput.LubeOilAmount * multiplier);
            machInput.RepairCost = (machInput.RepairCost * multiplier);
            machInput.LaborCost = (machInput.LaborCost * multiplier);
            machInput.LaborAmount = (machInput.LaborAmount * multiplier);
            machInput.CapitalRecoveryCost = (machInput.CapitalRecoveryCost * multiplier);
            machInput.TaxesHousingInsuranceCost = (machInput.TaxesHousingInsuranceCost);
            //this is ok for machinerystock analysis (but not for npv)
            machInput.TotalOC = machInput.FuelCost + machInput.LubeOilCost +
                machInput.RepairCost + machInput.LaborCost;
            machInput.TotalAOH = machInput.CapitalRecoveryCost + machInput.TaxesHousingInsuranceCost;
        }
        public static void ChangeMachineryInputByInputMultipliers(Machinery1Input machInput, 
            double multiplier)
        {
            //cost per hour * hrs/acre * input.times
            machInput.FuelCost = (machInput.FuelCost * multiplier * machInput.OCAmount);
            machInput.FuelAmount = (machInput.FuelAmount * multiplier * machInput.OCAmount);
            machInput.LubeOilCost = (machInput.LubeOilCost * multiplier * machInput.OCAmount);
            machInput.LubeOilAmount = (machInput.LubeOilAmount * multiplier * machInput.OCAmount);
            machInput.RepairCost = (machInput.RepairCost * multiplier * machInput.OCAmount);
            machInput.LaborCost = (machInput.LaborCost * multiplier * machInput.OCAmount);
            machInput.LaborAmount = (machInput.LaborAmount * multiplier * machInput.OCAmount);
            //163 stopped using AOHAmount because it isn't always set set
            machInput.CapitalRecoveryCost = (machInput.CapitalRecoveryCost * multiplier * machInput.OCAmount);
            machInput.TaxesHousingInsuranceCost = (machInput.TaxesHousingInsuranceCost * multiplier * machInput.OCAmount);
            //this is ok for machinerystock analysis
            machInput.TotalOC = machInput.FuelCost + machInput.LubeOilCost +
                machInput.RepairCost + machInput.LaborCost;
            machInput.TotalAOH = machInput.CapitalRecoveryCost + machInput.TaxesHousingInsuranceCost;
        }
        
        public static void AddMachinery1InputTotals(Machinery1Input machInput,
            ref XElement machInputCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            machInput.TotalOC = machInput.FuelCost + machInput.LaborCost
                + machInput.RepairCost + machInput.LubeOilCost;
            machInput.TotalAOH = machInput.CapitalRecoveryCost
                + machInput.TaxesHousingInsuranceCost;
            CalculatorHelpers.SetAttributeDoubleF2(machInputCalcElement,
                CostBenefitCalculator.TOC, machInput.TotalOC);
            CalculatorHelpers.SetAttributeDoubleF2(machInputCalcElement,
                CostBenefitCalculator.TAOH, machInput.TotalAOH);
            //extra multiplier needed for display
            CalculatorHelpers.SetAttributeDoubleF2(machInputCalcElement,
                Input.INPUT_TIMES, machInput.Times);
        }
        public static void AddMachineryStockTotals(Machinery1Stock mach1Stock,
            ref XElement machStockCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            mach1Stock.TotalOC = mach1Stock.TotalFuelCost + mach1Stock.TotalLaborCost
                + mach1Stock.TotalRepairCost + mach1Stock.TotalLubeOilCost;
            mach1Stock.TotalAOH = mach1Stock.TotalCapitalRecoveryCost
                + mach1Stock.TotalTaxesHousingInsuranceCost;
            CalculatorHelpers.SetAttributeDoubleF2(machStockCalcElement,
                CostBenefitCalculator.TOC, mach1Stock.TotalOC);
            CalculatorHelpers.SetAttributeDoubleF2(machStockCalcElement,
                CostBenefitCalculator.TAOH, mach1Stock.TotalAOH);
        }
    }
}
