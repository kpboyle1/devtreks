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
    ///Purpose:		The Machinery2Stock class extends the NPVCalculators.TimelinessOpComp1 
    ///             class and is used to analyze the scheduling and timing of operation and 
    ///             component capital stock use. Basic machinery stock statistical 
    ///             analyzers derive from this class to generate additional statistics.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes        1. 
    ///</summary>
    public class Machinery2Stock : TimelinessOpComp1
    {
        //calls the base-class version, and initializes the base class properties.
        public Machinery2Stock()
            : base()
        {
            //base input (don't init more)
            InitCalculatorProperties();
            //machinery
            InitTotalMachinery2StockProperties();
        }
        //copy constructor
        public Machinery2Stock(Machinery2Stock calculator)
            : base(calculator)
        {
            CopyTotalMachinery2StockProperties(calculator);
        }

        //calculator properties
        //machinery collection
        public IDictionary<int, List<TimelinessOpComp1>> Machinery2Stocks = null;

        //totals
        public double TotalAmount { get; set; }
        public double TotalLaborAvailable { get; set; }
        public double TotalWorkdayProbability { get; set; }
        public double TotalTimelinessPenalty1 { get; set; }
        public double TotalTimelinessPenaltyDaysFromStart1 { get; set; }
        public double TotalTimelinessPenalty2 { get; set; }
        public double TotalTimelinessPenaltyDaysFromStart2 { get; set; }
        public double TotalWorkdaysLimit { get; set; }
        public double TotalFieldCapacity { get; set; }
        public double TotalAreaCovered { get; set; }
        public double TotalFieldDays { get; set; }
        public double TotalOutputPrice { get; set; }
        public double TotalOutputYield { get; set; }
        public double TotalCompositionAmount { get; set; }
        public double TotalOutputTimes { get; set; }
        public double TotalProbableFieldDays { get; set; }
        public double TotalTimelinessPenaltyCost { get; set; }
        public double TotalTimelinessPenaltyCostPerHour { get; set; }
        //totalrevenue is TotalR from basecalcs

        //totals
        private const string TAmount = "TAmount";
        private const string TLaborAvailable = "TLaborAvailable";
        public const string TWorkdayProbability = "TWorkdayProbability";
        public const string TTimelinessPenalty1 = "TTimelinessPenalty1";
        public const string TTimelinessPenaltyDaysFromStart1 = "TTimelinessPenaltyDaysFromStart1";
        public const string TTimelinessPenalty2 = "TTimelinessPenalty2";
        public const string TTimelinessPenaltyDaysFromStart2 = "TTimelinessPenaltyDaysFromStart2";
        public const string TTotalWorkdaysLimit = "TTotalWorkdaysLimit";
        public const string TFieldCapacity = "TFieldCapacity";
        public const string TAreaCovered = "TAreaCovered";
        public const string TFieldDays = "TFieldDays";
        public const string TOutputPrice = "TOutputPrice";
        private const string TOutputYield = "TOutputYield";
        private const string TCompositionAmount = "TCompositionAmount";
        private const string TOutputTimes = "TOutputTimes";
        private const string TProbableFieldDays = "TProbableFieldDays";
        public const string TTimelinessPenaltyCost = "TTimelinessPenaltyCost";
        public const string TTimelinessPenaltyCostPerHour = "TTimelinessPenaltyCostPerHour";
        public virtual void InitTotalMachinery2StockProperties()
        {
            this.TotalAmount = 0;
            this.TotalLaborAvailable = 0;
            this.TotalWorkdayProbability = 0;
            this.TotalTimelinessPenalty1 = 0;
            this.TotalTimelinessPenaltyDaysFromStart1 = 0;
            this.TotalTimelinessPenalty2 = 0;
            this.TotalTimelinessPenaltyDaysFromStart2 = 0;
            this.TotalWorkdaysLimit = 0;
            this.TotalFieldCapacity = 0;
            this.TotalAreaCovered = 0;
            this.TotalFieldDays = 0;
            this.TotalOutputPrice = 0;
            this.TotalOutputYield = 0;
            this.TotalCompositionAmount = 0;
            this.TotalOutputTimes = 0;
            this.TotalProbableFieldDays = 0;
            this.TotalTimelinessPenaltyCost = 0;
            this.TotalTimelinessPenaltyCostPerHour = 0;
            this.TotalR = 0;
        }
        public virtual void CopyTotalMachinery2StockProperties(
            Machinery2Stock calculator)
        {
            this.TotalAmount = calculator.TotalAmount;
            this.TotalLaborAvailable = calculator.TotalLaborAvailable;
            this.TotalWorkdayProbability = calculator.TotalWorkdayProbability;
            this.TotalTimelinessPenalty1 = calculator.TotalTimelinessPenalty1;
            this.TotalTimelinessPenaltyDaysFromStart1 = calculator.TotalTimelinessPenaltyDaysFromStart1;
            this.TotalTimelinessPenalty2 = calculator.TotalTimelinessPenalty2;
            this.TotalTimelinessPenaltyDaysFromStart2 = calculator.TotalTimelinessPenaltyDaysFromStart2;
            this.TotalWorkdaysLimit = calculator.TotalWorkdaysLimit;
            this.TotalAreaCovered = calculator.TotalAreaCovered;
            this.TotalFieldCapacity = calculator.TotalFieldCapacity;
            this.TotalFieldDays = calculator.TotalFieldDays;
            this.TotalOutputPrice = calculator.TotalOutputPrice;
            this.TotalOutputYield = calculator.TotalOutputYield;
            this.TotalCompositionAmount = calculator.TotalCompositionAmount;
            this.TotalOutputTimes = calculator.TotalOutputTimes;
            this.TotalProbableFieldDays = calculator.TotalProbableFieldDays;
            this.TotalTimelinessPenaltyCost = calculator.TotalTimelinessPenaltyCost;
            this.TotalTimelinessPenaltyCostPerHour = calculator.TotalTimelinessPenaltyCostPerHour;
            this.TotalR = calculator.TotalR;
        }
        public virtual void SetTotalMachinery2StockProperties(XElement calculator)
        {
            this.TotalAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TAmount);
            this.TotalWorkdayProbability = CalculatorHelpers.GetAttributeDouble(calculator,
                TWorkdayProbability);
            this.TotalLaborAvailable = CalculatorHelpers.GetAttributeDouble(calculator,
                TLaborAvailable);
            this.TotalTimelinessPenalty1 = CalculatorHelpers.GetAttributeDouble(calculator,
                TTimelinessPenalty1);
            this.TotalTimelinessPenaltyDaysFromStart1 = CalculatorHelpers.GetAttributeDouble(calculator,
                TTimelinessPenaltyDaysFromStart1);
            this.TotalTimelinessPenalty2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TTimelinessPenalty2);
            this.TotalTimelinessPenaltyDaysFromStart2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TTimelinessPenaltyDaysFromStart2);
            this.TotalWorkdaysLimit = CalculatorHelpers.GetAttributeDouble(calculator,
                TTotalWorkdaysLimit);
            this.TotalFieldCapacity = CalculatorHelpers.GetAttributeDouble(calculator,
                TFieldCapacity);
            this.TotalAreaCovered = CalculatorHelpers.GetAttributeDouble(calculator,
                TAreaCovered);
            this.TotalFieldDays = CalculatorHelpers.GetAttributeDouble(calculator,
                TFieldDays);
            this.TotalOutputPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TOutputPrice);
            this.TotalOutputYield = CalculatorHelpers.GetAttributeDouble(calculator,
                TOutputYield);
            this.TotalCompositionAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TCompositionAmount);
            this.TotalOutputTimes = CalculatorHelpers.GetAttributeDouble(calculator,
                TOutputTimes);
            this.TotalProbableFieldDays = CalculatorHelpers.GetAttributeDouble(calculator,
                TProbableFieldDays);
            this.TotalTimelinessPenaltyCost = CalculatorHelpers.GetAttributeDouble(calculator,
                TTimelinessPenaltyCost);
            this.TotalTimelinessPenaltyCostPerHour = CalculatorHelpers.GetAttributeDouble(calculator,
                TTimelinessPenaltyCostPerHour);
            this.TotalR = CalculatorHelpers.GetAttributeDouble(calculator,
                CostBenefitCalculator.TR);
        }
        public virtual void SetTotalMachinery2StockProperties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TAmount:
                    this.TotalAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TWorkdayProbability:
                    this.TotalWorkdayProbability = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TLaborAvailable:
                    this.TotalLaborAvailable = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimelinessPenalty1:
                    this.TotalTimelinessPenalty1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimelinessPenaltyDaysFromStart1:
                    this.TotalTimelinessPenaltyDaysFromStart1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimelinessPenalty2:
                    this.TotalTimelinessPenalty2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimelinessPenaltyDaysFromStart2:
                    this.TotalTimelinessPenaltyDaysFromStart2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTotalWorkdaysLimit:
                    this.TotalWorkdaysLimit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFieldCapacity:
                    this.TotalFieldCapacity = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAreaCovered:
                    this.TotalAreaCovered = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TFieldDays:
                    this.TotalFieldDays = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOutputPrice:
                    this.TotalOutputPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOutputYield:
                    this.TotalOutputYield = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCompositionAmount:
                    this.TotalCompositionAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOutputTimes:
                    this.TotalOutputTimes = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TProbableFieldDays:
                    this.TotalProbableFieldDays = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimelinessPenaltyCost:
                    this.TotalTimelinessPenaltyCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TTimelinessPenaltyCostPerHour:
                    this.TotalTimelinessPenaltyCostPerHour = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case CostBenefitCalculator.TR:
                    this.TotalR = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalMachinery2StockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TAmount:
                    sPropertyValue = this.TotalAmount.ToString();
                    break;
                case TWorkdayProbability:
                    sPropertyValue = this.TotalWorkdayProbability.ToString();
                    break;
                case TLaborAvailable:
                    sPropertyValue = this.TotalLaborAvailable.ToString();
                    break;
                case TTimelinessPenalty1:
                    sPropertyValue = this.TotalTimelinessPenalty1.ToString();
                    break;
                case TTimelinessPenaltyDaysFromStart1:
                    sPropertyValue = this.TotalTimelinessPenaltyDaysFromStart1.ToString();
                    break;
                case TTimelinessPenalty2:
                    sPropertyValue = this.TotalTimelinessPenalty2.ToString();
                    break;
                case TTimelinessPenaltyDaysFromStart2:
                    sPropertyValue = this.TotalTimelinessPenaltyDaysFromStart2.ToString();
                    break;
                case TTotalWorkdaysLimit:
                    sPropertyValue = this.TotalWorkdaysLimit.ToString();
                    break;
                case TFieldCapacity:
                    sPropertyValue = this.TotalFieldCapacity.ToString();
                    break;
                case TAreaCovered:
                    sPropertyValue = this.TotalAreaCovered.ToString();
                    break;
                case TFieldDays:
                    sPropertyValue = this.TotalFieldDays.ToString();
                    break;
                case TOutputPrice:
                    sPropertyValue = this.TotalOutputPrice.ToString();
                    break;
                case TOutputYield:
                    sPropertyValue = this.TotalOutputYield.ToString();
                    break;
                case TCompositionAmount:
                    sPropertyValue = this.TotalCompositionAmount.ToString();
                    break;
                case TOutputTimes:
                    sPropertyValue = this.TotalOutputTimes.ToString();
                    break;
                case TProbableFieldDays:
                    sPropertyValue = this.TotalProbableFieldDays.ToString();
                    break;
                case TTimelinessPenaltyCost:
                    sPropertyValue = this.TotalTimelinessPenaltyCost.ToString();
                    break;
                case TTimelinessPenaltyCostPerHour:
                    sPropertyValue = this.TotalTimelinessPenaltyCostPerHour.ToString();
                    break;
                case CostBenefitCalculator.TR:
                    sPropertyValue = this.TotalR.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalMachinery2StockAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAmount, attNameExtension),
                this.TotalAmount);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TWorkdayProbability, attNameExtension),
                this.TotalWorkdayProbability);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TLaborAvailable, attNameExtension),
                this.TotalLaborAvailable);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTimelinessPenalty1, attNameExtension),
                this.TotalTimelinessPenalty1);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTimelinessPenaltyDaysFromStart1, attNameExtension),
                this.TotalTimelinessPenaltyDaysFromStart1);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTimelinessPenalty2, attNameExtension),
                this.TotalTimelinessPenalty2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTimelinessPenaltyDaysFromStart2, attNameExtension),
                this.TotalTimelinessPenaltyDaysFromStart2);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTotalWorkdaysLimit, attNameExtension),
                this.TotalWorkdaysLimit);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TFieldCapacity, attNameExtension),
                this.TotalFieldCapacity);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TAreaCovered, attNameExtension),
                this.TotalAreaCovered);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TFieldDays, attNameExtension),
                this.TotalFieldDays);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOutputPrice, attNameExtension),
                this.TotalOutputPrice);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOutputYield, attNameExtension),
                this.TotalOutputYield);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TCompositionAmount, attNameExtension),
                this.TotalCompositionAmount);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TOutputTimes, attNameExtension),
                this.TotalOutputTimes);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TProbableFieldDays, attNameExtension),
                this.TotalProbableFieldDays);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTimelinessPenaltyCost, attNameExtension),
                this.TotalTimelinessPenaltyCost);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(TTimelinessPenaltyCostPerHour, attNameExtension),
                this.TotalTimelinessPenaltyCostPerHour);
            CalculatorHelpers.SetAttributeDoubleF3(calculator,
                string.Concat(CostBenefitCalculator.TR, attNameExtension),
                this.TotalR);
        }
        public virtual void SetTotalMachinery2StockAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TAmount, attNameExtension),
                this.TotalAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(TWorkdayProbability, attNameExtension),
                this.TotalWorkdayProbability.ToString());
            writer.WriteAttributeString(
                string.Concat(TLaborAvailable, attNameExtension),
                this.TotalLaborAvailable.ToString());
            writer.WriteAttributeString(
                string.Concat(TTimelinessPenalty1, attNameExtension),
                this.TotalTimelinessPenalty1.ToString());
            writer.WriteAttributeString(
                string.Concat(TTimelinessPenaltyDaysFromStart1, attNameExtension),
                this.TotalTimelinessPenaltyDaysFromStart1.ToString());
            writer.WriteAttributeString(
               string.Concat(TTimelinessPenalty2, attNameExtension),
               this.TotalTimelinessPenalty2.ToString());
            writer.WriteAttributeString(
                string.Concat(TTimelinessPenaltyDaysFromStart2, attNameExtension),
                this.TotalTimelinessPenaltyDaysFromStart2.ToString());
            writer.WriteAttributeString(
                string.Concat(TTotalWorkdaysLimit, attNameExtension),
                this.TotalWorkdaysLimit.ToString());
            writer.WriteAttributeString(
                string.Concat(TFieldCapacity, attNameExtension),
                this.TotalFieldCapacity.ToString());
            writer.WriteAttributeString(
                string.Concat(TAreaCovered, attNameExtension),
                this.TotalAreaCovered.ToString());
            writer.WriteAttributeString(
                string.Concat(TFieldDays, attNameExtension),
                this.TotalFieldDays.ToString());
            writer.WriteAttributeString(
                string.Concat(TOutputPrice, attNameExtension),
                this.TotalOutputPrice.ToString());
            writer.WriteAttributeString(
                string.Concat(TOutputYield, attNameExtension),
                this.TotalOutputYield.ToString());
            writer.WriteAttributeString(
                string.Concat(TCompositionAmount, attNameExtension),
                this.TotalCompositionAmount.ToString());
            writer.WriteAttributeString(
                string.Concat(TOutputTimes, attNameExtension),
                this.TotalOutputTimes.ToString());
            writer.WriteAttributeString(
               string.Concat(TProbableFieldDays, attNameExtension),
               this.TotalProbableFieldDays.ToString());
            writer.WriteAttributeString(
                string.Concat(TTimelinessPenaltyCost, attNameExtension),
                this.TotalTimelinessPenaltyCost.ToString());
            writer.WriteAttributeString(
                string.Concat(TTimelinessPenaltyCostPerHour, attNameExtension),
                this.TotalTimelinessPenaltyCostPerHour.ToString());
            writer.WriteAttributeString(
                string.Concat(CostBenefitCalculator.TR, attNameExtension),
                this.TotalR.ToString());
        }
    }
    public static class Machinery2Extensions
    {
        //add a TimelinessOpComp1 to the baseStat.Machinery2Stocks dictionary
        public static bool AddMachinery2StocksToDictionary(
            this Machinery2Stock baseStat,
            int filePosition, int nodePosition, TimelinessOpComp1 calculator)
        {
            bool bIsAdded = false;
            if (filePosition < 0 || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.Machinery2Stocks == null)
                baseStat.Machinery2Stocks
                = new Dictionary<int, List<TimelinessOpComp1>>();
            if (baseStat.Machinery2Stocks.ContainsKey(filePosition))
            {
                if (baseStat.Machinery2Stocks[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.Machinery2Stocks[filePosition].Count <= i)
                        {
                            baseStat.Machinery2Stocks[filePosition]
                                .Add(new TimelinessOpComp1());
                        }
                    }
                    baseStat.Machinery2Stocks[filePosition][nodePosition]
                        = calculator;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<TimelinessOpComp1> baseStats
                    = new List<TimelinessOpComp1>();
                KeyValuePair<int, List<TimelinessOpComp1>> newStat
                    = new KeyValuePair<int, List<TimelinessOpComp1>>(
                        filePosition, baseStats);
                baseStat.Machinery2Stocks.Add(newStat);
                bIsAdded = AddMachinery2StocksToDictionary(baseStat,
                    filePosition, nodePosition, calculator);
            }
            return bIsAdded;
        }
        public static int GetNodePositionCount(this Machinery2Stock baseStat,
            int filePosition, TimelinessOpComp1 calculator)
        {
            int iNodeCount = 0;
            if (baseStat.Machinery2Stocks == null)
                return iNodeCount;
            if (baseStat.Machinery2Stocks.ContainsKey(filePosition))
            {
                if (baseStat.Machinery2Stocks[filePosition] != null)
                {
                    iNodeCount = baseStat.Machinery2Stocks[filePosition].Count;
                }
            }
            return iNodeCount;
        }
    }
}
