using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Supports general capital calculations.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>      
    public class GeneralCapital1Input : Machinery1Input
    {
        //constructor
        public GeneralCapital1Input()
        {
            InitGeneralCapital1InputProperties();
        }
        //copy constructors
        public GeneralCapital1Input(GeneralCapital1Input calculator)
        {
            CopyGeneralCapital1InputProperties(calculator);
        }
        //copies the underlying input and locals props too
        public GeneralCapital1Input(CalculatorParameters calcParameters,
            GeneralCapital1Input calculator)
        {
            CopyGeneralCapital1InputProperties(calcParameters, calculator);
        }
        private const string cEnergyUseHr = "EnergyUseHr";
        private const string cEnergyEffTypical = "EnergyEffTypical";

        public double EnergyUseHr { get; set; }
        public double EnergyEffTypical { get; set; }
        public Capital1Constant Capital1Constant { get; set; }

        public virtual void InitGeneralCapital1InputProperties()
        {
            this.Capital1Constant = new Capital1Constant();
            this.EnergyUseHr = 0;
            this.EnergyEffTypical = 0;
        }
        //copy constructors
        public void CopyGeneralCapital1InputProperties(CalculatorParameters calcParameters,
            GeneralCapital1Input calculator)
        {
            //set the base input properties
            this.SetInputProperties(calcParameters, calculator);
            this.Local = new Local(calcParameters, calculator.Local);
            //set the constants properties
            this.Constants = new Machinery1Constant();
            this.Constants.SetMachinery1ConstantProperties(calculator.Constants);
            this.Sizes = new SizeRanges();
            this.Sizes.CopySizeRangesProperties(calculator.Sizes);
            this.Capital1Constant = new Capital1Constant();
            this.Capital1Constant.CopyCapital1ConstantProperties(calculator.Capital1Constant);
            CopyGeneralCapital1InputProperties(calculator);
        }
        private void CopyGeneralCapital1InputProperties(
            GeneralCapital1Input calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.Constants = calculator.Constants;
            this.Sizes = calculator.Sizes;
            this.Local = calculator.Local;
            this.Capital1Constant = calculator.Capital1Constant;
            this.EnergyUseHr = calculator.EnergyUseHr;
            this.EnergyEffTypical = calculator.EnergyEffTypical;
        }
        public virtual void SetGeneralCapital1InputProperties(CalculatorParameters calcParameters, 
            XElement calculator, XElement currentElement)
        {
            //set the base input properties
            SetMachinery1InputProperties(calcParameters,
                calculator, currentElement);
            //set the constants properties
            this.Capital1Constant = new Capital1Constant();
            this.Capital1Constant.SetCapital1ConstantProperties(calculator);
            SetGeneralCapital1InputProperties(calculator);
        }
        public virtual void SetGeneralCapital1InputProperties(XElement calculator)
        {
            //set this object's properties
            this.EnergyUseHr = CalculatorHelpers.GetAttributeDouble(calculator,
               cEnergyUseHr);
            this.EnergyEffTypical = CalculatorHelpers.GetAttributeDouble(calculator,
               cEnergyEffTypical);
        }
        public virtual void SetGeneralCapital1InputProperty(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cEnergyUseHr:
                    this.EnergyUseHr = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEnergyEffTypical:
                    this.EnergyEffTypical = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetGeneralCapital1InputProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cEnergyUseHr:
                    sPropertyValue = this.EnergyUseHr.ToString();
                    break;
                case cEnergyEffTypical:
                    sPropertyValue = this.EnergyEffTypical.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        
        public void SetGeneralCapital1InputAttributes(CalculatorParameters calcParameters,
            ref XElement calculator, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //set the base attributes
            SetMachinery1InputAttributes(calcParameters,
                ref calculator, ref currentElement,
                updates);
            //set the constants
            this.Capital1Constant.SetCapital1ConstantAttributes(ref calculator);
            //set this object
            string sAttNameExtension = string.Empty;
            SetGeneralCapital1InputAttributes(sAttNameExtension,
                ref calculator);
        }
        public void SetGeneralCapital1InputAttributes(CalculatorParameters calcParameters,
             ref XElement calculator, ref XElement currentElement)
        {
            //set the base attributes
            SetMachinery1InputAttributes(calcParameters,
                ref calculator, ref currentElement);
            //set the constants
            this.Capital1Constant.SetCapital1ConstantAttributes(ref calculator);
            //set this object
            string sAttNameExtension = string.Empty;
            SetGeneralCapital1InputAttributes(sAttNameExtension,
                ref calculator);
        }
        public virtual void SetGeneralCapital1InputAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cEnergyUseHr, attNameExtension), this.EnergyUseHr);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cEnergyEffTypical, attNameExtension), this.EnergyEffTypical);
        }
        public virtual void SetGeneralCapital1InputAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                 string.Concat(cEnergyUseHr, attNameExtension), this.EnergyUseHr.ToString());
            writer.WriteAttributeString(
                string.Concat(cEnergyEffTypical, attNameExtension), this.EnergyEffTypical.ToString());
        }
    }
}
