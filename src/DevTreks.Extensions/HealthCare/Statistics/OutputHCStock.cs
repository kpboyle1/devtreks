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
    ///Purpose:		The OutputHCStock class extends the HealthBenefit1Calculator() 
    ///             class and is used by health care benefits calculators and analyzers 
    ///             to set totals and basic health care benefit statistics. Basic 
    ///             health care benefit statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    public class OutputHCStock : HealthBenefit1Calculator
    {
        //calls the base-class version, and initializes the base class properties.
        public OutputHCStock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            InitTotalBenefitsProperties();
            //health care cost object
            InitTotalOutputHCStockProperties();
        }
        //copy constructor
        public OutputHCStock(OutputHCStock calculator)
            : base(calculator)
        {
            CopyTotalOutputHCStockProperties(calculator);
        }

        //calculator properties
        //hcOutput collection
        //int = file number, basestat position in list = basestat number
        //i.e. output 1 has a zero index position, output 2 a one index ...
        public IDictionary<int, List<HealthBenefit1Calculator>> HealthCareBenefit1s = null;

        public double TotalOutputCost { get; set; }
        public double TotalBenefitAdjustment { get; set; }
        public double TotalAdjustedBenefit { get; set; }
        public double TotalOutputEffect1Amount { get; set; }
        public double TotalOutputEffect1Price { get; set; }
        public double TotalOutputEffect1Cost { get; set; }

        public double TotalPhysicalHealthRating { get; set; }
        public double TotalEmotionalHealthRating { get; set; }
        public double TotalSocialHealthRating { get; set; }
        public double TotalEconomicHealthRating { get; set; }
        public double TotalHealthCareDeliveryRating { get; set; }
        public double TotalBeforeQOLRating { get; set; }
        public double TotalAfterQOLRating { get; set; }
        public double TotalBeforeYears { get; set; }
        public double TotalAfterYears { get; set; }
        public double TotalAfterYearsProb { get; set; }
        public double TotalTimeTradeoffYears { get; set; }
        public double TotalEquityMultiplier { get; set; }
        public double TotalAverageBenefitRating { get; set; }
        public double TotalQALY { get; set; }
        public double TotalICERQALY { get; set; }
        public double TotalTTOQALY { get; set; }

        private const string TOutputCost = "TOutputCost";
        private const string TBenefitAdjustment = "TBenefitAdjustment";
        private const string TAdjustedBenefit = "TAdjustedBenefit";
        private const string TOutputEffect1Amount = "TOutputEffect1Amount";
        private const string TOutputEffect1Price = "TOutputEffect1Price";
        private const string TOutputEffect1Cost = "TOutputEffect1Cost";

        private const string TPhysicalHealthRating = "TPhysicalHealthRating";
        private const string TEmotionalHealthRating = "TEmotionalHealthRating";
        private const string TSocialHealthRating = "TSocialHealthRating";
        private const string TEconomicHealthRating = "TEconomicHealthRating";
        private const string THealthCareDeliveryRating = "THealthCareDeliveryRating";
        private const string TBeforeQOLRating = "TBeforeQOLRating";
        private const string TAfterQOLRating = "TAfterQOLRating";
        private const string TBeforeYears = "TBeforeYears";
        private const string TAfterYears = "TAfterYears";
        private const string TAfterYearsProb = "TAfterYearsProb";
        private const string TTimeTradeoffYears = "TTimeTradeoffYears";
        private const string TEquityMultiplier = "TEquityMultiplier";
        private const string TAverageBenefitRating = "TAverageBenefitRating";
        private const string TQALY = "TQALY";
        private const string TICERQALY = "TICERQALY";
        private const string TTTOQALY = "TTTOQALY";

        public virtual void InitTotalOutputHCStockProperties()
        {
            //avoid null references to properties
            this.TotalOutputCost = 0;
            //AdjustedPrice = BasePriceAdjustment * BasePrice
            this.TotalBenefitAdjustment = 0;
            this.TotalAdjustedBenefit = 0;
            this.TotalOutputEffect1Amount = 0;
            this.TotalOutputEffect1Price = 0;
            this.TotalOutputEffect1Cost = 0;
            this.TotalPhysicalHealthRating = 0;
            this.TotalEmotionalHealthRating = 0;
            this.TotalSocialHealthRating = 0;
            this.TotalEconomicHealthRating = 0;
            this.TotalHealthCareDeliveryRating = 0;
            this.TotalBeforeQOLRating = 0;
            this.TotalAfterQOLRating = 0;
            this.TotalBeforeYears = 0;
            this.TotalAfterYears = 0;
            this.TotalAfterYearsProb = 0;
            this.TotalTimeTradeoffYears = 0;
            this.TotalEquityMultiplier = 0;
            this.TotalAverageBenefitRating = 0;
            this.TotalQALY = 0;
            this.TotalICERQALY = 0;
            this.TotalTTOQALY = 0;
        }
        public virtual void CopyTotalOutputHCStockProperties(
            OutputHCStock calculator)
        {
            this.TotalOutputCost = calculator.TotalOutputCost;
            this.TotalBenefitAdjustment = calculator.TotalBenefitAdjustment;
            this.TotalAdjustedBenefit = calculator.TotalAdjustedBenefit;
            this.TotalOutputEffect1Amount = calculator.TotalOutputEffect1Amount;
            this.TotalOutputEffect1Price = calculator.TotalOutputEffect1Price;
            this.TotalOutputEffect1Cost = calculator.TotalOutputEffect1Cost;
            this.TotalPhysicalHealthRating = calculator.TotalPhysicalHealthRating;
            this.TotalEmotionalHealthRating = calculator.TotalEmotionalHealthRating;
            this.TotalSocialHealthRating = calculator.TotalSocialHealthRating;
            this.TotalEconomicHealthRating = calculator.TotalEconomicHealthRating;
            this.TotalHealthCareDeliveryRating = calculator.TotalHealthCareDeliveryRating;
            this.TotalBeforeQOLRating = calculator.TotalBeforeQOLRating;
            this.TotalAfterQOLRating = calculator.TotalAfterQOLRating;
            this.TotalBeforeYears = calculator.TotalBeforeYears;
            this.TotalAfterYears = calculator.TotalAfterYears;
            this.TotalAfterYearsProb = calculator.TotalAfterYearsProb;
            this.TotalTimeTradeoffYears = calculator.TotalTimeTradeoffYears;
            this.TotalEquityMultiplier = calculator.TotalEquityMultiplier;
            this.TotalAverageBenefitRating = calculator.TotalAverageBenefitRating;
            this.TotalQALY = calculator.QALY;
            this.TotalICERQALY = calculator.ICERQALY;
            this.TotalTTOQALY = calculator.TotalTTOQALY;
        }
        //set the class properties using the XElement
        public virtual void SetTotalOutputHCStockProperties(XElement currentCalculationsElement)
        {
            this.TotalOutputCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOutputCost);
            this.TotalBenefitAdjustment = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBenefitAdjustment);
            this.TotalAdjustedBenefit = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAdjustedBenefit);
            this.TotalOutputEffect1Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOutputEffect1Amount);
            this.TotalOutputEffect1Price = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOutputEffect1Price);
            this.TotalOutputEffect1Cost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TOutputEffect1Cost);
            this.TotalPhysicalHealthRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TPhysicalHealthRating);
            this.TotalEmotionalHealthRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TEmotionalHealthRating);
            this.TotalSocialHealthRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TSocialHealthRating);
            this.TotalEconomicHealthRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TEconomicHealthRating);
            this.TotalHealthCareDeliveryRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, THealthCareDeliveryRating);
            this.TotalBeforeQOLRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBeforeQOLRating);
            this.TotalAfterQOLRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAfterQOLRating);
            this.TotalBeforeYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TBeforeYears);
            this.TotalAfterYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAfterYears);
            this.TotalAfterYearsProb = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAfterYearsProb);
            this.TotalTimeTradeoffYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTimeTradeoffYears);
            this.TotalEquityMultiplier = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TEquityMultiplier);
            this.TotalAverageBenefitRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TAverageBenefitRating);
            this.TotalQALY = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TQALY);
            this.TotalICERQALY = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TICERQALY);
            this.TotalTTOQALY = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, TTTOQALY);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalOutputHCStockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOutputCost:
                    this.TotalOutputCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBenefitAdjustment:
                    this.TotalBenefitAdjustment = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAdjustedBenefit:
                    this.TotalAdjustedBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOutputEffect1Amount:
                    this.TotalOutputEffect1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOutputEffect1Price:
                    this.TotalOutputEffect1Price = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOutputEffect1Cost:
                    this.TotalOutputEffect1Cost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEmotionalHealthRating:
                    this.TotalEmotionalHealthRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TSocialHealthRating:
                    this.TotalSocialHealthRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEconomicHealthRating:
                    this.TotalEconomicHealthRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case THealthCareDeliveryRating:
                    this.TotalHealthCareDeliveryRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBeforeQOLRating:
                    this.TotalBeforeQOLRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAfterQOLRating:
                    this.TotalAfterQOLRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBeforeYears:
                    this.TotalBeforeYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAfterYears:
                    this.TotalAfterYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAfterYearsProb:
                    this.TotalAfterYearsProb = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimeTradeoffYears:
                    this.TotalTimeTradeoffYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TEquityMultiplier:
                    this.TotalEquityMultiplier = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAverageBenefitRating:
                    this.TotalAverageBenefitRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TQALY:
                    this.TotalQALY = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TICERQALY:
                    this.TotalICERQALY = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTTOQALY:
                    this.TotalTTOQALY = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetTotalOutputHCStockAttributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOutputCost, attNameExtension), this.TotalOutputCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TBenefitAdjustment, attNameExtension), this.TotalBenefitAdjustment);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TAdjustedBenefit, attNameExtension), this.TotalAdjustedBenefit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOutputEffect1Amount, attNameExtension), this.TotalOutputEffect1Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOutputEffect1Price, attNameExtension), this.TotalOutputEffect1Price);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(TOutputEffect1Cost, attNameExtension), this.TotalOutputEffect1Cost);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TPhysicalHealthRating, attNameExtension), this.TotalPhysicalHealthRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TEmotionalHealthRating, attNameExtension), this.TotalEmotionalHealthRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TSocialHealthRating, attNameExtension), this.TotalSocialHealthRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TEconomicHealthRating, attNameExtension), this.TotalEconomicHealthRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(THealthCareDeliveryRating, attNameExtension), this.TotalHealthCareDeliveryRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TBeforeQOLRating, attNameExtension), this.TotalBeforeQOLRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(TAfterQOLRating, attNameExtension), this.TotalAfterQOLRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(TBeforeYears, attNameExtension), this.TotalBeforeYears);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(TAfterYears, attNameExtension), this.TotalAfterYears);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(TAfterYearsProb, attNameExtension), this.TotalAfterYearsProb);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(TTimeTradeoffYears, attNameExtension), this.TotalTimeTradeoffYears);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(TEquityMultiplier, attNameExtension), this.TotalEquityMultiplier);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TAverageBenefitRating, attNameExtension), this.TotalAverageBenefitRating);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
              string.Concat(TQALY, attNameExtension), this.TotalQALY);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TICERQALY, attNameExtension), this.TotalICERQALY);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(TTTOQALY, attNameExtension), this.TotalTTOQALY);
        }
        public virtual void SetTotalOutputHCStockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(TOutputCost, attNameExtension), this.TotalOutputCost.ToString());
            writer.WriteAttributeString(string.Concat(TBenefitAdjustment, attNameExtension), this.TotalBenefitAdjustment.ToString());
            writer.WriteAttributeString(string.Concat(TAdjustedBenefit, attNameExtension), this.TotalAdjustedBenefit.ToString());
            writer.WriteAttributeString(string.Concat(TOutputEffect1Amount, attNameExtension), this.TotalOutputEffect1Amount.ToString());
            writer.WriteAttributeString(string.Concat(TOutputEffect1Price, attNameExtension), this.TotalOutputEffect1Price.ToString());
            writer.WriteAttributeString(string.Concat(TOutputEffect1Cost, attNameExtension), this.TotalOutputEffect1Cost.ToString());
            writer.WriteAttributeString(string.Concat(TPhysicalHealthRating, attNameExtension), this.TotalPhysicalHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(TEmotionalHealthRating, attNameExtension), this.TotalEmotionalHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(TSocialHealthRating, attNameExtension), this.TotalSocialHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(TEconomicHealthRating, attNameExtension), this.TotalEconomicHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(THealthCareDeliveryRating, attNameExtension), this.TotalHealthCareDeliveryRating.ToString());
            writer.WriteAttributeString(string.Concat(TBeforeQOLRating, attNameExtension), this.TotalBeforeQOLRating.ToString());
            writer.WriteAttributeString(string.Concat(TAfterQOLRating, attNameExtension), this.TotalAfterQOLRating.ToString());
            writer.WriteAttributeString(string.Concat(TBeforeYears, attNameExtension), this.TotalBeforeYears.ToString());
            writer.WriteAttributeString(string.Concat(TAfterYears, attNameExtension), this.TotalAfterYears.ToString());
            writer.WriteAttributeString(string.Concat(TAfterYearsProb, attNameExtension), this.TotalAfterYearsProb.ToString());
            writer.WriteAttributeString(string.Concat(TTimeTradeoffYears, attNameExtension), this.TotalTimeTradeoffYears.ToString());
            writer.WriteAttributeString(string.Concat(TEquityMultiplier, attNameExtension), this.TotalEquityMultiplier.ToString());
            writer.WriteAttributeString(string.Concat(TAverageBenefitRating, attNameExtension), this.TotalAverageBenefitRating.ToString());
            writer.WriteAttributeString(string.Concat(TQALY, attNameExtension), this.TotalQALY.ToString());
            writer.WriteAttributeString(string.Concat(TICERQALY, attNameExtension), this.TotalICERQALY.ToString());
            writer.WriteAttributeString(string.Concat(TTTOQALY, attNameExtension), this.TotalTTOQALY.ToString());
        }
    }
    public static class OutputHCExtensions
    {
        //add a hcOutputfact to the baseStat.HealthCareBenefit1s dictionary
        public static bool AddOutputHCStocksToDictionary(this OutputHCStock baseStat,
            int filePosition, int nodePosition, HealthBenefit1Calculator hcOutputStock)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.HealthCareBenefit1s == null)
                baseStat.HealthCareBenefit1s
                = new Dictionary<int, List<HealthBenefit1Calculator>>();
            if (baseStat.HealthCareBenefit1s.ContainsKey(filePosition))
            {
                if (baseStat.HealthCareBenefit1s[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.HealthCareBenefit1s[filePosition].Count <= i)
                        {
                            baseStat.HealthCareBenefit1s[filePosition]
                                .Add(new HealthBenefit1Calculator());
                        }
                    }
                    baseStat.HealthCareBenefit1s[filePosition][nodePosition]
                        = hcOutputStock;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<HealthBenefit1Calculator> baseStats
                    = new List<HealthBenefit1Calculator>();
                KeyValuePair<int, List<HealthBenefit1Calculator>> newStat
                    = new KeyValuePair<int, List<HealthBenefit1Calculator>>(
                        filePosition, baseStats);
                baseStat.HealthCareBenefit1s.Add(newStat);
                bIsAdded = AddOutputHCStocksToDictionary(baseStat,
                    filePosition, nodePosition, hcOutputStock);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this OutputHCStock baseStat,
            int filePosition, HealthBenefit1Calculator hcOutputStock)
        {
            int iNodeCount = 0;
            if (baseStat.HealthCareBenefit1s == null)
                return iNodeCount;
            if (baseStat.HealthCareBenefit1s.ContainsKey(filePosition))
            {
                if (baseStat.HealthCareBenefit1s[filePosition] != null)
                {
                    iNodeCount = baseStat.HealthCareBenefit1s[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}
