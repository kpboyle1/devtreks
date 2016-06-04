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
    ///             The class tracks annual changes in totals.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. 
    ///</summary>
    public class SB1Change1 : SB1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB1Change1(CalculatorParameters calcs)
            : base()
        {
            //subprice object
            InitTotalSB1Change1Properties(this, calcs);
        }
        //copy constructor
        public SB1Change1(SB1Change1 calculator)
            : base(calculator)
        {
            CopyTotalSB1Change1Properties(this, calculator);
        }
        #region props
        //note that additional display properties are in parent SB101Stock
        public double TotalSBScoreAmountChange { get; set; }
        public double TotalSBScorePercentChange { get; set; }
        public double TotalSBScoreBaseChange { get; set; }
        public double TotalSBScoreBasePercentChange { get; set; }

        private const string cTotalSBScoreAmountChange = "TSBScoreAmountChange";
        private const string cTotalSBScorePercentChange = "TSBScorePercentChange";
        private const string cTotalSBScoreBaseChange = "TSBScoreBaseChange";
        private const string cTotalSBScoreBasePercentChange = "TSBScoreBasePercentChange";

        public double TotalSBScoreLAmountChange { get; set; }
        public double TotalSBScoreLPercentChange { get; set; }
        public double TotalSBScoreLBaseChange { get; set; }
        public double TotalSBScoreLBasePercentChange { get; set; }

        private const string cTotalSBScoreLAmountChange = "TSBScoreLAmountChange";
        private const string cTotalSBScoreLPercentChange = "TSBScoreLPercentChange";
        private const string cTotalSBScoreLBaseChange = "TSBScoreLBaseChange";
        private const string cTotalSBScoreLBasePercentChange = "TSBScoreLBasePercentChange";

        public double TotalSBScoreUAmountChange { get; set; }
        public double TotalSBScoreUPercentChange { get; set; }
        public double TotalSBScoreUBaseChange { get; set; }
        public double TotalSBScoreUBasePercentChange { get; set; }

        private const string cTotalSBScoreUAmountChange = "TSBScoreUAmountChange";
        private const string cTotalSBScoreUPercentChange = "TSBScoreUPercentChange";
        private const string cTotalSBScoreUBaseChange = "TSBScoreUBaseChange";
        private const string cTotalSBScoreUBasePercentChange = "TSBScoreUBasePercentChange";


        public double TotalSB1AmountChange { get; set; }
        public double TotalSB1PercentChange { get; set; }
        public double TotalSB1BaseChange { get; set; }
        public double TotalSB1BasePercentChange { get; set; }

        private const string cTotalSB1AmountChange = "TSB1AmountChange";
        private const string cTotalSB1PercentChange = "TSB1PercentChange";
        private const string cTotalSB1BaseChange = "TSB1BaseChange";
        private const string cTotalSB1BasePercentChange = "TSB1BasePercentChange";

        public double TotalSB2AmountChange { get; set; }
        public double TotalSB2PercentChange { get; set; }
        public double TotalSB2BaseChange { get; set; }
        public double TotalSB2BasePercentChange { get; set; }

        private const string cTotalSB2AmountChange = "TSB2AmountChange";
        private const string cTotalSB2PercentChange = "TSB2PercentChange";
        private const string cTotalSB2BaseChange = "TSB2BaseChange";
        private const string cTotalSB2BasePercentChange = "TSB2BasePercentChange";

        public double TotalSB3AmountChange { get; set; }
        public double TotalSB3PercentChange { get; set; }
        public double TotalSB3BaseChange { get; set; }
        public double TotalSB3BasePercentChange { get; set; }

        private const string cTotalSB3AmountChange = "TSB3AmountChange";
        private const string cTotalSB3PercentChange = "TSB3PercentChange";
        private const string cTotalSB3BaseChange = "TSB3BaseChange";
        private const string cTotalSB3BasePercentChange = "TSB3BasePercentChange";

        public double TotalSB4AmountChange { get; set; }
        public double TotalSB4PercentChange { get; set; }
        public double TotalSB4BaseChange { get; set; }
        public double TotalSB4BasePercentChange { get; set; }

        private const string cTotalSB4AmountChange = "TSB4AmountChange";
        private const string cTotalSB4PercentChange = "TSB4PercentChange";
        private const string cTotalSB4BaseChange = "TSB4BaseChange";
        private const string cTotalSB4BasePercentChange = "TSB4BasePercentChange";

        public double TotalSB5AmountChange { get; set; }
        public double TotalSB5PercentChange { get; set; }
        public double TotalSB5BaseChange { get; set; }
        public double TotalSB5BasePercentChange { get; set; }

        private const string cTotalSB5AmountChange = "TSB5AmountChange";
        private const string cTotalSB5PercentChange = "TSB5PercentChange";
        private const string cTotalSB5BaseChange = "TSB5BaseChange";
        private const string cTotalSB5BasePercentChange = "TSB5BasePercentChange";

        public double TotalSB6AmountChange { get; set; }
        public double TotalSB6PercentChange { get; set; }
        public double TotalSB6BaseChange { get; set; }
        public double TotalSB6BasePercentChange { get; set; }

        private const string cTotalSB6AmountChange = "TSB6AmountChange";
        private const string cTotalSB6PercentChange = "TSB6PercentChange";
        private const string cTotalSB6BaseChange = "TSB6BaseChange";
        private const string cTotalSB6BasePercentChange = "TSB6BasePercentChange";

        public double TotalSB7AmountChange { get; set; }
        public double TotalSB7PercentChange { get; set; }
        public double TotalSB7BaseChange { get; set; }
        public double TotalSB7BasePercentChange { get; set; }

        private const string cTotalSB7AmountChange = "TSB7AmountChange";
        private const string cTotalSB7PercentChange = "TSB7PercentChange";
        private const string cTotalSB7BaseChange = "TSB7BaseChange";
        private const string cTotalSB7BasePercentChange = "TSB7BasePercentChange";

        public double TotalSB8AmountChange { get; set; }
        public double TotalSB8PercentChange { get; set; }
        public double TotalSB8BaseChange { get; set; }
        public double TotalSB8BasePercentChange { get; set; }

        private const string cTotalSB8AmountChange = "TSB8AmountChange";
        private const string cTotalSB8PercentChange = "TSB8PercentChange";
        private const string cTotalSB8BaseChange = "TSB8BaseChange";
        private const string cTotalSB8BasePercentChange = "TSB8BasePercentChange";

        public double TotalSB9AmountChange { get; set; }
        public double TotalSB9PercentChange { get; set; }
        public double TotalSB9BaseChange { get; set; }
        public double TotalSB9BasePercentChange { get; set; }

        private const string cTotalSB9AmountChange = "TSB9AmountChange";
        private const string cTotalSB9PercentChange = "TSB9PercentChange";
        private const string cTotalSB9BaseChange = "TSB9BaseChange";
        private const string cTotalSB9BasePercentChange = "TSB9BasePercentChange";

        public double TotalSB10AmountChange { get; set; }
        public double TotalSB10PercentChange { get; set; }
        public double TotalSB10BaseChange { get; set; }
        public double TotalSB10BasePercentChange { get; set; }

        private const string cTotalSB10AmountChange = "TSB10AmountChange";
        private const string cTotalSB10PercentChange = "TSB10PercentChange";
        private const string cTotalSB10BaseChange = "TSB10BaseChange";
        private const string cTotalSB10BasePercentChange = "TSB10BasePercentChange";

        public double TotalSB11AmountChange { get; set; }
        public double TotalSB11PercentChange { get; set; }
        public double TotalSB11BaseChange { get; set; }
        public double TotalSB11BasePercentChange { get; set; }

        private const string cTotalSB11AmountChange = "TSB11AmountChange";
        private const string cTotalSB11PercentChange = "TSB11PercentChange";
        private const string cTotalSB11BaseChange = "TSB11BaseChange";
        private const string cTotalSB11BasePercentChange = "TSB11BasePercentChange";

        public double TotalSB12AmountChange { get; set; }
        public double TotalSB12PercentChange { get; set; }
        public double TotalSB12BaseChange { get; set; }
        public double TotalSB12BasePercentChange { get; set; }

        private const string cTotalSB12AmountChange = "TSB12AmountChange";
        private const string cTotalSB12PercentChange = "TSB12PercentChange";
        private const string cTotalSB12BaseChange = "TSB12BaseChange";
        private const string cTotalSB12BasePercentChange = "TSB12BasePercentChange";

        public double TotalSB13AmountChange { get; set; }
        public double TotalSB13PercentChange { get; set; }
        public double TotalSB13BaseChange { get; set; }
        public double TotalSB13BasePercentChange { get; set; }

        private const string cTotalSB13AmountChange = "TSB13AmountChange";
        private const string cTotalSB13PercentChange = "TSB13PercentChange";
        private const string cTotalSB13BaseChange = "TSB13BaseChange";
        private const string cTotalSB13BasePercentChange = "TSB13BasePercentChange";

        public double TotalSB14AmountChange { get; set; }
        public double TotalSB14PercentChange { get; set; }
        public double TotalSB14BaseChange { get; set; }
        public double TotalSB14BasePercentChange { get; set; }

        private const string cTotalSB14AmountChange = "TSB14AmountChange";
        private const string cTotalSB14PercentChange = "TSB14PercentChange";
        private const string cTotalSB14BaseChange = "TSB14BaseChange";
        private const string cTotalSB14BasePercentChange = "TSB14BasePercentChange";

        public double TotalSB15AmountChange { get; set; }
        public double TotalSB15PercentChange { get; set; }
        public double TotalSB15BaseChange { get; set; }
        public double TotalSB15BasePercentChange { get; set; }

        private const string cTotalSB15AmountChange = "TSB15AmountChange";
        private const string cTotalSB15PercentChange = "TSB15PercentChange";
        private const string cTotalSB15BaseChange = "TSB15BaseChange";
        private const string cTotalSB15BasePercentChange = "TSB15BasePercentChange";

        public double TotalSB16AmountChange { get; set; }
        public double TotalSB16PercentChange { get; set; }
        public double TotalSB16BaseChange { get; set; }
        public double TotalSB16BasePercentChange { get; set; }

        private const string cTotalSB16AmountChange = "TSB16AmountChange";
        private const string cTotalSB16PercentChange = "TSB16PercentChange";
        private const string cTotalSB16BaseChange = "TSB16BaseChange";
        private const string cTotalSB16BasePercentChange = "TSB16BasePercentChange";

        public double TotalSB17AmountChange { get; set; }
        public double TotalSB17PercentChange { get; set; }
        public double TotalSB17BaseChange { get; set; }
        public double TotalSB17BasePercentChange { get; set; }

        private const string cTotalSB17AmountChange = "TSB17AmountChange";
        private const string cTotalSB17PercentChange = "TSB17PercentChange";
        private const string cTotalSB17BaseChange = "TSB17BaseChange";
        private const string cTotalSB17BasePercentChange = "TSB17BasePercentChange";

        public double TotalSB18AmountChange { get; set; }
        public double TotalSB18PercentChange { get; set; }
        public double TotalSB18BaseChange { get; set; }
        public double TotalSB18BasePercentChange { get; set; }

        private const string cTotalSB18AmountChange = "TSB18AmountChange";
        private const string cTotalSB18PercentChange = "TSB18PercentChange";
        private const string cTotalSB18BaseChange = "TSB18BaseChange";
        private const string cTotalSB18BasePercentChange = "TSB18BasePercentChange";

        public double TotalSB19AmountChange { get; set; }
        public double TotalSB19PercentChange { get; set; }
        public double TotalSB19BaseChange { get; set; }
        public double TotalSB19BasePercentChange { get; set; }

        private const string cTotalSB19AmountChange = "TSB19AmountChange";
        private const string cTotalSB19PercentChange = "TSB19PercentChange";
        private const string cTotalSB19BaseChange = "TSB19BaseChange";
        private const string cTotalSB19BasePercentChange = "TSB19BasePercentChange";

        public double TotalSB20AmountChange { get; set; }
        public double TotalSB20PercentChange { get; set; }
        public double TotalSB20BaseChange { get; set; }
        public double TotalSB20BasePercentChange { get; set; }

        private const string cTotalSB20AmountChange = "TSB20AmountChange";
        private const string cTotalSB20PercentChange = "TSB20PercentChange";
        private const string cTotalSB20BaseChange = "TSB20BaseChange";
        private const string cTotalSB20BasePercentChange = "TSB20BasePercentChange";
        #endregion
        #region init
        public void InitTotalSB1Change1Properties(SB1Change1 ind,
            CalculatorParameters calcs)
        {
            //avoid nulls
            InitSB1AnalysisProperties(ind, calcs);

            ind.TotalSBScoreAmountChange = 0;
            ind.TotalSBScorePercentChange = 0;
            ind.TotalSBScoreBaseChange = 0;
            ind.TotalSBScoreBasePercentChange = 0;
            ind.TotalSBScoreLAmountChange = 0;
            ind.TotalSBScoreLPercentChange = 0;
            ind.TotalSBScoreLBaseChange = 0;
            ind.TotalSBScoreLBasePercentChange = 0;
            ind.TotalSBScoreUAmountChange = 0;
            ind.TotalSBScoreUPercentChange = 0;
            ind.TotalSBScoreUBaseChange = 0;
            ind.TotalSBScoreUBasePercentChange = 0;

            ind.ErrorMessage = string.Empty;
            ind.TotalSB1AmountChange = 0;
            ind.TotalSB1PercentChange = 0;
            ind.TotalSB1BaseChange = 0;
            ind.TotalSB1BasePercentChange = 0;

            ind.TotalSB2AmountChange = 0;
            ind.TotalSB2PercentChange = 0;
            ind.TotalSB2BaseChange = 0;
            ind.TotalSB2BasePercentChange = 0;

            ind.TotalSB3AmountChange = 0;
            ind.TotalSB3PercentChange = 0;
            ind.TotalSB3BaseChange = 0;
            ind.TotalSB3BasePercentChange = 0;

            ind.TotalSB4AmountChange = 0;
            ind.TotalSB4PercentChange = 0;
            ind.TotalSB4BaseChange = 0;
            ind.TotalSB4BasePercentChange = 0;

            ind.TotalSB5AmountChange = 0;
            ind.TotalSB5PercentChange = 0;
            ind.TotalSB5BaseChange = 0;
            ind.TotalSB5BasePercentChange = 0;

            ind.TotalSB6AmountChange = 0;
            ind.TotalSB6PercentChange = 0;
            ind.TotalSB6BaseChange = 0;
            ind.TotalSB6BasePercentChange = 0;

            ind.TotalSB7AmountChange = 0;
            ind.TotalSB7PercentChange = 0;
            ind.TotalSB7BaseChange = 0;
            ind.TotalSB7BasePercentChange = 0;

            ind.TotalSB8AmountChange = 0;
            ind.TotalSB8PercentChange = 0;
            ind.TotalSB8BaseChange = 0;
            ind.TotalSB8BasePercentChange = 0;

            ind.TotalSB9AmountChange = 0;
            ind.TotalSB9PercentChange = 0;
            ind.TotalSB9BaseChange = 0;
            ind.TotalSB9BasePercentChange = 0;

            ind.TotalSB10AmountChange = 0;
            ind.TotalSB10PercentChange = 0;
            ind.TotalSB10BaseChange = 0;
            ind.TotalSB10BasePercentChange = 0;

            ind.TotalSB11AmountChange = 0;
            ind.TotalSB11PercentChange = 0;
            ind.TotalSB11BaseChange = 0;
            ind.TotalSB11BasePercentChange = 0;

            ind.TotalSB12AmountChange = 0;
            ind.TotalSB12PercentChange = 0;
            ind.TotalSB12BaseChange = 0;
            ind.TotalSB12BasePercentChange = 0;

            ind.TotalSB13AmountChange = 0;
            ind.TotalSB13PercentChange = 0;
            ind.TotalSB13BaseChange = 0;
            ind.TotalSB13BasePercentChange = 0;

            ind.TotalSB14AmountChange = 0;
            ind.TotalSB14PercentChange = 0;
            ind.TotalSB14BaseChange = 0;
            ind.TotalSB14BasePercentChange = 0;

            ind.TotalSB15AmountChange = 0;
            ind.TotalSB15PercentChange = 0;
            ind.TotalSB15BaseChange = 0;
            ind.TotalSB15BasePercentChange = 0;

            ind.TotalSB16AmountChange = 0;
            ind.TotalSB16PercentChange = 0;
            ind.TotalSB16BaseChange = 0;
            ind.TotalSB16BasePercentChange = 0;

            ind.TotalSB17AmountChange = 0;
            ind.TotalSB17PercentChange = 0;
            ind.TotalSB17BaseChange = 0;
            ind.TotalSB17BasePercentChange = 0;

            ind.TotalSB18AmountChange = 0;
            ind.TotalSB18PercentChange = 0;
            ind.TotalSB18BaseChange = 0;
            ind.TotalSB18BasePercentChange = 0;

            ind.TotalSB19AmountChange = 0;
            ind.TotalSB19PercentChange = 0;
            ind.TotalSB19BaseChange = 0;
            ind.TotalSB19BasePercentChange = 0;

            ind.TotalSB20AmountChange = 0;
            ind.TotalSB20PercentChange = 0;
            ind.TotalSB20BaseChange = 0;
            ind.TotalSB20BasePercentChange = 0;

            ind.CalcParameters = calcs;
            ind.SB11Stock = new SB101Stock();
            ind.SB12Stock = new SB102Stock();
        }

        public void CopyTotalSB1Change1Properties(SB1Change1 ind,
            SB1Change1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;

            ind.TotalSBScoreAmountChange = calculator.TotalSBScoreAmountChange;
            ind.TotalSBScorePercentChange = calculator.TotalSBScorePercentChange;
            ind.TotalSBScoreBaseChange = calculator.TotalSBScoreBaseChange;
            ind.TotalSBScoreBasePercentChange = calculator.TotalSBScoreBasePercentChange;
            ind.TotalSBScoreLAmountChange = calculator.TotalSBScoreLAmountChange;
            ind.TotalSBScoreLPercentChange = calculator.TotalSBScoreLPercentChange;
            ind.TotalSBScoreLBaseChange = calculator.TotalSBScoreLBaseChange;
            ind.TotalSBScoreLBasePercentChange = calculator.TotalSBScoreLBasePercentChange;
            ind.TotalSBScoreUAmountChange = calculator.TotalSBScoreUAmountChange;
            ind.TotalSBScoreUPercentChange = calculator.TotalSBScoreUPercentChange;
            ind.TotalSBScoreUBaseChange = calculator.TotalSBScoreUBaseChange;
            ind.TotalSBScoreUBasePercentChange = calculator.TotalSBScoreUBasePercentChange;

            ind.TotalSB1AmountChange = calculator.TotalSB1AmountChange;
            ind.TotalSB1PercentChange = calculator.TotalSB1PercentChange;
            ind.TotalSB1BaseChange = calculator.TotalSB1BaseChange;
            ind.TotalSB1BasePercentChange = calculator.TotalSB1BasePercentChange;

            ind.TotalSB2AmountChange = calculator.TotalSB2AmountChange;
            ind.TotalSB2PercentChange = calculator.TotalSB2PercentChange;
            ind.TotalSB2BaseChange = calculator.TotalSB2BaseChange;
            ind.TotalSB2BasePercentChange = calculator.TotalSB2BasePercentChange;

            ind.TotalSB3AmountChange = calculator.TotalSB3AmountChange;
            ind.TotalSB3PercentChange = calculator.TotalSB3PercentChange;
            ind.TotalSB3BaseChange = calculator.TotalSB3BaseChange;
            ind.TotalSB3BasePercentChange = calculator.TotalSB3BasePercentChange;

            ind.TotalSB4AmountChange = calculator.TotalSB4AmountChange;
            ind.TotalSB4PercentChange = calculator.TotalSB4PercentChange;
            ind.TotalSB4BaseChange = calculator.TotalSB4BaseChange;
            ind.TotalSB4BasePercentChange = calculator.TotalSB4BasePercentChange;

            ind.TotalSB5AmountChange = calculator.TotalSB5AmountChange;
            ind.TotalSB5PercentChange = calculator.TotalSB5PercentChange;
            ind.TotalSB5BaseChange = calculator.TotalSB5BaseChange;
            ind.TotalSB5BasePercentChange = calculator.TotalSB5BasePercentChange;

            ind.TotalSB6AmountChange = calculator.TotalSB6AmountChange;
            ind.TotalSB6PercentChange = calculator.TotalSB6PercentChange;
            ind.TotalSB6BaseChange = calculator.TotalSB6BaseChange;
            ind.TotalSB6BasePercentChange = calculator.TotalSB6BasePercentChange;

            ind.TotalSB7AmountChange = calculator.TotalSB7AmountChange;
            ind.TotalSB7PercentChange = calculator.TotalSB7PercentChange;
            ind.TotalSB7BaseChange = calculator.TotalSB7BaseChange;
            ind.TotalSB7BasePercentChange = calculator.TotalSB7BasePercentChange;

            ind.TotalSB8AmountChange = calculator.TotalSB8AmountChange;
            ind.TotalSB8PercentChange = calculator.TotalSB8PercentChange;
            ind.TotalSB8BaseChange = calculator.TotalSB8BaseChange;
            ind.TotalSB8BasePercentChange = calculator.TotalSB8BasePercentChange;

            ind.TotalSB9AmountChange = calculator.TotalSB9AmountChange;
            ind.TotalSB9PercentChange = calculator.TotalSB9PercentChange;
            ind.TotalSB9BaseChange = calculator.TotalSB9BaseChange;
            ind.TotalSB9BasePercentChange = calculator.TotalSB9BasePercentChange;

            ind.TotalSB10AmountChange = calculator.TotalSB10AmountChange;
            ind.TotalSB10PercentChange = calculator.TotalSB10PercentChange;
            ind.TotalSB10BaseChange = calculator.TotalSB10BaseChange;
            ind.TotalSB10BasePercentChange = calculator.TotalSB10BasePercentChange;

            ind.TotalSB11AmountChange = calculator.TotalSB11AmountChange;
            ind.TotalSB11PercentChange = calculator.TotalSB11PercentChange;
            ind.TotalSB11BaseChange = calculator.TotalSB11BaseChange;
            ind.TotalSB11BasePercentChange = calculator.TotalSB11BasePercentChange;

            ind.TotalSB12AmountChange = calculator.TotalSB12AmountChange;
            ind.TotalSB12PercentChange = calculator.TotalSB12PercentChange;
            ind.TotalSB12BaseChange = calculator.TotalSB12BaseChange;
            ind.TotalSB12BasePercentChange = calculator.TotalSB12BasePercentChange;

            ind.TotalSB13AmountChange = calculator.TotalSB13AmountChange;
            ind.TotalSB13PercentChange = calculator.TotalSB13PercentChange;
            ind.TotalSB13BaseChange = calculator.TotalSB13BaseChange;
            ind.TotalSB13BasePercentChange = calculator.TotalSB13BasePercentChange;

            ind.TotalSB14AmountChange = calculator.TotalSB14AmountChange;
            ind.TotalSB14PercentChange = calculator.TotalSB14PercentChange;
            ind.TotalSB14BaseChange = calculator.TotalSB14BaseChange;
            ind.TotalSB14BasePercentChange = calculator.TotalSB14BasePercentChange;

            ind.TotalSB15AmountChange = calculator.TotalSB15AmountChange;
            ind.TotalSB15PercentChange = calculator.TotalSB15PercentChange;
            ind.TotalSB15BaseChange = calculator.TotalSB15BaseChange;
            ind.TotalSB15BasePercentChange = calculator.TotalSB15BasePercentChange;

            ind.TotalSB16AmountChange = calculator.TotalSB16AmountChange;
            ind.TotalSB16PercentChange = calculator.TotalSB16PercentChange;
            ind.TotalSB16BaseChange = calculator.TotalSB16BaseChange;
            ind.TotalSB16BasePercentChange = calculator.TotalSB16BasePercentChange;

            ind.TotalSB17AmountChange = calculator.TotalSB17AmountChange;
            ind.TotalSB17PercentChange = calculator.TotalSB17PercentChange;
            ind.TotalSB17BaseChange = calculator.TotalSB17BaseChange;
            ind.TotalSB17BasePercentChange = calculator.TotalSB17BasePercentChange;

            ind.TotalSB18AmountChange = calculator.TotalSB18AmountChange;
            ind.TotalSB18PercentChange = calculator.TotalSB18PercentChange;
            ind.TotalSB18BaseChange = calculator.TotalSB18BaseChange;
            ind.TotalSB18BasePercentChange = calculator.TotalSB18BasePercentChange;

            ind.TotalSB19AmountChange = calculator.TotalSB19AmountChange;
            ind.TotalSB19PercentChange = calculator.TotalSB19PercentChange;
            ind.TotalSB19BaseChange = calculator.TotalSB19BaseChange;
            ind.TotalSB19BasePercentChange = calculator.TotalSB19BasePercentChange;

            ind.TotalSB20AmountChange = calculator.TotalSB20AmountChange;
            ind.TotalSB20PercentChange = calculator.TotalSB20PercentChange;
            ind.TotalSB20BaseChange = calculator.TotalSB20BaseChange;
            ind.TotalSB20BasePercentChange = calculator.TotalSB20BasePercentChange;
        }

        public void SetTotalSB1Change1Properties(SB1Change1 ind,
            string attNameExtension, XElement calculator)
        {

            ind.TotalSBScoreAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreAmountChange, attNameExtension));
            ind.TotalSBScorePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScorePercentChange, attNameExtension));
            ind.TotalSBScoreBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreBaseChange, attNameExtension));
            ind.TotalSBScoreBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreBasePercentChange, attNameExtension));
            ind.TotalSBScoreLAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLAmountChange, attNameExtension));
            ind.TotalSBScoreLPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLPercentChange, attNameExtension));
            ind.TotalSBScoreLBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLBaseChange, attNameExtension));
            ind.TotalSBScoreLBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLBasePercentChange, attNameExtension));
            ind.TotalSBScoreUAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUAmountChange, attNameExtension));
            ind.TotalSBScoreUPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUPercentChange, attNameExtension));
            ind.TotalSBScoreUBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUBaseChange, attNameExtension));
            ind.TotalSBScoreUBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUBasePercentChange, attNameExtension));

            ind.TotalSB1AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1AmountChange, attNameExtension));
            ind.TotalSB1PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1PercentChange, attNameExtension));
            ind.TotalSB1BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1BaseChange, attNameExtension));
            ind.TotalSB1BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1BasePercentChange, attNameExtension));

            ind.TotalSB2AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2AmountChange, attNameExtension));
            ind.TotalSB2PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2PercentChange, attNameExtension));
            ind.TotalSB2BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2BaseChange, attNameExtension));
            ind.TotalSB2BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2BasePercentChange, attNameExtension));

            ind.TotalSB3AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3AmountChange, attNameExtension));
            ind.TotalSB3PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3PercentChange, attNameExtension));
            ind.TotalSB3BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3BaseChange, attNameExtension));
            ind.TotalSB3BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3BasePercentChange, attNameExtension));

            ind.TotalSB4AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4AmountChange, attNameExtension));
            ind.TotalSB4PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4PercentChange, attNameExtension));
            ind.TotalSB4BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4BaseChange, attNameExtension));
            ind.TotalSB4BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4BasePercentChange, attNameExtension));

            ind.TotalSB5AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5AmountChange, attNameExtension));
            ind.TotalSB5PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5PercentChange, attNameExtension));
            ind.TotalSB5BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5BaseChange, attNameExtension));
            ind.TotalSB5BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5BasePercentChange, attNameExtension));

            ind.TotalSB6AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6AmountChange, attNameExtension));
            ind.TotalSB6PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6PercentChange, attNameExtension));
            ind.TotalSB6BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6BaseChange, attNameExtension));
            ind.TotalSB6BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6BasePercentChange, attNameExtension));

            ind.TotalSB7AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7AmountChange, attNameExtension));
            ind.TotalSB7PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7PercentChange, attNameExtension));
            ind.TotalSB7BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7BaseChange, attNameExtension));
            ind.TotalSB7BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7BasePercentChange, attNameExtension));

            ind.TotalSB8AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8AmountChange, attNameExtension));
            ind.TotalSB8PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8PercentChange, attNameExtension));
            ind.TotalSB8BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8BaseChange, attNameExtension));
            ind.TotalSB8BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8BasePercentChange, attNameExtension));

            ind.TotalSB9AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9AmountChange, attNameExtension));
            ind.TotalSB9PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9PercentChange, attNameExtension));
            ind.TotalSB9BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9BaseChange, attNameExtension));
            ind.TotalSB9BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9BasePercentChange, attNameExtension));

            ind.TotalSB10AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10AmountChange, attNameExtension));
            ind.TotalSB10PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10PercentChange, attNameExtension));
            ind.TotalSB10BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10BaseChange, attNameExtension));
            ind.TotalSB10BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10BasePercentChange, attNameExtension));

            ind.TotalSB11AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11AmountChange, attNameExtension));
            ind.TotalSB11PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11PercentChange, attNameExtension));
            ind.TotalSB11BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11BaseChange, attNameExtension));
            ind.TotalSB11BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11BasePercentChange, attNameExtension));

            ind.TotalSB12AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12AmountChange, attNameExtension));
            ind.TotalSB12PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12PercentChange, attNameExtension));
            ind.TotalSB12BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12BaseChange, attNameExtension));
            ind.TotalSB12BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12BasePercentChange, attNameExtension));

            ind.TotalSB13AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13AmountChange, attNameExtension));
            ind.TotalSB13PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13PercentChange, attNameExtension));
            ind.TotalSB13BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13BaseChange, attNameExtension));
            ind.TotalSB13BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13BasePercentChange, attNameExtension));

            ind.TotalSB14AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14AmountChange, attNameExtension));
            ind.TotalSB14PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14PercentChange, attNameExtension));
            ind.TotalSB14BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14BaseChange, attNameExtension));
            ind.TotalSB14BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14BasePercentChange, attNameExtension));

            ind.TotalSB15AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15AmountChange, attNameExtension));
            ind.TotalSB15PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15PercentChange, attNameExtension));
            ind.TotalSB15BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15BaseChange, attNameExtension));
            ind.TotalSB15BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15BasePercentChange, attNameExtension));

            ind.TotalSB16AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16AmountChange, attNameExtension));
            ind.TotalSB16PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16PercentChange, attNameExtension));
            ind.TotalSB16BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16BaseChange, attNameExtension));
            ind.TotalSB16BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16BasePercentChange, attNameExtension));

            ind.TotalSB17AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17AmountChange, attNameExtension));
            ind.TotalSB17PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17PercentChange, attNameExtension));
            ind.TotalSB17BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17BaseChange, attNameExtension));
            ind.TotalSB17BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17BasePercentChange, attNameExtension));

            ind.TotalSB18AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18AmountChange, attNameExtension));
            ind.TotalSB18PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18PercentChange, attNameExtension));
            ind.TotalSB18BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18BaseChange, attNameExtension));
            ind.TotalSB18BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18BasePercentChange, attNameExtension));

            ind.TotalSB19AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19AmountChange, attNameExtension));
            ind.TotalSB19PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19PercentChange, attNameExtension));
            ind.TotalSB19BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19BaseChange, attNameExtension));
            ind.TotalSB19BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19BasePercentChange, attNameExtension));

            ind.TotalSB20AmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20AmountChange, attNameExtension));
            ind.TotalSB20PercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20PercentChange, attNameExtension));
            ind.TotalSB20BaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20BaseChange, attNameExtension));
            ind.TotalSB20BasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20BasePercentChange, attNameExtension));
        }

        public void SetTotalSB1Change1Property(SB1Change1 ind,
            string attName, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSBScoreAmountChange:
                    ind.TotalSBScoreAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScorePercentChange:
                    ind.TotalSBScorePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreBaseChange:
                    ind.TotalSBScoreBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreBasePercentChange:
                    ind.TotalSBScoreBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLAmountChange:
                    ind.TotalSBScoreLAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLPercentChange:
                    ind.TotalSBScoreLPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLBaseChange:
                    ind.TotalSBScoreLBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLBasePercentChange:
                    ind.TotalSBScoreLBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUAmountChange:
                    ind.TotalSBScoreUAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUPercentChange:
                    ind.TotalSBScoreUPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUBaseChange:
                    ind.TotalSBScoreUBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUBasePercentChange:
                    ind.TotalSBScoreUBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1AmountChange:
                    ind.TotalSB1AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1PercentChange:
                    ind.TotalSB1PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1BaseChange:
                    ind.TotalSB1BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1BasePercentChange:
                    ind.TotalSB1BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2AmountChange:
                    ind.TotalSB2AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2PercentChange:
                    ind.TotalSB2PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2BaseChange:
                    ind.TotalSB2BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2BasePercentChange:
                    ind.TotalSB2BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3AmountChange:
                    ind.TotalSB3AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3PercentChange:
                    ind.TotalSB3PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3BaseChange:
                    ind.TotalSB3BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3BasePercentChange:
                    ind.TotalSB3BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4AmountChange:
                    ind.TotalSB4AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4PercentChange:
                    ind.TotalSB4PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4BaseChange:
                    ind.TotalSB4BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4BasePercentChange:
                    ind.TotalSB4BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
               case cTotalSB5AmountChange:
                    ind.TotalSB5AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5PercentChange:
                    ind.TotalSB5PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5BaseChange:
                    ind.TotalSB5BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5BasePercentChange:
                    ind.TotalSB5BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
               case cTotalSB6AmountChange:
                    ind.TotalSB6AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6PercentChange:
                    ind.TotalSB6PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6BaseChange:
                    ind.TotalSB6BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6BasePercentChange:
                    ind.TotalSB6BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7AmountChange:
                    ind.TotalSB7AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7PercentChange:
                    ind.TotalSB7PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7BaseChange:
                    ind.TotalSB7BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7BasePercentChange:
                    ind.TotalSB7BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                 case cTotalSB8AmountChange:
                    ind.TotalSB8AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8PercentChange:
                    ind.TotalSB8PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8BaseChange:
                    ind.TotalSB8BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8BasePercentChange:
                    ind.TotalSB8BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9AmountChange:
                    ind.TotalSB9AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9PercentChange:
                    ind.TotalSB9PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9BaseChange:
                    ind.TotalSB9BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9BasePercentChange:
                    ind.TotalSB9BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10AmountChange:
                    ind.TotalSB10AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10PercentChange:
                    ind.TotalSB10PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10BaseChange:
                    ind.TotalSB10BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10BasePercentChange:
                    ind.TotalSB10BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11AmountChange:
                    ind.TotalSB11AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11PercentChange:
                    ind.TotalSB11PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11BaseChange:
                    ind.TotalSB11BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11BasePercentChange:
                    ind.TotalSB11BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12AmountChange:
                    ind.TotalSB12AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12PercentChange:
                    ind.TotalSB12PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12BaseChange:
                    ind.TotalSB12BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12BasePercentChange:
                    ind.TotalSB12BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
               case cTotalSB13AmountChange:
                    ind.TotalSB13AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13PercentChange:
                    ind.TotalSB13PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13BaseChange:
                    ind.TotalSB13BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13BasePercentChange:
                    ind.TotalSB13BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14AmountChange:
                    ind.TotalSB14AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14PercentChange:
                    ind.TotalSB14PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14BaseChange:
                    ind.TotalSB14BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14BasePercentChange:
                    ind.TotalSB14BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15AmountChange:
                    ind.TotalSB15AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15PercentChange:
                    ind.TotalSB15PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15BaseChange:
                    ind.TotalSB15BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15BasePercentChange:
                    ind.TotalSB15BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16AmountChange:
                    ind.TotalSB16AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16PercentChange:
                    ind.TotalSB16PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16BaseChange:
                    ind.TotalSB16BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16BasePercentChange:
                    ind.TotalSB16BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17AmountChange:
                    ind.TotalSB17AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17PercentChange:
                    ind.TotalSB17PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17BaseChange:
                    ind.TotalSB17BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17BasePercentChange:
                    ind.TotalSB17BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
               case cTotalSB18AmountChange:
                    ind.TotalSB18AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18PercentChange:
                    ind.TotalSB18PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18BaseChange:
                    ind.TotalSB18BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18BasePercentChange:
                    ind.TotalSB18BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19AmountChange:
                    ind.TotalSB19AmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19PercentChange:
                    ind.TotalSB19PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19BaseChange:
                    ind.TotalSB19BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19BasePercentChange:
                    ind.TotalSB19BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20PercentChange:
                    ind.TotalSB20PercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20BaseChange:
                    ind.TotalSB20BaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20BasePercentChange:
                    ind.TotalSB20BasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalSB1Change1Property(SB1Change1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSBScoreAmountChange:
                    sPropertyValue = ind.TotalSBScoreAmountChange.ToString();
                    break;
                case cTotalSBScorePercentChange:
                    sPropertyValue = ind.TotalSBScorePercentChange.ToString();
                    break;
                case cTotalSBScoreBaseChange:
                    sPropertyValue = ind.TotalSBScoreBaseChange.ToString();
                    break;
                case cTotalSBScoreBasePercentChange:
                    sPropertyValue = ind.TotalSBScoreBasePercentChange.ToString();
                    break;
                case cTotalSBScoreLAmountChange:
                    sPropertyValue = ind.TotalSBScoreLAmountChange.ToString();
                    break;
                case cTotalSBScoreLPercentChange:
                    sPropertyValue = ind.TotalSBScoreLPercentChange.ToString();
                    break;
                case cTotalSBScoreLBaseChange:
                    sPropertyValue = ind.TotalSBScoreLBaseChange.ToString();
                    break;
                case cTotalSBScoreLBasePercentChange:
                    sPropertyValue = ind.TotalSBScoreLBasePercentChange.ToString();
                    break;
                case cTotalSBScoreUAmountChange:
                    sPropertyValue = ind.TotalSBScoreUAmountChange.ToString();
                    break;
                case cTotalSBScoreUPercentChange:
                    sPropertyValue = ind.TotalSBScoreUPercentChange.ToString();
                    break;
                case cTotalSBScoreUBaseChange:
                    sPropertyValue = ind.TotalSBScoreUBaseChange.ToString();
                    break;
                case cTotalSBScoreUBasePercentChange:
                    sPropertyValue = ind.TotalSBScoreUBasePercentChange.ToString();
                    break;
                case cTotalSB1AmountChange:
                    sPropertyValue = ind.TotalSB1AmountChange.ToString();
                    break;
                case cTotalSB1PercentChange:
                    sPropertyValue = ind.TotalSB1PercentChange.ToString();
                    break;
                case cTotalSB1BaseChange:
                    sPropertyValue = ind.TotalSB1BaseChange.ToString();
                    break;
                case cTotalSB1BasePercentChange:
                    sPropertyValue = ind.TotalSB1BasePercentChange.ToString();
                    break;
                case cTotalSB2AmountChange:
                    sPropertyValue = ind.TotalSB2AmountChange.ToString();
                    break;
                case cTotalSB2PercentChange:
                    sPropertyValue = ind.TotalSB2PercentChange.ToString();
                    break;
                case cTotalSB2BaseChange:
                    sPropertyValue = ind.TotalSB2BaseChange.ToString();
                    break;
                case cTotalSB2BasePercentChange:
                    sPropertyValue = ind.TotalSB2BasePercentChange.ToString();
                    break;
               case cTotalSB3AmountChange:
                    sPropertyValue = ind.TotalSB3AmountChange.ToString();
                    break;
                case cTotalSB3PercentChange:
                    sPropertyValue = ind.TotalSB3PercentChange.ToString();
                    break;
                case cTotalSB3BaseChange:
                    sPropertyValue = ind.TotalSB3BaseChange.ToString();
                    break;
                case cTotalSB3BasePercentChange:
                    sPropertyValue = ind.TotalSB3BasePercentChange.ToString();
                    break;
                case cTotalSB4AmountChange:
                    sPropertyValue = ind.TotalSB4AmountChange.ToString();
                    break;
                case cTotalSB4PercentChange:
                    sPropertyValue = ind.TotalSB4PercentChange.ToString();
                    break;
                case cTotalSB4BaseChange:
                    sPropertyValue = ind.TotalSB4BaseChange.ToString();
                    break;
                case cTotalSB4BasePercentChange:
                    sPropertyValue = ind.TotalSB4BasePercentChange.ToString();
                    break;
                case cTotalSB5AmountChange:
                    sPropertyValue = ind.TotalSB5AmountChange.ToString();
                    break;
                case cTotalSB5PercentChange:
                    sPropertyValue = ind.TotalSB5PercentChange.ToString();
                    break;
                case cTotalSB5BaseChange:
                    sPropertyValue = ind.TotalSB5BaseChange.ToString();
                    break;
                case cTotalSB5BasePercentChange:
                    sPropertyValue = ind.TotalSB5BasePercentChange.ToString();
                    break;
                case cTotalSB6AmountChange:
                    sPropertyValue = ind.TotalSB6AmountChange.ToString();
                    break;
                case cTotalSB6PercentChange:
                    sPropertyValue = ind.TotalSB6PercentChange.ToString();
                    break;
                case cTotalSB6BaseChange:
                    sPropertyValue = ind.TotalSB6BaseChange.ToString();
                    break;
                case cTotalSB6BasePercentChange:
                    sPropertyValue = ind.TotalSB6BasePercentChange.ToString();
                    break;
                case cTotalSB7AmountChange:
                    sPropertyValue = ind.TotalSB7AmountChange.ToString();
                    break;
                case cTotalSB7PercentChange:
                    sPropertyValue = ind.TotalSB7PercentChange.ToString();
                    break;
                case cTotalSB7BaseChange:
                    sPropertyValue = ind.TotalSB7BaseChange.ToString();
                    break;
                case cTotalSB7BasePercentChange:
                    sPropertyValue = ind.TotalSB7BasePercentChange.ToString();
                    break;
                case cTotalSB8AmountChange:
                    sPropertyValue = ind.TotalSB8AmountChange.ToString();
                    break;
                case cTotalSB8PercentChange:
                    sPropertyValue = ind.TotalSB8PercentChange.ToString();
                    break;
                case cTotalSB8BaseChange:
                    sPropertyValue = ind.TotalSB8BaseChange.ToString();
                    break;
                case cTotalSB8BasePercentChange:
                    sPropertyValue = ind.TotalSB8BasePercentChange.ToString();
                    break;
               case cTotalSB9AmountChange:
                    sPropertyValue = ind.TotalSB9AmountChange.ToString();
                    break;
                case cTotalSB9PercentChange:
                    sPropertyValue = ind.TotalSB9PercentChange.ToString();
                    break;
                case cTotalSB9BaseChange:
                    sPropertyValue = ind.TotalSB9BaseChange.ToString();
                    break;
                case cTotalSB9BasePercentChange:
                    sPropertyValue = ind.TotalSB9BasePercentChange.ToString();
                    break;
                case cTotalSB10AmountChange:
                    sPropertyValue = ind.TotalSB10AmountChange.ToString();
                    break;
                case cTotalSB10PercentChange:
                    sPropertyValue = ind.TotalSB10PercentChange.ToString();
                    break;
                case cTotalSB10BaseChange:
                    sPropertyValue = ind.TotalSB10BaseChange.ToString();
                    break;
                case cTotalSB10BasePercentChange:
                    sPropertyValue = ind.TotalSB10BasePercentChange.ToString();
                    break;
                case cTotalSB11AmountChange:
                    sPropertyValue = ind.TotalSB11AmountChange.ToString();
                    break;
                case cTotalSB11PercentChange:
                    sPropertyValue = ind.TotalSB11PercentChange.ToString();
                    break;
                case cTotalSB11BaseChange:
                    sPropertyValue = ind.TotalSB11BaseChange.ToString();
                    break;
                case cTotalSB11BasePercentChange:
                    sPropertyValue = ind.TotalSB11BasePercentChange.ToString();
                    break;
                case cTotalSB12AmountChange:
                    sPropertyValue = ind.TotalSB12AmountChange.ToString();
                    break;
                case cTotalSB12PercentChange:
                    sPropertyValue = ind.TotalSB12PercentChange.ToString();
                    break;
                case cTotalSB12BaseChange:
                    sPropertyValue = ind.TotalSB12BaseChange.ToString();
                    break;
                case cTotalSB12BasePercentChange:
                    sPropertyValue = ind.TotalSB12BasePercentChange.ToString();
                    break;
                case cTotalSB13AmountChange:
                    sPropertyValue = ind.TotalSB13AmountChange.ToString();
                    break;
                case cTotalSB13PercentChange:
                    sPropertyValue = ind.TotalSB13PercentChange.ToString();
                    break;
                case cTotalSB13BaseChange:
                    sPropertyValue = ind.TotalSB13BaseChange.ToString();
                    break;
                case cTotalSB13BasePercentChange:
                    sPropertyValue = ind.TotalSB13BasePercentChange.ToString();
                    break;
                case cTotalSB14AmountChange:
                    sPropertyValue = ind.TotalSB14AmountChange.ToString();
                    break;
                case cTotalSB14PercentChange:
                    sPropertyValue = ind.TotalSB14PercentChange.ToString();
                    break;
                case cTotalSB14BaseChange:
                    sPropertyValue = ind.TotalSB14BaseChange.ToString();
                    break;
                case cTotalSB14BasePercentChange:
                    sPropertyValue = ind.TotalSB14BasePercentChange.ToString();
                    break;
                case cTotalSB15AmountChange:
                    sPropertyValue = ind.TotalSB15AmountChange.ToString();
                    break;
                case cTotalSB15PercentChange:
                    sPropertyValue = ind.TotalSB15PercentChange.ToString();
                    break;
                case cTotalSB15BaseChange:
                    sPropertyValue = ind.TotalSB15BaseChange.ToString();
                    break;
                case cTotalSB15BasePercentChange:
                    sPropertyValue = ind.TotalSB15BasePercentChange.ToString();
                    break;
                case cTotalSB16AmountChange:
                    sPropertyValue = ind.TotalSB16AmountChange.ToString();
                    break;
                case cTotalSB16PercentChange:
                    sPropertyValue = ind.TotalSB16PercentChange.ToString();
                    break;
                case cTotalSB16BaseChange:
                    sPropertyValue = ind.TotalSB16BaseChange.ToString();
                    break;
                case cTotalSB16BasePercentChange:
                    sPropertyValue = ind.TotalSB16BasePercentChange.ToString();
                    break;
                case cTotalSB17AmountChange:
                    sPropertyValue = ind.TotalSB17AmountChange.ToString();
                    break;
                case cTotalSB17PercentChange:
                    sPropertyValue = ind.TotalSB17PercentChange.ToString();
                    break;
                case cTotalSB17BaseChange:
                    sPropertyValue = ind.TotalSB17BaseChange.ToString();
                    break;
                case cTotalSB17BasePercentChange:
                    sPropertyValue = ind.TotalSB17BasePercentChange.ToString();
                    break;
                case cTotalSB18AmountChange:
                    sPropertyValue = ind.TotalSB18AmountChange.ToString();
                    break;
                case cTotalSB18PercentChange:
                    sPropertyValue = ind.TotalSB18PercentChange.ToString();
                    break;
                case cTotalSB18BaseChange:
                    sPropertyValue = ind.TotalSB18BaseChange.ToString();
                    break;
                case cTotalSB18BasePercentChange:
                    sPropertyValue = ind.TotalSB18BasePercentChange.ToString();
                    break;case cTotalSB19AmountChange:
                    sPropertyValue = ind.TotalSB19AmountChange.ToString();
                    break;
                case cTotalSB19PercentChange:
                    sPropertyValue = ind.TotalSB19PercentChange.ToString();
                    break;
                case cTotalSB19BaseChange:
                    sPropertyValue = ind.TotalSB19BaseChange.ToString();
                    break;
                case cTotalSB19BasePercentChange:
                    sPropertyValue = ind.TotalSB19BasePercentChange.ToString();
                    break;
                case cTotalSB20AmountChange:
                    sPropertyValue = ind.TotalSB20AmountChange.ToString();
                    break;
                case cTotalSB20PercentChange:
                    sPropertyValue = ind.TotalSB20PercentChange.ToString();
                    break;
                case cTotalSB20BaseChange:
                    sPropertyValue = ind.TotalSB20BaseChange.ToString();
                    break;
                case cTotalSB20BasePercentChange:
                    sPropertyValue = ind.TotalSB20BasePercentChange.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalSB1Change1Attributes(SB1Change1 ind,
            string attNameExtension, XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreAmountChange, attNameExtension), ind.TotalSBScoreAmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScorePercentChange, attNameExtension), ind.TotalSBScorePercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreBaseChange, attNameExtension), ind.TotalSBScoreBaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreBasePercentChange, attNameExtension), ind.TotalSBScoreBasePercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLAmountChange, attNameExtension), ind.TotalSBScoreLAmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLPercentChange, attNameExtension), ind.TotalSBScoreLPercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLBaseChange, attNameExtension), ind.TotalSBScoreLBaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLBasePercentChange, attNameExtension), ind.TotalSBScoreLBasePercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUAmountChange, attNameExtension), ind.TotalSBScoreUAmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUPercentChange, attNameExtension), ind.TotalSBScoreUPercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUBaseChange, attNameExtension), ind.TotalSBScoreUBaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUBasePercentChange, attNameExtension), ind.TotalSBScoreUBasePercentChange);


           CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1AmountChange, attNameExtension), ind.TotalSB1AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1PercentChange, attNameExtension), ind.TotalSB1PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1BaseChange, attNameExtension), ind.TotalSB1BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1BasePercentChange, attNameExtension), ind.TotalSB1BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2AmountChange, attNameExtension), ind.TotalSB2AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2PercentChange, attNameExtension), ind.TotalSB2PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2BaseChange, attNameExtension), ind.TotalSB2BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2BasePercentChange, attNameExtension), ind.TotalSB2BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3AmountChange, attNameExtension), ind.TotalSB3AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3PercentChange, attNameExtension), ind.TotalSB3PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3BaseChange, attNameExtension), ind.TotalSB3BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3BasePercentChange, attNameExtension), ind.TotalSB3BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4AmountChange, attNameExtension), ind.TotalSB4AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4PercentChange, attNameExtension), ind.TotalSB4PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4BaseChange, attNameExtension), ind.TotalSB4BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4BasePercentChange, attNameExtension), ind.TotalSB4BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5AmountChange, attNameExtension), ind.TotalSB5AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5PercentChange, attNameExtension), ind.TotalSB5PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5BaseChange, attNameExtension), ind.TotalSB5BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5BasePercentChange, attNameExtension), ind.TotalSB5BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6AmountChange, attNameExtension), ind.TotalSB6AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6PercentChange, attNameExtension), ind.TotalSB6PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6BaseChange, attNameExtension), ind.TotalSB6BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6BasePercentChange, attNameExtension), ind.TotalSB6BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7AmountChange, attNameExtension), ind.TotalSB7AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7PercentChange, attNameExtension), ind.TotalSB7PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7BaseChange, attNameExtension), ind.TotalSB7BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7BasePercentChange, attNameExtension), ind.TotalSB7BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8AmountChange, attNameExtension), ind.TotalSB8AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8PercentChange, attNameExtension), ind.TotalSB8PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8BaseChange, attNameExtension), ind.TotalSB8BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8BasePercentChange, attNameExtension), ind.TotalSB8BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9AmountChange, attNameExtension), ind.TotalSB9AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9PercentChange, attNameExtension), ind.TotalSB9PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9BaseChange, attNameExtension), ind.TotalSB9BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9BasePercentChange, attNameExtension), ind.TotalSB9BasePercentChange);

           CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10AmountChange, attNameExtension), ind.TotalSB10AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10PercentChange, attNameExtension), ind.TotalSB10PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10BaseChange, attNameExtension), ind.TotalSB10BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10BasePercentChange, attNameExtension), ind.TotalSB10BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11AmountChange, attNameExtension), ind.TotalSB11AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11PercentChange, attNameExtension), ind.TotalSB11PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11BaseChange, attNameExtension), ind.TotalSB11BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11BasePercentChange, attNameExtension), ind.TotalSB11BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12AmountChange, attNameExtension), ind.TotalSB12AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12PercentChange, attNameExtension), ind.TotalSB12PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12BaseChange, attNameExtension), ind.TotalSB12BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12BasePercentChange, attNameExtension), ind.TotalSB12BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13AmountChange, attNameExtension), ind.TotalSB13AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13PercentChange, attNameExtension), ind.TotalSB13PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13BaseChange, attNameExtension), ind.TotalSB13BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13BasePercentChange, attNameExtension), ind.TotalSB13BasePercentChange);

           CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14AmountChange, attNameExtension), ind.TotalSB14AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14PercentChange, attNameExtension), ind.TotalSB14PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14BaseChange, attNameExtension), ind.TotalSB14BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14BasePercentChange, attNameExtension), ind.TotalSB14BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15AmountChange, attNameExtension), ind.TotalSB15AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15PercentChange, attNameExtension), ind.TotalSB15PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15BaseChange, attNameExtension), ind.TotalSB15BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15BasePercentChange, attNameExtension), ind.TotalSB15BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16AmountChange, attNameExtension), ind.TotalSB16AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16PercentChange, attNameExtension), ind.TotalSB16PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16BaseChange, attNameExtension), ind.TotalSB16BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16BasePercentChange, attNameExtension), ind.TotalSB16BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17AmountChange, attNameExtension), ind.TotalSB17AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17PercentChange, attNameExtension), ind.TotalSB17PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17BaseChange, attNameExtension), ind.TotalSB17BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17BasePercentChange, attNameExtension), ind.TotalSB17BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18AmountChange, attNameExtension), ind.TotalSB18AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18PercentChange, attNameExtension), ind.TotalSB18PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18BaseChange, attNameExtension), ind.TotalSB18BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18BasePercentChange, attNameExtension), ind.TotalSB18BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19AmountChange, attNameExtension), ind.TotalSB19AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19PercentChange, attNameExtension), ind.TotalSB19PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19BaseChange, attNameExtension), ind.TotalSB19BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19BasePercentChange, attNameExtension), ind.TotalSB19BasePercentChange);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20AmountChange, attNameExtension), ind.TotalSB20AmountChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20PercentChange, attNameExtension), ind.TotalSB20PercentChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20BaseChange, attNameExtension), ind.TotalSB20BaseChange);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20BasePercentChange, attNameExtension), ind.TotalSB20BasePercentChange);

        }

        public async Task SetTotalSB1Change1AttributesAsync(SB1Change1 ind,
            string attNameExtension, XmlWriter writer)
        {
            //comparative analyses need to know how many rows to display
            int iSBCount = 0;
            if (ind.TSB1ScoreMUnit.Length > 0)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreAmountChange, attNameExtension), string.Empty, ind.TotalSBScoreAmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScorePercentChange, attNameExtension), string.Empty, ind.TotalSBScorePercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreBaseChange, attNameExtension), string.Empty, ind.TotalSBScoreBaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreBasePercentChange, attNameExtension), string.Empty, ind.TotalSBScoreBasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLAmountChange, attNameExtension), string.Empty, ind.TotalSBScoreLAmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLPercentChange, attNameExtension), string.Empty, ind.TotalSBScoreLPercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLBaseChange, attNameExtension), string.Empty, ind.TotalSBScoreLBaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLBasePercentChange, attNameExtension), string.Empty, ind.TotalSBScoreLBasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUAmountChange, attNameExtension), string.Empty, ind.TotalSBScoreUAmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUPercentChange, attNameExtension), string.Empty, ind.TotalSBScoreUPercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUBaseChange, attNameExtension), string.Empty, ind.TotalSBScoreUBaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUBasePercentChange, attNameExtension), string.Empty, ind.TotalSBScoreUBasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label1.Length > 0)
            {
                iSBCount++;
                 await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1AmountChange, attNameExtension), string.Empty, ind.TotalSB1AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1PercentChange, attNameExtension), string.Empty, ind.TotalSB1PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1BaseChange, attNameExtension), string.Empty, ind.TotalSB1BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1BasePercentChange, attNameExtension), string.Empty, ind.TotalSB1BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label2.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2AmountChange, attNameExtension), string.Empty, ind.TotalSB2AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2PercentChange, attNameExtension), string.Empty, ind.TotalSB2PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2BaseChange, attNameExtension), string.Empty, ind.TotalSB2BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2BasePercentChange, attNameExtension), string.Empty, ind.TotalSB2BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label3.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3AmountChange, attNameExtension), string.Empty, ind.TotalSB3AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3PercentChange, attNameExtension), string.Empty, ind.TotalSB3PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3BaseChange, attNameExtension), string.Empty, ind.TotalSB3BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3BasePercentChange, attNameExtension), string.Empty, ind.TotalSB3BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label4.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4AmountChange, attNameExtension), string.Empty, ind.TotalSB4AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4PercentChange, attNameExtension), string.Empty, ind.TotalSB4PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4BaseChange, attNameExtension), string.Empty, ind.TotalSB4BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4BasePercentChange, attNameExtension), string.Empty, ind.TotalSB4BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label5.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5AmountChange, attNameExtension), string.Empty, ind.TotalSB5AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5PercentChange, attNameExtension), string.Empty, ind.TotalSB5PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5BaseChange, attNameExtension), string.Empty, ind.TotalSB5BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5BasePercentChange, attNameExtension), string.Empty, ind.TotalSB5BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label6.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6AmountChange, attNameExtension), string.Empty, ind.TotalSB6AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6PercentChange, attNameExtension), string.Empty, ind.TotalSB6PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6BaseChange, attNameExtension), string.Empty, ind.TotalSB6BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6BasePercentChange, attNameExtension), string.Empty, ind.TotalSB6BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label7.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7AmountChange, attNameExtension), string.Empty, ind.TotalSB7AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7PercentChange, attNameExtension), string.Empty, ind.TotalSB7PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7BaseChange, attNameExtension), string.Empty, ind.TotalSB7BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7BasePercentChange, attNameExtension), string.Empty, ind.TotalSB7BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label8.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8AmountChange, attNameExtension), string.Empty, ind.TotalSB8AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8PercentChange, attNameExtension), string.Empty, ind.TotalSB8PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8BaseChange, attNameExtension), string.Empty, ind.TotalSB8BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8BasePercentChange, attNameExtension), string.Empty, ind.TotalSB8BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label9.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9AmountChange, attNameExtension), string.Empty, ind.TotalSB9AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9PercentChange, attNameExtension), string.Empty, ind.TotalSB9PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9BaseChange, attNameExtension), string.Empty, ind.TotalSB9BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9BasePercentChange, attNameExtension), string.Empty, ind.TotalSB9BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label10.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10AmountChange, attNameExtension), string.Empty, ind.TotalSB10AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10PercentChange, attNameExtension), string.Empty, ind.TotalSB10PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10BaseChange, attNameExtension), string.Empty, ind.TotalSB10BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10BasePercentChange, attNameExtension), string.Empty, ind.TotalSB10BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label11.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11AmountChange, attNameExtension), string.Empty, ind.TotalSB11AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11PercentChange, attNameExtension), string.Empty, ind.TotalSB11PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11BaseChange, attNameExtension), string.Empty, ind.TotalSB11BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11BasePercentChange, attNameExtension), string.Empty, ind.TotalSB11BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label12.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12AmountChange, attNameExtension), string.Empty, ind.TotalSB12AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12PercentChange, attNameExtension), string.Empty, ind.TotalSB12PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12BaseChange, attNameExtension), string.Empty, ind.TotalSB12BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12BasePercentChange, attNameExtension), string.Empty, ind.TotalSB12BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label13.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13AmountChange, attNameExtension), string.Empty, ind.TotalSB13AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13PercentChange, attNameExtension), string.Empty, ind.TotalSB13PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13BaseChange, attNameExtension), string.Empty, ind.TotalSB13BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13BasePercentChange, attNameExtension), string.Empty, ind.TotalSB13BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label14.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14AmountChange, attNameExtension), string.Empty, ind.TotalSB14AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14PercentChange, attNameExtension), string.Empty, ind.TotalSB14PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14BaseChange, attNameExtension), string.Empty, ind.TotalSB14BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14BasePercentChange, attNameExtension), string.Empty, ind.TotalSB14BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label15.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15AmountChange, attNameExtension), string.Empty, ind.TotalSB15AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15PercentChange, attNameExtension), string.Empty, ind.TotalSB15PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15BaseChange, attNameExtension), string.Empty, ind.TotalSB15BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15BasePercentChange, attNameExtension), string.Empty, ind.TotalSB15BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label16.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16AmountChange, attNameExtension), string.Empty, ind.TotalSB16AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16PercentChange, attNameExtension), string.Empty, ind.TotalSB16PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16BaseChange, attNameExtension), string.Empty, ind.TotalSB16BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16BasePercentChange, attNameExtension), string.Empty, ind.TotalSB16BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label17.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17AmountChange, attNameExtension), string.Empty, ind.TotalSB17AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17PercentChange, attNameExtension), string.Empty, ind.TotalSB17PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17BaseChange, attNameExtension), string.Empty, ind.TotalSB17BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17BasePercentChange, attNameExtension), string.Empty, ind.TotalSB17BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label18.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18AmountChange, attNameExtension), string.Empty, ind.TotalSB18AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18PercentChange, attNameExtension), string.Empty, ind.TotalSB18PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18BaseChange, attNameExtension), string.Empty, ind.TotalSB18BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18BasePercentChange, attNameExtension), string.Empty, ind.TotalSB18BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label19.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19AmountChange, attNameExtension), string.Empty, ind.TotalSB19AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19PercentChange, attNameExtension), string.Empty, ind.TotalSB19PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19BaseChange, attNameExtension), string.Empty, ind.TotalSB19BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19BasePercentChange, attNameExtension), string.Empty, ind.TotalSB19BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label20.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20AmountChange, attNameExtension), string.Empty, ind.TotalSB20AmountChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20PercentChange, attNameExtension), string.Empty, ind.TotalSB20PercentChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20BaseChange, attNameExtension), string.Empty, ind.TotalSB20BaseChange.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20BasePercentChange, attNameExtension), string.Empty, ind.TotalSB20BasePercentChange.ToString("N4", CultureInfo.InvariantCulture));
            }
            this.SBCount = iSBCount;
        }
        #endregion
        //run the analyses for inputs an outputs
        public async Task<bool> RunAnalyses(SB1Stock sb1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (use Total1 object to avoid duplication)
            SB1Total1 total = new SB1Total1(this.CalcParameters);
            //this adds the totals to sb1stock.total1 (not to total)
            bHasAnalyses = await total.RunAnalyses(sb1Stock);
            if (sb1Stock.Total1 != null)
            {
                //copy at least the stock and substock totals from total1 to stat1
                //subprices only if needed in future analyses
                sb1Stock.Change1 = new SB1Change1(this.CalcParameters);
                //need one property set
                sb1Stock.Change1.SubApplicationType = this.CalcParameters.SubApplicationType.ToString();
                bHasAnalyses = CopyTotalIndicatorsToSB1Stock(sb1Stock.Change1, sb1Stock.Total1);
                //CopyTotalToSB1Stock(sb1Stock.Change1, sb1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing change analysis
        public bool RunAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //set calculated changestocks
            List<SB1Stock> changeStocks = new List<SB1Stock>();
            if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                changeStocks = SetIOAnalyses(sb1Stock, calcs);
            }
            else
            {
                string sNodeName = sb1Stock.CalcParameters.CurrentElementNodeName;
                if (sNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || sNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no changes
                    changeStocks = SetTotals(sb1Stock, calcs);
                }
                else if (sNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || sNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //tps with currentnodename set only need nets (inputs minus outputs)
                    //note that only sb1stock is used (not changestocks)
                    changeStocks = SetTotals(sb1Stock, calcs);
                }
                else
                {
                    changeStocks = SetAnalyses(sb1Stock, calcs);
                }
            }
            //add the changestocks to parent stock
            if (changeStocks != null)
            {
                bHasAnalyses = AddChangeStocksToBaseStock(sb1Stock, changeStocks);
                //sb1Stock must still add the members of change1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }

        private List<SB1Stock> SetTotals(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            List<SB1Stock> changeStocks = new List<SB1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of sb1stocks for each input and output
            //object model is calc.Total1.SB101Stocks
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Change1 != null)
                        {
                            string sNodeName = sb1Stock.CalcParameters.CurrentElementNodeName;
                            //tps start substracting outcomes from op/comps
                            if (sNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                || sNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                            {
                                //add stock2 (outputs) to stock1 (inputs) and don't add outcomes stock to collection
                                stock.Change1.SubApplicationType = stock.SubApplicationType;
                                bHasTotals = AddSubTotalToTotalStock(sb1Stock.Change1, stock.Multiplier,
                                        stock.Change1);
                                if (bHasTotals
                                    && stock.SubApplicationType != Constants.SUBAPPLICATION_TYPES.outcomeprices.ToString())
                                {
                                    changeStocks.Add(sb1Stock);
                                }
                            }
                            else
                            {
                                //calc holds an input or output stock
                                //add that stock to sb1stock (some analyses will need to use subprices too)
                                bHasTotals = AddSubTotalToTotalStock(sb1Stock.Change1, stock.Multiplier, stock.Change1);
                                if (bHasTotals)
                                {
                                    changeStocks.Add(sb1Stock);
                                }
                            }
                        }
                    }
                }
            }
            return changeStocks;
        }
        private List<SB1Stock> SetAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            List<SB1Stock> changeStocks = new List<SB1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of sb1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Change1.SB101Stocks for costs and 2s for benefits
            //set N
            int iQN = 0;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation; alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            foreach (var calcbyalt in calcsByAlt2)
            {
                //set the calc totals in each observation
                SB1Stock observationStock = new SB1Stock();
                observationStock.Change1 = new SB1Change1(this.CalcParameters);
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(sb1Stock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Change1 != null)
                            {
                                SB1Stock observation2Stock = new SB1Stock();
                                //168 need calc.Mults not agg.Mults
                                //stock.Multiplier = sb1Stock.Multiplier;
                                bHasTotals = SetObservationStock(changeStocks, sb1Stock,
                                    stock, observation2Stock);
                                if (bHasTotals)
                                {
                                    //add to the stats collection
                                    changeStocks.Add(observation2Stock);
                                    //N is determined from the stocks
                                    iQN++;
                                }
                            }
                        }
                    }
                }
            }
            //188 allows analyzers to run algos
            if (!string.IsNullOrEmpty(sb1Stock.MathSubType)
                && sb1Stock.MathSubType != Calculator1.MATH_SUBTYPES.none.ToString())
            {
                if (iQN > 0)
                {
                    bHasTotals = SetAlgoAnalyses(changeStocks, sb1Stock, iQN);
                }
            }
            else
            {
                if (iQN > 0)
                {
                    bHasTotals = SetChangesAnalysis(changeStocks, sb1Stock, iQN);
                }
            }
            return changeStocks;
        }
        private List<SB1Stock> SetIOAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            List<SB1Stock> changeStocks = new List<SB1Stock>();
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iQN2 = 0;
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        //stock.Change1 holds the initial substock/price totals
                        if (stock.Change1 != null)
                        {
                            SB1Stock observation2Stock = new SB1Stock();
                            stock.Multiplier = sb1Stock.Multiplier;
                            bHasTotals = SetObservationStock(changeStocks, sb1Stock,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                changeStocks.Add(observation2Stock);
                                //N is determined from the stocks
                                iQN2++;
                            }
                        }
                    }
                }
            }
            //188 allows analyzers to run algos
            if (!string.IsNullOrEmpty(sb1Stock.MathType)
                && sb1Stock.MathSubType != Calculator1.MATH_SUBTYPES.none.ToString())
            {
                if (iQN2 > 0)
                {
                    bHasTotals = SetAlgoAnalyses(changeStocks, sb1Stock, iQN2);
                }
            }
            else
            {
                if (iQN2 > 0)
                {
                    bHasTotals = SetChangesAnalysis(changeStocks, sb1Stock, iQN2);
                }
            }
            return changeStocks;
        }
        private bool SetAlgoAnalyses(List<SB1Stock> changeStocks, SB1Stock sb1Stock, int qn)
        {
            bool bHasTotals = false;
            //188 allows analyzers to run algos
            if (sb1Stock.MathType == Calculator1.MATH_TYPES.algorithm1.ToString()
                && sb1Stock.MathSubType == Calculator1.MATH_SUBTYPES.subalgorithm8.ToString())
            {
                SB1Base analyzerStock = new SB1Base();
                analyzerStock.CopyCalculatorProperties(sb1Stock);
                //sb1base uses sb1 props in all calcs, move calc1.Maths to analyzerStock.ScoreMaths
                analyzerStock.CopyCalculatorMathToScoreMath();

                //188 moved all orderbys to the analyzer base elements (for uniform display)
                
                analyzerStock.MathResult = string.Empty;
                Task<string> Task1 = analyzerStock.ProcessAlgosForAnalyzersAsync(changeStocks);
                //transfer mathresult back to sb1stock for consistency, but maths don't get displayed in analysis doc 
                sb1Stock.MathResult = analyzerStock.MathResult;
                if (!string.IsNullOrEmpty(analyzerStock.ErrorMessage))
                {
                    sb1Stock.CalculatorDescription += analyzerStock.ErrorMessage;
                    bHasTotals = false;
                }
                else
                {
                    //move the data from the analyzerStock to change stocks
                    bHasTotals = SetMeanChangesAnalysis(changeStocks, sb1Stock, analyzerStock);
                }
            }
            return bHasTotals;
        }
        private bool SetObservationStock(List<SB1Stock> changeStocks,
            SB1Stock sb1Stock, SB1Stock stock, SB1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Change1 = new SB1Change1(this.CalcParameters);
            //188 added datasets and math to calculator that must be copied to obsstock
            observation2Stock.CopyCalculatorProperties(stock);
            observation2Stock.CopyData(stock);
            //rest was before 188
            observation2Stock.Id = stock.Id;
            observation2Stock.Change1.Id = stock.Id;
            //copy some stock props to progress1
            BISB1StockAnalyzerAsync.CopyBaseElementProperties(stock, observation2Stock.Change1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BISB1StockAnalyzerAsync.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Change1.CalcParameters == null)
                stock.Change1.CalcParameters = new CalculatorParameters();
            stock.Change1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //at oc and outcome level no aggregating by year, id or alt
            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Change1, stock.Multiplier, stock.Change1);
            return bHasTotals;
        }
        private bool SetChangesAnalysis(List<SB1Stock> changeStocks, SB1Stock sb1Stock,
            int qN)
        {
            bool bHasTotals = false;
            //set the total observations total
            bool bHasCurrents = changeStocks.Any(c => c.ChangeType == Calculator1.CHANGE_TYPES.current.ToString());
            foreach (var stat in changeStocks)
            {
                //only current gets added to parent cumulative totals
                if (stat.ChangeType == Calculator1.CHANGE_TYPES.current.ToString())
                {
                    bHasTotals = AddSubTotalToTotalStock(sb1Stock.Change1, 1, stat.Change1);
                }
                else
                {
                    if (!bHasCurrents)
                    {
                        //no changes?, straight totals needed
                        bHasTotals = AddSubTotalToTotalStock(sb1Stock.Change1, 1, stat.Change1);
                    }
                }
            }
            if (qN > 0)
            {
                //if any changestock has this property, it's trying to compare antecedents, rather than siblings
                if (bHasCurrents)
                {
                    //budgets uses antecendent, rather than sibling, comparators
                    SetSB1ScoreBudgetChanges(sb1Stock, changeStocks);
                    SetSB1ScoreLBudgetChanges(sb1Stock, changeStocks);
                    SetSB1ScoreUBudgetChanges(sb1Stock, changeStocks);
                    SetSB1BudgetChanges(sb1Stock, changeStocks);
                    SetSB2BudgetChanges(sb1Stock, changeStocks);
                    SetSB3BudgetChanges(sb1Stock, changeStocks);
                    SetSB4BudgetChanges(sb1Stock, changeStocks);
                    SetSB5BudgetChanges(sb1Stock, changeStocks);
                    SetSB6BudgetChanges(sb1Stock, changeStocks);
                    SetSB7BudgetChanges(sb1Stock, changeStocks);
                    SetSB8BudgetChanges(sb1Stock, changeStocks);
                    SetSB9BudgetChanges(sb1Stock, changeStocks);
                    SetSB10BudgetChanges(sb1Stock, changeStocks);
                    SetSB11BudgetChanges(sb1Stock, changeStocks);
                    SetSB12BudgetChanges(sb1Stock, changeStocks);
                    SetSB13BudgetChanges(sb1Stock, changeStocks);
                    SetSB14BudgetChanges(sb1Stock, changeStocks);
                    SetSB15BudgetChanges(sb1Stock, changeStocks);
                    SetSB16BudgetChanges(sb1Stock, changeStocks);
                    SetSB17BudgetChanges(sb1Stock, changeStocks);
                    SetSB18BudgetChanges(sb1Stock, changeStocks);
                    SetSB19BudgetChanges(sb1Stock, changeStocks);
                    SetSB20BudgetChanges(sb1Stock, changeStocks);
                }
                else
                {
                    //set change numbers
                    SetSBScoreChanges(sb1Stock, changeStocks);
                    SetSBScoreLChanges(sb1Stock, changeStocks);
                    SetSBScoreUChanges(sb1Stock, changeStocks);
                    SetSB1Changes(sb1Stock, changeStocks);
                    SetSB2Changes(sb1Stock, changeStocks);
                    SetSB3Changes(sb1Stock, changeStocks);
                    SetSB4Changes(sb1Stock, changeStocks);
                    SetSB5Changes(sb1Stock, changeStocks);
                    SetSB6Changes(sb1Stock, changeStocks);
                    SetSB7Changes(sb1Stock, changeStocks);
                    SetSB8Changes(sb1Stock, changeStocks);
                    SetSB9Changes(sb1Stock, changeStocks);
                    SetSB10Changes(sb1Stock, changeStocks);
                    SetSB11Changes(sb1Stock, changeStocks);
                    SetSB12Changes(sb1Stock, changeStocks);
                    SetSB13Changes(sb1Stock, changeStocks);
                    SetSB14Changes(sb1Stock, changeStocks);
                    SetSB15Changes(sb1Stock, changeStocks);
                    SetSB16Changes(sb1Stock, changeStocks);
                    SetSB17Changes(sb1Stock, changeStocks);
                    SetSB18Changes(sb1Stock, changeStocks);
                    SetSB19Changes(sb1Stock, changeStocks);
                    SetSB20Changes(sb1Stock, changeStocks);
                }
                bHasTotals = true;
            }
            if (qN > 0)
            {
                //remove the comparators (only display the actual)
                changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.xminus1.ToString());
                changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.baseline.ToString());
            }
            return bHasTotals;
        }
        private bool SetMeanChangesAnalysis(List<SB1Stock> changeStocks, SB1Stock sb1Stock,
            SB1Base analyzerStock)
        {
            bool bHasTotals = false;
            //set the total observations total
            //same as regular, because same pattern and same scores
            bool bHasCurrents = changeStocks.Any(c => c.ChangeType == Calculator1.CHANGE_TYPES.current.ToString());
            foreach (var stat in changeStocks)
            {
                //only current gets added to parent cumulative totals
                if (stat.ChangeType == Calculator1.CHANGE_TYPES.current.ToString())
                {
                    bHasTotals = AddSubTotalToTotalStock(sb1Stock.Change1, 1, stat.Change1);
                }
                else
                {
                    if (!bHasCurrents)
                    {
                        //no changes?, straight totals needed
                        bHasTotals = AddSubTotalToTotalStock(sb1Stock.Change1, 1, stat.Change1);
                    }
                }
            }
            //if any changestock has this property, it's trying to compare antecedents, rather than siblings
            if (bHasCurrents)
            {
                //budgets uses antecendent, rather than sibling, comparators
                //need the same scores
                SetSB1ScoreBudgetChanges(sb1Stock, changeStocks);
                SetSB1ScoreLBudgetChanges(sb1Stock, changeStocks);
                SetSB1ScoreUBudgetChanges(sb1Stock, changeStocks);
                //algos don't use this pattern
                //SetSB1MeanBudgetChanges(sb1Stock, changeStocks);
                //because the data has to come from an algo and the algo does the analysis
                SetSB1MeanChanges(analyzerStock, changeStocks);
                SetSB2MeanChanges(analyzerStock, changeStocks);
                SetSB3MeanChanges(analyzerStock, changeStocks);
                SetSB4MeanChanges(analyzerStock, changeStocks);
                SetSB5MeanChanges(analyzerStock, changeStocks);
                SetSB6MeanChanges(analyzerStock, changeStocks);
                SetSB7MeanChanges(analyzerStock, changeStocks);
                SetSB8MeanChanges(analyzerStock, changeStocks);
                SetSB9MeanChanges(analyzerStock, changeStocks);
                SetSB10MeanChanges(analyzerStock, changeStocks);
                SetSB11MeanChanges(analyzerStock, changeStocks);
                SetSB12MeanChanges(analyzerStock, changeStocks);
                SetSB13MeanChanges(analyzerStock, changeStocks);
                SetSB14MeanChanges(analyzerStock, changeStocks);
                SetSB15MeanChanges(analyzerStock, changeStocks);
                SetSB16MeanChanges(analyzerStock, changeStocks);
                SetSB17MeanChanges(analyzerStock, changeStocks);
                SetSB18MeanChanges(analyzerStock, changeStocks);
                SetSB19MeanChanges(analyzerStock, changeStocks);
                SetSB20MeanChanges(analyzerStock, changeStocks);
            }
            else
            {
                //set change numbers
                SetSBScoreChanges(sb1Stock, changeStocks);
                SetSBScoreLChanges(sb1Stock, changeStocks);
                SetSBScoreUChanges(sb1Stock, changeStocks);
                SetSB1MeanChanges(analyzerStock, changeStocks);
                SetSB2MeanChanges(analyzerStock, changeStocks);
                SetSB3MeanChanges(analyzerStock, changeStocks);
                SetSB4MeanChanges(analyzerStock, changeStocks);
                SetSB5MeanChanges(analyzerStock, changeStocks);
                SetSB6MeanChanges(analyzerStock, changeStocks);
                SetSB7MeanChanges(analyzerStock, changeStocks);
                SetSB8MeanChanges(analyzerStock, changeStocks);
                SetSB9MeanChanges(analyzerStock, changeStocks);
                SetSB10MeanChanges(analyzerStock, changeStocks);
                SetSB11MeanChanges(analyzerStock, changeStocks);
                SetSB12MeanChanges(analyzerStock, changeStocks);
                SetSB13MeanChanges(analyzerStock, changeStocks);
                SetSB14MeanChanges(analyzerStock, changeStocks);
                SetSB15MeanChanges(analyzerStock, changeStocks);
                SetSB16MeanChanges(analyzerStock, changeStocks);
                SetSB17MeanChanges(analyzerStock, changeStocks);
                SetSB18MeanChanges(analyzerStock, changeStocks);
                SetSB19MeanChanges(analyzerStock, changeStocks);
                SetSB20MeanChanges(analyzerStock, changeStocks);
            }
            bHasTotals = true;
            //remove the comparators (only display the actual)
            changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.xminus1.ToString());
            changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.baseline.ToString());
            return bHasTotals;
        }
        private static void SetSB1ScoreBudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSBScoreBaseChange = stat.Change1.TSB1ScoreM - benchmark.Change1.TSB1ScoreM;
                        stat.Change1.TotalSBScoreBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSBScoreBaseChange, benchmark.Change1.TSB1ScoreM);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSBScoreAmountChange
                            = stat.Change1.TSB1ScoreM - xminus1.Change1.TSB1ScoreM;
                        stat.Change1.TotalSBScorePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1AmountChange, xminus1.Change1.TSB1ScoreM);
                    }
                }
            }
        }
        private static void SetSB1ScoreLBudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSBScoreLBaseChange = stat.Change1.TSB1ScoreLAmount - benchmark.Change1.TSB1ScoreLAmount;
                        stat.Change1.TotalSBScoreLBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSBScoreLBaseChange, benchmark.Change1.TSB1ScoreLAmount);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSBScoreLAmountChange
                            = stat.Change1.TSB1ScoreLAmount - xminus1.Change1.TSB1ScoreLAmount;
                        stat.Change1.TotalSBScoreLPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1AmountChange, xminus1.Change1.TSB1ScoreLAmount);
                    }
                }
            }
        }
        private static void SetSB1ScoreUBudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSBScoreUBaseChange = stat.Change1.TSB1ScoreUAmount - benchmark.Change1.TSB1ScoreUAmount;
                        stat.Change1.TotalSBScoreUBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSBScoreUBaseChange, benchmark.Change1.TSB1ScoreUAmount);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSBScoreUAmountChange
                            = stat.Change1.TSB1ScoreUAmount - xminus1.Change1.TSB1ScoreUAmount;
                        stat.Change1.TotalSBScoreUPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1AmountChange, xminus1.Change1.TSB1ScoreUAmount);
                    }
                }
            }
        }
        private static void SetSB1BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB1BaseChange = stat.Change1.TSB1TMAmount1 - benchmark.Change1.TSB1TMAmount1;
                        stat.Change1.TotalSB1BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1BaseChange, benchmark.Change1.TSB1TMAmount1);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB1AmountChange
                            = stat.Change1.TSB1TMAmount1 - xminus1.Change1.TSB1TMAmount1;
                        stat.Change1.TotalSB1PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1AmountChange, xminus1.Change1.TSB1TMAmount1);
                    }
                }
            }
        }
        private static void SetSB2BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB2BaseChange = stat.Change1.TSB1TMAmount2 - benchmark.Change1.TSB1TMAmount2;
                        stat.Change1.TotalSB2BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB2BaseChange, benchmark.Change1.TSB1TMAmount2);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB2AmountChange
                            = stat.Change1.TSB1TMAmount2 - xminus1.Change1.TSB1TMAmount2;
                        stat.Change1.TotalSB2PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB2AmountChange, xminus1.Change1.TSB1TMAmount2);
                    }
                }
            }
        }
        private static void SetSB3BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB3BaseChange = stat.Change1.TSB1TMAmount3 - benchmark.Change1.TSB1TMAmount3;
                        stat.Change1.TotalSB3BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB3BaseChange, benchmark.Change1.TSB1TMAmount3);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB3AmountChange
                            = stat.Change1.TSB1TMAmount3 - xminus1.Change1.TSB1TMAmount3;
                        stat.Change1.TotalSB3PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB3AmountChange, xminus1.Change1.TSB1TMAmount3);
                    }
                }
            }
        }
        private static void SetSB4BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB4BaseChange = stat.Change1.TSB1TMAmount4 - benchmark.Change1.TSB1TMAmount4;
                        stat.Change1.TotalSB4BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB4BaseChange, benchmark.Change1.TSB1TMAmount4);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB4AmountChange
                            = stat.Change1.TSB1TMAmount4 - xminus1.Change1.TSB1TMAmount4;
                        stat.Change1.TotalSB4PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB4AmountChange, xminus1.Change1.TSB1TMAmount4);
                    }
                }
            }
        }
        private static void SetSB5BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB5BaseChange = stat.Change1.TSB1TMAmount5 - benchmark.Change1.TSB1TMAmount5;
                        stat.Change1.TotalSB5BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB5BaseChange, benchmark.Change1.TSB1TMAmount5);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB5AmountChange
                            = stat.Change1.TSB1TMAmount5 - xminus1.Change1.TSB1TMAmount5;
                        stat.Change1.TotalSB5PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB5AmountChange, xminus1.Change1.TSB1TMAmount5);
                    }
                }
            }
        }
        private static void SetSB6BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB6BaseChange = stat.Change1.TSB1TMAmount6 - benchmark.Change1.TSB1TMAmount6;
                        stat.Change1.TotalSB6BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB6BaseChange, benchmark.Change1.TSB1TMAmount6);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB6AmountChange
                            = stat.Change1.TSB1TMAmount6 - xminus1.Change1.TSB1TMAmount6;
                        stat.Change1.TotalSB6PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB6AmountChange, xminus1.Change1.TSB1TMAmount6);
                    }
                }
            }
        }
        private static void SetSB7BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB7BaseChange = stat.Change1.TSB1TMAmount7 - benchmark.Change1.TSB1TMAmount7;
                        stat.Change1.TotalSB7BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB7BaseChange, benchmark.Change1.TSB1TMAmount7);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB7AmountChange
                            = stat.Change1.TSB1TMAmount7 - xminus1.Change1.TSB1TMAmount7;
                        stat.Change1.TotalSB7PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB7AmountChange, xminus1.Change1.TSB1TMAmount7);
                    }
                }
            }
        }
        private static void SetSB8BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB8BaseChange = stat.Change1.TSB1TMAmount8 - benchmark.Change1.TSB1TMAmount8;
                        stat.Change1.TotalSB8BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB8BaseChange, benchmark.Change1.TSB1TMAmount8);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB8AmountChange
                            = stat.Change1.TSB1TMAmount8 - xminus1.Change1.TSB1TMAmount8;
                        stat.Change1.TotalSB8PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB8AmountChange, xminus1.Change1.TSB1TMAmount8);
                    }
                }
            }
        }
        private static void SetSB9BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB9BaseChange = stat.Change1.TSB1TMAmount9 - benchmark.Change1.TSB1TMAmount9;
                        stat.Change1.TotalSB9BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB9BaseChange, benchmark.Change1.TSB1TMAmount9);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB9AmountChange
                            = stat.Change1.TSB1TMAmount9 - xminus1.Change1.TSB1TMAmount9;
                        stat.Change1.TotalSB9PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB9AmountChange, xminus1.Change1.TSB1TMAmount9);
                    }
                }
            }
        }
        private static void SetSB10BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB10BaseChange = stat.Change1.TSB1TMAmount10 - benchmark.Change1.TSB1TMAmount10;
                        stat.Change1.TotalSB10BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB10BaseChange, benchmark.Change1.TSB1TMAmount10);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB10AmountChange
                            = stat.Change1.TSB1TMAmount10 - xminus1.Change1.TSB1TMAmount10;
                        stat.Change1.TotalSB10PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB10AmountChange, xminus1.Change1.TSB1TMAmount10);
                    }
                }
            }
        }
        private static void SetSB11BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB11BaseChange = stat.Change1.TSB1TMAmount11 - benchmark.Change1.TSB1TMAmount11;
                        stat.Change1.TotalSB11BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB11BaseChange, benchmark.Change1.TSB1TMAmount11);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB11AmountChange
                            = stat.Change1.TSB1TMAmount11 - xminus1.Change1.TSB1TMAmount11;
                        stat.Change1.TotalSB11PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB11AmountChange, xminus1.Change1.TSB1TMAmount11);
                    }
                }
            }
        }
        private static void SetSB12BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB12BaseChange = stat.Change1.TSB1TMAmount12 - benchmark.Change1.TSB1TMAmount12;
                        stat.Change1.TotalSB12BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB12BaseChange, benchmark.Change1.TSB1TMAmount12);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB12AmountChange
                            = stat.Change1.TSB1TMAmount12 - xminus1.Change1.TSB1TMAmount12;
                        stat.Change1.TotalSB12PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB12AmountChange, xminus1.Change1.TSB1TMAmount12);
                    }
                }
            }
        }
        private static void SetSB13BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB13BaseChange = stat.Change1.TSB1TMAmount13 - benchmark.Change1.TSB1TMAmount13;
                        stat.Change1.TotalSB13BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB13BaseChange, benchmark.Change1.TSB1TMAmount13);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB13AmountChange
                            = stat.Change1.TSB1TMAmount13 - xminus1.Change1.TSB1TMAmount13;
                        stat.Change1.TotalSB13PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB13AmountChange, xminus1.Change1.TSB1TMAmount13);
                    }
                }
            }
        }
        private static void SetSB14BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB14BaseChange = stat.Change1.TSB1TMAmount14 - benchmark.Change1.TSB1TMAmount14;
                        stat.Change1.TotalSB14BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB14BaseChange, benchmark.Change1.TSB1TMAmount14);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB14AmountChange
                            = stat.Change1.TSB1TMAmount14 - xminus1.Change1.TSB1TMAmount14;
                        stat.Change1.TotalSB14PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB14AmountChange, xminus1.Change1.TSB1TMAmount14);
                    }
                }
            }
        }
        private static void SetSB15BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB15BaseChange = stat.Change1.TSB1TMAmount15 - benchmark.Change1.TSB1TMAmount15;
                        stat.Change1.TotalSB15BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB15BaseChange, benchmark.Change1.TSB1TMAmount15);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB15AmountChange
                            = stat.Change1.TSB1TMAmount15 - xminus1.Change1.TSB1TMAmount15;
                        stat.Change1.TotalSB15PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB15AmountChange, xminus1.Change1.TSB1TMAmount15);
                    }
                }
            }
        }
        private static void SetSB16BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB16BaseChange = stat.Change1.TSB1TMAmount16 - benchmark.Change1.TSB1TMAmount16;
                        stat.Change1.TotalSB16BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB16BaseChange, benchmark.Change1.TSB1TMAmount16);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB16AmountChange
                            = stat.Change1.TSB1TMAmount16 - xminus1.Change1.TSB1TMAmount16;
                        stat.Change1.TotalSB16PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB16AmountChange, xminus1.Change1.TSB1TMAmount16);
                    }
                }
            }
        }
        private static void SetSB17BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB17BaseChange = stat.Change1.TSB1TMAmount17 - benchmark.Change1.TSB1TMAmount17;
                        stat.Change1.TotalSB17BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB17BaseChange, benchmark.Change1.TSB1TMAmount17);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB17AmountChange
                            = stat.Change1.TSB1TMAmount17 - xminus1.Change1.TSB1TMAmount17;
                        stat.Change1.TotalSB17PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB17AmountChange, xminus1.Change1.TSB1TMAmount17);
                    }
                }
            }
        }
        private static void SetSB18BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB18BaseChange = stat.Change1.TSB1TMAmount18 - benchmark.Change1.TSB1TMAmount18;
                        stat.Change1.TotalSB18BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB18BaseChange, benchmark.Change1.TSB1TMAmount18);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB18AmountChange
                            = stat.Change1.TSB1TMAmount18 - xminus1.Change1.TSB1TMAmount18;
                        stat.Change1.TotalSB18PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB18AmountChange, xminus1.Change1.TSB1TMAmount18);
                    }
                }
            }
        }
        private static void SetSB19BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB19BaseChange = stat.Change1.TSB1TMAmount19 - benchmark.Change1.TSB1TMAmount19;
                        stat.Change1.TotalSB19BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB19BaseChange, benchmark.Change1.TSB1TMAmount19);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB19AmountChange
                            = stat.Change1.TSB1TMAmount19 - xminus1.Change1.TSB1TMAmount19;
                        stat.Change1.TotalSB19PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB19AmountChange, xminus1.Change1.TSB1TMAmount19);
                    }
                }
            }
        }
        private static void SetSB20BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    SB1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalSB20BaseChange = stat.Change1.TSB1TMAmount20 - benchmark.Change1.TSB1TMAmount20;
                        stat.Change1.TotalSB20BasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB20BaseChange, benchmark.Change1.TSB1TMAmount20);
                    }
                    //set the xminus change using partialtarget tt
                    SB1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalSB20AmountChange
                            = stat.Change1.TSB1TMAmount20 - xminus1.Change1.TSB1TMAmount20;
                        stat.Change1.TotalSB20PercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalSB20AmountChange, xminus1.Change1.TSB1TMAmount20);
                    }
                }
            }
        }
        
        private static SB1Stock GetChangeStockByLabel(SB1Stock actual, List<int> ids,
            List<SB1Stock> changeStocks, string changeType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            SB1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (changeStocks.Any(p => p.Label == actual.Label
                && p.ChangeType == changeType))
            {
                int iIndex = 1;
                foreach (SB1Stock change in changeStocks)
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
            SB1Stock change, List<SB1Stock> changeStocks, string changeType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (SB1Stock rp in changeStocks)
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
        private static void SetSBScoreChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1ScoreM = 0;
            double dbLastTSB1ScoreM = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1ScoreM = stat.Change1.TSB1ScoreM;
                }
                else
                {
                    if (dbLastTSB1ScoreM != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSBScoreAmountChange = stat.Change1.TSB1ScoreM - dbLastTSB1ScoreM;
                        stat.Change1.TotalSBScorePercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSBScoreAmountChange, dbLastTSB1ScoreM);
                    }
                    dbLastTSB1ScoreM = stat.Change1.TSB1ScoreM;

                    stat.Change1.TotalSBScoreBaseChange = stat.Change1.TSB1ScoreM - dbBaseTSB1ScoreM;
                    stat.Change1.TotalSBScoreBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSBScoreBaseChange, dbBaseTSB1ScoreM);
                }
                i++;
            }
        }
        private static void SetSBScoreLChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1ScoreL = 0;
            double dbLastTSB1ScoreL = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1ScoreL = stat.Change1.TSB1ScoreLAmount;
                }
                else
                {
                    if (dbLastTSB1ScoreL != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSBScoreLAmountChange = stat.Change1.TSB1ScoreLAmount - dbLastTSB1ScoreL;
                        stat.Change1.TotalSBScoreLPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSBScoreLAmountChange, dbLastTSB1ScoreL);
                    }
                    dbLastTSB1ScoreL = stat.Change1.TSB1ScoreLAmount;

                    stat.Change1.TotalSBScoreLBaseChange = stat.Change1.TSB1ScoreLAmount - dbBaseTSB1ScoreL;
                    stat.Change1.TotalSBScoreLBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSBScoreLBaseChange, dbBaseTSB1ScoreL);
                }
                i++;
            }
        }
        private static void SetSBScoreUChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1ScoreU = 0;
            double dbLastTSB1ScoreU = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1ScoreU = stat.Change1.TSB1ScoreUAmount;
                }
                else
                {
                    if (dbLastTSB1ScoreU != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSBScoreUAmountChange = stat.Change1.TSB1ScoreUAmount - dbLastTSB1ScoreU;
                        stat.Change1.TotalSBScoreUPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSBScoreUAmountChange, dbLastTSB1ScoreU);
                    }
                    dbLastTSB1ScoreU = stat.Change1.TSB1ScoreUAmount;

                    stat.Change1.TotalSBScoreUBaseChange = stat.Change1.TSB1ScoreUAmount - dbBaseTSB1ScoreU;
                    stat.Change1.TotalSBScoreUBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSBScoreUBaseChange, dbBaseTSB1ScoreU);
                }
                i++;
            }
        }
        private static void SetSB1Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount1 = 0;
            double dbLastTSB1TMAmount1 = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount1 = stat.Change1.TSB1TMAmount1;
                }
                else
                {
                    if (dbLastTSB1TMAmount1 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB1AmountChange = stat.Change1.TSB1TMAmount1 - dbLastTSB1TMAmount1;
                        stat.Change1.TotalSB1PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB1AmountChange, dbLastTSB1TMAmount1);
                    }
                    dbLastTSB1TMAmount1 = stat.Change1.TSB1TMAmount1;

                    stat.Change1.TotalSB1BaseChange = stat.Change1.TSB1TMAmount1 - dbBaseTSB1TMAmount1;
                    stat.Change1.TotalSB1BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB1BaseChange, dbBaseTSB1TMAmount1);
                }
                i++;
            }
        }
        private static List<List<double>> GetResults(SB1Base analyzerStock, List<SB1Stock> changeStocks) 
        {
            string sLabel = changeStocks[0].TSB1Label1;
            if (string.IsNullOrEmpty(sLabel))
            {
                sLabel = changeStocks[0].Change1.TSB1Label1;
            }
            List<List<double>> results = new List<List<double>>();
            if (analyzerStock.DataToAnalyze.ContainsKey(sLabel))
            {
                results = analyzerStock.DataToAnalyze[sLabel];
            }
            return results;
        }
        
        private static void SetSB2Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount2 = 0;
            double dbLastTSB1TMAmount2 = 0;
            int i = 0;
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount2 = stat.Change1.TSB1TMAmount2;
                }
                else
                {
                    if (dbLastTSB1TMAmount2 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB2AmountChange = stat.Change1.TSB1TMAmount2 - dbLastTSB1TMAmount2;
                        stat.Change1.TotalSB2PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB2AmountChange, dbLastTSB1TMAmount2);
                    }
                    dbLastTSB1TMAmount2 = stat.Change1.TSB1TMAmount2;

                    stat.Change1.TotalSB2BaseChange = stat.Change1.TSB1TMAmount2 - dbBaseTSB1TMAmount2;
                    stat.Change1.TotalSB2BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB2BaseChange, dbBaseTSB1TMAmount2);
                }
                i++;
            }
        }
        private static void SetSB3Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount3 = 0;
            double dbLastTSB1TMAmount3 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount3 = stat.Change1.TSB1TMAmount3;
                }
                else
                {
                    if (dbLastTSB1TMAmount3 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB3AmountChange = stat.Change1.TSB1TMAmount3 - dbLastTSB1TMAmount3;
                        stat.Change1.TotalSB3PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB3AmountChange, dbLastTSB1TMAmount3);
                    }
                    dbLastTSB1TMAmount3 = stat.Change1.TSB1TMAmount3;

                    stat.Change1.TotalSB3BaseChange = stat.Change1.TSB1TMAmount3 - dbBaseTSB1TMAmount3;
                    stat.Change1.TotalSB3BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB3BaseChange, dbBaseTSB1TMAmount3);
                }
                i++;
            }
        }
        private static void SetSB4Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount4 = 0;
            double dbLastTSB1TMAmount4 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount4 = stat.Change1.TSB1TMAmount4;
                }
                else
                {
                    if (dbLastTSB1TMAmount4 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB4AmountChange = stat.Change1.TSB1TMAmount4 - dbLastTSB1TMAmount4;
                        stat.Change1.TotalSB4PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB4AmountChange, dbLastTSB1TMAmount4);
                    }
                    dbLastTSB1TMAmount4 = stat.Change1.TSB1TMAmount4;

                    stat.Change1.TotalSB4BaseChange = stat.Change1.TSB1TMAmount4 - dbBaseTSB1TMAmount4;
                    stat.Change1.TotalSB4BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB4BaseChange, dbBaseTSB1TMAmount4);
                }
                i++;
            }
        }
        private static void SetSB5Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount5 = 0;
            double dbLastTSB1TMAmount5 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount5 = stat.Change1.TSB1TMAmount5;
                }
                else
                {
                    if (dbLastTSB1TMAmount5 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB5AmountChange = stat.Change1.TSB1TMAmount5 - dbLastTSB1TMAmount5;
                        stat.Change1.TotalSB5PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB5AmountChange, dbLastTSB1TMAmount5);
                    }
                    dbLastTSB1TMAmount5 = stat.Change1.TSB1TMAmount5;

                    stat.Change1.TotalSB5BaseChange = stat.Change1.TSB1TMAmount5 - dbBaseTSB1TMAmount5;
                    stat.Change1.TotalSB5BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB5BaseChange, dbBaseTSB1TMAmount5);
                }
                i++;
            }
        }
        private static void SetSB6Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount6 = 0;
            double dbLastTSB1TMAmount6 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount6 = stat.Change1.TSB1TMAmount6;
                }
                else
                {
                    if (dbLastTSB1TMAmount6 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB6AmountChange = stat.Change1.TSB1TMAmount6 - dbLastTSB1TMAmount6;
                        stat.Change1.TotalSB6PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB6AmountChange, dbLastTSB1TMAmount6);
                    }
                    dbLastTSB1TMAmount6 = stat.Change1.TSB1TMAmount6;

                    stat.Change1.TotalSB6BaseChange = stat.Change1.TSB1TMAmount6 - dbBaseTSB1TMAmount6;
                    stat.Change1.TotalSB6BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB6BaseChange, dbBaseTSB1TMAmount6);
                }
                i++;
            }
        }
        private static void SetSB7Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount7 = 0;
            double dbLastTSB1TMAmount7 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount7 = stat.Change1.TSB1TMAmount7;
                }
                else
                {
                    if (dbLastTSB1TMAmount7 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB7AmountChange = stat.Change1.TSB1TMAmount7 - dbLastTSB1TMAmount7;
                        stat.Change1.TotalSB7PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB7AmountChange, dbLastTSB1TMAmount7);
                    }
                    dbLastTSB1TMAmount7 = stat.Change1.TSB1TMAmount7;

                    stat.Change1.TotalSB7BaseChange = stat.Change1.TSB1TMAmount7 - dbBaseTSB1TMAmount7;
                    stat.Change1.TotalSB7BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB7BaseChange, dbBaseTSB1TMAmount7);
                }
                i++;
            }
        }
        private static void SetSB8Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount8 = 0;
            double dbLastTSB1TMAmount8 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount8 = stat.Change1.TSB1TMAmount8;
                }
                else
                {
                    if (dbLastTSB1TMAmount8 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB8AmountChange = stat.Change1.TSB1TMAmount8 - dbLastTSB1TMAmount8;
                        stat.Change1.TotalSB8PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB8AmountChange, dbLastTSB1TMAmount8);
                    }
                    dbLastTSB1TMAmount8 = stat.Change1.TSB1TMAmount8;

                    stat.Change1.TotalSB8BaseChange = stat.Change1.TSB1TMAmount8 - dbBaseTSB1TMAmount8;
                    stat.Change1.TotalSB8BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB8BaseChange, dbBaseTSB1TMAmount8);
                }
                i++;
            }
        }
        private static void SetSB9Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount9 = 0;
            double dbLastTSB1TMAmount9 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount9 = stat.Change1.TSB1TMAmount9;
                }
                else
                {
                    if (dbLastTSB1TMAmount9 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB9AmountChange = stat.Change1.TSB1TMAmount9 - dbLastTSB1TMAmount9;
                        stat.Change1.TotalSB9PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB9AmountChange, dbLastTSB1TMAmount9);
                    }
                    dbLastTSB1TMAmount9 = stat.Change1.TSB1TMAmount9;

                    stat.Change1.TotalSB9BaseChange = stat.Change1.TSB1TMAmount9 - dbBaseTSB1TMAmount9;
                    stat.Change1.TotalSB9BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB9BaseChange, dbBaseTSB1TMAmount9);
                }
                i++;
            }
        }
        private static void SetSB10Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount10 = 0;
            double dbLastTSB1TMAmount10 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount10 = stat.Change1.TSB1TMAmount10;
                }
                else
                {
                    if (dbLastTSB1TMAmount10 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB10AmountChange = stat.Change1.TSB1TMAmount10 - dbLastTSB1TMAmount10;
                        stat.Change1.TotalSB10PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB10AmountChange, dbLastTSB1TMAmount10);
                    }
                    dbLastTSB1TMAmount10 = stat.Change1.TSB1TMAmount10;

                    stat.Change1.TotalSB10BaseChange = stat.Change1.TSB1TMAmount10 - dbBaseTSB1TMAmount10;
                    stat.Change1.TotalSB10BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB10BaseChange, dbBaseTSB1TMAmount10);
                }
                i++;
            }
        }
        private static void SetSB11Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount11 = 0;
            double dbLastTSB1TMAmount11 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount11 = stat.Change1.TSB1TMAmount11;
                }
                else
                {
                    if (dbLastTSB1TMAmount11 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB11AmountChange = stat.Change1.TSB1TMAmount11 - dbLastTSB1TMAmount11;
                        stat.Change1.TotalSB11PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB11AmountChange, dbLastTSB1TMAmount11);
                    }
                    dbLastTSB1TMAmount11 = stat.Change1.TSB1TMAmount11;

                    stat.Change1.TotalSB11BaseChange = stat.Change1.TSB1TMAmount11 - dbBaseTSB1TMAmount11;
                    stat.Change1.TotalSB11BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB11BaseChange, dbBaseTSB1TMAmount11);
                }
                i++;
            }
        }
        private static void SetSB12Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount12 = 0;
            double dbLastTSB1TMAmount12 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount12 = stat.Change1.TSB1TMAmount12;
                }
                else
                {
                    if (dbLastTSB1TMAmount12 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB12AmountChange = stat.Change1.TSB1TMAmount12 - dbLastTSB1TMAmount12;
                        stat.Change1.TotalSB12PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB12AmountChange, dbLastTSB1TMAmount12);
                    }
                    dbLastTSB1TMAmount12 = stat.Change1.TSB1TMAmount12;

                    stat.Change1.TotalSB12BaseChange = stat.Change1.TSB1TMAmount12 - dbBaseTSB1TMAmount12;
                    stat.Change1.TotalSB12BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB12BaseChange, dbBaseTSB1TMAmount12);
                }
                i++;
            }
        }
        private static void SetSB13Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount13 = 0;
            double dbLastTSB1TMAmount13 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount13 = stat.Change1.TSB1TMAmount13;
                }
                else
                {
                    if (dbLastTSB1TMAmount13 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB13AmountChange = stat.Change1.TSB1TMAmount13 - dbLastTSB1TMAmount13;
                        stat.Change1.TotalSB13PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB13AmountChange, dbLastTSB1TMAmount13);
                    }
                    dbLastTSB1TMAmount13 = stat.Change1.TSB1TMAmount13;

                    stat.Change1.TotalSB13BaseChange = stat.Change1.TSB1TMAmount13 - dbBaseTSB1TMAmount13;
                    stat.Change1.TotalSB13BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB13BaseChange, dbBaseTSB1TMAmount13);
                }
                i++;
            }
        }
        private static void SetSB14Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount14 = 0;
            double dbLastTSB1TMAmount14 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount14 = stat.Change1.TSB1TMAmount14;
                }
                else
                {
                    if (dbLastTSB1TMAmount14 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB14AmountChange = stat.Change1.TSB1TMAmount14 - dbLastTSB1TMAmount14;
                        stat.Change1.TotalSB14PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB14AmountChange, dbLastTSB1TMAmount14);
                    }
                    dbLastTSB1TMAmount14 = stat.Change1.TSB1TMAmount14;

                    stat.Change1.TotalSB14BaseChange = stat.Change1.TSB1TMAmount14 - dbBaseTSB1TMAmount14;
                    stat.Change1.TotalSB14BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB14BaseChange, dbBaseTSB1TMAmount14);
                }
                i++;
            }
        }
        private static void SetSB15Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount15 = 0;
            double dbLastTSB1TMAmount15 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount15 = stat.Change1.TSB1TMAmount15;
                }
                else
                {
                    if (dbLastTSB1TMAmount15 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB15AmountChange = stat.Change1.TSB1TMAmount15 - dbLastTSB1TMAmount15;
                        stat.Change1.TotalSB15PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB15AmountChange, dbLastTSB1TMAmount15);
                    }
                    dbLastTSB1TMAmount15 = stat.Change1.TSB1TMAmount15;

                    stat.Change1.TotalSB15BaseChange = stat.Change1.TSB1TMAmount15 - dbBaseTSB1TMAmount15;
                    stat.Change1.TotalSB15BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB15BaseChange, dbBaseTSB1TMAmount15);
                }
                i++;
            }
        }
        private static void SetSB16Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount16 = 0;
            double dbLastTSB1TMAmount16 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount16 = stat.Change1.TSB1TMAmount16;
                }
                else
                {
                    if (dbLastTSB1TMAmount16 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB16AmountChange = stat.Change1.TSB1TMAmount16 - dbLastTSB1TMAmount16;
                        stat.Change1.TotalSB16PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB16AmountChange, dbLastTSB1TMAmount16);
                    }
                    dbLastTSB1TMAmount16 = stat.Change1.TSB1TMAmount16;

                    stat.Change1.TotalSB16BaseChange = stat.Change1.TSB1TMAmount16 - dbBaseTSB1TMAmount16;
                    stat.Change1.TotalSB16BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB16BaseChange, dbBaseTSB1TMAmount16);
                }
                i++;
            }
        }
        private static void SetSB17Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount17 = 0;
            double dbLastTSB1TMAmount17 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount17 = stat.Change1.TSB1TMAmount17;
                }
                else
                {
                    if (dbLastTSB1TMAmount17 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB17AmountChange = stat.Change1.TSB1TMAmount17 - dbLastTSB1TMAmount17;
                        stat.Change1.TotalSB17PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB17AmountChange, dbLastTSB1TMAmount17);
                    }
                    dbLastTSB1TMAmount17 = stat.Change1.TSB1TMAmount17;

                    stat.Change1.TotalSB17BaseChange = stat.Change1.TSB1TMAmount17 - dbBaseTSB1TMAmount17;
                    stat.Change1.TotalSB17BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB17BaseChange, dbBaseTSB1TMAmount17);
                }
                i++;
            }
        }
        private static void SetSB18Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount18 = 0;
            double dbLastTSB1TMAmount18 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount18 = stat.Change1.TSB1TMAmount18;
                }
                else
                {
                    if (dbLastTSB1TMAmount18 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB18AmountChange = stat.Change1.TSB1TMAmount18 - dbLastTSB1TMAmount18;
                        stat.Change1.TotalSB18PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB18AmountChange, dbLastTSB1TMAmount18);
                    }
                    dbLastTSB1TMAmount18 = stat.Change1.TSB1TMAmount18;

                    stat.Change1.TotalSB18BaseChange = stat.Change1.TSB1TMAmount18 - dbBaseTSB1TMAmount18;
                    stat.Change1.TotalSB18BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB18BaseChange, dbBaseTSB1TMAmount18);
                }
                i++;
            }
        }
        private static void SetSB19Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount19 = 0;
            double dbLastTSB1TMAmount19 = 0;
            int i = 0;
           
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount19 = stat.Change1.TSB1TMAmount19;
                }
                else
                {
                    if (dbLastTSB1TMAmount19 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB19AmountChange = stat.Change1.TSB1TMAmount19 - dbLastTSB1TMAmount19;
                        stat.Change1.TotalSB19PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB19AmountChange, dbLastTSB1TMAmount19);
                    }
                    dbLastTSB1TMAmount19 = stat.Change1.TSB1TMAmount19;

                    stat.Change1.TotalSB19BaseChange = stat.Change1.TSB1TMAmount19 - dbBaseTSB1TMAmount19;
                    stat.Change1.TotalSB19BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB19BaseChange, dbBaseTSB1TMAmount19);
                }
                i++;
            }
        }
        private static void SetSB20Changes(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        {
            double dbBaseTSB1TMAmount20 = 0;
            double dbLastTSB1TMAmount20 = 0;
            int i = 0;
            
            foreach (SB1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTSB1TMAmount20 = stat.Change1.TSB1TMAmount20;
                }
                else
                {
                    if (dbLastTSB1TMAmount20 != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalSB20AmountChange = stat.Change1.TSB1TMAmount20 - dbLastTSB1TMAmount20;
                        stat.Change1.TotalSB20PercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB20AmountChange, dbLastTSB1TMAmount20);
                    }
                    dbLastTSB1TMAmount20 = stat.Change1.TSB1TMAmount20;

                    stat.Change1.TotalSB20BaseChange = stat.Change1.TSB1TMAmount20 - dbBaseTSB1TMAmount20;
                    stat.Change1.TotalSB20BasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalSB20BaseChange, dbBaseTSB1TMAmount20);
                }
                i++;
            }
        }
        //private static void SetSB1BudgetChanges(SB1Stock sb1Stock, List<SB1Stock> changeStocks)
        //{
        //    List<int> baseIds = new List<int>();
        //    List<int> xMinus1Ids = new List<int>();
        //    foreach (SB1Stock stat in changeStocks)
        //    {
        //        //actual compared to base comparator and xminus1 comparator
        //        if (stat.ChangeType == CHANGE_TYPES.current.ToString())
        //        {
        //            //set the base change using benchmark tt
        //            SB1Stock benchmark = GetChangeStockByLabel(
        //                stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
        //            if (benchmark != null)
        //            {
        //                stat.Change1.TotalSBScoreBaseChange = stat.Change1.TSB1ScoreM - benchmark.Change1.TSB1ScoreM;
        //                stat.Change1.TotalSBScoreBasePercentChange
        //                    = CalculatorHelpers.GetPercent(stat.Change1.TotalSBScoreBaseChange, benchmark.Change1.TSB1ScoreM);
        //            }
        //            //set the xminus change using partialtarget tt
        //            SB1Stock xminus1 = GetChangeStockByLabel(
        //                stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
        //            if (xminus1 != null)
        //            {
        //                stat.Change1.TotalSBScoreAmountChange
        //                    = stat.Change1.TSB1ScoreM - xminus1.Change1.TSB1ScoreM;
        //                stat.Change1.TotalSBScorePercentChange
        //                    = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1AmountChange, xminus1.Change1.TSB1ScoreM);
        //            }
        //        }
        //    }
        //}
        //private static void SetSB1MeanBudgetChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        //{
        //    List<int> baseIds = new List<int>();
        //    List<int> xMinus1Ids = new List<int>();
        //    int i = 0;
        //    List<List<double>> results = GetResults(analyzerStock, changeStocks);
        //    //the analyses set up the results so that each changestock has a list of doubles containing results
        //    if (results.Count >= changeStocks.Count)
        //    {
        //        foreach (SB1Stock stat in changeStocks)
        //        {
        //            if (results[i].Count >= 5)
        //            {
        //                //actual compared to base comparator and xminus1 comparator
        //                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
        //                {
        //                    //set the base change using benchmark tt
        //                    SB1Stock benchmark = GetChangeStockByLabel(
        //                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
        //                    if (benchmark != null)
        //                    {
        //                        //mean diff
        //                        stat.Change1.TotalSB1BaseChange = results[i].ElementAt(3);
        //                        stat.Change1.TotalSB1BasePercentChange = results[i].ElementAt(4);
        //                        //stat.Change1.TotalSB1BaseChange = stat.Change1.TSB1TMAmount1 - benchmark.Change1.TSB1TMAmount1;
        //                        //stat.Change1.TotalSB1BasePercentChange
        //                        //    = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1BaseChange, benchmark.Change1.TSB1TMAmount1);
        //                    }
        //                    //set the xminus change using partialtarget tt
        //                    SB1Stock xminus1 = GetChangeStockByLabel(
        //                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
        //                    if (xminus1 != null)
        //                    {
        //                        //mean diff
        //                        stat.Change1.TotalSB1AmountChange = results[i].ElementAt(1);
        //                        //plus or minus ci
        //                        stat.Change1.TotalSB1PercentChange = results[i].ElementAt(2);
        //                        //stat.Change1.TotalSB1AmountChange
        //                        //    = stat.Change1.TSB1TMAmount1 - xminus1.Change1.TSB1TMAmount1;
        //                        //stat.Change1.TotalSB1PercentChange
        //                        //    = CalculatorHelpers.GetPercent(stat.Change1.TotalSB1AmountChange, xminus1.Change1.TSB1TMAmount1);
        //                    }
        //                }
        //            }
        //            i++;
        //        }
        //    }
        //}
        private static void SetSB1MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount1 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB1AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB1PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB1BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB1BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount1 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB1AmountChange = 0;
                                stat.Change1.TotalSB1PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB1AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB1PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB1BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB1BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB2MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount2 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB2AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB2PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB2BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB2BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount2 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB2AmountChange = 0;
                                stat.Change1.TotalSB2PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB2AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB2PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB2BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB2BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB3MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount3 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB3AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB3PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB3BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB3BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount3 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB3AmountChange = 0;
                                stat.Change1.TotalSB3PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB3AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB3PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB3BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB3BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB4MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount4 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB4AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB4PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB4BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB4BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount4 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB4AmountChange = 0;
                                stat.Change1.TotalSB4PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB4AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB4PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB4BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB4BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB5MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount5 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB5AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB5PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB5BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB5BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount5 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB5AmountChange = 0;
                                stat.Change1.TotalSB5PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB5AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB5PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB5BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB5BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB6MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount6 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB6AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB6PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB6BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB6BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount6 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB6AmountChange = 0;
                                stat.Change1.TotalSB6PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB6AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB6PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB6BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB6BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB7MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount7 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB7AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB7PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB7BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB7BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount7 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB7AmountChange = 0;
                                stat.Change1.TotalSB7PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB7AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB7PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB7BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB7BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB8MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount8 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB8AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB8PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB8BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB8BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount8 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB8AmountChange = 0;
                                stat.Change1.TotalSB8PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB8AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB8PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB8BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB8BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB9MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount9 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB9AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB9PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB9BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB9BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount9 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB9AmountChange = 0;
                                stat.Change1.TotalSB9PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB9AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB9PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB9BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB9BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB10MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount10 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB10AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB10PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB10BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB10BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount10 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB10AmountChange = 0;
                                stat.Change1.TotalSB10PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB10AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB10PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB10BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB10BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB11MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount11 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB11AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB11PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB11BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB11BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount11 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB11AmountChange = 0;
                                stat.Change1.TotalSB11PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB11AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB11PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB11BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB11BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB12MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount12 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB12AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB12PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB12BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB12BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount12 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB12AmountChange = 0;
                                stat.Change1.TotalSB12PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB12AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB12PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB12BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB12BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB13MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount13 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB13AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB13PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB13BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB13BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount13 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB13AmountChange = 0;
                                stat.Change1.TotalSB13PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB13AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB13PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB13BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB13BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB14MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount14 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB14AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB14PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB14BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB14BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount14 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB14AmountChange = 0;
                                stat.Change1.TotalSB14PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB14AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB14PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB14BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB14BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB15MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount15 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB15AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB15PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB15BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB15BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount15 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB15AmountChange = 0;
                                stat.Change1.TotalSB15PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB15AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB15PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB15BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB15BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB16MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount16 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB16AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB16PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB16BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB16BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount16 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB16AmountChange = 0;
                                stat.Change1.TotalSB16PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB16AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB16PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB16BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB16BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB17MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount17 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB17AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB17PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB17BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB17BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount17 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB17AmountChange = 0;
                                stat.Change1.TotalSB17PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB17AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB17PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB17BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB17BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB18MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount18 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB18AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB18PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB18BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB18BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount18 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB18AmountChange = 0;
                                stat.Change1.TotalSB18PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB18AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB18PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB18BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB18BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB19MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount19 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB19AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB19PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB19BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB19BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount19 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB19AmountChange = 0;
                                stat.Change1.TotalSB19PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB19AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB19PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB19BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB19BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private static void SetSB20MeanChanges(SB1Base analyzerStock, List<SB1Stock> changeStocks)
        {
            int i = 0;
            List<List<double>> results = GetResults(analyzerStock, changeStocks);
            //the analyses set up the results so that each changestock has a list of doubles containing results
            if (results.Count >= changeStocks.Count)
            {
                foreach (SB1Stock stat in changeStocks)
                {
                    if (results[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            stat.Change1.TSB1TMAmount20 = results[i].ElementAt(0);
                            //f stats
                            stat.Change1.TotalSB20AmountChange = results[i].ElementAt(1);
                            stat.Change1.TotalSB20PercentChange = results[i].ElementAt(2);
                            stat.Change1.TotalSB20BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB20BasePercentChange = results[i].ElementAt(4);
                        }
                        else
                        {
                            //mean treatment
                            stat.Change1.TSB1TMAmount20 = results[i].ElementAt(0);
                            if (i == 1)
                            {
                                stat.Change1.TotalSB20AmountChange = 0;
                                stat.Change1.TotalSB20PercentChange = 0;
                            }
                            else
                            {
                                //mean diff
                                stat.Change1.TotalSB20AmountChange = results[i].ElementAt(1);
                                //plus or minus ci
                                stat.Change1.TotalSB20PercentChange = results[i].ElementAt(2);
                            }
                            //mean diff
                            stat.Change1.TotalSB20BaseChange = results[i].ElementAt(3);
                            stat.Change1.TotalSB20BasePercentChange = results[i].ElementAt(4);
                        }
                    }
                    i++;
                }
            }
        }
        private bool AddChangeStocksToBaseStock(SB1Stock sb1Stock,
            List<SB1Stock> changeStocks)
        {
            bool bHasAnalyses = false;
            sb1Stock.Stocks = new List<SB1Stock>();
            foreach (SB1Stock changeStock in changeStocks)
            {
                //add it to the list
                sb1Stock.Stocks.Add(changeStock);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }

        
    }
}