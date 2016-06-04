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
    ///Purpose:		Add monitoring and evaluation indicators to DevTreks input, 
    ///             output, operation, outcome, and budget elements. Can be run 
    ///             stand alone, with normal stock totals, or as an analyzer step
    ///             that keeps track of each meal Qs, or each household member Qs
    ///Date:		2015, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        Uses _ColIndex to distinguish collection members in xml attributes
    ///             Ratio: (Amount1 * VarAmount) / (Amount2 * VarAmount2)
    /// </summary>   
    public class ME2Indicator2 : SB1Base
    {
        //constructor
        public ME2Indicator2()
        {
            InitME2Indicator2sProperties();
        }
        //copy constructors
        public ME2Indicator2(ME2Indicator2 calculator)
        {
            CopyME2Indicator2sProperties(calculator);
        }

        //list of indicators 
        public List<ME2Indicator2> ME2Indicator2s = new List<ME2Indicator2>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfME2Indicator2s = 20;
        //name of the overall indicator
        public string IndName { get; set; }
        //description
        public string IndDescription { get; set; }
        //aggregation label
        public string IndLabel { get; set; }
        //INDIC_TYPES enum
        public string IndType { get; set; }
        //weight (general multiplier) for weighted milestone EVM
        public double IndWeight { get; set; }
        //mathematical operation for the two qs
        public string IndMathType { get; set; }
        //date of indicator measurement
        public DateTime IndDate { get; set; }
        //first quantitative prop
        //amount
        public double Ind1Amount { get; set; }
        //unit: can be expressed as a ratio %targpop per 1000 residents
        //can be a simple cost 1000 pounds at $1 per pound = $1000 Total
        public string Ind1Unit { get; set; }
        //second quantity
        public double Ind2Amount { get; set; }
        //second unit
        public string Ind2Unit { get; set; }
        //total of the two indicators (p*q = cost)
        public double IndTotal { get; set; }
        //unit for total (i.e. hours physical activity, cost, benefit, number (food groups)
        public string IndUnit { get; set; }

        public const string cIndName = "IndName";
        public const string cIndDescription = "IndDescription";
        public const string cIndLabel = "IndLabel";
        public const string cIndType = "IndType";
        public const string cIndMathType = "IndMathType";
        public const string cIndDate = "IndDate";
        public const string cInd1Amount = "Ind1Amount";
        public const string cInd1Unit = "Ind1Unit";
        public const string cInd2Amount = "Ind2Amount";
        public const string cInd2Unit = "Ind2Unit";
        public const string cIndTotal = "IndTotal";
        public const string cIndUnit = "IndUnit";
        public const string cIndWeight = "IndWeight";

        public virtual void InitME2Indicator2sProperties()
        {
            if (this.ME2Indicator2s == null)
            {
                this.ME2Indicator2s = new List<ME2Indicator2>();
            }
            foreach (ME2Indicator2 ind in this.ME2Indicator2s)
            {
                InitME2Indicator2Properties(ind);
            }
        }
        private void InitME2Indicator2Properties(ME2Indicator2 ind)
        {
            ind.InitSB1BaseProperties();
            ind.IndDescription = string.Empty;
            ind.IndName = string.Empty;
            ind.IndLabel = string.Empty;
            ind.IndType = INDIC_TYPES.none.ToString();
            ind.IndMathType = QMATH_TYPE.none.ToString();
            ind.IndDate = CalculatorHelpers.GetDateShortNow();
            ind.Ind1Amount = 0;
            ind.Ind1Unit = string.Empty;
            ind.Ind2Amount = 0;
            ind.Ind2Unit = string.Empty;
            ind.IndTotal = 0;
            ind.IndUnit = string.Empty;
            ind.IndMathType = string.Empty;
            ind.IndWeight = 1.0;
        }
        public virtual void CopyME2Indicator2sProperties(
            ME2Indicator2 calculator)
        {
            if (calculator.ME2Indicator2s != null)
            {
                if (this.ME2Indicator2s == null)
                {
                    this.ME2Indicator2s = new List<ME2Indicator2>();
                }
                foreach (ME2Indicator2 calculatorInd in calculator.ME2Indicator2s)
                {
                    ME2Indicator2 ind = new ME2Indicator2();
                    CopyME2Indicator2Properties(ind, calculatorInd);
                    this.ME2Indicator2s.Add(ind);
                }
                this.Observations = this.ME2Indicator2s.Count;
            }
        }
        private void CopyME2Indicator2Properties(
            ME2Indicator2 ind, ME2Indicator2 calculator)
        {
            ind.CopySB1BaseProperties(calculator);
            ind.IndDescription = calculator.IndDescription;
            ind.IndName = calculator.IndName;
            ind.IndLabel = calculator.IndLabel;
            ind.IndType = calculator.IndType;
            ind.IndMathType = calculator.IndMathType;
            ind.IndDate = calculator.IndDate;
            ind.Ind1Amount = calculator.Ind1Amount;
            ind.Ind1Unit = calculator.Ind1Unit;
            ind.Ind2Amount = calculator.Ind2Amount;
            ind.Ind2Unit = calculator.Ind2Unit;
            ind.IndTotal = calculator.IndTotal;
            ind.IndUnit = calculator.IndUnit;
            ind.IndWeight = calculator.IndWeight;
        }
        public virtual void SetME2Indicator2sProperties(XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name 
            //this.SetCalculatorProperties(calculator);
            if (this.ME2Indicator2s == null)
            {
                this.ME2Indicator2s = new List<ME2Indicator2>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfME2Indicator2s; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cIndName, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    ME2Indicator2 ind1 = new ME2Indicator2();
                    SetME2Indicator2Properties(ind1, sAttNameExtension, calculator);
                    this.ME2Indicator2s.Add(ind1);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetME2Indicator2Properties(ME2Indicator2 ind, string attNameExtension,
            XElement calculator)
        {
            //set this object's properties
            ind.IndDescription = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndDescription, attNameExtension));
            ind.IndName = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndName, attNameExtension));
            ind.IndLabel = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndLabel, attNameExtension));
            ind.IndType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndType, attNameExtension));
            ind.Ind1Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cInd1Amount, attNameExtension));
            ind.Ind1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cInd1Unit, attNameExtension));
            ind.IndMathType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndMathType, attNameExtension));
            ind.IndDate = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cIndDate, attNameExtension));
            ind.Ind2Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cInd2Amount, attNameExtension));
            ind.Ind2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cInd2Unit, attNameExtension));
            ind.IndTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTotal, attNameExtension));
            ind.IndUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndUnit, attNameExtension));
            ind.IndWeight = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndWeight, attNameExtension));
        }
        public virtual void SetME2Indicator2sProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.ME2Indicator2s == null)
            {
                this.ME2Indicator2s = new List<ME2Indicator2>();
            }
            if (this.ME2Indicator2s.Count < (colIndex + 1))
            {
                ME2Indicator2 ind1 = new ME2Indicator2();
                this.ME2Indicator2s.Insert(colIndex, ind1);
            }
            ME2Indicator2 ind = this.ME2Indicator2s.ElementAt(colIndex);
            if (ind != null)
            {
                SetME2Indicator2Property(ind, attName, attValue);
            }
        }
        private void SetME2Indicator2Property(ME2Indicator2 ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cIndDescription:
                    ind.IndDescription = attValue;
                    break;
                case cIndName:
                    ind.IndName = attValue;
                    break;
                case cIndLabel:
                    ind.IndLabel = attValue;
                    break;
                case cIndType:
                    ind.IndType = attValue;
                    break;
                case cIndMathType:
                    ind.IndMathType = attValue;
                    break;
                case cIndDate:
                    ind.IndDate = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cInd1Amount:
                    ind.Ind1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd1Unit:
                    ind.Ind1Unit = attValue;
                    break;
                case cInd2Amount:
                    ind.Ind2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd2Unit:
                    ind.Ind2Unit = attValue;
                    break;
                case cIndTotal:
                    ind.IndTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndUnit:
                    ind.IndUnit = attValue;
                    break;
                case cIndWeight:
                    ind.IndWeight = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetME2Indicator2sProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.ME2Indicator2s.Count >= (colIndex + 1))
            {
                ME2Indicator2 ind = this.ME2Indicator2s.ElementAt(colIndex);
                if (ind != null)
                {
                    sPropertyValue = GetME2Indicator2Property(ind, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetME2Indicator2Property(ME2Indicator2 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cIndDescription:
                    sPropertyValue = ind.IndDescription;
                    break;
                case cIndName:
                    sPropertyValue = ind.IndName;
                    break;
                case cIndLabel:
                    sPropertyValue = ind.IndLabel;
                    break;
                case cIndType:
                    sPropertyValue = ind.IndType;
                    break;
                case cIndMathType:
                    sPropertyValue = ind.IndMathType.ToString();
                    break;
                case cIndDate:
                    sPropertyValue = ind.IndDate.ToString();
                    break;
                case cInd1Amount:
                    sPropertyValue = ind.Ind1Amount.ToString();
                    break;
                case cInd1Unit:
                    sPropertyValue = ind.Ind1Unit.ToString();
                    break;
                case cInd2Amount:
                    sPropertyValue = ind.Ind2Amount.ToString();
                    break;
                case cInd2Unit:
                    sPropertyValue = ind.Ind2Unit;
                    break;
                case cIndTotal:
                    sPropertyValue = ind.IndTotal.ToString();
                    break;
                case cIndUnit:
                    sPropertyValue = ind.IndUnit.ToString();
                    break;
                case cIndWeight:
                    sPropertyValue = ind.IndWeight.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetME2Indicator2sAttributes(ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
            if (this.ME2Indicator2s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (ME2Indicator2 ind in this.ME2Indicator2s)
                {
                    sAttNameExtension = i.ToString();
                    SetME2Indicator2Attributes(ind, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetME2Indicator2Attributes(ME2Indicator2 ind, string attNameExtension,
            ref XElement calculator)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (ind.IndName != string.Empty && ind.IndName != Constants.NONE)
            {
                //remember that the calculator inheriting from this class must set id and name atts
                //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cIndName, attNameExtension), ind.IndName);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cIndLabel, attNameExtension), ind.IndLabel);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cIndDescription, attNameExtension), ind.IndDescription);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cIndDate, attNameExtension), ind.IndDate);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cIndType, attNameExtension), ind.IndType);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cIndMathType, attNameExtension), ind.IndMathType);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                    string.Concat(cIndTotal, attNameExtension), ind.IndTotal);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cIndUnit, attNameExtension), ind.IndUnit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cIndWeight, attNameExtension), ind.IndWeight);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cInd1Amount, attNameExtension), ind.Ind1Amount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cInd1Unit, attNameExtension), ind.Ind1Unit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cInd2Amount, attNameExtension), ind.Ind2Amount);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cInd2Unit, attNameExtension), ind.Ind2Unit);
            }
        }
        public virtual void SetME2Indicator2sAttributes(ref XmlWriter writer)
        {
            if (this.ME2Indicator2s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (ME2Indicator2 ind in this.ME2Indicator2s)
                {
                    sAttNameExtension = i.ToString();
                    SetME2Indicator2Attributes(ind, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public virtual void SetME2Indicator2Attributes(ME2Indicator2 ind, string attNameExtension,
           ref XmlWriter writer)
        {
            if (ind.IndName != string.Empty && ind.IndName != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cIndName, attNameExtension), ind.IndName);
                writer.WriteAttributeString(
                    string.Concat(cIndDescription, attNameExtension), ind.IndDescription);
                writer.WriteAttributeString(
                        string.Concat(cIndLabel, attNameExtension), ind.IndLabel);
                writer.WriteAttributeString(
                        string.Concat(cIndType, attNameExtension), ind.IndType);
                writer.WriteAttributeString(
                        string.Concat(cIndMathType, attNameExtension), ind.IndMathType.ToString());
                writer.WriteAttributeString(
                        string.Concat(cIndDate, attNameExtension), ind.IndDate.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cInd1Amount, attNameExtension), ind.Ind1Amount.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cInd1Unit, attNameExtension), ind.Ind1Unit.ToString());
                writer.WriteAttributeString(
                        string.Concat(cInd2Amount, attNameExtension), ind.Ind2Amount.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cInd2Unit, attNameExtension), ind.Ind2Unit);
                writer.WriteAttributeString(
                    string.Concat(cIndTotal, attNameExtension), ind.IndTotal.ToString("N3", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cIndUnit, attNameExtension), ind.IndUnit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cIndWeight, attNameExtension), ind.IndWeight.ToString());
            }
        }

        //run the calculations
        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            bHasCalculations = SetCalculations();
            return bHasCalculations;
        }
        private bool SetCalculations()
        {
            bool bHasCalcs = false;
            foreach (ME2Indicator2 ind in this.ME2Indicator2s)
            {
                ind.IndTotal = GetTotal(ind.IndWeight, ind.IndMathType,
                    ind.Ind1Amount, ind.Ind2Amount, ind.IndTotal);
                if (!bHasCalcs)
                    bHasCalcs = true;
            }
            return bHasCalcs;
        }
        public static double GetTotal(double IndWeight, string mathType,
            double Ind1Amount, double Ind2Amount, double IndTotal)
        {
            double dbTotal = 0;
            if (mathType == QMATH_TYPE.Q1_divide_Q2.ToString())
            {
                if (Ind2Amount == 0)
                {
                    return 0;
                }
                dbTotal = Ind1Amount / Ind2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_multiply_Q2.ToString())
            {
                dbTotal = Ind1Amount * Ind2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_add_Q2.ToString())
            {
                dbTotal = Ind1Amount + Ind2Amount;
            }
            else if (mathType == QMATH_TYPE.Q1_subtract_Q2.ToString())
            {
                dbTotal = Ind1Amount - Ind2Amount;
            }
            else
            {
                //194: default is data entry
                dbTotal = IndTotal;
            }
            //May, 2013: weighted totals support weighted milestones
            dbTotal = dbTotal * IndWeight;
            return dbTotal;
        }

    }
}
