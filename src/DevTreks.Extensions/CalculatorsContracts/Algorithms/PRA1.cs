using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Errors = DevTreks.Exceptions.DevTreksErrors;
using MathNet.Numerics.Distributions;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Probabilistic Risk Assessment algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA algo1 subalgo1,2,3,4
    ///</summary>
    public class PRA1 : Calculator1
    {
        public PRA1() : base() { }
        public PRA1(string label, string[] mathTerms, string[] colNames, string[] depColNames,
            int totalsNeeded, string subalgorithm, int ciLevel, int iterations,
            int random, List<double> qTs, IndicatorQT1 qT1, CalculatorParameters calcParams)
            : base()
        {
            ColNames = colNames;
            _depColNames = depColNames;
            MathTerms = mathTerms;
            _subalgorithm = subalgorithm;
            //188 anors can run algos and pass back totals to calling procedure
            _totalsNeeded = totalsNeeded;
            Label = label;
            CILevel = ciLevel;
            Random = random;
            Iterations = iterations;
            if (qT1 == null)
                qT1 = new IndicatorQT1();
            IndicatorQT = new IndicatorQT1(qT1);
            //getobserveddata
            ObsTs = qTs.ToArray();
            Params = calcParams;
        }
        public PRA1(int ciLevel, int iterations,
           int random, IndicatorQT1 qT1, CalculatorParameters calcParams)
            : base()
        {
            CILevel = ciLevel;
            Random = random;
            Iterations = iterations;
            if (qT1 == null)
                qT1 = new IndicatorQT1();
            this.IndicatorQT = new IndicatorQT1(qT1);
            Params = calcParams;
        }
        public PRA1(PRA1 pra1)
            : base()
        {
            ColNames = pra1.ColNames;
            _depColNames = pra1._depColNames;
            MathTerms = pra1.MathTerms;
            _subalgorithm = pra1._subalgorithm;
            //188 anors can run algos and pass back totals to calling procedure
            _totalsNeeded = pra1._totalsNeeded;
            Label = pra1.Label;
            CILevel = pra1.CILevel;
            Random = pra1.Random;
            Iterations = pra1.Iterations;
            IndicatorQT = new IndicatorQT1(pra1.IndicatorQT);
            //getobserveddata
            ObsTs = pra1.ObsTs;
        }
        public CalculatorParameters Params { get; set; }
        public string[] ColNames { get; set; }
        //all of the the dependent var column names including intercept
        private string[] _depColNames { get; set; }
        //corresponding Ix.Qx names (1 less count because no dependent var)
        public string[] MathTerms { get; set; }
        //Q1 to Q5s
        private double[] _qs { get; set; }
        //which regression algorithm is being run?
        public string _subalgorithm { get; set; }
        //how many totals need to be returned to calling procedure?
        private int _totalsNeeded { get; set; }

        public int CILevel { get; set; }
        public int Iterations { get; set; }
        public int Random { get; set; }
        //used with observational TEXT datasets, rather than sampled data
        public double[] ObsTs { get; set; }
        //output
        public IndicatorQT1 IndicatorQT { get; set; }
        
        public async Task RunAlgorithmAsync()
        {
            try
            {
                if (ObsTs.Count() > 0)
                {
                    this.SetObservedMathResult(this.IndicatorQT.QMathType, this.IndicatorQT.QDistributionType,
                        this.IndicatorQT.QMathSubType, this.IndicatorQT.QT, ObsTs);
                }
                else
                {
                    var data = this.GetSampleData(this.IndicatorQT.QDistributionType,
                        this.IndicatorQT.QT, this.IndicatorQT.QTD1, this.IndicatorQT.QTD2);
                    this.SetRange(this.IndicatorQT.QT, this.IndicatorQT.QTD1, 
                        this.IndicatorQT.QTD2, data);
                }
                if (!string.IsNullOrEmpty(this.ErrorMessage))
                {
                    this.IndicatorQT.MathResult += this.ErrorMessage;
                    this.IndicatorQT.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.IndicatorQT.ErrorMessage = ex.Message;
            }
        }
        public async Task RunAlgorithmAsync(double[] data)
        {
            try
            {
                this.SetRange(this.IndicatorQT.QT, this.IndicatorQT.QTD1, 
                    this.IndicatorQT.QTD2, data);
                if (!string.IsNullOrEmpty(this.ErrorMessage))
                {
                    this.IndicatorQT.MathResult += this.ErrorMessage;
                    this.IndicatorQT.ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.IndicatorQT.ErrorMessage = ex.Message;
            }
        }
        public void SetRange(double mostLikelyEstimate, double lowEstimate, double highEstimate,
            double[] data, List<double> cdf = null)
        {
            StringBuilder sb = new StringBuilder();
            if (data == null)
            {
                sb.AppendLine("This indicator does not have the properties needed for descriptive statistics.");
            }
            if (data.Count() == 0)
            {
                sb.AppendLine("This indicator does not have the properties needed for descriptive statistics.");
            }
            //default has no range of values
            double lowRange = lowEstimate;
            double highRange = highEstimate;
            double mostLikely = mostLikelyEstimate;
            string mostLikelyUnit = string.Empty;
            string lowUnit = string.Empty;
            string highUnit = string.Empty;
            bool bNeedsCDF = false;
            var stats = new MathNet.Numerics.Statistics.DescriptiveStatistics(data);
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            //some data has all zeros and does not use sampled data
            if (stats.Mean != 0)
            {
                mostLikely = stats.Mean;
                mostLikelyUnit = "mean";
                lowRange = mostLikely - CalculatorHelpers.GetConfidenceInterval(this.CILevel, stats.Count, stats.StandardDeviation);
                lowUnit = sLowerCI;
                highRange = mostLikely + CalculatorHelpers.GetConfidenceInterval(this.CILevel, stats.Count, stats.StandardDeviation);
                highUnit = sUpperCI;
                bNeedsCDF = true;
            }

            if (bNeedsCDF)
            {
                //csv strings use f4 not n4
                sb.AppendLine(Errors.GetMessage("STATS_DESC1"));
                sb.AppendLine(Errors.GetMessage("STATS_DESC2"));
                sb.AppendLine(string.Concat(stats.Count.ToString(), ", ", data.Sum().ToString("F4", CultureInfo.InvariantCulture), ", ",
                    stats.Mean.ToString("F4", CultureInfo.InvariantCulture), ", ",
                    MathNet.Numerics.Statistics.Statistics.Median(data).ToString("F4", CultureInfo.InvariantCulture), ", ",
                    stats.StandardDeviation.ToString("F4", CultureInfo.InvariantCulture), ", ", stats.Variance.ToString("F4", CultureInfo.InvariantCulture), ", ",
                    stats.Minimum.ToString("F4", CultureInfo.InvariantCulture), ", ", stats.Maximum.ToString("F4", CultureInfo.InvariantCulture), ", "));
                //xy array of 10 points on cdf
                var array = data.ToArray();
                Array.Sort(array);
                int i00 = 0;
                int i10 = (int)(array.Length * .10);
                int i20 = (int)(array.Length * .20);
                int i30 = (int)(array.Length * .30);
                int i40 = (int)(array.Length * .40);
                int i50 = (int)(array.Length * .50);
                int i60 = (int)(array.Length * .60);
                int i70 = (int)(array.Length * .70);
                int i80 = (int)(array.Length * .80);
                int i90 = (int)(array.Length * .90);
                int i100 = (int)(array.Length - 1);
                sb.AppendLine(Errors.GetMessage("STATS_DESC4"));
                sb.AppendLine(string.Concat(
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i00]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i10]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i20]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i30]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i40]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i50]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i60]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i70]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i80]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i90]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(data, array[i100]).ToString("N2", CultureInfo.InvariantCulture)));
                sb.AppendLine(string.Concat(
                    array[i00].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i10].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i20].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i30].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i40].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i50].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i60].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i70].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i80].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i90].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    array[i100].ToString("F4", CultureInfo.InvariantCulture)));
                if (cdf == null)
                {
                    cdf = new List<double>();
                }
                cdf.Add(array[i00]);
                cdf.Add(array[i10]);
                cdf.Add(array[i20]);
                cdf.Add(array[i30]);
                cdf.Add(array[i40]);
                cdf.Add(array[i50]);
                cdf.Add(array[i60]);
                cdf.Add(array[i70]);
                cdf.Add(array[i80]);
                cdf.Add(array[i90]);
                cdf.Add(array[i100]);
            }
            else
            {
                sb.AppendLine(Errors.GetMessage("STATS_DESC6"));
            }
            //doubles need restricted digits
            mostLikely = Math.Round(mostLikely, 4);
            lowRange = Math.Round(lowRange, 4);
            highRange = Math.Round(highRange, 4);
            string sD1Unit = "low";
            string sD2Unit = "high";
            this.IndicatorQT.QTD1 = lowEstimate;
            if (this.IndicatorQT.QTD1Unit == string.Empty || this.IndicatorQT.QTD1Unit == Constants.NONE)
                this.IndicatorQT.QTD1Unit = sD1Unit;
            this.IndicatorQT.QTD2 = highEstimate;
            if (this.IndicatorQT.QTD2Unit == string.Empty || this.IndicatorQT.QTD2Unit == Constants.NONE)
                this.IndicatorQT.QTD2Unit = sD2Unit;
            //computed results
            this.IndicatorQT.QTM = mostLikely;
            if (this.IndicatorQT.QTMUnit == string.Empty || this.IndicatorQT.QTMUnit == Constants.NONE)
                this.IndicatorQT.QTMUnit = mostLikelyUnit;
            this.IndicatorQT.QTL = lowRange;
            if (this.IndicatorQT.QTLUnit == string.Empty || this.IndicatorQT.QTLUnit == Constants.NONE)
                this.IndicatorQT.QTLUnit = lowUnit;
            this.IndicatorQT.QTU = highRange;
            if (this.IndicatorQT.QTUUnit == string.Empty || this.IndicatorQT.QTUUnit == Constants.NONE)
                this.IndicatorQT.QTUUnit = highUnit;
            if (this.IndicatorQT.MathResult.ToLower().StartsWith("http"))
            {
                string sError = string.Empty;
                bool bHasSaved = CalculatorHelpers.SaveTextInURI(
                    Params.ExtensionDocToCalcURI, sb.ToString(), this.IndicatorQT.MathResult, out sError);
                if (!string.IsNullOrEmpty(sError))
                {
                    this.IndicatorQT.MathResult += sError;
                }
            }
            else
            {
                this.IndicatorQT.MathResult = sb.ToString();
            }
        }
        public double[] GetSampleData(string distType, double mostLikelyEstimate, 
            double lowEstimate, double highEstimate)
        {
            if (Iterations > 10000)
                Iterations = 10000;
            if (Iterations <= 2)
                Iterations = 1000;
            if (this.CILevel < 10)
                this.CILevel = 90;
            if (this.CILevel > 99)
                this.CILevel = 99;
            Random rnd = new Random(Random);
            mostLikelyEstimate = Math.Round(mostLikelyEstimate, 4);
            lowEstimate = Math.Round(lowEstimate, 4);
            highEstimate = Math.Round(highEstimate, 4);
            var sampledata = new double[Iterations];
            if (distType == Calculator1.RUC_TYPES.triangle.ToString())
            {
                if (lowEstimate >= mostLikelyEstimate || lowEstimate == 0)
                {
                    //arbitrary rules (25%)
                    lowEstimate = mostLikelyEstimate * .75;
                    //no errors: lowEstimate = 0 is often the case
                    //sb.AppendLine(Errors.GetMessage("DATA_BADDISTRIBUTION"));
                }
                if (highEstimate <= mostLikelyEstimate || highEstimate == 0)
                {
                    //arbitrary rules (25%)
                    highEstimate = mostLikelyEstimate * 1.25;
                }
                if (Random != 0)
                {
                    //generate samples of the Triangular(low, high, mode) distribution;
                    Triangular.Samples(rnd, sampledata, lowEstimate, highEstimate, mostLikelyEstimate);
                }
                else
                {
                    //generate samples of the Triangular(low, high, mode) distribution;
                    Triangular.Samples(sampledata, lowEstimate, highEstimate, mostLikelyEstimate);
                }
            }
            else if (distType == Calculator1.RUC_TYPES.normal.ToString())
            {
                //generate samples of the Normal(mean, sd) distribution;
                if (Random != 0)
                {
                    Normal.Samples(rnd, sampledata, lowEstimate, highEstimate);
                }
                else
                {
                    Normal.Samples(sampledata, lowEstimate, highEstimate);
                }
            }
            else if (distType == Calculator1.RUC_TYPES.lognormal.ToString())
            {
                if (Random != 0)
                {
                    LogNormal.Samples(rnd, sampledata, lowEstimate, highEstimate);
                }
                else
                {
                    LogNormal.Samples(sampledata, lowEstimate, highEstimate);
                }
            }
            else if (distType == Calculator1.RUC_TYPES.weibull.ToString())
            {
                if (Random != 0)
                {
                    Weibull.Samples(rnd, sampledata, lowEstimate, highEstimate);
                }
                else
                {
                    Weibull.Samples(sampledata, lowEstimate, highEstimate);
                }
            }
            else if (distType == Calculator1.RUC_TYPES.beta.ToString())
            {
                if (Random != 0)
                {
                    Beta.Samples(rnd, sampledata, lowEstimate, highEstimate);
                }
                else
                {
                    Beta.Samples(sampledata, lowEstimate, highEstimate);
                }
            }
            else if (distType == Calculator1.RUC_TYPES.pareto.ToString())
            {
                if (Random != 0)
                {
                    Pareto.Samples(rnd, sampledata, lowEstimate, highEstimate);
                }
                else
                {
                    Pareto.Samples(sampledata, lowEstimate, highEstimate);
                }
            }
            else if (distType == Calculator1.RUC_TYPES.uniform.ToString())
            {
                var sampleints = new int[Iterations];
                int iLower = CalculatorHelpers.ConvertStringToInt(lowEstimate.ToString());
                int iUpper = CalculatorHelpers.ConvertStringToInt(highEstimate.ToString());
                if (Random != 0)
                {
                    DiscreteUniform.Samples(rnd, sampleints, iLower, iUpper);
                }
                else
                {
                    DiscreteUniform.Samples(sampleints, iLower, iUpper);
                }
                for (int i = 0; i < sampleints.Count(); i++)
                {
                    sampledata[i] = sampleints[i];
                }
            }
            else if (distType == Calculator1.RUC_TYPES.bernoulli.ToString())
            {
                var sampleints = new int[Iterations];
                if (Random != 0)
                {
                    Bernoulli.Samples(rnd, sampleints, lowEstimate);
                }
                else
                {
                    Bernoulli.Samples(sampleints, lowEstimate);
                }
                for (int i = 0; i < sampleints.Count(); i++)
                {
                    sampledata[i] = sampleints[i];
                }
            }
            else if (distType == Calculator1.RUC_TYPES.poisson.ToString())
            {
                var sampleints = new int[Iterations];
                if (Random != 0)
                {
                    Poisson.Samples(rnd, sampleints, lowEstimate);
                }
                else
                {
                    Poisson.Samples(sampleints, lowEstimate);
                }
                for (int i = 0; i < sampleints.Count(); i++)
                {
                    sampledata[i] = sampleints[i];
                }
            }
            else if (distType == Calculator1.RUC_TYPES.binomial.ToString())
            {
                var sampleints = new int[Iterations];
                int iUpperEstimate = CalculatorHelpers.ConvertStringToInt(highEstimate.ToString());
                if (Random != 0)
                {
                    Binomial.Samples(rnd, sampleints, lowEstimate, iUpperEstimate);
                }
                else
                {
                    Binomial.Samples(sampleints, lowEstimate, iUpperEstimate);
                }
                for (int i = 0; i < sampleints.Count(); i++)
                {
                    sampledata[i] = sampleints[i];
                }
            }
            else if (distType == Calculator1.RUC_TYPES.gamma.ToString())
            {
                //generate samples of the Gamma(shape, scale) distribution;
                if (Random != 0)
                {
                    Gamma.Samples(rnd, sampledata, lowEstimate, highEstimate);
                }
                else
                {
                    Gamma.Samples(sampledata, lowEstimate, highEstimate);
                }
            }
            else
            {
                //don't force them to use distribution
            }
            //hold for possible infernet use
            //else if (distType == Calculator1.RUC_TYPES.dirichlet.ToString())
            //{
            //    //generate samples of the Dirichlet(random, alpha) distribution;
            //    Dirichlet.Sample(sampledata, lowEstimate);
            //}
            //else if (distType == Calculator1.RUC_TYPES.wishart.ToString())
            //{
            //    //generate samples of the Wishart(random, degrees of freedom, scale) distribution;
            //    Wishart.Sample(sampledata, lowEstimate, highEstimate);
            //}

            //the mathlibrary supports more than a dozen additional distributions

            return sampledata;
        }
        public void SetObservedMathResult(string mathType, string distType,
            string mathSubType, double mostLikely, double[] obsTs)
        {
            //this is observed data rather than randomly sampled data
            StringBuilder sb = new StringBuilder();
            if (obsTs.Count() > 0)
            {
                //xy array of 10 points on cdf
                //var array = qTs.ToArray();
                Array.Sort(obsTs);
                int i00 = 0;
                int i10 = (int)(obsTs.Length * .10);
                int i20 = (int)(obsTs.Length * .20);
                int i30 = (int)(obsTs.Length * .30);
                int i40 = (int)(obsTs.Length * .40);
                int i50 = (int)(obsTs.Length * .50);
                int i60 = (int)(obsTs.Length * .60);
                int i70 = (int)(obsTs.Length * .70);
                int i80 = (int)(obsTs.Length * .80);
                int i90 = (int)(obsTs.Length * .90);
                int i100 = (int)(obsTs.Length - 1);
                sb.AppendLine(Errors.GetMessage("STATS_DESC5"));
                sb.AppendLine(string.Concat(
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i00]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i10]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i20]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i30]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i40]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i50]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i60]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i70]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i80]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i90]).ToString("N2", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.EmpiricalCDF(obsTs, obsTs[i100]).ToString("N2", CultureInfo.InvariantCulture)));
                sb.AppendLine(string.Concat(
                obsTs[i00].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i10].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i20].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i30].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i40].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i50].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i60].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i70].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i80].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i90].ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                obsTs[i100].ToString("F4", CultureInfo.InvariantCulture)));
                sb.AppendLine(Errors.GetMessage("STATS_DESC3"));
                sb.AppendLine(Errors.GetMessage("STATS_DESC2"));
                //qT is the result of math and stored in sixth col
                var stats = new MathNet.Numerics.Statistics.DescriptiveStatistics(obsTs);
                sb.AppendLine(string.Concat(stats.Count.ToString(), Constants.CSV_DELIMITER, obsTs.Sum().ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    stats.Mean.ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    MathNet.Numerics.Statistics.Statistics.Median(obsTs).ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    stats.StandardDeviation.ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER, stats.Variance.ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER,
                    stats.Minimum.ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER, stats.Maximum.ToString("F4", CultureInfo.InvariantCulture), Constants.CSV_DELIMITER));
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                this.IndicatorQT.QTM = stats.Mean;
                this.IndicatorQT.QTL = stats.Mean - CalculatorHelpers.GetConfidenceInterval(this.CILevel, stats.Count, stats.StandardDeviation);
                this.IndicatorQT.QTU = stats.Mean + CalculatorHelpers.GetConfidenceInterval(this.CILevel, stats.Count, stats.StandardDeviation);
                this.IndicatorQT.QTLUnit = sLowerCI;
                this.IndicatorQT.QTUUnit = sUpperCI;
            }
            else
            {
                sb.AppendLine(Errors.MakeStandardErrorMsg("DATAURL_BADDATA"));
            }
            if (this.IndicatorQT.MathResult.ToLower().StartsWith("http"))
            {
                string sError = string.Empty;
                bool bHasSaved = CalculatorHelpers.SaveTextInURI(
                    Params.ExtensionDocToCalcURI, sb.ToString(), this.IndicatorQT.MathResult, out sError);
                if (!string.IsNullOrEmpty(sError))
                {
                    this.IndicatorQT.MathResult += sError;
                }
            }
            else
            {
                this.IndicatorQT.MathResult = sb.ToString();
            }
        }
    }
}
