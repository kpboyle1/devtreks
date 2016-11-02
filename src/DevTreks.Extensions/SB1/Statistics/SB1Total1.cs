using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             Costs: SB1Stock.Total1.SB11Stock.SB01Stock
    ///             Benefits: SB1Stock.Total1.SB12Stock.SB02Stock
    ///             The class aggregates sbstocks.
    ///Author:		www.devtreks.org
    ///Date:		2016, November
    ///NOTES        1. Budgets could use a NET set of props rather than TOTALS, but causes too much display customization
    ///</summary>
    public class SB1Total1 : SB1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB1Total1(CalculatorParameters calcs)
            : base()
        {
            //subprice object
            InitTotalSB1Total1Properties(this, calcs);
        }
        //copy constructor
        public SB1Total1(SB1Total1 calculator)
            : base (calculator)
        {
            CopyTotalSB1Total1Properties(this, calculator);
        }
        
        public void InitTotalSB1Total1Properties(SB1Total1 ind, CalculatorParameters calcs)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = calcs;
            ind.SB11Stock = new SB101Stock();
            ind.SB12Stock = new SB102Stock();
        }
       
        public void CopyTotalSB1Total1Properties(SB1Total1 ind,
            SB1Total1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            if (calculator.SB11Stock == null)
                calculator.SB11Stock = new SB101Stock();
            if (ind.SB11Stock == null)
                ind.SB11Stock = new SB101Stock();
            ind.SB11Stock.CopyTotalSB101StockProperties(calculator.SB11Stock);
            if (calculator.SB12Stock == null)
                calculator.SB12Stock = new SB102Stock();
            if (ind.SB12Stock == null)
                ind.SB12Stock = new SB102Stock();
            ind.SB12Stock.CopyTotalSB102StockProperties(calculator.SB12Stock);
        }

        //run the analyses for inputs an outputs
        public async Task<bool> RunAnalyses(SB1Stock sb1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (
            bHasAnalyses = await SetAnalyses(sb1Stock);
            return bHasAnalyses;
        }
        //run the analyes for everything else 
        //descendentstock holds input and output stock totals and calculators
        public bool RunAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to sb1stock.Total1
            if (sb1Stock.Total1 == null)
            {
                sb1Stock.Total1 = new SB1Total1(this.CalcParameters);
            }
            //need one property set
            sb1Stock.Total1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
            bHasAnalyses = SetAnalyses(sb1Stock, calcs);
            return bHasAnalyses;
        }
        private async Task<bool> SetAnalyses(SB1Stock sb1Stock)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used to store numerical results
            //calcprops and analyzerprops stored in sb1stock
            sb1Stock.Total1 = new SB1Total1(this.CalcParameters);
            //need a couple of properties set
            sb1Stock.Total1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
            sb1Stock.Total1.AnalyzerType = this.CalcParameters.AnalyzerParms.AnalyzerType;
            //these are the sbc and sbb calculations
            //the initial aggregation must have serialized them correctly as sbc or sbb calcors
            //costs
            foreach (SBC1Calculator ind in sb1Stock.SB11Stock.SB1Calcs)
            {
                if (ind.CalculatorType
                    == SB1CalculatorHelper.CALCULATOR_TYPES.sb101.ToString())
                {
                    //204 deprecated running calcs again -calcs don't change
                    //rerun calcs using input.OCAmount or input.CAPAmount and multiplier (input.times)
                    //await ind.RunSB1C1CalculationsAsync(sb1Stock.CalcParameters);
                    //need a couple of properties set
                    sb1Stock.Total1.SB11Stock.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
                    sb1Stock.Total1.SB11Stock.AnalyzerType = this.CalcParameters.AnalyzerParms.AnalyzerType;
                    //sum indicators then use multiplier
                    bHasTotals = AddInputToTotalStock(sb1Stock.Total1, sb1Stock.Multiplier, ind);
                    //stock needs some calculator properties (date)
                    BISB1StockAnalyzerAsync.CopyBaseElementProperties(ind.SB1CInput, sb1Stock);
                    //188 added algo props
                    sb1Stock.CopyCalculatorProperties(ind);
                    sb1Stock.CopyData(ind);
                    sb1Stock.Date = ind.SB1CInput.Date;
                    //any total returns true
                    if (bHasTotals)
                    {
                        bHasAnalysis = true;
                    }
                }
            }
            //benefits
            foreach (SBB1Calculator ind in sb1Stock.SB12Stock.SB2Calcs)
            {
                if (ind.CalculatorType
                    == SB1CalculatorHelper.CALCULATOR_TYPES.sb102.ToString())
                {
                    //204 deprecated running calcs again -calcs don't change
                    //rerun calcs using input.OCAmount or input.CAPAmount and multiplier (input.times)
                    //await ind.RunSB1B1CalculationsAsync(sb1Stock.CalcParameters);
                    //need a couple of properties set
                    sb1Stock.Total1.SB12Stock.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
                    sb1Stock.Total1.SB12Stock.AnalyzerType = this.CalcParameters.AnalyzerParms.AnalyzerType;
                    //sum indicators then use multipliers
                    bHasTotals = AddOutputToTotalStock(sb1Stock.Total1, sb1Stock.Multiplier, ind);
                    //stock needs some calculator properties (date)
                    BISB1StockAnalyzerAsync.CopyBaseElementProperties(ind.SB1BOutput, sb1Stock);
                    //188 added algo props
                    sb1Stock.CopyCalculatorProperties(ind);
                    sb1Stock.CopyData(ind);
                    sb1Stock.Date = ind.SB1BOutput.Date;
                    if (bHasTotals)
                    {
                        bHasAnalysis = true;
                    }
                }
            }
            return bHasAnalysis;
        }
        private bool SetAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of calculators
            //so input.calc not used here but used in parent

            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Total1 != null)
                        {
                            string sNodeName = sb1Stock.CalcParameters.CurrentElementNodeName;
                            //set two properties
                            stock.Total1.SubApplicationType = stock.SubApplicationType;
                            stock.Total1.AnalyzerType = sb1Stock.CalcParameters.AnalyzerParms.AnalyzerType;
                            //tps start substracting outcomes from op/comps
                            if (sNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                || sNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                //add stock2 (outputs) to stock1 (inputs) and display stock1 net only
                                bHasTotals = AddSubTotalToTotalStock2(sb1Stock.Total1, stock.Multiplier, stock.Total1);
                            }
                            else
                            {
                                //multiplier is found in stock, not sb1Stock, to ensure that correct before-aggregated element multiplier is used
                                bHasTotals = AddSubTotalToTotalStock(sb1Stock.Total1, stock.Multiplier, stock.Total1);
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
        
        public static bool AddSubTotalToTotalStock(SB1Total1 baseStat, double multiplier,
           SB1Total1 subTotal)
        {
            bool bHasCalculations = false;
            //use underlying stock 
            bHasCalculations = baseStat.SB11Stock.AddSubTotalToTotalStock(subTotal.SB11Stock);
            //multipliers only applied to totals
            baseStat.SB11Stock.ChangeSubTotalByMultipliers(multiplier);
            bHasCalculations = baseStat.SB12Stock.AddSubTotalToTotalStock(subTotal.SB12Stock);
            //multipliers only applied to totals
            baseStat.SB12Stock.ChangeSubTotalByMultipliers(multiplier);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddSubTotalToTotalStock2(SB1Total1 baseStat, double multiplier,
           SB1Total1 subTotal)
        {
            bool bHasCalculations = false;
            
            //use underlying stock 
            //to avoid double multiplying baseStat by multipliers (baseStat.TP has Operations and then Outcome stocks run)
            //run multipliers only on subtotal
            subTotal.SB11Stock.ChangeSubTotalByMultipliers(multiplier);
            bHasCalculations = baseStat.SB11Stock.AddSubTotalToTotalStock(subTotal.SB11Stock);
            //run multipliers only on subtotal
            subTotal.SB12Stock.ChangeSubTotalByMultipliers(multiplier);
            //SB11Stock will hold nets; SB12Stock will be set to zero
            bHasCalculations = baseStat.SB11Stock.AddSubTotalToTotalStock(subTotal.SB12Stock);
            //reset SB12Stock to zero so they don't get counted in subsequent analyses
            baseStat.SB12Stock.InitTSB1BaseStockProperties();
            return bHasCalculations;
        }

       
        
        public bool AddInputToTotalStock(SB1Total1 totStock, double multiplier,
            SBC1Calculator sbcInput)
        {
            bool bHasCalculations = false;
            //totStock.SB11Stock.TotalSB1Score += (sbcInput.SB1Score * multiplier);
            totStock.SB11Stock.AddInputToTotalStock(sbcInput);
            //multipliers only applied to totals
            totStock.SB11Stock.ChangeSubTotalByMultipliers(multiplier);
            bHasCalculations = true;
            return bHasCalculations;
        }

        public bool AddOutputToTotalStock(SB1Total1 totStock, double multiplier,
            SBB1Calculator sbbOutput)
        {
            bool bHasCalculations = false;
            //totStock.SB12Stock.TotalSB1Score += (sbbOutput.SB1Score * multiplier);
            totStock.SB12Stock.AddOutputToTotalStock(sbbOutput);
            //multipliers only applied to totals
            totStock.SB12Stock.ChangeSubTotalByMultipliers(multiplier);
            bHasCalculations = true;
            return bHasCalculations;
        }
        
    }
}