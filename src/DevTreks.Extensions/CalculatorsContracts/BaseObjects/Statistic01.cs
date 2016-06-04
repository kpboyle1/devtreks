using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

using DataHelpers = DevTreks.Data.Helpers;
using DataAppHelpers = DevTreks.Data.AppHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The Statistic01 derives is a base class used analyzers 
    ///             that need basic statistics (mean, standard deviation, variance,
    ///             median). The virtual methods are meant 
    ///             to be overridden because some analyses, due to file size 
    ///             and performance issues, need to limit the properties 
    ///             used in an object and subsequently deserialized to 
    ///             an xelement's attributes.
    ///             Helpers.AnalyzerParameters has a member of this type.
    ///Author:		www.devtreks.org
    ///Date:		2013, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    ///</summary>          
    public class Statistic01 
    {
        //calls the base-class version, and initializes the base class properties.
        public Statistic01()
            : base()
        {
            //avoid null references
            InitStatistic01sProperties();
        }
        //copy constructor
        public Statistic01(Statistic01 calculator)
        {
            CopyStatistic01sProperties(calculator);
        }
        //list of statistics (for graphs) 
        public List<Statistic01> Stat1s = new List<Statistic01>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfIndicator1s = 20;
        //general
        public double Total { get; set; }
        public double Mean { get; set; }
        public double Variance { get; set; }
        public double StandardDeviation { get; set; }
        public double Median { get; set; }
        public double Observations { get; set; }
        //5% confidence interval total
        public double CI5Percent { get; set; }
        //95% confidence interval total
        public double CI95Percent { get; set; }
        //basic stat substring
        public const string cTotal = "Total";
        public const string cMean = "Mean";
        public const string cVariance = "Variance";
        public const string cStandardDeviation = "StandardDeviation";
        public const string cMedian = "Median";
        public const string cObservations = "Observations";
        public const string cCI5Percent = "CI5Percent";
        public const string cCI95Percent = "CI95Percent";

        public virtual void InitStatistic01sProperties()
        {
            if (this.Stat1s == null)
            {
                this.Stat1s = new List<Statistic01>();
            }
            foreach (Statistic01 stat in this.Stat1s)
            {
                InitStatistic01Properties(stat);
            }
        }
        public void InitStatistic01Properties(Statistic01 stat)
        {
            //avoid null references
            stat.Total = 0;
            stat.Mean = 0;
            stat.Variance = 0;
            stat.StandardDeviation = 0;
            stat.Median = 0;
            stat.Observations = 0;
            stat.CI5Percent = 0;
            stat.CI95Percent = 0;
        }
        //copy method
        public virtual void CopyStatistic01sProperties(Statistic01 calculator)
        {
            if (calculator.Stat1s != null)
            {
                if (this.Stat1s == null)
                {
                    this.Stat1s = new List<Statistic01>();
                }
                foreach (Statistic01 calculatorStat in calculator.Stat1s)
                {
                    Statistic01 stat = new Statistic01();
                    CopyStatistic01Properties(stat, calculatorStat);
                    this.Stat1s.Add(stat);
                }
            }
        }
        private void CopyStatistic01Properties(
            Statistic01 stat, Statistic01 calculatorStat)
        {
            //copy this object's properties
            stat.Total = calculatorStat.Total;
            stat.Mean = calculatorStat.Mean;
            stat.Variance = calculatorStat.Variance;
            stat.StandardDeviation = calculatorStat.StandardDeviation;
            stat.Median = calculatorStat.Median;
            stat.Observations = calculatorStat.Observations;
            stat.CI5Percent = calculatorStat.CI5Percent;
            stat.CI95Percent = calculatorStat.CI95Percent;
        }
        public virtual void SetStatistic01sProperties(XElement calculator)
        {
            if (this.Stat1s == null)
            {
                this.Stat1s = new List<Statistic01>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfIndicator1s; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cTotal, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    Statistic01 stat = new Statistic01();
                    SetStatistic01Properties(stat, sAttNameExtension, calculator);
                    this.Stat1s.Add(stat);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetStatistic01Properties(Statistic01 stat, string attNameExtension,
            XElement calculator)
        {
            stat.Total = CalculatorHelpers.GetAttributeDouble(calculator,
                cTotal);
            stat.Mean = CalculatorHelpers.GetAttributeDouble(calculator,
                cMean);
            stat.Variance = CalculatorHelpers.GetAttributeDouble(calculator,
                cVariance);
            stat.StandardDeviation = CalculatorHelpers.GetAttributeDouble(calculator,
                cStandardDeviation);
            stat.Median = CalculatorHelpers.GetAttributeDouble(calculator,
                cMedian);
            stat.Observations = CalculatorHelpers.GetAttributeDouble(calculator,
                cObservations);
            stat.CI5Percent = CalculatorHelpers.GetAttributeDouble(calculator,
                cCI5Percent);
            stat.CI95Percent = CalculatorHelpers.GetAttributeDouble(calculator,
                cCI95Percent);
        }
        public virtual void SetStatistic01Property(string attName,
           string attValue, int colIndex)
        {
            if (this.Stat1s == null)
            {
                this.Stat1s = new List<Statistic01>();
            }
            if (this.Stat1s.Count < (colIndex + 1))
            {
                Statistic01 stat1 = new Statistic01();
                this.Stat1s.Insert(colIndex, stat1);
            }
            Statistic01 stat = this.Stat1s.ElementAt(colIndex);
            if (stat != null)
            {
                SetStatistic01Property(stat, attName, attValue);
            }
        }
        private void SetStatistic01Property(Statistic01 stat, 
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotal:
                    stat.Total
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMean:
                    stat.Mean
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cVariance:
                    stat.Variance
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cStandardDeviation:
                    stat.StandardDeviation
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cMedian:
                    stat.Median
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cObservations:
                    stat.Observations
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCI5Percent:
                    stat.CI5Percent
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCI95Percent:
                    stat.CI95Percent
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual void SetStatistic01sAttributes(ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
            if (this.Stat1s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (Statistic01 stat in this.Stat1s)
                {
                    sAttNameExtension = i.ToString();
                    SetStatistic01Attributes(stat, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetStatistic01Attributes(Statistic01 stat, string attNameExtension,
            ref XElement calculator)
        {
            //copy this object's properties
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cTotal, attNameExtension),
               stat.Total);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cMean, attNameExtension),
               stat.Mean);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cVariance, attNameExtension),
               stat.Variance);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cStandardDeviation, attNameExtension),
               stat.StandardDeviation);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cMedian, attNameExtension),
               stat.Median);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cObservations, attNameExtension),
               stat.Observations);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cCI5Percent, attNameExtension),
               stat.CI5Percent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cCI95Percent, attNameExtension),
               stat.CI95Percent);
        }
        public virtual void SetStatistic01sAttributes(ref XmlWriter writer)
        {
            if (this.Stat1s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (Statistic01 stat in this.Stat1s)
                {
                    sAttNameExtension = i.ToString();
                    SetStatistic01Attributes(stat, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public virtual void SetStatistic01Attributes(Statistic01 stat, string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(cTotal, attNameExtension),
               stat.Total.ToString());
            writer.WriteAttributeString(
               string.Concat(cMean, attNameExtension),
              stat.Mean.ToString());
            writer.WriteAttributeString(
               string.Concat(cVariance, attNameExtension),
              stat.Variance.ToString());
            writer.WriteAttributeString(
               string.Concat(cStandardDeviation, attNameExtension),
              stat.StandardDeviation.ToString());
            writer.WriteAttributeString(
               string.Concat(cMedian, attNameExtension),
              stat.Median.ToString());
            writer.WriteAttributeString(
               string.Concat(cObservations, attNameExtension),
              stat.Observations.ToString());
            writer.WriteAttributeString(
               string.Concat(cCI5Percent, attNameExtension),
              stat.CI5Percent.ToString());
            writer.WriteAttributeString(
               string.Concat(cCI95Percent, attNameExtension),
              stat.CI95Percent.ToString());
        }
        
    }
}
