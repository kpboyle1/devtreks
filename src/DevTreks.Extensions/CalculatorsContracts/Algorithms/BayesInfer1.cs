using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using MicrosoftResearch.Infer;
//using MicrosoftResearch.Infer.Models;
//using MicrosoftResearch.Infer.Distributions;
//using MicrosoftResearch.Infer.Maths;

//namespace DevTreks.Extensions.Algorithms
//{
//    // <summary>
//    ///Purpose:		Bayesian prediction algorithms
//    ///Author:		www.devtreks.org
//    ///Date:		2016, May
//    ///References:	CTA example 8
//    public class BayesInfer1
//    {
//        public BayesInfer1()
//        {

//        }
//        /// <summary>
//        /// Initialize the Bayesian prediction algorithm
//        /// </summary>
//        /// <param name="colNames"></param>
//        /// <param name="qs"></param>
//        /// <param name="mathExpress">//pass in the MathExpression to make sure the data colindex (i.e. q1 = col[3]) 
//        ///is in the mathexpress (i.e. mathexpress.contains("q1")</param>
//        public BayesInfer1(string[] mathTerms, string[] colNames, string[] depColNames, double[] qs)
//        {
//            _colNames = colNames;
//            _depColNames = depColNames;
//            _mathTerms = mathTerms;
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
//        public enum BAYES1_MODELS
//        {
//            none        = 0, 
//            predict1    = 1,
//            classify1   = 2,
//            recommend1  = 3
//        }
//        /// <summary>
//        /// Run a bayesian prob risk analysis
//        /// </summary> 
//        /// <param name="model">Probability distributions in JDataURL column format</param>
//        /// <param name="data">Data being analyzed</param>
//        /// <returns></returns>
//        public async Task RunAlgorithmAsync(List<List<string>> model, List<List<string>> data)
//        {
//            try
//            {
//                ///label, date, learningstep, population, q6
//                if (data.Count == 0)
//                {
//                    Error = "Bayes analysis requires at least one row of data in the dataset.";
//                    return;
//                }
//                List<Distribution> dists = new List<Distribution>();
//                foreach (var distribution in model)
//                {
//                    Distribution newDist = new Distribution(distribution);
//                    dists.Add(newDist);
//                }
//                //run each model separately 
//                List<Distribution> predict1 = dists
//                    .Where(l => l.ModelName == BAYES1_MODELS.predict1.ToString()).ToList();
//                if (predict1 != null)
//                {
//                    if (predict1.Count() > 0)
//                    {
//                        await RunPredict1Async(predict1, data);
//                    }
//                }
//                List<Distribution> classify1 = dists
//                    .Where(l => l.ModelName == BAYES1_MODELS.classify1.ToString()).ToList();
//                if (classify1 != null)
//                {
//                    if (classify1.Count() > 0)
//                    {
//                        await RunClassify1Async(classify1, data);
//                    }
//                }
//                List<Distribution> recommend1 = dists
//                    .Where(l => l.ModelName == BAYES1_MODELS.recommend1.ToString()).ToList();
//                if (recommend1 != null)
//                {
//                    if (recommend1.Count() > 0)
//                    {
//                        await RunRecommend1Async(recommend1, data);
//                    }
//                }
//                //Vector[] data = dataList.ToArray();







//            }
//            catch (Exception ex)
//            {
//                this.Error = ex.Message;
//            }
//        }
//        public async Task RunPredict1Async(List<Distribution> predict1, List<List<string>> data)
//        {
//            //the prediction1 model uses standard doubles for y vector and x matrix
//            VariableArray<double> y = Shared.GetYDataForVariableArray(data);
//            VariableArray2D<double> x = Shared.GetDoubleVariableArray2D(data, _colNames, _depColNames);
//            //build models
//            StringBuilder sb = new StringBuilder();
//            int i = 1;
//            List<ModelTraining> colTraining = new List<ModelTraining>();
//            sb.AppendLine(string.Concat("\nLearning step number ", i.ToString()));
//            ModelTraining bayesTraining = new ModelTraining();
//            bayesTraining.CreateModel();
//            sb.AppendLine(string.Concat("\nLearning step number ", i.ToString()));
//            //models are vectors of distributions that correspond to data
//            ModelData initPriors = new ModelData();
//            foreach (var distribution in predict1)
//            {
//                if (distribution.DistributionType == Calculator1.RUC_TYPES.normal.ToString())
//                {
//                    //182 supports 1 set of params for each distribution
//                    initPriors.NormalDist = Gaussian.FromMeanAndPrecision(distribution.D1, distribution.D2);
//                }
//                else if (distribution.DistributionType == Calculator1.RUC_TYPES.gamma.ToString())
//                {
//                    initPriors.GammaDist = Gamma.FromShapeAndScale(distribution.D1, distribution.D2);
//                }
//            }
//            bayesTraining.SetModelData(initPriors);
//            colTraining.Add(bayesTraining);








