using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The MNSR01Stock class extends the MNC1Calculator() 
    ///             class and is used by food nutrition calculators and analyzers 
    ///             to set totals and basic food nutrition statistics. Basic 
    ///             food nutrition statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    public class MNSR01Stock : MNSRStock
    {
        //calls the base-class version, and initializes the base class properties.
        public MNSR01Stock()
            : base()
        {
            //food nutrition stock
            InitTotalMNSR01StockProperties();
        }
        //copy constructor
        public MNSR01Stock(MNSR01Stock calculator)
            : base(calculator)
        {
            CopyTotalMNSR01StockProperties(calculator);
        }

        //simple lists holding calculated results (after being run at io level)
        public List<MNC1Calculator> FoodNutritionCalcs = new List<MNC1Calculator>();
     
        public virtual void InitTotalMNSR01StockProperties()
        {
            //avoid null references to properties
            InitTotalMNSRStockProperties();
            this.FoodNutritionCalcs = new List<MNC1Calculator>();
        }
        public virtual void CopyTotalMNSR01StockProperties(
            MNSR01Stock calculator)
        {
            CopyTotalMNSRStockProperties(calculator);
            this.FoodNutritionCalcs = new List<MNC1Calculator>();
            if (calculator.FoodNutritionCalcs != null)
            {
                //use the extension function to copy
                this.AddFoodCalcsToStock(calculator.FoodNutritionCalcs);
            }
        }
        //set the class properties using the XElement
        public virtual void SetTotalMNSR01StockProperties(XElement currentCalculationsElement)
        {
            //set the calculator properties
            SetTotalMNSRStockProperties(currentCalculationsElement);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalMNSR01StockProperty(string attName,
            string attValue)
        {
            SetTotalMNSRStockProperty(attName, attValue);
        }
        public void SetTotalMNSR01StockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            SetTotalMNSRStockAttributes(attNameExtension, ref currentCalculationsElement);
        }
        public virtual void SetTotalMNSR01StockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            SetTotalMNSRStockAttributes(attNameExtension, ref writer);
        }
    }
    public static class MNSR01Extensions
    {
        public static void AddFoodCalcsToStock(this MNSR01Stock baseStat,
            List<MNC1Calculator> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.FoodNutritionCalcs == null)
                    baseStat.FoodNutritionCalcs = new List<MNC1Calculator>();
                foreach (MNC1Calculator calc in calcs)
                {
                    if (calc.CalculatorType
                        == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                    {
                        MNC1Calculator mnc = new MNC1Calculator();
                        if (calc.GetType().Equals(mnc.GetType()))
                        {
                            MNC1Calculator mncInput = (MNC1Calculator)calc;
                            mnc.CopyMNC1Properties(mncInput);
                            baseStat.FoodNutritionCalcs.Add(mnc);
                        }
                    }
                }
            }
        }
        public static void AddInputCalcsToStock(this MNSR01Stock baseStat, List<Calculator1> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.FoodNutritionCalcs == null)
                    baseStat.FoodNutritionCalcs = new List<MNC1Calculator>();
                foreach (Calculator1 calc in calcs)
                {
                    AddInputCalcToStock(baseStat, calc);
                }
            }
        }
        public static void AddInputCalcToStock(this MNSR01Stock baseStat, Calculator1 calc)
        {
            if (calc.CalculatorType
               == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
            {
                MNC1Calculator mnc = new MNC1Calculator();
                if (calc.GetType().Equals(mnc.GetType()))
                {
                    MNC1Calculator mncInput = (MNC1Calculator)calc;
                    mnc.CopyMNC1Properties(mncInput);
                    baseStat.FoodNutritionCalcs.Add(mnc);
                }
            }
        }
        public static void AddInputToTotalStock(this MNSR01Stock baseStat, double multiplier,
            MNC1Calculator mncInput)
        {
            //multiplier adjusted nutrients
            baseStat.TotalContainerPrice += mncInput.ContainerPrice * multiplier;
            baseStat.TotalContainerSizeInSSUnits += mncInput.ContainerSizeInSSUnits * multiplier;
            baseStat.TotalServingCost += mncInput.ServingCost * multiplier;
            baseStat.TotalActualServingSize += mncInput.ActualServingSize * multiplier;
            baseStat.TotalTypicalServingsPerContainer += mncInput.TypicalServingsPerContainer * multiplier;
            baseStat.TotalActualServingsPerContainer += mncInput.ActualServingsPerContainer * multiplier;
            baseStat.TotalTypicalServingSize += mncInput.TypicalServingSize * multiplier;
            baseStat.TotalServingSizeUnit = mncInput.ServingSizeUnit;
            baseStat.TotalWater_g += mncInput.ActualWater_g * multiplier;
            baseStat.TotalEnerg_Kcal += mncInput.ActualEnerg_Kcal * multiplier;
            baseStat.TotalProtein_g += mncInput.ActualProtein_g * multiplier;
            baseStat.TotalLipid_Tot_g += mncInput.ActualLipid_Tot_g * multiplier;
            baseStat.TotalAsh_g += mncInput.ActualAsh_g * multiplier;
            baseStat.TotalCarbohydrt_g += mncInput.ActualCarbohydrt_g * multiplier;
            baseStat.TotalFiber_TD_g += mncInput.ActualFiber_TD_g * multiplier;
            baseStat.TotalSugar_Tot_g += mncInput.ActualSugar_Tot_g * multiplier;
            baseStat.TotalCalcium_mg += mncInput.ActualCalcium_mg * multiplier;
            baseStat.TotalIron_mg += mncInput.ActualIron_mg * multiplier;
            baseStat.TotalMagnesium_mg += mncInput.ActualMagnesium_mg * multiplier;
            baseStat.TotalPhosphorus_mg += mncInput.ActualPhosphorus_mg * multiplier;
            baseStat.TotalPotassium_mg += mncInput.ActualPotassium_mg * multiplier;
            baseStat.TotalSodium_mg += mncInput.ActualSodium_mg * multiplier;
            baseStat.TotalZinc_mg += mncInput.ActualZinc_mg * multiplier;
            baseStat.TotalCopper_mg += mncInput.ActualCopper_mg * multiplier;
            baseStat.TotalManganese_mg += mncInput.ActualManganese_mg * multiplier;
            baseStat.TotalSelenium_pg += mncInput.ActualSelenium_pg * multiplier;
            baseStat.TotalVit_C_mg += mncInput.ActualVit_C_mg * multiplier;
            baseStat.TotalThiamin_mg += mncInput.ActualThiamin_mg * multiplier;
            baseStat.TotalRiboflavin_mg += mncInput.ActualRiboflavin_mg * multiplier;
            baseStat.TotalNiacin_mg += mncInput.ActualNiacin_mg * multiplier;
            baseStat.TotalPanto_Acid_mg += mncInput.ActualPanto_Acid_mg * multiplier;
            baseStat.TotalVit_B6_mg += mncInput.ActualVit_B6_mg * multiplier;
            baseStat.TotalFolate_Tot_pg += mncInput.ActualFolate_Tot_pg * multiplier;
            baseStat.TotalFolic_Acid_pg += mncInput.ActualFolic_Acid_pg * multiplier;
            baseStat.TotalFood_Folate_pg += mncInput.ActualFood_Folate_pg * multiplier;
            baseStat.TotalFolate_DFE_pg += mncInput.ActualFolate_DFE_pg * multiplier;
            baseStat.TotalCholine_Tot_mg += mncInput.ActualCholine_Tot_mg * multiplier;
            baseStat.TotalVit_B12_pg += mncInput.ActualVit_B12_pg * multiplier;
            baseStat.TotalVit_A_IU += mncInput.ActualVit_A_IU * multiplier;
            baseStat.TotalVit_A_RAE += mncInput.ActualVit_A_RAE * multiplier;
            baseStat.TotalRetinol_pg += mncInput.ActualRetinol_pg * multiplier;
            baseStat.TotalAlpha_Carot_pg += mncInput.ActualAlpha_Carot_pg * multiplier;
            baseStat.TotalBeta_Carot_pg += mncInput.ActualBeta_Carot_pg * multiplier;
            baseStat.TotalBeta_Crypt_pg += mncInput.ActualBeta_Crypt_pg * multiplier;
            baseStat.TotalLycopene_pg += mncInput.ActualLycopene_pg * multiplier;
            baseStat.TotalLut_Zea_pg += mncInput.ActualLut_Zea_pg * multiplier;
            baseStat.TotalVit_E_mg += mncInput.ActualVit_E_mg * multiplier;
            baseStat.TotalVit_D_pg += mncInput.ActualVit_D_pg * multiplier;
            baseStat.TotalViVit_D_IU += mncInput.ActualViVit_D_IU * multiplier;
            baseStat.TotalVit_K_pg += mncInput.ActualVit_K_pg * multiplier;
            baseStat.TotalFA_Sat_g += mncInput.ActualFA_Sat_g * multiplier;
            baseStat.TotalFA_Mono_g += mncInput.ActualFA_Mono_g * multiplier;
            baseStat.TotalFA_Poly_g += mncInput.ActualFA_Poly_g * multiplier;
            baseStat.TotalCholestrl_mg += mncInput.ActualCholestrl_mg * multiplier;
            baseStat.TotalExtra1 += mncInput.ActualExtra1 * multiplier;
            baseStat.TotalExtra2 += mncInput.ActualExtra2 * multiplier;
        }
    }
}

