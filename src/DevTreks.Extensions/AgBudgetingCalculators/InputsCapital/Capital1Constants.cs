using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Constants used by capital inputs
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>    
    public class Capital1Constant
    {
        //constructor
        public Capital1Constant()
        {
            InitCapital1ConstantProperties();
        }
        //copy constructors
        public Capital1Constant(Capital1Constant calculator)
        {
            CopyCapital1ConstantProperties(calculator);
        }
        public double RandMPercent { get; set; }
        //Replacement costs (lcc requires this cost)
        public double ReplacementCost { get; set; }
        //number of years from base year 
        public double ReplacementYrsFromBaseDate { get; set; }
        public double TotalReplacementCost { get; set; }
        public const string cRandMPercent = "RandMPercent";
        public const string cReplacementCost = "ReplacementCost";
        public const string cTotalReplacementCost = "TReplacementCost";
        //Annual recurring years from end of preproduction period (or, in NIST 135, Planning/Construction period)
        public const string cReplacementYrsFromBaseDate = "ReplacementYrsFromBaseDate";

        public virtual void InitCapital1ConstantProperties()
        {
            this.RandMPercent = 0;
            this.ReplacementCost = 0;
            this.TotalReplacementCost = 0;
            this.ReplacementYrsFromBaseDate = 0;
        }
        public virtual void CopyCapital1ConstantProperties(
            Capital1Constant calculator)
        {
            this.RandMPercent = calculator.RandMPercent;
            this.ReplacementCost = calculator.ReplacementCost;
            this.TotalReplacementCost = calculator.TotalReplacementCost;
            this.ReplacementYrsFromBaseDate = calculator.ReplacementYrsFromBaseDate;
        }
        public void SetCapital1ConstantProperties(XElement currentCalculationsElement)
        {
            this.RandMPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cRandMPercent);
            this.ReplacementCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
              cReplacementCost);
            this.TotalReplacementCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
              cTotalReplacementCost);
            this.ReplacementYrsFromBaseDate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cReplacementYrsFromBaseDate);
        }
        public virtual void SetCapital1ConstantProperty(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cRandMPercent:
                    this.RandMPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cReplacementCost:
                    this.ReplacementCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalReplacementCost:
                    this.TotalReplacementCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cReplacementYrsFromBaseDate:
                    this.ReplacementYrsFromBaseDate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetSizeRangesProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cRandMPercent:
                    sPropertyValue = this.RandMPercent.ToString();
                    break;
                case cReplacementCost:
                    sPropertyValue = this.ReplacementCost.ToString();
                    break;
                case cTotalReplacementCost:
                    sPropertyValue = this.TotalReplacementCost.ToString();
                    break;
                case cReplacementYrsFromBaseDate:
                    sPropertyValue = this.ReplacementYrsFromBaseDate.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public void SetCapital1ConstantAttributes(
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cRandMPercent, this.RandMPercent);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cReplacementCost, this.ReplacementCost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cTotalReplacementCost, this.TotalReplacementCost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cReplacementYrsFromBaseDate, this.ReplacementYrsFromBaseDate);
        }
        public virtual void SetCapital1ConstantAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                 string.Concat(cRandMPercent, attNameExtension), this.RandMPercent.ToString());
            writer.WriteAttributeString(
                 string.Concat(cReplacementCost, attNameExtension), this.ReplacementCost.ToString());
            writer.WriteAttributeString(
                 string.Concat(cTotalReplacementCost, attNameExtension), this.TotalReplacementCost.ToString());
            writer.WriteAttributeString(
                 string.Concat(cReplacementYrsFromBaseDate, attNameExtension), this.ReplacementYrsFromBaseDate.ToString());
        }
    }
}
