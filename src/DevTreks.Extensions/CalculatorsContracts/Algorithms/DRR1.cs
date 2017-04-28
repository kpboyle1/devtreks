using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		DRR1 algorithm
    ///Author:		www.devtreks.org
    ///Date:		2017, April
    ///References:	CTA algo1, CTAP subalgo9, 10, 11, 12
    ///</summary>
    public class DRR1 : PRA1
    {
        public DRR1()
            : base() {}
        
        /// <summary>
        /// Initialize the DRR1 algorithm
        /// </summary>
        /// <param name="mathTerms">Math Expression terms, in format Ix.Qx. Potential use in vector math.</param>
        /// <param name="colNames">All column names for data vectors. Identifies vectors to exclude and include.</param>
        /// <param name="depColNames">Column names adjusted for independent var data vectors. Includes intercept term, excludes dep var term.</param>
        /// <param name="totalsNeeded">Number of totals needed by calling procedure</param>
        ///is in the mathexpress (i.e. mathexpress.contains("q1")</param>
        public DRR1(int indicatorIndex, string label, string[] mathTerms, 
            string[] colNames, string[] depColNames, int totalsNeeded, 
            string subalgorithm, int ciLevel, int iterations,
            int random, List<double> qTs, IndicatorQT1 qT1, CalculatorParameters calcParams)
            : base(label, mathTerms, colNames, depColNames,
                totalsNeeded, subalgorithm, ciLevel, iterations, random,
                qTs, qT1, calcParams)
        {
            //ep don't use actual observations
            ObsTs = new double[] { };
            IndicatorIndex = indicatorIndex;
            Params = calcParams;
        }
        public CalculatorParameters Params { get; set; }
        //this algorithm runs sequential calculations that have different formats in datasets
        public int IndicatorIndex { get; set; }
        //make an index for the rownames
        public List<int> RowNameIndex = new List<int>();
        //store the results of calculations
        public List<List<string>> DataResults = new List<List<string>>();
        //ind 5 and 6 sensitivity analysis
        public List<List<double>> DataSet2 = new List<List<double>>();
        //used to fill in Qs for Ind6
        public double Highestorlowest = 0;
        public List<int> HighestBCRIndex = new List<int>();
        public List<IndicatorQT1> BCRs = new List<IndicatorQT1>();
        //this is asych for calling Task.WhenAll
        //but does not necessarily need internal asyc awaits
        public async Task RunAlgorithmAsync(List<List<string>> data, 
            List<List<string>> rowNames, List<string> lines2)
        {
            try
            {
                //the bool allows errors to be propagated
                bool bHasCalculations = false;
                //minimal data requirement is first five cols
                if (ColNames.Count() < 5)
                {
                    IndicatorQT.ErrorMessage = "DRR analysis requires at least 1 output variable and 1 input variable.";
                    return;
                }
                if (data.Count() < 3 && IndicatorIndex != 2)
                {
                    //185 same as other analysis
                    IndicatorQT.ErrorMessage = "DRR requires at least 3 rows of data distributions.";
                    return;
                }
                if (data.Count() != rowNames.Count)
                {
                    //185 same as other analysis
                    IndicatorQT.ErrorMessage = "The number of rows of numeric data don't match the number of string rows used to report the data. An Indicator.URL dataset is formatted incorrectly.";
                    return;
                }
                if (IndicatorIndex == 5)
                {
                    if (!(lines2.Count() > 0))
                    {
                        IndicatorQT.ErrorMessage = "This indicator relies on data stored in a second URL TEXT file. The file is missing. Please review the CTAP reference.";
                        return;
                    }
                }
                //no ind 4 because it can be calcd using data
                if (IndicatorIndex == 6 || IndicatorIndex == 7
                    || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                    || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    )
                {
                    if (!(lines2.Count() > 0))
                    {
                        IndicatorQT.ErrorMessage = "This indicator relies on data stored in the Math Results of a previous indicator. The needed MathResult is missing.";
                        return;
                    }
                }
                if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()
                    || _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                {
                    if ((IndicatorIndex == 3)
                        && lines2.Count > 0)
                    {
                        //get Ind2 costs and also set rates and life for row and col counts
                        DataSet2 = GetRatesandLifes(lines2);
                    }
                }
                else
                {
                    if ((IndicatorIndex == 6 || IndicatorIndex == 7)
                        && lines2.Count > 0)
                    {
                        //get Ind5 costs and also set rates and life for row and col counts
                        DataSet2 = GetRatesandLifes(lines2);
                    }
                }
                //1
                if ((IndicatorIndex == 1)
                    && lines2.Count == 0)
                {
                    bHasCalculations = await CalculateIndicators(data, rowNames, lines2);
                    
                }
                else if ((IndicatorIndex == 3 && lines2.Count == 0) 
                    || IndicatorIndex == 4)
                {
                    bHasCalculations = await CalculateIndicators2(data, rowNames, lines2);
                }
                else if (((IndicatorIndex == 6 || IndicatorIndex == 7))
                    || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                    || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    && lines2.Count > 0)
                {
                    if (IndicatorIndex == 6)
                    {
                        Highestorlowest = 0;
                    }
                    else if (IndicatorIndex == 7
                        || IndicatorIndex == 3)
                    {
                        //cer
                        Highestorlowest = 1000000000;
                    }
                    SetBCMathResult(rowNames, data, lines2);
                    return;
                }
                //put the results in MathResult
                SetMathResult(rowNames);
            }
            catch (Exception ex)
            {
                IndicatorQT.ErrorMessage = ex.Message;
            }
        }
        private double GetDataResultsRowSum(List<string> row)
        {
            double sum = 0;
            int i = 0;
            foreach(var col in row)
            {
                //skip totals and distribtype cols
                if (i > 1)
                {
                    sum += CalculatorHelpers.ConvertStringToDouble(col);
                }
                i++;
            }
            return sum;
        }
        private List<IndicatorQT1> MakeLines2List(List<string> lines2)
        {
            List<IndicatorQT1> qts = new List<IndicatorQT1>();
            int i = 0;
            foreach (var line in lines2)
            {
                if (i == 0)
                {
                    //skip the header row
                }
                else
                {

                    IndicatorQT1 qt = new IndicatorQT1();
                    string[] arr = line.Split(Constants.CSV_DELIMITERS);
                    //label
                    qt.Label = arr[0];
                    //location
                    if (!arr[1].Contains("yes"))
                    {
                        if (arr.Count() >= 11)
                        {
                            //get rid of any 1.0000 format issues with numbers
                            string[] arrLocDist = arr[1].Split(Constants.FILEEXTENSION_DELIMITERS);
                            qt.Label2 = arrLocDist[0];
                            //assettype
                            qt.Name = arr[2];
                            qt.QTM = CalculatorHelpers.ConvertStringToDouble(arr[5]);
                            qt.QTMUnit = arr[6];
                            qt.QTL = CalculatorHelpers.ConvertStringToDouble(arr[7]);
                            qt.QTLUnit = arr[8];
                            qt.QTU = CalculatorHelpers.ConvertStringToDouble(arr[9]);
                            qt.QTUUnit = arr[10];
                            //drr2 convention is to add quantity of asset to q2 (q1 held weight)
                            qt.Q2 = CalculatorHelpers.ConvertStringToDouble(arr[arr.Count() - 1]);
                        }
                    }
                    else
                    {
                        //trend dataset has been appended 
                        qt.Label2 = arr[1];
                        //assettype
                        qt.Name = arr[2];
                        //trends are listed by event; up to 7 events allowed
                        if (arr.Count() >= 6)
                        {
                            qt.Q1
                                = CalculatorHelpers.ConvertStringToDouble(arr[5]);
                        }
                        if (arr.Count() >= 7)
                        {
                            qt.Q2
                                = CalculatorHelpers.ConvertStringToDouble(arr[6]);
                        }
                        if (arr.Count() >= 8)
                        {
                            qt.Q3
                                = CalculatorHelpers.ConvertStringToDouble(arr[7]);
                        }
                        if (arr.Count() >= 9)
                        {
                            qt.Q4
                                = CalculatorHelpers.ConvertStringToDouble(arr[8]);
                        }
                        if (arr.Count() >= 10)
                        {
                            qt.Q5
                                = CalculatorHelpers.ConvertStringToDouble(arr[9]);
                        }
                        if (arr.Count() >= 11)
                        {
                            //no Q6s so fudge
                            qt.QTD1
                                = CalculatorHelpers.ConvertStringToDouble(arr[10]);
                        }
                        if (arr.Count() >= 12)
                        {
                            qt.QTD2
                                = CalculatorHelpers.ConvertStringToDouble(arr[11]);
                        }
                    }
                    //never include the label column or subsequent calcs are NaN
                    if (qt.Label != "label"
                        && qt.QTM.ToString() != "NaN")
                    {
                        qts.Add(qt);
                    }
                }
                i++;
            }
            return qts;
        }
        private async Task<bool> CalculateIndicators(List<List<string>> data, 
            List<List<string>> rowNames, List<string> lines2)
        {
            bool bHasCalculations = false;
            //make a new list with same matrix, to be replaced with results
            DataResults = new List<List<string>>(data);
            string sDistributionType = string.Empty;
            int iLoops = data.Count / 3;
            //this is the starting row index (0, 2, 5)
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                //all 3 indicators store distribution types in column index[1]
                sDistributionType = data[i][1];
                List<string> distribution = new List<string>();
                //iterate through columns, skipping y column
                for (int c = 2; c < data[i].Count; c++)
                {
                    //distribution defined by 3 rows in each column
                    for (int r = i; r < (i + 3); r++)
                    {
                        if (data.Count > r)
                        {
                            //iterate through rows
                            distribution.Add(data[r][c]);
                        }
                        else
                        {
                            //extra unnecessary row in csv
                            IndicatorQT.ErrorMessage = string.Concat("Ind ", IndicatorIndex, " has a dataset has blank ending rows.");
                            break;
                        }
                    }
                    if (distribution.Count() == 3)
                    {
                        //need separate objects when running concurrent tasks
                        //so qtd1s are not overwritten
                        PRA1 pra1 = new PRA1(this);
                        pra1.IndicatorQT.QDistributionType = sDistributionType;
                        pra1.IndicatorQT.QT = CalculatorHelpers.ConvertStringToDouble(distribution[0]);
                        pra1.IndicatorQT.QTD1 = CalculatorHelpers.ConvertStringToDouble(distribution[1]);
                        pra1.IndicatorQT.QTD2 = CalculatorHelpers.ConvertStringToDouble(distribution[2]);
                        distribution = new List<string>();
                        await pra1.RunAlgorithmAsync();
                        int rowCount = 0;
                        //start the cumulative totals
                        if (i == 0 && c == 2)
                        {
                            //init all math props back to zero
                            IndicatorQT1.InitIndicatorQT1MathProperties(IndicatorQT);
                        }
                        IndicatorQT.QT += pra1.IndicatorQT.QT;
                        IndicatorQT.QTD1 += pra1.IndicatorQT.QTD1;
                        IndicatorQT.QTD2 += pra1.IndicatorQT.QTD2;

                        //this will put the results in the exact same matrix position as the distribution
                        for (int r2 = i; r2 < (i + 3); r2++)
                        {
                            //iterate through rows
                            if (rowCount == 0)
                            {
                                DataResults[r2][c] = pra1.IndicatorQT.QTM.ToString();
                                IndicatorQT.QTM += pra1.IndicatorQT.QTM;
                            }
                            else if (rowCount == 1)
                            {
                                DataResults[r2][c] = pra1.IndicatorQT.QTL.ToString();
                                IndicatorQT.QTL += pra1.IndicatorQT.QTL;
                            }
                            else if (rowCount == 2)
                            {
                                DataResults[r2][c] = pra1.IndicatorQT.QTU.ToString();
                                IndicatorQT.QTU += pra1.IndicatorQT.QTU;
                            }
                            rowCount++;
                        }
                    }
                    if (c == 2)
                    {
                        IndicatorQT.Q1 += CalculatorHelpers.ConvertStringToDouble(DataResults[i][c]);
                    }
                    else if (c == 3)
                    {
                        IndicatorQT.Q2 += CalculatorHelpers.ConvertStringToDouble(DataResults[i][c]);
                    }
                    else if (c == 4)
                    {
                        IndicatorQT.Q3 += CalculatorHelpers.ConvertStringToDouble(DataResults[i][c]);
                    }
                    else if (c == 5)
                    {
                        IndicatorQT.Q4 += CalculatorHelpers.ConvertStringToDouble(DataResults[i][c]);
                    }
                    else if (c == 6)
                    {
                        IndicatorQT.Q5 += CalculatorHelpers.ConvertStringToDouble(DataResults[i][c]);
                    }
                }
                //fill in the data totals in the first column for info purposes
                for (int r = i; r < (i + 3); r++)
                {
                    double sum = GetDataResultsRowSum(DataResults[r]) / (data[r].Count - 1);
                    DataResults[r][0] = sum.ToString("F4", CultureInfo.InvariantCulture);
                }
                //the iteration loop only increases by 1
                //add 2 more rows
                i = i + 2;
            }
            //fill in parent indicator properties with the results
            int iDRRrobs = (data[0].Count - 1) * iLoops;
            //divide them by the probabilistic revents
            IndicatorQT.QT = IndicatorQT.QT / iDRRrobs;
            IndicatorQT.QTD1 = IndicatorQT.QTD1 / iDRRrobs;
            IndicatorQT.QTD2 = IndicatorQT.QTD2 / iDRRrobs;
            IndicatorQT.QTM = IndicatorQT.QTM / iDRRrobs;
            IndicatorQT.QTL = IndicatorQT.QTL / iDRRrobs;
            IndicatorQT.QTU = IndicatorQT.QTU / iDRRrobs;
            IndicatorQT.Q1 = IndicatorQT.Q1 / iLoops;
            IndicatorQT.Q2 = IndicatorQT.Q2 / iLoops;
            IndicatorQT.Q3 = IndicatorQT.Q3 / iLoops;
            IndicatorQT.Q4 = IndicatorQT.Q4 / iLoops;
            IndicatorQT.Q5 = IndicatorQT.Q5 / iLoops;
            if (ColNames.Count() >= 10)
            {
                IndicatorQT.Q1Unit = ColNames[5];
                IndicatorQT.Q2Unit = ColNames[6];
                IndicatorQT.Q3Unit = ColNames[7];
                IndicatorQT.Q4Unit = ColNames[8];
                IndicatorQT.Q5Unit = ColNames[9];
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        private async Task<bool> CalculateIndicators2(List<List<string>> data, 
            List<List<string>> rowNames, List<string> lines2)
        {
            bool bHasCalculations = false;
            //make a new list with same matrix, to be replaced with results
            int iColCount = data[0].Count;
            if (lines2.Count > 0
                && IndicatorIndex != 3)
            {
                //lines2 stores the exposure asset values
                //add 1 extra column to store the quantity of the assets (for total costs)
                iColCount = data[0].Count + 1;
                DataResults = CalculatorHelpers.GetList(data.Count, iColCount);
            }
            else
            {
                //make a new list with same matrix, to be replaced with results
                DataResults = new List<List<string>>(data);
            }
            double quantity = 0;
            //label, assettype, and location identify asset value in lines2
            string sAssetType = string.Empty;
            string sLocation = string.Empty;
            int iLocationIndex = 0;
            List<IndicatorQT1> line2QTs = MakeLines2List(lines2);
            string sDistributionType = string.Empty;
            string sCatIndexLabel = string.Empty;
            bool bIsBaseLineTRDamage = false;
            //init all math props back to zero
            IndicatorQT1.InitIndicatorQT1MathProperties(IndicatorQT);
            int iCatIndex = 0;
            //sum of categorical index qtms
            List<List<IndicatorQT1>> scoreIndexes = new List<List<IndicatorQT1>>();
            //total risk
            List<List<IndicatorQT1>> locationIndexes = new List<List<IndicatorQT1>>();
            //start a cat index list: rows are QTMs for each subindicator in the category
            List<List<IndicatorQT1>> catIndexes = new List<List<IndicatorQT1>>();
            //start a cdf list
            List<List<double>> cdfs = new List<List<double>>();
            //this is the starting row index (0, 2, 5)
            int i = 0;
            for (i = 0; i < data.Count; i++)
            {
                PRA1 pra1 = new PRA1(this);
                //labels are in the first column of row names
                if (rowNames.Count >= i)
                {
                    //get the label
                    sCatIndexLabel
                        = CalculatorHelpers.GetParsedString(0, Constants.FILENAME_DELIMITERS, rowNames[i][0]);
                    //get the asset type from the corresponding row of rownames (Ind3)
                    sAssetType = rowNames[i][1];
                    //column 3 is formatted: 1_QT
                    sLocation = rowNames[i][2];
                    if (!string.IsNullOrEmpty(sLocation))
                    {
                        sLocation
                            = CalculatorHelpers.GetParsedString(0, Constants.FILENAME_DELIMITERS, sLocation);
                        //get rid of any 1.0000 format issues with numbers
                        sLocation = CalculatorHelpers.GetParsedString(0, Constants.FILEEXTENSION_DELIMITERS, sLocation);
                        iLocationIndex = CalculatorHelpers.ConvertStringToInt(sLocation);
                    }
                    //196 quantity is not a summation of children -its in last column of assetvalues
                    if (lines2.Count == 0
                        && IndicatorIndex != 3)
                    {
                        //last column of the custom damage distribution holds the asset quantities
                        quantity = CalculatorHelpers.ConvertStringToDouble(data[i][data[i].Count - 1]);
                    }
                    else
                    {
                        //last column of the custom damage distribution holds the asset quantities
                        quantity = CalculatorHelpers.ConvertStringToDouble(data[i][data[i].Count - 1]);
                        if (lines2.Count > 0)
                        {
                            quantity = GetAssetValueRowQuantity(line2QTs,
                                sCatIndexLabel, sLocation, sAssetType);
                        }
                    }
                    if (quantity == 0)
                        quantity = 1;
                    //each categorical index has 3 chars (RF1), subinds have 4 chars (RF1A)
                    //scores have 2 chars (RF)
                    if (sCatIndexLabel.Count() == 3)
                    {
                        if (catIndexes.Count > 0)
                        {
                            await SetCategoryandScoreDataResult(sCatIndexLabel, iLocationIndex,
                                quantity, catIndexes, cdfs, iCatIndex, 
                                bIsBaseLineTRDamage, scoreIndexes);
                        }
                        //initialize
                        iCatIndex = i;
                        catIndexes = new List<List<IndicatorQT1>>();
                    }
                    else if (sCatIndexLabel.Count() == 2)
                    {
                        if (sCatIndexLabel == "TR")
                        {
                            //only use baseline TR damages to fill in IndicatorQT.Qs
                            bIsBaseLineTRDamage
                                = (rowNames[i][0].Count() > sCatIndexLabel.Count()) ? false : true;
                            await SetCategoryandScoreDataResult(sCatIndexLabel,
                                iLocationIndex, quantity, locationIndexes, cdfs, 
                                i, bIsBaseLineTRDamage);
                            //initialize
                            locationIndexes = new List<List<IndicatorQT1>>();
                            bIsBaseLineTRDamage = false;
                        }
                        else
                        {
                            if (catIndexes.Count > 0)
                            {
                                await SetCategoryandScoreDataResult(sCatIndexLabel, iLocationIndex,
                                    quantity, catIndexes, cdfs, iCatIndex, bIsBaseLineTRDamage, scoreIndexes);
                                //set the score 
                                await SetCategoryandScoreDataResult(sCatIndexLabel, iLocationIndex,
                                    quantity, scoreIndexes, cdfs, i, bIsBaseLineTRDamage, locationIndexes);
                            }
                            //initialize
                            catIndexes = new List<List<IndicatorQT1>>();
                            scoreIndexes = new List<List<IndicatorQT1>>();
                        }
                    }
                    else
                    {
                        catIndexes.Add(new List<IndicatorQT1>());
                        //all 3 indicators store distribution types in column index[1]
                        sDistributionType = data[i][1];
                        List<string> distribution = new List<string>();
                        //iterate through columns, skipping y column
                        for (int c = 2; c < data[i].Count; c++)
                        {
                            //distribution defined by 3 rows in each column
                            for (int r = i; r < (i + 3); r++)
                            {
                                if (data.Count > r)
                                {
                                    //iterate through rows
                                    //dist has 3 items for dist 
                                    distribution.Add(data[r][c]);
                                }
                                else
                                {
                                    //extra unnecessary row in csv
                                    IndicatorQT.ErrorMessage = string.Concat("Ind ", IndicatorIndex, " has a dataset has blank ending rows.");
                                    break;
                                }
                            }
                            if (distribution.Count() >= 3)
                            {
                                if (lines2.Count == 0)
                                {
                                    //need separate objects when running concurrent tasks
                                    //so qtd1s are not overwritten
                                    pra1 = new PRA1(this);
                                    pra1.IndicatorQT.QDistributionType = sDistributionType;
                                    pra1.IndicatorQT.QT = CalculatorHelpers.ConvertStringToDouble(distribution[0]);
                                    pra1.IndicatorQT.QTD1 = CalculatorHelpers.ConvertStringToDouble(distribution[1]);
                                    pra1.IndicatorQT.QTD2 = CalculatorHelpers.ConvertStringToDouble(distribution[2]);
                                    //set the quantity from the custom damage distribution
                                    pra1.IndicatorQT.Q2 = quantity;
                                    distribution = new List<string>();
                                    //196: no Task.WhenAll because of inconsistent sorting
                                    await pra1.RunAlgorithmAsync();
                                }
                                else
                                {
                                    pra1 = new PRA1(this);
                                    //fill out pra1.IndicatorQT with calculated damages
                                    //distribution has damage percents ci; line2QTs has assetvaluecis
                                    SetAssetDamage(pra1, sCatIndexLabel, sLocation,
                                        sAssetType, line2QTs, distribution, c);
                                    //second possible place quantity is stored
                                    //keep it in pra1 so that categories can aggregate them
                                    quantity = pra1.IndicatorQT.Q2;
                                    distribution = new List<string>();
                                }
                                int rowCount = 0;
                                //this will put the results in the exact same matrix position as the distribution
                                for (int r2 = i; r2 < (i + 3); r2++)
                                {
                                    //iterate through rows
                                    if (rowCount == 0)
                                    {
                                        DataResults[r2][c] = pra1.IndicatorQT.QTM.ToString("F2", CultureInfo.InvariantCulture);
                                    }
                                    else if (rowCount == 1)
                                    {
                                        DataResults[r2][c] = pra1.IndicatorQT.QTL.ToString("F2", CultureInfo.InvariantCulture);

                                    }
                                    else if (rowCount == 2)
                                    {
                                        DataResults[r2][c] = pra1.IndicatorQT.QTU.ToString("F2", CultureInfo.InvariantCulture);

                                    }
                                    rowCount++;
                                }
                                //add the subindicator to the categorical indexes list
                                //1 subindicator for each column
                                catIndexes[catIndexes.Count - 1].Add(pra1.IndicatorQT);
                            }
                        }
                        //this sets the average annual totals column
                        SetAverageAnnualDataResult(i, catIndexes[catIndexes.Count - 1], 
                            bIsBaseLineTRDamage);
                        //this sets the quantity column used in costs
                        SetQuantityDataResult(i, quantity);
                        //set the distribtype column
                        SetDistributionDataResult(i, 1, sDistributionType);
                    }
                }
                //the iteration loop only increases by 1
                //add 2 more rows
                i = i + 2;
            }
            if (ColNames.Count() >= 10)
            {
                IndicatorQT.Q1Unit = ColNames[5];
                IndicatorQT.Q2Unit = ColNames[6];
                IndicatorQT.Q3Unit = ColNames[7];
                IndicatorQT.Q4Unit = ColNames[8];
                IndicatorQT.Q5Unit = ColNames[9];
            }
            if (IndicatorIndex == 3)
            {
                IndicatorQT.QTMUnit = "total percent damage";
            }
            else
            {
                IndicatorQT.QTMUnit = "total avg ann damages";
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        private static double GetAssetValueRowQuantity(List<IndicatorQT1> line2QTs, 
            string label, string location, string assetType)
        {
            double quantity = 0;
            //196 quantity is listed in assetvalues
            //assettype removed because large datasets have hard-to-spot differences in that string
            IndicatorQT1 assetValueCI = line2QTs.FirstOrDefault(l => l.Label == label
                    && l.Label2 == location);
            if (assetValueCI != null)
            {
                quantity = assetValueCI.Q2;
            }
            return quantity;
        }
        private async Task SetCategoryandScoreDataResult(string catIndexLabel, int locationIndex,
            double quantity, List<List<IndicatorQT1>> catIndexes, List<List<double>> cdfs, int catIndex,
            bool isBaseLineTRDamage, List<List<IndicatorQT1>> scoreIndicators = null)
        {
            List<double> cdf = new List<double>();
            //physical indicators
            List<IndicatorQT1> rfIndicators = new List<IndicatorQT1>();
            //social indicators
            List<IndicatorQT1> fIndicators = new List<IndicatorQT1>();
            IndicatorQT1 catIndicator = new IndicatorQT1();
            int iCIIndex = 0;
            int iScoreLocation = 0;
            foreach (var cis in catIndexes)
            {
                //set the quantity from the first column 
                //quantity += cis[0].Q2;
                //aggregate the damages
                for (int c = 0; c < cis.Count; c++)
                {
                    iScoreLocation 
                        = CalculatorHelpers.ConvertStringToInt(cis[c].Label2);
                    if (scoreIndicators == null
                        && IndicatorIndex != 3)
                    {
                        if (locationIndex == iScoreLocation)
                        {
                            if (cis[c].Label.ToUpper().StartsWith("RF"))
                            {
                                if (rfIndicators.Count == 0)
                                {
                                    for (int i = 0; i < cis.Count; i++)
                                    {
                                        rfIndicators.Add(new IndicatorQT1());
                                    }
                                }
                                rfIndicators[c].QTM += cis[c].QTM;
                                rfIndicators[c].QTL += cis[c].QTL;
                                rfIndicators[c].QTU += cis[c].QTU;
                                rfIndicators[c].Label = catIndexLabel;
                                rfIndicators[c].Label2 = locationIndex.ToString();
                                if (c == 0)
                                {
                                    //196 changed from using summations to using the 
                                    //actual quantity listed on each row of asset value
                                    //rfIndicators[c].Q2 += cis[0].Q2;
                                    rfIndicators[c].Q2 = quantity;
                                }
                            }
                            else
                            {
                                if (fIndicators.Count == 0)
                                {
                                    for (int i = 0; i < cis.Count; i++)
                                    {
                                        fIndicators.Add(new IndicatorQT1());
                                    }
                                }
                                fIndicators[c].QTM += cis[c].QTM;
                                fIndicators[c].QTL += cis[c].QTL;
                                fIndicators[c].QTU += cis[c].QTU;
                                fIndicators[c].Label = catIndexLabel;
                                fIndicators[c].Label2 = locationIndex.ToString();
                                if (c == 0)
                                {
                                    fIndicators[c].Q2 = quantity;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rfIndicators.Count == 0)
                        {
                            for (int i = 0; i < cis.Count; i++)
                            {
                                rfIndicators.Add(new IndicatorQT1());
                            }
                        }
                        rfIndicators[c].QTM += cis[c].QTM;
                        rfIndicators[c].QTL += cis[c].QTL;
                        rfIndicators[c].QTU += cis[c].QTU;
                        rfIndicators[c].Label = catIndexLabel;
                        //determines number of total risk indicators to add at end
                        rfIndicators[c].Label2 = locationIndex.ToString();
                        if (c == 0)
                        {
                            rfIndicators[c].Q2 = quantity;
                        }
                    }
                }
                iCIIndex++;
            }
            //calculate total risk
            if (scoreIndicators == null
                && IndicatorIndex != 3)
            {
                for (int c = 0; c < rfIndicators.Count; c++)
                {
                    if (rfIndicators.Count > c && fIndicators.Count > c)
                    {
                        rfIndicators[c].QTM = rfIndicators[c].QTM * (1 + fIndicators[c].QTM);
                        rfIndicators[c].QTL = rfIndicators[c].QTL * (1 + fIndicators[c].QTL);
                        rfIndicators[c].QTU = rfIndicators[c].QTU * (1 + fIndicators[c].QTU);
                        if (c == 0)
                        {
                            rfIndicators[c].Q2 = rfIndicators[c].Q2 + fIndicators[c].Q2;
                        }
                    }
                }
            }
            //this will put the results in the exact same matrix position as the distribution
            for (int c = 0; c < rfIndicators.Count; c++)
            {
                //exceed prob start in 3rd colums
                if (DataResults.Count > catIndex)
                {
                    //qtm in top row of category
                    DataResults[catIndex][c + 2] = rfIndicators[c].QTM.ToString("F4", CultureInfo.InvariantCulture);
                    if (DataResults.Count > catIndex + 1)
                    {
                        //qtl in next row
                        DataResults[catIndex + 1][c + 2] = rfIndicators[c].QTL.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    if (DataResults.Count > catIndex + 2)
                    {
                        //qtu in next row
                        DataResults[catIndex + 2][c + 2] = rfIndicators[c].QTU.ToString("F4", CultureInfo.InvariantCulture);
                    }
                    //fill in exceedance event total damages
                    if (isBaseLineTRDamage)
                    {
                        if ((c + 2) == 2)
                        {
                            IndicatorQT.Q1 += rfIndicators[c].QTM;
                        }
                        else if ((c + 2) == 3)
                        {
                            IndicatorQT.Q2 += rfIndicators[c].QTM;
                        }
                        else if ((c + 2) == 4)
                        {
                            IndicatorQT.Q3 += rfIndicators[c].QTM;
                        }
                        else if ((c + 2) == 5)
                        {
                            IndicatorQT.Q4 += rfIndicators[c].QTM;
                        }
                        else if ((c + 2) == 6)
                        {
                            IndicatorQT.Q5 += rfIndicators[c].QTM;
                        }
                        else
                        {
                            //calculator only displays up to q5
                        }
                    }
                }
            }
            //this sets the average annual totals column
            SetAverageAnnualDataResult(catIndex, rfIndicators, 
                isBaseLineTRDamage);
            //this sets the quantity column used in costs
            if (rfIndicators.Capacity > 0)
            {
                quantity = rfIndicators[0].Q2;
            }
            SetQuantityDataResult(catIndex, quantity);
            SetDistributionDataResult(catIndex, 1, Constants.NONE);
            if (scoreIndicators != null)
            {
                //add the category summations to the score
                scoreIndicators.Add(rfIndicators);
            }
            ////keep for reference: add the cdf
            //for (int c = 0; c < cdf.Count; c++)
            //{
            //    if (DataResults[catIndex][7 + c + 1] != null)
            //    {
            //        //1 more for distrib type
            //        DataResults[catIndex][7 + c + 1] = cdf[c].ToString("F4", CultureInfo.InvariantCulture);
            //    }
            //}
        }
      
       private void SetAssetDamage(PRA1 pra1, 
            string catIndexLabel, string location, string assetType, 
            List<IndicatorQT1> lines2QTs, List<string> damagePercentCI, 
            int columnIndex)
        {
            IndicatorQT1 assetValueCI 
                = lines2QTs.FirstOrDefault(l => l.Label == catIndexLabel
                && l.Label2 == location && l.Name == assetType);
            if (assetValueCI != null)
            {
                pra1.IndicatorQT = new IndicatorQT1(assetValueCI);
                if (damagePercentCI.Count > 2)
                {
                    pra1.IndicatorQT.QTM = assetValueCI.QTM
                        * (CalculatorHelpers.ConvertStringToDouble(damagePercentCI[0]) / 100);
                    pra1.IndicatorQT.QTL = assetValueCI.QTL
                        * (CalculatorHelpers.ConvertStringToDouble(damagePercentCI[1]) / 100);
                    pra1.IndicatorQT.QTU = assetValueCI.QTU
                        * (CalculatorHelpers.ConvertStringToDouble(damagePercentCI[2]) / 100);
                    //need the quantity to calculate the costs in bcr and icer
                    pra1.IndicatorQT.Q2 = assetValueCI.Q2;
                    //convention for identifying the appended trend dataset
                    string sTrendLocation = string.Concat(location, "_yes");
                    IndicatorQT1 trend
                        = lines2QTs.FirstOrDefault(l => l.Label == catIndexLabel
                        && l.Label2 == sTrendLocation && l.Name == assetType);
                    if (trend != null)
                    {
                        //trends are simple multipliers
                        if (columnIndex == 2)
                        {
                            pra1.IndicatorQT.QTM 
                                = pra1.IndicatorQT.QTM * trend.Q1;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.Q1;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.Q1;
                        }
                        else if (columnIndex == 3)
                        {
                            pra1.IndicatorQT.QTM
                                = pra1.IndicatorQT.QTM * trend.Q2;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.Q2;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.Q2;
                        }
                        else if (columnIndex == 4)
                        {
                            pra1.IndicatorQT.QTM
                                = pra1.IndicatorQT.QTM * trend.Q3;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.Q3;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.Q3;
                        }
                        else if (columnIndex == 5)
                        {
                            pra1.IndicatorQT.QTM
                                = pra1.IndicatorQT.QTM * trend.Q4;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.Q4;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.Q4;
                        }
                        else if (columnIndex == 6)
                        {
                            pra1.IndicatorQT.QTM
                                = pra1.IndicatorQT.QTM * trend.Q5;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.Q5;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.Q5;
                        }
                        else if (columnIndex == 7)
                        {
                            pra1.IndicatorQT.QTM
                                = pra1.IndicatorQT.QTM * trend.QTD1;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.QTD1;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.QTD1;
                        }
                        else if (columnIndex == 8)
                        {
                            pra1.IndicatorQT.QTM
                                = pra1.IndicatorQT.QTM * trend.QTD2;
                            pra1.IndicatorQT.QTL
                                = pra1.IndicatorQT.QTL * trend.QTD2;
                            pra1.IndicatorQT.QTU
                                = pra1.IndicatorQT.QTU * trend.QTD2;
                        }
                    }
                    
                }
            }
            else
            {
                pra1.IndicatorQT = new IndicatorQT1();
            }
        }
        private void SetMathResult(List<List<string>> rowNames)
        {
            if (IndicatorIndex == 6 || IndicatorIndex == 7
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()))
            {
                return;
            }
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
            else
            {
                sb.AppendLine("dri results");
            }
            //arrange new csv dataset with better names
            if (IndicatorIndex != 2)
            {
                ColNames[2] = "loc_confid";
                if (IndicatorIndex == 5)
                {
                    for (int i = 0; i < DataSet2[1].Count; i++)
                    {
                        //life span sensitivity analysis
                        ColNames[i + 4] = string.Concat("life", Constants.FILENAME_DELIMITER, DataSet2[1][i].ToString());
                    }
                }
                else if (IndicatorIndex == 4)
                {
                    if (ColNames[ColNames.Count() - 1] != "quantity")
                    {
                        List<string> cols = ColNames.ToList();
                        cols.Add("quantity");
                        ColNames = cols.ToArray();
                    }
                }
            }
            else
            {
                ColNames[5] = "QTMost";
                ColNames[6] = "QTMostUnit";
                ColNames[7] = "QTLow";
                ColNames[8] = "QTLowUnit";
                ColNames[9] = "QTUp";
                ColNames[10] = "QTUpUnit";
            }
            sb.AppendLine(GetColumnNameRow());
            StringBuilder rb = new StringBuilder();
            int iRowCount = 0;
            int iColCount = 0;
            foreach (var row in rowNames)
            {
                iColCount = 0;
                string sRowName = string.Empty;
                foreach (var colc in row)
                {
                    sRowName = colc;
                    //assemble rows from the columns
                    //ind2 doesn't use these as row names so also safe with that indicator
                    if (sRowName.ToUpper().Contains("QTD1"))
                    {
                        sRowName = sRowName.Replace("QTD1", "QTL");
                    }
                    else if (sRowName.ToUpper().Contains("QTD2"))
                    {
                        sRowName = sRowName.Replace("QTD2", "QTU");
                    }
                    else if (sRowName.ToUpper().Contains("QT"))
                    {
                        if (!sRowName.ToUpper().Contains("QTM")
                            && !sRowName.ToUpper().Contains("QTL")
                            && !sRowName.ToUpper().Contains("QTU"))
                        {
                            sRowName = sRowName.Replace("QT", "QTM");
                        }
                    }
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
        
        private string GetColumnNameRow()
        {
            StringBuilder rb = new StringBuilder();
            int iColCount = ColNames.Count();
            if (IndicatorIndex == 2)
            {
                iColCount = iColCount - 2;
            }
            int iCols = 0;
            foreach (var cn in ColNames)
            {
                if (iCols <= iColCount)
                {
                    if (iCols == ColNames.Count() - 1)
                    {
                        rb.Append(cn);
                    }
                    else
                    {
                        rb.Append(string.Concat(cn, Constants.CSV_DELIMITER));
                    }
                }
                iCols++;
            }
            return rb.ToString();
        }
        private void SetDistributionDataResult(int row, int col,
            string distribution)
        {
            if (IndicatorIndex != 3)
            {
                //first indicator in the catinds column collection holds q2
                //add quantity to each row
                if (DataResults.Count > row)
                {
                    DataResults[row][col] = distribution;
                }
                if (DataResults.Count > row + 1)
                {
                    DataResults[row + 1][col] = distribution;
                }
                if (DataResults.Count > row + 2)
                {
                    DataResults[row + 2][col] = distribution;
                }
            }
        }
        private void SetQuantityDataResult(int catIndex,
            double quantity)
        {
            if (IndicatorIndex != 3)
            {
                int qColIndex = DataResults[0].Count - 1;
                //first indicator in the catinds column collection holds q2
                //add quantity to each row
                if (DataResults.Count > catIndex)
                {
                    DataResults[catIndex][qColIndex] = quantity.ToString("F4", CultureInfo.InvariantCulture);
                }
                if (DataResults.Count > catIndex + 1)
                {
                    DataResults[catIndex + 1][qColIndex] = quantity.ToString("F4", CultureInfo.InvariantCulture);
                }
                if (DataResults.Count > catIndex + 2)
                {
                    DataResults[catIndex + 2][qColIndex] = quantity.ToString("F4", CultureInfo.InvariantCulture);
                }
            }
        }
        private void SetAverageAnnualDataResult(int catIndex, 
            List<IndicatorQT1> catIndicators, bool isBaseLineTRDamage)
        {
            //196 allows any exceedance period integer and up to 7 events
            double qtm = 0;
            double qtl = 0;
            double qtu = 0;
            //the exact year is parsed from corresponding ColNames
            string sYear = string.Empty;
            //double calcs better than int calcs
            double dbEPPeriod = 0;
            double dbEPLossPercent = 0;
            for (int i = 0; i < catIndicators.Count; i++)
            {
                if (i == 7)
                {
                    //10 variable limit means that up to 7 events can be included per dataset
                    break;
                }
                //this algo always starts first ep period in 6th column
                if (ColNames.Count() > (i + 5))
                {
                    sYear = ColNames[i + 5].Replace("years", string.Empty);
                    sYear = ColNames[i + 5].Replace("year", string.Empty);
                    dbEPPeriod = CalculatorHelpers.ConvertStringToDouble(
                        sYear);
                    //allow division by zero so an Infinity result is displayed and user knows data is wrong
                    dbEPLossPercent = 1 / dbEPPeriod;
                    qtm += catIndicators[i].QTM * dbEPLossPercent;
                    qtl += catIndicators[i].QTL * dbEPLossPercent;
                    qtu += catIndicators[i].QTU * dbEPLossPercent;
                }
            }
            //qtm in top row of category
            DataResults[catIndex][0] = qtm.ToString("F4", CultureInfo.InvariantCulture);
            //qtl
            if (DataResults.Count > catIndex + 1)
            {
                DataResults[catIndex + 1][0] = qtl.ToString("F4", CultureInfo.InvariantCulture);
            }
            //qtu
            if (DataResults.Count > catIndex + 2)
            {
                DataResults[catIndex + 2][0] = qtu.ToString("F4", CultureInfo.InvariantCulture);
            }
            if (isBaseLineTRDamage)
                IndicatorQT.QTM += qtm;
            if (isBaseLineTRDamage)
                IndicatorQT.QTL += qtl;
            if (isBaseLineTRDamage)
                IndicatorQT.QTU += qtu;
        }
       
        private List<List<double>> GetRatesandLifes(List<string> lines2)
        {
            List<List<double>> ratesandLifes = new List<List<double>>();
            //first row are rates
            ratesandLifes.Add(new List<double>());
            //second row are lifes
            ratesandLifes.Add(new List<double>());
            int i = 0;
            string sName = string.Empty;
            string sUnit = string.Empty;
            double dbSensVar = 0;
            foreach (var line in lines2)
            {
                //DataSet2.Add(new List<double>());
                string[] arr = line.Split(Constants.CSV_DELIMITERS);
                if (arr.Count() >= 5)
                {
                    for (int j = 4; j < arr.Count(); j++)
                    {
                        sName = arr[j];
                        if (!string.IsNullOrEmpty(sName))
                        {
                            if (i == 0)
                            {
                                //lifes are in header row
                                string[] lifes = sName.Split(Constants.FILENAME_DELIMITERS);
                                if (lifes.Count() == 2)
                                {
                                    dbSensVar = CalculatorHelpers.ConvertStringToDouble(lifes[1]);
                                    if (!ratesandLifes[1].Contains(dbSensVar))
                                    {
                                        ratesandLifes[1].Add(dbSensVar);
                                    }
                                }
                            }
                            else
                            {
                                //rates are in each row's unit
                                //2_QT_0.05
                                sUnit = arr[2];
                                if (!string.IsNullOrEmpty(sUnit))
                                {
                                    //.05, .12
                                    string[] rates = sUnit.Split(Constants.FILENAME_DELIMITERS);
                                    if (rates.Count() == 3)
                                    {
                                        dbSensVar = CalculatorHelpers.ConvertStringToDouble(rates[2]);
                                        if (!ratesandLifes[0].Contains(dbSensVar))
                                        {
                                            ratesandLifes[0].Add(dbSensVar);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                i++;
            }
            return ratesandLifes;
        }
        
        private async void SetBCMathResult(List<List<string>> rowNames,
            List<List<string>> data, List<string> lines2)
        {
            //add the data to a string builder
            StringBuilder sb = new StringBuilder();
            StringBuilder rb = new StringBuilder();
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
            else
            {
                sb.AppendLine("dri results");
            }
            //the avg annual costs for each life and span is calculated using
            //random samples of install and om costs for each project alternative
            //int iLoops = data.Count / 3;
            //this is the starting row index (0, 1, 2)
            int i = 0;
            double dbRate = 0;
            double dbLife = 0;
            string sLabel = string.Empty;
            string sAltName = string.Empty;
            string sLocation = string.Empty;
            string[] arrLabels = new List<string>().ToArray();
            double dbBaseDamage = 0;
            double dbBaseCostsQ = 0;
            string sUnit = string.Empty;
            List<string> baseLineRow = new List<string>();
            List<List<string>> alternLineRows = new List<List<string>>();
            //report highest bcr
            HighestBCRIndex.Add(0);
            HighestBCRIndex.Add(0);
            HighestBCRIndex.Add(0);
            BCRs = new List<IndicatorQT1>();
            
            //dataset2 holds sens analysis variables -rates in first row, lifes in second
            for (i = 0; i < data.Count; i++)
            {
                //each row in data get DataSet2[0] * DataSet2[1] rows
                //for sens analysis
                for (int r = 0; r < DataSet2[0].Count(); r++)
                {
                    dbRate = DataSet2[0][r];
                    for (int l = 0; l < DataSet2[1].Count(); l++)
                    {
                        alternLineRows = new List<List<string>>();
                        dbLife = DataSet2[1][l];
                        //each rate and life gets a unique row
                        //1 to 1 correlation between rownames and data
                        baseLineRow = rowNames[i];
                        if (baseLineRow.Count > 2)
                        {
                            sLabel = baseLineRow[0];
                            sAltName = baseLineRow[1];
                            //location is altname in subalg11, get rid of 1.000 mistaken formatting
                            sAltName = CalculatorHelpers.GetParsedString(0, Constants.FILEEXTENSION_DELIMITERS, sAltName);
                            sUnit = baseLineRow[2];
                            //the y col in data holds avg ann damages, the rest are totals
                            if (data[i].Count >= 1)
                            {
                                dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(data[i][0]);
                                //cost multiplier used to set categorical total costs
                                dbBaseCostsQ = CalculatorHelpers.ConvertStringToDouble(data[i][data[i].Count - 1]);
                                //only iterate through the baseline (RF1A) not the project alts (RF1A_A)
                                //only alts have underscores
                                if (!sLabel.Contains(Constants.FILENAME_DELIMITER))
                                {
                                    //alternLineRows = rowNames
                                    //    .Where(rn => rn[0] != sLabel && rn[1] == sAltName && rn[2] == sUnit)
                                    //    .Select(rn => rn.ToList()).ToList();
                                    //need to find the corresonding data rows from alternrows
                                    List<int> dataRowIndexes = new List<int>();
                                    List<string> row = new List<string>();
                                    double dbAltDamage = 0;
                                    double dbAltCostsQ = 0;
                                    for (int j = 0; j < rowNames.Count; j++)
                                    {
                                        row = rowNames[j];
                                        if (row.Count > 2)
                                        {
                                            //location is altname, get rid of 1.000 mistaken formatting
                                            sLocation = CalculatorHelpers.GetParsedString(0, Constants.FILEEXTENSION_DELIMITERS, row[1]);
                                            if (row[0] != sLabel && sLocation == sAltName && row[2] == sUnit)
                                            {
                                                if (row[0].ToUpper().StartsWith(sLabel.ToUpper()))
                                                {
                                                    //get the corresponding altdamage from data
                                                    if (data.Count > j)
                                                    {
                                                        if(_subalgorithm != MATH_SUBTYPES.subalgorithm11.ToString()
                                                            && _subalgorithm != MATH_SUBTYPES.subalgorithm12.ToString())
                                                        {
                                                            dbAltDamage = CalculatorHelpers.ConvertStringToDouble(data[j][0]);
                                                            row.Add(dbAltDamage.ToString());
                                                            dbAltCostsQ = CalculatorHelpers.ConvertStringToDouble(data[j][data[j].Count - 1]);
                                                            row.Add(dbAltCostsQ.ToString());
                                                            alternLineRows.Add(row);
                                                        }
                                                        else
                                                        {
                                                            //altname is unit
                                                            //indicator values are horizontal not vertical
                                                            //qtm
                                                            dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(data[i][2]);
                                                            row.Add(dbBaseDamage.ToString());
                                                            dbAltDamage = CalculatorHelpers.ConvertStringToDouble(data[j][2]);
                                                            row.Add(dbAltDamage.ToString());
                                                            row.Add(string.Concat(sAltName, Constants.FILENAME_DELIMITER, "QTM"));
                                                            //qtl
                                                            dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(data[i][4]);
                                                            row.Add(dbBaseDamage.ToString());
                                                            dbAltDamage = CalculatorHelpers.ConvertStringToDouble(data[j][4]);
                                                            row.Add(dbAltDamage.ToString());
                                                            row.Add(string.Concat(sAltName, Constants.FILENAME_DELIMITER, "QTL"));
                                                            //qtu
                                                            dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(data[i][6]);
                                                            row.Add(dbBaseDamage.ToString());
                                                            dbAltDamage = CalculatorHelpers.ConvertStringToDouble(data[j][6]);
                                                            row.Add(dbAltDamage.ToString());
                                                            row.Add(string.Concat(sAltName, Constants.FILENAME_DELIMITER, "QTU"));
                                                            dbBaseCostsQ = CalculatorHelpers.ConvertStringToDouble(data[i][data[i].Count - 1]);
                                                            row.Add(dbBaseCostsQ.ToString());
                                                            dbAltCostsQ = CalculatorHelpers.ConvertStringToDouble(data[j][data[j].Count - 1]);
                                                            row.Add(dbAltCostsQ.ToString());
                                                            alternLineRows.Add(row);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (alternLineRows != null)
                                    {
                                        if (alternLineRows.Count > 0)
                                        {
                                            if (i == 0 && r == 0 && l == 0)
                                            {
                                                sb.AppendLine(GetBCHeaderRow(sLabel, alternLineRows));
                                                //196 sorts for best cumulative bcr
                                                BCRs = InitBCRs();
                                            }
                                            if (_subalgorithm != MATH_SUBTYPES.subalgorithm11.ToString()
                                                && _subalgorithm != MATH_SUBTYPES.subalgorithm12.ToString())
                                            {
                                                sb.AppendLine(GetBCRow(i, r, l, lines2, dbRate, dbLife,
                                                    sLabel, sAltName, sUnit, dbBaseDamage, dbBaseCostsQ,
                                                    alternLineRows, rowNames));
                                            }
                                            else
                                            {
                                                sAltName = sUnit;
                                                Get3BCRows(sb, rowNames,
                                                    sLabel, sAltName, i, r, l, 
                                                    dbRate, dbLife, alternLineRows, lines2);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //1) copy best qtm into IndicatorQT for display on client
            CopyBestQTM();
            if (IndicatorQT.MathResult.ToLower().StartsWith("http"))
            {
                string sError = string.Empty;
                bool bHasSaved = CalculatorHelpers.SaveTextInURI(
                    Params.ExtensionDocToCalcURI, sb.ToString(), IndicatorQT.MathResult, out sError);
                if (!string.IsNullOrEmpty(sError))
                {
                    IndicatorQT.MathResult += string.Concat("--", sError);
                }
            }
            else
            {
                IndicatorQT.MathResult = sb.ToString();
            }
        }
        private List<IndicatorQT1> InitBCRs()
        {
            BCRs = new List<IndicatorQT1>();
            double dbRate = 0;
            double dbLife = 0;
            for (int r = 0; r < DataSet2[0].Count(); r++)
            {
                dbRate = DataSet2[0][r];
                for (int l = 0; l < DataSet2[1].Count(); l++)
                {
                    dbLife = DataSet2[1][l];
                    //1 line per qtm, qtl, and qtu
                    for (int i = 0; i < 3; i++)
                    {
                        string sBCRType = "QTM";
                        if (i == 1)
                        {
                            sBCRType = "QTL";
                        }
                        else if (i == 2)
                        {
                            sBCRType = "QTU";
                        }
                        IndicatorQT1 bcr = new IndicatorQT1();
                        //index stored in alttype 
                        bcr.AlternativeType 
                            = GetAlternativeIndex(dbRate, dbLife, sBCRType);
                        BCRs.Add(bcr);
                    }
                }
            }
            return BCRs;
        }
        private string GetAlternativeIndex(double rate, double life, string qTotalType)
        {
            string sAltern = string.Concat(qTotalType,
                rate.ToString("F4"), life.ToString("F2"));
            return sAltern;
        }
        private string GetBCHeaderRow(string label, List<List<string>> alternLineRows)
        {
            StringBuilder rb = new StringBuilder();
            if (ColNames.Count() > 3)
            {
                //label
                rb.Append(string.Concat(ColNames[0], Constants.CSV_DELIMITER));
                if ((IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                    || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()))
                {
                    //indicator
                    ColNames[1] = ColNames[2];
                    ColNames[2] = "loc_confid";
                    rb.Append(string.Concat(ColNames[1], Constants.CSV_DELIMITER));
                    rb.Append(string.Concat(ColNames[2], Constants.CSV_DELIMITER));
                }
                else
                {
                    //assettype
                    rb.Append(string.Concat(ColNames[1], Constants.CSV_DELIMITER));
                    //confidence
                    rb.Append(string.Concat(ColNames[2], Constants.CSV_DELIMITER));
                }
                //totals
                rb.Append(string.Concat(ColNames[3], Constants.CSV_DELIMITER));
            }
            //header row
            string sLabelAlt = string.Empty;
            string sLabelCost = string.Empty;
            int i = 0;
            foreach (var alternative in alternLineRows)
            {
                if (alternative.Count() > 0)
                {
                    sLabelAlt = alternative[0].ToString();
                    if (i == 0)
                    {
                        if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()
                            || _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                        {
                            //6A_perform
                            rb.Append(string.Concat(label, Constants.FILENAME_DELIMITER, "perform", Constants.CSV_DELIMITER));
                        }
                        else
                        {
                            //6A_damage, base damage
                            rb.Append(string.Concat(label, Constants.FILENAME_DELIMITER, "damage", Constants.CSV_DELIMITER));
                        }
                        //5A_cost, base cost
                        rb.Append(string.Concat(label, Constants.FILENAME_DELIMITER, "cost", Constants.CSV_DELIMITER));
                    }
                    if (_subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString()
                        || _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString())
                    {
                        //6A_perform
                        rb.Append(string.Concat(sLabelAlt, Constants.FILENAME_DELIMITER, "perform", Constants.CSV_DELIMITER));
                    }
                    else
                    {
                        //6A_damage, base damage
                        rb.Append(string.Concat(sLabelAlt, Constants.FILENAME_DELIMITER, "damage", Constants.CSV_DELIMITER));
                    }
                    
                    rb.Append(string.Concat(sLabelAlt, Constants.FILENAME_DELIMITER, "cost", Constants.CSV_DELIMITER));
                    if (IndicatorIndex == 6)
                    {
                        rb.Append(string.Concat(sLabelAlt, Constants.FILENAME_DELIMITER, "bcr", Constants.CSV_DELIMITER));
                    }
                    else if (IndicatorIndex == 7
                        || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                        || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()))
                    {
                        rb.Append(string.Concat(sLabelAlt, Constants.FILENAME_DELIMITER, "cer", Constants.CSV_DELIMITER));
                    }

                }
                i++;
            }
            //get rid of last csv
            rb = rb.Remove(rb.Length - 1, 1);
            return rb.ToString();
        }
        private void Get3BCRows(StringBuilder sb, List<List<string>> rowNames, 
            string label, string altName, int i, int r, int l, double rate, double life,
            List<List<string>> alternLineRows, List<string> lines2)
        {
            //qtm row
            List<List<string>> qAlternLineRows
                = GetNewAlternLineRows(3, altName, alternLineRows);
            double dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(alternLineRows[0][3]);
            double dbBaseCostsQ = CalculatorHelpers.ConvertStringToDouble(alternLineRows[0][12]);
            string sUnit = alternLineRows[0][5];
            sb.AppendLine(GetBCRow(i, r, l, lines2, rate, life,
                label, altName, sUnit, dbBaseDamage, dbBaseCostsQ,
                qAlternLineRows, rowNames));
            //qtl row
            //change alternRows to QTL
            qAlternLineRows
                = GetNewAlternLineRows(6, altName, alternLineRows);
            dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(alternLineRows[0][6]);
            sUnit = alternLineRows[0][8];
            //append the line
            sb.AppendLine(GetBCRow(i, r, l, lines2, rate, life,
                label, altName, sUnit, dbBaseDamage, dbBaseCostsQ,
                qAlternLineRows, rowNames));
            //qtu row
            qAlternLineRows
                = GetNewAlternLineRows(9, altName, alternLineRows);
            dbBaseDamage = CalculatorHelpers.ConvertStringToDouble(alternLineRows[0][9]);
            sUnit = alternLineRows[0][11];
            sb.AppendLine(GetBCRow(i, r, l, lines2, rate, life,
                label, altName, sUnit, dbBaseDamage, dbBaseCostsQ,
                qAlternLineRows, rowNames));
        }
        private List<List<string>> GetNewAlternLineRows(int colIndex, 
            string altName, List<List<string>> alternLineRows)
        {
            List<List<string>> newAlterns = new List<List<string>>();
            foreach(var row in alternLineRows)
            {
                List<string> newRow = new List<string>();
                newRow.Add(row[0]);
                newRow.Add(altName);
                //change the unit
                newRow.Add(row[colIndex + 2]);
                //change the alt damages
                newRow.Add(row[colIndex + 1]);
                //change the altcostq
                newRow.Add(row[13]);
                newAlterns.Add(newRow.ToList());
            }
            return newAlterns;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d">data row index</param>
        /// <param name="r">rate index</param>
        /// <param name="l">life index</param>
        /// <param name="lines2">costs</param>
        /// <param name="rate">project real rate</param>
        /// <param name="life">project life span</param>
        /// <param name="label"></param>
        /// <param name="assetType"></param>
        /// <param name="unit"></param>
        /// <param name="baseDamage">base damage amount</param>
        /// <param name="baseCostQ">quantity of asset</param>
        /// <param name="alternLineRows">project alternatives</param>
        /// <param name="rowNames">first 3 cols of data</param>
        /// <returns></returns>
        private string GetBCRow(int d, int r, int l, 
            List<string> lines2, double rate, double life, 
            string label, string assetType, string unit, double baseDamage,
            double baseCostQ, List<List<string>> alternLineRows, 
            List<List<string>> rowNames)
        {
            StringBuilder rb = new StringBuilder();
            //header row
            string sLabelCostAlt = string.Empty;
            //kiss for now (first data row is used)
            string[] arrCostRow = lines2[1].Split(Constants.CSV_DELIMITERS);
            string sLabelCost = arrCostRow[0];
            double dbAltDamage = 0;
            double dbAltCost = 0;
            double dbAltCostQ = 0;
            double dbBaseCost = 0;
            double cbBCR = 0;
            string sAssetType = string.Empty;
            string sUnit = string.Empty;
            int i = 0;
            //196 isprojectcost must be returned from cost row along with cost
            List<string> costs = new List<string>();
            bool bIsProjectCost = true;
            //category and score costs have to be calculated from subinds in data[] and costs in lines2
            bool bNeedsGroupCost = false;
            foreach (var alternative in alternLineRows)
            {
                dbAltDamage = 0;
                dbAltCost = 0;
                cbBCR = 0;
                if (alternative.Count() > 3)
                {
                    sLabelCostAlt = alternative[0].ToString();
                    if (i == 0)
                    {
                        //label
                        rb.Append(string.Concat(sLabelCostAlt, Constants.CSV_DELIMITER));
                        //assettype
                        sAssetType = alternative[1];
                        rb.Append(string.Concat(sAssetType, Constants.CSV_DELIMITER));
                        //confidence unit (QTM)
                        sUnit = string.Concat(alternative[2], Constants.FILENAME_DELIMITER,
                             rate.ToString(), Constants.FILENAME_DELIMITER, life.ToString(),
                             Constants.CSV_DELIMITER);
                        rb.Append(sUnit);
                        //totals
                        rb.Append(string.Concat(0, Constants.CSV_DELIMITER));
                        if (IndicatorIndex == 6)
                        {
                            //196: BCR only good with monetary damages
                            baseDamage = Shared.GetUPV(baseDamage, life, rate);
                        }
                        rb.Append(string.Concat(baseDamage, Constants.CSV_DELIMITER));
                        //see if the baseline label needs to calc costs from subinds
                        if (label.Count() == 2 || label.Count() == 3)
                        {
                            bNeedsGroupCost = true;
                        }
                    }
                    //cost labels replace RF1A or RF1B with AC1A (if needed, can accomodate AC1B ...)
                    sLabelCostAlt = string.Concat(sLabelCost, Constants.FILENAME_DELIMITER,
                        CalculatorHelpers.GetParsedString(1, Constants.FILENAME_DELIMITERS, sLabelCostAlt));
                    costs = new List<string>();
                    if (!bNeedsGroupCost)
                    {
                        costs = GetCost(lines2, sLabelCost, unit, rate, life);
                        dbBaseCost = CalculatorHelpers.ConvertStringToDouble(costs[0]);
                        bIsProjectCost = CalculatorHelpers.ConvertStringToBool(costs[1]);
                        if (bIsProjectCost)
                            baseCostQ = 1;
                        //multiply by quantity of assets (for categorical costs)
                        dbBaseCost = dbBaseCost * baseCostQ;
                    }
                    else
                    {
                        //no need yet for any difference
                        costs = GetCost(lines2, sLabelCost, unit, rate, life);
                        dbBaseCost = CalculatorHelpers.ConvertStringToDouble(costs[0]);
                        bIsProjectCost = CalculatorHelpers.ConvertStringToBool(costs[1]);
                        if (bIsProjectCost)
                            baseCostQ = 1;
                        dbBaseCost = dbBaseCost * baseCostQ;
                    }
                    if (i == 0)
                    {
                        //basecost
                        rb.Append(string.Concat(dbBaseCost, Constants.CSV_DELIMITER));
                    }
                    dbAltCostQ = CalculatorHelpers.ConvertStringToDouble(alternative[4]);
                    if (!bNeedsGroupCost)
                    {
                        //alternative costs
                        costs = GetCost(lines2, sLabelCostAlt, unit, rate, life);
                        dbAltCost = CalculatorHelpers.ConvertStringToDouble(costs[0]);
                        bIsProjectCost = CalculatorHelpers.ConvertStringToBool(costs[1]);
                        if (bIsProjectCost)
                            dbAltCostQ = 1;
                        //multiply by quantity of assets (for categorical costs)
                        dbAltCost = dbAltCost * dbAltCostQ;
                    }
                    else
                    {
                        //no need yet for any difference
                        costs = GetCost(lines2, sLabelCostAlt, unit, rate, life);
                        dbAltCost = CalculatorHelpers.ConvertStringToDouble(costs[0]);
                        bIsProjectCost = CalculatorHelpers.ConvertStringToBool(costs[1]);
                        if (bIsProjectCost)
                            dbAltCostQ = 1;
                        dbAltCost = dbAltCost * dbAltCostQ;
                    }
                    dbAltDamage = CalculatorHelpers.ConvertStringToDouble(alternative[3]);
                    if (IndicatorIndex == 6)
                    {
                        //196: BCR only good with monetary damages
                        dbAltDamage = Shared.GetUPV(dbAltDamage, life, rate);
                    }
                    rb.Append(string.Concat(dbAltDamage.ToString(), Constants.CSV_DELIMITER));
                    rb.Append(string.Concat(dbAltCost.ToString(), Constants.CSV_DELIMITER));
                    cbBCR = GetRatio(baseDamage, dbBaseCost, dbAltDamage, dbAltCost,
                        life, rate);
                    rb.Append(string.Concat(cbBCR.ToString(), Constants.CSV_DELIMITER));
                    
                    //196 condition added: only want best cumulative ratio
                    //across locations
                    if (label.ToUpper().StartsWith("TR"))
                    {
                        AddAlternBCR(d, sUnit, r, l, rate, life, i,
                            baseDamage, dbBaseCost, dbAltDamage, dbAltCost,
                            sLabelCostAlt, sAssetType);
                    }
                }
                i++;
            }
            //get rid of last csv
            rb = rb.Remove(rb.Length - 1, 1);
            return rb.ToString();
        }
        private double GetRatio(double baseDamage,
            double baseCost, double altDamage, double altCost,
            double life, double rate)
        {
            double cbBCR = 0;
            double dbDamageChange = 0;
            double dbCostChange = 0;
            if (_subalgorithm != MATH_SUBTYPES.subalgorithm11.ToString()
                && _subalgorithm != MATH_SUBTYPES.subalgorithm12.ToString())
            {
                dbDamageChange = baseDamage - altDamage;
                //old way: dbDamageChange = baseDamage - dbAltDamage;
                dbCostChange = altCost - baseCost;
            }
            else
            {
                //costs should generally rise
                //but these are not damages - they are performance indicators
                //and should be increasing
                dbDamageChange = altDamage - baseDamage;
                dbCostChange = altCost - baseCost;
            }
            if (IndicatorIndex == 6)
            {
                //bcr
                if (dbCostChange == 0)
                    dbCostChange = 1.000;
                cbBCR = dbDamageChange / dbCostChange;
            }
            else if (IndicatorIndex == 7
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()))
            {
                if (dbDamageChange == 0)
                    dbDamageChange = 1.000;
                //cer
                cbBCR = dbCostChange / dbDamageChange;
            }
            //uniform double format
            cbBCR = CalculatorHelpers.ConvertStringToDouble(cbBCR.ToString());
            return cbBCR;
        }
        private double GetNetBenefits(double baseDamage,
            double baseCost, double altDamage, double altCost,
            double life, double rate)
        {
            double netBenefits = 0;
            double dbDamageChange = 0;
            double dbCostChange = 0;
            if (_subalgorithm != MATH_SUBTYPES.subalgorithm11.ToString()
                && _subalgorithm != MATH_SUBTYPES.subalgorithm12.ToString())
            {
                dbDamageChange = baseDamage - altDamage;
                //old way: dbDamageChange = baseDamage - dbAltDamage;
                dbCostChange = altCost - baseCost;
            }
            else
            {
                //costs should generally rise
                //but these are not damages - they are performance indicators
                //and should be increasing
                dbDamageChange = altDamage - baseDamage;
                dbCostChange = altCost - baseCost;
            }
            if (IndicatorIndex == 6)
            {
                //nets
                if (dbCostChange == 0)
                    dbCostChange = 1.000;
                netBenefits = dbDamageChange - dbCostChange;
            }
            else if (IndicatorIndex == 7
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()))
            {
                if (dbDamageChange == 0)
                    dbDamageChange = 1.000;
                //not nets possible, use cer
                netBenefits = dbCostChange / dbDamageChange;
            }
            //uniform double format
            netBenefits = CalculatorHelpers.ConvertStringToDouble(netBenefits.ToString());
            return netBenefits;
        }
        private void AddAlternBCR(double d, string unit,
            double r, double l, double rate, double life, int i,
            double baseDamage, double baseCost, double altDamage, 
            double altCost, string labelCostAlt, string assetType)
        {
            //196 calcs use TR rows to aggregate BCRs across all locations
            string sAlternType = string.Empty;
            string sBCUnit = " bcr";
            if (IndicatorIndex == 7
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm11.ToString())
                || (IndicatorIndex == 3 && _subalgorithm == MATH_SUBTYPES.subalgorithm12.ToString()))
            {
                sBCUnit = " cer";
            }
            bool bNeedsToBeAdded = false;
            if (unit.ToUpper().Contains("QTM"))
            {
                IndicatorQT1 alternBCR
                    = GetAlternativeIndicator("QTM", rate, life);
                if (alternBCR != null)
                {
                    //aggregate across locations into separate alterns
                    IndicatorQT1 alternQT = alternBCR.IndicatorQT1s
                        .FirstOrDefault(b => b.AlternativeType == i.ToString());
                    if (alternQT == null)
                    {
                        alternQT = new IndicatorQT1();
                        alternQT.AlternativeType = i.ToString();
                        bNeedsToBeAdded = true;
                    }
                    //costs have been calculated as discounted costs with uniform pv of o&m
                    //avg annual damages have to do the same
                    alternQT.Q1 += baseDamage; 
                    alternQT.Q1Unit = "base damage";
                    alternQT.Q2 += baseCost;
                    alternQT.Q2Unit = "base cost";
                    alternQT.Q3 += altDamage;
                    alternQT.Q3Unit = string.Concat(labelCostAlt, " damage");
                    alternQT.Q4 += altCost;
                    alternQT.Q4Unit = string.Concat(labelCostAlt, " cost");
                    //damages minus costs
                    alternQT.QT = GetNetBenefits(alternQT.Q1, alternQT.Q2,
                        alternQT.Q3, alternQT.Q4, life, rate);
                    alternQT.QTUnit = "net benefits";
                    //bcr or cer ratios
                    alternQT.QTM = GetRatio(alternQT.Q1, alternQT.Q2,
                        alternQT.Q3, alternQT.Q4, life, rate);
                    alternQT.QTMUnit = string.Concat(unit, sBCUnit);
                    alternQT.QTLUnit = Constants.NONE;
                    alternQT.QTUUnit = Constants.NONE;
                    //avoid blanks
                    alternQT.Q5Unit = Constants.NONE;
                    alternQT.QTD1Unit = Constants.NONE;
                    alternQT.QTD2Unit = Constants.NONE;
                    if (bNeedsToBeAdded)
                    {
                        alternBCR.IndicatorQT1s.Add(alternQT);
                    }
                }
            }
            else if (unit.ToUpper().Contains("QTL"))
            {
                IndicatorQT1 alternBCR
                    = GetAlternativeIndicator("QTL", rate, life);
                if (alternBCR != null)
                {
                    //aggregate across locations into separate alterns
                    IndicatorQT1 alternQT = alternBCR.IndicatorQT1s
                        .FirstOrDefault(b => b.AlternativeType == i.ToString());
                    if (alternQT == null)
                    {
                        alternQT = new IndicatorQT1();
                        alternQT.AlternativeType = i.ToString();
                        bNeedsToBeAdded = true;
                    }
                    alternQT.Q1 += baseDamage;
                    alternQT.Q1Unit = "base damage";
                    alternQT.Q2 += baseCost;
                    alternQT.Q2Unit = "base cost";
                    alternQT.Q3 += altDamage;
                    alternQT.Q3Unit = string.Concat(labelCostAlt, " damage");
                    alternQT.Q4 += altCost;
                    alternQT.Q4Unit = string.Concat(labelCostAlt, " cost");
                    //damages minus costs
                    alternQT.QT = GetNetBenefits(alternQT.Q1, alternQT.Q2,
                        alternQT.Q3, alternQT.Q4, life, rate);
                    alternQT.QTUnit = "net benefits";
                    //bcr or cer ratios
                    alternQT.QTL = GetRatio(alternQT.Q1, alternQT.Q2,
                        alternQT.Q3, alternQT.Q4, life, rate);
                    alternQT.QTLUnit = string.Concat("lower ", CILevel.ToString(), " %", sBCUnit);
                    alternQT.QTMUnit = Constants.NONE;
                    alternQT.QTUUnit = Constants.NONE;
                    //avoid blanks
                    alternQT.Q5Unit = Constants.NONE;
                    alternQT.QTD1Unit = Constants.NONE;
                    alternQT.QTD2Unit = Constants.NONE;
                    if (bNeedsToBeAdded)
                    {
                        alternBCR.IndicatorQT1s.Add(alternQT);
                    }
                }
            }
            else if (unit.ToUpper().Contains("QTU"))
            {
                IndicatorQT1 alternBCR
                    = GetAlternativeIndicator("QTU", rate, life);
                if (alternBCR != null)
                {
                    //aggregate across locations into separate alterns
                    IndicatorQT1 alternQT = alternBCR.IndicatorQT1s
                        .FirstOrDefault(b => b.AlternativeType == i.ToString());
                    if (alternQT == null)
                    {
                        alternQT = new IndicatorQT1();
                        alternQT.AlternativeType = i.ToString();
                        bNeedsToBeAdded = true;
                    }
                    alternQT.Q1 += baseDamage;
                    alternQT.Q1Unit = "base damage";
                    alternQT.Q2 += baseCost;
                    alternQT.Q2Unit = "base cost";
                    alternQT.Q3 += altDamage;
                    alternQT.Q3Unit = string.Concat(labelCostAlt, " damage");
                    alternQT.Q4 += altCost;
                    alternQT.Q4Unit = string.Concat(labelCostAlt, " cost");
                    //damages minus costs
                    alternQT.QT = GetNetBenefits(alternQT.Q1, alternQT.Q2,
                         alternQT.Q3, alternQT.Q4, life, rate);
                    alternQT.QTUnit = "net benefits";
                    //bcr or cer ratios
                    alternQT.QTU = GetRatio(alternQT.Q1, alternQT.Q2,
                        alternQT.Q3, alternQT.Q4, life, rate);
                    alternQT.QTUUnit = string.Concat("upper ", CILevel.ToString(), " %", sBCUnit);
                    alternQT.QTLUnit = Constants.NONE;
                    alternQT.QTMUnit = Constants.NONE;
                    //avoid blanks
                    alternQT.Q5Unit = Constants.NONE;
                    alternQT.QTD1Unit = Constants.NONE;
                    alternQT.QTD2Unit = Constants.NONE;
                    if (bNeedsToBeAdded)
                    {
                        alternBCR.IndicatorQT1s.Add(alternQT);
                    }
                }
            }
        }
        private IndicatorQT1 GetAlternativeIndicator(string qTotalType,
            double rate, double life)
        {
            string sAlternType = GetAlternativeIndex(rate, life, qTotalType);
            IndicatorQT1 alternBCR = BCRs
                .FirstOrDefault(b => b.AlternativeType == sAlternType);
            if (alternBCR != null)
            {
                if (alternBCR.IndicatorQT1s == null)
                {
                    alternBCR.IndicatorQT1s = new List<IndicatorQT1>();
                }
            }
            return alternBCR;
        }
        private void CopyBestQTM()
        {
            if (BCRs != null)
            {
                IndicatorQT1 highestQT = new IndicatorQT1();
                string sAlternativeType = string.Empty;
                string sAlternativeIndex = string.Empty;
                foreach (var qt in BCRs)
                {
                    if (qt.AlternativeType.Contains("QTM"))
                    {
                        sAlternativeType = qt.AlternativeType;
                        if (IndicatorIndex == 6)
                        {
                            //highest bcr
                            foreach (var alternQt in qt.IndicatorQT1s)
                            {
                                if (alternQt.QTM > highestQT.QTM)
                                {
                                    highestQT.CopyIndicatorQT1Properties(highestQT, alternQt);
                                    highestQT.AlternativeType = sAlternativeType;
                                    sAlternativeIndex = alternQt.AlternativeType;
                                }
                            }
                        }
                        else
                        {
                            //change QTM from zero
                            if (!highestQT.AlternativeType.Contains("QTM"))
                            {
                                highestQT.QTM = 1000000000;
                            }
                            //lowest cer
                            foreach (var alternQt in qt.IndicatorQT1s)
                            {
                                if (alternQt.QTM < highestQT.QTM)
                                {
                                    highestQT.CopyIndicatorQT1Properties(highestQT, alternQt);
                                    highestQT.AlternativeType = sAlternativeType;
                                    sAlternativeIndex = alternQt.AlternativeType;
                                }
                            }
                        }
                    }
                }
                if (highestQT.AlternativeType.Contains("QTM"))
                {
                    sAlternativeType = highestQT.AlternativeType
                        .Replace("QTM", "QTL");
                    IndicatorQT1 QTL = BCRs
                        .FirstOrDefault(b => b.AlternativeType == sAlternativeType);
                    if (QTL != null)
                    {
                        IndicatorQT1 alternQT = QTL.IndicatorQT1s
                            .FirstOrDefault(b => b.AlternativeType == sAlternativeIndex);
                        if (alternQT != null)
                        {
                            highestQT.QTL = alternQT.QTL;
                            highestQT.QTLUnit = alternQT.QTLUnit;
                        }
                    }
                    sAlternativeType = sAlternativeType.Replace("QTL", "QTU");
                    IndicatorQT1 QTU = BCRs
                        .FirstOrDefault(b => b.AlternativeType == sAlternativeType);
                    if (QTU != null)
                    {
                        IndicatorQT1 alternQT = QTU.IndicatorQT1s
                            .FirstOrDefault(b => b.AlternativeType == sAlternativeIndex);
                        if (alternQT != null)
                        {
                            highestQT.QTU = alternQT.QTU;
                            highestQT.QTUUnit = alternQT.QTUUnit;
                        }
                    }
                    //change math and nothing more
                    IndicatorQT.Q1 = highestQT.Q1;
                    IndicatorQT.Q1Unit = highestQT.Q1Unit;
                    IndicatorQT.Q2 = highestQT.Q2;
                    IndicatorQT.Q2Unit = highestQT.Q2Unit;
                    IndicatorQT.Q3 = highestQT.Q3;
                    IndicatorQT.Q3Unit = highestQT.Q3Unit;
                    IndicatorQT.Q4 = highestQT.Q4;
                    IndicatorQT.Q4Unit = highestQT.Q4Unit;
                    IndicatorQT.QT = highestQT.QT;
                    IndicatorQT.QTUnit = highestQT.QTUnit;
                    IndicatorQT.QTM = highestQT.QTM;
                    IndicatorQT.QTMUnit = highestQT.QTMUnit;
                    IndicatorQT.QTL = highestQT.QTL;
                    IndicatorQT.QTLUnit = highestQT.QTLUnit;
                    IndicatorQT.QTU = highestQT.QTU;
                    IndicatorQT.QTUUnit = highestQT.QTUUnit;
                    //avoid blanks
                    IndicatorQT.Q5Unit = Constants.NONE;
                    IndicatorQT.QTD1Unit = Constants.NONE;
                    IndicatorQT.QTD2Unit = Constants.NONE;
                }
            }
        }
        
        private List<string> GetCost(List<string> lines2, string altlabel, string unit, 
            double rate, double life)
        {
            //196 variable added to cost file
            List<string> costcalcs = new List<string>();
            costcalcs.Add("0");
            costcalcs.Add("0");
            string sCost = string.Empty;
            string sInd5AltLabel = altlabel;
            string sRate = string.Empty;
            int iLifeColIndex = 0;
            int i = 0;
            foreach (var costrow in lines2)
            {
                string[] costs = costrow.Split(Constants.CSV_DELIMITERS);
                if (costs != null)
                {
                    if (costs.Count() > 4)
                    {
                        if (i == 0)
                        {
                            for (int j = 4; j < costs.Count(); j++)
                            {
                                if (costs[j].ToString().ToUpper().EndsWith(life.ToString().ToUpper()))
                                {
                                    iLifeColIndex = j;
                                }
                            }
                        }
                        else
                        {
                            //RF1A
                            if (costs[0].ToString().ToUpper().Equals(sInd5AltLabel.ToUpper()))
                            {
                                //1_QT_0.05
                                if (costs[2].ToUpper().StartsWith(unit.ToUpper()))
                                {
                                    string[] rates = costs[2].Split(Constants.FILENAME_DELIMITERS);
                                    if (rates.Count() == 3)
                                    {
                                        sRate = rates[2];
                                        if (!string.IsNullOrEmpty(sRate))
                                        {
                                            if (CalculatorHelpers.ConvertStringToDouble(sRate) == rate)
                                            {
                                                //this is the correct cost row
                                                //now get the correct rate column
                                                sCost = costs[iLifeColIndex];
                                                
                                                if (string.IsNullOrEmpty(sCost))
                                                {
                                                    //return the zeros already in costcalcs list
                                                }
                                                else
                                                {
                                                    costcalcs[0] = sCost;
                                                    //196 isprojectcost? in last column
                                                    costcalcs[1] = costs[costs.Count() - 1];
                                                    return costcalcs;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                i++;
            }
            return costcalcs;
        }
        //retain the following 2 methods for reference
        private double GetBaseCategoryCost(List<List<string>> rowNames, 
            List<string> lines2, string groupLabel, string altlabel, string unit,
            double rate, double life)
        {
            double dbCatCost = 0;
            List<string> row = new List<string>();
            string[] arr = new string[] { };
            for (int j = 0; j < rowNames.Count; j++)
            {
                row = rowNames[j];
                if (row.Count > 2)
                {
                    //2 conditions to find subinds 1. label with RF1 and unit match 1_QTM
                    if (row[0] != groupLabel && row[2] == unit)
                    {
                        if (!row[0].ToUpper().Contains(Constants.FILENAME_DELIMITER))
                        {
                            if (row[0].ToUpper().StartsWith(groupLabel.ToUpper()))
                            {
                                //last condition: subinds have exactly 4 chars in name: RF1A, RF1B
                                arr = row[0].Split(Constants.FILENAME_DELIMITERS);
                                if (arr[0].Count() == 4)
                                {
                                    //dbCatCost = GetCost(lines2, altlabel, unit, rate, life);
                                    return dbCatCost;
                                }
                            }
                        }
                    }
                }
            }
            return dbCatCost;
        }
        private double GetAltCategoryCost(List<List<string>> rowNames,
            List<string> lines2, string groupLabel, string altlabel, string unit,
            double rate, double life)
        {
            double dbCatCost = 0;
            List<string> row = new List<string>();
            int iDelimitedIndex = altlabel.IndexOf(Constants.FILENAME_DELIMITER);
            if (iDelimitedIndex < 0)
                iDelimitedIndex = 0;
            string sAltNumber = altlabel.Remove(0, iDelimitedIndex);
            string[] arr = new string[] { };
            for (int j = 0; j < rowNames.Count; j++)
            {
                row = rowNames[j];
                if (row.Count > 2)
                {
                    if (row[0] != groupLabel && row[2] == unit)
                    {
                        //_A
                        if (row[0].ToUpper().EndsWith(sAltNumber))
                        {
                            if (row[0].ToUpper().StartsWith(groupLabel.ToUpper()))
                            {
                                //last condition: subinds have exactly 4 chars in name: RF1A, RF1B
                                arr = row[0].Split(Constants.FILENAME_DELIMITERS);
                                if (arr[0].Count() == 4)
                                {
                                    //dbCatCost = GetCost(lines2, altlabel, unit, rate, life);
                                    return dbCatCost;
                                }
                            }
                        }
                    }
                }
            }
            return dbCatCost;
        }
        private string GetName(string name)
        {
            string sName = string.Empty;
            //these need to line up correctly
            if (name.Count() > 20)
            {
                //letters use more space than " "
                sName = name.Substring(0, 16);
            }
            else if (name.Count() < 20)
            {
                int c = 22 - name.Count();
                sName = name;
                for (int i = 0; i <= c; i++)
                {
                    sName += " ";
                }
            }
            return sName;
        }
    }
}
