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
    ///Purpose:		The SubPrice2Stock class indirectly extends the SubPrice1() class.
    ///             The SubPrice2Stock.SubPrice2Stocks collection is used to manipulate the class.
    ///             This class is needed for budget elements that need to store both 
    ///             cost (SubPrice1Stock) and benefits (SubPrice2Stock) properties and attributes
    ///             in the same xml nodes.
    ///Author:		www.devtreks.org
    ///Date:		2013, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       1. Budgets need to keep separate track of costs and benefits. this class
    ///             is usually used for benefits.
    ///             
    ///</summary>
    public class SubPrice2Stock : SubPrice1
    {
        //calls the base-class version, and initializes the base class properties.
        public SubPrice2Stock()
            : base()
        {
            //subprice object
            InitTotalSubPrice2StocksProperties();
        }
        //copy constructor
        public SubPrice2Stock(SubPrice2Stock calculator)
        {
            CopyTotalSubPrice2StocksProperties(calculator);
        }

        //calculator properties

        //list of indicator1stocks using List pattern
        public List<SubPrice2Stock> SubPrice2Stocks = new List<SubPrice2Stock>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfSubPrice2Stocks = 20;

        public string TotalSubP2Name { get; set; }
        public string TotalSubP2Label { get; set; }
        public string TotalSubP2Description { get; set; }
        public double TotalSubP2Total { get; set; }
        public string TotalSubP2Unit { get; set; }
        public double TotalSubP2TotalPerUnit { get; set; }
        public double TotalSubP2Price { get; set; }
        public double TotalSubP2Amount { get; set; }

        private const string cTotalSubP2Description = "TSubP2Description";
        private const string cTotalSubP2Name = "TSubP2Name";
        private const string cTotalSubP2Label = "TSubP2Label";
        private const string cTotalSubP2Total = "TSubP2Total";
        private const string cTotalSubP2TotalPerUnit = "TSubP2TotalPerUnit";
        private const string cTotalSubP2Price = "TSubP2Price";
        private const string cTotalSubP2Unit = "TSubP2Unit";
        private const string cTotalSubP2Amount = "TSubP2Amount";
        public virtual void InitTotalSubPrice2StocksProperties()
        {
            if (this.SubPrice2Stocks == null)
            {
                this.SubPrice2Stocks = new List<SubPrice2Stock>();
            }
            foreach (SubPrice2Stock ind in this.SubPrice2Stocks)
            {
                InitTotalSubPrice2StockProperties(ind);
            }
        }
        private void InitTotalSubPrice2StockProperties(SubPrice2Stock ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.TotalSubP2Description = string.Empty;
            ind.TotalSubP2Name = string.Empty;
            ind.TotalSubP2Label = string.Empty;
            ind.TotalSubP2Total = 0;
            ind.TotalSubP2TotalPerUnit = 0;
            ind.TotalSubP2Price = 0;
            ind.TotalSubP2Unit = string.Empty;
            ind.TotalSubP2Amount = 0;
        }
        public virtual void CopyTotalSubPrice2StocksProperties(
           SubPrice2Stock calculator)
        {
            if (calculator.SubPrice2Stocks != null)
            {
                if (this.SubPrice2Stocks == null)
                {
                    this.SubPrice2Stocks = new List<SubPrice2Stock>();
                }
                foreach (SubPrice2Stock calculatorInd in calculator.SubPrice2Stocks)
                {
                    SubPrice2Stock indstock = new SubPrice2Stock();
                    CopyTotalSubPrice2StockProperties(indstock, calculatorInd);
                    this.SubPrice2Stocks.Add(indstock);
                }
            }
        }
        private void CopyTotalSubPrice2StockProperties(SubPrice2Stock ind,
            SubPrice2Stock calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalSubP2Description = calculator.TotalSubP2Description;
            ind.TotalSubP2Name = calculator.TotalSubP2Name;
            ind.TotalSubP2Label = calculator.TotalSubP2Label;
            ind.TotalSubP2Total = calculator.TotalSubP2Total;
            ind.TotalSubP2TotalPerUnit = calculator.TotalSubP2TotalPerUnit;
            ind.TotalSubP2Price = calculator.TotalSubP2Price;
            ind.TotalSubP2Unit = calculator.TotalSubP2Unit;
            ind.TotalSubP2Amount = calculator.TotalSubP2Amount;
        }
        public virtual void SetTotalSubPrice2StocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            if (this.SubPrice2Stocks == null)
            {
                this.SubPrice2Stocks = new List<SubPrice2Stock>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfSubPrice2Stocks; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(SubPrice1.cSubPName, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    SubPrice2Stock ind1 = new SubPrice2Stock();
                    SetTotalSubPrice2StockProperties(ind1, sAttNameExtension, calculator);
                    this.SubPrice2Stocks.Add(ind1);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetTotalSubPrice2StockProperties(SubPrice2Stock ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalSubP2Description = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP2Description, attNameExtension));
            ind.TotalSubP2Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP2Name, attNameExtension));
            ind.TotalSubP2Label = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP2Label, attNameExtension));
            ind.TotalSubP2Total = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP2Total, attNameExtension));
            ind.TotalSubP2TotalPerUnit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP2TotalPerUnit, attNameExtension));
            ind.TotalSubP2Price = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP2Price, attNameExtension));
            ind.TotalSubP2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalSubP2Unit, attNameExtension));
            ind.TotalSubP2Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSubP2Amount, attNameExtension));
        }
        public virtual void SetTotalSubPrice2StocksProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.SubPrice2Stocks == null)
            {
                this.SubPrice2Stocks = new List<SubPrice2Stock>();
            }
            if (this.SubPrice2Stocks.Count < (colIndex + 1))
            {
                SubPrice2Stock ind1 = new SubPrice2Stock();
                this.SubPrice2Stocks.Insert(colIndex, ind1);
            }
            SubPrice2Stock ind = this.SubPrice2Stocks.ElementAt(colIndex);
            if (ind != null)
            {
                SetTotalSubPrice2StockProperty(ind, attName, attValue);
            }
        }
        private void SetTotalSubPrice2StockProperty(SubPrice2Stock ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotalSubP2Description:
                    ind.TotalSubP2Description = attValue;
                    break;
                case cTotalSubP2Name:
                    ind.TotalSubP2Name = attValue;
                    break;
                case cTotalSubP2Label:
                    ind.TotalSubP2Label = attValue;
                    break;
                case cTotalSubP2Total:
                    ind.TotalSubP2Total = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2TotalPerUnit:
                    ind.TotalSubP2TotalPerUnit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Price:
                    ind.TotalSubP2Price = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSubP2Unit:
                    ind.TotalSubP2Unit = attValue;
                    break;
                case cTotalSubP2Amount:
                    ind.TotalSubP2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalSubPrice2StocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.SubPrice2Stocks.Count >= (colIndex + 1))
            {
                SubPrice2Stock ind = this.SubPrice2Stocks.ElementAt(colIndex);
                if (ind != null)
                {
                    sPropertyValue = GetTotalSubPrice2StockProperty(ind, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetTotalSubPrice2StockProperty(SubPrice2Stock ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSubP2Description:
                    sPropertyValue = ind.TotalSubP2Description;
                    break;
                case cTotalSubP2Name:
                    sPropertyValue = ind.TotalSubP2Name.ToString();
                    break;
                case cTotalSubP2Label:
                    sPropertyValue = ind.TotalSubP2Label.ToString();
                    break;
                case cTotalSubP2Total:
                    sPropertyValue = ind.TotalSubP2Total.ToString();
                    break;
                case cTotalSubP2TotalPerUnit:
                    sPropertyValue = ind.TotalSubP2TotalPerUnit.ToString();
                    break;
                case cTotalSubP2Price:
                    sPropertyValue = ind.TotalSubP2Price.ToString();
                    break;
                case cTotalSubP2Unit:
                    sPropertyValue = ind.TotalSubP2Unit;
                    break;
                case cTotalSubP2Amount:
                    sPropertyValue = ind.TotalSubP2Amount.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalSubPrice2StocksAttributes(string attNameExt, ref XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.SubPrice2Stocks != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice2Stock ind in this.SubPrice2Stocks)
                {
                    sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                    SetTotalSubPrice2StockAttributes(ind, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        public virtual void SetTotalSubPrice2StockAttributes(SubPrice2Stock ind,
            string attNameExtension, ref XElement calculator)
        {
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cTotalSubP2Description, attNameExtension), ind.TotalSubP2Description);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalSubP2Name, attNameExtension), ind.TotalSubP2Name);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalSubP2Label, attNameExtension), ind.TotalSubP2Label);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP2Total, attNameExtension), ind.TotalSubP2Total);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP2TotalPerUnit, attNameExtension), ind.TotalSubP2TotalPerUnit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP2Price, attNameExtension), ind.TotalSubP2Price);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalSubP2Unit, attNameExtension), ind.TotalSubP2Unit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalSubP2Amount, attNameExtension), ind.TotalSubP2Amount);
        }
        public virtual void SetTotalSubPrice2StocksAttributes(string attNameExt, ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.SubPrice2Stocks != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice2Stock ind in this.SubPrice2Stocks)
                {
                    sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                    SetTotalSubPrice2StockAttributes(ind, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        private void SetTotalSubPrice2StockAttributes(SubPrice2Stock ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                   string.Concat(cTotalSubP2Description, attNameExtension), ind.TotalSubP2Description);
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Name, attNameExtension), ind.TotalSubP2Name.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Label, attNameExtension), ind.TotalSubP2Label.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Total, attNameExtension), ind.TotalSubP2Total.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalSubP2TotalPerUnit, attNameExtension), ind.TotalSubP2TotalPerUnit.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalSubP2Price, attNameExtension), ind.TotalSubP2Price.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Unit, attNameExtension), ind.TotalSubP2Unit);
            writer.WriteAttributeString(
                    string.Concat(cTotalSubP2Amount, attNameExtension), ind.TotalSubP2Amount.ToString("N3", CultureInfo.InvariantCulture));
        }
    }
    public static class SubPrice2StockExtensions
    {
        public static void AddSubPrice2ToStocks(this SubPrice2Stock baseStat, SubPrice1 subprice)
        {
            //make sure that each subprice has a corresponding stock
            if (baseStat.SubPrice2Stocks == null)
            {
                SubPrice2Stock ind1 = new SubPrice2Stock();
                baseStat.SubPrice2Stocks.Add(ind1);
            }
            if (!baseStat.SubPrice2Stocks
                .Any(s => s.TotalSubP2Label == subprice.SubPLabel))
            {
                if (subprice.SubPLabel != string.Empty)
                {
                    SubPrice2Stock stock = new SubPrice2Stock();
                    stock.TotalSubP2Label = subprice.SubPLabel;
                    stock.TotalSubP2Name = subprice.SubPName;
                    stock.TotalSubP2Unit = subprice.SubPUnit;
                    stock.TotalSubP2Description = subprice.SubPDescription;
                    baseStat.SubPrice2Stocks.Add(stock);
                }
            }
            else
            {
                //update the identifiers in case they have changed
                SubPrice2Stock stock = baseStat.SubPrice2Stocks
                    .FirstOrDefault(s => s.TotalSubP2Label == subprice.SubPLabel);
                if (stock != null)
                {
                    stock.TotalSubP2Label = subprice.SubPLabel;
                    stock.TotalSubP2Name = subprice.SubPName;
                    stock.TotalSubP2Unit = subprice.SubPUnit;
                    stock.TotalSubP2Description = subprice.SubPDescription;
                }
            }
        }

    }
}

