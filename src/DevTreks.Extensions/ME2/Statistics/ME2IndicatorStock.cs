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
    ///Date:		2016, November
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
        public ME2IndicatorStock(CalculatorParameters calcParams)
            : base(calcParams)
        {
            //indicator stock object
            InitTotalME2IndicatorStocksProperties();
        }
        
        //calculator properties
        //list of indicator1stocks (costs) using List pattern
        public List<ME2IndicatorStock> ME2IndicatorStocks = new List<ME2IndicatorStock>();
        //maximum limit for reasonable serialization 
        //treat the score like any other stock that can be aggreg and compared (so iterate with i = 0)
        private int MaximumNumberOfME2IndicatorStocks = 15;

        public string TME2Name { get; set; }
        //description
        public string TME2Description { get; set; }
        //aggregation label
        public string TME2Label { get; set; }
        //RUC_TYPES or distribution enum
        public string TME2Type { get; set; }
        //date of indicator measurement
        public DateTime TME2Date { get; set; }
        //algorithm = basic stats ...
        public string TME2MathType { get; set; }
        public string TME2BaseIO { get; set; }
        //first quantitative prop
        //amount
        public double TME21Amount { get; set; }
        public string TME21Unit { get; set; }
        //second quantity
        public double TME22Amount { get; set; }
        //second unit
        public string TME22Unit { get; set; }
        //third quantity
        public double TME23Amount { get; set; }
        public string TME23Unit { get; set; }
        public double TME24Amount { get; set; }
        public string TME24Unit { get; set; }
        //total of the two indicators (p*q = cost)
        public double TME25Amount { get; set; }
        //unit for total (i.e. hours physical activity, cost, benefit, number (stock groups)
        public string TME25Unit { get; set; }
        //related indicator label i.e. emissions and env performance
        public string TME2RelLabel { get; set; }
        public double TME2TAmount { get; set; }
        public string TME2TUnit { get; set; }
        public double TME2TD1Amount { get; set; }
        public string TME2TD1Unit { get; set; }
        public double TME2TD2Amount { get; set; }
        public string TME2TD2Unit { get; set; }
        public string TME2MathResult { get; set; }
        public string TME2MathSubType { get; set; }

        public double TME2TMAmount { get; set; }
        public string TME2TMUnit { get; set; }
        public double TME2TLAmount { get; set; }
        public string TME2TLUnit { get; set; }
        public double TME2TUAmount { get; set; }
        public string TME2TUUnit { get; set; }
        public string TME2MathOperator { get; set; }
        public string TME2MathExpression { get; set; }
        public double TME2N { get; set; }

        public const string cTME2Name = "TME2Name";
        public const string cTME2Description = "TME2Description";
        public const string cTME2Label = "TME2Label";
        public const string cTME2Type = "TME2Type";
        public const string cTME2Date = "TME2Date";
        public const string cTME2MathType = "TME2MathType";
        public const string cTME2BaseIO = "TME2BaseIO";
        public const string cTME21Amount = "TME21Amount";
        public const string cTME21Unit = "TME21Unit";
        public const string cTME22Amount = "TME22Amount";
        public const string cTME22Unit = "TME22Unit";
        public const string cTME23Amount = "TME23Amount";
        public const string cTME23Unit = "TME23Unit";
        public const string cTME24Amount = "TME24Amount";
        public const string cTME24Unit = "TME24Unit";
        public const string cTME25Amount = "TME25Amount";
        public const string cTME25Unit = "TME25Unit";
        public const string cTME2RelLabel = "TME2RelLabel";
        public const string cTME2TAmount = "TME2TAmount";
        public const string cTME2TUnit = "TME2TUnit";
        public const string cTME2TD1Amount = "TME2TD1Amount";
        public const string cTME2TD1Unit = "TME2TD1Unit";
        public const string cTME2TD2Amount = "TME2TD2Amount";
        public const string cTME2TD2Unit = "TME2TD2Unit";
        public const string cTME2MathResult = "TME2MathResult";
        public const string cTME2MathSubType = "TME2MathSubType";

        public const string cTME2TMAmount = "TME2TMAmount";
        public const string cTME2TMUnit = "TME2TMUnit";
        public const string cTME2TLAmount = "TME2TLAmount";
        public const string cTME2TLUnit = "TME2TLUnit";
        public const string cTME2TUAmount = "TME2TUAmount";
        public const string cTME2TUUnit = "TME2TUUnit";
        public const string cTME2MathOperator = "TME2MathOperator";
        public const string cTME2MathExpression = "TME2MathExpression";
        public const string cTME2N = "TME2N";

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
            ind.TME2Description = string.Empty;
            ind.TME2Name = string.Empty;
            ind.TME2Label = string.Empty;
            ind.TME2Type = RUC_TYPES.none.ToString();
            ind.TME2RelLabel = string.Empty;
            ind.TME2TAmount = 0;
            ind.TME2TUnit = string.Empty;
            ind.TME2TD1Amount = 0;
            ind.TME2TD1Unit = string.Empty;
            ind.TME2TD2Amount = 0;
            ind.TME2TD2Unit = string.Empty;
            ind.TME2MathResult = string.Empty;
            ind.TME2MathSubType = Constants.NONE;
            ind.TME2TMAmount = 0;
            ind.TME2TMUnit = string.Empty;
            ind.TME2TLAmount = 0;
            ind.TME2TLUnit = string.Empty;
            ind.TME2TUAmount = 0;
            ind.TME2TUUnit = string.Empty;
            ind.TME2MathOperator = MATH_OPERATOR_TYPES.none.ToString();
            ind.TME2MathExpression = string.Empty;
            ind.TME2N = 0;
            ind.TME2Date = CalculatorHelpers.GetDateShortNow();
            ind.TME2MathType = MATH_TYPES.none.ToString();
            ind.TME2BaseIO = ME2Indicator.BASEIO_TYPES.none.ToString();
            ind.TME21Amount = 0;
            ind.TME21Unit = string.Empty;
            ind.TME22Amount = 0;
            ind.TME22Unit = string.Empty;
            ind.TME25Amount = 0;
            ind.TME25Unit = string.Empty;
            ind.TME23Amount = 0;
            ind.TME23Unit = string.Empty;
            ind.TME24Amount = 0;
            ind.TME24Unit = string.Empty;
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
                    ME2IndicatorStock indstock = new ME2IndicatorStock(calculator.CalcParameters);
                    CopyTotalME2IndicatorStockProperties(indstock, calculatorInd);
                    this.ME2IndicatorStocks.Add(indstock);
                }
            }
        }
        public void CopyTotalME2IndicatorStockProperties(ME2IndicatorStock ind,
            ME2IndicatorStock calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TME2Description = calculator.TME2Description;
            ind.TME2Name = calculator.TME2Name;
            ind.TME2Label = calculator.TME2Label;
            ind.TME2Type = calculator.TME2Type;
            ind.TME2RelLabel = calculator.TME2RelLabel;
            ind.TME2TAmount = calculator.TME2TAmount;
            ind.TME2TUnit = calculator.TME2TUnit;
            ind.TME2TD1Amount = calculator.TME2TD1Amount;
            ind.TME2TD1Unit = calculator.TME2TD1Unit;
            ind.TME2TD2Amount = calculator.TME2TD2Amount;
            ind.TME2TD2Unit = calculator.TME2TD2Unit;
            ind.TME2MathResult = calculator.TME2MathResult;
            ind.TME2MathSubType = calculator.TME2MathSubType;

            ind.TME2TMAmount = calculator.TME2TMAmount;
            ind.TME2TMUnit = calculator.TME2TMUnit;
            ind.TME2TLAmount = calculator.TME2TLAmount;
            ind.TME2TLUnit = calculator.TME2TLUnit;
            ind.TME2TUAmount = calculator.TME2TUAmount;
            ind.TME2TUUnit = calculator.TME2TUUnit;
            ind.TME2MathOperator = calculator.TME2MathOperator;
            ind.TME2MathExpression = calculator.TME2MathExpression;
            ind.TME2N = calculator.TME2N;
            ind.TME2Date = calculator.TME2Date;
            ind.TME2MathType = calculator.TME2MathType;
            ind.TME2BaseIO = calculator.TME2BaseIO;
            ind.TME21Amount = calculator.TME21Amount;
            ind.TME21Unit = calculator.TME21Unit;
            ind.TME22Amount = calculator.TME22Amount;
            ind.TME22Unit = calculator.TME22Unit;
            ind.TME25Amount = calculator.TME25Amount;
            ind.TME25Unit = calculator.TME25Unit;
            ind.TME23Amount = calculator.TME23Amount;
            ind.TME23Unit = calculator.TME23Unit;
            ind.TME24Amount = calculator.TME24Amount;
            ind.TME24Unit = calculator.TME24Unit;

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
            //score, or index = 0, is just another stock that can be aggreg and compared to other scores
            int i = 0;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 0; i < this.MaximumNumberOfME2IndicatorStocks; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cTME2Name, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cTME2Label, sAttNameExtension));
                    if (!string.IsNullOrEmpty(sHasAttribute))
                    {
                        ME2IndicatorStock ind1 = new ME2IndicatorStock(this.CalcParameters);
                        SetTotalME2IndicatorStockProperties(ind1, sAttNameExtension, calculator);
                        this.ME2IndicatorStocks.Add(ind1);
                    }
                }
                sHasAttribute = string.Empty;
            }
        }
        public void SetTotalME2IndicatorStockProperties(ME2IndicatorStock ind,
            string attNameExtension, XElement calculator)
        {
            ind.TME2Description = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTME2Description, attNameExtension));
            ind.TME2Name = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2Name, attNameExtension));
            ind.TME2Label = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2Label, attNameExtension));
            ind.TME2Type = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2Type, attNameExtension));
            ind.TME21Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME21Amount, attNameExtension));
            ind.TME21Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME21Unit, attNameExtension));
            ind.TME2RelLabel = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2RelLabel, attNameExtension));
            ind.TME2TAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2TAmount, attNameExtension));
            ind.TME2TUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2TUnit, attNameExtension));
            ind.TME2TD1Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2TD1Amount, attNameExtension));
            ind.TME2TD1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2TD1Unit, attNameExtension));
            ind.TME2TD2Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2TD2Amount, attNameExtension));
            ind.TME2TD2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2TD2Unit, attNameExtension));
            ind.TME2MathResult = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2MathResult, attNameExtension));
            ind.TME2MathSubType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2MathSubType, attNameExtension));

            ind.TME2TMAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2TMAmount, attNameExtension));
            ind.TME2TMUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2TMUnit, attNameExtension));
            ind.TME2TLAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2TLAmount, attNameExtension));
            ind.TME2TLUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2TLUnit, attNameExtension));
            ind.TME2TUAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2TUAmount, attNameExtension));
            ind.TME2TUUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2TUUnit, attNameExtension));
            ind.TME2MathOperator = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2MathOperator, attNameExtension));
            ind.TME2MathExpression = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2MathExpression, attNameExtension));
            ind.TME2N = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME2N, attNameExtension));
            ind.TME2Date = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTME2Date, attNameExtension));
            ind.TME2MathType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2MathType, attNameExtension));
            ind.TME2BaseIO = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME2BaseIO, attNameExtension));
            ind.TME22Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME22Amount, attNameExtension));
            ind.TME22Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME22Unit, attNameExtension));
            ind.TME25Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTME25Amount, attNameExtension));
            ind.TME25Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME25Unit, attNameExtension));
            ind.TME23Amount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTME23Amount, attNameExtension));
            ind.TME23Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME23Unit, attNameExtension));
            ind.TME24Amount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTME24Amount, attNameExtension));
            ind.TME24Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTME24Unit, attNameExtension));
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
                ME2IndicatorStock ind1 = new ME2IndicatorStock(this.CalcParameters);
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
                case cTME2Description:
                    ind.TME2Description = attValue;
                    break;
                case cTME2Name:
                    ind.TME2Name = attValue;
                    break;
                case cTME2Label:
                    ind.TME2Label = attValue;
                    break;
                case cTME2Type:
                    ind.TME2Type = attValue;
                    break;
                case cTME2RelLabel:
                    ind.TME2RelLabel = attValue;
                    break;
                case cTME2TAmount:
                    ind.TME2TAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2TUnit:
                    ind.TME2TUnit = attValue;
                    break;
                case cTME2TD1Amount:
                    ind.TME2TD1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2TD1Unit:
                    ind.TME2TD1Unit = attValue;
                    break;
                case cTME2TD2Amount:
                    ind.TME2TD2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2TD2Unit:
                    ind.TME2TD2Unit = attValue;
                    break;
                case cTME2MathResult:
                    ind.TME2MathResult = attValue;
                    break;
                case cTME2MathSubType:
                    ind.TME2MathSubType = attValue;
                    break;
                case cTME2TMAmount:
                    ind.TME2TMAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2TMUnit:
                    ind.TME2TMUnit = attValue;
                    break;
                case cTME2TLAmount:
                    ind.TME2TLAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2TLUnit:
                    ind.TME2TLUnit = attValue;
                    break;
                case cTME2TUAmount:
                    ind.TME2TUAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2TUUnit:
                    ind.TME2TUUnit = attValue;
                    break;
                case cTME2MathOperator:
                    ind.TME2MathOperator = attValue;
                    break;
                case cTME2MathExpression:
                    ind.TME2MathExpression = attValue;
                    break;
                case cTME2N:
                    ind.TME2N = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME2Date:
                    ind.TME2Date = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTME2MathType:
                    ind.TME2MathType = attValue;
                    break;
                case cTME21Amount:
                    ind.TME21Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME21Unit:
                    ind.TME21Unit = attValue;
                    break;
                case cTME22Amount:
                    ind.TME22Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME22Unit:
                    ind.TME22Unit = attValue;
                    break;
                case cTME25Amount:
                    ind.TME25Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME25Unit:
                    ind.TME25Unit = attValue;
                    break;
                case cTME23Amount:
                    ind.TME23Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME23Unit:
                    ind.TME23Unit = attValue;
                    break;
                case cTME24Amount:
                    ind.TME24Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTME24Unit:
                    ind.TME24Unit = attValue;
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
                case cTME2Description:
                    sPropertyValue = ind.TME2Description;
                    break;
                case cTME2Name:
                    sPropertyValue = ind.TME2Name;
                    break;
                case cTME2Type:
                    sPropertyValue = ind.TME2Type;
                    break;
                case cTME2Label:
                    sPropertyValue = ind.TME2Label;
                    break;
                case cTME2RelLabel:
                    sPropertyValue = ind.TME2RelLabel;
                    break;
                case cTME2TAmount:
                    sPropertyValue = ind.TME2TAmount.ToString();
                    break;
                case cTME2TUnit:
                    sPropertyValue = ind.TME2TUnit.ToString();
                    break;
                case cTME2TD1Amount:
                    sPropertyValue = ind.TME2TD1Amount.ToString();
                    break;
                case cTME2TD1Unit:
                    sPropertyValue = ind.TME2TD1Unit.ToString();
                    break;
                case cTME2TD2Amount:
                    sPropertyValue = ind.TME2TD2Amount.ToString();
                    break;
                case cTME2TD2Unit:
                    sPropertyValue = ind.TME2TD2Unit.ToString();
                    break;
                case cTME2MathResult:
                    sPropertyValue = ind.TME2MathResult.ToString();
                    break;
                case cTME2MathSubType:
                    sPropertyValue = ind.TME2MathSubType.ToString();
                    break;
                case cTME2TMAmount:
                    sPropertyValue = ind.TME2TMAmount.ToString();
                    break;
                case cTME2TMUnit:
                    sPropertyValue = ind.TME2TMUnit.ToString();
                    break;
                case cTME2TLAmount:
                    sPropertyValue = ind.TME2TLAmount.ToString();
                    break;
                case cTME2TLUnit:
                    sPropertyValue = ind.TME2TLUnit.ToString();
                    break;
                case cTME2TUAmount:
                    sPropertyValue = ind.TME2TUAmount.ToString();
                    break;
                case cTME2TUUnit:
                    sPropertyValue = ind.TME2TUUnit.ToString();
                    break;
                case cTME2MathOperator:
                    sPropertyValue = ind.TME2MathOperator.ToString();
                    break;
                case cTME2MathExpression:
                    sPropertyValue = ind.TME2MathExpression.ToString();
                    break;
                case cTME2N:
                    sPropertyValue = ind.TME2N.ToString();
                    break;
                case cTME2Date:
                    sPropertyValue = ind.TME2Date.ToString();
                    break;
                case cTME2MathType:
                    sPropertyValue = ind.TME2MathType;
                    break;
                case cTME21Amount:
                    sPropertyValue = ind.TME21Amount.ToString();
                    break;
                case cTME21Unit:
                    sPropertyValue = ind.TME21Unit.ToString();
                    break;
                case cTME22Amount:
                    sPropertyValue = ind.TME22Amount.ToString();
                    break;
                case cTME22Unit:
                    sPropertyValue = ind.TME22Unit;
                    break;
                case cTME25Amount:
                    sPropertyValue = ind.TME25Amount.ToString();
                    break;
                case cTME25Unit:
                    sPropertyValue = ind.TME25Unit.ToString();
                    break;
                case cTME23Amount:
                    sPropertyValue = ind.TME23Amount.ToString();
                    break;
                case cTME23Unit:
                    sPropertyValue = ind.TME23Unit;
                    break;
                case cTME24Amount:
                    sPropertyValue = ind.TME24Amount.ToString();
                    break;
                case cTME24Unit:
                    sPropertyValue = ind.TME24Unit;
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
                int i = 0;
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
            if (!string.IsNullOrEmpty(ind.TME2Name) && ind.TME2Name != Constants.NONE)
            {
                //remember that the calculator inheriting from this class must set id and name atts
                //and remove unwanted old atts i.e. ind.SetCalculatorAttributes(calculator);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2Name, attNameExtension), ind.TME2Name);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2Label, attNameExtension), ind.TME2Label);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2Description, attNameExtension), ind.TME2Description);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTME2Date, attNameExtension), ind.TME2Date);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2MathType, attNameExtension), ind.TME2MathType);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2BaseIO, attNameExtension), ind.TME2BaseIO);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2Type, attNameExtension), ind.TME2Type);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2RelLabel, attNameExtension), ind.TME2RelLabel);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME2TAmount, attNameExtension), ind.TME2TAmount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2TUnit, attNameExtension), ind.TME2TUnit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME2TD1Amount, attNameExtension), ind.TME2TD1Amount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2TD1Unit, attNameExtension), ind.TME2TD1Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME2TD2Amount, attNameExtension), ind.TME2TD2Amount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2TD2Unit, attNameExtension), ind.TME2TD2Unit);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTME2MathResult, attNameExtension), ind.TME2MathResult);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTME2MathSubType, attNameExtension), ind.TME2MathSubType);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME2TMAmount, attNameExtension), ind.TME2TMAmount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2TMUnit, attNameExtension), ind.TME2TMUnit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME2TLAmount, attNameExtension), ind.TME2TLAmount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2TLUnit, attNameExtension), ind.TME2TLUnit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME2TUAmount, attNameExtension), ind.TME2TUAmount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2TUUnit, attNameExtension), ind.TME2TUUnit);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTME2MathOperator, attNameExtension), ind.TME2MathOperator);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME2MathExpression, attNameExtension), ind.TME2MathExpression);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTME2N, attNameExtension), ind.TME2N);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTME25Amount, attNameExtension), ind.TME25Amount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME25Unit, attNameExtension), ind.TME25Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME21Amount, attNameExtension), ind.TME21Amount);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTME21Unit, attNameExtension), ind.TME21Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTME22Amount, attNameExtension), ind.TME22Amount);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTME22Unit, attNameExtension), ind.TME22Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTME23Amount, attNameExtension), ind.TME23Amount);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTME23Unit, attNameExtension), ind.TME23Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTME24Amount, attNameExtension), ind.TME24Amount);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTME24Unit, attNameExtension), ind.TME24Unit);
            }
        }
        public virtual void SetTotalME2IndicatorStocksAttributes(string attNameExt, 
            ref XmlWriter writer)
        {
            //remember that the analyzer inheriting from this must .SetAnalyzerAtts
            if (this.ME2IndicatorStocks != null)
            {
                int i = 0;
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
            if (!string.IsNullOrEmpty(ind.TME2Name) && ind.TME2Name != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTME2Name, attNameExtension), ind.TME2Name);
                writer.WriteAttributeString(
                    string.Concat(cTME2Description, attNameExtension), ind.TME2Description);
                writer.WriteAttributeString(
                        string.Concat(cTME2Label, attNameExtension), ind.TME2Label);
                writer.WriteAttributeString(
                        string.Concat(cTME2Type, attNameExtension), ind.TME2Type);
                writer.WriteAttributeString(
                    string.Concat(cTME2RelLabel, attNameExtension), ind.TME2RelLabel);
                writer.WriteAttributeString(
                    string.Concat(cTME2TAmount, attNameExtension), ind.TME2TAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME2TUnit, attNameExtension), ind.TME2TUnit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME2TD1Amount, attNameExtension), ind.TME2TD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME2TD1Unit, attNameExtension), ind.TME2TD1Unit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME2TD2Amount, attNameExtension), ind.TME2TD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME2TD2Unit, attNameExtension), ind.TME2TD2Unit.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTME2MathResult, attNameExtension), ind.TME2MathResult.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTME2MathSubType, attNameExtension), ind.TME2MathSubType.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTME2TMAmount, attNameExtension), ind.TME2TMAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME2TMUnit, attNameExtension), ind.TME2TMUnit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME2TLAmount, attNameExtension), ind.TME2TLAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME2TLUnit, attNameExtension), ind.TME2TLUnit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME2TUAmount, attNameExtension), ind.TME2TUAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME2TUUnit, attNameExtension), ind.TME2TUUnit.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTME2MathOperator, attNameExtension), ind.TME2MathOperator.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME2MathExpression, attNameExtension), ind.TME2MathExpression.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME2N, attNameExtension), ind.TME2N.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTME2Date, attNameExtension), ind.TME2Date.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTME2MathType, attNameExtension), ind.TME2MathType);
                writer.WriteAttributeString(
                    string.Concat(cTME2BaseIO, attNameExtension), ind.TME2BaseIO);
                writer.WriteAttributeString(
                        string.Concat(cTME21Amount, attNameExtension), ind.TME21Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTME21Unit, attNameExtension), ind.TME21Unit.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTME22Amount, attNameExtension), ind.TME22Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME22Unit, attNameExtension), ind.TME22Unit);
                writer.WriteAttributeString(
                    string.Concat(cTME25Amount, attNameExtension), ind.TME25Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME25Unit, attNameExtension), ind.TME25Unit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTME23Amount, attNameExtension), ind.TME23Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME23Unit, attNameExtension), ind.TME23Unit);
                writer.WriteAttributeString(
                    string.Concat(cTME24Amount, attNameExtension), ind.TME24Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTME24Unit, attNameExtension), ind.TME24Unit);
            }
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
                .Any(s => s.TME2Label == indicator.IndLabel))
            {
                if (indicator.IndLabel != string.Empty)
                {
                    ME2IndicatorStock stock = new ME2IndicatorStock(indicator.CalcParameters);
                    stock.TME2Description = indicator.IndDescription;
                    stock.TME2Name = indicator.IndName;
                    stock.TME2Label = indicator.IndLabel;
                    stock.TME2Type = indicator.IndType;
                    stock.TME2RelLabel = indicator.IndRelLabel;
                    stock.TME2TAmount = indicator.IndTAmount;
                    stock.TME2TUnit = indicator.IndTUnit;
                    stock.TME2TD1Amount = indicator.IndTD1Amount;
                    stock.TME2TD1Unit = indicator.IndTD1Unit;
                    stock.TME2TD2Amount = indicator.IndTD2Amount;
                    stock.TME2TD2Unit = indicator.IndTD2Unit;
                    stock.TME2MathResult = indicator.IndMathResult;
                    stock.TME2MathSubType = indicator.IndMathSubType;
                    stock.TME2TMAmount = indicator.IndTMAmount;
                    stock.TME2TMUnit = indicator.IndTMUnit;
                    stock.TME2TLAmount = indicator.IndTLAmount;
                    stock.TME2TLUnit = indicator.IndTLUnit;
                    stock.TME2TUAmount = indicator.IndTUAmount;
                    stock.TME2TUUnit = indicator.IndTUUnit;
                    stock.TME2MathOperator = indicator.IndMathOperator;
                    stock.TME2MathExpression = indicator.IndMathExpression;
                    stock.TME2Date = indicator.IndDate;
                    stock.TME2MathType = indicator.IndMathType;
                    stock.TME2BaseIO = indicator.IndBaseIO;
                    stock.TME21Amount = indicator.Ind1Amount;
                    stock.TME21Unit = indicator.Ind1Unit;
                    stock.TME22Amount = indicator.Ind2Amount;
                    stock.TME22Unit = indicator.Ind2Unit;
                    stock.TME25Amount = indicator.Ind5Amount;
                    stock.TME25Unit = indicator.Ind5Unit;
                    stock.TME23Amount = indicator.Ind3Amount;
                    stock.TME23Unit = indicator.Ind3Unit;
                    stock.TME24Amount = indicator.Ind4Amount;
                    stock.TME24Unit = indicator.Ind4Unit;
                    //test
                    stock.TME2N = 1;
                    //add the stock to the basestat
                    baseStat.ME2IndicatorStocks.Add(stock);
                }
            }
            else
            {
                //update the identifiers in case they have changed
                ME2IndicatorStock stock = baseStat.ME2IndicatorStocks
                    .FirstOrDefault(s => s.TME2Label == indicator.IndLabel);
                if (stock != null)
                {
                    stock.TME2Description = indicator.IndDescription;
                    stock.TME2Name = indicator.IndName;
                    stock.TME2Label = indicator.IndLabel;
                    stock.TME2Type = indicator.IndType;
                    stock.TME2RelLabel = indicator.IndRelLabel;
                    stock.TME2TAmount = indicator.IndTAmount;
                    stock.TME2TUnit = indicator.IndTUnit;
                    stock.TME2TD1Amount = indicator.IndTD1Amount;
                    stock.TME2TD1Unit = indicator.IndTD1Unit;
                    stock.TME2TD2Amount = indicator.IndTD2Amount;
                    stock.TME2TD2Unit = indicator.IndTD2Unit;
                    stock.TME2MathResult = indicator.IndMathResult;
                    stock.TME2MathSubType = indicator.IndMathSubType;
                    stock.TME2TMAmount = indicator.IndTMAmount;
                    stock.TME2TMUnit = indicator.IndTMUnit;
                    stock.TME2TLAmount = indicator.IndTLAmount;
                    stock.TME2TLUnit = indicator.IndTLUnit;
                    stock.TME2TUAmount = indicator.IndTUAmount;
                    stock.TME2TUUnit = indicator.IndTUUnit;
                    stock.TME2MathOperator = indicator.IndMathOperator;
                    stock.TME2MathExpression = indicator.IndMathExpression;
                    stock.TME2Date = indicator.IndDate;
                    stock.TME2MathType = indicator.IndMathType;
                    stock.TME2BaseIO = indicator.IndBaseIO;
                    stock.TME21Amount = indicator.Ind1Amount;
                    stock.TME21Unit = indicator.Ind1Unit;
                    stock.TME22Amount = indicator.Ind2Amount;
                    stock.TME22Unit = indicator.Ind2Unit;
                    stock.TME25Amount = indicator.Ind5Amount;
                    stock.TME25Unit = indicator.Ind5Unit;
                    stock.TME23Amount = indicator.Ind3Amount;
                    stock.TME23Unit = indicator.Ind3Unit;
                    stock.TME24Amount = indicator.Ind4Amount;
                    stock.TME24Unit = indicator.Ind4Unit;
                    //test
                    stock.TME2N++;
                }
            }
        }
    }
}
