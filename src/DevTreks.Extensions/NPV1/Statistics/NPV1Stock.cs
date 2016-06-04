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
    ///             NPV1Stock.Analyzer
    ///             Important to use separate agg objects because
    ///             some analyses are sequential and use the results of previous analyses.
    ///             The class keeps track of npv quantitative totals.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTEs:       1. Analyzer should only be run at budget.TimePeriod or otherElement.Group level. 
    ///</summary>
    public class NPV1Stock : CostBenefitCalculator
    {
        //calls the base-class version, and initializes the base class properties.
        public NPV1Stock(CalculatorParameters calcParams, string analyzerType)
            : base()
        {
            if (calcParams != null)
            {
                this.CalcParameters = new CalculatorParameters(calcParams);
            }
            this.AnalyzerType = analyzerType;
            //subprice object
            InitTotalNPV1StocksProperties();
        }
        //copy constructor
        public NPV1Stock(NPV1Stock calculator)
        {
            CopyTotalNPV1StocksProperties(calculator);
        }
        //inheritors
        public NPV1Stock()
            : base()
        {
        }
        public CalculatorParameters CalcParameters { get; set; }
        //changes and progress use collections of results
        public List<NPV1Stock> Stocks { get; set; }
        //totals analyses (see if any of the others need this to be a list)
        public NPV1Total1 Total1 { get; set; }
        //statistical analyses
        public NPV1Stat1 Stat1 { get; set; }
        //change analyses
        public NPV1Change1 Change1 { get; set; }
        //progress analyses
        public NPV1Progress1 Progress1 { get; set; }

        public virtual void InitTotalNPV1StocksProperties()
        {
            if (this.Total1 == null)
            {
                this.Total1 = new NPV1Total1();
            }
            if (this.Stat1 == null)
            {
                this.Stat1 = new NPV1Stat1();
            }
            if (this.Change1 == null)
            {
                this.Change1 = new NPV1Change1();
            }
            if (this.Progress1 == null)
            {
                this.Progress1 = new NPV1Progress1();
            }
            if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                //this.Stat1 = new NPV1Stat1();
                this.Stat1.InitTotalNPV1Stat1Properties(this.Stat1);
            }
            else if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                //this.Change1 = new NPV1Change1();
                this.Change1.InitTotalNPV1Change1Properties(this.Change1);
            }
            else if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                //this.Progress1 = new NPV1Progress1();
                this.Progress1.InitTotalNPV1Progress1Properties(this.Progress1);
            }
            else
            {
                //default are total indicators
                //this.Total1 = new NPV1Total1();
                this.Total1.InitTotalNPV1Total1Properties(this.Total1);
            }
        }
        public virtual void InitTotalNPV1StocksProperties(CostBenefitCalculator baseElement)
        {
            //this is called during the initial calculator collections
            //it adds those initial calcs to correct agg stock
            //base calculator holds totals
            //each agg has to set CalcParameters during object construction
            if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                this.Stat1 = new NPV1Stat1();
                this.Stat1.CopyCalculatorProperties(baseElement);
                this.Stat1.CopyTotalCostsProperties(baseElement);
                this.Stat1.CopyTotalBenefitsProperties(baseElement);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                this.Change1 = new NPV1Change1();
                this.Change1.CopyCalculatorProperties(baseElement);
                this.Change1.CopyTotalCostsProperties(baseElement);
                this.Change1.CopyTotalBenefitsProperties(baseElement);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                this.Progress1 = new NPV1Progress1();
                this.Progress1.CopyCalculatorProperties(baseElement);
                this.Progress1.CopyTotalCostsProperties(baseElement);
                this.Progress1.CopyTotalBenefitsProperties(baseElement);
            }
            else
            {
                this.Total1 = new NPV1Total1();
                this.Total1.CopyCalculatorProperties(baseElement);
                this.Total1.CopyTotalCostsProperties(baseElement);
                this.Total1.CopyTotalBenefitsProperties(baseElement);
            }
        }
        public virtual void InitTotalNPV1StocksProperties(NPV1Stock calculator)
        {
            //this is called during the initial calculator collections
            //it adds those initial calcs to correct agg stock
            //base calculator holds totals
            if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                this.Stat1 = new NPV1Stat1();
                //each analysis stores data according to analyzertype and nowehere else
                if (calculator.Stat1 != null)
                {
                    this.Stat1.CalcParameters = new CalculatorParameters(calculator.Stat1.CalcParameters);
                    this.Stat1.CopyCalculatorProperties(calculator.Stat1);
                    this.Stat1.CopyTotalCostsProperties(calculator.Stat1);
                    this.Stat1.CopyTotalBenefitsProperties(calculator.Stat1);
                }
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                this.Change1 = new NPV1Change1();
                if (calculator.Change1 != null)
                {
                    this.Change1.CalcParameters = new CalculatorParameters(calculator.Change1.CalcParameters);
                    this.Change1.CopyCalculatorProperties(calculator.Change1);
                    this.Change1.CopyTotalCostsProperties(calculator.Change1);
                    this.Change1.CopyTotalBenefitsProperties(calculator.Change1);
                }
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                this.Progress1 = new NPV1Progress1();
                if (calculator.Progress1 != null)
                {
                    this.Progress1.CalcParameters = new CalculatorParameters(calculator.Progress1.CalcParameters);
                    this.Progress1.CopyCalculatorProperties(calculator.Progress1);
                    this.Progress1.CopyTotalCostsProperties(calculator.Progress1);
                    this.Progress1.CopyTotalBenefitsProperties(calculator.Progress1);
                }
            }
            else
            {
                this.Total1 = new NPV1Total1();
                if (calculator.Total1 != null)
                {
                    this.Total1.CalcParameters = new CalculatorParameters(calculator.Total1.CalcParameters);
                    this.Total1.CopyCalculatorProperties(calculator.Total1);
                    this.Total1.CopyTotalCostsProperties(calculator.Total1);
                    this.Total1.CopyTotalBenefitsProperties(calculator.Total1);
                }
            }
        }
        public virtual void CopyTotalNPV1StocksProperties(
            NPV1Stock calculator)
        {
            //this is called after the initial calculator collections have been built
            //it adds those initial calcs to correct agg stock
            this.CopyCalculatorProperties(calculator);
            this.CalcParameters = calculator.CalcParameters;
            //base calculator holds totals (always inits with calculator.Total)
            if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                this.Stat1 = new NPV1Stat1();
                this.Stat1.CopyTotalNPV1Stat1Properties(
                    this.Stat1, calculator.Stat1);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                this.Change1 = new NPV1Change1();
                this.Change1.CopyTotalNPV1Change1Properties(
                    this.Change1, calculator.Change1);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                this.Progress1 = new NPV1Progress1();
                this.Progress1.CopyTotalNPV1Progress1Properties(
                    this.Progress1, calculator.Progress1);
            }
            else
            {
                //calculator stores starting npv totals in calculator.Total1
                this.Total1 = new NPV1Total1();
                this.Total1.CopyTotalNPV1Total1Properties(
                    this.Total1, calculator.Total1);
            }
        }
        public virtual void SetTotalNPV1StocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            string sAttNameExtension = string.Empty;
            if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                if (this.Stat1 == null)
                {
                    this.Stat1 = new NPV1Stat1();
                }
                this.Stat1.SetTotalNPV1Stat1Properties(
                    this.Stat1, sAttNameExtension, calculator);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                if (this.Change1 == null)
                {
                    this.Change1 = new NPV1Change1();
                }
                this.Change1.SetTotalNPV1Change1Properties(
                    this.Change1, sAttNameExtension, calculator);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                if (this.Progress1 == null)
                {
                    this.Progress1 = new NPV1Progress1();
                }
                this.Progress1.SetTotalNPV1Progress1Properties(
                    this.Progress1, sAttNameExtension, calculator);
            }
            else
            {
                if (this.Total1 == null)
                {
                    this.Total1 = new NPV1Total1();
                }
                this.Total1.SetTotalNPV1Total1Properties(
                    this.Total1, sAttNameExtension, calculator);
            }
        }
        
        public virtual void SetDescendantNPV1StockProperties(XElement currentCalculationsElement)
        {
            //abbreviated properties for descendant stock totals
            this.SetCalculatorProperties(currentCalculationsElement);
            //no set shared object props, they already have props set
            //don't SetTotalIndicator1StocksProperties because the descendants have their own stock totals
        }
        public virtual void SetTotalNPV1StocksProperty(string attName,
               string attValue, int colIndex)
        {
            if (this.AnalyzerType
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                this.Stat1.SetTotalNPV1Stat1Property(
                    this.Stat1, attName, attValue);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                this.Change1.SetTotalNPV1Change1Property(
                    this.Change1, attName, attValue);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                this.Progress1.SetTotalNPV1Progress1Property(
                    this.Progress1, attName, attValue);
            }
            else
            {
                this.Total1.SetTotalNPV1Total1Property(
                    this.Total1, attName, attValue);
            }
        }
        public virtual string GetTotalNPV1StocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                sPropertyValue = this.Stat1.GetTotalNPV1Stat1Property(
                   this.Stat1, attName);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                sPropertyValue = this.Change1.GetTotalNPV1Change1Property(
                  this.Change1, attName);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                sPropertyValue = this.Progress1.GetTotalNPV1Progress1Property(
                  this.Progress1, attName);
            }
            else
            {
                sPropertyValue = this.Total1.GetTotalNPV1Total1Property(
                    this.Total1, attName);
            }
            return sPropertyValue;
        }
        public void SetDescendantNPV1StockAttributes(string attNameExt, ref XmlWriter writer)
        {
            //abbreviated list for decendant stock totals
            if (writer != null)
            {
                //set the base analyzer
                int iId = this.Id;
                this.SetCalculatorAttributes(attNameExt, ref writer);
                //set the calculations
                this.SetTotalNPV1StocksAttributes(attNameExt, ref writer);
            }
        }
        public void SetDescendantNPV1StockInputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XmlWriter writer)
        {
            //the npvtotal1 analyzer adds its totals to the base lcc1calculator linked view
            //less duplication and easier to display and extend
            if (writer != null)
            {
                //set the base analyzer
                this.SetCalculatorAttributes(attNameExt, ref writer);
                bool bHasCalcs = false;
                //don't need both subps and subpstock
                if (!bHasCalcs)
                {
                    this.SetTotalNPV1StocksAttributes(attNameExt, ref writer);
                }
            }
        }
        public void SetDescendantNPV1StockOutputAttributes(string attNameExt, CalculatorParameters calcParams,
            ref XmlWriter writer)
        {
            //the npvtotal1 analyzer adds its totals to the base lcb1calculator linked view
            //less duplication and easier to extend
            if (writer != null)
            {
                if (this.AnalyzerType
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvtotal1.ToString()
                    || this.AnalyzerType
                    == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString()
                    || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                    || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                    || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString()
                    || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                {
                    this.SetCalculatorAttributes(attNameExt, ref writer);
                    bool bHasCalcs = false;
                    //don't need both subps and subpstock
                    if (!bHasCalcs)
                    {
                        //set the analysis
                        this.SetTotalNPV1StocksAttributes(attNameExt, ref writer);
                    }
                }
            }
        }
       
        public virtual void SetTotalNPV1StocksAttributes(string attNameExt, ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                if (this.Stat1 != null)
                {
                    this.Stat1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    this.Stat1.CalcParameters.SubApplicationType = this.CalcParameters.SubApplicationType;
                    //set the analysis
                    this.Stat1.SetTotalNPV1Stat1Attributes(
                        this.Stat1, attNameExt, ref writer);
                }
            }
            else if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                if (this.Change1 != null)
                {
                    this.Change1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    this.Change1.CalcParameters.SubApplicationType = this.CalcParameters.SubApplicationType;
                    //set the analysis
                    this.Change1.SetTotalNPV1Change1Attributes(
                        this.Change1, attNameExt, ref writer);
                }
            }
            else if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
            {
                if (this.Progress1 != null)
                {
                    this.Progress1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    this.Progress1.CalcParameters.SubApplicationType = this.CalcParameters.SubApplicationType;
                    //set the analysis
                    this.Progress1.SetTotalNPV1Progress1Attributes(
                        this.Progress1, attNameExt, ref writer);
                }
            }
            else
            {
                if (this.Total1 != null)
                {
                    this.Total1.CalcParameters.CurrentElementNodeName
                        = this.CalcParameters.CurrentElementNodeName;
                    this.Total1.CalcParameters.SubApplicationType = this.CalcParameters.SubApplicationType;
                    //set the analysis
                    this.Total1.SetTotalNPV1Total1Attributes(this.Total1, attNameExt, ref writer);
                }
            }
        }
        //note this must only copy the R Props (TRAmount) not TRAM, because the NPV calculator originally ran that calc
        public virtual void CopyTotalNPV1RPropertiesToCalc(Calculator1 calc)
        {
            if (calc == null)
                return;
            if (calc.GetType().Equals(this.GetType()))
            {
                NPV1Stock toCalcStock = (NPV1Stock)calc;
                if (toCalcStock != null)
                {
                    if (this.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
                    {
                        if (this.Stat1 == null)
                            return;
                        if (toCalcStock.Stat1 == null)
                        {
                            toCalcStock.Stat1 = new NPV1Stat1();
                        }
                        toCalcStock.Stat1.CopyTotalNPV1Stat1RProperties(this.Stat1);
                    }
                    else if (this.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                        || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                        || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
                    {
                        if (this.Change1 == null)
                            return;
                        if (toCalcStock.Change1 == null)
                        {
                            toCalcStock.Change1 = new NPV1Change1();
                        }
                        toCalcStock.Change1.CopyTotalNPV1Change1RProperties(this.Change1);
                    }
                    else if (this.AnalyzerType
                        == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
                    {
                        if (this.Progress1 == null)
                            return;
                        if (toCalcStock.Progress1 == null)
                        {
                            toCalcStock.Progress1 = new NPV1Progress1();
                        }
                        toCalcStock.Progress1.CopyTotalNPV1Progress1RProperties(this.Progress1);
                    }
                    else
                    {
                        if (this.Total1 == null)
                            return;
                        if (toCalcStock.Total1 == null)
                        {
                            toCalcStock.Total1 = new NPV1Total1();
                        }
                        toCalcStock.Total1.CopyTotalNPV1Total1RProperties(this.Total1);
                    }
                }
            }
        }
        //run the analyses
        public bool RunAnalyses()
        {
            bool bHasAnalyses = false;
            if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this);
            }
            else if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvstat1.ToString())
            {
                bHasAnalyses = this.Stat1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeyr.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangeid.ToString()
                || this.AnalyzerType == NPV1AnalyzerHelper.ANALYZER_TYPES.npvchangealt.ToString())
            {
                bHasAnalyses = this.Change1.RunAnalyses(this, calcs);
            }
            else if (this.AnalyzerType
                == NPV1AnalyzerHelper.ANALYZER_TYPES.npvprogress1.ToString())
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