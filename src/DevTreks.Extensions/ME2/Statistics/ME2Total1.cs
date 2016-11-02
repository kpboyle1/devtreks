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
    ///             Initial: ME2Stock.Total1.ME2Indicators
    ///             End: ME2Stock.Stocks.Total1.Stocks.ME2Indicators
    ///             Total1 inherits totals from ME2Stock which inherits from ME2IndStock which has Totals props
    ///             The class aggregates mes.
    ///Author:		www.devtreks.org
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class ME2Total1 : ME2Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2Total1(CalculatorParameters calcParams)
            : base(calcParams)
        {
            //indicator object
            InitTotalME2Total1Properties(this);
        }

        public void InitTotalME2Total1Properties(ME2Total1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = new CalculatorParameters();
            InitTotalME2IndicatorStockProperties(ind);
        }
        public void CopyTotalME2Total1Properties(ME2Total1 calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            //copy the initial totals and the indicators (used in RunAnalyses)
            CopyTotalME2IndicatorStockProperties(this, calculator);
            //copy the calculator.ME2Stocks collection
            if (this.Stocks == null)
                this.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //copy the calculated totals and the indicators
            //obsStock.Total1.Stocks holds a collection of total1s
            if (calculator.Stocks != null)
            {
                foreach (ME2Stock totalStock in calculator.Stocks)
                {
                    ME2Total1 total = new ME2Total1(this.CalcParameters);
                    if (totalStock.GetType().Equals(total.GetType()))
                    {
                        total = (ME2Total1)totalStock;
                        if (total != null)
                        {
                            ME2Total1 newTotal = new ME2Total1(this.CalcParameters);
                            //copy the totals and the indicators
                            CopyTotalME2IndicatorStockProperties(newTotal, total);
                            this.Stocks.Add(newTotal);
                        }
                    }
                }
            }
        }
        public void SetTotalME2Total1Properties(ME2Total1 ind,
            string attNameExtension, XElement calculator)
        {
            SetTotalME2IndicatorStockProperties(ind, attNameExtension, calculator);
        }
    
        public void SetTotalME2Total1Property(ME2Total1 ind,
            string attName, string attValue)
        {
            SetTotalME2IndicatorStockProperty(ind, attName, attValue);
        }
        public string GetTotalME2Total1Property(ME2Total1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            GetTotalME2IndicatorStockProperty(ind, attName);
            return sPropertyValue;
        }
        public virtual void SetTotalME2Total1Attributes(string attNameExt,
            ref XmlWriter writer)
        {
            //the calling procedure processes the regular observation stock
            //obsStock.Total1.Stocks holds a collection of total1s
            if (this.Stocks != null)
            {
                int i = 0;
                string sAttNameExtension = string.Empty;
                foreach (ME2Stock totalStock in this.Stocks)
                {
                    ME2Total1 total = new ME2Total1(this.CalcParameters);
                    if (totalStock.GetType().Equals(total.GetType()))
                    {
                        total = (ME2Total1)totalStock;
                        //1 index : Name2; not 2: Name2_3
                        sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                        SetTotalME2IndicatorStockAttributes(total, sAttNameExtension,
                            ref writer);
                        i++;
                    }
                }
            }
        }
        private void SetTotalME2Total1Attributes(ME2Total1 total,
            string attNameExtension, ref XmlWriter writer)
        {
            //this runs in ME2IndicatorStock object
            SetTotalME2IndicatorStockAttributes(total, attNameExtension, ref writer);
        }
        //run the analyses for inputs an outputs
        public bool RunAnalyses(ME2Stock me2Stock)
        {
            bool bHasAnalyses = false;
            //add totals to me2stock.Total1
            if (me2Stock.Total1 == null)
            {
                return bHasAnalyses;
            }
            //add totals to me stocks (
            bHasAnalyses = me2Stock.Total1.SetTotals(me2Stock.Total1);
            return bHasAnalyses;
        }
        //run the analyes for everything else 
        //descendentstock holds input and output stock totals and calculators
        public bool RunAnalyses(ME2Stock me2Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            bHasAnalyses = SetAnalyses(me2Stock, calcs);
            return bHasAnalyses;
        }


        private bool SetAnalyses(ME2Stock me2Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation (i.e. year, alttype, wbs label); alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            List<ME2Stock> obsStocks = new List<ME2Stock>();
            foreach (var calcbyalt in calcsByAlt2)
            {
                //observationStock goes into me2Stock.Stocks
                ME2Stock observationStock = new ME2Stock(me2Stock.CalcParameters,
                    me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                //set the calcprops using first calcbyalt -it has good calcids (me2Stock could be parent and have bad ids)
                int i = 0;
                //only the totStocks are used in results
                //replace list of totalstocks with list of changestocks
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(me2Stock.GetType()))
                    {
                        //calc has the right ids and props
                        ME2Stock stock = (ME2Stock)calc;
                        if (i == 0)
                        {
                            //need base el id, not me2Stock id
                            observationStock.CopyCalculatorProperties(stock);
                            //where the totals go
                            observationStock.Total1 = new ME2Total1(this.CalcParameters);
                            observationStock.Total1.CalcParameters = new CalculatorParameters(stock.CalcParameters);
                            observationStock.Total1.CopyCalculatorProperties(stock);
                        }
                        if (stock != null)
                        {
                            //this initial calculator results are placed in this object
                            if (stock.Stocks != null)
                            {
                                foreach (ME2Stock obsStock in stock.Stocks)
                                {
                                    if (obsStock.Total1 != null)
                                    {
                                        //set the multiplier; each calculator holds its own multiplier
                                        obsStock.Total1.Multiplier = stock.Multiplier;
                                        //run new calcs and put the result in stock.Total1.Stocks collection
                                        //that is a label-dependent collection of totals1s
                                        bHasTotals = observationStock.Total1.SetTotals(obsStock.Total1);
                                        if (bHasTotals)
                                        {
                                            //1 total is enough for an analysis
                                            bHasAnalysis = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    i++;
                }
                if (bHasAnalysis)
                {
                    //all analyes are now ready to run with good observations and collections of totals and indicators
                    obsStocks.Add(observationStock);
                }
            }
            if (bHasAnalysis)
            {
                me2Stock.Stocks = new List<ME2Stock>();
                me2Stock.Stocks = obsStocks;
            }
            return bHasAnalysis;
        }
    }
    public static class ME2Total1Extensions
    {
        //all analyzers first run totals and put the observations in me2Stock.Stocks collection
        //the stocks collection is an indicator-label based collection -each stock holds same type of indicators
        //baseStat is me2Stock.Total1, me2Stock.Stat1 ...
        //newCalc.Multiplier must be set before calling this
        public static bool SetTotals(this ME2Total1 baseStat, ME2Total1 newCalc)
        {
            bool bHasAnalysis = false;
            //bool bHasTotals = false;
            if (newCalc.ME2Indicators != null)
            {
                //204
                if (newCalc.TME2Stage != null)
                {
                    baseStat.TME2Stage = newCalc.TME2Stage;
                }
                else
                {
                    baseStat.TME2Stage = ME2Stock.ME_STAGES.none.ToString();
                }
                //set up the calcs
                foreach (ME2Indicator ind in newCalc.ME2Indicators)
                {
                    //multipliers (input.times)
                    ChangeIndicatorByMultiplier(ind, newCalc.Multiplier);
                }
                //204 deprecated rerunning calcs -not necessary when base elements updated properlty
                //rerun the calculations
                bHasAnalysis = true;
                //bHasAnalysis = newCalc.RunCalculations();
                foreach (ME2Indicator ind in newCalc.ME2Indicators)
                {
                    //make sure that each indicator has a corresponding stock
                    baseStat.AddME2IndicatorToStocks(ind);
                }
            }
            return bHasAnalysis;
        }
        private static void ChangeIndicatorByMultiplier(ME2Indicator ind,
            double multiplier)
        {
            //194 correction - calcs already run; all props are stand alone
            ind.IndTMAmount = ind.IndTMAmount * multiplier;
            ind.IndTLAmount = ind.IndTLAmount * multiplier;
            ind.IndTUAmount = ind.IndTUAmount * multiplier;
        }
        public static void AddME2IndicatorToStocks(this ME2Total1 baseStat, ME2Indicator indicator)
        {
            //make sure that each indicator has a corresponding stock
            if (baseStat.Stocks == null)
            {
                baseStat.Stocks = new List<ME2Stock>();
            }
            if (!baseStat.Stocks
                .Any(s => s.TME2Label == indicator.IndLabel))
            {
                if (!string.IsNullOrEmpty(indicator.IndLabel))
                {
                    ME2Total1 stock = new ME2Total1(indicator.CalcParameters);
                    stock.TME2Description = indicator.IndDescription;
                    stock.TME2Name = indicator.IndName;
                    stock.TME2Label = indicator.IndLabel;
                    stock.TME2Type = indicator.IndType;
                    stock.TME2RelLabel = indicator.IndRelLabel;
                    stock.TME2TAmount = indicator.IndTAmount;
                    stock.TME2TUnit = indicator.IndTUnit;
                    stock.TME2TD1Amount = indicator.IndTD1Amount;
                    stock.TME2TD1Unit = indicator.IndTD1Unit;
                    stock.TME2TD2Amount = indicator.IndTD2Amount;
                    stock.TME2TD2Unit = indicator.IndTD2Unit;
                    stock.TME2MathResult = indicator.IndMathResult;
                    stock.TME2MathSubType = indicator.IndMathSubType;
                    stock.TME2TMAmount = indicator.IndTMAmount;
                    stock.TME2TMUnit = indicator.IndTMUnit;
                    stock.TME2TLAmount = indicator.IndTLAmount;
                    stock.TME2TLUnit = indicator.IndTLUnit;
                    stock.TME2TUAmount = indicator.IndTUAmount;
                    stock.TME2TUUnit = indicator.IndTUUnit;
                    stock.TME2MathOperator = indicator.IndMathOperator;
                    stock.TME2MathExpression = indicator.IndMathExpression;
                    stock.TME2Date = indicator.IndDate;
                    stock.TME2MathType = indicator.IndMathType;
                    stock.TME2BaseIO = indicator.IndBaseIO;
                    stock.TME21Amount = indicator.Ind1Amount;
                    stock.TME21Unit = indicator.Ind1Unit;
                    stock.TME22Amount = indicator.Ind2Amount;
                    stock.TME22Unit = indicator.Ind2Unit;
                    stock.TME25Amount = indicator.Ind5Amount;
                    stock.TME25Unit = indicator.Ind5Unit;
                    stock.TME23Amount = indicator.Ind3Amount;
                    stock.TME23Unit = indicator.Ind3Unit;
                    stock.TME24Amount = indicator.Ind4Amount;
                    stock.TME24Unit = indicator.Ind4Unit;
                    //test
                    stock.TME2N = 1;

                    //add the indicator to this stock
                    stock.ME2Indicators.Add(indicator);
                    //add the stock to the basestat
                    baseStat.Stocks.Add(stock);
                }
            }
            else
            {
                //this is the same as the ME2Total stock in previous condition
                ME2Stock stock = baseStat.Stocks
                    .FirstOrDefault(s => s.TME2Label == indicator.IndLabel);
                if (stock != null)
                {
                    stock.TME2TAmount += indicator.IndTAmount;
                    stock.TME2TMAmount += indicator.IndTMAmount;
                    stock.TME2TLAmount += indicator.IndTLAmount;
                    stock.TME2TUAmount += indicator.IndTUAmount;
                    stock.TME21Amount += indicator.Ind1Amount;
                    stock.TME22Amount += indicator.Ind2Amount;
                    stock.TME25Amount += indicator.Ind5Amount;
                    stock.TME23Amount += indicator.Ind3Amount;
                    stock.TME24Amount += indicator.Ind4Amount;
                    //test
                    stock.TME2N++;

                    //add the indicator to this stock
                    stock.ME2Indicators.Add(indicator);
                }
            }
        }
        //only comes into play if aggregated base elements were being agg together
        //ok for npv and lca, but agg base element in M&E analysis is standalone
        public static void AddSubStock1ToTotalStocks(this ME2Total1 baseStat, ME2Total1 newCalc)
        {
            //make sure that each indicator has a corresponding stock
            if (baseStat.Stocks == null)
            {
                baseStat.Stocks = new List<ME2Stock>();
            }
            if (!baseStat.Stocks
                .Any(s => s.TME2Label == newCalc.TME2Label))
            {
                if (!string.IsNullOrEmpty(newCalc.TME2Label))
                {
                    baseStat.TME2Description = newCalc.IndDescription;
                    baseStat.TME2Name = newCalc.IndName;
                    baseStat.TME2Label = newCalc.IndLabel;
                    baseStat.TME2Type = newCalc.IndType;
                    baseStat.TME2RelLabel = newCalc.IndRelLabel;
                    baseStat.TME2TUnit = newCalc.IndTUnit;
                    baseStat.TME2TD1Unit = newCalc.IndTD1Unit;
                    baseStat.TME2TD2Unit = newCalc.IndTD2Unit;
                    baseStat.TME2MathResult = newCalc.IndMathResult;
                    baseStat.TME2MathSubType = newCalc.IndMathSubType;
                    baseStat.TME2TMUnit = newCalc.IndTMUnit;
                    baseStat.TME2TLUnit = newCalc.IndTLUnit;
                    baseStat.TME2TUUnit = newCalc.IndTUUnit;
                    baseStat.TME2MathOperator = newCalc.IndMathOperator;
                    baseStat.TME2MathExpression = newCalc.IndMathExpression;
                    baseStat.TME2Date = newCalc.IndDate;
                    baseStat.TME2MathType = newCalc.IndMathType;
                    baseStat.TME2BaseIO = newCalc.IndBaseIO;
                    baseStat.TME21Unit = newCalc.Ind1Unit;
                    baseStat.TME22Unit = newCalc.Ind2Unit;
                    baseStat.TME25Unit = newCalc.Ind5Unit;
                    baseStat.TME23Unit = newCalc.Ind3Unit;
                    baseStat.TME24Unit = newCalc.Ind4Unit;
                    //new calc already have been multiplied, but baseStat 
                    //may have a new one (i.e. parent)
                    baseStat.TME2TAmount += (newCalc.IndTAmount * newCalc.Multiplier);
                    baseStat.TME2TMAmount += (newCalc.IndTMAmount * newCalc.Multiplier);
                    baseStat.TME2TLAmount += (newCalc.IndTLAmount * newCalc.Multiplier);
                    baseStat.TME2TUAmount += (newCalc.IndTUAmount * newCalc.Multiplier);
                    baseStat.TME21Amount += (newCalc.Ind1Amount * newCalc.Multiplier);
                    baseStat.TME22Amount += (newCalc.Ind2Amount * newCalc.Multiplier);
                    baseStat.TME25Amount += (newCalc.Ind5Amount * newCalc.Multiplier);
                    baseStat.TME23Amount += (newCalc.Ind3Amount * newCalc.Multiplier);
                    baseStat.TME24Amount += (newCalc.Ind4Amount * newCalc.Multiplier);

                    baseStat.Stocks.Add(newCalc);
                }
            }
            else
            {
                ME2Stock stock = baseStat.Stocks
                    .FirstOrDefault(s => s.TME2Label == newCalc.TME2Label);
                if (stock != null)
                {
                    baseStat.TME2TAmount += (newCalc.IndTAmount * newCalc.Multiplier);
                    baseStat.TME2TMAmount += (newCalc.IndTMAmount * newCalc.Multiplier);
                    baseStat.TME2TLAmount += (newCalc.IndTLAmount * newCalc.Multiplier);
                    baseStat.TME2TUAmount += (newCalc.IndTUAmount * newCalc.Multiplier);
                    baseStat.TME21Amount += (newCalc.Ind1Amount * newCalc.Multiplier);
                    baseStat.TME22Amount += (newCalc.Ind2Amount * newCalc.Multiplier);
                    baseStat.TME25Amount += (newCalc.Ind5Amount * newCalc.Multiplier);
                    baseStat.TME23Amount += (newCalc.Ind3Amount * newCalc.Multiplier);
                    baseStat.TME24Amount += (newCalc.Ind4Amount * newCalc.Multiplier);
                    if (newCalc.ME2Indicators != null)
                    {
                        foreach (ME2Indicator indicator in newCalc.ME2Indicators)
                        {
                            baseStat.TME2TAmount += (indicator.IndTAmount * newCalc.Multiplier);
                            baseStat.TME2TMAmount += (indicator.IndTMAmount * newCalc.Multiplier);
                            baseStat.TME2TLAmount += (indicator.IndTLAmount * newCalc.Multiplier);
                            baseStat.TME2TUAmount += (indicator.IndTUAmount * newCalc.Multiplier);
                            baseStat.TME21Amount += (indicator.Ind1Amount * newCalc.Multiplier);
                            baseStat.TME22Amount += (indicator.Ind2Amount * newCalc.Multiplier);
                            baseStat.TME25Amount += (indicator.Ind5Amount * newCalc.Multiplier);
                            baseStat.TME23Amount += (indicator.Ind3Amount * newCalc.Multiplier);
                            baseStat.TME24Amount += (indicator.Ind4Amount * newCalc.Multiplier);
                            //add the indicator to this stock
                            stock.ME2Indicators.Add(indicator);
                        }
                    }
                }
            }
        }
    }
}