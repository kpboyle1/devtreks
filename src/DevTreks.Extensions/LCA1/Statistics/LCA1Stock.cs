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
    ///             Costs: LCA1Stock.Total1.SubP1Stock.SubStock1s.SubPrice1s
    ///             Benefits: LCA1Stock.Total1.SubP2Stock.SubStock2s.SubPrice1s
    ///             Important to use separate agg objects because
    ///             some analyses are sequential and use the results of previous analyses.
    ///             The class keeps track of lca quantitative totals.
    ///Author:		www.devtreks.org
    ///Date:		2013, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTEs:       1. Analyzer should only be run at budget.TimePeriod or otherElement.Group level. 
    ///</summary>
    public class LCA1Stock : Calculator1
    {
        //calls the base-class version, and initializes the base class properties.
        public LCA1Stock(CalculatorParameters calcParams, string analyzerType)
            : base()
        {
            if (calcParams != null)
            {
                this.CalcParameters = new CalculatorParameters(calcParams);
            }
            //SubPrice1Stock has to know which analysis to run
            this.AnalyzerType = analyzerType;
            //subprice object
            InitTotalLCA1StocksProperties();
        }
        //copy constructor
        public LCA1Stock(LCA1Stock calculator)
        {
            CopyTotalLCA1StocksProperties(calculator);
        }
        //inheritors
        public LCA1Stock()
            : base()
        {
        }
        //note the properties contained in SubPrice1Stock are also used for display
        public CalculatorParameters CalcParameters { get; set; }
        //costs
        public SubPrice1Stock SubP1Stock { get; set; }
        //benefits
        public SubPrice2Stock SubP2Stock { get; set; }
        //changes and progress use collections of results
        public List<LCA1Stock> Stocks { get; set; }
        //totals analyses (see if any of the others need this to be a list)
        public LCA1Total1 Total1 { get; set; }
        //statistical analyses
        public LCA1Stat1 Stat1 { get; set; }
        //change analyses
        public LCA1Change1 Change1 { get; set; }
        //progress analyses
        public LCA1Progress1 Progress1 { get; set; }

        public virtual void InitTotalLCA1StocksProperties()
        {
            if (this.SubP1Stock == null)
            {
                this.SubP1Stock = new SubPrice1Stock();
            }
            if (this.SubP2Stock == null)
            {
                this.SubP2Stock = new SubPrice2Stock();
            }
            if (this.Total1 == null)
            {
                this.Total1 = new LCA1Total1();
            }
            if (this.Stat1 == null)
            {
                this.Stat1 = new LCA1Stat1();
            }
            if (this.Change1 == null)
            {
                this.Change1 = new LCA1Change1();
            }
            if (this.Progress1 == null)
            {
                this.Progress1 = new LCA1Progress1();
            }
            if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                this.Stat1 = new LCA1Stat1();
                this.Stat1.InitTotalLCA1Stat1Properties(this.Stat1);
                //set costs
                this.Stat1.SubP1Stock = new SubPrice1Stock();
                this.Stat1.SubP1Stock.InitTotalSubPrice1StocksProperties();
                //set benefits
                this.Stat1.SubP2Stock = new SubPrice2Stock();
                this.Stat1.SubP2Stock.InitTotalSubPrice2StocksProperties();
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                this.Change1 = new LCA1Change1();
                this.Change1.InitTotalLCA1Change1Properties(this.Change1);
                //set costs
                this.Change1.SubP1Stock = new SubPrice1Stock();
                this.Change1.SubP1Stock.InitTotalSubPrice1StocksProperties();
                //set benefits
                this.Change1.SubP2Stock = new SubPrice2Stock();
                this.Change1.SubP2Stock.InitTotalSubPrice2StocksProperties();
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                this.Progress1 = new LCA1Progress1();
                this.Progress1.InitTotalLCA1Progress1Properties(this.Progress1);
                //set costs
                this.Progress1.SubP1Stock = new SubPrice1Stock();
                this.Progress1.SubP1Stock.InitTotalSubPrice1StocksProperties();
                //set benefits
                this.Progress1.SubP2Stock = new SubPrice2Stock();
                this.Progress1.SubP2Stock.InitTotalSubPrice2StocksProperties();
            }
            else
            {
                //default are total indicators
                this.Total1 = new LCA1Total1();
                this.Total1.InitTotalLCA1Total1Properties(this.Total1);
                //set costs
                this.Total1.SubP1Stock = new SubPrice1Stock();
                this.Total1.SubP1Stock.InitTotalSubPrice1StocksProperties();
                //set benefits
                this.Total1.SubP2Stock = new SubPrice2Stock();
                this.Total1.SubP2Stock.InitTotalSubPrice2StocksProperties();
            }
        }
        public virtual void CopyTotalLCA1StocksProperties(
              LCA1Stock calculator)
        {
            if (this.AnalyzerType 
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                this.Stat1 = new LCA1Stat1();
                this.CopyCalculatorProperties(calculator);
                this.Stat1.CopyCalculatorProperties(calculator.Stat1);
                this.Stat1.CopyTotalLCA1Stat1Properties(
                    this.Stat1, calculator.Stat1);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                this.Change1 = new LCA1Change1();
                this.CopyCalculatorProperties(calculator);
                this.Change1.CopyCalculatorProperties(calculator.Change1);
                this.Change1.CopyTotalLCA1Change1Properties(
                    this.Change1, calculator.Change1);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                this.Progress1 = new LCA1Progress1();
                this.CopyCalculatorProperties(calculator);
                this.Progress1.CopyCalculatorProperties(calculator.Progress1);
                this.Progress1.CopyTotalLCA1Progress1Properties(
                    this.Progress1, calculator.Progress1);
            }
            else
            {
                //this handles subobjects (calparams, subp1stock, subp2stock)
                this.Total1 = new LCA1Total1();
                this.CopyCalculatorProperties(calculator);
                this.Total1.CopyCalculatorProperties(calculator.Total1);
                this.Total1.CopyTotalLCA1Total1Properties(
                    this.Total1, calculator.Total1);
            }
            
        }
        public virtual void SetTotalLCA1StocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            string sAttNameExtension = string.Empty;
            if (this.AnalyzerType
                        == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                if (this.Stat1 == null)
                {
                    this.Stat1 = new LCA1Stat1();
                }
                this.Stat1.SetCalculatorProperties(calculator);
                this.Stat1.SetTotalLCA1Stat1Properties(
                    this.Stat1, sAttNameExtension, calculator);
                //costs
                if (this.Stat1.SubP1Stock == null)
                {
                    this.Stat1.SubP1Stock = new SubPrice1Stock();
                }
                this.Stat1.SubP1Stock.SetTotalSubPrice1StocksProperties(calculator);
                //benefits
                if (this.Stat1.SubP2Stock == null)
                {
                    this.Stat1.SubP2Stock = new SubPrice2Stock();
                }
                this.Stat1.SubP2Stock.SetTotalSubPrice2StocksProperties(calculator);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                if (this.Change1 == null)
                {
                    this.Change1 = new LCA1Change1();
                }
                this.Change1.SetCalculatorProperties(calculator);
                this.Change1.SetTotalLCA1Change1Properties(
                    this.Change1, sAttNameExtension, calculator);
                //costs
                if (this.Change1.SubP1Stock == null)
                {
                    this.Change1.SubP1Stock = new SubPrice1Stock();
                }
                this.Change1.SubP1Stock.SetTotalSubPrice1StocksProperties(calculator);
                //benefits
                if (this.Change1.SubP2Stock == null)
                {
                    this.Change1.SubP2Stock = new SubPrice2Stock();
                }
                this.Change1.SubP2Stock.SetTotalSubPrice2StocksProperties(calculator);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                if (this.Progress1 == null)
                {
                    this.Progress1 = new LCA1Progress1();
                }
                this.Progress1.SetCalculatorProperties(calculator);
                this.Progress1.SetTotalLCA1Progress1Properties(
                    this.Progress1, sAttNameExtension, calculator);
                //costs
                if (this.Progress1.SubP1Stock == null)
                {
                    this.Progress1.SubP1Stock = new SubPrice1Stock();
                }
                this.Progress1.SubP1Stock.SetTotalSubPrice1StocksProperties(calculator);
                //benefits
                if (this.Progress1.SubP2Stock == null)
                {
                    this.Progress1.SubP2Stock = new SubPrice2Stock();
                }
                this.Progress1.SubP2Stock.SetTotalSubPrice2StocksProperties(calculator);
            }
            else
            {
                if (this.Total1 == null)
                {
                    this.Total1 = new LCA1Total1();
                }
                this.Total1.SetCalculatorProperties(calculator);
                this.Total1.SetTotalLCA1Total1Properties(
                    this.Total1, sAttNameExtension, calculator);
                //costs
                if (this.Total1.SubP1Stock == null)
                {
                    this.Total1.SubP1Stock = new SubPrice1Stock();
                }
                this.Total1.SubP1Stock.SetTotalSubPrice1StocksProperties(calculator);
                //benefits
                if (this.Total1.SubP2Stock == null)
                {
                    this.Total1.SubP2Stock = new SubPrice2Stock();
                }
                this.Total1.SubP2Stock.SetTotalSubPrice2StocksProperties(calculator);
            }
        }
        
        public virtual void SetDescendantLCA1StockProperties(XElement currentCalculationsElement)
        {
            //abbreviated properties for descendant stock totals
            this.SetCalculatorProperties(currentCalculationsElement);
            //no set shared object props, they already have props set
            //don't SetTotalIndicator1StocksProperties because the descendants have their own stock totals
        }
        public virtual void SetTotalLCA1StocksProperty(string attName,
               string attValue, int colIndex)
        {
            if (this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                this.Stat1.SetTotalLCA1Stat1Property(
                    this.Stat1, attName, attValue);
                //costs
                this.Stat1.SubP1Stock.SetTotalSubPrice1StocksProperty(attName, attValue, colIndex);
                //benefits
                this.Stat1.SubP2Stock.SetTotalSubPrice2StocksProperty(attName, attValue, colIndex);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                this.Change1.SetTotalLCA1Change1Property(
                    this.Change1, attName, attValue);
                //costs
                this.Change1.SubP1Stock.SetTotalSubPrice1StocksProperty(attName, attValue, colIndex);
                //benefits
                this.Change1.SubP2Stock.SetTotalSubPrice2StocksProperty(attName, attValue, colIndex);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                this.Progress1.SetTotalLCA1Progress1Property(
                    this.Progress1, attName, attValue);
                //costs
                this.Progress1.SubP1Stock.SetTotalSubPrice1StocksProperty(attName, attValue, colIndex);
                //benefits
                this.Progress1.SubP2Stock.SetTotalSubPrice2StocksProperty(attName, attValue, colIndex);
            }
            else
            {
                this.Total1.SetTotalLCA1Total1Property(
                    this.Total1, attName, attValue);
                //costs
                this.Total1.SubP1Stock.SetTotalSubPrice1StocksProperty(attName, attValue, colIndex);
                //benefits
                this.Total1.SubP2Stock.SetTotalSubPrice2StocksProperty(attName, attValue, colIndex);
            }
        }
        public virtual string GetTotalLCA1StocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.AnalyzerType
                        == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                sPropertyValue = this.Stat1.GetTotalLCA1Stat1Property(
                   this.Stat1, attName);
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Stat1.SubP1Stock.GetTotalSubPrice1StocksProperty(attName, colIndex);
                }
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Stat1.SubP2Stock.GetTotalSubPrice2StocksProperty(attName, colIndex);
                }
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                sPropertyValue = this.Change1.GetTotalLCA1Change1Property(
                  this.Change1, attName);
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Change1.SubP1Stock.GetTotalSubPrice1StocksProperty(attName, colIndex);
                }
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Change1.SubP2Stock.GetTotalSubPrice2StocksProperty(attName, colIndex);
                }
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                sPropertyValue = this.Progress1.GetTotalLCA1Progress1Property(
                  this.Progress1, attName);
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Progress1.SubP1Stock.GetTotalSubPrice1StocksProperty(attName, colIndex);
                }
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Progress1.SubP2Stock.GetTotalSubPrice2StocksProperty(attName, colIndex);
                }
            }
            else
            {
                sPropertyValue = this.Total1.GetTotalLCA1Total1Property(
                    this.Total1, attName);
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Total1.SubP1Stock.GetTotalSubPrice1StocksProperty(attName, colIndex);
                }
                if (string.IsNullOrEmpty(sPropertyValue))
                {
                    sPropertyValue = this.Total1.SubP2Stock.GetTotalSubPrice2StocksProperty(attName, colIndex);
                }
            }
            return sPropertyValue;
        }
        public void SetDescendantLCA1StockAttributes(string attNameExt, ref XmlWriter writer)
        {
            //abbreviated list for decendant stock totals
            if (writer != null)
            {
                //set the base analyzer
                int iId = this.Id;
                this.SetCalculatorAttributes(attNameExt, ref writer);
                //set the calculations
                this.SetTotalLCA1StocksAttributes(attNameExt, ref writer);
            }
        }
        public void SetDescendantLCA1StockInputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XmlWriter writer)
        {
            //the lcatotal1 analyzer adds its totals to the base lcc1calculator linked view
            //less duplication and easier to display and extend
            if (writer != null)
            {
                //set the base analyzer
                this.SetCalculatorAttributes(attNameExt, ref writer);
                bool bHasCalcs = false;
                if (this.SubP1Stock.SubPrice1s != null)
                {
                    if (this.SubP1Stock.SubPrice1s.Count > 0)
                    {
                        LCC1Calculator lcc = (LCC1Calculator)this.SubP1Stock.SubPrice1s.FirstOrDefault();
                        if (lcc != null)
                        {
                            //set locals 
                            lcc.LCCInput.Local.SetLocalAttributesForCalculator(attNameExt, calcParams, 
                                ref writer);
                            lcc.SetLCC1Attributes(attNameExt, ref writer);
                            bHasCalcs = true;
                        }
                    }
                }
                //don't need both subps and subpstock
                if (!bHasCalcs)
                {
                    this.SetTotalLCA1StocksAttributes(attNameExt, ref writer);
                }
            }
        }
        public void SetDescendantLCA1StockOutputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XmlWriter writer)
        {
            //the lcatotal1 analyzer adds its totals to the base lcb1calculator linked view
            //less duplication and easier to extend
            if (writer != null)
            {
                if (this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcatotal1.ToString()
                    || this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
                {
                    this.SetCalculatorAttributes(attNameExt, ref writer);
                    bool bHasCalcs = false;
                    if (this.SubP2Stock.SubPrice1s != null)
                    {
                        if (this.SubP2Stock.SubPrice1s.Count > 0)
                        {
                            LCB1Calculator lcb = (LCB1Calculator)this.SubP2Stock.SubPrice1s.FirstOrDefault();
                            if (lcb != null)
                            {
                                //set locals 
                                lcb.LCBOutput.Local.SetLocalAttributesForCalculator(attNameExt, calcParams, ref writer);
                                lcb.SetLCB1Attributes(attNameExt, ref writer);
                                bHasCalcs = true;
                            }
                        }
                    }
                    //don't need both subps and subpstock
                    if (!bHasCalcs)
                    {
                        //set the analysis
                        this.SetTotalLCA1StocksAttributes(attNameExt, ref writer);
                    }
                }
            }
        }
        //the XElement methods were deprecated in favor of the XmlWriter methods
        public void SetDescendantLCA1StockAttributes(string attNameExt, ref XElement calculator)
        {
            //abbreviated list for decendant stock totals
            if (calculator != null)
            {
                //set the base analyzer
                this.SetAndRemoveCalculatorAttributes(attNameExt, ref calculator);
                //set the calculations
                this.SetTotalLCA1StocksAttributes(attNameExt, ref calculator);
            }
        }
        public void SetDescendantLCA1StockInputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XElement calculator, XElement currentElement)
        {
            //the lcatotal1 analyzer adds its totals to the base lcc1calculator linked view
            //less duplication and easier to display and extend
            if (calculator != null)
            {
                //can display input totals using two analyzers
                if (this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcatotal1.ToString()
                    || this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
                {
                    string sAttNameExt = string.Empty;
                    this.SetAndRemoveCalculatorAttributes(sAttNameExt, ref calculator);
                    bool bHasCalcs = false;
                    if (this.SubP1Stock.SubPrice1s != null)
                    {
                        if (this.SubP1Stock.SubPrice1s.Count > 0)
                        {
                            LCC1Calculator lcc = (LCC1Calculator)this.SubP1Stock.SubPrice1s.FirstOrDefault();
                            if (lcc != null)
                            {
                                //set locals 
                                lcc.LCCInput.Local.SetLocalAttributesForCalculator(calcParams, ref calculator);
                                lcc.SetLCC1Attributes(string.Empty, ref calculator);
                                bHasCalcs = true;
                            }
                        }
                    }
                    //don't need both subps and subpstock
                    if (!bHasCalcs)
                    {
                        this.SetTotalLCA1StocksAttributes(attNameExt, ref calculator);
                    }
                }
            }
        }
        
        public void SetDescendantLCA1StockOutputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XElement calculator, XElement currentElement)
        {
            //the lcatotal1 analyzer adds its totals to the base lcb1calculator linked view
            //less duplication and easier to extend
            if (calculator != null)
            {
                if (this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcatotal1.ToString()
                    || this.AnalyzerType
                    == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString()
                    || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
                {
                    string sAttNameExt = string.Empty;
                    this.SetAndRemoveCalculatorAttributes(sAttNameExt, ref calculator);
                    bool bHasCalcs = false;
                    if (this.SubP2Stock.SubPrice1s != null)
                    {
                        if (this.SubP2Stock.SubPrice1s.Count > 0)
                        {
                            LCB1Calculator lcb = (LCB1Calculator)this.SubP2Stock.SubPrice1s.FirstOrDefault();
                            if (lcb != null)
                            {
                                //set locals 
                                lcb.LCBOutput.Local.SetLocalAttributesForCalculator(calcParams, ref calculator);
                                lcb.SetLCB1Attributes(string.Empty, ref calculator);
                                bHasCalcs = true;
                            }
                        }
                    }
                    //don't need both subps and subpstock
                    if (!bHasCalcs)
                    {
                        //set the analysis
                        this.SetTotalLCA1StocksAttributes(attNameExt, ref calculator);
                    }
                }
            }
        }
        
        public virtual void SetTotalLCA1StocksAttributes(string attNameExt, ref XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    this.Stat1.CalcParameters.CurrentElementNodeName 
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Stat1.SetTotalLCA1Stat1Attributes(
                        this.Stat1, attNameExt, ref calculator);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Stat1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Stat1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                    else
                    {
                        //set both
                        this.Stat1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                        this.Stat1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                }
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    this.Change1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Change1.SetTotalLCA1Change1Attributes(
                        this.Change1, attNameExt, ref calculator);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Change1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Change1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                    else
                    {
                        //set both
                        this.Change1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                        this.Change1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                }
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Progress1.SetTotalLCA1Progress1Attributes(
                        this.Progress1, attNameExt, ref calculator);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Progress1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Progress1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                    else
                    {
                        //set both
                        this.Progress1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                        this.Progress1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    this.Total1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Total1.SetTotalLCA1Total1Attributes(
                        this.Total1, attNameExt, ref calculator);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Total1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Total1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                    else
                    {
                        //set both
                        this.Total1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref calculator);
                        this.Total1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref calculator);
                    }
                }
            }
        }

        public virtual void SetTotalLCA1StocksAttributes(string attNameExt, ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    this.Stat1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Stat1.SetTotalLCA1Stat1Attributes(
                        this.Stat1, attNameExt, ref writer);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Stat1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Stat1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                    else
                    {
                        //set both
                        this.Stat1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                        this.Stat1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                }
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    this.Change1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Change1.SetTotalLCA1Change1Attributes(
                        this.Change1, attNameExt, ref writer);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Change1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Change1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                    else
                    {
                        //set both
                        this.Change1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                        this.Change1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                }
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Progress1.SetTotalLCA1Progress1Attributes(
                        this.Progress1, attNameExt, ref writer);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Progress1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Progress1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                    else
                    {
                        //set both
                        this.Progress1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                        this.Progress1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    this.Total1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Total1.SetTotalLCA1Total1Attributes(
                        this.Total1, attNameExt, ref writer);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Total1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Total1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                    else
                    {
                        //set both
                        this.Total1.SubP1Stock.SetTotalSubPrice1StocksAttributes(attNameExt, ref writer);
                        this.Total1.SubP2Stock.SetTotalSubPrice2StocksAttributes(attNameExt, ref writer);
                    }
                }
            }
        }
        //run the analyses
        public bool RunAnalyses()
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this);
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                bHasAnalyses = this.Progress1.RunAnalyses(this);
            }
            else
            {
                //add totals to partial target stocks
                bHasAnalyses = this.Total1.RunAnalyses(this);
            }
            return bHasAnalyses;
        }
        public bool RunAnalyses(List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcastat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeyr.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangeid.ToString()
                || this.AnalyzerType == LCA1AnalyzerHelper.ANALYZER_TYPES.lcachangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType
                == LCA1AnalyzerHelper.ANALYZER_TYPES.lcaprogress1.ToString())
            {
                bHasAnalyses = this.Progress1.RunAnalyses(this, calcs);
            }
            else
            {
                //add totals to partial target stocks
                bHasAnalyses = this.Total1.RunAnalyses(this, calcs);
            }
            return bHasAnalyses;
        }
    }
}