//            int t = 0;
//            foreach (var bTrain in colTraining)
//            {
//                //int rows = x.ObservedValue.GetUpperBound(0) + 1;
//                //int cols = x.ObservedValue.GetUpperBound(1) + 1;
//                //double[] xvector = new double[rows];
//                //for (int c = 0; c < cols; ++c)
//                //{
//                //    for (int r = 0; r < rows; ++r)
//                //    {
//                //        xvector[r] = x.ObservedValue[r, c];
//                //    }
//                //}
//                //process each vector
//                //ModelData posteriors1 = bayesTraining.InferModelData(xvector.ToArray());

//                //sb.AppendLine(string.Concat("Mean distribution = " + posteriors1.NormalDist));
//                //sb.AppendLine(string.Concat("Precision distribution = " + posteriors1.GammaDist));

//                ////Make predictions based on the trained model
//                //ModelPrediction bayesPrediction = new ModelPrediction();
//                //bayesPrediction.CreateModel();
//                //bayesPrediction.SetModelData(posteriors1);

//                //Gaussian QTMDist = bayesPrediction.InferQTMDist();

//                //double QTMean = QTMDist.GetMean();
//                //double QTStdDev = Math.Sqrt(QTMDist.GetVariance());

//                //sb.AppendLine(string.Concat("QTM: ", QTMean));
//                //sb.AppendLine(string.Concat("QTM standard deviation:", QTStdDev));

//                ////double QT = _qs[0];
//                //Distribution predictedDist
//                //    = predict1.FirstOrDefault(d => d.DistributionType == Calculator1.RUC_TYPES.normal.ToString());

//                //if (predictedDist != null)
//                //{
//                //    sb.AppendLine(string.Concat("Probability that QTM is ",
//                //    predictedDist.MathOp,
//                //    " ",
//                //    predictedDist.MathOpQ.ToString(),
//                //    " ",
//                //    bayesPrediction.InferConstrainedProbability(
//                //        predictedDist.MathOp, predictedDist.MathOpQ)));
//                //} this.QTPredicted = QTMean;
//                //double d95Range = CalculatorHelpers.GetConfidenceInterval(95, y.ObservedValue.Count(), QTStdDev);
//                //QTL = this.QTPredicted - d95Range;
//                //QTLUnit = "lower 95% ci";
//                //QTU = this.QTPredicted + d95Range;
//                //QTUUnit = "upper 95% ci";
//                //t++;
//            }




//            //process yvector last -may need to be combo of xs
//            bool bHasYVector = y.ObservedValue.Any(a => a != 0);
//            if (bHasYVector)
//            {
//                ModelData posteriors1 = bayesTraining.InferModelData(y);

//                sb.AppendLine(string.Concat("Mean distribution = " + posteriors1.NormalDist));
//                sb.AppendLine(string.Concat("Precision distribution = " + posteriors1.GammaDist));

//                //Make predictions based on the trained model
//                ModelPrediction bayesPrediction = new ModelPrediction();
//                bayesPrediction.CreateModel();
//                bayesPrediction.SetModelData(posteriors1);

//                Gaussian QTMDist = bayesPrediction.InferQTMDist();

//                double QTMean = QTMDist.GetMean();
//                double QTStdDev = Math.Sqrt(QTMDist.GetVariance());

//                sb.AppendLine(string.Concat("QTM: ", QTMean));
//                sb.AppendLine(string.Concat("QTM standard deviation:", QTStdDev));

//                //double QT = _qs[0];
//                Distribution predictedDist
//                    = predict1.FirstOrDefault(d => d.DistributionType == Calculator1.RUC_TYPES.normal.ToString());

//                if (predictedDist != null)
//                {
//                    sb.AppendLine(string.Concat("Probability that QTM is ",
//                    predictedDist.MathOp,
//                    " ",
//                    predictedDist.MathOpQ.ToString(),
//                    " ",
//                    bayesPrediction.InferConstrainedProbability(
//                        predictedDist.MathOp, predictedDist.MathOpQ)));
//                }                this.QTPredicted = QTMean;
//                double d95Range = CalculatorHelpers.GetConfidenceInterval(95, y.ObservedValue.Count(), QTStdDev);
//                QTL = this.QTPredicted - d95Range;
//                QTLUnit = "lower 95% ci";
//                QTU = this.QTPredicted + d95Range;
//                QTUUnit = "upper 95% ci";
//            }

