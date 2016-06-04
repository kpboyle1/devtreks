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
    ///             The class statistically analyzes sbs.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1. 
    ///</summary>
    public class SB1Stat1 : SB1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public SB1Stat1(CalculatorParameters calcs)
            : base()
        {
            InitTotalSB1Stat1Properties(this, calcs);
        }
        //copy constructor
        public SB1Stat1(SB1Stat1 calculator)
            : base(calculator)
        {
            CopyTotalSB1Stat1Properties(this, calculator);
        }
        //note that additional display properties are in parent SB101Stock
        public double TotalSBScoreMean { get; set; }
        public double TotalSBScoreMedian { get; set; }
        public double TotalSBScoreVariance { get; set; }
        public double TotalSBScoreStandDev { get; set; }

        private const string cTotalSBScoreMean = "TSBScoreMean";
        private const string cTotalSBScoreMedian = "TSBScoreMedian";
        private const string cTotalSBScoreVariance = "TSBScoreVariance";
        private const string cTotalSBScoreStandDev = "TSBScoreStandDev";

        public double TotalSBScoreLMean { get; set; }
        public double TotalSBScoreLMedian { get; set; }
        public double TotalSBScoreLVariance { get; set; }
        public double TotalSBScoreLStandDev { get; set; }

        private const string cTotalSBScoreLMean = "TSBScoreLMean";
        private const string cTotalSBScoreLMedian = "TSBScoreLMedian";
        private const string cTotalSBScoreLVariance = "TSBScoreLVariance";
        private const string cTotalSBScoreLStandDev = "TSBScoreLStandDev";

        public double TotalSBScoreUMean { get; set; }
        public double TotalSBScoreUMedian { get; set; }
        public double TotalSBScoreUVariance { get; set; }
        public double TotalSBScoreUStandDev { get; set; }

        private const string cTotalSBScoreUMean = "TSBScoreUMean";
        private const string cTotalSBScoreUMedian = "TSBScoreUMedian";
        private const string cTotalSBScoreUVariance = "TSBScoreUVariance";
        private const string cTotalSBScoreUStandDev = "TSBScoreUStandDev";

        public double TotalSB1Mean { get; set; }
        public double TotalSB1Median { get; set; }
        public double TotalSB1Variance { get; set; }
        public double TotalSB1StandDev { get; set; }

        private const string cTotalSB1Mean = "TSB1Mean";
        private const string cTotalSB1Median = "TSB1Median";
        private const string cTotalSB1Variance = "TSB1Variance";
        private const string cTotalSB1StandDev = "TSB1StandDev";

        public double TotalSB2Mean { get; set; }
        public double TotalSB2Median { get; set; }
        public double TotalSB2Variance { get; set; }
        public double TotalSB2StandDev { get; set; }

        private const string cTotalSB2Mean = "TSB2Mean";
        private const string cTotalSB2Median = "TSB2Median";
        private const string cTotalSB2Variance = "TSB2Variance";
        private const string cTotalSB2StandDev = "TSB2StandDev";

        public double TotalSB3Mean { get; set; }
        public double TotalSB3Median { get; set; }
        public double TotalSB3Variance { get; set; }
        public double TotalSB3StandDev { get; set; }

        private const string cTotalSB3Mean = "TSB3Mean";
        private const string cTotalSB3Median = "TSB3Median";
        private const string cTotalSB3Variance = "TSB3Variance";
        private const string cTotalSB3StandDev = "TSB3StandDev";

        public double TotalSB4Mean { get; set; }
        public double TotalSB4Median { get; set; }
        public double TotalSB4Variance { get; set; }
        public double TotalSB4StandDev { get; set; }

        private const string cTotalSB4Mean = "TSB4Mean";
        private const string cTotalSB4Median = "TSB4Median";
        private const string cTotalSB4Variance = "TSB4Variance";
        private const string cTotalSB4StandDev = "TSB4StandDev";

        public double TotalSB5Mean { get; set; }
        public double TotalSB5Median { get; set; }
        public double TotalSB5Variance { get; set; }
        public double TotalSB5StandDev { get; set; }

        private const string cTotalSB5Mean = "TSB5Mean";
        private const string cTotalSB5Median = "TSB5Median";
        private const string cTotalSB5Variance = "TSB5Variance";
        private const string cTotalSB5StandDev = "TSB5StandDev";

        public double TotalSB6Mean { get; set; }
        public double TotalSB6Median { get; set; }
        public double TotalSB6Variance { get; set; }
        public double TotalSB6StandDev { get; set; }

        private const string cTotalSB6Mean = "TSB6Mean";
        private const string cTotalSB6Median = "TSB6Median";
        private const string cTotalSB6Variance = "TSB6Variance";
        private const string cTotalSB6StandDev = "TSB6StandDev";

        public double TotalSB7Mean { get; set; }
        public double TotalSB7Median { get; set; }
        public double TotalSB7Variance { get; set; }
        public double TotalSB7StandDev { get; set; }

        private const string cTotalSB7Mean = "TSB7Mean";
        private const string cTotalSB7Median = "TSB7Median";
        private const string cTotalSB7Variance = "TSB7Variance";
        private const string cTotalSB7StandDev = "TSB7StandDev";

        public double TotalSB8Mean { get; set; }
        public double TotalSB8Median { get; set; }
        public double TotalSB8Variance { get; set; }
        public double TotalSB8StandDev { get; set; }

        private const string cTotalSB8Mean = "TSB8Mean";
        private const string cTotalSB8Median = "TSB8Median";
        private const string cTotalSB8Variance = "TSB8Variance";
        private const string cTotalSB8StandDev = "TSB8StandDev";

        public double TotalSB9Mean { get; set; }
        public double TotalSB9Median { get; set; }
        public double TotalSB9Variance { get; set; }
        public double TotalSB9StandDev { get; set; }

        private const string cTotalSB9Mean = "TSB9Mean";
        private const string cTotalSB9Median = "TSB9Median";
        private const string cTotalSB9Variance = "TSB9Variance";
        private const string cTotalSB9StandDev = "TSB9StandDev";

        public double TotalSB10Mean { get; set; }
        public double TotalSB10Median { get; set; }
        public double TotalSB10Variance { get; set; }
        public double TotalSB10StandDev { get; set; }

        private const string cTotalSB10Mean = "TSB10Mean";
        private const string cTotalSB10Median = "TSB10Median";
        private const string cTotalSB10Variance = "TSB10Variance";
        private const string cTotalSB10StandDev = "TSB10StandDev";

        public double TotalSB11Mean { get; set; }
        public double TotalSB11Median { get; set; }
        public double TotalSB11Variance { get; set; }
        public double TotalSB11StandDev { get; set; }

        private const string cTotalSB11Mean = "TSB11Mean";
        private const string cTotalSB11Median = "TSB11Median";
        private const string cTotalSB11Variance = "TSB11Variance";
        private const string cTotalSB11StandDev = "TSB11StandDev";

        public double TotalSB12Mean { get; set; }
        public double TotalSB12Median { get; set; }
        public double TotalSB12Variance { get; set; }
        public double TotalSB12StandDev { get; set; }

        private const string cTotalSB12Mean = "TSB12Mean";
        private const string cTotalSB12Median = "TSB12Median";
        private const string cTotalSB12Variance = "TSB12Variance";
        private const string cTotalSB12StandDev = "TSB12StandDev";

        public double TotalSB13Mean { get; set; }
        public double TotalSB13Median { get; set; }
        public double TotalSB13Variance { get; set; }
        public double TotalSB13StandDev { get; set; }

        private const string cTotalSB13Mean = "TSB13Mean";
        private const string cTotalSB13Median = "TSB13Median";
        private const string cTotalSB13Variance = "TSB13Variance";
        private const string cTotalSB13StandDev = "TSB13StandDev";

        public double TotalSB14Mean { get; set; }
        public double TotalSB14Median { get; set; }
        public double TotalSB14Variance { get; set; }
        public double TotalSB14StandDev { get; set; }

        private const string cTotalSB14Mean = "TSB14Mean";
        private const string cTotalSB14Median = "TSB14Median";
        private const string cTotalSB14Variance = "TSB14Variance";
        private const string cTotalSB14StandDev = "TSB14StandDev";

        public double TotalSB15Mean { get; set; }
        public double TotalSB15Median { get; set; }
        public double TotalSB15Variance { get; set; }
        public double TotalSB15StandDev { get; set; }

        private const string cTotalSB15Mean = "TSB15Mean";
        private const string cTotalSB15Median = "TSB15Median";
        private const string cTotalSB15Variance = "TSB15Variance";
        private const string cTotalSB15StandDev = "TSB15StandDev";

        public double TotalSB16Mean { get; set; }
        public double TotalSB16Median { get; set; }
        public double TotalSB16Variance { get; set; }
        public double TotalSB16StandDev { get; set; }

        private const string cTotalSB16Mean = "TSB16Mean";
        private const string cTotalSB16Median = "TSB16Median";
        private const string cTotalSB16Variance = "TSB16Variance";
        private const string cTotalSB16StandDev = "TSB16StandDev";

        public double TotalSB17Mean { get; set; }
        public double TotalSB17Median { get; set; }
        public double TotalSB17Variance { get; set; }
        public double TotalSB17StandDev { get; set; }

        private const string cTotalSB17Mean = "TSB17Mean";
        private const string cTotalSB17Median = "TSB17Median";
        private const string cTotalSB17Variance = "TSB17Variance";
        private const string cTotalSB17StandDev = "TSB17StandDev";

        public double TotalSB18Mean { get; set; }
        public double TotalSB18Median { get; set; }
        public double TotalSB18Variance { get; set; }
        public double TotalSB18StandDev { get; set; }

        private const string cTotalSB18Mean = "TSB18Mean";
        private const string cTotalSB18Median = "TSB18Median";
        private const string cTotalSB18Variance = "TSB18Variance";
        private const string cTotalSB18StandDev = "TSB18StandDev";

        public double TotalSB19Mean { get; set; }
        public double TotalSB19Median { get; set; }
        public double TotalSB19Variance { get; set; }
        public double TotalSB19StandDev { get; set; }

        private const string cTotalSB19Mean = "TSB19Mean";
        private const string cTotalSB19Median = "TSB19Median";
        private const string cTotalSB19Variance = "TSB19Variance";
        private const string cTotalSB19StandDev = "TSB19StandDev";

        public double TotalSB20Mean { get; set; }
        public double TotalSB20Median { get; set; }
        public double TotalSB20Variance { get; set; }
        public double TotalSB20StandDev { get; set; }

        private const string cTotalSB20Mean = "TSB20Mean";
        private const string cTotalSB20Median = "TSB20Median";
        private const string cTotalSB20Variance = "TSB20Variance";
        private const string cTotalSB20StandDev = "TSB20StandDev";

        public void InitTotalSB1Stat1Properties(SB1Stat1 ind,
            CalculatorParameters calcs)
        {
            //avoid nulls
            InitSB1AnalysisProperties(ind, calcs);
            ind.ErrorMessage = string.Empty;

            ind.TotalSBScoreMean = 0;
            ind.TotalSBScoreMedian = 0;
            ind.TotalSBScoreVariance = 0;
            ind.TotalSBScoreStandDev = 0;
            ind.TotalSBScoreLMean = 0;
            ind.TotalSBScoreLMedian = 0;
            ind.TotalSBScoreLVariance = 0;
            ind.TotalSBScoreLStandDev = 0;
            ind.TotalSBScoreUMean = 0;
            ind.TotalSBScoreUMedian = 0;
            ind.TotalSBScoreUVariance = 0;
            ind.TotalSBScoreUStandDev = 0;

            ind.TotalSB1Mean = 0;
            ind.TotalSB1Median = 0;
            ind.TotalSB1Variance = 0;
            ind.TotalSB1StandDev = 0;

            ind.TotalSB2Mean = 0;
            ind.TotalSB2Median = 0;
            ind.TotalSB2Variance = 0;
            ind.TotalSB2StandDev = 0;

            ind.TotalSB3Mean = 0;
            ind.TotalSB3Median = 0;
            ind.TotalSB3Variance = 0;
            ind.TotalSB3StandDev = 0;

            ind.TotalSB4Mean = 0;
            ind.TotalSB4Median = 0;
            ind.TotalSB4Variance = 0;
            ind.TotalSB4StandDev = 0;

            ind.TotalSB5Mean = 0;
            ind.TotalSB5Median = 0;
            ind.TotalSB5Variance = 0;
            ind.TotalSB5StandDev = 0;

            ind.TotalSB6Mean = 0;
            ind.TotalSB6Median = 0;
            ind.TotalSB6Variance = 0;
            ind.TotalSB6StandDev = 0;

            ind.TotalSB7Mean = 0;
            ind.TotalSB7Median = 0;
            ind.TotalSB7Variance = 0;
            ind.TotalSB7StandDev = 0;

            ind.TotalSB8Mean = 0;
            ind.TotalSB8Median = 0;
            ind.TotalSB8Variance = 0;
            ind.TotalSB8StandDev = 0;

            ind.TotalSB9Mean = 0;
            ind.TotalSB9Median = 0;
            ind.TotalSB9Variance = 0;
            ind.TotalSB9StandDev = 0;

            ind.TotalSB10Mean = 0;
            ind.TotalSB10Median = 0;
            ind.TotalSB10Variance = 0;
            ind.TotalSB10StandDev = 0;

            ind.TotalSB11Mean = 0;
            ind.TotalSB11Median = 0;
            ind.TotalSB11Variance = 0;
            ind.TotalSB11StandDev = 0;

            ind.TotalSB12Mean = 0;
            ind.TotalSB12Median = 0;
            ind.TotalSB12Variance = 0;
            ind.TotalSB12StandDev = 0;

            ind.TotalSB13Mean = 0;
            ind.TotalSB13Median = 0;
            ind.TotalSB13Variance = 0;
            ind.TotalSB13StandDev = 0;

            ind.TotalSB14Mean = 0;
            ind.TotalSB14Median = 0;
            ind.TotalSB14Variance = 0;
            ind.TotalSB14StandDev = 0;

            ind.TotalSB15Mean = 0;
            ind.TotalSB15Median = 0;
            ind.TotalSB15Variance = 0;
            ind.TotalSB15StandDev = 0;

            ind.TotalSB16Mean = 0;
            ind.TotalSB16Median = 0;
            ind.TotalSB16Variance = 0;
            ind.TotalSB16StandDev = 0;

            ind.TotalSB17Mean = 0;
            ind.TotalSB17Median = 0;
            ind.TotalSB17Variance = 0;
            ind.TotalSB17StandDev = 0;

            ind.TotalSB18Mean = 0;
            ind.TotalSB18Median = 0;
            ind.TotalSB18Variance = 0;
            ind.TotalSB18StandDev = 0;

            ind.TotalSB19Mean = 0;
            ind.TotalSB19Median = 0;
            ind.TotalSB19Variance = 0;
            ind.TotalSB19StandDev = 0;

            ind.TotalSB20Mean = 0;
            ind.TotalSB20Median = 0;
            ind.TotalSB20Variance = 0;
            ind.TotalSB20StandDev = 0;

            ind.CalcParameters = calcs;
            ind.SB11Stock = new SB101Stock();
            ind.SB12Stock = new SB102Stock();
        }

        public void CopyTotalSB1Stat1Properties(SB1Stat1 ind,
            SB1Stat1 calculator)
        {
            //avoid nulls
            CopySB1AnalysisProperties(ind, calculator);
            ind.ErrorMessage = calculator.ErrorMessage;

            ind.TotalSBScoreMean = calculator.TotalSBScoreMean;
            ind.TotalSBScoreMedian = calculator.TotalSBScoreMedian;
            ind.TotalSBScoreVariance = calculator.TotalSBScoreVariance;
            ind.TotalSBScoreStandDev = calculator.TotalSBScoreStandDev;
            ind.TotalSBScoreLMean = calculator.TotalSBScoreLMean;
            ind.TotalSBScoreLMedian = calculator.TotalSBScoreLMedian;
            ind.TotalSBScoreLVariance = calculator.TotalSBScoreLVariance;
            ind.TotalSBScoreLStandDev = calculator.TotalSBScoreLStandDev;
            ind.TotalSBScoreUMean = calculator.TotalSBScoreUMean;
            ind.TotalSBScoreUMedian = calculator.TotalSBScoreUMedian;
            ind.TotalSBScoreUVariance = calculator.TotalSBScoreUVariance;
            ind.TotalSBScoreUStandDev = calculator.TotalSBScoreUStandDev;

            ind.TotalSB1Mean = calculator.TotalSB1Mean;
            ind.TotalSB1Median = calculator.TotalSB1Median;
            ind.TotalSB1Variance = calculator.TotalSB1Variance;
            ind.TotalSB1StandDev = calculator.TotalSB1StandDev;

            ind.TotalSB2Mean = calculator.TotalSB2Mean;
            ind.TotalSB2Median = calculator.TotalSB2Median;
            ind.TotalSB2Variance = calculator.TotalSB2Variance;
            ind.TotalSB2StandDev = calculator.TotalSB2StandDev;

            ind.TotalSB3Mean = calculator.TotalSB3Mean;
            ind.TotalSB3Median = calculator.TotalSB3Median;
            ind.TotalSB3Variance = calculator.TotalSB3Variance;
            ind.TotalSB3StandDev = calculator.TotalSB3StandDev;

            ind.TotalSB4Mean = calculator.TotalSB4Mean;
            ind.TotalSB4Median = calculator.TotalSB4Median;
            ind.TotalSB4Variance = calculator.TotalSB4Variance;
            ind.TotalSB4StandDev = calculator.TotalSB4StandDev;

            ind.TotalSB5Mean = calculator.TotalSB5Mean;
            ind.TotalSB5Median = calculator.TotalSB5Median;
            ind.TotalSB5Variance = calculator.TotalSB5Variance;
            ind.TotalSB5StandDev = calculator.TotalSB5StandDev;

            ind.TotalSB6Mean = calculator.TotalSB6Mean;
            ind.TotalSB6Median = calculator.TotalSB6Median;
            ind.TotalSB6Variance = calculator.TotalSB6Variance;
            ind.TotalSB6StandDev = calculator.TotalSB6StandDev;

            ind.TotalSB7Mean = calculator.TotalSB7Mean;
            ind.TotalSB7Median = calculator.TotalSB7Median;
            ind.TotalSB7Variance = calculator.TotalSB7Variance;
            ind.TotalSB7StandDev = calculator.TotalSB7StandDev;

            ind.TotalSB8Mean = calculator.TotalSB8Mean;
            ind.TotalSB8Median = calculator.TotalSB8Median;
            ind.TotalSB8Variance = calculator.TotalSB8Variance;
            ind.TotalSB8StandDev = calculator.TotalSB8StandDev;

            ind.TotalSB9Mean = calculator.TotalSB9Mean;
            ind.TotalSB9Median = calculator.TotalSB9Median;
            ind.TotalSB9Variance = calculator.TotalSB9Variance;
            ind.TotalSB9StandDev = calculator.TotalSB9StandDev;

            ind.TotalSB10Mean = calculator.TotalSB10Mean;
            ind.TotalSB10Median = calculator.TotalSB10Median;
            ind.TotalSB10Variance = calculator.TotalSB10Variance;
            ind.TotalSB10StandDev = calculator.TotalSB10StandDev;

            ind.TotalSB11Mean = calculator.TotalSB11Mean;
            ind.TotalSB11Median = calculator.TotalSB11Median;
            ind.TotalSB11Variance = calculator.TotalSB11Variance;
            ind.TotalSB11StandDev = calculator.TotalSB11StandDev;

            ind.TotalSB12Mean = calculator.TotalSB12Mean;
            ind.TotalSB12Median = calculator.TotalSB12Median;
            ind.TotalSB12Variance = calculator.TotalSB12Variance;
            ind.TotalSB12StandDev = calculator.TotalSB12StandDev;

            ind.TotalSB13Mean = calculator.TotalSB13Mean;
            ind.TotalSB13Median = calculator.TotalSB13Median;
            ind.TotalSB13Variance = calculator.TotalSB13Variance;
            ind.TotalSB13StandDev = calculator.TotalSB13StandDev;

            ind.TotalSB14Mean = calculator.TotalSB14Mean;
            ind.TotalSB14Median = calculator.TotalSB14Median;
            ind.TotalSB14Variance = calculator.TotalSB14Variance;
            ind.TotalSB14StandDev = calculator.TotalSB14StandDev;

            ind.TotalSB15Mean = calculator.TotalSB15Mean;
            ind.TotalSB15Median = calculator.TotalSB15Median;
            ind.TotalSB15Variance = calculator.TotalSB15Variance;
            ind.TotalSB15StandDev = calculator.TotalSB15StandDev;

            ind.TotalSB16Mean = calculator.TotalSB16Mean;
            ind.TotalSB16Median = calculator.TotalSB16Median;
            ind.TotalSB16Variance = calculator.TotalSB16Variance;
            ind.TotalSB16StandDev = calculator.TotalSB16StandDev;

            ind.TotalSB17Mean = calculator.TotalSB17Mean;
            ind.TotalSB17Median = calculator.TotalSB17Median;
            ind.TotalSB17Variance = calculator.TotalSB17Variance;
            ind.TotalSB17StandDev = calculator.TotalSB17StandDev;

            ind.TotalSB18Mean = calculator.TotalSB18Mean;
            ind.TotalSB18Median = calculator.TotalSB18Median;
            ind.TotalSB18Variance = calculator.TotalSB18Variance;
            ind.TotalSB18StandDev = calculator.TotalSB18StandDev;

            ind.TotalSB19Mean = calculator.TotalSB19Mean;
            ind.TotalSB19Median = calculator.TotalSB19Median;
            ind.TotalSB19Variance = calculator.TotalSB19Variance;
            ind.TotalSB19StandDev = calculator.TotalSB19StandDev;

            ind.TotalSB20Mean = calculator.TotalSB20Mean;
            ind.TotalSB20Median = calculator.TotalSB20Median;
            ind.TotalSB20Variance = calculator.TotalSB20Variance;
            ind.TotalSB20StandDev = calculator.TotalSB20StandDev;
        }

        public void SetTotalSB1Stat1Properties(SB1Stat1 ind,
            string attNameExtension, XElement calculator)
        {

            ind.TotalSBScoreMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreMean, attNameExtension));
            ind.TotalSBScoreMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreMedian, attNameExtension));
            ind.TotalSBScoreVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreVariance, attNameExtension));
            ind.TotalSBScoreStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreStandDev, attNameExtension));
            ind.TotalSBScoreLMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLMean, attNameExtension));
            ind.TotalSBScoreLMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLMedian, attNameExtension));
            ind.TotalSBScoreLVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLVariance, attNameExtension));
            ind.TotalSBScoreLStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreLStandDev, attNameExtension));
            ind.TotalSBScoreUMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUMean, attNameExtension));
            ind.TotalSBScoreUMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUMedian, attNameExtension));
            ind.TotalSBScoreUVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUVariance, attNameExtension));
            ind.TotalSBScoreUStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSBScoreUStandDev, attNameExtension));

            ind.TotalSB1Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1Mean, attNameExtension));
            ind.TotalSB1Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1Median, attNameExtension));
            ind.TotalSB1Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1Variance, attNameExtension));
            ind.TotalSB1StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB1StandDev, attNameExtension));

            ind.TotalSB2Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2Mean, attNameExtension));
            ind.TotalSB2Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2Median, attNameExtension));
            ind.TotalSB2Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2Variance, attNameExtension));
            ind.TotalSB2StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB2StandDev, attNameExtension));

            ind.TotalSB3Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3Mean, attNameExtension));
            ind.TotalSB3Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3Median, attNameExtension));
            ind.TotalSB3Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3Variance, attNameExtension));
            ind.TotalSB3StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB3StandDev, attNameExtension));

            ind.TotalSB4Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4Mean, attNameExtension));
            ind.TotalSB4Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4Median, attNameExtension));
            ind.TotalSB4Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4Variance, attNameExtension));
            ind.TotalSB4StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB4StandDev, attNameExtension));

            ind.TotalSB5Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5Mean, attNameExtension));
            ind.TotalSB5Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5Median, attNameExtension));
            ind.TotalSB5Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5Variance, attNameExtension));
            ind.TotalSB5StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB5StandDev, attNameExtension));

            ind.TotalSB6Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6Mean, attNameExtension));
            ind.TotalSB6Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6Median, attNameExtension));
            ind.TotalSB6Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6Variance, attNameExtension));
            ind.TotalSB6StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB6StandDev, attNameExtension));

            ind.TotalSB7Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7Mean, attNameExtension));
            ind.TotalSB7Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7Median, attNameExtension));
            ind.TotalSB7Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7Variance, attNameExtension));
            ind.TotalSB7StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB7StandDev, attNameExtension));

            ind.TotalSB8Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8Mean, attNameExtension));
            ind.TotalSB8Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8Median, attNameExtension));
            ind.TotalSB8Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8Variance, attNameExtension));
            ind.TotalSB8StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB8StandDev, attNameExtension));

            ind.TotalSB9Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9Mean, attNameExtension));
            ind.TotalSB9Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9Median, attNameExtension));
            ind.TotalSB9Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9Variance, attNameExtension));
            ind.TotalSB9StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB9StandDev, attNameExtension));

            ind.TotalSB10Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10Mean, attNameExtension));
            ind.TotalSB10Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10Median, attNameExtension));
            ind.TotalSB10Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10Variance, attNameExtension));
            ind.TotalSB10StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB10StandDev, attNameExtension));

            ind.TotalSB11Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11Mean, attNameExtension));
            ind.TotalSB11Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11Median, attNameExtension));
            ind.TotalSB11Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11Variance, attNameExtension));
            ind.TotalSB11StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB11StandDev, attNameExtension));

            ind.TotalSB12Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12Mean, attNameExtension));
            ind.TotalSB12Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12Median, attNameExtension));
            ind.TotalSB12Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12Variance, attNameExtension));
            ind.TotalSB12StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB12StandDev, attNameExtension));

            ind.TotalSB13Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13Mean, attNameExtension));
            ind.TotalSB13Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13Median, attNameExtension));
            ind.TotalSB13Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13Variance, attNameExtension));
            ind.TotalSB13StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB13StandDev, attNameExtension));

            ind.TotalSB14Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14Mean, attNameExtension));
            ind.TotalSB14Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14Median, attNameExtension));
            ind.TotalSB14Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14Variance, attNameExtension));
            ind.TotalSB14StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB14StandDev, attNameExtension));

            ind.TotalSB15Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15Mean, attNameExtension));
            ind.TotalSB15Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15Median, attNameExtension));
            ind.TotalSB15Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15Variance, attNameExtension));
            ind.TotalSB15StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB15StandDev, attNameExtension));

            ind.TotalSB16Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16Mean, attNameExtension));
            ind.TotalSB16Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16Median, attNameExtension));
            ind.TotalSB16Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16Variance, attNameExtension));
            ind.TotalSB16StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB16StandDev, attNameExtension));

            ind.TotalSB17Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17Mean, attNameExtension));
            ind.TotalSB17Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17Median, attNameExtension));
            ind.TotalSB17Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17Variance, attNameExtension));
            ind.TotalSB17StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB17StandDev, attNameExtension));

            ind.TotalSB18Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18Mean, attNameExtension));
            ind.TotalSB18Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18Median, attNameExtension));
            ind.TotalSB18Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18Variance, attNameExtension));
            ind.TotalSB18StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB18StandDev, attNameExtension));

            ind.TotalSB19Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19Mean, attNameExtension));
            ind.TotalSB19Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19Median, attNameExtension));
            ind.TotalSB19Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19Variance, attNameExtension));
            ind.TotalSB19StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB19StandDev, attNameExtension));

            ind.TotalSB20Mean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20Mean, attNameExtension));
            ind.TotalSB20Median = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20Median, attNameExtension));
            ind.TotalSB20Variance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20Variance, attNameExtension));
            ind.TotalSB20StandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalSB20StandDev, attNameExtension));
        }

        public void SetTotalSB1Stat1Property(SB1Stat1 ind,
            string attName, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSBScoreMean:
                    ind.TotalSBScoreMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreMedian:
                    ind.TotalSBScoreMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreVariance:
                    ind.TotalSBScoreVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreStandDev:
                    ind.TotalSBScoreStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLMean:
                    ind.TotalSBScoreLMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLMedian:
                    ind.TotalSBScoreLMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLVariance:
                    ind.TotalSBScoreLVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreLStandDev:
                    ind.TotalSBScoreLStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUMean:
                    ind.TotalSBScoreUMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUMedian:
                    ind.TotalSBScoreUMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUVariance:
                    ind.TotalSBScoreUVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSBScoreUStandDev:
                    ind.TotalSBScoreUStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1Mean:
                    ind.TotalSB1Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1Median:
                    ind.TotalSB1Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1Variance:
                    ind.TotalSB1Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB1StandDev:
                    ind.TotalSB1StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2Mean:
                    ind.TotalSB2Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2Median:
                    ind.TotalSB2Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2Variance:
                    ind.TotalSB2Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB2StandDev:
                    ind.TotalSB2StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3Mean:
                    ind.TotalSB3Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3Median:
                    ind.TotalSB3Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3Variance:
                    ind.TotalSB3Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB3StandDev:
                    ind.TotalSB3StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4Mean:
                    ind.TotalSB4Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4Median:
                    ind.TotalSB4Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4Variance:
                    ind.TotalSB4Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB4StandDev:
                    ind.TotalSB4StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5Mean:
                    ind.TotalSB5Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5Median:
                    ind.TotalSB5Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5Variance:
                    ind.TotalSB5Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB5StandDev:
                    ind.TotalSB5StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6Mean:
                    ind.TotalSB6Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6Median:
                    ind.TotalSB6Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6Variance:
                    ind.TotalSB6Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB6StandDev:
                    ind.TotalSB6StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7Mean:
                    ind.TotalSB7Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7Median:
                    ind.TotalSB7Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7Variance:
                    ind.TotalSB7Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB7StandDev:
                    ind.TotalSB7StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8Mean:
                    ind.TotalSB8Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8Median:
                    ind.TotalSB8Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8Variance:
                    ind.TotalSB8Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB8StandDev:
                    ind.TotalSB8StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9Mean:
                    ind.TotalSB9Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9Median:
                    ind.TotalSB9Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9Variance:
                    ind.TotalSB9Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB9StandDev:
                    ind.TotalSB9StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10Mean:
                    ind.TotalSB10Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10Median:
                    ind.TotalSB10Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10Variance:
                    ind.TotalSB10Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB10StandDev:
                    ind.TotalSB10StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11Mean:
                    ind.TotalSB11Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11Median:
                    ind.TotalSB11Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11Variance:
                    ind.TotalSB11Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB11StandDev:
                    ind.TotalSB11StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12Mean:
                    ind.TotalSB12Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12Median:
                    ind.TotalSB12Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12Variance:
                    ind.TotalSB12Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB12StandDev:
                    ind.TotalSB12StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13Mean:
                    ind.TotalSB13Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13Median:
                    ind.TotalSB13Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13Variance:
                    ind.TotalSB13Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB13StandDev:
                    ind.TotalSB13StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14Mean:
                    ind.TotalSB14Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14Median:
                    ind.TotalSB14Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14Variance:
                    ind.TotalSB14Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB14StandDev:
                    ind.TotalSB14StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15Mean:
                    ind.TotalSB15Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15Median:
                    ind.TotalSB15Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15Variance:
                    ind.TotalSB15Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB15StandDev:
                    ind.TotalSB15StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16Mean:
                    ind.TotalSB16Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16Median:
                    ind.TotalSB16Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16Variance:
                    ind.TotalSB16Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB16StandDev:
                    ind.TotalSB16StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17Mean:
                    ind.TotalSB17Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17Median:
                    ind.TotalSB17Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17Variance:
                    ind.TotalSB17Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB17StandDev:
                    ind.TotalSB17StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18Mean:
                    ind.TotalSB18Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18Median:
                    ind.TotalSB18Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18Variance:
                    ind.TotalSB18Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB18StandDev:
                    ind.TotalSB18StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19Mean:
                    ind.TotalSB19Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19Median:
                    ind.TotalSB19Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19Variance:
                    ind.TotalSB19Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB19StandDev:
                    ind.TotalSB19StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20Mean:
                    ind.TotalSB20Mean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20Median:
                    ind.TotalSB20Median = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20Variance:
                    ind.TotalSB20Variance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalSB20StandDev:
                    ind.TotalSB20StandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalSB1Stat1Property(SB1Stat1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalSBScoreMean:
                    sPropertyValue = ind.TotalSBScoreMean.ToString();
                    break;
                case cTotalSBScoreMedian:
                    sPropertyValue = ind.TotalSBScoreMedian.ToString();
                    break;
                case cTotalSBScoreVariance:
                    sPropertyValue = ind.TotalSBScoreVariance.ToString();
                    break;
                case cTotalSBScoreStandDev:
                    sPropertyValue = ind.TotalSBScoreStandDev.ToString();
                    break;
                case cTotalSBScoreLMean:
                    sPropertyValue = ind.TotalSBScoreLMean.ToString();
                    break;
                case cTotalSBScoreLMedian:
                    sPropertyValue = ind.TotalSBScoreLMedian.ToString();
                    break;
                case cTotalSBScoreLVariance:
                    sPropertyValue = ind.TotalSBScoreLVariance.ToString();
                    break;
                case cTotalSBScoreLStandDev:
                    sPropertyValue = ind.TotalSBScoreLStandDev.ToString();
                    break;
                case cTotalSBScoreUMean:
                    sPropertyValue = ind.TotalSBScoreUMean.ToString();
                    break;
                case cTotalSBScoreUMedian:
                    sPropertyValue = ind.TotalSBScoreUMedian.ToString();
                    break;
                case cTotalSBScoreUVariance:
                    sPropertyValue = ind.TotalSBScoreUVariance.ToString();
                    break;
                case cTotalSBScoreUStandDev:
                    sPropertyValue = ind.TotalSBScoreUStandDev.ToString();
                    break;
                case cTotalSB1Mean:
                    sPropertyValue = ind.TotalSB1Mean.ToString();
                    break;
                case cTotalSB1Median:
                    sPropertyValue = ind.TotalSB1Median.ToString();
                    break;
                case cTotalSB1Variance:
                    sPropertyValue = ind.TotalSB1Variance.ToString();
                    break;
                case cTotalSB1StandDev:
                    sPropertyValue = ind.TotalSB1StandDev.ToString();
                    break;
                case cTotalSB2Mean:
                    sPropertyValue = ind.TotalSB2Mean.ToString();
                    break;
                case cTotalSB2Median:
                    sPropertyValue = ind.TotalSB2Median.ToString();
                    break;
                case cTotalSB2Variance:
                    sPropertyValue = ind.TotalSB2Variance.ToString();
                    break;
                case cTotalSB2StandDev:
                    sPropertyValue = ind.TotalSB2StandDev.ToString();
                    break;
                case cTotalSB3Mean:
                    sPropertyValue = ind.TotalSB3Mean.ToString();
                    break;
                case cTotalSB3Median:
                    sPropertyValue = ind.TotalSB3Median.ToString();
                    break;
                case cTotalSB3Variance:
                    sPropertyValue = ind.TotalSB3Variance.ToString();
                    break;
                case cTotalSB3StandDev:
                    sPropertyValue = ind.TotalSB3StandDev.ToString();
                    break;
                case cTotalSB4Mean:
                    sPropertyValue = ind.TotalSB4Mean.ToString();
                    break;
                case cTotalSB4Median:
                    sPropertyValue = ind.TotalSB4Median.ToString();
                    break;
                case cTotalSB4Variance:
                    sPropertyValue = ind.TotalSB4Variance.ToString();
                    break;
                case cTotalSB4StandDev:
                    sPropertyValue = ind.TotalSB4StandDev.ToString();
                    break;
                case cTotalSB5Mean:
                    sPropertyValue = ind.TotalSB5Mean.ToString();
                    break;
                case cTotalSB5Median:
                    sPropertyValue = ind.TotalSB5Median.ToString();
                    break;
                case cTotalSB5Variance:
                    sPropertyValue = ind.TotalSB5Variance.ToString();
                    break;
                case cTotalSB5StandDev:
                    sPropertyValue = ind.TotalSB5StandDev.ToString();
                    break;
                case cTotalSB6Mean:
                    sPropertyValue = ind.TotalSB6Mean.ToString();
                    break;
                case cTotalSB6Median:
                    sPropertyValue = ind.TotalSB6Median.ToString();
                    break;
                case cTotalSB6Variance:
                    sPropertyValue = ind.TotalSB6Variance.ToString();
                    break;
                case cTotalSB6StandDev:
                    sPropertyValue = ind.TotalSB6StandDev.ToString();
                    break;
                case cTotalSB7Mean:
                    sPropertyValue = ind.TotalSB7Mean.ToString();
                    break;
                case cTotalSB7Median:
                    sPropertyValue = ind.TotalSB7Median.ToString();
                    break;
                case cTotalSB7Variance:
                    sPropertyValue = ind.TotalSB7Variance.ToString();
                    break;
                case cTotalSB7StandDev:
                    sPropertyValue = ind.TotalSB7StandDev.ToString();
                    break;
                case cTotalSB8Mean:
                    sPropertyValue = ind.TotalSB8Mean.ToString();
                    break;
                case cTotalSB8Median:
                    sPropertyValue = ind.TotalSB8Median.ToString();
                    break;
                case cTotalSB8Variance:
                    sPropertyValue = ind.TotalSB8Variance.ToString();
                    break;
                case cTotalSB8StandDev:
                    sPropertyValue = ind.TotalSB8StandDev.ToString();
                    break;
                case cTotalSB9Mean:
                    sPropertyValue = ind.TotalSB9Mean.ToString();
                    break;
                case cTotalSB9Median:
                    sPropertyValue = ind.TotalSB9Median.ToString();
                    break;
                case cTotalSB9Variance:
                    sPropertyValue = ind.TotalSB9Variance.ToString();
                    break;
                case cTotalSB9StandDev:
                    sPropertyValue = ind.TotalSB9StandDev.ToString();
                    break;
                case cTotalSB10Mean:
                    sPropertyValue = ind.TotalSB10Mean.ToString();
                    break;
                case cTotalSB10Median:
                    sPropertyValue = ind.TotalSB10Median.ToString();
                    break;
                case cTotalSB10Variance:
                    sPropertyValue = ind.TotalSB10Variance.ToString();
                    break;
                case cTotalSB10StandDev:
                    sPropertyValue = ind.TotalSB10StandDev.ToString();
                    break;
                case cTotalSB11Mean:
                    sPropertyValue = ind.TotalSB11Mean.ToString();
                    break;
                case cTotalSB11Median:
                    sPropertyValue = ind.TotalSB11Median.ToString();
                    break;
                case cTotalSB11Variance:
                    sPropertyValue = ind.TotalSB11Variance.ToString();
                    break;
                case cTotalSB11StandDev:
                    sPropertyValue = ind.TotalSB11StandDev.ToString();
                    break;
                case cTotalSB12Mean:
                    sPropertyValue = ind.TotalSB12Mean.ToString();
                    break;
                case cTotalSB12Median:
                    sPropertyValue = ind.TotalSB12Median.ToString();
                    break;
                case cTotalSB12Variance:
                    sPropertyValue = ind.TotalSB12Variance.ToString();
                    break;
                case cTotalSB12StandDev:
                    sPropertyValue = ind.TotalSB12StandDev.ToString();
                    break;
                case cTotalSB13Mean:
                    sPropertyValue = ind.TotalSB13Mean.ToString();
                    break;
                case cTotalSB13Median:
                    sPropertyValue = ind.TotalSB13Median.ToString();
                    break;
                case cTotalSB13Variance:
                    sPropertyValue = ind.TotalSB13Variance.ToString();
                    break;
                case cTotalSB13StandDev:
                    sPropertyValue = ind.TotalSB13StandDev.ToString();
                    break;
                case cTotalSB14Mean:
                    sPropertyValue = ind.TotalSB14Mean.ToString();
                    break;
                case cTotalSB14Median:
                    sPropertyValue = ind.TotalSB14Median.ToString();
                    break;
                case cTotalSB14Variance:
                    sPropertyValue = ind.TotalSB14Variance.ToString();
                    break;
                case cTotalSB14StandDev:
                    sPropertyValue = ind.TotalSB14StandDev.ToString();
                    break;
                case cTotalSB15Mean:
                    sPropertyValue = ind.TotalSB15Mean.ToString();
                    break;
                case cTotalSB15Median:
                    sPropertyValue = ind.TotalSB15Median.ToString();
                    break;
                case cTotalSB15Variance:
                    sPropertyValue = ind.TotalSB15Variance.ToString();
                    break;
                case cTotalSB15StandDev:
                    sPropertyValue = ind.TotalSB15StandDev.ToString();
                    break;
                case cTotalSB16Mean:
                    sPropertyValue = ind.TotalSB16Mean.ToString();
                    break;
                case cTotalSB16Median:
                    sPropertyValue = ind.TotalSB16Median.ToString();
                    break;
                case cTotalSB16Variance:
                    sPropertyValue = ind.TotalSB16Variance.ToString();
                    break;
                case cTotalSB16StandDev:
                    sPropertyValue = ind.TotalSB16StandDev.ToString();
                    break;
                case cTotalSB17Mean:
                    sPropertyValue = ind.TotalSB17Mean.ToString();
                    break;
                case cTotalSB17Median:
                    sPropertyValue = ind.TotalSB17Median.ToString();
                    break;
                case cTotalSB17Variance:
                    sPropertyValue = ind.TotalSB17Variance.ToString();
                    break;
                case cTotalSB17StandDev:
                    sPropertyValue = ind.TotalSB17StandDev.ToString();
                    break;
                case cTotalSB18Mean:
                    sPropertyValue = ind.TotalSB18Mean.ToString();
                    break;
                case cTotalSB18Median:
                    sPropertyValue = ind.TotalSB18Median.ToString();
                    break;
                case cTotalSB18Variance:
                    sPropertyValue = ind.TotalSB18Variance.ToString();
                    break;
                case cTotalSB18StandDev:
                    sPropertyValue = ind.TotalSB18StandDev.ToString();
                    break;
                case cTotalSB19Mean:
                    sPropertyValue = ind.TotalSB19Mean.ToString();
                    break;
                case cTotalSB19Median:
                    sPropertyValue = ind.TotalSB19Median.ToString();
                    break;
                case cTotalSB19Variance:
                    sPropertyValue = ind.TotalSB19Variance.ToString();
                    break;
                case cTotalSB19StandDev:
                    sPropertyValue = ind.TotalSB19StandDev.ToString();
                    break;
                case cTotalSB20Mean:
                    sPropertyValue = ind.TotalSB20Mean.ToString();
                    break;
                case cTotalSB20Median:
                    sPropertyValue = ind.TotalSB20Median.ToString();
                    break;
                case cTotalSB20Variance:
                    sPropertyValue = ind.TotalSB20Variance.ToString();
                    break;
                case cTotalSB20StandDev:
                    sPropertyValue = ind.TotalSB20StandDev.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalSB1Stat1Attributes(SB1Stat1 ind,
            string attNameExtension, XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreMean, attNameExtension), ind.TotalSBScoreMean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreMedian, attNameExtension), ind.TotalSBScoreMedian);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreVariance, attNameExtension), ind.TotalSBScoreVariance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreStandDev, attNameExtension), ind.TotalSBScoreStandDev);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLMean, attNameExtension), ind.TotalSBScoreLMean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLMedian, attNameExtension), ind.TotalSBScoreLMedian);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLVariance, attNameExtension), ind.TotalSBScoreLVariance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreLStandDev, attNameExtension), ind.TotalSBScoreLStandDev);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUMean, attNameExtension), ind.TotalSBScoreUMean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUMedian, attNameExtension), ind.TotalSBScoreUMedian);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUVariance, attNameExtension), ind.TotalSBScoreUVariance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSBScoreUStandDev, attNameExtension), ind.TotalSBScoreUStandDev);


            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1Mean, attNameExtension), ind.TotalSB1Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1Median, attNameExtension), ind.TotalSB1Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1Variance, attNameExtension), ind.TotalSB1Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB1StandDev, attNameExtension), ind.TotalSB1StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2Mean, attNameExtension), ind.TotalSB2Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2Median, attNameExtension), ind.TotalSB2Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2Variance, attNameExtension), ind.TotalSB2Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB2StandDev, attNameExtension), ind.TotalSB2StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3Mean, attNameExtension), ind.TotalSB3Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3Median, attNameExtension), ind.TotalSB3Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3Variance, attNameExtension), ind.TotalSB3Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB3StandDev, attNameExtension), ind.TotalSB3StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4Mean, attNameExtension), ind.TotalSB4Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4Median, attNameExtension), ind.TotalSB4Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4Variance, attNameExtension), ind.TotalSB4Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB4StandDev, attNameExtension), ind.TotalSB4StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5Mean, attNameExtension), ind.TotalSB5Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5Median, attNameExtension), ind.TotalSB5Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5Variance, attNameExtension), ind.TotalSB5Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB5StandDev, attNameExtension), ind.TotalSB5StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6Mean, attNameExtension), ind.TotalSB6Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6Median, attNameExtension), ind.TotalSB6Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6Variance, attNameExtension), ind.TotalSB6Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB6StandDev, attNameExtension), ind.TotalSB6StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7Mean, attNameExtension), ind.TotalSB7Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7Median, attNameExtension), ind.TotalSB7Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7Variance, attNameExtension), ind.TotalSB7Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB7StandDev, attNameExtension), ind.TotalSB7StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8Mean, attNameExtension), ind.TotalSB8Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8Median, attNameExtension), ind.TotalSB8Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8Variance, attNameExtension), ind.TotalSB8Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB8StandDev, attNameExtension), ind.TotalSB8StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9Mean, attNameExtension), ind.TotalSB9Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9Median, attNameExtension), ind.TotalSB9Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9Variance, attNameExtension), ind.TotalSB9Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB9StandDev, attNameExtension), ind.TotalSB9StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10Mean, attNameExtension), ind.TotalSB10Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10Median, attNameExtension), ind.TotalSB10Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10Variance, attNameExtension), ind.TotalSB10Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB10StandDev, attNameExtension), ind.TotalSB10StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11Mean, attNameExtension), ind.TotalSB11Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11Median, attNameExtension), ind.TotalSB11Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11Variance, attNameExtension), ind.TotalSB11Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB11StandDev, attNameExtension), ind.TotalSB11StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12Mean, attNameExtension), ind.TotalSB12Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12Median, attNameExtension), ind.TotalSB12Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12Variance, attNameExtension), ind.TotalSB12Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB12StandDev, attNameExtension), ind.TotalSB12StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13Mean, attNameExtension), ind.TotalSB13Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13Median, attNameExtension), ind.TotalSB13Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13Variance, attNameExtension), ind.TotalSB13Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB13StandDev, attNameExtension), ind.TotalSB13StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14Mean, attNameExtension), ind.TotalSB14Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14Median, attNameExtension), ind.TotalSB14Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14Variance, attNameExtension), ind.TotalSB14Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB14StandDev, attNameExtension), ind.TotalSB14StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15Mean, attNameExtension), ind.TotalSB15Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15Median, attNameExtension), ind.TotalSB15Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15Variance, attNameExtension), ind.TotalSB15Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB15StandDev, attNameExtension), ind.TotalSB15StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16Mean, attNameExtension), ind.TotalSB16Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16Median, attNameExtension), ind.TotalSB16Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16Variance, attNameExtension), ind.TotalSB16Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB16StandDev, attNameExtension), ind.TotalSB16StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17Mean, attNameExtension), ind.TotalSB17Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17Median, attNameExtension), ind.TotalSB17Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17Variance, attNameExtension), ind.TotalSB17Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB17StandDev, attNameExtension), ind.TotalSB17StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18Mean, attNameExtension), ind.TotalSB18Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18Median, attNameExtension), ind.TotalSB18Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18Variance, attNameExtension), ind.TotalSB18Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB18StandDev, attNameExtension), ind.TotalSB18StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19Mean, attNameExtension), ind.TotalSB19Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19Median, attNameExtension), ind.TotalSB19Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19Variance, attNameExtension), ind.TotalSB19Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB19StandDev, attNameExtension), ind.TotalSB19StandDev);

            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20Mean, attNameExtension), ind.TotalSB20Mean);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20Median, attNameExtension), ind.TotalSB20Median);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20Variance, attNameExtension), ind.TotalSB20Variance);
            CalculatorHelpers.SetAttributeDoubleN4(calculator,
                    string.Concat(cTotalSB20StandDev, attNameExtension), ind.TotalSB20StandDev);

        }

        public async Task SetTotalSB1Stat1AttributesAsync(SB1Stat1 ind,
            string attNameExtension, XmlWriter writer)
        {
            //comparative analyses need to know how many rows to display
            int iSBCount = 0;
            if (ind.TSB1ScoreMUnit.Length > 0)
            {
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreMean, attNameExtension), string.Empty, ind.TotalSBScoreMean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreMedian, attNameExtension), string.Empty, ind.TotalSBScoreMedian.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreVariance, attNameExtension), string.Empty, ind.TotalSBScoreVariance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreStandDev, attNameExtension), string.Empty, ind.TotalSBScoreStandDev.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLMean, attNameExtension), string.Empty, ind.TotalSBScoreLMean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLMedian, attNameExtension), string.Empty, ind.TotalSBScoreLMedian.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLVariance, attNameExtension), string.Empty, ind.TotalSBScoreLVariance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreLStandDev, attNameExtension), string.Empty, ind.TotalSBScoreLStandDev.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUMean, attNameExtension), string.Empty, ind.TotalSBScoreUMean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUMedian, attNameExtension), string.Empty, ind.TotalSBScoreUMedian.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUVariance, attNameExtension), string.Empty, ind.TotalSBScoreUVariance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSBScoreUStandDev, attNameExtension), string.Empty, ind.TotalSBScoreUStandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label1.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1Mean, attNameExtension), string.Empty, ind.TotalSB1Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1Median, attNameExtension), string.Empty, ind.TotalSB1Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1Variance, attNameExtension), string.Empty, ind.TotalSB1Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB1StandDev, attNameExtension), string.Empty, ind.TotalSB1StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label2.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2Mean, attNameExtension), string.Empty, ind.TotalSB2Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2Median, attNameExtension), string.Empty, ind.TotalSB2Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2Variance, attNameExtension), string.Empty, ind.TotalSB2Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB2StandDev, attNameExtension), string.Empty, ind.TotalSB2StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label3.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3Mean, attNameExtension), string.Empty, ind.TotalSB3Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3Median, attNameExtension), string.Empty, ind.TotalSB3Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3Variance, attNameExtension), string.Empty, ind.TotalSB3Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB3StandDev, attNameExtension), string.Empty, ind.TotalSB3StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label4.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4Mean, attNameExtension), string.Empty, ind.TotalSB4Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4Median, attNameExtension), string.Empty, ind.TotalSB4Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4Variance, attNameExtension), string.Empty, ind.TotalSB4Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB4StandDev, attNameExtension), string.Empty, ind.TotalSB4StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label5.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5Mean, attNameExtension), string.Empty, ind.TotalSB5Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5Median, attNameExtension), string.Empty, ind.TotalSB5Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5Variance, attNameExtension), string.Empty, ind.TotalSB5Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB5StandDev, attNameExtension), string.Empty, ind.TotalSB5StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label6.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6Mean, attNameExtension), string.Empty, ind.TotalSB6Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6Median, attNameExtension), string.Empty, ind.TotalSB6Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6Variance, attNameExtension), string.Empty, ind.TotalSB6Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB6StandDev, attNameExtension), string.Empty, ind.TotalSB6StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label7.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7Mean, attNameExtension), string.Empty, ind.TotalSB7Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7Median, attNameExtension), string.Empty, ind.TotalSB7Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7Variance, attNameExtension), string.Empty, ind.TotalSB7Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB7StandDev, attNameExtension), string.Empty, ind.TotalSB7StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label8.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8Mean, attNameExtension), string.Empty, ind.TotalSB8Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8Median, attNameExtension), string.Empty, ind.TotalSB8Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8Variance, attNameExtension), string.Empty, ind.TotalSB8Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB8StandDev, attNameExtension), string.Empty, ind.TotalSB8StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label9.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9Mean, attNameExtension), string.Empty, ind.TotalSB9Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9Median, attNameExtension), string.Empty, ind.TotalSB9Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9Variance, attNameExtension), string.Empty, ind.TotalSB9Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB9StandDev, attNameExtension), string.Empty, ind.TotalSB9StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label10.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10Mean, attNameExtension), string.Empty, ind.TotalSB10Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10Median, attNameExtension), string.Empty, ind.TotalSB10Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10Variance, attNameExtension), string.Empty, ind.TotalSB10Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB10StandDev, attNameExtension), string.Empty, ind.TotalSB10StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label11.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11Mean, attNameExtension), string.Empty, ind.TotalSB11Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11Median, attNameExtension), string.Empty, ind.TotalSB11Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11Variance, attNameExtension), string.Empty, ind.TotalSB11Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB11StandDev, attNameExtension), string.Empty, ind.TotalSB11StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label12.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12Mean, attNameExtension), string.Empty, ind.TotalSB12Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12Median, attNameExtension), string.Empty, ind.TotalSB12Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12Variance, attNameExtension), string.Empty, ind.TotalSB12Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB12StandDev, attNameExtension), string.Empty, ind.TotalSB12StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label13.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13Mean, attNameExtension), string.Empty, ind.TotalSB13Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13Median, attNameExtension), string.Empty, ind.TotalSB13Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13Variance, attNameExtension), string.Empty, ind.TotalSB13Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB13StandDev, attNameExtension), string.Empty, ind.TotalSB13StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label14.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14Mean, attNameExtension), string.Empty, ind.TotalSB14Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14Median, attNameExtension), string.Empty, ind.TotalSB14Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14Variance, attNameExtension), string.Empty, ind.TotalSB14Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB14StandDev, attNameExtension), string.Empty, ind.TotalSB14StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label15.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15Mean, attNameExtension), string.Empty, ind.TotalSB15Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15Median, attNameExtension), string.Empty, ind.TotalSB15Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15Variance, attNameExtension), string.Empty, ind.TotalSB15Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB15StandDev, attNameExtension), string.Empty, ind.TotalSB15StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label16.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16Mean, attNameExtension), string.Empty, ind.TotalSB16Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16Median, attNameExtension), string.Empty, ind.TotalSB16Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16Variance, attNameExtension), string.Empty, ind.TotalSB16Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB16StandDev, attNameExtension), string.Empty, ind.TotalSB16StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label17.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17Mean, attNameExtension), string.Empty, ind.TotalSB17Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17Median, attNameExtension), string.Empty, ind.TotalSB17Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17Variance, attNameExtension), string.Empty, ind.TotalSB17Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB17StandDev, attNameExtension), string.Empty, ind.TotalSB17StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label18.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18Mean, attNameExtension), string.Empty, ind.TotalSB18Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18Median, attNameExtension), string.Empty, ind.TotalSB18Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18Variance, attNameExtension), string.Empty, ind.TotalSB18Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB18StandDev, attNameExtension), string.Empty, ind.TotalSB18StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label19.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19Mean, attNameExtension), string.Empty, ind.TotalSB19Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19Median, attNameExtension), string.Empty, ind.TotalSB19Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19Variance, attNameExtension), string.Empty, ind.TotalSB19Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB19StandDev, attNameExtension), string.Empty, ind.TotalSB19StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            if (ind.TSB1Label20.Length > 0)
            {
                iSBCount++;
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20Mean, attNameExtension), string.Empty, ind.TotalSB20Mean.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20Median, attNameExtension), string.Empty, ind.TotalSB20Median.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20Variance, attNameExtension), string.Empty, ind.TotalSB20Variance.ToString("N4", CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(string.Empty,
                    string.Concat(cTotalSB20StandDev, attNameExtension), string.Empty, ind.TotalSB20StandDev.ToString("N4", CultureInfo.InvariantCulture));
            }
            //tells ss how many inds to display
            this.SBCount = iSBCount;
        }
        public async Task<bool> RunAnalyses(SB1Stock sb1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (use Total1 object to avoid duplication)
            SB1Total1 total = new SB1Total1(this.CalcParameters);
            //this adds the totals to sb1stock.total1 (not to total)
            //but does not aggregate the indicators (need variance and sd)
            bHasAnalyses = await total.RunAnalyses(sb1Stock);
            if (sb1Stock.Total1 != null)
            {
                //copy at least the stock and substock totals from total1 to stat1
                sb1Stock.Stat1 = new SB1Stat1(this.CalcParameters);
                //this accounts for separate, rather than aggregated, indicators
                bHasAnalyses = CopyTotalIndicatorsToSB1Stock(sb1Stock.Stat1, sb1Stock.Total1);
            }
            return bHasAnalyses;
        }
        
        //calcs holds the collections needing statistical analysis
        public bool RunAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to sb1stock.Stat1
            if (sb1Stock.Stat1 == null)
            {
                sb1Stock.Stat1 = new SB1Stat1(this.CalcParameters);
            }

            if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                //inputs and inputs calcs are calculated as separate observations (mean input price for similar inputs is meaningfull)
                bHasAnalyses = SetIOAnalyses(sb1Stock, calcs);
            }
            else
            {
                //inputs and inputs are not calculated as separate observation (mean input price for dissimilar inputs is meaningless)
                bHasAnalyses = SetAnalyses(sb1Stock, calcs);
            }
            return bHasAnalyses;
        }

        private bool SetAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of sb1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Stat1.SB101Stocks for costs and 2s for benefits
            //set N
            int iQN = 0;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation; alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            //stats lists holds observation.stat1 collection with totals
            List<SB1Stat1> stats = new List<SB1Stat1>();
            foreach (var calcbyalt in calcsByAlt2)
            {
                //set the calc totals in each observation
                SB1Stock observationStock = new SB1Stock();
                observationStock.Stat1 = new SB1Stat1(this.CalcParameters);
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(sb1Stock.GetType()))
                    {
                        SB1Stock stock = (SB1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Stat1 != null)
                            {
                                string sNodeName = sb1Stock.CalcParameters.CurrentElementNodeName;
                                //set two properties
                                stock.Stat1.SubApplicationType = stock.SubApplicationType;
                                stock.Stat1.AnalyzerType = sb1Stock.CalcParameters.AnalyzerParms.AnalyzerType;
                                //tps start substracting outcomes from op/comps
                                if (sNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                                    || sNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                                {
                                    //add stock2 (outputs) to stock1 (inputs) and display stock1 net only
                                    //number of observations reflects both input and input indicators
                                    bHasTotals = AddSubTotalToTotalStock(observationStock.Stat1, stock.Multiplier,
                                        stock.Stat1);
                                }
                                else
                                {
                                    //set each observation's totals
                                    bHasTotals = AddSubTotalToTotalStock(observationStock.Stat1, stock.Multiplier,
                                        stock.Stat1);
                                }
                            }
                        }
                    }
                }
                //add to the stats collection
                stats.Add(observationStock.Stat1);
                //N is determined from the cost SB1Stock
                iQN++;
            }
            if (iQN > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats, sb1Stock);
            }
            return bHasAnalysis;
        }
        private bool SetIOAnalyses(SB1Stock sb1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iQN2 = 0;
            List<SB1Stat1> stats2 = new List<SB1Stat1>();
            //inputs and inputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(sb1Stock.GetType()))
                {
                    SB1Stock stock = (SB1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Stat1 != null)
                        {
                            //set the calc totals in each observation
                            SB1Stock observation2Stock = new SB1Stock();
                            observation2Stock.Stat1 = new SB1Stat1(this.CalcParameters);
                            //set each observation's totals
                            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Stat1,
                                sb1Stock.Multiplier, stock.Stat1);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                stats2.Add(observation2Stock.Stat1);
                                iQN2++;
                            }
                        }
                    }
                }
            }
            if (iQN2 > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats2, sb1Stock);
            }
            return bHasAnalysis;
        }
        private bool SetStatsAnalysis(List<SB1Stat1> stats2, SB1Stock statStock)
        {
            bool bHasTotals = false;
            //set the total observations total
            foreach (var stat in stats2)
            {
                bHasTotals = AddSubTotalToTotalStock(statStock.Stat1, 1, stat);
                bHasTotals = true;
            }
            if (statStock.Stat1.TSB1ScoreN > 0)
            {
                //set the means
                statStock.Stat1.TotalSBScoreMean = statStock.Stat1.TSB1ScoreM / statStock.Stat1.TSB1ScoreN;
                //set the stats
                SetSBScoreStatistics(statStock, stats2);
                statStock.Stat1.TotalSBScoreLMean = statStock.Stat1.TSB1ScoreLAmount / statStock.Stat1.TSB1ScoreN;
                //set the stats
                SetSBScoreLStatistics(statStock, stats2);
                statStock.Stat1.TotalSBScoreUMean = statStock.Stat1.TSB1ScoreUAmount / statStock.Stat1.TSB1ScoreN;
                //set the stats
                SetSBScoreUStatistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N1 > 0)
            {
                //set the means
                statStock.Stat1.TotalSB1Mean = statStock.Stat1.TSB1TMAmount1 / statStock.Stat1.TSB1N1;
                //set the stats
                SetSB1Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N2 > 0)
            {
                statStock.Stat1.TotalSB2Mean = statStock.Stat1.TSB1TMAmount2 / statStock.Stat1.TSB1N2;
                SetSB2Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N3 > 0)
            {
                statStock.Stat1.TotalSB3Mean = statStock.Stat1.TSB1TMAmount3 / statStock.Stat1.TSB1N3;
                SetSB3Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N4 > 0)
            {
                statStock.Stat1.TotalSB4Mean = statStock.Stat1.TSB1TMAmount4 / statStock.Stat1.TSB1N4;
                SetSB4Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N5 > 0)
            {
                statStock.Stat1.TotalSB5Mean = statStock.Stat1.TSB1TMAmount5 / statStock.Stat1.TSB1N5;
                SetSB5Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N6 > 0)
            {
                statStock.Stat1.TotalSB6Mean = statStock.Stat1.TSB1TMAmount6 / statStock.Stat1.TSB1N6;
                SetSB6Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N7 > 0)
            {
                statStock.Stat1.TotalSB7Mean = statStock.Stat1.TSB1TMAmount7 / statStock.Stat1.TSB1N7;
                SetSB7Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N8 > 0)
            {
                statStock.Stat1.TotalSB8Mean = statStock.Stat1.TSB1TMAmount8 / statStock.Stat1.TSB1N8;
                SetSB8Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N9 > 0)
            {
                statStock.Stat1.TotalSB9Mean = statStock.Stat1.TSB1TMAmount9 / statStock.Stat1.TSB1N9;
                SetSB9Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N10 > 0)
            {
                statStock.Stat1.TotalSB10Mean = statStock.Stat1.TSB1TMAmount10 / statStock.Stat1.TSB1N10;
                SetSB10Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N11 > 0)
            {
                statStock.Stat1.TotalSB11Mean = statStock.Stat1.TSB1TMAmount11 / statStock.Stat1.TSB1N11;
                SetSB11Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N12 > 0)
            {
                statStock.Stat1.TotalSB12Mean = statStock.Stat1.TSB1TMAmount12 / statStock.Stat1.TSB1N12;
                SetSB12Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N13 > 0)
            {
                statStock.Stat1.TotalSB13Mean = statStock.Stat1.TSB1TMAmount13 / statStock.Stat1.TSB1N13;
                SetSB13Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N14 > 0)
            {
                statStock.Stat1.TotalSB14Mean = statStock.Stat1.TSB1TMAmount14 / statStock.Stat1.TSB1N14;
                SetSB14Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N15 > 0)
            {
                statStock.Stat1.TotalSB15Mean = statStock.Stat1.TSB1TMAmount15 / statStock.Stat1.TSB1N15;
                SetSB15Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N16 > 0)
            {
                statStock.Stat1.TotalSB16Mean = statStock.Stat1.TSB1TMAmount16 / statStock.Stat1.TSB1N16;
                SetSB16Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N17 > 0)
            {
                statStock.Stat1.TotalSB17Mean = statStock.Stat1.TSB1TMAmount17 / statStock.Stat1.TSB1N17;
                SetSB17Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N18 > 0)
            {
                statStock.Stat1.TotalSB18Mean = statStock.Stat1.TSB1TMAmount18 / statStock.Stat1.TSB1N18;
                SetSB18Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N19 > 0)
            {
                statStock.Stat1.TotalSB19Mean = statStock.Stat1.TSB1TMAmount19 / statStock.Stat1.TSB1N19;
                SetSB19Statistics(statStock, stats2);
            }
            if (statStock.Stat1.TSB1N20 > 0)
            {
                statStock.Stat1.TotalSB20Mean = statStock.Stat1.TSB1TMAmount20 / statStock.Stat1.TSB1N20;
                SetSB20Statistics(statStock, stats2);
            }
            return bHasTotals;
        }
        private static void SetSBScoreStatistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1ScoreM);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1ScoreM - sb1Stock.Stat1.TotalSBScoreMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSBScoreMedian = (stat.TSB1ScoreM + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSBScoreMedian = stat.TSB1ScoreM;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1ScoreM;
            }

            //don't divide by 1ero
            if (sb1Stock.Stat1.TSB1ScoreN > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1ScoreN - 1));
                sb1Stock.Stat1.TotalSBScoreVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSBScoreMean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSBScoreStandDev = Math.Sqrt(sb1Stock.Stat1.TotalSBScoreVariance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSBScoreVariance = 0;
                sb1Stock.Stat1.TotalSBScoreStandDev = 0;
            }
        }
        private static void SetSBScoreLStatistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1ScoreLAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1ScoreLAmount - sb1Stock.Stat1.TotalSBScoreLMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSBScoreLMedian = (stat.TSB1ScoreLAmount + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSBScoreLMedian = stat.TSB1ScoreLAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1ScoreLAmount;
            }

            //don't divide by 1ero
            if (sb1Stock.Stat1.TSB1ScoreN > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1ScoreN - 1));
                sb1Stock.Stat1.TotalSBScoreLVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSBScoreLMean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSBScoreLStandDev = Math.Sqrt(sb1Stock.Stat1.TotalSBScoreLVariance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSBScoreLVariance = 0;
                sb1Stock.Stat1.TotalSBScoreLStandDev = 0;
            }
        }
        private static void SetSBScoreUStatistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1ScoreUAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1ScoreUAmount - sb1Stock.Stat1.TotalSBScoreUMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSBScoreUMedian = (stat.TSB1ScoreUAmount + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSBScoreUMedian = stat.TSB1ScoreUAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1ScoreUAmount;
            }

            //don't divide by 1ero
            if (sb1Stock.Stat1.TSB1ScoreN > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1ScoreN - 1));
                sb1Stock.Stat1.TotalSBScoreUVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSBScoreUMean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSBScoreUStandDev = Math.Sqrt(sb1Stock.Stat1.TotalSBScoreUVariance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSBScoreUVariance = 0;
                sb1Stock.Stat1.TotalSBScoreUStandDev = 0;
            }
        }
        private static void SetSB1Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount1);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount1 - sb1Stock.Stat1.TotalSB1Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB1Median = (stat.TSB1TMAmount1 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB1Median = stat.TSB1TMAmount1;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount1;
            }

            //don't divide by 1ero
            if (sb1Stock.Stat1.TSB1N1 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N1 - 1));
                sb1Stock.Stat1.TotalSB1Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB1Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB1StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB1Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB1Variance = 0;
                sb1Stock.Stat1.TotalSB1StandDev = 0;
            }
        }
        private static void SetSB2Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount2);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount2 - sb1Stock.Stat1.TotalSB2Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB2Median = (stat.TSB1TMAmount2 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB2Median = stat.TSB1TMAmount2;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount2;
            }

            //don't divide by 2ero
            if (sb1Stock.Stat1.TSB1N2 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N2 - 1));
                sb1Stock.Stat1.TotalSB2Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB2Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB2StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB2Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB2Variance = 0;
                sb1Stock.Stat1.TotalSB2StandDev = 0;
            }
        }
        private static void SetSB3Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount3);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount3 - sb1Stock.Stat1.TotalSB3Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB3Median = (stat.TSB1TMAmount3 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB3Median = stat.TSB1TMAmount3;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount3;
            }

            //don't divide by 3ero
            if (sb1Stock.Stat1.TSB1N3 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N3 - 1));
                sb1Stock.Stat1.TotalSB3Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB3Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB3StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB3Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB3Variance = 0;
                sb1Stock.Stat1.TotalSB3StandDev = 0;
            }
        }
        private static void SetSB4Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount4);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount4 - sb1Stock.Stat1.TotalSB4Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB4Median = (stat.TSB1TMAmount4 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB4Median = stat.TSB1TMAmount4;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount4;
            }

            //don't divide by 4ero
            if (sb1Stock.Stat1.TSB1N4 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N4 - 1));
                sb1Stock.Stat1.TotalSB4Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB4Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB4StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB4Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB4Variance = 0;
                sb1Stock.Stat1.TotalSB4StandDev = 0;
            }
        }
        private static void SetSB5Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount5);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount5 - sb1Stock.Stat1.TotalSB5Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB5Median = (stat.TSB1TMAmount5 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB5Median = stat.TSB1TMAmount5;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount5;
            }

            //don't divide by 5ero
            if (sb1Stock.Stat1.TSB1N5 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N5 - 1));
                sb1Stock.Stat1.TotalSB5Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB5Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB5StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB5Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB5Variance = 0;
                sb1Stock.Stat1.TotalSB5StandDev = 0;
            }
        }
        private static void SetSB6Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount6);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount6 - sb1Stock.Stat1.TotalSB6Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB6Median = (stat.TSB1TMAmount6 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB6Median = stat.TSB1TMAmount6;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount6;
            }

            //don't divide by 6ero
            if (sb1Stock.Stat1.TSB1N6 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N6 - 1));
                sb1Stock.Stat1.TotalSB6Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB6Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB6StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB6Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB6Variance = 0;
                sb1Stock.Stat1.TotalSB6StandDev = 0;
            }
        }
        private static void SetSB7Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount7);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount7 - sb1Stock.Stat1.TotalSB7Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB7Median = (stat.TSB1TMAmount7 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB7Median = stat.TSB1TMAmount7;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount7;
            }

            //don't divide by 7ero
            if (sb1Stock.Stat1.TSB1N7 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N7 - 1));
                sb1Stock.Stat1.TotalSB7Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB7Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB7StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB7Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB7Variance = 0;
                sb1Stock.Stat1.TotalSB7StandDev = 0;
            }
        }
        private static void SetSB8Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount8);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount8 - sb1Stock.Stat1.TotalSB8Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB8Median = (stat.TSB1TMAmount8 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB8Median = stat.TSB1TMAmount8;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount8;
            }

            //don't divide by 8ero
            if (sb1Stock.Stat1.TSB1N8 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N8 - 1));
                sb1Stock.Stat1.TotalSB8Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB8Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB8StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB8Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB8Variance = 0;
                sb1Stock.Stat1.TotalSB8StandDev = 0;
            }
        }
        private static void SetSB9Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount9);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount9 - sb1Stock.Stat1.TotalSB9Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB9Median = (stat.TSB1TMAmount9 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB9Median = stat.TSB1TMAmount9;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount9;
            }

            //don't divide by 9ero
            if (sb1Stock.Stat1.TSB1N9 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N9 - 1));
                sb1Stock.Stat1.TotalSB9Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB9Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB9StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB9Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB9Variance = 0;
                sb1Stock.Stat1.TotalSB9StandDev = 0;
            }
        }
        private static void SetSB10Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount10);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount10 - sb1Stock.Stat1.TotalSB10Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB10Median = (stat.TSB1TMAmount10 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB10Median = stat.TSB1TMAmount10;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount10;
            }

            //don't divide by 10ero
            if (sb1Stock.Stat1.TSB1N10 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N10 - 1));
                sb1Stock.Stat1.TotalSB10Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB10Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB10StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB10Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB10Variance = 0;
                sb1Stock.Stat1.TotalSB10StandDev = 0;
            }
        }
        private static void SetSB11Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount11);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount11 - sb1Stock.Stat1.TotalSB11Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB11Median = (stat.TSB1TMAmount11 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB11Median = stat.TSB1TMAmount11;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount11;
            }

            //don't divide by 11ero
            if (sb1Stock.Stat1.TSB1N1 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N11 - 1));
                sb1Stock.Stat1.TotalSB11Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB11Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB11StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB11Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB11Variance = 0;
                sb1Stock.Stat1.TotalSB11StandDev = 0;
            }
        }
        private static void SetSB12Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount12);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount12 - sb1Stock.Stat1.TotalSB12Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB12Median = (stat.TSB1TMAmount12 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB12Median = stat.TSB1TMAmount12;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount12;
            }

            //don't divide by 12ero
            if (sb1Stock.Stat1.TSB1N12 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N12 - 1));
                sb1Stock.Stat1.TotalSB12Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB12Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB12StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB12Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB12Variance = 0;
                sb1Stock.Stat1.TotalSB12StandDev = 0;
            }
        }
        private static void SetSB13Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount13);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount13 - sb1Stock.Stat1.TotalSB13Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB13Median = (stat.TSB1TMAmount13 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB13Median = stat.TSB1TMAmount13;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount13;
            }

            //don't divide by 13ero
            if (sb1Stock.Stat1.TSB1N13 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N13 - 1));
                sb1Stock.Stat1.TotalSB13Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB13Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB13StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB13Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB13Variance = 0;
                sb1Stock.Stat1.TotalSB13StandDev = 0;
            }
        }
        private static void SetSB14Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount14);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount14 - sb1Stock.Stat1.TotalSB14Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB14Median = (stat.TSB1TMAmount14 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB14Median = stat.TSB1TMAmount14;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount14;
            }

            //don't divide by 14ero
            if (sb1Stock.Stat1.TSB1N14 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N14 - 1));
                sb1Stock.Stat1.TotalSB14Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB14Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB14StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB14Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB14Variance = 0;
                sb1Stock.Stat1.TotalSB14StandDev = 0;
            }
        }
        private static void SetSB15Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount15);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount15 - sb1Stock.Stat1.TotalSB15Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB15Median = (stat.TSB1TMAmount15 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB15Median = stat.TSB1TMAmount15;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount15;
            }

            //don't divide by 15ero
            if (sb1Stock.Stat1.TSB1N15 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N15 - 1));
                sb1Stock.Stat1.TotalSB15Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB15Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB15StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB15Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB15Variance = 0;
                sb1Stock.Stat1.TotalSB15StandDev = 0;
            }
        }
        private static void SetSB16Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount16);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount16 - sb1Stock.Stat1.TotalSB16Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB16Median = (stat.TSB1TMAmount16 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB16Median = stat.TSB1TMAmount16;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount16;
            }

            //don't divide by 16ero
            if (sb1Stock.Stat1.TSB1N16 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N16 - 1));
                sb1Stock.Stat1.TotalSB16Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB16Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB16StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB16Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB16Variance = 0;
                sb1Stock.Stat1.TotalSB16StandDev = 0;
            }
        }
        private static void SetSB17Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount17);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount17 - sb1Stock.Stat1.TotalSB17Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB17Median = (stat.TSB1TMAmount17 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB17Median = stat.TSB1TMAmount17;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount17;
            }

            //don't divide by 17ero
            if (sb1Stock.Stat1.TSB1N17 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N17 - 1));
                sb1Stock.Stat1.TotalSB17Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB17Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB17StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB17Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB17Variance = 0;
                sb1Stock.Stat1.TotalSB17StandDev = 0;
            }
        }
        private static void SetSB18Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount18);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount18 - sb1Stock.Stat1.TotalSB18Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB18Median = (stat.TSB1TMAmount18 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB18Median = stat.TSB1TMAmount18;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount18;
            }

            //don't divide by 18ero
            if (sb1Stock.Stat1.TSB1N18 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N18 - 1));
                sb1Stock.Stat1.TotalSB18Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB18Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB18StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB18Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB18Variance = 0;
                sb1Stock.Stat1.TotalSB18StandDev = 0;
            }
        }
        private static void SetSB19Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount19);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount19 - sb1Stock.Stat1.TotalSB19Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB19Median = (stat.TSB1TMAmount19 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB19Median = stat.TSB1TMAmount19;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount19;
            }

            //don't divide by 19ero
            if (sb1Stock.Stat1.TSB1N19 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N19 - 1));
                sb1Stock.Stat1.TotalSB19Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB19Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB19StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB19Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB19Variance = 0;
                sb1Stock.Stat1.TotalSB19StandDev = 0;
            }
        }
        private static void SetSB20Statistics(SB1Stock sb1Stock, List<SB1Stat1> stats)
        {
            //reorder for median
            IEnumerable<SB1Stat1> stat2s = stats.OrderByDescending(s => s.TSB1TMAmount20);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (SB1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TSB1TMAmount20 - sb1Stock.Stat1.TotalSB20Mean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        sb1Stock.Stat1.TotalSB20Median = (stat.TSB1TMAmount20 + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        sb1Stock.Stat1.TotalSB20Median = stat.TSB1TMAmount20;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TSB1TMAmount20;
            }

            //don't divide by 20ero
            if (sb1Stock.Stat1.TSB1N20 > 1)
            {
                //sample variance
                double dbCount = (1 / (sb1Stock.Stat1.TSB1N20 - 1));
                sb1Stock.Stat1.TotalSB20Variance = dbMemberSquaredTotalQ1 * dbCount;
                if (sb1Stock.Stat1.TotalSB20Mean != 0)
                {
                    //sample standard deviation
                    sb1Stock.Stat1.TotalSB20StandDev = Math.Sqrt(sb1Stock.Stat1.TotalSB20Variance);
                }
            }
            else
            {
                sb1Stock.Stat1.TotalSB20Variance = 0;
                sb1Stock.Stat1.TotalSB20StandDev = 0;
            }
        }
    }
}
