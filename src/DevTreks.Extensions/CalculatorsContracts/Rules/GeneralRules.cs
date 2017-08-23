using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		General rules for calculators.
    ///Author:		www.devtreks.org
    ///Date:		2017, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class GeneralRules
    {
        /// <summary>
        /// type of growth series to extend profit and investment flows into the future
        /// </summary>
        public enum GROWTH_SERIES_TYPES
        {
            none            = 0,
            uniform         = 1,
            linear          = 2,
            geometric       = 3,
            //NIST 135 Handbook indexes or other upv, femupv, indexes
            upvtable        = 4,
            //single present value used instead of growth series
            spv             = 5,
            //amortized annual costs
            caprecovery     = 6,
            upv             = 7,
            exponential     = 8,
            logarithmic     = 9,
            eaa             = 10,
            caprecoveryspv  = 11
        }
        public enum DISCOUNT_TYPES
        {
            none = 0, //= no escalation,
            factor = 1, //use the escalateR property as a multiplier (the factor has been pre-calculated from a discounting formula)
            spv = 2, //single present value,
            upv = 3, //uniform present value,
            cap = 4, //capital recovery or amortized,
            uniform = 5, //uniform escalation,
            geometric = 6, //geometric escalation,
            linear = 7,
            exponential = 8, //exponential escalation,
            eaa = 9 //equivalent annual annuity
        }
        public enum UNIT_TYPES
        {
            none = 0,
            metric = 1,
            imperial = 2
        }

        public enum INFLATION_TYPES
        {
            none = 0,
            inflationyesfirst = 1,
            inflationyesall = 2,
            inflationno = 3
        }

        public enum FUEL_TYPES
        {
            none = 0,
            operation = 1,
            enterprise = 2
        }
        public enum FUEL_PRICE_TYPES
        {
            none        = 0,
            diesel      = 1,
            gas         = 2,
            lpg         = 3,
            electric    = 4,
            naturalgas  = 5, 
            ethanol     = 6,
            gasohol     = 7
        }
        public enum LABOR_PRICE_TYPES
        {
            none = 0,
            regular = 1,
            machinery = 2,
            supervisory = 3
        }
        public enum TIME_TYPES
        {
            none = 0,
            costsvary = 1,
            costdonotvary = 2,
            costsandoutputsvary = 3
        }

        public enum CAPACITY_TYPES
        {
            none = 0,
            area = 1,
            material = 2,
            area2 = 3,
            material2 = 4
        }
        public enum CAPACITY2_TYPES
        {
            none = 0,
            area = 1,
            hours = 2
        }
        //public const string TIME_PERIOD_ELEMENT = "<timeperiod />";
        //mathematical constants
        public const double OneOverTwelve = 0.0833333;
        public const double GallonToLiter = 3.785412;
        public const double PSITokPa = 6.894757;
        public const double MeterToFeet = .305;
        //1 horsepower = .746 kilowatts (1000 kw = 746 hp)
        public const double HpToKw = .746;
        //1 kw = 1.341 hp
        public const double KwToHp = 1.341;
        //1 pound per foot = 1.48816394 kilograms per meter
        public const double KgPerMeterToPoundPerFoot = .672;
        public const double CubicFeetToCubicMeter = .02831685;
        public const double AcreInchToMeter3 = 102.8;
        public const double AcreWidth = 8.25;
        //(10,000 sq meters / ha) / (1,000 m / km) gives width of hectare 1 km long
        public const double HectareWidth = 10;
        public const double LIST_PRICE_ADJ = 0.150;
        public const double SALVAGE_VALUE_ADJ = 0.10;
        public const int MAX_TIME_PERIODS = 100;
        private const string LITERS_PER_HOUR = "liters/hour";
        private const string GALLONS_PER_HOUR = "gallons/hour";
        private const string KW_PER_HOUR = "kw/hour";
        //cubic m per hour
        private const string M3_PER_HOUR = "m3/hour";
        //thousand cubic feet per hour
        private const string MCF_PER_HOUR = "mcf/hour";

        public const string REAL_RATE = "RealRate";
        public const string NOMINAL_RATE = "NominalRate";
        public const string INFLATION_RATE = "InflationRate";
        public const double DOUBLE_YEAR = 365.25;
        public static GROWTH_SERIES_TYPES GetGrowthType(string growthType)
        {
            GROWTH_SERIES_TYPES eGrowthType = GROWTH_SERIES_TYPES.none;
            if (growthType == GROWTH_SERIES_TYPES.geometric.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.geometric;
            }
            else if (growthType == GROWTH_SERIES_TYPES.linear.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.linear;
            }
            else if (growthType == GROWTH_SERIES_TYPES.uniform.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.uniform;
            }
            else if (growthType == GROWTH_SERIES_TYPES.upvtable.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.upvtable;
            }
            else if (growthType == GROWTH_SERIES_TYPES.spv.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.spv;
            }
            else if (growthType == GROWTH_SERIES_TYPES.caprecovery.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.caprecovery;
            }
            else if (growthType == GROWTH_SERIES_TYPES.upv.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.upv;
            }
            else if (growthType == GROWTH_SERIES_TYPES.eaa.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.eaa;
            }
            else if (growthType == GROWTH_SERIES_TYPES.exponential.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.exponential;
            }
            else if (growthType == GROWTH_SERIES_TYPES.caprecoveryspv.ToString())
            {
                eGrowthType = GROWTH_SERIES_TYPES.caprecoveryspv;
            }
            return eGrowthType;
        }
        public static INFLATION_TYPES GetInflationType(string inflationType)
        {
            INFLATION_TYPES eInflationType = INFLATION_TYPES.none;
            if (inflationType == INFLATION_TYPES.inflationno.ToString())
            {
                eInflationType = INFLATION_TYPES.inflationno;
            }
            else if (inflationType == INFLATION_TYPES.inflationyesall.ToString())
            {
                eInflationType = INFLATION_TYPES.inflationyesall;
            }
            else if (inflationType == INFLATION_TYPES.inflationyesfirst.ToString())
            {
                eInflationType = INFLATION_TYPES.inflationyesfirst;
            }
            return eInflationType;
        }
        /// <summary>
        /// Calculate the equivalent annual annuity of a discounted cash flow stream.
        /// Corresponds to AAEA equation 10.13.
        /// </summary>
        public static double CalculateEquivalentAnnualAnnuity(double pvInvestment,
            double periods, double realRate, double nomRate)
        {
            double dbAnnuity = 0;
            if (periods == 0)
            {
                dbAnnuity = 0;
                return dbAnnuity;
            }
            if (realRate != 0)
            {
                dbAnnuity = pvInvestment / ((1 - (1 / (System.Math.Pow((1 + realRate), periods)))) / realRate);
            }
            else if (nomRate != 0)
            {
                dbAnnuity = pvInvestment / ((1 - (1 / (System.Math.Pow((1 + nomRate), periods)))) / nomRate);
            }
            return dbAnnuity;
        }
        /// <summary>
        /// Calculate an interest rate, given two of the three rates used in the Fisher equation.
        /// Corresponds to AAEA equation 2.6.
        /// </summary>
        public static double CalculateMissingRate(double realRate, double nomRate, 
            double inflationRate)
        {
            double dbMissingRate = 0;
            if (nomRate == 0)
            {
                dbMissingRate = (realRate + inflationRate + (inflationRate * realRate));
            }
            else if (realRate == 0)
            {
                dbMissingRate = ((nomRate - inflationRate) / (1 + inflationRate));
            }
            else if (inflationRate == 0)
            {
                dbMissingRate = ((nomRate - realRate) / (1 + realRate));
            }
            return dbMissingRate;
        }
        /// <summary>
        /// Calculate the fractional payment factor for the last time period of annuites that end mid-stream.
        /// Corresponds to AAEA equations 2B.26 and page 13-20
        /// </summary>
        public static double CalculateFractionalPaymentFactor(double annualRate,
            double fullPeriods, int roundedPeriods,
            double initPeriod, ref double lastTimeDays)
        {
            double dbDiscountFactor = 0;
            double dbFractionPeriod = 0;
            double dbRate = 0;
            if ((initPeriod <= 31))
            {
                lastTimeDays = (fullPeriods * 30) - (roundedPeriods * 30);
                if (lastTimeDays < 0) lastTimeDays = (lastTimeDays * -1);
                if ((lastTimeDays >= 0) && (lastTimeDays <= 1))
                {
                    //1 day or less means 1
                    dbFractionPeriod = 1;
                    lastTimeDays = 30;
                }
                else
                {
                    dbFractionPeriod = (lastTimeDays / 30);
                }
            }
            else
            {
                // use annual
                lastTimeDays = (fullPeriods * DOUBLE_YEAR) - (roundedPeriods * DOUBLE_YEAR);
                if (lastTimeDays < 0) lastTimeDays = (lastTimeDays * -1);
                if ((lastTimeDays >= 0) && (lastTimeDays <= 1))
                {
                    dbFractionPeriod = 1;
                    lastTimeDays = DOUBLE_YEAR;
                }
                else
                {
                    dbFractionPeriod = (lastTimeDays / DOUBLE_YEAR);
                }
            }
            dbRate = 1 + annualRate;
            dbDiscountFactor = (((System.Math.Pow(dbRate, dbFractionPeriod)) - 1) / annualRate);
            return dbDiscountFactor;
        }
        /// <summary>
        /// Calculate a monthly or annual discount factor.
        /// Corresponds to AAEA equation 2.13 for interest and page 2-23 top for factor
        /// </summary>
        public static double CalculateDiscountFactorByMonths(double annualRate,
            double numberOfMonths)
        {
            double dbDiscountFactor = 0;
            double dbMonthlyRate = 0;
            //n = one divided by twelve
            double n = 0.083333;

            dbMonthlyRate = (System.Math.Pow((1 + annualRate), n) - 1);
            //use a monthly discount factor
            dbDiscountFactor = System.Math.Pow((1 + dbMonthlyRate), numberOfMonths);
            return dbDiscountFactor;
        }
        /// <summary>
        /// Calculate a monthly or annual discount factor.
        /// Corresponds to AAEA equation 2B.26 and pages 13-19 and 13-20
        /// </summary>
        public static double CalculateDiscountFactorByMonthsorYears(double annualRate,
            DateTime initialEndOfPeriodDate, DateTime endOfGrowthPeriodDate, bool useMonthly)
        {
            double dbDiscountFactor = 0;
            int iDiscountDays = 0;
            double periods = 0;

            System.TimeSpan spanDiscountDays = endOfGrowthPeriodDate - initialEndOfPeriodDate;
            iDiscountDays = spanDiscountDays.Days;
            if (iDiscountDays < 0)
            {
                iDiscountDays = -1 * (iDiscountDays);
            }
            //don't divide by zero
            if (annualRate == -1) annualRate = 1;

            if (useMonthly == true)
            {
                //if the time periods are monthly, use monthly compounding
                periods = iDiscountDays / 30;
                dbDiscountFactor = CalculateDiscountFactorByMonths(annualRate, periods);
            }
            else
            {
                //if the time periods are not monthly, use annual compounding
                periods = iDiscountDays / DOUBLE_YEAR;
                dbDiscountFactor = CalculateDiscountFactorByYears(annualRate, periods);
            }

            return dbDiscountFactor;
        }
        public static double CalculateDiscountFactorByYears(double annualRate,
            double periods)
        {
            if (annualRate == -1) annualRate = 1;
            if (annualRate > 0)
                annualRate = annualRate / 100;
            double dbDiscountFactor = (1 / (System.Math.Pow((1 + annualRate), periods)));
            return dbDiscountFactor;
        }
        /// <summary>
        /// Calculate the capital recovery factor
        /// </summary>
        public static double CalculateCapitalRecoveryFactor(double periods,
            double realRate, double nomRate)
        {
            double dbFactor = 0;
            if (periods == 0)
            {
                dbFactor = 0;
                return dbFactor;
            }
            if (realRate != 0)
            {
                dbFactor = realRate /
                    (1 - (1 / (System.Math.Pow((1 + realRate), periods))));
            }
            else if (nomRate != 0)
            {
                dbFactor = nomRate /
                    (1 - (1 / (System.Math.Pow((1 + nomRate), periods))));
            }
            return dbFactor;
        }
        /// <summary>
        /// Return the enumerated value of a unit string
        /// </summary>
        /// <param name="unitMeasure"></param>
        /// <returns></returns>
        public static UNIT_TYPES GetUnitsEnum(string unitMeasure)
        {
            UNIT_TYPES unitType = UNIT_TYPES.metric;
            if (unitMeasure == UNIT_TYPES.imperial.ToString())
            {
                unitType = UNIT_TYPES.imperial;
            }
            else if (unitMeasure == UNIT_TYPES.metric.ToString())
            {
                unitType = UNIT_TYPES.metric;
            }
            return unitType;
        }
        public static UNIT_TYPES GetUnitsEnum(int unitGroupId)
        {
            UNIT_TYPES unitType = UNIT_TYPES.metric;
            string sUnitType = GetUnitType(unitGroupId);
            if (sUnitType == UNIT_TYPES.imperial.ToString())
            {
                unitType = UNIT_TYPES.imperial;
            }
            else if (sUnitType == UNIT_TYPES.metric.ToString())
            {
                unitType = UNIT_TYPES.metric;
            }
            return unitType;
        }
        public static string GetUnitType(int unitGroupId)
        {
            string sUnitType = UNIT_TYPES.metric.ToString();
            if (unitGroupId != 1)
            {
                sUnitType = UNIT_TYPES.imperial.ToString();
            }
            return sUnitType;
        }
        /// <summary>
        /// Return the enumerated fuel price type given it's string representation
        /// </summary>
        /// <param name="fuelType"></param>
        /// <returns></returns>
        public static FUEL_PRICE_TYPES GetFuelPriceType(string fuelType)
        {
            FUEL_PRICE_TYPES eFuelPriceType = FUEL_PRICE_TYPES.none;
            if (fuelType == FUEL_PRICE_TYPES.gas.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.gas;
            }
            else if (fuelType == FUEL_PRICE_TYPES.diesel.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.diesel;
            }
            else if (fuelType == FUEL_PRICE_TYPES.lpg.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.lpg;
            }
            else if (fuelType == FUEL_PRICE_TYPES.electric.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.electric;
            }
            else if (fuelType == FUEL_PRICE_TYPES.naturalgas.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.naturalgas;
            }
            else if (fuelType == FUEL_PRICE_TYPES.ethanol.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.ethanol;
            }
            else if (fuelType == FUEL_PRICE_TYPES.gasohol.ToString())
            {
                eFuelPriceType = FUEL_PRICE_TYPES.gasohol;
            }
            return eFuelPriceType;
        }
        public static string GetFuelUnit(string fuelType,
            int unitGroupId)
        {
            string sFuelUnit = LITERS_PER_HOUR;
            //refactor: this should be tied to fueltype
            if (unitGroupId < 1001)
            {
                //metric
                sFuelUnit = LITERS_PER_HOUR;
            }
            else
            {
                sFuelUnit = GALLONS_PER_HOUR;
            }
            return sFuelUnit;
        }
        public static string GetFuelUnit(FUEL_PRICE_TYPES fuelType,
            UNIT_TYPES unitType)
        {
            string sFuelUnit = LITERS_PER_HOUR;
            switch (fuelType)
            {
                case FUEL_PRICE_TYPES.gas:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        sFuelUnit = GALLONS_PER_HOUR;
                    }
                    else
                    {
                        sFuelUnit = LITERS_PER_HOUR;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        sFuelUnit = GALLONS_PER_HOUR;
                    }
                    else
                    {
                        sFuelUnit = LITERS_PER_HOUR;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        sFuelUnit = GALLONS_PER_HOUR;
                    }
                    else
                    {
                        sFuelUnit = LITERS_PER_HOUR;
                    }
                    break;
                case FUEL_PRICE_TYPES.electric:
                    sFuelUnit = KW_PER_HOUR;
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        sFuelUnit = MCF_PER_HOUR;
                    }
                    else
                    {
                        sFuelUnit = M3_PER_HOUR;
                    }
                    break;
                default:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        sFuelUnit = GALLONS_PER_HOUR;
                    }
                    else
                    {
                        sFuelUnit = LITERS_PER_HOUR;
                    }
                    break;
            }
            return sFuelUnit;
        }
        
        /// <summary>
        /// Validate amounts
        /// </summary>
        /// <param name="attValue"></param>
        public static void ValidateAmounts(ref string attValue)
        {
            try
            {
                int iIndex;
                int iCount;
                int iLength;
                const int iMax = 1000000;
                const int iOne = 1;
                //1. get rid of non-numeric characters: letters, dollar signs, and commas

                string sDollar = "$";
                attValue = attValue.Replace(sDollar, "");
                string sComma = ",";
                attValue = attValue.Replace(sComma, "");

                string sPeriod = ".";
                iIndex = attValue.IndexOf(sPeriod);
                if (iIndex != -1)
                {
                    iLength = attValue.Length;
                    iCount = iLength - iIndex;
                    attValue = attValue.Remove(iIndex, iCount);
                }
                //2. if the attValue can't be converted to an integer, set it equal to one
                int iAmount = CalculatorHelpers.ConvertStringToInt(attValue);
                if (iAmount > iMax)
                {
                    attValue = iMax.ToString();
                }
                if (iAmount < 0)
                {
                    attValue = iOne.ToString();
                }
            }
            catch (Exception)
            {
                attValue = "1";
            }
        }
        
        /// <summary>
        /// Purpose: AAEA Equation 2.6 Fisher equation: calculate the interest rate given two out of three rates
        /// </summary>
        /// <param name="realRate"></param>
        /// <param name="inflationRate"></param>
        /// <param name="nomRate"></param>
        /// <returns>missing rate byref</returns>
        public static void MissingRate(ref double realRate, ref double nomRate,
            ref double inflationRate)
        {
            if (nomRate == 0)
            {
                nomRate = realRate + inflationRate + (inflationRate * realRate);
            }
            else if (realRate == 0)
            {
                realRate = (nomRate - inflationRate) / (1 + inflationRate);
            }
            else if (inflationRate == 0)
            {
                inflationRate = (nomRate - realRate) / (1 + realRate);
            }
        }

        /// <summary>
        /// Purpose: calculate the monthly interest rate given an annual nominal rate 
        /// </summary>
        /// <param name="annualRate"></param>
        /// <param name="numberOfMonths"></param>
        /// <returns>monthly interest rate</returns>
        public static double DiscountFactorByMonths(double annualRate, double numberOfMonths)
        {
            double dbDiscountFactorByMonths = 0;
            double dbMonthlyRate = 0;

            dbMonthlyRate = (System.Math.Pow((1 + annualRate), OneOverTwelve) - 1);
            //use a montly interest rate
            dbDiscountFactorByMonths = System.Math.Pow((1 + dbMonthlyRate), numberOfMonths);
            return dbDiscountFactorByMonths;
        }

        /// <summary>
        /// Purpose: AAEA Equation 5.8: Calculate cumulative repair and maintenance costs
        /// </summary>
        /// <param name="listPriceInitial">Pt(ListPriceInitial * NomRate) = machine initial list price in nominal dollars as of the end of the year</param>
        /// <param name="realRate"></param>
        /// <param name="inflationRate"></param>
        /// <param name="RF1">repair factor 1</param>
        /// <param name="RF2">repair factor 2</param>
        /// <param name="startingHrs">ht(StartingHrs + PlannedUseHrs)= accumulated machine use in hours at the end of the tth period</param>
        /// <param name="plannedUseHrs"></param>
        /// <returns>Crmt(CumRandM) = total cumulative repair and maintenance cost at the end of the year in dollars</returns>
        public static double CumRandM(double listPriceInitial, double realRate, double inflationRate,
            double RF1, double RF2, double startingHrs, double plannedUseHrs)
        {
            double dbCumRandM = 0;
            double dbListPriceNom = 0;
            double dbAccMachHrs = 0;

            dbListPriceNom = listPriceInitial + (inflationRate * listPriceInitial);
            dbAccMachHrs = startingHrs + plannedUseHrs;

            dbCumRandM = RF1 * dbListPriceNom * (System.Math.Pow((dbAccMachHrs / 500), RF2));
            return dbCumRandM;
        }

        /// <summary>
        /// Purpose:	AAEA Equation 5.8: Average repair cost per hour
        /// Output:		RMCosth(AvgRepairCostHr) = average repair cost per hour
        /// Input:		ht(StartingHrs + PlannedUseHrs) = accumulated machine use in hours at the end of the tth period
        ///				Crmt(CumRepair) = total cumulative repair and maintenance cost at the end of the year
        /// </summary>
        /// <param name="cumRandM2"></param>
        /// <param name="startingHrs"></param>
        /// <param name="plannedUseHrs"></param>
        /// <returns></returns>
        public static double AvgRepairCostHr(double cumRandM2, double startingHrs, double plannedUseHrs)
        {
            double dbAvgRepairCostHr = 0;
            double dbAccMachHrs = 0;

            dbAccMachHrs = startingHrs + plannedUseHrs;
            if (dbAccMachHrs == 0)
            {
                dbAvgRepairCostHr = (double)(-1);
                return dbAvgRepairCostHr;
            }
            dbAvgRepairCostHr = cumRandM2 / dbAccMachHrs;
            return dbAvgRepairCostHr;
        }

        /// <summary>
        ///Purpose: AAEA Equation 5.10: repair cost for year t in dollars
        ///Output:    RCt(RepairCostYr) = repair cost for year t in dollars
        ///Input:
        ///           Pt(ListPriceInitial * NomRate) = machine initial list price in nominal dollars as of the end of the year
        ///           RF1 = repair factor 1
        ///           RF2 = repair factor 2
        ///           ht-1(StartingHrs)= accumulated machine use at beginning of year t in hours
        ///           ht(StartingHrs + PlannedUseHrs)= accumulated machine use in hours at the end of the tth period
        /// </summary>
        /// <param name="listPriceInitial"></param>
        /// <param name="realRate"></param>
        /// <param name="inflationRate"></param>
        /// <param name="RF1"></param>
        /// <param name="RF2"></param>
        /// <param name="startingHrs"></param>
        /// <param name="plannedUseHrs"></param>
        /// <returns></returns>
        public static double RepairCostYr(double listPriceInitial, double realRate, double inflationRate, double RF1, double RF2,
            double startingHrs, double plannedUseHrs)
        {
            double dbRepairCostYr = 0;
            double dbListPriceNom = 0;
            double dbAccMachHrs = 0;

            dbListPriceNom = listPriceInitial + (inflationRate * listPriceInitial);
            dbAccMachHrs = startingHrs + plannedUseHrs;

            dbRepairCostYr = RF1 * dbListPriceNom * (System.Math.Pow((dbAccMachHrs / 500), RF2) - System.Math.Pow((startingHrs / 500), RF2));
            return dbRepairCostYr;
        }
        /// <summary>
        /// most basic, but most incomplete, formula for repairs
        /// </summary>
        /// <param name="marketValue"></param>
        /// <param name="randMPercent"></param>
        /// <returns></returns>
        public static double RepairCostHr(double marketValue, double randMPercent, int plannedUseHrs)
        {
            double dbRepairCostHr = 0;
            if (randMPercent > 1) randMPercent = randMPercent / 100;
            if (plannedUseHrs == 0) plannedUseHrs = -1;
            dbRepairCostHr = (marketValue * randMPercent) / plannedUseHrs;
            return dbRepairCostHr;
        }
        /// <summary>
        ///Purpose: AAEA Equation 5.18 and 5.10b: amortized average annual repair cost
        ///Output:  ARC(AmortAvgAnnRepairCost) = amortized average annual repair cost
        ///Input:
        ///           ListPriceInitital = initial list price
        ///           InflationRate = inflation rate
        ///           RealRate = real interest rate
        ///           RF1 = repair factor 1
        ///           RF2 = repair factor 2
        ///           startingPeriods = if the equipment if used will be > 0
        ///           UsefulLifeHrs = expected life span of the machine in hours
        ///           iPlannedUsePeriods = expected annual hours of machine use
        /// </summary>
        /// <param name="timeType"></param>
        /// <param name="inflationType"></param>
        /// <param name="listPriceInitial"></param>
        /// <param name="inflationRate"></param>
        /// <param name="realRate"></param>
        /// <param name="RF1"></param>
        /// <param name="RF2"></param>
        /// <param name="startingPeriods"></param>
        /// <param name="plannedUsePeriods"></param>
        /// <param name="usefulLifeHrs"></param>
        /// <param name="useOnlyTimePeriod"></param>
        /// <returns></returns>
        public static double AmortAvgRepairCostHr(TIME_TYPES timeType, INFLATION_TYPES inflationType,
            double listPriceInitial, double inflationRate,
            double realRate, double RF1, double RF2, int startingPeriods,
            int plannedUsePeriods, int usefulLifeHrs, int useOnlyTimePeriod)
        {
            double dbAmortAvgRepairCostHr = 0;
            int iAccMachHrs = 0;
            double dbInfAdjCostEndYrTMinus1 = 0;
            double dbAccCostEndYrT = 0;
            double dbDiscCostBeginYrT = 0;
            double dbSumCosts = 0;
            double dbDivisor = 0;
            double dbAnnuityFactor = 1;
            double dbListPriceYrTMinus1 = 0;
            double dbFirstYrRate = 0;
            int iTimeUsed = 0;
            int iNewStartingPeriods = 0;
            double dbSumUsedCosts = 0;
            int iRemainingHrs = 0;
            int iTimePeriods = 0;
            double dbInflationInit = 0;


            //negative one means division by zero violatio
            if (plannedUsePeriods == 0)
            {
                dbAmortAvgRepairCostHr = (double)(-1);
                return dbAmortAvgRepairCostHr;
            }

            //is it used or new equipment?
            if (startingPeriods > 1)
            {
                //first time period
                iTimeUsed = (startingPeriods / plannedUsePeriods);
                iRemainingHrs = (usefulLifeHrs - startingPeriods);
            }
            if (iRemainingHrs == 0)
            {
                iRemainingHrs = 1;
                if ((timeType == TIME_TYPES.costsvary) || (timeType == TIME_TYPES.costsandoutputsvary))
                {
                    iTimePeriods = (usefulLifeHrs / plannedUsePeriods);
                    iAccMachHrs = plannedUsePeriods;
                    //option of only using accumulated repairs through a specific time period
                    if (useOnlyTimePeriod != 0)
                    {
                        iTimePeriods = useOnlyTimePeriod;
                    }
                }
                else if (timeType == TIME_TYPES.costdonotvary)
                {
                    //set time periods to one
                    iTimePeriods = usefulLifeHrs / usefulLifeHrs;
                    iAccMachHrs = usefulLifeHrs;
                }
            }

            //if no inflation is to be used
            if (inflationType == INFLATION_TYPES.inflationno)
            {
                inflationRate = 0;
                dbInflationInit = 0;
            }
            else
            {
                dbInflationInit = inflationRate;
            }

            if (iTimePeriods == 0)
            {
                //negative numbers returned to client
                dbAmortAvgRepairCostHr = (double)(-1);
                return dbAmortAvgRepairCostHr;
            }
            //the following procedure will return same answers as tables 5.3, 5.4, 5.5,
            //the examples for 5.8, and the used machinery examples
            for (int t = 1; t <= iTimePeriods; t++)
            {
                //adjust the initital list price for this year//s inflation, if any
                listPriceInitial = listPriceInitial * (1 + inflationRate);
                //see equation 5.10b for numerator
                dbAccCostEndYrT = (RF1 * listPriceInitial * System.Math.Pow((iAccMachHrs / 1000.00), RF2));
                dbInfAdjCostEndYrTMinus1 = (RF1 * dbListPriceYrTMinus1 * System.Math.Pow((iNewStartingPeriods / 1000.00), RF2));
                //see equation 5.18 for divisor and inflation options
                if (inflationType == INFLATION_TYPES.inflationyesfirst)
                {
                    if (t == 1)
                    {
                        dbDivisor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                        dbFirstYrRate = dbDivisor;
                    }
                    else
                    {
                        //need to include the first year inflated discount rate
                        dbDivisor = ((System.Math.Pow((1 + inflationRate), (t - 1)) * System.Math.Pow((1 + realRate), (t - 1))) * dbFirstYrRate);
                    }
                }
                else if (inflationType == INFLATION_TYPES.inflationyesall)
                {
                    //don't make any special provisions for first year
                    dbInfAdjCostEndYrTMinus1 = dbInfAdjCostEndYrTMinus1 * (1 + inflationRate);
                    dbDivisor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                }
                else if (inflationType == INFLATION_TYPES.inflationno)
                {
                    dbDivisor = (System.Math.Pow((1 + realRate), t));
                }

                //account for option to not vary costs over time
                if (iTimePeriods == 1)
                {
                    dbDivisor = 1;
                }

                //final equation
                dbDiscCostBeginYrT = (dbAccCostEndYrT - dbInfAdjCostEndYrTMinus1) / dbDivisor;

                //adjust variables that change with time
                dbSumCosts = dbSumCosts + dbDiscCostBeginYrT;

                //if it's used, store the costs at end of the starting hours
                if (iTimeUsed > 1)
                {
                    if (t == iTimeUsed)
                    {
                        dbSumUsedCosts = dbSumCosts;
                    }
                }
                iNewStartingPeriods = iNewStartingPeriods + plannedUsePeriods;
                iAccMachHrs = iAccMachHrs + plannedUsePeriods;
                //the end of this year has already been adjusted for inflation
                dbListPriceYrTMinus1 = listPriceInitial;
                //if inflation is used only for the first year's costs
                if ((inflationType == INFLATION_TYPES.inflationyesfirst) && (iTimePeriods > 1))
                {
                    inflationRate = 0;
                }
            }//for t
            //if time varies, convert to annuity and account for used vs. new equipment
            if (iTimeUsed > 1)
            {
                //used machinery
                if (iTimePeriods > 1)
                {
                    //subtract the used cost discounted sum (yrt) from the total discounted sum (yrn)
                    dbSumCosts = dbSumCosts - dbSumUsedCosts;
                    //amortize this remainder over the remaining service life of the equipment
                    dbAnnuityFactor = CapRecovFactor((iTimePeriods - iTimeUsed), realRate, 0);
                    dbSumCosts = dbSumCosts * dbAnnuityFactor;
                    dbAmortAvgRepairCostHr = dbSumCosts / plannedUsePeriods;
                }
                else
                {
                    //the used costs will not have been set in for...next loop, set it here
                    dbSumUsedCosts = ((RF1 * listPriceInitial * System.Math.Pow((startingPeriods / 1000.00), RF2))) * (1 + dbInflationInit);
                    dbAmortAvgRepairCostHr = (dbSumCosts - dbSumUsedCosts) / iRemainingHrs;
                }
            }
            else
            {
                //new machinery
                if (iTimePeriods > 1)
                {
                    //convert to annuity
                    dbAnnuityFactor = CapRecovFactor(iTimePeriods, realRate, 0);
                    dbAmortAvgRepairCostHr = (dbSumCosts * dbAnnuityFactor) / plannedUsePeriods;
                }
                else
                {
                    dbAmortAvgRepairCostHr = dbSumCosts / usefulLifeHrs;
                }
            }
            if (timeType == TIME_TYPES.costsvary)
            {
                //added for version 1.1; the sums are correct and the annuity is correct; just add inflation to that annuity
                if ((inflationType == INFLATION_TYPES.inflationyesfirst) || (inflationType == INFLATION_TYPES.inflationyesall))
                {
                    dbAmortAvgRepairCostHr = dbAmortAvgRepairCostHr * (1 + dbInflationInit);
                }
            }
            return dbAmortAvgRepairCostHr;
        }
        /// <summary>
        /// Returns the capital recovery factor given a specific rate
        /// </summary>
        /// <param name="periods"></param>
        /// <param name="realRate"></param>
        /// <param name="nomRate"></param>
        /// <returns></returns>
        public static double CapRecovFactor(double periods, double realRate, double nomRate)
        {
            double dbCapRecovFactor = 0;
            if (realRate != 0)
            {
                dbCapRecovFactor = realRate / (1 - (1 / System.Math.Pow((1 + realRate), periods)));
            }
            else if (nomRate != 0)
            {
                dbCapRecovFactor = nomRate / (1 - (1 / System.Math.Pow((1 + nomRate), periods)));
            }
            return dbCapRecovFactor;
        }

        //<summary>
        //Purpose: AAEA equations 5.19: used for an enterprise wide average annual fuel consumption
        //Output:  average annual fuel consumption in GallonOrLiters per hour
        //Input:
        //         FuelType = options are gas, diesel, or lpg
        //         PTOmax(HPPTOmax)  = maximum PTO horsepower per hour
        //</summary>
        //<param name="unitType"></param>
        //<param name="fuelType"></param>
        //<param name="PTOMax"></param>
        //<param name="priceGasGallonOrLiter"></param>
        //<param name="priceDieselGallonOrLiter"></param>
        //<param name="priceLPGGallonOrLiter"></param>
        //<param name="fuelAmountPerHour"></param>
        //<returns></returns>
        public static double FuelAvgCostHr(UNIT_TYPES unitType, string fuelType,
            double PTOMax, double priceGasGallonOrLiter, double priceDieselGallonOrLiter,
            double priceLPGGallonOrLiter, ref double fuelAmountPerHour)
        {
            double dbFuelAvgCostHr = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        fuelAmountPerHour = 0.06 * PTOMax * 0.7457; //see ASAE conversion from hp-h to kw-h
                        dbFuelAvgCostHr = fuelAmountPerHour * priceGasGallonOrLiter;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelAmountPerHour = 0.06 * PTOMax;
                        dbFuelAvgCostHr = fuelAmountPerHour * priceGasGallonOrLiter;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        fuelAmountPerHour = 0.06 * PTOMax * 0.73 * 0.7457;
                        dbFuelAvgCostHr = fuelAmountPerHour * priceDieselGallonOrLiter;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelAmountPerHour = 0.06 * PTOMax * 0.73;
                        dbFuelAvgCostHr = fuelAmountPerHour * priceDieselGallonOrLiter;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        fuelAmountPerHour = 0.06 * PTOMax * 1.2 * 0.7457;
                        dbFuelAvgCostHr = fuelAmountPerHour * priceLPGGallonOrLiter;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelAmountPerHour = 0.06 * PTOMax * 1.2;
                        dbFuelAvgCostHr = fuelAmountPerHour * priceLPGGallonOrLiter;
                    }
                    break;
                default:
                    fuelAmountPerHour = 0;
                    dbFuelAvgCostHr = 0;
                    break;
            }
            return dbFuelAvgCostHr;
        }

        //<summary>
        //Purpose: AAEA equations 5.20 and 5.21: used to calculate an operation//s fuel consumption
        //Output:  fuel cost in dollars per hour
        //Input:
        //         FuelType = options are gas, diesel, or lpg
        //         PTOmax(HPPTOmax)  = maximum PTO horsepower per hour
        //         PTOEquiv = equivalent PTO hp
        //         3 fuel prices
        //</summary>
        //<param name="unitType"></param>
        //<param name="fuelType"></param>
        //<param name="PTOMax"></param>
        //<param name="PTOEquiv"></param>
        //<param name="priceGasGallonOrLiter"></param>
        //<param name="priceDieselGallonOrLiter"></param>
        //<param name="priceLPGGallonOrLiter"></param>
        //<param name="fuelAmountPerHour"></param>
        //<returns></returns>
        public static double FuelCostHr(UNIT_TYPES unitType, string fuelType, double PTOMax, double PTOEquiv,
            double priceGasGallonOrLiter, double priceDieselGallonOrLiter, double priceLPGGallonOrLiter, ref double fuelAmountPerHour)
        {
            double dbFuelCostHr = 0;
            double dbFuelMult = 0;
            double dbPTOCalc = 0;
            //division by zero																																																																												  
            if (PTOMax == 0)
            {
                //negative numbers signify division by zero error
                dbFuelCostHr = (double)(-1);
                return dbFuelCostHr;
            }
            dbPTOCalc = PTOEquiv / PTOMax;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        dbFuelMult = (2.74 * dbPTOCalc) + 3.15 - (0.203 * System.Math.Pow((697 * dbPTOCalc), 0.5));
                        fuelAmountPerHour = PTOEquiv * dbFuelMult;
                        dbFuelCostHr = fuelAmountPerHour * priceGasGallonOrLiter;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        dbFuelMult = (0.54 * dbPTOCalc) + 0.62 - (0.04 * System.Math.Pow((697 * dbPTOCalc), 0.5));
                        fuelAmountPerHour = PTOEquiv * dbFuelMult;
                        dbFuelCostHr = fuelAmountPerHour * priceGasGallonOrLiter;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        dbFuelMult = (2.64 * dbPTOCalc) + 3.91 - (0.203 * System.Math.Pow(((738 * dbPTOCalc) + 173), 0.5));
                        fuelAmountPerHour = PTOEquiv * dbFuelMult;
                        dbFuelCostHr = fuelAmountPerHour * priceDieselGallonOrLiter;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        dbFuelMult = (0.52 * dbPTOCalc) + 0.77 - (0.04 * System.Math.Pow(((738 * dbPTOCalc) + 173), 0.5));
                        fuelAmountPerHour = PTOEquiv * dbFuelMult;
                        dbFuelCostHr = fuelAmountPerHour * priceDieselGallonOrLiter;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        dbFuelMult = (2.69 * dbPTOCalc) + 3.41 - (0.203 * System.Math.Pow((646 * dbPTOCalc), 0.5));
                        fuelAmountPerHour = PTOEquiv * dbFuelMult;
                        dbFuelCostHr = fuelAmountPerHour * priceLPGGallonOrLiter;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        dbFuelMult = (0.53 * dbPTOCalc) + 0.62 - (0.04 * System.Math.Pow((646 * dbPTOCalc), 0.5));
                        fuelAmountPerHour = PTOEquiv * dbFuelMult;
                        dbFuelCostHr = fuelAmountPerHour * priceLPGGallonOrLiter;
                    }
                    break;
                default:
                    fuelAmountPerHour = 0;
                    dbFuelCostHr = 0;
                    break;
            }
            return dbFuelCostHr;
        }
        public static double FuelCostHr2(UNIT_TYPES unitType, string fuelType, double energyUseHr, 
            double energyEff, double priceGas, double priceDiesel, double priceLPG,
            double priceElectric, double priceNaturalGas,
            ref double fuelAmountPerHour, out string fuelUnit)
        {
            //get fuel cost
            double dbFuelCostHr = 0;
            if (energyEff == 0) energyEff = -1;
            //i.e. 1.6 liters per hour = 2 liters/hr * (80 / 100)
            fuelAmountPerHour = energyUseHr * (energyEff / 100);
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            //get fuel unit
            fuelUnit = GetFuelUnit(eFuelPriceType, unitType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    dbFuelCostHr = fuelAmountPerHour * priceGas;
                    if (unitType == UNIT_TYPES.imperial)
                    {

                    }
                    else
                    {
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    dbFuelCostHr = fuelAmountPerHour * priceDiesel;
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    dbFuelCostHr = fuelAmountPerHour * priceLPG;
                    break;
                case FUEL_PRICE_TYPES.electric:
                    dbFuelCostHr = fuelAmountPerHour * priceElectric;
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    dbFuelCostHr = fuelAmountPerHour * priceNaturalGas;
                    break;
                default:
                    fuelAmountPerHour = 0;
                    dbFuelCostHr = 0;
                    break;
            }
            return dbFuelCostHr;
        }
        
        /// <summary>
        /// Martin et al., 2011
        /// </summary>
        public static double GetEnergyMultiplier(UNIT_TYPES unitType, string fuelType,
            out string fuelUnit)
        {
            fuelUnit = string.Empty;
            double dbEnergyMultiplier = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 8.66;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        dbEnergyMultiplier = 8.66;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 12.5;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        dbEnergyMultiplier = 12.5;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 6.89;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        dbEnergyMultiplier = 6.89;
                    }
                    break;
                case FUEL_PRICE_TYPES.electric:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "kwh";
                        dbEnergyMultiplier = .885;
                    }
                    else
                    {
                        //kwh
                        fuelUnit = "kwh";
                        dbEnergyMultiplier = .885;
                    }
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    //this is per thousand cubic feet of natural gas
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "mcf";
                        dbEnergyMultiplier = 61.7;
                    }
                    else
                    {
                        fuelUnit = "m3";
                        dbEnergyMultiplier = 61.7;
                    }
                    break;
                case FUEL_PRICE_TYPES.ethanol:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 5.85;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        dbEnergyMultiplier = 5.85;
                    }
                    break;
                case FUEL_PRICE_TYPES.gasohol:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 8.31;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        dbEnergyMultiplier = 8.31;
                    }
                    break;
                default:
                    dbEnergyMultiplier = 0;
                    break;
            }
            return dbEnergyMultiplier;
        }
        public static double GetEnergyBTUs(UNIT_TYPES unitType, string fuelType,
            out string fuelUnit)
        {
            fuelUnit = string.Empty;
            //Martin et al
            double dbBTUs = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    dbBTUs = 125000;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                    }
                    else
                    {
                        fuelUnit = "liters";
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    dbBTUs = 138690;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                    }
                    else
                    {
                        fuelUnit = "liters";
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    dbBTUs = 95475;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                    }
                    else
                    {
                        fuelUnit = "liters";
                    }
                    break;
                case FUEL_PRICE_TYPES.electric:
                    dbBTUs = 3412;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "kwh";
                    }
                    else
                    {
                        //kwh
                        fuelUnit = "kwh";
                    }
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    //this is per thousand cubic feet of natural gas
                    dbBTUs = 1020000;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "mcf";
                    }
                    else
                    {
                        fuelUnit = "m3";
                    }
                    break;
                case FUEL_PRICE_TYPES.ethanol:
                    dbBTUs = 84400;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                    }
                    else
                    {
                        fuelUnit = "liters";
                    }
                    break;
                case FUEL_PRICE_TYPES.gasohol:
                    dbBTUs = 120000;
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                    }
                    else
                    {
                        fuelUnit = "liters";
                    }
                    break;
                default:
                    dbBTUs = 0;
                    break;
            }
            return dbBTUs;
        }
        /// <summary>
        /// Page 5-38, equation 5.27
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="fuelType"></param>
        /// <returns></returns>
        public static double GetEnergyMultiplier(UNIT_TYPES unitType, string fuelType,
            out string fuelUnit, out double imperialToMetric)
        {
            fuelUnit = string.Empty;
            imperialToMetric = 1;
            double dbEnergyMultiplier = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 8.66;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        imperialToMetric = GallonToLiter;
                        dbEnergyMultiplier = 8.66;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 12.5;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        imperialToMetric = GallonToLiter;
                        dbEnergyMultiplier = 12.5;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "gallons";
                        dbEnergyMultiplier = 6.89;
                    }
                    else
                    {
                        fuelUnit = "liters";
                        imperialToMetric = GallonToLiter;
                        dbEnergyMultiplier = 6.89;
                    }
                    break;
                case FUEL_PRICE_TYPES.electric:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "kwh";
                        dbEnergyMultiplier = .885;
                    }
                    else
                    {
                        //kwh
                        fuelUnit = "kwh";
                        dbEnergyMultiplier = .885;
                        imperialToMetric = 1;
                    }
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    if (unitType == UNIT_TYPES.imperial)
                    {
                        fuelUnit = "mcf";
                        dbEnergyMultiplier = 61.7;
                    }
                    else
                    {
                        fuelUnit = "m3";
                        imperialToMetric = CubicFeetToCubicMeter / 1000;
                        dbEnergyMultiplier = 61.7;
                    }
                    break;
                default:
                    imperialToMetric = 1;
                    dbEnergyMultiplier = 1;
                    break;
            }
            return dbEnergyMultiplier;
        }
        public static double FuelCostHr(double fuelAmountHr, string fuelType, double priceGas,
            double priceDiesel, double priceLPG, double priceElectric, double priceNG)
        {
            double dbFuelCostHr = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    dbFuelCostHr = fuelAmountHr * priceGas;
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    dbFuelCostHr = fuelAmountHr * priceDiesel;
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    dbFuelCostHr = fuelAmountHr * priceLPG;
                    break;
                case FUEL_PRICE_TYPES.electric:
                    dbFuelCostHr = fuelAmountHr * priceElectric;
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    dbFuelCostHr = fuelAmountHr * priceNG;
                    break;
                default:
                    break;
            }
            return dbFuelCostHr;
        }
        //Purpose: adjust and amortize any cost that varies by time and inflation options
        //AAEA equation: 5.18 - modified for any cost that varies over time
        //Output:    amortized cost adjusted for time and inflation options
        //Input:
        //           CostToAdjust is equivalent to listprice initial in equation 5.18 = the cost being adjusted
        //           Hrs starting, planned, and useful = time period adjustments
        //           OptionsForInflation = which type of inflation adjustments to make
        public static double AmortAvgRemainingCostHr(INFLATION_TYPES inflationType, double listPriceInitial, double inflationRate,
            double realRate, int startingHrs, int plannedUseHrs, int usefulLifeHrs)
        {
            double dbAmortAvgRemainingCostHr = 0;
            int t = 1;
            double dbInfAdjCostEndYrTMinus1 = 0;
            double dbAccCostEndYrT = 0;
            double dbDiscCostBeginYrT = 0;
            double dbSumCosts = 0;
            double dbDivisor = 0;
            double dbAnnuityFactor = 0;
            double dbListPriceYrTMinus1 = 0;
            double dbFirstYrRate = 0;
            int iRemainingHrs = 0;
            int iTimePeriods = 0;

            //negative one means division by zero violation
            if (plannedUseHrs == 0)
            {
                dbAmortAvgRemainingCostHr = (double)(-1);
                return dbAmortAvgRemainingCostHr;
            }

            //just amortize the costs over remaining service life
            iRemainingHrs = usefulLifeHrs - startingHrs;
            iTimePeriods = iRemainingHrs / plannedUseHrs;
            if (iRemainingHrs == 0) iRemainingHrs = 1;
            dbSumCosts = 0;
            dbListPriceYrTMinus1 = 0;
            dbAnnuityFactor = 1;
            //if no inflation is to be used
            if (inflationType == INFLATION_TYPES.inflationno)
            {
                inflationRate = 0;
            }

            //************************************************************************************
            //the following procedure will return same answers as tables 5.3, 5.4, 5.5,
            //the examples for 5.8, and the used machinery examples
            //*************************************************************************************
            for (t = 1; t <= iTimePeriods; t++)
            {
                //adjust the initital price for this year's inflation, if any
                listPriceInitial = listPriceInitial * (1 + inflationRate);
                //see equation 5.10b for numerator
                dbAccCostEndYrT = listPriceInitial;
                dbInfAdjCostEndYrTMinus1 = dbListPriceYrTMinus1;

                //see equation 5.18 for divisor
                if (inflationType == INFLATION_TYPES.inflationyesfirst)
                {
                    if (t == 1)
                    {
                        dbDivisor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                        dbFirstYrRate = dbDivisor;
                    }
                    else
                    {
                        //need to include the first year inflated discount rate
                        dbDivisor = (System.Math.Pow((1 + inflationRate), (t - 1)) * System.Math.Pow((1 + realRate), (t - 1))) * (dbFirstYrRate);
                    }
                }
                else
                {
                    //don't make any special provisions for first year
                    dbDivisor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                }

                //account for option to not vary costs over time
                if (iTimePeriods == 1)
                {
                    dbDivisor = 1;
                }
                //final equation
                if (t == 1)
                {
                    dbDiscCostBeginYrT = (dbAccCostEndYrT - dbInfAdjCostEndYrTMinus1) / dbDivisor;
                }
                else
                {
                    dbDiscCostBeginYrT = (dbAccCostEndYrT + (dbAccCostEndYrT - dbInfAdjCostEndYrTMinus1)) / dbDivisor;
                }

                //adjust variables that change with time
                dbSumCosts = dbSumCosts + dbDiscCostBeginYrT;

                //the end of this year has already been adjusted for inflation
                dbListPriceYrTMinus1 = listPriceInitial;
                //if inflation is used only for the first year's costs
                if (inflationType == INFLATION_TYPES.inflationyesfirst)
                {
                    inflationRate = 0;
                }
            }//end for t

            //convert to annuity
            if (iTimePeriods > 1)
            {
                //convert to annuity
                dbAnnuityFactor = CapRecovFactor(iTimePeriods, realRate, 0);
                dbAmortAvgRemainingCostHr = (dbSumCosts * dbAnnuityFactor) / plannedUseHrs;
            }
            else
            {
                dbAmortAvgRemainingCostHr = (dbSumCosts * dbAnnuityFactor) / plannedUseHrs;
            }

            return dbAmortAvgRemainingCostHr;
        }


        /// <summary>
        /// Purpose: unit cost formula accounts for varying output and costs over remaining service life
        ///	AAEA equation: 6A.3 - varying output and cost
        ///	Output:    unit cost
        ///	Input: outputs, costs, interest rates and options, multipliers
        /// </summary>
        /// <param name="inflationType"></param>
        /// <param name="PVMultiplier"></param>
        /// <param name="marketValue"></param>
        /// <param name="salvageValue"></param>
        /// <param name="inflationRate"></param>
        /// <param name="nomRate"></param>
        /// <param name="realRate"></param>
        /// <param name="output"></param>
        /// <param name="operatingCost"></param>
        /// <param name="outputMultiplier"></param>
        /// <param name="opCostMultiplier"></param>
        /// <param name="startingHrs"></param>
        /// <param name="plannedUseHrs"></param>
        /// <param name="usefulLifeHrs"></param>
        /// <param name="yrToStartMultipliers"></param>
        /// <param name="useOnlyTimePeriod"></param>
        /// <returns></returns>
        public static double UnitCost(INFLATION_TYPES inflationType, double PVMultiplier, double marketValue, double salvageValue, double inflationRate,
            double nomRate, double realRate, double output, double operatingCost, double outputMultiplier, double opCostMultiplier,
            int startingHrs, int plannedUseHrs, int usefulLifeHrs, int yrToStartMultipliers, int useOnlyTimePeriod)
        {
            double dbUnitCost = 0;
            int t = 0;
            double dbSumCosts = 0;
            double dbSumOutput = 0;
            double dbAnnuityFactor = 1;
            double dbFirstYrRate = 0;
            int iRemainingHrs = 0;
            int iTimePeriods = 0;
            double dbPVSalvage = 0;
            double dbDiscCostBeginYrT = 0;
            double dbDiscOutputBeginYrT = 0;

            //set up the outputs to use
            if (output == 0)
            {
                //if no output has been sent, the output will be quality adjusted service hours of use
                output = (double)plannedUseHrs;
            }

            //just amortize the costs over remaining service life
            iRemainingHrs = usefulLifeHrs - startingHrs;
            iTimePeriods = iRemainingHrs / plannedUseHrs;
            if (iRemainingHrs == 0) iRemainingHrs = 1;
            //option of only using accumulated remaining costs through a specific time period
            if (useOnlyTimePeriod != 0)
            {
                if (useOnlyTimePeriod < iTimePeriods)
                {
                    iTimePeriods = useOnlyTimePeriod;
                }
                else
                {
                    //leave time periods as set
                }
            }
            //if no inflation is to be used
            if (inflationType == INFLATION_TYPES.inflationno)
            {
                inflationRate = 0;
            }

            //the following procedure attempts to match formula 6A.3

            //Step 1 - handle discounted market and salvage values
            dbPVSalvage = CapRecoverCostAnn(inflationType, marketValue, inflationRate, realRate, nomRate,
                startingHrs, usefulLifeHrs, plannedUseHrs, salvageValue);

            //Step 2 - sum, weight and adjust output and costs
            for (t = 1; t <= iTimePeriods; t++)
            {
                //adjust the initital price for this year's inflation, if any
                operatingCost = operatingCost * (1 + inflationRate);

                //determine beta - the discount weight to use for the year
                //see equation 5.18 for divisor and inflation options
                if (inflationType == INFLATION_TYPES.inflationyesfirst)
                {
                    if (t == 1)
                    {
                        dbAnnuityFactor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                        dbFirstYrRate = dbAnnuityFactor;
                    }
                    else
                    {
                        //need to include the first year inflated discount rate
                        dbAnnuityFactor = (System.Math.Pow((1 + inflationRate), (t - 1)) * System.Math.Pow((1 + realRate), (t - 1))) * (dbFirstYrRate);
                    }
                }
                else if (inflationType == INFLATION_TYPES.inflationyesall)
                {
                    //don't make any special provisions for first year
                    dbDiscCostBeginYrT = dbDiscCostBeginYrT * (1 + inflationRate);
                    dbAnnuityFactor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                }
                else if (inflationType == INFLATION_TYPES.inflationno)
                {
                    dbAnnuityFactor = System.Math.Pow((1 + realRate), t);
                }

                //divide by one for discounting
                dbAnnuityFactor = 1 / dbAnnuityFactor;

                //weight and multiply
                if (t == yrToStartMultipliers)
                {
                    dbDiscCostBeginYrT = dbAnnuityFactor * operatingCost * opCostMultiplier;
                    dbDiscOutputBeginYrT = dbAnnuityFactor * output * outputMultiplier;
                }
                else
                {
                    dbDiscCostBeginYrT = dbAnnuityFactor * operatingCost;
                    dbDiscOutputBeginYrT = dbAnnuityFactor * output;
                }

                //adjust variables that change with time
                dbSumCosts = dbSumCosts + dbDiscCostBeginYrT;
                dbSumOutput = dbSumOutput + dbDiscOutputBeginYrT;

                //if inflation is used only for the first year//s costs
                if (inflationType == INFLATION_TYPES.inflationyesfirst)
                {
                    inflationRate = 0;
                }
            }//for t
            //unit costs
            dbUnitCost = ((PVMultiplier * dbPVSalvage) - dbSumCosts) / dbSumOutput;
            return dbUnitCost;
        }

        /// <summary>
        /// Purpose: AAEA Equation 6.7: capital recovery or service cost using original asset value
        /// Output:    CSC(CapRecoverCost) = capital recovery cost
        /// Input:
        ///          InflationRate = inflation rate
        ///          RealRate = real interest rate
        ///          UsefulLifeHrs = useful life of the asset in hours
        ///          PlannedUseHrs = planned annual hrs of use
        ///          Market value = market value of machine at time of these hours
        ///          PurchasePriceAdj = if list prices are used, adjustment factor to decrease list prices to purchase prices
        ///          SalvVal = salvage value of the asset at the end of period n
        /// </summary>
        /// <param name="inflationType"></param>
        /// <param name="marketValue"></param>
        /// <param name="inflationRate"></param>
        /// <param name="realRate"></param>
        /// <param name="nomRate"></param>
        /// <param name="startingHrs"></param>
        /// <param name="usefulLifeHrs"></param>
        /// <param name="plannedUseHrs"></param>
        /// <param name="salvVal"></param>
        /// <returns></returns>
        public static double CapRecoverCostAnn(INFLATION_TYPES inflationType, 
            double marketValue, double inflationRate, double realRate, double nomRate,
            double startingHrs, double usefulLifeHrs, double plannedUseHrs,
            double salvVal)
        {
            double dbCapRecoverCostAnn = 0;
            int iYrsLife = 0;
            double CRF = 0;
            double dbRemainingHrs = 0;

            if (plannedUseHrs == 0)
            {
                dbCapRecoverCostAnn = (double)(-1);
                return dbCapRecoverCostAnn;
            }

            //using market values, so use remaining life as time periods to amortize over
            dbRemainingHrs = (double)usefulLifeHrs - startingHrs;
            if (dbRemainingHrs == 0) dbRemainingHrs = 1;
            iYrsLife = (int)(dbRemainingHrs / plannedUseHrs);
            if (iYrsLife == 0) iYrsLife = 1;

            //rates to use depend on inflation option chosen
            if (inflationType == INFLATION_TYPES.inflationno)
            {
                //use real rates
                CRF = realRate / (1 - (1 / System.Math.Pow((1 + realRate), iYrsLife)));
                dbCapRecoverCostAnn = (marketValue - (salvVal / System.Math.Pow((1 + realRate), iYrsLife))) * CRF;
            }
            else if ((inflationType == INFLATION_TYPES.inflationyesfirst))
            {
                CRF = realRate / (1 - (1 / System.Math.Pow((1 + realRate), iYrsLife)));
                dbCapRecoverCostAnn = (marketValue - (salvVal / System.Math.Pow((1 + realRate), iYrsLife))) * CRF;
                //adjust this rate by an inflation factor in first year only
                dbCapRecoverCostAnn = dbCapRecoverCostAnn * (1 + inflationRate);
            }
            else if ((inflationType == INFLATION_TYPES.inflationyesall))
            {
                //use nominal rates
                CRF = nomRate / (1 - (1 / System.Math.Pow((1 + nomRate), iYrsLife)));
                dbCapRecoverCostAnn = (marketValue - (salvVal / System.Math.Pow((1 + nomRate), iYrsLife))) * CRF;
            }

            return dbCapRecoverCostAnn;

        }
        public static double CapRecoverCostAnn(double marketValue, 
            double realRate, double nomRate, double years, double salvVal)
        {
            double dbCapRecoverCostAnn = 0;
            double CRF = 0;

            if (realRate > 0)
            {
                //use real rates
                CRF = realRate / (1 - (1 / System.Math.Pow((1 + realRate), years)));
                dbCapRecoverCostAnn = (marketValue - (salvVal / System.Math.Pow((1 + realRate), years))) * CRF;
            }
            else 
            {
                //use nominal rates
                CRF = nomRate / (1 - (1 / System.Math.Pow((1 + nomRate), years)));
                dbCapRecoverCostAnn = (marketValue - (salvVal / System.Math.Pow((1 + nomRate), years))) * CRF;
            }

            return dbCapRecoverCostAnn;

        }
        /// <summary>
        /// Purpose: AAEA equations 5.22 : used to calculate an operation//s oil consumption
        /// Output:  fuel consumption in GallonOrLiters per hour
        /// Input:
        ///          FuelType = options are gas, diesel, or lpg
        ///          HP  = rated engine horsepower
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="fuelType"></param>
        /// <param name="HP"></param>
        /// <param name="priceOil"></param>
        /// <returns></returns>
        public static void LubeOilCostHr(UNIT_TYPES unitType, string fuelType,
            double HP, double priceOil, out double lubeOilAmount, 
            out string lubeOilUnit, out double lubeOilCostHr)
        {
            lubeOilAmount = 0;
            lubeOilUnit = GetLubeOilUnit(unitType);
            lubeOilCostHr = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        lubeOilAmount = ((0.000566 * HP) + 0.02487);
                        lubeOilCostHr = lubeOilAmount * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilAmount = ((0.00011 * HP) + 0.00657);
                        lubeOilCostHr = lubeOilAmount * priceOil;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        lubeOilAmount = ((0.00059 * HP) + 0.02169);
                        lubeOilCostHr = lubeOilAmount * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilAmount = ((0.00021 * HP) + 0.00573);
                        lubeOilCostHr = lubeOilAmount * priceOil;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    if (unitType == UNIT_TYPES.metric)
                    {
                        lubeOilAmount = ((0.00041 * HP) + 0.02);
                        lubeOilCostHr = lubeOilAmount * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilAmount = ((0.00008 * HP) + 0.00755);
                        lubeOilCostHr = lubeOilAmount * priceOil;
                    }
                    break;
                default:
                    lubeOilAmount = 0;
                    lubeOilCostHr = 0;
                    break;

            }
        }
        /// <summary>
        /// Page 5-40 AAEA Guidelines
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="fuelType"></param>
        /// <param name="waterHP"></param>
        /// <param name="priceOil"></param>
        /// <returns></returns>
        public static void IrrLubeOilCostHr(UNIT_TYPES unitType, string fuelType, double waterHP, double priceOil,
            out double lubeOilHr, out string lubeOilUnit, out double lubeOilCostHr)
        {
            lubeOilHr = 0;
            lubeOilUnit = string.Empty;
            lubeOilCostHr = 0;
            double dbOilMultiplierPowerUnit = 0;
            const int iOilMultiplierGearDrive = 4000;
            lubeOilUnit = GetLubeOilUnit(unitType);
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    //step 1. Equation 5.32: set oil multiplier
                    dbOilMultiplierPowerUnit = 800;
                    //step 2. Equation 5.31: oil consumed, gallons/hour
                    lubeOilHr = (waterHP / dbOilMultiplierPowerUnit) + (waterHP / iOilMultiplierGearDrive);
                    if (unitType == UNIT_TYPES.metric)
                    {
                        //convert gallons to liters
                        lubeOilHr = lubeOilHr * GallonToLiter;
                        //step 3. Equation 5.33: lubrication costs per hour
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    //step 1. Equation 5.32: set oil multiplier
                    dbOilMultiplierPowerUnit = 900;
                    //step 2. Equation 5.31: oil consumed, gallons/hour
                    lubeOilHr = (waterHP / dbOilMultiplierPowerUnit) + (waterHP / iOilMultiplierGearDrive);
                    if (unitType == UNIT_TYPES.metric)
                    {
                        //convert gallons to liters
                        lubeOilHr = lubeOilHr * GallonToLiter;
                        //step 3. Equation 5.33: lubrication costs per hour
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    //step 1. Equation 5.32: set oil multiplier
                    dbOilMultiplierPowerUnit = 800;
                    //step 2. Equation 5.31: oil consumed, gallons/hour
                    lubeOilHr = (waterHP / dbOilMultiplierPowerUnit) + (waterHP / iOilMultiplierGearDrive);
                    if (unitType == UNIT_TYPES.metric)
                    {
                        //convert gallons to liters
                        lubeOilHr = lubeOilHr * GallonToLiter;
                        //step 3. Equation 5.33: lubrication costs per hour
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    break;
                case FUEL_PRICE_TYPES.electric:
                    //step 1. Equation 5.32: set oil multiplier
                    dbOilMultiplierPowerUnit = 4000;
                    //step 2. Equation 5.31: oil consumed, gallons/hour
                    lubeOilHr = (waterHP / dbOilMultiplierPowerUnit) + (waterHP / iOilMultiplierGearDrive);
                    if (unitType == UNIT_TYPES.metric)
                    {
                        //convert gallons to liters
                        lubeOilHr = lubeOilHr * GallonToLiter;
                        //step 3. Equation 5.33: lubrication costs per hour
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    //step 1. Equation 5.32: set oil multiplier
                    dbOilMultiplierPowerUnit = 800;
                    //step 2. Equation 5.31: oil consumed, gallons/hour
                    lubeOilHr = (waterHP / dbOilMultiplierPowerUnit) + (waterHP / iOilMultiplierGearDrive);
                    if (unitType == UNIT_TYPES.metric)
                    {
                        //convert gallons to liters
                        lubeOilHr = lubeOilHr * GallonToLiter;
                        //step 3. Equation 5.33: lubrication costs per hour
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    else if (unitType == UNIT_TYPES.imperial)
                    {
                        lubeOilCostHr = lubeOilHr * priceOil;
                    }
                    break;
                default:
                    lubeOilHr = 0;
                    lubeOilCostHr = 0;
                    break;
            }
        }
        private static string GetLubeOilUnit(UNIT_TYPES unitType)
        {
            string sLubeOilUnit = string.Empty;
            if (unitType == UNIT_TYPES.metric)
            {
                sLubeOilUnit = "liter";
            }
            else if (unitType == UNIT_TYPES.imperial)
            {
                sLubeOilUnit = "gallon";
            }
            return sLubeOilUnit;
        }
        /// <summary>
        ///AAEA  McGrann method
        /// </summary>
        /// <param name="fuelType"></param>
        /// <param name="priceLabor"></param>
        /// <returns></returns>
        public static double IrrRepairCostHr(string fuelType, double marketValue, int plannedUseHrs)
        {
            double dbRepairFactor = 0;
            double dbRepairCostHr = 0;
            if (plannedUseHrs < 0) plannedUseHrs = plannedUseHrs * -1;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    //step 1. Table 5.6: set repair factors
                    dbRepairFactor = .07;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (marketValue * dbRepairFactor) / (plannedUseHrs);
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    //step 1. Table 5.6: set repair factors
                    dbRepairFactor = .07;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (marketValue * dbRepairFactor) / (plannedUseHrs);
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    //step 1. Table 5.6: set repair factors
                    dbRepairFactor = .055;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (marketValue * dbRepairFactor) / (plannedUseHrs);
                    break;
                case FUEL_PRICE_TYPES.electric:
                    //step 1. Table 5.6: set repair factors
                    dbRepairFactor = .02;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (marketValue * dbRepairFactor) / (plannedUseHrs);
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    //step 1. Table 5.6: set repair factors
                    dbRepairFactor = .055;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (marketValue * dbRepairFactor) / (plannedUseHrs);
                    break;
                default:
                    dbRepairCostHr = 0;
                    break;
            }
            return dbRepairCostHr;
        }
        /// <summary>
        /// AAEA Selley method
        /// </summary>
        /// <param name="fuelType"></param>
        /// <param name="priceLabor"></param>
        /// <param name="plannedUseHours"></param>
        /// <returns></returns>
        public static double IrrSelleyRepairCostHr(string fuelType, double priceLabor)
        {
            double dbRepairFactor = 0;
            double dbRepairCostHr = 0;
            int iLaborHrs = 0;
            FUEL_PRICE_TYPES eFuelPriceType = GetFuelPriceType(fuelType);
            switch (eFuelPriceType)
            {
                case FUEL_PRICE_TYPES.gas:
                    //step 1. Table 5.7: set repair and labor factors
                    dbRepairFactor = 3.15;
                    iLaborHrs = 40;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (dbRepairFactor / 1000) + ((priceLabor * iLaborHrs) / 1000);
                    break;
                case FUEL_PRICE_TYPES.diesel:
                    //step 1.  Table 5.7: set repair factor
                    dbRepairFactor = 5.00;
                    iLaborHrs = 20;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (dbRepairFactor / 1000) + ((priceLabor * iLaborHrs) / 1000);
                    break;
                case FUEL_PRICE_TYPES.lpg:
                    //step 1. Table 5.7: set repair factor
                    dbRepairFactor = 2.40;
                    iLaborHrs = 40;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (dbRepairFactor / 1000) + ((priceLabor * iLaborHrs) / 1000);
                    break;
                case FUEL_PRICE_TYPES.electric:
                    //step 1. Table 5.7: set repair factor
                    dbRepairFactor = .62;
                    iLaborHrs = 20;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (dbRepairFactor / 1000) + ((priceLabor * iLaborHrs) / 1000);
                    break;
                case FUEL_PRICE_TYPES.naturalgas:
                    //step 1. Table 5.7: set repair factor
                    dbRepairFactor = 2.40;
                    iLaborHrs = 40;
                    //step 2. Page 5.35: hourly repair cost
                    dbRepairCostHr = (dbRepairFactor / 1000) + ((priceLabor * iLaborHrs) / 1000);
                    break;
                default:
                    dbRepairCostHr = 0;
                    break;
            }
            return dbRepairCostHr;
        }
        /// <summary>
        /// Purpose: AAEA Table 6.4: remaining value as a percent of list price
        /// Output:  remaining value at the end of n years of age
        /// Input:
        ///          PlannedUseHrs = annual hours of use
        ///          UsefulLifeHrs = useful life in hours
        ///          C1, C2, C3 = ASAE constants
        /// </summary>
        /// <param name="plannedUseHrs"></param>
        /// <param name="startingHrs"></param>
        /// <param name="C1"></param>
        /// <param name="C2"></param>
        /// <param name="C3"></param>
        /// <returns></returns>
        public static double RemainValue(double plannedUseHrs, double startingHrs,
            double C1, double C2, double C3)
        {
            double dbRemainValue = 0;
            int iYrsOld = 0;																																				//calculate the effective life of the machine using the stored hours parameters
            iYrsOld = (int)(startingHrs / plannedUseHrs);
            if (iYrsOld == 0)
            {
                iYrsOld = 1;
                dbRemainValue = 50 * System.Math.Pow((C1 - (C2 * System.Math.Pow(iYrsOld, 0.5)) - (C3 * plannedUseHrs)), 2);
            }
            return dbRemainValue;
        }
        /// <summary>
        /// Returns the capital recovery cost on an hourly basis
        /// </summary>
        /// <param name="capRecoverCost"></param>
        /// <param name="plannedUseHrs"></param>
        /// <returns></returns>
        public static double CapRecoverCostHr(double capRecoverCost, int plannedUseHrs)
        {
            double dbCapRecoverCostHr = 0;
            if (plannedUseHrs == 0)
            {
                dbCapRecoverCostHr = (double)(-1);
                return dbCapRecoverCostHr;
            }
            dbCapRecoverCostHr = capRecoverCost / plannedUseHrs;
            return dbCapRecoverCostHr;
        }

        /// <summary>
        /// Purpose: calculate taxes, housing, and Insurance costs
        /// Output:  THandICosts: (PP - SV (discounted) / 2) * cumulative percent rh&i
        /// Reference: page 14-15 in AAEA reference
        /// </summary>
        /// <param name="taxPercent"></param>
        /// <param name="insurePercent"></param>
        /// <param name="housingPercent"></param>
        /// <param name="marketValue"></param>
        /// <returns></returns>
        public static double THandICosts(double taxPercent, double insurePercent, double housingPercent, double marketValue,
            double salvageValue, double realRate, int startingPeriods, int plannedUseHrs, int usefulLifeHrs)
        {
            double dbTHandICosts = 0;
            int iSalvageValueLife = (usefulLifeHrs - startingPeriods) / plannedUseHrs;
            double dbDiscountedSalvageValue = salvageValue / (System.Math.Pow((1 + realRate), iSalvageValueLife));
            dbTHandICosts = ((marketValue + dbDiscountedSalvageValue) / 2) * (taxPercent + housingPercent + insurePercent);
            return dbTHandICosts;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tHandICosts"></param>
        /// <param name="plannedUseHrs"></param>
        /// <returns></returns>
        public static double THandICostsHr(double tHandICosts, int plannedUseHrs)
        {
            double dbTHandICostsHr = 0;
            if (plannedUseHrs == 0)
            {
                dbTHandICostsHr = (double)(-1);
                return dbTHandICostsHr;
            }
            dbTHandICostsHr = tHandICosts / plannedUseHrs;
            return dbTHandICostsHr;
        }

        //======================================================================================
        //Purpose: Equation 5.6; Acres per hour calculated capacity
        //Output:    Ca(AcsHr) = acres per hour calculated machinery
        //Input:
        //           S(FieldSpeedTypical) = implement speed in miles per hour
        //           W (Width) = measured width of the implement in feet
        //           Ef(FieldEffTypical) = field efficiency (the ratio of effective accomplishment _
        //to theoretical accomplishment expressed in percent
        //           8.25 = 43.560 (sq ft per acre) divided by 5280 (feet per mile) = width of acre 1 mile long
        public static double Capacity(UNIT_TYPES unitType, CAPACITY_TYPES capacityType,
            double fieldSpeedTypical, double width, double fieldEffTypical, double output)
        {

            double dbCapacity = 0;
            if (fieldEffTypical > 0.999)
            {
                fieldEffTypical = fieldEffTypical / 100;
            }

            //division by zero always returns negative numbers
            if (output == 0)
            {
                output = -1;
            }
            if (fieldSpeedTypical == 0)
            {
                fieldSpeedTypical = -1;
            }
            if (width == 0)
            {
                width = -1;
            }
            if (fieldEffTypical == 0)
            {
                fieldEffTypical = -1;
            }

            switch (unitType)
            {
                case UNIT_TYPES.metric:
                    if (capacityType == CAPACITY_TYPES.area)
                    {
                        dbCapacity = 1 / ((fieldSpeedTypical * width * fieldEffTypical) / HectareWidth);
                    }
                    else if (capacityType == CAPACITY_TYPES.material)
                    {
                        dbCapacity = 1 / ((fieldSpeedTypical * width * output * fieldEffTypical) / HectareWidth);
                    }
                    else if (capacityType == CAPACITY_TYPES.area2)
                    {
                        dbCapacity = (fieldSpeedTypical * width * fieldEffTypical) / HectareWidth;
                    }
                    else if (capacityType == CAPACITY_TYPES.material2)
                    {
                        dbCapacity = (fieldSpeedTypical * width * output * fieldEffTypical) / HectareWidth;
                    }
                    break;
                case UNIT_TYPES.imperial:
                    if (capacityType == CAPACITY_TYPES.area)
                    {
                        dbCapacity = 1 / ((fieldSpeedTypical * width * fieldEffTypical) / AcreWidth);
                    }
                    else if (capacityType == CAPACITY_TYPES.material)
                    {
                        dbCapacity = 1 / ((fieldSpeedTypical * width * output * fieldEffTypical) / AcreWidth);
                    }
                    else if (capacityType == CAPACITY_TYPES.area2)
                    {
                        dbCapacity = (fieldSpeedTypical * width * fieldEffTypical) / AcreWidth;
                    }
                    else if (capacityType == CAPACITY_TYPES.material2)
                    {
                        dbCapacity = (fieldSpeedTypical * width * output * fieldEffTypical) / AcreWidth;
                    }
                    break;
                default:
                    //defaults have been changed assume metric
                    if (capacityType == CAPACITY_TYPES.area)
                    {
                        dbCapacity = 1 / ((fieldSpeedTypical * width * fieldEffTypical) / HectareWidth);
                    }
                    else if (capacityType == CAPACITY_TYPES.material)
                    {
                        dbCapacity = 1 / ((fieldSpeedTypical * width * output * fieldEffTypical) / HectareWidth);
                    }
                    else if (capacityType == CAPACITY_TYPES.area2)
                    {
                        dbCapacity = (fieldSpeedTypical * width * fieldEffTypical) / HectareWidth;
                    }
                    else if (capacityType == CAPACITY_TYPES.material2)
                    {
                        dbCapacity = (fieldSpeedTypical * width * output * fieldEffTypical) / HectareWidth;
                    }
                    break;
            }
            return dbCapacity;
        }
        public static double AdjustLaborPercent(double percentage)
        {
            //when instructions say the percent entered will be divided by 100
            //5 / 100 = .05, but multiplier is 1.05 
            double dbPercentageMultiplier = 1 + (percentage / 100);
            return dbPercentageMultiplier;
        }
        /// <summary>
        /// Calculate a labor cost per hour
        /// </summary>
        /// <param name="laborType"></param>
        /// <param name="priceRegularLabor"></param>
        /// <param name="priceMachineryLabor"></param>
        /// <param name="priceSupervisoryLabor"></param>
        /// <param name="laborPriceAdj"></param>
        /// <returns></returns>
        public static double LaborCostHr(string laborType, double priceRegularLabor, 
            double priceMachineryLabor, double priceSupervisoryLabor,
            double laborAmountAdj, ref double laborAmount, out string laborUnit)
        {
            laborUnit = "hour";
            double dbLaborCostHr = 0;
            //calculate the labor amount adustment multiplier 
            //(note that instructions say it will be dividied by 100, so 20 / 100 = .2,
            //and .15 hrs/acre * .2 = .18 hours of labor (per acre)
            laborAmount = AdjustLaborPercent(laborAmountAdj);
            double dbLaborPrice = GetLaborPrice(laborType, priceRegularLabor, priceMachineryLabor,
                priceSupervisoryLabor);
            //labor cost per hour is price * amount
            dbLaborCostHr = dbLaborPrice * laborAmount;
            return dbLaborCostHr;
        }
        public static double GetLaborPrice(string laborType, double priceRegularLabor, 
            double priceMachineryLabor, double priceSupervisoryLabor)
        {
            double dbLaborPrice = 0;
            if (laborType == LABOR_PRICE_TYPES.none.ToString())
            {
                dbLaborPrice = 0;
            }
            else if (laborType == LABOR_PRICE_TYPES.machinery.ToString())
            {
                dbLaborPrice = priceMachineryLabor;
            }
            else if (laborType == LABOR_PRICE_TYPES.regular.ToString())
            {
                dbLaborPrice = priceRegularLabor;
            }
            else if (laborType == LABOR_PRICE_TYPES.supervisory.ToString())
            {
                dbLaborPrice = priceSupervisoryLabor;
            }
            return dbLaborPrice;
        }
        
        
        public static void GetInterestRates(XElement calculationsElement, 
            out double realRate, out double nominalRate, out double inflationRate)
        {
            realRate = 0;
            nominalRate = 0;
            inflationRate = 0;
            realRate = CalculatorHelpers.GetAttributeDouble(calculationsElement, REAL_RATE);
            nominalRate = CalculatorHelpers.GetAttributeDouble(calculationsElement, NOMINAL_RATE);
            inflationRate = CalculatorHelpers.GetAttributeDouble(calculationsElement, INFLATION_RATE);
            MissingRate(ref realRate, ref nominalRate, ref inflationRate);
        }
        public static int MinPTOHorsepower(int unitGroupId, double width, double speed, 
            double draftSoilMultiplier)
        {
            //file A3-28 Ag Decision Maker from Iowa State Cooperative Extension
            int hpPTOMax = 0;
            GeneralRules.UNIT_TYPES unitType = GeneralRules.GetUnitsEnum(unitGroupId);
            if (unitType == GeneralRules.UNIT_TYPES.metric)
            {
                double widthInFeet = width * MeterToFeet;
                double poundPerFeet = draftSoilMultiplier * KgPerMeterToPoundPerFoot;
                hpPTOMax = (int)(widthInFeet * speed * poundPerFeet) / 375;
            }
            else if (unitType == GeneralRules.UNIT_TYPES.imperial)
            {
                hpPTOMax = (int)(width * speed * draftSoilMultiplier) / 375;
            }
            return hpPTOMax;
        }
        public static double GetGradientRealDiscountValue(double cashValue, double interestRate, 
            double serviceLifeYears, double yearsFromBaseDate, double planningConstructionYears,
            GeneralRules.GROWTH_SERIES_TYPES growthType, double escalationRate, double discountFactor, 
            double discountYears, double discountYearTimes, double salvValue)
        {
            double dbGradientFactor = 0;
            double dbServicePlusPCYears = serviceLifeYears + planningConstructionYears;
            double dbPlanningConstructionPhaseFactor = 0;
            double dbAdjustedTotal = cashValue;
            //check for divide by zero violations
            if (interestRate == 0)
                interestRate = -1;
            if ((interestRate - escalationRate) == 0)
                interestRate = -1;
            if (growthType == GeneralRules.GROWTH_SERIES_TYPES.uniform)
            {
                //NIST 135 recurring uniform formula
                dbGradientFactor = (System.Math.Pow((1 + interestRate), dbServicePlusPCYears) - 1)
                    / (interestRate * System.Math.Pow((1 + interestRate), dbServicePlusPCYears));
                if (planningConstructionYears > 0)
                {
                    dbPlanningConstructionPhaseFactor = (System.Math.Pow((1 + interestRate), planningConstructionYears) - 1)
                    / (interestRate * System.Math.Pow((1 + interestRate), planningConstructionYears));
                    dbGradientFactor = dbGradientFactor - dbPlanningConstructionPhaseFactor;
                }
                dbAdjustedTotal = cashValue * dbGradientFactor;
                //no salvage value adjustment
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.linear)
            {
                //NIST 135 recurring nonuniform formula
                dbGradientFactor = ((1 + escalationRate) / (interestRate - escalationRate))
                    * (1 - (System.Math.Pow(((1 + escalationRate) / (1 + interestRate)), dbServicePlusPCYears)));
                if (planningConstructionYears > 0)
                {
                    dbPlanningConstructionPhaseFactor = ((1 + escalationRate) / (interestRate - escalationRate))
                        * (1 - (System.Math.Pow(((1 + escalationRate) / (1 + interestRate)), planningConstructionYears)));
                    dbGradientFactor = dbGradientFactor - dbPlanningConstructionPhaseFactor;
                }
                dbAdjustedTotal = cashValue * dbGradientFactor;
                //no salvage value adjustment
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.geometric)
            {
                //Chang 2.26 (adapted to the NIST way to handle pc period)
                if (interestRate == escalationRate)
                {
                    dbGradientFactor = dbServicePlusPCYears * (cashValue / (1 + interestRate));
                    dbPlanningConstructionPhaseFactor = planningConstructionYears * (cashValue / (1 + interestRate));
                    dbAdjustedTotal = dbGradientFactor - dbPlanningConstructionPhaseFactor;
                }
                else
                {
                    dbGradientFactor = (1 -
                        (System.Math.Pow((1 + escalationRate), dbServicePlusPCYears) * System.Math.Pow((1 + interestRate), (dbServicePlusPCYears * -1))))
                        / (interestRate - escalationRate);
                    if (planningConstructionYears > 0)
                    {
                        dbPlanningConstructionPhaseFactor = (1 -
                            (System.Math.Pow((1 + escalationRate), planningConstructionYears) * System.Math.Pow((1 + interestRate), (planningConstructionYears * -1))))
                            / (interestRate - escalationRate);
                        dbGradientFactor = dbGradientFactor - dbPlanningConstructionPhaseFactor;
                    }
                    dbAdjustedTotal = cashValue * dbGradientFactor;
                    //no salvage value adjustment
                }
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.exponential)
            {
                dbGradientFactor = System.Math.Exp(escalationRate * dbServicePlusPCYears);
                if (planningConstructionYears > 0)
                {
                    dbPlanningConstructionPhaseFactor = System.Math.Exp(escalationRate * planningConstructionYears);
                    dbGradientFactor = dbGradientFactor - dbPlanningConstructionPhaseFactor;
                }
                dbAdjustedTotal = cashValue * dbGradientFactor;
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.upvtable)
            {
                //NIST 135 index factor
                dbAdjustedTotal = cashValue * discountFactor;
                //no salvage value adjustment
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.spv)
            {

                if (discountYearTimes > 1)
                {
                    dbAdjustedTotal = 0;
                    //add recurring spv costs
                    double dbYears = discountYears;
                    for (int i = 1; i <= discountYearTimes; i++)
                    {
                        //total years must be less than or equal to servicelifeplus years
                        if (dbYears <= dbServicePlusPCYears)
                        {
                            //sum the recurrent costs
                            dbGradientFactor = 1 / (System.Math.Pow((1 + interestRate), dbYears));
                            dbAdjustedTotal += cashValue * dbGradientFactor;
                        }
                        //add addition years to discount next recurrent cost
                        dbYears += discountYears;
                    }
                }
                else
                {
                    //NIST 135 single present value, table 5.4
                    dbGradientFactor = 1 / (System.Math.Pow((1 + interestRate), discountYears));
                    dbAdjustedTotal = cashValue * dbGradientFactor;
                }
                //add salvage value (a negative number)
                double dbSalvageValue = CalculateSalvageValue(salvValue, dbServicePlusPCYears, interestRate);
                dbAdjustedTotal += dbSalvageValue;

            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.caprecovery)
            {
                if (discountYearTimes > 1)
                {
                    //cost per hour of operation style calculations
                    dbAdjustedTotal = CapRecoverCostAnn(cashValue, interestRate, 0, discountYears, salvValue) / discountYearTimes;
                }
                else
                {
                    //annual capital recovery costs
                    dbAdjustedTotal = CapRecoverCostAnn(cashValue, interestRate, 0, discountYears, salvValue); ;
                }
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.caprecoveryspv)
            {
                //208: use spv to discount back to present with years from base date
                double dbDiscountYears = yearsFromBaseDate;
                dbAdjustedTotal = GeneralRules.GetGradientRealDiscountValue(cashValue,
                    interestRate, serviceLifeYears, yearsFromBaseDate, planningConstructionYears,
                    GeneralRules.GROWTH_SERIES_TYPES.spv, escalationRate, discountFactor,
                    yearsFromBaseDate, discountYearTimes, salvValue);
                //then use caprecovery to discount over service+pcyears
                dbDiscountYears = serviceLifeYears + planningConstructionYears;
                dbAdjustedTotal = GeneralRules.GetGradientRealDiscountValue(cashValue,
                    interestRate, serviceLifeYears, yearsFromBaseDate, planningConstructionYears,
                    GeneralRules.GROWTH_SERIES_TYPES.caprecovery, escalationRate, discountFactor,
                    dbDiscountYears, discountYearTimes, salvValue);
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.upv)
            {
                if (discountYearTimes > 1)
                {
                    //cost per hour of operation style calculations
                    dbAdjustedTotal = (cashValue * ((Math.Pow(1 + interestRate, discountYears) - 1) / (interestRate * (Math.Pow((1 + interestRate), discountYears))))) / discountYearTimes;
                }
                else
                {
                    dbAdjustedTotal = cashValue * ((Math.Pow(1 + interestRate, discountYears) - 1) / (interestRate * (Math.Pow((1 + interestRate), discountYears))));
                }
            }
            else if (growthType == GROWTH_SERIES_TYPES.eaa)
            {
                dbAdjustedTotal = CalculateEquivalentAnnualAnnuity(cashValue, discountYears, interestRate, 0);
            }
            else
            {
                dbAdjustedTotal = cashValue;
                //208 deprecated
                ////single present value using subcost.years
                //dbGradientFactor = 1 / (System.Math.Pow((1 + interestRate), discountYears));
                //dbAdjustedTotal = cashValue * dbGradientFactor;
                ////add salvage value (a negative number)
                //double dbSalvageValue = CalculateSalvageValue(salvValue, dbServicePlusPCYears, interestRate);
                //dbAdjustedTotal += dbSalvageValue;
            }

            return dbAdjustedTotal;
        }
        private static double CalculateSalvageValue(double salvValue, double servicePlusPCYears, 
            double realRate)
        {
            //NIST 135 discounting
            double dbSalvageValue = 0;
            double dbDiscountFactor = 1;
            if (salvValue != 0)
            {
                dbDiscountFactor = GeneralRules.CalculateDiscountFactorByYears(realRate,
                    (servicePlusPCYears));
                //must be substracted
                dbSalvageValue = (salvValue * dbDiscountFactor) * -1;
            }
            return dbSalvageValue;
        }
        public static double GetStandardDeviation(double total, int observations, double memberSquaredTotal)
        {
            double dbStandardDeviation = 0;
            double dbS1 = (Math.Pow(total, 2)) / observations;
            double dbS2 = memberSquaredTotal - dbS1;
            int iNMinus1 = observations - 1;
            if (dbS2 == 0)
            {
                //single observation 
                dbStandardDeviation = 0;
                return dbStandardDeviation;
            }
            //sample std deviation; population would use iCount as divisor
            double dbS2Count = dbS2 / iNMinus1;
            if (dbS2Count < 0) dbS2Count = 0;
            dbStandardDeviation = Math.Sqrt(dbS2Count);
            return dbStandardDeviation;
        }
    }
}
