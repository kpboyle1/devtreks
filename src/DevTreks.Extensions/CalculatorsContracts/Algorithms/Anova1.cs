using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//probab risk
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		ANOVA algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, November
    ///References:	CTA algo1 subalgo8
    ///</summary>
    public class Anova1 : Calculator1
    {
        public Anova1()
            : base()
        {
        }
        /// <summary>
        /// Initialize the ANOVA algorithm
        /// </summary>
        /// <param name="mathTerms">Math Expression terms, in format Ix.Qx. Potential use in vector math.</param>
        /// <param name="colNames">All column names for data vectors. Identifies vectors to exclude and include.</param>
        /// <param name="depColNames">Column names adjusted for independent var data vectors. Includes intercept term, excludes dep var term.</param>
        /// <param name="totalsNeeded">Number of totals needed by calling procedure</param>
        ///is in the mathexpress (i.e. mathexpress.contains("q1")</param>
        public Anova1(string label, string[] mathTerms, string[] colNames, string[] depColNames,
            int totalsNeeded, string subalgorithm, int confidenceInterval, CalculatorParameters calcParams) 
            : base()
        {
            _colNames = colNames;
            _depColNames = depColNames;
            _mathTerms = mathTerms;
            _subalgorithm = subalgorithm;
            _confidenceInt = confidenceInterval;
            //188 anors can run algos and pass back totals to calling procedure
            _totalsNeeded = totalsNeeded;
            Label = label;
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
        private double[] _qs { get; set; }
        //which regression algorithm is being run?
        private string _subalgorithm { get; set; }
        //ci
        private int _confidenceInt { get; set; }
        //how many totals need to be returned to calling procedure?
        private int _totalsNeeded { get; set; }
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
                    ErrorMessage = "Complete randomized anova requires at least 1 dependent variable and 1 independent variable. Randomized block and factorial anovas require at least 2 independent variables.";
                    return;
                }
                if (data.Count() < 5)
                {
                    //185 same as other analysis
                    ErrorMessage = "Anova requires at least 2 rows of observed data and 3 rows of scoring data.";
                    return;
                }
                
                //convert data to a Math.Net Matrix
                //last 3 rows are used to generate ci
                List<List<double>> dataci = data.Skip(data.Count - _scoreRows).ToList();
                data.Reverse();
                List<List<double>> dataobs = data.Skip(_scoreRows).ToList();
                dataobs.Reverse();
                //actual observed values
                Vector<double> y = Shared.GetYData(dataobs);
                //treatments or factor1 levels
                Matrix<double> treatments = Shared.GetDistinctMatrix(dataobs, 1);
                //206 condition added due to M and E dataset indexing
                _totalsNeeded = treatments.ColumnCount;
                double tDF = treatments.ColumnCount - 1;
                //step 1. get total of observed data
                double yTotal = y.Sum();
                //step 2. get total of observed data squared
                double yTotal2 = y.PointwisePower(2).Sum();
                //step 3. set CM
                double CM = Math.Pow(yTotal, 2) / y.Count();
                //step 4. treatments and blocks
                double SSTotal = yTotal2 - CM;
                //add the data to a string builder
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("anova results");
                //5 col array
                string[] cols = new string[] { "source", "df", "SS", "MS", "F" };
                sb.AppendLine(Shared.GetLine(cols, true));
                List<List<double>> totals = new List<List<double>>(_totalsNeeded);
                //min treatment block required
                bool bIsBlock = (_depColNames.Contains("treatment") && _depColNames.Contains("block")) ? true : false;
                //min 2 factors required
                bool bIsFactorial = (_depColNames.Contains("factor1") && _depColNames.Contains("factor2")) ? true : false;
                bool bIsComplete = (bIsBlock == false && bIsFactorial == false) ? true : false;
                if (bIsComplete)
                {
                    double eDF = y.Count() - treatments.ColumnCount;
                    //step 5.treatments (correct to divide by rows)
                    double SST = ((treatments.ColumnSums().PointwisePower(2).Sum()) / treatments.RowCount) - CM;
                    
                    //step 6. error
                    double SSE = SSTotal - SST;
                    
                    //step 7. mean treatment
                    double MST = SST / tDF;
                    
                    //step 8. mean error
                    double MSE = SSE / (y.Count() - treatments.ColumnCount);
                    
                    //step 9. F treatments
                    double FT = MST / MSE;
                    
                    //tests
                    double s = Math.Pow(MSE, 0.5);
                    //correct 2 tailed t test
                    int itDF = CalculatorHelpers.ConvertStringToInt(tDF.ToString());
                    int ieDF = CalculatorHelpers.ConvertStringToInt(eDF.ToString());

                    double dbCI = CalculatorHelpers.GetConfidenceIntervalProb(_confidenceInt);
                    //TINV divides dbCI by 2 to get student t
                    double tCriticalValue = ExcelFunctions.TInv(dbCI, ieDF);
                    //prevents an error in Finv
                    if (itDF == 0) itDF = 1;
                    double FCriticalTValue = ExcelFunctions.FInv(dbCI, itDF, ieDF);
                    string FTGreaterFCritical = (FT > FCriticalTValue) ? "true" : "false";
                    for (int i = 0; i < _totalsNeeded; i++)
                    {
                        //206 condition added due to M and E dataset indexing
                        if (i < treatments.ColumnCount)
                        {
                            SetAnovaIntervals(i, totals, treatments, tCriticalValue, s,
                                CalculatorHelpers.ConvertStringToDouble(treatments.RowCount.ToString()),
                                FT, FCriticalTValue, bIsComplete);
                        }
                    }
                    this.DataToAnalyze.Add(Label, totals);
                    ////add the data to a string builder
                    
                    cols = new string[] { "treats", itDF.ToString("F0"), SST.ToString("F4"), MST.ToString("F4"), FT.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    
                    cols = new string[] { "error  ", ieDF.ToString("F0"), SSE.ToString("F4"), MSE.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "total    ", (y.Count() - 1).ToString("F0"), (SSTotal).ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));

                    cols = new string[] { string.Concat("F Crit ", "treats"), FCriticalTValue.ToString("F5"), "F > F Critical", FTGreaterFCritical };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    
                }
                else
                {
                    //observations per cell for factorials (data[0] if first row of data)
                    double r = Shared.GetObservationsPerCell(dataobs, 1, data[0].ElementAt(1), data[0].ElementAt(2));
                    //blocks or factor2 levels
                    Matrix<double> blocks = Shared.GetDistinctMatrix(dataobs, 2);
                    double bDF = blocks.ColumnCount - 1;
                    double eDF = y.Count() - treatments.ColumnCount - blocks.ColumnCount + 1;
                    if (bIsFactorial)
                    {
                        eDF = (treatments.ColumnCount * blocks.ColumnCount) * (r - 1);
                    }
                    //factorial interaction df
                    double tbDF = tDF * bDF;
                    
                    //step 5.treatments (correct to divide by r)
                    double SST = ((treatments.ColumnSums().PointwisePower(2).Sum()) / (blocks.ColumnCount * r)) - CM;
                    //step 6. blocks
                    double SSB = ((blocks.ColumnSums().PointwisePower(2).Sum()) / (treatments.ColumnCount * r)) - CM;
                    //factor level interaction
                    double SSFL = 0;
                    //step 7. error
                    double SSE = 0;
                    if (bIsFactorial)
                    {
                        double totalinteraction = Shared.GetTotalInteraction(blocks, r);
                        //watch block.colcount for 2 x 3 factorials
                        SSFL = (totalinteraction / r) - SST - SSB - CM;
                        //step 7. error
                        SSE = SSTotal - SST - SSB - SSFL;
                    }
                    else
                    {
                        //step 7. error
                        SSE = SSTotal - SST - SSB;
                    }
                    //step 8. mean treatment
                    double MST = SST / tDF;
                    //step 9. mean block
                    double MSB = SSB / bDF;
                    //step 10. mean error
                    double MSE = SSE / eDF;
                    double MSFL = SSFL / tbDF;
                    //step 11. F treatments
                    double FT = MST / MSE;
                    //step 12. F blocks
                    double FB = MSB / MSE;
                    double FTB = MSFL / MSE;
                    //tests
                    double s = Math.Pow(MSE, 0.5);
                    //correct 2 tailed t test
                    int itDF = CalculatorHelpers.ConvertStringToInt(tDF.ToString());
                    int ibDF = CalculatorHelpers.ConvertStringToInt(bDF.ToString());
                    int itbDF = CalculatorHelpers.ConvertStringToInt(tbDF.ToString());
                    int ieDF = CalculatorHelpers.ConvertStringToInt(eDF.ToString());

                    double dbCI = CalculatorHelpers.GetConfidenceIntervalProb(_confidenceInt);
                    //TINV divides dbCI by 2 to get student t
                    double tCriticalValue = ExcelFunctions.TInv(dbCI, ieDF);
                    //prevents an error in Finv
                    if (itDF == 0) itDF = 1;
                    double FCriticalTValue = ExcelFunctions.FInv(dbCI, itDF, ieDF);
                    string FTGreaterFCritical = (FT > FCriticalTValue) ? "true" : "false";
                    //prevents an error in Finv
                    if (ibDF == 0) ibDF = 1;
                    double FCriticalBValue = ExcelFunctions.FInv(dbCI, ibDF, ieDF);
                    string FBGreaterFCritical = (FB > FCriticalBValue) ? "true" : "false";
                    //prevents an error in Finv
                    if (itbDF == 0) itbDF = 1;
                    double FCriticalTBValue = ExcelFunctions.FInv(dbCI, itbDF, ieDF);
                    string FTBGreaterFCritical = (FTB > FCriticalTBValue) ? "true" : "false";
                    //List<List<double>> totals = new List<List<double>>(_totalsNeeded);
                    if (bIsFactorial)
                    {
                        //unless custom stylesheets are developed, can only display factor 1 - factor 2 diffs
                        //build a matrix equivalent to treatments -1 row, variable cols
                        Matrix<double> torbs = Matrix<double>.Build.Dense(1, _totalsNeeded);
                        List<double> mrow = new List<double>(_totalsNeeded);
                        for (int i = 0; i < _totalsNeeded; i++)
                        {
                            double cellMean = Shared.GetMeanPerCell(dataobs, 1, 2, i, i, r);
                            mrow.Add(cellMean);
                        }
                        torbs.SetRow(0, mrow.ToArray());
                        for (int i = 0; i < _totalsNeeded; i++)
                        {
                            //206 condition added due to M and E dataset indexing
                            if (i < treatments.ColumnCount)
                            {
                                //treatments
                                SetAnovaIntervals(i, totals, torbs, tCriticalValue, s,
                                CalculatorHelpers.ConvertStringToDouble(blocks.ColumnCount.ToString()),
                                FT, FCriticalTValue, bIsComplete);
                            }
                        }
                        //cell1Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 0, r);
                        //cell2Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 1, r);
                        //SetAnovaIntervals2(1, cell1Mean, cell2Mean, tCriticalValue, s, r);
                        //cell1Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 0, r);
                        //cell2Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 2, 0, r);
                        //SetAnovaIntervals2(2, cell1Mean, cell2Mean, tCriticalValue, s, r);
                        //double cell1Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 0, r);
                        //double cell2Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 1, 0, r);
                        //SetAnovaIntervals2(0, cell1Mean, cell2Mean, tCriticalValue, s, r);
                        //cell1Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 0, r);
                        //cell2Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 1, r);
                        //SetAnovaIntervals2(1, cell1Mean, cell2Mean, tCriticalValue, s, r);
                        //cell1Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 0, 0, r);
                        //cell2Mean = Shared.GetMeanPerCell(dataobs, 1, 2, 2, 0, r);
                        //SetAnovaIntervals2(2, cell1Mean, cell2Mean, tCriticalValue, s, r);
                    }
                    else
                    {
                        //unless custom stylesheets are developed, need to only display treatment diffs
                        for (int i = 0; i < _totalsNeeded; i++)
                        {
                            //206 condition added due to M and E dataset indexing
                            if (i < treatments.ColumnCount)
                            {
                                //treatments
                                SetAnovaIntervals(i, totals, treatments, tCriticalValue, s,
                                CalculatorHelpers.ConvertStringToDouble(blocks.ColumnCount.ToString()),
                                FT, FCriticalTValue, bIsComplete);
                                ////blocks
                                //SetAnovaIntervals(i, totals, blocks, tCriticalValue, s, 
                                //    CalculatorHelpers.ConvertStringToDouble(treatments.ColumnCount.ToString()),
                                //    FB, FCriticalBValue, bIsComplete);
                                //interactions
                            }

                        }
                        
                    }
                    this.DataToAnalyze.Add(Label, totals);
                    string sTreats = "treats ";
                    string sBlocks = "blocks ";
                    if (bIsFactorial)
                    {
                        sTreats = "factor1 ";
                        sBlocks = "factor2 ";
                    }
                    ////add the data to a string builder
                    //StringBuilder sb = new StringBuilder();
                    //sb.AppendLine("anova results");
                    
                    cols = new string[] { sTreats, itDF.ToString("F0"), SST.ToString("F4"), MST.ToString("F4"), FT.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { sBlocks, bDF.ToString("F0"), SSB.ToString("F4"), MSB.ToString("F4"), FB.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    if (bIsFactorial)
                    {
                        cols = new string[] { "interacts  ", tbDF.ToString("F0"), SSFL.ToString("F4"), MSFL.ToString("F4"), FTB.ToString("F4") };
                        sb.AppendLine(Shared.GetLine(cols, false));
                    }
                    cols = new string[] { "error  ", ieDF.ToString("F0"), SSE.ToString("F4"), MSE.ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));
                    cols = new string[] { "total    ", (y.Count() - 1).ToString("F0"), (SSTotal).ToString("F4") };
                    sb.AppendLine(Shared.GetLine(cols, false));

                    cols = new string[] { string.Concat("F Crit ", sTreats), FCriticalTValue.ToString("F5"), "F > F Critical", FTGreaterFCritical };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    cols = new string[] { string.Concat("F Crit ", sBlocks), FCriticalBValue.ToString("F5"), "F > F Critical", FBGreaterFCritical };
                    sb.AppendLine(Shared.GetLine(cols, true));
                    if (bIsFactorial)
                    {
                        cols = new string[] { "F Crit Interacts", FCriticalTBValue.ToString("F5"), "F > F Critical", FTBGreaterFCritical };
                        sb.AppendLine(Shared.GetLine(cols, true));
                    }
                    
                }
                cols = new string[] { "estimate", "mean diff", string.Concat("lower ", _confidenceInt.ToString(), "%"), string.Concat("upper ", _confidenceInt.ToString(), "%") };
                sb.AppendLine(Shared.GetLine(cols, true));
                //same report for calculator and analyzer
                for (int i = 0; i < _totalsNeeded; i++)
                {
                    if (totals[i].Count >= 5)
                    {
                        if (i == 0)
                        {
                            QTPredicted = totals[i].ElementAt(0);
                            QTL = QTPredicted - totals[i].ElementAt(4);
                            QTU = QTPredicted + totals[i].ElementAt(4);
                            cols = new string[] { "Treat 1 Mean ", QTPredicted.ToString("F4"), QTL.ToString("F4"), QTU.ToString("F4") };
                            sb.AppendLine(Shared.GetLine(cols, false));

                        }
                        else
                        {
                            QTPredicted = totals[i].ElementAt(1);
                            QTL = QTPredicted - totals[i].ElementAt(2);
                            QTU = QTPredicted + totals[i].ElementAt(2);
                            cols = new string[] { string.Concat("xminus1 ", i.ToString(), " "), QTPredicted.ToString("F4"), QTL.ToString("F4"), QTU.ToString("F4") };
                            sb.AppendLine(Shared.GetLine(cols, false));
                            QTPredicted = totals[i].ElementAt(3);
                            QTL = QTPredicted - totals[i].ElementAt(4);
                            QTU = QTPredicted + totals[i].ElementAt(4);
                            cols = new string[] { string.Concat("base ", i.ToString(), " "), QTPredicted.ToString("F4"), QTL.ToString("F4"), QTU.ToString("F4") };
                            sb.AppendLine(Shared.GetLine(cols, false));
                        }
                    }
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
        
        
        private void SetAnovaIntervals2(int qTIndex, double cell1Mean, double cell2Mean,
            double tCriticalValue, double s, double colcount)
        {
            double aFactor = (tCriticalValue * s) * (Math.Pow(2.000 / colcount, 0.5));
            if (qTIndex == 0)
            {
                QTPredicted = cell1Mean - cell2Mean;
                QTL = QTPredicted - aFactor;
                QTU = QTPredicted + aFactor;
            }
            else if (qTIndex == 1)
            {
                QTPredicted10 = cell1Mean - cell2Mean;
                QTL10 = QTPredicted10 - aFactor;
                QTU10 = QTPredicted10 + aFactor;
            }
            else if (qTIndex == 2)
            {
                QTPredicted20 = cell1Mean - cell2Mean;
                QTL20 = QTPredicted20 - aFactor;
                QTU20 = QTPredicted20 + aFactor;
                
            }
        }
        
        private void SetAnovaIntervals(int qTIndex,List<List<double>> totals, Matrix<double> torb,
            double tCriticalValue, double s, double rowOrColcount, double fStat, double fCritical, 
            bool isComplete)
        {
            //this passes back the uncertainty of mean, mean minus base, and mean minus x minus 1
            //the calling procedure is expected to know what's in each of the 9 list members
            List<double> total = new List<double>(6);
            //comparator
            int iComparator = 0;
            double aFactor = (tCriticalValue) * (s / Math.Pow(rowOrColcount, 0.5));
            //treatment means
            QTPredicted = torb.Column(qTIndex).Average();
            total.Add(QTPredicted);
            if (qTIndex == 0)
            {
                total.Add(fStat);
                total.Add(fCritical);
                total.Add(QTPredicted);
                total.Add(aFactor);
            }
            else
            {
                aFactor = (tCriticalValue * s) * (Math.Pow(2 * (1 / rowOrColcount), 0.5));
                //x-1 comparators treatment mean differences tn - tn-1
                iComparator = qTIndex - 1;
                if (isComplete)
                {
                    QTPredicted10 = torb.Column(qTIndex).Average() - torb.Column(iComparator).Average();
                }
                else
                {
                    QTPredicted10 = torb.Column(iComparator).Average() - torb.Column(qTIndex).Average();
                }
                total.Add(QTPredicted10);
                total.Add(aFactor);
                if (torb.ColumnCount > qTIndex)
                {
                    //base comparators mean differences tn - t0
                    iComparator = 0;
                    if (isComplete)
                    {
                        QTPredicted20 = torb.Column(qTIndex).Average() - torb.Column(iComparator).Average();
                    }
                    else
                    {
                        QTPredicted20 = torb.Column(iComparator).Average() - torb.Column(qTIndex).Average();
                    }
                    total.Add(QTPredicted20);
                    total.Add(aFactor);
                }
            }
            totals.Add(total);
        }
        
    }
}
