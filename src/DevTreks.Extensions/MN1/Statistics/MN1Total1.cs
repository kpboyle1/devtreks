using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             Costs: MN1Stock.Total1.MNSR1Stock.FoodNutritionCalcs
    ///             Benefits: MN1Stock.Total1.MNSR2Stock.FoodNutritionCalcs
    ///             The class aggregates mnstocks.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///Note1:       Budgets could use a NET set of props rather than TOTALS, but causes too much display customization
    ///</summary>
    public class MN1Total1 : MN1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public MN1Total1(CalculatorParameters calcs)
            : base()
        {
            //subprice object
            InitTotalMN1Total1Properties(this, calcs);
        }
        //copy constructor
        public MN1Total1(MN1Total1 calculator)
            : base (calculator)
        {
            CopyTotalMN1Total1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent MN1Stock
        //calculator properties
        public double TotalFNIndex { get; set; }
       
        //keep on hand
        private const string cTotalFNIndex = "TFNIndex";
        
        public void InitTotalMN1Total1Properties(MN1Total1 ind, CalculatorParameters calcs)
        {
            ind.ErrorMessage = string.Empty;
            ind.TotalFNIndex = 0;
            ind.CalcParameters = calcs;
            ind.MNSR1Stock = new MNSR01Stock();
            ind.MNSR2Stock = new MNSR02Stock();
        }
       
        public void CopyTotalMN1Total1Properties(MN1Total1 ind,
            MN1Total1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalFNIndex = calculator.TotalFNIndex;
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            if (calculator.MNSR1Stock == null)
                calculator.MNSR1Stock = new MNSR01Stock();
            if (ind.MNSR1Stock == null)
                ind.MNSR1Stock = new MNSR01Stock();
            ind.MNSR1Stock.CopyTotalMNSR01StockProperties(calculator.MNSR1Stock);
            if (calculator.MNSR2Stock == null)
                calculator.MNSR2Stock = new MNSR02Stock();
            if (ind.MNSR2Stock == null)
                ind.MNSR2Stock = new MNSR02Stock();
            ind.MNSR2Stock.CopyTotalMNSR02StockProperties(calculator.MNSR2Stock);
        }

        public void SetTotalMN1Total1Properties(MN1Total1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalFNIndex = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalFNIndex, attNameExtension));
        }
    
        public void SetTotalMN1Total1Property(MN1Total1 ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotalFNIndex:
                    ind.TotalFNIndex = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
      
        public string GetTotalMN1Total1Property(MN1Total1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalFNIndex:
                    sPropertyValue = ind.TotalFNIndex.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        
        public virtual void SetTotalMN1Total1Attributes(MN1Total1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalFNIndex, attNameExtension), ind.TotalFNIndex);
            }
            if (bIsBenefitNode || bIsBoth)
            {
            }
        }
        
        public void SetTotalMN1Total1Attributes(MN1Total1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                       string.Concat(cTotalFNIndex, attNameExtension), ind.TotalFNIndex.ToString("f2"));
            }
            if (bIsBenefitNode || bIsBoth)
            {
            }
        }
        //run the analyses for inputs an outputs
        public bool RunAnalyses(MN1Stock mn1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (
            bHasAnalyses = SetAnalyses(mn1Stock);
            return bHasAnalyses;
        }
        //run the analyes for everything else 
        //descendentstock holds input and output stock totals and calculators
        public bool RunAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to mn1stock.Total1
            if (mn1Stock.Total1 == null)
            {
                mn1Stock.Total1 = new MN1Total1(this.CalcParameters);
            }
            //need one property set
            mn1Stock.Total1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
            bHasAnalyses = SetAnalyses(mn1Stock, calcs);
            return bHasAnalyses;
        }
        private bool SetAnalyses(MN1Stock mn1Stock)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used to store numerical results
            //calcprops and analyzerprops stored in mn1stock
            mn1Stock.Total1 = new MN1Total1(this.CalcParameters);
            //need one property set
            mn1Stock.Total1.SubApplicationType = mn1Stock.CalcParameters.SubApplicationType.ToString();
            //these are the mnc and mnb calculations
            //the initial aggregation must have serialized them correctly as mnc or mnb calcors
            //costs
            foreach (MNC1Calculator ind in mn1Stock.MNSR1Stock.FoodNutritionCalcs)
            {
                if (ind.CalculatorType
                    == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                {
                    //rerun calcs using input.OCAmount or input.CAPAmount and multiplier (input.times)
                    ind.RunMNC1Calculations(mn1Stock.CalcParameters);
                    //rerun calcs using multiplier (input.times)
                    bHasTotals = AddInputToTotalStock(mn1Stock.Total1, mn1Stock.Multiplier,
                        ind);
                    //stock needs some calculator properties (date)
                    BIMN1StockAnalyzer.CopyBaseElementProperties(ind.MNCInput, mn1Stock);
                    mn1Stock.Date = ind.MNCInput.Date;
                    if (bHasTotals)
                    {
                        bHasAnalysis = true;
                    }
                }
            }
            //benefits
            foreach (MNB1Calculator ind in mn1Stock.MNSR2Stock.FoodNutritionCalcs)
            {
                if (ind.CalculatorType
                    == MN1CalculatorHelper.CALCULATOR_TYPES.foodnutSR02.ToString())
                {
                    //rerun calcs using input.OCAmount or input.CAPAmount and multiplier (input.times)
                    ind.RunMNB1Calculations(mn1Stock.CalcParameters);
                    //ind.FoodNutritionCalcs holds the subprices collection (which must also be totaled)
                    bHasTotals = AddOutputToTotalStock(mn1Stock.Total1, mn1Stock.Multiplier,
                        ind);
                    //stock needs some calculator properties (date)
                    BIMN1StockAnalyzer.CopyBaseElementProperties(ind.MNBOutput, mn1Stock);
                    mn1Stock.Date = ind.MNBOutput.Date;
                    if (bHasTotals)
                    {
                        bHasAnalysis = true;
                    }
                }
            }
            return bHasAnalysis;
        }
        private bool SetAnalyses(MN1Stock mn1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of calculators
            //so input.calc not used here but used in parent

            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(mn1Stock.GetType()))
                {
                    MN1Stock stock = (MN1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Total1 != null)
                        {
                            //tps start substracting outcomes from op/comps
                            if (mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                || mn1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                //subtract stock2 (outputs) from stock1 (inputs) and display stock1 net only
                                stock.Total1.SubApplicationType = stock.SubApplicationType;
                                bHasTotals = AddandSubtractSubTotalToTotalStock(mn1Stock.Total1, stock.Multiplier, stock.Total1);
                            }
                            else
                            {
                                //multiplier is found in stock, not mn1Stock, to ensure that correct before-aggregated element multiplier is used
                                bHasTotals = AddSubTotalToTotalStock(mn1Stock.Total1, stock.Multiplier, stock.Total1);
                            }
                            if (bHasTotals)
                            {
                                bHasAnalysis = true;
                            }
                        }
                    }
                }
            }
            return bHasAnalysis;
        }

       
        public static bool AddSubTotalToTotalStock(MN1Total1 baseStat, double multiplier,
           MN1Total1 subTotal)
        {
            bool bHasCalculations = false;
            //use underlying stock 
            baseStat.MNSR1Stock.AddSubTotalToTotalStock(multiplier, subTotal.MNSR1Stock);
            baseStat.MNSR2Stock.AddSubTotalToTotalStock(multiplier, subTotal.MNSR2Stock);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddandSubtractSubTotalToTotalStock(MN1Total1 baseStat, double multiplier,
           MN1Total1 subTotal)
        {
            bool bHasCalculations = false;
            //use underlying stock 
            baseStat.MNSR1Stock.AddSubTotalToTotalStock(multiplier, subTotal.MNSR1Stock);
            //MNSR1Stock holds nets; MNSR2Stock will be set to zero
            //no need for subTotal.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
            //because MNSR2Stock holds output calcs
            baseStat.MNSR1Stock.SubtractSubTotalFromTotalStock(multiplier, subTotal.MNSR2Stock);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool AddInputToTotalStock(MN1Total1 totStock, double multiplier,
            MNC1Calculator mncInput)
        {
            bool bHasCalculations = false;
            //multiplier adjusted nutrients
            totStock.TotalFNIndex += mncInput.FNIndex * multiplier;
            totStock.MNSR1Stock.AddInputToTotalStock(multiplier, mncInput);
            bHasCalculations = true;
            return bHasCalculations;
        }

        public bool AddOutputToTotalStock(MN1Total1 totStock, double multiplier,
            MNB1Calculator mnbOutput)
        {
            bool bHasCalculations = false;
            totStock.TotalFNIndex += mnbOutput.FNIndex * multiplier;
            totStock.MNSR2Stock.AddOutputToTotalStock(multiplier, mnbOutput);
            bHasCalculations = true;
            return bHasCalculations;
        }
       
    }
}