using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The SubPrice3Stock class indirectly extends the SubPrice3() class.
    ///             The SubPrice3.SubPrice3s collection is used to manipulate the class.
    ///             The class keeps track of quantitative progress with benchmark, 
    ///             partial target, and full target, indicators.
    ///Author:		www.devtreks.org
    ///Date:		2013, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES:       1. The Econ Eval element that inherits from this class must
    ///             use two properties from the base class: SubPrice3.RiskType and 
    ///             SubPrice3.DecisionType. Because all nodes must be calculated using
    ///             uniform risk and decision criteria (they could have been calculated 
    ///             differently).
    ///             
    ///</summary>
    public class SubPrice3Stock : SubPrice3
    {
        //calls the base-class version, and initializes the base class properties.
        public SubPrice3Stock()
            : base()
        {
            //subprice object
            InitTotalSubPrice3StocksProperties();
        }
        //copy constructor
        public SubPrice3Stock(SubPrice3Stock calculator)
        {
            CopyTotalSubPrice3StocksProperties(calculator);
        }

        //calculator properties

        //list of indicator1stocks using List pattern
        public List<SubPrice3Stock> SubPrice3Stocks = new List<SubPrice3Stock>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfSubPrice3Stocks = 20;


        //label collection of indicators
        public IDictionary<string, List<SubPrice3>> SubPrice3Labels = null;

        //sensitity analysis properties 
        //(vary one cost or benefit property to see how overall costs/bens vary)
        //subprice to vary for analysis
        public string TotalInd1SensLabel { get; set; }
        //sensitity type: price, quantity, escalationrate to vary
        public string TotalInd1SensType { get; set; }
        //min value for analysis
        public double TotalInd1SensMin { get; set; }
        //max value for analysis
        public double TotalInd1SensMax { get; set; }

        //risk analysis properties 
        //(vary several cost or benefit properties to see how overall costs/bens vary)
        //use same parameters for sensitivity, plus
//        Somewhat greater than an even chance

//0.45-0.55

//Medium:

//> 0.45 <=0.55

//An even chance to occur

//0.35-0.45

//Medium:

//> 0.35 <=0.45

//Somewhat less than an even chance

//0.25-0.35

//Low:

//> 0.25 <=0.35

//Not very likely to occur

//0.15-0.25

//Low:

//> 0.15 <=0.25

//Not likely to occur

//0.00-0.15

//Low:

//> 0.00 <=0.15

//Almost sure not to occur

        public string TotalInd1Name { get; set; }
        public string TotalInd1Label { get; set; }
        public string TotalInd1Description { get; set; }

        //RISKANALYSIS_TYPE is not needed put it in parent class
        public string TotalInd1RiskType { get; set; }
        //ytd total
        //cumulative indicator totals (i.e. ind = Q1*Q2 + ind2 = Q1*Q2
        public double TotalInd1Total { get; set; }
        //unit to display for totals, benchmarks and targets 
        public string TotalInd1Unit { get; set; }
        //benchmark amount (set in mcc01 M&E type)
        public double TotalInd1BM { get; set; }
        //progress %bm
        public double TotalInd1BMProg { get; set; }
        //partial target date
        public DateTime Ind1PTDate { get; set; }
        //ytd date
        public DateTime Ind1YTDDate { get; set; }
        //current partial target total (quarter 1 + quarter 2)
        public double TotalInd1PartTarget { get; set; }
        //current full target total (set in mcc01 M&E type)
        public double TotalInd1FullTarget { get; set; }
        //%target (TotalYTD / TotalPT)
        public double TotalInd1PTProg { get; set; }
        //%target (TotalYTD / TotalFT)
        public double TotalInd1FTProg { get; set; }

        //future potential stats
        //public double IndMin{ get; set; }
        //public double IndMax{ get; set; }

        private const string cTotalInd1Description = "TInd1Description";
        private const string cTotalInd1RiskType = "TInd1RiskType";
        private const string cTotalInd1Name = "TName";
        private const string cTotalInd1Label = "TInd1Label";
        private const string cTotalInd1Total = "TInd1Total";
        private const string cTotalInd1BM = "TInd1BM";
        private const string cTotalInd1BMProg = "TInd1BMProg";
        private const string cTotalInd1Unit = "TInd1Unit";
        private const string cInd1PTDate = "TInd1PTDate";
        private const string cInd1YTDDate = "TInd1YTDDate";
        private const string cTotalInd1PartTarget = "TInd1PartTarget";
        private const string cTotalInd1FullTarget = "TInd1FullTarget";
        private const string cTotalInd1PTProg = "TInd1PTProg";
        private const string cTotalInd1FTProg = "TInd1FTProg";
        public virtual void InitTotalSubPrice3StocksProperties()
        {
            if (this.SubPrice3Stocks == null)
            {
                this.SubPrice3Stocks = new List<SubPrice3Stock>();
            }
            foreach (SubPrice3Stock ind in this.SubPrice3Stocks)
            {
                InitTotalSubPrice3StockProperties(ind);
            }
        }
        private void InitTotalSubPrice3StockProperties(SubPrice3Stock ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.TotalInd1Description = string.Empty;
            ind.TotalInd1RiskType = string.Empty;
            ind.TotalInd1Name = string.Empty;
            ind.TotalInd1Label = string.Empty;
            ind.TotalInd1Total = 0;
            ind.TotalInd1BM = 0;
            ind.TotalInd1BMProg = 0;
            ind.TotalInd1Unit = string.Empty;
            //ind.SubP1PTDate = CalculatorHelpers.GetDateShortNow();
            //ind.SubP1YTDDate = CalculatorHelpers.GetDateShortNow();
            ind.TotalInd1PartTarget = 0;
            ind.TotalInd1FullTarget = 0;
            ind.TotalInd1PTProg = 0;
            ind.TotalInd1FTProg = 0;
        }
        public virtual void CopyTotalSubPrice3StocksProperties(
           SubPrice3Stock calculator)
        {
            if (calculator.SubPrice3Stocks != null)
            {
                if (this.SubPrice3Stocks == null)
                {
                    this.SubPrice3Stocks = new List<SubPrice3Stock>();
                }
                foreach (SubPrice3Stock calculatorInd in calculator.SubPrice3Stocks)
                {
                    SubPrice3Stock indstock = new SubPrice3Stock();
                    CopyTotalSubPrice3StockProperties(indstock, calculatorInd);
                    this.SubPrice3Stocks.Add(indstock);
                }
            }
        }
        private void CopyTotalSubPrice3StockProperties(SubPrice3Stock ind,
            SubPrice3Stock calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalInd1Description = calculator.TotalInd1Description;
            ind.TotalInd1RiskType = calculator.TotalInd1RiskType;
            ind.TotalInd1Name = calculator.TotalInd1Name;
            ind.TotalInd1Label = calculator.TotalInd1Label;
            ind.TotalInd1Total = calculator.TotalInd1Total;
            ind.TotalInd1BM = calculator.TotalInd1BM;
            ind.TotalInd1BMProg = calculator.TotalInd1BMProg;
            ind.TotalInd1Unit = calculator.TotalInd1Unit;
            //ind.SubP1PTDate = calculator.SubP1PTDate;
            //ind.SubP1YTDDate = calculator.SubP1YTDDate;
            ind.TotalInd1PartTarget = calculator.TotalInd1PartTarget;
            ind.TotalInd1FullTarget = calculator.TotalInd1FullTarget;
            ind.TotalInd1PTProg = calculator.TotalInd1PTProg;
            ind.TotalInd1FTProg = calculator.TotalInd1FTProg;
        }
        public virtual void SetTotalSubPrice3StocksProperties(XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerProps
            if (this.SubPrice3Stocks == null)
            {
                this.SubPrice3Stocks = new List<SubPrice3Stock>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfSubPrice3Stocks; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(SubPrice3.cSubPName, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    SubPrice3Stock ind1 = new SubPrice3Stock();
                    SetTotalSubPrice3StockProperties(ind1, sAttNameExtension, calculator);
                    this.SubPrice3Stocks.Add(ind1);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetTotalSubPrice3StockProperties(SubPrice3Stock ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalInd1Description = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalInd1Description, attNameExtension));
            ind.TotalInd1RiskType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalInd1RiskType, attNameExtension));
            ind.TotalInd1Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalInd1Name, attNameExtension));
            ind.TotalInd1Label = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalInd1Label, attNameExtension));
            ind.TotalInd1Total = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1Total, attNameExtension));
            ind.TotalInd1BM = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1BM, attNameExtension));
            ind.TotalInd1BMProg = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1BMProg, attNameExtension));
            ind.TotalInd1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTotalInd1Unit, attNameExtension));
            //ind.SubP1PTDate = CalculatorHelpers.GetAttributeDate(calculator,
            //   string.Concat(cInd1PTDate, attNameExtension));
            //ind.SubP1YTDDate = CalculatorHelpers.GetAttributeDate(calculator,
            //   string.Concat(cInd1YTDDate, attNameExtension));
            ind.TotalInd1PartTarget = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1PartTarget, attNameExtension));
            ind.TotalInd1FullTarget = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1FullTarget, attNameExtension));
            ind.TotalInd1PTProg = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1PTProg, attNameExtension));
            ind.TotalInd1FTProg = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalInd1FTProg, attNameExtension));
        }
        public virtual void SetTotalSubPrice3StocksProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.SubPrice3Stocks == null)
            {
                this.SubPrice3Stocks = new List<SubPrice3Stock>();
            }
            if (this.SubPrice3Stocks.Count < (colIndex + 1))
            {
                SubPrice3Stock ind1 = new SubPrice3Stock();
                this.SubPrice3Stocks.Insert(colIndex, ind1);
            }
            SubPrice3Stock ind = this.SubPrice3Stocks.ElementAt(colIndex);
            if (ind != null)
            {
                SetTotalSubPrice3StockProperty(ind, attName, attValue);
            }
        }
        private void SetTotalSubPrice3StockProperty(SubPrice3Stock ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotalInd1Description:
                    ind.TotalInd1Description = attValue;
                    break;
                case cTotalInd1RiskType:
                    ind.TotalInd1RiskType = attValue;
                    break;
                case cTotalInd1Name:
                    ind.TotalInd1Name = attValue;
                    break;
                case cTotalInd1Label:
                    ind.TotalInd1Label = attValue;
                    break;
                case cTotalInd1Total:
                    ind.TotalInd1Total = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalInd1BM:
                    ind.TotalInd1BM = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalInd1BMProg:
                    ind.TotalInd1BMProg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalInd1Unit:
                    ind.TotalInd1Unit = attValue;
                    break;
                //case cInd1PTDate:
                //    ind.SubP1PTDate = CalculatorHelpers.ConvertStringToDate(attValue);
                //    break;
                //case cInd1YTDDate:
                //    ind.SubP1YTDDate = CalculatorHelpers.ConvertStringToDate(attValue);
                //    break;
                case cTotalInd1PartTarget:
                    ind.TotalInd1PartTarget = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalInd1FullTarget:
                    ind.TotalInd1FullTarget = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalInd1PTProg:
                    ind.TotalInd1PTProg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalInd1FTProg:
                    ind.TotalInd1FTProg = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalSubPrice3StocksProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.SubPrice3s.Count >= (colIndex + 1))
            {
                SubPrice3Stock ind = this.SubPrice3Stocks.ElementAt(colIndex);
                if (ind != null)
                {
                    sPropertyValue = GetTotalSubPrice3StockProperty(ind, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetTotalSubPrice3StockProperty(SubPrice3Stock ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalInd1Description:
                    sPropertyValue = ind.TotalInd1Description;
                    break;
                case cTotalInd1RiskType:
                    sPropertyValue = ind.TotalInd1RiskType;
                    break;
                case cTotalInd1Name:
                    sPropertyValue = ind.TotalInd1Name.ToString();
                    break;
                case cTotalInd1Label:
                    sPropertyValue = ind.TotalInd1Label.ToString();
                    break;
                case cTotalInd1Total:
                    sPropertyValue = ind.TotalInd1Total.ToString();
                    break;
                case cTotalInd1BM:
                    sPropertyValue = ind.TotalInd1BM.ToString();
                    break;
                case cTotalInd1BMProg:
                    sPropertyValue = ind.TotalInd1BMProg.ToString();
                    break;
                case cTotalInd1Unit:
                    sPropertyValue = ind.TotalInd1Unit;
                    break;
                //case cInd1PTDate:
                //    sPropertyValue = ind.SubP1PTDate.ToString();
                //    break;
                //case cInd1YTDDate:
                //    sPropertyValue = ind.SubP1YTDDate.ToString();
                //    break;
                case cTotalInd1PartTarget:
                    sPropertyValue = ind.TotalInd1PartTarget.ToString();
                    break;
                case cTotalInd1FullTarget:
                    sPropertyValue = ind.TotalInd1FullTarget.ToString();
                    break;
                case cTotalInd1PTProg:
                    sPropertyValue = ind.TotalInd1PTProg.ToString();
                    break;
                case cTotalInd1FTProg:
                    sPropertyValue = ind.TotalInd1FTProg.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalSubPrice3StocksAttributes(ref XElement calculator)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.SubPrice3s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice3Stock ind in this.SubPrice3Stocks)
                {
                    sAttNameExtension = i.ToString();
                    SetTotalSubPrice3StockAttributes(ind, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        public virtual void SetTotalSubPrice3StockAttributes(SubPrice3Stock ind,
            string attNameExtension, ref XElement calculator)
        {
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cTotalInd1Description, attNameExtension), ind.TotalInd1Description);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cTotalInd1RiskType, attNameExtension), ind.TotalInd1RiskType);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalInd1Name, attNameExtension), ind.TotalInd1Name);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalInd1Label, attNameExtension), ind.TotalInd1Label);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1Total, attNameExtension), ind.TotalInd1Total);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1BM, attNameExtension), ind.TotalInd1BM);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1BMProg, attNameExtension), ind.TotalInd1BMProg);
            CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTotalInd1Unit, attNameExtension), ind.TotalInd1Unit);
            //CalculatorHelpers.SetAttributeDateS(calculator,
            //        string.Concat(cInd1PTDate, attNameExtension), ind.SubP1PTDate);
            //CalculatorHelpers.SetAttributeDateS(calculator,
            //        string.Concat(cInd1YTDDate, attNameExtension), ind.SubP1YTDDate);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1PartTarget, attNameExtension), ind.TotalInd1PartTarget);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1FullTarget, attNameExtension), ind.TotalInd1FullTarget);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1PTProg, attNameExtension), ind.TotalInd1PTProg);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                    string.Concat(cTotalInd1FTProg, attNameExtension), ind.TotalInd1FTProg);
        }
        public virtual void SetTotalSubPrice3StocksAttributes(ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.SubPrice3s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice3Stock ind in this.SubPrice3Stocks)
                {
                    sAttNameExtension = i.ToString();
                    SetTotalSubPrice3StockAttributes(ind, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        private void SetTotalSubPrice3StockAttributes(SubPrice3Stock ind,
            string attNameExtension, ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                   string.Concat(cTotalInd1Description, attNameExtension), ind.TotalInd1Description);
            writer.WriteAttributeString(
                   string.Concat(cTotalInd1RiskType, attNameExtension), ind.TotalInd1RiskType);
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1Name, attNameExtension), ind.TotalInd1Name.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1Label, attNameExtension), ind.TotalInd1Label.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1Total, attNameExtension), ind.TotalInd1Total.ToString());
            writer.WriteAttributeString(
                string.Concat(cTotalInd1BM, attNameExtension), ind.TotalInd1BM.ToString());
            writer.WriteAttributeString(
                string.Concat(cTotalInd1BMProg, attNameExtension), ind.TotalInd1BMProg.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1Unit, attNameExtension), ind.TotalInd1Unit);
            //writer.WriteAttributeString(
            //        string.Concat(cInd1PTDate, attNameExtension), ind.SubP1PTDate.ToString());
            //writer.WriteAttributeString(
            //        string.Concat(cInd1YTDDate, attNameExtension), ind.SubP1YTDDate.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1PartTarget, attNameExtension), ind.TotalInd1PartTarget.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1FullTarget, attNameExtension), ind.TotalInd1FullTarget.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1PTProg, attNameExtension), ind.TotalInd1PTProg.ToString());
            writer.WriteAttributeString(
                    string.Concat(cTotalInd1FTProg, attNameExtension), ind.TotalInd1FTProg.ToString());
        }
        //run the analyses
        public bool RunAnalyses()
        {
            bool bHasAnalyses = false;
            //add totals to partial target stocks
            List<SubPrice3Stock> ptStocks = SetAnalyses();
            //add the ptstocks to parent indicators
            if (ptStocks != null)
            {
                bHasAnalyses = AddPTStocksToSubPrice3Stocks(ptStocks);
            }
            return bHasAnalyses;
        }
        private List<SubPrice3Stock> SetAnalyses()
        {
            //only the ptStocks are used in results
            List<SubPrice3Stock> ptStocks = new List<SubPrice3Stock>();
            //step 1. put all of the indicators in a list
            List<SubPrice3> inds = new List<SubPrice3>();
            foreach (SubPrice3Stock indStock in this.SubPrice3Stocks)
            {
                foreach (SubPrice3 ind in indStock.SubPrice3s)
                {
                    //ind.SubPrice3s holds the collection
                    foreach (SubPrice3 indMember in ind.SubPrice3s)
                    {
                        //give it a temp id to find where it's partial target stock total goes
                        //partial target stock total goes with immediate preceding YTD actual date
                        indMember.Id = indStock.Id;
                        inds.Add(indMember);
                    }
                }
            }
            //step 2. order by label and date
            IOrderedEnumerable<SubPrice3> orderedinds
                = inds.OrderBy(i => i.SubPLabel).ThenBy(j => j.SubPName);//j.SubPDate);
            //use label-aggregated groups (must already be ordered by ascending dates)
            IEnumerable<System.Linq.IGrouping<string, SubPrice3>>
                IndsByLabel = orderedinds.GroupBy(i => i.SubPLabel);
            //this.SubPrice3Labels.OrderBy(x => x.Value.OrderBy(y => y.SubPDate));
            //step 3. add all of the indicators to a list of pt stocks and run cumulative totals
            foreach (var labelinds in IndsByLabel)
            {
                //build the ptstocks collection (1stock per partial target)
                foreach (SubPrice3 ind in labelinds)
                {
                    if (ind.SubPAltType == ALTERNATIVE_TYPE.one.ToString())
                    {
                        SubPrice3Stock ptStock = new SubPrice3Stock();
                        //the results only display these partial target stocks
                        ptStock.TotalInd1PartTarget = ind.SubPTotal;
                        //the date must be later than the actuals in the divisor
                        //ptStock.SubP1PTDate = ind.SubPDate;
                        ptStock.TotalInd1Label = ind.SubPLabel;
                        ptStock.TotalInd1Name = ind.SubPName;
                        ptStock.TotalInd1Description = ind.SubPDescription;
                        //store alt type in mandetype (parent has actual mandetype)
                        ptStock.TotalInd1RiskType = ind.SubPAltType;
                        ptStock.TotalInd1Unit = ind.SubPUnit;
                        ptStock.Id = ind.Id;
                        ptStocks.Add(ptStock);
                    }
                }
                //add the totals
                foreach (SubPrice3Stock ptStock in ptStocks)
                {
                    foreach (SubPrice3 ind in labelinds)
                    {
                        //run the cumulative totals
                        AddSubPrice3ToPTStock(ind, ptStock);
                    }
                    AddPTStockTotals(ptStock);
                }
            }
            return ptStocks;
        }

        private void AddSubPrice3ToPTStock(SubPrice3 ind,
            SubPrice3Stock ptStock)
        {
            if (ind.SubPAltType == SubPrice3.ALTERNATIVE_TYPE.one.ToString())
            {
                ptStock.TotalInd1BM = ind.SubPTotal;
            }
            else if (ind.SubPAltType == SubPrice3.ALTERNATIVE_TYPE.two.ToString())
            {
                ptStock.TotalInd1FullTarget = ind.SubPTotal;
            }
            else if (ind.SubPAltType == SubPrice3.ALTERNATIVE_TYPE.three.ToString())
            {
                //if (ptStock.SubP1PTDate > ind.SubPDate
                //    && ptStock.TotalInd1Label == ind.SubPLabel)
                //{
                //    //these must be ordered by date prior to coming in here
                //    ptStock.TotalInd1Total += ind.SubPTotal;
                //    //set ind to zero so it won't accumulate in next ptstock
                //    ind.SubPTotal = 0;
                //    //the date must be correctly entered as before the corresponding partial target date
                //    ptStock.SubP1YTDDate = ind.SubPDate;
                //}
            }
        }
        private void AddPTStockTotals(SubPrice3Stock ptStock)
        {
            if (ptStock.TotalInd1BM == 0)
            {
                ptStock.TotalInd1BMProg = 0;
            }
            else
            {
                ptStock.TotalInd1BMProg
                    = (ptStock.TotalInd1Total / ptStock.TotalInd1BM) * 100;
            }
            if (ptStock.TotalInd1PartTarget == 0)
            {
                ptStock.TotalInd1PTProg = 0;
            }
            else
            {
                ptStock.TotalInd1PTProg
                    = (ptStock.TotalInd1Total / ptStock.TotalInd1PartTarget) * 100;
            }
            if (ptStock.TotalInd1FullTarget == 0)
            {
                ptStock.TotalInd1FTProg = 0;
            }
            else
            {
                ptStock.TotalInd1FTProg
                    = (ptStock.TotalInd1Total / ptStock.TotalInd1FullTarget) * 100;
            }
        }
        private bool AddPTStocksToSubPrice3Stocks(List<SubPrice3Stock> ptStocks)
        {
            bool bHasAnalyses = false;
            //one to one correspondance between this.SubP1Stocks and RISKANALYSIS elements
            foreach (SubPrice3Stock indStock in this.SubPrice3Stocks)
            {
                foreach (SubPrice3 ind in indStock.SubPrice3s)
                {
                    foreach (SubPrice3Stock stock in ptStocks)
                    {
                        //note that indStocId is the same as parent stock id
                        if (indStock.Id == stock.Id)
                        {
                            //up to 20 stocks can be displayed (coming from multiple indicators)
                            indStock.SubPrice3Stocks.Add(stock);
                            bHasAnalyses = true;
                        }
                    }
                }
            }
            return bHasAnalyses;
        }
    }
    public static class SubPrice3StockExtensions
    {
        //add a base health input stock to the baseStat.BuildCost1s dictionary
        public static bool AddSubPrice3StocksToDictionary(this SubPrice3Stock baseStat,
            string filePosition, int nodePosition, SubPrice3 indicators)
        {
            bool bIsAdded = false;
            if (string.IsNullOrEmpty(filePosition) || nodePosition < 0)
            {
                baseStat.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return false;
            }
            if (baseStat.SubPrice3Labels == null)
                baseStat.SubPrice3Labels
                = new Dictionary<string, List<SubPrice3>>();
            if (baseStat.SubPrice3Labels.ContainsKey(filePosition))
            {
                if (baseStat.SubPrice3Labels[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (baseStat.SubPrice3Labels[filePosition].Count <= i)
                        {
                            baseStat.SubPrice3Labels[filePosition]
                                .Add(new SubPrice3());
                        }
                    }
                    baseStat.SubPrice3Labels[filePosition][nodePosition]
                        = indicators;
                    bIsAdded = true;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<SubPrice3> baseStats
                    = new List<SubPrice3>();
                KeyValuePair<string, List<SubPrice3>> newStat
                    = new KeyValuePair<string, List<SubPrice3>>(
                        filePosition, baseStats);
                baseStat.SubPrice3Labels.Add(newStat);
                bIsAdded = AddSubPrice3StocksToDictionary(baseStat,
                    filePosition, nodePosition, indicators);
            }
            return bIsAdded;
        }
        //public static int GetNodePositionCount(this SubPrice3Stock baseStat,
        //    int filePosition, SubPrice3 indicators)
        //{
        //    int iNodeCount = 0;
        //    if (baseStat.SubPrice3s == null)
        //        return iNodeCount;
        //    if (baseStat.SubPrice3s.ContainsKey(filePosition))
        //    {
        //        if (baseStat.SubPrice3s[filePosition] != null)
        //        {
        //            iNodeCount = baseStat.SubPrice3s[filePosition].Count;
        //        }
        //    }
        //    return iNodeCount;
        //}





    }
}
