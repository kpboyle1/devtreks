using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a generic factor demographic object with
    ///             properties. This object is generally inserted into 
    ///             other calculators to provide demographic support. 
    ///Author:		www.devtreks.org
    ///Date:		2013, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Most data manipulation takes place using the collection property.
    public class Demog4
    {
        public Demog4()
            : base()
        {
            //health care benefit object
            InitDemog4sProperties();
        }
        //copy constructor
        public Demog4(Demog4 demog11)
        {
            CopyDemog4sProperties(demog11);
        }
        //list of Demog4 
        public List<Demog4> Demog4s = new List<Demog4>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfDemog4s = 5;
        //properties
        public string D4Name { get; set; }
        public string D4Label { get; set; }
        public string D4Description { get; set; }
        public string D4NameA { get; set; }
        public string D4LabelA { get; set; }
        //string so that it can be quantity (height = 175) or description (race = hispanic)
        public string D4AmountA { get; set; }
        //population (1650 children with height = 175)
        public string D4PopA { get; set; }
        public string D4UnitA { get; set; }
        public string D4NameB { get; set; }
        public string D4LabelB { get; set; }
        public string D4AmountB { get; set; }
        public string D4PopB { get; set; }
        public string D4UnitB { get; set; }
        public string D4NameC { get; set; }
        public string D4LabelC { get; set; }
        public string D4AmountC { get; set; }
        public string D4PopC { get; set; }
        public string D4UnitC { get; set; }
        public string D4NameD { get; set; }
        public string D4LabelD { get; set; }
        public string D4AmountD { get; set; }
        public string D4PopD { get; set; }
        public string D4UnitD { get; set; }
        public string D4NameE { get; set; }
        public string D4LabelE { get; set; }
        public string D4AmountE { get; set; }
        public string D4PopE { get; set; }
        public string D4UnitE { get; set; }

        private const string cD4Name = "D4Name";
        private const string cD4Label = "D4Label";
        private const string cD4Description = "D4Description";
        private const string cD4NameA = "D4NameA";
        private const string cD4LabelA = "D4LabelA";
        private const string cD4AmountA = "D4AmountA";
        private const string cD4PopA = "D4PopA";
        private const string cD4UnitA = "D4UnitA";
        private const string cD4NameB = "D4NameB";
        private const string cD4LabelB = "D4LabelB";
        private const string cD4AmountB = "D4AmountB";
        private const string cD4PopB = "D4PopB";
        private const string cD4UnitB = "D4UnitB";
        private const string cD4NameC = "D4NameC";
        private const string cD4LabelC = "D4LabelC";
        private const string cD4AmountC = "D4AmountC";
        private const string cD4PopC = "D4PopC";
        private const string cD4UnitC = "D4UnitC";
        private const string cD4NameD = "D4NameD";
        private const string cD4LabelD = "D4LabelD";
        private const string cD4AmountD = "D4AmountD";
        private const string cD4PopD = "D4PopD";
        private const string cD4UnitD = "D4UnitD";
        private const string cD4NameE = "D4NameE";
        private const string cD4LabelE = "D4LabelE";
        private const string cD4AmountE = "D4AmountE";
        private const string cD4PopE = "D4PopE";
        private const string cD4UnitE = "D4UnitE";
        public virtual void InitDemog4sProperties()
        {
            if (this.Demog4s == null)
            {
                this.Demog4s = new List<Demog4>();
            }
            foreach (Demog4 demog in this.Demog4s)
            {
                InitDemog4Properties(demog);
            }
        }
        private void InitDemog4Properties(Demog4 demog)
        {
            //avoid null references to properties
            demog.D4Name = string.Empty;
            demog.D4Label = string.Empty;
            demog.D4Description = string.Empty;
            demog.D4NameA = string.Empty;
            demog.D4LabelA = string.Empty;
            demog.D4AmountA = string.Empty;
            demog.D4PopA = string.Empty;
            demog.D4UnitA = string.Empty;
            demog.D4NameB = string.Empty;
            demog.D4LabelB = string.Empty;
            demog.D4AmountB = string.Empty;
            demog.D4PopB = string.Empty;
            demog.D4UnitB = string.Empty;
            demog.D4NameC = string.Empty;
            demog.D4LabelC = string.Empty;
            demog.D4AmountC = string.Empty;
            demog.D4PopC = string.Empty;
            demog.D4UnitC = string.Empty;
            demog.D4NameD = string.Empty;
            demog.D4LabelD = string.Empty;
            demog.D4AmountD = string.Empty;
            demog.D4PopD = string.Empty;
            demog.D4UnitD = string.Empty;
            demog.D4NameE = string.Empty;
            demog.D4LabelE = string.Empty;
            demog.D4AmountE = string.Empty;
            demog.D4PopE = string.Empty;
            demog.D4UnitE = string.Empty;
        }
        public virtual void CopyDemog4sProperties(
            Demog4 calculator)
        {
            if (calculator.Demog4s != null)
            {
                foreach (Demog4 calculatorInd in calculator.Demog4s)
                {
                    foreach (Demog4 demog in this.Demog4s)
                    {
                        CopyDemog4Properties(demog, calculatorInd);
                    }
                }
            }
        }
        private void CopyDemog4Properties(Demog4 demog,
            Demog4 calculator)
        {
            demog.D4Name = calculator.D4Name;
            demog.D4Label = calculator.D4Label;
            demog.D4Description = calculator.D4Description;
            demog.D4NameA = calculator.D4NameA;
            demog.D4LabelA = calculator.D4LabelA;
            demog.D4AmountA = calculator.D4AmountA;
            demog.D4PopA = calculator.D4PopA;
            demog.D4UnitA = calculator.D4UnitA;
            demog.D4NameB = calculator.D4NameB;
            demog.D4LabelB = calculator.D4LabelB;
            demog.D4AmountB = calculator.D4AmountB;
            demog.D4PopB = calculator.D4PopB;
            demog.D4UnitB = calculator.D4UnitB;
            demog.D4NameC = calculator.D4NameC;
            demog.D4LabelC = calculator.D4LabelC;
            demog.D4AmountC = calculator.D4AmountC;
            demog.D4PopC = calculator.D4PopC;
            demog.D4UnitC = calculator.D4UnitC;
            demog.D4NameD = calculator.D4NameD;
            demog.D4LabelD = calculator.D4LabelD;
            demog.D4AmountD = calculator.D4AmountD;
            demog.D4PopD = calculator.D4PopD;
            demog.D4UnitD = calculator.D4UnitD;
            demog.D4NameE = calculator.D4NameE;
            demog.D4LabelE = calculator.D4LabelE;
            demog.D4AmountE = calculator.D4AmountE;
            demog.D4PopE = calculator.D4PopE;
            demog.D4UnitE = calculator.D4UnitE;
        }
        public virtual void SetDemog4sProperties(XElement calculator)
        {
            if (this.Demog4s == null)
            {
                this.Demog4s = new List<Demog4>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfDemog4s; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cD4Name, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    Demog4 demog = new Demog4();
                    SetDemog4Properties(demog, sAttNameExtension, calculator);
                    this.Demog4s.Add(demog);
                }
                sHasAttribute = string.Empty;
            }
        }
        //set the class properties using the XElement
        private void SetDemog4Properties(Demog4 demog, string attNameExtension, 
            XElement currentCalculationsElement)
        {
            //don't set any input properties; each calculator should set what's needed separately
            demog.D4Name = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4Name, attNameExtension));
            demog.D4Label = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4Label, attNameExtension));
            demog.D4Description = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4Description, attNameExtension));
            demog.D4NameA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4NameA, attNameExtension));
            demog.D4LabelA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4LabelA, attNameExtension));
            demog.D4AmountA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4AmountA, attNameExtension));
            demog.D4PopA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4PopA, attNameExtension));
            demog.D4UnitA = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4UnitA, attNameExtension));
            demog.D4NameB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4NameB, attNameExtension));
            demog.D4LabelB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4LabelB, attNameExtension));
            demog.D4AmountB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4AmountB, attNameExtension));
            demog.D4PopB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4PopB, attNameExtension));
            demog.D4UnitB = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4UnitB, attNameExtension));
            demog.D4NameC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4NameC, attNameExtension));
            demog.D4LabelC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4LabelC, attNameExtension));
            demog.D4AmountC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4AmountC, attNameExtension));
            demog.D4PopC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4PopC, attNameExtension));
            demog.D4UnitC = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4UnitC, attNameExtension));
            demog.D4NameD = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4NameD, attNameExtension));
            demog.D4LabelD = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4LabelD, attNameExtension));
            demog.D4AmountD = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4AmountD, attNameExtension));
            demog.D4PopD = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4PopD, attNameExtension));
            demog.D4UnitD = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4UnitD, attNameExtension));
            demog.D4NameE = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4NameE, attNameExtension));
            demog.D4LabelE = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4LabelE, attNameExtension));
            demog.D4AmountE = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4AmountE, attNameExtension));
            demog.D4PopE = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4PopE, attNameExtension));
            demog.D4UnitE = CalculatorHelpers.GetAttribute(currentCalculationsElement, string.Concat(cD4UnitE, attNameExtension));
        }
        public virtual void SetDemog4sProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.Demog4s == null)
            {
                this.Demog4s = new List<Demog4>();
            }
            if (this.Demog4s.Count < (colIndex + 1))
            {
                Demog4 demog1 = new Demog4();
                this.Demog4s.Insert(colIndex, demog1);
            }
            Demog4 demog = this.Demog4s.ElementAt(colIndex);
            if (demog != null)
            {
                SetDemog4Property(demog, attName, attValue);
            }
        }
        //attname and attvalue generally passed in from a reader
        private void SetDemog4Property(Demog4 demog, string attName,
            string attValue)
        {
            switch (attName)
            {
                case cD4Name:
                    demog.D4Name = attValue;
                    break;
                case cD4Label:
                    demog.D4Label = attValue;
                    break;
                case cD4Description:
                    demog.D4Description = attValue;
                    break;
                case cD4NameA:
                    demog.D4NameA = attValue;
                    break;
                case cD4LabelA:
                    demog.D4LabelA = attValue;
                    break;
                case cD4AmountA:
                    demog.D4AmountA = attValue;
                    break;
                case cD4PopA:
                    demog.D4PopA = attValue;
                    break;
                case cD4UnitA:
                    demog.D4UnitA = attValue;
                    break;
                case cD4NameB:
                    demog.D4NameB = attValue;
                    break;
                case cD4LabelB:
                    demog.D4LabelB = attValue;
                    break;
                case cD4AmountB:
                    demog.D4AmountB = attValue;
                    break;
                case cD4PopB:
                    demog.D4PopB = attValue;
                    break;
                case cD4UnitB:
                    demog.D4UnitB = attValue;
                    break;
                case cD4NameC:
                    demog.D4NameC = attValue;
                    break;
                case cD4LabelC:
                    demog.D4LabelC = attValue;
                    break;
                case cD4AmountC:
                    demog.D4AmountC = attValue;
                    break;
                case cD4PopC:
                    demog.D4PopC = attValue;
                    break;
                case cD4UnitC:
                    demog.D4UnitC = attValue;
                    break;
                case cD4NameD:
                    demog.D4NameD = attValue;
                    break;
                case cD4LabelD:
                    demog.D4LabelD = attValue;
                    break;
                case cD4AmountD:
                    demog.D4AmountD = attValue;
                    break;
                case cD4PopD:
                    demog.D4PopD = attValue;
                    break;
                case cD4UnitD:
                    demog.D4UnitD = attValue;
                    break;
                case cD4NameE:
                    demog.D4NameE = attValue;
                    break;
                case cD4LabelE:
                    demog.D4LabelE = attValue;
                    break;
                case cD4AmountE:
                    demog.D4AmountE = attValue;
                    break;
                case cD4PopE:
                    demog.D4PopE = attValue;
                    break;
                case cD4UnitE:
                    demog.D4UnitE = attValue;
                    break;
                default:
                    break;
            }
        }
        //public virtual string GetDemog4sProperty(string attName, int colIndex)
        //{
        //    string sPropertyValue = string.Empty;
        //    if (this.Demog4s.Count >= (colIndex + 1))
        //    {
        //        Demog4 demog = this.Demog4s.ElementAt(colIndex);
        //        if (demog != null)
        //        {
        //            sPropertyValue = GetDemog4Property(demog, attName);
        //        }
        //    }
        //    return sPropertyValue;
        //}
        public virtual void SetDemog4sAttributes(ref XElement calculator)
        {
            if (this.Demog4s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (Demog4 demog in this.Demog4s)
                {
                    sAttNameExtension = i.ToString();
                    SetDemog4Attributes(demog, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetDemog4Attributes(Demog4 demog, string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            if (demog.D4Name != string.Empty && demog.D4Name != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4Name, attNameExtension), demog.D4Name);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4Label, attNameExtension), demog.D4Label);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4Description, attNameExtension), demog.D4Description);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4NameA, attNameExtension), demog.D4NameA);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4LabelA, attNameExtension), demog.D4LabelA);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4AmountA, attNameExtension), demog.D4AmountA);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4PopA, attNameExtension), demog.D4PopA);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4UnitA, attNameExtension), demog.D4UnitA);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4NameB, attNameExtension), demog.D4NameB);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                   string.Concat(cD4LabelB, attNameExtension), demog.D4LabelB);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4AmountB, attNameExtension), demog.D4AmountB);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4PopB, attNameExtension), demog.D4PopB);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4UnitB, attNameExtension), demog.D4UnitB);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4NameC, attNameExtension), demog.D4NameC);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                   string.Concat(cD4LabelC, attNameExtension), demog.D4LabelC);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4AmountC, attNameExtension), demog.D4AmountC);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4PopC, attNameExtension), demog.D4PopC);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4UnitC, attNameExtension), demog.D4UnitC);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4NameD, attNameExtension), demog.D4NameD);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                   string.Concat(cD4LabelD, attNameExtension), demog.D4LabelD);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4AmountD, attNameExtension), demog.D4AmountD);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4PopD, attNameExtension), demog.D4PopD);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4UnitD, attNameExtension), demog.D4UnitD);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4NameE, attNameExtension), demog.D4NameE);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                   string.Concat(cD4LabelE, attNameExtension), demog.D4LabelE);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4AmountE, attNameExtension), demog.D4AmountE);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4PopE, attNameExtension), demog.D4PopE);
                CalculatorHelpers.SetAttribute(currentCalculationsElement,
                    string.Concat(cD4UnitE, attNameExtension), demog.D4UnitE);
            }
        }
        public virtual void SetDemog4sAttributes(ref XmlWriter writer)
        {
            if (this.Demog4s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (Demog4 demog in this.Demog4s)
                {
                    sAttNameExtension = i.ToString();
                    SetDemog4Attributes(demog, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        private void SetDemog4Attributes(Demog4 demog, string attNameExtension,
           ref XmlWriter writer)
        {
            if (demog.D4Name != string.Empty && demog.D4Name != Constants.NONE)
            {
                writer.WriteAttributeString(string.Concat(cD4Name, attNameExtension), demog.D4Name.ToString());
                writer.WriteAttributeString(string.Concat(cD4Label, attNameExtension), demog.D4Label.ToString());
                writer.WriteAttributeString(string.Concat(cD4Description, attNameExtension), demog.D4Description.ToString());
                writer.WriteAttributeString(string.Concat(cD4NameA, attNameExtension), demog.D4NameA.ToString());
                writer.WriteAttributeString(string.Concat(cD4LabelA, attNameExtension), demog.D4LabelA.ToString());
                writer.WriteAttributeString(string.Concat(cD4AmountA, attNameExtension), demog.D4AmountA.ToString());
                writer.WriteAttributeString(string.Concat(cD4PopA, attNameExtension), demog.D4PopA.ToString());
                writer.WriteAttributeString(string.Concat(cD4UnitA, attNameExtension), demog.D4UnitA.ToString());
                writer.WriteAttributeString(string.Concat(cD4NameB, attNameExtension), demog.D4NameB);
                writer.WriteAttributeString(string.Concat(cD4LabelB, attNameExtension), demog.D4LabelB.ToString());
                writer.WriteAttributeString(string.Concat(cD4AmountB, attNameExtension), demog.D4AmountB.ToString());
                writer.WriteAttributeString(string.Concat(cD4PopB, attNameExtension), demog.D4PopB.ToString());
                writer.WriteAttributeString(string.Concat(cD4UnitB, attNameExtension), demog.D4UnitB.ToString());
                writer.WriteAttributeString(string.Concat(cD4NameC, attNameExtension), demog.D4NameC);
                writer.WriteAttributeString(string.Concat(cD4LabelC, attNameExtension), demog.D4LabelC.ToString());
                writer.WriteAttributeString(string.Concat(cD4AmountC, attNameExtension), demog.D4AmountC.ToString());
                writer.WriteAttributeString(string.Concat(cD4PopC, attNameExtension), demog.D4PopC.ToString());
                writer.WriteAttributeString(string.Concat(cD4UnitC, attNameExtension), demog.D4UnitC.ToString());
                writer.WriteAttributeString(string.Concat(cD4NameD, attNameExtension), demog.D4NameD.ToString());
                writer.WriteAttributeString(string.Concat(cD4LabelD, attNameExtension), demog.D4LabelD.ToString());
                writer.WriteAttributeString(string.Concat(cD4AmountD, attNameExtension), demog.D4AmountD.ToString());
                writer.WriteAttributeString(string.Concat(cD4PopD, attNameExtension), demog.D4PopD.ToString());
                writer.WriteAttributeString(string.Concat(cD4UnitD, attNameExtension), demog.D4UnitD.ToString());
                writer.WriteAttributeString(string.Concat(cD4NameE, attNameExtension), demog.D4NameE);
                writer.WriteAttributeString(string.Concat(cD4LabelE, attNameExtension), demog.D4LabelE.ToString());
                writer.WriteAttributeString(string.Concat(cD4AmountE, attNameExtension), demog.D4AmountE.ToString());
                writer.WriteAttributeString(string.Concat(cD4PopE, attNameExtension), demog.D4PopE.ToString());
                writer.WriteAttributeString(string.Concat(cD4UnitE, attNameExtension), demog.D4UnitE.ToString());
            }
        }
    }
}
