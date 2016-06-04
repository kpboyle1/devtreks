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
    ///Purpose:		The Statistic02 derives is a base class used analyzers 
    ///             that need basic probability statistics (amount, likelihood). 
    ///             The virtual methods are meant 
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
    public class Statistic02
    {
        //calls the base-class version, and initializes the base class properties.
        public Statistic02()
            : base()
        {
            //avoid null references
            InitStatistic02sProperties();
        }
        //copy constructor
        public Statistic02(Statistic02 calculator)
        {
            CopyStatistic02sProperties(calculator);
        }
        //list of statistics (for graphs) 
        public List<Statistic02> Stat2s = new List<Statistic02>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfIndicator1s = 50000;
        //general
        public double Amount { get; set; }
        public double Likelihood { get; set; }
        //basic stat substring
        public const string cAmount = "Amount";
        public const string cLikelihood = "Likelihood";


        public virtual void InitStatistic02sProperties()
        {
            if (this.Stat2s == null)
            {
                this.Stat2s = new List<Statistic02>();
            }
            foreach (Statistic02 stat in this.Stat2s)
            {
                InitStatistic02Properties(stat);
            }
        }
        public void InitStatistic02Properties(Statistic02 stat)
        {
            //avoid null references
            stat.Amount = 0;
            stat.Likelihood = 0;
        }
        //copy method
        public virtual void CopyStatistic02sProperties(Statistic02 calculator)
        {
            if (calculator.Stat2s != null)
            {
                if (this.Stat2s == null)
                {
                    this.Stat2s = new List<Statistic02>();
                }
                foreach (Statistic02 calculatorStat in calculator.Stat2s)
                {
                    Statistic02 stat = new Statistic02();
                    CopyStatistic02Properties(stat, calculatorStat);
                    this.Stat2s.Add(stat);
                }
            }
        }
        private void CopyStatistic02Properties(
            Statistic02 stat, Statistic02 calculatorStat)
        {
            //copy this object's properties
            stat.Amount = calculatorStat.Amount;
            stat.Likelihood = calculatorStat.Likelihood;
        }
        public virtual void SetStatistic02sProperties(XElement calculator)
        {
            if (this.Stat2s == null)
            {
                this.Stat2s = new List<Statistic02>();
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
                    string.Concat(cAmount, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    Statistic02 stat = new Statistic02();
                    SetStatistic02Properties(stat, sAttNameExtension, calculator);
                    this.Stat2s.Add(stat);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetStatistic02Properties(Statistic02 stat, string attNameExtension,
            XElement calculator)
        {
            stat.Amount = CalculatorHelpers.GetAttributeDouble(calculator,
                cAmount);
            stat.Likelihood = CalculatorHelpers.GetAttributeDouble(calculator,
                cLikelihood);
        }
        public virtual void SetStatistic02Property(string attName,
           string attValue, int colIndex)
        {
            if (this.Stat2s == null)
            {
                this.Stat2s = new List<Statistic02>();
            }
            if (this.Stat2s.Count < (colIndex + 1))
            {
                Statistic02 stat1 = new Statistic02();
                this.Stat2s.Insert(colIndex, stat1);
            }
            Statistic02 stat = this.Stat2s.ElementAt(colIndex);
            if (stat != null)
            {
                SetStatistic02Property(stat, attName, attValue);
            }
        }
        private void SetStatistic02Property(Statistic02 stat,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cAmount:
                    stat.Amount
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLikelihood:
                    stat.Likelihood
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual void SetStatistic02sAttributes(ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
            if (this.Stat2s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (Statistic02 stat in this.Stat2s)
                {
                    sAttNameExtension = i.ToString();
                    SetStatistic02Attributes(stat, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetStatistic02Attributes(Statistic02 stat, string attNameExtension,
            ref XElement calculator)
        {
            //copy this object's properties
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cAmount, attNameExtension),
               stat.Amount);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(cLikelihood, attNameExtension),
               stat.Likelihood);
        }
        public virtual void SetStatistic02sAttributes(ref XmlWriter writer)
        {
            if (this.Stat2s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (Statistic02 stat in this.Stat2s)
                {
                    sAttNameExtension = i.ToString();
                    SetStatistic02Attributes(stat, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public virtual void SetStatistic02Attributes(Statistic02 stat, string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(cAmount, attNameExtension),
               stat.Amount.ToString());
            writer.WriteAttributeString(
               string.Concat(cLikelihood, attNameExtension),
              stat.Likelihood.ToString());
        }

    }
}
