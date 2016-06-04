using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Serialize and deserialize a life cycle cost object.
    ///             This calculator is used with inputs to calculate costs.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Extends the base object Input object
    ///</summary>
    public class LCC1Input : Input
    {
        public LCC1Input()
            : base()
        {
            //health care cost object
            InitLCC1Properties();
        }
        //copy constructor
        public LCC1Input(LCC1Input lca1Calc)
            : base(lca1Calc)
        {
            CopyLCC1Properties(lca1Calc);
        }

        //life in years
        public double ServiceLifeYears { get; set; }
        //years from base date to service date
        //this.Date is the stateful base date (used for sorting, set from this.SetSharedProps())
        public double YearsFromBaseDate { get; set; }
        //preproduction or planning/construction period
        public double PlanningConstructionYears { get; set; }
        //area (or volume) and unit for capital input host i.e. 1000 ft2 house
        //support subcost price per unit totals (energy cost per ft2)
        public double PerUnitAmount { get; set; }
        public string PerUnitUnit { get; set; }

        //totals, including initcosts, salvageval, replacement, and subcosts
        public double OCTotalCost { get; set; }
        public double AOHTotalCost { get; set; }
        public double CAPTotalCost { get; set; }
        //total lcc cost
        public double LCCTotalCost { get; set; }
        //total eaa cost (equiv ann annuity)
        public double EAATotalCost { get; set; }
        //total per unit costs
        public double UnitTotalCost { get; set; }
        //subprice1 props
        public SubPrice1 SubPrice1 { get; set; }

        private const string cServiceLifeYears = "ServiceLifeYears";
        private const string cYearsFromBaseDate = "YearsFromBaseDate";
        private const string cPlanningConstructionYears = "PlanningConstructionYears";
        private const string cPerUnitAmount = "PerUnitAmount";
        private const string cPerUnitUnit = "PerUnitUnit";
        //costs
        private const string cOCTotalCost = "OCTotalCost";
        private const string cAOHTotalCost = "AOHTotalCost";
        private const string cCAPTotalCost = "CAPTotalCost";
        private const string cLCCTotalCost = "LCCTotalCost";
        private const string cEAATotalCost = "EAATotalCost";
        private const string cUnitTotalCost = "UnitTotalCost";

        public virtual void InitLCC1Properties()
        {
            //avoid null references to properties
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.SubPrice1 = new SubPrice1();
            this.SubPrice1.InitSubPrice1Properties(this.SubPrice1);

            this.ServiceLifeYears = 0;
            this.YearsFromBaseDate = 0;
            this.PlanningConstructionYears = 0;
            //avoid divide by zero when left out
            this.PerUnitAmount = 1;
            this.PerUnitUnit = string.Empty;
            this.OCTotalCost = 0;
            this.AOHTotalCost = 0;
            this.CAPTotalCost = 0;
            this.LCCTotalCost = 0;
            this.EAATotalCost = 0;
            this.UnitTotalCost = 0;
        }

        public virtual void CopyLCC1Properties(
            LCC1Input calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            //this.CopySubPrice1sProperties(calculator);
            this.ServiceLifeYears = calculator.ServiceLifeYears;
            this.YearsFromBaseDate = calculator.YearsFromBaseDate;
            this.PlanningConstructionYears = calculator.PlanningConstructionYears;
            this.PerUnitAmount = calculator.PerUnitAmount;
            this.PerUnitUnit = calculator.PerUnitUnit;
            this.SubPrice1 = new SubPrice1(calculator.SubPrice1);
            this.OCTotalCost = calculator.OCTotalCost;
            this.AOHTotalCost = calculator.AOHTotalCost;
            this.CAPTotalCost = calculator.CAPTotalCost;
            this.LCCTotalCost = calculator.LCCTotalCost;
            this.EAATotalCost = calculator.EAATotalCost;
            this.UnitTotalCost = calculator.UnitTotalCost;
        }
        //set the class properties using the XElement
        public virtual void SetLCC1Properties(XElement calculator,
            XElement currentElement)
        {
            this.SetCalculatorProperties(calculator);
            //need the aggregating params (label, groupid, typeid and Date for sorting)
            this.SetSharedObjectProperties(currentElement);
            this.SubPrice1.SetSubPrice1Properties(this.SubPrice1, string.Empty, calculator);
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
            this.OCTotalCost = CalculatorHelpers.GetAttributeDouble(calculator,
                cOCTotalCost);
            this.AOHTotalCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cAOHTotalCost);
            this.CAPTotalCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cCAPTotalCost);
            this.LCCTotalCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cLCCTotalCost);
            this.EAATotalCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cEAATotalCost);
            this.UnitTotalCost = CalculatorHelpers.GetAttributeDouble(calculator,
               cUnitTotalCost);
        }
        
        //attname and attvalue generally passed in from a reader
        public virtual void SetLCC1Property(string attName,
            string attValue, int colIndex)
        {
            this.SubPrice1.SetSubPrice1Property(this.SubPrice1, attName, attValue);
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
                case cOCTotalCost:
                    this.OCTotalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAOHTotalCost:
                    this.AOHTotalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCAPTotalCost:
                    this.CAPTotalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cLCCTotalCost:
                    this.LCCTotalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEAATotalCost:
                    this.EAATotalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cUnitTotalCost:
                    this.UnitTotalCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetLCC1Property(string attName)
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
                case cOCTotalCost:
                    sPropertyValue = this.OCTotalCost.ToString();
                    break;
                case cAOHTotalCost:
                    sPropertyValue = this.AOHTotalCost.ToString();
                    break;
                case cCAPTotalCost:
                    sPropertyValue = this.CAPTotalCost.ToString();
                    break;
                case cLCCTotalCost:
                    sPropertyValue = this.LCCTotalCost.ToString();
                    break;
                case cEAATotalCost:
                    sPropertyValue = this.EAATotalCost.ToString();
                    break;
                case cUnitTotalCost:
                    sPropertyValue = this.UnitTotalCost.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public void SetLCC1Attributes(string attNameExtension,
            ref XElement calculator)
        {
            //must remove old unwanted attributes
            if (calculator != null)
            {
                //do not remove atts here, they were removed in prior this.SubPrice1.SetInputAtts
                //and now include good locals
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(attNameExtension, ref calculator);
            }
            this.SubPrice1.SetSubPrice1Attributes(this.SubPrice1, string.Empty, ref calculator);
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
                string.Concat(cOCTotalCost, attNameExtension), this.OCTotalCost);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cAOHTotalCost, attNameExtension), this.AOHTotalCost);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cCAPTotalCost, attNameExtension), this.CAPTotalCost);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cLCCTotalCost, attNameExtension), this.LCCTotalCost);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cEAATotalCost, attNameExtension), this.EAATotalCost);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cUnitTotalCost, attNameExtension), this.UnitTotalCost);
        }
        public virtual void SetLCC1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //note must first use use either setanalyzeratts or SetCalculatorAttributes(attNameExtension, ref writer);
            this.SubPrice1.SetSubPrice1Attributes(this.SubPrice1, string.Empty, ref writer);
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
                   string.Concat(cOCTotalCost, attNameExtension), this.OCTotalCost.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cAOHTotalCost, attNameExtension), this.AOHTotalCost.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cCAPTotalCost, attNameExtension), this.CAPTotalCost.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cLCCTotalCost, attNameExtension), this.LCCTotalCost.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cEAATotalCost, attNameExtension), this.EAATotalCost.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                   string.Concat(cUnitTotalCost, attNameExtension), this.UnitTotalCost.ToString("N2", CultureInfo.InvariantCulture));
        }

        public bool SetLCC1Calculations(
            CalculatorParameters calcParameters, ref XElement calculator,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            string sErrorMessage = string.Empty;
            //deserialize xml to object
            //set the base input properties (updates base input prices)
            //locals come from input
            this.SetInputProperties(calcParameters,
                calculator, currentElement);
            //set the calculator
            this.SetLCC1Properties(calculator, currentElement);
            bHasCalculations = RunLCC1Calculations(ref sErrorMessage);
            //serialize object back to xml and fill in updates list
            this.SetInputAttributes(calcParameters,
                ref currentElement, calcParameters.Updates);
            this.SetLCC1Attributes(string.Empty, ref calculator);
            //set the totals into calculator
            this.SetNewInputAttributes(calcParameters, ref calculator);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            calcParameters.ErrorMessage = sErrorMessage;
            return bHasCalculations;
        }
        public bool RunLCC1Calculations(ref string errorMsg)
        {
            bool bHasCalculations = false;
            //run cost calcs
            bHasCalculations = RunCostCalculations(this);
            return bHasCalculations;
        }
        private void UpdateBaseInputOutputUnitPrices()
        {
            //needs to be implemented for both inputs and outputs
        }
        /// <summary>
        /// Purpose     Calculate life cycle costs.
        ///             Calculations come from NIST 135.
        /// </summary>
        public bool RunCostCalculations(LCC1Input lifeCycleInput)
        {
            bool bHasCalcs = false;
            GeneralRules.GROWTH_SERIES_TYPES eGrowthType;
            //one subcost to calculate
            double dbSubPrice1Total = 0;
            //only the real rate and constant dollars are used
            //but keep these for possible future use
            double dbNominalRate = 0;
            double dbRealRate = 0;
            double dbInflationRate = 0;

            //the totals have to be discounted and escalated from these initial totals
            //(multiplicative means that price does not have to be used)
            //ok to use the default order of the subcosts
            dbSubPrice1Total = lifeCycleInput.SubPrice1.SubPAmount * lifeCycleInput.SubPrice1.SubPPrice;
            
            //init calculation parameters
            dbRealRate = lifeCycleInput.Local.RealRate;
            dbNominalRate = lifeCycleInput.Local.NominalRate;
            dbInflationRate = 0;
            GeneralRules.MissingRate(ref dbRealRate,
                ref dbNominalRate, ref dbInflationRate);
            if (dbRealRate > 0.999) dbRealRate = dbRealRate / 100;
            if (dbNominalRate > 0.999) dbNominalRate = dbNominalRate / 100;
            if (dbInflationRate > 0.999) dbInflationRate = dbInflationRate / 100;
            lifeCycleInput.Local.RealRate = dbRealRate;
            lifeCycleInput.Local.NominalRate = dbNominalRate;
            lifeCycleInput.Local.InflationRate = dbInflationRate;
            //get the recurring cost factors
            //ok to use the default order of the subcosts
            eGrowthType = GeneralRules.GetGrowthType(lifeCycleInput.SubPrice1.SubPEscType);
            lifeCycleInput.SubPrice1.SubPTotal = GeneralRules.GetGradientRealDiscountValue(dbSubPrice1Total,
                dbRealRate, lifeCycleInput.ServiceLifeYears, lifeCycleInput.YearsFromBaseDate,
                lifeCycleInput.PlanningConstructionYears, eGrowthType, lifeCycleInput.SubPrice1.SubPEscRate,
                lifeCycleInput.SubPrice1.SubPFactor, lifeCycleInput.SubPrice1.SubPYears, 
                lifeCycleInput.SubPrice1.SubPYearTimes, lifeCycleInput.SubPrice1.SubPSalvValue);
            lifeCycleInput.SubPrice1.SubPTotalPerUnit = lifeCycleInput.SubPrice1.SubPTotal / lifeCycleInput.PerUnitAmount;
            
            //set lifeCyleInput's total properties to these values
            SetCostTotals(lifeCycleInput);
            //update the base input's prices (unit costs, not full costs)
            UpdateBaseInputUnitPrices(lifeCycleInput);
            bHasCalcs = true;
            return bHasCalcs;
        }

        //need to add the correct subcost price type with the correct parent pricetotal
        private static void SetCostTotals(LCC1Input lifeCycleInput)
        {
            //init at zero (these get summed in npv and lcc comp and investment calcors)
            lifeCycleInput.OCPrice = 0;
            lifeCycleInput.AOHPrice = 0;
            lifeCycleInput.CAPPrice = 0;

            lifeCycleInput.OCTotalCost = 0;
            lifeCycleInput.AOHTotalCost = 0;
            lifeCycleInput.CAPTotalCost = 0;
            //set the totals
            //ok to use the default order of the subcosts
            SetInputBasePrice(lifeCycleInput, lifeCycleInput.SubPrice1.SubPType, lifeCycleInput.SubPrice1.SubPTotal);
            //set lcc total
            lifeCycleInput.LCCTotalCost = lifeCycleInput.OCTotalCost + lifeCycleInput.AOHTotalCost + lifeCycleInput.CAPTotalCost;
            //set eaa total
            lifeCycleInput.EAATotalCost = GeneralRules.CalculateEquivalentAnnualAnnuity(lifeCycleInput.LCCTotalCost,
                lifeCycleInput.ServiceLifeYears, lifeCycleInput.Local.RealRate, lifeCycleInput.Local.NominalRate);
            //set unit cost
            if (lifeCycleInput.PerUnitAmount == 0)
                lifeCycleInput.PerUnitAmount = 1;
            lifeCycleInput.UnitTotalCost = lifeCycleInput.LCCTotalCost / lifeCycleInput.PerUnitAmount;
        }
        private static void SetInputBasePrice(LCC1Input lifeCycleInput, string priceType, double cost)
        {
            if (priceType == SubPrice1.PRICE_TYPE.oc.ToString())
            {
                lifeCycleInput.OCTotalCost += cost;
            }
            else if (priceType == SubPrice1.PRICE_TYPE.aoh.ToString())
            {
                lifeCycleInput.AOHTotalCost += cost;
            }
            else if (priceType == SubPrice1.PRICE_TYPE.cap.ToString())
            {
                lifeCycleInput.CAPTotalCost += cost;
            }
        }
        private void UpdateBaseInputUnitPrices(LCC1Input lifeCycleInput)
        {
            //base input prices are entered as total costs (composite capital input)
            //they have to enter units and amounts, units should generally be "each"
            lifeCycleInput.OCPrice = lifeCycleInput.OCTotalCost;
            lifeCycleInput.AOHPrice = lifeCycleInput.AOHTotalCost;
            lifeCycleInput.CAPPrice = lifeCycleInput.CAPTotalCost;
            //operating and capital budgets use TotalAMAOH
            lifeCycleInput.SubPrice1.TotalAMAOH = lifeCycleInput.EAATotalCost;
        }
       
    }
}
