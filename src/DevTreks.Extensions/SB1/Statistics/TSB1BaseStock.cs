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
    ///Purpose:		The TSB1BaseStock class extends the CostBenefitCalculator() 
    ///             class and is used by Stock calculators and analyzers 
    ///             to set totals and basic Stock statistics. Basic 
    ///             Stock statistical objects derive from this class 
    ///             to support further statistical analysis.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. 
    ///</summary>
    public class TSB1BaseStock : CostBenefitCalculator
    {
        //calls the base-class version, and initializes the base class properties.
        public TSB1BaseStock()
            : base()
        {
            //base input
            InitCalculatorProperties();
            //benefits and costs are not used (yet) by these analyzers
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
            //Stock totals
            InitTSB1BaseStockProperties();
        }
        //copy constructor
        public TSB1BaseStock(TSB1BaseStock calculator)
            : base(calculator)
        {
            CopyTSB1BaseStockProperties(calculator);
        }
        #region Props
        public double TSB1Score { get; set; }
        public string TSB1ScoreUnit { get; set; }
        public double TSB1ScoreD1Amount { get; set; }
        public string TSB1ScoreD1Unit { get; set; }
        public double TSB1ScoreD2Amount { get; set; }
        public string TSB1ScoreD2Unit { get; set; }
        public string TSB1ScoreMathExpression { get; set; }
        public double TSB1ScoreM { get; set; }
        public string TSB1ScoreMUnit { get; set; }
        public double TSB1ScoreLAmount { get; set; }
        public string TSB1ScoreLUnit { get; set; }
        public double TSB1ScoreUAmount { get; set; }
        public string TSB1ScoreUUnit { get; set; }
        public string TSB1ScoreDistType { get; set; }
        public string TSB1ScoreMathType { get; set; }
        public string TSB1ScoreMathResult { get; set; }
        public string TSB1ScoreMathSubType { get; set; }
        //remember that Calculator1.DataToAnalyze holds aggregated data from calcs for algos
        public int TSB1Iterations { get; set; }
        //formulas don't work with ints
        public double TSB1ScoreN { get; set; }
        public const string cTSB1Score = "TSB1Score";
        public const string cTSB1ScoreUnit = "TSB1ScoreUnit";
        public const string cTSB1ScoreD1Amount = "TSB1ScoreD1Amount";
        public const string cTSB1ScoreD1Unit = "TSB1ScoreD1Unit";
        public const string cTSB1ScoreD2Amount = "TSB1ScoreD2Amount";
        public const string cTSB1ScoreD2Unit = "TSB1ScoreD2Unit";
        public const string cTSB1ScoreMathExpression = "TSB1ScoreMathExpression";
        public const string cTSB1ScoreM = "TSB1ScoreM";
        public const string cTSB1ScoreMUnit = "TSB1ScoreMUnit";
        public const string cTSB1ScoreLAmount = "TSB1ScoreLAmount";
        public const string cTSB1ScoreLUnit = "TSB1ScoreLUnit";
        public const string cTSB1ScoreUAmount = "TSB1ScoreUAmount";
        public const string cTSB1ScoreUUnit = "TSB1ScoreUUnit";
        public const string cTSB1Iterations = "TSB1Iterations";
        public const string cTSB1ScoreDistType = "TSB1ScoreDistType";
        public const string cTSB1ScoreMathType = "TSB1ScoreMathType";
        public const string cTSB1ScoreMathResult = "TSB1ScoreMathResult";
        public const string cTSB1ScoreMathSubType = "TSB1ScoreMathSubType";
        public const string cTSB1ScoreN = "TSB1ScoreN";
        //comparisons to display
        public double SBCount { get; set; }
        public const string cSBCount = "SBCount";

        //name of indicator 1
        public string TSB1Name1 { get; set; }
        //description
        public string TSB1Description1 { get; set; }
        //aggregation label
        public string TSB1Label1 { get; set; }
        //RUC_TYPES or distribution enum
        public string TSB1Type1 { get; set; }
        //date of indicator measurement
        public DateTime TSB1Date1 { get; set; }
        //algorithm1 = basic stats ...
        public string TSB1MathType1 { get; set; }
        public string TSB1BaseIO1 { get; set; }
        //first quantitative prop
        //amount
        public double TSB11Amount1 { get; set; }
        public string TSB11Unit1 { get; set; }
        //second quantity
        public double TSB12Amount1 { get; set; }
        //second unit
        public string TSB12Unit1 { get; set; }
        //third quantity
        public double TSB13Amount1 { get; set; }
        public string TSB13Unit1 { get; set; }
        public double TSB14Amount1 { get; set; }
        public string TSB14Unit1 { get; set; }
        //total of the two indicators (p*q = cost)
        public double TSB15Amount1 { get; set; }
        //unit for total (i.e. hours physical activity, cost, benefit, number (stock groups)
        public string TSB15Unit1 { get; set; }
        //related indicator label i.e. emissions and env performance
        public string TSB1RelLabel1 { get; set; }
        public double TSB1TAmount1 { get; set; }
        public string TSB1TUnit1 { get; set; }
        public double TSB1TD1Amount1 { get; set; }
        public string TSB1TD1Unit1 { get; set; }
        public double TSB1TD2Amount1 { get; set; }
        public string TSB1TD2Unit1 { get; set; }
        public string TSB1MathResult1 { get; set; }
        public string TSB1MathSubType1 { get; set; }

        public double TSB1TMAmount1 { get; set; }
        public string TSB1TMUnit1 { get; set; }
        public double TSB1TLAmount1 { get; set; }
        public string TSB1TLUnit1 { get; set; }
        public double TSB1TUAmount1 { get; set; }
        public string TSB1TUUnit1 { get; set; }
        public string TSB1MathOperator1 { get; set; }
        public string TSB1MathExpression1 { get; set; }
        public double TSB1N1 { get; set; }

        public const string cTSB1Name1 = "TSB1Name1";
        public const string cTSB1Description1 = "TSB1Description1";
        public const string cTSB1Label1 = "TSB1Label1";
        public const string cTSB1Type1 = "TSB1Type1";
        public const string cTSB1Date1 = "TSB1Date1";
        public const string cTSB1MathType1 = "TSB1MathType1";
        public const string cTSB1BaseIO1 = "TSB1BaseIO1";
        public const string cTSB11Amount1 = "TSB11Amount1";
        public const string cTSB11Unit1 = "TSB11Unit1";
        public const string cTSB12Amount1 = "TSB12Amount1";
        public const string cTSB12Unit1 = "TSB12Unit1";
        public const string cTSB13Amount1 = "TSB13Amount1";
        public const string cTSB13Unit1 = "TSB13Unit1";
        public const string cTSB14Amount1 = "TSB14Amount1";
        public const string cTSB14Unit1 = "TSB14Unit1";
        public const string cTSB15Amount1 = "TSB15Amount1";
        public const string cTSB15Unit1 = "TSB15Unit1";
        public const string cTSB1RelLabel1 = "TSB1RelLabel1";
        public const string cTSB1TAmount1 = "TSB1TAmount1";
        public const string cTSB1TUnit1 = "TSB1TUnit1";
        public const string cTSB1TD1Amount1 = "TSB1TD1Amount1";
        public const string cTSB1TD1Unit1 = "TSB1TD1Unit1";
        public const string cTSB1TD2Amount1 = "TSB1TD2Amount1";
        public const string cTSB1TD2Unit1 = "TSB1TD2Unit1";
        public const string cTSB1MathResult1 = "TSB1MathResult1";
        public const string cTSB1MathSubType1 = "TSB1MathSubType1";

        public const string cTSB1TMAmount1 = "TSB1TMAmount1";
        public const string cTSB1TMUnit1 = "TSB1TMUnit1";
        public const string cTSB1TLAmount1 = "TSB1TLAmount1";
        public const string cTSB1TLUnit1 = "TSB1TLUnit1";
        public const string cTSB1TUAmount1 = "TSB1TUAmount1";
        public const string cTSB1TUUnit1 = "TSB1TUUnit1";
        public const string cTSB1MathOperator1 = "TSB1MathOperator1";
        public const string cTSB1MathExpression1 = "TSB1MathExpression1";
        public const string cTSB1N1 = "TSB1N1";

        //name of indicator 2
        public string TSB1Name2 { get; set; }
        public string TSB1Description2 { get; set; }
        public string TSB1Label2 { get; set; }
        public string TSB1Type2 { get; set; }
        public DateTime TSB1Date2 { get; set; }
        public string TSB1MathType2 { get; set; }
        public string TSB1BaseIO2 { get; set; }
        public double TSB11Amount2 { get; set; }
        public string TSB11Unit2 { get; set; }
        public double TSB12Amount2 { get; set; }
        public string TSB12Unit2 { get; set; }
        public double TSB13Amount2 { get; set; }
        public string TSB13Unit2 { get; set; }
        public double TSB14Amount2 { get; set; }
        public string TSB14Unit2 { get; set; }
        public double TSB15Amount2 { get; set; }
        public string TSB15Unit2 { get; set; }
        public string TSB1RelLabel2 { get; set; }
        public double TSB1TAmount2 { get; set; }
        public string TSB1TUnit2 { get; set; }
        public double TSB1TD1Amount2 { get; set; }
        public string TSB1TD1Unit2 { get; set; }
        public double TSB1TD2Amount2 { get; set; }
        public string TSB1TD2Unit2 { get; set; }
        public string TSB1MathResult2 { get; set; }
        public string TSB1MathSubType2 { get; set; }

        public double TSB1TMAmount2 { get; set; }
        public string TSB1TMUnit2 { get; set; }
        public double TSB1TLAmount2 { get; set; }
        public string TSB1TLUnit2 { get; set; }
        public double TSB1TUAmount2 { get; set; }
        public string TSB1TUUnit2 { get; set; }
        public string TSB1MathOperator2 { get; set; }
        public string TSB1MathExpression2 { get; set; }
        public double TSB1N2 { get; set; }

        public const string cTSB1Name2 = "TSB1Name2";
        public const string cTSB1Description2 = "TSB1Description2";
        public const string cTSB1Label2 = "TSB1Label2";
        public const string cTSB1Type2 = "TSB1Type2";
        public const string cTSB1Date2 = "TSB1Date2";
        public const string cTSB1MathType2 = "TSB1MathType2";
        public const string cTSB1BaseIO2 = "TSB1BaseIO2";
        public const string cTSB11Amount2 = "TSB11Amount2";
        public const string cTSB11Unit2 = "TSB11Unit2";
        public const string cTSB12Amount2 = "TSB12Amount2";
        public const string cTSB12Unit2 = "TSB12Unit2";
        public const string cTSB13Amount2 = "TSB13Amount2";
        public const string cTSB13Unit2 = "TSB13Unit2";
        public const string cTSB14Amount2 = "TSB14Amount2";
        public const string cTSB14Unit2 = "TSB14Unit2";
        public const string cTSB15Amount2 = "TSB15Amount2";
        public const string cTSB15Unit2 = "TSB15Unit2";
        public const string cTSB1RelLabel2 = "TSB1RelLabel2";
        public const string cTSB1TAmount2 = "TSB1TAmount2";
        public const string cTSB1TUnit2 = "TSB1TUnit2";
        public const string cTSB1TD1Amount2 = "TSB1TD1Amount2";
        public const string cTSB1TD1Unit2 = "TSB1TD1Unit2";
        public const string cTSB1TD2Amount2 = "TSB1TD2Amount2";
        public const string cTSB1TD2Unit2 = "TSB1TD2Unit2";
        public const string cTSB1MathResult2 = "TSB1MathResult2";
        public const string cTSB1MathSubType2 = "TSB1MathSubType2";

        public const string cTSB1TMAmount2 = "TSB1TMAmount2";
        public const string cTSB1TMUnit2 = "TSB1TMUnit2";
        public const string cTSB1TLAmount2 = "TSB1TLAmount2";
        public const string cTSB1TLUnit2 = "TSB1TLUnit2";
        public const string cTSB1TUAmount2 = "TSB1TUAmount2";
        public const string cTSB1TUUnit2 = "TSB1TUUnit2";
        public const string cTSB1MathOperator2 = "TSB1MathOperator2";
        public const string cTSB1MathExpression2 = "TSB1MathExpression2";
        public const string cTSB1N2 = "TSB1N2";

        //name of indicator 3
        public string TSB1Name3 { get; set; }
        public string TSB1Description3 { get; set; }
        public string TSB1Label3 { get; set; }
        public string TSB1Type3 { get; set; }
        public DateTime TSB1Date3 { get; set; }
        public string TSB1MathType3 { get; set; }
        public string TSB1BaseIO3 { get; set; }
        public double TSB11Amount3 { get; set; }
        public string TSB11Unit3 { get; set; }
        public double TSB12Amount3 { get; set; }
        public string TSB12Unit3 { get; set; }
        public double TSB13Amount3 { get; set; }
        public string TSB13Unit3 { get; set; }
        public double TSB14Amount3 { get; set; }
        public string TSB14Unit3 { get; set; }
        public double TSB15Amount3 { get; set; }
        public string TSB15Unit3 { get; set; }
        public string TSB1RelLabel3 { get; set; }
        public double TSB1TAmount3 { get; set; }
        public string TSB1TUnit3 { get; set; }
        public double TSB1TD1Amount3 { get; set; }
        public string TSB1TD1Unit3 { get; set; }
        public double TSB1TD2Amount3 { get; set; }
        public string TSB1TD2Unit3 { get; set; }
        public string TSB1MathResult3 { get; set; }
        public string TSB1MathSubType3 { get; set; }

        public double TSB1TMAmount3 { get; set; }
        public string TSB1TMUnit3 { get; set; }
        public double TSB1TLAmount3 { get; set; }
        public string TSB1TLUnit3 { get; set; }
        public double TSB1TUAmount3 { get; set; }
        public string TSB1TUUnit3 { get; set; }
        public string TSB1MathOperator3 { get; set; }
        public string TSB1MathExpression3 { get; set; }
        public double TSB1N3 { get; set; }

        public const string cTSB1Name3 = "TSB1Name3";
        public const string cTSB1Description3 = "TSB1Description3";
        public const string cTSB1Label3 = "TSB1Label3";
        public const string cTSB1Type3 = "TSB1Type3";
        public const string cTSB1Date3 = "TSB1Date3";
        public const string cTSB1MathType3 = "TSB1MathType3";
        public const string cTSB1BaseIO3 = "TSB1BaseIO3";
        public const string cTSB11Amount3 = "TSB11Amount3";
        public const string cTSB11Unit3 = "TSB11Unit3";
        public const string cTSB12Amount3 = "TSB12Amount3";
        public const string cTSB12Unit3 = "TSB12Unit3";
        public const string cTSB13Amount3 = "TSB13Amount3";
        public const string cTSB13Unit3 = "TSB13Unit3";
        public const string cTSB14Amount3 = "TSB14Amount3";
        public const string cTSB14Unit3 = "TSB14Unit3";
        public const string cTSB15Amount3 = "TSB15Amount3";
        public const string cTSB15Unit3 = "TSB15Unit3";
        public const string cTSB1RelLabel3 = "TSB1RelLabel3";
        public const string cTSB1TAmount3 = "TSB1TAmount3";
        public const string cTSB1TUnit3 = "TSB1TUnit3";
        public const string cTSB1TD1Amount3 = "TSB1TD1Amount3";
        public const string cTSB1TD1Unit3 = "TSB1TD1Unit3";
        public const string cTSB1TD2Amount3 = "TSB1TD2Amount3";
        public const string cTSB1TD2Unit3 = "TSB1TD2Unit3";
        public const string cTSB1MathResult3 = "TSB1MathResult3";
        public const string cTSB1MathSubType3 = "TSB1MathSubType3";

        public const string cTSB1TMAmount3 = "TSB1TMAmount3";
        public const string cTSB1TMUnit3 = "TSB1TMUnit3";
        public const string cTSB1TLAmount3 = "TSB1TLAmount3";
        public const string cTSB1TLUnit3 = "TSB1TLUnit3";
        public const string cTSB1TUAmount3 = "TSB1TUAmount3";
        public const string cTSB1TUUnit3 = "TSB1TUUnit3";
        public const string cTSB1MathOperator3 = "TSB1MathOperator3";
        public const string cTSB1MathExpression3 = "TSB1MathExpression3";
        public const string cTSB1N3 = "TSB1N3";

        //name of indicator 4
        public string TSB1Name4 { get; set; }
        public string TSB1Description4 { get; set; }
        public string TSB1Label4 { get; set; }
        public string TSB1Type4 { get; set; }
        public DateTime TSB1Date4 { get; set; }
        public string TSB1MathType4 { get; set; }
        public string TSB1BaseIO4 { get; set; }
        public double TSB11Amount4 { get; set; }
        public string TSB11Unit4 { get; set; }
        public double TSB12Amount4 { get; set; }
        public string TSB12Unit4 { get; set; }
        public double TSB13Amount4 { get; set; }
        public string TSB13Unit4 { get; set; }
        public double TSB14Amount4 { get; set; }
        public string TSB14Unit4 { get; set; }
        public double TSB15Amount4 { get; set; }
        public string TSB15Unit4 { get; set; }
        public string TSB1RelLabel4 { get; set; }
        public double TSB1TAmount4 { get; set; }
        public string TSB1TUnit4 { get; set; }
        public double TSB1TD1Amount4 { get; set; }
        public string TSB1TD1Unit4 { get; set; }
        public double TSB1TD2Amount4 { get; set; }
        public string TSB1TD2Unit4 { get; set; }
        public string TSB1MathResult4 { get; set; }
        public string TSB1MathSubType4 { get; set; }

        public double TSB1TMAmount4 { get; set; }
        public string TSB1TMUnit4 { get; set; }
        public double TSB1TLAmount4 { get; set; }
        public string TSB1TLUnit4 { get; set; }
        public double TSB1TUAmount4 { get; set; }
        public string TSB1TUUnit4 { get; set; }
        public string TSB1MathOperator4 { get; set; }
        public string TSB1MathExpression4 { get; set; }
        public double TSB1N4 { get; set; }

        public const string cTSB1Name4 = "TSB1Name4";
        public const string cTSB1Description4 = "TSB1Description4";
        public const string cTSB1Label4 = "TSB1Label4";
        public const string cTSB1Type4 = "TSB1Type4";
        public const string cTSB1Date4 = "TSB1Date4";
        public const string cTSB1MathType4 = "TSB1MathType4";
        public const string cTSB1BaseIO4 = "TSB1BaseIO4";
        public const string cTSB11Amount4 = "TSB11Amount4";
        public const string cTSB11Unit4 = "TSB11Unit4";
        public const string cTSB12Amount4 = "TSB12Amount4";
        public const string cTSB12Unit4 = "TSB12Unit4";
        public const string cTSB13Amount4 = "TSB13Amount4";
        public const string cTSB13Unit4 = "TSB13Unit4";
        public const string cTSB14Amount4 = "TSB14Amount4";
        public const string cTSB14Unit4 = "TSB14Unit4";
        public const string cTSB15Amount4 = "TSB15Amount4";
        public const string cTSB15Unit4 = "TSB15Unit4";
        public const string cTSB1RelLabel4 = "TSB1RelLabel4";
        public const string cTSB1TAmount4 = "TSB1TAmount4";
        public const string cTSB1TUnit4 = "TSB1TUnit4";
        public const string cTSB1TD1Amount4 = "TSB1TD1Amount4";
        public const string cTSB1TD1Unit4 = "TSB1TD1Unit4";
        public const string cTSB1TD2Amount4 = "TSB1TD2Amount4";
        public const string cTSB1TD2Unit4 = "TSB1TD2Unit4";
        public const string cTSB1MathResult4 = "TSB1MathResult4";
        public const string cTSB1MathSubType4 = "TSB1MathSubType4";

        public const string cTSB1TMAmount4 = "TSB1TMAmount4";
        public const string cTSB1TMUnit4 = "TSB1TMUnit4";
        public const string cTSB1TLAmount4 = "TSB1TLAmount4";
        public const string cTSB1TLUnit4 = "TSB1TLUnit4";
        public const string cTSB1TUAmount4 = "TSB1TUAmount4";
        public const string cTSB1TUUnit4 = "TSB1TUUnit4";
        public const string cTSB1MathOperator4 = "TSB1MathOperator4";
        public const string cTSB1MathExpression4 = "TSB1MathExpression4";
        public const string cTSB1N4 = "TSB1N4";

        //name of indicator 5
        public string TSB1Name5 { get; set; }
        public string TSB1Description5 { get; set; }
        public string TSB1Label5 { get; set; }
        public string TSB1Type5 { get; set; }
        public DateTime TSB1Date5 { get; set; }
        public string TSB1MathType5 { get; set; }
        public string TSB1BaseIO5 { get; set; }
        public double TSB11Amount5 { get; set; }
        public string TSB11Unit5 { get; set; }
        public double TSB12Amount5 { get; set; }
        public string TSB12Unit5 { get; set; }
        public double TSB13Amount5 { get; set; }
        public string TSB13Unit5 { get; set; }
        public double TSB14Amount5 { get; set; }
        public string TSB14Unit5 { get; set; }
        public double TSB15Amount5 { get; set; }
        public string TSB15Unit5 { get; set; }
        public string TSB1RelLabel5 { get; set; }
        public double TSB1TAmount5 { get; set; }
        public string TSB1TUnit5 { get; set; }
        public double TSB1TD1Amount5 { get; set; }
        public string TSB1TD1Unit5 { get; set; }
        public double TSB1TD2Amount5 { get; set; }
        public string TSB1TD2Unit5 { get; set; }
        public string TSB1MathResult5 { get; set; }
        public string TSB1MathSubType5 { get; set; }

        public double TSB1TMAmount5 { get; set; }
        public string TSB1TMUnit5 { get; set; }
        public double TSB1TLAmount5 { get; set; }
        public string TSB1TLUnit5 { get; set; }
        public double TSB1TUAmount5 { get; set; }
        public string TSB1TUUnit5 { get; set; }
        public string TSB1MathOperator5 { get; set; }
        public string TSB1MathExpression5 { get; set; }
        public double TSB1N5 { get; set; }

        public const string cTSB1Name5 = "TSB1Name5";
        public const string cTSB1Description5 = "TSB1Description5";
        public const string cTSB1Label5 = "TSB1Label5";
        public const string cTSB1Type5 = "TSB1Type5";
        public const string cTSB1Date5 = "TSB1Date5";
        public const string cTSB1MathType5 = "TSB1MathType5";
        public const string cTSB1BaseIO5 = "TSB1BaseIO5";
        public const string cTSB11Amount5 = "TSB11Amount5";
        public const string cTSB11Unit5 = "TSB11Unit5";
        public const string cTSB12Amount5 = "TSB12Amount5";
        public const string cTSB12Unit5 = "TSB12Unit5";
        public const string cTSB13Amount5 = "TSB13Amount5";
        public const string cTSB13Unit5 = "TSB13Unit5";
        public const string cTSB14Amount5 = "TSB14Amount5";
        public const string cTSB14Unit5 = "TSB14Unit5";
        public const string cTSB15Amount5 = "TSB15Amount5";
        public const string cTSB15Unit5 = "TSB15Unit5";
        public const string cTSB1RelLabel5 = "TSB1RelLabel5";
        public const string cTSB1TAmount5 = "TSB1TAmount5";
        public const string cTSB1TUnit5 = "TSB1TUnit5";
        public const string cTSB1TD1Amount5 = "TSB1TD1Amount5";
        public const string cTSB1TD1Unit5 = "TSB1TD1Unit5";
        public const string cTSB1TD2Amount5 = "TSB1TD2Amount5";
        public const string cTSB1TD2Unit5 = "TSB1TD2Unit5";
        public const string cTSB1MathResult5 = "TSB1MathResult5";
        public const string cTSB1MathSubType5 = "TSB1MathSubType5";

        public const string cTSB1TMAmount5 = "TSB1TMAmount5";
        public const string cTSB1TMUnit5 = "TSB1TMUnit5";
        public const string cTSB1TLAmount5 = "TSB1TLAmount5";
        public const string cTSB1TLUnit5 = "TSB1TLUnit5";
        public const string cTSB1TUAmount5 = "TSB1TUAmount5";
        public const string cTSB1TUUnit5 = "TSB1TUUnit5";
        public const string cTSB1MathOperator5 = "TSB1MathOperator5";
        public const string cTSB1MathExpression5 = "TSB1MathExpression5";
        public const string cTSB1N5 = "TSB1N5";

        //name of indicator 6
        public string TSB1Name6 { get; set; }
        public string TSB1Description6 { get; set; }
        public string TSB1Label6 { get; set; }
        public string TSB1Type6 { get; set; }
        public DateTime TSB1Date6 { get; set; }
        public string TSB1MathType6 { get; set; }
        public string TSB1BaseIO6 { get; set; }
        public double TSB11Amount6 { get; set; }
        public string TSB11Unit6 { get; set; }
        public double TSB12Amount6 { get; set; }
        public string TSB12Unit6 { get; set; }
        public double TSB13Amount6 { get; set; }
        public string TSB13Unit6 { get; set; }
        public double TSB14Amount6 { get; set; }
        public string TSB14Unit6 { get; set; }
        public double TSB15Amount6 { get; set; }
        public string TSB15Unit6 { get; set; }
        public string TSB1RelLabel6 { get; set; }
        public double TSB1TAmount6 { get; set; }
        public string TSB1TUnit6 { get; set; }
        public double TSB1TD1Amount6 { get; set; }
        public string TSB1TD1Unit6 { get; set; }
        public double TSB1TD2Amount6 { get; set; }
        public string TSB1TD2Unit6 { get; set; }
        public string TSB1MathResult6 { get; set; }
        public string TSB1MathSubType6 { get; set; }

        public double TSB1TMAmount6 { get; set; }
        public string TSB1TMUnit6 { get; set; }
        public double TSB1TLAmount6 { get; set; }
        public string TSB1TLUnit6 { get; set; }
        public double TSB1TUAmount6 { get; set; }
        public string TSB1TUUnit6 { get; set; }
        public string TSB1MathOperator6 { get; set; }
        public string TSB1MathExpression6 { get; set; }
        public double TSB1N6 { get; set; }

        public const string cTSB1Name6 = "TSB1Name6";
        public const string cTSB1Description6 = "TSB1Description6";
        public const string cTSB1Label6 = "TSB1Label6";
        public const string cTSB1Type6 = "TSB1Type6";
        public const string cTSB1Date6 = "TSB1Date6";
        public const string cTSB1MathType6 = "TSB1MathType6";
        public const string cTSB1BaseIO6 = "TSB1BaseIO6";
        public const string cTSB11Amount6 = "TSB11Amount6";
        public const string cTSB11Unit6 = "TSB11Unit6";
        public const string cTSB12Amount6 = "TSB12Amount6";
        public const string cTSB12Unit6 = "TSB12Unit6";
        public const string cTSB13Amount6 = "TSB13Amount6";
        public const string cTSB13Unit6 = "TSB13Unit6";
        public const string cTSB14Amount6 = "TSB14Amount6";
        public const string cTSB14Unit6 = "TSB14Unit6";
        public const string cTSB15Amount6 = "TSB15Amount6";
        public const string cTSB15Unit6 = "TSB15Unit6";
        public const string cTSB1RelLabel6 = "TSB1RelLabel6";
        public const string cTSB1TAmount6 = "TSB1TAmount6";
        public const string cTSB1TUnit6 = "TSB1TUnit6";
        public const string cTSB1TD1Amount6 = "TSB1TD1Amount6";
        public const string cTSB1TD1Unit6 = "TSB1TD1Unit6";
        public const string cTSB1TD2Amount6 = "TSB1TD2Amount6";
        public const string cTSB1TD2Unit6 = "TSB1TD2Unit6";
        public const string cTSB1MathResult6 = "TSB1MathResult6";
        public const string cTSB1MathSubType6 = "TSB1MathSubType6";

        public const string cTSB1TMAmount6 = "TSB1TMAmount6";
        public const string cTSB1TMUnit6 = "TSB1TMUnit6";
        public const string cTSB1TLAmount6 = "TSB1TLAmount6";
        public const string cTSB1TLUnit6 = "TSB1TLUnit6";
        public const string cTSB1TUAmount6 = "TSB1TUAmount6";
        public const string cTSB1TUUnit6 = "TSB1TUUnit6";
        public const string cTSB1MathOperator6 = "TSB1MathOperator6";
        public const string cTSB1MathExpression6 = "TSB1MathExpression6";
        public const string cTSB1N6 = "TSB1N6";

        //name of indicator 7
        public string TSB1Name7 { get; set; }
        public string TSB1Description7 { get; set; }
        public string TSB1Label7 { get; set; }
        public string TSB1Type7 { get; set; }
        public DateTime TSB1Date7 { get; set; }
        public string TSB1MathType7 { get; set; }
        public string TSB1BaseIO7 { get; set; }
        public double TSB11Amount7 { get; set; }
        public string TSB11Unit7 { get; set; }
        public double TSB12Amount7 { get; set; }
        public string TSB12Unit7 { get; set; }
        public double TSB13Amount7 { get; set; }
        public string TSB13Unit7 { get; set; }
        public double TSB14Amount7 { get; set; }
        public string TSB14Unit7 { get; set; }
        public double TSB15Amount7 { get; set; }
        public string TSB15Unit7 { get; set; }
        public string TSB1RelLabel7 { get; set; }
        public double TSB1TAmount7 { get; set; }
        public string TSB1TUnit7 { get; set; }
        public double TSB1TD1Amount7 { get; set; }
        public string TSB1TD1Unit7 { get; set; }
        public double TSB1TD2Amount7 { get; set; }
        public string TSB1TD2Unit7 { get; set; }
        public string TSB1MathResult7 { get; set; }
        public string TSB1MathSubType7 { get; set; }

        public double TSB1TMAmount7 { get; set; }
        public string TSB1TMUnit7 { get; set; }
        public double TSB1TLAmount7 { get; set; }
        public string TSB1TLUnit7 { get; set; }
        public double TSB1TUAmount7 { get; set; }
        public string TSB1TUUnit7 { get; set; }
        public string TSB1MathOperator7 { get; set; }
        public string TSB1MathExpression7 { get; set; }
        public double TSB1N7 { get; set; }

        public const string cTSB1Name7 = "TSB1Name7";
        public const string cTSB1Description7 = "TSB1Description7";
        public const string cTSB1Label7 = "TSB1Label7";
        public const string cTSB1Type7 = "TSB1Type7";
        public const string cTSB1Date7 = "TSB1Date7";
        public const string cTSB1MathType7 = "TSB1MathType7";
        public const string cTSB1BaseIO7 = "TSB1BaseIO7";
        public const string cTSB11Amount7 = "TSB11Amount7";
        public const string cTSB11Unit7 = "TSB11Unit7";
        public const string cTSB12Amount7 = "TSB12Amount7";
        public const string cTSB12Unit7 = "TSB12Unit7";
        public const string cTSB13Amount7 = "TSB13Amount7";
        public const string cTSB13Unit7 = "TSB13Unit7";
        public const string cTSB14Amount7 = "TSB14Amount7";
        public const string cTSB14Unit7 = "TSB14Unit7";
        public const string cTSB15Amount7 = "TSB15Amount7";
        public const string cTSB15Unit7 = "TSB15Unit7";
        public const string cTSB1RelLabel7 = "TSB1RelLabel7";
        public const string cTSB1TAmount7 = "TSB1TAmount7";
        public const string cTSB1TUnit7 = "TSB1TUnit7";
        public const string cTSB1TD1Amount7 = "TSB1TD1Amount7";
        public const string cTSB1TD1Unit7 = "TSB1TD1Unit7";
        public const string cTSB1TD2Amount7 = "TSB1TD2Amount7";
        public const string cTSB1TD2Unit7 = "TSB1TD2Unit7";
        public const string cTSB1MathResult7 = "TSB1MathResult7";
        public const string cTSB1MathSubType7 = "TSB1MathSubType7";

        public const string cTSB1TMAmount7 = "TSB1TMAmount7";
        public const string cTSB1TMUnit7 = "TSB1TMUnit7";
        public const string cTSB1TLAmount7 = "TSB1TLAmount7";
        public const string cTSB1TLUnit7 = "TSB1TLUnit7";
        public const string cTSB1TUAmount7 = "TSB1TUAmount7";
        public const string cTSB1TUUnit7 = "TSB1TUUnit7";
        public const string cTSB1MathOperator7 = "TSB1MathOperator7";
        public const string cTSB1MathExpression7 = "TSB1MathExpression7";
        public const string cTSB1N7 = "TSB1N7";

        //name of indicator 8
        public string TSB1Name8 { get; set; }
        public string TSB1Description8 { get; set; }
        public string TSB1Label8 { get; set; }
        public string TSB1Type8 { get; set; }
        public DateTime TSB1Date8 { get; set; }
        public string TSB1MathType8 { get; set; }
        public string TSB1BaseIO8 { get; set; }
        public double TSB11Amount8 { get; set; }
        public string TSB11Unit8 { get; set; }
        public double TSB12Amount8 { get; set; }
        public string TSB12Unit8 { get; set; }
        public double TSB13Amount8 { get; set; }
        public string TSB13Unit8 { get; set; }
        public double TSB14Amount8 { get; set; }
        public string TSB14Unit8 { get; set; }
        public double TSB15Amount8 { get; set; }
        public string TSB15Unit8 { get; set; }
        public string TSB1RelLabel8 { get; set; }
        public double TSB1TAmount8 { get; set; }
        public string TSB1TUnit8 { get; set; }
        public double TSB1TD1Amount8 { get; set; }
        public string TSB1TD1Unit8 { get; set; }
        public double TSB1TD2Amount8 { get; set; }
        public string TSB1TD2Unit8 { get; set; }
        public string TSB1MathResult8 { get; set; }
        public string TSB1MathSubType8 { get; set; }

        public double TSB1TMAmount8 { get; set; }
        public string TSB1TMUnit8 { get; set; }
        public double TSB1TLAmount8 { get; set; }
        public string TSB1TLUnit8 { get; set; }
        public double TSB1TUAmount8 { get; set; }
        public string TSB1TUUnit8 { get; set; }
        public string TSB1MathOperator8 { get; set; }
        public string TSB1MathExpression8 { get; set; }
        public double TSB1N8 { get; set; }

        public const string cTSB1Name8 = "TSB1Name8";
        public const string cTSB1Description8 = "TSB1Description8";
        public const string cTSB1Label8 = "TSB1Label8";
        public const string cTSB1Type8 = "TSB1Type8";
        public const string cTSB1Date8 = "TSB1Date8";
        public const string cTSB1MathType8 = "TSB1MathType8";
        public const string cTSB1BaseIO8 = "TSB1BaseIO8";
        public const string cTSB11Amount8 = "TSB11Amount8";
        public const string cTSB11Unit8 = "TSB11Unit8";
        public const string cTSB12Amount8 = "TSB12Amount8";
        public const string cTSB12Unit8 = "TSB12Unit8";
        public const string cTSB13Amount8 = "TSB13Amount8";
        public const string cTSB13Unit8 = "TSB13Unit8";
        public const string cTSB14Amount8 = "TSB14Amount8";
        public const string cTSB14Unit8 = "TSB14Unit8";
        public const string cTSB15Amount8 = "TSB15Amount8";
        public const string cTSB15Unit8 = "TSB15Unit8";
        public const string cTSB1RelLabel8 = "TSB1RelLabel8";
        public const string cTSB1TAmount8 = "TSB1TAmount8";
        public const string cTSB1TUnit8 = "TSB1TUnit8";
        public const string cTSB1TD1Amount8 = "TSB1TD1Amount8";
        public const string cTSB1TD1Unit8 = "TSB1TD1Unit8";
        public const string cTSB1TD2Amount8 = "TSB1TD2Amount8";
        public const string cTSB1TD2Unit8 = "TSB1TD2Unit8";
        public const string cTSB1MathResult8 = "TSB1MathResult8";
        public const string cTSB1MathSubType8 = "TSB1MathSubType8";

        public const string cTSB1TMAmount8 = "TSB1TMAmount8";
        public const string cTSB1TMUnit8 = "TSB1TMUnit8";
        public const string cTSB1TLAmount8 = "TSB1TLAmount8";
        public const string cTSB1TLUnit8 = "TSB1TLUnit8";
        public const string cTSB1TUAmount8 = "TSB1TUAmount8";
        public const string cTSB1TUUnit8 = "TSB1TUUnit8";
        public const string cTSB1MathOperator8 = "TSB1MathOperator8";
        public const string cTSB1MathExpression8 = "TSB1MathExpression8";
        public const string cTSB1N8 = "TSB1N8";

        //name of indicator 9
        public string TSB1Name9 { get; set; }
        public string TSB1Description9 { get; set; }
        public string TSB1Label9 { get; set; }
        public string TSB1Type9 { get; set; }
        public DateTime TSB1Date9 { get; set; }
        public string TSB1MathType9 { get; set; }
        public string TSB1BaseIO9 { get; set; }
        public double TSB11Amount9 { get; set; }
        public string TSB11Unit9 { get; set; }
        public double TSB12Amount9 { get; set; }
        public string TSB12Unit9 { get; set; }
        public double TSB13Amount9 { get; set; }
        public string TSB13Unit9 { get; set; }
        public double TSB14Amount9 { get; set; }
        public string TSB14Unit9 { get; set; }
        public double TSB15Amount9 { get; set; }
        public string TSB15Unit9 { get; set; }
        public string TSB1RelLabel9 { get; set; }
        public double TSB1TAmount9 { get; set; }
        public string TSB1TUnit9 { get; set; }
        public double TSB1TD1Amount9 { get; set; }
        public string TSB1TD1Unit9 { get; set; }
        public double TSB1TD2Amount9 { get; set; }
        public string TSB1TD2Unit9 { get; set; }
        public string TSB1MathResult9 { get; set; }
        public string TSB1MathSubType9 { get; set; }

        public double TSB1TMAmount9 { get; set; }
        public string TSB1TMUnit9 { get; set; }
        public double TSB1TLAmount9 { get; set; }
        public string TSB1TLUnit9 { get; set; }
        public double TSB1TUAmount9 { get; set; }
        public string TSB1TUUnit9 { get; set; }
        public string TSB1MathOperator9 { get; set; }
        public string TSB1MathExpression9 { get; set; }
        public double TSB1N9 { get; set; }

        public const string cTSB1Name9 = "TSB1Name9";
        public const string cTSB1Description9 = "TSB1Description9";
        public const string cTSB1Label9 = "TSB1Label9";
        public const string cTSB1Type9 = "TSB1Type9";
        public const string cTSB1Date9 = "TSB1Date9";
        public const string cTSB1MathType9 = "TSB1MathType9";
        public const string cTSB1BaseIO9 = "TSB1BaseIO9";
        public const string cTSB11Amount9 = "TSB11Amount9";
        public const string cTSB11Unit9 = "TSB11Unit9";
        public const string cTSB12Amount9 = "TSB12Amount9";
        public const string cTSB12Unit9 = "TSB12Unit9";
        public const string cTSB13Amount9 = "TSB13Amount9";
        public const string cTSB13Unit9 = "TSB13Unit9";
        public const string cTSB14Amount9 = "TSB14Amount9";
        public const string cTSB14Unit9 = "TSB14Unit9";
        public const string cTSB15Amount9 = "TSB15Amount9";
        public const string cTSB15Unit9 = "TSB15Unit9";
        public const string cTSB1RelLabel9 = "TSB1RelLabel9";
        public const string cTSB1TAmount9 = "TSB1TAmount9";
        public const string cTSB1TUnit9 = "TSB1TUnit9";
        public const string cTSB1TD1Amount9 = "TSB1TD1Amount9";
        public const string cTSB1TD1Unit9 = "TSB1TD1Unit9";
        public const string cTSB1TD2Amount9 = "TSB1TD2Amount9";
        public const string cTSB1TD2Unit9 = "TSB1TD2Unit9";
        public const string cTSB1MathResult9 = "TSB1MathResult9";
        public const string cTSB1MathSubType9 = "TSB1MathSubType9";

        public const string cTSB1TMAmount9 = "TSB1TMAmount9";
        public const string cTSB1TMUnit9 = "TSB1TMUnit9";
        public const string cTSB1TLAmount9 = "TSB1TLAmount9";
        public const string cTSB1TLUnit9 = "TSB1TLUnit9";
        public const string cTSB1TUAmount9 = "TSB1TUAmount9";
        public const string cTSB1TUUnit9 = "TSB1TUUnit9";
        public const string cTSB1MathOperator9 = "TSB1MathOperator9";
        public const string cTSB1MathExpression9 = "TSB1MathExpression9";
        public const string cTSB1N9 = "TSB1N9";

        //name of indicator 10
        public string TSB1Name10 { get; set; }
        public string TSB1Description10 { get; set; }
        public string TSB1Label10 { get; set; }
        public string TSB1Type10 { get; set; }
        public DateTime TSB1Date10 { get; set; }
        public string TSB1MathType10 { get; set; }
        public string TSB1BaseIO10 { get; set; }
        public double TSB11Amount10 { get; set; }
        public string TSB11Unit10 { get; set; }
        public double TSB12Amount10 { get; set; }
        public string TSB12Unit10 { get; set; }
        public double TSB13Amount10 { get; set; }
        public string TSB13Unit10 { get; set; }
        public double TSB14Amount10 { get; set; }
        public string TSB14Unit10 { get; set; }
        public double TSB15Amount10 { get; set; }
        public string TSB15Unit10 { get; set; }
        public string TSB1RelLabel10 { get; set; }
        public double TSB1TAmount10 { get; set; }
        public string TSB1TUnit10 { get; set; }
        public double TSB1TD1Amount10 { get; set; }
        public string TSB1TD1Unit10 { get; set; }
        public double TSB1TD2Amount10 { get; set; }
        public string TSB1TD2Unit10 { get; set; }
        public string TSB1MathResult10 { get; set; }
        public string TSB1MathSubType10 { get; set; }

        public double TSB1TMAmount10 { get; set; }
        public string TSB1TMUnit10 { get; set; }
        public double TSB1TLAmount10 { get; set; }
        public string TSB1TLUnit10 { get; set; }
        public double TSB1TUAmount10 { get; set; }
        public string TSB1TUUnit10 { get; set; }
        public string TSB1MathOperator10 { get; set; }
        public string TSB1MathExpression10 { get; set; }
        public double TSB1N10 { get; set; }

        public const string cTSB1Name10 = "TSB1Name10";
        public const string cTSB1Description10 = "TSB1Description10";
        public const string cTSB1Label10 = "TSB1Label10";
        public const string cTSB1Type10 = "TSB1Type10";
        public const string cTSB1Date10 = "TSB1Date10";
        public const string cTSB1MathType10 = "TSB1MathType10";
        public const string cTSB1BaseIO10 = "TSB1BaseIO10";
        public const string cTSB11Amount10 = "TSB11Amount10";
        public const string cTSB11Unit10 = "TSB11Unit10";
        public const string cTSB12Amount10 = "TSB12Amount10";
        public const string cTSB12Unit10 = "TSB12Unit10";
        public const string cTSB13Amount10 = "TSB13Amount10";
        public const string cTSB13Unit10 = "TSB13Unit10";
        public const string cTSB14Amount10 = "TSB14Amount10";
        public const string cTSB14Unit10 = "TSB14Unit10";
        public const string cTSB15Amount10 = "TSB15Amount10";
        public const string cTSB15Unit10 = "TSB15Unit10";
        public const string cTSB1RelLabel10 = "TSB1RelLabel10";
        public const string cTSB1TAmount10 = "TSB1TAmount10";
        public const string cTSB1TUnit10 = "TSB1TUnit10";
        public const string cTSB1TD1Amount10 = "TSB1TD1Amount10";
        public const string cTSB1TD1Unit10 = "TSB1TD1Unit10";
        public const string cTSB1TD2Amount10 = "TSB1TD2Amount10";
        public const string cTSB1TD2Unit10 = "TSB1TD2Unit10";
        public const string cTSB1MathResult10 = "TSB1MathResult10";
        public const string cTSB1MathSubType10 = "TSB1MathSubType10";

        public const string cTSB1TMAmount10 = "TSB1TMAmount10";
        public const string cTSB1TMUnit10 = "TSB1TMUnit10";
        public const string cTSB1TLAmount10 = "TSB1TLAmount10";
        public const string cTSB1TLUnit10 = "TSB1TLUnit10";
        public const string cTSB1TUAmount10 = "TSB1TUAmount10";
        public const string cTSB1TUUnit10 = "TSB1TUUnit10";
        public const string cTSB1MathOperator10 = "TSB1MathOperator10";
        public const string cTSB1MathExpression10 = "TSB1MathExpression10";
        public const string cTSB1N10 = "TSB1N10";

        //name of indicator 11
        public string TSB1Name11 { get; set; }
        public string TSB1Description11 { get; set; }
        public string TSB1Label11 { get; set; }
        public string TSB1Type11 { get; set; }
        public DateTime TSB1Date11 { get; set; }
        public string TSB1MathType11 { get; set; }
        public string TSB1BaseIO11 { get; set; }
        public double TSB11Amount11 { get; set; }
        public string TSB11Unit11 { get; set; }
        public double TSB12Amount11 { get; set; }
        public string TSB12Unit11 { get; set; }
        public double TSB13Amount11 { get; set; }
        public string TSB13Unit11 { get; set; }
        public double TSB14Amount11 { get; set; }
        public string TSB14Unit11 { get; set; }
        public double TSB15Amount11 { get; set; }
        public string TSB15Unit11 { get; set; }
        public string TSB1RelLabel11 { get; set; }
        public double TSB1TAmount11 { get; set; }
        public string TSB1TUnit11 { get; set; }
        public double TSB1TD1Amount11 { get; set; }
        public string TSB1TD1Unit11 { get; set; }
        public double TSB1TD2Amount11 { get; set; }
        public string TSB1TD2Unit11 { get; set; }
        public string TSB1MathResult11 { get; set; }
        public string TSB1MathSubType11 { get; set; }

        public double TSB1TMAmount11 { get; set; }
        public string TSB1TMUnit11 { get; set; }
        public double TSB1TLAmount11 { get; set; }
        public string TSB1TLUnit11 { get; set; }
        public double TSB1TUAmount11 { get; set; }
        public string TSB1TUUnit11 { get; set; }
        public string TSB1MathOperator11 { get; set; }
        public string TSB1MathExpression11 { get; set; }
        public double TSB1N11 { get; set; }

        public const string cTSB1Name11 = "TSB1Name11";
        public const string cTSB1Description11 = "TSB1Description11";
        public const string cTSB1Label11 = "TSB1Label11";
        public const string cTSB1Type11 = "TSB1Type11";
        public const string cTSB1Date11 = "TSB1Date11";
        public const string cTSB1MathType11 = "TSB1MathType11";
        public const string cTSB1BaseIO11 = "TSB1BaseIO11";
        public const string cTSB11Amount11 = "TSB11Amount11";
        public const string cTSB11Unit11 = "TSB11Unit11";
        public const string cTSB12Amount11 = "TSB12Amount11";
        public const string cTSB12Unit11 = "TSB12Unit11";
        public const string cTSB13Amount11 = "TSB13Amount11";
        public const string cTSB13Unit11 = "TSB13Unit11";
        public const string cTSB14Amount11 = "TSB14Amount11";
        public const string cTSB14Unit11 = "TSB14Unit11";
        public const string cTSB15Amount11 = "TSB15Amount11";
        public const string cTSB15Unit11 = "TSB15Unit11";
        public const string cTSB1RelLabel11 = "TSB1RelLabel11";
        public const string cTSB1TAmount11 = "TSB1TAmount11";
        public const string cTSB1TUnit11 = "TSB1TUnit11";
        public const string cTSB1TD1Amount11 = "TSB1TD1Amount11";
        public const string cTSB1TD1Unit11 = "TSB1TD1Unit11";
        public const string cTSB1TD2Amount11 = "TSB1TD2Amount11";
        public const string cTSB1TD2Unit11 = "TSB1TD2Unit11";
        public const string cTSB1MathResult11 = "TSB1MathResource11";
        public const string cTSB1MathSubType11 = "TSB1MathSubType11";

        public const string cTSB1TMAmount11 = "TSB1TMAmount11";
        public const string cTSB1TMUnit11 = "TSB1TMUnit11";
        public const string cTSB1TLAmount11 = "TSB1TLAmount11";
        public const string cTSB1TLUnit11 = "TSB1TLUnit11";
        public const string cTSB1TUAmount11 = "TSB1TUAmount11";
        public const string cTSB1TUUnit11 = "TSB1TUUnit11";
        public const string cTSB1MathOperator11 = "TSB1MathOperator11";
        public const string cTSB1MathExpression11 = "TSB1MathExpression11";
        public const string cTSB1N11 = "TSB1N11";

        //name of indicator 12
        public string TSB1Name12 { get; set; }
        public string TSB1Description12 { get; set; }
        public string TSB1Label12 { get; set; }
        public string TSB1Type12 { get; set; }
        public DateTime TSB1Date12 { get; set; }
        public string TSB1MathType12 { get; set; }
        public string TSB1BaseIO12 { get; set; }
        public double TSB11Amount12 { get; set; }
        public string TSB11Unit12 { get; set; }
        public double TSB12Amount12 { get; set; }
        public string TSB12Unit12 { get; set; }
        public double TSB13Amount12 { get; set; }
        public string TSB13Unit12 { get; set; }
        public double TSB14Amount12 { get; set; }
        public string TSB14Unit12 { get; set; }
        public double TSB15Amount12 { get; set; }
        public string TSB15Unit12 { get; set; }
        public string TSB1RelLabel12 { get; set; }
        public double TSB1TAmount12 { get; set; }
        public string TSB1TUnit12 { get; set; }
        public double TSB1TD1Amount12 { get; set; }
        public string TSB1TD1Unit12 { get; set; }
        public double TSB1TD2Amount12 { get; set; }
        public string TSB1TD2Unit12 { get; set; }
        public string TSB1MathResult12 { get; set; }
        public string TSB1MathSubType12 { get; set; }

        public double TSB1TMAmount12 { get; set; }
        public string TSB1TMUnit12 { get; set; }
        public double TSB1TLAmount12 { get; set; }
        public string TSB1TLUnit12 { get; set; }
        public double TSB1TUAmount12 { get; set; }
        public string TSB1TUUnit12 { get; set; }
        public string TSB1MathOperator12 { get; set; }
        public string TSB1MathExpression12 { get; set; }
        public double TSB1N12 { get; set; }

        public const string cTSB1Name12 = "TSB1Name12";
        public const string cTSB1Description12 = "TSB1Description12";
        public const string cTSB1Label12 = "TSB1Label12";
        public const string cTSB1Type12 = "TSB1Type12";
        public const string cTSB1Date12 = "TSB1Date12";
        public const string cTSB1MathType12 = "TSB1MathType12";
        public const string cTSB1BaseIO12 = "TSB1BaseIO12";
        public const string cTSB11Amount12 = "TSB11Amount12";
        public const string cTSB11Unit12 = "TSB11Unit12";
        public const string cTSB12Amount12 = "TSB12Amount12";
        public const string cTSB12Unit12 = "TSB12Unit12";
        public const string cTSB13Amount12 = "TSB13Amount12";
        public const string cTSB13Unit12 = "TSB13Unit12";
        public const string cTSB14Amount12 = "TSB14Amount12";
        public const string cTSB14Unit12 = "TSB14Unit12";
        public const string cTSB15Amount12 = "TSB15Amount12";
        public const string cTSB15Unit12 = "TSB15Unit12";
        public const string cTSB1RelLabel12 = "TSB1RelLabel12";
        public const string cTSB1TAmount12 = "TSB1TAmount12";
        public const string cTSB1TUnit12 = "TSB1TUnit12";
        public const string cTSB1TD1Amount12 = "TSB1TD1Amount12";
        public const string cTSB1TD1Unit12 = "TSB1TD1Unit12";
        public const string cTSB1TD2Amount12 = "TSB1TD2Amount12";
        public const string cTSB1TD2Unit12 = "TSB1TD2Unit12";
        public const string cTSB1MathResult12 = "TSB1MathResult12";
        public const string cTSB1MathSubType12 = "TSB1MathSubType12";

        public const string cTSB1TMAmount12 = "TSB1TMAmount12";
        public const string cTSB1TMUnit12 = "TSB1TMUnit12";
        public const string cTSB1TLAmount12 = "TSB1TLAmount12";
        public const string cTSB1TLUnit12 = "TSB1TLUnit12";
        public const string cTSB1TUAmount12 = "TSB1TUAmount12";
        public const string cTSB1TUUnit12 = "TSB1TUUnit12";
        public const string cTSB1MathOperator12 = "TSB1MathOperator12";
        public const string cTSB1MathExpression12 = "TSB1MathExpression12";
        public const string cTSB1N12 = "TSB1N12";

        //name of indicator 13
        public string TSB1Name13 { get; set; }
        public string TSB1Description13 { get; set; }
        public string TSB1Label13 { get; set; }
        public string TSB1Type13 { get; set; }
        public DateTime TSB1Date13 { get; set; }
        public string TSB1MathType13 { get; set; }
        public string TSB1BaseIO13 { get; set; }
        public double TSB11Amount13 { get; set; }
        public string TSB11Unit13 { get; set; }
        public double TSB12Amount13 { get; set; }
        public string TSB12Unit13 { get; set; }
        public double TSB13Amount13 { get; set; }
        public string TSB13Unit13 { get; set; }
        public double TSB14Amount13 { get; set; }
        public string TSB14Unit13 { get; set; }
        public double TSB15Amount13 { get; set; }
        public string TSB15Unit13 { get; set; }
        public string TSB1RelLabel13 { get; set; }
        public double TSB1TAmount13 { get; set; }
        public string TSB1TUnit13 { get; set; }
        public double TSB1TD1Amount13 { get; set; }
        public string TSB1TD1Unit13 { get; set; }
        public double TSB1TD2Amount13 { get; set; }
        public string TSB1TD2Unit13 { get; set; }
        public string TSB1MathResult13 { get; set; }
        public string TSB1MathSubType13 { get; set; }

        public double TSB1TMAmount13 { get; set; }
        public string TSB1TMUnit13 { get; set; }
        public double TSB1TLAmount13 { get; set; }
        public string TSB1TLUnit13 { get; set; }
        public double TSB1TUAmount13 { get; set; }
        public string TSB1TUUnit13 { get; set; }
        public string TSB1MathOperator13 { get; set; }
        public string TSB1MathExpression13 { get; set; }
        public double TSB1N13 { get; set; }

        public const string cTSB1Name13 = "TSB1Name13";
        public const string cTSB1Description13 = "TSB1Description13";
        public const string cTSB1Label13 = "TSB1Label13";
        public const string cTSB1Type13 = "TSB1Type13";
        public const string cTSB1Date13 = "TSB1Date13";
        public const string cTSB1MathType13 = "TSB1MathType13";
        public const string cTSB1BaseIO13 = "TSB1BaseIO13";
        public const string cTSB11Amount13 = "TSB11Amount13";
        public const string cTSB11Unit13 = "TSB11Unit13";
        public const string cTSB12Amount13 = "TSB12Amount13";
        public const string cTSB12Unit13 = "TSB12Unit13";
        public const string cTSB13Amount13 = "TSB13Amount13";
        public const string cTSB13Unit13 = "TSB13Unit13";
        public const string cTSB14Amount13 = "TSB14Amount13";
        public const string cTSB14Unit13 = "TSB14Unit13";
        public const string cTSB15Amount13 = "TSB15Amount13";
        public const string cTSB15Unit13 = "TSB15Unit13";
        public const string cTSB1RelLabel13 = "TSB1RelLabel13";
        public const string cTSB1TAmount13 = "TSB1TAmount13";
        public const string cTSB1TUnit13 = "TSB1TUnit13";
        public const string cTSB1TD1Amount13 = "TSB1TD1Amount13";
        public const string cTSB1TD1Unit13 = "TSB1TD1Unit13";
        public const string cTSB1TD2Amount13 = "TSB1TD2Amount13";
        public const string cTSB1TD2Unit13 = "TSB1TD2Unit13";
        public const string cTSB1MathResult13 = "TSB1MathResult13";
        public const string cTSB1MathSubType13 = "TSB1MathSubType13";

        public const string cTSB1TMAmount13 = "TSB1TMAmount13";
        public const string cTSB1TMUnit13 = "TSB1TMUnit13";
        public const string cTSB1TLAmount13 = "TSB1TLAmount13";
        public const string cTSB1TLUnit13 = "TSB1TLUnit13";
        public const string cTSB1TUAmount13 = "TSB1TUAmount13";
        public const string cTSB1TUUnit13 = "TSB1TUUnit13";
        public const string cTSB1MathOperator13 = "TSB1MathOperator13";
        public const string cTSB1MathExpression13 = "TSB1MathExpression13";
        public const string cTSB1N13 = "TSB1N13";

        //name of indicator 14
        public string TSB1Name14 { get; set; }
        public string TSB1Description14 { get; set; }
        public string TSB1Label14 { get; set; }
        public string TSB1Type14 { get; set; }
        public DateTime TSB1Date14 { get; set; }
        public string TSB1MathType14 { get; set; }
        public string TSB1BaseIO14 { get; set; }
        public double TSB11Amount14 { get; set; }
        public string TSB11Unit14 { get; set; }
        public double TSB12Amount14 { get; set; }
        public string TSB12Unit14 { get; set; }
        public double TSB13Amount14 { get; set; }
        public string TSB13Unit14 { get; set; }
        public double TSB14Amount14 { get; set; }
        public string TSB14Unit14 { get; set; }
        public double TSB15Amount14 { get; set; }
        public string TSB15Unit14 { get; set; }
        public string TSB1RelLabel14 { get; set; }
        public double TSB1TAmount14 { get; set; }
        public string TSB1TUnit14 { get; set; }
        public double TSB1TD1Amount14 { get; set; }
        public string TSB1TD1Unit14 { get; set; }
        public double TSB1TD2Amount14 { get; set; }
        public string TSB1TD2Unit14 { get; set; }
        public string TSB1MathResult14 { get; set; }
        public string TSB1MathSubType14 { get; set; }

        public double TSB1TMAmount14 { get; set; }
        public string TSB1TMUnit14 { get; set; }
        public double TSB1TLAmount14 { get; set; }
        public string TSB1TLUnit14 { get; set; }
        public double TSB1TUAmount14 { get; set; }
        public string TSB1TUUnit14 { get; set; }
        public string TSB1MathOperator14 { get; set; }
        public string TSB1MathExpression14 { get; set; }
        public double TSB1N14 { get; set; }

        public const string cTSB1Name14 = "TSB1Name14";
        public const string cTSB1Description14 = "TSB1Description14";
        public const string cTSB1Label14 = "TSB1Label14";
        public const string cTSB1Type14 = "TSB1Type14";
        public const string cTSB1Date14 = "TSB1Date14";
        public const string cTSB1MathType14 = "TSB1MathType14";
        public const string cTSB1BaseIO14 = "TSB1BaseIO14";
        public const string cTSB11Amount14 = "TSB11Amount14";
        public const string cTSB11Unit14 = "TSB11Unit14";
        public const string cTSB12Amount14 = "TSB12Amount14";
        public const string cTSB12Unit14 = "TSB12Unit14";
        public const string cTSB13Amount14 = "TSB13Amount14";
        public const string cTSB13Unit14 = "TSB13Unit14";
        public const string cTSB14Amount14 = "TSB14Amount14";
        public const string cTSB14Unit14 = "TSB14Unit14";
        public const string cTSB15Amount14 = "TSB15Amount14";
        public const string cTSB15Unit14 = "TSB15Unit14";
        public const string cTSB1RelLabel14 = "TSB1RelLabel14";
        public const string cTSB1TAmount14 = "TSB1TAmount14";
        public const string cTSB1TUnit14 = "TSB1TUnit14";
        public const string cTSB1TD1Amount14 = "TSB1TD1Amount14";
        public const string cTSB1TD1Unit14 = "TSB1TD1Unit14";
        public const string cTSB1TD2Amount14 = "TSB1TD2Amount14";
        public const string cTSB1TD2Unit14 = "TSB1TD2Unit14";
        public const string cTSB1MathResult14 = "TSB1MathResult14";
        public const string cTSB1MathSubType14 = "TSB1MathSubType14";

        public const string cTSB1TMAmount14 = "TSB1TMAmount14";
        public const string cTSB1TMUnit14 = "TSB1TMUnit14";
        public const string cTSB1TLAmount14 = "TSB1TLAmount14";
        public const string cTSB1TLUnit14 = "TSB1TLUnit14";
        public const string cTSB1TUAmount14 = "TSB1TUAmount14";
        public const string cTSB1TUUnit14 = "TSB1TUUnit14";
        public const string cTSB1MathOperator14 = "TSB1MathOperator14";
        public const string cTSB1MathExpression14 = "TSB1MathExpression14";
        public const string cTSB1N14 = "TSB1N14";

        //name of indicator 15
        public string TSB1Name15 { get; set; }
        public string TSB1Description15 { get; set; }
        public string TSB1Label15 { get; set; }
        public string TSB1Type15 { get; set; }
        public DateTime TSB1Date15 { get; set; }
        public string TSB1MathType15 { get; set; }
        public string TSB1BaseIO15 { get; set; }
        public double TSB11Amount15 { get; set; }
        public string TSB11Unit15 { get; set; }
        public double TSB12Amount15 { get; set; }
        public string TSB12Unit15 { get; set; }
        public double TSB13Amount15 { get; set; }
        public string TSB13Unit15 { get; set; }
        public double TSB14Amount15 { get; set; }
        public string TSB14Unit15 { get; set; }
        public double TSB15Amount15 { get; set; }
        public string TSB15Unit15 { get; set; }
        public string TSB1RelLabel15 { get; set; }
        public double TSB1TAmount15 { get; set; }
        public string TSB1TUnit15 { get; set; }
        public double TSB1TD1Amount15 { get; set; }
        public string TSB1TD1Unit15 { get; set; }
        public double TSB1TD2Amount15 { get; set; }
        public string TSB1TD2Unit15 { get; set; }
        public string TSB1MathResult15 { get; set; }
        public string TSB1MathSubType15 { get; set; }

        public double TSB1TMAmount15 { get; set; }
        public string TSB1TMUnit15 { get; set; }
        public double TSB1TLAmount15 { get; set; }
        public string TSB1TLUnit15 { get; set; }
        public double TSB1TUAmount15 { get; set; }
        public string TSB1TUUnit15 { get; set; }
        public string TSB1MathOperator15 { get; set; }
        public string TSB1MathExpression15 { get; set; }
        public double TSB1N15 { get; set; }

        public const string cTSB1Name15 = "TSB1Name15";
        public const string cTSB1Description15 = "TSB1Description15";
        public const string cTSB1Label15 = "TSB1Label15";
        public const string cTSB1Type15 = "TSB1Type15";
        public const string cTSB1Date15 = "TSB1Date15";
        public const string cTSB1MathType15 = "TSB1MathType15";
        public const string cTSB1BaseIO15 = "TSB1BaseIO15";
        public const string cTSB11Amount15 = "TSB11Amount15";
        public const string cTSB11Unit15 = "TSB11Unit15";
        public const string cTSB12Amount15 = "TSB12Amount15";
        public const string cTSB12Unit15 = "TSB12Unit15";
        public const string cTSB13Amount15 = "TSB13Amount15";
        public const string cTSB13Unit15 = "TSB13Unit15";
        public const string cTSB14Amount15 = "TSB14Amount15";
        public const string cTSB14Unit15 = "TSB14Unit15";
        public const string cTSB15Amount15 = "TSB15Amount15";
        public const string cTSB15Unit15 = "TSB15Unit15";
        public const string cTSB1RelLabel15 = "TSB1RelLabel15";
        public const string cTSB1TAmount15 = "TSB1TAmount15";
        public const string cTSB1TUnit15 = "TSB1TUnit15";
        public const string cTSB1TD1Amount15 = "TSB1TD1Amount15";
        public const string cTSB1TD1Unit15 = "TSB1TD1Unit15";
        public const string cTSB1TD2Amount15 = "TSB1TD2Amount15";
        public const string cTSB1TD2Unit15 = "TSB1TD2Unit15";
        public const string cTSB1MathResult15 = "TSB1MathResult15";
        public const string cTSB1MathSubType15 = "TSB1MathSubType15";

        public const string cTSB1TMAmount15 = "TSB1TMAmount15";
        public const string cTSB1TMUnit15 = "TSB1TMUnit15";
        public const string cTSB1TLAmount15 = "TSB1TLAmount15";
        public const string cTSB1TLUnit15 = "TSB1TLUnit15";
        public const string cTSB1TUAmount15 = "TSB1TUAmount15";
        public const string cTSB1TUUnit15 = "TSB1TUUnit15";
        public const string cTSB1MathOperator15 = "TSB1MathOperator15";
        public const string cTSB1MathExpression15 = "TSB1MathExpression15";
        public const string cTSB1N15 = "TSB1N15";

        //name of indicator 16
        public string TSB1Name16 { get; set; }
        public string TSB1Description16 { get; set; }
        public string TSB1Label16 { get; set; }
        public string TSB1Type16 { get; set; }
        public DateTime TSB1Date16 { get; set; }
        public string TSB1MathType16 { get; set; }
        public string TSB1BaseIO16 { get; set; }
        public double TSB11Amount16 { get; set; }
        public string TSB11Unit16 { get; set; }
        public double TSB12Amount16 { get; set; }
        public string TSB12Unit16 { get; set; }
        public double TSB13Amount16 { get; set; }
        public string TSB13Unit16 { get; set; }
        public double TSB14Amount16 { get; set; }
        public string TSB14Unit16 { get; set; }
        public double TSB15Amount16 { get; set; }
        public string TSB15Unit16 { get; set; }
        public string TSB1RelLabel16 { get; set; }
        public double TSB1TAmount16 { get; set; }
        public string TSB1TUnit16 { get; set; }
        public double TSB1TD1Amount16 { get; set; }
        public string TSB1TD1Unit16 { get; set; }
        public double TSB1TD2Amount16 { get; set; }
        public string TSB1TD2Unit16 { get; set; }
        public string TSB1MathResult16 { get; set; }
        public string TSB1MathSubType16 { get; set; }

        public double TSB1TMAmount16 { get; set; }
        public string TSB1TMUnit16 { get; set; }
        public double TSB1TLAmount16 { get; set; }
        public string TSB1TLUnit16 { get; set; }
        public double TSB1TUAmount16 { get; set; }
        public string TSB1TUUnit16 { get; set; }
        public string TSB1MathOperator16 { get; set; }
        public string TSB1MathExpression16 { get; set; }
        public double TSB1N16 { get; set; }

        public const string cTSB1Name16 = "TSB1Name16";
        public const string cTSB1Description16 = "TSB1Description16";
        public const string cTSB1Label16 = "TSB1Label16";
        public const string cTSB1Type16 = "TSB1Type16";
        public const string cTSB1Date16 = "TSB1Date16";
        public const string cTSB1MathType16 = "TSB1MathType16";
        public const string cTSB1BaseIO16 = "TSB1BaseIO16";
        public const string cTSB11Amount16 = "TSB11Amount16";
        public const string cTSB11Unit16 = "TSB11Unit16";
        public const string cTSB12Amount16 = "TSB12Amount16";
        public const string cTSB12Unit16 = "TSB12Unit16";
        public const string cTSB13Amount16 = "TSB13Amount16";
        public const string cTSB13Unit16 = "TSB13Unit16";
        public const string cTSB14Amount16 = "TSB14Amount16";
        public const string cTSB14Unit16 = "TSB14Unit16";
        public const string cTSB15Amount16 = "TSB15Amount16";
        public const string cTSB15Unit16 = "TSB15Unit16";
        public const string cTSB1RelLabel16 = "TSB1RelLabel16";
        public const string cTSB1TAmount16 = "TSB1TAmount16";
        public const string cTSB1TUnit16 = "TSB1TUnit16";
        public const string cTSB1TD1Amount16 = "TSB1TD1Amount16";
        public const string cTSB1TD1Unit16 = "TSB1TD1Unit16";
        public const string cTSB1TD2Amount16 = "TSB1TD2Amount16";
        public const string cTSB1TD2Unit16 = "TSB1TD2Unit16";
        public const string cTSB1MathResult16 = "TSB1MathResult16";
        public const string cTSB1MathSubType16 = "TSB1MathSubType16";

        public const string cTSB1TMAmount16 = "TSB1TMAmount16";
        public const string cTSB1TMUnit16 = "TSB1TMUnit16";
        public const string cTSB1TLAmount16 = "TSB1TLAmount16";
        public const string cTSB1TLUnit16 = "TSB1TLUnit16";
        public const string cTSB1TUAmount16 = "TSB1TUAmount16";
        public const string cTSB1TUUnit16 = "TSB1TUUnit16";
        public const string cTSB1MathOperator16 = "TSB1MathOperator16";
        public const string cTSB1MathExpression16 = "TSB1MathExpression16";
        public const string cTSB1N16 = "TSB1N16";

        //name of indicator 17
        public string TSB1Name17 { get; set; }
        public string TSB1Description17 { get; set; }
        public string TSB1Label17 { get; set; }
        public string TSB1Type17 { get; set; }
        public DateTime TSB1Date17 { get; set; }
        public string TSB1MathType17 { get; set; }
        public string TSB1BaseIO17 { get; set; }
        public double TSB11Amount17 { get; set; }
        public string TSB11Unit17 { get; set; }
        public double TSB12Amount17 { get; set; }
        public string TSB12Unit17 { get; set; }
        public double TSB13Amount17 { get; set; }
        public string TSB13Unit17 { get; set; }
        public double TSB14Amount17 { get; set; }
        public string TSB14Unit17 { get; set; }
        public double TSB15Amount17 { get; set; }
        public string TSB15Unit17 { get; set; }
        public string TSB1RelLabel17 { get; set; }
        public double TSB1TAmount17 { get; set; }
        public string TSB1TUnit17 { get; set; }
        public double TSB1TD1Amount17 { get; set; }
        public string TSB1TD1Unit17 { get; set; }
        public double TSB1TD2Amount17 { get; set; }
        public string TSB1TD2Unit17 { get; set; }
        public string TSB1MathResult17 { get; set; }
        public string TSB1MathSubType17 { get; set; }

        public double TSB1TMAmount17 { get; set; }
        public string TSB1TMUnit17 { get; set; }
        public double TSB1TLAmount17 { get; set; }
        public string TSB1TLUnit17 { get; set; }
        public double TSB1TUAmount17 { get; set; }
        public string TSB1TUUnit17 { get; set; }
        public string TSB1MathOperator17 { get; set; }
        public string TSB1MathExpression17 { get; set; }
        public double TSB1N17 { get; set; }

        public const string cTSB1Name17 = "TSB1Name17";
        public const string cTSB1Description17 = "TSB1Description17";
        public const string cTSB1Label17 = "TSB1Label17";
        public const string cTSB1Type17 = "TSB1Type17";
        public const string cTSB1Date17 = "TSB1Date17";
        public const string cTSB1MathType17 = "TSB1MathType17";
        public const string cTSB1BaseIO17 = "TSB1BaseIO17";
        public const string cTSB11Amount17 = "TSB11Amount17";
        public const string cTSB11Unit17 = "TSB11Unit17";
        public const string cTSB12Amount17 = "TSB12Amount17";
        public const string cTSB12Unit17 = "TSB12Unit17";
        public const string cTSB13Amount17 = "TSB13Amount17";
        public const string cTSB13Unit17 = "TSB13Unit17";
        public const string cTSB14Amount17 = "TSB14Amount17";
        public const string cTSB14Unit17 = "TSB14Unit17";
        public const string cTSB15Amount17 = "TSB15Amount17";
        public const string cTSB15Unit17 = "TSB15Unit17";
        public const string cTSB1RelLabel17 = "TSB1RelLabel17";
        public const string cTSB1TAmount17 = "TSB1TAmount17";
        public const string cTSB1TUnit17 = "TSB1TUnit17";
        public const string cTSB1TD1Amount17 = "TSB1TD1Amount17";
        public const string cTSB1TD1Unit17 = "TSB1TD1Unit17";
        public const string cTSB1TD2Amount17 = "TSB1TD2Amount17";
        public const string cTSB1TD2Unit17 = "TSB1TD2Unit17";
        public const string cTSB1MathResult17 = "TSB1MathResult17";
        public const string cTSB1MathSubType17 = "TSB1MathSubType17";

        public const string cTSB1TMAmount17 = "TSB1TMAmount17";
        public const string cTSB1TMUnit17 = "TSB1TMUnit17";
        public const string cTSB1TLAmount17 = "TSB1TLAmount17";
        public const string cTSB1TLUnit17 = "TSB1TLUnit17";
        public const string cTSB1TUAmount17 = "TSB1TUAmount17";
        public const string cTSB1TUUnit17 = "TSB1TUUnit17";
        public const string cTSB1MathOperator17 = "TSB1MathOperator17";
        public const string cTSB1MathExpression17 = "TSB1MathExpression17";
        public const string cTSB1N17 = "TSB1N17";

        //name of indicator 18
        public string TSB1Name18 { get; set; }
        public string TSB1Description18 { get; set; }
        public string TSB1Label18 { get; set; }
        public string TSB1Type18 { get; set; }
        public DateTime TSB1Date18 { get; set; }
        public string TSB1MathType18 { get; set; }
        public string TSB1BaseIO18 { get; set; }
        public double TSB11Amount18 { get; set; }
        public string TSB11Unit18 { get; set; }
        public double TSB12Amount18 { get; set; }
        public string TSB12Unit18 { get; set; }
        public double TSB13Amount18 { get; set; }
        public string TSB13Unit18 { get; set; }
        public double TSB14Amount18 { get; set; }
        public string TSB14Unit18 { get; set; }
        public double TSB15Amount18 { get; set; }
        public string TSB15Unit18 { get; set; }
        public string TSB1RelLabel18 { get; set; }
        public double TSB1TAmount18 { get; set; }
        public string TSB1TUnit18 { get; set; }
        public double TSB1TD1Amount18 { get; set; }
        public string TSB1TD1Unit18 { get; set; }
        public double TSB1TD2Amount18 { get; set; }
        public string TSB1TD2Unit18 { get; set; }
        public string TSB1MathResult18 { get; set; }
        public string TSB1MathSubType18 { get; set; }

        public double TSB1TMAmount18 { get; set; }
        public string TSB1TMUnit18 { get; set; }
        public double TSB1TLAmount18 { get; set; }
        public string TSB1TLUnit18 { get; set; }
        public double TSB1TUAmount18 { get; set; }
        public string TSB1TUUnit18 { get; set; }
        public string TSB1MathOperator18 { get; set; }
        public string TSB1MathExpression18 { get; set; }
        public double TSB1N18 { get; set; }

        public const string cTSB1Name18 = "TSB1Name18";
        public const string cTSB1Description18 = "TSB1Description18";
        public const string cTSB1Label18 = "TSB1Label18";
        public const string cTSB1Type18 = "TSB1Type18";
        public const string cTSB1Date18 = "TSB1Date18";
        public const string cTSB1MathType18 = "TSB1MathType18";
        public const string cTSB1BaseIO18 = "TSB1BaseIO18";
        public const string cTSB11Amount18 = "TSB11Amount18";
        public const string cTSB11Unit18 = "TSB11Unit18";
        public const string cTSB12Amount18 = "TSB12Amount18";
        public const string cTSB12Unit18 = "TSB12Unit18";
        public const string cTSB13Amount18 = "TSB13Amount18";
        public const string cTSB13Unit18 = "TSB13Unit18";
        public const string cTSB14Amount18 = "TSB14Amount18";
        public const string cTSB14Unit18 = "TSB14Unit18";
        public const string cTSB15Amount18 = "TSB15Amount18";
        public const string cTSB15Unit18 = "TSB15Unit18";
        public const string cTSB1RelLabel18 = "TSB1RelLabel18";
        public const string cTSB1TAmount18 = "TSB1TAmount18";
        public const string cTSB1TUnit18 = "TSB1TUnit18";
        public const string cTSB1TD1Amount18 = "TSB1TD1Amount18";
        public const string cTSB1TD1Unit18 = "TSB1TD1Unit18";
        public const string cTSB1TD2Amount18 = "TSB1TD2Amount18";
        public const string cTSB1TD2Unit18 = "TSB1TD2Unit18";
        public const string cTSB1MathResult18 = "TSB1MathResult18";
        public const string cTSB1MathSubType18 = "TSB1MathSubType18";

        public const string cTSB1TMAmount18 = "TSB1TMAmount18";
        public const string cTSB1TMUnit18 = "TSB1TMUnit18";
        public const string cTSB1TLAmount18 = "TSB1TLAmount18";
        public const string cTSB1TLUnit18 = "TSB1TLUnit18";
        public const string cTSB1TUAmount18 = "TSB1TUAmount18";
        public const string cTSB1TUUnit18 = "TSB1TUUnit18";
        public const string cTSB1MathOperator18 = "TSB1MathOperator18";
        public const string cTSB1MathExpression18 = "TSB1MathExpression18";
        public const string cTSB1N18 = "TSB1N18";

        //name of indicator 19
        public string TSB1Name19 { get; set; }
        public string TSB1Description19 { get; set; }
        public string TSB1Label19 { get; set; }
        public string TSB1Type19 { get; set; }
        public DateTime TSB1Date19 { get; set; }
        public string TSB1MathType19 { get; set; }
        public string TSB1BaseIO19 { get; set; }
        public double TSB11Amount19 { get; set; }
        public string TSB11Unit19 { get; set; }
        public double TSB12Amount19 { get; set; }
        public string TSB12Unit19 { get; set; }
        public double TSB13Amount19 { get; set; }
        public string TSB13Unit19 { get; set; }
        public double TSB14Amount19 { get; set; }
        public string TSB14Unit19 { get; set; }
        public double TSB15Amount19 { get; set; }
        public string TSB15Unit19 { get; set; }
        public string TSB1RelLabel19 { get; set; }
        public double TSB1TAmount19 { get; set; }
        public string TSB1TUnit19 { get; set; }
        public double TSB1TD1Amount19 { get; set; }
        public string TSB1TD1Unit19 { get; set; }
        public double TSB1TD2Amount19 { get; set; }
        public string TSB1TD2Unit19 { get; set; }
        public string TSB1MathResult19 { get; set; }
        public string TSB1MathSubType19 { get; set; }

        public double TSB1TMAmount19 { get; set; }
        public string TSB1TMUnit19 { get; set; }
        public double TSB1TLAmount19 { get; set; }
        public string TSB1TLUnit19 { get; set; }
        public double TSB1TUAmount19 { get; set; }
        public string TSB1TUUnit19 { get; set; }
        public string TSB1MathOperator19 { get; set; }
        public string TSB1MathExpression19 { get; set; }
        public double TSB1N19 { get; set; }

        public const string cTSB1Name19 = "TSB1Name19";
        public const string cTSB1Description19 = "TSB1Description19";
        public const string cTSB1Label19 = "TSB1Label19";
        public const string cTSB1Type19 = "TSB1Type19";
        public const string cTSB1Date19 = "TSB1Date19";
        public const string cTSB1MathType19 = "TSB1MathType19";
        public const string cTSB1BaseIO19 = "TSB1BaseIO19";
        public const string cTSB11Amount19 = "TSB11Amount19";
        public const string cTSB11Unit19 = "TSB11Unit19";
        public const string cTSB12Amount19 = "TSB12Amount19";
        public const string cTSB12Unit19 = "TSB12Unit19";
        public const string cTSB13Amount19 = "TSB13Amount19";
        public const string cTSB13Unit19 = "TSB13Unit19";
        public const string cTSB14Amount19 = "TSB14Amount19";
        public const string cTSB14Unit19 = "TSB14Unit19";
        public const string cTSB15Amount19 = "TSB15Amount19";
        public const string cTSB15Unit19 = "TSB15Unit19";
        public const string cTSB1RelLabel19 = "TSB1RelLabel19";
        public const string cTSB1TAmount19 = "TSB1TAmount19";
        public const string cTSB1TUnit19 = "TSB1TUnit19";
        public const string cTSB1TD1Amount19 = "TSB1TD1Amount19";
        public const string cTSB1TD1Unit19 = "TSB1TD1Unit19";
        public const string cTSB1TD2Amount19 = "TSB1TD2Amount19";
        public const string cTSB1TD2Unit19 = "TSB1TD2Unit19";
        public const string cTSB1MathResult19 = "TSB1MathResult19";
        public const string cTSB1MathSubType19 = "TSB1MathSubType19";

        public const string cTSB1TMAmount19 = "TSB1TMAmount19";
        public const string cTSB1TMUnit19 = "TSB1TMUnit19";
        public const string cTSB1TLAmount19 = "TSB1TLAmount19";
        public const string cTSB1TLUnit19 = "TSB1TLUnit19";
        public const string cTSB1TUAmount19 = "TSB1TUAmount19";
        public const string cTSB1TUUnit19 = "TSB1TUUnit19";
        public const string cTSB1MathOperator19 = "TSB1MathOperator19";
        public const string cTSB1MathExpression19 = "TSB1MathExpression19";
        public const string cTSB1N19 = "TSB1N19";
        //name of indicator 20
        public string TSB1Name20 { get; set; }
        public string TSB1Description20 { get; set; }
        public string TSB1Label20 { get; set; }
        public string TSB1Type20 { get; set; }
        public DateTime TSB1Date20 { get; set; }
        public string TSB1MathType20 { get; set; }
        public string TSB1BaseIO20 { get; set; }
        public double TSB11Amount20 { get; set; }
        public string TSB11Unit20 { get; set; }
        public double TSB12Amount20 { get; set; }
        public string TSB12Unit20 { get; set; }
        public double TSB13Amount20 { get; set; }
        public string TSB13Unit20 { get; set; }
        public double TSB14Amount20 { get; set; }
        public string TSB14Unit20 { get; set; }
        public double TSB15Amount20 { get; set; }
        public string TSB15Unit20 { get; set; }
        public string TSB1RelLabel20 { get; set; }
        public double TSB1TAmount20 { get; set; }
        public string TSB1TUnit20 { get; set; }
        public double TSB1TD1Amount20 { get; set; }
        public string TSB1TD1Unit20 { get; set; }
        public double TSB1TD2Amount20 { get; set; }
        public string TSB1TD2Unit20 { get; set; }
        public string TSB1MathResult20 { get; set; }
        public string TSB1MathSubType20 { get; set; }

        public double TSB1TMAmount20 { get; set; }
        public string TSB1TMUnit20 { get; set; }
        public double TSB1TLAmount20 { get; set; }
        public string TSB1TLUnit20 { get; set; }
        public double TSB1TUAmount20 { get; set; }
        public string TSB1TUUnit20 { get; set; }
        public string TSB1MathOperator20 { get; set; }
        public string TSB1MathExpression20 { get; set; }
        public double TSB1N20 { get; set; }

        public const string cTSB1Name20 = "TSB1Name20";
        public const string cTSB1Description20 = "TSB1Description20";
        public const string cTSB1Label20 = "TSB1Label20";
        public const string cTSB1Type20 = "TSB1Type20";
        public const string cTSB1Date20 = "TSB1Date20";
        public const string cTSB1MathType20 = "TSB1MathType20";
        public const string cTSB1BaseIO20 = "TSB1BaseIO20";
        public const string cTSB11Amount20 = "TSB11Amount20";
        public const string cTSB11Unit20 = "TSB11Unit20";
        public const string cTSB12Amount20 = "TSB12Amount20";
        public const string cTSB12Unit20 = "TSB12Unit20";
        public const string cTSB13Amount20 = "TSB13Amount20";
        public const string cTSB13Unit20 = "TSB13Unit20";
        public const string cTSB14Amount20 = "TSB14Amount20";
        public const string cTSB14Unit20 = "TSB14Unit20";
        public const string cTSB15Amount20 = "TSB15Amount20";
        public const string cTSB15Unit20 = "TSB15Unit20";
        public const string cTSB1RelLabel20 = "TSB1RelLabel20";
        public const string cTSB1TAmount20 = "TSB1TAmount20";
        public const string cTSB1TUnit20 = "TSB1TUnit20";
        public const string cTSB1TD1Amount20 = "TSB1TD1Amount20";
        public const string cTSB1TD1Unit20 = "TSB1TD1Unit20";
        public const string cTSB1TD2Amount20 = "TSB1TD2Amount20";
        public const string cTSB1TD2Unit20 = "TSB1TD2Unit20";
        public const string cTSB1MathResult20 = "TSB1MathResult20";
        public const string cTSB1MathSubType20 = "TSB1MathSubType20";

        public const string cTSB1TMAmount20 = "TSB1TMAmount20";
        public const string cTSB1TMUnit20 = "TSB1TMUnit20";
        public const string cTSB1TLAmount20 = "TSB1TLAmount20";
        public const string cTSB1TLUnit20 = "TSB1TLUnit20";
        public const string cTSB1TUAmount20 = "TSB1TUAmount20";
        public const string cTSB1TUUnit20 = "TSB1TUUnit20";
        public const string cTSB1MathOperator20 = "TSB1MathOperator20";
        public const string cTSB1MathExpression20 = "TSB1MathExpression20";
        public const string cTSB1N20 = "TSB1N20";

        public virtual void InitTSB1BaseStockProperties()
        {
            this.ErrorMessage = string.Empty;
            this.TSB1Score = 0;
            this.TSB1ScoreUnit = string.Empty;
            this.TSB1ScoreD1Amount = 0;
            this.TSB1ScoreD1Unit = string.Empty;
            this.TSB1ScoreD2Amount = 0;
            this.TSB1ScoreD2Unit = string.Empty;
            this.TSB1ScoreMathExpression = string.Empty;
            this.TSB1ScoreM = 0;
            this.TSB1ScoreMUnit = string.Empty;
            this.TSB1ScoreLAmount = 0;
            this.TSB1ScoreLUnit = string.Empty;
            this.TSB1ScoreUAmount = 0;
            this.TSB1ScoreUUnit = string.Empty;

            this.TSB1Iterations = 0;
            this.TSB1ScoreDistType = string.Empty;
            this.TSB1ScoreMathType = string.Empty;
            this.TSB1ScoreMathResult = string.Empty;
            this.TSB1ScoreMathSubType = string.Empty;
            this.DataToAnalyze = new Dictionary<string, List<List<double>>>();
            this.TSB1ScoreN = 0;
            this.TSB1Description1 = string.Empty;
            this.TSB1Name1 = string.Empty;
            this.TSB1Label1 = string.Empty;
            this.TSB1Type1 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel1 = string.Empty;
            this.TSB1TAmount1 = 0;
            this.TSB1TUnit1 = string.Empty;
            this.TSB1TD1Amount1 = 0;
            this.TSB1TD1Unit1 = string.Empty;
            this.TSB1TD2Amount1 = 0;
            this.TSB1TD2Unit1 = string.Empty;
            this.TSB1MathResult1 = string.Empty;
            this.TSB1MathSubType1 = Constants.NONE;

            this.TSB1TMAmount1 = 0;
            this.TSB1TMUnit1 = string.Empty;
            this.TSB1TLAmount1 = 0;
            this.TSB1TLUnit1 = string.Empty;
            this.TSB1TUAmount1 = 0;
            this.TSB1TUUnit1 = string.Empty;
            this.TSB1MathOperator1 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression1 = string.Empty;
            this.TSB1N1 = 0;
            this.TSB1Date1 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType1 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO1 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount1 = 0;
            this.TSB11Unit1 = string.Empty;
            this.TSB12Amount1 = 0;
            this.TSB12Unit1 = string.Empty;
            this.TSB15Amount1 = 0;
            this.TSB15Unit1 = string.Empty;
            this.TSB13Amount1 = 0;
            this.TSB13Unit1 = string.Empty;
            this.TSB14Amount1 = 0;
            this.TSB14Unit1 = string.Empty;
            this.TSB1Description2 = string.Empty;
            this.TSB1Name2 = string.Empty;
            this.TSB1Label2 = string.Empty;
            this.TSB1Type2 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel2 = string.Empty;
            this.TSB1TAmount2 = 0;
            this.TSB1TUnit2 = string.Empty;
            this.TSB1TD1Amount2 = 0;
            this.TSB1TD1Unit2 = string.Empty;
            this.TSB1TD2Amount2 = 0;
            this.TSB1TD2Unit2 = string.Empty;
            this.TSB1MathResult2 = string.Empty;
            this.TSB1MathSubType2 = Constants.NONE;

            this.TSB1TMAmount2 = 0;
            this.TSB1TMUnit2 = string.Empty;
            this.TSB1TLAmount2 = 0;
            this.TSB1TLUnit2 = string.Empty;
            this.TSB1TUAmount2 = 0;
            this.TSB1TUUnit2 = string.Empty;
            this.TSB1MathOperator2 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression2 = string.Empty;
            this.TSB1N2 = 0;
            this.TSB1Date2 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType2 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO2 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount2 = 0;
            this.TSB11Unit2 = string.Empty;
            this.TSB12Amount2 = 0;
            this.TSB12Unit2 = string.Empty;
            this.TSB15Amount2 = 0;
            this.TSB15Unit2 = string.Empty;
            this.TSB13Amount2 = 0;
            this.TSB13Unit2 = string.Empty;
            this.TSB14Amount2 = 0;
            this.TSB14Unit2 = string.Empty;
            this.TSB1Description3 = string.Empty;
            this.TSB1Name3 = string.Empty;
            this.TSB1Label3 = string.Empty;
            this.TSB1Type3 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel3 = string.Empty;
            this.TSB1TAmount3 = 0;
            this.TSB1TUnit3 = string.Empty;
            this.TSB1TD1Amount3 = 0;
            this.TSB1TD1Unit3 = string.Empty;
            this.TSB1TD2Amount3 = 0;
            this.TSB1TD2Unit3 = string.Empty;
            this.TSB1MathResult3 = string.Empty;
            this.TSB1MathSubType3 = Constants.NONE;

            this.TSB1TMAmount3 = 0;
            this.TSB1TMUnit3 = string.Empty;
            this.TSB1TLAmount3 = 0;
            this.TSB1TLUnit3 = string.Empty;
            this.TSB1TUAmount3 = 0;
            this.TSB1TUUnit3 = string.Empty;
            this.TSB1MathOperator3 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression3 = string.Empty;
            this.TSB1N3 = 0;
            this.TSB1Date3 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType3 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO3 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount3 = 0;
            this.TSB11Unit3 = string.Empty;
            this.TSB12Amount3 = 0;
            this.TSB12Unit3 = string.Empty;
            this.TSB15Amount3 = 0;
            this.TSB15Unit3 = string.Empty;
            this.TSB13Amount3 = 0;
            this.TSB13Unit3 = string.Empty;
            this.TSB14Amount3 = 0;
            this.TSB14Unit3 = string.Empty;
            this.TSB1Description4 = string.Empty;
            this.TSB1Name4 = string.Empty;
            this.TSB1Label4 = string.Empty;
            this.TSB1Type4 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel4 = string.Empty;
            this.TSB1TAmount4 = 0;
            this.TSB1TUnit4 = string.Empty;
            this.TSB1TD1Amount4 = 0;
            this.TSB1TD1Unit4 = string.Empty;
            this.TSB1TD2Amount4 = 0;
            this.TSB1TD2Unit4 = string.Empty;
            this.TSB1MathResult4 = string.Empty;
            this.TSB1MathSubType4 = Constants.NONE;

            this.TSB1TMAmount4 = 0;
            this.TSB1TMUnit4 = string.Empty;
            this.TSB1TLAmount4 = 0;
            this.TSB1TLUnit4 = string.Empty;
            this.TSB1TUAmount4 = 0;
            this.TSB1TUUnit4 = string.Empty;
            this.TSB1MathOperator4 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression4 = string.Empty;
            this.TSB1N4 = 0;
            this.TSB1Date4 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType4 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO4 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount4 = 0;
            this.TSB11Unit4 = string.Empty;
            this.TSB12Amount4 = 0;
            this.TSB12Unit4 = string.Empty;
            this.TSB15Amount4 = 0;
            this.TSB15Unit4 = string.Empty;
            this.TSB13Amount4 = 0;
            this.TSB13Unit4 = string.Empty;
            this.TSB14Amount4 = 0;
            this.TSB14Unit4 = string.Empty;


            this.TSB1Description5 = string.Empty;
            this.TSB1Name5 = string.Empty;
            this.TSB1Label5 = string.Empty;
            this.TSB1Type5 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel5 = string.Empty;
            this.TSB1TAmount5 = 0;
            this.TSB1TUnit5 = string.Empty;
            this.TSB1TD1Amount5 = 0;
            this.TSB1TD1Unit5 = string.Empty;
            this.TSB1TD2Amount5 = 0;
            this.TSB1TD2Unit5 = string.Empty;
            this.TSB1MathResult5 = string.Empty;
            this.TSB1MathSubType5 = Constants.NONE;

            this.TSB1TMAmount5 = 0;
            this.TSB1TMUnit5 = string.Empty;
            this.TSB1TLAmount5 = 0;
            this.TSB1TLUnit5 = string.Empty;
            this.TSB1TUAmount5 = 0;
            this.TSB1TUUnit5 = string.Empty;
            this.TSB1MathOperator5 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression5 = string.Empty;
            this.TSB1N5 = 0;
            this.TSB1Date5 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType5 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO5 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount5 = 0;
            this.TSB11Unit5 = string.Empty;
            this.TSB12Amount5 = 0;
            this.TSB12Unit5 = string.Empty;
            this.TSB15Amount5 = 0;
            this.TSB15Unit5 = string.Empty;
            this.TSB13Amount5 = 0;
            this.TSB13Unit5 = string.Empty;
            this.TSB14Amount5 = 0;
            this.TSB14Unit5 = string.Empty;

            this.TSB1Description6 = string.Empty;
            this.TSB1Name6 = string.Empty;
            this.TSB1Label6 = string.Empty;
            this.TSB1Type6 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel6 = string.Empty;
            this.TSB1TAmount6 = 0;
            this.TSB1TUnit6 = string.Empty;
            this.TSB1TD1Amount6 = 0;
            this.TSB1TD1Unit6 = string.Empty;
            this.TSB1TD2Amount6 = 0;
            this.TSB1TD2Unit6 = string.Empty;
            this.TSB1MathResult6 = string.Empty;
            this.TSB1MathSubType6 = Constants.NONE;

            this.TSB1TMAmount6 = 0;
            this.TSB1TMUnit6 = string.Empty;
            this.TSB1TLAmount6 = 0;
            this.TSB1TLUnit6 = string.Empty;
            this.TSB1TUAmount6 = 0;
            this.TSB1TUUnit6 = string.Empty;
            this.TSB1MathOperator6 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression6 = string.Empty;
            this.TSB1N6 = 0;
            this.TSB1Date6 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType6 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO6 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount6 = 0;
            this.TSB11Unit6 = string.Empty;
            this.TSB12Amount6 = 0;
            this.TSB12Unit6 = string.Empty;
            this.TSB15Amount6 = 0;
            this.TSB15Unit6 = string.Empty;
            this.TSB13Amount6 = 0;
            this.TSB13Unit6 = string.Empty;
            this.TSB14Amount6 = 0;
            this.TSB14Unit6 = string.Empty;

            this.TSB1Description7 = string.Empty;
            this.TSB1Name7 = string.Empty;
            this.TSB1Label7 = string.Empty;
            this.TSB1Type7 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel7 = string.Empty;
            this.TSB1TAmount7 = 0;
            this.TSB1TUnit7 = string.Empty;
            this.TSB1TD1Amount7 = 0;
            this.TSB1TD1Unit7 = string.Empty;
            this.TSB1TD2Amount7 = 0;
            this.TSB1TD2Unit7 = string.Empty;
            this.TSB1MathResult7 = string.Empty;
            this.TSB1MathSubType7 = Constants.NONE;

            this.TSB1TMAmount7 = 0;
            this.TSB1TMUnit7 = string.Empty;
            this.TSB1TLAmount7 = 0;
            this.TSB1TLUnit7 = string.Empty;
            this.TSB1TUAmount7 = 0;
            this.TSB1TUUnit7 = string.Empty;
            this.TSB1MathOperator7 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression7 = string.Empty;
            this.TSB1N7 = 0;
            this.TSB1Date7 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType7 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO7 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount7 = 0;
            this.TSB11Unit7 = string.Empty;
            this.TSB12Amount7 = 0;
            this.TSB12Unit7 = string.Empty;
            this.TSB15Amount7 = 0;
            this.TSB15Unit7 = string.Empty;
            this.TSB13Amount7 = 0;
            this.TSB13Unit7 = string.Empty;
            this.TSB14Amount7 = 0;
            this.TSB14Unit7 = string.Empty;

            this.TSB1Description8 = string.Empty;
            this.TSB1Name8 = string.Empty;
            this.TSB1Label8 = string.Empty;
            this.TSB1Type8 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel8 = string.Empty;
            this.TSB1TAmount8 = 0;
            this.TSB1TUnit8 = string.Empty;
            this.TSB1TD1Amount8 = 0;
            this.TSB1TD1Unit8 = string.Empty;
            this.TSB1TD2Amount8 = 0;
            this.TSB1TD2Unit8 = string.Empty;
            this.TSB1MathResult8 = string.Empty;
            this.TSB1MathSubType8 = Constants.NONE;

            this.TSB1TMAmount8 = 0;
            this.TSB1TMUnit8 = string.Empty;
            this.TSB1TLAmount8 = 0;
            this.TSB1TLUnit8 = string.Empty;
            this.TSB1TUAmount8 = 0;
            this.TSB1TUUnit8 = string.Empty;
            this.TSB1MathOperator8 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression8 = string.Empty;
            this.TSB1N8 = 0;
            this.TSB1Date8 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType8 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO8 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount8 = 0;
            this.TSB11Unit8 = string.Empty;
            this.TSB12Amount8 = 0;
            this.TSB12Unit8 = string.Empty;
            this.TSB15Amount8 = 0;
            this.TSB15Unit8 = string.Empty;
            this.TSB13Amount8 = 0;
            this.TSB13Unit8 = string.Empty;
            this.TSB14Amount8 = 0;
            this.TSB14Unit8 = string.Empty;

            this.TSB1Description9 = string.Empty;
            this.TSB1Name9 = string.Empty;
            this.TSB1Label9 = string.Empty;
            this.TSB1Type9 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel9 = string.Empty;
            this.TSB1TAmount9 = 0;
            this.TSB1TUnit9 = string.Empty;
            this.TSB1TD1Amount9 = 0;
            this.TSB1TD1Unit9 = string.Empty;
            this.TSB1TD2Amount9 = 0;
            this.TSB1TD2Unit9 = string.Empty;
            this.TSB1MathResult9 = string.Empty;
            this.TSB1MathSubType9 = Constants.NONE;

            this.TSB1TMAmount9 = 0;
            this.TSB1TMUnit9 = string.Empty;
            this.TSB1TLAmount9 = 0;
            this.TSB1TLUnit9 = string.Empty;
            this.TSB1TUAmount9 = 0;
            this.TSB1TUUnit9 = string.Empty;
            this.TSB1MathOperator9 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression9 = string.Empty;
            this.TSB1N9 = 0;
            this.TSB1Date9 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType9 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO9 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount9 = 0;
            this.TSB11Unit9 = string.Empty;
            this.TSB12Amount9 = 0;
            this.TSB12Unit9 = string.Empty;
            this.TSB15Amount9 = 0;
            this.TSB15Unit9 = string.Empty;
            this.TSB13Amount9 = 0;
            this.TSB13Unit9 = string.Empty;
            this.TSB14Amount9 = 0;
            this.TSB14Unit9 = string.Empty;

            this.TSB1Description10 = string.Empty;
            this.TSB1Name10 = string.Empty;
            this.TSB1Label10 = string.Empty;
            this.TSB1Type10 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel10 = string.Empty;
            this.TSB1TAmount10 = 0;
            this.TSB1TUnit10 = string.Empty;
            this.TSB1TD1Amount10 = 0;
            this.TSB1TD1Unit10 = string.Empty;
            this.TSB1TD2Amount10 = 0;
            this.TSB1TD2Unit10 = string.Empty;
            this.TSB1MathResult10 = string.Empty;
            this.TSB1MathSubType10 = Constants.NONE;

            this.TSB1TMAmount10 = 0;
            this.TSB1TMUnit10 = string.Empty;
            this.TSB1TLAmount10 = 0;
            this.TSB1TLUnit10 = string.Empty;
            this.TSB1TUAmount10 = 0;
            this.TSB1TUUnit10 = string.Empty;
            this.TSB1MathOperator10 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression10 = string.Empty;
            this.TSB1N10 = 0;
            this.TSB1Date10 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType10 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO10 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount10 = 0;
            this.TSB11Unit10 = string.Empty;
            this.TSB12Amount10 = 0;
            this.TSB12Unit10 = string.Empty;
            this.TSB15Amount10 = 0;
            this.TSB15Unit10 = string.Empty;
            this.TSB13Amount10 = 0;
            this.TSB13Unit10 = string.Empty;
            this.TSB14Amount10 = 0;
            this.TSB14Unit10 = string.Empty;

            this.TSB1Description11 = string.Empty;
            this.TSB1Name11 = string.Empty;
            this.TSB1Label11 = string.Empty;
            this.TSB1Type11 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel11 = string.Empty;
            this.TSB1TAmount11 = 0;
            this.TSB1TUnit11 = string.Empty;
            this.TSB1TD1Amount11 = 0;
            this.TSB1TD1Unit11 = string.Empty;
            this.TSB1TD2Amount11 = 0;
            this.TSB1TD2Unit11 = string.Empty;
            this.TSB1MathResult11 = string.Empty;
            this.TSB1MathSubType11 = Constants.NONE;

            this.TSB1TMAmount11 = 0;
            this.TSB1TMUnit11 = string.Empty;
            this.TSB1TLAmount11 = 0;
            this.TSB1TLUnit11 = string.Empty;
            this.TSB1TUAmount11 = 0;
            this.TSB1TUUnit11 = string.Empty;
            this.TSB1MathOperator11 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression11 = string.Empty;
            this.TSB1N11 = 0;
            this.TSB1Date11 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType11 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO11 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount11 = 0;
            this.TSB11Unit11 = string.Empty;
            this.TSB12Amount11 = 0;
            this.TSB12Unit11 = string.Empty;
            this.TSB15Amount11 = 0;
            this.TSB15Unit11 = string.Empty;
            this.TSB13Amount11 = 0;
            this.TSB13Unit11 = string.Empty;
            this.TSB14Amount11 = 0;
            this.TSB14Unit11 = string.Empty;

            this.TSB1Description12 = string.Empty;
            this.TSB1Name12 = string.Empty;
            this.TSB1Label12 = string.Empty;
            this.TSB1Type12 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel12 = string.Empty;
            this.TSB1TAmount12 = 0;
            this.TSB1TUnit12 = string.Empty;
            this.TSB1TD1Amount12 = 0;
            this.TSB1TD1Unit12 = string.Empty;
            this.TSB1TD2Amount12 = 0;
            this.TSB1TD2Unit12 = string.Empty;
            this.TSB1MathResult12 = string.Empty;
            this.TSB1MathSubType12 = Constants.NONE;

            this.TSB1TMAmount12 = 0;
            this.TSB1TMUnit12 = string.Empty;
            this.TSB1TLAmount12 = 0;
            this.TSB1TLUnit12 = string.Empty;
            this.TSB1TUAmount12 = 0;
            this.TSB1TUUnit12 = string.Empty;
            this.TSB1MathOperator12 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression12 = string.Empty;
            this.TSB1N12 = 0;
            this.TSB1Date12 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType12 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO12 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount12 = 0;
            this.TSB11Unit12 = string.Empty;
            this.TSB12Amount12 = 0;
            this.TSB12Unit12 = string.Empty;
            this.TSB15Amount12 = 0;
            this.TSB15Unit12 = string.Empty;
            this.TSB13Amount12 = 0;
            this.TSB13Unit12 = string.Empty;
            this.TSB14Amount12 = 0;
            this.TSB14Unit12 = string.Empty;

            this.TSB1Description13 = string.Empty;
            this.TSB1Name13 = string.Empty;
            this.TSB1Label13 = string.Empty;
            this.TSB1Type13 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel13 = string.Empty;
            this.TSB1TAmount13 = 0;
            this.TSB1TUnit13 = string.Empty;
            this.TSB1TD1Amount13 = 0;
            this.TSB1TD1Unit13 = string.Empty;
            this.TSB1TD2Amount13 = 0;
            this.TSB1TD2Unit13 = string.Empty;
            this.TSB1MathResult13 = string.Empty;
            this.TSB1MathSubType13 = Constants.NONE;

            this.TSB1TMAmount13 = 0;
            this.TSB1TMUnit13 = string.Empty;
            this.TSB1TLAmount13 = 0;
            this.TSB1TLUnit13 = string.Empty;
            this.TSB1TUAmount13 = 0;
            this.TSB1TUUnit13 = string.Empty;
            this.TSB1MathOperator13 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression13 = string.Empty;
            this.TSB1N13 = 0;
            this.TSB1Date13 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType13 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO13 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount13 = 0;
            this.TSB11Unit13 = string.Empty;
            this.TSB12Amount13 = 0;
            this.TSB12Unit13 = string.Empty;
            this.TSB15Amount13 = 0;
            this.TSB15Unit13 = string.Empty;
            this.TSB13Amount13 = 0;
            this.TSB13Unit13 = string.Empty;
            this.TSB14Amount13 = 0;
            this.TSB14Unit13 = string.Empty;

            this.TSB1Description14 = string.Empty;
            this.TSB1Name14 = string.Empty;
            this.TSB1Label14 = string.Empty;
            this.TSB1Type14 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel14 = string.Empty;
            this.TSB1TAmount14 = 0;
            this.TSB1TUnit14 = string.Empty;
            this.TSB1TD1Amount14 = 0;
            this.TSB1TD1Unit14 = string.Empty;
            this.TSB1TD2Amount14 = 0;
            this.TSB1TD2Unit14 = string.Empty;
            this.TSB1MathResult14 = string.Empty;
            this.TSB1MathSubType14 = Constants.NONE;

            this.TSB1TMAmount14 = 0;
            this.TSB1TMUnit14 = string.Empty;
            this.TSB1TLAmount14 = 0;
            this.TSB1TLUnit14 = string.Empty;
            this.TSB1TUAmount14 = 0;
            this.TSB1TUUnit14 = string.Empty;
            this.TSB1MathOperator14 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression14 = string.Empty;
            this.TSB1N14 = 0;
            this.TSB1Date14 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType14 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO14 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount14 = 0;
            this.TSB11Unit14 = string.Empty;
            this.TSB12Amount14 = 0;
            this.TSB12Unit14 = string.Empty;
            this.TSB15Amount14 = 0;
            this.TSB15Unit14 = string.Empty;
            this.TSB13Amount14 = 0;
            this.TSB13Unit14 = string.Empty;
            this.TSB14Amount14 = 0;
            this.TSB14Unit14 = string.Empty;

            this.TSB1Description15 = string.Empty;
            this.TSB1Name15 = string.Empty;
            this.TSB1Label15 = string.Empty;
            this.TSB1Type15 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel15 = string.Empty;
            this.TSB1TAmount15 = 0;
            this.TSB1TUnit15 = string.Empty;
            this.TSB1TD1Amount15 = 0;
            this.TSB1TD1Unit15 = string.Empty;
            this.TSB1TD2Amount15 = 0;
            this.TSB1TD2Unit15 = string.Empty;
            this.TSB1MathResult15 = string.Empty;
            this.TSB1MathSubType15 = Constants.NONE;

            this.TSB1TMAmount15 = 0;
            this.TSB1TMUnit15 = string.Empty;
            this.TSB1TLAmount15 = 0;
            this.TSB1TLUnit15 = string.Empty;
            this.TSB1TUAmount15 = 0;
            this.TSB1TUUnit15 = string.Empty;
            this.TSB1MathOperator15 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression15 = string.Empty;
            this.TSB1N15 = 0;
            this.TSB1Date15 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType15 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO15 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount15 = 0;
            this.TSB11Unit15 = string.Empty;
            this.TSB12Amount15 = 0;
            this.TSB12Unit15 = string.Empty;
            this.TSB15Amount15 = 0;
            this.TSB15Unit15 = string.Empty;
            this.TSB13Amount15 = 0;
            this.TSB13Unit15 = string.Empty;
            this.TSB14Amount15 = 0;
            this.TSB14Unit15 = string.Empty;

            this.TSB1Description16 = string.Empty;
            this.TSB1Name16 = string.Empty;
            this.TSB1Label16 = string.Empty;
            this.TSB1Type16 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel16 = string.Empty;
            this.TSB1TAmount16 = 0;
            this.TSB1TUnit16 = string.Empty;
            this.TSB1TD1Amount16 = 0;
            this.TSB1TD1Unit16 = string.Empty;
            this.TSB1TD2Amount16 = 0;
            this.TSB1TD2Unit16 = string.Empty;
            this.TSB1MathResult16 = string.Empty;
            this.TSB1MathSubType16 = Constants.NONE;

            this.TSB1TMAmount16 = 0;
            this.TSB1TMUnit16 = string.Empty;
            this.TSB1TLAmount16 = 0;
            this.TSB1TLUnit16 = string.Empty;
            this.TSB1TUAmount16 = 0;
            this.TSB1TUUnit16 = string.Empty;
            this.TSB1MathOperator16 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression16 = string.Empty;
            this.TSB1N16 = 0;
            this.TSB1Date16 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType16 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO16 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount16 = 0;
            this.TSB11Unit16 = string.Empty;
            this.TSB12Amount16 = 0;
            this.TSB12Unit16 = string.Empty;
            this.TSB15Amount16 = 0;
            this.TSB15Unit16 = string.Empty;
            this.TSB13Amount16 = 0;
            this.TSB13Unit16 = string.Empty;
            this.TSB14Amount16 = 0;
            this.TSB14Unit16 = string.Empty;

            this.TSB1Description17 = string.Empty;
            this.TSB1Name17 = string.Empty;
            this.TSB1Label17 = string.Empty;
            this.TSB1Type17 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel17 = string.Empty;
            this.TSB1TAmount17 = 0;
            this.TSB1TUnit17 = string.Empty;
            this.TSB1TD1Amount17 = 0;
            this.TSB1TD1Unit17 = string.Empty;
            this.TSB1TD2Amount17 = 0;
            this.TSB1TD2Unit17 = string.Empty;
            this.TSB1MathResult17 = string.Empty;
            this.TSB1MathSubType17 = Constants.NONE;

            this.TSB1TMAmount17 = 0;
            this.TSB1TMUnit17 = string.Empty;
            this.TSB1TLAmount17 = 0;
            this.TSB1TLUnit17 = string.Empty;
            this.TSB1TUAmount17 = 0;
            this.TSB1TUUnit17 = string.Empty;
            this.TSB1MathOperator17 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression17 = string.Empty;
            this.TSB1N17 = 0;
            this.TSB1Date17 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType17 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO17 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount17 = 0;
            this.TSB11Unit17 = string.Empty;
            this.TSB12Amount17 = 0;
            this.TSB12Unit17 = string.Empty;
            this.TSB15Amount17 = 0;
            this.TSB15Unit17 = string.Empty;
            this.TSB13Amount17 = 0;
            this.TSB13Unit17 = string.Empty;
            this.TSB14Amount17 = 0;
            this.TSB14Unit17 = string.Empty;

            this.TSB1Description18 = string.Empty;
            this.TSB1Name18 = string.Empty;
            this.TSB1Label18 = string.Empty;
            this.TSB1Type18 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel18 = string.Empty;
            this.TSB1TAmount18 = 0;
            this.TSB1TUnit18 = string.Empty;
            this.TSB1TD1Amount18 = 0;
            this.TSB1TD1Unit18 = string.Empty;
            this.TSB1TD2Amount18 = 0;
            this.TSB1TD2Unit18 = string.Empty;
            this.TSB1MathResult18 = string.Empty;
            this.TSB1MathSubType18 = Constants.NONE;

            this.TSB1TMAmount18 = 0;
            this.TSB1TMUnit18 = string.Empty;
            this.TSB1TLAmount18 = 0;
            this.TSB1TLUnit18 = string.Empty;
            this.TSB1TUAmount18 = 0;
            this.TSB1TUUnit18 = string.Empty;
            this.TSB1MathOperator18 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression18 = string.Empty;
            this.TSB1N18 = 0;
            this.TSB1Date18 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType18 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO18 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount18 = 0;
            this.TSB11Unit18 = string.Empty;
            this.TSB12Amount18 = 0;
            this.TSB12Unit18 = string.Empty;
            this.TSB15Amount18 = 0;
            this.TSB15Unit18 = string.Empty;
            this.TSB13Amount18 = 0;
            this.TSB13Unit18 = string.Empty;
            this.TSB14Amount18 = 0;
            this.TSB14Unit18 = string.Empty;

            this.TSB1Description19 = string.Empty;
            this.TSB1Name19 = string.Empty;
            this.TSB1Label19 = string.Empty;
            this.TSB1Type19 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel19 = string.Empty;
            this.TSB1TAmount19 = 0;
            this.TSB1TUnit19 = string.Empty;
            this.TSB1TD1Amount19 = 0;
            this.TSB1TD1Unit19 = string.Empty;
            this.TSB1TD2Amount19 = 0;
            this.TSB1TD2Unit19 = string.Empty;
            this.TSB1MathResult19 = string.Empty;
            this.TSB1MathSubType19 = Constants.NONE;

            this.TSB1TMAmount19 = 0;
            this.TSB1TMUnit19 = string.Empty;
            this.TSB1TLAmount19 = 0;
            this.TSB1TLUnit19 = string.Empty;
            this.TSB1TUAmount19 = 0;
            this.TSB1TUUnit19 = string.Empty;
            this.TSB1MathOperator19 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression19 = string.Empty;
            this.TSB1N19 = 0;
            this.TSB1Date19 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType19 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO19 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount19 = 0;
            this.TSB11Unit19 = string.Empty;
            this.TSB12Amount19 = 0;
            this.TSB12Unit19 = string.Empty;
            this.TSB15Amount19 = 0;
            this.TSB15Unit19 = string.Empty;
            this.TSB13Amount19 = 0;
            this.TSB13Unit19 = string.Empty;
            this.TSB14Amount19 = 0;
            this.TSB14Unit19 = string.Empty;

            this.TSB1Description20 = string.Empty;
            this.TSB1Name20 = string.Empty;
            this.TSB1Label20 = string.Empty;
            this.TSB1Type20 = RUC_TYPES.none.ToString();
            this.TSB1RelLabel20 = string.Empty;
            this.TSB1TAmount20 = 0;
            this.TSB1TUnit20 = string.Empty;
            this.TSB1TD1Amount20 = 0;
            this.TSB1TD1Unit20 = string.Empty;
            this.TSB1TD2Amount20 = 0;
            this.TSB1TD2Unit20 = string.Empty;
            this.TSB1MathResult20 = string.Empty;
            this.TSB1MathSubType20 = Constants.NONE;

            this.TSB1TMAmount20 = 0;
            this.TSB1TMUnit20 = string.Empty;
            this.TSB1TLAmount20 = 0;
            this.TSB1TLUnit20 = string.Empty;
            this.TSB1TUAmount20 = 0;
            this.TSB1TUUnit20 = string.Empty;
            this.TSB1MathOperator20 = MATH_OPERATOR_TYPES.none.ToString();
            this.TSB1MathExpression20 = string.Empty;
            this.TSB1N20 = 0;
            this.TSB1Date20 = CalculatorHelpers.GetDateShortNow();
            this.TSB1MathType20 = MATH_TYPES.none.ToString();
            this.TSB1BaseIO20 = SB1Base.BASEIO_TYPES.none.ToString();
            this.TSB11Amount20 = 0;
            this.TSB11Unit20 = string.Empty;
            this.TSB12Amount20 = 0;
            this.TSB12Unit20 = string.Empty;
            this.TSB15Amount20 = 0;
            this.TSB15Unit20 = string.Empty;
            this.TSB13Amount20 = 0;
            this.TSB13Unit20 = string.Empty;
            this.TSB14Amount20 = 0;
            this.TSB14Unit20 = string.Empty;
        }

        public virtual void CopyTSB1BaseStockProperties(
            TSB1BaseStock calculator)
        {
            this.ErrorMessage = calculator.ErrorMessage;
            this.TSB1Score = calculator.TSB1Score;
            this.TSB1ScoreUnit = calculator.TSB1ScoreUnit;
            this.TSB1ScoreD1Amount = calculator.TSB1ScoreD1Amount;
            this.TSB1ScoreD1Unit = calculator.TSB1ScoreD1Unit;
            this.TSB1ScoreD2Amount = calculator.TSB1ScoreD2Amount;
            this.TSB1ScoreD2Unit = calculator.TSB1ScoreD2Unit;
            this.TSB1ScoreMathExpression = calculator.TSB1ScoreMathExpression;
            this.TSB1ScoreM = calculator.TSB1ScoreM;
            this.TSB1ScoreMUnit = calculator.TSB1ScoreMUnit;
            this.TSB1ScoreLAmount = calculator.TSB1ScoreLAmount;
            this.TSB1ScoreLUnit = calculator.TSB1ScoreLUnit;
            this.TSB1ScoreUAmount = calculator.TSB1ScoreUAmount;
            this.TSB1ScoreUUnit = calculator.TSB1ScoreUUnit;
            this.TSB1Iterations = calculator.TSB1Iterations;
            this.TSB1ScoreDistType = calculator.TSB1ScoreDistType;
            this.TSB1ScoreMathType = calculator.TSB1ScoreMathType;
            this.TSB1ScoreMathResult = calculator.TSB1ScoreMathResult;
            this.TSB1ScoreMathSubType = calculator.TSB1ScoreMathSubType;
            //copy the data from each calculator into an aggregated data object
            this.CopyData(calculator);
            this.TSB1ScoreN = calculator.TSB1ScoreN;
            this.TSB1Description1 = calculator.TSB1Description1;
            this.TSB1Name1 = calculator.TSB1Name1;
            this.TSB1Label1 = calculator.TSB1Label1;
            this.TSB1Type1 = calculator.TSB1Type1;
            this.TSB1RelLabel1 = calculator.TSB1RelLabel1;
            this.TSB1TAmount1 = calculator.TSB1TAmount1;
            this.TSB1TUnit1 = calculator.TSB1TUnit1;
            this.TSB1TD1Amount1 = calculator.TSB1TD1Amount1;
            this.TSB1TD1Unit1 = calculator.TSB1TD1Unit1;
            this.TSB1TD2Amount1 = calculator.TSB1TD2Amount1;
            this.TSB1TD2Unit1 = calculator.TSB1TD2Unit1;
            this.TSB1MathResult1 = calculator.TSB1MathResult1;
            this.TSB1MathSubType1 = calculator.TSB1MathSubType1;

            this.TSB1TMAmount1 = calculator.TSB1TMAmount1;
            this.TSB1TMUnit1 = calculator.TSB1TMUnit1;
            this.TSB1TLAmount1 = calculator.TSB1TLAmount1;
            this.TSB1TLUnit1 = calculator.TSB1TLUnit1;
            this.TSB1TUAmount1 = calculator.TSB1TUAmount1;
            this.TSB1TUUnit1 = calculator.TSB1TUUnit1;
            this.TSB1MathOperator1 = calculator.TSB1MathOperator1;
            this.TSB1MathExpression1 = calculator.TSB1MathExpression1;
            this.TSB1N1 = calculator.TSB1N1;
            this.TSB1Date1 = calculator.TSB1Date1;
            this.TSB1MathType1 = calculator.TSB1MathType1;
            this.TSB1BaseIO1 = calculator.TSB1BaseIO1;
            this.TSB11Amount1 = calculator.TSB11Amount1;
            this.TSB11Unit1 = calculator.TSB11Unit1;
            this.TSB12Amount1 = calculator.TSB12Amount1;
            this.TSB12Unit1 = calculator.TSB12Unit1;
            this.TSB15Amount1 = calculator.TSB15Amount1;
            this.TSB15Unit1 = calculator.TSB15Unit1;
            this.TSB13Amount1 = calculator.TSB13Amount1;
            this.TSB13Unit1 = calculator.TSB13Unit1;
            this.TSB14Amount1 = calculator.TSB14Amount1;
            this.TSB14Unit1 = calculator.TSB14Unit1;

            this.TSB1Description2 = calculator.TSB1Description2;
            this.TSB1Name2 = calculator.TSB1Name2;
            this.TSB1Label2 = calculator.TSB1Label2;
            this.TSB1Type2 = calculator.TSB1Type2;
            this.TSB1RelLabel2 = calculator.TSB1RelLabel2;
            this.TSB1TAmount2 = calculator.TSB1TAmount2;
            this.TSB1TUnit2 = calculator.TSB1TUnit2;
            this.TSB1TD1Amount2 = calculator.TSB1TD1Amount2;
            this.TSB1TD1Unit2 = calculator.TSB1TD1Unit2;
            this.TSB1TD2Amount2 = calculator.TSB1TD2Amount2;
            this.TSB1TD2Unit2 = calculator.TSB1TD2Unit2;
            this.TSB1MathResult2 = calculator.TSB1MathResult2;
            this.TSB1MathSubType2 = calculator.TSB1MathSubType2;

            this.TSB1TMAmount2 = calculator.TSB1TMAmount2;
            this.TSB1TMUnit2 = calculator.TSB1TMUnit2;
            this.TSB1TLAmount2 = calculator.TSB1TLAmount2;
            this.TSB1TLUnit2 = calculator.TSB1TLUnit2;
            this.TSB1TUAmount2 = calculator.TSB1TUAmount2;
            this.TSB1TUUnit2 = calculator.TSB1TUUnit2;
            this.TSB1MathOperator2 = calculator.TSB1MathOperator2;
            this.TSB1MathExpression2 = calculator.TSB1MathExpression2;
            this.TSB1N2 = calculator.TSB1N2;
            this.TSB1Date2 = calculator.TSB1Date2;
            this.TSB1MathType2 = calculator.TSB1MathType2;
            this.TSB1BaseIO2 = calculator.TSB1BaseIO2;
            this.TSB11Amount2 = calculator.TSB11Amount2;
            this.TSB11Unit2 = calculator.TSB11Unit2;
            this.TSB12Amount2 = calculator.TSB12Amount2;
            this.TSB12Unit2 = calculator.TSB12Unit2;
            this.TSB15Amount2 = calculator.TSB15Amount2;
            this.TSB15Unit2 = calculator.TSB15Unit2;
            this.TSB13Amount2 = calculator.TSB13Amount2;
            this.TSB13Unit2 = calculator.TSB13Unit2;
            this.TSB14Amount2 = calculator.TSB14Amount2;
            this.TSB14Unit2 = calculator.TSB14Unit2;

            this.TSB1Description3 = calculator.TSB1Description3;
            this.TSB1Name3 = calculator.TSB1Name3;
            this.TSB1Label3 = calculator.TSB1Label3;
            this.TSB1Type3 = calculator.TSB1Type3;
            this.TSB1RelLabel3 = calculator.TSB1RelLabel3;
            this.TSB1TAmount3 = calculator.TSB1TAmount3;
            this.TSB1TUnit3 = calculator.TSB1TUnit3;
            this.TSB1TD1Amount3 = calculator.TSB1TD1Amount3;
            this.TSB1TD1Unit3 = calculator.TSB1TD1Unit3;
            this.TSB1TD2Amount3 = calculator.TSB1TD2Amount3;
            this.TSB1TD2Unit3 = calculator.TSB1TD2Unit3;
            this.TSB1MathResult3 = calculator.TSB1MathResult3;
            this.TSB1MathSubType3 = calculator.TSB1MathSubType3;

            this.TSB1TMAmount3 = calculator.TSB1TMAmount3;
            this.TSB1TMUnit3 = calculator.TSB1TMUnit3;
            this.TSB1TLAmount3 = calculator.TSB1TLAmount3;
            this.TSB1TLUnit3 = calculator.TSB1TLUnit3;
            this.TSB1TUAmount3 = calculator.TSB1TUAmount3;
            this.TSB1TUUnit3 = calculator.TSB1TUUnit3;
            this.TSB1MathOperator3 = calculator.TSB1MathOperator3;
            this.TSB1MathExpression3 = calculator.TSB1MathExpression3;
            this.TSB1N3 = calculator.TSB1N3;
            this.TSB1Date3 = calculator.TSB1Date3;
            this.TSB1MathType3 = calculator.TSB1MathType3;
            this.TSB1BaseIO3 = calculator.TSB1BaseIO3;
            this.TSB11Amount3 = calculator.TSB11Amount3;
            this.TSB11Unit3 = calculator.TSB11Unit3;
            this.TSB12Amount3 = calculator.TSB12Amount3;
            this.TSB12Unit3 = calculator.TSB12Unit3;
            this.TSB15Amount3 = calculator.TSB15Amount3;
            this.TSB15Unit3 = calculator.TSB15Unit3;
            this.TSB13Amount3 = calculator.TSB13Amount3;
            this.TSB13Unit3 = calculator.TSB13Unit3;
            this.TSB14Amount3 = calculator.TSB14Amount3;
            this.TSB14Unit3 = calculator.TSB14Unit3;

            this.TSB1Description4 = calculator.TSB1Description4;
            this.TSB1Name4 = calculator.TSB1Name4;
            this.TSB1Label4 = calculator.TSB1Label4;
            this.TSB1Type4 = calculator.TSB1Type4;
            this.TSB1RelLabel4 = calculator.TSB1RelLabel4;
            this.TSB1TAmount4 = calculator.TSB1TAmount4;
            this.TSB1TUnit4 = calculator.TSB1TUnit4;
            this.TSB1TD1Amount4 = calculator.TSB1TD1Amount4;
            this.TSB1TD1Unit4 = calculator.TSB1TD1Unit4;
            this.TSB1TD2Amount4 = calculator.TSB1TD2Amount4;
            this.TSB1TD2Unit4 = calculator.TSB1TD2Unit4;
            this.TSB1MathResult4 = calculator.TSB1MathResult4;
            this.TSB1MathSubType4 = calculator.TSB1MathSubType4;

            this.TSB1TMAmount4 = calculator.TSB1TMAmount4;
            this.TSB1TMUnit4 = calculator.TSB1TMUnit4;
            this.TSB1TLAmount4 = calculator.TSB1TLAmount4;
            this.TSB1TLUnit4 = calculator.TSB1TLUnit4;
            this.TSB1TUAmount4 = calculator.TSB1TUAmount4;
            this.TSB1TUUnit4 = calculator.TSB1TUUnit4;
            this.TSB1MathOperator4 = calculator.TSB1MathOperator4;
            this.TSB1MathExpression4 = calculator.TSB1MathExpression4;
            this.TSB1N4 = calculator.TSB1N4;
            this.TSB1Date4 = calculator.TSB1Date4;
            this.TSB1MathType4 = calculator.TSB1MathType4;
            this.TSB1BaseIO4 = calculator.TSB1BaseIO4;
            this.TSB11Amount4 = calculator.TSB11Amount4;
            this.TSB11Unit4 = calculator.TSB11Unit4;
            this.TSB12Amount4 = calculator.TSB12Amount4;
            this.TSB12Unit4 = calculator.TSB12Unit4;
            this.TSB15Amount4 = calculator.TSB15Amount4;
            this.TSB15Unit4 = calculator.TSB15Unit4;
            this.TSB13Amount4 = calculator.TSB13Amount4;
            this.TSB13Unit4 = calculator.TSB13Unit4;
            this.TSB14Amount4 = calculator.TSB14Amount4;
            this.TSB14Unit4 = calculator.TSB14Unit4;

            this.TSB1Description5 = calculator.TSB1Description5;
            this.TSB1Name5 = calculator.TSB1Name5;
            this.TSB1Label5 = calculator.TSB1Label5;
            this.TSB1Type5 = calculator.TSB1Type5;
            this.TSB1RelLabel5 = calculator.TSB1RelLabel5;
            this.TSB1TAmount5 = calculator.TSB1TAmount5;
            this.TSB1TUnit5 = calculator.TSB1TUnit5;
            this.TSB1TD1Amount5 = calculator.TSB1TD1Amount5;
            this.TSB1TD1Unit5 = calculator.TSB1TD1Unit5;
            this.TSB1TD2Amount5 = calculator.TSB1TD2Amount5;
            this.TSB1TD2Unit5 = calculator.TSB1TD2Unit5;
            this.TSB1MathResult5 = calculator.TSB1MathResult5;
            this.TSB1MathSubType5 = calculator.TSB1MathSubType5;

            this.TSB1TMAmount5 = calculator.TSB1TMAmount5;
            this.TSB1TMUnit5 = calculator.TSB1TMUnit5;
            this.TSB1TLAmount5 = calculator.TSB1TLAmount5;
            this.TSB1TLUnit5 = calculator.TSB1TLUnit5;
            this.TSB1TUAmount5 = calculator.TSB1TUAmount5;
            this.TSB1TUUnit5 = calculator.TSB1TUUnit5;
            this.TSB1MathOperator5 = calculator.TSB1MathOperator5;
            this.TSB1MathExpression5 = calculator.TSB1MathExpression5;
            this.TSB1N5 = calculator.TSB1N5;
            this.TSB1Date5 = calculator.TSB1Date5;
            this.TSB1MathType5 = calculator.TSB1MathType5;
            this.TSB1BaseIO5 = calculator.TSB1BaseIO5;
            this.TSB11Amount5 = calculator.TSB11Amount5;
            this.TSB11Unit5 = calculator.TSB11Unit5;
            this.TSB12Amount5 = calculator.TSB12Amount5;
            this.TSB12Unit5 = calculator.TSB12Unit5;
            this.TSB15Amount5 = calculator.TSB15Amount5;
            this.TSB15Unit5 = calculator.TSB15Unit5;
            this.TSB13Amount5 = calculator.TSB13Amount5;
            this.TSB13Unit5 = calculator.TSB13Unit5;
            this.TSB14Amount5 = calculator.TSB14Amount5;
            this.TSB14Unit5 = calculator.TSB14Unit5;

            this.TSB1Description6 = calculator.TSB1Description6;
            this.TSB1Name6 = calculator.TSB1Name6;
            this.TSB1Label6 = calculator.TSB1Label6;
            this.TSB1Type6 = calculator.TSB1Type6;
            this.TSB1RelLabel6 = calculator.TSB1RelLabel6;
            this.TSB1TAmount6 = calculator.TSB1TAmount6;
            this.TSB1TUnit6 = calculator.TSB1TUnit6;
            this.TSB1TD1Amount6 = calculator.TSB1TD1Amount6;
            this.TSB1TD1Unit6 = calculator.TSB1TD1Unit6;
            this.TSB1TD2Amount6 = calculator.TSB1TD2Amount6;
            this.TSB1TD2Unit6 = calculator.TSB1TD2Unit6;
            this.TSB1MathResult6 = calculator.TSB1MathResult6;
            this.TSB1MathSubType6 = calculator.TSB1MathSubType6;

            this.TSB1TMAmount6 = calculator.TSB1TMAmount6;
            this.TSB1TMUnit6 = calculator.TSB1TMUnit6;
            this.TSB1TLAmount6 = calculator.TSB1TLAmount6;
            this.TSB1TLUnit6 = calculator.TSB1TLUnit6;
            this.TSB1TUAmount6 = calculator.TSB1TUAmount6;
            this.TSB1TUUnit6 = calculator.TSB1TUUnit6;
            this.TSB1MathOperator6 = calculator.TSB1MathOperator6;
            this.TSB1MathExpression6 = calculator.TSB1MathExpression6;
            this.TSB1N6 = calculator.TSB1N6;
            this.TSB1Date6 = calculator.TSB1Date6;
            this.TSB1MathType6 = calculator.TSB1MathType6;
            this.TSB1BaseIO6 = calculator.TSB1BaseIO6;
            this.TSB11Amount6 = calculator.TSB11Amount6;
            this.TSB11Unit6 = calculator.TSB11Unit6;
            this.TSB12Amount6 = calculator.TSB12Amount6;
            this.TSB12Unit6 = calculator.TSB12Unit6;
            this.TSB15Amount6 = calculator.TSB15Amount6;
            this.TSB15Unit6 = calculator.TSB15Unit6;
            this.TSB13Amount6 = calculator.TSB13Amount6;
            this.TSB13Unit6 = calculator.TSB13Unit6;
            this.TSB14Amount6 = calculator.TSB14Amount6;
            this.TSB14Unit6 = calculator.TSB14Unit6;

            this.TSB1Description7 = calculator.TSB1Description7;
            this.TSB1Name7 = calculator.TSB1Name7;
            this.TSB1Label7 = calculator.TSB1Label7;
            this.TSB1Type7 = calculator.TSB1Type7;
            this.TSB1RelLabel7 = calculator.TSB1RelLabel7;
            this.TSB1TAmount7 = calculator.TSB1TAmount7;
            this.TSB1TUnit7 = calculator.TSB1TUnit7;
            this.TSB1TD1Amount7 = calculator.TSB1TD1Amount7;
            this.TSB1TD1Unit7 = calculator.TSB1TD1Unit7;
            this.TSB1TD2Amount7 = calculator.TSB1TD2Amount7;
            this.TSB1TD2Unit7 = calculator.TSB1TD2Unit7;
            this.TSB1MathResult7 = calculator.TSB1MathResult7;
            this.TSB1MathSubType7 = calculator.TSB1MathSubType7;

            this.TSB1TMAmount7 = calculator.TSB1TMAmount7;
            this.TSB1TMUnit7 = calculator.TSB1TMUnit7;
            this.TSB1TLAmount7 = calculator.TSB1TLAmount7;
            this.TSB1TLUnit7 = calculator.TSB1TLUnit7;
            this.TSB1TUAmount7 = calculator.TSB1TUAmount7;
            this.TSB1TUUnit7 = calculator.TSB1TUUnit7;
            this.TSB1MathOperator7 = calculator.TSB1MathOperator7;
            this.TSB1MathExpression7 = calculator.TSB1MathExpression7;
            this.TSB1N7 = calculator.TSB1N7;
            this.TSB1Date7 = calculator.TSB1Date7;
            this.TSB1MathType7 = calculator.TSB1MathType7;
            this.TSB1BaseIO7 = calculator.TSB1BaseIO7;
            this.TSB11Amount7 = calculator.TSB11Amount7;
            this.TSB11Unit7 = calculator.TSB11Unit7;
            this.TSB12Amount7 = calculator.TSB12Amount7;
            this.TSB12Unit7 = calculator.TSB12Unit7;
            this.TSB15Amount7 = calculator.TSB15Amount7;
            this.TSB15Unit7 = calculator.TSB15Unit7;
            this.TSB13Amount7 = calculator.TSB13Amount7;
            this.TSB13Unit7 = calculator.TSB13Unit7;
            this.TSB14Amount7 = calculator.TSB14Amount7;
            this.TSB14Unit7 = calculator.TSB14Unit7;

            this.TSB1Description8 = calculator.TSB1Description8;
            this.TSB1Name8 = calculator.TSB1Name8;
            this.TSB1Label8 = calculator.TSB1Label8;
            this.TSB1Type8 = calculator.TSB1Type8;
            this.TSB1RelLabel8 = calculator.TSB1RelLabel8;
            this.TSB1TAmount8 = calculator.TSB1TAmount8;
            this.TSB1TUnit8 = calculator.TSB1TUnit8;
            this.TSB1TD1Amount8 = calculator.TSB1TD1Amount8;
            this.TSB1TD1Unit8 = calculator.TSB1TD1Unit8;
            this.TSB1TD2Amount8 = calculator.TSB1TD2Amount8;
            this.TSB1TD2Unit8 = calculator.TSB1TD2Unit8;
            this.TSB1MathResult8 = calculator.TSB1MathResult8;
            this.TSB1MathSubType8 = calculator.TSB1MathSubType8;

            this.TSB1TMAmount8 = calculator.TSB1TMAmount8;
            this.TSB1TMUnit8 = calculator.TSB1TMUnit8;
            this.TSB1TLAmount8 = calculator.TSB1TLAmount8;
            this.TSB1TLUnit8 = calculator.TSB1TLUnit8;
            this.TSB1TUAmount8 = calculator.TSB1TUAmount8;
            this.TSB1TUUnit8 = calculator.TSB1TUUnit8;
            this.TSB1MathOperator8 = calculator.TSB1MathOperator8;
            this.TSB1MathExpression8 = calculator.TSB1MathExpression8;
            this.TSB1N8 = calculator.TSB1N8;
            this.TSB1Date8 = calculator.TSB1Date8;
            this.TSB1MathType8 = calculator.TSB1MathType8;
            this.TSB1BaseIO8 = calculator.TSB1BaseIO8;
            this.TSB11Amount8 = calculator.TSB11Amount8;
            this.TSB11Unit8 = calculator.TSB11Unit8;
            this.TSB12Amount8 = calculator.TSB12Amount8;
            this.TSB12Unit8 = calculator.TSB12Unit8;
            this.TSB15Amount8 = calculator.TSB15Amount8;
            this.TSB15Unit8 = calculator.TSB15Unit8;
            this.TSB13Amount8 = calculator.TSB13Amount8;
            this.TSB13Unit8 = calculator.TSB13Unit8;
            this.TSB14Amount8 = calculator.TSB14Amount8;
            this.TSB14Unit8 = calculator.TSB14Unit8;

            this.TSB1Description9 = calculator.TSB1Description9;
            this.TSB1Name9 = calculator.TSB1Name9;
            this.TSB1Label9 = calculator.TSB1Label9;
            this.TSB1Type9 = calculator.TSB1Type9;
            this.TSB1RelLabel9 = calculator.TSB1RelLabel9;
            this.TSB1TAmount9 = calculator.TSB1TAmount9;
            this.TSB1TUnit9 = calculator.TSB1TUnit9;
            this.TSB1TD1Amount9 = calculator.TSB1TD1Amount9;
            this.TSB1TD1Unit9 = calculator.TSB1TD1Unit9;
            this.TSB1TD2Amount9 = calculator.TSB1TD2Amount9;
            this.TSB1TD2Unit9 = calculator.TSB1TD2Unit9;
            this.TSB1MathResult9 = calculator.TSB1MathResult9;
            this.TSB1MathSubType9 = calculator.TSB1MathSubType9;

            this.TSB1TMAmount9 = calculator.TSB1TMAmount9;
            this.TSB1TMUnit9 = calculator.TSB1TMUnit9;
            this.TSB1TLAmount9 = calculator.TSB1TLAmount9;
            this.TSB1TLUnit9 = calculator.TSB1TLUnit9;
            this.TSB1TUAmount9 = calculator.TSB1TUAmount9;
            this.TSB1TUUnit9 = calculator.TSB1TUUnit9;
            this.TSB1MathOperator9 = calculator.TSB1MathOperator9;
            this.TSB1MathExpression9 = calculator.TSB1MathExpression9;
            this.TSB1N9 = calculator.TSB1N9;
            this.TSB1Date9 = calculator.TSB1Date9;
            this.TSB1MathType9 = calculator.TSB1MathType9;
            this.TSB1BaseIO9 = calculator.TSB1BaseIO9;
            this.TSB11Amount9 = calculator.TSB11Amount9;
            this.TSB11Unit9 = calculator.TSB11Unit9;
            this.TSB12Amount9 = calculator.TSB12Amount9;
            this.TSB12Unit9 = calculator.TSB12Unit9;
            this.TSB15Amount9 = calculator.TSB15Amount9;
            this.TSB15Unit9 = calculator.TSB15Unit9;
            this.TSB13Amount9 = calculator.TSB13Amount9;
            this.TSB13Unit9 = calculator.TSB13Unit9;
            this.TSB14Amount9 = calculator.TSB14Amount9;
            this.TSB14Unit9 = calculator.TSB14Unit9;

            this.TSB1Description10 = calculator.TSB1Description10;
            this.TSB1Name10 = calculator.TSB1Name10;
            this.TSB1Label10 = calculator.TSB1Label10;
            this.TSB1Type10 = calculator.TSB1Type10;
            this.TSB1RelLabel10 = calculator.TSB1RelLabel10;
            this.TSB1TAmount10 = calculator.TSB1TAmount10;
            this.TSB1TUnit10 = calculator.TSB1TUnit10;
            this.TSB1TD1Amount10 = calculator.TSB1TD1Amount10;
            this.TSB1TD1Unit10 = calculator.TSB1TD1Unit10;
            this.TSB1TD2Amount10 = calculator.TSB1TD2Amount10;
            this.TSB1TD2Unit10 = calculator.TSB1TD2Unit10;
            this.TSB1MathResult10 = calculator.TSB1MathResult10;
            this.TSB1MathSubType10 = calculator.TSB1MathSubType10;

            this.TSB1TMAmount10 = calculator.TSB1TMAmount10;
            this.TSB1TMUnit10 = calculator.TSB1TMUnit10;
            this.TSB1TLAmount10 = calculator.TSB1TLAmount10;
            this.TSB1TLUnit10 = calculator.TSB1TLUnit10;
            this.TSB1TUAmount10 = calculator.TSB1TUAmount10;
            this.TSB1TUUnit10 = calculator.TSB1TUUnit10;
            this.TSB1MathOperator10 = calculator.TSB1MathOperator10;
            this.TSB1MathExpression10 = calculator.TSB1MathExpression10;
            this.TSB1N10 = calculator.TSB1N10;
            this.TSB1Date10 = calculator.TSB1Date10;
            this.TSB1MathType10 = calculator.TSB1MathType10;
            this.TSB1BaseIO10 = calculator.TSB1BaseIO10;
            this.TSB11Amount10 = calculator.TSB11Amount10;
            this.TSB11Unit10 = calculator.TSB11Unit10;
            this.TSB12Amount10 = calculator.TSB12Amount10;
            this.TSB12Unit10 = calculator.TSB12Unit10;
            this.TSB15Amount10 = calculator.TSB15Amount10;
            this.TSB15Unit10 = calculator.TSB15Unit10;
            this.TSB13Amount10 = calculator.TSB13Amount10;
            this.TSB13Unit10 = calculator.TSB13Unit10;
            this.TSB14Amount10 = calculator.TSB14Amount10;
            this.TSB14Unit10 = calculator.TSB14Unit10;

            this.TSB1Description11 = calculator.TSB1Description11;
            this.TSB1Name11 = calculator.TSB1Name11;
            this.TSB1Label11 = calculator.TSB1Label11;
            this.TSB1Type11 = calculator.TSB1Type11;
            this.TSB1RelLabel11 = calculator.TSB1RelLabel11;
            this.TSB1TAmount11 = calculator.TSB1TAmount11;
            this.TSB1TUnit11 = calculator.TSB1TUnit11;
            this.TSB1TD1Amount11 = calculator.TSB1TD1Amount11;
            this.TSB1TD1Unit11 = calculator.TSB1TD1Unit11;
            this.TSB1TD2Amount11 = calculator.TSB1TD2Amount11;
            this.TSB1TD2Unit11 = calculator.TSB1TD2Unit11;
            this.TSB1MathResult11 = calculator.TSB1MathResult11;
            this.TSB1MathSubType11 = calculator.TSB1MathSubType11;

            this.TSB1TMAmount11 = calculator.TSB1TMAmount11;
            this.TSB1TMUnit11 = calculator.TSB1TMUnit11;
            this.TSB1TLAmount11 = calculator.TSB1TLAmount11;
            this.TSB1TLUnit11 = calculator.TSB1TLUnit11;
            this.TSB1TUAmount11 = calculator.TSB1TUAmount11;
            this.TSB1TUUnit11 = calculator.TSB1TUUnit11;
            this.TSB1MathOperator11 = calculator.TSB1MathOperator11;
            this.TSB1MathExpression11 = calculator.TSB1MathExpression11;
            this.TSB1N11 = calculator.TSB1N11;
            this.TSB1Date11 = calculator.TSB1Date11;
            this.TSB1MathType11 = calculator.TSB1MathType11;
            this.TSB1BaseIO11 = calculator.TSB1BaseIO11;
            this.TSB11Amount11 = calculator.TSB11Amount11;
            this.TSB11Unit11 = calculator.TSB11Unit11;
            this.TSB12Amount11 = calculator.TSB12Amount11;
            this.TSB12Unit11 = calculator.TSB12Unit11;
            this.TSB15Amount11 = calculator.TSB15Amount11;
            this.TSB15Unit11 = calculator.TSB15Unit11;
            this.TSB13Amount11 = calculator.TSB13Amount11;
            this.TSB13Unit11 = calculator.TSB13Unit11;
            this.TSB14Amount11 = calculator.TSB14Amount11;
            this.TSB14Unit11 = calculator.TSB14Unit11;

            this.TSB1Description12 = calculator.TSB1Description12;
            this.TSB1Name12 = calculator.TSB1Name12;
            this.TSB1Label12 = calculator.TSB1Label12;
            this.TSB1Type12 = calculator.TSB1Type12;
            this.TSB1RelLabel12 = calculator.TSB1RelLabel12;
            this.TSB1TAmount12 = calculator.TSB1TAmount12;
            this.TSB1TUnit12 = calculator.TSB1TUnit12;
            this.TSB1TD1Amount12 = calculator.TSB1TD1Amount12;
            this.TSB1TD1Unit12 = calculator.TSB1TD1Unit12;
            this.TSB1TD2Amount12 = calculator.TSB1TD2Amount12;
            this.TSB1TD2Unit12 = calculator.TSB1TD2Unit12;
            this.TSB1MathResult12 = calculator.TSB1MathResult12;
            this.TSB1MathSubType12 = calculator.TSB1MathSubType12;

            this.TSB1TMAmount12 = calculator.TSB1TMAmount12;
            this.TSB1TMUnit12 = calculator.TSB1TMUnit12;
            this.TSB1TLAmount12 = calculator.TSB1TLAmount12;
            this.TSB1TLUnit12 = calculator.TSB1TLUnit12;
            this.TSB1TUAmount12 = calculator.TSB1TUAmount12;
            this.TSB1TUUnit12 = calculator.TSB1TUUnit12;
            this.TSB1MathOperator12 = calculator.TSB1MathOperator12;
            this.TSB1MathExpression12 = calculator.TSB1MathExpression12;
            this.TSB1N12 = calculator.TSB1N12;
            this.TSB1Date12 = calculator.TSB1Date12;
            this.TSB1MathType12 = calculator.TSB1MathType12;
            this.TSB1BaseIO12 = calculator.TSB1BaseIO12;
            this.TSB11Amount12 = calculator.TSB11Amount12;
            this.TSB11Unit12 = calculator.TSB11Unit12;
            this.TSB12Amount12 = calculator.TSB12Amount12;
            this.TSB12Unit12 = calculator.TSB12Unit12;
            this.TSB15Amount12 = calculator.TSB15Amount12;
            this.TSB15Unit12 = calculator.TSB15Unit12;
            this.TSB13Amount12 = calculator.TSB13Amount12;
            this.TSB13Unit12 = calculator.TSB13Unit12;
            this.TSB14Amount12 = calculator.TSB14Amount12;
            this.TSB14Unit12 = calculator.TSB14Unit12;

            this.TSB1Description13 = calculator.TSB1Description13;
            this.TSB1Name13 = calculator.TSB1Name13;
            this.TSB1Label13 = calculator.TSB1Label13;
            this.TSB1Type13 = calculator.TSB1Type13;
            this.TSB1RelLabel13 = calculator.TSB1RelLabel13;
            this.TSB1TAmount13 = calculator.TSB1TAmount13;
            this.TSB1TUnit13 = calculator.TSB1TUnit13;
            this.TSB1TD1Amount13 = calculator.TSB1TD1Amount13;
            this.TSB1TD1Unit13 = calculator.TSB1TD1Unit13;
            this.TSB1TD2Amount13 = calculator.TSB1TD2Amount13;
            this.TSB1TD2Unit13 = calculator.TSB1TD2Unit13;
            this.TSB1MathResult13 = calculator.TSB1MathResult13;
            this.TSB1MathSubType13 = calculator.TSB1MathSubType13;

            this.TSB1TMAmount13 = calculator.TSB1TMAmount13;
            this.TSB1TMUnit13 = calculator.TSB1TMUnit13;
            this.TSB1TLAmount13 = calculator.TSB1TLAmount13;
            this.TSB1TLUnit13 = calculator.TSB1TLUnit13;
            this.TSB1TUAmount13 = calculator.TSB1TUAmount13;
            this.TSB1TUUnit13 = calculator.TSB1TUUnit13;
            this.TSB1MathOperator13 = calculator.TSB1MathOperator13;
            this.TSB1MathExpression13 = calculator.TSB1MathExpression13;
            this.TSB1N13 = calculator.TSB1N13;
            this.TSB1Date13 = calculator.TSB1Date13;
            this.TSB1MathType13 = calculator.TSB1MathType13;
            this.TSB1BaseIO13 = calculator.TSB1BaseIO13;
            this.TSB11Amount13 = calculator.TSB11Amount13;
            this.TSB11Unit13 = calculator.TSB11Unit13;
            this.TSB12Amount13 = calculator.TSB12Amount13;
            this.TSB12Unit13 = calculator.TSB12Unit13;
            this.TSB15Amount13 = calculator.TSB15Amount13;
            this.TSB15Unit13 = calculator.TSB15Unit13;
            this.TSB13Amount13 = calculator.TSB13Amount13;
            this.TSB13Unit13 = calculator.TSB13Unit13;
            this.TSB14Amount13 = calculator.TSB14Amount13;
            this.TSB14Unit13 = calculator.TSB14Unit13;

            this.TSB1Description14 = calculator.TSB1Description14;
            this.TSB1Name14 = calculator.TSB1Name14;
            this.TSB1Label14 = calculator.TSB1Label14;
            this.TSB1Type14 = calculator.TSB1Type14;
            this.TSB1RelLabel14 = calculator.TSB1RelLabel14;
            this.TSB1TAmount14 = calculator.TSB1TAmount14;
            this.TSB1TUnit14 = calculator.TSB1TUnit14;
            this.TSB1TD1Amount14 = calculator.TSB1TD1Amount14;
            this.TSB1TD1Unit14 = calculator.TSB1TD1Unit14;
            this.TSB1TD2Amount14 = calculator.TSB1TD2Amount14;
            this.TSB1TD2Unit14 = calculator.TSB1TD2Unit14;
            this.TSB1MathResult14 = calculator.TSB1MathResult14;
            this.TSB1MathSubType14 = calculator.TSB1MathSubType14;

            this.TSB1TMAmount14 = calculator.TSB1TMAmount14;
            this.TSB1TMUnit14 = calculator.TSB1TMUnit14;
            this.TSB1TLAmount14 = calculator.TSB1TLAmount14;
            this.TSB1TLUnit14 = calculator.TSB1TLUnit14;
            this.TSB1TUAmount14 = calculator.TSB1TUAmount14;
            this.TSB1TUUnit14 = calculator.TSB1TUUnit14;
            this.TSB1MathOperator14 = calculator.TSB1MathOperator14;
            this.TSB1MathExpression14 = calculator.TSB1MathExpression14;
            this.TSB1N14 = calculator.TSB1N14;
            this.TSB1Date14 = calculator.TSB1Date14;
            this.TSB1MathType14 = calculator.TSB1MathType14;
            this.TSB1BaseIO14 = calculator.TSB1BaseIO14;
            this.TSB11Amount14 = calculator.TSB11Amount14;
            this.TSB11Unit14 = calculator.TSB11Unit14;
            this.TSB12Amount14 = calculator.TSB12Amount14;
            this.TSB12Unit14 = calculator.TSB12Unit14;
            this.TSB15Amount14 = calculator.TSB15Amount14;
            this.TSB15Unit14 = calculator.TSB15Unit14;
            this.TSB13Amount14 = calculator.TSB13Amount14;
            this.TSB13Unit14 = calculator.TSB13Unit14;
            this.TSB14Amount14 = calculator.TSB14Amount14;
            this.TSB14Unit14 = calculator.TSB14Unit14;

            this.TSB1Description15 = calculator.TSB1Description15;
            this.TSB1Name15 = calculator.TSB1Name15;
            this.TSB1Label15 = calculator.TSB1Label15;
            this.TSB1Type15 = calculator.TSB1Type15;
            this.TSB1RelLabel15 = calculator.TSB1RelLabel15;
            this.TSB1TAmount15 = calculator.TSB1TAmount15;
            this.TSB1TUnit15 = calculator.TSB1TUnit15;
            this.TSB1TD1Amount15 = calculator.TSB1TD1Amount15;
            this.TSB1TD1Unit15 = calculator.TSB1TD1Unit15;
            this.TSB1TD2Amount15 = calculator.TSB1TD2Amount15;
            this.TSB1TD2Unit15 = calculator.TSB1TD2Unit15;
            this.TSB1MathResult15 = calculator.TSB1MathResult15;
            this.TSB1MathSubType15 = calculator.TSB1MathSubType15;

            this.TSB1TMAmount15 = calculator.TSB1TMAmount15;
            this.TSB1TMUnit15 = calculator.TSB1TMUnit15;
            this.TSB1TLAmount15 = calculator.TSB1TLAmount15;
            this.TSB1TLUnit15 = calculator.TSB1TLUnit15;
            this.TSB1TUAmount15 = calculator.TSB1TUAmount15;
            this.TSB1TUUnit15 = calculator.TSB1TUUnit15;
            this.TSB1MathOperator15 = calculator.TSB1MathOperator15;
            this.TSB1MathExpression15 = calculator.TSB1MathExpression15;
            this.TSB1N15 = calculator.TSB1N15;
            this.TSB1Date15 = calculator.TSB1Date15;
            this.TSB1MathType15 = calculator.TSB1MathType15;
            this.TSB1BaseIO15 = calculator.TSB1BaseIO15;
            this.TSB11Amount15 = calculator.TSB11Amount15;
            this.TSB11Unit15 = calculator.TSB11Unit15;
            this.TSB12Amount15 = calculator.TSB12Amount15;
            this.TSB12Unit15 = calculator.TSB12Unit15;
            this.TSB15Amount15 = calculator.TSB15Amount15;
            this.TSB15Unit15 = calculator.TSB15Unit15;
            this.TSB13Amount15 = calculator.TSB13Amount15;
            this.TSB13Unit15 = calculator.TSB13Unit15;
            this.TSB14Amount15 = calculator.TSB14Amount15;
            this.TSB14Unit15 = calculator.TSB14Unit15;

            this.TSB1Description16 = calculator.TSB1Description16;
            this.TSB1Name16 = calculator.TSB1Name16;
            this.TSB1Label16 = calculator.TSB1Label16;
            this.TSB1Type16 = calculator.TSB1Type16;
            this.TSB1RelLabel16 = calculator.TSB1RelLabel16;
            this.TSB1TAmount16 = calculator.TSB1TAmount16;
            this.TSB1TUnit16 = calculator.TSB1TUnit16;
            this.TSB1TD1Amount16 = calculator.TSB1TD1Amount16;
            this.TSB1TD1Unit16 = calculator.TSB1TD1Unit16;
            this.TSB1TD2Amount16 = calculator.TSB1TD2Amount16;
            this.TSB1TD2Unit16 = calculator.TSB1TD2Unit16;
            this.TSB1MathResult16 = calculator.TSB1MathResult16;
            this.TSB1MathSubType16 = calculator.TSB1MathSubType16;

            this.TSB1TMAmount16 = calculator.TSB1TMAmount16;
            this.TSB1TMUnit16 = calculator.TSB1TMUnit16;
            this.TSB1TLAmount16 = calculator.TSB1TLAmount16;
            this.TSB1TLUnit16 = calculator.TSB1TLUnit16;
            this.TSB1TUAmount16 = calculator.TSB1TUAmount16;
            this.TSB1TUUnit16 = calculator.TSB1TUUnit16;
            this.TSB1MathOperator16 = calculator.TSB1MathOperator16;
            this.TSB1MathExpression16 = calculator.TSB1MathExpression16;
            this.TSB1N16 = calculator.TSB1N16;
            this.TSB1Date16 = calculator.TSB1Date16;
            this.TSB1MathType16 = calculator.TSB1MathType16;
            this.TSB1BaseIO16 = calculator.TSB1BaseIO16;
            this.TSB11Amount16 = calculator.TSB11Amount16;
            this.TSB11Unit16 = calculator.TSB11Unit16;
            this.TSB12Amount16 = calculator.TSB12Amount16;
            this.TSB12Unit16 = calculator.TSB12Unit16;
            this.TSB15Amount16 = calculator.TSB15Amount16;
            this.TSB15Unit16 = calculator.TSB15Unit16;
            this.TSB13Amount16 = calculator.TSB13Amount16;
            this.TSB13Unit16 = calculator.TSB13Unit16;
            this.TSB14Amount16 = calculator.TSB14Amount16;
            this.TSB14Unit16 = calculator.TSB14Unit16;

            this.TSB1Description17 = calculator.TSB1Description17;
            this.TSB1Name17 = calculator.TSB1Name17;
            this.TSB1Label17 = calculator.TSB1Label17;
            this.TSB1Type17 = calculator.TSB1Type17;
            this.TSB1RelLabel17 = calculator.TSB1RelLabel17;
            this.TSB1TAmount17 = calculator.TSB1TAmount17;
            this.TSB1TUnit17 = calculator.TSB1TUnit17;
            this.TSB1TD1Amount17 = calculator.TSB1TD1Amount17;
            this.TSB1TD1Unit17 = calculator.TSB1TD1Unit17;
            this.TSB1TD2Amount17 = calculator.TSB1TD2Amount17;
            this.TSB1TD2Unit17 = calculator.TSB1TD2Unit17;
            this.TSB1MathResult17 = calculator.TSB1MathResult17;
            this.TSB1MathSubType17 = calculator.TSB1MathSubType17;

            this.TSB1TMAmount17 = calculator.TSB1TMAmount17;
            this.TSB1TMUnit17 = calculator.TSB1TMUnit17;
            this.TSB1TLAmount17 = calculator.TSB1TLAmount17;
            this.TSB1TLUnit17 = calculator.TSB1TLUnit17;
            this.TSB1TUAmount17 = calculator.TSB1TUAmount17;
            this.TSB1TUUnit17 = calculator.TSB1TUUnit17;
            this.TSB1MathOperator17 = calculator.TSB1MathOperator17;
            this.TSB1MathExpression17 = calculator.TSB1MathExpression17;
            this.TSB1N17 = calculator.TSB1N17;
            this.TSB1Date17 = calculator.TSB1Date17;
            this.TSB1MathType17 = calculator.TSB1MathType17;
            this.TSB1BaseIO17 = calculator.TSB1BaseIO17;
            this.TSB11Amount17 = calculator.TSB11Amount17;
            this.TSB11Unit17 = calculator.TSB11Unit17;
            this.TSB12Amount17 = calculator.TSB12Amount17;
            this.TSB12Unit17 = calculator.TSB12Unit17;
            this.TSB15Amount17 = calculator.TSB15Amount17;
            this.TSB15Unit17 = calculator.TSB15Unit17;
            this.TSB13Amount17 = calculator.TSB13Amount17;
            this.TSB13Unit17 = calculator.TSB13Unit17;
            this.TSB14Amount17 = calculator.TSB14Amount17;
            this.TSB14Unit17 = calculator.TSB14Unit17;

            this.TSB1Description18 = calculator.TSB1Description18;
            this.TSB1Name18 = calculator.TSB1Name18;
            this.TSB1Label18 = calculator.TSB1Label18;
            this.TSB1Type18 = calculator.TSB1Type18;
            this.TSB1RelLabel18 = calculator.TSB1RelLabel18;
            this.TSB1TAmount18 = calculator.TSB1TAmount18;
            this.TSB1TUnit18 = calculator.TSB1TUnit18;
            this.TSB1TD1Amount18 = calculator.TSB1TD1Amount18;
            this.TSB1TD1Unit18 = calculator.TSB1TD1Unit18;
            this.TSB1TD2Amount18 = calculator.TSB1TD2Amount18;
            this.TSB1TD2Unit18 = calculator.TSB1TD2Unit18;
            this.TSB1MathResult18 = calculator.TSB1MathResult18;
            this.TSB1MathSubType18 = calculator.TSB1MathSubType18;

            this.TSB1TMAmount18 = calculator.TSB1TMAmount18;
            this.TSB1TMUnit18 = calculator.TSB1TMUnit18;
            this.TSB1TLAmount18 = calculator.TSB1TLAmount18;
            this.TSB1TLUnit18 = calculator.TSB1TLUnit18;
            this.TSB1TUAmount18 = calculator.TSB1TUAmount18;
            this.TSB1TUUnit18 = calculator.TSB1TUUnit18;
            this.TSB1MathOperator18 = calculator.TSB1MathOperator18;
            this.TSB1MathExpression18 = calculator.TSB1MathExpression18;
            this.TSB1N18 = calculator.TSB1N18;
            this.TSB1Date18 = calculator.TSB1Date18;
            this.TSB1MathType18 = calculator.TSB1MathType18;
            this.TSB1BaseIO18 = calculator.TSB1BaseIO18;
            this.TSB11Amount18 = calculator.TSB11Amount18;
            this.TSB11Unit18 = calculator.TSB11Unit18;
            this.TSB12Amount18 = calculator.TSB12Amount18;
            this.TSB12Unit18 = calculator.TSB12Unit18;
            this.TSB15Amount18 = calculator.TSB15Amount18;
            this.TSB15Unit18 = calculator.TSB15Unit18;
            this.TSB13Amount18 = calculator.TSB13Amount18;
            this.TSB13Unit18 = calculator.TSB13Unit18;
            this.TSB14Amount18 = calculator.TSB14Amount18;
            this.TSB14Unit18 = calculator.TSB14Unit18;

            this.TSB1Description19 = calculator.TSB1Description19;
            this.TSB1Name19 = calculator.TSB1Name19;
            this.TSB1Label19 = calculator.TSB1Label19;
            this.TSB1Type19 = calculator.TSB1Type19;
            this.TSB1RelLabel19 = calculator.TSB1RelLabel19;
            this.TSB1TAmount19 = calculator.TSB1TAmount19;
            this.TSB1TUnit19 = calculator.TSB1TUnit19;
            this.TSB1TD1Amount19 = calculator.TSB1TD1Amount19;
            this.TSB1TD1Unit19 = calculator.TSB1TD1Unit19;
            this.TSB1TD2Amount19 = calculator.TSB1TD2Amount19;
            this.TSB1TD2Unit19 = calculator.TSB1TD2Unit19;
            this.TSB1MathResult19 = calculator.TSB1MathResult19;
            this.TSB1MathSubType19 = calculator.TSB1MathSubType19;

            this.TSB1TMAmount19 = calculator.TSB1TMAmount19;
            this.TSB1TMUnit19 = calculator.TSB1TMUnit19;
            this.TSB1TLAmount19 = calculator.TSB1TLAmount19;
            this.TSB1TLUnit19 = calculator.TSB1TLUnit19;
            this.TSB1TUAmount19 = calculator.TSB1TUAmount19;
            this.TSB1TUUnit19 = calculator.TSB1TUUnit19;
            this.TSB1MathOperator19 = calculator.TSB1MathOperator19;
            this.TSB1MathExpression19 = calculator.TSB1MathExpression19;
            this.TSB1N19 = calculator.TSB1N19;
            this.TSB1Date19 = calculator.TSB1Date19;
            this.TSB1MathType19 = calculator.TSB1MathType19;
            this.TSB1BaseIO19 = calculator.TSB1BaseIO19;
            this.TSB11Amount19 = calculator.TSB11Amount19;
            this.TSB11Unit19 = calculator.TSB11Unit19;
            this.TSB12Amount19 = calculator.TSB12Amount19;
            this.TSB12Unit19 = calculator.TSB12Unit19;
            this.TSB15Amount19 = calculator.TSB15Amount19;
            this.TSB15Unit19 = calculator.TSB15Unit19;
            this.TSB13Amount19 = calculator.TSB13Amount19;
            this.TSB13Unit19 = calculator.TSB13Unit19;
            this.TSB14Amount19 = calculator.TSB14Amount19;
            this.TSB14Unit19 = calculator.TSB14Unit19;

            this.TSB1Description20 = calculator.TSB1Description20;
            this.TSB1Name20 = calculator.TSB1Name20;
            this.TSB1Label20 = calculator.TSB1Label20;
            this.TSB1Type20 = calculator.TSB1Type20;
            this.TSB1RelLabel20 = calculator.TSB1RelLabel20;
            this.TSB1TAmount20 = calculator.TSB1TAmount20;
            this.TSB1TUnit20 = calculator.TSB1TUnit20;
            this.TSB1TD1Amount20 = calculator.TSB1TD1Amount20;
            this.TSB1TD1Unit20 = calculator.TSB1TD1Unit20;
            this.TSB1TD2Amount20 = calculator.TSB1TD2Amount20;
            this.TSB1TD2Unit20 = calculator.TSB1TD2Unit20;
            this.TSB1MathResult20 = calculator.TSB1MathResult20;
            this.TSB1MathSubType20 = calculator.TSB1MathSubType20;

            this.TSB1TMAmount20 = calculator.TSB1TMAmount20;
            this.TSB1TMUnit20 = calculator.TSB1TMUnit20;
            this.TSB1TLAmount20 = calculator.TSB1TLAmount20;
            this.TSB1TLUnit20 = calculator.TSB1TLUnit20;
            this.TSB1TUAmount20 = calculator.TSB1TUAmount20;
            this.TSB1TUUnit20 = calculator.TSB1TUUnit20;
            this.TSB1MathOperator20 = calculator.TSB1MathOperator20;
            this.TSB1MathExpression20 = calculator.TSB1MathExpression20;
            this.TSB1N20 = calculator.TSB1N20;
            this.TSB1Date20 = calculator.TSB1Date20;
            this.TSB1MathType20 = calculator.TSB1MathType20;
            this.TSB1BaseIO20 = calculator.TSB1BaseIO20;
            this.TSB11Amount20 = calculator.TSB11Amount20;
            this.TSB11Unit20 = calculator.TSB11Unit20;
            this.TSB12Amount20 = calculator.TSB12Amount20;
            this.TSB12Unit20 = calculator.TSB12Unit20;
            this.TSB15Amount20 = calculator.TSB15Amount20;
            this.TSB15Unit20 = calculator.TSB15Unit20;
            this.TSB13Amount20 = calculator.TSB13Amount20;
            this.TSB13Unit20 = calculator.TSB13Unit20;
            this.TSB14Amount20 = calculator.TSB14Amount20;
            this.TSB14Unit20 = calculator.TSB14Unit20;
        }
        //set the class properties using the XElement
        public virtual void SetTSB1BaseStockProperties(string attNameExtension, XElement calculator)
        {
            this.TSB1Score = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1Score, attNameExtension));
            this.TSB1ScoreUnit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreUnit, attNameExtension));
            this.TSB1ScoreD1Amount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1ScoreD1Amount, attNameExtension));
            this.TSB1ScoreD1Unit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreD1Unit, attNameExtension));
            this.TSB1ScoreD2Amount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1ScoreD2Amount, attNameExtension));
            this.TSB1ScoreD2Unit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreD2Unit, attNameExtension));
            this.TSB1ScoreMathExpression = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreMathExpression, attNameExtension));
            this.TSB1ScoreM = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1ScoreM, attNameExtension));
            this.TSB1ScoreMUnit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreMUnit, attNameExtension));
            this.TSB1ScoreLAmount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1ScoreLAmount, attNameExtension));
            this.TSB1ScoreLUnit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreLUnit, attNameExtension));
            this.TSB1ScoreUAmount = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1ScoreUAmount, attNameExtension));
            this.TSB1ScoreUUnit = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreUUnit, attNameExtension));
            this.TSB1Iterations = CalculatorHelpers.GetAttributeInt(calculator,
              string.Concat(cTSB1Iterations, attNameExtension));
            this.TSB1ScoreDistType = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreDistType, attNameExtension));
            this.TSB1ScoreMathType = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreMathType, attNameExtension));
            this.TSB1ScoreMathResult = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreMathResult, attNameExtension));
            this.TSB1ScoreMathSubType = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1ScoreMathSubType, attNameExtension));
            this.TSB1ScoreN = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1ScoreN, attNameExtension));
            this.TSB1Description1 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description1, attNameExtension));
            this.TSB1Name1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name1, attNameExtension));
            this.TSB1Label1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label1, attNameExtension));
            this.TSB1Type1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type1, attNameExtension));
            this.TSB11Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount1, attNameExtension));
            this.TSB11Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit1, attNameExtension));
            this.TSB1RelLabel1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel1, attNameExtension));
            this.TSB1TAmount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount1, attNameExtension));
            this.TSB1TUnit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit1, attNameExtension));
            this.TSB1TD1Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD1Amount1, attNameExtension));
            this.TSB1TD1Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit1, attNameExtension));
            this.TSB1TD2Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount1, attNameExtension));
            this.TSB1TD2Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit1, attNameExtension));
            this.TSB1MathResult1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult1, attNameExtension));
            this.TSB1MathSubType1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType1, attNameExtension));

            this.TSB1TMAmount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TMAmount1, attNameExtension));
            this.TSB1TMUnit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit1, attNameExtension));
            this.TSB1TLAmount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TLAmount1, attNameExtension));
            this.TSB1TLUnit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit1, attNameExtension));
            this.TSB1TUAmount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount1, attNameExtension));
            this.TSB1TUUnit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit1, attNameExtension));
            this.TSB1MathOperator1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator1, attNameExtension));
            this.TSB1MathExpression1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression1, attNameExtension));
            this.TSB1N1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N1, attNameExtension));
            this.TSB1Date1 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date1, attNameExtension));
            this.TSB1MathType1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType1, attNameExtension));
            this.TSB1BaseIO1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO1, attNameExtension));
            this.TSB12Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount1, attNameExtension));
            this.TSB12Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit1, attNameExtension));
            this.TSB15Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount1, attNameExtension));
            this.TSB15Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit1, attNameExtension));
            this.TSB13Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount1, attNameExtension));
            this.TSB13Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit1, attNameExtension));
            this.TSB14Amount1 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount1, attNameExtension));
            this.TSB14Unit1 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit1, attNameExtension));

            this.TSB1Description2 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description2, attNameExtension));
            this.TSB1Name2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name2, attNameExtension));
            this.TSB1Label2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label2, attNameExtension));
            this.TSB1Type2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type2, attNameExtension));
            this.TSB11Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount2, attNameExtension));
            this.TSB11Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit2, attNameExtension));
            this.TSB1RelLabel2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel2, attNameExtension));
            this.TSB1TAmount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount2, attNameExtension));
            this.TSB1TUnit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit2, attNameExtension));
            this.TSB1TD1Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount2, attNameExtension));
            this.TSB1TD1Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit2, attNameExtension));
            this.TSB1TD2Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount2, attNameExtension));
            this.TSB1TD2Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit2, attNameExtension));
            this.TSB1MathResult2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult2, attNameExtension));
            this.TSB1MathSubType2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType2, attNameExtension));

            this.TSB1TMAmount2 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount2, attNameExtension));
            this.TSB1TMUnit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit2, attNameExtension));
            this.TSB1TLAmount2 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount2, attNameExtension));
            this.TSB1TLUnit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit2, attNameExtension));
            this.TSB1TUAmount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount2, attNameExtension));
            this.TSB1TUUnit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit2, attNameExtension));
            this.TSB1MathOperator2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator2, attNameExtension));
            this.TSB1MathExpression2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression2, attNameExtension));
            this.TSB1N2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N2, attNameExtension));
            this.TSB1Date2 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date2, attNameExtension));
            this.TSB1MathType2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType2, attNameExtension));
            this.TSB1BaseIO2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO2, attNameExtension));
            this.TSB12Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount2, attNameExtension));
            this.TSB12Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit2, attNameExtension));
            this.TSB15Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount2, attNameExtension));
            this.TSB15Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit2, attNameExtension));
            this.TSB13Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount2, attNameExtension));
            this.TSB13Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit2, attNameExtension));
            this.TSB14Amount2 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount2, attNameExtension));
            this.TSB14Unit2 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit2, attNameExtension));

            this.TSB1Description3 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description3, attNameExtension));
            this.TSB1Name3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name3, attNameExtension));
            this.TSB1Label3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label3, attNameExtension));
            this.TSB1Type3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type3, attNameExtension));
            this.TSB11Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount3, attNameExtension));
            this.TSB11Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit3, attNameExtension));
            this.TSB1RelLabel3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel3, attNameExtension));
            this.TSB1TAmount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount3, attNameExtension));
            this.TSB1TUnit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit3, attNameExtension));
            this.TSB1TD1Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount3, attNameExtension));
            this.TSB1TD1Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit3, attNameExtension));
            this.TSB1TD2Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount3, attNameExtension));
            this.TSB1TD2Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit3, attNameExtension));
            this.TSB1MathResult3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult3, attNameExtension));
            this.TSB1MathSubType3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType3, attNameExtension));

            this.TSB1TMAmount3 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount3, attNameExtension));
            this.TSB1TMUnit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit3, attNameExtension));
            this.TSB1TLAmount3 = CalculatorHelpers.GetAttributeDouble(calculator,
             string.Concat(cTSB1TLAmount3, attNameExtension));
            this.TSB1TLUnit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit3, attNameExtension));
            this.TSB1TUAmount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount3, attNameExtension));
            this.TSB1TUUnit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit3, attNameExtension));
            this.TSB1MathOperator3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator3, attNameExtension));
            this.TSB1MathExpression3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression3, attNameExtension));
            this.TSB1N3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N3, attNameExtension));
            this.TSB1Date3 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date3, attNameExtension));
            this.TSB1MathType3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType3, attNameExtension));
            this.TSB1BaseIO3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO3, attNameExtension));
            this.TSB12Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount3, attNameExtension));
            this.TSB12Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit3, attNameExtension));
            this.TSB15Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount3, attNameExtension));
            this.TSB15Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit3, attNameExtension));
            this.TSB13Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB13Amount3, attNameExtension));
            this.TSB13Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit3, attNameExtension));
            this.TSB14Amount3 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount3, attNameExtension));
            this.TSB14Unit3 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit3, attNameExtension));

            this.TSB1Description4 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description4, attNameExtension));
            this.TSB1Name4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name4, attNameExtension));
            this.TSB1Label4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label4, attNameExtension));
            this.TSB1Type4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type4, attNameExtension));
            this.TSB11Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount4, attNameExtension));
            this.TSB11Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit4, attNameExtension));
            this.TSB1RelLabel4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel4, attNameExtension));
            this.TSB1TAmount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount4, attNameExtension));
            this.TSB1TUnit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit4, attNameExtension));
            this.TSB1TD1Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount4, attNameExtension));
            this.TSB1TD1Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit4, attNameExtension));
            this.TSB1TD2Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount4, attNameExtension));
            this.TSB1TD2Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit4, attNameExtension));
            this.TSB1MathResult4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult4, attNameExtension));
            this.TSB1MathSubType4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType4, attNameExtension));

            this.TSB1TMAmount4 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount4, attNameExtension));
            this.TSB1TMUnit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit4, attNameExtension));
            this.TSB1TLAmount4 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount4, attNameExtension));
            this.TSB1TLUnit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit4, attNameExtension));
            this.TSB1TUAmount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount4, attNameExtension));
            this.TSB1TUUnit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit4, attNameExtension));
            this.TSB1MathOperator4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator4, attNameExtension));
            this.TSB1MathExpression4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression4, attNameExtension));
            this.TSB1N4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N4, attNameExtension));
            this.TSB1Date4 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date4, attNameExtension));
            this.TSB1MathType4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType4, attNameExtension));
            this.TSB1BaseIO4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO4, attNameExtension));
            this.TSB12Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount4, attNameExtension));
            this.TSB12Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit4, attNameExtension));
            this.TSB15Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount4, attNameExtension));
            this.TSB15Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit4, attNameExtension));
            this.TSB13Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount4, attNameExtension));
            this.TSB13Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit4, attNameExtension));
            this.TSB14Amount4 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount4, attNameExtension));
            this.TSB14Unit4 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit4, attNameExtension));

            this.TSB1Description5 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description5, attNameExtension));
            this.TSB1Name5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name5, attNameExtension));
            this.TSB1Label5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label5, attNameExtension));
            this.TSB1Type5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type5, attNameExtension));
            this.TSB11Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount5, attNameExtension));
            this.TSB11Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit5, attNameExtension));
            this.TSB1RelLabel5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel5, attNameExtension));
            this.TSB1TAmount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount5, attNameExtension));
            this.TSB1TUnit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit5, attNameExtension));
            this.TSB1TD1Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount5, attNameExtension));
            this.TSB1TD1Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit5, attNameExtension));
            this.TSB1TD2Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount5, attNameExtension));
            this.TSB1TD2Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit5, attNameExtension));
            this.TSB1MathResult5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult5, attNameExtension));
            this.TSB1MathSubType5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType5, attNameExtension));

            this.TSB1TMAmount5 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount5, attNameExtension));
            this.TSB1TMUnit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit5, attNameExtension));
            this.TSB1TLAmount5 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount5, attNameExtension));
            this.TSB1TLUnit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit5, attNameExtension));
            this.TSB1TUAmount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount5, attNameExtension));
            this.TSB1TUUnit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit5, attNameExtension));
            this.TSB1MathOperator5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator5, attNameExtension));
            this.TSB1MathExpression5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression5, attNameExtension));
            this.TSB1N5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N5, attNameExtension));
            this.TSB1Date5 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date5, attNameExtension));
            this.TSB1MathType5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType5, attNameExtension));
            this.TSB1BaseIO5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO5, attNameExtension));
            this.TSB12Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount5, attNameExtension));
            this.TSB12Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit5, attNameExtension));
            this.TSB15Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount5, attNameExtension));
            this.TSB15Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit5, attNameExtension));
            this.TSB13Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount5, attNameExtension));
            this.TSB13Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit5, attNameExtension));
            this.TSB14Amount5 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount5, attNameExtension));
            this.TSB14Unit5 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit5, attNameExtension));

            this.TSB1Description6 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description6, attNameExtension));
            this.TSB1Name6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name6, attNameExtension));
            this.TSB1Label6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label6, attNameExtension));
            this.TSB1Type6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type6, attNameExtension));
            this.TSB11Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount6, attNameExtension));
            this.TSB11Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit6, attNameExtension));
            this.TSB1RelLabel6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel6, attNameExtension));
            this.TSB1TAmount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount6, attNameExtension));
            this.TSB1TUnit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit6, attNameExtension));
            this.TSB1TD1Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount6, attNameExtension));
            this.TSB1TD1Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit6, attNameExtension));
            this.TSB1TD2Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount6, attNameExtension));
            this.TSB1TD2Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit6, attNameExtension));
            this.TSB1MathResult6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult6, attNameExtension));
            this.TSB1MathSubType6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType6, attNameExtension));

            this.TSB1TMAmount6 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount6, attNameExtension));
            this.TSB1TMUnit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit6, attNameExtension));
            this.TSB1TLAmount6 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount6, attNameExtension));
            this.TSB1TLUnit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit6, attNameExtension));
            this.TSB1TUAmount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount6, attNameExtension));
            this.TSB1TUUnit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit6, attNameExtension));
            this.TSB1MathOperator6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator6, attNameExtension));
            this.TSB1MathExpression6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression6, attNameExtension));
            this.TSB1N6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N6, attNameExtension));
            this.TSB1Date6 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date6, attNameExtension));
            this.TSB1MathType6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType6, attNameExtension));
            this.TSB1BaseIO6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO6, attNameExtension));
            this.TSB12Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount6, attNameExtension));
            this.TSB12Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit6, attNameExtension));
            this.TSB15Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount6, attNameExtension));
            this.TSB15Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit6, attNameExtension));
            this.TSB13Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount6, attNameExtension));
            this.TSB13Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit6, attNameExtension));
            this.TSB14Amount6 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount6, attNameExtension));
            this.TSB14Unit6 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit6, attNameExtension));

            this.TSB1Description7 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description7, attNameExtension));
            this.TSB1Name7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name7, attNameExtension));
            this.TSB1Label7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label7, attNameExtension));
            this.TSB1Type7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type7, attNameExtension));
            this.TSB11Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount7, attNameExtension));
            this.TSB11Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit7, attNameExtension));
            this.TSB1RelLabel7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel7, attNameExtension));
            this.TSB1TAmount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount7, attNameExtension));
            this.TSB1TUnit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit7, attNameExtension));
            this.TSB1TD1Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount7, attNameExtension));
            this.TSB1TD1Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit7, attNameExtension));
            this.TSB1TD2Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount7, attNameExtension));
            this.TSB1TD2Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit7, attNameExtension));
            this.TSB1MathResult7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult7, attNameExtension));
            this.TSB1MathSubType7 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1MathSubType7, attNameExtension));

            this.TSB1TMAmount7 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount7, attNameExtension));
            this.TSB1TMUnit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit7, attNameExtension));
            this.TSB1TLAmount7 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount7, attNameExtension));
            this.TSB1TLUnit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit7, attNameExtension));
            this.TSB1TUAmount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount7, attNameExtension));
            this.TSB1TUUnit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit7, attNameExtension));
            this.TSB1MathOperator7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator7, attNameExtension));
            this.TSB1MathExpression7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression7, attNameExtension));
            this.TSB1N7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N7, attNameExtension));
            this.TSB1Date7 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date7, attNameExtension));
            this.TSB1MathType7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType7, attNameExtension));
            this.TSB1BaseIO7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO7, attNameExtension));
            this.TSB12Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount7, attNameExtension));
            this.TSB12Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit7, attNameExtension));
            this.TSB15Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount7, attNameExtension));
            this.TSB15Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit7, attNameExtension));
            this.TSB13Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount7, attNameExtension));
            this.TSB13Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit7, attNameExtension));
            this.TSB14Amount7 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount7, attNameExtension));
            this.TSB14Unit7 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit7, attNameExtension));

            this.TSB1Description8 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description8, attNameExtension));
            this.TSB1Name8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name8, attNameExtension));
            this.TSB1Label8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label8, attNameExtension));
            this.TSB1Type8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type8, attNameExtension));
            this.TSB11Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount8, attNameExtension));
            this.TSB11Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit8, attNameExtension));
            this.TSB1RelLabel8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel8, attNameExtension));
            this.TSB1TAmount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount8, attNameExtension));
            this.TSB1TUnit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit8, attNameExtension));
            this.TSB1TD1Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount8, attNameExtension));
            this.TSB1TD1Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit8, attNameExtension));
            this.TSB1TD2Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount8, attNameExtension));
            this.TSB1TD2Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit8, attNameExtension));
            this.TSB1MathResult8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult8, attNameExtension));
            this.TSB1MathSubType8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType8, attNameExtension));

            this.TSB1TMAmount8 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount8, attNameExtension));
            this.TSB1TMUnit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit8, attNameExtension));
            this.TSB1TLAmount8 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount8, attNameExtension));
            this.TSB1TLUnit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit8, attNameExtension));
            this.TSB1TUAmount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount8, attNameExtension));
            this.TSB1TUUnit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit8, attNameExtension));
            this.TSB1MathOperator8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator8, attNameExtension));
            this.TSB1MathExpression8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression8, attNameExtension));
            this.TSB1N8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N8, attNameExtension));
            this.TSB1Date8 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date8, attNameExtension));
            this.TSB1MathType8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType8, attNameExtension));
            this.TSB1BaseIO8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO8, attNameExtension));
            this.TSB12Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount8, attNameExtension));
            this.TSB12Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit8, attNameExtension));
            this.TSB15Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount8, attNameExtension));
            this.TSB15Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit8, attNameExtension));
            this.TSB13Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount8, attNameExtension));
            this.TSB13Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit8, attNameExtension));
            this.TSB14Amount8 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount8, attNameExtension));
            this.TSB14Unit8 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit8, attNameExtension));

            this.TSB1Description9 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description9, attNameExtension));
            this.TSB1Name9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name9, attNameExtension));
            this.TSB1Label9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label9, attNameExtension));
            this.TSB1Type9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type9, attNameExtension));
            this.TSB11Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount9, attNameExtension));
            this.TSB11Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit9, attNameExtension));
            this.TSB1RelLabel9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel9, attNameExtension));
            this.TSB1TAmount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount9, attNameExtension));
            this.TSB1TUnit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit9, attNameExtension));
            this.TSB1TD1Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount9, attNameExtension));
            this.TSB1TD1Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit9, attNameExtension));
            this.TSB1TD2Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount9, attNameExtension));
            this.TSB1TD2Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit9, attNameExtension));
            this.TSB1MathResult9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult9, attNameExtension));
            this.TSB1MathSubType9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType9, attNameExtension));

            this.TSB1TMAmount9 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount9, attNameExtension));
            this.TSB1TMUnit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit9, attNameExtension));
            this.TSB1TLAmount9 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount9, attNameExtension));
            this.TSB1TLUnit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit9, attNameExtension));
            this.TSB1TUAmount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount9, attNameExtension));
            this.TSB1TUUnit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit9, attNameExtension));
            this.TSB1MathOperator9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator9, attNameExtension));
            this.TSB1MathExpression9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression9, attNameExtension));
            this.TSB1N9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N9, attNameExtension));
            this.TSB1Date9 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date9, attNameExtension));
            this.TSB1MathType9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType9, attNameExtension));
            this.TSB1BaseIO9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO9, attNameExtension));
            this.TSB12Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount9, attNameExtension));
            this.TSB12Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit9, attNameExtension));
            this.TSB15Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount9, attNameExtension));
            this.TSB15Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit9, attNameExtension));
            this.TSB13Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount9, attNameExtension));
            this.TSB13Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit9, attNameExtension));
            this.TSB14Amount9 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount9, attNameExtension));
            this.TSB14Unit9 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit9, attNameExtension));

            this.TSB1Description10 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description10, attNameExtension));
            this.TSB1Name10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name10, attNameExtension));
            this.TSB1Label10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label10, attNameExtension));
            this.TSB1Type10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type10, attNameExtension));
            this.TSB11Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount10, attNameExtension));
            this.TSB11Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit10, attNameExtension));
            this.TSB1RelLabel10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel10, attNameExtension));
            this.TSB1TAmount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount10, attNameExtension));
            this.TSB1TUnit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit10, attNameExtension));
            this.TSB1TD1Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount10, attNameExtension));
            this.TSB1TD1Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit10, attNameExtension));
            this.TSB1TD2Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount10, attNameExtension));
            this.TSB1TD2Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit10, attNameExtension));
            this.TSB1MathResult10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult10, attNameExtension));
            this.TSB1MathSubType10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType10, attNameExtension));

            this.TSB1TMAmount10 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount10, attNameExtension));
            this.TSB1TMUnit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit10, attNameExtension));
            this.TSB1TLAmount10 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount10, attNameExtension));
            this.TSB1TLUnit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit10, attNameExtension));
            this.TSB1TUAmount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount10, attNameExtension));
            this.TSB1TUUnit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit10, attNameExtension));
            this.TSB1MathOperator10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator10, attNameExtension));
            this.TSB1MathExpression10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression10, attNameExtension));
            this.TSB1N10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N10, attNameExtension));
            this.TSB1Date10 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date10, attNameExtension));
            this.TSB1MathType10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType10, attNameExtension));
            this.TSB1BaseIO10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO10, attNameExtension));
            this.TSB12Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount10, attNameExtension));
            this.TSB12Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit10, attNameExtension));
            this.TSB15Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount10, attNameExtension));
            this.TSB15Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit10, attNameExtension));
            this.TSB13Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount10, attNameExtension));
            this.TSB13Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit10, attNameExtension));
            this.TSB14Amount10 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount10, attNameExtension));
            this.TSB14Unit10 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit10, attNameExtension));

            this.TSB1Description11 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description11, attNameExtension));
            this.TSB1Name11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name11, attNameExtension));
            this.TSB1Label11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label11, attNameExtension));
            this.TSB1Type11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type11, attNameExtension));
            this.TSB11Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount11, attNameExtension));
            this.TSB11Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit11, attNameExtension));
            this.TSB1RelLabel11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel11, attNameExtension));
            this.TSB1TAmount11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount11, attNameExtension));
            this.TSB1TUnit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit11, attNameExtension));
            this.TSB1TD1Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount11, attNameExtension));
            this.TSB1TD1Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit11, attNameExtension));
            this.TSB1TD2Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount11, attNameExtension));
            this.TSB1TD2Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit11, attNameExtension));
            this.TSB1MathResult11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult11, attNameExtension));
            this.TSB1MathSubType11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType11, attNameExtension));

            this.TSB1TMAmount11 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount11, attNameExtension));
            this.TSB1TMUnit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit11, attNameExtension));
            this.TSB1TLAmount11 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount11, attNameExtension));
            this.TSB1TLUnit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit11, attNameExtension));
            this.TSB1TUAmount11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount11, attNameExtension));
            this.TSB1TUUnit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit11, attNameExtension));
            this.TSB1MathOperator11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator11, attNameExtension));
            this.TSB1MathExpression11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression11, attNameExtension));
            this.TSB1N11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N11, attNameExtension));
            this.TSB1Date11 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date11, attNameExtension));
            this.TSB1MathType11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType11, attNameExtension));
            this.TSB1BaseIO11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO11, attNameExtension));
            this.TSB12Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount11, attNameExtension));
            this.TSB12Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit11, attNameExtension));
            this.TSB15Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount11, attNameExtension));
            this.TSB15Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit11, attNameExtension));
            this.TSB13Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount11, attNameExtension));
            this.TSB13Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit11, attNameExtension));
            this.TSB14Amount11 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount11, attNameExtension));
            this.TSB14Unit11 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit11, attNameExtension));

            this.TSB1Description12 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description12, attNameExtension));
            this.TSB1Name12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name12, attNameExtension));
            this.TSB1Label12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label12, attNameExtension));
            this.TSB1Type12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type12, attNameExtension));
            this.TSB11Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount12, attNameExtension));
            this.TSB11Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit12, attNameExtension));
            this.TSB1RelLabel12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel12, attNameExtension));
            this.TSB1TAmount12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount12, attNameExtension));
            this.TSB1TUnit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit12, attNameExtension));
            this.TSB1TD1Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount12, attNameExtension));
            this.TSB1TD1Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit12, attNameExtension));
            this.TSB1TD2Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount12, attNameExtension));
            this.TSB1TD2Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit12, attNameExtension));
            this.TSB1MathResult12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult12, attNameExtension));
            this.TSB1MathSubType12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType12, attNameExtension));

            this.TSB1TMAmount12 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount12, attNameExtension));
            this.TSB1TMUnit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit12, attNameExtension));
            this.TSB1TLAmount12 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount12, attNameExtension));
            this.TSB1TLUnit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit12, attNameExtension));
            this.TSB1TUAmount12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount12, attNameExtension));
            this.TSB1TUUnit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit12, attNameExtension));
            this.TSB1MathOperator12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator12, attNameExtension));
            this.TSB1MathExpression12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression12, attNameExtension));
            this.TSB1N12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N12, attNameExtension));
            this.TSB1Date12 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date12, attNameExtension));
            this.TSB1MathType12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType12, attNameExtension));
            this.TSB1BaseIO12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO12, attNameExtension));
            this.TSB12Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount12, attNameExtension));
            this.TSB12Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit12, attNameExtension));
            this.TSB15Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount12, attNameExtension));
            this.TSB15Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit12, attNameExtension));
            this.TSB13Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount12, attNameExtension));
            this.TSB13Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit12, attNameExtension));
            this.TSB14Amount12 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount12, attNameExtension));
            this.TSB14Unit12 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit12, attNameExtension));

            this.TSB1Description13 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description13, attNameExtension));
            this.TSB1Name13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name13, attNameExtension));
            this.TSB1Label13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label13, attNameExtension));
            this.TSB1Type13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type13, attNameExtension));
            this.TSB11Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount13, attNameExtension));
            this.TSB11Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit13, attNameExtension));
            this.TSB1RelLabel13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel13, attNameExtension));
            this.TSB1TAmount13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount13, attNameExtension));
            this.TSB1TUnit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit13, attNameExtension));
            this.TSB1TD1Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount13, attNameExtension));
            this.TSB1TD1Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit13, attNameExtension));
            this.TSB1TD2Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount13, attNameExtension));
            this.TSB1TD2Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit13, attNameExtension));
            this.TSB1MathResult13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult13, attNameExtension));
            this.TSB1MathSubType13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType13, attNameExtension));

            this.TSB1TMAmount13 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount13, attNameExtension));
            this.TSB1TMUnit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit13, attNameExtension));
            this.TSB1TLAmount13 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount13, attNameExtension));
            this.TSB1TLUnit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit13, attNameExtension));
            this.TSB1TUAmount13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount13, attNameExtension));
            this.TSB1TUUnit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit13, attNameExtension));
            this.TSB1MathOperator13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator13, attNameExtension));
            this.TSB1MathExpression13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression13, attNameExtension));
            this.TSB1N13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N13, attNameExtension));
            this.TSB1Date13 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date13, attNameExtension));
            this.TSB1MathType13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType13, attNameExtension));
            this.TSB1BaseIO13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO13, attNameExtension));
            this.TSB12Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount13, attNameExtension));
            this.TSB12Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit13, attNameExtension));
            this.TSB15Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount13, attNameExtension));
            this.TSB15Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit13, attNameExtension));
            this.TSB13Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount13, attNameExtension));
            this.TSB13Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit13, attNameExtension));
            this.TSB14Amount13 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount13, attNameExtension));
            this.TSB14Unit13 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit13, attNameExtension));

            this.TSB1Description14 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description14, attNameExtension));
            this.TSB1Name14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name14, attNameExtension));
            this.TSB1Label14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label14, attNameExtension));
            this.TSB1Type14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type14, attNameExtension));
            this.TSB11Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount14, attNameExtension));
            this.TSB11Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit14, attNameExtension));
            this.TSB1RelLabel14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel14, attNameExtension));
            this.TSB1TAmount14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount14, attNameExtension));
            this.TSB1TUnit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit14, attNameExtension));
            this.TSB1TD1Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount14, attNameExtension));
            this.TSB1TD1Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit14, attNameExtension));
            this.TSB1TD2Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount14, attNameExtension));
            this.TSB1TD2Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit14, attNameExtension));
            this.TSB1MathResult14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult14, attNameExtension));
            this.TSB1MathSubType14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType14, attNameExtension));

            this.TSB1TMAmount14 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount14, attNameExtension));
            this.TSB1TMUnit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit14, attNameExtension));
            this.TSB1TLAmount14 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount14, attNameExtension));
            this.TSB1TLUnit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit14, attNameExtension));
            this.TSB1TUAmount14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount14, attNameExtension));
            this.TSB1TUUnit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit14, attNameExtension));
            this.TSB1MathOperator14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator14, attNameExtension));
            this.TSB1MathExpression14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression14, attNameExtension));
            this.TSB1N14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N14, attNameExtension));
            this.TSB1Date14 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date14, attNameExtension));
            this.TSB1MathType14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType14, attNameExtension));
            this.TSB1BaseIO14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO14, attNameExtension));
            this.TSB12Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount14, attNameExtension));
            this.TSB12Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit14, attNameExtension));
            this.TSB15Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount14, attNameExtension));
            this.TSB15Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit14, attNameExtension));
            this.TSB13Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount14, attNameExtension));
            this.TSB13Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit14, attNameExtension));
            this.TSB14Amount14 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount14, attNameExtension));
            this.TSB14Unit14 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit14, attNameExtension));

            this.TSB1Description15 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description15, attNameExtension));
            this.TSB1Name15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name15, attNameExtension));
            this.TSB1Label15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label15, attNameExtension));
            this.TSB1Type15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type15, attNameExtension));
            this.TSB11Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount15, attNameExtension));
            this.TSB11Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit15, attNameExtension));
            this.TSB1RelLabel15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel15, attNameExtension));
            this.TSB1TAmount15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount15, attNameExtension));
            this.TSB1TUnit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit15, attNameExtension));
            this.TSB1TD1Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount15, attNameExtension));
            this.TSB1TD1Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit15, attNameExtension));
            this.TSB1TD2Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount15, attNameExtension));
            this.TSB1TD2Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit15, attNameExtension));
            this.TSB1MathResult15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult15, attNameExtension));
            this.TSB1MathSubType15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType15, attNameExtension));

            this.TSB1TMAmount15 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount15, attNameExtension));
            this.TSB1TMUnit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit15, attNameExtension));
            this.TSB1TLAmount15 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount15, attNameExtension));
            this.TSB1TLUnit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit15, attNameExtension));
            this.TSB1TUAmount15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount15, attNameExtension));
            this.TSB1TUUnit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit15, attNameExtension));
            this.TSB1MathOperator15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator15, attNameExtension));
            this.TSB1MathExpression15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression15, attNameExtension));
            this.TSB1N15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N15, attNameExtension));
            this.TSB1Date15 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date15, attNameExtension));
            this.TSB1MathType15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType15, attNameExtension));
            this.TSB1BaseIO15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO15, attNameExtension));
            this.TSB12Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount15, attNameExtension));
            this.TSB12Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit15, attNameExtension));
            this.TSB15Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount15, attNameExtension));
            this.TSB15Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit15, attNameExtension));
            this.TSB13Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount15, attNameExtension));
            this.TSB13Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit15, attNameExtension));
            this.TSB14Amount15 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount15, attNameExtension));
            this.TSB14Unit15 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit15, attNameExtension));

            this.TSB1Description16 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description16, attNameExtension));
            this.TSB1Name16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name16, attNameExtension));
            this.TSB1Label16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label16, attNameExtension));
            this.TSB1Type16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type16, attNameExtension));
            this.TSB11Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount16, attNameExtension));
            this.TSB11Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit16, attNameExtension));
            this.TSB1RelLabel16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel16, attNameExtension));
            this.TSB1TAmount16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount16, attNameExtension));
            this.TSB1TUnit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit16, attNameExtension));
            this.TSB1TD1Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount16, attNameExtension));
            this.TSB1TD1Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit16, attNameExtension));
            this.TSB1TD2Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount16, attNameExtension));
            this.TSB1TD2Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit16, attNameExtension));
            this.TSB1MathResult16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult16, attNameExtension));
            this.TSB1MathSubType16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType16, attNameExtension));

            this.TSB1TMAmount16 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount16, attNameExtension));
            this.TSB1TMUnit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit16, attNameExtension));
            this.TSB1TLAmount16 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount16, attNameExtension));
            this.TSB1TLUnit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit16, attNameExtension));
            this.TSB1TUAmount16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount16, attNameExtension));
            this.TSB1TUUnit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit16, attNameExtension));
            this.TSB1MathOperator16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator16, attNameExtension));
            this.TSB1MathExpression16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression16, attNameExtension));
            this.TSB1N16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N16, attNameExtension));
            this.TSB1Date16 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date16, attNameExtension));
            this.TSB1MathType17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType17, attNameExtension));
            this.TSB1BaseIO16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO16, attNameExtension));
            this.TSB12Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount16, attNameExtension));
            this.TSB12Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit16, attNameExtension));
            this.TSB15Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount16, attNameExtension));
            this.TSB15Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit16, attNameExtension));
            this.TSB13Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount16, attNameExtension));
            this.TSB13Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit16, attNameExtension));
            this.TSB14Amount16 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount16, attNameExtension));
            this.TSB14Unit16 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit16, attNameExtension));

            this.TSB1Description17 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description17, attNameExtension));
            this.TSB1Name17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name17, attNameExtension));
            this.TSB1Label17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label17, attNameExtension));
            this.TSB1Type17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type17, attNameExtension));
            this.TSB11Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount17, attNameExtension));
            this.TSB11Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit17, attNameExtension));
            this.TSB1RelLabel17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel17, attNameExtension));
            this.TSB1TAmount17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount17, attNameExtension));
            this.TSB1TUnit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit17, attNameExtension));
            this.TSB1TD1Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount17, attNameExtension));
            this.TSB1TD1Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit17, attNameExtension));
            this.TSB1TD2Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount17, attNameExtension));
            this.TSB1TD2Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit17, attNameExtension));
            this.TSB1MathResult17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult17, attNameExtension));
            this.TSB1MathSubType17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType17, attNameExtension));

            this.TSB1TMAmount17 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount17, attNameExtension));
            this.TSB1TMUnit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit17, attNameExtension));
            this.TSB1TLAmount17 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount17, attNameExtension));
            this.TSB1TLUnit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit17, attNameExtension));
            this.TSB1TUAmount17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount17, attNameExtension));
            this.TSB1TUUnit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit17, attNameExtension));
            this.TSB1MathOperator17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator17, attNameExtension));
            this.TSB1MathExpression17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression17, attNameExtension));
            this.TSB1N17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N17, attNameExtension));
            this.TSB1Date17 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date17, attNameExtension));
            this.TSB1MathType17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType17, attNameExtension));
            this.TSB1BaseIO17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO17, attNameExtension));
            this.TSB12Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount17, attNameExtension));
            this.TSB12Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit17, attNameExtension));
            this.TSB15Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount17, attNameExtension));
            this.TSB15Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit17, attNameExtension));
            this.TSB13Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount17, attNameExtension));
            this.TSB13Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit17, attNameExtension));
            this.TSB14Amount17 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount17, attNameExtension));
            this.TSB14Unit17 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit17, attNameExtension));

            this.TSB1Description18 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description18, attNameExtension));
            this.TSB1Name18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name18, attNameExtension));
            this.TSB1Label18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label18, attNameExtension));
            this.TSB1Type18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type18, attNameExtension));
            this.TSB11Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount18, attNameExtension));
            this.TSB11Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit18, attNameExtension));
            this.TSB1RelLabel18 = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(cTSB1RelLabel18, attNameExtension));
            this.TSB1TAmount18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount18, attNameExtension));
            this.TSB1TUnit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit18, attNameExtension));
            this.TSB1TD1Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount18, attNameExtension));
            this.TSB1TD1Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit18, attNameExtension));
            this.TSB1TD2Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount18, attNameExtension));
            this.TSB1TD2Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit18, attNameExtension));
            this.TSB1MathResult18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult18, attNameExtension));
            this.TSB1MathSubType18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType18, attNameExtension));

            this.TSB1TMAmount18 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount18, attNameExtension));
            this.TSB1TMUnit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit18, attNameExtension));
            this.TSB1TLAmount18 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount18, attNameExtension));
            this.TSB1TLUnit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit18, attNameExtension));
            this.TSB1TUAmount18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount18, attNameExtension));
            this.TSB1TUUnit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit18, attNameExtension));
            this.TSB1MathOperator18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator18, attNameExtension));
            this.TSB1MathExpression18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression18, attNameExtension));
            this.TSB1N18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N18, attNameExtension));
            this.TSB1Date18 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date18, attNameExtension));
            this.TSB1MathType18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType18, attNameExtension));
            this.TSB1BaseIO18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO18, attNameExtension));
            this.TSB12Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount18, attNameExtension));
            this.TSB12Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit18, attNameExtension));
            this.TSB15Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount18, attNameExtension));
            this.TSB15Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit18, attNameExtension));
            this.TSB13Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount18, attNameExtension));
            this.TSB13Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit18, attNameExtension));
            this.TSB14Amount18 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount18, attNameExtension));
            this.TSB14Unit18 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit18, attNameExtension));

            this.TSB1Description19 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description19, attNameExtension));
            this.TSB1Name19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name19, attNameExtension));
            this.TSB1Label19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label19, attNameExtension));
            this.TSB1Type19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type19, attNameExtension));
            this.TSB11Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount19, attNameExtension));
            this.TSB11Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit19, attNameExtension));
            this.TSB1RelLabel19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel19, attNameExtension));
            this.TSB1TAmount19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount19, attNameExtension));
            this.TSB1TUnit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit19, attNameExtension));
            this.TSB1TD1Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount19, attNameExtension));
            this.TSB1TD1Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit19, attNameExtension));
            this.TSB1TD2Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount19, attNameExtension));
            this.TSB1TD2Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit19, attNameExtension));
            this.TSB1MathResult19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult19, attNameExtension));
            this.TSB1MathSubType19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType19, attNameExtension));

            this.TSB1TMAmount19 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount19, attNameExtension));
            this.TSB1TMUnit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit19, attNameExtension));
            this.TSB1TLAmount19 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TLAmount19, attNameExtension));
            this.TSB1TLUnit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit19, attNameExtension));
            this.TSB1TUAmount19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount19, attNameExtension));
            this.TSB1TUUnit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit19, attNameExtension));
            this.TSB1MathOperator19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator19, attNameExtension));
            this.TSB1MathExpression19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression19, attNameExtension));
            this.TSB1N19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N19, attNameExtension));
            this.TSB1Date19 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date19, attNameExtension));
            this.TSB1MathType19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType19, attNameExtension));
            this.TSB1BaseIO19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO19, attNameExtension));
            this.TSB12Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount19, attNameExtension));
            this.TSB12Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit19, attNameExtension));
            this.TSB15Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount19, attNameExtension));
            this.TSB15Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit19, attNameExtension));
            this.TSB13Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount19, attNameExtension));
            this.TSB13Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit19, attNameExtension));
            this.TSB14Amount19 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount19, attNameExtension));
            this.TSB14Unit19 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit19, attNameExtension));

            this.TSB1Description20 = CalculatorHelpers.GetAttribute(calculator,
              string.Concat(cTSB1Description20, attNameExtension));
            this.TSB1Name20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Name20, attNameExtension));
            this.TSB1Label20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Label20, attNameExtension));
            this.TSB1Type20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1Type20, attNameExtension));
            this.TSB11Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB11Amount20, attNameExtension));
            this.TSB11Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB11Unit20, attNameExtension));
            this.TSB1RelLabel20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1RelLabel20, attNameExtension));
            this.TSB1TAmount20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TAmount20, attNameExtension));
            this.TSB1TUnit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUnit20, attNameExtension));
            this.TSB1TD1Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TD1Amount20, attNameExtension));
            this.TSB1TD1Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD1Unit20, attNameExtension));
            this.TSB1TD2Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TD2Amount20, attNameExtension));
            this.TSB1TD2Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TD2Unit20, attNameExtension));
            this.TSB1MathResult20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathResult20, attNameExtension));
            this.TSB1MathSubType20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathSubType20, attNameExtension));

            this.TSB1TMAmount20 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB1TMAmount20, attNameExtension));
            this.TSB1TMUnit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TMUnit20, attNameExtension));
            this.TSB1TLAmount20 = CalculatorHelpers.GetAttributeDouble(calculator,
             string.Concat(cTSB1TLAmount20, attNameExtension));
            this.TSB1TLUnit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TLUnit20, attNameExtension));
            this.TSB1TUAmount20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1TUAmount20, attNameExtension));
            this.TSB1TUUnit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1TUUnit20, attNameExtension));
            this.TSB1MathOperator20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathOperator20, attNameExtension));
            this.TSB1MathExpression20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathExpression20, attNameExtension));
            this.TSB1N20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB1N20, attNameExtension));
            this.TSB1Date20 = CalculatorHelpers.GetAttributeDate(calculator,
               string.Concat(cTSB1Date20, attNameExtension));
            this.TSB1MathType20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1MathType20, attNameExtension));
            this.TSB1BaseIO20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB1BaseIO20, attNameExtension));
            this.TSB12Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB12Amount20, attNameExtension));
            this.TSB12Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB12Unit20, attNameExtension));
            this.TSB15Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTSB15Amount20, attNameExtension));
            this.TSB15Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB15Unit20, attNameExtension));
            this.TSB13Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB13Amount20, attNameExtension));
            this.TSB13Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB13Unit20, attNameExtension));
            this.TSB14Amount20 = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cTSB14Amount20, attNameExtension));
            this.TSB14Unit20 = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cTSB14Unit20, attNameExtension));
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTSB1BaseStockProperty(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cTSB1Score:
                    this.TSB1Score = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1ScoreUnit:
                    this.TSB1ScoreUnit = attValue;
                    break;
                case cTSB1ScoreD1Amount:
                    this.TSB1ScoreD1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1ScoreD1Unit:
                    this.TSB1ScoreD1Unit = attValue;
                    break;
                case cTSB1ScoreD2Amount:
                    this.TSB1ScoreD2Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1ScoreD2Unit:
                    this.TSB1ScoreD2Unit = attValue;
                    break;
                case cTSB1ScoreMathExpression:
                    this.TSB1ScoreMathExpression = attValue;
                    break;
                case cTSB1ScoreM:
                    this.TSB1ScoreM = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1ScoreMUnit:
                    this.TSB1ScoreMUnit = attValue;
                    break;
                case cTSB1ScoreLAmount:
                    this.TSB1ScoreLAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1ScoreLUnit:
                    this.TSB1ScoreLUnit = attValue;
                    break;
                case cTSB1ScoreUAmount:
                    this.TSB1ScoreUAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1ScoreUUnit:
                    this.TSB1ScoreUUnit = attValue;
                    break;
                case cTSB1Iterations:
                    this.TSB1Iterations = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cTSB1ScoreDistType:
                    this.TSB1ScoreDistType = attValue;
                    break;
                case cTSB1ScoreMathType:
                    this.TSB1ScoreMathType = attValue;
                    break;
                case cTSB1ScoreMathResult:
                    this.TSB1ScoreMathResult = attValue;
                    break;
                case cTSB1ScoreMathSubType:
                    this.TSB1ScoreMathSubType = attValue;
                    break;
                case cTSB1ScoreN:
                    this.TSB1ScoreN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Description1:
                    this.TSB1Description1 = attValue;
                    break;
                case cTSB1Name1:
                    this.TSB1Name1 = attValue;
                    break;
                case cTSB1Label1:
                    this.TSB1Label1 = attValue;
                    break;
                case cTSB1Type1:
                    this.TSB1Type1 = attValue;
                    break;
                case cTSB1RelLabel1:
                    this.TSB1RelLabel1 = attValue;
                    break;
                case cTSB1TAmount1:
                    this.TSB1TAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit1:
                    this.TSB1TUnit1 = attValue;
                    break;
                case cTSB1TD1Amount1:
                    this.TSB1TD1Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit1:
                    this.TSB1TD1Unit1 = attValue;
                    break;
                case cTSB1TD2Amount1:
                    this.TSB1TD2Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit1:
                    this.TSB1TD2Unit1 = attValue;
                    break;
                case cTSB1MathResult1:
                    this.TSB1MathResult1 = attValue;
                    break;
                case cTSB1MathSubType1:
                    this.TSB1MathSubType1 = attValue;
                    break;
                case cTSB1TMAmount1:
                    this.TSB1TMAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit1:
                    this.TSB1TMUnit1 = attValue;
                    break;
                case cTSB1TLAmount1:
                    this.TSB1TLAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit1:
                    this.TSB1TLUnit1 = attValue;
                    break;
                case cTSB1TUAmount1:
                    this.TSB1TUAmount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit1:
                    this.TSB1TUUnit1 = attValue;
                    break;
                case cTSB1MathOperator1:
                    this.TSB1MathOperator1 = attValue;
                    break;
                case cTSB1MathExpression1:
                    this.TSB1MathExpression1 = attValue;
                    break;
                case cTSB1N1:
                    this.TSB1N1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date1:
                    this.TSB1Date1 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType1:
                    this.TSB1MathType1 = attValue;
                    break;
                case cTSB11Amount1:
                    this.TSB11Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit1:
                    this.TSB11Unit1 = attValue;
                    break;
                case cTSB12Amount1:
                    this.TSB12Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit1:
                    this.TSB12Unit1 = attValue;
                    break;
                case cTSB15Amount1:
                    this.TSB15Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit1:
                    this.TSB15Unit1 = attValue;
                    break;
                case cTSB13Amount1:
                    this.TSB13Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit1:
                    this.TSB13Unit1 = attValue;
                    break;
                case cTSB14Amount1:
                    this.TSB14Amount1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit1:
                    this.TSB14Unit1 = attValue;
                    break;
                case cTSB1Description2:
                    this.TSB1Description2 = attValue;
                    break;
                case cTSB1Name2:
                    this.TSB1Name2 = attValue;
                    break;
                case cTSB1Label2:
                    this.TSB1Label2 = attValue;
                    break;
                case cTSB1Type2:
                    this.TSB1Type2 = attValue;
                    break;
                case cTSB1RelLabel2:
                    this.TSB1RelLabel2 = attValue;
                    break;
                case cTSB1TAmount2:
                    this.TSB1TAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit2:
                    this.TSB1TUnit2 = attValue;
                    break;
                case cTSB1TD1Amount2:
                    this.TSB1TD1Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit2:
                    this.TSB1TD1Unit2 = attValue;
                    break;
                case cTSB1TD2Amount2:
                    this.TSB1TD2Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit2:
                    this.TSB1TD2Unit2 = attValue;
                    break;
                case cTSB1MathResult2:
                    this.TSB1MathResult2 = attValue;
                    break;
                case cTSB1MathSubType2:
                    this.TSB1MathSubType2 = attValue;
                    break;
                case cTSB1TMAmount2:
                    this.TSB1TMAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit2:
                    this.TSB1TMUnit2 = attValue;
                    break;
                case cTSB1TLAmount2:
                    this.TSB1TLAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit2:
                    this.TSB1TLUnit2 = attValue;
                    break;
                case cTSB1TUAmount2:
                    this.TSB1TUAmount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit2:
                    this.TSB1TUUnit2 = attValue;
                    break;
                case cTSB1MathOperator2:
                    this.TSB1MathOperator2 = attValue;
                    break;
                case cTSB1MathExpression2:
                    this.TSB1MathExpression2 = attValue;
                    break;
                case cTSB1N2:
                    this.TSB1N2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date2:
                    this.TSB1Date2 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType2:
                    this.TSB1MathType2 = attValue;
                    break;
                case cTSB11Amount2:
                    this.TSB11Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit2:
                    this.TSB11Unit2 = attValue;
                    break;
                case cTSB12Amount2:
                    this.TSB12Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit2:
                    this.TSB12Unit2 = attValue;
                    break;
                case cTSB15Amount2:
                    this.TSB15Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit2:
                    this.TSB15Unit2 = attValue;
                    break;
                case cTSB13Amount2:
                    this.TSB13Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit2:
                    this.TSB13Unit2 = attValue;
                    break;
                case cTSB14Amount2:
                    this.TSB14Amount2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit2:
                    this.TSB14Unit2 = attValue;
                    break;
                case cTSB1Description3:
                    this.TSB1Description3 = attValue;
                    break;
                case cTSB1Name3:
                    this.TSB1Name3 = attValue;
                    break;
                case cTSB1Label3:
                    this.TSB1Label3 = attValue;
                    break;
                case cTSB1Type3:
                    this.TSB1Type3 = attValue;
                    break;
                case cTSB1RelLabel3:
                    this.TSB1RelLabel3 = attValue;
                    break;
                case cTSB1TAmount3:
                    this.TSB1TAmount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit3:
                    this.TSB1TUnit3 = attValue;
                    break;
                case cTSB1TD1Amount3:
                    this.TSB1TD1Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit3:
                    this.TSB1TD1Unit3 = attValue;
                    break;
                case cTSB1TD2Amount3:
                    this.TSB1TD2Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit3:
                    this.TSB1TD2Unit3 = attValue;
                    break;
                case cTSB1MathResult3:
                    this.TSB1MathResult3 = attValue;
                    break;
                case cTSB1MathSubType3:
                    this.TSB1MathResult3 = attValue;
                    break;
                case cTSB1TMAmount3:
                    this.TSB1TMAmount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit3:
                    this.TSB1TMUnit3 = attValue;
                    break;
                case cTSB1TLAmount3:
                    this.TSB1TLAmount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit3:
                    this.TSB1TLUnit3 = attValue;
                    break;
                case cTSB1TUAmount3:
                    this.TSB1TUAmount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit3:
                    this.TSB1TUUnit3 = attValue;
                    break;
                case cTSB1MathOperator3:
                    this.TSB1MathOperator3 = attValue;
                    break;
                case cTSB1MathExpression3:
                    this.TSB1MathExpression3 = attValue;
                    break;
                case cTSB1N3:
                    this.TSB1N3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date3:
                    this.TSB1Date3 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType3:
                    this.TSB1MathType3 = attValue;
                    break;
                case cTSB11Amount3:
                    this.TSB11Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit3:
                    this.TSB11Unit3 = attValue;
                    break;
                case cTSB12Amount3:
                    this.TSB12Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit3:
                    this.TSB12Unit3 = attValue;
                    break;
                case cTSB15Amount3:
                    this.TSB15Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit3:
                    this.TSB15Unit3 = attValue;
                    break;
                case cTSB13Amount3:
                    this.TSB13Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit3:
                    this.TSB13Unit3 = attValue;
                    break;
                case cTSB14Amount3:
                    this.TSB14Amount3 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit3:
                    this.TSB14Unit3 = attValue;
                    break;
                case cTSB1Description4:
                    this.TSB1Description4 = attValue;
                    break;
                case cTSB1Name4:
                    this.TSB1Name4 = attValue;
                    break;
                case cTSB1Label4:
                    this.TSB1Label4 = attValue;
                    break;
                case cTSB1Type4:
                    this.TSB1Type4 = attValue;
                    break;
                case cTSB1RelLabel4:
                    this.TSB1RelLabel4 = attValue;
                    break;
                case cTSB1TAmount4:
                    this.TSB1TAmount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit4:
                    this.TSB1TUnit4 = attValue;
                    break;
                case cTSB1TD1Amount4:
                    this.TSB1TD1Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit4:
                    this.TSB1TD1Unit4 = attValue;
                    break;
                case cTSB1TD2Amount4:
                    this.TSB1TD2Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit4:
                    this.TSB1TD2Unit4 = attValue;
                    break;
                case cTSB1MathResult4:
                    this.TSB1MathResult4 = attValue;
                    break;
                case cTSB1MathSubType4:
                    this.TSB1MathResult4 = attValue;
                    break;
                case cTSB1TMAmount4:
                    this.TSB1TMAmount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit4:
                    this.TSB1TMUnit4 = attValue;
                    break;
                case cTSB1TLAmount4:
                    this.TSB1TLAmount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit4:
                    this.TSB1TLUnit4 = attValue;
                    break;
                case cTSB1TUAmount4:
                    this.TSB1TUAmount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit4:
                    this.TSB1TUUnit4 = attValue;
                    break;
                case cTSB1MathOperator4:
                    this.TSB1MathOperator4 = attValue;
                    break;
                case cTSB1MathExpression4:
                    this.TSB1MathExpression4 = attValue;
                    break;
                case cTSB1N4:
                    this.TSB1N4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date4:
                    this.TSB1Date4 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType4:
                    this.TSB1MathType4 = attValue;
                    break;
                case cTSB11Amount4:
                    this.TSB11Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit4:
                    this.TSB11Unit4 = attValue;
                    break;
                case cTSB12Amount4:
                    this.TSB12Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit4:
                    this.TSB12Unit4 = attValue;
                    break;
                case cTSB15Amount4:
                    this.TSB15Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit4:
                    this.TSB15Unit4 = attValue;
                    break;
                case cTSB13Amount4:
                    this.TSB13Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit4:
                    this.TSB13Unit4 = attValue;
                    break;
                case cTSB14Amount4:
                    this.TSB14Amount4 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit4:
                    this.TSB14Unit4 = attValue;
                    break;
                case cTSB1Description5:
                    this.TSB1Description5 = attValue;
                    break;
                case cTSB1Name5:
                    this.TSB1Name5 = attValue;
                    break;
                case cTSB1Label5:
                    this.TSB1Label5 = attValue;
                    break;
                case cTSB1Type5:
                    this.TSB1Type5 = attValue;
                    break;
                case cTSB1RelLabel5:
                    this.TSB1RelLabel5 = attValue;
                    break;
                case cTSB1TAmount5:
                    this.TSB1TAmount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit5:
                    this.TSB1TUnit5 = attValue;
                    break;
                case cTSB1TD1Amount5:
                    this.TSB1TD1Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit5:
                    this.TSB1TD1Unit5 = attValue;
                    break;
                case cTSB1TD2Amount5:
                    this.TSB1TD2Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit5:
                    this.TSB1TD2Unit5 = attValue;
                    break;
                case cTSB1MathResult5:
                    this.TSB1MathResult5 = attValue;
                    break;
                case cTSB1MathSubType5:
                    this.TSB1MathResult5 = attValue;
                    break;
                case cTSB1TMAmount5:
                    this.TSB1TMAmount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit5:
                    this.TSB1TMUnit5 = attValue;
                    break;
                case cTSB1TLAmount5:
                    this.TSB1TLAmount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit5:
                    this.TSB1TLUnit5 = attValue;
                    break;
                case cTSB1TUAmount5:
                    this.TSB1TUAmount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit5:
                    this.TSB1TUUnit5 = attValue;
                    break;
                case cTSB1MathOperator5:
                    this.TSB1MathOperator5 = attValue;
                    break;
                case cTSB1MathExpression5:
                    this.TSB1MathExpression5 = attValue;
                    break;
                case cTSB1N5:
                    this.TSB1N5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date5:
                    this.TSB1Date5 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType5:
                    this.TSB1MathType5 = attValue;
                    break;
                case cTSB11Amount5:
                    this.TSB11Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit5:
                    this.TSB11Unit5 = attValue;
                    break;
                case cTSB12Amount5:
                    this.TSB12Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit5:
                    this.TSB12Unit5 = attValue;
                    break;
                case cTSB15Amount5:
                    this.TSB15Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit5:
                    this.TSB15Unit5 = attValue;
                    break;
                case cTSB13Amount5:
                    this.TSB13Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit5:
                    this.TSB13Unit5 = attValue;
                    break;
                case cTSB14Amount5:
                    this.TSB14Amount5 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit5:
                    this.TSB14Unit5 = attValue;
                    break;
                case cTSB1Description6:
                    this.TSB1Description6 = attValue;
                    break;
                case cTSB1Name6:
                    this.TSB1Name6 = attValue;
                    break;
                case cTSB1Label6:
                    this.TSB1Label6 = attValue;
                    break;
                case cTSB1Type6:
                    this.TSB1Type6 = attValue;
                    break;
                case cTSB1RelLabel6:
                    this.TSB1RelLabel6 = attValue;
                    break;
                case cTSB1TAmount6:
                    this.TSB1TAmount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit6:
                    this.TSB1TUnit6 = attValue;
                    break;
                case cTSB1TD1Amount6:
                    this.TSB1TD1Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit6:
                    this.TSB1TD1Unit6 = attValue;
                    break;
                case cTSB1TD2Amount6:
                    this.TSB1TD2Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit6:
                    this.TSB1TD2Unit6 = attValue;
                    break;
                case cTSB1MathResult6:
                    this.TSB1MathResult6 = attValue;
                    break;
                case cTSB1MathSubType6:
                    this.TSB1MathResult6 = attValue;
                    break;
                case cTSB1TMAmount6:
                    this.TSB1TMAmount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit6:
                    this.TSB1TMUnit6 = attValue;
                    break;
                case cTSB1TLAmount6:
                    this.TSB1TLAmount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit6:
                    this.TSB1TLUnit6 = attValue;
                    break;
                case cTSB1TUAmount6:
                    this.TSB1TUAmount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit6:
                    this.TSB1TUUnit6 = attValue;
                    break;
                case cTSB1MathOperator6:
                    this.TSB1MathOperator6 = attValue;
                    break;
                case cTSB1MathExpression6:
                    this.TSB1MathExpression6 = attValue;
                    break;
                case cTSB1N6:
                    this.TSB1N6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date6:
                    this.TSB1Date6 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType6:
                    this.TSB1MathType6 = attValue;
                    break;
                case cTSB11Amount6:
                    this.TSB11Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit6:
                    this.TSB11Unit6 = attValue;
                    break;
                case cTSB12Amount6:
                    this.TSB12Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit6:
                    this.TSB12Unit6 = attValue;
                    break;
                case cTSB15Amount6:
                    this.TSB15Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit6:
                    this.TSB15Unit6 = attValue;
                    break;
                case cTSB13Amount6:
                    this.TSB13Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit6:
                    this.TSB13Unit6 = attValue;
                    break;
                case cTSB14Amount6:
                    this.TSB14Amount6 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit6:
                    this.TSB14Unit6 = attValue;
                    break;
                case cTSB1Description7:
                    this.TSB1Description7 = attValue;
                    break;
                case cTSB1Name7:
                    this.TSB1Name7 = attValue;
                    break;
                case cTSB1Label7:
                    this.TSB1Label7 = attValue;
                    break;
                case cTSB1Type7:
                    this.TSB1Type7 = attValue;
                    break;
                case cTSB1RelLabel7:
                    this.TSB1RelLabel7 = attValue;
                    break;
                case cTSB1TAmount7:
                    this.TSB1TAmount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit7:
                    this.TSB1TUnit7 = attValue;
                    break;
                case cTSB1TD1Amount7:
                    this.TSB1TD1Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit7:
                    this.TSB1TD1Unit7 = attValue;
                    break;
                case cTSB1TD2Amount7:
                    this.TSB1TD2Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit7:
                    this.TSB1TD2Unit7 = attValue;
                    break;
                case cTSB1MathResult7:
                    this.TSB1MathResult7 = attValue;
                    break;
                case cTSB1MathSubType7:
                    this.TSB1MathResult7 = attValue;
                    break;
                case cTSB1TMAmount7:
                    this.TSB1TMAmount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit7:
                    this.TSB1TMUnit7 = attValue;
                    break;
                case cTSB1TLAmount7:
                    this.TSB1TLAmount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit7:
                    this.TSB1TLUnit7 = attValue;
                    break;
                case cTSB1TUAmount7:
                    this.TSB1TUAmount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit7:
                    this.TSB1TUUnit7 = attValue;
                    break;
                case cTSB1MathOperator7:
                    this.TSB1MathOperator7 = attValue;
                    break;
                case cTSB1MathExpression7:
                    this.TSB1MathExpression7 = attValue;
                    break;
                case cTSB1N7:
                    this.TSB1N7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date7:
                    this.TSB1Date7 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType7:
                    this.TSB1MathType7 = attValue;
                    break;
                case cTSB11Amount7:
                    this.TSB11Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit7:
                    this.TSB11Unit7 = attValue;
                    break;
                case cTSB12Amount7:
                    this.TSB12Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit7:
                    this.TSB12Unit7 = attValue;
                    break;
                case cTSB15Amount7:
                    this.TSB15Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit7:
                    this.TSB15Unit7 = attValue;
                    break;
                case cTSB13Amount7:
                    this.TSB13Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit7:
                    this.TSB13Unit7 = attValue;
                    break;
                case cTSB14Amount7:
                    this.TSB14Amount7 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit7:
                    this.TSB14Unit7 = attValue;
                    break;
                case cTSB1Description8:
                    this.TSB1Description8 = attValue;
                    break;
                case cTSB1Name8:
                    this.TSB1Name8 = attValue;
                    break;
                case cTSB1Label8:
                    this.TSB1Label8 = attValue;
                    break;
                case cTSB1Type8:
                    this.TSB1Type8 = attValue;
                    break;
                case cTSB1RelLabel8:
                    this.TSB1RelLabel8 = attValue;
                    break;
                case cTSB1TAmount8:
                    this.TSB1TAmount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit8:
                    this.TSB1TUnit8 = attValue;
                    break;
                case cTSB1TD1Amount8:
                    this.TSB1TD1Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit8:
                    this.TSB1TD1Unit8 = attValue;
                    break;
                case cTSB1TD2Amount8:
                    this.TSB1TD2Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit8:
                    this.TSB1TD2Unit8 = attValue;
                    break;
                case cTSB1MathResult8:
                    this.TSB1MathResult8 = attValue;
                    break;
                case cTSB1MathSubType8:
                    this.TSB1MathResult8 = attValue;
                    break;
                case cTSB1TMAmount8:
                    this.TSB1TMAmount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit8:
                    this.TSB1TMUnit8 = attValue;
                    break;
                case cTSB1TLAmount8:
                    this.TSB1TLAmount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit8:
                    this.TSB1TLUnit8 = attValue;
                    break;
                case cTSB1TUAmount8:
                    this.TSB1TUAmount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit8:
                    this.TSB1TUUnit8 = attValue;
                    break;
                case cTSB1MathOperator8:
                    this.TSB1MathOperator8 = attValue;
                    break;
                case cTSB1MathExpression8:
                    this.TSB1MathExpression8 = attValue;
                    break;
                case cTSB1N8:
                    this.TSB1N8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date8:
                    this.TSB1Date8 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType8:
                    this.TSB1MathType8 = attValue;
                    break;
                case cTSB11Amount8:
                    this.TSB11Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit8:
                    this.TSB11Unit8 = attValue;
                    break;
                case cTSB12Amount8:
                    this.TSB12Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit8:
                    this.TSB12Unit8 = attValue;
                    break;
                case cTSB15Amount8:
                    this.TSB15Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit8:
                    this.TSB15Unit8 = attValue;
                    break;
                case cTSB13Amount8:
                    this.TSB13Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit8:
                    this.TSB13Unit8 = attValue;
                    break;
                case cTSB14Amount8:
                    this.TSB14Amount8 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit8:
                    this.TSB14Unit8 = attValue;
                    break;
                case cTSB1Description9:
                    this.TSB1Description9 = attValue;
                    break;
                case cTSB1Name9:
                    this.TSB1Name9 = attValue;
                    break;
                case cTSB1Label9:
                    this.TSB1Label9 = attValue;
                    break;
                case cTSB1Type9:
                    this.TSB1Type9 = attValue;
                    break;
                case cTSB1RelLabel9:
                    this.TSB1RelLabel9 = attValue;
                    break;
                case cTSB1TAmount9:
                    this.TSB1TAmount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit9:
                    this.TSB1TUnit9 = attValue;
                    break;
                case cTSB1TD1Amount9:
                    this.TSB1TD1Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit9:
                    this.TSB1TD1Unit9 = attValue;
                    break;
                case cTSB1TD2Amount9:
                    this.TSB1TD2Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit9:
                    this.TSB1TD2Unit9 = attValue;
                    break;
                case cTSB1MathResult9:
                    this.TSB1MathResult9 = attValue;
                    break;
                case cTSB1MathSubType9:
                    this.TSB1MathResult9 = attValue;
                    break;
                case cTSB1TMAmount9:
                    this.TSB1TMAmount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit9:
                    this.TSB1TMUnit9 = attValue;
                    break;
                case cTSB1TLAmount9:
                    this.TSB1TLAmount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit9:
                    this.TSB1TLUnit9 = attValue;
                    break;
                case cTSB1TUAmount9:
                    this.TSB1TUAmount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit9:
                    this.TSB1TUUnit9 = attValue;
                    break;
                case cTSB1MathOperator9:
                    this.TSB1MathOperator9 = attValue;
                    break;
                case cTSB1MathExpression9:
                    this.TSB1MathExpression9 = attValue;
                    break;
                case cTSB1N9:
                    this.TSB1N9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date9:
                    this.TSB1Date9 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType9:
                    this.TSB1MathType9 = attValue;
                    break;
                case cTSB11Amount9:
                    this.TSB11Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit9:
                    this.TSB11Unit9 = attValue;
                    break;
                case cTSB12Amount9:
                    this.TSB12Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit9:
                    this.TSB12Unit9 = attValue;
                    break;
                case cTSB15Amount9:
                    this.TSB15Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit9:
                    this.TSB15Unit9 = attValue;
                    break;
                case cTSB13Amount9:
                    this.TSB13Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit9:
                    this.TSB13Unit9 = attValue;
                    break;
                case cTSB14Amount9:
                    this.TSB14Amount9 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit9:
                    this.TSB14Unit9 = attValue;
                    break;
                case cTSB1Description10:
                    this.TSB1Description10 = attValue;
                    break;
                case cTSB1Name10:
                    this.TSB1Name10 = attValue;
                    break;
                case cTSB1Label10:
                    this.TSB1Label10 = attValue;
                    break;
                case cTSB1Type10:
                    this.TSB1Type10 = attValue;
                    break;
                case cTSB1RelLabel10:
                    this.TSB1RelLabel10 = attValue;
                    break;
                case cTSB1TAmount10:
                    this.TSB1TAmount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit10:
                    this.TSB1TUnit10 = attValue;
                    break;
                case cTSB1TD1Amount10:
                    this.TSB1TD1Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit10:
                    this.TSB1TD1Unit10 = attValue;
                    break;
                case cTSB1TD2Amount10:
                    this.TSB1TD2Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit10:
                    this.TSB1TD2Unit10 = attValue;
                    break;
                case cTSB1MathResult10:
                    this.TSB1MathResult10 = attValue;
                    break;
                case cTSB1MathSubType10:
                    this.TSB1MathResult10 = attValue;
                    break;
                case cTSB1TMAmount10:
                    this.TSB1TMAmount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit10:
                    this.TSB1TMUnit10 = attValue;
                    break;
                case cTSB1TLAmount10:
                    this.TSB1TLAmount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit10:
                    this.TSB1TLUnit10 = attValue;
                    break;
                case cTSB1TUAmount10:
                    this.TSB1TUAmount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit10:
                    this.TSB1TUUnit10 = attValue;
                    break;
                case cTSB1MathOperator10:
                    this.TSB1MathOperator10 = attValue;
                    break;
                case cTSB1MathExpression10:
                    this.TSB1MathExpression10 = attValue;
                    break;
                case cTSB1N10:
                    this.TSB1N10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date10:
                    this.TSB1Date10 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType10:
                    this.TSB1MathType10 = attValue;
                    break;
                case cTSB11Amount10:
                    this.TSB11Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit10:
                    this.TSB11Unit10 = attValue;
                    break;
                case cTSB12Amount10:
                    this.TSB12Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit10:
                    this.TSB12Unit10 = attValue;
                    break;
                case cTSB15Amount10:
                    this.TSB15Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit10:
                    this.TSB15Unit10 = attValue;
                    break;
                case cTSB13Amount10:
                    this.TSB13Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit10:
                    this.TSB13Unit10 = attValue;
                    break;
                case cTSB14Amount10:
                    this.TSB14Amount10 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit10:
                    this.TSB14Unit10 = attValue;
                    break;
                case cTSB1Description11:
                    this.TSB1Description11 = attValue;
                    break;
                case cTSB1Name11:
                    this.TSB1Name11 = attValue;
                    break;
                case cTSB1Label11:
                    this.TSB1Label11 = attValue;
                    break;
                case cTSB1Type11:
                    this.TSB1Type11 = attValue;
                    break;
                case cTSB1RelLabel11:
                    this.TSB1RelLabel11 = attValue;
                    break;
                case cTSB1TAmount11:
                    this.TSB1TAmount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit11:
                    this.TSB1TUnit11 = attValue;
                    break;
                case cTSB1TD1Amount11:
                    this.TSB1TD1Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit11:
                    this.TSB1TD1Unit11 = attValue;
                    break;
                case cTSB1TD2Amount11:
                    this.TSB1TD2Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit11:
                    this.TSB1TD2Unit11 = attValue;
                    break;
                case cTSB1MathResult11:
                    this.TSB1MathResult11 = attValue;
                    break;
                case cTSB1MathSubType11:
                    this.TSB1MathResult11 = attValue;
                    break;
                case cTSB1TMAmount11:
                    this.TSB1TMAmount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit11:
                    this.TSB1TMUnit11 = attValue;
                    break;
                case cTSB1TLAmount11:
                    this.TSB1TLAmount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit11:
                    this.TSB1TLUnit11 = attValue;
                    break;
                case cTSB1TUAmount11:
                    this.TSB1TUAmount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit11:
                    this.TSB1TUUnit11 = attValue;
                    break;
                case cTSB1MathOperator11:
                    this.TSB1MathOperator11 = attValue;
                    break;
                case cTSB1MathExpression11:
                    this.TSB1MathExpression11 = attValue;
                    break;
                case cTSB1N11:
                    this.TSB1N11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date11:
                    this.TSB1Date11 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType11:
                    this.TSB1MathType11 = attValue;
                    break;
                case cTSB11Amount11:
                    this.TSB11Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit11:
                    this.TSB11Unit11 = attValue;
                    break;
                case cTSB12Amount11:
                    this.TSB12Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit11:
                    this.TSB12Unit11 = attValue;
                    break;
                case cTSB15Amount11:
                    this.TSB15Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit11:
                    this.TSB15Unit11 = attValue;
                    break;
                case cTSB13Amount11:
                    this.TSB13Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit11:
                    this.TSB13Unit11 = attValue;
                    break;
                case cTSB14Amount11:
                    this.TSB14Amount11 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit11:
                    this.TSB14Unit11 = attValue;
                    break;
                case cTSB1Description12:
                    this.TSB1Description12 = attValue;
                    break;
                case cTSB1Name12:
                    this.TSB1Name12 = attValue;
                    break;
                case cTSB1Label12:
                    this.TSB1Label12 = attValue;
                    break;
                case cTSB1Type12:
                    this.TSB1Type12 = attValue;
                    break;
                case cTSB1RelLabel12:
                    this.TSB1RelLabel12 = attValue;
                    break;
                case cTSB1TAmount12:
                    this.TSB1TAmount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit12:
                    this.TSB1TUnit12 = attValue;
                    break;
                case cTSB1TD1Amount12:
                    this.TSB1TD1Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit12:
                    this.TSB1TD1Unit12 = attValue;
                    break;
                case cTSB1TD2Amount12:
                    this.TSB1TD2Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit12:
                    this.TSB1TD2Unit12 = attValue;
                    break;
                case cTSB1MathResult12:
                    this.TSB1MathResult12 = attValue;
                    break;
                case cTSB1MathSubType12:
                    this.TSB1MathResult12 = attValue;
                    break;
                case cTSB1TMAmount12:
                    this.TSB1TMAmount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit12:
                    this.TSB1TMUnit12 = attValue;
                    break;
                case cTSB1TLAmount12:
                    this.TSB1TLAmount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit12:
                    this.TSB1TLUnit12 = attValue;
                    break;
                case cTSB1TUAmount12:
                    this.TSB1TUAmount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit12:
                    this.TSB1TUUnit12 = attValue;
                    break;
                case cTSB1MathOperator12:
                    this.TSB1MathOperator12 = attValue;
                    break;
                case cTSB1MathExpression12:
                    this.TSB1MathExpression12 = attValue;
                    break;
                case cTSB1N12:
                    this.TSB1N12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date12:
                    this.TSB1Date12 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType12:
                    this.TSB1MathType12 = attValue;
                    break;
                case cTSB11Amount12:
                    this.TSB11Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit12:
                    this.TSB11Unit12 = attValue;
                    break;
                case cTSB12Amount12:
                    this.TSB12Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit12:
                    this.TSB12Unit12 = attValue;
                    break;
                case cTSB15Amount12:
                    this.TSB15Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit12:
                    this.TSB15Unit12 = attValue;
                    break;
                case cTSB13Amount12:
                    this.TSB13Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit12:
                    this.TSB13Unit12 = attValue;
                    break;
                case cTSB14Amount12:
                    this.TSB14Amount12 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit12:
                    this.TSB14Unit12 = attValue;
                    break;
                case cTSB1Description13:
                    this.TSB1Description13 = attValue;
                    break;
                case cTSB1Name13:
                    this.TSB1Name13 = attValue;
                    break;
                case cTSB1Label13:
                    this.TSB1Label13 = attValue;
                    break;
                case cTSB1Type13:
                    this.TSB1Type13 = attValue;
                    break;
                case cTSB1RelLabel13:
                    this.TSB1RelLabel13 = attValue;
                    break;
                case cTSB1TAmount13:
                    this.TSB1TAmount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit13:
                    this.TSB1TUnit13 = attValue;
                    break;
                case cTSB1TD1Amount13:
                    this.TSB1TD1Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit13:
                    this.TSB1TD1Unit13 = attValue;
                    break;
                case cTSB1TD2Amount13:
                    this.TSB1TD2Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit13:
                    this.TSB1TD2Unit13 = attValue;
                    break;
                case cTSB1MathResult13:
                    this.TSB1MathResult13 = attValue;
                    break;
                case cTSB1MathSubType13:
                    this.TSB1MathResult13 = attValue;
                    break;
                case cTSB1TMAmount13:
                    this.TSB1TMAmount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit13:
                    this.TSB1TMUnit13 = attValue;
                    break;
                case cTSB1TLAmount13:
                    this.TSB1TLAmount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit13:
                    this.TSB1TLUnit13 = attValue;
                    break;
                case cTSB1TUAmount13:
                    this.TSB1TUAmount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit13:
                    this.TSB1TUUnit13 = attValue;
                    break;
                case cTSB1MathOperator13:
                    this.TSB1MathOperator13 = attValue;
                    break;
                case cTSB1MathExpression13:
                    this.TSB1MathExpression13 = attValue;
                    break;
                case cTSB1N13:
                    this.TSB1N13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date13:
                    this.TSB1Date13 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType13:
                    this.TSB1MathType13 = attValue;
                    break;
                case cTSB11Amount13:
                    this.TSB11Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit13:
                    this.TSB11Unit13 = attValue;
                    break;
                case cTSB12Amount13:
                    this.TSB12Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit13:
                    this.TSB12Unit13 = attValue;
                    break;
                case cTSB15Amount13:
                    this.TSB15Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit13:
                    this.TSB15Unit13 = attValue;
                    break;
                case cTSB13Amount13:
                    this.TSB13Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit13:
                    this.TSB13Unit13 = attValue;
                    break;
                case cTSB14Amount13:
                    this.TSB14Amount13 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit13:
                    this.TSB14Unit13 = attValue;
                    break;
                case cTSB1Description14:
                    this.TSB1Description14 = attValue;
                    break;
                case cTSB1Name14:
                    this.TSB1Name14 = attValue;
                    break;
                case cTSB1Label14:
                    this.TSB1Label14 = attValue;
                    break;
                case cTSB1Type14:
                    this.TSB1Type14 = attValue;
                    break;
                case cTSB1RelLabel14:
                    this.TSB1RelLabel14 = attValue;
                    break;
                case cTSB1TAmount14:
                    this.TSB1TAmount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit14:
                    this.TSB1TUnit14 = attValue;
                    break;
                case cTSB1TD1Amount14:
                    this.TSB1TD1Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit14:
                    this.TSB1TD1Unit14 = attValue;
                    break;
                case cTSB1TD2Amount14:
                    this.TSB1TD2Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit14:
                    this.TSB1TD2Unit14 = attValue;
                    break;
                case cTSB1MathResult14:
                    this.TSB1MathResult14 = attValue;
                    break;
                case cTSB1MathSubType14:
                    this.TSB1MathResult14 = attValue;
                    break;
                case cTSB1TMAmount14:
                    this.TSB1TMAmount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit14:
                    this.TSB1TMUnit14 = attValue;
                    break;
                case cTSB1TLAmount14:
                    this.TSB1TLAmount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit14:
                    this.TSB1TLUnit14 = attValue;
                    break;
                case cTSB1TUAmount14:
                    this.TSB1TUAmount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit14:
                    this.TSB1TUUnit14 = attValue;
                    break;
                case cTSB1MathOperator14:
                    this.TSB1MathOperator14 = attValue;
                    break;
                case cTSB1MathExpression14:
                    this.TSB1MathExpression14 = attValue;
                    break;
                case cTSB1N14:
                    this.TSB1N14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date14:
                    this.TSB1Date14 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType14:
                    this.TSB1MathType14 = attValue;
                    break;
                case cTSB11Amount14:
                    this.TSB11Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit14:
                    this.TSB11Unit14 = attValue;
                    break;
                case cTSB12Amount14:
                    this.TSB12Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit14:
                    this.TSB12Unit14 = attValue;
                    break;
                case cTSB15Amount14:
                    this.TSB15Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit14:
                    this.TSB15Unit14 = attValue;
                    break;
                case cTSB13Amount14:
                    this.TSB13Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit14:
                    this.TSB13Unit14 = attValue;
                    break;
                case cTSB14Amount14:
                    this.TSB14Amount14 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit14:
                    this.TSB14Unit14 = attValue;
                    break;
                case cTSB1Description15:
                    this.TSB1Description15 = attValue;
                    break;
                case cTSB1Name15:
                    this.TSB1Name15 = attValue;
                    break;
                case cTSB1Label15:
                    this.TSB1Label15 = attValue;
                    break;
                case cTSB1Type15:
                    this.TSB1Type15 = attValue;
                    break;
                case cTSB1RelLabel15:
                    this.TSB1RelLabel15 = attValue;
                    break;
                case cTSB1TAmount15:
                    this.TSB1TAmount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit15:
                    this.TSB1TUnit15 = attValue;
                    break;
                case cTSB1TD1Amount15:
                    this.TSB1TD1Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit15:
                    this.TSB1TD1Unit15 = attValue;
                    break;
                case cTSB1TD2Amount15:
                    this.TSB1TD2Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit15:
                    this.TSB1TD2Unit15 = attValue;
                    break;
                case cTSB1MathResult15:
                    this.TSB1MathResult15 = attValue;
                    break;
                case cTSB1MathSubType15:
                    this.TSB1MathResult15 = attValue;
                    break;
                case cTSB1TMAmount15:
                    this.TSB1TMAmount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit15:
                    this.TSB1TMUnit15 = attValue;
                    break;
                case cTSB1TLAmount15:
                    this.TSB1TLAmount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit15:
                    this.TSB1TLUnit15 = attValue;
                    break;
                case cTSB1TUAmount15:
                    this.TSB1TUAmount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit15:
                    this.TSB1TUUnit15 = attValue;
                    break;
                case cTSB1MathOperator15:
                    this.TSB1MathOperator15 = attValue;
                    break;
                case cTSB1MathExpression15:
                    this.TSB1MathExpression15 = attValue;
                    break;
                case cTSB1N15:
                    this.TSB1N15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date15:
                    this.TSB1Date15 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType15:
                    this.TSB1MathType15 = attValue;
                    break;
                case cTSB11Amount15:
                    this.TSB11Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit15:
                    this.TSB11Unit15 = attValue;
                    break;
                case cTSB12Amount15:
                    this.TSB12Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit15:
                    this.TSB12Unit15 = attValue;
                    break;
                case cTSB15Amount15:
                    this.TSB15Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit15:
                    this.TSB15Unit15 = attValue;
                    break;
                case cTSB13Amount15:
                    this.TSB13Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit15:
                    this.TSB13Unit15 = attValue;
                    break;
                case cTSB14Amount15:
                    this.TSB14Amount15 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit15:
                    this.TSB14Unit15 = attValue;
                    break;
                case cTSB1Description16:
                    this.TSB1Description16 = attValue;
                    break;
                case cTSB1Name16:
                    this.TSB1Name16 = attValue;
                    break;
                case cTSB1Label16:
                    this.TSB1Label16 = attValue;
                    break;
                case cTSB1Type16:
                    this.TSB1Type16 = attValue;
                    break;
                case cTSB1RelLabel16:
                    this.TSB1RelLabel16 = attValue;
                    break;
                case cTSB1TAmount16:
                    this.TSB1TAmount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit16:
                    this.TSB1TUnit16 = attValue;
                    break;
                case cTSB1TD1Amount16:
                    this.TSB1TD1Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit16:
                    this.TSB1TD1Unit16 = attValue;
                    break;
                case cTSB1TD2Amount16:
                    this.TSB1TD2Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit16:
                    this.TSB1TD2Unit16 = attValue;
                    break;
                case cTSB1MathResult16:
                    this.TSB1MathResult16 = attValue;
                    break;
                case cTSB1MathSubType16:
                    this.TSB1MathResult16 = attValue;
                    break;
                case cTSB1TMAmount16:
                    this.TSB1TMAmount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit16:
                    this.TSB1TMUnit16 = attValue;
                    break;
                case cTSB1TLAmount16:
                    this.TSB1TLAmount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit16:
                    this.TSB1TLUnit16 = attValue;
                    break;
                case cTSB1TUAmount16:
                    this.TSB1TUAmount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit16:
                    this.TSB1TUUnit16 = attValue;
                    break;
                case cTSB1MathOperator16:
                    this.TSB1MathOperator16 = attValue;
                    break;
                case cTSB1MathExpression16:
                    this.TSB1MathExpression16 = attValue;
                    break;
                case cTSB1N16:
                    this.TSB1N16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date16:
                    this.TSB1Date16 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType16:
                    this.TSB1MathType16 = attValue;
                    break;
                case cTSB11Amount16:
                    this.TSB11Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit16:
                    this.TSB11Unit16 = attValue;
                    break;
                case cTSB12Amount16:
                    this.TSB12Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit16:
                    this.TSB12Unit16 = attValue;
                    break;
                case cTSB15Amount16:
                    this.TSB15Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit16:
                    this.TSB15Unit16 = attValue;
                    break;
                case cTSB13Amount16:
                    this.TSB13Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit16:
                    this.TSB13Unit16 = attValue;
                    break;
                case cTSB14Amount16:
                    this.TSB14Amount16 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit16:
                    this.TSB14Unit16 = attValue;
                    break;
                case cTSB1Description17:
                    this.TSB1Description17 = attValue;
                    break;
                case cTSB1Name17:
                    this.TSB1Name17 = attValue;
                    break;
                case cTSB1Label17:
                    this.TSB1Label17 = attValue;
                    break;
                case cTSB1Type17:
                    this.TSB1Type17 = attValue;
                    break;
                case cTSB1RelLabel17:
                    this.TSB1RelLabel17 = attValue;
                    break;
                case cTSB1TAmount17:
                    this.TSB1TAmount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit17:
                    this.TSB1TUnit17 = attValue;
                    break;
                case cTSB1TD1Amount17:
                    this.TSB1TD1Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit17:
                    this.TSB1TD1Unit17 = attValue;
                    break;
                case cTSB1TD2Amount17:
                    this.TSB1TD2Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit17:
                    this.TSB1TD2Unit17 = attValue;
                    break;
                case cTSB1MathResult17:
                    this.TSB1MathResult17 = attValue;
                    break;
                case cTSB1MathSubType17:
                    this.TSB1MathResult17 = attValue;
                    break;
                case cTSB1TMAmount17:
                    this.TSB1TMAmount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit17:
                    this.TSB1TMUnit17 = attValue;
                    break;
                case cTSB1TLAmount17:
                    this.TSB1TLAmount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit17:
                    this.TSB1TLUnit17 = attValue;
                    break;
                case cTSB1TUAmount17:
                    this.TSB1TUAmount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit17:
                    this.TSB1TUUnit17 = attValue;
                    break;
                case cTSB1MathOperator17:
                    this.TSB1MathOperator17 = attValue;
                    break;
                case cTSB1MathExpression17:
                    this.TSB1MathExpression17 = attValue;
                    break;
                case cTSB1N17:
                    this.TSB1N17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date17:
                    this.TSB1Date17 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType17:
                    this.TSB1MathType17 = attValue;
                    break;
                case cTSB11Amount17:
                    this.TSB11Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit17:
                    this.TSB11Unit17 = attValue;
                    break;
                case cTSB12Amount17:
                    this.TSB12Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit17:
                    this.TSB12Unit17 = attValue;
                    break;
                case cTSB15Amount17:
                    this.TSB15Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit17:
                    this.TSB15Unit17 = attValue;
                    break;
                case cTSB13Amount17:
                    this.TSB13Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit17:
                    this.TSB13Unit17 = attValue;
                    break;
                case cTSB14Amount17:
                    this.TSB14Amount17 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit17:
                    this.TSB14Unit17 = attValue;
                    break;
                case cTSB1Description18:
                    this.TSB1Description18 = attValue;
                    break;
                case cTSB1Name18:
                    this.TSB1Name18 = attValue;
                    break;
                case cTSB1Label18:
                    this.TSB1Label18 = attValue;
                    break;
                case cTSB1Type18:
                    this.TSB1Type18 = attValue;
                    break;
                case cTSB1RelLabel18:
                    this.TSB1RelLabel18 = attValue;
                    break;
                case cTSB1TAmount18:
                    this.TSB1TAmount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit18:
                    this.TSB1TUnit18 = attValue;
                    break;
                case cTSB1TD1Amount18:
                    this.TSB1TD1Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit18:
                    this.TSB1TD1Unit18 = attValue;
                    break;
                case cTSB1TD2Amount18:
                    this.TSB1TD2Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit18:
                    this.TSB1TD2Unit18 = attValue;
                    break;
                case cTSB1MathResult18:
                    this.TSB1MathResult18 = attValue;
                    break;
                case cTSB1MathSubType18:
                    this.TSB1MathResult18 = attValue;
                    break;
                case cTSB1TMAmount18:
                    this.TSB1TMAmount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit18:
                    this.TSB1TMUnit18 = attValue;
                    break;
                case cTSB1TLAmount18:
                    this.TSB1TLAmount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit18:
                    this.TSB1TLUnit18 = attValue;
                    break;
                case cTSB1TUAmount18:
                    this.TSB1TUAmount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit18:
                    this.TSB1TUUnit18 = attValue;
                    break;
                case cTSB1MathOperator18:
                    this.TSB1MathOperator18 = attValue;
                    break;
                case cTSB1MathExpression18:
                    this.TSB1MathExpression18 = attValue;
                    break;
                case cTSB1N18:
                    this.TSB1N18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date18:
                    this.TSB1Date18 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType18:
                    this.TSB1MathType18 = attValue;
                    break;
                case cTSB11Amount18:
                    this.TSB11Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit18:
                    this.TSB11Unit18 = attValue;
                    break;
                case cTSB12Amount18:
                    this.TSB12Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit18:
                    this.TSB12Unit18 = attValue;
                    break;
                case cTSB15Amount18:
                    this.TSB15Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit18:
                    this.TSB15Unit18 = attValue;
                    break;
                case cTSB13Amount18:
                    this.TSB13Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit18:
                    this.TSB13Unit18 = attValue;
                    break;
                case cTSB14Amount18:
                    this.TSB14Amount18 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit18:
                    this.TSB14Unit18 = attValue;
                    break;
                case cTSB1Description19:
                    this.TSB1Description19 = attValue;
                    break;
                case cTSB1Name19:
                    this.TSB1Name19 = attValue;
                    break;
                case cTSB1Label19:
                    this.TSB1Label19 = attValue;
                    break;
                case cTSB1Type19:
                    this.TSB1Type19 = attValue;
                    break;
                case cTSB1RelLabel19:
                    this.TSB1RelLabel19 = attValue;
                    break;
                case cTSB1TAmount19:
                    this.TSB1TAmount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit19:
                    this.TSB1TUnit19 = attValue;
                    break;
                case cTSB1TD1Amount19:
                    this.TSB1TD1Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit19:
                    this.TSB1TD1Unit19 = attValue;
                    break;
                case cTSB1TD2Amount19:
                    this.TSB1TD2Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit19:
                    this.TSB1TD2Unit19 = attValue;
                    break;
                case cTSB1MathResult19:
                    this.TSB1MathResult19 = attValue;
                    break;
                case cTSB1MathSubType19:
                    this.TSB1MathResult19 = attValue;
                    break;
                case cTSB1TMAmount19:
                    this.TSB1TMAmount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit19:
                    this.TSB1TMUnit19 = attValue;
                    break;
                case cTSB1TLAmount19:
                    this.TSB1TLAmount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit19:
                    this.TSB1TLUnit19 = attValue;
                    break;
                case cTSB1TUAmount19:
                    this.TSB1TUAmount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit19:
                    this.TSB1TUUnit19 = attValue;
                    break;
                case cTSB1MathOperator19:
                    this.TSB1MathOperator19 = attValue;
                    break;
                case cTSB1MathExpression19:
                    this.TSB1MathExpression19 = attValue;
                    break;
                case cTSB1N19:
                    this.TSB1N19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date19:
                    this.TSB1Date19 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType19:
                    this.TSB1MathType19 = attValue;
                    break;
                case cTSB11Amount19:
                    this.TSB11Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit19:
                    this.TSB11Unit19 = attValue;
                    break;
                case cTSB12Amount19:
                    this.TSB12Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit19:
                    this.TSB12Unit19 = attValue;
                    break;
                case cTSB15Amount19:
                    this.TSB15Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit19:
                    this.TSB15Unit19 = attValue;
                    break;
                case cTSB13Amount19:
                    this.TSB13Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit19:
                    this.TSB13Unit19 = attValue;
                    break;
                case cTSB14Amount19:
                    this.TSB14Amount19 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit19:
                    this.TSB14Unit19 = attValue;
                    break;
                case cTSB1Description20:
                    this.TSB1Description20 = attValue;
                    break;
                case cTSB1Name20:
                    this.TSB1Name20 = attValue;
                    break;
                case cTSB1Label20:
                    this.TSB1Label20 = attValue;
                    break;
                case cTSB1Type20:
                    this.TSB1Type20 = attValue;
                    break;
                case cTSB1RelLabel20:
                    this.TSB1RelLabel20 = attValue;
                    break;
                case cTSB1TAmount20:
                    this.TSB1TAmount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUnit20:
                    this.TSB1TUnit20 = attValue;
                    break;
                case cTSB1TD1Amount20:
                    this.TSB1TD1Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD1Unit20:
                    this.TSB1TD1Unit20 = attValue;
                    break;
                case cTSB1TD2Amount20:
                    this.TSB1TD2Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TD2Unit20:
                    this.TSB1TD2Unit20 = attValue;
                    break;
                case cTSB1MathResult20:
                    this.TSB1MathResult20 = attValue;
                    break;
                case cTSB1MathSubType20:
                    this.TSB1MathResult20 = attValue;
                    break;
                case cTSB1TMAmount20:
                    this.TSB1TMAmount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TMUnit20:
                    this.TSB1TMUnit20 = attValue;
                    break;
                case cTSB1TLAmount20:
                    this.TSB1TLAmount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TLUnit20:
                    this.TSB1TLUnit20 = attValue;
                    break;
                case cTSB1TUAmount20:
                    this.TSB1TUAmount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1TUUnit20:
                    this.TSB1TUUnit20 = attValue;
                    break;
                case cTSB1MathOperator20:
                    this.TSB1MathOperator20 = attValue;
                    break;
                case cTSB1MathExpression20:
                    this.TSB1MathExpression20 = attValue;
                    break;
                case cTSB1N20:
                    this.TSB1N20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB1Date20:
                    this.TSB1Date20 = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTSB1MathType20:
                    this.TSB1MathType20 = attValue;
                    break;
                case cTSB11Amount20:
                    this.TSB11Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB11Unit20:
                    this.TSB11Unit20 = attValue;
                    break;
                case cTSB12Amount20:
                    this.TSB12Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB12Unit20:
                    this.TSB12Unit20 = attValue;
                    break;
                case cTSB15Amount20:
                    this.TSB15Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB15Unit20:
                    this.TSB15Unit20 = attValue;
                    break;
                case cTSB13Amount20:
                    this.TSB13Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB13Unit20:
                    this.TSB13Unit20 = attValue;
                    break;
                case cTSB14Amount20:
                    this.TSB14Amount20 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTSB14Unit20:
                    this.TSB14Unit20 = attValue;
                    break;
                default:
                    break;
            }
        }
        public string GetTSB1BaseStockProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTSB1Score:
                    sPropertyValue = this.TSB1Score.ToString();
                    break;
                case cTSB1ScoreUnit:
                    sPropertyValue = this.TSB1ScoreUnit;
                    break;
                case cTSB1ScoreD1Amount:
                    sPropertyValue = this.TSB1ScoreD1Amount.ToString();
                    break;
                case cTSB1ScoreD1Unit:
                    sPropertyValue = this.TSB1ScoreD1Unit.ToString();
                    break;
                case cTSB1ScoreD2Amount:
                    sPropertyValue = this.TSB1ScoreD2Amount.ToString();
                    break;
                case cTSB1ScoreD2Unit:
                    sPropertyValue = this.TSB1ScoreD2Unit.ToString();
                    break;
                case cTSB1ScoreMathExpression:
                    sPropertyValue = this.TSB1ScoreMathExpression;
                    break;
                case cTSB1ScoreM:
                    sPropertyValue = this.TSB1ScoreM.ToString();
                    break;
                case cTSB1ScoreMUnit:
                    sPropertyValue = this.TSB1ScoreMUnit;
                    break;
                case cTSB1ScoreLAmount:
                    sPropertyValue = this.TSB1ScoreLAmount.ToString();
                    break;
                case cTSB1ScoreLUnit:
                    sPropertyValue = this.TSB1ScoreLUnit.ToString();
                    break;
                case cTSB1ScoreUAmount:
                    sPropertyValue = this.TSB1ScoreUAmount.ToString();
                    break;
                case cTSB1ScoreUUnit:
                    sPropertyValue = this.TSB1ScoreUUnit.ToString();
                    break;
                case cTSB1Iterations:
                    sPropertyValue = this.TSB1Iterations.ToString();
                    break;
                case cTSB1ScoreDistType:
                    sPropertyValue = this.TSB1ScoreDistType.ToString();
                    break;
                case cTSB1ScoreMathType:
                    sPropertyValue = this.TSB1ScoreMathType.ToString();
                    break;
                case cTSB1ScoreMathResult:
                    sPropertyValue = this.TSB1ScoreMathResult.ToString();
                    break;
                case cTSB1ScoreMathSubType:
                    sPropertyValue = this.TSB1ScoreMathSubType.ToString();
                    break;
                case cTSB1ScoreN:
                    sPropertyValue = this.TSB1ScoreN.ToString();
                    break;
                case cTSB1Description1:
                    sPropertyValue = this.TSB1Description1;
                    break;
                case cTSB1Name1:
                    sPropertyValue = this.TSB1Name1;
                    break;
                case cTSB1Type1:
                    sPropertyValue = this.TSB1Type1;
                    break;
                case cTSB1Label1:
                    sPropertyValue = this.TSB1Label1;
                    break;
                case cTSB1RelLabel1:
                    sPropertyValue = this.TSB1RelLabel1;
                    break;
                case cTSB1TAmount1:
                    sPropertyValue = this.TSB1TAmount1.ToString();
                    break;
                case cTSB1TUnit1:
                    sPropertyValue = this.TSB1TUnit1.ToString();
                    break;
                case cTSB1TD1Amount1:
                    sPropertyValue = this.TSB1TD1Amount1.ToString();
                    break;
                case cTSB1TD1Unit1:
                    sPropertyValue = this.TSB1TD1Unit1.ToString();
                    break;
                case cTSB1TD2Amount1:
                    sPropertyValue = this.TSB1TD2Amount1.ToString();
                    break;
                case cTSB1TD2Unit1:
                    sPropertyValue = this.TSB1TD2Unit1.ToString();
                    break;
                case cTSB1MathResult1:
                    sPropertyValue = this.TSB1MathResult1.ToString();
                    break;
                case cTSB1MathSubType1:
                    sPropertyValue = this.TSB1MathSubType1.ToString();
                    break;
                case cTSB1TMAmount1:
                    sPropertyValue = this.TSB1TMAmount1.ToString();
                    break;
                case cTSB1TMUnit1:
                    sPropertyValue = this.TSB1TMUnit1.ToString();
                    break;
                case cTSB1TLAmount1:
                    sPropertyValue = this.TSB1TLAmount1.ToString();
                    break;
                case cTSB1TLUnit1:
                    sPropertyValue = this.TSB1TLUnit1.ToString();
                    break;
                case cTSB1TUAmount1:
                    sPropertyValue = this.TSB1TUAmount1.ToString();
                    break;
                case cTSB1TUUnit1:
                    sPropertyValue = this.TSB1TUUnit1.ToString();
                    break;
                case cTSB1MathOperator1:
                    sPropertyValue = this.TSB1MathOperator1.ToString();
                    break;
                case cTSB1MathExpression1:
                    sPropertyValue = this.TSB1MathExpression1.ToString();
                    break;
                case cTSB1N1:
                    sPropertyValue = this.TSB1N1.ToString();
                    break;
                case cTSB1Date1:
                    sPropertyValue = this.TSB1Date1.ToString();
                    break;
                case cTSB1MathType1:
                    sPropertyValue = this.TSB1MathType1;
                    break;
                case cTSB11Amount1:
                    sPropertyValue = this.TSB11Amount1.ToString();
                    break;
                case cTSB11Unit1:
                    sPropertyValue = this.TSB11Unit1.ToString();
                    break;
                case cTSB12Amount1:
                    sPropertyValue = this.TSB12Amount1.ToString();
                    break;
                case cTSB12Unit1:
                    sPropertyValue = this.TSB12Unit1;
                    break;
                case cTSB15Amount1:
                    sPropertyValue = this.TSB15Amount1.ToString();
                    break;
                case cTSB15Unit1:
                    sPropertyValue = this.TSB15Unit1.ToString();
                    break;
                case cTSB13Amount1:
                    sPropertyValue = this.TSB13Amount1.ToString();
                    break;
                case cTSB13Unit1:
                    sPropertyValue = this.TSB13Unit1;
                    break;
                case cTSB14Amount1:
                    sPropertyValue = this.TSB14Amount1.ToString();
                    break;
                case cTSB14Unit1:
                    sPropertyValue = this.TSB14Unit1;
                    break;
                case cTSB1Description2:
                    sPropertyValue = this.TSB1Description2;
                    break;
                case cTSB1Name2:
                    sPropertyValue = this.TSB1Name2;
                    break;
                case cTSB1Label2:
                    sPropertyValue = this.TSB1Label2;
                    break;
                case cTSB1Type2:
                    sPropertyValue = this.TSB1Type2;
                    break;
                case cTSB1RelLabel2:
                    sPropertyValue = this.TSB1RelLabel2;
                    break;
                case cTSB1TAmount2:
                    sPropertyValue = this.TSB1TAmount2.ToString();
                    break;
                case cTSB1TUnit2:
                    sPropertyValue = this.TSB1TUnit2.ToString();
                    break;
                case cTSB1TD1Amount2:
                    sPropertyValue = this.TSB1TD1Amount2.ToString();
                    break;
                case cTSB1TD1Unit2:
                    sPropertyValue = this.TSB1TD1Unit2.ToString();
                    break;
                case cTSB1TD2Amount2:
                    sPropertyValue = this.TSB1TD2Amount2.ToString();
                    break;
                case cTSB1TD2Unit2:
                    sPropertyValue = this.TSB1TD2Unit2.ToString();
                    break;
                case cTSB1MathResult2:
                    sPropertyValue = this.TSB1MathResult2.ToString();
                    break;
                case cTSB1MathSubType2:
                    sPropertyValue = this.TSB1MathSubType2.ToString();
                    break;
                case cTSB1TMAmount2:
                    sPropertyValue = this.TSB1TMAmount2.ToString();
                    break;
                case cTSB1TMUnit2:
                    sPropertyValue = this.TSB1TMUnit2.ToString();
                    break;
                case cTSB1TLAmount2:
                    sPropertyValue = this.TSB1TLAmount2.ToString();
                    break;
                case cTSB1TLUnit2:
                    sPropertyValue = this.TSB1TLUnit2.ToString();
                    break;
                case cTSB1TUAmount2:
                    sPropertyValue = this.TSB1TUAmount2.ToString();
                    break;
                case cTSB1TUUnit2:
                    sPropertyValue = this.TSB1TUUnit2.ToString();
                    break;
                case cTSB1MathOperator2:
                    sPropertyValue = this.TSB1MathOperator2.ToString();
                    break;
                case cTSB1MathExpression2:
                    sPropertyValue = this.TSB1MathExpression2.ToString();
                    break;
                case cTSB1N2:
                    sPropertyValue = this.TSB1N2.ToString();
                    break;
                case cTSB1Date2:
                    sPropertyValue = this.TSB1Date2.ToString();
                    break;
                case cTSB1MathType2:
                    sPropertyValue = this.TSB1MathType2;
                    break;
                case cTSB11Amount2:
                    sPropertyValue = this.TSB11Amount2.ToString();
                    break;
                case cTSB11Unit2:
                    sPropertyValue = this.TSB11Unit2.ToString();
                    break;
                case cTSB12Amount2:
                    sPropertyValue = this.TSB12Amount2.ToString();
                    break;
                case cTSB12Unit2:
                    sPropertyValue = this.TSB12Unit2;
                    break;
                case cTSB15Amount2:
                    sPropertyValue = this.TSB15Amount2.ToString();
                    break;
                case cTSB15Unit2:
                    sPropertyValue = this.TSB15Unit2.ToString();
                    break;
                case cTSB13Amount2:
                    sPropertyValue = this.TSB13Amount2.ToString();
                    break;
                case cTSB13Unit2:
                    sPropertyValue = this.TSB13Unit2;
                    break;
                case cTSB14Amount2:
                    sPropertyValue = this.TSB14Amount2.ToString();
                    break;
                case cTSB14Unit2:
                    sPropertyValue = this.TSB14Unit2;
                    break;
                case cTSB1Description3:
                    sPropertyValue = this.TSB1Description3;
                    break;
                case cTSB1Name3:
                    sPropertyValue = this.TSB1Name3;
                    break;
                case cTSB1Label3:
                    sPropertyValue = this.TSB1Label3;
                    break;
                case cTSB1Type3:
                    sPropertyValue = this.TSB1Type3;
                    break;
                case cTSB1RelLabel3:
                    sPropertyValue = this.TSB1RelLabel3;
                    break;
                case cTSB1TAmount3:
                    sPropertyValue = this.TSB1TAmount3.ToString();
                    break;
                case cTSB1TUnit3:
                    sPropertyValue = this.TSB1TUnit3.ToString();
                    break;
                case cTSB1TD1Amount3:
                    sPropertyValue = this.TSB1TD1Amount3.ToString();
                    break;
                case cTSB1TD1Unit3:
                    sPropertyValue = this.TSB1TD1Unit3.ToString();
                    break;
                case cTSB1TD2Amount3:
                    sPropertyValue = this.TSB1TD2Amount3.ToString();
                    break;
                case cTSB1TD2Unit3:
                    sPropertyValue = this.TSB1TD2Unit3.ToString();
                    break;
                case cTSB1MathResult3:
                    sPropertyValue = this.TSB1MathResult3.ToString();
                    break;
                case cTSB1MathSubType3:
                    sPropertyValue = this.TSB1MathSubType3.ToString();
                    break;
                case cTSB1TMAmount3:
                    sPropertyValue = this.TSB1TMAmount3.ToString();
                    break;
                case cTSB1TMUnit3:
                    sPropertyValue = this.TSB1TMUnit3.ToString();
                    break;
                case cTSB1TLAmount3:
                    sPropertyValue = this.TSB1TLAmount3.ToString();
                    break;
                case cTSB1TLUnit3:
                    sPropertyValue = this.TSB1TLUnit3.ToString();
                    break;
                case cTSB1TUAmount3:
                    sPropertyValue = this.TSB1TUAmount3.ToString();
                    break;
                case cTSB1TUUnit3:
                    sPropertyValue = this.TSB1TUUnit3.ToString();
                    break;
                case cTSB1MathOperator3:
                    sPropertyValue = this.TSB1MathOperator3.ToString();
                    break;
                case cTSB1MathExpression3:
                    sPropertyValue = this.TSB1MathExpression3.ToString();
                    break;
                case cTSB1N3:
                    sPropertyValue = this.TSB1N3.ToString();
                    break;
                case cTSB1Date3:
                    sPropertyValue = this.TSB1Date3.ToString();
                    break;
                case cTSB1MathType3:
                    sPropertyValue = this.TSB1MathType3;
                    break;
                case cTSB11Amount3:
                    sPropertyValue = this.TSB11Amount3.ToString();
                    break;
                case cTSB11Unit3:
                    sPropertyValue = this.TSB11Unit3.ToString();
                    break;
                case cTSB12Amount3:
                    sPropertyValue = this.TSB12Amount3.ToString();
                    break;
                case cTSB12Unit3:
                    sPropertyValue = this.TSB12Unit3;
                    break;
                case cTSB15Amount3:
                    sPropertyValue = this.TSB15Amount3.ToString();
                    break;
                case cTSB15Unit3:
                    sPropertyValue = this.TSB15Unit3.ToString();
                    break;
                case cTSB13Amount3:
                    sPropertyValue = this.TSB13Amount3.ToString();
                    break;
                case cTSB13Unit3:
                    sPropertyValue = this.TSB13Unit3;
                    break;
                case cTSB14Amount3:
                    sPropertyValue = this.TSB14Amount3.ToString();
                    break;
                case cTSB14Unit3:
                    sPropertyValue = this.TSB14Unit3;
                    break;
                case cTSB1Description4:
                    sPropertyValue = this.TSB1Description4;
                    break;
                case cTSB1Name4:
                    sPropertyValue = this.TSB1Name4;
                    break;
                case cTSB1Label4:
                    sPropertyValue = this.TSB1Label4;
                    break;
                case cTSB1Type4:
                    sPropertyValue = this.TSB1Type4;
                    break;
                case cTSB1RelLabel4:
                    sPropertyValue = this.TSB1RelLabel4;
                    break;
                case cTSB1TAmount4:
                    sPropertyValue = this.TSB1TAmount4.ToString();
                    break;
                case cTSB1TUnit4:
                    sPropertyValue = this.TSB1TUnit4.ToString();
                    break;
                case cTSB1TD1Amount4:
                    sPropertyValue = this.TSB1TD1Amount4.ToString();
                    break;
                case cTSB1TD1Unit4:
                    sPropertyValue = this.TSB1TD1Unit4.ToString();
                    break;
                case cTSB1TD2Amount4:
                    sPropertyValue = this.TSB1TD2Amount4.ToString();
                    break;
                case cTSB1TD2Unit4:
                    sPropertyValue = this.TSB1TD2Unit4.ToString();
                    break;
                case cTSB1MathResult4:
                    sPropertyValue = this.TSB1MathResult4.ToString();
                    break;
                case cTSB1MathSubType4:
                    sPropertyValue = this.TSB1MathSubType4.ToString();
                    break;
                case cTSB1TMAmount4:
                    sPropertyValue = this.TSB1TMAmount4.ToString();
                    break;
                case cTSB1TMUnit4:
                    sPropertyValue = this.TSB1TMUnit4.ToString();
                    break;
                case cTSB1TLAmount4:
                    sPropertyValue = this.TSB1TLAmount4.ToString();
                    break;
                case cTSB1TLUnit4:
                    sPropertyValue = this.TSB1TLUnit4.ToString();
                    break;
                case cTSB1TUAmount4:
                    sPropertyValue = this.TSB1TUAmount4.ToString();
                    break;
                case cTSB1TUUnit4:
                    sPropertyValue = this.TSB1TUUnit4.ToString();
                    break;
                case cTSB1MathOperator4:
                    sPropertyValue = this.TSB1MathOperator4.ToString();
                    break;
                case cTSB1MathExpression4:
                    sPropertyValue = this.TSB1MathExpression4.ToString();
                    break;
                case cTSB1N4:
                    sPropertyValue = this.TSB1N4.ToString();
                    break;
                case cTSB1Date4:
                    sPropertyValue = this.TSB1Date4.ToString();
                    break;
                case cTSB1MathType4:
                    sPropertyValue = this.TSB1MathType4;
                    break;
                case cTSB11Amount4:
                    sPropertyValue = this.TSB11Amount4.ToString();
                    break;
                case cTSB11Unit4:
                    sPropertyValue = this.TSB11Unit4.ToString();
                    break;
                case cTSB12Amount4:
                    sPropertyValue = this.TSB12Amount4.ToString();
                    break;
                case cTSB12Unit4:
                    sPropertyValue = this.TSB12Unit4;
                    break;
                case cTSB15Amount4:
                    sPropertyValue = this.TSB15Amount4.ToString();
                    break;
                case cTSB15Unit4:
                    sPropertyValue = this.TSB15Unit4.ToString();
                    break;
                case cTSB13Amount4:
                    sPropertyValue = this.TSB13Amount4.ToString();
                    break;
                case cTSB13Unit4:
                    sPropertyValue = this.TSB13Unit4;
                    break;
                case cTSB14Amount4:
                    sPropertyValue = this.TSB14Amount4.ToString();
                    break;
                case cTSB14Unit4:
                    sPropertyValue = this.TSB14Unit4;
                    break;
                case cTSB1Description5:
                    sPropertyValue = this.TSB1Description5;
                    break;
                case cTSB1Name5:
                    sPropertyValue = this.TSB1Name5;
                    break;
                case cTSB1Label5:
                    sPropertyValue = this.TSB1Label5;
                    break;
                case cTSB1Type5:
                    sPropertyValue = this.TSB1Type5;
                    break;
                case cTSB1RelLabel5:
                    sPropertyValue = this.TSB1RelLabel5;
                    break;
                case cTSB1TAmount5:
                    sPropertyValue = this.TSB1TAmount5.ToString();
                    break;
                case cTSB1TUnit5:
                    sPropertyValue = this.TSB1TUnit5.ToString();
                    break;
                case cTSB1TD1Amount5:
                    sPropertyValue = this.TSB1TD1Amount5.ToString();
                    break;
                case cTSB1TD1Unit5:
                    sPropertyValue = this.TSB1TD1Unit5.ToString();
                    break;
                case cTSB1TD2Amount5:
                    sPropertyValue = this.TSB1TD2Amount5.ToString();
                    break;
                case cTSB1TD2Unit5:
                    sPropertyValue = this.TSB1TD2Unit5.ToString();
                    break;
                case cTSB1MathResult5:
                    sPropertyValue = this.TSB1MathResult5.ToString();
                    break;
                case cTSB1MathSubType5:
                    sPropertyValue = this.TSB1MathSubType5.ToString();
                    break;
                case cTSB1TMAmount5:
                    sPropertyValue = this.TSB1TMAmount5.ToString();
                    break;
                case cTSB1TMUnit5:
                    sPropertyValue = this.TSB1TMUnit5.ToString();
                    break;
                case cTSB1TLAmount5:
                    sPropertyValue = this.TSB1TLAmount5.ToString();
                    break;
                case cTSB1TLUnit5:
                    sPropertyValue = this.TSB1TLUnit5.ToString();
                    break;
                case cTSB1TUAmount5:
                    sPropertyValue = this.TSB1TUAmount5.ToString();
                    break;
                case cTSB1TUUnit5:
                    sPropertyValue = this.TSB1TUUnit5.ToString();
                    break;
                case cTSB1MathOperator5:
                    sPropertyValue = this.TSB1MathOperator5.ToString();
                    break;
                case cTSB1MathExpression5:
                    sPropertyValue = this.TSB1MathExpression5.ToString();
                    break;
                case cTSB1N5:
                    sPropertyValue = this.TSB1N5.ToString();
                    break;
                case cTSB1Date5:
                    sPropertyValue = this.TSB1Date5.ToString();
                    break;
                case cTSB1MathType5:
                    sPropertyValue = this.TSB1MathType5;
                    break;
                case cTSB11Amount5:
                    sPropertyValue = this.TSB11Amount5.ToString();
                    break;
                case cTSB11Unit5:
                    sPropertyValue = this.TSB11Unit5.ToString();
                    break;
                case cTSB12Amount5:
                    sPropertyValue = this.TSB12Amount5.ToString();
                    break;
                case cTSB12Unit5:
                    sPropertyValue = this.TSB12Unit5;
                    break;
                case cTSB15Amount5:
                    sPropertyValue = this.TSB15Amount5.ToString();
                    break;
                case cTSB15Unit5:
                    sPropertyValue = this.TSB15Unit5.ToString();
                    break;
                case cTSB13Amount5:
                    sPropertyValue = this.TSB13Amount5.ToString();
                    break;
                case cTSB13Unit5:
                    sPropertyValue = this.TSB13Unit5;
                    break;
                case cTSB14Amount5:
                    sPropertyValue = this.TSB14Amount5.ToString();
                    break;
                case cTSB14Unit5:
                    sPropertyValue = this.TSB14Unit5;
                    break;
                case cTSB1Description6:
                    sPropertyValue = this.TSB1Description6;
                    break;
                case cTSB1Name6:
                    sPropertyValue = this.TSB1Name6;
                    break;
                case cTSB1Label6:
                    sPropertyValue = this.TSB1Label6;
                    break;
                case cTSB1Type6:
                    sPropertyValue = this.TSB1Type6;
                    break;
                case cTSB1RelLabel6:
                    sPropertyValue = this.TSB1RelLabel6;
                    break;
                case cTSB1TAmount6:
                    sPropertyValue = this.TSB1TAmount6.ToString();
                    break;
                case cTSB1TUnit6:
                    sPropertyValue = this.TSB1TUnit6.ToString();
                    break;
                case cTSB1TD1Amount6:
                    sPropertyValue = this.TSB1TD1Amount6.ToString();
                    break;
                case cTSB1TD1Unit6:
                    sPropertyValue = this.TSB1TD1Unit6.ToString();
                    break;
                case cTSB1TD2Amount6:
                    sPropertyValue = this.TSB1TD2Amount6.ToString();
                    break;
                case cTSB1TD2Unit6:
                    sPropertyValue = this.TSB1TD2Unit6.ToString();
                    break;
                case cTSB1MathResult6:
                    sPropertyValue = this.TSB1MathResult6.ToString();
                    break;
                case cTSB1MathSubType6:
                    sPropertyValue = this.TSB1MathSubType6.ToString();
                    break;
                case cTSB1TMAmount6:
                    sPropertyValue = this.TSB1TMAmount6.ToString();
                    break;
                case cTSB1TMUnit6:
                    sPropertyValue = this.TSB1TMUnit6.ToString();
                    break;
                case cTSB1TLAmount6:
                    sPropertyValue = this.TSB1TLAmount6.ToString();
                    break;
                case cTSB1TLUnit6:
                    sPropertyValue = this.TSB1TLUnit6.ToString();
                    break;
                case cTSB1TUAmount6:
                    sPropertyValue = this.TSB1TUAmount6.ToString();
                    break;
                case cTSB1TUUnit6:
                    sPropertyValue = this.TSB1TUUnit6.ToString();
                    break;
                case cTSB1MathOperator6:
                    sPropertyValue = this.TSB1MathOperator6.ToString();
                    break;
                case cTSB1MathExpression6:
                    sPropertyValue = this.TSB1MathExpression6.ToString();
                    break;
                case cTSB1N6:
                    sPropertyValue = this.TSB1N6.ToString();
                    break;
                case cTSB1Date6:
                    sPropertyValue = this.TSB1Date6.ToString();
                    break;
                case cTSB1MathType6:
                    sPropertyValue = this.TSB1MathType6;
                    break;
                case cTSB11Amount6:
                    sPropertyValue = this.TSB11Amount6.ToString();
                    break;
                case cTSB11Unit6:
                    sPropertyValue = this.TSB11Unit6.ToString();
                    break;
                case cTSB12Amount6:
                    sPropertyValue = this.TSB12Amount6.ToString();
                    break;
                case cTSB12Unit6:
                    sPropertyValue = this.TSB12Unit6;
                    break;
                case cTSB15Amount6:
                    sPropertyValue = this.TSB15Amount6.ToString();
                    break;
                case cTSB15Unit6:
                    sPropertyValue = this.TSB15Unit6.ToString();
                    break;
                case cTSB13Amount6:
                    sPropertyValue = this.TSB13Amount6.ToString();
                    break;
                case cTSB13Unit6:
                    sPropertyValue = this.TSB13Unit6;
                    break;
                case cTSB14Amount6:
                    sPropertyValue = this.TSB14Amount6.ToString();
                    break;
                case cTSB14Unit6:
                    sPropertyValue = this.TSB14Unit6;
                    break;
                case cTSB1Description7:
                    sPropertyValue = this.TSB1Description7;
                    break;
                case cTSB1Name7:
                    sPropertyValue = this.TSB1Name7;
                    break;
                case cTSB1Label7:
                    sPropertyValue = this.TSB1Label7;
                    break;
                case cTSB1Type7:
                    sPropertyValue = this.TSB1Type7;
                    break;
                case cTSB1RelLabel7:
                    sPropertyValue = this.TSB1RelLabel7;
                    break;
                case cTSB1TAmount7:
                    sPropertyValue = this.TSB1TAmount7.ToString();
                    break;
                case cTSB1TUnit7:
                    sPropertyValue = this.TSB1TUnit7.ToString();
                    break;
                case cTSB1TD1Amount7:
                    sPropertyValue = this.TSB1TD1Amount7.ToString();
                    break;
                case cTSB1TD1Unit7:
                    sPropertyValue = this.TSB1TD1Unit7.ToString();
                    break;
                case cTSB1TD2Amount7:
                    sPropertyValue = this.TSB1TD2Amount7.ToString();
                    break;
                case cTSB1TD2Unit7:
                    sPropertyValue = this.TSB1TD2Unit7.ToString();
                    break;
                case cTSB1MathResult7:
                    sPropertyValue = this.TSB1MathResult7.ToString();
                    break;
                case cTSB1MathSubType7:
                    sPropertyValue = this.TSB1MathSubType7.ToString();
                    break;
                case cTSB1TMAmount7:
                    sPropertyValue = this.TSB1TMAmount7.ToString();
                    break;
                case cTSB1TMUnit7:
                    sPropertyValue = this.TSB1TMUnit7.ToString();
                    break;
                case cTSB1TLAmount7:
                    sPropertyValue = this.TSB1TLAmount7.ToString();
                    break;
                case cTSB1TLUnit7:
                    sPropertyValue = this.TSB1TLUnit7.ToString();
                    break;
                case cTSB1TUAmount7:
                    sPropertyValue = this.TSB1TUAmount7.ToString();
                    break;
                case cTSB1TUUnit7:
                    sPropertyValue = this.TSB1TUUnit7.ToString();
                    break;
                case cTSB1MathOperator7:
                    sPropertyValue = this.TSB1MathOperator7.ToString();
                    break;
                case cTSB1MathExpression7:
                    sPropertyValue = this.TSB1MathExpression7.ToString();
                    break;
                case cTSB1N7:
                    sPropertyValue = this.TSB1N7.ToString();
                    break;
                case cTSB1Date7:
                    sPropertyValue = this.TSB1Date7.ToString();
                    break;
                case cTSB1MathType7:
                    sPropertyValue = this.TSB1MathType7;
                    break;
                case cTSB11Amount7:
                    sPropertyValue = this.TSB11Amount7.ToString();
                    break;
                case cTSB11Unit7:
                    sPropertyValue = this.TSB11Unit7.ToString();
                    break;
                case cTSB12Amount7:
                    sPropertyValue = this.TSB12Amount7.ToString();
                    break;
                case cTSB12Unit7:
                    sPropertyValue = this.TSB12Unit7;
                    break;
                case cTSB15Amount7:
                    sPropertyValue = this.TSB15Amount7.ToString();
                    break;
                case cTSB15Unit7:
                    sPropertyValue = this.TSB15Unit7.ToString();
                    break;
                case cTSB13Amount7:
                    sPropertyValue = this.TSB13Amount7.ToString();
                    break;
                case cTSB13Unit7:
                    sPropertyValue = this.TSB13Unit7;
                    break;
                case cTSB14Amount7:
                    sPropertyValue = this.TSB14Amount7.ToString();
                    break;
                case cTSB14Unit7:
                    sPropertyValue = this.TSB14Unit7;
                    break;
                case cTSB1Description8:
                    sPropertyValue = this.TSB1Description8;
                    break;
                case cTSB1Name8:
                    sPropertyValue = this.TSB1Name8;
                    break;
                case cTSB1Label8:
                    sPropertyValue = this.TSB1Label8;
                    break;
                case cTSB1Type8:
                    sPropertyValue = this.TSB1Type8;
                    break;
                case cTSB1RelLabel8:
                    sPropertyValue = this.TSB1RelLabel8;
                    break;
                case cTSB1TAmount8:
                    sPropertyValue = this.TSB1TAmount8.ToString();
                    break;
                case cTSB1TUnit8:
                    sPropertyValue = this.TSB1TUnit8.ToString();
                    break;
                case cTSB1TD1Amount8:
                    sPropertyValue = this.TSB1TD1Amount8.ToString();
                    break;
                case cTSB1TD1Unit8:
                    sPropertyValue = this.TSB1TD1Unit8.ToString();
                    break;
                case cTSB1TD2Amount8:
                    sPropertyValue = this.TSB1TD2Amount8.ToString();
                    break;
                case cTSB1TD2Unit8:
                    sPropertyValue = this.TSB1TD2Unit8.ToString();
                    break;
                case cTSB1MathResult8:
                    sPropertyValue = this.TSB1MathResult8.ToString();
                    break;
                case cTSB1MathSubType8:
                    sPropertyValue = this.TSB1MathSubType8.ToString();
                    break;
                case cTSB1TMAmount8:
                    sPropertyValue = this.TSB1TMAmount8.ToString();
                    break;
                case cTSB1TMUnit8:
                    sPropertyValue = this.TSB1TMUnit8.ToString();
                    break;
                case cTSB1TLAmount8:
                    sPropertyValue = this.TSB1TLAmount8.ToString();
                    break;
                case cTSB1TLUnit8:
                    sPropertyValue = this.TSB1TLUnit8.ToString();
                    break;
                case cTSB1TUAmount8:
                    sPropertyValue = this.TSB1TUAmount8.ToString();
                    break;
                case cTSB1TUUnit8:
                    sPropertyValue = this.TSB1TUUnit8.ToString();
                    break;
                case cTSB1MathOperator8:
                    sPropertyValue = this.TSB1MathOperator8.ToString();
                    break;
                case cTSB1MathExpression8:
                    sPropertyValue = this.TSB1MathExpression8.ToString();
                    break;
                case cTSB1N8:
                    sPropertyValue = this.TSB1N8.ToString();
                    break;
                case cTSB1Date8:
                    sPropertyValue = this.TSB1Date8.ToString();
                    break;
                case cTSB1MathType8:
                    sPropertyValue = this.TSB1MathType8;
                    break;
                case cTSB11Amount8:
                    sPropertyValue = this.TSB11Amount8.ToString();
                    break;
                case cTSB11Unit8:
                    sPropertyValue = this.TSB11Unit8.ToString();
                    break;
                case cTSB12Amount8:
                    sPropertyValue = this.TSB12Amount8.ToString();
                    break;
                case cTSB12Unit8:
                    sPropertyValue = this.TSB12Unit8;
                    break;
                case cTSB15Amount8:
                    sPropertyValue = this.TSB15Amount8.ToString();
                    break;
                case cTSB15Unit8:
                    sPropertyValue = this.TSB15Unit8.ToString();
                    break;
                case cTSB13Amount8:
                    sPropertyValue = this.TSB13Amount8.ToString();
                    break;
                case cTSB13Unit8:
                    sPropertyValue = this.TSB13Unit8;
                    break;
                case cTSB14Amount8:
                    sPropertyValue = this.TSB14Amount8.ToString();
                    break;
                case cTSB14Unit8:
                    sPropertyValue = this.TSB14Unit8;
                    break;
                case cTSB1Description9:
                    sPropertyValue = this.TSB1Description9;
                    break;
                case cTSB1Name9:
                    sPropertyValue = this.TSB1Name9;
                    break;
                case cTSB1Label9:
                    sPropertyValue = this.TSB1Label9;
                    break;
                case cTSB1Type9:
                    sPropertyValue = this.TSB1Type9;
                    break;
                case cTSB1RelLabel9:
                    sPropertyValue = this.TSB1RelLabel9;
                    break;
                case cTSB1TAmount9:
                    sPropertyValue = this.TSB1TAmount9.ToString();
                    break;
                case cTSB1TUnit9:
                    sPropertyValue = this.TSB1TUnit9.ToString();
                    break;
                case cTSB1TD1Amount9:
                    sPropertyValue = this.TSB1TD1Amount9.ToString();
                    break;
                case cTSB1TD1Unit9:
                    sPropertyValue = this.TSB1TD1Unit9.ToString();
                    break;
                case cTSB1TD2Amount9:
                    sPropertyValue = this.TSB1TD2Amount9.ToString();
                    break;
                case cTSB1TD2Unit9:
                    sPropertyValue = this.TSB1TD2Unit9.ToString();
                    break;
                case cTSB1MathResult9:
                    sPropertyValue = this.TSB1MathResult9.ToString();
                    break;
                case cTSB1MathSubType9:
                    sPropertyValue = this.TSB1MathSubType9.ToString();
                    break;
                case cTSB1TMAmount9:
                    sPropertyValue = this.TSB1TMAmount9.ToString();
                    break;
                case cTSB1TMUnit9:
                    sPropertyValue = this.TSB1TMUnit9.ToString();
                    break;
                case cTSB1TLAmount9:
                    sPropertyValue = this.TSB1TLAmount9.ToString();
                    break;
                case cTSB1TLUnit9:
                    sPropertyValue = this.TSB1TLUnit9.ToString();
                    break;
                case cTSB1TUAmount9:
                    sPropertyValue = this.TSB1TUAmount9.ToString();
                    break;
                case cTSB1TUUnit9:
                    sPropertyValue = this.TSB1TUUnit9.ToString();
                    break;
                case cTSB1MathOperator9:
                    sPropertyValue = this.TSB1MathOperator9.ToString();
                    break;
                case cTSB1MathExpression9:
                    sPropertyValue = this.TSB1MathExpression9.ToString();
                    break;
                case cTSB1N9:
                    sPropertyValue = this.TSB1N9.ToString();
                    break;
                case cTSB1Date9:
                    sPropertyValue = this.TSB1Date9.ToString();
                    break;
                case cTSB1MathType9:
                    sPropertyValue = this.TSB1MathType9;
                    break;
                case cTSB11Amount9:
                    sPropertyValue = this.TSB11Amount9.ToString();
                    break;
                case cTSB11Unit9:
                    sPropertyValue = this.TSB11Unit9.ToString();
                    break;
                case cTSB12Amount9:
                    sPropertyValue = this.TSB12Amount9.ToString();
                    break;
                case cTSB12Unit9:
                    sPropertyValue = this.TSB12Unit9;
                    break;
                case cTSB15Amount9:
                    sPropertyValue = this.TSB15Amount9.ToString();
                    break;
                case cTSB15Unit9:
                    sPropertyValue = this.TSB15Unit9.ToString();
                    break;
                case cTSB13Amount9:
                    sPropertyValue = this.TSB13Amount9.ToString();
                    break;
                case cTSB13Unit9:
                    sPropertyValue = this.TSB13Unit9;
                    break;
                case cTSB14Amount9:
                    sPropertyValue = this.TSB14Amount9.ToString();
                    break;
                case cTSB14Unit9:
                    sPropertyValue = this.TSB14Unit9;
                    break;
                case cTSB1Description10:
                    sPropertyValue = this.TSB1Description10;
                    break;
                case cTSB1Name10:
                    sPropertyValue = this.TSB1Name10;
                    break;
                case cTSB1Label10:
                    sPropertyValue = this.TSB1Label10;
                    break;
                case cTSB1Type10:
                    sPropertyValue = this.TSB1Type10;
                    break;
                case cTSB1RelLabel10:
                    sPropertyValue = this.TSB1RelLabel10;
                    break;
                case cTSB1TAmount10:
                    sPropertyValue = this.TSB1TAmount10.ToString();
                    break;
                case cTSB1TUnit10:
                    sPropertyValue = this.TSB1TUnit10.ToString();
                    break;
                case cTSB1TD1Amount10:
                    sPropertyValue = this.TSB1TD1Amount10.ToString();
                    break;
                case cTSB1TD1Unit10:
                    sPropertyValue = this.TSB1TD1Unit10.ToString();
                    break;
                case cTSB1TD2Amount10:
                    sPropertyValue = this.TSB1TD2Amount10.ToString();
                    break;
                case cTSB1TD2Unit10:
                    sPropertyValue = this.TSB1TD2Unit10.ToString();
                    break;
                case cTSB1MathResult10:
                    sPropertyValue = this.TSB1MathResult10.ToString();
                    break;
                case cTSB1MathSubType10:
                    sPropertyValue = this.TSB1MathSubType10.ToString();
                    break;
                case cTSB1TMAmount10:
                    sPropertyValue = this.TSB1TMAmount10.ToString();
                    break;
                case cTSB1TMUnit10:
                    sPropertyValue = this.TSB1TMUnit10.ToString();
                    break;
                case cTSB1TLAmount10:
                    sPropertyValue = this.TSB1TLAmount10.ToString();
                    break;
                case cTSB1TLUnit10:
                    sPropertyValue = this.TSB1TLUnit10.ToString();
                    break;
                case cTSB1TUAmount10:
                    sPropertyValue = this.TSB1TUAmount10.ToString();
                    break;
                case cTSB1TUUnit10:
                    sPropertyValue = this.TSB1TUUnit10.ToString();
                    break;
                case cTSB1MathOperator10:
                    sPropertyValue = this.TSB1MathOperator10.ToString();
                    break;
                case cTSB1MathExpression10:
                    sPropertyValue = this.TSB1MathExpression10.ToString();
                    break;
                case cTSB1N10:
                    sPropertyValue = this.TSB1N10.ToString();
                    break;
                case cTSB1Date10:
                    sPropertyValue = this.TSB1Date10.ToString();
                    break;
                case cTSB1MathType10:
                    sPropertyValue = this.TSB1MathType10;
                    break;
                case cTSB11Amount10:
                    sPropertyValue = this.TSB11Amount10.ToString();
                    break;
                case cTSB11Unit10:
                    sPropertyValue = this.TSB11Unit10.ToString();
                    break;
                case cTSB12Amount10:
                    sPropertyValue = this.TSB12Amount10.ToString();
                    break;
                case cTSB12Unit10:
                    sPropertyValue = this.TSB12Unit10;
                    break;
                case cTSB15Amount10:
                    sPropertyValue = this.TSB15Amount10.ToString();
                    break;
                case cTSB15Unit10:
                    sPropertyValue = this.TSB15Unit10.ToString();
                    break;
                case cTSB13Amount10:
                    sPropertyValue = this.TSB13Amount10.ToString();
                    break;
                case cTSB13Unit10:
                    sPropertyValue = this.TSB13Unit10;
                    break;
                case cTSB14Amount10:
                    sPropertyValue = this.TSB14Amount10.ToString();
                    break;
                case cTSB14Unit10:
                    sPropertyValue = this.TSB14Unit10;
                    break;
                case cTSB1Description11:
                    sPropertyValue = this.TSB1Description11;
                    break;
                case cTSB1Name11:
                    sPropertyValue = this.TSB1Name11;
                    break;
                case cTSB1Label11:
                    sPropertyValue = this.TSB1Label11;
                    break;
                case cTSB1Type11:
                    sPropertyValue = this.TSB1Type11;
                    break;
                case cTSB1RelLabel11:
                    sPropertyValue = this.TSB1RelLabel11;
                    break;
                case cTSB1TAmount11:
                    sPropertyValue = this.TSB1TAmount11.ToString();
                    break;
                case cTSB1TUnit11:
                    sPropertyValue = this.TSB1TUnit11.ToString();
                    break;
                case cTSB1TD1Amount11:
                    sPropertyValue = this.TSB1TD1Amount11.ToString();
                    break;
                case cTSB1TD1Unit11:
                    sPropertyValue = this.TSB1TD1Unit11.ToString();
                    break;
                case cTSB1TD2Amount11:
                    sPropertyValue = this.TSB1TD2Amount11.ToString();
                    break;
                case cTSB1TD2Unit11:
                    sPropertyValue = this.TSB1TD2Unit11.ToString();
                    break;
                case cTSB1MathResult11:
                    sPropertyValue = this.TSB1MathResult11.ToString();
                    break;
                case cTSB1MathSubType11:
                    sPropertyValue = this.TSB1MathSubType11.ToString();
                    break;
                case cTSB1TMAmount11:
                    sPropertyValue = this.TSB1TMAmount11.ToString();
                    break;
                case cTSB1TMUnit11:
                    sPropertyValue = this.TSB1TMUnit11.ToString();
                    break;
                case cTSB1TLAmount11:
                    sPropertyValue = this.TSB1TLAmount11.ToString();
                    break;
                case cTSB1TLUnit11:
                    sPropertyValue = this.TSB1TLUnit11.ToString();
                    break;
                case cTSB1TUAmount11:
                    sPropertyValue = this.TSB1TUAmount11.ToString();
                    break;
                case cTSB1TUUnit11:
                    sPropertyValue = this.TSB1TUUnit11.ToString();
                    break;
                case cTSB1MathOperator11:
                    sPropertyValue = this.TSB1MathOperator11.ToString();
                    break;
                case cTSB1MathExpression11:
                    sPropertyValue = this.TSB1MathExpression11.ToString();
                    break;
                case cTSB1N11:
                    sPropertyValue = this.TSB1N11.ToString();
                    break;
                case cTSB1Date11:
                    sPropertyValue = this.TSB1Date11.ToString();
                    break;
                case cTSB1MathType11:
                    sPropertyValue = this.TSB1MathType11;
                    break;
                case cTSB11Amount11:
                    sPropertyValue = this.TSB11Amount11.ToString();
                    break;
                case cTSB11Unit11:
                    sPropertyValue = this.TSB11Unit11.ToString();
                    break;
                case cTSB12Amount11:
                    sPropertyValue = this.TSB12Amount11.ToString();
                    break;
                case cTSB12Unit11:
                    sPropertyValue = this.TSB12Unit11;
                    break;
                case cTSB15Amount11:
                    sPropertyValue = this.TSB15Amount11.ToString();
                    break;
                case cTSB15Unit11:
                    sPropertyValue = this.TSB15Unit11.ToString();
                    break;
                case cTSB13Amount11:
                    sPropertyValue = this.TSB13Amount11.ToString();
                    break;
                case cTSB13Unit11:
                    sPropertyValue = this.TSB13Unit11;
                    break;
                case cTSB14Amount11:
                    sPropertyValue = this.TSB14Amount11.ToString();
                    break;
                case cTSB14Unit11:
                    sPropertyValue = this.TSB14Unit11;
                    break;
                case cTSB1Description12:
                    sPropertyValue = this.TSB1Description12;
                    break;
                case cTSB1Name12:
                    sPropertyValue = this.TSB1Name12;
                    break;
                case cTSB1Label12:
                    sPropertyValue = this.TSB1Label12;
                    break;
                case cTSB1Type12:
                    sPropertyValue = this.TSB1Type12;
                    break;
                case cTSB1RelLabel12:
                    sPropertyValue = this.TSB1RelLabel12;
                    break;
                case cTSB1TAmount12:
                    sPropertyValue = this.TSB1TAmount12.ToString();
                    break;
                case cTSB1TUnit12:
                    sPropertyValue = this.TSB1TUnit12.ToString();
                    break;
                case cTSB1TD1Amount12:
                    sPropertyValue = this.TSB1TD1Amount12.ToString();
                    break;
                case cTSB1TD1Unit12:
                    sPropertyValue = this.TSB1TD1Unit12.ToString();
                    break;
                case cTSB1TD2Amount12:
                    sPropertyValue = this.TSB1TD2Amount12.ToString();
                    break;
                case cTSB1TD2Unit12:
                    sPropertyValue = this.TSB1TD2Unit12.ToString();
                    break;
                case cTSB1MathResult12:
                    sPropertyValue = this.TSB1MathResult12.ToString();
                    break;
                case cTSB1MathSubType12:
                    sPropertyValue = this.TSB1MathSubType12.ToString();
                    break;
                case cTSB1TMAmount12:
                    sPropertyValue = this.TSB1TMAmount12.ToString();
                    break;
                case cTSB1TMUnit12:
                    sPropertyValue = this.TSB1TMUnit12.ToString();
                    break;
                case cTSB1TLAmount12:
                    sPropertyValue = this.TSB1TLAmount12.ToString();
                    break;
                case cTSB1TLUnit12:
                    sPropertyValue = this.TSB1TLUnit12.ToString();
                    break;
                case cTSB1TUAmount12:
                    sPropertyValue = this.TSB1TUAmount12.ToString();
                    break;
                case cTSB1TUUnit12:
                    sPropertyValue = this.TSB1TUUnit12.ToString();
                    break;
                case cTSB1MathOperator12:
                    sPropertyValue = this.TSB1MathOperator12.ToString();
                    break;
                case cTSB1MathExpression12:
                    sPropertyValue = this.TSB1MathExpression12.ToString();
                    break;
                case cTSB1N12:
                    sPropertyValue = this.TSB1N12.ToString();
                    break;
                case cTSB1Date12:
                    sPropertyValue = this.TSB1Date12.ToString();
                    break;
                case cTSB1MathType12:
                    sPropertyValue = this.TSB1MathType12;
                    break;
                case cTSB11Amount12:
                    sPropertyValue = this.TSB11Amount12.ToString();
                    break;
                case cTSB11Unit12:
                    sPropertyValue = this.TSB11Unit12.ToString();
                    break;
                case cTSB12Amount12:
                    sPropertyValue = this.TSB12Amount12.ToString();
                    break;
                case cTSB12Unit12:
                    sPropertyValue = this.TSB12Unit12;
                    break;
                case cTSB15Amount12:
                    sPropertyValue = this.TSB15Amount12.ToString();
                    break;
                case cTSB15Unit12:
                    sPropertyValue = this.TSB15Unit12.ToString();
                    break;
                case cTSB13Amount12:
                    sPropertyValue = this.TSB13Amount12.ToString();
                    break;
                case cTSB13Unit12:
                    sPropertyValue = this.TSB13Unit12;
                    break;
                case cTSB14Amount12:
                    sPropertyValue = this.TSB14Amount12.ToString();
                    break;
                case cTSB14Unit12:
                    sPropertyValue = this.TSB14Unit12;
                    break;
                case cTSB1Description13:
                    sPropertyValue = this.TSB1Description13;
                    break;
                case cTSB1Name13:
                    sPropertyValue = this.TSB1Name13;
                    break;
                case cTSB1Label13:
                    sPropertyValue = this.TSB1Label13;
                    break;
                case cTSB1Type13:
                    sPropertyValue = this.TSB1Type13;
                    break;
                case cTSB1RelLabel13:
                    sPropertyValue = this.TSB1RelLabel13;
                    break;
                case cTSB1TAmount13:
                    sPropertyValue = this.TSB1TAmount13.ToString();
                    break;
                case cTSB1TUnit13:
                    sPropertyValue = this.TSB1TUnit13.ToString();
                    break;
                case cTSB1TD1Amount13:
                    sPropertyValue = this.TSB1TD1Amount13.ToString();
                    break;
                case cTSB1TD1Unit13:
                    sPropertyValue = this.TSB1TD1Unit13.ToString();
                    break;
                case cTSB1TD2Amount13:
                    sPropertyValue = this.TSB1TD2Amount13.ToString();
                    break;
                case cTSB1TD2Unit13:
                    sPropertyValue = this.TSB1TD2Unit13.ToString();
                    break;
                case cTSB1MathResult13:
                    sPropertyValue = this.TSB1MathResult13.ToString();
                    break;
                case cTSB1MathSubType13:
                    sPropertyValue = this.TSB1MathSubType13.ToString();
                    break;
                case cTSB1TMAmount13:
                    sPropertyValue = this.TSB1TMAmount13.ToString();
                    break;
                case cTSB1TMUnit13:
                    sPropertyValue = this.TSB1TMUnit13.ToString();
                    break;
                case cTSB1TLAmount13:
                    sPropertyValue = this.TSB1TLAmount13.ToString();
                    break;
                case cTSB1TLUnit13:
                    sPropertyValue = this.TSB1TLUnit13.ToString();
                    break;
                case cTSB1TUAmount13:
                    sPropertyValue = this.TSB1TUAmount13.ToString();
                    break;
                case cTSB1TUUnit13:
                    sPropertyValue = this.TSB1TUUnit13.ToString();
                    break;
                case cTSB1MathOperator13:
                    sPropertyValue = this.TSB1MathOperator13.ToString();
                    break;
                case cTSB1MathExpression13:
                    sPropertyValue = this.TSB1MathExpression13.ToString();
                    break;
                case cTSB1N13:
                    sPropertyValue = this.TSB1N13.ToString();
                    break;
                case cTSB1Date13:
                    sPropertyValue = this.TSB1Date13.ToString();
                    break;
                case cTSB1MathType13:
                    sPropertyValue = this.TSB1MathType13;
                    break;
                case cTSB11Amount13:
                    sPropertyValue = this.TSB11Amount13.ToString();
                    break;
                case cTSB11Unit13:
                    sPropertyValue = this.TSB11Unit13.ToString();
                    break;
                case cTSB12Amount13:
                    sPropertyValue = this.TSB12Amount13.ToString();
                    break;
                case cTSB12Unit13:
                    sPropertyValue = this.TSB12Unit13;
                    break;
                case cTSB15Amount13:
                    sPropertyValue = this.TSB15Amount13.ToString();
                    break;
                case cTSB15Unit13:
                    sPropertyValue = this.TSB15Unit13.ToString();
                    break;
                case cTSB13Amount13:
                    sPropertyValue = this.TSB13Amount13.ToString();
                    break;
                case cTSB13Unit13:
                    sPropertyValue = this.TSB13Unit13;
                    break;
                case cTSB14Amount13:
                    sPropertyValue = this.TSB14Amount13.ToString();
                    break;
                case cTSB14Unit13:
                    sPropertyValue = this.TSB14Unit13;
                    break;
                case cTSB1Description14:
                    sPropertyValue = this.TSB1Description14;
                    break;
                case cTSB1Name14:
                    sPropertyValue = this.TSB1Name14;
                    break;
                case cTSB1Label14:
                    sPropertyValue = this.TSB1Label14;
                    break;
                case cTSB1Type14:
                    sPropertyValue = this.TSB1Type14;
                    break;
                case cTSB1RelLabel14:
                    sPropertyValue = this.TSB1RelLabel14;
                    break;
                case cTSB1TAmount14:
                    sPropertyValue = this.TSB1TAmount14.ToString();
                    break;
                case cTSB1TUnit14:
                    sPropertyValue = this.TSB1TUnit14.ToString();
                    break;
                case cTSB1TD1Amount14:
                    sPropertyValue = this.TSB1TD1Amount14.ToString();
                    break;
                case cTSB1TD1Unit14:
                    sPropertyValue = this.TSB1TD1Unit14.ToString();
                    break;
                case cTSB1TD2Amount14:
                    sPropertyValue = this.TSB1TD2Amount14.ToString();
                    break;
                case cTSB1TD2Unit14:
                    sPropertyValue = this.TSB1TD2Unit14.ToString();
                    break;
                case cTSB1MathResult14:
                    sPropertyValue = this.TSB1MathResult14.ToString();
                    break;
                case cTSB1MathSubType14:
                    sPropertyValue = this.TSB1MathSubType14.ToString();
                    break;
                case cTSB1TMAmount14:
                    sPropertyValue = this.TSB1TMAmount14.ToString();
                    break;
                case cTSB1TMUnit14:
                    sPropertyValue = this.TSB1TMUnit14.ToString();
                    break;
                case cTSB1TLAmount14:
                    sPropertyValue = this.TSB1TLAmount14.ToString();
                    break;
                case cTSB1TLUnit14:
                    sPropertyValue = this.TSB1TLUnit14.ToString();
                    break;
                case cTSB1TUAmount14:
                    sPropertyValue = this.TSB1TUAmount14.ToString();
                    break;
                case cTSB1TUUnit14:
                    sPropertyValue = this.TSB1TUUnit14.ToString();
                    break;
                case cTSB1MathOperator14:
                    sPropertyValue = this.TSB1MathOperator14.ToString();
                    break;
                case cTSB1MathExpression14:
                    sPropertyValue = this.TSB1MathExpression14.ToString();
                    break;
                case cTSB1N14:
                    sPropertyValue = this.TSB1N14.ToString();
                    break;
                case cTSB1Date14:
                    sPropertyValue = this.TSB1Date14.ToString();
                    break;
                case cTSB1MathType14:
                    sPropertyValue = this.TSB1MathType14;
                    break;
                case cTSB11Amount14:
                    sPropertyValue = this.TSB11Amount14.ToString();
                    break;
                case cTSB11Unit14:
                    sPropertyValue = this.TSB11Unit14.ToString();
                    break;
                case cTSB12Amount14:
                    sPropertyValue = this.TSB12Amount14.ToString();
                    break;
                case cTSB12Unit14:
                    sPropertyValue = this.TSB12Unit14;
                    break;
                case cTSB15Amount14:
                    sPropertyValue = this.TSB15Amount14.ToString();
                    break;
                case cTSB15Unit14:
                    sPropertyValue = this.TSB15Unit14.ToString();
                    break;
                case cTSB13Amount14:
                    sPropertyValue = this.TSB13Amount14.ToString();
                    break;
                case cTSB13Unit14:
                    sPropertyValue = this.TSB13Unit14;
                    break;
                case cTSB14Amount14:
                    sPropertyValue = this.TSB14Amount14.ToString();
                    break;
                case cTSB14Unit14:
                    sPropertyValue = this.TSB14Unit14;
                    break;
                case cTSB1Description15:
                    sPropertyValue = this.TSB1Description15;
                    break;
                case cTSB1Name15:
                    sPropertyValue = this.TSB1Name15;
                    break;
                case cTSB1Label15:
                    sPropertyValue = this.TSB1Label15;
                    break;
                case cTSB1Type15:
                    sPropertyValue = this.TSB1Type15;
                    break;
                case cTSB1RelLabel15:
                    sPropertyValue = this.TSB1RelLabel15;
                    break;
                case cTSB1TAmount15:
                    sPropertyValue = this.TSB1TAmount15.ToString();
                    break;
                case cTSB1TUnit15:
                    sPropertyValue = this.TSB1TUnit15.ToString();
                    break;
                case cTSB1TD1Amount15:
                    sPropertyValue = this.TSB1TD1Amount15.ToString();
                    break;
                case cTSB1TD1Unit15:
                    sPropertyValue = this.TSB1TD1Unit15.ToString();
                    break;
                case cTSB1TD2Amount15:
                    sPropertyValue = this.TSB1TD2Amount15.ToString();
                    break;
                case cTSB1TD2Unit15:
                    sPropertyValue = this.TSB1TD2Unit15.ToString();
                    break;
                case cTSB1MathResult15:
                    sPropertyValue = this.TSB1MathResult15.ToString();
                    break;
                case cTSB1MathSubType15:
                    sPropertyValue = this.TSB1MathSubType15.ToString();
                    break;
                case cTSB1TMAmount15:
                    sPropertyValue = this.TSB1TMAmount15.ToString();
                    break;
                case cTSB1TMUnit15:
                    sPropertyValue = this.TSB1TMUnit15.ToString();
                    break;
                case cTSB1TLAmount15:
                    sPropertyValue = this.TSB1TLAmount15.ToString();
                    break;
                case cTSB1TLUnit15:
                    sPropertyValue = this.TSB1TLUnit15.ToString();
                    break;
                case cTSB1TUAmount15:
                    sPropertyValue = this.TSB1TUAmount15.ToString();
                    break;
                case cTSB1TUUnit15:
                    sPropertyValue = this.TSB1TUUnit15.ToString();
                    break;
                case cTSB1MathOperator15:
                    sPropertyValue = this.TSB1MathOperator15.ToString();
                    break;
                case cTSB1MathExpression15:
                    sPropertyValue = this.TSB1MathExpression15.ToString();
                    break;
                case cTSB1N15:
                    sPropertyValue = this.TSB1N15.ToString();
                    break;
                case cTSB1Date15:
                    sPropertyValue = this.TSB1Date15.ToString();
                    break;
                case cTSB1MathType15:
                    sPropertyValue = this.TSB1MathType15;
                    break;
                case cTSB11Amount15:
                    sPropertyValue = this.TSB11Amount15.ToString();
                    break;
                case cTSB11Unit15:
                    sPropertyValue = this.TSB11Unit15.ToString();
                    break;
                case cTSB12Amount15:
                    sPropertyValue = this.TSB12Amount15.ToString();
                    break;
                case cTSB12Unit15:
                    sPropertyValue = this.TSB12Unit15;
                    break;
                case cTSB15Amount15:
                    sPropertyValue = this.TSB15Amount15.ToString();
                    break;
                case cTSB15Unit15:
                    sPropertyValue = this.TSB15Unit15.ToString();
                    break;
                case cTSB13Amount15:
                    sPropertyValue = this.TSB13Amount15.ToString();
                    break;
                case cTSB13Unit15:
                    sPropertyValue = this.TSB13Unit15;
                    break;
                case cTSB14Amount15:
                    sPropertyValue = this.TSB14Amount15.ToString();
                    break;
                case cTSB14Unit15:
                    sPropertyValue = this.TSB14Unit15;
                    break;
                case cTSB1Description16:
                    sPropertyValue = this.TSB1Description16;
                    break;
                case cTSB1Name16:
                    sPropertyValue = this.TSB1Name16;
                    break;
                case cTSB1Label16:
                    sPropertyValue = this.TSB1Label16;
                    break;
                case cTSB1Type16:
                    sPropertyValue = this.TSB1Type16;
                    break;
                case cTSB1RelLabel16:
                    sPropertyValue = this.TSB1RelLabel16;
                    break;
                case cTSB1TAmount16:
                    sPropertyValue = this.TSB1TAmount16.ToString();
                    break;
                case cTSB1TUnit16:
                    sPropertyValue = this.TSB1TUnit16.ToString();
                    break;
                case cTSB1TD1Amount16:
                    sPropertyValue = this.TSB1TD1Amount16.ToString();
                    break;
                case cTSB1TD1Unit16:
                    sPropertyValue = this.TSB1TD1Unit16.ToString();
                    break;
                case cTSB1TD2Amount16:
                    sPropertyValue = this.TSB1TD2Amount16.ToString();
                    break;
                case cTSB1TD2Unit16:
                    sPropertyValue = this.TSB1TD2Unit16.ToString();
                    break;
                case cTSB1MathResult16:
                    sPropertyValue = this.TSB1MathResult16.ToString();
                    break;
                case cTSB1MathSubType16:
                    sPropertyValue = this.TSB1MathSubType16.ToString();
                    break;
                case cTSB1TMAmount16:
                    sPropertyValue = this.TSB1TMAmount16.ToString();
                    break;
                case cTSB1TMUnit16:
                    sPropertyValue = this.TSB1TMUnit16.ToString();
                    break;
                case cTSB1TLAmount16:
                    sPropertyValue = this.TSB1TLAmount16.ToString();
                    break;
                case cTSB1TLUnit16:
                    sPropertyValue = this.TSB1TLUnit16.ToString();
                    break;
                case cTSB1TUAmount16:
                    sPropertyValue = this.TSB1TUAmount16.ToString();
                    break;
                case cTSB1TUUnit16:
                    sPropertyValue = this.TSB1TUUnit16.ToString();
                    break;
                case cTSB1MathOperator16:
                    sPropertyValue = this.TSB1MathOperator16.ToString();
                    break;
                case cTSB1MathExpression16:
                    sPropertyValue = this.TSB1MathExpression16.ToString();
                    break;
                case cTSB1N16:
                    sPropertyValue = this.TSB1N16.ToString();
                    break;
                case cTSB1Date16:
                    sPropertyValue = this.TSB1Date16.ToString();
                    break;
                case cTSB1MathType16:
                    sPropertyValue = this.TSB1MathType16;
                    break;
                case cTSB11Amount16:
                    sPropertyValue = this.TSB11Amount16.ToString();
                    break;
                case cTSB11Unit16:
                    sPropertyValue = this.TSB11Unit16.ToString();
                    break;
                case cTSB12Amount16:
                    sPropertyValue = this.TSB12Amount16.ToString();
                    break;
                case cTSB12Unit16:
                    sPropertyValue = this.TSB12Unit16;
                    break;
                case cTSB15Amount16:
                    sPropertyValue = this.TSB15Amount16.ToString();
                    break;
                case cTSB15Unit16:
                    sPropertyValue = this.TSB15Unit16.ToString();
                    break;
                case cTSB13Amount16:
                    sPropertyValue = this.TSB13Amount16.ToString();
                    break;
                case cTSB13Unit16:
                    sPropertyValue = this.TSB13Unit16;
                    break;
                case cTSB14Amount16:
                    sPropertyValue = this.TSB14Amount16.ToString();
                    break;
                case cTSB14Unit16:
                    sPropertyValue = this.TSB14Unit16;
                    break;
                case cTSB1Description17:
                    sPropertyValue = this.TSB1Description17;
                    break;
                case cTSB1Name17:
                    sPropertyValue = this.TSB1Name17;
                    break;
                case cTSB1Label17:
                    sPropertyValue = this.TSB1Label17;
                    break;
                case cTSB1Type17:
                    sPropertyValue = this.TSB1Type17;
                    break;
                case cTSB1RelLabel17:
                    sPropertyValue = this.TSB1RelLabel17;
                    break;
                case cTSB1TAmount17:
                    sPropertyValue = this.TSB1TAmount17.ToString();
                    break;
                case cTSB1TUnit17:
                    sPropertyValue = this.TSB1TUnit17.ToString();
                    break;
                case cTSB1TD1Amount17:
                    sPropertyValue = this.TSB1TD1Amount17.ToString();
                    break;
                case cTSB1TD1Unit17:
                    sPropertyValue = this.TSB1TD1Unit17.ToString();
                    break;
                case cTSB1TD2Amount17:
                    sPropertyValue = this.TSB1TD2Amount17.ToString();
                    break;
                case cTSB1TD2Unit17:
                    sPropertyValue = this.TSB1TD2Unit17.ToString();
                    break;
                case cTSB1MathResult17:
                    sPropertyValue = this.TSB1MathResult17.ToString();
                    break;
                case cTSB1MathSubType17:
                    sPropertyValue = this.TSB1MathSubType17.ToString();
                    break;
                case cTSB1TMAmount17:
                    sPropertyValue = this.TSB1TMAmount17.ToString();
                    break;
                case cTSB1TMUnit17:
                    sPropertyValue = this.TSB1TMUnit17.ToString();
                    break;
                case cTSB1TLAmount17:
                    sPropertyValue = this.TSB1TLAmount17.ToString();
                    break;
                case cTSB1TLUnit17:
                    sPropertyValue = this.TSB1TLUnit17.ToString();
                    break;
                case cTSB1TUAmount17:
                    sPropertyValue = this.TSB1TUAmount17.ToString();
                    break;
                case cTSB1TUUnit17:
                    sPropertyValue = this.TSB1TUUnit17.ToString();
                    break;
                case cTSB1MathOperator17:
                    sPropertyValue = this.TSB1MathOperator17.ToString();
                    break;
                case cTSB1MathExpression17:
                    sPropertyValue = this.TSB1MathExpression17.ToString();
                    break;
                case cTSB1N17:
                    sPropertyValue = this.TSB1N17.ToString();
                    break;
                case cTSB1Date17:
                    sPropertyValue = this.TSB1Date17.ToString();
                    break;
                case cTSB1MathType17:
                    sPropertyValue = this.TSB1MathType17;
                    break;
                case cTSB11Amount17:
                    sPropertyValue = this.TSB11Amount17.ToString();
                    break;
                case cTSB11Unit17:
                    sPropertyValue = this.TSB11Unit17.ToString();
                    break;
                case cTSB12Amount17:
                    sPropertyValue = this.TSB12Amount17.ToString();
                    break;
                case cTSB12Unit17:
                    sPropertyValue = this.TSB12Unit17;
                    break;
                case cTSB15Amount17:
                    sPropertyValue = this.TSB15Amount17.ToString();
                    break;
                case cTSB15Unit17:
                    sPropertyValue = this.TSB15Unit17.ToString();
                    break;
                case cTSB13Amount17:
                    sPropertyValue = this.TSB13Amount17.ToString();
                    break;
                case cTSB13Unit17:
                    sPropertyValue = this.TSB13Unit17;
                    break;
                case cTSB14Amount17:
                    sPropertyValue = this.TSB14Amount17.ToString();
                    break;
                case cTSB14Unit17:
                    sPropertyValue = this.TSB14Unit17;
                    break;
                case cTSB1Description18:
                    sPropertyValue = this.TSB1Description18;
                    break;
                case cTSB1Name18:
                    sPropertyValue = this.TSB1Name18;
                    break;
                case cTSB1Label18:
                    sPropertyValue = this.TSB1Label18;
                    break;
                case cTSB1Type18:
                    sPropertyValue = this.TSB1Type18;
                    break;
                case cTSB1RelLabel18:
                    sPropertyValue = this.TSB1RelLabel18;
                    break;
                case cTSB1TAmount18:
                    sPropertyValue = this.TSB1TAmount18.ToString();
                    break;
                case cTSB1TUnit18:
                    sPropertyValue = this.TSB1TUnit18.ToString();
                    break;
                case cTSB1TD1Amount18:
                    sPropertyValue = this.TSB1TD1Amount18.ToString();
                    break;
                case cTSB1TD1Unit18:
                    sPropertyValue = this.TSB1TD1Unit18.ToString();
                    break;
                case cTSB1TD2Amount18:
                    sPropertyValue = this.TSB1TD2Amount18.ToString();
                    break;
                case cTSB1TD2Unit18:
                    sPropertyValue = this.TSB1TD2Unit18.ToString();
                    break;
                case cTSB1MathResult18:
                    sPropertyValue = this.TSB1MathResult18.ToString();
                    break;
                case cTSB1MathSubType18:
                    sPropertyValue = this.TSB1MathSubType18.ToString();
                    break;
                case cTSB1TMAmount18:
                    sPropertyValue = this.TSB1TMAmount18.ToString();
                    break;
                case cTSB1TMUnit18:
                    sPropertyValue = this.TSB1TMUnit18.ToString();
                    break;
                case cTSB1TLAmount18:
                    sPropertyValue = this.TSB1TLAmount18.ToString();
                    break;
                case cTSB1TLUnit18:
                    sPropertyValue = this.TSB1TLUnit18.ToString();
                    break;
                case cTSB1TUAmount18:
                    sPropertyValue = this.TSB1TUAmount18.ToString();
                    break;
                case cTSB1TUUnit18:
                    sPropertyValue = this.TSB1TUUnit18.ToString();
                    break;
                case cTSB1MathOperator18:
                    sPropertyValue = this.TSB1MathOperator18.ToString();
                    break;
                case cTSB1MathExpression18:
                    sPropertyValue = this.TSB1MathExpression18.ToString();
                    break;
                case cTSB1N18:
                    sPropertyValue = this.TSB1N18.ToString();
                    break;
                case cTSB1Date18:
                    sPropertyValue = this.TSB1Date18.ToString();
                    break;
                case cTSB1MathType18:
                    sPropertyValue = this.TSB1MathType18;
                    break;
                case cTSB11Amount18:
                    sPropertyValue = this.TSB11Amount18.ToString();
                    break;
                case cTSB11Unit18:
                    sPropertyValue = this.TSB11Unit18.ToString();
                    break;
                case cTSB12Amount18:
                    sPropertyValue = this.TSB12Amount18.ToString();
                    break;
                case cTSB12Unit18:
                    sPropertyValue = this.TSB12Unit18;
                    break;
                case cTSB15Amount18:
                    sPropertyValue = this.TSB15Amount18.ToString();
                    break;
                case cTSB15Unit18:
                    sPropertyValue = this.TSB15Unit18.ToString();
                    break;
                case cTSB13Amount18:
                    sPropertyValue = this.TSB13Amount18.ToString();
                    break;
                case cTSB13Unit18:
                    sPropertyValue = this.TSB13Unit18;
                    break;
                case cTSB14Amount18:
                    sPropertyValue = this.TSB14Amount18.ToString();
                    break;
                case cTSB14Unit18:
                    sPropertyValue = this.TSB14Unit18;
                    break;
                case cTSB1Description19:
                    sPropertyValue = this.TSB1Description19;
                    break;
                case cTSB1Name19:
                    sPropertyValue = this.TSB1Name19;
                    break;
                case cTSB1Label19:
                    sPropertyValue = this.TSB1Label19;
                    break;
                case cTSB1Type19:
                    sPropertyValue = this.TSB1Type19;
                    break;
                case cTSB1RelLabel19:
                    sPropertyValue = this.TSB1RelLabel19;
                    break;
                case cTSB1TAmount19:
                    sPropertyValue = this.TSB1TAmount19.ToString();
                    break;
                case cTSB1TUnit19:
                    sPropertyValue = this.TSB1TUnit19.ToString();
                    break;
                case cTSB1TD1Amount19:
                    sPropertyValue = this.TSB1TD1Amount19.ToString();
                    break;
                case cTSB1TD1Unit19:
                    sPropertyValue = this.TSB1TD1Unit19.ToString();
                    break;
                case cTSB1TD2Amount19:
                    sPropertyValue = this.TSB1TD2Amount19.ToString();
                    break;
                case cTSB1TD2Unit19:
                    sPropertyValue = this.TSB1TD2Unit19.ToString();
                    break;
                case cTSB1MathResult19:
                    sPropertyValue = this.TSB1MathResult19.ToString();
                    break;
                case cTSB1MathSubType19:
                    sPropertyValue = this.TSB1MathSubType19.ToString();
                    break;
                case cTSB1TMAmount19:
                    sPropertyValue = this.TSB1TMAmount19.ToString();
                    break;
                case cTSB1TMUnit19:
                    sPropertyValue = this.TSB1TMUnit19.ToString();
                    break;
                case cTSB1TLAmount19:
                    sPropertyValue = this.TSB1TLAmount19.ToString();
                    break;
                case cTSB1TLUnit19:
                    sPropertyValue = this.TSB1TLUnit19.ToString();
                    break;
                case cTSB1TUAmount19:
                    sPropertyValue = this.TSB1TUAmount19.ToString();
                    break;
                case cTSB1TUUnit19:
                    sPropertyValue = this.TSB1TUUnit19.ToString();
                    break;
                case cTSB1MathOperator19:
                    sPropertyValue = this.TSB1MathOperator19.ToString();
                    break;
                case cTSB1MathExpression19:
                    sPropertyValue = this.TSB1MathExpression19.ToString();
                    break;
                case cTSB1N19:
                    sPropertyValue = this.TSB1N19.ToString();
                    break;
                case cTSB1Date19:
                    sPropertyValue = this.TSB1Date19.ToString();
                    break;
                case cTSB1MathType19:
                    sPropertyValue = this.TSB1MathType19;
                    break;
                case cTSB11Amount19:
                    sPropertyValue = this.TSB11Amount19.ToString();
                    break;
                case cTSB11Unit19:
                    sPropertyValue = this.TSB11Unit19.ToString();
                    break;
                case cTSB12Amount19:
                    sPropertyValue = this.TSB12Amount19.ToString();
                    break;
                case cTSB12Unit19:
                    sPropertyValue = this.TSB12Unit19;
                    break;
                case cTSB15Amount19:
                    sPropertyValue = this.TSB15Amount19.ToString();
                    break;
                case cTSB15Unit19:
                    sPropertyValue = this.TSB15Unit19.ToString();
                    break;
                case cTSB13Amount19:
                    sPropertyValue = this.TSB13Amount19.ToString();
                    break;
                case cTSB13Unit19:
                    sPropertyValue = this.TSB13Unit19;
                    break;
                case cTSB14Amount19:
                    sPropertyValue = this.TSB14Amount19.ToString();
                    break;
                case cTSB14Unit19:
                    sPropertyValue = this.TSB14Unit19;
                    break;
                case cTSB1Description20:
                    sPropertyValue = this.TSB1Description20;
                    break;
                case cTSB1Name20:
                    sPropertyValue = this.TSB1Name20;
                    break;
                case cTSB1Label20:
                    sPropertyValue = this.TSB1Label20;
                    break;
                case cTSB1Type20:
                    sPropertyValue = this.TSB1Type20;
                    break;
                case cTSB1RelLabel20:
                    sPropertyValue = this.TSB1RelLabel20;
                    break;
                case cTSB1TAmount20:
                    sPropertyValue = this.TSB1TAmount20.ToString();
                    break;
                case cTSB1TUnit20:
                    sPropertyValue = this.TSB1TUnit20.ToString();
                    break;
                case cTSB1TD1Amount20:
                    sPropertyValue = this.TSB1TD1Amount20.ToString();
                    break;
                case cTSB1TD1Unit20:
                    sPropertyValue = this.TSB1TD1Unit20.ToString();
                    break;
                case cTSB1TD2Amount20:
                    sPropertyValue = this.TSB1TD2Amount20.ToString();
                    break;
                case cTSB1TD2Unit20:
                    sPropertyValue = this.TSB1TD2Unit20.ToString();
                    break;
                case cTSB1MathResult20:
                    sPropertyValue = this.TSB1MathResult20.ToString();
                    break;
                case cTSB1MathSubType20:
                    sPropertyValue = this.TSB1MathSubType20.ToString();
                    break;
                case cTSB1TMAmount20:
                    sPropertyValue = this.TSB1TMAmount20.ToString();
                    break;
                case cTSB1TMUnit20:
                    sPropertyValue = this.TSB1TMUnit20.ToString();
                    break;
                case cTSB1TLAmount20:
                    sPropertyValue = this.TSB1TLAmount20.ToString();
                    break;
                case cTSB1TLUnit20:
                    sPropertyValue = this.TSB1TLUnit20.ToString();
                    break;
                case cTSB1TUAmount20:
                    sPropertyValue = this.TSB1TUAmount20.ToString();
                    break;
                case cTSB1TUUnit20:
                    sPropertyValue = this.TSB1TUUnit20.ToString();
                    break;
                case cTSB1MathOperator20:
                    sPropertyValue = this.TSB1MathOperator20.ToString();
                    break;
                case cTSB1MathExpression20:
                    sPropertyValue = this.TSB1MathExpression20.ToString();
                    break;
                case cTSB1N20:
                    sPropertyValue = this.TSB1N20.ToString();
                    break;
                case cTSB1Date20:
                    sPropertyValue = this.TSB1Date20.ToString();
                    break;
                case cTSB1MathType20:
                    sPropertyValue = this.TSB1MathType20;
                    break;
                case cTSB11Amount20:
                    sPropertyValue = this.TSB11Amount20.ToString();
                    break;
                case cTSB11Unit20:
                    sPropertyValue = this.TSB11Unit20.ToString();
                    break;
                case cTSB12Amount20:
                    sPropertyValue = this.TSB12Amount20.ToString();
                    break;
                case cTSB12Unit20:
                    sPropertyValue = this.TSB12Unit20;
                    break;
                case cTSB15Amount20:
                    sPropertyValue = this.TSB15Amount20.ToString();
                    break;
                case cTSB15Unit20:
                    sPropertyValue = this.TSB15Unit20.ToString();
                    break;
                case cTSB13Amount20:
                    sPropertyValue = this.TSB13Amount20.ToString();
                    break;
                case cTSB13Unit20:
                    sPropertyValue = this.TSB13Unit20;
                    break;
                case cTSB14Amount20:
                    sPropertyValue = this.TSB14Amount20.ToString();
                    break;
                case cTSB14Unit20:
                    sPropertyValue = this.TSB14Unit20;
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public void SetTSB1BaseStockAttributes(string attNameExtension,
            XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                string.Concat(cTSB1Score, attNameExtension), this.TSB1Score);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreUnit, attNameExtension), this.TSB1ScoreUnit);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                string.Concat(cTSB1ScoreD1Amount, attNameExtension), this.TSB1ScoreD1Amount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreD1Unit, attNameExtension), this.TSB1ScoreD1Unit);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                string.Concat(cTSB1ScoreD2Amount, attNameExtension), this.TSB1ScoreD2Amount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreD2Unit, attNameExtension), this.TSB1ScoreD2Unit);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreMathExpression, attNameExtension), this.TSB1ScoreMathExpression);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                string.Concat(cTSB1ScoreM, attNameExtension), this.TSB1ScoreM);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreMUnit, attNameExtension), this.TSB1ScoreMUnit);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                string.Concat(cTSB1ScoreLAmount, attNameExtension), this.TSB1ScoreLAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreLUnit, attNameExtension), this.TSB1ScoreLUnit);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                string.Concat(cTSB1ScoreUAmount, attNameExtension), this.TSB1ScoreUAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreUUnit, attNameExtension), this.TSB1ScoreUUnit);
            CalculatorHelpers.SetAttributeInt(calculator,
                string.Concat(cTSB1Iterations, attNameExtension), this.TSB1Iterations);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreDistType, attNameExtension), this.TSB1ScoreDistType);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreMathType, attNameExtension), this.TSB1ScoreMathType);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreMathResult, attNameExtension), this.TSB1ScoreMathResult);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(cTSB1ScoreMathSubType, attNameExtension), this.TSB1ScoreMathSubType);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                string.Concat(cTSB1ScoreN, attNameExtension), this.TSB1ScoreN);
            //don't needlessly add these to linkedviews if they are not being used
            if (this.TSB1Name1 != string.Empty && this.TSB1Name1 != Constants.NONE)
            {
                //remember that the calculator inheriting from this class must set id and name atts
                //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name1, attNameExtension), this.TSB1Name1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label1, attNameExtension), this.TSB1Label1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description1, attNameExtension), this.TSB1Description1);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date1, attNameExtension), this.TSB1Date1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType1, attNameExtension), this.TSB1MathType1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO1, attNameExtension), this.TSB1BaseIO1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type1, attNameExtension), this.TSB1Type1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel1, attNameExtension), this.TSB1RelLabel1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount1, attNameExtension), this.TSB1TAmount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit1, attNameExtension), this.TSB1TUnit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD1Amount1, attNameExtension), this.TSB1TD1Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit1, attNameExtension), this.TSB1TD1Unit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount1, attNameExtension), this.TSB1TD2Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit1, attNameExtension), this.TSB1TD2Unit1);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult1, attNameExtension), this.TSB1MathResult1);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType1, attNameExtension), this.TSB1MathSubType1);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TMAmount1, attNameExtension), this.TSB1TMAmount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit1, attNameExtension), this.TSB1TMUnit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TLAmount1, attNameExtension), this.TSB1TLAmount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit1, attNameExtension), this.TSB1TLUnit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount1, attNameExtension), this.TSB1TUAmount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit1, attNameExtension), this.TSB1TUUnit1);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator1, attNameExtension), this.TSB1MathOperator1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression1, attNameExtension), this.TSB1MathExpression1);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N1, attNameExtension), this.TSB1N1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount1, attNameExtension), this.TSB15Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit1, attNameExtension), this.TSB15Unit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount1, attNameExtension), this.TSB11Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit1, attNameExtension), this.TSB11Unit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount1, attNameExtension), this.TSB12Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit1, attNameExtension), this.TSB12Unit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount1, attNameExtension), this.TSB13Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit1, attNameExtension), this.TSB13Unit1);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount1, attNameExtension), this.TSB14Amount1);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit1, attNameExtension), this.TSB14Unit1);
            }
            if (this.TSB1Name2 != string.Empty && this.TSB1Name2 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name2, attNameExtension), this.TSB1Name2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label2, attNameExtension), this.TSB1Label2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description2, attNameExtension), this.TSB1Description2);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date2, attNameExtension), this.TSB1Date2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType2, attNameExtension), this.TSB1MathType2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO2, attNameExtension), this.TSB1BaseIO2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type2, attNameExtension), this.TSB1Type2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel2, attNameExtension), this.TSB1RelLabel2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount2, attNameExtension), this.TSB1TAmount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit2, attNameExtension), this.TSB1TUnit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount2, attNameExtension), this.TSB1TD1Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit2, attNameExtension), this.TSB1TD1Unit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount2, attNameExtension), this.TSB1TD2Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit2, attNameExtension), this.TSB1TD2Unit2);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult2, attNameExtension), this.TSB1MathResult2);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType2, attNameExtension), this.TSB1MathSubType2);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount2, attNameExtension), this.TSB1TMAmount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit2, attNameExtension), this.TSB1TMUnit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount2, attNameExtension), this.TSB1TLAmount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit2, attNameExtension), this.TSB1TLUnit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount2, attNameExtension), this.TSB1TUAmount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit2, attNameExtension), this.TSB1TUUnit2);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator2, attNameExtension), this.TSB1MathOperator2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression2, attNameExtension), this.TSB1MathExpression2);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N2, attNameExtension), this.TSB1N2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount2, attNameExtension), this.TSB15Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit2, attNameExtension), this.TSB15Unit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount2, attNameExtension), this.TSB11Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit2, attNameExtension), this.TSB11Unit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount2, attNameExtension), this.TSB12Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit2, attNameExtension), this.TSB12Unit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount2, attNameExtension), this.TSB13Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit2, attNameExtension), this.TSB13Unit2);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount2, attNameExtension), this.TSB14Amount2);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit2, attNameExtension), this.TSB14Unit2);
            }
            if (this.TSB1Name3 != string.Empty && this.TSB1Name3 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name3, attNameExtension), this.TSB1Name3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label3, attNameExtension), this.TSB1Label3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description3, attNameExtension), this.TSB1Description3);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date3, attNameExtension), this.TSB1Date3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType3, attNameExtension), this.TSB1MathType3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO3, attNameExtension), this.TSB1BaseIO3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type3, attNameExtension), this.TSB1Type3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel3, attNameExtension), this.TSB1RelLabel3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount3, attNameExtension), this.TSB1TAmount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit3, attNameExtension), this.TSB1TUnit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount3, attNameExtension), this.TSB1TD1Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit3, attNameExtension), this.TSB1TD1Unit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount3, attNameExtension), this.TSB1TD2Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit3, attNameExtension), this.TSB1TD2Unit3);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult3, attNameExtension), this.TSB1MathResult3);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType3, attNameExtension), this.TSB1MathSubType3);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount3, attNameExtension), this.TSB1TMAmount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit3, attNameExtension), this.TSB1TMUnit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount3, attNameExtension), this.TSB1TLAmount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit3, attNameExtension), this.TSB1TLUnit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount3, attNameExtension), this.TSB1TUAmount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit3, attNameExtension), this.TSB1TUUnit3);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator3, attNameExtension), this.TSB1MathOperator3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression3, attNameExtension), this.TSB1MathExpression3);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N3, attNameExtension), this.TSB1N3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount3, attNameExtension), this.TSB15Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit3, attNameExtension), this.TSB15Unit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount3, attNameExtension), this.TSB11Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit3, attNameExtension), this.TSB11Unit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount3, attNameExtension), this.TSB12Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit3, attNameExtension), this.TSB12Unit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount3, attNameExtension), this.TSB13Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit3, attNameExtension), this.TSB13Unit3);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount3, attNameExtension), this.TSB14Amount3);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit3, attNameExtension), this.TSB14Unit3);
            }
            if (this.TSB1Name4 != string.Empty && this.TSB1Name4 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name4, attNameExtension), this.TSB1Name4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label4, attNameExtension), this.TSB1Label4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description4, attNameExtension), this.TSB1Description4);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date4, attNameExtension), this.TSB1Date4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType4, attNameExtension), this.TSB1MathType4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO4, attNameExtension), this.TSB1BaseIO4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type4, attNameExtension), this.TSB1Type4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel4, attNameExtension), this.TSB1RelLabel4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount4, attNameExtension), this.TSB1TAmount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit4, attNameExtension), this.TSB1TUnit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount4, attNameExtension), this.TSB1TD1Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit4, attNameExtension), this.TSB1TD1Unit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount4, attNameExtension), this.TSB1TD2Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit4, attNameExtension), this.TSB1TD2Unit4);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult4, attNameExtension), this.TSB1MathResult4);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType4, attNameExtension), this.TSB1MathSubType4);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount4, attNameExtension), this.TSB1TMAmount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit4, attNameExtension), this.TSB1TMUnit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount4, attNameExtension), this.TSB1TLAmount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit4, attNameExtension), this.TSB1TLUnit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount4, attNameExtension), this.TSB1TUAmount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit4, attNameExtension), this.TSB1TUUnit4);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator4, attNameExtension), this.TSB1MathOperator4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression4, attNameExtension), this.TSB1MathExpression4);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N4, attNameExtension), this.TSB1N4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount4, attNameExtension), this.TSB15Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit4, attNameExtension), this.TSB15Unit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount4, attNameExtension), this.TSB11Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit4, attNameExtension), this.TSB11Unit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount4, attNameExtension), this.TSB12Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit4, attNameExtension), this.TSB12Unit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount4, attNameExtension), this.TSB13Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit4, attNameExtension), this.TSB13Unit4);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount4, attNameExtension), this.TSB14Amount4);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit4, attNameExtension), this.TSB14Unit4);
            }
            if (this.TSB1Name5 != string.Empty && this.TSB1Name5 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name5, attNameExtension), this.TSB1Name5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label5, attNameExtension), this.TSB1Label5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description5, attNameExtension), this.TSB1Description5);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date5, attNameExtension), this.TSB1Date5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType5, attNameExtension), this.TSB1MathType5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO5, attNameExtension), this.TSB1BaseIO5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type5, attNameExtension), this.TSB1Type5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel5, attNameExtension), this.TSB1RelLabel5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount5, attNameExtension), this.TSB1TAmount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit5, attNameExtension), this.TSB1TUnit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount5, attNameExtension), this.TSB1TD1Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit5, attNameExtension), this.TSB1TD1Unit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount5, attNameExtension), this.TSB1TD2Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit5, attNameExtension), this.TSB1TD2Unit5);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult5, attNameExtension), this.TSB1MathResult5);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType5, attNameExtension), this.TSB1MathSubType5);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount5, attNameExtension), this.TSB1TMAmount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit5, attNameExtension), this.TSB1TMUnit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount5, attNameExtension), this.TSB1TLAmount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit5, attNameExtension), this.TSB1TLUnit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount5, attNameExtension), this.TSB1TUAmount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit5, attNameExtension), this.TSB1TUUnit5);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator5, attNameExtension), this.TSB1MathOperator5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression5, attNameExtension), this.TSB1MathExpression5);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N5, attNameExtension), this.TSB1N5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount5, attNameExtension), this.TSB15Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit5, attNameExtension), this.TSB15Unit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount5, attNameExtension), this.TSB11Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit5, attNameExtension), this.TSB11Unit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount5, attNameExtension), this.TSB12Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit5, attNameExtension), this.TSB12Unit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount5, attNameExtension), this.TSB13Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit5, attNameExtension), this.TSB13Unit5);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount5, attNameExtension), this.TSB14Amount5);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit5, attNameExtension), this.TSB14Unit5);
            }
            if (this.TSB1Name6 != string.Empty && this.TSB1Name6 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name6, attNameExtension), this.TSB1Name6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label6, attNameExtension), this.TSB1Label6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description6, attNameExtension), this.TSB1Description6);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date6, attNameExtension), this.TSB1Date6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType6, attNameExtension), this.TSB1MathType6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO6, attNameExtension), this.TSB1BaseIO6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type6, attNameExtension), this.TSB1Type6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel6, attNameExtension), this.TSB1RelLabel6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount6, attNameExtension), this.TSB1TAmount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit6, attNameExtension), this.TSB1TUnit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount6, attNameExtension), this.TSB1TD1Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit6, attNameExtension), this.TSB1TD1Unit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount6, attNameExtension), this.TSB1TD2Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit6, attNameExtension), this.TSB1TD2Unit6);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult6, attNameExtension), this.TSB1MathResult6);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType6, attNameExtension), this.TSB1MathSubType6);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount6, attNameExtension), this.TSB1TMAmount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit6, attNameExtension), this.TSB1TMUnit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount6, attNameExtension), this.TSB1TLAmount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit6, attNameExtension), this.TSB1TLUnit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount6, attNameExtension), this.TSB1TUAmount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit6, attNameExtension), this.TSB1TUUnit6);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator6, attNameExtension), this.TSB1MathOperator6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression6, attNameExtension), this.TSB1MathExpression6);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N6, attNameExtension), this.TSB1N6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount6, attNameExtension), this.TSB15Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit6, attNameExtension), this.TSB15Unit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount6, attNameExtension), this.TSB11Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit6, attNameExtension), this.TSB11Unit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount6, attNameExtension), this.TSB12Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit6, attNameExtension), this.TSB12Unit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount6, attNameExtension), this.TSB13Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit6, attNameExtension), this.TSB13Unit6);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount6, attNameExtension), this.TSB14Amount6);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit6, attNameExtension), this.TSB14Unit6);
            }
            if (this.TSB1Name7 != string.Empty && this.TSB1Name7 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name7, attNameExtension), this.TSB1Name7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label7, attNameExtension), this.TSB1Label7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description7, attNameExtension), this.TSB1Description7);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date7, attNameExtension), this.TSB1Date7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType7, attNameExtension), this.TSB1MathType7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO7, attNameExtension), this.TSB1BaseIO7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type7, attNameExtension), this.TSB1Type7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel7, attNameExtension), this.TSB1RelLabel7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount7, attNameExtension), this.TSB1TAmount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit7, attNameExtension), this.TSB1TUnit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount7, attNameExtension), this.TSB1TD1Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit7, attNameExtension), this.TSB1TD1Unit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount7, attNameExtension), this.TSB1TD2Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit7, attNameExtension), this.TSB1TD2Unit7);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult7, attNameExtension), this.TSB1MathResult7);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType7, attNameExtension), this.TSB1MathSubType7);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount7, attNameExtension), this.TSB1TMAmount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit7, attNameExtension), this.TSB1TMUnit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount7, attNameExtension), this.TSB1TLAmount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit7, attNameExtension), this.TSB1TLUnit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount7, attNameExtension), this.TSB1TUAmount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit7, attNameExtension), this.TSB1TUUnit7);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator7, attNameExtension), this.TSB1MathOperator7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression7, attNameExtension), this.TSB1MathExpression7);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N7, attNameExtension), this.TSB1N7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount7, attNameExtension), this.TSB15Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit7, attNameExtension), this.TSB15Unit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount7, attNameExtension), this.TSB11Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit7, attNameExtension), this.TSB11Unit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount7, attNameExtension), this.TSB12Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit7, attNameExtension), this.TSB12Unit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount7, attNameExtension), this.TSB13Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit7, attNameExtension), this.TSB13Unit7);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount7, attNameExtension), this.TSB14Amount7);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit7, attNameExtension), this.TSB14Unit7);
            }
            if (this.TSB1Name8 != string.Empty && this.TSB1Name8 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name8, attNameExtension), this.TSB1Name8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label8, attNameExtension), this.TSB1Label8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description8, attNameExtension), this.TSB1Description8);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date8, attNameExtension), this.TSB1Date8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType8, attNameExtension), this.TSB1MathType8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO8, attNameExtension), this.TSB1BaseIO8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type8, attNameExtension), this.TSB1Type8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel8, attNameExtension), this.TSB1RelLabel8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount8, attNameExtension), this.TSB1TAmount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit8, attNameExtension), this.TSB1TUnit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount8, attNameExtension), this.TSB1TD1Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit8, attNameExtension), this.TSB1TD1Unit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount8, attNameExtension), this.TSB1TD2Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit8, attNameExtension), this.TSB1TD2Unit8);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult8, attNameExtension), this.TSB1MathResult8);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType8, attNameExtension), this.TSB1MathSubType8);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount8, attNameExtension), this.TSB1TMAmount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit8, attNameExtension), this.TSB1TMUnit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount8, attNameExtension), this.TSB1TLAmount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit8, attNameExtension), this.TSB1TLUnit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount8, attNameExtension), this.TSB1TUAmount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit8, attNameExtension), this.TSB1TUUnit8);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator8, attNameExtension), this.TSB1MathOperator8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression8, attNameExtension), this.TSB1MathExpression8);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N8, attNameExtension), this.TSB1N8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount8, attNameExtension), this.TSB15Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit8, attNameExtension), this.TSB15Unit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount8, attNameExtension), this.TSB11Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit8, attNameExtension), this.TSB11Unit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount8, attNameExtension), this.TSB12Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit8, attNameExtension), this.TSB12Unit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount8, attNameExtension), this.TSB13Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit8, attNameExtension), this.TSB13Unit8);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount8, attNameExtension), this.TSB14Amount8);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit8, attNameExtension), this.TSB14Unit8);
            }
            if (this.TSB1Name9 != string.Empty && this.TSB1Name9 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name9, attNameExtension), this.TSB1Name9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label9, attNameExtension), this.TSB1Label9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description9, attNameExtension), this.TSB1Description9);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date9, attNameExtension), this.TSB1Date9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType9, attNameExtension), this.TSB1MathType9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO9, attNameExtension), this.TSB1BaseIO9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type9, attNameExtension), this.TSB1Type9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel9, attNameExtension), this.TSB1RelLabel9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount9, attNameExtension), this.TSB1TAmount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit9, attNameExtension), this.TSB1TUnit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount9, attNameExtension), this.TSB1TD1Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit9, attNameExtension), this.TSB1TD1Unit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount9, attNameExtension), this.TSB1TD2Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit9, attNameExtension), this.TSB1TD2Unit9);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult9, attNameExtension), this.TSB1MathResult9);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType9, attNameExtension), this.TSB1MathSubType9);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount9, attNameExtension), this.TSB1TMAmount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit9, attNameExtension), this.TSB1TMUnit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount9, attNameExtension), this.TSB1TLAmount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit9, attNameExtension), this.TSB1TLUnit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount9, attNameExtension), this.TSB1TUAmount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit9, attNameExtension), this.TSB1TUUnit9);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator9, attNameExtension), this.TSB1MathOperator9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression9, attNameExtension), this.TSB1MathExpression9);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N9, attNameExtension), this.TSB1N9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount9, attNameExtension), this.TSB15Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit9, attNameExtension), this.TSB15Unit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount9, attNameExtension), this.TSB11Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit9, attNameExtension), this.TSB11Unit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount9, attNameExtension), this.TSB12Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit9, attNameExtension), this.TSB12Unit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount9, attNameExtension), this.TSB13Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit9, attNameExtension), this.TSB13Unit9);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount9, attNameExtension), this.TSB14Amount9);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit9, attNameExtension), this.TSB14Unit9);
            }
            if (this.TSB1Name10 != string.Empty && this.TSB1Name10 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name10, attNameExtension), this.TSB1Name10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label10, attNameExtension), this.TSB1Label10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description10, attNameExtension), this.TSB1Description10);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date10, attNameExtension), this.TSB1Date10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType10, attNameExtension), this.TSB1MathType10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO10, attNameExtension), this.TSB1BaseIO10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type10, attNameExtension), this.TSB1Type10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel10, attNameExtension), this.TSB1RelLabel10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount10, attNameExtension), this.TSB1TAmount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit10, attNameExtension), this.TSB1TUnit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount10, attNameExtension), this.TSB1TD1Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit10, attNameExtension), this.TSB1TD1Unit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount10, attNameExtension), this.TSB1TD2Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit10, attNameExtension), this.TSB1TD2Unit10);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult10, attNameExtension), this.TSB1MathResult10);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType10, attNameExtension), this.TSB1MathSubType10);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount10, attNameExtension), this.TSB1TMAmount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit10, attNameExtension), this.TSB1TMUnit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount10, attNameExtension), this.TSB1TLAmount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit10, attNameExtension), this.TSB1TLUnit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount10, attNameExtension), this.TSB1TUAmount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit10, attNameExtension), this.TSB1TUUnit10);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator10, attNameExtension), this.TSB1MathOperator10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression10, attNameExtension), this.TSB1MathExpression10);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N10, attNameExtension), this.TSB1N10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount10, attNameExtension), this.TSB15Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit10, attNameExtension), this.TSB15Unit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount10, attNameExtension), this.TSB11Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit10, attNameExtension), this.TSB11Unit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount10, attNameExtension), this.TSB12Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit10, attNameExtension), this.TSB12Unit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount10, attNameExtension), this.TSB13Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit10, attNameExtension), this.TSB13Unit10);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount10, attNameExtension), this.TSB14Amount10);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit10, attNameExtension), this.TSB14Unit10);
            }
            if (this.TSB1Name11 != string.Empty && this.TSB1Name11 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name11, attNameExtension), this.TSB1Name11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label11, attNameExtension), this.TSB1Label11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description11, attNameExtension), this.TSB1Description11);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date11, attNameExtension), this.TSB1Date11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType11, attNameExtension), this.TSB1MathType11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO11, attNameExtension), this.TSB1BaseIO11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type11, attNameExtension), this.TSB1Type11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel11, attNameExtension), this.TSB1RelLabel11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount11, attNameExtension), this.TSB1TAmount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit11, attNameExtension), this.TSB1TUnit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount11, attNameExtension), this.TSB1TD1Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit11, attNameExtension), this.TSB1TD1Unit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount11, attNameExtension), this.TSB1TD2Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit11, attNameExtension), this.TSB1TD2Unit11);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult11, attNameExtension), this.TSB1MathResult11);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType11, attNameExtension), this.TSB1MathSubType11);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount11, attNameExtension), this.TSB1TMAmount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit11, attNameExtension), this.TSB1TMUnit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount11, attNameExtension), this.TSB1TLAmount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit11, attNameExtension), this.TSB1TLUnit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount11, attNameExtension), this.TSB1TUAmount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit11, attNameExtension), this.TSB1TUUnit11);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator11, attNameExtension), this.TSB1MathOperator11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression11, attNameExtension), this.TSB1MathExpression11);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N11, attNameExtension), this.TSB1N11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount11, attNameExtension), this.TSB15Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit11, attNameExtension), this.TSB15Unit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount11, attNameExtension), this.TSB11Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit11, attNameExtension), this.TSB11Unit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount11, attNameExtension), this.TSB12Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit11, attNameExtension), this.TSB12Unit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount11, attNameExtension), this.TSB13Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit11, attNameExtension), this.TSB13Unit11);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount11, attNameExtension), this.TSB14Amount11);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit11, attNameExtension), this.TSB14Unit11);
            }
            if (this.TSB1Name12 != string.Empty && this.TSB1Name12 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name12, attNameExtension), this.TSB1Name12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label12, attNameExtension), this.TSB1Label12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description12, attNameExtension), this.TSB1Description12);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date12, attNameExtension), this.TSB1Date12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType12, attNameExtension), this.TSB1MathType12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO12, attNameExtension), this.TSB1BaseIO12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type12, attNameExtension), this.TSB1Type12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel12, attNameExtension), this.TSB1RelLabel12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount12, attNameExtension), this.TSB1TAmount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit12, attNameExtension), this.TSB1TUnit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount12, attNameExtension), this.TSB1TD1Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit12, attNameExtension), this.TSB1TD1Unit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount12, attNameExtension), this.TSB1TD2Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit12, attNameExtension), this.TSB1TD2Unit12);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult12, attNameExtension), this.TSB1MathResult12);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType12, attNameExtension), this.TSB1MathSubType12);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount12, attNameExtension), this.TSB1TMAmount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit12, attNameExtension), this.TSB1TMUnit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount12, attNameExtension), this.TSB1TLAmount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit12, attNameExtension), this.TSB1TLUnit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount12, attNameExtension), this.TSB1TUAmount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit12, attNameExtension), this.TSB1TUUnit12);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator12, attNameExtension), this.TSB1MathOperator12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression12, attNameExtension), this.TSB1MathExpression12);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N12, attNameExtension), this.TSB1N12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount12, attNameExtension), this.TSB15Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit12, attNameExtension), this.TSB15Unit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount12, attNameExtension), this.TSB11Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit12, attNameExtension), this.TSB11Unit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount12, attNameExtension), this.TSB12Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit12, attNameExtension), this.TSB12Unit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount12, attNameExtension), this.TSB13Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit12, attNameExtension), this.TSB13Unit12);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount12, attNameExtension), this.TSB14Amount12);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit12, attNameExtension), this.TSB14Unit12);
            }
            if (this.TSB1Name13 != string.Empty && this.TSB1Name13 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name13, attNameExtension), this.TSB1Name13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label13, attNameExtension), this.TSB1Label13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description13, attNameExtension), this.TSB1Description13);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date13, attNameExtension), this.TSB1Date13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType13, attNameExtension), this.TSB1MathType13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO13, attNameExtension), this.TSB1BaseIO13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type13, attNameExtension), this.TSB1Type13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel13, attNameExtension), this.TSB1RelLabel13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount13, attNameExtension), this.TSB1TAmount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit13, attNameExtension), this.TSB1TUnit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount13, attNameExtension), this.TSB1TD1Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit13, attNameExtension), this.TSB1TD1Unit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount13, attNameExtension), this.TSB1TD2Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit13, attNameExtension), this.TSB1TD2Unit13);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult13, attNameExtension), this.TSB1MathResult13);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType13, attNameExtension), this.TSB1MathSubType13);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount13, attNameExtension), this.TSB1TMAmount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit13, attNameExtension), this.TSB1TMUnit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount13, attNameExtension), this.TSB1TLAmount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit13, attNameExtension), this.TSB1TLUnit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount13, attNameExtension), this.TSB1TUAmount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit13, attNameExtension), this.TSB1TUUnit13);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator13, attNameExtension), this.TSB1MathOperator13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression13, attNameExtension), this.TSB1MathExpression13);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N13, attNameExtension), this.TSB1N13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount13, attNameExtension), this.TSB15Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit13, attNameExtension), this.TSB15Unit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount13, attNameExtension), this.TSB11Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit13, attNameExtension), this.TSB11Unit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount13, attNameExtension), this.TSB12Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit13, attNameExtension), this.TSB12Unit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount13, attNameExtension), this.TSB13Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit13, attNameExtension), this.TSB13Unit13);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount13, attNameExtension), this.TSB14Amount13);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit13, attNameExtension), this.TSB14Unit13);
            }
            if (this.TSB1Name14 != string.Empty && this.TSB1Name14 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name14, attNameExtension), this.TSB1Name14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label14, attNameExtension), this.TSB1Label14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description14, attNameExtension), this.TSB1Description14);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date14, attNameExtension), this.TSB1Date14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType14, attNameExtension), this.TSB1MathType14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO14, attNameExtension), this.TSB1BaseIO14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type14, attNameExtension), this.TSB1Type14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel14, attNameExtension), this.TSB1RelLabel14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount14, attNameExtension), this.TSB1TAmount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit14, attNameExtension), this.TSB1TUnit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount14, attNameExtension), this.TSB1TD1Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit14, attNameExtension), this.TSB1TD1Unit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount14, attNameExtension), this.TSB1TD2Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit14, attNameExtension), this.TSB1TD2Unit14);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult14, attNameExtension), this.TSB1MathResult14);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType14, attNameExtension), this.TSB1MathSubType14);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount14, attNameExtension), this.TSB1TMAmount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit14, attNameExtension), this.TSB1TMUnit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount14, attNameExtension), this.TSB1TLAmount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit14, attNameExtension), this.TSB1TLUnit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount14, attNameExtension), this.TSB1TUAmount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit14, attNameExtension), this.TSB1TUUnit14);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator14, attNameExtension), this.TSB1MathOperator14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression14, attNameExtension), this.TSB1MathExpression14);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N14, attNameExtension), this.TSB1N14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount14, attNameExtension), this.TSB15Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit14, attNameExtension), this.TSB15Unit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount14, attNameExtension), this.TSB11Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit14, attNameExtension), this.TSB11Unit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount14, attNameExtension), this.TSB12Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit14, attNameExtension), this.TSB12Unit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount14, attNameExtension), this.TSB13Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit14, attNameExtension), this.TSB13Unit14);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount14, attNameExtension), this.TSB14Amount14);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit14, attNameExtension), this.TSB14Unit14);
            }
            if (this.TSB1Name15 != string.Empty && this.TSB1Name15 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name15, attNameExtension), this.TSB1Name15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label15, attNameExtension), this.TSB1Label15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description15, attNameExtension), this.TSB1Description15);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date15, attNameExtension), this.TSB1Date15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType15, attNameExtension), this.TSB1MathType15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO15, attNameExtension), this.TSB1BaseIO15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type15, attNameExtension), this.TSB1Type15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel15, attNameExtension), this.TSB1RelLabel15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount15, attNameExtension), this.TSB1TAmount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit15, attNameExtension), this.TSB1TUnit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount15, attNameExtension), this.TSB1TD1Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit15, attNameExtension), this.TSB1TD1Unit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount15, attNameExtension), this.TSB1TD2Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit15, attNameExtension), this.TSB1TD2Unit15);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult15, attNameExtension), this.TSB1MathResult15);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType15, attNameExtension), this.TSB1MathSubType15);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount15, attNameExtension), this.TSB1TMAmount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit15, attNameExtension), this.TSB1TMUnit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount15, attNameExtension), this.TSB1TLAmount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit15, attNameExtension), this.TSB1TLUnit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount15, attNameExtension), this.TSB1TUAmount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit15, attNameExtension), this.TSB1TUUnit15);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator15, attNameExtension), this.TSB1MathOperator15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression15, attNameExtension), this.TSB1MathExpression15);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N15, attNameExtension), this.TSB1N15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount15, attNameExtension), this.TSB15Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit15, attNameExtension), this.TSB15Unit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount15, attNameExtension), this.TSB11Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit15, attNameExtension), this.TSB11Unit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount15, attNameExtension), this.TSB12Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit15, attNameExtension), this.TSB12Unit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount15, attNameExtension), this.TSB13Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit15, attNameExtension), this.TSB13Unit15);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount15, attNameExtension), this.TSB14Amount15);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit15, attNameExtension), this.TSB14Unit15);
            }
            if (this.TSB1Name16 != string.Empty && this.TSB1Name16 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name16, attNameExtension), this.TSB1Name16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label16, attNameExtension), this.TSB1Label16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description16, attNameExtension), this.TSB1Description16);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date16, attNameExtension), this.TSB1Date16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType16, attNameExtension), this.TSB1MathType16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO16, attNameExtension), this.TSB1BaseIO16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type16, attNameExtension), this.TSB1Type16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel16, attNameExtension), this.TSB1RelLabel16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount16, attNameExtension), this.TSB1TAmount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit16, attNameExtension), this.TSB1TUnit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount16, attNameExtension), this.TSB1TD1Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit16, attNameExtension), this.TSB1TD1Unit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount16, attNameExtension), this.TSB1TD2Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit16, attNameExtension), this.TSB1TD2Unit16);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult16, attNameExtension), this.TSB1MathResult16);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType16, attNameExtension), this.TSB1MathSubType16);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount16, attNameExtension), this.TSB1TMAmount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit16, attNameExtension), this.TSB1TMUnit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount16, attNameExtension), this.TSB1TLAmount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit16, attNameExtension), this.TSB1TLUnit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount16, attNameExtension), this.TSB1TUAmount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit16, attNameExtension), this.TSB1TUUnit16);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator16, attNameExtension), this.TSB1MathOperator16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression16, attNameExtension), this.TSB1MathExpression16);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N16, attNameExtension), this.TSB1N16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount16, attNameExtension), this.TSB15Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit16, attNameExtension), this.TSB15Unit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount16, attNameExtension), this.TSB11Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit16, attNameExtension), this.TSB11Unit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount16, attNameExtension), this.TSB12Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit16, attNameExtension), this.TSB12Unit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount16, attNameExtension), this.TSB13Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit16, attNameExtension), this.TSB13Unit16);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount16, attNameExtension), this.TSB14Amount16);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit16, attNameExtension), this.TSB14Unit16);
            }
            if (this.TSB1Name17 != string.Empty && this.TSB1Name17 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name17, attNameExtension), this.TSB1Name17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label17, attNameExtension), this.TSB1Label17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description17, attNameExtension), this.TSB1Description17);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date17, attNameExtension), this.TSB1Date17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType17, attNameExtension), this.TSB1MathType17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO17, attNameExtension), this.TSB1BaseIO17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type17, attNameExtension), this.TSB1Type17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel17, attNameExtension), this.TSB1RelLabel17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount17, attNameExtension), this.TSB1TAmount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit17, attNameExtension), this.TSB1TUnit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount17, attNameExtension), this.TSB1TD1Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit17, attNameExtension), this.TSB1TD1Unit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount17, attNameExtension), this.TSB1TD2Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit17, attNameExtension), this.TSB1TD2Unit17);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult17, attNameExtension), this.TSB1MathResult17);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType17, attNameExtension), this.TSB1MathSubType17);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount17, attNameExtension), this.TSB1TMAmount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit17, attNameExtension), this.TSB1TMUnit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount17, attNameExtension), this.TSB1TLAmount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit17, attNameExtension), this.TSB1TLUnit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount17, attNameExtension), this.TSB1TUAmount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit17, attNameExtension), this.TSB1TUUnit17);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator17, attNameExtension), this.TSB1MathOperator17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression17, attNameExtension), this.TSB1MathExpression17);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N17, attNameExtension), this.TSB1N17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount17, attNameExtension), this.TSB15Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit17, attNameExtension), this.TSB15Unit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount17, attNameExtension), this.TSB11Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit17, attNameExtension), this.TSB11Unit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount17, attNameExtension), this.TSB12Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit17, attNameExtension), this.TSB12Unit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount17, attNameExtension), this.TSB13Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit17, attNameExtension), this.TSB13Unit17);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount17, attNameExtension), this.TSB14Amount17);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit17, attNameExtension), this.TSB14Unit17);
            }
            if (this.TSB1Name18 != string.Empty && this.TSB1Name18 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name18, attNameExtension), this.TSB1Name18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label18, attNameExtension), this.TSB1Label18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description18, attNameExtension), this.TSB1Description18);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date18, attNameExtension), this.TSB1Date18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType18, attNameExtension), this.TSB1MathType18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO18, attNameExtension), this.TSB1BaseIO18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type18, attNameExtension), this.TSB1Type18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel18, attNameExtension), this.TSB1RelLabel18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount18, attNameExtension), this.TSB1TAmount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit18, attNameExtension), this.TSB1TUnit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount18, attNameExtension), this.TSB1TD1Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit18, attNameExtension), this.TSB1TD1Unit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount18, attNameExtension), this.TSB1TD2Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit18, attNameExtension), this.TSB1TD2Unit18);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult18, attNameExtension), this.TSB1MathResult18);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType18, attNameExtension), this.TSB1MathSubType18);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount18, attNameExtension), this.TSB1TMAmount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit18, attNameExtension), this.TSB1TMUnit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount18, attNameExtension), this.TSB1TLAmount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit18, attNameExtension), this.TSB1TLUnit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount18, attNameExtension), this.TSB1TUAmount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit18, attNameExtension), this.TSB1TUUnit18);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator18, attNameExtension), this.TSB1MathOperator18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression18, attNameExtension), this.TSB1MathExpression18);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N18, attNameExtension), this.TSB1N18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount18, attNameExtension), this.TSB15Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit18, attNameExtension), this.TSB15Unit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount18, attNameExtension), this.TSB11Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit18, attNameExtension), this.TSB11Unit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount18, attNameExtension), this.TSB12Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit18, attNameExtension), this.TSB12Unit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount18, attNameExtension), this.TSB13Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit18, attNameExtension), this.TSB13Unit18);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount18, attNameExtension), this.TSB14Amount18);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit18, attNameExtension), this.TSB14Unit18);
            }
            if (this.TSB1Name19 != string.Empty && this.TSB1Name19 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name19, attNameExtension), this.TSB1Name19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label19, attNameExtension), this.TSB1Label19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description19, attNameExtension), this.TSB1Description19);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date19, attNameExtension), this.TSB1Date19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType19, attNameExtension), this.TSB1MathType19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO19, attNameExtension), this.TSB1BaseIO19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type19, attNameExtension), this.TSB1Type19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel19, attNameExtension), this.TSB1RelLabel19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount19, attNameExtension), this.TSB1TAmount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit19, attNameExtension), this.TSB1TUnit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount19, attNameExtension), this.TSB1TD1Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit19, attNameExtension), this.TSB1TD1Unit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount19, attNameExtension), this.TSB1TD2Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit19, attNameExtension), this.TSB1TD2Unit19);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult19, attNameExtension), this.TSB1MathResult19);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType19, attNameExtension), this.TSB1MathSubType19);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount19, attNameExtension), this.TSB1TMAmount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit19, attNameExtension), this.TSB1TMUnit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount19, attNameExtension), this.TSB1TLAmount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit19, attNameExtension), this.TSB1TLUnit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount19, attNameExtension), this.TSB1TUAmount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit19, attNameExtension), this.TSB1TUUnit19);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator19, attNameExtension), this.TSB1MathOperator19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression19, attNameExtension), this.TSB1MathExpression19);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N19, attNameExtension), this.TSB1N19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount19, attNameExtension), this.TSB15Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit19, attNameExtension), this.TSB15Unit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount19, attNameExtension), this.TSB11Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit19, attNameExtension), this.TSB11Unit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount19, attNameExtension), this.TSB12Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit19, attNameExtension), this.TSB12Unit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount19, attNameExtension), this.TSB13Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit19, attNameExtension), this.TSB13Unit19);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount19, attNameExtension), this.TSB14Amount19);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit19, attNameExtension), this.TSB14Unit19);
            }
            if (this.TSB1Name20 != string.Empty && this.TSB1Name20 != Constants.NONE)
            {
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Name20, attNameExtension), this.TSB1Name20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Label20, attNameExtension), this.TSB1Label20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Description20, attNameExtension), this.TSB1Description20);
                CalculatorHelpers.SetAttributeDateS(calculator,
                        string.Concat(cTSB1Date20, attNameExtension), this.TSB1Date20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathType20, attNameExtension), this.TSB1MathType20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1BaseIO20, attNameExtension), this.TSB1BaseIO20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1Type20, attNameExtension), this.TSB1Type20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1RelLabel20, attNameExtension), this.TSB1RelLabel20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TAmount20, attNameExtension), this.TSB1TAmount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUnit20, attNameExtension), this.TSB1TUnit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TD1Amount20, attNameExtension), this.TSB1TD1Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD1Unit20, attNameExtension), this.TSB1TD1Unit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TD2Amount20, attNameExtension), this.TSB1TD2Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TD2Unit20, attNameExtension), this.TSB1TD2Unit20);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathResult20, attNameExtension), this.TSB1MathResult20);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathSubType20, attNameExtension), this.TSB1MathSubType20);

                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TMAmount20, attNameExtension), this.TSB1TMAmount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TMUnit20, attNameExtension), this.TSB1TMUnit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB1TLAmount20, attNameExtension), this.TSB1TLAmount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TLUnit20, attNameExtension), this.TSB1TLUnit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB1TUAmount20, attNameExtension), this.TSB1TUAmount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1TUUnit20, attNameExtension), this.TSB1TUUnit20);
                CalculatorHelpers.SetAttribute(calculator,
                       string.Concat(cTSB1MathOperator20, attNameExtension), this.TSB1MathOperator20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB1MathExpression20, attNameExtension), this.TSB1MathExpression20);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                        string.Concat(cTSB1N20, attNameExtension), this.TSB1N20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTSB15Amount20, attNameExtension), this.TSB15Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB15Unit20, attNameExtension), this.TSB15Unit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB11Amount20, attNameExtension), this.TSB11Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                        string.Concat(cTSB11Unit20, attNameExtension), this.TSB11Unit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                        string.Concat(cTSB12Amount20, attNameExtension), this.TSB12Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB12Unit20, attNameExtension), this.TSB12Unit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB13Amount20, attNameExtension), this.TSB13Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB13Unit20, attNameExtension), this.TSB13Unit20);
                CalculatorHelpers.SetAttributeDoubleN4(calculator,
                       string.Concat(cTSB14Amount20, attNameExtension), this.TSB14Amount20);
                CalculatorHelpers.SetAttribute(calculator,
                    string.Concat(cTSB14Unit20, attNameExtension), this.TSB14Unit20);
            }
        }
        public virtual void SetTSB1BaseStockAttributes(string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
               string.Concat(cTSB1Score, attNameExtension), this.TSB1Score.ToString("N4", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreUnit, attNameExtension), this.TSB1ScoreUnit);
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreD1Amount, attNameExtension), this.TSB1ScoreD1Amount.ToString("N4", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreD1Unit, attNameExtension), this.TSB1ScoreD1Unit);
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreD2Amount, attNameExtension), this.TSB1ScoreD2Amount.ToString("N4", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreD2Unit, attNameExtension), this.TSB1ScoreD2Unit);
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreMathExpression, attNameExtension), this.TSB1ScoreMathExpression);
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreM, attNameExtension), this.TSB1ScoreM.ToString("N4", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreMUnit, attNameExtension), this.TSB1ScoreMUnit);
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreLAmount, attNameExtension), this.TSB1ScoreLAmount.ToString("N4", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreLUnit, attNameExtension), this.TSB1ScoreLUnit);
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreUAmount, attNameExtension), this.TSB1ScoreUAmount.ToString("N4", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreUUnit, attNameExtension), this.TSB1ScoreUUnit);
            writer.WriteAttributeString(
                string.Concat(cTSB1Iterations, attNameExtension), this.TSB1Iterations.ToString());
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreDistType, attNameExtension), this.TSB1ScoreDistType.ToString());
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreMathType, attNameExtension), this.TSB1ScoreMathType.ToString());
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreMathResult, attNameExtension), this.TSB1ScoreMathResult.ToString());
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreMathSubType, attNameExtension), this.TSB1ScoreMathSubType.ToString());
            writer.WriteAttributeString(
                string.Concat(cTSB1ScoreN, attNameExtension), this.TSB1ScoreN.ToString("N1", CultureInfo.InvariantCulture));
            if (this.TSB1Name1 != string.Empty && this.TSB1Name1 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name1, attNameExtension), this.TSB1Name1);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description1, attNameExtension), this.TSB1Description1);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label1, attNameExtension), this.TSB1Label1);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type1, attNameExtension), this.TSB1Type1);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel1, attNameExtension), this.TSB1RelLabel1);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount1, attNameExtension), this.TSB1TAmount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit1, attNameExtension), this.TSB1TUnit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount1, attNameExtension), this.TSB1TD1Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit1, attNameExtension), this.TSB1TD1Unit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount1, attNameExtension), this.TSB1TD2Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit1, attNameExtension), this.TSB1TD2Unit1.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult1, attNameExtension), this.TSB1MathResult1.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType1, attNameExtension), this.TSB1MathSubType1.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount1, attNameExtension), this.TSB1TMAmount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit1, attNameExtension), this.TSB1TMUnit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount1, attNameExtension), this.TSB1TLAmount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit1, attNameExtension), this.TSB1TLUnit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount1, attNameExtension), this.TSB1TUAmount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit1, attNameExtension), this.TSB1TUUnit1.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator1, attNameExtension), this.TSB1MathOperator1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression1, attNameExtension), this.TSB1MathExpression1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N1, attNameExtension), this.TSB1N1.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date1, attNameExtension), this.TSB1Date1.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType1, attNameExtension), this.TSB1MathType1);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO1, attNameExtension), this.TSB1BaseIO1);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount1, attNameExtension), this.TSB11Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit1, attNameExtension), this.TSB11Unit1.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount1, attNameExtension), this.TSB12Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit1, attNameExtension), this.TSB12Unit1);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount1, attNameExtension), this.TSB15Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit1, attNameExtension), this.TSB15Unit1.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB13Amount1, attNameExtension), this.TSB13Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit1, attNameExtension), this.TSB13Unit1);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount1, attNameExtension), this.TSB14Amount1.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit1, attNameExtension), this.TSB14Unit1);
            }
            if (this.TSB1Name2 != string.Empty && this.TSB1Name2 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name2, attNameExtension), this.TSB1Name2);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description2, attNameExtension), this.TSB1Description2);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label2, attNameExtension), this.TSB1Label2);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type2, attNameExtension), this.TSB1Type2);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel2, attNameExtension), this.TSB1RelLabel2);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount2, attNameExtension), this.TSB1TAmount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit2, attNameExtension), this.TSB1TUnit2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount2, attNameExtension), this.TSB1TD1Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit2, attNameExtension), this.TSB1TD1Unit2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount2, attNameExtension), this.TSB1TD2Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit2, attNameExtension), this.TSB1TD2Unit2.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult2, attNameExtension), this.TSB1MathResult2.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType2, attNameExtension), this.TSB1MathSubType2.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount2, attNameExtension), this.TSB1TMAmount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit2, attNameExtension), this.TSB1TMUnit2.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB1TLAmount2, attNameExtension), this.TSB1TLAmount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit2, attNameExtension), this.TSB1TLUnit2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount2, attNameExtension), this.TSB1TUAmount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit2, attNameExtension), this.TSB1TUUnit2.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator2, attNameExtension), this.TSB1MathOperator2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression2, attNameExtension), this.TSB1MathExpression2.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N2, attNameExtension), this.TSB1N2.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date2, attNameExtension), this.TSB1Date2.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType2, attNameExtension), this.TSB1MathType2);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO2, attNameExtension), this.TSB1BaseIO2);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount2, attNameExtension), this.TSB11Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit2, attNameExtension), this.TSB11Unit2.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount2, attNameExtension), this.TSB12Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit2, attNameExtension), this.TSB12Unit2);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount2, attNameExtension), this.TSB15Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit2, attNameExtension), this.TSB15Unit2.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount2, attNameExtension), this.TSB13Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit2, attNameExtension), this.TSB13Unit2);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount2, attNameExtension), this.TSB14Amount2.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit2, attNameExtension), this.TSB14Unit2);
            }
            if (this.TSB1Name3 != string.Empty && this.TSB1Name3 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name3, attNameExtension), this.TSB1Name3);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description3, attNameExtension), this.TSB1Description3);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label3, attNameExtension), this.TSB1Label3);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type3, attNameExtension), this.TSB1Type3);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel3, attNameExtension), this.TSB1RelLabel3);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount3, attNameExtension), this.TSB1TAmount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit3, attNameExtension), this.TSB1TUnit3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount3, attNameExtension), this.TSB1TD1Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit3, attNameExtension), this.TSB1TD1Unit3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount3, attNameExtension), this.TSB1TD2Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit3, attNameExtension), this.TSB1TD2Unit3.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult3, attNameExtension), this.TSB1MathResult3.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType3, attNameExtension), this.TSB1MathSubType3.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount3, attNameExtension), this.TSB1TMAmount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit3, attNameExtension), this.TSB1TMUnit3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount3, attNameExtension), this.TSB1TLAmount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit3, attNameExtension), this.TSB1TLUnit3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount3, attNameExtension), this.TSB1TUAmount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit3, attNameExtension), this.TSB1TUUnit3.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator3, attNameExtension), this.TSB1MathOperator3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression3, attNameExtension), this.TSB1MathExpression3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N3, attNameExtension), this.TSB1N3.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date3, attNameExtension), this.TSB1Date3.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType3, attNameExtension), this.TSB1MathType3);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO3, attNameExtension), this.TSB1BaseIO3);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount3, attNameExtension), this.TSB11Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit3, attNameExtension), this.TSB11Unit3.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount3, attNameExtension), this.TSB12Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit3, attNameExtension), this.TSB12Unit3);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount3, attNameExtension), this.TSB15Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit3, attNameExtension), this.TSB15Unit3.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB13Amount3, attNameExtension), this.TSB13Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit3, attNameExtension), this.TSB13Unit3);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount3, attNameExtension), this.TSB14Amount3.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit3, attNameExtension), this.TSB14Unit3);
            }
            if (this.TSB1Name4 != string.Empty && this.TSB1Name4 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name4, attNameExtension), this.TSB1Name4);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description4, attNameExtension), this.TSB1Description4);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label4, attNameExtension), this.TSB1Label4);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type4, attNameExtension), this.TSB1Type4);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel4, attNameExtension), this.TSB1RelLabel4);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount4, attNameExtension), this.TSB1TAmount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit4, attNameExtension), this.TSB1TUnit4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount4, attNameExtension), this.TSB1TD1Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit4, attNameExtension), this.TSB1TD1Unit4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount4, attNameExtension), this.TSB1TD2Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit4, attNameExtension), this.TSB1TD2Unit4.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult4, attNameExtension), this.TSB1MathResult4.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType4, attNameExtension), this.TSB1MathSubType4.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount4, attNameExtension), this.TSB1TMAmount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit4, attNameExtension), this.TSB1TMUnit4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount4, attNameExtension), this.TSB1TLAmount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit4, attNameExtension), this.TSB1TLUnit4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount4, attNameExtension), this.TSB1TUAmount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit4, attNameExtension), this.TSB1TUUnit4.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator4, attNameExtension), this.TSB1MathOperator4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression4, attNameExtension), this.TSB1MathExpression4.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N4, attNameExtension), this.TSB1N4.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date4, attNameExtension), this.TSB1Date4.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType4, attNameExtension), this.TSB1MathType4);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO4, attNameExtension), this.TSB1BaseIO4);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount4, attNameExtension), this.TSB11Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit4, attNameExtension), this.TSB11Unit4.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount4, attNameExtension), this.TSB12Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit4, attNameExtension), this.TSB12Unit4);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount4, attNameExtension), this.TSB15Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit4, attNameExtension), this.TSB15Unit4.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount4, attNameExtension), this.TSB13Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit4, attNameExtension), this.TSB13Unit4);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount4, attNameExtension), this.TSB14Amount4.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit4, attNameExtension), this.TSB14Unit4);
            }
            if (this.TSB1Name5 != string.Empty && this.TSB1Name5 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name5, attNameExtension), this.TSB1Name5);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description5, attNameExtension), this.TSB1Description5);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label5, attNameExtension), this.TSB1Label5);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type5, attNameExtension), this.TSB1Type5);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel5, attNameExtension), this.TSB1RelLabel5);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount5, attNameExtension), this.TSB1TAmount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit5, attNameExtension), this.TSB1TUnit5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount5, attNameExtension), this.TSB1TD1Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit5, attNameExtension), this.TSB1TD1Unit5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount5, attNameExtension), this.TSB1TD2Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit5, attNameExtension), this.TSB1TD2Unit5.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult5, attNameExtension), this.TSB1MathResult5.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType5, attNameExtension), this.TSB1MathSubType5.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount5, attNameExtension), this.TSB1TMAmount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit5, attNameExtension), this.TSB1TMUnit5.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB1TLAmount5, attNameExtension), this.TSB1TLAmount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit5, attNameExtension), this.TSB1TLUnit5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount5, attNameExtension), this.TSB1TUAmount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit5, attNameExtension), this.TSB1TUUnit5.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator5, attNameExtension), this.TSB1MathOperator5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression5, attNameExtension), this.TSB1MathExpression5.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N5, attNameExtension), this.TSB1N5.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date5, attNameExtension), this.TSB1Date5.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType5, attNameExtension), this.TSB1MathType5);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO5, attNameExtension), this.TSB1BaseIO5);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount5, attNameExtension), this.TSB11Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit5, attNameExtension), this.TSB11Unit5.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount5, attNameExtension), this.TSB12Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit5, attNameExtension), this.TSB12Unit5);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount5, attNameExtension), this.TSB15Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit5, attNameExtension), this.TSB15Unit5.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount5, attNameExtension), this.TSB13Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit5, attNameExtension), this.TSB13Unit5);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount5, attNameExtension), this.TSB14Amount5.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit5, attNameExtension), this.TSB14Unit5);
            }
            if (this.TSB1Name6 != string.Empty && this.TSB1Name6 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name6, attNameExtension), this.TSB1Name6);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description6, attNameExtension), this.TSB1Description6);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label6, attNameExtension), this.TSB1Label6);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type6, attNameExtension), this.TSB1Type6);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel6, attNameExtension), this.TSB1RelLabel6);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount6, attNameExtension), this.TSB1TAmount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit6, attNameExtension), this.TSB1TUnit6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount6, attNameExtension), this.TSB1TD1Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit6, attNameExtension), this.TSB1TD1Unit6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount6, attNameExtension), this.TSB1TD2Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit6, attNameExtension), this.TSB1TD2Unit6.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult6, attNameExtension), this.TSB1MathResult6.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType6, attNameExtension), this.TSB1MathSubType6.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount6, attNameExtension), this.TSB1TMAmount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit6, attNameExtension), this.TSB1TMUnit6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount6, attNameExtension), this.TSB1TLAmount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit6, attNameExtension), this.TSB1TLUnit6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount6, attNameExtension), this.TSB1TUAmount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit6, attNameExtension), this.TSB1TUUnit6.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator6, attNameExtension), this.TSB1MathOperator6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression6, attNameExtension), this.TSB1MathExpression6.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N6, attNameExtension), this.TSB1N6.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date6, attNameExtension), this.TSB1Date6.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType6, attNameExtension), this.TSB1MathType6);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO6, attNameExtension), this.TSB1BaseIO6);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount6, attNameExtension), this.TSB11Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit6, attNameExtension), this.TSB11Unit6.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount6, attNameExtension), this.TSB12Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit6, attNameExtension), this.TSB12Unit6);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount6, attNameExtension), this.TSB15Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit6, attNameExtension), this.TSB15Unit6.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount6, attNameExtension), this.TSB13Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit6, attNameExtension), this.TSB13Unit6);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount6, attNameExtension), this.TSB14Amount6.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit6, attNameExtension), this.TSB14Unit6);
            }
            if (this.TSB1Name7 != string.Empty && this.TSB1Name7 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name7, attNameExtension), this.TSB1Name7);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description7, attNameExtension), this.TSB1Description7);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label7, attNameExtension), this.TSB1Label7);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type7, attNameExtension), this.TSB1Type7);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel7, attNameExtension), this.TSB1RelLabel7);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount7, attNameExtension), this.TSB1TAmount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit7, attNameExtension), this.TSB1TUnit7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount7, attNameExtension), this.TSB1TD1Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit7, attNameExtension), this.TSB1TD1Unit7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount7, attNameExtension), this.TSB1TD2Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit7, attNameExtension), this.TSB1TD2Unit7.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult7, attNameExtension), this.TSB1MathResult7.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType7, attNameExtension), this.TSB1MathSubType7.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount7, attNameExtension), this.TSB1TMAmount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit7, attNameExtension), this.TSB1TMUnit7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount7, attNameExtension), this.TSB1TLAmount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit7, attNameExtension), this.TSB1TLUnit7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount7, attNameExtension), this.TSB1TUAmount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit7, attNameExtension), this.TSB1TUUnit7.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator7, attNameExtension), this.TSB1MathOperator7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression7, attNameExtension), this.TSB1MathExpression7.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N7, attNameExtension), this.TSB1N7.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date7, attNameExtension), this.TSB1Date7.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType7, attNameExtension), this.TSB1MathType7);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO7, attNameExtension), this.TSB1BaseIO7);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount7, attNameExtension), this.TSB11Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit7, attNameExtension), this.TSB11Unit7.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount7, attNameExtension), this.TSB12Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit7, attNameExtension), this.TSB12Unit7);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount7, attNameExtension), this.TSB15Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit7, attNameExtension), this.TSB15Unit7.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount7, attNameExtension), this.TSB13Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit7, attNameExtension), this.TSB13Unit7);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount7, attNameExtension), this.TSB14Amount7.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit7, attNameExtension), this.TSB14Unit7);
            }
            if (this.TSB1Name8 != string.Empty && this.TSB1Name8 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name8, attNameExtension), this.TSB1Name8);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description8, attNameExtension), this.TSB1Description8);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label8, attNameExtension), this.TSB1Label8);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type8, attNameExtension), this.TSB1Type8);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel8, attNameExtension), this.TSB1RelLabel8);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount8, attNameExtension), this.TSB1TAmount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit8, attNameExtension), this.TSB1TUnit8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount8, attNameExtension), this.TSB1TD1Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit8, attNameExtension), this.TSB1TD1Unit8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount8, attNameExtension), this.TSB1TD2Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit8, attNameExtension), this.TSB1TD2Unit8.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult8, attNameExtension), this.TSB1MathResult8.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType8, attNameExtension), this.TSB1MathSubType8.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount8, attNameExtension), this.TSB1TMAmount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit8, attNameExtension), this.TSB1TMUnit8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount8, attNameExtension), this.TSB1TLAmount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit8, attNameExtension), this.TSB1TLUnit8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount8, attNameExtension), this.TSB1TUAmount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit8, attNameExtension), this.TSB1TUUnit8.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator8, attNameExtension), this.TSB1MathOperator8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression8, attNameExtension), this.TSB1MathExpression8.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N8, attNameExtension), this.TSB1N8.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date8, attNameExtension), this.TSB1Date8.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType8, attNameExtension), this.TSB1MathType8);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO8, attNameExtension), this.TSB1BaseIO8);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount8, attNameExtension), this.TSB11Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit8, attNameExtension), this.TSB11Unit8.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount8, attNameExtension), this.TSB12Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit8, attNameExtension), this.TSB12Unit8);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount8, attNameExtension), this.TSB15Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit8, attNameExtension), this.TSB15Unit8.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount8, attNameExtension), this.TSB13Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit8, attNameExtension), this.TSB13Unit8);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount8, attNameExtension), this.TSB14Amount8.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit8, attNameExtension), this.TSB14Unit8);
            }
            if (this.TSB1Name9 != string.Empty && this.TSB1Name9 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name9, attNameExtension), this.TSB1Name9);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description9, attNameExtension), this.TSB1Description9);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label9, attNameExtension), this.TSB1Label9);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type9, attNameExtension), this.TSB1Type9);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel9, attNameExtension), this.TSB1RelLabel9);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount9, attNameExtension), this.TSB1TAmount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit9, attNameExtension), this.TSB1TUnit9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount9, attNameExtension), this.TSB1TD1Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit9, attNameExtension), this.TSB1TD1Unit9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount9, attNameExtension), this.TSB1TD2Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit9, attNameExtension), this.TSB1TD2Unit9.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult9, attNameExtension), this.TSB1MathResult9.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType9, attNameExtension), this.TSB1MathSubType9.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount9, attNameExtension), this.TSB1TMAmount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit9, attNameExtension), this.TSB1TMUnit9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount9, attNameExtension), this.TSB1TLAmount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit9, attNameExtension), this.TSB1TLUnit9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount9, attNameExtension), this.TSB1TUAmount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit9, attNameExtension), this.TSB1TUUnit9.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator9, attNameExtension), this.TSB1MathOperator9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression9, attNameExtension), this.TSB1MathExpression9.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N9, attNameExtension), this.TSB1N9.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date9, attNameExtension), this.TSB1Date9.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType9, attNameExtension), this.TSB1MathType9);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO9, attNameExtension), this.TSB1BaseIO9);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount9, attNameExtension), this.TSB11Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit9, attNameExtension), this.TSB11Unit9.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount9, attNameExtension), this.TSB12Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit9, attNameExtension), this.TSB12Unit9);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount9, attNameExtension), this.TSB15Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit9, attNameExtension), this.TSB15Unit9.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount9, attNameExtension), this.TSB13Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit9, attNameExtension), this.TSB13Unit9);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount9, attNameExtension), this.TSB14Amount9.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit9, attNameExtension), this.TSB14Unit9);
            }
            if (this.TSB1Name10 != string.Empty && this.TSB1Name10 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name10, attNameExtension), this.TSB1Name10);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description10, attNameExtension), this.TSB1Description10);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label10, attNameExtension), this.TSB1Label10);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type10, attNameExtension), this.TSB1Type10);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel10, attNameExtension), this.TSB1RelLabel10);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount10, attNameExtension), this.TSB1TAmount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit10, attNameExtension), this.TSB1TUnit10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount10, attNameExtension), this.TSB1TD1Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit10, attNameExtension), this.TSB1TD1Unit10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount10, attNameExtension), this.TSB1TD2Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit10, attNameExtension), this.TSB1TD2Unit10.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult10, attNameExtension), this.TSB1MathResult10.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType10, attNameExtension), this.TSB1MathSubType10.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount10, attNameExtension), this.TSB1TMAmount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit10, attNameExtension), this.TSB1TMUnit10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount10, attNameExtension), this.TSB1TLAmount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit10, attNameExtension), this.TSB1TLUnit10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount10, attNameExtension), this.TSB1TUAmount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit10, attNameExtension), this.TSB1TUUnit10.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator10, attNameExtension), this.TSB1MathOperator10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression10, attNameExtension), this.TSB1MathExpression10.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N10, attNameExtension), this.TSB1N10.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date10, attNameExtension), this.TSB1Date10.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType10, attNameExtension), this.TSB1MathType10);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO10, attNameExtension), this.TSB1BaseIO10);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount10, attNameExtension), this.TSB11Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit10, attNameExtension), this.TSB11Unit10.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount10, attNameExtension), this.TSB12Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit10, attNameExtension), this.TSB12Unit10);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount10, attNameExtension), this.TSB15Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit10, attNameExtension), this.TSB15Unit10.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount10, attNameExtension), this.TSB13Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit10, attNameExtension), this.TSB13Unit10);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount10, attNameExtension), this.TSB14Amount10.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit10, attNameExtension), this.TSB14Unit10);
            }
            if (this.TSB1Name11 != string.Empty && this.TSB1Name11 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name11, attNameExtension), this.TSB1Name11);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description11, attNameExtension), this.TSB1Description11);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label11, attNameExtension), this.TSB1Label11);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type11, attNameExtension), this.TSB1Type11);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel11, attNameExtension), this.TSB1RelLabel11);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount11, attNameExtension), this.TSB1TAmount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit11, attNameExtension), this.TSB1TUnit11.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount11, attNameExtension), this.TSB1TD1Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit11, attNameExtension), this.TSB1TD1Unit11.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount11, attNameExtension), this.TSB1TD2Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit11, attNameExtension), this.TSB1TD2Unit11.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult11, attNameExtension), this.TSB1MathResult11.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType11, attNameExtension), this.TSB1MathSubType11.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount11, attNameExtension), this.TSB1TMAmount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit11, attNameExtension), this.TSB1TMUnit11.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount11, attNameExtension), this.TSB1TLAmount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit11, attNameExtension), this.TSB1TLUnit11.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount11, attNameExtension), this.TSB1TUAmount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit11, attNameExtension), this.TSB1TUUnit11.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator11, attNameExtension), this.TSB1MathOperator11.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression11, attNameExtension), this.TSB1MathExpression11.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N11, attNameExtension), this.TSB1N11.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date11, attNameExtension), this.TSB1Date11.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType11, attNameExtension), this.TSB1MathType11);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO11, attNameExtension), this.TSB1BaseIO11);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount11, attNameExtension), this.TSB11Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit11, attNameExtension), this.TSB11Unit11.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount11, attNameExtension), this.TSB12Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit11, attNameExtension), this.TSB12Unit11);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount11, attNameExtension), this.TSB15Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit11, attNameExtension), this.TSB15Unit11.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount11, attNameExtension), this.TSB13Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit11, attNameExtension), this.TSB13Unit11);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount11, attNameExtension), this.TSB14Amount11.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit11, attNameExtension), this.TSB14Unit11);
            }
            if (this.TSB1Name12 != string.Empty && this.TSB1Name12 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name12, attNameExtension), this.TSB1Name12);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description12, attNameExtension), this.TSB1Description12);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label12, attNameExtension), this.TSB1Label12);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type12, attNameExtension), this.TSB1Type12);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel12, attNameExtension), this.TSB1RelLabel12);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount12, attNameExtension), this.TSB1TAmount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit12, attNameExtension), this.TSB1TUnit12.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount12, attNameExtension), this.TSB1TD1Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit12, attNameExtension), this.TSB1TD1Unit12.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount12, attNameExtension), this.TSB1TD2Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit12, attNameExtension), this.TSB1TD2Unit12.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult12, attNameExtension), this.TSB1MathResult12.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType12, attNameExtension), this.TSB1MathSubType12.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount12, attNameExtension), this.TSB1TMAmount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit12, attNameExtension), this.TSB1TMUnit12.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount12, attNameExtension), this.TSB1TLAmount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit12, attNameExtension), this.TSB1TLUnit12.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount12, attNameExtension), this.TSB1TUAmount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit12, attNameExtension), this.TSB1TUUnit12.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator12, attNameExtension), this.TSB1MathOperator12.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression12, attNameExtension), this.TSB1MathExpression12.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N12, attNameExtension), this.TSB1N12.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date12, attNameExtension), this.TSB1Date12.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType12, attNameExtension), this.TSB1MathType12);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO12, attNameExtension), this.TSB1BaseIO12);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount12, attNameExtension), this.TSB11Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit12, attNameExtension), this.TSB11Unit12.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount12, attNameExtension), this.TSB12Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit12, attNameExtension), this.TSB12Unit12);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount12, attNameExtension), this.TSB15Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit12, attNameExtension), this.TSB15Unit12.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount12, attNameExtension), this.TSB13Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit12, attNameExtension), this.TSB13Unit12);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount12, attNameExtension), this.TSB14Amount12.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit12, attNameExtension), this.TSB14Unit12);
            }
            if (this.TSB1Name13 != string.Empty && this.TSB1Name13 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name13, attNameExtension), this.TSB1Name13);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description13, attNameExtension), this.TSB1Description13);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label13, attNameExtension), this.TSB1Label13);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type13, attNameExtension), this.TSB1Type13);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel13, attNameExtension), this.TSB1RelLabel13);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount13, attNameExtension), this.TSB1TAmount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit13, attNameExtension), this.TSB1TUnit13.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount13, attNameExtension), this.TSB1TD1Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit13, attNameExtension), this.TSB1TD1Unit13.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount13, attNameExtension), this.TSB1TD2Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit13, attNameExtension), this.TSB1TD2Unit13.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult13, attNameExtension), this.TSB1MathResult13.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType13, attNameExtension), this.TSB1MathSubType13.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount13, attNameExtension), this.TSB1TMAmount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit13, attNameExtension), this.TSB1TMUnit13.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount13, attNameExtension), this.TSB1TLAmount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit13, attNameExtension), this.TSB1TLUnit13.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount13, attNameExtension), this.TSB1TUAmount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit13, attNameExtension), this.TSB1TUUnit13.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator13, attNameExtension), this.TSB1MathOperator13.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression13, attNameExtension), this.TSB1MathExpression13.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N13, attNameExtension), this.TSB1N13.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date13, attNameExtension), this.TSB1Date13.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType13, attNameExtension), this.TSB1MathType13);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO13, attNameExtension), this.TSB1BaseIO13);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount13, attNameExtension), this.TSB11Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit13, attNameExtension), this.TSB11Unit13.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount13, attNameExtension), this.TSB12Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit13, attNameExtension), this.TSB12Unit13);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount13, attNameExtension), this.TSB15Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit13, attNameExtension), this.TSB15Unit13.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount13, attNameExtension), this.TSB13Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit13, attNameExtension), this.TSB13Unit13);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount13, attNameExtension), this.TSB14Amount13.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit13, attNameExtension), this.TSB14Unit13);
            }
            if (this.TSB1Name14 != string.Empty && this.TSB1Name14 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name14, attNameExtension), this.TSB1Name14);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description14, attNameExtension), this.TSB1Description14);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label14, attNameExtension), this.TSB1Label14);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type14, attNameExtension), this.TSB1Type14);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel14, attNameExtension), this.TSB1RelLabel14);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount14, attNameExtension), this.TSB1TAmount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit14, attNameExtension), this.TSB1TUnit14.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount14, attNameExtension), this.TSB1TD1Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit14, attNameExtension), this.TSB1TD1Unit14.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount14, attNameExtension), this.TSB1TD2Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit14, attNameExtension), this.TSB1TD2Unit14.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult14, attNameExtension), this.TSB1MathResult14.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType14, attNameExtension), this.TSB1MathSubType14.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount14, attNameExtension), this.TSB1TMAmount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit14, attNameExtension), this.TSB1TMUnit14.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount14, attNameExtension), this.TSB1TLAmount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit14, attNameExtension), this.TSB1TLUnit14.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount14, attNameExtension), this.TSB1TUAmount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit14, attNameExtension), this.TSB1TUUnit14.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator14, attNameExtension), this.TSB1MathOperator14.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression14, attNameExtension), this.TSB1MathExpression14.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N14, attNameExtension), this.TSB1N14.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date14, attNameExtension), this.TSB1Date14.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType14, attNameExtension), this.TSB1MathType14);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO14, attNameExtension), this.TSB1BaseIO14);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount14, attNameExtension), this.TSB11Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit14, attNameExtension), this.TSB11Unit14.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount14, attNameExtension), this.TSB12Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit14, attNameExtension), this.TSB12Unit14);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount14, attNameExtension), this.TSB15Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit14, attNameExtension), this.TSB15Unit14.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount14, attNameExtension), this.TSB13Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit14, attNameExtension), this.TSB13Unit14);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount14, attNameExtension), this.TSB14Amount14.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit14, attNameExtension), this.TSB14Unit14);
            }
            if (this.TSB1Name15 != string.Empty && this.TSB1Name15 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name15, attNameExtension), this.TSB1Name15);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description15, attNameExtension), this.TSB1Description15);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label15, attNameExtension), this.TSB1Label15);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type15, attNameExtension), this.TSB1Type15);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel15, attNameExtension), this.TSB1RelLabel15);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount15, attNameExtension), this.TSB1TAmount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit15, attNameExtension), this.TSB1TUnit15.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount15, attNameExtension), this.TSB1TD1Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit15, attNameExtension), this.TSB1TD1Unit15.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount15, attNameExtension), this.TSB1TD2Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit15, attNameExtension), this.TSB1TD2Unit15.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult15, attNameExtension), this.TSB1MathResult15.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType15, attNameExtension), this.TSB1MathSubType15.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount15, attNameExtension), this.TSB1TMAmount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit15, attNameExtension), this.TSB1TMUnit15.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount15, attNameExtension), this.TSB1TLAmount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit15, attNameExtension), this.TSB1TLUnit15.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount15, attNameExtension), this.TSB1TUAmount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit15, attNameExtension), this.TSB1TUUnit15.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator15, attNameExtension), this.TSB1MathOperator15.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression15, attNameExtension), this.TSB1MathExpression15.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N15, attNameExtension), this.TSB1N15.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date15, attNameExtension), this.TSB1Date15.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType15, attNameExtension), this.TSB1MathType15);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO15, attNameExtension), this.TSB1BaseIO15);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount15, attNameExtension), this.TSB11Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit15, attNameExtension), this.TSB11Unit15.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount15, attNameExtension), this.TSB12Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit15, attNameExtension), this.TSB12Unit15);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount15, attNameExtension), this.TSB15Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit15, attNameExtension), this.TSB15Unit15.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount15, attNameExtension), this.TSB13Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit15, attNameExtension), this.TSB13Unit15);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount15, attNameExtension), this.TSB14Amount15.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit15, attNameExtension), this.TSB14Unit15);
            }
            if (this.TSB1Name16 != string.Empty && this.TSB1Name16 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name16, attNameExtension), this.TSB1Name16);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description16, attNameExtension), this.TSB1Description16);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label16, attNameExtension), this.TSB1Label16);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type16, attNameExtension), this.TSB1Type16);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel16, attNameExtension), this.TSB1RelLabel16);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount16, attNameExtension), this.TSB1TAmount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit16, attNameExtension), this.TSB1TUnit16.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount16, attNameExtension), this.TSB1TD1Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit16, attNameExtension), this.TSB1TD1Unit16.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount16, attNameExtension), this.TSB1TD2Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit16, attNameExtension), this.TSB1TD2Unit16.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult16, attNameExtension), this.TSB1MathResult16.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType16, attNameExtension), this.TSB1MathSubType16.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount16, attNameExtension), this.TSB1TMAmount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit16, attNameExtension), this.TSB1TMUnit16.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount16, attNameExtension), this.TSB1TLAmount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit16, attNameExtension), this.TSB1TLUnit16.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount16, attNameExtension), this.TSB1TUAmount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit16, attNameExtension), this.TSB1TUUnit16.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator16, attNameExtension), this.TSB1MathOperator16.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression16, attNameExtension), this.TSB1MathExpression16.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N16, attNameExtension), this.TSB1N16.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date16, attNameExtension), this.TSB1Date16.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType16, attNameExtension), this.TSB1MathType16);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO16, attNameExtension), this.TSB1BaseIO16);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount16, attNameExtension), this.TSB11Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit16, attNameExtension), this.TSB11Unit16.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount16, attNameExtension), this.TSB12Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit16, attNameExtension), this.TSB12Unit16);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount16, attNameExtension), this.TSB15Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit16, attNameExtension), this.TSB15Unit16.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount16, attNameExtension), this.TSB13Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit16, attNameExtension), this.TSB13Unit16);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount16, attNameExtension), this.TSB14Amount16.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit16, attNameExtension), this.TSB14Unit16);
            }
            if (this.TSB1Name17 != string.Empty && this.TSB1Name17 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name17, attNameExtension), this.TSB1Name17);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description17, attNameExtension), this.TSB1Description17);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label17, attNameExtension), this.TSB1Label17);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type17, attNameExtension), this.TSB1Type17);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel17, attNameExtension), this.TSB1RelLabel17);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount17, attNameExtension), this.TSB1TAmount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit17, attNameExtension), this.TSB1TUnit17.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount17, attNameExtension), this.TSB1TD1Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit17, attNameExtension), this.TSB1TD1Unit17.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount17, attNameExtension), this.TSB1TD2Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit17, attNameExtension), this.TSB1TD2Unit17.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult17, attNameExtension), this.TSB1MathResult17.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType17, attNameExtension), this.TSB1MathSubType17.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount17, attNameExtension), this.TSB1TMAmount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit17, attNameExtension), this.TSB1TMUnit17.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB1TLAmount17, attNameExtension), this.TSB1TLAmount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit17, attNameExtension), this.TSB1TLUnit17.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount17, attNameExtension), this.TSB1TUAmount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit17, attNameExtension), this.TSB1TUUnit17.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator17, attNameExtension), this.TSB1MathOperator17.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression17, attNameExtension), this.TSB1MathExpression17.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N17, attNameExtension), this.TSB1N17.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date17, attNameExtension), this.TSB1Date17.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType17, attNameExtension), this.TSB1MathType17);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO17, attNameExtension), this.TSB1BaseIO17);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount17, attNameExtension), this.TSB11Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit17, attNameExtension), this.TSB11Unit17.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount17, attNameExtension), this.TSB12Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit17, attNameExtension), this.TSB12Unit17);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount17, attNameExtension), this.TSB15Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit17, attNameExtension), this.TSB15Unit17.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB13Amount17, attNameExtension), this.TSB13Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit17, attNameExtension), this.TSB13Unit17);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount17, attNameExtension), this.TSB14Amount17.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit17, attNameExtension), this.TSB14Unit17);
            }
            if (this.TSB1Name18 != string.Empty && this.TSB1Name18 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name18, attNameExtension), this.TSB1Name18);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description18, attNameExtension), this.TSB1Description18);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label18, attNameExtension), this.TSB1Label18);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type18, attNameExtension), this.TSB1Type18);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel18, attNameExtension), this.TSB1RelLabel18);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount18, attNameExtension), this.TSB1TAmount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit18, attNameExtension), this.TSB1TUnit18.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount18, attNameExtension), this.TSB1TD1Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit18, attNameExtension), this.TSB1TD1Unit18.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount18, attNameExtension), this.TSB1TD2Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit18, attNameExtension), this.TSB1TD2Unit18.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult18, attNameExtension), this.TSB1MathResult18.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType18, attNameExtension), this.TSB1MathSubType18.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount18, attNameExtension), this.TSB1TMAmount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit18, attNameExtension), this.TSB1TMUnit18.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount18, attNameExtension), this.TSB1TLAmount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit18, attNameExtension), this.TSB1TLUnit18.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount18, attNameExtension), this.TSB1TUAmount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit18, attNameExtension), this.TSB1TUUnit18.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator18, attNameExtension), this.TSB1MathOperator18.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression18, attNameExtension), this.TSB1MathExpression18.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N18, attNameExtension), this.TSB1N18.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date18, attNameExtension), this.TSB1Date18.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType18, attNameExtension), this.TSB1MathType18);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO18, attNameExtension), this.TSB1BaseIO18);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount18, attNameExtension), this.TSB11Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit18, attNameExtension), this.TSB11Unit18.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount18, attNameExtension), this.TSB12Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit18, attNameExtension), this.TSB12Unit18);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount18, attNameExtension), this.TSB15Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit18, attNameExtension), this.TSB15Unit18.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount18, attNameExtension), this.TSB13Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit18, attNameExtension), this.TSB13Unit18);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount18, attNameExtension), this.TSB14Amount18.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit18, attNameExtension), this.TSB14Unit18);
            }
            if (this.TSB1Name19 != string.Empty && this.TSB1Name19 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name19, attNameExtension), this.TSB1Name19);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description19, attNameExtension), this.TSB1Description19);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label19, attNameExtension), this.TSB1Label19);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type19, attNameExtension), this.TSB1Type19);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel19, attNameExtension), this.TSB1RelLabel19);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount19, attNameExtension), this.TSB1TAmount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit19, attNameExtension), this.TSB1TUnit19.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount19, attNameExtension), this.TSB1TD1Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit19, attNameExtension), this.TSB1TD1Unit19.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount19, attNameExtension), this.TSB1TD2Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit19, attNameExtension), this.TSB1TD2Unit19.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult19, attNameExtension), this.TSB1MathResult19.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType19, attNameExtension), this.TSB1MathSubType19.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount19, attNameExtension), this.TSB1TMAmount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit19, attNameExtension), this.TSB1TMUnit19.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount19, attNameExtension), this.TSB1TLAmount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit19, attNameExtension), this.TSB1TLUnit19.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount19, attNameExtension), this.TSB1TUAmount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit19, attNameExtension), this.TSB1TUUnit19.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator19, attNameExtension), this.TSB1MathOperator19.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression19, attNameExtension), this.TSB1MathExpression19.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N19, attNameExtension), this.TSB1N19.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date19, attNameExtension), this.TSB1Date19.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType19, attNameExtension), this.TSB1MathType19);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO19, attNameExtension), this.TSB1BaseIO19);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount19, attNameExtension), this.TSB11Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit19, attNameExtension), this.TSB11Unit19.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount19, attNameExtension), this.TSB12Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit19, attNameExtension), this.TSB12Unit19);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount19, attNameExtension), this.TSB15Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit19, attNameExtension), this.TSB15Unit19.ToString());
                writer.WriteAttributeString(
                   string.Concat(cTSB13Amount19, attNameExtension), this.TSB13Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit19, attNameExtension), this.TSB13Unit19);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount19, attNameExtension), this.TSB14Amount19.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit19, attNameExtension), this.TSB14Unit19);
            }
            if (this.TSB1Name20 != string.Empty && this.TSB1Name20 != Constants.NONE)
            {
                writer.WriteAttributeString(
                        string.Concat(cTSB1Name20, attNameExtension), this.TSB1Name20);
                writer.WriteAttributeString(
                    string.Concat(cTSB1Description20, attNameExtension), this.TSB1Description20);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Label20, attNameExtension), this.TSB1Label20);
                writer.WriteAttributeString(
                        string.Concat(cTSB1Type20, attNameExtension), this.TSB1Type20);
                writer.WriteAttributeString(
                    string.Concat(cTSB1RelLabel20, attNameExtension), this.TSB1RelLabel20);
                writer.WriteAttributeString(
                    string.Concat(cTSB1TAmount20, attNameExtension), this.TSB1TAmount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUnit20, attNameExtension), this.TSB1TUnit20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Amount20, attNameExtension), this.TSB1TD1Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD1Unit20, attNameExtension), this.TSB1TD1Unit20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Amount20, attNameExtension), this.TSB1TD2Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TD2Unit20, attNameExtension), this.TSB1TD2Unit20.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathResult20, attNameExtension), this.TSB1MathResult20.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathSubType20, attNameExtension), this.TSB1MathSubType20.ToString());

                writer.WriteAttributeString(
                    string.Concat(cTSB1TMAmount20, attNameExtension), this.TSB1TMAmount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TMUnit20, attNameExtension), this.TSB1TMUnit20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLAmount20, attNameExtension), this.TSB1TLAmount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TLUnit20, attNameExtension), this.TSB1TLUnit20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUAmount20, attNameExtension), this.TSB1TUAmount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB1TUUnit20, attNameExtension), this.TSB1TUUnit20.ToString());
                writer.WriteAttributeString(
                  string.Concat(cTSB1MathOperator20, attNameExtension), this.TSB1MathOperator20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1MathExpression20, attNameExtension), this.TSB1MathExpression20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB1N20, attNameExtension), this.TSB1N20.ToString("N1", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB1Date20, attNameExtension), this.TSB1Date20.ToString("d", DateTimeFormatInfo.InvariantInfo));
                writer.WriteAttributeString(
                        string.Concat(cTSB1MathType20, attNameExtension), this.TSB1MathType20);
                writer.WriteAttributeString(
                    string.Concat(cTSB1BaseIO20, attNameExtension), this.TSB1BaseIO20);
                writer.WriteAttributeString(
                        string.Concat(cTSB11Amount20, attNameExtension), this.TSB11Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTSB11Unit20, attNameExtension), this.TSB11Unit20.ToString());
                writer.WriteAttributeString(
                        string.Concat(cTSB12Amount20, attNameExtension), this.TSB12Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB12Unit20, attNameExtension), this.TSB12Unit20);
                writer.WriteAttributeString(
                    string.Concat(cTSB15Amount20, attNameExtension), this.TSB15Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB15Unit20, attNameExtension), this.TSB15Unit20.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTSB13Amount20, attNameExtension), this.TSB13Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB13Unit20, attNameExtension), this.TSB13Unit20);
                writer.WriteAttributeString(
                    string.Concat(cTSB14Amount20, attNameExtension), this.TSB14Amount20.ToString("N4", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTSB14Unit20, attNameExtension), this.TSB14Unit20);
            }
        }
        public virtual async Task SetTSB1BaseStockAttributesAsync(string attNameExtension,
           XmlWriter writer)
        {
            await writer.WriteAttributeStringAsync(string.Empty,
               string.Concat(cTSB1Score, attNameExtension), string.Empty, this.TSB1Score.ToString("N4", CultureInfo.InvariantCulture));
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreUnit, attNameExtension), string.Empty, this.TSB1ScoreUnit);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreD1Amount, attNameExtension), string.Empty, this.TSB1ScoreD1Amount.ToString("N4", CultureInfo.InvariantCulture));
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreD1Unit, attNameExtension), string.Empty, this.TSB1ScoreD1Unit);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreD2Amount, attNameExtension), string.Empty, this.TSB1ScoreD2Amount.ToString("N4", CultureInfo.InvariantCulture));
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreD2Unit, attNameExtension), string.Empty, this.TSB1ScoreD2Unit);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreMathExpression, attNameExtension), string.Empty, this.TSB1ScoreMathExpression);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreM, attNameExtension), string.Empty, this.TSB1ScoreM.ToString("N4", CultureInfo.InvariantCulture));
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreMUnit, attNameExtension), string.Empty, this.TSB1ScoreMUnit);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreLAmount, attNameExtension), string.Empty, this.TSB1ScoreLAmount.ToString("N4", CultureInfo.InvariantCulture));
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreLUnit, attNameExtension), string.Empty, this.TSB1ScoreLUnit);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreUAmount, attNameExtension), string.Empty, this.TSB1ScoreUAmount.ToString("N4", CultureInfo.InvariantCulture));
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreUUnit, attNameExtension), string.Empty, this.TSB1ScoreUUnit);
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1Iterations, attNameExtension), string.Empty, this.TSB1Iterations.ToString());
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreDistType, attNameExtension), string.Empty, this.TSB1ScoreDistType.ToString());
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreMathType, attNameExtension), string.Empty, this.TSB1ScoreMathType.ToString());
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreMathResult, attNameExtension), string.Empty, this.TSB1ScoreMathResult.ToString());
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreMathSubType, attNameExtension), string.Empty, this.TSB1ScoreMathSubType.ToString());
            await writer.WriteAttributeStringAsync(string.Empty,
                string.Concat(cTSB1ScoreN, attNameExtension), string.Empty, this.TSB1ScoreN.ToString("N1", CultureInfo.InvariantCulture));
            if (this.TSB1Name1 != string.Empty && this.TSB1Name1 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name1, attNameExtension), string.Empty, this.TSB1Name1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description1, attNameExtension), string.Empty, this.TSB1Description1);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label1, attNameExtension), string.Empty, this.TSB1Label1);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type1, attNameExtension), string.Empty, this.TSB1Type1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel1, attNameExtension), string.Empty, this.TSB1RelLabel1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount1, attNameExtension), string.Empty, this.TSB1TAmount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit1, attNameExtension), string.Empty, this.TSB1TUnit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount1, attNameExtension), string.Empty, this.TSB1TD1Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit1, attNameExtension), string.Empty, this.TSB1TD1Unit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount1, attNameExtension), string.Empty, this.TSB1TD2Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit1, attNameExtension), string.Empty, this.TSB1TD2Unit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult1, attNameExtension), string.Empty, this.TSB1MathResult1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType1, attNameExtension), string.Empty, this.TSB1MathSubType1.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount1, attNameExtension), string.Empty, this.TSB1TMAmount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit1, attNameExtension), string.Empty, this.TSB1TMUnit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount1, attNameExtension), string.Empty, this.TSB1TLAmount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit1, attNameExtension), string.Empty, this.TSB1TLUnit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount1, attNameExtension), string.Empty, this.TSB1TUAmount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit1, attNameExtension), string.Empty, this.TSB1TUUnit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator1, attNameExtension), string.Empty, this.TSB1MathOperator1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression1, attNameExtension), string.Empty, this.TSB1MathExpression1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N1, attNameExtension), string.Empty, this.TSB1N1.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date1, attNameExtension), string.Empty, this.TSB1Date1.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType1, attNameExtension), string.Empty, this.TSB1MathType1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO1, attNameExtension), string.Empty, this.TSB1BaseIO1);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount1, attNameExtension), string.Empty, this.TSB11Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit1, attNameExtension), string.Empty, this.TSB11Unit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount1, attNameExtension), string.Empty, this.TSB12Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit1, attNameExtension), string.Empty, this.TSB12Unit1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount1, attNameExtension), string.Empty, this.TSB15Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit1, attNameExtension), string.Empty, this.TSB15Unit1.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Amount1, attNameExtension), string.Empty, this.TSB13Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit1, attNameExtension), string.Empty, this.TSB13Unit1);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount1, attNameExtension), string.Empty, this.TSB14Amount1.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit1, attNameExtension), string.Empty, this.TSB14Unit1);
            }
            if (this.TSB1Name2 != string.Empty && this.TSB1Name2 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name2, attNameExtension), string.Empty, this.TSB1Name2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description2, attNameExtension), string.Empty, this.TSB1Description2);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label2, attNameExtension), string.Empty, this.TSB1Label2);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type2, attNameExtension), string.Empty, this.TSB1Type2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel2, attNameExtension), string.Empty, this.TSB1RelLabel2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount2, attNameExtension), string.Empty, this.TSB1TAmount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit2, attNameExtension), string.Empty, this.TSB1TUnit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount2, attNameExtension), string.Empty, this.TSB1TD1Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit2, attNameExtension), string.Empty, this.TSB1TD1Unit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount2, attNameExtension), string.Empty, this.TSB1TD2Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit2, attNameExtension), string.Empty, this.TSB1TD2Unit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult2, attNameExtension), string.Empty, this.TSB1MathResult2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType2, attNameExtension), string.Empty, this.TSB1MathSubType2.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount2, attNameExtension), string.Empty, this.TSB1TMAmount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit2, attNameExtension), string.Empty, this.TSB1TMUnit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB1TLAmount2, attNameExtension), string.Empty, this.TSB1TLAmount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit2, attNameExtension), string.Empty, this.TSB1TLUnit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount2, attNameExtension), string.Empty, this.TSB1TUAmount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit2, attNameExtension), string.Empty, this.TSB1TUUnit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator2, attNameExtension), string.Empty, this.TSB1MathOperator2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression2, attNameExtension), string.Empty, this.TSB1MathExpression2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N2, attNameExtension), string.Empty, this.TSB1N2.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date2, attNameExtension), string.Empty, this.TSB1Date2.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType2, attNameExtension), string.Empty, this.TSB1MathType2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO2, attNameExtension), string.Empty, this.TSB1BaseIO2);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount2, attNameExtension), string.Empty, this.TSB11Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit2, attNameExtension), string.Empty, this.TSB11Unit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount2, attNameExtension), string.Empty, this.TSB12Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit2, attNameExtension), string.Empty, this.TSB12Unit2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount2, attNameExtension), string.Empty, this.TSB15Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit2, attNameExtension), string.Empty, this.TSB15Unit2.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount2, attNameExtension), string.Empty, this.TSB13Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit2, attNameExtension), string.Empty, this.TSB13Unit2);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount2, attNameExtension), string.Empty, this.TSB14Amount2.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit2, attNameExtension), string.Empty, this.TSB14Unit2);
            }
            if (this.TSB1Name3 != string.Empty && this.TSB1Name3 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name3, attNameExtension), string.Empty, this.TSB1Name3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description3, attNameExtension), string.Empty, this.TSB1Description3);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label3, attNameExtension), string.Empty, this.TSB1Label3);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type3, attNameExtension), string.Empty, this.TSB1Type3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel3, attNameExtension), string.Empty, this.TSB1RelLabel3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount3, attNameExtension), string.Empty, this.TSB1TAmount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit3, attNameExtension), string.Empty, this.TSB1TUnit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount3, attNameExtension), string.Empty, this.TSB1TD1Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit3, attNameExtension), string.Empty, this.TSB1TD1Unit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount3, attNameExtension), string.Empty, this.TSB1TD2Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit3, attNameExtension), string.Empty, this.TSB1TD2Unit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult3, attNameExtension), string.Empty, this.TSB1MathResult3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType3, attNameExtension), string.Empty, this.TSB1MathSubType3.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount3, attNameExtension), string.Empty, this.TSB1TMAmount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit3, attNameExtension), string.Empty, this.TSB1TMUnit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount3, attNameExtension), string.Empty, this.TSB1TLAmount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit3, attNameExtension), string.Empty, this.TSB1TLUnit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount3, attNameExtension), string.Empty, this.TSB1TUAmount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit3, attNameExtension), string.Empty, this.TSB1TUUnit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator3, attNameExtension), string.Empty, this.TSB1MathOperator3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression3, attNameExtension), string.Empty, this.TSB1MathExpression3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N3, attNameExtension), string.Empty, this.TSB1N3.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date3, attNameExtension), string.Empty, this.TSB1Date3.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType3, attNameExtension), string.Empty, this.TSB1MathType3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO3, attNameExtension), string.Empty, this.TSB1BaseIO3);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount3, attNameExtension), string.Empty, this.TSB11Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit3, attNameExtension), string.Empty, this.TSB11Unit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount3, attNameExtension), string.Empty, this.TSB12Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit3, attNameExtension), string.Empty, this.TSB12Unit3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount3, attNameExtension), string.Empty, this.TSB15Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit3, attNameExtension), string.Empty, this.TSB15Unit3.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Amount3, attNameExtension), string.Empty, this.TSB13Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit3, attNameExtension), string.Empty, this.TSB13Unit3);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount3, attNameExtension), string.Empty, this.TSB14Amount3.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit3, attNameExtension), string.Empty, this.TSB14Unit3);
            }
            if (this.TSB1Name4 != string.Empty && this.TSB1Name4 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name4, attNameExtension), string.Empty, this.TSB1Name4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description4, attNameExtension), string.Empty, this.TSB1Description4);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label4, attNameExtension), string.Empty, this.TSB1Label4);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type4, attNameExtension), string.Empty, this.TSB1Type4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel4, attNameExtension), string.Empty, this.TSB1RelLabel4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount4, attNameExtension), string.Empty, this.TSB1TAmount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit4, attNameExtension), string.Empty, this.TSB1TUnit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount4, attNameExtension), string.Empty, this.TSB1TD1Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit4, attNameExtension), string.Empty, this.TSB1TD1Unit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount4, attNameExtension), string.Empty, this.TSB1TD2Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit4, attNameExtension), string.Empty, this.TSB1TD2Unit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult4, attNameExtension), string.Empty, this.TSB1MathResult4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType4, attNameExtension), string.Empty, this.TSB1MathSubType4.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount4, attNameExtension), string.Empty, this.TSB1TMAmount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit4, attNameExtension), string.Empty, this.TSB1TMUnit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount4, attNameExtension), string.Empty, this.TSB1TLAmount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit4, attNameExtension), string.Empty, this.TSB1TLUnit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount4, attNameExtension), string.Empty, this.TSB1TUAmount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit4, attNameExtension), string.Empty, this.TSB1TUUnit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator4, attNameExtension), string.Empty, this.TSB1MathOperator4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression4, attNameExtension), string.Empty, this.TSB1MathExpression4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N4, attNameExtension), string.Empty, this.TSB1N4.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date4, attNameExtension), string.Empty, this.TSB1Date4.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType4, attNameExtension), string.Empty, this.TSB1MathType4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO4, attNameExtension), string.Empty, this.TSB1BaseIO4);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount4, attNameExtension), string.Empty, this.TSB11Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit4, attNameExtension), string.Empty, this.TSB11Unit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount4, attNameExtension), string.Empty, this.TSB12Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit4, attNameExtension), string.Empty, this.TSB12Unit4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount4, attNameExtension), string.Empty, this.TSB15Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit4, attNameExtension), string.Empty, this.TSB15Unit4.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount4, attNameExtension), string.Empty, this.TSB13Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit4, attNameExtension), string.Empty, this.TSB13Unit4);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount4, attNameExtension), string.Empty, this.TSB14Amount4.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit4, attNameExtension), string.Empty, this.TSB14Unit4);
            }
            if (this.TSB1Name5 != string.Empty && this.TSB1Name5 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name5, attNameExtension), string.Empty, this.TSB1Name5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description5, attNameExtension), string.Empty, this.TSB1Description5);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label5, attNameExtension), string.Empty, this.TSB1Label5);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type5, attNameExtension), string.Empty, this.TSB1Type5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel5, attNameExtension), string.Empty, this.TSB1RelLabel5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount5, attNameExtension), string.Empty, this.TSB1TAmount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit5, attNameExtension), string.Empty, this.TSB1TUnit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount5, attNameExtension), string.Empty, this.TSB1TD1Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit5, attNameExtension), string.Empty, this.TSB1TD1Unit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount5, attNameExtension), string.Empty, this.TSB1TD2Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit5, attNameExtension), string.Empty, this.TSB1TD2Unit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult5, attNameExtension), string.Empty, this.TSB1MathResult5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType5, attNameExtension), string.Empty, this.TSB1MathSubType5.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount5, attNameExtension), string.Empty, this.TSB1TMAmount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit5, attNameExtension), string.Empty, this.TSB1TMUnit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB1TLAmount5, attNameExtension), string.Empty, this.TSB1TLAmount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit5, attNameExtension), string.Empty, this.TSB1TLUnit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount5, attNameExtension), string.Empty, this.TSB1TUAmount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit5, attNameExtension), string.Empty, this.TSB1TUUnit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator5, attNameExtension), string.Empty, this.TSB1MathOperator5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression5, attNameExtension), string.Empty, this.TSB1MathExpression5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N5, attNameExtension), string.Empty, this.TSB1N5.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date5, attNameExtension), string.Empty, this.TSB1Date5.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType5, attNameExtension), string.Empty, this.TSB1MathType5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO5, attNameExtension), string.Empty, this.TSB1BaseIO5);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount5, attNameExtension), string.Empty, this.TSB11Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit5, attNameExtension), string.Empty, this.TSB11Unit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount5, attNameExtension), string.Empty, this.TSB12Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit5, attNameExtension), string.Empty, this.TSB12Unit5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount5, attNameExtension), string.Empty, this.TSB15Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit5, attNameExtension), string.Empty, this.TSB15Unit5.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount5, attNameExtension), string.Empty, this.TSB13Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit5, attNameExtension), string.Empty, this.TSB13Unit5);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount5, attNameExtension), string.Empty, this.TSB14Amount5.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit5, attNameExtension), string.Empty, this.TSB14Unit5);
            }
            if (this.TSB1Name6 != string.Empty && this.TSB1Name6 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name6, attNameExtension), string.Empty, this.TSB1Name6);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description6, attNameExtension), string.Empty, this.TSB1Description6);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label6, attNameExtension), string.Empty, this.TSB1Label6);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type6, attNameExtension), string.Empty, this.TSB1Type6);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel6, attNameExtension), string.Empty, this.TSB1RelLabel6);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount6, attNameExtension), string.Empty, this.TSB1TAmount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit6, attNameExtension), string.Empty, this.TSB1TUnit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount6, attNameExtension), string.Empty, this.TSB1TD1Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit6, attNameExtension), string.Empty, this.TSB1TD1Unit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount6, attNameExtension), string.Empty, this.TSB1TD2Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit6, attNameExtension), string.Empty, this.TSB1TD2Unit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult6, attNameExtension), string.Empty, this.TSB1MathResult6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType6, attNameExtension), string.Empty, this.TSB1MathSubType6.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount6, attNameExtension), string.Empty, this.TSB1TMAmount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit6, attNameExtension), string.Empty, this.TSB1TMUnit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount6, attNameExtension), string.Empty, this.TSB1TLAmount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit6, attNameExtension), string.Empty, this.TSB1TLUnit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount6, attNameExtension), string.Empty, this.TSB1TUAmount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit6, attNameExtension), string.Empty, this.TSB1TUUnit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator6, attNameExtension), string.Empty, this.TSB1MathOperator6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression6, attNameExtension), string.Empty, this.TSB1MathExpression6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N6, attNameExtension), string.Empty, this.TSB1N6.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date6, attNameExtension), string.Empty, this.TSB1Date6.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType6, attNameExtension), string.Empty, this.TSB1MathType6);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO6, attNameExtension), string.Empty, this.TSB1BaseIO6);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount6, attNameExtension), string.Empty, this.TSB11Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit6, attNameExtension), string.Empty, this.TSB11Unit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount6, attNameExtension), string.Empty, this.TSB12Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit6, attNameExtension), string.Empty, this.TSB12Unit6);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount6, attNameExtension), string.Empty, this.TSB15Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit6, attNameExtension), string.Empty, this.TSB15Unit6.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount6, attNameExtension), string.Empty, this.TSB13Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit6, attNameExtension), string.Empty, this.TSB13Unit6);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount6, attNameExtension), string.Empty, this.TSB14Amount6.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit6, attNameExtension), string.Empty, this.TSB14Unit6);
            }
            if (this.TSB1Name7 != string.Empty && this.TSB1Name7 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name7, attNameExtension), string.Empty, this.TSB1Name7);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description7, attNameExtension), string.Empty, this.TSB1Description7);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label7, attNameExtension), string.Empty, this.TSB1Label7);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type7, attNameExtension), string.Empty, this.TSB1Type7);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel7, attNameExtension), string.Empty, this.TSB1RelLabel7);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount7, attNameExtension), string.Empty, this.TSB1TAmount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit7, attNameExtension), string.Empty, this.TSB1TUnit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount7, attNameExtension), string.Empty, this.TSB1TD1Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit7, attNameExtension), string.Empty, this.TSB1TD1Unit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount7, attNameExtension), string.Empty, this.TSB1TD2Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit7, attNameExtension), string.Empty, this.TSB1TD2Unit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult7, attNameExtension), string.Empty, this.TSB1MathResult7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType7, attNameExtension), string.Empty, this.TSB1MathSubType7.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount7, attNameExtension), string.Empty, this.TSB1TMAmount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit7, attNameExtension), string.Empty, this.TSB1TMUnit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount7, attNameExtension), string.Empty, this.TSB1TLAmount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit7, attNameExtension), string.Empty, this.TSB1TLUnit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount7, attNameExtension), string.Empty, this.TSB1TUAmount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit7, attNameExtension), string.Empty, this.TSB1TUUnit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator7, attNameExtension), string.Empty, this.TSB1MathOperator7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression7, attNameExtension), string.Empty, this.TSB1MathExpression7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N7, attNameExtension), string.Empty, this.TSB1N7.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date7, attNameExtension), string.Empty, this.TSB1Date7.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType7, attNameExtension), string.Empty, this.TSB1MathType7);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO7, attNameExtension), string.Empty, this.TSB1BaseIO7);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount7, attNameExtension), string.Empty, this.TSB11Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit7, attNameExtension), string.Empty, this.TSB11Unit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount7, attNameExtension), string.Empty, this.TSB12Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit7, attNameExtension), string.Empty, this.TSB12Unit7);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount7, attNameExtension), string.Empty, this.TSB15Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit7, attNameExtension), string.Empty, this.TSB15Unit7.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount7, attNameExtension), string.Empty, this.TSB13Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit7, attNameExtension), string.Empty, this.TSB13Unit7);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount7, attNameExtension), string.Empty, this.TSB14Amount7.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit7, attNameExtension), string.Empty, this.TSB14Unit7);
            }
            if (this.TSB1Name8 != string.Empty && this.TSB1Name8 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name8, attNameExtension), string.Empty, this.TSB1Name8);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description8, attNameExtension), string.Empty, this.TSB1Description8);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label8, attNameExtension), string.Empty, this.TSB1Label8);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type8, attNameExtension), string.Empty, this.TSB1Type8);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel8, attNameExtension), string.Empty, this.TSB1RelLabel8);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount8, attNameExtension), string.Empty, this.TSB1TAmount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit8, attNameExtension), string.Empty, this.TSB1TUnit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount8, attNameExtension), string.Empty, this.TSB1TD1Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit8, attNameExtension), string.Empty, this.TSB1TD1Unit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount8, attNameExtension), string.Empty, this.TSB1TD2Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit8, attNameExtension), string.Empty, this.TSB1TD2Unit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult8, attNameExtension), string.Empty, this.TSB1MathResult8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType8, attNameExtension), string.Empty, this.TSB1MathSubType8.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount8, attNameExtension), string.Empty, this.TSB1TMAmount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit8, attNameExtension), string.Empty, this.TSB1TMUnit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount8, attNameExtension), string.Empty, this.TSB1TLAmount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit8, attNameExtension), string.Empty, this.TSB1TLUnit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount8, attNameExtension), string.Empty, this.TSB1TUAmount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit8, attNameExtension), string.Empty, this.TSB1TUUnit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator8, attNameExtension), string.Empty, this.TSB1MathOperator8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression8, attNameExtension), string.Empty, this.TSB1MathExpression8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N8, attNameExtension), string.Empty, this.TSB1N8.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date8, attNameExtension), string.Empty, this.TSB1Date8.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType8, attNameExtension), string.Empty, this.TSB1MathType8);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO8, attNameExtension), string.Empty, this.TSB1BaseIO8);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount8, attNameExtension), string.Empty, this.TSB11Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit8, attNameExtension), string.Empty, this.TSB11Unit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount8, attNameExtension), string.Empty, this.TSB12Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit8, attNameExtension), string.Empty, this.TSB12Unit8);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount8, attNameExtension), string.Empty, this.TSB15Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit8, attNameExtension), string.Empty, this.TSB15Unit8.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount8, attNameExtension), string.Empty, this.TSB13Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit8, attNameExtension), string.Empty, this.TSB13Unit8);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount8, attNameExtension), string.Empty, this.TSB14Amount8.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit8, attNameExtension), string.Empty, this.TSB14Unit8);
            }
            if (this.TSB1Name9 != string.Empty && this.TSB1Name9 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name9, attNameExtension), string.Empty, this.TSB1Name9);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description9, attNameExtension), string.Empty, this.TSB1Description9);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label9, attNameExtension), string.Empty, this.TSB1Label9);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type9, attNameExtension), string.Empty, this.TSB1Type9);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel9, attNameExtension), string.Empty, this.TSB1RelLabel9);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount9, attNameExtension), string.Empty, this.TSB1TAmount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit9, attNameExtension), string.Empty, this.TSB1TUnit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount9, attNameExtension), string.Empty, this.TSB1TD1Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit9, attNameExtension), string.Empty, this.TSB1TD1Unit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount9, attNameExtension), string.Empty, this.TSB1TD2Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit9, attNameExtension), string.Empty, this.TSB1TD2Unit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult9, attNameExtension), string.Empty, this.TSB1MathResult9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType9, attNameExtension), string.Empty, this.TSB1MathSubType9.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount9, attNameExtension), string.Empty, this.TSB1TMAmount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit9, attNameExtension), string.Empty, this.TSB1TMUnit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount9, attNameExtension), string.Empty, this.TSB1TLAmount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit9, attNameExtension), string.Empty, this.TSB1TLUnit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount9, attNameExtension), string.Empty, this.TSB1TUAmount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit9, attNameExtension), string.Empty, this.TSB1TUUnit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator9, attNameExtension), string.Empty, this.TSB1MathOperator9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression9, attNameExtension), string.Empty, this.TSB1MathExpression9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N9, attNameExtension), string.Empty, this.TSB1N9.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date9, attNameExtension), string.Empty, this.TSB1Date9.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType9, attNameExtension), string.Empty, this.TSB1MathType9);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO9, attNameExtension), string.Empty, this.TSB1BaseIO9);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount9, attNameExtension), string.Empty, this.TSB11Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit9, attNameExtension), string.Empty, this.TSB11Unit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount9, attNameExtension), string.Empty, this.TSB12Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit9, attNameExtension), string.Empty, this.TSB12Unit9);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount9, attNameExtension), string.Empty, this.TSB15Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit9, attNameExtension), string.Empty, this.TSB15Unit9.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount9, attNameExtension), string.Empty, this.TSB13Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit9, attNameExtension), string.Empty, this.TSB13Unit9);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount9, attNameExtension), string.Empty, this.TSB14Amount9.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit9, attNameExtension), string.Empty, this.TSB14Unit9);
            }
            if (this.TSB1Name10 != string.Empty && this.TSB1Name10 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name10, attNameExtension), string.Empty, this.TSB1Name10);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description10, attNameExtension), string.Empty, this.TSB1Description10);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label10, attNameExtension), string.Empty, this.TSB1Label10);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type10, attNameExtension), string.Empty, this.TSB1Type10);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel10, attNameExtension), string.Empty, this.TSB1RelLabel10);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount10, attNameExtension), string.Empty, this.TSB1TAmount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit10, attNameExtension), string.Empty, this.TSB1TUnit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount10, attNameExtension), string.Empty, this.TSB1TD1Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit10, attNameExtension), string.Empty, this.TSB1TD1Unit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount10, attNameExtension), string.Empty, this.TSB1TD2Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit10, attNameExtension), string.Empty, this.TSB1TD2Unit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult10, attNameExtension), string.Empty, this.TSB1MathResult10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType10, attNameExtension), string.Empty, this.TSB1MathSubType10.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount10, attNameExtension), string.Empty, this.TSB1TMAmount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit10, attNameExtension), string.Empty, this.TSB1TMUnit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount10, attNameExtension), string.Empty, this.TSB1TLAmount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit10, attNameExtension), string.Empty, this.TSB1TLUnit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount10, attNameExtension), string.Empty, this.TSB1TUAmount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit10, attNameExtension), string.Empty, this.TSB1TUUnit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator10, attNameExtension), string.Empty, this.TSB1MathOperator10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression10, attNameExtension), string.Empty, this.TSB1MathExpression10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N10, attNameExtension), string.Empty, this.TSB1N10.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date10, attNameExtension), string.Empty, this.TSB1Date10.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType10, attNameExtension), string.Empty, this.TSB1MathType10);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO10, attNameExtension), string.Empty, this.TSB1BaseIO10);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount10, attNameExtension), string.Empty, this.TSB11Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit10, attNameExtension), string.Empty, this.TSB11Unit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount10, attNameExtension), string.Empty, this.TSB12Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit10, attNameExtension), string.Empty, this.TSB12Unit10);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount10, attNameExtension), string.Empty, this.TSB15Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit10, attNameExtension), string.Empty, this.TSB15Unit10.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount10, attNameExtension), string.Empty, this.TSB13Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit10, attNameExtension), string.Empty, this.TSB13Unit10);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount10, attNameExtension), string.Empty, this.TSB14Amount10.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit10, attNameExtension), string.Empty, this.TSB14Unit10);
            }
            if (this.TSB1Name11 != string.Empty && this.TSB1Name11 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name11, attNameExtension), string.Empty, this.TSB1Name11);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description11, attNameExtension), string.Empty, this.TSB1Description11);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label11, attNameExtension), string.Empty, this.TSB1Label11);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type11, attNameExtension), string.Empty, this.TSB1Type11);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel11, attNameExtension), string.Empty, this.TSB1RelLabel11);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount11, attNameExtension), string.Empty, this.TSB1TAmount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit11, attNameExtension), string.Empty, this.TSB1TUnit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount11, attNameExtension), string.Empty, this.TSB1TD1Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit11, attNameExtension), string.Empty, this.TSB1TD1Unit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount11, attNameExtension), string.Empty, this.TSB1TD2Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit11, attNameExtension), string.Empty, this.TSB1TD2Unit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult11, attNameExtension), string.Empty, this.TSB1MathResult11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType11, attNameExtension), string.Empty, this.TSB1MathSubType11.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount11, attNameExtension), string.Empty, this.TSB1TMAmount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit11, attNameExtension), string.Empty, this.TSB1TMUnit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount11, attNameExtension), string.Empty, this.TSB1TLAmount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit11, attNameExtension), string.Empty, this.TSB1TLUnit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount11, attNameExtension), string.Empty, this.TSB1TUAmount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit11, attNameExtension), string.Empty, this.TSB1TUUnit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator11, attNameExtension), string.Empty, this.TSB1MathOperator11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression11, attNameExtension), string.Empty, this.TSB1MathExpression11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N11, attNameExtension), string.Empty, this.TSB1N11.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date11, attNameExtension), string.Empty, this.TSB1Date11.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType11, attNameExtension), string.Empty, this.TSB1MathType11);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO11, attNameExtension), string.Empty, this.TSB1BaseIO11);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount11, attNameExtension), string.Empty, this.TSB11Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit11, attNameExtension), string.Empty, this.TSB11Unit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount11, attNameExtension), string.Empty, this.TSB12Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit11, attNameExtension), string.Empty, this.TSB12Unit11);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount11, attNameExtension), string.Empty, this.TSB15Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit11, attNameExtension), string.Empty, this.TSB15Unit11.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount11, attNameExtension), string.Empty, this.TSB13Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit11, attNameExtension), string.Empty, this.TSB13Unit11);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount11, attNameExtension), string.Empty, this.TSB14Amount11.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit11, attNameExtension), string.Empty, this.TSB14Unit11);
            }
            if (this.TSB1Name12 != string.Empty && this.TSB1Name12 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name12, attNameExtension), string.Empty, this.TSB1Name12);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description12, attNameExtension), string.Empty, this.TSB1Description12);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label12, attNameExtension), string.Empty, this.TSB1Label12);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type12, attNameExtension), string.Empty, this.TSB1Type12);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel12, attNameExtension), string.Empty, this.TSB1RelLabel12);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount12, attNameExtension), string.Empty, this.TSB1TAmount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit12, attNameExtension), string.Empty, this.TSB1TUnit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount12, attNameExtension), string.Empty, this.TSB1TD1Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit12, attNameExtension), string.Empty, this.TSB1TD1Unit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount12, attNameExtension), string.Empty, this.TSB1TD2Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit12, attNameExtension), string.Empty, this.TSB1TD2Unit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult12, attNameExtension), string.Empty, this.TSB1MathResult12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType12, attNameExtension), string.Empty, this.TSB1MathSubType12.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount12, attNameExtension), string.Empty, this.TSB1TMAmount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit12, attNameExtension), string.Empty, this.TSB1TMUnit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount12, attNameExtension), string.Empty, this.TSB1TLAmount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit12, attNameExtension), string.Empty, this.TSB1TLUnit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount12, attNameExtension), string.Empty, this.TSB1TUAmount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit12, attNameExtension), string.Empty, this.TSB1TUUnit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator12, attNameExtension), string.Empty, this.TSB1MathOperator12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression12, attNameExtension), string.Empty, this.TSB1MathExpression12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N12, attNameExtension), string.Empty, this.TSB1N12.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date12, attNameExtension), string.Empty, this.TSB1Date12.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType12, attNameExtension), string.Empty, this.TSB1MathType12);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO12, attNameExtension), string.Empty, this.TSB1BaseIO12);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount12, attNameExtension), string.Empty, this.TSB11Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit12, attNameExtension), string.Empty, this.TSB11Unit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount12, attNameExtension), string.Empty, this.TSB12Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit12, attNameExtension), string.Empty, this.TSB12Unit12);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount12, attNameExtension), string.Empty, this.TSB15Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit12, attNameExtension), string.Empty, this.TSB15Unit12.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount12, attNameExtension), string.Empty, this.TSB13Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit12, attNameExtension), string.Empty, this.TSB13Unit12);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount12, attNameExtension), string.Empty, this.TSB14Amount12.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit12, attNameExtension), string.Empty, this.TSB14Unit12);
            }
            if (this.TSB1Name13 != string.Empty && this.TSB1Name13 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name13, attNameExtension), string.Empty, this.TSB1Name13);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description13, attNameExtension), string.Empty, this.TSB1Description13);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label13, attNameExtension), string.Empty, this.TSB1Label13);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type13, attNameExtension), string.Empty, this.TSB1Type13);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel13, attNameExtension), string.Empty, this.TSB1RelLabel13);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount13, attNameExtension), string.Empty, this.TSB1TAmount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit13, attNameExtension), string.Empty, this.TSB1TUnit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount13, attNameExtension), string.Empty, this.TSB1TD1Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit13, attNameExtension), string.Empty, this.TSB1TD1Unit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount13, attNameExtension), string.Empty, this.TSB1TD2Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit13, attNameExtension), string.Empty, this.TSB1TD2Unit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult13, attNameExtension), string.Empty, this.TSB1MathResult13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType13, attNameExtension), string.Empty, this.TSB1MathSubType13.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount13, attNameExtension), string.Empty, this.TSB1TMAmount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit13, attNameExtension), string.Empty, this.TSB1TMUnit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount13, attNameExtension), string.Empty, this.TSB1TLAmount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit13, attNameExtension), string.Empty, this.TSB1TLUnit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount13, attNameExtension), string.Empty, this.TSB1TUAmount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit13, attNameExtension), string.Empty, this.TSB1TUUnit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator13, attNameExtension), string.Empty, this.TSB1MathOperator13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression13, attNameExtension), string.Empty, this.TSB1MathExpression13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N13, attNameExtension), string.Empty, this.TSB1N13.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date13, attNameExtension), string.Empty, this.TSB1Date13.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType13, attNameExtension), string.Empty, this.TSB1MathType13);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO13, attNameExtension), string.Empty, this.TSB1BaseIO13);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount13, attNameExtension), string.Empty, this.TSB11Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit13, attNameExtension), string.Empty, this.TSB11Unit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount13, attNameExtension), string.Empty, this.TSB12Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit13, attNameExtension), string.Empty, this.TSB12Unit13);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount13, attNameExtension), string.Empty, this.TSB15Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit13, attNameExtension), string.Empty, this.TSB15Unit13.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount13, attNameExtension), string.Empty, this.TSB13Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit13, attNameExtension), string.Empty, this.TSB13Unit13);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount13, attNameExtension), string.Empty, this.TSB14Amount13.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit13, attNameExtension), string.Empty, this.TSB14Unit13);
            }
            if (this.TSB1Name14 != string.Empty && this.TSB1Name14 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name14, attNameExtension), string.Empty, this.TSB1Name14);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description14, attNameExtension), string.Empty, this.TSB1Description14);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label14, attNameExtension), string.Empty, this.TSB1Label14);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type14, attNameExtension), string.Empty, this.TSB1Type14);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel14, attNameExtension), string.Empty, this.TSB1RelLabel14);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount14, attNameExtension), string.Empty, this.TSB1TAmount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit14, attNameExtension), string.Empty, this.TSB1TUnit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount14, attNameExtension), string.Empty, this.TSB1TD1Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit14, attNameExtension), string.Empty, this.TSB1TD1Unit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount14, attNameExtension), string.Empty, this.TSB1TD2Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit14, attNameExtension), string.Empty, this.TSB1TD2Unit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult14, attNameExtension), string.Empty, this.TSB1MathResult14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType14, attNameExtension), string.Empty, this.TSB1MathSubType14.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount14, attNameExtension), string.Empty, this.TSB1TMAmount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit14, attNameExtension), string.Empty, this.TSB1TMUnit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount14, attNameExtension), string.Empty, this.TSB1TLAmount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit14, attNameExtension), string.Empty, this.TSB1TLUnit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount14, attNameExtension), string.Empty, this.TSB1TUAmount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit14, attNameExtension), string.Empty, this.TSB1TUUnit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator14, attNameExtension), string.Empty, this.TSB1MathOperator14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression14, attNameExtension), string.Empty, this.TSB1MathExpression14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N14, attNameExtension), string.Empty, this.TSB1N14.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date14, attNameExtension), string.Empty, this.TSB1Date14.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType14, attNameExtension), string.Empty, this.TSB1MathType14);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO14, attNameExtension), string.Empty, this.TSB1BaseIO14);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount14, attNameExtension), string.Empty, this.TSB11Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit14, attNameExtension), string.Empty, this.TSB11Unit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount14, attNameExtension), string.Empty, this.TSB12Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit14, attNameExtension), string.Empty, this.TSB12Unit14);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount14, attNameExtension), string.Empty, this.TSB15Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit14, attNameExtension), string.Empty, this.TSB15Unit14.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount14, attNameExtension), string.Empty, this.TSB13Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit14, attNameExtension), string.Empty, this.TSB13Unit14);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount14, attNameExtension), string.Empty, this.TSB14Amount14.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit14, attNameExtension), string.Empty, this.TSB14Unit14);
            }
            if (this.TSB1Name15 != string.Empty && this.TSB1Name15 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name15, attNameExtension), string.Empty, this.TSB1Name15);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description15, attNameExtension), string.Empty, this.TSB1Description15);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label15, attNameExtension), string.Empty, this.TSB1Label15);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type15, attNameExtension), string.Empty, this.TSB1Type15);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel15, attNameExtension), string.Empty, this.TSB1RelLabel15);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount15, attNameExtension), string.Empty, this.TSB1TAmount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit15, attNameExtension), string.Empty, this.TSB1TUnit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount15, attNameExtension), string.Empty, this.TSB1TD1Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit15, attNameExtension), string.Empty, this.TSB1TD1Unit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount15, attNameExtension), string.Empty, this.TSB1TD2Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit15, attNameExtension), string.Empty, this.TSB1TD2Unit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult15, attNameExtension), string.Empty, this.TSB1MathResult15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType15, attNameExtension), string.Empty, this.TSB1MathSubType15.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount15, attNameExtension), string.Empty, this.TSB1TMAmount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit15, attNameExtension), string.Empty, this.TSB1TMUnit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount15, attNameExtension), string.Empty, this.TSB1TLAmount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit15, attNameExtension), string.Empty, this.TSB1TLUnit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount15, attNameExtension), string.Empty, this.TSB1TUAmount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit15, attNameExtension), string.Empty, this.TSB1TUUnit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator15, attNameExtension), string.Empty, this.TSB1MathOperator15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression15, attNameExtension), string.Empty, this.TSB1MathExpression15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N15, attNameExtension), string.Empty, this.TSB1N15.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date15, attNameExtension), string.Empty, this.TSB1Date15.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType15, attNameExtension), string.Empty, this.TSB1MathType15);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO15, attNameExtension), string.Empty, this.TSB1BaseIO15);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount15, attNameExtension), string.Empty, this.TSB11Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit15, attNameExtension), string.Empty, this.TSB11Unit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount15, attNameExtension), string.Empty, this.TSB12Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit15, attNameExtension), string.Empty, this.TSB12Unit15);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount15, attNameExtension), string.Empty, this.TSB15Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit15, attNameExtension), string.Empty, this.TSB15Unit15.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount15, attNameExtension), string.Empty, this.TSB13Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit15, attNameExtension), string.Empty, this.TSB13Unit15);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount15, attNameExtension), string.Empty, this.TSB14Amount15.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit15, attNameExtension), string.Empty, this.TSB14Unit15);
            }
            if (this.TSB1Name16 != string.Empty && this.TSB1Name16 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name16, attNameExtension), string.Empty, this.TSB1Name16);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description16, attNameExtension), string.Empty, this.TSB1Description16);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label16, attNameExtension), string.Empty, this.TSB1Label16);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type16, attNameExtension), string.Empty, this.TSB1Type16);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel16, attNameExtension), string.Empty, this.TSB1RelLabel16);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount16, attNameExtension), string.Empty, this.TSB1TAmount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit16, attNameExtension), string.Empty, this.TSB1TUnit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount16, attNameExtension), string.Empty, this.TSB1TD1Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit16, attNameExtension), string.Empty, this.TSB1TD1Unit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount16, attNameExtension), string.Empty, this.TSB1TD2Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit16, attNameExtension), string.Empty, this.TSB1TD2Unit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult16, attNameExtension), string.Empty, this.TSB1MathResult16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType16, attNameExtension), string.Empty, this.TSB1MathSubType16.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount16, attNameExtension), string.Empty, this.TSB1TMAmount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit16, attNameExtension), string.Empty, this.TSB1TMUnit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount16, attNameExtension), string.Empty, this.TSB1TLAmount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit16, attNameExtension), string.Empty, this.TSB1TLUnit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount16, attNameExtension), string.Empty, this.TSB1TUAmount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit16, attNameExtension), string.Empty, this.TSB1TUUnit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator16, attNameExtension), string.Empty, this.TSB1MathOperator16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression16, attNameExtension), string.Empty, this.TSB1MathExpression16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N16, attNameExtension), string.Empty, this.TSB1N16.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date16, attNameExtension), string.Empty, this.TSB1Date16.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType16, attNameExtension), string.Empty, this.TSB1MathType16);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO16, attNameExtension), string.Empty, this.TSB1BaseIO16);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount16, attNameExtension), string.Empty, this.TSB11Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit16, attNameExtension), string.Empty, this.TSB11Unit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount16, attNameExtension), string.Empty, this.TSB12Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit16, attNameExtension), string.Empty, this.TSB12Unit16);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount16, attNameExtension), string.Empty, this.TSB15Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit16, attNameExtension), string.Empty, this.TSB15Unit16.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount16, attNameExtension), string.Empty, this.TSB13Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit16, attNameExtension), string.Empty, this.TSB13Unit16);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount16, attNameExtension), string.Empty, this.TSB14Amount16.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit16, attNameExtension), string.Empty, this.TSB14Unit16);
            }
            if (this.TSB1Name17 != string.Empty && this.TSB1Name17 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name17, attNameExtension), string.Empty, this.TSB1Name17);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description17, attNameExtension), string.Empty, this.TSB1Description17);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label17, attNameExtension), string.Empty, this.TSB1Label17);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type17, attNameExtension), string.Empty, this.TSB1Type17);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel17, attNameExtension), string.Empty, this.TSB1RelLabel17);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount17, attNameExtension), string.Empty, this.TSB1TAmount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit17, attNameExtension), string.Empty, this.TSB1TUnit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount17, attNameExtension), string.Empty, this.TSB1TD1Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit17, attNameExtension), string.Empty, this.TSB1TD1Unit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount17, attNameExtension), string.Empty, this.TSB1TD2Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit17, attNameExtension), string.Empty, this.TSB1TD2Unit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult17, attNameExtension), string.Empty, this.TSB1MathResult17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType17, attNameExtension), string.Empty, this.TSB1MathSubType17.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount17, attNameExtension), string.Empty, this.TSB1TMAmount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit17, attNameExtension), string.Empty, this.TSB1TMUnit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB1TLAmount17, attNameExtension), string.Empty, this.TSB1TLAmount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit17, attNameExtension), string.Empty, this.TSB1TLUnit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount17, attNameExtension), string.Empty, this.TSB1TUAmount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit17, attNameExtension), string.Empty, this.TSB1TUUnit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator17, attNameExtension), string.Empty, this.TSB1MathOperator17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression17, attNameExtension), string.Empty, this.TSB1MathExpression17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N17, attNameExtension), string.Empty, this.TSB1N17.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date17, attNameExtension), string.Empty, this.TSB1Date17.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType17, attNameExtension), string.Empty, this.TSB1MathType17);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO17, attNameExtension), string.Empty, this.TSB1BaseIO17);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount17, attNameExtension), string.Empty, this.TSB11Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit17, attNameExtension), string.Empty, this.TSB11Unit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount17, attNameExtension), string.Empty, this.TSB12Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit17, attNameExtension), string.Empty, this.TSB12Unit17);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount17, attNameExtension), string.Empty, this.TSB15Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit17, attNameExtension), string.Empty, this.TSB15Unit17.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Amount17, attNameExtension), string.Empty, this.TSB13Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit17, attNameExtension), string.Empty, this.TSB13Unit17);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount17, attNameExtension), string.Empty, this.TSB14Amount17.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit17, attNameExtension), string.Empty, this.TSB14Unit17);
            }
            if (this.TSB1Name18 != string.Empty && this.TSB1Name18 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name18, attNameExtension), string.Empty, this.TSB1Name18);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description18, attNameExtension), string.Empty, this.TSB1Description18);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label18, attNameExtension), string.Empty, this.TSB1Label18);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type18, attNameExtension), string.Empty, this.TSB1Type18);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel18, attNameExtension), string.Empty, this.TSB1RelLabel18);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount18, attNameExtension), string.Empty, this.TSB1TAmount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit18, attNameExtension), string.Empty, this.TSB1TUnit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount18, attNameExtension), string.Empty, this.TSB1TD1Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit18, attNameExtension), string.Empty, this.TSB1TD1Unit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount18, attNameExtension), string.Empty, this.TSB1TD2Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit18, attNameExtension), string.Empty, this.TSB1TD2Unit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult18, attNameExtension), string.Empty, this.TSB1MathResult18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType18, attNameExtension), string.Empty, this.TSB1MathSubType18.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount18, attNameExtension), string.Empty, this.TSB1TMAmount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit18, attNameExtension), string.Empty, this.TSB1TMUnit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount18, attNameExtension), string.Empty, this.TSB1TLAmount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit18, attNameExtension), string.Empty, this.TSB1TLUnit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount18, attNameExtension), string.Empty, this.TSB1TUAmount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit18, attNameExtension), string.Empty, this.TSB1TUUnit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator18, attNameExtension), string.Empty, this.TSB1MathOperator18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression18, attNameExtension), string.Empty, this.TSB1MathExpression18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N18, attNameExtension), string.Empty, this.TSB1N18.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date18, attNameExtension), string.Empty, this.TSB1Date18.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType18, attNameExtension), string.Empty, this.TSB1MathType18);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO18, attNameExtension), string.Empty, this.TSB1BaseIO18);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount18, attNameExtension), string.Empty, this.TSB11Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit18, attNameExtension), string.Empty, this.TSB11Unit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount18, attNameExtension), string.Empty, this.TSB12Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit18, attNameExtension), string.Empty, this.TSB12Unit18);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount18, attNameExtension), string.Empty, this.TSB15Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit18, attNameExtension), string.Empty, this.TSB15Unit18.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount18, attNameExtension), string.Empty, this.TSB13Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit18, attNameExtension), string.Empty, this.TSB13Unit18);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount18, attNameExtension), string.Empty, this.TSB14Amount18.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit18, attNameExtension), string.Empty, this.TSB14Unit18);
            }
            if (this.TSB1Name19 != string.Empty && this.TSB1Name19 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name19, attNameExtension), string.Empty, this.TSB1Name19);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description19, attNameExtension), string.Empty, this.TSB1Description19);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label19, attNameExtension), string.Empty, this.TSB1Label19);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type19, attNameExtension), string.Empty, this.TSB1Type19);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel19, attNameExtension), string.Empty, this.TSB1RelLabel19);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount19, attNameExtension), string.Empty, this.TSB1TAmount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit19, attNameExtension), string.Empty, this.TSB1TUnit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount19, attNameExtension), string.Empty, this.TSB1TD1Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit19, attNameExtension), string.Empty, this.TSB1TD1Unit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount19, attNameExtension), string.Empty, this.TSB1TD2Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit19, attNameExtension), string.Empty, this.TSB1TD2Unit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult19, attNameExtension), string.Empty, this.TSB1MathResult19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType19, attNameExtension), string.Empty, this.TSB1MathSubType19.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount19, attNameExtension), string.Empty, this.TSB1TMAmount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit19, attNameExtension), string.Empty, this.TSB1TMUnit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount19, attNameExtension), string.Empty, this.TSB1TLAmount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit19, attNameExtension), string.Empty, this.TSB1TLUnit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount19, attNameExtension), string.Empty, this.TSB1TUAmount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit19, attNameExtension), string.Empty, this.TSB1TUUnit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator19, attNameExtension), string.Empty, this.TSB1MathOperator19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression19, attNameExtension), string.Empty, this.TSB1MathExpression19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N19, attNameExtension), string.Empty, this.TSB1N19.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date19, attNameExtension), string.Empty, this.TSB1Date19.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType19, attNameExtension), string.Empty, this.TSB1MathType19);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO19, attNameExtension), string.Empty, this.TSB1BaseIO19);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount19, attNameExtension), string.Empty, this.TSB11Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit19, attNameExtension), string.Empty, this.TSB11Unit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount19, attNameExtension), string.Empty, this.TSB12Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit19, attNameExtension), string.Empty, this.TSB12Unit19);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount19, attNameExtension), string.Empty, this.TSB15Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit19, attNameExtension), string.Empty, this.TSB15Unit19.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                   string.Concat(cTSB13Amount19, attNameExtension), string.Empty, this.TSB13Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit19, attNameExtension), string.Empty, this.TSB13Unit19);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount19, attNameExtension), string.Empty, this.TSB14Amount19.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit19, attNameExtension), string.Empty, this.TSB14Unit19);
            }
            if (this.TSB1Name20 != string.Empty && this.TSB1Name20 != Constants.NONE)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Name20, attNameExtension), string.Empty, this.TSB1Name20);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1Description20, attNameExtension), string.Empty, this.TSB1Description20);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Label20, attNameExtension), string.Empty, this.TSB1Label20);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Type20, attNameExtension), string.Empty, this.TSB1Type20);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1RelLabel20, attNameExtension), string.Empty, this.TSB1RelLabel20);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TAmount20, attNameExtension), string.Empty, this.TSB1TAmount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUnit20, attNameExtension), string.Empty, this.TSB1TUnit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Amount20, attNameExtension), string.Empty, this.TSB1TD1Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD1Unit20, attNameExtension), string.Empty, this.TSB1TD1Unit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Amount20, attNameExtension), string.Empty, this.TSB1TD2Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TD2Unit20, attNameExtension), string.Empty, this.TSB1TD2Unit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathResult20, attNameExtension), string.Empty, this.TSB1MathResult20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathSubType20, attNameExtension), string.Empty, this.TSB1MathSubType20.ToString());

                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMAmount20, attNameExtension), string.Empty, this.TSB1TMAmount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TMUnit20, attNameExtension), string.Empty, this.TSB1TMUnit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLAmount20, attNameExtension), string.Empty, this.TSB1TLAmount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TLUnit20, attNameExtension), string.Empty, this.TSB1TLUnit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUAmount20, attNameExtension), string.Empty, this.TSB1TUAmount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1TUUnit20, attNameExtension), string.Empty, this.TSB1TUUnit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                  string.Concat(cTSB1MathOperator20, attNameExtension), string.Empty, this.TSB1MathOperator20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1MathExpression20, attNameExtension), string.Empty, this.TSB1MathExpression20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1N20, attNameExtension), string.Empty, this.TSB1N20.ToString("N1", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1Date20, attNameExtension), string.Empty, this.TSB1Date20.ToString("d", DateTimeFormatInfo.InvariantInfo));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB1MathType20, attNameExtension), string.Empty, this.TSB1MathType20);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB1BaseIO20, attNameExtension), string.Empty, this.TSB1BaseIO20);
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Amount20, attNameExtension), string.Empty, this.TSB11Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB11Unit20, attNameExtension), string.Empty, this.TSB11Unit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                        string.Concat(cTSB12Amount20, attNameExtension), string.Empty, this.TSB12Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB12Unit20, attNameExtension), string.Empty, this.TSB12Unit20);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Amount20, attNameExtension), string.Empty, this.TSB15Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB15Unit20, attNameExtension), string.Empty, this.TSB15Unit20.ToString());
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Amount20, attNameExtension), string.Empty, this.TSB13Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB13Unit20, attNameExtension), string.Empty, this.TSB13Unit20);
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Amount20, attNameExtension), string.Empty, this.TSB14Amount20.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTSB14Unit20, attNameExtension), string.Empty, this.TSB14Unit20);
            }
        }
    }
        #endregion

    public static class TSB1Extensions
    {
        public static bool AddSubTotalToTotalStock(this TSB1BaseStock baseStat, TSB1BaseStock subTotal, string selectedLabel = "")
        {
            bool bHasTotals = false;
            //add the combined scores
            baseStat.AddSubStockScoreToTotalStock(subTotal);
            //add the individual indicators
            int i = baseStat.GetStockIndex(subTotal.TSB1Label1);
            if (selectedLabel != string.Empty)
            {
                i = baseStat.GetStockIndex(selectedLabel);
            }
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name1, subTotal.TSB1Label1, subTotal.TSB1RelLabel1,
                        subTotal.TSB1Description1, subTotal.TSB1MathExpression1, subTotal.TSB1Date1,
                        subTotal.TSB1Type1, subTotal.TSB1MathType1, subTotal.TSB1BaseIO1, subTotal.TSB1MathOperator1, subTotal.TSB1MathSubType1,
                        subTotal.TSB11Unit1, subTotal.TSB12Unit1, subTotal.TSB13Unit1, subTotal.TSB14Unit1, subTotal.TSB15Unit1, subTotal.TSB1TUnit1,
                        subTotal.TSB1TD1Unit1, subTotal.TSB1TD2Unit1, subTotal.TSB1TMUnit1, subTotal.TSB1TLUnit1, subTotal.TSB1TUUnit1);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N1,
                    subTotal.TSB11Amount1, subTotal.TSB12Amount1, subTotal.TSB13Amount1, subTotal.TSB14Amount1, subTotal.TSB15Amount1, subTotal.TSB1TAmount1,
                    subTotal.TSB1TD1Amount1, subTotal.TSB1TD2Amount1, subTotal.TSB1TMAmount1, subTotal.TSB1TLAmount1, subTotal.TSB1TUAmount1);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label2);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name2, subTotal.TSB1Label2, subTotal.TSB1RelLabel2,
                        subTotal.TSB1Description2, subTotal.TSB1MathExpression2, subTotal.TSB1Date2,
                        subTotal.TSB1Type2, subTotal.TSB1MathType2, subTotal.TSB1BaseIO2, subTotal.TSB1MathOperator2, subTotal.TSB1MathSubType2,
                        subTotal.TSB11Unit2, subTotal.TSB12Unit2, subTotal.TSB13Unit2, subTotal.TSB14Unit2, subTotal.TSB15Unit2, subTotal.TSB1TUnit2,
                        subTotal.TSB1TD1Unit2, subTotal.TSB1TD2Unit2, subTotal.TSB1TMUnit2, subTotal.TSB1TLUnit2, subTotal.TSB1TUUnit2);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N2,
                    subTotal.TSB11Amount2, subTotal.TSB12Amount2, subTotal.TSB13Amount2, subTotal.TSB14Amount2, subTotal.TSB15Amount2, subTotal.TSB1TAmount2,
                    subTotal.TSB1TD1Amount2, subTotal.TSB1TD2Amount2, subTotal.TSB1TMAmount2, subTotal.TSB1TLAmount2, subTotal.TSB1TUAmount2);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label3);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name3, subTotal.TSB1Label3, subTotal.TSB1RelLabel3,
                        subTotal.TSB1Description3, subTotal.TSB1MathExpression3, subTotal.TSB1Date3,
                        subTotal.TSB1Type3, subTotal.TSB1MathType3, subTotal.TSB1BaseIO3, subTotal.TSB1MathOperator3, subTotal.TSB1MathSubType3,
                        subTotal.TSB11Unit3, subTotal.TSB12Unit3, subTotal.TSB13Unit3, subTotal.TSB14Unit3, subTotal.TSB15Unit3, subTotal.TSB1TUnit3,
                        subTotal.TSB1TD1Unit3, subTotal.TSB1TD2Unit3, subTotal.TSB1TMUnit3, subTotal.TSB1TLUnit3, subTotal.TSB1TUUnit3);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N3,
                    subTotal.TSB11Amount3, subTotal.TSB12Amount3, subTotal.TSB13Amount3, subTotal.TSB14Amount3, subTotal.TSB15Amount3, subTotal.TSB1TAmount3,
                    subTotal.TSB1TD1Amount3, subTotal.TSB1TD2Amount3, subTotal.TSB1TMAmount3, subTotal.TSB1TLAmount3, subTotal.TSB1TUAmount3);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label4);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name4, subTotal.TSB1Label4, subTotal.TSB1RelLabel4,
                       subTotal.TSB1Description4, subTotal.TSB1MathExpression4, subTotal.TSB1Date4,
                        subTotal.TSB1Type4, subTotal.TSB1MathType4, subTotal.TSB1BaseIO4, subTotal.TSB1MathOperator4, subTotal.TSB1MathSubType4,
                        subTotal.TSB11Unit4, subTotal.TSB12Unit4, subTotal.TSB13Unit4, subTotal.TSB14Unit4, subTotal.TSB15Unit4, subTotal.TSB1TUnit4,
                        subTotal.TSB1TD1Unit4, subTotal.TSB1TD2Unit4, subTotal.TSB1TMUnit4, subTotal.TSB1TLUnit4, subTotal.TSB1TUUnit4);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N4,
                    subTotal.TSB11Amount4, subTotal.TSB12Amount4, subTotal.TSB13Amount4, subTotal.TSB14Amount4, subTotal.TSB15Amount4, subTotal.TSB1TAmount4,
                    subTotal.TSB1TD1Amount4, subTotal.TSB1TD2Amount4, subTotal.TSB1TMAmount4, subTotal.TSB1TLAmount4, subTotal.TSB1TUAmount4);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label5);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name5, subTotal.TSB1Label5, subTotal.TSB1RelLabel5,
                        subTotal.TSB1Description5, subTotal.TSB1MathExpression5, subTotal.TSB1Date5,
                        subTotal.TSB1Type5, subTotal.TSB1MathType5, subTotal.TSB1BaseIO5, subTotal.TSB1MathOperator5, subTotal.TSB1MathSubType5,
                        subTotal.TSB11Unit5, subTotal.TSB12Unit5, subTotal.TSB13Unit5, subTotal.TSB14Unit5, subTotal.TSB15Unit5, subTotal.TSB1TUnit5,
                        subTotal.TSB1TD1Unit5, subTotal.TSB1TD2Unit5, subTotal.TSB1TMUnit5, subTotal.TSB1TLUnit5, subTotal.TSB1TUUnit5);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N5,
                subTotal.TSB11Amount5, subTotal.TSB12Amount5, subTotal.TSB13Amount5, subTotal.TSB14Amount5, subTotal.TSB15Amount5, subTotal.TSB1TAmount5,
                subTotal.TSB1TD1Amount5, subTotal.TSB1TD2Amount5, subTotal.TSB1TMAmount5, subTotal.TSB1TLAmount5, subTotal.TSB1TUAmount5);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label6);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name6, subTotal.TSB1Label6, subTotal.TSB1RelLabel6,
                        subTotal.TSB1Description6, subTotal.TSB1MathExpression6, subTotal.TSB1Date6,
                        subTotal.TSB1Type6, subTotal.TSB1MathType6, subTotal.TSB1BaseIO6, subTotal.TSB1MathOperator6, subTotal.TSB1MathSubType6,
                        subTotal.TSB11Unit6, subTotal.TSB12Unit6, subTotal.TSB13Unit6, subTotal.TSB14Unit6, subTotal.TSB15Unit6, subTotal.TSB1TUnit6,
                        subTotal.TSB1TD1Unit6, subTotal.TSB1TD2Unit6, subTotal.TSB1TMUnit6, subTotal.TSB1TLUnit6, subTotal.TSB1TUUnit6);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N6,
                    subTotal.TSB11Amount6, subTotal.TSB12Amount6, subTotal.TSB13Amount6, subTotal.TSB14Amount6, subTotal.TSB15Amount6, subTotal.TSB1TAmount6,
                    subTotal.TSB1TD1Amount6, subTotal.TSB1TD2Amount6, subTotal.TSB1TMAmount6, subTotal.TSB1TLAmount6, subTotal.TSB1TUAmount6);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label7);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name7, subTotal.TSB1Label7, subTotal.TSB1RelLabel7,
                    subTotal.TSB1Description7, subTotal.TSB1MathExpression7, subTotal.TSB1Date7,
                        subTotal.TSB1Type7, subTotal.TSB1MathType7, subTotal.TSB1BaseIO7, subTotal.TSB1MathOperator7, subTotal.TSB1MathSubType7,
                        subTotal.TSB11Unit7, subTotal.TSB12Unit7, subTotal.TSB13Unit7, subTotal.TSB14Unit7, subTotal.TSB15Unit7, subTotal.TSB1TUnit7,
                        subTotal.TSB1TD1Unit7, subTotal.TSB1TD2Unit7, subTotal.TSB1TMUnit7, subTotal.TSB1TLUnit7, subTotal.TSB1TUUnit7);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N7,
                    subTotal.TSB11Amount7, subTotal.TSB12Amount7, subTotal.TSB13Amount7, subTotal.TSB14Amount7, subTotal.TSB15Amount7, subTotal.TSB1TAmount7,
                    subTotal.TSB1TD1Amount7, subTotal.TSB1TD2Amount7, subTotal.TSB1TMAmount7, subTotal.TSB1TLAmount7, subTotal.TSB1TUAmount7);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label8);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name8, subTotal.TSB1Label8, subTotal.TSB1RelLabel8,
                    subTotal.TSB1Description8, subTotal.TSB1MathExpression8, subTotal.TSB1Date8,
                        subTotal.TSB1Type8, subTotal.TSB1MathType8, subTotal.TSB1BaseIO8, subTotal.TSB1MathOperator8, subTotal.TSB1MathSubType8,
                        subTotal.TSB11Unit8, subTotal.TSB12Unit8, subTotal.TSB13Unit8, subTotal.TSB14Unit8, subTotal.TSB15Unit8, subTotal.TSB1TUnit8,
                        subTotal.TSB1TD1Unit8, subTotal.TSB1TD2Unit8, subTotal.TSB1TMUnit8, subTotal.TSB1TLUnit8, subTotal.TSB1TUUnit8);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N8,
                subTotal.TSB11Amount8, subTotal.TSB12Amount8, subTotal.TSB13Amount8, subTotal.TSB14Amount8, subTotal.TSB15Amount8, subTotal.TSB1TAmount8,
                subTotal.TSB1TD1Amount8, subTotal.TSB1TD2Amount8, subTotal.TSB1TMAmount8, subTotal.TSB1TLAmount8, subTotal.TSB1TUAmount8);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label9);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name9, subTotal.TSB1Label9, subTotal.TSB1RelLabel9,
                    subTotal.TSB1Description9, subTotal.TSB1MathExpression9, subTotal.TSB1Date9,
                        subTotal.TSB1Type9, subTotal.TSB1MathType9, subTotal.TSB1BaseIO9, subTotal.TSB1MathOperator9, subTotal.TSB1MathSubType9,
                        subTotal.TSB11Unit9, subTotal.TSB12Unit9, subTotal.TSB13Unit9, subTotal.TSB14Unit9, subTotal.TSB15Unit9, subTotal.TSB1TUnit9,
                        subTotal.TSB1TD1Unit9, subTotal.TSB1TD2Unit9, subTotal.TSB1TMUnit9, subTotal.TSB1TLUnit9, subTotal.TSB1TUUnit9);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N9,
                    subTotal.TSB11Amount9, subTotal.TSB12Amount9, subTotal.TSB13Amount9, subTotal.TSB14Amount9, subTotal.TSB15Amount9, subTotal.TSB1TAmount9,
                    subTotal.TSB1TD1Amount9, subTotal.TSB1TD2Amount9, subTotal.TSB1TMAmount9, subTotal.TSB1TLAmount9, subTotal.TSB1TUAmount9);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label10);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name10, subTotal.TSB1Label10, subTotal.TSB1RelLabel10,
                       subTotal.TSB1Description10, subTotal.TSB1MathExpression10, subTotal.TSB1Date10,
                        subTotal.TSB1Type10, subTotal.TSB1MathType10, subTotal.TSB1BaseIO10, subTotal.TSB1MathOperator10, subTotal.TSB1MathSubType10,
                        subTotal.TSB11Unit10, subTotal.TSB12Unit10, subTotal.TSB13Unit10, subTotal.TSB14Unit10, subTotal.TSB15Unit10, subTotal.TSB1TUnit10,
                        subTotal.TSB1TD1Unit10, subTotal.TSB1TD2Unit10, subTotal.TSB1TMUnit10, subTotal.TSB1TLUnit10, subTotal.TSB1TUUnit10);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N10,
                    subTotal.TSB11Amount10, subTotal.TSB12Amount10, subTotal.TSB13Amount10, subTotal.TSB14Amount10, subTotal.TSB15Amount10, subTotal.TSB1TAmount10,
                    subTotal.TSB1TD1Amount10, subTotal.TSB1TD2Amount10, subTotal.TSB1TMAmount10, subTotal.TSB1TLAmount10, subTotal.TSB1TUAmount10);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label11);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name11, subTotal.TSB1Label11, subTotal.TSB1RelLabel11,
                        subTotal.TSB1Description11, subTotal.TSB1MathExpression11, subTotal.TSB1Date11,
                        subTotal.TSB1Type11, subTotal.TSB1MathType11, subTotal.TSB1BaseIO11, subTotal.TSB1MathOperator11, subTotal.TSB1MathSubType11,
                        subTotal.TSB11Unit11, subTotal.TSB12Unit11, subTotal.TSB13Unit11, subTotal.TSB14Unit11, subTotal.TSB15Unit11, subTotal.TSB1TUnit11,
                        subTotal.TSB1TD1Unit11, subTotal.TSB1TD2Unit11, subTotal.TSB1TMUnit11, subTotal.TSB1TLUnit11, subTotal.TSB1TUUnit11);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N11,
                    subTotal.TSB11Amount11, subTotal.TSB12Amount11, subTotal.TSB13Amount11, subTotal.TSB14Amount11, subTotal.TSB15Amount11, subTotal.TSB1TAmount11,
                    subTotal.TSB1TD1Amount11, subTotal.TSB1TD2Amount11, subTotal.TSB1TMAmount11, subTotal.TSB1TLAmount11, subTotal.TSB1TUAmount11);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label12);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name12, subTotal.TSB1Label12, subTotal.TSB1RelLabel12,
                        subTotal.TSB1Description12, subTotal.TSB1MathExpression12, subTotal.TSB1Date12,
                        subTotal.TSB1Type12, subTotal.TSB1MathType12, subTotal.TSB1BaseIO12, subTotal.TSB1MathOperator12, subTotal.TSB1MathSubType12,
                        subTotal.TSB11Unit12, subTotal.TSB12Unit12, subTotal.TSB13Unit12, subTotal.TSB14Unit12, subTotal.TSB15Unit12, subTotal.TSB1TUnit12,
                        subTotal.TSB1TD1Unit12, subTotal.TSB1TD2Unit12, subTotal.TSB1TMUnit12, subTotal.TSB1TLUnit12, subTotal.TSB1TUUnit12);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N12,
                    subTotal.TSB11Amount12, subTotal.TSB12Amount12, subTotal.TSB13Amount12, subTotal.TSB14Amount12, subTotal.TSB15Amount12, subTotal.TSB1TAmount12,
                    subTotal.TSB1TD1Amount12, subTotal.TSB1TD2Amount12, subTotal.TSB1TMAmount12, subTotal.TSB1TLAmount12, subTotal.TSB1TUAmount12);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label13);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name13, subTotal.TSB1Label13, subTotal.TSB1RelLabel13,
                        subTotal.TSB1Description13, subTotal.TSB1MathExpression13, subTotal.TSB1Date13,
                        subTotal.TSB1Type13, subTotal.TSB1MathType13, subTotal.TSB1BaseIO13, subTotal.TSB1MathOperator13, subTotal.TSB1MathSubType13,
                        subTotal.TSB11Unit13, subTotal.TSB12Unit13, subTotal.TSB13Unit13, subTotal.TSB14Unit13, subTotal.TSB15Unit13, subTotal.TSB1TUnit13,
                        subTotal.TSB1TD1Unit13, subTotal.TSB1TD2Unit13, subTotal.TSB1TMUnit13, subTotal.TSB1TLUnit13, subTotal.TSB1TUUnit13);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N13,
                    subTotal.TSB11Amount13, subTotal.TSB12Amount13, subTotal.TSB13Amount13, subTotal.TSB14Amount13, subTotal.TSB15Amount13, subTotal.TSB1TAmount13,
                    subTotal.TSB1TD1Amount13, subTotal.TSB1TD2Amount13, subTotal.TSB1TMAmount13, subTotal.TSB1TLAmount13, subTotal.TSB1TUAmount13);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label14);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name14, subTotal.TSB1Label14, subTotal.TSB1RelLabel14,
                        subTotal.TSB1Description14, subTotal.TSB1MathExpression14, subTotal.TSB1Date14,
                        subTotal.TSB1Type14, subTotal.TSB1MathType14, subTotal.TSB1BaseIO14, subTotal.TSB1MathOperator14, subTotal.TSB1MathSubType14,
                        subTotal.TSB11Unit14, subTotal.TSB12Unit14, subTotal.TSB13Unit14, subTotal.TSB14Unit14, subTotal.TSB15Unit14, subTotal.TSB1TUnit14,
                        subTotal.TSB1TD1Unit14, subTotal.TSB1TD2Unit14, subTotal.TSB1TMUnit14, subTotal.TSB1TLUnit14, subTotal.TSB1TUUnit14);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N14,
                    subTotal.TSB11Amount14, subTotal.TSB12Amount14, subTotal.TSB13Amount14, subTotal.TSB14Amount14, subTotal.TSB15Amount14, subTotal.TSB1TAmount14,
                    subTotal.TSB1TD1Amount14, subTotal.TSB1TD2Amount14, subTotal.TSB1TMAmount14, subTotal.TSB1TLAmount14, subTotal.TSB1TUAmount14);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label15);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name15, subTotal.TSB1Label15, subTotal.TSB1RelLabel15,
                        subTotal.TSB1Description15, subTotal.TSB1MathExpression15, subTotal.TSB1Date15,
                        subTotal.TSB1Type15, subTotal.TSB1MathType15, subTotal.TSB1BaseIO15, subTotal.TSB1MathOperator15, subTotal.TSB1MathSubType15,
                        subTotal.TSB11Unit15, subTotal.TSB12Unit15, subTotal.TSB13Unit15, subTotal.TSB14Unit15, subTotal.TSB15Unit15, subTotal.TSB1TUnit15,
                        subTotal.TSB1TD1Unit15, subTotal.TSB1TD2Unit15, subTotal.TSB1TMUnit15, subTotal.TSB1TLUnit15, subTotal.TSB1TUUnit15);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N15,
                    subTotal.TSB11Amount15, subTotal.TSB12Amount15, subTotal.TSB13Amount15, subTotal.TSB14Amount15, subTotal.TSB15Amount15, subTotal.TSB1TAmount15,
                    subTotal.TSB1TD1Amount15, subTotal.TSB1TD2Amount15, subTotal.TSB1TMAmount15, subTotal.TSB1TLAmount15, subTotal.TSB1TUAmount15);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label16);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name16, subTotal.TSB1Label16, subTotal.TSB1RelLabel16,
                        subTotal.TSB1Description16, subTotal.TSB1MathExpression16, subTotal.TSB1Date16,
                        subTotal.TSB1Type16, subTotal.TSB1MathType16, subTotal.TSB1BaseIO16, subTotal.TSB1MathOperator16, subTotal.TSB1MathSubType16,
                        subTotal.TSB11Unit16, subTotal.TSB12Unit16, subTotal.TSB13Unit16, subTotal.TSB14Unit16, subTotal.TSB15Unit16, subTotal.TSB1TUnit16,
                        subTotal.TSB1TD1Unit16, subTotal.TSB1TD2Unit16, subTotal.TSB1TMUnit16, subTotal.TSB1TLUnit16, subTotal.TSB1TUUnit16);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N16,
                    subTotal.TSB11Amount16, subTotal.TSB12Amount16, subTotal.TSB13Amount16, subTotal.TSB14Amount16, subTotal.TSB15Amount16, subTotal.TSB1TAmount16,
                    subTotal.TSB1TD1Amount16, subTotal.TSB1TD2Amount16, subTotal.TSB1TMAmount16, subTotal.TSB1TLAmount16, subTotal.TSB1TUAmount16);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label17);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name17, subTotal.TSB1Label17, subTotal.TSB1RelLabel17,
                        subTotal.TSB1Description17, subTotal.TSB1MathExpression17, subTotal.TSB1Date17,
                        subTotal.TSB1Type17, subTotal.TSB1MathType17, subTotal.TSB1BaseIO17, subTotal.TSB1MathOperator17, subTotal.TSB1MathSubType17,
                        subTotal.TSB11Unit17, subTotal.TSB12Unit17, subTotal.TSB13Unit17, subTotal.TSB14Unit17, subTotal.TSB15Unit17, subTotal.TSB1TUnit17,
                        subTotal.TSB1TD1Unit17, subTotal.TSB1TD2Unit17, subTotal.TSB1TMUnit17, subTotal.TSB1TLUnit17, subTotal.TSB1TUUnit17);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N17,
                    subTotal.TSB11Amount17, subTotal.TSB12Amount17, subTotal.TSB13Amount17, subTotal.TSB14Amount17, subTotal.TSB15Amount17, subTotal.TSB1TAmount17,
                    subTotal.TSB1TD1Amount17, subTotal.TSB1TD2Amount17, subTotal.TSB1TMAmount17, subTotal.TSB1TLAmount17, subTotal.TSB1TUAmount17);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label18);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name18, subTotal.TSB1Label18, subTotal.TSB1RelLabel18,
                        subTotal.TSB1Description18, subTotal.TSB1MathExpression18, subTotal.TSB1Date18,
                        subTotal.TSB1Type18, subTotal.TSB1MathType18, subTotal.TSB1BaseIO18, subTotal.TSB1MathOperator18, subTotal.TSB1MathSubType18,
                        subTotal.TSB11Unit18, subTotal.TSB12Unit18, subTotal.TSB13Unit18, subTotal.TSB14Unit18, subTotal.TSB15Unit18, subTotal.TSB1TUnit18,
                        subTotal.TSB1TD1Unit18, subTotal.TSB1TD2Unit18, subTotal.TSB1TMUnit18, subTotal.TSB1TLUnit18, subTotal.TSB1TUUnit18);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N18,
                    subTotal.TSB11Amount18, subTotal.TSB12Amount18, subTotal.TSB13Amount18, subTotal.TSB14Amount18, subTotal.TSB15Amount18, subTotal.TSB1TAmount18,
                    subTotal.TSB1TD1Amount18, subTotal.TSB1TD2Amount18, subTotal.TSB1TMAmount18, subTotal.TSB1TLAmount18, subTotal.TSB1TUAmount18);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label19);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name19, subTotal.TSB1Label19, subTotal.TSB1RelLabel19,
                        subTotal.TSB1Description19, subTotal.TSB1MathExpression19, subTotal.TSB1Date19,
                        subTotal.TSB1Type19, subTotal.TSB1MathType19, subTotal.TSB1BaseIO19, subTotal.TSB1MathOperator19, subTotal.TSB1MathSubType19,
                        subTotal.TSB11Unit19, subTotal.TSB12Unit19, subTotal.TSB13Unit19, subTotal.TSB14Unit19, subTotal.TSB15Unit19, subTotal.TSB1TUnit19,
                        subTotal.TSB1TD1Unit19, subTotal.TSB1TD2Unit19, subTotal.TSB1TMUnit19, subTotal.TSB1TLUnit19, subTotal.TSB1TUUnit19);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N19,
                    subTotal.TSB11Amount19, subTotal.TSB12Amount19, subTotal.TSB13Amount19, subTotal.TSB14Amount19, subTotal.TSB15Amount19, subTotal.TSB1TAmount19,
                    subTotal.TSB1TD1Amount19, subTotal.TSB1TD2Amount19, subTotal.TSB1TMAmount19, subTotal.TSB1TLAmount19, subTotal.TSB1TUAmount19);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label20);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name20, subTotal.TSB1Label20, subTotal.TSB1RelLabel20,
                        subTotal.TSB1Description20, subTotal.TSB1MathExpression20, subTotal.TSB1Date20,
                        subTotal.TSB1Type20, subTotal.TSB1MathType20, subTotal.TSB1BaseIO20, subTotal.TSB1MathOperator20, subTotal.TSB1MathSubType20,
                        subTotal.TSB11Unit20, subTotal.TSB12Unit20, subTotal.TSB13Unit20, subTotal.TSB14Unit20, subTotal.TSB15Unit20, subTotal.TSB1TUnit20,
                        subTotal.TSB1TD1Unit20, subTotal.TSB1TD2Unit20, subTotal.TSB1TMUnit20, subTotal.TSB1TLUnit20, subTotal.TSB1TUUnit20);
                }
                AddSubStockToTotalStock(baseStat, i, subTotal.TSB1N20,
                    subTotal.TSB11Amount20, subTotal.TSB12Amount20, subTotal.TSB13Amount20, subTotal.TSB14Amount20, subTotal.TSB15Amount20, subTotal.TSB1TAmount20,
                    subTotal.TSB1TD1Amount20, subTotal.TSB1TD2Amount20, subTotal.TSB1TMAmount20, subTotal.TSB1TLAmount20, subTotal.TSB1TUAmount20);
            }
            bHasTotals = true;
            return bHasTotals;
        }
        public static bool CopySubTotalToTotalStock(this TSB1BaseStock baseStat, TSB1BaseStock subTotal, string selectedLabel = "")
        {
            bool bHasTotals = false;
            //add the combined scores
            baseStat.CopySubStockScoreToTotalStock(subTotal);
            //add the individual indicators
            int i = baseStat.GetStockIndex(subTotal.TSB1Label1);
            if (selectedLabel != string.Empty)
            {
                i = baseStat.GetStockIndex(selectedLabel);
            }
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name1, subTotal.TSB1Label1, subTotal.TSB1RelLabel1,
                        subTotal.TSB1Description1, subTotal.TSB1MathExpression1, subTotal.TSB1Date1,
                        subTotal.TSB1Type1, subTotal.TSB1MathType1, subTotal.TSB1BaseIO1, subTotal.TSB1MathOperator1, subTotal.TSB1MathSubType1,
                        subTotal.TSB11Unit1, subTotal.TSB12Unit1, subTotal.TSB13Unit1, subTotal.TSB14Unit1, subTotal.TSB15Unit1, subTotal.TSB1TUnit1,
                        subTotal.TSB1TD1Unit1, subTotal.TSB1TD2Unit1, subTotal.TSB1TMUnit1, subTotal.TSB1TLUnit1, subTotal.TSB1TUUnit1);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N1,
                    subTotal.TSB11Amount1, subTotal.TSB12Amount1, subTotal.TSB13Amount1, subTotal.TSB14Amount1, subTotal.TSB15Amount1, subTotal.TSB1TAmount1,
                    subTotal.TSB1TD1Amount1, subTotal.TSB1TD2Amount1, subTotal.TSB1TMAmount1, subTotal.TSB1TLAmount1, subTotal.TSB1TUAmount1);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label2);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name2, subTotal.TSB1Label2, subTotal.TSB1RelLabel2,
                        subTotal.TSB1Description2, subTotal.TSB1MathExpression2, subTotal.TSB1Date2,
                        subTotal.TSB1Type2, subTotal.TSB1MathType2, subTotal.TSB1BaseIO2, subTotal.TSB1MathOperator2, subTotal.TSB1MathSubType2,
                        subTotal.TSB11Unit2, subTotal.TSB12Unit2, subTotal.TSB13Unit2, subTotal.TSB14Unit2, subTotal.TSB15Unit2, subTotal.TSB1TUnit2,
                        subTotal.TSB1TD1Unit2, subTotal.TSB1TD2Unit2, subTotal.TSB1TMUnit2, subTotal.TSB1TLUnit2, subTotal.TSB1TUUnit2);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N2,
                    subTotal.TSB11Amount2, subTotal.TSB12Amount2, subTotal.TSB13Amount2, subTotal.TSB14Amount2, subTotal.TSB15Amount2, subTotal.TSB1TAmount2,
                    subTotal.TSB1TD1Amount2, subTotal.TSB1TD2Amount2, subTotal.TSB1TMAmount2, subTotal.TSB1TLAmount2, subTotal.TSB1TUAmount2);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label3);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name3, subTotal.TSB1Label3, subTotal.TSB1RelLabel3,
                        subTotal.TSB1Description3, subTotal.TSB1MathExpression3, subTotal.TSB1Date3,
                        subTotal.TSB1Type3, subTotal.TSB1MathType3, subTotal.TSB1BaseIO3, subTotal.TSB1MathOperator3, subTotal.TSB1MathSubType3,
                        subTotal.TSB11Unit3, subTotal.TSB12Unit3, subTotal.TSB13Unit3, subTotal.TSB14Unit3, subTotal.TSB15Unit3, subTotal.TSB1TUnit3,
                        subTotal.TSB1TD1Unit3, subTotal.TSB1TD2Unit3, subTotal.TSB1TMUnit3, subTotal.TSB1TLUnit3, subTotal.TSB1TUUnit3);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N3,
                    subTotal.TSB11Amount3, subTotal.TSB12Amount3, subTotal.TSB13Amount3, subTotal.TSB14Amount3, subTotal.TSB15Amount3, subTotal.TSB1TAmount3,
                    subTotal.TSB1TD1Amount3, subTotal.TSB1TD2Amount3, subTotal.TSB1TMAmount3, subTotal.TSB1TLAmount3, subTotal.TSB1TUAmount3);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label4);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name4, subTotal.TSB1Label4, subTotal.TSB1RelLabel4,
                       subTotal.TSB1Description4, subTotal.TSB1MathExpression4, subTotal.TSB1Date4,
                        subTotal.TSB1Type4, subTotal.TSB1MathType4, subTotal.TSB1BaseIO4, subTotal.TSB1MathOperator4, subTotal.TSB1MathSubType4,
                        subTotal.TSB11Unit4, subTotal.TSB12Unit4, subTotal.TSB13Unit4, subTotal.TSB14Unit4, subTotal.TSB15Unit4, subTotal.TSB1TUnit4,
                        subTotal.TSB1TD1Unit4, subTotal.TSB1TD2Unit4, subTotal.TSB1TMUnit4, subTotal.TSB1TLUnit4, subTotal.TSB1TUUnit4);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N4,
                    subTotal.TSB11Amount4, subTotal.TSB12Amount4, subTotal.TSB13Amount4, subTotal.TSB14Amount4, subTotal.TSB15Amount4, subTotal.TSB1TAmount4,
                    subTotal.TSB1TD1Amount4, subTotal.TSB1TD2Amount4, subTotal.TSB1TMAmount4, subTotal.TSB1TLAmount4, subTotal.TSB1TUAmount4);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label5);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name5, subTotal.TSB1Label5, subTotal.TSB1RelLabel5,
                        subTotal.TSB1Description5, subTotal.TSB1MathExpression5, subTotal.TSB1Date5,
                        subTotal.TSB1Type5, subTotal.TSB1MathType5, subTotal.TSB1BaseIO5, subTotal.TSB1MathOperator5, subTotal.TSB1MathSubType5,
                        subTotal.TSB11Unit5, subTotal.TSB12Unit5, subTotal.TSB13Unit5, subTotal.TSB14Unit5, subTotal.TSB15Unit5, subTotal.TSB1TUnit5,
                        subTotal.TSB1TD1Unit5, subTotal.TSB1TD2Unit5, subTotal.TSB1TMUnit5, subTotal.TSB1TLUnit5, subTotal.TSB1TUUnit5);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N5,
                    subTotal.TSB11Amount5, subTotal.TSB12Amount5, subTotal.TSB13Amount5, subTotal.TSB14Amount5, subTotal.TSB15Amount5, subTotal.TSB1TAmount5,
                    subTotal.TSB1TD1Amount5, subTotal.TSB1TD2Amount5, subTotal.TSB1TMAmount5, subTotal.TSB1TLAmount5, subTotal.TSB1TUAmount5);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label6);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name6, subTotal.TSB1Label6, subTotal.TSB1RelLabel6,
                        subTotal.TSB1Description6, subTotal.TSB1MathExpression6, subTotal.TSB1Date6,
                        subTotal.TSB1Type6, subTotal.TSB1MathType6, subTotal.TSB1BaseIO6, subTotal.TSB1MathOperator6, subTotal.TSB1MathSubType6,
                        subTotal.TSB11Unit6, subTotal.TSB12Unit6, subTotal.TSB13Unit6, subTotal.TSB14Unit6, subTotal.TSB15Unit6, subTotal.TSB1TUnit6,
                        subTotal.TSB1TD1Unit6, subTotal.TSB1TD2Unit6, subTotal.TSB1TMUnit6, subTotal.TSB1TLUnit6, subTotal.TSB1TUUnit6);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N6,
                    subTotal.TSB11Amount6, subTotal.TSB12Amount6, subTotal.TSB13Amount6, subTotal.TSB14Amount6, subTotal.TSB15Amount6, subTotal.TSB1TAmount6,
                    subTotal.TSB1TD1Amount6, subTotal.TSB1TD2Amount6, subTotal.TSB1TMAmount6, subTotal.TSB1TLAmount6, subTotal.TSB1TUAmount6);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label7);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name7, subTotal.TSB1Label7, subTotal.TSB1RelLabel7,
                    subTotal.TSB1Description7, subTotal.TSB1MathExpression7, subTotal.TSB1Date7,
                        subTotal.TSB1Type7, subTotal.TSB1MathType7, subTotal.TSB1BaseIO7, subTotal.TSB1MathOperator7, subTotal.TSB1MathSubType7,
                        subTotal.TSB11Unit7, subTotal.TSB12Unit7, subTotal.TSB13Unit7, subTotal.TSB14Unit7, subTotal.TSB15Unit7, subTotal.TSB1TUnit7,
                        subTotal.TSB1TD1Unit7, subTotal.TSB1TD2Unit7, subTotal.TSB1TMUnit7, subTotal.TSB1TLUnit7, subTotal.TSB1TUUnit7);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N7,
                    subTotal.TSB11Amount7, subTotal.TSB12Amount7, subTotal.TSB13Amount7, subTotal.TSB14Amount7, subTotal.TSB15Amount7, subTotal.TSB1TAmount7,
                    subTotal.TSB1TD1Amount7, subTotal.TSB1TD2Amount7, subTotal.TSB1TMAmount7, subTotal.TSB1TLAmount7, subTotal.TSB1TUAmount7);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label8);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name8, subTotal.TSB1Label8, subTotal.TSB1RelLabel8,
                    subTotal.TSB1Description8, subTotal.TSB1MathExpression8, subTotal.TSB1Date8,
                        subTotal.TSB1Type8, subTotal.TSB1MathType8, subTotal.TSB1BaseIO8, subTotal.TSB1MathOperator8, subTotal.TSB1MathSubType8,
                        subTotal.TSB11Unit8, subTotal.TSB12Unit8, subTotal.TSB13Unit8, subTotal.TSB14Unit8, subTotal.TSB15Unit8, subTotal.TSB1TUnit8,
                        subTotal.TSB1TD1Unit8, subTotal.TSB1TD2Unit8, subTotal.TSB1TMUnit8, subTotal.TSB1TLUnit8, subTotal.TSB1TUUnit8);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N8,
                    subTotal.TSB11Amount8, subTotal.TSB12Amount8, subTotal.TSB13Amount8, subTotal.TSB14Amount8, subTotal.TSB15Amount8, subTotal.TSB1TAmount8,
                    subTotal.TSB1TD1Amount8, subTotal.TSB1TD2Amount8, subTotal.TSB1TMAmount8, subTotal.TSB1TLAmount8, subTotal.TSB1TUAmount8);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label9);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name9, subTotal.TSB1Label9, subTotal.TSB1RelLabel9,
                    subTotal.TSB1Description9, subTotal.TSB1MathExpression9, subTotal.TSB1Date9,
                        subTotal.TSB1Type9, subTotal.TSB1MathType9, subTotal.TSB1BaseIO9, subTotal.TSB1MathOperator9, subTotal.TSB1MathSubType9,
                        subTotal.TSB11Unit9, subTotal.TSB12Unit9, subTotal.TSB13Unit9, subTotal.TSB14Unit9, subTotal.TSB15Unit9, subTotal.TSB1TUnit9,
                        subTotal.TSB1TD1Unit9, subTotal.TSB1TD2Unit9, subTotal.TSB1TMUnit9, subTotal.TSB1TLUnit9, subTotal.TSB1TUUnit9);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N9,
                    subTotal.TSB11Amount9, subTotal.TSB12Amount9, subTotal.TSB13Amount9, subTotal.TSB14Amount9, subTotal.TSB15Amount9, subTotal.TSB1TAmount9,
                    subTotal.TSB1TD1Amount9, subTotal.TSB1TD2Amount9, subTotal.TSB1TMAmount9, subTotal.TSB1TLAmount9, subTotal.TSB1TUAmount9);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label10);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name10, subTotal.TSB1Label10, subTotal.TSB1RelLabel10,
                       subTotal.TSB1Description10, subTotal.TSB1MathExpression10, subTotal.TSB1Date10,
                        subTotal.TSB1Type10, subTotal.TSB1MathType10, subTotal.TSB1BaseIO10, subTotal.TSB1MathOperator10, subTotal.TSB1MathSubType10,
                        subTotal.TSB11Unit10, subTotal.TSB12Unit10, subTotal.TSB13Unit10, subTotal.TSB14Unit10, subTotal.TSB15Unit10, subTotal.TSB1TUnit10,
                        subTotal.TSB1TD1Unit10, subTotal.TSB1TD2Unit10, subTotal.TSB1TMUnit10, subTotal.TSB1TLUnit10, subTotal.TSB1TUUnit10);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N10,
                    subTotal.TSB11Amount10, subTotal.TSB12Amount10, subTotal.TSB13Amount10, subTotal.TSB14Amount10, subTotal.TSB15Amount10, subTotal.TSB1TAmount10,
                    subTotal.TSB1TD1Amount10, subTotal.TSB1TD2Amount10, subTotal.TSB1TMAmount10, subTotal.TSB1TLAmount10, subTotal.TSB1TUAmount10);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label11);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name11, subTotal.TSB1Label11, subTotal.TSB1RelLabel11,
                        subTotal.TSB1Description11, subTotal.TSB1MathExpression11, subTotal.TSB1Date11,
                        subTotal.TSB1Type11, subTotal.TSB1MathType11, subTotal.TSB1BaseIO11, subTotal.TSB1MathOperator11, subTotal.TSB1MathSubType11,
                        subTotal.TSB11Unit11, subTotal.TSB12Unit11, subTotal.TSB13Unit11, subTotal.TSB14Unit11, subTotal.TSB15Unit11, subTotal.TSB1TUnit11,
                        subTotal.TSB1TD1Unit11, subTotal.TSB1TD2Unit11, subTotal.TSB1TMUnit11, subTotal.TSB1TLUnit11, subTotal.TSB1TUUnit11);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N11,
                    subTotal.TSB11Amount11, subTotal.TSB12Amount11, subTotal.TSB13Amount11, subTotal.TSB14Amount11, subTotal.TSB15Amount11, subTotal.TSB1TAmount11,
                    subTotal.TSB1TD1Amount11, subTotal.TSB1TD2Amount11, subTotal.TSB1TMAmount11, subTotal.TSB1TLAmount11, subTotal.TSB1TUAmount11);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label12);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name12, subTotal.TSB1Label12, subTotal.TSB1RelLabel12,
                        subTotal.TSB1Description12, subTotal.TSB1MathExpression12, subTotal.TSB1Date12,
                        subTotal.TSB1Type12, subTotal.TSB1MathType12, subTotal.TSB1BaseIO12, subTotal.TSB1MathOperator12, subTotal.TSB1MathSubType12,
                        subTotal.TSB11Unit12, subTotal.TSB12Unit12, subTotal.TSB13Unit12, subTotal.TSB14Unit12, subTotal.TSB15Unit12, subTotal.TSB1TUnit12,
                        subTotal.TSB1TD1Unit12, subTotal.TSB1TD2Unit12, subTotal.TSB1TMUnit12, subTotal.TSB1TLUnit12, subTotal.TSB1TUUnit12);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N12,
                    subTotal.TSB11Amount12, subTotal.TSB12Amount12, subTotal.TSB13Amount12, subTotal.TSB14Amount12, subTotal.TSB15Amount12, subTotal.TSB1TAmount12,
                    subTotal.TSB1TD1Amount12, subTotal.TSB1TD2Amount12, subTotal.TSB1TMAmount12, subTotal.TSB1TLAmount12, subTotal.TSB1TUAmount12);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label13);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name13, subTotal.TSB1Label13, subTotal.TSB1RelLabel13,
                        subTotal.TSB1Description13, subTotal.TSB1MathExpression13, subTotal.TSB1Date13,
                        subTotal.TSB1Type13, subTotal.TSB1MathType13, subTotal.TSB1BaseIO13, subTotal.TSB1MathOperator13, subTotal.TSB1MathSubType13,
                        subTotal.TSB11Unit13, subTotal.TSB12Unit13, subTotal.TSB13Unit13, subTotal.TSB14Unit13, subTotal.TSB15Unit13, subTotal.TSB1TUnit13,
                        subTotal.TSB1TD1Unit13, subTotal.TSB1TD2Unit13, subTotal.TSB1TMUnit13, subTotal.TSB1TLUnit13, subTotal.TSB1TUUnit13);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N13,
                    subTotal.TSB11Amount13, subTotal.TSB12Amount13, subTotal.TSB13Amount13, subTotal.TSB14Amount13, subTotal.TSB15Amount13, subTotal.TSB1TAmount13,
                    subTotal.TSB1TD1Amount13, subTotal.TSB1TD2Amount13, subTotal.TSB1TMAmount13, subTotal.TSB1TLAmount13, subTotal.TSB1TUAmount13);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label14);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name14, subTotal.TSB1Label14, subTotal.TSB1RelLabel14,
                        subTotal.TSB1Description14, subTotal.TSB1MathExpression14, subTotal.TSB1Date14,
                        subTotal.TSB1Type14, subTotal.TSB1MathType14, subTotal.TSB1BaseIO14, subTotal.TSB1MathOperator14, subTotal.TSB1MathSubType14,
                        subTotal.TSB11Unit14, subTotal.TSB12Unit14, subTotal.TSB13Unit14, subTotal.TSB14Unit14, subTotal.TSB15Unit14, subTotal.TSB1TUnit14,
                        subTotal.TSB1TD1Unit14, subTotal.TSB1TD2Unit14, subTotal.TSB1TMUnit14, subTotal.TSB1TLUnit14, subTotal.TSB1TUUnit14);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N14,
                    subTotal.TSB11Amount14, subTotal.TSB12Amount14, subTotal.TSB13Amount14, subTotal.TSB14Amount14, subTotal.TSB15Amount14, subTotal.TSB1TAmount14,
                    subTotal.TSB1TD1Amount14, subTotal.TSB1TD2Amount14, subTotal.TSB1TMAmount14, subTotal.TSB1TLAmount14, subTotal.TSB1TUAmount14);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label15);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name15, subTotal.TSB1Label15, subTotal.TSB1RelLabel15,
                        subTotal.TSB1Description15, subTotal.TSB1MathExpression15, subTotal.TSB1Date15,
                        subTotal.TSB1Type15, subTotal.TSB1MathType15, subTotal.TSB1BaseIO15, subTotal.TSB1MathOperator15, subTotal.TSB1MathSubType15,
                        subTotal.TSB11Unit15, subTotal.TSB12Unit15, subTotal.TSB13Unit15, subTotal.TSB14Unit15, subTotal.TSB15Unit15, subTotal.TSB1TUnit15,
                        subTotal.TSB1TD1Unit15, subTotal.TSB1TD2Unit15, subTotal.TSB1TMUnit15, subTotal.TSB1TLUnit15, subTotal.TSB1TUUnit15);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N15,
                    subTotal.TSB11Amount15, subTotal.TSB12Amount15, subTotal.TSB13Amount15, subTotal.TSB14Amount15, subTotal.TSB15Amount15, subTotal.TSB1TAmount15,
                    subTotal.TSB1TD1Amount15, subTotal.TSB1TD2Amount15, subTotal.TSB1TMAmount15, subTotal.TSB1TLAmount15, subTotal.TSB1TUAmount15);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label16);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name16, subTotal.TSB1Label16, subTotal.TSB1RelLabel16,
                        subTotal.TSB1Description16, subTotal.TSB1MathExpression16, subTotal.TSB1Date16,
                        subTotal.TSB1Type16, subTotal.TSB1MathType16, subTotal.TSB1BaseIO16, subTotal.TSB1MathOperator16, subTotal.TSB1MathSubType16,
                        subTotal.TSB11Unit16, subTotal.TSB12Unit16, subTotal.TSB13Unit16, subTotal.TSB14Unit16, subTotal.TSB15Unit16, subTotal.TSB1TUnit16,
                        subTotal.TSB1TD1Unit16, subTotal.TSB1TD2Unit16, subTotal.TSB1TMUnit16, subTotal.TSB1TLUnit16, subTotal.TSB1TUUnit16);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N16,
                    subTotal.TSB11Amount16, subTotal.TSB12Amount16, subTotal.TSB13Amount16, subTotal.TSB14Amount16, subTotal.TSB15Amount16, subTotal.TSB1TAmount16,
                    subTotal.TSB1TD1Amount16, subTotal.TSB1TD2Amount16, subTotal.TSB1TMAmount16, subTotal.TSB1TLAmount16, subTotal.TSB1TUAmount16);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label17);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name17, subTotal.TSB1Label17, subTotal.TSB1RelLabel17,
                        subTotal.TSB1Description17, subTotal.TSB1MathExpression17, subTotal.TSB1Date17,
                        subTotal.TSB1Type17, subTotal.TSB1MathType17, subTotal.TSB1BaseIO17, subTotal.TSB1MathOperator17, subTotal.TSB1MathSubType17,
                        subTotal.TSB11Unit17, subTotal.TSB12Unit17, subTotal.TSB13Unit17, subTotal.TSB14Unit17, subTotal.TSB15Unit17, subTotal.TSB1TUnit17,
                        subTotal.TSB1TD1Unit17, subTotal.TSB1TD2Unit17, subTotal.TSB1TMUnit17, subTotal.TSB1TLUnit17, subTotal.TSB1TUUnit17);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N17,
                    subTotal.TSB11Amount17, subTotal.TSB12Amount17, subTotal.TSB13Amount17, subTotal.TSB14Amount17, subTotal.TSB15Amount17, subTotal.TSB1TAmount17,
                    subTotal.TSB1TD1Amount17, subTotal.TSB1TD2Amount17, subTotal.TSB1TMAmount17, subTotal.TSB1TLAmount17, subTotal.TSB1TUAmount17);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label18);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name18, subTotal.TSB1Label18, subTotal.TSB1RelLabel18,
                        subTotal.TSB1Description18, subTotal.TSB1MathExpression18, subTotal.TSB1Date18,
                        subTotal.TSB1Type18, subTotal.TSB1MathType18, subTotal.TSB1BaseIO18, subTotal.TSB1MathOperator18, subTotal.TSB1MathSubType18,
                        subTotal.TSB11Unit18, subTotal.TSB12Unit18, subTotal.TSB13Unit18, subTotal.TSB14Unit18, subTotal.TSB15Unit18, subTotal.TSB1TUnit18,
                        subTotal.TSB1TD1Unit18, subTotal.TSB1TD2Unit18, subTotal.TSB1TMUnit18, subTotal.TSB1TLUnit18, subTotal.TSB1TUUnit18);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N18,
                    subTotal.TSB11Amount18, subTotal.TSB12Amount18, subTotal.TSB13Amount18, subTotal.TSB14Amount18, subTotal.TSB15Amount18, subTotal.TSB1TAmount18,
                    subTotal.TSB1TD1Amount18, subTotal.TSB1TD2Amount18, subTotal.TSB1TMAmount18, subTotal.TSB1TLAmount18, subTotal.TSB1TUAmount18);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label19);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name19, subTotal.TSB1Label19, subTotal.TSB1RelLabel19,
                        subTotal.TSB1Description19, subTotal.TSB1MathExpression19, subTotal.TSB1Date19,
                        subTotal.TSB1Type19, subTotal.TSB1MathType19, subTotal.TSB1BaseIO19, subTotal.TSB1MathOperator19, subTotal.TSB1MathSubType19,
                        subTotal.TSB11Unit19, subTotal.TSB12Unit19, subTotal.TSB13Unit19, subTotal.TSB14Unit19, subTotal.TSB15Unit19, subTotal.TSB1TUnit19,
                        subTotal.TSB1TD1Unit19, subTotal.TSB1TD2Unit19, subTotal.TSB1TMUnit19, subTotal.TSB1TLUnit19, subTotal.TSB1TUUnit19);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N19,
                    subTotal.TSB11Amount19, subTotal.TSB12Amount19, subTotal.TSB13Amount19, subTotal.TSB14Amount19, subTotal.TSB15Amount19, subTotal.TSB1TAmount19,
                    subTotal.TSB1TD1Amount19, subTotal.TSB1TD2Amount19, subTotal.TSB1TMAmount19, subTotal.TSB1TLAmount19, subTotal.TSB1TUAmount19);
            }
            i = baseStat.GetStockIndex(subTotal.TSB1Label20);
            if (i > 0)
            {
                if (baseStat.TotalStockNeedsIds(i))
                {
                    AddSubStockToTotalStock(baseStat, i, subTotal.TSB1Name20, subTotal.TSB1Label20, subTotal.TSB1RelLabel20,
                        subTotal.TSB1Description20, subTotal.TSB1MathExpression20, subTotal.TSB1Date20,
                        subTotal.TSB1Type20, subTotal.TSB1MathType20, subTotal.TSB1BaseIO20, subTotal.TSB1MathOperator20, subTotal.TSB1MathSubType20,
                        subTotal.TSB11Unit20, subTotal.TSB12Unit20, subTotal.TSB13Unit20, subTotal.TSB14Unit20, subTotal.TSB15Unit20, subTotal.TSB1TUnit20,
                        subTotal.TSB1TD1Unit20, subTotal.TSB1TD2Unit20, subTotal.TSB1TMUnit20, subTotal.TSB1TLUnit20, subTotal.TSB1TUUnit20);
                }
                CopySubStockToTotalStock(baseStat, i, subTotal.TSB1N20,
                    subTotal.TSB11Amount20, subTotal.TSB12Amount20, subTotal.TSB13Amount20, subTotal.TSB14Amount20, subTotal.TSB15Amount20, subTotal.TSB1TAmount20,
                    subTotal.TSB1TD1Amount20, subTotal.TSB1TD2Amount20, subTotal.TSB1TMAmount20, subTotal.TSB1TLAmount20, subTotal.TSB1TUAmount20);
            }
            bHasTotals = true;
            return bHasTotals;
        }
        public static void AddSubStockToTotalStock(this TSB1BaseStock baseStat, int i, 
            string name, string label, string rellabel, string description, string mathExpression,
            DateTime date, string distType, string mathType, string baseIO, string mathOperator, string mathSubType, 
            string q1Unit, string q2Unit, string q3Unit, string q4Unit, string q5Unit, string qTUnit,
            string qTD1Unit, string qTD2Unit, string qTMUnit, string qTLUnit, string qTUUnit)
        {
            //SB1Stock.AddStockScore uses only a subset of the total props in the Stats, Change, and Progress 
            //math result not passed in -not used in agg calcs and increases file size
            if (i == 1)
            {
                baseStat.TSB1Name1 = name;
                baseStat.TSB1Label1 = label;
                baseStat.TSB1RelLabel1 = rellabel;
                baseStat.TSB1Description1 = description;
                baseStat.TSB1MathExpression1 = mathExpression;
                baseStat.TSB1Date1 = date;
                baseStat.TSB1Type1 = distType;
                baseStat.TSB1MathType1 = mathType;
                baseStat.TSB1BaseIO1 = baseIO;
                baseStat.TSB1MathOperator1 = mathOperator;
                baseStat.TSB1MathSubType1 = mathSubType;
                baseStat.TSB11Unit1 = q1Unit;
                baseStat.TSB12Unit1 = q2Unit;
                baseStat.TSB13Unit1 = q3Unit;
                baseStat.TSB14Unit1 = q4Unit;
                baseStat.TSB15Unit1 = q5Unit;
                baseStat.TSB1TUnit1 = qTUnit;
                baseStat.TSB1TD1Unit1 = qTD1Unit;
                baseStat.TSB1TD2Unit1 = qTD2Unit;
                baseStat.TSB1TMUnit1 = qTMUnit;
                baseStat.TSB1TLUnit1 = qTLUnit;
                baseStat.TSB1TUUnit1 = qTUUnit;
            }
            if (i == 2)
            {
                baseStat.TSB1Name2 = name;
                baseStat.TSB1Label2 = label;
                baseStat.TSB1RelLabel2 = rellabel;
                baseStat.TSB1Description2 = description;
                baseStat.TSB1MathExpression2 = mathExpression;
                baseStat.TSB1Date2 = date;
                baseStat.TSB1Type2 = distType;
                baseStat.TSB1MathType2 = mathType;
                baseStat.TSB1BaseIO2 = baseIO;
                baseStat.TSB1MathOperator2 = mathOperator;
                baseStat.TSB1MathSubType2 = mathSubType;
                baseStat.TSB11Unit2 = q1Unit;
                baseStat.TSB12Unit2 = q2Unit;
                baseStat.TSB13Unit2 = q3Unit;
                baseStat.TSB14Unit2 = q4Unit;
                baseStat.TSB15Unit2 = q5Unit;
                baseStat.TSB1TUnit2 = qTUnit;
                baseStat.TSB1TD1Unit2 = qTD1Unit;
                baseStat.TSB1TD2Unit2 = qTD2Unit;
                baseStat.TSB1TMUnit2 = qTMUnit;
                baseStat.TSB1TLUnit2 = qTLUnit;
                baseStat.TSB1TUUnit2 = qTUUnit;
            }
            if (i == 3)
            {
                baseStat.TSB1Name3 = name;
                baseStat.TSB1Label3 = label;
                baseStat.TSB1RelLabel3 = rellabel;
                baseStat.TSB1Description3 = description;
                baseStat.TSB1MathExpression3 = mathExpression;
                baseStat.TSB1Date3 = date;
                baseStat.TSB1Type3 = distType;
                baseStat.TSB1MathType3 = mathType;
                baseStat.TSB1BaseIO3 = baseIO;
                baseStat.TSB1MathOperator3 = mathOperator;
                baseStat.TSB1MathSubType3 = mathSubType;
                baseStat.TSB11Unit3 = q1Unit;
                baseStat.TSB12Unit3 = q2Unit;
                baseStat.TSB13Unit3 = q3Unit;
                baseStat.TSB14Unit3 = q4Unit;
                baseStat.TSB15Unit3 = q5Unit;
                baseStat.TSB1TUnit3 = qTUnit;
                baseStat.TSB1TD1Unit3 = qTD1Unit;
                baseStat.TSB1TD2Unit3 = qTD2Unit;
                baseStat.TSB1TMUnit3 = qTMUnit;
                baseStat.TSB1TLUnit3 = qTLUnit;
                baseStat.TSB1TUUnit3 = qTUUnit;
            }
            if (i == 4)
            {
                baseStat.TSB1Name4 = name;
                baseStat.TSB1Label4 = label;
                baseStat.TSB1RelLabel4 = rellabel;
                baseStat.TSB1Description4 = description;
                baseStat.TSB1MathExpression4 = mathExpression;
                baseStat.TSB1Date4 = date;
                baseStat.TSB1Type4 = distType;
                baseStat.TSB1MathType4 = mathType;
                baseStat.TSB1BaseIO4 = baseIO;
                baseStat.TSB1MathOperator4 = mathOperator;
                baseStat.TSB1MathSubType4 = mathSubType;
                baseStat.TSB11Unit4 = q1Unit;
                baseStat.TSB12Unit4 = q2Unit;
                baseStat.TSB13Unit4 = q3Unit;
                baseStat.TSB14Unit4 = q4Unit;
                baseStat.TSB15Unit4 = q5Unit;
                baseStat.TSB1TUnit4 = qTUnit;
                baseStat.TSB1TD1Unit4 = qTD1Unit;
                baseStat.TSB1TD2Unit4 = qTD2Unit;
                baseStat.TSB1TMUnit4 = qTMUnit;
                baseStat.TSB1TLUnit4 = qTLUnit;
                baseStat.TSB1TUUnit4 = qTUUnit;
            }
            if (i == 5)
            {
                baseStat.TSB1Name5 = name;
                baseStat.TSB1Label5 = label;
                baseStat.TSB1RelLabel5 = rellabel;
                baseStat.TSB1Description5 = description;
                baseStat.TSB1MathExpression5 = mathExpression;
                baseStat.TSB1Date5 = date;
                baseStat.TSB1Type5 = distType;
                baseStat.TSB1MathType5 = mathType;
                baseStat.TSB1BaseIO5 = baseIO;
                baseStat.TSB1MathOperator5 = mathOperator;
                baseStat.TSB1MathSubType5 = mathSubType;
                baseStat.TSB11Unit5 = q1Unit;
                baseStat.TSB12Unit5 = q2Unit;
                baseStat.TSB13Unit5 = q3Unit;
                baseStat.TSB14Unit5 = q4Unit;
                baseStat.TSB15Unit5 = q5Unit;
                baseStat.TSB1TUnit5 = qTUnit;
                baseStat.TSB1TD1Unit5 = qTD1Unit;
                baseStat.TSB1TD2Unit5 = qTD2Unit;
                baseStat.TSB1TMUnit5 = qTMUnit;
                baseStat.TSB1TLUnit5 = qTLUnit;
                baseStat.TSB1TUUnit5 = qTUUnit;
            }
            if (i == 6)
            {
                baseStat.TSB1Name6 = name;
                baseStat.TSB1Label6 = label;
                baseStat.TSB1RelLabel6 = rellabel;
                baseStat.TSB1Description6 = description;
                baseStat.TSB1MathExpression6 = mathExpression;
                baseStat.TSB1Date6 = date;
                baseStat.TSB1Type6 = distType;
                baseStat.TSB1MathType6 = mathType;
                baseStat.TSB1BaseIO6 = baseIO;
                baseStat.TSB1MathOperator6 = mathOperator;
                baseStat.TSB1MathSubType6 = mathSubType;
                baseStat.TSB11Unit6 = q1Unit;
                baseStat.TSB12Unit6 = q2Unit;
                baseStat.TSB13Unit6 = q3Unit;
                baseStat.TSB14Unit6 = q4Unit;
                baseStat.TSB15Unit6 = q5Unit;
                baseStat.TSB1TUnit6 = qTUnit;
                baseStat.TSB1TD1Unit6 = qTD1Unit;
                baseStat.TSB1TD2Unit6 = qTD2Unit;
                baseStat.TSB1TMUnit6 = qTMUnit;
                baseStat.TSB1TLUnit6 = qTLUnit;
                baseStat.TSB1TUUnit6 = qTUUnit;
            }
            if (i == 7)
            {
                baseStat.TSB1Name7 = name;
                baseStat.TSB1Label7 = label;
                baseStat.TSB1RelLabel7 = rellabel;
                baseStat.TSB1Description7 = description;
                baseStat.TSB1MathExpression7 = mathExpression;
                baseStat.TSB1Date7 = date;
                baseStat.TSB1Type7 = distType;
                baseStat.TSB1MathType7 = mathType;
                baseStat.TSB1BaseIO7 = baseIO;
                baseStat.TSB1MathOperator7 = mathOperator;
                baseStat.TSB1MathSubType7 = mathSubType;
                baseStat.TSB11Unit7 = q1Unit;
                baseStat.TSB12Unit7 = q2Unit;
                baseStat.TSB13Unit7 = q3Unit;
                baseStat.TSB14Unit7 = q4Unit;
                baseStat.TSB15Unit7 = q5Unit;
                baseStat.TSB1TUnit7 = qTUnit;
                baseStat.TSB1TD1Unit7 = qTD1Unit;
                baseStat.TSB1TD2Unit7 = qTD2Unit;
                baseStat.TSB1TMUnit7 = qTMUnit;
                baseStat.TSB1TLUnit7 = qTLUnit;
                baseStat.TSB1TUUnit7 = qTUUnit;
            }
            if (i == 8)
            {
                baseStat.TSB1Name8 = name;
                baseStat.TSB1Label8 = label;
                baseStat.TSB1RelLabel8 = rellabel;
                baseStat.TSB1Description8 = description;
                baseStat.TSB1MathExpression8 = mathExpression;
                baseStat.TSB1Date8 = date;
                baseStat.TSB1Type8 = distType;
                baseStat.TSB1MathType8 = mathType;
                baseStat.TSB1BaseIO8 = baseIO;
                baseStat.TSB1MathOperator8 = mathOperator;
                baseStat.TSB1MathSubType8 = mathSubType;
                baseStat.TSB11Unit8 = q1Unit;
                baseStat.TSB12Unit8 = q2Unit;
                baseStat.TSB13Unit8 = q3Unit;
                baseStat.TSB14Unit8 = q4Unit;
                baseStat.TSB15Unit8 = q5Unit;
                baseStat.TSB1TUnit8 = qTUnit;
                baseStat.TSB1TD1Unit8 = qTD1Unit;
                baseStat.TSB1TD2Unit8 = qTD2Unit;
                baseStat.TSB1TMUnit8 = qTMUnit;
                baseStat.TSB1TLUnit8 = qTLUnit;
                baseStat.TSB1TUUnit8 = qTUUnit;
            }
            if (i == 9)
            {
                baseStat.TSB1Name9 = name;
                baseStat.TSB1Label9 = label;
                baseStat.TSB1RelLabel9 = rellabel;
                baseStat.TSB1Description9 = description;
                baseStat.TSB1MathExpression9 = mathExpression;
                baseStat.TSB1Date9 = date;
                baseStat.TSB1Type9 = distType;
                baseStat.TSB1MathType9 = mathType;
                baseStat.TSB1BaseIO9 = baseIO;
                baseStat.TSB1MathOperator9 = mathOperator;
                baseStat.TSB1MathSubType9 = mathSubType;
                baseStat.TSB11Unit9 = q1Unit;
                baseStat.TSB12Unit9 = q2Unit;
                baseStat.TSB13Unit9 = q3Unit;
                baseStat.TSB14Unit9 = q4Unit;
                baseStat.TSB15Unit9 = q5Unit;
                baseStat.TSB1TUnit9 = qTUnit;
                baseStat.TSB1TD1Unit9 = qTD1Unit;
                baseStat.TSB1TD2Unit9 = qTD2Unit;
                baseStat.TSB1TMUnit9 = qTMUnit;
                baseStat.TSB1TLUnit9 = qTLUnit;
                baseStat.TSB1TUUnit9 = qTUUnit;
            }
            if (i == 10)
            {
                baseStat.TSB1Name10 = name;
                baseStat.TSB1Label10 = label;
                baseStat.TSB1RelLabel10 = rellabel;
                baseStat.TSB1Description10 = description;
                baseStat.TSB1MathExpression10 = mathExpression;
                baseStat.TSB1Date10 = date;
                baseStat.TSB1Type10 = distType;
                baseStat.TSB1MathType10 = mathType;
                baseStat.TSB1BaseIO10 = baseIO;
                baseStat.TSB1MathOperator10 = mathOperator;
                baseStat.TSB1MathSubType10 = mathSubType;
                baseStat.TSB11Unit10 = q1Unit;
                baseStat.TSB12Unit10 = q2Unit;
                baseStat.TSB13Unit10 = q3Unit;
                baseStat.TSB14Unit10 = q4Unit;
                baseStat.TSB15Unit10 = q5Unit;
                baseStat.TSB1TUnit10 = qTUnit;
                baseStat.TSB1TD1Unit10 = qTD1Unit;
                baseStat.TSB1TD2Unit10 = qTD2Unit;
                baseStat.TSB1TMUnit10 = qTMUnit;
                baseStat.TSB1TLUnit10 = qTLUnit;
                baseStat.TSB1TUUnit10 = qTUUnit;
            }
            if (i == 11)
            {
                baseStat.TSB1Name11 = name;
                baseStat.TSB1Label11 = label;
                baseStat.TSB1RelLabel11 = rellabel;
                baseStat.TSB1Description11 = description;
                baseStat.TSB1MathExpression11 = mathExpression;
                baseStat.TSB1Date11 = date;
                baseStat.TSB1Type11 = distType;
                baseStat.TSB1MathType11 = mathType;
                baseStat.TSB1BaseIO11 = baseIO;
                baseStat.TSB1MathOperator11 = mathOperator;
                baseStat.TSB1MathSubType11 = mathSubType;
                baseStat.TSB11Unit11 = q1Unit;
                baseStat.TSB12Unit11 = q2Unit;
                baseStat.TSB13Unit11 = q3Unit;
                baseStat.TSB14Unit11 = q4Unit;
                baseStat.TSB15Unit11 = q5Unit;
                baseStat.TSB1TUnit11 = qTUnit;
                baseStat.TSB1TD1Unit11 = qTD1Unit;
                baseStat.TSB1TD2Unit11 = qTD2Unit;
                baseStat.TSB1TMUnit11 = qTMUnit;
                baseStat.TSB1TLUnit11 = qTLUnit;
                baseStat.TSB1TUUnit11 = qTUUnit;
            }
            if (i == 12)
            {
                baseStat.TSB1Name12 = name;
                baseStat.TSB1Label12 = label;
                baseStat.TSB1RelLabel12 = rellabel;
                baseStat.TSB1Description12 = description;
                baseStat.TSB1MathExpression12 = mathExpression;
                baseStat.TSB1Date12 = date;
                baseStat.TSB1Type12 = distType;
                baseStat.TSB1MathType12 = mathType;
                baseStat.TSB1BaseIO12 = baseIO;
                baseStat.TSB1MathOperator12 = mathOperator;
                baseStat.TSB1MathSubType12 = mathSubType;
                baseStat.TSB11Unit12 = q1Unit;
                baseStat.TSB12Unit12 = q2Unit;
                baseStat.TSB13Unit12 = q3Unit;
                baseStat.TSB14Unit12 = q4Unit;
                baseStat.TSB15Unit12 = q5Unit;
                baseStat.TSB1TUnit12 = qTUnit;
                baseStat.TSB1TD1Unit12 = qTD1Unit;
                baseStat.TSB1TD2Unit12 = qTD2Unit;
                baseStat.TSB1TMUnit12 = qTMUnit;
                baseStat.TSB1TLUnit12 = qTLUnit;
                baseStat.TSB1TUUnit12 = qTUUnit;
            }
            if (i == 13)
            {
                baseStat.TSB1Name13 = name;
                baseStat.TSB1Label13 = label;
                baseStat.TSB1RelLabel13 = rellabel;
                baseStat.TSB1Description13 = description;
                baseStat.TSB1MathExpression13 = mathExpression;
                baseStat.TSB1Date13 = date;
                baseStat.TSB1Type13 = distType;
                baseStat.TSB1MathType13 = mathType;
                baseStat.TSB1BaseIO13 = baseIO;
                baseStat.TSB1MathOperator13 = mathOperator;
                baseStat.TSB1MathSubType13 = mathSubType;
                baseStat.TSB11Unit13 = q1Unit;
                baseStat.TSB12Unit13 = q2Unit;
                baseStat.TSB13Unit13 = q3Unit;
                baseStat.TSB14Unit13 = q4Unit;
                baseStat.TSB15Unit13 = q5Unit;
                baseStat.TSB1TUnit13 = qTUnit;
                baseStat.TSB1TD1Unit13 = qTD1Unit;
                baseStat.TSB1TD2Unit13 = qTD2Unit;
                baseStat.TSB1TMUnit13 = qTMUnit;
                baseStat.TSB1TLUnit13 = qTLUnit;
                baseStat.TSB1TUUnit13 = qTUUnit;
            }
            if (i == 14)
            {
                baseStat.TSB1Name14 = name;
                baseStat.TSB1Label14 = label;
                baseStat.TSB1RelLabel14 = rellabel;
                baseStat.TSB1Description14 = description;
                baseStat.TSB1MathExpression14 = mathExpression;
                baseStat.TSB1Date14 = date;
                baseStat.TSB1Type14 = distType;
                baseStat.TSB1MathType14 = mathType;
                baseStat.TSB1BaseIO14 = baseIO;
                baseStat.TSB1MathOperator14 = mathOperator;
                baseStat.TSB1MathSubType14 = mathSubType;
                baseStat.TSB11Unit14 = q1Unit;
                baseStat.TSB12Unit14 = q2Unit;
                baseStat.TSB13Unit14 = q3Unit;
                baseStat.TSB14Unit14 = q4Unit;
                baseStat.TSB15Unit14 = q5Unit;
                baseStat.TSB1TUnit14 = qTUnit;
                baseStat.TSB1TD1Unit14 = qTD1Unit;
                baseStat.TSB1TD2Unit14 = qTD2Unit;
                baseStat.TSB1TMUnit14 = qTMUnit;
                baseStat.TSB1TLUnit14 = qTLUnit;
                baseStat.TSB1TUUnit14 = qTUUnit;
            }
            if (i == 15)
            {
                baseStat.TSB1Name15 = name;
                baseStat.TSB1Label15 = label;
                baseStat.TSB1RelLabel15 = rellabel;
                baseStat.TSB1Description15 = description;
                baseStat.TSB1MathExpression15 = mathExpression;
                baseStat.TSB1Date15 = date;
                baseStat.TSB1Type15 = distType;
                baseStat.TSB1MathType15 = mathType;
                baseStat.TSB1BaseIO15 = baseIO;
                baseStat.TSB1MathOperator15 = mathOperator;
                baseStat.TSB1MathSubType15 = mathSubType;
                baseStat.TSB11Unit15 = q1Unit;
                baseStat.TSB12Unit15 = q2Unit;
                baseStat.TSB13Unit15 = q3Unit;
                baseStat.TSB14Unit15 = q4Unit;
                baseStat.TSB15Unit15 = q5Unit;
                baseStat.TSB1TUnit15 = qTUnit;
                baseStat.TSB1TD1Unit15 = qTD1Unit;
                baseStat.TSB1TD2Unit15 = qTD2Unit;
                baseStat.TSB1TMUnit15 = qTMUnit;
                baseStat.TSB1TLUnit15 = qTLUnit;
                baseStat.TSB1TUUnit15 = qTUUnit;
            }
            if (i == 16)
            {
                baseStat.TSB1Name16 = name;
                baseStat.TSB1Label16 = label;
                baseStat.TSB1RelLabel16 = rellabel;
                baseStat.TSB1Description16 = description;
                baseStat.TSB1MathExpression16 = mathExpression;
                baseStat.TSB1Date16 = date;
                baseStat.TSB1Type16 = distType;
                baseStat.TSB1MathType16 = mathType;
                baseStat.TSB1BaseIO16 = baseIO;
                baseStat.TSB1MathOperator16 = mathOperator;
                baseStat.TSB1MathSubType16 = mathSubType;
                baseStat.TSB11Unit16 = q1Unit;
                baseStat.TSB12Unit16 = q2Unit;
                baseStat.TSB13Unit16 = q3Unit;
                baseStat.TSB14Unit16 = q4Unit;
                baseStat.TSB15Unit16 = q5Unit;
                baseStat.TSB1TUnit16 = qTUnit;
                baseStat.TSB1TD1Unit16 = qTD1Unit;
                baseStat.TSB1TD2Unit16 = qTD2Unit;
                baseStat.TSB1TMUnit16 = qTMUnit;
                baseStat.TSB1TLUnit16 = qTLUnit;
                baseStat.TSB1TUUnit16 = qTUUnit;
            }
            if (i == 17)
            {
                baseStat.TSB1Name17 = name;
                baseStat.TSB1Label17 = label;
                baseStat.TSB1RelLabel17 = rellabel;
                baseStat.TSB1Description17 = description;
                baseStat.TSB1MathExpression17 = mathExpression;
                baseStat.TSB1Date17 = date;
                baseStat.TSB1Type17 = distType;
                baseStat.TSB1MathType17 = mathType;
                baseStat.TSB1BaseIO17 = baseIO;
                baseStat.TSB1MathOperator17 = mathOperator;
                baseStat.TSB1MathSubType17 = mathSubType;
                baseStat.TSB11Unit17 = q1Unit;
                baseStat.TSB12Unit17 = q2Unit;
                baseStat.TSB13Unit17 = q3Unit;
                baseStat.TSB14Unit17 = q4Unit;
                baseStat.TSB15Unit17 = q5Unit;
                baseStat.TSB1TUnit17 = qTUnit;
                baseStat.TSB1TD1Unit17 = qTD1Unit;
                baseStat.TSB1TD2Unit17 = qTD2Unit;
                baseStat.TSB1TMUnit17 = qTMUnit;
                baseStat.TSB1TLUnit17 = qTLUnit;
                baseStat.TSB1TUUnit17 = qTUUnit;
            }
            if (i == 18)
            {
                baseStat.TSB1Name18 = name;
                baseStat.TSB1Label18 = label;
                baseStat.TSB1RelLabel18 = rellabel;
                baseStat.TSB1Description18 = description;
                baseStat.TSB1MathExpression18 = mathExpression;
                baseStat.TSB1Date18 = date;
                baseStat.TSB1Type18 = distType;
                baseStat.TSB1MathType18 = mathType;
                baseStat.TSB1BaseIO18 = baseIO;
                baseStat.TSB1MathOperator18 = mathOperator;
                baseStat.TSB1MathSubType18 = mathSubType;
                baseStat.TSB11Unit18 = q1Unit;
                baseStat.TSB12Unit18 = q2Unit;
                baseStat.TSB13Unit18 = q3Unit;
                baseStat.TSB14Unit18 = q4Unit;
                baseStat.TSB15Unit18 = q5Unit;
                baseStat.TSB1TUnit18 = qTUnit;
                baseStat.TSB1TD1Unit18 = qTD1Unit;
                baseStat.TSB1TD2Unit18 = qTD2Unit;
                baseStat.TSB1TMUnit18 = qTMUnit;
                baseStat.TSB1TLUnit18 = qTLUnit;
                baseStat.TSB1TUUnit18 = qTUUnit;
            }
            if (i == 19)
            {
                baseStat.TSB1Name19 = name;
                baseStat.TSB1Label19 = label;
                baseStat.TSB1RelLabel19 = rellabel;
                baseStat.TSB1Description19 = description;
                baseStat.TSB1MathExpression19 = mathExpression;
                baseStat.TSB1Date19 = date;
                baseStat.TSB1Type19 = distType;
                baseStat.TSB1MathType19 = mathType;
                baseStat.TSB1BaseIO19 = baseIO;
                baseStat.TSB1MathOperator19 = mathOperator;
                baseStat.TSB1MathSubType19 = mathSubType;
                baseStat.TSB11Unit19 = q1Unit;
                baseStat.TSB12Unit19 = q2Unit;
                baseStat.TSB13Unit19 = q3Unit;
                baseStat.TSB14Unit19 = q4Unit;
                baseStat.TSB15Unit19 = q5Unit;
                baseStat.TSB1TUnit19 = qTUnit;
                baseStat.TSB1TD1Unit19 = qTD1Unit;
                baseStat.TSB1TD2Unit19 = qTD2Unit;
                baseStat.TSB1TMUnit19 = qTMUnit;
                baseStat.TSB1TLUnit19 = qTLUnit;
                baseStat.TSB1TUUnit19 = qTUUnit;
            }
            if (i == 20)
            {
                baseStat.TSB1Name20 = name;
                baseStat.TSB1Label20 = label;
                baseStat.TSB1RelLabel20 = rellabel;
                baseStat.TSB1Description20 = description;
                baseStat.TSB1MathExpression20 = mathExpression;
                baseStat.TSB1Date20 = date;
                baseStat.TSB1Type20 = distType;
                baseStat.TSB1MathType20 = mathType;
                baseStat.TSB1BaseIO20 = baseIO;
                baseStat.TSB1MathOperator20 = mathOperator;
                baseStat.TSB1MathSubType20 = mathSubType;
                baseStat.TSB11Unit20 = q1Unit;
                baseStat.TSB12Unit20 = q2Unit;
                baseStat.TSB13Unit20 = q3Unit;
                baseStat.TSB14Unit20 = q4Unit;
                baseStat.TSB15Unit20 = q5Unit;
                baseStat.TSB1TUnit20 = qTUnit;
                baseStat.TSB1TD1Unit20 = qTD1Unit;
                baseStat.TSB1TD2Unit20 = qTD2Unit;
                baseStat.TSB1TMUnit20 = qTMUnit;
                baseStat.TSB1TLUnit20 = qTLUnit;
                baseStat.TSB1TUUnit20 = qTUUnit;
            }
        }
        public static void AddSubStockToTotalStock(this TSB1BaseStock baseStat, int i, double n,
            double q1, double q2, double q3, double q4, double q5, double qT,
            double qTd1, double qTd2, double qTm, double qTl, double qTu)
        {
            //whatever calls this must multiply the qs before or after using ChangeSubTotalByMultipliers
            if (i == 1)
            {
                baseStat.TSB11Amount1 += q1;
                baseStat.TSB12Amount1 += q2;
                baseStat.TSB13Amount1 += q3;
                baseStat.TSB14Amount1 += q4;
                baseStat.TSB15Amount1 += q5;
                baseStat.TSB1TAmount1 += qT;
                baseStat.TSB1N1 += n;
                baseStat.TSB1TD1Amount1 += qTd1;
                baseStat.TSB1TD2Amount1 += qTd2;
                baseStat.TSB1TMAmount1 += qTm;
                baseStat.TSB1TLAmount1 += qTl;
                baseStat.TSB1TUAmount1 += qTu;
            }
            if (i == 2)
            {
                baseStat.TSB11Amount2 += q1;
                baseStat.TSB12Amount2 += q2;
                baseStat.TSB13Amount2 += q3;
                baseStat.TSB14Amount2 += q4;
                baseStat.TSB15Amount2 += q5;
                baseStat.TSB1TAmount2 += qT;
                baseStat.TSB1N2 += n;
                baseStat.TSB1TD1Amount2 += qTd1;
                baseStat.TSB1TD2Amount2 += qTd2;
                baseStat.TSB1TMAmount2 += qTm;
                baseStat.TSB1TLAmount2 += qTl;
                baseStat.TSB1TUAmount2 += qTu;
            }
            if (i == 3)
            {
                baseStat.TSB11Amount3 += q1;
                baseStat.TSB12Amount3 += q2;
                baseStat.TSB13Amount3 += q3;
                baseStat.TSB14Amount3 += q4;
                baseStat.TSB15Amount3 += q5;
                baseStat.TSB1TAmount3 += qT;
                baseStat.TSB1N3 += n;
                baseStat.TSB1TD1Amount3 += qTd1;
                baseStat.TSB1TD2Amount3 += qTd2;
                baseStat.TSB1TMAmount3 += qTm;
                baseStat.TSB1TLAmount3 += qTl;
                baseStat.TSB1TUAmount3 += qTu;
            }
            if (i == 4)
            {
                baseStat.TSB11Amount4 += q1;
                baseStat.TSB12Amount4 += q2;
                baseStat.TSB13Amount4 += q3;
                baseStat.TSB14Amount4 += q4;
                baseStat.TSB15Amount4 += q5;
                baseStat.TSB1TAmount4 += qT;
                baseStat.TSB1N4 += n;
                baseStat.TSB1TD1Amount4 += qTd1;
                baseStat.TSB1TD2Amount4 += qTd2;
                baseStat.TSB1TMAmount4 += qTm;
                baseStat.TSB1TLAmount4 += qTl;
                baseStat.TSB1TUAmount4 += qTu;
            }
            if (i == 5)
            {
                baseStat.TSB11Amount5 += q1;
                baseStat.TSB12Amount5 += q2;
                baseStat.TSB13Amount5 += q3;
                baseStat.TSB14Amount5 += q4;
                baseStat.TSB15Amount5 += q5;
                baseStat.TSB1TAmount5 += qT;
                baseStat.TSB1N5 += n;
                baseStat.TSB1TD1Amount5 += qTd1;
                baseStat.TSB1TD2Amount5 += qTd2;
                baseStat.TSB1TMAmount5 += qTm;
                baseStat.TSB1TLAmount5 += qTl;
                baseStat.TSB1TUAmount5 += qTu;
            }
            if (i == 6)
            {
                baseStat.TSB11Amount6 += q1;
                baseStat.TSB12Amount6 += q2;
                baseStat.TSB13Amount6 += q3;
                baseStat.TSB14Amount6 += q4;
                baseStat.TSB15Amount6 += q5;
                baseStat.TSB1TAmount6 += qT;
                baseStat.TSB1N6 += n;
                baseStat.TSB1TD1Amount6 += qTd1;
                baseStat.TSB1TD2Amount6 += qTd2;
                baseStat.TSB1TMAmount6 += qTm;
                baseStat.TSB1TLAmount6 += qTl;
                baseStat.TSB1TUAmount6 += qTu;
            }
            if (i == 7)
            {
                baseStat.TSB11Amount7 += q1;
                baseStat.TSB12Amount7 += q2;
                baseStat.TSB13Amount7 += q3;
                baseStat.TSB14Amount7 += q4;
                baseStat.TSB15Amount7 += q5;
                baseStat.TSB1TAmount7 += qT;
                baseStat.TSB1N7 += n;
                baseStat.TSB1TD1Amount7 += qTd1;
                baseStat.TSB1TD2Amount7 += qTd2;
                baseStat.TSB1TMAmount7 += qTm;
                baseStat.TSB1TLAmount7 += qTl;
                baseStat.TSB1TUAmount7 += qTu;
            }
            if (i == 8)
            {
                baseStat.TSB11Amount8 += q1;
                baseStat.TSB12Amount8 += q2;
                baseStat.TSB13Amount8 += q3;
                baseStat.TSB14Amount8 += q4;
                baseStat.TSB15Amount8 += q5;
                baseStat.TSB1TAmount8 += qT;
                baseStat.TSB1N8 += n;
                baseStat.TSB1TD1Amount8 += qTd1;
                baseStat.TSB1TD2Amount8 += qTd2;
                baseStat.TSB1TMAmount8 += qTm;
                baseStat.TSB1TLAmount8 += qTl;
                baseStat.TSB1TUAmount8 += qTu;
            }
            if (i == 9)
            {
                baseStat.TSB11Amount9 += q1;
                baseStat.TSB12Amount9 += q2;
                baseStat.TSB13Amount9 += q3;
                baseStat.TSB14Amount9 += q4;
                baseStat.TSB15Amount9 += q5;
                baseStat.TSB1TAmount9 += qT;
                baseStat.TSB1N9 += n;
                baseStat.TSB1TD1Amount9 += qTd1;
                baseStat.TSB1TD2Amount9 += qTd2;
                baseStat.TSB1TMAmount9 += qTm;
                baseStat.TSB1TLAmount9 += qTl;
                baseStat.TSB1TUAmount9 += qTu;
            }
            if (i == 10)
            {
                baseStat.TSB11Amount10 += q1;
                baseStat.TSB12Amount10 += q2;
                baseStat.TSB13Amount10 += q3;
                baseStat.TSB14Amount10 += q4;
                baseStat.TSB15Amount10 += q5;
                baseStat.TSB1TAmount10 += qT;
                baseStat.TSB1N10 += n;
                baseStat.TSB1TD1Amount10 += qTd1;
                baseStat.TSB1TD2Amount10 += qTd2;
                baseStat.TSB1TMAmount10 += qTm;
                baseStat.TSB1TLAmount10 += qTl;
                baseStat.TSB1TUAmount10 += qTu;
            }
            if (i == 11)
            {
                baseStat.TSB11Amount11 += q1;
                baseStat.TSB12Amount11 += q2;
                baseStat.TSB13Amount11 += q3;
                baseStat.TSB14Amount11 += q4;
                baseStat.TSB15Amount11 += q5;
                baseStat.TSB1TAmount11 += qT;
                baseStat.TSB1N11 += n;
                baseStat.TSB1TD1Amount11 += qTd1;
                baseStat.TSB1TD2Amount11 += qTd2;
                baseStat.TSB1TMAmount11 += qTm;
                baseStat.TSB1TLAmount11 += qTl;
                baseStat.TSB1TUAmount11 += qTu;
            }
            if (i == 12)
            {
                baseStat.TSB11Amount12 += q1;
                baseStat.TSB12Amount12 += q2;
                baseStat.TSB13Amount12 += q3;
                baseStat.TSB14Amount12 += q4;
                baseStat.TSB15Amount12 += q5;
                baseStat.TSB1TAmount12 += qT;
                baseStat.TSB1N12 += n;
                baseStat.TSB1TD1Amount12 += qTd1;
                baseStat.TSB1TD2Amount12 += qTd2;
                baseStat.TSB1TMAmount12 += qTm;
                baseStat.TSB1TLAmount12 += qTl;
                baseStat.TSB1TUAmount12 += qTu;
            }
            if (i == 13)
            {
                baseStat.TSB11Amount13 += q1;
                baseStat.TSB12Amount13 += q2;
                baseStat.TSB13Amount13 += q3;
                baseStat.TSB14Amount13 += q4;
                baseStat.TSB15Amount13 += q5;
                baseStat.TSB1TAmount13 += qT;
                baseStat.TSB1N13 += n;
                baseStat.TSB1TD1Amount13 += qTd1;
                baseStat.TSB1TD2Amount13 += qTd2;
                baseStat.TSB1TMAmount13 += qTm;
                baseStat.TSB1TLAmount13 += qTl;
                baseStat.TSB1TUAmount13 += qTu;
            }
            if (i == 14)
            {
                baseStat.TSB11Amount14 += q1;
                baseStat.TSB12Amount14 += q2;
                baseStat.TSB13Amount14 += q3;
                baseStat.TSB14Amount14 += q4;
                baseStat.TSB15Amount14 += q5;
                baseStat.TSB1TAmount14 += qT;
                baseStat.TSB1N14 += n;
                baseStat.TSB1TD1Amount14 += qTd1;
                baseStat.TSB1TD2Amount14 += qTd2;
                baseStat.TSB1TMAmount14 += qTm;
                baseStat.TSB1TLAmount14 += qTl;
                baseStat.TSB1TUAmount14 += qTu;
            }
            if (i == 15)
            {
                baseStat.TSB11Amount15 += q1;
                baseStat.TSB12Amount15 += q2;
                baseStat.TSB13Amount15 += q3;
                baseStat.TSB14Amount15 += q4;
                baseStat.TSB15Amount15 += q5;
                baseStat.TSB1TAmount15 += qT;
                baseStat.TSB1N15 += n;
                baseStat.TSB1TD1Amount15 += qTd1;
                baseStat.TSB1TD2Amount15 += qTd2;
                baseStat.TSB1TMAmount15 += qTm;
                baseStat.TSB1TLAmount15 += qTl;
                baseStat.TSB1TUAmount15 += qTu;
            }
            if (i == 16)
            {
                baseStat.TSB11Amount16 += q1;
                baseStat.TSB12Amount16 += q2;
                baseStat.TSB13Amount16 += q3;
                baseStat.TSB14Amount16 += q4;
                baseStat.TSB15Amount16 += q5;
                baseStat.TSB1TAmount16 += qT;
                baseStat.TSB1N16 += n;
                baseStat.TSB1TD1Amount16 += qTd1;
                baseStat.TSB1TD2Amount16 += qTd2;
                baseStat.TSB1TMAmount16 += qTm;
                baseStat.TSB1TLAmount16 += qTl;
                baseStat.TSB1TUAmount16 += qTu;
            }
            if (i == 17)
            {
                baseStat.TSB11Amount17 += q1;
                baseStat.TSB12Amount17 += q2;
                baseStat.TSB13Amount17 += q3;
                baseStat.TSB14Amount17 += q4;
                baseStat.TSB15Amount17 += q5;
                baseStat.TSB1TAmount17 += qT;
                baseStat.TSB1N17 += n;
                baseStat.TSB1TD1Amount17 += qTd1;
                baseStat.TSB1TD2Amount17 += qTd2;
                baseStat.TSB1TMAmount17 += qTm;
                baseStat.TSB1TLAmount17 += qTl;
                baseStat.TSB1TUAmount17 += qTu;
            }
            if (i == 18)
            {
                baseStat.TSB11Amount18 += q1;
                baseStat.TSB12Amount18 += q2;
                baseStat.TSB13Amount18 += q3;
                baseStat.TSB14Amount18 += q4;
                baseStat.TSB15Amount18 += q5;
                baseStat.TSB1TAmount18 += qT;
                baseStat.TSB1N18 += n;
                baseStat.TSB1TD1Amount18 += qTd1;
                baseStat.TSB1TD2Amount18 += qTd2;
                baseStat.TSB1TMAmount18 += qTm;
                baseStat.TSB1TLAmount18 += qTl;
                baseStat.TSB1TUAmount18 += qTu;
            }
            if (i == 19)
            {
                baseStat.TSB11Amount19 += q1;
                baseStat.TSB12Amount19 += q2;
                baseStat.TSB13Amount19 += q3;
                baseStat.TSB14Amount19 += q4;
                baseStat.TSB15Amount19 += q5;
                baseStat.TSB1TAmount19 += qT;
                baseStat.TSB1N19 += n;
                baseStat.TSB1TD1Amount19 += qTd1;
                baseStat.TSB1TD2Amount19 += qTd2;
                baseStat.TSB1TMAmount19 += qTm;
                baseStat.TSB1TLAmount19 += qTl;
                baseStat.TSB1TUAmount19 += qTu;
            }
            if (i == 20)
            {
                baseStat.TSB11Amount20 += q1;
                baseStat.TSB12Amount20 += q2;
                baseStat.TSB13Amount20 += q3;
                baseStat.TSB14Amount20 += q4;
                baseStat.TSB15Amount20 += q5;
                baseStat.TSB1TAmount20 += qT;
                baseStat.TSB1N20 += n;
                baseStat.TSB1TD1Amount20 += qTd1;
                baseStat.TSB1TD2Amount20 += qTd2;
                baseStat.TSB1TMAmount20 += qTm;
                baseStat.TSB1TLAmount20 += qTl;
                baseStat.TSB1TUAmount20 += qTu;
            }
        }
        public static void CopySubStockToTotalStock(this TSB1BaseStock baseStat, int i, double n,
            double q1, double q2, double q3, double q4, double q5, double qT,
            double qTd1, double qTd2, double qTm, double qTl, double qTu)
        {
            if (i == 1)
            {
                baseStat.TSB11Amount1 = q1;
                baseStat.TSB12Amount1 = q2;
                baseStat.TSB13Amount1 = q3;
                baseStat.TSB14Amount1 = q4;
                baseStat.TSB15Amount1 = q5;
                baseStat.TSB1TAmount1 = qT;
                baseStat.TSB1N1 = n;
                baseStat.TSB1TD1Amount1 = qTd1;
                baseStat.TSB1TD2Amount1 = qTd2;
                baseStat.TSB1TMAmount1 = qTm;
                baseStat.TSB1TLAmount1 = qTl;
                baseStat.TSB1TUAmount1 = qTu;
            }
            if (i == 2)
            {
                baseStat.TSB11Amount2 = q1;
                baseStat.TSB12Amount2 = q2;
                baseStat.TSB13Amount2 = q3;
                baseStat.TSB14Amount2 = q4;
                baseStat.TSB15Amount2 = q5;
                baseStat.TSB1TAmount2 = qT;
                baseStat.TSB1N2 = n;
                baseStat.TSB1TD1Amount2 = qTd1;
                baseStat.TSB1TD2Amount2 = qTd2;
                baseStat.TSB1TMAmount2 = qTm;
                baseStat.TSB1TLAmount2 = qTl;
                baseStat.TSB1TUAmount2 = qTu;
            }
            if (i == 3)
            {
                baseStat.TSB11Amount3 = q1;
                baseStat.TSB12Amount3 = q2;
                baseStat.TSB13Amount3 = q3;
                baseStat.TSB14Amount3 = q4;
                baseStat.TSB15Amount3 = q5;
                baseStat.TSB1TAmount3 = qT;
                baseStat.TSB1N3 = n;
                baseStat.TSB1TD1Amount3 = qTd1;
                baseStat.TSB1TD2Amount3 = qTd2;
                baseStat.TSB1TMAmount3 = qTm;
                baseStat.TSB1TLAmount3 = qTl;
                baseStat.TSB1TUAmount3 = qTu;
            }
            if (i == 4)
            {
                baseStat.TSB11Amount4 = q1;
                baseStat.TSB12Amount4 = q2;
                baseStat.TSB13Amount4 = q3;
                baseStat.TSB14Amount4 = q4;
                baseStat.TSB15Amount4 = q5;
                baseStat.TSB1TAmount4 = qT;
                baseStat.TSB1N4 = n;
                baseStat.TSB1TD1Amount4 = qTd1;
                baseStat.TSB1TD2Amount4 = qTd2;
                baseStat.TSB1TMAmount4 = qTm;
                baseStat.TSB1TLAmount4 = qTl;
                baseStat.TSB1TUAmount4 = qTu;
            }
            if (i == 5)
            {
                baseStat.TSB11Amount5 = q1;
                baseStat.TSB12Amount5 = q2;
                baseStat.TSB13Amount5 = q3;
                baseStat.TSB14Amount5 = q4;
                baseStat.TSB15Amount5 = q5;
                baseStat.TSB1TAmount5 = qT;
                baseStat.TSB1N5 = n;
                baseStat.TSB1TD1Amount5 = qTd1;
                baseStat.TSB1TD2Amount5 = qTd2;
                baseStat.TSB1TMAmount5 = qTm;
                baseStat.TSB1TLAmount5 = qTl;
                baseStat.TSB1TUAmount5 = qTu;
            }
            if (i == 6)
            {
                baseStat.TSB11Amount6 = q1;
                baseStat.TSB12Amount6 = q2;
                baseStat.TSB13Amount6 = q3;
                baseStat.TSB14Amount6 = q4;
                baseStat.TSB15Amount6 = q5;
                baseStat.TSB1TAmount6 = qT;
                baseStat.TSB1N6 = n;
                baseStat.TSB1TD1Amount6 = qTd1;
                baseStat.TSB1TD2Amount6 = qTd2;
                baseStat.TSB1TMAmount6 = qTm;
                baseStat.TSB1TLAmount6 = qTl;
                baseStat.TSB1TUAmount6 = qTu;
            }
            if (i == 7)
            {
                baseStat.TSB11Amount7 = q1;
                baseStat.TSB12Amount7 = q2;
                baseStat.TSB13Amount7 = q3;
                baseStat.TSB14Amount7 = q4;
                baseStat.TSB15Amount7 = q5;
                baseStat.TSB1TAmount7 = qT;
                baseStat.TSB1N7 = n;
                baseStat.TSB1TD1Amount7 = qTd1;
                baseStat.TSB1TD2Amount7 = qTd2;
                baseStat.TSB1TMAmount7 = qTm;
                baseStat.TSB1TLAmount7 = qTl;
                baseStat.TSB1TUAmount7 = qTu;
            }
            if (i == 8)
            {
                baseStat.TSB11Amount8 = q1;
                baseStat.TSB12Amount8 = q2;
                baseStat.TSB13Amount8 = q3;
                baseStat.TSB14Amount8 = q4;
                baseStat.TSB15Amount8 = q5;
                baseStat.TSB1TAmount8 = qT;
                baseStat.TSB1N8 = n;
                baseStat.TSB1TD1Amount8 = qTd1;
                baseStat.TSB1TD2Amount8 = qTd2;
                baseStat.TSB1TMAmount8 = qTm;
                baseStat.TSB1TLAmount8 = qTl;
                baseStat.TSB1TUAmount8 = qTu;
            }
            if (i == 9)
            {
                baseStat.TSB11Amount9 = q1;
                baseStat.TSB12Amount9 = q2;
                baseStat.TSB13Amount9 = q3;
                baseStat.TSB14Amount9 = q4;
                baseStat.TSB15Amount9 = q5;
                baseStat.TSB1TAmount9 = qT;
                baseStat.TSB1N9 = n;
                baseStat.TSB1TD1Amount9 = qTd1;
                baseStat.TSB1TD2Amount9 = qTd2;
                baseStat.TSB1TMAmount9 = qTm;
                baseStat.TSB1TLAmount9 = qTl;
                baseStat.TSB1TUAmount9 = qTu;
            }
            if (i == 10)
            {
                baseStat.TSB11Amount10 = q1;
                baseStat.TSB12Amount10 = q2;
                baseStat.TSB13Amount10 = q3;
                baseStat.TSB14Amount10 = q4;
                baseStat.TSB15Amount10 = q5;
                baseStat.TSB1TAmount10 = qT;
                baseStat.TSB1N10 = n;
                baseStat.TSB1TD1Amount10 = qTd1;
                baseStat.TSB1TD2Amount10 = qTd2;
                baseStat.TSB1TMAmount10 = qTm;
                baseStat.TSB1TLAmount10 = qTl;
                baseStat.TSB1TUAmount10 = qTu;
            }
            if (i == 11)
            {
                baseStat.TSB11Amount11 = q1;
                baseStat.TSB12Amount11 = q2;
                baseStat.TSB13Amount11 = q3;
                baseStat.TSB14Amount11 = q4;
                baseStat.TSB15Amount11 = q5;
                baseStat.TSB1TAmount11 = qT;
                baseStat.TSB1N11 = n;
                baseStat.TSB1TD1Amount11 = qTd1;
                baseStat.TSB1TD2Amount11 = qTd2;
                baseStat.TSB1TMAmount11 = qTm;
                baseStat.TSB1TLAmount11 = qTl;
                baseStat.TSB1TUAmount11 = qTu;
            }
            if (i == 12)
            {
                baseStat.TSB11Amount12 = q1;
                baseStat.TSB12Amount12 = q2;
                baseStat.TSB13Amount12 = q3;
                baseStat.TSB14Amount12 = q4;
                baseStat.TSB15Amount12 = q5;
                baseStat.TSB1TAmount12 = qT;
                baseStat.TSB1N12 = n;
                baseStat.TSB1TD1Amount12 = qTd1;
                baseStat.TSB1TD2Amount12 = qTd2;
                baseStat.TSB1TMAmount12 = qTm;
                baseStat.TSB1TLAmount12 = qTl;
                baseStat.TSB1TUAmount12 = qTu;
            }
            if (i == 13)
            {
                baseStat.TSB11Amount13 = q1;
                baseStat.TSB12Amount13 = q2;
                baseStat.TSB13Amount13 = q3;
                baseStat.TSB14Amount13 = q4;
                baseStat.TSB15Amount13 = q5;
                baseStat.TSB1TAmount13 = qT;
                baseStat.TSB1N13 = n;
                baseStat.TSB1TD1Amount13 = qTd1;
                baseStat.TSB1TD2Amount13 = qTd2;
                baseStat.TSB1TMAmount13 = qTm;
                baseStat.TSB1TLAmount13 = qTl;
                baseStat.TSB1TUAmount13 = qTu;
            }
            if (i == 14)
            {
                baseStat.TSB11Amount14 = q1;
                baseStat.TSB12Amount14 = q2;
                baseStat.TSB13Amount14 = q3;
                baseStat.TSB14Amount14 = q4;
                baseStat.TSB15Amount14 = q5;
                baseStat.TSB1TAmount14 = qT;
                baseStat.TSB1N14 = n;
                baseStat.TSB1TD1Amount14 = qTd1;
                baseStat.TSB1TD2Amount14 = qTd2;
                baseStat.TSB1TMAmount14 = qTm;
                baseStat.TSB1TLAmount14 = qTl;
                baseStat.TSB1TUAmount14 = qTu;
            }
            if (i == 15)
            {
                baseStat.TSB11Amount15 = q1;
                baseStat.TSB12Amount15 = q2;
                baseStat.TSB13Amount15 = q3;
                baseStat.TSB14Amount15 = q4;
                baseStat.TSB15Amount15 = q5;
                baseStat.TSB1TAmount15 = qT;
                baseStat.TSB1N15 = n;
                baseStat.TSB1TD1Amount15 = qTd1;
                baseStat.TSB1TD2Amount15 = qTd2;
                baseStat.TSB1TMAmount15 = qTm;
                baseStat.TSB1TLAmount15 = qTl;
                baseStat.TSB1TUAmount15 = qTu;
            }
            if (i == 16)
            {
                baseStat.TSB11Amount16 = q1;
                baseStat.TSB12Amount16 = q2;
                baseStat.TSB13Amount16 = q3;
                baseStat.TSB14Amount16 = q4;
                baseStat.TSB15Amount16 = q5;
                baseStat.TSB1TAmount16 = qT;
                baseStat.TSB1N16 = n;
                baseStat.TSB1TD1Amount16 = qTd1;
                baseStat.TSB1TD2Amount16 = qTd2;
                baseStat.TSB1TMAmount16 = qTm;
                baseStat.TSB1TLAmount16 = qTl;
                baseStat.TSB1TUAmount16 = qTu;
            }
            if (i == 17)
            {
                baseStat.TSB11Amount17 = q1;
                baseStat.TSB12Amount17 = q2;
                baseStat.TSB13Amount17 = q3;
                baseStat.TSB14Amount17 = q4;
                baseStat.TSB15Amount17 = q5;
                baseStat.TSB1TAmount17 = qT;
                baseStat.TSB1N17 = n;
                baseStat.TSB1TD1Amount17 = qTd1;
                baseStat.TSB1TD2Amount17 = qTd2;
                baseStat.TSB1TMAmount17 = qTm;
                baseStat.TSB1TLAmount17 = qTl;
                baseStat.TSB1TUAmount17 = qTu;
            }
            if (i == 18)
            {
                baseStat.TSB11Amount18 = q1;
                baseStat.TSB12Amount18 = q2;
                baseStat.TSB13Amount18 = q3;
                baseStat.TSB14Amount18 = q4;
                baseStat.TSB15Amount18 = q5;
                baseStat.TSB1TAmount18 = qT;
                baseStat.TSB1N18 = n;
                baseStat.TSB1TD1Amount18 = qTd1;
                baseStat.TSB1TD2Amount18 = qTd2;
                baseStat.TSB1TMAmount18 = qTm;
                baseStat.TSB1TLAmount18 = qTl;
                baseStat.TSB1TUAmount18 = qTu;
            }
            if (i == 19)
            {
                baseStat.TSB11Amount19 = q1;
                baseStat.TSB12Amount19 = q2;
                baseStat.TSB13Amount19 = q3;
                baseStat.TSB14Amount19 = q4;
                baseStat.TSB15Amount19 = q5;
                baseStat.TSB1TAmount19 = qT;
                baseStat.TSB1N19 = n;
                baseStat.TSB1TD1Amount19 = qTd1;
                baseStat.TSB1TD2Amount19 = qTd2;
                baseStat.TSB1TMAmount19 = qTm;
                baseStat.TSB1TLAmount19 = qTl;
                baseStat.TSB1TUAmount19 = qTu;
            }
            if (i == 20)
            {
                baseStat.TSB11Amount20 = q1;
                baseStat.TSB12Amount20 = q2;
                baseStat.TSB13Amount20 = q3;
                baseStat.TSB14Amount20 = q4;
                baseStat.TSB15Amount20 = q5;
                baseStat.TSB1TAmount20 = qT;
                baseStat.TSB1N20 = n;
                baseStat.TSB1TD1Amount20 = qTd1;
                baseStat.TSB1TD2Amount20 = qTd2;
                baseStat.TSB1TMAmount20 = qTm;
                baseStat.TSB1TLAmount20 = qTl;
                baseStat.TSB1TUAmount20 = qTu;
            }
        }
        public static void AddSubStockScoreToTotalStock(this TSB1BaseStock baseStat, TSB1BaseStock subTotal)
        {
            //SB1Stock.AddStockScore uses only a subset of the total props in the Stats, Change, and Progress 
            baseStat.TSB1Score += subTotal.TSB1Score;
            baseStat.TSB1ScoreUnit = subTotal.TSB1ScoreUnit;
            baseStat.TSB1ScoreD1Amount += subTotal.TSB1ScoreD1Amount;
            baseStat.TSB1ScoreD1Unit = subTotal.TSB1ScoreD1Unit;
            baseStat.TSB1ScoreD2Amount += subTotal.TSB1ScoreD2Amount;
            baseStat.TSB1ScoreD2Unit = subTotal.TSB1ScoreD2Unit;
            baseStat.TSB1ScoreM += subTotal.TSB1ScoreM;
            baseStat.TSB1ScoreMUnit = subTotal.TSB1ScoreMUnit;
            baseStat.TSB1ScoreLAmount += subTotal.TSB1ScoreLAmount;
            baseStat.TSB1ScoreLUnit = subTotal.TSB1ScoreLUnit;
            baseStat.TSB1ScoreUAmount += subTotal.TSB1ScoreUAmount;
            baseStat.TSB1ScoreUUnit = subTotal.TSB1ScoreUUnit;
            baseStat.TSB1Iterations = subTotal.TSB1Iterations;
            baseStat.TSB1ScoreN += subTotal.TSB1ScoreN;
            //somewhat misleading when different els use different techniques but useful to see representative props
            baseStat.TSB1ScoreDistType = subTotal.TSB1ScoreDistType;
            baseStat.TSB1ScoreMathType = subTotal.TSB1ScoreMathType;
            baseStat.TSB1ScoreMathSubType = subTotal.TSB1ScoreMathSubType;
            baseStat.TSB1ScoreMathExpression = subTotal.TSB1ScoreMathExpression;
            baseStat.TSB1ScoreMathResult = subTotal.TSB1ScoreMathResult;
        }
        public static void CopySubStockScoreToTotalStock(this TSB1BaseStock baseStat, TSB1BaseStock subTotal)
        {
            baseStat.TSB1Score = subTotal.TSB1Score;
            baseStat.TSB1ScoreUnit = subTotal.TSB1ScoreUnit;
            baseStat.TSB1ScoreD1Amount = subTotal.TSB1ScoreD1Amount;
            baseStat.TSB1ScoreD1Unit = subTotal.TSB1ScoreD1Unit;
            baseStat.TSB1ScoreD2Amount = subTotal.TSB1ScoreD2Amount;
            baseStat.TSB1ScoreD2Unit = subTotal.TSB1ScoreD2Unit;
            baseStat.TSB1ScoreM = subTotal.TSB1ScoreM;
            baseStat.TSB1ScoreMUnit = subTotal.TSB1ScoreMUnit;
            baseStat.TSB1ScoreLAmount = subTotal.TSB1ScoreLAmount;
            baseStat.TSB1ScoreLUnit = subTotal.TSB1ScoreLUnit;
            baseStat.TSB1ScoreUAmount = subTotal.TSB1ScoreUAmount;
            baseStat.TSB1ScoreUUnit = subTotal.TSB1ScoreUUnit;
            baseStat.TSB1Iterations = subTotal.TSB1Iterations;
            baseStat.TSB1ScoreN = subTotal.TSB1ScoreN;
            baseStat.TSB1ScoreDistType = subTotal.TSB1ScoreDistType;
            baseStat.TSB1ScoreMathType = subTotal.TSB1ScoreMathType;
            baseStat.TSB1ScoreMathSubType = subTotal.TSB1ScoreMathSubType;
            baseStat.TSB1ScoreMathExpression = subTotal.TSB1ScoreMathExpression;
            baseStat.TSB1ScoreMathResult = subTotal.TSB1ScoreMathResult;
        }
        public static void AddSubStockScoreToTotalStock(this TSB1BaseStock baseStat, double n, double score, string scoreUnit,
            double scoreD1, string scoreD1Unit, double scoreD2, string scoreD2Unit,
            double scoreM, string scoreMUnit, double scoreL, string scoreLUnit, double scoreU, string scoreUUnit, 
            string scoreMathExpress, string scoreDistType, string scoreMathType, string scoreMathSubType, 
            string scoreMathResult, int iterations)
        {
            baseStat.TSB1Score += score;
            baseStat.TSB1ScoreUnit = scoreUnit;
            baseStat.TSB1ScoreD1Amount += scoreD1;
            baseStat.TSB1ScoreD1Unit = scoreD1Unit;
            baseStat.TSB1ScoreD2Amount += scoreD2;
            baseStat.TSB1ScoreD2Unit = scoreD2Unit;
            baseStat.TSB1ScoreM += scoreM;
            baseStat.TSB1ScoreMUnit = scoreMUnit;
            baseStat.TSB1ScoreLAmount += scoreL;
            baseStat.TSB1ScoreLUnit = scoreLUnit;
            baseStat.TSB1ScoreUAmount += scoreU;
            baseStat.TSB1ScoreUUnit = scoreUUnit;
            baseStat.TSB1Iterations = iterations;
            baseStat.TSB1ScoreN += n;
            //somewhat misleading when different els use different techniques but useful to see representative props
            baseStat.TSB1ScoreMathExpression = scoreMathExpress;
            baseStat.TSB1ScoreDistType = scoreDistType;
            baseStat.TSB1ScoreMathType = scoreMathType;
            baseStat.TSB1ScoreMathSubType = scoreMathSubType;
            baseStat.TSB1ScoreMathResult = scoreMathResult;
        }
        public static void ChangeSubTotalByMultipliers(this TSB1BaseStock subTotal, double multiplier)
        {
            //multiplier adjusted indicators
            subTotal.TSB1Score = subTotal.TSB1Score * multiplier;
            subTotal.TSB1ScoreD1Amount = subTotal.TSB1ScoreD1Amount * multiplier;
            subTotal.TSB1ScoreD2Amount = subTotal.TSB1ScoreD2Amount * multiplier;
            subTotal.TSB1ScoreM = subTotal.TSB1ScoreM * multiplier;
            subTotal.TSB1ScoreLAmount = subTotal.TSB1ScoreLAmount * multiplier;
            subTotal.TSB1ScoreUAmount = subTotal.TSB1ScoreUAmount * multiplier;
            //keep the same pattern as the additions and subtractions above
            //KISS and let analyst interpret the data
            subTotal.TSB15Amount1 = subTotal.TSB15Amount1 * multiplier;
            subTotal.TSB11Amount1 = subTotal.TSB11Amount1 * multiplier;
            subTotal.TSB12Amount1 = subTotal.TSB12Amount1 * multiplier;
            subTotal.TSB13Amount1 = subTotal.TSB13Amount1 * multiplier;
            subTotal.TSB14Amount1 = subTotal.TSB14Amount1 * multiplier;
            subTotal.TSB1TAmount1 = subTotal.TSB1TAmount1 * multiplier;
            subTotal.TSB1TD1Amount1 = subTotal.TSB1TD1Amount1 * multiplier;
            subTotal.TSB1TD2Amount1 = subTotal.TSB1TD2Amount1 * multiplier;
            subTotal.TSB1TMAmount1 = subTotal.TSB1TMAmount1 * multiplier;
            subTotal.TSB1TLAmount1 = subTotal.TSB1TLAmount1 * multiplier;
            subTotal.TSB1TUAmount1 = subTotal.TSB1TUAmount1 * multiplier;

            subTotal.TSB15Amount2 = subTotal.TSB15Amount2 * multiplier;
            subTotal.TSB11Amount2 = subTotal.TSB11Amount2 * multiplier;
            subTotal.TSB12Amount2 = subTotal.TSB12Amount2 * multiplier;
            subTotal.TSB13Amount2 = subTotal.TSB13Amount2 * multiplier;
            subTotal.TSB14Amount2 = subTotal.TSB14Amount2 * multiplier;
            subTotal.TSB1TAmount2 = subTotal.TSB1TAmount2 * multiplier;
            subTotal.TSB1TD1Amount2 = subTotal.TSB1TD1Amount2 * multiplier;
            subTotal.TSB1TD2Amount2 = subTotal.TSB1TD2Amount2 * multiplier;
            subTotal.TSB1TMAmount2 = subTotal.TSB1TMAmount2 * multiplier;
            subTotal.TSB1TLAmount2 = subTotal.TSB1TLAmount2 * multiplier;
            subTotal.TSB1TUAmount2 = subTotal.TSB1TUAmount2 * multiplier;

            subTotal.TSB15Amount3 = subTotal.TSB15Amount3 * multiplier;
            subTotal.TSB11Amount3 = subTotal.TSB11Amount3 * multiplier;
            subTotal.TSB12Amount3 = subTotal.TSB12Amount3 * multiplier;
            subTotal.TSB13Amount3 = subTotal.TSB13Amount3 * multiplier;
            subTotal.TSB14Amount3 = subTotal.TSB14Amount3 * multiplier;
            subTotal.TSB1TAmount3 = subTotal.TSB1TAmount3 * multiplier;
            subTotal.TSB1TD1Amount3 = subTotal.TSB1TD1Amount3 * multiplier;
            subTotal.TSB1TD2Amount3 = subTotal.TSB1TD2Amount3 * multiplier;
            subTotal.TSB1TMAmount3 = subTotal.TSB1TMAmount3 * multiplier;
            subTotal.TSB1TLAmount3 = subTotal.TSB1TLAmount3 * multiplier;
            subTotal.TSB1TUAmount3 = subTotal.TSB1TUAmount3 * multiplier;

            subTotal.TSB15Amount4 = subTotal.TSB15Amount4 * multiplier;
            subTotal.TSB11Amount4 = subTotal.TSB11Amount4 * multiplier;
            subTotal.TSB12Amount4 = subTotal.TSB12Amount4 * multiplier;
            subTotal.TSB13Amount4 = subTotal.TSB13Amount4 * multiplier;
            subTotal.TSB14Amount4 = subTotal.TSB14Amount4 * multiplier;
            subTotal.TSB1TAmount4 = subTotal.TSB1TAmount4 * multiplier;
            subTotal.TSB1TD1Amount4 = subTotal.TSB1TD1Amount4 * multiplier;
            subTotal.TSB1TD2Amount4 = subTotal.TSB1TD2Amount4 * multiplier;
            subTotal.TSB1TMAmount4 = subTotal.TSB1TMAmount4 * multiplier;
            subTotal.TSB1TLAmount4 = subTotal.TSB1TLAmount4 * multiplier;
            subTotal.TSB1TUAmount4 = subTotal.TSB1TUAmount4 * multiplier;

            subTotal.TSB15Amount5 = subTotal.TSB15Amount5 * multiplier;
            subTotal.TSB11Amount5 = subTotal.TSB11Amount5 * multiplier;
            subTotal.TSB12Amount5 = subTotal.TSB12Amount5 * multiplier;
            subTotal.TSB13Amount5 = subTotal.TSB13Amount5 * multiplier;
            subTotal.TSB14Amount5 = subTotal.TSB14Amount5 * multiplier;
            subTotal.TSB1TAmount5 = subTotal.TSB1TAmount5 * multiplier;
            subTotal.TSB1TD1Amount5 = subTotal.TSB1TD1Amount5 * multiplier;
            subTotal.TSB1TD2Amount5 = subTotal.TSB1TD2Amount5 * multiplier;
            subTotal.TSB1TMAmount5 = subTotal.TSB1TMAmount5 * multiplier;
            subTotal.TSB1TLAmount5 = subTotal.TSB1TLAmount5 * multiplier;
            subTotal.TSB1TUAmount5 = subTotal.TSB1TUAmount5 * multiplier;

            subTotal.TSB15Amount6 = subTotal.TSB15Amount6 * multiplier;
            subTotal.TSB11Amount6 = subTotal.TSB11Amount6 * multiplier;
            subTotal.TSB12Amount6 = subTotal.TSB12Amount6 * multiplier;
            subTotal.TSB13Amount6 = subTotal.TSB13Amount6 * multiplier;
            subTotal.TSB14Amount6 = subTotal.TSB14Amount6 * multiplier;
            subTotal.TSB1TAmount6 = subTotal.TSB1TAmount6 * multiplier;
            subTotal.TSB1TD1Amount6 = subTotal.TSB1TD1Amount6 * multiplier;
            subTotal.TSB1TD2Amount6 = subTotal.TSB1TD2Amount6 * multiplier;
            subTotal.TSB1TMAmount6 = subTotal.TSB1TMAmount6 * multiplier;
            subTotal.TSB1TLAmount6 = subTotal.TSB1TLAmount6 * multiplier;
            subTotal.TSB1TUAmount6 = subTotal.TSB1TUAmount6 * multiplier;

            subTotal.TSB15Amount7 = subTotal.TSB15Amount7 * multiplier;
            subTotal.TSB11Amount7 = subTotal.TSB11Amount7 * multiplier;
            subTotal.TSB12Amount7 = subTotal.TSB12Amount7 * multiplier;
            subTotal.TSB13Amount7 = subTotal.TSB13Amount7 * multiplier;
            subTotal.TSB14Amount7 = subTotal.TSB14Amount7 * multiplier;
            subTotal.TSB1TAmount7 = subTotal.TSB1TAmount7 * multiplier;
            subTotal.TSB1TD1Amount7 = subTotal.TSB1TD1Amount7 * multiplier;
            subTotal.TSB1TD2Amount7 = subTotal.TSB1TD2Amount7 * multiplier;
            subTotal.TSB1TMAmount7 = subTotal.TSB1TMAmount7 * multiplier;
            subTotal.TSB1TLAmount7 = subTotal.TSB1TLAmount7 * multiplier;
            subTotal.TSB1TUAmount7 = subTotal.TSB1TUAmount7 * multiplier;

            subTotal.TSB15Amount8 = subTotal.TSB15Amount8 * multiplier;
            subTotal.TSB11Amount8 = subTotal.TSB11Amount8 * multiplier;
            subTotal.TSB12Amount8 = subTotal.TSB12Amount8 * multiplier;
            subTotal.TSB13Amount8 = subTotal.TSB13Amount8 * multiplier;
            subTotal.TSB14Amount8 = subTotal.TSB14Amount8 * multiplier;
            subTotal.TSB1TAmount8 = subTotal.TSB1TAmount8 * multiplier;
            subTotal.TSB1TD1Amount8 = subTotal.TSB1TD1Amount8 * multiplier;
            subTotal.TSB1TD2Amount8 = subTotal.TSB1TD2Amount8 * multiplier;
            subTotal.TSB1TMAmount8 = subTotal.TSB1TMAmount8 * multiplier;
            subTotal.TSB1TLAmount8 = subTotal.TSB1TLAmount8 * multiplier;
            subTotal.TSB1TUAmount8 = subTotal.TSB1TUAmount8 * multiplier;

            subTotal.TSB15Amount9 = subTotal.TSB15Amount9 * multiplier;
            subTotal.TSB11Amount9 = subTotal.TSB11Amount9 * multiplier;
            subTotal.TSB12Amount9 = subTotal.TSB12Amount9 * multiplier;
            subTotal.TSB13Amount9 = subTotal.TSB13Amount9 * multiplier;
            subTotal.TSB14Amount9 = subTotal.TSB14Amount9 * multiplier;
            subTotal.TSB1TAmount9 = subTotal.TSB1TAmount9 * multiplier;
            subTotal.TSB1TD1Amount9 = subTotal.TSB1TD1Amount9 * multiplier;
            subTotal.TSB1TD2Amount9 = subTotal.TSB1TD2Amount9 * multiplier;
            subTotal.TSB1TMAmount9 = subTotal.TSB1TMAmount9 * multiplier;
            subTotal.TSB1TLAmount9 = subTotal.TSB1TLAmount9 * multiplier;
            subTotal.TSB1TUAmount9 = subTotal.TSB1TUAmount9 * multiplier;

            subTotal.TSB15Amount10 = subTotal.TSB15Amount10 * multiplier;
            subTotal.TSB11Amount10 = subTotal.TSB11Amount10 * multiplier;
            subTotal.TSB12Amount10 = subTotal.TSB12Amount10 * multiplier;
            subTotal.TSB13Amount10 = subTotal.TSB13Amount10 * multiplier;
            subTotal.TSB14Amount10 = subTotal.TSB14Amount10 * multiplier;
            subTotal.TSB1TAmount10 = subTotal.TSB1TAmount10 * multiplier;
            subTotal.TSB1TD1Amount10 = subTotal.TSB1TD1Amount10 * multiplier;
            subTotal.TSB1TD2Amount10 = subTotal.TSB1TD2Amount10 * multiplier;
            subTotal.TSB1TMAmount10 = subTotal.TSB1TMAmount10 * multiplier;
            subTotal.TSB1TLAmount10 = subTotal.TSB1TLAmount10 * multiplier;
            subTotal.TSB1TUAmount10 = subTotal.TSB1TUAmount10 * multiplier;

            subTotal.TSB15Amount11 = subTotal.TSB15Amount11 * multiplier;
            subTotal.TSB11Amount11 = subTotal.TSB11Amount11 * multiplier;
            subTotal.TSB12Amount11 = subTotal.TSB12Amount11 * multiplier;
            subTotal.TSB13Amount11 = subTotal.TSB13Amount11 * multiplier;
            subTotal.TSB14Amount11 = subTotal.TSB14Amount11 * multiplier;
            subTotal.TSB1TAmount11 = subTotal.TSB1TAmount11 * multiplier;
            subTotal.TSB1TD1Amount11 = subTotal.TSB1TD1Amount11 * multiplier;
            subTotal.TSB1TD2Amount11 = subTotal.TSB1TD2Amount11 * multiplier;
            subTotal.TSB1TMAmount11 = subTotal.TSB1TMAmount11 * multiplier;
            subTotal.TSB1TLAmount11 = subTotal.TSB1TLAmount11 * multiplier;
            subTotal.TSB1TUAmount11 = subTotal.TSB1TUAmount11 * multiplier;

            subTotal.TSB15Amount12 = subTotal.TSB15Amount12 * multiplier;
            subTotal.TSB11Amount12 = subTotal.TSB11Amount12 * multiplier;
            subTotal.TSB12Amount12 = subTotal.TSB12Amount12 * multiplier;
            subTotal.TSB13Amount12 = subTotal.TSB13Amount12 * multiplier;
            subTotal.TSB14Amount12 = subTotal.TSB14Amount12 * multiplier;
            subTotal.TSB1TAmount12 = subTotal.TSB1TAmount12 * multiplier;
            subTotal.TSB1TD1Amount12 = subTotal.TSB1TD1Amount12 * multiplier;
            subTotal.TSB1TD2Amount12 = subTotal.TSB1TD2Amount12 * multiplier;
            subTotal.TSB1TMAmount12 = subTotal.TSB1TMAmount12 * multiplier;
            subTotal.TSB1TLAmount12 = subTotal.TSB1TLAmount12 * multiplier;
            subTotal.TSB1TUAmount12 = subTotal.TSB1TUAmount12 * multiplier;

            subTotal.TSB15Amount13 = subTotal.TSB15Amount13 * multiplier;
            subTotal.TSB11Amount13 = subTotal.TSB11Amount13 * multiplier;
            subTotal.TSB12Amount13 = subTotal.TSB12Amount13 * multiplier;
            subTotal.TSB13Amount13 = subTotal.TSB13Amount13 * multiplier;
            subTotal.TSB14Amount13 = subTotal.TSB14Amount13 * multiplier;
            subTotal.TSB1TAmount13 = subTotal.TSB1TAmount13 * multiplier;
            subTotal.TSB1TD1Amount13 = subTotal.TSB1TD1Amount13 * multiplier;
            subTotal.TSB1TD2Amount13 = subTotal.TSB1TD2Amount13 * multiplier;
            subTotal.TSB1TMAmount13 = subTotal.TSB1TMAmount13 * multiplier;
            subTotal.TSB1TLAmount13 = subTotal.TSB1TLAmount13 * multiplier;
            subTotal.TSB1TUAmount13 = subTotal.TSB1TUAmount13 * multiplier;

            subTotal.TSB15Amount14 = subTotal.TSB15Amount14 * multiplier;
            subTotal.TSB11Amount14 = subTotal.TSB11Amount14 * multiplier;
            subTotal.TSB12Amount14 = subTotal.TSB12Amount14 * multiplier;
            subTotal.TSB13Amount14 = subTotal.TSB13Amount14 * multiplier;
            subTotal.TSB14Amount14 = subTotal.TSB14Amount14 * multiplier;
            subTotal.TSB1TAmount14 = subTotal.TSB1TAmount14 * multiplier;
            subTotal.TSB1TD1Amount14 = subTotal.TSB1TD1Amount14 * multiplier;
            subTotal.TSB1TD2Amount14 = subTotal.TSB1TD2Amount14 * multiplier;
            subTotal.TSB1TMAmount14 = subTotal.TSB1TMAmount14 * multiplier;
            subTotal.TSB1TLAmount14 = subTotal.TSB1TLAmount14 * multiplier;
            subTotal.TSB1TUAmount14 = subTotal.TSB1TUAmount14 * multiplier;

            subTotal.TSB15Amount15 = subTotal.TSB15Amount15 * multiplier;
            subTotal.TSB11Amount15 = subTotal.TSB11Amount15 * multiplier;
            subTotal.TSB12Amount15 = subTotal.TSB12Amount15 * multiplier;
            subTotal.TSB13Amount15 = subTotal.TSB13Amount15 * multiplier;
            subTotal.TSB14Amount15 = subTotal.TSB14Amount15 * multiplier;
            subTotal.TSB1TAmount15 = subTotal.TSB1TAmount15 * multiplier;
            subTotal.TSB1TD1Amount15 = subTotal.TSB1TD1Amount15 * multiplier;
            subTotal.TSB1TD2Amount15 = subTotal.TSB1TD2Amount15 * multiplier;
            subTotal.TSB1TMAmount15 = subTotal.TSB1TMAmount15 * multiplier;
            subTotal.TSB1TLAmount15 = subTotal.TSB1TLAmount15 * multiplier;
            subTotal.TSB1TUAmount15 = subTotal.TSB1TUAmount15 * multiplier;

            subTotal.TSB15Amount16 = subTotal.TSB15Amount16 * multiplier;
            subTotal.TSB11Amount16 = subTotal.TSB11Amount16 * multiplier;
            subTotal.TSB12Amount16 = subTotal.TSB12Amount16 * multiplier;
            subTotal.TSB13Amount16 = subTotal.TSB13Amount16 * multiplier;
            subTotal.TSB14Amount16 = subTotal.TSB14Amount16 * multiplier;
            subTotal.TSB1TAmount16 = subTotal.TSB1TAmount16 * multiplier;
            subTotal.TSB1TD1Amount16 = subTotal.TSB1TD1Amount16 * multiplier;
            subTotal.TSB1TD2Amount16 = subTotal.TSB1TD2Amount16 * multiplier;
            subTotal.TSB1TMAmount16 = subTotal.TSB1TMAmount16 * multiplier;
            subTotal.TSB1TLAmount16 = subTotal.TSB1TLAmount16 * multiplier;
            subTotal.TSB1TUAmount16 = subTotal.TSB1TUAmount16 * multiplier;

            subTotal.TSB15Amount17 = subTotal.TSB15Amount17 * multiplier;
            subTotal.TSB11Amount17 = subTotal.TSB11Amount17 * multiplier;
            subTotal.TSB12Amount17 = subTotal.TSB12Amount17 * multiplier;
            subTotal.TSB13Amount17 = subTotal.TSB13Amount17 * multiplier;
            subTotal.TSB14Amount17 = subTotal.TSB14Amount17 * multiplier;
            subTotal.TSB1TAmount17 = subTotal.TSB1TAmount17 * multiplier;
            subTotal.TSB1TD1Amount17 = subTotal.TSB1TD1Amount17 * multiplier;
            subTotal.TSB1TD2Amount17 = subTotal.TSB1TD2Amount17 * multiplier;
            subTotal.TSB1TMAmount17 = subTotal.TSB1TMAmount17 * multiplier;
            subTotal.TSB1TLAmount17 = subTotal.TSB1TLAmount17 * multiplier;
            subTotal.TSB1TUAmount17 = subTotal.TSB1TUAmount17 * multiplier;

            subTotal.TSB15Amount18 = subTotal.TSB15Amount18 * multiplier;
            subTotal.TSB11Amount18 = subTotal.TSB11Amount18 * multiplier;
            subTotal.TSB12Amount18 = subTotal.TSB12Amount18 * multiplier;
            subTotal.TSB13Amount18 = subTotal.TSB13Amount18 * multiplier;
            subTotal.TSB14Amount18 = subTotal.TSB14Amount18 * multiplier;
            subTotal.TSB1TAmount18 = subTotal.TSB1TAmount18 * multiplier;
            subTotal.TSB1TD1Amount18 = subTotal.TSB1TD1Amount18 * multiplier;
            subTotal.TSB1TD2Amount18 = subTotal.TSB1TD2Amount18 * multiplier;
            subTotal.TSB1TMAmount18 = subTotal.TSB1TMAmount18 * multiplier;
            subTotal.TSB1TLAmount18 = subTotal.TSB1TLAmount18 * multiplier;
            subTotal.TSB1TUAmount18 = subTotal.TSB1TUAmount18 * multiplier;

            subTotal.TSB15Amount19 = subTotal.TSB15Amount19 * multiplier;
            subTotal.TSB11Amount19 = subTotal.TSB11Amount19 * multiplier;
            subTotal.TSB12Amount19 = subTotal.TSB12Amount19 * multiplier;
            subTotal.TSB13Amount19 = subTotal.TSB13Amount19 * multiplier;
            subTotal.TSB14Amount19 = subTotal.TSB14Amount19 * multiplier;
            subTotal.TSB1TAmount19 = subTotal.TSB1TAmount19 * multiplier;
            subTotal.TSB1TD1Amount19 = subTotal.TSB1TD1Amount19 * multiplier;
            subTotal.TSB1TD2Amount19 = subTotal.TSB1TD2Amount19 * multiplier;
            subTotal.TSB1TMAmount19 = subTotal.TSB1TMAmount19 * multiplier;
            subTotal.TSB1TLAmount19 = subTotal.TSB1TLAmount19 * multiplier;
            subTotal.TSB1TUAmount19 = subTotal.TSB1TUAmount19 * multiplier;

            subTotal.TSB15Amount20 = subTotal.TSB15Amount20 * multiplier;
            subTotal.TSB11Amount20 = subTotal.TSB11Amount20 * multiplier;
            subTotal.TSB12Amount20 = subTotal.TSB12Amount20 * multiplier;
            subTotal.TSB13Amount20 = subTotal.TSB13Amount20 * multiplier;
            subTotal.TSB14Amount20 = subTotal.TSB14Amount20 * multiplier;
            subTotal.TSB1TAmount20 = subTotal.TSB1TAmount20 * multiplier;
            subTotal.TSB1TD1Amount20 = subTotal.TSB1TD1Amount20 * multiplier;
            subTotal.TSB1TD2Amount20 = subTotal.TSB1TD2Amount20 * multiplier;
            subTotal.TSB1TMAmount20 = subTotal.TSB1TMAmount20 * multiplier;
            subTotal.TSB1TLAmount20 = subTotal.TSB1TLAmount20 * multiplier;
            subTotal.TSB1TUAmount20 = subTotal.TSB1TUAmount20 * multiplier;
        }
        
        public static int GetStockIndex(this TSB1BaseStock baseStat,
           string label)
        {
            if (string.IsNullOrEmpty(label))
            {
                return 0;
            }
            int i = GetStockMatchPos(baseStat, label);
            //stats need each indicator to calc variance and sd
            if (baseStat.AnalyzerType == SB1AnalyzerHelper.ANALYZER_TYPES.sbstat1.ToString())
            {
                i = 0;
            }
            if (i == 0)
            {
                i = GetStockLastEmptyPos(baseStat, label);
            }
            return i;
        }
        private static int GetStockMatchPos(TSB1BaseStock baseStat, 
           string label)
        {
            int i = 0;
            if (baseStat.TSB1Label1 == label)
            {
                i = 1;
                return i;
            }
            if (baseStat.TSB1Label2 == label)
            {
                i = 2;
                return i;
            }
            if (baseStat.TSB1Label3 == label)
            {
                i = 3;
                return i;
            }
            if (baseStat.TSB1Label4 == label)
            {
                i = 4;
                return i;
            }
            if (baseStat.TSB1Label5 == label)
            {
                i = 5;
                return i;
            }
            if (baseStat.TSB1Label6 == label)
            {
                i = 6;
                return i;
            }
            if (baseStat.TSB1Label7 == label)
            {
                i = 7;
                return i;
            }
            if (baseStat.TSB1Label8 == label)
            {
                i = 8;
                return i;
            }
            if (baseStat.TSB1Label9 == label)
            {
                i = 9;
                return i;
            }
            if (baseStat.TSB1Label10 == label)
            {
                i = 10;
                return i;
            }
            if (baseStat.TSB1Label11 == label)
            {
                i = 11;
                return i;
            }
            if (baseStat.TSB1Label12 == label)
            {
                i = 12;
                return i;
            }
            if (baseStat.TSB1Label13 == label)
            {
                i = 13;
                return i;
            }
            if (baseStat.TSB1Label14 == label)
            {
                i = 14;
                return i;
            }
            if (baseStat.TSB1Label15 == label)
            {
                i = 15;
                return i;
            }
            if (baseStat.TSB1Label16 == label)
            {
                i = 16;
                return i;
            }
            if (baseStat.TSB1Label17 == label)
            {
                i = 17;
                return i;
            }
            if (baseStat.TSB1Label18 == label)
            {
                i = 18;
                return i;
            }
            if (baseStat.TSB1Label19 == label)
            {
                i = 19;
                return i;
            }
            if (baseStat.TSB1Label20 == label)
            {
                i = 20;
                return i;
            }
            return i;
        }
        private static int GetStockLastEmptyPos(TSB1BaseStock baseStat,
           string label)
        {
            int i = 0;
            if (string.IsNullOrEmpty(baseStat.TSB1Label1))
            {
                i = 1;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label2))
            {
                i = 2;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label3))
            {
                i = 3;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label4))
            {
                i = 4;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label5))
            {
                i = 5;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label6))
            {
                i = 6;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label7))
            {
                i = 7;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label8))
            {
                i = 8;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label9))
            {
                i = 9;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label10))
            {
                i = 10;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label11))
            {
                i = 11;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label12))
            {
                i = 12;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label13))
            {
                i = 13;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label14))
            {
                i = 14;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label15))
            {
                i = 15;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label16))
            {
                i = 16;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label17))
            {
                i = 17;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label18))
            {
                i = 18;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label19))
            {
                i = 19;
                return i;
            }
            if (string.IsNullOrEmpty(baseStat.TSB1Label20))
            {
                i = 20;
                return i;
            }
            return i;
        }
        public static bool TotalStockNeedsIds(this TSB1BaseStock baseStat, int i)
        {
            bool bNeedsIds = false;
            if (i == 1)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label1))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 2)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label2))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 3)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label3))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 4)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label4))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 5)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label5))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 6)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label6))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 7)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label7))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 8)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label8))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 9)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label9))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 10)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label10))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 11)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label11))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 12)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label12))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 13)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label13))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 14)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label14))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 15)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label15))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 16)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label16))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 17)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label17))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 18)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label18))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 19)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label19))
                {
                    bNeedsIds = true;
                }
            }
            if (i == 20)
            {
                if (string.IsNullOrEmpty(baseStat.TSB1Label20))
                {
                    bNeedsIds = true;
                }
            }
            return bNeedsIds;
        }
        
    }
}

