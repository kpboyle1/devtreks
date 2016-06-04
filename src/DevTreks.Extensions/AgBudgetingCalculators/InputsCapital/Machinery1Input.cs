using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The Machinery1Input class is a base class used 
    ///             by most machinery calculators/analyzers to hold 
    ///             resource stock totals (i.e. fuel consumed by a tractor/s). 
    ///             The virtual methods are meant to be overridden because 
    ///             some analyses, due to file size and performance issues, 
    ///             need to limit the properties used in an object and subsequently 
    ///             deserialized to an xelement's attributes.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Derived classes usually extend this class with additional 
    ///             resource use properties, such as totals and statistical properties.
    ///             2. This class supports standard agricultural budgeting conventions 
    ///             such as displaying all of the machinery and machinery facts used 
    ///             in an operation/component, time period, budget, or budget group.
    ///             3. Data definition documents will become available during 
    ///             late-stage testing.
    /// </summary>         
    public class Machinery1Input : Input
    {
        //constructor
        public Machinery1Input()
        {
            InitMachinery1InputProperties();
        }
        //copy constructors
        public Machinery1Input(Machinery1Input calculator)
        {
            CopyMachinery1InputProperties(calculator);
        }
        //copies the underlying input and locals props too
        public Machinery1Input(CalculatorParameters calcParameters,
            Machinery1Input calculator)
        {
            CopyMachinery1InputProperties(calcParameters, calculator);
        }
        private const string cMarketValue = "MarketValue";
        private const string cListPriceAdj = "ListPriceAdj";
        public const string cSalvageValue = "SalvageValue";
        public const string cFuelAmount = "FuelAmount";
        private const string cFuelUnit = "FuelUnit";
        public const string cFuelPrice = "FuelPrice";
        public const string cFuelCost = "FuelCost";
        public const string cLubeOilAmount = "LubeOilAmount";
        public const string cLubeOilPrice = "LubeOilPrice";
        private const string cLubeOilUnit = "LubeOilUnit";
        public const string cLubeOilCost = "LubeOilCost";
        public const string cRepairCost = "RepairCost";
        private const string cLaborAmount = "LaborAmount";
        private const string cLaborUnit = "LaborUnit";
        public const string cLaborPrice = "LaborPrice";
        public const string cLaborCost = "LaborCost";
        private const string cServiceUnits = "ServiceUnits";
        private const string cServiceCapacityUnits = "ServiceCapacityUnits";
        private const string cCapitalRecoveryCost = "CapitalRecoveryCost";
        private const string cTaxesHousingInsuranceCost = "TaxesHousingInsuranceCost";
        private const string cOptionForCapacity = "OptionForCapacity";
        public const string cOptionForInflation = "OptionForInflation";
        public const string cOptionForTime = "OptionForTime";
        private const string cOptionForFuel = "OptionForFuel";
        private const string cLaborAmountAdj = "LaborAmountAdj";
        
        public double MarketValue { get; set; }
        public double ListPriceAdj { get; set; }
        public double SalvageValue { get; set; }
        public double FuelAmount { get; set; }
        public string FuelUnit { get; set; }
        public double FuelPrice { get; set; }
        public double FuelCost { get; set; }
        public double LubeOilAmount { get; set; }
        public string LubeOilUnit { get; set; }
        public double LubeOilPrice { get; set; }
        public double LubeOilCost { get; set; }
        public double RepairCost { get; set; }
        public double LaborAmount { get; set; }
        public string LaborUnit { get; set; }
        public double LaborPrice { get; set; }
        public double LaborCost { get; set; }
        public string ServiceUnits { get; set; }
        public string ServiceCapacityUnits { get; set; }
        public double CapitalRecoveryCost { get; set; }
        public double TaxesHousingInsuranceCost { get; set; }
        public int OptionForCapacity { get; set; }
        public int OptionForInflation { get; set; }
        public int OptionForTime { get; set; }
        public int OptionForFuel { get; set; }
        public double LaborAmountAdj { get; set; }
        
        public Machinery1Constant Constants { get; set; }
        public SizeRanges Sizes { get; set; }

        public virtual void InitMachinery1InputProperties()
        {
            this.Constants = new Machinery1Constant();
            this.Sizes = new SizeRanges();
            this.Local = new Local();
            this.MarketValue = 0;
            this.ListPriceAdj = 0;
            this.SalvageValue = 0;
            this.FuelAmount = 0;
            this.FuelUnit = string.Empty;
            this.FuelPrice = 0;
            this.FuelCost = 0;
            this.LubeOilAmount = 0;
            this.LubeOilUnit = string.Empty;
            this.LubeOilPrice = 0;
            this.LubeOilCost = 0;
            this.RepairCost = 0;
            this.LaborAmount = 0;
            this.LaborUnit = string.Empty;
            this.LaborPrice = 0;
            this.LaborCost = 0;

            this.ServiceUnits = string.Empty;
            this.ServiceCapacityUnits = string.Empty;
            this.CapitalRecoveryCost = 0;
            this.TaxesHousingInsuranceCost = 0;
            this.OptionForCapacity = 0;
            this.OptionForInflation = 0;
            this.OptionForTime = 0;
            this.OptionForFuel = 0;
            this.LaborAmountAdj = 0;
        }
        //copy constructors
        public void CopyMachinery1InputProperties(CalculatorParameters calcParameters,
            Machinery1Input calculator)
        {
            //set the base input properties
            this.SetInputProperties(calcParameters, calculator);
            this.Local = new Local(calcParameters, calculator.Local);
            //set the constants properties
            this.Constants = new Machinery1Constant();
            this.Constants.SetMachinery1ConstantProperties(calculator.Constants);
            this.Sizes = new SizeRanges();
            this.Sizes.CopySizeRangesProperties(calculator.Sizes);
            CopyMachinery1InputProperties(calculator);
        }
        private void CopyMachinery1InputProperties(Machinery1Input calculator)
        {
            this.MarketValue = calculator.MarketValue;
            this.ListPriceAdj = calculator.ListPriceAdj;
            this.SalvageValue = calculator.SalvageValue;
            this.FuelAmount = calculator.FuelAmount;
            this.FuelUnit = calculator.FuelUnit;
            this.FuelPrice = calculator.FuelPrice;
            this.FuelCost = calculator.FuelCost;
            this.LubeOilAmount = calculator.LubeOilAmount;
            this.LubeOilUnit = calculator.LubeOilUnit;
            this.LubeOilPrice = calculator.LubeOilPrice;
            this.LubeOilCost = calculator.LubeOilCost;
            this.RepairCost = calculator.RepairCost;
            this.LaborAmount = calculator.LaborAmount;
            this.LaborUnit = calculator.LaborUnit;
            this.LaborPrice = calculator.LaborPrice;
            this.LaborCost = calculator.LaborCost;

            this.ServiceUnits = calculator.ServiceUnits;
            this.ServiceCapacityUnits = calculator.ServiceCapacityUnits;
            this.CapitalRecoveryCost = calculator.CapitalRecoveryCost;
            this.TaxesHousingInsuranceCost = calculator.TaxesHousingInsuranceCost;
            this.OptionForCapacity = calculator.OptionForCapacity;
            this.OptionForInflation = calculator.OptionForInflation;
            this.OptionForTime = calculator.OptionForTime;
            this.OptionForFuel = calculator.OptionForFuel;
            this.LaborAmountAdj = calculator.LaborAmountAdj;
        }
        public virtual void SetMachinery1InputProperties(CalculatorParameters calcParameters, 
            XElement calculator, XElement currentElement)
        {
            //set the base input properties (note, although calculator also 
            //uses the same input attributes within the calculator, they are used to 
            //facilitate display only; the currentElement input attributes are 
            //the numbers used in calculations (unless the calculator uses aliases to change them))
            this.SetInputProperties(calcParameters, calculator,
                currentElement);
            //v145a separated base els from calcs
            this.SetCalculatorProperties(calculator);
            //set the constants properties
            this.Constants = new Machinery1Constant();
            this.Constants.SetMachinery1ConstantProperties(calcParameters, 
                calculator, currentElement);
            this.Sizes = new SizeRanges();
            this.Sizes.SetSizeRangesProperties(calculator);
            SetMachinery1InputProperties(calculator);
        }
        public virtual void SetMachinery1InputProperties(XElement calculator)
        {
            //set this object's properties
            this.MarketValue = CalculatorHelpers.GetAttributeDouble(calculator,
               cMarketValue);
            this.ListPriceAdj = CalculatorHelpers.GetAttributeDouble(calculator,
               cListPriceAdj);
            this.SalvageValue = CalculatorHelpers.GetAttributeDouble(calculator,
               cSalvageValue);
            this.FuelAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               cFuelAmount);
            this.FuelUnit = CalculatorHelpers.GetAttribute(calculator,
               cFuelUnit);
            this.FuelPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               cFuelPrice);
            this.FuelCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cFuelCost);
            this.LubeOilAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               cLubeOilAmount);
            this.LubeOilUnit = CalculatorHelpers.GetAttribute(calculator,
               cLubeOilUnit);
            this.LubeOilPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               cLubeOilPrice);
            this.LubeOilCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cLubeOilCost);
            this.RepairCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cRepairCost);
            this.LaborAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               cLaborAmount);
            this.LaborUnit = CalculatorHelpers.GetAttribute(calculator,
               cLaborUnit);
            this.LaborPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               cLaborPrice);
            this.LaborCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cLaborCost);

            this.ServiceUnits = CalculatorHelpers.GetAttribute(calculator,
               cServiceUnits);
            this.ServiceCapacityUnits = CalculatorHelpers.GetAttribute(calculator,
               cServiceCapacityUnits);
            this.CapitalRecoveryCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cCapitalRecoveryCost);
            this.TaxesHousingInsuranceCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cTaxesHousingInsuranceCost);
            this.OptionForCapacity = CalculatorHelpers.GetAttributeInt(calculator,
               cOptionForCapacity);
            this.OptionForInflation = CalculatorHelpers.GetAttributeInt(calculator,
               cOptionForInflation);
            this.OptionForTime = CalculatorHelpers.GetAttributeInt(calculator,
               cOptionForTime);
            this.OptionForFuel = CalculatorHelpers.GetAttributeInt(calculator,
               cOptionForFuel);
            this.LaborAmountAdj = CalculatorHelpers.GetAttributeDouble(calculator,
               cLaborAmountAdj);
        }
        public virtual void SetMachinery1InputProperty(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cMarketValue:
                    this.MarketValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cListPriceAdj:
                    this.ListPriceAdj = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSalvageValue:
                    this.SalvageValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFuelAmount:
                    this.FuelAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFuelUnit:
                    this.FuelUnit = attValue;
                    break;
                case cFuelPrice:
                    this.FuelPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFuelCost:
                    this.FuelCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLubeOilAmount:
                    this.LubeOilAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLubeOilUnit:
                    this.LubeOilUnit = attValue;
                    break;
                case cLubeOilPrice:
                    this.LubeOilPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLubeOilCost:
                    this.LubeOilCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRepairCost:
                    this.RepairCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLaborAmount:
                    this.LaborAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLaborUnit:
                    this.LaborUnit = attValue;
                    break;
                case cLaborPrice:
                    this.LaborPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLaborCost:
                    this.LaborCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cServiceUnits:
                    this.ServiceUnits = attValue;
                    break;
                case cServiceCapacityUnits:
                    this.ServiceCapacityUnits = attValue;
                    break;
                case cCapitalRecoveryCost:
                    this.CapitalRecoveryCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTaxesHousingInsuranceCost:
                    this.TaxesHousingInsuranceCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOptionForCapacity:
                    this.OptionForCapacity = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cOptionForInflation:
                    this.OptionForFuel = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cOptionForTime:
                    this.OptionForTime = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cOptionForFuel:
                    this.OptionForFuel = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cLaborAmountAdj:
                    this.LaborAmountAdj = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetMachinery1InputProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cMarketValue:
                    sPropertyValue = this.MarketValue.ToString();
                    break;
                case cListPriceAdj:
                    sPropertyValue = this.ListPriceAdj.ToString();
                    break;
                case cSalvageValue:
                    sPropertyValue = this.SalvageValue.ToString();
                    break;
                case cFuelAmount:
                    sPropertyValue = this.FuelAmount.ToString();
                    break;
                case cFuelUnit:
                    sPropertyValue = this.FuelUnit.ToString();
                    break;
                case cFuelPrice:
                    sPropertyValue = this.FuelPrice.ToString();
                    break;
                case cFuelCost:
                    sPropertyValue = this.FuelCost.ToString();
                    break;
                case cLubeOilAmount:
                    sPropertyValue = this.LubeOilAmount.ToString();
                    break;
                case cLubeOilUnit:
                    sPropertyValue = this.LubeOilUnit.ToString();
                    break;
                case cLubeOilPrice:
                    sPropertyValue = this.LubeOilPrice.ToString();
                    break;
                case cLubeOilCost:
                    sPropertyValue = this.LubeOilCost.ToString();
                    break;
                case cRepairCost:
                    sPropertyValue = this.RepairCost.ToString();
                    break;
                case cLaborAmount:
                    sPropertyValue = this.LaborAmount.ToString();
                    break;
                case cLaborUnit:
                    sPropertyValue = this.LaborUnit.ToString();
                    break;
                case cLaborPrice:
                    sPropertyValue = this.LaborPrice.ToString();
                    break;
                case cLaborCost:
                    sPropertyValue = this.LaborCost.ToString();
                    break;
                case cServiceUnits:
                    sPropertyValue = this.ServiceUnits.ToString();
                    break;
                case cServiceCapacityUnits:
                    sPropertyValue = this.ServiceCapacityUnits.ToString();
                    break;
                case cCapitalRecoveryCost:
                    sPropertyValue = this.CapitalRecoveryCost.ToString();
                    break;
                case cTaxesHousingInsuranceCost:
                    sPropertyValue = this.TaxesHousingInsuranceCost.ToString();
                    break;
                case cOptionForCapacity:
                    sPropertyValue = this.OptionForCapacity.ToString();
                    break;
                case cOptionForInflation:
                    sPropertyValue = this.OptionForFuel.ToString();
                    break;
                case cOptionForTime:
                    sPropertyValue = this.OptionForTime.ToString();
                    break;
                case cOptionForFuel:
                    sPropertyValue = this.OptionForFuel.ToString();
                    break;
                case cLaborAmountAdj:
                    sPropertyValue = this.LaborAmountAdj.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        
        public void SetMachinery1InputAttributes(CalculatorParameters calcParameters,
            ref XElement calculator, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //set the base input's new calculations
            this.SetInputAttributes(calcParameters, ref currentElement, updates);
            this.SetAndRemoveCalculatorAttributes(string.Empty, ref calculator);
            //set the constants
            this.Constants.SetMachinery1ConstantAttributes(ref calculator);
            //use the same attributes within the calculator as well
            this.SetNewInputAttributes(calcParameters, ref calculator);
            //set this object
            string sAttNameExtension = string.Empty;
            this.Sizes.SetSizeRangesAttributes(sAttNameExtension, ref calculator);
            SetMachinery1InputAttributes(sAttNameExtension,
                ref calculator);
        }
        public void SetMachinery1InputAttributes(CalculatorParameters calcParameters,
            ref XElement calculator, ref XElement currentElement)
        {
            this.SetAndRemoveCalculatorAttributes(string.Empty, ref calculator);
            //set the constants
            this.Constants.SetMachinery1ConstantAttributes(ref calculator);
            //use the same attributes within the calculator as well
            this.SetNewInputAttributes(calcParameters, ref calculator);
            //set this object
            string sAttNameExtension = string.Empty;
            this.Sizes.SetSizeRangesAttributes(sAttNameExtension, ref calculator);
            SetMachinery1InputAttributes(sAttNameExtension, ref calculator);
        }
        public virtual void SetMachinery1InputAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cMarketValue, attNameExtension), this.MarketValue);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cListPriceAdj, attNameExtension), this.ListPriceAdj);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
               string.Concat(cSalvageValue, attNameExtension), this.SalvageValue);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cFuelAmount, attNameExtension), this.FuelAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cFuelUnit, attNameExtension), this.FuelUnit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
               string.Concat(cFuelPrice, attNameExtension), this.FuelPrice);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cFuelCost, attNameExtension), this.FuelCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cLubeOilAmount, attNameExtension), this.LubeOilAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cLubeOilUnit, attNameExtension), this.LubeOilUnit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
               string.Concat(cLubeOilPrice, attNameExtension), this.LubeOilPrice);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cLubeOilCost, attNameExtension), this.LubeOilCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cRepairCost, attNameExtension), this.RepairCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cLaborAmount, attNameExtension), this.LaborAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cLaborUnit, attNameExtension), this.LaborUnit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                string.Concat(cLaborPrice, attNameExtension), this.LaborPrice);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cLaborCost, attNameExtension), this.LaborCost);

            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cServiceUnits, attNameExtension), this.ServiceUnits);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cServiceCapacityUnits, attNameExtension), this.ServiceCapacityUnits);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cCapitalRecoveryCost, attNameExtension), this.CapitalRecoveryCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cTaxesHousingInsuranceCost, attNameExtension), this.TaxesHousingInsuranceCost);
            CalculatorHelpers.SetAttributeInt(calculator,
                string.Concat(cOptionForCapacity, attNameExtension), this.OptionForCapacity);
            CalculatorHelpers.SetAttributeInt(calculator,
                string.Concat(cOptionForInflation, attNameExtension), this.OptionForInflation);
            CalculatorHelpers.SetAttributeInt(calculator,
                string.Concat(cOptionForTime, attNameExtension), this.OptionForTime);
            CalculatorHelpers.SetAttributeInt(calculator,
                string.Concat(cOptionForFuel, attNameExtension), this.OptionForFuel);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(cLaborAmountAdj, attNameExtension), this.LaborAmountAdj);
        }
        public virtual void SetMachinery1InputAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                 string.Concat(cMarketValue, attNameExtension), this.MarketValue.ToString());
            writer.WriteAttributeString(
                string.Concat(cListPriceAdj, attNameExtension), this.ListPriceAdj.ToString());
            writer.WriteAttributeString(
               string.Concat(cSalvageValue, attNameExtension), this.SalvageValue.ToString());
            writer.WriteAttributeString(
                string.Concat(cFuelAmount, attNameExtension), this.FuelAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(cFuelUnit, attNameExtension), this.FuelUnit.ToString());
            writer.WriteAttributeString(
               string.Concat(cFuelPrice, attNameExtension), this.FuelPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(cFuelCost, attNameExtension), this.FuelCost.ToString());
            writer.WriteAttributeString(
                string.Concat(cLubeOilAmount, attNameExtension), this.LubeOilAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(cLubeOilUnit, attNameExtension), this.LubeOilUnit.ToString());
            writer.WriteAttributeString(
               string.Concat(cLubeOilPrice, attNameExtension), this.LubeOilPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(cLubeOilCost, attNameExtension), this.LubeOilCost.ToString());
            writer.WriteAttributeString(
                string.Concat(cRepairCost, attNameExtension), this.RepairCost.ToString());
            writer.WriteAttributeString(
                string.Concat(cLaborAmount, attNameExtension), this.LaborAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(cLaborUnit, attNameExtension), this.LaborUnit.ToString());
            writer.WriteAttributeString(
                string.Concat(cLaborPrice, attNameExtension), this.LaborPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(cLaborCost, attNameExtension), this.LaborCost.ToString());

            writer.WriteAttributeString(
                string.Concat(cServiceUnits, attNameExtension), this.ServiceUnits.ToString());
            writer.WriteAttributeString(
                string.Concat(cServiceCapacityUnits, attNameExtension), this.ServiceCapacityUnits.ToString());
            writer.WriteAttributeString(
                string.Concat(cCapitalRecoveryCost, attNameExtension), this.CapitalRecoveryCost.ToString());
            writer.WriteAttributeString(
                string.Concat(cTaxesHousingInsuranceCost, attNameExtension), this.TaxesHousingInsuranceCost.ToString());
            writer.WriteAttributeString(
                string.Concat(cOptionForCapacity, attNameExtension), this.OptionForCapacity.ToString());
            writer.WriteAttributeString(
                string.Concat(cOptionForInflation, attNameExtension), this.OptionForInflation.ToString());
            writer.WriteAttributeString(
                string.Concat(cOptionForTime, attNameExtension), this.OptionForTime.ToString());
            writer.WriteAttributeString(
                string.Concat(cOptionForFuel, attNameExtension), this.OptionForFuel.ToString());
            writer.WriteAttributeString(
                string.Concat(cLaborAmountAdj, attNameExtension), this.LaborAmountAdj.ToString());
        }
        
    }
}