//            this.MathResult = sb.ToString();
//        }
//        public async Task RunClassify1Async(List<Distribution> classify1, List<List<string>> data)
//        {
//        }
//        public async Task RunRecommend1Async(List<Distribution> recommend1, List<List<string>> data)
//        {
//        }
//        //source for distribution models
//        public class Distribution
//        {
//            public Distribution(List<string> distributionVars)
//            {
//                if (distributionVars.Count == 10)
//                {
//                    Label = distributionVars[0];
//                    InputVariable = distributionVars[1];
//                    Dataset = CalculatorHelpers.ConvertStringToInt(distributionVars[2]);
//                    ModelName = distributionVars[3];
//                    Uncertainty = distributionVars[4];
//                    DataFormat = distributionVars[5];
//                    VariableFormat = distributionVars[6];
//                    Range = distributionVars[7];
//                    DistributionType = distributionVars[8];
//                    D1 = CalculatorHelpers.ConvertStringToDouble(distributionVars[9]);
//                    D2 = CalculatorHelpers.ConvertStringToDouble(distributionVars[10]);
//                    MathOp = distributionVars[11];
//                    MathOpQ = CalculatorHelpers.ConvertStringToDouble(distributionVars[12]);
//                }
//                else 
//                {
//                    ErrorMsg = "The distributions for variables have not been defined correctly in the JDataURL.";
//                }

//            }
//            public string ErrorMsg { get; set; }
//            public string Label { get; set; }
//            public string InputVariable { get; set; }
//            public int Dataset { get; set; }
//            public string ModelName { get; set; }
//            public string Uncertainty { get; set; }
//            public string DataFormat { get; set; }
//            public string VariableFormat { get; set; }
//            public string Range { get; set; }
//            public string DistributionType { get; set; }
//            public double D1 { get; set; }
//            public double D2 { get; set; }
//            public string MathOp { get; set; }
//            public double MathOpQ { get; set; }
//        }
//        //source for cycling3
//        public class ModelMixedBase
//        {
//            protected InferenceEngine InferenceEngine;

//            protected int NumComponents;

//            protected VariableArray<Gaussian> AverageQTPriors;
//            protected VariableArray<Gamma> ModelNoisePriors;
//            protected Variable<Dirichlet> MixingPrior;

//            protected VariableArray<double> AverageQT;
//            protected VariableArray<double> ModelNoise;
//            protected Variable<Vector> MixingCoefficients;

//            public virtual void CreateModel()
//            {
//                NumComponents = 2;
//                Range ComponentRange = new Range(NumComponents);
//                InferenceEngine = new InferenceEngine(new VariationalMessagePassing());
//                InferenceEngine.ShowProgress = false;
//                //avoid security exceptions by using memory only
//                InferenceEngine.Compiler.WriteSourceFiles = false;
//                InferenceEngine.Compiler.GenerateInMemory = false;
//                //if the same model has to be run multiple times but with different data
//                //engine.Compiler.FreeMemory = false;

//                AverageQTPriors = Variable.Array<Gaussian>(ComponentRange);
//                ModelNoisePriors = Variable.Array<Gamma>(ComponentRange);
//                AverageQT = Variable.Array<double>(ComponentRange);
//                ModelNoise = Variable.Array<double>(ComponentRange);

//                using (Variable.ForEach(ComponentRange))
//                {
//                    AverageQT[ComponentRange] = Variable<double>.Random(AverageQTPriors[ComponentRange]);
//                    ModelNoise[ComponentRange] = Variable<double>.Random(ModelNoisePriors[ComponentRange]);
//                }

//                //Mixing coefficients
//                MixingPrior = Variable.New<Dirichlet>();
//                MixingCoefficients = Variable<Vector>.Random(MixingPrior);
//                MixingCoefficients.SetValueRange(ComponentRange);
//            }

//            public virtual void SetModelData(ModelDataMixed modelData)
//            {
//                AverageQTPriors.ObservedValue = modelData.AverageQTDist;
//                ModelNoisePriors.ObservedValue = modelData.ModelNoiseDist;
//                MixingPrior.ObservedValue = modelData.MixingDist;
//            }
//        }

//        public class ModelMixedTraining : ModelMixedBase
//        {
//            protected Variable<int> NumTrips;
//            protected VariableArray<double> TravelTimes;
//            protected VariableArray<int> ComponentIndices;

//            public override void CreateModel()
//            {
//                base.CreateModel();

//                NumTrips = Variable.New<int>();
//                Range tripRange = new Range(NumTrips);
//                TravelTimes = Variable.Array<double>(tripRange);
//                ComponentIndices = Variable.Array<int>(tripRange);

