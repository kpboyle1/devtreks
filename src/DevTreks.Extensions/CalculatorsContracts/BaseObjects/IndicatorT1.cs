using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Add indicator properties that can identify the node/calculator/property to use. 
    ///Date:		2013, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        Uses _ColIC1ex to distinguish collection members in xml attributes
    /// </summary>   
    public class IndicatorT1
    {
        //constructor
        public IndicatorT1()
        {
            InitIndicatorT1Properties();
        }
        //copy constructors
        public IndicatorT1(IndicatorT1 calculator)
        {
            CopyIndicatorT1Properties(calculator);
        }
        //node type holding indicator
        public enum NODE_TYPE
        {
            none        = 0,
            input       = 1,
            output      = 2,
            operation   = 3,
            component   = 4,
            outcome     = 5,
            budget      = 6,
            investment  = 7
        }
        
        //node type of IT1ndicator
        public string IT1NodeType { get; set; }
        //calculator name holding indicator (no calcname means new property added)
        public string IT1CalcName { get; set; }
        //property name of indicator (no PropertyName means new property added)
        public string IT1PropertyName { get; set; }

        public const string cIT1NodeType = "IT1NodeType";
        public const string cIT1CalcName = "IT1CalcName";
        public const string cIT1PropertyName = "IT1PropertyName";

        public void InitIndicatorT1Properties()
        {
            this.IT1NodeType = NODE_TYPE.none.ToString();
            this.IT1CalcName = string.Empty;
            this.IT1PropertyName = string.Empty;
        }
        
        public void CopyIndicatorT1Properties(
            IndicatorT1 calculator)
        {
            this.IT1NodeType = calculator.IT1NodeType;
            this.IT1CalcName = calculator.IT1CalcName;
            this.IT1PropertyName = calculator.IT1PropertyName;
        }
        
        public void SetIndicatorT1Properties(string attNameExtension,
            XElement calculator)
        {
            //set this object's properties
            this.IT1NodeType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIT1NodeType, attNameExtension));
            this.IT1CalcName = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIT1CalcName, attNameExtension));
            this.IT1PropertyName = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIT1PropertyName, attNameExtension));
        }
        
        public void SetIndicatorT1Property(string attName, string attValue)
        {
            switch (attName)
            {
                case cIT1NodeType:
                    this.IT1NodeType = attValue;
                    break;
                case cIT1CalcName:
                    this.IT1CalcName = attValue;
                    break;
                case cIT1PropertyName:
                    this.IT1PropertyName = attValue;
                    break;
                default:
                    break;
            }
        }
        public string GetIndicatorT1Property(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cIT1NodeType:
                    sPropertyValue = this.IT1NodeType;
                    break;
                case cIT1CalcName:
                    sPropertyValue = this.IT1CalcName;
                    break;
                case cIT1PropertyName:
                    sPropertyValue = this.IT1PropertyName;
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public void SetIndicatorT1Attributes(string attNameExtension,
            ref XElement calculator)
        {
            
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cIT1NodeType, attNameExtension), this.IT1NodeType);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cIT1CalcName, attNameExtension), this.IT1CalcName);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cIT1PropertyName, attNameExtension), this.IT1PropertyName);
        }
        public virtual void SetIndicatorT1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                    string.Concat(cIT1NodeType, attNameExtension), this.IT1NodeType.ToString());
            writer.WriteAttributeString(
                    string.Concat(cIT1CalcName, attNameExtension), this.IT1CalcName.ToString());
            writer.WriteAttributeString(
                    string.Concat(cIT1PropertyName, attNameExtension), this.IT1PropertyName.ToString());
        }
    }
}
