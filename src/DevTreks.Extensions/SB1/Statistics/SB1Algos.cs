using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Errors = DevTreks.Exceptions.DevTreksErrors;

using MathNet.Numerics.LinearAlgebra;

namespace DevTreks.Extensions.SB1Statistics
{
    ///<summary>
    ///Purpose:		Run algorithms
    ///Author:		www.devtreks.org
    ///Date:		2017, April
    ///NOTES        1. 
    /// </summary> 
    public class SB1Algos : SB1Base
    {
        public SB1Algos()
            : base()
        {
            //class is initialized from sb1base
        }
        //copy constructor
        public SB1Algos(SB1Base stockSR)
            : base(stockSR)
        {
            //stock fact
            CopySB1BaseProperties(stockSR);
        }
        
        public async Task<string> SetAlgoPRAStats(string label, string[] colNames,
            List<double> qTs, double[] data = null)
        {
            //probabilistic risk using montecarlo
            //if the algo is used with the label, return it as affirmation
            string algoindicator = string.Empty;
            if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm1)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm2)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm3)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm4))
            {
                //if its a good calc returns the string
                algoindicator = await SetPRAIndicatorStats(label, colNames, qTs, data);
            }
            return algoindicator;
        }
        
        public async Task<string> SetAlgoCorrIndicatorStats(string jdataURL, IDictionary<string, List<List<double>>> data,
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
        public async Task<string> SetAlgoIndicatorStats1(string label, List<List<double>> data, string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            string algoindicator = string.Empty;
            
            if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6))
            {
                //if its a good calc returns the string
                algoindicator = await SetRGRIndicatorStats(label, colNames, data);
            }
            else if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
            {
                if (colNames.Contains("treatment") || colNames.Contains("factor1"))
                {
                    //random block or random factorial
                    algoindicator = await SetANVIndicatorStats(label, colNames, data);
                }
                else
                {
                    //one way anova can use regression
                    algoindicator = await SetRGRIndicatorStats(label, colNames, data);
                }
            }
            return algoindicator;
        }
        public async Task<string> SetAlgoIndicatorStats2(string label, string[] colNames, 
            string dataURL, string jDataURL)
        {
            //if the algo is used with the label, return it as affirmation
            string algoindicator = string.Empty;
            //assume additional algos will use this data format
            string sPlatForm = CalculatorHelpers.GetPlatform(this.CalcParameters.ExtensionDocToCalcURI, dataURL);
            if (sPlatForm == CalculatorHelpers.PLATFORM_TYPES.azure.ToString())
            {
                if (this.HasMathType(label, MATH_TYPES.algorithm4))
                {
                    //if its a good calc returns the string
                    algoindicator = await SetScriptCloudStats(label, colNames, dataURL, jDataURL);
                }
            }
            else 
            {
                if (this.HasMathType(label, MATH_TYPES.algorithm2)
                    || this.HasMathType(label, MATH_TYPES.algorithm3))
                {
                    //if its a good calc returns the string
                    algoindicator = await SetScriptWebStats(label, colNames, dataURL, jDataURL);
                }
                else if (this.HasMathType(label, MATH_TYPES.algorithm4))
                {
                    //always runs the cloud web servive (but response can vary)
                    algoindicator = await SetScriptCloudStats(label, colNames, dataURL, jDataURL);
                }
            }
            return algoindicator;
        }
        public async Task<string> SetAlgoIndicatorStats3(string label, List<List<string>> data,
            List<List<string>> colData, List<string> lines2, string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            string algoindicator = string.Empty;
            //assume additional algos will use this data format
            if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12))
            {
                //if its a good exceedance probability calc returns the string
                algoindicator = await SetDRR1IndicatorStats(label, colNames, data, 
                    colData, lines2);
            }
            return algoindicator;
        }
        public async Task<string> SetAlgoIndicatorStats4(string label, List<List<string>> data,
            List<List<string>> colData, List<string> lines2, string[] colNames)
        {
            //if the algo is used with the label, return it as affirmation
            string algoindicator = string.Empty;
            //assume additional algos will use this data format
            if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm9)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm10)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm11)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm12)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm13)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm14)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm15)
                || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm16))
            {
                //if its a good exceedance probability calc returns the string
                algoindicator = await SetDRR2IndicatorStats(label, colNames, data,
                    colData, lines2);
            }
            return algoindicator;
        }
        public async Task<string> SetAlgoIndicatorCalcs(string label, List<List<double>> data)
        {
            //if the algo is used with the label, return it as affirmation
            string algoindicator = string.Empty;
            //init the algo using the new indicator
            if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm5))
            {
                DevTreks.Extensions.Algorithms.SimulatedAnnealing1 sa = InitSA1Algo(label);
                //if its a good calc returns the string
                bool bHasCalculation = await sa.RunAlgorithmAsync(data);
                if (bHasCalculation)
                {
                    algoindicator = label;
                }
                //set the results
                SetSA1AlgoRanges(label, sa);
                //start with any error messages
                this.SB1ScoreMathResult += sa.ErrorMessage;
            }
            else if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm7))
            {
                DevTreks.Extensions.Algorithms.NeuralNetwork1 nn = InitNN1Algo(label);
                //run the simulation
                bool bHasCalculation = await nn.RunAlgorithmAsync(data);
                if (bHasCalculation)
                {
                    algoindicator = label;
                }
                //set the results
                SetNN1AlgoRanges(label, nn);
                //start with any error messages
                this.SB1ScoreMathResult += nn.ErrorMessage;
            }
            return algoindicator;
        }
        
        
        private async Task<string> SetPRAIndicatorStats(string label, string[] colNames,
            List<double> qTs, double[] data = null)
        {
            string algoIndicator = string.Empty;
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            if (label == _score
                && HasMathExpression(this.SB1ScoreMathExpression))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(label);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1ScoreMathSubType, colNames, qt1, this.SB1CILevel,
                        this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                //label has to be _score
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label1
                && HasMathExpression(this.SB1MathExpression1))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label1);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType1, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label2
                && HasMathExpression(this.SB1MathExpression2))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label2);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType2, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label3
                && HasMathExpression(this.SB1MathExpression3))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label3);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType3, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label4
                && HasMathExpression(this.SB1MathExpression4))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label4);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType4, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label5
                && HasMathExpression(this.SB1MathExpression5))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label5);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType5, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label6
                && HasMathExpression(this.SB1MathExpression6))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label6);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType6, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label7
                && HasMathExpression(this.SB1MathExpression7))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label7);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType7, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label8
                && HasMathExpression(this.SB1MathExpression8))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label8);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType8, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label9
                && HasMathExpression(this.SB1MathExpression9))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label9);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType9, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label10
                && HasMathExpression(this.SB1MathExpression10))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label10);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType10, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label11
                && HasMathExpression(this.SB1MathExpression11))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label11);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType11, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label12
                && HasMathExpression(this.SB1MathExpression12))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label12);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType12, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label13
                && HasMathExpression(this.SB1MathExpression13))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label13);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType13, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label14
                && HasMathExpression(this.SB1MathExpression14))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label14);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType14, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label15
                && HasMathExpression(this.SB1MathExpression15))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label15);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType15, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label16
                && HasMathExpression(this.SB1MathExpression16))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label16);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType16, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label17
                && HasMathExpression(this.SB1MathExpression17))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label17);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType17, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label18
                && HasMathExpression(this.SB1MathExpression18))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label18);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType18, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label19
                && HasMathExpression(this.SB1MathExpression19))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label19);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType19, colNames, qt1, this.SB1CILevel,
                    this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else if (label == this.SB1Label20
                && HasMathExpression(this.SB1MathExpression20))
            {
                algoIndicator = label;
                IndicatorQT1 qt1 = FillIndicator(this.SB1Label20);
                DevTreks.Extensions.Algorithms.PRA1 pra
                    = InitPRA1Algo(label, this.SB1MathSubType20, colNames, qt1, this.SB1CILevel,
                   this.SB1Iterations, this.SB1Random, qTs);
                if (data == null)
                {
                    await pra.RunAlgorithmAsync();
                }
                else
                {
                    await pra.RunAlgorithmAsync(data);
                }
                FillBaseIndicator(pra.IndicatorQT, pra.IndicatorQT.Label, sLowerCI, sUpperCI);
            }
            else
            {
                //ignore the row
            }
            
            return algoIndicator;
        }
        private async Task<string> SetPRACorrIndicatorStats(string jdataURL, string[] colNames, 
            IDictionary<string, List<List<double>>> data)
        {
            string algoIndicator = string.Empty;
            //do not use the condition if (data.Count > 0) because correlated inds can use random samples
            
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            IndicatorQT1 qt = FillIndicators();
            DevTreks.Extensions.Algorithms.PRA2 pra
                = InitPRA2Algo(qt, this.SB1CILevel, this.SB1Iterations, this.SB1Random);
            algoIndicator = await pra.RunAlgorithmAsync(jdataURL, data);
            FillBaseIndicators(pra.IndicatorQT, sLowerCI, sUpperCI);
            //version 1.9.0 had to move this out of PRA2 (can't access SB1Base from there)
            if (!string.IsNullOrEmpty(algoIndicator))
            {
                string[] algoIndicators = algoIndicator.Split(Constants.CSV_DELIMITERS);
                string[] indicators = new string[] { };
                //use the randomsample data to generate Score, ScoreM, ScoreL, and ScoreU
                indicators = SetScoresFromRandomSamples(algoIndicators, pra.RandomSampleData, colNames);
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
                    && HasMathExpression(this.SB1ScoreMathExpression))
                {
                    IndicatorQT1 qt1 = FillIndicator(_score); 
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 1
                    && HasMathExpression(this.SB1MathExpression1))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label1); 
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 2
                    && HasMathExpression(this.SB1MathExpression2))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label2);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 3
                    && HasMathExpression(this.SB1MathExpression3))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label3);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 4
                    && HasMathExpression(this.SB1MathExpression4))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label4);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 5
                    && HasMathExpression(this.SB1MathExpression5))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label5);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 6
                    && HasMathExpression(this.SB1MathExpression6))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label6);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 7
                    && HasMathExpression(this.SB1MathExpression7))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label7);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 8
                    && HasMathExpression(this.SB1MathExpression8))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label8);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 9
                    && HasMathExpression(this.SB1MathExpression9))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label9);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 10
                    && HasMathExpression(this.SB1MathExpression10))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label10);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 11
                    && HasMathExpression(this.SB1MathExpression11))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label11);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 12
                    && HasMathExpression(this.SB1MathExpression12))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label12);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 13
                    && HasMathExpression(this.SB1MathExpression13))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label13);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 14
                    && HasMathExpression(this.SB1MathExpression14))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label14);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 15
                    && HasMathExpression(this.SB1MathExpression15))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label15);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 16
                    && HasMathExpression(this.SB1MathExpression16))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label16);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 17
                    && HasMathExpression(this.SB1MathExpression17))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label17);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 18
                    && HasMathExpression(this.SB1MathExpression18))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label18);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 19
                    && HasMathExpression(this.SB1MathExpression19))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label19);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else if (i == 20
                    && HasMathExpression(this.SB1MathExpression20))
                {
                    IndicatorQT1 qt1 = FillIndicator(this.SB1Label20);
                    qt.IndicatorQT1s.Add(qt1);
                }
                else
                {
                    //ignore the row
                }
            }
            return qt;
        }
        
        private IndicatorQT1 FillIndicator(string label)
        {
            IndicatorQT1 qt = new IndicatorQT1();
            if (label == _score
                && HasMathExpression(this.SB1ScoreMathExpression))
            {
                qt = new IndicatorQT1(_score, this.SB1ScoreM, this.SB1ScoreLAmount, SB1ScoreUAmount,
                    this.SB1Score, this.SB1ScoreD1Amount, this.SB1ScoreD2Amount, this.SB1ScoreMUnit, this.SB1ScoreLUnit, this.SB1ScoreUUnit,
                    this.SB1ScoreUnit, this.SB1ScoreD1Unit, this.SB1ScoreD2Unit, this.SB1ScoreMathType, this.SB1ScoreMathSubType,
                    this.SB1ScoreDistType, this.SB1ScoreMathExpression, this.SB1ScoreMathResult,
                    0, 0, 0, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            else if (label == this.SB1Label1
                && HasMathExpression(this.SB1MathExpression1))
            {
                qt = new IndicatorQT1(this.SB1Label1, this.SB1TMAmount1, this.SB1TLAmount1, SB1TUAmount1,
                    this.SB1TAmount1, this.SB1TD1Amount1, this.SB1TD2Amount1, this.SB1TMUnit1, this.SB1TLUnit1, this.SB1TUUnit1,
                    this.SB1TUnit1, this.SB1TD1Unit1, this.SB1TD2Unit1, this.SB1MathType1, this.SB1MathSubType1,
                    this.SB1Type1, this.SB1MathExpression1, this.SB1MathResult1,
                    this.SB11Amount1, this.SB12Amount1, this.SB13Amount1, this.SB14Amount1, this.SB15Amount1,
                    this.SB11Unit1, this.SB12Unit1, this.SB13Unit1, this.SB14Unit1, this.SB15Unit1);
            }
            else if (label == this.SB1Label2
                && HasMathExpression(this.SB1MathExpression2))
            {
                qt = new IndicatorQT1(this.SB1Label2, this.SB1TMAmount2, this.SB1TLAmount2, SB1TUAmount2,
                    this.SB1TAmount2, this.SB1TD1Amount2, this.SB1TD2Amount2, this.SB1TMUnit2, this.SB1TLUnit2, this.SB1TUUnit2,
                    this.SB1TUnit2, this.SB1TD1Unit2, this.SB1TD2Unit2, this.SB1MathType2, this.SB1MathSubType2,
                    this.SB1Type2, this.SB1MathExpression2, this.SB1MathResult2,
                    this.SB11Amount2, this.SB12Amount2, this.SB13Amount2, this.SB14Amount2, this.SB15Amount2,
                    this.SB11Unit2, this.SB12Unit2, this.SB13Unit2, this.SB14Unit2, this.SB15Unit2);
            }
            else if (label == this.SB1Label3
                && HasMathExpression(this.SB1MathExpression3))
            {
                qt = new IndicatorQT1(this.SB1Label3, this.SB1TMAmount3, this.SB1TLAmount3, SB1TUAmount3,
                    this.SB1TAmount3, this.SB1TD1Amount3, this.SB1TD2Amount3, this.SB1TMUnit3, this.SB1TLUnit3, this.SB1TUUnit3,
                    this.SB1TUnit3, this.SB1TD1Unit3, this.SB1TD2Unit3, this.SB1MathType3, this.SB1MathSubType3,
                    this.SB1Type3, this.SB1MathExpression3, this.SB1MathResult3,
                    this.SB11Amount3, this.SB12Amount3, this.SB13Amount3, this.SB14Amount3, this.SB15Amount3,
                    this.SB11Unit3, this.SB12Unit3, this.SB13Unit3, this.SB14Unit3, this.SB15Unit3);
            }
            else if (label == this.SB1Label4
                && HasMathExpression(this.SB1MathExpression4))
            {
                qt = new IndicatorQT1(this.SB1Label4, this.SB1TMAmount4, this.SB1TLAmount4, SB1TUAmount4,
                    this.SB1TAmount4, this.SB1TD1Amount4, this.SB1TD2Amount4, this.SB1TMUnit4, this.SB1TLUnit4, this.SB1TUUnit4,
                    this.SB1TUnit4, this.SB1TD1Unit4, this.SB1TD2Unit4, this.SB1MathType4, this.SB1MathSubType4,
                    this.SB1Type4, this.SB1MathExpression4, this.SB1MathResult4,
                    this.SB11Amount4, this.SB12Amount4, this.SB13Amount4, this.SB14Amount4, this.SB15Amount4,
                    this.SB11Unit4, this.SB12Unit4, this.SB13Unit4, this.SB14Unit4, this.SB15Unit4);
            }
            else if (label == this.SB1Label5
                && HasMathExpression(this.SB1MathExpression5))
            {
                qt = new IndicatorQT1(this.SB1Label5, this.SB1TMAmount5, this.SB1TLAmount5, SB1TUAmount5,
                    this.SB1TAmount5, this.SB1TD1Amount5, this.SB1TD2Amount5, this.SB1TMUnit5, this.SB1TLUnit5, this.SB1TUUnit5,
                    this.SB1TUnit5, this.SB1TD1Unit5, this.SB1TD2Unit5, this.SB1MathType5, this.SB1MathSubType5,
                    this.SB1Type5, this.SB1MathExpression5, this.SB1MathResult5,
                    this.SB11Amount5, this.SB12Amount5, this.SB13Amount5, this.SB14Amount5, this.SB15Amount5,
                    this.SB11Unit5, this.SB12Unit5, this.SB13Unit5, this.SB14Unit5, this.SB15Unit5);
            }
            else if (label == this.SB1Label6
                && HasMathExpression(this.SB1MathExpression6))
            {
                qt = new IndicatorQT1(this.SB1Label6, this.SB1TMAmount6, this.SB1TLAmount6, SB1TUAmount6,
                    this.SB1TAmount6, this.SB1TD1Amount6, this.SB1TD2Amount6, this.SB1TMUnit6, this.SB1TLUnit6, this.SB1TUUnit6,
                    this.SB1TUnit6, this.SB1TD1Unit6, this.SB1TD2Unit6, this.SB1MathType6, this.SB1MathSubType6,
                    this.SB1Type6, this.SB1MathExpression6, this.SB1MathResult6,
                    this.SB11Amount6, this.SB12Amount6, this.SB13Amount6, this.SB14Amount6, this.SB15Amount6,
                    this.SB11Unit6, this.SB12Unit6, this.SB13Unit6, this.SB14Unit6, this.SB15Unit6);
            }
            else if (label == this.SB1Label7
                && HasMathExpression(this.SB1MathExpression7))
            {
                qt = new IndicatorQT1(this.SB1Label7, this.SB1TMAmount7, this.SB1TLAmount7, SB1TUAmount7,
                    this.SB1TAmount7, this.SB1TD1Amount7, this.SB1TD2Amount7, this.SB1TMUnit7, this.SB1TLUnit7, this.SB1TUUnit7,
                    this.SB1TUnit7, this.SB1TD1Unit7, this.SB1TD2Unit7, this.SB1MathType7, this.SB1MathSubType7,
                    this.SB1Type7, this.SB1MathExpression7, this.SB1MathResult7,
                    this.SB11Amount7, this.SB12Amount7, this.SB13Amount7, this.SB14Amount7, this.SB15Amount7,
                    this.SB11Unit7, this.SB12Unit7, this.SB13Unit7, this.SB14Unit7, this.SB15Unit7);
            }
            else if (label == this.SB1Label8
                && HasMathExpression(this.SB1MathExpression8))
            {
                qt = new IndicatorQT1(this.SB1Label8, this.SB1TMAmount8, this.SB1TLAmount8, SB1TUAmount8,
                    this.SB1TAmount8, this.SB1TD1Amount8, this.SB1TD2Amount8, this.SB1TMUnit8, this.SB1TLUnit8, this.SB1TUUnit8,
                    this.SB1TUnit8, this.SB1TD1Unit8, this.SB1TD2Unit8, this.SB1MathType8, this.SB1MathSubType8,
                    this.SB1Type8, this.SB1MathExpression8, this.SB1MathResult8,
                    this.SB11Amount8, this.SB12Amount8, this.SB13Amount8, this.SB14Amount8, this.SB15Amount8,
                    this.SB11Unit8, this.SB12Unit8, this.SB13Unit8, this.SB14Unit8, this.SB15Unit8);
            }
            else if (label == this.SB1Label9
                && HasMathExpression(this.SB1MathExpression9))
            {
                qt = new IndicatorQT1(this.SB1Label9, this.SB1TMAmount9, this.SB1TLAmount9, SB1TUAmount9,
                    this.SB1TAmount9, this.SB1TD1Amount9, this.SB1TD2Amount9, this.SB1TMUnit9, this.SB1TLUnit9, this.SB1TUUnit9,
                    this.SB1TUnit9, this.SB1TD1Unit9, this.SB1TD2Unit9, this.SB1MathType9, this.SB1MathSubType9,
                    this.SB1Type9, this.SB1MathExpression9, this.SB1MathResult9,
                    this.SB11Amount9, this.SB12Amount9, this.SB13Amount9, this.SB14Amount9, this.SB15Amount9,
                    this.SB11Unit9, this.SB12Unit9, this.SB13Unit9, this.SB14Unit9, this.SB15Unit9);
            }
            else if (label == this.SB1Label10
                && HasMathExpression(this.SB1MathExpression10))
            {
                qt = new IndicatorQT1(this.SB1Label10, this.SB1TMAmount10, this.SB1TLAmount10, SB1TUAmount10,
                    this.SB1TAmount10, this.SB1TD1Amount10, this.SB1TD2Amount10, this.SB1TMUnit10, this.SB1TLUnit10, this.SB1TUUnit10,
                    this.SB1TUnit10, this.SB1TD1Unit10, this.SB1TD2Unit10, this.SB1MathType10, this.SB1MathSubType10,
                    this.SB1Type10, this.SB1MathExpression10, this.SB1MathResult10,
                    this.SB11Amount10, this.SB12Amount10, this.SB13Amount10, this.SB14Amount10, this.SB15Amount10,
                    this.SB11Unit10, this.SB12Unit10, this.SB13Unit10, this.SB14Unit10, this.SB15Unit10);
            }
            else if (label == this.SB1Label11
                && HasMathExpression(this.SB1MathExpression11))
            {
                qt = new IndicatorQT1(this.SB1Label11, this.SB1TMAmount11, this.SB1TLAmount11, SB1TUAmount11,
                    this.SB1TAmount11, this.SB1TD1Amount11, this.SB1TD2Amount11, this.SB1TMUnit11, this.SB1TLUnit11, this.SB1TUUnit11,
                    this.SB1TUnit11, this.SB1TD1Unit11, this.SB1TD2Unit11, this.SB1MathType11, this.SB1MathSubType11,
                    this.SB1Type11, this.SB1MathExpression11, this.SB1MathResult11,
                    this.SB11Amount11, this.SB12Amount11, this.SB13Amount11, this.SB14Amount11, this.SB15Amount11,
                    this.SB11Unit11, this.SB12Unit11, this.SB13Unit11, this.SB14Unit11, this.SB15Unit11);
            }
            else if (label == this.SB1Label12
                && HasMathExpression(this.SB1MathExpression12))
            {
                qt = new IndicatorQT1(this.SB1Label12, this.SB1TMAmount12, this.SB1TLAmount12, SB1TUAmount12,
                    this.SB1TAmount12, this.SB1TD1Amount12, this.SB1TD2Amount12, this.SB1TMUnit12, this.SB1TLUnit12, this.SB1TUUnit12,
                    this.SB1TUnit12, this.SB1TD1Unit12, this.SB1TD2Unit12, this.SB1MathType12, this.SB1MathSubType12,
                    this.SB1Type12, this.SB1MathExpression12, this.SB1MathResult12,
                    this.SB11Amount12, this.SB12Amount12, this.SB13Amount12, this.SB14Amount12, this.SB15Amount12,
                    this.SB11Unit12, this.SB12Unit12, this.SB13Unit12, this.SB14Unit12, this.SB15Unit12);
            }
            else if (label == this.SB1Label13
                && HasMathExpression(this.SB1MathExpression13))
            {
                qt = new IndicatorQT1(this.SB1Label13, this.SB1TMAmount13, this.SB1TLAmount13, SB1TUAmount13,
                    this.SB1TAmount13, this.SB1TD1Amount13, this.SB1TD2Amount13, this.SB1TMUnit13, this.SB1TLUnit13, this.SB1TUUnit13,
                    this.SB1TUnit13, this.SB1TD1Unit13, this.SB1TD2Unit13, this.SB1MathType13, this.SB1MathSubType13,
                    this.SB1Type13, this.SB1MathExpression13, this.SB1MathResult13,
                    this.SB11Amount13, this.SB12Amount13, this.SB13Amount13, this.SB14Amount13, this.SB15Amount13,
                    this.SB11Unit13, this.SB12Unit13, this.SB13Unit13, this.SB14Unit13, this.SB15Unit13);
            }
            else if (label == this.SB1Label14
                && HasMathExpression(this.SB1MathExpression14))
            {
                qt = new IndicatorQT1(this.SB1Label14, this.SB1TMAmount14, this.SB1TLAmount14, SB1TUAmount14,
                    this.SB1TAmount14, this.SB1TD1Amount14, this.SB1TD2Amount14, this.SB1TMUnit14, this.SB1TLUnit14, this.SB1TUUnit14,
                    this.SB1TUnit14, this.SB1TD1Unit14, this.SB1TD2Unit14, this.SB1MathType14, this.SB1MathSubType14,
                    this.SB1Type14, this.SB1MathExpression14, this.SB1MathResult14,
                    this.SB11Amount14, this.SB12Amount14, this.SB13Amount14, this.SB14Amount14, this.SB15Amount14,
                    this.SB11Unit14, this.SB12Unit14, this.SB13Unit14, this.SB14Unit14, this.SB15Unit14);
            }
            else if (label == this.SB1Label15
                && HasMathExpression(this.SB1MathExpression15))
            {
                qt = new IndicatorQT1(this.SB1Label15, this.SB1TMAmount15, this.SB1TLAmount15, SB1TUAmount15,
                    this.SB1TAmount15, this.SB1TD1Amount15, this.SB1TD2Amount15, this.SB1TMUnit15, this.SB1TLUnit15, this.SB1TUUnit15,
                    this.SB1TUnit15, this.SB1TD1Unit15, this.SB1TD2Unit15, this.SB1MathType15, this.SB1MathSubType15,
                    this.SB1Type15, this.SB1MathExpression15, this.SB1MathResult15,
                    this.SB11Amount15, this.SB12Amount15, this.SB13Amount15, this.SB14Amount15, this.SB15Amount15,
                    this.SB11Unit15, this.SB12Unit15, this.SB13Unit15, this.SB14Unit15, this.SB15Unit15);
            }
            else if (label == this.SB1Label16
                && HasMathExpression(this.SB1MathExpression16))
            {
                qt = new IndicatorQT1(this.SB1Label16, this.SB1TMAmount16, this.SB1TLAmount16, SB1TUAmount16,
                    this.SB1TAmount16, this.SB1TD1Amount16, this.SB1TD2Amount16, this.SB1TMUnit16, this.SB1TLUnit16, this.SB1TUUnit16,
                    this.SB1TUnit16, this.SB1TD1Unit16, this.SB1TD2Unit16, this.SB1MathType16, this.SB1MathSubType16,
                    this.SB1Type16, this.SB1MathExpression16, this.SB1MathResult16,
                    this.SB11Amount16, this.SB12Amount16, this.SB13Amount16, this.SB14Amount16, this.SB15Amount16,
                    this.SB11Unit16, this.SB12Unit16, this.SB13Unit16, this.SB14Unit16, this.SB15Unit16);
            }
            else if (label == this.SB1Label17
                && HasMathExpression(this.SB1MathExpression17))
            {
                qt = new IndicatorQT1(this.SB1Label17, this.SB1TMAmount17, this.SB1TLAmount17, SB1TUAmount17,
                    this.SB1TAmount17, this.SB1TD1Amount17, this.SB1TD2Amount17, this.SB1TMUnit17, this.SB1TLUnit17, this.SB1TUUnit17,
                    this.SB1TUnit17, this.SB1TD1Unit17, this.SB1TD2Unit17, this.SB1MathType17, this.SB1MathSubType17,
                    this.SB1Type17, this.SB1MathExpression17, this.SB1MathResult17,
                    this.SB11Amount17, this.SB12Amount17, this.SB13Amount17, this.SB14Amount17, this.SB15Amount17,
                    this.SB11Unit17, this.SB12Unit17, this.SB13Unit17, this.SB14Unit17, this.SB15Unit17);
            }
            else if (label == this.SB1Label18
                && HasMathExpression(this.SB1MathExpression18))
            {
                qt = new IndicatorQT1(this.SB1Label18, this.SB1TMAmount18, this.SB1TLAmount18, SB1TUAmount18,
                    this.SB1TAmount18, this.SB1TD1Amount18, this.SB1TD2Amount18, this.SB1TMUnit18, this.SB1TLUnit18, this.SB1TUUnit18,
                    this.SB1TUnit18, this.SB1TD1Unit18, this.SB1TD2Unit18, this.SB1MathType18, this.SB1MathSubType18,
                    this.SB1Type18, this.SB1MathExpression18, this.SB1MathResult18,
                    this.SB11Amount18, this.SB12Amount18, this.SB13Amount18, this.SB14Amount18, this.SB15Amount18,
                    this.SB11Unit18, this.SB12Unit18, this.SB13Unit18, this.SB14Unit18, this.SB15Unit18);
            }
            else if (label == this.SB1Label19
                && HasMathExpression(this.SB1MathExpression19))
            {
                qt = new IndicatorQT1(this.SB1Label19, this.SB1TMAmount19, this.SB1TLAmount19, SB1TUAmount19,
                    this.SB1TAmount19, this.SB1TD1Amount19, this.SB1TD2Amount19, this.SB1TMUnit19, this.SB1TLUnit19, this.SB1TUUnit19,
                    this.SB1TUnit19, this.SB1TD1Unit19, this.SB1TD2Unit19, this.SB1MathType19, this.SB1MathSubType19,
                    this.SB1Type19, this.SB1MathExpression19, this.SB1MathResult19,
                    this.SB11Amount19, this.SB12Amount19, this.SB13Amount19, this.SB14Amount19, this.SB15Amount19,
                    this.SB11Unit19, this.SB12Unit19, this.SB13Unit19, this.SB14Unit19, this.SB15Unit19);
            }
            else if (label == this.SB1Label20
                && HasMathExpression(this.SB1MathExpression20))
            {
                qt = new IndicatorQT1(this.SB1Label20, this.SB1TMAmount20, this.SB1TLAmount20, SB1TUAmount20,
                    this.SB1TAmount20, this.SB1TD1Amount20, this.SB1TD2Amount20, this.SB1TMUnit20, this.SB1TLUnit20, this.SB1TUUnit20,
                    this.SB1TUnit20, this.SB1TD1Unit20, this.SB1TD2Unit20, this.SB1MathType20, this.SB1MathSubType20,
                    this.SB1Type20, this.SB1MathExpression20, this.SB1MathResult20,
                    this.SB11Amount20, this.SB12Amount20, this.SB13Amount20, this.SB14Amount20, this.SB15Amount20,
                    this.SB11Unit20, this.SB12Unit20, this.SB13Unit20, this.SB14Unit20, this.SB15Unit20);
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
                    && HasMathExpression(this.SB1ScoreMathExpression))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], _score, lowerci, upperci);
                }
                else if (i == 1
                    && HasMathExpression(this.SB1MathExpression1))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 2
                    && HasMathExpression(this.SB1MathExpression2))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 3
                    && HasMathExpression(this.SB1MathExpression3))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 4
                    && HasMathExpression(this.SB1MathExpression4))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 5
                    && HasMathExpression(this.SB1MathExpression5))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 6
                    && HasMathExpression(this.SB1MathExpression6))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 7
                    && HasMathExpression(this.SB1MathExpression7))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 8
                    && HasMathExpression(this.SB1MathExpression8))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 9
                    && HasMathExpression(this.SB1MathExpression9))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 10
                    && HasMathExpression(this.SB1MathExpression10))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 11
                    && HasMathExpression(this.SB1MathExpression11))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 12
                    && HasMathExpression(this.SB1MathExpression12))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 13
                    && HasMathExpression(this.SB1MathExpression13))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 14
                    && HasMathExpression(this.SB1MathExpression14))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 15
                    && HasMathExpression(this.SB1MathExpression15))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 16
                    && HasMathExpression(this.SB1MathExpression16))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 17
                    && HasMathExpression(this.SB1MathExpression17))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 18
                    && HasMathExpression(this.SB1MathExpression18))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 19
                    && HasMathExpression(this.SB1MathExpression19))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else if (i == 20
                    && HasMathExpression(this.SB1MathExpression20))
                {
                    FillBaseIndicator(qt1.IndicatorQT1s[i], qt1.IndicatorQT1s[i].Label, lowerci, upperci);
                }
                else
                {
                    //ignore the row
                }
            }
        }
        private void FillBaseIndicator(IndicatorQT1 qt1, string label, string lowerci, string upperci)
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
            if (label == _score
                && HasMathExpression(this.SB1ScoreMathExpression))
            {
                this.SB1ScoreM = qt1.QTM;
                this.SB1ScoreMUnit = qt1.QTMUnit;
                this.SB1ScoreLAmount = qt1.QTL;
                this.SB1ScoreLUnit = lowerci;
                this.SB1ScoreUAmount = qt1.QTU;
                this.SB1ScoreUUnit = upperci;
                this.SB1ScoreMathResult = qt1.ErrorMessage;
                this.SB1ScoreMathResult += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1Score = qt1.QT;
                    this.SB1ScoreUnit = qt1.QTUnit;
                    this.SB1ScoreD1Amount = qt1.QTD1;
                    this.SB1ScoreD1Unit = qt1.QTD1Unit;
                    this.SB1ScoreD2Amount = qt1.QTD2;
                    this.SB1ScoreD2Unit = qt1.QTD2Unit;
                }
            }
            else if (label == this.SB1Label1
                && HasMathExpression(this.SB1MathExpression1))
            {
                this.SB1TMAmount1 = qt1.QTM;
                this.SB1TMUnit1 = qt1.QTMUnit;
                this.SB1TUnit1 = qt1.QTUnit;
                this.SB1TLAmount1 = qt1.QTL;
                this.SB1TLUnit1 = lowerci;
                this.SB1TUAmount1 = qt1.QTU;
                this.SB1TUUnit1 = upperci;
                this.SB1MathResult1 = qt1.ErrorMessage;
                this.SB1MathResult1 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount1 = qt1.QT;
                    this.SB1TD1Amount1 = qt1.QTD1;
                    this.SB1TD1Unit1 = qt1.QTD1Unit;
                    this.SB1TD2Amount1 = qt1.QTD2;
                    this.SB1TD2Unit1 = qt1.QTD2Unit;
                }
                this.SB11Amount1 = qt1.Q1;
                this.SB11Unit1 = qt1.Q1Unit;
                this.SB12Amount1 = qt1.Q2;
                this.SB12Unit1 = qt1.Q2Unit;
                this.SB13Amount1 = qt1.Q3;
                this.SB13Unit1 = qt1.Q3Unit;
                this.SB14Amount1 = qt1.Q4;
                this.SB14Unit1 = qt1.Q4Unit;
                this.SB15Amount1 = qt1.Q5;
                this.SB15Unit1 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label2
                && HasMathExpression(this.SB1MathExpression2))
            {
                this.SB1TMAmount2 = qt1.QTM;
                this.SB1TMUnit2 = qt1.QTMUnit;
                this.SB1TUnit2 = qt1.QTUnit;
                this.SB1TLAmount2 = qt1.QTL;
                this.SB1TLUnit2 = lowerci;
                this.SB1TUAmount2 = qt1.QTU;
                this.SB1TUUnit2 = upperci;
                this.SB1MathResult2 = qt1.ErrorMessage;
                this.SB1MathResult2 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount2 = qt1.QT;
                    this.SB1TD1Amount2 = qt1.QTD1;
                    this.SB1TD1Unit2 = qt1.QTD1Unit;
                    this.SB1TD2Amount2 = qt1.QTD2;
                    this.SB1TD2Unit2 = qt1.QTD2Unit;
                }
                this.SB11Amount2 = qt1.Q1;
                this.SB11Unit2 = qt1.Q1Unit;
                this.SB12Amount2 = qt1.Q2;
                this.SB12Unit2 = qt1.Q2Unit;
                this.SB13Amount2 = qt1.Q3;
                this.SB13Unit2 = qt1.Q3Unit;
                this.SB14Amount2 = qt1.Q4;
                this.SB14Unit2 = qt1.Q4Unit;
                this.SB15Amount2 = qt1.Q5;
                this.SB15Unit2 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label3
                && HasMathExpression(this.SB1MathExpression3))
            {
                this.SB1TMAmount3 = qt1.QTM;
                this.SB1TMUnit3 = qt1.QTMUnit;
                this.SB1TUnit3 = qt1.QTUnit;
                this.SB1TLAmount3 = qt1.QTL;
                this.SB1TLUnit3 = lowerci;
                this.SB1TUAmount3 = qt1.QTU;
                this.SB1TUUnit3 = upperci;
                this.SB1MathResult3 = qt1.ErrorMessage;
                this.SB1MathResult3 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount3 = qt1.QT;
                    this.SB1TD1Amount3 = qt1.QTD1;
                    this.SB1TD1Unit3 = qt1.QTD1Unit;
                    this.SB1TD2Amount3 = qt1.QTD2;
                    this.SB1TD2Unit3 = qt1.QTD2Unit;
                }
                this.SB11Amount3 = qt1.Q1;
                this.SB11Unit3 = qt1.Q1Unit;
                this.SB12Amount3 = qt1.Q2;
                this.SB12Unit3 = qt1.Q2Unit;
                this.SB13Amount3 = qt1.Q3;
                this.SB13Unit3 = qt1.Q3Unit;
                this.SB14Amount3 = qt1.Q4;
                this.SB14Unit3 = qt1.Q4Unit;
                this.SB15Amount3 = qt1.Q5;
                this.SB15Unit3 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label4
                && HasMathExpression(this.SB1MathExpression4))
            {
                this.SB1TMAmount4 = qt1.QTM;
                this.SB1TMUnit4 = qt1.QTMUnit;
                this.SB1TUnit4 = qt1.QTUnit;
                this.SB1TLAmount4 = qt1.QTL;
                this.SB1TLUnit4 = lowerci;
                this.SB1TUAmount4 = qt1.QTU;
                this.SB1TUUnit4 = upperci;
                this.SB1MathResult4 = qt1.ErrorMessage;
                this.SB1MathResult4 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount4 = qt1.QT;
                    this.SB1TD1Amount4 = qt1.QTD1;
                    this.SB1TD1Unit4 = qt1.QTD1Unit;
                    this.SB1TD2Amount4 = qt1.QTD2;
                    this.SB1TD2Unit4 = qt1.QTD2Unit;
                }
                this.SB11Amount4 = qt1.Q1;
                this.SB11Unit4 = qt1.Q1Unit;
                this.SB12Amount4 = qt1.Q2;
                this.SB12Unit4 = qt1.Q2Unit;
                this.SB13Amount4 = qt1.Q3;
                this.SB13Unit4 = qt1.Q3Unit;
                this.SB14Amount4 = qt1.Q4;
                this.SB14Unit4 = qt1.Q4Unit;
                this.SB15Amount4 = qt1.Q5;
                this.SB15Unit4 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label5
                && HasMathExpression(this.SB1MathExpression5))
            {
                this.SB1TMAmount5 = qt1.QTM;
                this.SB1TMUnit5 = qt1.QTMUnit;
                this.SB1TUnit5 = qt1.QTUnit;
                this.SB1TLAmount5 = qt1.QTL;
                this.SB1TLUnit5 = lowerci;
                this.SB1TUAmount5 = qt1.QTU;
                this.SB1TUUnit5 = upperci;
                this.SB1MathResult5 = qt1.ErrorMessage;
                this.SB1MathResult5 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount5 = qt1.QT;
                    this.SB1TD1Amount5 = qt1.QTD1;
                    this.SB1TD1Unit5 = qt1.QTD1Unit;
                    this.SB1TD2Amount5 = qt1.QTD2;
                    this.SB1TD2Unit5 = qt1.QTD2Unit;
                }
                this.SB11Amount5 = qt1.Q1;
                this.SB11Unit5 = qt1.Q1Unit;
                this.SB12Amount5 = qt1.Q2;
                this.SB12Unit5 = qt1.Q2Unit;
                this.SB13Amount5 = qt1.Q3;
                this.SB13Unit5 = qt1.Q3Unit;
                this.SB14Amount5 = qt1.Q4;
                this.SB14Unit5 = qt1.Q4Unit;
                this.SB15Amount5 = qt1.Q5;
                this.SB15Unit5 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label6
                && HasMathExpression(this.SB1MathExpression6))
            {
                this.SB1TMAmount6 = qt1.QTM;
                this.SB1TMUnit6 = qt1.QTMUnit;
                this.SB1TUnit6 = qt1.QTUnit;
                this.SB1TLAmount6 = qt1.QTL;
                this.SB1TLUnit6 = lowerci;
                this.SB1TUAmount6 = qt1.QTU;
                this.SB1TUUnit6 = upperci;
                this.SB1MathResult6 = qt1.ErrorMessage;
                this.SB1MathResult6 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount6 = qt1.QT;
                    this.SB1TD1Amount6 = qt1.QTD1;
                    this.SB1TD1Unit6 = qt1.QTD1Unit;
                    this.SB1TD2Amount6 = qt1.QTD2;
                    this.SB1TD2Unit6 = qt1.QTD2Unit;
                }
                this.SB11Amount6 = qt1.Q1;
                this.SB11Unit6 = qt1.Q1Unit;
                this.SB12Amount6 = qt1.Q2;
                this.SB12Unit6 = qt1.Q2Unit;
                this.SB13Amount6 = qt1.Q3;
                this.SB13Unit6 = qt1.Q3Unit;
                this.SB14Amount6 = qt1.Q4;
                this.SB14Unit6 = qt1.Q4Unit;
                this.SB15Amount6 = qt1.Q5;
                this.SB15Unit6 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label7
                && HasMathExpression(this.SB1MathExpression7))
            {
                this.SB1TMAmount7 = qt1.QTM;
                this.SB1TMUnit7 = qt1.QTMUnit;
                this.SB1TUnit7 = qt1.QTUnit;
                this.SB1TLAmount7 = qt1.QTL;
                this.SB1TLUnit7 = lowerci;
                this.SB1TUAmount7 = qt1.QTU;
                this.SB1TUUnit7 = upperci;
                this.SB1MathResult7 = qt1.ErrorMessage;
                this.SB1MathResult7 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount7 = qt1.QT;
                    this.SB1TD1Amount7 = qt1.QTD1;
                    this.SB1TD1Unit7 = qt1.QTD1Unit;
                    this.SB1TD2Amount7 = qt1.QTD2;
                    this.SB1TD2Unit7 = qt1.QTD2Unit;
                }
                this.SB11Amount7 = qt1.Q1;
                this.SB11Unit7 = qt1.Q1Unit;
                this.SB12Amount7 = qt1.Q2;
                this.SB12Unit7 = qt1.Q2Unit;
                this.SB13Amount7 = qt1.Q3;
                this.SB13Unit7 = qt1.Q3Unit;
                this.SB14Amount7 = qt1.Q4;
                this.SB14Unit7 = qt1.Q4Unit;
                this.SB15Amount7 = qt1.Q5;
                this.SB15Unit7 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label8
                && HasMathExpression(this.SB1MathExpression8))
            {
                this.SB1TMAmount8 = qt1.QTM;
                this.SB1TMUnit8 = qt1.QTMUnit;
                this.SB1TUnit8 = qt1.QTUnit;
                this.SB1TLAmount8 = qt1.QTL;
                this.SB1TLUnit8 = lowerci;
                this.SB1TUAmount8 = qt1.QTU;
                this.SB1TUUnit8 = upperci;
                this.SB1MathResult8 = qt1.ErrorMessage;
                this.SB1MathResult8 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount8 = qt1.QT;
                    this.SB1TD1Amount8 = qt1.QTD1;
                    this.SB1TD1Unit8 = qt1.QTD1Unit;
                    this.SB1TD2Amount8 = qt1.QTD2;
                    this.SB1TD2Unit8 = qt1.QTD2Unit;
                }
                this.SB11Amount8 = qt1.Q1;
                this.SB11Unit8 = qt1.Q1Unit;
                this.SB12Amount8 = qt1.Q2;
                this.SB12Unit8 = qt1.Q2Unit;
                this.SB13Amount8 = qt1.Q3;
                this.SB13Unit8 = qt1.Q3Unit;
                this.SB14Amount8 = qt1.Q4;
                this.SB14Unit8 = qt1.Q4Unit;
                this.SB15Amount8 = qt1.Q5;
                this.SB15Unit8 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label9
                && HasMathExpression(this.SB1MathExpression9))
            {
                this.SB1TMAmount9 = qt1.QTM;
                this.SB1TMUnit9 = qt1.QTMUnit;
                this.SB1TUnit9 = qt1.QTUnit;
                this.SB1TLAmount9 = qt1.QTL;
                this.SB1TLUnit9 = lowerci;
                this.SB1TUAmount9 = qt1.QTU;
                this.SB1TUUnit9 = upperci;
                this.SB1MathResult9 = qt1.ErrorMessage;
                this.SB1MathResult9 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount9 = qt1.QT;
                    this.SB1TD1Amount9 = qt1.QTD1;
                    this.SB1TD1Unit9 = qt1.QTD1Unit;
                    this.SB1TD2Amount9 = qt1.QTD2;
                    this.SB1TD2Unit9 = qt1.QTD2Unit;
                }
                this.SB11Amount9 = qt1.Q1;
                this.SB11Unit9 = qt1.Q1Unit;
                this.SB12Amount9 = qt1.Q2;
                this.SB12Unit9 = qt1.Q2Unit;
                this.SB13Amount9 = qt1.Q3;
                this.SB13Unit9 = qt1.Q3Unit;
                this.SB14Amount9 = qt1.Q4;
                this.SB14Unit9 = qt1.Q4Unit;
                this.SB15Amount9 = qt1.Q5;
                this.SB15Unit9 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label10
                && HasMathExpression(this.SB1MathExpression10))
            {
                this.SB1TMAmount10 = qt1.QTM;
                this.SB1TMUnit10 = qt1.QTMUnit;
                this.SB1TUnit10 = qt1.QTUnit;
                this.SB1TLAmount10 = qt1.QTL;
                this.SB1TLUnit10 = lowerci;
                this.SB1TUAmount10 = qt1.QTU;
                this.SB1TUUnit10 = upperci;
                this.SB1MathResult10 = qt1.ErrorMessage;
                this.SB1MathResult10 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount10 = qt1.QT;
                    this.SB1TD1Amount10 = qt1.QTD1;
                    this.SB1TD1Unit10 = qt1.QTD1Unit;
                    this.SB1TD2Amount10 = qt1.QTD2;
                    this.SB1TD2Unit10 = qt1.QTD2Unit;
                }
                this.SB11Amount10 = qt1.Q1;
                this.SB11Unit10 = qt1.Q1Unit;
                this.SB12Amount10 = qt1.Q2;
                this.SB12Unit10 = qt1.Q2Unit;
                this.SB13Amount10 = qt1.Q3;
                this.SB13Unit10 = qt1.Q3Unit;
                this.SB14Amount10 = qt1.Q4;
                this.SB14Unit10 = qt1.Q4Unit;
                this.SB15Amount10 = qt1.Q5;
                this.SB15Unit10 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label11
                && HasMathExpression(this.SB1MathExpression11))
            {
                this.SB1TMAmount11 = qt1.QTM;
                this.SB1TMUnit11 = qt1.QTMUnit;
                this.SB1TUnit11 = qt1.QTUnit;
                this.SB1TLAmount11 = qt1.QTL;
                this.SB1TLUnit11 = lowerci;
                this.SB1TUAmount11 = qt1.QTU;
                this.SB1TUUnit11 = upperci;
                this.SB1MathResult11 = qt1.ErrorMessage;
                this.SB1MathResult11 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount11 = qt1.QT;
                    this.SB1TD1Amount11 = qt1.QTD1;
                    this.SB1TD1Unit11 = qt1.QTD1Unit;
                    this.SB1TD2Amount11 = qt1.QTD2;
                    this.SB1TD2Unit11 = qt1.QTD2Unit;
                }
                this.SB11Amount11 = qt1.Q1;
                this.SB11Unit11 = qt1.Q1Unit;
                this.SB12Amount11 = qt1.Q2;
                this.SB12Unit11 = qt1.Q2Unit;
                this.SB13Amount11 = qt1.Q3;
                this.SB13Unit11 = qt1.Q3Unit;
                this.SB14Amount11 = qt1.Q4;
                this.SB14Unit11 = qt1.Q4Unit;
                this.SB15Amount11 = qt1.Q5;
                this.SB15Unit11 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label12
                && HasMathExpression(this.SB1MathExpression12))
            {
                this.SB1TMAmount12 = qt1.QTM;
                this.SB1TMUnit12 = qt1.QTMUnit;
                this.SB1TUnit12 = qt1.QTUnit;
                this.SB1TLAmount12 = qt1.QTL;
                this.SB1TLUnit12 = lowerci;
                this.SB1TUAmount12 = qt1.QTU;
                this.SB1TUUnit12 = upperci;
                this.SB1MathResult12 = qt1.ErrorMessage;
                this.SB1MathResult12 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount12 = qt1.QT;
                    this.SB1TD1Amount12 = qt1.QTD1;
                    this.SB1TD1Unit12 = qt1.QTD1Unit;
                    this.SB1TD2Amount12 = qt1.QTD2;
                    this.SB1TD2Unit12 = qt1.QTD2Unit;
                }
                this.SB11Amount12 = qt1.Q1;
                this.SB11Unit12 = qt1.Q1Unit;
                this.SB12Amount12 = qt1.Q2;
                this.SB12Unit12 = qt1.Q2Unit;
                this.SB13Amount12 = qt1.Q3;
                this.SB13Unit12 = qt1.Q3Unit;
                this.SB14Amount12 = qt1.Q4;
                this.SB14Unit12 = qt1.Q4Unit;
                this.SB15Amount12 = qt1.Q5;
                this.SB15Unit12 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label13
                && HasMathExpression(this.SB1MathExpression13))
            {
                this.SB1TMAmount13 = qt1.QTM;
                this.SB1TMUnit13 = qt1.QTMUnit;
                this.SB1TUnit13 = qt1.QTUnit;
                this.SB1TLAmount13 = qt1.QTL;
                this.SB1TLUnit13 = lowerci;
                this.SB1TUAmount13 = qt1.QTU;
                this.SB1TUUnit13 = upperci;
                this.SB1MathResult13 = qt1.ErrorMessage;
                this.SB1MathResult13 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount13 = qt1.QT;
                    this.SB1TD1Amount13 = qt1.QTD1;
                    this.SB1TD1Unit13 = qt1.QTD1Unit;
                    this.SB1TD2Amount13 = qt1.QTD2;
                    this.SB1TD2Unit13 = qt1.QTD2Unit;
                }
                this.SB11Amount13 = qt1.Q1;
                this.SB11Unit13 = qt1.Q1Unit;
                this.SB12Amount13 = qt1.Q2;
                this.SB12Unit13 = qt1.Q2Unit;
                this.SB13Amount13 = qt1.Q3;
                this.SB13Unit13 = qt1.Q3Unit;
                this.SB14Amount13 = qt1.Q4;
                this.SB14Unit13 = qt1.Q4Unit;
                this.SB15Amount13 = qt1.Q5;
                this.SB15Unit13 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label14
                && HasMathExpression(this.SB1MathExpression14))
            {
                this.SB1TMAmount14 = qt1.QTM;
                this.SB1TMUnit14 = qt1.QTMUnit;
                this.SB1TUnit14 = qt1.QTUnit;
                this.SB1TLAmount14 = qt1.QTL;
                this.SB1TLUnit14 = lowerci;
                this.SB1TUAmount14 = qt1.QTU;
                this.SB1TUUnit14 = upperci;
                this.SB1MathResult14 = qt1.ErrorMessage;
                this.SB1MathResult14 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount14 = qt1.QT;
                    this.SB1TD1Amount14 = qt1.QTD1;
                    this.SB1TD1Unit14 = qt1.QTD1Unit;
                    this.SB1TD2Amount14 = qt1.QTD2;
                    this.SB1TD2Unit14 = qt1.QTD2Unit;
                }
                this.SB11Amount14 = qt1.Q1;
                this.SB11Unit14 = qt1.Q1Unit;
                this.SB12Amount14 = qt1.Q2;
                this.SB12Unit14 = qt1.Q2Unit;
                this.SB13Amount14 = qt1.Q3;
                this.SB13Unit14 = qt1.Q3Unit;
                this.SB14Amount14 = qt1.Q4;
                this.SB14Unit14 = qt1.Q4Unit;
                this.SB15Amount14 = qt1.Q5;
                this.SB15Unit14 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label15
                && HasMathExpression(this.SB1MathExpression15))
            {
                this.SB1TMAmount15 = qt1.QTM;
                this.SB1TMUnit15 = qt1.QTMUnit;
                this.SB1TUnit15 = qt1.QTUnit;
                this.SB1TLAmount15 = qt1.QTL;
                this.SB1TLUnit15 = lowerci;
                this.SB1TUAmount15 = qt1.QTU;
                this.SB1TUUnit15 = upperci;
                this.SB1MathResult15 = qt1.ErrorMessage;
                this.SB1MathResult15 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount15 = qt1.QT;
                    this.SB1TD1Amount15 = qt1.QTD1;
                    this.SB1TD1Unit15 = qt1.QTD1Unit;
                    this.SB1TD2Amount15 = qt1.QTD2;
                    this.SB1TD2Unit15 = qt1.QTD2Unit;
                }
                this.SB11Amount15 = qt1.Q1;
                this.SB11Unit15 = qt1.Q1Unit;
                this.SB12Amount15 = qt1.Q2;
                this.SB12Unit15 = qt1.Q2Unit;
                this.SB13Amount15 = qt1.Q3;
                this.SB13Unit15 = qt1.Q3Unit;
                this.SB14Amount15 = qt1.Q4;
                this.SB14Unit15 = qt1.Q4Unit;
                this.SB15Amount15 = qt1.Q5;
                this.SB15Unit15 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label16
                && HasMathExpression(this.SB1MathExpression16))
            {
                this.SB1TMAmount16 = qt1.QTM;
                this.SB1TMUnit16 = qt1.QTMUnit;
                this.SB1TUnit16 = qt1.QTUnit;
                this.SB1TLAmount16 = qt1.QTL;
                this.SB1TLUnit16 = lowerci;
                this.SB1TUAmount16 = qt1.QTU;
                this.SB1TUUnit16 = upperci;
                this.SB1MathResult16 = qt1.ErrorMessage;
                this.SB1MathResult16 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount16 = qt1.QT;
                    this.SB1TD1Amount16 = qt1.QTD1;
                    this.SB1TD1Unit16 = qt1.QTD1Unit;
                    this.SB1TD2Amount16 = qt1.QTD2;
                    this.SB1TD2Unit16 = qt1.QTD2Unit;
                }
                this.SB11Amount16 = qt1.Q1;
                this.SB11Unit16 = qt1.Q1Unit;
                this.SB12Amount16 = qt1.Q2;
                this.SB12Unit16 = qt1.Q2Unit;
                this.SB13Amount16 = qt1.Q3;
                this.SB13Unit16 = qt1.Q3Unit;
                this.SB14Amount16 = qt1.Q4;
                this.SB14Unit16 = qt1.Q4Unit;
                this.SB15Amount16 = qt1.Q5;
                this.SB15Unit16 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label17
                && HasMathExpression(this.SB1MathExpression17))
            {
                this.SB1TMAmount17 = qt1.QTM;
                this.SB1TMUnit17 = qt1.QTMUnit;
                this.SB1TUnit17 = qt1.QTUnit;
                this.SB1TLAmount17 = qt1.QTL;
                this.SB1TLUnit17 = lowerci;
                this.SB1TUAmount17 = qt1.QTU;
                this.SB1TUUnit17 = upperci;
                this.SB1MathResult17 = qt1.ErrorMessage;
                this.SB1MathResult17 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount17 = qt1.QT;
                    this.SB1TD1Amount17 = qt1.QTD1;
                    this.SB1TD1Unit17 = qt1.QTD1Unit;
                    this.SB1TD2Amount17 = qt1.QTD2;
                    this.SB1TD2Unit17 = qt1.QTD2Unit;
                }
                this.SB11Amount17 = qt1.Q1;
                this.SB11Unit17 = qt1.Q1Unit;
                this.SB12Amount17 = qt1.Q2;
                this.SB12Unit17 = qt1.Q2Unit;
                this.SB13Amount17 = qt1.Q3;
                this.SB13Unit17 = qt1.Q3Unit;
                this.SB14Amount17 = qt1.Q4;
                this.SB14Unit17 = qt1.Q4Unit;
                this.SB15Amount17 = qt1.Q5;
                this.SB15Unit17 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label18
                && HasMathExpression(this.SB1MathExpression18))
            {
                this.SB1TMAmount18 = qt1.QTM;
                this.SB1TMUnit18 = qt1.QTMUnit;
                this.SB1TUnit18 = qt1.QTUnit;
                this.SB1TLAmount18 = qt1.QTL;
                this.SB1TLUnit18 = lowerci;
                this.SB1TUAmount18 = qt1.QTU;
                this.SB1TUUnit18 = upperci;
                this.SB1MathResult18 = qt1.ErrorMessage;
                this.SB1MathResult18 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount18 = qt1.QT;
                    this.SB1TD1Amount18 = qt1.QTD1;
                    this.SB1TD1Unit18 = qt1.QTD1Unit;
                    this.SB1TD2Amount18 = qt1.QTD2;
                    this.SB1TD2Unit18 = qt1.QTD2Unit;
                }
                this.SB11Amount18 = qt1.Q1;
                this.SB11Unit18 = qt1.Q1Unit;
                this.SB12Amount18 = qt1.Q2;
                this.SB12Unit18 = qt1.Q2Unit;
                this.SB13Amount18 = qt1.Q3;
                this.SB13Unit18 = qt1.Q3Unit;
                this.SB14Amount18 = qt1.Q4;
                this.SB14Unit18 = qt1.Q4Unit;
                this.SB15Amount18 = qt1.Q5;
                this.SB15Unit18 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label19
                && HasMathExpression(this.SB1MathExpression19))
            {
                this.SB1TMAmount19 = qt1.QTM;
                this.SB1TMUnit19 = qt1.QTMUnit;
                this.SB1TUnit19 = qt1.QTUnit;
                this.SB1TLAmount19 = qt1.QTL;
                this.SB1TLUnit19 = lowerci;
                this.SB1TUAmount19 = qt1.QTU;
                this.SB1TUUnit19 = upperci;
                this.SB1MathResult19 = qt1.ErrorMessage;
                this.SB1MathResult19 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount19 = qt1.QT;
                    this.SB1TD1Amount19 = qt1.QTD1;
                    this.SB1TD1Unit19 = qt1.QTD1Unit;
                    this.SB1TD2Amount19 = qt1.QTD2;
                    this.SB1TD2Unit19 = qt1.QTD2Unit;
                }
                this.SB11Amount19 = qt1.Q1;
                this.SB11Unit19 = qt1.Q1Unit;
                this.SB12Amount19 = qt1.Q2;
                this.SB12Unit19 = qt1.Q2Unit;
                this.SB13Amount19 = qt1.Q3;
                this.SB13Unit19 = qt1.Q3Unit;
                this.SB14Amount19 = qt1.Q4;
                this.SB14Unit19 = qt1.Q4Unit;
                this.SB15Amount19 = qt1.Q5;
                this.SB15Unit19 = qt1.Q5Unit;
            }
            else if (label == this.SB1Label20
                && HasMathExpression(this.SB1MathExpression20))
            {
                this.SB1TMAmount20 = qt1.QTM;
                this.SB1TMUnit20 = qt1.QTMUnit;
                this.SB1TUnit20 = qt1.QTUnit;
                this.SB1TLAmount20 = qt1.QTL;
                this.SB1TLUnit20 = lowerci;
                this.SB1TUAmount20 = qt1.QTU;
                this.SB1TUUnit20 = upperci;
                this.SB1MathResult20 = qt1.ErrorMessage;
                this.SB1MathResult20 += qt1.MathResult;
                if (bNeedsDistribution)
                {
                    this.SB1TAmount20 = qt1.QT;
                    this.SB1TD1Amount20 = qt1.QTD1;
                    this.SB1TD1Unit20 = qt1.QTD1Unit;
                    this.SB1TD2Amount20 = qt1.QTD2;
                    this.SB1TD2Unit20 = qt1.QTD2Unit;
                }
                this.SB11Amount20 = qt1.Q1;
                this.SB11Unit20 = qt1.Q1Unit;
                this.SB12Amount20 = qt1.Q2;
                this.SB12Unit20 = qt1.Q2Unit;
                this.SB13Amount20 = qt1.Q3;
                this.SB13Unit20 = qt1.Q3Unit;
                this.SB14Amount20 = qt1.Q4;
                this.SB14Unit20 = qt1.Q4Unit;
                this.SB15Amount20 = qt1.Q5;
                this.SB15Unit20 = qt1.Q5Unit;
            }
            else
            {
                //ignore the row
            }
        }
        private string[] SetScoresFromRandomSamples(string[] indicators, Matrix<double> randomSampleData,
            string[] colNames)
        {
            //1. store the Scores for each row in a double
            List<double> scores = new List<double>();
            //2. set a new SB1Base object by copying this
            var sb1base = new SB1Base(this);
            //3.calculate indicators that are not correlated but may still be in the ind.SetRowQT or Score.MathExpress as constant QTM
            int iIndNumber = 1;
            //add the scoreMUnit to tell CalculateIndicators not to calculate score yet
            List<string> newInds = indicators.ToList();
            newInds.Add(_score);
            sb1base._indicators = newInds.ToArray();
            //this assumes that corrs are only run and stored for scores
            sb1base.CalculateIndicators(iIndNumber);
            //but don't double display the ScoreMathResult
            sb1base.SB1ScoreMathResult = string.Empty;
            //4. use the indicators to set each indicator.QT in the new object from each row of R
            for (int i = 0; i < randomSampleData.RowCount; i++)
            {
                var row = randomSampleData.Row(i);
                //the order of the indicators is the order of the columns
                int j = 0;
                foreach (var ind in indicators)
                {
                    SetRowQT(sb1base, ind, j, row);
                    j++;
                }
                //set sb1Base.Score
                sb1base.SetTotalScore(colNames);
                scores.Add(sb1base.SB1Score);
            }
            List<double> qTs = new List<double>();
            Task<string> tsk = sb1base.SetAlgoPRAStats(_score, qTs, scores.ToArray());
            string sScoreMathR = string.Concat(this.SB1ScoreMathResult, sb1base.SB1ScoreMathResult);
            //5. 204: reset the indicator.QTs to mean of random sample columns 
            //rather than last row of of randoms
            for (int i = 0; i < randomSampleData.ColumnCount; i++)
            {
                var col = randomSampleData.Column(i);
                //the order of the indicators is the order of the columns
                int j = 0;
                foreach (var ind in indicators)
                {
                    if (j == i)
                    {
                        SetColumnQT(sb1base, ind, col);
                    }
                    j++;
                }
            }
            this.CopySB1BaseProperties(sb1base);
            this.SB1ScoreMathResult = string.Empty;
            this.SB1ScoreMathResult = sScoreMathR;
            //186 add the remaining indicators -they've already been calculated
            sb1base.AddAllIndicators(newInds);
            if (!string.IsNullOrEmpty(sb1base.ErrorMessage))
            {
                this.SB1ScoreMathResult += sb1base.ErrorMessage;
                this.ErrorMessage = string.Empty;
            }
            //return the byref indicators
            return newInds.ToArray();
        }
        private void SetRowQT(SB1Base sb1base, string label, int col, Vector<double> row)
        {
            //the indicators were used to set the order of row.cols
            //so their order, col, will correspond to row.column
            if (label == sb1base.SB1Label1)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount1 = row[col];
                }
            }
            else if (label == sb1base.SB1Label2)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount2 = row[col];
                }
            }
            else if (label == sb1base.SB1Label3)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount3 = row[col];
                }
            }
            else if (label == sb1base.SB1Label4)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount4 = row[col];
                }
            }
            else if (label == sb1base.SB1Label5)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount5 = row[col];
                }
            }
            else if (label == sb1base.SB1Label6)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount6 = row[col];
                }
            }
            else if (label == sb1base.SB1Label7)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount7 = row[col];
                }
            }
            else if (label == sb1base.SB1Label8)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount8 = row[col];
                }
            }
            else if (label == sb1base.SB1Label9)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount9 = row[col];
                }
            }
            else if (label == sb1base.SB1Label10)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount10 = row[col];
                }
            }
            else if (label == sb1base.SB1Label11)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount11 = row[col];
                }
            }
            else if (label == sb1base.SB1Label12)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount12 = row[col];
                }
            }
            else if (label == sb1base.SB1Label13)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount13 = row[col];
                }
            }
            else if (label == sb1base.SB1Label14)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount14 = row[col];
                }
            }
            else if (label == sb1base.SB1Label15)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount15 = row[col];
                }
            }
            else if (label == sb1base.SB1Label16)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount16 = row[col];
                }
            }
            else if (label == sb1base.SB1Label17)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount17 = row[col];
                }
            }
            else if (label == sb1base.SB1Label18)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount18 = row[col];
                }
            }
            else if (label == sb1base.SB1Label19)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount19 = row[col];
                }
            }
            else if (label == sb1base.SB1Label20)
            {
                //make sure the index is in row
                if (row.Count >= (col + 1))
                {
                    sb1base.SB1TAmount20 = row[col];
                }
            }
            else
            {
                //colindex = 0
            }
        }
        private void SetColumnQT(SB1Base sb1base, string label, Vector<double> column)
        {
            if (label == sb1base.SB1Label1)
            {
                sb1base.SB1TAmount1 = column.Average();
            }
            else if (label == sb1base.SB1Label2)
            {
                sb1base.SB1TAmount2 = column.Average();
            }
            else if (label == sb1base.SB1Label3)
            {
                sb1base.SB1TAmount3 = column.Average();
            }
            else if (label == sb1base.SB1Label4)
            {
                sb1base.SB1TAmount4 = column.Average();
            }
            else if (label == sb1base.SB1Label5)
            {
                sb1base.SB1TAmount5 = column.Average();
            }
            else if (label == sb1base.SB1Label6)
            {
                sb1base.SB1TAmount6 = column.Average();
            }
            else if (label == sb1base.SB1Label7)
            {
                sb1base.SB1TAmount7 = column.Average();
            }
            else if (label == sb1base.SB1Label8)
            {
                sb1base.SB1TAmount8 = column.Average();
            }
            else if (label == sb1base.SB1Label9)
            {
                sb1base.SB1TAmount9 = column.Average();
            }
            else if (label == sb1base.SB1Label10)
            {
                sb1base.SB1TAmount10 = column.Average();
            }
            else if (label == sb1base.SB1Label11)
            {
                sb1base.SB1TAmount11 = column.Average();
            }
            else if (label == sb1base.SB1Label12)
            {
                sb1base.SB1TAmount12 = column.Average();
            }
            else if (label == sb1base.SB1Label13)
            {
                sb1base.SB1TAmount13 = column.Average();
            }
            else if (label == sb1base.SB1Label14)
            {
                sb1base.SB1TAmount14 = column.Average();
            }
            else if (label == sb1base.SB1Label15)
            {
                sb1base.SB1TAmount15 = column.Average();
            }
            else if (label == sb1base.SB1Label16)
            {
                sb1base.SB1TAmount16 = column.Average();
            }
            else if (label == sb1base.SB1Label17)
            {
                sb1base.SB1TAmount17 = column.Average();
            }
            else if (label == sb1base.SB1Label18)
            {
                sb1base.SB1TAmount18 = column.Average();
            }
            else if (label == sb1base.SB1Label19)
            {
                sb1base.SB1TAmount19 = column.Average();
            }
            else if (label == sb1base.SB1Label20)
            {
                sb1base.SB1TAmount20 = column.Average();
            }
            else
            {
                //colindex = 0
            }
        }
        private async Task<string> SetRGRIndicatorStats(string label, string[] colNames, List<List<double>> data)
        {
            string algoIndicator = string.Empty;
            if (data.Count > 0)
            {
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (label == this.SB1Label1
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression1))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, 
                            colNames, this.SB1MathExpression1, this.SB1MathSubType1, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount1 = rgr.QTPredicted;
                    this.SB1TLAmount1 = rgr.QTL;
                    this.SB1TLUnit1 = sLowerCI;
                    this.SB1TUAmount1 = rgr.QTU;
                    this.SB1TUUnit1 = sUpperCI;
                    //no condition on type of result yet KISS for now
                    this.SB1MathResult1 = rgr.ErrorMessage;
                    this.SB1MathResult1 += rgr.MathResult;
                }
                else if (label == this.SB1Label2
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression2))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression2, this.SB1MathSubType2, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount2 = rgr.QTPredicted;
                    this.SB1TLAmount2 = rgr.QTL;
                    this.SB1TLUnit2 = sLowerCI;
                    this.SB1TUAmount2 = rgr.QTU;
                    this.SB1TUUnit2 = sUpperCI;
                    this.SB1MathResult2 = rgr.ErrorMessage;
                    this.SB1MathResult2 += rgr.MathResult;
                }
                else if (label == this.SB1Label3
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression3))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression3, this.SB1MathSubType3, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount3 = rgr.QTPredicted;
                    this.SB1TLAmount3 = rgr.QTL;
                    this.SB1TLUnit3 = sLowerCI;
                    this.SB1TUAmount3 = rgr.QTU;
                    this.SB1TUUnit3 = sUpperCI;
                    this.SB1MathResult3 = rgr.ErrorMessage;
                    this.SB1MathResult3 += rgr.MathResult;
                }
                else if (label == this.SB1Label4
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression4))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression4, this.SB1MathSubType4, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount4 = rgr.QTPredicted;
                    this.SB1TLAmount4 = rgr.QTL;
                    this.SB1TLUnit4 = sLowerCI;
                    this.SB1TUAmount4 = rgr.QTU;
                    this.SB1TUUnit4 = sUpperCI;
                    this.SB1MathResult4 = rgr.ErrorMessage;
                    this.SB1MathResult4 += rgr.MathResult;
                }
                else if (label == this.SB1Label5
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression5))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression5, this.SB1MathSubType5, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount5 = rgr.QTPredicted;
                    this.SB1TLAmount5 = rgr.QTL;
                    this.SB1TLUnit5 = sLowerCI;
                    this.SB1TUAmount5 = rgr.QTU;
                    this.SB1TUUnit5 = sUpperCI;
                    this.SB1MathResult5 = rgr.ErrorMessage;
                    this.SB1MathResult5 += rgr.MathResult;
                }
                else if (label == this.SB1Label6
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression6))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression6, this.SB1MathSubType6, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount6 = rgr.QTPredicted;
                    this.SB1TLAmount6 = rgr.QTL;
                    this.SB1TLUnit6 = sLowerCI;
                    this.SB1TUAmount6 = rgr.QTU;
                    this.SB1TUUnit6 = sUpperCI;
                    this.SB1MathResult6 = rgr.ErrorMessage;
                    this.SB1MathResult6 += rgr.MathResult;
                }
                else if (label == this.SB1Label7
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression7))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression7, this.SB1MathSubType7, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount7 = rgr.QTPredicted;
                    this.SB1TLAmount7 = rgr.QTL;
                    this.SB1TLUnit7 = sLowerCI;
                    this.SB1TUAmount7 = rgr.QTU;
                    this.SB1TUUnit7 = sUpperCI;
                    this.SB1MathResult7 = rgr.ErrorMessage;
                    this.SB1MathResult7 += rgr.MathResult;
                }
                else if (label == this.SB1Label8
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression8))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression8, this.SB1MathSubType8, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount8 = rgr.QTPredicted;
                    this.SB1TLAmount8 = rgr.QTL;
                    this.SB1TLUnit8 = sLowerCI;
                    this.SB1TUAmount8 = rgr.QTU;
                    this.SB1TUUnit8 = sUpperCI;
                    this.SB1MathResult8 = rgr.ErrorMessage;
                    this.SB1MathResult8 += rgr.MathResult;
                }
                else if (label == this.SB1Label9
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression9))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression9, this.SB1MathSubType9, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount9 = rgr.QTPredicted;
                    this.SB1TLAmount9 = rgr.QTL;
                    this.SB1TLUnit9 = sLowerCI;
                    this.SB1TUAmount9 = rgr.QTU;
                    this.SB1TUUnit9 = sUpperCI;
                    this.SB1MathResult9 = rgr.ErrorMessage;
                    this.SB1MathResult9 += rgr.MathResult;
                }
                else if (label == this.SB1Label10
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression10))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression10, this.SB1MathSubType10, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount10 = rgr.QTPredicted;
                    this.SB1TLAmount10 = rgr.QTL;
                    this.SB1TLUnit10 = sLowerCI;
                    this.SB1TUAmount10 = rgr.QTU;
                    this.SB1TUUnit10 = sUpperCI;
                    this.SB1MathResult10 = rgr.ErrorMessage;
                    this.SB1MathResult10 += rgr.MathResult;
                }
                else if (label == this.SB1Label11
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression11))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression11, this.SB1MathSubType11, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount11 = rgr.QTPredicted;
                    this.SB1TLAmount11 = rgr.QTL;
                    this.SB1TLUnit11 = sLowerCI;
                    this.SB1TUAmount11 = rgr.QTU;
                    this.SB1TUUnit11 = sUpperCI;
                    this.SB1MathResult11 = rgr.ErrorMessage;
                    this.SB1MathResult11 += rgr.MathResult;
                }
                else if (label == this.SB1Label12
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression12))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression12, this.SB1MathSubType12, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount12 = rgr.QTPredicted;
                    this.SB1TLAmount12 = rgr.QTL;
                    this.SB1TLUnit12 = sLowerCI;
                    this.SB1TUAmount12 = rgr.QTU;
                    this.SB1TUUnit12 = sUpperCI;
                    this.SB1MathResult12 = rgr.ErrorMessage;
                    this.SB1MathResult12 += rgr.MathResult;
                }
                else if (label == this.SB1Label13
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression13))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression13, this.SB1MathSubType13, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount13 = rgr.QTPredicted;
                    this.SB1TLAmount13 = rgr.QTL;
                    this.SB1TLUnit13 = sLowerCI;
                    this.SB1TUAmount13 = rgr.QTU;
                    this.SB1TUUnit13 = sUpperCI;
                    this.SB1MathResult13 = rgr.ErrorMessage;
                    this.SB1MathResult13 += rgr.MathResult;
                }
                else if (label == this.SB1Label14
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression14))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression14, this.SB1MathSubType14, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount14 = rgr.QTPredicted;
                    this.SB1TLAmount14 = rgr.QTL;
                    this.SB1TLUnit14 = sLowerCI;
                    this.SB1TUAmount14 = rgr.QTU;
                    this.SB1TUUnit14 = sUpperCI;
                    this.SB1MathResult14 = rgr.ErrorMessage;
                    this.SB1MathResult14 += rgr.MathResult;
                }
                else if (label == this.SB1Label15
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression15))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression15, this.SB1MathSubType15, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount15 = rgr.QTPredicted;
                    this.SB1TLAmount15 = rgr.QTL;
                    this.SB1TLUnit15 = sLowerCI;
                    this.SB1TUAmount15 = rgr.QTU;
                    this.SB1TUUnit15 = sUpperCI;
                    this.SB1MathResult15 = rgr.ErrorMessage;
                    this.SB1MathResult15 += rgr.MathResult;
                }
                else if (label == this.SB1Label16
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression16))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression16, this.SB1MathSubType16, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount16 = rgr.QTPredicted;
                    this.SB1TLAmount16 = rgr.QTL;
                    this.SB1TLUnit16 = sLowerCI;
                    this.SB1TUAmount16 = rgr.QTU;
                    this.SB1TUUnit16 = sUpperCI;
                    this.SB1MathResult16 = rgr.ErrorMessage;
                    this.SB1MathResult16 += rgr.MathResult;
                }
                else if (label == this.SB1Label17
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression17))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression17, this.SB1MathSubType17, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount17 = rgr.QTPredicted;
                    this.SB1TLAmount17 = rgr.QTL;
                    this.SB1TLUnit17 = sLowerCI;
                    this.SB1TUAmount17 = rgr.QTU;
                    this.SB1TUUnit17 = sUpperCI;
                    this.SB1MathResult17 = rgr.ErrorMessage;
                    this.SB1MathResult17 += rgr.MathResult;
                }
                else if (label == this.SB1Label18
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression18))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression18, this.SB1MathSubType18, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount18 = rgr.QTPredicted;
                    this.SB1TLAmount18 = rgr.QTL;
                    this.SB1TLUnit18 = sLowerCI;
                    this.SB1TUAmount18 = rgr.QTU;
                    this.SB1TUUnit18 = sUpperCI;
                    this.SB1MathResult18 = rgr.ErrorMessage;
                    this.SB1MathResult18 += rgr.MathResult;
                }
                else if (label == this.SB1Label19
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression19))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression19, this.SB1MathSubType19, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount19 = rgr.QTPredicted;
                    this.SB1TLAmount19 = rgr.QTL;
                    this.SB1TLUnit19 = sLowerCI;
                    this.SB1TUAmount19 = rgr.QTU;
                    this.SB1TUUnit19 = sUpperCI;
                    this.SB1MathResult19 = rgr.ErrorMessage;
                    this.SB1MathResult19 += rgr.MathResult;
                }
                else if (label == this.SB1Label20
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm6.ToString()
                    || this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression20))
                {
                    algoIndicator = label;
                    DevTreks.Extensions.Algorithms.Regression1 rgr
                        = InitRGR1Algo(label, colNames, this.SB1MathExpression20, this.SB1MathSubType20, this.SB1CILevel);
                    await rgr.RunAlgorithmAsync(data);
                    this.SB1TMAmount20 = rgr.QTPredicted;
                    this.SB1TLAmount20 = rgr.QTL;
                    this.SB1TLUnit20 = sLowerCI;
                    this.SB1TUAmount20 = rgr.QTU;
                    this.SB1TUUnit20 = sUpperCI;
                    this.SB1MathResult20 = rgr.ErrorMessage;
                    this.SB1MathResult20 += rgr.MathResult;
                }
                else
                {
                    //ignore the row
                    
                }
            }
            return algoIndicator;
        }
        private async Task<string> SetANVIndicatorStats(string label, string[] colNames, List<List<double>> data)
        {
            string algoIndicator = string.Empty;
            if (data.Count > 0)
            {
                DevTreks.Extensions.Algorithms.Anova1 anv = new Algorithms.Anova1();
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (label == this.SB1Label1
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression1))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression1, this.SB1MathSubType1, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount1 = anv.QTPredicted;
                    this.SB1TLAmount1 = anv.QTL;
                    this.SB1TLUnit1 = sLowerCI;
                    this.SB1TUAmount1 = anv.QTU;
                    this.SB1TUUnit1 = sUpperCI;
                    this.SB1MathResult1 = anv.ErrorMessage;
                    this.SB1MathResult1 += anv.MathResult;
                }
                else if (label == this.SB1Label2
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression2))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression2, this.SB1MathSubType2, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount2 = anv.QTPredicted;
                    this.SB1TLAmount2 = anv.QTL;
                    this.SB1TLUnit2 = sLowerCI;
                    this.SB1TUAmount2 = anv.QTU;
                    this.SB1TUUnit2 = sUpperCI;
                    this.SB1MathResult2 = anv.ErrorMessage;
                    this.SB1MathResult2 += anv.MathResult;
                }
                else if (label == this.SB1Label3
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression3))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression3, this.SB1MathSubType3, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount3 = anv.QTPredicted;
                    this.SB1TLAmount3 = anv.QTL;
                    this.SB1TLUnit3 = sLowerCI;
                    this.SB1TUAmount3 = anv.QTU;
                    this.SB1TUUnit3 = sUpperCI;
                    this.SB1MathResult3 = anv.ErrorMessage;
                    this.SB1MathResult3 += anv.MathResult;
                }
                else if (label == this.SB1Label4
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression4))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression4, this.SB1MathSubType4, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount4 = anv.QTPredicted;
                    this.SB1TLAmount4 = anv.QTL;
                    this.SB1TLUnit4 = sLowerCI;
                    this.SB1TUAmount4 = anv.QTU;
                    this.SB1TUUnit4 = sUpperCI;
                    this.SB1MathResult4 = anv.ErrorMessage;
                    this.SB1MathResult4 += anv.MathResult;
                }
                else if (label == this.SB1Label5
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression5))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression5, this.SB1MathSubType5, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount5 = anv.QTPredicted;
                    this.SB1TLAmount5 = anv.QTL;
                    this.SB1TLUnit5 = sLowerCI;
                    this.SB1TUAmount5 = anv.QTU;
                    this.SB1TUUnit5 = sUpperCI;
                    this.SB1MathResult5 = anv.ErrorMessage;
                    this.SB1MathResult5 += anv.MathResult;
                }
                else if (label == this.SB1Label6
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression6))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression6, this.SB1MathSubType6, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount6 = anv.QTPredicted;
                    this.SB1TLAmount6 = anv.QTL;
                    this.SB1TLUnit6 = sLowerCI;
                    this.SB1TUAmount6 = anv.QTU;
                    this.SB1TUUnit6 = sUpperCI;
                    this.SB1MathResult6 = anv.ErrorMessage;
                    this.SB1MathResult6 += anv.MathResult;
                }
                else if (label == this.SB1Label7
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression7))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression7, this.SB1MathSubType7, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount7 = anv.QTPredicted;
                    this.SB1TLAmount7 = anv.QTL;
                    this.SB1TLUnit7 = sLowerCI;
                    this.SB1TUAmount7 = anv.QTU;
                    this.SB1TUUnit7 = sUpperCI;
                    this.SB1MathResult7 = anv.ErrorMessage;
                    this.SB1MathResult7 += anv.MathResult;
                }
                else if (label == this.SB1Label8
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression8))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression8, this.SB1MathSubType8, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount8 = anv.QTPredicted;
                    this.SB1TLAmount8 = anv.QTL;
                    this.SB1TLUnit8 = sLowerCI;
                    this.SB1TUAmount8 = anv.QTU;
                    this.SB1TUUnit8 = sUpperCI;
                    this.SB1MathResult8 = anv.ErrorMessage;
                    this.SB1MathResult8 += anv.MathResult;
                }
                else if (label == this.SB1Label9
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression9))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression9, this.SB1MathSubType9, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount9 = anv.QTPredicted;
                    this.SB1TLAmount9 = anv.QTL;
                    this.SB1TLUnit9 = sLowerCI;
                    this.SB1TUAmount9 = anv.QTU;
                    this.SB1TUUnit9 = sUpperCI;
                    this.SB1MathResult9 = anv.ErrorMessage;
                    this.SB1MathResult9 += anv.MathResult;
                }
                else if (label == this.SB1Label10
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression10))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression10, this.SB1MathSubType10, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount10 = anv.QTPredicted;
                    this.SB1TLAmount10 = anv.QTL;
                    this.SB1TLUnit10 = sLowerCI;
                    this.SB1TUAmount10 = anv.QTU;
                    this.SB1TUUnit10 = sUpperCI;
                    this.SB1MathResult10 = anv.ErrorMessage;
                    this.SB1MathResult10 += anv.MathResult;
                }
                else if (label == this.SB1Label11
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression11))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression11, this.SB1MathSubType11, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount11 = anv.QTPredicted;
                    this.SB1TLAmount11 = anv.QTL;
                    this.SB1TLUnit11 = sLowerCI;
                    this.SB1TUAmount11 = anv.QTU;
                    this.SB1TUUnit11 = sUpperCI;
                    this.SB1MathResult11 = anv.ErrorMessage;
                    this.SB1MathResult11 += anv.MathResult;
                }
                else if (label == this.SB1Label12
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression12))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression12, this.SB1MathSubType12, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount12 = anv.QTPredicted;
                    this.SB1TLAmount12 = anv.QTL;
                    this.SB1TLUnit12 = sLowerCI;
                    this.SB1TUAmount12 = anv.QTU;
                    this.SB1TUUnit12 = sUpperCI;
                    this.SB1MathResult12 = anv.ErrorMessage;
                    this.SB1MathResult12 += anv.MathResult;
                }
                else if (label == this.SB1Label13
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression13))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression13, this.SB1MathSubType13, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount13 = anv.QTPredicted;
                    this.SB1TLAmount13 = anv.QTL;
                    this.SB1TLUnit13 = sLowerCI;
                    this.SB1TUAmount13 = anv.QTU;
                    this.SB1TUUnit13 = sUpperCI;
                    this.SB1MathResult13 = anv.ErrorMessage;
                    this.SB1MathResult13 += anv.MathResult;
                }
                else if (label == this.SB1Label14
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression14))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression14, this.SB1MathSubType14, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount14 = anv.QTPredicted;
                    this.SB1TLAmount14 = anv.QTL;
                    this.SB1TLUnit14 = sLowerCI;
                    this.SB1TUAmount14 = anv.QTU;
                    this.SB1TUUnit14 = sUpperCI;
                    this.SB1MathResult14 = anv.ErrorMessage;
                    this.SB1MathResult14 += anv.MathResult;
                }
                else if (label == this.SB1Label15
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression15))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression15, this.SB1MathSubType15, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount15 = anv.QTPredicted;
                    this.SB1TLAmount15 = anv.QTL;
                    this.SB1TLUnit15 = sLowerCI;
                    this.SB1TUAmount15 = anv.QTU;
                    this.SB1TUUnit15 = sUpperCI;
                    this.SB1MathResult15 = anv.ErrorMessage;
                    this.SB1MathResult15 += anv.MathResult;
                }
                else if (label == this.SB1Label16
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression16))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression16, this.SB1MathSubType16, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount16 = anv.QTPredicted;
                    this.SB1TLAmount16 = anv.QTL;
                    this.SB1TLUnit16 = sLowerCI;
                    this.SB1TUAmount16 = anv.QTU;
                    this.SB1TUUnit16 = sUpperCI;
                    this.SB1MathResult16 = anv.ErrorMessage;
                    this.SB1MathResult16 += anv.MathResult;
                }
                else if (label == this.SB1Label17
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression17))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression17, this.SB1MathSubType17, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount17 = anv.QTPredicted;
                    this.SB1TLAmount17 = anv.QTL;
                    this.SB1TLUnit17 = sLowerCI;
                    this.SB1TUAmount17 = anv.QTU;
                    this.SB1TUUnit17 = sUpperCI;
                    this.SB1MathResult17 = anv.ErrorMessage;
                    this.SB1MathResult17 += anv.MathResult;
                }
                else if (label == this.SB1Label18
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression18))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression18, this.SB1MathSubType18, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount18 = anv.QTPredicted;
                    this.SB1TLAmount18 = anv.QTL;
                    this.SB1TLUnit18 = sLowerCI;
                    this.SB1TUAmount18 = anv.QTU;
                    this.SB1TUUnit18 = sUpperCI;
                    this.SB1MathResult18 = anv.ErrorMessage;
                    this.SB1MathResult18 += anv.MathResult;
                }
                else if (label == this.SB1Label19
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression19))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression19, this.SB1MathSubType19, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount19 = anv.QTPredicted;
                    this.SB1TLAmount19 = anv.QTL;
                    this.SB1TLUnit19 = sLowerCI;
                    this.SB1TUAmount19 = anv.QTU;
                    this.SB1TUUnit19 = sUpperCI;
                    this.SB1MathResult19 = anv.ErrorMessage;
                    this.SB1MathResult19 += anv.MathResult;
                }
                else if (label == this.SB1Label20
                    && (this.SB1MathSubType1 == MATH_SUBTYPES.subalgorithm8.ToString())
                    && HasMathExpression(this.SB1MathExpression20))
                {
                    algoIndicator = label;
                    anv = InitANV1Algo(label, colNames, this.SB1MathExpression20, this.SB1MathSubType20, this.SB1CILevel, this.Observations);
                    await anv.RunAlgorithmAsync(data);
                    this.SB1TMAmount20 = anv.QTPredicted;
                    this.SB1TLAmount20 = anv.QTL;
                    this.SB1TLUnit20 = sLowerCI;
                    this.SB1TUAmount20 = anv.QTU;
                    this.SB1TUUnit20 = sUpperCI;
                    this.SB1MathResult20 = anv.ErrorMessage;
                    this.SB1MathResult20 += anv.MathResult;
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
        private async Task<string> SetDRR1IndicatorStats(string label, string[] colNames, 
            List<List<string>> data, List<List<string>> colData, List<string> lines2)
        {
            string algoIndicator = string.Empty;
            if (data.Count > 0)
            {
                DevTreks.Extensions.Algorithms.DRR1 drr = new Algorithms.DRR1();
                string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                if (label == _score
                    && HasMathExpression(this.SB1ScoreMathExpression))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(1, label, colNames, qt1, this.SB1ScoreMathExpression, this.SB1ScoreMathSubType,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label1
                    && HasMathExpression(this.SB1MathExpression1))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(1, label, colNames, qt1, this.SB1MathExpression1, this.SB1MathSubType1,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label2
                    && HasMathExpression(this.SB1MathExpression2))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(2, label, colNames, qt1, this.SB1MathExpression2, this.SB1MathSubType2,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label3
                    && HasMathExpression(this.SB1MathExpression3))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(3, label, colNames, qt1, this.SB1MathExpression3, this.SB1MathSubType3,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label4
                    && HasMathExpression(this.SB1MathExpression4))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(4, label, colNames, qt1, this.SB1MathExpression4, this.SB1MathSubType4,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label5
                    && HasMathExpression(this.SB1MathExpression5))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(5, label, colNames, qt1, this.SB1MathExpression5, this.SB1MathSubType5,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label6
                    && HasMathExpression(this.SB1MathExpression6))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(6, label, colNames, qt1, this.SB1MathExpression6, this.SB1MathSubType6,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label7
                    && HasMathExpression(this.SB1MathExpression7))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(7, label, colNames, qt1, this.SB1MathExpression7, this.SB1MathSubType7,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label8
                    && HasMathExpression(this.SB1MathExpression8))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(8, label, colNames, qt1, this.SB1MathExpression8, this.SB1MathSubType8,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label9
                    && HasMathExpression(this.SB1MathExpression9))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(9, label, colNames, qt1, this.SB1MathExpression9, this.SB1MathSubType9,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label10
                    && HasMathExpression(this.SB1MathExpression10))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(10, label, colNames, qt1, this.SB1MathExpression10, this.SB1MathSubType10,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label11
                    && HasMathExpression(this.SB1MathExpression11))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(11, label, colNames, qt1, this.SB1MathExpression11, this.SB1MathSubType11,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label12
                    && HasMathExpression(this.SB1MathExpression12))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(12, label, colNames, qt1, this.SB1MathExpression12, this.SB1MathSubType12,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label13
                    && HasMathExpression(this.SB1MathExpression13))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(13, label, colNames, qt1, this.SB1MathExpression13, this.SB1MathSubType13,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label14
                    && HasMathExpression(this.SB1MathExpression14))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(14, label, colNames, qt1, this.SB1MathExpression14, this.SB1MathSubType14,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label15
                    && HasMathExpression(this.SB1MathExpression15))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(15, label, colNames, qt1, this.SB1MathExpression15, this.SB1MathSubType15,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label16
                    && HasMathExpression(this.SB1MathExpression16))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(16, label, colNames, qt1, this.SB1MathExpression16, this.SB1MathSubType16,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label17
                    && HasMathExpression(this.SB1MathExpression17))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(17, label, colNames, qt1, this.SB1MathExpression17, this.SB1MathSubType17,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label18
                    && HasMathExpression(this.SB1MathExpression18))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(18, label, colNames, qt1, this.SB1MathExpression18, this.SB1MathSubType18,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label19
                    && HasMathExpression(this.SB1MathExpression19))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(19, label, colNames, qt1, this.SB1MathExpression19, this.SB1MathSubType19,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label20
                    && HasMathExpression(this.SB1MathExpression20))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    drr = InitDRR1Algo(20, label, colNames, qt1, this.SB1MathExpression20, this.SB1MathSubType20,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await drr.RunAlgorithmAsync(data, colData, lines2);
                    FillBaseIndicator(drr.IndicatorQT, drr.IndicatorQT.Label, sLowerCI, sUpperCI);
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
        private async Task<string> SetDRR2IndicatorStats(string label, string[] colNames,
            List<List<string>> data, List<List<string>> colData, List<string> lines2)
        {
            string algoIndicator = string.Empty;
            if (data.Count > 0)
            {
                DevTreks.Extensions.Algorithms.DRR2 rmi = new Algorithms.DRR2();
                string sLowerCI = string.Empty;
                string sUpperCI = string.Empty;
                if (this.SB1CILevel != 0)
                {
                    sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                    sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
                }
                if (label == _score
                    && HasMathExpression(this.SB1ScoreMathExpression))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    //this must to use a zero index
                    rmi = InitDRR2Algo(0, label, colNames, qt1, this.SB1ScoreMathExpression, this.SB1ScoreMathSubType,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label1
                    && HasMathExpression(this.SB1MathExpression1))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    //this must use a 1 index
                    rmi = InitDRR2Algo(1, label, colNames, qt1, this.SB1MathExpression1, this.SB1MathSubType1,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label2
                    && HasMathExpression(this.SB1MathExpression2))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(2, label, colNames, qt1, this.SB1MathExpression2, this.SB1MathSubType2,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label3
                    && HasMathExpression(this.SB1MathExpression3))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(3, label, colNames, qt1, this.SB1MathExpression3, this.SB1MathSubType3,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label4
                    && HasMathExpression(this.SB1MathExpression4))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(4, label, colNames, qt1, this.SB1MathExpression4, this.SB1MathSubType4,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label5
                    && HasMathExpression(this.SB1MathExpression5))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(5, label, colNames, qt1, this.SB1MathExpression5, this.SB1MathSubType5,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label6
                    && HasMathExpression(this.SB1MathExpression6))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(6, label, colNames, qt1, this.SB1MathExpression6, this.SB1MathSubType6,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label7
                    && HasMathExpression(this.SB1MathExpression7))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(7, label, colNames, qt1, this.SB1MathExpression7, this.SB1MathSubType7,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label8
                    && HasMathExpression(this.SB1MathExpression8))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(8, label, colNames, qt1, this.SB1MathExpression8, this.SB1MathSubType8,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label9
                    && HasMathExpression(this.SB1MathExpression9))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(9, label, colNames, qt1, this.SB1MathExpression9, this.SB1MathSubType9,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label10
                    && HasMathExpression(this.SB1MathExpression10))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(10, label, colNames, qt1, this.SB1MathExpression10, this.SB1MathSubType10,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label11
                    && HasMathExpression(this.SB1MathExpression11))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(11, label, colNames, qt1, this.SB1MathExpression11, this.SB1MathSubType11,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label12
                    && HasMathExpression(this.SB1MathExpression12))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(12, label, colNames, qt1, this.SB1MathExpression12, this.SB1MathSubType12,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label13
                    && HasMathExpression(this.SB1MathExpression13))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(13, label, colNames, qt1, this.SB1MathExpression13, this.SB1MathSubType13,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label14
                    && HasMathExpression(this.SB1MathExpression14))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(14, label, colNames, qt1, this.SB1MathExpression14, this.SB1MathSubType14,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label15
                    && HasMathExpression(this.SB1MathExpression15))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(15, label, colNames, qt1, this.SB1MathExpression15, this.SB1MathSubType15,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label16
                    && HasMathExpression(this.SB1MathExpression16))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(16, label, colNames, qt1, this.SB1MathExpression16, this.SB1MathSubType16,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label17
                    && HasMathExpression(this.SB1MathExpression17))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(17, label, colNames, qt1, this.SB1MathExpression17, this.SB1MathSubType17,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label18
                    && HasMathExpression(this.SB1MathExpression18))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(18, label, colNames, qt1, this.SB1MathExpression18, this.SB1MathSubType18,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label19
                    && HasMathExpression(this.SB1MathExpression19))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(19, label, colNames, qt1, this.SB1MathExpression19, this.SB1MathSubType19,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
                }
                else if (label == this.SB1Label20
                    && HasMathExpression(this.SB1MathExpression20))
                {
                    algoIndicator = label;
                    IndicatorQT1 qt1 = FillIndicator(label);
                    rmi = InitDRR2Algo(20, label, colNames, qt1, this.SB1MathExpression20, this.SB1MathSubType20,
                        this.SB1CILevel, this.SB1Iterations, this.SB1Random, this.Observations);
                    await rmi.RunAlgorithmAsync2(data, colData, lines2);
                    FillBaseIndicator(rmi.IndicatorQT, rmi.IndicatorQT.Label, sLowerCI, sUpperCI);
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
        public static int GetSiblingIndicatorIndex(string label, int indWithMathExpressIndex, SB1Base calcor)
        {
            //indicators are 1 based
            //last sibling indicator holds mathexpression if rules followed
            int iSiblingIndex = 0;
            if (label == calcor.SB1Label1
                && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression1) == false
                && indWithMathExpressIndex != 1)
            {
                iSiblingIndex = 1;
            }
            else if (label == calcor.SB1Label2
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression2) == false
                && indWithMathExpressIndex != 2)
            {
                iSiblingIndex = 2;
            }
            else if (label == calcor.SB1Label3
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression3) == false
                && indWithMathExpressIndex != 3)
            {
                iSiblingIndex = 3;
            }
            else if (label == calcor.SB1Label4
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression4) == false
                && indWithMathExpressIndex != 4)
            {
                iSiblingIndex = 4;
            }
            else if (label == calcor.SB1Label5
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression5) == false
                && indWithMathExpressIndex != 5)
            {
                iSiblingIndex = 5;
            }
            else if (label == calcor.SB1Label6
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression6) == false
                && indWithMathExpressIndex != 6)
            {
                iSiblingIndex = 6;
            }
            else if (label == calcor.SB1Label7
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression7) == false
                && indWithMathExpressIndex != 7)
            {
                iSiblingIndex = 7;
            }
            else if (label == calcor.SB1Label8
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression8) == false
                && indWithMathExpressIndex != 8)
            {
                iSiblingIndex = 8;
            }
            else if (label == calcor.SB1Label9
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression9) == false
                && indWithMathExpressIndex != 9)
            {
                iSiblingIndex = 9;
            }
            else if (label == calcor.SB1Label10
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression10) == false
                && indWithMathExpressIndex != 10)
            {
                iSiblingIndex = 10;
            }
            else if (label == calcor.SB1Label11
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression11) == false
                && indWithMathExpressIndex != 11)
            {
                iSiblingIndex = 11;
            }
            else if (label == calcor.SB1Label12
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression12) == false
                && indWithMathExpressIndex != 12)
            {
                iSiblingIndex = 12;
            }
            else if (label == calcor.SB1Label13
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression13) == false
                && indWithMathExpressIndex != 13)
            {
                iSiblingIndex = 13;
            }
            else if (label == calcor.SB1Label14
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression14) == false
                && indWithMathExpressIndex != 14)
            {
                iSiblingIndex = 14;
            }
            else if (label == calcor.SB1Label15
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression15) == false
                && indWithMathExpressIndex != 15)
            {
                iSiblingIndex = 15;
            }
            else if (label == calcor.SB1Label16
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression16) == false
                && indWithMathExpressIndex != 16)
            {
                iSiblingIndex = 16;
            }
            else if (label == calcor.SB1Label17
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression17) == false
                && indWithMathExpressIndex != 17)
            {
                iSiblingIndex = 17;
            }
            else if (label == calcor.SB1Label18
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression18) == false
                && indWithMathExpressIndex != 18)
            {
                iSiblingIndex = 18;
            }
            else if (label == calcor.SB1Label19
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression19) == false
                && indWithMathExpressIndex != 19)
            {
                iSiblingIndex = 19;
            }
            else if (label == calcor.SB1Label20
                 && SB1Statistics.SB1Algos.HasMathExpression(calcor.SB1MathExpression20) == false
                && indWithMathExpressIndex != 20)
            {
                iSiblingIndex = 20;
            }
            return iSiblingIndex;
        }
        private async Task<string> SetScriptCloudStats(string label, string[] colNames, 
            string dataURL, string jDataURL)
        {
            string algoIndicator = label;
            System.Threading.CancellationToken ctk = new System.Threading.CancellationToken();
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            if (label == this.SB1Label1
                && (this.SB1MathType1 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression1))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression1, this.SB1MathType1, this.SB1MathSubType1);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount1 = script2.QTPredicted;
                this.SB1TLAmount1 = script2.QTL;
                this.SB1TLUnit1 = sLowerCI;
                this.SB1TUAmount1 = script2.QTU;
                this.SB1TUUnit1 = sUpperCI;
                //no condition on type of result yet KISS for now
                this.SB1MathResult1 = script2.ErrorMessage;
                this.SB1MathResult1 += script2.MathResult;
            }
            else if (label == this.SB1Label2
                 && (this.SB1MathType2 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression2))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression2, this.SB1MathType2, this.SB1MathSubType2);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount2 = script2.QTPredicted;
                this.SB1TLAmount2 = script2.QTL;
                this.SB1TLUnit2 = sLowerCI;
                this.SB1TUAmount2 = script2.QTU;
                this.SB1TUUnit2 = sUpperCI;
                this.SB1MathResult2 = script2.ErrorMessage;
                this.SB1MathResult2 += script2.MathResult;
            }
            else if (label == this.SB1Label3
                 && (this.SB1MathType3 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression3))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression3, this.SB1MathType3, this.SB1MathSubType3);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount3 = script2.QTPredicted;
                this.SB1TLAmount3 = script2.QTL;
                this.SB1TLUnit3 = sLowerCI;
                this.SB1TUAmount3 = script2.QTU;
                this.SB1TUUnit3 = sUpperCI;
                this.SB1MathResult3 = script2.ErrorMessage;
                this.SB1MathResult3 += script2.MathResult;
            }
            else if (label == this.SB1Label4
                 && (this.SB1MathType4 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression4))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression4, this.SB1MathType4, this.SB1MathSubType4);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount4 = script2.QTPredicted;
                this.SB1TLAmount4 = script2.QTL;
                this.SB1TLUnit4 = sLowerCI;
                this.SB1TUAmount4 = script2.QTU;
                this.SB1TUUnit4 = sUpperCI;
                this.SB1MathResult4 = script2.ErrorMessage;
                this.SB1MathResult4 += script2.MathResult;
            }
            else if (label == this.SB1Label5
                 && (this.SB1MathType5 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression5))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression5, this.SB1MathType5, this.SB1MathSubType5);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount5 = script2.QTPredicted;
                this.SB1TLAmount5 = script2.QTL;
                this.SB1TLUnit5 = sLowerCI;
                this.SB1TUAmount5 = script2.QTU;
                this.SB1TUUnit5 = sUpperCI;
                this.SB1MathResult5 = script2.ErrorMessage;
                this.SB1MathResult5 += script2.MathResult;
            }
            else if (label == this.SB1Label6
                 && (this.SB1MathType6 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression6))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression6, this.SB1MathType6, this.SB1MathSubType6);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount6 = script2.QTPredicted;
                this.SB1TLAmount6 = script2.QTL;
                this.SB1TLUnit6 = sLowerCI;
                this.SB1TUAmount6 = script2.QTU;
                this.SB1TUUnit6 = sUpperCI;
                this.SB1MathResult6 = script2.ErrorMessage;
                this.SB1MathResult6 += script2.MathResult;
            }
            else if (label == this.SB1Label7
                 && (this.SB1MathType7 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression7))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression7, this.SB1MathType7, this.SB1MathSubType7);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount7 = script2.QTPredicted;
                this.SB1TLAmount7 = script2.QTL;
                this.SB1TLUnit7 = sLowerCI;
                this.SB1TUAmount7 = script2.QTU;
                this.SB1TUUnit7 = sUpperCI;
                this.SB1MathResult7 = script2.ErrorMessage;
                this.SB1MathResult7 += script2.MathResult;
            }
            else if (label == this.SB1Label8
                 && (this.SB1MathType8 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression8))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression8, this.SB1MathType8, this.SB1MathSubType8);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount8 = script2.QTPredicted;
                this.SB1TLAmount8 = script2.QTL;
                this.SB1TLUnit8 = sLowerCI;
                this.SB1TUAmount8 = script2.QTU;
                this.SB1TUUnit8 = sUpperCI;
                this.SB1MathResult8 = script2.ErrorMessage;
                this.SB1MathResult8 += script2.MathResult;
            }
            else if (label == this.SB1Label9
                 && (this.SB1MathType9 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression9))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression9, this.SB1MathType9, this.SB1MathSubType9);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount9 = script2.QTPredicted;
                this.SB1TLAmount9 = script2.QTL;
                this.SB1TLUnit9 = sLowerCI;
                this.SB1TUAmount9 = script2.QTU;
                this.SB1TUUnit9 = sUpperCI;
                this.SB1MathResult9 = script2.ErrorMessage;
                this.SB1MathResult9 += script2.MathResult;
            }
            else if (label == this.SB1Label10
                 && (this.SB1MathType10 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression10))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression10, this.SB1MathType10, this.SB1MathSubType10);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount10 = script2.QTPredicted;
                this.SB1TLAmount10 = script2.QTL;
                this.SB1TLUnit10 = sLowerCI;
                this.SB1TUAmount10 = script2.QTU;
                this.SB1TUUnit10 = sUpperCI;
                this.SB1MathResult10 = script2.ErrorMessage;
                this.SB1MathResult10 += script2.MathResult;
            }
            else if (label == this.SB1Label11
                 && (this.SB1MathType11 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression11))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression11, this.SB1MathType11, this.SB1MathSubType11);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount11 = script2.QTPredicted;
                this.SB1TLAmount11 = script2.QTL;
                this.SB1TLUnit11 = sLowerCI;
                this.SB1TUAmount11 = script2.QTU;
                this.SB1TUUnit11 = sUpperCI;
                this.SB1MathResult11 = script2.ErrorMessage;
                this.SB1MathResult11 += script2.MathResult;
            }
            else if (label == this.SB1Label12
                 && (this.SB1MathType12 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression12))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression12, this.SB1MathType12, this.SB1MathSubType12);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount12 = script2.QTPredicted;
                this.SB1TLAmount12 = script2.QTL;
                this.SB1TLUnit12 = sLowerCI;
                this.SB1TUAmount12 = script2.QTU;
                this.SB1TUUnit12 = sUpperCI;
                this.SB1MathResult12 = script2.ErrorMessage;
                this.SB1MathResult12 += script2.MathResult;
            }
            else if (label == this.SB1Label13
                 && (this.SB1MathType13 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression13))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression13, this.SB1MathType13, this.SB1MathSubType13);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount13 = script2.QTPredicted;
                this.SB1TLAmount13 = script2.QTL;
                this.SB1TLUnit13 = sLowerCI;
                this.SB1TUAmount13 = script2.QTU;
                this.SB1TUUnit13 = sUpperCI;
                this.SB1MathResult13 = script2.ErrorMessage;
                this.SB1MathResult13 += script2.MathResult;
            }
            else if (label == this.SB1Label14
                 && (this.SB1MathType14 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression14))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression14, this.SB1MathType14, this.SB1MathSubType14);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount14 = script2.QTPredicted;
                this.SB1TLAmount14 = script2.QTL;
                this.SB1TLUnit14 = sLowerCI;
                this.SB1TUAmount14 = script2.QTU;
                this.SB1TUUnit14 = sUpperCI;
                this.SB1MathResult14 = script2.ErrorMessage;
                this.SB1MathResult14 += script2.MathResult;
            }
            else if (label == this.SB1Label15
                 && (this.SB1MathType15 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression15))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression15, this.SB1MathType15, this.SB1MathSubType15);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount15 = script2.QTPredicted;
                this.SB1TLAmount15 = script2.QTL;
                this.SB1TLUnit15 = sLowerCI;
                this.SB1TUAmount15 = script2.QTU;
                this.SB1TUUnit15 = sUpperCI;
                this.SB1MathResult15 = script2.ErrorMessage;
                this.SB1MathResult15 += script2.MathResult;
            }
            else if (label == this.SB1Label16
                 && (this.SB1MathType16 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression16))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression16, this.SB1MathType16, this.SB1MathSubType16);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount16 = script2.QTPredicted;
                this.SB1TLAmount16 = script2.QTL;
                this.SB1TLUnit16 = sLowerCI;
                this.SB1TUAmount16 = script2.QTU;
                this.SB1TUUnit16 = sUpperCI;
                this.SB1MathResult16 = script2.ErrorMessage;
                this.SB1MathResult16 += script2.MathResult;
            }
            else if (label == this.SB1Label17
                 && (this.SB1MathType17 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression17))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression17, this.SB1MathType17, this.SB1MathSubType17);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount17 = script2.QTPredicted;
                this.SB1TLAmount17 = script2.QTL;
                this.SB1TLUnit17 = sLowerCI;
                this.SB1TUAmount17 = script2.QTU;
                this.SB1TUUnit17 = sUpperCI;
                this.SB1MathResult17 = script2.ErrorMessage;
                this.SB1MathResult17 += script2.MathResult;
            }
            else if (label == this.SB1Label18
                && (this.SB1MathType18 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression18))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression18, this.SB1MathType18, this.SB1MathSubType18);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount18 = script2.QTPredicted;
                this.SB1TLAmount18 = script2.QTL;
                this.SB1TLUnit18 = sLowerCI;
                this.SB1TUAmount18 = script2.QTU;
                this.SB1TUUnit18 = sUpperCI;
                this.SB1MathResult18 = script2.ErrorMessage;
                this.SB1MathResult18 += script2.MathResult;
            }
            else if (label == this.SB1Label19
                 && (this.SB1MathType19 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression19))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression19, this.SB1MathType19, this.SB1MathSubType19);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount19 = script2.QTPredicted;
                this.SB1TLAmount19 = script2.QTL;
                this.SB1TLUnit19 = sLowerCI;
                this.SB1TUAmount19 = script2.QTU;
                this.SB1TUUnit19 = sUpperCI;
                this.SB1MathResult19 = script2.ErrorMessage;
                this.SB1MathResult19 += script2.MathResult;
            }
            else if (label == this.SB1Label20
                 && (this.SB1MathType20 == MATH_TYPES.algorithm4.ToString())
                && HasMathExpression(this.SB1MathExpression20))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script2 script2
                    = InitScript2Algo(label, colNames, this.SB1MathExpression20, this.SB1MathType20, this.SB1MathSubType20);
                bool bHasCalcs = await script2.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount20 = script2.QTPredicted;
                this.SB1TLAmount20 = script2.QTL;
                this.SB1TLUnit20 = sLowerCI;
                this.SB1TUAmount20 = script2.QTU;
                this.SB1TUUnit20 = sUpperCI;
                this.SB1MathResult20 = script2.ErrorMessage;
                this.SB1MathResult20 += script2.MathResult;
            }
            else
            {
                //ignore the row
            }
            return algoIndicator;
        }
        private async Task<string> SetScriptWebStats(string label, string[] colNames,
            string dataURL, string jDataURL)
        {
            string algoIndicator = label;
            System.Threading.CancellationToken ctk = new System.Threading.CancellationToken();
            string sLowerCI = string.Concat(Errors.GetMessage("LOWER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            string sUpperCI = string.Concat(Errors.GetMessage("UPPER"), this.SB1CILevel.ToString(), Errors.GetMessage("CI_PCT"));
            if (label == this.SB1Label1
                && (this.SB1MathType1 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType1 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression1))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression1, this.SB1MathType1, this.SB1MathSubType1);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount1 = script1.QTPredicted;
                this.SB1TLAmount1 = script1.QTL;
                this.SB1TLUnit1 = sLowerCI;
                this.SB1TUAmount1 = script1.QTU;
                this.SB1TUUnit1 = sUpperCI;
                //no condition on type of result yet KISS for now
                this.SB1MathResult1 = script1.ErrorMessage;
                this.SB1MathResult1 += script1.MathResult;
            }
            else if (label == this.SB1Label2
                && (this.SB1MathType2 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType2 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression2))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression2, this.SB1MathType2, this.SB1MathSubType2);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount2 = script1.QTPredicted;
                this.SB1TLAmount2 = script1.QTL;
                this.SB1TLUnit2 = sLowerCI;
                this.SB1TUAmount2 = script1.QTU;
                this.SB1TUUnit2 = sUpperCI;
                this.SB1MathResult2 = script1.ErrorMessage;
                this.SB1MathResult2 += script1.MathResult;
            }
            else if (label == this.SB1Label3
                && (this.SB1MathType3 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType3 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression3))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression3, this.SB1MathType3, this.SB1MathSubType3);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount3 = script1.QTPredicted;
                this.SB1TLAmount3 = script1.QTL;
                this.SB1TLUnit3 = sLowerCI;
                this.SB1TUAmount3 = script1.QTU;
                this.SB1TUUnit3 = sUpperCI;
                this.SB1MathResult3 = script1.ErrorMessage;
                this.SB1MathResult3 += script1.MathResult;
            }
            else if (label == this.SB1Label4
                && (this.SB1MathType4 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType4 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression4))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression4, this.SB1MathType4, this.SB1MathSubType4);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount4 = script1.QTPredicted;
                this.SB1TLAmount4 = script1.QTL;
                this.SB1TLUnit4 = sLowerCI;
                this.SB1TUAmount4 = script1.QTU;
                this.SB1TUUnit4 = sUpperCI;
                this.SB1MathResult4 = script1.ErrorMessage;
                this.SB1MathResult4 += script1.MathResult;
            }
            else if (label == this.SB1Label5
               && (this.SB1MathType5 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType5 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression5))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression5, this.SB1MathType5, this.SB1MathSubType5);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount5 = script1.QTPredicted;
                this.SB1TLAmount5 = script1.QTL;
                this.SB1TLUnit5 = sLowerCI;
                this.SB1TUAmount5 = script1.QTU;
                this.SB1TUUnit5 = sUpperCI;
                this.SB1MathResult5 = script1.ErrorMessage;
                this.SB1MathResult5 += script1.MathResult;
            }
            else if (label == this.SB1Label6
                && (this.SB1MathType6 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType6 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression6))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression6, this.SB1MathType6, this.SB1MathSubType6);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount6 = script1.QTPredicted;
                this.SB1TLAmount6 = script1.QTL;
                this.SB1TLUnit6 = sLowerCI;
                this.SB1TUAmount6 = script1.QTU;
                this.SB1TUUnit6 = sUpperCI;
                this.SB1MathResult6 = script1.ErrorMessage;
                this.SB1MathResult6 += script1.MathResult;
            }
            else if (label == this.SB1Label7
                && (this.SB1MathType7 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType7 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression7))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression7, this.SB1MathType7, this.SB1MathSubType7);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount7 = script1.QTPredicted;
                this.SB1TLAmount7 = script1.QTL;
                this.SB1TLUnit7 = sLowerCI;
                this.SB1TUAmount7 = script1.QTU;
                this.SB1TUUnit7 = sUpperCI;
                this.SB1MathResult7 = script1.ErrorMessage;
                this.SB1MathResult7 += script1.MathResult;
            }
            else if (label == this.SB1Label8
                && (this.SB1MathType8 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType8 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression8))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression8, this.SB1MathType8, this.SB1MathSubType8);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount8 = script1.QTPredicted;
                this.SB1TLAmount8 = script1.QTL;
                this.SB1TLUnit8 = sLowerCI;
                this.SB1TUAmount8 = script1.QTU;
                this.SB1TUUnit8 = sUpperCI;
                this.SB1MathResult8 = script1.ErrorMessage;
                this.SB1MathResult8 += script1.MathResult;
            }
            else if (label == this.SB1Label9
                && (this.SB1MathType9 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType9 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression9))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression9, this.SB1MathType9, this.SB1MathSubType9);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount9 = script1.QTPredicted;
                this.SB1TLAmount9 = script1.QTL;
                this.SB1TLUnit9 = sLowerCI;
                this.SB1TUAmount9 = script1.QTU;
                this.SB1TUUnit9 = sUpperCI;
                this.SB1MathResult9 = script1.ErrorMessage;
                this.SB1MathResult9 += script1.MathResult;
            }
            else if (label == this.SB1Label10
                && (this.SB1MathType10 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType10 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression10))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression10, this.SB1MathType10, this.SB1MathSubType10);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount10 = script1.QTPredicted;
                this.SB1TLAmount10 = script1.QTL;
                this.SB1TLUnit10 = sLowerCI;
                this.SB1TUAmount10 = script1.QTU;
                this.SB1TUUnit10 = sUpperCI;
                this.SB1MathResult10 = script1.ErrorMessage;
                this.SB1MathResult10 += script1.MathResult;
            }
            else if (label == this.SB1Label11
                && (this.SB1MathType11 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType11 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression11))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression11, this.SB1MathType11, this.SB1MathSubType11);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount11 = script1.QTPredicted;
                this.SB1TLAmount11 = script1.QTL;
                this.SB1TLUnit11 = sLowerCI;
                this.SB1TUAmount11 = script1.QTU;
                this.SB1TUUnit11 = sUpperCI;
                this.SB1MathResult11 = script1.ErrorMessage;
                this.SB1MathResult11 += script1.MathResult;
            }
            else if (label == this.SB1Label12
                && (this.SB1MathType12 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType12 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression12))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression12, this.SB1MathType12, this.SB1MathSubType12);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount12 = script1.QTPredicted;
                this.SB1TLAmount12 = script1.QTL;
                this.SB1TLUnit12 = sLowerCI;
                this.SB1TUAmount12 = script1.QTU;
                this.SB1TUUnit12 = sUpperCI;
                this.SB1MathResult12 = script1.ErrorMessage;
                this.SB1MathResult12 += script1.MathResult;
            }
            else if (label == this.SB1Label13
                && (this.SB1MathType13 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType13 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression13))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression13, this.SB1MathType13, this.SB1MathSubType13);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount13 = script1.QTPredicted;
                this.SB1TLAmount13 = script1.QTL;
                this.SB1TLUnit13 = sLowerCI;
                this.SB1TUAmount13 = script1.QTU;
                this.SB1TUUnit13 = sUpperCI;
                this.SB1MathResult13 = script1.ErrorMessage;
                this.SB1MathResult13 += script1.MathResult;
            }
            else if (label == this.SB1Label14
                && (this.SB1MathType14 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType14 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression14))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression14, this.SB1MathType14, this.SB1MathSubType14);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount14 = script1.QTPredicted;
                this.SB1TLAmount14 = script1.QTL;
                this.SB1TLUnit14 = sLowerCI;
                this.SB1TUAmount14 = script1.QTU;
                this.SB1TUUnit14 = sUpperCI;
                this.SB1MathResult14 = script1.ErrorMessage;
                this.SB1MathResult14 += script1.MathResult;
            }
            else if (label == this.SB1Label15
                && (this.SB1MathType15 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType15 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression15))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression15, this.SB1MathType15, this.SB1MathSubType15);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount15 = script1.QTPredicted;
                this.SB1TLAmount15 = script1.QTL;
                this.SB1TLUnit15 = sLowerCI;
                this.SB1TUAmount15 = script1.QTU;
                this.SB1TUUnit15 = sUpperCI;
                this.SB1MathResult15 = script1.ErrorMessage;
                this.SB1MathResult15 += script1.MathResult;
            }
            else if (label == this.SB1Label16
                && (this.SB1MathType16 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType16 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression16))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression16, this.SB1MathType16, this.SB1MathSubType16);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount16 = script1.QTPredicted;
                this.SB1TLAmount16 = script1.QTL;
                this.SB1TLUnit16 = sLowerCI;
                this.SB1TUAmount16 = script1.QTU;
                this.SB1TUUnit16 = sUpperCI;
                this.SB1MathResult16 = script1.ErrorMessage;
                this.SB1MathResult16 += script1.MathResult;
            }
            else if (label == this.SB1Label17
                && (this.SB1MathType17 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType17 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression17))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression17, this.SB1MathType17, this.SB1MathSubType17);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount17 = script1.QTPredicted;
                this.SB1TLAmount17 = script1.QTL;
                this.SB1TLUnit17 = sLowerCI;
                this.SB1TUAmount17 = script1.QTU;
                this.SB1TUUnit17 = sUpperCI;
                this.SB1MathResult17 = script1.ErrorMessage;
                this.SB1MathResult17 += script1.MathResult;
            }
            else if (label == this.SB1Label18
                && (this.SB1MathType18 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType18 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression18))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression18, this.SB1MathType18, this.SB1MathSubType18);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount18 = script1.QTPredicted;
                this.SB1TLAmount18 = script1.QTL;
                this.SB1TLUnit18 = sLowerCI;
                this.SB1TUAmount18 = script1.QTU;
                this.SB1TUUnit18 = sUpperCI;
                this.SB1MathResult18 = script1.ErrorMessage;
                this.SB1MathResult18 += script1.MathResult;
            }
            else if (label == this.SB1Label19
                && (this.SB1MathType19 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType19 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression19))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression19, this.SB1MathType19, this.SB1MathSubType19);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount19 = script1.QTPredicted;
                this.SB1TLAmount19 = script1.QTL;
                this.SB1TLUnit19 = sLowerCI;
                this.SB1TUAmount19 = script1.QTU;
                this.SB1TUUnit19 = sUpperCI;
                this.SB1MathResult19 = script1.ErrorMessage;
                this.SB1MathResult19 += script1.MathResult;
            }
            else if (label == this.SB1Label20
                && (this.SB1MathType20 == MATH_TYPES.algorithm2.ToString()
                    || this.SB1MathType20 == MATH_TYPES.algorithm3.ToString())
                && HasMathExpression(this.SB1MathExpression20))
            {
                algoIndicator = label;
                DevTreks.Extensions.Algorithms.Script1 script1
                    = InitScript1Algo(label, colNames, this.SB1MathExpression20, this.SB1MathType20, this.SB1MathSubType20);
                bool bHasCalcs = await script1.RunAlgorithmAsync(dataURL, jDataURL, ctk);
                this.SB1TMAmount20 = script1.QTPredicted;
                this.SB1TLAmount20 = script1.QTL;
                this.SB1TLUnit20 = sLowerCI;
                this.SB1TUAmount20 = script1.QTU;
                this.SB1TUUnit20 = sUpperCI;
                this.SB1MathResult20 = script1.ErrorMessage;
                this.SB1MathResult20 += script1.MathResult;
            }
            else
            {
                //ignore the row
            }
            return algoIndicator;
        }
       
        private List<List<string>> GetAlgo8Model(string label, string jDataURL)
        {
            string sError = string.Empty;
            List<string> lines = CalculatorHelpers.ReadLines(this.CalcParameters.ExtensionDocToCalcURI, jDataURL, out sError);
            List<string> cLines = lines
                .Where(l => l.StartsWith(label, true, CultureInfo.InvariantCulture))
                .Select(l => l.ToString()).ToList();
            List<List<string>> model = new List<List<string>>();
            if (cLines.Count <= 0)
            {
                this.ErrorMessage += string.Concat(" ", Errors.MakeStandardErrorMsg("JDATAURL_BAD"), jDataURL);
                return model;
            }
            foreach (var jdata in cLines)
            {
                string[] distributes = jdata.Split(Constants.CSV_DELIMITERS);
                List<string> lstDist = new List<string>(distributes);
                model.Add(lstDist);
            }
            return model;
            ////gets build from Joint Data
            //Dictionary<string, List<double>> model = new Dictionary<string, List<double>>();
            //string sKey = Calculator1.RUC_TYPES.normal.ToString();
            //List<double> dist1 = new List<double>();
            //dist1.Add(15);
            //dist1.Add(0.01);
            //model.Add(sKey, dist1);
            //sKey = Calculator1.RUC_TYPES.gamma.ToString();
            //dist1 = new List<double>();
            //dist1.Add(2);
            //dist1.Add(0.5);
            //model.Add(sKey, dist1);
        }
        private DevTreks.Extensions.Algorithms.PRA1 InitPRA1Algo(string label, string subalgo,
            string[] colNames, IndicatorQT1 qt1, int confidInt, int iterations, int random, List<double> qTs)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, qt1.QMathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(label, mathTerms);
            int totalsNeeded = 0;
            DevTreks.Extensions.Algorithms.PRA1 pra
                    = new Algorithms.PRA1(label, mathTerms.ToArray(), colNames, depColNames.ToArray(),
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
        private DevTreks.Extensions.Algorithms.Regression1 InitRGR1Algo(string label, string[] colNames, 
            string mathExpression, string subalgo, int confidInt)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(label, mathTerms);
            DevTreks.Extensions.Algorithms.Regression1 rgr
                    = new Algorithms.Regression1(mathTerms.ToArray(), colNames, depColNames.ToArray(), 
                        qs.ToArray(), subalgo, confidInt, this.CalcParameters);
            return rgr;
        }
        private DevTreks.Extensions.Algorithms.Anova1 InitANV1Algo(string label, string[] colNames,
            string mathExpression, string subalgo, int confidInt, int totalsNeeded)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
            //List<double> qs = GetQsForMathTerms(label, mathTerms);
            if (totalsNeeded <= 0) totalsNeeded = 3;
            DevTreks.Extensions.Algorithms.Anova1 anv
                    = new Algorithms.Anova1(label, mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, this.CalcParameters);
            return anv;
        }
        private DevTreks.Extensions.Algorithms.DRR1 InitDRR1Algo(int indicatorIndex, string label, string[] colNames,
            IndicatorQT1 qt1, string mathExpression, string subalgo, int confidInt, int iterations, 
            int random, int totalsNeeded)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(label, mathTerms);
            if (totalsNeeded <= 0) totalsNeeded = 3;
            DevTreks.Extensions.Algorithms.DRR1 DRR
                    = new Algorithms.DRR1(indicatorIndex, label, mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, iterations, random, qs, qt1, this.CalcParameters);
            return DRR;
        }
        private DevTreks.Extensions.Algorithms.DRR2 InitDRR2Algo(int indicatorIndex, string label, string[] colNames,
            IndicatorQT1 qt1, string mathExpression, string subalgo, int confidInt, int iterations,
            int random, int totalsNeeded)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(label, mathTerms);
            if (totalsNeeded <= 0) totalsNeeded = 3;
            DevTreks.Extensions.Algorithms.DRR2 RMI
                    = new Algorithms.DRR2(indicatorIndex, label, mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        totalsNeeded, subalgo, confidInt, iterations, random, qs, qt1, this.CalcParameters);
            return RMI;
        }
        private DevTreks.Extensions.Algorithms.Script2 InitScript2Algo(string label, string[] colNames,
           string mathExpression, string algorithm, string subalgorithm)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(label, mathTerms);
            DevTreks.Extensions.Algorithms.Script2 script2
                    = new Algorithms.Script2(mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        qs.ToArray(), algorithm, subalgorithm, this.CalcParameters);
            return script2;
        }
        private DevTreks.Extensions.Algorithms.Script1 InitScript1Algo(string label, string[] colNames,
           string mathExpression, string algorithm, string subalgorithm)
        {
            //mathterms define which qamount to send to algorith for predicting a given set of qxs
            List<string> mathTerms = new List<string>();
            //dependent var colNames found in MathExpression
            List<string> depColNames = new List<string>();
            GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
            List<double> qs = GetQsForMathTerms(label, mathTerms);
            DevTreks.Extensions.Algorithms.Script1 script1
                = new Algorithms.Script1(mathTerms.ToArray(), colNames, depColNames.ToArray(),
                        qs.ToArray(), algorithm, subalgorithm, this.CalcParameters);
            return script1;
        }
        //keep for reference
        //private DevTreks.Extensions.Algorithms.BayesMNRegress1 InitBayesRegress1Algo(string label, string[] colNames,
        //   string mathExpression)
        //{
        //    //mathterms define which qamount to send to algorith for predicting a given set of qxs
        //    List<string> mathTerms = new List<string>();
        //    //dependent var colNames found in MathExpression
        //    List<string> depColNames = new List<string>();
        //    GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
        //    List<double> qs = GetQsForMathTerms(label, mathTerms);
        //    DevTreks.Extensions.Algorithms.BayesMNRegress1 bayes1
        //            = new Algorithms.BayesMNRegress1(mathTerms.ToArray(), colNames, depColNames.ToArray(), qs.ToArray(), 
        //                this.SB1Iterations);
        //    return bayes1;
        //}
        //private DevTreks.Extensions.Algorithms.BayesInfer1 InitBayes1Algo(string label, string[] colNames,
        //    string mathExpression)
        //{
        //    //mathterms define which qamount to send to algorith for predicting a given set of qxs
        //    List<string> mathTerms = new List<string>();
        //    //dependent var colNames found in MathExpression
        //    List<string> depColNames = new List<string>();
        //    GetDataToAnalyzeColNames(label, mathExpression, colNames, ref depColNames, ref mathTerms);
        //    List<double> qs = GetQsForMathTerms(label, mathTerms);
        //    DevTreks.Extensions.Algorithms.BayesInfer1 bayes1
        //            = new Algorithms.BayesInfer1(mathTerms.ToArray(), colNames, depColNames.ToArray(), qs.ToArray());
        //    return bayes1;
        //}
        //can't be async so byref is ok
        public void GetDataToAnalyzeColNames(string label, string mathExpression, string[] colNames, 
            ref List<string> depColNames, ref List<string> mathTerms)
        {
            string sMathTerm = string.Empty;
            if (colNames == null)
                colNames = new string[] { };
            if (!string.IsNullOrEmpty(mathExpression)
                && colNames.Count() >= 5)
            {
                //regression puts intercept first and is not in the mathexpression
                if (this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm6)
                    || this.HasMathType(label, MATH_TYPES.algorithm1, MATH_SUBTYPES.subalgorithm8))
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

                            sMathTerm = ParseMathTerm(label, mathExpression, colNames[j]);
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
        public string ParseMathTerm(string label, string mathExpression, string colName)
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
                    sMATHTERM = GetMathTerm(label, sParsedTerm);
                }
                ////the mathterm is either 6 chars or 7 chars in front (I1.Q1. or I10.Q5.)
                ////start seven chars in front to account for period separator
                //if ((iEndIndex - 7) >= 0)
                //    sParsedTerm = mathExpression.Substring((iEndIndex - 7), 7);
                //if (!string.IsNullOrEmpty(sParsedTerm))
                //{
                //    if (!sParsedTerm.ToLower().StartsWith("i"))
                //    {
                //        //get rid of (,+)
                //        sParsedTerm = sParsedTerm.Substring(1, sParsedTerm.Count() - 1);
                //    }
                //    if (!sParsedTerm.ToLower().StartsWith("i"))
                //    {
                //        sParsedTerm = sParsedTerm.Substring(1, sParsedTerm.Count() - 1);
                //    }
                //    if (sParsedTerm.EndsWith(Constants.FILEEXTENSION_DELIMITER))
                //        sParsedTerm = sParsedTerm.Substring(0, sParsedTerm.Count() - 1);
                //    sMATHTERM = GetMathTerm(label, sParsedTerm);
                //}
                //if (string.IsNullOrEmpty(sMATHTERM))
                //{
                //    //start six chars in front to account for period separator
                //    if ((iEndIndex - 6) >= 0)
                //        sParsedTerm = mathExpression.Substring((iEndIndex - 6), 6);
                //    if (!string.IsNullOrEmpty(sParsedTerm))
                //    {
                //        if (!sParsedTerm.ToLower().StartsWith("i"))
                //        {
                //            sParsedTerm = sParsedTerm.Substring(1, sParsedTerm.Count() - 1);
                //        }
                //        if (sParsedTerm.EndsWith(Constants.FILEEXTENSION_DELIMITER))
                //            sParsedTerm = sParsedTerm.Substring(0, sParsedTerm.Count() - 1);
                //        sMATHTERM = GetMathTerm(label, sParsedTerm);
                //    }
                //}
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
        private string GetMathTerm(string label, string parsedIxQx)
        {
            string sMathTerm = string.Empty;
            bool bHasMathTerm = false;
            //the units must be set correctly
            //and the mathexpress has to contain the var
            if (label == this.SB1Label1)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 0, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label2)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 1, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label3)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 2, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label4)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 3, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label5)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 4, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label6)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 5, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label7)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 6, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label8)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 7, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label9)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 8, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label10)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 9, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label11)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 10, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label12)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 11, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label13)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 12, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label14)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 13, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label15)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 14, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label16)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 15, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label17)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 16, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label18)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 17, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label19)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 18, 15))
                {
                    bHasMathTerm = true;
                }
            }
            if (label == this.SB1Label20)
            {
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 0))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 1))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 2))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 3))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 4))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 10))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 11))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 12))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 13))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 14))
                {
                    bHasMathTerm = true;
                }
                if (SB1Base.HasMathTerm(parsedIxQx, 19, 15))
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
        private List<double> GetQsForMathTerms(string label, List<string> mathTerms)
        {
            //index has to match mathterms (and adjcolnames)
            List<double> qs = new List<double>(mathTerms.Count);
            //the units must be set correctly
            //and the mathexpress has to contain the var
            if (label == this.SB1Label1)
            {
                if (SB1Base.HasMathTerm(mathTerms, 0, 0))
                {
                    qs.Add(this.SB11Amount1);
                }
                if (SB1Base.HasMathTerm(mathTerms, 0, 1))
                {
                    qs.Add(this.SB12Amount1);
                }
                if (SB1Base.HasMathTerm(mathTerms, 0, 2))
                {
                    qs.Add(this.SB13Amount1);
                }
                if (SB1Base.HasMathTerm(mathTerms, 0, 3))
                {
                    qs.Add(this.SB14Amount1);
                }
                if (SB1Base.HasMathTerm(mathTerms, 0, 4))
                {
                    qs.Add(this.SB15Amount1);
                }
                //Q5 is the limit
                //if (SB1Base.HasMathTerm(mathTerms, z, 11))
                //{
                //    qs.Add(this.SB15Amount1);
                //}
            }
            if (label == this.SB1Label2)
            {
                if (SB1Base.HasMathTerm(mathTerms, 1, 0))
                {
                    qs.Add(this.SB11Amount2);
                }
                if (SB1Base.HasMathTerm(mathTerms, 1, 1))
                {
                    qs.Add(this.SB12Amount2);
                }
                if (SB1Base.HasMathTerm(mathTerms, 1, 2))
                {
                    qs.Add(this.SB13Amount2);
                }
                if (SB1Base.HasMathTerm(mathTerms, 1, 3))
                {
                    qs.Add(this.SB14Amount2);
                }
                if (SB1Base.HasMathTerm(mathTerms, 1, 4))
                {
                    qs.Add(this.SB15Amount2);
                }
            }
            if (label == this.SB1Label3)
            {
                if (SB1Base.HasMathTerm(mathTerms, 2, 0))
                {
                    qs.Add(this.SB11Amount3);
                }
                if (SB1Base.HasMathTerm(mathTerms, 2, 1))
                {
                    qs.Add(this.SB12Amount3);
                }
                if (SB1Base.HasMathTerm(mathTerms, 2, 2))
                {
                    qs.Add(this.SB13Amount3);
                }
                if (SB1Base.HasMathTerm(mathTerms, 2, 3))
                {
                    qs.Add(this.SB14Amount3);
                }
                if (SB1Base.HasMathTerm(mathTerms, 2, 4))
                {
                    qs.Add(this.SB15Amount3);
                }
            }
            if (label == this.SB1Label4)
            {
                if (SB1Base.HasMathTerm(mathTerms, 3, 0))
                {
                    qs.Add(this.SB11Amount4);
                }
                if ( HasMathTerm(mathTerms, 3, 1))
                {
                    qs.Add(this.SB12Amount4);
                }
                if (SB1Base.HasMathTerm(mathTerms, 3, 2))
                {
                    qs.Add(this.SB13Amount4);
                }
                if (SB1Base.HasMathTerm(mathTerms, 3, 3))
                {
                    qs.Add(this.SB14Amount4);
                }
                if (SB1Base.HasMathTerm(mathTerms, 3, 4))
                {
                    qs.Add(this.SB15Amount4);
                }
            }
            if (label == this.SB1Label5)
            {
                if (SB1Base.HasMathTerm(mathTerms, 4, 0))
                {
                    qs.Add(this.SB11Amount5);
                }
                if (SB1Base.HasMathTerm(mathTerms, 4, 1))
                {
                    qs.Add(this.SB12Amount5);
                }
                if (SB1Base.HasMathTerm(mathTerms, 4, 2))
                {
                    qs.Add(this.SB13Amount5);
                }
                if (SB1Base.HasMathTerm(mathTerms, 4, 3))
                {
                    qs.Add(this.SB14Amount5);
                }
                if (SB1Base.HasMathTerm(mathTerms, 4, 4))
                {
                    qs.Add(this.SB15Amount5);
                }
            }
            if (label == this.SB1Label6)
            {
                if (SB1Base.HasMathTerm(mathTerms, 5, 0))
                {
                    qs.Add(this.SB11Amount6);
                }
                if (SB1Base.HasMathTerm(mathTerms, 5, 1))
                {
                    qs.Add(this.SB12Amount6);
                }
                if (SB1Base.HasMathTerm(mathTerms, 5, 2))
                {
                    qs.Add(this.SB13Amount6);
                }
                if (SB1Base.HasMathTerm(mathTerms, 5, 3))
                {
                    qs.Add(this.SB14Amount6);
                }
                if (SB1Base.HasMathTerm(mathTerms, 5, 4))
                {
                    qs.Add(this.SB15Amount6);
                }
            }
            if (label == this.SB1Label7)
            {
                if (SB1Base.HasMathTerm(mathTerms, 6, 0))
                {
                    qs.Add(this.SB11Amount7);
                }
                if (SB1Base.HasMathTerm(mathTerms, 6, 1))
                {
                    qs.Add(this.SB12Amount7);
                }
                if (SB1Base.HasMathTerm(mathTerms, 6, 2))
                {
                    qs.Add(this.SB13Amount7);
                }
                if (SB1Base.HasMathTerm(mathTerms, 6, 3))
                {
                    qs.Add(this.SB14Amount7);
                }
                if (SB1Base.HasMathTerm(mathTerms, 6, 4))
                {
                    qs.Add(this.SB15Amount7);
                }
            }
            if (label == this.SB1Label8)
            {
                if (SB1Base.HasMathTerm(mathTerms, 7, 0))
                {
                    qs.Add(this.SB11Amount8);
                }
                if (SB1Base.HasMathTerm(mathTerms, 7, 1))
                {
                    qs.Add(this.SB12Amount8);
                }
                if (SB1Base.HasMathTerm(mathTerms, 7, 2))
                {
                    qs.Add(this.SB13Amount8);
                }
                if (SB1Base.HasMathTerm(mathTerms, 7, 3))
                {
                    qs.Add(this.SB14Amount8);
                }
                if (SB1Base.HasMathTerm(mathTerms, 7, 4))
                {
                    qs.Add(this.SB15Amount8);
                }
            }
            if (label == this.SB1Label9)
            {
                if (SB1Base.HasMathTerm(mathTerms, 8, 0))
                {
                    qs.Add(this.SB11Amount9);
                }
                if (SB1Base.HasMathTerm(mathTerms, 8, 1))
                {
                    qs.Add(this.SB12Amount9);
                }
                if (SB1Base.HasMathTerm(mathTerms, 8, 2))
                {
                    qs.Add(this.SB13Amount9);
                }
                if (SB1Base.HasMathTerm(mathTerms, 8, 3))
                {
                    qs.Add(this.SB14Amount9);
                }
                if (SB1Base.HasMathTerm(mathTerms, 8, 4))
                {
                    qs.Add(this.SB15Amount9);
                }
            }
            if (label == this.SB1Label10)
            {
                if (SB1Base.HasMathTerm(mathTerms, 9, 0))
                {
                    qs.Add(this.SB11Amount10);
                }
                if (SB1Base.HasMathTerm(mathTerms, 9, 1))
                {
                    qs.Add(this.SB12Amount10);
                }
                if (SB1Base.HasMathTerm(mathTerms, 9, 2))
                {
                    qs.Add(this.SB13Amount10);
                }
                if (SB1Base.HasMathTerm(mathTerms, 9, 3))
                {
                    qs.Add(this.SB14Amount10);
                }
                if (SB1Base.HasMathTerm(mathTerms, 9, 4))
                {
                    qs.Add(this.SB15Amount10);
                }
            }
            if (label == this.SB1Label11)
            {
                if (SB1Base.HasMathTerm(mathTerms, 10, 0))
                {
                    qs.Add(this.SB11Amount11);
                }
                if (SB1Base.HasMathTerm(mathTerms, 10, 1))
                {
                    qs.Add(this.SB12Amount11);
                }
                if (SB1Base.HasMathTerm(mathTerms, 10, 2))
                {
                    qs.Add(this.SB13Amount11);
                }
                if (SB1Base.HasMathTerm(mathTerms, 10, 3))
                {
                    qs.Add(this.SB14Amount11);
                }
                if (SB1Base.HasMathTerm(mathTerms, 10, 4))
                {
                    qs.Add(this.SB15Amount11);
                }
            }
            if (label == this.SB1Label12)
            {
                if (SB1Base.HasMathTerm(mathTerms, 11, 0))
                {
                    qs.Add(this.SB11Amount12);
                }
                if (SB1Base.HasMathTerm(mathTerms, 11, 1))
                {
                    qs.Add(this.SB12Amount12);
                }
                if (SB1Base.HasMathTerm(mathTerms, 11, 2))
                {
                    qs.Add(this.SB13Amount12);
                }
                if (SB1Base.HasMathTerm(mathTerms, 11, 3))
                {
                    qs.Add(this.SB14Amount12);
                }
                if (SB1Base.HasMathTerm(mathTerms, 11, 4))
                {
                    qs.Add(this.SB15Amount12);
                }
            }
            if (label == this.SB1Label13)
            {
                if (SB1Base.HasMathTerm(mathTerms, 12, 0))
                {
                    qs.Add(this.SB11Amount13);
                }
                if (SB1Base.HasMathTerm(mathTerms, 12, 1))
                {
                    qs.Add(this.SB12Amount13);
                }
                if (SB1Base.HasMathTerm(mathTerms, 12, 2))
                {
                    qs.Add(this.SB13Amount13);
                }
                if (SB1Base.HasMathTerm(mathTerms, 12, 3))
                {
                    qs.Add(this.SB14Amount13);
                }
                if (SB1Base.HasMathTerm(mathTerms, 12, 4))
                {
                    qs.Add(this.SB15Amount13);
                }
            }
            if (label == this.SB1Label14)
            {
                if (SB1Base.HasMathTerm(mathTerms, 13, 0))
                {
                    qs.Add(this.SB11Amount14);
                }
                if (SB1Base.HasMathTerm(mathTerms, 13, 1))
                {
                    qs.Add(this.SB12Amount14);
                }
                if (SB1Base.HasMathTerm(mathTerms, 13, 2))
                {
                    qs.Add(this.SB13Amount14);
                }
                if (SB1Base.HasMathTerm(mathTerms, 13, 3))
                {
                    qs.Add(this.SB14Amount14);
                }
                if (SB1Base.HasMathTerm(mathTerms, 13, 4))
                {
                    qs.Add(this.SB15Amount14);
                }
            }
            if (label == this.SB1Label15)
            {
                if (SB1Base.HasMathTerm(mathTerms, 14, 0))
                {
                    qs.Add(this.SB11Amount15);
                }
                if (SB1Base.HasMathTerm(mathTerms, 14, 1))
                {
                    qs.Add(this.SB12Amount15);
                }
                if (SB1Base.HasMathTerm(mathTerms, 14, 2))
                {
                    qs.Add(this.SB13Amount15);
                }
                if (SB1Base.HasMathTerm(mathTerms, 14, 3))
                {
                    qs.Add(this.SB14Amount15);
                }
                if (SB1Base.HasMathTerm(mathTerms, 14, 4))
                {
                    qs.Add(this.SB15Amount15);
                }
            }
            if (label == this.SB1Label16)
            {
                if (SB1Base.HasMathTerm(mathTerms, 15, 0))
                {
                    qs.Add(this.SB11Amount16);
                }
                if (SB1Base.HasMathTerm(mathTerms, 15, 1))
                {
                    qs.Add(this.SB12Amount16);
                }
                if (SB1Base.HasMathTerm(mathTerms, 15, 2))
                {
                    qs.Add(this.SB13Amount16);
                }
                if (SB1Base.HasMathTerm(mathTerms, 15, 3))
                {
                    qs.Add(this.SB14Amount16);
                }
                if (SB1Base.HasMathTerm(mathTerms, 15, 4))
                {
                    qs.Add(this.SB15Amount16);
                }
            }
            if (label == this.SB1Label17)
            {
                if (SB1Base.HasMathTerm(mathTerms, 16, 0))
                {
                    qs.Add(this.SB11Amount17);
                }
                if (SB1Base.HasMathTerm(mathTerms, 16, 1))
                {
                    qs.Add(this.SB12Amount17);
                }
                if (SB1Base.HasMathTerm(mathTerms, 16, 2))
                {
                    qs.Add(this.SB13Amount17);
                }
                if (SB1Base.HasMathTerm(mathTerms, 16, 3))
                {
                    qs.Add(this.SB14Amount17);
                }
                if (SB1Base.HasMathTerm(mathTerms, 16, 4))
                {
                    qs.Add(this.SB15Amount17);
                }
            }
            if (label == this.SB1Label18)
            {
                if (SB1Base.HasMathTerm(mathTerms, 17, 0))
                {
                    qs.Add(this.SB11Amount18);
                }
                if (SB1Base.HasMathTerm(mathTerms, 17, 1))
                {
                    qs.Add(this.SB12Amount18);
                }
                if (SB1Base.HasMathTerm(mathTerms, 17, 2))
                {
                    qs.Add(this.SB13Amount18);
                }
                if (SB1Base.HasMathTerm(mathTerms, 17, 3))
                {
                    qs.Add(this.SB14Amount18);
                }
                if (SB1Base.HasMathTerm(mathTerms, 17, 4))
                {
                    qs.Add(this.SB15Amount18);
                }
            }
            if (label == this.SB1Label19)
            {
                if (SB1Base.HasMathTerm(mathTerms, 18, 0))
                {
                    qs.Add(this.SB11Amount19);
                }
                if (SB1Base.HasMathTerm(mathTerms, 18, 1))
                {
                    qs.Add(this.SB12Amount19);
                }
                if (SB1Base.HasMathTerm(mathTerms, 18, 2))
                {
                    qs.Add(this.SB13Amount19);
                }
                if (SB1Base.HasMathTerm(mathTerms, 18, 3))
                {
                    qs.Add(this.SB14Amount19);
                }
                if (SB1Base.HasMathTerm(mathTerms, 18, 4))
                {
                    qs.Add(this.SB15Amount19);
                }
            }
            if (label == this.SB1Label20)
            {
                if (SB1Base.HasMathTerm(mathTerms, 19, 0))
                {
                    qs.Add(this.SB11Amount20);
                }
                if (SB1Base.HasMathTerm(mathTerms, 19, 1))
                {
                    qs.Add(this.SB12Amount20);
                }
                if (SB1Base.HasMathTerm(mathTerms, 19, 2))
                {
                    qs.Add(this.SB13Amount20);
                }
                if (SB1Base.HasMathTerm(mathTerms, 19, 3))
                {
                    qs.Add(this.SB14Amount20);
                }
                if (SB1Base.HasMathTerm(mathTerms, 19, 4))
                {
                    qs.Add(this.SB15Amount20);
                }
            }
            int iMissingQs = qs.Count - mathTerms.Count;
            for (int i = 0; i < iMissingQs; i++)
            {
                qs.Add(0);
            }
                return qs;
        }
        private List<double> GetQsForMathTermswUnits(string label, List<string> mathTerms)
        {
            List<double> qs = new List<double>();
            //the units must be set correctly
            //and the mathexpress has to contain the var
            if (label == this.SB1Label1)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit1)
                    && this.SB11Unit1 != Constants.NONE
                    && HasMathTerm(mathTerms, 0, 0))
                {
                    qs.Add(this.SB11Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit1)
                    && this.SB12Unit1 != Constants.NONE
                    && HasMathTerm(mathTerms, 0, 1))
                {
                    qs.Add(this.SB12Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit1)
                    && this.SB13Unit1 != Constants.NONE
                    && HasMathTerm(mathTerms, 0, 2))
                {
                    qs.Add(this.SB13Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit1)
                    && this.SB14Unit1 != Constants.NONE
                    && HasMathTerm(mathTerms, 0, 3))
                {
                    qs.Add(this.SB14Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit1)
                    && this.SB15Unit1 != Constants.NONE
                    && HasMathTerm(mathTerms, 0, 4))
                {
                    qs.Add(this.SB15Amount1);
                }
            }
            if (label == this.SB1Label2)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit2)
                    && this.SB11Unit2 != Constants.NONE
                    && HasMathTerm(mathTerms, 1, 0))
                {
                    qs.Add(this.SB11Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit2)
                    && this.SB12Unit2 != Constants.NONE
                    && HasMathTerm(mathTerms, 1, 1))
                {
                    qs.Add(this.SB12Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit2)
                    && this.SB13Unit2 != Constants.NONE
                    && HasMathTerm(mathTerms, 1, 2))
                {
                    qs.Add(this.SB13Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit2)
                    && this.SB14Unit2 != Constants.NONE
                    && HasMathTerm(mathTerms, 1, 3))
                {
                    qs.Add(this.SB14Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit2)
                    && this.SB15Unit2 != Constants.NONE
                    && HasMathTerm(mathTerms, 1, 4))
                {
                    qs.Add(this.SB15Amount2);
                }
            }
            if (label == this.SB1Label3)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit3)
                    && this.SB11Unit3 != Constants.NONE
                    && HasMathTerm(mathTerms, 2, 0))
                {
                    qs.Add(this.SB11Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit3)
                    && this.SB12Unit3 != Constants.NONE
                    && HasMathTerm(mathTerms, 2, 1))
                {
                    qs.Add(this.SB12Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit3)
                    && this.SB13Unit3 != Constants.NONE
                    && HasMathTerm(mathTerms, 2, 2))
                {
                    qs.Add(this.SB13Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit3)
                    && this.SB14Unit3 != Constants.NONE
                    && HasMathTerm(mathTerms, 2, 3))
                {
                    qs.Add(this.SB14Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit3)
                    && this.SB15Unit3 != Constants.NONE
                    && HasMathTerm(mathTerms, 2, 4))
                {
                    qs.Add(this.SB15Amount3);
                }
            }
            if (label == this.SB1Label4)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit4)
                    && this.SB11Unit4 != Constants.NONE
                    && HasMathTerm(mathTerms, 3, 0))
                {
                    qs.Add(this.SB11Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit4)
                    && this.SB12Unit4 != Constants.NONE
                    && HasMathTerm(mathTerms, 3, 1))
                {
                    qs.Add(this.SB12Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit4)
                    && this.SB13Unit4 != Constants.NONE
                    && HasMathTerm(mathTerms, 3, 2))
                {
                    qs.Add(this.SB13Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit4)
                    && this.SB14Unit4 != Constants.NONE
                    && HasMathTerm(mathTerms, 3, 3))
                {
                    qs.Add(this.SB14Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit4)
                    && this.SB15Unit4 != Constants.NONE
                    && HasMathTerm(mathTerms, 3, 4))
                {
                    qs.Add(this.SB15Amount4);
                }
            }
            if (label == this.SB1Label5)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit5)
                    && this.SB11Unit5 != Constants.NONE
                    && HasMathTerm(mathTerms, 4, 0))
                {
                    qs.Add(this.SB11Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit5)
                    && this.SB12Unit5 != Constants.NONE
                    && HasMathTerm(mathTerms, 4, 1))
                {
                    qs.Add(this.SB12Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit5)
                    && this.SB13Unit5 != Constants.NONE
                    && HasMathTerm(mathTerms, 4, 2))
                {
                    qs.Add(this.SB13Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit5)
                    && this.SB14Unit5 != Constants.NONE
                    && HasMathTerm(mathTerms, 4, 3))
                {
                    qs.Add(this.SB14Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit5)
                    && this.SB15Unit5 != Constants.NONE
                    && HasMathTerm(mathTerms, 4, 4))
                {
                    qs.Add(this.SB15Amount5);
                }
            }
            if (label == this.SB1Label6)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit6)
                    && this.SB11Unit6 != Constants.NONE
                    && HasMathTerm(mathTerms, 5, 0))
                {
                    qs.Add(this.SB11Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit6)
                    && this.SB12Unit6 != Constants.NONE
                    && HasMathTerm(mathTerms, 5, 1))
                {
                    qs.Add(this.SB12Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit6)
                    && this.SB13Unit6 != Constants.NONE
                    && HasMathTerm(mathTerms, 5, 2))
                {
                    qs.Add(this.SB13Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit6)
                    && this.SB14Unit6 != Constants.NONE
                    && HasMathTerm(mathTerms, 5, 3))
                {
                    qs.Add(this.SB14Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit6)
                    && this.SB15Unit6 != Constants.NONE
                    && HasMathTerm(mathTerms, 5, 4))
                {
                    qs.Add(this.SB15Amount6);
                }
            }
            if (label == this.SB1Label7)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit7)
                    && this.SB11Unit7 != Constants.NONE
                    && HasMathTerm(mathTerms, 6, 0))
                {
                    qs.Add(this.SB11Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit7)
                    && this.SB12Unit7 != Constants.NONE
                    && HasMathTerm(mathTerms, 6, 1))
                {
                    qs.Add(this.SB12Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit7)
                    && this.SB13Unit7 != Constants.NONE
                    && HasMathTerm(mathTerms, 6, 2))
                {
                    qs.Add(this.SB13Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit7)
                    && this.SB14Unit7 != Constants.NONE
                    && HasMathTerm(mathTerms, 6, 3))
                {
                    qs.Add(this.SB14Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit7)
                    && this.SB15Unit7 != Constants.NONE
                    && HasMathTerm(mathTerms, 6, 4))
                {
                    qs.Add(this.SB15Amount7);
                }
            }
            if (label == this.SB1Label8)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit8)
                    && this.SB11Unit8 != Constants.NONE
                    && HasMathTerm(mathTerms, 7, 0))
                {
                    qs.Add(this.SB11Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit8)
                    && this.SB12Unit8 != Constants.NONE
                     && HasMathTerm(mathTerms, 7, 1))
                {
                    qs.Add(this.SB12Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit8)
                    && this.SB13Unit8 != Constants.NONE
                     && HasMathTerm(mathTerms, 7, 2))
                {
                    qs.Add(this.SB13Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit8)
                    && this.SB14Unit8 != Constants.NONE
                     && HasMathTerm(mathTerms, 7, 3))
                {
                    qs.Add(this.SB14Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit8)
                    && this.SB15Unit8 != Constants.NONE
                     && HasMathTerm(mathTerms, 7, 4))
                {
                    qs.Add(this.SB15Amount8);
                }
            }
            if (label == this.SB1Label9)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit9)
                    && this.SB11Unit9 != Constants.NONE
                    && HasMathTerm(mathTerms, 8, 0))
                {
                    qs.Add(this.SB11Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit9)
                    && this.SB12Unit9 != Constants.NONE
                    && HasMathTerm(mathTerms, 8, 1))
                {
                    qs.Add(this.SB12Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit9)
                    && this.SB13Unit9 != Constants.NONE
                    && HasMathTerm(mathTerms, 8, 2))
                {
                    qs.Add(this.SB13Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit9)
                    && this.SB14Unit9 != Constants.NONE
                    && HasMathTerm(mathTerms, 8, 3))
                {
                    qs.Add(this.SB14Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit9)
                    && this.SB15Unit9 != Constants.NONE
                    && HasMathTerm(mathTerms, 8, 4))
                {
                    qs.Add(this.SB15Amount9);
                }
            }
            if (label == this.SB1Label10)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit10)
                    && this.SB11Unit10 != Constants.NONE
                    && HasMathTerm(mathTerms, 9, 0))
                {
                    qs.Add(this.SB11Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit10)
                    && this.SB12Unit10 != Constants.NONE
                    && HasMathTerm(mathTerms, 9, 1))
                {
                    qs.Add(this.SB12Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit10)
                    && this.SB13Unit10 != Constants.NONE
                    && HasMathTerm(mathTerms, 9, 2))
                {
                    qs.Add(this.SB13Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit10)
                    && this.SB14Unit10 != Constants.NONE
                    && HasMathTerm(mathTerms, 9, 3))
                {
                    qs.Add(this.SB14Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit10)
                    && this.SB15Unit10 != Constants.NONE
                    && HasMathTerm(mathTerms, 9, 4))
                {
                    qs.Add(this.SB15Amount10);
                }
            }
            if (label == this.SB1Label11)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit11)
                    && this.SB11Unit11 != Constants.NONE
                    && HasMathTerm(mathTerms, 10, 0))
                {
                    qs.Add(this.SB11Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit11)
                    && this.SB12Unit11 != Constants.NONE
                    && HasMathTerm(mathTerms, 10, 1))
                {
                    qs.Add(this.SB12Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit11)
                    && this.SB13Unit11 != Constants.NONE
                    && HasMathTerm(mathTerms, 10, 2))
                {
                    qs.Add(this.SB13Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit11)
                    && this.SB14Unit11 != Constants.NONE
                    && HasMathTerm(mathTerms, 10, 3))
                {
                    qs.Add(this.SB14Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit11)
                    && this.SB15Unit11 != Constants.NONE
                    && HasMathTerm(mathTerms, 10, 4))
                {
                    qs.Add(this.SB15Amount11);
                }
            }
            if (label == this.SB1Label12)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit12)
                    && this.SB11Unit12 != Constants.NONE
                    && HasMathTerm(mathTerms, 11, 0))
                {
                    qs.Add(this.SB11Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit12)
                    && this.SB12Unit12 != Constants.NONE
                    && HasMathTerm(mathTerms, 11, 1))
                {
                    qs.Add(this.SB12Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit12)
                    && this.SB13Unit12 != Constants.NONE
                    && HasMathTerm(mathTerms, 11, 2))
                {
                    qs.Add(this.SB13Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit12)
                    && this.SB14Unit12 != Constants.NONE
                    && HasMathTerm(mathTerms, 11, 3))
                {
                    qs.Add(this.SB14Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit12)
                    && this.SB15Unit12 != Constants.NONE
                    && HasMathTerm(mathTerms, 11, 4))
                {
                    qs.Add(this.SB15Amount12);
                }
            }
            if (label == this.SB1Label13)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit13)
                    && this.SB11Unit13 != Constants.NONE
                    && HasMathTerm(mathTerms, 12, 0))
                {
                    qs.Add(this.SB11Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit13)
                    && this.SB12Unit13 != Constants.NONE
                    && HasMathTerm(mathTerms, 12, 1))
                {
                    qs.Add(this.SB12Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit13)
                    && this.SB13Unit13 != Constants.NONE
                    && HasMathTerm(mathTerms, 12, 2))
                {
                    qs.Add(this.SB13Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit13)
                    && this.SB14Unit13 != Constants.NONE
                    && HasMathTerm(mathTerms, 12, 3))
                {
                    qs.Add(this.SB14Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit13)
                    && this.SB15Unit13 != Constants.NONE
                    && HasMathTerm(mathTerms, 12, 4))
                {
                    qs.Add(this.SB15Amount13);
                }
            }
            if (label == this.SB1Label14)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit14)
                    && this.SB11Unit14 != Constants.NONE
                    && HasMathTerm(mathTerms, 13, 0))
                {
                    qs.Add(this.SB11Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit14)
                    && this.SB12Unit14 != Constants.NONE
                    && HasMathTerm(mathTerms, 13, 1))
                {
                    qs.Add(this.SB12Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit14)
                    && this.SB13Unit14 != Constants.NONE
                    && HasMathTerm(mathTerms, 13, 2))
                {
                    qs.Add(this.SB13Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit14)
                    && this.SB14Unit14 != Constants.NONE
                    && HasMathTerm(mathTerms, 13, 3))
                {
                    qs.Add(this.SB14Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit14)
                    && this.SB15Unit14 != Constants.NONE
                    && HasMathTerm(mathTerms, 13, 4))
                {
                    qs.Add(this.SB15Amount14);
                }
            }
            if (label == this.SB1Label15)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit15)
                    && this.SB11Unit15 != Constants.NONE
                    && HasMathTerm(mathTerms, 14, 0))
                {
                    qs.Add(this.SB11Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit15)
                    && this.SB12Unit15 != Constants.NONE
                    && HasMathTerm(mathTerms, 14, 1))
                {
                    qs.Add(this.SB12Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit15)
                    && this.SB13Unit15 != Constants.NONE
                    && HasMathTerm(mathTerms, 14, 2))
                {
                    qs.Add(this.SB13Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit15)
                    && this.SB14Unit15 != Constants.NONE
                    && HasMathTerm(mathTerms, 14, 3))
                {
                    qs.Add(this.SB14Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit15)
                    && this.SB15Unit15 != Constants.NONE
                    && HasMathTerm(mathTerms, 14, 4))
                {
                    qs.Add(this.SB15Amount15);
                }
            }
            if (label == this.SB1Label16)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit16)
                    && this.SB11Unit16 != Constants.NONE
                    && HasMathTerm(mathTerms, 15, 0))
                {
                    qs.Add(this.SB11Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit16)
                    && this.SB12Unit16 != Constants.NONE
                    && HasMathTerm(mathTerms, 15, 1))
                {
                    qs.Add(this.SB12Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit16)
                    && this.SB13Unit16 != Constants.NONE
                    && HasMathTerm(mathTerms, 15, 2))
                {
                    qs.Add(this.SB13Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit16)
                    && this.SB14Unit16 != Constants.NONE
                    && HasMathTerm(mathTerms, 15, 3))
                {
                    qs.Add(this.SB14Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit16)
                    && this.SB15Unit16 != Constants.NONE
                    && HasMathTerm(mathTerms, 15, 4))
                {
                    qs.Add(this.SB15Amount16);
                }
            }
            if (label == this.SB1Label17)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit17)
                    && this.SB11Unit17 != Constants.NONE
                    && HasMathTerm(mathTerms, 16, 0))
                {
                    qs.Add(this.SB11Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit17)
                    && this.SB12Unit17 != Constants.NONE
                    && HasMathTerm(mathTerms, 16, 1))
                {
                    qs.Add(this.SB12Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit17)
                    && this.SB13Unit17 != Constants.NONE
                    && HasMathTerm(mathTerms, 16, 2))
                {
                    qs.Add(this.SB13Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit17)
                    && this.SB14Unit17 != Constants.NONE
                    && HasMathTerm(mathTerms, 16, 3))
                {
                    qs.Add(this.SB14Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit17)
                    && this.SB15Unit17 != Constants.NONE
                    && HasMathTerm(mathTerms, 16, 4))
                {
                    qs.Add(this.SB15Amount17);
                }
            }
            if (label == this.SB1Label18)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit18)
                    && this.SB11Unit18 != Constants.NONE
                    && HasMathTerm(mathTerms, 17, 0))
                {
                    qs.Add(this.SB11Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit18)
                    && this.SB12Unit18 != Constants.NONE
                    && HasMathTerm(mathTerms, 17, 1))
                {
                    qs.Add(this.SB12Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit18)
                    && this.SB13Unit18 != Constants.NONE
                    && HasMathTerm(mathTerms, 17, 2))
                {
                    qs.Add(this.SB13Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit18)
                    && this.SB14Unit18 != Constants.NONE
                    && HasMathTerm(mathTerms, 17, 3))
                {
                    qs.Add(this.SB14Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit18)
                    && this.SB15Unit18 != Constants.NONE
                    && HasMathTerm(mathTerms, 17, 4))
                {
                    qs.Add(this.SB15Amount18);
                }
            }
            if (label == this.SB1Label19)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit19)
                    && this.SB11Unit19 != Constants.NONE
                    && HasMathTerm(mathTerms, 18, 0))
                {
                    qs.Add(this.SB11Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit19)
                    && this.SB12Unit19 != Constants.NONE
                    && HasMathTerm(mathTerms, 18, 1))
                {
                    qs.Add(this.SB12Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit19)
                    && this.SB13Unit19 != Constants.NONE
                    && HasMathTerm(mathTerms, 18, 2))
                {
                    qs.Add(this.SB13Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit19)
                    && this.SB14Unit19 != Constants.NONE
                    && HasMathTerm(mathTerms, 18, 3))
                {
                    qs.Add(this.SB14Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit19)
                    && this.SB15Unit19 != Constants.NONE
                    && HasMathTerm(mathTerms, 18, 4))
                {
                    qs.Add(this.SB15Amount19);
                }
            }
            if (label == this.SB1Label20)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit20)
                    && this.SB11Unit20 != Constants.NONE
                    && HasMathTerm(mathTerms, 19, 0))
                {
                    qs.Add(this.SB11Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit20)
                    && this.SB12Unit20 != Constants.NONE
                    && HasMathTerm(mathTerms, 19, 1))
                {
                    qs.Add(this.SB12Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit20)
                    && this.SB13Unit20 != Constants.NONE
                    && HasMathTerm(mathTerms, 19, 2))
                {
                    qs.Add(this.SB13Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit20)
                    && this.SB14Unit20 != Constants.NONE
                    && HasMathTerm(mathTerms, 19, 3))
                {
                    qs.Add(this.SB14Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit20)
                    && this.SB15Unit20 != Constants.NONE
                    && HasMathTerm(mathTerms, 19, 4))
                {
                    qs.Add(this.SB15Amount20);
                }
            }
            return qs;
        }
       
        
        private List<double> GetQsForLabel(string label)
        {
            List<double> qs = new List<double>();

            if (label == this.SB1Label1)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit1)
                    && this.SB11Unit1 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit1)
                    && this.SB12Unit1 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit1)
                    && this.SB13Unit1 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit1)
                    && this.SB14Unit1 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount1);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit1)
                    && this.SB15Unit1 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount1);
                }
            }
            if (label == this.SB1Label2)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit2)
                    && this.SB11Unit2 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit2)
                    && this.SB12Unit2 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit2)
                    && this.SB13Unit2 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit2)
                    && this.SB14Unit2 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount2);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit2)
                    && this.SB15Unit2 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount2);
                }
            }
            if (label == this.SB1Label3)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit3)
                    && this.SB11Unit3 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit3)
                    && this.SB12Unit3 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit3)
                    && this.SB13Unit3 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit3)
                    && this.SB14Unit3 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount3);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit3)
                    && this.SB15Unit3 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount3);
                }
            }
            if (label == this.SB1Label4)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit4)
                    && this.SB11Unit4 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit4)
                    && this.SB12Unit4 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit4)
                    && this.SB13Unit4 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit4)
                    && this.SB14Unit4 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount4);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit4)
                    && this.SB15Unit4 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount4);
                }
            }
            if (label == this.SB1Label5)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit5)
                    && this.SB11Unit5 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit5)
                    && this.SB12Unit5 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit5)
                    && this.SB13Unit5 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit5)
                    && this.SB14Unit5 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount5);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit5)
                    && this.SB15Unit5 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount5);
                }
            }
            if (label == this.SB1Label6)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit6)
                    && this.SB11Unit6 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit6)
                    && this.SB12Unit6 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit6)
                    && this.SB13Unit6 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit6)
                    && this.SB14Unit6 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount6);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit6)
                    && this.SB15Unit6 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount6);
                }
            }
            if (label == this.SB1Label7)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit7)
                    && this.SB11Unit7 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit7)
                    && this.SB12Unit7 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit7)
                    && this.SB13Unit7 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit7)
                    && this.SB14Unit7 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount7);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit7)
                    && this.SB15Unit7 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount7);
                }
            }
            if (label == this.SB1Label8)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit8)
                    && this.SB11Unit8 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit8)
                    && this.SB12Unit8 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit8)
                    && this.SB13Unit8 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit8)
                    && this.SB14Unit8 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount8);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit8)
                    && this.SB15Unit8 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount8);
                }
            }
            if (label == this.SB1Label9)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit9)
                    && this.SB11Unit9 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit9)
                    && this.SB12Unit9 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit9)
                    && this.SB13Unit9 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit9)
                    && this.SB14Unit9 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount9);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit9)
                    && this.SB15Unit9 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount9);
                }
            }
            if (label == this.SB1Label10)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit10)
                    && this.SB11Unit10 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit10)
                    && this.SB12Unit10 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit10)
                    && this.SB13Unit10 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit10)
                    && this.SB14Unit10 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount10);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit10)
                    && this.SB15Unit10 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount10);
                }
            }
            if (label == this.SB1Label11)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit11)
                    && this.SB11Unit11 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit11)
                    && this.SB12Unit11 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit11)
                    && this.SB13Unit11 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit11)
                    && this.SB14Unit11 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount11);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit11)
                    && this.SB15Unit11 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount11);
                }
            }
            if (label == this.SB1Label12)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit12)
                    && this.SB11Unit12 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit12)
                    && this.SB12Unit12 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit12)
                    && this.SB13Unit12 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit12)
                    && this.SB14Unit12 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount12);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit12)
                    && this.SB15Unit12 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount12);
                }
            }
            if (label == this.SB1Label13)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit13)
                    && this.SB11Unit13 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit13)
                    && this.SB12Unit13 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit13)
                    && this.SB13Unit13 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit13)
                    && this.SB14Unit13 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount13);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit13)
                    && this.SB15Unit13 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount13);
                }
            }
            if (label == this.SB1Label14)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit14)
                    && this.SB11Unit14 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit14)
                    && this.SB12Unit14 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit14)
                    && this.SB13Unit14 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit14)
                    && this.SB14Unit14 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount14);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit14)
                    && this.SB15Unit14 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount14);
                }
            }
            if (label == this.SB1Label15)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit15)
                    && this.SB11Unit15 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit15)
                    && this.SB12Unit15 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit15)
                    && this.SB13Unit15 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit15)
                    && this.SB14Unit15 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount15);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit15)
                    && this.SB15Unit15 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount15);
                }
            }
            if (label == this.SB1Label16)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit16)
                    && this.SB11Unit16 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit16)
                    && this.SB12Unit16 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit16)
                    && this.SB13Unit16 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit16)
                    && this.SB14Unit16 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount16);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit16)
                    && this.SB15Unit16 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount16);
                }
            }
            if (label == this.SB1Label17)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit17)
                    && this.SB11Unit17 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit17)
                    && this.SB12Unit17 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit17)
                    && this.SB13Unit17 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit17)
                    && this.SB14Unit17 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount17);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit17)
                    && this.SB15Unit17 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount17);
                }
            }
            if (label == this.SB1Label18)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit18)
                    && this.SB11Unit18 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit18)
                    && this.SB12Unit18 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit18)
                    && this.SB13Unit18 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit18)
                    && this.SB14Unit18 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount18);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit18)
                    && this.SB15Unit18 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount18);
                }
            }
            if (label == this.SB1Label19)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit19)
                    && this.SB11Unit19 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit19)
                    && this.SB12Unit19 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit19)
                    && this.SB13Unit19 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit19)
                    && this.SB14Unit19 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount19);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit19)
                    && this.SB15Unit19 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount19);
                }
            }
            if (label == this.SB1Label20)
            {
                if (!string.IsNullOrEmpty(this.SB11Unit20)
                    && this.SB11Unit20 != Constants.NONE)
                {
                    qs.Add(this.SB11Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB12Unit20)
                    && this.SB12Unit20 != Constants.NONE)
                {
                    qs.Add(this.SB12Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB13Unit20)
                    && this.SB13Unit20 != Constants.NONE)
                {
                    qs.Add(this.SB13Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB14Unit20)
                    && this.SB14Unit20 != Constants.NONE)
                {
                    qs.Add(this.SB14Amount20);
                }
                if (!string.IsNullOrEmpty(this.SB15Unit20)
                    && this.SB15Unit20 != Constants.NONE)
                {
                    qs.Add(this.SB15Amount20);
                }
            }
            return qs;
        }
        private DevTreks.Extensions.Algorithms.SimulatedAnnealing1 InitSA1Algo(string label)
        {
            DevTreks.Extensions.Algorithms.SimulatedAnnealing1 sa
                    = new Algorithms.SimulatedAnnealing1(0, 0, 0, 0, this.CalcParameters);
            if (label == this.SB1Label1)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount1, this.SB12Amount1,
                   this.SB13Amount1, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label2)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount2, this.SB12Amount2,
                   this.SB13Amount2, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label3)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount3, this.SB12Amount3,
                   this.SB13Amount3, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label4)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount4, this.SB12Amount4,
                   this.SB13Amount4, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label5)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount5, this.SB12Amount5,
                   this.SB13Amount5, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label6)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount6, this.SB12Amount6,
                   this.SB13Amount6, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label7)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount7, this.SB12Amount7,
                   this.SB13Amount7, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label8)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount8, this.SB12Amount8,
                   this.SB13Amount8, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label9)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount9, this.SB12Amount9,
                   this.SB13Amount9, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label10)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount10, this.SB12Amount10,
                   this.SB13Amount10, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label11)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount11, this.SB12Amount11,
                   this.SB13Amount11, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label12)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount12, this.SB12Amount12,
                   this.SB13Amount12, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label13)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount13, this.SB12Amount13,
                   this.SB13Amount13, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label14)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount14, this.SB12Amount14,
                   this.SB13Amount14, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label15)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount15, this.SB12Amount15,
                   this.SB13Amount15, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label16)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount16, this.SB12Amount16,
                   this.SB13Amount16, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label17)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount17, this.SB12Amount17,
                   this.SB13Amount17, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label18)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount18, this.SB12Amount18,
                   this.SB13Amount18, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label19)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount19, this.SB12Amount19,
                   this.SB13Amount19, this.SB1Iterations, this.CalcParameters);
            }
            else if (label == this.SB1Label20)
            {
                sa = new Algorithms.SimulatedAnnealing1(this.SB11Amount20, this.SB12Amount20,
                   this.SB13Amount20, this.SB1Iterations, this.CalcParameters);
            }
            else
            {
            }
            return sa;
        }
        private void SetSA1AlgoRanges(string label, DevTreks.Extensions.Algorithms.SimulatedAnnealing1 sa)
        {
            string[] colNames = new List<string>().ToArray();
            List<double> qTs = new List<double>();
            if (label == this.SB1Label1)
            {
                this.SB1TAmount1 = sa.BestEnergy;
                this.SB1TMAmount1 = sa.BestEnergy;
                //regular high and low estimation
                SetPRAIndicatorStats(label, colNames, qTs);
                //SetTotalRange1();
                //no condition on type of result yet KISS for now
                this.SB1MathResult1 += sa.MathResult;
            }
            else if (label == this.SB1Label2)
            {
                this.SB1TAmount2 = sa.BestEnergy;
                this.SB1TMAmount2 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult2 += sa.MathResult;
            }
            else if (label == this.SB1Label3)
            {
                this.SB1TAmount3 = sa.BestEnergy;
                this.SB1TMAmount3 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult3 += sa.MathResult;
            }
            else if (label == this.SB1Label4)
            {
                this.SB1TAmount4 = sa.BestEnergy;
                this.SB1TMAmount4 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult4 += sa.MathResult;
            }
            else if (label == this.SB1Label5)
            {
                this.SB1TAmount5 = sa.BestEnergy;
                this.SB1TMAmount5 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult5 += sa.MathResult;
            }
            else if (label == this.SB1Label6)
            {
                this.SB1TAmount6 = sa.BestEnergy;
                this.SB1TMAmount6 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult6 += sa.MathResult;
            }
            else if (label == this.SB1Label7)
            {
                this.SB1TAmount7 = sa.BestEnergy;
                this.SB1TMAmount7 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult7 += sa.MathResult;
            }
            else if (label == this.SB1Label8)
            {
                this.SB1TAmount8 = sa.BestEnergy;
                this.SB1TMAmount8 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult8 += sa.MathResult;
            }
            else if (label == this.SB1Label9)
            {
                this.SB1TAmount9 = sa.BestEnergy;
                this.SB1TMAmount9 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult9 += sa.MathResult;
            }
            else if (label == this.SB1Label10)
            {
                this.SB1TAmount10 = sa.BestEnergy;
                this.SB1TMAmount10 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult10 += sa.MathResult;
            }
            else if (label == this.SB1Label11)
            {
                this.SB1TAmount11 = sa.BestEnergy;
                this.SB1TMAmount11 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult11 += sa.MathResult;
            }
            else if (label == this.SB1Label12)
            {
                this.SB1TAmount12 = sa.BestEnergy;
                this.SB1TMAmount12 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult12 += sa.MathResult;
            }
            else if (label == this.SB1Label13)
            {
                this.SB1TAmount13 = sa.BestEnergy;
                this.SB1TMAmount13 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult13 += sa.MathResult;
            }
            else if (label == this.SB1Label14)
            {
                this.SB1TAmount14 = sa.BestEnergy;
                this.SB1TMAmount14 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult14 += sa.MathResult;
            }
            else if (label == this.SB1Label15)
            {
                this.SB1TAmount15 = sa.BestEnergy;
                this.SB1TMAmount15 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult15 += sa.MathResult;
            }
            else if (label == this.SB1Label16)
            {
                this.SB1TAmount16 = sa.BestEnergy;
                this.SB1TMAmount16 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult16 += sa.MathResult;
            }
            else if (label == this.SB1Label17)
            {
                this.SB1TAmount17 = sa.BestEnergy;
                this.SB1TMAmount17 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult17 += sa.MathResult;
            }
            else if (label == this.SB1Label18)
            {
                this.SB1TAmount18 = sa.BestEnergy;
                this.SB1TMAmount18 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult18 += sa.MathResult;
            }
            else if (label == this.SB1Label19)
            {
                this.SB1TAmount19 = sa.BestEnergy;
                this.SB1TMAmount19 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult19 += sa.MathResult;
            }
            else if (label == this.SB1Label20)
            {
                this.SB1TAmount20 = sa.BestEnergy;
                this.SB1TMAmount20 = sa.BestEnergy;
                SetPRAIndicatorStats(label, colNames, qTs);
                this.SB1MathResult20 += sa.MathResult;
            }
            else
            {
                //ignore the row
            }
        }
        private DevTreks.Extensions.Algorithms.NeuralNetwork1 InitNN1Algo(string label)
        {
            List<double> qs = GetQsForLabel(label);
            DevTreks.Extensions.Algorithms.NeuralNetwork1 nn
                    = new Algorithms.NeuralNetwork1(qs, this.SB1Iterations, this.CalcParameters);
            return nn;
        }
        private void SetNN1AlgoRanges(string label, DevTreks.Extensions.Algorithms.NeuralNetwork1 nn)
        {
            int iIndicator = GetNN1Index(label);
            string[] colNames = new List<string>().ToArray();
            List<double> qTs = new List<double>();
            if (iIndicator == 1)
            {
                this.SB1TAmount1 = nn.QTPredicted;
                this.SB1TMAmount1 = nn.QTPredicted;
                this.SB1TLAmount1 = nn.QTL;
                this.SB1TLUnit1 = nn.QTLUnit;
                if (this.SB1Type1 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type1))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                    //SetTotalRange1();
                }
                //no condition on type of result yet KISS for now
                this.SB1MathResult1 += nn.MathResult;
            }
            else if (iIndicator == 2)
            {
                this.SB1TAmount2 = nn.QTPredicted;
                this.SB1TMAmount2 = nn.QTPredicted;
                this.SB1TLAmount2 = nn.QTL;
                this.SB1TLUnit2 = nn.QTLUnit;
                if (this.SB1Type2 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type2))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult2 += nn.MathResult;
            }
            else if (iIndicator == 3)
            {
                this.SB1TAmount3 = nn.QTPredicted;
                this.SB1TMAmount3 = nn.QTPredicted;
                this.SB1TLAmount3 = nn.QTL;
                this.SB1TLUnit3 = nn.QTLUnit;
                if (this.SB1Type3 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type3))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult3 += nn.MathResult;
            }
            else if (iIndicator == 4)
            {
                this.SB1TAmount4 = nn.QTPredicted;
                this.SB1TMAmount4 = nn.QTPredicted;
                this.SB1TLAmount4 = nn.QTL;
                this.SB1TLUnit4 = nn.QTLUnit;
                if (this.SB1Type4 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type4))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult4 += nn.MathResult;
            }
            else if (iIndicator == 5)
            {
                this.SB1TAmount5 = nn.QTPredicted;
                this.SB1TMAmount5 = nn.QTPredicted;
                this.SB1TLAmount5 = nn.QTL;
                this.SB1TLUnit5 = nn.QTLUnit;
                if (this.SB1Type5 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type5))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult5 += nn.MathResult;
            }
            else if (iIndicator == 6)
            {
                this.SB1TAmount6 = nn.QTPredicted;
                this.SB1TMAmount6 = nn.QTPredicted;
                this.SB1TLAmount6 = nn.QTL;
                this.SB1TLUnit6 = nn.QTLUnit;
                if (this.SB1Type6 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type6))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult6 += nn.MathResult;
            }
            else if (iIndicator == 7)
            {
                this.SB1TAmount7 = nn.QTPredicted;
                this.SB1TMAmount7 = nn.QTPredicted;
                this.SB1TLAmount7 = nn.QTL;
                this.SB1TLUnit7 = nn.QTLUnit;
                if (this.SB1Type7 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type7))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult7 += nn.MathResult;
            }
            else if (iIndicator == 8)
            {
                this.SB1TAmount8 = nn.QTPredicted;
                this.SB1TMAmount8 = nn.QTPredicted;
                this.SB1TLAmount8 = nn.QTL;
                this.SB1TLUnit8 = nn.QTLUnit;
                if (this.SB1Type8 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type8))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult8 += nn.MathResult;
            }
            else if (iIndicator == 9)
            {
                this.SB1TAmount9 = nn.QTPredicted;
                this.SB1TMAmount9 = nn.QTPredicted;
                this.SB1TLAmount9 = nn.QTL;
                this.SB1TLUnit9 = nn.QTLUnit;
                if (this.SB1Type9 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type9))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult9 += nn.MathResult;
            }
            else if (iIndicator == 10)
            {
                this.SB1TAmount10 = nn.QTPredicted;
                this.SB1TMAmount10 = nn.QTPredicted;
                this.SB1TLAmount10 = nn.QTL;
                this.SB1TLUnit10 = nn.QTLUnit;
                if (this.SB1Type10 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type10))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult10 += nn.MathResult;
            }
            else if (iIndicator == 11)
            {
                this.SB1TAmount11 = nn.QTPredicted;
                this.SB1TMAmount11 = nn.QTPredicted;
                this.SB1TLAmount11 = nn.QTL;
                this.SB1TLUnit11 = nn.QTLUnit;
                if (this.SB1Type11 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type11))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult11 += nn.MathResult;
            }
            else if (iIndicator == 12)
            {
                this.SB1TAmount12 = nn.QTPredicted;
                this.SB1TMAmount12 = nn.QTPredicted;
                this.SB1TLAmount12 = nn.QTL;
                this.SB1TLUnit12 = nn.QTLUnit;
                if (this.SB1Type12 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type12))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult12 += nn.MathResult;
            }
            else if (iIndicator == 13)
            {
                this.SB1TAmount13 = nn.QTPredicted;
                this.SB1TMAmount13 = nn.QTPredicted;
                this.SB1TLAmount13 = nn.QTL;
                this.SB1TLUnit13 = nn.QTLUnit;
                if (this.SB1Type13 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type13))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult13 += nn.MathResult;
            }
            else if (iIndicator == 14)
            {
                this.SB1TAmount14 = nn.QTPredicted;
                this.SB1TMAmount14 = nn.QTPredicted;
                this.SB1TLAmount14 = nn.QTL;
                this.SB1TLUnit14 = nn.QTLUnit;
                if (this.SB1Type14 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type14))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult14 += nn.MathResult;
            }
            else if (iIndicator == 15)
            {
                this.SB1TAmount15 = nn.QTPredicted;
                this.SB1TMAmount15 = nn.QTPredicted;
                this.SB1TLAmount15 = nn.QTL;
                this.SB1TLUnit15 = nn.QTLUnit;
                if (this.SB1Type15 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type15))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult15 += nn.MathResult;
            }
            else if (iIndicator == 16)
            {
                this.SB1TAmount16 = nn.QTPredicted;
                this.SB1TMAmount16 = nn.QTPredicted;
                this.SB1TLAmount16 = nn.QTL;
                this.SB1TLUnit16 = nn.QTLUnit;
                if (this.SB1Type16 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type16))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult16 += nn.MathResult;
            }
            else if (iIndicator == 17)
            {
                this.SB1TAmount17 = nn.QTPredicted;
                this.SB1TMAmount17 = nn.QTPredicted;
                this.SB1TLAmount17 = nn.QTL;
                this.SB1TLUnit17 = nn.QTLUnit;
                if (this.SB1Type17 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type17))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult17 += nn.MathResult;
            }
            else if (iIndicator == 18)
            {
                this.SB1TAmount18 = nn.QTPredicted;
                this.SB1TMAmount18 = nn.QTPredicted;
                this.SB1TLAmount18 = nn.QTL;
                this.SB1TLUnit18 = nn.QTLUnit;
                if (this.SB1Type18 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type18))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult18 += nn.MathResult;
            }
            else if (iIndicator == 19)
            {
                this.SB1TAmount19 = nn.QTPredicted;
                this.SB1TMAmount19 = nn.QTPredicted;
                this.SB1TLAmount19 = nn.QTL;
                this.SB1TLUnit19 = nn.QTLUnit;
                if (this.SB1Type19 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type19))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult19 += nn.MathResult;
            }
            else if (iIndicator == 20)
            {
                this.SB1TAmount20 = nn.QTPredicted;
                this.SB1TMAmount20 = nn.QTPredicted;
                this.SB1TLAmount20 = nn.QTL;
                this.SB1TLUnit20 = nn.QTLUnit;
                if (this.SB1Type20 != Calculator1.RUC_TYPES.none.ToString()
                    && !string.IsNullOrEmpty(this.SB1Type20))
                {
                    //regular high and low estimation
                    SetPRAIndicatorStats(label, colNames, qTs);
                }
                this.SB1MathResult20 += nn.MathResult;
            }
            else
            {
                //ignore the row
            }
        }
        private int GetNN1Index(string label)
        {
            //one based so that if zero returned ignore the label
            int iLastIndicatorOneBased = 0;
            if (label == this.SB1Label1)
            {
                iLastIndicatorOneBased = 1;
            }
            if (label == this.SB1Label2)
            {
                iLastIndicatorOneBased = 2;
            }
            if (label == this.SB1Label3)
            {
                iLastIndicatorOneBased = 3;
            }
            if (label == this.SB1Label4)
            {
                iLastIndicatorOneBased = 4;
            }
            if (label == this.SB1Label5)
            {
                iLastIndicatorOneBased = 5;
            }
            if (label == this.SB1Label6)
            {
                iLastIndicatorOneBased = 6;
            }
            if (label == this.SB1Label7)
            {
                iLastIndicatorOneBased = 7;
            }
            if (label == this.SB1Label8)
            {
                iLastIndicatorOneBased = 8;
            }
            if (label == this.SB1Label9)
            {
                iLastIndicatorOneBased = 9;
            }
            if (label == this.SB1Label10)
            {
                iLastIndicatorOneBased = 10;
            }
            if (label == this.SB1Label11)
            {
                iLastIndicatorOneBased = 11;
            }
            if (label == this.SB1Label12)
            {
                iLastIndicatorOneBased = 12;
            }
            if (label == this.SB1Label13)
            {
                iLastIndicatorOneBased = 13;
            }
            if (label == this.SB1Label14)
            {
                iLastIndicatorOneBased = 14;
            }
            if (label == this.SB1Label15)
            {
                iLastIndicatorOneBased = 15;
            }
            if (label == this.SB1Label16)
            {
                iLastIndicatorOneBased = 16;
            }
            if (label == this.SB1Label17)
            {
                iLastIndicatorOneBased = 17;
            }
            if (label == this.SB1Label18)
            {
                iLastIndicatorOneBased = 18;
            }
            if (label == this.SB1Label19)
            {
                iLastIndicatorOneBased = 19;
            }
            if (label == this.SB1Label20)
            {
                iLastIndicatorOneBased = 20;
            }
            return iLastIndicatorOneBased;
        }
        
    }
}
