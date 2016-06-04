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
    ///             Initital: ME2Stock.Total1.ME2Indicators
    ///             End: ME2Stock.Stocks.Total1.Stocks.ME2Indicators
    ///             Total1 inherits totals from ME2Stock which inherits from ME2IndStock which has Totals props
    ///             The class aggregates mes.
    ///Author:		www.devtreks.org
    ///Date:		2014, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class ME2Total1 : ME2Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2Total1()
            : base()
        {
            //indicator object
            InitTotalME2Total1Properties(this);
        }
        //copy constructor
        public ME2Total1(ME2Total1 calculator)
            : base (calculator)
        {
            this.CopyTotalME2Total1Properties(calculator);
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
                    ME2Total1 total = new ME2Total1();
                    if (totalStock.GetType().Equals(total.GetType()))
                    {
                        total = (ME2Total1)totalStock;
                        if (total != null)
                        {
                            ME2Total1 newTotal = new ME2Total1();
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
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (ME2Stock totalStock in this.Stocks)
                {
                    ME2Total1 total = new ME2Total1();
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
                            observationStock.Total1 = new ME2Total1();
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
                //set in analyzer, not calculator
                if (newCalc.TotalME2Type != null)
                {
                    baseStat.TotalME2Type = newCalc.TotalME2Type;
                }
                else
                {
                    baseStat.TotalME2Type = ME2IndicatorStock.ME_TYPES.none.ToString();
                }
                //set up the calcs
                foreach (ME2Indicator ind in newCalc.ME2Indicators)
                {
                    //multipliers (input.times)
                    ChangeIndicatorByMultiplier(ind, newCalc.Multiplier);
                }
                //rerun the calculations (set the multiplier adjusted totals)
                bHasAnalysis = newCalc.RunCalculations();
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
            ind.IndTotal = ind.IndTotal * multiplier;
            ind.Ind1Amount = ind.Ind1Amount * multiplier;
            ind.Ind2Amount = ind.Ind2Amount * multiplier;
            //if (ind.IndMathType == ME2Indicator.QMATH_TYPE.Q1_multiply_Q2.ToString())
            //{
            //    //multiplication (i.e. q * p) needs only one property changed
            //    ind.Ind1Amount = ind.Ind1Amount * multiplier;
            //}
            //else
            //{
            //    ind.Ind1Amount = ind.Ind1Amount * multiplier;
            //    ind.Ind2Amount = ind.Ind2Amount * multiplier;
            //}
        }
        public static void AddME2IndicatorToStocks(this ME2Total1 baseStat, ME2Indicator indicator)
        {
            //make sure that each indicator has a corresponding stock
            if (baseStat.Stocks == null)
            {
                baseStat.Stocks = new List<ME2Stock>();
            }
            if (!baseStat.Stocks
                .Any(s => s.TotalME2Label == indicator.IndLabel))
            {
                if (indicator.IndLabel != string.Empty)
                {
                    ME2Total1 stock = new ME2Total1();
                    stock.TotalME2Label = indicator.IndLabel;
                    stock.TotalME2Name = indicator.IndName;
                    stock.TotalME2Date = indicator.IndDate;
                    //this has to be set from analyzer props, so add to baseStat prior to coming here
                    stock.TotalME2Type = baseStat.TotalME2Type;
                    stock.TotalME2Unit = indicator.IndUnit;
                    stock.TotalME2Q1Unit = indicator.Ind1Unit;
                    stock.TotalME2Q2Unit = indicator.Ind2Unit;
                    stock.TotalME2Description = indicator.IndDescription;
                    stock.TotalME2Total = indicator.IndTotal;
                    stock.TotalME2Q1Total = indicator.Ind1Amount;
                    stock.TotalME2Q2Total = indicator.Ind2Amount;
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
                    .FirstOrDefault(s => s.TotalME2Label == indicator.IndLabel);
                if (stock != null)
                {
                    stock.TotalME2Total += indicator.IndTotal;
                    stock.TotalME2Q1Total += indicator.Ind1Amount;
                    stock.TotalME2Q2Total += indicator.Ind2Amount;
                    //add the indicator to this stock
                    stock.ME2Indicators.Add(indicator);
                }
            }
        }
        //only comes into play if aggregated base elements were being agg together
        //ok for npv and lca, but aggg base element in M&E analysis is standalone
        public static void AddSubStock1ToTotalStocks(this ME2Total1 baseStat, ME2Total1 newCalc)
        {
            //make sure that each indicator has a corresponding stock
            if (baseStat.Stocks == null)
            {
                baseStat.Stocks = new List<ME2Stock>();
            }
            if (!baseStat.Stocks
                .Any(s => s.TotalME2Label == newCalc.TotalME2Label))
            {
                if (newCalc.TotalME2Label != string.Empty)
                {
                    baseStat.TotalME2Label = newCalc.TotalME2Label;
                    baseStat.TotalME2Name = newCalc.TotalME2Name;
                    baseStat.TotalME2Date = newCalc.TotalME2Date;
                    baseStat.TotalME2Type = newCalc.TotalME2Type;
                    baseStat.TotalME2Unit = newCalc.TotalME2Unit;
                    baseStat.TotalME2Q1Unit = newCalc.TotalME2Q1Unit;
                    baseStat.TotalME2Q2Unit = newCalc.TotalME2Q2Unit;
                    baseStat.TotalME2Description = newCalc.TotalME2Description;
                    //new calc already have been multiplied, but baseStat may have a new one (i.e. parent)
                    baseStat.TotalME2Total = (newCalc.TotalME2Total * newCalc.Multiplier);
                    baseStat.TotalME2Q1Total = (newCalc.TotalME2Q1Total * newCalc.Multiplier);
                    baseStat.TotalME2Q2Total = (newCalc.TotalME2Q2Total * newCalc.Multiplier);
                    baseStat.Stocks.Add(newCalc);
                }
            }
            else
            {
                ME2Stock stock = baseStat.Stocks
                    .FirstOrDefault(s => s.TotalME2Label == newCalc.TotalME2Label);
                 if (stock != null)
                 {
                     baseStat.TotalME2Total += (newCalc.TotalME2Total * newCalc.Multiplier);
                     baseStat.TotalME2Q1Total += (newCalc.TotalME2Q1Total * newCalc.Multiplier);
                     baseStat.TotalME2Q2Total += (newCalc.TotalME2Q2Total * newCalc.Multiplier);
                     if (newCalc.ME2Indicators != null)
                     {
                         foreach (ME2Indicator indicator in newCalc.ME2Indicators)
                         {
                             stock.TotalME2Total += (indicator.IndTotal * newCalc.Multiplier);
                             stock.TotalME2Q1Total += (indicator.Ind1Amount * newCalc.Multiplier);
                             stock.TotalME2Q2Total += (indicator.Ind2Amount * newCalc.Multiplier);
                            //add the indicator to this stock
                            stock.ME2Indicators.Add(indicator);
                         }
                     }
                 }
            }
        }
    }
}