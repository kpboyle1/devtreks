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
    ///Purpose:		Calculate food nutrition (USDA standard reference) stocks for inputs.
    ///Author:		www.devtreks.org
    ///Date:		2012, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    /// </summary>
    public class IOFNSR01StockCalculator
    {
        public IOFNSR01StockCalculator() { }
        public IOFNSR01StockCalculator(CalculatorParameters calcParameters)
        {
            BIFNSR01Calculator = new BIFNSR01StockCalculator(calcParameters);
        }
        //stateful food nutrition stock
        BIFNSR01StockCalculator BIFNSR01Calculator { get; set; }

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
                bHasCalculations = BIFNSR01Calculator.SetTotalFNSR01StockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else
            {
                if (currentCalculationsElement != null)
                {
                    FNSR01Calculator foodNutSRInput = new FNSR01Calculator();
                    //deserialize xml to object
                    foodNutSRInput.SetFNSR01Properties(calcParameters,
                        currentCalculationsElement, currentElement);
                    //run the calculations
                    bHasCalculations = RunFNSR01StockCalculations(foodNutSRInput,
                        calcParameters);
                    //serialize object back to xml
                    string sAttNameExtension = string.Empty;
                    ////bool bRemoveAtts = false;
                    //note that unlike other IOAnalyzers, this runs the input calc too
                    //and must update input props to calculated results (OCAmount and OCPrice calcs)
                    //also note that if input analyzers are needed, probably want to use BIFNSR01StockCalcor
                    //so that does not update input db props and keeps consistent pattern
                    foodNutSRInput.SetInputAttributes(calcParameters,
                        ref currentElement, updates);
                    //update the calculator attributes
                    foodNutSRInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                        ref currentCalculationsElement);
                    foodNutSRInput.SetNewInputAttributes(calcParameters, ref currentCalculationsElement);
                    foodNutSRInput.SetFNSR01Attributes(sAttNameExtension,
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
                        int iNodePosition = BIFNSR01Calculator.FNSR01Stock
                            .GetNodePositionCount(calcParameters.AnalyzerParms.FilePositionIndex,
                            foodNutSRInput);
                        if (iNodePosition < 0)
                            iNodePosition = 0;
                        bHasCalculations = BIFNSR01Calculator.FNSR01Stock
                            .AddFNSR01StocksToDictionary(
                            calcParameters.AnalyzerParms.FilePositionIndex, iNodePosition,
                            foodNutSRInput);
                    }
                }
            }
            return bHasCalculations;
        }
        public bool RunFNSR01StockCalculations(FNSR01Calculator foodNutSRInput,
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //see if any db props are being changed by calculator
            TransferCorrespondingDbProperties(ref foodNutSRInput);
            //set the multiplier (the multiplier in most inputs is 1,
            //this is kept here to keep a uniform pattern when the multiplier 
            //can be changed -see the food nutrition calculator)
            double multiplier = GetMultiplierForFNSR01(foodNutSRInput);
            bHasCalculations = SetFNSRStockCalculations(multiplier,
                calcParameters, foodNutSRInput);
            return bHasCalculations;
        }
        public static double GetMultiplierForFNSR01(FNSR01Calculator foodNutSR)
        {
            //food fact only multiplier (IOFNSR01StockSubscriber uses input.times instead)
            double multiplier = 1; 
            //step 1. To calculate the “amount of nutrient in edible portion of 1 pound (453.6
                //grams) as purchased,” use the following formula:
                //Y = V*4.536*[(100-R)/100]
                //where
                //Y = nutrient value per 1 pound as purchased,
                //V = nutrient value per 100 g (Nutr_Val in the Nutrient Data file), and
                //R = percent refuse
            //step 2. To calculate the nutrient content per household measure:
                //N = (V*W)/100
                //Where:
                //N = nutrient value per household measure,
                //V = nutrient value per 100 g (Nutr_Val in the Nutrient Data file), and
                //W = g weight of portion (Gm_Wgt in the Weight file).
            //step 3. To calculate the actual nutrient content per actual serving size 
                //(instead of per typical serving size)
                //AN = (N / typicalservingsize) * actualservingsize

            //double multiplier = 1; (foodNutSR.ActualServingsPerContainer == 0)
                 //? 1 : foodNutSR.ServingsPerContainer / foodNutSR.ActualServingsPerContainer;
            return multiplier;
        }
        private void TransferCorrespondingDbProperties(
            ref FNSR01Calculator foodNutSRInput)
        {
            //calculators can use aliases to change db properties
            //but this calc gets its MV from input.CapPrice
            foodNutSRInput.MarketValue = foodNutSRInput.CAPPrice;
        }

        public bool SetFNSRStockCalculations(double multiplier,
            CalculatorParameters calcParameters, FNSR01Calculator foodNutSR)
        {
            bool bHasCalculations = false;
            if (foodNutSR != null)
            {
                foodNutSR.ActualWater_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Water_g);
                foodNutSR.ActualEnerg_Kcal = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Energ_Kcal);
                foodNutSR.ActualProtein_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Protein_g);
                foodNutSR.ActualLipid_Tot_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Lipid_Tot_g);
                foodNutSR.ActualAsh_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Ash_g);
                foodNutSR.ActualCarbohydrt_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Carbohydrt_g);
                foodNutSR.ActualFiber_TD_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Fiber_TD_g);
                foodNutSR.ActualSugar_Tot_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Sugar_Tot_g);
                foodNutSR.ActualCalcium_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Calcium_mg);
                foodNutSR.ActualIron_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Iron_mg);
                foodNutSR.ActualMagnesium_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Magnesium_mg);
                foodNutSR.ActualPhosphorus_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Phosphorus_mg);
                foodNutSR.ActualPotassium_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Potassium_mg);
                foodNutSR.ActualSodium_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Sodium_mg);
                foodNutSR.ActualZinc_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Zinc_mg);
                foodNutSR.ActualCopper_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Copper_mg);
                foodNutSR.ActualManganese_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Manganese_mg);
                foodNutSR.ActualSelenium_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Selenium_pg);
                foodNutSR.ActualVit_C_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_C_mg);
                foodNutSR.ActualThiamin_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Thiamin_mg);
                foodNutSR.ActualRiboflavin_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Riboflavin_mg);
                foodNutSR.ActualNiacin_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Niacin_mg);
                foodNutSR.ActualPanto_Acid_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Panto_Acid_mg);
                foodNutSR.ActualVit_B6_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_B6_mg);
                foodNutSR.ActualFolate_Tot_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Folate_Tot_pg);
                foodNutSR.ActualFolic_Acid_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Folic_Acid_pg);
                foodNutSR.ActualFood_Folate_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Food_Folate_pg);
                foodNutSR.ActualFolate_DFE_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Folate_DFE_pg);
                foodNutSR.ActualCholine_Tot_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Choline_Tot_mg);
                foodNutSR.ActualVit_B12_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_B12_pg);
                foodNutSR.ActualVit_A_IU = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_A_IU);
                foodNutSR.ActualVit_A_RAE = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_A_RAE);
                foodNutSR.ActualRetinol_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Retinol_pg);
                foodNutSR.ActualAlpha_Carot_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Alpha_Carot_pg);
                foodNutSR.ActualBeta_Carot_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Beta_Carot_pg);
                foodNutSR.ActualBeta_Crypt_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Beta_Crypt_pg);
                foodNutSR.ActualLycopene_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Lycopene_pg);
                foodNutSR.ActualLut_Zea_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Lut_Zea_pg);
                foodNutSR.ActualVit_E_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_E_mg);
                foodNutSR.ActualVit_D_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_D_pg);
                foodNutSR.ActualViVit_D_IU = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.ViVit_D_IU);
                foodNutSR.ActualVit_K_pg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Vit_K_pg);
                foodNutSR.ActualFA_Sat_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.FA_Sat_g);
                foodNutSR.ActualFA_Mono_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.FA_Mono_g);
                foodNutSR.ActualFA_Poly_g = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.FA_Poly_g);
                foodNutSR.ActualCholestrl_mg = GetActualFoodNutrientProperty(foodNutSR, foodNutSR.Cholestrl_mg);

                SetFoodNutritionInputCosts(calcParameters, ref foodNutSR);

                bHasCalculations = true;
            }
            else
            {
                calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            }
            return bHasCalculations;
        }
        private double GetActualFoodNutrientProperty(FNSR01Calculator foodNutSR, double nutrientValToAdjust)
        {
            //food fact only multiplier (IOFNSR01StockSubscriber uses input.times instead)
            double dbActualNutValuePerHouseHoldMeasure = 1;
            //step 1. To calculate the “amount of nutrient in edible portion of 1 pound (453.6
                //grams) as purchased,” use the following formula:
                //Y = V*4.536*[(100-R)/100]
                //where
                //Y = nutrient value per 1 pound as purchased,
                //V = nutrient value per 100 g (Nutr_Val in the Nutrient Data file), and
                //R = percent refuse
            double dbNutValuePerPound = nutrientValToAdjust;
            if (foodNutSR.Refuse_Pct > 0)
            {
                dbNutValuePerPound = nutrientValToAdjust * 4.536 * ((100 - foodNutSR.Refuse_Pct) / 100);
            }
            //step 2. 100. For example, to calculate the
                //amount of fat in 1 tablespoon of butter (NDB No. 01001),
                //VH=(N*CM)/100
                //where:
                //Vh = the nutrient content per the desired common measure
                //N = the nutrient content per 100 g
                //For NDB No. 01001, fat = 81.11 g/100 g
                //CM = grams of the common measure
                //For NDB No. 01001, 1 tablespoon = 14.2 g
                //So using this formula for the above example:
                //Vh = (81.11*14.2)/100 = 11.52 g fat in 1 tablespoon of butter
            SetPortionWeightAndUnit(foodNutSR);
            //use typical serving size
            dbActualNutValuePerHouseHoldMeasure = (dbNutValuePerPound * foodNutSR.TypicalServingSize) / 100;
            //step 3. To calculate the actual nutrient content per actual serving size 
            //(instead of per typical serving size)
            //AN = (N / typicalservingsize) * actualservingsize
            //use actual serving size
            dbActualNutValuePerHouseHoldMeasure = (dbActualNutValuePerHouseHoldMeasure / foodNutSR.TypicalServingSize) 
                * foodNutSR.ActualServingSize;
            return dbActualNutValuePerHouseHoldMeasure;
        }
        private void SetPortionWeightAndUnit(FNSR01Calculator foodNutSR)
        {
            if (foodNutSR.WeightToUseType == FNSR01Calculator.WEIGHT_TOUSE_TYPES.weight2.ToString())
            {
                //smaller portion than weight 1
                foodNutSR.ServingSizeUnit = foodNutSR.TypWt2_Unit;
                foodNutSR.TypicalServingSize = foodNutSR.TypWt2_Amount;
            }
            else if (foodNutSR.WeightToUseType == FNSR01Calculator.WEIGHT_TOUSE_TYPES.weight2metric.ToString())
            {
                foodNutSR.ServingSizeUnit = "gram";
                foodNutSR.TypicalServingSize = foodNutSR.GmWt_2;
            }
            else if (foodNutSR.WeightToUseType == FNSR01Calculator.WEIGHT_TOUSE_TYPES.weight1metric.ToString())
            {
                foodNutSR.ServingSizeUnit = "gram";
                foodNutSR.TypicalServingSize = foodNutSR.GmWt_1;
            }
            else
            {
                //weight1 is default
                foodNutSR.WeightToUseType = FNSR01Calculator.WEIGHT_TOUSE_TYPES.weight1.ToString();
                foodNutSR.ServingSizeUnit = foodNutSR.TypWt1_Unit;
                foodNutSR.TypicalServingSize = foodNutSR.TypWt1_Amount;
            }
        }
        
        private void SetFoodNutritionInputCosts(
            CalculatorParameters calcParameters, ref FNSR01Calculator foodNutSR)
        {
            //check illegal divisors
            foodNutSR.ContainerSizeUsingServingSizeUnit = (foodNutSR.ContainerSizeUsingServingSizeUnit == 0)
                ? -1 : foodNutSR.ContainerSizeUsingServingSizeUnit;
            foodNutSR.TypicalServingsPerContainer = foodNutSR.ContainerSizeUsingServingSizeUnit / foodNutSR.TypicalServingSize;
            foodNutSR.ActualServingsPerContainer = foodNutSR.ContainerSizeUsingServingSizeUnit / foodNutSR.ActualServingSize;
            //keep this calculator simple
            //update these input fields automatically in the db
            //(they just insert caprice in input)
            //serving size = ocamount
            //if actual serving size is 1 slice of bread, but a sandwich, use the input.times multiplier to adjust to 2 slices
            foodNutSR.OCAmount = foodNutSR.ActualServingSize;
            //calculate OCPrice as a unit cost
            foodNutSR.OCPrice = foodNutSR.MarketValue / foodNutSR.ContainerSizeUsingServingSizeUnit;
            //set ocunit
            foodNutSR.OCUnit = foodNutSR.ServingSizeUnit;
            //calculate cost per actual serving
            //note that this can change when the input is added elsewhere
            foodNutSR.ServingCost = foodNutSR.OCPrice * foodNutSR.OCAmount;
            foodNutSR.TotalOC = foodNutSR.ServingCost;
        }
    }
}
