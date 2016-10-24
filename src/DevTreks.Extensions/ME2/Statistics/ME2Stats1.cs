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
    ///Purpose:		ME2Stock.Stocks.ME2Stat1.Stocks.ME2Stat1.ME2Indicators
    ///             ME2Stock.Stocks is a collection of ME2Stocks (unique observations)
    ///             Each member of ME2Stocks holds an analyzer stock (Stat1)
    ///             Each analyzer stock (Stat1) holds a collection of Stat1s
    ///             The class statistically analyzes mes.
    ///Author:		www.devtreks.org
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class ME2Stat1 : ME2Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2Stat1(CalculatorParameters calcParams)
            : base(calcParams)
        {
            //subprice object
            InitTotalME2Stat1Properties(this);
        }
        //note that display properties, such as name, description, unit are in 
        //parent ME2Stock calculator properties
        //the total properties come from ME2IndicatorStock

        //most
        public double TotalME2MMean { get; set; }
        public double TotalME2MMedian { get; set; }
        public double TotalME2MVariance { get; set; }
        public double TotalME2MStandDev { get; set; }
        //lower
        public double TotalME2LMean { get; set; }
        public double TotalME2LMedian { get; set; }
        public double TotalME2LVariance { get; set; }
        public double TotalME2LStandDev { get; set; }
        //upper
        public double TotalME2UMean { get; set; }
        public double TotalME2UMedian { get; set; }
        public double TotalME2UVariance { get; set; }
        public double TotalME2UStandDev { get; set; }

        private const string cTotalME2MMean = "TME2MMean";
        private const string cTotalME2MMedian = "TME2MMedian";
        private const string cTotalME2MVariance = "TME2MVariance";
        private const string cTotalME2MStandDev = "TME2MStandDev";

        private const string cTotalME2LMean = "TME2LMean";
        private const string cTotalME2LMedian = "TME2LMedian";
        private const string cTotalME2LVariance = "TME2LVariance";
        private const string cTotalME2LStandDev = "TME2LStandDev";

        private const string cTotalME2UMean = "TME2UMean";
        private const string cTotalME2UMedian = "TME2UMedian";
        private const string cTotalME2UVariance = "TME2UVariance";
        private const string cTotalME2UStandDev = "TME2UStandDev";

        public void InitTotalME2Stat1Properties(ME2Stat1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = new CalculatorParameters();
            InitTotalME2IndicatorStockProperties(ind);
            //init stats
            ind.TME2N = 0;
            ind.TotalME2MMean = 0;
            ind.TotalME2MMedian = 0;
            ind.TotalME2MVariance = 0;
            ind.TotalME2MStandDev = 0;
            ind.TotalME2LMean = 0;
            ind.TotalME2LMedian = 0;
            ind.TotalME2LVariance = 0;
            ind.TotalME2LStandDev = 0;
            ind.TotalME2UMean = 0;
            ind.TotalME2UMedian = 0;
            ind.TotalME2UVariance = 0;
            ind.TotalME2UStandDev = 0;
        }
        public void CopyTotalME2Stat1Properties(ME2Stat1 calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            //copy the initial totals and the indicators (used in RunAnalyses)
            CopyTotalME2IndicatorStockProperties(this, calculator);
            //copy the stats properties
            CopyME2Stat1Properties(this, calculator);
            //copy the calculator.ME2Stocks collection
            if (this.Stocks == null)
                this.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //copy the calculated totals and the indicators
            //obsStock.Stat1.Stocks holds a collection of total1s
            if (calculator.Stocks != null)
            {
                foreach (ME2Stock statStock in calculator.Stocks)
                {
                    ME2Stat1 stat = new ME2Stat1(this.CalcParameters);
                    if (statStock.GetType().Equals(stat.GetType()))
                    {
                        stat = (ME2Stat1)statStock;
                        if (stat != null)
                        {
                            ME2Stat1 newStat = new ME2Stat1(this.CalcParameters);
                            //copy the totals and the indicators
                            CopyTotalME2IndicatorStockProperties(newStat, stat);
                            //copy the stats properties
                            CopyME2Stat1Properties(newStat, stat);
                            this.Stocks.Add(newStat);
                        }
                    }
                }
            }
        }
       
        private void CopyME2Stat1Properties(ME2Stat1 ind,
            ME2Stat1 calculator)
        {
            //stats
            ind.TME2N = calculator.TME2N;
            ind.TotalME2MMean = calculator.TotalME2MMean;
            ind.TotalME2MMedian = calculator.TotalME2MMedian;
            ind.TotalME2MVariance = calculator.TotalME2MVariance;
            ind.TotalME2MStandDev = calculator.TotalME2MStandDev;
            ind.TotalME2LMean = calculator.TotalME2LMean;
            ind.TotalME2LMedian = calculator.TotalME2LMedian;
            ind.TotalME2LVariance = calculator.TotalME2LVariance;
            ind.TotalME2LStandDev = calculator.TotalME2LStandDev;
            ind.TotalME2UMean = calculator.TotalME2UMean;
            ind.TotalME2UMedian = calculator.TotalME2UMedian;
            ind.TotalME2UVariance = calculator.TotalME2UVariance;
            ind.TotalME2UStandDev = calculator.TotalME2UStandDev;
        }
        //1. totals are first run for the stats 
        public void CopyTotalME2Stat1Properties(ME2Total1 ind,
            ME2Stat1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            //copy the totals and the indicators
            CopyTotalME2IndicatorStockProperties(ind, calculator);
            //copy the calculator.ME2Stocks collection
            if (ind.Stocks == null)
                ind.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //calculator.Stocks is a collection of Total1s
            foreach (ME2Stock me2stock in calculator.Stocks)
            {
                ME2Total1 newStat = new ME2Total1(this.CalcParameters);
                //copy the totals and the indicators
                CopyTotalME2IndicatorStockProperties(newStat, me2stock);
                if (newStat != null)
                {
                    ind.Stocks.Add(newStat);
                }
            }
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
        }
        //2. and the totals are copied to stats
        public void CopyTotalME2Stat1Properties(ME2Stat1 ind,
            ME2Total1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            //copy the totals and the indicators
            CopyTotalME2IndicatorStockProperties(ind, calculator);
            //copy the calculator.ME2Stocks collection
            if (ind.Stocks == null)
                ind.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //calculator.Stocks is a collection of Total1s
            foreach (ME2Stock me2stock in calculator.Stocks)
            {
                ME2Stat1 newStat = new ME2Stat1(this.CalcParameters);
                //copy the totals and the indicators
                CopyTotalME2IndicatorStockProperties(newStat, me2stock);
                if (newStat != null)
                {
                    ind.Stocks.Add(newStat);
                }
            }
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
        }
        public void SetTotalME2Stat1Properties(ME2Stat1 ind,
            string attNameExtension, XElement calculator)
        {
            //stats always based on indicators
            SetTotalME2IndicatorStockProperties(ind, attNameExtension, calculator);
            //stats
            ind.TME2N = CalculatorHelpers.GetAttributeInt(calculator,
               string.Concat(cTME2N, attNameExtension));
            ind.TotalME2MMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MMean, attNameExtension));
            ind.TotalME2MMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MMedian, attNameExtension));
            ind.TotalME2MVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MVariance, attNameExtension));
            ind.TotalME2MStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MStandDev, attNameExtension));
            ind.TotalME2LMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LMean, attNameExtension));
            ind.TotalME2LMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LMedian, attNameExtension));
            ind.TotalME2LVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LVariance, attNameExtension));
            ind.TotalME2LStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LStandDev, attNameExtension));
            ind.TotalME2UMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UMean, attNameExtension));
            ind.TotalME2UMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UMedian, attNameExtension));
            ind.TotalME2UVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UVariance, attNameExtension));
            ind.TotalME2UStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UStandDev, attNameExtension));
        }

        public void SetTotalME2Stat1Property(ME2Stat1 ind,
            string attName, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTME2N:
                    ind.TME2N = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cTotalME2MMean:
                    ind.TotalME2MMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2MMedian:
                    ind.TotalME2MMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2MVariance:
                    ind.TotalME2MVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2MStandDev:
                    ind.TotalME2MStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LMean:
                    ind.TotalME2LMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LMedian:
                    ind.TotalME2LMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LVariance:
                    ind.TotalME2LVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LStandDev:
                    ind.TotalME2LStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UMean:
                    ind.TotalME2UMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UMedian:
                    ind.TotalME2UMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UVariance:
                    ind.TotalME2UVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UStandDev:
                    ind.TotalME2UStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalME2Stat1Property(ME2Stat1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTME2N:
                    sPropertyValue = ind.TME2N.ToString();
                    break;
                case cTotalME2MMean:
                    sPropertyValue = ind.TotalME2MMean.ToString();
                    break;
                case cTotalME2MMedian:
                    sPropertyValue = ind.TotalME2MMedian.ToString();
                    break;
                case cTotalME2MVariance:
                    sPropertyValue = ind.TotalME2MVariance.ToString();
                    break;
                case cTotalME2MStandDev:
                    sPropertyValue = ind.TotalME2MStandDev.ToString();
                    break;
                case cTotalME2LMean:
                    sPropertyValue = ind.TotalME2LMean.ToString();
                    break;
                case cTotalME2LMedian:
                    sPropertyValue = ind.TotalME2LMedian.ToString();
                    break;
                case cTotalME2LVariance:
                    sPropertyValue = ind.TotalME2LVariance.ToString();
                    break;
                case cTotalME2LStandDev:
                    sPropertyValue = ind.TotalME2LStandDev.ToString();
                    break;
                case cTotalME2UMean:
                    sPropertyValue = ind.TotalME2UMean.ToString();
                    break;
                case cTotalME2UMedian:
                    sPropertyValue = ind.TotalME2UMedian.ToString();
                    break;
                case cTotalME2UVariance:
                    sPropertyValue = ind.TotalME2UVariance.ToString();
                    break;
                case cTotalME2UStandDev:
                    sPropertyValue = ind.TotalME2UStandDev.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalME2Stat1Attributes(string attNameExt,
            ref XmlWriter writer)
        {
            if (this.Stocks != null)
            {
                int i = 0;
                string sAttNameExtension = string.Empty;
                foreach (ME2Stat1 stat in this.Stocks)
                {
                    if (stat != null)
                    {
                        sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                        //this runs in ME2IndicatorStock object
                        SetTotalME2IndicatorStockAttributes(stat, sAttNameExtension, ref writer);
                        SetTotalME2Stat1Attributes(stat, sAttNameExtension, ref writer);
                    }
                    i++;
                }
            }
        }
        private void SetTotalME2Stat1Attributes(ME2Stat1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            //ind.TME2N is handled with TotalName, TotalLabel ...
            writer.WriteAttributeString(
                string.Concat(cTotalME2MMean, attNameExtension), ind.TotalME2MMean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2MMedian, attNameExtension), ind.TotalME2MMedian.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2MVariance, attNameExtension), ind.TotalME2MVariance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2MStandDev, attNameExtension), ind.TotalME2MStandDev.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LMean, attNameExtension), ind.TotalME2LMean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LMedian, attNameExtension), ind.TotalME2LMedian.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LVariance, attNameExtension), ind.TotalME2LVariance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LStandDev, attNameExtension), ind.TotalME2LStandDev.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UMean, attNameExtension), ind.TotalME2UMean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UMedian, attNameExtension), ind.TotalME2UMedian.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UVariance, attNameExtension), ind.TotalME2UVariance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UStandDev, attNameExtension), ind.TotalME2UStandDev.ToString("N3", CultureInfo.InvariantCulture));
        }
        //calcs holds the collections needing statistical analysis
        public bool RunAnalyses(ME2Stock me2Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //convert calcs to totals
            List<Calculator1> totals = SetTotals(me2Stock, calcs);
            //run calcs and set up me2Stock.Stocks collection 
            bool bHasTotals = me2Stock.Total1.RunAnalyses(me2Stock, totals);
            //run a statistical analysis 
            //the alternative2 aggregator was already used in Total1, don't use it again
            if (bHasTotals)
            {
                bHasAnalyses = SetAnalyses(me2Stock);
            }  
            return bHasAnalyses;
        }
        private List<Calculator1> SetTotals(ME2Stock me2Stock, List<Calculator1> calcs)
        {
            //build a list of initial totals that can be used to runtotals
            List<Calculator1> stocks = new List<Calculator1>();
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(me2Stock.GetType()))
                {
                    ME2Stock stock = (ME2Stock)calc;
                    if (stock != null)
                    {
                        //this initial calculator results are placed in this object
                        if (stock.Stocks != null)
                        {
                            List<ME2Stock> obsStocks = new List<ME2Stock>();
                            foreach (ME2Stock obsStock in stock.Stocks)
                            {
                                //id comes from original calc
                                obsStock.CopyCalculatorProperties(stock);
                                if (obsStock.Stat1 != null)
                                {
                                    obsStock.Total1 = new ME2Total1(this.CalcParameters);
                                    if (obsStock.Stat1.ME2Indicators != null)
                                    {
                                        if (obsStock.Stat1.ME2Indicators.Count > 0)
                                        {
                                            obsStock.Total1.CopyME2IndicatorsProperties(obsStock.Stat1);
                                            //id comes from original calc
                                            obsStock.Total1.CopyCalculatorProperties(stock);
                                            //clear the initial indicators
                                            obsStock.Stat1.ME2Indicators = new List<ME2Indicator>();
                                            obsStocks.Add(obsStock);
                                        }
                                    }
                                }
                            }
                            //reset stock.Storks
                            stock.Stocks = new List<ME2Stock>();
                            foreach (ME2Stock ostock in obsStocks)
                            {
                                stock.Stocks.Add(ostock);
                            }
                            stocks.Add(stock);
                        }
                    }
                }
            }
            return stocks;
        }
        private bool SetAnalyses(ME2Stock me2Stock)
        {
            bool bHasTotals = false;
            if (me2Stock.Stocks != null)
            {
                //each of the stocks is an observation derived from alt2
                List<ME2Stock> newStatStocks = new List<ME2Stock>();
                foreach (ME2Stock totalStock in me2Stock.Stocks)
                {
                    if (totalStock.Total1 != null)
                    {
                        if (totalStock.Total1.Stocks != null)
                        {
                            ME2Stock observationStock = new ME2Stock(this.CalcParameters,
                                me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                            //stocks hold the calculatorids (sometimes me2Stock is parent running multiple children)
                            observationStock.CopyCalculatorProperties(totalStock);
                            //where the stats go
                            observationStock.Stat1 = new ME2Stat1(this.CalcParameters);
                            observationStock.Stat1.CalcParameters = new CalculatorParameters(totalStock.CalcParameters);
                            observationStock.Stat1.CopyCalculatorProperties(totalStock);
                            //each of the stocks is a unique label-dependent total
                            observationStock.Stat1.Stocks = new List<ME2Stock>();
                            foreach (ME2Stock totStock in totalStock.Total1.Stocks)
                            {
                                ME2Stat1 newStat = new ME2Stat1(this.CalcParameters);
                                //copy the totals and the indicators
                                CopyTotalME2IndicatorStockProperties(newStat, totStock);
                                if (newStat.ME2Indicators != null)
                                {
                                    //set N
                                    newStat.TME2N = newStat.ME2Indicators.Count;
                                    //set the cost means
                                    newStat.TotalME2MMean = newStat.TME2TMAmount / newStat.ME2Indicators.Count;
                                    newStat.TotalME2LMean = newStat.TME2TLAmount / newStat.ME2Indicators.Count;
                                    newStat.TotalME2UMean = newStat.TME2TUAmount / newStat.ME2Indicators.Count;
                                    //set the median, variance, and standard deviation costs
                                    SetME2TotalStatistics(newStat);
                                    SetME2LTotalStatistics(newStat);
                                    SetME2UTotalStatistics(newStat);
                                    if (observationStock.Stat1.Stocks == null)
                                        observationStock.Stat1.Stocks = new List<ME2Stock>();
                                    observationStock.Stat1.Stocks.Add(newStat);
                                }
                            }
                            if (observationStock.Stat1.Stocks.Count > 0)
                            {
                                totalStock.Stocks = new List<ME2Stock>();
                                bHasTotals = true;
                                newStatStocks.Add(observationStock);
                            }
                        }
                    }
                }
                if (newStatStocks.Count > 0)
                {
                    bHasTotals = true;
                    me2Stock.Stocks = newStatStocks;
                }
            }
            return bHasTotals;
        }
        
        private static void SetME2TotalStatistics(ME2Stat1 stat)
        {
            //reorder for median
            IEnumerable<ME2Indicator> inds = stat.ME2Indicators.OrderByDescending(s => s.IndTMAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalL = 0;
            double dbMemberSquaredL = 0;
            dbMemberSquaredTotalL = 0;
            double dbMedianL = inds.Count() / 2;
            double dbRemainderL = Math.IEEERemainder(inds.Count(), 2);
            double dbLastTotalL = 0;
            foreach (ME2Indicator ind in inds)
            {
                dbMemberSquaredL = Math.Pow((ind.IndTMAmount - stat.TotalME2MMean), 2);
                dbMemberSquaredTotalL += dbMemberSquaredL;
                if (j > dbMedianL && j != 0)
                {
                    if (dbRemainderL == 0)
                    {
                        //divide the middle two numbers
                        stat.TotalME2MMedian = (ind.IndTMAmount + dbLastTotalL) / 2;
                    }
                    else
                    {
                        //use the middle number
                        stat.TotalME2MMedian = ind.IndTMAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalL = ind.IndTMAmount;
            }

            //don't divide by zero
            if (stat.TME2N > 1)
            {
                //sample variance
                double dbCount = (1 / (stat.TME2N - 1));
                stat.TotalME2MVariance = dbMemberSquaredTotalL * dbCount;
                if (stat.TotalME2MMean != 0)
                {
                    //sample standard deviation
                    stat.TotalME2MStandDev = Math.Sqrt(stat.TotalME2MVariance);
                }
            }
            else
            {
                stat.TotalME2MVariance = 0;
                stat.TotalME2MStandDev = 0;
            }
        }
        private static void SetME2LTotalStatistics(ME2Stat1 stat)
        {
            //reorder for median
            IEnumerable<ME2Indicator> inds = stat.ME2Indicators.OrderByDescending(s => s.IndTMAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalL = 0;
            double dbMemberSquaredL = 0;
            dbMemberSquaredTotalL = 0;
            double dbMedianL = inds.Count() / 2;
            double dbRemainderL = Math.IEEERemainder(inds.Count(), 2);
            double dbLastTotalL = 0;
            foreach (ME2Indicator ind in inds)
            {
                dbMemberSquaredL = Math.Pow((ind.IndTLAmount - stat.TotalME2LMean), 2);
                dbMemberSquaredTotalL += dbMemberSquaredL;
                if (j > dbMedianL && j != 0)
                {
                    if (dbRemainderL == 0)
                    {
                        //divide the middle two numbers
                        stat.TotalME2LMedian = (ind.IndTLAmount + dbLastTotalL) / 2;
                    }
                    else
                    {
                        //use the middle number
                        stat.TotalME2LMedian = ind.IndTLAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalL = ind.IndTLAmount;
            }

            //don't divide by zero
            if (stat.TME2N > 1)
            {
                //sample variance
                double dbCount = (1 / (stat.TME2N - 1));
                stat.TotalME2LVariance = dbMemberSquaredTotalL * dbCount;
                if (stat.TotalME2LMean != 0)
                {
                    //sample standard deviation
                    stat.TotalME2LStandDev = Math.Sqrt(stat.TotalME2LVariance);
                }
            }
            else
            {
                stat.TotalME2LVariance = 0;
                stat.TotalME2LStandDev = 0;
            }
        }
        private static void SetME2UTotalStatistics(ME2Stat1 stat)
        {
            //reorder for median
            IEnumerable<ME2Indicator> inds = stat.ME2Indicators.OrderByDescending(s => s.IndTMAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalL = 0;
            double dbMemberSquaredL = 0;
            dbMemberSquaredTotalL = 0;
            double dbMedianL = inds.Count() / 2;
            double dbRemainderL = Math.IEEERemainder(inds.Count(), 2);
            double dbLastTotalL = 0;
            foreach (ME2Indicator ind in inds)
            {
                dbMemberSquaredL = Math.Pow((ind.IndTUAmount - stat.TotalME2UMean), 2);
                dbMemberSquaredTotalL += dbMemberSquaredL;
                if (j > dbMedianL && j != 0)
                {
                    if (dbRemainderL == 0)
                    {
                        //divide the middle two numbers
                        stat.TotalME2UMedian = (ind.IndTUAmount + dbLastTotalL) / 2;
                    }
                    else
                    {
                        //use the middle number
                        stat.TotalME2UMedian = ind.IndTUAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalL = ind.IndTUAmount;
            }

            //don't divide by zero
            if (stat.TME2N > 1)
            {
                //sample variance
                double dbCount = (1 / (stat.TME2N - 1));
                stat.TotalME2UVariance = dbMemberSquaredTotalL * dbCount;
                if (stat.TotalME2UMean != 0)
                {
                    //sample standard deviation
                    stat.TotalME2UStandDev = Math.Sqrt(stat.TotalME2UVariance);
                }
            }
            else
            {
                stat.TotalME2UVariance = 0;
                stat.TotalME2UStandDev = 0;
            }
        }
    }
    
}
