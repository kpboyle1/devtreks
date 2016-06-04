using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run food nutrition stock calculations for operating 
    ///             and capital budgets
    ///Author:		www.devtreks.org
    ///Date:		2013, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. Garbage properties eliminated in favor of newer patterns
    /// </summary>
    public class BIFNSR01StockCalculator : BudgetInvestmentCalculator
    {
        public BIFNSR01StockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set machstock
            Init();
        }
        public BIFNSR01StockCalculator()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            //collections of calculated food nutrition
            this.FNSR01Stock = new FNSR01Stock();
            this.FNSR01Stock.FoodNutritionStocks
                = new Dictionary<int, List<FNSR01Calculator>>();
            //init indexing
            this.TimePeriodStartingFileIndex = 0;
            this.BudgetStartingFileIndex = 0;
        }

        //constants used by this calculator
        public const string ZERO = "0";

        //stateful food nutrition stock
        public FNSR01Stock FNSR01Stock { get; set; }

        //these objects hold collections of descendants for optimizing total costs
        public FNSR01Calculator FoodSR1Input { get; set; }

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

        public bool AddStock1CalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //input food nutrition1 facts are aggregated by each ancestor
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetTotalFNSR01StockCalculations(
                   ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIFNSR01StockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodFNSR01StockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                //outputs are not used in this analyzer
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                bHasCalculations = SetOpOrCompFNSR01StockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetInputFNSR01StockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }

        public bool SetTotalFNSR01StockCalculations(ref XElement currentCalculationsElement,
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
            //init foodStock props
            this.FNSR01Stock.SetCalculatorProperties(currentCalculationsElement);
            //the foodStock.machstocks dictionary can now be summed to derive totals
            //bimachstockcalculator handles calculations
            double dbMultiplier = 1;
            //set the food nutrition stock totals from foodStock collection
            bHasCalculations = SetTotalFNSR01StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNSR01Stock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNSR01Stock.SetTotalFNSR01StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNSR01StockTotals(this.FNSR01Stock, ref currentCalculationsElement);
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
        public bool SetBIFNSR01StockCalculations(ref XElement currentCalculationsElement,
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
            //init foodStock props
            this.FNSR01Stock.SetCalculatorProperties(currentCalculationsElement);
            //the foodStock.machstocks dictionary can now be summed to derive totals
            double dbMultiplier = 1;
            //set the food nutrition stock totals from foodStock collection
            bHasCalculations = SetTotalFNSR01StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNSR01Stock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNSR01Stock.SetTotalFNSR01StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNSR01StockTotals(this.FNSR01Stock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.FNSR01Stock.InitTotalFNSR01StockProperties();
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
        public bool SetTimePeriodFNSR01StockCalculations(ref XElement currentCalculationsElement,
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
            //note that the foodNutritionInput calculator can not change TimePeriod properties
            //but needs properties from the TimePeriod (i.e. Amount)
            this.TimePeriod.SetTimePeriodProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init foodStock props
            this.FNSR01Stock.SetCalculatorProperties(currentCalculationsElement);
            //don't double count the tp multiplier -each op comp already used it to set penalties
            double dbMultiplier = 1;
            //the foodStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalFNSR01StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNSR01Stock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNSR01Stock.SetTotalFNSR01StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNSR01StockTotals(this.FNSR01Stock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next tp)
            this.FNSR01Stock.InitTotalFNSR01StockProperties();
            //reset foodStock.machstocks fileposition index
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

        public bool SetOpOrCompFNSR01StockCalculations(ref XElement currentCalculationsElement,
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
            //note that the foodNutritionInput calculator can not change Operation properties
            //but needs several properties from the Operation (i.e. Id, Amount)
            this.OpComp.SetOperationComponentProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init foodStock props
            this.FNSR01Stock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the foodStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalFNSR01StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNSR01Stock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNSR01Stock.SetTotalFNSR01StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNSR01StockTotals(this.FNSR01Stock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.FNSR01Stock.InitTotalFNSR01StockProperties();
            //reset foodStock.machstocks fileposition index
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
        public bool SetInputFNSR01StockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of foodNutritionInputs
            if (this.OpComp == null)
            {
                this.OpComp = new OperationComponent();
            }
            if (currentCalculationsElement != null)
            {
                //note that the foodNutritionInput calculator can not change Input properties
                //when running from opOrComps or budgets
                //but needs several properties from the Input (i.e. Id, Times)
                this.FoodSR1Input = new FNSR01Calculator();
                //deserialize xml to object
                this.FoodSR1Input.SetFNSR01Properties(this.GCCalculatorParams,
                   currentCalculationsElement, currentElement);
                //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                double dbMultiplier = GetInputFullCostMultiplier(this.FoodSR1Input, this.GCCalculatorParams);
                //change fuel cost, repair cost, by input.times * input.ocamount or input.aohamount
                ChangeFNInputByInputMultipliers(this.FoodSR1Input, dbMultiplier);
                //serialize calculator object back to xml
                //(calculator doesn't change opOrComp, so don't serialize it)
                string sAttNameExtension = string.Empty;
                //set new foodfact input totals
                this.FoodSR1Input.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                this.FoodSR1Input.SetNewInputAttributes(this.GCCalculatorParams, ref currentCalculationsElement);
                this.FoodSR1Input.SetFNSR01Attributes(sAttNameExtension,
                    ref currentCalculationsElement);
                //set the totaloc and totalaoh
                AddFNSR01CalculatorTotals(this.FoodSR1Input, ref currentCalculationsElement);
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the foodNutritionInput to the foodStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.FNSR01Stock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.FoodSR1Input);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.FNSR01Stock
                    .AddFNSR01StocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.FoodSR1Input);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                //note that foodNutritionInput can be retrieved by converting the input to the 
                //FNSR01Calculator type (foodNutritionInput = (FNSR01Calculator) input)
                if (this.FoodSR1Input != null)
                {
                    this.OpComp.Inputs.Add(this.FoodSR1Input);
                }
                bHasCalculations = true;
            }
            return bHasCalculations;
        }

        public bool SetTotalFNSR01StockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.FNSR01Stock.FoodNutritionStocks != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<FNSR01Calculator>> kvp
                    in this.FNSR01Stock.FoodNutritionStocks)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (FNSR01Calculator foodNutritionInput in kvp.Value)
                        {
                            bool bAdjustTotals = true;
                            bHasCalculations
                                = AddFNInputToStock(multiplier, foodNutritionInput, currentNodeName, bAdjustTotals);
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

        public bool AddFNInputToStock(double multiplier, FNSR01Calculator foodNutritionInput,
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
                    //i.e. foodNutritionInput.cost = input.times * input.ocamount
                    ChangeFNInputByInputMultipliers(foodNutritionInput, multiplier);
                }
                else
                {
                    //i.e. budget adjustment = if operation multiplier = op.amount, 
                    //if timeperiod multiplier = tp.amount)
                    ChangeFNInputByMultiplier(foodNutritionInput, multiplier);
                }
            }
            this.FNSR01Stock.TotalMarketValue += foodNutritionInput.MarketValue;
            this.FNSR01Stock.TotalContainerSizeUsingServingSizeUnit += foodNutritionInput.ContainerSizeUsingServingSizeUnit;
            this.FNSR01Stock.TotalServingCost += foodNutritionInput.ServingCost;
            this.FNSR01Stock.TotalActualServingSize += foodNutritionInput.ActualServingSize;
            this.FNSR01Stock.TotalTypicalServingsPerContainer += foodNutritionInput.TypicalServingsPerContainer;
            this.FNSR01Stock.TotalActualServingsPerContainer += foodNutritionInput.ActualServingsPerContainer;
            this.FNSR01Stock.TotalTypicalServingSize += foodNutritionInput.TypicalServingSize;
            this.FNSR01Stock.TotalWater_g += foodNutritionInput.ActualWater_g;
            this.FNSR01Stock.TotalEnerg_Kcal += foodNutritionInput.ActualEnerg_Kcal;
            this.FNSR01Stock.TotalProtein_g += foodNutritionInput.ActualProtein_g;
            this.FNSR01Stock.TotalLipid_Tot_g += foodNutritionInput.ActualLipid_Tot_g;
            this.FNSR01Stock.TotalAsh_g += foodNutritionInput.ActualAsh_g;
            this.FNSR01Stock.TotalCarbohydrt_g += foodNutritionInput.ActualCarbohydrt_g;
            this.FNSR01Stock.TotalFiber_TD_g += foodNutritionInput.ActualFiber_TD_g;
            this.FNSR01Stock.TotalSugar_Tot_g += foodNutritionInput.ActualSugar_Tot_g;
            this.FNSR01Stock.TotalCalcium_mg += foodNutritionInput.ActualCalcium_mg;
            this.FNSR01Stock.TotalIron_mg += foodNutritionInput.ActualIron_mg;
            this.FNSR01Stock.TotalMagnesium_mg += foodNutritionInput.ActualMagnesium_mg;
            this.FNSR01Stock.TotalPhosphorus_mg += foodNutritionInput.ActualPhosphorus_mg;
            this.FNSR01Stock.TotalPotassium_mg += foodNutritionInput.ActualPotassium_mg;
            this.FNSR01Stock.TotalSodium_mg += foodNutritionInput.ActualSodium_mg;
            this.FNSR01Stock.TotalZinc_mg += foodNutritionInput.ActualZinc_mg;
            this.FNSR01Stock.TotalCopper_mg += foodNutritionInput.ActualCopper_mg;
            this.FNSR01Stock.TotalManganese_mg += foodNutritionInput.ActualManganese_mg;
            this.FNSR01Stock.TotalSelenium_pg += foodNutritionInput.ActualSelenium_pg;
            this.FNSR01Stock.TotalVit_C_mg += foodNutritionInput.ActualVit_C_mg;
            this.FNSR01Stock.TotalThiamin_mg += foodNutritionInput.ActualThiamin_mg;
            this.FNSR01Stock.TotalRiboflavin_mg += foodNutritionInput.ActualRiboflavin_mg;
            this.FNSR01Stock.TotalNiacin_mg += foodNutritionInput.ActualNiacin_mg;
            this.FNSR01Stock.TotalPanto_Acid_mg += foodNutritionInput.ActualPanto_Acid_mg;
            this.FNSR01Stock.TotalVit_B6_mg += foodNutritionInput.ActualVit_B6_mg;
            this.FNSR01Stock.TotalFolate_Tot_pg += foodNutritionInput.ActualFolate_Tot_pg;
            this.FNSR01Stock.TotalFolic_Acid_pg += foodNutritionInput.ActualFolic_Acid_pg;
            this.FNSR01Stock.TotalFood_Folate_pg += foodNutritionInput.ActualFood_Folate_pg;
            this.FNSR01Stock.TotalFolate_DFE_pg += foodNutritionInput.ActualFolate_DFE_pg;
            this.FNSR01Stock.TotalCholine_Tot_mg += foodNutritionInput.ActualCholine_Tot_mg;
            this.FNSR01Stock.TotalVit_B12_pg += foodNutritionInput.ActualVit_B12_pg;
            this.FNSR01Stock.TotalVit_A_IU += foodNutritionInput.ActualVit_A_IU;
            this.FNSR01Stock.TotalVit_A_RAE += foodNutritionInput.ActualVit_A_RAE;
            this.FNSR01Stock.TotalRetinol_pg += foodNutritionInput.ActualRetinol_pg;
            this.FNSR01Stock.TotalAlpha_Carot_pg += foodNutritionInput.ActualAlpha_Carot_pg;
            this.FNSR01Stock.TotalBeta_Carot_pg += foodNutritionInput.ActualBeta_Carot_pg;
            this.FNSR01Stock.TotalBeta_Crypt_pg += foodNutritionInput.ActualBeta_Crypt_pg;
            this.FNSR01Stock.TotalLycopene_pg += foodNutritionInput.ActualLycopene_pg;
            this.FNSR01Stock.TotalLut_Zea_pg += foodNutritionInput.ActualLut_Zea_pg;
            this.FNSR01Stock.TotalVit_E_mg += foodNutritionInput.ActualVit_E_mg;
            this.FNSR01Stock.TotalVit_D_pg += foodNutritionInput.ActualVit_D_pg;
            this.FNSR01Stock.TotalViVit_D_IU += foodNutritionInput.ActualViVit_D_IU;
            this.FNSR01Stock.TotalVit_K_pg += foodNutritionInput.ActualVit_K_pg;
            this.FNSR01Stock.TotalFA_Sat_g += foodNutritionInput.ActualFA_Sat_g;
            this.FNSR01Stock.TotalFA_Mono_g += foodNutritionInput.ActualFA_Mono_g;
            this.FNSR01Stock.TotalFA_Poly_g += foodNutritionInput.ActualFA_Poly_g;
            this.FNSR01Stock.TotalCholestrl_mg += foodNutritionInput.ActualCholestrl_mg;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddFNInputToStock(FNSR01Stock foodStock, double multiplier,
            FNSR01Calculator foodNutritionInput, string currentNodeName, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
                //don't change per hour food nutrition costs, only total costs
                if (currentNodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    //i.e. foodNutritionInput.cost = foodNutritionInput.cost * multiplier * input.ocamount
                    //multiplier = input.times * oc.amount * tp.amount
                    ChangeFNInputByInputMultipliers(foodNutritionInput, multiplier);
                }
                else
                {
                    //i.e. foodNutritionInput.cost = foodNutritionInput.cost * multiplier (1 in stock analyzers)
                    ChangeFNInputByMultiplier(foodNutritionInput, multiplier);
                }
            }
            foodStock.TotalMarketValue += foodNutritionInput.MarketValue;
            foodStock.TotalContainerSizeUsingServingSizeUnit += foodNutritionInput.ContainerSizeUsingServingSizeUnit;
            foodStock.TotalServingCost += foodNutritionInput.ServingCost;
            foodStock.TotalActualServingSize += foodNutritionInput.ActualServingSize;
            foodStock.TotalTypicalServingsPerContainer += foodNutritionInput.TypicalServingsPerContainer;
            foodStock.TotalActualServingsPerContainer += foodNutritionInput.ActualServingsPerContainer;
            foodStock.TotalTypicalServingSize += foodNutritionInput.TypicalServingSize;
            foodStock.TotalWater_g += foodNutritionInput.ActualWater_g;
            foodStock.TotalEnerg_Kcal += foodNutritionInput.ActualEnerg_Kcal;
            foodStock.TotalProtein_g += foodNutritionInput.ActualProtein_g;
            foodStock.TotalLipid_Tot_g += foodNutritionInput.ActualLipid_Tot_g;
            foodStock.TotalAsh_g += foodNutritionInput.ActualAsh_g;
            foodStock.TotalCarbohydrt_g += foodNutritionInput.ActualCarbohydrt_g;
            foodStock.TotalFiber_TD_g += foodNutritionInput.ActualFiber_TD_g;
            foodStock.TotalSugar_Tot_g += foodNutritionInput.ActualSugar_Tot_g;
            foodStock.TotalCalcium_mg += foodNutritionInput.ActualCalcium_mg;
            foodStock.TotalIron_mg += foodNutritionInput.ActualIron_mg;
            foodStock.TotalMagnesium_mg += foodNutritionInput.ActualMagnesium_mg;
            foodStock.TotalPhosphorus_mg += foodNutritionInput.ActualPhosphorus_mg;
            foodStock.TotalPotassium_mg += foodNutritionInput.ActualPotassium_mg;
            foodStock.TotalSodium_mg += foodNutritionInput.ActualSodium_mg;
            foodStock.TotalZinc_mg += foodNutritionInput.ActualZinc_mg;
            foodStock.TotalCopper_mg += foodNutritionInput.ActualCopper_mg;
            foodStock.TotalManganese_mg += foodNutritionInput.ActualManganese_mg;
            foodStock.TotalSelenium_pg += foodNutritionInput.ActualSelenium_pg;
            foodStock.TotalVit_C_mg += foodNutritionInput.ActualVit_C_mg;
            foodStock.TotalThiamin_mg += foodNutritionInput.ActualThiamin_mg;
            foodStock.TotalRiboflavin_mg += foodNutritionInput.ActualRiboflavin_mg;
            foodStock.TotalNiacin_mg += foodNutritionInput.ActualNiacin_mg;
            foodStock.TotalPanto_Acid_mg += foodNutritionInput.ActualPanto_Acid_mg;
            foodStock.TotalVit_B6_mg += foodNutritionInput.ActualVit_B6_mg;
            foodStock.TotalFolate_Tot_pg += foodNutritionInput.ActualFolate_Tot_pg;
            foodStock.TotalFolic_Acid_pg += foodNutritionInput.ActualFolic_Acid_pg;
            foodStock.TotalFood_Folate_pg += foodNutritionInput.ActualFood_Folate_pg;
            foodStock.TotalFolate_DFE_pg += foodNutritionInput.ActualFolate_DFE_pg;
            foodStock.TotalCholine_Tot_mg += foodNutritionInput.ActualCholine_Tot_mg;
            foodStock.TotalVit_B12_pg += foodNutritionInput.ActualVit_B12_pg;
            foodStock.TotalVit_A_IU += foodNutritionInput.ActualVit_A_IU;
            foodStock.TotalVit_A_RAE += foodNutritionInput.ActualVit_A_RAE;
            foodStock.TotalRetinol_pg += foodNutritionInput.ActualRetinol_pg;
            foodStock.TotalAlpha_Carot_pg += foodNutritionInput.ActualAlpha_Carot_pg;
            foodStock.TotalBeta_Carot_pg += foodNutritionInput.ActualBeta_Carot_pg;
            foodStock.TotalBeta_Crypt_pg += foodNutritionInput.ActualBeta_Crypt_pg;
            foodStock.TotalLycopene_pg += foodNutritionInput.ActualLycopene_pg;
            foodStock.TotalLut_Zea_pg += foodNutritionInput.ActualLut_Zea_pg;
            foodStock.TotalVit_E_mg += foodNutritionInput.ActualVit_E_mg;
            foodStock.TotalVit_D_pg += foodNutritionInput.ActualVit_D_pg;
            foodStock.TotalViVit_D_IU += foodNutritionInput.ActualViVit_D_IU;
            foodStock.TotalVit_K_pg += foodNutritionInput.ActualVit_K_pg;
            foodStock.TotalFA_Sat_g += foodNutritionInput.ActualFA_Sat_g;
            foodStock.TotalFA_Mono_g += foodNutritionInput.ActualFA_Mono_g;
            foodStock.TotalFA_Poly_g += foodNutritionInput.ActualFA_Poly_g;
            foodStock.TotalCholestrl_mg += foodNutritionInput.ActualCholestrl_mg;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddFNSR01StockToStock(FNSR01Stock totalsFN1Stock,
            double multiplier, FNSR01Stock currentFN1Stock)
        {
            bool bHasCalculations = false;
            //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
            //don't change per hour food nutrition costs, only total costs
            totalsFN1Stock.TotalMarketValue += currentFN1Stock.TotalMarketValue;
            totalsFN1Stock.TotalContainerSizeUsingServingSizeUnit += currentFN1Stock.TotalContainerSizeUsingServingSizeUnit;
            totalsFN1Stock.TotalServingCost += currentFN1Stock.TotalServingCost;
            totalsFN1Stock.TotalActualServingSize += currentFN1Stock.TotalActualServingSize;
            totalsFN1Stock.TotalTypicalServingsPerContainer += currentFN1Stock.TotalTypicalServingsPerContainer;
            totalsFN1Stock.TotalActualServingsPerContainer += currentFN1Stock.TotalActualServingsPerContainer;
            totalsFN1Stock.TotalTypicalServingSize += currentFN1Stock.TotalTypicalServingSize;
            totalsFN1Stock.TotalWater_g += currentFN1Stock.TotalWater_g;
            totalsFN1Stock.TotalEnerg_Kcal += currentFN1Stock.TotalEnerg_Kcal;
            totalsFN1Stock.TotalProtein_g += currentFN1Stock.TotalProtein_g;
            totalsFN1Stock.TotalLipid_Tot_g += currentFN1Stock.TotalLipid_Tot_g;
            totalsFN1Stock.TotalAsh_g += currentFN1Stock.TotalAsh_g;
            totalsFN1Stock.TotalCarbohydrt_g += currentFN1Stock.TotalCarbohydrt_g;
            totalsFN1Stock.TotalFiber_TD_g += currentFN1Stock.TotalFiber_TD_g;
            totalsFN1Stock.TotalSugar_Tot_g += currentFN1Stock.TotalSugar_Tot_g;
            totalsFN1Stock.TotalCalcium_mg += currentFN1Stock.TotalCalcium_mg;
            totalsFN1Stock.TotalIron_mg += currentFN1Stock.TotalIron_mg;
            totalsFN1Stock.TotalMagnesium_mg += currentFN1Stock.TotalMagnesium_mg;
            totalsFN1Stock.TotalPhosphorus_mg += currentFN1Stock.TotalPhosphorus_mg;
            totalsFN1Stock.TotalPotassium_mg += currentFN1Stock.TotalPotassium_mg;
            totalsFN1Stock.TotalSodium_mg += currentFN1Stock.TotalSodium_mg;
            totalsFN1Stock.TotalZinc_mg += currentFN1Stock.TotalZinc_mg;
            totalsFN1Stock.TotalCopper_mg += currentFN1Stock.TotalCopper_mg;
            totalsFN1Stock.TotalManganese_mg += currentFN1Stock.TotalManganese_mg;
            totalsFN1Stock.TotalSelenium_pg += currentFN1Stock.TotalSelenium_pg;
            totalsFN1Stock.TotalVit_C_mg += currentFN1Stock.TotalVit_C_mg;
            totalsFN1Stock.TotalThiamin_mg += currentFN1Stock.TotalThiamin_mg;
            totalsFN1Stock.TotalRiboflavin_mg += currentFN1Stock.TotalRiboflavin_mg;
            totalsFN1Stock.TotalNiacin_mg += currentFN1Stock.TotalNiacin_mg;
            totalsFN1Stock.TotalPanto_Acid_mg += currentFN1Stock.TotalPanto_Acid_mg;
            totalsFN1Stock.TotalVit_B6_mg += currentFN1Stock.TotalVit_B6_mg;
            totalsFN1Stock.TotalFolate_Tot_pg += currentFN1Stock.TotalFolate_Tot_pg;
            totalsFN1Stock.TotalFolic_Acid_pg += currentFN1Stock.TotalFolic_Acid_pg;
            totalsFN1Stock.TotalFood_Folate_pg += currentFN1Stock.TotalFood_Folate_pg;
            totalsFN1Stock.TotalFolate_DFE_pg += currentFN1Stock.TotalFolate_DFE_pg;
            totalsFN1Stock.TotalCholine_Tot_mg += currentFN1Stock.TotalCholine_Tot_mg;
            totalsFN1Stock.TotalVit_B12_pg += currentFN1Stock.TotalVit_B12_pg;
            totalsFN1Stock.TotalVit_A_IU += currentFN1Stock.TotalVit_A_IU;
            totalsFN1Stock.TotalVit_A_RAE += currentFN1Stock.TotalVit_A_RAE;
            totalsFN1Stock.TotalRetinol_pg += currentFN1Stock.TotalRetinol_pg;
            totalsFN1Stock.TotalAlpha_Carot_pg += currentFN1Stock.TotalAlpha_Carot_pg;
            totalsFN1Stock.TotalBeta_Carot_pg += currentFN1Stock.TotalBeta_Carot_pg;
            totalsFN1Stock.TotalBeta_Crypt_pg += currentFN1Stock.TotalBeta_Crypt_pg;
            totalsFN1Stock.TotalLycopene_pg += currentFN1Stock.TotalLycopene_pg;
            totalsFN1Stock.TotalLut_Zea_pg += currentFN1Stock.TotalLut_Zea_pg;
            totalsFN1Stock.TotalVit_E_mg += currentFN1Stock.TotalVit_E_mg;
            totalsFN1Stock.TotalVit_D_pg += currentFN1Stock.TotalVit_D_pg;
            totalsFN1Stock.TotalViVit_D_IU += currentFN1Stock.TotalViVit_D_IU;
            totalsFN1Stock.TotalVit_K_pg += currentFN1Stock.TotalVit_K_pg;
            totalsFN1Stock.TotalFA_Sat_g += currentFN1Stock.TotalFA_Sat_g;
            totalsFN1Stock.TotalFA_Mono_g += currentFN1Stock.TotalFA_Mono_g;
            totalsFN1Stock.TotalFA_Poly_g += currentFN1Stock.TotalFA_Poly_g;
            totalsFN1Stock.TotalCholestrl_mg += currentFN1Stock.TotalCholestrl_mg;
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
        public static double GetInputFullCostMultiplier(FNSR01Calculator foodNutritionInput,
            CalculatorParameters calcParams)
        {
            //to keep all food nutrition stock analysis consistent, put the totals 
            //in the inputs, then all can be compared consistently
            //input.times
            double dbInputMultiplier = IOFNStockSubscriber
                .GetMultiplierForTechInput(foodNutritionInput);
            //oc.amount
            double dbOCMultiplier = OCFNStockSubscriber
                .GetMultiplierForOperation(calcParams.ParentOperationComponent);
            double dbTPAmount = BIFNStockSubscriber.GetMultiplierForTimePeriod(
                calcParams.ParentTimePeriod);
            double dbMultiplier = dbInputMultiplier * dbOCMultiplier * dbTPAmount;
            return dbMultiplier;
        }

        public static void ChangeFNInputByMultiplier(FNSR01Calculator foodNutritionInput,
            double multiplier)
        {
            //cost per unit served * units served * multiplier
            foodNutritionInput.ServingCost = (foodNutritionInput.ServingCost * multiplier);
            foodNutritionInput.TotalOC = foodNutritionInput.ServingCost;
        }
        public static void ChangeFNInputByInputMultipliers(FNSR01Calculator foodNutritionInput,
            double multiplier)
        {
            //this is ok for foodstock analysis (not npv analysis)
            //cost per unit served * units served * input.times
            foodNutritionInput.ServingCost = (foodNutritionInput.OCPrice * multiplier * foodNutritionInput.OCAmount);
            foodNutritionInput.TotalOC = foodNutritionInput.ServingCost;
        }

        public static void AddFNSR01CalculatorTotals(FNSR01Calculator foodNutritionInput,
            ref XElement foodNutritionInputCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            foodNutritionInput.TotalOC = foodNutritionInput.ServingCost;
            CalculatorHelpers.SetAttributeDoubleF2(foodNutritionInputCalcElement,
                CostBenefitCalculator.TOC, foodNutritionInput.TotalOC);
            //extra multiplier needed for display
            CalculatorHelpers.SetAttributeDoubleF2(foodNutritionInputCalcElement,
                Input.INPUT_TIMES, foodNutritionInput.Times);
        }
        public static void AddFNSR01StockTotals(FNSR01Stock foodFactStock,
            ref XElement foodStockCalcElement)
        {
            ////these have already been adjusted by input multipliers (ocamount, times)
            foodFactStock.TotalOC = foodFactStock.TotalServingCost;
            CalculatorHelpers.SetAttributeDoubleF2(foodStockCalcElement,
                CostBenefitCalculator.TOC, foodFactStock.TotalOC);
        }
    }
}