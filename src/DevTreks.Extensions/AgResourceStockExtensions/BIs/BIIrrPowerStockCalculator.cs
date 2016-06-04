using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run irrigation power stock calculations for operating 
    ///             and capital budgets
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. 
    /// </summary>
    public class BIIrrPowerStockCalculator : BudgetInvestmentCalculator
    {
        public BIIrrPowerStockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters) 
        {
            //base sets calc and analysis
            //this needs to set machstock
            Init();
        }
        private void Init()
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            this.IrrPowerStock = new IrrPower1Stock();
            this.IrrPowerStock.IrrPowerStocks
                = new Dictionary<int, List<IrrigationPower1Input>>();
            this.TimePeriodStartingFileIndex = 0;
            this.BudgetStartingFileIndex = 0;
        }
        //constants used by this calculator
        public const string ZERO = "0";
        //properties used by this calculator
        //stateful machinery stock
        public IrrPower1Stock IrrPowerStock { get; set; }
        //these objects hold collections of descendants for optimizing total costs
        public IrrigationPower1Input IrrPower1Input { get; set; }
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
            this.IrrPowerStock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            //bimachstockcalculator handles calculations
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            ////bool bRemoveAtts = true;
            //set new machinery stock totals
            this.IrrPowerStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //fuel cost, fuel amount
            this.IrrPowerStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //price diesel, price labor
            this.IrrPowerStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.IrrPowerStock.SetTotalIrrPower1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh 
            AddMachineryStockTotals(this.IrrPowerStock, ref currentCalculationsElement);
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
            this.IrrPowerStock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            ////bool bRemoveAtts = true;
            //set new machinery stock totals
            this.IrrPowerStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //fuel cost, fuel amount
            this.IrrPowerStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //price diesel, price labor
            this.IrrPowerStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.IrrPowerStock.SetTotalIrrPower1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.IrrPowerStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.IrrPowerStock.InitTotalMachinery1StockProperties();
            this.IrrPowerStock.InitTotalMachinery1ConstantProperties();
            this.IrrPowerStock.InitTotalIrrPower1StockProperties();
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
            this.IrrPowerStock.SetCalculatorProperties(currentCalculationsElement);
            //don't double count the tp multiplier -each op comp already used it to set penalties
            double dbMultiplier = 1; 
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //bool bRemoveAtts = true;
            //set new machinery stock totals
            this.IrrPowerStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //fuel cost, fuel amount
            this.IrrPowerStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //price diesel, price labor
            this.IrrPowerStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.IrrPowerStock.SetTotalIrrPower1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.IrrPowerStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.IrrPowerStock.InitTotalMachinery1StockProperties();
            this.IrrPowerStock.InitTotalMachinery1ConstantProperties();
            this.IrrPowerStock.InitTotalIrrPower1StockProperties();
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
            this.IrrPowerStock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachineryStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new machinery stock totals
            this.IrrPowerStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //fuel cost, fuel amount
            this.IrrPowerStock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //price diesel, price labor
            this.IrrPowerStock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.IrrPowerStock.SetTotalIrrPower1StockAttributes(sAttNameExtension,
               ref currentCalculationsElement);
            //set the totaloc and totalaoh
            AddMachineryStockTotals(this.IrrPowerStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.IrrPowerStock.InitTotalMachinery1StockProperties();
            this.IrrPowerStock.InitTotalMachinery1ConstantProperties();
            this.IrrPowerStock.InitTotalIrrPower1StockProperties();
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
                this.IrrPower1Input = new IrrigationPower1Input();
                //deserialize xml to object
                this.IrrPower1Input.SetIrrigationPower1InputProperties(this.GCCalculatorParams,
                     currentCalculationsElement, currentElement);
                //init analyzer props
                this.IrrPower1Input.SetCalculatorProperties(currentCalculationsElement);
                //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                double dbMultiplier = BIMachineryStockCalculator.GetInputFullCostMultiplier(
                    this.IrrPower1Input, this.GCCalculatorParams);
                //change fuel cost, repair cost, by input.times * input.ocamount or input.aohamount
                ChangeMachineryInputByInputMultipliers(this.IrrPower1Input, dbMultiplier);
                //serialize calculator object back to xml
                //(calculator doesn't change opOrComp, so don't serialize it)
                string sAttNameExtension = string.Empty;
                //set new machinery input totals
                this.IrrPower1Input.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                this.IrrPower1Input.SetIrrigationPower1Attributes(this.GCCalculatorParams,
                       ref currentCalculationsElement, ref currentElement);
                //set the totaloc and totalaoh
                AddIrrPower1InputTotals(this.IrrPower1Input, ref currentCalculationsElement);
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the machinput to the machStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.IrrPowerStock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.IrrPower1Input);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.IrrPowerStock
                    .AddIrrPower1StocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.IrrPower1Input);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                //note that machinput can be retrieved by converting the input to the 
                //IrrPower1Input type (machinput = (IrrPower1Input) input)
                this.OpComp.Inputs.Add(this.IrrPower1Input);
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        public bool SetTotalMachineryStockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.IrrPowerStock.IrrPowerStocks != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<IrrigationPower1Input>> kvp
                    in this.IrrPowerStock.IrrPowerStocks)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (IrrigationPower1Input machinput in kvp.Value)
                        {
                            bHasCalculations
                                = AddMachineryInputToStock(multiplier, machinput, currentNodeName);
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
        public bool AddMachineryInputToStock(double multiplier, IrrigationPower1Input machinput,
            string currentNodeName)
        {
            bool bHasCalculations = false;
            //add the base machinput properties
            bool bAdjustTotals = false;
            bHasCalculations
                = BIMachineryStockCalculator
                .AddMachineryInputToStock(this.IrrPowerStock, multiplier, machinput, currentNodeName, bAdjustTotals);
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
            this.IrrPowerStock.TotalEngineEfficiency += machinput.EngineEfficiency;
            this.IrrPowerStock.TotalFuelConsumptionPerHour += machinput.FuelConsumptionPerHour;
            this.IrrPowerStock.TotalWaterHP += machinput.WaterHP;
            this.IrrPowerStock.TotalBrakeHP += machinput.BrakeHP;
            this.IrrPowerStock.TotalFlowRate += machinput.FlowRate;
            this.IrrPowerStock.TotalStaticHead += machinput.StaticHead;
            this.IrrPowerStock.TotalPressureHead += machinput.PressureHead;
            this.IrrPowerStock.TotalFrictionHead += machinput.FrictionHead;
            this.IrrPowerStock.TotalOtherHead += machinput.OtherHead;
            this.IrrPowerStock.TotalPumpEfficiency += machinput.PumpEfficiency;
            this.IrrPowerStock.TotalGearDriveEfficiency += machinput.GearDriveEfficiency;
            this.IrrPowerStock.TotalExtraPower1 += machinput.ExtraPower1;
            this.IrrPowerStock.TotalExtraPower2 += machinput.ExtraPower2;
            this.IrrPowerStock.TotalEnergyExtraCostPerNetAcOrHa += machinput.EnergyExtraCostPerNetAcOrHa;
            this.IrrPowerStock.TotalEnergyExtraCost += machinput.EnergyExtraCost;
            this.IrrPowerStock.TotalPumpCapacity += machinput.PumpCapacity;
            this.IrrPowerStock.TotalEngineFlywheelPower += machinput.EngineFlywheelPower;
            this.IrrPowerStock.TotalFuelAmountRequired += machinput.FuelAmountRequired;
            this.IrrPowerStock.TotalPumpingPlantPerformance += machinput.PumpingPlantPerformance;
            this.IrrPowerStock.TotalSeasonWaterNeed += machinput.SeasonWaterNeed;
            this.IrrPowerStock.TotalSeasonWaterExtraCredit += machinput.SeasonWaterExtraCredit;
            this.IrrPowerStock.TotalSeasonWaterExtraDebit += machinput.SeasonWaterExtraDebit;
            this.IrrPowerStock.TotalWaterPrice += machinput.WaterPrice;
            this.IrrPowerStock.TotalDistributionUniformity += machinput.DistributionUniformity;
            this.IrrPowerStock.TotalSeasonWaterApplied += machinput.SeasonWaterApplied;
            this.IrrPowerStock.TotalWaterCost += machinput.WaterCost;
            this.IrrPowerStock.TotalPumpHoursPerUnitArea += machinput.PumpHoursPerUnitArea;
            this.IrrPowerStock.TotalIrrigationTimes += machinput.IrrigationTimes;
            this.IrrPowerStock.TotalIrrigationDurationPerSet += machinput.IrrigationDurationPerSet;
            this.IrrPowerStock.TotalIrrigationDurationLaborHoursPerSet += machinput.IrrigationDurationLaborHoursPerSet;
            this.IrrPowerStock.TotalIrrigationNetArea += machinput.IrrigationNetArea;
            this.IrrPowerStock.TotalEquipmentLaborAmount += machinput.EquipmentLaborAmount;
            this.IrrPowerStock.TotalEquipmentLaborCost += machinput.EquipmentLaborCost;
            this.IrrPowerStock.TotalRepairCostsPerNetAcOrHa += machinput.RepairCostsPerNetAcOrHa;
            this.IrrPowerStock.TotalRandMPercent += machinput.RandMPercent;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddMachineryInputToStock(IrrPower1Stock machStock, double multiplier,
            IrrigationPower1Input machinput, string currentNodeName)
        {
            bool bHasCalculations = false;
            bool bAdjustTotals = false;
            //eliminate double counting before using this
            //add the base totals (fuelamount, fuelcost ...)
            BIMachineryStockCalculator.AddMachineryInputToStock(machStock, multiplier,
                machinput, currentNodeName, bAdjustTotals);
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
            //add the totals for the irrpower stock
            machStock.TotalEngineEfficiency += machinput.EngineEfficiency;
            machStock.TotalFuelConsumptionPerHour += machinput.FuelConsumptionPerHour;
            machStock.TotalWaterHP += machinput.WaterHP;
            machStock.TotalBrakeHP += machinput.BrakeHP;
            machStock.TotalFlowRate += machinput.FlowRate;
            machStock.TotalStaticHead += machinput.StaticHead;
            machStock.TotalPressureHead += machinput.PressureHead;
            machStock.TotalFrictionHead += machinput.FrictionHead;
            machStock.TotalOtherHead += machinput.OtherHead;
            machStock.TotalPumpEfficiency += machinput.PumpEfficiency;
            machStock.TotalGearDriveEfficiency += machinput.GearDriveEfficiency;
            machStock.TotalExtraPower1 += machinput.ExtraPower1;
            machStock.TotalExtraPower2 += machinput.ExtraPower2;
            machStock.TotalEnergyExtraCostPerNetAcOrHa += machinput.EnergyExtraCostPerNetAcOrHa;
            machStock.TotalEnergyExtraCost += machinput.EnergyExtraCost;
            machStock.TotalPumpCapacity += machinput.PumpCapacity;
            machStock.TotalEngineFlywheelPower += machinput.EngineFlywheelPower;
            machStock.TotalFuelAmountRequired += machinput.FuelAmountRequired;
            machStock.TotalPumpingPlantPerformance += machinput.PumpingPlantPerformance;
            machStock.TotalSeasonWaterNeed += machinput.SeasonWaterNeed;
            machStock.TotalSeasonWaterExtraCredit += machinput.SeasonWaterExtraCredit;
            machStock.TotalSeasonWaterExtraDebit += machinput.SeasonWaterExtraDebit;
            machStock.TotalWaterPrice += machinput.WaterPrice;
            machStock.TotalDistributionUniformity += machinput.DistributionUniformity;
            machStock.TotalSeasonWaterApplied += machinput.SeasonWaterApplied;
            machStock.TotalWaterCost += machinput.WaterCost;
            machStock.TotalPumpHoursPerUnitArea += machinput.PumpHoursPerUnitArea;
            machStock.TotalIrrigationTimes += machinput.IrrigationTimes;
            machStock.TotalIrrigationDurationPerSet += machinput.IrrigationDurationPerSet;
            machStock.TotalIrrigationDurationLaborHoursPerSet += machinput.IrrigationDurationLaborHoursPerSet;
            machStock.TotalIrrigationNetArea += machinput.IrrigationNetArea;
            machStock.TotalEquipmentLaborAmount += machinput.EquipmentLaborAmount;
            machStock.TotalEquipmentLaborCost += machinput.EquipmentLaborCost;
            machStock.TotalRepairCostsPerNetAcOrHa += machinput.RepairCostsPerNetAcOrHa;
            machStock.TotalRandMPercent += machinput.RandMPercent;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddMachinery1StockToStock(IrrPower1Stock totalsMach1Stock,
            double multiplier, IrrPower1Stock currentMach1Stock)
        {
            bool bHasCalculations = false;
            //eliminate double counting before using this
            //add the base totals (fuelamount, fuelcost ...)
            BIMachineryStockCalculator.AddMachinery1StockToStock(totalsMach1Stock, multiplier,
                currentMach1Stock);
            //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
            //don't change per hour machinery costs, only total costs
            //add the totals for the irrpower stock
            totalsMach1Stock.TotalEngineEfficiency += currentMach1Stock.TotalEngineEfficiency;
            totalsMach1Stock.TotalFuelConsumptionPerHour += currentMach1Stock.TotalFuelConsumptionPerHour;
            totalsMach1Stock.TotalWaterHP += currentMach1Stock.TotalWaterHP;
            totalsMach1Stock.TotalBrakeHP += currentMach1Stock.TotalBrakeHP;
            totalsMach1Stock.TotalFlowRate += currentMach1Stock.TotalFlowRate;
            totalsMach1Stock.TotalStaticHead += currentMach1Stock.TotalStaticHead;
            totalsMach1Stock.TotalPressureHead += currentMach1Stock.TotalPressureHead;
            totalsMach1Stock.TotalFrictionHead += currentMach1Stock.TotalFrictionHead;
            totalsMach1Stock.TotalOtherHead += currentMach1Stock.TotalOtherHead;
            totalsMach1Stock.TotalPumpEfficiency += currentMach1Stock.TotalPumpEfficiency;
            totalsMach1Stock.TotalGearDriveEfficiency += currentMach1Stock.TotalGearDriveEfficiency;
            totalsMach1Stock.TotalExtraPower1 += currentMach1Stock.TotalExtraPower1;
            totalsMach1Stock.TotalExtraPower2 += currentMach1Stock.TotalExtraPower2;
            totalsMach1Stock.TotalEnergyExtraCostPerNetAcOrHa += currentMach1Stock.TotalEnergyExtraCostPerNetAcOrHa;
            totalsMach1Stock.TotalEnergyExtraCost += currentMach1Stock.TotalEnergyExtraCost;
            totalsMach1Stock.TotalPumpCapacity += currentMach1Stock.TotalPumpCapacity;
            totalsMach1Stock.TotalEngineFlywheelPower += currentMach1Stock.TotalEngineFlywheelPower;
            totalsMach1Stock.TotalFuelAmountRequired += currentMach1Stock.TotalFuelAmountRequired;
            totalsMach1Stock.TotalPumpingPlantPerformance += currentMach1Stock.TotalPumpingPlantPerformance;
            totalsMach1Stock.TotalSeasonWaterNeed += currentMach1Stock.TotalSeasonWaterNeed;
            totalsMach1Stock.TotalSeasonWaterExtraCredit += currentMach1Stock.TotalSeasonWaterExtraCredit;
            totalsMach1Stock.TotalSeasonWaterExtraDebit += currentMach1Stock.TotalSeasonWaterExtraDebit;
            totalsMach1Stock.TotalWaterPrice += currentMach1Stock.TotalWaterPrice;
            totalsMach1Stock.TotalDistributionUniformity += currentMach1Stock.TotalDistributionUniformity;
            totalsMach1Stock.TotalSeasonWaterApplied += currentMach1Stock.TotalSeasonWaterApplied;
            totalsMach1Stock.TotalWaterCost += currentMach1Stock.TotalWaterCost;
            totalsMach1Stock.TotalPumpHoursPerUnitArea += currentMach1Stock.TotalPumpHoursPerUnitArea;
            totalsMach1Stock.TotalIrrigationTimes += currentMach1Stock.TotalIrrigationTimes;
            totalsMach1Stock.TotalIrrigationDurationPerSet += currentMach1Stock.TotalIrrigationDurationPerSet;
            totalsMach1Stock.TotalIrrigationDurationLaborHoursPerSet += currentMach1Stock.TotalIrrigationDurationLaborHoursPerSet;
            totalsMach1Stock.TotalIrrigationNetArea += currentMach1Stock.TotalIrrigationNetArea;
            totalsMach1Stock.TotalEquipmentLaborAmount += currentMach1Stock.TotalEquipmentLaborAmount;
            totalsMach1Stock.TotalEquipmentLaborCost += currentMach1Stock.TotalEquipmentLaborCost;
            totalsMach1Stock.TotalRepairCostsPerNetAcOrHa += currentMach1Stock.TotalRepairCostsPerNetAcOrHa;
            totalsMach1Stock.TotalRandMPercent += currentMach1Stock.TotalRandMPercent;
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


        public static void ChangeMachineryInputByMultiplier(IrrigationPower1Input machInput, 
            double multiplier)
        {
            //cost per hour * hrs/acre * input.times
            machInput.FuelCost = (machInput.FuelCost * multiplier);
            machInput.FuelAmount = (machInput.FuelAmount * multiplier);
            //extra energy
            machInput.EnergyExtraCost = (machInput.EnergyExtraCost * multiplier);
            //lube
            machInput.LubeOilCost = (machInput.LubeOilCost * multiplier);
            machInput.LubeOilAmount = (machInput.LubeOilAmount * multiplier);
            //repair
            machInput.RepairCost = (machInput.RepairCost * multiplier);
            //water
            machInput.SeasonWaterApplied = (machInput.SeasonWaterApplied * multiplier);
            machInput.WaterCost = (machInput.WaterCost * multiplier);
            //irrigation labor
            machInput.LaborCost = (machInput.LaborCost * multiplier);
            machInput.LaborAmount = (machInput.LaborAmount * multiplier);
            //equip labor
            machInput.EquipmentLaborAmount = (machInput.EquipmentLaborAmount * multiplier);
            machInput.EquipmentLaborCost = (machInput.EquipmentLaborCost * multiplier);
            //cap recovery
            machInput.CapitalRecoveryCost = (machInput.CapitalRecoveryCost * multiplier);
            machInput.TaxesHousingInsuranceCost = (machInput.TaxesHousingInsuranceCost);
            //this is ok for machinerystock analysis (but not for npv)
            machInput.TotalOC = machInput.FuelCost + machInput.LubeOilCost
                + machInput.RepairCost + machInput.LaborCost
                + machInput.EnergyExtraCost + machInput.WaterCost + machInput.EquipmentLaborCost;
            machInput.TotalAOH = machInput.CapitalRecoveryCost + machInput.TaxesHousingInsuranceCost;
        }
        public static void ChangeMachineryInputByInputMultipliers(IrrigationPower1Input machInput,
            double multiplier)
        {
            //cost = cost per acin * acin/ac * input.times * comp.amount * tp.amount
            machInput.FuelCost = (machInput.FuelCost * multiplier * machInput.OCAmount);
            //amount = amount per acre * input.times * comp.amount * tp.amount
            machInput.FuelAmount = (machInput.FuelAmount * multiplier);
            //extra energy
            machInput.EnergyExtraCost = (machInput.EnergyExtraCost * multiplier * machInput.OCAmount);
            //lube
            machInput.LubeOilCost = (machInput.LubeOilCost * multiplier * machInput.OCAmount);
            machInput.LubeOilAmount = (machInput.LubeOilAmount * multiplier);
            machInput.RepairCost = (machInput.RepairCost * multiplier * machInput.OCAmount);
            //water
            machInput.SeasonWaterApplied = (machInput.SeasonWaterApplied * multiplier * machInput.OCAmount);
            machInput.WaterCost = (machInput.WaterCost * multiplier * machInput.OCAmount);
            //irrigation labor
            machInput.LaborCost = (machInput.LaborCost * multiplier * machInput.OCAmount);
            machInput.LaborAmount = (machInput.LaborAmount * multiplier);
            //equip labor
            machInput.EquipmentLaborAmount = (machInput.EquipmentLaborAmount * multiplier);
            machInput.EquipmentLaborCost = (machInput.EquipmentLaborCost * multiplier * machInput.OCAmount);

            machInput.CapitalRecoveryCost = (machInput.CapitalRecoveryCost * multiplier * machInput.AOHAmount);
            machInput.TaxesHousingInsuranceCost = (machInput.TaxesHousingInsuranceCost * multiplier * machInput.AOHAmount);
            //this is ok for machinerystock analysis
            machInput.TotalOC = machInput.FuelCost + machInput.LubeOilCost 
                + machInput.RepairCost + machInput.LaborCost
                + machInput.EnergyExtraCost + machInput.WaterCost + machInput.EquipmentLaborCost;
            machInput.TotalAOH = machInput.CapitalRecoveryCost + machInput.TaxesHousingInsuranceCost;
        }
        public static void AddIrrPower1InputTotals(IrrigationPower1Input machInput,
            ref XElement machInputCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            machInput.TotalOC = machInput.FuelCost + machInput.LaborCost
                + machInput.RepairCost + machInput.LubeOilCost
                + machInput.EnergyExtraCost + machInput.WaterCost + machInput.EquipmentLaborCost;
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
        public static void AddMachineryStockTotals(IrrPower1Stock mach1Stock,
            ref XElement machStockCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            mach1Stock.TotalOC = mach1Stock.TotalFuelCost + mach1Stock.TotalLaborCost
                + mach1Stock.TotalRepairCost + mach1Stock.TotalLubeOilCost
                + mach1Stock.TotalEnergyExtraCost + mach1Stock.TotalWaterCost + mach1Stock.TotalEquipmentLaborCost;
            mach1Stock.TotalAOH = mach1Stock.TotalCapitalRecoveryCost
                + mach1Stock.TotalTaxesHousingInsuranceCost;
            CalculatorHelpers.SetAttributeDoubleF2(machStockCalcElement,
                CostBenefitCalculator.TOC, mach1Stock.TotalOC);
            CalculatorHelpers.SetAttributeDoubleF2(machStockCalcElement,
                CostBenefitCalculator.TAOH, mach1Stock.TotalAOH);
        }
    }
}
