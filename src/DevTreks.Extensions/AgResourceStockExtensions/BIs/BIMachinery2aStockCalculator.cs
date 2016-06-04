using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Extends BIMachinery2StockCalculator with least cost stock calculations 
    ///             for operating and capital budgets.
    ///Author:		www.devtreks.org
    ///Date:		2014, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// NOTES       1. Keep MachStock and Mach2Stock collections in synch by using the 
    ///             exact same fileposition index in both -will then have both 
    ///             machinery total costs and timeliness total costs. The stock
    ///             fileposition index will be handled by this class.
    ///             Most total calculations are run using the base Timeliness 
    ///             collections holding base objects, such as inputs, rather 
    ///             than the base machinery stock collections.
    /// </summary>
    public class BIMachinery2aStockCalculator : BIMachinery2StockCalculator
    {
        public BIMachinery2aStockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets the stocks properties
            this.PowerInputs = new List<Machinery1Input>();
            this.NonPowerInputs = new List<Machinery1Input>();
            this.RandomId = new Random(0);
            this.StartingFilePositionIdex = 0;
        }
        
        //properties
        //carries out internal calculations
        private List<Machinery1Input> PowerInputs { get; set; }
        private List<Machinery1Input> NonPowerInputs { get; set; }
        private Random RandomId { get; set; }
        //starts with current op/comp and adds new fps for new size combos
        private int StartingFilePositionIdex { get; set; }

        public bool AddStock2aCalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //input machinery1 facts are aggregated by each ancestor
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetTotalMachinery2aStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIMachinery2aStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodMachinery2aStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                bHasCalculations = SetOutcome2aCollection(ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                bHasCalculations = SetOutput2aCollection(ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //init the stock2a calculations
                bHasCalculations = SetOpOrCompMachinery2aStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetInputMachinery2aStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        public bool SetTotalMachinery2aStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. groups)
            bool bHasCalculations = false;
            if (currentCalculationsElement == null)
            {
                //budget resource stock analyzers always show budget totals which are stored in
                //currentCalcElement; if null (uses set NeedsChildren to false) set it to 
                //current linkedview
                currentCalculationsElement = this.GCCalculatorParams.LinkedViewElement;
            }
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                //serialize the stateful collections into currentcalcsElement
                bHasCalculations = SerializeOpCompGroupMachinery(ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                //Step 1. set both stock collections
                int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
                bHasCalculations = this.SetTotalMachineryStockCalculations(ref currentCalculationsElement, ref currentElement);
                this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
                bHasCalculations = this.SetTotalMachinery2StockCalculations(ref currentCalculationsElement, ref currentElement);
                this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
                //serialize the stateful collections into currentcalcsElement
                bHasCalculations = SerializeBIGroupMachinery(ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        public bool SetBIMachinery2aStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroup)
            bool bHasCalculations = false;
            //Step 1. set both stock collections
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            bHasCalculations = this.SetBIMachinery2StockJointCalculations(ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            //Step 2. Generate new collections of optimized machinery collections for each tp
            //the tps should be the same (second tp checks for date overlaps with first and 
            //runs conflict subroutines to minimize costs (i.e. by switching to higher hp equipment)
            bHasCalculations = ReplaceWithBestMachineryCollections(ref currentCalculationsElement, ref currentElement);
            if (!bHasCalculations)
            {
                this.GCCalculatorParams.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS2A_NOBESTTIMELINESSCALCS"); ;
            }
            return bHasCalculations;
        }
        
        public bool SetTimePeriodMachinery2aStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            bHasCalculations = this.SetTimePeriodMachinery2StockJointCalculations(
                ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            //v162 eliminated -last method recalcs machstock2; this method double counts tp amount in oc amounts
            ////and recalculate collections (uses the Timeliness collections rather than MachStocks)
            //this.ReCalculateTimePeriodTimelinessOpCompCollections();
            return bHasCalculations;
        }
        public bool SetOutcome2aCollection(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //only need to run stock2 outcomes
            bHasCalculations = this.SetOutcome2Collection(ref currentCalculationsElement, ref currentElement);
            return bHasCalculations;
        }
        public bool SetOutput2aCollection(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //only need to run stock2 outputs
            bHasCalculations = this.SetOutput2Collection(ref currentCalculationsElement, ref currentElement);
            return bHasCalculations;
        }
        public bool SetOpOrCompMachinery2aStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //Step 1. Set both stock collections
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            bHasCalculations = this.SetOpOrCompMachinery2StockJointCalculations(
                ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            //bHasCalculations = SetInitialOpCompStocks(ref currentCalculationsElement, ref currentElement);
            //Step 2. Replace the this.Mach1Stock and Mach2Stock collections with feasible (matched) machinery combos
            //keep the same exact indices for both collections (so machstocks costs will aggregate correctly)
            //note that this also runs for existing machinery that doesn't need to be recalcd (or won't be displayed)
            bHasCalculations = ReplaceOCMachineryCollections(ref currentCalculationsElement, ref currentElement);
            if (bHasCalculations)
            {
                //step 3. Run mach1stock and mach2stock calculations on the synchronized collections
                //note that adjustments must be made at this level for both outputs and tp.amount
                //(or timeliness penalties are wrong)
                int iResetFilePositionIndexTo = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
                List<TimelinessOpComp1> currentTimelinessOCs = SetMachineryOCStockCalculations(ref currentElement, currentCalculationsElement);
                this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iResetFilePositionIndexTo;
                //step 5. replace the old stateful tocs with the new timelinessopcomp 
                bHasCalculations = ReplaceTimelinessOpComps(currentTimelinessOCs);
            }
            return bHasCalculations;
        }
        public bool SetInputMachinery2aStockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set a collection of base machinery1 and machinery2 stocks (uses this.MachStock1 and MachStock2)
            //sets both the base fuel costs in machstock1 and timeliness costs in machstock2
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            bHasCalculations = this.SetInputMachinery2StockJointCalculations(ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            return bHasCalculations;
        }

        private bool ReplaceWithBestMachineryCollections(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //step 3. Run the selection and scheduling calculator, generating an 
            //optimum set of unique machinery combinations
            if (this.GCCalculatorParams.ErrorMessage != string.Empty)
            {
                //don't run calculator if errors are pending, means collections not made correctly
                return false;
            }
            StatsAnalyzers.Mach1SelectSchedule1 calculator = new StatsAnalyzers.Mach1SelectSchedule1();
            if (this.TimelinessBudgetGroup != null)
            {
                if (this.TimelinessBudgetGroup.TimelinessBudgets != null)
                {
                    foreach (var budget in this.TimelinessBudgetGroup.TimelinessBudgets)
                    {
                        if (budget.TimelinessTimePeriods != null)
                        {
                            calculator.RunOptimization(budget.TimelinessTimePeriods);
                            this.GCCalculatorParams.ErrorMessage = calculator.ErrorMessage;
                            if (string.IsNullOrEmpty(calculator.ErrorMessage))
                            {
                                //replace the existing tp.opcomps with best combos
                                budget.TimelinessTimePeriods.Clear();
                                budget.TimelinessTimePeriods = new List<TimelinessTimePeriod1>();
                                foreach (var tp in calculator.BestTimelinessTimePeriods)
                                {
                                    budget.TimelinessTimePeriods.Add(tp);
                                }
                                bHasCalculations = SerializeBestMachinery(budget,
                                    ref currentCalculationsElement, ref currentElement);
                            }
                            else
                            {
                                this.GCCalculatorParams.ErrorMessage += calculator.ErrorMessage;
                                bHasCalculations = false;
                            }
                        }
                        else
                        {
                            this.GCCalculatorParams.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS2A_NOBASESTOCKTIMEPERIODS"); ;
                            return false;
                        }
                    }
                }
            }
            else
            {
                this.GCCalculatorParams.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS2A_NOBASESTOCKBUDGET"); ;
                return false;
            }
            return bHasCalculations;
        }
        private bool ReplaceOCMachineryCollections(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //step 1. Make the PowerInputs and NonPower Lists 
            //add the power inputs for the new maxhp ranges
            //add the nonpower inputs for the new field capacity and fuel ranges
            //this returns true -want those one input combines to be included
            bool bHasNewSizes = AddSizeRanges();
            if (bHasNewSizes)
            {
                //step 2. Remove any powerInput collection member that is below implement.minHP collection
                //note: can't add one when all are too small because don't know list price -need to report the null
                RemoveMinimumHorsepowerConstraint();

                //step 3. Run standard machinery calculations for the new machinery collections
                bHasCalculations = AddPowerInputCalculations(currentCalculationsElement);
                bHasCalculations = AddNonPowerInputCalculations(currentCalculationsElement);

                //step 4. Replace the machinery 1 and 2 stock collections using a feasible list of machinery combinations 
                bHasCalculations = ReplaceMachineryStocks();
            }
            else
            {
                //no error: ok not to have new size ranges and keep old calcs
                //need to run next step for display purposes (i.e. operation/operation is displayed)
                if (this.PowerInputs.Count >= 1 || this.NonPowerInputs.Count >= 1)
                {
                    bHasCalculations = true;
                }
            }
            return bHasCalculations;
        }
        private bool AddSizeRanges()
        {
            bool bHasCompleted = false;
            if (this.MachineryStock.MachineryStocks.Count > 0)
            {
                //step 2. Make the List -add the power inputs for the new hp ranges 
                // -and add the nonpower inputs for the new field capacity and fuel ranges
                //the last member of this.MachineryStock.MachineryStocks are the last inputs added
                //must correspond 100% to this.GCCalculatorParams.AnalyzerParms.FilePositionIndex
                this.StartingFilePositionIdex = this.MachineryStock.MachineryStocks.Keys.Count - 1;
                if (this.MachineryStock.MachineryStocks.ContainsKey(this.StartingFilePositionIdex)
                    && this.StartingFilePositionIdex == this.GCCalculatorParams.AnalyzerParms.FilePositionIndex)
                {
                    List<Machinery1Input> currentMachInputs 
                        = this.MachineryStock.MachineryStocks[this.GCCalculatorParams.AnalyzerParms.FilePositionIndex];
                    //for op/comps init new collections (might want to keep collections for budgets)
                    this.PowerInputs = new List<Machinery1Input>();
                    this.NonPowerInputs = new List<Machinery1Input>();
                    foreach (Machinery1Input machinput in currentMachInputs)
                    {
                        //add the base machinputs to powerinput and nonpowerinput collections
                        if (machinput.FuelCost > 0)
                        {
                            this.PowerInputs.Add(machinput);
                        }
                        else
                        {
                            this.NonPowerInputs.Add(machinput);
                        }
                        //add the size ranges
                        bHasCompleted = AddSizeRanges(this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, machinput);
                    }
                }
            }
            return bHasCompleted;
        }
        private void RemoveMinimumHorsepowerConstraint()
        {
            if (this.PowerInputs.Count > 1)
            {
                List<Machinery1Input> powerInputsToRemove = GetPowerInputsWithInsufficientPower();
                foreach (Machinery1Input powerinput in powerInputsToRemove)
                {
                    this.PowerInputs.Remove(powerinput);
                }
            }
        }
        private List<Machinery1Input> GetPowerInputsWithInsufficientPower()
        {
            List<Machinery1Input> powerInputsToRemove
                = new List<Machinery1Input>();
            foreach (Machinery1Input powerinput in this.PowerInputs)
            {
                bool bIsHigherThanOneNonPowerInputMaxHPPTO = false;
                foreach (Machinery1Input nonpowerinput in this.NonPowerInputs)
                {
                    if (nonpowerinput.Constants.HPPTOMax > powerinput.Constants.HPPTOMax)
                    {
                        bIsHigherThanOneNonPowerInputMaxHPPTO = NonPowerInputs.Any(np => np.Constants.HPPTOMax <= powerinput.Constants.HPPTOMax);
                        if (bIsHigherThanOneNonPowerInputMaxHPPTO == false)
                        {
                            powerInputsToRemove.Add(powerinput);
                        }
                    }
                }
            }
            return powerInputsToRemove;
        }
        private bool AddSizeRanges(int index, Machinery1Input machinput)
        {
            bool bHasCompleted = false;
            if (machinput.Sizes != null)
            {
                //if it is a power input, use the SizeRange property as HP
                if (machinput.FuelCost > 0)
                {
                    bHasCompleted = AddPowerInputs(index, machinput);
                }
                else
                {
                    bHasCompleted = AddNonPowerInputs(index, machinput);
                }
            }
            return bHasCompleted;
        }
        private bool AddPowerInputs(int index, Machinery1Input machinput)
        {
            bool bHasCompleted = false;
            //the stylesheet is the defacto dictionary for storing these correctly
            AddPowerInput(machinput, machinput.Sizes.SizeVarD1, machinput.Sizes.SizePrice1, machinput.Sizes.SizeRange1,
                machinput.Sizes.SizeVarA1, machinput.Sizes.SizeVarB1, machinput.Sizes.SizeVarC1);
            AddPowerInput(machinput, machinput.Sizes.SizeVarD2, machinput.Sizes.SizePrice2, machinput.Sizes.SizeRange2,
                machinput.Sizes.SizeVarA2, machinput.Sizes.SizeVarB2, machinput.Sizes.SizeVarC2);
            AddPowerInput(machinput, machinput.Sizes.SizeVarD3, machinput.Sizes.SizePrice3, machinput.Sizes.SizeRange3,
                machinput.Sizes.SizeVarA3, machinput.Sizes.SizeVarB3, machinput.Sizes.SizeVarC3);
            AddPowerInput(machinput, machinput.Sizes.SizeVarD4, machinput.Sizes.SizePrice4, machinput.Sizes.SizeRange4,
                machinput.Sizes.SizeVarA4, machinput.Sizes.SizeVarB4, machinput.Sizes.SizeVarC4);
            AddPowerInput(machinput, machinput.Sizes.SizeVarD5, machinput.Sizes.SizePrice5, machinput.Sizes.SizeRange5,
                machinput.Sizes.SizeVarA5, machinput.Sizes.SizeVarB5, machinput.Sizes.SizeVarC5);
            if (PowerInputs.Count > 0)
            {
                bHasCompleted = true;
            }
            return bHasCompleted;
        }
        private void AddPowerInput(Machinery1Input machinput, double maxPTOHP,
            double listPrice, double width, double speed, double fieldEfficiency, double equivPTOHP)
        {
            if (maxPTOHP > 0 && listPrice > 0)
            {
                Machinery1Input powerInput = new Machinery1Input(this.GCCalculatorParams, machinput);
                //set the proportions before changing them
                double hpProportion = GetHPProportion(machinput);
                double equivHPProportion = GetEquivHPProportion(machinput);
                powerInput.CAPPrice = listPrice;
                if (maxPTOHP > 0)
                {
                    powerInput.Constants.HPPTOMax = (int)maxPTOHP;
                }
                //width, speed and fieldEff determine field capacity (and timeliness penalties)
                if (width > 0)
                {
                    powerInput.Constants.Width = width;
                }
                if (speed > 0)
                {
                    powerInput.Constants.FieldSpeedTypical = speed;
                }
                if (fieldEfficiency > 0)
                {
                    powerInput.Constants.FieldEffTypical = fieldEfficiency;
                }
                //keep the same hp proportions (for fuel costs, only maxptohp comes from power input, equivpto comes from implement 
                //except when only one power input is used)
                if (hpProportion >= 1)
                {
                    powerInput.Constants.HP = (int)(powerInput.Constants.HPPTOMax * hpProportion);
                }
                else
                {
                    powerInput.Constants.HP = powerInput.Constants.HPPTOMax;
                }
                //set equivhtpto
                SetEquivHTPTO(powerInput, equivPTOHP, equivHPProportion);
                powerInput.Id = CalculatorHelpers.GetRandomInteger(this.RandomId);
                powerInput.CalculatorId = powerInput.Id;
                //tells calculators to swap out input being calculated with this one
                powerInput.Type = CostBenefitCalculator.TYPE_NEWCALCS;
                //don't add the same power input to the collection (note that age is not used yet -this collection inits off one powerint)
                if (!HasPowerInput(powerInput, this.PowerInputs))
                {
                    PowerInputs.Add(powerInput);
                }
            }
        }
        public static bool HasPowerInput(Machinery1Input machInput, List<Machinery1Input> powerInputs)
        {
            //test before adding a nonpower input to collection
            bool bIsInList = powerInputs.Any(np => np.Name == machInput.Name
                && np.Constants.HP == machInput.Constants.HP && np.CAPPrice == machInput.CAPPrice);
            return bIsInList;
        }
       
        private void SetEquivHTPTO(Machinery1Input machinput, double equivPTOHP,
            double equivHPProportion)
        {
            if (equivPTOHP > 0)
            {
                machinput.Constants.HPPTOEquiv = (int)equivPTOHP;
            }
            else
            {
                //use the proportion if its legit
                if (equivHPProportion > 0 && equivHPProportion <= 1)
                {
                    machinput.Constants.HPPTOEquiv = (int)(machinput.Constants.HPPTOMax * equivHPProportion);
                }
                else
                {
                    machinput.Constants.HPPTOEquiv = machinput.Constants.HPPTOMax;
                }
            }
        }
        private bool AddNonPowerInputs(int index, Machinery1Input machinput)
        {
            bool bHasCompleted = false;
            //the stylesheet is the defacto dictionary for storing these correctly
            AddNonPowerInput(machinput, machinput.Sizes.SizeRange1, machinput.Sizes.SizePrice1,
                 machinput.Sizes.SizeVarB1, machinput.Sizes.SizeVarA1,
                 machinput.Sizes.SizeVarC1, machinput.Sizes.SizeVarD1);
            AddNonPowerInput(machinput, machinput.Sizes.SizeRange2, machinput.Sizes.SizePrice2,
                 machinput.Sizes.SizeVarB2, machinput.Sizes.SizeVarA2,
                 machinput.Sizes.SizeVarC2, machinput.Sizes.SizeVarD2);
            AddNonPowerInput(machinput, machinput.Sizes.SizeRange3, machinput.Sizes.SizePrice3,
                 machinput.Sizes.SizeVarB3, machinput.Sizes.SizeVarA3,
                 machinput.Sizes.SizeVarC3, machinput.Sizes.SizeVarD3);
            AddNonPowerInput(machinput, machinput.Sizes.SizeRange4, machinput.Sizes.SizePrice4,
                 machinput.Sizes.SizeVarB4, machinput.Sizes.SizeVarA4,
                 machinput.Sizes.SizeVarC4, machinput.Sizes.SizeVarD4);
            AddNonPowerInput(machinput, machinput.Sizes.SizeRange5, machinput.Sizes.SizePrice5,
                 machinput.Sizes.SizeVarB5, machinput.Sizes.SizeVarA5,
                 machinput.Sizes.SizeVarC5, machinput.Sizes.SizeVarD5);
            if (NonPowerInputs.Count > 0)
            {
                bHasCompleted = true;
            }
            return bHasCompleted;
        }
        private void AddNonPowerInput(Machinery1Input machinput,
            double size, double listPrice, double fieldEfficiency, double speed,
            double equivPTOHP, double maxPTOHP)
        {
            if (size > 0 && listPrice > 0 && fieldEfficiency > 0)
            {
                Machinery1Input nonpowerInput = new Machinery1Input(this.GCCalculatorParams, machinput);
                double equivHPProportion = GetEquivHPProportion(machinput);
                nonpowerInput.CAPPrice = listPrice;
                //width, speed and field efficiency determine field capacity (and timeliness penalty)
                if (size > 0)
                {
                    nonpowerInput.Constants.Width = (int)size;
                }
                if (speed > 0)
                {
                    nonpowerInput.Constants.FieldSpeedTypical = speed;
                }
                if (fieldEfficiency > 0)
                {
                    nonpowerInput.Constants.FieldEffTypical = fieldEfficiency;
                }
                if (maxPTOHP > 0)
                {
                    nonpowerInput.Constants.HPPTOMax = (int)maxPTOHP;
                }
                //set equivhtpto
                SetEquivHTPTO(nonpowerInput, equivPTOHP, equivHPProportion);
                //set id to be unique
                nonpowerInput.Id = CalculatorHelpers.GetRandomInteger(this.RandomId);
                nonpowerInput.CalculatorId = nonpowerInput.Id;
                //tells calculators to swap out input being calculated with this one
                nonpowerInput.Type = CostBenefitCalculator.TYPE_NEWCALCS;
                if (!HasNonPowerInput(nonpowerInput, this.NonPowerInputs))
                {
                    NonPowerInputs.Add(nonpowerInput);
                }
            }
        }
        public static bool HasNonPowerInput(Machinery1Input machInput, List<Machinery1Input> nonpowerInputs)
        {
            //test before adding a nonpower input to collection
            bool bIsInList = nonpowerInputs.Any(np => np.Name == machInput.Name 
                && np.Constants.Width == machInput.Constants.Width && np.CAPPrice == machInput.CAPPrice);
            return bIsInList;
        }
        private double GetHPProportion(Machinery1Input machinput)
        {
            double hp = (double)machinput.Constants.HP;
            double hpPTOMax = (double)machinput.Constants.HPPTOMax;
            double hpProportion = (hpPTOMax != 0)
                    ? (double)(hp / hpPTOMax) : 1;
            return hpProportion;
        }
        private double GetEquivHPProportion(Machinery1Input machinput)
        {
            double hpPTOEquiv = (double)machinput.Constants.HPPTOEquiv;
            double hpPTOMax = (double)machinput.Constants.HPPTOMax;
            double hpProportion = (hpPTOMax != 0)
                    ? (double)(hpPTOEquiv / hpPTOMax) : 1;
            return hpProportion;
        }
        private bool AddPowerInputCalculations(XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            string sErrorMsg = string.Empty;
            if (this.PowerInputs.Count > 0)
            {
                foreach (Machinery1Input powerinput in this.PowerInputs)
                {
                    XElement newCalcsElement = new XElement(currentCalculationsElement);
                    //run the new calculations with the new field capacity properties
                    bool bHasThisCalculations = Machinery1InputCalculator.SetAgMachineryCalculations(
                        this.GCCalculatorParams, AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery,
                        powerinput, newCalcsElement, ref sErrorMsg);
                }
            }
            return bHasCalculations;
        }
        private bool AddNonPowerInputCalculations(XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            string sErrorMsg = string.Empty;
            if (this.NonPowerInputs.Count > 0)
            {
                foreach (Machinery1Input nonpowerinput in this.NonPowerInputs)
                {
                    XElement newCalcsElement = new XElement(currentCalculationsElement);
                    //run the new calculations with the new field capacity properties
                    bool bHasThisCalculations = Machinery1InputCalculator.SetAgMachineryCalculations(
                        this.GCCalculatorParams, AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery,
                        nonpowerinput, newCalcsElement, ref sErrorMsg);
                }
            }
            return bHasCalculations;
        }
        private bool ReplaceMachineryStocks()
        {
            bool bHasCompleted = false;
            int iNodePosition = 0;
            //init toc
            TimelinessOpComp1 toc = GetLastTimelinessToc();
            //init the machinery stock collections that are being processed in preparation 
            //for adding the new collections
            this.MachineryStock.MachineryStocks[this.GCCalculatorParams.AnalyzerParms.FilePositionIndex]
                = new List<Machinery1Input>();
            this.Machinery2Stock.Machinery2Stocks[this.GCCalculatorParams.AnalyzerParms.FilePositionIndex]
               = new List<TimelinessOpComp1>();
            //make feasible combinations
            if (this.NonPowerInputs.Count > 0)
            {
                foreach (Machinery1Input nonpowerinput in this.NonPowerInputs)
                {
                    int i = 0;
                    int iHPUpperRange = 16;
                    List<Machinery1Input> powerInputs = new List<Machinery1Input>();
                    //+- 16 hp is close enough
                    for (i = 0; i < iHPUpperRange; i++)
                    {
                        //first pass is to get a matching hp tractor +- 9 HP
                        powerInputs = FindMatchingPowerInputs(i, nonpowerinput);
                        if (powerInputs.Count == 1)
                        {
                            break;
                        }
                    }
                    if (this.PowerInputs.Count > 1)
                    {
                        for (i = 0; i < iHPUpperRange; i++)
                        {
                            //second pass is to add another matching hp tractor +- 9 HP, 
                            //but with a different market value (new vs. used tractors)
                            AddMatchingPowerInputs(powerInputs, i, nonpowerinput);
                            if (powerInputs.Count == 2)
                            {
                                break;
                            }
                        }
                    }
                    if (powerInputs != null)
                    {
                        if (powerInputs.Count == 0)
                        {
                            //couldn't find a match (meaning nonpowermaxpto is probably set wrong)
                            if (this.PowerInputs.Count > 0)
                            {
                                //the first member added is always the existing one in base opcomp
                                powerInputs.Add(this.PowerInputs[0]);
                            }
                        }
                        foreach (var powerinput in powerInputs)
                        {
                            //add to mach1 stocks
                            iNodePosition = 0;
                            this.MachineryStock.AddMachinery1StocksToDictionary(this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, 
                                iNodePosition, powerinput);
                            iNodePosition = 1;
                            this.MachineryStock.AddMachinery1StocksToDictionary(this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, 
                                iNodePosition, nonpowerinput);
                            //add initNPVOpComp1 to mach2 stocks, keeping file position indices synched
                            iNodePosition = 0;
                            this.Machinery2Stock.AddMachinery2StocksToDictionary(this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, 
                                iNodePosition, toc);
                            //set the base file position index here (don't allow base objects to change it)
                            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex++;
                        }
                    }
                }
            }
            else
            {
                //combines, other ops, can be single machinery
                foreach (Machinery1Input powerinput in this.PowerInputs)
                {
                    iNodePosition = 0;
                    this.MachineryStock.AddMachinery1StocksToDictionary(this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, 
                        iNodePosition, powerinput);
                    //add initNPVOpComp1 to mach2 stocks, keeping file position indices synched
                    iNodePosition = 0;
                    this.Machinery2Stock.AddMachinery2StocksToDictionary(this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, 
                        iNodePosition, toc);
                    //set the base file position index here (don't allow base objects to change it)
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex++;
                }
            }
            if (this.MachineryStock.MachineryStocks.Count > 0)
            {
                bHasCompleted = true;
            }
            return bHasCompleted;
        }
        private TimelinessOpComp1 GetLastTimelinessToc()
        {
            TimelinessOpComp1 toc = new TimelinessOpComp1();
            if (this.TimelinessTimePeriod != null)
            {
                if (this.TimelinessTimePeriod.TimelinessOpComps != null)
                {
                    if (this.TimelinessTimePeriod.TimelinessOpComps.Count > 0)
                    {
                        //the last member added is the current opcomp being processed
                        toc = new TimelinessOpComp1(this.TimelinessTimePeriod.TimelinessOpComps.LastOrDefault());
                        toc.CopyCalculatorProperties(this.TimelinessTimePeriod.TimelinessOpComps.LastOrDefault());
                    }
                }
            }
            return toc;
        }
        private List<Machinery1Input> FindMatchingPowerInputs(int hpIncrement, Machinery1Input nonPowerInput)
        {
            List<Machinery1Input> powerInputs = new List<Machinery1Input>();
            foreach (Machinery1Input powerinput in this.PowerInputs)
            {
                if (powerInputs.Count == 0)
                {
                    //163 adjustment
                    if (powerinput.Constants.HPPTOMax <= (nonPowerInput.Constants.HPPTOMax + hpIncrement)
                        && powerinput.Constants.HPPTOMax >= (nonPowerInput.Constants.HPPTOMax - hpIncrement))
                    {
                        powerInputs.Add(powerinput);
                    }
                    //if (powerinput.Constants.HPPTOMax == (nonPowerInput.Constants.HPPTOMax + hpIncrement))
                    //{
                    //    powerInputs.Add(powerinput);
                    //}
                    //else if (powerinput.Constants.HPPTOMax == (nonPowerInput.Constants.HPPTOMax - hpIncrement))
                    //{
                    //    powerInputs.Add(powerinput);
                    //}
                }
                else
                {
                    break;
                }
            }
            return powerInputs;
        }
        private void AddMatchingPowerInputs(List<Machinery1Input> selectedPowerInputs, int hpIncrement, Machinery1Input nonPowerInput)
        {
            foreach (Machinery1Input powerinput in this.PowerInputs)
            {
                if (selectedPowerInputs.Count == 1)
                {
                    //163 adjustment
                    if (powerinput.Constants.HPPTOMax <= (nonPowerInput.Constants.HPPTOMax + hpIncrement)
                        && powerinput.Constants.HPPTOMax >= (nonPowerInput.Constants.HPPTOMax - hpIncrement))
                    {
                        //don't include the same selection
                        if (powerinput.Id != selectedPowerInputs[0].Id)
                        {
                            //include only one with a different market value 
                            if (powerinput.MarketValue != selectedPowerInputs[0].MarketValue)
                            {
                                selectedPowerInputs.Add(powerinput);
                            }
                        }
                    }
                    //if (powerinput.Constants.HPPTOMax == (nonPowerInput.Constants.HPPTOMax + hpIncrement))
                    //{
                    //    //don't include the same selection
                    //    if (powerinput.Id != selectedPowerInputs[0].Id)
                    //    {
                    //        //include only one with a different market value 
                    //        if (powerinput.MarketValue != selectedPowerInputs[0].MarketValue)
                    //        {
                    //            selectedPowerInputs.Add(powerinput);
                    //        }
                    //    }
                    //}
                    //else if (powerinput.Constants.HPPTOMax == (nonPowerInput.Constants.HPPTOMax - hpIncrement))
                    //{
                    //    //don't include the same selection
                    //    if (powerinput.Id != selectedPowerInputs[0].Id)
                    //    {
                    //        //include only one with a different market value 
                    //        if (powerinput.MarketValue != selectedPowerInputs[0].MarketValue)
                    //        {
                    //            selectedPowerInputs.Add(powerinput);
                    //        }
                    //    }
                    //}
                }
                else
                {
                    break;
                }
            }
        }
        private List<TimelinessOpComp1> SetMachineryOCStockCalculations(ref XElement currentElement, XElement currentCalculationsElement)
        {
            //optimization collection (filled when timeliness penalties are computed)
            List<TimelinessOpComp1> currentTimelinessOCs = new List<TimelinessOpComp1>();
            //the xml that will be displayed has to be built here
            if (this.MachineryStock.MachineryStocks.Count > 0)
            {
                OCCalculator ocCalculator = new OCCalculator();
                int i = 0;
                foreach (KeyValuePair<int, List<Machinery1Input>> kvp
                    in this.MachineryStock.MachineryStocks)
                {
                    //process only new machinput collections
                    if (kvp.Key >= this.StartingFilePositionIdex)
                    {
                        if (kvp.Value != null)
                        {
                            if (kvp.Value.Count > 0)
                            {
                                //keep the underlying stock calculators insynch with this position
                                //this.SetOpOrCompMachineryStockCalculations needs to know which one to calculate
                                this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = kvp.Key;
                                //set the joint input calculations
                                ocCalculator.SetJointInputCalculations(kvp.Value);
                                //set the opcomp mach1totals using new opcomp elements
                                XElement opCompElement = GetNewOpCompElement(currentElement);
                                if (opCompElement != null)
                                {
                                    XElement opCompCalculation = new XElement(currentCalculationsElement);
                                    //set the input mach1 attributes
                                    i = 0;
                                    double dbMultiplier = 1;
                                    //need new totals for these machinputs
                                    Machinery1Stock mach1Stock = new Machinery1Stock();
                                    TimelinessOpComp1 currentTimelinessOC = new TimelinessOpComp1();
                                    currentTimelinessOC.Inputs = new List<Input>();
                                    //container for holding inputs
                                    XElement rootInputs = new XElement(Constants.ROOT_PATH);
                                    foreach (Machinery1Input machinput in kvp.Value)
                                    {
                                        if (i == 0)
                                        {
                                            //set the totals for the timeliness costs to opcomp
                                            //and add to this.TimelinessOpComp.IimelinessOpComps
                                            currentTimelinessOC = GetNewTimelinessOpComp(
                                                kvp.Key, machinput, ref opCompCalculation);
                                            i++;
                                        }
                                        //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                                        dbMultiplier = GetInputFullCostMultiplier(this.Mach1Input, 
                                            this.GCCalculatorParams);
                                        //multiply machinput costs by multiplier and add it new machstock collection
                                        bool bAdjustTotals = true;
                                        BIMachineryStockCalculator.AddMachineryInputToStock(mach1Stock,
                                            dbMultiplier, machinput, Input.INPUT_PRICE_TYPES.input.ToString(), bAdjustTotals);
                                        //ChangeMachineryInputByInputMultipliers(machinput, dbMultiplier);
                                        //AddMachineryInputToStock(mach1Stock, currentTimelinessOC.Amount, machinput);
                                        //add the input with linked view to opcomp
                                        XElement inputElement = GetNewInputElement(currentElement.Name.LocalName);
                                        if (inputElement != null)
                                        {
                                            XElement inputCalculation = new XElement(Constants.LINKEDVIEWS_TYPES.linkedview.ToString());
                                            //set basic name and id attributes 
                                            Input.SetInputAllAttributes(machinput, ref inputElement);
                                            //set new machinery stock totals in the calculator
                                            machinput.SetMachinery1InputAttributes(this.GCCalculatorParams,
                                                ref inputCalculation, ref inputElement);
                                            //set the totaloc and totalaoh
                                            BIMachineryStockCalculator.AddMachinery1InputTotals(machinput, 
                                                ref inputCalculation);
                                            machinput.SetIdAndNameAttributes(ref inputCalculation);
                                            //set calculatorid (primary way to display calculation attributes)
                                            CalculatorHelpers.SetCalculatorId(inputCalculation, inputElement);
                                            //bi subapps don't build xml until after optimization run, 
                                            //but useful to have this xml later
                                            XElement root1 = new XElement(Constants.ROOT_PATH);
                                            root1.Add(inputCalculation);
                                            inputElement.Add(root1);
                                            //add to object
                                            machinput.XmlDocElement = new XElement(inputElement);
                                            //add to inputs container
                                            rootInputs.Add(inputElement);
                                            //add the machinput to currentTimelinessOpComp
                                            currentTimelinessOC.Inputs.Add(machinput);
                                        }
                                    }
                                    //the new base mach2stock collection can be used to set the 
                                    //base machinery costs: TotalToc and TotalAOH
                                    //BIMachinery2StockCalculator.AddStock1TotalsToElement(mach1Stock, currentTimelinessOC,
                                    //    ref opCompCalculation);
                                    //bi subapps don't build xml until after optimization run
                                    //but useful to have this xml later
                                    //add the calculations in the linkedview
                                    //add machstock1, holding new base mach totals to opCompCalculation
                                    mach1Stock.SetTotalMachinery1StockAttributes(string.Empty,
                                        ref opCompCalculation);
                                    mach1Stock.SetTotalMachinery1ConstantAttributes(string.Empty,
                                        ref opCompCalculation);
                                    AddMachineryStockTotals(mach1Stock, ref opCompCalculation);
                                    //set the totals
                                    currentTimelinessOC.TotalOC = mach1Stock.TotalOC;
                                    currentTimelinessOC.TotalAOH = mach1Stock.TotalAOH;
                                    XElement root2 = new XElement(Constants.ROOT_PATH);
                                    root2.Add(opCompCalculation);
                                    opCompElement.Add(root2);
                                    //add the inputs container elements
                                    opCompElement.Add(rootInputs.Elements());
                                    //make sure it has a unique it to distinguish from siblings
                                    CalculatorHelpers.SetAttributeInt(opCompElement, Calculator1.cId, currentTimelinessOC.Id);
                                    //set calculatorid (primary way to display calculation attributes)
                                    CalculatorHelpers.SetCalculatorId(opCompCalculation, opCompElement);
                                    //add the new opcomp as child of current opcomp (stylsheet will handle display)
                                    //note that this is now <op><op> 
                                    currentElement.Add(opCompElement);
                                    //add to the optimization collections
                                    if (currentTimelinessOC != null)
                                    {
                                        //watch the impact on memory
                                        //add xml to object (can use the regular currentElement, currentCalcsEl 
                                        //pattern to serialize optimum machinery combos)
                                        currentTimelinessOC.XmlDocElement = new XElement(currentElement);
                                        //currenttoc has the collection of machinputs, so full info on hand
                                        currentTimelinessOCs.Add(currentTimelinessOC);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return currentTimelinessOCs;
        }
        private bool ReplaceTimelinessOpComps(List<TimelinessOpComp1> currentTimelinessOCs)
        {
            bool bHasReplaced = false;
            if (this.TimelinessTimePeriod != null && currentTimelinessOCs != null)
            {
                //move the last toc added during SetInitialOpCompStocks()
                if (this.TimelinessTimePeriod.TimelinessOpComps != null)
                {
                    if (this.TimelinessTimePeriod.TimelinessOpComps.Count > 0
                        && currentTimelinessOCs.Count > 0)
                    {
                        //clear out the old tocls (note that .last doesn't clear out last)
                        int iLastIndex = this.TimelinessTimePeriod.TimelinessOpComps.Count - 1;
                        TimelinessOpComp1 tocCheck = this.TimelinessTimePeriod.TimelinessOpComps.ElementAt(iLastIndex);
                        //check for nulls
                        if (tocCheck != null)
                        {
                            if (tocCheck.TimelinessOpComps == null)
                            {
                                this.TimelinessTimePeriod.TimelinessOpComps.ElementAt(iLastIndex).TimelinessOpComps = new List<TimelinessOpComp1>();
                            }
                            this.TimelinessTimePeriod.TimelinessOpComps.ElementAt(iLastIndex).TimelinessOpComps.Clear();
                            //add the new ones
                            foreach (var foc in currentTimelinessOCs)
                            {
                                this.TimelinessTimePeriod.TimelinessOpComps.ElementAt(iLastIndex).TimelinessOpComps.Add(foc);
                                bHasReplaced = true;
                            }
                        }
                    }
                }
            }
            else
            {
                //ok to have zero count collections but not null (null means they were never initiated)
                this.GCCalculatorParams.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS2A_NOTIMELINESSOBJECT"); ;
                return false;
            }
            return bHasReplaced;
        }
        private XElement GetNewInputElement(string currentNodeName)
        {
            XElement inputElement = null;
            if (currentNodeName == OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
            {
                inputElement = new XElement(OperationComponent.OPERATION_PRICE_TYPES.operationinput.ToString());
            }
            else if (currentNodeName == OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
            {
                inputElement = new XElement(OperationComponent.COMPONENT_PRICE_TYPES.componentinput.ToString());
            }
            else if (currentNodeName == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString())
            {
                inputElement = new XElement(BudgetInvestment.BUDGET_TYPES.budgetinput.ToString());
            }
            else if (currentNodeName == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                inputElement = new XElement(BudgetInvestment.INVESTMENT_TYPES.investmentinput.ToString());
            }
            return inputElement;
        }
        private XElement GetNewOpCompElement(XElement currentOpCompElement)
        {
            XElement opCompElement = new XElement(currentOpCompElement);
            //get rid of the children linkedviews and children inputs
            opCompElement.RemoveNodes();
            return opCompElement;
        }
        private TimelinessOpComp1 GetNewTimelinessOpComp(int key, Machinery1Input machInput, 
            ref XElement opCompCalculation)
        {
            //must make a new toc, not a reference to existing machstock member
            TimelinessOpComp1 newTimelinessOC = new TimelinessOpComp1();
            //add the timeliness attributes to opCompCalculation
            if (this.Machinery2Stock.Machinery2Stocks.ContainsKey(key))
            {
                TimelinessOpComp1 oldTimelinessOC
                    = this.Machinery2Stock.Machinery2Stocks[key][0];
                if (oldTimelinessOC != null)
                {
                    newTimelinessOC = new TimelinessOpComp1(oldTimelinessOC);
                    newTimelinessOC.CopyCalculatorProperties(oldTimelinessOC);
                    newTimelinessOC.CopyTimelinessOC1Properties(oldTimelinessOC);
                    newTimelinessOC.CopyTotalCostsProperties(oldTimelinessOC);
                    //reset the outputs and rerun the timeliness cost using the outputs
                    //the base oc already added the tpamount, so don't double count (or 950 x 950 in calcs)
                    double dbTPAmount = 1;
                    newTimelinessOC.ReSetParentTimePeriodProperties(machInput.OCAmount,
                        newTimelinessOC.PlannedStartDate, dbTPAmount, this.TimelinessTimePeriod.Outcomes);
                    newTimelinessOC.SetTimelinessOC1Attributes(string.Empty,
                        opCompCalculation);
                    //need to display some base atts with calculated results
                    newTimelinessOC.SetTimelinessBaseAttributes(string.Empty, opCompCalculation);
                    //currentTOC needs a unique Id so that its unique cost can be referenced
                    //(but the CalculatorId should equal parentTOC.Id)
                    newTimelinessOC.Id = CalculatorHelpers.GetRandomInteger(this.RandomId);
                }
            }
            newTimelinessOC.Inputs = new List<Input>();
            return newTimelinessOC;
        }
        
        
        private bool SerializeOpCompGroupMachinery(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasAddedMachTotalsToCalculator = false;
            //the feasible collections were added from machstocks to:
            if (this.TimelinessTimePeriod != null)
            {
                if (this.TimelinessTimePeriod.TimelinessOpComps != null)
                {
                     //build a new mach1stock totals
                    Machinery1Stock tpMach1Stock = new Machinery1Stock();
                    //build a new mach2stock totals
                    Machinery2Stock tpMach2Stock = new Machinery2Stock();
                    //all multipliers were already calculated
                    double dbMultiplier = 1;
                    foreach (var uoc in this.TimelinessTimePeriod.TimelinessOpComps)
                    {
                        if (uoc.TimelinessOpComps != null)
                        {
                            foreach (var foc in uoc.TimelinessOpComps)
                            {
                                //accumulate mach2stock totals
                                BIMachinery2StockCalculator.AddTimelinessOpComp1ToStock(
                                    tpMach2Stock, dbMultiplier, foc);
                                if (foc.Inputs != null)
                                {
                                    foreach (Machinery1Input machinput in foc.Inputs)
                                    {
                                        if (machinput != null)
                                        {
                                            bool bAdjustTotals = true;
                                            //accumulate mach1stock totals
                                            BIMachineryStockCalculator.AddMachineryInputToStock(
                                                tpMach1Stock, dbMultiplier, machinput,
                                                Input.INPUT_PRICE_TYPES.input.ToString(), bAdjustTotals);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //serialize both totals to currentcalculator
                    if (currentCalculationsElement != null)
                    {
                        string sAttNameExtension = string.Empty;
                        //serialize
                        tpMach2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension,
                            ref currentCalculationsElement);
                        tpMach1Stock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                            ref currentCalculationsElement);
                        tpMach1Stock.SetTotalMachinery1ConstantAttributes(string.Empty,
                            ref currentCalculationsElement);
                        AddMachineryStockTotals(tpMach1Stock, ref currentCalculationsElement);
                        //set calculatorid (primary way to display calculation attributes)
                        CalculatorHelpers.SetCalculatorId(currentCalculationsElement, currentElement);
                        bHasAddedMachTotalsToCalculator = true;
                    }
                }
            }
            return bHasAddedMachTotalsToCalculator;
        }
        private bool SerializeBIGroupMachinery(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasAddedMachTotalsToCalculator = false;
            //the feasible collections were added from machstocks to:
            if (this.TimelinessBudgetGroup != null)
            {
                //build a new mach1stock totals
                Machinery1Stock bigMach1Stock = new Machinery1Stock();
                //build a new mach2stock totals
                Machinery2Stock bigMach2Stock = new Machinery2Stock();
                if (this.TimelinessBudgetGroup.TimelinessBudgets != null)
                {
                    foreach(var boc in this.TimelinessBudgetGroup.TimelinessBudgets)
                    {
                        if (boc.TimelinessTimePeriods != null)
                        {
                            foreach (var tpoc in boc.TimelinessTimePeriods)
                            {
                                if (tpoc.TimelinessOpComps != null)
                                {

                                    //all multipliers were already calculated
                                    double dbMultiplier = 1;
                                    foreach (var uoc in tpoc.TimelinessOpComps)
                                    {
                                        if (uoc.TimelinessOpComps != null)
                                        {
                                            foreach (var foc in uoc.TimelinessOpComps)
                                            {
                                                //accumulate mach2stock totals
                                                BIMachinery2StockCalculator.AddTimelinessOpComp1ToStock(bigMach2Stock, dbMultiplier, foc);
                                                if (foc.Inputs != null)
                                                {
                                                    foreach (Machinery1Input machinput in foc.Inputs)
                                                    {
                                                        if (machinput != null)
                                                        {
                                                            //accumulate mach1stock totals
                                                            bool bAdjustTotals = true;
                                                            BIMachineryStockCalculator.AddMachineryInputToStock(
                                                                bigMach1Stock, dbMultiplier, machinput,
                                                                Input.INPUT_PRICE_TYPES.input.ToString(), bAdjustTotals);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //serialize both totals to currentcalculator
                if (currentCalculationsElement != null)
                {
                    string sAttNameExtension = string.Empty;
                    //serialize
                    bigMach2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    bigMach1Stock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    bigMach1Stock.SetTotalMachinery1ConstantAttributes(string.Empty,
                        ref currentCalculationsElement);
                    AddMachineryStockTotals(bigMach1Stock, ref currentCalculationsElement);
                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(currentCalculationsElement, currentElement);
                    bHasAddedMachTotalsToCalculator = true;
                }
            }
            return bHasAddedMachTotalsToCalculator;
        }
        private bool SerializeBestMachinery(TimelinessBI1 timelinessBudget,
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasSerializedAllBests = false;
            //Step 3. Serialize this.TimelinessBudgetGroup.TBs to xml for display
            if (currentElement.HasElements)
            {
                //build a new mach1stock totals for budget/investment node
                Machinery1Stock biMach1Stock = new Machinery1Stock();
                //build a new mach2stock totals for budget/investment node
                Machinery2Stock biMach2Stock = new Machinery2Stock();
                foreach (XElement tpElement in currentElement.Elements())
                {
                    if (tpElement.Name.LocalName
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || tpElement.Name.LocalName
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        if (tpElement.HasElements)
                        {
                            foreach (XElement groupingElement in tpElement.Elements())
                            {
                                if (groupingElement.Name.LocalName
                                    == BudgetInvestment.BUDGET_TYPES.budgetoperations.ToString()
                                    || groupingElement.Name.LocalName
                                    == BudgetInvestment.INVESTMENT_TYPES.investmentcomponents.ToString())
                                {
                                    int iTpId = CalculatorHelpers.GetAttributeInt(tpElement, Calculator1.cId);
                                    foreach (var tp in timelinessBudget.TimelinessTimePeriods)
                                    {
                                        if (tp.Id == iTpId)
                                        {
                                            if (tp.TimelinessOpComps != null)
                                            {
                                                //clear out current children
                                                groupingElement.RemoveAll();
                                                //build a new mach1stock totals
                                                Machinery1Stock tpMach1Stock = new Machinery1Stock();
                                                //build a new mach2stock totals
                                                Machinery2Stock tpMach2Stock = new Machinery2Stock();
                                                bHasSerializedAllBests = SetBestMachineryOpCompCalculations(tp,
                                                    groupingElement, tpMach1Stock, tpMach2Stock);
                                                //nothing to serialize without the best opcomp collection
                                                bHasSerializedAllBests = SetBestMachineryTimePeriodCalculations(tp, tpElement,
                                                    tpMach1Stock, tpMach2Stock);
                                                //add the tp totals to bi totals
                                                SetBestMachineryBICalculations(biMach1Stock, biMach2Stock, 
                                                    tpMach1Stock, tpMach2Stock);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (bHasSerializedAllBests)
                {
                    //note that running calcs at tp level only
                    //may result in a null currentcalcs element
                    if (currentCalculationsElement != null)
                    {
                        string sAttNameExtension = string.Empty;
                        //serialize
                        biMach2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension,
                            ref currentCalculationsElement);
                        biMach1Stock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                            ref currentCalculationsElement);
                        biMach1Stock.SetTotalMachinery1ConstantAttributes(string.Empty,
                            ref currentCalculationsElement);
                        AddMachineryStockTotals(biMach1Stock, ref currentCalculationsElement);
                        //set the totals
                        timelinessBudget.TotalOC = biMach1Stock.TotalOC;
                        timelinessBudget.TotalAOH = biMach1Stock.TotalAOH;
                        //timelinessBudget.TotalOC = biMach1Stock.TotalFuelCost + biMach1Stock.TotalLaborCost
                        //    + biMach1Stock.TotalRepairCost + biMach1Stock.TotalLubeOilCost;
                        //timelinessBudget.TotalAOH = biMach1Stock.TotalCapitalRecoveryCost
                        //    + biMach1Stock.TotalTaxesHousingInsuranceCost;
                        //CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                        //    CostBenefitCalculator.TOC, timelinessBudget.TotalOC);
                        //CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                        //    CostBenefitCalculator.TAOH, timelinessBudget.TotalAOH);
                    }
                }
            }
            return bHasSerializedAllBests;
        }
        private bool SetBestMachineryOpCompCalculations(TimelinessTimePeriod1 tp, XElement groupingElement,
            Machinery1Stock tpMach1Stock, Machinery2Stock tpMach2Stock)
        {
            bool bHasSerializedAllBests = false;
            //go through uniques
            int iBests = 0;
            //multipliers already were used in base calculations
            //don't redo any base calculation (just sum the new machstocks)
            double dbMultiplier = 1;
            foreach (var uoc in tp.TimelinessOpComps)
            {
                if (uoc.TimelinessOpComps != null)
                {
                    //feasibles are now the best 
                    foreach (var foc in uoc.TimelinessOpComps)
                    {
                        //foc.XmlDocElement is <budgetop<budgetop> structure
                        //need the child budget op that represents 'best'
                        //it will hold correct children inputs
                        if (foc.XmlDocElement != null)
                        {
                            if (foc.XmlDocElement.Name.LocalName
                                == BudgetInvestment.BUDGET_TYPES.budgetoperation.ToString()
                                || foc.XmlDocElement.Name.LocalName
                                == BudgetInvestment.INVESTMENT_TYPES.investmentcomponent.ToString())
                            {
                                //this holds the inputs so no work on them are needed
                                XElement bestOpCompElement = CalculatorHelpers.GetElement(foc.XmlDocElement,
                                    foc.XmlDocElement.Name.LocalName, foc.Id.ToString());
                                if (bestOpCompElement != null)
                                {
                                    //adjustments might have been made to timeliness penalties in 
                                    //optimization calculator but not to input machinery costs
                                    string sCalculatorId = CalculatorHelpers.GetAttribute(bestOpCompElement,
                                        Calculator1.cCalculatorId);
                                    //only one calculator was added in base calcs
                                    //it holds both mach1stock and mach2stock totals
                                    XElement calculator = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                                       bestOpCompElement, Calculator1.cId, sCalculatorId);
                                    if (calculator != null)
                                    {
                                        //the 'best' calcs can only change timeliness properties
                                        //transfer any new ones to calculator
                                        foc.SetTimelinessOC1Attributes(string.Empty, calculator);
                                        foc.SetTimelinessBaseAttributes(string.Empty, calculator);
                                        //note that all calculatorids for displaying were set in original xml
                                        //and shouldn't be reset
                                        CalculatorHelpers.ReplaceOrInsertChildLinkedViewElement(
                                            calculator, ref bestOpCompElement);
                                        groupingElement.Add(bestOpCompElement);
                                        iBests++;
                                        //add them to the tp machinery stocks
                                        //calculator has both mach1stock and mach2stock attributes
                                        Machinery1Stock opCompMach1Stock = new Machinery1Stock();
                                        opCompMach1Stock.SetTotalMachinery1StockProperties(calculator);
                                        opCompMach1Stock.SetTotalMachinery1ConstantProperties(calculator);
                                        //add to tpmach1stock
                                        BIMachineryStockCalculator.AddMachinery1StockToStock(tpMach1Stock,
                                            dbMultiplier, opCompMach1Stock);
                                        //add to tpmach2stock
                                        BIMachinery2StockCalculator.AddTimelinessOpComp1ToStock(
                                            tpMach2Stock, dbMultiplier, foc);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (tp.TimelinessOpComps.Count == iBests)
            {
                //iBests is the counter for the number of best opcomps serialized
                bHasSerializedAllBests = true;
            }
            return bHasSerializedAllBests;
        }
        private bool SetBestMachineryTimePeriodCalculations(TimelinessTimePeriod1 tp, XElement tpElement,
            Machinery1Stock tpMach1Stock, Machinery2Stock tpMach2Stock)
        {
            bool bHasSerializedAllBests = false;
            XElement calculator = null;
            //both stock totals go into the 2a tp calculator
            string sCalculatorId = CalculatorHelpers.GetAttribute(tpElement,
                Calculator1.cCalculatorId);
            if (!string.IsNullOrEmpty(sCalculatorId))
            {
                calculator = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                    tpElement, Calculator1.cCalculatorId, sCalculatorId);
            }
            else
            {
                //tp must have built a base 2a calculator during base calcs
                calculator = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                   tpElement, Calculator1.cAnalyzerType, ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString());
            }
            if (calculator != null)
            {
                //get rid of byref possiblility
                XElement newTPCalculator = new XElement(calculator);
                //serialize both totals to calculator (only totals, don't change ids, names ...)
                string sAttNameExtension = string.Empty;
                //serialize calculator mach2stocks totals
                tpMach2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension, ref newTPCalculator);
                //only summations needed (no multipliers needed)
                //serialize calculator mach1stocks totals
                tpMach1Stock.SetTotalMachinery1StockAttributes(sAttNameExtension,
                    ref newTPCalculator);
                tpMach1Stock.SetTotalMachinery1ConstantAttributes(sAttNameExtension,
                    ref newTPCalculator);
                AddMachineryStockTotals(tpMach1Stock, ref newTPCalculator);
                //set the totals
                tp.TotalOC = tpMach1Stock.TotalOC;
                tp.TotalAOH = tpMach1Stock.TotalAOH;
                //AddStock1TotalsToElement(tpMach1Stock, tp, ref newTPCalculator);
                
                //reset calculatorId
                CalculatorHelpers.SetCalculatorId(newTPCalculator, tpElement);
                //replace calculator
                bHasSerializedAllBests = CalculatorHelpers.ReplaceOrInsertChildLinkedViewElement(
                    newTPCalculator, ref tpElement);
            }
            return bHasSerializedAllBests;
        }
        private static void AddStock1TotalsToElement(Machinery1Stock mach1Stock,
            TimelinessTimePeriod1 currentTP, ref XElement tpCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            //and opcomp mulipliers (amount)
            currentTP.TotalOC = mach1Stock.TotalFuelCost + mach1Stock.TotalLaborCost
                + mach1Stock.TotalRepairCost + mach1Stock.TotalLubeOilCost;
            currentTP.TotalAOH = mach1Stock.TotalCapitalRecoveryCost
                + mach1Stock.TotalTaxesHousingInsuranceCost;
            CalculatorHelpers.SetAttributeDoubleF2(tpCalcElement,
                CostBenefitCalculator.TOC, currentTP.TotalOC);
            CalculatorHelpers.SetAttributeDoubleF2(tpCalcElement,
                CostBenefitCalculator.TAOH, currentTP.TotalAOH);
        }
        private bool SetBestMachineryBICalculations(
            Machinery1Stock biMach1Stock, Machinery2Stock biMach2Stock,
            Machinery1Stock tpMach1Stock, Machinery2Stock tpMach2Stock)
        {
            bool bHasSerializedAllBests = false;
            //no multipliers needed
            double dbMultiplier = 1;
            //add the tpmach1stock to the bimach1stock
            BIMachineryStockCalculator.AddMachinery1StockToStock(biMach1Stock,
                dbMultiplier, tpMach1Stock);
            //add the tpmach2stock to the bimach2stock
            BIMachinery2StockCalculator.AddMachinery2StockToStock(biMach2Stock,
                dbMultiplier, tpMach2Stock);
            return bHasSerializedAllBests;
        }
    }
}
