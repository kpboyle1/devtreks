using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run general capital stock calculations for operating 
    ///             and capital budgets
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. 
    /// </summary>
    public class BIGeneralCapitalStockCalculator : BudgetInvestmentCalculator
    {
        public BIGeneralCapitalStockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters) 
        {
            //base sets calc and analysis
            //this needs to set machstock
            Init(calcParameters);
        }
        private void Init(CalculatorParameters calcParameters)
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            this.GeneralCapitalStock = new GeneralCapital1Stock();
            this.GeneralCapitalStock.GenCapitalStocks
                = new Dictionary<int, List<GeneralCapital1Input>>();
            this.TimePeriodStartingFileIndex = 0;
            this.BudgetStartingFileIndex = 0;
        }
        //constants used by this calculator
        public const string ZERO = "0";
        //properties used by this calculator
        //stateful machinery stock
        public GeneralCapital1Stock GeneralCapitalStock { get; set; }
        //these objects hold collections of descendants for optimizing total costs
        public GeneralCapital1Input GenCapital1Input { get; set; }
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
        public Output Output { get; set; }

        public bool AddCalculationsToCurrentElement(
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
                bHasCalculations = SetTotalMachineryStockCalculations(
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
            this.GeneralCapitalStock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            //bimachstockcalculator handles calculations
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.GeneralCapitalStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalGeneralCapital1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.GeneralCapitalStock, ref currentCalculationsElement);
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
            this.GeneralCapitalStock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.GeneralCapitalStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalGeneralCapital1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.GeneralCapitalStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.GeneralCapitalStock.InitTotalMachinery1StockProperties();
            this.GeneralCapitalStock.InitTotalMachinery1ConstantProperties();
            this.GeneralCapitalStock.InitTotalGeneralCapital1StockProperties();
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
            this.GeneralCapitalStock.SetCalculatorProperties(currentCalculationsElement);
            double dbMultiplier = 1;
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.GeneralCapitalStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalGeneralCapital1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.GeneralCapitalStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.GeneralCapitalStock.InitTotalMachinery1StockProperties();
            this.GeneralCapitalStock.InitTotalMachinery1ConstantProperties();
            this.GeneralCapitalStock.InitTotalGeneralCapital1StockProperties();
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
            this.GeneralCapitalStock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.GeneralCapitalStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.GeneralCapitalStock.SetTotalGeneralCapital1StockAttributes(sAttNameExtension,
               ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.GeneralCapitalStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.GeneralCapitalStock.InitTotalMachinery1StockProperties();
            this.GeneralCapitalStock.InitTotalMachinery1ConstantProperties();
            this.GeneralCapitalStock.InitTotalGeneralCapital1StockProperties();
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
                this.GenCapital1Input = new GeneralCapital1Input();
                //deserialize xml to object
                this.GenCapital1Input.SetGeneralCapital1InputProperties(this.GCCalculatorParams,
                     currentCalculationsElement, currentElement);
                //init analyzer props
                this.GenCapital1Input.SetCalculatorProperties(currentCalculationsElement);
                //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                double dbMultiplier = BIMachineryStockCalculator.GetInputFullCostMultiplier(
                    this.GenCapital1Input, this.GCCalculatorParams);
                //change fuel cost, repair cost, by input.times * input.ocamount or input.aohamount
                ChangeMachineryInputByInputMultipliers(this.GenCapital1Input, dbMultiplier);
                //serialize calculator object back to xml
                //(calculator doesn't change opOrComp, so don't serialize it)
                string sAttNameExtension = string.Empty;
                //set new machinery input totals
                this.GenCapital1Input.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                this.GenCapital1Input.SetGeneralCapital1InputAttributes(this.GCCalculatorParams,
                   ref currentCalculationsElement, ref currentElement);
                //set the totaloc and totalaoh
                AddGeneralCapital1InputTotals(this.GenCapital1Input, ref currentCalculationsElement);
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the machinput to the machStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.GeneralCapitalStock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.GenCapital1Input);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.GeneralCapitalStock
                    .AddGeneralCapital1StocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.GenCapital1Input);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                //note that machinput can be retrieved by converting the input to the 
                //Machinery1Input type (machinput = (Machinery1Input) input)
                this.OpComp.Inputs.Add(this.GenCapital1Input);
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public bool SetTotalMachineryStockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.GeneralCapitalStock.GenCapitalStocks != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<GeneralCapital1Input>> kvp
                    in this.GeneralCapitalStock.GenCapitalStocks)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (GeneralCapital1Input machinput in kvp.Value)
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

        public bool AddMachineryInputToStock(double multiplier, GeneralCapital1Input machinput,
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
            this.GeneralCapitalStock.TotalMarketValue += (machinput.MarketValue);
            this.GeneralCapitalStock.TotalSalvageValue += (machinput.SalvageValue);
            this.GeneralCapitalStock.TotalFuelAmount += (machinput.FuelAmount);
            this.GeneralCapitalStock.TotalFuelPrice += AgBudgetingRules.GetFuelPrice(machinput);
            this.GeneralCapitalStock.TotalFuelCost += (machinput.FuelCost);
            this.GeneralCapitalStock.TotalEnergyUseHr += (machinput.EnergyUseHr);
            this.GeneralCapitalStock.TotalEnergyEffTypical += (machinput.EnergyEffTypical);
            this.GeneralCapitalStock.TotalRandMPercent += (machinput.Capital1Constant.RandMPercent);
            this.GeneralCapitalStock.TotalRepairCost += (machinput.RepairCost);
            this.GeneralCapitalStock.TotalLaborAmount += (machinput.LaborAmount);
            this.GeneralCapitalStock.TotalLaborPrice += AgBudgetingRules.GetLaborPrice(machinput);
            this.GeneralCapitalStock.TotalLaborCost += (machinput.LaborCost);
            this.GeneralCapitalStock.TotalCapitalRecoveryCost += (machinput.CapitalRecoveryCost);
            this.GeneralCapitalStock.TotalTaxesHousingInsuranceCost += (machinput.TaxesHousingInsuranceCost);
            this.GeneralCapitalStock.TotalPriceGas += (machinput.Constants.PriceGas);
            this.GeneralCapitalStock.TotalPriceDiesel += (machinput.Constants.PriceDiesel);
            this.GeneralCapitalStock.TotalPriceLP += (machinput.Constants.PriceLP);
            this.GeneralCapitalStock.TotalPriceElectric += (machinput.Constants.PriceElectric);
            this.GeneralCapitalStock.TotalPriceNG += (machinput.Constants.PriceNG);
            this.GeneralCapitalStock.TotalPriceOil += (machinput.Constants.PriceOil);
            this.GeneralCapitalStock.TotalPriceRegularLabor += (machinput.Constants.PriceRegularLabor);
            this.GeneralCapitalStock.TotalPriceMachineryLabor += (machinput.Constants.PriceMachineryLabor);
            this.GeneralCapitalStock.TotalPriceSupervisorLabor += (machinput.Constants.PriceSupervisorLabor);
            this.GeneralCapitalStock.TotalStartingHrs += (machinput.Constants.StartingHrs);
            this.GeneralCapitalStock.TotalPlannedUseHrs += (machinput.Constants.PlannedUseHrs);
            this.GeneralCapitalStock.TotalUsefulLifeHrs += (machinput.Constants.UsefulLifeHrs);
            this.GeneralCapitalStock.TotalHousingPercent += (machinput.Constants.HousingPercent);
            this.GeneralCapitalStock.TotalTaxPercent += (machinput.Constants.TaxPercent);
            this.GeneralCapitalStock.TotalInsurePercent += (machinput.Constants.InsurePercent);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddMachineryInputToStock(GeneralCapital1Stock machStock, double multiplier,
            GeneralCapital1Input machinput, string currentNodeName, bool adjustTotals)
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
            machStock.TotalEnergyUseHr += (machinput.EnergyUseHr);
            machStock.TotalEnergyEffTypical += (machinput.EnergyEffTypical);
            machStock.TotalRandMPercent += (machinput.Capital1Constant.RandMPercent);
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
            bHasCalculations = true;
            return bHasCalculations;
        }

        private bool NeedsFilePosition(int filePosition, string currentNodeName)
        {
            bool bNeedsFilePositionFacts = true;
            if (currentNodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString())
                || currentNodeName.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                if (this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                    == Constants.SUBAPPLICATION_TYPES.inputprices.ToString()
                    || this.GCCalculatorParams.ExtensionDocToCalcURI.URIDataManager.SubAppType
                    == Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
                {
                    //each input or output is a file position member
                    //aggregate only using the current file position
                    if (this.GCCalculatorParams.AnalyzerParms.FilePositionIndex != filePosition)
                    {
                        bNeedsFilePositionFacts = false;
                    }
                }
            }
            else if (currentNodeName
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

        public static void ChangeMachineryInputByMultiplier(GeneralCapital1Input machInput,
            double multiplier)
        {
            //cost per hour * hrs/acre * input.times
            machInput.FuelCost = (machInput.FuelCost * multiplier);
            machInput.FuelAmount = (machInput.FuelAmount * multiplier);
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
        public static void ChangeMachineryInputByInputMultipliers(GeneralCapital1Input machInput,
            double multiplier)
        {
            //cost per hour * hrs/acre * input.times
            machInput.FuelCost = (machInput.FuelCost * multiplier * machInput.OCAmount);
            machInput.FuelAmount = (machInput.FuelAmount * multiplier * machInput.OCAmount);
            machInput.RepairCost = (machInput.RepairCost * multiplier * machInput.OCAmount);
            machInput.LaborCost = (machInput.LaborCost * multiplier * machInput.OCAmount);
            machInput.LaborAmount = (machInput.LaborAmount * multiplier * machInput.OCAmount);
            machInput.CapitalRecoveryCost = (machInput.CapitalRecoveryCost * multiplier * machInput.AOHAmount);
            machInput.TaxesHousingInsuranceCost = (machInput.TaxesHousingInsuranceCost * multiplier * machInput.AOHAmount);
            //this is ok for machinerystock analysis
            machInput.TotalOC = machInput.FuelCost + machInput.RepairCost + machInput.LaborCost;
            machInput.TotalAOH = machInput.CapitalRecoveryCost + machInput.TaxesHousingInsuranceCost;
        }
        public static void AddGeneralCapital1InputTotals(GeneralCapital1Input machInput,
            ref XElement machInputCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            machInput.TotalOC = machInput.FuelCost + machInput.LaborCost
                + machInput.RepairCost;
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
        public static void AddMachineryStockTotals(GeneralCapital1Stock mach1Stock,
            ref XElement machStockCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            mach1Stock.TotalOC = mach1Stock.TotalFuelCost + mach1Stock.TotalLaborCost
                + mach1Stock.TotalRepairCost;
            mach1Stock.TotalAOH = mach1Stock.TotalCapitalRecoveryCost
                + mach1Stock.TotalTaxesHousingInsuranceCost;
            CalculatorHelpers.SetAttributeDoubleF2(machStockCalcElement,
                CostBenefitCalculator.TOC, mach1Stock.TotalOC);
            CalculatorHelpers.SetAttributeDoubleF2(machStockCalcElement,
                CostBenefitCalculator.TAOH, mach1Stock.TotalAOH);
        }
        
    }
}