//                using (Variable.ForEach(tripRange))
//                {
//                    ComponentIndices[tripRange] = Variable.Discrete(MixingCoefficients);
//                    using (Variable.Switch(ComponentIndices[tripRange]))
//                    {
//                        TravelTimes[tripRange].SetTo(
//                            Variable.GaussianFromMeanAndPrecision(
//                                AverageQT[ComponentIndices[tripRange]],
//                                ModelNoise[ComponentIndices[tripRange]]));
//                    }
//                }
//            }
//            public ModelDataMixed InferModelData(double[] trainingData) //Training model
//            {
//                ModelDataMixed posteriors;

//                //Set Priors and training data
//                NumTrips.ObservedValue = trainingData.Length;
//                TravelTimes.ObservedValue = trainingData;

//                posteriors.AverageQTDist = InferenceEngine.Infer<Gaussian[]>(AverageQT);
//                posteriors.ModelNoiseDist = InferenceEngine.Infer<Gamma[]>(ModelNoise);
//                posteriors.MixingDist = InferenceEngine.Infer<Dirichlet>(MixingCoefficients);

//                return posteriors;
//            }
//        }

//        public class ModelMixedPrediction : ModelMixedBase
//        {
//            private Gaussian QTMDist;
//            private Variable<double> QTM;

//            public override void CreateModel()
//            {
//                base.CreateModel();

//                Variable<int> componentIndex = Variable.Discrete(MixingCoefficients);
//                QTM = Variable.New<double>();

//                using (Variable.Switch(componentIndex))
//                {
//                    QTM.SetTo(
//                          Variable.GaussianFromMeanAndPrecision(
//                          AverageQT[componentIndex],
//                          ModelNoise[componentIndex]));
//                }
//            }

//            public Gaussian InferQTM() //Prediction mode
//            {
//                QTMDist = InferenceEngine.Infer<Gaussian>(QTM);
//                return QTMDist;
//            }
//        }

//        public struct ModelDataMixed
//        {
//            public Gaussian[] AverageQTDist;
//            public Gamma[] ModelNoiseDist;
//            public Dirichlet MixingDist;
//        }

//        //source for cycling4
//        public class ModelWithEvidence : ModelTraining
//        {
//            protected Variable<bool> Evidence;

//            public override void CreateModel()
//            {
//                Evidence = Variable.Bernoulli(0.5);
//                using (Variable.If(Evidence))
//                {
//                    base.CreateModel();
//                }
//            }

//            public double InferEvidence(double[] trainingData)
//            {
//                double logEvidence;
//                //ModelData posteriors = base.InferModelData(trainingData);
//                logEvidence = InferenceEngine.Infer<Bernoulli>(Evidence).LogOdds;

//                return logEvidence;
//            }
//        }

//        public class ModelMixedWithEvidence : ModelMixedTraining
//        {
//            protected Variable<bool> Evidence;

//            public override void CreateModel()
//            {
//                Evidence = Variable.Bernoulli(0.5);
//                using (Variable.If(Evidence))
//                {
//                    base.CreateModel();
//                }
//            }

//            public double InferEvidence(double[] trainingData)
//            {
//                double logEvidence;
//                ModelDataMixed posteriors = base.InferModelData(trainingData);
//                logEvidence = InferenceEngine.Infer<Bernoulli>(Evidence).LogOdds;

//                return logEvidence;
//            }
//        }

//        //source for running cycle5
//        private ModelTraining population1, population2;

//        public void CreateModel()
//        {
//            population1 = new ModelTraining();
//            population1.CreateModel();
//            population2 = new ModelTraining();
//            population2.CreateModel();
//        }

//        public void SetModelData(ModelData modelData)
//        {
//            population1.SetModelData(modelData);
//            population2.SetModelData(modelData);
//        }

//        public ModelData[] InferModelData(double[] trainingData1,
//                                          double[] trainingData2)
//        {
//            ModelData[] posteriors = new ModelData[2];

//            //posteriors[0] = population1.InferModelData(trainingData1);
//            //posteriors[1] = population2.InferModelData(trainingData2);

//            return posteriors;
//        }

//    }

//    public class TwoModelsPrediction
//    {
//        private ModelPrediction population1, population2;
//        private Variable<double> TimeDifference;
//        private Variable<bool> Model1IsFaster;
//        private InferenceEngine CommonEngine;

//        public void CreateModel()
//        {
//            CommonEngine = new InferenceEngine();

//            population1 = new ModelPrediction() { InferenceEngine = CommonEngine };
//            population1.CreateModel();
//            population2 = new ModelPrediction() { InferenceEngine = CommonEngine };
//            population2.CreateModel();

