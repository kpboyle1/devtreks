using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The MNSR02Stock class extends the MNB1Calculator() 
    ///             class and is used by food nutrition calculators and analyzers 
    ///             to set totals and basic food nutrition statistics. Basic 
    ///             food nutrition statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    public class MNSR02Stock : MNSRStock
    {
        //calls the base-class version, and initializes the base class properties.
        public MNSR02Stock()
            : base()
        {
            //food nutrition stock
            InitTotalMNSR02StockProperties();
        }
        //copy constructor
        public MNSR02Stock(MNSR02Stock calculator)
            : base(calculator)
        {
            CopyTotalMNSR02StockProperties(calculator);
        }

        //simple lists holding calculated results (after being run at io level)
        public List<MNB1Calculator> FoodNutritionCalcs = new List<MNB1Calculator>();
     
        public virtual void InitTotalMNSR02StockProperties()
        {
            //avoid null references to properties
            InitTotalMNSRStockProperties();
            this.FoodNutritionCalcs = new List<MNB1Calculator>();
        }
        public virtual void CopyTotalMNSR02StockProperties(
            MNSR02Stock calculator)
        {
            CopyTotalMNSRStockProperties(calculator);
            this.FoodNutritionCalcs = new List<MNB1Calculator>();
            if (calculator.FoodNutritionCalcs != null)
            {
                //use the extension function to copy
                this.AddFoodCalcsToStock(calculator.FoodNutritionCalcs);
            }
        }
        //set the class properties using the XElement
        public virtual void SetTotalMNSR02StockProperties(XElement currentCalculationsElement)
        {
            //set the calculator properties
            SetTotalMNSRStockProperties(currentCalculationsElement);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalMNSR02StockProperty(string attName,
            string attValue)
        {
            SetTotalMNSRStockProperty(attName, attValue);
        }
        public void SetTotalMNSR02StockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            SetTotalMNSRStockAttributes(attNameExtension, ref currentCalculationsElement);
        }
        public virtual void SetTotalMNSR02StockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            SetTotalMNSRStockAttributes(attNameExtension, ref writer);
        }
    }
    public static class MNSR02Extensions
    {
        public static void AddFoodCalcsToStock(this MNSR02Stock baseStat,
            List<MNB1Calculator> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.FoodNutritionCalcs == null)
                    baseStat.FoodNutritionCalcs = new List<MNB1Calculator>();
                foreach (MNB1Calculator calc in calcs)
                {
                    if (calc.CalculatorType
                        == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02.ToString())
                    {
                        MNB1Calculator mnb = new MNB1Calculator();
                        if (calc.GetType().Equals(mnb.GetType()))
                        {
                            MNB1Calculator mnbOutput = (MNB1Calculator)calc;
                            mnb.CopyMNB1Properties(mnbOutput);
                            baseStat.FoodNutritionCalcs.Add(mnb);
                        }
                    }
                }
            }
        }
        public static void AddOutputCalcsToStock(this MNSR02Stock baseStat, List<Calculator1> calcs)
        {
            if (calcs != null)
            {
                if (baseStat.FoodNutritionCalcs == null)
                    baseStat.FoodNutritionCalcs = new List<MNB1Calculator>();
                foreach (Calculator1 calc in calcs)
                {
                    AddOutputCalcToStock(baseStat, calc);
                }
            }
        }
        public static void AddOutputCalcToStock(this MNSR02Stock baseStat, Calculator1 calc)
        {
            if (calc.CalculatorType
                == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02.ToString())
            {
                MNB1Calculator mnb = new MNB1Calculator();
                if (calc.GetType().Equals(mnb.GetType()))
                {
                    MNB1Calculator mnbOutput = (MNB1Calculator)calc;
                    mnb.CopyMNB1Properties(mnbOutput);
                    baseStat.FoodNutritionCalcs.Add(mnb);
                }
            }
        }
        public static void AddOutputToTotalStock(this MNSR02Stock baseStat, double multiplier,
            MNB1Calculator mnbOutput)
        {
            //multiplier adjusted nutrients
            baseStat.TotalContainerPrice += mnbOutput.ContainerPrice * multiplier;
            baseStat.TotalContainerSizeInSSUnits += mnbOutput.ContainerSizeInSSUnits * multiplier;
            baseStat.TotalServingCost += mnbOutput.ServingCost * multiplier;
            baseStat.TotalActualServingSize += mnbOutput.ActualServingSize * multiplier;
            baseStat.TotalTypicalServingsPerContainer += mnbOutput.TypicalServingsPerContainer * multiplier;
            baseStat.TotalActualServingsPerContainer += mnbOutput.ActualServingsPerContainer * multiplier;
            baseStat.TotalTypicalServingSize += mnbOutput.TypicalServingSize * multiplier;
            baseStat.TotalServingSizeUnit = mnbOutput.ServingSizeUnit;
            baseStat.TotalWater_g += mnbOutput.ActualWater_g * multiplier;
            baseStat.TotalEnerg_Kcal += mnbOutput.ActualEnerg_Kcal * multiplier;
            baseStat.TotalProtein_g += mnbOutput.ActualProtein_g * multiplier;
            baseStat.TotalLipid_Tot_g += mnbOutput.ActualLipid_Tot_g * multiplier;
            baseStat.TotalAsh_g += mnbOutput.ActualAsh_g * multiplier;
            baseStat.TotalCarbohydrt_g += mnbOutput.ActualCarbohydrt_g * multiplier;
            baseStat.TotalFiber_TD_g += mnbOutput.ActualFiber_TD_g * multiplier;
            baseStat.TotalSugar_Tot_g += mnbOutput.ActualSugar_Tot_g * multiplier;
            baseStat.TotalCalcium_mg += mnbOutput.ActualCalcium_mg * multiplier;
            baseStat.TotalIron_mg += mnbOutput.ActualIron_mg * multiplier;
            baseStat.TotalMagnesium_mg += mnbOutput.ActualMagnesium_mg * multiplier;
            baseStat.TotalPhosphorus_mg += mnbOutput.ActualPhosphorus_mg * multiplier;
            baseStat.TotalPotassium_mg += mnbOutput.ActualPotassium_mg * multiplier;
            baseStat.TotalSodium_mg += mnbOutput.ActualSodium_mg * multiplier;
            baseStat.TotalZinc_mg += mnbOutput.ActualZinc_mg * multiplier;
            baseStat.TotalCopper_mg += mnbOutput.ActualCopper_mg * multiplier;
            baseStat.TotalManganese_mg += mnbOutput.ActualManganese_mg * multiplier;
            baseStat.TotalSelenium_pg += mnbOutput.ActualSelenium_pg * multiplier;
            baseStat.TotalVit_C_mg += mnbOutput.ActualVit_C_mg * multiplier;
            baseStat.TotalThiamin_mg += mnbOutput.ActualThiamin_mg * multiplier;
            baseStat.TotalRiboflavin_mg += mnbOutput.ActualRiboflavin_mg * multiplier;
            baseStat.TotalNiacin_mg += mnbOutput.ActualNiacin_mg * multiplier;
            baseStat.TotalPanto_Acid_mg += mnbOutput.ActualPanto_Acid_mg * multiplier;
            baseStat.TotalVit_B6_mg += mnbOutput.ActualVit_B6_mg * multiplier;
            baseStat.TotalFolate_Tot_pg += mnbOutput.ActualFolate_Tot_pg * multiplier;
            baseStat.TotalFolic_Acid_pg += mnbOutput.ActualFolic_Acid_pg * multiplier;
            baseStat.TotalFood_Folate_pg += mnbOutput.ActualFood_Folate_pg * multiplier;
            baseStat.TotalFolate_DFE_pg += mnbOutput.ActualFolate_DFE_pg * multiplier;
            baseStat.TotalCholine_Tot_mg += mnbOutput.ActualCholine_Tot_mg * multiplier;
            baseStat.TotalVit_B12_pg += mnbOutput.ActualVit_B12_pg * multiplier;
            baseStat.TotalVit_A_IU += mnbOutput.ActualVit_A_IU * multiplier;
            baseStat.TotalVit_A_RAE += mnbOutput.ActualVit_A_RAE * multiplier;
            baseStat.TotalRetinol_pg += mnbOutput.ActualRetinol_pg * multiplier;
            baseStat.TotalAlpha_Carot_pg += mnbOutput.ActualAlpha_Carot_pg * multiplier;
            baseStat.TotalBeta_Carot_pg += mnbOutput.ActualBeta_Carot_pg * multiplier;
            baseStat.TotalBeta_Crypt_pg += mnbOutput.ActualBeta_Crypt_pg * multiplier;
            baseStat.TotalLycopene_pg += mnbOutput.ActualLycopene_pg * multiplier;
            baseStat.TotalLut_Zea_pg += mnbOutput.ActualLut_Zea_pg * multiplier;
            baseStat.TotalVit_E_mg += mnbOutput.ActualVit_E_mg * multiplier;
            baseStat.TotalVit_D_pg += mnbOutput.ActualVit_D_pg * multiplier;
            baseStat.TotalViVit_D_IU += mnbOutput.ActualViVit_D_IU * multiplier;
            baseStat.TotalVit_K_pg += mnbOutput.ActualVit_K_pg * multiplier;
            baseStat.TotalFA_Sat_g += mnbOutput.ActualFA_Sat_g * multiplier;
            baseStat.TotalFA_Mono_g += mnbOutput.ActualFA_Mono_g * multiplier;
            baseStat.TotalFA_Poly_g += mnbOutput.ActualFA_Poly_g * multiplier;
            baseStat.TotalCholestrl_mg += mnbOutput.ActualCholestrl_mg * multiplier;
            baseStat.TotalExtra1 += mnbOutput.ActualExtra1 * multiplier;
            baseStat.TotalExtra2 += mnbOutput.ActualExtra2 * multiplier;
        }
    }
}

