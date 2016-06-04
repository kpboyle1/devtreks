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
    ///Purpose:		The ME2IndicatorStock class extends the ME2Calculator class.
    ///             This object acts as a calculator for the Indicators as well.
    ///Author:		www.devtreks.org
    ///Date:		2014, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       1. This class usually is initialized by finding the currentelement's 
    ///             ME2Calculator and setting the underlying ME2Calculator.ME2Indicators collection 
    ///             using the calculator properties. Those indicators are then used for 
    ///             all subsequent analyses.
    ///             
    ///</summary>
    public class ME2IndicatorStock : ME2Calculator 
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2IndicatorStock()
            : base()
        {
            //indicator stock object
            InitTotalME2IndicatorStocksProperties();
        }
        //copy constructor
        public ME2IndicatorStock(ME2IndicatorStock calculator)
        {
            CopyTotalME2IndicatorStocksProperties(calculator);
        }
        //indicators can be prices, demogs, nature, or economic (performance)
        public enum ME_TYPES
        {
            none = 0,
            baseline = 1,
            realtime = 2,
            midterm = 3,
            final = 4,
            expost = 5,
            other = 6
        }
        //calculator properties
        //list of indicator1stocks (costs) using List pattern
        public List<ME2IndicatorStock> ME2IndicatorStocks = new List<ME2IndicatorStock>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfME2IndicatorStocks = 20;

        //very likely needs all 15 or put the atts needed directly in Stats, Change, Progress
        //distinguish the M&E indicators being aggregated using these props from baseEl Name, Label, Date
        //for now, Target Type and Altern Type use the base Calc1 props
        public string TotalME2Name { get; set; }
        public string TotalME2Label { get; set; }
        public string TotalME2Description { get; set; }
        public DateTime TotalME2Date { get; set; }
        public string TotalME2Type { get; set; }
        
        //must be a double to generate doubles in formulas (else returns 0)
        public double TotalME2N { get; set; }
        public double TotalME2Total { get; set; }
        public string TotalME2Unit { get; set; }
        public double TotalME2Q1Total { get; set; }
        public string TotalME2Q1Unit { get; set; }
        public double TotalME2Q2Total { get; set; }
        public string TotalME2Q2Unit { get; set; }

        private const string cTotalME2Description = "TME2Description";
        private const string cTotalME2Name = "TME2Name";
        private const string cTotalME2Label = "TME2Label";
        private const string cTotalME2Date = "TME2Date";
        public const string cTotalME2Type = "TME2Type";
        public const string cTotalME2N = "TME2N";
        private const string cTotalME2Total = "TME2Total";
        private const string cTotalME2Unit = "TME2Unit";
        private const string cTotalME2Q1Total = "TME2Q1Total";
        private const string cTotalME2Q1Unit = "TME2Q1Unit";
        private const string cTotalME2Q2Total = "TME2Q2Total";
        private const string cTotalME2Q2Unit = "TME2Q2Unit";

        
        ////Step 1. Extend Indicator1Stock with demographic properties and constants
        //public Demog4 Demog4 { get; set; }
        

        public virtual void InitTotalME2IndicatorStocksProperties()
        {
            if (this.ME2IndicatorStocks == null)
            {
                this.ME2IndicatorStocks = new List<ME2IndicatorStock>();
            }
            foreach (ME2IndicatorStock ind in this.ME2IndicatorStocks)
            {
                InitTotalME2IndicatorStockProperties(ind);
            }
        }
        public void InitTotalME2IndicatorStockProperties(ME2IndicatorStock ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.TotalME2Description = string.Empty;
            ind.TotalME2Name = string.Empty;
            ind.TotalME2Label = string.Empty;
            ind.TotalME2Date = CalculatorHelpers.GetDateShortNow();
            ind.TotalME2Type = ME_TYPES.none.ToString();
            ind.TotalME2N = 0;
            ind.TotalME2Total = 0;
            ind.TotalME2Q1Total = 0;
            ind.TotalME2Q1Unit = string.Empty;
            ind.TotalME2Q2Unit = string.Empty;
            ind.TotalME2Unit = string.Empty;
            ind.TotalME2Q2Total = 0;
            
        }
        public virtual void CopyTotalME2IndicatorStocksProperties(
           ME2IndicatorStock calculator)
        {
            if (calculator.ME2IndicatorStocks != null)
            {
                if (this.ME2IndicatorStocks == null)
                {
                    this.ME2IndicatorStocks = new List<ME2IndicatorStock>();
                }
                foreach (ME2IndicatorStock calculatorInd in calculator.ME2IndicatorStocks)
                {
                    ME2IndicatorStock indstock = new ME2IndicatorStock();
                    CopyTotalME2IndicatorStockProperties(indstock, calculatorInd);
                    this.ME2IndicatorStocks.Add(indstock);
                }
            }
        }
        public void CopyTotalME2IndicatorStockProperties(ME2IndicatorStock ind,
            ME2IndicatorStock calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalME2Description = calculator.TotalME2Description;
            ind.TotalME2Name = calculator.TotalME2Name;
            ind.TotalME2Label = calculator.TotalME2Label;
            ind.TotalME2Date = calculator.TotalME2Date;
            ind.TotalME2Type = calculator.TotalME2Type;
            ind.TotalME2N = calculator.TotalME2N;
            ind.TotalME2Total = calculator.TotalME2Total;
            ind.TotalME2Q1Total = calculator.TotalME2Q1Total;
            ind.TotalME2Q1Unit = calculator.TotalME2Q1Unit;
            ind.TotalME2Q2Unit = calculator.TotalME2Q2Unit;
            ind.TotalME2Unit = calculator.TotalME2Unit;
            ind.TotalME2Q2Total = calculator.TotalME2Q2Total;
            
            //copy the calculator.ME2Indicators
            ind.CopyME2IndicatorsProperties(calculator);
        }
        public virtual void SetTotalME2IndicatorStocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            if (this.ME2IndicatorStocks == null)
            {
                this.ME2IndicatorStocks = new List<ME2IndicatorStock>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfME2IndicatorStocks; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cTotalME2Name, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    ME2IndicatorStock ind1 = new ME2IndicatorStock();
                    SetTotalME2IndicatorStockProperties(ind1, sAttNameExtension, calculator);
                    this.ME2IndicatorStocks.Add(ind1);
                }
                sHasAttribute = string.Empty;
            }
        }
        public void SetTotalME2IndicatorStockProperties(ME2IndicatorStock ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalME2Description = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalME2Description, attNameExtension));
            ind.TotalME2Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalME2Name, attNameExtension));
            ind.TotalME2Label = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalME2Label, attNameExtension));
            ind.TotalME2Date = CalculatorHelpers.GetAttributeDate(calculator,
              string.Concat(cTotalME2Date, attNameExtension));
            ind.TotalME2Type = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(cTotalME2Type, attNameExtension));
            ind.TotalME2N = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2N, attNameExtension));
            ind.TotalME2Total = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Total, attNameExtension));
            ind.TotalME2Q1Total = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q1Total, attNameExtension));
            ind.TotalME2Q1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalME2Q1Unit, attNameExtension));
            ind.TotalME2Q2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalME2Q2Unit, attNameExtension));
            ind.TotalME2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalME2Unit, attNameExtension));
            ind.TotalME2Q2Total = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2Q2Total, attNameExtension));
        }
        public virtual void SetTotalME2IndicatorStocksProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.ME2IndicatorStocks == null)
            {
                this.ME2IndicatorStocks = new List<ME2IndicatorStock>();
            }
            if (this.ME2IndicatorStocks.Count < (colIndex + 1))
            {
                ME2IndicatorStock ind1 = new ME2IndicatorStock();
                this.ME2IndicatorStocks.Insert(colIndex, ind1);
            }
            ME2IndicatorStock ind = this.ME2IndicatorStocks.ElementAt(colIndex);
            if (ind != null)
            {
                SetTotalME2IndicatorStockProperty(ind, attName, attValue);
            }
        }
        public void SetTotalME2IndicatorStockProperty(ME2IndicatorStock ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotalME2Description:
                    ind.TotalME2Description = attValue;
                    break;
                case cTotalME2Name:
                    ind.TotalME2Name = attValue;
                    break;
                case cTotalME2Label:
                    ind.TotalME2Label = attValue;
                    break;
                case cTotalME2Date:
                    ind.TotalME2Date = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTotalME2Type:
                    ind.TotalME2Type = attValue;
                    break;
                case cTotalME2N:
                    ind.TotalME2N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Total:
                    ind.TotalME2Total = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q1Total:
                    ind.TotalME2Q1Total = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2Q1Unit:
                    ind.TotalME2Q1Unit = attValue;
                    break;
                case cTotalME2Q2Unit:
                    ind.TotalME2Q2Unit = attValue;
                    break;
                case cTotalME2Unit:
                    ind.TotalME2Unit = attValue;
                    break;
                case cTotalME2Q2Total:
                    ind.TotalME2Q2Total = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalME2IndicatorStocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.ME2IndicatorStocks.Count >= (colIndex + 1))
            {
                ME2IndicatorStock ind = this.ME2IndicatorStocks.ElementAt(colIndex);
                if (ind != null)
                {
                    sPropertyValue = GetTotalME2IndicatorStockProperty(ind, attName);
                }
            }
            return sPropertyValue;
        }
        public string GetTotalME2IndicatorStockProperty(ME2IndicatorStock ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalME2Description:
                    sPropertyValue = ind.TotalME2Description;
                    break;
                case cTotalME2Name:
                    sPropertyValue = ind.TotalME2Name.ToString();
                    break;
                case cTotalME2Label:
                    sPropertyValue = ind.TotalME2Label.ToString();
                    break;
                case cTotalME2Date:
                    sPropertyValue = ind.TotalME2Date.ToString();
                    break;
                case cTotalME2Type:
                    sPropertyValue = ind.TotalME2Type;
                    break;
                case cTotalME2N:
                    sPropertyValue = ind.TotalME2N.ToString();
                    break;
                case cTotalME2Total:
                    sPropertyValue = ind.TotalME2Total.ToString();
                    break;
                case cTotalME2Q1Total:
                    sPropertyValue = ind.TotalME2Q1Total.ToString();
                    break;
                case cTotalME2Q1Unit:
                    sPropertyValue = ind.TotalME2Q1Unit.ToString();
                    break;
                case cTotalME2Q2Unit:
                    sPropertyValue = ind.TotalME2Q2Unit.ToString();
                    break;
                case cTotalME2Unit:
                    sPropertyValue = ind.TotalME2Unit;
                    break;
                case cTotalME2Q2Total:
                    sPropertyValue = ind.TotalME2Q2Total.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalME2IndicatorStocksAttributes(string attNameExt, ref XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.ME2IndicatorStocks != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (ME2IndicatorStock ind in this.ME2IndicatorStocks)
                {
                    //Name2_3
                    sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                    SetTotalME2IndicatorStockAttributes(ind, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        public virtual void SetTotalME2IndicatorStockAttributes(ME2IndicatorStock ind,
            string attNameExtension, ref XElement calculator)
        {
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cTotalME2Description, attNameExtension), ind.TotalME2Description);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalME2Name, attNameExtension), ind.TotalME2Name);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalME2Label, attNameExtension), ind.TotalME2Label);
            CalculatorHelpers.SetAttributeDateS(calculator,
                    string.Concat(cTotalME2Date, attNameExtension), ind.TotalME2Date);
            CalculatorHelpers.SetAttribute(calculator,
                  string.Concat(cTotalME2Type, attNameExtension), ind.TotalME2Type);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                   string.Concat(cTotalME2N, attNameExtension), ind.TotalME2N);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalME2Total, attNameExtension), ind.TotalME2Total);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalME2Q1Total, attNameExtension), ind.TotalME2Q1Total);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalME2Q1Unit, attNameExtension), ind.TotalME2Q1Unit);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalME2Q2Unit, attNameExtension), ind.TotalME2Q2Unit);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalME2Unit, attNameExtension), ind.TotalME2Unit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalME2Q2Total, attNameExtension), ind.TotalME2Q2Total);
        }
        public virtual void SetTotalME2IndicatorStocksAttributes(string attNameExt, 
            ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.ME2IndicatorStocks != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (ME2IndicatorStock ind in this.ME2IndicatorStocks)
                {
                    //Name2_3
                    sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                    SetTotalME2IndicatorStockAttributes(ind, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public void SetTotalME2IndicatorStockAttributes(ME2IndicatorStock ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                   string.Concat(cTotalME2Description, attNameExtension), ind.TotalME2Description);
            writer.WriteAttributeString(
                    string.Concat(cTotalME2Name, attNameExtension), ind.TotalME2Name.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalME2Label, attNameExtension), ind.TotalME2Label.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalME2Date, attNameExtension), ind.TotalME2Date.ToString("d", DateTimeFormatInfo.InvariantInfo));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Type, attNameExtension), ind.TotalME2Type.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalME2N, attNameExtension), ind.TotalME2N.ToString("N0", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalME2Total, attNameExtension), ind.TotalME2Total.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q1Total, attNameExtension), ind.TotalME2Q1Total.ToString("N3", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q1Unit, attNameExtension), ind.TotalME2Q1Unit);
            writer.WriteAttributeString(
                string.Concat(cTotalME2Q2Unit, attNameExtension), ind.TotalME2Q2Unit);
            writer.WriteAttributeString(
                    string.Concat(cTotalME2Unit, attNameExtension), ind.TotalME2Unit);
            writer.WriteAttributeString(
                    string.Concat(cTotalME2Q2Total, attNameExtension), ind.TotalME2Q2Total.ToString("N3", CultureInfo.InvariantCulture));
        }
        
    }
    public static class ME2IndicatorStockExtensions
    {
        public static void AddME2IndicatorToStocks(this ME2IndicatorStock baseStat, ME2Indicator indicator)
        {
            //make sure that each indicator has a corresponding stock
            if (baseStat.ME2IndicatorStocks == null)
            {
                baseStat.ME2IndicatorStocks = new List<ME2IndicatorStock>();
            }
            if (!baseStat.ME2IndicatorStocks
                .Any(s => s.TotalME2Label == indicator.IndLabel))
            {
                if (indicator.IndLabel != string.Empty)
                {
                    ME2IndicatorStock stock = new ME2IndicatorStock();
                    stock.TotalME2Label = indicator.IndLabel;
                    stock.TotalME2Date = indicator.IndDate;
                    stock.TotalME2Type = indicator.IndType;
                    stock.TotalME2Name = indicator.IndName;
                    stock.TotalME2Unit = indicator.IndUnit;
                    stock.TotalME2Q1Unit = indicator.Ind1Unit;
                    stock.TotalME2Q2Unit = indicator.Ind2Unit;
                    stock.TotalME2Description = indicator.IndDescription;
                    //add the stock to the basestat
                    baseStat.ME2IndicatorStocks.Add(stock);
                }
            }
            else
            {
                //update the identifiers in case they have changed
                ME2IndicatorStock stock = baseStat.ME2IndicatorStocks
                    .FirstOrDefault(s => s.TotalME2Label == indicator.IndLabel);
                if (stock != null)
                {
                    stock.TotalME2Label = indicator.IndLabel;
                    stock.TotalME2Date = indicator.IndDate;
                    stock.TotalME2Type = indicator.IndType;
                    stock.TotalME2Name = indicator.IndName;
                    stock.TotalME2Unit = indicator.IndUnit;
                    stock.TotalME2Q1Unit = indicator.Ind1Unit;
                    stock.TotalME2Q2Unit = indicator.Ind2Unit;
                    stock.TotalME2Description = indicator.IndDescription;
                }
            }
        }
    }
}
