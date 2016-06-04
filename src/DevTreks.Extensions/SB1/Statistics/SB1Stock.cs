using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             Costs: SB1Stock.T1.SB11Stock
    ///             Benefits: SB1Stock.T1.SB12Stock
    ///             Important to use separate agg objects because
    ///             some analyses are sequential and use the results of previous analyses.
    ///             The class keeps track of stock quantitative totals.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. Analyzer should only be run at budget.TimePeriod or otherElement.Group level. 
    ///</summary>
    public class SB1Stock : TSB1BaseStock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB1Stock(CalculatorParameters calcParams, string analyzerType)
            : base()
        {
            if (calcParams != null)
            {
                this.CalcParameters = new CalculatorParameters(calcParams);
            }
            //SB101Stock has to know which analysis to run
            this.AnalyzerType = analyzerType;
            //subprice object
            InitTotalSB1StockProperties();
        }
        //copy constructor
        public SB1Stock(SB1Stock calculator)
        {
            CopyTotalSB1StockProperties(calculator);
        }
        //inheritors
        public SB1Stock()
            : base()
        {
        }
        //note the properties contained in SB101Stock are also used for display
        public CalculatorParameters CalcParameters { get; set; }
        //input indicators
        public SB101Stock SB11Stock { get; set; }
        //output indicators
        public SB102Stock SB12Stock { get; set; }
        //changes and progress use collections of results
        public List<SB1Stock> Stocks { get; set; }
        //totals analyses (see if any of the others need this to be a list)
        public SB1Total1 Total1 { get; set; }
        //statistical analyses
        public SB1Stat1 Stat1 { get; set; }
        //change analyses
        public SB1Change1 Change1 { get; set; }
        //progress analyses
        public SB1Progress1 Progress1 { get; set; }

        
        public virtual void InitTotalSB1StockProperties()
        {
            if (this.SB11Stock == null)
            {
                this.SB11Stock = new SB101Stock();
            }
            if (this.SB12Stock == null)
            {
                this.SB12Stock = new SB102Stock();
            }
            if (this.Total1 == null)
            {
                this.Total1 = new SB1Total1(this.CalcParameters);
            }
            if (this.Stat1 == null)
            {
                this.Stat1 = new SB1Stat1(this.CalcParameters);
            }
            if (this.Change1 == null)
            {
                this.Change1 = new SB1Change1(this.CalcParameters);
            }
            if (this.Progress1 == null)
            {
                this.Progress1 = new SB1Progress1(this.CalcParameters);
            }
            if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                this.Stat1 = new SB1Stat1(this.CalcParameters);
                this.Stat1.InitTotalSB1Stat1Properties(this.Stat1, this.CalcParameters);
                this.Stat1.InitSB1AnalysisProperties(this.Stat1, this.CalcParameters);
                //set costs
                this.Stat1.SB11Stock = new SB101Stock();
                this.Stat1.SB11Stock.InitTotalSB101StockProperties();
                //set benefits
                this.Stat1.SB12Stock = new SB102Stock();
                this.Stat1.SB12Stock.InitTotalSB102StockProperties();
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                this.Change1 = new SB1Change1(this.CalcParameters);
                this.Change1.InitTotalSB1Change1Properties(this.Change1, this.CalcParameters);
                this.Change1.InitSB1AnalysisProperties(this.Change1, this.CalcParameters);
                //set costs
                this.Change1.SB11Stock = new SB101Stock();
                this.Change1.SB11Stock.InitTotalSB101StockProperties();
                //set benefits
                this.Change1.SB12Stock = new SB102Stock();
                this.Change1.SB12Stock.InitTotalSB102StockProperties();
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                this.Progress1 = new SB1Progress1(this.CalcParameters);
                this.Progress1.InitTotalSB1Progress1Properties(this.Progress1, this.CalcParameters);
                this.Progress1.InitSB1AnalysisProperties(this.Progress1, this.CalcParameters);
                //set costs
                this.Progress1.SB11Stock = new SB101Stock();
                this.Progress1.SB11Stock.InitTotalSB101StockProperties();
                //set benefits
                this.Progress1.SB12Stock = new SB102Stock();
                this.Progress1.SB12Stock.InitTotalSB102StockProperties();
            }
            else
            {
                //default are total indicators
                this.Total1 = new SB1Total1(this.CalcParameters);
                this.Total1.InitTotalSB1Total1Properties(this.Total1, this.CalcParameters);
                //set costs
                this.Total1.SB11Stock = new SB101Stock();
                this.Total1.SB11Stock.InitTotalSB101StockProperties();
                //set benefits
                this.Total1.SB12Stock = new SB102Stock();
                this.Total1.SB12Stock.InitTotalSB102StockProperties();
            }

        }
        public virtual void CopyTotalSB1StockProperties(
              SB1Stock calculator)
        {
            if (this.AnalyzerType 
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                this.Stat1 = new SB1Stat1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Stat1.CopyCalculatorProperties(calculator.Stat1);
                this.Stat1.CopyTotalSB1Stat1Properties(
                    this.Stat1, calculator.Stat1);
                this.Stat1.CopySB1AnalysisProperties(
                    this.Stat1, calculator.Stat1);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                this.Change1 = new SB1Change1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Change1.CopyCalculatorProperties(calculator.Change1);
                this.Change1.CopyTotalSB1Change1Properties(
                    this.Change1, calculator.Change1);
                this.Change1.CopySB1AnalysisProperties(
                    this.Change1, calculator.Change1);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                this.Progress1 = new SB1Progress1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Progress1.CopyCalculatorProperties(calculator.Progress1);
                this.Progress1.CopyTotalSB1Progress1Properties(
                    this.Progress1, calculator.Progress1);
                this.Progress1.CopySB1AnalysisProperties(
                    this.Progress1, calculator.Progress1);
            }
            else
            {
                //this handles subobjects (calparams, subp1stock, subp2stock)
                this.Total1 = new SB1Total1(this.CalcParameters);
                this.CopyCalculatorProperties(calculator);
                this.Total1.CopyCalculatorProperties(calculator.Total1);
                this.Total1.CopyTotalSB1Total1Properties(
                    this.Total1, calculator.Total1);
            }
            
        }
        public virtual void SetTotalSB1StockProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            string sAttNameExtension = string.Empty;
            if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                if (this.Stat1 == null)
                {
                    this.Stat1 = new SB1Stat1(this.CalcParameters);
                }
                this.Stat1.SetCalculatorProperties(calculator);
                this.Stat1.SetTotalSB1Stat1Properties(
                    this.Stat1, sAttNameExtension, calculator);
                this.Stat1.SetSB1AnalysisProperties(
                    this.Stat1, sAttNameExtension, calculator);
                //costs
                if (this.Stat1.SB11Stock == null)
                {
                    this.Stat1.SB11Stock = new SB101Stock();
                }
                this.Stat1.SB11Stock.SetTotalSB101StockProperties(calculator);
                //benefits
                if (this.Stat1.SB12Stock == null)
                {
                    this.Stat1.SB12Stock = new SB102Stock();
                }
                this.Stat1.SB12Stock.SetTotalSB102StockProperties(calculator);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                if (this.Change1 == null)
                {
                    this.Change1 = new SB1Change1(this.CalcParameters);
                }
                this.Change1.SetCalculatorProperties(calculator);
                this.Change1.SetTotalSB1Change1Properties(
                    this.Change1, sAttNameExtension, calculator);
                this.Change1.SetSB1AnalysisProperties(
                    this.Change1, sAttNameExtension, calculator);
                //costs
                if (this.Change1.SB11Stock == null)
                {
                    this.Change1.SB11Stock = new SB101Stock();
                }
                this.Change1.SB11Stock.SetTotalSB101StockProperties(calculator);
                //benefits
                if (this.Change1.SB12Stock == null)
                {
                    this.Change1.SB12Stock = new SB102Stock();
                }
                this.Change1.SB12Stock.SetTotalSB102StockProperties(calculator);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                if (this.Progress1 == null)
                {
                    this.Progress1 = new SB1Progress1(this.CalcParameters);
                }
                this.Progress1.SetCalculatorProperties(calculator);
                this.Progress1.SetTotalSB1Progress1Properties(
                    this.Progress1, sAttNameExtension, calculator);
                this.Progress1.SetSB1AnalysisProperties(
                    this.Progress1, sAttNameExtension, calculator);
                //costs
                if (this.Progress1.SB11Stock == null)
                {
                    this.Progress1.SB11Stock = new SB101Stock();
                }
                this.Progress1.SB11Stock.SetTotalSB101StockProperties(calculator);
                //benefits
                if (this.Progress1.SB12Stock == null)
                {
                    this.Progress1.SB12Stock = new SB102Stock();
                }
                this.Progress1.SB12Stock.SetTotalSB102StockProperties(calculator);
            }
            else
            {
                if (this.Total1 == null)
                {
                    this.Total1 = new SB1Total1(this.CalcParameters);
                }
                this.Total1.SetCalculatorProperties(calculator);
                //costs
                if (this.Total1.SB11Stock == null)
                {
                    this.Total1.SB11Stock = new SB101Stock();
                }
                this.Total1.SB11Stock.SetTotalSB101StockProperties(calculator);
                //benefits
                if (this.Total1.SB12Stock == null)
                {
                    this.Total1.SB12Stock = new SB102Stock();
                }
                this.Total1.SB12Stock.SetTotalSB102StockProperties(calculator);
            }
        }
        
        public virtual void SetDescendantSB1StockProperties(XElement currentCalculationsElement)
        {
            //abbreviated properties for descendant stock totals
            this.SetCalculatorProperties(currentCalculationsElement);
            //no set shared object props, they already have props set
            //don't SetTotalIndicator1StockProperties because the descendants have their own stock totals
        }
        public virtual void SetTotalSB1StockProperty(string attName,
               string attValue)
        {
            if (this.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                this.Stat1.SetTotalSB1Stat1Property(
                    this.Stat1, attName, attValue);
                this.Stat1.SetSB1AnalysisProperty(this.Stat1, attName, attValue);
                //costs
                this.Stat1.SB11Stock.SetTotalSB101StockProperty(attName, attValue);
                //benefits
                this.Stat1.SB12Stock.SetTotalSB102StockProperty(attName, attValue);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                this.Change1.SetTotalSB1Change1Property(
                    this.Change1, attName, attValue);
                this.Change1.SetSB1AnalysisProperty(this.Change1, attName, attValue);
                //costs
                this.Change1.SB11Stock.SetTotalSB101StockProperty(attName, attValue);
                //benefits
                this.Change1.SB12Stock.SetTotalSB102StockProperty(attName, attValue);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                this.Progress1.SetTotalSB1Progress1Property(
                    this.Progress1, attName, attValue);
                this.Progress1.SetSB1AnalysisProperty(this.Progress1, attName, attValue);
                //costs
                this.Progress1.SB11Stock.SetTotalSB101StockProperty(attName, attValue);
                //benefits
                this.Progress1.SB12Stock.SetTotalSB102StockProperty(attName, attValue);
            }
            else
            {
                //the subsequent calls handle
                //this.Total1.SetTotalSB1Total1Property(
                //    this.Total1, attName, attValue);
                //costs
                this.Total1.SB11Stock.SetTotalSB101StockProperty(attName, attValue);
                //benefits
                this.Total1.SB12Stock.SetTotalSB102StockProperty(attName, attValue);
            }
        }
        public virtual string GetTotalSB1StockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            //if (this.AnalyzerType
            //            == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            //{
            //    sPropertyValue = this.Stat1.GetTotalSB1Stat1Property(
            //       this.Stat1, attName);
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Stat1.SB11Stock.GetTotalSB101StockProperty(attName);
            //    }
            //    if (string.IsNullOrEmpty(sPropertyValue))
            //    {
            //        sPropertyValue = this.Stat1.SB12Stock.GetTotalSB102StockProperty(attName);
            //    }
            //}
            return sPropertyValue;
        }
        public async Task SetDescendantSB1StockAttributesAsync(string attNameExt, XmlWriter writer)
        {
            //abbreviated list for decendant stock totals
            if (writer != null)
            {
                //set the base analyzer
                int iId = this.Id;
                await this.SetCalculatorAttributesAsync(attNameExt, writer);
                //set the calculations
                await this.SetTotalSB1StockAttributesAsync(attNameExt, writer);
            }
        }
        public async Task SetDescendantSB1StockInputAttributesAsync(string attNameExt, CalculatorParameters calcParams,
            XmlWriter writer)
        {
            //the sbtotal1 analyzer adds its totals to the base sbc1calculator linked view
            //less duplication and easier to display and extend
            if (writer != null)
            {
                //set the base analyzer
                await this.SetCalculatorAttributesAsync(attNameExt, writer);
                await this.SetTotalSB1StockAttributesAsync(attNameExt, writer);
            }
        }
        public async Task SetDescendantSB1StockOutputAttributesAsync(string attNameExt, CalculatorParameters calcParams,
            XmlWriter writer)
        {
            //the sbtotal1 analyzer adds its totals to the base sbb1calculator linked view
            //less duplication and easier to extend
            if (writer != null)
            {
                await this.SetCalculatorAttributesAsync(attNameExt, writer);
                //set the analysis
                await this.SetTotalSB1StockAttributesAsync(attNameExt, writer);
            }
        }
        
        //the XElement methods were deprecated in favor of the XmlWriter methods
        public void  SetDescendantSB1StockAttributes(string attNameExt, XElement calculator)
        {
            //abbreviated list for decendant stock totals
            if (calculator != null)
            {
                //set the base analyzer
                this.SetAndRemoveCalculatorAttributes(attNameExt, calculator);
                //set the calculations
                this.SetTotalSB1StockAttributes(attNameExt, calculator);
            }
        }
        public void  SetDescendantSB1StockInputAttributes(string attNameExt, CalculatorParameters calcParams,
            XElement calculator, XElement currentElement)
        {
            //the sbtotal1 analyzer adds its totals to the base sbc1calculator linked view
            //less duplication and easier to display and extend
            if (calculator != null)
            {
                //can display input totals using two analyzers
                if (this.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString()
                    || this.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                {
                    string sAttNameExt = string.Empty;
                    this.SetAndRemoveCalculatorAttributes(sAttNameExt, calculator);
                    this.SetTotalSB1StockAttributes(attNameExt, calculator);
                }
            }
        }
        
        public void  SetDescendantSB1StockOutputAttributes(string attNameExt, CalculatorParameters calcParams,
            XElement calculator, XElement currentElement)
        {
            //the sbtotal1 analyzer adds its totals to the base sbb1calculator linked view
            //less duplication and easier to extend
            if (calculator != null)
            {
                if (this.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbtotal1.ToString()
                    || this.AnalyzerType
                    == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString()
                    || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
                {
                    string sAttNameExt = string.Empty;
                    this.SetAndRemoveCalculatorAttributes(sAttNameExt, calculator);
                    //set the analysis
                    this.SetTotalSB1StockAttributes(attNameExt, calculator);
                }
            }
        }
        public virtual void  SetTotalSB1StockAttributes(string attNameExt, XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    this.Stat1.CalcParameters.CurrentElementNodeName 
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Stat1.SetTotalSB1Stat1Attributes(
                        this.Stat1, attNameExt, calculator);
                    this.Stat1.SetSB1AnalysisAttributes(
                        this.Stat1, attNameExt, calculator);
                }
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    this.Change1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Change1.SetTotalSB1Change1Attributes(
                        this.Change1, attNameExt, calculator);
                    this.Change1.SetSB1AnalysisAttributes(
                        this.Change1, attNameExt, calculator);
                }
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    this.Progress1.SetTotalSB1Progress1Attributes(
                        this.Progress1, attNameExt, calculator);
                    this.Progress1.SetSB1AnalysisAttributes(
                        this.Progress1, attNameExt, calculator);
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    this.Total1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                }
            }
        }

        
        public virtual async Task SetTotalSB1StockAttributesAsync(string attNameExt, XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    //set the analysis
                    await this.Stat1.SetTotalSB1Stat1AttributesAsync(
                        this.Stat1, attNameExt, writer);
                    await this.Stat1.SetSB1AnalysisAttributesAsync(
                       this.Stat1, attNameExt, writer);
                    //row count first comes from actual number of indicators
                    this.SBCount = this.Stat1.SBCount;
                }
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    //set the analysis
                    await this.Change1.SetTotalSB1Change1AttributesAsync(
                        this.Change1, attNameExt, writer);
                    await this.Change1.SetSB1AnalysisAttributesAsync(
                       this.Change1, attNameExt, writer);
                    this.SBCount = this.Change1.SBCount;

                }
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    //set the analysis
                    await this.Progress1.SetTotalSB1Progress1AttributesAsync(
                        this.Progress1, attNameExt, writer);
                    await this.Progress1.SetSB1AnalysisAttributesAsync(
                       this.Progress1, attNameExt, writer);
                    this.SBCount = this.Progress1.SBCount;
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
                    //the two subsequent methods set the atts
                    //this.Total1.SetTotalSB1Total1AttributesAsync(
                    //    this.Total1, attNameExt, writer);
                    if (bIsCostNode)
                    {
                        //set the costs
                        await this.Total1.SB11Stock.SetTotalSB101StockAttributesAsync(attNameExt, writer);
                    }
                    else if (bIsBenefitNode)
                    {
                        //set the benefits
                        await this.Total1.SB12Stock.SetTotalSB102StockAttributesAsync(attNameExt, writer);
                    }
                    else
                    {
                        //stock01 is input totals
                        await this.Total1.SB11Stock.SetTotalSB101StockAttributesAsync(attNameExt, writer);
                    }
                }
            }
        }
        
        //run the analyses
        public async Task<bool> RunAnalyses()
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                bHasAnalyses = await this.Stat1.RunAnalyses(this);
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                bHasAnalyses = await this.Change1.RunAnalyses(this);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
            {
                bHasAnalyses = await this.Progress1.RunAnalyses(this);
            }
            else
            {
                //add totals to partial target stocks
                bHasAnalyses = await this.Total1.RunAnalyses(this);
            }
            return bHasAnalyses;
        }
        public bool RunAnalyses(List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeyr.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangeid.ToString()
                || this.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbchangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType
                == SB1AnalyzerHelper.ANALYZER_TYPES.sbprogress1.ToString())
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

 
        public void InitSB1AnalysisProperties(SB1Stock ind,
            CalculatorParameters calcs)
        {
            ind.ErrorMessage = string.Empty;
            ind.InitTSB1BaseStockProperties();
            ind.CalcParameters = calcs;
            ind.SB11Stock = new SB101Stock();
            ind.SB12Stock = new SB102Stock();
        }

        public void CopySB1AnalysisProperties(SB1Stock ind,
            SB1Stock calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.CopyTSB1BaseStockProperties(calculator);
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            if (calculator.SB11Stock == null)
                calculator.SB11Stock = new SB101Stock();
            if (ind.SB11Stock == null)
                ind.SB11Stock = new SB101Stock();
            ind.SB11Stock.CopyTotalSB101StockProperties(calculator.SB11Stock);
            if (calculator.SB12Stock == null)
                calculator.SB12Stock = new SB102Stock();
            if (ind.SB12Stock == null)
                ind.SB12Stock = new SB102Stock();
            ind.SB12Stock.CopyTotalSB102StockProperties(calculator.SB12Stock);
        }

        public void SetSB1AnalysisProperties(SB1Stock ind,
            string attNameExtension, XElement calculator)
        {
            ind.SetTSB1BaseStockProperties(attNameExtension, calculator);
        }

        public void SetSB1AnalysisProperty(SB1Stock ind,
            string attName, string attValue)
        {
            ind.SetTotalSB1StockProperty(attName, attValue);
        }

        public string GetSB1AnalysisProperty(SB1Stock ind, string attName)
        {
            string sPropertyValue = string.Empty;
            sPropertyValue = ind.GetTSB1BaseStockProperty(attName);
            return sPropertyValue;
        }

        public virtual void SetSB1AnalysisAttributes(SB1Stock ind,
            string attNameExtension, XElement calculator)
        {
            ind.SetTSB1BaseStockAttributes(attNameExtension, calculator);
        }

        public async Task SetSB1AnalysisAttributesAsync(SB1Stock ind,
            string attNameExtension, XmlWriter writer)
        {
            await ind.SetTSB1BaseStockAttributesAsync(attNameExtension, writer);
        }
        public bool CopyTotalIndicatorsToSB1Stock(SB1Stock totStock, SB1Total1 subTotal)
        {
            //stats can't use CopyTotalToSB1Stock because they don't aggregate indicators when running totals

            //v178: can very likely use totStock.AddSubTotalToTotalStock(subTotal) instead of this method
            //but disadvantage might be larger file sizes with props that are needed in Stats, Change, Progress
            //they need variance which comes from separate indicators

            bool bHasTotals = false;
            int iStockCount = 0;
            iStockCount++;
            //label dependent aggregations
            //tps on store nets in stock 1
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1ScoreMUnit))
            {
                AddStockScore(totStock, subTotal.SB11Stock.TSB1ScoreN, subTotal.SB11Stock.TSB1ScoreM, subTotal.SB11Stock.TSB1ScoreMUnit,
                    subTotal.SB11Stock.TSB1ScoreLAmount, subTotal.SB11Stock.TSB1ScoreLUnit, subTotal.SB11Stock.TSB1ScoreUAmount, subTotal.SB11Stock.TSB1ScoreUUnit);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1ScoreMUnit))
            {
                AddStockScore(totStock, subTotal.SB12Stock.TSB1ScoreN, subTotal.SB12Stock.TSB1ScoreM, subTotal.SB12Stock.TSB1ScoreMUnit,
                    subTotal.SB12Stock.TSB1ScoreLAmount, subTotal.SB12Stock.TSB1ScoreLUnit, subTotal.SB12Stock.TSB1ScoreUAmount, subTotal.SB12Stock.TSB1ScoreUUnit);
            }
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label1))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N1, subTotal.SB11Stock.TSB1Label1, subTotal.SB11Stock.TSB1Name1,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount1, subTotal.SB11Stock.TSB1TMUnit1);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label1))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N1, subTotal.SB12Stock.TSB1Label1, subTotal.SB12Stock.TSB1Name1,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount1, subTotal.SB12Stock.TSB1TMUnit1);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label2))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N2, subTotal.SB11Stock.TSB1Label2, subTotal.SB11Stock.TSB1Name2,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount2, subTotal.SB11Stock.TSB1TMUnit2);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label2))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N2, subTotal.SB12Stock.TSB1Label2, subTotal.SB12Stock.TSB1Name2,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount2, subTotal.SB12Stock.TSB1TMUnit2);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label3))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N3, subTotal.SB11Stock.TSB1Label3, subTotal.SB11Stock.TSB1Name3,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount3, subTotal.SB11Stock.TSB1TMUnit3);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label3))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N3, subTotal.SB12Stock.TSB1Label3, subTotal.SB12Stock.TSB1Name3,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount3, subTotal.SB12Stock.TSB1TMUnit3);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label4))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N4, subTotal.SB11Stock.TSB1Label4, subTotal.SB11Stock.TSB1Name4,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount4, subTotal.SB11Stock.TSB1TMUnit4);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label4))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N4, subTotal.SB12Stock.TSB1Label4, subTotal.SB12Stock.TSB1Name4,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount4, subTotal.SB12Stock.TSB1TMUnit4);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label5))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N5, subTotal.SB11Stock.TSB1Label5, subTotal.SB11Stock.TSB1Name5,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount5, subTotal.SB11Stock.TSB1TMUnit5);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label5))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N5, subTotal.SB12Stock.TSB1Label5, subTotal.SB12Stock.TSB1Name5,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount5, subTotal.SB12Stock.TSB1TMUnit5);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label6))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N6, subTotal.SB11Stock.TSB1Label6, subTotal.SB11Stock.TSB1Name6,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount6, subTotal.SB11Stock.TSB1TMUnit6);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label6))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N6, subTotal.SB12Stock.TSB1Label6, subTotal.SB12Stock.TSB1Name6,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount6, subTotal.SB12Stock.TSB1TMUnit6);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label7))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N7, subTotal.SB11Stock.TSB1Label7, subTotal.SB11Stock.TSB1Name7,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount7, subTotal.SB11Stock.TSB1TMUnit7);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label7))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N7, subTotal.SB12Stock.TSB1Label7, subTotal.SB12Stock.TSB1Name7,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount7, subTotal.SB12Stock.TSB1TMUnit7);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label8))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N8, subTotal.SB11Stock.TSB1Label8, subTotal.SB11Stock.TSB1Name8,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount8, subTotal.SB11Stock.TSB1TMUnit8);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label8))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N8, subTotal.SB12Stock.TSB1Label8, subTotal.SB12Stock.TSB1Name8,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount8, subTotal.SB12Stock.TSB1TMUnit8);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label9))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N9, subTotal.SB11Stock.TSB1Label9, subTotal.SB11Stock.TSB1Name9,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount9, subTotal.SB11Stock.TSB1TMUnit9);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label9))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N9, subTotal.SB12Stock.TSB1Label9, subTotal.SB12Stock.TSB1Name9,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount9, subTotal.SB12Stock.TSB1TMUnit9);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label10))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N10, subTotal.SB11Stock.TSB1Label10, subTotal.SB11Stock.TSB1Name10,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount10, subTotal.SB11Stock.TSB1TMUnit10);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label10))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N10, subTotal.SB12Stock.TSB1Label10, subTotal.SB12Stock.TSB1Name10,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount10, subTotal.SB12Stock.TSB1TMUnit10);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label11))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N11, subTotal.SB11Stock.TSB1Label11, subTotal.SB11Stock.TSB1Name11,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount11, subTotal.SB11Stock.TSB1TMUnit11);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label11))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N11, subTotal.SB12Stock.TSB1Label11, subTotal.SB12Stock.TSB1Name11,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount11, subTotal.SB12Stock.TSB1TMUnit11);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label12))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N12, subTotal.SB11Stock.TSB1Label12, subTotal.SB11Stock.TSB1Name12,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount12, subTotal.SB11Stock.TSB1TMUnit12);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label12))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N12, subTotal.SB12Stock.TSB1Label12, subTotal.SB12Stock.TSB1Name12,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount12, subTotal.SB12Stock.TSB1TMUnit12);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label13))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N13, subTotal.SB11Stock.TSB1Label13, subTotal.SB11Stock.TSB1Name13,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount13, subTotal.SB11Stock.TSB1TMUnit13);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label13))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N13, subTotal.SB12Stock.TSB1Label13, subTotal.SB12Stock.TSB1Name13,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount13, subTotal.SB12Stock.TSB1TMUnit13);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label14))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N14, subTotal.SB11Stock.TSB1Label14, subTotal.SB11Stock.TSB1Name14,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount14, subTotal.SB11Stock.TSB1TMUnit14);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label14))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N14, subTotal.SB12Stock.TSB1Label14, subTotal.SB12Stock.TSB1Name14,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount14, subTotal.SB12Stock.TSB1TMUnit14);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label15))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N15, subTotal.SB11Stock.TSB1Label15, subTotal.SB11Stock.TSB1Name15,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount15, subTotal.SB11Stock.TSB1TMUnit15);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label15))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N15, subTotal.SB12Stock.TSB1Label15, subTotal.SB12Stock.TSB1Name15,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount15, subTotal.SB12Stock.TSB1TMUnit15);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label16))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N16, subTotal.SB11Stock.TSB1Label16, subTotal.SB11Stock.TSB1Name16,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount16, subTotal.SB11Stock.TSB1TMUnit16);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label16))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N16, subTotal.SB12Stock.TSB1Label16, subTotal.SB12Stock.TSB1Name16,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount16, subTotal.SB12Stock.TSB1TMUnit16);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label17))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N17, subTotal.SB11Stock.TSB1Label17, subTotal.SB11Stock.TSB1Name17,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount17, subTotal.SB11Stock.TSB1TMUnit17);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label17))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N17, subTotal.SB12Stock.TSB1Label17, subTotal.SB12Stock.TSB1Name17,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount17, subTotal.SB12Stock.TSB1TMUnit17);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label18))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N18, subTotal.SB11Stock.TSB1Label18, subTotal.SB11Stock.TSB1Name18,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount18, subTotal.SB11Stock.TSB1TMUnit18);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label18))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N18, subTotal.SB12Stock.TSB1Label18, subTotal.SB12Stock.TSB1Name18,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount18, subTotal.SB12Stock.TSB1TMUnit18);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label19))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N19, subTotal.SB11Stock.TSB1Label19, subTotal.SB11Stock.TSB1Name19,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount19, subTotal.SB11Stock.TSB1TMUnit19);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label19))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N19, subTotal.SB12Stock.TSB1Label19, subTotal.SB12Stock.TSB1Name19,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount19, subTotal.SB12Stock.TSB1TMUnit19);
            }
            iStockCount++;
            if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label20))
            {
                AddStock(totStock, subTotal.SB11Stock.TSB1N20, subTotal.SB11Stock.TSB1Label20, subTotal.SB11Stock.TSB1Name20,
                    iStockCount, subTotal.SB11Stock.TSB1TMAmount20, subTotal.SB11Stock.TSB1TMUnit20);
            }
            else if (!string.IsNullOrEmpty(subTotal.SB12Stock.TSB1Label20))
            {
                AddStock(totStock, subTotal.SB12Stock.TSB1N20, subTotal.SB12Stock.TSB1Label20, subTotal.SB12Stock.TSB1Name20,
                    iStockCount, subTotal.SB12Stock.TSB1TMAmount20, subTotal.SB12Stock.TSB1TMUnit20);
            }
            bHasTotals = true;
            return bHasTotals;
        }
        public bool CopyTotalToSB1Stock(SB1Stock totStock, SB1Total1 subTotal)
        {
            bool bHasCalculations = false;
            if (this.CalcParameters.UrisToAnalyze != null)
            {
                foreach (var sb in this.CalcParameters.UrisToAnalyze)
                {
                    if (sb == SB1Base.cSB1Label1)
                    {
                        //label dependent aggregations
                        //tps on store nets in stock 1
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label1))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label1);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label1);
                        }

                    }
                    else if (sb == SB1Base.cSB1Label2)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label2))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label2);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label2);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label3)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label3))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label3);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label3);
                        }

                    }
                    else if (sb == SB1Base.cSB1Label4)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label4))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label4);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label4);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label5)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label5))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label5);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label5);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label6)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label6))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label6);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label6);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label7)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label7))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label7);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label7);
                        }

                    }
                    else if (sb == SB1Base.cSB1Label8)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label8))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label8);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label8);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label9)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label9))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label9);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label9);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label10)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label10))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label10);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label10);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label11)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label11))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label11);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label11);
                        }

                    }
                    else if (sb == SB1Base.cSB1Label12)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label12))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label12);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label12);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label13)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label13))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label13);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label13);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label14)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label14))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label14);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label14);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label15)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label15))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label15);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label15);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label16)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label16))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label16);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label16);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label17)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label17))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label17);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label16);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label18)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label18))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label18);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label18);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label19)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label19))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label19);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label19);
                        }
                    }
                    else if (sb == SB1Base.cSB1Label20)
                    {
                        if (!string.IsNullOrEmpty(subTotal.SB11Stock.TSB1Label20))
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB11Stock, subTotal.SB11Stock.TSB1Label20);
                        }
                        else
                        {
                            totStock.AddSubTotalToTotalStock(subTotal.SB12Stock, subTotal.SB12Stock.TSB1Label20);
                        }
                    }
                }
            }
            return bHasCalculations;
        }
        public static bool AddStockScore(SB1Stock totStock, double stockN,
            double score, string scoreUnit, double scoreL, string scoreLUnit,
            double scoreU, string scoreUUnit)
        {
            //only a subset of the total props are used by the Stats, Change, and Progress 
            bool bHasStock = false;
            totStock.TSB1ScoreN = stockN;
            totStock.TSB1ScoreM = score;
            totStock.TSB1ScoreMUnit = scoreUnit;
            totStock.TSB1ScoreLAmount = scoreL;
            totStock.TSB1ScoreLUnit = scoreLUnit;
            totStock.TSB1ScoreUAmount = scoreU;
            totStock.TSB1ScoreUUnit = scoreUUnit;
            bHasStock = true;
            return bHasStock;
        }
        public static bool AddStock(SB1Stock totStock, double stockN,
            string stockLabel, string stockName, int stockCount,
            double q, string qUnit)
        {
            bool bHasStock = false;
            //only a subset of the total props are used by the Stats, Change, and Progress 
            if (stockCount == 1)
            {
                totStock.TSB1N1 = stockN;
                totStock.TSB1Label1 = stockLabel;
                totStock.TSB1Name1 = stockName;
                totStock.TSB1TMAmount1 = q;
                totStock.TSB1TMUnit1 = qUnit;
            }
            else if (stockCount == 2)
            {
                totStock.TSB1N2 = stockN;
                totStock.TSB1Label2 = stockLabel;
                totStock.TSB1Name2 = stockName;
                totStock.TSB1TMAmount2 = q;
                totStock.TSB1TMUnit2 = qUnit;
            }
            else if (stockCount == 3)
            {
                totStock.TSB1N3 = stockN;
                totStock.TSB1Label3 = stockLabel;
                totStock.TSB1Name3 = stockName;
                totStock.TSB1TMAmount3 = q;
                totStock.TSB1TMUnit3 = qUnit;
            }
            else if (stockCount == 4)
            {
                totStock.TSB1N4 = stockN;
                totStock.TSB1Label4 = stockLabel;
                totStock.TSB1Name4 = stockName;
                totStock.TSB1TMAmount4 = q;
                totStock.TSB1TMUnit4 = qUnit;
            }
            else if (stockCount == 5)
            {
                totStock.TSB1N5 = stockN;
                totStock.TSB1Label5 = stockLabel;
                totStock.TSB1Name5 = stockName;
                totStock.TSB1TMAmount5 = q;
                totStock.TSB1TMUnit5 = qUnit;
            }
            else if (stockCount == 6)
            {
                totStock.TSB1N6 = stockN;
                totStock.TSB1Label6 = stockLabel;
                totStock.TSB1Name6 = stockName;
                totStock.TSB1TMAmount6 = q;
                totStock.TSB1TMUnit6 = qUnit;
            }
            else if (stockCount == 7)
            {
                totStock.TSB1N7 = stockN;
                totStock.TSB1Label7 = stockLabel;
                totStock.TSB1Name7 = stockName;
                totStock.TSB1TMAmount7 = q;
                totStock.TSB1TMUnit7 = qUnit;
            }
            else if (stockCount == 8)
            {
                totStock.TSB1N8 = stockN;
                totStock.TSB1Label8 = stockLabel;
                totStock.TSB1Name8 = stockName;
                totStock.TSB1TMAmount8 = q;
                totStock.TSB1TMUnit8 = qUnit;
            }
            else if (stockCount == 9)
            {
                totStock.TSB1N9 = stockN;
                totStock.TSB1Label9 = stockLabel;
                totStock.TSB1Name9 = stockName;
                totStock.TSB1TMAmount9 = q;
                totStock.TSB1TMUnit9 = qUnit;
            }
            else if (stockCount == 10)
            {
                totStock.TSB1N10 = stockN;
                totStock.TSB1Label10 = stockLabel;
                totStock.TSB1Name10 = stockName;
                totStock.TSB1TMAmount10 = q;
                totStock.TSB1TMUnit10 = qUnit;
            }
            else if (stockCount == 11)
            {
                totStock.TSB1N11 = stockN;
                totStock.TSB1Label11 = stockLabel;
                totStock.TSB1Name11 = stockName;
                totStock.TSB1TMAmount11 = q;
                totStock.TSB1TMUnit11 = qUnit;
            }
            else if (stockCount == 12)
            {
                totStock.TSB1N12 = stockN;
                totStock.TSB1Label12 = stockLabel;
                totStock.TSB1Name12 = stockName;
                totStock.TSB1TMAmount12 = q;
                totStock.TSB1TMUnit12 = qUnit;
            }
            else if (stockCount == 13)
            {
                totStock.TSB1N13 = stockN;
                totStock.TSB1Label13 = stockLabel;
                totStock.TSB1Name13 = stockName;
                totStock.TSB1TMAmount13 = q;
                totStock.TSB1TMUnit13 = qUnit;
            }
            else if (stockCount == 14)
            {
                totStock.TSB1N14 = stockN;
                totStock.TSB1Label14 = stockLabel;
                totStock.TSB1Name14 = stockName;
                totStock.TSB1TMAmount14 = q;
                totStock.TSB1TMUnit14 = qUnit;
            }
            else if (stockCount == 15)
            {
                totStock.TSB1N15 = stockN;
                totStock.TSB1Label15 = stockLabel;
                totStock.TSB1Name15 = stockName;
                totStock.TSB1TMAmount15 = q;
                totStock.TSB1TMUnit15 = qUnit;
            }
            else if (stockCount == 16)
            {
                totStock.TSB1N16 = stockN;
                totStock.TSB1Label16 = stockLabel;
                totStock.TSB1Name16 = stockName;
                totStock.TSB1TMAmount16 = q;
                totStock.TSB1TMUnit16 = qUnit;
            }
            else if (stockCount == 17)
            {
                totStock.TSB1N17 = stockN;
                totStock.TSB1Label17 = stockLabel;
                totStock.TSB1Name17 = stockName;
                totStock.TSB1TMAmount17 = q;
                totStock.TSB1TMUnit17 = qUnit;
            }
            else if (stockCount == 18)
            {
                totStock.TSB1N18 = stockN;
                totStock.TSB1Label18 = stockLabel;
                totStock.TSB1Name18 = stockName;
                totStock.TSB1TMAmount18 = q;
                totStock.TSB1TMUnit18 = qUnit;
            }
            else if (stockCount == 19)
            {
                totStock.TSB1N19 = stockN;
                totStock.TSB1Label19 = stockLabel;
                totStock.TSB1Name19 = stockName;
                totStock.TSB1TMAmount19 = q;
                totStock.TSB1TMUnit19 = qUnit;
            }
            else if (stockCount == 20)
            {
                totStock.TSB1N20 = stockN;
                totStock.TSB1Label20 = stockLabel;
                totStock.TSB1Name20 = stockName;
                totStock.TSB1TMAmount20 = q;
                totStock.TSB1TMUnit20 = qUnit;
            }
            return bHasStock;
        }
        public static bool AddSubTotalToTotalStock(SB1Stock totStock, double multiplier,
            SB1Stock subTotal)
        {
            bool bHasTotals = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            bHasTotals = totStock.AddSubTotalToTotalStock(subTotal);
            return bHasTotals;
        }
        public static bool CopySubTotalToTotalStock(SB1Stock totStock, double multiplier,
             SB1Stock subTotal)
        {
            bool bHasTotals = false;
            bHasTotals = totStock.CopySubTotalToTotalStock(subTotal);
            return bHasTotals;
        }
        public static void ChangeSubTotalByMultipliers(SB1Stock subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
            subTotal.ChangeSubTotalByMultipliers(multiplier);
        }
    }
}