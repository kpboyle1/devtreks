using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The IrrPower1Stock class extends the IrrigationPower1Input 
    ///             class and is used by machinery resource stock calculators 
    ///             and analyzers to set totals. Basic water stock statistical 
    ///             analyzers derive from this class to generate additional statistics.
    ///Author:		www.devtr2014, Aprileks.org
    ///Date:		2012, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. This class analyzes irrigation stock costs.
    /// </summary>           
    public class IrrPower1Stock : Machinery1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public IrrPower1Stock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
            //water
            InitTotalIrrPower1StockProperties();
        }
        //copy constructor
        public IrrPower1Stock(IrrPower1Stock calculator)
            : base(calculator)
        {
            CopyTotalIrrPower1StockProperties(calculator);
        }

        //calculator properties
        //irrigation power stock collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<IrrigationPower1Input>> IrrPowerStocks = null;
        //totals
        public double TotalEngineEfficiency { get; set; }
        public double TotalFuelConsumptionPerHour { get; set; }
        public double TotalWaterHP { get; set; }
        public double TotalBrakeHP { get; set; }
        public double TotalFlowRate { get; set; }
        public double TotalStaticHead { get; set; }
        public double TotalPressureHead { get; set; }
        public double TotalFrictionHead { get; set; }
        public double TotalOtherHead { get; set; }
        public double TotalPumpEfficiency { get; set; }
        public double TotalGearDriveEfficiency { get; set; }
        public double TotalExtraPower1 { get; set; }
        public double TotalExtraPower2 { get; set; }
        public double TotalEnergyExtraCostPerNetAcOrHa { get; set; }
        //all costs are per acre inch
        public double TotalEnergyExtraCost { get; set; }
        //PHRS in McGrann, conversion to acre inches per hour, for hourly cost estimate
        public double TotalPumpCapacity { get; set; }
        public double TotalEngineFlywheelPower { get; set; }
        public double TotalFuelAmountRequired { get; set; }
        public double TotalPumpingPlantPerformance { get; set; }
        //water properties
        public double TotalSeasonWaterNeed { get; set; }
        public double TotalSeasonWaterExtraCredit { get; set; }
        public double TotalSeasonWaterExtraDebit { get; set; }
        //district delivered water price
        public double TotalWaterPrice { get; set; }
        public double TotalDistributionUniformity { get; set; }
        public double TotalSeasonWaterApplied { get; set; }
        public double TotalWaterCost { get; set; }
        public double TotalPumpHoursPerUnitArea { get; set; }

        //labor properties
        //substitute input.Times in ops and comps
        public double TotalIrrigationTimes { get; set; }
        public double TotalIrrigationDurationPerSet { get; set; }
        //if irrigators irrigate other fields at same time
        public double TotalIrrigationDurationLaborHoursPerSet { get; set; }
        //substitute oporcomp.Amount in ops and comps
        //substitute timeperiod.Amount in budgets
        public double TotalIrrigationNetArea { get; set; }
        public double TotalEquipmentLaborAmount { get; set; }
        public double TotalEquipmentLaborCost { get; set; }
        //repair and maintenance
        public double TotalRepairCostsPerNetAcOrHa { get; set; }
        public double TotalRandMPercent { get; set; }

        //attribute names
        private const string TEngineEfficiency = "TEngineEfficiency";
        private const string TFuelConsumptionPerHour = "TFuelConsumptionPerHour";
        private const string TWaterHP = "TWaterHP";
        private const string TBrakeHP = "TBrakeHP";
        private const string TFlowRate = "TFlowRate";
        private const string TStaticHead = "TStaticHead";
        private const string TPressureHead = "TPressureHead";
        private const string TFrictionHead = "TFrictionHead";
        private const string TOtherHead = "TOtherHead";
        private const string TPumpEfficiency = "TPumpEfficiency";
        private const string TGearDriveEfficiency = "TGearDriveEfficiency";
        private const string TExtraPower1 = "TExtraPower1";
        private const string TExtraPower2 = "TExtraPower2";
        private const string TEnergyExtraCostPerNetAcOrHa = "TEnergyExtraCostPerNetAcOrHa";
        //all costs are per acre inch
        private const string TEnergyExtraCost = "TEnergyExtraCost";
        //PHRS in McGrann, conversion to acre inches per hour, for hourly cost estimate
        private const string TPumpCapacity = "TPumpCapacity";
        private const string TEngineFlywheelPower = "TEngineFlywheelPower";
        private const string TFuelAmountRequired = "TFuelAmountRequired";
        private const string TPumpingPlantPerformance = "TPumpingPlantPerformance";
        //water properties
        private const string TSeasonWaterNeed = "TSeasonWaterNeed";
        private const string TSeasonWaterExtraCredit = "TSeasonWaterExtraCredit";
        private const string TSeasonWaterExtraDebit = "TSeasonWaterExtraDebit";
        //district delivered water price
        private const string TWaterPrice = "TWaterPrice";
        private const string TDistributionUniformity = "TDistributionUniformity";
        private const string TSeasonWaterApplied = "TSeasonWaterApplied";
        private const string TWaterCost = "TWaterCost";
        private const string TPumpHoursPerUnitArea = "TPumpHoursPerUnitArea";

        //labor properties
        //substitute input.Times in ops and comps
        private const string TIrrigationTimes = "TIrrigationTimes";
        private const string TIrrigationDurationPerSet = "TIrrigationDurationPerSet";
        //if irrigators irrigate other fields at same time
        private const string TIrrigationDurationLaborHoursPerSet = "TIrrigationDurationLaborHoursPerSet";
        //substitute oporcomp.Amount in ops and comps
        //substitute timeperiod.Amount in budgets
        private const string TIrrigationNetArea = "TIrrigationNetArea";
        private const string TEquipmentLaborAmount = "TEquipmentLaborAmount";
        private const string TEquipmentLaborCost = "TEquipmentLaborCost";
        //repair and maintenance
        private const string TRepairCostsPerNetAcOrHa = "TRepairCostsPerNetAcOrHa";
        private const string TRandMPercent = "TRandMPercent";

        public virtual void InitTotalIrrPower1StockProperties()
        {
            this.TotalEngineEfficiency = 0;
            this.TotalFuelConsumptionPerHour = 0;
            this.TotalWaterHP = 0;
            this.TotalBrakeHP = 0;
            this.TotalFlowRate = 0;
            this.TotalStaticHead = 0;
            this.TotalPressureHead = 0;
            this.TotalFrictionHead = 0;
            this.TotalOtherHead = 0;
            this.TotalPumpEfficiency = 0;
            this.TotalGearDriveEfficiency = 0;
            this.TotalExtraPower1 = 0;
            this.TotalExtraPower2 = 0;
            this.TotalEnergyExtraCostPerNetAcOrHa = 0;
            this.TotalEnergyExtraCost = 0;
            this.TotalPumpCapacity = 0;
            this.TotalEngineFlywheelPower = 0;
            this.TotalFuelAmountRequired = 0;
            this.TotalPumpingPlantPerformance = 0;
            this.TotalSeasonWaterNeed = 0;
            this.TotalSeasonWaterExtraCredit = 0;
            this.TotalSeasonWaterExtraDebit = 0;
            this.TotalWaterPrice = 0;
            this.TotalDistributionUniformity = 0;
            this.TotalSeasonWaterApplied = 0;
            this.TotalWaterCost = 0;
            this.TotalPumpHoursPerUnitArea = 0;
            this.TotalIrrigationTimes = 0;
            this.TotalIrrigationDurationPerSet = 0;
            this.TotalIrrigationDurationLaborHoursPerSet = 0;
            this.TotalIrrigationNetArea = 0;
            this.TotalEquipmentLaborAmount = 0;
            this.TotalEquipmentLaborCost = 0;
            this.TotalRepairCostsPerNetAcOrHa = 0;
            this.TotalRandMPercent = 0;
        }
        public virtual void CopyTotalIrrPower1StockProperties(
            IrrPower1Stock calculator)
        {
            this.TotalEngineEfficiency = calculator.TotalEngineEfficiency;
            this.TotalFuelConsumptionPerHour = calculator.TotalFuelConsumptionPerHour;
            this.TotalWaterHP = calculator.TotalWaterHP;
            this.TotalBrakeHP = calculator.TotalBrakeHP;
            this.TotalFlowRate = calculator.TotalFlowRate;
            this.TotalStaticHead = calculator.TotalStaticHead;
            this.TotalPressureHead = calculator.TotalPressureHead;
            this.TotalFrictionHead = calculator.TotalFrictionHead;
            this.TotalOtherHead = calculator.TotalOtherHead;
            this.TotalPumpEfficiency = calculator.TotalPumpEfficiency;
            this.TotalGearDriveEfficiency = calculator.TotalGearDriveEfficiency;
            this.TotalExtraPower1 = calculator.TotalExtraPower1;
            this.TotalExtraPower2 = calculator.TotalExtraPower2;
            this.TotalEnergyExtraCostPerNetAcOrHa = calculator.TotalEnergyExtraCostPerNetAcOrHa;
            this.TotalEnergyExtraCost = calculator.TotalEnergyExtraCost;
            this.TotalPumpCapacity = calculator.TotalPumpCapacity;
            this.TotalEngineFlywheelPower = calculator.TotalEngineFlywheelPower;
            this.TotalFuelAmountRequired = calculator.TotalFuelAmountRequired;
            this.TotalPumpingPlantPerformance = calculator.TotalPumpingPlantPerformance;
            this.TotalSeasonWaterNeed = calculator.TotalSeasonWaterNeed;
            this.TotalSeasonWaterExtraCredit = calculator.TotalSeasonWaterExtraCredit;
            this.TotalSeasonWaterExtraDebit = calculator.TotalSeasonWaterExtraDebit;
            this.TotalWaterPrice = calculator.TotalWaterPrice;
            this.TotalDistributionUniformity = calculator.TotalDistributionUniformity;
            this.TotalSeasonWaterApplied = calculator.TotalSeasonWaterApplied;
            this.TotalWaterCost = calculator.TotalWaterCost;
            this.TotalPumpHoursPerUnitArea = calculator.TotalPumpHoursPerUnitArea;
            this.TotalIrrigationTimes = calculator.TotalIrrigationTimes;
            this.TotalIrrigationDurationPerSet = calculator.TotalIrrigationDurationPerSet;
            this.TotalIrrigationDurationLaborHoursPerSet = calculator.TotalIrrigationDurationLaborHoursPerSet;
            this.TotalIrrigationNetArea = calculator.TotalIrrigationNetArea;
            this.TotalEquipmentLaborAmount = calculator.TotalEquipmentLaborAmount;
            this.TotalEquipmentLaborCost = calculator.TotalEquipmentLaborCost;
            this.TotalRepairCostsPerNetAcOrHa = calculator.TotalRepairCostsPerNetAcOrHa;
            this.TotalRandMPercent = calculator.TotalRandMPercent;
        }
        
        public virtual void SetTotalIrrPower1StockProperties(XElement calculator)
        {
            this.TotalEngineEfficiency = CalculatorHelpers.GetAttributeDouble(calculator, TEngineEfficiency);
            this.TotalFuelConsumptionPerHour = CalculatorHelpers.GetAttributeDouble(calculator, TFuelConsumptionPerHour);
            this.TotalWaterHP = CalculatorHelpers.GetAttributeDouble(calculator, TWaterHP);
            this.TotalBrakeHP = CalculatorHelpers.GetAttributeDouble(calculator, TBrakeHP);
            this.TotalFlowRate = CalculatorHelpers.GetAttributeDouble(calculator, TFlowRate);
            this.TotalStaticHead = CalculatorHelpers.GetAttributeDouble(calculator, TStaticHead);
            this.TotalPressureHead = CalculatorHelpers.GetAttributeDouble(calculator, TPressureHead);
            this.TotalFrictionHead = CalculatorHelpers.GetAttributeDouble(calculator, TFrictionHead);
            this.TotalOtherHead = CalculatorHelpers.GetAttributeDouble(calculator, TOtherHead);
            this.TotalPumpEfficiency = CalculatorHelpers.GetAttributeDouble(calculator, TPumpEfficiency);
            this.TotalGearDriveEfficiency = CalculatorHelpers.GetAttributeDouble(calculator, TGearDriveEfficiency);
            this.TotalExtraPower1 = CalculatorHelpers.GetAttributeDouble(calculator, TExtraPower1);
            this.TotalExtraPower2 = CalculatorHelpers.GetAttributeDouble(calculator, TExtraPower2);
            this.TotalEnergyExtraCostPerNetAcOrHa = CalculatorHelpers.GetAttributeDouble(calculator, TEnergyExtraCostPerNetAcOrHa);
            this.TotalEnergyExtraCost = CalculatorHelpers.GetAttributeDouble(calculator, TEnergyExtraCost);
            this.TotalPumpCapacity = CalculatorHelpers.GetAttributeDouble(calculator, TPumpCapacity);
            this.TotalEngineFlywheelPower = CalculatorHelpers.GetAttributeDouble(calculator, TEngineFlywheelPower);
            this.TotalFuelAmountRequired = CalculatorHelpers.GetAttributeDouble(calculator, TFuelAmountRequired);
            this.TotalPumpingPlantPerformance = CalculatorHelpers.GetAttributeDouble(calculator, TPumpingPlantPerformance);
            this.TotalSeasonWaterNeed = CalculatorHelpers.GetAttributeDouble(calculator, TSeasonWaterNeed);
            this.TotalSeasonWaterExtraCredit = CalculatorHelpers.GetAttributeDouble(calculator, TSeasonWaterExtraCredit);
            this.TotalSeasonWaterExtraDebit = CalculatorHelpers.GetAttributeDouble(calculator, TSeasonWaterExtraDebit);
            this.TotalWaterPrice = CalculatorHelpers.GetAttributeDouble(calculator, TWaterPrice);
            this.TotalDistributionUniformity = CalculatorHelpers.GetAttributeDouble(calculator, TDistributionUniformity);
            this.TotalSeasonWaterApplied = CalculatorHelpers.GetAttributeDouble(calculator, TSeasonWaterApplied);
            this.TotalWaterCost = CalculatorHelpers.GetAttributeDouble(calculator, TWaterCost);
            this.TotalPumpHoursPerUnitArea = CalculatorHelpers.GetAttributeDouble(calculator, TPumpHoursPerUnitArea);

            this.TotalIrrigationTimes = CalculatorHelpers.GetAttributeDouble(calculator, TIrrigationTimes);
            this.TotalIrrigationDurationPerSet = CalculatorHelpers.GetAttributeDouble(calculator, TIrrigationDurationPerSet);
            this.TotalIrrigationDurationLaborHoursPerSet = CalculatorHelpers.GetAttributeDouble(calculator, TIrrigationDurationLaborHoursPerSet);
            this.TotalIrrigationNetArea = CalculatorHelpers.GetAttributeDouble(calculator, TIrrigationNetArea);
            this.TotalEquipmentLaborAmount = CalculatorHelpers.GetAttributeDouble(calculator, TEquipmentLaborAmount);
            this.TotalEquipmentLaborCost = CalculatorHelpers.GetAttributeDouble(calculator, TEquipmentLaborCost);
            this.TotalRepairCostsPerNetAcOrHa = CalculatorHelpers.GetAttributeDouble(calculator, TRepairCostsPerNetAcOrHa);
            this.TotalRandMPercent = CalculatorHelpers.GetAttributeDouble(calculator, TRandMPercent);
        }
        public virtual void SetTotalIrrPower1StockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TEngineEfficiency:
                    this.TotalEngineEfficiency = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFuelConsumptionPerHour:
                    this.TotalFuelConsumptionPerHour = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWaterHP:
                    this.TotalWaterHP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBrakeHP:
                    this.TotalBrakeHP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFlowRate:
                    this.TotalFlowRate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TStaticHead:
                    this.TotalStaticHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPressureHead:
                    this.TotalPressureHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFrictionHead:
                    this.TotalFrictionHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOtherHead:
                    this.TotalOtherHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPumpEfficiency:
                    this.TotalPumpEfficiency = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TGearDriveEfficiency:
                    this.TotalGearDriveEfficiency = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TExtraPower1:
                    this.TotalExtraPower1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TExtraPower2:
                    this.TotalExtraPower2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEnergyExtraCostPerNetAcOrHa:
                    this.TotalEnergyExtraCostPerNetAcOrHa = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEnergyExtraCost:
                    this.TotalEnergyExtraCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPumpCapacity:
                    this.TotalPumpCapacity = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEngineFlywheelPower:
                    this.TotalEngineFlywheelPower = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFuelAmountRequired:
                    this.TotalFuelAmountRequired = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPumpingPlantPerformance:
                    this.TotalPumpingPlantPerformance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSeasonWaterNeed:
                    this.TotalSeasonWaterNeed = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSeasonWaterExtraCredit:
                    this.TotalSeasonWaterExtraCredit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSeasonWaterExtraDebit:
                    this.TotalSeasonWaterExtraDebit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWaterPrice:
                    this.TotalWaterPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TDistributionUniformity:
                    this.TotalDistributionUniformity = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSeasonWaterApplied:
                    this.TotalSeasonWaterApplied = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWaterCost:
                    this.TotalWaterCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TPumpHoursPerUnitArea:
                    this.TotalPumpHoursPerUnitArea = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIrrigationTimes:
                    this.TotalIrrigationTimes = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIrrigationDurationPerSet:
                    this.TotalIrrigationDurationPerSet = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIrrigationDurationLaborHoursPerSet:
                    this.TotalIrrigationDurationLaborHoursPerSet = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TIrrigationNetArea:
                    this.TotalIrrigationNetArea = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEquipmentLaborAmount:
                    this.TotalEquipmentLaborAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEquipmentLaborCost:
                    this.TotalEquipmentLaborCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRepairCostsPerNetAcOrHa:
                    this.TotalRepairCostsPerNetAcOrHa = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRandMPercent:
                    this.TotalRandMPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalIrrPower1StockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TEngineEfficiency:
                    sPropertyValue = this.TotalEngineEfficiency.ToString();
                    break;
                case TFuelConsumptionPerHour:
                    sPropertyValue = this.TotalFuelConsumptionPerHour.ToString();
                    break;
                case TWaterHP:
                    sPropertyValue = this.TotalWaterHP.ToString();
                    break;
                case TBrakeHP:
                    sPropertyValue = this.TotalBrakeHP.ToString();
                    break;
                case TFlowRate:
                    sPropertyValue = this.TotalFlowRate.ToString();
                    break;
                case TStaticHead:
                    sPropertyValue = this.TotalStaticHead.ToString();
                    break;
                case TPressureHead:
                    sPropertyValue = this.TotalPressureHead.ToString();
                    break;
                case TFrictionHead:
                    sPropertyValue = this.TotalFrictionHead.ToString();
                    break;
                case TOtherHead:
                    sPropertyValue = this.TotalOtherHead.ToString();
                    break;
                case TPumpEfficiency:
                    sPropertyValue = this.TotalPumpEfficiency.ToString();
                    break;
                case TGearDriveEfficiency:
                    sPropertyValue = this.TotalGearDriveEfficiency.ToString();
                    break;
                case TExtraPower1:
                    sPropertyValue = this.TotalExtraPower1.ToString();
                    break;
                case TExtraPower2:
                    sPropertyValue = this.TotalExtraPower2.ToString();
                    break;
                case TEnergyExtraCostPerNetAcOrHa:
                    sPropertyValue = this.TotalEnergyExtraCostPerNetAcOrHa.ToString();
                    break;
                case TEnergyExtraCost:
                    sPropertyValue = this.TotalEnergyExtraCost.ToString();
                    break;
                case TPumpCapacity:
                    sPropertyValue = this.TotalPumpCapacity.ToString();
                    break;
                case TEngineFlywheelPower:
                    sPropertyValue = this.TotalEngineFlywheelPower.ToString();
                    break;
                case TFuelAmountRequired:
                    sPropertyValue = this.TotalFuelAmountRequired.ToString();
                    break;
                case TPumpingPlantPerformance:
                    sPropertyValue = this.TotalPumpingPlantPerformance.ToString();
                    break;
                case TSeasonWaterNeed:
                    sPropertyValue = this.TotalSeasonWaterNeed.ToString();
                    break;
                case TSeasonWaterExtraCredit:
                    sPropertyValue = this.TotalSeasonWaterExtraCredit.ToString();
                    break;
                case TSeasonWaterExtraDebit:
                    sPropertyValue = this.TotalSeasonWaterExtraDebit.ToString();
                    break;
                case TWaterPrice:
                    sPropertyValue = this.TotalWaterPrice.ToString();
                    break;
                case TDistributionUniformity:
                    sPropertyValue = this.TotalDistributionUniformity.ToString();
                    break;
                case TSeasonWaterApplied:
                    sPropertyValue = this.TotalSeasonWaterApplied.ToString();
                    break;
                case TWaterCost:
                    sPropertyValue = this.TotalWaterCost.ToString();
                    break;
                case TPumpHoursPerUnitArea:
                    sPropertyValue = this.TotalPumpHoursPerUnitArea.ToString();
                    break;
                case TIrrigationTimes:
                    sPropertyValue = this.TotalIrrigationTimes.ToString();
                    break;
                case TIrrigationDurationPerSet:
                    sPropertyValue = this.TotalIrrigationDurationPerSet.ToString();
                    break;
                case TIrrigationDurationLaborHoursPerSet:
                    sPropertyValue = this.TotalIrrigationDurationLaborHoursPerSet.ToString();
                    break;
                case TIrrigationNetArea:
                    sPropertyValue = this.TotalIrrigationNetArea.ToString();
                    break;
                case TEquipmentLaborAmount:
                    sPropertyValue = this.TotalEquipmentLaborAmount.ToString();
                    break;
                case TEquipmentLaborCost:
                    sPropertyValue = this.TotalEquipmentLaborCost.ToString();
                    break;
                case TRepairCostsPerNetAcOrHa:
                    sPropertyValue = this.TotalRepairCostsPerNetAcOrHa.ToString();
                    break;
                case TRandMPercent:
                    sPropertyValue = this.TotalRandMPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalIrrPower1StockAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TEngineEfficiency, attNameExtension),
                this.TotalEngineEfficiency);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TFuelConsumptionPerHour, attNameExtension),
                this.TotalFuelConsumptionPerHour);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TWaterHP, attNameExtension),
                this.TotalWaterHP);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TBrakeHP, attNameExtension),
                this.TotalBrakeHP);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TFlowRate, attNameExtension),
                this.TotalFlowRate);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TStaticHead, attNameExtension),
                this.TotalStaticHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TPressureHead, attNameExtension),
                this.TotalPressureHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TFrictionHead, attNameExtension),
                this.TotalFrictionHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TOtherHead, attNameExtension),
                this.TotalOtherHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TPumpEfficiency, attNameExtension),
                this.TotalPumpEfficiency);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TGearDriveEfficiency, attNameExtension),
                this.TotalGearDriveEfficiency);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TExtraPower1, attNameExtension),
                this.TotalExtraPower1);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TExtraPower2, attNameExtension),
                this.TotalExtraPower2);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TEnergyExtraCostPerNetAcOrHa, attNameExtension),
                this.TotalEnergyExtraCostPerNetAcOrHa);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TEnergyExtraCost, attNameExtension),
                this.TotalEnergyExtraCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TPumpCapacity, attNameExtension),
                this.TotalPumpCapacity);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TEngineFlywheelPower, attNameExtension),
                this.TotalEngineFlywheelPower);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TFuelAmountRequired, attNameExtension),
                this.TotalFuelAmountRequired);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TPumpingPlantPerformance, attNameExtension),
                this.TotalPumpingPlantPerformance);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TSeasonWaterNeed, attNameExtension),
                this.TotalSeasonWaterNeed);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TSeasonWaterExtraCredit, attNameExtension),
                this.TotalSeasonWaterExtraCredit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TSeasonWaterExtraDebit, attNameExtension),
                this.TotalSeasonWaterExtraDebit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TWaterPrice, attNameExtension),
                this.TotalWaterPrice);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TDistributionUniformity, attNameExtension),
                this.TotalDistributionUniformity);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TSeasonWaterApplied, attNameExtension),
                this.TotalSeasonWaterApplied);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TWaterCost, attNameExtension),
                this.TotalWaterCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TPumpHoursPerUnitArea, attNameExtension),
                this.TotalPumpHoursPerUnitArea);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TIrrigationTimes, attNameExtension),
                this.TotalIrrigationTimes);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TIrrigationDurationPerSet, attNameExtension),
                this.TotalIrrigationDurationPerSet);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TIrrigationDurationLaborHoursPerSet, attNameExtension),
                this.TotalIrrigationDurationLaborHoursPerSet);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TIrrigationNetArea, attNameExtension),
                this.TotalIrrigationNetArea);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TEquipmentLaborAmount, attNameExtension),
                this.TotalEquipmentLaborAmount);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TEquipmentLaborCost, attNameExtension),
                this.TotalEquipmentLaborCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TRepairCostsPerNetAcOrHa, attNameExtension),
                this.TotalRepairCostsPerNetAcOrHa);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                string.Concat(TRandMPercent, attNameExtension),
                this.TotalRandMPercent);
        }
        public virtual void SetTotalIrrPower1StockAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TEngineEfficiency, attNameExtension),
                this.TotalEngineEfficiency.ToString());
            writer.WriteAttributeString(
                string.Concat(TFuelConsumptionPerHour, attNameExtension),
                this.TotalFuelConsumptionPerHour.ToString());
            writer.WriteAttributeString(
                string.Concat(TWaterHP, attNameExtension),
                this.TotalWaterHP.ToString());
            writer.WriteAttributeString(
                string.Concat(TBrakeHP, attNameExtension),
                this.TotalBrakeHP.ToString());
            writer.WriteAttributeString(
                string.Concat(TFlowRate, attNameExtension),
                this.TotalFlowRate.ToString());
            writer.WriteAttributeString(
                string.Concat(TStaticHead, attNameExtension),
                this.TotalStaticHead.ToString());
            writer.WriteAttributeString(
                string.Concat(TPressureHead, attNameExtension),
                this.TotalPressureHead.ToString());
            writer.WriteAttributeString(
                string.Concat(TFrictionHead, attNameExtension),
                this.TotalFrictionHead.ToString());
            writer.WriteAttributeString(
                string.Concat(TOtherHead, attNameExtension),
                this.TotalOtherHead.ToString());
            writer.WriteAttributeString(
                string.Concat(TPumpEfficiency, attNameExtension),
                this.TotalPumpEfficiency.ToString());
            writer.WriteAttributeString(
                string.Concat(TGearDriveEfficiency, attNameExtension),
                this.TotalGearDriveEfficiency.ToString());
            writer.WriteAttributeString(
                string.Concat(TExtraPower1, attNameExtension),
                this.TotalExtraPower1.ToString());
            writer.WriteAttributeString(
                string.Concat(TExtraPower2, attNameExtension),
                this.TotalExtraPower2.ToString());
            writer.WriteAttributeString(
                string.Concat(TEnergyExtraCostPerNetAcOrHa, attNameExtension),
                this.TotalEnergyExtraCostPerNetAcOrHa.ToString());
            writer.WriteAttributeString(
                string.Concat(TEnergyExtraCost, attNameExtension),
                this.TotalEnergyExtraCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TPumpCapacity, attNameExtension),
                this.TotalPumpCapacity.ToString());
            writer.WriteAttributeString(
                string.Concat(TEngineFlywheelPower, attNameExtension),
                this.TotalEngineFlywheelPower.ToString());
            writer.WriteAttributeString(
                string.Concat(TFuelAmountRequired, attNameExtension),
                this.TotalFuelAmountRequired.ToString());
            writer.WriteAttributeString(
                string.Concat(TPumpingPlantPerformance, attNameExtension),
                this.TotalPumpingPlantPerformance.ToString());
            writer.WriteAttributeString(
                string.Concat(TSeasonWaterNeed, attNameExtension),
                this.TotalSeasonWaterNeed.ToString());
            writer.WriteAttributeString(
                string.Concat(TSeasonWaterExtraCredit, attNameExtension),
                this.TotalSeasonWaterExtraCredit.ToString());
            writer.WriteAttributeString(
                string.Concat(TSeasonWaterExtraDebit, attNameExtension),
                this.TotalSeasonWaterExtraDebit.ToString());
            writer.WriteAttributeString(
                string.Concat(TWaterPrice, attNameExtension),
                this.TotalWaterPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(TDistributionUniformity, attNameExtension),
                this.TotalDistributionUniformity.ToString());
            writer.WriteAttributeString(
                string.Concat(TSeasonWaterApplied, attNameExtension),
                this.TotalSeasonWaterApplied.ToString());
            writer.WriteAttributeString(
                string.Concat(TWaterCost, attNameExtension),
                this.TotalWaterCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TPumpHoursPerUnitArea, attNameExtension),
                this.TotalPumpHoursPerUnitArea.ToString());
            writer.WriteAttributeString(
                string.Concat(TIrrigationTimes, attNameExtension),
                this.TotalIrrigationTimes.ToString());
            writer.WriteAttributeString(
                string.Concat(TIrrigationDurationPerSet, attNameExtension),
                this.TotalIrrigationDurationPerSet.ToString());
            writer.WriteAttributeString(
                string.Concat(TIrrigationDurationLaborHoursPerSet, attNameExtension),
                this.TotalIrrigationDurationLaborHoursPerSet.ToString());
            writer.WriteAttributeString(
                string.Concat(TIrrigationNetArea, attNameExtension),
                this.TotalIrrigationNetArea.ToString());
            writer.WriteAttributeString(
                string.Concat(TEquipmentLaborAmount, attNameExtension),
                this.TotalEquipmentLaborAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(TEquipmentLaborCost, attNameExtension),
                this.TotalEquipmentLaborCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TRepairCostsPerNetAcOrHa, attNameExtension),
                this.TotalRepairCostsPerNetAcOrHa.ToString());
            writer.WriteAttributeString(
                string.Concat(TRandMPercent, attNameExtension),
                this.TotalRandMPercent.ToString());
        }
    }
    public static class IrrPower1Extensions
    {
        //add a IrrigationPower1Input to the baseStat.IrrPowerStocks dictionary
        public static bool AddIrrPower1StocksToDictionary(
            this IrrPower1Stock baseStat,
            int filePosition, int nodePosition, IrrigationPower1Input calculator)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.IrrPowerStocks == null)
                baseStat.IrrPowerStocks
                = new Dictionary<int, List<IrrigationPower1Input>>();
            if (baseStat.IrrPowerStocks.ContainsKey(filePosition))
            {
                if (baseStat.IrrPowerStocks[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.IrrPowerStocks[filePosition].Count <= i)
                        {
                            baseStat.IrrPowerStocks[filePosition]
                                .Add(new IrrigationPower1Input());
                        }
                    }
                    baseStat.IrrPowerStocks[filePosition][nodePosition]
                        = calculator;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<IrrigationPower1Input> baseStats
                    = new List<IrrigationPower1Input>();
                KeyValuePair<int, List<IrrigationPower1Input>> newStat
                    = new KeyValuePair<int, List<IrrigationPower1Input>>(
                        filePosition, baseStats);
                baseStat.IrrPowerStocks.Add(newStat);
                bIsAdded = AddIrrPower1StocksToDictionary(baseStat,
                    filePosition, nodePosition, calculator);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this IrrPower1Stock baseStat,
            int filePosition, IrrigationPower1Input calculator)
        {
            int iNodeCount = 0;
            if (baseStat.IrrPowerStocks == null)
                return iNodeCount;
            if (baseStat.IrrPowerStocks.ContainsKey(filePosition))
            {
                if (baseStat.IrrPowerStocks[filePosition] != null)
                {
                    iNodeCount = baseStat.IrrPowerStocks[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}
