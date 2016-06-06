using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Calculate food nutrition stocks for inputs.
    ///Author:		www.devtreks.org
    ///Date:		2012, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    /// </summary>
    public class IOFNStockCalculator
    {
        public IOFNStockCalculator() { }
        public IOFNStockCalculator(CalculatorParameters calcParameters)
        {
            BIFN1Calculator = new BIFNStockCalculator(calcParameters);
        }
        //stateful food nutrition stock
        BIFNStockCalculator BIFN1Calculator { get; set; }

        public bool AddCalculationsToCurrentElement(
            CalculatorParameters calcParameters, 
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString()
                && calcParameters.ExtensionDocToCalcURI.URINodeName
                != Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                bHasCalculations = BIFN1Calculator.SetTotalFNStockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                if (currentCalculationsElement != null)
                {
                    FoodFactCalculator foodFactInput = new FoodFactCalculator();
                    //deserialize xml to object
                    foodFactInput.SetFoodFactProperties(calcParameters,
                        currentCalculationsElement, currentElement);
                    //run the calculations
                    bHasCalculations = RunFNStockCalculations(foodFactInput,
                        calcParameters);
                    //serialize object back to xml
                    string sAttNameExtension = string.Empty;
                    //bool bRemoveAtts = false;
                    //note that unlike other IOAnalyzers, this runs the input calc too
                    //and must update input props to calculated results (OCAmount and OCPrice calcs)
                    //also note that if input analyzers are needed, probably want to use BIFNStockCalcor
                    //so that does not update input db props and keeps consistent pattern
                    foodFactInput.SetInputAttributes(calcParameters,
                        ref currentElement, updates);
                    //update the calculator attributes
                    foodFactInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    foodFactInput.SetNewInputAttributes(calcParameters, ref currentCalculationsElement);
                    foodFactInput.SetFoodFactAttributes(sAttNameExtension,
                        ref currentCalculationsElement);

                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    //input groups only aggregate inputs (not input series)
                    if (currentElement.Name.LocalName
                        .Contains(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        //add the food nutrition to the machstock.machstocks dictionary
                        //the count is 1-based, while iNodePosition is 0-based
                        //so the count is the correct next index position
                        int iNodePosition = BIFN1Calculator.FNStock
                            .GetNodePositionCount(calcParameters.AnalyzerParms.FilePositionIndex,
                            foodFactInput);
                        if (iNodePosition < 0)
                            iNodePosition = 0;
                        bHasCalculations = BIFN1Calculator.FNStock
                            .AddFNStocksToDictionary(
                            calcParameters.AnalyzerParms.FilePositionIndex, iNodePosition,
                            foodFactInput);
                    }
                }
            }
            return bHasCalculations;
        }
        public bool RunFNStockCalculations(FoodFactCalculator foodFactInput,
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //see if any db props are being changed by calculator
            TransferCorrespondingDbProperties(ref foodFactInput);
            //set the multiplier (the multiplier in most inputs is 1,
            //this is kept here to keep a uniform pattern when the multiplier 
            //can be changed -see the food nutrition calculator)
            double multiplier = GetMultiplierForFoodFact(foodFactInput);
            bHasCalculations = SetFNStockCalculations(multiplier,
                calcParameters, foodFactInput);
            return bHasCalculations;
        }
        public static double GetMultiplierForFoodFact(FoodFactCalculator foodFact)
        {
            //food fact only multiplier (IOFNStockSubscriber uses input.times instead)
            double multiplier = (foodFact.ActualServingsPerContainer == 0)
                ? 1 : foodFact.ServingsPerContainer / foodFact.ActualServingsPerContainer;
            return multiplier;
        }
        private void TransferCorrespondingDbProperties(
            ref FoodFactCalculator foodFactInput)
        {
            //calculators can use aliases to change db properties
            //but this calc gets its MV from input.CapPrice
            foodFactInput.MarketValue = foodFactInput.CAPPrice;
        }
   
        public bool SetFNStockCalculations(double multiplier,
            CalculatorParameters calcParameters, FoodFactCalculator foodFact)
        {
            bool bHasCalculations = false;
            if (foodFact != null)
            {
                foodFact.CaloriesPerActualServing = foodFact.CaloriesPerServing * multiplier;

                foodFact.CaloriesFromFatPerActualServing = foodFact.CaloriesFromFatPerServing * multiplier;

                foodFact.TotalFatPerActualServing = foodFact.TotalFatPerServing * multiplier;

                foodFact.SaturatedFatPerActualServing = foodFact.SaturatedFatPerServing * multiplier;

                foodFact.TransFatPerActualServing = foodFact.TransFatPerServing * multiplier;

                foodFact.CholesterolPerActualServing = foodFact.CholesterolPerServing * multiplier;

                foodFact.SodiumPerActualServing = foodFact.SodiumPerServing * multiplier;

                foodFact.PotassiumPerActualServing = foodFact.PotassiumPerServing * multiplier;

                foodFact.TotalCarbohydratePerActualServing = foodFact.TotalCarbohydratePerServing * multiplier;

                foodFact.OtherCarbohydratePerActualServing = foodFact.OtherCarbohydratePerServing * multiplier;

                foodFact.DietaryFiberPerActualServing = foodFact.DietaryFiberPerServing * multiplier;

                foodFact.SugarsPerActualServing = foodFact.SugarsPerServing * multiplier;

                foodFact.ProteinPerActualServing = foodFact.ProteinPerServing * multiplier;

                foodFact.VitaminAPercentActualDailyValue = foodFact.VitaminAPercentDailyValue * multiplier;

                foodFact.VitaminCPercentActualDailyValue = foodFact.VitaminCPercentDailyValue * multiplier;

                foodFact.VitaminDPercentActualDailyValue = foodFact.VitaminDPercentDailyValue * multiplier;

                foodFact.CalciumPercentActualDailyValue = foodFact.CalciumPercentDailyValue * multiplier;

                foodFact.IronPercentActualDailyValue = foodFact.IronPercentDailyValue * multiplier;

                foodFact.ThiaminPercentActualDailyValue = foodFact.ThiaminPercentDailyValue * multiplier;

                foodFact.FolatePercentActualDailyValue = foodFact.FolatePercentDailyValue * multiplier;

                foodFact.RiboflavinPercentActualDailyValue = foodFact.RiboflavinPercentDailyValue * multiplier;

                foodFact.NiacinPercentActualDailyValue = foodFact.NiacinPercentDailyValue * multiplier;

                foodFact.VitaminB6PercentActualDailyValue = foodFact.VitaminB6PercentDailyValue * multiplier;

                foodFact.VitaminB12PercentActualDailyValue = foodFact.VitaminB12PercentDailyValue * multiplier;

                foodFact.PhosphorousPercentActualDailyValue = foodFact.PhosphorousPercentDailyValue * multiplier;

                foodFact.MagnesiumPercentActualDailyValue = foodFact.MagnesiumPercentDailyValue * multiplier;

                foodFact.ZincPercentActualDailyValue = foodFact.ZincPercentDailyValue * multiplier;

                SetFoodNutritionPercentDailyValues(calcParameters, ref foodFact);

                SetFoodNutritionServingSize(calcParameters, ref foodFact);

                bHasCalculations = true;
            }
            else
            {
                calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            }
            return bHasCalculations;
        }
        private void SetFoodNutritionPercentDailyValues(
            CalculatorParameters calcParameters, ref FoodFactCalculator foodFact)
        {
            double TotalFatDailyAmount = 65;
            double SaturatedFatDailyAmount = 20;
            double CholesterolDailyAmount = 300;
            double SodiumDailyAmount = 2400;
            double TotalCarbohydrateDailyAmount = 300;
            //note: no data available for othercarbohydrate
            double DietaryFiberDailyAmount = 25;
            double ProteinDailyAmount = 50;
            if (foodFact.ActualCaloriesPerDay == 2500)
            {
                //not sure if this is a simple proportion or not
                //if it is a simple proportion, switch to a more flexible
                //"ActualCaloriesPerDay" form element and use the proportions for calcs
                TotalFatDailyAmount = 80;
                SaturatedFatDailyAmount = 25;
                CholesterolDailyAmount = 300;
                SodiumDailyAmount = 2400;
                TotalCarbohydrateDailyAmount = 375;
                DietaryFiberDailyAmount = 30;
                ProteinDailyAmount = 65;
            }
            foodFact.TotalFatActualDailyPercent = (foodFact.TotalFatPerActualServing / TotalFatDailyAmount) * 100;

            foodFact.SaturatedFatActualDailyPercent = (foodFact.SaturatedFatPerActualServing / SaturatedFatDailyAmount) * 100;

            foodFact.CholesterolActualDailyPercent = (foodFact.CholesterolPerActualServing / CholesterolDailyAmount) * 100;

            foodFact.SodiumActualDailyPercent = (foodFact.SodiumPerActualServing / SodiumDailyAmount) * 100;

            foodFact.TotalCarbohydrateActualDailyPercent = (foodFact.TotalCarbohydratePerActualServing / TotalCarbohydrateDailyAmount) * 100;

            foodFact.DietaryFiberActualDailyPercent = (foodFact.DietaryFiberPerActualServing / DietaryFiberDailyAmount) * 100;

            foodFact.ProteinActualDailyPercent = (foodFact.ProteinPerActualServing / ProteinDailyAmount) * 100;
        }
        private void SetFoodNutritionServingSize(
            CalculatorParameters calcParameters, ref FoodFactCalculator foodFact)
        {
            //check illegal divisors
            foodFact.ActualServingsPerContainer = (foodFact.ActualServingsPerContainer == 0)
                ? 1 : foodFact.ActualServingsPerContainer;
            foodFact.ContainerSize = (foodFact.ContainerSize == 0)
                ? 1 : foodFact.ContainerSize;
            //keep this calculator simple
            //update these input fields automatically in the db
            //(they just insert caprice in input)
            //serving size = ocamount
            //if needed (i.e. 2 slices bread) use the input.times property to make further adjustments in tech side
            foodFact.OCAmount =
               foodFact.ContainerSize / foodFact.ActualServingsPerContainer;
            //calculate OCPrice as a unit cost
            foodFact.OCPrice = foodFact.CAPPrice / foodFact.ContainerSize;
            //0.9.1 removed, no reason for aohprice) keep aoh price synched with ocprice
            //foodFact.AOHPrice = foodFact.OCPrice;
            //calculate cost per actual serving
            //note that this can change when the input is added elsewhere
            foodFact.ServingCost = foodFact.OCPrice * foodFact.OCAmount;
        }
    }
}
