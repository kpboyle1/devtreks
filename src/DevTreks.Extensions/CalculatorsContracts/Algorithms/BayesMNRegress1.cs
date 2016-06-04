using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using MicrosoftResearch.Infer;
//using MicrosoftResearch.Infer.Maths;
//using MicrosoftResearch.Infer.Models;
//using MicrosoftResearch.Infer.Distributions;
//using MicrosoftResearch.Infer.Factors;
//namespace DevTreks.Extensions.Algorithms
//{
//    // <summary>
//    ///Purpose:		Bayesian multinomial regression model
//    ///Author:		www.devtreks.org
//    ///Date:		2016, May
//    ///References:	CTA example 8
//    public class BayesMNRegress1
//    {
//        public BayesMNRegress1(string[] mathTerms, string[] colNames, string[] depColNames, 
//            double[] qs, int iterations)
//        {
//            _colNames = colNames;
//            _depColNames = depColNames;
//            _mathTerms = mathTerms;
//            _iterations = iterations;
//            if (_iterations <= 0)
//            {
//                _iterations = 1000;
//            }
//            //not always needed
//            if (qs.Count() > 0)
//            {
//                _qs = new double[qs.Count()];
//                ////add an intercept to qs 
//                //_qs = new double[qs.Count() + 1];
//                ////1 * b0 = b0
//                //_qs[0] = 1;
//                qs.CopyTo(_qs, 0);
//            }
//        }
//private CalculatorParameters _params { get; set; }
//        //all of the the dependent and independent var column names
//        private string[] _colNames { get; set; }
//        //all of the the dependent var column names including intercept
//        private string[] _depColNames { get; set; }
//        //corresponding Ix.Qx names (1 less count because no dependent var)
//        private string[] _mathTerms { get; set; }
//        //corresponding Qx amounts
//        private double[] _qs { get; set; }

//        private int _distributionNumber { get; set; }
//        private int _populationNumber { get; set; }
//        private int _learningStep { get; set; }
//        private int _iterations { get; set; }

//        //output
//        //q6 = marginal productivity
//        public double QTSlope { get; set; }
//        //q6M = predicted y
//        public double QTPredicted { get; set; }
//        //lower 95% ci
//        public double QTL { get; set; }
//        public string QTLUnit { get; set; }
//        //upper 95% ci
//        public double QTU { get; set; }
//        public string QTUUnit { get; set; }
//        public string MathResult { get; set; }
//        public string Error { get; set; }
//        /// <summary>
//        /// Run a bayesian prob risk analysis
//        /// </summary> 
//        /// <param name="model">Probability distributions in JDataURL column format</param>
//        /// <param name="data">Data being analyzed</param>
//        /// <returns></returns>
//        public async Task RunAlgorithmAsync(List<List<string>> data)
//        {
//            try
//            {
//                ///label, date, learningstep, population, q6
//                if (data.Count == 0)
//                {
//                    Error = "Bayes analysis requires at least one row of data in the dataset.";
//                    return;
//                }
//                int[][] yObs; //= new int[][] { };
//                Vector[] xObs;
//                if (data.Count() == 1)
//                {
//                    //can use the _qs as means
//                    double results = MultinomialRegressionSynthetic(
//                        _iterations, 6, 4, 10);
//                }
//                else
//                {
//                    //most of this source code comes from the InferNet documentation
//                    //rows correspond to first array, second array stores the indexed y categorical y var {0, 0, 1}
//                    yObs = Shared.GetYDataCategories(data);
//                    //standard 2 dimensional array of independent variables
//                    xObs = Shared.GetDoubleVector(data, _colNames, _depColNames);
//                    //build models
//                    StringBuilder sb = new StringBuilder();
//                    VectorGaussian[] bPost;
//                    Gaussian[] meanPost;
//                    MultinomialRegression(xObs, yObs, out bPost, out meanPost);
//                    var bMeans = bPost.Select(o => o.GetMean()).ToArray();
//                    var bVars = bPost.Select(o => o.GetVariance()).ToArray();
//                    double error = 0;

