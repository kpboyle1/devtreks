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
    ///Date:		2014, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class ME2Stat1 : ME2Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2Stat1()
            : base()
        {
            //subprice object
            InitTotalME2Stat1Properties(this);
        }
        //copy constructor
        public ME2Stat1(ME2Stat1 calculator)
            : base(calculator)
        {
            CopyTotalME2Stat1Properties(calculator);
        }
        //note that display properties, such as name, description, total, unit, observations, 
        //are in parent ME2IndicatorStock
        //calculator properties
        //statistical properties of the totals 
        //total
        //the total properties come from ME2IndicatorStock
        //public double TotalME2Total { get; set; }
        //public double TotalME2N { get; set; }
        public double TotalME2Mean { get; set; }
        public double TotalME2Median { get; set; }
        public double TotalME2Variance { get; set; }
        public double TotalME2StandDev { get; set; }
        //q1
        public double TotalME2Q1Mean { get; set; }
        public double TotalME2Q1Median { get; set; }
        public double TotalME2Q1Variance { get; set; }
        public double TotalME2Q1StandDev { get; set; }
        //q2
        public double TotalME2Q2Mean { get; set; }
        public double TotalME2Q2Median { get; set; }
        public double TotalME2Q2Variance { get; set; }
        public double TotalME2Q2StandDev { get; set; }

        private const string cTotalME2Mean = "TME2Mean";
        private const string cTotalME2Median = "TME2Median";
        private const string cTotalME2Variance = "TME2Variance";
        private const string cTotalME2StandDev = "TME2StandDev";

        private const string cTotalME2Q1Mean = "TME2Q1Mean";
        private const string cTotalME2Q1Median = "TME2Q1Median";
        private const string cTotalME2Q1Variance = "TME2Q1Variance";
        private const string cTotalME2Q1StandDev = "TME2Q1StandDev";

        private const string cTotalME2Q2Mean = "TME2Q2Mean";
        private const string cTotalME2Q2Median = "TME2Q2Median";
        private const string cTotalME2Q2Variance = "TME2Q2Variance";
        private const string cTotalME2Q2StandDev = "TME2Q2StandDev";

        public void InitTotalME2Stat1Properties(ME2Stat1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = new CalculatorParameters();
            InitTotalME2IndicatorStockProperties(ind);
            //init stats
            ind.TotalME2N = 0;
            ind.TotalME2Mean = 0;
            ind.TotalME2Median = 0;
            ind.TotalME2Variance = 0;
            ind.TotalME2StandDev = 0;
            ind.TotalME2Q1Mean = 0;
            ind.TotalME2Q1Median = 0;
            ind.TotalME2Q1Variance = 0;
            ind.TotalME2Q1StandDev = 0;
            ind.TotalME2Q2Mean = 0;
            ind.TotalME2Q2Median = 0;
            ind.TotalME2Q2Variance = 0;
            ind.TotalME2Q2StandDev = 0;
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
                    ME2Stat1 stat = new ME2Stat1();
                    if (statStock.GetType().Equals(stat.GetType()))
                    {
                        stat = (ME2Stat1)statStock;
                        if (stat != null)
                        {
                            ME2Stat1 newStat = new ME2Stat1();
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
            ind.TotalME2N = calculator.TotalME2N;
            ind.TotalME2Mean = calculator.TotalME2Mean;
            ind.TotalME2Median = calculator.TotalME2Median;
            ind.TotalME2Variance = calculator.TotalME2Variance;
            ind.TotalME2StandDev = calculator.TotalME2StandDev;
            ind.TotalME2Q1Mean = calculator.TotalME2Q1Mean;
            ind.TotalME2Q1Median = calculator.TotalME2Q1Median;
            ind.TotalME2Q1Variance = calculator.TotalME2Q1Variance;
            ind.TotalME2Q1StandDev = calculator.TotalME2Q1StandDev;
            ind.TotalME2Q2Mean = calculator.TotalME2Q2Mean;
            ind.TotalME2Q2Median = calculator.TotalME2Q2Median;
            ind.TotalME2Q2Variance = calculator.TotalME2Q2Variance;
            ind.TotalME2Q2StandDev = calculator.TotalME2Q2StandDev;
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
                ME2Total1 newStat = new ME2Total1();
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
                ME2Stat1 newStat = new ME2Stat1();
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
            ind.TotalME2N = CalculatorHelpers.GetAttributeInt(calculator,
               string.Concat(cTotalME2N, attNameExtension));
            ind.TotalME2Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Mean, attNameExtension));
            ind.TotalME2Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Median, attNameExtension));
            ind.TotalME2Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Variance, attNameExtension));
            ind.TotalME2StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2StandDev, attNameExtension));
            ind.TotalME2Q1Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q1Mean, attNameExtension));
            ind.TotalME2Q1Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q1Median, attNameExtension));
            ind.TotalME2Q1Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q1Variance, attNameExtension));
            ind.TotalME2Q1StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q1StandDev, attNameExtension));
            ind.TotalME2Q2Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q2Mean, attNameExtension));
            ind.TotalME2Q2Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q2Median, attNameExtension));
            ind.TotalME2Q2Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q2Variance, attNameExtension));
            ind.TotalME2Q2StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q2StandDev, attNameExtension));
        }

        public void SetTotalME2Stat1Property(ME2Stat1 ind,
            string attName, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalME2N:
                    ind.TotalME2N = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cTotalME2Mean:
                    ind.TotalME2Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Median:
                    ind.TotalME2Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Variance:
                    ind.TotalME2Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2StandDev:
                    ind.TotalME2StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q1Mean:
                    ind.TotalME2Q1Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q1Median:
                    ind.TotalME2Q1Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q1Variance:
                    ind.TotalME2Q1Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q1StandDev:
                    ind.TotalME2Q1StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q2Mean:
                    ind.TotalME2Q2Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q2Median:
                    ind.TotalME2Q2Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q2Variance:
                    ind.TotalME2Q2Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q2StandDev:
                    ind.TotalME2Q2StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
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
                case cTotalME2N:
                    sPropertyValue = ind.TotalME2N.ToString();
                    break;
                case cTotalME2Mean:
                    sPropertyValue = ind.TotalME2Mean.ToString();
                    break;
                case cTotalME2Median:
                    sPropertyValue = ind.TotalME2Median.ToString();
                    break;
                case cTotalME2Variance:
                    sPropertyValue = ind.TotalME2Variance.ToString();
                    break;
                case cTotalME2StandDev:
                    sPropertyValue = ind.TotalME2StandDev.ToString();
                    break;
                case cTotalME2Q1Mean:
                    sPropertyValue = ind.TotalME2Q1Mean.ToString();
                    break;
                case cTotalME2Q1Median:
                    sPropertyValue = ind.TotalME2Q1Median.ToString();
                    break;
                case cTotalME2Q1Variance:
                    sPropertyValue = ind.TotalME2Q1Variance.ToString();
                    break;
                case cTotalME2Q1StandDev:
                    sPropertyValue = ind.TotalME2Q1StandDev.ToString();
                    break;
                case cTotalME2Q2Mean:
                    sPropertyValue = ind.TotalME2Q2Mean.ToString();
                    break;
                case cTotalME2Q2Median:
                    sPropertyValue = ind.TotalME2Q2Median.ToString();
                    break;
                case cTotalME2Q2Variance:
                    sPropertyValue = ind.TotalME2Q2Variance.ToString();
                    break;
                case cTotalME2Q2StandDev:
                    sPropertyValue = ind.TotalME2Q2StandDev.ToString();
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
                int i = 1;
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
            //ind.TotalME2N is handled with TotalName, TotalLabel ...
            writer.WriteAttributeString(
                string.Concat(cTotalME2Mean, attNameExtension), ind.TotalME2Mean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Median, attNameExtension), ind.TotalME2Median.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Variance, attNameExtension), ind.TotalME2Variance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2StandDev, attNameExtension), ind.TotalME2StandDev.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q1Mean, attNameExtension), ind.TotalME2Q1Mean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q1Median, attNameExtension), ind.TotalME2Q1Median.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q1Variance, attNameExtension), ind.TotalME2Q1Variance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q1StandDev, attNameExtension), ind.TotalME2Q1StandDev.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q2Mean, attNameExtension), ind.TotalME2Q2Mean.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q2Median, attNameExtension), ind.TotalME2Q2Median.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q2Variance, attNameExtension), ind.TotalME2Q2Variance.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q2StandDev, attNameExtension), ind.TotalME2Q2StandDev.ToString("N3", CultureInfo.InvariantCulture));
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
                                    obsStock.Total1 = new ME2Total1();
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
                            ME2Stock observationStock = new ME2Stock(me2Stock.CalcParameters,
                                me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                            //stocks hold the calculatorids (sometimes me2Stock is parent running multiple children)
                            observationStock.CopyCalculatorProperties(totalStock);
                            //where the stats go
                            observationStock.Stat1 = new ME2Stat1();
                            observationStock.Stat1.CalcParameters = new CalculatorParameters(totalStock.CalcParameters);
                            observationStock.Stat1.CopyCalculatorProperties(totalStock);
                            //each of the stocks is a unique label-dependent total
                            observationStock.Stat1.Stocks = new List<ME2Stock>();
                            foreach (ME2Stock totStock in totalStock.Total1.Stocks)
                            {
                                ME2Stat1 newStat = new ME2Stat1();
                                //copy the totals and the indicators
                                CopyTotalME2IndicatorStockProperties(newStat, totStock);
                                if (newStat.ME2Indicators != null)
                                {
                                    //set N
                                    newStat.TotalME2N = newStat.ME2Indicators.Count;
                                    //set the cost means
                                    newStat.TotalME2Mean = newStat.TotalME2Total / newStat.ME2Indicators.Count;
                                    newStat.TotalME2Q1Mean = newStat.TotalME2Q1Total / newStat.ME2Indicators.Count;
                                    newStat.TotalME2Q2Mean = newStat.TotalME2Q2Total / newStat.ME2Indicators.Count;
                                    //set the median, variance, and standard deviation costs
                                    SetME2TotalStatistics(newStat);
                                    SetME2Q1TotalStatistics(newStat);
                                    SetME2Q2TotalStatistics(newStat);
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
            IEnumerable<ME2Indicator> inds = stat.ME2Indicators.OrderByDescending(s => s.IndTotal);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = inds.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(inds.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (ME2Indicator ind in inds)
            {
                dbMemberSquaredQ1 = Math.Pow((ind.IndTotal - stat.TotalME2Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        stat.TotalME2Median = (ind.IndTotal + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        stat.TotalME2Median = ind.IndTotal;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = ind.IndTotal;
            }

            //don't divide by zero
            if (stat.TotalME2N > 1)
            {
                //sample variance
                double dbCount = (1 / (stat.TotalME2N - 1));
                stat.TotalME2Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (stat.TotalME2Mean != 0)
                {
                    //sample standard deviation
                    stat.TotalME2StandDev = Math.Sqrt(stat.TotalME2Variance);
                }
            }
            else
            {
                stat.TotalME2Variance = 0;
                stat.TotalME2StandDev = 0;
            }
        }
        private static void SetME2Q1TotalStatistics(ME2Stat1 stat)
        {
            //reorder for median
            IEnumerable<ME2Indicator> inds = stat.ME2Indicators.OrderByDescending(s => s.IndTotal);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = inds.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(inds.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (ME2Indicator ind in inds)
            {
                dbMemberSquaredQ1 = Math.Pow((ind.Ind1Amount - stat.TotalME2Q1Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        stat.TotalME2Q1Median = (ind.Ind1Amount + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        stat.TotalME2Q1Median = ind.Ind1Amount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = ind.Ind1Amount;
            }

            //don't divide by zero
            if (stat.TotalME2N > 1)
            {
                //sample variance
                double dbCount = (1 / (stat.TotalME2N - 1));
                stat.TotalME2Q1Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (stat.TotalME2Q1Mean != 0)
                {
                    //sample standard deviation
                    stat.TotalME2Q1StandDev = Math.Sqrt(stat.TotalME2Q1Variance);
                }
            }
            else
            {
                stat.TotalME2Q1Variance = 0;
                stat.TotalME2Q1StandDev = 0;
            }
        }
        private static void SetME2Q2TotalStatistics(ME2Stat1 stat)
        {
            //reorder for median
            IEnumerable<ME2Indicator> inds = stat.ME2Indicators.OrderByDescending(s => s.IndTotal);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = inds.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(inds.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (ME2Indicator ind in inds)
            {
                dbMemberSquaredQ1 = Math.Pow((ind.Ind2Amount - stat.TotalME2Q2Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        stat.TotalME2Q2Median = (ind.Ind2Amount + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        stat.TotalME2Q2Median = ind.Ind2Amount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = ind.Ind2Amount;
            }

            //don't divide by zero
            if (stat.TotalME2N > 1)
            {
                //sample variance
                double dbCount = (1 / (stat.TotalME2N - 1));
                stat.TotalME2Q2Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (stat.TotalME2Q2Mean != 0)
                {
                    //sample standard deviation
                    stat.TotalME2Q2StandDev = Math.Sqrt(stat.TotalME2Q2Variance);
                }
            }
            else
            {
                stat.TotalME2Q2Variance = 0;
                stat.TotalME2Q2StandDev = 0;
            }
        }
    }
    
}
