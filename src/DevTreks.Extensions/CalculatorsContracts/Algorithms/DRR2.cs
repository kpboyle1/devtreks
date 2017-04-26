using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using MathNet.Numerics.LinearAlgebra;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		DRR2 algorithm
    ///Author:		www.devtreks.org
    ///Date:		2017, April
    ///References:	CTA algo1, CTAP subalgo 9, 10, 11, 12, RCA subalgo 13
    ///</summary>
    public class DRR2 : DRR1
    {
        public DRR2()
            : base()
        { }
        private const int trendPeriods = 7;
        /// <summary>
        /// Initialize the DRR1 algorithm
        /// </summary>
        /// <param name="mathTerms">Math Expression terms, in format Ix.Qx. Potential use in vector math.</param>
        public DRR2(int indicatorIndex, string label, string[] mathTerms, 
            string[] colNames, string[] depColNames,
            int totalsNeeded, string subalgorithm, int ciLevel, 
            int iterations, int random, List<double> qTs, IndicatorQT1 qT1, 
            CalculatorParameters calcParams)
            : base(indicatorIndex, label, mathTerms,
                colNames, depColNames, totalsNeeded,
                subalgorithm, ciLevel, iterations,
                random, qTs, qT1, calcParams)
        {
        }
        //set up a list of concurrent tasks to run
        private List<Task<Vector<double>>> _runTask2s = new List<Task<Vector<double>>>();

        //this is asych for the calling Task.WhenAll
        //but does not necessarily need internal asych awaits
        public async Task RunAlgorithmAsync2(List<List<string>> data,
            List<List<string>> rowNames, List<string> lines2)
        {
            try
            {
                //the bool allows errors to be propagated
                bool bHasCalculations = false;
                //minimal data requirement is first five cols
                if (ColNames.Count() < 5)
                {
                    IndicatorQT.ErrorMessage = "RMI analysis requires at least 1 output variable and 1 input variable.";
                    return;
                }
                if (data.Count() < 3 && IndicatorIndex != 2)
                {
                    //185 same as other analysis
                    IndicatorQT.ErrorMessage = "RMI requires at least 3 rows of data distributions.";
                    return;
                }
                if (data.Count() != rowNames.Count)
                {
                    //185 same as other analysis
                    IndicatorQT.ErrorMessage = "The number of rows of numeric data don't match the number of string rows used to report the data. An Indicator.URL dataset is formatted incorrectly.";
                    return;
                }
                //subalgo11 uses 1 and subalgo10 uses 2
                if (((IndicatorIndex == 1 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()))
                    && lines2.Count == 0)
                {
                    //4 level indicator systems with weighted avgs
                    bHasCalculations = await Calculate4LevelIndicators(data, rowNames);
                }
                else if ((IndicatorIndex == 0 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    || (IndicatorIndex == 1 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    || (IndicatorIndex == 2 && _subalgorithm == MATH_SUBTYPES.subalgorithm9.ToString())
                    || (IndicatorIndex == 2 && _subalgorithm == MATH_SUBTYPES.subalgorithm10.ToString()))
                {
                    //4 level indicator systems with no weighted avgs
                    bHasCalculations = await Calculate4LevelIndicators(data, rowNames);
                }
                else if (((IndicatorIndex == 2 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                    || (IndicatorIndex == 2 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    || (IndicatorIndex == 5 && _subalgorithm == MATH_SUBTYPES.subalgorithm9.ToString())
                    || (IndicatorIndex == 5 && _subalgorithm == MATH_SUBTYPES.subalgorithm10.ToString()))
                    && lines2.Count > 0)
                {
                    //convert lines 2 to rates and life span doubles
                    //0.05 0.12
                    //10   25
                    DataSet2 = GetRatesandLifes(lines2);
                    bHasCalculations = await CalculateIndicators2(data, rowNames);
                }
                else if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString()
                    || _subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
                {
                    //4 level indicator systems
                    bHasCalculations = await Calculate4LevelIndicators(data, rowNames);
                }
                //put the results in MathResult
                SetMathResult(rowNames);
            }
            catch (Exception ex)
            {
                IndicatorQT.ErrorMessage = ex.Message;
            }
        }
        public async Task<bool> Calculate4LevelIndicators(List<List<string>> data,
            List<List<string>> rowNames)
        {
            bool bHasCalculations = false;
            //make a new list with same matrix, to be replaced with results
            int iColCount = data[0].Count;
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                //need time trends plus QTMs, QTLs, and QTUs
                iColCount = data[0].Count + 4;
            }
            DataResults = CalculatorHelpers.GetList(data.Count, iColCount);
            string sCatIndexLabel = string.Empty;
            string sAlternative = string.Empty;
            int iLocationIndex = 0;
            int iCatIndex = 0;
            //if needed for means
            int iDataCountNoCategories = 0;
            //display of location total damages
            int iQLocation = 1;
            //total exposure will be displayed
            IndicatorQT.QTM = 0;
            IndicatorQT.QTL = 0;
            IndicatorQT.QTU = 0;
            //highest alternative aggregated across locations
            IndicatorQT1 ScoreIndicator = new IndicatorQT1();
            //total risk, TR (used to aggreg Locations into final scores)
            IndicatorQT1 LocationIndicator = new IndicatorQT1();
            //partial risk, RF (thirdindicator.qt1inds are used to aggreg RFs into TRs)
            IndicatorQT1 ThirdIndicator = new IndicatorQT1();
            //holds the list of twoPRA1
            List<PRA1> twoIndexes = new List<PRA1>();
            //holds the lists of two indexes
            Dictionary<PRA1, List<PRA1>> threeIndexes
                = new Dictionary<PRA1, List<PRA1>>();
            PRA1 pra1 = new PRA1(this);
            PRA1 twoPRA1 = new PRA1(this);
            PRA1 threePRA1 = new PRA1(this);
            PRA1 fourPRA1 = new PRA1(this);
            //this is the row
            for (int r = 0; r < data.Count(); r++)
            {
                //labels are in the first column of row names
                if (rowNames.Count > r)
                {
                    //some algos also include _A alternative suffixes
                    sCatIndexLabel
                        = CalculatorHelpers.GetParsedString(0, Constants.FILENAME_DELIMITERS, rowNames[r][0]);
                    sAlternative
                        = CalculatorHelpers.GetParsedString(1, Constants.FILENAME_DELIMITERS, rowNames[r][0]);
                    iLocationIndex = CalculatorHelpers.ConvertStringToInt(rowNames[r][1]);
                    //each categorical index has 3 chars (RF1) or includes an underscore in 3rd char (RF_1), 
                    //subinds have 4 chars (RF1A), scores have 2 chars (RF)
                    if (IsCategoricalIndex(sCatIndexLabel))
                    {
                        if (twoIndexes.Count > 0)
                        {
                            threeIndexes.Add(twoPRA1, twoIndexes);
                        }
                        //initialize
                        iCatIndex = r;
                        twoIndexes = new List<PRA1>();
                        twoPRA1 = new PRA1(this);
                        //fill in pra1.Indicator1, but don't run independent dists yet
                        FillIndicatorDistribution(data, rowNames, r, twoPRA1);
                    }
                    else if (sCatIndexLabel.Count() == 2)
                    {
                        if (sCatIndexLabel == "TR")
                        {
                            //store TR in Location for later aggregation of TRs
                            FillLocationIndicator(data, rowNames, r, sAlternative, iLocationIndex, LocationIndicator);
                            if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()
                                || _subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString()
                                || _subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
                            {
                                //resiliency indexes or mcas in scores, no weighted avg
                                SetTRDataResult(r, iColCount, ThirdIndicator, LocationIndicator);
                                //don't need the locationindex but need to display Qs
                                iQLocation++;
                                //don't init location indicator --need full collection of QT1s
                            }
                            else
                            {
                                //set the location (total risk index or risk mngt index)
                                //subalgo 11 uses wtd avgs subalgo 10 uses Moncho with no wtd avgs
                                SetLocationDataResult(r, iLocationIndex, ThirdIndicator, LocationIndicator);
                                iQLocation++;
                            }
                            ScoreIndicator.IndicatorQT1s.Add(LocationIndicator);
                            //init location indicator
                            LocationIndicator = new IndicatorQT1();
                            //init third indicator for next location
                            ThirdIndicator = new IndicatorQT1();
                            //reinitialize Indexes for next location
                            threeIndexes = new Dictionary<PRA1, List<PRA1>>();
                            twoIndexes = new List<PRA1>();
                        }
                        else
                        {
                            if (twoIndexes.Count > 0)
                            {
                                threeIndexes.Add(twoPRA1, twoIndexes);
                                //this holds the aggindex used by TR
                                threePRA1 = new PRA1(this);
                                //fill in pra1.Indicator1, but don't run independent dists yet
                                FillIndicatorDistribution(data, rowNames, r, threePRA1);
                                twoIndexes = new List<PRA1>();
                                threeIndexes.Add(threePRA1, twoIndexes);
                            }
                            //normalize, weight, aggregate all of the data
                            await SetCategoryAndIndicatorDataResult(iLocationIndex,
                                threeIndexes, data[r], r, ThirdIndicator);
                            //reinitialize Indexes for next location
                            threeIndexes = new Dictionary<PRA1, List<PRA1>>();
                            twoIndexes = new List<PRA1>();
                        }
                    }
                    else
                    {
                        iDataCountNoCategories++;
                        pra1 = new PRA1(this);
                        FillIndicatorDistribution(data, rowNames, r, pra1);
                        //fill in pra1.Indicator1 (only place where dists are run)
                        PRA1 pra = CalculateSubIndicators(pra1);
                        twoIndexes.Add(pra);
                    }
                }
                else
                {
                    //skip to the next row
                }
            }
            FillIndicatorQT(ScoreIndicator);
            //made it this far without an error, so good calcs
            bHasCalculations = true;
            return bHasCalculations;
        }
        private static bool IsCategoricalIndex(string label)
        {
            bool bIsCatIndex = false;
            if (label.Count() == 3)
            {
                //rf1
                bIsCatIndex = true;
            }
            return bIsCatIndex;
        }
        private void FillIndicatorDistribution(List<List<string>> data, List<List<string>> rowNames,
            int r, PRA1 pra1)
        {
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                FillIndicatorDistributionForRCA1(data, rowNames, r, pra1);
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                FillIndicatorDistributionForRCA2(data, rowNames, r, pra1);
            }
            else
            {
                //iterate through columns, skipping y column
                for (int c = 0; c < data[r].Count; c++)
                {
                    if (c == 0)
                    {
                        //don't need totals in data but do need label in rowNames
                        pra1.IndicatorQT.Label = rowNames[r][0];
                    }
                    else if (c == 1)
                    {
                        pra1.IndicatorQT.QDistributionType = data[r][c];
                    }
                    else if (c == 2)
                    {
                        pra1.IndicatorQT.QT
                            = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                    }
                    else if (c == 3)
                    {
                        pra1.IndicatorQT.QTUnit = data[r][c];
                    }
                    else if (c == 4)
                    {
                        pra1.IndicatorQT.QTD1
                            = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                    }
                    else if (c == 5)
                    {
                        pra1.IndicatorQT.QTD1Unit = data[r][c];
                    }
                    else if (c == 6)
                    {
                        pra1.IndicatorQT.QTD2
                            = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                    }
                    else if (c == 7)
                    {
                        pra1.IndicatorQT.QTD2Unit = data[r][c];
                    }
                    else if (c == 8)
                    {
                        //q1unit stores normalization type 
                        pra1.IndicatorQT.Q1Unit = data[r][c];
                    }
                    else if (c == 9)
                    {
                        //q1 stores weight
                        pra1.IndicatorQT.Q1
                            = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                    }
                    else if (c == 10)
                    {
                        //q2 stores quantity of assets
                        //gets added to last column of mathresults
                        //then gets added to last column of Indicator4.MathResults
                        //then acts as a unit multiplier for costs in bcrs and ceas
                        if (data[r][c] != null)
                        {
                            pra1.IndicatorQT.Q2
                                = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                        }
                    }
                }
            }
        }
        private void FillIndicatorDistributionForRCA1(List<List<string>> data, List<List<string>> rowNames,
            int r, PRA1 pra1)
        {
            //iterate through columns, skipping y column
            for (int c = 0; c < data[r].Count; c++)
            {
                if (c == 0)
                {
                    //need label in rowNames
                    pra1.IndicatorQT.Label = rowNames[r][0];
                    pra1.IndicatorQT.QDistributionType = data[r][c];
                }
                else if (c == 1)
                {
                    pra1.IndicatorQT.QT
                           = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 2)
                {
                    pra1.IndicatorQT.QTUnit = data[r][c];
                }
                else if (c == 3)
                {
                    pra1.IndicatorQT.QTD1
                        = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 4)
                {
                    pra1.IndicatorQT.QTD1Unit = data[r][c];
                }
                else if (c == 5)
                {
                    pra1.IndicatorQT.QTD2
                        = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 6)
                {
                    pra1.IndicatorQT.QTD2Unit = data[r][c];
                }
                else if (c == 7)
                {
                    //q3 stores certainty1
                    pra1.IndicatorQT.Q3 = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 8)
                {
                    //q4 stores certainty2
                    pra1.IndicatorQT.Q4 = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 9)
                {
                    //same convention
                    //q1unit stores normalization type 
                    pra1.IndicatorQT.Q1Unit = data[r][c];
                }
                else if (c == 10)
                {
                    //q1 stores weight
                    pra1.IndicatorQT.Q1
                        = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                    //use existing code that uses q2 as a multiplier
                    pra1.IndicatorQT.Q2 = 1;
                }
            }
        }
        private void FillIndicatorDistributionForRCA2(List<List<string>> data, List<List<string>> rowNames,
            int r, PRA1 pra1)
        {
            //iterate through columns, skipping y column
            for (int c = 0; c < data[r].Count; c++)
            {
                if (c == 0)
                {
                    //need label in rowNames
                    pra1.IndicatorQT.Label = rowNames[r][0];
                    //zero index holds trend period 
                    pra1.IndicatorQT.Indicators = new string[trendPeriods];
                    pra1.IndicatorQT.Indicators[0] = data[r][c];
                }
                else if (c == 1)
                {
                    pra1.IndicatorQT.Indicators[1] = data[r][c];
                }
                else if (c == 2)
                {
                    pra1.IndicatorQT.Indicators[2] = data[r][c];
                }
                else if (c == 3)
                {
                    pra1.IndicatorQT.Indicators[3] = data[r][c];
                }
                else if (c == 4)
                {
                    pra1.IndicatorQT.Indicators[4] = data[r][c];
                }
                else if (c == 5)
                {
                    pra1.IndicatorQT.Indicators[5] = data[r][c];
                }
                else if (c == 6)
                {
                    pra1.IndicatorQT.Indicators[6] = data[r][c];
                }
                else if (c == 7)
                {
                    //q3 stores certainty1
                    pra1.IndicatorQT.Q3 = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 8)
                {
                    //q4 stores certainty2
                    pra1.IndicatorQT.Q4 = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 9)
                {
                    //same convention
                    //q1unit stores normalization type 
                    pra1.IndicatorQT.Q1Unit = data[r][c];
                }
                else if (c == 10)
                {
                    //q1 stores weight
                    pra1.IndicatorQT.Q1
                        = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
            }
        }
        
        private void FillLocationIndicator(List<List<string>> data, List<List<string>> rowNames,
            int r, string altName, int location, IndicatorQT1 locationIndicator)
        {
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString()
                || _subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                FillLocationTrendIndicator(data, rowNames, r, altName, location, locationIndicator);
            }
            else
            {
                locationIndicator.Label = "TR";
                locationIndicator.AlternativeType = altName;
                locationIndicator.Alternative2 = location;
                if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                {
                    locationIndicator.QTMUnit = "risk mngt index";
                }
                else if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                {
                    locationIndicator.QTMUnit = "resiliency index";
                }
                else if (_subalgorithm
                    == MATH_SUBTYPES.subalgorithm9.ToString())
                {
                    locationIndicator.QTMUnit = "total asset values";
                }
                else
                {
                    locationIndicator.QTMUnit = "total risk";
                }
                for (int c = 0; c < data[r].Count; c++)
                {
                    if (c == 0)
                    {
                        //don't need totals in data but do need label in rowNames
                        locationIndicator.Label = rowNames[r][0];
                    }
                    else if (c == 1)
                    {
                        //not used; but retain for potential future use
                        //could be independently distributed
                        //same with columns 2 to 10
                        locationIndicator.QDistributionType = data[r][c];
                    }
                    else if (c == 8)
                    {
                        if (data[r][c] != null)
                        {
                            //q1unit stores normalization type 
                            locationIndicator.Q1Unit = data[r][c];
                        }
                    }
                    else if (c == 9)
                    {
                        if (data[r][c] != null)
                        {
                            //q1 stores weight
                            locationIndicator.Q1
                            = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                        }
                    }
                    else if (c == 10)
                    {
                        if (data[r][c] != null)
                        {
                            locationIndicator.Q2
                                = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                        }
                    }
                }
            }
        }
        private void FillLocationTrendIndicator(List<List<string>> data, List<List<string>> rowNames,
            int r, string altName, int location, IndicatorQT1 locationIndicator)
        {
            locationIndicator.Label = "TR";
            locationIndicator.AlternativeType = altName;
            locationIndicator.Alternative2 = location;
            //not used by these subalgos, but can reuse exising source
            locationIndicator.Q2 = 1;
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString()
                || _subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                locationIndicator.QTMUnit = "performance score";
            }
            else
            {
                locationIndicator.QTMUnit = "total risk";
            }
            for (int c = 0; c < data[r].Count; c++)
            {
                if (c == 0)
                {
                    //don't need totals in data but do need label in rowNames
                    locationIndicator.Label = rowNames[r][0];
                    if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
                    {
                        locationIndicator.QDistributionType = data[r][c];
                    }
                }
                if (c == 7)
                {
                    //q3 stores certainty1
                    locationIndicator.Q3 = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 8)
                {
                    //q4 stores certainty2
                    locationIndicator.Q4 = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                }
                else if (c == 9)
                {
                    if (data[r][c] != null)
                    {
                        //q1unit stores normalization type 
                        locationIndicator.Q1Unit = data[r][c];
                    }
                }
                else if (c == 10)
                {
                    if (data[r][c] != null)
                    {
                        //q1 stores weight
                        locationIndicator.Q1
                            = CalculatorHelpers.ConvertStringToDouble(data[r][c]);
                    }
                }
            }
        }
        private void FillIndicatorQT(IndicatorQT1 scoreIndicator)
        {
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                FillTrendIndicatorQT(scoreIndicator);
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                FillTrendIndicatorQT(scoreIndicator);
            }
            else
            {
                //fill in IndicatorQT with location or alternative with highest qtms
                int i = 0;
                if (scoreIndicator.IndicatorQT1s != null)
                {
                    foreach (var location in scoreIndicator.IndicatorQT1s)
                    {
                        //convention is to start with location = 1
                        i++;
                        if (IndicatorIndex == 2)
                        {
                            IndicatorQT.QTM += location.QTM;
                            IndicatorQT.QTL += location.QTL;
                            IndicatorQT.QTU += location.QTU;
                            if (IndicatorIndex == 0)
                            {
                                IndicatorQT.QTMUnit = "mca all locations";
                            }
                            else if (IndicatorIndex == 1)
                            {
                                IndicatorQT.QTMUnit = "ri all locations";
                            }
                            else
                            {
                                IndicatorQT.QTMUnit = "drr all locations";
                            }
                            IndicatorQT.QTLUnit = location.QTLUnit;
                            IndicatorQT.QTUUnit = location.QTUUnit;
                            //calculated based on locations
                            FillIndicatorQs(location, "location ", i);
                        }
                        else
                        {
                            //calculate based on project alternatives
                            if (IndicatorQT.IndicatorQT1s
                                .Any(l => l.AlternativeType
                                == location.AlternativeType))
                            {
                                IndicatorQT1 AltIndicator
                                    = IndicatorQT.IndicatorQT1s
                                    .FirstOrDefault(l => l.AlternativeType
                                    == location.AlternativeType);
                                if (AltIndicator != null)
                                {
                                    AltIndicator.QTM += location.QTM;
                                    AltIndicator.QTL += location.QTL;
                                    AltIndicator.QTU += location.QTU;
                                }
                            }
                            else
                            {
                                IndicatorQT.IndicatorQT1s.Add(location);
                            }
                        }
                    }
                    if (IndicatorIndex != 2)
                    {
                        i = 0;
                        IndicatorQT.QTM = 0;
                        foreach (var alt in IndicatorQT.IndicatorQT1s)
                        {
                            i++;
                            //TR is a required convention for all subalgos
                            if (alt.AlternativeType != "TR")
                            {
                                //report alt with highest score
                                if (alt.QTM > IndicatorQT.QTM)
                                {
                                    IndicatorQT.QTM = alt.QTM;
                                    IndicatorQT.QTL = alt.QTL;
                                    IndicatorQT.QTU = alt.QTU;
                                    if (IndicatorIndex == 0)
                                    {
                                        IndicatorQT.QTMUnit = string.Concat("mca all locations for ", alt.AlternativeType);
                                    }
                                    else if (IndicatorIndex == 1)
                                    {
                                        IndicatorQT.QTMUnit = string.Concat("ri all locations for ", alt.AlternativeType);
                                    }
                                    else
                                    {
                                        IndicatorQT.QTMUnit = string.Concat("drr all locations for ", alt.AlternativeType);
                                    }
                                    IndicatorQT.QTLUnit = alt.QTLUnit;
                                    IndicatorQT.QTUUnit = alt.QTUUnit;
                                }
                            }
                            else
                            {
                                //report baseline score as Score.ScoreQT, QTD1 and QTD2
                                IndicatorQT.QT = alt.QTM;
                                IndicatorQT.QTUnit = "base MCA score";
                                IndicatorQT.QTD1 = alt.QTL;
                                IndicatorQT.QTD2 = alt.QTU;
                                IndicatorQT.QTD1Unit = alt.QTLUnit;
                                IndicatorQT.QTD2Unit = alt.QTUUnit;
                            }
                        }
                    }
                }
            }
        }
        private void FillIndicatorQs(IndicatorQT1 locorAltIndicator,
            string locorAltName, int locOrAltNum)
        {
            if (locOrAltNum == 1)
            {
                IndicatorQT.Q1 = locorAltIndicator.QTM;
                IndicatorQT.Q1Unit
                    = string.Concat(locorAltName, locOrAltNum.ToString(), " total value");
                IndicatorQT.QT = locorAltIndicator.QTM;
                IndicatorQT.QTUnit
                    = string.Concat(locorAltName, locOrAltNum.ToString(), " total value");
                //init
                IndicatorQT.Q2 = 0;
                IndicatorQT.Q2Unit = Constants.NONE;
                IndicatorQT.Q3 = 0;
                IndicatorQT.Q3Unit = Constants.NONE;
                IndicatorQT.Q4 = 0;
                IndicatorQT.Q4Unit = Constants.NONE;
                IndicatorQT.Q5 = 0;
                IndicatorQT.Q5Unit = Constants.NONE;
            }
            else if (locOrAltNum == 2)
            {
                IndicatorQT.Q2 = locorAltIndicator.QTM;
                IndicatorQT.Q2Unit
                    = string.Concat(locorAltName, locOrAltNum.ToString(), " total value");
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1, 2 totals";
            }
            else if (locOrAltNum == 3)
            {
                IndicatorQT.Q3 = locorAltIndicator.QTM;
                IndicatorQT.Q3Unit
                    = string.Concat(locorAltName, locOrAltNum.ToString(), " total value");
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1, 2, 3 totals";
            }
            else if (locOrAltNum == 4)
            {
                IndicatorQT.Q4 = locorAltIndicator.QTM;
                IndicatorQT.Q4Unit
                    = string.Concat(locorAltName, locOrAltNum.ToString(), " total value");
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1, 2, 3, 4 totals";
            }
            else if (locOrAltNum == 5)
            {
                IndicatorQT.Q5 = locorAltIndicator.QTM;
                IndicatorQT.Q5Unit
                    = string.Concat(locorAltName, locOrAltNum.ToString(), " total value");
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1, 2, 3, 4, 5 totals";
            }
            else if (locOrAltNum == 6)
            {
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1 to 6 totals";
            }
            else if (locOrAltNum == 7)
            {
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1 to 7 totals";
            }
            else if (locOrAltNum == 8)
            {
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1 to 8 totals";
            }
            else if (locOrAltNum == 9)
            {
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1 to 9 totals";
            }
            else if (locOrAltNum == 10)
            {
                IndicatorQT.QT += locorAltIndicator.QTM;
                IndicatorQT.QTUnit = "1 to 10 totals";
            }
        }
        private void FillTrendIndicatorQT(IndicatorQT1 scoreIndicator)
        {
            //fill in IndicatorQT with location or alternative with highest qtms
            IndicatorQT.Q1 = 0;
            IndicatorQT.Q2 = 0;
            IndicatorQT.Q3 = 0;
            IndicatorQT.Q4 = 0;
            IndicatorQT.Q5 = 0;
            IndicatorQT.QTM = 0;
            IndicatorQT.QTL = 0;
            IndicatorQT.QTU = 0;
            IndicatorQT.QT = 0;
            IndicatorQT.QTD1 = 0;
            IndicatorQT.QTD2 = 0;
            int i = 0;
            if (scoreIndicator.IndicatorQT1s != null)
            {
                foreach (var location in scoreIndicator.IndicatorQT1s)
                {
                    if (location.AlternativeType == "TR")
                    {
                        IndicatorQT.Q4 += location.QTM;
                        IndicatorQT.Q5 += location.QTL;
                        IndicatorQT.QT += location.QTU;
                        IndicatorQT.Q4Unit = "benchmark most score";
                        IndicatorQT.Q5Unit = "benchmark low score";
                        IndicatorQT.QTUnit = "benchmark high score";
                    }
                    else if (location.AlternativeType.Count() == 2)
                    {
                        //actuals have 2 chars
                        i++;
                        IndicatorQT.QTM += location.QTM;
                        IndicatorQT.QTL += location.QTL;
                        IndicatorQT.QTU += location.QTU;
                        IndicatorQT.QTD1 += location.Q3 / i;
                        IndicatorQT.QTD2 += location.Q4 / i;
                        IndicatorQT.QTMUnit = "actual most score";
                        IndicatorQT.QTLUnit = "actual low score";
                        IndicatorQT.QTUUnit = "actual high score";
                        IndicatorQT.QTD1Unit = "actual certainty1";
                        IndicatorQT.QTD2Unit = "actual certainty2";
                    }
                    else if (location.AlternativeType.Count() == 1)
                    {
                        //targets have 1
                        IndicatorQT.Q1 += location.QTM;
                        IndicatorQT.Q2 += location.QTL;
                        IndicatorQT.Q3 += location.QTU;
                        IndicatorQT.Q1Unit = "target most score";
                        IndicatorQT.Q2Unit = "target low score";
                        IndicatorQT.Q3Unit = "target high score";
                    }
                }
            }
        }
        
        private PRA1 CalculateSubIndicators(PRA1 pra1)
        {
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                //qtm = avg of trends
                List<double> trends = new List<double>();
                for (int i = 0; i < pra1.IndicatorQT.Indicators.Count(); i++)
                {
                    trends.Add(CalculatorHelpers
                        .ConvertStringToDouble(pra1.IndicatorQT.Indicators[i]));
                }
                var stats = new MathNet.Numerics.Statistics.DescriptiveStatistics(trends);
                pra1.IndicatorQT.QTM = stats.Mean;
                pra1.IndicatorQT.QTMUnit = "mean score";
                pra1.IndicatorQT.QTL = stats.Minimum;
                pra1.IndicatorQT.QTLUnit = "lowest score";
                pra1.IndicatorQT.QTU = stats.Maximum;
                pra1.IndicatorQT.QTUUnit = "highest score";
                //2.0.8 only supports normal and triangle distribution
                if (pra1.IndicatorQT.QDistributionType == Calculator1.RUC_TYPES.normal.ToString())
                {
                    pra1.IndicatorQT.QT = stats.Mean;
                    pra1.IndicatorQT.QTD1 = stats.Mean;
                    pra1.IndicatorQT.QTD2 = stats.StandardDeviation;
                    pra1.RunAlgorithmAsync();
                }
                else if (pra1.IndicatorQT.QDistributionType == Calculator1.RUC_TYPES.triangle.ToString())
                {
                    pra1.IndicatorQT.QT = stats.Mean;
                    pra1.IndicatorQT.QTD1 = pra1.IndicatorQT.QTL;
                    pra1.IndicatorQT.QTD2 = pra1.IndicatorQT.QTU;
                    pra1.RunAlgorithmAsync();
                }
            }
            else
            {
                pra1.RunAlgorithmAsync();
                //196: Ind2 set QTM, QTL, and QTU (asset value = p * Q)
                pra1.IndicatorQT.QTM = pra1.IndicatorQT.QTM * pra1.IndicatorQT.Q2;
                pra1.IndicatorQT.QTL = pra1.IndicatorQT.QTL * pra1.IndicatorQT.Q2;
                pra1.IndicatorQT.QTU = pra1.IndicatorQT.QTU * pra1.IndicatorQT.Q2;
            }
            PRA1 pra = new PRA1(pra1);
            return pra;
        }
        
        private async Task SetCategoryAndIndicatorDataResult(
            int locationIndex, Dictionary<PRA1, List<PRA1>> locationIndexes,
            List<string> dataR, int r, IndicatorQT1 locationIndicator)
        {
            List<double> qtMs = new List<double>();
            List<double> qtLs = new List<double>();
            List<double> qtUs = new List<double>();
            //208
            //weights
            List<double> qtQ1s = new List<double>();
            //trends
            List<List<double>> trends = new List<List<double>>();
            string sNormType = string.Empty;
            //get normalization vector for qts
            foreach (var catpra in locationIndexes)
            {
                foreach (var subpra in catpra.Value)
                {
                    //first legit norm type sets value for whole vector
                    if (string.IsNullOrEmpty(sNormType)
                        || sNormType == Constants.NONE)
                    {
                        sNormType = subpra.IndicatorQT.Q1Unit;
                    }
                    qtMs.Add(subpra.IndicatorQT.QTM);
                    //add all of the vectors to 1 vector so that normaliz shows more differences in cis
                    qtLs.Add(subpra.IndicatorQT.QTL);
                    qtUs.Add(subpra.IndicatorQT.QTU);
                    //weights for normalizs
                    qtQ1s.Add(subpra.IndicatorQT.Q1);
                    if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
                    {
                        //trends
                        Shared.AddStringArrayToDoubleArray(subpra.IndicatorQT.Indicators, trends);
                    }
                }
            }
            //add all of the vectors to 1 vector so that normalized vector
            //shows more differences in cis
            List<double> qts = new List<double>();
            qts.AddRange(qtMs);
            qts.AddRange(qtLs);
            qts.AddRange(qtUs);
            //init vectors
            Vector<double> nQTs
                = Vector<double>.Build.Dense(qts.ToArray());
            Vector<double> nQTMs
                = Vector<double>.Build.Dense(qtMs.ToArray());
            Vector<double> nQTLs
                = Vector<double>.Build.Dense(qtLs.ToArray());
            Vector<double> nQTUs
                = Vector<double>.Build.Dense(qtUs.ToArray());
            if (!string.IsNullOrEmpty(sNormType) 
                && sNormType != Constants.NONE)
            {
                //no pnorm allowed yet
                double start = 0;
                if (sNormType == CalculatorHelpers.NORMALIZATION_TYPES.weights.ToString())
                {
                    start = qtQ1s.Sum();
                }
                //normalize the vectors but can't scale using Moncho equation data
                bool bScale = false;
                nQTs = Shared.GetNormalizedVector(sNormType, start, bScale,
                    qts.ToArray());
                int iCount = nQTs.Count / 3;
                nQTMs = nQTs.SubVector(0, iCount);
                nQTLs = nQTs.SubVector(iCount, iCount);
                nQTUs = nQTs.SubVector((iCount + iCount), iCount);
                if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
                {
                    trends = Shared.GetNormalizedandWeightedLists(sNormType, start, bScale,
                        qtQ1s, trends);
                }
            }
            int i = 0;
            int rStart = 0;
            //init categories to zero -they are summations of subsinds only
            //and can't have independent values
            foreach (var catpra in locationIndexes)
            {
                catpra.Key.IndicatorQT.QTM = 0;
                catpra.Key.IndicatorQT.QTL = 0;
                catpra.Key.IndicatorQT.QTU = 0;
                catpra.Key.IndicatorQT.Q3 = 0;
                catpra.Key.IndicatorQT.Q4 = 0;
                catpra.Key.IndicatorQT.Indicators = new string[] { };
            }
            List<PRA1> catcategories = new List<PRA1>();
            foreach (var catpra in locationIndexes)
            {
                //rf, fs, sr categories 
                if (catpra.Value.Count == 0)
                {
                    foreach (var cat in catcategories)
                    {
                        //locational index (RF)
                        catpra.Key.IndicatorQT.QTM
                                += cat.IndicatorQT.QTM * catpra.Key.IndicatorQT.Q1;
                        catpra.Key.IndicatorQT.QTL
                                += cat.IndicatorQT.QTL * catpra.Key.IndicatorQT.Q1;
                        catpra.Key.IndicatorQT.QTU
                                += cat.IndicatorQT.QTU * catpra.Key.IndicatorQT.Q1;
                        //certainty1
                        catpra.Key.IndicatorQT.Q3
                                += (cat.IndicatorQT.Q3 / catcategories.Count);
                        //certainty2
                        catpra.Key.IndicatorQT.Q4
                                += (cat.IndicatorQT.Q4 / catcategories.Count);
                        //trends
                        catpra.Key.IndicatorQT.Indicators
                                = Shared.AddStringArrayToStringArray(
                                    catpra.Key.IndicatorQT.Indicators, trendPeriods, 
                                    cat.IndicatorQT.Indicators);
                    }
                    catcategories = new List<PRA1>();
                }
                else
                {
                    //weight the gross QTMs
                    foreach (var subpra in catpra.Value)
                    {
                        //indicator
                        subpra.IndicatorQT.QTM = nQTMs[i] * subpra.IndicatorQT.Q1;
                        //category index (RFA)
                        catpra.Key.IndicatorQT.QTM
                            += subpra.IndicatorQT.QTM * catpra.Key.IndicatorQT.Q1;
                        subpra.IndicatorQT.QTL = nQTLs[i] * subpra.IndicatorQT.Q1;
                        catpra.Key.IndicatorQT.QTL
                            += subpra.IndicatorQT.QTL * catpra.Key.IndicatorQT.Q1;
                        subpra.IndicatorQT.QTU = nQTUs[i] * subpra.IndicatorQT.Q1;
                        catpra.Key.IndicatorQT.QTU
                            += subpra.IndicatorQT.QTU * catpra.Key.IndicatorQT.Q1;
                        //certainty1
                        catpra.Key.IndicatorQT.Q3
                                += (subpra.IndicatorQT.Q3 / catpra.Value.Count);
                        //certainty2
                        catpra.Key.IndicatorQT.Q4
                                += (subpra.IndicatorQT.Q4 / catpra.Value.Count);
                        //trends
                        catpra.Key.IndicatorQT.Indicators
                                = Shared.AddStringArrayToStringArray(
                                    catpra.Key.IndicatorQT.Indicators, trends, i);
                        i++;
                        rStart++;
                    }
                }
                catpra.Key.IndicatorQT.QTMUnit = "mean";
                catpra.Key.IndicatorQT.QTLUnit = "low ci";
                catpra.Key.IndicatorQT.QTUUnit = "high ci";
                if (catpra.Value.Count > 0)
                {
                    catcategories.Add(catpra.Key);
                }
                rStart++;
            }
            //fill in the dataresults
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                await FillCatAndIndDataResult2(locationIndex, locationIndexes,
                   dataR, r, locationIndicator, rStart);
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                //includes trends which change dataR, so DataResults used to set cols
                await FillCatAndIndTrendDataResult(locationIndex, locationIndexes,
                     dataR, r, locationIndicator, rStart, trends);
            }
            else
            {
                await FillCatAndIndDataResult(locationIndex, locationIndexes,
                    dataR, r, locationIndicator, rStart);
            }
            ////198: all datasets must be 4 level
            ////rstart is a count, r is a zero-based index
            //i = r - (rStart - 1);
            //IndicatorQT1 ScoreIndicator = new IndicatorQT1();
            //if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
            //{
            //    ScoreIndicator.QTMUnit = "risk mngt index";
            //}
            //else if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
            //{
            //    ScoreIndicator.QTMUnit = "resiliency index";
            //}
            //else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            //{
            //    ScoreIndicator.QTMUnit = "performance score";
            //}
            //else
            //{
            //    ScoreIndicator.QTMUnit = "sum categories";
            //}
            //foreach (var catpra in locationIndexes)
            //{
            //    //rf, fs, sr categories 
            //    if (catpra.Value.Count == 0)
            //    {
            //        //locational indexes can be weighted
            //        //198: weighted averages carried out with TR, not with RF, SR ..
            //        ScoreIndicator.QTM = ScoreIndicator.QTM * catpra.Key.IndicatorQT.Q1;
            //        ScoreIndicator.QTL = ScoreIndicator.QTL * catpra.Key.IndicatorQT.Q1;
            //        ScoreIndicator.QTU = ScoreIndicator.QTU * catpra.Key.IndicatorQT.Q1;
            //        //misc
            //        ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
            //        ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
            //        ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
            //        ScoreIndicator.Q2 = catpra.Key.IndicatorQT.Q2;
            //        SetScoreDataResult(i, ScoreIndicator, locationIndicator);

            //        ScoreIndicator = new IndicatorQT1();
            //        if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
            //        {
            //            ScoreIndicator.QTMUnit = "risk mngt index";
            //        }
            //        else if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
            //        {
            //            ScoreIndicator.QTMUnit = "resiliency index";
            //        }
            //        else
            //        {
            //            ScoreIndicator.QTMUnit = "sum categories";
            //        }
            //    }
            //    else
            //    {
            //        for (int c = 1; c < dataR.Count; c++)
            //        {
            //            if (c == 1)
            //            {
            //                DataResults[i][1] = catpra.Key.IndicatorQT.QDistributionType;
            //                ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
            //                //critical for equations such as moncho
            //                ScoreIndicator.Label = catpra.Key.IndicatorQT.Label;
            //                ScoreIndicator.Label2 = locationIndex.ToString();
            //            }
            //            else if (c == 2)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
            //                ScoreIndicator.QTM += catpra.Key.IndicatorQT.QTM;
            //            }
            //            else if (c == 3)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.QTMUnit;
            //            }
            //            else if (c == 4)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
            //                ScoreIndicator.QTL += catpra.Key.IndicatorQT.QTL;
            //            }
            //            else if (c == 5)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.QTLUnit;
            //                ScoreIndicator.QTLUnit = catpra.Key.IndicatorQT.QTLUnit;
            //            }
            //            else if (c == 6)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
            //                ScoreIndicator.QTU += catpra.Key.IndicatorQT.QTU;
            //            }
            //            else if (c == 7)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.QTUUnit;
            //                ScoreIndicator.QTUUnit = catpra.Key.IndicatorQT.QTUUnit;
            //            }
            //            else if (c == 8)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.Q1Unit;
            //                ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
            //            }
            //            else if (c == 9)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.Q1.ToString("F4", CultureInfo.InvariantCulture);
            //                ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
            //            }
            //            else if (c == 10)
            //            {
            //                DataResults[i][c] = catpra.Key.IndicatorQT.Q2.ToString("F4", CultureInfo.InvariantCulture);
            //                ScoreIndicator.Q2 = catpra.Key.IndicatorQT.Q2;
            //            }
            //        }
            //    }
            //    i++;
            //    foreach (var subpra in catpra.Value)
            //    {
            //        //subindicators
            //        for (int c = 1; c < dataR.Count; c++)
            //        {
            //            if (c == 1)
            //            {
            //                DataResults[i][1] = subpra.IndicatorQT.QDistributionType;
            //            }
            //            else if (c == 2)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
            //            }
            //            else if (c == 3)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.QTMUnit;
            //            }
            //            else if (c == 4)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
            //            }
            //            else if (c == 5)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.QTLUnit;
            //            }
            //            else if (c == 6)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
            //            }
            //            else if (c == 7)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.QTUUnit;
            //            }
            //            else if (c == 8)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.Q1Unit;
            //            }
            //            else if (c == 9)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.Q1.ToString("F4", CultureInfo.InvariantCulture);
            //            }
            //            else if (c == 10)
            //            {
            //                DataResults[i][c] = subpra.IndicatorQT.Q2.ToString("F4", CultureInfo.InvariantCulture);
            //            }
            //        }
            //        i++;
            //    }
            //}
        }
        private async Task FillCatAndIndDataResult(
            int locationIndex, Dictionary<PRA1, List<PRA1>> locationIndexes,
            List<string> dataR, int r, IndicatorQT1 locationIndicator,
            int rStart)
        {
            //fill in the dataresults
            //198: all datasets must be 4 level
            //rstart is a count, r is a zero-based index
            int i = r - (rStart - 1);
            IndicatorQT1 ScoreIndicator = new IndicatorQT1();
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
            {
                ScoreIndicator.QTMUnit = "risk mngt index";
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
            {
                ScoreIndicator.QTMUnit = "resiliency index";
            }
            else
            {
                ScoreIndicator.QTMUnit = "sum categories";
            }
            foreach (var catpra in locationIndexes)
            {
                //rf, fs, sr categories 
                if (catpra.Value.Count == 0)
                {
                    //locational indexes can be weighted
                    //198: weighted averages carried out with TR, not with RF, SR ..
                    ScoreIndicator.QTM = ScoreIndicator.QTM * catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.QTL = ScoreIndicator.QTL * catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.QTU = ScoreIndicator.QTU * catpra.Key.IndicatorQT.Q1;
                    //misc
                    ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
                    ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
                    ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.Q2 = catpra.Key.IndicatorQT.Q2;
                    SetScoreDataResult(i, ScoreIndicator, locationIndicator);

                    ScoreIndicator = new IndicatorQT1();
                    if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                    {
                        ScoreIndicator.QTMUnit = "risk mngt index";
                    }
                    else if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    {
                        ScoreIndicator.QTMUnit = "resiliency index";
                    }
                    else
                    {
                        ScoreIndicator.QTMUnit = "sum categories";
                    }
                }
                else
                {
                    for (int c = 1; c < dataR.Count; c++)
                    {
                        if (c == 1)
                        {
                            DataResults[i][1] = catpra.Key.IndicatorQT.QDistributionType;
                            ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
                            //critical for equations such as moncho
                            ScoreIndicator.Label = catpra.Key.IndicatorQT.Label;
                            ScoreIndicator.Label2 = locationIndex.ToString();
                        }
                        else if (c == 2)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTM += catpra.Key.IndicatorQT.QTM;
                        }
                        else if (c == 3)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTMUnit;
                        }
                        else if (c == 4)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTL += catpra.Key.IndicatorQT.QTL;
                        }
                        else if (c == 5)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTLUnit;
                            ScoreIndicator.QTLUnit = catpra.Key.IndicatorQT.QTLUnit;
                        }
                        else if (c == 6)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTU += catpra.Key.IndicatorQT.QTU;
                        }
                        else if (c == 7)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTUUnit;
                            ScoreIndicator.QTUUnit = catpra.Key.IndicatorQT.QTUUnit;
                        }
                        else if (c == 8)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q1Unit;
                            ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
                        }
                        else if (c == 9)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q1.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
                        }
                        else if (c == 10)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q2.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q2 = catpra.Key.IndicatorQT.Q2;
                        }
                    }
                }
                i++;
                foreach (var subpra in catpra.Value)
                {
                    //subindicators
                    for (int c = 1; c < dataR.Count; c++)
                    {
                        if (c == 1)
                        {
                            DataResults[i][1] = subpra.IndicatorQT.QDistributionType;
                        }
                        else if (c == 2)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 3)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTMUnit;
                        }
                        else if (c == 4)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 5)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTLUnit;
                        }
                        else if (c == 6)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 7)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTUUnit;
                        }
                        else if (c == 8)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q1Unit;
                        }
                        else if (c == 9)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q1.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 10)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q2.ToString("F4", CultureInfo.InvariantCulture);
                        }
                    }
                    i++;
                }
            }
        }
        private async Task FillCatAndIndDataResult2(
            int locationIndex, Dictionary<PRA1, List<PRA1>> locationIndexes,
            List<string> dataR, int r, IndicatorQT1 locationIndicator,
            int rStart)
        {
            //fill in the dataresults
            //198: all datasets must be 4 level
            //rstart is a count, r is a zero-based index
            int i = r - (rStart - 1);
            IndicatorQT1 ScoreIndicator = new IndicatorQT1();
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                ScoreIndicator.QTMUnit = "performance score";
            }
            else
            {
                ScoreIndicator.QTMUnit = "sum categories";
            }
            foreach (var catpra in locationIndexes)
            {
                //rf, fs, sr categories 
                if (catpra.Value.Count == 0)
                {
                    //locational indexes can be weighted
                    //198: weighted averages carried out with TR, not with RF, SR ..
                    ScoreIndicator.QTM = ScoreIndicator.QTM * catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.QTL = ScoreIndicator.QTL * catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.QTU = ScoreIndicator.QTU * catpra.Key.IndicatorQT.Q1;
                    //misc
                    ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
                    ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
                    ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.Q2 = catpra.Key.IndicatorQT.Q2;
                    //certainty
                    ScoreIndicator.Q3 = catpra.Key.IndicatorQT.Q3;
                    ScoreIndicator.Q4 = catpra.Key.IndicatorQT.Q4;
                    SetScoreDataResult2(i, ScoreIndicator, locationIndicator);
                    ScoreIndicator = new IndicatorQT1();
                    if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
                    {
                        ScoreIndicator.QTMUnit = "performance score";
                    }
                    else
                    {
                        ScoreIndicator.QTMUnit = "sum categories";
                    }
                }
                else
                {
                    for (int c = 0; c < dataR.Count; c++)
                    {
                        if (c == 0)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QDistributionType;
                            ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
                            //critical for equations such as moncho
                            ScoreIndicator.Label = catpra.Key.IndicatorQT.Label;
                            ScoreIndicator.Label2 = locationIndex.ToString();
                        }
                        else if (c == 1)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTM += catpra.Key.IndicatorQT.QTM;
                        }
                        else if (c == 2)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTMUnit;
                        }
                        else if (c == 3)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTL += catpra.Key.IndicatorQT.QTL;
                        }
                        else if (c == 4)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTLUnit;
                            ScoreIndicator.QTLUnit = catpra.Key.IndicatorQT.QTLUnit;
                        }
                        else if (c == 5)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTU += catpra.Key.IndicatorQT.QTU;
                        }
                        else if (c == 6)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTUUnit;
                            ScoreIndicator.QTUUnit = catpra.Key.IndicatorQT.QTUUnit;
                        }
                        else if (c == 7)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q3.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q3 = catpra.Key.IndicatorQT.Q3;
                        }
                        else if (c == 8)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q4.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q4 = catpra.Key.IndicatorQT.Q4;
                        }
                        else if (c == 9)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q1Unit;
                            ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
                        }
                        else if (c == 10)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q1.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
                        }
                    }
                }
                i++;
                foreach (var subpra in catpra.Value)
                {
                    //subindicators
                    for (int c = 0; c < dataR.Count; c++)
                    {
                        if (c == 0)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QDistributionType;
                        }
                        else if (c == 1)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        if (c == 2)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTMUnit;
                        }
                        else if (c == 3)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 4)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTLUnit;
                        }
                        else if (c == 5)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 6)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTUUnit;
                        }
                        else if (c == 7)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q3.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 8)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q4.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 9)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q1Unit;
                        }
                        else if (c == 10)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q1.ToString("F4", CultureInfo.InvariantCulture);
                        }
                    }
                    i++;
                }
            }
        }
        private async Task FillCatAndIndTrendDataResult(
            int locationIndex, Dictionary<PRA1, List<PRA1>> locationIndexes,
            List<string> dataR, int r, IndicatorQT1 locationIndicator,
            int rStart, List<List<double>> trends)
        {
            int i = r - (rStart - 1);
            int trendIndex = 0;
            IndicatorQT1 ScoreIndicator = new IndicatorQT1();
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                ScoreIndicator.QTMUnit = "performance score";
            }
            else
            { 
                ScoreIndicator.QTMUnit = "sum categories";
            }
            foreach (var catpra in locationIndexes)
            {
                //rf, fs, sr categories 
                if (catpra.Value.Count == 0)
                {
                    //locational indexes can be weighted
                    //198: weighted averages carried out with TR, not with RF, SR ..
                    ScoreIndicator.QTM = ScoreIndicator.QTM * catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.QTL = ScoreIndicator.QTL * catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.QTU = ScoreIndicator.QTU * catpra.Key.IndicatorQT.Q1;
                    //misc
                    ScoreIndicator.QDistributionType = catpra.Key.IndicatorQT.QDistributionType;
                    ScoreIndicator.Q1Unit = catpra.Key.IndicatorQT.Q1Unit;
                    ScoreIndicator.Q1 = catpra.Key.IndicatorQT.Q1;
                    ScoreIndicator.Q2 = catpra.Key.IndicatorQT.Q2;
                    //trends
                    ScoreIndicator.Indicators = catpra.Key.IndicatorQT.Indicators;
                    //certainty
                    ScoreIndicator.Q3 = catpra.Key.IndicatorQT.Q3;
                    ScoreIndicator.Q4 = catpra.Key.IndicatorQT.Q4;
                    SetScoreTrendDataResult(i, ScoreIndicator, locationIndicator);
                    ScoreIndicator = new IndicatorQT1();
                    if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
                    {
                        ScoreIndicator.QTMUnit = "performance score";
                    }
                    else
                    {
                        ScoreIndicator.QTMUnit = "sum categories";
                    }
                }
                else
                {
                    for (int c = 0; c < DataResults[i].Count; c++)
                    {
                        if (c == 0)
                        {
                            DataResults[i][c] 
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                            //critical for equations such as moncho
                            ScoreIndicator.Label = catpra.Key.IndicatorQT.Label;
                            ScoreIndicator.Label2 = locationIndex.ToString();
                        }
                        else if (c == 1)
                        {
                            DataResults[i][c]
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                        }
                        else if (c == 2)
                        {
                            DataResults[i][c]
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                        }
                        else if (c == 3)
                        {
                            DataResults[i][c]
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                        }
                        else if (c == 4)
                        {
                            DataResults[i][c]
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                        }
                        else if (c == 5)
                        {
                            DataResults[i][c]
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                        }
                        else if (c == 6)
                        {
                            DataResults[i][c]
                                = Shared.GetDoubleValue(catpra.Key.IndicatorQT.Indicators, c)
                                    .ToString("F4", CultureInfo.InvariantCulture);
                            //trends
                            ScoreIndicator.Indicators
                                    = Shared.AddStringArrayToStringArray(
                                        catpra.Key.IndicatorQT.Indicators, trends, i);
                        }
                        else if (c == 7)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q3.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q3 += catpra.Key.IndicatorQT.Q3;
                        }
                        else if (c == 8)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.Q4.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.Q4 += catpra.Key.IndicatorQT.Q4;
                        }
                        else if (c == 9)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTM += catpra.Key.IndicatorQT.QTM;
                        }
                        else if (c == 10)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTMUnit;
                        }
                        else if (c == 11)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTL += catpra.Key.IndicatorQT.QTL;
                        }
                        else if (c == 12)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTLUnit;
                            ScoreIndicator.QTLUnit = catpra.Key.IndicatorQT.QTLUnit;
                        }
                        else if (c == 13)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
                            ScoreIndicator.QTU += catpra.Key.IndicatorQT.QTU;
                        }
                        else if (c == 14)
                        {
                            DataResults[i][c] = catpra.Key.IndicatorQT.QTUUnit;
                            ScoreIndicator.QTUUnit = catpra.Key.IndicatorQT.QTUUnit;
                        }
                    }
                }
                i++;
                foreach (var subpra in catpra.Value)
                {
                    //indicators
                    for (int c = 0; c < DataResults[i].Count; c++)
                    {
                        if (c == 0)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 1)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 2)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 3)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 4)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 5)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 6)
                        {
                            DataResults[i][c] = Shared.GetStringValue(trends, trendIndex, c);
                        }
                        else if (c == 7)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q3.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 8)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.Q4.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 9)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTM.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 10)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTMUnit;
                        }
                        else if (c == 11)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTL.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 12)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTLUnit;
                        }
                        else if (c == 13)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTU.ToString("F4", CultureInfo.InvariantCulture);
                        }
                        else if (c == 14)
                        {
                            DataResults[i][c] = subpra.IndicatorQT.QTUUnit;
                        }
                    }
                    trendIndex++;
                    i++;
                }
            }
        }
        private void SetTRDataResult(int trIndex, int colCount,
            IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                SetTRRCA1DataResult(trIndex, colCount, thirdIndicator, locationIndicator);
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                SetTRRCA2DataResult(trIndex, colCount, thirdIndicator, locationIndicator);
            }
            else
            {
                IndicatorQT1 tr = new IndicatorQT1();
                foreach (var rf in thirdIndicator.IndicatorQT1s)
                {
                    //fill in TR for this individual location
                    tr.QDistributionType = rf.QDistributionType;
                    //add it the locationsindicator for display of Qs
                    tr.QTM += rf.QTM;
                    tr.QTMUnit = rf.QTMUnit;
                    tr.QTL += rf.QTL;
                    tr.QTLUnit = rf.QTLUnit;
                    tr.QTU += rf.QTU;
                    tr.QTUUnit = rf.QTUUnit;
                    tr.Q2 += rf.Q2;
                    //add it the locationsindicator for display of Qs
                    locationIndicator.QTM += rf.QTM;
                    locationIndicator.QTMUnit = rf.QTMUnit;
                    locationIndicator.QTL += rf.QTL;
                    locationIndicator.QTLUnit = rf.QTLUnit;
                    locationIndicator.QTU += rf.QTU;
                    locationIndicator.QTUUnit = rf.QTUUnit;
                    locationIndicator.Q2 += rf.Q2;
                }
                for (int c = 1; c < colCount; c++)
                {
                    if (c == 1)
                    {
                        DataResults[trIndex][c] = locationIndicator.QDistributionType;
                    }
                    else if (c == 2)
                    {
                        DataResults[trIndex][c] = tr.QTM.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 3)
                    {
                        DataResults[trIndex][c] = tr.QTMUnit;
                    }
                    else if (c == 4)
                    {
                        DataResults[trIndex][c] = tr.QTL.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 5)
                    {
                        DataResults[trIndex][c] = tr.QTLUnit;
                    }
                    else if (c == 6)
                    {
                        DataResults[trIndex][c] = tr.QTU.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 7)
                    {
                        DataResults[trIndex][c] = tr.QTUUnit;
                    }
                    else if (c == 8)
                    {
                        //2 props stored in the locationindicator
                        DataResults[trIndex][c] = locationIndicator.Q1Unit;
                    }
                    else if (c == 9)
                    {
                        DataResults[trIndex][c] = locationIndicator.Q1.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 10)
                    {
                        DataResults[trIndex][c] = tr.Q2.ToString("F4", CultureInfo.InvariantCulture);
                    }
                }
            }
        }
        private void SetTRRCA1DataResult(int trIndex, int colCount,
            IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            IndicatorQT1 tr = new IndicatorQT1();
            foreach (var rf in thirdIndicator.IndicatorQT1s)
            {
                //fill in TR for this individual location
                tr.QDistributionType = rf.QDistributionType;
                //add it the locationsindicator for display of Qs
                tr.QTM += rf.QTM;
                tr.QTMUnit = rf.QTMUnit;
                tr.QTL += rf.QTL;
                tr.QTLUnit = rf.QTLUnit;
                tr.QTU += rf.QTU;
                tr.QTUUnit = rf.QTUUnit;
                tr.Q3 += (rf.Q3 / thirdIndicator.IndicatorQT1s.Count);
                tr.Q4 += (rf.Q4 / thirdIndicator.IndicatorQT1s.Count);
                //add it the locationsindicator for display of Qs
                locationIndicator.QTM += rf.QTM;
                locationIndicator.QTMUnit = rf.QTMUnit;
                locationIndicator.QTL += rf.QTL;
                locationIndicator.QTLUnit = rf.QTLUnit;
                locationIndicator.QTU += rf.QTU;
                locationIndicator.QTUUnit = rf.QTUUnit;
                locationIndicator.Q3 += (rf.Q3 / thirdIndicator.IndicatorQT1s.Count);
                locationIndicator.Q4 += (rf.Q4 / thirdIndicator.IndicatorQT1s.Count);
                locationIndicator.Indicators = Shared.AddStringArrayToStringArray(
                    locationIndicator.Indicators, trendPeriods, rf.Indicators);
            }
            for (int c = 0; c < colCount; c++)
            {
                if (c == 0)
                {
                    DataResults[trIndex][c] = locationIndicator.QDistributionType;
                }
                else if (c == 1)
                {
                    DataResults[trIndex][c] = tr.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 2)
                {
                    DataResults[trIndex][c] = tr.QTMUnit;
                }
                else if (c == 3)
                {
                    DataResults[trIndex][c] = tr.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 4)
                {
                    DataResults[trIndex][c] = tr.QTLUnit;
                }
                else if (c == 5)
                {
                    DataResults[trIndex][c] = tr.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 6)
                {
                    DataResults[trIndex][c] = tr.QTUUnit;
                }
                else if (c == 7)
                {
                    DataResults[trIndex][c] = tr.Q3.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 8)
                {
                    DataResults[trIndex][c] = tr.Q4.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 9)
                {
                    //2 props stored in the locationindicator
                    DataResults[trIndex][c] = locationIndicator.Q1Unit;
                }
                else if (c == 10)
                {
                    DataResults[trIndex][c] = locationIndicator.Q1.ToString("F4", CultureInfo.InvariantCulture);
                }
            }
        }
        private void SetTRRCA2DataResult(int trIndex, int colCount,
            IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            IndicatorQT1 tr = new IndicatorQT1();
            foreach (var rf in thirdIndicator.IndicatorQT1s)
            {
                //fill in TR for this individual location
                tr.QDistributionType = rf.QDistributionType;
                //add it the locationsindicator for display of Qs
                tr.QTM += rf.QTM;
                tr.QTMUnit = rf.QTMUnit;
                tr.QTL += rf.QTL;
                tr.QTLUnit = rf.QTLUnit;
                tr.QTU += rf.QTU;
                tr.QTUUnit = rf.QTUUnit;
                tr.Q3 += (rf.Q3 / thirdIndicator.IndicatorQT1s.Count);
                tr.Q4 += (rf.Q4 / thirdIndicator.IndicatorQT1s.Count);
                tr.Indicators = Shared.AddStringArrayToStringArray(
                    tr.Indicators, trendPeriods, rf.Indicators);
                //add it the locationsindicator for display of Qs
                locationIndicator.QTM += rf.QTM;
                locationIndicator.QTMUnit = rf.QTMUnit;
                locationIndicator.QTL += rf.QTL;
                locationIndicator.QTLUnit = rf.QTLUnit;
                locationIndicator.QTU += rf.QTU;
                locationIndicator.QTUUnit = rf.QTUUnit;
                locationIndicator.Q3 += (rf.Q3 / thirdIndicator.IndicatorQT1s.Count);
                locationIndicator.Q4 += (rf.Q4 / thirdIndicator.IndicatorQT1s.Count);
                locationIndicator.Indicators = Shared.AddStringArrayToStringArray(
                    locationIndicator.Indicators, trendPeriods, rf.Indicators);
            }
            for (int c = 0; c < colCount; c++)
            {
                if (c == 0)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 1)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 2)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 3)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 4)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 5)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 6)
                {
                    DataResults[trIndex][c] = Shared.GetDoubleValue(tr.Indicators, c)
                        .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 7)
                {
                    DataResults[trIndex][c] = tr.Q3.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 8)
                {
                    DataResults[trIndex][c] = tr.Q4.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 9)
                {
                    DataResults[trIndex][c] = tr.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 10)
                {
                    DataResults[trIndex][c] = tr.QTMUnit;
                }
                else if (c == 11)
                {
                    DataResults[trIndex][c] = tr.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 12)
                {
                    DataResults[trIndex][c] = tr.QTLUnit;
                }
                else if (c == 13)
                {
                    DataResults[trIndex][c] = tr.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 14)
                {
                    DataResults[trIndex][c] = tr.QTUUnit;
                }
            }
        }
        private void SetScoreDataResult(int scoreIndex, 
            IndicatorQT1 scoreIndicator, IndicatorQT1 locationIndicator)
        {
            if (locationIndicator.IndicatorQT1s == null)
            {
                locationIndicator.IndicatorQT1s = new List<IndicatorQT1>();
            }
            locationIndicator.IndicatorQT1s.Add(scoreIndicator);
            //this will put the results in the exact same matrix position as the distribution
            for (int c = 1; c <= 10; c++)
            {
                if (c == 1)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QDistributionType;
                }
                else if (c == 2)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 3)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTMUnit;
                }
                else if (c == 4)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 5)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTLUnit;
                }
                else if (c == 6)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 7)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTUUnit;
                }
                else if (c == 8)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q1Unit;
                }
                else if (c == 9)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q1.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 10)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q2.ToString("F4", CultureInfo.InvariantCulture);
                }
            }
        }
        private void SetScoreDataResult2(int scoreIndex,
            IndicatorQT1 scoreIndicator, IndicatorQT1 locationIndicator)
        {
            if (locationIndicator.IndicatorQT1s == null)
            {
                locationIndicator.IndicatorQT1s = new List<IndicatorQT1>();
            }
            locationIndicator.IndicatorQT1s.Add(scoreIndicator);
            //this will put the results in the exact same matrix position as the distribution
            for (int c = 0; c <= 10; c++)
            {
                if (c == 0)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QDistributionType;
                }
                else if (c == 1)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 2)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTMUnit;
                }
                else if (c == 3)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 4)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTLUnit;
                }
                else if (c == 5)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 6)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTUUnit;
                }
                else if (c == 7)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q3.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 8)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q4.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 9)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q1Unit;
                }
                else if (c == 10)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q1.ToString("F4", CultureInfo.InvariantCulture);
                }
            }
        }
        private void SetScoreTrendDataResult(int scoreIndex,
            IndicatorQT1 scoreIndicator, IndicatorQT1 locationIndicator)
        {
            if (locationIndicator.IndicatorQT1s == null)
            {
                locationIndicator.IndicatorQT1s = new List<IndicatorQT1>();
            }
            locationIndicator.IndicatorQT1s.Add(scoreIndicator);
            for (int c = 0; c < DataResults[scoreIndex].Count; c++)
            {
                if (c == 0)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 1)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 2)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 3)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 4)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 5)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 6)
                {
                    DataResults[scoreIndex][c]
                        = Shared.GetDoubleValue(scoreIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 7)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q3.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 8)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.Q4.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 9)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 10)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTMUnit;
                }
                else if (c == 11)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 12)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTLUnit;
                }
                else if (c == 13)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 14)
                {
                    DataResults[scoreIndex][c] = scoreIndicator.QTUUnit;
                }
            }
        }
        private void SetLocationDataResult(int r, int locationIndex,
            IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                SetLocationRCA1DataResult(r, locationIndex, thirdIndicator, locationIndicator);
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                SetLocationRCA2DataResult(r, locationIndex, thirdIndicator, locationIndicator);
            }
            else
            {
                SetLocationResult(locationIndex, thirdIndicator, locationIndicator);
                //add a total risk row
                //this will put the results in the exact same matrix position as the distribution
                for (int c = 1; c <= 10; c++)
                {
                    if (c == 1)
                    {
                        DataResults[r][c] = locationIndicator.QDistributionType;
                    }
                    else if (c == 2)
                    {
                        DataResults[r][c] = locationIndicator.QTM.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 3)
                    {
                        DataResults[r][c] = locationIndicator.QTMUnit;
                    }
                    else if (c == 4)
                    {
                        DataResults[r][c] = locationIndicator.QTL.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 5)
                    {
                        DataResults[r][c] = locationIndicator.QTLUnit;
                    }
                    else if (c == 6)
                    {
                        DataResults[r][c] = locationIndicator.QTU.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 7)
                    {
                        DataResults[r][c] = locationIndicator.QTUUnit;
                    }
                    else if (c == 8)
                    {
                        DataResults[r][c] = locationIndicator.Q1Unit;
                    }
                    else if (c == 9)
                    {
                        DataResults[r][c] = locationIndicator.Q1.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    else if (c == 10)
                    {
                        DataResults[r][c] = locationIndicator.Q2.ToString("F4", CultureInfo.InvariantCulture);
                    }
                }
            }
        }
        private void SetLocationResult(int locationIndex,
            IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            int iScoreLocation = 0;
            if (thirdIndicator.IndicatorQT1s != null)
            {
                int iLocationsCount = 0;
                //Rf physical indicator summation
                IndicatorQT1 Rf = new IndicatorQT1();
                //F social indicator summation
                IndicatorQT1 F = new IndicatorQT1();
                foreach (var score in thirdIndicator.IndicatorQT1s)
                {
                    iScoreLocation = CalculatorHelpers.ConvertStringToInt(score.Label2);
                    iLocationsCount = thirdIndicator.IndicatorQT1s.Count();
                    if (locationIndex == iScoreLocation)
                    {
                        if (score.Label.ToUpper().StartsWith("RF"))
                        {
                            Rf.QTM += score.QTM;
                            Rf.QTL += score.QTL;
                            Rf.QTU += score.QTU;
                            if (!string.IsNullOrEmpty(score.QTMUnit)
                                && score.QTMUnit != Constants.NONE)
                            {
                                Rf.QTMUnit = score.QTMUnit;
                                Rf.QTLUnit = score.QTLUnit;
                                Rf.QTUUnit = score.QTUUnit;
                            }
                        }
                        else
                        {
                            F.QTM += score.QTM;
                            F.QTL += score.QTL;
                            F.QTU += score.QTU;
                            if (!string.IsNullOrEmpty(score.QTMUnit)
                                && score.QTMUnit != Constants.NONE)
                            {
                                F.QTMUnit = score.QTMUnit;
                                F.QTLUnit = score.QTLUnit;
                                F.QTUUnit = score.QTUUnit;
                            }
                        }
                    }
                }
                if (_subalgorithm == MATH_SUBTYPES.subalgorithm10.ToString())
                {
                    //moncho equation
                    //total risk
                    locationIndicator.QTM = Rf.QTM * (1 + F.QTM);
                    locationIndicator.QTL = Rf.QTL * (1 + F.QTL);
                    locationIndicator.QTU = Rf.QTU * (1 + F.QTU);
                }
                else if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                {
                    //subalgo 11 uses wtd averages
                    if (iLocationsCount == 0)
                        iLocationsCount = 1;
                    locationIndicator.QTM
                        = (Rf.QTM + F.QTM) / iLocationsCount;
                    locationIndicator.QTL = (Rf.QTL + F.QTL) / iLocationsCount;
                    locationIndicator.QTU = (Rf.QTU + F.QTU) / iLocationsCount;
                }
                else
                {
                    //no wtd averages, but SetTR is used by most subalgos
                    if (iLocationsCount == 0)
                        iLocationsCount = 1;
                    locationIndicator.QTM = (Rf.QTM + F.QTM);
                    locationIndicator.QTL = (Rf.QTL + F.QTL);
                    locationIndicator.QTU = (Rf.QTU + F.QTU);
                }
                if (!string.IsNullOrEmpty(Rf.QTMUnit)
                        && Rf.QTMUnit != Constants.NONE)
                {
                    locationIndicator.QTMUnit = Rf.QTMUnit;
                    locationIndicator.QTLUnit = Rf.QTLUnit;
                    locationIndicator.QTUUnit = Rf.QTUUnit;
                }
                else
                {
                    if (!string.IsNullOrEmpty(F.QTMUnit)
                        && F.QTMUnit != Constants.NONE)
                    {
                        locationIndicator.QTMUnit = F.QTMUnit;
                        locationIndicator.QTLUnit = F.QTLUnit;
                        locationIndicator.QTUUnit = F.QTUUnit;
                    }
                }
            }
        }
        private void SetLocationRCA1DataResult(int r, int locationIndex,
            IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            SetLocationResult(locationIndex, thirdIndicator, locationIndicator);
            //add a total risk row
            //this will put the results in the exact same matrix position as the distribution
            for (int c = 0; c <= 10; c++)
            {
                if (c == 0)
                {
                    DataResults[r][c] = locationIndicator.QDistributionType;
                }
                else if (c == 1)
                {
                    DataResults[r][c] = locationIndicator.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 2)
                {
                    DataResults[r][c] = locationIndicator.QTMUnit;
                }
                else if (c == 3)
                {
                    DataResults[r][c] = locationIndicator.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 4)
                {
                    DataResults[r][c] = locationIndicator.QTLUnit;
                }
                else if (c == 5)
                {
                    DataResults[r][c] = locationIndicator.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 6)
                {
                    DataResults[r][c] = locationIndicator.QTUUnit;
                }
                else if (c == 7)
                {
                    DataResults[r][c] = locationIndicator.Q3.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 8)
                {
                    DataResults[r][c] = locationIndicator.Q4.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 9)
                {
                    DataResults[r][c] = locationIndicator.Q1Unit;
                }
                else if (c == 10)
                {
                    DataResults[r][c] = locationIndicator.Q1.ToString("F4", CultureInfo.InvariantCulture);
                }
            }
        }
        private void SetLocationRCA2DataResult(int r, int locationIndex,
                IndicatorQT1 thirdIndicator, IndicatorQT1 locationIndicator)
        {
            SetLocationResult(locationIndex, thirdIndicator, locationIndicator);
            //add a total risk row
            for (int c = 0; c <= 14; c++)
            {
                if (c == 0)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 1)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 2)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 3)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 4)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 5)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 6)
                {
                    DataResults[r][c]
                        = Shared.GetDoubleValue(locationIndicator.Indicators, c)
                            .ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 7)
                {
                    DataResults[r][c] = locationIndicator.Q3.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 8)
                {
                    DataResults[r][c] = locationIndicator.Q4.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 9)
                {
                    DataResults[r][c] = locationIndicator.QTM.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 10)
                {
                    DataResults[r][c] = locationIndicator.QTMUnit;
                }
                else if (c == 11)
                {
                    DataResults[r][c] = locationIndicator.QTL.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 12)
                {
                    DataResults[r][c] = locationIndicator.QTLUnit;
                }
                else if (c == 13)
                {
                    DataResults[r][c] = locationIndicator.QTU.ToString("F4", CultureInfo.InvariantCulture);
                }
                else if (c == 14)
                {
                    DataResults[r][c] = locationIndicator.QTUUnit;
                }
            }
        }
        private async Task<bool> CalculateIndicators2(List<List<string>> data,
            List<List<string>> rowNames)
        {
            bool bHasCalculations = false;
            List<string> cols = new List<string>();
            //make a dataset formatted for the new rates and lifes
            for (int dr = 0; dr < (data.Count * DataSet2.Count()); dr++)
            {
                cols = new List<string>();
                //1 extra col for y
                //196: plus 1 extra col for isprojectcosts
                for (int ls = 0; ls < (DataSet2[0].Count() + 2); ls++)
                {
                    cols.Add(0.ToString());
                }
                DataResults.Add(cols);
            }
            //List<double[]> sampleCosts = new List<double[]>();
            List<double> distributionInstall = new List<double>();
            List<double> distributionOM = new List<double>();
            string sInstallDistType = string.Empty;
            string sOMDistType = string.Empty;
            string sIsProjectCost = "yes";
            IndicatorQT1 installInd = new IndicatorQT1();
            IndicatorQT1 omInd = new IndicatorQT1();
            IndicatorQT1 totalInd = new IndicatorQT1();
            //the avg annual costs for each life and span is calculated using
            //random samples of install and om costs for each project alternative
            int iLoops = data.Count / 3;
            //this is the starting row index (0, 2, 5)
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                //sampleCosts = new List<double[]>();
                //196 faster to calc costs after QTM than from 10000 samples
                installInd = new IndicatorQT1();
                omInd = new IndicatorQT1();
                totalInd = new IndicatorQT1();
                distributionInstall = new List<double>();
                distributionOM = new List<double>(); ;
                //iterate through columns, skipping y column
                for (int c = 1; c < data[i].Count; c++)
                {
                    if (c == 1)
                    {
                        //install distribution defined by 3 rows in each column
                        for (int r = i; r < (i + 3); r++)
                        {
                            if (data.Count > r)
                            {
                                //iterate through rows
                                distributionInstall.Add(CalculatorHelpers.ConvertStringToDouble(data[r][c]));
                            }
                            else
                            {
                                //extra unnecessary row in csv
                                IndicatorQT.ErrorMessage = string.Concat("Ind ", IndicatorIndex, " has a dataset has blank ending rows.");
                                break;
                            }
                        }
                    }
                    else if (c == 2)
                    {
                        //install cost dist type
                        sInstallDistType = data[i][c];
                    }
                    else if (c == 3)
                    {
                        //om distribution defined by 3 rows in each column
                        for (int r = i; r < (i + 3); r++)
                        {
                            //iterate through rows
                            distributionOM.Add(CalculatorHelpers.ConvertStringToDouble(data[r][c]));
                        }
                    }
                    else if (c == 4)
                    {
                        sOMDistType = data[i][c];
                    }
                    else if (c == 5)
                    {
                        //196: yes or 1 = project costs
                        //else unit costs whose Ind 6 and 7 aggregators must be multiplied by quantity
                        //this is is already included in the DataSet
                        //and no further action is needed -it's a reminder that is is there
                        sIsProjectCost = data[i][c];
                    }
                }
                //install
                if (distributionInstall.Count() == 3)
                {
                    PRA1 pra1 = new PRA1(this);
                    pra1.IndicatorQT.QT = distributionInstall[0];
                    pra1.IndicatorQT.QTD1 = distributionInstall[1];
                    pra1.IndicatorQT.QTD2 = distributionInstall[2];
                    pra1.IndicatorQT.QDistributionType = sInstallDistType;
                    distributionInstall = new List<double>();
                    await pra1.RunAlgorithmAsync();
                    //196: removed all asynch calcs because of inconsistent ordering of results 
                    //RunTasks.Add(pra1.RunAlgorithmAsync());
                    installInd.CopyIndicatorQT1Properties(installInd, pra1.IndicatorQT);
                }
                else
                {
                    //blank row message already set
                }
                //om 
                if (distributionOM.Count() == 3)
                {
                    PRA1 pra2 = new PRA1(this);
                    pra2.IndicatorQT.QT = distributionOM[0];
                    pra2.IndicatorQT.QTD1 = distributionOM[1];
                    pra2.IndicatorQT.QTD2 = distributionOM[2];
                    pra2.IndicatorQT.QDistributionType = sOMDistType;
                    distributionOM = new List<double>();
                    await pra2.RunAlgorithmAsync();
                    omInd.CopyIndicatorQT1Properties(omInd, pra2.IndicatorQT);
                }
                else
                {
                    //blank row error already set
                }
                double dbAvgAnnualCost = 0;
                double dbInstallCost = 0;
                double dbOMCost = 0;
                List<double> avgAnnCosts = new List<double>();
                //columns and rows of cis for each rate/life option
                List<List<IndicatorQT1>> cis = new List<List<IndicatorQT1>>();
                //loop through discount rates
                for (int dr = 0; dr < DataSet2[0].Count(); dr++)
                {
                    //rows
                    cis.Add(new List<IndicatorQT1>());
                    //use the rates to increase the number of rows for each project alternative
                    //use the lifespans to increase the number of columns for each project alternative
                    //loop through lifespans
                    for (int ls = 0; ls < DataSet2[1].Count(); ls++)
                    {
                        //separate costs for each rate and life
                        totalInd = new IndicatorQT1();
                        //196: need discounted pv costs and pv benefits in bcrs
                        //not amortized avg annual costs vs nondiscounted avg ann benefits
                        //install costs discounted 1 year only
                        dbInstallCost = Shared.GetDiscountedAmount(installInd.QTM,
                            1, DataSet2[0][dr]);
                        dbOMCost = Shared.GetUPV(omInd.QTM,
                             DataSet2[1][ls], DataSet2[0][dr]);
                        dbAvgAnnualCost = dbInstallCost + dbOMCost;
                        totalInd.QTM = dbAvgAnnualCost;
                        //qtls
                        dbInstallCost = Shared.GetDiscountedAmount(installInd.QTL,
                            1, DataSet2[0][dr]);
                        dbOMCost = Shared.GetUPV(omInd.QTL,
                             DataSet2[1][ls], DataSet2[0][dr]);
                        dbAvgAnnualCost = dbInstallCost + dbOMCost;
                        totalInd.QTL = dbAvgAnnualCost;
                        //qtus
                        dbInstallCost = Shared.GetDiscountedAmount(installInd.QTU,
                            1, DataSet2[0][dr]);
                        dbOMCost = Shared.GetUPV(omInd.QTU,
                             DataSet2[1][ls], DataSet2[0][dr]);
                        dbAvgAnnualCost = dbInstallCost + dbOMCost;
                        totalInd.QTU = dbAvgAnnualCost;
                        //this means cols are life spans
                        cis[dr].Add(totalInd);
                    }
                }
                //add the cis to new dataset
                int iDS2ColCount = 0;
                int iDRColCount = DataResults[0].Count;
                int iDRIndex = i * DataSet2[0].Count();
                int iRows = 0;
                for (int dr = 0; dr < DataSet2[0].Count(); dr++)
                {
                    iRows = 0;
                    iDS2ColCount = DataSet2[dr].Count();
                    //3 distribution rows for each dr
                    for (int r2 = iDRIndex; r2 < (iDRIndex + 3); r2++)
                    {
                        for (int ls = 0; ls < DataSet2[dr].Count(); ls++)
                        {
                            if (iRows == 0)
                            {
                                if (ls == 0)
                                {
                                    DataResults[r2][ls] = cis[dr][ls].QTM.ToString("F4", CultureInfo.InvariantCulture);
                                    IndicatorQT.QTM += cis[dr][ls].QTM;
                                }
                                DataResults[r2][ls + 1] = cis[dr][ls].QTM.ToString("F4", CultureInfo.InvariantCulture);
                                if ((ls == iDS2ColCount - 1)
                                    && (ls + 2 < iDRColCount))
                                {
                                    //overwrite last column
                                    DataResults[r2][ls + 2] = sIsProjectCost;
                                }
                            }
                            else if (iRows == 1)
                            {
                                if (ls == 0)
                                {
                                    DataResults[r2][ls] = cis[dr][ls].QTL.ToString("F4", CultureInfo.InvariantCulture);
                                    IndicatorQT.QTL += cis[dr][ls].QTL;
                                }
                                DataResults[r2][ls + 1] = cis[dr][ls].QTL.ToString("F4", CultureInfo.InvariantCulture);
                                if ((ls == iDS2ColCount - 1)
                                    && (ls + 2 < iDRColCount))
                                {
                                    DataResults[r2][ls + 2] = sIsProjectCost;
                                }
                            }
                            else if (iRows == 2)
                            {
                                if (ls == 0)
                                {
                                    DataResults[r2][ls] = cis[dr][ls].QTU.ToString("F4", CultureInfo.InvariantCulture);
                                    IndicatorQT.QTU += cis[dr][ls].QTU;
                                }
                                DataResults[r2][ls + 1] = cis[dr][ls].QTU.ToString("F4", CultureInfo.InvariantCulture);
                                if ((ls == iDS2ColCount - 1)
                                    && (ls + 2 < iDRColCount))
                                {
                                    DataResults[r2][ls + 2] = sIsProjectCost;
                                }
                            }
                        }
                        RowNameIndex.Add((i * (DataSet2[0].Count() - 1)) + iRows);
                        iRows++;
                    }
                    //3 more rows for next dr
                    iDRIndex = iDRIndex + 3;
                }
                //add 2 more rows
                i = i + 2;
            }
            //fill in parent indicator properties with the results
            int iDRRrobs = (data[0].Count - 1) * iLoops;
            //divide them by the probabilistic revents
            IndicatorQT.QT = IndicatorQT.QT / iDRRrobs;
            if (IndicatorQT.QTUnit == string.Empty || IndicatorQT.QTUnit == Constants.NONE)
                IndicatorQT.QTUnit = "mean avg annual costs";
            IndicatorQT.QTM = IndicatorQT.QTM / iDRRrobs;
            if (IndicatorQT.QTMUnit == string.Empty || IndicatorQT.QTMUnit == Constants.NONE)
                IndicatorQT.QTMUnit = "mean avg annual costs";
            IndicatorQT.QTL = IndicatorQT.QTL / iDRRrobs;
            IndicatorQT.QTU = IndicatorQT.QTU / iDRRrobs;
            //no real reason to fill in more other than none and 0
            bHasCalculations = true;
            return bHasCalculations;
        }
        
        private List<List<double>> GetRatesandLifes(List<string> lines2)
        {
            //returns a list of rates in first row and life spans in second row
            List<List<double>> ratesandLifes = new List<List<double>>();
            int i = 0;
            foreach (var line in lines2)
            {
                //skip the header row
                if (i != 0)
                {
                    if (i == 3)
                    {
                        //1.9.0 only allows 2 rows of data
                        break;
                    }
                    ratesandLifes.Add(new List<double>());
                    string[] arr = line.Split(Constants.CSV_DELIMITERS);
                    if (arr.Count() >= 4)
                    {
                        //rates start in col4
                        for (int j = 3; j < arr.Count(); j++)
                        {
                            if (!string.IsNullOrEmpty(arr[j]))
                            {
                                ratesandLifes[i - 1].Add(CalculatorHelpers.ConvertStringToDouble(arr[j]));
                            }
                            else
                            {
                                ratesandLifes[i - 1].Add(0);
                            }
                        }
                    }
                }
                i++;
            }
            return ratesandLifes;
        }

        private void SetMathResult(List<List<string>> rowNames)
        {
            //add the data to a string builder
            StringBuilder sb = new StringBuilder();
            if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
            {
                sb.AppendLine("rmi results");
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
            {
                sb.AppendLine("ri results");
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm9.ToString())
            {
                sb.AppendLine("drr results");
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString()
                || _subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                sb.AppendLine("rca results");
            }
            else
            {
                sb.AppendLine("dri results");
            }
            //arrange new csv dataset with better names
            if ((_subalgorithm == MATH_SUBTYPES.subalgorithm9.ToString()
                && IndicatorIndex == 5)
                || (_subalgorithm == MATH_SUBTYPES.subalgorithm10.ToString()
                && IndicatorIndex == 5)
                || (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()
                && IndicatorIndex == 2)
                || (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()
                && IndicatorIndex == 2))
            {
                ColNames[2] = "loc_confid";
                if (IndicatorIndex == 5
                    || (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()
                        && IndicatorIndex == 2)
                    || (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()
                        && IndicatorIndex == 2))
                {
                    for (int i = 0; i < DataSet2[1].Count; i++)
                    {
                        if (i == 0 || i == 1)
                        {
                            //life span sensitivity analysis
                            ColNames[i + 4] = string.Concat("life", Constants.FILENAME_DELIMITER, DataSet2[1][i].ToString());
                            if (i == 1)
                            {
                                //isprojectcost
                                if (ColNames.Count() > 8)
                                {
                                    ColNames[i + 5] = ColNames[8];
                                }
                            }
                        }
                    }
                }
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm13.ToString())
            {
                ColNames[4] = "QTM";
                ColNames[5] = "QTMUnit";
                ColNames[6] = "QTL";
                ColNames[7] = "QTLUnit";
                ColNames[8] = "QTU";
                ColNames[9] = "QTUUnit";
            }
            else if (_subalgorithm == MATH_SUBTYPES.subalgorithm14.ToString())
            {
                string[] newColNames = new string[18];
                for (int i = 0; i < ColNames.Count(); i++)
                {
                    newColNames[i] = ColNames[i];
                }
                //new cols changed by algo
                newColNames[12] = "QTM";
                newColNames[13] = "QTMUnit";
                //new cols
                newColNames[14] = "QTL";
                newColNames[15] = "QTLUnit";
                newColNames[16] = "QTU";
                newColNames[17] = "QTUUnit";
                ColNames = newColNames;
            }
            else
            {
                ColNames[5] = "QTM";
                ColNames[6] = "QTMUnit";
                ColNames[7] = "QTL";
                ColNames[8] = "QTLUnit";
                ColNames[9] = "QTU";
                ColNames[10] = "QTUUnit";
                //11, 12, and 13 don't change
                //ColNames[11] = "quantity";
            }
            sb.AppendLine(GetColumnNameRow());
            StringBuilder rb = new StringBuilder();
            int iRowCount = 0;
            int iColCount = 0;
            if (IndicatorIndex == 5
                || (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()
                && IndicatorIndex == 2)
                || (_subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()
                && IndicatorIndex == 2))
            {
                SetInd5MathResult(sb, rowNames);
            }
            else
            {
                foreach (var row in rowNames)
                {
                    iColCount = 0;
                    string sRowName = string.Empty;
                    foreach (var colc in row)
                    {
                        sRowName = colc;
                        rb.Append(string.Concat(sRowName, Constants.CSV_DELIMITER));
                    }
                    if (DataResults.Count() > iRowCount)
                    {
                        var resultrow = DataResults[iRowCount];
                        iColCount = 0;
                        foreach (var resultcolumn in resultrow)
                        {
                            if (iColCount == resultrow.Count - 1)
                            {
                                rb.Append(resultcolumn.ToString());
                            }
                            else
                            {
                                rb.Append(string.Concat(resultcolumn.ToString(), Constants.CSV_DELIMITER));
                            }
                            iColCount++;
                        }
                    }
                    sb.AppendLine(rb.ToString());
                    rb = new StringBuilder();
                    iRowCount++;
                }
            }
            if (IndicatorQT.MathResult.ToLower().StartsWith("http"))
            {
                string sError = string.Empty;
                bool bHasSaved = CalculatorHelpers.SaveTextInURI(
                    Params.ExtensionDocToCalcURI, sb.ToString(), IndicatorQT.MathResult, out sError);
                if (!string.IsNullOrEmpty(sError))
                {
                    IndicatorQT.MathResult += sError;
                }
            }
            else
            {
                IndicatorQT.MathResult = sb.ToString();
            }
        }
        private void SetInd5MathResult(StringBuilder sb, List<List<string>> rowNames)
        {
            StringBuilder rb = new StringBuilder();
            List<string> row = new List<string>();
            int iColCount = 0;
            int iCount = 0;
            int iRNIndex = 0;
            int iRateIndex = 0;
            for (int i = 0; i < DataResults.Count; i++)
            {
                if (RowNameIndex.Count > i)
                {
                    iRNIndex = RowNameIndex[i];
                    if (rowNames.Count > iRNIndex)
                    {
                        row = rowNames[iRNIndex];
                        string sRowName = string.Empty;
                        foreach (var colc in row)
                        {
                            sRowName = colc;
                            //assemble rows from the columns
                            //ind2 doesn't use these as row names so also safe with that indicator
                            //delimiter allows rate to be parsed in ind6
                            if (sRowName.ToUpper().Contains("QTD1"))
                            {
                                sRowName = sRowName.Replace("QTD1", string.Concat("QTL", Constants.FILENAME_DELIMITER, DataSet2[0][iRateIndex]));
                            }
                            else if (sRowName.ToUpper().Contains("QTD2"))
                            {
                                sRowName = sRowName.Replace("QTD2", string.Concat("QTU", Constants.FILENAME_DELIMITER, DataSet2[0][iRateIndex]));
                            }
                            else if (sRowName.ToUpper().Contains("QT"))
                            {
                                if (!sRowName.ToUpper().Contains("QTM")
                                    && !sRowName.ToUpper().Contains("QTL")
                                    && !sRowName.ToUpper().Contains("QTU"))
                                {
                                    sRowName = sRowName.Replace("QT", string.Concat("QTM", Constants.FILENAME_DELIMITER, DataSet2[0][iRateIndex]));
                                }
                            }
                            rb.Append(string.Concat(sRowName, Constants.CSV_DELIMITER));
                        }
                        var resultrow = DataResults[i];
                        iColCount = 0;
                        foreach (var resultcolumn in resultrow)
                        {
                            if (iColCount == resultrow.Count - 1)
                            {
                                rb.Append(resultcolumn.ToString());
                            }
                            else
                            {
                                rb.Append(string.Concat(resultcolumn.ToString(), Constants.CSV_DELIMITER));
                            }
                            iColCount++;
                        }
                        sb.AppendLine(rb.ToString());
                        rb = new StringBuilder();
                    }
                }
                iCount++;
                if (iCount == (DataSet2[0].Count * 3))
                {
                    iCount = 0;
                    iRateIndex = 0;
                }
                else if (iCount == 3)
                {
                    iRateIndex = 1;
                }
                else if (iCount == 6)
                {
                    iRateIndex = 2;
                }
                else if (iCount == 9)
                {
                    iRateIndex = 3;
                }
                else if (iCount == 12)
                {
                    iRateIndex = 4;
                }
                else if (iCount == 15)
                {
                    iRateIndex = 5;
                }
            }
        }
        
        private string GetColumnNameRow()
        {
            StringBuilder rb = new StringBuilder();
            int iColCount = ColNames.Count();
            //if (IndicatorIndex == 2
            //    || (IndicatorIndex == 1 
            //    && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()))
            //{
            //    iColCount = iColCount - 2;
            //}
            if (IndicatorIndex == 5)
            {
                //196 added an extra isprojectcost column
                iColCount = iColCount - 2;
            }
            int iCols = 0;
            foreach (var cn in ColNames)
            {
                if (iCols < iColCount)
                {
                   rb.Append(string.Concat(cn, Constants.CSV_DELIMITER));
                }
                iCols++;
            }
            //get rid of last csv
            rb = rb.Remove(rb.Length - 1, 1);
            //not in 1.9.0
            //if (IndicatorIndex == 2)
            //{
            //    //now add 11 columns to hold catindex cdf
            //    rb.Append(string.Concat("cdf0", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf10", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf20", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf30", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf40", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf50", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf60", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf70", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf80", Constants.CSV_DELIMITER));
            //    rb.Append(string.Concat("cdf90", Constants.CSV_DELIMITER));
            //    rb.Append("cdf100");
            //}
            return rb.ToString();
        }
        //198: deprecated but keep for reference; the WhenAll may still 
        //be required for large datasets
        public async Task<bool> CalculateIndicatorsAsync(List<List<string>> data,
            List<List<string>> rowNames)
        {
            bool bHasCalculations = false;
            //make a new list with same matrix, to be replaced with results
            //last 2 cols hold vars needed by specific algo but not in mathresult
            int iColCount = data[0].Count - 2;
            DataResults = CalculatorHelpers.GetList(data.Count, iColCount);
            string sCatIndexLabel = string.Empty;
            string sAlternative = string.Empty;
            int iLocationIndex = 0;
            int iCatIndex = 0;
            //if needed for means
            int iDataCountNoCategories = 0;
            //display of location total damages
            int iQLocation = 1;
            //total exposure will be displayed
            IndicatorQT.QTM = 0;
            IndicatorQT.QTL = 0;
            IndicatorQT.QTU = 0;
            //total risk
            IndicatorQT1 LocationIndicator = new IndicatorQT1();
            //start a cat index dictionary (for normalization and weighting)
            Dictionary<PRA1, List<PRA1>> locationIndexes
                = new Dictionary<PRA1, List<PRA1>>();
            PRA1 pra1 = new PRA1(this);
            PRA1 catPRA1 = new PRA1(this);
            List<PRA1> catIndexes = new List<PRA1>();
            //set up a list of tasks to run
            List<Task<PRA1>> runTasks = new List<Task<PRA1>>();
            //this is the row
            for (int r = 0; r < data.Count(); r++)
            {
                //labels are in the first column of row names
                if (rowNames.Count > r)
                {
                    //some algos also include _A alternative suffixes
                    sCatIndexLabel
                        = CalculatorHelpers.GetParsedString(0, Constants.FILENAME_DELIMITERS, rowNames[r][0]);
                    sAlternative
                        = CalculatorHelpers.GetParsedString(1, Constants.FILENAME_DELIMITERS, rowNames[r][0]);
                    iLocationIndex = CalculatorHelpers.ConvertStringToInt(rowNames[r][1]);
                    //each categorical index has 3 chars (RF1), subinds have 4 chars (RF1A)
                    //scores have 2 chars (RF)
                    if (sCatIndexLabel.Count() == 3)
                    {
                        if (runTasks.Count > 0)
                        {
                            //run them
                            PRA1[] prs = await Task.WhenAll(runTasks.ToList());
                            catIndexes = prs.ToList();
                            locationIndexes.Add(catPRA1, catIndexes);
                        }
                        //initialize
                        iCatIndex = r;
                        catIndexes = new List<PRA1>();
                        runTasks = new List<Task<PRA1>>();
                        catPRA1 = new PRA1(this);
                        //fill in pra1.Indicator1, but don't run independent dists yet
                        FillIndicatorDistribution(data, rowNames, r, catPRA1);
                    }
                    else if (sCatIndexLabel.Count() == 2)
                    {
                        if (sCatIndexLabel == "TR")
                        {
                            //use data to fill in some props of locationind
                            FillLocationIndicator(data, rowNames, r, sAlternative, iLocationIndex, LocationIndicator);
                            //subalgo10 should have count == 0; others will be positive
                            if (runTasks.Count > 0)
                            {
                                //run them
                                PRA1[] prs = await Task.WhenAll(runTasks.ToList());
                                catIndexes = prs.ToList();
                                locationIndexes.Add(catPRA1, catIndexes);
                                //subalgos 11 treats TR like a score (locationalindex)
                                catPRA1 = new PRA1(this);
                                //fill in pra1.Indicator1, but don't run independent dists yet
                                FillIndicatorDistribution(data, rowNames, r, catPRA1);
                                catIndexes = new List<PRA1>();
                                locationIndexes.Add(catPRA1, catIndexes);
                            }
                            //normalize and weight all of the data
                            //can now run the normalization and weight calculations
                            //and fill in the dataresults (starts at r - (catPRA1s.Count + catIndexes.Count)
                            await SetCategoryAndIndicatorDataResult(iLocationIndex,
                                locationIndexes, data[r], r, LocationIndicator);
                            if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                            {
                                //don't need the locationindex but need to display Qs
                                iQLocation++;
                                //don't init location indicator --need full collection of QT1s
                            }
                            else
                            {
                                iQLocation++;
                                //init location indicator
                                LocationIndicator = new IndicatorQT1();
                            }
                            //reinitialize Indexes for next location
                            locationIndexes = new Dictionary<PRA1, List<PRA1>>();
                            catIndexes = new List<PRA1>();
                            runTasks = new List<Task<PRA1>>();
                        }
                        else
                        {
                            //rf, fs, sr
                            if (runTasks.Count > 0)
                            {
                                //run them
                                PRA1[] prs = await Task.WhenAll(runTasks.ToList());
                                catIndexes = prs.ToList();
                                locationIndexes.Add(catPRA1, catIndexes);
                            }
                            catPRA1 = new PRA1(this);
                            //fill in pra1.Indicator1, but don't run independent dists yet
                            FillIndicatorDistribution(data, rowNames, r, catPRA1);
                            catIndexes = new List<PRA1>();
                            locationIndexes.Add(catPRA1, catIndexes);
                            //initialize
                            catIndexes = new List<PRA1>();
                            runTasks = new List<Task<PRA1>>();
                        }
                    }
                    else
                    {
                        iDataCountNoCategories++;
                        pra1 = new PRA1(this);
                        //fill in pra1.Indicator1
                        FillIndicatorDistribution(data, rowNames, r, pra1);
                        runTasks.Add(CalculateSubIndicatorsAsync(pra1));
                    }
                }
                else
                {
                    //skip to the next row
                }
            }

            //made it this far without an error, so good calcs
            bHasCalculations = true;
            return bHasCalculations;
        }
        private async Task<PRA1> CalculateSubIndicatorsAsync(PRA1 pra1)
        {
            await pra1.RunAlgorithmAsync();
            //196: Ind2 set QTM, QTL, and QTU (asset value = p * Q)
            pra1.IndicatorQT.QTM = pra1.IndicatorQT.QTM * pra1.IndicatorQT.Q2;
            pra1.IndicatorQT.QTL = pra1.IndicatorQT.QTL * pra1.IndicatorQT.Q2;
            pra1.IndicatorQT.QTU = pra1.IndicatorQT.QTU * pra1.IndicatorQT.Q2;
            return pra1;
        }
    }
}
