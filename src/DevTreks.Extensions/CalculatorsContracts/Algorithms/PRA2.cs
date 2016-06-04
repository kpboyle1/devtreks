using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using MathNet.Numerics.Distributions;
//probab risk
using MathNet.Numerics.LinearAlgebra;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Probabilistic Risk Assessment algorithm 2
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA algo1 subalgo1,2,3,4 with JDataURL correlations
    ///</summary>
    public class PRA2 : PRA1
    {
        public PRA2(int ciLevel, int iterations,
           int random, IndicatorQT1 qT1, CalculatorParameters calcParams)
            : base(ciLevel, iterations, random, qT1, calcParams)
        {
            //base uses the parameters
            //qT1.qt1s correspond to Score plus 20 indicators
        }
        //has to be passed back to calling procedure
        public Matrix<double> RandomSampleData { get; set; }


        public async Task<string> RunAlgorithmAsync(string jdataURL, IDictionary<string, List<List<double>>> data)
        {
            string sIndicatorsCSV = string.Empty;
            bool bHasCalcs = false;
            string sError = string.Empty;
            List<string> lines = CalculatorHelpers.ReadLines(Params.ExtensionDocToCalcURI, jdataURL, out sError);
            //async returns too fast - the xml is written before calcs have been added
            //List<string> lines = await CalculatorHelpers.ReadLinesAsync(jointDataURL);
            if (lines == null)
            {
                //first qt1 is always score
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                return null;
            }
            if (lines.Count == 0)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                return null;
            }
            if (!string.IsNullOrEmpty(sError))
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += sError;
                return null;
            }
            //first line: get correlationtype
            MathHelpers.CORRELATION_TYPES eCType = GetCorrelationType(lines);
            //second line: indicators (the number of indicators is the rank of the correlation matrix
            string[] indicators = GetIndicatorLabels(lines, 1);
            if (indicators.Count() == 0)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_NOCINDICATORS"));
                return null;
            }
            //rank of matrix is indicators.Count, start with third line down
            var jointData = GetMatrix(lines, indicators.Count(), 0, 2);
            if (jointData == null)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADFORMAT"));
                return null;
            }
            //check for consistent correlation matrix
            if ((jointData.ColumnCount != jointData.RowCount)
                || (jointData.ColumnCount != indicators.Count()))
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADFORMAT"));
                return null;
            }
            //see if there is a corresponding dataset
            //this requires a 1 to 1 relation between dataurls and jointdataurls
            //IDictionary<string, List<List<double>>> data = await GetDataURLAsync(dataIndex);
            //if joint data has all zeros, needs calculated correlation matrix
            jointData = GetCorrelationMatrix(eCType, jointData, indicators, data);
            if (jointData == null)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCOR"));
                return null;
            }
            if (jointData.RowCount == 0)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCOR"));
                return null;
            }
            var sampleData = GetSampleData(indicators);
            if (sampleData == null)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("SAMPLES_BADDIST"));
                return null;
            }
            if (sampleData.RowCount == 0)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("SAMPLES_BADDIST"));
                return null;
            }
            var randomSampleData = GetRandomSample(jointData, sampleData, indicators);
            if (randomSampleData == null)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("SAMPLES_BADRND"));
                return null;
            }
            if (randomSampleData.RowCount == 0)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("SAMPLES_BADRND"));
                return null;
            }
            //set the descriptive statistics
            bHasCalcs = SetSampleCalculation(indicators, randomSampleData);
            //store the correlation matrix for the random sample data (for comparison to actual)
            SetCorrelationMatrix(eCType, jointData, randomSampleData);
            //new in 1.9.0
            this.RandomSampleData = randomSampleData;
            //version 1.9.0 had to move this to SB1Algos (can't access SB1Base from here)
            //if (this.IndicatorQT.IndicatorQT1s[0].QTD1 == 0 && this.IndicatorQT.IndicatorQT1s[0].QTD2 == 0)
            //{
            //    //use the randomsample data to generate Score, ScoreM, ScoreL, and ScoreU
            //    indicators = SetScoresFromRandomSamples(indicators, randomSampleData);
            //}
            //this was kept here but can be replaced by SetScoresFromRS
            foreach (var indicator in indicators)
            {
                if (!string.IsNullOrEmpty(indicator))
                {
                    sIndicatorsCSV += string.Concat(indicator, Constants.CSV_DELIMITER);
                }
            }
            //remove the last delimiter
            sIndicatorsCSV = sIndicatorsCSV.Remove(sIndicatorsCSV.Length - 1, 1);
            return sIndicatorsCSV;
        }
        
        private Matrix<double> GetMatrix(List<string> csvlines,
            int indicatorsRank, int skipCols, int skipRows)
        {
            //init matrix
            Matrix<double> m = Matrix<double>.Build.Dense(indicatorsRank, indicatorsRank);
            if (csvlines.Count > 0)
            {
                //generate an enumerable collection of q1 to q5s (rows and cols)
                IEnumerable<IEnumerable<double>> qryQs =
                    from line in csvlines
                    let elements = line.Split(Constants.CSV_DELIMITERS)
                    //skip label and date columns
                    let amounts = elements.Skip(skipCols)
                    select (from a in amounts
                            select CalculatorHelpers.ConvertStringToDouble(a));
                //could also use
                //var row = (from Match match in matches where match.Length > 0 select match.Value).ToArray();
                var qs = qryQs.ToList();
                if (qs.Count > 0)
                {
                    int i = 0;
                    //ds is a List<double> of indicators for each ds.Key = indicator.Label
                    foreach (var q in qs)
                    {
                        if (i >= skipRows)
                        {
                            if (i == skipRows)
                            {
                                //clear matrix and rebuild
                                m.Clear();
                                m = Matrix<double>.Build.Dense(csvlines.Count() - skipRows, q.Count());
                            }
                            //start with col[0]
                            m.SetColumn(i - skipRows, q.ToArray());
                        }
                        i++;
                    }
                }
            }
            return m;
        }
        private Matrix<double> GetRandomSample(Matrix<double> jointData, Matrix<double> sampleData,
            string[] indicators)
        {
            if (this.Iterations == 0)
                this.Iterations = 1000;
            int cols = indicators.Count();
            //results
            Matrix<double> randomSampleData = Matrix<double>.Build.Dense(this.Iterations, cols);
            //random normal standards
            //var v = Vector<double>.Build.Random(this.Iterations, new ContinuousUniform());
            if (HasMathType(MATH_TYPES.algorithm1.ToString(), MATH_SUBTYPES.subalgorithm2.ToString(), indicators))
            {
                //normal copula with cholesy decomposition
                var A = jointData.Cholesky();
                if (A == null)
                {
                    this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCHOL"));
                    return null;
                }
                //random normal standards
                Matrix<double> u;
                if (this.Random != 0)
                {
                    u = Matrix<double>.Build.Random(this.Iterations, cols, this.Random);
                }
                else
                {
                    u = Matrix<double>.Build.Random(this.Iterations, cols);
                }
                //some copulas use random uniforms
                //var u = Matrix<double>.Build.Random(this.Iterations, cols, new ContinuousUniform());
                //generate correlated randoms by multiplying both matrixes together
                var X = u.Multiply(A.Factor);
                //random sample vector n = inverse cumulative density function for Xn
                SetCorrelatedRandomSamples(X, sampleData, randomSampleData);
            }
            else if (HasMathType(MATH_TYPES.algorithm1.ToString(), MATH_SUBTYPES.subalgorithm3.ToString(), indicators))
            {
                //eigenvalue decomposition
                //this is the correlation matrix (order is the number of rows and columns which must be equal)
                var evd = jointData.Evd();
                if (evd == null)
                {
                    this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADEIGEN"));
                    return null;
                }
                //take the square root of the diagonal eigenvalue matrix
                Matrix<double> squarerootEigenValues = evd.D.PointwisePower(0.5);
                //multiply the eigenvalues square root matrix by the eigenvectors diagonal matrix
                var v = evd.EigenVectors.Multiply(squarerootEigenValues);
                //random normal standards
                Matrix<double> u;
                if (this.Random != 0)
                {
                    u = Matrix<double>.Build.Random(this.Iterations, cols, this.Random);
                }
                else
                {
                    u = Matrix<double>.Build.Random(this.Iterations, cols);
                }
                //generate correlated normal randoms by multiplying both matrixes together
                var X = u.TransposeAndMultiply(v);
                //random sample vector n = inverse of cumulative density function for Xn
                SetCorrelatedRandomSamples(X, sampleData, randomSampleData);
            }
            else if (HasMathType(MATH_TYPES.algorithm1.ToString(), MATH_SUBTYPES.subalgorithm4.ToString(), indicators))
            {
                //eigenvalue decomposition
                //this is the correlation matrix (order is the number of rows and columns which must be equal)
                var evd = jointData.Evd();
                if (evd == null)
                {
                    this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADEIGEN"));
                    return null;
                }
                //take the square root of the diagonal eigenvalue matrix
                Matrix<double> squarerootEigenValues = evd.D.PointwisePower(0.5);
                //multiply the eigenvalues square root matrix by the eigenvectors diagonal matrix
                var v = evd.EigenVectors.Multiply(squarerootEigenValues);
                //only change from algo 3: random uniforms
                var u = Matrix<double>.Build.Random(this.Iterations, cols, new ContinuousUniform());
                //generate correlated normal randoms by multiplying both matrixes together
                var X = u.TransposeAndMultiply(v);
                //random sample vector n = inverse of cumulative density function for Xn
                SetCorrelatedRandomSamples(X, sampleData, randomSampleData);
            }
            else if (HasMathType(MATH_TYPES.algorithm1.ToString(), MATH_SUBTYPES.subalgorithm5.ToString(), indicators))
            {
                //LU copula -testing shows it works
                var A = jointData.LU();
                if (A == null)
                {
                    this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCHOL"));
                    return null;
                }
                if (A == null)
                {
                    this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCHOL"));
                    return null;
                }
                //random uniforms
                var u = Matrix<double>.Build.Random(this.Iterations, cols, new ContinuousUniform());
                //generate correlated randoms by multiplying both matrixes together
                var X = u.Multiply(A.L);
                //random sample vector n = inverse cumulative density function for Xn
                SetCorrelatedRandomSamples(X, sampleData, randomSampleData);
            }
            return randomSampleData;
        }
        public bool HasMathType(string algorithm, string subAlgorithm, string[] indicators)
        {
            bool bHasMathType = false;
            //the score does not have an indicator label
            if (this.IndicatorQT.IndicatorQT1s[0].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[0].QMathSubType == subAlgorithm)
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[1].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[1].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[1].Label))
            {
                //but the rest do
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[2].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[2].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[2].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[3].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[3].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[3].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[4].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[4].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[4].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[5].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[5].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[5].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[6].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[6].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[6].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[7].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[7].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[7].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[8].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[8].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[8].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[9].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[9].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[9].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[10].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[10].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[10].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[11].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[11].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[11].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[12].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[12].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[12].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[13].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[13].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[13].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[14].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[14].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[14].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[15].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[15].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[15].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[16].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[16].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[16].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[17].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[17].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[17].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[18].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[18].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[18].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[19].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[19].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[19].Label))
            {
                return true;
            }
            else if (this.IndicatorQT.IndicatorQT1s[20].QMathType == algorithm
                && this.IndicatorQT.IndicatorQT1s[20].QMathSubType == subAlgorithm
                && indicators.Contains(this.IndicatorQT.IndicatorQT1s[20].Label))
            {
                return true;
            }
            return bHasMathType;
        }
        private void SetCorrelatedRandomSamples(Matrix<double> x, Matrix<double> sampleData,
            Matrix<double> randomSampleData)
        {
            //generate random correlated samples by taking inverse of T
            for (int i = 0; i < x.ColumnCount; i++)
            {
                //determine probability of each normal variable in x.Column(i) by using
                //CDF transformation
                var xnstats = new MathNet.Numerics.Statistics.DescriptiveStatistics(x.Column(i));
                //use an inverse transform of sampledata
                var sstats = new MathNet.Numerics.Statistics.DescriptiveStatistics(sampleData.Column(i));
                double[] R = new double[this.Iterations];
                int j = 0;
                foreach (var n in x.Column(i))
                {
                    //determine probability of each normal variable in x.Column(i) by using
                    //CDF transformation
                    var p = Normal.CDF(xnstats.Mean, xnstats.StandardDeviation, n);
                    //now use the probabilty to get the inverse normal cdf of sampled distribution
                    R[j] = Normal.InvCDF(sstats.Mean, sstats.StandardDeviation, p);
                    j++;
                }
                randomSampleData.SetColumn(i, R);
            }
        }
        private void SetCorrelatedRandomSamples2(Matrix<double> x, Matrix<double> sampleData,
            Matrix<double> randomSampleData)
        {
            //generate random correlated samples by taking inverse of T
            for (int i = 0; i < x.ColumnCount; i++)
            {
                //generate a vector of uniform randoms 
                var v = Vector<double>.Build.Random(this.Iterations, new ContinuousUniform());
                int j = 0;
                foreach (var u in x.Column(i))
                {
                    //set the inverse of uniform vars with N(0,1) distribution (equiv to NORMINV(rand()) in Excel)
                    v[j] = Normal.InvCDF(0, 1, u);
                    j++;
                }
                //generate a vector of random correlated samples by multiplying each vector in x * xv
                var xv = x.Column(i).PointwiseMultiply(v);
                //add the correlate random vector to a matrix
                randomSampleData.SetColumn(i, xv);
            }
        }
        private MathHelpers.CORRELATION_TYPES GetCorrelationType(List<string> lines)
        {
            MathHelpers.CORRELATION_TYPES eCType = MathHelpers.CORRELATION_TYPES.pearson;
            string sCType = lines[0];
            if (sCType.ToLower().Contains(MathHelpers.CORRELATION_TYPES.spearman.ToString()))
            {
                eCType = MathHelpers.CORRELATION_TYPES.spearman;
            }
            //else return default pearson
            return eCType;
        }
        public string[] GetIndicatorLabels(List<string> lines, int rowZeroIndex)
        {
            string sCType = string.Empty;
            if (lines.Count >= (rowZeroIndex + 1))
            {
                sCType = lines[rowZeroIndex];
            }
            string[] indsArr = sCType.Split(Constants.CSV_DELIMITERS);
            return indsArr;
        }
        private Matrix<double> GetCorrelationMatrix(MathHelpers.CORRELATION_TYPES cType, Matrix<double> jData,
            string[] indicators, IDictionary<string, List<List<double>>> data)
        {
            //see if the joint data is a zeros matrix
            if (jData == null)
                return jData;
            Vector<double> col0s = jData.Column(0);
            if (col0s == null)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCOR"));
                return null;
            }
            //need at least 2 correlations??
            if (col0s.Count < 2)
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCOR"));
                return null;
            }
            bool bIsZeroMatrix = IsZeroMatrix(col0s);
            if (!bIsZeroMatrix)
            {
                //its a real correlation matrix
                return jData;
            }
            List<double[]> vectors = new List<double[]>();
            Matrix<double> jointData = jData;
            //ds is a List<double> of indicators for each ds.Key = indicator.Label
            int i = 0;
            int iRowCount = 0;
            //the correlations are based on QT, run the MathExpression and generate the QT cols in all datasets
            foreach (var ds in data)
            {
                bool bIsCorrelatedInd = indicators.Any(o => o.Equals(ds.Key));
                if (bIsCorrelatedInd)
                {
                    //get the qTs for each indicator in data
                    if (!(ds.Value.Count > 0))
                    {
                        this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCORDATA"));
                        return null;
                    }
                    var qTs = from row in ds.Value select row.ElementAt(0);
                    if (qTs == null)
                    {
                        this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCORDATA"));
                        return null;
                    }
                    if (i == 0)
                    {
                        //set with first indicator
                        iRowCount = qTs.Count();
                    }
                    //must be at least 3 observations and must have symetric observation matrix
                    if (iRowCount < 3
                        || iRowCount != qTs.Count())
                    {
                        this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCORDATA"));
                        return null;
                    }
                    //add each column to a matrix
                    vectors.Add(qTs.ToArray());
                    i++;
                }
            }
            jointData.Clear();
            //calculate the correlation matrix
            if (cType == MathHelpers.CORRELATION_TYPES.spearman)
            {
                jointData = MathNet.Numerics.Statistics.Correlation.SpearmanMatrix(vectors);
            }
            else
            {
                //linear correlation matrix
                jointData = MathNet.Numerics.Statistics.Correlation.PearsonMatrix(vectors);
            }
            return jointData;
        }
        private void SetCorrelationMatrix(MathHelpers.CORRELATION_TYPES cType, Matrix<double> jointData, Matrix<double> randomSampleData)
        {
            //remove the original after testing is complete -too much data in MathResult
            //save the original correlation matrix
            StringBuilder str = new StringBuilder();
            str.AppendLine(string.Concat("original ", cType.ToString()));
            str.Append(jointData.ToString());
            List<double[]> vectors = new List<double[]>();
            for (int i = 0; i < randomSampleData.ColumnCount; i++)
            {
                var vectori = randomSampleData.Column(i);
                vectors.Add(vectori.ToArray());
            }
            if (vectors.Count > 0)
            {
                //calculate the correlation matrix
                if (cType == MathHelpers.CORRELATION_TYPES.spearman)
                {
                    var corRSD = MathNet.Numerics.Statistics.Correlation.SpearmanMatrix(vectors);
                    str.AppendLine(string.Concat("new ", cType.ToString()));
                    str.Append(corRSD.ToString());
                }
                else
                {
                    //linear correlation matrix
                    var corRSD = MathNet.Numerics.Statistics.Correlation.PearsonMatrix(vectors);
                    str.AppendLine(string.Concat("new ", cType.ToString()));
                    str.Append(corRSD.ToString());
                }
                str.AppendLine(string.Empty);
                this.IndicatorQT.IndicatorQT1s[0].MathResult += str.ToString();
            }
            else
            {
                this.IndicatorQT.IndicatorQT1s[0].MathResult += string.Concat(" ", Errors.MakeStandardErrorMsg("MATRIX_BADCOR"));
            }
        }
        private bool IsZeroMatrix(IEnumerable<double> q0s)
        {
            bool bIsZeroMatrix = false;
            if (q0s != null)
            {
                //if the correlation matrix does not start with a 1, but does start with a zero, it is not a real correlation matrix
                if (q0s.First() == 0)
                {
                    bIsZeroMatrix = true;
                }
            }
            return bIsZeroMatrix;
        }
        
        private Matrix<double> GetSampleData(string[] indicators)
        {
            //generate the matrix using iterations for rows and indicators for cols
            Matrix<double> sampleData = Matrix<double>.Build.Dense(this.Iterations, indicators.Count());
            int i = 0;
            //ds is a List<double> of indicators for each ds.Key = indicator.Label
            foreach (var ind in indicators)
            {
                //get the qTs for each indicator in data
                double[] sampleVector = GetSampleData(ind);
                if (sampleVector != null)
                {
                    if (sampleVector.Count() > 0)
                    {
                        //add each column to a matrix
                        sampleData.SetColumn(i, sampleVector);
                        i++;
                    }
                }
            }
            return sampleData;
        }
        private double[] GetSampleData(string label)
        {
            double[] sampledata = { };
            //skip the score in idex 0
            if (label == this.IndicatorQT.IndicatorQT1s[1].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[1].QDistributionType, 
                    this.IndicatorQT.IndicatorQT1s[1].QT, this.IndicatorQT.IndicatorQT1s[1].QTD1, 
                    this.IndicatorQT.IndicatorQT1s[1].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[2].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[2].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[2].QT, this.IndicatorQT.IndicatorQT1s[2].QTD1,
                    this.IndicatorQT.IndicatorQT1s[2].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[3].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[3].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[3].QT, this.IndicatorQT.IndicatorQT1s[3].QTD1,
                    this.IndicatorQT.IndicatorQT1s[3].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[4].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[4].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[4].QT, this.IndicatorQT.IndicatorQT1s[4].QTD1,
                    this.IndicatorQT.IndicatorQT1s[4].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[5].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[5].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[5].QT, this.IndicatorQT.IndicatorQT1s[5].QTD1,
                    this.IndicatorQT.IndicatorQT1s[5].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[6].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[6].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[6].QT, this.IndicatorQT.IndicatorQT1s[6].QTD1,
                    this.IndicatorQT.IndicatorQT1s[6].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[7].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[7].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[7].QT, this.IndicatorQT.IndicatorQT1s[7].QTD1,
                    this.IndicatorQT.IndicatorQT1s[7].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[8].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[8].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[8].QT, this.IndicatorQT.IndicatorQT1s[8].QTD1,
                    this.IndicatorQT.IndicatorQT1s[8].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[9].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[9].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[9].QT, this.IndicatorQT.IndicatorQT1s[9].QTD1,
                    this.IndicatorQT.IndicatorQT1s[9].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[10].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[10].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[10].QT, this.IndicatorQT.IndicatorQT1s[10].QTD1,
                    this.IndicatorQT.IndicatorQT1s[10].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[11].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[11].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[11].QT, this.IndicatorQT.IndicatorQT1s[11].QTD1,
                    this.IndicatorQT.IndicatorQT1s[11].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[12].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[12].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[12].QT, this.IndicatorQT.IndicatorQT1s[12].QTD1,
                    this.IndicatorQT.IndicatorQT1s[12].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[13].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[13].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[13].QT, this.IndicatorQT.IndicatorQT1s[13].QTD1,
                    this.IndicatorQT.IndicatorQT1s[13].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[14].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[14].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[14].QT, this.IndicatorQT.IndicatorQT1s[14].QTD1,
                    this.IndicatorQT.IndicatorQT1s[14].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[15].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[15].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[15].QT, this.IndicatorQT.IndicatorQT1s[15].QTD1,
                    this.IndicatorQT.IndicatorQT1s[15].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[16].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[16].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[16].QT, this.IndicatorQT.IndicatorQT1s[16].QTD1,
                    this.IndicatorQT.IndicatorQT1s[16].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[17].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[17].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[17].QT, this.IndicatorQT.IndicatorQT1s[17].QTD1,
                    this.IndicatorQT.IndicatorQT1s[17].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[18].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[18].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[18].QT, this.IndicatorQT.IndicatorQT1s[18].QTD1,
                    this.IndicatorQT.IndicatorQT1s[18].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[19].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[19].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[19].QT, this.IndicatorQT.IndicatorQT1s[19].QTD1,
                    this.IndicatorQT.IndicatorQT1s[19].QTD2);
            }
            else if (label == this.IndicatorQT.IndicatorQT1s[20].Label)
            {
                sampledata = this.GetSampleData(this.IndicatorQT.IndicatorQT1s[20].QDistributionType,
                    this.IndicatorQT.IndicatorQT1s[20].QT, this.IndicatorQT.IndicatorQT1s[20].QTD1,
                    this.IndicatorQT.IndicatorQT1s[20].QTD2);
            }
            else
            {
                //ignore the row
            }
            return sampledata;
        }
        private bool SetSampleCalculation(string[] inds, Matrix<double> randomSampleData)
        {
            bool bHasCalcs = false;
            int i = 0;
            foreach (var label in inds)
            {
                //skip the score
                if (label == this.IndicatorQT.IndicatorQT1s[1].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        //label came form the same indicators as randomSample
                        var col = randomSampleData.Column(i);
                        //the math goes into this.IndicatorQT
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[1].QT, this.IndicatorQT.IndicatorQT1s[1].QTD1,
                            this.IndicatorQT.IndicatorQT1s[1].QTD2, col.ToArray());
                        //now copy the math results back (but don't overwrite the q1s entered in the ui)
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[1], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[2].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[2].QT, this.IndicatorQT.IndicatorQT1s[2].QTD1,
                            this.IndicatorQT.IndicatorQT1s[2].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[2], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[3].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[3].QT, this.IndicatorQT.IndicatorQT1s[3].QTD1,
                            this.IndicatorQT.IndicatorQT1s[3].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[3], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[4].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[4].QT, this.IndicatorQT.IndicatorQT1s[4].QTD1,
                            this.IndicatorQT.IndicatorQT1s[4].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[4], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[5].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[5].QT, this.IndicatorQT.IndicatorQT1s[5].QTD1,
                            this.IndicatorQT.IndicatorQT1s[5].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[5], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[6].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[6].QT, this.IndicatorQT.IndicatorQT1s[6].QTD1,
                            this.IndicatorQT.IndicatorQT1s[6].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[6], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[7].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[7].QT, this.IndicatorQT.IndicatorQT1s[7].QTD1,
                            this.IndicatorQT.IndicatorQT1s[7].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[7], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[8].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[8].QT, this.IndicatorQT.IndicatorQT1s[8].QTD1,
                            this.IndicatorQT.IndicatorQT1s[8].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[8], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[9].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[9].QT, this.IndicatorQT.IndicatorQT1s[9].QTD1,
                            this.IndicatorQT.IndicatorQT1s[9].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[9], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[10].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[10].QT, this.IndicatorQT.IndicatorQT1s[10].QTD1,
                            this.IndicatorQT.IndicatorQT1s[10].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[10], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[11].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[11].QT, this.IndicatorQT.IndicatorQT1s[11].QTD1,
                            this.IndicatorQT.IndicatorQT1s[11].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[11], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[12].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[12].QT, this.IndicatorQT.IndicatorQT1s[12].QTD1,
                            this.IndicatorQT.IndicatorQT1s[12].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[12], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[13].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[13].QT, this.IndicatorQT.IndicatorQT1s[13].QTD1,
                            this.IndicatorQT.IndicatorQT1s[13].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[13], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[14].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[14].QT, this.IndicatorQT.IndicatorQT1s[14].QTD1,
                            this.IndicatorQT.IndicatorQT1s[14].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[14], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[15].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[15].QT, this.IndicatorQT.IndicatorQT1s[15].QTD1,
                            this.IndicatorQT.IndicatorQT1s[15].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[15], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[16].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[16].QT, this.IndicatorQT.IndicatorQT1s[16].QTD1,
                            this.IndicatorQT.IndicatorQT1s[16].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[16], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[17].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[17].QT, this.IndicatorQT.IndicatorQT1s[17].QTD1,
                            this.IndicatorQT.IndicatorQT1s[17].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[17], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[18].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[18].QT, this.IndicatorQT.IndicatorQT1s[18].QTD1,
                            this.IndicatorQT.IndicatorQT1s[18].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[18], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[19].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[19].QT, this.IndicatorQT.IndicatorQT1s[19].QTD1,
                            this.IndicatorQT.IndicatorQT1s[19].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[19], this.IndicatorQT);
                    }
                    i++;
                }
                else if (label == this.IndicatorQT.IndicatorQT1s[20].Label)
                {
                    if (randomSampleData.ColumnCount > i)
                    {
                        var col = randomSampleData.Column(i);
                        this.SetRange(this.IndicatorQT.IndicatorQT1s[20].QT, this.IndicatorQT.IndicatorQT1s[20].QTD1,
                            this.IndicatorQT.IndicatorQT1s[20].QTD2, col.ToArray());
                        this.IndicatorQT.CopyIndicatorQT1MathProperties(this.IndicatorQT.IndicatorQT1s[20], this.IndicatorQT);
                    }
                    i++;
                }
                else
                {
                    //ignore the row
                }
            }
            if (i > 0)
            {
                bHasCalcs = true;
            }
            return bHasCalcs;
        }
    }
}
