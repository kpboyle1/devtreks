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
    ///Date:		2013, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. 
    /// </summary>
    public class BIFNStockCalculator : BudgetInvestmentCalculator
    {
        public BIFNStockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets calc and analysis
            //this needs to set machstock
            Init();
        }
        public BIFNStockCalculator()
        {
            //only used to access some methods
        }
        private void Init()
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            //collections of calculated food nutrition
            this.FNStock = new FNStock();
            this.FNStock.FoodFacts
                = new Dictionary<int, List<FoodFactCalculator>>();
            //init indexing
            this.TimePeriodStartingFileIndex = 0;
            this.BudgetStartingFileIndex = 0;
        }

        //constants used by this calculator
        public const string ZERO = "0";
        //stateful food nutrition stock
        public FNStock FNStock { get; set; }

        //these objects hold collections of descendants for optimizing total costs
        public FoodFactCalculator FoodFact1Input { get; set; }

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
                bHasCalculations = SetTotalFNStockCalculations(
                   ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIFNStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = SetTimePeriodFNStockCalculations(
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
                bHasCalculations = SetOpOrCompFNStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetInputFNStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }

        public bool SetTotalFNStockCalculations(ref XElement currentCalculationsElement,
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
            this.FNStock.SetCalculatorProperties(currentCalculationsElement);
            //the foodStock.machstocks dictionary can now be summed to derive totals
            //bimachstockcalculator handles calculations
            double dbMultiplier = 1;
            //set the food nutrition stock totals from foodStock collection
            bHasCalculations = SetTotalFNStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNStock.SetTotalFNStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNStockTotals(this.FNStock, ref currentCalculationsElement);
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
        public bool SetBIFNStockCalculations(ref XElement currentCalculationsElement,
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
            this.FNStock.SetCalculatorProperties(currentCalculationsElement);
            //the foodStock.machstocks dictionary can now be summed to derive totals
            double dbMultiplier = 1;
            //set the food nutrition stock totals from foodStock collection
            bHasCalculations = SetTotalFNStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNStock.SetTotalFNStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNStockTotals(this.FNStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.FNStock.InitTotalFNStockProperties();
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
        public bool SetTimePeriodFNStockCalculations(ref XElement currentCalculationsElement,
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
            this.FNStock.SetCalculatorProperties(currentCalculationsElement);
            //don't double count the tp multiplier -each op comp already used it to set penalties
            double dbMultiplier = 1;

            //the foodStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalFNStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNStock.SetTotalFNStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNStockTotals(this.FNStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next tp)
            this.FNStock.InitTotalFNStockProperties();
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
        
        public bool SetOpOrCompFNStockCalculations(ref XElement currentCalculationsElement,
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
            this.FNStock.SetCalculatorProperties(currentCalculationsElement);
            //oc.amount was multiplied in the inputs
            double dbMultiplier = 1;
            //the foodStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalFNStockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change opOrComp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            //set new food nutrition stock totals
            this.FNStock.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            this.FNStock.SetTotalFNStockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //(note: already set with correct discounting in base npv; is this here for penalties apps?)
            //set the totaloc and totalaoh
            AddFNStockTotals(this.FNStock, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next opOrComp)
            this.FNStock.InitTotalFNStockProperties();
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
        public bool SetInputFNStockCalculations(ref XElement currentCalculationsElement,
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
                this.FoodFact1Input = new FoodFactCalculator();
                //deserialize xml to object
                this.FoodFact1Input.SetFoodFactProperties(this.GCCalculatorParams,
                   currentCalculationsElement, currentElement);
                //all stocks analyzers put full costs in inputs (easier to manipulate collections)
                double dbMultiplier = GetInputFullCostMultiplier(this.FoodFact1Input, this.GCCalculatorParams);
                //change fuel cost, repair cost, by input.times * input.ocamount or input.aohamount
                ChangeFNInputByInputMultipliers(this.FoodFact1Input, dbMultiplier);
                //serialize calculator object back to xml
                //(calculator doesn't change opOrComp, so don't serialize it)
                string sAttNameExtension = string.Empty;
                //set new foodfact input totals
                this.FoodFact1Input.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                   ref currentCalculationsElement);
                this.FoodFact1Input.SetNewInputAttributes(this.GCCalculatorParams, ref currentCalculationsElement);
                this.FoodFact1Input.SetFoodFactAttributes(sAttNameExtension,
                    ref currentCalculationsElement);
                //set the totaloc and totalaoh
                AddFoodFactCalculatorTotals(this.FoodFact1Input, ref currentCalculationsElement);
                //set calculatorid (primary way to display calculation attributes)
                CalculatorHelpers.SetCalculatorId(
                    currentCalculationsElement, currentElement);
                //add the foodNutritionInput to the foodStock.machstocks dictionary
                //the count is 1-based, while iNodePosition is 0-based
                //so the count is the correct next index position
                int iNodePosition = this.FNStock.GetNodePositionCount(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.FoodFact1Input);
                if (iNodePosition < 0)
                    iNodePosition = 0;
                bHasCalculations = this.FNStock
                    .AddFNStocksToDictionary(
                    this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                    this.FoodFact1Input);
                //add to collection
                if (this.OpComp.Inputs == null)
                {
                    this.OpComp.Inputs = new List<Input>();
                }
                //note that foodNutritionInput can be retrieved by converting the input to the 
                //FoodFactCalculator type (foodNutritionInput = (FoodFactCalculator) input)
                if (this.FoodFact1Input != null)
                {
                    this.OpComp.Inputs.Add(this.FoodFact1Input);
                }
                bHasCalculations = true;
            }
            return bHasCalculations;
        }

        public bool SetTotalFNStockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.FNStock.FoodFacts != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<FoodFactCalculator>> kvp
                    in this.FNStock.FoodFacts)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        iNodeCount = 0;
                        foreach (FoodFactCalculator foodNutritionInput in kvp.Value)
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

        public bool AddFNInputToStock(double multiplier, FoodFactCalculator foodNutritionInput,
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
            this.FNStock.TotalMarketValue += (foodNutritionInput.MarketValue);
            this.FNStock.TotalContainerSize += (foodNutritionInput.ContainerSize);
            this.FNStock.TotalServingCost += (foodNutritionInput.ServingCost);
            this.FNStock.TotalActualCaloriesPerDay += (foodNutritionInput.ActualCaloriesPerDay);
            this.FNStock.TotalServingsPerContainer += (foodNutritionInput.ServingsPerContainer);
            this.FNStock.TotalActualServingsPerContainer += (foodNutritionInput.ActualServingsPerContainer);
            this.FNStock.TotalGenderOfServingPerson += 1;
            this.FNStock.TotalWeightOfServingPerson += (foodNutritionInput.WeightOfServingPerson);
            this.FNStock.TotalCaloriesPerServing += (foodNutritionInput.CaloriesPerServing);
            this.FNStock.TotalCaloriesPerActualServing += (foodNutritionInput.CaloriesPerActualServing);
            this.FNStock.TotalCaloriesFromFatPerServing += (foodNutritionInput.CaloriesFromFatPerServing);
            this.FNStock.TotalCaloriesFromFatPerActualServing += (foodNutritionInput.CaloriesFromFatPerActualServing);
            this.FNStock.TotalTotalFatPerServing += (foodNutritionInput.TotalFatPerServing);
            this.FNStock.TotalTotalFatPerActualServing += (foodNutritionInput.TotalFatPerActualServing);
            this.FNStock.TotalTotalFatActualDailyPercent += (foodNutritionInput.TotalFatActualDailyPercent);
            this.FNStock.TotalSaturatedFatPerServing += (foodNutritionInput.SaturatedFatPerServing);
            this.FNStock.TotalSaturatedFatPerActualServing += (foodNutritionInput.SaturatedFatPerActualServing);
            this.FNStock.TotalSaturatedFatActualDailyPercent += (foodNutritionInput.SaturatedFatActualDailyPercent);
            this.FNStock.TotalTransFatPerServing += (foodNutritionInput.TransFatPerServing);
            this.FNStock.TotalTransFatPerActualServing += (foodNutritionInput.TransFatPerActualServing);
            this.FNStock.TotalCholesterolPerServing += (foodNutritionInput.CholesterolPerServing);
            this.FNStock.TotalCholesterolPerActualServing += (foodNutritionInput.CholesterolPerActualServing);
            this.FNStock.TotalCholesterolActualDailyPercent += (foodNutritionInput.CholesterolActualDailyPercent);
            this.FNStock.TotalSodiumPerServing += (foodNutritionInput.SodiumPerServing);
            this.FNStock.TotalSodiumPerActualServing += (foodNutritionInput.SodiumPerActualServing);
            this.FNStock.TotalSodiumActualDailyPercent += (foodNutritionInput.SodiumActualDailyPercent);
            this.FNStock.TotalPotassiumPerServing += (foodNutritionInput.PotassiumPerServing);
            this.FNStock.TotalPotassiumPerActualServing += (foodNutritionInput.PotassiumPerActualServing);
            this.FNStock.TotalTotalCarbohydratePerServing += (foodNutritionInput.TotalCarbohydratePerServing);
            this.FNStock.TotalTotalCarbohydratePerActualServing += (foodNutritionInput.TotalCarbohydratePerActualServing);
            this.FNStock.TotalTotalCarbohydrateActualDailyPercent += (foodNutritionInput.TotalCarbohydrateActualDailyPercent);
            this.FNStock.TotalOtherCarbohydratePerServing += (foodNutritionInput.OtherCarbohydratePerServing);
            this.FNStock.TotalOtherCarbohydratePerActualServing += (foodNutritionInput.OtherCarbohydratePerActualServing);
            this.FNStock.TotalOtherCarbohydrateActualDailyPercent += (foodNutritionInput.OtherCarbohydrateActualDailyPercent);
            this.FNStock.TotalDietaryFiberPerServing += (foodNutritionInput.DietaryFiberPerServing);
            this.FNStock.TotalDietaryFiberPerActualServing += (foodNutritionInput.DietaryFiberPerActualServing);
            this.FNStock.TotalDietaryFiberActualDailyPercent += (foodNutritionInput.DietaryFiberActualDailyPercent);
            this.FNStock.TotalSugarsPerServing += (foodNutritionInput.SugarsPerServing);
            this.FNStock.TotalSugarsPerActualServing += (foodNutritionInput.SugarsPerActualServing);
            this.FNStock.TotalProteinPerServing += (foodNutritionInput.ProteinPerServing);
            this.FNStock.TotalProteinPerActualServing += (foodNutritionInput.ProteinPerActualServing);
            this.FNStock.TotalProteinActualDailyPercent += (foodNutritionInput.ProteinActualDailyPercent);
            this.FNStock.TotalVitaminAPercentDailyValue += (foodNutritionInput.VitaminAPercentDailyValue);
            this.FNStock.TotalVitaminAPercentActualDailyValue += (foodNutritionInput.VitaminAPercentActualDailyValue);
            this.FNStock.TotalVitaminCPercentDailyValue += (foodNutritionInput.VitaminCPercentDailyValue);
            this.FNStock.TotalVitaminCPercentActualDailyValue += (foodNutritionInput.VitaminCPercentActualDailyValue);
            this.FNStock.TotalVitaminDPercentDailyValue += (foodNutritionInput.VitaminDPercentDailyValue);
            this.FNStock.TotalVitaminDPercentActualDailyValue += (foodNutritionInput.VitaminDPercentActualDailyValue);
            this.FNStock.TotalCalciumPercentDailyValue += (foodNutritionInput.CalciumPercentDailyValue);
            this.FNStock.TotalCalciumPercentActualDailyValue += (foodNutritionInput.CalciumPercentActualDailyValue);
            this.FNStock.TotalIronPercentDailyValue += (foodNutritionInput.IronPercentDailyValue);
            this.FNStock.TotalIronPercentActualDailyValue += (foodNutritionInput.IronPercentActualDailyValue);
            this.FNStock.TotalThiaminPercentDailyValue += (foodNutritionInput.ThiaminPercentDailyValue);
            this.FNStock.TotalThiaminPercentActualDailyValue += (foodNutritionInput.ThiaminPercentActualDailyValue);
            this.FNStock.TotalFolatePercentDailyValue += (foodNutritionInput.FolatePercentDailyValue);
            this.FNStock.TotalFolatePercentActualDailyValue += (foodNutritionInput.FolatePercentActualDailyValue);
            this.FNStock.TotalRiboflavinPercentDailyValue += (foodNutritionInput.RiboflavinPercentDailyValue);
            this.FNStock.TotalRiboflavinPercentActualDailyValue += (foodNutritionInput.RiboflavinPercentActualDailyValue);
            this.FNStock.TotalNiacinPercentDailyValue += (foodNutritionInput.NiacinPercentDailyValue);
            this.FNStock.TotalNiacinPercentActualDailyValue += (foodNutritionInput.NiacinPercentActualDailyValue);
            this.FNStock.TotalVitaminB6PercentDailyValue += (foodNutritionInput.VitaminB6PercentDailyValue);
            this.FNStock.TotalVitaminB6PercentActualDailyValue += (foodNutritionInput.VitaminB6PercentActualDailyValue);
            this.FNStock.TotalVitaminB12PercentDailyValue += (foodNutritionInput.VitaminB12PercentDailyValue);
            this.FNStock.TotalVitaminB12PercentActualDailyValue += (foodNutritionInput.VitaminB12PercentActualDailyValue);
            this.FNStock.TotalPhosphorousPercentDailyValue += (foodNutritionInput.PhosphorousPercentDailyValue);
            this.FNStock.TotalPhosphorousPercentActualDailyValue += (foodNutritionInput.PhosphorousPercentActualDailyValue);
            this.FNStock.TotalMagnesiumPercentDailyValue += (foodNutritionInput.MagnesiumPercentDailyValue);
            this.FNStock.TotalMagnesiumPercentActualDailyValue += (foodNutritionInput.MagnesiumPercentActualDailyValue);
            this.FNStock.TotalZincPercentDailyValue += (foodNutritionInput.ZincPercentDailyValue);
            this.FNStock.TotalZincPercentActualDailyValue += (foodNutritionInput.ZincPercentActualDailyValue);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddFNInputToStock(FNStock foodStock, double multiplier,
            FoodFactCalculator foodNutritionInput, string currentNodeName, bool adjustTotals)
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
            foodStock.TotalMarketValue += (foodNutritionInput.MarketValue);
            foodStock.TotalContainerSize += (foodNutritionInput.ContainerSize);
            foodStock.TotalServingCost += (foodNutritionInput.ServingCost);
            foodStock.TotalActualCaloriesPerDay += (foodNutritionInput.ActualCaloriesPerDay);
            foodStock.TotalServingsPerContainer += (foodNutritionInput.ServingsPerContainer);
            foodStock.TotalActualServingsPerContainer += (foodNutritionInput.ActualServingsPerContainer);
            foodStock.TotalGenderOfServingPerson += 1;
            foodStock.TotalWeightOfServingPerson += (foodNutritionInput.WeightOfServingPerson);
            foodStock.TotalCaloriesPerServing += (foodNutritionInput.CaloriesPerServing);
            foodStock.TotalCaloriesPerActualServing += (foodNutritionInput.CaloriesPerActualServing);
            foodStock.TotalCaloriesFromFatPerServing += (foodNutritionInput.CaloriesFromFatPerServing);
            foodStock.TotalCaloriesFromFatPerActualServing += (foodNutritionInput.CaloriesFromFatPerActualServing);
            foodStock.TotalTotalFatPerServing += (foodNutritionInput.TotalFatPerServing);
            foodStock.TotalTotalFatPerActualServing += (foodNutritionInput.TotalFatPerActualServing);
            foodStock.TotalTotalFatActualDailyPercent += (foodNutritionInput.TotalFatActualDailyPercent);
            foodStock.TotalSaturatedFatPerServing += (foodNutritionInput.SaturatedFatPerServing);
            foodStock.TotalSaturatedFatPerActualServing += (foodNutritionInput.SaturatedFatPerActualServing);
            foodStock.TotalSaturatedFatActualDailyPercent += (foodNutritionInput.SaturatedFatActualDailyPercent);
            foodStock.TotalTransFatPerServing += (foodNutritionInput.TransFatPerServing);
            foodStock.TotalTransFatPerActualServing += (foodNutritionInput.TransFatPerActualServing);
            foodStock.TotalCholesterolPerServing += (foodNutritionInput.CholesterolPerServing);
            foodStock.TotalCholesterolPerActualServing += (foodNutritionInput.CholesterolPerActualServing);
            foodStock.TotalCholesterolActualDailyPercent += (foodNutritionInput.CholesterolActualDailyPercent);
            foodStock.TotalSodiumPerServing += (foodNutritionInput.SodiumPerServing);
            foodStock.TotalSodiumPerActualServing += (foodNutritionInput.SodiumPerActualServing);
            foodStock.TotalSodiumActualDailyPercent += (foodNutritionInput.SodiumActualDailyPercent);
            foodStock.TotalPotassiumPerServing += (foodNutritionInput.PotassiumPerServing);
            foodStock.TotalPotassiumPerActualServing += (foodNutritionInput.PotassiumPerActualServing);
            foodStock.TotalTotalCarbohydratePerServing += (foodNutritionInput.TotalCarbohydratePerServing);
            foodStock.TotalTotalCarbohydratePerActualServing += (foodNutritionInput.TotalCarbohydratePerActualServing);
            foodStock.TotalTotalCarbohydrateActualDailyPercent += (foodNutritionInput.TotalCarbohydrateActualDailyPercent);
            foodStock.TotalOtherCarbohydratePerServing += (foodNutritionInput.OtherCarbohydratePerServing);
            foodStock.TotalOtherCarbohydratePerActualServing += (foodNutritionInput.OtherCarbohydratePerActualServing);
            foodStock.TotalOtherCarbohydrateActualDailyPercent += (foodNutritionInput.OtherCarbohydrateActualDailyPercent);
            foodStock.TotalDietaryFiberPerServing += (foodNutritionInput.DietaryFiberPerServing);
            foodStock.TotalDietaryFiberPerActualServing += (foodNutritionInput.DietaryFiberPerActualServing);
            foodStock.TotalDietaryFiberActualDailyPercent += (foodNutritionInput.DietaryFiberActualDailyPercent);
            foodStock.TotalSugarsPerServing += (foodNutritionInput.SugarsPerServing);
            foodStock.TotalSugarsPerActualServing += (foodNutritionInput.SugarsPerActualServing);
            foodStock.TotalProteinPerServing += (foodNutritionInput.ProteinPerServing);
            foodStock.TotalProteinPerActualServing += (foodNutritionInput.ProteinPerActualServing);
            foodStock.TotalProteinActualDailyPercent += (foodNutritionInput.ProteinActualDailyPercent);
            foodStock.TotalVitaminAPercentDailyValue += (foodNutritionInput.VitaminAPercentDailyValue);
            foodStock.TotalVitaminAPercentActualDailyValue += (foodNutritionInput.VitaminAPercentActualDailyValue);
            foodStock.TotalVitaminCPercentDailyValue += (foodNutritionInput.VitaminCPercentDailyValue);
            foodStock.TotalVitaminCPercentActualDailyValue += (foodNutritionInput.VitaminCPercentActualDailyValue);
            foodStock.TotalVitaminDPercentDailyValue += (foodNutritionInput.VitaminDPercentDailyValue);
            foodStock.TotalVitaminDPercentActualDailyValue += (foodNutritionInput.VitaminDPercentActualDailyValue);
            foodStock.TotalCalciumPercentDailyValue += (foodNutritionInput.CalciumPercentDailyValue);
            foodStock.TotalCalciumPercentActualDailyValue += (foodNutritionInput.CalciumPercentActualDailyValue);
            foodStock.TotalIronPercentDailyValue += (foodNutritionInput.IronPercentDailyValue);
            foodStock.TotalIronPercentActualDailyValue += (foodNutritionInput.IronPercentActualDailyValue);
            foodStock.TotalThiaminPercentDailyValue += (foodNutritionInput.ThiaminPercentDailyValue);
            foodStock.TotalThiaminPercentActualDailyValue += (foodNutritionInput.ThiaminPercentActualDailyValue);
            foodStock.TotalFolatePercentDailyValue += (foodNutritionInput.FolatePercentDailyValue);
            foodStock.TotalFolatePercentActualDailyValue += (foodNutritionInput.FolatePercentActualDailyValue);
            foodStock.TotalRiboflavinPercentDailyValue += (foodNutritionInput.RiboflavinPercentDailyValue);
            foodStock.TotalRiboflavinPercentActualDailyValue += (foodNutritionInput.RiboflavinPercentActualDailyValue);
            foodStock.TotalNiacinPercentDailyValue += (foodNutritionInput.NiacinPercentDailyValue);
            foodStock.TotalNiacinPercentActualDailyValue += (foodNutritionInput.NiacinPercentActualDailyValue);
            foodStock.TotalVitaminB6PercentDailyValue += (foodNutritionInput.VitaminB6PercentDailyValue);
            foodStock.TotalVitaminB6PercentActualDailyValue += (foodNutritionInput.VitaminB6PercentActualDailyValue);
            foodStock.TotalVitaminB12PercentDailyValue += (foodNutritionInput.VitaminB12PercentDailyValue);
            foodStock.TotalVitaminB12PercentActualDailyValue += (foodNutritionInput.VitaminB12PercentActualDailyValue);
            foodStock.TotalPhosphorousPercentDailyValue += (foodNutritionInput.PhosphorousPercentDailyValue);
            foodStock.TotalPhosphorousPercentActualDailyValue += (foodNutritionInput.PhosphorousPercentActualDailyValue);
            foodStock.TotalMagnesiumPercentDailyValue += (foodNutritionInput.MagnesiumPercentDailyValue);
            foodStock.TotalMagnesiumPercentActualDailyValue += (foodNutritionInput.MagnesiumPercentActualDailyValue);
            foodStock.TotalZincPercentDailyValue += (foodNutritionInput.ZincPercentDailyValue);
            foodStock.TotalZincPercentActualDailyValue += (foodNutritionInput.ZincPercentActualDailyValue);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddFNStockToStock(FNStock totalsFN1Stock,
            double multiplier, FNStock currentFN1Stock)
        {
            bool bHasCalculations = false;
            //multipliers (input.times, out.compositionamount, oc.amount, tp.amount)
            //don't change per hour food nutrition costs, only total costs
            totalsFN1Stock.TotalMarketValue += (currentFN1Stock.TotalMarketValue);
            totalsFN1Stock.TotalContainerSize += (currentFN1Stock.ContainerSize * multiplier);
            totalsFN1Stock.TotalServingCost += (currentFN1Stock.ServingCost * multiplier);
            totalsFN1Stock.TotalActualCaloriesPerDay += (currentFN1Stock.ActualCaloriesPerDay * multiplier);
            totalsFN1Stock.TotalServingsPerContainer += (currentFN1Stock.ServingsPerContainer * multiplier);
            totalsFN1Stock.TotalActualServingsPerContainer += (currentFN1Stock.ActualServingsPerContainer * multiplier);
            totalsFN1Stock.TotalGenderOfServingPerson += 1;
            totalsFN1Stock.TotalWeightOfServingPerson += (currentFN1Stock.WeightOfServingPerson * multiplier);
            totalsFN1Stock.TotalCaloriesPerServing += (currentFN1Stock.CaloriesPerServing * multiplier);
            totalsFN1Stock.TotalCaloriesPerActualServing += (currentFN1Stock.CaloriesPerActualServing * multiplier);
            totalsFN1Stock.TotalCaloriesFromFatPerServing += (currentFN1Stock.CaloriesFromFatPerServing * multiplier);
            totalsFN1Stock.TotalCaloriesFromFatPerActualServing += (currentFN1Stock.CaloriesFromFatPerActualServing * multiplier);
            totalsFN1Stock.TotalTotalFatPerServing += (currentFN1Stock.TotalFatPerServing * multiplier);
            totalsFN1Stock.TotalTotalFatPerActualServing += (currentFN1Stock.TotalFatPerActualServing * multiplier);
            totalsFN1Stock.TotalTotalFatActualDailyPercent += (currentFN1Stock.TotalFatActualDailyPercent * multiplier);
            totalsFN1Stock.TotalSaturatedFatPerServing += (currentFN1Stock.SaturatedFatPerServing * multiplier);
            totalsFN1Stock.TotalSaturatedFatPerActualServing += (currentFN1Stock.SaturatedFatPerActualServing * multiplier);
            totalsFN1Stock.TotalSaturatedFatActualDailyPercent += (currentFN1Stock.SaturatedFatActualDailyPercent * multiplier);
            totalsFN1Stock.TotalTransFatPerServing += (currentFN1Stock.TransFatPerServing * multiplier);
            totalsFN1Stock.TotalTransFatPerActualServing += (currentFN1Stock.TransFatPerActualServing * multiplier);
            totalsFN1Stock.TotalCholesterolPerServing += (currentFN1Stock.CholesterolPerServing * multiplier);
            totalsFN1Stock.TotalCholesterolPerActualServing += (currentFN1Stock.CholesterolPerActualServing * multiplier);
            totalsFN1Stock.TotalCholesterolActualDailyPercent += (currentFN1Stock.CholesterolActualDailyPercent * multiplier);
            totalsFN1Stock.TotalSodiumPerServing += (currentFN1Stock.SodiumPerServing * multiplier);
            totalsFN1Stock.TotalSodiumPerActualServing += (currentFN1Stock.SodiumPerActualServing * multiplier);
            totalsFN1Stock.TotalSodiumActualDailyPercent += (currentFN1Stock.SodiumActualDailyPercent * multiplier);
            totalsFN1Stock.TotalPotassiumPerServing += (currentFN1Stock.PotassiumPerServing * multiplier);
            totalsFN1Stock.TotalPotassiumPerActualServing += (currentFN1Stock.PotassiumPerActualServing * multiplier);
            totalsFN1Stock.TotalTotalCarbohydratePerServing += (currentFN1Stock.TotalCarbohydratePerServing * multiplier);
            totalsFN1Stock.TotalTotalCarbohydratePerActualServing += (currentFN1Stock.TotalCarbohydratePerActualServing * multiplier);
            totalsFN1Stock.TotalTotalCarbohydrateActualDailyPercent += (currentFN1Stock.TotalCarbohydrateActualDailyPercent * multiplier);
            totalsFN1Stock.TotalOtherCarbohydratePerServing += (currentFN1Stock.OtherCarbohydratePerServing * multiplier);
            totalsFN1Stock.TotalOtherCarbohydratePerActualServing += (currentFN1Stock.OtherCarbohydratePerActualServing * multiplier);
            totalsFN1Stock.TotalOtherCarbohydrateActualDailyPercent += (currentFN1Stock.OtherCarbohydrateActualDailyPercent * multiplier);
            totalsFN1Stock.TotalDietaryFiberPerServing += (currentFN1Stock.DietaryFiberPerServing * multiplier);
            totalsFN1Stock.TotalDietaryFiberPerActualServing += (currentFN1Stock.DietaryFiberPerActualServing * multiplier);
            totalsFN1Stock.TotalDietaryFiberActualDailyPercent += (currentFN1Stock.DietaryFiberActualDailyPercent * multiplier);
            totalsFN1Stock.TotalSugarsPerServing += (currentFN1Stock.SugarsPerServing * multiplier);
            totalsFN1Stock.TotalSugarsPerActualServing += (currentFN1Stock.SugarsPerActualServing * multiplier);
            totalsFN1Stock.TotalProteinPerServing += (currentFN1Stock.ProteinPerServing * multiplier);
            totalsFN1Stock.TotalProteinPerActualServing += (currentFN1Stock.ProteinPerActualServing * multiplier);
            totalsFN1Stock.TotalProteinActualDailyPercent += (currentFN1Stock.ProteinActualDailyPercent * multiplier);
            totalsFN1Stock.TotalVitaminAPercentDailyValue += (currentFN1Stock.VitaminAPercentDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminAPercentActualDailyValue += (currentFN1Stock.VitaminAPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminCPercentDailyValue += (currentFN1Stock.VitaminCPercentDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminCPercentActualDailyValue += (currentFN1Stock.VitaminCPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminDPercentDailyValue += (currentFN1Stock.VitaminDPercentDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminDPercentActualDailyValue += (currentFN1Stock.VitaminDPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalCalciumPercentDailyValue += (currentFN1Stock.CalciumPercentDailyValue * multiplier);
            totalsFN1Stock.TotalCalciumPercentActualDailyValue += (currentFN1Stock.CalciumPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalIronPercentDailyValue += (currentFN1Stock.IronPercentDailyValue * multiplier);
            totalsFN1Stock.TotalIronPercentActualDailyValue += (currentFN1Stock.IronPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalThiaminPercentDailyValue += (currentFN1Stock.ThiaminPercentDailyValue * multiplier);
            totalsFN1Stock.TotalThiaminPercentActualDailyValue += (currentFN1Stock.ThiaminPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalFolatePercentDailyValue += (currentFN1Stock.FolatePercentDailyValue * multiplier);
            totalsFN1Stock.TotalFolatePercentActualDailyValue += (currentFN1Stock.FolatePercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalRiboflavinPercentDailyValue += (currentFN1Stock.RiboflavinPercentDailyValue * multiplier);
            totalsFN1Stock.TotalRiboflavinPercentActualDailyValue += (currentFN1Stock.RiboflavinPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalNiacinPercentDailyValue += (currentFN1Stock.NiacinPercentDailyValue * multiplier);
            totalsFN1Stock.TotalNiacinPercentActualDailyValue += (currentFN1Stock.NiacinPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminB6PercentDailyValue += (currentFN1Stock.VitaminB6PercentDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminB6PercentActualDailyValue += (currentFN1Stock.VitaminB6PercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminB12PercentDailyValue += (currentFN1Stock.VitaminB12PercentDailyValue * multiplier);
            totalsFN1Stock.TotalVitaminB12PercentActualDailyValue += (currentFN1Stock.VitaminB12PercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalPhosphorousPercentDailyValue += (currentFN1Stock.PhosphorousPercentDailyValue * multiplier);
            totalsFN1Stock.TotalPhosphorousPercentActualDailyValue += (currentFN1Stock.PhosphorousPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalMagnesiumPercentDailyValue += (currentFN1Stock.MagnesiumPercentDailyValue * multiplier);
            totalsFN1Stock.TotalMagnesiumPercentActualDailyValue += (currentFN1Stock.MagnesiumPercentActualDailyValue * multiplier);
            totalsFN1Stock.TotalZincPercentDailyValue += (currentFN1Stock.ZincPercentDailyValue * multiplier);
            totalsFN1Stock.TotalZincPercentActualDailyValue += (currentFN1Stock.ZincPercentActualDailyValue * multiplier);
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
        public static double GetInputFullCostMultiplier(FoodFactCalculator foodNutritionInput,
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

        public static void ChangeFNInputByMultiplier(FoodFactCalculator foodNutritionInput,
            double multiplier)
        {
            //cost per unit served * units served * multiplier
            foodNutritionInput.ServingCost = (foodNutritionInput.ServingCost * multiplier);
            foodNutritionInput.TotalOC = foodNutritionInput.ServingCost;
        }
        public static void ChangeFNInputByInputMultipliers(FoodFactCalculator foodNutritionInput,
            double multiplier)
        {
            //this is ok for foodstock analysis (not npv analysis)
            //cost per unit served * units served * input.times
            foodNutritionInput.ServingCost = (foodNutritionInput.OCPrice * multiplier * foodNutritionInput.OCAmount);
            //foodNutritionInput.ServingCost = (foodNutritionInput.ServingCost * multiplier * foodNutritionInput.OCAmount);
            foodNutritionInput.TotalOC = foodNutritionInput.ServingCost;
        }

        public static void AddFoodFactCalculatorTotals(FoodFactCalculator foodNutritionInput,
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
        public static void AddFNStockTotals(FNStock foodFactStock,
            ref XElement foodStockCalcElement)
        {
            ////these have already been adjusted by input multipliers (ocamount, times)
            foodFactStock.TotalOC = foodFactStock.TotalServingCost;
            CalculatorHelpers.SetAttributeDoubleF2(foodStockCalcElement,
                CostBenefitCalculator.TOC, foodFactStock.TotalOC);
        }
    }
}
