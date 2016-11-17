using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             ME2Stock.Stocks.ME2Total1.Stocks.ME2Total1.ME2Indicators
    ///             ME2Stock.Stocks is a collection of ME2Stocks (unique observations)
    ///             Each member of ME2Stocks holds an analyzer stock (Total1)
    ///             Each analyzer stock (Total1) holds a collection of Total1s
    ///             The unique observations can be compared for change and progress analyses
    ///             Important to use separate stocks because each stock represents
    ///             an aggregated observation
    ///Author:		www.devtreks.org
    ///Date:		2016, November
    ///References:	Refer to the M and E tutorials
    ///</summary>
    public class ME2Stock : ME2IndicatorStock
    {
        //inheritors
        public ME2Stock(CalculatorParameters calcParams)
            : base(calcParams)
        {
        }
        //calls the base-class version, and initializes the base class properties.
        public ME2Stock(CalculatorParameters calcParams, string analyzerType)
            : base(calcParams)
        {
            //ME2IndicatorStock has to know which analysis to run
            this.AnalyzerType = analyzerType;
            //subprice object
            InitTotalME2StocksProperties();
        }
        //note the properties contained in ME2IndicatorStock are also used for display
        //public CalculatorParameters CalcParameters { get; set; }
        //changes and progress use collections of results
        public List<ME2Stock> Stocks { get; set; }
        //totals analyses 
        public ME2Total1 Total1 { get; set; }
        //statistical analyses
        public ME2Stat1 Stat1 { get; set; }
        //change analyses
        public ME2Change1 Change1 { get; set; }
        //progress analyses
        public ME2Progress1 Progress1 { get; set; }
        //analyzers set one Stock property
        public string TME2Stage { get; set; }
        public const string cTME2Stage = "TME2Stage";
        public enum ME_STAGES
        {
            none        = 0,
            baseline    = 1,
            realtime    = 2,
            midterm     = 3,
            final       = 4,
            expost      = 5,
            other       = 6
        }
        public virtual void InitTotalME2StocksProperties()
        {
            if (this.Total1 == null)
            {
                this.Total1 = new ME2Total1(this.CalcParameters);
            }
            if (this.Stat1 == null)
            {
                this.Stat1 = new ME2Stat1(this.CalcParameters);
            }
            if (this.Change1 == null)
            {
                this.Change1 = new ME2Change1(this.CalcParameters);
            }
            if (this.Progress1 == null)
            {
                this.Progress1 = new ME2Progress1(this.CalcParameters);
            }
            //need to organize stocks by observations
            this.Stocks = new List<ME2Stock>();
            ME2Stock stock = new ME2Stock(this.CalcParameters);
            if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                stock.Stat1 = new ME2Stat1(this.CalcParameters);
                stock.Stat1.InitTotalME2Stat1Properties(stock.Stat1);
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                stock.Change1 = new ME2Change1(this.CalcParameters);
                stock.Change1.InitTotalME2Change1Properties(stock.Change1);
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                stock.Progress1 = new ME2Progress1(this.CalcParameters);
                stock.Progress1.InitTotalME2Progress1Properties(stock.Progress1);
            }
            else
            {
                //default are total indicators
                stock.Total1 = new ME2Total1(this.CalcParameters);
                stock.Total1.InitTotalME2Total1Properties(stock.Total1);
            }
            this.Stocks.Add(stock);
        }
        public virtual void CopyME2CalculatorToME2Stock(ME2Calculator calculator)
        {
            //analyzers hold all calcs and analyses
            if (this.Total1 == null)
            {
                this.Total1 = new ME2Total1(this.CalcParameters);
            }
            if (this.Stat1 == null)
            {
                this.Stat1 = new ME2Stat1(this.CalcParameters);
            }
            if (this.Change1 == null)
            {
                this.Change1 = new ME2Change1(this.CalcParameters);
            }
            if (this.Progress1 == null)
            {
                this.Progress1 = new ME2Progress1(this.CalcParameters);
            }
            //need to organize stocks by observations
            this.Stocks = new List<ME2Stock>();
            //MEStock uses this.AnalyzerType for most calcs
            calculator.AnalyzerType = this.AnalyzerType;
            this.CopyCalculatorProperties(calculator);
            ME2Stock stock = new ME2Stock(this.CalcParameters);
            //this copies the ME2Calc.Indicators to the analyzer
            if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                stock.Stat1 = new ME2Stat1(this.CalcParameters);
                stock.Stat1.CopyME2Properties(calculator);
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                stock.Change1 = new ME2Change1(this.CalcParameters);
                stock.Change1.CopyME2Properties(calculator);
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                stock.Progress1 = new ME2Progress1(this.CalcParameters);
                stock.Progress1.CopyME2Properties(calculator);
            }
            else
            {
                stock.Total1 = new ME2Total1(this.CalcParameters);
                stock.Total1.CopyME2Properties(calculator);
            }
            this.Stocks.Add(stock);
        }
        public virtual void CopyTotalME2StocksProperties(
              ME2Stock calculator)
        {
            if (calculator.Stocks != null)
            {
                //set up the new me2Stock
                this.CopyCalculatorProperties(calculator);
                this.AnalyzerType = calculator.AnalyzerType;
                this.Stocks = new List<ME2Stock>();
                foreach (ME2Stock obsStock in calculator.Stocks)
                {
                    //set up the new stock for the stocks collection
                    ME2Stock stock = new ME2Stock(CalcParameters);
                    //stock gets same props as this (which are copied from calculator
                    stock.CopyCalculatorProperties(this);
                    stock.AnalyzerType = this.AnalyzerType;
                    //copy the analysis to the stock
                    if (this.AnalyzerType
                        == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
                    {
                        if (obsStock.Stat1 != null)
                        {
                            stock.Stat1 = new ME2Stat1(this.CalcParameters);
                            stock.Stat1.CopyCalculatorProperties(this);
                            stock.Stat1.CopyTotalME2Stat1Properties(
                                obsStock.Stat1);
                        }
                    }
                    else if (this.AnalyzerType
                        == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                        || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                        || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
                    {
                        if (obsStock.Change1 != null)
                        {
                            stock.Change1 = new ME2Change1(this.CalcParameters);
                            stock.Change1.CopyCalculatorProperties(this);
                            stock.Change1.CopyTotalME2Change1Properties(
                                obsStock.Change1);
                        }
                    }
                    else if (this.AnalyzerType
                        == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
                    {
                        this.Progress1 = new ME2Progress1(this.CalcParameters);
                        if (obsStock.Progress1 != null)
                        {
                            stock.Progress1 = new ME2Progress1(this.CalcParameters);
                            stock.Progress1.CopyCalculatorProperties(this);
                            stock.Progress1.CopyTotalME2Progress1Properties(
                                obsStock.Progress1);
                        }
                    }
                    else
                    {
                        if (obsStock.Total1 != null)
                        {
                            stock.Total1 = new ME2Total1(this.CalcParameters);
                            stock.Total1.CopyCalculatorProperties(this);
                            stock.Total1.CopyTotalME2Total1Properties(
                                obsStock.Total1);
                        }
                    }
                    this.Stocks.Add(stock);
                }
            }
        }
        public virtual void SetTotalME2StocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            string sAttNameExtension = string.Empty;
            if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                if (this.Stat1 == null)
                {
                    this.Stat1 = new ME2Stat1(this.CalcParameters);
                }
                this.Stat1.SetCalculatorProperties(calculator);
                this.Stat1.SetTotalME2Stat1Properties(
                    this.Stat1, sAttNameExtension, calculator);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                if (this.Change1 == null)
                {
                    this.Change1 = new ME2Change1(this.CalcParameters);
                }
                this.Change1.SetCalculatorProperties(calculator);
                this.Change1.SetTotalME2Change1Properties(
                    this.Change1, sAttNameExtension, calculator);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                if (this.Progress1 == null)
                {
                    this.Progress1 = new ME2Progress1(this.CalcParameters);
                }
                this.Progress1.SetCalculatorProperties(calculator);
                this.Progress1.SetTotalME2Progress1Properties(
                    this.Progress1, sAttNameExtension, calculator);
            }
            else
            {
                if (this.Total1 == null)
                {
                    this.Total1 = new ME2Total1(this.CalcParameters);
                }
                this.Total1.SetCalculatorProperties(calculator);
                this.Total1.SetTotalME2Total1Properties(
                    this.Total1, sAttNameExtension, calculator);
            }
        }

        public virtual void SetDescendantME2StockProperties(XElement currentCalculationsElement)
        {
            if (currentCalculationsElement != null)
            {
                //abbreviated properties for descendant stock totals
                this.SetCalculatorProperties(currentCalculationsElement);
                //no set shared object props, they already have props set
                //don't SetTotalIndicator1StocksProperties because the descendants have their own stock totals

                //204
                //analyzers set exactly one ME2Stock property: TMEStage (baseline, midterm ...)
                string sMEStage = CalculatorHelpers.GetAttribute(
                    currentCalculationsElement, cTME2Stage);
                if (!string.IsNullOrEmpty(sMEStage))
                {
                    this.TME2Stage = sMEStage;
                    //the remaining properties derive from aggregated children indicators
                }
                else
                {
                    this.TME2Stage = ME_STAGES.none.ToString();
                }
            }
        }
        public virtual void SetTotalME2StocksProperty(string attName,
               string attValue, int colIndex)
        {
            if (this.AnalyzerType
                    == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                SetTotalME2IndicatorStockProperty(this.Stat1, attName, attValue);
                this.Stat1.SetTotalME2Stat1Property(
                    this.Stat1, attName, attValue);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                SetTotalME2IndicatorStockProperty(this.Change1, attName, attValue);
                this.Change1.SetTotalME2Change1Property(
                    this.Change1, attName, attValue);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                SetTotalME2IndicatorStockProperty(this.Progress1, attName, attValue);
                this.Progress1.SetTotalME2Progress1Property(
                    this.Progress1, attName, attValue);
            }
            else
            {
                this.Total1.SetTotalME2Total1Property(
                    this.Total1, attName, attValue);
            }
        }
        public virtual string GetTotalME2StocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                GetTotalME2IndicatorStockProperty(this.Stat1, attName);
                sPropertyValue = this.Stat1.GetTotalME2Stat1Property(
                   this.Stat1, attName);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                GetTotalME2IndicatorStockProperty(this.Change1, attName);
                sPropertyValue = this.Change1.GetTotalME2Change1Property(
                   this.Change1, attName);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                GetTotalME2IndicatorStockProperty(this.Progress1, attName);
                sPropertyValue = this.Progress1.GetTotalME2Progress1Property(
                   this.Progress1, attName);
            }
            else
            {
                sPropertyValue = this.Total1.GetTotalME2Total1Property(
                    this.Total1, attName);
            }
            return sPropertyValue;
        }
        public void SetDescendantME2StockAttributes(string attNameExt, ref XmlWriter writer)
        {
            //abbreviated list for decendant stock totals
            if (writer != null)
            {
                //set the base analyzer
                this.SetCalculatorAttributes(attNameExt, ref writer);
                //set the calculations
                this.SetTotalME2StocksAttributes(attNameExt, ref writer);
                //TMEStage att is set in calling BIAnalyzer because it stores that property
            }
        }
        public virtual void SetTotalME2StocksAttributes(string attNameExt, ref XmlWriter writer)
        {
            //*******this must be called from a foreach loop of this.Stocks
            if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    if (this.Stat1.Stocks != null)
                    {
                        this.Stat1.SetTotalME2Stat1Attributes(attNameExt, ref writer);
                    }
                }
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    if (this.Change1.Stocks != null)
                    {
                        this.Change1.CalcParameters.CurrentElementNodeName
                            = this.CalcParameters.CurrentElementNodeName;
                        //set the analysis
                        this.Change1.SetTotalME2Change1Attributes(attNameExt, ref writer);
                    }
                }
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    if (this.Progress1.Stocks != null)
                    {
                        this.Progress1.CalcParameters.CurrentElementNodeName
                            = this.CalcParameters.CurrentElementNodeName;
                        //set the analysis
                        this.Progress1.SetTotalME2Progress1Attributes(attNameExt, ref writer);
                    }
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    if (this.Total1.Stocks != null)
                    {
                        this.Total1.SetTotalME2Total1Attributes(attNameExt, ref writer);
                    }
                }
            }
        }
        public virtual bool HasAnalyses()
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    if (this.Stat1.Stocks != null)
                    {
                        if (this.Stat1.Stocks.Count > 0)
                        {
                            bHasAnalyses = true;
                        }
                    }
                }
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    if (this.Change1.Stocks != null)
                    {
                        if (this.Change1.Stocks.Count > 0)
                        {
                            bHasAnalyses = true;
                        }
                    }
                }
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    if (this.Progress1.Stocks != null)
                    {
                        if (this.Progress1.Stocks.Count > 0)
                        {
                            bHasAnalyses = true;
                        }
                    }
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    if (this.Total1.Stocks != null)
                    {
                        if (this.Total1.Stocks.Count > 0)
                        {
                            bHasAnalyses = true;
                        }
                    }
                }
            }
            return bHasAnalyses;
        }
        public bool RunAnalyses(List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.mestat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeyr.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangeid.ToString()
                || this.AnalyzerType == ME2AnalyzerHelper.ANALYZER_TYPES.mechangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType
                == ME2AnalyzerHelper.ANALYZER_TYPES.meprogress1.ToString())
            {
                bHasAnalyses = this.Progress1.RunAnalyses(this, calcs);
            }
            else
            {
                //calcs will be added to this.Stocks (not this.Total1)
                bHasAnalyses = this.Total1.RunAnalyses(this, calcs);
            }
            return bHasAnalyses;
        }
    }
}