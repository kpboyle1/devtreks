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
    ///             Costs: MN1Stock.Total1.MNSR1Stock
    ///             Benefits: MN1Stock.Total1.MNSR2Stock
    ///             Important to use separate agg objects because
    ///             some analyses are sequential and use the results of previous analyses.
    ///             The class keeps track of lca quantitative totals.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTEs:       1. Analyzer should only be run at budget.TimePeriod or otherElement.Group level. 
    ///</summary>
    public class MN1Stock : Calculator1
    {
        //calls the base-class version, and initializes the base class properties.
        public MN1Stock(CalculatorParameters calcParams, string analyzerType)
            : base()
        {
            if (calcParams != null)
            {
                this.CalcParameters = new CalculatorParameters(calcParams);
            }
            //MNSR01Stock has to know which analysis to run
            this.AnalyzerType = analyzerType;
            //subprice object
            InitTotalMN1StockProperties();
        }
        //copy constructor
        public MN1Stock(MN1Stock calculator)
        {
            CopyTotalMN1StockProperties(calculator);
        }
        //inheritors
        public MN1Stock()
            : base()
        {
        }
        //note the properties contained in MNSR01Stock are also used for display
        public CalculatorParameters CalcParameters { get; set; }
        //costs
        public MNSR01Stock MNSR1Stock { get; set; }
        //benefits
        public MNSR02Stock MNSR2Stock { get; set; }
        //changes and progress use collections of results
        public List<MN1Stock> Stocks { get; set; }
        //totals analyses (see if any of the others need this to be a list)
        public MN1Total1 Total1 { get; set; }
        //statistical analyses
        public MN1Stat1 Stat1 { get; set; }
        //change analyses
        public MN1Change1 Change1 { get; set; }
        //progress analyses
        public MN1Progress1 Progress1 { get; set; }

        public virtual void InitTotalMN1StockProperties()
        {
            if (this.MNSR1Stock == null)
            {
                this.MNSR1Stock = new MNSR01Stock();
            }
            if (this.MNSR2Stock == null)
            {
                this.MNSR2Stock = new MNSR02Stock();
            }
            if (this.Total1 == null)
            {
                this.Total1 = new MN1Total1(this.CalcParameters);
            }
            if (this.Stat1 == null)
            {
                this.Stat1 = new MN1Stat1(this.CalcParameters);
            }
            if (this.Change1 == null)
            {
                this.Change1 = new MN1Change1(this.CalcParameters);
            }
            if (this.Progress1 == null)
            {
                this.Progress1 = new MN1Progress1(this.CalcParameters);
            }
            if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                this.Stat1 = new MN1Stat1(this.CalcParameters);
                this.Stat1.InitTotalMN1Stat1Properties(this.Stat1, this.CalcParameters);
                //set costs
                this.Stat1.MNSR1Stock = new MNSR01Stock();
                this.Stat1.MNSR1Stock.InitTotalMNSR01StockProperties();
                //set benefits
                this.Stat1.MNSR2Stock = new MNSR02Stock();
                this.Stat1.MNSR2Stock.InitTotalMNSR02StockProperties();
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                this.Change1 = new MN1Change1(this.CalcParameters);
                this.Change1.InitTotalMN1Change1Properties(this.Change1, this.CalcParameters);
                //set costs
                this.Change1.MNSR1Stock = new MNSR01Stock();
                this.Change1.MNSR1Stock.InitTotalMNSR01StockProperties();
                //set benefits
                this.Change1.MNSR2Stock = new MNSR02Stock();
                this.Change1.MNSR2Stock.InitTotalMNSR02StockProperties();
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                this.Progress1 = new MN1Progress1(this.CalcParameters);
                this.Progress1.InitTotalMN1Progress1Properties(this.Progress1, this.CalcParameters);
                //set costs
                this.Progress1.MNSR1Stock = new MNSR01Stock();
                this.Progress1.MNSR1Stock.InitTotalMNSR01StockProperties();
                //set benefits
                this.Progress1.MNSR2Stock = new MNSR02Stock();
                this.Progress1.MNSR2Stock.InitTotalMNSR02StockProperties();
            }
            else
            {
                //default are total indicators
                this.Total1 = new MN1Total1(this.CalcParameters);
                this.Total1.InitTotalMN1Total1Properties(this.Total1, this.CalcParameters);
                //set costs
                this.Total1.MNSR1Stock = new MNSR01Stock();
                this.Total1.MNSR1Stock.InitTotalMNSR01StockProperties();
                //set benefits
                this.Total1.MNSR2Stock = new MNSR02Stock();
                this.Total1.MNSR2Stock.InitTotalMNSR02StockProperties();
            }
        }
        public virtual void CopyTotalMN1StockProperties(
              MN1Stock calculator)
        {
            if (this.AnalyzerType 
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                this.Stat1 = new MN1Stat1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Stat1.CopyCalculatorProperties(calculator.Stat1);
                this.Stat1.CopyTotalMN1Stat1Properties(
                    this.Stat1, calculator.Stat1);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                this.Change1 = new MN1Change1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Change1.CopyCalculatorProperties(calculator.Change1);
                this.Change1.CopyTotalMN1Change1Properties(
                    this.Change1, calculator.Change1);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                this.Progress1 = new MN1Progress1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Progress1.CopyCalculatorProperties(calculator.Progress1);
                this.Progress1.CopyTotalMN1Progress1Properties(
                    this.Progress1, calculator.Progress1);
            }
            else
            {
                //this handles subobjects (calparams, subp1stock, subp2stock)
                this.Total1 = new MN1Total1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Total1.CopyCalculatorProperties(calculator.Total1);
                this.Total1.CopyTotalMN1Total1Properties(
                    this.Total1, calculator.Total1);
            }
            
        }
        public virtual void SetTotalMN1StockProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            string sAttNameExtension = string.Empty;
            if (this.AnalyzerType
                        == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                if (this.Stat1 == null)
                {
                    this.Stat1 = new MN1Stat1(this.CalcParameters);
                }
                this.Stat1.SetCalculatorProperties(calculator);
                this.Stat1.SetTotalMN1Stat1Properties(
                    this.Stat1, sAttNameExtension, calculator);
                //costs
                if (this.Stat1.MNSR1Stock == null)
                {
                    this.Stat1.MNSR1Stock = new MNSR01Stock();
                }
                this.Stat1.MNSR1Stock.SetTotalMNSR01StockProperties(calculator);
                //benefits
                if (this.Stat1.MNSR2Stock == null)
                {
                    this.Stat1.MNSR2Stock = new MNSR02Stock();
                }
                this.Stat1.MNSR2Stock.SetTotalMNSR02StockProperties(calculator);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                if (this.Change1 == null)
                {
                    this.Change1 = new MN1Change1(this.CalcParameters);
                }
                this.Change1.SetCalculatorProperties(calculator);
                this.Change1.SetTotalMN1Change1Properties(
                    this.Change1, sAttNameExtension, calculator);
                //costs
                if (this.Change1.MNSR1Stock == null)
                {
                    this.Change1.MNSR1Stock = new MNSR01Stock();
                }
                this.Change1.MNSR1Stock.SetTotalMNSR01StockProperties(calculator);
                //benefits
                if (this.Change1.MNSR2Stock == null)
                {
                    this.Change1.MNSR2Stock = new MNSR02Stock();
                }
                this.Change1.MNSR2Stock.SetTotalMNSR02StockProperties(calculator);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                if (this.Progress1 == null)
                {
                    this.Progress1 = new MN1Progress1(this.CalcParameters);
                }
                this.Progress1.SetCalculatorProperties(calculator);
                this.Progress1.SetTotalMN1Progress1Properties(
                    this.Progress1, sAttNameExtension, calculator);
                //costs
                if (this.Progress1.MNSR1Stock == null)
                {
                    this.Progress1.MNSR1Stock = new MNSR01Stock();
                }
                this.Progress1.MNSR1Stock.SetTotalMNSR01StockProperties(calculator);
                //benefits
                if (this.Progress1.MNSR2Stock == null)
                {
                    this.Progress1.MNSR2Stock = new MNSR02Stock();
                }
                this.Progress1.MNSR2Stock.SetTotalMNSR02StockProperties(calculator);
            }
            else
            {
                if (this.Total1 == null)
                {
                    this.Total1 = new MN1Total1(this.CalcParameters);
                }
                this.Total1.SetCalculatorProperties(calculator);
                this.Total1.SetTotalMN1Total1Properties(
                    this.Total1, sAttNameExtension, calculator);
                //costs
                if (this.Total1.MNSR1Stock == null)
                {
                    this.Total1.MNSR1Stock = new MNSR01Stock();
                }
                this.Total1.MNSR1Stock.SetTotalMNSR01StockProperties(calculator);
                //benefits
                if (this.Total1.MNSR2Stock == null)
                {
                    this.Total1.MNSR2Stock = new MNSR02Stock();
                }
                this.Total1.MNSR2Stock.SetTotalMNSR02StockProperties(calculator);
            }
        }
        
        public virtual void SetDescendantMN1StockProperties(XElement currentCalculationsElement)
        {
            //abbreviated properties for descendant stock totals
            this.SetCalculatorProperties(currentCalculationsElement);
            //no set shared object props, they already have props set
            //don't SetTotalIndicator1StockProperties because the descendants have their own stock totals
        }
        public virtual void SetTotalMN1StockProperty(string attName,
               string attValue)
        {
            if (this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                this.Stat1.SetTotalMN1Stat1Property(
                    this.Stat1, attName, attValue);
                //costs
                this.Stat1.MNSR1Stock.SetTotalMNSR01StockProperty(attName, attValue);
                //benefits
                this.Stat1.MNSR2Stock.SetTotalMNSR02StockProperty(attName, attValue);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                this.Change1.SetTotalMN1Change1Property(
                    this.Change1, attName, attValue);
                //costs
                this.Change1.MNSR1Stock.SetTotalMNSR01StockProperty(attName, attValue);
                //benefits
                this.Change1.MNSR2Stock.SetTotalMNSR02StockProperty(attName, attValue);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                this.Progress1.SetTotalMN1Progress1Property(
                    this.Progress1, attName, attValue);
                //costs
                this.Progress1.MNSR1Stock.SetTotalMNSR01StockProperty(attName, attValue);
                //benefits
                this.Progress1.MNSR2Stock.SetTotalMNSR02StockProperty(attName, attValue);
            }
            else
            {
                this.Total1.SetTotalMN1Total1Property(
                    this.Total1, attName, attValue);
                //costs
                this.Total1.MNSR1Stock.SetTotalMNSR01StockProperty(attName, attValue);
                //benefits
                this.Total1.MNSR2Stock.SetTotalMNSR02StockProperty(attName, attValue);
            }
        }
        public virtual string GetTotalMN1StockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            //if (this.AnalyzerType
            //            == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            //{
            //    sPropertyValue = this.Stat1.GetTotalMN1Stat1Property(
            //       this.Stat1, attName);
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Stat1.MNSR1Stock.GetTotalMNSR01StockProperty(attName);
            //    }
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Stat1.MNSR2Stock.GetTotalMNSR02StockProperty(attName);
            //    }
            //}
            //else if (this.AnalyzerType
            //    == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
            //    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
            //    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            //{
            //    sPropertyValue = this.Change1.GetTotalMN1Change1Property(
            //      this.Change1, attName);
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Change1.MNSR1Stock.GetTotalMNSR01StockProperty(attName);
            //    }
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Change1.MNSR2Stock.GetTotalMNSR02StockProperty(attName);
            //    }
            //}
            //else if (this.AnalyzerType
            //    == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            //{
            //    sPropertyValue = this.Progress1.GetTotalMN1Progress1Property(
            //      this.Progress1, attName);
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Progress1.MNSR1Stock.GetTotalMNSR01StockProperty(attName);
            //    }
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Progress1.MNSR2Stock.GetTotalMNSR02StockProperty(attName);
            //    }
            //}
            //else
            //{
            //    sPropertyValue = this.Total1.GetTotalMN1Total1Property(
            //        this.Total1, attName);
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Total1.MNSR1Stock.GetTotalMNSR01StockProperty(attName);
            //    }
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Total1.MNSR2Stock.GetTotalMNSR02StockProperty(attName);
            //    }
            //}
            return sPropertyValue;
        }
        public void SetDescendantMN1StockAttributes(string attNameExt, ref XmlWriter writer)
        {
            //abbreviated list for decendant stock totals
            if (writer != null)
            {
                //set the base analyzer
                int iId = this.Id;
                this.SetCalculatorAttributes(attNameExt, ref writer);
                //set the calculations
                this.SetTotalMN1StockAttributes(attNameExt, ref writer);
            }
        }
        public void SetDescendantMN1StockInputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XmlWriter writer)
        {
            //the mntotal1 analyzer adds its totals to the base mnc1calculator linked view
            //less duplication and easier to display and extend
            if (writer != null)
            {
                //set the base analyzer
                this.SetCalculatorAttributes(attNameExt, ref writer);
                this.SetTotalMN1StockAttributes(attNameExt, ref writer);
            }
        }
        public void SetDescendantMN1StockOutputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XmlWriter writer)
        {
            //the mntotal1 analyzer adds its totals to the base mnb1calculator linked view
            //less duplication and easier to extend
            if (writer != null)
            {
                if (this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString()
                    || this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                {
                    this.SetCalculatorAttributes(attNameExt, ref writer);
                    //set the analysis
                    this.SetTotalMN1StockAttributes(attNameExt, ref writer);
                }
            }
        }
        //the XElement methods were deprecated in favor of the XmlWriter methods
        public void SetDescendantMN1StockAttributes(string attNameExt, ref XElement calculator)
        {
            //abbreviated list for decendant stock totals
            if (calculator != null)
            {
                //set the base analyzer
                this.SetAndRemoveCalculatorAttributes(attNameExt, ref calculator);
                //set the calculations
                this.SetTotalMN1StockAttributes(attNameExt, ref calculator);
            }
        }
        public void SetDescendantMN1StockInputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XElement calculator, XElement currentElement)
        {
            //the mntotal1 analyzer adds its totals to the base mnc1calculator linked view
            //less duplication and easier to display and extend
            if (calculator != null)
            {
                //can display input totals using two analyzers
                if (this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString()
                    || this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                {
                    string sAttNameExt = string.Empty;
                    this.SetAndRemoveCalculatorAttributes(sAttNameExt, ref calculator);
                    this.SetTotalMN1StockAttributes(attNameExt, ref calculator);
                }
            }
        }
        
        public void SetDescendantMN1StockOutputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XElement calculator, XElement currentElement)
        {
            //the mntotal1 analyzer adds its totals to the base mnb1calculator linked view
            //less duplication and easier to extend
            if (calculator != null)
            {
                if (this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mntotal1.ToString()
                    || this.AnalyzerType
                    == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString()
                    || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
                {
                    string sAttNameExt = string.Empty;
                    this.SetAndRemoveCalculatorAttributes(sAttNameExt, ref calculator);
                    //set the analysis
                    this.SetTotalMN1StockAttributes(attNameExt, ref calculator);
                }
            }
        }
        
        public virtual void SetTotalMN1StockAttributes(string attNameExt, ref XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    this.Stat1.CalcParameters.CurrentElementNodeName 
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Stat1.SetTotalMN1Stat1Attributes(
                        this.Stat1, attNameExt, ref calculator);
                    //set the 10 nutrients

                }
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    this.Change1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Change1.SetTotalMN1Change1Attributes(
                        this.Change1, attNameExt, ref calculator);
                }
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Progress1.SetTotalMN1Progress1Attributes(
                        this.Progress1, attNameExt, ref calculator);
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    this.Total1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Total1.SetTotalMN1Total1Attributes(
                        this.Total1, attNameExt, ref calculator);
                }
            }
        }
        
        public virtual void SetTotalMN1StockAttributes(string attNameExt, ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    //set the analysis
                    this.Stat1.SetTotalMN1Stat1Attributes(
                        this.Stat1, attNameExt, ref writer);
                }
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    //set the analysis
                    this.Change1.SetTotalMN1Change1Attributes(
                        this.Change1, attNameExt, ref writer);
                }
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Progress1.SetTotalMN1Progress1Attributes(
                        this.Progress1, attNameExt, ref writer);
                    
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
                    bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
                    this.Total1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Total1.SetTotalMN1Total1Attributes(
                        this.Total1, attNameExt, ref writer);
                    if (bIsCostNode)
                    {
                        //set the costs
                        this.Total1.MNSR1Stock.SetTotalMNSR01StockAttributes(attNameExt, ref writer);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        this.Total1.MNSR2Stock.SetTotalMNSR02StockAttributes(attNameExt, ref writer);
                    }
                    else
                    {
                        //stock01 is the net from subtracting outputs from inputs (otherwise too many nutr to display)
                        this.Total1.MNSR1Stock.SetTotalMNSR01StockAttributes(attNameExt, ref writer);
                    }
                }
            }
        }
        
        //run the analyses
        public bool RunAnalyses()
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this);
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
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
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnstat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeyr.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangeid.ToString()
                || this.AnalyzerType == MN1AnalyzerHelper.ANALYZER_TYPES.mnchangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType
                == MN1AnalyzerHelper.ANALYZER_TYPES.mnprogress1.ToString())
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