using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Serialize and deserialize a generic factor indQ indicator object with
    ///             properties. This object is generally inserted into 
    ///             other calculators to define uncertain performance parameters that 
    ///             are used in indQ analysis. 
    ///Author:		www.devtreks.org
    ///Date:		2013, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Most data manipulation takes place using the collection property.
    /// </summary>
    public class IndicatorQ1
    {
        public IndicatorQ1()
            : base()
        {
            InitIndicatorQ1sProperties();
        }
        //copy constructor
        public IndicatorQ1(IndicatorQ1 indQ1)
        {
            CopyIndicatorQ1sProperties(indQ1);
        }
        //list of IndicatorQ1 
        public List<IndicatorQ1> IndicatorQ1s = new List<IndicatorQ1>();
        //maximum limit for reasonable serialization in parent (i.e. flood damage exceedance)
        private int MaximumNumberOfIndicatorQ1s = 10;
        //properties
        public string IQ1NameA { get; set; }
        public string IQ1AmountA { get; set; }
        public string IQ1UnitA { get; set; }
        public string IQ1NameB { get; set; }
        public string IQ1AmountB { get; set; }
        public string IQ1UnitB { get; set; }
        public string IQ1NameC { get; set; }
        public string IQ1AmountC { get; set; }
        public string IQ1UnitC { get; set; }

        private const string cIQ1NameA = "IQ1NameA";
        private const string cIQ1AmountA = "IQ1AmountA";
        private const string cIQ1UnitA = "IQ1UnitA";
        private const string cIQ1NameB = "IQ1NameB";
        private const string cIQ1AmountB = "IQ1AmountB";
        private const string cIQ1UnitB = "IQ1UnitB";
        private const string cIQ1NameC = "IQ1NameC";
        private const string cIQ1AmountC = "IQ1AmountC";
        private const string cIQ1UnitC = "IQ1UnitC";
        public virtual void InitIndicatorQ1sProperties()
        {
            if (this.IndicatorQ1s == null)
            {
                this.IndicatorQ1s = new List<IndicatorQ1>();
            }
            foreach (IndicatorQ1 indQ in this.IndicatorQ1s)
            {
                InitIndicatorQ1Properties(indQ);
            }
        }
        private void InitIndicatorQ1Properties(IndicatorQ1 indQ)
        {
            //avoid null references to properties
            indQ.IQ1NameA = string.Empty;
            indQ.IQ1AmountA = string.Empty;
            indQ.IQ1UnitA = string.Empty;
            indQ.IQ1NameB = string.Empty;
            indQ.IQ1AmountB = string.Empty;
            indQ.IQ1UnitB = string.Empty;
            indQ.IQ1NameC = string.Empty;
            indQ.IQ1AmountC = string.Empty;
            indQ.IQ1UnitC = string.Empty;
        }
        public virtual void CopyIndicatorQ1sProperties(
            IndicatorQ1 calculator)
        {
            if (calculator.IndicatorQ1s != null)
            {
                foreach (IndicatorQ1 calculatorInd in calculator.IndicatorQ1s)
                {
                    foreach (IndicatorQ1 indQ in this.IndicatorQ1s)
                    {
                        CopyIndicatorQ1Properties(indQ, calculatorInd);
                    }
                }
            }
        }
        private void CopyIndicatorQ1Properties(IndicatorQ1 indQ,
            IndicatorQ1 calculator)
        {
            indQ.IQ1NameA = calculator.IQ1NameA;
            indQ.IQ1AmountA = calculator.IQ1AmountA;
            indQ.IQ1UnitA = calculator.IQ1UnitA;
            indQ.IQ1NameB = calculator.IQ1NameB;
            indQ.IQ1AmountB = calculator.IQ1AmountB;
            indQ.IQ1UnitB = calculator.IQ1UnitB;
            indQ.IQ1NameC = calculator.IQ1NameC;
            indQ.IQ1AmountC = calculator.IQ1AmountC;
            indQ.IQ1UnitC = calculator.IQ1UnitC;
        }
        public virtual void SetIndicatorQ1sProperties(XElement calculator)
        {
            if (this.IndicatorQ1s == null)
            {
                this.IndicatorQ1s = new List<IndicatorQ1>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfIndicatorQ1s; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cIQ1NameA, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    IndicatorQ1 indQ = new IndicatorQ1();
                    SetIndicatorQ1Properties(indQ, sAttNameExtension, calculator);
                    this.IndicatorQ1s.Add(indQ);
                }
                sHasAttribute = string.Empty;
            }
        }
        //set the class properties using the XElement
        private void SetIndicatorQ1Properties(IndicatorQ1 indQ, string attNameExtension,
            XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            indQ.IQ1NameA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1NameA, attNameExtension));
            indQ.IQ1AmountA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1AmountA, attNameExtension));
            indQ.IQ1UnitA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1UnitA, attNameExtension));
            indQ.IQ1NameB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1NameB, attNameExtension));
            indQ.IQ1AmountB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1AmountB, attNameExtension));
            indQ.IQ1UnitB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1UnitB, attNameExtension));
            indQ.IQ1NameC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1NameC, attNameExtension));
            indQ.IQ1AmountC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1AmountC, attNameExtension));
            indQ.IQ1UnitC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cIQ1UnitC, attNameExtension));
        }
        public virtual void SetIndicatorQ1sProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.IndicatorQ1s == null)
            {
                this.IndicatorQ1s = new List<IndicatorQ1>();
            }
            if (this.IndicatorQ1s.Count < (colIndex + 1))
            {
                IndicatorQ1 indQ1 = new IndicatorQ1();
                this.IndicatorQ1s.Insert(colIndex, indQ1);
            }
            IndicatorQ1 indQ = this.IndicatorQ1s.ElementAt(colIndex);
            if (indQ != null)
            {
                SetIndicatorQ1Property(indQ, attName, attValue);
            }
        }
        //attname and attvalue generally passed in from a reader
        private void SetIndicatorQ1Property(IndicatorQ1 indQ, string attName,
            string attValue)
        {
            switch (attName)
            {
                case cIQ1NameA:
                    indQ.IQ1NameA = attValue;
                    break;
                case cIQ1AmountA:
                    indQ.IQ1AmountA = attValue;
                    break;
                case cIQ1UnitA:
                    indQ.IQ1UnitA = attValue;
                    break;
                case cIQ1NameB:
                    indQ.IQ1NameB = attValue;
                    break;
                case cIQ1AmountB:
                    indQ.IQ1AmountB = attValue;
                    break;
                case cIQ1UnitB:
                    indQ.IQ1UnitB = attValue;
                    break;
                case cIQ1NameC:
                    indQ.IQ1NameC = attValue;
                    break;
                case cIQ1AmountC:
                    indQ.IQ1AmountC = attValue;
                    break;
                case cIQ1UnitC:
                    indQ.IQ1UnitC = attValue;
                    break;
                default:
                    break;
            }
        }
        //public virtual string GetIndicatorQ1sProperty(string attName, int colIndex)
        //{
        //    string sPropertyValue = string.Empty;
        //    if (this.IndicatorQ1s.Count >= (colIndex + 1))
        //    {
        //        IndicatorQ1 indQ = this.IndicatorQ1s.ElementAt(colIndex);
        //        if (indQ != null)
        //        {
        //            sPropertyValue = GetIndicatorQ1Property(indQ, attName);
        //        }
        //    }
        //    return sPropertyValue;
        //}
        public virtual void SetIndicatorQ1sAttributes(ref XElement calculator)
        {
            if (this.IndicatorQ1s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (IndicatorQ1 indQ in this.IndicatorQ1s)
                {
                    sAttNameExtension = i.ToString();
                    SetIndicatorQ1Attributes(indQ, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetIndicatorQ1Attributes(IndicatorQ1 indQ, string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1NameA, attNameExtension), indQ.IQ1NameA);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1AmountA, attNameExtension), indQ.IQ1AmountA);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1UnitA, attNameExtension), indQ.IQ1UnitA);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1NameB, attNameExtension), indQ.IQ1NameB);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1AmountB, attNameExtension), indQ.IQ1AmountB);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1UnitB, attNameExtension), indQ.IQ1UnitB);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1NameC, attNameExtension), indQ.IQ1NameC);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1AmountC, attNameExtension), indQ.IQ1AmountC);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cIQ1UnitC, attNameExtension), indQ.IQ1UnitC);
        }
        public virtual void SetIndicatorQ1sAttributes(ref XmlWriter writer)
        {
            if (this.IndicatorQ1s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (IndicatorQ1 indQ in this.IndicatorQ1s)
                {
                    sAttNameExtension = i.ToString();
                    SetIndicatorQ1Attributes(indQ, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        private void SetIndicatorQ1Attributes(IndicatorQ1 indQ, string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cIQ1NameA, attNameExtension), indQ.IQ1NameA.ToString());
            writer.WriteAttributeString(string.Concat(cIQ1AmountA, attNameExtension), indQ.IQ1AmountA.ToString());
            writer.WriteAttributeString(string.Concat(cIQ1UnitA, attNameExtension), indQ.IQ1UnitA.ToString());
            writer.WriteAttributeString(string.Concat(cIQ1NameB, attNameExtension), indQ.IQ1NameB);
            writer.WriteAttributeString(string.Concat(cIQ1AmountB, attNameExtension), indQ.IQ1AmountB.ToString());
            writer.WriteAttributeString(string.Concat(cIQ1UnitB, attNameExtension), indQ.IQ1UnitB.ToString());
            writer.WriteAttributeString(string.Concat(cIQ1NameC, attNameExtension), indQ.IQ1NameC);
            writer.WriteAttributeString(string.Concat(cIQ1AmountC, attNameExtension), indQ.IQ1AmountC.ToString());
            writer.WriteAttributeString(string.Concat(cIQ1UnitC, attNameExtension), indQ.IQ1UnitC.ToString());
        }
    }
}