//            TimeDifference = population1.QTM - population2.QTM;
//            Model1IsFaster = population1.QTM < population2.QTM;
//        }

//        public void SetModelData(ModelData[] modelData)
//        {
//            population1.SetModelData(modelData[0]);
//            population2.SetModelData(modelData[1]);
//        }

//        public Gaussian[] InferQTMDist()
//        {
//            Gaussian[] tomorrowsTime = new Gaussian[2];

//            tomorrowsTime[0] = population1.InferQTMDist();
//            tomorrowsTime[1] = population2.InferQTMDist();
//            return tomorrowsTime;
//        }

//        public Gaussian InferTimeDifference()
//        {
//            return CommonEngine.Infer<Gaussian>(TimeDifference);
//        }

//        public Bernoulli InferModel1IsFaster()
//        {
//            return CommonEngine.Infer<Bernoulli>(Model1IsFaster);
//        }
//    }
//}
//// <summary>
/////Purpose:		Base classes used bayesian prediction
/////Author:		www.devtreks.org
/////Date:		2015, March
/////References:	Infer project
//public class ModelBase
//{
//    public InferenceEngine InferenceEngine;

//    protected Variable<double> VarQ1Q2;
//    protected Variable<double> VarQ3Q4;
//    protected Variable<Gaussian> VarQ1Q2Prior;
//    protected Variable<Gamma> VarQ3Q4Prior;

//    public virtual void CreateModel()
//    {
//        VarQ1Q2Prior = Variable.New<Gaussian>();
//        VarQ3Q4Prior = Variable.New<Gamma>();
//        VarQ1Q2 = Variable<double>.Random(VarQ1Q2Prior);
//        VarQ3Q4 = Variable<double>.Random(VarQ3Q4Prior);
//        if (InferenceEngine == null)
//        {
//            InferenceEngine = new InferenceEngine();
//        }
//    }

//    public virtual void SetModelData(ModelData priors)
//    {
//        VarQ1Q2Prior.ObservedValue = priors.NormalDist;
//        VarQ3Q4Prior.ObservedValue = priors.GammaDist;
//    }
//}

//public class ModelTraining : ModelBase
//{
//    protected VariableArray<double> TravelTimes;
//    protected Variable<int> NumTrips;

//    public override void CreateModel()
//    {
//        base.CreateModel();
//        NumTrips = Variable.New<int>();
//        Range tripRange = new Range(NumTrips);
//        TravelTimes = Variable.Array<double>(tripRange);
//        using (Variable.ForEach(tripRange))
//        {
//            TravelTimes[tripRange] = Variable.GaussianFromMeanAndPrecision(VarQ1Q2, VarQ3Q4);
//        }
//    }
//    public ModelData InferModelData(VariableArray<double> trainingData)
//    {
//        ModelData posteriors;

//        NumTrips.ObservedValue = trainingData.ObservedValue.Length;
//        TravelTimes.ObservedValue = trainingData.ObservedValue;
//        posteriors.NormalDist = InferenceEngine.Infer<Gaussian>(VarQ1Q2);
//        posteriors.GammaDist = InferenceEngine.Infer<Gamma>(VarQ3Q4);
//        return posteriors;
//    }
//}

//public class ModelPrediction : ModelBase
//{
//    private Gaussian QTMDist;
//    public Variable<double> QTM;

//    public override void CreateModel()
//    {
//        base.CreateModel();
//        QTM = Variable.GaussianFromMeanAndPrecision(VarQ1Q2, VarQ3Q4);
//    }

//    public Gaussian InferQTMDist()
//    {
//        QTMDist = InferenceEngine.Infer<Gaussian>(QTM);
//        return QTMDist;
//    }

//    public Bernoulli InferConstrainedProbability(string mathOp, double q6)
//    {
//        Bernoulli bVar = new Bernoulli();
//        if (mathOp == DevTreks.Extensions.Calculator1.MATH_OPERATOR_TYPES.lessthan.ToString())
//        {
//            return InferenceEngine.Infer<Bernoulli>(QTM < q6);
//        }
//        return bVar;
//    }
//}

//public struct ModelData
//{
//    public Gaussian NormalDist;
//    public Gamma GammaDist;
//    //public Beta BetaDist;
//    public ModelData(Gaussian mean, Gamma precision)
//    {
//        NormalDist = mean;
//        GammaDist = precision;
//        //BetaDist = new Beta();
//    }
//    //public ModelData(Beta mean, Gamma precision)
//    //{
//    //    BetaDist = mean;
//    //    GammaDist = precision;
//    //}
//}