using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Serialize and deserialize a life cycle benefit calculator.
    ///             This calculator is used with outputs to calculate benefits.
    ///Author:		www.devtreks.org
    ///Date:		2013, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Extends the base object LCB1Calculator object
    ///</summary>
    public class LCB1Calculator : SubPrice1
    {
        public LCB1Calculator()
            : base()
        {
            //health care cost object
            InitLCB1Properties();
        }
        //copy constructor
        public LCB1Calculator(LCB1Calculator lca1Calc)
            : base(lca1Calc)
        {
            CopyLCB1Properties(lca1Calc);
        }
        
        //life in years
        public double ServiceLifeYears { get; set; }
        //years from base date to service date
        public double YearsFromBaseDate { get; set; }
        //preproduction or planning/construction period
        public double PlanningConstructionYears { get; set; }
        //area (or volume) and unit for capital input host i.e. 1000 ft2 house
        //support subcost price per unit totals (energy cost per ft2)
        public double PerUnitAmount { get; set; }
        public string PerUnitUnit { get; set; }
        //need to store locals and update parent output.trev
        public Output LCBOutput { get; set; }

        private const string cServiceLifeYears = "ServiceLifeYears";
        private const string cYearsFromBaseDate = "YearsFromBaseDate";
        private const string cPlanningConstructionYears = "PlanningConstructionYears";
        private const string cPerUnitAmount = "PerUnitAmount";
        private const string cPerUnitUnit = "PerUnitUnit";

        //benefits
        public double RTotalBenefit { get; set; }
        //total lcc benefit
        public double LCBTotalBenefit { get; set; }
        //total eaa benefit (equiv ann annuity)
        public double EAATotalBenefit { get; set; }
        //total per unit benefits
        public double UnitTotalBenefit { get; set; }
        private const string cRTotalBenefit = "RTotalBenefit";
        private const string cLCBTotalBenefit = "LCBTotalBenefit";
        private const string cEAATotalBenefit = "EAATotalBenefit";
        private const string cUnitTotalBenefit = "UnitTotalBenefit";

        public virtual void InitLCB1Properties()
        {
            //avoid null references to properties
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.InitSubPrice1sProperties();

            this.ServiceLifeYears = 0;
            this.YearsFromBaseDate = 0;
            this.PlanningConstructionYears = 0;
            //avoid divide by zero when left out
            this.PerUnitAmount = 1;
            this.PerUnitUnit = string.Empty;
            this.LCBOutput = new Output();
            this.RTotalBenefit = 0;
            this.LCBTotalBenefit = 0;
            this.EAATotalBenefit = 0;
            this.UnitTotalBenefit = 0;
        }

        public virtual void CopyLCB1Properties(
            LCB1Calculator calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            this.CopySubPrice1sProperties(calculator);
            this.ServiceLifeYears = calculator.ServiceLifeYears;
            this.YearsFromBaseDate = calculator.YearsFromBaseDate;
            this.PlanningConstructionYears = calculator.PlanningConstructionYears;
            this.PerUnitAmount = calculator.PerUnitAmount;
            this.PerUnitUnit = calculator.PerUnitUnit;
            this.LCBOutput = new Output(calculator.LCBOutput);
            this.RTotalBenefit = calculator.RTotalBenefit;
            this.LCBTotalBenefit = calculator.LCBTotalBenefit;
            this.EAATotalBenefit = calculator.EAATotalBenefit;
            this.UnitTotalBenefit = calculator.UnitTotalBenefit;
        }
        //set the class properties using the XElement
        public virtual void SetLCB1Properties(XElement calculator,
            XElement currentElement)
        {
            this.SetCalculatorProperties(calculator);
            //need the aggregating params (label, groupid, typeid(
            this.SetSharedObjectProperties(currentElement);
            this.SetSubPrice1sProperties(calculator);
            this.ServiceLifeYears = CalculatorHelpers.GetAttributeDouble(calculator,
               cServiceLifeYears);
            this.YearsFromBaseDate = CalculatorHelpers.GetAttributeDouble(calculator,
               cYearsFromBaseDate);
            this.PlanningConstructionYears = CalculatorHelpers.GetAttributeDouble(calculator,
               cPlanningConstructionYears);
            this.PerUnitAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               cPerUnitAmount);
            this.PerUnitUnit = CalculatorHelpers.GetAttribute(calculator,
               cPerUnitUnit);
            this.RTotalBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
                cRTotalBenefit);
            this.LCBTotalBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               cLCBTotalBenefit);
            this.EAATotalBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               cEAATotalBenefit);
            this.UnitTotalBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               cUnitTotalBenefit);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetLCB1Property(string attName,
            string attValue, int colIndex)
        {
            this.SetSubPrice1sProperty(attName, attValue, colIndex);
            switch (attName)
            {
                case cServiceLifeYears:
                    this.ServiceLifeYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cYearsFromBaseDate:
                    this.YearsFromBaseDate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPlanningConstructionYears:
                    this.PlanningConstructionYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPerUnitAmount:
                    this.PerUnitAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPerUnitUnit:
                    this.PerUnitUnit = attValue;
                    break;
                case cRTotalBenefit:
                    this.RTotalBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLCBTotalBenefit:
                    this.LCBTotalBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEAATotalBenefit:
                    this.EAATotalBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cUnitTotalBenefit:
                    this.UnitTotalBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetLCB1Property(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cServiceLifeYears:
                    sPropertyValue = this.ServiceLifeYears.ToString();
                    break;
                case cYearsFromBaseDate:
                    sPropertyValue = this.YearsFromBaseDate.ToString();
                    break;
                case cPlanningConstructionYears:
                    sPropertyValue = this.PlanningConstructionYears.ToString();
                    break;
                case cPerUnitAmount:
                    sPropertyValue = this.PerUnitAmount.ToString();
                    break;
                case cPerUnitUnit:
                    sPropertyValue = this.PerUnitUnit;
                    break;
                case cRTotalBenefit:
                    sPropertyValue = this.RTotalBenefit.ToString();
                    break;
                case cLCBTotalBenefit:
                    sPropertyValue = this.LCBTotalBenefit.ToString();
                    break;
                case cEAATotalBenefit:
                    sPropertyValue = this.EAATotalBenefit.ToString();
                    break;
                case cUnitTotalBenefit:
                    sPropertyValue = this.UnitTotalBenefit.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public void SetLCB1Attributes(string attNameExtension,
            ref XElement calculator)
        {
            //must remove old unwanted attributes
            if (calculator != null)
            {
                //do not remove atts here, they were removed in prior this.LCCOutput.SetOutputAtts
                //and now include good locals
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(attNameExtension, ref calculator);
            }
            this.SetSubPrice1sAttributes(ref calculator);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cServiceLifeYears, attNameExtension), this.ServiceLifeYears);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                string.Concat(cYearsFromBaseDate, attNameExtension), this.YearsFromBaseDate);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                string.Concat(cPlanningConstructionYears, attNameExtension), this.PlanningConstructionYears);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                 string.Concat(cPerUnitAmount, attNameExtension), this.PerUnitAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cPerUnitUnit, attNameExtension), this.PerUnitUnit);

            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                string.Concat(cRTotalBenefit, attNameExtension), this.RTotalBenefit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cLCBTotalBenefit, attNameExtension), this.LCBTotalBenefit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cEAATotalBenefit, attNameExtension), this.EAATotalBenefit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cUnitTotalBenefit, attNameExtension), this.UnitTotalBenefit);
        }
        public virtual void SetLCB1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //note must first use use either setanalyzeratts or SetCalculatorAttributes(attNameExtension, ref writer);
            this.SetSubPrice1sAttributes(ref writer);
            writer.WriteAttributeString(
                   string.Concat(cServiceLifeYears, attNameExtension), this.ServiceLifeYears.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cYearsFromBaseDate, attNameExtension), this.YearsFromBaseDate.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cPlanningConstructionYears, attNameExtension), this.PlanningConstructionYears.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cPerUnitAmount, attNameExtension), this.PerUnitAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cPerUnitUnit, attNameExtension), this.PerUnitUnit);

            writer.WriteAttributeString(
                  string.Concat(cRTotalBenefit, attNameExtension), this.RTotalBenefit.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                  string.Concat(cLCBTotalBenefit, attNameExtension), this.LCBTotalBenefit.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                  string.Concat(cEAATotalBenefit, attNameExtension), this.EAATotalBenefit.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                  string.Concat(cUnitTotalBenefit, attNameExtension), this.UnitTotalBenefit.ToString("N2", CultureInfo.InvariantCulture));
        }

        public bool SetLCB1Calculations(
            LCA1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement calculator,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            string sErrorMessage = string.Empty;
            //deserialize xml to object
            //set the base output properties (including local)
            this.LCBOutput.SetOutputProperties(calcParameters,
                calculator, currentElement);
            this.SetLCB1Properties(calculator, currentElement);
            bHasCalculations = RunLCB1Calculations(ref sErrorMessage);
            //serialize object back to xml and fill in updates list
            //locals come from output
            this.LCBOutput.SetOutputAttributes(calcParameters,
                ref currentElement, calcParameters.Updates);
            this.SetLCB1Attributes(string.Empty, ref calculator);
            //set the totals into calculator
            this.LCBOutput.SetNewOutputAttributes(calcParameters, ref calculator);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            calcParameters.ErrorMessage = sErrorMessage;
            return bHasCalculations;
        }
        public bool RunLCB1Calculations(ref string errorMsg)
        {
            bool bHasCalculations = false;
            //run benefit calcs
            bHasCalculations = RunBenefitCalculations(this);
            return bHasCalculations;
        }
        
        public bool RunBenefitCalculations(LCB1Calculator lifeCycleOutput)
        {
            bool bHasCalcs = false;
            GeneralRules.GROWTH_SERIES_TYPES eGrowthType;
            //five subbenefits to calculate
            double dbSubPrice1Total = 0;
            double dbSubPrice2Total = 0;
            double dbSubPrice3Total = 0;
            double dbSubPrice4Total = 0;
            double dbSubPrice5Total = 0;
            double dbSubPrice6Total = 0;
            double dbSubPrice7Total = 0;
            double dbSubPrice8Total = 0;
            double dbSubPrice9Total = 0;
            double dbSubPrice10Total = 0;
            //only the real rate and constant dollars are used
            //but keep these for possible future use
            double dbNominalRate = 0;
            double dbRealRate = 0;
            double dbInflationRate = 0;
            //ok to use the default order of the subcosts
            int i = 1;
            foreach (SubPrice1 subprice in lifeCycleOutput.SubPrice1s)
            {
                if (i == 1)
                {
                    dbSubPrice1Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 2)
                {
                    dbSubPrice2Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 3)
                {
                    dbSubPrice3Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 4)
                {
                    dbSubPrice4Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 5)
                {
                    dbSubPrice5Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 6)
                {
                    dbSubPrice6Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 7)
                {
                    dbSubPrice7Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 8)
                {
                    dbSubPrice8Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 9)
                {
                    dbSubPrice9Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                else if (i == 10)
                {
                    dbSubPrice10Total = subprice.SubPAmount * subprice.SubPPrice;
                }
                i++;
            }

            //init calculation parameters
            dbRealRate = lifeCycleOutput.LCBOutput.Local.RealRate;
            dbNominalRate = lifeCycleOutput.LCBOutput.Local.NominalRate;
            dbInflationRate = 0;
            GeneralRules.MissingRate(ref dbRealRate,
                ref dbNominalRate, ref dbInflationRate);
            if (dbRealRate > 0.999) dbRealRate = dbRealRate / 100;
            if (dbNominalRate > 0.999) dbNominalRate = dbNominalRate / 100;
            if (dbInflationRate > 0.999) dbInflationRate = dbInflationRate / 100;
            lifeCycleOutput.LCBOutput.Local.RealRate = dbRealRate;
            lifeCycleOutput.LCBOutput.Local.NominalRate = dbNominalRate;
            lifeCycleOutput.LCBOutput.Local.InflationRate = dbInflationRate;
            //ok to use the default order of the subcosts
            i = 1;
            foreach (SubPrice1 subprice in lifeCycleOutput.SubPrice1s)
            {
                eGrowthType = GeneralRules.GetGrowthType(subprice.SubPEscType);
                if (i == 1)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice1Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 2)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice2Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 3)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice3Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 4)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice4Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 5)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice5Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 6)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice6Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 7)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice7Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 8)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice8Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 9)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice9Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                else if (i == 10)
                {
                    subprice.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice10Total,
                        dbRealRate, lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.YearsFromBaseDate,
                        lifeCycleOutput.PlanningConstructionYears, eGrowthType, subprice.SubPEscRate,
                        subprice.SubPFactor, subprice.SubPYears, subprice.SubPYearTimes, subprice.SubPSalvValue);
                }
                subprice.SubPTotalPerUnit = subprice.SubPTotal / lifeCycleOutput.PerUnitAmount;
                i++;
            }
            //set lifeCyleOutput's total properties to these values
            SetBenefitTotals(lifeCycleOutput);
            //update the base output's prices (unit benefits, not full benefits)
            UpdateBaseOutputUnitPrices(lifeCycleOutput);
            bHasCalcs = true;
            return bHasCalcs;
        }
        //need to add the correct subbenefit price type with the correct parent pricetotal
        private static void SetBenefitTotals(LCB1Calculator lifeCycleOutput)
        {
            //init at zero (these get summed in npv and lcc comp and investment calcors)
            lifeCycleOutput.LCBOutput.Price = 0;

            lifeCycleOutput.RTotalBenefit = 0;
            //ok to use the default order of the subcosts
            foreach (SubPrice1 subprice in lifeCycleOutput.SubPrice1s)
            {
                SetOutputBasePrice(lifeCycleOutput, subprice.SubPType, subprice.SubPTotal);
            }
            //set lcc total
            lifeCycleOutput.LCBTotalBenefit = lifeCycleOutput.RTotalBenefit;
            //set eaa total
            lifeCycleOutput.EAATotalBenefit = GeneralRules.CalculateEquivalentAnnualAnnuity(lifeCycleOutput.LCBTotalBenefit,
                lifeCycleOutput.ServiceLifeYears, lifeCycleOutput.LCBOutput.Local.RealRate, lifeCycleOutput.LCBOutput.Local.NominalRate);
            //set unit benefit
            if (lifeCycleOutput.PerUnitAmount == 0)
                lifeCycleOutput.PerUnitAmount = 1;
            lifeCycleOutput.UnitTotalBenefit = lifeCycleOutput.LCBTotalBenefit / lifeCycleOutput.PerUnitAmount;
        }
        private static void SetOutputBasePrice(LCB1Calculator lifeCycleOutput, string priceType,
            double benefit)
        {
            if (priceType == SubPrices.PRICE_TYPES.rev.ToString())
            {
                lifeCycleOutput.RTotalBenefit += benefit;
            }
        }
        private void UpdateBaseOutputUnitPrices(LCB1Calculator lifeCycleOutput)
        {
            //they have to enter units, units should generally be "each"
            lifeCycleOutput.LCBOutput.Price = lifeCycleOutput.RTotalBenefit;
            //operating and capital budgets use TotalAMR
            lifeCycleOutput.LCBOutput.TotalAMR = lifeCycleOutput.EAATotalBenefit;
        }
    }
}