//                    //number of rows
//                    int iNumSamples = xObs[0].Count();
//                    //number of categories for y
//                    int iNumClasses = yObs[0].Count();
//                    var features = new Vector[iNumSamples];
//                    var counts = new int[iNumSamples][];
//                    //var coefficients = new Vector[iNumClasses];
//                    var mean = Vector.Zero(iNumClasses);
//                    sb.AppendLine("Coefficients -------------- ");
//                    for (int i = 0; i < iNumClasses; i++)
//                    {
//                        error += (bMeans[i] - xObs[i]).Sum(o => o * o);
//                        sb.AppendLine("True " + xObs[i]);
//                        sb.AppendLine("Inferred " + bMeans[i]);
//                    }
//                    sb.AppendLine("Mean -------------- ");
//                    sb.AppendLine("True " + mean);
//                    sb.AppendLine(
//                      "Inferred " + Vector.FromArray(meanPost.Select(o => o.GetMean()).ToArray()));
//                    error = Math.Sqrt(error / (iNumClasses * _depColNames.Count()));
//                    sb.AppendLine(iNumSamples + " " + error);
//                    this.MathResult = sb.ToString();
//                }
//            }
//            catch (Exception ex)
//            {
//                this.Error = ex.Message;
//            }
//        }
//        public void LinearRegression(Vector[] xObs, int[][] yObs)
//        {
//            //Vector[] data = new Vector[] { new Vector(1.0, -3), new Vector(1.0, -2.1), new Vector(1.0, -1.3), new Vector(1.0, 0.5), new Vector(1.0, 1.2), new Vector(1.0, 3.3), new Vector(1.0, 4.4), new Vector(1.0, 5.5) };
//            Range rows = new Range(xObs.Length);
//            VariableArray<Vector> x = Variable.Constant(xObs, rows).Named("x");
//            Variable<Vector> w = Variable.VectorGaussianFromMeanAndPrecision(Vector.Zero(xObs.Count()), 
//                PositiveDefiniteMatrix.Identity(2)).Named("w");
//           // Variable<Vector> w = Variable.VectorGaussianFromMeanAndPrecision(new Vector(new double[] { 0, 0 }), PositiveDefiniteMatrix.Identity(2)).Named("w");
//            VariableArray<double> y = Variable.Array<double>(rows);
//            y[rows] = Variable.GaussianFromMeanAndVariance(Variable.InnerProduct(x[rows], w), 1.0);
//            //y.ObservedValue = new double[] { 30, 45, 40, 80, 70, 100, 130, 110 };
//            InferenceEngine engine = new InferenceEngine(new VariationalMessagePassing());
//            VectorGaussian postW = engine.Infer<VectorGaussian>(w);
//            Console.WriteLine("Posterior over the weights: " + Environment.NewLine + postW);
//        }
//        /// <summary>
//        /// Build and run a multinomial regression model. 
//        /// </summary>
//        /// <param name="xObs">An array of vectors of observed inputs.
//        /// The length of the array is the number of samples, and the
//        ///   length of the vectors is the number of input features. </param>
//        /// <param name="yObs">An array of array of counts, where the first index is the sample,
//        /// and the second index is the class.  </param>
//        /// <param name="bPost">The returned posterior over the coefficients.</param>
//        /// <param name="meanPost">The returned posterior over the means.</param>
//        public void MultinomialRegression(
//           Vector[] xObs, int[][] yObs, out VectorGaussian[] bPost, out Gaussian[] meanPost)
//        {
//            int C = yObs[0].Length;
//            int N = xObs.Length;
//            int K = xObs[0].Count;
//            var c = new Range(C).Named("c");
//            var n = new Range(N).Named("n");

//            // model
//            var B = Variable.Array<Vector>(c).Named("coefficients");
//            B[c] = Variable.VectorGaussianFromMeanAndPrecision(
//                Vector.Zero(K), PositiveDefiniteMatrix.Identity(K)).ForEach(c);
//            var m = Variable.Array<double>(c).Named("mean");
//            m[c] = Variable.GaussianFromMeanAndPrecision(0, 1).ForEach(c);
//            Variable.ConstrainEqualRandom(B[C - 1], VectorGaussian.PointMass(Vector.Zero(K)));
//            Variable.ConstrainEqualRandom(m[C - 1], Gaussian.PointMass(0));
//            var x = Variable.Array<Vector>(n);
//            x.ObservedValue = xObs;
//            var yData = Variable.Array(Variable.Array<int>(c), n);
//            yData.ObservedValue = yObs;
//            var trialsCount = Variable.Array<int>(n);
//            trialsCount.ObservedValue = yObs.Select(o => o.Sum()).ToArray();
//            var g = Variable.Array(Variable.Array<double>(c), n);
//            g[n][c] = Variable.InnerProduct(B[c], x[n]) + m[c];
//            var p = Variable.Array<Vector>(n);
//            p[n] = Variable.Softmax(g[n]);
//            using (Variable.ForEach(n))
//                yData[n] = Variable.Multinomial(trialsCount[n], p[n]);

