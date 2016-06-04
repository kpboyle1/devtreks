using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a food nutrition USDA standard reference 
    ///             object with properties derived from the USDA ARS Standard Reference 
    ///             Food Nutrition databases.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        These support unit input and output nutrients. The Q must be set in 
    ///             the Op/Comp/Outcome.
    public class MNSR1 : CostBenefitCalculator
    {
        public MNSR1()
            : base()
        {
            //food fact
            InitMNSR1Properties();
        }
        //copy constructor
        public MNSR1(MNSR1 foodSR)
            : base(foodSR)
        {
            //food fact
            CopyMNSR1Properties(foodSR);
        }
        public enum WEIGHT_TOUSE_TYPES
        {
            //food nutrition db wt1
            weight1         = 1,
            weight1metric   = 2,
            //wt2
            weight2         = 3,
            weight2metric   = 4
        }

        //**********************************
        //the instructions will state that the actual nutrient values
        //should be calculated as unit costs and unit nutrients that are based on 
        //1 unit of actual serving size (i.e. slice bread, tablespoon butter)
        //the input.OCUnit should be set to the actual serving size unit
        //input.Amount should default to 1 (always) and allow 
        //to be changed when the input is added to an op/comp (i.e. 2 slices bread)
        //input.Times just an extra multiplier
        //*********************************
        
        //calculator properties
        //container price used to set capprice
        public double ContainerPrice { get; set; }
        //container size in server size units (can convert container size to servings per container)
        public double ContainerSizeInSSUnits { get; set; }
        //ocprice = caprice / containersize
        //ocamount = actualservingsize
        //servingcost (occost) = ocamount * ocprice
        public double ServingCost { get; set; }
        //ocamount (must be entered manually)
        public double ActualServingSize { get; set; }
        //derived from typical serving size
        public double TypicalServingsPerContainer { get; set; }
        public double ActualServingsPerContainer { get; set; }
        //wt_1, wt_2, metric (grams)
        public string WeightToUseType { get; set; }
        //derived from weighttousetype (weight1, weight2, metric)
        public double TypicalServingSize { get; set; }
        //ocunit = derived from weighttousetype (weight1, weight2, metric)
        public string ServingSizeUnit { get; set; }
        //cap unit
        public string ContainerUnit { get; set; }
        //standard ref label and description
        public string SRLabel { get; set; }
        public string SRDescription { get; set; }
        //placeholder because analyses need summary nutrients for display and communication
        public double FNIndex { get; set; }
        //comparisons to display
        public double FNCount { get; set; }

        //decided only practical way to use constant is for each food input
        //so oatmeal would need 10 separate calcs for each gender group
        //which would require subnutrients 
        //potential for different calculator but too complicated for unit calcs
        //constants: age-gender group from App 5 USDA Nutritional Recommendations
        //those recommended levels are used to generate the  GoalPC (food nutrient goal percent) numbers
        //public string FoodGoalType { get; set; }

        //food nutritional values
        public double Water_g { get; set; }
        public double Energ_Kcal { get; set; }
        public double Protein_g { get; set; }
        public double Lipid_Tot_g { get; set; }
        public double Ash_g { get; set; }
        public double Carbohydrt_g { get; set; }
        public double Fiber_TD_g { get; set; }
        public double Sugar_Tot_g { get; set; }
        public double Calcium_mg { get; set; }
        public double Iron_mg { get; set; }
        public double Magnesium_mg { get; set; }
        public double Phosphorus_mg { get; set; }
        public double Potassium_mg { get; set; }
        public double Sodium_mg { get; set; }
        public double Zinc_mg { get; set; }
        public double Copper_mg { get; set; }
        public double Manganese_mg { get; set; }
        public double Selenium_pg { get; set; }
        public double Vit_C_mg { get; set; }
        public double Thiamin_mg { get; set; }
        public double Riboflavin_mg { get; set; }
        public double Niacin_mg { get; set; }
        public double Panto_Acid_mg { get; set; }
        public double Vit_B6_mg { get; set; }
        public double Folate_Tot_pg { get; set; }
        public double Folic_Acid_pg { get; set; }
        public double Food_Folate_pg { get; set; }
        public double Folate_DFE_pg { get; set; }
        public double Choline_Tot_mg { get; set; }
        public double Vit_B12_pg { get; set; }
        public double Vit_A_IU { get; set; }
        public double Vit_A_RAE { get; set; }
        public double Retinol_pg { get; set; }
        public double Alpha_Carot_pg { get; set; }
        public double Beta_Carot_pg { get; set; }
        public double Beta_Crypt_pg { get; set; }
        public double Lycopene_pg { get; set; }
        public double Lut_Zea_pg { get; set; }
        public double Vit_E_mg { get; set; }
        public double Vit_D_pg { get; set; }
        public double ViVit_D_IU { get; set; }
        public double Vit_K_pg { get; set; }
        public double FA_Sat_g { get; set; }
        public double FA_Mono_g { get; set; }
        public double FA_Poly_g { get; set; }
        public double Cholestrl_mg { get; set; }
        public double Extra1 { get; set; }
        public double Extra2 { get; set; }

        public double ActualWater_g { get; set; }
        public double ActualEnerg_Kcal { get; set; }
        public double ActualProtein_g { get; set; }
        public double ActualLipid_Tot_g { get; set; }
        public double ActualAsh_g { get; set; }
        public double ActualCarbohydrt_g { get; set; }
        public double ActualFiber_TD_g { get; set; }
        public double ActualSugar_Tot_g { get; set; }
        public double ActualCalcium_mg { get; set; }
        public double ActualIron_mg { get; set; }
        public double ActualMagnesium_mg { get; set; }
        public double ActualPhosphorus_mg { get; set; }
        public double ActualPotassium_mg { get; set; }
        public double ActualSodium_mg { get; set; }
        public double ActualZinc_mg { get; set; }
        public double ActualCopper_mg { get; set; }
        public double ActualManganese_mg { get; set; }
        public double ActualSelenium_pg { get; set; }
        public double ActualVit_C_mg { get; set; }
        public double ActualThiamin_mg { get; set; }
        public double ActualRiboflavin_mg { get; set; }
        public double ActualNiacin_mg { get; set; }
        public double ActualPanto_Acid_mg { get; set; }
        public double ActualVit_B6_mg { get; set; }
        public double ActualFolate_Tot_pg { get; set; }
        public double ActualFolic_Acid_pg { get; set; }
        public double ActualFood_Folate_pg { get; set; }
        public double ActualFolate_DFE_pg { get; set; }
        public double ActualCholine_Tot_mg { get; set; }
        public double ActualVit_B12_pg { get; set; }
        public double ActualVit_A_IU { get; set; }
        public double ActualVit_A_RAE { get; set; }
        public double ActualRetinol_pg { get; set; }
        public double ActualAlpha_Carot_pg { get; set; }
        public double ActualBeta_Carot_pg { get; set; }
        public double ActualBeta_Crypt_pg { get; set; }
        public double ActualLycopene_pg { get; set; }
        public double ActualLut_Zea_pg { get; set; }
        public double ActualVit_E_mg { get; set; }
        public double ActualVit_D_pg { get; set; }
        public double ActualViVit_D_IU { get; set; }
        public double ActualVit_K_pg { get; set; }
        public double ActualFA_Sat_g { get; set; }
        public double ActualFA_Mono_g { get; set; }
        public double ActualFA_Poly_g { get; set; }
        public double ActualCholestrl_mg { get; set; }
        public double ActualExtra1 { get; set; }
        public double ActualExtra2 { get; set; }

        //typical serving size (i.e. household portion)
        public double GmWt_1 { get; set; }
        public double TypWt1_Amount { get; set; }
        public string TypWt1_Unit { get; set; }
        //smaller typical serving size 
        public double GmWt_2 { get; set; }
        public double TypWt2_Amount { get; set; }
        public string TypWt2_Unit { get; set; }
        //1 pound of steak hh measure will have refuse that reduces food nutrients consumed
        public double Refuse_Pct { get; set; }

        public const string cContainerPrice = "ContainerPrice";
        public const string cContainerSizeInSSUnits = "ContainerSizeInSSUnits";
        public const string cServingCost = "ServingCost";
        public const string cActualServingSize = "ActualServingSize";
        public const string cWeightToUseType = "WeightToUseType";
        public const string cTypicalServingSize = "TypicalServingSize";
        public const string cServingSizeUnit = "ServingSizeUnit";
        public const string cContainerUnit = "ContainerUnit";
        public const string cSRLabel = "SRLabel";
        public const string cSRDescription = "SRDescription";
        public const string cTypicalServingsPerContainer = "TypicalServingsPerContainer";
        public const string cActualServingsPerContainer = "ActualServingsPerContainer";

        public const string cWater_g = "Water_g";
        public const string cEnerg_Kcal = "Energ_Kcal";
        public const string cProtein_g = "Protein_g";
        public const string cLipid_Tot_g = "Lipid_Tot_g";
        public const string cAsh_g = "Ash_g";
        public const string cCarbohydrt_g = "Carbohydrt_g";
        public const string cFiber_TD_g = "Fiber_TD_g";
        public const string cSugar_Tot_g = "Sugar_Tot_g";
        public const string cCalcium_mg = "Calcium_mg";
        public const string cIron_mg = "Iron_mg";
        public const string cMagnesium_mg = "Magnesium_mg";
        public const string cPhosphorus_mg = "Phosphorus_mg";
        public const string cPotassium_mg = "Potassium_mg";
        public const string cSodium_mg = "Sodium_mg";
        public const string cZinc_mg = "Zinc_mg";
        public const string cCopper_mg = "Copper_mg";
        public const string cManganese_mg = "Manganese_mg";
        public const string cSelenium_pg = "Selenium_pg";
        public const string cVit_C_mg = "Vit_C_mg";
        public const string cThiamin_mg = "Thiamin_mg";
        public const string cRiboflavin_mg = "Riboflavin_mg";
        public const string cNiacin_mg = "Niacin_mg";
        public const string cPanto_Acid_mg = "Panto_Acid_mg";
        public const string cVit_B6_mg = "Vit_B6_mg";
        public const string cFolate_Tot_pg = "Folate_Tot_pg";
        public const string cFolic_Acid_pg = "Folic_Acid_pg";
        public const string cFood_Folate_pg = "Food_Folate_pg";
        public const string cFolate_DFE_pg = "Folate_DFE_pg";
        public const string cCholine_Tot_mg = "Choline_Tot_mg";
        public const string cVit_B12_pg = "Vit_B12_pg";
        public const string cVit_A_IU = "Vit_A_IU";
        public const string cVit_A_RAE = "Vit_A_RAE";
        public const string cRetinol_pg = "Retinol_pg";
        public const string cAlpha_Carot_pg = "Alpha_Carot_pg";
        public const string cBeta_Carot_pg = "Beta_Carot_pg";
        public const string cBeta_Crypt_pg = "Beta_Crypt_pg";
        public const string cLycopene_pg = "Lycopene_pg";
        public const string cLut_Zea_pg = "Lut_Zea_pg";
        public const string cVit_E_mg = "Vit_E_mg";
        public const string cVit_D_pg = "Vit_D_pg";
        public const string cViVit_D_IU = "ViVit_D_IU";
        public const string cVit_K_pg = "Vit_K_pg";
        public const string cFA_Sat_g = "FA_Sat_g";
        public const string cFA_Mono_g = "FA_Mono_g";
        public const string cFA_Poly_g = "FA_Poly_g";
        public const string cCholestrl_mg = "Cholestrl_mg";
        public const string cExtra1 = "Extra1";
        public const string cExtra2 = "Extra2";

        public const string cFNIndex = "FNIndex";
        public const string cFNCount = "FNCount";

        private const string cActualWater_g = "ActualWater_g";
        private const string cActualEnerg_Kcal = "ActualEnerg_Kcal";
        private const string cActualProtein_g = "ActualProtein_g";
        private const string cActualLipid_Tot_g = "ActualLipid_Tot_g";
        private const string cActualAsh_g = "ActualAsh_g";
        private const string cActualCarbohydrt_g = "ActualCarbohydrt_g";
        private const string cActualFiber_TD_g = "ActualFiber_TD_g";
        private const string cActualSugar_Tot_g = "ActualSugar_Tot_g";
        private const string cActualCalcium_mg = "ActualCalcium_mg";
        private const string cActualIron_mg = "ActualIron_mg";
        private const string cActualMagnesium_mg = "ActualMagnesium_mg";
        private const string cActualPhosphorus_mg = "ActualPhosphorus_mg";
        private const string cActualPotassium_mg = "ActualPotassium_mg";
        private const string cActualSodium_mg = "ActualSodium_mg";
        private const string cActualZinc_mg = "ActualZinc_mg";
        private const string cActualCopper_mg = "ActualCopper_mg";
        private const string cActualManganese_mg = "ActualManganese_mg";
        private const string cActualSelenium_pg = "ActualSelenium_pg";
        private const string cActualVit_C_mg = "ActualVit_C_mg";
        private const string cActualThiamin_mg = "ActualThiamin_mg";
        private const string cActualRiboflavin_mg = "ActualRiboflavin_mg";
        private const string cActualNiacin_mg = "ActualNiacin_mg";
        private const string cActualPanto_Acid_mg = "ActualPanto_Acid_mg";
        private const string cActualVit_B6_mg = "ActualVit_B6_mg";
        private const string cActualFolate_Tot_pg = "ActualFolate_Tot_pg";
        private const string cActualFolic_Acid_pg = "ActualFolic_Acid_pg";
        private const string cActualFood_Folate_pg = "ActualFood_Folate_pg";
        private const string cActualFolate_DFE_pg = "ActualFolate_DFE_pg";
        private const string cActualCholine_Tot_mg = "ActualCholine_Tot_mg";
        private const string cActualVit_B12_pg = "ActualVit_B12_pg";
        private const string cActualVit_A_IU = "ActualVit_A_IU";
        private const string cActualVit_A_RAE = "ActualVit_A_RAE";
        private const string cActualRetinol_pg = "ActualRetinol_pg";
        private const string cActualAlpha_Carot_pg = "ActualAlpha_Carot_pg";
        private const string cActualBeta_Carot_pg = "ActualBeta_Carot_pg";
        private const string cActualBeta_Crypt_pg = "ActualBeta_Crypt_pg";
        private const string cActualLycopene_pg = "ActualLycopene_pg";
        private const string cActualLut_Zea_pg = "ActualLut_Zea_pg";
        private const string cActualVit_E_mg = "ActualVit_E_mg";
        private const string cActualVit_D_pg = "ActualVit_D_pg";
        private const string cActualViVit_D_IU = "ActualViVit_D_IU";
        private const string cActualVit_K_pg = "ActualVit_K_pg";
        private const string cActualFA_Sat_g = "ActualFA_Sat_g";
        private const string cActualFA_Mono_g = "ActualFA_Mono_g";
        private const string cActualFA_Poly_g = "ActualFA_Poly_g";
        private const string cActualCholestrl_mg = "ActualCholestrl_mg";
        private const string cActualExtra1 = "ActualExtra1";
        private const string cActualExtra2 = "ActualExtra2";

        private const string cGmWt_1 = "GmWt_1";
        private const string cTypWt1_Amount = "TypWt1_Amount";
        private const string cTypWt1_Unit = "TypWt1_Unit";
        private const string cGmWt_2 = "GmWt_2";
        private const string cTypWt2_Amount = "TypWt2_Amount";
        private const string cTypWt2_Unit = "TypWt2_Unit";
        private const string cRefuse_Pct = "Refuse_Pct";

        public virtual void InitMNSR1Properties()
        {
            //avoid null references to properties
            this.ContainerPrice = 0;
            this.ContainerSizeInSSUnits = 0;
            this.ServingCost = 0;
            this.ActualServingSize = 0;
            this.TypicalServingsPerContainer = 0;
            this.ActualServingsPerContainer = 0;
            this.WeightToUseType = string.Empty;
            this.TypicalServingSize = 0;
            this.ServingSizeUnit = string.Empty;
            this.ContainerUnit = string.Empty;
            this.SRLabel = string.Empty;
            this.SRDescription = string.Empty;
            this.FNIndex = 0;

            this.Water_g = 0;
            this.Energ_Kcal = 0;
            this.Protein_g = 0;
            this.Lipid_Tot_g = 0;
            this.Ash_g = 0;
            this.Carbohydrt_g = 0;
            this.Fiber_TD_g = 0;
            this.Sugar_Tot_g = 0;
            this.Calcium_mg = 0;
            this.Iron_mg = 0;
            this.Magnesium_mg = 0;
            this.Phosphorus_mg = 0;
            this.Potassium_mg = 0;
            this.Sodium_mg = 0;
            this.Zinc_mg = 0;
            this.Copper_mg = 0;
            this.Manganese_mg = 0;
            this.Selenium_pg = 0;
            this.Vit_C_mg = 0;
            this.Thiamin_mg = 0;
            this.Riboflavin_mg = 0;
            this.Niacin_mg = 0;
            this.Panto_Acid_mg = 0;
            this.Vit_B6_mg = 0;
            this.Folate_Tot_pg = 0;
            this.Folic_Acid_pg = 0;
            this.Food_Folate_pg = 0;
            this.Folate_DFE_pg = 0;
            this.Choline_Tot_mg = 0;
            this.Vit_B12_pg = 0;
            this.Vit_A_IU = 0;
            this.Vit_A_RAE = 0;
            this.Retinol_pg = 0;
            this.Alpha_Carot_pg = 0;
            this.Beta_Carot_pg = 0;
            this.Beta_Crypt_pg = 0;
            this.Lycopene_pg = 0;
            this.Lut_Zea_pg = 0;
            this.Vit_E_mg = 0;
            this.Vit_D_pg = 0;
            this.ViVit_D_IU = 0;
            this.Vit_K_pg = 0;
            this.FA_Sat_g = 0;
            this.FA_Mono_g = 0;
            this.FA_Poly_g = 0;
            this.Cholestrl_mg = 0;
            this.Extra1 = 0;
            this.Extra2 = 0;

            this.ActualWater_g = 0;
            this.ActualEnerg_Kcal = 0;
            this.ActualProtein_g = 0;
            this.ActualLipid_Tot_g = 0;
            this.ActualAsh_g = 0;
            this.ActualCarbohydrt_g = 0;
            this.ActualFiber_TD_g = 0;
            this.ActualSugar_Tot_g = 0;
            this.ActualCalcium_mg = 0;
            this.ActualIron_mg = 0;
            this.ActualMagnesium_mg = 0;
            this.ActualPhosphorus_mg = 0;
            this.ActualPotassium_mg = 0;
            this.ActualSodium_mg = 0;
            this.ActualZinc_mg = 0;
            this.ActualCopper_mg = 0;
            this.ActualManganese_mg = 0;
            this.ActualSelenium_pg = 0;
            this.ActualVit_C_mg = 0;
            this.ActualThiamin_mg = 0;
            this.ActualRiboflavin_mg = 0;
            this.ActualNiacin_mg = 0;
            this.ActualPanto_Acid_mg = 0;
            this.ActualVit_B6_mg = 0;
            this.ActualFolate_Tot_pg = 0;
            this.ActualFolic_Acid_pg = 0;
            this.ActualFood_Folate_pg = 0;
            this.ActualFolate_DFE_pg = 0;
            this.ActualCholine_Tot_mg = 0;
            this.ActualVit_B12_pg = 0;
            this.ActualVit_A_IU = 0;
            this.ActualVit_A_RAE = 0;
            this.ActualRetinol_pg = 0;
            this.ActualAlpha_Carot_pg = 0;
            this.ActualBeta_Carot_pg = 0;
            this.ActualBeta_Crypt_pg = 0;
            this.ActualLycopene_pg = 0;
            this.ActualLut_Zea_pg = 0;
            this.ActualVit_E_mg = 0;
            this.ActualVit_D_pg = 0;
            this.ActualViVit_D_IU = 0;
            this.ActualVit_K_pg = 0;
            this.ActualFA_Sat_g = 0;
            this.ActualFA_Mono_g = 0;
            this.ActualFA_Poly_g = 0;
            this.ActualCholestrl_mg = 0;
            this.ActualExtra1 = 0;
            this.ActualExtra2 = 0;

            this.GmWt_1 = 0;
            this.TypWt1_Amount = 0;
            this.TypWt1_Unit = string.Empty;
            this.GmWt_2 = 0;
            this.TypWt2_Amount = 0;
            this.TypWt2_Unit = string.Empty;
            this.Refuse_Pct = 0;
        }

        public virtual void CopyMNSR1Properties(
            MNSR1 calculator)
        {
            this.ContainerPrice = calculator.ContainerPrice;
            this.ContainerSizeInSSUnits = calculator.ContainerSizeInSSUnits;
            this.ServingCost = calculator.ServingCost;
            this.ActualServingSize = calculator.ActualServingSize;
            this.TypicalServingsPerContainer = calculator.TypicalServingsPerContainer;
            this.ActualServingsPerContainer = calculator.ActualServingsPerContainer;
            this.WeightToUseType = calculator.WeightToUseType;
            this.TypicalServingSize = calculator.TypicalServingSize;
            this.ServingSizeUnit = calculator.ServingSizeUnit;
            this.ContainerUnit = calculator.ContainerUnit;
            this.SRLabel = calculator.SRLabel;
            this.SRDescription = calculator.SRDescription;
            this.FNIndex = calculator.FNIndex;

            this.Water_g = calculator.Water_g;
            this.Energ_Kcal = calculator.Energ_Kcal;
            this.Protein_g = calculator.Protein_g;
            this.Lipid_Tot_g = calculator.Lipid_Tot_g;
            this.Ash_g = calculator.Ash_g;
            this.Carbohydrt_g = calculator.Carbohydrt_g;
            this.Fiber_TD_g = calculator.Fiber_TD_g;
            this.Sugar_Tot_g = calculator.Sugar_Tot_g;
            this.Calcium_mg = calculator.Calcium_mg;
            this.Iron_mg = calculator.Iron_mg;
            this.Magnesium_mg = calculator.Magnesium_mg;
            this.Phosphorus_mg = calculator.Phosphorus_mg;
            this.Potassium_mg = calculator.Potassium_mg;
            this.Sodium_mg = calculator.Sodium_mg;
            this.Zinc_mg = calculator.Zinc_mg;
            this.Copper_mg = calculator.Copper_mg;
            this.Manganese_mg = calculator.Manganese_mg;
            this.Selenium_pg = calculator.Selenium_pg;
            this.Vit_C_mg = calculator.Vit_C_mg;
            this.Thiamin_mg = calculator.Thiamin_mg;
            this.Riboflavin_mg = calculator.Riboflavin_mg;
            this.Niacin_mg = calculator.Niacin_mg;
            this.Panto_Acid_mg = calculator.Panto_Acid_mg;
            this.Vit_B6_mg = calculator.Vit_B6_mg;
            this.Folate_Tot_pg = calculator.Folate_Tot_pg;
            this.Folic_Acid_pg = calculator.Folic_Acid_pg;
            this.Food_Folate_pg = calculator.Food_Folate_pg;
            this.Folate_DFE_pg = calculator.Folate_DFE_pg;
            this.Choline_Tot_mg = calculator.Choline_Tot_mg;
            this.Vit_B12_pg = calculator.Vit_B12_pg;
            this.Vit_A_IU = calculator.Vit_A_IU;
            this.Vit_A_RAE = calculator.Vit_A_RAE;
            this.Retinol_pg = calculator.Retinol_pg;
            this.Alpha_Carot_pg = calculator.Alpha_Carot_pg;
            this.Beta_Carot_pg = calculator.Beta_Carot_pg;
            this.Beta_Crypt_pg = calculator.Beta_Crypt_pg;
            this.Lycopene_pg = calculator.Lycopene_pg;
            this.Lut_Zea_pg = calculator.Lut_Zea_pg;
            this.Vit_E_mg = calculator.Vit_E_mg;
            this.Vit_D_pg = calculator.Vit_D_pg;
            this.ViVit_D_IU = calculator.ViVit_D_IU;
            this.Vit_K_pg = calculator.Vit_K_pg;
            this.FA_Sat_g = calculator.FA_Sat_g;
            this.FA_Mono_g = calculator.FA_Mono_g;
            this.FA_Poly_g = calculator.FA_Poly_g;
            this.Cholestrl_mg = calculator.Cholestrl_mg;
            this.Extra1 = calculator.Extra1;
            this.Extra2 = calculator.Extra2;

            this.ActualWater_g = calculator.ActualWater_g;
            this.ActualEnerg_Kcal = calculator.ActualEnerg_Kcal;
            this.ActualProtein_g = calculator.ActualProtein_g;
            this.ActualLipid_Tot_g = calculator.ActualLipid_Tot_g;
            this.ActualAsh_g = calculator.ActualAsh_g;
            this.ActualCarbohydrt_g = calculator.ActualCarbohydrt_g;
            this.ActualFiber_TD_g = calculator.ActualFiber_TD_g;
            this.ActualSugar_Tot_g = calculator.ActualSugar_Tot_g;
            this.ActualCalcium_mg = calculator.ActualCalcium_mg;
            this.ActualIron_mg = calculator.ActualIron_mg;
            this.ActualMagnesium_mg = calculator.ActualMagnesium_mg;
            this.ActualPhosphorus_mg = calculator.ActualPhosphorus_mg;
            this.ActualPotassium_mg = calculator.ActualPotassium_mg;
            this.ActualSodium_mg = calculator.ActualSodium_mg;
            this.ActualZinc_mg = calculator.ActualZinc_mg;
            this.ActualCopper_mg = calculator.ActualCopper_mg;
            this.ActualManganese_mg = calculator.ActualManganese_mg;
            this.ActualSelenium_pg = calculator.ActualSelenium_pg;
            this.ActualVit_C_mg = calculator.ActualVit_C_mg;
            this.ActualThiamin_mg = calculator.ActualThiamin_mg;
            this.ActualRiboflavin_mg = calculator.ActualRiboflavin_mg;
            this.ActualNiacin_mg = calculator.ActualNiacin_mg;
            this.ActualPanto_Acid_mg = calculator.ActualPanto_Acid_mg;
            this.ActualVit_B6_mg = calculator.ActualVit_B6_mg;
            this.ActualFolate_Tot_pg = calculator.ActualFolate_Tot_pg;
            this.ActualFolic_Acid_pg = calculator.ActualFolic_Acid_pg;
            this.ActualFood_Folate_pg = calculator.ActualFood_Folate_pg;
            this.ActualFolate_DFE_pg = calculator.ActualFolate_DFE_pg;
            this.ActualCholine_Tot_mg = calculator.ActualCholine_Tot_mg;
            this.ActualVit_B12_pg = calculator.ActualVit_B12_pg;
            this.ActualVit_A_IU = calculator.ActualVit_A_IU;
            this.ActualVit_A_RAE = calculator.ActualVit_A_RAE;
            this.ActualRetinol_pg = calculator.ActualRetinol_pg;
            this.ActualAlpha_Carot_pg = calculator.ActualAlpha_Carot_pg;
            this.ActualBeta_Carot_pg = calculator.ActualBeta_Carot_pg;
            this.ActualBeta_Crypt_pg = calculator.ActualBeta_Crypt_pg;
            this.ActualLycopene_pg = calculator.ActualLycopene_pg;
            this.ActualLut_Zea_pg = calculator.ActualLut_Zea_pg;
            this.ActualVit_E_mg = calculator.ActualVit_E_mg;
            this.ActualVit_D_pg = calculator.ActualVit_D_pg;
            this.ActualViVit_D_IU = calculator.ActualViVit_D_IU;
            this.ActualVit_K_pg = calculator.ActualVit_K_pg;
            this.ActualFA_Sat_g = calculator.ActualFA_Sat_g;
            this.ActualFA_Mono_g = calculator.ActualFA_Mono_g;
            this.ActualFA_Poly_g = calculator.ActualFA_Poly_g;
            this.ActualCholestrl_mg = calculator.ActualCholestrl_mg;
            this.ActualExtra1 = calculator.ActualExtra1;
            this.ActualExtra2 = calculator.ActualExtra2;

            this.GmWt_1 = calculator.GmWt_1;
            this.TypWt1_Amount = calculator.TypWt1_Amount;
            this.TypWt1_Unit = calculator.TypWt1_Unit;
            this.GmWt_2 = calculator.GmWt_2;
            this.TypWt2_Amount = calculator.TypWt2_Amount;
            this.TypWt2_Unit = calculator.TypWt2_Unit;
            this.Refuse_Pct = calculator.Refuse_Pct;
        }
        public virtual void SetMNSR1Properties(CalculatorParameters calcParameters,
            XElement calculator, XElement currentElement)
        {
            SetCalculatorProperties(calculator);
            SetMNSR1Properties(calculator);
        }
        //set the class properties using the XElement
        public virtual void SetMNSR1Properties(XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            this.ContainerPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cContainerPrice);
            this.ContainerSizeInSSUnits = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cContainerSizeInSSUnits);
            this.ServingCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cServingCost);
            this.ActualServingSize = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualServingSize);
            this.TypicalServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTypicalServingsPerContainer);
            this.ActualServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualServingsPerContainer);
            this.WeightToUseType = CalculatorHelpers.GetAttribute(currentCalculationsElement, cWeightToUseType);
            this.TypicalServingSize = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTypicalServingSize);
            this.ServingSizeUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cServingSizeUnit);
            this.ContainerUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cContainerUnit);
            this.SRLabel = CalculatorHelpers.GetAttribute(currentCalculationsElement, cSRLabel);
            this.SRDescription = CalculatorHelpers.GetAttribute(currentCalculationsElement, cSRDescription);

            this.Water_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cWater_g);
            this.Energ_Kcal = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEnerg_Kcal);
            this.Protein_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cProtein_g);
            this.Lipid_Tot_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cLipid_Tot_g);
            this.Ash_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAsh_g);
            this.Carbohydrt_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCarbohydrt_g);
            this.Fiber_TD_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFiber_TD_g);
            this.Sugar_Tot_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSugar_Tot_g);
            this.Calcium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCalcium_mg);
            this.Iron_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cIron_mg);
            this.Magnesium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cMagnesium_mg);
            this.Phosphorus_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPhosphorus_mg);
            this.Potassium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPotassium_mg);
            this.Sodium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSodium_mg);
            this.Zinc_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cZinc_mg);
            this.Copper_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCopper_mg);
            this.Manganese_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cManganese_mg);
            this.Selenium_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cSelenium_pg);
            this.Vit_C_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_C_mg);
            this.Thiamin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cThiamin_mg);
            this.Riboflavin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cRiboflavin_mg);
            this.Niacin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cNiacin_mg);
            this.Panto_Acid_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cPanto_Acid_mg);
            this.Vit_B6_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_B6_mg);
            this.Folate_Tot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFolate_Tot_pg);
            this.Folic_Acid_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFolic_Acid_pg);
            this.Food_Folate_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFood_Folate_pg);
            this.Folate_DFE_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFolate_DFE_pg);
            this.Choline_Tot_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCholine_Tot_mg);
            this.Vit_B12_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_B12_pg);
            this.Vit_A_IU = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_A_IU);
            this.Vit_A_RAE = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_A_RAE);
            this.Retinol_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cRetinol_pg);
            this.Alpha_Carot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAlpha_Carot_pg);
            this.Beta_Carot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBeta_Carot_pg);
            this.Beta_Crypt_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBeta_Crypt_pg);
            this.Lycopene_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cLycopene_pg);
            this.Lut_Zea_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cLut_Zea_pg);
            this.Vit_E_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_E_mg);
            this.Vit_D_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_D_pg);
            this.ViVit_D_IU = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cViVit_D_IU);
            this.Vit_K_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cVit_K_pg);
            this.FA_Sat_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFA_Sat_g);
            this.FA_Mono_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFA_Mono_g);
            this.FA_Poly_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFA_Poly_g);
            this.Cholestrl_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCholestrl_mg);
            this.Extra1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cExtra1);
            this.Extra2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cExtra2);

            this.ActualWater_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualWater_g);
            this.ActualEnerg_Kcal = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualEnerg_Kcal);
            this.ActualProtein_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualProtein_g);
            this.ActualLipid_Tot_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualLipid_Tot_g);
            this.ActualAsh_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualAsh_g);
            this.ActualCarbohydrt_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualCarbohydrt_g);
            this.ActualFiber_TD_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFiber_TD_g);
            this.ActualSugar_Tot_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualSugar_Tot_g);
            this.ActualCalcium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualCalcium_mg);
            this.ActualIron_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualIron_mg);
            this.ActualMagnesium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualMagnesium_mg);
            this.ActualPhosphorus_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualPhosphorus_mg);
            this.ActualPotassium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualPotassium_mg);
            this.ActualSodium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualSodium_mg);
            this.ActualZinc_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualZinc_mg);
            this.ActualCopper_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualCopper_mg);
            this.ActualManganese_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualManganese_mg);
            this.ActualSelenium_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualSelenium_pg);
            this.ActualVit_C_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_C_mg);
            this.ActualThiamin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualThiamin_mg);
            this.ActualRiboflavin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualRiboflavin_mg);
            this.ActualNiacin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualNiacin_mg);
            this.ActualPanto_Acid_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualPanto_Acid_mg);
            this.ActualVit_B6_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_B6_mg);
            this.ActualFolate_Tot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFolate_Tot_pg);
            this.ActualFolic_Acid_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFolic_Acid_pg);
            this.ActualFood_Folate_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFood_Folate_pg);
            this.ActualFolate_DFE_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFolate_DFE_pg);
            this.ActualCholine_Tot_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualCholine_Tot_mg);
            this.ActualVit_B12_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_B12_pg);
            this.ActualVit_A_IU = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_A_IU);
            this.ActualVit_A_RAE = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_A_RAE);
            this.ActualRetinol_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualRetinol_pg);
            this.ActualAlpha_Carot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualAlpha_Carot_pg);
            this.ActualBeta_Carot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualBeta_Carot_pg);
            this.ActualBeta_Crypt_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualBeta_Crypt_pg);
            this.ActualLycopene_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualLycopene_pg);
            this.ActualLut_Zea_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualLut_Zea_pg);
            this.ActualVit_E_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_E_mg);
            this.ActualVit_D_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_D_pg);
            this.ActualViVit_D_IU = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualViVit_D_IU);
            this.ActualVit_K_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualVit_K_pg);
            this.ActualFA_Sat_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFA_Sat_g);
            this.ActualFA_Mono_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFA_Mono_g);
            this.ActualFA_Poly_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualFA_Poly_g);
            this.ActualCholestrl_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualCholestrl_mg);
            this.ActualExtra1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualExtra1);
            this.ActualExtra2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cActualExtra2);

            this.GmWt_1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cGmWt_1);
            this.TypWt1_Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTypWt1_Amount);
            this.TypWt1_Unit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cTypWt1_Unit);
            this.GmWt_2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cGmWt_2);
            this.TypWt2_Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTypWt2_Amount);
            this.TypWt2_Unit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cTypWt2_Unit);
            this.Refuse_Pct = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cRefuse_Pct);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetMNSR1Property(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cContainerPrice:
                    this.ContainerPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cContainerSizeInSSUnits:
                    this.ContainerSizeInSSUnits = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cServingCost:
                    this.ServingCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualServingSize:
                    this.ActualServingSize = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTypicalServingsPerContainer:
                    this.TypicalServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualServingsPerContainer:
                    this.ActualServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWeightToUseType:
                    this.WeightToUseType = attValue;
                    break;
                case cTypicalServingSize:
                    this.TypicalServingSize = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cServingSizeUnit:
                    this.ServingSizeUnit = attValue;
                    break;
                case cContainerUnit:
                    this.ContainerUnit = attValue;
                    break;
                case cSRLabel:
                    this.SRLabel = attValue;
                    break;
                case cSRDescription:
                    this.SRDescription = attValue;
                    break;
                case cWater_g:
                    this.Water_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEnerg_Kcal:
                    this.Energ_Kcal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProtein_g:
                    this.Protein_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLipid_Tot_g:
                    this.Lipid_Tot_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAsh_g:
                    this.Ash_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCarbohydrt_g:
                    this.Carbohydrt_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFiber_TD_g:
                    this.Fiber_TD_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSugar_Tot_g:
                    this.Sugar_Tot_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCalcium_mg:
                    this.Calcium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIron_mg:
                    this.Iron_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMagnesium_mg:
                    this.Magnesium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPhosphorus_mg:
                    this.Phosphorus_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPotassium_mg:
                    this.Potassium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSodium_mg:
                    this.Sodium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cZinc_mg:
                    this.Zinc_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCopper_mg:
                    this.Copper_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cManganese_mg:
                    this.Manganese_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSelenium_pg:
                    this.Selenium_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_C_mg:
                    this.Vit_C_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cThiamin_mg:
                    this.Thiamin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRiboflavin_mg:
                    this.Riboflavin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cNiacin_mg:
                    this.Niacin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPanto_Acid_mg:
                    this.Panto_Acid_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_B6_mg:
                    this.Vit_B6_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFolate_Tot_pg:
                    this.Folate_Tot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFolic_Acid_pg:
                    this.Folic_Acid_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFood_Folate_pg:
                    this.Food_Folate_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFolate_DFE_pg:
                    this.Folate_DFE_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCholine_Tot_mg:
                    this.Choline_Tot_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_B12_pg:
                    this.Vit_B12_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_A_IU:
                    this.Vit_A_IU = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_A_RAE:
                    this.Vit_A_RAE = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRetinol_pg:
                    this.Retinol_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAlpha_Carot_pg:
                    this.Alpha_Carot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cBeta_Carot_pg:
                    this.Beta_Carot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cBeta_Crypt_pg:
                    this.Beta_Crypt_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLycopene_pg:
                    this.Lycopene_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLut_Zea_pg:
                    this.Lut_Zea_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_E_mg:
                    this.Vit_E_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_D_pg:
                    this.Vit_D_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cViVit_D_IU:
                    this.ViVit_D_IU = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVit_K_pg:
                    this.Vit_K_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFA_Sat_g:
                    this.FA_Sat_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFA_Mono_g:
                    this.FA_Mono_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFA_Poly_g:
                    this.FA_Poly_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCholestrl_mg:
                    this.Cholestrl_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cExtra1:
                    this.Extra1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cExtra2:
                    this.Extra2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualWater_g:
                    this.ActualWater_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualEnerg_Kcal:
                    this.ActualEnerg_Kcal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualProtein_g:
                    this.ActualProtein_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualLipid_Tot_g:
                    this.ActualLipid_Tot_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualAsh_g:
                    this.ActualAsh_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualCarbohydrt_g:
                    this.ActualCarbohydrt_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFiber_TD_g:
                    this.ActualFiber_TD_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualSugar_Tot_g:
                    this.ActualSugar_Tot_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualCalcium_mg:
                    this.ActualCalcium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualIron_mg:
                    this.ActualIron_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualMagnesium_mg:
                    this.ActualMagnesium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualPhosphorus_mg:
                    this.ActualPhosphorus_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualPotassium_mg:
                    this.ActualPotassium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualSodium_mg:
                    this.ActualSodium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualZinc_mg:
                    this.ActualZinc_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualCopper_mg:
                    this.ActualCopper_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualManganese_mg:
                    this.ActualManganese_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualSelenium_pg:
                    this.ActualSelenium_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_C_mg:
                    this.ActualVit_C_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualThiamin_mg:
                    this.ActualThiamin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualRiboflavin_mg:
                    this.ActualRiboflavin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualNiacin_mg:
                    this.ActualNiacin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualPanto_Acid_mg:
                    this.ActualPanto_Acid_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_B6_mg:
                    this.ActualVit_B6_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFolate_Tot_pg:
                    this.ActualFolate_Tot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFolic_Acid_pg:
                    this.ActualFolic_Acid_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFood_Folate_pg:
                    this.ActualFood_Folate_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFolate_DFE_pg:
                    this.ActualFolate_DFE_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualCholine_Tot_mg:
                    this.ActualCholine_Tot_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_B12_pg:
                    this.ActualVit_B12_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_A_IU:
                    this.ActualVit_A_IU = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_A_RAE:
                    this.ActualVit_A_RAE = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualRetinol_pg:
                    this.ActualRetinol_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualAlpha_Carot_pg:
                    this.ActualAlpha_Carot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualBeta_Carot_pg:
                    this.ActualBeta_Carot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualBeta_Crypt_pg:
                    this.ActualBeta_Crypt_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualLycopene_pg:
                    this.ActualLycopene_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualLut_Zea_pg:
                    this.ActualLut_Zea_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_E_mg:
                    this.ActualVit_E_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_D_pg:
                    this.ActualVit_D_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualViVit_D_IU:
                    this.ActualViVit_D_IU = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualVit_K_pg:
                    this.ActualVit_K_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFA_Sat_g:
                    this.ActualFA_Sat_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFA_Mono_g:
                    this.ActualFA_Mono_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualFA_Poly_g:
                    this.ActualFA_Poly_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualCholestrl_mg:
                    this.ActualCholestrl_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualExtra1:
                    this.ActualExtra1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cActualExtra2:
                    this.ActualExtra2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cGmWt_1:
                    this.GmWt_1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTypWt1_Amount:
                    this.TypWt1_Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTypWt1_Unit:
                    this.TypWt1_Unit = attValue;
                    break;
                case cGmWt_2:
                    this.GmWt_2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTypWt2_Amount:
                    this.TypWt2_Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTypWt2_Unit:
                    this.TypWt2_Unit = attValue;
                    break;
                case cRefuse_Pct:
                    this.Refuse_Pct = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetMNSR1Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cContainerPrice, attNameExtension), this.ContainerPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cContainerSizeInSSUnits, attNameExtension), this.ContainerSizeInSSUnits);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cServingCost, attNameExtension), this.ServingCost);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cWeightToUseType, attNameExtension), this.WeightToUseType);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(cTypicalServingSize, attNameExtension), this.TypicalServingSize);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cServingSizeUnit, attNameExtension), this.ServingSizeUnit);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cContainerUnit, attNameExtension), this.ContainerUnit);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cSRLabel, attNameExtension), this.SRLabel);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cSRDescription, attNameExtension), this.SRDescription);

            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(cActualServingSize, attNameExtension), this.ActualServingSize);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTypicalServingsPerContainer, attNameExtension), this.TypicalServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualServingsPerContainer, attNameExtension), this.ActualServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cWater_g, attNameExtension), this.Water_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cEnerg_Kcal, attNameExtension), this.Energ_Kcal);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cProtein_g, attNameExtension), this.Protein_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cLipid_Tot_g, attNameExtension), this.Lipid_Tot_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAsh_g, attNameExtension), this.Ash_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCarbohydrt_g, attNameExtension), this.Carbohydrt_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFiber_TD_g, attNameExtension), this.Fiber_TD_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSugar_Tot_g, attNameExtension), this.Sugar_Tot_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCalcium_mg, attNameExtension), this.Calcium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cIron_mg, attNameExtension), this.Iron_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cMagnesium_mg, attNameExtension), this.Magnesium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPhosphorus_mg, attNameExtension), this.Phosphorus_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPotassium_mg, attNameExtension), this.Potassium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSodium_mg, attNameExtension), this.Sodium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cZinc_mg, attNameExtension), this.Zinc_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCopper_mg, attNameExtension), this.Copper_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cManganese_mg, attNameExtension), this.Manganese_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cSelenium_pg, attNameExtension), this.Selenium_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_C_mg, attNameExtension), this.Vit_C_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cThiamin_mg, attNameExtension), this.Thiamin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cRiboflavin_mg, attNameExtension), this.Riboflavin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cNiacin_mg, attNameExtension), this.Niacin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cPanto_Acid_mg, attNameExtension), this.Panto_Acid_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_B6_mg, attNameExtension), this.Vit_B6_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFolate_Tot_pg, attNameExtension), this.Folate_Tot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFolic_Acid_pg, attNameExtension), this.Folic_Acid_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFood_Folate_pg, attNameExtension), this.Food_Folate_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFolate_DFE_pg, attNameExtension), this.Folate_DFE_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCholine_Tot_mg, attNameExtension), this.Choline_Tot_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_B12_pg, attNameExtension), this.Vit_B12_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_A_IU, attNameExtension), this.Vit_A_IU);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_A_RAE, attNameExtension), this.Vit_A_RAE);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cRetinol_pg, attNameExtension), this.Retinol_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAlpha_Carot_pg, attNameExtension), this.Alpha_Carot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cBeta_Carot_pg, attNameExtension), this.Beta_Carot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cBeta_Crypt_pg, attNameExtension), this.Beta_Crypt_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cLycopene_pg, attNameExtension), this.Lycopene_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cLut_Zea_pg, attNameExtension), this.Lut_Zea_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_E_mg, attNameExtension), this.Vit_E_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_D_pg, attNameExtension), this.Vit_D_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cViVit_D_IU, attNameExtension), this.ViVit_D_IU);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cVit_K_pg, attNameExtension), this.Vit_K_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFA_Sat_g, attNameExtension), this.FA_Sat_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFA_Mono_g, attNameExtension), this.FA_Mono_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFA_Poly_g, attNameExtension), this.FA_Poly_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCholestrl_mg, attNameExtension), this.Cholestrl_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cExtra1, attNameExtension), this.Extra1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cExtra2, attNameExtension), this.Extra2);


            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualWater_g, attNameExtension), this.ActualWater_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualEnerg_Kcal, attNameExtension), this.ActualEnerg_Kcal);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualProtein_g, attNameExtension), this.ActualProtein_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualLipid_Tot_g, attNameExtension), this.ActualLipid_Tot_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualAsh_g, attNameExtension), this.ActualAsh_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualCarbohydrt_g, attNameExtension), this.ActualCarbohydrt_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFiber_TD_g, attNameExtension), this.ActualFiber_TD_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualSugar_Tot_g, attNameExtension), this.ActualSugar_Tot_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualCalcium_mg, attNameExtension), this.ActualCalcium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualIron_mg, attNameExtension), this.ActualIron_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualMagnesium_mg, attNameExtension), this.ActualMagnesium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualPhosphorus_mg, attNameExtension), this.ActualPhosphorus_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualPotassium_mg, attNameExtension), this.ActualPotassium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualSodium_mg, attNameExtension), this.ActualSodium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualZinc_mg, attNameExtension), this.ActualZinc_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualCopper_mg, attNameExtension), this.ActualCopper_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualManganese_mg, attNameExtension), this.ActualManganese_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualSelenium_pg, attNameExtension), this.ActualSelenium_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_C_mg, attNameExtension), this.ActualVit_C_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualThiamin_mg, attNameExtension), this.ActualThiamin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualRiboflavin_mg, attNameExtension), this.ActualRiboflavin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualNiacin_mg, attNameExtension), this.ActualNiacin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualPanto_Acid_mg, attNameExtension), this.ActualPanto_Acid_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_B6_mg, attNameExtension), this.ActualVit_B6_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFolate_Tot_pg, attNameExtension), this.ActualFolate_Tot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFolic_Acid_pg, attNameExtension), this.ActualFolic_Acid_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFood_Folate_pg, attNameExtension), this.ActualFood_Folate_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFolate_DFE_pg, attNameExtension), this.ActualFolate_DFE_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualCholine_Tot_mg, attNameExtension), this.ActualCholine_Tot_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_B12_pg, attNameExtension), this.ActualVit_B12_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_A_IU, attNameExtension), this.ActualVit_A_IU);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_A_RAE, attNameExtension), this.ActualVit_A_RAE);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualRetinol_pg, attNameExtension), this.ActualRetinol_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualAlpha_Carot_pg, attNameExtension), this.ActualAlpha_Carot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualBeta_Carot_pg, attNameExtension), this.ActualBeta_Carot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualBeta_Crypt_pg, attNameExtension), this.ActualBeta_Crypt_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualLycopene_pg, attNameExtension), this.ActualLycopene_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualLut_Zea_pg, attNameExtension), this.ActualLut_Zea_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_E_mg, attNameExtension), this.ActualVit_E_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_D_pg, attNameExtension), this.ActualVit_D_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualViVit_D_IU, attNameExtension), this.ActualViVit_D_IU);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualVit_K_pg, attNameExtension), this.ActualVit_K_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFA_Sat_g, attNameExtension), this.ActualFA_Sat_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFA_Mono_g, attNameExtension), this.ActualFA_Mono_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualFA_Poly_g, attNameExtension), this.ActualFA_Poly_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualCholestrl_mg, attNameExtension), this.ActualCholestrl_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualExtra1, attNameExtension), this.ActualExtra1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cActualExtra2, attNameExtension), this.ActualExtra2);

            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cGmWt_1, attNameExtension), this.GmWt_1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cTypWt1_Amount, attNameExtension), this.TypWt1_Amount);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cTypWt1_Unit, attNameExtension), this.TypWt1_Unit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cGmWt_2, attNameExtension), this.GmWt_2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTypWt2_Amount, attNameExtension), this.TypWt2_Amount);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cTypWt2_Unit, attNameExtension), this.TypWt2_Unit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cRefuse_Pct, attNameExtension), this.Refuse_Pct);
        }
        public virtual void SetMNSR1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cContainerPrice, attNameExtension), this.ContainerPrice.ToString());
            writer.WriteAttributeString(string.Concat(cContainerSizeInSSUnits, attNameExtension), this.ContainerSizeInSSUnits.ToString());
            writer.WriteAttributeString(string.Concat(cServingCost, attNameExtension), this.ServingCost.ToString());
            writer.WriteAttributeString(string.Concat(cWeightToUseType, attNameExtension), this.WeightToUseType.ToString());
            writer.WriteAttributeString(string.Concat(cTypicalServingSize, attNameExtension), this.TypicalServingSize.ToString());
            writer.WriteAttributeString(string.Concat(cServingSizeUnit, attNameExtension), this.ServingSizeUnit.ToString());
            writer.WriteAttributeString(string.Concat(cContainerUnit, attNameExtension), this.ContainerUnit.ToString());
            writer.WriteAttributeString(string.Concat(cSRLabel, attNameExtension), this.SRLabel.ToString());
            writer.WriteAttributeString(string.Concat(cSRDescription, attNameExtension), this.SRDescription.ToString());

            //subprices needed

            writer.WriteAttributeString(string.Concat(cActualServingSize, attNameExtension), this.ActualServingSize.ToString());
            writer.WriteAttributeString(string.Concat(cTypicalServingsPerContainer, attNameExtension), this.TypicalServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(cActualServingsPerContainer, attNameExtension), this.ActualServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(cWater_g, attNameExtension), this.Water_g.ToString());
            writer.WriteAttributeString(string.Concat(cEnerg_Kcal, attNameExtension), this.Energ_Kcal.ToString());
            writer.WriteAttributeString(string.Concat(cProtein_g, attNameExtension), this.Protein_g.ToString());
            writer.WriteAttributeString(string.Concat(cLipid_Tot_g, attNameExtension), this.Lipid_Tot_g.ToString());
            writer.WriteAttributeString(string.Concat(cAsh_g, attNameExtension), this.Ash_g.ToString());
            writer.WriteAttributeString(string.Concat(cCarbohydrt_g, attNameExtension), this.Carbohydrt_g.ToString());
            writer.WriteAttributeString(string.Concat(cFiber_TD_g, attNameExtension), this.Fiber_TD_g.ToString());
            writer.WriteAttributeString(string.Concat(cSugar_Tot_g, attNameExtension), this.Sugar_Tot_g.ToString());
            writer.WriteAttributeString(string.Concat(cCalcium_mg, attNameExtension), this.Calcium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cIron_mg, attNameExtension), this.Iron_mg.ToString());
            writer.WriteAttributeString(string.Concat(cMagnesium_mg, attNameExtension), this.Magnesium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cPhosphorus_mg, attNameExtension), this.Phosphorus_mg.ToString());
            writer.WriteAttributeString(string.Concat(cPotassium_mg, attNameExtension), this.Potassium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cSodium_mg, attNameExtension), this.Sodium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cZinc_mg, attNameExtension), this.Zinc_mg.ToString());
            writer.WriteAttributeString(string.Concat(cCopper_mg, attNameExtension), this.Copper_mg.ToString());
            writer.WriteAttributeString(string.Concat(cManganese_mg, attNameExtension), this.Manganese_mg.ToString());
            writer.WriteAttributeString(string.Concat(cSelenium_pg, attNameExtension), this.Selenium_pg.ToString());
            writer.WriteAttributeString(string.Concat(cVit_C_mg, attNameExtension), this.Vit_C_mg.ToString());
            writer.WriteAttributeString(string.Concat(cThiamin_mg, attNameExtension), this.Thiamin_mg.ToString());
            writer.WriteAttributeString(string.Concat(cRiboflavin_mg, attNameExtension), this.Riboflavin_mg.ToString());
            writer.WriteAttributeString(string.Concat(cNiacin_mg, attNameExtension), this.Niacin_mg.ToString());
            writer.WriteAttributeString(string.Concat(cPanto_Acid_mg, attNameExtension), this.Panto_Acid_mg.ToString());
            writer.WriteAttributeString(string.Concat(cVit_B6_mg, attNameExtension), this.Vit_B6_mg.ToString());
            writer.WriteAttributeString(string.Concat(cFolate_Tot_pg, attNameExtension), this.Folate_Tot_pg.ToString());
            writer.WriteAttributeString(string.Concat(cFolic_Acid_pg, attNameExtension), this.Folic_Acid_pg.ToString());
            writer.WriteAttributeString(string.Concat(cFood_Folate_pg, attNameExtension), this.Food_Folate_pg.ToString());
            writer.WriteAttributeString(string.Concat(cFolate_DFE_pg, attNameExtension), this.Folate_DFE_pg.ToString());
            writer.WriteAttributeString(string.Concat(cCholine_Tot_mg, attNameExtension), this.Choline_Tot_mg.ToString());
            writer.WriteAttributeString(string.Concat(cVit_B12_pg, attNameExtension), this.Vit_B12_pg.ToString());
            writer.WriteAttributeString(string.Concat(cVit_A_IU, attNameExtension), this.Vit_A_IU.ToString());
            writer.WriteAttributeString(string.Concat(cVit_A_RAE, attNameExtension), this.Vit_A_RAE.ToString());
            writer.WriteAttributeString(string.Concat(cRetinol_pg, attNameExtension), this.Retinol_pg.ToString());
            writer.WriteAttributeString(string.Concat(cAlpha_Carot_pg, attNameExtension), this.Alpha_Carot_pg.ToString());
            writer.WriteAttributeString(string.Concat(cBeta_Carot_pg, attNameExtension), this.Beta_Carot_pg.ToString());
            writer.WriteAttributeString(string.Concat(cBeta_Crypt_pg, attNameExtension), this.Beta_Crypt_pg.ToString());
            writer.WriteAttributeString(string.Concat(cLycopene_pg, attNameExtension), this.Lycopene_pg.ToString());
            writer.WriteAttributeString(string.Concat(cLut_Zea_pg, attNameExtension), this.Lut_Zea_pg.ToString());
            writer.WriteAttributeString(string.Concat(cVit_E_mg, attNameExtension), this.Vit_E_mg.ToString());
            writer.WriteAttributeString(string.Concat(cVit_D_pg, attNameExtension), this.Vit_D_pg.ToString());
            writer.WriteAttributeString(string.Concat(cViVit_D_IU, attNameExtension), this.ViVit_D_IU.ToString());
            writer.WriteAttributeString(string.Concat(cVit_K_pg, attNameExtension), this.Vit_K_pg.ToString());
            writer.WriteAttributeString(string.Concat(cFA_Sat_g, attNameExtension), this.FA_Sat_g.ToString());
            writer.WriteAttributeString(string.Concat(cFA_Mono_g, attNameExtension), this.FA_Mono_g.ToString());
            writer.WriteAttributeString(string.Concat(cFA_Poly_g, attNameExtension), this.FA_Poly_g.ToString());
            writer.WriteAttributeString(string.Concat(cCholestrl_mg, attNameExtension), this.Cholestrl_mg.ToString());
            writer.WriteAttributeString(string.Concat(cExtra1, attNameExtension), this.Extra1.ToString());
            writer.WriteAttributeString(string.Concat(cExtra2, attNameExtension), this.Extra2.ToString());

            writer.WriteAttributeString(string.Concat(cActualWater_g, attNameExtension), this.ActualWater_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualEnerg_Kcal, attNameExtension), this.ActualEnerg_Kcal.ToString());
            writer.WriteAttributeString(string.Concat(cActualProtein_g, attNameExtension), this.ActualProtein_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualLipid_Tot_g, attNameExtension), this.ActualLipid_Tot_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualAsh_g, attNameExtension), this.ActualAsh_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualCarbohydrt_g, attNameExtension), this.ActualCarbohydrt_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualFiber_TD_g, attNameExtension), this.ActualFiber_TD_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualSugar_Tot_g, attNameExtension), this.ActualSugar_Tot_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualCalcium_mg, attNameExtension), this.ActualCalcium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualIron_mg, attNameExtension), this.ActualIron_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualMagnesium_mg, attNameExtension), this.ActualMagnesium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualPhosphorus_mg, attNameExtension), this.ActualPhosphorus_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualPotassium_mg, attNameExtension), this.ActualPotassium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualSodium_mg, attNameExtension), this.ActualSodium_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualZinc_mg, attNameExtension), this.ActualZinc_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualCopper_mg, attNameExtension), this.ActualCopper_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualManganese_mg, attNameExtension), this.ActualManganese_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualSelenium_pg, attNameExtension), this.ActualSelenium_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_C_mg, attNameExtension), this.ActualVit_C_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualThiamin_mg, attNameExtension), this.ActualThiamin_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualRiboflavin_mg, attNameExtension), this.ActualRiboflavin_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualNiacin_mg, attNameExtension), this.ActualNiacin_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualPanto_Acid_mg, attNameExtension), this.ActualPanto_Acid_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_B6_mg, attNameExtension), this.ActualVit_B6_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualFolate_Tot_pg, attNameExtension), this.ActualFolate_Tot_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualFolic_Acid_pg, attNameExtension), this.ActualFolic_Acid_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualFood_Folate_pg, attNameExtension), this.ActualFood_Folate_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualFolate_DFE_pg, attNameExtension), this.ActualFolate_DFE_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualCholine_Tot_mg, attNameExtension), this.ActualCholine_Tot_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_B12_pg, attNameExtension), this.ActualVit_B12_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_A_IU, attNameExtension), this.ActualVit_A_IU.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_A_RAE, attNameExtension), this.ActualVit_A_RAE.ToString());
            writer.WriteAttributeString(string.Concat(cActualRetinol_pg, attNameExtension), this.ActualRetinol_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualAlpha_Carot_pg, attNameExtension), this.ActualAlpha_Carot_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualBeta_Carot_pg, attNameExtension), this.ActualBeta_Carot_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualBeta_Crypt_pg, attNameExtension), this.ActualBeta_Crypt_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualLycopene_pg, attNameExtension), this.ActualLycopene_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualLut_Zea_pg, attNameExtension), this.ActualLut_Zea_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_E_mg, attNameExtension), this.ActualVit_E_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_D_pg, attNameExtension), this.ActualVit_D_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualViVit_D_IU, attNameExtension), this.ActualViVit_D_IU.ToString());
            writer.WriteAttributeString(string.Concat(cActualVit_K_pg, attNameExtension), this.ActualVit_K_pg.ToString());
            writer.WriteAttributeString(string.Concat(cActualFA_Sat_g, attNameExtension), this.ActualFA_Sat_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualFA_Mono_g, attNameExtension), this.ActualFA_Mono_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualFA_Poly_g, attNameExtension), this.ActualFA_Poly_g.ToString());
            writer.WriteAttributeString(string.Concat(cActualCholestrl_mg, attNameExtension), this.ActualCholestrl_mg.ToString());
            writer.WriteAttributeString(string.Concat(cActualExtra1, attNameExtension), this.ActualExtra1.ToString());
            writer.WriteAttributeString(string.Concat(cActualExtra2, attNameExtension), this.ActualExtra2.ToString());

            writer.WriteAttributeString(string.Concat(cGmWt_1, attNameExtension), this.GmWt_1.ToString());
            writer.WriteAttributeString(string.Concat(cTypWt1_Amount, attNameExtension), this.TypWt1_Amount.ToString());
            writer.WriteAttributeString(string.Concat(cTypWt1_Unit, attNameExtension), this.TypWt1_Unit);
            writer.WriteAttributeString(string.Concat(cGmWt_2, attNameExtension), this.GmWt_2.ToString());
            writer.WriteAttributeString(string.Concat(cTypWt2_Amount, attNameExtension), this.TypWt2_Amount.ToString());
            writer.WriteAttributeString(string.Concat(cTypWt2_Unit, attNameExtension), this.TypWt2_Unit);
            writer.WriteAttributeString(string.Concat(cRefuse_Pct, attNameExtension), this.Refuse_Pct.ToString());
        }
        public bool RunMNSR1Calculations(CalculatorParameters calcParameters, double multiplier)
        {
            bool bHasCalculations = false;
            bHasCalculations = SetFNSRStockCalculations(multiplier,
                calcParameters);
            return bHasCalculations;
        }
        
        public bool SetFNSRStockCalculations(double multiplier,
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            if (this != null)
            {
                //adjust the serving size by the multiplier so Q served can be displayed
                this.ActualServingSize = this.ActualServingSize * multiplier;
                this.ActualWater_g = GetActualFoodNutrientProperty(this.Water_g);
                this.ActualEnerg_Kcal = GetActualFoodNutrientProperty(this.Energ_Kcal);
                this.ActualProtein_g = GetActualFoodNutrientProperty(this.Protein_g);
                this.ActualLipid_Tot_g = GetActualFoodNutrientProperty(this.Lipid_Tot_g);
                this.ActualAsh_g = GetActualFoodNutrientProperty(this.Ash_g);
                this.ActualCarbohydrt_g = GetActualFoodNutrientProperty(this.Carbohydrt_g);
                this.ActualFiber_TD_g = GetActualFoodNutrientProperty(this.Fiber_TD_g);
                this.ActualSugar_Tot_g = GetActualFoodNutrientProperty(this.Sugar_Tot_g);
                this.ActualCalcium_mg = GetActualFoodNutrientProperty(this.Calcium_mg);
                this.ActualIron_mg = GetActualFoodNutrientProperty(this.Iron_mg);
                this.ActualMagnesium_mg = GetActualFoodNutrientProperty(this.Magnesium_mg);
                this.ActualPhosphorus_mg = GetActualFoodNutrientProperty(this.Phosphorus_mg);
                this.ActualPotassium_mg = GetActualFoodNutrientProperty(this.Potassium_mg);
                this.ActualSodium_mg = GetActualFoodNutrientProperty(this.Sodium_mg);
                this.ActualZinc_mg = GetActualFoodNutrientProperty(this.Zinc_mg);
                this.ActualCopper_mg = GetActualFoodNutrientProperty(this.Copper_mg);
                this.ActualManganese_mg = GetActualFoodNutrientProperty(this.Manganese_mg);
                this.ActualSelenium_pg = GetActualFoodNutrientProperty(this.Selenium_pg);
                this.ActualVit_C_mg = GetActualFoodNutrientProperty(this.Vit_C_mg);
                this.ActualThiamin_mg = GetActualFoodNutrientProperty(this.Thiamin_mg);
                this.ActualRiboflavin_mg = GetActualFoodNutrientProperty(this.Riboflavin_mg);
                this.ActualNiacin_mg = GetActualFoodNutrientProperty(this.Niacin_mg);
                this.ActualPanto_Acid_mg = GetActualFoodNutrientProperty(this.Panto_Acid_mg);
                this.ActualVit_B6_mg = GetActualFoodNutrientProperty(this.Vit_B6_mg);
                this.ActualFolate_Tot_pg = GetActualFoodNutrientProperty(this.Folate_Tot_pg);
                this.ActualFolic_Acid_pg = GetActualFoodNutrientProperty(this.Folic_Acid_pg);
                this.ActualFood_Folate_pg = GetActualFoodNutrientProperty(this.Food_Folate_pg);
                this.ActualFolate_DFE_pg = GetActualFoodNutrientProperty(this.Folate_DFE_pg);
                this.ActualCholine_Tot_mg = GetActualFoodNutrientProperty(this.Choline_Tot_mg);
                this.ActualVit_B12_pg = GetActualFoodNutrientProperty(this.Vit_B12_pg);
                this.ActualVit_A_IU = GetActualFoodNutrientProperty(this.Vit_A_IU);
                this.ActualVit_A_RAE = GetActualFoodNutrientProperty(this.Vit_A_RAE);
                this.ActualRetinol_pg = GetActualFoodNutrientProperty(this.Retinol_pg);
                this.ActualAlpha_Carot_pg = GetActualFoodNutrientProperty(this.Alpha_Carot_pg);
                this.ActualBeta_Carot_pg = GetActualFoodNutrientProperty(this.Beta_Carot_pg);
                this.ActualBeta_Crypt_pg = GetActualFoodNutrientProperty(this.Beta_Crypt_pg);
                this.ActualLycopene_pg = GetActualFoodNutrientProperty(this.Lycopene_pg);
                this.ActualLut_Zea_pg = GetActualFoodNutrientProperty(this.Lut_Zea_pg);
                this.ActualVit_E_mg = GetActualFoodNutrientProperty(this.Vit_E_mg);
                this.ActualVit_D_pg = GetActualFoodNutrientProperty(this.Vit_D_pg);
                this.ActualViVit_D_IU = GetActualFoodNutrientProperty(this.ViVit_D_IU);
                this.ActualVit_K_pg = GetActualFoodNutrientProperty(this.Vit_K_pg);
                this.ActualFA_Sat_g = GetActualFoodNutrientProperty(this.FA_Sat_g);
                this.ActualFA_Mono_g = GetActualFoodNutrientProperty(this.FA_Mono_g);
                this.ActualFA_Poly_g = GetActualFoodNutrientProperty(this.FA_Poly_g);
                this.ActualCholestrl_mg = GetActualFoodNutrientProperty(this.Cholestrl_mg);
                //actual = typical
                this.ActualExtra1 = this.Extra1;
                this.ActualExtra2 = this.Extra2;
                bHasCalculations = true;
            }
            else
            {
                calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            }
            return bHasCalculations;
        }
        private double GetActualFoodNutrientProperty(double nutrientValToAdjust)
        {
            //food fact only multiplier (IOMNSR1Subscriber uses input.times instead)
            double dbActualNutValuePerHouseHoldMeasure = 1;

            //step 1. 100. For example, to calculate the
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
            double dbCM = SetPortionWeightAndUnit();
            //use typical serving size
            dbActualNutValuePerHouseHoldMeasure = (nutrientValToAdjust * dbCM) / 100;
            //step 2. To calculate the actual nutrient content per actual serving size 
            //(instead of per typical serving size)
            //AN = (N / typicalservingsize) * actualservingsize
            //use actual serving size
            dbActualNutValuePerHouseHoldMeasure = (dbActualNutValuePerHouseHoldMeasure / this.TypicalServingSize)
                * (this.ActualServingSize);
            //step 3. To calculate the “amount of nutrient in edible portion of 1 pound (453.6
            //grams) as purchased,” use the following formula:
            //Y = V*4.536*[(100-R)/100]
            //dbActualNutValuePerHouseHoldMeasure = V*4.536 (except not always per pound)
            double dbActualNutValuePerHHMRefuse = dbActualNutValuePerHouseHoldMeasure;
            if (this.Refuse_Pct > 0)
            {
                dbActualNutValuePerHHMRefuse = dbActualNutValuePerHHMRefuse * ((100 - this.Refuse_Pct) / 100);
            }
            return dbActualNutValuePerHHMRefuse;
        }
        
        private double SetPortionWeightAndUnit()
        {
            double dbCM = 0;
            if (this.WeightToUseType == WEIGHT_TOUSE_TYPES.weight2.ToString())
            {
                //smaller portion than weight 1
                this.ServingSizeUnit = this.TypWt2_Unit;
                this.TypicalServingSize = this.TypWt2_Amount;
                dbCM = this.GmWt_2;
            }
            else if (this.WeightToUseType == WEIGHT_TOUSE_TYPES.weight2metric.ToString())
            {
                this.ServingSizeUnit = "gram";
                this.TypicalServingSize = 1;
                //value per 100 g * 1 / 100 == value per g
                dbCM = 1;
            }
            else if (this.WeightToUseType == WEIGHT_TOUSE_TYPES.weight1metric.ToString())
            {
                this.ServingSizeUnit = "gram";
                this.TypicalServingSize = 1;
                //value per 100 g * 1 / 100 == value per g
                dbCM = 1;
            }
            else
            {
                //weight1 is default
                this.WeightToUseType = WEIGHT_TOUSE_TYPES.weight1.ToString();
                this.ServingSizeUnit = this.TypWt1_Unit;
                this.TypicalServingSize = this.TypWt1_Amount;
                dbCM = this.GmWt_1;
            }
            return dbCM;
        }
    }
}
