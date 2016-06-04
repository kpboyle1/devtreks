using System.Collections.Generic;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Calculate machinery, and other capital, input costs
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    //NOTES         1. Carry out calculations by deserializing currentCalculationsElement 
    //              and currentElement into an AddInViews.BaseObject and using the object
    //              to run the calculations
    //              2. Serialize the object's new calculations back to 
    //              currentCalculationsElement and currentElement, and fill in 
    //              the updates collection with any db fields that have changed
    /// </summary>      
    public class Machinery1InputCalculator
    {
        private const string HOUR = "hour";
        private const string EACH = "each";
        private const string ACRE = "acre";
        private const string HECTARE = "hectare";
        private const string HOURS_PER_HECTARE = "hours/hectare";
        private const string HOURS_PER_ACRE = "hours/acre";
        private const string HOURS_PER_METRIC_TON = "hours/metric ton";
        private const string HOURS_PER_TON = "hours/ton";
        private const string METRIC_TONS_PER_HOUR = "metric tons/hour";
        private const string TONS_PER_HOUR = "tons/hour";
        
        public static bool SetMachinery1Calculations(
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement, 
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            string sErrorMessage = string.Empty;
            switch (calculatorType)
            {
                case AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery:
                    Machinery1Input machineryInput = new Machinery1Input();
                    //deserialize xml to object
                    machineryInput.SetMachinery1InputProperties(calcParameters, 
                        currentCalculationsElement, currentElement);
                    bHasCalculations = SetAgMachineryCalculations(calcParameters, 
                        calculatorType, machineryInput, currentCalculationsElement,
                        ref sErrorMessage);
                    //serialize object back to xml and fill in updates list
                    machineryInput.SetMachinery1InputAttributes(
                        calcParameters, ref currentCalculationsElement, 
                        ref currentElement, updates);
                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.irrpower:
                    IrrigationPower1Input irrigationPowerInput = new IrrigationPower1Input();
                    irrigationPowerInput.SetIrrigationPower1InputProperties(calcParameters,
                        currentCalculationsElement, currentElement);
                    bHasCalculations = SetIrrPowerCalculations(calcParameters,
                        calculatorType, irrigationPowerInput, ref sErrorMessage);
                    //serialize the new calculations back into currentCalculationsElement
                    irrigationPowerInput.SetIrrigationPower1Attributes(
                        calcParameters, ref currentCalculationsElement,
                        ref currentElement, updates);
                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.gencapital:
                    GeneralCapital1Input capitalInput = new GeneralCapital1Input();
                    capitalInput.SetGeneralCapital1InputProperties(calcParameters,
                        currentCalculationsElement, currentElement);
                    bHasCalculations = SetGenCapitalCalculations(calcParameters,
                        calculatorType, capitalInput, ref sErrorMessage);
                    //serialize the new calculations back into currentCalculationsElement
                    capitalInput.SetGeneralCapital1InputAttributes(
                        calcParameters, ref currentCalculationsElement,
                        ref currentElement, updates);
                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices:
                    CapitalService1Input capitalServicesInput = new CapitalService1Input();
                    capitalServicesInput.SetCapitalService1InputProperties(calcParameters,
                        currentCalculationsElement, currentElement);
                    bHasCalculations = SetCapitalServiceCalculations(calcParameters, 
                        calculatorType, ref capitalServicesInput, ref sErrorMessage);
                    //serialize the new calculations back into currentCalculationsElement
                    capitalServicesInput.SetCapitalService1InputAttributes(
                        calcParameters, ref currentCalculationsElement,
                        ref currentElement, updates);
                    //set calculatorid (primary way to display calculation attributes)
                    CalculatorHelpers.SetCalculatorId(
                        currentCalculationsElement, currentElement);
                    break;
                case AgBudgetingHelpers.CALCULATOR_TYPES.lifecycle:
                    LCC1Input lcc1Input = new LCC1Input();
                    bHasCalculations = lcc1Input.SetLCC1Calculations(calcParameters,
                        ref currentCalculationsElement, ref currentElement);
                    break;
                default:
                    break;
            }
            calcParameters.ErrorMessage = sErrorMessage;
            return bHasCalculations;
        }
        public static bool SetAgMachineryCalculations(CalculatorParameters calcParameters, 
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            Machinery1Input machineryInput, XElement calculationsElement,
            ref string errorMsg)
        {
            bool bHasCalculations = false;
            TransferCorrespondingDbProperties(calcParameters, machineryInput, true);
            //set the initial oc calculations
            SetAgMachineryOCCalculations(ref machineryInput);
            //set the aoh calculations
            SetAgMachineryAOHCalculations(calculatorType, ref machineryInput);
            //set any remaining time-dependent calculations
            CapitalService1Input oCapServicesInput = InitCapitalServicesObject(calcParameters, 
                calculatorType, calculationsElement, machineryInput);
            //carry out the calculations based on the initial params contained in capservices doc
            SetCapitalServicesCalculations(calcParameters, 
                AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery, ref oCapServicesInput);
            SetCapitalServiceUnits(calculatorType, ref oCapServicesInput);
            //transfer the capital services totals to the machineryInput
            SetCapitalServiceTotals(oCapServicesInput, ref machineryInput);
            if (string.IsNullOrEmpty(machineryInput.ErrorMessage))
            {
                //see if any db props are being changed by calculator
                TransferCorrespondingDbProperties(calcParameters, machineryInput, false);
                bHasCalculations = true;
            }
            else
            {
                errorMsg = machineryInput.ErrorMessage;
            }
            return bHasCalculations;
        }

        public static bool SetIrrPowerCalculations(CalculatorParameters calcParameters,
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            IrrigationPower1Input irrigationPowerInput, ref string errorMsg)
        {
            bool bHasCalculations = false;
            TransferCorrespondingDbProperties(calcParameters, irrigationPowerInput, true);
            //set the initial oc calculations
            SetIrrPowerOCCalculations(ref irrigationPowerInput);
            //set the aoh calculations
            SetIrrPowerAOHCalculations(calculatorType, ref irrigationPowerInput);
            if (string.IsNullOrEmpty(irrigationPowerInput.ErrorMessage))
            {
                //see if any db props are being changed by calculator
                TransferCorrespondingDbProperties(calcParameters, irrigationPowerInput, false);
                bHasCalculations = true;
            }
            else
            {
                errorMsg = irrigationPowerInput.ErrorMessage;
            }
            return bHasCalculations;
        }

        public static bool SetGenCapitalCalculations(CalculatorParameters calcParameters,
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            GeneralCapital1Input capitalInput, ref string errorMsg)
        {
            bool bHasCalculations = false;
            TransferCorrespondingDbProperties(calcParameters, capitalInput, true);
            //set the initial oc calculations
            SetGenCapitalOCCalculations(ref capitalInput);
            //set the aoh calculations
            SetGeneralCapitalAOHCalculations(calculatorType, ref capitalInput);
            if (string.IsNullOrEmpty(capitalInput.ErrorMessage))
            {
                //see if any db props are being changed by calculator
                TransferCorrespondingDbProperties(calcParameters, capitalInput, false);
                bHasCalculations = true;
            }
            else
            {
                errorMsg = capitalInput.ErrorMessage;
            }
            return bHasCalculations;
        }
   
        public static bool SetCapitalServiceCalculations(CalculatorParameters calcParameters, 
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            ref CapitalService1Input capitalServiceInput, 
            ref string errorMsg)
        {
            bool bHasCalculations = false;
            TransferCorrespondingDbProperties(calcParameters, capitalServiceInput, true);
            //set any remaining time-dependent calculations
            InitCapitalServicesObject(calculatorType, ref capitalServiceInput);
            //carry out the calculations based on the initial params contained in capservices doc
            SetCapitalServicesCalculations(calcParameters, 
            AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices, ref capitalServiceInput);
            SetCapitalServiceUnits(calculatorType, ref capitalServiceInput);
            if (string.IsNullOrEmpty(capitalServiceInput.ErrorMessage))
            {
                //see if any db props are being changed by calculator
                TransferCorrespondingDbProperties(calcParameters, capitalServiceInput, false);
                bHasCalculations = true;
            }
            else
            {
                errorMsg = capitalServiceInput.ErrorMessage;
            }
            return bHasCalculations;
        }
     
        private static void TransferCorrespondingDbProperties(
            CalculatorParameters calcParameters,
            Machinery1Input machineryInput, bool isPreCalculation)
        {
            //don't make it easy to change db properties because of descendent calcs
            if (isPreCalculation)
            {
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (machineryInput.CAPPrice > 0)
                    {
                        //these calculators don't change db CAPPrice
                        machineryInput.MarketValue = machineryInput.CAPPrice;
                    }
                }
                else
                {
                    //but stand alone and custom do
                }
            }
            else
            {
                //calculators rely on aliases to change db properties
                //(i.e. ServiceCapacity for OCAmount)
                //reminder that changing descendendent db attributes is generally not a good idea
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (machineryInput.Constants.ServiceCapacity > 0)
                    {
                        machineryInput.OCAmount = machineryInput.Constants.ServiceCapacity;
                    }
                }
                else
                {
                    if (machineryInput.MarketValue > 0)
                    {
                        machineryInput.CAPPrice = machineryInput.MarketValue;
                    }
                    if (machineryInput.Constants.ServiceCapacity > 0)
                    {
                        machineryInput.OCAmount = machineryInput.Constants.ServiceCapacity;
                    }
                }
            }
        }
        private static void TransferCorrespondingDbProperties(
            CalculatorParameters calcParameters,
            IrrigationPower1Input irrigationPowerInput, bool isPreCalculation)
        {
            if (isPreCalculation)
            {
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (irrigationPowerInput.CAPPrice > 0)
                    {
                        //these calculators don't change db CAPPrice
                        irrigationPowerInput.MarketValue = irrigationPowerInput.CAPPrice;
                    }
                }
                else
                {
                    //but stand alone and custom do
                }
            }
            else
            {
                //calculators rely on aliases to change db properties
                //(i.e. ServiceCapacity for OCAmount)
                //reminder that changing descendendent db attributes is generally not a good idea
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (irrigationPowerInput.Constants.ServiceCapacity > 0)
                    {
                        irrigationPowerInput.OCAmount = irrigationPowerInput.Constants.ServiceCapacity;
                    }
                }
                else
                {
                    if (irrigationPowerInput.MarketValue > 0)
                    {
                        irrigationPowerInput.CAPPrice = irrigationPowerInput.MarketValue;
                    }
                    if (irrigationPowerInput.Constants.ServiceCapacity > 0)
                    {
                        irrigationPowerInput.OCAmount = irrigationPowerInput.Constants.ServiceCapacity;
                    }
                }
            }
        }
        
        private static void TransferCorrespondingDbProperties(
            CalculatorParameters calcParameters,
            GeneralCapital1Input capitalInput, bool isPreCalculation)
        {
            if (isPreCalculation)
            {
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (capitalInput.CAPPrice > 0)
                    {
                        //these calculators don't change db CAPPrice
                        capitalInput.MarketValue = capitalInput.CAPPrice;
                    }
                }
                else
                {
                    //but stand alone and custom do
                }
            }
            else
            {
                //calculators rely on aliases to change db properties
                //(i.e. ServiceCapacity for OCAmount)
                //reminder that changing descendendent db attributes is generally not a good idea
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (capitalInput.Constants.ServiceCapacity > 0)
                    {
                        capitalInput.OCAmount = capitalInput.Constants.ServiceCapacity;
                    }
                }
                else
                {
                    if (capitalInput.MarketValue > 0)
                    {
                        capitalInput.CAPPrice = capitalInput.MarketValue;
                    }
                    if (capitalInput.Constants.ServiceCapacity > 0)
                    {
                        capitalInput.OCAmount = capitalInput.Constants.ServiceCapacity;
                    }
                }
            }
        }
        private static void TransferCorrespondingDbProperties(
            CalculatorParameters calcParameters,
            CapitalService1Input capitalServiceInput, bool isPreCalculation)
        {
            if (isPreCalculation)
            {
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (capitalServiceInput.CAPPrice > 0)
                    {
                        //these calculators don't change db CAPPrice
                        capitalServiceInput.MarketValue = capitalServiceInput.CAPPrice;
                    }
                }
                else
                {
                    //but stand alone and custom do
                }
            }
            else
            {
                //calculators rely on aliases to change db properties
                //(i.e. ServiceCapacity for OCAmount)
                //reminder that changing descendendent db attributes is generally not a good idea
                if (calcParameters.AttributeNeedsDbUpdate)
                {
                    if (capitalServiceInput.Constants.ServiceCapacity > 0)
                    {
                        capitalServiceInput.OCAmount = capitalServiceInput.Constants.ServiceCapacity;
                    }
                }
                else
                {
                    if (capitalServiceInput.MarketValue > 0)
                    {
                        capitalServiceInput.CAPPrice = capitalServiceInput.MarketValue;
                    }
                    if (capitalServiceInput.Constants.ServiceCapacity > 0)
                    {
                        capitalServiceInput.OCAmount = capitalServiceInput.Constants.ServiceCapacity;
                    }
                }
            }
        }
        public static void SetAgMachineryOCCalculations(
            ref Machinery1Input machineryInput)
        {
            GeneralRules.CAPACITY_TYPES eCapacityType;
            GeneralRules.FUEL_TYPES eFuelType;
            double dbLaborAmount = 0;
            double dbFuelAmount = 0;
            double dbFuelCostHr = 0;
            double dbLaborCostHr = 0;
            double dbLubeOilAmount = 0;
            string sLubeOilUnit = string.Empty;
            double dbLubeCostHr = 0;
           
            double dbCapacity = 0;

            eCapacityType = (GeneralRules.CAPACITY_TYPES)machineryInput.OptionForCapacity;
            eFuelType = (GeneralRules.FUEL_TYPES)machineryInput.OptionForFuel;
            string sLaborUnit = string.Empty;
            GeneralRules.UNIT_TYPES eUnitType
                = GeneralRules.GetUnitsEnum(machineryInput.Local.UnitGroupId);

            //calculate fuel and lube costs per hour
            if ((machineryInput.Constants.FuelType == Constants.NONE)
                || (machineryInput.Constants.HPPTOMax <= 0))
            {
                dbFuelCostHr = 0;
                dbLubeCostHr = 0;
            }
            else
            {
                if (eFuelType == GeneralRules.FUEL_TYPES.operation)
                {
                    dbFuelCostHr = GeneralRules.FuelCostHr(eUnitType,
                        machineryInput.Constants.FuelType, machineryInput.Constants.HPPTOMax,
                        machineryInput.Constants.HPPTOEquiv, machineryInput.Constants.PriceGas,
                        machineryInput.Constants.PriceDiesel, machineryInput.Constants.PriceLP,
                        ref dbFuelAmount);
                }
                else if (eFuelType == GeneralRules.FUEL_TYPES.enterprise)
                {
                    dbFuelCostHr = GeneralRules.FuelAvgCostHr(eUnitType,
                        machineryInput.Constants.FuelType, machineryInput.Constants.HPPTOMax,
                        machineryInput.Constants.PriceGas, machineryInput.Constants.PriceDiesel,
                        machineryInput.Constants.PriceLP, ref dbFuelAmount);
                }
                GeneralRules.LubeOilCostHr(eUnitType, 
                    machineryInput.Constants.FuelType, machineryInput.Constants.HP, 
                    machineryInput.Constants.PriceOil, out dbLubeOilAmount,
                    out sLubeOilUnit, out dbLubeCostHr);
            }

            //get labor cost
            dbLaborCostHr = GeneralRules.LaborCostHr(machineryInput.Constants.LaborType, 
                machineryInput.Constants.PriceRegularLabor,
                machineryInput.Constants.PriceMachineryLabor,
                machineryInput.Constants.PriceSupervisorLabor, 
                machineryInput.LaborAmountAdj, ref dbLaborAmount, out sLaborUnit);

            //set capacity calculation
            dbCapacity = GeneralRules.Capacity(eUnitType, eCapacityType,
                machineryInput.Constants.FieldSpeedTypical, machineryInput.Constants.Width,
                machineryInput.Constants.FieldEffTypical, machineryInput.Constants.ServiceCapacity);
            machineryInput.Constants.ServiceCapacity = dbCapacity;
            machineryInput.OCAmount = dbCapacity;
            //set fuel costs
            machineryInput.FuelCost = dbFuelCostHr;
            machineryInput.FuelPrice = AgBudgetingRules.GetFuelPrice(machineryInput);
            machineryInput.FuelAmount = dbFuelAmount;
            machineryInput.FuelUnit = GeneralRules.GetFuelUnit(
                machineryInput.Constants.FuelType, machineryInput.Local.UnitGroupId);
            machineryInput.LubeOilAmount = dbLubeOilAmount;
            machineryInput.LubeOilUnit = sLubeOilUnit;
            machineryInput.LubeOilCost = dbLubeCostHr;
            machineryInput.LubeOilPrice = machineryInput.Constants.PriceOil;
            //set labor costs
            machineryInput.LaborCost = dbLaborCostHr;
            machineryInput.LaborPrice = AgBudgetingRules.GetLaborPrice(machineryInput);
            //this is a multiplier (i.e. 1.1)
            machineryInput.LaborAmount = dbLaborAmount;
            machineryInput.LaborUnit = sLaborUnit;
        }
        public static void SetIrrPowerOCCalculations(
            ref IrrigationPower1Input irrigationPowerInput)
        {
            GeneralRules.UNIT_TYPES eUnitType
                = GeneralRules.GetUnitsEnum(irrigationPowerInput.Local.UnitGroupId);
            //calculate fuel and lube costs per hour
            //set calcs
            irrigationPowerInput.FuelPrice = AgBudgetingRules.GetFuelPrice(irrigationPowerInput);
            if ((irrigationPowerInput.Constants.FuelType == Constants.NONE)
                || (irrigationPowerInput.FlowRate <= 0))
            {
                irrigationPowerInput.FuelCost = 0;
            }
            else
            {
                //the costs are per acin or per m3 (easier to understand than per hour costs)
                //ocunit = acin or m3
                //ocamount = acin per acre or m3 per hectare
                //ocprice per acin or m3 = fuelcost per acin + watercost per acin + laborcost per acin + repaircost per acin + lubeoilcost per acin
                //occost = ocamount * ocprice
                //total seasonalcost = input.times * occost per acre or per hectare
                //set the power properties
                IrrigationPower1Input.SetIrrigationPowerProperties(eUnitType, irrigationPowerInput);
                //set the fuel properties
                IrrigationPower1Input.SetIrrigationPowerFuelProperties(eUnitType, irrigationPowerInput);
                //set the lube
                IrrigationPower1Input.SetIrrigationLubeCostPerVolumeWater(eUnitType, irrigationPowerInput);
            }
            //the costs are per acin or per m3 (easier to understand than per hour costs)
            //set the water properties
            IrrigationPower1Input.SetIrrigationWaterProperties(eUnitType, irrigationPowerInput);
          
            //set the labor properties
            IrrigationPower1Input.SetIrrigationLaborProperties(eUnitType, irrigationPowerInput);
            
            //calc repair cost
            IrrigationPower1Input.SetIrrigationRepairCostPerVolumeWater(irrigationPowerInput);

            //calc standby energy costs
            IrrigationPower1Input.SetExtraEnergyProperties(eUnitType, irrigationPowerInput);

            //oc costs per acin or m3 of irrigation water applied
            irrigationPowerInput.OCPrice = irrigationPowerInput.FuelCost + irrigationPowerInput.LubeOilCost
                + irrigationPowerInput.LaborCost + irrigationPowerInput.EquipmentLaborCost + irrigationPowerInput.RepairCost
                + irrigationPowerInput.WaterCost + irrigationPowerInput.EnergyExtraCost;

            //ac in or m3 per ac or per ha (beta 0.9.0 bases costs on volume of water applied not hours)
            irrigationPowerInput.OCAmount = irrigationPowerInput.SeasonWaterApplied;
            //ocunit is ac in or m3
            irrigationPowerInput.OCUnit = "acre inch";
            if (eUnitType != GeneralRules.UNIT_TYPES.imperial)
            {
                irrigationPowerInput.OCUnit = "meter cubed";
            }
            irrigationPowerInput.TotalOC = irrigationPowerInput.OCAmount * irrigationPowerInput.OCPrice;
        }
        public static void SetGenCapitalOCCalculations(ref GeneralCapital1Input capitalInput)
        {
            double dbLaborAmount = 0;
            double dbFuelCostHr = 0;
            string sLaborUnit = string.Empty;
            double dbLaborCostHr = 0;
            string sFuelUnit = string.Empty;
            double dbFuelAmount = 0;
            double dbRepairCostHr = 0;

            if (capitalInput.Capital1Constant.RandMPercent > 1)
            {
                //use decimals
                capitalInput.Capital1Constant.RandMPercent = capitalInput.Capital1Constant.RandMPercent / 100;
            }

            GeneralRules.UNIT_TYPES eUnitType
                = GeneralRules.GetUnitsEnum(capitalInput.Local.UnitGroupId);
            //calculate fuel per hour
            if ((capitalInput.Constants.FuelType == Constants.NONE)
                || (capitalInput.EnergyUseHr <= 0))
            {
                dbFuelCostHr = 0;
            }
            else
            {
                dbFuelCostHr = GeneralRules.FuelCostHr2(eUnitType, capitalInput.Constants.FuelType,
                    capitalInput.EnergyUseHr, capitalInput.EnergyEffTypical,
                    capitalInput.Constants.PriceGas, capitalInput.Constants.PriceDiesel,
                    capitalInput.Constants.PriceLP, capitalInput.Constants.PriceElectric,
                    capitalInput.Constants.PriceNG, ref dbFuelAmount, out sFuelUnit);
            }
            //get labor cost
            dbLaborCostHr = GeneralRules.LaborCostHr(capitalInput.Constants.LaborType,
                capitalInput.Constants.PriceRegularLabor, capitalInput.Constants.PriceMachineryLabor,
                capitalInput.Constants.PriceSupervisorLabor,
                capitalInput.LaborAmountAdj, ref dbLaborAmount, out sLaborUnit);
            //get repair cost
            dbRepairCostHr = GeneralRules.RepairCostHr(capitalInput.MarketValue,
                capitalInput.Capital1Constant.RandMPercent, capitalInput.Constants.PlannedUseHrs);

            //no capacity calculations in this version
            capitalInput.Constants.ServiceCapacity = 0;
            //set fuel costs
            capitalInput.FuelCost = dbFuelCostHr;
            capitalInput.FuelAmount = dbFuelAmount;
            capitalInput.FuelPrice = AgBudgetingRules.GetFuelPrice(capitalInput);
            capitalInput.FuelUnit = sFuelUnit;
            //set repair and maintenance costs
            capitalInput.RepairCost = dbRepairCostHr;
            //set labor costs
            capitalInput.LaborCost = dbLaborCostHr;
            //this is a multiplier (i.e. 1.1)
            capitalInput.LaborAmount = dbLaborAmount;
            capitalInput.LaborPrice = AgBudgetingRules.GetLaborPrice(capitalInput);
            capitalInput.LaborUnit = sLaborUnit;
            capitalInput.OCPrice = dbFuelCostHr + dbLaborCostHr + dbRepairCostHr;
            //Note: the oc amount has be set in the calculator; all costs are per hour of use
            //so if OCAmount = 2, the costs will be costs per hour * 2
        }
        public static void SetAgMachineryAOHCalculations(
            AgBudgetingHelpers.CALCULATOR_TYPES calculateType,
            ref Machinery1Input machineryInput)
        {
            double dbTHandI = 0;
            if (machineryInput.MarketValue == 0)
            {
                //they forgot to fill in a market value
                machineryInput.MarketValue = machineryInput.CAPPrice;
            }
            FixAOHConstantsAndLocals(machineryInput.Local, machineryInput.Constants);
            //calculate taxes, housing and insurance
            dbTHandI = GeneralRules.THandICosts(machineryInput.Constants.TaxPercent,
                machineryInput.Constants.InsurePercent, machineryInput.Constants.HousingPercent,
                machineryInput.MarketValue, machineryInput.SalvageValue,
                machineryInput.Local.RealRate, machineryInput.Constants.StartingHrs,
                machineryInput.Constants.PlannedUseHrs, machineryInput.Constants.UsefulLifeHrs);
            machineryInput.TaxesHousingInsuranceCost
                = GeneralRules.THandICostsHr(dbTHandI, machineryInput.Constants.PlannedUseHrs);
            //goes through capital services to figure time-dependent costs
            machineryInput.AOHPrice = machineryInput.TaxesHousingInsuranceCost;
            //synch oc and aoh amounts
            machineryInput.AOHAmount = machineryInput.OCAmount;
        }
        public static void SetIrrPowerAOHCalculations(
            AgBudgetingHelpers.CALCULATOR_TYPES calculateType,
            ref IrrigationPower1Input irrigationPowerInput)
        {
            double dbTHandI = 0;
            double dbCapRecoverCost = 0;
            GeneralRules.INFLATION_TYPES eInflationType;
            if (irrigationPowerInput.MarketValue == 0)
            {
                //they forgot to fill in a market value
                irrigationPowerInput.MarketValue = irrigationPowerInput.CAPPrice;
            }
            FixAOHConstantsAndLocals(irrigationPowerInput.Local, irrigationPowerInput.Constants);
            //calculate taxes, housing and insurance
            dbTHandI = GeneralRules.THandICosts(irrigationPowerInput.Constants.TaxPercent,
                irrigationPowerInput.Constants.InsurePercent, irrigationPowerInput.Constants.HousingPercent,
                irrigationPowerInput.MarketValue, irrigationPowerInput.SalvageValue,
                irrigationPowerInput.Local.RealRate, irrigationPowerInput.Constants.StartingHrs,
                irrigationPowerInput.Constants.PlannedUseHrs, irrigationPowerInput.Constants.UsefulLifeHrs);
            //irrigation costs are calculated  per acre inch volume water applied
            //version 1.6.3 corrected formula from per hour to per acre and per acreinch
            double dbTaxesHousingInsuranceCostPerVolumeWater = (dbTHandI / irrigationPowerInput.IrrigationNetArea)
                / irrigationPowerInput.SeasonWaterApplied;
            //double dbTaxesHousingInsuranceCostPerVolumeWater
            //    = GeneralRules.THandICostsHr(dbTHandI, irrigationPowerInput.Constants.PlannedUseHrs)
            //    / irrigationPowerInput.PumpCapacity;
            irrigationPowerInput.TaxesHousingInsuranceCost = dbTaxesHousingInsuranceCostPerVolumeWater;
            if (irrigationPowerInput.MarketValue > 0)
            {
                eInflationType = (GeneralRules.INFLATION_TYPES)irrigationPowerInput.OptionForInflation;
                dbCapRecoverCost = GeneralRules.CapRecoverCostAnn(
                    eInflationType, irrigationPowerInput.MarketValue, irrigationPowerInput.Local.InflationRate,
                    irrigationPowerInput.Local.RealRate, irrigationPowerInput.Local.NominalRate,
                    irrigationPowerInput.Constants.StartingHrs, irrigationPowerInput.Constants.UsefulLifeHrs,
                    irrigationPowerInput.Constants.PlannedUseHrs, irrigationPowerInput.SalvageValue);
                //irrigation costs are calculated  per acre inch volume water applied
                //version 1.6.3 corrected formula from per hour to per acre and per acreinch
                double dbCapitalCostPerVolumeWater = (dbCapRecoverCost / irrigationPowerInput.IrrigationNetArea)
                    / irrigationPowerInput.SeasonWaterApplied;
                //double dbCapitalCostPerVolumeWater = GeneralRules.CapRecoverCostHr(
                //    dbCapRecoverCost, irrigationPowerInput.Constants.PlannedUseHrs) 
                //    / irrigationPowerInput.PumpCapacity;
                irrigationPowerInput.CapitalRecoveryCost = dbCapitalCostPerVolumeWater;
                irrigationPowerInput.AOHPrice
                    = irrigationPowerInput.CapitalRecoveryCost + irrigationPowerInput.TaxesHousingInsuranceCost;
            }
            //synch oc and aoh amounts
            irrigationPowerInput.AOHAmount = irrigationPowerInput.OCAmount;
            irrigationPowerInput.AOHUnit = irrigationPowerInput.OCUnit;
        }
        public static void SetGeneralCapitalAOHCalculations(
            AgBudgetingHelpers.CALCULATOR_TYPES calculateType,
            ref GeneralCapital1Input capitalInput)
        {
            double dbTHandI = 0;
            double dbCapRecoverCost = 0;
            GeneralRules.INFLATION_TYPES eInflationType;
            if (capitalInput.MarketValue == 0)
            {
                //they forgot to fill in a market value
                capitalInput.MarketValue = capitalInput.CAPPrice;
            }
            FixAOHConstantsAndLocals(capitalInput.Local, capitalInput.Constants);
            //calculate taxes, housing and insurance
            dbTHandI = GeneralRules.THandICosts(capitalInput.Constants.TaxPercent,
                capitalInput.Constants.InsurePercent, capitalInput.Constants.HousingPercent,
                capitalInput.MarketValue, capitalInput.SalvageValue,
                capitalInput.Local.RealRate, capitalInput.Constants.StartingHrs,
                capitalInput.Constants.PlannedUseHrs, capitalInput.Constants.UsefulLifeHrs);
            capitalInput.TaxesHousingInsuranceCost
                = GeneralRules.THandICostsHr(dbTHandI, capitalInput.Constants.PlannedUseHrs);
            if (capitalInput.MarketValue > 0)
            {

                eInflationType = (GeneralRules.INFLATION_TYPES)capitalInput.OptionForInflation;
                dbCapRecoverCost = GeneralRules.CapRecoverCostAnn(
                    eInflationType, capitalInput.MarketValue, capitalInput.Local.InflationRate,
                    capitalInput.Local.RealRate, capitalInput.Local.NominalRate,
                    capitalInput.Constants.StartingHrs, capitalInput.Constants.UsefulLifeHrs,
                    capitalInput.Constants.PlannedUseHrs, capitalInput.SalvageValue);
                capitalInput.CapitalRecoveryCost = GeneralRules.CapRecoverCostHr(
                    dbCapRecoverCost, capitalInput.Constants.PlannedUseHrs);
                capitalInput.AOHPrice
                    = capitalInput.CapitalRecoveryCost + capitalInput.TaxesHousingInsuranceCost;
            }
            //synch oc and aoh amounts
            capitalInput.AOHAmount = capitalInput.OCAmount;
        }
        private static void FixAOHConstantsAndLocals(Local local, 
            Machinery1Constant constant)
        {
            if (local.RealRate > 0.999)
                local.RealRate = local.RealRate / 100;

            if (constant.PlannedUseHrs == 0)
                constant.PlannedUseHrs = 1;
            //note that these are 7.5% per $1000 so have two extra digits in multiplier (1.5 = .0015)
            //the 0.5 condition is because only one option is less than 1 (0.5)
            if (constant.HousingPercent > 0.99 || constant.HousingPercent == 0.5)
            {
                constant.HousingPercent = constant.HousingPercent / 1000;
            }
            if (constant.TaxPercent > 0.99 || constant.TaxPercent == 0.5)
            {
                constant.TaxPercent = constant.TaxPercent / 1000;
            }
            if (constant.InsurePercent > 0.99 || constant.InsurePercent == 0.5)
            {
                constant.InsurePercent = constant.InsurePercent / 1000;
            }
            double dbRealRate = local.RealRate;
            double dbNominalRate = local.NominalRate;
            double dbInflationRate = 0;
            GeneralRules.MissingRate(ref dbRealRate,
                ref dbNominalRate, ref dbInflationRate);
            local.RealRate = dbRealRate;
            local.NominalRate = dbNominalRate;
            local.InflationRate = dbInflationRate;
        }
        public static void SetCapitalServiceTotals(
            CapitalService1Input capitalServicesInput,
             ref Machinery1Input machineryInput)
        {
            machineryInput.FuelCost = capitalServicesInput.FuelCost;
            machineryInput.LubeOilCost = capitalServicesInput.LubeOilCost;
            machineryInput.RepairCost = capitalServicesInput.RepairCost;
            machineryInput.LaborCost = capitalServicesInput.LaborCost;
            machineryInput.LaborAmount = capitalServicesInput.LaborAmount;
            machineryInput.OCPrice = capitalServicesInput.OCPrice;
            machineryInput.CapitalRecoveryCost = capitalServicesInput.CapitalRecoveryCost;
            machineryInput.TaxesHousingInsuranceCost = capitalServicesInput.TaxesHousingInsuranceCost;
            machineryInput.AOHPrice = capitalServicesInput.AOHPrice;
            machineryInput.OCUnit = capitalServicesInput.OCUnit;
            machineryInput.AOHUnit = capitalServicesInput.AOHUnit;
            machineryInput.CAPUnit = EACH;
        }
        private static void SetCapitalServiceUnits(AgBudgetingHelpers.CALCULATOR_TYPES
            calculatorType, ref CapitalService1Input capitalServicesInput)
        {
            string sAttValue = string.Empty;
            if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower
                || calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital)
            {
                sAttValue = HOUR;
            }
            else
            {
                GetCapacityUnits(capitalServicesInput.OptionForCapacity.ToString(),
                    capitalServicesInput.Local.UnitGroupId.ToString(), out sAttValue);
            }
            capitalServicesInput.OCUnit = sAttValue;
            capitalServicesInput.AOHUnit = sAttValue;
            capitalServicesInput.CAPUnit = EACH;
        }
        
        /// <summary>
        /// Get an input unit based on two input parameters
        /// </summary>
        public static void GetCapacityUnits(string optionForCapacity,
            string unitGroupId, out string unit)
        {
            unit = HOUR;
            int iUnitGroupId = (string.IsNullOrEmpty(unitGroupId) || unitGroupId == null)
                ? 1 : CalculatorHelpers.ConvertStringToInt(unitGroupId);
            if (optionForCapacity == "1")
            {
                if (iUnitGroupId < 1001)
                {
                    //metric
                    unit = HOURS_PER_HECTARE;
                }
                else
                {
                    unit = HOURS_PER_ACRE;
                }

            }
            else if (optionForCapacity == "2")
            {
                if (iUnitGroupId < 1001)
                {
                    //metric
                    unit = HOURS_PER_METRIC_TON;
                }
                else
                {
                    unit = HOURS_PER_TON;
                }
            }
            if (optionForCapacity == "3")
            {
                if (iUnitGroupId < 1001)
                {
                    //metric
                    unit = METRIC_TONS_PER_HOUR;
                }
                else
                {
                    unit = TONS_PER_HOUR;
                }
            }
        }
        /// <summary>
        /// Make a capital services node, holding calculation params,
        /// from an existing Machinery1Input input node 
        /// (so that time-dependent costs can be treated uniformly among calculators)
        /// </summary>
        public static CapitalService1Input InitCapitalServicesObject(CalculatorParameters calcParameters, 
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType, XElement calculationsElement,
            Machinery1Input machineryInput)
        {
            //1. build a generic capital input
            //2. add time periods to it and carry out calculations for each time period
            //3. sum the time period calcs and add the sums to the capital input
            CapitalService1Input capitalServicesInput = new CapitalService1Input();
            capitalServicesInput.SetCapitalService1InputProperties(calcParameters, 
                machineryInput, calculationsElement);
            if (machineryInput.MarketValue == 0)
            {
                machineryInput.MarketValue = machineryInput.CAPPrice;
            }
            double dbListPrice = machineryInput.MarketValue + (machineryInput.MarketValue * (machineryInput.ListPriceAdj / 100));
            double dbOC1Amount = machineryInput.FuelCost * machineryInput.Constants.PlannedUseHrs;
            double dbOC2Amount = machineryInput.LubeOilCost * machineryInput.Constants.PlannedUseHrs;
            double dbOC4Amount = machineryInput.LaborCost * machineryInput.Constants.PlannedUseHrs;
            double dbAOH2Amount = machineryInput.TaxesHousingInsuranceCost * machineryInput.Constants.PlannedUseHrs;
            capitalServicesInput.OC1Amount = dbOC1Amount;
            capitalServicesInput.OC2Amount = dbOC2Amount;
            //note oc3 starts with list price
            capitalServicesInput.OC3Amount = dbListPrice;
            capitalServicesInput.OC4Amount = dbOC4Amount;
            //aoh1 is calculated later
            capitalServicesInput.AOH1Amount = 0;
            capitalServicesInput.AOH2Amount = dbAOH2Amount;
            string sUnitType = GeneralRules.GetUnitType(machineryInput.Local.UnitGroupId);
            InitCapitalServicesObject(sUnitType, ref capitalServicesInput);
            return capitalServicesInput;
        }
        public static void InitCapitalServicesObject(
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            ref CapitalService1Input capitalServicesInput)
        {
            if (capitalServicesInput.MarketValue == 0)
            {
                capitalServicesInput.MarketValue = capitalServicesInput.CAPPrice;
            }
            double dbListPrice = capitalServicesInput.MarketValue +
                (capitalServicesInput.MarketValue * (capitalServicesInput.ListPriceAdj / 100));
            double dbOC1Amount = capitalServicesInput.FuelCost * 
                capitalServicesInput.Constants.PlannedUseHrs;
            double dbOC2Amount = capitalServicesInput.LubeOilCost * 
                capitalServicesInput.Constants.PlannedUseHrs;
            double dbOC4Amount = capitalServicesInput.LaborCost * 
                capitalServicesInput.Constants.PlannedUseHrs;
            double dbAOH2Amount = capitalServicesInput.TaxesHousingInsuranceCost * 
                capitalServicesInput.Constants.PlannedUseHrs;
            capitalServicesInput.OC1Amount = dbOC1Amount;
            capitalServicesInput.OC2Amount = dbOC2Amount;
            //note oc3 starts with list price
            capitalServicesInput.OC3Amount = dbListPrice;
            capitalServicesInput.OC4Amount = dbOC4Amount;
            //aoh1 is calculated later
            capitalServicesInput.AOH1Amount = 0;
            capitalServicesInput.AOH2Amount = dbAOH2Amount;
            string sUnitType = GeneralRules.GetUnitType(
                capitalServicesInput.Local.UnitGroupId);
            InitCapitalServicesObject(sUnitType, ref capitalServicesInput);
        }
        private static void InitCapitalServicesObject(string unitType,
            ref CapitalService1Input capitalServicesInput)
        {
            capitalServicesInput.ServiceUnits = HOUR;
            //will be adjusted for capacity later
            if (unitType == GeneralRules.UNIT_TYPES.imperial.ToString())
            {
                capitalServicesInput.ServiceCapacityUnits = ACRE;
            }
            else if (unitType == GeneralRules.UNIT_TYPES.metric.ToString())
            {
                capitalServicesInput.ServiceCapacityUnits = HECTARE;
            }
            capitalServicesInput.OC1Name = "Fuel Cost";
            capitalServicesInput.OC2Name = "Lube Oil Cost";
            capitalServicesInput.OC3Name = "Repair Cost";
            capitalServicesInput.OC4Name = "Labor Cost";
            capitalServicesInput.AOH1Name = "Capital Recovery Cost";
            capitalServicesInput.AOH2Name = "Taxes Housing Insurance Cost";

            //changes (i.e. gradients) are not fully implemented in current version
            //only fuel and labor changes
            capitalServicesInput.OC1Change = 0;
            capitalServicesInput.OC2Change = 0;
            capitalServicesInput.OC3Change = 0;
            capitalServicesInput.OC4Change = 0;
            capitalServicesInput.AOH1Change = 0;
            //only thi changes
            capitalServicesInput.AOH2Change = 0;
            capitalServicesInput.ServiceUnitPrice = 0;
            capitalServicesInput.ServiceEnhanceAmount = 0;
        }
        /// <summary>
        /// Purpose:		calculate capital services costs
        /// Inputs:			capitalServicesInput object storing initial parameters needed in calculations
        /// Outputs:		capservice document holding collection of time period calculations; 5 annuities for the sum of the discounted costs (in year t)
        ///	XmlStructure:	root/linkedview/timeperiod
        /// References:		Refer to Chapter 2, tables 5.3 and 5.4, and Chapter Appendices in AAEA reference and related equations
        public static void SetCapitalServicesCalculations(CalculatorParameters calcParameters, 
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType, 
            ref CapitalService1Input capitalServicesInput)
        {
            GeneralRules.TIME_TYPES eTimeType;
            GeneralRules.INFLATION_TYPES eInflationType;
            GeneralRules.GROWTH_SERIES_TYPES eGrowthType;
            //begin cost
            double dbOC1Amount = 0;
            //sum costs
            double dbOC2Amount = 0;
            double dbOC3Amount = 0;
            double dbOC4Amount = 0;

            double dbSalvValue = 0;
            double dbAOH1Amount = 0;
            double dbAOH2Amount = 0;

            int iTimePeriods = 0;
            int iTimeUsed = 0;
            int iRemainingPeriods = 0;
            double dbRF1 = 0;
            double dbRF2 = 0;

            double dbNominalRate = 0;
            double dbRealRate = 0;
            double dbInflationRate = 0;

            int i = 0;
            double dbGradientFactor = 0;
            double dbGradientOC1Factor = 0;
            double dbGradientOC2Factor = 0;
            double dbGradientOC3Factor = 0;
            double dbGradientOC4Factor = 0;
            double dbGradientAOH1Factor = 0;
            double dbGradientAOH2Factor = 0;

            double dbSumCosts = 0;
            double dbOC3SumUsedCost = 0;
            double dbSumOutputs = 0;

            double dbValueToDiscount = 0;
            double dbInflationInit = 0;
            double dbCapRecoverCost = 0;
            double dbAmortizedAvgCostperPeriod = 0;
            double dbOCPrice = 0;
            double dbAOHPrice = 0;

            dbOC1Amount = capitalServicesInput.OC1Amount;
            dbOC2Amount = capitalServicesInput.OC2Amount;
            dbOC3Amount = capitalServicesInput.OC3Amount;
            dbOC4Amount = capitalServicesInput.OC4Amount;
            dbAOH1Amount = capitalServicesInput.AOH1Amount;
            dbAOH2Amount = capitalServicesInput.AOH2Amount;
            dbRF1 = capitalServicesInput.Constants.RF1;
            dbRF2 = capitalServicesInput.Constants.RF2;

            eTimeType = (GeneralRules.TIME_TYPES)capitalServicesInput.OptionForTime;
            eInflationType = (GeneralRules.INFLATION_TYPES)capitalServicesInput.OptionForInflation;

            dbSalvValue = capitalServicesInput.SalvageValue;

            eGrowthType = (GeneralRules.GROWTH_SERIES_TYPES)capitalServicesInput.GrowthType;
            dbRealRate = capitalServicesInput.Local.RealRate;
            dbNominalRate = capitalServicesInput.Local.NominalRate;
            dbInflationRate = 0;
            GeneralRules.MissingRate(ref dbRealRate,
                ref dbNominalRate, ref dbInflationRate);
            if (dbRealRate > 0.999) dbRealRate = dbRealRate / 100;
            if (dbNominalRate > 0.999) dbNominalRate = dbNominalRate / 100;
            if (dbInflationRate > 0.999) dbInflationRate = dbInflationRate / 100;
            capitalServicesInput.Local.RealRate = dbRealRate;
            capitalServicesInput.Local.NominalRate = dbNominalRate;
            capitalServicesInput.Local.InflationRate = dbInflationRate;
            //negative one means division by zero violation
            if (capitalServicesInput.Constants.PlannedUseHrs == 0)
            {
                return;
            }
            //Rule 1. if (a market value for the capital input has been passed in, use a straight capital recovery calc
            //else treat AOH1 like any other cost
            if (capitalServicesInput.MarketValue > 0)
            {
                dbCapRecoverCost = GeneralRules.CapRecoverCostAnn(eInflationType,
                    capitalServicesInput.MarketValue, dbInflationRate, capitalServicesInput.Local.RealRate,
                    capitalServicesInput.Local.NominalRate, capitalServicesInput.Constants.StartingHrs,
                    capitalServicesInput.Constants.UsefulLifeHrs, capitalServicesInput.Constants.PlannedUseHrs,
                    dbSalvValue);
                capitalServicesInput.CapitalRecoveryCost
                    = GeneralRules.CapRecoverCostHr(dbCapRecoverCost,
                    capitalServicesInput.Constants.PlannedUseHrs);
                dbAOH1Amount = 0;
            }
            //if no inflation is to be used
            if (eInflationType == GeneralRules.INFLATION_TYPES.inflationno)
            {
                dbInflationRate = 0;
                dbInflationInit = 0;
            }
            else
            {
                dbInflationInit = dbInflationRate;
            }
            //get the gradient cost factors
            TimePeriodCS.GetGradientFactor(eGrowthType,
                capitalServicesInput.OC1Change, dbOC1Amount, out dbGradientOC1Factor);
            TimePeriodCS.GetGradientFactor(eGrowthType,
                capitalServicesInput.OC2Change, dbOC2Amount, out dbGradientOC2Factor);
            TimePeriodCS.GetGradientFactor(eGrowthType,
                capitalServicesInput.OC3Change, dbOC3Amount, out dbGradientOC3Factor);
            TimePeriodCS.GetGradientFactor(eGrowthType,
                capitalServicesInput.OC4Change, dbOC4Amount, out dbGradientOC4Factor);
            TimePeriodCS.GetGradientFactor(eGrowthType,
                capitalServicesInput.AOH1Change, dbAOH1Amount, out dbGradientAOH1Factor);
            TimePeriodCS.GetGradientFactor(eGrowthType,
                capitalServicesInput.AOH2Change, dbAOH2Amount, out dbGradientAOH2Factor);
            //Step 2. Calculate each of the six cost variables separately
            for (i = 1; i <= 6; i++)
            {
                //initialize variables that were set on last loop
                dbAmortizedAvgCostperPeriod = 0;
                dbSumCosts = 0;
                dbSumOutputs = 0;
                if (i == 1)
                {
                    dbValueToDiscount = dbOC1Amount;
                    dbGradientFactor = dbGradientOC1Factor;
                }
                else if (i == 2)
                {
                    dbValueToDiscount = dbOC2Amount;
                    dbGradientFactor = dbGradientOC2Factor;
                }
                else if (i == 3)
                {
                    //if using the agmach repair calcs, this is initial list price to start
                    dbValueToDiscount = dbOC3Amount;
                    dbGradientFactor = dbGradientOC3Factor;
                }
                else if (i == 4)
                {
                    dbValueToDiscount = dbOC4Amount;
                    dbGradientFactor = dbGradientOC4Factor;
                }
                else if (i == 5)
                {
                    dbValueToDiscount = dbAOH1Amount;
                    dbGradientFactor = dbGradientAOH1Factor;
                }
                else if (i == 6)
                {
                    dbValueToDiscount = dbAOH2Amount;
                    dbGradientFactor = dbGradientAOH2Factor;
                }
                if ((dbValueToDiscount != 0) || (i == 1))
                {
                    //fill in time period costs based on starting parameters and elements
                    TimePeriodCS.CalculateCostsOverTime(calcParameters, calculatorType,
                        ref capitalServicesInput, i, eGrowthType, eTimeType,
                        eInflationType, capitalServicesInput.Local.RealRate, capitalServicesInput.Local.NominalRate,
                        dbInflationInit, dbInflationRate, capitalServicesInput.Constants.StartingHrs,
                        capitalServicesInput.Constants.PlannedUseHrs, dbRF1, dbRF2, dbGradientFactor,
                        dbValueToDiscount, out iTimePeriods, out iTimeUsed,
                        out iRemainingPeriods, out dbSumCosts, out dbOC3SumUsedCost,
                        out dbSumOutputs);
                    //if time varies, convert to annuity and account for used vs. new equipment;
                    //client has option to use annuity or to use a specific year from the collection
                    //calculate amortized costs
                    if (((iTimeUsed > 1) || (capitalServicesInput.Constants.StartingHrs > 0)) && (i == 3))
                    {
                        AgBudgetingRules.CalcUsedMachineryRepairCosts(calculatorType,
                            iTimeUsed, iTimePeriods, capitalServicesInput.Constants.PlannedUseHrs,
                            capitalServicesInput.Constants.StartingHrs, iRemainingPeriods,
                            capitalServicesInput.Local.RealRate, dbInflationRate, eTimeType,
                            dbRF1, dbRF2, dbOC3Amount, dbSumCosts, dbSumOutputs,
                            dbOC3SumUsedCost, out dbAmortizedAvgCostperPeriod);
                    }
                    else
                    {
                        AgBudgetingRules.CalcAmortizdServiceCosts(calculatorType,
                            i, iTimePeriods, capitalServicesInput.Constants.PlannedUseHrs,
                            capitalServicesInput.Constants.UsefulLifeHrs, dbRealRate, eTimeType,
                            dbSumCosts, dbSumOutputs, out dbAmortizedAvgCostperPeriod);
                    }
                    if (eTimeType == GeneralRules.TIME_TYPES.costsvary)
                    {
                        //added for version 1.1; the sums are correct and the annuity is correct; just add inflation to that annuity
                        if ((eInflationType == GeneralRules.INFLATION_TYPES.inflationyesfirst)
                            || (eInflationType == GeneralRules.INFLATION_TYPES.inflationyesall))
                        {
                            dbAmortizedAvgCostperPeriod
                                = dbAmortizedAvgCostperPeriod * (1 + dbInflationInit);
                        }
                    }
                }
                if (capitalServicesInput.MarketValue > 0 && i == 5)
                {
                    //market value means always use cap recovery calculations
                    dbAmortizedAvgCostperPeriod = capitalServicesInput.CapitalRecoveryCost;
                }
                //set the totals in the cap service element
                SetServiceCostTotals(calculatorType, i,
                    dbAmortizedAvgCostperPeriod, capitalServicesInput.CapitalRecoveryCost,
                    capitalServicesInput.MarketValue, ref capitalServicesInput,
                    ref dbOCPrice, ref dbAOHPrice);
            }//for six cost variables
            capitalServicesInput.OCPrice = dbOCPrice;
            capitalServicesInput.AOHPrice = dbAOHPrice;
        }
        /// <summary>
        /// Set the final totals attributes in the capital service element
        /// </summary>
        private static void SetServiceCostTotals(AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            int iCostVariable, double amortizedAvgCostperPeriod, double capRecoverCostPeriod,
            double marketValue, ref CapitalService1Input capitalServicesInput,
            ref double ocPrice, ref double aohPrice)
        {
            //set final calculator variables
            double dbAnnuityPerServiceUnit = 0;
            switch (iCostVariable)
            {
                case 1:
                    dbAnnuityPerServiceUnit = amortizedAvgCostperPeriod;
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        capitalServicesInput.FuelCost = dbAnnuityPerServiceUnit;
                    }
                    else if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices)
                    {
                        capitalServicesInput.OC1Cost = dbAnnuityPerServiceUnit;
                    }
                    ocPrice = dbAnnuityPerServiceUnit;
                    break;
                case 2:
                    dbAnnuityPerServiceUnit = amortizedAvgCostperPeriod;
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        capitalServicesInput.LubeOilCost = dbAnnuityPerServiceUnit;
                    }
                    else if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices)
                    {
                        capitalServicesInput.OC2Cost = dbAnnuityPerServiceUnit;
                    }
                    ocPrice = dbAnnuityPerServiceUnit + ocPrice;
                    break;
                case 3:
                    dbAnnuityPerServiceUnit = amortizedAvgCostperPeriod;
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        capitalServicesInput.RepairCost = dbAnnuityPerServiceUnit;
                    }
                    else if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices)
                    {
                        capitalServicesInput.OC3Cost = dbAnnuityPerServiceUnit;
                    }
                    ocPrice = dbAnnuityPerServiceUnit + ocPrice;
                    break;
                case 4:
                    dbAnnuityPerServiceUnit = amortizedAvgCostperPeriod;
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        capitalServicesInput.LaborCost = dbAnnuityPerServiceUnit;
                    }
                    else if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices)
                    {
                        capitalServicesInput.OC4Cost = dbAnnuityPerServiceUnit;
                    }
                    ocPrice = dbAnnuityPerServiceUnit + ocPrice;
                    break;
                case 5:
                    if (marketValue > 0)
                    {
                        dbAnnuityPerServiceUnit = capRecoverCostPeriod;
                    }
                    else
                    {
                        dbAnnuityPerServiceUnit = amortizedAvgCostperPeriod;
                    }
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        capitalServicesInput.CapitalRecoveryCost = dbAnnuityPerServiceUnit;
                    }
                    else if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices)
                    {
                        capitalServicesInput.AOH1Cost = dbAnnuityPerServiceUnit;
                    }
                    aohPrice = dbAnnuityPerServiceUnit;
                    break;
                case 6:
                    dbAnnuityPerServiceUnit = amortizedAvgCostperPeriod;
                    if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery)
                    {
                        capitalServicesInput.TaxesHousingInsuranceCost = dbAnnuityPerServiceUnit;
                    }
                    else if (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices)
                    {
                        capitalServicesInput.AOH2Cost = dbAnnuityPerServiceUnit;
                    }
                    aohPrice = dbAnnuityPerServiceUnit + aohPrice;
                    break;
                default:
                    break;
            }
        }
    }
}