//            // inference
//            var ie = new InferenceEngine(new VariationalMessagePassing());
//            ie.Compiler.GivePriorityTo(typeof(SoftmaxOp_Taylor));
//            //ie.Compiler.GivePriorityTo(typeof(GammaSoftmaxOp));
//            //ie.Compiler.GivePriorityTo(typeof(SaulJordanSoftmaxOp_NCVMP));
//            bPost = ie.Infer<VectorGaussian[]>(B);
//            meanPost = ie.Infer<Gaussian[]>(m);
//        }

//        /// <summary>
//        /// For the multinomial regression model: generate synthetic data,
//        /// infer the model parameters and calculate the RMSE between the true
//        /// and mean inferred coefficients. 
//        /// </summary>
//        /// <param name="numSamples">Number of samples</param>
//        /// <param name="numFeatures">Number of input features</param>
//        /// <param name="numClasses">Number of classes</param>
//        /// <param name="countPerSample">Total count per sample</param>
//        /// <returns>RMSE between the true and mean inferred coefficients</returns>
//        public double MultinomialRegressionSynthetic(
//             int numSamples, int numFeatures, int numClasses, int countPerSample)
//        {
//            var features = new Vector[numSamples];
//            var counts = new int[numSamples][];
//            var coefficients = new Vector[numClasses];
//            var mean = Vector.Zero(numClasses);
//            Rand.Restart(1);
//            for (int i = 0; i < numClasses - 1; i++)
//            {
//                mean[i] = Rand.Normal();
//                coefficients[i] = Vector.Zero(numFeatures);
//                Rand.Normal(
//                  Vector.Zero(numFeatures),
//                  PositiveDefiniteMatrix.Identity(numFeatures), coefficients[i]);
//            }
//            mean[numClasses - 1] = 0;
//            coefficients[numClasses - 1] = Vector.Zero(numFeatures);
//            for (int i = 0; i < numSamples; i++)
//            {
//                features[i] = Vector.Zero(numFeatures);
//                Rand.Normal(
//                  Vector.Zero(numFeatures),
//                  PositiveDefiniteMatrix.Identity(numFeatures), features[i]);
//                var temp = Vector.FromArray(coefficients.Select(o => o.Inner(features[i])).ToArray());
//                var p = MMath.Softmax(temp + mean);
//                counts[i] = Rand.Multinomial(countPerSample, p);
//            }
//            Rand.Restart(DateTime.Now.Millisecond);
//            VectorGaussian[] bPost;
//            Gaussian[] meanPost;
//            MultinomialRegression(features, counts, out bPost, out meanPost);
//            var bMeans = bPost.Select(o => o.GetMean()).ToArray();
//            var bVars = bPost.Select(o => o.GetVariance()).ToArray();
//            double error = 0;
//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine("Coefficients -------------- ");
//            for (int i = 0; i < numClasses; i++)
//            {
//                error += (bMeans[i] - coefficients[i]).Sum(o => o * o);
//                sb.AppendLine("True " + coefficients[i]);
//                sb.AppendLine("Inferred " + bMeans[i]);
//            }
//            sb.AppendLine("Mean -------------- ");
//            sb.AppendLine("True " + mean);
//            sb.AppendLine(
//              "Inferred " + Vector.FromArray(meanPost.Select(o => o.GetMean()).ToArray()));
//            error = Math.Sqrt(error / (numClasses * numFeatures));
//            sb.AppendLine(numSamples + " " + error);
//            this.MathResult = sb.ToString();
//            return error;
//        }

//        /// <summary>
//        /// Run the synthetic data experiment on a number of different sample sizes. 
//        /// </summary>
//        /// <param name="numFeatures">Number of input features</param>
//        /// <param name="numClasses">Number of classes</param>
//        /// <param name="totalCount">Total count per individual</param>
//        public void TestMultinomialRegressionSampleSize(
//           int numFeatures, int numClasses, int totalCount)
//        {
//            var sampleSize = new int[] {
//         10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 1000, 1500, 2000 };
//            var results = new double[sampleSize.Length];
//            for (int i = 0; i < sampleSize.Length; i++)
//            {
//                results[i] = MultinomialRegressionSynthetic(
//                sampleSize[i], numFeatures, numClasses, totalCount);
//            }
//            for (int i = 0; i < sampleSize.Length; i++)
//            {
//                Console.WriteLine(sampleSize[i] + " " + results[i]);
//            }
//        }
//    }
//}
