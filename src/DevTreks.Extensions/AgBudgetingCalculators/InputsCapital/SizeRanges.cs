using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Add machinery selection and scheduling properties to machinery calculators. 
    ///             The two critical parameters are horsepower (VarA) and field capacity (which derives 
    ///             from width, speed and field efficiency).
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>      
    public class SizeRanges
    {
        //constructor
        public SizeRanges()
        {
            InitSizeRangesProperties();
        }
        //copy constructors
        public SizeRanges(SizeRanges calculator)
        {
            CopySizeRangesProperties(calculator);
        }
        
        public double SizeRange1 { get; set; }
        public double SizePrice1 { get; set; }
        public double SizeVarA1 { get; set; }
        public double SizeVarB1 { get; set; }
        public double SizeVarC1 { get; set; }
        public double SizeVarD1 { get; set; }
        public double SizeRange2 { get; set; }
        public double SizePrice2 { get; set; }
        public double SizeVarA2 { get; set; }
        public double SizeVarB2 { get; set; }
        public double SizeVarC2 { get; set; }
        public double SizeVarD2 { get; set; }
        public double SizeRange3 { get; set; }
        public double SizePrice3 { get; set; }
        public double SizeVarA3 { get; set; }
        public double SizeVarB3 { get; set; }
        public double SizeVarC3 { get; set; }
        public double SizeVarD3 { get; set; }
        public double SizeRange4 { get; set; }
        public double SizePrice4 { get; set; }
        public double SizeVarA4 { get; set; }
        public double SizeVarB4 { get; set; }
        public double SizeVarC4 { get; set; }
        public double SizeVarD4 { get; set; }
        public double SizeRange5 { get; set; }
        public double SizePrice5 { get; set; }
        public double SizeVarA5 { get; set; }
        public double SizeVarB5 { get; set; }
        public double SizeVarC5 { get; set; }
        public double SizeVarD5 { get; set; }

        private const string cSizeRange1 = "SizeRange1";
        private const string cSizePrice1 = "SizePrice1";
        private const string cSizeVarA1 = "SizeVarA1";
        private const string cSizeVarB1 = "SizeVarB1";
        private const string cSizeVarC1 = "SizeVarC1";
        private const string cSizeVarD1 = "SizeVarD1";
        private const string cSizeRange2 = "SizeRange2";
        private const string cSizeVarA2 = "SizeVarA2";
        private const string cSizeVarB2 = "SizeVarB2";
        private const string cSizeVarC2 = "SizeVarC2";
        private const string cSizePrice2 = "SizePrice2";
        private const string cSizeVarD2 = "SizeVarD2";
        private const string cSizeRange3 = "SizeRange3";
        private const string cSizePrice3 = "SizePrice3";
        private const string cSizeVarA3 = "SizeVarA3";
        private const string cSizeVarB3 = "SizeVarB3";
        private const string cSizeVarC3 = "SizeVarC3";
        private const string cSizeVarD3 = "SizeVarD3";
        private const string cSizeRange4 = "SizeRange4";
        private const string cSizePrice4 = "SizePrice4";
        private const string cSizeVarA4 = "SizeVarA4";
        private const string cSizeVarB4 = "SizeVarB4";
        private const string cSizeVarC4 = "SizeVarC4";
        private const string cSizeVarD4 = "SizeVarD4";
        private const string cSizeRange5 = "SizeRange5";
        private const string cSizePrice5 = "SizePrice5";
        private const string cSizeVarA5 = "SizeVarA5";
        private const string cSizeVarB5 = "SizeVarB5";
        private const string cSizeVarC5 = "SizeVarC5";
        private const string cSizeVarD5 = "SizeVarD5";

        public virtual void InitSizeRangesProperties()
        {
            this.SizeRange1 = 0;
            this.SizePrice1 = 0;
            this.SizeVarA1 = 0;
            this.SizeVarB1 = 0;
            this.SizeVarC1 = 0;
            this.SizeVarD1 = 0;
            this.SizeRange2 = 0;
            this.SizePrice2 = 0;
            this.SizeVarA2 = 0;
            this.SizeVarB2 = 0;
            this.SizeVarC2 = 0;
            this.SizeVarD2 = 0;
            this.SizeRange3 = 0;
            this.SizeVarA3 = 0;
            this.SizeVarB3 = 0;
            this.SizeVarC3 = 0;
            this.SizePrice3 = 0;
            this.SizeVarD3 = 0;
            this.SizeRange4 = 0;
            this.SizePrice4 = 0;
            this.SizeVarA4 = 0;
            this.SizeVarB4 = 0;
            this.SizeVarC4 = 0;
            this.SizeVarD4 = 0;
            this.SizeRange5 = 0;
            this.SizePrice5 = 0;
            this.SizeVarA5 = 0;
            this.SizeVarB5 = 0;
            this.SizeVarC5 = 0;
            this.SizeVarD5 = 0;
        }
        public virtual void CopySizeRangesProperties(
            SizeRanges calculator)
        {
            this.SizeRange1 = calculator.SizeRange1;
            this.SizePrice1 = calculator.SizePrice1;
            this.SizeVarA1 = calculator.SizeVarA1;
            this.SizeVarB1 = calculator.SizeVarB1;
            this.SizeVarC1 = calculator.SizeVarC1;
            this.SizeVarD1 = calculator.SizeVarD1;
            this.SizeRange2 = calculator.SizeRange2;
            this.SizePrice2 = calculator.SizePrice2;
            this.SizeVarA2 = calculator.SizeVarA2;
            this.SizeVarB2 = calculator.SizeVarB2;
            this.SizeVarC2 = calculator.SizeVarC2;
            this.SizeVarD2 = calculator.SizeVarD2;
            this.SizeRange3 = calculator.SizeRange3;
            this.SizePrice3 = calculator.SizePrice3;
            this.SizeVarA3 = calculator.SizeVarA3;
            this.SizeVarB3 = calculator.SizeVarB3;
            this.SizeVarC3 = calculator.SizeVarC3;
            this.SizeVarD3 = calculator.SizeVarD3;
            this.SizeRange4 = calculator.SizeRange4;
            this.SizePrice4 = calculator.SizePrice4;
            this.SizeVarA4 = calculator.SizeVarA4;
            this.SizeVarB4 = calculator.SizeVarB4;
            this.SizeVarC4 = calculator.SizeVarC4;
            this.SizeVarD4 = calculator.SizeVarD4;
            this.SizeRange5 = calculator.SizeRange5;
            this.SizePrice5 = calculator.SizePrice5;
            this.SizeVarA5 = calculator.SizeVarA5;
            this.SizeVarB5 = calculator.SizeVarB5;
            this.SizeVarC5 = calculator.SizeVarC5;
            this.SizeVarD5 = calculator.SizeVarD5;
        }
        public virtual void SetSizeRangesProperties(XElement calculator)
        {
            //set this object's properties
            this.SizeRange1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeRange1);
            this.SizeVarA1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarA1);
            this.SizeVarB1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarB1);
            this.SizeVarC1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarC1);
            this.SizePrice1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizePrice1);
            this.SizeVarD1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarD1);
            this.SizeRange2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeRange2);
            this.SizePrice2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizePrice2);
            this.SizeVarA2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarA2);
            this.SizeVarB2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarB2);
            this.SizeVarC2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarC2);
            this.SizeVarD2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarD2);
            this.SizeRange3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeRange3);
            this.SizePrice3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizePrice3);
            this.SizeVarA3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarA3);
            this.SizeVarB3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarB3);
            this.SizeVarC3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarC3);
            this.SizeVarD3 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarD3);
            this.SizeRange4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeRange4);
            this.SizePrice4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizePrice4);
            this.SizeVarA4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarA4);
            this.SizeVarB4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarB4);
            this.SizeVarC4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarC4);
            this.SizeVarD4 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarD4);
            this.SizeRange5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeRange5);
            this.SizePrice5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizePrice5);
            this.SizeVarA5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarA5);
            this.SizeVarB5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarB5);
            this.SizeVarC5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarC5);
            this.SizeVarD5 = CalculatorHelpers.GetAttributeDouble(calculator,
               cSizeVarD5);
            
        }
        public virtual void SetSizeRangesProperty(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cSizeRange1:
                    this.SizeRange1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizePrice1:
                    this.SizePrice1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarA1:
                    this.SizeVarA1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarB1:
                    this.SizeVarB1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarC1:
                    this.SizeVarC1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarD1:
                    this.SizeVarD1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeRange2:
                    this.SizeRange2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizePrice2:
                    this.SizePrice2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarA2:
                    this.SizeVarA2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarB2:
                    this.SizeVarB2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarC2:
                    this.SizeVarC2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarD2:
                    this.SizeVarD2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeRange3:
                    this.SizeRange3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizePrice3:
                    this.SizePrice3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarA3:
                    this.SizeVarA3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarB3:
                    this.SizeVarB3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarC3:
                    this.SizeVarC3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarD3:
                    this.SizeVarD3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeRange4:
                    this.SizeRange4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizePrice4:
                    this.SizePrice4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarA4:
                    this.SizeVarA4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarB4:
                    this.SizeVarB4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarC4:
                    this.SizeVarC4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarD4:
                    this.SizeVarD4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeRange5:
                    this.SizeRange5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizePrice5:
                    this.SizePrice5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarA5:
                    this.SizeVarA5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarB5:
                    this.SizeVarB5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarC5:
                    this.SizeVarC5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSizeVarD5:
                    this.SizeVarD5 = CalculatorHelpers.ConvertStringToDouble(attValue);
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
                case cSizeRange1:
                    sPropertyValue = this.SizeRange1.ToString();
                    break;
                case cSizePrice1:
                    sPropertyValue = this.SizePrice1.ToString();
                    break;
                case cSizeVarA1:
                    sPropertyValue = this.SizeVarA1.ToString();
                    break;
                case cSizeVarB1:
                    sPropertyValue = this.SizeVarB1.ToString();
                    break;
                case cSizeVarC1:
                    sPropertyValue = this.SizeVarC1.ToString();
                    break;
                case cSizeVarD1:
                    sPropertyValue = this.SizeVarD1.ToString();
                    break;
                case cSizeRange2:
                    sPropertyValue = this.SizeRange2.ToString();
                    break;
                case cSizePrice2:
                    sPropertyValue = this.SizePrice2.ToString();
                    break;
                case cSizeVarA2:
                    sPropertyValue = this.SizeVarA2.ToString();
                    break;
                case cSizeVarB2:
                    sPropertyValue = this.SizeVarB2.ToString();
                    break;
                case cSizeVarC2:
                    sPropertyValue = this.SizeVarC2.ToString();
                    break;
                case cSizeVarD2:
                    sPropertyValue = this.SizeVarD2.ToString();
                    break;
                case cSizeRange3:
                    sPropertyValue = this.SizeRange3.ToString();
                    break;
                case cSizePrice3:
                    sPropertyValue = this.SizePrice3.ToString();
                    break;
                case cSizeVarA3:
                    sPropertyValue = this.SizeVarA3.ToString();
                    break;
                case cSizeVarB3:
                    sPropertyValue = this.SizeVarB3.ToString();
                    break;
                case cSizeVarC3:
                    sPropertyValue = this.SizeVarC3.ToString();
                    break;
                case cSizeVarD3:
                    sPropertyValue = this.SizeVarD3.ToString();
                    break;
                case cSizeRange4:
                    sPropertyValue = this.SizeRange4.ToString();
                    break;
                case cSizePrice4:
                    sPropertyValue = this.SizePrice4.ToString();
                    break;
                case cSizeVarA4:
                    sPropertyValue = this.SizeVarA4.ToString();
                    break;
                case cSizeVarB4:
                    sPropertyValue = this.SizeVarB4.ToString();
                    break;
                case cSizeVarC4:
                    sPropertyValue = this.SizeVarC4.ToString();
                    break;
                case cSizeVarD4:
                    sPropertyValue = this.SizeVarD4.ToString();
                    break;
                case cSizeRange5:
                    sPropertyValue = this.SizeRange5.ToString();
                    break;
                case cSizePrice5:
                    sPropertyValue = this.SizePrice5.ToString();
                    break;
                case cSizeVarA5:
                    sPropertyValue = this.SizeVarA5.ToString();
                    break;
                case cSizeVarB5:
                    sPropertyValue = this.SizeVarB5.ToString();
                    break;
                case cSizeVarC5:
                    sPropertyValue = this.SizeVarC5.ToString();
                    break;
                case cSizeVarD5:
                    sPropertyValue = this.SizeVarD5.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetSizeRangesAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.SizePrice1 == 0)
            {
                return;
            }
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeRange1, attNameExtension), this.SizeRange1);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizePrice1, attNameExtension), this.SizePrice1);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarA1, attNameExtension), this.SizeVarA1);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarB1, attNameExtension), this.SizeVarB1);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarC1, attNameExtension), this.SizeVarC1);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarD1, attNameExtension), this.SizeVarD1);
            //don't needlessly add these to linkedviews if they are not being used
            if (this.SizePrice2 == 0)
            {
                return;
            }
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeRange2, attNameExtension), this.SizeRange2);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizePrice2, attNameExtension), this.SizePrice2);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarA2, attNameExtension), this.SizeVarA2);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarB2, attNameExtension), this.SizeVarB2);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarC2, attNameExtension), this.SizeVarC2);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarD2, attNameExtension), this.SizeVarD2);
            if (this.SizePrice3 == 0)
            {
                return;
            }
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeRange3, attNameExtension), this.SizeRange3);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizePrice3, attNameExtension), this.SizePrice3);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarA3, attNameExtension), this.SizeVarA3);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarB3, attNameExtension), this.SizeVarB3);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarC3, attNameExtension), this.SizeVarC3);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarD3, attNameExtension), this.SizeVarD3);
            if (this.SizePrice4 == 0)
            {
                return;
            }
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeRange4, attNameExtension), this.SizeRange4);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizePrice4, attNameExtension), this.SizePrice4);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarA4, attNameExtension), this.SizeVarA4);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarB4, attNameExtension), this.SizeVarB4);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarC4, attNameExtension), this.SizeVarC4);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarD4, attNameExtension), this.SizeVarD4);
            if (this.SizePrice5 == 0)
            {
                return;
            }
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeRange5, attNameExtension), this.SizeRange5);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizePrice5, attNameExtension), this.SizePrice5);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarA5, attNameExtension), this.SizeVarA5);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarB5, attNameExtension), this.SizeVarB5);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarC5, attNameExtension), this.SizeVarC5);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSizeVarD5, attNameExtension), this.SizeVarD5);
        }
        public virtual void SetSizeRangesAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if (this.SizePrice1 == 0)
            {
                return;
            }
            writer.WriteAttributeString(
                 string.Concat(cSizeRange1, attNameExtension), this.SizeRange1.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizePrice1, attNameExtension), this.SizePrice1.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarA1, attNameExtension), this.SizeVarA1.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarB1, attNameExtension), this.SizeVarB1.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarC1, attNameExtension), this.SizeVarC1.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarD1, attNameExtension), this.SizeVarD1.ToString());
            if (this.SizePrice2 == 0)
            {
                return;
            }
            writer.WriteAttributeString(
                 string.Concat(cSizeRange2, attNameExtension), this.SizeRange2.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizePrice2, attNameExtension), this.SizePrice2.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarA2, attNameExtension), this.SizeVarA2.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarB2, attNameExtension), this.SizeVarB2.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarC2, attNameExtension), this.SizeVarC2.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarD2, attNameExtension), this.SizeVarD2.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeRange3, attNameExtension), this.SizeRange3.ToString());
            if (this.SizePrice3 == 0)
            {
                return;
            }
            writer.WriteAttributeString(
                 string.Concat(cSizePrice3, attNameExtension), this.SizePrice3.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarA3, attNameExtension), this.SizeVarA3.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarB3, attNameExtension), this.SizeVarB3.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarC3, attNameExtension), this.SizeVarC3.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarD3, attNameExtension), this.SizeVarD3.ToString());
            if (this.SizePrice4 == 0)
            {
                return;
            }
            writer.WriteAttributeString(
                 string.Concat(cSizeRange4, attNameExtension), this.SizeRange4.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizePrice4, attNameExtension), this.SizePrice4.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarA4, attNameExtension), this.SizeVarA4.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarB4, attNameExtension), this.SizeVarB4.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarC4, attNameExtension), this.SizeVarC4.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarD4, attNameExtension), this.SizeVarD4.ToString());
            if (this.SizePrice5 == 0)
            {
                return;
            }
            writer.WriteAttributeString(
                 string.Concat(cSizeRange5, attNameExtension), this.SizeRange5.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizePrice5, attNameExtension), this.SizePrice5.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarA5, attNameExtension), this.SizeVarA5.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarB5, attNameExtension), this.SizeVarB5.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarC5, attNameExtension), this.SizeVarC5.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSizeVarD5, attNameExtension), this.SizeVarD5.ToString());
        }
    }
}
