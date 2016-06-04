using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Constants and locals used by calculators
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>      
    public class Machinery1Constant
    {
        //string constants
        private const string cMachConstantId = "MachConstantId";
        private const string cMachConstantGroupId = "MachConstantGroupId";
        public const string cServiceCapacity = "ServiceCapacity";
        private const string cRF1 = "RF1";
        private const string cRF2 = "RF2";
        private const string cRV1 = "RV1";
        private const string cRV2 = "RV2";
        private const string cRV3 = "RV3";
        public const string cFuelType = "FuelType";
        private const string cLaborType = "LaborType";
        private const string cWidth = "Width";
        private const string cStartingHrs = "StartingHrs";
        private const string cPlannedUseHrs = "PlannedUseHrs";
        private const string cUsefulLifeHrs = "UsefulLifeHrs";
        private const string cHP = "HP";
        public const string cHPPTOEquiv = "HPPTOEquiv";
        public const string cHPPTOMax = "HPPTOMax";
        private const string cFieldEffRange = "FieldEffRange";
        private const string cFieldSpeedRange = "FieldSpeedRange";
        private const string cFieldSpeedRangekm = "FieldSpeedRangekm";
        private const string cFieldSpeedTypical = "FieldSpeedTypical";
        private const string cFieldSpeedTypicalkm = "FieldSpeedTypicalkm";
        private const string cFieldEffTypical = "FieldEffTypical";
        private const string cTotalLifeRandM = "TotalLifeRandM";
        public const string cPriceGas = "PriceGas";
        public const string cPriceDiesel = "PriceDiesel";
        public const string cPriceLP = "PriceLP";
        public const string cPriceElectric = "PriceElectric";
        public const string cPriceNG = "PriceNG";
        public const string cPriceOil = "PriceOil";
        private const string cPriceRegularLabor = "PriceRegularLabor";
        private const string cPriceMachineryLabor = "PriceMachineryLabor";
        private const string cPriceSupervisorLabor = "PriceSupervisorLabor";
        private const string cHousingPercent = "HousingPercent";
        private const string cTaxPercent = "TaxPercent";
        private const string cInsurePercent = "InsurePercent";

        //properties
        public int MachConstantId { get; set; }
        public int MachConstantGroupId { get; set; }
        //alias for OCAmount
        public double ServiceCapacity { get; set; }
        public double RF1 { get; set; }
        public double RF2 { get; set; }
        public double RV1 { get; set; }
        public double RV2 { get; set; }
        public double RV3 { get; set; }
        public string FuelType { get; set; }
        public string LaborType { get; set; }
        public double Width { get; set; }
        public int StartingHrs { get; set; }
        public int PlannedUseHrs { get; set; }
        public int UsefulLifeHrs { get; set; }
        public int HP { get; set; }
        public int HPPTOEquiv { get; set; }
        public int HPPTOMax { get; set; }
        public string FieldEffRange { get; set; }
        public string FieldSpeedRange { get; set; }
        public string FieldSpeedRangekm { get; set; }
        public double FieldSpeedTypical { get; set; }
        public double FieldSpeedTypicalkm { get; set; }
        public double FieldEffTypical { get; set; }
        public double TotalLifeRandM { get; set; }
        public double PriceGas { get; set; }
        public double PriceDiesel { get; set; }
        public double PriceLP { get; set; }
        public double PriceElectric { get; set; }
        public double PriceNG { get; set; }
        public double PriceOil { get; set; }
        public double PriceRegularLabor { get; set; }
        public double PriceMachineryLabor { get; set; }
        public double PriceSupervisorLabor { get; set; }
        public double HousingPercent { get; set; }
        public double TaxPercent { get; set; }
        public double InsurePercent { get; set; }

        public void SetMachinery1ConstantProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            this.MachConstantId = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cMachConstantId);
            this.MachConstantGroupId = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cMachConstantGroupId);
            this.ServiceCapacity = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cServiceCapacity);
            this.RF1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cRF1);
            this.RF2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cRF2);
            this.RV1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cRV1);
            this.RV2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cRV2);
            this.RV3 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cRV3);
            this.FuelType = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFuelType);
            this.LaborType = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cLaborType);
            this.Width = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cWidth);
            this.StartingHrs = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cStartingHrs);
            this.PlannedUseHrs = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cPlannedUseHrs);
            this.UsefulLifeHrs = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cUsefulLifeHrs);
            this.HP = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cHP);
            this.HPPTOEquiv = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cHPPTOEquiv);
            this.HPPTOMax = CalculatorHelpers.GetAttributeInt(currentCalculationsElement,
               cHPPTOMax);
            this.FieldEffRange = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFieldEffRange);
            this.FieldSpeedRange = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFieldSpeedRange);
            this.FieldSpeedRangekm = CalculatorHelpers.GetAttribute(currentCalculationsElement,
               cFieldSpeedRangekm);
            this.FieldSpeedTypical = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cFieldSpeedTypical);
            this.FieldSpeedTypicalkm = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cFieldSpeedTypicalkm);
            this.FieldEffTypical = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cFieldEffTypical);
            this.TotalLifeRandM = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cTotalLifeRandM);
            this.PriceGas = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceGas);
            this.PriceDiesel = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceDiesel);
            this.PriceLP = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceLP);
            this.PriceElectric = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceElectric);
            this.PriceNG = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceNG);
            this.PriceOil = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceOil);
            this.PriceRegularLabor = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceRegularLabor);
            this.PriceMachineryLabor = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceMachineryLabor);
            this.PriceSupervisorLabor = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cPriceSupervisorLabor);
            this.HousingPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cHousingPercent);
            this.TaxPercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cTaxPercent);
            this.InsurePercent = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
               cInsurePercent);
        }
        public void SetMachinery1ConstantProperties(Machinery1Constant constant)
        {
            this.MachConstantId = constant.MachConstantId;
            this.MachConstantGroupId = constant.MachConstantGroupId;
            this.ServiceCapacity = constant.ServiceCapacity;
            this.RF1 = constant.RF1;
            this.RF2 = constant.RF2;
            this.RV1 = constant.RV1;
            this.RV2 = constant.RV2;
            this.RV3 = constant.RV3;
            this.FuelType = constant.FuelType;
            this.LaborType = constant.LaborType;
            this.Width = constant.Width;
            this.StartingHrs = constant.StartingHrs;
            this.PlannedUseHrs = constant.PlannedUseHrs;
            this.UsefulLifeHrs = constant.UsefulLifeHrs;
            this.HP = constant.HP;
            this.HPPTOEquiv = constant.HPPTOEquiv;
            this.HPPTOMax = constant.HPPTOMax;
            this.FieldEffRange = constant.FieldEffRange;
            this.FieldSpeedRange = constant.FieldSpeedRange;
            this.FieldSpeedRangekm = constant.FieldSpeedRangekm;
            this.FieldSpeedTypical = constant.FieldSpeedTypical;
            this.FieldSpeedTypicalkm = constant.FieldSpeedTypicalkm;
            this.FieldEffTypical = constant.FieldEffTypical;
            this.TotalLifeRandM = constant.TotalLifeRandM;
            this.PriceGas = constant.PriceGas;
            this.PriceDiesel = constant.PriceDiesel;
            this.PriceLP = constant.PriceLP;
            this.PriceElectric = constant.PriceElectric;
            this.PriceNG = constant.PriceNG;
            this.PriceOil = constant.PriceOil;
            this.PriceRegularLabor = constant.PriceRegularLabor;
            this.PriceMachineryLabor = constant.PriceMachineryLabor;
            this.PriceSupervisorLabor = constant.PriceSupervisorLabor;
            this.HousingPercent = constant.HousingPercent;
            this.TaxPercent = constant.TaxPercent;
            this.InsurePercent = constant.InsurePercent;
        }
        public void SetMachinery1ConstantAttributes(
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cMachConstantId, this.MachConstantId);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cMachConstantGroupId, this.MachConstantGroupId);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cServiceCapacity, this.ServiceCapacity);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cRF1, this.RF1);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cRF2, this.RF2);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cRV1, this.RV1);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cRV2, this.RV2);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cRV3, this.RV3);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFuelType, this.FuelType);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cLaborType, this.LaborType);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cWidth, this.Width);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cStartingHrs, this.StartingHrs);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cPlannedUseHrs, this.PlannedUseHrs);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cUsefulLifeHrs, this.UsefulLifeHrs);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cHP, this.HP);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cHPPTOEquiv, this.HPPTOEquiv);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               cHPPTOMax, this.HPPTOMax);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFieldEffRange, this.FieldEffRange);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFieldSpeedRange, this.FieldSpeedRange);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cFieldSpeedRangekm, this.FieldSpeedRangekm);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cFieldSpeedTypical, this.FieldSpeedTypical);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cFieldSpeedTypicalkm, this.FieldSpeedTypicalkm);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cFieldEffTypical, this.FieldEffTypical);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cTotalLifeRandM, this.TotalLifeRandM);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceGas, this.PriceGas);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceDiesel, this.PriceDiesel);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceLP, this.PriceLP);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceElectric, this.PriceElectric);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceNG, this.PriceNG);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceOil, this.PriceOil);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceRegularLabor, this.PriceRegularLabor);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceMachineryLabor, this.PriceMachineryLabor);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               cPriceSupervisorLabor, this.PriceSupervisorLabor);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cHousingPercent, this.HousingPercent);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cTaxPercent, this.TaxPercent);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cInsurePercent, this.InsurePercent);
        }
    }
}
