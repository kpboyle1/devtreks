using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The FNSR01Stock class extends the FNSR01Calculator() 
    ///             class and is used by food nutrition calculators and analyzers 
    ///             to set totals and basic food nutrition statistics. Basic 
    ///             food nutrition statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2012, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    public class FNSR01Stock : FNSR01Calculator
    {
        //calls the base-class version, and initializes the base class properties.
        public FNSR01Stock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
            //food nutrition stock
            InitTotalFNSR01StockProperties();
        }
        //copy constructor
        public FNSR01Stock(FNSR01Stock calculator)
            : base(calculator)
        {
            CopyTotalFNSR01StockProperties(calculator);
        }

        //calculator properties
        //food nutrition collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<FNSR01Calculator>> FoodNutritionStocks = null;

        //food nutrition totals (actualfoodnutrition values)
        public double TotalMarketValue { get; set; }
        public double TotalContainerSizeUsingServingSizeUnit { get; set; }
        public double TotalServingCost { get; set; }
        public double TotalActualServingSize { get; set; }
        public double TotalTypicalServingsPerContainer { get; set; }
        public double TotalActualServingsPerContainer { get; set; }
        public double TotalTypicalServingSize { get; set; }
        //food nutritional values
        public double TotalWater_g { get; set; }
        public double TotalEnerg_Kcal { get; set; }
        public double TotalProtein_g { get; set; }
        public double TotalLipid_Tot_g { get; set; }
        public double TotalAsh_g { get; set; }
        public double TotalCarbohydrt_g { get; set; }
        public double TotalFiber_TD_g { get; set; }
        public double TotalSugar_Tot_g { get; set; }
        public double TotalCalcium_mg { get; set; }
        public double TotalIron_mg { get; set; }
        public double TotalMagnesium_mg { get; set; }
        public double TotalPhosphorus_mg { get; set; }
        public double TotalPotassium_mg { get; set; }
        public double TotalSodium_mg { get; set; }
        public double TotalZinc_mg { get; set; }
        public double TotalCopper_mg { get; set; }
        public double TotalManganese_mg { get; set; }
        public double TotalSelenium_pg { get; set; }
        public double TotalVit_C_mg { get; set; }
        public double TotalThiamin_mg { get; set; }
        public double TotalRiboflavin_mg { get; set; }
        public double TotalNiacin_mg { get; set; }
        public double TotalPanto_Acid_mg { get; set; }
        public double TotalVit_B6_mg { get; set; }
        public double TotalFolate_Tot_pg { get; set; }
        public double TotalFolic_Acid_pg { get; set; }
        public double TotalFood_Folate_pg { get; set; }
        public double TotalFolate_DFE_pg { get; set; }
        public double TotalCholine_Tot_mg { get; set; }
        public double TotalVit_B12_pg { get; set; }
        public double TotalVit_A_IU { get; set; }
        public double TotalVit_A_RAE { get; set; }
        public double TotalRetinol_pg { get; set; }
        public double TotalAlpha_Carot_pg { get; set; }
        public double TotalBeta_Carot_pg { get; set; }
        public double TotalBeta_Crypt_pg { get; set; }
        public double TotalLycopene_pg { get; set; }
        public double TotalLut_Zea_pg { get; set; }
        public double TotalVit_E_mg { get; set; }
        public double TotalVit_D_pg { get; set; }
        public double TotalViVit_D_IU { get; set; }
        public double TotalVit_K_pg { get; set; }
        public double TotalFA_Sat_g { get; set; }
        public double TotalFA_Mono_g { get; set; }
        public double TotalFA_Poly_g { get; set; }
        public double TotalCholestrl_mg { get; set; }

        private const string TMarketValue = "TMarketValue";
        private const string TContainerSizeUsingServingSizeUnit = "TContainerSizeUsingServingSizeUnit";
        private const string TServingCost = "TServingCost";
        private const string TActualServingSize = "TActualServingSize";
        private const string TTypicalServingSize = "TTypicalServingSize";
        private const string TTypicalServingsPerContainer = "TTypicalServingsPerContainer";
        private const string TActualServingsPerContainer = "TActualServingsPerContainer";

        private const string TWater_g = "TWater_g";
        private const string TEnerg_Kcal = "TEnerg_Kcal";
        private const string TProtein_g = "TProtein_g";
        private const string TLipid_Tot_g = "TLipid_Tot_g";
        private const string TAsh_g = "TAsh_g";
        private const string TCarbohydrt_g = "TCarbohydrt_g";
        private const string TFiber_TD_g = "TFiber_TD_g";
        private const string TSugar_Tot_g = "TSugar_Tot_g";
        private const string TCalcium_mg = "TCalcium_mg";
        private const string TIron_mg = "TIron_mg";
        private const string TMagnesium_mg = "TMagnesium_mg";
        private const string TPhosphorus_mg = "TPhosphorus_mg";
        private const string TPotassium_mg = "TPotassium_mg";
        private const string TSodium_mg = "TSodium_mg";
        private const string TZinc_mg = "TZinc_mg";
        private const string TCopper_mg = "TCopper_mg";
        private const string TManganese_mg = "TManganese_mg";
        private const string TSelenium_pg = "TSelenium_pg";
        private const string TVit_C_mg = "TVit_C_mg";
        private const string TThiamin_mg = "TThiamin_mg";
        private const string TRiboflavin_mg = "TRiboflavin_mg";
        private const string TNiacin_mg = "TNiacin_mg";
        private const string TPanto_Acid_mg = "TPanto_Acid_mg";
        private const string TVit_B6_mg = "TVit_B6_mg";
        private const string TFolate_Tot_pg = "TFolate_Tot_pg";
        private const string TFolic_Acid_pg = "TFolic_Acid_pg";
        private const string TFood_Folate_pg = "TFood_Folate_pg";
        private const string TFolate_DFE_pg = "TFolate_DFE_pg";
        private const string TCholine_Tot_mg = "TCholine_Tot_mg";
        private const string TVit_B12_pg = "TVit_B12_pg";
        private const string TVit_A_IU = "TVit_A_IU";
        private const string TVit_A_RAE = "TVit_A_RAE";
        private const string TRetinol_pg = "TRetinol_pg";
        private const string TAlpha_Carot_pg = "TAlpha_Carot_pg";
        private const string TBeta_Carot_pg = "TBeta_Carot_pg";
        private const string TBeta_Crypt_pg = "TBeta_Crypt_pg";
        private const string TLycopene_pg = "TLycopene_pg";
        private const string TLut_Zea_pg = "TLut_Zea_pg";
        private const string TVit_E_mg = "TVit_E_mg";
        private const string TVit_D_pg = "TVit_D_pg";
        private const string TViVit_D_IU = "TViVit_D_IU";
        private const string TVit_K_pg = "TVit_K_pg";
        private const string TFA_Sat_g = "TFA_Sat_g";
        private const string TFA_Mono_g = "TFA_Mono_g";
        private const string TFA_Poly_g = "TFA_Poly_g";
        private const string TCholestrl_mg = "TCholestrl_mg";

        public virtual void InitTotalFNSR01StockProperties()
        {
            //avoid null references to properties
            this.TotalMarketValue = 0;
            this.TotalContainerSizeUsingServingSizeUnit = 0;
            this.TotalServingCost = 0;
            this.TotalActualServingSize = 0;
            this.TotalTypicalServingsPerContainer = 0;
            this.TotalActualServingsPerContainer = 0;
            this.TotalTypicalServingSize = 0;
            this.TotalWater_g = 0;
            this.TotalEnerg_Kcal = 0;
            this.TotalProtein_g = 0;
            this.TotalLipid_Tot_g = 0;
            this.TotalAsh_g = 0;
            this.TotalCarbohydrt_g = 0;
            this.TotalFiber_TD_g = 0;
            this.TotalSugar_Tot_g = 0;
            this.TotalCalcium_mg = 0;
            this.TotalIron_mg = 0;
            this.TotalMagnesium_mg = 0;
            this.TotalPhosphorus_mg = 0;
            this.TotalPotassium_mg = 0;
            this.TotalSodium_mg = 0;
            this.TotalZinc_mg = 0;
            this.TotalCopper_mg = 0;
            this.TotalManganese_mg = 0;
            this.TotalSelenium_pg = 0;
            this.TotalVit_C_mg = 0;
            this.TotalThiamin_mg = 0;
            this.TotalRiboflavin_mg = 0;
            this.TotalNiacin_mg = 0;
            this.TotalPanto_Acid_mg = 0;
            this.TotalVit_B6_mg = 0;
            this.TotalFolate_Tot_pg = 0;
            this.TotalFolic_Acid_pg = 0;
            this.TotalFood_Folate_pg = 0;
            this.TotalFolate_DFE_pg = 0;
            this.TotalCholine_Tot_mg = 0;
            this.TotalVit_B12_pg = 0;
            this.TotalVit_A_IU = 0;
            this.TotalVit_A_RAE = 0;
            this.TotalRetinol_pg = 0;
            this.TotalAlpha_Carot_pg = 0;
            this.TotalBeta_Carot_pg = 0;
            this.TotalBeta_Crypt_pg = 0;
            this.TotalLycopene_pg = 0;
            this.TotalLut_Zea_pg = 0;
            this.TotalVit_E_mg = 0;
            this.TotalVit_D_pg = 0;
            this.TotalViVit_D_IU = 0;
            this.TotalVit_K_pg = 0;
            this.TotalFA_Sat_g = 0;
            this.TotalFA_Mono_g = 0;
            this.TotalFA_Poly_g = 0;
            this.TotalCholestrl_mg = 0;
        }
        public virtual void CopyTotalFNSR01StockProperties(
            FNSR01Stock calculator)
        {
            this.TotalMarketValue = calculator.TotalMarketValue;
            this.TotalContainerSizeUsingServingSizeUnit = calculator.TotalContainerSizeUsingServingSizeUnit;
            this.TotalServingCost = calculator.TotalServingCost;
            this.TotalActualServingSize = calculator.TotalActualServingSize;
            this.TotalTypicalServingsPerContainer = calculator.TotalTypicalServingsPerContainer;
            this.TotalActualServingsPerContainer = calculator.TotalActualServingsPerContainer;
            this.TotalTypicalServingSize = calculator.TotalTypicalServingSize;
            this.TotalWater_g = calculator.TotalWater_g;
            this.TotalEnerg_Kcal = calculator.TotalEnerg_Kcal;
            this.TotalProtein_g = calculator.TotalProtein_g;
            this.TotalLipid_Tot_g = calculator.TotalLipid_Tot_g;
            this.TotalAsh_g = calculator.TotalAsh_g;
            this.TotalCarbohydrt_g = calculator.TotalCarbohydrt_g;
            this.TotalFiber_TD_g = calculator.TotalFiber_TD_g;
            this.TotalSugar_Tot_g = calculator.TotalSugar_Tot_g;
            this.TotalCalcium_mg = calculator.TotalCalcium_mg;
            this.TotalIron_mg = calculator.TotalIron_mg;
            this.TotalMagnesium_mg = calculator.TotalMagnesium_mg;
            this.TotalPhosphorus_mg = calculator.TotalPhosphorus_mg;
            this.TotalPotassium_mg = calculator.TotalPotassium_mg;
            this.TotalSodium_mg = calculator.TotalSodium_mg;
            this.TotalZinc_mg = calculator.TotalZinc_mg;
            this.TotalCopper_mg = calculator.TotalCopper_mg;
            this.TotalManganese_mg = calculator.TotalManganese_mg;
            this.TotalSelenium_pg = calculator.TotalSelenium_pg;
            this.TotalVit_C_mg = calculator.TotalVit_C_mg;
            this.TotalThiamin_mg = calculator.TotalThiamin_mg;
            this.TotalRiboflavin_mg = calculator.TotalRiboflavin_mg;
            this.TotalNiacin_mg = calculator.TotalNiacin_mg;
            this.TotalPanto_Acid_mg = calculator.TotalPanto_Acid_mg;
            this.TotalVit_B6_mg = calculator.TotalVit_B6_mg;
            this.TotalFolate_Tot_pg = calculator.TotalFolate_Tot_pg;
            this.TotalFolic_Acid_pg = calculator.TotalFolic_Acid_pg;
            this.TotalFood_Folate_pg = calculator.TotalFood_Folate_pg;
            this.TotalFolate_DFE_pg = calculator.TotalFolate_DFE_pg;
            this.TotalCholine_Tot_mg = calculator.TotalCholine_Tot_mg;
            this.TotalVit_B12_pg = calculator.TotalVit_B12_pg;
            this.TotalVit_A_IU = calculator.TotalVit_A_IU;
            this.TotalVit_A_RAE = calculator.TotalVit_A_RAE;
            this.TotalRetinol_pg = calculator.TotalRetinol_pg;
            this.TotalAlpha_Carot_pg = calculator.TotalAlpha_Carot_pg;
            this.TotalBeta_Carot_pg = calculator.TotalBeta_Carot_pg;
            this.TotalBeta_Crypt_pg = calculator.TotalBeta_Crypt_pg;
            this.TotalLycopene_pg = calculator.TotalLycopene_pg;
            this.TotalLut_Zea_pg = calculator.TotalLut_Zea_pg;
            this.TotalVit_E_mg = calculator.TotalVit_E_mg;
            this.TotalVit_D_pg = calculator.TotalVit_D_pg;
            this.TotalViVit_D_IU = calculator.TotalViVit_D_IU;
            this.TotalVit_K_pg = calculator.TotalVit_K_pg;
            this.TotalFA_Sat_g = calculator.TotalFA_Sat_g;
            this.TotalFA_Mono_g = calculator.TotalFA_Mono_g;
            this.TotalFA_Poly_g = calculator.TotalFA_Poly_g;
            this.TotalCholestrl_mg = calculator.TotalCholestrl_mg;
        }
        //set the class properties using the XElement
        public virtual void SetTotalFNSR01StockProperties(XElement currentCalculationsElement)
        {
            //set the calculator properties
            this.TotalMarketValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TMarketValue);
            this.TotalContainerSizeUsingServingSizeUnit = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TContainerSizeUsingServingSizeUnit);
            this.TotalServingCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TServingCost);
            this.TotalActualServingSize = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TActualServingSize);
            this.TotalTypicalServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTypicalServingsPerContainer);
            this.TotalActualServingsPerContainer = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TActualServingsPerContainer);
            this.TotalTypicalServingSize = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTypicalServingSize);
            this.TotalWater_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TWater_g);
            this.TotalEnerg_Kcal = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TEnerg_Kcal);
            this.TotalProtein_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TProtein_g);
            this.TotalLipid_Tot_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TLipid_Tot_g);
            this.TotalAsh_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAsh_g);
            this.TotalCarbohydrt_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCarbohydrt_g);
            this.TotalFiber_TD_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFiber_TD_g);
            this.TotalSugar_Tot_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSugar_Tot_g);
            this.TotalCalcium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCalcium_mg);
            this.TotalIron_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TIron_mg);
            this.TotalMagnesium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TMagnesium_mg);
            this.TotalPhosphorus_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPhosphorus_mg);
            this.TotalPotassium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPotassium_mg);
            this.TotalSodium_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSodium_mg);
            this.TotalZinc_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TZinc_mg);
            this.TotalCopper_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCopper_mg);
            this.TotalManganese_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TManganese_mg);
            this.TotalSelenium_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSelenium_pg);
            this.TotalVit_C_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_C_mg);
            this.TotalThiamin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TThiamin_mg);
            this.TotalRiboflavin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TRiboflavin_mg);
            this.TotalNiacin_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TNiacin_mg);
            this.TotalPanto_Acid_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPanto_Acid_mg);
            this.TotalVit_B6_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_B6_mg);
            this.TotalFolate_Tot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFolate_Tot_pg);
            this.TotalFolic_Acid_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFolic_Acid_pg);
            this.TotalFood_Folate_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFood_Folate_pg);
            this.TotalFolate_DFE_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFolate_DFE_pg);
            this.TotalCholine_Tot_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCholine_Tot_mg);
            this.TotalVit_B12_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_B12_pg);
            this.TotalVit_A_IU = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_A_IU);
            this.TotalVit_A_RAE = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_A_RAE);
            this.TotalRetinol_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TRetinol_pg);
            this.TotalAlpha_Carot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAlpha_Carot_pg);
            this.TotalBeta_Carot_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBeta_Carot_pg);
            this.TotalBeta_Crypt_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBeta_Crypt_pg);
            this.TotalLycopene_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TLycopene_pg);
            this.TotalLut_Zea_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TLut_Zea_pg);
            this.TotalVit_E_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_E_mg);
            this.TotalVit_D_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_D_pg);
            this.TotalViVit_D_IU = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TViVit_D_IU);
            this.TotalVit_K_pg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TVit_K_pg);
            this.TotalFA_Sat_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFA_Sat_g);
            this.TotalFA_Mono_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFA_Mono_g);
            this.TotalFA_Poly_g = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TFA_Poly_g);
            this.TotalCholestrl_mg = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TCholestrl_mg);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalFNSR01StockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TMarketValue:
                    this.TotalMarketValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TContainerSizeUsingServingSizeUnit:
                    this.TotalContainerSizeUsingServingSizeUnit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TServingCost:
                    this.TotalServingCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TActualServingSize:
                    this.TotalActualServingSize = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTypicalServingsPerContainer:
                    this.TotalTypicalServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TActualServingsPerContainer:
                    this.TotalActualServingsPerContainer = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTypicalServingSize:
                    this.TotalTypicalServingSize = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWater_g:
                    this.TotalWater_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEnerg_Kcal:
                    this.TotalEnerg_Kcal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TProtein_g:
                    this.TotalProtein_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLipid_Tot_g:
                    this.TotalLipid_Tot_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAsh_g:
                    this.TotalAsh_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCarbohydrt_g:
                    this.TotalCarbohydrt_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFiber_TD_g:
                    this.TotalFiber_TD_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSugar_Tot_g:
                    this.TotalSugar_Tot_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCalcium_mg:
                    this.TotalCalcium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIron_mg:
                    this.TotalIron_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TMagnesium_mg:
                    this.TotalMagnesium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPhosphorus_mg:
                    this.TotalPhosphorus_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPotassium_mg:
                    this.TotalPotassium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSodium_mg:
                    this.TotalSodium_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TZinc_mg:
                    this.TotalZinc_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCopper_mg:
                    this.TotalCopper_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TManganese_mg:
                    this.TotalManganese_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSelenium_pg:
                    this.TotalSelenium_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_C_mg:
                    this.TotalVit_C_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TThiamin_mg:
                    this.TotalThiamin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRiboflavin_mg:
                    this.TotalRiboflavin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TNiacin_mg:
                    this.TotalNiacin_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPanto_Acid_mg:
                    this.TotalPanto_Acid_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_B6_mg:
                    this.TotalVit_B6_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFolate_Tot_pg:
                    this.TotalFolate_Tot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFolic_Acid_pg:
                    this.TotalFolic_Acid_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFood_Folate_pg:
                    this.TotalFood_Folate_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFolate_DFE_pg:
                    this.TotalFolate_DFE_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCholine_Tot_mg:
                    this.TotalCholine_Tot_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_B12_pg:
                    this.TotalVit_B12_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_A_IU:
                    this.TotalVit_A_IU = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_A_RAE:
                    this.TotalVit_A_RAE = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRetinol_pg:
                    this.TotalRetinol_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAlpha_Carot_pg:
                    this.TotalAlpha_Carot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBeta_Carot_pg:
                    this.TotalBeta_Carot_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBeta_Crypt_pg:
                    this.TotalBeta_Crypt_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLycopene_pg:
                    this.TotalLycopene_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLut_Zea_pg:
                    this.TotalLut_Zea_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_E_mg:
                    this.TotalVit_E_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_D_pg:
                    this.TotalVit_D_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TViVit_D_IU:
                    this.TotalViVit_D_IU = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TVit_K_pg:
                    this.TotalVit_K_pg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFA_Sat_g:
                    this.TotalFA_Sat_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFA_Mono_g:
                    this.TotalFA_Mono_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFA_Poly_g:
                    this.TotalFA_Poly_g = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCholestrl_mg:
                    this.TotalCholestrl_mg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetTotalFNSR01StockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TMarketValue, attNameExtension), this.TotalMarketValue);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TContainerSizeUsingServingSizeUnit, attNameExtension), this.TotalContainerSizeUsingServingSizeUnit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TServingCost, attNameExtension), this.TotalServingCost);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(TTypicalServingSize, attNameExtension), this.TotalTypicalServingSize);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                string.Concat(TActualServingSize, attNameExtension), this.TotalActualServingSize);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TTypicalServingsPerContainer, attNameExtension), this.TotalTypicalServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TActualServingsPerContainer, attNameExtension), this.TotalActualServingsPerContainer);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TWater_g, attNameExtension), this.TotalWater_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TEnerg_Kcal, attNameExtension), this.TotalEnerg_Kcal);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TProtein_g, attNameExtension), this.TotalProtein_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TLipid_Tot_g, attNameExtension), this.TotalLipid_Tot_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAsh_g, attNameExtension), this.TotalAsh_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCarbohydrt_g, attNameExtension), this.TotalCarbohydrt_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFiber_TD_g, attNameExtension), this.TotalFiber_TD_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSugar_Tot_g, attNameExtension), this.TotalSugar_Tot_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCalcium_mg, attNameExtension), this.TotalCalcium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TIron_mg, attNameExtension), this.TotalIron_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TMagnesium_mg, attNameExtension), this.TotalMagnesium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPhosphorus_mg, attNameExtension), this.TotalPhosphorus_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPotassium_mg, attNameExtension), this.TotalPotassium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSodium_mg, attNameExtension), this.TotalSodium_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TZinc_mg, attNameExtension), this.TotalZinc_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCopper_mg, attNameExtension), this.TotalCopper_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TManganese_mg, attNameExtension), this.TotalManganese_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TSelenium_pg, attNameExtension), this.TotalSelenium_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_C_mg, attNameExtension), this.TotalVit_C_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TThiamin_mg, attNameExtension), this.TotalThiamin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TRiboflavin_mg, attNameExtension), this.TotalRiboflavin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TNiacin_mg, attNameExtension), this.TotalNiacin_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TPanto_Acid_mg, attNameExtension), this.TotalPanto_Acid_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_B6_mg, attNameExtension), this.TotalVit_B6_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFolate_Tot_pg, attNameExtension), this.TotalFolate_Tot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFolic_Acid_pg, attNameExtension), this.TotalFolic_Acid_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFood_Folate_pg, attNameExtension), this.TotalFood_Folate_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFolate_DFE_pg, attNameExtension), this.TotalFolate_DFE_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCholine_Tot_mg, attNameExtension), this.TotalCholine_Tot_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_B12_pg, attNameExtension), this.TotalVit_B12_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_A_IU, attNameExtension), this.TotalVit_A_IU);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_A_RAE, attNameExtension), this.TotalVit_A_RAE);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TRetinol_pg, attNameExtension), this.TotalRetinol_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAlpha_Carot_pg, attNameExtension), this.TotalAlpha_Carot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TBeta_Carot_pg, attNameExtension), this.TotalBeta_Carot_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TBeta_Crypt_pg, attNameExtension), this.TotalBeta_Crypt_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TLycopene_pg, attNameExtension), this.TotalLycopene_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TLut_Zea_pg, attNameExtension), this.TotalLut_Zea_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_E_mg, attNameExtension), this.TotalVit_E_mg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_D_pg, attNameExtension), this.TotalVit_D_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TViVit_D_IU, attNameExtension), this.TotalViVit_D_IU);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TVit_K_pg, attNameExtension), this.TotalVit_K_pg);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFA_Sat_g, attNameExtension), this.TotalFA_Sat_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFA_Mono_g, attNameExtension), this.TotalFA_Mono_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TFA_Poly_g, attNameExtension), this.TotalFA_Poly_g);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TCholestrl_mg, attNameExtension), this.TotalCholestrl_mg);
        }
        public virtual void SetTotalFNSR01StockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(TMarketValue, attNameExtension), this.TotalMarketValue.ToString());
            writer.WriteAttributeString(string.Concat(TContainerSizeUsingServingSizeUnit, attNameExtension), this.TotalContainerSizeUsingServingSizeUnit.ToString());
            writer.WriteAttributeString(string.Concat(TServingCost, attNameExtension), this.TotalServingCost.ToString());
            writer.WriteAttributeString(string.Concat(TTypicalServingSize, attNameExtension), this.TotalTypicalServingSize.ToString());
            writer.WriteAttributeString(string.Concat(TActualServingSize, attNameExtension), this.TotalActualServingSize.ToString());
            writer.WriteAttributeString(string.Concat(TTypicalServingsPerContainer, attNameExtension), this.TotalTypicalServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(TActualServingsPerContainer, attNameExtension), this.TotalActualServingsPerContainer.ToString());
            writer.WriteAttributeString(string.Concat(TWater_g, attNameExtension), this.TotalWater_g.ToString());
            writer.WriteAttributeString(string.Concat(TEnerg_Kcal, attNameExtension), this.TotalEnerg_Kcal.ToString());
            writer.WriteAttributeString(string.Concat(TProtein_g, attNameExtension), this.TotalProtein_g.ToString());
            writer.WriteAttributeString(string.Concat(TLipid_Tot_g, attNameExtension), this.TotalLipid_Tot_g.ToString());
            writer.WriteAttributeString(string.Concat(TAsh_g, attNameExtension), this.TotalAsh_g.ToString());
            writer.WriteAttributeString(string.Concat(TCarbohydrt_g, attNameExtension), this.TotalCarbohydrt_g.ToString());
            writer.WriteAttributeString(string.Concat(TFiber_TD_g, attNameExtension), this.TotalFiber_TD_g.ToString());
            writer.WriteAttributeString(string.Concat(TSugar_Tot_g, attNameExtension), this.TotalSugar_Tot_g.ToString());
            writer.WriteAttributeString(string.Concat(TCalcium_mg, attNameExtension), this.TotalCalcium_mg.ToString());
            writer.WriteAttributeString(string.Concat(TIron_mg, attNameExtension), this.TotalIron_mg.ToString());
            writer.WriteAttributeString(string.Concat(TMagnesium_mg, attNameExtension), this.TotalMagnesium_mg.ToString());
            writer.WriteAttributeString(string.Concat(TPhosphorus_mg, attNameExtension), this.TotalPhosphorus_mg.ToString());
            writer.WriteAttributeString(string.Concat(TPotassium_mg, attNameExtension), this.TotalPotassium_mg.ToString());
            writer.WriteAttributeString(string.Concat(TSodium_mg, attNameExtension), this.TotalSodium_mg.ToString());
            writer.WriteAttributeString(string.Concat(TZinc_mg, attNameExtension), this.TotalZinc_mg.ToString());
            writer.WriteAttributeString(string.Concat(TCopper_mg, attNameExtension), this.TotalCopper_mg.ToString());
            writer.WriteAttributeString(string.Concat(TManganese_mg, attNameExtension), this.TotalManganese_mg.ToString());
            writer.WriteAttributeString(string.Concat(TSelenium_pg, attNameExtension), this.TotalSelenium_pg.ToString());
            writer.WriteAttributeString(string.Concat(TVit_C_mg, attNameExtension), this.TotalVit_C_mg.ToString());
            writer.WriteAttributeString(string.Concat(TThiamin_mg, attNameExtension), this.TotalThiamin_mg.ToString());
            writer.WriteAttributeString(string.Concat(TRiboflavin_mg, attNameExtension), this.TotalRiboflavin_mg.ToString());
            writer.WriteAttributeString(string.Concat(TNiacin_mg, attNameExtension), this.TotalNiacin_mg.ToString());
            writer.WriteAttributeString(string.Concat(TPanto_Acid_mg, attNameExtension), this.TotalPanto_Acid_mg.ToString());
            writer.WriteAttributeString(string.Concat(TVit_B6_mg, attNameExtension), this.TotalVit_B6_mg.ToString());
            writer.WriteAttributeString(string.Concat(TFolate_Tot_pg, attNameExtension), this.TotalFolate_Tot_pg.ToString());
            writer.WriteAttributeString(string.Concat(TFolic_Acid_pg, attNameExtension), this.TotalFolic_Acid_pg.ToString());
            writer.WriteAttributeString(string.Concat(TFood_Folate_pg, attNameExtension), this.TotalFood_Folate_pg.ToString());
            writer.WriteAttributeString(string.Concat(TFolate_DFE_pg, attNameExtension), this.TotalFolate_DFE_pg.ToString());
            writer.WriteAttributeString(string.Concat(TCholine_Tot_mg, attNameExtension), this.TotalCholine_Tot_mg.ToString());
            writer.WriteAttributeString(string.Concat(TVit_B12_pg, attNameExtension), this.TotalVit_B12_pg.ToString());
            writer.WriteAttributeString(string.Concat(TVit_A_IU, attNameExtension), this.TotalVit_A_IU.ToString());
            writer.WriteAttributeString(string.Concat(TVit_A_RAE, attNameExtension), this.TotalVit_A_RAE.ToString());
            writer.WriteAttributeString(string.Concat(TRetinol_pg, attNameExtension), this.TotalRetinol_pg.ToString());
            writer.WriteAttributeString(string.Concat(TAlpha_Carot_pg, attNameExtension), this.TotalAlpha_Carot_pg.ToString());
            writer.WriteAttributeString(string.Concat(TBeta_Carot_pg, attNameExtension), this.TotalBeta_Carot_pg.ToString());
            writer.WriteAttributeString(string.Concat(TBeta_Crypt_pg, attNameExtension), this.TotalBeta_Crypt_pg.ToString());
            writer.WriteAttributeString(string.Concat(TLycopene_pg, attNameExtension), this.TotalLycopene_pg.ToString());
            writer.WriteAttributeString(string.Concat(TLut_Zea_pg, attNameExtension), this.TotalLut_Zea_pg.ToString());
            writer.WriteAttributeString(string.Concat(TVit_E_mg, attNameExtension), this.TotalVit_E_mg.ToString());
            writer.WriteAttributeString(string.Concat(TVit_D_pg, attNameExtension), this.TotalVit_D_pg.ToString());
            writer.WriteAttributeString(string.Concat(TViVit_D_IU, attNameExtension), this.TotalViVit_D_IU.ToString());
            writer.WriteAttributeString(string.Concat(TVit_K_pg, attNameExtension), this.TotalVit_K_pg.ToString());
            writer.WriteAttributeString(string.Concat(TFA_Sat_g, attNameExtension), this.TotalFA_Sat_g.ToString());
            writer.WriteAttributeString(string.Concat(TFA_Mono_g, attNameExtension), this.TotalFA_Mono_g.ToString());
            writer.WriteAttributeString(string.Concat(TFA_Poly_g, attNameExtension), this.TotalFA_Poly_g.ToString());
            writer.WriteAttributeString(string.Concat(TCholestrl_mg, attNameExtension), this.TotalCholestrl_mg.ToString());
        }
    }
    public static class FNSR01Extensions
    {
        //add a foodfact to the baseStat.FoodNutritionStocks dictionary
        public static bool AddFNSR01StocksToDictionary(this FNSR01Stock baseStat,
            int filePosition, int nodePosition, FNSR01Calculator foodStock)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.FoodNutritionStocks == null)
                baseStat.FoodNutritionStocks
                = new Dictionary<int, List<FNSR01Calculator>>();
            if (baseStat.FoodNutritionStocks.ContainsKey(filePosition))
            {
                if (baseStat.FoodNutritionStocks[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.FoodNutritionStocks[filePosition].Count <= i)
                        {
                            baseStat.FoodNutritionStocks[filePosition]
                                .Add(new FNSR01Calculator());
                        }
                    }
                    baseStat.FoodNutritionStocks[filePosition][nodePosition]
                        = foodStock;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<FNSR01Calculator> baseStats
                    = new List<FNSR01Calculator>();
                KeyValuePair<int, List<FNSR01Calculator>> newStat
                    = new KeyValuePair<int, List<FNSR01Calculator>>(
                        filePosition, baseStats);
                baseStat.FoodNutritionStocks.Add(newStat);
                bIsAdded = AddFNSR01StocksToDictionary(baseStat,
                    filePosition, nodePosition, foodStock);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this FNSR01Stock baseStat,
            int filePosition, FNSR01Calculator foodStock)
        {
            int iNodeCount = 0;
            if (baseStat.FoodNutritionStocks == null)
                return iNodeCount;
            if (baseStat.FoodNutritionStocks.ContainsKey(filePosition))
            {
                if (baseStat.FoodNutritionStocks[filePosition] != null)
                {
                    iNodeCount = baseStat.FoodNutritionStocks[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}

