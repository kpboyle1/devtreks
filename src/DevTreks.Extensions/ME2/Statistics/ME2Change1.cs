using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:	    Typical Object model: 
    ///             ME2Stock.Stocks.ME2Change1.Stocks.ME2Change1.ME2Indicators
    ///             ME2Stock.Stocks is a collection of ME2Stocks (unique observations)
    ///             Each member of ME2Stocks holds an analyzer stock (Change1)
    ///             Each analyzer stock (Change1) holds a collection of Change1s
    ///             The class tracks annual changes in totals.
    ///Author:		www.devtreks.org
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class ME2Change1 : ME2Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public ME2Change1(CalculatorParameters calcParams)
            : base(calcParams)
        {
            //subprice object
            InitTotalME2Change1Properties(this);
        }
        //note that display properties, such as name, description, unit are in 
        //parent ME2Stock calculator properties
        //the total properties come from ME2IndicatorStock
        //total change from last time period
        public double TotalME2MAmountChange { get; set; }
        //percent change from last time period
        public double TotalME2MPercentChange { get; set; }
        //total change from base lcc or lcb calculator
        public double TotalME2MBaseChange { get; set; }
        //percent change from base lcc or lcb calculator
        public double TotalME2MBasePercentChange { get; set; }

        public double TotalME2LAmountChange { get; set; }
        public double TotalME2LPercentChange { get; set; }
        public double TotalME2LBaseChange { get; set; }
        public double TotalME2LBasePercentChange { get; set; }

        public double TotalME2UAmountChange { get; set; }
        public double TotalME2UPercentChange { get; set; }
        public double TotalME2UBaseChange { get; set; }
        public double TotalME2UBasePercentChange { get; set; }

        private const string cTotalME2MAmountChange = "TME2MAmountChange";
        private const string cTotalME2MPercentChange = "TME2MPercentChange";
        private const string cTotalME2MBaseChange = "TME2MBaseChange";
        private const string cTotalME2MBasePercentChange = "TME2MBasePercentChange";

        private const string cTotalME2LAmountChange = "TME2LAmountChange";
        private const string cTotalME2LPercentChange = "TME2LPercentChange";
        private const string cTotalME2LBaseChange = "TME2LBaseChange";
        private const string cTotalME2LBasePercentChange = "TME2LBasePercentChange";

        private const string cTotalME2UAmountChange = "TME2UAmountChange";
        private const string cTotalME2UPercentChange = "TME2UPercentChange";
        private const string cTotalME2UBaseChange = "TME2UBaseChange";
        private const string cTotalME2UBasePercentChange = "TME2UBasePercentChange";

        public void InitTotalME2Change1Properties(ME2Change1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.CalcParameters = new CalculatorParameters();
            InitTotalME2IndicatorStockProperties(ind);
            //set the change
            ind.TotalME2MAmountChange = 0;
            ind.TotalME2MPercentChange = 0;
            ind.TotalME2MBaseChange = 0;
            ind.TotalME2MBasePercentChange = 0;

            ind.TotalME2LAmountChange = 0;
            ind.TotalME2LPercentChange = 0;
            ind.TotalME2LBaseChange = 0;
            ind.TotalME2LBasePercentChange = 0;

            ind.TotalME2UAmountChange = 0;
            ind.TotalME2UPercentChange = 0;
            ind.TotalME2UBaseChange = 0;
            ind.TotalME2UBasePercentChange = 0;
        }

        public void CopyTotalME2Change1Properties(ME2Change1 calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            //copy the initial totals and the indicators (used in RunAnalyses)
            CopyTotalME2IndicatorStockProperties(this, calculator);
            //copy the stats properties
            CopyME2Change1Properties(this, calculator);
            //copy the calculator.ME2Stocks collection
            if (this.Stocks == null)
                this.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //copy the calculated totals and the indicators
            //obsStock.Change1.Stocks holds a collection of change1s
            if (calculator.Stocks != null)
            {
                foreach (ME2Stock statStock in calculator.Stocks)
                {
                    ME2Change1 stat = new ME2Change1(this.CalcParameters);
                    if (statStock.GetType().Equals(stat.GetType()))
                    {
                        stat = (ME2Change1)statStock;
                        if (stat != null)
                        {
                            ME2Change1 newStat = new ME2Change1(this.CalcParameters);
                            //copy the totals and the indicators
                            CopyTotalME2IndicatorStockProperties(newStat, stat);
                            //copy the stats properties
                            CopyME2Change1Properties(newStat, stat);
                            //this refers to me2Stock.Stocks[x].Change1
                            this.Stocks.Add(newStat);
                        }
                    }
                }
            }
        }
        
        public void CopyME2Change1Properties(ME2Change1 ind,
            ME2Change1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalME2MAmountChange = calculator.TotalME2MAmountChange;
            ind.TotalME2MPercentChange = calculator.TotalME2MPercentChange;
            ind.TotalME2MBaseChange = calculator.TotalME2MBaseChange;
            ind.TotalME2MBasePercentChange = calculator.TotalME2MBasePercentChange;

            ind.TotalME2LAmountChange = calculator.TotalME2LAmountChange;
            ind.TotalME2LPercentChange = calculator.TotalME2LPercentChange;
            ind.TotalME2LBaseChange = calculator.TotalME2LBaseChange;
            ind.TotalME2LBasePercentChange = calculator.TotalME2LBasePercentChange;

            ind.TotalME2UAmountChange = calculator.TotalME2UAmountChange;
            ind.TotalME2UPercentChange = calculator.TotalME2UPercentChange;
            ind.TotalME2UBaseChange = calculator.TotalME2UBaseChange;
            ind.TotalME2UBasePercentChange = calculator.TotalME2UBasePercentChange;
        }
        public void CopyTotalME2Change1Properties(ME2Change1 ind,
            ME2Total1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            //copy the totals and the indicators
            CopyTotalME2IndicatorStockProperties(ind, calculator);
            //copy the calculator.ME2Stocks collection
            if (ind.Stocks == null)
                ind.Stocks = new List<ME2Stock>();
            if (calculator.Stocks == null)
                calculator.Stocks = new List<ME2Stock>();
            //calculator.Stocks is a collection of Total1s
            foreach (ME2Stock me2stock in calculator.Stocks)
            {
                ME2Change1 newChange = new ME2Change1(this.CalcParameters);
                //copy the totals and the indicators
                CopyTotalME2IndicatorStockProperties(newChange, me2stock);
                if (newChange != null)
                {
                    ind.Stocks.Add(newChange);
                }
            }
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
        }
        public void SetTotalME2Change1Properties(ME2Change1 ind,
            string attNameExtension, XElement calculator)
        {
            //stats always based on indicators
            ind.SetTotalME2IndicatorStockProperties(ind, attNameExtension, calculator);

            ind.TotalME2MAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MAmountChange, attNameExtension));
            ind.TotalME2MPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MPercentChange, attNameExtension));
            ind.TotalME2MBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MBaseChange, attNameExtension));
            ind.TotalME2MBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2MBasePercentChange, attNameExtension));

            ind.TotalME2LAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LAmountChange, attNameExtension));
            ind.TotalME2LPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LPercentChange, attNameExtension));
            ind.TotalME2LBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LBaseChange, attNameExtension));
            ind.TotalME2LBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2LBasePercentChange, attNameExtension));

            ind.TotalME2UAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UAmountChange, attNameExtension));
            ind.TotalME2UPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UPercentChange, attNameExtension));
            ind.TotalME2UBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UBaseChange, attNameExtension));
            ind.TotalME2UBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalME2UBasePercentChange, attNameExtension));
        }

        public void SetTotalME2Change1Property(ME2Change1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalME2MAmountChange:
                    ind.TotalME2MAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2MPercentChange:
                    ind.TotalME2MPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2MBaseChange:
                    ind.TotalME2MBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2MBasePercentChange:
                    ind.TotalME2MBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LAmountChange:
                    ind.TotalME2LAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LPercentChange:
                    ind.TotalME2LPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LBaseChange:
                    ind.TotalME2LBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2LBasePercentChange:
                    ind.TotalME2LBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UAmountChange:
                    ind.TotalME2UAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UPercentChange:
                    ind.TotalME2UPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UBaseChange:
                    ind.TotalME2UBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalME2UBasePercentChange:
                    ind.TotalME2UBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalME2Change1Property(ME2Change1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalME2MAmountChange:
                    sPropertyValue = ind.TotalME2MAmountChange.ToString();
                    break;
                case cTotalME2MPercentChange:
                    sPropertyValue = ind.TotalME2MPercentChange.ToString();
                    break;
                case cTotalME2MBaseChange:
                    sPropertyValue = ind.TotalME2MBaseChange.ToString();
                    break;
                case cTotalME2MBasePercentChange:
                    sPropertyValue = ind.TotalME2MBasePercentChange.ToString();
                    break;
                case cTotalME2LAmountChange:
                    sPropertyValue = ind.TotalME2LAmountChange.ToString();
                    break;
                case cTotalME2LPercentChange:
                    sPropertyValue = ind.TotalME2LPercentChange.ToString();
                    break;
                case cTotalME2LBaseChange:
                    sPropertyValue = ind.TotalME2LBaseChange.ToString();
                    break;
                case cTotalME2LBasePercentChange:
                    sPropertyValue = ind.TotalME2LBasePercentChange.ToString();
                    break;
                case cTotalME2UAmountChange:
                    sPropertyValue = ind.TotalME2UAmountChange.ToString();
                    break;
                case cTotalME2UPercentChange:
                    sPropertyValue = ind.TotalME2UPercentChange.ToString();
                    break;
                case cTotalME2UBaseChange:
                    sPropertyValue = ind.TotalME2UBaseChange.ToString();
                    break;
                case cTotalME2UBasePercentChange:
                    sPropertyValue = ind.TotalME2UBasePercentChange.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalME2Change1Attributes(string attNameExt,
            ref XmlWriter writer)
        {
            if (this.Stocks != null)
            {
                int i = 0;
                string sAttNameExtension = string.Empty;
                foreach (ME2Change1 stat in this.Stocks)
                {
                    if (stat != null)
                    {
                        sAttNameExtension = string.Concat(i.ToString(), attNameExt);
                        //this runs in ME2IndicatorStock object
                        SetTotalME2IndicatorStockAttributes(stat, sAttNameExtension, ref writer);
                        SetTotalME2Change1Attributes(stat, sAttNameExtension, ref writer);
                    }
                    i++;
                }
            }
        }
        public void SetTotalME2Change1Attributes(ME2Change1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(cTotalME2MAmountChange, attNameExtension), ind.TotalME2MAmountChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2MPercentChange, attNameExtension), ind.TotalME2MPercentChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2MBaseChange, attNameExtension), ind.TotalME2MBaseChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2MBasePercentChange, attNameExtension), ind.TotalME2MBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(cTotalME2LAmountChange, attNameExtension), ind.TotalME2LAmountChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LPercentChange, attNameExtension), ind.TotalME2LPercentChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LBaseChange, attNameExtension), ind.TotalME2LBaseChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2LBasePercentChange, attNameExtension), ind.TotalME2LBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(cTotalME2UAmountChange, attNameExtension), ind.TotalME2UAmountChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UPercentChange, attNameExtension), ind.TotalME2UPercentChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UBaseChange, attNameExtension), ind.TotalME2UBaseChange.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTotalME2UBasePercentChange, attNameExtension), ind.TotalME2UBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));
        }
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
                                if (obsStock.Change1 != null)
                                {
                                    obsStock.Total1 = new ME2Total1(this.CalcParameters);
                                    ////204 allowed more flexibility with indicators 
                                    ////ancestors and siblings can have multiple inds with different labels and 1 stock for each ind collection
                                    if (obsStock.Change1.Stocks != null)
                                    {
                                        if (obsStock.Change1.Stocks.Count > 0)
                                        {
                                            int k = 0;
                                            foreach (var addedstock in obsStock.Change1.Stocks)
                                            {
                                                if (addedstock.ME2Indicators.Count > 0)
                                                {
                                                    if (k == 0)
                                                    {
                                                        //this resets indicator list
                                                        obsStock.Change1.CopyME2IndicatorsProperties(addedstock);
                                                    }
                                                    else
                                                    {
                                                        //when totals are run it will use ind.Label to add total to proper stock
                                                        obsStock.Change1.AddME2IndicatorsProperties(addedstock);
                                                    }
                                                }
                                                k++;
                                            }
                                        }
                                    }
                                    if (obsStock.Change1.ME2Indicators != null)
                                    {
                                        if (obsStock.Change1.ME2Indicators.Count > 0)
                                        {
                                            obsStock.Total1.CopyME2IndicatorsProperties(obsStock.Change1);
                                            //id comes from original calc
                                            obsStock.Total1.CopyCalculatorProperties(stock);
                                            //clear the initial indicators
                                            obsStock.Change1.ME2Indicators = new List<ME2Indicator>();
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
                bHasTotals = SetChangesAnalysis(me2Stock);
            }
            return bHasTotals;
        }

        private bool SetChangesAnalysis(ME2Stock me2Stock)
        {
            bool bHasTotalChanges = false;
            if (me2Stock.Stocks != null)
            {
                //set the total observations total
                bool bHasCurrents = me2Stock.Stocks
                    .Any(c => c.ChangeType == Calculator1.CHANGE_TYPES.current.ToString());
                //if any changestock has this property, it's trying to compare antecedents, rather than siblings
                if (bHasCurrents)
                {
                    //budgets uses antecendent, rather than sibling, comparators
                    bHasTotalChanges = SetBudgetChanges(me2Stock);
                }
                else
                {
                    //set change numbers
                    bHasTotalChanges = SetChanges(me2Stock);
                }
            }
            return bHasTotalChanges;
        }

        private static bool SetBudgetChanges(ME2Stock me2Stock)
        {
            bool bHasChanges = false;
            //replace list of totalstocks with list of changestocks
            List<ME2Stock> obsStocks = new List<ME2Stock>();
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            int i = 0;
            foreach (ME2Stock observation in me2Stock.Stocks)
            {
                //actual totals are contained in observation.Total1.Stocks (and Stocks are Total1s)
                if (observation.Total1 != null
                    && observation.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //unlike totals, obsstock needs obs.CopyCalcs so it matches with baseelement
                    ME2Stock observationStock = new ME2Stock(observation.CalcParameters,
                        me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                    //need the base el id
                    observationStock.CopyCalculatorProperties(observation);
                    //where the stats go
                    observationStock.Change1 = new ME2Change1(observation.CalcParameters);
                    observationStock.Change1.CalcParameters = new CalculatorParameters(me2Stock.CalcParameters);
                    observationStock.Change1.CopyCalculatorProperties(observation);
                    foreach (ME2Total1 total in observation.Total1.Stocks)
                    {
                        //add to observationStock for potential Ancestor calcs use
                        observationStock.Change1.InitTotalME2Change1Properties(observationStock.Change1);
                        observationStock.Change1.CopyTotalME2IndicatorStockProperties(observationStock.Change1, total);
                    }
                    //each of the stocks is a unique label-dependent total
                    observationStock.Change1.Stocks = new List<ME2Stock>();
                    //lists needed to store label-aggregated indicators (a1010, a1011) for each observation
                    List<ME2Change1> baseTotals = GetBaseChanges(baseIds, me2Stock, observation);
                    List<ME2Change1> xminus1Totals = GetXMinus1Changes(xMinus1Ids, me2Stock, observation);
                    AddChangesToStock(i, observation, observationStock,
                        baseTotals, xminus1Totals);
                    
                    obsStocks.Add(observationStock);
                    i++;
                }
            }
            if (obsStocks.Count > 0)
            {
                //replace the totalstocks with change stocks
                me2Stock.Stocks = obsStocks;
                bHasChanges = true;
            }
            return bHasChanges;
        }
        private static List<ME2Change1> GetBaseChanges(List<int> baseIds, 
            ME2Stock me2Stock, ME2Stock observation)
        {
            List<ME2Change1> baseTotals = new List<ME2Change1>();
            ME2Stock benchmark = GetChangeStockByLabel(observation, baseIds, 
                me2Stock.Stocks, Calculator1.CHANGE_TYPES.baseline.ToString());
            if (benchmark != null)
            {
                if (benchmark.Total1 != null)
                {
                    if (benchmark.Total1.Stocks != null)
                    {
                        //loop through the indicator label-aggregated totals
                        foreach (ME2Total1 total in benchmark.Total1.Stocks)
                        {
                            //and fill in the base list
                            ME2Change1 baseChange = new ME2Change1(observation.CalcParameters);
                            baseChange.CopyTotalME2IndicatorStockProperties(baseChange, total);
                            baseTotals.Add(baseChange);
                        }
                    }
                }
            }
            return baseTotals;
        }
        private static List<ME2Change1> GetXMinus1Changes(List<int> xMinus1Ids, 
            ME2Stock me2Stock, ME2Stock observation)
        {
            List<ME2Change1> xminus1Totals = new List<ME2Change1>();
            ME2Stock xminus1 = GetChangeStockByLabel(observation, xMinus1Ids, 
                me2Stock.Stocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
            if (xminus1 != null)
            {
                if (xminus1.Total1 != null)
                {
                    if (xminus1.Total1.Stocks != null)
                    {
                        //loop through the indicator label-aggregated totals
                        foreach (ME2Total1 total in xminus1.Total1.Stocks)
                        {
                            //and fill in the base list
                            ME2Change1 xminus1Change = new ME2Change1(observation.CalcParameters);
                            xminus1Change.CopyTotalME2IndicatorStockProperties(xminus1Change, total);
                            xminus1Totals.Add(xminus1Change);
                        }
                    }
                }
            }
            return xminus1Totals;
        }
        private static ME2Stock GetBaseChangeStockByLabel(ME2Stock actual, List<int> ids,
            List<ME2Stock> changeStocks, string changeType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            ME2Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (changeStocks.Any(p => p.Label == actual.Label
                && p.ChangeType == changeType))
            {
                foreach (ME2Stock change in changeStocks)
                {
                    if (change.ChangeType == changeType)
                    {
                        if (actual.Label == change.Label)
                        {
                            plannedMatch = change;
                            //break the for loop
                            break;
                        }
                    }
                }
            }
            return plannedMatch;
        }
        private static ME2Stock GetChangeStockByLabel(ME2Stock actual, List<int> ids,
            List<ME2Stock> changeStocks, string changeType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            ME2Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (changeStocks.Any(p => p.Label == actual.Label
                && p.ChangeType == changeType))
            {
                int iIndex = 1;
                foreach (ME2Stock change in changeStocks)
                {
                    if (change.ChangeType == changeType)
                    {
                        if (actual.Label == change.Label)
                        {
                            //make sure it hasn't already been used (2 or more els with same Labels)
                            if (!ids.Any(i => i == iIndex))
                            {
                                plannedMatch = change;
                                //index based check is ok
                                ids.Add(iIndex);
                                //break the for loop
                                break;
                            }
                            else
                            {
                                //break if no remaining planned has same label
                                bool bHasMatch = HasChangeMatchByLabel(actual.Label, change,
                                    changeStocks, changeType);
                                if (!bHasMatch)
                                {
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
        private static bool HasChangeMatchByLabel(string aggLabel,
            ME2Stock change, List<ME2Stock> changeStocks, string changeType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (ME2Stock rp in changeStocks)
            {
                if (rp.ChangeType == changeType)
                {
                    if (bStart)
                    {
                        if (aggLabel == change.Label)
                        {
                            bHasMatch = true;
                            break;
                        }
                    }
                    if (rp.Id == change.Id)
                    {
                        bStart = true;
                    }
                }
            }
            return bHasMatch;
        }

        private static bool SetChanges(ME2Stock me2Stock)
        {
            bool bHasChanges = false;
            int i = 0;
            //replace list of totalstocks with list of changestocks
            List<ME2Stock> obsStocks = new List<ME2Stock>();
            //lists needed to store label-aggregated indicators (a1010, a1011) for each observation
            List<ME2Change1> baseTotals = new List<ME2Change1>();
            List<ME2Change1> xminus1Totals = new List<ME2Change1>();
            //loop throught the observations being compared
            foreach (ME2Stock observation in me2Stock.Stocks)
            {
                //actual totals are contained in observation.Total1.Stocks (and Stocks are Total1s)
                if (observation.Total1 != null)
                {
                    if (observation.Total1.Stocks != null)
                    {
                        //unlike totals, obsstock needs obs.CopyCalcs so it matches with baseelement
                        ME2Stock observationStock = new ME2Stock(me2Stock.CalcParameters,
                            me2Stock.CalcParameters.AnalyzerParms.AnalyzerType);
                        //need the base el id
                        observationStock.CopyCalculatorProperties(observation);
                        //where the stats go
                        observationStock.Change1 = new ME2Change1(observation.CalcParameters);
                        observationStock.Change1.CalcParameters = new CalculatorParameters(me2Stock.CalcParameters);
                        //need the base el id
                        observationStock.Change1.CopyCalculatorProperties(observation);
                        //each of the stocks will be a unique label-dependent change
                        observationStock.Change1.Stocks = new List<ME2Stock>();
                        //if count == 0 will go to condition and insert all zeroes for change analysis
                        if (i == 0
                            && observation.Total1.Stocks.Count > 0)
                        {
                            //loop through the indicator label-aggregated totals
                            foreach (ME2Total1 total in observation.Total1.Stocks)
                            {
                                //and fill in the base list
                                ME2Change1 baseChange = new ME2Change1(observation.CalcParameters);
                                baseChange.CopyTotalME2IndicatorStockProperties(baseChange, total);
                                baseTotals.Add(baseChange);
                                //add to observationStock for potential Ancestor calcs use
                                observationStock.Change1.InitTotalME2Change1Properties(observationStock.Change1);
                                observationStock.Change1.CopyTotalME2IndicatorStockProperties(observationStock.Change1, total);
                            }
                        }
                        else
                        {
                            AddChangesToStock(i, observation, observationStock,
                                baseTotals, xminus1Totals);
                            xminus1Totals = new List<ME2Change1>();
                            //loop through the indicator label-aggregated totals
                            foreach (ME2Total1 total in observation.Total1.Stocks)
                            {
                                //and fill in the base list
                                ME2Change1 xminus1Change = new ME2Change1(observation.CalcParameters);
                                xminus1Change.CopyTotalME2IndicatorStockProperties(xminus1Change, total);
                                xminus1Totals.Add(xminus1Change);
                                //add to observationStock for potential Ancestor calcs use
                                observationStock.Change1.InitTotalME2Change1Properties(observationStock.Change1);
                                observationStock.Change1.CopyTotalME2IndicatorStockProperties(observationStock.Change1, total);
                            }
                        }
                        if (observationStock.Change1.Stocks.Count > 0)
                        {
                            obsStocks.Add(observationStock);
                        }
                        else
                        {
                            //insert zeroes
                            AddChangesToStock(i, observation, observationStock,
                                baseTotals, xminus1Totals);
                            obsStocks.Add(observationStock);
                        }
                        i++;
                    }
                }
            }
            if (obsStocks.Count > 0)
            {
                //replace the totalstocks with change stocks
                me2Stock.Stocks = obsStocks;
                bHasChanges = true;
            }
            return bHasChanges;
        }
        private static void AddChangesToStock(int i, ME2Stock observation, ME2Stock observationStock,
            List<ME2Change1> baseTotals, List<ME2Change1> xminus1Totals)
        {
            //loop through the indicator label-aggregated totals
            foreach (ME2Total1 total in observation.Total1.Stocks)
            {
                ME2Change1 newChange = new ME2Change1(observation.CalcParameters);
                newChange.InitTotalME2Change1Properties(newChange);
                newChange.CopyTotalME2IndicatorStockProperties(newChange, total);
                if (newChange.ME2Indicators != null)
                {
                    //set N
                    newChange.TME2N = newChange.ME2Indicators.Count;
                }
                //need the same label aggregated total
                ME2Change1 baseChange = GetBaseChange(baseTotals, newChange);
                ME2Change1 xminus1Change = GetXMinus1Change(xminus1Totals, newChange);
                if (baseChange != null)
                {
                    //total
                    newChange.TotalME2MBaseChange = newChange.TME2TMAmount - baseChange.TME2TMAmount;
                    newChange.TotalME2MBasePercentChange
                        = CalculatorHelpers.GetPercent(newChange.TotalME2MBaseChange, baseChange.TME2TMAmount);
                    //q1
                    newChange.TotalME2LBaseChange = newChange.TME2TLAmount - baseChange.TME2TLAmount;
                    newChange.TotalME2LBasePercentChange
                        = CalculatorHelpers.GetPercent(newChange.TotalME2LBaseChange, baseChange.TME2TLAmount);
                    //q2
                    newChange.TotalME2UBaseChange = newChange.TME2TUAmount - baseChange.TME2TUAmount;
                    newChange.TotalME2UBasePercentChange
                        = CalculatorHelpers.GetPercent(newChange.TotalME2UBaseChange, baseChange.TME2TUAmount);
                }
                else
                {
                    newChange.TotalME2MBaseChange = 0;
                    newChange.TotalME2MBasePercentChange = 0;
                    //q1
                    newChange.TotalME2LBaseChange = 0;
                    newChange.TotalME2LBasePercentChange = 0;
                    //q2
                    newChange.TotalME2UBaseChange = 0;
                    newChange.TotalME2UBasePercentChange = 0;
                }
                if (xminus1Change != null)
                {
                    newChange.TotalME2MAmountChange = newChange.TME2TMAmount - xminus1Change.TME2TMAmount;
                    newChange.TotalME2MPercentChange
                        = CalculatorHelpers.GetPercent(newChange.TotalME2MAmountChange, xminus1Change.TME2TMAmount);
                    //q1
                    newChange.TotalME2LAmountChange = newChange.TME2TLAmount - xminus1Change.TME2TLAmount;
                    newChange.TotalME2LPercentChange
                        = CalculatorHelpers.GetPercent(newChange.TotalME2LAmountChange, xminus1Change.TME2TLAmount);
                    //q2
                    newChange.TotalME2UAmountChange = newChange.TME2TUAmount - xminus1Change.TME2TUAmount;
                    newChange.TotalME2UPercentChange
                        = CalculatorHelpers.GetPercent(newChange.TotalME2UAmountChange, xminus1Change.TME2TUAmount);
                }
                else
                {
                    newChange.TotalME2MAmountChange = 0;
                    newChange.TotalME2MPercentChange = 0;
                    //q1
                    newChange.TotalME2LAmountChange = 0;
                    newChange.TotalME2LPercentChange = 0;
                    //q2
                    newChange.TotalME2UAmountChange = 0;
                    newChange.TotalME2UPercentChange = 0;
                }
                //add new change to observationStock.Change1.Stocks
                observationStock.Change1.Stocks.Add(newChange);
            }
        }
        private static ME2Change1 GetBaseChange(List<ME2Change1> baseTotals, ME2Change1 newChange)
        {
            ME2Change1 baseChange = null;
            //changes are measured between label-aggregated indicators
            if (baseTotals.Any(t => t.TME2Label == newChange.TME2Label))
            {
                baseChange = baseTotals
                    .FirstOrDefault(t => t.TME2Label == newChange.TME2Label);
            }
            return baseChange;
        }
        private static ME2Change1 GetXMinus1Change(List<ME2Change1> xminus1Totals, ME2Change1 newChange)
        {
            ME2Change1 xminus1Change = null;
            if (xminus1Totals.Any(t => t.TME2Label == newChange.TME2Label))
            {
                xminus1Change = xminus1Totals
                    .FirstOrDefault(t => t.TME2Label == newChange.TME2Label);
            }
            return xminus1Change;
        }
    }
}