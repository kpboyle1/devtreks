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
        public double TotalMPFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalMPCTotal { get; set; }
        //actual period
        public double TotalMAPTotal { get; set; }
        //actual cumulative 
        public double TotalMACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalMAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalMACChange { get; set; }
        //planned period
        public double TotalMPPPercent { get; set; }
        //planned cumulative
        public double TotalMPCPercent { get; set; }
        //planned full
        public double TotalMPFPercent { get; set; }

        public double TotalLPFTotal { get; set; }
        public double TotalLPCTotal { get; set; }
        public double TotalLAPTotal { get; set; }
        public double TotalLACTotal { get; set; }
        public double TotalLAPChange { get; set; }
        public double TotalLACChange { get; set; }
        public double TotalLPPPercent { get; set; }
        public double TotalLPCPercent { get; set; }
        public double TotalLPFPercent { get; set; }

        public double TotalUPFTotal { get; set; }
        public double TotalUPCTotal { get; set; }
        public double TotalUAPTotal { get; set; }
        public double TotalUACTotal { get; set; }
        public double TotalUAPChange { get; set; }
        public double TotalUACChange { get; set; }
        public double TotalUPPPercent { get; set; }
        public double TotalUPCPercent { get; set; }
        public double TotalUPFPercent { get; set; }

        private const string cTotalMPFTotal = "TMPFTotal";
        private const string cTotalMPCTotal = "TMPCTotal";
        private const string cTotalMAPTotal = "TMAPTotal";
        private const string cTotalMACTotal = "TMACTotal";
        private const string cTotalMAPChange = "TMAPChange";
        private const string cTotalMACChange = "TMACChange";
        private const string cTotalMPPPercent = "TMPPPercent";
        private const string cTotalMPCPercent = "TMPCPercent";
        private const string cTotalMPFPercent = "TMPFPercent";

        private const string cTotalLPFTotal = "TLPFTotal";
        private const string cTotalLPCTotal = "TLPCTotal";
        private const string cTotalLAPTotal = "TLAPTotal";
        private const string cTotalLACTotal = "TLACTotal";
        private const string cTotalLAPChange = "TLAPChange";
        private const string cTotalLACChange = "TLACChange";
        private const string cTotalLPPPercent = "TLPPPercent";
        private const string cTotalLPCPercent = "TLPCPercent";
        private const string cTotalLPFPercent = "TLPFPercent";

        private const string cTotalUPFTotal = "TUPFTotal";
        private const string cTotalUPCTotal = "TUPCTotal";
        private const string cTotalUAPTotal = "TUAPTotal";
        private const string cTotalUACTotal = "TUACTotal";
        private const string cTotalUAPChange = "TUAPChange";
        private const string cTotalUACChange = "TUACChange";
        private const string cTotalUPPPercent = "TUPPPercent";
        private const string cTotalUPCPercent = "TUPCPercent";
        private const string cTotalUPFPercent = "TUPFPercent";
        public void InitTotalME2Progress1Properties(ME2Progress1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = new CalculatorParameters();
            InitTotalME2IndicatorStockProperties(ind);

            ind.TotalMPFTotal = 0;
            ind.TotalMPCTotal = 0;
            ind.TotalMAPTotal = 0;
            ind.TotalMACTotal = 0;
            ind.TotalMAPChange = 0;
            ind.TotalMACChange = 0;
            ind.TotalMPPPercent = 0;
            ind.TotalMPCPercent = 0;
            ind.TotalMPFPercent = 0;

            ind.TotalLPFTotal = 0;
            ind.TotalLPCTotal = 0;
            ind.TotalLAPTotal = 0;
            ind.TotalLACTotal = 0;
            ind.TotalLAPChange = 0;
            ind.TotalLACChange = 0;
            ind.TotalLPPPercent = 0;
            ind.TotalLPCPercent = 0;
            ind.TotalLPFPercent = 0;

            ind.TotalUPFTotal = 0;
            ind.TotalUPCTotal = 0;
            ind.TotalUAPTotal = 0;
            ind.TotalUACTotal = 0;
            ind.TotalUAPChange = 0;
            ind.TotalUACChange = 0;
            ind.TotalUPPPercent = 0;
            ind.TotalUPCPercent = 0;
            ind.TotalUPFPercent = 0;
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
            ind.TotalMPFTotal = calculator.TotalMPFTotal;
            ind.TotalMPCTotal = calculator.TotalMPCTotal;
            ind.TotalMAPTotal = calculator.TotalMAPTotal;
            ind.TotalMACTotal = calculator.TotalMACTotal;
            ind.TotalMAPChange = calculator.TotalMAPChange;
            ind.TotalMACChange = calculator.TotalMACChange;
            ind.TotalMPPPercent = calculator.TotalMPPPercent;
            ind.TotalMPCPercent = calculator.TotalMPCPercent;
            ind.TotalMPFPercent = calculator.TotalMPFPercent;

            ind.TotalLPFTotal = calculator.TotalLPFTotal;
            ind.TotalLPCTotal = calculator.TotalLPCTotal;
            ind.TotalLAPTotal = calculator.TotalLAPTotal;
            ind.TotalLACTotal = calculator.TotalLACTotal;
            ind.TotalLAPChange = calculator.TotalLAPChange;
            ind.TotalLACChange = calculator.TotalLACChange;
            ind.TotalLPPPercent = calculator.TotalLPPPercent;
            ind.TotalLPCPercent = calculator.TotalLPCPercent;
            ind.TotalLPFPercent = calculator.TotalLPFPercent;

            ind.TotalUPFTotal = calculator.TotalUPFTotal;
            ind.TotalUPCTotal = calculator.TotalUPCTotal;
            ind.TotalUAPTotal = calculator.TotalUAPTotal;
            ind.TotalUACTotal = calculator.TotalUACTotal;
            ind.TotalUAPChange = calculator.TotalUAPChange;
            ind.TotalUACChange = calculator.TotalUACChange;
            ind.TotalUPPPercent = calculator.TotalUPPPercent;
            ind.TotalUPCPercent = calculator.TotalUPCPercent;
            ind.TotalUPFPercent = calculator.TotalUPFPercent;
        }

        public void SetTotalME2Progress1Properties(ME2Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            //stats always based on indicators
            ind.SetTotalME2IndicatorStockProperties(ind, attNameExtension, calculator);

            ind.TotalMPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMPFTotal, attNameExtension));
            ind.TotalMPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMPCTotal, attNameExtension));
            ind.TotalMAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMAPTotal, attNameExtension));
            ind.TotalMACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMACTotal, attNameExtension));
            ind.TotalMAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMAPChange, attNameExtension));
            ind.TotalMACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMACChange, attNameExtension));
            ind.TotalMPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMPPPercent, attNameExtension));
            ind.TotalMPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMPCPercent, attNameExtension));
            ind.TotalMPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalMPFPercent, attNameExtension));

            ind.TotalLPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLPFTotal, attNameExtension));
            ind.TotalLPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLPCTotal, attNameExtension));
            ind.TotalLAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLAPTotal, attNameExtension));
            ind.TotalLACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLACTotal, attNameExtension));
            ind.TotalLAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLAPChange, attNameExtension));
            ind.TotalLACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLACChange, attNameExtension));
            ind.TotalLPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLPPPercent, attNameExtension));
            ind.TotalLPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLPCPercent, attNameExtension));
            ind.TotalLPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLPFPercent, attNameExtension));

            ind.TotalUPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUPFTotal, attNameExtension));
            ind.TotalUPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUPCTotal, attNameExtension));
            ind.TotalUAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUAPTotal, attNameExtension));
            ind.TotalUACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUACTotal, attNameExtension));
            ind.TotalUAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUAPChange, attNameExtension));
            ind.TotalUACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUACChange, attNameExtension));
            ind.TotalUPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUPPPercent, attNameExtension));
            ind.TotalUPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUPCPercent, attNameExtension));
            ind.TotalUPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUPFPercent, attNameExtension));
        }
        public void SetTotalME2Progress1Property(ME2Progress1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalMPFTotal:
                    ind.TotalMPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMPCTotal:
                    ind.TotalMPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMAPTotal:
                    ind.TotalMAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMACTotal:
                    ind.TotalMACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMAPChange:
                    ind.TotalMAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMACChange:
                    ind.TotalMACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMPPPercent:
                    ind.TotalMPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMPCPercent:
                    ind.TotalMPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalMPFPercent:
                    ind.TotalMPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLPFTotal:
                    ind.TotalLPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLPCTotal:
                    ind.TotalLPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLAPTotal:
                    ind.TotalLAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLACTotal:
                    ind.TotalLACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLAPChange:
                    ind.TotalLAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLACChange:
                    ind.TotalLACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLPPPercent:
                    ind.TotalLPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLPCPercent:
                    ind.TotalLPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLPFPercent:
                    ind.TotalLPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUPFTotal:
                    ind.TotalUPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUPCTotal:
                    ind.TotalUPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUAPTotal:
                    ind.TotalUAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUACTotal:
                    ind.TotalUACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUAPChange:
                    ind.TotalUAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUACChange:
                    ind.TotalUACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUPPPercent:
                    ind.TotalUPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUPCPercent:
                    ind.TotalUPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUPFPercent:
                    ind.TotalUPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
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
                case cTotalMPFTotal:
                    sPropertyValue = ind.TotalMPFTotal.ToString();
                    break;
                case cTotalMPCTotal:
                    sPropertyValue = ind.TotalMPCTotal.ToString();
                    break;
                case cTotalMAPTotal:
                    sPropertyValue = ind.TotalMAPTotal.ToString();
                    break;
                case cTotalMACTotal:
                    sPropertyValue = ind.TotalMACTotal.ToString();
                    break;
                case cTotalMAPChange:
                    sPropertyValue = ind.TotalMAPChange.ToString();
                    break;
                case cTotalMACChange:
                    sPropertyValue = ind.TotalMACChange.ToString();
                    break;
                case cTotalMPPPercent:
                    sPropertyValue = ind.TotalMPPPercent.ToString();
                    break;
                case cTotalMPCPercent:
                    sPropertyValue = ind.TotalMPCPercent.ToString();
                    break;
                case cTotalMPFPercent:
                    sPropertyValue = ind.TotalMPFPercent.ToString();
                    break;
                case cTotalLPFTotal:
                    sPropertyValue = ind.TotalLPFTotal.ToString();
                    break;
                case cTotalLPCTotal:
                    sPropertyValue = ind.TotalLPCTotal.ToString();
                    break;
                case cTotalLAPTotal:
                    sPropertyValue = ind.TotalLAPTotal.ToString();
                    break;
                case cTotalLACTotal:
                    sPropertyValue = ind.TotalLACTotal.ToString();
                    break;
                case cTotalLAPChange:
                    sPropertyValue = ind.TotalLAPChange.ToString();
                    break;
                case cTotalLACChange:
                    sPropertyValue = ind.TotalLACChange.ToString();
                    break;
                case cTotalLPPPercent:
                    sPropertyValue = ind.TotalLPPPercent.ToString();
                    break;
                case cTotalLPCPercent:
                    sPropertyValue = ind.TotalLPCPercent.ToString();
                    break;
                case cTotalLPFPercent:
                    sPropertyValue = ind.TotalLPFPercent.ToString();
                    break;
                case cTotalUPFTotal:
                    sPropertyValue = ind.TotalUPFTotal.ToString();
                    break;
                case cTotalUPCTotal:
                    sPropertyValue = ind.TotalUPCTotal.ToString();
                    break;
                case cTotalUAPTotal:
                    sPropertyValue = ind.TotalUAPTotal.ToString();
                    break;
                case cTotalUACTotal:
                    sPropertyValue = ind.TotalUACTotal.ToString();
                    break;
                case cTotalUAPChange:
                    sPropertyValue = ind.TotalUAPChange.ToString();
                    break;
                case cTotalUACChange:
                    sPropertyValue = ind.TotalUACChange.ToString();
                    break;
                case cTotalUPPPercent:
                    sPropertyValue = ind.TotalUPPPercent.ToString();
                    break;
                case cTotalUPCPercent:
                    sPropertyValue = ind.TotalUPCPercent.ToString();
                    break;
                case cTotalUPFPercent:
                    sPropertyValue = ind.TotalUPFPercent.ToString();
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
                    string.Concat(cTotalMPFTotal, attNameExtension), ind.TotalMPFTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMPCTotal, attNameExtension), ind.TotalMPCTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMAPTotal, attNameExtension), ind.TotalMAPTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMACTotal, attNameExtension), ind.TotalMACTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMAPChange, attNameExtension), ind.TotalMAPChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMACChange, attNameExtension), ind.TotalMACChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMPPPercent, attNameExtension), ind.TotalMPPPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMPCPercent, attNameExtension), ind.TotalMPCPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalMPFPercent, attNameExtension), ind.TotalMPFPercent.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                    string.Concat(cTotalLPFTotal, attNameExtension), ind.TotalLPFTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLPCTotal, attNameExtension), ind.TotalLPCTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLAPTotal, attNameExtension), ind.TotalLAPTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLACTotal, attNameExtension), ind.TotalLACTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLAPChange, attNameExtension), ind.TotalLAPChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLACChange, attNameExtension), ind.TotalLACChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLPPPercent, attNameExtension), ind.TotalLPPPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLPCPercent, attNameExtension), ind.TotalLPCPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalLPFPercent, attNameExtension), ind.TotalLPFPercent.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                    string.Concat(cTotalUPFTotal, attNameExtension), ind.TotalUPFTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUPCTotal, attNameExtension), ind.TotalUPCTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUAPTotal, attNameExtension), ind.TotalUAPTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUACTotal, attNameExtension), ind.TotalUACTotal.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUAPChange, attNameExtension), ind.TotalUAPChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUACChange, attNameExtension), ind.TotalUACChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUPPPercent, attNameExtension), ind.TotalUPPPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUPCPercent, attNameExtension), ind.TotalUPCPercent.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalUPFPercent, attNameExtension), ind.TotalUPFPercent.ToString("N2", CultureInfo.InvariantCulture));
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
                                    ////204 allowed more flexibility with indicators 
                                    ////ancestors and siblings can have multiple inds with different labels and 1 stock for each ind collection
                                    if (obsStock.Progress1.Stocks != null)
                                    {
                                        if (obsStock.Progress1.Stocks.Count > 0)
                                        {
                                            int k = 0;
                                            foreach (var addedstock in obsStock.Progress1.Stocks)
                                            {
                                                if (addedstock.ME2Indicators.Count > 0)
                                                {
                                                    if (k == 0)
                                                    {
                                                        //this resets indicator list
                                                        obsStock.Progress1.CopyME2IndicatorsProperties(addedstock);
                                                    }
                                                    else
                                                    {
                                                        //when totals are run it will use ind.Label to add total to proper stock
                                                        obsStock.Progress1.AddME2IndicatorsProperties(addedstock);
                                                    }
                                                }
                                                k++;
                                            }
                                        }
                                    }
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
                            //reset stock.Stocks
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
            bool bHasTotalMProgress = false;
            if (me2Stock.Stocks != null)
            {
                //set//set change numbers
                bHasTotalMProgress = SetProgress(me2Stock); 
            }
            return bHasTotalMProgress;
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
                    double dbPlannedTotalL = 0;
                    double dbPlannedTotalU = 0;
                    if (stock.Progress1.Stocks != null)
                    {
                        foreach (ME2Progress1 progress in stock.Progress1.Stocks)
                        {
                            foreach (ME2Total1 cumtotal in cumTotals)
                            {
                                if (progress.TME2Label == cumtotal.TME2Label)
                                {
                                    dbPlannedTotal += cumtotal.TME2TMAmount;
                                    dbPlannedTotalL += cumtotal.TME2TLAmount;
                                    dbPlannedTotalU += cumtotal.TME2TUAmount;
                                }
                            }
                            progress.TotalMPFTotal = dbPlannedTotal;
                            progress.TotalLPFTotal = dbPlannedTotalL;
                            progress.TotalUPFTotal = dbPlannedTotalU;
                            dbPlannedTotal = 0;
                            dbPlannedTotalL = 0;
                            dbPlannedTotalU = 0;
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
            double dbPlannedTotalL = 0;
            double dbPlannedTotalU = 0;
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
                    dbPlannedTotalL = 0;
                    dbPlannedTotalU = 0;
                    foreach (ME2Total1 cumtotal in cumTotals.OrderBy(c => c.Date))
                    {
                        if (total.TME2Label == cumtotal.TME2Label)
                        {
                            dbPlannedTotal += cumtotal.TME2TMAmount;
                            dbPlannedTotalL += cumtotal.TME2TLAmount;
                            dbPlannedTotalU += cumtotal.TME2TUAmount;
                        }
                    }
                    newProgress.TotalMPCTotal = dbPlannedTotal;
                    newProgress.TotalLPCTotal = dbPlannedTotalL;
                    newProgress.TotalUPCTotal = dbPlannedTotalU;
                    //add new change to observationStock.Progress1.Stocks
                    observationStock.Progress1.Stocks.Add(newProgress);
                }
            }
        }
        private static void AddActualProgressToStock(List<ME2Total1> cumTotals,
            List<int> ids, List<ME2Stock> obsStocks, ME2Stock actual, ME2Stock observationStock)
        {
            double dbActualTotal = 0;
            double dbActualTotalL = 0;
            double dbActualTotalU = 0;
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
                    dbActualTotalL = 0;
                    dbActualTotalU = 0;
                    foreach (ME2Total1 cumtotal in cumTotals.OrderBy(c => c.Date))
                    {
                        if (total.TME2Label == cumtotal.TME2Label)
                        {
                            dbActualTotal += cumtotal.TME2TMAmount;
                            dbActualTotalL += cumtotal.TME2TLAmount;
                            dbActualTotalU += cumtotal.TME2TUAmount;
                        }
                    }
                    newProgress.TotalMACTotal = dbActualTotal;
                    newProgress.TotalLACTotal = dbActualTotalL;
                    newProgress.TotalUACTotal = dbActualTotalU;

                    //set actual period using last actual total
                    newProgress.TotalMAPTotal = newProgress.TME2TMAmount;
                    //q1
                    //set actual period using last actual total
                    newProgress.TotalLAPTotal = newProgress.TME2TLAmount;
                    //q2
                    //set actual period using last actual total
                    newProgress.TotalUAPTotal = newProgress.TME2TUAmount;
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
                                    newProgress.TotalMPCTotal = progress.TotalMPCTotal;
                                    //set actual.planned period
                                    //Total is always planned period and TotalMAPTotal is actual period
                                    newProgress.TME2TMAmount = progress.TME2TMAmount;
                                    //the planned fulltotal to the planned full total
                                    newProgress.TotalMPFTotal = progress.TotalMPFTotal;
                                    //q1
                                    newProgress.TotalLPCTotal = progress.TotalLPCTotal;
                                    newProgress.TME2TLAmount = progress.TME2TLAmount;
                                    newProgress.TotalLPFTotal = progress.TotalLPFTotal;
                                    //q1
                                    newProgress.TotalUPCTotal = progress.TotalUPCTotal;
                                    newProgress.TME2TUAmount = progress.TME2TUAmount;
                                    newProgress.TotalUPFTotal = progress.TotalUPFTotal;
                                }
                            }
                        }
                    }
                    //set the variances
                    //partial period change
                    newProgress.TotalMAPChange = newProgress.TotalMAPTotal - newProgress.TME2TMAmount;
                    //cumulative change
                    newProgress.TotalMACChange = newProgress.TotalMACTotal - newProgress.TotalMPCTotal;
                    //set planned period percent
                    newProgress.TotalMPPPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalMAPTotal, newProgress.TME2TMAmount);
                    newProgress.TotalMPCPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalMACTotal, newProgress.TotalMPCTotal);
                    newProgress.TotalMPFPercent
                            = CalculatorHelpers.GetPercent(newProgress.TotalMACTotal, newProgress.TotalMPFTotal);
                    //q1
                    newProgress.TotalLAPChange = newProgress.TotalLAPTotal - newProgress.TME2TLAmount;
                    newProgress.TotalLACChange = newProgress.TotalLACTotal - newProgress.TotalLPCTotal;
                    newProgress.TotalLPPPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalLAPTotal, newProgress.TME2TLAmount);
                    newProgress.TotalLPCPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalLACTotal, newProgress.TotalLPCTotal);
                    newProgress.TotalLPFPercent
                            = CalculatorHelpers.GetPercent(newProgress.TotalLACTotal, newProgress.TotalLPFTotal);
                    //q2
                    newProgress.TotalUAPChange = newProgress.TotalUAPTotal - newProgress.TME2TUAmount;
                    newProgress.TotalUACChange = newProgress.TotalUACTotal - newProgress.TotalUPCTotal;
                    newProgress.TotalUPPPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalUAPTotal, newProgress.TME2TUAmount);
                    newProgress.TotalUPCPercent
                        = CalculatorHelpers.GetPercent(newProgress.TotalUACTotal, newProgress.TotalUPCTotal);
                    newProgress.TotalUPFPercent
                            = CalculatorHelpers.GetPercent(newProgress.TotalUACTotal, newProgress.TotalUPFTotal);
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
                                    //if no match use the last one (i.e. input series with 1 bm and 15 actuals)
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
