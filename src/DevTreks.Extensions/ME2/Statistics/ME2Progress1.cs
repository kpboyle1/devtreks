using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		ME2Stock.Stocks.ME2Progress1.Stocks.ME2Progress1.ME2Indicators
    ///             ME2Stock.Stocks is a collection of ME2Stocks (unique observations)
    ///             Each member of ME2Stocks holds an analyzer stock (Progress)
    ///             Each analyzer stock (Progress) holds a collection of Progress1s
    ///             The class measures planned vs actual progress.
    ///Author:		www.devtreks.org
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class ME2Progress1 : ME2Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2Progress1(CalculatorParameters calcParams)
            : base(calcParams)
        {
            //subprice object
            InitTotalME2Progress1Properties(this);
        }
        #region
        //note that display properties, such as name, description, unit are in 
        //parent ME2Stock calculator properties
        //the total properties come from ME2IndicatorStock
        //planned period
        //planned full (sum of all planning periods)
        public double TotalPFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalPCTotal { get; set; }
        //actual period
        public double TotalAPTotal { get; set; }
        //actual cumulative 
        public double TotalACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalACChange { get; set; }
        //planned period
        public double TotalPPPercent { get; set; }
        //planned cumulative
        public double TotalPCPercent { get; set; }
        //planned full
        public double TotalPFPercent { get; set; }

        public double TotalQ1PFTotal { get; set; }
        public double TotalQ1PCTotal { get; set; }
        public double TotalQ1APTotal { get; set; }
        public double TotalQ1ACTotal { get; set; }
        public double TotalQ1APChange { get; set; }
        public double TotalQ1ACChange { get; set; }
        public double TotalQ1PPPercent { get; set; }
        public double TotalQ1PCPercent { get; set; }
        public double TotalQ1PFPercent { get; set; }

        public double TotalQ2PFTotal { get; set; }
        public double TotalQ2PCTotal { get; set; }
        public double TotalQ2APTotal { get; set; }
        public double TotalQ2ACTotal { get; set; }
        public double TotalQ2APChange { get; set; }
        public double TotalQ2ACChange { get; set; }
        public double TotalQ2PPPercent { get; set; }
        public double TotalQ2PCPercent { get; set; }
        public double TotalQ2PFPercent { get; set; }

        private const string cTotalPFTotal = "TPFTotal";
        private const string cTotalPCTotal = "TPCTotal";
        private const string cTotalAPTotal = "TAPTotal";
        private const string cTotalACTotal = "TACTotal";
        private const string cTotalAPChange = "TAPChange";
        private const string cTotalACChange = "TACChange";
        private const string cTotalPPPercent = "TPPPercent";
        private const string cTotalPCPercent = "TPCPercent";
        private const string cTotalPFPercent = "TPFPercent";

        private const string cTotalQ1PFTotal = "TQ1PFTotal";
        private const string cTotalQ1PCTotal = "TQ1PCTotal";
        private const string cTotalQ1APTotal = "TQ1APTotal";
        private const string cTotalQ1ACTotal = "TQ1ACTotal";
        private const string cTotalQ1APChange = "TQ1APChange";
        private const string cTotalQ1ACChange = "TQ1ACChange";
        private const string cTotalQ1PPPercent = "TQ1PPPercent";
        private const string cTotalQ1PCPercent = "TQ1PCPercent";
        private const string cTotalQ1PFPercent = "TQ1PFPercent";

        private const string cTotalQ2PFTotal = "TQ2PFTotal";
        private const string cTotalQ2PCTotal = "TQ2PCTotal";
        private const string cTotalQ2APTotal = "TQ2APTotal";
        private const string cTotalQ2ACTotal = "TQ2ACTotal";
        private const string cTotalQ2APChange = "TQ2APChange";
        private const string cTotalQ2ACChange = "TQ2ACChange";
        private const string cTotalQ2PPPercent = "TQ2PPPercent";
        private const string cTotalQ2PCPercent = "TQ2PCPercent";
        private const string cTotalQ2PFPercent = "TQ2PFPercent";
        public void InitTotalME2Progress1Properties(ME2Progress1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = new CalculatorParameters();
            InitTotalME2IndicatorStockProperties(ind);

            ind.TotalPFTotal = 0;
            ind.TotalPCTotal = 0;
            ind.TotalAPTotal = 0;
            ind.TotalACTotal = 0;
            ind.TotalAPChange = 0;
            ind.TotalACChange = 0;
            ind.TotalPPPercent = 0;
            ind.TotalPCPercent = 0;
            ind.TotalPFPercent = 0;

            ind.TotalQ1PFTotal = 0;
            ind.TotalQ1PCTotal = 0;
            ind.TotalQ1APTotal = 0;
            ind.TotalQ1ACTotal = 0;
            ind.TotalQ1APChange = 0;
            ind.TotalQ1ACChange = 0;
            ind.TotalQ1PPPercent = 0;
            ind.TotalQ1PCPercent = 0;
            ind.TotalQ1PFPercent = 0;

            ind.TotalQ2PFTotal = 0;
            ind.TotalQ2PCTotal = 0;
            ind.TotalQ2APTotal = 0;
            ind.TotalQ2ACTotal = 0;
            ind.TotalQ2APChange = 0;
            ind.TotalQ2ACChange = 0;
            ind.TotalQ2PPPercent = 0;
            ind.TotalQ2PCPercent = 0;
            ind.TotalQ2PFPercent = 0;
        }
        public void CopyTotalME2Progress1Properties(ME2Progress1 calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            //copy the initial totals and the indicators (used in RunAnalyses)
            CopyTotalME2IndicatorStockProperties(this, calculator);
            //copy the stats properties
            CopyME2Progress1Properties(this, calculator);
            //copy the calculator.ME2Stocks collection
            if (this.Stocks == null)
                this.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //copy the calculated totals and the indicators
            //obsStock.Progress1.Stocks holds a collection of change1s
            if (calculator.Stocks != null)
            {
                foreach (ME2Stock statStock in calculator.Stocks)
                {
                    ME2Progress1 stat = new ME2Progress1(this.CalcParameters);
                    if (statStock.GetType().Equals(stat.GetType()))
                    {
                        stat = (ME2Progress1)statStock;
                        if (stat != null)
                        {
                            ME2Progress1 newStat = new ME2Progress1(this.CalcParameters);
                            //copy the totals and the indicators
                            CopyTotalME2IndicatorStockProperties(newStat, stat);
                            //copy the stats properties
                            CopyME2Progress1Properties(newStat, stat);
                            //this refers to me2Stock.Stocks[x].Progress1
                            this.Stocks.Add(newStat);
                        }
                    }
                }
            }
        }
        public void CopyME2Progress1Properties(ME2Progress1 ind,
            ME2Progress1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalPFTotal = calculator.TotalPFTotal;
            ind.TotalPCTotal = calculator.TotalPCTotal;
            ind.TotalAPTotal = calculator.TotalAPTotal;
            ind.TotalACTotal = calculator.TotalACTotal;
            ind.TotalAPChange = calculator.TotalAPChange;
            ind.TotalACChange = calculator.TotalACChange;
            ind.TotalPPPercent = calculator.TotalPPPercent;
            ind.TotalPCPercent = calculator.TotalPCPercent;
            ind.TotalPFPercent = calculator.TotalPFPercent;

            ind.TotalQ1PFTotal = calculator.TotalQ1PFTotal;
            ind.TotalQ1PCTotal = calculator.TotalQ1PCTotal;
            ind.TotalQ1APTotal = calculator.TotalQ1APTotal;
            ind.TotalQ1ACTotal = calculator.TotalQ1ACTotal;
            ind.TotalQ1APChange = calculator.TotalQ1APChange;
            ind.TotalQ1ACChange = calculator.TotalQ1ACChange;
            ind.TotalQ1PPPercent = calculator.TotalQ1PPPercent;
            ind.TotalQ1PCPercent = calculator.TotalQ1PCPercent;
            ind.TotalQ1PFPercent = calculator.TotalQ1PFPercent;

            ind.TotalQ2PFTotal = calculator.TotalQ2PFTotal;
            ind.TotalQ2PCTotal = calculator.TotalQ2PCTotal;
            ind.TotalQ2APTotal = calculator.TotalQ2APTotal;
            ind.TotalQ2ACTotal = calculator.TotalQ2ACTotal;
            ind.TotalQ2APChange = calculator.TotalQ2APChange;
            ind.TotalQ2ACChange = calculator.TotalQ2ACChange;
            ind.TotalQ2PPPercent = calculator.TotalQ2PPPercent;
            ind.TotalQ2PCPercent = calculator.TotalQ2PCPercent;
            ind.TotalQ2PFPercent = calculator.TotalQ2PFPercent;
        }

        public void SetTotalME2Progress1Properties(ME2Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            //stats always based on indicators
            ind.SetTotalME2IndicatorStockProperties(ind, attNameExtension, calculator);

            ind.TotalPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalPFTotal, attNameExtension));
            ind.TotalPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalPCTotal, attNameExtension));
            ind.TotalAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAPTotal, attNameExtension));
            ind.TotalACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalACTotal, attNameExtension));
            ind.TotalAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAPChange, attNameExtension));
            ind.TotalACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalACChange, attNameExtension));
            ind.TotalPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalPPPercent, attNameExtension));
            ind.TotalPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalPCPercent, attNameExtension));
            ind.TotalPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalPFPercent, attNameExtension));

            ind.TotalQ1PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1PFTotal, attNameExtension));
            ind.TotalQ1PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1PCTotal, attNameExtension));
            ind.TotalQ1APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1APTotal, attNameExtension));
            ind.TotalQ1ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1ACTotal, attNameExtension));
            ind.TotalQ1APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1APChange, attNameExtension));
            ind.TotalQ1ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1ACChange, attNameExtension));
            ind.TotalQ1PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1PPPercent, attNameExtension));
            ind.TotalQ1PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1PCPercent, attNameExtension));
            ind.TotalQ1PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ1PFPercent, attNameExtension));

            ind.TotalQ2PFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2PFTotal, attNameExtension));
            ind.TotalQ2PCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2PCTotal, attNameExtension));
            ind.TotalQ2APTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2APTotal, attNameExtension));
            ind.TotalQ2ACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2ACTotal, attNameExtension));
            ind.TotalQ2APChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2APChange, attNameExtension));
            ind.TotalQ2ACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2ACChange, attNameExtension));
            ind.TotalQ2PPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2PPPercent, attNameExtension));
            ind.TotalQ2PCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2PCPercent, attNameExtension));
            ind.TotalQ2PFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalQ2PFPercent, attNameExtension));
        }
        public void SetTotalME2Progress1Property(ME2Progress1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalPFTotal:
                    ind.TotalPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalPCTotal:
                    ind.TotalPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAPTotal:
                    ind.TotalAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalACTotal:
                    ind.TotalACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAPChange:
                    ind.TotalAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalACChange:
                    ind.TotalACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalPPPercent:
                    ind.TotalPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalPCPercent:
                    ind.TotalPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalPFPercent:
                    ind.TotalPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1PFTotal:
                    ind.TotalQ1PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1PCTotal:
                    ind.TotalQ1PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1APTotal:
                    ind.TotalQ1APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1ACTotal:
                    ind.TotalQ1ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1APChange:
                    ind.TotalQ1APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1ACChange:
                    ind.TotalQ1ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1PPPercent:
                    ind.TotalQ1PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1PCPercent:
                    ind.TotalQ1PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ1PFPercent:
                    ind.TotalQ1PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2PFTotal:
                    ind.TotalQ2PFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2PCTotal:
                    ind.TotalQ2PCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2APTotal:
                    ind.TotalQ2APTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2ACTotal:
                    ind.TotalQ2ACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2APChange:
                    ind.TotalQ2APChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2ACChange:
                    ind.TotalQ2ACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2PPPercent:
                    ind.TotalQ2PPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2PCPercent:
                    ind.TotalQ2PCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalQ2PFPercent:
                    ind.TotalQ2PFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public string GetTotalME2Progress1Property(ME2Progress1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalPFTotal:
                    sPropertyValue = ind.TotalPFTotal.ToString();
                    break;
                case cTotalPCTotal:
                    sPropertyValue = ind.TotalPCTotal.ToString();
                    break;
                case cTotalAPTotal:
                    sPropertyValue = ind.TotalAPTotal.ToString();
                    break;
                case cTotalACTotal:
                    sPropertyValue = ind.TotalACTotal.ToString();
                    break;
                case cTotalAPChange:
                    sPropertyValue = ind.TotalAPChange.ToString();
                    break;
                case cTotalACChange:
                    sPropertyValue = ind.TotalACChange.ToString();
                    break;
                case cTotalPPPercent:
                    sPropertyValue = ind.TotalPPPercent.ToString();
                    break;
                case cTotalPCPercent:
                    sPropertyValue = ind.TotalPCPercent.ToString();
                    break;
                case cTotalPFPercent:
                    sPropertyValue = ind.TotalPFPercent.ToString();
                    break;
                case cTotalQ1PFTotal:
                    sPropertyValue = ind.TotalQ1PFTotal.ToString();
                    break;
                case cTotalQ1PCTotal:
                    sPropertyValue = ind.TotalQ1PCTotal.ToString();
                    break;
                case cTotalQ1APTotal:
                    sPropertyValue = ind.TotalQ1APTotal.ToString();
                    break;
                case cTotalQ1ACTotal:
                    sPropertyValue = ind.TotalQ1ACTotal.ToString();
                    break;
                case cTotalQ1APChange:
                    sPropertyValue = ind.TotalQ1APChange.ToString();
                    break;
                case cTotalQ1ACChange:
                    sPropertyValue = ind.TotalQ1ACChange.ToString();
                    break;
                case cTotalQ1PPPercent:
                    sPropertyValue = ind.TotalQ1PPPercent.ToString();
                    break;
                case cTotalQ1PCPercent:
                    sPropertyValue = ind.TotalQ1PCPercent.ToString();
                    break;
                case cTotalQ1PFPercent:
                    sPropertyValue = ind.TotalQ1PFPercent.ToString();
                    break;
                case cTotalQ2PFTotal:
                    sPropertyValue = ind.TotalQ2PFTotal.ToString();
                    break;
                case cTotalQ2PCTotal:
                    sPropertyValue = ind.TotalQ2PCTotal.ToString();
                    break;
                case cTotalQ2APTotal:
                    sPropertyValue = ind.TotalQ2APTotal.ToString();
                    break;
                case cTotalQ2ACTotal:
                    sPropertyValue = ind.TotalQ2ACTotal.ToString();
                    break;
                case cTotalQ2APChange:
                    sPropertyValue = ind.TotalQ2APChange.ToString();
                    break;
                case cTotalQ2ACChange:
                    sPropertyValue = ind.TotalQ2ACChange.ToString();
                    break;
                case cTotalQ2PPPercent:
                    sPropertyValue = ind.TotalQ2PPPercent.ToString();
                    break;
                case cTotalQ2PCPercent:
                    sPropertyValue = ind.TotalQ2PCPercent.ToString();
                    break;
                case cTotalQ2PFPercent:
                    sPropertyValue = ind.TotalQ2PFPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalME2Progress1Attributes(string attNameExt,
             ref XmlWriter writer)
        {
            if (this.Stocks != null)
            {
                int i = 0;
                string sAttNameExtension = string.Empty;
                foreach (ME2Progress1 stat in this.Stocks)
                {
                    if (stat != null)
                    {
                        sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                        //this runs in ME2IndicatorStock object
                        SetTotalME2IndicatorStockAttributes(stat, sAttNameExtension, ref writer);
                        SetTotalME2Progress1Attributes(stat, sAttNameExtension, ref writer);
                    }
                    i++;
                }
            }
        }
        public void SetTotalME2Progress1Attributes(ME2Progress1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                    string.Concat(cTotalPFTotal, attNameExtension), ind.TotalPFTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalPCTotal, attNameExtension), ind.TotalPCTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalAPTotal, attNameExtension), ind.TotalAPTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalACTotal, attNameExtension), ind.TotalACTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalAPChange, attNameExtension), ind.TotalAPChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalACChange, attNameExtension), ind.TotalACChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalPPPercent, attNameExtension), ind.TotalPPPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalPCPercent, attNameExtension), ind.TotalPCPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalPFPercent, attNameExtension), ind.TotalPFPercent.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                    string.Concat(cTotalQ1PFTotal, attNameExtension), ind.TotalQ1PFTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1PCTotal, attNameExtension), ind.TotalQ1PCTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1APTotal, attNameExtension), ind.TotalQ1APTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1ACTotal, attNameExtension), ind.TotalQ1ACTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1APChange, attNameExtension), ind.TotalQ1APChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1ACChange, attNameExtension), ind.TotalQ1ACChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1PPPercent, attNameExtension), ind.TotalQ1PPPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1PCPercent, attNameExtension), ind.TotalQ1PCPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ1PFPercent, attNameExtension), ind.TotalQ1PFPercent.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                    string.Concat(cTotalQ2PFTotal, attNameExtension), ind.TotalQ2PFTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2PCTotal, attNameExtension), ind.TotalQ2PCTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2APTotal, attNameExtension), ind.TotalQ2APTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2ACTotal, attNameExtension), ind.TotalQ2ACTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2APChange, attNameExtension), ind.TotalQ2APChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2ACChange, attNameExtension), ind.TotalQ2ACChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2PPPercent, attNameExtension), ind.TotalQ2PPPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2PCPercent, attNameExtension), ind.TotalQ2PCPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalQ2PFPercent, attNameExtension), ind.TotalQ2PFPercent.ToString("N2", CultureInfo.InvariantCulture));
        }
        #endregion
        //calcs holds the collections needing statistical analysis
        public bool RunAnalyses(ME2Stock me2Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //convert calcs to totals
            List<Calculator1> totals = SetTotals(me2Stock, calcs);
            //run calcs and set up me2Stock.Stocks collection 
            bool bHasTotals = me2Stock.Total1.RunAnalyses(me2Stock, totals);
            //run a change analysis 
            //the alternative2 aggregator was already used in Total1, don't use it again
            if (bHasTotals)
            {
                bHasAnalyses = SetAnalyses(me2Stock);
            }
            return bHasAnalyses;
        }
        private List<Calculator1> SetTotals(ME2Stock me2Stock, List<Calculator1> calcs)
        {
            //build a list of initial totals that can be used to runtotals
            List<Calculator1> stocks = new List<Calculator1>();
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(me2Stock.GetType()))
                {
                    ME2Stock stock = (ME2Stock)calc;
                    if (stock != null)
                    {
                        //this initial calculator results are placed in this object
                        if (stock.Stocks != null)
                        {
                            List<ME2Stock> obsStocks = new List<ME2Stock>();
                            foreach (ME2Stock obsStock in stock.Stocks)
                            {
                                //id comes from original calc
                                obsStock.CopyCalculatorProperties(stock);
                                if (obsStock.Progress1 != null)
                                {
                                    obsStock.Total1 = new ME2Total1(this.CalcParameters);
                                    if (obsStock.Progress1.ME2Indicators != null)
                                    {
                                        if (obsStock.Progress1.ME2Indicators.Count > 0)
                                        {
                                            obsStock.Total1.CopyME2IndicatorsProperties(obsStock.Progress1);
                                            //id comes from original calc
                                            obsStock.Total1.CopyCalculatorProperties(stock);
                                            //clear the initial indicators
                                            obsStock.Progress1.ME2Indicators = new List<ME2Indicator>();
                                            obsStocks.Add(obsStock);
                                        }
                                    }
                                }
                            }
                            //reset stock.Storks
                            stock.Stocks = new List<ME2Stock>();
                            foreach (ME2Stock ostock in obsStocks)
                            {
                                stock.Stocks.Add(ostock);
                            }
                            stocks.Add(stock);
                        }
                    }
                }
            }
            return stocks;
        }
        private bool SetAnalyses(ME2Stock me2Stock)
        {
            bool bHasTotals = false;
            if (me2Stock.Stocks != null)
            {
                bHasTotals = SetProgressAnalysis(me2Stock);
            }
            return bHasTotals;
        }
        private bool SetProgressAnalysis(ME2Stock me2Stock)
        {
            bool bHasTotalProgress = false;
            if (me2Stock.Stocks != null)
            {
                //set//set change numbers
                bHasTotalProgress = SetProgress(me2Stock); 
            }
            return bHasTotalProgress;
        }
        private static bool SetProgress(ME2Stock me2Stock)
        {
            bool bHasProgress = false;
            //replace list of totalstocks with list of changestocks
            List<ME2Stock> obsStocks = new List<ME2Stock>();
            //each label will be used to set cumulative totals
            List<ME2Total1> cumTotals = new List<ME2Total1>();
            //process the benchmark first so that planned totals can be set
            foreach (ME2Stock stock in me2Stock.Stocks.OrderBy(c => c.Date))
            {
                //stock totals are contained in observation.Total1.Stocks (and Stocks are Total1s)
                if (stock.Total1 != null
                    && stock.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //unlike totals, obsstock needs obs.CopyCalcs so it matches with baseelement
                    ME2Stock observationStock = new ME2Stock(stock.CalcParameters,
                        me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                    //need the base el id
                    observationStock.CopyCalculatorProperties(stock);
                    //where the stats go
                    observationStock.Progress1 = new ME2Progress1(stock.CalcParameters);
                    observationStock.Progress1.CalcParameters = new CalculatorParameters(me2Stock.CalcParameters);
                    observationStock.Progress1.CopyCalculatorProperties(stock);
                    if (stock.Total1.Stocks != null)
                    {
                        foreach (ME2Total1 total in stock.Total1.Stocks)
                        {
                            //194 no dates or correct labels in stock.Total1.Stocks
                            total.CopyCalculatorProperties(stock);
                            //add to observationStock for potential Ancestor calcs use
                            observationStock.Progress1.InitTotalME2Progress1Properties(observationStock.Progress1);
                            observationStock.Progress1.CopyTotalME2IndicatorStockProperties(observationStock.Progress1, total);
                            //add to the cumulative totals
                            cumTotals.Add(total);
                        }
                    }
                    //each of the stocks is a unique label-dependent total
                    observationStock.Progress1.Stocks = new List<ME2Stock>();
                    //add the planned progress 
                    //(these must be run first so that planned totals can be set for actual)
                    AddPlannedProgressToStock(cumTotals, me2Stock, stock, observationStock);
                    obsStocks.Add(observationStock);
                }
            }
            //set the planned full totals
            foreach (ME2Stock stock in obsStocks.OrderBy(c => c.Date))
            {
                //stock totals are contained in observation.Progress1.Stocks (and Stocks are Progress11s)
                if (stock.Progress1 != null
                    && stock.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    double dbPlannedTotal = 0;
                    double dbPlannedTotalQ1 = 0;
                    double dbPlannedTotalQ2 = 0;
                    if (stock.Progress1.Stocks != null)
                    {
                        foreach (ME2Progress1 progress in stock.Progress1.Stocks)
                        {
                            foreach (ME2Total1 cumtotal in cumTotals)
                            {
                                if (progress.TME2Label == cumtotal.TME2Label)
                                {
                                    dbPlannedTotal += cumtotal.TME2TMAmount;
                                    dbPlannedTotalQ1 += cumtotal.TME2TLAmount;
                                    dbPlannedTotalQ2 += cumtotal.TME2TUAmount;
                                }
                            }
                            progress.TotalPFTotal = dbPlannedTotal;
                            progress.TotalQ1PFTotal = dbPlannedTotalQ1;
                            progress.TotalQ2PFTotal = dbPlannedTotalQ2;
                            dbPlannedTotal = 0;
                            dbPlannedTotalQ1 = 0;
                            dbPlannedTotalQ2 = 0;
                        }
                    }
                }
            }
            //loop through the indicator label-aggregated totals
            cumTotals = new List<ME2Total1>();
            List<int> ids = new List<int>();
            foreach (ME2Stock stock in me2Stock.Stocks.OrderBy(c => c.Date))
            {
                //stock totals are contained in observation.Total1.Stocks (and Stocks are Total1s)
                if (stock.Total1 != null
                    && stock.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //unlike totals, obsstock needs obs.CopyCalcs so it matches with baseelement
                    ME2Stock observationStock = new ME2Stock(stock.CalcParameters,
                        me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                    //need the base el id
                    observationStock.CopyCalculatorProperties(stock);
                    //where the stats go
                    observationStock.Progress1 = new ME2Progress1(stock.CalcParameters);
                    observationStock.Progress1.CalcParameters = new CalculatorParameters(me2Stock.CalcParameters);
                    observationStock.Progress1.CopyCalculatorProperties(stock);
                    if (stock.Total1.Stocks != null)
                    {
                        foreach (ME2Total1 total in stock.Total1.Stocks)
                        {
                            //add to observationStock for potential Ancestor calcs use
                            observationStock.Progress1.InitTotalME2Progress1Properties(observationStock.Progress1);
                            observationStock.Progress1.CopyTotalME2IndicatorStockProperties(observationStock.Progress1, total);
                            //add to the cumulative totals
                            cumTotals.Add(total);
                        }
                    }
                    //each of the stocks is a unique label-dependent total
                    observationStock.Progress1.Stocks = new List<ME2Stock>();
                    //add the actual progress
                    AddActualProgressToStock(cumTotals, ids, obsStocks, stock, observationStock);
                    obsStocks.Add(observationStock);
                }
            }
            if (obsStocks.Count > 0)
            {
                //replace the totalstocks with change stocks
                me2Stock.Stocks = obsStocks;
                bHasProgress = true;
            }
            return bHasProgress;
        }
        private static void AddPlannedProgressToStock(List<ME2Total1> cumTotals, 
            ME2Stock me2Stock, ME2Stock planned, ME2Stock observationStock)
        {
            double dbPlannedTotal = 0;
            double dbPlannedTotalQ1 = 0;
            double dbPlannedTotalQ2 = 0;
            if (planned.Total1.Stocks != null)
            {
                foreach (ME2Total1 total in planned.Total1.Stocks.OrderBy(c => c.Date))
                {
                    ME2Progress1 newProgress = new ME2Progress1(observationStock.CalcParameters);
                    //194 no dates or correct labels in newprogress
                    newProgress.CopyCalculatorProperties(total);
                    //set props to zero
                    newProgress.InitTotalME2Progress1Properties(newProgress);
                    newProgress.CopyTotalME2IndicatorStockProperties(newProgress, total);
                    if (newProgress.ME2Indicators != null)
                    {
                        //set N
                        newProgress.TME2N = newProgress.ME2Indicators.Count;
                    };
                    //set planned period totals
                    newProgress.TME2TMAmount = total.TME2TMAmount;
                    newProgress.TME2TLAmount = total.TME2TLAmount;
                    newProgress.TME2TUAmount = total.TME2TUAmount;
                    //set planned cumulative
                    dbPlannedTotal = 0;
                    dbPlannedTotalQ1 = 0;
                    dbPlannedTotalQ2 = 0;
                    foreach (ME2Total1 cumtotal in cumTotals.OrderBy(c => c.Date))
                    {
                        if (total.TME2Label == cumtotal.TME2Label)
                        {
                            dbPlannedTotal += cumtotal.TME2TMAmount;
                            dbPlannedTotalQ1 += cumtotal.TME2TLAmount;
                            dbPlannedTotalQ2 += cumtotal.TME2TUAmount;
                        }
                    }
                    newProgress.TotalPCTotal = dbPlannedTotal;
                    newProgress.TotalQ1PCTotal = dbPlannedTotalQ1;
                    newProgress.TotalQ2PCTotal = dbPlannedTotalQ2;
                    //add new change to observationStock.Progress1.Stocks
                    observationStock.Progress1.Stocks.Add(newProgress);
                }
            }
        }
        private static void AddActualProgressToStock(List<ME2Total1> cumTotals,
            List<int> ids, List<ME2Stock> obsStocks, ME2Stock actual, ME2Stock observationStock)
        {
            double dbActualTotal = 0;
            double dbActualTotalQ1 = 0;
            double dbActualTotalQ2 = 0;
            //get the corresponding planned totals
            ME2Stock planned = GetProgressStockByLabel(
                actual, ids, obsStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
            if (actual.Total1.Stocks != null)
            {
                foreach (ME2Total1 total in actual.Total1.Stocks.OrderBy(c => c.Date))
                {
                    ME2Progress1 newProgress = new ME2Progress1(observationStock.CalcParameters);
                    newProgress.InitTotalME2Progress1Properties(newProgress);
                    newProgress.CopyTotalME2IndicatorStockProperties(newProgress, total);
                    if (newProgress.ME2Indicators != null)
                    {
                        //set N
                        newProgress.TME2N = newProgress.ME2Indicators.Count;
                    };
                    //set planned cumulative
                    dbActualTotal = 0;
                    dbActualTotalQ1 = 0;
                    dbActualTotalQ2 = 0;
                    foreach (ME2Total1 cumtotal in cumTotals.OrderBy(c => c.Date))
                    {
                        if (total.TME2Label == cumtotal.TME2Label)
                        {
                            dbActualTotal += cumtotal.TME2TMAmount;
                            dbActualTotalQ1 += cumtotal.TME2TLAmount;
                            dbActualTotalQ2 += cumtotal.TME2TUAmount;
                        }
                    }
                    newProgress.TotalACTotal = dbActualTotal;
                    newProgress.TotalQ1ACTotal = dbActualTotalQ1;
                    newProgress.TotalQ2ACTotal = dbActualTotalQ2;

                    //set actual period using last actual total
                    newProgress.TotalAPTotal = newProgress.TME2TMAmount;
                    //q1
                    //set actual period using last actual total
                    newProgress.TotalQ1APTotal = newProgress.TME2TLAmount;
                    //q2
                    //set actual period using last actual total
                    newProgress.TotalQ2APTotal = newProgress.TME2TUAmount;
                    //set the corresponding planned totals
                    if (planned != null)
                    {
                        if (planned.Progress1 != null)
                        {
                            foreach (ME2Progress1 progress in planned.Progress1.Stocks)
                            {
                                if (progress.TME2Label == total.TME2Label)
                                {
                                    //set actual.planned cumulative
                                    newProgress.TotalPCTotal = progress.TotalPCTotal;
                                    //set actual.planned period
                                    //Total is always planned period and TotalAPTotal is actual period
                                    newProgress.TME2TMAmount = progress.TME2TMAmount;
                                    //the planned fulltotal to the planned full total
                                    newProgress.TotalPFTotal = progress.TotalPFTotal;
                                    //q1
                                    newProgress.TotalQ1PCTotal = progress.TotalQ1PCTotal;
                                    newProgress.TME2TLAmount = progress.TME2TLAmount;
                                    newProgress.TotalQ1PFTotal = progress.TotalQ1PFTotal;
                                    //q1
                                    newProgress.TotalQ2PCTotal = progress.TotalQ2PCTotal;
                                    newProgress.TME2TUAmount = progress.TME2TUAmount;
                                    newProgress.TotalQ2PFTotal = progress.TotalQ2PFTotal;
                                }
                            }
                        }
                    }
                    //set the variances
                    //partial period change
                    newProgress.TotalAPChange = newProgress.TotalAPTotal - newProgress.TME2TMAmount;
                    //cumulative change
                    newProgress.TotalACChange = newProgress.TotalACTotal - newProgress.TotalPCTotal;
                    //set planned period percent
                    newProgress.TotalPPPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalAPTotal, newProgress.TME2TMAmount);
                    newProgress.TotalPCPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalACTotal, newProgress.TotalPCTotal);
                    newProgress.TotalPFPercent
                            = CalculatorHelpers.GetPercent(newProgress.TotalACTotal, newProgress.TotalPFTotal);
                    //q1
                    newProgress.TotalQ1APChange = newProgress.TotalQ1APTotal - newProgress.TME2TLAmount;
                    newProgress.TotalQ1ACChange = newProgress.TotalQ1ACTotal - newProgress.TotalQ1PCTotal;
                    newProgress.TotalQ1PPPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalQ1APTotal, newProgress.TME2TLAmount);
                    newProgress.TotalQ1PCPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalQ1ACTotal, newProgress.TotalQ1PCTotal);
                    newProgress.TotalQ1PFPercent
                            = CalculatorHelpers.GetPercent(newProgress.TotalQ1ACTotal, newProgress.TotalQ1PFTotal);
                    //q2
                    newProgress.TotalQ2APChange = newProgress.TotalQ2APTotal - newProgress.TME2TUAmount;
                    newProgress.TotalQ2ACChange = newProgress.TotalQ2ACTotal - newProgress.TotalQ2PCTotal;
                    newProgress.TotalQ2PPPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalQ2APTotal, newProgress.TME2TUAmount);
                    newProgress.TotalQ2PCPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalQ2ACTotal, newProgress.TotalQ2PCTotal);
                    newProgress.TotalQ2PFPercent
                            = CalculatorHelpers.GetPercent(newProgress.TotalQ2ACTotal, newProgress.TotalQ2PFTotal);
                    //add new change to observationStock.Progress1.Stocks
                    observationStock.Progress1.Stocks.Add(newProgress);
                }
            }
        }
        private static ME2Stock GetProgressStockByLabel(ME2Stock actual, List<int> ids,
            List<ME2Stock> obsStocks, string targetType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            ME2Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (obsStocks.Any(p => p.Label == actual.Label
                && p.TargetType == targetType))
            {
                //2.0.4 went to zero based index, double check that this is not important
                int iIndex = 1;
                foreach (ME2Stock planned in obsStocks)
                {
                    if (planned.TargetType == targetType)
                    {
                        if (actual.Label == planned.Label)
                        {
                            //make sure it hasn't already been used (2 or more els with same Labels)
                            if (!ids.Any(i => i == iIndex))
                            {
                                plannedMatch = planned;
                                //index based check is ok
                                ids.Add(iIndex);
                                //break the for loop
                                break;
                            }
                            else
                            {
                                bool bHasMatch = HasProgressMatchByLabel(actual.Label, planned,
                                    obsStocks, targetType);
                                if (!bHasMatch)
                                {
                                    //if no match use the last one (i.e. input series with 1 bm and 20 actuals)
                                    plannedMatch = obsStocks.LastOrDefault(p => p.Label == actual.Label
                                         && p.TargetType == targetType);
                                    break;
                                }
                            }
                        }
                    }
                    iIndex++;
                }
            }
            return plannedMatch;
        }
        private static bool HasProgressMatchByLabel(string aggLabel,
            ME2Stock planned, List<ME2Stock> progressStocks, string targetType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (ME2Stock rp in progressStocks)
            {
                if (rp.TargetType == targetType)
                {
                    if (bStart)
                    {
                        if (aggLabel == planned.Label)
                        {
                            bHasMatch = true;
                            break;
                        }
                    }
                    if (rp.Id == planned.Id)
                    {
                        bStart = true;
                    }
                }
            }
            return bHasMatch;
        }
    }
}
