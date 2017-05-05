using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;

using MathNet.Numerics.LinearAlgebra;

namespace DevTreks.Extensions.ME2Statistics
{
    ///<summary>
    ///Purpose:		Run algorithms
    ///Author:		www.devtreks.org
    ///Date:		2017, April
    ///NOTES        These algorithm patterns derived directly from the equivalent code 
    ///             in the Resource Stock Calculator. They need to evolve to handle 
    ///             large numbers of algorithms.
    /// </summary> 
    public class ME2Algos : ME2Indicator
    {
        public ME2Algos()
            : base()
        {
            //class is initialized from ME2Indicator
        }
        //copy constructor
        public ME2Algos(ME2Indicator stockME)
            : base(stockME)
        {
            //stock fact
            CopyME2IndicatorsProperties(stockME);
        }
        
        public async Task<int> SetAlgoPRAStats(int index, string[] colNames,
            List<double> qTs, double[] data = null)
        {
            //probabilistic risk using montecarlo
            //if the algo is used with the label, return it as affirmation
            int algoindicator = -1;
            if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm1)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
            {
                //if its a good calc returns the string
                algoindicator = await SetPRAIndicatorStats(index, colNames, qTs, data);
            }
            return algoindicator;
        }
        
        public async Task<string> SetAlgoCorrIndicatorStats(string jdataURL, IDictionary<int, List<List<double>>> data,
            string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            string algoindicators = string.Empty;
            //init the algo using the new indicator
            if (HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                || HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
            {
                algoindicators = await SetPRACorrIndicatorStats(jdataURL, colNames, data);
            }
            return algoindicators;
        }
        public async Task<int> SetAlgoIndicatorStats1(int index, List<List<double>> data, string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            int algoindicator = -1;
            
            if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6))
            {
                //if its a good calc returns the string
                algoindicator = await SetRGRIndicatorStats(index, colNames, data);
            }
            else if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
            {
                if (colNames.Contains("treatment") || colNames.Contains("factor1"))
                {
                    //random block or random factorial
                    algoindicator = await SetANVIndicatorStats(index, colNames, data);
                }
                else
                {
                    //one way anova can use regression
                    algoindicator = await SetRGRIndicatorStats(index, colNames, data);
                }
            }
            return algoindicator;
        }
        public async Task<int> SetAlgoIndicatorStats2(int index, string[] colNames, 
            string dataURL, string jDataURL)
        {
            //if the algo is used with the label, return it as affirmation
            int algoindicator = -1;
            //assume additional algos will use this data format
            string sPlatForm = CalculatorHelpers.GetPlatform(this.CalcParameters.ExtensionDocToCalcURI, dataURL);
            if (sPlatForm == CalculatorHelpers.PLATFORM_TYPES.azure.ToString())
            {
                if (this.HasMathType(index, MATH_TYPES.algorithm4))
                {
                    //if its a good calc returns the string
                    algoindicator = await SetScriptCloudStats(index, colNames, dataURL, jDataURL);
                }
            }
            else 
            {
                if (this.HasMathType(index, MATH_TYPES.algorithm2)
                    || this.HasMathType(index, MATH_TYPES.algorithm3))
                {
                    //if its a good calc returns the string
                    algoindicator = await SetScriptWebStats(index, colNames, dataURL, jDataURL);
                }
                else if (this.HasMathType(index, MATH_TYPES.algorithm4))
                {
                    //always runs the cloud web servive (but response can vary)
                    algoindicator = await SetScriptCloudStats(index, colNames, dataURL, jDataURL);
                }
            }
            return algoindicator;
        }
        public async Task<int> SetAlgoIndicatorStats3(int index, List<List<string>> data,
            List<List<string>> colData, List<string> lines2, string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            int algoindicator = -1;
            //assume additional algos will use this data format
            if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
            {
                //if its a good exceedance probability calc returns the string
                algoindicator = await SetDRR1IndicatorStats(index, colNames, data, 
                    colData, lines2);
            }
            return algoindicator;
        }
        public async Task<int> SetAlgoIndicatorStats4(int index, List<List<string>> data,
            List<List<string>> colData, List<string> lines2, string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            int algoindicator = -1;
            //assume additional algos will use this data format
            if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm13)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm14)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm15)
                || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm16))
            {
                //if its a good exceedance probability calc returns the string
                algoindicator = await SetDRR2IndicatorStats(index, colNames, data,
                    colData, lines2);
            }
            return algoindicator;
        }
        public async Task<int> SetAlgoIndicatorCalcs(int index, List<List<double>> data)
        {
            //if the algo is used with the label, return it as affirmation
            int algoindicator = -1;
            //init the algo using the new indicator
            if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5))
            {
                DevTreks.Extensions.Algorithms.SimulatedAnnealing1 sa = InitSA1Algo(index);
                //if its a good calc returns the string
                bool bHasCalculation = await sa.RunAlgorithmAsync(data);
                if (bHasCalculation)
                {
                    algoindicator = index;
                }
                //set the results
                SetSA1AlgoRanges(index, sa);
                //start with any error messages
                this.ME2Indicators[0].IndMathResult += sa.ErrorMessage;
            }
            else if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7))
            {
                DevTreks.Extensions.Algorithms.NeuralNetwork1 nn = InitNN1Algo(index);
                //run the simulation
                bool bHasCalculation = await nn.RunAlgorithmAsync(data);
                if (bHasCalculation)
                {
                    algoindicator = index;
                }
                //set the results
                SetNN1AlgoRanges(index, nn);
                //start with any error messages
                this.ME2Indicators[0].IndMathResult += nn.ErrorMessage;
            }
            return algoindicator;
        }
        
        
        private async Task<int> SetPRAIndicatorStats(int index, string[] colNames,
            List<double> qTs, double[] data = null)
        {
            int algoIndicator = -1;
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            if (index == 0
                && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(index);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[0].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                        this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                //label has to be [0]
                FillBaseIndicator(pra.IndicatorQT, 0, sLowerCI, sUpperCI);
            }
            else if (index == 1
                && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(1);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[1].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 1, sLowerCI, sUpperCI);
            }
            else if (index == 2
                && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(2);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[2].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 2, sLowerCI, sUpperCI);
            }
            else if (index == 3
                && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(3);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[3].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 3, sLowerCI, sUpperCI);
            }
            else if (index == 4
                && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(4);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[4].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 4, sLowerCI, sUpperCI);
            }
            else if (index == 5
                && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(5);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[5].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 5, sLowerCI, sUpperCI);
            }
            else if (index == 6
                && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(6);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[6].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 6, sLowerCI, sUpperCI);
            }
            else if (index == 7
                && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(7);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[7].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 7, sLowerCI, sUpperCI);
            }
            else if (index == 8
                && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(8);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[8].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 8, sLowerCI, sUpperCI);
            }
            else if (index == 9
                && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(9);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[9].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 9, sLowerCI, sUpperCI);
            }
            else if (index == 10
                && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(9);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[10].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 10, sLowerCI, sUpperCI);
            }
            else if (index == 11
                && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(11);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[11].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 11, sLowerCI, sUpperCI);
            }
            else if (index == 12
                && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(12);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[12].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 12, sLowerCI, sUpperCI);
            }
            else if (index == 13
                && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(13);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[13].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 13, sLowerCI, sUpperCI);
            }
            else if (index == 14
                && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(14);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[14].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 14, sLowerCI, sUpperCI);
            }
            else if (index == 15
                && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(15);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[15].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 15, sLowerCI, sUpperCI);
            }
            else if (index == 16
                && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(16);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[16].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 16, sLowerCI, sUpperCI);
            }
            else if (index == 17
                && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(17);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[17].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 17, sLowerCI, sUpperCI);
            }
            else if (index == 18
                && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(18);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[18].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 18, sLowerCI, sUpperCI);
            }
            else if (index == 19
                && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(19);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[19].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                    this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 19, sLowerCI, sUpperCI);
            }
            else if (index == 20
                && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
            {
                algoIndicator = index;
                IndicatorQT1 qt1 = FillIndicator(20);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(index, this.ME2Indicators[20].IndMathSubType, colNames, qt1, this.ME2Indicators[0].IndCILevel,
                   this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, 20, sLowerCI, sUpperCI);
            }
            else
            {
                //ignore the row
            }
            
            return algoIndicator;
        }
        private async Task<string> SetPRACorrIndicatorStats(string jdataURL, string[] colNames,
            IDictionary<int, List<List<double>>> data)
        {
            string algoIndicator = string.Empty;
            //do not use the condition if (data.Count > 0) because correlated inds can use random samples
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            IndicatorQT1 qt = FillIndicators();
            DevTreks.Extensions.Algorithms.PRA2 pra
                = InitPRA2Algo(qt, this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom);
            IDictionary<string, List<List<double>>> data2 = ConvertDataToString(data);
            algoIndicator = await pra.RunAlgorithmAsync(jdataURL, data2);
            FillBaseIndicators(pra.IndicatorQT, sLowerCI, sUpperCI);
            if (!string.IsNullOrEmpty(algoIndicator))
            {
                int[] algoIndexes = GetIndexes(algoIndicator);
                int[] indicators = new int[] { };

                //use the randomsample data to generate Score, ScoreM, ScoreL, and ScoreU
                indicators = SetScoresFromRandomSamples(algoIndexes, pra.RandomSampleData, colNames);
                algoIndicator = GetIndicatorsCSV(indicators.ToList(), algoIndicator);
            }
            return algoIndicator;
        }
       
        private IndicatorQT1 FillIndicators()
        {
            IndicatorQT1 qt = new IndicatorQT1();
            qt.IndicatorQT1s = new List<IndicatorQT1>();
            //21 qt1s need to be copied to list
            for (int i = 0; i < 21; i++)
            {
                if (i == 0
                    && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(0); 
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 1
                    && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(1); 
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 2
                    && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(2);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 3
                    && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(3);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 4
                    && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(4);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 5
                    && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(5);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 6
                    && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(6);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 7
                    && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(7);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 8
                    && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(8);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 9
                    && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(9);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 10
                    && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(9);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 11
                    && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(11);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 12
                    && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(12);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 13
                    && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(13);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 14
                    && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(14);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 15
                    && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(15);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 16
                    && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(16);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 17
                    && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(17);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 18
                    && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(18);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 19
                    && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(19);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 20
                    && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(20);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else
                {
                    //ignore the row
                }
            }
            return qt;
        }
        
        private IndicatorQT1 FillIndicator(int index)
        {
            IndicatorQT1 qt = new IndicatorQT1();
            if (index == 0
                && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[0].IndLabel, this.ME2Indicators[0].IndTMAmount, this.ME2Indicators[0].IndTLAmount, ME2Indicators[0].IndTUAmount,
                    this.ME2Indicators[0].IndTAmount, this.ME2Indicators[0].IndTD1Amount, this.ME2Indicators[0].IndTD2Amount, this.ME2Indicators[0].IndTMUnit, this.ME2Indicators[0].IndTLUnit, this.ME2Indicators[0].IndTUUnit,
                    this.ME2Indicators[0].IndTUnit, this.ME2Indicators[0].IndTD1Unit, this.ME2Indicators[0].IndTD2Unit, this.ME2Indicators[0].IndMathType, this.ME2Indicators[0].IndMathSubType,
                    this.ME2Indicators[0].IndType, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathResult,
                    0, 0, 0, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            else if (index == 1
                && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[1].IndLabel, this.ME2Indicators[1].IndTMAmount, this.ME2Indicators[1].IndTLAmount, ME2Indicators[1].IndTUAmount,
                    this.ME2Indicators[1].IndTAmount, this.ME2Indicators[1].IndTD1Amount, this.ME2Indicators[1].IndTD2Amount, this.ME2Indicators[1].IndTMUnit, this.ME2Indicators[1].IndTLUnit, this.ME2Indicators[1].IndTUUnit,
                    this.ME2Indicators[1].IndTUnit, this.ME2Indicators[1].IndTD1Unit, this.ME2Indicators[1].IndTD2Unit, this.ME2Indicators[1].IndMathType, this.ME2Indicators[1].IndMathSubType,
                    this.ME2Indicators[1].IndType, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathResult,
                    this.ME2Indicators[1].Ind1Amount, this.ME2Indicators[1].Ind2Amount, this.ME2Indicators[1].Ind3Amount, this.ME2Indicators[1].Ind4Amount, this.ME2Indicators[1].Ind5Amount,
                    this.ME2Indicators[1].Ind1Unit, this.ME2Indicators[1].Ind2Unit, this.ME2Indicators[1].Ind3Unit, this.ME2Indicators[1].Ind4Unit, this.ME2Indicators[1].Ind5Unit);
            }
            else if (index == 2
                && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[2].IndLabel, this.ME2Indicators[2].IndTMAmount, this.ME2Indicators[2].IndTLAmount, ME2Indicators[2].IndTUAmount,
                    this.ME2Indicators[2].IndTAmount, this.ME2Indicators[2].IndTD1Amount, this.ME2Indicators[2].IndTD2Amount, this.ME2Indicators[2].IndTMUnit, this.ME2Indicators[2].IndTLUnit, this.ME2Indicators[2].IndTUUnit,
                    this.ME2Indicators[2].IndTUnit, this.ME2Indicators[2].IndTD1Unit, this.ME2Indicators[2].IndTD2Unit, this.ME2Indicators[2].IndMathType, this.ME2Indicators[2].IndMathSubType,
                    this.ME2Indicators[2].IndType, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathResult,
                    this.ME2Indicators[2].Ind1Amount, this.ME2Indicators[2].Ind2Amount, this.ME2Indicators[2].Ind3Amount, this.ME2Indicators[2].Ind4Amount, this.ME2Indicators[2].Ind5Amount,
                    this.ME2Indicators[2].Ind1Unit, this.ME2Indicators[2].Ind2Unit, this.ME2Indicators[2].Ind3Unit, this.ME2Indicators[2].Ind4Unit, this.ME2Indicators[2].Ind5Unit);
            }
            else if (index == 3
                && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[3].IndLabel, this.ME2Indicators[3].IndTMAmount, this.ME2Indicators[3].IndTLAmount, ME2Indicators[3].IndTUAmount,
                    this.ME2Indicators[3].IndTAmount, this.ME2Indicators[3].IndTD1Amount, this.ME2Indicators[3].IndTD2Amount, this.ME2Indicators[3].IndTMUnit, this.ME2Indicators[3].IndTLUnit, this.ME2Indicators[3].IndTUUnit,
                    this.ME2Indicators[3].IndTUnit, this.ME2Indicators[3].IndTD1Unit, this.ME2Indicators[3].IndTD2Unit, this.ME2Indicators[3].IndMathType, this.ME2Indicators[3].IndMathSubType,
                    this.ME2Indicators[3].IndType, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathResult,
                    this.ME2Indicators[3].Ind1Amount, this.ME2Indicators[3].Ind2Amount, this.ME2Indicators[3].Ind3Amount, this.ME2Indicators[3].Ind4Amount, this.ME2Indicators[3].Ind5Amount,
                    this.ME2Indicators[3].Ind1Unit, this.ME2Indicators[3].Ind2Unit, this.ME2Indicators[3].Ind3Unit, this.ME2Indicators[3].Ind4Unit, this.ME2Indicators[3].Ind5Unit);
            }
            else if (index == 4
                && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[4].IndLabel, this.ME2Indicators[4].IndTMAmount, this.ME2Indicators[4].IndTLAmount, ME2Indicators[4].IndTUAmount,
                    this.ME2Indicators[4].IndTAmount, this.ME2Indicators[4].IndTD1Amount, this.ME2Indicators[4].IndTD2Amount, this.ME2Indicators[4].IndTMUnit, this.ME2Indicators[4].IndTLUnit, this.ME2Indicators[4].IndTUUnit,
                    this.ME2Indicators[4].IndTUnit, this.ME2Indicators[4].IndTD1Unit, this.ME2Indicators[4].IndTD2Unit, this.ME2Indicators[4].IndMathType, this.ME2Indicators[4].IndMathSubType,
                    this.ME2Indicators[4].IndType, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathResult,
                    this.ME2Indicators[4].Ind1Amount, this.ME2Indicators[4].Ind2Amount, this.ME2Indicators[4].Ind3Amount, this.ME2Indicators[4].Ind4Amount, this.ME2Indicators[4].Ind5Amount,
                    this.ME2Indicators[4].Ind1Unit, this.ME2Indicators[4].Ind2Unit, this.ME2Indicators[4].Ind3Unit, this.ME2Indicators[4].Ind4Unit, this.ME2Indicators[4].Ind5Unit);
            }
            else if (index == 5
                && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[5].IndLabel, this.ME2Indicators[5].IndTMAmount, this.ME2Indicators[5].IndTLAmount, ME2Indicators[5].IndTUAmount,
                    this.ME2Indicators[5].IndTAmount, this.ME2Indicators[5].IndTD1Amount, this.ME2Indicators[5].IndTD2Amount, this.ME2Indicators[5].IndTMUnit, this.ME2Indicators[5].IndTLUnit, this.ME2Indicators[5].IndTUUnit,
                    this.ME2Indicators[5].IndTUnit, this.ME2Indicators[5].IndTD1Unit, this.ME2Indicators[5].IndTD2Unit, this.ME2Indicators[5].IndMathType, this.ME2Indicators[5].IndMathSubType,
                    this.ME2Indicators[5].IndType, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathResult,
                    this.ME2Indicators[5].Ind1Amount, this.ME2Indicators[5].Ind2Amount, this.ME2Indicators[5].Ind3Amount, this.ME2Indicators[5].Ind4Amount, this.ME2Indicators[5].Ind5Amount,
                    this.ME2Indicators[5].Ind1Unit, this.ME2Indicators[5].Ind2Unit, this.ME2Indicators[5].Ind3Unit, this.ME2Indicators[5].Ind4Unit, this.ME2Indicators[5].Ind5Unit);
            }
            else if (index == 6
                && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[6].IndLabel, this.ME2Indicators[6].IndTMAmount, this.ME2Indicators[6].IndTLAmount, ME2Indicators[6].IndTUAmount,
                    this.ME2Indicators[6].IndTAmount, this.ME2Indicators[6].IndTD1Amount, this.ME2Indicators[6].IndTD2Amount, this.ME2Indicators[6].IndTMUnit, this.ME2Indicators[6].IndTLUnit, this.ME2Indicators[6].IndTUUnit,
                    this.ME2Indicators[6].IndTUnit, this.ME2Indicators[6].IndTD1Unit, this.ME2Indicators[6].IndTD2Unit, this.ME2Indicators[6].IndMathType, this.ME2Indicators[6].IndMathSubType,
                    this.ME2Indicators[6].IndType, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathResult,
                    this.ME2Indicators[6].Ind1Amount, this.ME2Indicators[6].Ind2Amount, this.ME2Indicators[6].Ind3Amount, this.ME2Indicators[6].Ind4Amount, this.ME2Indicators[6].Ind5Amount,
                    this.ME2Indicators[6].Ind1Unit, this.ME2Indicators[6].Ind2Unit, this.ME2Indicators[6].Ind3Unit, this.ME2Indicators[6].Ind4Unit, this.ME2Indicators[6].Ind5Unit);
            }
            else if (index == 7
                && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[7].IndLabel, this.ME2Indicators[7].IndTMAmount, this.ME2Indicators[7].IndTLAmount, ME2Indicators[7].IndTUAmount,
                    this.ME2Indicators[7].IndTAmount, this.ME2Indicators[7].IndTD1Amount, this.ME2Indicators[7].IndTD2Amount, this.ME2Indicators[7].IndTMUnit, this.ME2Indicators[7].IndTLUnit, this.ME2Indicators[7].IndTUUnit,
                    this.ME2Indicators[7].IndTUnit, this.ME2Indicators[7].IndTD1Unit, this.ME2Indicators[7].IndTD2Unit, this.ME2Indicators[7].IndMathType, this.ME2Indicators[7].IndMathSubType,
                    this.ME2Indicators[7].IndType, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathResult,
                    this.ME2Indicators[7].Ind1Amount, this.ME2Indicators[7].Ind2Amount, this.ME2Indicators[7].Ind3Amount, this.ME2Indicators[7].Ind4Amount, this.ME2Indicators[7].Ind5Amount,
                    this.ME2Indicators[7].Ind1Unit, this.ME2Indicators[7].Ind2Unit, this.ME2Indicators[7].Ind3Unit, this.ME2Indicators[7].Ind4Unit, this.ME2Indicators[7].Ind5Unit);
            }
            else if (index == 8
                && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[8].IndLabel, this.ME2Indicators[8].IndTMAmount, this.ME2Indicators[8].IndTLAmount, ME2Indicators[8].IndTUAmount,
                    this.ME2Indicators[8].IndTAmount, this.ME2Indicators[8].IndTD1Amount, this.ME2Indicators[8].IndTD2Amount, this.ME2Indicators[8].IndTMUnit, this.ME2Indicators[8].IndTLUnit, this.ME2Indicators[8].IndTUUnit,
                    this.ME2Indicators[8].IndTUnit, this.ME2Indicators[8].IndTD1Unit, this.ME2Indicators[8].IndTD2Unit, this.ME2Indicators[8].MathType, this.ME2Indicators[8].IndMathSubType,
                    this.ME2Indicators[8].IndType, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathResult,
                    this.ME2Indicators[8].Ind1Amount, this.ME2Indicators[8].Ind2Amount, this.ME2Indicators[8].Ind3Amount, this.ME2Indicators[8].Ind4Amount, this.ME2Indicators[8].Ind5Amount,
                    this.ME2Indicators[8].Ind1Unit, this.ME2Indicators[8].Ind2Unit, this.ME2Indicators[8].Ind3Unit, this.ME2Indicators[8].Ind4Unit, this.ME2Indicators[8].Ind5Unit);
            }
            else if (index == 9
                && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[9].IndLabel, this.ME2Indicators[9].IndTMAmount, this.ME2Indicators[9].IndTLAmount, ME2Indicators[9].IndTUAmount,
                    this.ME2Indicators[9].IndTAmount, this.ME2Indicators[9].IndTD1Amount, this.ME2Indicators[9].IndTD2Amount, this.ME2Indicators[9].IndTMUnit, this.ME2Indicators[9].IndTLUnit, this.ME2Indicators[9].IndTUUnit,
                    this.ME2Indicators[9].IndTUnit, this.ME2Indicators[9].IndTD1Unit, this.ME2Indicators[9].IndTD2Unit, this.ME2Indicators[9].IndMathType, this.ME2Indicators[9].IndMathSubType,
                    this.ME2Indicators[9].IndType, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathResult,
                    this.ME2Indicators[9].Ind1Amount, this.ME2Indicators[9].Ind2Amount, this.ME2Indicators[9].Ind3Amount, this.ME2Indicators[9].Ind4Amount, this.ME2Indicators[9].Ind5Amount,
                    this.ME2Indicators[9].Ind1Unit, this.ME2Indicators[9].Ind2Unit, this.ME2Indicators[9].Ind3Unit, this.ME2Indicators[9].Ind4Unit, this.ME2Indicators[9].Ind5Unit);
            }
            else if (index == 10
                && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[10].IndLabel, this.ME2Indicators[10].IndTMAmount, this.ME2Indicators[10].IndTLAmount, ME2Indicators[10].IndTUAmount,
                    this.ME2Indicators[10].IndTAmount, this.ME2Indicators[10].IndTD1Amount, this.ME2Indicators[10].IndTD2Amount, this.ME2Indicators[10].IndTMUnit, this.ME2Indicators[10].IndTLUnit, this.ME2Indicators[10].IndTUUnit,
                    this.ME2Indicators[10].IndTUnit, this.ME2Indicators[10].IndTD1Unit, this.ME2Indicators[10].IndTD2Unit, this.ME2Indicators[10].IndMathType, this.ME2Indicators[10].IndMathSubType,
                    this.ME2Indicators[10].IndType, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathResult,
                    this.ME2Indicators[10].Ind1Amount, this.ME2Indicators[10].Ind2Amount, this.ME2Indicators[10].Ind3Amount, this.ME2Indicators[10].Ind4Amount, this.ME2Indicators[10].Ind5Amount,
                    this.ME2Indicators[10].Ind1Unit, this.ME2Indicators[10].Ind2Unit, this.ME2Indicators[10].Ind3Unit, this.ME2Indicators[10].Ind4Unit, this.ME2Indicators[10].Ind5Unit);
            }
            else if (index == 11
                && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[11].IndLabel, this.ME2Indicators[11].IndTMAmount, this.ME2Indicators[11].IndTLAmount, ME2Indicators[11].IndTUAmount,
                    this.ME2Indicators[11].IndTAmount, this.ME2Indicators[11].IndTD1Amount, this.ME2Indicators[11].IndTD2Amount, this.ME2Indicators[11].IndTMUnit, this.ME2Indicators[11].IndTLUnit, this.ME2Indicators[11].IndTUUnit,
                    this.ME2Indicators[11].IndTUnit, this.ME2Indicators[11].IndTD1Unit, this.ME2Indicators[11].IndTD2Unit, this.ME2Indicators[11].IndMathType, this.ME2Indicators[11].IndMathSubType,
                    this.ME2Indicators[11].IndType, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathResult,
                    this.ME2Indicators[11].Ind1Amount, this.ME2Indicators[11].Ind2Amount, this.ME2Indicators[11].Ind3Amount, this.ME2Indicators[11].Ind4Amount, this.ME2Indicators[11].Ind5Amount,
                    this.ME2Indicators[11].Ind1Unit, this.ME2Indicators[11].Ind2Unit, this.ME2Indicators[11].Ind3Unit, this.ME2Indicators[11].Ind4Unit, this.ME2Indicators[11].Ind5Unit);
            }
            else if (index == 12
                && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[12].IndLabel, this.ME2Indicators[12].IndTMAmount, this.ME2Indicators[12].IndTLAmount, ME2Indicators[12].IndTUAmount,
                    this.ME2Indicators[12].IndTAmount, this.ME2Indicators[12].IndTD1Amount, this.ME2Indicators[12].IndTD2Amount, this.ME2Indicators[12].IndTMUnit, this.ME2Indicators[12].IndTLUnit, this.ME2Indicators[12].IndTUUnit,
                    this.ME2Indicators[12].IndTUnit, this.ME2Indicators[12].IndTD1Unit, this.ME2Indicators[12].IndTD2Unit, this.ME2Indicators[12].IndMathType, this.ME2Indicators[12].IndMathSubType,
                    this.ME2Indicators[12].IndType, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathResult,
                    this.ME2Indicators[12].Ind1Amount, this.ME2Indicators[12].Ind2Amount, this.ME2Indicators[12].Ind3Amount, this.ME2Indicators[12].Ind4Amount, this.ME2Indicators[12].Ind5Amount,
                    this.ME2Indicators[12].Ind1Unit, this.ME2Indicators[12].Ind2Unit, this.ME2Indicators[12].Ind3Unit, this.ME2Indicators[12].Ind4Unit, this.ME2Indicators[12].Ind5Unit);
            }
            else if (index == 13
                && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[13].IndLabel, this.ME2Indicators[13].IndTMAmount, this.ME2Indicators[13].IndTLAmount, ME2Indicators[13].IndTUAmount,
                    this.ME2Indicators[13].IndTAmount, this.ME2Indicators[13].IndTD1Amount, this.ME2Indicators[13].IndTD2Amount, this.ME2Indicators[13].IndTMUnit, this.ME2Indicators[13].IndTLUnit, this.ME2Indicators[13].IndTUUnit,
                    this.ME2Indicators[13].IndTUnit, this.ME2Indicators[13].IndTD1Unit, this.ME2Indicators[13].IndTD2Unit, this.ME2Indicators[13].IndMathType, this.ME2Indicators[13].IndMathSubType,
                    this.ME2Indicators[13].IndType, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathResult,
                    this.ME2Indicators[13].Ind1Amount, this.ME2Indicators[13].Ind2Amount, this.ME2Indicators[13].Ind3Amount, this.ME2Indicators[13].Ind4Amount, this.ME2Indicators[13].Ind5Amount,
                    this.ME2Indicators[13].Ind1Unit, this.ME2Indicators[13].Ind2Unit, this.ME2Indicators[13].Ind3Unit, this.ME2Indicators[13].Ind4Unit, this.ME2Indicators[13].Ind5Unit);
            }
            else if (index == 14
                && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[14].IndLabel, this.ME2Indicators[14].IndTMAmount, this.ME2Indicators[14].IndTLAmount, ME2Indicators[14].IndTUAmount,
                    this.ME2Indicators[14].IndTAmount, this.ME2Indicators[14].IndTD1Amount, this.ME2Indicators[14].IndTD2Amount, this.ME2Indicators[14].IndTMUnit, this.ME2Indicators[14].IndTLUnit, this.ME2Indicators[14].IndTUUnit,
                    this.ME2Indicators[14].IndTUnit, this.ME2Indicators[14].IndTD1Unit, this.ME2Indicators[14].IndTD2Unit, this.ME2Indicators[14].IndMathType, this.ME2Indicators[14].IndMathSubType,
                    this.ME2Indicators[14].IndType, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathResult,
                    this.ME2Indicators[14].Ind1Amount, this.ME2Indicators[14].Ind2Amount, this.ME2Indicators[14].Ind3Amount, this.ME2Indicators[14].Ind4Amount, this.ME2Indicators[14].Ind5Amount,
                    this.ME2Indicators[14].Ind1Unit, this.ME2Indicators[14].Ind2Unit, this.ME2Indicators[14].Ind3Unit, this.ME2Indicators[14].Ind4Unit, this.ME2Indicators[14].Ind5Unit);
            }
            else if (index == 15
                && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[15].IndLabel, this.ME2Indicators[15].IndTMAmount, this.ME2Indicators[15].IndTLAmount, ME2Indicators[15].IndTUAmount,
                    this.ME2Indicators[15].IndTAmount, this.ME2Indicators[15].IndTD1Amount, this.ME2Indicators[15].IndTD2Amount, this.ME2Indicators[15].IndTMUnit, this.ME2Indicators[15].IndTLUnit, this.ME2Indicators[15].IndTUUnit,
                    this.ME2Indicators[15].IndTUnit, this.ME2Indicators[15].IndTD1Unit, this.ME2Indicators[15].IndTD2Unit, this.ME2Indicators[15].IndMathType, this.ME2Indicators[15].IndMathSubType,
                    this.ME2Indicators[15].IndType, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathResult,
                    this.ME2Indicators[15].Ind1Amount, this.ME2Indicators[15].Ind2Amount, this.ME2Indicators[15].Ind3Amount, this.ME2Indicators[15].Ind4Amount, this.ME2Indicators[15].Ind5Amount,
                    this.ME2Indicators[15].Ind1Unit, this.ME2Indicators[15].Ind2Unit, this.ME2Indicators[15].Ind3Unit, this.ME2Indicators[15].Ind4Unit, this.ME2Indicators[15].Ind5Unit);
            }
            else if (index == 16
                && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[16].IndLabel, this.ME2Indicators[16].IndTMAmount, this.ME2Indicators[16].IndTLAmount, ME2Indicators[16].IndTUAmount,
                    this.ME2Indicators[16].IndTAmount, this.ME2Indicators[16].IndTD1Amount, this.ME2Indicators[16].IndTD2Amount, this.ME2Indicators[16].IndTMUnit, this.ME2Indicators[16].IndTLUnit, this.ME2Indicators[16].IndTUUnit,
                    this.ME2Indicators[16].IndTUnit, this.ME2Indicators[16].IndTD1Unit, this.ME2Indicators[16].IndTD2Unit, this.ME2Indicators[16].IndMathType, this.ME2Indicators[16].IndMathSubType,
                    this.ME2Indicators[16].IndType, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathResult,
                    this.ME2Indicators[16].Ind1Amount, this.ME2Indicators[16].Ind2Amount, this.ME2Indicators[16].Ind3Amount, this.ME2Indicators[16].Ind4Amount, this.ME2Indicators[16].Ind5Amount,
                    this.ME2Indicators[16].Ind1Unit, this.ME2Indicators[16].Ind2Unit, this.ME2Indicators[16].Ind3Unit, this.ME2Indicators[16].Ind4Unit, this.ME2Indicators[16].Ind5Unit);
            }
            else if (index == 17
                && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[17].IndLabel, this.ME2Indicators[17].IndTMAmount, this.ME2Indicators[17].IndTLAmount, ME2Indicators[17].IndTUAmount,
                    this.ME2Indicators[17].IndTAmount, this.ME2Indicators[17].IndTD1Amount, this.ME2Indicators[17].IndTD2Amount, this.ME2Indicators[17].IndTMUnit, this.ME2Indicators[17].IndTLUnit, this.ME2Indicators[17].IndTUUnit,
                    this.ME2Indicators[17].IndTUnit, this.ME2Indicators[17].IndTD1Unit, this.ME2Indicators[17].IndTD2Unit, this.ME2Indicators[17].IndMathType, this.ME2Indicators[17].IndMathSubType,
                    this.ME2Indicators[17].IndType, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathResult,
                    this.ME2Indicators[17].Ind1Amount, this.ME2Indicators[17].Ind2Amount, this.ME2Indicators[17].Ind3Amount, this.ME2Indicators[17].Ind4Amount, this.ME2Indicators[17].Ind5Amount,
                    this.ME2Indicators[17].Ind1Unit, this.ME2Indicators[17].Ind2Unit, this.ME2Indicators[17].Ind3Unit, this.ME2Indicators[17].Ind4Unit, this.ME2Indicators[17].Ind5Unit);
            }
            else if (index == 18
                && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[18].IndLabel, this.ME2Indicators[18].IndTMAmount, this.ME2Indicators[18].IndTLAmount, ME2Indicators[18].IndTUAmount,
                    this.ME2Indicators[18].IndTAmount, this.ME2Indicators[18].IndTD1Amount, this.ME2Indicators[18].IndTD2Amount, this.ME2Indicators[18].IndTMUnit, this.ME2Indicators[18].IndTLUnit, this.ME2Indicators[18].IndTUUnit,
                    this.ME2Indicators[18].IndTUnit, this.ME2Indicators[18].IndTD1Unit, this.ME2Indicators[18].IndTD2Unit, this.ME2Indicators[18].IndMathType, this.ME2Indicators[18].IndMathSubType,
                    this.ME2Indicators[18].IndType, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathResult,
                    this.ME2Indicators[18].Ind1Amount, this.ME2Indicators[18].Ind2Amount, this.ME2Indicators[18].Ind3Amount, this.ME2Indicators[18].Ind4Amount, this.ME2Indicators[18].Ind5Amount,
                    this.ME2Indicators[18].Ind1Unit, this.ME2Indicators[18].Ind2Unit, this.ME2Indicators[18].Ind3Unit, this.ME2Indicators[18].Ind4Unit, this.ME2Indicators[18].Ind5Unit);
            }
            else if (index == 19
                && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[19].IndLabel, this.ME2Indicators[19].IndTMAmount, this.ME2Indicators[19].IndTLAmount, ME2Indicators[19].IndTUAmount,
                    this.ME2Indicators[19].IndTAmount, this.ME2Indicators[19].IndTD1Amount, this.ME2Indicators[19].IndTD2Amount, this.ME2Indicators[19].IndTMUnit, this.ME2Indicators[19].IndTLUnit, this.ME2Indicators[19].IndTUUnit,
                    this.ME2Indicators[19].IndTUnit, this.ME2Indicators[19].IndTD1Unit, this.ME2Indicators[19].IndTD2Unit, this.ME2Indicators[19].IndMathType, this.ME2Indicators[19].IndMathSubType,
                    this.ME2Indicators[19].IndType, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathResult,
                    this.ME2Indicators[19].Ind1Amount, this.ME2Indicators[19].Ind2Amount, this.ME2Indicators[19].Ind3Amount, this.ME2Indicators[19].Ind4Amount, this.ME2Indicators[19].Ind5Amount,
                    this.ME2Indicators[19].Ind1Unit, this.ME2Indicators[19].Ind2Unit, this.ME2Indicators[19].Ind3Unit, this.ME2Indicators[19].Ind4Unit, this.ME2Indicators[19].Ind5Unit);
            }
            else if (index == 20
                && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
            {
                qt = new IndicatorQT1(this.ME2Indicators[20].IndLabel, this.ME2Indicators[20].IndTMAmount, this.ME2Indicators[20].IndTLAmount, ME2Indicators[20].IndTUAmount,
                    this.ME2Indicators[20].IndTAmount, this.ME2Indicators[20].IndTD1Amount, this.ME2Indicators[20].IndTD2Amount, this.ME2Indicators[20].IndTMUnit, this.ME2Indicators[20].IndTLUnit, this.ME2Indicators[20].IndTUUnit,
                    this.ME2Indicators[20].IndTUnit, this.ME2Indicators[20].IndTD1Unit, this.ME2Indicators[20].IndTD2Unit, this.ME2Indicators[20].IndMathType, this.ME2Indicators[20].IndMathSubType,
                    this.ME2Indicators[20].IndType, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathResult,
                    this.ME2Indicators[20].Ind1Amount, this.ME2Indicators[20].Ind2Amount, this.ME2Indicators[20].Ind3Amount, this.ME2Indicators[20].Ind4Amount, this.ME2Indicators[20].Ind5Amount,
                    this.ME2Indicators[20].Ind1Unit, this.ME2Indicators[20].Ind2Unit, this.ME2Indicators[20].Ind3Unit, this.ME2Indicators[20].Ind4Unit, this.ME2Indicators[20].Ind5Unit);
            }
            else
            {
                //ignore the row
            }
            return qt;
        }
        private void FillBaseIndicators(IndicatorQT1 qt1, string lowerci, string upperci)
        {
            //21 qt1s need to be copied to list
            for (int i = 0; i < 21; i++)
            {
                if (i == 0
                    && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 0, lowerci, upperci);
                }
                else if (i == 1
                    && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 1, lowerci, upperci);
                }
                else if (i == 2
                    && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 2, lowerci, upperci);
                }
                else if (i == 3
                    && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 3, lowerci, upperci);
                }
                else if (i == 4
                    && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 4, lowerci, upperci);
                }
                else if (i == 5
                    && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 5, lowerci, upperci);
                }
                else if (i == 6
                    && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 6, lowerci, upperci);
                }
                else if (i == 7
                    && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 7, lowerci, upperci);
                }
                else if (i == 8
                    && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 8, lowerci, upperci);
                }
                else if (i == 9
                    && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 9, lowerci, upperci);
                }
                else if (i == 10
                    && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 10, lowerci, upperci);
                }
                else if (i == 11
                    && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 11, lowerci, upperci);
                }
                else if (i == 12
                    && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 12, lowerci, upperci);
                }
                else if (i == 13
                    && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 13, lowerci, upperci);
                }
                else if (i == 14
                    && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 14, lowerci, upperci);
                }
                else if (i == 15
                    && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 15, lowerci, upperci);
                }
                else if (i == 16
                    && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 16, lowerci, upperci);
                }
                else if (i == 17
                    && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 17, lowerci, upperci);
                }
                else if (i == 18
                    && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 18, lowerci, upperci);
                }
                else if (i == 19
                    && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 19, lowerci, upperci);
                }
                else if (i == 20
                    && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], 20, lowerci, upperci);
                }
                else
                {
                    //ignore the row
                }
            }
        }
        private void FillBaseIndicator(IndicatorQT1 qt1, int index, string lowerci, string upperci)
        {
            bool bNeedsDistribution = true;
            //208
            if (this.HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm13)
                || this.HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm14)
                || this.HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm15)
                || this.HasMathType(MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm16))
            {
                lowerci = qt1.QTLUnit;
                upperci = qt1.QTUUnit;
                bNeedsDistribution = true;
            }
            if (index == 0
                && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
            {
                this.ME2Indicators[0].IndTMAmount = qt1.QTM;
                this.ME2Indicators[0].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[0].IndTLAmount = qt1.QTL;
                this.ME2Indicators[0].IndTLUnit = lowerci;
                this.ME2Indicators[0].IndTUAmount = qt1.QTU;
                this.ME2Indicators[0].IndTUUnit = upperci;
                this.ME2Indicators[0].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[0].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[0].IndTAmount = qt1.QT;
                    this.ME2Indicators[0].IndTUnit = qt1.QTUnit;
                    this.ME2Indicators[0].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[0].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[0].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[0].IndTD2Unit = qt1.QTD2Unit;
                }
            }
            else if (index == 1
                && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
            {
                this.ME2Indicators[1].IndTMAmount = qt1.QTM;
                this.ME2Indicators[1].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[1].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[1].IndTLAmount = qt1.QTL;
                this.ME2Indicators[1].IndTLUnit = lowerci;
                this.ME2Indicators[1].IndTUAmount = qt1.QTU;
                this.ME2Indicators[1].IndTUUnit = upperci;
                this.ME2Indicators[1].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[1].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[1].IndTAmount = qt1.QT;
                    this.ME2Indicators[1].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[1].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[1].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[1].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[1].Ind1Amount = qt1.Q1;
                this.ME2Indicators[1].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[1].Ind2Amount = qt1.Q2;
                this.ME2Indicators[1].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[1].Ind3Amount = qt1.Q3;
                this.ME2Indicators[1].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[1].Ind4Amount = qt1.Q4;
                this.ME2Indicators[1].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[1].Ind5Amount = qt1.Q5;
                this.ME2Indicators[1].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 2
                && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
            {
                this.ME2Indicators[2].IndTMAmount = qt1.QTM;
                this.ME2Indicators[2].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[2].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[2].IndTLAmount = qt1.QTL;
                this.ME2Indicators[2].IndTLUnit = lowerci;
                this.ME2Indicators[2].IndTUAmount = qt1.QTU;
                this.ME2Indicators[2].IndTUUnit = upperci;
                this.ME2Indicators[2].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[2].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[2].IndTAmount = qt1.QT;
                    this.ME2Indicators[2].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[2].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[2].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[2].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[2].Ind1Amount = qt1.Q1;
                this.ME2Indicators[2].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[2].Ind2Amount = qt1.Q2;
                this.ME2Indicators[2].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[2].Ind3Amount = qt1.Q3;
                this.ME2Indicators[2].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[2].Ind4Amount = qt1.Q4;
                this.ME2Indicators[2].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[2].Ind5Amount = qt1.Q5;
                this.ME2Indicators[2].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 3
                && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
            {
                this.ME2Indicators[3].IndTMAmount = qt1.QTM;
                this.ME2Indicators[3].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[3].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[3].IndTLAmount = qt1.QTL;
                this.ME2Indicators[3].IndTLUnit = lowerci;
                this.ME2Indicators[3].IndTUAmount = qt1.QTU;
                this.ME2Indicators[3].IndTUUnit = upperci;
                this.ME2Indicators[3].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[3].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[3].IndTAmount = qt1.QT;
                    this.ME2Indicators[3].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[3].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[3].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[3].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[3].Ind1Amount = qt1.Q1;
                this.ME2Indicators[3].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[3].Ind2Amount = qt1.Q2;
                this.ME2Indicators[3].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[3].Ind3Amount = qt1.Q3;
                this.ME2Indicators[3].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[3].Ind4Amount = qt1.Q4;
                this.ME2Indicators[3].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[3].Ind5Amount = qt1.Q5;
                this.ME2Indicators[3].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 4
                && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
            {
                this.ME2Indicators[4].IndTMAmount = qt1.QTM;
                this.ME2Indicators[4].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[4].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[4].IndTLAmount = qt1.QTL;
                this.ME2Indicators[4].IndTLUnit = lowerci;
                this.ME2Indicators[4].IndTUAmount = qt1.QTU;
                this.ME2Indicators[4].IndTUUnit = upperci;
                this.ME2Indicators[4].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[4].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[4].IndTAmount = qt1.QT;
                    this.ME2Indicators[4].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[4].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[4].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[4].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[4].Ind1Amount = qt1.Q1;
                this.ME2Indicators[4].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[4].Ind2Amount = qt1.Q2;
                this.ME2Indicators[4].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[4].Ind3Amount = qt1.Q3;
                this.ME2Indicators[4].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[4].Ind4Amount = qt1.Q4;
                this.ME2Indicators[4].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[4].Ind5Amount = qt1.Q5;
                this.ME2Indicators[4].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 5
                && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
            {
                this.ME2Indicators[5].IndTMAmount = qt1.QTM;
                this.ME2Indicators[5].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[5].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[5].IndTLAmount = qt1.QTL;
                this.ME2Indicators[5].IndTLUnit = lowerci;
                this.ME2Indicators[5].IndTUAmount = qt1.QTU;
                this.ME2Indicators[5].IndTUUnit = upperci;
                this.ME2Indicators[5].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[5].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[5].IndTAmount = qt1.QT;
                    this.ME2Indicators[5].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[5].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[5].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[5].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[5].Ind1Amount = qt1.Q1;
                this.ME2Indicators[5].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[5].Ind2Amount = qt1.Q2;
                this.ME2Indicators[5].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[5].Ind3Amount = qt1.Q3;
                this.ME2Indicators[5].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[5].Ind4Amount = qt1.Q4;
                this.ME2Indicators[5].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[5].Ind5Amount = qt1.Q5;
                this.ME2Indicators[5].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 6
                && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
            {
                this.ME2Indicators[6].IndTMAmount = qt1.QTM;
                this.ME2Indicators[6].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[6].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[6].IndTLAmount = qt1.QTL;
                this.ME2Indicators[6].IndTLUnit = lowerci;
                this.ME2Indicators[6].IndTUAmount = qt1.QTU;
                this.ME2Indicators[6].IndTUUnit = upperci;
                this.ME2Indicators[6].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[6].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[6].IndTAmount = qt1.QT;
                    this.ME2Indicators[6].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[6].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[6].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[6].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[6].Ind1Amount = qt1.Q1;
                this.ME2Indicators[6].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[6].Ind2Amount = qt1.Q2;
                this.ME2Indicators[6].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[6].Ind3Amount = qt1.Q3;
                this.ME2Indicators[6].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[6].Ind4Amount = qt1.Q4;
                this.ME2Indicators[6].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[6].Ind5Amount = qt1.Q5;
                this.ME2Indicators[6].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 7
                && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
            {
                this.ME2Indicators[7].IndTMAmount = qt1.QTM;
                this.ME2Indicators[7].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[7].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[7].IndTLAmount = qt1.QTL;
                this.ME2Indicators[7].IndTLUnit = lowerci;
                this.ME2Indicators[7].IndTUAmount = qt1.QTU;
                this.ME2Indicators[7].IndTUUnit = upperci;
                this.ME2Indicators[7].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[7].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[7].IndTAmount = qt1.QT;
                    this.ME2Indicators[7].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[7].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[7].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[7].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[7].Ind1Amount = qt1.Q1;
                this.ME2Indicators[7].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[7].Ind2Amount = qt1.Q2;
                this.ME2Indicators[7].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[7].Ind3Amount = qt1.Q3;
                this.ME2Indicators[7].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[7].Ind4Amount = qt1.Q4;
                this.ME2Indicators[7].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[7].Ind5Amount = qt1.Q5;
                this.ME2Indicators[7].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 8
                && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
            {
                this.ME2Indicators[8].IndTMAmount = qt1.QTM;
                this.ME2Indicators[8].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[8].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[8].IndTLAmount = qt1.QTL;
                this.ME2Indicators[8].IndTLUnit = lowerci;
                this.ME2Indicators[8].IndTUAmount = qt1.QTU;
                this.ME2Indicators[8].IndTUUnit = upperci;
                this.ME2Indicators[8].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[8].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[8].IndTAmount = qt1.QT;
                    this.ME2Indicators[8].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[8].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[8].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[8].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[8].Ind1Amount = qt1.Q1;
                this.ME2Indicators[8].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[8].Ind2Amount = qt1.Q2;
                this.ME2Indicators[8].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[8].Ind3Amount = qt1.Q3;
                this.ME2Indicators[8].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[8].Ind4Amount = qt1.Q4;
                this.ME2Indicators[8].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[8].Ind5Amount = qt1.Q5;
                this.ME2Indicators[8].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 9
                && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
            {
                this.ME2Indicators[9].IndTMAmount = qt1.QTM;
                this.ME2Indicators[9].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[9].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[9].IndTLAmount = qt1.QTL;
                this.ME2Indicators[9].IndTLUnit = lowerci;
                this.ME2Indicators[9].IndTUAmount = qt1.QTU;
                this.ME2Indicators[9].IndTUUnit = upperci;
                this.ME2Indicators[9].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[9].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[9].IndTAmount = qt1.QT;
                    this.ME2Indicators[9].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[9].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[9].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[9].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[9].Ind1Amount = qt1.Q1;
                this.ME2Indicators[9].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[9].Ind2Amount = qt1.Q2;
                this.ME2Indicators[9].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[9].Ind3Amount = qt1.Q3;
                this.ME2Indicators[9].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[9].Ind4Amount = qt1.Q4;
                this.ME2Indicators[9].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[9].Ind5Amount = qt1.Q5;
                this.ME2Indicators[9].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 10
                && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
            {
                this.ME2Indicators[10].IndTMAmount = qt1.QTM;
                this.ME2Indicators[10].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[10].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[10].IndTLAmount = qt1.QTL;
                this.ME2Indicators[10].IndTLUnit = lowerci;
                this.ME2Indicators[10].IndTUAmount = qt1.QTU;
                this.ME2Indicators[10].IndTUUnit = upperci;
                this.ME2Indicators[10].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[10].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[10].IndTAmount = qt1.QT;
                    this.ME2Indicators[10].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[10].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[10].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[10].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[10].Ind1Amount = qt1.Q1;
                this.ME2Indicators[10].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[10].Ind2Amount = qt1.Q2;
                this.ME2Indicators[10].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[10].Ind3Amount = qt1.Q3;
                this.ME2Indicators[10].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[10].Ind4Amount = qt1.Q4;
                this.ME2Indicators[10].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[10].Ind5Amount = qt1.Q5;
                this.ME2Indicators[10].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 11
                && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
            {
                this.ME2Indicators[11].IndTMAmount = qt1.QTM;
                this.ME2Indicators[11].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[11].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[11].IndTLAmount = qt1.QTL;
                this.ME2Indicators[11].IndTLUnit = lowerci;
                this.ME2Indicators[11].IndTUAmount = qt1.QTU;
                this.ME2Indicators[11].IndTUUnit = upperci;
                this.ME2Indicators[11].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[11].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[11].IndTAmount = qt1.QT;
                    this.ME2Indicators[11].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[11].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[11].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[11].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[11].Ind1Amount = qt1.Q1;
                this.ME2Indicators[11].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[11].Ind2Amount = qt1.Q2;
                this.ME2Indicators[11].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[11].Ind3Amount = qt1.Q3;
                this.ME2Indicators[11].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[11].Ind4Amount = qt1.Q4;
                this.ME2Indicators[11].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[11].Ind5Amount = qt1.Q5;
                this.ME2Indicators[11].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 12
                && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
            {
                this.ME2Indicators[12].IndTMAmount = qt1.QTM;
                this.ME2Indicators[12].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[12].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[12].IndTLAmount = qt1.QTL;
                this.ME2Indicators[12].IndTLUnit = lowerci;
                this.ME2Indicators[12].IndTUAmount = qt1.QTU;
                this.ME2Indicators[12].IndTUUnit = upperci;
                this.ME2Indicators[12].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[12].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[12].IndTAmount = qt1.QT;
                    this.ME2Indicators[12].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[12].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[12].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[12].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[12].Ind1Amount = qt1.Q1;
                this.ME2Indicators[12].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[12].Ind2Amount = qt1.Q2;
                this.ME2Indicators[12].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[12].Ind3Amount = qt1.Q3;
                this.ME2Indicators[12].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[12].Ind4Amount = qt1.Q4;
                this.ME2Indicators[12].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[12].Ind5Amount = qt1.Q5;
                this.ME2Indicators[12].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 13
                && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
            {
                this.ME2Indicators[13].IndTMAmount = qt1.QTM;
                this.ME2Indicators[13].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[13].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[13].IndTLAmount = qt1.QTL;
                this.ME2Indicators[13].IndTLUnit = lowerci;
                this.ME2Indicators[13].IndTUAmount = qt1.QTU;
                this.ME2Indicators[13].IndTUUnit = upperci;
                this.ME2Indicators[13].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[13].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[13].IndTAmount = qt1.QT;
                    this.ME2Indicators[13].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[13].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[13].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[13].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[13].Ind1Amount = qt1.Q1;
                this.ME2Indicators[13].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[13].Ind2Amount = qt1.Q2;
                this.ME2Indicators[13].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[13].Ind3Amount = qt1.Q3;
                this.ME2Indicators[13].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[13].Ind4Amount = qt1.Q4;
                this.ME2Indicators[13].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[13].Ind5Amount = qt1.Q5;
                this.ME2Indicators[13].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 14
                && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
            {
                this.ME2Indicators[14].IndTMAmount = qt1.QTM;
                this.ME2Indicators[14].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[14].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[14].IndTLAmount = qt1.QTL;
                this.ME2Indicators[14].IndTLUnit = lowerci;
                this.ME2Indicators[14].IndTUAmount = qt1.QTU;
                this.ME2Indicators[14].IndTUUnit = upperci;
                this.ME2Indicators[14].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[14].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[14].IndTAmount = qt1.QT;
                    this.ME2Indicators[14].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[14].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[14].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[14].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[14].Ind1Amount = qt1.Q1;
                this.ME2Indicators[14].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[14].Ind2Amount = qt1.Q2;
                this.ME2Indicators[14].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[14].Ind3Amount = qt1.Q3;
                this.ME2Indicators[14].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[14].Ind4Amount = qt1.Q4;
                this.ME2Indicators[14].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[14].Ind5Amount = qt1.Q5;
                this.ME2Indicators[14].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 15
                && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
            {
                this.ME2Indicators[15].IndTMAmount = qt1.QTM;
                this.ME2Indicators[15].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[15].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[15].IndTLAmount = qt1.QTL;
                this.ME2Indicators[15].IndTLUnit = lowerci;
                this.ME2Indicators[15].IndTUAmount = qt1.QTU;
                this.ME2Indicators[15].IndTUUnit = upperci;
                this.ME2Indicators[15].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[15].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[15].IndTAmount = qt1.QT;
                    this.ME2Indicators[15].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[15].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[15].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[15].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[15].Ind1Amount = qt1.Q1;
                this.ME2Indicators[15].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[15].Ind2Amount = qt1.Q2;
                this.ME2Indicators[15].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[15].Ind3Amount = qt1.Q3;
                this.ME2Indicators[15].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[15].Ind4Amount = qt1.Q4;
                this.ME2Indicators[15].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[15].Ind5Amount = qt1.Q5;
                this.ME2Indicators[15].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 16
                && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
            {
                this.ME2Indicators[16].IndTMAmount = qt1.QTM;
                this.ME2Indicators[16].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[16].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[16].IndTLAmount = qt1.QTL;
                this.ME2Indicators[16].IndTLUnit = lowerci;
                this.ME2Indicators[16].IndTUAmount = qt1.QTU;
                this.ME2Indicators[16].IndTUUnit = upperci;
                this.ME2Indicators[16].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[16].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[16].IndTAmount = qt1.QT;
                    this.ME2Indicators[16].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[16].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[16].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[16].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[16].Ind1Amount = qt1.Q1;
                this.ME2Indicators[16].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[16].Ind2Amount = qt1.Q2;
                this.ME2Indicators[16].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[16].Ind3Amount = qt1.Q3;
                this.ME2Indicators[16].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[16].Ind4Amount = qt1.Q4;
                this.ME2Indicators[16].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[16].Ind5Amount = qt1.Q5;
                this.ME2Indicators[16].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 17
                && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
            {
                this.ME2Indicators[17].IndTMAmount = qt1.QTM;
                this.ME2Indicators[17].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[17].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[17].IndTLAmount = qt1.QTL;
                this.ME2Indicators[17].IndTLUnit = lowerci;
                this.ME2Indicators[17].IndTUAmount = qt1.QTU;
                this.ME2Indicators[17].IndTUUnit = upperci;
                this.ME2Indicators[17].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[17].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[17].IndTAmount = qt1.QT;
                    this.ME2Indicators[17].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[17].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[17].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[17].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[17].Ind1Amount = qt1.Q1;
                this.ME2Indicators[17].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[17].Ind2Amount = qt1.Q2;
                this.ME2Indicators[17].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[17].Ind3Amount = qt1.Q3;
                this.ME2Indicators[17].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[17].Ind4Amount = qt1.Q4;
                this.ME2Indicators[17].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[17].Ind5Amount = qt1.Q5;
                this.ME2Indicators[17].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 18
                && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
            {
                this.ME2Indicators[18].IndTMAmount = qt1.QTM;
                this.ME2Indicators[18].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[18].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[18].IndTLAmount = qt1.QTL;
                this.ME2Indicators[18].IndTLUnit = lowerci;
                this.ME2Indicators[18].IndTUAmount = qt1.QTU;
                this.ME2Indicators[18].IndTUUnit = upperci;
                this.ME2Indicators[18].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[18].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[18].IndTAmount = qt1.QT;
                    this.ME2Indicators[18].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[18].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[18].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[18].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[18].Ind1Amount = qt1.Q1;
                this.ME2Indicators[18].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[18].Ind2Amount = qt1.Q2;
                this.ME2Indicators[18].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[18].Ind3Amount = qt1.Q3;
                this.ME2Indicators[18].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[18].Ind4Amount = qt1.Q4;
                this.ME2Indicators[18].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[18].Ind5Amount = qt1.Q5;
                this.ME2Indicators[18].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 19
                && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
            {
                this.ME2Indicators[19].IndTMAmount = qt1.QTM;
                this.ME2Indicators[19].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[19].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[19].IndTLAmount = qt1.QTL;
                this.ME2Indicators[19].IndTLUnit = lowerci;
                this.ME2Indicators[19].IndTUAmount = qt1.QTU;
                this.ME2Indicators[19].IndTUUnit = upperci;
                this.ME2Indicators[19].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[19].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[19].IndTAmount = qt1.QT;
                    this.ME2Indicators[19].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[19].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[19].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[19].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[19].Ind1Amount = qt1.Q1;
                this.ME2Indicators[19].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[19].Ind2Amount = qt1.Q2;
                this.ME2Indicators[19].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[19].Ind3Amount = qt1.Q3;
                this.ME2Indicators[19].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[19].Ind4Amount = qt1.Q4;
                this.ME2Indicators[19].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[19].Ind5Amount = qt1.Q5;
                this.ME2Indicators[19].Ind5Unit = qt1.Q5Unit;
            }
            else if (index == 20
                && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
            {
                this.ME2Indicators[20].IndTMAmount = qt1.QTM;
                this.ME2Indicators[20].IndTMUnit = qt1.QTMUnit;
                this.ME2Indicators[20].IndTUnit = qt1.QTUnit;
                this.ME2Indicators[20].IndTLAmount = qt1.QTL;
                this.ME2Indicators[20].IndTLUnit = lowerci;
                this.ME2Indicators[20].IndTUAmount = qt1.QTU;
                this.ME2Indicators[20].IndTUUnit = upperci;
                this.ME2Indicators[20].IndMathResult = qt1.ErrorMessage;
                this.ME2Indicators[20].IndMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.ME2Indicators[20].IndTAmount = qt1.QT;
                    this.ME2Indicators[20].IndTD1Amount = qt1.QTD1;
                    this.ME2Indicators[20].IndTD1Unit = qt1.QTD1Unit;
                    this.ME2Indicators[20].IndTD2Amount = qt1.QTD2;
                    this.ME2Indicators[20].IndTD2Unit = qt1.QTD2Unit;
                }
                this.ME2Indicators[20].Ind1Amount = qt1.Q1;
                this.ME2Indicators[20].Ind1Unit = qt1.Q1Unit;
                this.ME2Indicators[20].Ind2Amount = qt1.Q2;
                this.ME2Indicators[20].Ind2Unit = qt1.Q2Unit;
                this.ME2Indicators[20].Ind3Amount = qt1.Q3;
                this.ME2Indicators[20].Ind3Unit = qt1.Q3Unit;
                this.ME2Indicators[20].Ind4Amount = qt1.Q4;
                this.ME2Indicators[20].Ind4Unit = qt1.Q4Unit;
                this.ME2Indicators[20].Ind5Amount = qt1.Q5;
                this.ME2Indicators[20].Ind5Unit = qt1.Q5Unit;
            }
            else
            {
                //ignore the row
            }
        }
        private int[] SetScoresFromRandomSamples(int[] indicators, Matrix<double> randomSampleData, 
            string[] colNames)
        {
            //1. store the Scores for each row in a double
            List<double> scores = new List<double>();
            //2. set a new ME2Indicator object by copying this
            var me2base = new ME2Indicator(this);
            //3.calculate indicators that are not correlated but may still be in the ind.SetRowQT or Score.MathExpress as constant QTM
            //start with index = 1;
            int iIndNumber = 1;
            List<int> newInds = indicators.ToList();
            //add the score to tell CalculateIndicators not to calculate score yet
            //204
            if (!newInds.Contains(0))
            {
                newInds.Add(0);
            }
            me2base._indicators = newInds.ToArray();
            //this assumes that corrs are only run and stored for scores
            me2base.CalculateIndicators(iIndNumber);
            //but don't double display the ScoreMathResult
            me2base.ME2Indicators[0].IndMathResult = string.Empty;
            //206: don't include the score index position or setrowqt is off by 1 index position
            var scoreIndicators = indicators.Where(i => i != 0);
            //4. use the indicators to set each indicator.QT in the new object from each row of R
            for (int i = 0; i < randomSampleData.RowCount; i++)
            {
                var row = randomSampleData.Row(i);
                //the order of the indicators is the order of the columns
                int j = 0;
                foreach (var ind in scoreIndicators)
                {
                    SetRowQT(me2base, ind, j, row);
                    j++;
                }
                //set sb1Base.Score
                me2base.SetTotalScore(colNames);
                scores.Add(me2base.ME2Indicators[0].IndTAmount);
            }
            List<double> qTs = new List<double>();
            Task<int> tsk = me2base.SetAlgoPRAStats(0, qTs, scores.ToArray());
            string sScoreMathR = string.Concat(this.ME2Indicators[0].IndMathResult, me2base.ME2Indicators[0].IndMathResult);
            //5. 204: reset the indicator.QTs to mean of random sample columns 
            //rather than last row of of randoms
            for (int i = 0; i < randomSampleData.ColumnCount; i++)
            {
                var col = randomSampleData.Column(i);
                //the order of the indicators is the order of the columns
                int j = 0;
                foreach (var ind in scoreIndicators)
                {
                    if (j == i)
                    {
                        SetColumnQT(me2base, ind, col);
                    }
                    j++;
                }
            }
            this.CopyME2IndicatorsProperties(me2base);
            this.ME2Indicators[0].IndMathResult = string.Empty;
            this.ME2Indicators[0].IndMathResult = sScoreMathR;
            //186 add the remaining indicators -they've already been calculated
            me2base.AddAllIndicators(newInds);
            if (!string.IsNullOrEmpty(me2base.ErrorMessage))
            {
                this.ME2Indicators[0].IndMathResult += me2base.ErrorMessage;
                this.ErrorMessage = string.Empty;
            }
            //return the byref indicators
            return newInds.ToArray();
        }
        private void SetRowQT(ME2Indicator me2base, int index, int col, Vector<double> row)
        {
            //the indicators were used to set the order of row.cols
            //so their order, col, will correspond to row.column
            if (index == 0)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[0].IndTAmount = row[col];
                }
            }
            else if (index == 1)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[1].IndTAmount = row[col];
                }
            }
            else if (index == 2)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[2].IndTAmount = row[col];
                }
            }
            else if (index == 3)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[3].IndTAmount = row[col];
                }
            }
            else if (index == 4)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[4].IndTAmount = row[col];
                }
            }
            else if (index == 5)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[5].IndTAmount = row[col];
                }
            }
            else if (index == 6)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[6].IndTAmount = row[col];
                }
            }
            else if (index == 7)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[7].IndTAmount = row[col];
                }
            }
            else if (index == 8)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[8].IndTAmount = row[col];
                }
            }
            else if (index == 9)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[9].IndTAmount = row[col];
                }
            }
            else if (index == 10)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[10].IndTAmount = row[col];
                }
            }
            else if (index == 11)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[11].IndTAmount = row[col];
                }
            }
            else if (index == 12)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[12].IndTAmount = row[col];
                }
            }
            else if (index == 13)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[13].IndTAmount = row[col];
                }
            }
            else if (index == 14)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[14].IndTAmount = row[col];
                }
            }
            else if (index == 15)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[15].IndTAmount = row[col];
                }
            }
            else if (index == 16)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[16].IndTAmount = row[col];
                }
            }
            else if (index == 17)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[17].IndTAmount = row[col];
                }
            }
            else if (index == 18)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[18].IndTAmount = row[col];
                }
            }
            else if (index == 19)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[19].IndTAmount = row[col];
                }
            }
            else if (index == 20)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    me2base.ME2Indicators[20].IndTAmount = row[col];
                }
            }
            else
            {
                //colindex = 0
            }
        }
        private void SetColumnQT(ME2Indicator me2base, int index, Vector<double> column)
        {
            if (index == 0)
            {
                me2base.ME2Indicators[0].IndTAmount = column.Average();
            }
            else if (index == 1)
            {
                me2base.ME2Indicators[1].IndTAmount = column.Average();
            }
            else if (index == 2)
            {
                me2base.ME2Indicators[2].IndTAmount = column.Average();
            }
            else if (index == 3)
            {
                me2base.ME2Indicators[3].IndTAmount = column.Average();
            }
            else if (index == 4)
            {
                me2base.ME2Indicators[4].IndTAmount = column.Average();
            }
            else if (index == 5)
            {
                me2base.ME2Indicators[5].IndTAmount = column.Average();
            }
            else if (index == 6)
            {
                me2base.ME2Indicators[6].IndTAmount = column.Average();
            }
            else if (index == 7)
            {
                me2base.ME2Indicators[7].IndTAmount = column.Average();
            }
            else if (index == 8)
            {
                me2base.ME2Indicators[8].IndTAmount = column.Average();
            }
            else if (index == 9)
            {
                me2base.ME2Indicators[9].IndTAmount = column.Average();
            }
            else if (index == 10)
            {
                me2base.ME2Indicators[10].IndTAmount = column.Average();
            }
            else if (index == 11)
            {
                me2base.ME2Indicators[11].IndTAmount = column.Average();
            }
            else if (index == 12)
            {
                me2base.ME2Indicators[12].IndTAmount = column.Average();
            }
            else if (index == 13)
            {
                me2base.ME2Indicators[13].IndTAmount = column.Average();
            }
            else if (index == 14)
            {
                me2base.ME2Indicators[14].IndTAmount = column.Average();
            }
            else if (index == 15)
            {
                me2base.ME2Indicators[15].IndTAmount = column.Average();
            }
            else if (index == 16)
            {
                me2base.ME2Indicators[16].IndTAmount = column.Average();
            }
            else if (index == 17)
            {
                me2base.ME2Indicators[17].IndTAmount = column.Average();
            }
            else if (index == 18)
            {
                me2base.ME2Indicators[18].IndTAmount = column.Average();
            }
            else if (index == 19)
            {
                me2base.ME2Indicators[19].IndTAmount = column.Average();
            }
            else if (index == 20)
            {
                me2base.ME2Indicators[20].IndTAmount = column.Average();
            }
            else
            {
                //colindex = -1
            }
        }
        private async Task<int> SetRGRIndicatorStats(int index, string[] colNames, List<List<double>> data)
        {
            int algoIndicator = -1;
            if (data.Count > 0)
            {
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (index == 0
                    && (this.ME2Indicators[0].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[0].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index,
                            colNames, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[0].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[0].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[0].IndTLUnit = sLowerCI;
                    this.ME2Indicators[0].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[0].IndTUUnit = sUpperCI;
                    //no condition on type of result yet KISS for now
                    this.ME2Indicators[0].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[0].IndMathResult += rgr.MathResult;
                }
                else if (index == 1
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, 
                            colNames, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[1].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[1].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[1].IndTLUnit = sLowerCI;
                    this.ME2Indicators[1].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[1].IndTUUnit = sUpperCI;
                    //no condition on type of result yet KISS for now
                    this.ME2Indicators[1].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[1].IndMathResult += rgr.MathResult;
                }
                else if (index == 2
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[2].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[2].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[2].IndTLUnit = sLowerCI;
                    this.ME2Indicators[2].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[2].IndTUUnit = sUpperCI;
                    this.ME2Indicators[2].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[2].IndMathResult += rgr.MathResult;
                }
                else if (index == 3
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[3].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[3].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[3].IndTLUnit = sLowerCI;
                    this.ME2Indicators[3].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[3].IndTUUnit = sUpperCI;
                    this.ME2Indicators[3].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[3].IndMathResult += rgr.MathResult;
                }
                else if (index == 4
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[4].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[4].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[4].IndTLUnit = sLowerCI;
                    this.ME2Indicators[4].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[4].IndTUUnit = sUpperCI;
                    this.ME2Indicators[4].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[4].IndMathResult += rgr.MathResult;
                }
                else if (index == 5
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[5].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[5].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[5].IndTLUnit = sLowerCI;
                    this.ME2Indicators[5].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[5].IndTUUnit = sUpperCI;
                    this.ME2Indicators[5].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[5].IndMathResult += rgr.MathResult;
                }
                else if (index == 6
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[6].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[6].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[6].IndTLUnit = sLowerCI;
                    this.ME2Indicators[6].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[6].IndTUUnit = sUpperCI;
                    this.ME2Indicators[6].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[6].IndMathResult += rgr.MathResult;
                }
                else if (index == 7
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[7].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[7].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[7].IndTLUnit = sLowerCI;
                    this.ME2Indicators[7].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[7].IndTUUnit = sUpperCI;
                    this.ME2Indicators[7].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[7].IndMathResult += rgr.MathResult;
                }
                else if (index == 8
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[8].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[8].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[8].IndTLUnit = sLowerCI;
                    this.ME2Indicators[8].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[8].IndTUUnit = sUpperCI;
                    this.ME2Indicators[8].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[8].IndMathResult += rgr.MathResult;
                }
                else if (index == 9
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[9].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[9].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[9].IndTLUnit = sLowerCI;
                    this.ME2Indicators[9].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[9].IndTUUnit = sUpperCI;
                    this.ME2Indicators[9].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[9].IndMathResult += rgr.MathResult;
                }
                else if (index == 10
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[10].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[10].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[10].IndTLUnit = sLowerCI;
                    this.ME2Indicators[10].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[10].IndTUUnit = sUpperCI;
                    this.ME2Indicators[10].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[10].IndMathResult += rgr.MathResult;
                }
                else if (index == 11
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[11].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[11].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[11].IndTLUnit = sLowerCI;
                    this.ME2Indicators[11].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[11].IndTUUnit = sUpperCI;
                    this.ME2Indicators[11].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[11].IndMathResult += rgr.MathResult;
                }
                else if (index == 12
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[12].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[12].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[12].IndTLUnit = sLowerCI;
                    this.ME2Indicators[12].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[12].IndTUUnit = sUpperCI;
                    this.ME2Indicators[12].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[12].IndMathResult += rgr.MathResult;
                }
                else if (index == 13
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[13].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[13].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[13].IndTLUnit = sLowerCI;
                    this.ME2Indicators[13].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[13].IndTUUnit = sUpperCI;
                    this.ME2Indicators[13].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[13].IndMathResult += rgr.MathResult;
                }
                else if (index == 14
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[14].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[14].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[14].IndTLUnit = sLowerCI;
                    this.ME2Indicators[14].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[14].IndTUUnit = sUpperCI;
                    this.ME2Indicators[14].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[14].IndMathResult += rgr.MathResult;
                }
                else if (index == 15
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[15].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[15].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[15].IndTLUnit = sLowerCI;
                    this.ME2Indicators[15].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[15].IndTUUnit = sUpperCI;
                    this.ME2Indicators[15].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[15].IndMathResult += rgr.MathResult;
                }
                else if (index == 16
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[16].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[16].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[16].IndTLUnit = sLowerCI;
                    this.ME2Indicators[16].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[16].IndTUUnit = sUpperCI;
                    this.ME2Indicators[16].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[16].IndMathResult += rgr.MathResult;
                }
                else if (index == 17
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[17].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[17].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[17].IndTLUnit = sLowerCI;
                    this.ME2Indicators[17].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[17].IndTUUnit = sUpperCI;
                    this.ME2Indicators[17].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[17].IndMathResult += rgr.MathResult;
                }
                else if (index == 18
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[18].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[18].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[18].IndTLUnit = sLowerCI;
                    this.ME2Indicators[18].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[18].IndTUUnit = sUpperCI;
                    this.ME2Indicators[18].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[18].IndMathResult += rgr.MathResult;
                }
                else if (index == 19
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[19].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[19].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[19].IndTLUnit = sLowerCI;
                    this.ME2Indicators[19].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[19].IndTUUnit = sUpperCI;
                    this.ME2Indicators[19].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[19].IndMathResult += rgr.MathResult;
                }
                else if (index == 20
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
                {
                    algoIndicator = index;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(index, colNames, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathSubType, this.ME2Indicators[0].IndCILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.ME2Indicators[20].IndTMAmount = rgr.QTPredicted;
                    this.ME2Indicators[20].IndTLAmount = rgr.QTL;
                    this.ME2Indicators[20].IndTLUnit = sLowerCI;
                    this.ME2Indicators[20].IndTUAmount = rgr.QTU;
                    this.ME2Indicators[20].IndTUUnit = sUpperCI;
                    this.ME2Indicators[20].IndMathResult = rgr.ErrorMessage;
                    this.ME2Indicators[20].IndMathResult += rgr.MathResult;
                }
                else
                {
                    //ignore the row
                    
                }
            }
            return algoIndicator;
        }
        private async Task<int> SetANVIndicatorStats(int index, string[] colNames, List<List<double>> data)
        {
            int algoIndicator = -1;
            if (data.Count > 0)
            {
                DevTreks.Extensions.Algorithms.Anova1 anv = new Algorithms.Anova1();
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (index == 0
                    && (this.ME2Indicators[0].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[0].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[0].IndTLAmount = anv.QTL;
                    this.ME2Indicators[0].IndTLUnit = sLowerCI;
                    this.ME2Indicators[0].IndTUAmount = anv.QTU;
                    this.ME2Indicators[0].IndTUUnit = sUpperCI;
                    this.ME2Indicators[0].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[0].IndMathResult += anv.MathResult;
                }
                else if (index == 1
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[1].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[1].IndTLAmount = anv.QTL;
                    this.ME2Indicators[1].IndTLUnit = sLowerCI;
                    this.ME2Indicators[1].IndTUAmount = anv.QTU;
                    this.ME2Indicators[1].IndTUUnit = sUpperCI;
                    this.ME2Indicators[1].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[1].IndMathResult += anv.MathResult;
                }
                else if (index == 2
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[2].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[2].IndTLAmount = anv.QTL;
                    this.ME2Indicators[2].IndTLUnit = sLowerCI;
                    this.ME2Indicators[2].IndTUAmount = anv.QTU;
                    this.ME2Indicators[2].IndTUUnit = sUpperCI;
                    this.ME2Indicators[2].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[2].IndMathResult += anv.MathResult;
                }
                else if (index == 3
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[3].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[3].IndTLAmount = anv.QTL;
                    this.ME2Indicators[3].IndTLUnit = sLowerCI;
                    this.ME2Indicators[3].IndTUAmount = anv.QTU;
                    this.ME2Indicators[3].IndTUUnit = sUpperCI;
                    this.ME2Indicators[3].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[3].IndMathResult += anv.MathResult;
                }
                else if (index == 4
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[4].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[4].IndTLAmount = anv.QTL;
                    this.ME2Indicators[4].IndTLUnit = sLowerCI;
                    this.ME2Indicators[4].IndTUAmount = anv.QTU;
                    this.ME2Indicators[4].IndTUUnit = sUpperCI;
                    this.ME2Indicators[4].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[4].IndMathResult += anv.MathResult;
                }
                else if (index == 5
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[5].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[5].IndTLAmount = anv.QTL;
                    this.ME2Indicators[5].IndTLUnit = sLowerCI;
                    this.ME2Indicators[5].IndTUAmount = anv.QTU;
                    this.ME2Indicators[5].IndTUUnit = sUpperCI;
                    this.ME2Indicators[5].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[5].IndMathResult += anv.MathResult;
                }
                else if (index == 6
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[6].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[6].IndTLAmount = anv.QTL;
                    this.ME2Indicators[6].IndTLUnit = sLowerCI;
                    this.ME2Indicators[6].IndTUAmount = anv.QTU;
                    this.ME2Indicators[6].IndTUUnit = sUpperCI;
                    this.ME2Indicators[6].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[6].IndMathResult += anv.MathResult;
                }
                else if (index == 7
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[7].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[7].IndTLAmount = anv.QTL;
                    this.ME2Indicators[7].IndTLUnit = sLowerCI;
                    this.ME2Indicators[7].IndTUAmount = anv.QTU;
                    this.ME2Indicators[7].IndTUUnit = sUpperCI;
                    this.ME2Indicators[7].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[7].IndMathResult += anv.MathResult;
                }
                else if (index == 8
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[8].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[8].IndTLAmount = anv.QTL;
                    this.ME2Indicators[8].IndTLUnit = sLowerCI;
                    this.ME2Indicators[8].IndTUAmount = anv.QTU;
                    this.ME2Indicators[8].IndTUUnit = sUpperCI;
                    this.ME2Indicators[8].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[8].IndMathResult += anv.MathResult;
                }
                else if (index == 9
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[9].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[9].IndTLAmount = anv.QTL;
                    this.ME2Indicators[9].IndTLUnit = sLowerCI;
                    this.ME2Indicators[9].IndTUAmount = anv.QTU;
                    this.ME2Indicators[9].IndTUUnit = sUpperCI;
                    this.ME2Indicators[9].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[9].IndMathResult += anv.MathResult;
                }
                else if (index == 10
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[10].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[10].IndTLAmount = anv.QTL;
                    this.ME2Indicators[10].IndTLUnit = sLowerCI;
                    this.ME2Indicators[10].IndTUAmount = anv.QTU;
                    this.ME2Indicators[10].IndTUUnit = sUpperCI;
                    this.ME2Indicators[10].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[10].IndMathResult += anv.MathResult;
                }
                else if (index == 11
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[11].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[11].IndTLAmount = anv.QTL;
                    this.ME2Indicators[11].IndTLUnit = sLowerCI;
                    this.ME2Indicators[11].IndTUAmount = anv.QTU;
                    this.ME2Indicators[11].IndTUUnit = sUpperCI;
                    this.ME2Indicators[11].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[11].IndMathResult += anv.MathResult;
                }
                else if (index == 12
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[12].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[12].IndTLAmount = anv.QTL;
                    this.ME2Indicators[12].IndTLUnit = sLowerCI;
                    this.ME2Indicators[12].IndTUAmount = anv.QTU;
                    this.ME2Indicators[12].IndTUUnit = sUpperCI;
                    this.ME2Indicators[12].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[12].IndMathResult += anv.MathResult;
                }
                else if (index == 13
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[13].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[13].IndTLAmount = anv.QTL;
                    this.ME2Indicators[13].IndTLUnit = sLowerCI;
                    this.ME2Indicators[13].IndTUAmount = anv.QTU;
                    this.ME2Indicators[13].IndTUUnit = sUpperCI;
                    this.ME2Indicators[13].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[13].IndMathResult += anv.MathResult;
                }
                else if (index == 14
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[14].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[14].IndTLAmount = anv.QTL;
                    this.ME2Indicators[14].IndTLUnit = sLowerCI;
                    this.ME2Indicators[14].IndTUAmount = anv.QTU;
                    this.ME2Indicators[14].IndTUUnit = sUpperCI;
                    this.ME2Indicators[14].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[14].IndMathResult += anv.MathResult;
                }
                else if (index == 15
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[15].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[15].IndTLAmount = anv.QTL;
                    this.ME2Indicators[15].IndTLUnit = sLowerCI;
                    this.ME2Indicators[15].IndTUAmount = anv.QTU;
                    this.ME2Indicators[15].IndTUUnit = sUpperCI;
                    this.ME2Indicators[15].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[15].IndMathResult += anv.MathResult;
                }
                else if (index == 16
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[16].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[16].IndTLAmount = anv.QTL;
                    this.ME2Indicators[16].IndTLUnit = sLowerCI;
                    this.ME2Indicators[16].IndTUAmount = anv.QTU;
                    this.ME2Indicators[16].IndTUUnit = sUpperCI;
                    this.ME2Indicators[16].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[16].IndMathResult += anv.MathResult;
                }
                else if (index == 17
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[17].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[17].IndTLAmount = anv.QTL;
                    this.ME2Indicators[17].IndTLUnit = sLowerCI;
                    this.ME2Indicators[17].IndTUAmount = anv.QTU;
                    this.ME2Indicators[17].IndTUUnit = sUpperCI;
                    this.ME2Indicators[17].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[17].IndMathResult += anv.MathResult;
                }
                else if (index == 18
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[18].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[18].IndTLAmount = anv.QTL;
                    this.ME2Indicators[18].IndTLUnit = sLowerCI;
                    this.ME2Indicators[18].IndTUAmount = anv.QTU;
                    this.ME2Indicators[18].IndTUUnit = sUpperCI;
                    this.ME2Indicators[18].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[18].IndMathResult += anv.MathResult;
                }
                else if (index == 19
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[19].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[19].IndTLAmount = anv.QTL;
                    this.ME2Indicators[19].IndTLUnit = sLowerCI;
                    this.ME2Indicators[19].IndTUAmount = anv.QTU;
                    this.ME2Indicators[19].IndTUUnit = sUpperCI;
                    this.ME2Indicators[19].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[19].IndMathResult += anv.MathResult;
                }
                else if (index == 20
                    && (this.ME2Indicators[1].IndMathSubType == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
                {
                    algoIndicator = index;
                    anv = InitANV1Algo(index, colNames, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathSubType, this.ME2Indicators[0].IndCILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.ME2Indicators[20].IndTMAmount = anv.QTPredicted;
                    this.ME2Indicators[20].IndTLAmount = anv.QTL;
                    this.ME2Indicators[20].IndTLUnit = sLowerCI;
                    this.ME2Indicators[20].IndTUAmount = anv.QTU;
                    this.ME2Indicators[20].IndTUUnit = sUpperCI;
                    this.ME2Indicators[20].IndMathResult = anv.ErrorMessage;
                    this.ME2Indicators[20].IndMathResult += anv.MathResult;
                }
                else
                {
                    //ignore the row

                }
                //188 assumes 1 analysis is run for analytic results and datasets
                this.MathResult = anv.ErrorMessage;
                this.MathResult += anv.MathResult;
                this.DataToAnalyze = anv.DataToAnalyze;
            }
            return algoIndicator;
        }
        private async Task<int> SetDRR1IndicatorStats(int index, string[] colNames, 
            List<List<string>> data, List<List<string>> colData, List<string> lines2)
        {
            int algoIndicator = -1;
            if (data.Count > 0)
            {
                DevTreks.Extensions.Algorithms.DRR1 drr = new Algorithms.DRR1();
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (index == 0
                    && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(1, index, colNames, qt1, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 0, sLowerCI, sUpperCI);
                }
                else if (index == 1
                    && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(1, index, colNames, qt1, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 1, sLowerCI, sUpperCI);
                }
                else if (index == 2
                    && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(2, index, colNames, qt1, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 2, sLowerCI, sUpperCI);
                }
                else if (index == 3
                    && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(3, index, colNames, qt1, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 3, sLowerCI, sUpperCI);
                }
                else if (index == 4
                    && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(4, index, colNames, qt1, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 4, sLowerCI, sUpperCI);
                }
                else if (index == 5
                    && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(5, index, colNames, qt1, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 5, sLowerCI, sUpperCI);
                }
                else if (index == 6
                    && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(6, index, colNames, qt1, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 6, sLowerCI, sUpperCI);
                }
                else if (index == 7
                    && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(7, index, colNames, qt1, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 7, sLowerCI, sUpperCI);
                }
                else if (index == 8
                    && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(8, index, colNames, qt1, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 8, sLowerCI, sUpperCI);
                }
                else if (index == 9
                    && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(9, index, colNames, qt1, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 9, sLowerCI, sUpperCI);
                }
                else if (index == 10
                    && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(10, index, colNames, qt1, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 10, sLowerCI, sUpperCI);
                }
                else if (index == 11
                    && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(11, index, colNames, qt1, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 11, sLowerCI, sUpperCI);
                }
                else if (index == 12
                    && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(12, index, colNames, qt1, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 12, sLowerCI, sUpperCI);
                }
                else if (index == 13
                    && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(13, index, colNames, qt1, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 13, sLowerCI, sUpperCI);
                }
                else if (index == 14
                    && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(14, index, colNames, qt1, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 14, sLowerCI, sUpperCI);
                }
                else if (index == 15
                    && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(15, index, colNames, qt1, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 15, sLowerCI, sUpperCI);
                }
                else if (index == 16
                    && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(16, index, colNames, qt1, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 16, sLowerCI, sUpperCI);
                }
                else if (index == 17
                    && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(17, index, colNames, qt1, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 17, sLowerCI, sUpperCI);
                }
                else if (index == 18
                    && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(18, index, colNames, qt1, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 18, sLowerCI, sUpperCI);
                }
                else if (index == 19
                    && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(19, index, colNames, qt1, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 19, sLowerCI, sUpperCI);
                }
                else if (index == 20
                    && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    drr = InitDRR1Algo(20, index, colNames, qt1, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, 20, sLowerCI, sUpperCI);
                }
                else
                {
                    //ignore the row

                }
                //188 assumes 1 analysis is run for analytic results and datasets
                this.MathResult = drr.ErrorMessage;
                this.MathResult += drr.MathResult;
                this.DataToAnalyze = drr.DataToAnalyze;
            }
            return algoIndicator;
        }
        private async Task<int> SetDRR2IndicatorStats(int index, string[] colNames,
            List<List<string>> data, List<List<string>> colData, List<string> lines2)
        {
            int algoIndicator = -1;
            if (data.Count > 0)
            {
                DevTreks.Extensions.Algorithms.DRR2 rmi = new Algorithms.DRR2();
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (index == 0
                    && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    //this must to use a zero index
                    rmi = InitDRR2Algo(0, index, colNames, qt1, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 0, sLowerCI, sUpperCI);
                }
                else if (index == 1
                    && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    //this must use a 1 index
                    rmi = InitDRR2Algo(1, index, colNames, qt1, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 1, sLowerCI, sUpperCI);
                }
                else if (index == 2
                    && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(2, index, colNames, qt1, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 2, sLowerCI, sUpperCI);
                }
                else if (index == 3
                    && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(3, index, colNames, qt1, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 3, sLowerCI, sUpperCI);
                }
                else if (index == 4
                    && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(4, index, colNames, qt1, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 4, sLowerCI, sUpperCI);
                }
                else if (index == 5
                    && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(5, index, colNames, qt1, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 5, sLowerCI, sUpperCI);
                }
                else if (index == 6
                    && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(6, index, colNames, qt1, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 6, sLowerCI, sUpperCI);
                }
                else if (index == 7
                    && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(7, index, colNames, qt1, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 7, sLowerCI, sUpperCI);
                }
                else if (index == 8
                    && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(8, index, colNames, qt1, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 8, sLowerCI, sUpperCI);
                }
                else if (index == 9
                    && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(9, index, colNames, qt1, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 9, sLowerCI, sUpperCI);
                }
                else if (index == 10
                    && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(10, index, colNames, qt1, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 10, sLowerCI, sUpperCI);
                }
                else if (index == 11
                    && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(11, index, colNames, qt1, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 11, sLowerCI, sUpperCI);
                }
                else if (index == 12
                    && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(12, index, colNames, qt1, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 12, sLowerCI, sUpperCI);
                }
                else if (index == 13
                    && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(13, index, colNames, qt1, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 13, sLowerCI, sUpperCI);
                }
                else if (index == 14
                    && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(14, index, colNames, qt1, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 14, sLowerCI, sUpperCI);
                }
                else if (index == 15
                    && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(15, index, colNames, qt1, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 15, sLowerCI, sUpperCI);
                }
                else if (index == 16
                    && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(16, index, colNames, qt1, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 16, sLowerCI, sUpperCI);
                }
                else if (index == 17
                    && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(17, index, colNames, qt1, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 17, sLowerCI, sUpperCI);
                }
                else if (index == 18
                    && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(18, index, colNames, qt1, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 18, sLowerCI, sUpperCI);
                }
                else if (index == 19
                    && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(19, index, colNames, qt1, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 19, sLowerCI, sUpperCI);
                }
                else if (index == 20
                    && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
                {
                    algoIndicator = index;
                    IndicatorQT1 qt1 = FillIndicator(index);
                    rmi = InitDRR2Algo(20, index, colNames, qt1, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathSubType,
                        this.ME2Indicators[0].IndCILevel, this.ME2Indicators[0].IndIterations, this.ME2Indicators[0].IndRandom, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, 20, sLowerCI, sUpperCI);
                }
                else
                {
                    //ignore the row

                }
                //188 assumes 1 analysis is run for analytic results and datasets
                this.MathResult = rmi.ErrorMessage;
                this.MathResult += rmi.MathResult;
                this.DataToAnalyze = rmi.DataToAnalyze;
            }
            return algoIndicator;
        }
        public static bool HasMathExpression(string mathExpression)
        {
            bool bHasMathExpress = false;
            if (!string.IsNullOrEmpty(mathExpression)
                && mathExpression != Constants.NONE)
            {
                bHasMathExpress = true;
            }
            return bHasMathExpress;
        }
        public static int GetSiblingIndicatorIndex(int index, int indWithMathExpressIndex, ME2Indicator calcor)
        {
            //indicators are 1 based
            //last sibling indicator holds mathexpression if rules followed
            int iSiblingIndex = -1;
            if (index == 0
                && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[0].IndMathExpression) == false
                && indWithMathExpressIndex != 0)
            {
                iSiblingIndex = 0;
            }
            else if (index == 1
                && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[1].IndMathExpression) == false
                && indWithMathExpressIndex != 1)
            {
                iSiblingIndex = 1;
            }
            else if (index == 2
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[2].IndMathExpression) == false
                && indWithMathExpressIndex != 2)
            {
                iSiblingIndex = 2;
            }
            else if (index == 3
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[3].IndMathExpression) == false
                && indWithMathExpressIndex != 3)
            {
                iSiblingIndex = 3;
            }
            else if (index == 4
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[4].IndMathExpression) == false
                && indWithMathExpressIndex != 4)
            {
                iSiblingIndex = 4;
            }
            else if (index == 5
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[5].IndMathExpression) == false
                && indWithMathExpressIndex != 5)
            {
                iSiblingIndex = 5;
            }
            else if (index == 6
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[6].IndMathExpression) == false
                && indWithMathExpressIndex != 6)
            {
                iSiblingIndex = 6;
            }
            else if (index == 7
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[7].IndMathExpression) == false
                && indWithMathExpressIndex != 7)
            {
                iSiblingIndex = 7;
            }
            else if (index == 8
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[8].IndMathExpression) == false
                && indWithMathExpressIndex != 8)
            {
                iSiblingIndex = 8;
            }
            else if (index == 9
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[9].IndMathExpression) == false
                && indWithMathExpressIndex != 9)
            {
                iSiblingIndex = 9;
            }
            else if (index == 10
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[10].IndMathExpression) == false
                && indWithMathExpressIndex != 10)
            {
                iSiblingIndex = 10;
            }
            else if (index == 11
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[11].IndMathExpression) == false
                && indWithMathExpressIndex != 11)
            {
                iSiblingIndex = 11;
            }
            else if (index == 12
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[12].IndMathExpression) == false
                && indWithMathExpressIndex != 12)
            {
                iSiblingIndex = 12;
            }
            else if (index == 13
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[13].IndMathExpression) == false
                && indWithMathExpressIndex != 13)
            {
                iSiblingIndex = 13;
            }
            else if (index == 14
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[14].IndMathExpression) == false
                && indWithMathExpressIndex != 14)
            {
                iSiblingIndex = 14;
            }
            else if (index == 15
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[15].IndMathExpression) == false
                && indWithMathExpressIndex != 15)
            {
                iSiblingIndex = 15;
            }
            else if (index == 16
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[16].IndMathExpression) == false
                && indWithMathExpressIndex != 16)
            {
                iSiblingIndex = 16;
            }
            else if (index == 17
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[17].IndMathExpression) == false
                && indWithMathExpressIndex != 17)
            {
                iSiblingIndex = 17;
            }
            else if (index == 18
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[18].IndMathExpression) == false
                && indWithMathExpressIndex != 18)
            {
                iSiblingIndex = 18;
            }
            else if (index == 19
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[19].IndMathExpression) == false
                && indWithMathExpressIndex != 19)
            {
                iSiblingIndex = 19;
            }
            else if (index == 20
                 && ME2Statistics.ME2Algos.HasMathExpression(calcor.ME2Indicators[20].IndMathExpression) == false
                && indWithMathExpressIndex != 20)
            {
                iSiblingIndex = 20;
            }
            return iSiblingIndex;
        }
        private async Task<int> SetScriptCloudStats(int index, string[] colNames, 
            string dataURL, string jDataURL)
        {
            int algoIndicator = index;
            System.Threading.CancellationToken ctk = new System.Threading.CancellationToken();
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            if (index == 0
                && (this.ME2Indicators[0].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathType, this.ME2Indicators[0].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[0].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[0].IndTLAmount = script2.QTL;
                this.ME2Indicators[0].IndTLUnit = sLowerCI;
                this.ME2Indicators[0].IndTUAmount = script2.QTU;
                this.ME2Indicators[0].IndTUUnit = sUpperCI;
                //no condition on type of result yet KISS for now
                this.ME2Indicators[0].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[0].IndMathResult += script2.MathResult;
            }
            else if (index == 1
                && (this.ME2Indicators[1].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathType, this.ME2Indicators[1].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[1].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[1].IndTLAmount = script2.QTL;
                this.ME2Indicators[1].IndTLUnit = sLowerCI;
                this.ME2Indicators[1].IndTUAmount = script2.QTU;
                this.ME2Indicators[1].IndTUUnit = sUpperCI;
                //no condition on type of result yet KISS for now
                this.ME2Indicators[1].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[1].IndMathResult += script2.MathResult;
            }
            else if (index == 2
                 && (this.ME2Indicators[2].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathType, this.ME2Indicators[2].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[2].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[2].IndTLAmount = script2.QTL;
                this.ME2Indicators[2].IndTLUnit = sLowerCI;
                this.ME2Indicators[2].IndTUAmount = script2.QTU;
                this.ME2Indicators[2].IndTUUnit = sUpperCI;
                this.ME2Indicators[2].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[2].IndMathResult += script2.MathResult;
            }
            else if (index == 3
                 && (this.ME2Indicators[3].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathType, this.ME2Indicators[3].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[3].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[3].IndTLAmount = script2.QTL;
                this.ME2Indicators[3].IndTLUnit = sLowerCI;
                this.ME2Indicators[3].IndTUAmount = script2.QTU;
                this.ME2Indicators[3].IndTUUnit = sUpperCI;
                this.ME2Indicators[3].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[3].IndMathResult += script2.MathResult;
            }
            else if (index == 4
                 && (this.ME2Indicators[4].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathType, this.ME2Indicators[4].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[4].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[4].IndTLAmount = script2.QTL;
                this.ME2Indicators[4].IndTLUnit = sLowerCI;
                this.ME2Indicators[4].IndTUAmount = script2.QTU;
                this.ME2Indicators[4].IndTUUnit = sUpperCI;
                this.ME2Indicators[4].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[4].IndMathResult += script2.MathResult;
            }
            else if (index == 5
                 && (this.ME2Indicators[5].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathType, this.ME2Indicators[5].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[5].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[5].IndTLAmount = script2.QTL;
                this.ME2Indicators[5].IndTLUnit = sLowerCI;
                this.ME2Indicators[5].IndTUAmount = script2.QTU;
                this.ME2Indicators[5].IndTUUnit = sUpperCI;
                this.ME2Indicators[5].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[5].IndMathResult += script2.MathResult;
            }
            else if (index == 6
                 && (this.ME2Indicators[6].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathType, this.ME2Indicators[6].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[6].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[6].IndTLAmount = script2.QTL;
                this.ME2Indicators[6].IndTLUnit = sLowerCI;
                this.ME2Indicators[6].IndTUAmount = script2.QTU;
                this.ME2Indicators[6].IndTUUnit = sUpperCI;
                this.ME2Indicators[6].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[6].IndMathResult += script2.MathResult;
            }
            else if (index == 7
                 && (this.ME2Indicators[7].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathType, this.ME2Indicators[7].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[7].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[7].IndTLAmount = script2.QTL;
                this.ME2Indicators[7].IndTLUnit = sLowerCI;
                this.ME2Indicators[7].IndTUAmount = script2.QTU;
                this.ME2Indicators[7].IndTUUnit = sUpperCI;
                this.ME2Indicators[7].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[7].IndMathResult += script2.MathResult;
            }
            else if (index == 8
                 && (this.ME2Indicators[8].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathType, this.ME2Indicators[8].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[8].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[8].IndTLAmount = script2.QTL;
                this.ME2Indicators[8].IndTLUnit = sLowerCI;
                this.ME2Indicators[8].IndTUAmount = script2.QTU;
                this.ME2Indicators[8].IndTUUnit = sUpperCI;
                this.ME2Indicators[8].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[8].IndMathResult += script2.MathResult;
            }
            else if (index == 9
                 && (this.ME2Indicators[9].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathType, this.ME2Indicators[9].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[9].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[9].IndTLAmount = script2.QTL;
                this.ME2Indicators[9].IndTLUnit = sLowerCI;
                this.ME2Indicators[9].IndTUAmount = script2.QTU;
                this.ME2Indicators[9].IndTUUnit = sUpperCI;
                this.ME2Indicators[9].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[9].IndMathResult += script2.MathResult;
            }
            else if (index == 10
                 && (this.ME2Indicators[10].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathType, this.ME2Indicators[10].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[10].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[10].IndTLAmount = script2.QTL;
                this.ME2Indicators[10].IndTLUnit = sLowerCI;
                this.ME2Indicators[10].IndTUAmount = script2.QTU;
                this.ME2Indicators[10].IndTUUnit = sUpperCI;
                this.ME2Indicators[10].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[10].IndMathResult += script2.MathResult;
            }
            else if (index == 11
                 && (this.ME2Indicators[11].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathType, this.ME2Indicators[11].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[11].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[11].IndTLAmount = script2.QTL;
                this.ME2Indicators[11].IndTLUnit = sLowerCI;
                this.ME2Indicators[11].IndTUAmount = script2.QTU;
                this.ME2Indicators[11].IndTUUnit = sUpperCI;
                this.ME2Indicators[11].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[11].IndMathResult += script2.MathResult;
            }
            else if (index == 12
                 && (this.ME2Indicators[12].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathType, this.ME2Indicators[12].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[12].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[12].IndTLAmount = script2.QTL;
                this.ME2Indicators[12].IndTLUnit = sLowerCI;
                this.ME2Indicators[12].IndTUAmount = script2.QTU;
                this.ME2Indicators[12].IndTUUnit = sUpperCI;
                this.ME2Indicators[12].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[12].IndMathResult += script2.MathResult;
            }
            else if (index == 13
                 && (this.ME2Indicators[13].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathType, this.ME2Indicators[13].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[13].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[13].IndTLAmount = script2.QTL;
                this.ME2Indicators[13].IndTLUnit = sLowerCI;
                this.ME2Indicators[13].IndTUAmount = script2.QTU;
                this.ME2Indicators[13].IndTUUnit = sUpperCI;
                this.ME2Indicators[13].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[13].IndMathResult += script2.MathResult;
            }
            else if (index == 14
                 && (this.ME2Indicators[14].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathType, this.ME2Indicators[14].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[14].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[14].IndTLAmount = script2.QTL;
                this.ME2Indicators[14].IndTLUnit = sLowerCI;
                this.ME2Indicators[14].IndTUAmount = script2.QTU;
                this.ME2Indicators[14].IndTUUnit = sUpperCI;
                this.ME2Indicators[14].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[14].IndMathResult += script2.MathResult;
            }
            else if (index == 15
                 && (this.ME2Indicators[15].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathType, this.ME2Indicators[15].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[15].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[15].IndTLAmount = script2.QTL;
                this.ME2Indicators[15].IndTLUnit = sLowerCI;
                this.ME2Indicators[15].IndTUAmount = script2.QTU;
                this.ME2Indicators[15].IndTUUnit = sUpperCI;
                this.ME2Indicators[15].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[15].IndMathResult += script2.MathResult;
            }
            else if (index == 16
                 && (this.ME2Indicators[16].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathType, this.ME2Indicators[16].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[16].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[16].IndTLAmount = script2.QTL;
                this.ME2Indicators[16].IndTLUnit = sLowerCI;
                this.ME2Indicators[16].IndTUAmount = script2.QTU;
                this.ME2Indicators[16].IndTUUnit = sUpperCI;
                this.ME2Indicators[16].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[16].IndMathResult += script2.MathResult;
            }
            else if (index == 17
                 && (this.ME2Indicators[17].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathType, this.ME2Indicators[17].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[17].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[17].IndTLAmount = script2.QTL;
                this.ME2Indicators[17].IndTLUnit = sLowerCI;
                this.ME2Indicators[17].IndTUAmount = script2.QTU;
                this.ME2Indicators[17].IndTUUnit = sUpperCI;
                this.ME2Indicators[17].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[17].IndMathResult += script2.MathResult;
            }
            else if (index == 18
                && (this.ME2Indicators[18].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathType, this.ME2Indicators[18].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[18].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[18].IndTLAmount = script2.QTL;
                this.ME2Indicators[18].IndTLUnit = sLowerCI;
                this.ME2Indicators[18].IndTUAmount = script2.QTU;
                this.ME2Indicators[18].IndTUUnit = sUpperCI;
                this.ME2Indicators[18].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[18].IndMathResult += script2.MathResult;
            }
            else if (index == 19
                 && (this.ME2Indicators[19].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathType, this.ME2Indicators[19].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[19].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[19].IndTLAmount = script2.QTL;
                this.ME2Indicators[19].IndTLUnit = sLowerCI;
                this.ME2Indicators[19].IndTUAmount = script2.QTU;
                this.ME2Indicators[19].IndTUUnit = sUpperCI;
                this.ME2Indicators[19].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[19].IndMathResult += script2.MathResult;
            }
            else if (index == 20
                 && (this.ME2Indicators[20].IndMathType == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(index, colNames, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathType, this.ME2Indicators[20].IndMathSubType);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[20].IndTMAmount = script2.QTPredicted;
                this.ME2Indicators[20].IndTLAmount = script2.QTL;
                this.ME2Indicators[20].IndTLUnit = sLowerCI;
                this.ME2Indicators[20].IndTUAmount = script2.QTU;
                this.ME2Indicators[20].IndTUUnit = sUpperCI;
                this.ME2Indicators[20].IndMathResult = script2.ErrorMessage;
                this.ME2Indicators[20].IndMathResult += script2.MathResult;
            }
            else
            {
                //ignore the row
            }
            return algoIndicator;
        }
        private async Task<int> SetScriptWebStats(int index, string[] colNames,
            string dataURL, string jDataURL)
        {
            int algoIndicator = index;
            System.Threading.CancellationToken ctk = new System.Threading.CancellationToken();
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.ME2Indicators[0].IndCILevel.ToString(), Errors.GetMessage("CI_PCT"));
            if (index == 0
                && (this.ME2Indicators[0].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[0].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[0].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[0].IndMathExpression, this.ME2Indicators[0].IndMathType, this.ME2Indicators[0].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[0].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[0].IndTLAmount = script1.QTL;
                this.ME2Indicators[0].IndTLUnit = sLowerCI;
                this.ME2Indicators[0].IndTUAmount = script1.QTU;
                this.ME2Indicators[0].IndTUUnit = sUpperCI;
                //no condition on type of result yet KISS for now
                this.ME2Indicators[0].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[0].IndMathResult += script1.MathResult;
            }
            else if (index == 1
                && (this.ME2Indicators[1].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[1].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[1].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[1].IndMathExpression, this.ME2Indicators[1].IndMathType, this.ME2Indicators[1].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[1].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[1].IndTLAmount = script1.QTL;
                this.ME2Indicators[1].IndTLUnit = sLowerCI;
                this.ME2Indicators[1].IndTUAmount = script1.QTU;
                this.ME2Indicators[1].IndTUUnit = sUpperCI;
                //no condition on type of result yet KISS for now
                this.ME2Indicators[1].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[1].IndMathResult += script1.MathResult;
            }
            else if (index == 2
                && (this.ME2Indicators[2].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[2].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[2].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[2].IndMathExpression, this.ME2Indicators[2].IndMathType, this.ME2Indicators[2].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[2].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[2].IndTLAmount = script1.QTL;
                this.ME2Indicators[2].IndTLUnit = sLowerCI;
                this.ME2Indicators[2].IndTUAmount = script1.QTU;
                this.ME2Indicators[2].IndTUUnit = sUpperCI;
                this.ME2Indicators[2].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[2].IndMathResult += script1.MathResult;
            }
            else if (index == 3
                && (this.ME2Indicators[3].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[3].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[3].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[3].IndMathExpression, this.ME2Indicators[3].IndMathType, this.ME2Indicators[3].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[3].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[3].IndTLAmount = script1.QTL;
                this.ME2Indicators[3].IndTLUnit = sLowerCI;
                this.ME2Indicators[3].IndTUAmount = script1.QTU;
                this.ME2Indicators[3].IndTUUnit = sUpperCI;
                this.ME2Indicators[3].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[3].IndMathResult += script1.MathResult;
            }
            else if (index == 4
                && (this.ME2Indicators[4].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[4].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[4].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[4].IndMathExpression, this.ME2Indicators[4].IndMathType, this.ME2Indicators[4].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[4].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[4].IndTLAmount = script1.QTL;
                this.ME2Indicators[4].IndTLUnit = sLowerCI;
                this.ME2Indicators[4].IndTUAmount = script1.QTU;
                this.ME2Indicators[4].IndTUUnit = sUpperCI;
                this.ME2Indicators[4].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[4].IndMathResult += script1.MathResult;
            }
            else if (index == 5
               && (this.ME2Indicators[5].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[5].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[5].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[5].IndMathExpression, this.ME2Indicators[5].IndMathType, this.ME2Indicators[5].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[5].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[5].IndTLAmount = script1.QTL;
                this.ME2Indicators[5].IndTLUnit = sLowerCI;
                this.ME2Indicators[5].IndTUAmount = script1.QTU;
                this.ME2Indicators[5].IndTUUnit = sUpperCI;
                this.ME2Indicators[5].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[5].IndMathResult += script1.MathResult;
            }
            else if (index == 6
                && (this.ME2Indicators[6].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[6].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[6].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[6].IndMathExpression, this.ME2Indicators[6].IndMathType, this.ME2Indicators[6].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[6].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[6].IndTLAmount = script1.QTL;
                this.ME2Indicators[6].IndTLUnit = sLowerCI;
                this.ME2Indicators[6].IndTUAmount = script1.QTU;
                this.ME2Indicators[6].IndTUUnit = sUpperCI;
                this.ME2Indicators[6].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[6].IndMathResult += script1.MathResult;
            }
            else if (index == 7
                && (this.ME2Indicators[7].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[7].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[7].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[7].IndMathExpression, this.ME2Indicators[7].IndMathType, this.ME2Indicators[7].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[7].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[7].IndTLAmount = script1.QTL;
                this.ME2Indicators[7].IndTLUnit = sLowerCI;
                this.ME2Indicators[7].IndTUAmount = script1.QTU;
                this.ME2Indicators[7].IndTUUnit = sUpperCI;
                this.ME2Indicators[7].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[7].IndMathResult += script1.MathResult;
            }
            else if (index == 8
                && (this.ME2Indicators[8].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[8].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[8].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[8].IndMathExpression, this.ME2Indicators[8].IndMathType, this.ME2Indicators[8].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[8].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[8].IndTLAmount = script1.QTL;
                this.ME2Indicators[8].IndTLUnit = sLowerCI;
                this.ME2Indicators[8].IndTUAmount = script1.QTU;
                this.ME2Indicators[8].IndTUUnit = sUpperCI;
                this.ME2Indicators[8].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[8].IndMathResult += script1.MathResult;
            }
            else if (index == 9
                && (this.ME2Indicators[9].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[9].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[9].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[9].IndMathExpression, this.ME2Indicators[9].IndMathType, this.ME2Indicators[9].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[9].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[9].IndTLAmount = script1.QTL;
                this.ME2Indicators[9].IndTLUnit = sLowerCI;
                this.ME2Indicators[9].IndTUAmount = script1.QTU;
                this.ME2Indicators[9].IndTUUnit = sUpperCI;
                this.ME2Indicators[9].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[9].IndMathResult += script1.MathResult;
            }
            else if (index == 10
                && (this.ME2Indicators[10].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[10].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[10].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[10].IndMathExpression, this.ME2Indicators[10].IndMathType, this.ME2Indicators[10].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[10].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[10].IndTLAmount = script1.QTL;
                this.ME2Indicators[10].IndTLUnit = sLowerCI;
                this.ME2Indicators[10].IndTUAmount = script1.QTU;
                this.ME2Indicators[10].IndTUUnit = sUpperCI;
                this.ME2Indicators[10].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[10].IndMathResult += script1.MathResult;
            }
            else if (index == 11
                && (this.ME2Indicators[11].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[11].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[11].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[11].IndMathExpression, this.ME2Indicators[11].IndMathType, this.ME2Indicators[11].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[11].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[11].IndTLAmount = script1.QTL;
                this.ME2Indicators[11].IndTLUnit = sLowerCI;
                this.ME2Indicators[11].IndTUAmount = script1.QTU;
                this.ME2Indicators[11].IndTUUnit = sUpperCI;
                this.ME2Indicators[11].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[11].IndMathResult += script1.MathResult;
            }
            else if (index == 12
                && (this.ME2Indicators[12].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[12].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[12].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[12].IndMathExpression, this.ME2Indicators[12].IndMathType, this.ME2Indicators[12].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[12].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[12].IndTLAmount = script1.QTL;
                this.ME2Indicators[12].IndTLUnit = sLowerCI;
                this.ME2Indicators[12].IndTUAmount = script1.QTU;
                this.ME2Indicators[12].IndTUUnit = sUpperCI;
                this.ME2Indicators[12].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[12].IndMathResult += script1.MathResult;
            }
            else if (index == 13
                && (this.ME2Indicators[13].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[13].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[13].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[13].IndMathExpression, this.ME2Indicators[13].IndMathType, this.ME2Indicators[13].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[13].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[13].IndTLAmount = script1.QTL;
                this.ME2Indicators[13].IndTLUnit = sLowerCI;
                this.ME2Indicators[13].IndTUAmount = script1.QTU;
                this.ME2Indicators[13].IndTUUnit = sUpperCI;
                this.ME2Indicators[13].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[13].IndMathResult += script1.MathResult;
            }
            else if (index == 14
                && (this.ME2Indicators[14].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[14].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[14].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[14].IndMathExpression, this.ME2Indicators[14].IndMathType, this.ME2Indicators[14].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[14].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[14].IndTLAmount = script1.QTL;
                this.ME2Indicators[14].IndTLUnit = sLowerCI;
                this.ME2Indicators[14].IndTUAmount = script1.QTU;
                this.ME2Indicators[14].IndTUUnit = sUpperCI;
                this.ME2Indicators[14].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[14].IndMathResult += script1.MathResult;
            }
            else if (index == 15
                && (this.ME2Indicators[15].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[15].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[15].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[15].IndMathExpression, this.ME2Indicators[15].IndMathType, this.ME2Indicators[15].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[15].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[15].IndTLAmount = script1.QTL;
                this.ME2Indicators[15].IndTLUnit = sLowerCI;
                this.ME2Indicators[15].IndTUAmount = script1.QTU;
                this.ME2Indicators[15].IndTUUnit = sUpperCI;
                this.ME2Indicators[15].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[15].IndMathResult += script1.MathResult;
            }
            else if (index == 16
                && (this.ME2Indicators[16].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[16].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[16].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[16].IndMathExpression, this.ME2Indicators[16].IndMathType, this.ME2Indicators[16].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[16].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[16].IndTLAmount = script1.QTL;
                this.ME2Indicators[16].IndTLUnit = sLowerCI;
                this.ME2Indicators[16].IndTUAmount = script1.QTU;
                this.ME2Indicators[16].IndTUUnit = sUpperCI;
                this.ME2Indicators[16].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[16].IndMathResult += script1.MathResult;
            }
            else if (index == 17
                && (this.ME2Indicators[17].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[17].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[17].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[17].IndMathExpression, this.ME2Indicators[17].IndMathType, this.ME2Indicators[17].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[17].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[17].IndTLAmount = script1.QTL;
                this.ME2Indicators[17].IndTLUnit = sLowerCI;
                this.ME2Indicators[17].IndTUAmount = script1.QTU;
                this.ME2Indicators[17].IndTUUnit = sUpperCI;
                this.ME2Indicators[17].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[17].IndMathResult += script1.MathResult;
            }
            else if (index == 18
                && (this.ME2Indicators[18].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[18].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[18].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[18].IndMathExpression, this.ME2Indicators[18].IndMathType, this.ME2Indicators[18].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[18].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[18].IndTLAmount = script1.QTL;
                this.ME2Indicators[18].IndTLUnit = sLowerCI;
                this.ME2Indicators[18].IndTUAmount = script1.QTU;
                this.ME2Indicators[18].IndTUUnit = sUpperCI;
                this.ME2Indicators[18].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[18].IndMathResult += script1.MathResult;
            }
            else if (index == 19
                && (this.ME2Indicators[19].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[19].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[19].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[19].IndMathExpression, this.ME2Indicators[19].IndMathType, this.ME2Indicators[19].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[19].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[19].IndTLAmount = script1.QTL;
                this.ME2Indicators[19].IndTLUnit = sLowerCI;
                this.ME2Indicators[19].IndTUAmount = script1.QTU;
                this.ME2Indicators[19].IndTUUnit = sUpperCI;
                this.ME2Indicators[19].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[19].IndMathResult += script1.MathResult;
            }
            else if (index == 20
                && (this.ME2Indicators[20].IndMathType == MATH_TYPES.algorithm2.ToString()
                    || this.ME2Indicators[20].IndMathType == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.ME2Indicators[20].IndMathExpression))
            {
                algoIndicator = index;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(index, colNames, this.ME2Indicators[20].IndMathExpression, this.ME2Indicators[20].IndMathType, this.ME2Indicators[20].IndMathSubType);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.ME2Indicators[20].IndTMAmount = script1.QTPredicted;
                this.ME2Indicators[20].IndTLAmount = script1.QTL;
                this.ME2Indicators[20].IndTLUnit = sLowerCI;
                this.ME2Indicators[20].IndTUAmount = script1.QTU;
                this.ME2Indicators[20].IndTUUnit = sUpperCI;
                this.ME2Indicators[20].IndMathResult = script1.ErrorMessage;
                this.ME2Indicators[20].IndMathResult += script1.MathResult;
            }
            else
            {
                //ignore the row
            }
            return algoIndicator;
        }
       
        
        private DevTreks.Extensions.Algorithms.PRA1 InitPRA1Algo(int index, string subalgo,
            string[] colNames, IndicatorQT1 qt1, int confidInt, int iterations, int random, List<double> qTs)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, qt1.QMathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(index, mathTerms);
            int totalsNeeded = 0;
            DevTreks.Extensions.Algorithms.PRA1 pra
                    = new Algorithms.PRA1(index.ToString(), mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, iterations, random, qTs, qt1, this.CalcParameters);
            return pra;
        }
        private DevTreks.Extensions.Algorithms.PRA2 InitPRA2Algo(
            IndicatorQT1 qt1, int confidInt, int iterations, int random)
        {
            //qt1.qt1s collection will be used to determine labels, math ...
            DevTreks.Extensions.Algorithms.PRA2 pra
                    = new Algorithms.PRA2(confidInt, iterations, random, qt1, this.CalcParameters);
            return pra;
        }
        private DevTreks.Extensions.Algorithms.Regression1 InitRGR1Algo(int index, string[] colNames, 
            string mathExpression, string subalgo, int confidInt)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(index, mathTerms);
            DevTreks.Extensions.Algorithms.Regression1 rgr
                    = new Algorithms.Regression1(mathTerms.ToArray(), colNames, depColNames.ToArray(), 
                        qs.ToArray(), subalgo, confidInt, this.CalcParameters);
            return rgr;
        }
        private DevTreks.Extensions.Algorithms.Anova1 InitANV1Algo(int index, string[] colNames,
            string mathExpression, string subalgo, int confidInt, int totalsNeeded)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
            //List<double> qs = GetQsForMathTerms(index, mathTerms);
            if (totalsNeeded <= 0) totalsNeeded = 3;
            DevTreks.Extensions.Algorithms.Anova1 anv
                    = new Algorithms.Anova1(index.ToString(), mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, this.CalcParameters);
            return anv;
        }
        private DevTreks.Extensions.Algorithms.DRR1 InitDRR1Algo(int indicatorIndex, int index, string[] colNames,
            IndicatorQT1 qt1, string mathExpression, string subalgo, int confidInt, int iterations, 
            int random, int totalsNeeded)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(index, mathTerms);
            if (totalsNeeded <= 0) totalsNeeded = 3;
            DevTreks.Extensions.Algorithms.DRR1 DRR
                    = new Algorithms.DRR1(indicatorIndex, index.ToString(), mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, iterations, random, qs, qt1, this.CalcParameters);
            return DRR;
        }
        private DevTreks.Extensions.Algorithms.DRR2 InitDRR2Algo(int indicatorIndex, int index, string[] colNames,
            IndicatorQT1 qt1, string mathExpression, string subalgo, int confidInt, int iterations,
            int random, int totalsNeeded)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(index, mathTerms);
            if (totalsNeeded <= 0) totalsNeeded = 3;
            DevTreks.Extensions.Algorithms.DRR2 RMI
                    = new Algorithms.DRR2(indicatorIndex, index.ToString(), mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, iterations, random, qs, qt1, this.CalcParameters);
            return RMI;
        }
        private DevTreks.Extensions.Algorithms.Script2 InitScript2Algo(int index, string[] colNames,
           string mathExpression, string algorithm, string subalgorithm)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(index, mathTerms);
            DevTreks.Extensions.Algorithms.Script2 script2
                    = new Algorithms.Script2(mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        qs.ToArray(), algorithm, subalgorithm, this.CalcParameters);
            return script2;
        }
        private DevTreks.Extensions.Algorithms.Script1 InitScript1Algo(int index, string[] colNames,
           string mathExpression, string algorithm, string subalgorithm)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(index, mathTerms);
            DevTreks.Extensions.Algorithms.Script1 script1
                = new Algorithms.Script1(mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        qs.ToArray(), algorithm, subalgorithm, this.CalcParameters);
            return script1;
        }
        //keep for reference
        //private DevTreks.Extensions.Algorithms.BayesMNRegress1 InitBayesRegress1Algo(int index, string[] colNames,
        //   string mathExpression)
        //{
        //    //mathterms define which qamount to send to algorith for predicting a given set of qxs
        //    List<string> mathTerms = new List<string>();
        //    //dependent var colNames found in MathExpression
        //    List<string> depColNames = new List<string>();
        //    GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
        //    List<double> qs = GetQsForMathTerms(index, mathTerms);
        //    DevTreks.Extensions.Algorithms.BayesMNRegress1 bayes1
        //            = new Algorithms.BayesMNRegress1(mathTerms.ToArray(), colNames, depColNames.ToArray(), qs.ToArray(), 
        //                this.ME2Indicators[0].IndIterations);
        //    return bayes1;
        //}
        //private DevTreks.Extensions.Algorithms.BayesInfer1 InitBayes1Algo(int index, string[] colNames,
        //    string mathExpression)
        //{
        //    //mathterms define which qamount to send to algorith for predicting a given set of qxs
        //    List<string> mathTerms = new List<string>();
        //    //dependent var colNames found in MathExpression
        //    List<string> depColNames = new List<string>();
        //    GetDataToAnalyzeColNames(index, mathExpression, colNames, ref depColNames, ref mathTerms);
        //    List<double> qs = GetQsForMathTerms(index, mathTerms);
        //    DevTreks.Extensions.Algorithms.BayesInfer1 bayes1
        //            = new Algorithms.BayesInfer1(mathTerms.ToArray(), colNames, depColNames.ToArray(), qs.ToArray());
        //    return bayes1;
        //}
        //can't be async so byref is ok
        public void GetDataToAnalyzeColNames(int index, string mathExpression, string[] colNames, 
            ref List<string> depColNames, ref List<string> mathTerms)
        {
            string sMathTerm = string.Empty;
            if (colNames == null)
                colNames = new string[] { };
            if (!string.IsNullOrEmpty(mathExpression)
                && colNames.Count() >= 5)
            {
                //regression puts intercept first and is not in the mathexpression
                if (this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                    || this.HasMathType(index, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
                {
                    depColNames.Add("intercept");
                }
                //mathterm colname (3rd term in I1.Q1.energy) equals colname (energy) in colnames
                for (int j = 0; j < colNames.Count(); j++)
                {
                    string sMathTCheckJ = string.Concat(Constants.FILEEXTENSION_DELIMITER, colNames[j]);
                    //dataset naming conventions should make this reliable because ".energy1" must be a unique colname
                    if (mathExpression.ToLower().Contains(sMathTCheckJ.ToLower()))
                    {
                        bool bIsUnique = true;
                        //now make sure another colname doesn't start with this term
                        //only column naming convention is that a columnname can't start with another colname
                        //i.e. if energy is a colname and colnames contain energy1, energy is not allowed
                        for (int k = 0; k < colNames.Count(); k++)
                        {
                            if (colNames[k].ToLower().StartsWith(colNames[j].ToLower())
                                && k != j)
                            {
                                bIsUnique = false;
                            }
                        }
                        if (bIsUnique)
                        {

                            sMathTerm = ParseMathTerm(index, mathExpression, colNames[j]);
                            if (!string.IsNullOrEmpty(sMathTerm))
                            {
                                //it's unique
                                depColNames.Add(colNames[j]);
                                //this also keeps qs in same order as colnames for the right prediction
                                mathTerms.Add(sMathTerm);
                            }
                        }
                    }
                }
            }
        }
        public string ParseMathTerm(int index, string mathExpression, string colName)
        {
            string sMathTerm = string.Empty;
            string sParsedTerm = string.Empty;
            string sMATHTERM = string.Empty;
            //1.9.0 get rid of spaces
            mathExpression = mathExpression.Replace(" ", "");
            //get the index of the first char in colname found in mathexpress (-1 accounts for period delimiter)
            int iEndIndex = mathExpression.IndexOf(colName) - 1;
            if (iEndIndex > 0)
            {
                //=I1.Q1.colname+I2.Q10
                string sParsedMathExpression = mathExpression.Remove(iEndIndex);
                //=I1.Q1.colname+I2
                int iQIndex = sParsedMathExpression.LastIndexOf(Constants.FILEEXTENSION_DELIMITER);
                //now use I to find startindex
                //=I1 or I10
                if (iQIndex > 0)
                {
                    string sParsedQExpression = sParsedMathExpression.Remove(iQIndex);
                    //start index can now be used with endindex
                    int iStartIndex = sParsedQExpression.ToUpper().LastIndexOf("I");
                    sParsedTerm = sParsedMathExpression.Substring(iStartIndex);
                    sMATHTERM = GetMathTerm(index, sParsedTerm);
                }
                if (!string.IsNullOrEmpty(sMATHTERM))
                {
                    //full math terms used
                    sMathTerm = string.Concat(sMATHTERM, Constants.FILEEXTENSION_DELIMITER, colName);
                }
            }
            return sMathTerm;
        }
        private List<string> GetColNamesForMathTerms(string[] colNames, List<string> mathTerms)
        {
            if (colNames == null)
                colNames = new string[] { };
            List<string> cns = new List<string>();
            for (int i = 0; i < colNames.Count(); i++)
            {
                foreach(var term in mathTerms)
                {
                    if (colNames[i].ToLower() == term)
                    {
                        cns.Add(colNames[i]);
                    }
                }
            }
            return cns;
        }
        private string GetMathTerm(int index, string parsedIxQx)
        {
            string sMathTerm = string.Empty;
            bool bHasMathTerm = false;
            //the units must be set correctly
            //and the mathexpress has to contain the var
            if (index == 0)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 0, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 1)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 1, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 2)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 2, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 3)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 3, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 4)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 4, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 5)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 5, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 6)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 6, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 7)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 7, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 8)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 8, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 9)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 9, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 10)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 10, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 11)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 11, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 12)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 12, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 13)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 13, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 14)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 14, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 15)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 15, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 16)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 16, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 17)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 17, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 18)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 18, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 19)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 19, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (index == 20)
            {
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 0))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 1))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 2))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 3))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 4))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 10))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 11))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 12))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 13))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 14))
                {
                    bHasMathTerm = true;
                }
                if (ME2Indicator.HasMathTerm(parsedIxQx, 20, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (bHasMathTerm)
            {
                sMathTerm = parsedIxQx;
            }
            return sMathTerm;
        }
        private List<double> GetQsForMathTerms(int index, List<string> mathTerms)
        {
            //index has to match mathterms (and adjcolnames)
            List<double> qs = new List<double>(mathTerms.Count);
            //the units must be set correctly
            //and the mathexpress has to contain the var
            if (index == 0)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 0, 0))
                {
                    qs.Add(this.ME2Indicators[0].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 0, 1))
                {
                    qs.Add(this.ME2Indicators[0].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 0, 2))
                {
                    qs.Add(this.ME2Indicators[0].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 0, 3))
                {
                    qs.Add(this.ME2Indicators[0].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 0, 4))
                {
                    qs.Add(this.ME2Indicators[0].Ind5Amount);
                }
            }
            if (index == 1)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 1, 0))
                {
                    qs.Add(this.ME2Indicators[1].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 1, 1))
                {
                    qs.Add(this.ME2Indicators[1].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 1, 2))
                {
                    qs.Add(this.ME2Indicators[1].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 1, 3))
                {
                    qs.Add(this.ME2Indicators[1].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 1, 4))
                {
                    qs.Add(this.ME2Indicators[1].Ind5Amount);
                }
            }
            if (index == 2)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 2, 0))
                {
                    qs.Add(this.ME2Indicators[2].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 2, 1))
                {
                    qs.Add(this.ME2Indicators[2].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 2, 2))
                {
                    qs.Add(this.ME2Indicators[2].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 2, 3))
                {
                    qs.Add(this.ME2Indicators[2].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 2, 4))
                {
                    qs.Add(this.ME2Indicators[2].Ind5Amount);
                }
            }
            if (index == 3)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 3, 0))
                {
                    qs.Add(this.ME2Indicators[3].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 3, 1))
                {
                    qs.Add(this.ME2Indicators[3].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 3, 2))
                {
                    qs.Add(this.ME2Indicators[3].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 3, 3))
                {
                    qs.Add(this.ME2Indicators[3].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 3, 4))
                {
                    qs.Add(this.ME2Indicators[3].Ind5Amount);
                }
            }
            if (index == 4)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 4, 0))
                {
                    qs.Add(this.ME2Indicators[4].Ind1Amount);
                }
                if ( HasMathTerm(mathTerms, 4, 1))
                {
                    qs.Add(this.ME2Indicators[4].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 4, 2))
                {
                    qs.Add(this.ME2Indicators[4].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 4, 3))
                {
                    qs.Add(this.ME2Indicators[4].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 4, 4))
                {
                    qs.Add(this.ME2Indicators[4].Ind5Amount);
                }
            }
            if (index == 5)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 5, 0))
                {
                    qs.Add(this.ME2Indicators[5].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 5, 1))
                {
                    qs.Add(this.ME2Indicators[5].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 5, 2))
                {
                    qs.Add(this.ME2Indicators[5].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 5, 3))
                {
                    qs.Add(this.ME2Indicators[5].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 5, 4))
                {
                    qs.Add(this.ME2Indicators[5].Ind5Amount);
                }
            }
            if (index == 6)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 6, 0))
                {
                    qs.Add(this.ME2Indicators[6].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 6, 1))
                {
                    qs.Add(this.ME2Indicators[6].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 6, 2))
                {
                    qs.Add(this.ME2Indicators[6].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 6, 3))
                {
                    qs.Add(this.ME2Indicators[6].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 6, 4))
                {
                    qs.Add(this.ME2Indicators[6].Ind5Amount);
                }
            }
            if (index == 7)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 7, 0))
                {
                    qs.Add(this.ME2Indicators[7].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 7, 1))
                {
                    qs.Add(this.ME2Indicators[7].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 7, 2))
                {
                    qs.Add(this.ME2Indicators[7].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 7, 3))
                {
                    qs.Add(this.ME2Indicators[7].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 7, 4))
                {
                    qs.Add(this.ME2Indicators[7].Ind5Amount);
                }
            }
            if (index == 8)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 8, 0))
                {
                    qs.Add(this.ME2Indicators[8].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 8, 1))
                {
                    qs.Add(this.ME2Indicators[8].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 8, 2))
                {
                    qs.Add(this.ME2Indicators[8].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 8, 3))
                {
                    qs.Add(this.ME2Indicators[8].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 8, 4))
                {
                    qs.Add(this.ME2Indicators[8].Ind5Amount);
                }
            }
            if (index == 9)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 9, 0))
                {
                    qs.Add(this.ME2Indicators[9].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 9, 1))
                {
                    qs.Add(this.ME2Indicators[9].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 9, 2))
                {
                    qs.Add(this.ME2Indicators[9].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 9, 3))
                {
                    qs.Add(this.ME2Indicators[9].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 9, 4))
                {
                    qs.Add(this.ME2Indicators[9].Ind5Amount);
                }
            }
            if (index == 10)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 10, 0))
                {
                    qs.Add(this.ME2Indicators[10].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 10, 1))
                {
                    qs.Add(this.ME2Indicators[10].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 10, 2))
                {
                    qs.Add(this.ME2Indicators[10].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 10, 3))
                {
                    qs.Add(this.ME2Indicators[10].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 10, 4))
                {
                    qs.Add(this.ME2Indicators[10].Ind5Amount);
                }
            }
            if (index == 11)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 11, 0))
                {
                    qs.Add(this.ME2Indicators[11].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 11, 1))
                {
                    qs.Add(this.ME2Indicators[11].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 11, 2))
                {
                    qs.Add(this.ME2Indicators[11].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 11, 3))
                {
                    qs.Add(this.ME2Indicators[11].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 11, 4))
                {
                    qs.Add(this.ME2Indicators[11].Ind5Amount);
                }
            }
            if (index == 12)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 12, 0))
                {
                    qs.Add(this.ME2Indicators[12].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 12, 1))
                {
                    qs.Add(this.ME2Indicators[12].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 12, 2))
                {
                    qs.Add(this.ME2Indicators[12].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 12, 3))
                {
                    qs.Add(this.ME2Indicators[12].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 12, 4))
                {
                    qs.Add(this.ME2Indicators[12].Ind5Amount);
                }
            }
            if (index == 13)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 13, 0))
                {
                    qs.Add(this.ME2Indicators[13].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 13, 1))
                {
                    qs.Add(this.ME2Indicators[13].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 13, 2))
                {
                    qs.Add(this.ME2Indicators[13].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 13, 3))
                {
                    qs.Add(this.ME2Indicators[13].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 13, 4))
                {
                    qs.Add(this.ME2Indicators[13].Ind5Amount);
                }
            }
            if (index == 14)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 14, 0))
                {
                    qs.Add(this.ME2Indicators[14].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 14, 1))
                {
                    qs.Add(this.ME2Indicators[14].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 14, 2))
                {
                    qs.Add(this.ME2Indicators[14].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 14, 3))
                {
                    qs.Add(this.ME2Indicators[14].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 14, 4))
                {
                    qs.Add(this.ME2Indicators[14].Ind5Amount);
                }
            }
            if (index == 15)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 15, 0))
                {
                    qs.Add(this.ME2Indicators[15].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 15, 1))
                {
                    qs.Add(this.ME2Indicators[15].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 15, 2))
                {
                    qs.Add(this.ME2Indicators[15].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 15, 3))
                {
                    qs.Add(this.ME2Indicators[15].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 15, 4))
                {
                    qs.Add(this.ME2Indicators[15].Ind5Amount);
                }
            }
            if (index == 16)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 16, 0))
                {
                    qs.Add(this.ME2Indicators[16].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 16, 1))
                {
                    qs.Add(this.ME2Indicators[16].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 16, 2))
                {
                    qs.Add(this.ME2Indicators[16].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 16, 3))
                {
                    qs.Add(this.ME2Indicators[16].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 16, 4))
                {
                    qs.Add(this.ME2Indicators[16].Ind5Amount);
                }
            }
            if (index == 17)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 17, 0))
                {
                    qs.Add(this.ME2Indicators[17].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 17, 1))
                {
                    qs.Add(this.ME2Indicators[17].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 17, 2))
                {
                    qs.Add(this.ME2Indicators[17].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 17, 3))
                {
                    qs.Add(this.ME2Indicators[17].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 17, 4))
                {
                    qs.Add(this.ME2Indicators[17].Ind5Amount);
                }
            }
            if (index == 18)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 18, 0))
                {
                    qs.Add(this.ME2Indicators[18].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 18, 1))
                {
                    qs.Add(this.ME2Indicators[18].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 18, 2))
                {
                    qs.Add(this.ME2Indicators[18].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 18, 3))
                {
                    qs.Add(this.ME2Indicators[18].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 18, 4))
                {
                    qs.Add(this.ME2Indicators[18].Ind5Amount);
                }
            }
            if (index == 19)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 19, 0))
                {
                    qs.Add(this.ME2Indicators[19].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 19, 1))
                {
                    qs.Add(this.ME2Indicators[19].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 19, 2))
                {
                    qs.Add(this.ME2Indicators[19].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 19, 3))
                {
                    qs.Add(this.ME2Indicators[19].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 19, 4))
                {
                    qs.Add(this.ME2Indicators[19].Ind5Amount);
                }
            }
            if (index == 20)
            {
                if (ME2Indicator.HasMathTerm(mathTerms, 20, 0))
                {
                    qs.Add(this.ME2Indicators[20].Ind1Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 20, 1))
                {
                    qs.Add(this.ME2Indicators[20].Ind2Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 20, 2))
                {
                    qs.Add(this.ME2Indicators[20].Ind3Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 20, 3))
                {
                    qs.Add(this.ME2Indicators[20].Ind4Amount);
                }
                if (ME2Indicator.HasMathTerm(mathTerms, 20, 4))
                {
                    qs.Add(this.ME2Indicators[20].Ind5Amount);
                }
            }
            int iMissingQs = qs.Count - mathTerms.Count;
            for (int i = 0; i < iMissingQs; i++)
            {
                qs.Add(0);
            }
                return qs;
        }
        //private List<double> GetQsForMathTermswUnits(int index, List<string> mathTerms)
        //{
        //    List<double> qs = new List<double>();
        //    //the units must be set correctly
        //    //and the mathexpress has to contain the var
        //    if (index == 1)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind1Unit)
        //            && this.ME2Indicators[1].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 0, 0))
        //        {
        //            qs.Add(this.ME2Indicators[1].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind2Unit)
        //            && this.ME2Indicators[1].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 0, 1))
        //        {
        //            qs.Add(this.ME2Indicators[1].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind3Unit)
        //            && this.ME2Indicators[1].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 0, 2))
        //        {
        //            qs.Add(this.ME2Indicators[1].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind4Unit)
        //            && this.ME2Indicators[1].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 0, 3))
        //        {
        //            qs.Add(this.ME2Indicators[1].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind5Unit)
        //            && this.ME2Indicators[1].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 0, 4))
        //        {
        //            qs.Add(this.ME2Indicators[1].Ind5Amount);
        //        }
        //    }
        //    if (index == 2)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind1Unit)
        //            && this.ME2Indicators[2].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 1, 0))
        //        {
        //            qs.Add(this.ME2Indicators[2].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind2Unit)
        //            && this.ME2Indicators[2].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 1, 1))
        //        {
        //            qs.Add(this.ME2Indicators[2].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind3Unit)
        //            && this.ME2Indicators[2].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 1, 2))
        //        {
        //            qs.Add(this.ME2Indicators[2].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind4Unit)
        //            && this.ME2Indicators[2].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 1, 3))
        //        {
        //            qs.Add(this.ME2Indicators[2].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind5Unit)
        //            && this.ME2Indicators[2].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 1, 4))
        //        {
        //            qs.Add(this.ME2Indicators[2].Ind5Amount);
        //        }
        //    }
        //    if (index == 3)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind1Unit)
        //            && this.ME2Indicators[3].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 2, 0))
        //        {
        //            qs.Add(this.ME2Indicators[3].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind2Unit)
        //            && this.ME2Indicators[3].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 2, 1))
        //        {
        //            qs.Add(this.ME2Indicators[3].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind3Unit)
        //            && this.ME2Indicators[3].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 2, 2))
        //        {
        //            qs.Add(this.ME2Indicators[3].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind4Unit)
        //            && this.ME2Indicators[3].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 2, 3))
        //        {
        //            qs.Add(this.ME2Indicators[3].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind5Unit)
        //            && this.ME2Indicators[3].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 2, 4))
        //        {
        //            qs.Add(this.ME2Indicators[3].Ind5Amount);
        //        }
        //    }
        //    if (index == 4)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind1Unit)
        //            && this.ME2Indicators[4].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 3, 0))
        //        {
        //            qs.Add(this.ME2Indicators[4].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind2Unit)
        //            && this.ME2Indicators[4].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 3, 1))
        //        {
        //            qs.Add(this.ME2Indicators[4].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind3Unit)
        //            && this.ME2Indicators[4].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 3, 2))
        //        {
        //            qs.Add(this.ME2Indicators[4].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind4Unit)
        //            && this.ME2Indicators[4].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 3, 3))
        //        {
        //            qs.Add(this.ME2Indicators[4].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind5Unit)
        //            && this.ME2Indicators[4].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 3, 4))
        //        {
        //            qs.Add(this.ME2Indicators[4].Ind5Amount);
        //        }
        //    }
        //    if (index == 5)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind1Unit)
        //            && this.ME2Indicators[5].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 4, 0))
        //        {
        //            qs.Add(this.ME2Indicators[5].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind2Unit)
        //            && this.ME2Indicators[5].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 4, 1))
        //        {
        //            qs.Add(this.ME2Indicators[5].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind3Unit)
        //            && this.ME2Indicators[5].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 4, 2))
        //        {
        //            qs.Add(this.ME2Indicators[5].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind4Unit)
        //            && this.ME2Indicators[5].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 4, 3))
        //        {
        //            qs.Add(this.ME2Indicators[5].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind5Unit)
        //            && this.ME2Indicators[5].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 4, 4))
        //        {
        //            qs.Add(this.ME2Indicators[5].Ind5Amount);
        //        }
        //    }
        //    if (index == 6)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind1Unit)
        //            && this.ME2Indicators[6].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 5, 0))
        //        {
        //            qs.Add(this.ME2Indicators[6].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind2Unit)
        //            && this.ME2Indicators[6].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 5, 1))
        //        {
        //            qs.Add(this.ME2Indicators[6].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind3Unit)
        //            && this.ME2Indicators[6].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 5, 2))
        //        {
        //            qs.Add(this.ME2Indicators[6].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind4Unit)
        //            && this.ME2Indicators[6].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 5, 3))
        //        {
        //            qs.Add(this.ME2Indicators[6].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind5Unit)
        //            && this.ME2Indicators[6].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 5, 4))
        //        {
        //            qs.Add(this.ME2Indicators[6].Ind5Amount);
        //        }
        //    }
        //    if (index == 7)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind1Unit)
        //            && this.ME2Indicators[7].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 6, 0))
        //        {
        //            qs.Add(this.ME2Indicators[7].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind2Unit)
        //            && this.ME2Indicators[7].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 6, 1))
        //        {
        //            qs.Add(this.ME2Indicators[7].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind3Unit)
        //            && this.ME2Indicators[7].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 6, 2))
        //        {
        //            qs.Add(this.ME2Indicators[7].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind4Unit)
        //            && this.ME2Indicators[7].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 6, 3))
        //        {
        //            qs.Add(this.ME2Indicators[7].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind5Unit)
        //            && this.ME2Indicators[7].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 6, 4))
        //        {
        //            qs.Add(this.ME2Indicators[7].Ind5Amount);
        //        }
        //    }
        //    if (index == 8)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind1Unit)
        //            && this.ME2Indicators[8].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 7, 0))
        //        {
        //            qs.Add(this.ME2Indicators[8].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind2Unit)
        //            && this.ME2Indicators[8].Ind2Unit != Constants.NONE
        //             && HasMathTerm(mathTerms, 7, 1))
        //        {
        //            qs.Add(this.ME2Indicators[8].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind3Unit)
        //            && this.ME2Indicators[8].Ind3Unit != Constants.NONE
        //             && HasMathTerm(mathTerms, 7, 2))
        //        {
        //            qs.Add(this.ME2Indicators[8].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind4Unit)
        //            && this.ME2Indicators[8].Ind4Unit != Constants.NONE
        //             && HasMathTerm(mathTerms, 7, 3))
        //        {
        //            qs.Add(this.ME2Indicators[8].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind5Unit)
        //            && this.ME2Indicators[8].Ind5Unit != Constants.NONE
        //             && HasMathTerm(mathTerms, 7, 4))
        //        {
        //            qs.Add(this.ME2Indicators[8].Ind5Amount);
        //        }
        //    }
        //    if (index == 9)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind1Unit)
        //            && this.ME2Indicators[9].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 8, 0))
        //        {
        //            qs.Add(this.ME2Indicators[9].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind2Unit)
        //            && this.ME2Indicators[9].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 8, 1))
        //        {
        //            qs.Add(this.ME2Indicators[9].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind3Unit)
        //            && this.ME2Indicators[9].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 8, 2))
        //        {
        //            qs.Add(this.ME2Indicators[9].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind4Unit)
        //            && this.ME2Indicators[9].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 8, 3))
        //        {
        //            qs.Add(this.ME2Indicators[9].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind5Unit)
        //            && this.ME2Indicators[9].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 8, 4))
        //        {
        //            qs.Add(this.ME2Indicators[9].Ind5Amount);
        //        }
        //    }
        //    if (index == 10)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind1Unit)
        //            && this.ME2Indicators[10].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 9, 0))
        //        {
        //            qs.Add(this.ME2Indicators[10].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind2Unit)
        //            && this.ME2Indicators[10].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 9, 1))
        //        {
        //            qs.Add(this.ME2Indicators[10].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind3Unit)
        //            && this.ME2Indicators[10].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 9, 2))
        //        {
        //            qs.Add(this.ME2Indicators[10].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind4Unit)
        //            && this.ME2Indicators[10].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 9, 3))
        //        {
        //            qs.Add(this.ME2Indicators[10].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind5Unit)
        //            && this.ME2Indicators[10].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 9, 4))
        //        {
        //            qs.Add(this.ME2Indicators[10].Ind5Amount);
        //        }
        //    }
        //    if (index == 11)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind1Unit)
        //            && this.ME2Indicators[11].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 10, 0))
        //        {
        //            qs.Add(this.ME2Indicators[11].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind2Unit)
        //            && this.ME2Indicators[11].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 10, 1))
        //        {
        //            qs.Add(this.ME2Indicators[11].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind3Unit)
        //            && this.ME2Indicators[11].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 10, 2))
        //        {
        //            qs.Add(this.ME2Indicators[11].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind4Unit)
        //            && this.ME2Indicators[11].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 10, 3))
        //        {
        //            qs.Add(this.ME2Indicators[11].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind5Unit)
        //            && this.ME2Indicators[11].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 10, 4))
        //        {
        //            qs.Add(this.ME2Indicators[11].Ind5Amount);
        //        }
        //    }
        //    if (index == 12)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind1Unit)
        //            && this.ME2Indicators[12].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 11, 0))
        //        {
        //            qs.Add(this.ME2Indicators[12].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind2Unit)
        //            && this.ME2Indicators[12].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 11, 1))
        //        {
        //            qs.Add(this.ME2Indicators[12].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind3Unit)
        //            && this.ME2Indicators[12].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 11, 2))
        //        {
        //            qs.Add(this.ME2Indicators[12].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind4Unit)
        //            && this.ME2Indicators[12].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 11, 3))
        //        {
        //            qs.Add(this.ME2Indicators[12].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind5Unit)
        //            && this.ME2Indicators[12].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 11, 4))
        //        {
        //            qs.Add(this.ME2Indicators[12].Ind5Amount);
        //        }
        //    }
        //    if (index == 13)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind1Unit)
        //            && this.ME2Indicators[13].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 12, 0))
        //        {
        //            qs.Add(this.ME2Indicators[13].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind2Unit)
        //            && this.ME2Indicators[13].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 12, 1))
        //        {
        //            qs.Add(this.ME2Indicators[13].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind3Unit)
        //            && this.ME2Indicators[13].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 12, 2))
        //        {
        //            qs.Add(this.ME2Indicators[13].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind4Unit)
        //            && this.ME2Indicators[13].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 12, 3))
        //        {
        //            qs.Add(this.ME2Indicators[13].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind5Unit)
        //            && this.ME2Indicators[13].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 12, 4))
        //        {
        //            qs.Add(this.ME2Indicators[13].Ind5Amount);
        //        }
        //    }
        //    if (index == 14)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind1Unit)
        //            && this.ME2Indicators[14].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 13, 0))
        //        {
        //            qs.Add(this.ME2Indicators[14].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind2Unit)
        //            && this.ME2Indicators[14].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 13, 1))
        //        {
        //            qs.Add(this.ME2Indicators[14].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind3Unit)
        //            && this.ME2Indicators[14].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 13, 2))
        //        {
        //            qs.Add(this.ME2Indicators[14].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind4Unit)
        //            && this.ME2Indicators[14].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 13, 3))
        //        {
        //            qs.Add(this.ME2Indicators[14].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind5Unit)
        //            && this.ME2Indicators[14].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 13, 4))
        //        {
        //            qs.Add(this.ME2Indicators[14].Ind5Amount);
        //        }
        //    }
        //    if (index == 15)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind1Unit)
        //            && this.ME2Indicators[15].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 14, 0))
        //        {
        //            qs.Add(this.ME2Indicators[15].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind2Unit)
        //            && this.ME2Indicators[15].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 14, 1))
        //        {
        //            qs.Add(this.ME2Indicators[15].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind3Unit)
        //            && this.ME2Indicators[15].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 14, 2))
        //        {
        //            qs.Add(this.ME2Indicators[15].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind4Unit)
        //            && this.ME2Indicators[15].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 14, 3))
        //        {
        //            qs.Add(this.ME2Indicators[15].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind5Unit)
        //            && this.ME2Indicators[15].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 14, 4))
        //        {
        //            qs.Add(this.ME2Indicators[15].Ind5Amount);
        //        }
        //    }
        //    if (index == 16)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind1Unit)
        //            && this.ME2Indicators[16].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 15, 0))
        //        {
        //            qs.Add(this.ME2Indicators[16].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind2Unit)
        //            && this.ME2Indicators[16].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 15, 1))
        //        {
        //            qs.Add(this.ME2Indicators[16].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind3Unit)
        //            && this.ME2Indicators[16].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 15, 2))
        //        {
        //            qs.Add(this.ME2Indicators[16].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind4Unit)
        //            && this.ME2Indicators[16].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 15, 3))
        //        {
        //            qs.Add(this.ME2Indicators[16].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind5Unit)
        //            && this.ME2Indicators[16].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 15, 4))
        //        {
        //            qs.Add(this.ME2Indicators[16].Ind5Amount);
        //        }
        //    }
        //    if (index == 17)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind1Unit)
        //            && this.ME2Indicators[17].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 16, 0))
        //        {
        //            qs.Add(this.ME2Indicators[17].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind2Unit)
        //            && this.ME2Indicators[17].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 16, 1))
        //        {
        //            qs.Add(this.ME2Indicators[17].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind3Unit)
        //            && this.ME2Indicators[17].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 16, 2))
        //        {
        //            qs.Add(this.ME2Indicators[17].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind4Unit)
        //            && this.ME2Indicators[17].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 16, 3))
        //        {
        //            qs.Add(this.ME2Indicators[17].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind5Unit)
        //            && this.ME2Indicators[17].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 16, 4))
        //        {
        //            qs.Add(this.ME2Indicators[17].Ind5Amount);
        //        }
        //    }
        //    if (index == 18)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind1Unit)
        //            && this.ME2Indicators[18].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 17, 0))
        //        {
        //            qs.Add(this.ME2Indicators[18].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind2Unit)
        //            && this.ME2Indicators[18].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 17, 1))
        //        {
        //            qs.Add(this.ME2Indicators[18].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind3Unit)
        //            && this.ME2Indicators[18].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 17, 2))
        //        {
        //            qs.Add(this.ME2Indicators[18].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind4Unit)
        //            && this.ME2Indicators[18].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 17, 3))
        //        {
        //            qs.Add(this.ME2Indicators[18].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind5Unit)
        //            && this.ME2Indicators[18].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 17, 4))
        //        {
        //            qs.Add(this.ME2Indicators[18].Ind5Amount);
        //        }
        //    }
        //    if (index == 19)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind1Unit)
        //            && this.ME2Indicators[19].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 18, 0))
        //        {
        //            qs.Add(this.ME2Indicators[19].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind2Unit)
        //            && this.ME2Indicators[19].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 18, 1))
        //        {
        //            qs.Add(this.ME2Indicators[19].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind3Unit)
        //            && this.ME2Indicators[19].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 18, 2))
        //        {
        //            qs.Add(this.ME2Indicators[19].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind4Unit)
        //            && this.ME2Indicators[19].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 18, 3))
        //        {
        //            qs.Add(this.ME2Indicators[19].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind5Unit)
        //            && this.ME2Indicators[19].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 18, 4))
        //        {
        //            qs.Add(this.ME2Indicators[19].Ind5Amount);
        //        }
        //    }
        //    if (index == 20)
        //    {
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind1Unit)
        //            && this.ME2Indicators[20].Ind1Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 19, 0))
        //        {
        //            qs.Add(this.ME2Indicators[20].Ind1Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind2Unit)
        //            && this.ME2Indicators[20].Ind2Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 19, 1))
        //        {
        //            qs.Add(this.ME2Indicators[20].Ind2Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind3Unit)
        //            && this.ME2Indicators[20].Ind3Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 19, 2))
        //        {
        //            qs.Add(this.ME2Indicators[20].Ind3Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind4Unit)
        //            && this.ME2Indicators[20].Ind4Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 19, 3))
        //        {
        //            qs.Add(this.ME2Indicators[20].Ind4Amount);
        //        }
        //        if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind5Unit)
        //            && this.ME2Indicators[20].Ind5Unit != Constants.NONE
        //            && HasMathTerm(mathTerms, 19, 4))
        //        {
        //            qs.Add(this.ME2Indicators[20].Ind5Amount);
        //        }
        //    }
        //    return qs;
        //}
       
        
        private List<double> GetQsForLabel(int index)
        {
            List<double> qs = new List<double>();
            if (index == 0)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[0].Ind1Unit)
                    && this.ME2Indicators[0].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[0].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[0].Ind2Unit)
                    && this.ME2Indicators[0].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[0].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[0].Ind3Unit)
                    && this.ME2Indicators[0].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[0].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[0].Ind4Unit)
                    && this.ME2Indicators[0].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[0].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[0].Ind5Unit)
                    && this.ME2Indicators[0].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[0].Ind5Amount);
                }
            }
            if (index == 1)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind1Unit)
                    && this.ME2Indicators[1].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[1].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind2Unit)
                    && this.ME2Indicators[1].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[1].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind3Unit)
                    && this.ME2Indicators[1].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[1].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind4Unit)
                    && this.ME2Indicators[1].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[1].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[1].Ind5Unit)
                    && this.ME2Indicators[1].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[1].Ind5Amount);
                }
            }
            if (index == 2)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind1Unit)
                    && this.ME2Indicators[2].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[2].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind2Unit)
                    && this.ME2Indicators[2].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[2].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind3Unit)
                    && this.ME2Indicators[2].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[2].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind4Unit)
                    && this.ME2Indicators[2].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[2].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[2].Ind5Unit)
                    && this.ME2Indicators[2].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[2].Ind5Amount);
                }
            }
            if (index == 3)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind1Unit)
                    && this.ME2Indicators[3].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[3].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind2Unit)
                    && this.ME2Indicators[3].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[3].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind3Unit)
                    && this.ME2Indicators[3].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[3].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind4Unit)
                    && this.ME2Indicators[3].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[3].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[3].Ind5Unit)
                    && this.ME2Indicators[3].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[3].Ind5Amount);
                }
            }
            if (index == 4)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind1Unit)
                    && this.ME2Indicators[4].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[4].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind2Unit)
                    && this.ME2Indicators[4].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[4].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind3Unit)
                    && this.ME2Indicators[4].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[4].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind4Unit)
                    && this.ME2Indicators[4].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[4].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[4].Ind5Unit)
                    && this.ME2Indicators[4].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[4].Ind5Amount);
                }
            }
            if (index == 5)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind1Unit)
                    && this.ME2Indicators[5].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[5].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind2Unit)
                    && this.ME2Indicators[5].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[5].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind3Unit)
                    && this.ME2Indicators[5].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[5].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind4Unit)
                    && this.ME2Indicators[5].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[5].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[5].Ind5Unit)
                    && this.ME2Indicators[5].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[5].Ind5Amount);
                }
            }
            if (index == 6)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind1Unit)
                    && this.ME2Indicators[6].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[6].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind2Unit)
                    && this.ME2Indicators[6].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[6].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind3Unit)
                    && this.ME2Indicators[6].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[6].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind4Unit)
                    && this.ME2Indicators[6].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[6].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[6].Ind5Unit)
                    && this.ME2Indicators[6].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[6].Ind5Amount);
                }
            }
            if (index == 7)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind1Unit)
                    && this.ME2Indicators[7].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[7].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind2Unit)
                    && this.ME2Indicators[7].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[7].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind3Unit)
                    && this.ME2Indicators[7].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[7].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind4Unit)
                    && this.ME2Indicators[7].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[7].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[7].Ind5Unit)
                    && this.ME2Indicators[7].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[7].Ind5Amount);
                }
            }
            if (index == 8)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind1Unit)
                    && this.ME2Indicators[8].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[8].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind2Unit)
                    && this.ME2Indicators[8].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[8].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind3Unit)
                    && this.ME2Indicators[8].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[8].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind4Unit)
                    && this.ME2Indicators[8].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[8].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[8].Ind5Unit)
                    && this.ME2Indicators[8].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[8].Ind5Amount);
                }
            }
            if (index == 9)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind1Unit)
                    && this.ME2Indicators[9].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[9].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind2Unit)
                    && this.ME2Indicators[9].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[9].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind3Unit)
                    && this.ME2Indicators[9].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[9].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind4Unit)
                    && this.ME2Indicators[9].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[9].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[9].Ind5Unit)
                    && this.ME2Indicators[9].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[9].Ind5Amount);
                }
            }
            if (index == 10)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind1Unit)
                    && this.ME2Indicators[10].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[10].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind2Unit)
                    && this.ME2Indicators[10].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[10].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind3Unit)
                    && this.ME2Indicators[10].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[10].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind4Unit)
                    && this.ME2Indicators[10].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[10].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[10].Ind5Unit)
                    && this.ME2Indicators[10].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[10].Ind5Amount);
                }
            }
            if (index == 11)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind1Unit)
                    && this.ME2Indicators[11].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[11].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind2Unit)
                    && this.ME2Indicators[11].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[11].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind3Unit)
                    && this.ME2Indicators[11].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[11].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind4Unit)
                    && this.ME2Indicators[11].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[11].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[11].Ind5Unit)
                    && this.ME2Indicators[11].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[11].Ind5Amount);
                }
            }
            if (index == 12)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind1Unit)
                    && this.ME2Indicators[12].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[12].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind2Unit)
                    && this.ME2Indicators[12].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[12].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind3Unit)
                    && this.ME2Indicators[12].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[12].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind4Unit)
                    && this.ME2Indicators[12].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[12].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[12].Ind5Unit)
                    && this.ME2Indicators[12].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[12].Ind5Amount);
                }
            }
            if (index == 13)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind1Unit)
                    && this.ME2Indicators[13].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[13].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind2Unit)
                    && this.ME2Indicators[13].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[13].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind3Unit)
                    && this.ME2Indicators[13].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[13].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind4Unit)
                    && this.ME2Indicators[13].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[13].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[13].Ind5Unit)
                    && this.ME2Indicators[13].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[13].Ind5Amount);
                }
            }
            if (index == 14)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind1Unit)
                    && this.ME2Indicators[14].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[14].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind2Unit)
                    && this.ME2Indicators[14].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[14].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind3Unit)
                    && this.ME2Indicators[14].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[14].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind4Unit)
                    && this.ME2Indicators[14].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[14].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[14].Ind5Unit)
                    && this.ME2Indicators[14].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[14].Ind5Amount);
                }
            }
            if (index == 15)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind1Unit)
                    && this.ME2Indicators[15].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[15].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind2Unit)
                    && this.ME2Indicators[15].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[15].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind3Unit)
                    && this.ME2Indicators[15].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[15].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind4Unit)
                    && this.ME2Indicators[15].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[15].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[15].Ind5Unit)
                    && this.ME2Indicators[15].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[15].Ind5Amount);
                }
            }
            if (index == 16)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind1Unit)
                    && this.ME2Indicators[16].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[16].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind2Unit)
                    && this.ME2Indicators[16].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[16].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind3Unit)
                    && this.ME2Indicators[16].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[16].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind4Unit)
                    && this.ME2Indicators[16].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[16].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[16].Ind5Unit)
                    && this.ME2Indicators[16].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[16].Ind5Amount);
                }
            }
            if (index == 17)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind1Unit)
                    && this.ME2Indicators[17].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[17].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind2Unit)
                    && this.ME2Indicators[17].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[17].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind3Unit)
                    && this.ME2Indicators[17].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[17].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind4Unit)
                    && this.ME2Indicators[17].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[17].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[17].Ind5Unit)
                    && this.ME2Indicators[17].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[17].Ind5Amount);
                }
            }
            if (index == 18)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind1Unit)
                    && this.ME2Indicators[18].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[18].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind2Unit)
                    && this.ME2Indicators[18].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[18].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind3Unit)
                    && this.ME2Indicators[18].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[18].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind4Unit)
                    && this.ME2Indicators[18].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[18].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[18].Ind5Unit)
                    && this.ME2Indicators[18].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[18].Ind5Amount);
                }
            }
            if (index == 19)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind1Unit)
                    && this.ME2Indicators[19].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[19].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind2Unit)
                    && this.ME2Indicators[19].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[19].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind3Unit)
                    && this.ME2Indicators[19].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[19].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind4Unit)
                    && this.ME2Indicators[19].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[19].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[19].Ind5Unit)
                    && this.ME2Indicators[19].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[19].Ind5Amount);
                }
            }
            if (index == 20)
            {
                if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind1Unit)
                    && this.ME2Indicators[20].Ind1Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[20].Ind1Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind2Unit)
                    && this.ME2Indicators[20].Ind2Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[20].Ind2Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind3Unit)
                    && this.ME2Indicators[20].Ind3Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[20].Ind3Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind4Unit)
                    && this.ME2Indicators[20].Ind4Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[20].Ind4Amount);
                }
                if (!string.IsNullOrEmpty(this.ME2Indicators[20].Ind5Unit)
                    && this.ME2Indicators[20].Ind5Unit != Constants.NONE)
                {
                    qs.Add(this.ME2Indicators[20].Ind5Amount);
                }
            }
            return qs;
        }
        private DevTreks.Extensions.Algorithms.SimulatedAnnealing1 InitSA1Algo(int index)
        {
            DevTreks.Extensions.Algorithms.SimulatedAnnealing1 sa
                    = new Algorithms.SimulatedAnnealing1(0, 0, 0, 0, this.CalcParameters);
            if (index == 0)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[0].Ind1Amount, this.ME2Indicators[0].Ind2Amount,
                   this.ME2Indicators[0].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 1)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[1].Ind1Amount, this.ME2Indicators[1].Ind2Amount,
                   this.ME2Indicators[1].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 2)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[2].Ind1Amount, this.ME2Indicators[2].Ind2Amount,
                   this.ME2Indicators[2].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 3)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[3].Ind1Amount, this.ME2Indicators[3].Ind2Amount,
                   this.ME2Indicators[3].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 4)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[4].Ind1Amount, this.ME2Indicators[4].Ind2Amount,
                   this.ME2Indicators[4].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 5)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[5].Ind1Amount, this.ME2Indicators[5].Ind2Amount,
                   this.ME2Indicators[5].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 6)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[6].Ind1Amount, this.ME2Indicators[6].Ind2Amount,
                   this.ME2Indicators[6].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 7)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[7].Ind1Amount, this.ME2Indicators[7].Ind2Amount,
                   this.ME2Indicators[7].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 8)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[8].Ind1Amount, this.ME2Indicators[8].Ind2Amount,
                   this.ME2Indicators[8].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 9)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[9].Ind1Amount, this.ME2Indicators[9].Ind2Amount,
                   this.ME2Indicators[9].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 10)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[10].Ind1Amount, this.ME2Indicators[10].Ind2Amount,
                   this.ME2Indicators[10].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 11)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[11].Ind1Amount, this.ME2Indicators[11].Ind2Amount,
                   this.ME2Indicators[11].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 12)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[12].Ind1Amount, this.ME2Indicators[12].Ind2Amount,
                   this.ME2Indicators[12].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 13)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[13].Ind1Amount, this.ME2Indicators[13].Ind2Amount,
                   this.ME2Indicators[13].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 14)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[14].Ind1Amount, this.ME2Indicators[14].Ind2Amount,
                   this.ME2Indicators[14].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 15)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[15].Ind1Amount, this.ME2Indicators[15].Ind2Amount,
                   this.ME2Indicators[15].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 16)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[16].Ind1Amount, this.ME2Indicators[16].Ind2Amount,
                   this.ME2Indicators[16].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 17)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[17].Ind1Amount, this.ME2Indicators[17].Ind2Amount,
                   this.ME2Indicators[17].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 18)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[18].Ind1Amount, this.ME2Indicators[18].Ind2Amount,
                   this.ME2Indicators[18].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 19)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[19].Ind1Amount, this.ME2Indicators[19].Ind2Amount,
                   this.ME2Indicators[19].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else if (index == 20)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.ME2Indicators[20].Ind1Amount, this.ME2Indicators[20].Ind2Amount,
                   this.ME2Indicators[20].Ind3Amount, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            }
            else
            {
            }
            return sa;
        }
        private void SetSA1AlgoRanges(int index, DevTreks.Extensions.Algorithms.SimulatedAnnealing1 sa)
        {
            string[] colNames = new List<string>().ToArray();
            List<double> qTs = new List<double>();
            if (index == 0)
            {
                this.ME2Indicators[0].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[0].IndTMAmount = sa.BestEnergy;
                //regular high and low estimation
                SetPRAIndicatorStats(index, colNames, qTs);
                //SetTotalRange1();
                //no condition on type of result yet KISS for now
                this.ME2Indicators[0].IndMathResult += sa.MathResult;
            }
            else if (index == 1)
            {
                this.ME2Indicators[1].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[1].IndTMAmount = sa.BestEnergy;
                //regular high and low estimation
                SetPRAIndicatorStats(index, colNames, qTs);
                //SetTotalRange1();
                //no condition on type of result yet KISS for now
                this.ME2Indicators[1].IndMathResult += sa.MathResult;
            }
            else if (index == 2)
            {
                this.ME2Indicators[2].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[2].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[2].IndMathResult += sa.MathResult;
            }
            else if (index == 3)
            {
                this.ME2Indicators[3].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[3].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[3].IndMathResult += sa.MathResult;
            }
            else if (index == 4)
            {
                this.ME2Indicators[4].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[4].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[4].IndMathResult += sa.MathResult;
            }
            else if (index == 5)
            {
                this.ME2Indicators[5].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[5].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[5].IndMathResult += sa.MathResult;
            }
            else if (index == 6)
            {
                this.ME2Indicators[6].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[6].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[6].IndMathResult += sa.MathResult;
            }
            else if (index == 7)
            {
                this.ME2Indicators[7].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[7].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[7].IndMathResult += sa.MathResult;
            }
            else if (index == 8)
            {
                this.ME2Indicators[8].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[8].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[8].IndMathResult += sa.MathResult;
            }
            else if (index == 9)
            {
                this.ME2Indicators[9].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[9].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[9].IndMathResult += sa.MathResult;
            }
            else if (index == 10)
            {
                this.ME2Indicators[10].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[10].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[10].IndMathResult += sa.MathResult;
            }
            else if (index == 11)
            {
                this.ME2Indicators[11].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[11].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[11].IndMathResult += sa.MathResult;
            }
            else if (index == 12)
            {
                this.ME2Indicators[12].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[12].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[12].IndMathResult += sa.MathResult;
            }
            else if (index == 13)
            {
                this.ME2Indicators[13].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[13].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[13].IndMathResult += sa.MathResult;
            }
            else if (index == 14)
            {
                this.ME2Indicators[14].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[14].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[14].IndMathResult += sa.MathResult;
            }
            else if (index == 15)
            {
                this.ME2Indicators[15].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[15].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[15].IndMathResult += sa.MathResult;
            }
            else if (index == 16)
            {
                this.ME2Indicators[16].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[16].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[16].IndMathResult += sa.MathResult;
            }
            else if (index == 17)
            {
                this.ME2Indicators[17].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[17].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[17].IndMathResult += sa.MathResult;
            }
            else if (index == 18)
            {
                this.ME2Indicators[18].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[18].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[18].IndMathResult += sa.MathResult;
            }
            else if (index == 19)
            {
                this.ME2Indicators[19].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[19].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[19].IndMathResult += sa.MathResult;
            }
            else if (index == 20)
            {
                this.ME2Indicators[20].IndTAmount = sa.BestEnergy;
                this.ME2Indicators[20].IndTMAmount = sa.BestEnergy;
                SetPRAIndicatorStats(index, colNames, qTs);
                this.ME2Indicators[20].IndMathResult += sa.MathResult;
            }
            else
            {
                //ignore the row
            }
        }
        private DevTreks.Extensions.Algorithms.NeuralNetwork1 InitNN1Algo(int index)
        {
            List<double> qs = GetQsForLabel(index);
            DevTreks.Extensions.Algorithms.NeuralNetwork1 nn
                    = new Algorithms.NeuralNetwork1(qs, this.ME2Indicators[0].IndIterations, this.CalcParameters);
            return nn;
        }
        private void SetNN1AlgoRanges(int index, DevTreks.Extensions.Algorithms.NeuralNetwork1 nn)
        {
            int iIndicator = GetNN1Index(index);
            string[] colNames = new List<string>().ToArray();
            List<double> qTs = new List<double>();
            if (iIndicator == 0)
            {
                this.ME2Indicators[0].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[0].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[0].IndTLAmount = nn.QTL;
                this.ME2Indicators[0].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[0].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[0].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                    //SetTotalRange1();
                }
                //no condition on type of result yet KISS for now
                this.ME2Indicators[0].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 1)
            {
                this.ME2Indicators[1].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[1].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[1].IndTLAmount = nn.QTL;
                this.ME2Indicators[1].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[1].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[1].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                    //SetTotalRange1();
                }
                //no condition on type of result yet KISS for now
                this.ME2Indicators[1].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 2)
            {
                this.ME2Indicators[2].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[2].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[2].IndTLAmount = nn.QTL;
                this.ME2Indicators[2].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[2].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[2].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[2].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 3)
            {
                this.ME2Indicators[3].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[3].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[3].IndTLAmount = nn.QTL;
                this.ME2Indicators[3].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[3].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[3].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[3].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 4)
            {
                this.ME2Indicators[4].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[4].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[4].IndTLAmount = nn.QTL;
                this.ME2Indicators[4].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[4].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[4].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[4].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 5)
            {
                this.ME2Indicators[5].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[5].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[5].IndTLAmount = nn.QTL;
                this.ME2Indicators[5].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[5].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[5].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[5].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 6)
            {
                this.ME2Indicators[6].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[6].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[6].IndTLAmount = nn.QTL;
                this.ME2Indicators[6].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[6].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[6].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[6].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 7)
            {
                this.ME2Indicators[7].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[7].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[7].IndTLAmount = nn.QTL;
                this.ME2Indicators[7].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[7].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[7].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[7].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 8)
            {
                this.ME2Indicators[8].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[8].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[8].IndTLAmount = nn.QTL;
                this.ME2Indicators[8].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[8].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[8].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[8].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 9)
            {
                this.ME2Indicators[9].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[9].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[9].IndTLAmount = nn.QTL;
                this.ME2Indicators[9].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[9].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[9].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[9].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 10)
            {
                this.ME2Indicators[10].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[10].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[10].IndTLAmount = nn.QTL;
                this.ME2Indicators[10].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[10].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[10].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[10].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 11)
            {
                this.ME2Indicators[11].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[11].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[11].IndTLAmount = nn.QTL;
                this.ME2Indicators[11].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[11].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[11].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[11].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 12)
            {
                this.ME2Indicators[12].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[12].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[12].IndTLAmount = nn.QTL;
                this.ME2Indicators[12].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[12].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[12].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[12].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 13)
            {
                this.ME2Indicators[13].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[13].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[13].IndTLAmount = nn.QTL;
                this.ME2Indicators[13].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[13].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[13].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[13].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 14)
            {
                this.ME2Indicators[14].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[14].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[14].IndTLAmount = nn.QTL;
                this.ME2Indicators[14].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[14].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[14].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[14].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 15)
            {
                this.ME2Indicators[15].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[15].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[15].IndTLAmount = nn.QTL;
                this.ME2Indicators[15].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[15].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[15].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[15].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 16)
            {
                this.ME2Indicators[16].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[16].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[16].IndTLAmount = nn.QTL;
                this.ME2Indicators[16].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[16].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[16].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[16].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 17)
            {
                this.ME2Indicators[17].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[17].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[17].IndTLAmount = nn.QTL;
                this.ME2Indicators[17].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[17].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[17].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[17].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 18)
            {
                this.ME2Indicators[18].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[18].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[18].IndTLAmount = nn.QTL;
                this.ME2Indicators[18].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[18].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[18].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[18].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 19)
            {
                this.ME2Indicators[19].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[19].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[19].IndTLAmount = nn.QTL;
                this.ME2Indicators[19].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[19].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[19].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[19].IndMathResult += nn.MathResult;
            }
            else if (iIndicator == 20)
            {
                this.ME2Indicators[20].IndTAmount = nn.QTPredicted;
                this.ME2Indicators[20].IndTMAmount = nn.QTPredicted;
                this.ME2Indicators[20].IndTLAmount = nn.QTL;
                this.ME2Indicators[20].IndTLUnit = nn.QTLUnit;
                if (this.ME2Indicators[20].IndType != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.ME2Indicators[20].IndType))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(index, colNames, qTs);
                }
                this.ME2Indicators[20].IndMathResult += nn.MathResult;
            }
            else
            {
                //ignore the row
            }
        }
        private int GetNN1Index(int index)
        {
            int iLastIndicatorOneBased = 0;
            if (index == 0)
            {
                iLastIndicatorOneBased = 0;
            }
            if (index == 1)
            {
                iLastIndicatorOneBased = 1;
            }
            if (index == 2)
            {
                iLastIndicatorOneBased = 2;
            }
            if (index == 3)
            {
                iLastIndicatorOneBased = 3;
            }
            if (index == 4)
            {
                iLastIndicatorOneBased = 4;
            }
            if (index == 5)
            {
                iLastIndicatorOneBased = 5;
            }
            if (index == 6)
            {
                iLastIndicatorOneBased = 6;
            }
            if (index == 7)
            {
                iLastIndicatorOneBased = 7;
            }
            if (index == 8)
            {
                iLastIndicatorOneBased = 8;
            }
            if (index == 9)
            {
                iLastIndicatorOneBased = 9;
            }
            if (index == 10)
            {
                iLastIndicatorOneBased = 10;
            }
            if (index == 11)
            {
                iLastIndicatorOneBased = 11;
            }
            if (index == 12)
            {
                iLastIndicatorOneBased = 12;
            }
            if (index == 13)
            {
                iLastIndicatorOneBased = 13;
            }
            if (index == 14)
            {
                iLastIndicatorOneBased = 14;
            }
            if (index == 15)
            {
                iLastIndicatorOneBased = 15;
            }
            if (index == 16)
            {
                iLastIndicatorOneBased = 16;
            }
            if (index == 17)
            {
                iLastIndicatorOneBased = 17;
            }
            if (index == 18)
            {
                iLastIndicatorOneBased = 18;
            }
            if (index == 19)
            {
                iLastIndicatorOneBased = 19;
            }
            if (index == 20)
            {
                iLastIndicatorOneBased = 20;
            }
            return iLastIndicatorOneBased;
        }
        
    }
}
