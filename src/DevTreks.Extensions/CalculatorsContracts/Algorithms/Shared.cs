using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Globalization;


namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Shared static functions that support algos
    ///Author:		www.devtreks.org
    ///Date:		2017, April
    ///References:	CTA reference
    ///</summary>
    public static class Shared
    {

        public static double[] t025 = new double[] { 12.706, 4.303, 3.182, 2.776, 2.571, 2.447, 2.365, 2.306, 2.262, 2.228, 2.201,
            2.179, 2.160, 2.145, 2.131, 2.120, 2.110, 2.101, 2.093, 2.086, 2.080, 2.074, 2.069, 2.064, 2.060, 2.056, 2.052,
            2.048, 2.045, 1.96, };
        public static double GetTStat95(int df)
        {
            double tstat = 0;
            ////2 sided test for 95% ci
            //tstat = MathNet.Numerics.ExcelFunctions.TInv(0.025, df);

            //1 sided result
            //tstat = t025[t025.Count() - 1];
            ////df is one based
            if (df > 29)
            {
                //1 sided result
                tstat = t025[t025.Count() - 1];
            }
            else
            {
                tstat = t025[df - 1];
            }
            return tstat;
        }
        public static double GetPValueForFDist(int dfn, int dfd, double testValue)
        {
            //f dist = fishersnedecor distribution
            double pvalue = MathNet.Numerics.Distributions.FisherSnedecor.CDF(dfn, dfd, testValue);
            pvalue = 1 - pvalue;
            //which is equal to:
            //double pv1 = MathNet.Numerics.ExcelFunctions.FDist(testValue, dfn, dfd);
            pvalue = Math.Round(pvalue, 4);
            if (pvalue == 0)
            {
                pvalue = .0001;
            }
            if (pvalue < 0)
            {
                pvalue = pvalue * -1;
            }
            //1d - FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x)
            //FDIST(15.20675,6,4) equals 0.01
            //FINV(0.01,6,4) equals 15.20675
            return pvalue;
        }
        public static double GetPValueForTDist(int df, double testValue, double mean, double variance)
        {
            if (mean < 0)
            {
                mean = mean * -1;
            }
            if (variance == 0)
            {
                variance = 1;
            }
            double lowertail = MathNet.Numerics.Distributions.StudentT.CDF(mean, variance, df, testValue);
            double uppertail = MathNet.Numerics.Distributions.StudentT.CDF(mean, variance, df, (testValue * -1));
            double pvalue = 2 * lowertail;
            if (testValue > 0)
            {
                pvalue = 2 * uppertail;
            }
            pvalue = Math.Round(pvalue, 4);
            if (pvalue == 0)
            {
                pvalue = .0001;
            }
            if (pvalue < 0)
            {
                pvalue = pvalue * -1;
            }
            //TDIST(1.96,60,2) equals 0.054645, or 5.46 percent
            //TINV(0.054645,60) equals 1.96
            return pvalue;
        }
        public static bool IsDouble(string test)
        {
            bool bIsDouble = false;
            double dbTest = CalculatorHelpers.ConvertStringToDouble(test);
            if (dbTest != 0)
            {
                bIsDouble = true;
            }
            return bIsDouble;
        }
        public static void AddStringArrayToDoubleArray(string[] strArray, 
            List<List<double>> trends)
        {
            //trends
            List<double> trend = new List<double>();
            for (int k = 0; k < strArray.Count(); k++)
            {
                trend.Add(CalculatorHelpers
                    .ConvertStringToDouble(strArray[k]));
            }
            trends.Add(trend);
        }
        public static string[] AddStringArrayToStringArray(string[] strArray,
            List<List<double>> trends, int rowIndex)
        {
            string[] newArray = new string[] { };
            if (trends.Count > rowIndex)
            {
                List<double> trend = trends[rowIndex];
                newArray = new string[trend.Count];
                if (strArray == null)
                    strArray = new string[trend.Count];
                if (strArray.Count() == 0)
                {
                    strArray = new string[trend.Count];
                }
                double trendValue = 0;
                double stringArrayValue = 0;
                for (int k = 0; k < strArray.Count(); k++)
                {
                    stringArrayValue = CalculatorHelpers
                        .ConvertStringToDouble(strArray[k]);
                    if (trend.Count > k)
                    {
                        trendValue = trend[k] + stringArrayValue;
                        newArray[k] = trendValue.ToString();
                    }
                }
            }
            return newArray;
        }
        public static string[] AddStringArrayToStringArray(string[] strArray, int strArrayCount,
            string[] strArray2)
        {
            string[] newArray = new string[strArrayCount];
            if (strArray == null)
                strArray = new string[strArrayCount];
            if (strArray.Count() == 0)
            {
                strArray = new string[strArrayCount];
            }
            if (strArray2 == null)
                strArray2 = new string[strArrayCount];
            if (strArray2.Count() == 0)
            {
                strArray2 = new string[strArrayCount];
            }
            double stringArrayValue = 0;
            double stringArrayValue2 = 0;
            for (int k = 0; k < strArray.Count(); k++)
            {
                stringArrayValue = CalculatorHelpers
                    .ConvertStringToDouble(strArray[k]);
                if (strArray2.Count() > k)
                {
                    stringArrayValue2 = CalculatorHelpers
                        .ConvertStringToDouble(strArray2[k]);
                    newArray[k] = (stringArrayValue + stringArrayValue2).ToString();
                }
            }
            return newArray;
        }
        public static void CopyStringDataToStringData(List<List<string>> dataToCopy, List<List<string>> data, 
            int numOfCols, int startColIndex)
        {
            int iRow = 0;
            int iCol = 0;
            int iColStop = numOfCols + startColIndex;
            double dbZero = 0;
            foreach(var row in dataToCopy)
            {
                foreach(var col in dataToCopy)
                {
                    if (data.Count() > iRow)
                    {
                        for(int i = startColIndex; i < iColStop; i++)
                        {
                            if (row.Count > i && data[iRow].Count > i)
                            {
                                //don't overwrite the location and tr indexes (they init with zero)
                                dbZero = CalculatorHelpers.ConvertStringToDouble(row[i]);
                                if (dbZero != 0)
                                {
                                    data[iRow][i] = row[i];
                                }
                            }
                        }

                    }
                    iCol++;
                }
                iRow++;
            }
        }
        public static double[,] GetDoubleArray(List<List<double>> data)
        {
            double[,] problemData = new double[,] { };
            int i = 0;
            foreach (var dlist in data)
            {
                if (i == 0)
                {
                    problemData = new double[data.Count(), dlist.Count()];
                }
                int j = 0;
                foreach (var d in dlist)
                {
                    problemData[i, j] = d;
                    j++;
                }
                i++;
            }
            return problemData;
        }
        public static double[] GetDoubleArrayColumn(int colIndex, List<List<double>> data)
        {
            double[] colData = new double[data.Count()];
            int i = 0;
            foreach (var dlist in data)
            {
                int j = 0;
                foreach (var d in dlist)
                {
                    if (j == colIndex)
                    {
                        colData[i] = d;
                        break;
                    }
                    j++;
                }
                i++;
            }
            return colData;
        }
        public static Matrix<double> GetDoubleMatrix(List<List<double>> data, string[] colNames,
            string[] dataColNames)
        {
            //convert data to a Math.Net Matrix
            //positive definite matrix, or square (replace y col with an intercept col)
            Matrix<double> m = Matrix<double>.Build.Dense(data.Count(), dataColNames.Count());
            //let bad indexes throw errors -they return bad index error message
            //skip first column holding y vars
            int k = 0;
            int iDataColumn = 0;
            for (int i = 0; i < colNames.Count(); i++)
            {
                if (i == 0)
                {
                    //ignore the yvector in the first column
                    //instead, add an intercept column for B0 with all 1s
                    double[] intercepts = new double[data.Count()];
                    for (int j = 0; j < intercepts.Count(); j++)
                    {
                        intercepts[j] = 1;
                    }
                    m.SetColumn(i, intercepts.ToArray());
                    //first colname is the intercept
                    k++;
                }
                else
                {
                    //datacolnames coincides with what is in data
                    if (NeedsColumnName(dataColNames, colNames[i]))
                    {
                        //dependent vars start in colNames[4]
                        //data[1] corresponds to that column, or 4 - 3 = 1
                        iDataColumn = i - 3;
                        var colX = from col in data select col.ElementAt(iDataColumn);
                        m.SetColumn(k, colX.ToArray());
                        k++;
                    };

                }
            }
            return m;
        }
        public static double GetObservationsPerCell(List<List<double>> data, int distinctDataColumnIndex,
            double factor1, double level1)
        {
            double r = 0;
            //obs per cell coincide with first factor col[1] and first level col[2] in first row of data
            //error checking already carried out on cols and rows counts
            int i = 0;
            bool bIsFactor1 = false;
            bool bIsLevel1 = false;
            foreach (var factor in data)
            {
                foreach (var level in factor)
                {
                    if (i == 1)
                    {
                        if (level == factor1)
                        {
                            bIsFactor1 = true;
                        }
                    }
                    else if (i == 2)
                    {
                        if (level == level1)
                        {
                            bIsLevel1 = true;
                        }
                    }
                    i++;
                }
                if (bIsFactor1 && bIsLevel1)
                {
                    r++;
                }
                bIsFactor1 = false;
                bIsLevel1 = false;
                i = 0;
            }
            return r;
        }
        public static double GetMeanPerCell(List<List<double>> data, int distinctFColIndex, int distinctLColIndex,
            int distinctRow1Index, int distinctRow2Index, double r)
        {
            double ymean = 0;
            //second col are factors
            var colX = from col in data select col.ElementAt(distinctFColIndex);
            if (distinctRow1Index > (colX.Count() - 1))
            {
                return ymean;
            }
            var factor1 = colX.Distinct().ElementAt(distinctRow1Index);

            var colX2 = from col in data select col.ElementAt(distinctLColIndex);
            if (distinctRow2Index > (colX2.Count() - 1))
            {
                return ymean;
            }
            var level1 = colX2.Distinct().ElementAt(distinctRow2Index);
            //obs per cell coincide with first factor col[1] and first level col[2] in first row of data
            //error checking already carried out on cols and rows counts
            int i = 0;
            bool bIsFactor1 = false;
            bool bIsLevel1 = false;
            double dbY = 0;
            foreach (var factor in data)
            {
                foreach (var level in factor)
                {
                    if (i == 1)
                    {
                        if (level == factor1)
                        {
                            bIsFactor1 = true;
                        }
                    }
                    else if (i == 2)
                    {
                        if (level == level1)
                        {
                            bIsLevel1 = true;
                        }
                    }
                    else
                    {
                        //y obs in col[0]
                        dbY = level;
                    }
                    i++;
                }
                if (bIsFactor1 && bIsLevel1)
                {
                    ymean += dbY;
                }
                bIsFactor1 = false;
                bIsLevel1 = false;
                i = 0;
                dbY = 0;
            }
            ymean = ymean / r;
            return ymean;
        }
        public static double GetTotalInteraction(Matrix<double> forl, double r)
        {
            double totalinteraction = 0;
            double singleinteraction = 0;
            int i = 1;
            for (int j = 0; j < r; j++)
            {
                i = 1;
                //either block or treats can be used
                foreach (var item in forl.Column(j))
                {
                    singleinteraction += item;
                    if (i == r)
                    {
                        totalinteraction += Math.Pow(singleinteraction, 2);
                        singleinteraction = 0;
                        i = 1;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return totalinteraction;
        }

        public static Matrix<double> GetDistinctMatrix(List<List<double>> data, int distinctDataColumnIndex)
        {
            //error checking already checked the number of columns
            var colY = from col in data select col.ElementAt(0);
            var colX = from col in data select col.ElementAt(distinctDataColumnIndex);
            var distinctX = colX.Distinct();
            List<List<double>> ydata = new List<List<double>>();
            foreach (var colIndex in distinctX)
            {
                List<double> newcol = new List<double>();
                ydata.Add(newcol);
            }
            int i = 0;
            foreach (var item in colX)
            {
                int j = 0;
                foreach (var colitem in distinctX)
                {
                    if (item == colitem)
                    {
                        ydata.ElementAt(j).Add(colY.ElementAt(i));
                    }
                    j++;
                }
                i++;
            }
            int rowcount = 0;
            if (ydata.ElementAt(0) != null)
            {
                rowcount = ydata.ElementAt(0).Count();
            }
            Matrix<double> m = GetMatrix(ydata, rowcount, distinctX.Count());
            return m;
        }
        public static Matrix<double> GetMatrix(List<List<double>> data, int rowcount, int colCount)
        {
            Matrix<double> m = Matrix<double>.Build.Dense(rowcount, colCount);
            int i = 0;
            foreach (var col in data)
            {
                m.SetColumn(i, col.ToArray());
                i++;
            }
            return m;
        }
        public static List<List<string>> GetRProjectData(List<List<string>> data, string[] colNames,
            string[] dataColNames)
        {
            //convert data to 2 d string lists
            List<List<string>> rData = new List<List<string>>();
            //let bad indexes throw errors -they return bad index error message
            int k = 0;
            //int iDataColumn = 0;
            //add dep column to col count
            //int iColumnCount = dataColNames.Count() + 1;
            for (int i = 0; i < colNames.Count(); i++)
            {
                if (i == 0)
                {
                    //dependent vars start in first columns
                    var colX = from col in data select col.ElementAt(k);
                    rData.Add(colX.ToList());
                    k++;
                }
                else
                {
                    //datacolnames coincides with what is in data
                    if (NeedsColumnName(dataColNames, colNames[i]))
                    {
                        //dependent vars start in colNames[4] or fifth column
                        //data[1] corresponds to that column, or 4 - 3 = 1
                        //iDataColumn = i - 3;
                        var colX = from col in data select col.ElementAt(k);
                        rData.Add(colX.ToList());
                        //rData[k] = colX.ToList();
                        k++;
                    };
                }
            }
            return rData;
        }
        public static StringBuilder GetRProjectDataFile(List<List<string>> data, string[] colNames,
            string[] depColNames)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetCSVRowHeader(colNames, depColNames));
            //get the raw data
            List<List<string>> rData = GetRProjectData(data, colNames, depColNames);
            if (rData.Count > 0)
            {
                List<string> row = new List<string>();
                int iRowCount = rData[0].Count();
                for (int i = 0; i < rData.Count(); i++)
                {
                    row = new List<string>();
                    //has 3 rows of column data
                    for (int j = 0; j < iRowCount; j++)
                    {
                        row.Add(rData[i][j]);
                    }
                    sb.AppendLine(GetCSVRow(row.ToArray()));
                }
            }
            return sb;
        }
        public static string GetCSVRowHeader(string[] colNames, string[] row)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            //4th column is dep var
            sb.Append(colNames[3]);
            sb.Append(Constants.CSV_DELIMITER);
            foreach (var s in row)
            {
                sb.Append(s);
                //row doesn't end in a delimiter
                if (i != (row.Count() - 1))
                {
                    sb.Append(Constants.CSV_DELIMITER);
                }
                i++;
            }
            return sb.ToString();
        }
        public static string GetCSVRow(string[] row)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var s in row)
            {
                if (i == 0)
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append(s);
                    //row doesn't end in a delimiter
                    if (i != (row.Count() - 1))
                    {
                        sb.Append(Constants.CSV_DELIMITER);
                    }
                }
                i++;
            }
            return sb.ToString();
        }
        public static bool NeedsColumnName(string[] colNames, string colName)
        {
            //only use the columns that are in the mathexpression
            bool bHasTerm = false;
            for (int i = 0; i < colNames.Count(); i++)
            {
                if (colName.ToLower() == colNames[i].ToLower())
                {
                    return true;
                }
            }
            return bHasTerm;
        }


        public static int GetColNameCount(string[] mathTerms, string[] colNames)
        {
            int iColCount = 0;
            //only use the columns that are in the mathexpression;
            for (int i = 0; i < colNames.Count(); i++)
            {
                for (int j = 0; j < mathTerms.Count(); j++)
                {
                    if (colNames[i].ToLower() == mathTerms[j].ToLower())
                    {
                        iColCount++;
                    }
                }
            }
            return iColCount;
        }
        public static bool MathExpressHasColumnName(string[] mathTerms, string colName)
        {
            //only use the columns that are in the mathexpression
            bool bHasTerm = false;
            for (int i = 0; i < mathTerms.Count(); i++)
            {
                if (colName.ToLower() == mathTerms[i].ToLower())
                {
                    return true;
                }
            }
            return bHasTerm;
        }

        public static string GetMathTermQxString(string mathTerm, int zerobasedIndex)
        {
            string[] arrMathTerms = mathTerm.Split(Constants.FILEEXTENSION_DELIMITERS);
            string sColName = string.Empty;
            if (arrMathTerms.Count() >= (zerobasedIndex + 1))
            {
                sColName = arrMathTerms[zerobasedIndex];
            }
            return sColName;
        }
        public static Vector<double> GetYData(List<List<double>> data)
        {
            //first columns holds the ys
            var ys = from row in data select row.ElementAt(0);
            Vector<double> y = Vector<double>.Build.Dense(ys.ToArray());
            return y;
        }

        public static double[] GetVector(List<List<double>> data, int col)
        {
            //first columns holds the ys
            var ys = from row in data select row.ElementAt(col);
            double[] y = ys.ToArray();
            return y;
        }


        public static Vector<double> GetYData(List<List<string>> data)
        {
            //first columns holds the ys as strings
            var ys = from row in data
                     select row.ElementAt(0);
            //convert to doubles
            var yds = ys
                .Select(y1 => CalculatorHelpers.ConvertStringToDouble(y1))
                .ToArray();
            //convert to vector
            Vector<double> y = Vector<double>.Build.Dense(yds.ToArray());
            return y;
        }
        public static int[][] GetYDataCategories(List<List<string>> data)
        {
            //first columns holds the ys
            var ys = GetYData(data);
            List<double> colCategories = ys.Distinct().ToList();
            var counts = new int[ys.Count()][];
            int[] category = new int[colCategories.Count()];
            int i = 0;
            foreach (var y in ys)
            {
                int j = 0;
                category = new int[colCategories.Count()];
                foreach (var c in colCategories)
                {
                    if (c == y)
                    {
                        //the position defines the category (i.e. {0, 0, 1, 0, 0} index[2] = green)
                        category[j] = 1;
                    }
                    else
                    {
                        category[j] = 0;
                    }
                    j++;
                }
                counts[i] = category;
                i++;
            }
            return counts;
        }
        private static Vector<double> GetYData1()
        {
            double[] yd = new double[5] { 1, 1, 2, 2, 4 };
            //first columns holds the ys
            Vector<double> y = Vector<double>.Build.Dense(yd.ToArray());
            return y;
        }
        private static Matrix<double> GetXMatrix(List<List<double>> data, string[] colNames)
        {
            //convert data to a Math.Net Matrix
            //positive definite matrix, or square (replace y col with an intercept col)
            Matrix<double> m = Matrix<double>.Build.Dense(data.Count(), colNames.Count());
            //skip first column holding y vars
            for (int i = 0; i < colNames.Count(); i++)
            {
                if (i == 0)
                {
                    //add an intercept column for B0 with all 1s
                    double[] intercepts = new double[data.Count()];
                    for (int j = 0; j < intercepts.Count(); j++)
                    {
                        intercepts[j] = 1;
                    }
                    m.SetColumn(i, intercepts.ToArray());
                }
                else
                {
                    var colX = from col in data select col.ElementAt(i);
                    m.SetColumn(i, colX.ToArray());
                    //var rowX = from row in data select row.ElementAt(i);
                    //m.SetColumn(i, rowX.ToArray());
                }
            }
            return m;
        }
        private static Matrix<double> GetXMatrix1()
        {
            //convert data to a Math.Net Matrix
            //positive definite matrix, or square
            Matrix<double> m = Matrix<double>.Build.Dense(5, 2);
            double[] xo = new double[5] { 1, 1, 1, 1, 1 };
            m.SetColumn(0, xo.ToArray());
            double[] xi = new double[5] { 1, 2, 3, 4, 5 };
            m.SetColumn(1, xi.ToArray());
            return m;
        }
        private static double[][] GetXData(List<List<double>> data, string[] colNames)
        {
            Matrix<double> xm = GetXMatrix(data, colNames);
            //convert data to a Math.Net Matrix
            //positive definite matrix, or square
            double[][] x = new double[xm.RowCount][];
            for (int i = 0; i < x.Length; ++i)
            {
                //CTA example uses 4 inputvars and 3 outputvals: (x0, x1, x2, x3), (y0, y1, y2)
                x[i] = new double[xm.ColumnCount];
            }
            for (int row = 0; row < xm.RowCount; ++row)
            {
                //inputs (cols - 1)
                //var ys = from yi in data select yi.ElementAt(row + 1);
                for (int col = 0; col < (xm.ColumnCount); ++col)
                {
                    x[row][col] = xm.Row(row)[col];
                }
            }
            return x;
        }
        public static string GetLine(string[] inputs, bool isTitles)
        {
            StringBuilder sb = new StringBuilder();
            string spaces = string.Empty;
            for (int i = 0; i < 17; i++)
            {
                if (isTitles)
                {
                    //spaces += "--";
                    spaces += " ";
                }
                else
                {
                    spaces += " ";
                }
            }
            int iLengthMissing = 0;
            for (int i = 0; i < inputs.Count(); i++)
            {
                iLengthMissing = 17 - inputs[i].Length;
                if (iLengthMissing < 0)
                    iLengthMissing = 0;
                sb.Append(inputs[i]);
                if (i != inputs.Count() - 1)
                {
                    sb.Append(spaces.Substring(0, iLengthMissing));
                }
            }
            return sb.ToString();
        }
        public static string MakeUniformString(string input, bool isTitles)
        {
            StringBuilder sb = new StringBuilder();
            string spaces = string.Empty;
            for (int i = 0; i < 17; i++)
            {
                if (isTitles)
                {
                    //spaces += "--";
                    spaces += " ";
                }
                else
                {
                    spaces += " ";
                }
            }
            int iLengthMissing = 0; iLengthMissing = 17 - input.Length;
            if (iLengthMissing < 0)
                iLengthMissing = 0;
            sb.Append(input);
            sb.Append(spaces.Substring(0, iLengthMissing));
            return sb.ToString();
        }
        
        public static double GetNormalizedValue(string subIndNormType, double startValue,
            MathNet.Numerics.Statistics.DescriptiveStatistics stats)
        {
            double dbNValue = startValue;
            if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.none.ToString()
                || string.IsNullOrEmpty(subIndNormType))
            {
                //data has already been normalized
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.zscore.ToString())
            {
                //z-score: (x – mean(x)) / stddev(x)
                dbNValue = (startValue - stats.Mean) / stats.StandardDeviation;
                dbNValue = CalculatorHelpers.CheckForNaNandRound4(dbNValue);
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.minmax.ToString())
            {
                //min-max: (x – min(x)) / (max(x) – min(x))
                dbNValue = (startValue - stats.Minimum) / (stats.Maximum - stats.Minimum);
                dbNValue = CalculatorHelpers.CheckForNaNandRound4(dbNValue);
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.logistic.ToString())
            {
                //logistic: 1 / (1 + exp(-x))
                dbNValue = MathNet.Numerics.SpecialFunctions.Logistic(startValue);
                dbNValue = CalculatorHelpers.CheckForNaNandRound4(dbNValue);
                //or
                //siIndex[x] = 1 / (1 + Math.Exp(-siIndex[x]));
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.logit.ToString())
            {
                //logit: inverese of logistic for y between 0 and 1
                //this assumes x is actually y 
                dbNValue = MathNet.Numerics.SpecialFunctions.Logit(startValue);
                dbNValue = CalculatorHelpers.CheckForNaNandRound4(dbNValue);
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.tanh.ToString())
            {
                //hyperbolic tangent
                dbNValue = MathNet.Numerics.Trig.Tanh(startValue);
                dbNValue = CalculatorHelpers.CheckForNaNandRound4(dbNValue);
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.pnorm.ToString())
            {
                //pnorm is for complete vectors in next function
            }
            else
            {
                //indicator 2 in drr1 (p and q, not norm and index)
            }
            return dbNValue;
        }
        public static Vector<double> GetNormalizedVector(
            string subIndNormType, double startValue,
            bool scaleUp4Digits, double[] subIndicatorData)
        {
            //normalize them
            var stats = new MathNet.Numerics.Statistics.DescriptiveStatistics(subIndicatorData);
            Vector<double> siIndex = Vector<double>.Build.Dense(subIndicatorData);
            if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.none.ToString()
                || string.IsNullOrEmpty(subIndNormType))
            {
                //data has already been normalized
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.zscore.ToString())
            {
                //z-score: (x – mean(x)) / stddev(x)
                for (int x = 0; x < siIndex.Count; x++)
                {
                    siIndex[x] = (siIndex[x] - stats.Mean) / stats.StandardDeviation;
                    if (scaleUp4Digits)
                    {
                        //scale the 4 digits by multiplying by 10,000
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                    else
                    {
                        siIndex[x] = CalculatorHelpers.CheckForNaNandRound4(siIndex[x]);
                    }
                }
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.minmax.ToString())
            {
                //min-max: (x – min(x)) / (max(x) – min(x))
                for (int x = 0; x < siIndex.Count; x++)
                {
                    siIndex[x] = (siIndex[x] - stats.Minimum) / (stats.Maximum - stats.Minimum);
                    if (scaleUp4Digits)
                    {
                        //scale the 4 digits by multiplying by 10,000
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                    else
                    {
                        siIndex[x] = CalculatorHelpers.CheckForNaNandRound4(siIndex[x]);
                    }
                }
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.logistic.ToString())
            {
                for (int x = 0; x < siIndex.Count; x++)
                {
                    //logistic: 1 / (1 + exp(-x))
                    siIndex[x] = MathNet.Numerics.SpecialFunctions.Logistic(siIndex[x]);
                    //or
                    //siIndex[x] = 1 / (1 + Math.Exp(-siIndex[x]));
                    if (scaleUp4Digits)
                    {
                        //scale the 4 digits by multiplying by 10,000
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                    else
                    {
                        siIndex[x] = CalculatorHelpers.CheckForNaNandRound4(siIndex[x]);
                    }
                }
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.logit.ToString())
            {
                for (int x = 0; x < siIndex.Count; x++)
                {
                    //logit: inverese of logistic for y between 0 and 1
                    //this assumes x is actually y 
                    siIndex[x] = MathNet.Numerics.SpecialFunctions.Logit(siIndex[x]);
                    if (scaleUp4Digits)
                    {
                        //scale the 4 digits by multiplying by 10,000
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                    else
                    {
                        siIndex[x] = CalculatorHelpers.CheckForNaNandRound4(siIndex[x]);
                    }
                }
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.tanh.ToString())
            {
                for (int x = 0; x < siIndex.Count; x++)
                {
                    //hyperbolic tangent
                    siIndex[x] = MathNet.Numerics.Trig.Tanh(siIndex[x]);
                    if (scaleUp4Digits)
                    {
                        //scale the 4 digits by multiplying by 10,000
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                    else
                    {
                        siIndex[x] = CalculatorHelpers.CheckForNaNandRound4(siIndex[x]);
                    }
                }
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.pnorm.ToString())
            {
                //p value for ttest with n-1
                double pValue = Shared.GetPValueForTDist(siIndex.Count() - 1,
                    startValue, stats.Mean, stats.Variance);
                pValue = CalculatorHelpers.CheckForNaNandRound4(pValue);
                //p norm
                siIndex = siIndex.Normalize(pValue);
                if (scaleUp4Digits)
                {
                    //scale the 4 digits by multiplying by 10,000
                    for (int x = 0; x < siIndex.Count; x++)
                    {
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                }
            }
            else if (subIndNormType == CalculatorHelpers.NORMALIZATION_TYPES.weights.ToString())
            {
                for (int x = 0; x < siIndex.Count; x++)
                {
                    //rand 2016 technique
                    siIndex[x] = siIndex[x] / startValue;
                    if (scaleUp4Digits)
                    {
                        //scale the 4 digits by multiplying by 10,000
                        siIndex[x] = siIndex[x] * 10000.00;
                        siIndex[x] = Math.Round(siIndex[x], 2);
                    }
                    else
                    {
                        siIndex[x] = CalculatorHelpers.CheckForNaNandRound4(siIndex[x]);
                    }
                }
            }
            else
            {
                //indicator 2 in drr1 (p and q, not norm and index)
            }
            //add them to parent cat index
            return siIndex;
        }
        public static string GetStringValue(List<List<double>> trends, int rowIndex, int colIndex)
        {
            string sValue = string.Empty;
            if (trends.Count > rowIndex)
            {
                if (trends[rowIndex].Count > colIndex)
                {
                    sValue = trends[rowIndex]
                        .ElementAt(colIndex).ToString("F4", CultureInfo.InvariantCulture);
                }
            }
            return sValue;
        }
        public static double GetDoubleValue(List<List<double>> trends, int rowIndex, int colIndex)
        {
            double dbValue = 0;
            if (trends.Count > rowIndex)
            {
                if (trends[rowIndex].Count > colIndex)
                {
                    dbValue = trends[rowIndex]
                        .ElementAt(colIndex);
                }
            }
            return dbValue;
        }
        public static double GetDoubleValue(string[] strArray, int colIndex)
        {
            double dbValue = 0;
            if (strArray == null)
                strArray = new string[colIndex];
            if (strArray.Count() > colIndex)
            {
                dbValue = CalculatorHelpers
                    .ConvertStringToDouble(strArray[colIndex]);
            }
            return dbValue;
        }
        public static List<List<double>> GetNormalizedandWeightedLists(
            string subIndNormType, double startValue, bool scaleUp4Digits, 
            List<double> weights, List<List<double>> trends)
        {
            List<List<double>> normTrends = new List<List<double>>();
            int i = 0;
            for (i = 0; i < trends[0].Count; i++)
            {
                //the columns are being normalized
                double[] colArray = GetDoubleArrayColumn(i, trends);
                Vector<double> normTrend = Shared.GetNormalizedVector(subIndNormType,
                    weights.Sum(), scaleUp4Digits, colArray);
                normTrends.Add(normTrend.ToList());
            }
            //but the normalized columns have to be returned back to the original rows in trends
            //and weighted
            List<List<double>> normTrends2 = new List<List<double>>();
            i = 0;
            for (i = 0; i < normTrends[0].Count; i++)
            {
                double[] colArray = GetDoubleArrayColumn(i, normTrends);
                List<double> normRow = new List<double>();
                //weighting is transitive math, shouldn't matter if before or after normaliz
                for (int j = 0; j < colArray.Count(); j++)
                {
                    if (weights.Count > i)
                    {
                        normRow.Add(colArray[j] * weights[i]);
                    }
                }
                normTrends2.Add(normRow);
            }
            return normTrends2;
        }
        public static double GetDiscountedTotal(string discountType, 
            double price, double quantity,
            double life, double realRate, double nominalRate, 
            double escalationRate, double times, double planningYear, 
            double serviceYears, double yearFromBase)
        {
            double dbDiscTotal = price * quantity;
            GeneralRules.GROWTH_SERIES_TYPES eGrowthType = GeneralRules.GetGrowthType(discountType);
            double dbSalvVal = 0;
            if (realRate > 0)
                realRate = realRate / 100;
            if (nominalRate > 0)
                nominalRate = nominalRate / 100;
            dbDiscTotal = GeneralRules.GetGradientRealDiscountValue(dbDiscTotal,
                realRate, serviceYears, yearFromBase,
                planningYear, eGrowthType, escalationRate,
                escalationRate, life,  times, dbSalvVal);
            return dbDiscTotal;
        }
        public static double GetDiscountedAmount(double initialAmount, double life, double rate)
        {
            double dbDiscAmount = initialAmount;
            dbDiscAmount = initialAmount * (1 / Math.Pow((1 + rate), life));
            return dbDiscAmount;
        }
        public static double GetUPV(double seriesAmount, double life, double rate)
        {
            double dbUPV = seriesAmount;
            dbUPV = seriesAmount * ((Math.Pow(1 + rate, life) - 1) / (rate * (Math.Pow((1 + rate), life))));
            return dbUPV;
        }
        public static double GetAvgAmortizedAmount(double amount, 
            double life, double rate)
        {
            double dbAAC = amount;
            //(amount / ((1 - (1 + rate) ^ (-1 * life)) / rate))
            int iPower = (int)(-1 * life);
            double dbAvgAnnFactor = (1 - Math.Pow((1 + rate), iPower)) / rate;
            dbAAC = amount / dbAvgAnnFactor;
            return dbAAC;
        }
    }
//infernet deprecated in favor of azure machine learning
    //using MicrosoftResearch.Infer;
//using MicrosoftResearch.Infer.Models;
//using MicrosoftResearch.Infer.Maths;
    //public static VariableArray2D<double> GetDoubleVariableArray2D(List<List<double>> data, string[] colNames,
    //       string[] depColNames)
    //    {
    //        //convert data to a Math.Net Matrix
    //        //positive definite matrix, or square 
    //        Range N = new Range(data.Count()).Named("N");
    //        Range T = new Range(depColNames.Count()).Named("T");
    //        Matrix<double> M = GetDoubleMatrix(data, colNames, depColNames);
    //        VariableArray2D<double> m = Variable.Observed<double>(M.ToArray()).Named("x");
    //        return m;
    //    }
    //    public static VariableArray2D<double> GetDoubleVariableArray2D(List<List<string>> data, string[] colNames,
    //        string[] depColNames)
    //    {
    //        //convert data to a Math.Net Matrix
    //        //positive definite matrix, or square 
    //        Range N = new Range(data.Count()).Named("N");
    //        Range T = new Range(depColNames.Count()).Named("T");
    //        Matrix<double> M = GetDoubleMatrix(data, colNames, depColNames);
    //        VariableArray2D<double> m = Variable.Observed<double>(M.ToArray()).Named("x");
    //        return m;
    //    }
    //    public static Vector[] GetDoubleVector(List<List<string>> data, string[] colNames,
    //        string[] depColNames)
    //    {
    //        //convert data to a Math.Net Matrix
    //        //positive definite matrix, or square 
    //        Range N = new Range(data.Count()).Named("N");
    //        Range T = new Range(depColNames.Count()).Named("T");
    //        Matrix<double> M = GetDoubleMatrix(data, colNames, depColNames);
    //        Vector[] x = new Vector[M.RowCount];
    //        for (int i = 0; i < M.RowCount; i++)
    //        {
    //            x[i] = Vector.FromList(M.Row(i).ToArray());
    //        }
    //        //Vector[] x = new Vector[M.ColumnCount];
    //        //for (int i = 0; i < M.ColumnCount; i++)
    //        //{
    //        //    x[i] = Vector.FromList(M.Column(i).ToArray());
    //        //}
    //        return x;
    //    }
    //    public static Vector[] GetDoubleVector(string csvFileName)
    //    {
    //        List<Vector> dataList = new List<Vector>();
    //        using (StreamReader sr = new StreamReader(csvFileName))
    //        {
    //            string str;
    //            while ((str = sr.ReadLine()) != null)
    //            {
    //                double[] arr = str.Split(',').Select(s => double.Parse(s)).ToArray();
    //                dataList.Add(Vector.FromArray(arr));
    //            }
    //        }
    //        Vector[] data = dataList.ToArray();
    //        return data;
    //    }
    //    public static VariableArray<double> GetYDataForVariableArray(List<List<double>> data)
    //    {
    //        Range N = new Range(data.Count()).Named("N");
    //        Vector<double> Y = GetYData(data);
    //        VariableArray<double> y = Variable.Observed<double>(Y.ToArray(), N).Named("y");
    //        return y;
    //    }
    //    public static VariableArray<double> GetYDataForVariableArray(List<List<string>> data)
    //    {
    //        Range N = new Range(data.Count()).Named("N");
    //        Vector<double> Y = GetYData(data);
    //        VariableArray<double> y = Variable.Observed<double>(Y.ToArray(), N).Named("y");
    //        return y;
    //    }
}
