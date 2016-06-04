using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Add subcosts or subbenefits to DevTreks input or output elements. 
    ///             Typical examples include contingencies, energy, garbage, water, 
    ///             repair in capital inputs. Environmental impact examples include 
    ///             carbon and SO2 which are both traded in markets and may have 'prices'.
    ///             Environmental factors include garbage and food contaminants for food 
    ///             nutrition.
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        1. 
    ///            
    /// </summary>   
    public class SubPrice1 : CostBenefitCalculator
    {
        //constructor
        public SubPrice1()
        {
            InitSubPrice1sProperties();
        }
        //copy constructors
        public SubPrice1(SubPrice1 calculator)
        {
            CopySubPrice1sProperties(calculator);
        }
        //indicators can be costs or revenues
        //by using the Ind1bAmount and Ind2bAmount as prices
        public enum PRICE_TYPE
        {
            none    = 0,
            rev     = 1,
            oc      = 2,
            aoh     = 3,
            cap     = 4
        }
        //severity of environmental impact, relative preference for food substitute
        //exceed or meet a quantitative standard
        public enum OTHERPRICE_TYPE
        {
            none        = 0,
            market      = 1,
            list        = 2,
            contracted  = 3,
            government  = 4,
            production  = 5,
            copay       = 6,
            premium     = 7,
            incentive   = 8,
            penalty     = 9,
            fee         = 10,
            engineered  = 11,
            consensus   = 12
        }
        //list of indicators 
        public List<SubPrice1> SubPrice1s = new List<SubPrice1>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfSubPrice1s = 20;

        //name
        public string SubPName { get; set; }
        //description
        public string SubPDescription { get; set; }
        //aggregation label
        public string SubPLabel { get; set; }
        //PRICE_TYPES enum
        public string SubPType { get; set; }
        //NIST 135 lump sum fempUPV index 
        public double SubPFactor { get; set; }
        //number of years to use in single present value discounting
        public double SubPYears { get; set; }
        //number of times for recurrent costs associated with SubPYears and spv discounting
        //i.e. 20 years / 5 SubPYears can have up to 4 costs to sum over the life
        public double SubPYearTimes { get; set; }
        //salvage value for annual cap recovery costs
        public double SubPSalvValue { get; set; }
        //amount
        public double SubPAmount { get; set; }
        //unit
        public string SubPUnit { get; set; }
        //price
        public double SubPPrice { get; set; }
        //escalation rate
        public double SubPEscRate { get; set; }
        //uniform, geometric, linear ...
        public string SubPEscType { get; set; }
        //total subcost (calculated)
        public double SubPTotal { get; set; }
        //total per unit subcost (unit amount passed in by parent) 
        //(calculated)
        public double SubPTotalPerUnit { get; set; }
        //generic other type or factor (i.e. copay, general benefit)
        public string SubPOtherType { get; set; }

        public const string cSubPName = "SubPName";
        private const string cSubPDescription = "SubPDescription";
        private const string cSubPLabel = "SubPLabel";
        private const string cSubPType = "SubPType";
        private const string cSubPFactor = "SubPFactor";
        private const string cSubPYears = "SubPYears";
        private const string cSubPYearTimes = "SubPYearTimes";
        private const string cSubPSalvValue = "SubPSalvValue";
        private const string cSubPAmount = "SubPAmount";
        private const string cSubPUnit = "SubPUnit";
        private const string cSubPPrice = "SubPPrice";
        private const string cSubPEscRate = "SubPEscRate";
        private const string cSubPEscType = "SubPEscType";
        private const string cSubPTotal = "SubPTotal";
        private const string cSubPTotalPerUnit = "SubPTotalPerUnit";
        private const string cSubPOtherType = "SubPOtherType";

        public virtual void InitSubPrice1sProperties()
        {
            if (this.SubPrice1s == null)
            {
                this.SubPrice1s = new List<SubPrice1>();
            }
            foreach (SubPrice1 subP in this.SubPrice1s)
            {
                InitSubPrice1Properties(subP);
            }
        }
        public void InitSubPrice1Properties(SubPrice1 subP)
        {
            subP.SubPName = string.Empty;
            subP.SubPDescription = string.Empty;
            subP.SubPLabel = string.Empty;
            subP.SubPType = PRICE_TYPE.none.ToString();
            subP.SubPFactor = 0;
            subP.SubPYears = 0;
            subP.SubPYearTimes = 0;
            subP.SubPSalvValue = 0;
            subP.SubPAmount = 0;
            subP.SubPUnit = string.Empty;
            subP.SubPPrice = 0;
            subP.SubPEscRate = 0;
            subP.SubPEscType = string.Empty;
            subP.SubPTotal = 0;
            subP.SubPTotalPerUnit = 0;
            subP.SubPFactor = 0;
            subP.SubPOtherType = OTHERPRICE_TYPE.none.ToString();
        }
        public virtual void CopySubPrice1sProperties(
            SubPrice1 calculator)
        {
            if (calculator.SubPrice1s != null)
            {
                if (this.SubPrice1s == null)
                {
                    this.SubPrice1s = new List<SubPrice1>();
                }
                foreach (SubPrice1 calculatorInd in calculator.SubPrice1s)
                {
                    SubPrice1 subP = new SubPrice1();
                    CopySubPrice1Properties(subP, calculatorInd);
                    this.SubPrice1s.Add(subP);
                }
            }
        }
        private void CopySubPrice1Properties(
            SubPrice1 subP, SubPrice1 calculator)
        {
            subP.SubPName = calculator.SubPName;
            subP.SubPDescription = calculator.SubPDescription;
            subP.SubPLabel = calculator.SubPLabel;
            subP.SubPType = calculator.SubPType;
            subP.SubPFactor = calculator.SubPFactor;
            subP.SubPYears = calculator.SubPYears;
            subP.SubPYearTimes = calculator.SubPYearTimes;
            subP.SubPSalvValue = calculator.SubPSalvValue;
            subP.SubPAmount = calculator.SubPAmount;
            subP.SubPUnit = calculator.SubPUnit;
            subP.SubPPrice = calculator.SubPPrice;
            subP.SubPEscRate = calculator.SubPEscRate;
            subP.SubPEscType = calculator.SubPEscType;
            subP.SubPTotal = calculator.SubPTotal;
            subP.SubPTotalPerUnit = calculator.SubPTotalPerUnit;
            subP.SubPOtherType = calculator.SubPOtherType;
        }
        public virtual void SetSubPrice1sProperties(XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name 
            //this.SetCalculatorProperties(calculator);
            if (this.SubPrice1s == null)
            {
                this.SubPrice1s = new List<SubPrice1>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfSubPrice1s; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cSubPName, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    SubPrice1 subP1 = new SubPrice1();
                    SetSubPrice1Properties(subP1, sAttNameExtension, calculator);
                    this.SubPrice1s.Add(subP1);
                }
                sHasAttribute = string.Empty;
            }
        }
        public void SetSubPrice1Properties(SubPrice1 subP, string attNameExtension,
            XElement calculator)
        {
            //set this object's properties
            subP.SubPName = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPName, attNameExtension));
            subP.SubPDescription = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPDescription, attNameExtension));
            subP.SubPLabel = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPLabel, attNameExtension));
            subP.SubPType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPType, attNameExtension));
            subP.SubPAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPAmount, attNameExtension));
            subP.SubPUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPUnit, attNameExtension));
            subP.SubPPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPPrice, attNameExtension));
            subP.SubPFactor = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPFactor, attNameExtension));
            subP.SubPYears = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPYears, attNameExtension));
            subP.SubPYearTimes = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPYearTimes, attNameExtension));
            subP.SubPSalvValue = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPSalvValue, attNameExtension));
            subP.SubPEscRate = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPEscRate, attNameExtension));
            subP.SubPEscType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPEscType, attNameExtension));
            subP.SubPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPTotal, attNameExtension));
            subP.SubPTotalPerUnit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPTotalPerUnit, attNameExtension));
            subP.SubPOtherType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPOtherType, attNameExtension));
        }
        public virtual void SetSubPrice1sProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.SubPrice1s == null)
            {
                this.SubPrice1s = new List<SubPrice1>();
            }
            if (this.SubPrice1s.Count < (colIndex + 1))
            {
                SubPrice1 subP1 = new SubPrice1();
                this.SubPrice1s.Insert(colIndex, subP1);
            }
            SubPrice1 subP = this.SubPrice1s.ElementAt(colIndex);
            if (subP != null)
            {
                SetSubPrice1Property(subP, attName, attValue);
            }
        }
        public void SetSubPrice1Property(SubPrice1 subP,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cSubPName:
                    subP.SubPName = attValue;
                    break;
                case cSubPDescription:
                    subP.SubPDescription = attValue;
                    break;
                case cSubPLabel:
                    subP.SubPLabel = attValue;
                    break;
                case cSubPType:
                    subP.SubPType = attValue;
                    break;
                case cSubPFactor:
                    subP.SubPFactor = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears:
                    subP.SubPYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYearTimes:
                    subP.SubPYearTimes = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPSalvValue:
                    subP.SubPSalvValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount:
                    subP.SubPAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit:
                    subP.SubPUnit = attValue;
                    break;
                case cSubPPrice:
                    subP.SubPPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate:
                    subP.SubPEscRate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType:
                    subP.SubPEscType = attValue;
                    break;
                case cSubPTotal:
                    subP.SubPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit:
                    subP.SubPTotalPerUnit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPOtherType:
                    subP.SubPOtherType = attValue;
                    break;
                default:
                    break;
            }
        }
        public virtual string GetSubPrice1sProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.SubPrice1s.Count >= (colIndex + 1))
            {
                SubPrice1 subP = this.SubPrice1s.ElementAt(colIndex);
                if (subP != null)
                {
                    sPropertyValue = GetSubPrice1Property(subP, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetSubPrice1Property(SubPrice1 subP, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cSubPName:
                    sPropertyValue = subP.SubPName;
                    break;
                case cSubPDescription:
                    sPropertyValue = subP.SubPDescription;
                    break;
                case cSubPLabel:
                    sPropertyValue = subP.SubPLabel;
                    break;
                case cSubPType:
                    sPropertyValue = subP.SubPType;
                    break;
                case cSubPFactor:
                    sPropertyValue = subP.SubPFactor.ToString();
                    break;
                case cSubPYears:
                    sPropertyValue = subP.SubPYears.ToString();
                    break;
                case cSubPYearTimes:
                    sPropertyValue = subP.SubPYearTimes.ToString();
                    break;
                case cSubPSalvValue:
                    sPropertyValue = subP.SubPSalvValue.ToString();
                    break;
                case cSubPAmount:
                    sPropertyValue = subP.SubPAmount.ToString();
                    break;
                case cSubPUnit:
                    sPropertyValue = subP.SubPUnit.ToString();
                    break;
                case cSubPPrice:
                    sPropertyValue = subP.SubPPrice.ToString();
                    break;
                case cSubPEscRate:
                    sPropertyValue = subP.SubPEscRate.ToString();
                    break;
                case cSubPEscType:
                    sPropertyValue = subP.SubPEscType;
                    break;
                case cSubPTotal:
                    sPropertyValue = subP.SubPTotal.ToString();
                    break;
                case cSubPTotalPerUnit:
                    sPropertyValue = subP.SubPTotalPerUnit.ToString();
                    break;
                case cSubPOtherType:
                    sPropertyValue = subP.SubPOtherType.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetSubPrice1sAttributes(ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
            if (this.SubPrice1s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice1 subP in this.SubPrice1s)
                {
                    sAttNameExtension = i.ToString();
                    SetSubPrice1Attributes(subP, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        public void SetSubPrice1Attributes(SubPrice1 subP, string attNameExtension,
            ref XElement calculator)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (subP.SubPName != string.Empty && subP.SubPName != Constants.NONE)
            {
                //remember that the calculator inheriting from this class must set id and name atts
                //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cSubPName, attNameExtension), subP.SubPName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cSubPDescription, attNameExtension), subP.SubPDescription);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPLabel, attNameExtension), subP.SubPLabel);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPType, attNameExtension), subP.SubPType);
                CalculatorHelpers.SetAttributeDoubleF4(calculator,
                     string.Concat(cSubPFactor, attNameExtension), subP.SubPFactor);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYears, attNameExtension), subP.SubPYears);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPYearTimes, attNameExtension), subP.SubPYearTimes);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPSalvValue, attNameExtension), subP.SubPSalvValue);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                     string.Concat(cSubPAmount, attNameExtension), subP.SubPAmount);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPUnit, attNameExtension), subP.SubPUnit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                     string.Concat(cSubPPrice, attNameExtension), subP.SubPPrice);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                     string.Concat(cSubPEscRate, attNameExtension), subP.SubPEscRate);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPEscType, attNameExtension), subP.SubPEscType);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotal, attNameExtension), subP.SubPTotal);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cSubPTotalPerUnit, attNameExtension), subP.SubPTotalPerUnit);
                CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPOtherType, attNameExtension), subP.SubPOtherType);
            }
        }
        
        public virtual void SetSubPrice1sAttributes(ref XmlWriter writer)
        {
            if (this.SubPrice1s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice1 subP in this.SubPrice1s)
                {
                    sAttNameExtension = i.ToString();
                    SetSubPrice1Attributes(subP, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public virtual void SetSubPrice1Attributes(SubPrice1 subP, string attNameExtension,
           ref XmlWriter writer)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (subP.SubPName != string.Empty && subP.SubPName != Constants.NONE)
            {
                writer.WriteAttributeString(
                    string.Concat(cSubPName, attNameExtension), subP.SubPName);
                writer.WriteAttributeString(
                    string.Concat(cSubPDescription, attNameExtension), subP.SubPDescription);
                writer.WriteAttributeString(
                     string.Concat(cSubPLabel, attNameExtension), subP.SubPLabel);
                writer.WriteAttributeString(
                     string.Concat(cSubPType, attNameExtension), subP.SubPType);
                writer.WriteAttributeString(
                     string.Concat(cSubPFactor, attNameExtension), subP.SubPFactor.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYears, attNameExtension), subP.SubPYears.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPYearTimes, attNameExtension), subP.SubPYearTimes.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                     string.Concat(cSubPSalvValue, attNameExtension), subP.SubPSalvValue.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                     string.Concat(cSubPAmount, attNameExtension), subP.SubPAmount.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                     string.Concat(cSubPUnit, attNameExtension), subP.SubPUnit);
                writer.WriteAttributeString(
                     string.Concat(cSubPPrice, attNameExtension), subP.SubPPrice.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                     string.Concat(cSubPEscRate, attNameExtension), subP.SubPEscRate.ToString());
                writer.WriteAttributeString(
                     string.Concat(cSubPEscType, attNameExtension), subP.SubPEscType);
                writer.WriteAttributeString(
                     string.Concat(cSubPTotal, attNameExtension), subP.SubPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                     string.Concat(cSubPTotalPerUnit, attNameExtension), subP.SubPTotalPerUnit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                     string.Concat(cSubPOtherType, attNameExtension), subP.SubPOtherType);
            }
        }

    }
}
