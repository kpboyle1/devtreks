using System.Collections.Generic;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The IrrigationPower1Input class is a base class used 
    ///             by most irrigation calculators/analyzers to hold 
    ///             resource stock totals (i.e. water consumed by a crop). 
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This class is considered a basic starting point for understanding 
    ///             the economics of irrigation water budgeting. Costs are computed on a 
    ///             per volume of water applied (acin or m3) per acre or hectare. 
    ///             For example, 25 acin per acre * $2 totalcost per acin = $120 per acre
    /// </summary>                  
    public class IrrigationPower1Input : Machinery1Input
    {
        //flow rate: 1 ac in per hr = 450 gpm (for calculations)
        public const double GPMPerHourToAcInHr = 450;
        //flow rate: 1 l/s = 15,852 US gallons per hour
        public const double LiterPerSecondToGMP = 15852;
        //1 af of water lifted 1 foot uses 1.372 HP
        //1 ac in of water lifed 1 foot uses .1142 hP
        public const double HPPerAcInch = .1143;
        //1 cubic meter per hour = 0.278 liters per second
        public const double LitersPerSecondToCubicMperHour = 0.278; 
        //1 ac inch = 102.8 cubic meters
        public const double AcInchToCubicMeters = 102.8; 
        //constructor
        public IrrigationPower1Input()
        {
            InitIrrigationPower1InputProperties();
        }
        //copy constructors
        public IrrigationPower1Input(IrrigationPower1Input calculator)
        {
            CopyIrrigationPower1InputProperties(calculator);
        }
        //copies the underlying input and locals props too
        public IrrigationPower1Input(CalculatorParameters calcParameters,
            IrrigationPower1Input calculator)
        {
            CopyIrrigationPower1InputProperties(calcParameters, calculator);
        }
        public enum FUEL_CONSUMPTION_TYPES
        {
            //McGrann or Jaee approach (flywheel, potential high estimate)
            energyuse       = 1,
            //Nebraska Pump Plant Criteria (whp, low estimate)
            nppc           = 2
        }
        public static FUEL_CONSUMPTION_TYPES GetFuelConsumptionType(int optionForFuelConsumption)
        {
            FUEL_CONSUMPTION_TYPES eFuelType = FUEL_CONSUMPTION_TYPES.energyuse;
            if (optionForFuelConsumption == 2)
            {
                eFuelType = FUEL_CONSUMPTION_TYPES.nppc;
            }
            return eFuelType;
        }
        //power properties
        public int OptionForFuelConsumption { get; set; }
        public double EngineEfficiency { get; set; }
        public double FuelConsumptionPerHour { get; set; }
        public double WaterHP { get; set; }
        public double BrakeHP { get; set; }
        public double FlowRate { get; set; }
        public double StaticHead { get; set; }
        public double PressureHead { get; set; }
        public double FrictionHead { get; set; }
        public double OtherHead { get; set; }
        public double PumpEfficiency { get; set; }
        public double GearDriveEfficiency { get; set; }
        public double ExtraPower1 { get; set; }
        public double ExtraPower2 { get; set; }
        public double EnergyExtraCostPerNetAcOrHa { get; set; }
        //all costs are per acre inch
        public double EnergyExtraCost { get; set; }
        //PHRS in McGrann, conversion to acre inches per hour, for hourly cost estimate
        public double PumpCapacity { get; set; }
        public double EngineFlywheelPower { get; set; }
        public double FuelAmountRequired { get; set; }
        public double PumpingPlantPerformance { get; set; }
        //water properties
        public double SeasonWaterNeed { get; set; }
        public double SeasonWaterExtraCredit { get; set; }
        public double SeasonWaterExtraDebit { get; set; }
        //district delivered water price
        public double WaterPrice { get; set; }
        public string WaterPriceUnit { get; set; }
        public double DistributionUniformity { get; set; }
        public double SeasonWaterApplied { get; set; }
        public double WaterCost { get; set; }
        public double PumpHoursPerUnitArea { get; set; }
        
        //labor properties
        //substitute input.Times in ops and comps
        public double IrrigationTimes { get; set; }
        public double IrrigationDurationPerSet { get; set; }
        public string IrrigationDurationUnit { get; set; }
        //if irrigators irrigate other fields at same time
        public double IrrigationDurationLaborHoursPerSet { get; set; }
        //substitute oporcomp.Amount in ops and comps
        //substitute timeperiod.Amount in budgets
        public double IrrigationNetArea { get; set; }
        public string IrrigationNetAreaUnit { get; set; }
        public double EquipmentLaborAmount { get; set; }
        public double EquipmentLaborCost { get; set; }
        //repair and maintenance
        public double RepairCostsPerNetAcOrHa { get; set; }
        public double RandMPercent { get; set; }

        private const string cOptionForFuelConsumption = "OptionForFuelConsumption";
        private const string cEngineEfficiency = "EngineEfficiency";
        private const string cFuelConsumptionPerHour = "FuelConsumptionPerHour";
        private const string cWaterHP = "WaterHP";
        private const string cBrakeHP = "BrakeHP";
        private const string cFlowRate = "FlowRate";
        private const string cStaticHead = "StaticHead";
        private const string cPressureHead = "PressureHead";
        private const string cFrictionHead = "FrictionHead";
        private const string cOtherHead = "OtherHead";
        private const string cPumpCapacity = "PumpCapacity";
        private const string cPumpEfficiency = "PumpEfficiency";
        private const string cGearDriveEfficiency = "GearDriveEfficiency";
        private const string cExtraPower1 = "ExtraPower1";
        private const string cExtraPower2 = "ExtraPower2";
        private const string cEnergyExtraCostPerNetAcOrHa = "EnergyExtraCostPerNetAcOrHa";
        private const string cEnergyExtraCost = "EnergyExtraCost";
        private const string cEngineFlywheelPower = "EngineFlywheelPower";
        private const string cFuelAmountRequired = "FuelAmountRequired";
        private const string cPumpingPlantPerformance = "PumpingPlantPerformance";

        private const string cSeasonWaterNeed = "SeasonWaterNeed";
        private const string cSeasonWaterExtraCredit = "SeasonWaterExtraCredit";
        private const string cSeasonWaterExtraDebit = "SeasonWaterExtraDebit";
        private const string cWaterPrice = "WaterPrice";
        private const string cWaterPriceUnit = "WaterPriceUnit";
        private const string cDistributionUniformity = "DistributionUniformity";
        private const string cSeasonWaterApplied = "SeasonWaterApplied";
        private const string cWaterCost = "WaterCost";
        private const string cPumpHoursPerUnitArea = "PumpHoursPerUnitArea";

        private const string cIrrigationTimes = "IrrigationTimes";
        private const string cIrrigationDurationPerSet = "IrrigationDurationPerSet";
        private const string cIrrigationDurationUnit = "IrrigationDurationUnit";
        private const string cIrrigationDurationLaborHoursPerSet = "IrrigationDurationLaborHoursPerSet";
        private const string cIrrigationNetArea = "IrrigationNetArea";
        private const string cIrrigationNetAreaUnit = "IrrigationNetAreaUnit";
        private const string cEquipmentLaborAmount = "EquipmentLaborAmount";
        private const string cEquipmentLaborCost = "EquipmentLaborCost";

        private const string cRepairCostsPerNetAcOrHa = "RepairCostsPerNetAcOrHa";
        private const string cRandMPercent = "RandMPercent";

        public virtual void InitIrrigationPower1InputProperties()
        {
            this.OptionForFuelConsumption = 0;
            this.EngineEfficiency = 0;
            this.FuelConsumptionPerHour = 0;
            this.WaterHP = 0;
            this.BrakeHP = 0;
            this.FlowRate = 0;
            this.StaticHead = 0;
            this.PressureHead = 0;
            this.FrictionHead = 0;
            this.OtherHead = 0;
            this.PumpCapacity = 0;
            this.PumpEfficiency = 0;
            this.GearDriveEfficiency = 0;
            this.ExtraPower1 = 0;
            this.ExtraPower2 = 0;
            this.EnergyExtraCostPerNetAcOrHa = 0;
            this.EnergyExtraCost = 0;
            this.EngineFlywheelPower = 0;
            this.FuelAmountRequired = 0;
            this.PumpingPlantPerformance = 0;

            this.SeasonWaterNeed = 0;
            this.SeasonWaterExtraCredit = 0;
            this.SeasonWaterExtraDebit = 0;
            this.WaterPrice = 0;
            this.WaterPriceUnit = string.Empty;
            this.DistributionUniformity = 0;
            this.SeasonWaterApplied = 0;
            this.WaterCost = 0;
            this.PumpHoursPerUnitArea = 0;

            this.IrrigationTimes = 0;
            this.IrrigationDurationPerSet = 0;
            this.IrrigationDurationUnit = string.Empty;
            this.IrrigationDurationLaborHoursPerSet = 0;
            this.IrrigationNetArea = 0;
            this.IrrigationNetAreaUnit = string.Empty;
            this.EquipmentLaborAmount = 0;
            this.EquipmentLaborCost = 0;

            this.RepairCostsPerNetAcOrHa = 0;
            this.RandMPercent = 0;
        }
        public void CopyIrrigationPower1InputProperties(
            CalculatorParameters calcParameters, IrrigationPower1Input calculator)
        {
            //set the base input properties
            this.SetInputProperties(calcParameters, calculator);
            this.Local = new Local(calcParameters, calculator.Local);
            //set the constants properties
            this.Constants = new Machinery1Constant();
            this.Constants.SetMachinery1ConstantProperties(calculator.Constants);
            this.Sizes = new SizeRanges();
            this.Sizes.CopySizeRangesProperties(calculator.Sizes);
            CopyIrrigationPower1InputProperties(calculator);
        }
        private void CopyIrrigationPower1InputProperties(
             IrrigationPower1Input calculator)
         {
             this.OptionForFuelConsumption = calculator.OptionForFuelConsumption;
             this.EngineEfficiency = calculator.EngineEfficiency;
             this.FuelConsumptionPerHour = calculator.FuelConsumptionPerHour;
             this.WaterHP = calculator.WaterHP;
             this.BrakeHP = calculator.BrakeHP;
             this.FlowRate = calculator.FlowRate;
             this.StaticHead = calculator.StaticHead;
             this.PressureHead = calculator.PressureHead;
             this.FrictionHead = calculator.FrictionHead;
             this.OtherHead = calculator.OtherHead;
             this.PumpCapacity = calculator.PumpCapacity;
             this.PumpEfficiency = calculator.PumpEfficiency;
             this.GearDriveEfficiency = calculator.GearDriveEfficiency;
             this.ExtraPower1 = calculator.ExtraPower1;
             this.ExtraPower2 = calculator.ExtraPower2;
             this.EnergyExtraCostPerNetAcOrHa = calculator.EnergyExtraCostPerNetAcOrHa;
             this.EnergyExtraCost = calculator.EnergyExtraCost;
             this.EngineFlywheelPower = calculator.EngineFlywheelPower;
             this.FuelAmountRequired = calculator.FuelAmountRequired;
             this.PumpingPlantPerformance = calculator.PumpingPlantPerformance;
             
             this.SeasonWaterNeed = calculator.SeasonWaterNeed;
             this.SeasonWaterExtraCredit = calculator.SeasonWaterExtraCredit;
             this.SeasonWaterExtraDebit = calculator.SeasonWaterExtraDebit;
             this.WaterPrice = calculator.WaterPrice;
             this.WaterPriceUnit = calculator.WaterPriceUnit;
             this.DistributionUniformity = calculator.DistributionUniformity;
             this.SeasonWaterApplied = calculator.SeasonWaterApplied;
             this.WaterCost = calculator.WaterCost;
             this.PumpHoursPerUnitArea = calculator.PumpHoursPerUnitArea;

             this.IrrigationTimes = calculator.IrrigationTimes;
             this.IrrigationDurationPerSet = calculator.IrrigationDurationPerSet;
             this.IrrigationDurationUnit = calculator.IrrigationDurationUnit;
             this.IrrigationDurationLaborHoursPerSet = calculator.IrrigationDurationLaborHoursPerSet;
             this.IrrigationNetArea = calculator.IrrigationNetArea;
             this.IrrigationNetAreaUnit = calculator.IrrigationNetAreaUnit;
             this.EquipmentLaborAmount = calculator.EquipmentLaborAmount;
             this.EquipmentLaborCost = calculator.EquipmentLaborCost;

             this.RepairCostsPerNetAcOrHa = calculator.RepairCostsPerNetAcOrHa;
             this.RandMPercent = calculator.RandMPercent;
         }
        public virtual void SetIrrigationPower1InputProperties(CalculatorParameters calcParameters,
            XElement calculator, XElement currentElement)
        {
            //set the base input properties
            SetMachinery1InputProperties(calcParameters,
                calculator, currentElement);
            //set this object's properties
            SetIrrigationPower1InputProperties(calculator);
        }
        public virtual void SetIrrigationPower1InputProperties(XElement calculator)
        {
            this.OptionForFuelConsumption = CalculatorHelpers.GetAttributeInt(calculator,
                cOptionForFuelConsumption);
            this.EngineEfficiency = CalculatorHelpers.GetAttributeDouble(calculator,
                cEngineEfficiency);
            this.FuelConsumptionPerHour = CalculatorHelpers.GetAttributeDouble(calculator,
                cFuelConsumptionPerHour);
            this.WaterHP = CalculatorHelpers.GetAttributeDouble(calculator,
                cWaterHP);
            this.BrakeHP = CalculatorHelpers.GetAttributeDouble(calculator,
               cBrakeHP);
            this.FlowRate = CalculatorHelpers.GetAttributeDouble(calculator,
               cFlowRate);
            this.StaticHead = CalculatorHelpers.GetAttributeDouble(calculator,
               cStaticHead);
            this.PressureHead = CalculatorHelpers.GetAttributeDouble(calculator,
               cPressureHead);
            this.FrictionHead = CalculatorHelpers.GetAttributeDouble(calculator,
               cFrictionHead);
            this.OtherHead = CalculatorHelpers.GetAttributeDouble(calculator,
               cOtherHead);
            this.PumpCapacity = CalculatorHelpers.GetAttributeDouble(calculator,
               cPumpCapacity);
            this.PumpEfficiency = CalculatorHelpers.GetAttributeDouble(calculator,
               cPumpEfficiency);
            this.GearDriveEfficiency = CalculatorHelpers.GetAttributeDouble(calculator,
               cGearDriveEfficiency);
            this.ExtraPower1 = CalculatorHelpers.GetAttributeDouble(calculator,
               cExtraPower1);
            this.ExtraPower2 = CalculatorHelpers.GetAttributeDouble(calculator,
               cExtraPower2);
            this.EnergyExtraCostPerNetAcOrHa = CalculatorHelpers.GetAttributeDouble(calculator,
               cEnergyExtraCostPerNetAcOrHa);
            this.EnergyExtraCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cEnergyExtraCost);
            this.EngineFlywheelPower = CalculatorHelpers.GetAttributeDouble(calculator,
               cEngineFlywheelPower);
            this.FuelAmountRequired = CalculatorHelpers.GetAttributeDouble(calculator,
               cFuelAmountRequired);
            this.PumpingPlantPerformance = CalculatorHelpers.GetAttributeDouble(calculator,
               cPumpingPlantPerformance);

            this.SeasonWaterNeed = CalculatorHelpers.GetAttributeDouble(calculator,
               cSeasonWaterNeed);
            this.SeasonWaterExtraCredit = CalculatorHelpers.GetAttributeDouble(calculator,
               cSeasonWaterExtraCredit);
            this.SeasonWaterExtraDebit = CalculatorHelpers.GetAttributeDouble(calculator,
               cSeasonWaterExtraDebit);
            this.WaterPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               cWaterPrice);
            this.WaterPriceUnit = CalculatorHelpers.GetAttribute(calculator,
               cWaterPriceUnit);
            this.DistributionUniformity = CalculatorHelpers.GetAttributeDouble(calculator,
               cDistributionUniformity);
            this.SeasonWaterApplied = CalculatorHelpers.GetAttributeDouble(calculator,
               cSeasonWaterApplied);
            this.WaterCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cWaterCost);
            this.PumpHoursPerUnitArea = CalculatorHelpers.GetAttributeDouble(calculator,
               cPumpHoursPerUnitArea);

            this.IrrigationTimes = CalculatorHelpers.GetAttributeDouble(calculator,
               cIrrigationTimes);
            this.IrrigationDurationPerSet = CalculatorHelpers.GetAttributeDouble(calculator,
               cIrrigationDurationPerSet);
            this.IrrigationDurationUnit = CalculatorHelpers.GetAttribute(calculator,
               cIrrigationDurationUnit);
            this.IrrigationDurationLaborHoursPerSet = CalculatorHelpers.GetAttributeDouble(calculator,
               cIrrigationDurationLaborHoursPerSet);
            this.IrrigationNetArea = CalculatorHelpers.GetAttributeDouble(calculator,
               cIrrigationNetArea);
            this.IrrigationNetAreaUnit = CalculatorHelpers.GetAttribute(calculator,
               cIrrigationNetAreaUnit);
            this.EquipmentLaborAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               cEquipmentLaborAmount);
            this.EquipmentLaborCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cEquipmentLaborCost);

            this.RepairCostsPerNetAcOrHa = CalculatorHelpers.GetAttributeDouble(calculator,
               cRepairCostsPerNetAcOrHa);
            this.RandMPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               cRandMPercent);
        }
        
        public virtual void SetIrrigationPower1InputProperty(string attName,
           string attValue)
        {
            switch (attName)
            {
                case cOptionForFuelConsumption:
                    this.OptionForFuelConsumption = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cEngineEfficiency:
                    this.EngineEfficiency = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFuelConsumptionPerHour:
                    this.FuelConsumptionPerHour = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWaterHP:
                    this.WaterHP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cBrakeHP:
                    this.BrakeHP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFlowRate:
                    this.FlowRate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cStaticHead:
                    this.StaticHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPressureHead:
                    this.PressureHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFrictionHead:
                    this.FrictionHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOtherHead:
                    this.OtherHead = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPumpCapacity:
                    this.PumpCapacity = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPumpEfficiency:
                    this.PumpEfficiency = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cGearDriveEfficiency:
                    this.GearDriveEfficiency = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cExtraPower1:
                    this.ExtraPower1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cExtraPower2:
                    this.ExtraPower2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEnergyExtraCostPerNetAcOrHa:
                    this.EnergyExtraCostPerNetAcOrHa = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEnergyExtraCost:
                    this.EnergyExtraCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEngineFlywheelPower:
                    this.EngineFlywheelPower = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFuelAmountRequired:
                    this.FuelAmountRequired = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPumpingPlantPerformance:
                    this.PumpingPlantPerformance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSeasonWaterNeed:
                    this.SeasonWaterNeed = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSeasonWaterExtraCredit:
                    this.SeasonWaterExtraCredit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSeasonWaterExtraDebit:
                    this.SeasonWaterExtraDebit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWaterPrice:
                    this.WaterPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWaterPriceUnit:
                    this.WaterPriceUnit = attValue;
                    break;
                case cDistributionUniformity:
                    this.DistributionUniformity = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSeasonWaterApplied:
                    this.SeasonWaterApplied = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWaterCost:
                    this.WaterCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPumpHoursPerUnitArea:
                    this.PumpHoursPerUnitArea = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIrrigationTimes:
                    this.IrrigationTimes = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIrrigationDurationPerSet:
                    this.IrrigationDurationPerSet = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIrrigationDurationUnit:
                    this.IrrigationDurationUnit = attValue;
                    break;
                case cIrrigationDurationLaborHoursPerSet:
                    this.IrrigationDurationLaborHoursPerSet = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIrrigationNetArea:
                    this.IrrigationNetArea = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIrrigationNetAreaUnit:
                    this.IrrigationNetAreaUnit = attValue;
                    break;
                case cEquipmentLaborAmount:
                    this.EquipmentLaborAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEquipmentLaborCost:
                    this.EquipmentLaborCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRepairCostsPerNetAcOrHa:
                    this.RepairCostsPerNetAcOrHa = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cRandMPercent:
                    this.RandMPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetIrrigationPower1InputProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cOptionForFuelConsumption:
                    sPropertyValue = this.OptionForFuelConsumption.ToString();
                    break;
                case cEngineEfficiency:
                    sPropertyValue = this.EngineEfficiency.ToString();
                    break;
                case cFuelConsumptionPerHour:
                    sPropertyValue = this.FuelConsumptionPerHour.ToString();
                    break;
                case cWaterHP:
                    sPropertyValue = this.WaterHP.ToString();
                    break;
                case cBrakeHP:
                    sPropertyValue = this.BrakeHP.ToString();
                    break;
                case cFlowRate:
                    sPropertyValue = this.FlowRate.ToString();
                    break;
                case cStaticHead:
                    sPropertyValue = this.StaticHead.ToString();
                    break;
                case cPressureHead:
                    sPropertyValue = this.PressureHead.ToString();
                    break;
                case cFrictionHead:
                    sPropertyValue = this.FrictionHead.ToString();
                    break;
                case cOtherHead:
                    sPropertyValue = this.OtherHead.ToString();
                    break;
                case cPumpCapacity:
                    sPropertyValue = this.PumpCapacity.ToString();
                    break;
                case cPumpEfficiency:
                    sPropertyValue = this.PumpEfficiency.ToString();
                    break;
                case cGearDriveEfficiency:
                    sPropertyValue = this.GearDriveEfficiency.ToString();
                    break;
                case cExtraPower1:
                    sPropertyValue = this.ExtraPower1.ToString();
                    break;
                case cExtraPower2:
                    sPropertyValue = this.ExtraPower2.ToString();
                    break;
                case cEnergyExtraCostPerNetAcOrHa:
                    sPropertyValue = this.EnergyExtraCostPerNetAcOrHa.ToString();
                    break;
                case cEnergyExtraCost:
                    sPropertyValue = this.EnergyExtraCost.ToString();
                    break;
                case cEngineFlywheelPower:
                    sPropertyValue = this.EngineFlywheelPower.ToString();
                    break;
                case cFuelAmountRequired:
                    sPropertyValue = this.FuelAmountRequired.ToString();
                    break;
                case cPumpingPlantPerformance:
                    sPropertyValue = this.PumpingPlantPerformance.ToString();
                    break;
                case cSeasonWaterNeed:
                    sPropertyValue = this.SeasonWaterNeed.ToString();
                    break;
                case cSeasonWaterExtraCredit:
                    sPropertyValue = this.SeasonWaterExtraCredit.ToString();
                    break;
                case cSeasonWaterExtraDebit:
                    sPropertyValue = this.SeasonWaterExtraDebit.ToString();
                    break;
                case cWaterPrice:
                    sPropertyValue = this.WaterPrice.ToString();
                    break;
                case cWaterPriceUnit:
                    sPropertyValue = this.WaterPriceUnit.ToString();
                    break;
                case cDistributionUniformity:
                    sPropertyValue = this.DistributionUniformity.ToString();
                    break;
                case cSeasonWaterApplied:
                    sPropertyValue = this.SeasonWaterApplied.ToString();
                    break;
                case cWaterCost:
                    sPropertyValue = this.WaterCost.ToString();
                    break;
                case cPumpHoursPerUnitArea:
                    sPropertyValue = this.PumpHoursPerUnitArea.ToString();
                    break;
                case cIrrigationTimes:
                    sPropertyValue = this.IrrigationTimes.ToString();
                    break;
                case cIrrigationDurationPerSet:
                    sPropertyValue = this.IrrigationDurationPerSet.ToString();
                    break;
                case cIrrigationDurationUnit:
                    sPropertyValue = this.IrrigationDurationUnit.ToString();
                    break;
                case cIrrigationDurationLaborHoursPerSet:
                    sPropertyValue = this.IrrigationDurationLaborHoursPerSet.ToString();
                    break;
                case cIrrigationNetArea:
                    sPropertyValue = this.IrrigationNetArea.ToString();
                    break;
                case cIrrigationNetAreaUnit:
                    sPropertyValue = this.IrrigationNetAreaUnit;
                    break;
                case cEquipmentLaborAmount:
                    sPropertyValue = this.EquipmentLaborAmount.ToString();
                    break;
                case cEquipmentLaborCost:
                    sPropertyValue = this.EquipmentLaborCost.ToString();
                    break;
                case cRepairCostsPerNetAcOrHa:
                    sPropertyValue = this.RepairCostsPerNetAcOrHa.ToString();
                    break;
                case cRandMPercent:
                    sPropertyValue = this.RandMPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetIrrigationPower1Attributes(CalculatorParameters calcParameters,
            ref XElement calculator, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //set the base attributes
            SetMachinery1InputAttributes(calcParameters,
                ref calculator, ref currentElement,
                updates);
            string sAttNameExtension = string.Empty;
            SetIrrigationPower1InputAttributes(sAttNameExtension,
                ref calculator);
        }
        
        public virtual void SetIrrigationPower1Attributes(CalculatorParameters calcParameters,
            ref XElement calculator, ref XElement currentElement)
        {
            //set the base attributes
            SetMachinery1InputAttributes(calcParameters,
                ref calculator, ref currentElement);
            string sAttNameExtension = string.Empty;
            SetIrrigationPower1InputAttributes(sAttNameExtension,
                ref calculator);
        }
        public virtual void SetIrrigationPower1InputAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeInt(calculator,
                cOptionForFuelConsumption, this.OptionForFuelConsumption);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cEngineEfficiency, this.EngineEfficiency);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cFuelConsumptionPerHour, this.FuelConsumptionPerHour);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cWaterHP, this.WaterHP);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cBrakeHP, this.BrakeHP);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cFlowRate, this.FlowRate);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cStaticHead, this.StaticHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cPressureHead, this.PressureHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cFrictionHead, this.FrictionHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cOtherHead, this.OtherHead);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cPumpCapacity, this.PumpCapacity);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cPumpEfficiency, this.PumpEfficiency);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cGearDriveEfficiency, this.GearDriveEfficiency);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cExtraPower1, this.ExtraPower1);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cExtraPower2, this.ExtraPower2);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cEnergyExtraCostPerNetAcOrHa, this.EnergyExtraCostPerNetAcOrHa);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cEnergyExtraCost, this.EnergyExtraCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cEngineFlywheelPower, this.EngineFlywheelPower);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cFuelAmountRequired, this.FuelAmountRequired);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cPumpingPlantPerformance, this.PumpingPlantPerformance);
            
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cSeasonWaterNeed, this.SeasonWaterNeed);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cSeasonWaterExtraCredit, this.SeasonWaterExtraCredit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cSeasonWaterExtraDebit, this.SeasonWaterExtraDebit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cWaterPrice, this.WaterPrice);
            CalculatorHelpers.SetAttribute(calculator,
                cWaterPriceUnit, this.WaterPriceUnit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cDistributionUniformity, this.DistributionUniformity);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cSeasonWaterApplied, this.SeasonWaterApplied);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cWaterCost, this.WaterCost);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cPumpHoursPerUnitArea, this.PumpHoursPerUnitArea);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cIrrigationTimes, this.IrrigationTimes);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cIrrigationDurationPerSet, this.IrrigationDurationPerSet);
            CalculatorHelpers.SetAttribute(calculator,
                cIrrigationDurationUnit, this.IrrigationDurationUnit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cIrrigationDurationLaborHoursPerSet, this.IrrigationDurationLaborHoursPerSet);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cIrrigationNetArea, this.IrrigationNetArea);
            CalculatorHelpers.SetAttribute(calculator,
                cIrrigationNetAreaUnit, this.IrrigationNetAreaUnit);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cEquipmentLaborAmount, this.EquipmentLaborAmount);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cEquipmentLaborCost, this.EquipmentLaborCost);

            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cRepairCostsPerNetAcOrHa, this.RepairCostsPerNetAcOrHa);
            CalculatorHelpers.SetAttributeDoubleF4(calculator,
                cRandMPercent, this.RandMPercent);
        }
        public static void SetIrrigationPowerProperties(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            //imperial constants (pressure to feet pressure head)
            double dbPSIConstant = 2.31;
            int iWHPConstant = 3960;
            double dbTotalDynamicPumpingHead = 0;
            if (unitType != GeneralRules.UNIT_TYPES.imperial)
            {
                //comes from Caterpillar (note that Cat uses L/s, so keep it uniform)
                //pressure to meters pressure head
                dbPSIConstant = .102;
                //metric constant for l/s (367 for m/s)
                iWHPConstant = 102;
            }
            //step 1. calculate water horsepower 
            //(Cat) (note: friction head entered as 1 ft head per 100 foot pipe run)
            dbTotalDynamicPumpingHead = irrigationPowerInput.StaticHead
                + (irrigationPowerInput.PressureHead * dbPSIConstant) + irrigationPowerInput.FrictionHead
                + irrigationPowerInput.OtherHead;
            irrigationPowerInput.WaterHP = irrigationPowerInput.FlowRate * (dbTotalDynamicPumpingHead / iWHPConstant);
            //Step 2. calculate brakehp
            irrigationPowerInput.BrakeHP = irrigationPowerInput.WaterHP 
                / ((irrigationPowerInput.PumpEfficiency / 100) * (irrigationPowerInput.GearDriveEfficiency/ 100));
            //Step 3. calculate total hp
            irrigationPowerInput.EngineFlywheelPower = irrigationPowerInput.BrakeHP
                + irrigationPowerInput.ExtraPower1 + irrigationPowerInput.ExtraPower2;
            //Step 4. calculate pump output in ac/in/hr or m3 per hour
            //1.77 acin/hr = 800 gpm / 450 acin/hour
            irrigationPowerInput.PumpCapacity = irrigationPowerInput.FlowRate / GPMPerHourToAcInHr;
            if (irrigationPowerInput.PumpCapacity == 0)
            {
                irrigationPowerInput.PumpCapacity = -1;
            }
            if (unitType != GeneralRules.UNIT_TYPES.imperial)
            {
                //use cubic meters for volume of irrigation water
                //222.4 cubic meter per hour = 0.278 * 800 l/s 
                double dbFlowRateInCubicM3PerHour = LitersPerSecondToCubicMperHour * irrigationPowerInput.FlowRate;
                irrigationPowerInput.PumpCapacity = dbFlowRateInCubicM3PerHour;
            }
        }
        public static void SetIrrigationPowerFuelProperties(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            IrrigationPower1Input.FUEL_CONSUMPTION_TYPES eFuelConsumptionType
                = IrrigationPower1Input.GetFuelConsumptionType(irrigationPowerInput.OptionForFuelConsumption);
            if (eFuelConsumptionType == IrrigationPower1Input.FUEL_CONSUMPTION_TYPES.energyuse)
            {
                //based on total energy use
                SetFuelCostsPerVolumeWaterForFlyWheelEnergy(unitType, irrigationPowerInput);
            }
            else if (eFuelConsumptionType == IrrigationPower1Input.FUEL_CONSUMPTION_TYPES.nppc)
            {
                //based on whp and NPPC
                SetFuelCostsForWaterHP(unitType, irrigationPowerInput);
            }
        }
        public static void SetFuelCostsForWaterHP(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            double dbFuelAmount = 0;
            string sFuelUnit = string.Empty;
            //get fuel unit
            double dbEnergyMultiplier = GeneralRules.GetEnergyMultiplier(unitType,
                irrigationPowerInput.Constants.FuelType, out sFuelUnit);
            //this is amount of fuel consumed per hour of use
            if (unitType == GeneralRules.UNIT_TYPES.imperial)
            {
                //the multipliers are imperial units
                //23 gallon diesel per hour = WHP(150 lift + (2.31 * 60 psi) / 12.5 whp-hr/gallon diesel
                dbFuelAmount = irrigationPowerInput.WaterHP / dbEnergyMultiplier;
            }
            else
            {
                //convert metric energy use hr (in kW) to imperial so imperial energy multipliers can be used
                //1 kw = 1.341 hp
                double dbHorsepowerUsePerHr = irrigationPowerInput.WaterHP / GeneralRules.KwToHp;
                double dbHPFuelAmount = dbHorsepowerUsePerHr / dbEnergyMultiplier;
                //convert any imperial units (gallons gas, diesel and lpg) to liters
                //this is the total fuel per hour used
                dbFuelAmount = GetMetricFuelAmount(dbHPFuelAmount, irrigationPowerInput);
            }
            if (unitType == GeneralRules.UNIT_TYPES.imperial)
            {
                //convert required fuel amount per hour to required fuel amount per volume water applied
                //see Martin et al (table 2)
                //2.63 gallon per acin = 23 gallon per hour / PumpCapacity (acin / hour)
                irrigationPowerInput.FuelAmountRequired = dbFuelAmount / irrigationPowerInput.PumpCapacity;
                //calculate actual fuel amount per acin 
                //(3.11 gallon diesel per acin = 5.5 gallons diesel actually used per hour / 1.77 acin-hr)
                //see Martin et al (top of page 111)
                double dbFuelAmountActuallyUsedPerHr = (irrigationPowerInput.FuelConsumptionPerHour > 0)
                    ? irrigationPowerInput.FuelConsumptionPerHour : dbFuelAmount;
                double dbFuelAmountActualPerAcreInch
                    = dbFuelAmountActuallyUsedPerHr / irrigationPowerInput.PumpCapacity;
                irrigationPowerInput.FuelAmount = dbFuelAmountActualPerAcreInch;
                //calculate pump plant efficiency
                irrigationPowerInput.PumpingPlantPerformance 
                    = (100 * irrigationPowerInput.FuelAmountRequired) / dbFuelAmountActualPerAcreInch;
                //3.11 gallons per acin
                irrigationPowerInput.FuelUnit = sFuelUnit;
                irrigationPowerInput.FuelCost = GetFuelCostPerUnit(dbFuelAmountActualPerAcreInch, irrigationPowerInput);
            }
            else
            {
                //10 liters diesel per m3 water = 20 liters diesel per hour / 2 m3 water applied per hour
                irrigationPowerInput.FuelAmountRequired = dbFuelAmount / irrigationPowerInput.PumpCapacity;
                //calculate actual fuel amount per m3
                double dbFuelAmountActuallyUsedPerHr = (irrigationPowerInput.FuelConsumptionPerHour > 0)
                    ? irrigationPowerInput.FuelConsumptionPerHour : dbFuelAmount;
                double dbFuelAmountActualPerCubicMeter
                    = dbFuelAmountActuallyUsedPerHr / irrigationPowerInput.PumpCapacity;
                irrigationPowerInput.FuelAmount = dbFuelAmountActualPerCubicMeter;
                //calculate pump plant efficiency
                irrigationPowerInput.PumpingPlantPerformance
                    = (100 * irrigationPowerInput.FuelAmountRequired) / dbFuelAmountActualPerCubicMeter;
                //3.11 gallons per acin
                irrigationPowerInput.FuelUnit = sFuelUnit;
                irrigationPowerInput.FuelCost = GetFuelCostPerUnit(dbFuelAmountActualPerCubicMeter, irrigationPowerInput);
            }
        }
        public static void SetFuelCostsPerVolumeWaterForFlyWheelEnergy(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            string sFuelUnit = string.Empty;
            //get energy btus
            double dbBTUs = GeneralRules.GetEnergyBTUs(unitType,
                irrigationPowerInput.Constants.FuelType, out sFuelUnit);
            //entered as whole number (95)
            if (irrigationPowerInput.EngineEfficiency == 0)
            {
                //error msg is negative number
                irrigationPowerInput.EngineEfficiency = -1;
            }
            //calculate using imperial units so standard imperial formulas and constants can be used
            double dbHorsepowerUsePerHr = irrigationPowerInput.EngineFlywheelPower;
            if (unitType != GeneralRules.UNIT_TYPES.imperial)
            {
                //convert metric energy use hr (in kW) to imperial hp
                //1 kw = 1.341 hp
                dbHorsepowerUsePerHr = irrigationPowerInput.EngineFlywheelPower / GeneralRules.KwToHp;
            }
            //calculate required energy use per acre inch of water applied (Guerrero et al, Hallam et al)
            //i.e. kw /ac inch = HP * 2545BTU/HP-HR * kw/dbBTUs(BTUs) * 1/EngineEfficiency * 450/GPM 
            double dbFuelAmountPerAcreInch = dbHorsepowerUsePerHr * (2545 / dbBTUs)
                * (1 / (irrigationPowerInput.EngineEfficiency / 100)) * (GPMPerHourToAcInHr / irrigationPowerInput.FlowRate);
            irrigationPowerInput.FuelAmountRequired = dbFuelAmountPerAcreInch;
            if (unitType != GeneralRules.UNIT_TYPES.imperial)
            {
                //convert any gallons (gas, diesel and lpg) to liters
                double dbMetricFuelAmountRequiredPerAcIn = GetMetricFuelAmount(dbFuelAmountPerAcreInch, irrigationPowerInput);
                //convert from liters per acre inch to liters per m3
                //5 liters per m3 = 20 liters used per ac in / 102.8 cubic m3 per ac in
                irrigationPowerInput.FuelAmountRequired = dbMetricFuelAmountRequiredPerAcIn / AcInchToCubicMeters;
            }
            //calculate fuel using amount actually used
            if (irrigationPowerInput.FuelConsumptionPerHour > 0)
            {
                double dbFuelAmountActuallyUsedPerHr = (irrigationPowerInput.FuelConsumptionPerHour > 0)
                    ? irrigationPowerInput.FuelConsumptionPerHour : irrigationPowerInput.FuelAmountRequired;
                double dbFuelAmountActualPerUnitWater
                    = dbFuelAmountActuallyUsedPerHr / irrigationPowerInput.PumpCapacity;
                irrigationPowerInput.FuelAmount = dbFuelAmountActualPerUnitWater;
            }
            else
            {
                irrigationPowerInput.FuelAmount = irrigationPowerInput.FuelAmountRequired;
            }
            //calculate pump plant efficiency
            irrigationPowerInput.PumpingPlantPerformance
                = (100 * irrigationPowerInput.FuelAmountRequired) / irrigationPowerInput.FuelAmount;
            //3.11 gallons (liters) per acin (per m3)
            irrigationPowerInput.FuelUnit = sFuelUnit;
            irrigationPowerInput.FuelCost = GetFuelCostPerUnit(irrigationPowerInput.FuelAmount, irrigationPowerInput);
        }
        
        public static double GetMetricFuelAmount(double fuelAmountPerUnit,
            IrrigationPower1Input irrigationPowerInput)
        {
            double dbMetricFuelAmount = 0;
            GeneralRules.FUEL_PRICE_TYPES eFuelPriceType = GeneralRules.GetFuelPriceType(irrigationPowerInput.Constants.FuelType);
            if (eFuelPriceType ==  GeneralRules.FUEL_PRICE_TYPES.gas
                || eFuelPriceType ==  GeneralRules.FUEL_PRICE_TYPES.diesel
                || eFuelPriceType ==  GeneralRules.FUEL_PRICE_TYPES.lpg
                || eFuelPriceType ==  GeneralRules.FUEL_PRICE_TYPES.ethanol
                || eFuelPriceType ==  GeneralRules.FUEL_PRICE_TYPES.gasohol)
            {
                dbMetricFuelAmount = fuelAmountPerUnit * GeneralRules.GallonToLiter;
            }
            return dbMetricFuelAmount;
        }
        public static double GetFuelCostPerUnit(double fuelAmountPerUnit,
            IrrigationPower1Input irrigationPowerInput)
        {
            double dbFuelCostPerUnit = 0;
            GeneralRules.FUEL_PRICE_TYPES eFuelPriceType = GeneralRules.GetFuelPriceType(irrigationPowerInput.Constants.FuelType);
            switch (eFuelPriceType)
            {
                case GeneralRules.FUEL_PRICE_TYPES.gas:
                    dbFuelCostPerUnit = fuelAmountPerUnit * irrigationPowerInput.Constants.PriceGas;
                    break;
                case GeneralRules.FUEL_PRICE_TYPES.diesel:
                    dbFuelCostPerUnit = fuelAmountPerUnit * irrigationPowerInput.Constants.PriceDiesel;
                    break;
                case GeneralRules.FUEL_PRICE_TYPES.lpg:
                    dbFuelCostPerUnit = fuelAmountPerUnit * irrigationPowerInput.Constants.PriceLP;
                    break;
                case GeneralRules.FUEL_PRICE_TYPES.electric:
                    dbFuelCostPerUnit = fuelAmountPerUnit * irrigationPowerInput.Constants.PriceElectric;
                    break;
                case GeneralRules.FUEL_PRICE_TYPES.naturalgas:
                    dbFuelCostPerUnit = fuelAmountPerUnit * irrigationPowerInput.Constants.PriceNG;
                    break;
                default:
                    fuelAmountPerUnit = 0;
                    dbFuelCostPerUnit = 0;
                    break;
            }
            return dbFuelCostPerUnit;
        }
        public static void SetExtraEnergyProperties(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            irrigationPowerInput.EnergyExtraCost 
                = irrigationPowerInput.EnergyExtraCostPerNetAcOrHa / irrigationPowerInput.SeasonWaterApplied;
        }
        public static void SetIrrigationWaterProperties(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            //must be acin or m3 per acre or per hectare
            irrigationPowerInput.WaterPriceUnit = "acre inch";
            if (unitType != GeneralRules.UNIT_TYPES.imperial)
            {
                irrigationPowerInput.WaterPriceUnit = "liters/sec";
            }
            //note that ocamount is now (beta 0.9.0) irrigationPowerInput.SeasonWaterApplied (not servicecapacity)
            //= acin needed - (rainfall) + (leaching) / DU
            irrigationPowerInput.SeasonWaterApplied = (irrigationPowerInput.SeasonWaterNeed 
                - irrigationPowerInput.SeasonWaterExtraCredit + irrigationPowerInput.SeasonWaterExtraDebit) 
                / (irrigationPowerInput.DistributionUniformity / 100);
            if (irrigationPowerInput.SeasonWaterApplied == 0)
            {
                irrigationPowerInput.SeasonWaterApplied = -1;
            }
            double dbTotalWaterCostPerAcre
               = irrigationPowerInput.SeasonWaterApplied * irrigationPowerInput.WaterPrice;
            //water cost per acin or m3: $2 per acin = $50 per acre / 25 acin
            irrigationPowerInput.WaterCost
                = dbTotalWaterCostPerAcre / irrigationPowerInput.SeasonWaterApplied;
            //service capacity is total acre in applied (ocamount)
            irrigationPowerInput.Constants.ServiceCapacity = irrigationPowerInput.SeasonWaterApplied;
            //hours of pump use per season (can be used to calc more exact hours of pump use)
            //25 hours per acre per year = 50 acin applied / 2 acin per hour;
            irrigationPowerInput.PumpHoursPerUnitArea
                = irrigationPowerInput.SeasonWaterApplied / irrigationPowerInput.PumpCapacity;
        }
        public static void SetIrrigationLaborProperties(GeneralRules.UNIT_TYPES unitType,
            IrrigationPower1Input irrigationPowerInput)
        {
            double dbLaborAmount = 0;
            string sLaborUnit = string.Empty;
            //these are kept per field, not per acre
            double dbTotalSeasonLaborHours = irrigationPowerInput.IrrigationTimes 
                * irrigationPowerInput.IrrigationDurationPerSet * irrigationPowerInput.IrrigationDurationLaborHoursPerSet;
            //convert to per area (ac or ha) labor
            dbLaborAmount = dbTotalSeasonLaborHours / irrigationPowerInput.IrrigationNetArea;
            //now convert to OC amount (per acin or m3) compatible number
            irrigationPowerInput.LaborAmount = dbLaborAmount / irrigationPowerInput.SeasonWaterApplied;
            irrigationPowerInput.LaborPrice = AgBudgetingRules.GetLaborPrice(irrigationPowerInput);
            double dbLaborCostPerAcOrHa = dbLaborAmount * irrigationPowerInput.LaborPrice;
            //$2 labor cost per acin = $50 labor cost per acre / 25 acin per season 
            double dbLaborCostPerVolumeWater = dbLaborCostPerAcOrHa / irrigationPowerInput.SeasonWaterApplied;
            //irrigation labor costs per volume of water
            irrigationPowerInput.LaborCost = dbLaborCostPerVolumeWater;
            //regular cost per acre costs
            irrigationPowerInput.LaborUnit = "hour";
            //irrigation equipment labor costs (setting up pump, transport to and from field)
            double dbLaborPrice = GeneralRules.GetLaborPrice(GeneralRules.LABOR_PRICE_TYPES.machinery.ToString(),
                irrigationPowerInput.Constants.PriceRegularLabor, irrigationPowerInput.Constants.PriceMachineryLabor,
                irrigationPowerInput.Constants.PriceSupervisorLabor);
            //same per acin or m3 amounts and costs
            irrigationPowerInput.EquipmentLaborAmount = irrigationPowerInput.LaborAmount * (irrigationPowerInput.LaborAmountAdj / 100);
            irrigationPowerInput.EquipmentLaborCost = irrigationPowerInput.EquipmentLaborAmount * dbLaborPrice;
        }
        public static void SetIrrigationRepairCostPerVolumeWater(IrrigationPower1Input irrigationPowerInput)
        {
            double dbRepairCostPerVolumeWater = 0;
            double dbRepairCostPerAcOrHa = 0;
            //preference is always to use repair bills
            if (irrigationPowerInput.RepairCostsPerNetAcOrHa > 0)
            {
                //$2 acin = $50 repair cost per irrigated acre per year / 25 acin per season 
                dbRepairCostPerVolumeWater = irrigationPowerInput.RepairCostsPerNetAcOrHa 
                    / irrigationPowerInput.SeasonWaterApplied;
            }
            else
            {
                if (irrigationPowerInput.RandMPercent > 0)
                {
                    dbRepairCostPerAcOrHa = (irrigationPowerInput.MarketValue 
                        / (irrigationPowerInput.RandMPercent / 100) / irrigationPowerInput.IrrigationNetArea);
                    dbRepairCostPerVolumeWater = dbRepairCostPerAcOrHa 
                        / irrigationPowerInput.SeasonWaterApplied;
                }
            }
            irrigationPowerInput.RepairCost = dbRepairCostPerVolumeWater;
        }
        public static void SetIrrigationLubeCostPerVolumeWater(GeneralRules.UNIT_TYPES unitType, 
            IrrigationPower1Input irrigationPowerInput)
        {
            double dbLubeOilHr = 0;
            string sLubeOilUnit = string.Empty;
            double dbLubeCostHr = 0;
            GeneralRules.IrrLubeOilCostHr(unitType, irrigationPowerInput.Constants.FuelType,
                    irrigationPowerInput.WaterHP, irrigationPowerInput.Constants.PriceOil,
                    out dbLubeOilHr, out sLubeOilUnit, out dbLubeCostHr);
            //1 gal per acin = 2 gal per hour / 2 acin per hr
            irrigationPowerInput.LubeOilAmount = dbLubeOilHr / irrigationPowerInput.PumpCapacity;
            irrigationPowerInput.LubeOilUnit = sLubeOilUnit;
            irrigationPowerInput.LubeOilPrice = irrigationPowerInput.Constants.PriceOil;
            irrigationPowerInput.LubeOilCost = irrigationPowerInput.LubeOilAmount * irrigationPowerInput.LubeOilPrice;
        }
    }
}
