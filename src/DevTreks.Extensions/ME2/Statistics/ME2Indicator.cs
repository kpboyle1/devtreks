using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Add monitoring and evaluation indicators to DevTreks input, 
    ///             output, operation/component, outcome, and budget elements. 
    ///             Can be run stand alone, with normal stock totals, or as an 
    ///             analyzer step that keeps track of each meal Qs, or each 
    ///             household member Qs
    ///Date:		2016, November
    ///References:	Monitoring and Evaluation Tutorials
    ///NOTES:       Version 2.0.4 upgraded to similar properties and methods as 
    ///             the ResourceStockCalculator to promote consistency in the use 
    ///             of indicators and to accomodate risk and uncertainty in 
    ///             indicator measurement.
    ///             ME2Indicators[0] is equivalent to the Score in the ResourceStockCalcor
    /// </summary>   
    public class ME2Indicator : CostBenefitCalculator
    {
        //constructor
        public ME2Indicator()
        {
            InitME2IndicatorsProperties();
        }
        //copy constructors
        public ME2Indicator(ME2Indicator calculator)
        {
            CopyME2IndicatorsProperties(calculator);
        }
        #region "algo props"
        //calcParams
        public CalculatorParameters CalcParameters { get; set; }
        //internal algo props
        public string[] _colNames { get; set; }
        public string[] _rowNames { get; set; }
        //186 put this var in class because exceptions will lose their state and not use them when running subsequent calcs
        //don't analyze more than CalcParameters.UrisToAnalyze inds
        public int[] _indicators = new int[] { };
        //public const string _score = "score";
        //minus 1 because ++ syntax inits with jdata[0]
        private int _dataIndex = -1;
        //number of x data columns
        public const int xcols = 10;
        public static string[] MATHTERMS = new string[] {
                "I1.Q1","I1.Q2","I1.Q3","I1.Q4","I1.Q5","I1.QTM","I1.QTD1","I1.QTD2","I1.QTL","I1.QTU","I1.QT","I1.Q6","I1.Q7","I1.Q8","I1.Q9","I1.Q10",
                "I2.Q1","I2.Q2","I2.Q3","I2.Q4","I2.Q5","I2.QTM","I2.QTD1","I2.QTD2","I2.QTL","I2.QTU","I2.QT","I2.Q6","I2.Q7","I2.Q8","I2.Q9","I2.Q10",
                "I3.Q1","I3.Q2","I3.Q3","I3.Q4","I3.Q5","I3.QTM","I3.QTD1","I3.QTD2","I3.QTL","I3.QTU","I3.QT","I3.Q6","I3.Q7","I3.Q8","I3.Q9","I3.Q10",
                "I4.Q1","I4.Q2","I4.Q3","I4.Q4","I4.Q5","I4.QTM","I4.QTD1","I4.QTD2","I4.QTL","I4.QTU","I4.QT","I4.Q6","I4.Q7","I4.Q8","I4.Q9","I4.Q10",
                "I5.Q1","I5.Q2","I5.Q3","I5.Q4","I5.Q5","I5.QTM","I5.QTD1","I5.QTD2","I5.QTL","I5.QTU","I5.QT","I5.Q6","I5.Q7","I5.Q8","I5.Q9","I5.Q10",
                "I6.Q1","I6.Q2","I6.Q3","I6.Q4","I6.Q5","I6.QTM","I6.QTD1","I6.QTD2","I6.QTL","I6.QTU","I6.QT","I6.Q6","I6.Q7","I6.Q8","I6.Q9","I6.Q10",
                "I7.Q1","I7.Q2","I7.Q3","I7.Q4","I7.Q5","I7.QTM","I7.QTD1","I7.QTD2","I7.QTL","I7.QTU","I7.QT","I7.Q6","I7.Q7","I7.Q8","I7.Q9","I7.Q10",
                "I8.Q1","I8.Q2","I8.Q3","I8.Q4","I8.Q5","I8.QTM","I8.QTD1","I8.QTD2","I8.QTL","I8.QTU","I8.QT","I8.Q6","I8.Q7","I8.Q8","I8.Q9","I8.Q10",
                "I9.Q1","I9.Q2","I9.Q3","I9.Q4","I9.Q5","I9.QTM","I9.QTD1","I9.QTD2","I9.QTL","I9.QTU","I9.QT","I9.Q6","I9.Q7","I9.Q8","I9.Q9","I9.Q10",
                "I10.Q1","I10.Q2","I10.Q3","I10.Q4","I10.Q5","I10.QTM","I10.QTD1","I10.QTD2","I10.QTL","I10.QTU","I10.QT","I10.Q6","I10.Q7","I10.Q8","I10.Q9","I10.Q10",
                "I11.Q1","I11.Q2","I11.Q3","I11.Q4","I11.Q5","I11.QTM","I11.QTD1","I11.QTD2","I11.QTL","I11.QTU","I11.QT","I11.Q6","I11.Q7","I11.Q8","I11.Q9","I11.Q10",
                "I12.Q1","I12.Q2","I12.Q3","I12.Q4","I12.Q5","I12.QTM","I12.QTD1","I12.QTD2","I12.QTL","I12.QTU","I12.QT","I12.Q6","I12.Q7","I12.Q8","I12.Q9","I12.Q10",
                "I13.Q1","I13.Q2","I13.Q3","I13.Q4","I13.Q5","I13.QTM","I13.QTD1","I13.QTD2","I13.QTL","I13.QTU","I13.QT","I13.Q6","I13.Q7","I13.Q8","I13.Q9","I13.Q10",
                "I14.Q1","I14.Q2","I14.Q3","I14.Q4","I14.Q5","I14.QTM","I14.QTD1","I14.QTD2","I14.QTL","I14.QTU","I14.QT","I14.Q6","I14.Q7","I14.Q8","I14.Q9","I14.Q10",
                "I15.Q1","I15.Q2","I15.Q3","I15.Q4","I15.Q5","I15.QTM","I15.QTD1","I15.QTD2","I15.QTL","I15.QTU","I15.QT","I15.Q6","I15.Q7","I15.Q8","I15.Q9","I15.Q10",
                "I16.Q1","I16.Q2","I16.Q3","I16.Q4","I16.Q5","I16.QTM","I16.QTD1","I16.QTD2","I16.QTL","I16.QTU","I16.QT","I16.Q6","I16.Q7","I16.Q8","I16.Q9","I16.Q10",
                "I17.Q1","I17.Q2","I17.Q3","I17.Q4","I17.Q5","I17.QTM","I17.QTD1","I17.QTD2","I17.QTL","I17.QTU","I17.QT","I17.Q6","I17.Q7","I17.Q8","I17.Q9","I17.Q10",
                "I18.Q1", "I18.Q2", "I18.Q3", "I18.Q4", "I18.Q5", "I18.QTM", "I18.QTD1", "I18.QTD2", "I18.QTL", "I18.QTU", "I18.QT","I18.Q6","I18.Q7","I18.Q8","I18.Q9","I18.Q10",
                "I19.Q1", "I19.Q2", "I19.Q3", "I19.Q4", "I19.Q5", "I19.QTM", "I19.QTD1", "I19.QTD2", "I19.QTL", "I19.QTU", "I19.QT","I19.Q6","I19.Q7","I19.Q8","I19.Q9","I19.Q10",
                "I20.Q1", "I20.Q2", "I20.Q3",  "I20.Q4", "I20.Q5", "I20.QTM", "I20.QTD1", "I20.QTD2", "I20.QTL", "I20.QTU", "I20.QT","I20.Q6","I20.Q7","I20.Q8","I20.Q9","I20.Q10"
                };

        #endregion
        #region "m and e props"
        //list of indicators 
        public List<ME2Indicator> ME2Indicators = new List<ME2Indicator>();
        //maximum limit for reasonable serialization (zero based, so 15 + score)
        private int MaximumNumberOfME2Indicators = 15;
        //base io to update
        public enum BASEIO_TYPES
        {
            none = 0,
            quantity = 1,
            times = 2,
            ocprice = 3,
            aohprice = 4,
            capprice = 5,
            benprice = 6,
            composquantity = 7,
        }
        //name of indicator 1
        public string IndName { get; set; }
        //description
        public string IndDescription { get; set; }
        //url
        public string IndURL { get; set; }
        //aggregation label
        public string IndLabel { get; set; }
        //RUC_TYPES or distribution enum
        public string IndType { get; set; }
        //date of indicator measurement
        public DateTime IndDate { get; set; }
        //algorithm1 = basic stats ...
        public string IndMathType { get; set; }
        //amount
        public double Ind1Amount { get; set; }
        public string Ind1Unit { get; set; }
        //second quantity
        public double Ind2Amount { get; set; }
        //second unit
        public string Ind2Unit { get; set; }
        //third quantity
        public double Ind3Amount { get; set; }
        public string Ind3Unit { get; set; }
        public double Ind4Amount { get; set; }
        public string Ind4Unit { get; set; }
        //total of the two indicators (p*q = cost)
        public double Ind5Amount { get; set; }
        //unit for total (i.e. hours physical activity, cost, benefit, number (stock groups)
        public string Ind5Unit { get; set; }
        //related indicator label i.e. emissions and env performance
        public string IndRelLabel { get; set; }
        public double IndTAmount { get; set; }
        public string IndTUnit { get; set; }
        public double IndTD1Amount { get; set; }
        public string IndTD1Unit { get; set; }
        public double IndTD2Amount { get; set; }
        public string IndTD2Unit { get; set; }
        public string IndMathResult { get; set; }
        public string IndMathSubType { get; set; }

        public double IndTMAmount { get; set; }
        public string IndTMUnit { get; set; }
        public double IndTLAmount { get; set; }
        public string IndTLUnit { get; set; }
        public double IndTUAmount { get; set; }
        public string IndTUUnit { get; set; }
        public string IndMathOperator { get; set; }
        public string IndMathExpression { get; set; }
        public string IndBaseIO { get; set; }
        //the following 3 props are equiv to Resource Stock Calc Score props
        public int IndIterations { get; set; }
        public int IndCILevel { get; set; }
        public int IndRandom { get; set; }

        //these also need the indicator index appended
        public const string cIndName = "IndName";
        public const string cIndDescription = "IndDescription";
        public const string cIndURL = "IndURL";
        public const string cIndLabel = "IndLabel";
        public const string cIndType = "IndType";
        public const string cIndDate = "IndDate";
        public const string cIndMathType = "IndMathType";
        public const string cInd1Amount = "Ind1Amount";
        public const string cInd1Unit = "Ind1Unit";
        public const string cInd2Amount = "Ind2Amount";
        public const string cInd2Unit = "Ind2Unit";
        public const string cInd3Amount = "Ind3Amount";
        public const string cInd3Unit = "Ind3Unit";
        public const string cInd4Amount = "Ind4Amount";
        public const string cInd4Unit = "Ind4Unit";
        public const string cInd5Amount = "Ind5Amount";
        public const string cInd5Unit = "Ind5Unit";
        public const string cIndRelLabel = "IndRelLabel";
        public const string cIndTAmount = "IndTAmount";
        public const string cIndTUnit = "IndTUnit";
        public const string cIndTD1Amount = "IndTD1Amount";
        public const string cIndTD1Unit = "IndTD1Unit";
        public const string cIndTD2Amount = "IndTD2Amount";
        public const string cIndTD2Unit = "IndTD2Unit";
        public const string cIndMathResult = "IndMathResult";
        public const string cIndMathSubType = "IndMathSubType";

        public const string cIndTMAmount = "IndTMAmount";
        public const string cIndTMUnit = "IndTMUnit";
        public const string cIndTLAmount = "IndTLAmount";
        public const string cIndTLUnit = "IndTLUnit";
        public const string cIndTUAmount = "IndTUAmount";
        public const string cIndTUUnit = "IndTUUnit";
        public const string cIndMathOperator = "IndMathOperator";
        public const string cIndMathExpression = "IndMathExpression";
        public const string cIndBaseIO = "IndBaseIO";
        public const string cIndIterations = "IndIterations";
        public const string cIndCILevel = "IndCILevel";
        public const string cIndRandom = "IndRandom";

        public virtual void InitME2IndicatorsProperties()
        {
            if (ME2Indicators == null)
            {
                ME2Indicators = new List<ME2Indicator>();
            }
            foreach (ME2Indicator ind in ME2Indicators)
            {
                InitME2IndicatorProperties(ind);
            }
        }
        private void InitME2IndicatorProperties(ME2Indicator ind)
        {
            ind.IndDescription = string.Empty;
            ind.IndURL = string.Empty;
            ind.IndName = string.Empty;
            ind.IndLabel = string.Empty;
            ind.IndType = RUC_TYPES.none.ToString();
            ind.IndRelLabel = string.Empty;
            ind.IndTAmount = 0;
            ind.IndTUnit = string.Empty;
            ind.IndTD1Amount = 0;
            ind.IndTD1Unit = string.Empty;
            ind.IndTD2Amount = 0;
            ind.IndTD2Unit = string.Empty;
            ind.IndMathResult = string.Empty;
            ind.IndMathSubType = Constants.NONE;

            ind.IndTMAmount = 0;
            ind.IndTMUnit = string.Empty;
            ind.IndTLAmount = 0;
            ind.IndTLUnit = string.Empty;
            ind.IndTUAmount = 0;
            ind.IndTUUnit = string.Empty;
            ind.IndMathOperator = MATH_OPERATOR_TYPES.none.ToString();
            ind.IndMathExpression = string.Empty;
            ind.IndBaseIO = string.Empty;
            ind.IndDate = CalculatorHelpers.GetDateShortNow();
            ind.IndMathType = MATH_TYPES.none.ToString();
            ind.Ind1Amount = 0;
            ind.Ind1Unit = string.Empty;
            ind.Ind2Amount = 0;
            ind.Ind2Unit = string.Empty;
            ind.Ind5Amount = 0;
            ind.Ind5Unit = string.Empty;
            ind.Ind3Amount = 0;
            ind.Ind3Unit = string.Empty;
            ind.Ind4Amount = 0;
            ind.Ind4Unit = string.Empty;
            ind.IndIterations = 0;
            ind.IndCILevel = 0;
            ind.IndRandom = 0;
        }
        public virtual void CopyME2IndicatorsProperties(
            ME2Indicator calculator)
        {
            if (calculator.ME2Indicators != null)
            {
                ME2Indicators = new List<ME2Indicator>();
                foreach (ME2Indicator calculatorInd in calculator.ME2Indicators)
                {
                    ME2Indicator ind = new ME2Indicator();
                    CopyME2IndicatorProperties(ind, calculatorInd);
                    ME2Indicators.Add(ind);
                }
                Observations = ME2Indicators.Count;
            }
        }
        private void CopyME2IndicatorProperties(
            ME2Indicator ind, ME2Indicator calculator)
        {
            ind.IndDescription = calculator.IndDescription;
            ind.IndURL = calculator.IndURL;
            ind.IndName = calculator.IndName;
            ind.IndLabel = calculator.IndLabel;
            ind.IndType = calculator.IndType;
            ind.IndRelLabel = calculator.IndRelLabel;
            ind.IndTAmount = calculator.IndTAmount;
            ind.IndTUnit = calculator.IndTUnit;
            ind.IndTD1Amount = calculator.IndTD1Amount;
            ind.IndTD1Unit = calculator.IndTD1Unit;
            ind.IndTD2Amount = calculator.IndTD2Amount;
            ind.IndTD2Unit = calculator.IndTD2Unit;
            ind.IndMathResult = calculator.IndMathResult;
            ind.IndMathSubType = calculator.IndMathSubType;

            ind.IndTMAmount = calculator.IndTMAmount;
            ind.IndTMUnit = calculator.IndTMUnit;
            ind.IndTLAmount = calculator.IndTLAmount;
            ind.IndTLUnit = calculator.IndTLUnit;
            ind.IndTUAmount = calculator.IndTUAmount;
            ind.IndTUUnit = calculator.IndTUUnit;
            ind.IndMathOperator = calculator.IndMathOperator;
            ind.IndMathExpression = calculator.IndMathExpression;
            ind.IndBaseIO = calculator.IndBaseIO;
            ind.IndDate = calculator.IndDate;
            ind.IndMathType = calculator.IndMathType;
            ind.Ind1Amount = calculator.Ind1Amount;
            ind.Ind1Unit = calculator.Ind1Unit;
            ind.Ind2Amount = calculator.Ind2Amount;
            ind.Ind2Unit = calculator.Ind2Unit;
            ind.Ind5Amount = calculator.Ind5Amount;
            ind.Ind5Unit = calculator.Ind5Unit;
            ind.Ind3Amount = calculator.Ind3Amount;
            ind.Ind3Unit = calculator.Ind3Unit;
            ind.Ind4Amount = calculator.Ind4Amount;
            ind.Ind4Unit = calculator.Ind4Unit;
            ind.IndIterations = calculator.IndIterations;
            ind.IndCILevel = calculator.IndCILevel;
            ind.IndRandom = calculator.IndRandom;
        }
        public virtual void SetME2IndicatorsProperties(XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name 
            //SetCalculatorProperties(calculator);
            if (ME2Indicators == null)
            {
                ME2Indicators = new List<ME2Indicator>();
            }
            //maxnumberofinds + score = 16 and finish with score 
            //score has index i = 0 because may want to add more inds in future
            int i = 0;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 0; i < MaximumNumberOfME2Indicators; i++)
            {
                //order will start with zero
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cIndName, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute)
                    && sHasAttribute != Constants.NONE)
                {
                    //name and label are needed
                    sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cIndLabel, sAttNameExtension));
                    if (!string.IsNullOrEmpty(sHasAttribute)
                        && sHasAttribute != Constants.NONE)
                    {
                        ME2Indicator ind1 = new ME2Indicator();
                        SetME2IndicatorProperties(ind1, sAttNameExtension, calculator);
                        ME2Indicators.Add(ind1);
                    }
                }
                sHasAttribute = string.Empty;
            }
            if (ME2Indicators.Count < 20)
            {
                //zero based index means 21 inds needed (to prevent null exceptions with inds 16 to 20)
                for (i = ME2Indicators.Count; i <= 20; i++)
                {
                    //many conditional causes use 15 indicators regardless of MaximumNumberOfME2Indicators
                    //prevent null errors
                    ME2Indicator ind1 = new ME2Indicator();
                    ME2Indicators.Add(ind1);
                }
            }
        }
        private void SetME2IndicatorProperties(ME2Indicator ind, string attNameExtension,
            XElement calculator)
        {
            //set ind's properties
            ind.IndDescription = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndDescription, attNameExtension));
            ind.IndURL = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(cIndURL, attNameExtension));
            ind.IndName = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndName, attNameExtension));
            ind.IndLabel = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndLabel, attNameExtension));
            ind.IndType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndType, attNameExtension));
            ind.Ind1Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cInd1Amount, attNameExtension));
            ind.Ind1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cInd1Unit, attNameExtension));
            ind.IndRelLabel = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndRelLabel, attNameExtension));
            ind.IndTAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTAmount, attNameExtension));
            ind.IndTUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndTUnit, attNameExtension));
            ind.IndTD1Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTD1Amount, attNameExtension));
            ind.IndTD1Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndTD1Unit, attNameExtension));
            ind.IndTD2Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTD2Amount, attNameExtension));
            ind.IndTD2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndTD2Unit, attNameExtension));
            ind.IndMathResult = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndMathResult, attNameExtension));
            ind.IndMathSubType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndMathSubType, attNameExtension));

            ind.IndTMAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTMAmount, attNameExtension));
            ind.IndTMUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndTMUnit, attNameExtension));
            ind.IndTLAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTLAmount, attNameExtension));
            ind.IndTLUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndTLUnit, attNameExtension));
            ind.IndTUAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cIndTUAmount, attNameExtension));
            ind.IndTUUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndTUUnit, attNameExtension));
            ind.IndMathOperator = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndMathOperator, attNameExtension));
            ind.IndMathExpression = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndMathExpression, attNameExtension));
            ind.IndBaseIO = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndBaseIO, attNameExtension));
            ind.IndDate = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cIndDate, attNameExtension));
            ind.IndMathType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cIndMathType, attNameExtension));
            ind.Ind2Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cInd2Amount, attNameExtension));
            ind.Ind2Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cInd2Unit, attNameExtension));
            ind.Ind5Amount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cInd5Amount, attNameExtension));
            ind.Ind5Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cInd5Unit, attNameExtension));
            ind.Ind3Amount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cInd3Amount, attNameExtension));
            ind.Ind3Unit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cInd3Unit, attNameExtension));
            ind.Ind4Amount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cInd4Amount, attNameExtension));
            ind.Ind4Unit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cInd4Unit, attNameExtension));
            ind.IndIterations = CalculatorHelpers.GetAttributeInt(calculator,
               string.Concat(cIndIterations, attNameExtension));
            ind.IndCILevel = CalculatorHelpers.GetAttributeInt(calculator,
               string.Concat(cIndCILevel, attNameExtension));
            ind.IndRandom = CalculatorHelpers.GetAttributeInt(calculator,
               string.Concat(cIndRandom, attNameExtension));
        }
        public virtual void SetME2IndicatorsProperty(string attName,
           string attValue, int colIndex)
        {
            if (ME2Indicators == null)
            {
                ME2Indicators = new List<ME2Indicator>();
            }
            if (ME2Indicators.Count < (colIndex + 1))
            {
                ME2Indicator ind1 = new ME2Indicator();
                ME2Indicators.Insert(colIndex, ind1);
            }
            ME2Indicator ind = ME2Indicators.ElementAt(colIndex);
            if (ind != null)
            {
                SetME2IndicatorProperty(ind, attName, attValue);
            }
        }
        private void SetME2IndicatorProperty(ME2Indicator ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cIndDescription:
                    ind.IndDescription = attValue;
                    break;
                case cIndURL:
                    ind.IndURL = attValue;
                    break;
                case cIndName:
                    ind.IndName = attValue;
                    break;
                case cIndLabel:
                    ind.IndLabel = attValue;
                    break;
                case cIndType:
                    ind.IndType = attValue;
                    break;
                case cIndRelLabel:
                    ind.IndRelLabel = attValue;
                    break;
                case cIndTAmount:
                    ind.IndTAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndTUnit:
                    ind.IndTUnit = attValue;
                    break;
                case cIndTD1Amount:
                    ind.IndTD1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndTD1Unit:
                    ind.IndTD1Unit = attValue;
                    break;
                case cIndTD2Amount:
                    ind.IndTD2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndTD2Unit:
                    ind.IndTD2Unit = attValue;
                    break;
                case cIndMathResult:
                    ind.IndMathResult = attValue;
                    break;
                case cIndMathSubType:
                    ind.IndMathSubType = attValue;
                    break;
                case cIndTMAmount:
                    ind.IndTMAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndTMUnit:
                    ind.IndTMUnit = attValue;
                    break;
                case cIndTLAmount:
                    ind.IndTLAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndTLUnit:
                    ind.IndTLUnit = attValue;
                    break;
                case cIndTUAmount:
                    ind.IndTUAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cIndTUUnit:
                    ind.IndTUUnit = attValue;
                    break;
                case cIndMathOperator:
                    ind.IndMathOperator = attValue;
                    break;
                case cIndMathExpression:
                    ind.IndMathExpression = attValue;
                    break;
                case cIndBaseIO:
                    ind.IndBaseIO = attValue;
                    break;
                case cIndDate:
                    ind.IndDate = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cIndMathType:
                    ind.IndMathType = attValue;
                    break;
                case cInd1Amount:
                    ind.Ind1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd1Unit:
                    ind.Ind1Unit = attValue;
                    break;
                case cInd2Amount:
                    ind.Ind2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd2Unit:
                    ind.Ind2Unit = attValue;
                    break;
                case cInd5Amount:
                    ind.Ind5Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd5Unit:
                    ind.Ind5Unit = attValue;
                    break;
                case cInd3Amount:
                    ind.Ind3Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd3Unit:
                    ind.Ind3Unit = attValue;
                    break;
                case cInd4Amount:
                    ind.Ind4Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cInd4Unit:
                    ind.Ind4Unit = attValue;
                    break;
                case cIndIterations:
                    ind.IndIterations = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cIndCILevel:
                    ind.IndCILevel = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cIndRandom:
                    ind.IndRandom = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetME2IndicatorsProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (ME2Indicators.Count >= (colIndex + 1))
            {
                ME2Indicator ind = ME2Indicators.ElementAt(colIndex);
                if (ind != null)
                {
                    sPropertyValue = GetME2IndicatorProperty(ind, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetME2IndicatorProperty(ME2Indicator ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cIndDescription:
                    sPropertyValue = ind.IndDescription;
                    break;
                case cIndURL:
                    sPropertyValue = ind.IndURL;
                    break;
                case cIndName:
                    sPropertyValue = ind.IndName;
                    break;
                case cIndType:
                    sPropertyValue = ind.IndType;
                    break;
                case cIndLabel:
                    sPropertyValue = ind.IndLabel;
                    break;
                case cIndRelLabel:
                    sPropertyValue = ind.IndRelLabel;
                    break;
                case cIndTAmount:
                    sPropertyValue = ind.IndTAmount.ToString();
                    break;
                case cIndTUnit:
                    sPropertyValue = ind.IndTUnit.ToString();
                    break;
                case cIndTD1Amount:
                    sPropertyValue = ind.IndTD1Amount.ToString();
                    break;
                case cIndTD1Unit:
                    sPropertyValue = ind.IndTD1Unit.ToString();
                    break;
                case cIndTD2Amount:
                    sPropertyValue = ind.IndTD2Amount.ToString();
                    break;
                case cIndTD2Unit:
                    sPropertyValue = ind.IndTD2Unit.ToString();
                    break;
                case cIndMathResult:
                    sPropertyValue = ind.IndMathResult.ToString();
                    break;
                case cIndMathSubType:
                    sPropertyValue = ind.IndMathSubType.ToString();
                    break;
                case cIndTMAmount:
                    sPropertyValue = ind.IndTMAmount.ToString();
                    break;
                case cIndTMUnit:
                    sPropertyValue = ind.IndTMUnit.ToString();
                    break;
                case cIndTLAmount:
                    sPropertyValue = ind.IndTLAmount.ToString();
                    break;
                case cIndTLUnit:
                    sPropertyValue = ind.IndTLUnit.ToString();
                    break;
                case cIndTUAmount:
                    sPropertyValue = ind.IndTUAmount.ToString();
                    break;
                case cIndTUUnit:
                    sPropertyValue = ind.IndTUUnit.ToString();
                    break;
                case cIndMathOperator:
                    sPropertyValue = ind.IndMathOperator.ToString();
                    break;
                case cIndMathExpression:
                    sPropertyValue = ind.IndMathExpression.ToString();
                    break;
                case cIndBaseIO:
                    sPropertyValue = ind.IndBaseIO.ToString();
                    break;
                case cIndDate:
                    sPropertyValue = ind.IndDate.ToString();
                    break;
                case cIndMathType:
                    sPropertyValue = ind.IndMathType;
                    break;
                case cInd1Amount:
                    sPropertyValue = ind.Ind1Amount.ToString();
                    break;
                case cInd1Unit:
                    sPropertyValue = ind.Ind1Unit.ToString();
                    break;
                case cInd2Amount:
                    sPropertyValue = ind.Ind2Amount.ToString();
                    break;
                case cInd2Unit:
                    sPropertyValue = ind.Ind2Unit;
                    break;
                case cInd5Amount:
                    sPropertyValue = ind.Ind5Amount.ToString();
                    break;
                case cInd5Unit:
                    sPropertyValue = ind.Ind5Unit.ToString();
                    break;
                case cInd3Amount:
                    sPropertyValue = ind.Ind3Amount.ToString();
                    break;
                case cInd3Unit:
                    sPropertyValue = ind.Ind3Unit;
                    break;
                case cInd4Amount:
                    sPropertyValue = ind.Ind4Amount.ToString();
                    break;
                case cInd4Unit:
                    sPropertyValue = ind.Ind4Unit;
                    break;
                case cIndIterations:
                    sPropertyValue = ind.IndIterations.ToString();
                    break;
                case cIndCILevel:
                    sPropertyValue = ind.IndCILevel.ToString();
                    break;
                case cIndRandom:
                    sPropertyValue = ind.IndRandom.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetME2IndicatorsAttributes(ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. SetCalculatorAttributes(calculator);
            if (ME2Indicators != null)
            {
                int i = 0;
                string sAttNameExtension = string.Empty;
                foreach (ME2Indicator ind in ME2Indicators)
                {
                    sAttNameExtension = i.ToString();
                    SetME2IndicatorAttributes(ind, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetME2IndicatorAttributes(ME2Indicator ind, string attNameExtension,
            ref XElement calculator)
        {
            //don't needlessly add these to linkedviews if they are not being used
            if ((!string.IsNullOrEmpty(ind.IndName) && ind.IndName != Constants.NONE)
                && (!string.IsNullOrEmpty(ind.IndLabel) && ind.IndLabel != Constants.NONE))
            {
                //remember that the calculator inheriting from this class must set id and name atts
                //and remove unwanted old atts i.e. SetCalculatorAttributes(calculator);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cIndName, attNameExtension),ind.IndName);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cIndLabel, attNameExtension),ind.IndLabel);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndDescription, attNameExtension),ind.IndDescription);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndURL, attNameExtension),ind.IndURL);
                CalculatorHelpers.SetAttributeDateS(calculator,
                       string.Concat(cIndDate, attNameExtension),ind.IndDate);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndMathType, attNameExtension),ind.IndMathType);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndType, attNameExtension),ind.IndType);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndRelLabel, attNameExtension),ind.IndRelLabel);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cIndTAmount, attNameExtension),ind.IndTAmount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndTUnit, attNameExtension),ind.IndTUnit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cIndTD1Amount, attNameExtension),ind.IndTD1Amount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndTD1Unit, attNameExtension),ind.IndTD1Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cIndTD2Amount, attNameExtension),ind.IndTD2Amount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndTD2Unit, attNameExtension),ind.IndTD2Unit);
                CalculatorHelpers.SetAttribute(calculator,
                      string.Concat(cIndMathResult, attNameExtension),ind.IndMathResult);
                CalculatorHelpers.SetAttribute(calculator,
                      string.Concat(cIndMathSubType, attNameExtension),ind.IndMathSubType);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cIndTMAmount, attNameExtension),ind.IndTMAmount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndTMUnit, attNameExtension),ind.IndTMUnit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cIndTLAmount, attNameExtension),ind.IndTLAmount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndTLUnit, attNameExtension),ind.IndTLUnit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cIndTUAmount, attNameExtension),ind.IndTUAmount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndTUUnit, attNameExtension),ind.IndTUUnit);
                CalculatorHelpers.SetAttribute(calculator,
                      string.Concat(cIndMathOperator, attNameExtension),ind.IndMathOperator);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndMathExpression, attNameExtension),ind.IndMathExpression);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cIndBaseIO, attNameExtension),ind.IndBaseIO);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                   string.Concat(cInd5Amount, attNameExtension),ind.Ind5Amount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cInd5Unit, attNameExtension),ind.Ind5Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cInd1Amount, attNameExtension),ind.Ind1Amount);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cInd1Unit, attNameExtension),ind.Ind1Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cInd2Amount, attNameExtension),ind.Ind2Amount);
                CalculatorHelpers.SetAttribute(calculator,
                   string.Concat(cInd2Unit, attNameExtension),ind.Ind2Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                      string.Concat(cInd3Amount, attNameExtension),ind.Ind3Amount);
                CalculatorHelpers.SetAttribute(calculator,
                   string.Concat(cInd3Unit, attNameExtension),ind.Ind3Unit);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                      string.Concat(cInd4Amount, attNameExtension),ind.Ind4Amount);
                CalculatorHelpers.SetAttribute(calculator,
                   string.Concat(cInd4Unit, attNameExtension),ind.Ind4Unit);
                CalculatorHelpers.SetAttributeInt(calculator,
                      string.Concat(cIndIterations, attNameExtension), ind.IndIterations);
                CalculatorHelpers.SetAttributeInt(calculator,
                      string.Concat(cIndCILevel, attNameExtension), ind.IndCILevel);
                CalculatorHelpers.SetAttributeInt(calculator,
                      string.Concat(cIndRandom, attNameExtension), ind.IndRandom);
            }
        }
        public virtual void SetME2IndicatorsAttributes(ref XmlWriter writer)
        {
            if (ME2Indicators != null)
            {
                int i = 0;
                string sAttNameExtension = string.Empty;
                foreach (ME2Indicator ind in ME2Indicators)
                {
                    sAttNameExtension = i.ToString();
                    SetME2IndicatorAttributes(ind, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public virtual void SetME2IndicatorAttributes(ME2Indicator ind, string attNameExtension,
           ref XmlWriter writer)
        {
            if ((!string.IsNullOrEmpty(ind.IndName) && ind.IndName != Constants.NONE)
                && (!string.IsNullOrEmpty(ind.IndLabel) && ind.IndLabel != Constants.NONE))
            {
                writer.WriteAttributeString(
                    string.Concat(cIndName, attNameExtension),ind.IndName);
                writer.WriteAttributeString(
                   string.Concat(cIndDescription, attNameExtension),ind.IndDescription);
                writer.WriteAttributeString(
                   string.Concat(cIndURL, attNameExtension),ind.IndURL);
                writer.WriteAttributeString(
                    string.Concat(cIndLabel, attNameExtension),ind.IndLabel);
                writer.WriteAttributeString(
                    string.Concat(cIndType, attNameExtension),ind.IndType);
                writer.WriteAttributeString(
                   string.Concat(cIndRelLabel, attNameExtension),ind.IndRelLabel);
                writer.WriteAttributeString(
                   string.Concat(cIndTAmount, attNameExtension),ind.IndTAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cIndTUnit, attNameExtension),ind.IndTUnit.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndTD1Amount, attNameExtension),ind.IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cIndTD1Unit, attNameExtension),ind.IndTD1Unit.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndTD2Amount, attNameExtension),ind.IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cIndTD2Unit, attNameExtension),ind.IndTD2Unit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cIndMathResult, attNameExtension),ind.IndMathResult.ToString());
                writer.WriteAttributeString(
                    string.Concat(cIndMathSubType, attNameExtension),ind.IndMathSubType.ToString());

                writer.WriteAttributeString(
                   string.Concat(cIndTMAmount, attNameExtension),ind.IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cIndTMUnit, attNameExtension),ind.IndTMUnit.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndTLAmount, attNameExtension),ind.IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cIndTLUnit, attNameExtension),ind.IndTLUnit.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndTUAmount, attNameExtension),ind.IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cIndTUUnit, attNameExtension),ind.IndTUUnit.ToString());
                writer.WriteAttributeString(
                 string.Concat(cIndMathOperator, attNameExtension),ind.IndMathOperator.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndMathExpression, attNameExtension),ind.IndMathExpression.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndBaseIO, attNameExtension),ind.IndBaseIO.ToString());
                writer.WriteAttributeString(
                    string.Concat(cIndDate, attNameExtension),ind.IndDate.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                    string.Concat(cIndMathType, attNameExtension),ind.IndMathType);
                writer.WriteAttributeString(
                    string.Concat(cInd1Amount, attNameExtension),ind.Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cInd1Unit, attNameExtension),ind.Ind1Unit.ToString());
                writer.WriteAttributeString(
                    string.Concat(cInd2Amount, attNameExtension),ind.Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cInd2Unit, attNameExtension),ind.Ind2Unit);
                writer.WriteAttributeString(
                   string.Concat(cInd5Amount, attNameExtension),ind.Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cInd5Unit, attNameExtension),ind.Ind5Unit.ToString());
                writer.WriteAttributeString(
                   string.Concat(cInd3Amount, attNameExtension),ind.Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cInd3Unit, attNameExtension),ind.Ind3Unit);
                writer.WriteAttributeString(
                   string.Concat(cInd4Amount, attNameExtension),ind.Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                   string.Concat(cInd4Unit, attNameExtension),ind.Ind4Unit);
                writer.WriteAttributeString(
                   string.Concat(cIndIterations, attNameExtension), ind.IndIterations.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndCILevel, attNameExtension), ind.IndCILevel.ToString());
                writer.WriteAttributeString(
                   string.Concat(cIndRandom, attNameExtension), ind.IndRandom.ToString());
            }
        }
        #endregion
        //run the calculations
        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            bHasCalculations = SetCalculations();
            return bHasCalculations;
        }
        private bool SetCalculations()
        {
            bool bHasCalcs = false;
            //score, or zero-based index, comes last -it uses the results of indicators
            int iIndicatorNumber = 1;
            Task<bool> bHasCalculations = SetCalculationsAsync(iIndicatorNumber);
            if (bHasCalculations.IsCompleted)
                bHasCalcs = true;
            return bHasCalcs;
        }
        public async Task<bool> SetCalculationsAsync(int indicatorNumber)
        {
            bool bHasCalculations = false;
            bool bHasError = false;
            Task<string>[] runAlgosTasks = new Task<string>[] { };
            if (indicatorNumber > 20)
            {
                return true;
            }
            //rule enforced that at least 1 indicator is needed to run calcs
            if (ME2Indicators.Count == 0)
            {
                CalculatorDescription += string.Concat(Errors.MakeStandardErrorMsg("INDICATORS_BAD"), CalculatorDescription);
                return true;
            }
            //don't want an error to interfere with running subsequent calcs
            //so catch uses an indicator number to keep going
            try
            {
                if (this != null)
                {
                    //clear out the mathresult to hold error messages and math
                    string sScoreResult = ME2Indicators[0].IndMathResult;
                    if (!sScoreResult.StartsWith("http"))
                    {
                        ME2Indicators[0].IndMathResult = string.Empty;
                    }
                    //if the datasets generate an error, try catch moves to next indicator
                    if (indicatorNumber == 1)
                    {
                        if (HasMathType(MATH_TYPES.algorithm5, MATH_SUBTYPES.subalgorithm1))
                        {
                            //properties filled in manually
                            ME2Indicators[0].IndMathResult = sScoreResult;
                            _indicators = GetIndicatorsDisplay();
                        }
                        if (HasDataMatrix())
                        {
                            string[] dataURLs = new string[] { };
                            //v192: changed this from an if else clause 
                            //to allow sibling calcs to run different algos (same as CalculatedInds)
                            if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
                            {
                                //these algos must have data urls
                                if (!string.IsNullOrEmpty(DataURL)
                                    && (DataURL != Constants.NONE))
                                {
                                    //these use numeric double datasets
                                    dataURLs = DataURL.Split(Constants.STRING_DELIMITERS);
                                    // Create a query. 
                                    IEnumerable<Task<string>> runAlgosTasksQuery =
                                        from dataURL in dataURLs select ProcessAlgosAsync(dataURL);
                                    // Use ToArray to execute the query and start the download tasks.
                                    runAlgosTasks = runAlgosTasksQuery.ToArray();
                                    //return the indicators
                                    string[] indicatorscsvs = await Task.WhenAll(runAlgosTasks);
                                    _indicators = GetIndicators(indicatorscsvs);
                                    bHasCalculations = true;
                                }
                                else
                                {
                                    //missing correlation matrix 
                                    CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_MISSING"));
                                }
                            }
                            if (HasMathType(MATH_TYPES.algorithm2)
                                || HasMathType(MATH_TYPES.algorithm3)
                                || HasMathType(MATH_TYPES.algorithm4))
                            {
                                //these algos must have data urls
                                if (!string.IsNullOrEmpty(DataURL)
                                    && (DataURL != Constants.NONE))
                                {
                                    //string data formats allow urls, text, categorical names 
                                    dataURLs = DataURL.Split(Constants.STRING_DELIMITERS);
                                    //stores rproject files in jdataurl
                                    //set up a list of tasks to run
                                    List<Task<string>> runTasks = new List<Task<string>>();
                                    for (int i = 0; i < dataURLs.Count(); i++)
                                    {
                                        //i corresponds to jointdataurl index
                                        //add the tasks to the collection
                                        runTasks.Add(ProcessAlgosAsync2(dataURLs[i], i));
                                    }
                                    //return a csv string of indicators when all of the tasks are completed
                                    string[] indicatorscsvs = await Task.WhenAll(runTasks);
                                    _indicators = GetIndicators(indicatorscsvs);
                                    bHasCalculations = true;
                                }
                                else
                                {
                                    //missing correlation matrix 
                                    CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_MISSING"));
                                }
                            }
                            if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5)
                                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7))
                            {
                                //these algos must have joint data urls with standard dataset format
                                if (!string.IsNullOrEmpty(ME2Indicators[0].IndURL)
                                    && (ME2Indicators[0].IndURL != Constants.NONE))
                                {
                                    dataURLs = ME2Indicators[0].IndURL.Split(Constants.STRING_DELIMITERS);
                                    // Create a query. 
                                    IEnumerable<Task<string>> runAlgosTasksQuery =
                                        from dataURL in dataURLs select ProcessAlgosAsync(dataURL);
                                    // Use ToArray to execute the query and start the download tasks.
                                    runAlgosTasks = runAlgosTasksQuery.ToArray();
                                    //return the indicators
                                    string[] indicatorscsvs = await Task.WhenAll(runAlgosTasks);
                                    _indicators = GetIndicators(indicatorscsvs);
                                    bHasCalculations = true;
                                }
                                else
                                {
                                    //missing correlation matrix 
                                    CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_MISSING"));
                                }
                            }
                            if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
                            {
                                //these algos must have joint urls but these are not standard dataset format
                                if (!string.IsNullOrEmpty(ME2Indicators[0].IndURL)
                                    && (ME2Indicators[0].IndURL != Constants.NONE))
                                {
                                    //process the joint data urls
                                    dataURLs = ME2Indicators[0].IndURL.Split(Constants.STRING_DELIMITERS);
                                    //set up a list of tasks to run
                                    List<Task<string>> runTasks = new List<Task<string>>();
                                    for (int i = 0; i < dataURLs.Count(); i++)
                                    {
                                        //i corresponds to jointdataurl index
                                        //add the tasks to the collection
                                        runTasks.Add(ProcessAlgoCorrAsync(dataURLs[i], i));
                                    }
                                    //return a csv string of indicators when all of the tasks are completed
                                    string[] indicatorscsvs = await Task.WhenAll(runTasks);
                                    _indicators = GetIndicators(indicatorscsvs);
                                    bHasCalculations = true;
                                }
                                else
                                {
                                    //missing correlation matrix 
                                    ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("JOINTURL_MISSING"));
                                }
                            }
                            //set descriptive statistics for observed datasets
                            if (!string.IsNullOrEmpty(DataURL)
                                && (DataURL != Constants.NONE))
                            {
                                if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                                {
                                    //198: scores use dataurl to run subalgos 10 and 11
                                    //string sTest = "skip";
                                }
                                else
                                {
                                    dataURLs = DataURL.Split(Constants.STRING_DELIMITERS);
                                    // Create a query. 
                                    IEnumerable<Task<string>> runAlgosTasksQuery =
                                        from dataURL in dataURLs select ProcessAlgosAsync(dataURL);
                                    // Use ToArray to execute the query and start the download tasks.
                                    runAlgosTasks = runAlgosTasksQuery.ToArray();
                                    //return the indicators
                                    string[] indicatorscsvs = await Task.WhenAll(runAlgosTasks);
                                    _indicators = GetIndicators(indicatorscsvs);
                                    bHasCalculations = true;
                                }
                            }
                        }
                        else
                        {
                            //don't use url-based calcs
                        }
                    }
                    //process remaining indicators
                    CalculateIndicators(indicatorNumber);
                    bHasCalculations = true;
                }
                else
                {
                    CalculatorDescription = string.Concat(Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE"), CalculatorDescription);
                    //let them save file and fix faults
                    bHasCalculations = true;
                }
            }
            catch (Exception x)
            {

                //dataurl has to be http or https (has to come from resource base element url)
                if (x.Message.Contains("404"))
                {
                    CalculatorDescription = string.Concat(x.Message, Errors.MakeStandardErrorMsg("DATAURL_BAD"), CalculatorDescription);
                }
                else
                {
                    CalculatorDescription = string.Concat(x.Message, Errors.MakeStandardErrorMsg("DATAURL_BADDATA"), CalculatorDescription);
                }
                //async error messages
                foreach (Task<string> faulted in runAlgosTasks.Where(t => t.IsFaulted))
                {
                    CalculatorDescription += faulted.ToString();
                    //don't process individual indicators
                    indicatorNumber = 20;
                }
                //let them save file and fix faults (or filled in indicator data is lost)
                bHasCalculations = true;
                //let them run calcs for next indicator (or 1 indicator can disrupt all calcs)
                bHasError = true;
            }
            if (bHasError)
            {
                //try to run calcs for next indicator 
                if (indicatorNumber <= 20)
                {

                    int iNextInd = indicatorNumber + 1;
                    bHasCalculations = await SetCalculationsAsync(iNextInd);
                }
            }
            //always return true to save error messages in properties
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool HasDataMatrix()
        {
            bool bHasMatrix = false;
            if (!string.IsNullOrEmpty(DataURL)
                && (DataURL != Constants.NONE))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(ME2Indicators[0].IndURL)
                && (ME2Indicators[0].IndURL != Constants.NONE))
            {
                if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7))
                {
                    return true;
                }
                else if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
                {
                    return true;
                }
            }
            return bHasMatrix;
        }

        public void CopyCalculatorMathToScoreMath()
        {
            //minimal props to run algos from analyzers
            ME2Indicators[0].IndMathExpression = MathExpression;
            ME2Indicators[0].IndMathResult = MathResult;
            ME2Indicators[0].IndMathSubType = MathSubType;
            ME2Indicators[0].IndMathType = MathType;
            ME2Indicators[0].IndCILevel = MathCILevel;
        }
        public bool HasMathType(Calculator1.MATH_TYPES algorithm, Calculator1.MATH_SUBTYPES subAlgorithm)
        {
            bool bHasMathType = false;

            if (ME2Indicators[0].IndMathType == algorithm.ToString()
                && ME2Indicators[0].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[1].IndMathType == algorithm.ToString()
                && ME2Indicators[1].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[2].IndMathType == algorithm.ToString()
                && ME2Indicators[2].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[3].IndMathType == algorithm.ToString()
                && ME2Indicators[3].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[4].IndMathType == algorithm.ToString()
                && ME2Indicators[4].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[5].IndMathType == algorithm.ToString()
                && ME2Indicators[5].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[6].IndMathType == algorithm.ToString()
                && ME2Indicators[6].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[7].IndMathType == algorithm.ToString()
                && ME2Indicators[7].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[8].IndMathType == algorithm.ToString()
                && ME2Indicators[8].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[9].IndMathType == algorithm.ToString()
                && ME2Indicators[9].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[10].IndMathType == algorithm.ToString()
                && ME2Indicators[10].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[11].IndMathType == algorithm.ToString()
                && ME2Indicators[11].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[12].IndMathType == algorithm.ToString()
                && ME2Indicators[12].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[13].IndMathType == algorithm.ToString()
                && ME2Indicators[13].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[14].IndMathType == algorithm.ToString()
                && ME2Indicators[14].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[15].IndMathType == algorithm.ToString()
                && ME2Indicators[15].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[16].IndMathType == algorithm.ToString()
                && ME2Indicators[16].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[17].IndMathType == algorithm.ToString()
                && ME2Indicators[17].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[18].IndMathType == algorithm.ToString()
                && ME2Indicators[18].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[19].IndMathType == algorithm.ToString()
                && ME2Indicators[19].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[20].IndMathType == algorithm.ToString()
                && ME2Indicators[20].IndMathSubType == subAlgorithm.ToString())
            {
                return true;
            }
            return bHasMathType;
        }
        public bool HasMathType(Calculator1.MATH_TYPES algorithm)
        {
            bool bHasMathType = false;

            if (ME2Indicators[0].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[1].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[2].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[3].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[4].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[5].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[6].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[7].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[8].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[9].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[10].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[11].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[12].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[13].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[14].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[15].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[16].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[17].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[18].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[19].IndMathType == algorithm.ToString())
            {
                return true;
            }
            else if (ME2Indicators[20].IndMathType == algorithm.ToString())
            {
                return true;
            }
            return bHasMathType;
        }
        public bool HasMathType(int index, Calculator1.MATH_TYPES algorithm,
            Calculator1.MATH_SUBTYPES subAlgorithm)
        {
            bool bHasMathType = false;

            if (ME2Indicators[0].IndMathType == algorithm.ToString()
                && ME2Indicators[0].IndMathSubType == subAlgorithm.ToString()
                && 0 == index)
            {
                return true;
            }
            else if (ME2Indicators[1].IndMathType == algorithm.ToString()
                && ME2Indicators[1].IndMathSubType == subAlgorithm.ToString()
                && 1 == index)
            {
                return true;
            }
            else if (ME2Indicators[2].IndMathType == algorithm.ToString()
                && ME2Indicators[2].IndMathSubType == subAlgorithm.ToString()
                && 2 == index)
            {
                return true;
            }
            else if (ME2Indicators[3].IndMathType == algorithm.ToString()
                && ME2Indicators[3].IndMathSubType == subAlgorithm.ToString()
                && 3 == index)
            {
                return true;
            }
            else if (ME2Indicators[4].IndMathType == algorithm.ToString()
                && ME2Indicators[4].IndMathSubType == subAlgorithm.ToString()
                && 4 == index)
            {
                return true;
            }
            else if (ME2Indicators[5].IndMathType == algorithm.ToString()
                && ME2Indicators[5].IndMathSubType == subAlgorithm.ToString()
                && 5 == index)
            {
                return true;
            }
            else if (ME2Indicators[6].IndMathType == algorithm.ToString()
                && ME2Indicators[6].IndMathSubType == subAlgorithm.ToString()
                && 6 == index)
            {
                return true;
            }
            else if (ME2Indicators[7].IndMathType == algorithm.ToString()
                && ME2Indicators[7].IndMathSubType == subAlgorithm.ToString()
                && 7 == index)
            {
                return true;
            }
            else if (ME2Indicators[8].IndMathType == algorithm.ToString()
                && ME2Indicators[8].IndMathSubType == subAlgorithm.ToString()
                && 8 == index)
            {
                return true;
            }
            else if (ME2Indicators[9].IndMathType == algorithm.ToString()
                && ME2Indicators[9].IndMathSubType == subAlgorithm.ToString()
                && 9 == index)
            {
                return true;
            }
            else if (ME2Indicators[10].IndMathType == algorithm.ToString()
                && ME2Indicators[10].IndMathSubType == subAlgorithm.ToString()
                && 10 == index)
            {
                return true;
            }
            else if (ME2Indicators[11].IndMathType == algorithm.ToString()
                && ME2Indicators[11].IndMathSubType == subAlgorithm.ToString()
                && 11 == index)
            {
                return true;
            }
            else if (ME2Indicators[12].IndMathType == algorithm.ToString()
                && ME2Indicators[12].IndMathSubType == subAlgorithm.ToString()
                && 12 == index)
            {
                return true;
            }
            else if (ME2Indicators[13].IndMathType == algorithm.ToString()
                && ME2Indicators[13].IndMathSubType == subAlgorithm.ToString()
                && 13 == index)
            {
                return true;
            }
            else if (ME2Indicators[14].IndMathType == algorithm.ToString()
                && ME2Indicators[14].IndMathSubType == subAlgorithm.ToString()
                && 14 == index)
            {
                return true;
            }
            else if (ME2Indicators[15].IndMathType == algorithm.ToString()
                && ME2Indicators[15].IndMathSubType == subAlgorithm.ToString()
                && 15 == index)
            {
                return true;
            }
            else if (ME2Indicators[16].IndMathType == algorithm.ToString()
                && ME2Indicators[16].IndMathSubType == subAlgorithm.ToString()
                && 16 == index)
            {
                return true;
            }
            else if (ME2Indicators[17].IndMathType == algorithm.ToString()
                && ME2Indicators[17].IndMathSubType == subAlgorithm.ToString()
                && 17 == index)
            {
                return true;
            }
            else if (ME2Indicators[18].IndMathType == algorithm.ToString()
                && ME2Indicators[18].IndMathSubType == subAlgorithm.ToString()
                && 18 == index)
            {
                return true;
            }
            else if (ME2Indicators[19].IndMathType == algorithm.ToString()
                && ME2Indicators[19].IndMathSubType == subAlgorithm.ToString()
                && 19 == index)
            {
                return true;
            }
            else if (ME2Indicators[20].IndMathType == algorithm.ToString()
                && ME2Indicators[20].IndMathSubType == subAlgorithm.ToString()
                && 20 == index)
            {
                return true;
            }
            return bHasMathType;
        }
        public bool HasMathType(int index, Calculator1.MATH_TYPES algorithm)
        {
            bool bHasMathType = false;

            if (ME2Indicators[0].IndMathType == algorithm.ToString()
                && 0 == index)
            {
                return true;
            }
            else if (ME2Indicators[1].IndMathType == algorithm.ToString()
                && 1 == index)
            {
                return true;
            }
            else if (ME2Indicators[2].IndMathType == algorithm.ToString()
                && 2 == index)
            {
                return true;
            }
            else if (ME2Indicators[3].IndMathType == algorithm.ToString()
                && 3 == index)
            {
                return true;
            }
            else if (ME2Indicators[4].IndMathType == algorithm.ToString()
                && 4 == index)
            {
                return true;
            }
            else if (ME2Indicators[5].IndMathType == algorithm.ToString()
                && 5 == index)
            {
                return true;
            }
            else if (ME2Indicators[6].IndMathType == algorithm.ToString()
                && 6 == index)
            {
                return true;
            }
            else if (ME2Indicators[7].IndMathType == algorithm.ToString()
                && 7 == index)
            {
                return true;
            }
            else if (ME2Indicators[8].IndMathType == algorithm.ToString()
                && 8 == index)
            {
                return true;
            }
            else if (ME2Indicators[9].IndMathType == algorithm.ToString()
                && 9 == index)
            {
                return true;
            }
            else if (ME2Indicators[10].IndMathType == algorithm.ToString()
                && 10 == index)
            {
                return true;
            }
            else if (ME2Indicators[11].IndMathType == algorithm.ToString()
                && 11 == index)
            {
                return true;
            }
            else if (ME2Indicators[12].IndMathType == algorithm.ToString()
                && 12 == index)
            {
                return true;
            }
            else if (ME2Indicators[13].IndMathType == algorithm.ToString()
                && 13 == index)
            {
                return true;
            }
            else if (ME2Indicators[14].IndMathType == algorithm.ToString()
                && 14 == index)
            {
                return true;
            }
            else if (ME2Indicators[15].IndMathType == algorithm.ToString()
                && 15 == index)
            {
                return true;
            }
            else if (ME2Indicators[16].IndMathType == algorithm.ToString()
                && 16 == index)
            {
                return true;
            }
            else if (ME2Indicators[17].IndMathType == algorithm.ToString()
                && 17 == index)
            {
                return true;
            }
            else if (ME2Indicators[18].IndMathType == algorithm.ToString()
                && 18 == index)
            {
                return true;
            }
            else if (ME2Indicators[19].IndMathType == algorithm.ToString()
                && 19 == index)
            {
                return true;
            }
            else if (ME2Indicators[20].IndMathType == algorithm.ToString()
                && 20 == index)
            {
                return true;
            }
            return bHasMathType;
        }
        public async void CalculateIndicators(int indicatorIndex)
        {
            bool bHasIndicator1 = false;
            List<double> qTs = new List<double>();
            if (indicatorIndex == 1)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[1].IndLabel)
                    && (ME2Indicators[1].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 1))
                    {
                        if (HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10))
                        {
                            ProcessAlgosAsync3(indicatorIndex, ME2Indicators[1].IndURL);
                        }
                        else if (HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                            || HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                        {
                            ProcessAlgosAsync4(indicatorIndex, ME2Indicators[1].IndURL);
                        }
                        else
                        {
                            //use MathExpress to set QT
                            SetTotalMathTypeStock1();
                            //1.9.4: none means none
                            if (ME2Indicators[1].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[1].IndMathSubType)))
                            {
                                //use MathType, Distribution, and QTs to set QTM, QTL, and QTU
                                SetAlgoPRAStats(1, qTs);
                            }
                        }
                        bHasIndicator1 = true;
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 2)
            {
                if (!bHasIndicator1)
                {
                    if (!string.IsNullOrEmpty(ME2Indicators[1].IndLabel)
                        && (ME2Indicators[1].IndLabel != Constants.NONE))
                    {
                        //bad dataruls generate an error and come back in using ind=2
                        //still need to try to process ind1
                        if (!_indicators.Any(o => o == 1))
                        {
                            if (HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                                || HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10))
                            {
                                ProcessAlgosAsync3(indicatorIndex, ME2Indicators[1].IndURL);
                            }
                            else if (HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                               || HasMathType(1, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                            {
                                ProcessAlgosAsync4(indicatorIndex, ME2Indicators[1].IndURL);
                            }
                            else
                            {
                                SetTotalMathTypeStock1();
                                if (ME2Indicators[1].IndMathSubType != Constants.NONE
                                    && (!string.IsNullOrEmpty(ME2Indicators[1].IndMathSubType)))
                                {
                                    SetAlgoPRAStats(1, qTs);
                                }
                            }
                        }
                    }
                }
                //everything above needs to be done first, otherwise no way to know if the conditions are satisfied
                //can each of the following ifs can be done as a Task.WhenAll() with indicatornum++??
                //handle ind2 normally
                if (!string.IsNullOrEmpty(ME2Indicators[2].IndLabel)
                    && (ME2Indicators[2].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 2))
                    {
                        if (HasMathType(2, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(2, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                            || HasMathType(2, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                            || HasMathType(2, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                        {
                            ProcessAlgosAsync4(indicatorIndex, ME2Indicators[2].IndURL);
                        }
                        else
                        {
                            SetTotalMathTypeStock2();
                            if (ME2Indicators[2].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[2].IndMathSubType)))
                            {
                                SetAlgoPRAStats(2, qTs);
                            }
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 3)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[3].IndLabel)
                    && (ME2Indicators[3].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 3))
                    {
                        if (HasMathType(3, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(3, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                            || HasMathType(3, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                            || HasMathType(3, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                        {
                            ProcessAlgosAsync3(indicatorIndex, ME2Indicators[3].IndURL);
                        }
                        else
                        {
                            SetTotalMathTypeStock3();
                            if (ME2Indicators[3].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[3].IndMathSubType)))
                            {
                                SetAlgoPRAStats(3, qTs);
                            }
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 4)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[4].IndLabel)
                    && (ME2Indicators[4].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 4))
                    {
                        if (HasMathType(4, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(4, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10))
                        {
                            ProcessAlgosAsync3(indicatorIndex, ME2Indicators[4].IndURL);
                        }
                        else
                        {
                            SetTotalMathTypeStock4();
                            if (ME2Indicators[4].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[4].IndMathSubType)))
                            {
                                SetAlgoPRAStats(4, qTs);
                            }
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 5)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[5].IndLabel)
                    && (ME2Indicators[5].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 5))
                    {
                        if (HasMathType(5, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(5, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10))
                        {
                            ProcessAlgosAsync4(indicatorIndex, ME2Indicators[5].IndURL);
                        }
                        else
                        {
                            SetTotalMathTypeStock5();
                            if (ME2Indicators[5].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[5].IndMathSubType)))
                            {
                                SetAlgoPRAStats(5, qTs);
                            }
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 6)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[6].IndLabel)
                    && (ME2Indicators[6].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 6))
                    {
                        if (HasMathType(6, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(6, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10))
                        {
                            ProcessAlgosAsync3(indicatorIndex, ME2Indicators[6].IndURL);
                        }
                        else
                        {
                            SetTotalMathTypeStock6();
                            if (ME2Indicators[6].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[6].IndMathSubType)))
                            {
                                SetAlgoPRAStats(6, qTs);
                            }
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 7)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[7].IndLabel)
                    && (ME2Indicators[7].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 7))
                    {
                        if (HasMathType(7, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                            || HasMathType(7, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10))
                        {
                            ProcessAlgosAsync3(indicatorIndex, ME2Indicators[7].IndURL);
                        }
                        else
                        {
                            SetTotalMathTypeStock7();
                            if (ME2Indicators[7].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[7].IndMathSubType)))
                            {
                                SetAlgoPRAStats(7, qTs);
                            }
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 8)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[8].IndLabel)
                    && (ME2Indicators[8].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 8))
                    {
                        SetTotalMathTypeStock8();
                        if (ME2Indicators[8].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[8].IndMathSubType)))
                        {
                            SetAlgoPRAStats(8, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 9)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[9].IndLabel)
                    && (ME2Indicators[9].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 9))
                    {
                        SetTotalMathTypeStock9();
                        if (ME2Indicators[9].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[9].IndMathSubType)))
                        {
                            SetAlgoPRAStats(9, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 10)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[10].IndLabel)
                    && (ME2Indicators[10].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 10))
                    {
                        SetTotalMathTypeStock10();
                        if (ME2Indicators[10].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[10].IndMathSubType)))
                        {
                            SetAlgoPRAStats(10, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 11)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[11].IndLabel)
                    && (ME2Indicators[11].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 11))
                    {
                        SetTotalMathTypeStock11();
                        if (ME2Indicators[11].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[11].IndMathSubType)))
                        {
                            SetAlgoPRAStats(11, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 12)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[12].IndLabel)
                    && (ME2Indicators[12].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 12))
                    {
                        SetTotalMathTypeStock12();
                        if (ME2Indicators[12].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[12].IndMathSubType)))
                        {
                            SetAlgoPRAStats(12, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 13)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[13].IndLabel)
                    && (ME2Indicators[13].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 13))
                    {
                        SetTotalMathTypeStock13();
                        if (ME2Indicators[13].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[13].IndMathSubType)))
                        {
                            SetAlgoPRAStats(13, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 14)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[14].IndLabel)
                    && (ME2Indicators[14].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 14))
                    {
                        SetTotalMathTypeStock14();
                        if (ME2Indicators[14].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[14].IndMathSubType)))
                        {
                            SetAlgoPRAStats(14, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 15)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[15].IndLabel)
                    && (ME2Indicators[15].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 15))
                    {
                        SetTotalMathTypeStock15();
                        if (ME2Indicators[15].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[15].IndMathSubType)))
                        {
                            SetAlgoPRAStats(15, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 16)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[16].IndLabel)
                    && (ME2Indicators[16].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 16))
                    {
                        SetTotalMathTypeStock16();
                        if (ME2Indicators[16].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[16].IndMathSubType)))
                        {
                            SetAlgoPRAStats(16, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 17)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[17].IndLabel)
                    && (ME2Indicators[17].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 17))
                    {
                        SetTotalMathTypeStock17();
                        if (ME2Indicators[17].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[17].IndMathSubType)))
                        {
                            SetAlgoPRAStats(17, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 18)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[18].IndLabel)
                    && (ME2Indicators[18].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 18))
                    {
                        SetTotalMathTypeStock18();
                        if (ME2Indicators[18].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[18].IndMathSubType)))
                        {
                            SetAlgoPRAStats(18, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 19)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[19].IndLabel)
                    && (ME2Indicators[19].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 19))
                    {
                        SetTotalMathTypeStock19();
                        if (ME2Indicators[19].IndMathSubType != Constants.NONE
                            && (!string.IsNullOrEmpty(ME2Indicators[19].IndMathSubType)))
                        {
                            SetAlgoPRAStats(19, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (indicatorIndex == 20)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[20].IndLabel)
                    && (ME2Indicators[20].IndLabel != Constants.NONE))
                {
                    if (!_indicators.Any(o => o == 19))
                    {
                        SetTotalMathTypeStock20();
                        if (ME2Indicators[20].IndMathSubType != Constants.NONE
                                && (!string.IsNullOrEmpty(ME2Indicators[20].IndMathSubType)))
                        {
                            SetAlgoPRAStats(19, qTs);
                        }
                    }
                }
                indicatorIndex++;
            }
            if (!string.IsNullOrEmpty(ME2Indicators[0].IndMathExpression)
                && (ME2Indicators[0].IndMathExpression != Constants.NONE))
            {
                //don't set scores that have already been calculated (using indicator data sets)
                if (!_indicators.Any(o => o == 0))
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        ME2Indicators[0].IndMathResult += ErrorMessage;
                        ErrorMessage = string.Empty;
                    }
                    indicatorIndex = 0;
                    if (HasMathType(0, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9))
                    {
                        ProcessAlgosAsync3(indicatorIndex, DataURL);
                    }
                    else if (HasMathType(0, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                        || HasMathType(0, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                        || HasMathType(0, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                    {
                        ProcessAlgosAsync4(indicatorIndex, DataURL);
                    }
                    else
                    {
                        this.SetTotalScore(_colNames);
                        if (ME2Indicators[0].IndMathSubType != Constants.NONE
                            && (!string.IsNullOrEmpty(ME2Indicators[0].IndMathSubType)))
                        {
                            SetAlgoPRAStats(0, qTs);
                        }
                    }
                }
            }
        }
        private bool HasIndicatorData(int indicatorIndex)
        {
            bool bHasData = false;
            if (indicatorIndex == 0)
            {
                //198: scores can run datasets for algos
                if (!string.IsNullOrEmpty(DataURL)
                    && (DataURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 1)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[1].IndURL)
                    && (ME2Indicators[1].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 2)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[2].IndURL)
                    && (ME2Indicators[2].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 3)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[3].IndURL)
                    && (ME2Indicators[3].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 4)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[4].IndURL)
                    && (ME2Indicators[4].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 5)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[5].IndURL)
                    && (ME2Indicators[5].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 6)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[6].IndURL)
                    && (ME2Indicators[6].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 7)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[7].IndURL)
                    && (ME2Indicators[7].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 8)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[8].IndURL)
                    && (ME2Indicators[8].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 9)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[9].IndURL)
                    && (ME2Indicators[9].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 10)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[10].IndURL)
                    && (ME2Indicators[10].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 11)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[11].IndURL)
                    && (ME2Indicators[11].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 12)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[12].IndURL)
                    && (ME2Indicators[12].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 13)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[13].IndURL)
                    && (ME2Indicators[13].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 14)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[14].IndURL)
                    && (ME2Indicators[14].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 15)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[15].IndURL)
                    && (ME2Indicators[15].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 16)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[16].IndURL)
                    && (ME2Indicators[16].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 17)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[17].IndURL)
                    && (ME2Indicators[17].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 18)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[18].IndURL)
                    && (ME2Indicators[18].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 19)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[19].IndURL)
                    && (ME2Indicators[19].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            else if (indicatorIndex == 20)
            {
                if (!string.IsNullOrEmpty(ME2Indicators[20].IndURL)
                    && (ME2Indicators[20].IndURL != Constants.NONE))
                {
                    return true;
                }
            }
            return bHasData;
        }
        //not used but hold for potential future use
        private bool NeedsIndicatorAlgo(int indicatorIndex, string mathType, string mathSubType)
        {
            //hold for future sequential calculations
            bool bNeedsInd = false;
            if (mathType == MATH_TYPES.algorithm1.ToString()
                && mathSubType == MATH_SUBTYPES.subalgorithm9.ToString())
            {
                if (string.IsNullOrEmpty(ME2Indicators[4].IndURL) || (ME2Indicators[4].IndURL == Constants.NONE))
                {
                    //process indicators 1, 2, and 3 and stop
                    if (indicatorIndex <= 3)
                    {
                        bNeedsInd = true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ME2Indicators[6].IndURL) || (ME2Indicators[6].IndURL == Constants.NONE))
                    {
                        //process indicators 4 and 5 and stop
                        if (indicatorIndex == 4 || indicatorIndex == 5)
                        {
                            bNeedsInd = true;
                        }
                    }
                    else
                    {
                        //process indicator 6 and remaining indicators and Score
                        if (indicatorIndex > 5)
                        {
                            bNeedsInd = true;
                        }
                    }
                }
            }
            return bNeedsInd;
        }
        private async Task<string> ProcessAlgosAsync(string dataURL)
        {
            string sIndicatorsCSV = string.Empty;
            //these algos use doubles in datasets
            IDictionary<int, List<List<double>>> data = new Dictionary<int, List<List<double>>>();
            List<string> lines = new List<string>();
            //some algorithms may need to stream the lines to cut down on memory
            lines = GetDataLines(dataURL);
            //lines = await GetDataLinesAsync(dataURL);
            if (lines != null)
            {
                //reset the data
                data = new Dictionary<int, List<List<double>>>();
                //it's ok for subsequent datasets to overwrite previous results (by design)
                if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
                {
                    //ok for algos that don't need to calculate qT from q1 to q10 vars
                    data = GetDataSet(lines);
                }
                else
                {
                    //algo1 calls this
                    //182: this is used with algos2-4, but they don't call it from here (they call it from PRA)
                    data = GetDataSetwithQT(lines);
                }
                //if null already has an error message
                if (data != null)
                {
                    //each data key is a different indicator
                    List<int> algoIndicators = new List<int>(data.Keys.Count);
                    int algoIndicator = 0;
                    foreach (var ds in data)
                    {
                        if (ds.Value.Count() > 0)
                        {
                            if (ds.Value[0].Count() > 1)
                            {
                                algoIndicator = 0;
                                if (_indicators.Contains(ds.Key) == false)
                                {
                                    //this supports multiple algos that use the same pattern
                                    if (HasMathType(ds.Key, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                                        || HasMathType(ds.Key, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
                                    {
                                        //regression
                                        algoIndicator = await SetAlgoStats1(ds.Key, ds.Value);
                                    }
                                    else if (HasMathType(ds.Key, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5)
                                        || HasMathType(ds.Key, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7))
                                    {
                                        //HasDataMatrix checked to make sure they have a good jointdataurl property
                                        //process dataurls first (no effect on indicatornum because key is checked)
                                        algoIndicator = await SetAlgoCalcs(ds.Key, ds.Value);
                                    }
                                    else if (HasMathType(ds.Key, MATH_TYPES.algorithm2)
                                       || HasMathType(ds.Key, MATH_TYPES.algorithm3)
                                       || HasMathType(ds.Key, MATH_TYPES.algorithm4)
                                       || HasMathType(ds.Key, MATH_TYPES.algorithm5))
                                    {
                                        algoIndicator = ds.Key;
                                    }
                                    else
                                    {
                                        //this generates descriptive stats based on data in data url
                                        //not based on distributions
                                        algoIndicator = SetIndicatorData(ds.Key, ds.Value);
                                    }
                                    if (algoIndicator != 0)
                                    {
                                        algoIndicators.Add(algoIndicator);
                                    }
                                    else
                                    {
                                        CalculatorDescription += "Indicator properties are missing or wrong. Do indicator labels correspond to dataset labels? Do indicators have correct algorithms? Please recheck all indicator properties.";
                                    }
                                }
                            }
                        }
                    }
                    sIndicatorsCSV = GetIndicatorsCSV(algoIndicators);
                    //188
                    //Calc1.DataToAnalyze holds QT vectors from each each ind.DataToAnalyze
                    CopyData(data);
                }
            }
            return sIndicatorsCSV;
        }
        public static string GetIndicatorsCSV(List<int> algoIndicators)
        {
            string sIndicatorsCSV = string.Empty;
            foreach (var indicator in algoIndicators)
            {
                if (indicator != 0)
                {
                    sIndicatorsCSV += string.Concat(indicator, Constants.CSV_DELIMITER);
                }
            }
            //remove the last delimiter
            if (sIndicatorsCSV.EndsWith(Constants.CSV_DELIMITER))
            {
                sIndicatorsCSV = sIndicatorsCSV.Remove(sIndicatorsCSV.Length - 1, 1);
            }
            return sIndicatorsCSV;
        }
        private async Task<string> ProcessAlgoCorrAsync(string jdataURL, int dataIndex)
        {

            string sIndicatorIndexes = string.Empty;
            string sError = string.Empty;
            //see if there is a corresponding dataset
            //this requires a 1 to 1 relation between dataurls and jointdataurls
            IDictionary<int, List<List<double>>> data = GetDataSet1(dataIndex);
            //has to replace the algo code above
            if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
            {
                sIndicatorIndexes = await SetAlgoCorrStats(jdataURL, data);
            }
            return sIndicatorIndexes;
        }
        private async Task<string> ProcessAlgosAsync3(int indicatorIndex, string urls)
        {
            string sIndicatorCSV = string.Empty;
            List<string> lines = new List<string>();
            List<string> lines2 = new List<string>();
            IDictionary<int, List<List<string>>> data = new Dictionary<int, List<List<string>>>();
            IDictionary<int, List<List<string>>> colSets = new Dictionary<int, List<List<string>>>();
            if (HasIndicatorData(indicatorIndex)
                && indicatorIndex != 4)
            {
                //all use at least 1 dataset
                lines = GetDataLines(0, urls);
                if (indicatorIndex == 6
                    || indicatorIndex == 7)
                {
                    //ind 5 has the costs
                    lines2 = GetMathResultLines(5);
                }
            }
            else
            {
                if (indicatorIndex == 3)
                {
                    if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                        || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                    {
                        //ind 3 has the rmis
                        lines = GetMathResultLines(1);
                        //ind 2 has the costs
                        lines2 = GetMathResultLines(2);
                    }
                }
                else if (indicatorIndex == 4)
                {
                    //ind 3 has the damage percents
                    lines = GetMathResultLines(3);
                    //ind 2 has the asset values
                    lines2 = GetMathResultLines(2);
                    //198: ind 4 may have the trends
                    if (HasIndicatorData(indicatorIndex))
                    {
                        List<string> lines3 = new List<string>();
                        lines3 = GetDataLines(0, urls);
                        //get rid of header row
                        lines3.RemoveAt(0);
                        //append them to lines2 (used in same calc)
                        lines2.AddRange(lines3);
                    }

                }
                else if (indicatorIndex == 6
                    || indicatorIndex == 7)
                {
                    //ind 4 has the avg ann damage 
                    lines = GetMathResultLines(4);
                    //ind 5 has the costs
                    lines2 = GetMathResultLines(5);
                }
            }
            if (lines != null)
            {
                //get the data
                data = GetDataSetFull2(lines);
                colSets = GetColumnSetFull(lines);
                //if null already has an error message
                if (data != null)
                {
                    List<int> algoIndicators = new List<int>(data.Keys.Count);
                    int algoIndicator = 0;
                    int iIndex = 0;
                    //this data url should have one and only 1 indicator in it -same as rproject datasets
                    foreach (var ds in data)
                    {
                        algoIndicator = 0;
                        iIndex = ds.Key;
                        //indicator index means labels aren't critical
                        if (indicatorIndex == 0)
                        {
                            iIndex = 0;
                        }
                        else if (indicatorIndex == 1)
                        {
                            iIndex = 1;
                        }
                        else if (indicatorIndex == 3)
                        {
                            iIndex =3;
                        }
                        else if (indicatorIndex == 4)
                        {
                            iIndex = 4;
                        }
                        else if (indicatorIndex == 6)
                        {
                            iIndex = 6;
                        }
                        else if (indicatorIndex == 7)
                        {
                            iIndex = 7;
                        }
                        if (_indicators.Contains(iIndex) == false)
                        {
                            //these all use 1 dataset
                            algoIndicator = await SetAlgoStats3(iIndex, ds.Value,
                                colSets[ds.Key], lines2);
                            if (algoIndicator != 0)
                            {
                                algoIndicators.Add(algoIndicator);
                            }
                            else
                            {
                                CalculatorDescription += "Indicator properties are missing or wrong. Do indicator labels correspond to dataset labels? Do indicators have correct algorithms? Please recheck all indicator properties.";
                            }
                        }
                    }
                    sIndicatorCSV = GetIndicatorsCSV(algoIndicators);
                }
            }
            return sIndicatorCSV;
        }
        private async Task<string> ProcessAlgosAsync4(int indicatorIndex, string urls)
        {
            string sIndicatorCSV = string.Empty;
            List<string> lines = new List<string>();
            List<string> lines2 = new List<string>();
            IDictionary<int, List<List<string>>> data = new Dictionary<int, List<List<string>>>();
            IDictionary<int, List<List<string>>> colSets = new Dictionary<int, List<List<string>>>();
            if (HasIndicatorData(indicatorIndex))
            {
                //all use at least 1 dataset
                lines = GetDataLines(0, urls);
                if (indicatorIndex == 5)
                {
                    lines2 = GetDataLines(1, urls);
                }
                else
                {
                    if (HasMathType(MATH_TYPES.algorithm1,
                        MATH_SUBTYPES.subalgorithm11)
                        || HasMathType(MATH_TYPES.algorithm1,
                        MATH_SUBTYPES.subalgorithm12))
                    {
                        if (indicatorIndex == 2)
                        {
                            lines2 = GetDataLines(1, urls);
                        }
                    }
                }
            }
            if (lines != null)
            {
                //get the data
                data = GetDataSetFull2(lines);
                colSets = GetColumnSetFull(lines);
                //if null already has an error message
                if (data != null)
                {
                    List<int> algoIndicators = new List<int>(data.Keys.Count);
                    int algoIndicator = 0;
                    int iIndex = 0;
                    foreach (var ds in data)
                    {
                        algoIndicator = 0;
                        iIndex = ds.Key;
                        if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                            || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
                        {
                            if (indicatorIndex == 0)
                            {
                                iIndex = 0;
                            }
                            else if (indicatorIndex == 1)
                            {
                                iIndex = 1;
                            }
                        }
                        //198: added 0 and 1 to list
                        if (indicatorIndex == 0)
                        {
                            iIndex = 0;
                        }
                        else if (indicatorIndex == 1)
                        {
                            iIndex = 1;
                        }
                        else if (indicatorIndex == 2)
                        {
                            iIndex = 2;
                        }
                        else if (indicatorIndex == 5)
                        {
                            iIndex = 5;
                        }
                        if (_indicators.Contains(iIndex) == false)
                        {
                            algoIndicator = await SetAlgoStats4(iIndex, ds.Value, colSets[ds.Key], lines2);
                            if (algoIndicator != 0)
                            {
                                algoIndicators.Add(algoIndicator);
                            }
                            else
                            {
                                CalculatorDescription += "Indicator properties are missing or wrong. Do indicator labels correspond to dataset labels? Do indicators have correct algorithms? Please recheck all indicator properties.";
                            }
                        }
                    }
                    sIndicatorCSV = GetIndicatorsCSV(algoIndicators);
                }
            }
            return sIndicatorCSV;
        }
        private async Task<string> ProcessAlgosAsync2(string dataURL, int jDataURLIndex)
        {
            string sIndicatorCSV = string.Empty;
            //increment the dataindex and set the corresponding jdataurl
            string sJDataURL = string.Empty;

            if (HasMathType(MATH_TYPES.algorithm2)
                || HasMathType(MATH_TYPES.algorithm3)
                || HasMathType(MATH_TYPES.algorithm4))
            {
                //these are actual rscripts which in fact end with a semicolon
                string[] jdataURLs = ME2Indicators[0].IndURL.Split(Constants.STRING_DELIMITERS);
                sJDataURL = jdataURLs[jDataURLIndex];
                if (string.IsNullOrEmpty(sJDataURL))
                {
                    //these store 
                    return string.Empty;
                }
            }
            //this algo uses r project data files passed directly to algo
            List<string> lines = new List<string>();
            if (HasMathType(MATH_TYPES.algorithm2)
                || HasMathType(MATH_TYPES.algorithm3)
                || HasMathType(MATH_TYPES.algorithm4))
            {
                //some algorithms will have to stream the lines to cut down on memory
                lines = GetDataLines(dataURL);
            }
            if (lines != null)
            {
                bool bHasColNames = false;
                if (HasMathType(MATH_TYPES.algorithm2)
                    || HasMathType(MATH_TYPES.algorithm3)
                    || HasMathType(MATH_TYPES.algorithm4))
                {
                    //ok for algos that don't need to calculate qT from q1 to q10 vars
                    bHasColNames = SetColumnNames(lines);
                }
                //if false already has an error message
                if (bHasColNames == true)
                {
                    //each data key is a different indicator
                    List<int> algoIndicators = new List<int>();
                    //uses colnames with Indicator.MathExpression to determine which indicator to update
                    int iIndicatorIndex = GetIndicatorIndex(_colNames);
                    if (_indicators.Contains(iIndicatorIndex) == false)
                    {
                        //this supports multiple algos that use the same pattern
                        if (HasMathType(MATH_TYPES.algorithm2)
                            || HasMathType(MATH_TYPES.algorithm3)
                            || HasMathType(MATH_TYPES.algorithm4))
                        {
                            iIndicatorIndex = await SetAlgoStats2(iIndicatorIndex, dataURL, sJDataURL);
                        }
                        if (iIndicatorIndex != 0)
                        {
                            algoIndicators.Add(iIndicatorIndex);
                        }
                        else
                        {
                            CalculatorDescription += "Indicator properties are missing or wrong. Do indicator labels correspond to dataset labels? Do indicators have correct algorithms? Please recheck all indicator properties.";
                        }
                        sIndicatorCSV = GetIndicatorsCSV(algoIndicators);
                    }
                }
            }
            return sIndicatorCSV;
        }


        public async Task<string> ProcessAlgosForAnalyzersAsync(List<ME2Stock> stocks)
        {
            string sIndicatorCSV = string.Empty;
            int i = 0;
            //copy the individual datasets into analyzerStock.DataToAnalyze
            //each change stock will have totals passed back from algo; algo needs to know how many totals are needed for changestocks
            Observations = stocks.Count;
            foreach (var calc in stocks)
            {
                CopyData(calc.DataToAnalyze);
                if (i == 0)
                {
                    if (!string.IsNullOrEmpty(calc.DataColNames))
                    {
                        //take column names from first dataset; by rule all datasets have to have the same column names
                        //as an alternative to using List<double> datasets could subsitute lines and have more flexibility and complexity
                        _colNames = calc.DataColNames.Split(Constants.CSV_DELIMITERS);
                    }
                }
                i++;
            }
            //if null already has an error message
            if (DataToAnalyze != null)
            {
                //each data key is a different indicator
                List<int> algoIndicators = new List<int>(DataToAnalyze.Keys.Count);
                int iIndicatorIndex = 0;
                int iKey = 0;
                i = 0;
                foreach (var ds in DataToAnalyze)
                {
                    iIndicatorIndex = 0;
                    iKey = CalculatorHelpers.ConvertStringToInt(ds.Key);
                    AddIndicatorToHoldTotals(i, ds.Key);
                    //this supports multiple algos that use the same pattern
                    if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                        || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
                    {
                        //regression
                        iIndicatorIndex = await SetAlgoStats1(iKey, ds.Value);
                    }
                    else if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5)
                        || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7))
                    {
                        //HasDataMatrix checked to make sure they have a good jointdataurl property
                        //process dataurls first (no effect on indicatornum because key is checked)
                        iIndicatorIndex = await SetAlgoCalcs(iKey, ds.Value);
                    }
                    else
                    {
                        //this generates descriptive stats based on data in data url
                        //not based on distributions
                        iIndicatorIndex = SetIndicatorData(iKey, ds.Value);
                    }
                    if (iIndicatorIndex != 0)
                    {
                        algoIndicators.Add(iIndicatorIndex);
                    }
                    else
                    {
                        ErrorMessage += Errors.MakeStandardErrorMsg("INDICATORS_BAD");
                    }
                    //analytic results for analyzers are in the dataset not in stock object
                    if (DataToAnalyze == null)
                    {
                        ErrorMessage += Errors.MakeStandardErrorMsg("INDICATORS_BAD");
                    }
                    if (DataToAnalyze.Count == 0)
                    {
                        ErrorMessage += Errors.MakeStandardErrorMsg("INDICATORS_BAD");
                    }
                    i++;
                }
                sIndicatorCSV = GetIndicatorsCSV(algoIndicators);
            }
            return sIndicatorCSV;
        }
        private void AddIndicatorToHoldTotals(int indicatorIndex, string label)
        {
            if (indicatorIndex == 0)
            {
                //these props are used later to run the correct algo
                ME2Indicators[0].IndLabel = label;
                ME2Indicators[0].IndMathExpression = MathExpression;
                ME2Indicators[0].IndMathType = MathType;
                ME2Indicators[0].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 1)
            {
                //these props are used later to run the correct algo
                ME2Indicators[1].IndLabel = label;
                ME2Indicators[1].IndMathExpression = MathExpression;
                ME2Indicators[1].IndMathType = MathType;
                ME2Indicators[1].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 2)
            {
                ME2Indicators[2].IndLabel = label;
                ME2Indicators[2].IndMathExpression = MathExpression;
                ME2Indicators[2].IndMathType = MathType;
                ME2Indicators[2].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 3)
            {
                ME2Indicators[3].IndLabel = label;
                ME2Indicators[3].IndMathExpression = MathExpression;
                ME2Indicators[3].IndMathType = MathType;
                ME2Indicators[3].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 4)
            {
                ME2Indicators[4].IndLabel = label;
                ME2Indicators[4].IndMathExpression = MathExpression;
                ME2Indicators[4].IndMathType = MathType;
                ME2Indicators[4].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 5)
            {
                ME2Indicators[5].IndLabel = label;
                ME2Indicators[5].IndMathExpression = MathExpression;
                ME2Indicators[5].IndMathType = MathType;
                ME2Indicators[5].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 6)
            {
                ME2Indicators[6].IndLabel = label;
                ME2Indicators[6].IndMathExpression = MathExpression;
                ME2Indicators[6].IndMathType = MathType;
                ME2Indicators[6].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 7)
            {
                ME2Indicators[7].IndLabel = label;
                ME2Indicators[7].IndMathExpression = MathExpression;
                ME2Indicators[7].IndMathType = MathType;
                ME2Indicators[7].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 8)
            {
                ME2Indicators[8].IndLabel = label;
                ME2Indicators[8].IndMathExpression = MathExpression;
                ME2Indicators[8].IndMathType = MathType;
                ME2Indicators[8].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 9)
            {
                ME2Indicators[9].IndLabel = label;
                ME2Indicators[9].IndMathExpression = MathExpression;
                ME2Indicators[9].IndMathType = MathType;
                ME2Indicators[9].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 10)
            {
                ME2Indicators[10].IndLabel = label;
                ME2Indicators[10].IndMathExpression = MathExpression;
                ME2Indicators[10].IndMathType = MathType;
                ME2Indicators[10].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 11)
            {
                ME2Indicators[11].IndLabel = label;
                ME2Indicators[11].IndMathExpression = MathExpression;
                ME2Indicators[11].IndMathType = MathType;
                ME2Indicators[11].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 12)
            {
                ME2Indicators[12].IndLabel = label;
                ME2Indicators[12].IndMathExpression = MathExpression;
                ME2Indicators[12].IndMathType = MathType;
                ME2Indicators[12].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 13)
            {
                ME2Indicators[13].IndLabel = label;
                ME2Indicators[13].IndMathExpression = MathExpression;
                ME2Indicators[13].IndMathType = MathType;
                ME2Indicators[13].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 14)
            {
                ME2Indicators[14].IndLabel = label;
                ME2Indicators[14].IndMathExpression = MathExpression;
                ME2Indicators[14].IndMathType = MathType;
                ME2Indicators[14].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 15)
            {
                ME2Indicators[15].IndLabel = label;
                ME2Indicators[15].IndMathExpression = MathExpression;
                ME2Indicators[15].IndMathType = MathType;
                ME2Indicators[15].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 16)
            {
                ME2Indicators[16].IndLabel = label;
                ME2Indicators[16].IndMathExpression = MathExpression;
                ME2Indicators[16].IndMathType = MathType;
                ME2Indicators[16].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 17)
            {
                ME2Indicators[17].IndLabel = label;
                ME2Indicators[17].IndMathExpression = MathExpression;
                ME2Indicators[17].IndMathType = MathType;
                ME2Indicators[17].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 18)
            {
                ME2Indicators[18].IndLabel = label;
                ME2Indicators[18].IndMathExpression = MathExpression;
                ME2Indicators[18].IndMathType = MathType;
                ME2Indicators[18].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 19)
            {
                ME2Indicators[19].IndLabel = label;
                ME2Indicators[19].IndMathExpression = MathExpression;
                ME2Indicators[19].IndMathType = MathType;
                ME2Indicators[19].IndMathSubType = MathSubType;
            }
            else if (indicatorIndex == 20)
            {
                ME2Indicators[20].IndLabel = label;
                ME2Indicators[20].IndMathExpression = MathExpression;
                ME2Indicators[20].IndMathType = MathType;
                ME2Indicators[20].IndMathSubType = MathSubType;
            }
        }
        private string GetJDataURL(string dataURL)
        {
            //run this synchronously
            //increment the dataindex and set the corresponding jdataurl
            _dataIndex++;
            int iJDataIndex = _dataIndex;
            if (string.IsNullOrEmpty(ME2Indicators[0].IndURL))
            {
                ErrorMessage += string.Concat(" ",
                    Errors.MakeStandardErrorMsg("JDATAURL_BAD"), dataURL, " index = ", iJDataIndex.ToString());
                return string.Empty;
            }
            string[] jDataURLs = ME2Indicators[0].IndURL.Split(Constants.STRING_DELIMITERS);
            string jDataURL = string.Empty;
            if (jDataURLs.Count() > iJDataIndex)
            {
                jDataURL = jDataURLs[iJDataIndex];
            }
            if (string.IsNullOrEmpty(jDataURL))
            {
                ErrorMessage += string.Concat(" ",
                    Errors.MakeStandardErrorMsg("JDATAURL_BAD"), dataURL, " index = ", iJDataIndex.ToString());
                return string.Empty;
            }
            return jDataURL;
        }

        private int[] GetIndicators(string[] csvIndicators)
        {
            List<int> inds = new List<int>();
            //version 1.9.2 allows multiple subalgos to be run for multiple subinds
            if (_indicators != null)
            {
                foreach (var indicator in _indicators)
                {
                    inds.Add(indicator);
                }
            }
            int iIndicator = 0;
            foreach (var indicatorscsv in csvIndicators)
            {
                string[] newindicators = indicatorscsv.Split(Constants.CSV_DELIMITERS);
                foreach (var newindicator in newindicators)
                {
                    iIndicator = CalculatorHelpers.ConvertStringToInt(newindicator);
                    inds.Add(iIndicator);
                }
            }
            return inds.ToArray();
        }
        public static int[] GetIndexes(string algoIndicator)
        {
            string[] algoIndicators = algoIndicator.Split(Constants.CSV_DELIMITERS);
            List<int> inds = new List<int>();
            int iIndicator = 0;
            foreach (var newindicator in algoIndicators)
            {
                iIndicator = CalculatorHelpers.ConvertStringToInt(newindicator);
                inds.Add(iIndicator);
            }
            return inds.ToArray();
        }
        public static IDictionary<string, List<List<double>>> ConvertDataToString(
            IDictionary<int, List<List<double>>> data)
        {
            IDictionary<string, List<List<double>>> dataSets = new Dictionary<string, List<List<double>>>();
            foreach(var dataset in data)
            {
                dataSets.Add(dataset.Key.ToString(), dataset.Value);
            }
            return dataSets;
        }
        public static bool ContainsIndex(int index, string algoIndicator)
        {
            bool bContainsIndex = false;
            string[] algoIndicators = algoIndicator.Split(Constants.CSV_DELIMITERS);
            foreach (var newindicator in algoIndicators)
            {
                if (newindicator == index.ToString())
                {
                    return true;
                }
            }
            return bContainsIndex;
        }
        private int[] GetIndicatorsDisplay()
        {
            List<int> inds = new List<int>();
            if (ME2Indicators[1].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(1);
            }
            if (ME2Indicators[2].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[2].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(2);
            }
            if (ME2Indicators[3].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[3].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(3);
            }
            if (ME2Indicators[4].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[4].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(4);
            }
            if (ME2Indicators[5].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[5].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(5);
            }
            if (ME2Indicators[6].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[6].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(6);
            }
            if (ME2Indicators[7].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[7].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(7);
            }
            if (ME2Indicators[8].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[8].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(8);
            }
            if (ME2Indicators[9].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[9].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(9);
            }
            if (ME2Indicators[10].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[10].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(10);
            }
            if (ME2Indicators[11].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[11].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(11);
            }
            if (ME2Indicators[12].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[12].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(12);
            }
            if (ME2Indicators[13].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[13].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(13);
            }
            if (ME2Indicators[14].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[14].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(14);
            }
            if (ME2Indicators[15].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[15].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(15);
            }
            if (ME2Indicators[16].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[16].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(16);
            }
            if (ME2Indicators[17].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[17].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(17);
            }
            if (ME2Indicators[18].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[18].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(18);
            }
            if (ME2Indicators[19].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[19].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(19);
            }
            if (ME2Indicators[20].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[20].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                inds.Add(19);
            }
            if (ME2Indicators[0].IndMathType == MATH_TYPES.algorithm5.ToString()
                && ME2Indicators[0].IndMathSubType == MATH_SUBTYPES.subalgorithm1.ToString())
            {
                //scores filtered by 'score'
                inds.Add(0);
            }
            return inds.ToArray();
        }
        public void AddAllIndicators(List<int> newInds)
        {
            if (!newInds.Contains(1)
                && (!string.IsNullOrEmpty(ME2Indicators[1].IndLabel)))
            {
                //ui doesn't have label just label constant
                newInds.Add(1);
            }
            if (!newInds.Contains(2)
                && (!string.IsNullOrEmpty(ME2Indicators[2].IndLabel)))
            {
                newInds.Add(2);
            }
            if (!newInds.Contains(3)
                && (!string.IsNullOrEmpty(ME2Indicators[3].IndLabel)))
            {
                newInds.Add(3);
            }
            if (!newInds.Contains(4)
                && (!string.IsNullOrEmpty(ME2Indicators[4].IndLabel)))
            {
                newInds.Add(4);
            }
            if (!newInds.Contains(5)
                && (!string.IsNullOrEmpty(ME2Indicators[5].IndLabel)))
            {
                newInds.Add(5);
            }
            if (!newInds.Contains(6)
                && (!string.IsNullOrEmpty(ME2Indicators[6].IndLabel)))
            {
                newInds.Add(6);
            }
            if (!newInds.Contains(7)
                && (!string.IsNullOrEmpty(ME2Indicators[7].IndLabel)))
            {
                newInds.Add(7);
            }
            if (!newInds.Contains(8)
                && (!string.IsNullOrEmpty(ME2Indicators[8].IndLabel)))
            {
                newInds.Add(8);
            }
            if (!newInds.Contains(9)
                && (!string.IsNullOrEmpty(ME2Indicators[9].IndLabel)))
            {
                newInds.Add(9);
            }
            if (!newInds.Contains(10)
                && (!string.IsNullOrEmpty(ME2Indicators[10].IndLabel)))
            {
                newInds.Add(10);
            }
            if (!newInds.Contains(11)
                && (!string.IsNullOrEmpty(ME2Indicators[11].IndLabel)))
            {
                newInds.Add(11);
            }
            if (!newInds.Contains(12)
                && (!string.IsNullOrEmpty(ME2Indicators[12].IndLabel)))
            {
                newInds.Add(12);
            }
            if (!newInds.Contains(13)
                && (!string.IsNullOrEmpty(ME2Indicators[13].IndLabel)))
            {
                newInds.Add(13);
            }
            if (!newInds.Contains(14)
                && (!string.IsNullOrEmpty(ME2Indicators[14].IndLabel)))
            {
                newInds.Add(14);
            }
            if (!newInds.Contains(15)
                && (!string.IsNullOrEmpty(ME2Indicators[15].IndLabel)))
            {
                newInds.Add(15);
            }
            if (!newInds.Contains(16)
                && (!string.IsNullOrEmpty(ME2Indicators[16].IndLabel)))
            {
                newInds.Add(16);
            }
            if (!newInds.Contains(17)
                && (!string.IsNullOrEmpty(ME2Indicators[17].IndLabel)))
            {
                newInds.Add(17);
            }
            if (!newInds.Contains(18)
                && (!string.IsNullOrEmpty(ME2Indicators[18].IndLabel)))
            {
                newInds.Add(18);
            }
            if (!newInds.Contains(19)
                && (!string.IsNullOrEmpty(ME2Indicators[19].IndLabel)))
            {
                newInds.Add(19);
            }
            if (!newInds.Contains(19)
                && (!string.IsNullOrEmpty(ME2Indicators[20].IndLabel)))
            {
                newInds.Add(19);
            }
        }
        //retain for potential use
        private string ExcludeIndicator(int index)
        {
            string scLabel = string.Empty;
            if (index == 0)
            {
                ME2Indicators[0].IndLabel = string.Empty;
                ME2Indicators[0].IndName = string.Empty;
            }
            else if (index == 1)
            {
                ME2Indicators[1].IndLabel = string.Empty;
                ME2Indicators[1].IndName = string.Empty;
            }
            else if (index == 2)
            {
                ME2Indicators[2].IndLabel = string.Empty;
                ME2Indicators[2].IndName = string.Empty;
            }
            else if (index == 3)
            {
                ME2Indicators[3].IndLabel = string.Empty;
                ME2Indicators[3].IndName = string.Empty;
            }
            else if (index == 4)
            {
                ME2Indicators[4].IndLabel = string.Empty;
                ME2Indicators[4].IndName = string.Empty;
            }
            else if (index == 5)
            {
                ME2Indicators[5].IndLabel = string.Empty;
                ME2Indicators[5].IndName = string.Empty;
            }
            else if (index == 6)
            {
                ME2Indicators[6].IndLabel = string.Empty;
                ME2Indicators[6].IndName = string.Empty;
            }
            else if (index == 7)
            {
                ME2Indicators[7].IndLabel = string.Empty;
                ME2Indicators[7].IndName = string.Empty;
            }
            else if (index == 8)
            {
                ME2Indicators[8].IndLabel = string.Empty;
                ME2Indicators[8].IndName = string.Empty;
            }
            else if (index == 9)
            {
                ME2Indicators[9].IndLabel = string.Empty;
                ME2Indicators[9].IndName = string.Empty;
            }
            else if (index == 10)
            {
                ME2Indicators[10].IndLabel = string.Empty;
                ME2Indicators[10].IndName = string.Empty;
            }
            else if (index == 11)
            {
                ME2Indicators[11].IndLabel = string.Empty;
                ME2Indicators[11].IndName = string.Empty;
            }
            else if (index == 12)
            {
                ME2Indicators[12].IndLabel = string.Empty;
                ME2Indicators[12].IndName = string.Empty;
            }
            else if (index == 13)
            {
                ME2Indicators[13].IndLabel = string.Empty;
                ME2Indicators[13].IndName = string.Empty;
            }
            else if (index == 14)
            {
                ME2Indicators[14].IndLabel = string.Empty;
                ME2Indicators[14].IndName = string.Empty;
            }
            else if (index == 15)
            {
                ME2Indicators[15].IndLabel = string.Empty;
                ME2Indicators[15].IndName = string.Empty;
            }
            else if (index == 16)
            {
                ME2Indicators[16].IndLabel = string.Empty;
                ME2Indicators[16].IndName = string.Empty;
            }
            else if (index == 17)
            {
                ME2Indicators[17].IndLabel = string.Empty;
                ME2Indicators[17].IndName = string.Empty;
            }
            else if (index == 18)
            {
                ME2Indicators[18].IndLabel = string.Empty;
                ME2Indicators[18].IndName = string.Empty;
            }
            else if (index == 19)
            {
                ME2Indicators[19].IndLabel = string.Empty;
                ME2Indicators[19].IndName = string.Empty;
            }
            else if (index == 20)
            {
                ME2Indicators[20].IndLabel = string.Empty;
                ME2Indicators[20].IndName = string.Empty;
            }
            else
            {
                //ignore the row
            }
            return scLabel;
        }
        public async Task<List<string>> GetDataLinesAsync(string dataURL)
        {
            List<string> lines = new List<string>();
            lines = await CalculatorHelpers.ReadLinesAsync(CalcParameters.ExtensionDocToCalcURI, dataURL);
            if (lines == null)
            {
                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                return null;
            }
            if (lines.Count == 0)
            {
                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                return null;
            }
            if (lines.Count == 0)
            {
                //not an error no data set for the d = 0 to d < 15 iteration
                return null;
            }
            return lines;
        }
        public List<string> GetDataLines(string dataURL)
        {
            string sError = string.Empty;
            List<string> lines = new List<string>();
            lines = CalculatorHelpers.ReadLines(CalcParameters.ExtensionDocToCalcURI, dataURL, out sError);
            if (lines == null)
            {
                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                return null;
            }
            if (lines.Count == 0)
            {
                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                return null;
            }
            if (!string.IsNullOrEmpty(sError))
            {
                CalculatorDescription += sError;
                return null;
            }
            if (lines.Count == 0)
            {
                //not an error no data set for the d = 0 to d < 15 iteration
                return null;
            }
            return lines;
        }
        private List<string> GetDataLines(int dataSetIndex, string url)
        {
            string sError = string.Empty;
            List<string> lines = new List<string>();
            //semicolon delimiter
            string[] dataURLs = url.Split(Constants.STRING_DELIMITERS);
            for (int i = 0; i < dataURLs.Count(); i++)
            {
                if (i == dataSetIndex)
                {
                    lines = CalculatorHelpers.ReadLines(CalcParameters.ExtensionDocToCalcURI, dataURLs[i], out sError);
                    if (lines == null)
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                        return null;
                    }
                    if (lines.Count == 0)
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                        return null;
                    }
                    if (!string.IsNullOrEmpty(sError))
                    {
                        CalculatorDescription += sError;
                        return null;
                    }
                }
            }
            if (lines.Count == 0)
            {
                //not an error no data set for the d = 0 to d < 15 iteration
                return null;
            }
            return lines;
        }
        private List<string> GetMathResultLines(int indicatorIndex)
        {
            List<string> lines = new List<string>();
            if (indicatorIndex == 1)
            {
                if (ME2Indicators[1].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[1].IndMathResult);
                }
                else
                {
                    //get the csv lines out of mathresult and skip the first line
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[1].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 2)
            {
                if (ME2Indicators[2].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[2].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[2].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 3)
            {
                if (ME2Indicators[3].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[3].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[3].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 4)
            {
                if (ME2Indicators[4].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[4].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[4].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 5)
            {
                if (ME2Indicators[5].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[5].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[5].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 6)
            {
                if (ME2Indicators[6].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[6].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[6].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 7)
            {
                if (ME2Indicators[7].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[7].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[7].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 8)
            {
                if (ME2Indicators[8].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[8].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[8].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 9)
            {
                if (ME2Indicators[9].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[9].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[9].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 10)
            {
                if (ME2Indicators[10].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[10].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[10].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 11)
            {
                if (ME2Indicators[11].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[11].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[11].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 12)
            {
                if (ME2Indicators[12].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[12].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[12].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 13)
            {
                if (ME2Indicators[13].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[13].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[13].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 14)
            {
                if (ME2Indicators[14].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[14].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[14].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 15)
            {
                if (ME2Indicators[15].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[15].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[15].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 16)
            {
                if (ME2Indicators[16].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[16].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[16].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 17)
            {
                if (ME2Indicators[17].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[17].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[17].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 18)
            {
                if (ME2Indicators[18].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[18].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[18].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 19)
            {
                if (ME2Indicators[19].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[19].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[19].IndMathResult, 1);
                }
            }
            else if (indicatorIndex == 20)
            {
                if (ME2Indicators[20].IndMathResult.ToLower().StartsWith("http"))
                {
                    lines = GetDataLines(ME2Indicators[20].IndMathResult);
                }
                else
                {
                    lines = CalculatorHelpers.GetLinesandSkip(ME2Indicators[20].IndMathResult, 1);
                }
            }
            return lines;
        }
        public void SetTotalMathTypeStock1()
        {
            ME2Indicators[1].IndTAmount = this.GetTotalFromMathExpression(1, _colNames, ME2Indicators[1].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[1].IndTMAmount = ME2Indicators[1].IndTAmount;
        }

        public void SetTotalMathTypeStock2()
        {
            ME2Indicators[2].IndTAmount = this.GetTotalFromMathExpression(2, _colNames, ME2Indicators[2].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[2].IndTMAmount = ME2Indicators[2].IndTAmount;
        }
        public void SetTotalMathTypeStock3()
        {
            ME2Indicators[3].IndTAmount = this.GetTotalFromMathExpression(3, _colNames, ME2Indicators[3].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[3].IndTMAmount = ME2Indicators[3].IndTAmount;
        }
        public void SetTotalMathTypeStock4()
        {
            ME2Indicators[4].IndTAmount = this.GetTotalFromMathExpression(4, _colNames, ME2Indicators[4].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[4].IndTMAmount = ME2Indicators[4].IndTAmount;
        }
        public void SetTotalMathTypeStock5()
        {
            ME2Indicators[5].IndTAmount = this.GetTotalFromMathExpression(5, _colNames, ME2Indicators[5].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[5].IndTMAmount = ME2Indicators[5].IndTAmount;
        }
        public void SetTotalMathTypeStock6()
        {
            ME2Indicators[6].IndTAmount = this.GetTotalFromMathExpression(6, _colNames, ME2Indicators[6].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[6].IndTMAmount = ME2Indicators[6].IndTAmount;
        }
        public void SetTotalMathTypeStock7()
        {
            ME2Indicators[7].IndTAmount = this.GetTotalFromMathExpression(7, _colNames, ME2Indicators[7].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[7].IndTMAmount = ME2Indicators[7].IndTAmount;
        }
        public void SetTotalMathTypeStock8()
        {
            ME2Indicators[8].IndTAmount = this.GetTotalFromMathExpression(8, _colNames, ME2Indicators[8].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[8].IndTMAmount = ME2Indicators[8].IndTAmount;
        }
        public void SetTotalMathTypeStock9()
        {
            ME2Indicators[9].IndTAmount = this.GetTotalFromMathExpression(9, _colNames, ME2Indicators[9].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[9].IndTMAmount = ME2Indicators[9].IndTAmount;
        }
        public void SetTotalMathTypeStock10()
        {
            ME2Indicators[10].IndTAmount = this.GetTotalFromMathExpression(10, _colNames, ME2Indicators[10].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[10].IndTMAmount = ME2Indicators[10].IndTAmount;
        }
        public void SetTotalMathTypeStock11()
        {
            ME2Indicators[11].IndTAmount = this.GetTotalFromMathExpression(11, _colNames, ME2Indicators[11].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[11].IndTMAmount = ME2Indicators[11].IndTAmount;
        }
        public void SetTotalMathTypeStock12()
        {
            ME2Indicators[12].IndTAmount = this.GetTotalFromMathExpression(12, _colNames, ME2Indicators[12].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[12].IndTMAmount = ME2Indicators[12].IndTAmount;
        }
        public void SetTotalMathTypeStock13()
        {
            ME2Indicators[13].IndTAmount = this.GetTotalFromMathExpression(13, _colNames, ME2Indicators[13].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[13].IndTMAmount = ME2Indicators[13].IndTAmount;
        }
        public void SetTotalMathTypeStock14()
        {
            ME2Indicators[14].IndTAmount = this.GetTotalFromMathExpression(14, _colNames, ME2Indicators[14].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[14].IndTMAmount = ME2Indicators[14].IndTAmount;
        }
        public void SetTotalMathTypeStock15()
        {
            ME2Indicators[15].IndTAmount = this.GetTotalFromMathExpression(15, _colNames, ME2Indicators[15].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[15].IndTMAmount = ME2Indicators[15].IndTAmount;
        }
        public void SetTotalMathTypeStock16()
        {
            ME2Indicators[16].IndTAmount = this.GetTotalFromMathExpression(16, _colNames, ME2Indicators[16].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[16].IndTMAmount = ME2Indicators[16].IndTAmount;
        }
        public void SetTotalMathTypeStock17()
        {
            ME2Indicators[17].IndTAmount = this.GetTotalFromMathExpression(17, _colNames, ME2Indicators[17].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[17].IndTMAmount = ME2Indicators[17].IndTAmount;
        }
        public void SetTotalMathTypeStock18()
        {
            ME2Indicators[18].IndTAmount = this.GetTotalFromMathExpression(18, _colNames, ME2Indicators[18].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[18].IndTMAmount = ME2Indicators[18].IndTAmount;
        }
        public void SetTotalMathTypeStock19()
        {
            ME2Indicators[19].IndTAmount = this.GetTotalFromMathExpression(19, _colNames, ME2Indicators[19].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[19].IndTMAmount = ME2Indicators[19].IndTAmount;
        }
        public void SetTotalMathTypeStock20()
        {
            ME2Indicators[20].IndTAmount = this.GetTotalFromMathExpression(20, _colNames, ME2Indicators[20].IndMathExpression, new List<double>(xcols - 5));
            ME2Indicators[20].IndTMAmount = ME2Indicators[20].IndTAmount;
        }

        public IDictionary<int, List<List<double>>> GetDataSet(List<string> lines)
        {
            //standard format is:
            //colNames = Indicator Label, Col1, Col2, QT Amount, Q1 Amount,	Q2 Amount, Q3 Amount, Q4 Amount, Q5 Amount, up to 11 numeric cols
            //matrix of doubles
            IDictionary<int, List<List<double>>> dataSets = new Dictionary<int, List<List<double>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //188
                            DataColNames = row;
                            //set the full colnames for the dataset
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                                return null;
                            }
                        }
                        else
                        {
                            _rowNames = new string[lines.Count() - 1];
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    List<string> cLines = lines
                                        .Where(l => l.StartsWith(sKey, true, CultureInfo.InvariantCulture))
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //rowNames can be retrieved from second col in cLines

                                        //generate an enumerable collection of doubles
                                        IEnumerable<IEnumerable<double>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //skip label, customcol1 and customcol2 columns
                                            let amounts = elements.Skip(3)
                                            select (from a in amounts
                                                    select CalculatorHelpers.ConvertStringToDouble(a));
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<double>> dataSet = new List<List<double>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                //ok for algos that don't need to calculate qT from q1 to q10 vars
                                                dataSet.Add(qvector.ToList());
                                                //otherwise should use GetDataSetwithQT(), not this function
                                            }
                                            dataSets.Add(iKey, dataSet);
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return dataSets;
        }
        public IDictionary<int, List<List<double>>> GetDataSetFull(List<string> lines)
        {
            //standard format is:
            //colNames = Indicator Label, Col1, Col2, QT Amount, Q1 Amount,	Q2 Amount, Q3 Amount, Q4 Amount, Q5 Amount, up to 11 numeric cols
            //matrix of doubles
            IDictionary<int, List<List<double>>> dataSets = new Dictionary<int, List<List<double>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            //second iteration fills dataset with complete doubles and exits loop
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //188
                            DataColNames = row;
                            //set the full colnames for the dataset
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                                return null;
                            }
                        }
                        else
                        {
                            _rowNames = new string[lines.Count() - 1];
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    //select all but the first row (the header row)
                                    List<string> cLines = lines
                                        .Skip(1)
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //rowNames can be retrieved from second col in cLines

                                        //generate an enumerable collection of doubles
                                        IEnumerable<IEnumerable<double>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //skip label, customcol1 and customcol2 columns
                                            let amounts = elements.Skip(3)
                                            select (from a in amounts
                                                    select CalculatorHelpers.ConvertStringToDouble(a));
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<double>> dataSet = new List<List<double>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                //ok for algos that don't need to calculate qT from q1 to q10 vars
                                                dataSet.Add(qvector.ToList());
                                            }
                                            dataSets.Add(iKey, dataSet);
                                            //no need for any more iterations, just exist with all rows
                                            return dataSets;
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return dataSets;
        }
        public IDictionary<int, List<List<string>>> GetDataSetFull2(List<string> lines)
        {
            //groups data for running datasets concurrently
            //standard format is:
            //colNames = Indicator Label, Col1, Col2, QT Amount, Q1 Amount,	Q2 Amount, Q3 Amount, Q4 Amount, Q5 Amount, up to 11 numeric cols
            //matrix of doubles
            IDictionary<int, List<List<string>>> dataSets = new Dictionary<int, List<List<string>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            //second iteration fills dataset with complete doubles and exits loop
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //190
                            DataColNames = row;
                            //set the full colnames for the dataset
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                                return null;
                            }
                        }
                        else
                        {
                            _rowNames = new string[lines.Count() - 1];
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    //select all but the first row (the header row)
                                    List<string> cLines = lines
                                        .Skip(1)
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //rowNames can be retrieved from second col in cLines

                                        //generate an enumerable collection of doubles
                                        IEnumerable<IEnumerable<string>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //skip label, customcol1 and customcol2 columns
                                            let amounts = elements.Skip(3)
                                            select (from a in amounts
                                                    select a);
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<string>> dataSet = new List<List<string>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                dataSet.Add(qvector.ToList());
                                            }
                                            dataSets.Add(iKey, dataSet);
                                            //no need for any more iterations, just exist with all rows
                                            return dataSets;
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return dataSets;
        }
        public IDictionary<int, List<List<string>>> GetDataSetFull3(List<string> lines)
        {
            //groups data for running datasets concurrently
            //standard format is:
            //colNames = Indicator Label, Col1, Col2, QT Amount, Q1 Amount,	Q2 Amount, Q3 Amount, Q4 Amount, Q5 Amount, up to 11 numeric cols
            //matrix of doubles
            IDictionary<int, List<List<string>>> dataSets = new Dictionary<int, List<List<string>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            //second iteration fills dataset with complete doubles and exits loop
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //190
                            DataColNames = row;
                            //set the full colnames for the dataset
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                                return null;
                            }
                        }
                        else
                        {
                            _rowNames = new string[lines.Count() - 1];
                            //the location column is the grouping variable (zip codes)
                            sKey = cols[2];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    //select all but the first row (the header row)
                                    List<string> cLines = lines
                                        .Skip(1)
                                        .Where(l => l.Equals(sKey))
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //rowNames can be retrieved from second col in cLines

                                        //generate an enumerable collection of doubles
                                        IEnumerable<IEnumerable<string>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //skip label, customcol1 and customcol2 columns
                                            let amounts = elements.Skip(3)
                                            select (from a in amounts
                                                    select a);
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<string>> dataSet = new List<List<string>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                dataSet.Add(qvector.ToList());
                                            }
                                            dataSets.Add(iKey, dataSet);
                                            //no need for any more iterations, just exist with all rows
                                            return dataSets;
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return dataSets;
        }
        public IDictionary<int, List<List<string>>> GetColumnSet(List<string> lines)
        {
            //matrix of strings
            IDictionary<int, List<List<string>>> colSets = new Dictionary<int, List<List<string>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //skip it
                        }
                        else
                        {
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    List<string> cLines = lines
                                        .Where(l => l.StartsWith(sKey, true, CultureInfo.InvariantCulture))
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //generate an enumerable collection of strings
                                        IEnumerable<IEnumerable<string>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //take label, customcol1 and customcol2 columns
                                            let amounts = elements.Take(3)
                                            select (from a in amounts
                                                    select a);
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<string>> colSet = new List<List<string>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                //ok for algos that don't need to calculate qT from q1 to q10 vars
                                                colSet.Add(qvector.ToList());
                                            }
                                            colSets.Add(iKey, colSet);
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return colSets;
        }
        public IDictionary<int, List<List<string>>> GetColumnSetFull(List<string> lines)
        {
            //matrix of strings
            IDictionary<int, List<List<string>>> colSets = new Dictionary<int, List<List<string>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //skip it
                        }
                        else
                        {
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    List<string> cLines = lines
                                        .Skip(1)
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //generate an enumerable collection of strings
                                        IEnumerable<IEnumerable<string>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //take label, customcol1 and customcol2 columns
                                            let amounts = elements.Take(3)
                                            select (from a in amounts
                                                    select a);
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<string>> colSet = new List<List<string>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                //ok for algos that don't need to calculate qT from q1 to q10 vars
                                                colSet.Add(qvector.ToList());
                                            }
                                            colSets.Add(iKey, colSet);
                                            //finished so return
                                            return colSets;
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return colSets;
        }
        public bool SetColumnNames(List<string> lines)
        {
            bool bHasColNames = false;
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0 && i == 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //188
                            DataColNames = row;
                            //set the full colnames for the dataset
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                            }
                            else
                            {
                                bHasColNames = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return bHasColNames;
        }
        private int GetIndicatorIndex(string[] cols)
        {
            int iIndIndex = 0;
            if (cols.Count() > 0)
            {
                //must have at least 1 dep col and 1 ind col 
                //the dep column is not included in Math Expressions
                iIndIndex = GetIndicatorIndex(cols, cols[0]);
            }
            return iIndIndex;
        }
        private int GetIndicatorIndex(string[] cols, string col)
        {
            int iIndIndex = 0;
            if (iIndIndex == 0)
            {
                //i == 0 is the dep column and not included in MathExpressions
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[1].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 1;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[2].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 2;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[3].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 3;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[4].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 4;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[5].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 5;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[6].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 6;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[7].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 7;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[8].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 8;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[9].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 9;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[10].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 10;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[11].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 11;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[12].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 12;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[13].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 13;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[14].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 14;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[15].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 15;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[16].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 16;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[17].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 17;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[18].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 18;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[19].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 19;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            if (iIndIndex == 0)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[20].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        iIndIndex = 20;
                    }
                    else
                    {
                        iIndIndex = 0;
                        break;
                    }
                }
            }
            return iIndIndex;
        }
        private string GetIndicatorLabel(string[] cols)
        {
            string sIndLabel = string.Empty;
            if (cols.Count() > 0)
            {
                //must have at least 1 dep col and 1 ind col 
                //the dep column is not included in Math Expressions
                sIndLabel = GetIndicatorLabel(cols, cols[0]);
            }
            return sIndLabel;
        }
        private string GetIndicatorLabel(string[] cols, string col)
        {
            string sIndLabel = string.Empty;

            if (sIndLabel == string.Empty)
            {
                //i == 0 is the dep column and not included in MathExpressions
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[1].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[1].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[2].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[2].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[3].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[3].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[4].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[4].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[5].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[5].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[6].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[6].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[7].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[7].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[8].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[8].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[9].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[9].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[10].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[10].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[11].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[11].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[12].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[12].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[13].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[13].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[14].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[14].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[15].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[15].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[16].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[16].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[17].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[17].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[18].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[18].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[19].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[19].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            if (sIndLabel == string.Empty)
            {
                for (int i = 1; i < cols.Count(); i++)
                {
                    if (ME2Indicators[20].IndMathExpression.ToLower().Contains(cols[i].ToLower()))
                    {
                        sIndLabel = ME2Indicators[20].IndLabel;
                    }
                    else
                    {
                        sIndLabel = string.Empty;
                        break;
                    }
                }
            }
            return sIndLabel;
        }
        public IDictionary<int, List<List<string>>> GetDataSet2(List<string> lines)
        {
            //standard format is:
            //colNames = Indicator Label, Col1, Col2, QT Amount, Q1 Amount,	Q2 Amount, Q3 Amount, Q4 Amount, Q5 Amount, up to 11 numeric cols
            //matrix of doubles
            IDictionary<int, List<List<string>>> dataSets = new Dictionary<int, List<List<string>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //188
                            DataColNames = row;
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                                return null;
                            }
                        }
                        else
                        {
                            _rowNames = new string[lines.Count() - 1];
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    List<string> cLines = lines
                                        .Where(l => l.StartsWith(sKey, true, CultureInfo.InvariantCulture))
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //generate an enumerable collection of strings
                                        IEnumerable<IEnumerable<string>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //skip label, customcol1 and customcol2 columns
                                            let amounts = elements.Skip(3)
                                            select (from a in amounts select a);
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            List<List<string>> dataSet = new List<List<string>>();
                                            //set qx
                                            foreach (var qvector in qs)
                                            {
                                                //ok for algos that don't need to calculate qT from q1 to q10 vars
                                                if (HasMathType(MATH_TYPES.algorithm2)
                                                    || HasMathType(MATH_TYPES.algorithm3)
                                                    || HasMathType(MATH_TYPES.algorithm4))
                                                {
                                                    dataSet.Add(qvector.ToList());
                                                }
                                            }
                                            dataSets.Add(iKey, dataSet);
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return dataSets;
        }
        private IDictionary<int, List<List<double>>> GetDataSet1(int dataIndex)
        {
            //ok to return data with 0 members
            IDictionary<int, List<List<double>>> data = new Dictionary<int, List<List<double>>>();
            if (!string.IsNullOrEmpty(DataURL)
                && DataURL != Constants.NONE)
            {
                string[] dataURLs = DataURL.Split(Constants.STRING_DELIMITERS);
                List<string> dlines = new List<string>();
                if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                    || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
                {
                    dlines = GetDataLines(dataURLs[dataIndex]);
                    //async returns too fast - the xml is written before calcs have been added
                    //dlines = await GetDataLinesAsync(dataURLs[dataIndex]);
                }
                if (dlines != null)
                {
                    //reset the data
                    data = new Dictionary<int, List<List<double>>>();
                    //it's ok for subsequent datasets to overwrite previous results (by design)
                    data = GetDataSetwithQT(dlines);
                    //if null already has an error message
                    if (data != null)
                    {
                        foreach (var ds in data)
                        {
                            //this will handle algo2, 3, and 4 when they have data in a dataurl
                            //start with the descriptive stats for each column
                            SetIndicatorData(ds.Key, ds.Value);
                            //joint data means joint calcs must be run
                            //note that certain algos also mean the joint calcs must be run
                            if (string.IsNullOrEmpty(ME2Indicators[0].IndURL)
                                || (ME2Indicators[0].IndURL == Constants.NONE))
                            {
                                SetSeparateRanges(ds.Key);
                            }
                        }
                    }
                }
                else
                {
                    data = new Dictionary<int, List<List<double>>>();
                }
            }
            return data;
        }
        public IDictionary<int, List<List<double>>> GetDataSetwithQT(List<string> lines)
        {
            //standard format is:
            //colNames = Indicator Label, Col1, Col2, QT Amount, Q1 Amount,	Q2 Amount, Q3 Amount, Q4 Amount, Q5 Amount, up to 11 numeric cols
            //matrix of doubles
            IDictionary<int, List<List<double>>> dataSets = new Dictionary<int, List<List<double>>>();
            string rowName = string.Empty;
            string sKey = string.Empty;
            int iKey = 0;
            List<int> cKeysUsed = new List<int>();
            int i = 0;
            foreach (var row in lines)
            {
                if (row.Length > 0)
                {
                    string[] cols = row.Split(Constants.CSV_DELIMITERS);
                    if (cols.Length > 0)
                    {
                        //first row is column names
                        if (i == 0)
                        {
                            //188
                            DataColNames = row;
                            //set the full colnames for the dataset
                            _colNames = new string[cols.Count()];
                            cols.CopyTo(_colNames, 0);
                            if (_colNames.Count() == 0)
                            {
                                ME2Indicators[0].IndMathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATA_NOCOLUMNNAMES"));
                                return null;
                            }
                        }
                        else
                        {
                            _rowNames = new string[lines.Count() - 1];
                            //the indicator label is key
                            sKey = cols[0];
                            if (!string.IsNullOrEmpty(sKey))
                            {
                                iKey = CalculatorHelpers.ConvertStringToInt(sKey);
                                if (!cKeysUsed.Contains(iKey))
                                {
                                    cKeysUsed.Add(iKey);
                                    List<string> cLines = lines
                                        .Where(l => l.StartsWith(sKey, true, CultureInfo.InvariantCulture))
                                        .Select(l => l.ToString()).ToList();
                                    if (cLines.Count > 0)
                                    {
                                        //generate an enumerable collection of doubles
                                        IEnumerable<IEnumerable<double>> qryQs =
                                            from line in cLines
                                            let elements = line.Split(Constants.CSV_DELIMITERS)
                                            //skip label, customcol1 and customcol2 columns
                                            let amounts = elements.Skip(3)
                                            select (from a in amounts
                                                    select CalculatorHelpers.ConvertStringToDouble(a));
                                        //execute the qry and get a list; qry is a List<IEnumerable<double>>
                                        var qs = qryQs.ToList();
                                        if (qs.Count > 0)
                                        {
                                            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
                                            //mathterms define which qamount to send to algorith for predicting a given set of qxs
                                            List<string> mathTerms = new List<string>();
                                            //dependent var colNames found in MathExpression
                                            List<string> depColNames = new List<string>();
                                            if (iKey == 1
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[1].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[1].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 2
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[2].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[2].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 3
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[3].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[3].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 4
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[4].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[4].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 5
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[5].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[5].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 6
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[6].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[6].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 7
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[7].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[7].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 8
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[8].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[8].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 9
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[9].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[9].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 10
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[10].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[10].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 11
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[11].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[11].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 12
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[12].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[12].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 13
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[13].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[13].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 14
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[14].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[14].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 15
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[15].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[15].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 16
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[16].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[16].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 17
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[17].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[17].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 18
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[18].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[18].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 19
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[19].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[19].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else if (iKey == 19
                                                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[20].IndMathExpression))
                                            {
                                                //temporary object to run calcs
                                                algos.GetDataToAnalyzeColNames(iKey, ME2Indicators[20].IndMathExpression, _colNames, ref depColNames, ref mathTerms);
                                            }
                                            else
                                            {
                                                //ignore the row
                                            }
                                            //has to also calc qT for each row
                                            List<List<double>> dataSet = FillDataSet(sKey, qs, depColNames, mathTerms);
                                            dataSets.Add(iKey, dataSet);
                                        }
                                    }
                                    else
                                    {
                                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                                    }
                                }
                            }
                            else
                            {
                                CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                            }
                        }
                        i++;
                    }
                    else
                    {
                        CalculatorDescription += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
                    }
                }
            }
            return dataSets;
        }

        private List<List<double>> FillDataSet(string key, List<IEnumerable<double>> qs, List<string> depColNames, List<string> mathTerms)
        {
            //new dataset holding qs
            List<List<double>> dataSet = new List<List<double>>();
            double qT = 0;
            double q1 = 0;
            double q2 = 0;
            double q3 = 0;
            double q4 = 0;
            double q5 = 0;
            double q6 = 0;
            double q7 = 0;
            double q8 = 0;
            double q9 = 0;
            double q10 = 0;
            //number of columns from first row
            int columnCount = qs[0].Count();
            //set qx
            foreach (var indicator in qs)
            {
                //qT is first because regression uses it as dependent variable
                qT = 0;
                q1 = 0;
                q2 = 0;
                q3 = 0;
                q4 = 0;
                q6 = 0;
                q5 = 0;
                q7 = 0;
                q8 = 0;
                q9 = 0;
                q10 = 0;
                List<double> qCalcs = new List<double>(xcols - 5);
                for (int column = 0; column < columnCount; column++)
                {
                    if (column == 0)
                    {
                        qT = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                    }
                    else if (column == 1)
                    {
                        q1 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q1);
                    }
                    else if (column == 2)
                    {
                        q2 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q2);
                    }
                    else if (column == 3)
                    {
                        q3 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q3);
                    }
                    else if (column == 4)
                    {
                        q4 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q4);
                    }
                    else if (column == 5)
                    {
                        q5 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q5);
                    }
                    else if (column == 6)
                    {
                        q6 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q6);
                    }
                    else if (column == 7)
                    {
                        q7 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q7);
                    }
                    else if (column == 8)
                    {
                        q8 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q8);
                    }
                    else if (column == 9)
                    {
                        q9 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q9);
                    }
                    else if (column == 10)
                    {
                        q10 = CalculatorHelpers.ConvertStringToDouble(indicator.ElementAt(column).ToString());
                        qCalcs.Add(q10);
                    }
                }
                //don't set QT for alg 6: regression must store an actual QT in the dataset
                //other algos have different rules
                if (!HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5)
                    && !HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                    && !HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7)
                    && !HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8)
                    && !HasMathType(MATH_TYPES.algorithm2)
                    && !HasMathType(MATH_TYPES.algorithm3)
                    && !HasMathType(MATH_TYPES.algorithm4))
                {
                    int iKey = CalculatorHelpers.ConvertStringToInt(key);
                    qT = GetQT(iKey, qT, qCalcs, depColNames, mathTerms);
                }
                var newIndicator = new List<double>(xcols - 5);
                //keep the order
                //rpackage requires a na when data is missing-here just use colnames to find right cols
                newIndicator.Add(qT);
                //most algos base their matrix on actual data
                //don't allow unnecessary cols in data
                if (columnCount >= 2)
                {
                    newIndicator.Add(q1);
                }
                if (columnCount >= 3)
                {
                    newIndicator.Add(q2);
                }
                if (columnCount >= 4)
                {
                    newIndicator.Add(q3);
                }
                if (columnCount >= 5)
                {
                    newIndicator.Add(q4);
                }
                if (columnCount >= 6)
                {
                    newIndicator.Add(q5);
                }
                if (columnCount >= 7)
                {
                    newIndicator.Add(q6);
                }
                if (columnCount >= 8)
                {
                    newIndicator.Add(q7);
                }
                if (columnCount >= 9)
                {
                    newIndicator.Add(q8);
                }
                if (columnCount >= 10)
                {
                    newIndicator.Add(q9);
                }
                if (columnCount >= 11)
                {
                    newIndicator.Add(q10);
                }
                dataSet.Add(newIndicator);
            }
            return dataSet;
        }
        private double GetQT(int index, double qT, List<double> qCalcs, List<string> depColNames, List<string> mathTerms)
        {
            double qTCalc = 0;
            List<double> morevars = new List<double>(xcols - 5);
            //need a temporary object to calc QT -calc comes from properties of object
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            if (index == 1
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[1].IndMathExpression))
            {
                //188 allows dep var from dataURLs to be included in math express -more flexible (i.e. anova can't use dummy vars but still wants desc stats for treatments ...)
                algos.ME2Indicators[1].IndTAmount = qT;
                //set the calcs for mathexpression
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                //use the temp qs to set qT (must use standard math express format)
                qTCalc = algos.GetTotalFromMathExpression(1, _colNames, algos.ME2Indicators[1].IndMathExpression, morevars);
            }
            else if (index == 2
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[2].IndMathExpression))
            {
                algos.ME2Indicators[2].IndTAmount = qT;
                //set the calcs for mathexpression
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(2, _colNames, algos.ME2Indicators[2].IndMathExpression, morevars);
            }
            else if (index == 3
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[3].IndMathExpression))
            {
                algos.ME2Indicators[3].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(3, _colNames, algos.ME2Indicators[3].IndMathExpression, morevars);
            }
            else if (index == 4
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[4].IndMathExpression))
            {
                algos.ME2Indicators[4].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(4, _colNames, algos.ME2Indicators[4].IndMathExpression, morevars);
            }
            else if (index == 5
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[5].IndMathExpression))
            {
                algos.ME2Indicators[5].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(5, _colNames, algos.ME2Indicators[5].IndMathExpression, morevars);
            }
            else if (index == 6
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[6].IndMathExpression))
            {
                algos.ME2Indicators[6].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(6, _colNames, algos.ME2Indicators[6].IndMathExpression, morevars);
            }
            else if (index == 7
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[7].IndMathExpression))
            {
                algos.ME2Indicators[7].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(7, _colNames, algos.ME2Indicators[7].IndMathExpression, morevars);
            }
            else if (index == 8
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[8].IndMathExpression))
            {
                algos.ME2Indicators[8].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(8, _colNames, algos.ME2Indicators[8].IndMathExpression, morevars);
            }
            else if (index == 9
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[9].IndMathExpression))
            {
                algos.ME2Indicators[9].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(9, _colNames, algos.ME2Indicators[9].IndMathExpression, morevars);
            }
            else if (index == 10
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[10].IndMathExpression))
            {
                algos.ME2Indicators[10].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(10, _colNames, algos.ME2Indicators[10].IndMathExpression, morevars);
            }
            else if (index == 11
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[11].IndMathExpression))
            {
                algos.ME2Indicators[11].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(11, _colNames, algos.ME2Indicators[11].IndMathExpression, morevars);
            }
            else if (index == 12
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[12].IndMathExpression))
            {
                algos.ME2Indicators[12].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(12, _colNames, algos.ME2Indicators[12].IndMathExpression, morevars);
            }
            else if (index == 13
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[13].IndMathExpression))
            {
                algos.ME2Indicators[13].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(13, _colNames, algos.ME2Indicators[13].IndMathExpression, morevars);
            }
            else if (index == 14
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[14].IndMathExpression))
            {
                algos.ME2Indicators[14].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(14, _colNames, algos.ME2Indicators[14].IndMathExpression, morevars);
            }
            else if (index == 15
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[15].IndMathExpression))
            {
                algos.ME2Indicators[15].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(15, _colNames, algos.ME2Indicators[15].IndMathExpression, morevars);
            }
            else if (index == 16
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[16].IndMathExpression))
            {
                algos.ME2Indicators[16].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(16, _colNames, algos.ME2Indicators[16].IndMathExpression, morevars);
            }
            else if (index == 17
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[17].IndMathExpression))
            {
                algos.ME2Indicators[17].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(17, _colNames, algos.ME2Indicators[17].IndMathExpression, morevars);
            }
            else if (index == 18
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[18].IndMathExpression))
            {
                algos.ME2Indicators[18].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(18, _colNames, algos.ME2Indicators[18].IndMathExpression, morevars);
            }
            else if (index == 19
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[19].IndMathExpression))
            {
                algos.ME2Indicators[19].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(19, _colNames, algos.ME2Indicators[19].IndMathExpression, morevars);
            }
            else if (index == 20
                 && ME2Statistics.ME2Algos.HasMathExpression(algos.ME2Indicators[20].IndMathExpression))
            {
                algos.ME2Indicators[20].IndTAmount = qT;
                morevars = SetQsForMathTerms(algos, index, mathTerms, qCalcs);
                qTCalc = algos.GetTotalFromMathExpression(20, _colNames, algos.ME2Indicators[20].IndMathExpression, morevars);
            }
            else
            {
                //ignore the row
            }
            return qTCalc;
        }
        public List<double> SetQsForMathTerms(ME2Indicator baseCalcor, int index, List<string> mathTerms,
            List<double> qs)
        {
            List<double> morevars = new List<double>(xcols - 5);
            for (var v = 0; v < (xcols - 5); v++)
            {
                morevars.Add(0);
            }
            int i = 0;
            //the mathterms order was used to set qs order -so they will correspond correctly
            //and will have same size
            //the units must be set correctly
            //and the mathexpress has to contain the var
            if (index == 1)
            {
                if (HasMathTerm(mathTerms, 0, 0))
                {
                    baseCalcor.ME2Indicators[1].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 1))
                {
                    baseCalcor.ME2Indicators[1].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 2))
                {
                    baseCalcor.ME2Indicators[1].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 3))
                {
                    baseCalcor.ME2Indicators[1].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 4))
                {
                    baseCalcor.ME2Indicators[1].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 0, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 2)
            {
                if (HasMathTerm(mathTerms, 1, 0))
                {
                    baseCalcor.ME2Indicators[2].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 1))
                {
                    baseCalcor.ME2Indicators[2].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 2))
                {
                    baseCalcor.ME2Indicators[2].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 3))
                {
                    baseCalcor.ME2Indicators[2].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 4))
                {
                    baseCalcor.ME2Indicators[2].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 1, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 3)
            {
                if (HasMathTerm(mathTerms, 2, 0))
                {
                    baseCalcor.ME2Indicators[3].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 1))
                {
                    baseCalcor.ME2Indicators[3].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 2))
                {
                    baseCalcor.ME2Indicators[3].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 3))
                {
                    baseCalcor.ME2Indicators[3].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 4))
                {
                    baseCalcor.ME2Indicators[3].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 2, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 4)
            {
                if (HasMathTerm(mathTerms, 3, 0))
                {
                    baseCalcor.ME2Indicators[4].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 1))
                {
                    baseCalcor.ME2Indicators[4].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 2))
                {
                    baseCalcor.ME2Indicators[4].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 3))
                {
                    baseCalcor.ME2Indicators[4].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 4))
                {
                    baseCalcor.ME2Indicators[4].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 3, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 5)
            {
                if (HasMathTerm(mathTerms, 4, 0))
                {
                    baseCalcor.ME2Indicators[5].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 1))
                {
                    baseCalcor.ME2Indicators[5].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 2))
                {
                    baseCalcor.ME2Indicators[5].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 3))
                {
                    baseCalcor.ME2Indicators[5].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 4))
                {
                    baseCalcor.ME2Indicators[5].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 4, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 6)
            {
                if (HasMathTerm(mathTerms, 5, 0))
                {
                    baseCalcor.ME2Indicators[6].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 1))
                {
                    baseCalcor.ME2Indicators[6].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 2))
                {
                    baseCalcor.ME2Indicators[6].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 3))
                {
                    baseCalcor.ME2Indicators[6].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 4))
                {
                    baseCalcor.ME2Indicators[6].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 5, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 7)
            {
                if (HasMathTerm(mathTerms, 6, 0))
                {
                    baseCalcor.ME2Indicators[7].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 1))
                {
                    baseCalcor.ME2Indicators[7].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 2))
                {
                    baseCalcor.ME2Indicators[7].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 3))
                {
                    baseCalcor.ME2Indicators[7].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 4))
                {
                    baseCalcor.ME2Indicators[7].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 6, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 8)
            {
                if (HasMathTerm(mathTerms, 7, 0))
                {
                    baseCalcor.ME2Indicators[8].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 1))
                {
                    baseCalcor.ME2Indicators[8].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 2))
                {
                    baseCalcor.ME2Indicators[8].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 3))
                {
                    baseCalcor.ME2Indicators[8].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 4))
                {
                    baseCalcor.ME2Indicators[8].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 7, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 9)
            {
                if (HasMathTerm(mathTerms, 8, 0))
                {
                    baseCalcor.ME2Indicators[9].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 1))
                {
                    baseCalcor.ME2Indicators[9].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 2))
                {
                    baseCalcor.ME2Indicators[9].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 3))
                {
                    baseCalcor.ME2Indicators[9].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 4))
                {
                    baseCalcor.ME2Indicators[9].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 8, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 10)
            {
                if (HasMathTerm(mathTerms, 9, 0))
                {
                    baseCalcor.ME2Indicators[10].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 1))
                {
                    baseCalcor.ME2Indicators[10].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 2))
                {
                    baseCalcor.ME2Indicators[10].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 3))
                {
                    baseCalcor.ME2Indicators[10].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 4))
                {
                    baseCalcor.ME2Indicators[10].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 9, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 11)
            {
                if (HasMathTerm(mathTerms, 10, 0))
                {
                    baseCalcor.ME2Indicators[11].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 1))
                {
                    baseCalcor.ME2Indicators[11].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 2))
                {
                    baseCalcor.ME2Indicators[11].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 3))
                {
                    baseCalcor.ME2Indicators[11].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 4))
                {
                    baseCalcor.ME2Indicators[11].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 10, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 12)
            {
                if (HasMathTerm(mathTerms, 11, 0))
                {
                    baseCalcor.ME2Indicators[12].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 1))
                {
                    baseCalcor.ME2Indicators[12].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 2))
                {
                    baseCalcor.ME2Indicators[12].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 3))
                {
                    baseCalcor.ME2Indicators[12].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 4))
                {
                    baseCalcor.ME2Indicators[12].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 11, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 13)
            {
                if (HasMathTerm(mathTerms, 12, 0))
                {
                    baseCalcor.ME2Indicators[13].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 1))
                {
                    baseCalcor.ME2Indicators[13].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 2))
                {
                    baseCalcor.ME2Indicators[13].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 3))
                {
                    baseCalcor.ME2Indicators[13].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 4))
                {
                    baseCalcor.ME2Indicators[13].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 12, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 14)
            {
                if (HasMathTerm(mathTerms, 13, 0))
                {
                    baseCalcor.ME2Indicators[14].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 1))
                {
                    baseCalcor.ME2Indicators[14].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 2))
                {
                    baseCalcor.ME2Indicators[14].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 3))
                {
                    baseCalcor.ME2Indicators[14].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 4))
                {
                    baseCalcor.ME2Indicators[14].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 13, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 15)
            {
                if (HasMathTerm(mathTerms, 14, 0))
                {
                    baseCalcor.ME2Indicators[15].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 1))
                {
                    baseCalcor.ME2Indicators[15].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 2))
                {
                    baseCalcor.ME2Indicators[15].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 3))
                {
                    baseCalcor.ME2Indicators[15].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 4))
                {
                    baseCalcor.ME2Indicators[15].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 14, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 16)
            {
                if (HasMathTerm(mathTerms, 15, 0))
                {
                    baseCalcor.ME2Indicators[16].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 1))
                {
                    baseCalcor.ME2Indicators[16].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 2))
                {
                    baseCalcor.ME2Indicators[16].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 3))
                {
                    baseCalcor.ME2Indicators[16].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 4))
                {
                    baseCalcor.ME2Indicators[16].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 15, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 17)
            {
                if (HasMathTerm(mathTerms, 16, 0))
                {
                    baseCalcor.ME2Indicators[17].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 1))
                {
                    baseCalcor.ME2Indicators[17].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 2))
                {
                    baseCalcor.ME2Indicators[17].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 3))
                {
                    baseCalcor.ME2Indicators[17].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 4))
                {
                    baseCalcor.ME2Indicators[17].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 16, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 18)
            {
                if (HasMathTerm(mathTerms, 17, 0))
                {
                    baseCalcor.ME2Indicators[18].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 1))
                {
                    baseCalcor.ME2Indicators[18].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 2))
                {
                    baseCalcor.ME2Indicators[18].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 3))
                {
                    baseCalcor.ME2Indicators[18].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 4))
                {
                    baseCalcor.ME2Indicators[18].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 17, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 19)
            {
                if (HasMathTerm(mathTerms, 18, 0))
                {
                    baseCalcor.ME2Indicators[19].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 1))
                {
                    baseCalcor.ME2Indicators[19].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 2))
                {
                    baseCalcor.ME2Indicators[19].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 3))
                {
                    baseCalcor.ME2Indicators[19].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 4))
                {
                    baseCalcor.ME2Indicators[19].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 18, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            if (index == 20)
            {
                if (HasMathTerm(mathTerms, 19, 0))
                {
                    baseCalcor.ME2Indicators[20].Ind1Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 1))
                {
                    baseCalcor.ME2Indicators[20].Ind2Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 2))
                {
                    baseCalcor.ME2Indicators[20].Ind3Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 3))
                {
                    baseCalcor.ME2Indicators[20].Ind4Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 4))
                {
                    baseCalcor.ME2Indicators[20].Ind5Amount = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 11))
                {
                    morevars[0] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 12))
                {
                    morevars[1] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 13))
                {
                    morevars[2] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 14))
                {
                    morevars[3] = qs[i];
                    i++;
                }
                if (HasMathTerm(mathTerms, 19, 15))
                {
                    morevars[4] = qs[i];
                    i++;
                }
            }
            return morevars;
        }
        public static bool HasMathTerm(List<string> mathTerms, int rowNumber, int colNumber)
        {
            bool bHasTerm = false;
            foreach (var mathterm in mathTerms)
            {
                //mathterm = colname = I1.Q1.housesize
                //GetMathTerm = I1.Q1
                if (mathterm.Contains(GetMathTerm(rowNumber, colNumber)))
                {
                    return true;
                }
            }
            return bHasTerm;
        }
        public static bool HasMathTerm(string mathTerm, int rowNumber, int colNumber)
        {
            bool bHasTerm = false;
            //mathterm = colname = I1.Q1.housesize
            //GetMathTerm = I1.Q1
            if (mathTerm.Contains(GetMathTerm(rowNumber, colNumber)))
            {
                return true;
            }
            return bHasTerm;
        }
        private static string GetMathTerm(int rowNumber, int colNumber)
        {
            string sMathTerm = string.Empty;
            int iMTIndex = (16 * rowNumber) + colNumber;
            bool bHasIndex = MATHTERMS.Any(s => MATHTERMS[iMTIndex] != null);
            if (bHasIndex)
            {
                sMathTerm = MATHTERMS[iMTIndex];
            }
            return sMathTerm;
        }
        public int SetIndicatorData(int index, List<List<double>> data)
        {
            int iAlgoIndicator = 0;
            if (data.Count > 0)
            {
                //get colcount from first row
                int iColCount = data[0].Count;
                IEnumerable<double> qTs = new List<double>();
                IEnumerable<double> q1s = new List<double>();
                IEnumerable<double> q2s = new List<double>();
                IEnumerable<double> q3s = new List<double>();
                IEnumerable<double> q4s = new List<double>();
                IEnumerable<double> q5s = new List<double>();
                IEnumerable<double> q6s = new List<double>();
                IEnumerable<double> q7s = new List<double>();
                IEnumerable<double> q8s = new List<double>();
                IEnumerable<double> q9s = new List<double>();
                IEnumerable<double> q10s = new List<double>();
                //need column data, not row data, data has been formatted with 11 cols
                //qT col is required for all data sets
                //data.count gives col count
                if (iColCount > 0)
                {
                    qTs = from row in data select row.ElementAt(0);
                }
                //so is 1 col of ind vars
                //data.count gives col count
                if (iColCount > 1)
                {
                    q1s = from row in data select row.ElementAt(1);
                }
                else
                {
                    //must have at least 1 dep var
                    return index;
                }
                //data.count gives col count
                if (iColCount > 2)
                {
                    q2s = from row in data select row.ElementAt(2);
                }
                if (iColCount > 3)
                {
                    q3s = from row in data select row.ElementAt(3);
                }
                if (iColCount > 4)
                {
                    q4s = from row in data select row.ElementAt(4);
                }
                if (iColCount > 5)
                {
                    q5s = from row in data select row.ElementAt(5);
                }
                if (iColCount > 6)
                {
                    q6s = from row in data select row.ElementAt(6);
                }
                if (iColCount > 7)
                {
                    q7s = from row in data select row.ElementAt(7);
                }
                if (iColCount > 8)
                {
                    q8s = from row in data select row.ElementAt(8);
                }
                if (iColCount > 9)
                {
                    q9s = from row in data select row.ElementAt(9);
                }
                if (iColCount > 10)
                {
                    q10s = from row in data select row.ElementAt(10);
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Empty);
                //means
                if (qTs.Count() > 0)
                {
                    sb.AppendLine("observed means");
                    sb.Append(string.Concat("QT mean = ", Math.Round(qTs.Average(), 4), ", "));
                }
                if (q1s.Count() > 0)
                {
                    sb.Append(string.Concat("Q1 mean = ", Math.Round(q1s.Average(), 4), ", "));
                }
                if (q2s.Count() > 0)
                {
                    sb.Append(string.Concat("Q2 mean = ", Math.Round(q2s.Average(), 4), ", "));
                }
                if (q3s.Count() > 0)
                {
                    sb.Append(string.Concat("Q3 mean = ", Math.Round(q3s.Average(), 4), ", "));
                }
                if (q4s.Count() > 0)
                {
                    sb.Append(string.Concat("Q4 mean = ", Math.Round(q4s.Average(), 4), ", "));
                }
                if (q5s.Count() > 0)
                {
                    sb.Append(string.Concat("Q5 mean = ", Math.Round(q5s.Average(), 4), ", "));
                }
                if (q6s.Count() > 0)
                {
                    sb.Append(string.Concat("Q6 mean = ", Math.Round(q6s.Average(), 4), ", "));
                }
                if (q7s.Count() > 0)
                {
                    sb.Append(string.Concat("Q7 mean = ", Math.Round(q7s.Average(), 4), ", "));
                }
                if (q8s.Count() > 0)
                {
                    sb.Append(string.Concat("Q8 mean = ", Math.Round(q8s.Average(), 4), ", "));
                }
                if (q9s.Count() > 0)
                {
                    sb.Append(string.Concat("Q9 mean = ", Math.Round(q9s.Average(), 4), ", "));
                }
                if (q10s.Count() > 0)
                {
                    sb.Append(string.Concat("Q10 mean = ", Math.Round(q10s.Average(), 4), ", "));
                }
                int iSiblingIndicator = 0;
                if (index == 1
                    && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[1].IndMathExpression))
                {
                    ME2Indicators[1].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    //ME2Indicators[1].IndMathResult = GetObservedMathResult(index, ME2Indicators[1].IndMathSubType, SB1Type1, 
                    //    ME2Indicators[1].IndMathType, qTs, qTs.Average());
                    ME2Indicators[1].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    //indicator1 should not have a sibling later, but keep the pattern consistent
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 1, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(1, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(1, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 2
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[2].IndMathExpression))
                {
                    ME2Indicators[2].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[2].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 2, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(2, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(2, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 3
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[3].IndMathExpression))
                {
                    ME2Indicators[3].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[3].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 3, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(3, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(3, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 4
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[4].IndMathExpression))
                {
                    ME2Indicators[4].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[4].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 4, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(4, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(4, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 5
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[5].IndMathExpression))
                {
                    ME2Indicators[5].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[5].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 5, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(5, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(5, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 6
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[6].IndMathExpression))
                {
                    ME2Indicators[6].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[6].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 6, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(6, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(6, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 7
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[7].IndMathExpression))
                {
                    ME2Indicators[7].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[7].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 7, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(7, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(7, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 8
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[8].IndMathExpression))
                {
                    ME2Indicators[8].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[8].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 8, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(8, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(8, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 9
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[9].IndMathExpression))
                {
                    ME2Indicators[9].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[9].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 9, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(9, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(9, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 10
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[10].IndMathExpression))
                {
                    ME2Indicators[10].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[10].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 10, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(10, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(10, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 11
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[11].IndMathExpression))
                {
                    ME2Indicators[11].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[11].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 11, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(11, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(11, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 12
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[12].IndMathExpression))
                {
                    ME2Indicators[12].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[12].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 12, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(12, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(12, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 13
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[13].IndMathExpression))
                {
                    ME2Indicators[13].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[13].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 13, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(13, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(13, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 14
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[14].IndMathExpression))
                {
                    ME2Indicators[14].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[14].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 14, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(14, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(14, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 15
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[15].IndMathExpression))
                {
                    ME2Indicators[15].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[15].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 15, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(15, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(15, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 16
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[16].IndMathExpression))
                {
                    ME2Indicators[16].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[16].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 16, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(16, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(16, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 17
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[17].IndMathExpression))
                {
                    ME2Indicators[17].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[17].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 17, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(17, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(17, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 18
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[18].IndMathExpression))
                {
                    ME2Indicators[18].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[18].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 18, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(18, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(18, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 19
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[19].IndMathExpression))
                {
                    ME2Indicators[19].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[19].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 19, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(19, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(19, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                if (index == 20
                     && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[20].IndMathExpression))
                {
                    ME2Indicators[20].IndTAmount = Math.Round(qTs.Average(), 4);
                    SetAlgoPRAStats(index, qTs.ToList());
                    ME2Indicators[20].IndMathResult += sb.ToString();
                    iAlgoIndicator = index;
                    iSiblingIndicator = ME2Statistics.ME2Algos.GetSiblingIndicatorIndex(index, 20, this);
                    if (iSiblingIndicator != 0)
                    {
                        SetIndicatorQxMeans(iSiblingIndicator, q1s, q2s, q3s, q4s, q5s);
                        SetIndicatorQxMeans(20, q6s, q7s, q8s, q9s, q10s);
                    }
                    else
                    {
                        SetIndicatorQxMeans(20, q1s, q2s, q3s, q4s, q5s);
                    }
                }
                else
                {
                    //ignore the row
                }
            }
            return iAlgoIndicator;
        }
        public void SetIndicatorQxMeans(int indicatorIndex, IEnumerable<double> q1s, IEnumerable<double> q2s,
            IEnumerable<double> q3s, IEnumerable<double> q4s, IEnumerable<double> q5s)
        {
            //the enumerables can be q1s to q11s
            if (indicatorIndex == 0)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[0].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[0].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[0].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[0].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[0].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 1)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[1].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[1].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[1].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[1].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[1].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 2)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[2].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[2].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[2].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[2].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[2].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 3)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[3].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[3].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[3].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[3].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[3].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 4)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[4].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[4].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[4].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[4].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[4].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 5)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[5].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[5].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[5].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[5].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[5].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 6)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[6].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[6].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[6].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[6].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[6].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 7)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[7].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[7].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[7].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[7].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[7].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 8)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[8].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[8].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[8].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[8].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[8].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 9)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[9].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[9].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[9].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[9].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[9].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 10)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[10].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[10].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[10].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[10].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[10].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 11)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[11].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[11].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[11].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[11].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[11].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 12)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[12].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[12].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[12].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[12].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[12].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 13)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[13].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[13].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[13].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[13].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[13].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 14)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[14].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[14].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[14].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[14].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[14].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 15)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[15].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[15].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[15].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[15].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[15].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 16)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[16].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[16].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[16].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[16].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[16].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 17)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[17].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[17].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[17].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[17].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[17].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 18)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[18].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[18].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[18].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[18].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[18].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 19)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[19].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[19].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[19].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[19].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[19].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
            else if (indicatorIndex == 20)
            {
                if (q1s.Count() > 0)
                    ME2Indicators[20].Ind1Amount = Math.Round(q1s.Average(), 4);
                if (q2s.Count() > 0)
                    ME2Indicators[20].Ind2Amount = Math.Round(q2s.Average(), 4);
                if (q3s.Count() > 0)
                    ME2Indicators[20].Ind3Amount = Math.Round(q3s.Average(), 4);
                if (q4s.Count() > 0)
                    ME2Indicators[20].Ind4Amount = Math.Round(q4s.Average(), 4);
                if (q5s.Count() > 0)
                    ME2Indicators[20].Ind5Amount = Math.Round(q5s.Average(), 4);
            }
        }
        public void SetSeparateRanges(int index)
        {
            List<double> qTs = new List<double>();
            if (index == 1
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[1].IndMathExpression))
            {
                //regular high and low estimation
                SetAlgoPRAStats(1, qTs);
                //SetTotalRange1();
            }
            else if (index == 2
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[2].IndMathExpression))
            {
                SetAlgoPRAStats(2, qTs);
            }
            else if (index == 3
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[3].IndMathExpression))
            {
                SetAlgoPRAStats(3, qTs);
            }
            else if (index == 4
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[4].IndMathExpression))
            {
                SetAlgoPRAStats(4, qTs);
            }
            else if (index == 5
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[5].IndMathExpression))
            {
                SetAlgoPRAStats(5, qTs);
            }
            else if (index == 6
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[6].IndMathExpression))
            {
                SetAlgoPRAStats(6, qTs);
            }
            else if (index == 7
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[7].IndMathExpression))
            {
                SetAlgoPRAStats(7, qTs);
            }
            else if (index == 8
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[8].IndMathExpression))
            {
                SetAlgoPRAStats(8, qTs);
            }
            else if (index == 9
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[9].IndMathExpression))
            {
                SetAlgoPRAStats(9, qTs);
            }
            else if (index == 10
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[10].IndMathExpression))
            {
                SetAlgoPRAStats(10, qTs);
            }
            else if (index == 11
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[11].IndMathExpression))
            {
                SetAlgoPRAStats(11, qTs);
            }
            else if (index == 12
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[12].IndMathExpression))
            {
                SetAlgoPRAStats(12, qTs);
            }
            else if (index == 13
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[13].IndMathExpression))
            {
                SetAlgoPRAStats(13, qTs);
            }
            else if (index == 14
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[14].IndMathExpression))
            {
                SetAlgoPRAStats(14, qTs);
            }
            else if (index == 15
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[15].IndMathExpression))
            {
                SetAlgoPRAStats(15, qTs);
            }
            else if (index == 16
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[16].IndMathExpression))
            {
                SetAlgoPRAStats(16, qTs);
            }
            else if (index == 17
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[17].IndMathExpression))
            {
                SetAlgoPRAStats(17, qTs);
            }
            else if (index == 18
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[18].IndMathExpression))
            {
                SetAlgoPRAStats(18, qTs);
            }
            else if (index == 19
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[19].IndMathExpression))
            {
                SetAlgoPRAStats(19, qTs);
            }
            else if (index == 20
                 && ME2Statistics.ME2Algos.HasMathExpression(ME2Indicators[20].IndMathExpression))
            {
                SetAlgoPRAStats(20, qTs);
            }
            else
            {
                //ignore the row
            }
        }

        public async Task<int> SetAlgoPRAStats(int index, List<double> qTs, double[] data = null)
        {
            string sError = string.Empty;
            //init the algos using this
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            int iIndicatorIndex = await algos.SetAlgoPRAStats(index, _colNames, qTs, data);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            //188 added support for analyzers
            MathResult = algos.MathResult;
            DataToAnalyze = algos.DataToAnalyze;
            //goes in calcparams because it has to be passed back to anorhelper for manual insertion into calcdoc linkedview
            CalcParameters.MathResult = MathResult;
            return iIndicatorIndex;
        }
        private async Task<string> SetAlgoCorrStats(string jdataURL, IDictionary<int, List<List<double>>> data)
        {
            string sError = string.Empty;
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            string sIndicatorIndexes = await algos.SetAlgoCorrIndicatorStats(jdataURL, data, _colNames);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            //188 added support for analyzers
            MathResult = algos.MathResult;
            DataToAnalyze = algos.DataToAnalyze;
            //goes in calcparams because it has to be passed back to anorhelper for manual insertion into calcdoc linkedview
            CalcParameters.MathResult = MathResult;
            return sIndicatorIndexes;
        }
        private async Task<int> SetAlgoStats1(int index, List<List<double>> data)
        {
            string sError = string.Empty;
            //init the algos using this
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            int iIndicatorIndex = await algos.SetAlgoIndicatorStats1(index, data, _colNames);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            //188 added support for analyzers
            MathResult = algos.MathResult;
            DataToAnalyze = algos.DataToAnalyze;
            //goes in calcparams because it has to be passed back to anorhelper for manual insertion into calcdoc linkedview
            CalcParameters.MathResult = MathResult;
            return iIndicatorIndex;
        }
        private async Task<int> SetAlgoStats2(int index, string dataURL, string jDataURL)
        {
            string sError = string.Empty;
            //init the algos using this
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            int iIndicatorIndex
                = await algos.SetAlgoIndicatorStats2(index, _colNames, dataURL, jDataURL);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            return iIndicatorIndex;
        }
        private async Task<int> SetAlgoStats3(int index, List<List<string>> data,
            List<List<string>> colData, List<string> lines2)
        {
            string sError = string.Empty;
            //init the algos using this
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            int iIndicatorIndex
                = await algos.SetAlgoIndicatorStats3(index, data, colData, lines2, _colNames);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            return iIndicatorIndex;
        }
        private async Task<int> SetAlgoStats4(int index, List<List<string>> data,
            List<List<string>> colData, List<string> lines2)
        {
            string sError = string.Empty;
            //init the algos using this
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            int iIndicatorIndex
                = await algos.SetAlgoIndicatorStats4(index, data, colData, lines2, _colNames);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            return iIndicatorIndex;
        }
        private async Task<int> SetAlgoCalcs(int index, List<List<double>> data)
        {
            string sError = string.Empty;
            //init the algos using this
            ME2Statistics.ME2Algos algos = new ME2Statistics.ME2Algos(this);
            int iIndicatorIndex = await algos.SetAlgoIndicatorCalcs(index, data);
            //copy all of the results back to this
            CopyME2IndicatorsProperties(algos);
            return iIndicatorIndex;
        }

    }

    public static class ME2IndicatorExtensions
    {
        public static void SetTotalScore(this ME2Indicator baseCalc, string[] colNames)
        {
            if (baseCalc.ME2Indicators[0].IndMathExpression == string.Empty)
            {
                baseCalc.ME2Indicators[0].IndMathExpression = Constants.NONE;
                return;
            }
            //scorem is set when the distribution and mathtype run
            baseCalc.ME2Indicators[0].IndTAmount = baseCalc.GetTotalFromMathExpression(0, colNames, baseCalc.ME2Indicators[0].IndMathExpression, new List<double>(ME2Indicator.xcols - 5));
            baseCalc.ME2Indicators[0].IndTMAmount = baseCalc.ME2Indicators[0].IndTAmount;
            if (!string.IsNullOrEmpty(baseCalc.ErrorMessage))
            {
                baseCalc.CalculatorDescription += baseCalc.ErrorMessage;
                baseCalc.ErrorMessage = string.Empty;
            }
            if ((baseCalc.ME2Indicators[0].IndTMUnit == string.Empty || baseCalc.ME2Indicators[0].IndTMUnit == Constants.NONE) && baseCalc.ME2Indicators[0].IndTMAmount != 0)
                baseCalc.ME2Indicators[0].IndTMUnit = "most likely";
            if ((baseCalc.ME2Indicators[0].IndTLUnit == string.Empty || baseCalc.ME2Indicators[0].IndTLUnit == Constants.NONE) && baseCalc.ME2Indicators[0].IndTLAmount != 0)
                baseCalc.ME2Indicators[0].IndTLUnit = "low estimate";
            if ((baseCalc.ME2Indicators[0].IndTUUnit == string.Empty || baseCalc.ME2Indicators[0].IndTUUnit == Constants.NONE) && baseCalc.ME2Indicators[0].IndTUAmount != 0)
                baseCalc.ME2Indicators[0].IndTUUnit = "high estimate";
        }


        public static double GetTotalFromMathExpression(this ME2Indicator baseCalc, int indicator, string[] colNames,
            string mathExpress, List<double> morevars)
        {
            double dbTotal = 0;
            if (morevars == null)
            {
                morevars = new List<double>(ME2Indicator.xcols - 5);
            }
            if (morevars.Count < 5)
            {
                for (var v = 0; v < (ME2Indicator.xcols - 5); v++)
                {
                    morevars.Add(0);
                }
            }
            if (!string.IsNullOrEmpty(mathExpress))
            {
                string sMathExpress = mathExpress;
                //must get rid of colnames from mathexpression
                if (colNames != null)
                {
                    for (int i = 0; i < colNames.Count(); i++)
                    {
                        sMathExpress = sMathExpress
                            .Replace(string.Concat(Constants.FILEEXTENSION_DELIMITER, colNames[i]), string.Empty);
                    }
                }
                //lower case
                sMathExpress = sMathExpress.Replace("q", "Q");
                sMathExpress = sMathExpress.Replace("i", "I");
                //
                //sibling vars
                sMathExpress = sMathExpress.Replace("I1.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                //has to come before I1.Q1
                sMathExpress = sMathExpress.Replace("I1.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q1", baseCalc.ME2Indicators[1].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q2", baseCalc.ME2Indicators[1].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q3", baseCalc.ME2Indicators[1].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q4", baseCalc.ME2Indicators[1].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.Q5", baseCalc.ME2Indicators[1].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.QTM", baseCalc.ME2Indicators[1].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.QTD1", baseCalc.ME2Indicators[1].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.QTD2", baseCalc.ME2Indicators[1].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.QTL", baseCalc.ME2Indicators[1].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.QTU", baseCalc.ME2Indicators[1].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I1.QT", baseCalc.ME2Indicators[1].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I2.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q1", baseCalc.ME2Indicators[2].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q2", baseCalc.ME2Indicators[2].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q3", baseCalc.ME2Indicators[2].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q4", baseCalc.ME2Indicators[2].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.Q5", baseCalc.ME2Indicators[2].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.QTM", baseCalc.ME2Indicators[2].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.QTD1", baseCalc.ME2Indicators[2].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.QTD2", baseCalc.ME2Indicators[2].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.QTL", baseCalc.ME2Indicators[2].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.QTU", baseCalc.ME2Indicators[2].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I2.QT", baseCalc.ME2Indicators[2].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I3.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q1", baseCalc.ME2Indicators[3].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q2", baseCalc.ME2Indicators[3].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q3", baseCalc.ME2Indicators[3].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q4", baseCalc.ME2Indicators[3].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.Q5", baseCalc.ME2Indicators[3].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.QTM", baseCalc.ME2Indicators[3].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.QTD1", baseCalc.ME2Indicators[3].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.QTD2", baseCalc.ME2Indicators[3].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.QTL", baseCalc.ME2Indicators[3].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.QTU", baseCalc.ME2Indicators[3].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I3.QT", baseCalc.ME2Indicators[3].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I4.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q1", baseCalc.ME2Indicators[4].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q2", baseCalc.ME2Indicators[4].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q3", baseCalc.ME2Indicators[4].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q4", baseCalc.ME2Indicators[4].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.Q5", baseCalc.ME2Indicators[4].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.QTM", baseCalc.ME2Indicators[4].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.QTD1", baseCalc.ME2Indicators[4].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.QTD2", baseCalc.ME2Indicators[4].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.QTL", baseCalc.ME2Indicators[4].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.QTU", baseCalc.ME2Indicators[4].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I4.QT", baseCalc.ME2Indicators[4].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));


                sMathExpress = sMathExpress.Replace("I5.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q1", baseCalc.ME2Indicators[5].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q2", baseCalc.ME2Indicators[5].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q3", baseCalc.ME2Indicators[5].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q4", baseCalc.ME2Indicators[5].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.Q5", baseCalc.ME2Indicators[5].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.QTM", baseCalc.ME2Indicators[5].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.QTD1", baseCalc.ME2Indicators[5].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.QTD2", baseCalc.ME2Indicators[5].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.QTL", baseCalc.ME2Indicators[5].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.QTU", baseCalc.ME2Indicators[5].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I5.QT", baseCalc.ME2Indicators[5].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I6.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q1", baseCalc.ME2Indicators[6].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q2", baseCalc.ME2Indicators[6].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q3", baseCalc.ME2Indicators[6].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q4", baseCalc.ME2Indicators[6].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.Q5", baseCalc.ME2Indicators[6].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.QTM", baseCalc.ME2Indicators[6].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.QTD1", baseCalc.ME2Indicators[6].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.QTD2", baseCalc.ME2Indicators[6].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.QTL", baseCalc.ME2Indicators[6].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.QTU", baseCalc.ME2Indicators[6].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I6.QT", baseCalc.ME2Indicators[6].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I7.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q1", baseCalc.ME2Indicators[7].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q2", baseCalc.ME2Indicators[7].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q3", baseCalc.ME2Indicators[7].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q4", baseCalc.ME2Indicators[7].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.Q5", baseCalc.ME2Indicators[7].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.QTM", baseCalc.ME2Indicators[7].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.QTD1", baseCalc.ME2Indicators[7].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.QTD2", baseCalc.ME2Indicators[7].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.QTL", baseCalc.ME2Indicators[7].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.QTU", baseCalc.ME2Indicators[7].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I7.QT", baseCalc.ME2Indicators[7].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I8.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q1", baseCalc.ME2Indicators[8].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q2", baseCalc.ME2Indicators[8].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q3", baseCalc.ME2Indicators[8].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q4", baseCalc.ME2Indicators[8].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.Q5", baseCalc.ME2Indicators[8].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.QTM", baseCalc.ME2Indicators[8].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.QTD1", baseCalc.ME2Indicators[8].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.QTD2", baseCalc.ME2Indicators[8].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.QTL", baseCalc.ME2Indicators[8].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.QTU", baseCalc.ME2Indicators[8].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I8.QT", baseCalc.ME2Indicators[8].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I9.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q1", baseCalc.ME2Indicators[9].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q2", baseCalc.ME2Indicators[9].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q3", baseCalc.ME2Indicators[9].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q4", baseCalc.ME2Indicators[9].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.Q5", baseCalc.ME2Indicators[9].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.QTM", baseCalc.ME2Indicators[9].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.QTD1", baseCalc.ME2Indicators[9].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.QTD2", baseCalc.ME2Indicators[9].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.QTL", baseCalc.ME2Indicators[9].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.QTU", baseCalc.ME2Indicators[9].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I9.QT", baseCalc.ME2Indicators[9].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I10.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q1", baseCalc.ME2Indicators[10].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q2", baseCalc.ME2Indicators[10].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q3", baseCalc.ME2Indicators[10].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q4", baseCalc.ME2Indicators[10].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.Q5", baseCalc.ME2Indicators[10].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.QTM", baseCalc.ME2Indicators[10].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.QTD1", baseCalc.ME2Indicators[10].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.QTD2", baseCalc.ME2Indicators[10].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.QTL", baseCalc.ME2Indicators[10].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.QTU", baseCalc.ME2Indicators[10].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I10.QT", baseCalc.ME2Indicators[10].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I11.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q1", baseCalc.ME2Indicators[11].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q2", baseCalc.ME2Indicators[11].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q3", baseCalc.ME2Indicators[11].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q4", baseCalc.ME2Indicators[11].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.Q5", baseCalc.ME2Indicators[11].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.QTM", baseCalc.ME2Indicators[11].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.QTD1", baseCalc.ME2Indicators[11].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.QTD2", baseCalc.ME2Indicators[11].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.QTL", baseCalc.ME2Indicators[11].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.QTU", baseCalc.ME2Indicators[11].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I11.QT", baseCalc.ME2Indicators[11].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I12.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q1", baseCalc.ME2Indicators[12].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q2", baseCalc.ME2Indicators[12].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q3", baseCalc.ME2Indicators[12].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q4", baseCalc.ME2Indicators[12].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.Q5", baseCalc.ME2Indicators[12].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.QTM", baseCalc.ME2Indicators[12].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.QTD1", baseCalc.ME2Indicators[12].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.QTD2", baseCalc.ME2Indicators[12].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.QTL", baseCalc.ME2Indicators[12].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.QTU", baseCalc.ME2Indicators[12].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I12.QT", baseCalc.ME2Indicators[12].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I13.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q1", baseCalc.ME2Indicators[13].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q2", baseCalc.ME2Indicators[13].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q3", baseCalc.ME2Indicators[13].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q4", baseCalc.ME2Indicators[13].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.Q5", baseCalc.ME2Indicators[13].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.QTM", baseCalc.ME2Indicators[13].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.QTD1", baseCalc.ME2Indicators[13].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.QTD2", baseCalc.ME2Indicators[13].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.QTL", baseCalc.ME2Indicators[13].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.QTU", baseCalc.ME2Indicators[13].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I13.QT", baseCalc.ME2Indicators[13].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I14.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q1", baseCalc.ME2Indicators[14].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q2", baseCalc.ME2Indicators[14].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q3", baseCalc.ME2Indicators[14].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q4", baseCalc.ME2Indicators[14].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.Q5", baseCalc.ME2Indicators[14].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.QTM", baseCalc.ME2Indicators[14].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.QTD1", baseCalc.ME2Indicators[14].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.QTD2", baseCalc.ME2Indicators[14].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.QTL", baseCalc.ME2Indicators[14].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.QTU", baseCalc.ME2Indicators[14].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I14.QT", baseCalc.ME2Indicators[14].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I15.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q1", baseCalc.ME2Indicators[15].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q2", baseCalc.ME2Indicators[15].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q3", baseCalc.ME2Indicators[15].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q4", baseCalc.ME2Indicators[15].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.Q5", baseCalc.ME2Indicators[15].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.QTM", baseCalc.ME2Indicators[15].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.QTD1", baseCalc.ME2Indicators[15].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.QTD2", baseCalc.ME2Indicators[15].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.QTL", baseCalc.ME2Indicators[15].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.QTU", baseCalc.ME2Indicators[15].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I15.QT", baseCalc.ME2Indicators[15].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I16.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q1", baseCalc.ME2Indicators[16].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q2", baseCalc.ME2Indicators[16].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q3", baseCalc.ME2Indicators[16].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q4", baseCalc.ME2Indicators[16].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.Q5", baseCalc.ME2Indicators[16].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.QTM", baseCalc.ME2Indicators[16].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.QTD1", baseCalc.ME2Indicators[16].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.QTD2", baseCalc.ME2Indicators[16].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.QTL", baseCalc.ME2Indicators[16].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.QTU", baseCalc.ME2Indicators[16].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I16.QT", baseCalc.ME2Indicators[16].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I17.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q1", baseCalc.ME2Indicators[17].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q2", baseCalc.ME2Indicators[17].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q3", baseCalc.ME2Indicators[17].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q4", baseCalc.ME2Indicators[17].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.Q5", baseCalc.ME2Indicators[17].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.QTM", baseCalc.ME2Indicators[17].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.QTD1", baseCalc.ME2Indicators[17].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.QTD2", baseCalc.ME2Indicators[17].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.QTL", baseCalc.ME2Indicators[17].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.QTU", baseCalc.ME2Indicators[17].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I17.QT", baseCalc.ME2Indicators[17].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I18.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q1", baseCalc.ME2Indicators[18].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q2", baseCalc.ME2Indicators[18].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q3", baseCalc.ME2Indicators[18].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q4", baseCalc.ME2Indicators[18].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.Q5", baseCalc.ME2Indicators[18].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.QTM", baseCalc.ME2Indicators[18].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.QTD1", baseCalc.ME2Indicators[18].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.QTD2", baseCalc.ME2Indicators[18].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.QTL", baseCalc.ME2Indicators[18].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.QTU", baseCalc.ME2Indicators[18].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I18.QT", baseCalc.ME2Indicators[18].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I19.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q1", baseCalc.ME2Indicators[19].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q2", baseCalc.ME2Indicators[19].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q3", baseCalc.ME2Indicators[19].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q4", baseCalc.ME2Indicators[19].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.Q5", baseCalc.ME2Indicators[19].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.QTM", baseCalc.ME2Indicators[19].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.QTD1", baseCalc.ME2Indicators[19].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.QTD2", baseCalc.ME2Indicators[19].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.QTL", baseCalc.ME2Indicators[19].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.QTU", baseCalc.ME2Indicators[19].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I19.QT", baseCalc.ME2Indicators[19].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                sMathExpress = sMathExpress.Replace("I20.Q6", morevars[0].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q7", morevars[1].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q8", morevars[2].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q9", morevars[3].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q10", morevars[4].ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q1", baseCalc.ME2Indicators[20].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q2", baseCalc.ME2Indicators[20].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q3", baseCalc.ME2Indicators[20].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q4", baseCalc.ME2Indicators[20].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.Q5", baseCalc.ME2Indicators[20].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.QTM", baseCalc.ME2Indicators[20].IndTMAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.QTD1", baseCalc.ME2Indicators[20].IndTD1Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.QTD2", baseCalc.ME2Indicators[20].IndTD2Amount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.QTL", baseCalc.ME2Indicators[20].IndTLAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.QTU", baseCalc.ME2Indicators[20].IndTUAmount.ToString("N4", CultureInfo.InvariantCulture));
                sMathExpress = sMathExpress.Replace("I20.QT", baseCalc.ME2Indicators[20].IndTAmount.ToString("N4", CultureInfo.InvariantCulture));

                //self variables (no self for ind = 0, scores set from other inds)
                if (indicator == 1)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[1].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[1].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[1].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[1].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[1].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 2)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[2].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[2].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[2].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[2].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[2].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 3)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[3].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[3].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[3].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[3].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[3].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 4)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[4].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[4].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[4].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[4].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[4].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 5)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[5].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[5].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[5].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[5].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[5].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 6)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[6].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[6].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[6].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[6].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[6].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 7)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[7].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[7].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[7].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[7].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[7].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 8)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[8].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[8].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[8].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[8].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[8].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 9)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[9].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[9].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[9].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[9].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[9].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 10)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[10].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[10].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[10].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[10].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[10].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 11)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[11].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[11].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[11].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[11].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[11].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 12)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[12].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[12].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[12].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[12].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[12].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 13)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[13].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[13].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[13].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[13].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[13].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 14)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[14].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[14].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[14].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[14].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[14].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 15)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[15].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[15].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[15].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[15].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[15].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 16)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[16].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[16].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[16].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[16].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[16].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 17)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[17].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[17].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[17].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[17].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[17].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 18)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[18].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[18].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[18].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[18].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[18].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 19)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[19].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[19].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[19].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[19].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[19].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 20)
                {
                    sMathExpress = sMathExpress.Replace("Q1", baseCalc.ME2Indicators[20].Ind1Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q2", baseCalc.ME2Indicators[20].Ind2Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q3", baseCalc.ME2Indicators[20].Ind3Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q4", baseCalc.ME2Indicators[20].Ind4Amount.ToString("N4", CultureInfo.InvariantCulture));
                    sMathExpress = sMathExpress.Replace("Q5", baseCalc.ME2Indicators[20].Ind5Amount.ToString("N4", CultureInfo.InvariantCulture));
                }
                else if (indicator == 0)
                {
                    //not applicable to score
                }
                //get rid of any constants or letters that can interfere with parse engine
                sMathExpress = sMathExpress.Replace(Constants.CSV_DELIMITER, string.Empty);
                sMathExpress = sMathExpress.Replace("A", string.Empty);
                sMathExpress = sMathExpress.Replace("B", string.Empty);
                sMathExpress = sMathExpress.Replace("C", string.Empty);
                sMathExpress = sMathExpress.Replace("D", string.Empty);
                sMathExpress = sMathExpress.Replace("E", string.Empty);
                sMathExpress = sMathExpress.Replace("F", string.Empty);
                sMathExpress = sMathExpress.Replace("G", string.Empty);
                sMathExpress = sMathExpress.Replace("H", string.Empty);
                sMathExpress = sMathExpress.Replace("I", string.Empty);
                sMathExpress = sMathExpress.Replace("J", string.Empty);
                sMathExpress = sMathExpress.Replace("K", string.Empty);
                sMathExpress = sMathExpress.Replace("L", string.Empty);
                sMathExpress = sMathExpress.Replace("M", string.Empty);
                sMathExpress = sMathExpress.Replace("N", string.Empty);
                sMathExpress = sMathExpress.Replace("O", string.Empty);
                sMathExpress = sMathExpress.Replace("P", string.Empty);
                sMathExpress = sMathExpress.Replace("Q", string.Empty);
                sMathExpress = sMathExpress.Replace("R", string.Empty);
                sMathExpress = sMathExpress.Replace("S", string.Empty);
                sMathExpress = sMathExpress.Replace("T", string.Empty);
                sMathExpress = sMathExpress.Replace("U", string.Empty);
                sMathExpress = sMathExpress.Replace("V", string.Empty);
                sMathExpress = sMathExpress.Replace("W", string.Empty);
                sMathExpress = sMathExpress.Replace("X", string.Empty);
                sMathExpress = sMathExpress.Replace("Y", string.Empty);
                sMathExpress = sMathExpress.Replace("Z", string.Empty);

                sMathExpress = sMathExpress.Replace(@"\", string.Empty);
                //don't remove nan =points out that an error needs to be fixed
                //sMathExpress = sMathExpress.Replace("NaN", string.Empty);
                //1.9.0 allows multiple vars to be used in expressions, but the algorithm handles the math, not this
                if (sMathExpress.Contains(".QN"))
                {
                    sMathExpress = string.Empty;
                }
                //must have some numbers substituted
                if (sMathExpress != mathExpress
                    && (!string.IsNullOrEmpty(sMathExpress)))
                {
                    try
                    {
                        Jace.CalculationEngine engine = new Jace.CalculationEngine();
                        dbTotal = engine.Calculate(sMathExpress);
                    }
                    catch (Exception x)
                    {
                        if (indicator != 0)
                        {
                            baseCalc.ErrorMessage +=
                                string.Concat(" Ind", indicator.ToString(), ": ", x.Message, Errors.MakeStandardErrorMsg("JACE_BASIC"));
                        }
                        else
                        {
                            baseCalc.ErrorMessage +=
                                string.Concat(" Score: ", x.Message, Errors.MakeStandardErrorMsg("JACE_BASIC"));
                        }
                    }
                }
            }
            dbTotal = Math.Round(dbTotal, 4);
            return dbTotal;
        }

    }
}
