using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The Machinery1Stock class extends the Machinery1Input 
    ///             class and is used by machinery resource stock calculators 
    ///             and analyzers to set totals. Basic machinery stock statistical 
    ///             analyzers derive from this class to generate additional statistics.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. The attributes used in this class will evolve. Feedback may 
    ///             point to fewer, or more, attributes.
    /// </summary>           
    public class Machinery1Stock : Machinery1Input
    {
        //calls the base-class version, and initializes the base class properties.
        public Machinery1Stock()
            : base()
        {
            //base input (don't init more)
            InitCalculatorProperties();
            //machinery
            InitTotalMachinery1StockProperties();
            InitTotalMachinery1ConstantProperties();
        }
        //copy constructor
        public Machinery1Stock(Machinery1Stock calculator)
            : base(calculator)
        {
            CopyTotalMachinery1StockProperties(calculator);
            CopyTotalMachinery1ConstantProperties(calculator);
        }

        //calculator properties
        //machinery collection
        public IDictionary<int, List<Machinery1Input>> MachineryStocks = null;

        //totals
        public double TotalMarketValue { get; set; }
        public double TotalSalvageValue { get; set; }
        public double TotalFuelAmount { get; set; }
        public double TotalFuelPrice { get; set; }
        public double TotalFuelCost { get; set; }
        public double TotalLubeOilAmount { get; set; }
        public double TotalLubeOilPrice { get; set; }
        public double TotalLubeOilCost { get; set; }
        public double TotalRepairCost { get; set; }
        public double TotalLaborAmount { get; set; }
        public double TotalLaborPrice { get; set; }
        public double TotalLaborCost { get; set; }
        public double TotalCapitalRecoveryCost { get; set; }
        public double TotalTaxesHousingInsuranceCost { get; set; }
        //totals for constants
        public double TotalPriceGas { get; set; }
        public double TotalPriceDiesel { get; set; }
        public double TotalPriceLP { get; set; }
        public double TotalPriceElectric { get; set; }
        public double TotalPriceNG { get; set; }
        public double TotalPriceOil { get; set; }

        public double TotalPriceRegularLabor { get; set; }
        public double TotalPriceMachineryLabor { get; set; }
        public double TotalPriceSupervisorLabor { get; set; }

        public double TotalStartingHrs { get; set; }
        public double TotalPlannedUseHrs { get; set; }
        //expended hrs = total ocamount
        public double TotalUsefulLifeHrs { get; set; }

        public double TotalHousingPercent { get; set; }
        public double TotalTaxPercent { get; set; }
        public double TotalInsurePercent { get; set; }

        public double TotalSpeed { get; set; }
        public double TotalWidth { get; set; }
        public double TotalHorsepower { get; set; }
        public double TotalHPPTOEquiv { get; set; }
        public double TotalFieldEffTypical { get; set; }
        //totals
        private const string TMarketValue = "TMarketValue";
        public const string TSalvageValue = "TSalvageValue";
        public const string TFuelAmount = "TFuelAmount";
        public const string TFuelPrice = "TFuelPrice";
        public const string TFuelCost = "TFuelCost";
        public const string TLubeOilAmount = "TLubeOilAmount";
        public const string TLubeOilPrice = "TLubeOilPrice";
        public const string TLubeOilCost = "TLubeOilCost";
        public const string TRepairCost = "TRepairCost";
        private const string TLaborAmount = "TLaborAmount";
        private const string TLaborPrice = "TLaborPrice";
        public const string TLaborCost = "TLaborCost";
        private const string TCapitalRecoveryCost = "TCapitalRecoveryCost";
        private const string TTaxesHousingInsuranceCost = "TTaxesHousingInsuranceCost";
        //totals for constants (keep them here because this class will be a base class for stats analysis)
        public const string TPriceGas = "TPriceGas";
        public const string TPriceDiesel = "TPriceDiesel";
        public const string TPriceLP = "TPriceLP";
        public const string TPriceElectric = "TPriceElectric";
        public const string TPriceNG = "TPriceNG";
        public const string TPriceOil = "TPriceOil";

        public const string TStartingHrs = "TStartingHrs";
        public const string TPlannedUseHrs = "TPlannedUseHrs";
        public const string TUsefulLifeHrs = "TUsefulLifeHrs";

        private const string TPriceRegularLabor = "TPriceRegularLabor";
        private const string TPriceMachineryLabor = "TPriceMachineryLabor";
        private const string TPriceSupervisorLabor = "TPriceSupervisorLabor";
        private const string THousingPercent = "THousingPercent";
        private const string TTaxPercent = "TTaxPercent";
        private const string TInsurePercent = "TInsurePercent";

        private const string TSpeed = "TSpeed";
        private const string TWidth = "TWidth";
        private const string THorsepower = "THP";
        private const string THPPTOEquiv = "THPPTOEquiv";
        private const string TFieldEffTypical = "TFieldEffTypical";

        public virtual void InitTotalMachinery1StockProperties()
        {
            this.TotalMarketValue = 0;
            this.TotalSalvageValue = 0;
            this.TotalFuelAmount = 0;
            this.TotalFuelPrice = 0;
            this.TotalFuelCost = 0;
            this.TotalLubeOilAmount = 0;
            this.TotalLubeOilPrice = 0;
            this.TotalLubeOilCost = 0;
            this.TotalRepairCost = 0;
            this.TotalLaborAmount = 0;
            this.TotalLaborPrice = 0;
            this.TotalLaborCost = 0;
            this.TotalCapitalRecoveryCost = 0;
            this.TotalTaxesHousingInsuranceCost = 0;
        }
        public virtual void CopyTotalMachinery1StockProperties(
            Machinery1Stock calculator)
        {
            this.TotalMarketValue = calculator.TotalMarketValue;
            this.TotalSalvageValue = calculator.TotalSalvageValue;
            this.TotalFuelAmount = calculator.TotalFuelAmount;
            this.TotalFuelPrice = calculator.TotalFuelPrice;
            this.TotalFuelCost = calculator.TotalFuelCost;
            this.TotalLubeOilPrice = calculator.TotalLubeOilPrice;
            this.TotalLubeOilAmount = calculator.TotalLubeOilAmount;
            this.TotalLubeOilCost = calculator.TotalLubeOilCost;
            this.TotalRepairCost = calculator.TotalRepairCost;
            this.TotalLaborAmount = calculator.TotalLaborAmount;
            this.TotalLaborPrice = calculator.TotalLaborPrice;
            this.TotalLaborCost = calculator.TotalLaborCost;
            this.TotalCapitalRecoveryCost = calculator.TotalCapitalRecoveryCost;
            this.TotalTaxesHousingInsuranceCost = calculator.TotalTaxesHousingInsuranceCost;
        }
        public virtual void SetTotalMachinery1StockProperties(XElement calculator)
        {
            this.TotalSalvageValue = CalculatorHelpers.GetAttributeDouble(calculator,
                TSalvageValue);
            this.TotalMarketValue = CalculatorHelpers.GetAttributeDouble(calculator,
                TMarketValue);
            this.TotalFuelAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TFuelAmount);
            this.TotalFuelPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TFuelPrice);
            this.TotalFuelCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TFuelCost);
            this.TotalLubeOilAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TLubeOilAmount);
            this.TotalLubeOilPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TLubeOilPrice);
            this.TotalLubeOilCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TLubeOilCost);
            this.TotalRepairCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TRepairCost);
            this.TotalLaborAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TLaborAmount);
            this.TotalLaborPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TLaborPrice);
            this.TotalLaborCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TLaborCost);
            this.TotalCapitalRecoveryCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TCapitalRecoveryCost);
            this.TotalTaxesHousingInsuranceCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TTaxesHousingInsuranceCost);
        }
        public virtual void SetTotalMachinery1StockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TSalvageValue:
                    this.TotalSalvageValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TMarketValue:
                    this.TotalMarketValue = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFuelAmount:
                    this.TotalFuelAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFuelPrice:
                    this.TotalFuelPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFuelCost:
                    this.TotalFuelCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLubeOilAmount:
                    this.TotalLubeOilAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLubeOilPrice:
                    this.TotalLubeOilPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLubeOilCost:
                    this.TotalLubeOilCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRepairCost:
                    this.TotalRepairCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLaborAmount:
                    this.TotalLaborAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLaborPrice:
                    this.TotalLaborPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLaborCost:
                    this.TotalLaborCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCapitalRecoveryCost:
                    this.TotalCapitalRecoveryCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTaxesHousingInsuranceCost:
                    this.TotalTaxesHousingInsuranceCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalMachinery1StockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TSalvageValue:
                    sPropertyValue = this.TotalSalvageValue.ToString();
                    break;
                case TMarketValue:
                    sPropertyValue = this.TotalMarketValue.ToString();
                    break;
                case TFuelAmount:
                    sPropertyValue = this.TotalFuelAmount.ToString();
                    break;
                case TFuelPrice:
                    sPropertyValue = this.TotalFuelPrice.ToString();
                    break;
                case TFuelCost:
                    sPropertyValue = this.TotalFuelCost.ToString();
                    break;
                case TLubeOilAmount:
                    sPropertyValue = this.TotalLubeOilAmount.ToString();
                    break;
                case TLubeOilPrice:
                    sPropertyValue = this.TotalLubeOilPrice.ToString();
                    break;
                case TLubeOilCost:
                    sPropertyValue = this.TotalLubeOilCost.ToString();
                    break;
                case TRepairCost:
                    sPropertyValue = this.TotalRepairCost.ToString();
                    break;
                case TLaborAmount:
                    sPropertyValue = this.TotalLaborAmount.ToString();
                    break;
                case TLaborPrice:
                    sPropertyValue = this.TotalLaborPrice.ToString();
                    break;
                case TLaborCost:
                    sPropertyValue = this.TotalLaborCost.ToString();
                    break;
                case TCapitalRecoveryCost:
                    sPropertyValue = this.TotalCapitalRecoveryCost.ToString();
                    break;
                case TTaxesHousingInsuranceCost:
                    sPropertyValue = this.TotalTaxesHousingInsuranceCost.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalMachinery1StockAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TSalvageValue, attNameExtension),
                this.TotalSalvageValue);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TMarketValue, attNameExtension),
                this.TotalMarketValue);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TFuelAmount, attNameExtension),
                this.TotalFuelAmount);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TFuelPrice, attNameExtension),
                this.TotalFuelPrice);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TFuelCost, attNameExtension),
                this.TotalFuelCost);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLubeOilAmount, attNameExtension),
                this.TotalLubeOilAmount);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLubeOilPrice, attNameExtension),
                this.TotalLubeOilPrice);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLubeOilCost, attNameExtension),
                this.TotalLubeOilCost);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TRepairCost, attNameExtension),
                this.TotalRepairCost);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLaborAmount, attNameExtension),
                this.TotalLaborAmount);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLaborPrice, attNameExtension),
                this.TotalLaborPrice);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLaborCost, attNameExtension),
                this.TotalLaborCost);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCapitalRecoveryCost, attNameExtension),
                this.TotalCapitalRecoveryCost);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTaxesHousingInsuranceCost, attNameExtension),
                this.TotalTaxesHousingInsuranceCost);
        }
        public virtual void SetTotalMachinery1StockAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TSalvageValue, attNameExtension),
                this.TotalSalvageValue.ToString());
            writer.WriteAttributeString(
                string.Concat(TMarketValue, attNameExtension),
                this.TotalMarketValue.ToString());
            writer.WriteAttributeString(
                string.Concat(TFuelAmount, attNameExtension),
                this.TotalFuelAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(TFuelPrice, attNameExtension),
                this.TotalFuelPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(TFuelCost, attNameExtension),
                this.TotalFuelCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TLubeOilAmount, attNameExtension),
                this.TotalLubeOilAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(TLubeOilPrice, attNameExtension),
                this.TotalLubeOilPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(TLubeOilCost, attNameExtension),
                this.TotalLubeOilCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TRepairCost, attNameExtension),
                this.TotalRepairCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TLaborAmount, attNameExtension),
                this.TotalLaborAmount.ToString());
            writer.WriteAttributeString(
               string.Concat(TLaborPrice, attNameExtension),
               this.TotalLaborPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(TLaborCost, attNameExtension),
                this.TotalLaborCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TCapitalRecoveryCost, attNameExtension),
                this.TotalCapitalRecoveryCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TTaxesHousingInsuranceCost, attNameExtension),
                this.TotalTaxesHousingInsuranceCost.ToString());
        }
        //constant totals
        public virtual void InitTotalMachinery1ConstantProperties()
        {
            this.TotalPriceGas = 0;
            this.TotalPriceDiesel = 0;
            this.TotalPriceLP = 0;
            this.TotalPriceElectric = 0;
            this.TotalPriceNG = 0;
            this.TotalPriceOil = 0;
            this.TotalPriceRegularLabor = 0;
            this.TotalPriceMachineryLabor = 0;
            this.TotalPriceSupervisorLabor = 0;
            this.TotalStartingHrs = 0;
            this.TotalPlannedUseHrs = 0;
            this.TotalUsefulLifeHrs = 0;
            this.TotalHousingPercent = 0;
            this.TotalTaxPercent = 0;
            this.TotalInsurePercent = 0;
            this.TotalSpeed = 0;
            this.TotalWidth = 0;
            this.TotalHorsepower = 0;
            this.TotalHPPTOEquiv = 0;
            this.TotalFieldEffTypical = 0;
        }
        public virtual void CopyTotalMachinery1ConstantProperties(
           Machinery1Stock calculator)
        {
            this.TotalPriceGas = calculator.TotalPriceGas;
            this.TotalPriceDiesel = calculator.TotalPriceDiesel;
            this.TotalPriceLP = calculator.TotalPriceLP;
            this.TotalPriceElectric = calculator.TotalPriceElectric;
            this.TotalPriceNG = calculator.TotalPriceNG;
            this.TotalPriceOil = calculator.TotalPriceOil;
            this.TotalPriceRegularLabor = calculator.TotalPriceRegularLabor;
            this.TotalPriceMachineryLabor = calculator.TotalPriceMachineryLabor;
            this.TotalPriceSupervisorLabor = calculator.TotalPriceSupervisorLabor;
            this.TotalStartingHrs = calculator.TotalStartingHrs;
            this.TotalPlannedUseHrs = calculator.TotalPlannedUseHrs;
            this.TotalUsefulLifeHrs = calculator.TotalUsefulLifeHrs;
            this.TotalHousingPercent = calculator.TotalHousingPercent;
            this.TotalTaxPercent = calculator.TotalTaxPercent;
            this.TotalInsurePercent = calculator.TotalInsurePercent;
            this.TotalSpeed = calculator.TotalSpeed;
            this.TotalWidth = calculator.TotalWidth;
            this.TotalHorsepower = calculator.TotalHorsepower;
            this.TotalHPPTOEquiv = calculator.TotalHPPTOEquiv;
            this.TotalFieldEffTypical = calculator.TotalFieldEffTypical;
        }
        public virtual void SetTotalMachinery1ConstantProperties(XElement calculator)
        {
            this.TotalPriceDiesel = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceDiesel);
            this.TotalPriceGas = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceGas);
            this.TotalPriceLP = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceLP);
            this.TotalPriceElectric = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceElectric);
            this.TotalPriceNG = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceNG);
            this.TotalPriceOil = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceOil);
            this.TotalPriceRegularLabor = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceRegularLabor);
            this.TotalPriceMachineryLabor = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceMachineryLabor);
            this.TotalPriceSupervisorLabor = CalculatorHelpers.GetAttributeDouble(calculator,
                TPriceSupervisorLabor);
            this.TotalStartingHrs = CalculatorHelpers.GetAttributeDouble(calculator,
               TStartingHrs);
            this.TotalPlannedUseHrs = CalculatorHelpers.GetAttributeDouble(calculator,
               TPlannedUseHrs);
            this.TotalUsefulLifeHrs = CalculatorHelpers.GetAttributeDouble(calculator,
               TUsefulLifeHrs);
            this.TotalHousingPercent = CalculatorHelpers.GetAttributeDouble(calculator,
                THousingPercent);
            this.TotalTaxPercent = CalculatorHelpers.GetAttributeDouble(calculator,
                TTaxPercent);
            this.TotalInsurePercent = CalculatorHelpers.GetAttributeDouble(calculator,
                TInsurePercent);
            this.TotalSpeed = CalculatorHelpers.GetAttributeDouble(calculator,
                TSpeed);
            this.TotalWidth = CalculatorHelpers.GetAttributeDouble(calculator,
                TWidth);
            this.TotalHorsepower = CalculatorHelpers.GetAttributeDouble(calculator,
                THorsepower);
            this.TotalHPPTOEquiv = CalculatorHelpers.GetAttributeDouble(calculator,
                THPPTOEquiv);
            this.TotalFieldEffTypical = CalculatorHelpers.GetAttributeDouble(calculator,
                TFieldEffTypical);
        }
        public virtual void SetTotalMachinery1ConstantProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TPriceDiesel:
                    this.TotalPriceDiesel = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceGas:
                    this.TotalPriceGas = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceLP:
                    this.TotalPriceLP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceElectric:
                    this.TotalPriceElectric = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceNG:
                    this.TotalPriceNG = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceOil:
                    this.TotalPriceOil = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceRegularLabor:
                    this.TotalPriceRegularLabor = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceMachineryLabor:
                    this.TotalPriceMachineryLabor = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPriceSupervisorLabor:
                    this.TotalPriceSupervisorLabor = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TStartingHrs:
                    this.TotalStartingHrs = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPlannedUseHrs:
                    this.TotalPlannedUseHrs = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TUsefulLifeHrs:
                    this.TotalUsefulLifeHrs = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case THousingPercent:
                    this.TotalHousingPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTaxPercent:
                    this.TotalTaxPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TInsurePercent:
                    this.TotalInsurePercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSpeed:
                    this.TotalSpeed = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWidth:
                    this.TotalWidth = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case THorsepower:
                    this.TotalHorsepower = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case THPPTOEquiv:
                    this.TotalHPPTOEquiv = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFieldEffTypical:
                    this.TotalFieldEffTypical = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalMachinery1ConstantProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TPriceDiesel:
                    sPropertyValue = this.TotalPriceDiesel.ToString();
                    break;
                case TPriceGas:
                    sPropertyValue = this.TotalPriceGas.ToString();
                    break;
                case TPriceLP:
                    sPropertyValue = this.TotalPriceLP.ToString();
                    break;
                case TPriceElectric:
                    sPropertyValue = this.TotalPriceElectric.ToString();
                    break;
                case TPriceNG:
                    sPropertyValue = this.TotalPriceNG.ToString();
                    break;
                case TPriceOil:
                    sPropertyValue = this.TotalPriceOil.ToString();
                    break;
                case TPriceRegularLabor:
                    sPropertyValue = this.TotalPriceRegularLabor.ToString();
                    break;
                case TPriceMachineryLabor:
                    sPropertyValue = this.TotalPriceMachineryLabor.ToString();
                    break;
                case TPriceSupervisorLabor:
                    sPropertyValue = this.TotalPriceSupervisorLabor.ToString();
                    break;
                case TStartingHrs:
                    sPropertyValue = this.TotalStartingHrs.ToString();
                    break;
                case TPlannedUseHrs:
                    sPropertyValue = this.TotalPlannedUseHrs.ToString();
                    break;
                case TUsefulLifeHrs:
                    sPropertyValue = this.TotalUsefulLifeHrs.ToString();
                    break;
                case THousingPercent:
                    sPropertyValue = this.TotalHousingPercent.ToString();
                    break;
                case TTaxPercent:
                    sPropertyValue = this.TotalTaxPercent.ToString();
                    break;
                case TInsurePercent:
                    sPropertyValue = this.TotalInsurePercent.ToString();
                    break;
                case TSpeed:
                    sPropertyValue = this.TotalSpeed.ToString();
                    break;
                case TWidth:
                    sPropertyValue = this.TotalWidth.ToString();
                    break;
                case THorsepower:
                    sPropertyValue = this.TotalHorsepower.ToString();
                    break;
                case THPPTOEquiv:
                    sPropertyValue = this.TotalHPPTOEquiv.ToString();
                    break;
                case TFieldEffTypical:
                    sPropertyValue = this.TotalFieldEffTypical.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalMachinery1ConstantAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceDiesel, attNameExtension),
                this.TotalPriceDiesel);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceGas, attNameExtension),
                this.TotalPriceGas);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceLP, attNameExtension),
                this.TotalPriceLP);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceElectric, attNameExtension),
                this.TotalPriceElectric);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceNG, attNameExtension),
                this.TotalPriceNG);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceOil, attNameExtension),
                this.TotalPriceOil);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceRegularLabor, attNameExtension),
                this.TotalPriceRegularLabor);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceMachineryLabor, attNameExtension),
                this.TotalPriceMachineryLabor);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TPriceSupervisorLabor, attNameExtension),
                this.TotalPriceSupervisorLabor);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TStartingHrs, attNameExtension),
                this.TotalStartingHrs);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(TPlannedUseHrs, attNameExtension),
               this.TotalPlannedUseHrs);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TUsefulLifeHrs, attNameExtension),
                this.TotalUsefulLifeHrs);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(THousingPercent, attNameExtension),
                this.TotalHousingPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTaxPercent, attNameExtension),
                this.TotalTaxPercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TInsurePercent, attNameExtension),
                this.TotalInsurePercent);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(TSpeed, attNameExtension),
               this.TotalSpeed);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(TWidth, attNameExtension),
               this.TotalWidth);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(THorsepower, attNameExtension),
               this.TotalHorsepower);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(THPPTOEquiv, attNameExtension),
               this.TotalHPPTOEquiv);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
               string.Concat(TFieldEffTypical, attNameExtension),
               this.TotalFieldEffTypical);
        }
        public virtual void SetTotalMachinery1ConstantAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TPriceDiesel, attNameExtension),
                this.TotalPriceDiesel.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceGas, attNameExtension),
                this.TotalPriceGas.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceLP, attNameExtension),
                this.TotalPriceLP.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceElectric, attNameExtension),
                this.TotalPriceElectric.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceNG, attNameExtension),
                this.TotalPriceNG.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceOil, attNameExtension),
                this.TotalPriceOil.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceRegularLabor, attNameExtension),
                this.TotalPriceRegularLabor.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceMachineryLabor, attNameExtension),
                this.TotalPriceMachineryLabor.ToString());
            writer.WriteAttributeString(
                string.Concat(TPriceSupervisorLabor, attNameExtension),
                this.TotalPriceSupervisorLabor.ToString());
            writer.WriteAttributeString(
                string.Concat(TStartingHrs, attNameExtension),
                this.TotalStartingHrs.ToString());
            writer.WriteAttributeString(
                string.Concat(TPlannedUseHrs, attNameExtension),
                this.TotalPlannedUseHrs.ToString());
            writer.WriteAttributeString(
                string.Concat(TUsefulLifeHrs, attNameExtension),
                this.TotalUsefulLifeHrs.ToString());
            writer.WriteAttributeString(
                string.Concat(THousingPercent, attNameExtension),
                this.TotalHousingPercent.ToString());
            writer.WriteAttributeString(
                string.Concat(TTaxPercent, attNameExtension),
                this.TotalTaxPercent.ToString());
            writer.WriteAttributeString(
               string.Concat(TInsurePercent, attNameExtension),
               this.TotalInsurePercent.ToString());
            writer.WriteAttributeString(
              string.Concat(TSpeed, attNameExtension),
              this.TotalSpeed.ToString());
            writer.WriteAttributeString(
              string.Concat(TWidth, attNameExtension),
              this.TotalWidth.ToString());
            writer.WriteAttributeString(
              string.Concat(THorsepower, attNameExtension),
              this.TotalHorsepower.ToString());
            writer.WriteAttributeString(
              string.Concat(THPPTOEquiv, attNameExtension),
              this.TotalHPPTOEquiv.ToString());
            writer.WriteAttributeString(
              string.Concat(TFieldEffTypical, attNameExtension),
              this.TotalFieldEffTypical.ToString());
        }
    }
    public static class Machinery1Extensions
    {
        //add a Machinery1Input to the baseStat.MachineryStocks dictionary
        public static bool AddMachinery1StocksToDictionary(
            this Machinery1Stock baseStat,
            int filePosition, int nodePosition, Machinery1Input calculator)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.MachineryStocks == null)
                baseStat.MachineryStocks
                = new Dictionary<int, List<Machinery1Input>>();
            if (baseStat.MachineryStocks.ContainsKey(filePosition))
            {
                if (baseStat.MachineryStocks[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.MachineryStocks[filePosition].Count <= i)
                        {
                            baseStat.MachineryStocks[filePosition]
                                .Add(new Machinery1Input());
                        }
                    }
                    baseStat.MachineryStocks[filePosition][nodePosition]
                        = calculator;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<Machinery1Input> baseStats
                    = new List<Machinery1Input>();
                KeyValuePair<int, List<Machinery1Input>> newStat
                    = new KeyValuePair<int, List<Machinery1Input>>(
                        filePosition, baseStats);
                baseStat.MachineryStocks.Add(newStat);
                bIsAdded = AddMachinery1StocksToDictionary(baseStat,
                    filePosition, nodePosition, calculator);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this Machinery1Stock baseStat,
            int filePosition, Machinery1Input calculator)
        {
            int iNodeCount = 0;
            if (baseStat.MachineryStocks == null)
                return iNodeCount;
            if (baseStat.MachineryStocks.ContainsKey(filePosition))
            {
                if (baseStat.MachineryStocks[filePosition] != null)
                {
                    iNodeCount = baseStat.MachineryStocks[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}
