using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//probab risk
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Regression analysis algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA algo1 subalgo6
    ///</summary>
    public class Regression1 : Calculator1
    {
        /// <summary>
        /// Initialize the regression analysis algorithm
        /// </summary>
        /// <param name="mathTerms">Math Expression terms, in format Ix.Qx. Potential use in vector math.</param>
        /// <param name="colNames">All column names for data vectors. Identifies vectors to exclude and include.</param>
        /// <param name="depColNames">Column names adjusted for independent var data vectors. Includes intercept term, excludes dep var term.</param>
        /// <param name="qs">Qx Amounts to be estimated, predicted, classified, or recommended.</param>
        ///is in the mathexpress (i.e. mathexpress.contains("q1")</param>
        public Regression1(string[] mathTerms, string[] colNames, string[] depColNames, 
            double[] qs, string subalgorithm, int confidenceInterval, CalculatorParameters calcParams)
            : base()
        {
            _colNames = colNames;
            _depColNames = depColNames;
            _mathTerms = mathTerms;
            _subalgorithm = subalgorithm;
            _confidenceInt = confidenceInterval;
            _params = calcParams;

        }
        private CalculatorParameters _params { get; set; }
        private const int _scoreRows = 3;
        //all of the the dependent and independent var column names
        private string[] _colNames { get; set; }
        //all of the the dependent var column names including intercept
        private string[] _depColNames { get; set; }
        //corresponding Ix.Qx names (1 less count because no dependent var)
        private string[] _mathTerms { get; set; }
        //Q1 to Q5s
        private double [] _qs { get; set; }
        //which regression algorithm is being run?
        private string _subalgorithm { get; set; }
        //ci
        private int _confidenceInt { get; set; }
        //output
        //q6 = marginal productivity
        public double QTSlope { get; set; }
        //q6M = predicted y
        public double QTPredicted { get; set; }
        //lower 95% ci
        public double QTL { get; set; }
        //upper 95% ci
        public double QTU { get; set; }
        //used to calculate 95% prediction interval
        public double QTPI { get; set; }
        public double QTPredicted10 { get; set; }
        //lower 95% ci
        public double QTL10 { get; set; }
        //upper 95% ci
        public double QTU10 { get; set; }
        //used to calculate 95% prediction interval
        public double QTPI10 { get; set; }
        public double QTPredicted20 { get; set; }
        //lower 95% ci
        public double QTL20 { get; set; }
        //upper 95% ci
        public double QTU20 { get; set; }
        //used to calculate 95% prediction interval
        public double QTPI20 { get; set; }

        //this is asych for the calling Task.WhenAll
        //but does not necessarily need internal asych awaits
        public async Task RunAlgorithmAsync(List<List<double>> data)
        {
            try
            {
                //minimal data requirement is first five cols
                if (_colNames.Count() < 5
                    || _mathTerms.Count() == 0)
                {
                    ErrorMessage = "Regression requires at least one dependent variable and one independent variable.";
                    return;
                }
                if (data.Count() < 5)
                {
                    //185 same as other analysis
                    ErrorMessage = "Regression requires at least 2 rows of observed data and 3 rows of scoring data.";
                    return;
                }
                //convert data to a Math.Net Matrix
                //v185 uses same ci technique as algos 2,3 and 4 -last 3 rows are used to generate ci
                List<List<double>> dataci = data.Skip(data.Count - _scoreRows).ToList();
                data.Reverse();
                List<List<double>> dataobs = data.Skip(_scoreRows).ToList();
                dataobs.Reverse();
                //actual observed values
                Vector<double> y = Shared.GetYData(dataobs);
                Matrix<double> x = Shared.GetDoubleMatrix(dataobs, _colNames, _depColNames);
                Matrix<double> ci = Shared.GetDoubleMatrix(dataci, _colNames, _depColNames);
                
                //model expected values - get the coefficents
                //use normal equations regression
                Vector<double> p = MultipleRegression.NormalEquations(x, y);
                //but note that this runs without errors in more cases but still does not give good results
                //Vector<double> p = MultipleRegression.QR(x, y);
                
                if (p.Count() != ci.Row(_scoreRows - 1).Count())
                {
                    //185 same as other analysis
                    ErrorMessage = "The scoring and training datasets have different numbers of columns.";
                    return;
                }
                //get the predicted yhats
                Vector<double> yhat = GetYHatandSetQTPred(y.Count, x, p, ci.Row(_scoreRows - 1).ToArray());
                //get the durbin-watson d statistic
                double d = GetDurbinWatson(y, yhat);
                double SSE = 0;
                //sum of the square of the error (between the predicted, p, and observed, y);
                SSE = Distance.SSD(yhat, y);
                double rSquared = GoodnessOfFit.RSquared(yhat, y);
                //sum of the square of the regression (between the predicted, p, and observed mean, statsY.Mean);
                double SSR = 0;
                for (int i = 0; i < yhat.Count(); i++)
                {
                    SSR += Math.Pow((yhat[i] - y.Mean()), 2);
                }
                //set joint vars properties
                //degrees freedom
                double dfR = x.ColumnCount - 1;
                double dfE = x.RowCount - x.ColumnCount;
                int idfR = x.ColumnCount - 1;
                int idfE = x.RowCount - x.ColumnCount;
                double s2 = SSE / dfE;
                double s = Math.Sqrt(s2);
                double MSR = SSR / dfR;
                double MSE = SSE / dfE;
                double FValue = MSR / MSE;
                double adjRSquared = 1 - ((x.RowCount - 1) * (MSE / (SSE + SSR)));
                double pValue = Shared.GetPValueForFDist(idfR, idfE, FValue);
                
                //correct 2 tailed t test
                //double TCritialValue = ExcelFunctions.TInv(0.05, idfE);
                //so do this
                double dbCI = CalculatorHelpers.GetConfidenceIntervalProb(_confidenceInt);
                double tCriticalValue = ExcelFunctions.TInv(dbCI, idfE);
                //set each coeff properties
                //coeffs st error
                //use matrix math to get the standard error of coefficients
                Matrix<double> xt = x.Transpose();
                //matrix x'x
                Matrix<double> xx = xt.Multiply(x);
                Matrix<double> xxminus1 = xx.Inverse();

                double sxx = 0;
                double[] xiSE = new double[x.ColumnCount];
                //coeff tstats
                double[] xiT = new double[x.ColumnCount];
                //lower value for pvalue
                double[] xiP = new double[x.ColumnCount];
                for (int i = 0; i < x.ColumnCount; i++)
                {
                    //use the matrix techniques shown on p 717 of Mendenhall and Sincich
                    sxx = s * Math.Sqrt(xxminus1.Column(i)[i]);
                    xiSE[i] = sxx;
                    xiT[i] = p[i] / sxx;
                    xiP[i] = Shared.GetPValueForTDist(idfE, xiT[i], 0, 1);
                        
                }
                double FCriticalValue = 0;
                string FGreaterFCritical = string.Empty;
                if (_subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm8.ToString())
                {
                    //anova regression
                    //anova critical fvalue test
                    //FCriticalValue = ExcelFunctions.FInv(1 - _confidenceInt, idfR, idfE);
                    FCriticalValue = ExcelFunctions.FInv(dbCI, idfR, idfE);
                    FGreaterFCritical = (FValue > FCriticalValue) ? "true" : "false";
                    SetAnovaIntervals(0, p, xiSE, tCriticalValue);
                    SetAnovaIntervals(1, p, xiSE, tCriticalValue);
                    SetAnovaIntervals(2, p, xiSE, tCriticalValue);
                }
                else
                {
                    //set QTM ci and pi intervals
                    SetQTIntervals(0, s, xxminus1, ci.Row(_scoreRows - 1).ToArray(), p, tCriticalValue);
                    SetQTIntervals(1, s, xxminus1, ci.Row(_scoreRows - 2).ToArray(), p, tCriticalValue);
                    SetQTIntervals(2, s, xxminus1, ci.Row(_scoreRows - 3).ToArray(), p, tCriticalValue);
                }
                //add the data to a string builder
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("regression results");
                //dep var has to be in the 4 column always
                string sLine = string.Concat("dependent variable:  ", _colNames[3]);
                sb.AppendLine(sLine);
                string[] cols = new string[]{"source", "df", "SS", "MS"};
                sb.AppendLine(Shared.GetLine(cols, true));
                cols = new string[] { "model", dfR.ToString("F0"), SSR.ToString("F4"), MSR.ToString("F4")};
                sb.AppendLine(Shared.GetLine(cols, false));
                cols = new string[] { "error  ", dfE.ToString("F0"), SSE.ToString("F4"), MSE.ToString("F4")};
                sb.AppendLine(Shared.GetLine(cols, false));
                cols = new string[] { "total    ", (dfR + dfE).ToString("F0"), (SSR + SSE).ToString("F4")};
                sb.AppendLine(Shared.GetLine(cols, false));
                sb.AppendLine(string.Empty);
                cols = new string[] { "R-squared", rSquared.ToString("F4"), "Adj R-squared", adjRSquared.ToString("F4")};
                sb.AppendLine(Shared.GetLine(cols, false));
                cols = new string[] { "F value", FValue.ToString("F4"), "prob > F", pValue.ToString("F4")};
                sb.AppendLine(Shared.GetLine(cols, false));
                sb.AppendLine(string.Empty);
                cols = new string[] { GetName("variable"), "coefficient", "stand error", "T-ratio", "prob > T" };
                sb.AppendLine(Shared.GetLine(cols, true));    
                for (int i = 0; i < p.Count(); i++)
                {
                    if (i == 0)
                    {
                        cols = new string[] { GetName(_depColNames[i]), p[i].ToString("F5"), xiSE[i].ToString("F4"), xiT[i].ToString("F4"), xiP[i].ToString("F4") };
                        sb.AppendLine(Shared.GetLine(cols, false));
                    }
                    else
                    {
                        cols = new string[] { GetName(_depColNames[i]), p[i].ToString("F5"), xiSE[i].ToString("F4"), xiT[i].ToString("F4"), xiP[i].ToString("F4") };
                        sb.AppendLine(Shared.GetLine(cols, false));
                    }
                }
                cols = new string[] { "durbin-watson: ", d.ToString("F4")};
                sb.AppendLine(Shared.GetLine(cols, false));
                if (_subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm8.ToString())
                {
                    cols = new string[] { "F Critical Value", FCriticalValue.ToString("F5"), "F > F Critical", FGreaterFCritical };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    cols = new string[] { "estimate", "predicted", string.Concat("lower ", _confidenceInt.ToString(), "%"), string.Concat("upper ", _confidenceInt.ToString(), "%") };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    cols = new string[] { "Col 0 Mean CI ", QTPredicted.ToString("F4"), QTL.ToString("F4"), QTU.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "Col 1 - 0 Mean CI ", QTPredicted10.ToString("F4"), QTL10.ToString("F4"), QTU10.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "Col 2 - 0 Mean CI ", QTPredicted20.ToString("F4"), QTL20.ToString("F4"), QTU20.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                }
                else
                {
                    cols = new string[] { "estimate", "predicted", string.Concat("lower ", _confidenceInt.ToString(), "%"), string.Concat("upper ", _confidenceInt.ToString(), "%") };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    cols = new string[] { "QTM CI ", QTPredicted.ToString("F4"), QTL.ToString("F4"), QTU.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "QTM PI ", QTPredicted.ToString("F4"), (QTPredicted - QTPI).ToString("F4"), (QTPredicted + QTPI).ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    string sRow = string.Concat("row ", data.Count - 2);
                    cols = new string[] { sRow };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    cols = new string[] { "CI ", QTPredicted10.ToString("F4"), QTL10.ToString("F4"), QTU10.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "PI ", QTPredicted10.ToString("F4"), (QTPredicted10 - QTPI10).ToString("F4"), (QTPredicted10 + QTPI10).ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    sRow = string.Concat("row ", data.Count - 1);
                    cols = new string[] { sRow };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    cols = new string[] { "CI ", QTPredicted20.ToString("F4"), QTL20.ToString("F4"), QTU20.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "PI ", QTPredicted20.ToString("F4"), (QTPredicted20 - QTPI20).ToString("F4"), (QTPredicted20 + QTPI20).ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                }
                if (this.MathResult.ToLower().StartsWith("http"))
                {
                    string sError = string.Empty;
                    bool bHasSaved = CalculatorHelpers.SaveTextInURI(
                        _params.ExtensionDocToCalcURI, sb.ToString(), this.MathResult, out sError);
                    if (!string.IsNullOrEmpty(sError))
                    {
                        this.MathResult += sError;
                    }
                }
                else
                {
                    this.MathResult = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
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
        
        
        private void AddQsToData(double[,] problemData)
        {
            //replace the first row of problemData with _qs
            int numCols = problemData.GetUpperBound(1) + 1;
            for (int i = 0; i < numCols; i++)
            {
                //qs.count is one based and i is zero based
                if (_qs.Count() > i)
                {
                    problemData[0, i] = _qs[i];
                }
            }
        }
        private Vector<double> GetYHatandSetQTPred(int yCount, Matrix<double> x, Vector<double> p, double[] qs)
        {
            double yPi = 0;
            Vector<double> yhat = Vector<double>.Build.Dense(yCount);
            //yhat = b0 + (x.Column(1).Multiply(b1));
            for (int i = 0; i < x.RowCount; i++)
            {
                //standard multiple regression formula varies with number of coefficients in p
                if (p.Count == 2)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]);
                    yhat[i] = yPi;
                    //first column in colnames is y col
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]);
                    }
                }
                else if (p.Count == 3)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]);
                    }
                }
                else if (p.Count == 4)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]);
                    }
                }
                else if (p.Count == 5)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3]) + (x.Column(4)[i] * p[4]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4]);
                    }
                }
                else if (p.Count == 6)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3]) + (x.Column(4)[i] * p[4]) + (x.Column(5)[i] * p[5]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                            + (p[5] * qs[5]);
                    }
                }
                else if (p.Count == 7)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3])
                        + (x.Column(4)[i] * p[4]) + (x.Column(5)[i] * p[5]) + (x.Column(6)[i] * p[6]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                            + (p[5] * qs[5]) + (p[6] * qs[6]);
                    }
                }
                else if (p.Count == 8)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3])
                        + (x.Column(4)[i] * p[4]) + (x.Column(5)[i] * p[5]) + (x.Column(6)[i] * p[6])
                        + (x.Column(7)[i] * p[7]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                            + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]);
                    }
                }
                else if (p.Count == 9)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3])
                        + (x.Column(4)[i] * p[4]) + (x.Column(5)[i] * p[5]) + (x.Column(6)[i] * p[6])
                        + (x.Column(7)[i] * p[7]) + (x.Column(8)[i] * p[8]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                            + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]) + (p[8] * qs[8]);
                    }

                }
                else if (p.Count == 10)
                {
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3])
                        + (x.Column(4)[i] * p[4]) + (x.Column(5)[i] * p[5]) + (x.Column(6)[i] * p[6])
                        + (x.Column(7)[i] * p[7]) + (x.Column(8)[i] * p[8]) + (x.Column(9)[i] * p[9]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                            + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]) + (p[8] * qs[8])
                            + (p[9] * qs[9]);
                    }
                }
                else if (p.Count == 11)
                {
                    //10 explanatory variable limit
                    //simple linear regression
                    yPi = p[0] + (x.Column(1)[i] * p[1]) + (x.Column(2)[i] * p[2]) + (x.Column(3)[i] * p[3])
                        + (x.Column(4)[i] * p[4]) + (x.Column(5)[i] * p[5]) + (x.Column(6)[i] * p[6])
                        + (x.Column(7)[i] * p[7]) + (x.Column(8)[i] * p[8]) + (x.Column(9)[i] * p[9])
                        + (x.Column(10)[i] * p[10]);
                    yhat[i] = yPi;
                    if (i == 0)
                    {
                        QTPredicted = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                            + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]) + (p[8] * qs[8])
                            + (p[9] * qs[9]) + (p[10] * qs[10]);
                    }
                }
            }
            return yhat;
        }
        
        private double GetQTM(double[] qs, Vector<double> p)
        {
            double yPi = 0;
            //standard multiple regression formula varies with number of coefficients in p
            if (p.Count == 2)
            {
                yPi = p[0] + (p[1] * qs[1]);
            }
            else if (p.Count == 3)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]);
            }
            else if (p.Count == 4)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]);
            }
            else if (p.Count == 5)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4]);
            }
            else if (p.Count == 6)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                        + (p[5] * qs[5]);
            }
            else if (p.Count == 7)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                        + (p[5] * qs[5]) + (p[6] * qs[6]);
            }
            else if (p.Count == 8)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                        + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]);
            }
            else if (p.Count == 9)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                        + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]) + (p[8] * qs[8]);

            }
            else if (p.Count == 10)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                        + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]) + (p[8] * qs[8])
                        + (p[9] * qs[9]);
            }
            else if (p.Count == 11)
            {
                yPi = p[0] + (p[1] * qs[1]) + (p[2] * qs[2]) + (p[3] * qs[3]) + (p[4] * qs[4])
                        + (p[5] * qs[5]) + (p[6] * qs[6]) + (p[7] * qs[7]) + (p[8] * qs[8])
                        + (p[9] * qs[9]) + (p[10] * qs[10]);
            }
            return yPi;
        }
        private double GetDurbinWatson(Vector<double> y, Vector<double> yhat)
        {
            double d = 0;
            double residDenominator = 0;
            double residNumerator = 0;
            double residminus1 = 0;
            for (int i = 0; i < yhat.Count(); i++)
            {
                if (i == 0)
                {
                    residminus1 = y[i] - yhat[i];
                    residDenominator = Math.Pow(residminus1, 2);
                }
                else
                {
                    residNumerator += Math.Pow((y[i] - yhat[i]) - residminus1, 2);
                    residminus1 = y[i] - yhat[i];
                    residDenominator += Math.Pow(residminus1, 2);
                }
            }
            d = residNumerator / residDenominator;
            return d;
        }
        private void SetQTIntervals(int q6Index, double s,
            Matrix<double> xminus1, double[] qs, Vector<double> p, double tCriticalValue)
        {
            //the ci is proofed using matrix math on page 722 of mendenhall
            //and the example 1 results are proofed using page 169 of same reference
            //q[0] is the dep var
            //change it to an intercept
            qs[0] = 1.000;
            //ci for qs
            Vector<double> aT = Vector<double>.Build.Dense(qs);
            //Vector<double> aT = Vector<double>.Build.Dense(_qs);
            //at[0] is 0 but needs to be 1 (b0 * 1 = b))
            //returns one row of matrix
            Vector<double> aTxxminus1 = xminus1.Multiply(aT.ToRowMatrix().Row(0));
            //returns one value in vector
            double aP = 0;
            for (var i = 0; i < aTxxminus1.Count(); i++)
            {
                //can't multiply a row vector by a column vector
                aP += aTxxminus1[i] * aT[i];

            }
            
            //ci factor
            double aFactor = tCriticalValue * s * Math.Sqrt(aP);
            if (q6Index == 0)
            {
                QTPredicted = GetQTM(qs, p);
                QTL = QTPredicted - aFactor;
                QTU = QTPredicted + aFactor;
                //pi factor
                QTPI = tCriticalValue * s * Math.Sqrt(1 + aP);
            }
            else if (q6Index == 1)
            {
                QTPredicted10 = GetQTM(qs, p);
                QTL10 = QTPredicted10 - aFactor;
                QTU10 = QTPredicted10 + aFactor;
                //pi factor
                QTPI10 = tCriticalValue * s * Math.Sqrt(1 + aP);
            }
            else if (q6Index == 2)
            {
                QTPredicted20 = GetQTM(qs, p);
                QTL20 = QTPredicted20 - aFactor;
                QTU20 = QTPredicted20 + aFactor;
                //pi factor
                QTPI20 = tCriticalValue * s * Math.Sqrt(1 + aP);
            }
        }
        private void SetAnovaIntervals(int q6Index, Vector<double> p,
            double[] xiSE, double tCriticalValue)
        {
            double aFactor = tCriticalValue * xiSE[q6Index];;
            if (q6Index == 0)
            {
                QTPredicted = p[q6Index];
                QTL = QTPredicted - aFactor;
                QTU = QTPredicted + aFactor;
            }
            else if (q6Index == 1)
            {
                QTPredicted10 = p[q6Index];
                QTL10 = QTPredicted10 - aFactor;
                QTU10 = QTPredicted10 + aFactor;
            }
            else if (q6Index == 2)
            {
                QTPredicted20 = p[q6Index];
                QTL20 = QTPredicted20 - aFactor;
                QTU20 = QTPredicted20 + aFactor;
            }
        }
        //185 deprecated in favor of using last 3 lines of dataset (same as other algos)
        //private void SetQTIntervals(int q6Index, int dfE, double s, 
        //    Matrix<double> xminus1, double[] qs)
        //{
        //    //the ci is proofed using matrix math on page 722 of mendenhall
        //    //and the example 1 results are proofed using page 169 of same reference

        //    //ci for q6
        //    Vector<double> aT = Vector<double>.Build.Dense(_qs);
        //    //at[0] is 0 but needs to be 1 (b0 * 1 = b))
        //    //returns one row of matrix
        //    Vector<double> aTxxminus1 = xminus1.Multiply(aT.ToRowMatrix().Row(0));
        //    //returns one value in vector
        //    double aP = 0;
        //    for (var i = 0; i < aTxxminus1.Count(); i++)
        //    {
        //        //can't multiply a row vector by a column vector
        //        aP += aTxxminus1[i] * aT[i];

        //    }
        //    double t025 = Shared.GetTStat95(dfE);
        //    //ci factor
        //    double aFactor = t025 * s * Math.Sqrt(aP);
        //    if (q6Index == 0)
        //    {
        //        QTL = QTPredicted - aFactor;
        //        QTU = QTPredicted + aFactor;
        //        //pi factor
        //        QTPI = t025 * s * Math.Sqrt(1 + aP);
        //    }
        //    else if (q6Index == 1)
        //    {
        //        QTPredicted10 = QTPredicted + (QTPredicted * 0.1);
        //        QTL10 = QTPredicted10 - aFactor;
        //        QTU10 = QTPredicted10 + aFactor;
        //        //pi factor
        //        QTPI10 = t025 * s * Math.Sqrt(1 + aP);
        //    }
        //    else if (q6Index == 2)
        //    {
        //        QTPredicted20 = QTPredicted + (QTPredicted * 0.2);
        //        QTL20 = QTPredicted20 - aFactor;
        //        QTU20 = QTPredicted20 + aFactor;
        //        //pi factor
        //        QTPI20 = t025 * s * Math.Sqrt(1 + aP);
        //    }
        //    if (q6Index == 0)
        //    {
        //        double[] q10s = new double[qs.Count()];
        //        for (int i = 0; i < qs.Count(); i++)
        //        {
        //            if (i == 0)
        //            {
        //                //keep a constant intercept
        //                q10s[0] = 1;
        //            }
        //            else
        //            {
        //                q10s[i] = qs[i] + (qs[i] * 0.10);
        //            }
        //        }
                    
        //        //add 10% to qs and generate new intervals
        //        SetQTIntervals(1, dfE, s, xminus1, q10s);
        //        //add 20% to qs and generate new intervals
        //        double[] q20s = new double[qs.Count()];
        //        for (int i = 0; i < qs.Count(); i++)
        //        {
        //            if (i == 0)
        //            {
        //                //keep a constant intercept
        //                q20s[0] = 1;
        //            }
        //            else
        //            {
        //                q20s[i] = qs[i] + (qs[i] * 0.20);
        //            }
        //        }
        //        SetQTIntervals(2, dfE, s, xminus1, q20s);
        //    }

        //}
    }
}
