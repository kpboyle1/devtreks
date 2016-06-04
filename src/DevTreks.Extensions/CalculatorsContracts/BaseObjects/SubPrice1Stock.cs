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
    ///Purpose:		The SubPrice1Stock class extends the SubPrice1() class.
    ///             The two stock lists hold the actual content (not the initial
    ///             enveloping SubPrice1Stock).
    ///             The LCA.LCA1Total object demonstrates typical use.
    ///Author:		www.devtreks.org
    ///Date:		2013, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       1. 
    ///             
    ///</summary>
    public class SubPrice1Stock : SubPrice1 
    {
        //calls the base-class version, and initializes the base class properties.
        public SubPrice1Stock()
            : base()
        {
            //subprice object
            InitTotalSubPrice1StocksProperties();
        }
        //copy constructor
        public SubPrice1Stock(SubPrice1Stock calculator)
        {
            CopyTotalSubPrice1StocksProperties(calculator);
        }

        //calculator properties
        //These two lists hold the actual content, not the initial parent SubPrice1Stock (no TotalSubP1Name, ...)
        //the LCA.LCA1Total object demonstrates how it is used
        //list of indicator1stocks (costs) using List pattern
        public List<SubPrice1Stock> SubPrice1Stocks = new List<SubPrice1Stock>();
        ////list of indicator1stocks (benefits) using List pattern
        //public List<SubPrice2Stock> SubPrice2Stocks = new List<SubPrice2Stock>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfSubPrice1Stocks = 20;

        public string TotalSubP1Name { get; set; }
        public string TotalSubP1Label { get; set; }
        public string TotalSubP1Description { get; set; }
        public double TotalSubP1Total { get; set; }
        public string TotalSubP1Unit { get; set; }
        public double TotalSubP1TotalPerUnit { get; set; }
        public double TotalSubP1Price { get; set; }
        public double TotalSubP1Amount { get; set; }

        private const string cTotalSubP1Description = "TSubP1Description";
        private const string cTotalSubP1Name = "TSubP1Name";
        private const string cTotalSubP1Label = "TSubP1Label";
        private const string cTotalSubP1Total = "TSubP1Total";
        private const string cTotalSubP1TotalPerUnit = "TSubP1TotalPerUnit";
        private const string cTotalSubP1Price = "TSubP1Price";
        private const string cTotalSubP1Unit = "TSubP1Unit";
        private const string cTotalSubP1Amount = "TSubP1Amount";
        public virtual void InitTotalSubPrice1StocksProperties()
        {
            if (this.SubPrice1Stocks == null)
            {
                this.SubPrice1Stocks = new List<SubPrice1Stock>();
            }
            foreach (SubPrice1Stock ind in this.SubPrice1Stocks)
            {
                InitTotalSubPrice1StockProperties(ind);
            }
        }
        private void InitTotalSubPrice1StockProperties(SubPrice1Stock ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.TotalSubP1Description = string.Empty;
            ind.TotalSubP1Name = string.Empty;
            ind.TotalSubP1Label = string.Empty;
            ind.TotalSubP1Total = 0;
            ind.TotalSubP1TotalPerUnit = 0;
            ind.TotalSubP1Price = 0;
            ind.TotalSubP1Unit = string.Empty;
            ind.TotalSubP1Amount = 0;
        }
        public virtual void CopyTotalSubPrice1StocksProperties(
           SubPrice1Stock calculator)
        {
            if (calculator.SubPrice1Stocks != null)
            {
                if (this.SubPrice1Stocks == null)
                {
                    this.SubPrice1Stocks = new List<SubPrice1Stock>();
                }
                foreach (SubPrice1Stock calculatorInd in calculator.SubPrice1Stocks)
                {
                    SubPrice1Stock indstock = new SubPrice1Stock();
                    CopyTotalSubPrice1StockProperties(indstock, calculatorInd);
                    this.SubPrice1Stocks.Add(indstock);
                }
            }
        }
        private void CopyTotalSubPrice1StockProperties(SubPrice1Stock ind,
            SubPrice1Stock calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalSubP1Description = calculator.TotalSubP1Description;
            ind.TotalSubP1Name = calculator.TotalSubP1Name;
            ind.TotalSubP1Label = calculator.TotalSubP1Label;
            ind.TotalSubP1Total = calculator.TotalSubP1Total;
            ind.TotalSubP1TotalPerUnit = calculator.TotalSubP1TotalPerUnit;
            ind.TotalSubP1Price = calculator.TotalSubP1Price;
            ind.TotalSubP1Unit = calculator.TotalSubP1Unit;
            ind.TotalSubP1Amount = calculator.TotalSubP1Amount;
        }
        public virtual void SetTotalSubPrice1StocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            if (this.SubPrice1Stocks == null)
            {
                this.SubPrice1Stocks = new List<SubPrice1Stock>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfSubPrice1Stocks; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cTotalSubP1Name, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    SubPrice1Stock ind1 = new SubPrice1Stock();
                    SetTotalSubPrice1StockProperties(ind1, sAttNameExtension, calculator);
                    this.SubPrice1Stocks.Add(ind1);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetTotalSubPrice1StockProperties(SubPrice1Stock ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalSubP1Description = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP1Description, attNameExtension));
            ind.TotalSubP1Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP1Name, attNameExtension));
            ind.TotalSubP1Label = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP1Label, attNameExtension));
            ind.TotalSubP1Total = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP1Total, attNameExtension));
            ind.TotalSubP1TotalPerUnit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP1TotalPerUnit, attNameExtension));
            ind.TotalSubP1Price = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP1Price, attNameExtension));
            ind.TotalSubP1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP1Unit, attNameExtension));
            ind.TotalSubP1Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP1Amount, attNameExtension));
        }
        public virtual void SetTotalSubPrice1StocksProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.SubPrice1Stocks == null)
            {
                this.SubPrice1Stocks = new List<SubPrice1Stock>();
            }
            if (this.SubPrice1Stocks.Count < (colIndex + 1))
            {
                SubPrice1Stock ind1 = new SubPrice1Stock();
                this.SubPrice1Stocks.Insert(colIndex, ind1);
            }
            SubPrice1Stock ind = this.SubPrice1Stocks.ElementAt(colIndex);
            if (ind != null)
            {
                SetTotalSubPrice1StockProperty(ind, attName, attValue);
            }
        }
        private void SetTotalSubPrice1StockProperty(SubPrice1Stock ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotalSubP1Description:
                    ind.TotalSubP1Description = attValue;
                    break;
                case cTotalSubP1Name:
                    ind.TotalSubP1Name = attValue;
                    break;
                case cTotalSubP1Label:
                    ind.TotalSubP1Label = attValue;
                    break;
                case cTotalSubP1Total:
                    ind.TotalSubP1Total = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1TotalPerUnit:
                    ind.TotalSubP1TotalPerUnit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Price:
                    ind.TotalSubP1Price = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP1Unit:
                    ind.TotalSubP1Unit = attValue;
                    break;
                case cTotalSubP1Amount:
                    ind.TotalSubP1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalSubPrice1StocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.SubPrice1Stocks.Count >= (colIndex + 1))
            {
                SubPrice1Stock ind = this.SubPrice1Stocks.ElementAt(colIndex);
                if (ind != null)
                {
                    sPropertyValue = GetTotalSubPrice1StockProperty(ind, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetTotalSubPrice1StockProperty(SubPrice1Stock ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSubP1Description:
                    sPropertyValue = ind.TotalSubP1Description;
                    break;
                case cTotalSubP1Name:
                    sPropertyValue = ind.TotalSubP1Name.ToString();
                    break;
                case cTotalSubP1Label:
                    sPropertyValue = ind.TotalSubP1Label.ToString();
                    break;
                case cTotalSubP1Total:
                    sPropertyValue = ind.TotalSubP1Total.ToString();
                    break;
                case cTotalSubP1TotalPerUnit:
                    sPropertyValue = ind.TotalSubP1TotalPerUnit.ToString();
                    break;
                case cTotalSubP1Price:
                    sPropertyValue = ind.TotalSubP1Price.ToString();
                    break;
                case cTotalSubP1Unit:
                    sPropertyValue = ind.TotalSubP1Unit;
                    break;
                case cTotalSubP1Amount:
                    sPropertyValue = ind.TotalSubP1Amount.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalSubPrice1StocksAttributes(string attNameExt, ref XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.SubPrice1Stocks != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice1Stock ind in this.SubPrice1Stocks)
                {
                    //Name1_3
                    sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                    SetTotalSubPrice1StockAttributes(ind, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        public virtual void SetTotalSubPrice1StockAttributes(SubPrice1Stock ind,
            string attNameExtension, ref XElement calculator)
        {
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cTotalSubP1Description, attNameExtension), ind.TotalSubP1Description);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalSubP1Name, attNameExtension), ind.TotalSubP1Name);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalSubP1Label, attNameExtension), ind.TotalSubP1Label);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP1Total, attNameExtension), ind.TotalSubP1Total);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP1TotalPerUnit, attNameExtension), ind.TotalSubP1TotalPerUnit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP1Price, attNameExtension), ind.TotalSubP1Price);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalSubP1Unit, attNameExtension), ind.TotalSubP1Unit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP1Amount, attNameExtension), ind.TotalSubP1Amount);
        }
        public virtual void SetTotalSubPrice1StocksAttributes(string attNameExt, 
            ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.SubPrice1Stocks != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice1Stock ind in this.SubPrice1Stocks)
                {
                    //Name1_3
                    sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                    SetTotalSubPrice1StockAttributes(ind, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public void SetTotalSubPrice1StockAttributes(SubPrice1Stock ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                   string.Concat(cTotalSubP1Description, attNameExtension), ind.TotalSubP1Description);
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Name, attNameExtension), ind.TotalSubP1Name.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Label, attNameExtension), ind.TotalSubP1Label.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Total, attNameExtension), ind.TotalSubP1Total.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalSubP1TotalPerUnit, attNameExtension), ind.TotalSubP1TotalPerUnit.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalSubP1Price, attNameExtension), ind.TotalSubP1Price.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Unit, attNameExtension), ind.TotalSubP1Unit);
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP1Amount, attNameExtension), ind.TotalSubP1Amount.ToString("N3", CultureInfo.InvariantCulture));
        }
        
    }
    public static class SubPrice1StockExtensions
    {
        public static void AddSubPrice1ToStocks(this SubPrice1Stock baseStat, SubPrice1 subprice)
        {
            //make sure that each subprice has a corresponding stock
            if (baseStat.SubPrice1Stocks == null)
            {
                baseStat.SubPrice1Stocks = new List<SubPrice1Stock>();
            }
            if (!baseStat.SubPrice1Stocks
                .Any(s => s.TotalSubP1Label == subprice.SubPLabel))
            {
                if (subprice.SubPLabel != string.Empty)
                {
                    SubPrice1Stock stock = new SubPrice1Stock();
                    stock.TotalSubP1Label = subprice.SubPLabel;
                    stock.TotalSubP1Name = subprice.SubPName;
                    stock.TotalSubP1Unit = subprice.SubPUnit;
                    stock.TotalSubP1Description = subprice.SubPDescription;
                    //add the stock to the basestat
                    baseStat.SubPrice1Stocks.Add(stock);
                }
            }
            else
            {
                //update the identifiers in case they have changed
                SubPrice1Stock stock = baseStat.SubPrice1Stocks
                    .FirstOrDefault(s => s.TotalSubP1Label == subprice.SubPLabel);
                if (stock != null)
                {
                    stock.TotalSubP1Label = subprice.SubPLabel;
                    stock.TotalSubP1Name = subprice.SubPName;
                    stock.TotalSubP1Unit = subprice.SubPUnit;
                    stock.TotalSubP1Description = subprice.SubPDescription;
                }
            }
        }
        
    }
}
