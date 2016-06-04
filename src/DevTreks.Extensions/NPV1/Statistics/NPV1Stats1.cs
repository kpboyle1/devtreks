using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Typical Object model: 
    ///             NPV1Stock.Stat1
    ///             The class statistically analyzes npvs.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///NOTE:        This classes uses a small subset of the Statistic01
    ///</summary>
    public class NPV1Stat1 : CostBenefitStatistic01
    {
        //calls the base-class version, and initializes the base class properties.
        public NPV1Stat1()
            : base()
        {
            //subprice object
            InitTotalNPV1Stat1Properties(this);
        }
        //copy constructor
        public NPV1Stat1(NPV1Stat1 calculator)
            : base(calculator)
        {
            CopyTotalNPV1Stat1Properties(this, calculator);
        }

        public CalculatorParameters CalcParameters { get; set; }
        //number of cost observations
        public double TotalCostN { get; set; }
        private const string TCostN = "TCostN";
        //number of benefit observations
        public double TotalBenefitN { get; set; }
        private const string TBenefitN = "TBenefitN";

        public void InitTotalNPV1Stat1Properties(NPV1Stat1 ind)
        {
            ind.ErrorMessage = string.Empty;
            //includes summary data
            ind.InitTotalBenefitsProperties();
            ind.InitTotalCostsProperties();

            ind.InitMeanBenefitsProperties();
            ind.InitMeanCostsProperties();
            ind.InitMedianBenefitsProperties();
            ind.InitMedianCostsProperties();
            ind.InitStdDevBenefitsProperties();
            ind.InitStdDevCostsProperties();
            ind.InitVarianceBenefitsProperties();
            ind.InitVarianceCostsProperties();

            ind.TotalCostN = 0;
            ind.TotalBenefitN = 0;
            ind.CalcParameters = new CalculatorParameters();
        }

        public void CopyTotalNPV1Stat1Properties(NPV1Stat1 ind,
            NPV1Stat1 calculator)
        {
            if (calculator != null)
            {
                //inits with standard cb totals
                ind.CopyCalculatorProperties(calculator);
                ind.CopyTotalBenefitsProperties(calculator);
                ind.CopyTotalCostsProperties(calculator);

                ind.ErrorMessage = calculator.ErrorMessage;
                ind.TotalCostN = calculator.TotalCostN;
                ind.TotalBenefitN = calculator.TotalBenefitN;

                ind.CopyMeanBenefitsProperties(calculator);
                ind.CopyMeanCostsProperties(calculator);
                ind.CopyMedianBenefitsProperties(calculator);
                ind.CopyMedianCostsProperties(calculator);
                ind.CopyVarianceBenefitsProperties(calculator);
                ind.CopyVarianceCostsProperties(calculator);
                ind.CopyStdDevBenefitsProperties(calculator);
                ind.CopyStdDevCostsProperties(calculator);
                ind.CopyMeanBenefitsProperties(calculator);
                ind.CopyMeanCostsProperties(calculator);

                if (calculator.CalcParameters == null)
                    calculator.CalcParameters = new CalculatorParameters();
                if (ind.CalcParameters == null)
                    ind.CalcParameters = new CalculatorParameters();
                ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            }
        }
        public void CopyTotalNPV1Stat1RProperties(NPV1Stat1 calculator)
        {
            if (calculator != null)
            {
                this.CopyTotalBenefitsPsandQsProperties(calculator);
            }
        }
        public void SetTotalNPV1Stat1Properties(NPV1Stat1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.SetTotalBenefitsSummaryProperties(attNameExtension, calculator);
            ind.SetTotalCostsSummaryProperties(attNameExtension, calculator);
            ind.SetMeanBenefitsProperties(calculator);
            ind.SetMeanCostsProperties(calculator);
            ind.SetMedianBenefitsProperties(calculator);
            ind.SetMedianCostsProperties(calculator);
            ind.SetVarianceBenefitsProperties(calculator);
            ind.SetVarianceCostsProperties(calculator);
            ind.SetStdDevBenefitsProperties(calculator);
            ind.SetStdDevCostsProperties(calculator);

            ind.TotalCostN = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TCostN, attNameExtension));
            ind.TotalBenefitN = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TBenefitN, attNameExtension));
        }
        
        public void SetTotalNPV1Stat1Property(NPV1Stat1 ind,
            string attName, string attValue)
        {
            ind.SetMeanBenefitsProperties(attName, attValue);
            ind.SetMeanCostsProperties(attName, attValue);
            ind.SetMedianBenefitsProperties(attName, attValue);
            ind.SetMedianCostsProperties(attName, attValue);
            ind.SetVarianceBenefitsProperties(attName, attValue);
            ind.SetVarianceCostsProperties(attName, attValue);
            ind.SetStdDevBenefitsProperties(attName, attValue);
            ind.SetStdDevCostsProperties(attName, attValue);
            switch (attName)
            {
                case TCostN:
                    ind.TotalCostN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TBenefitN:
                    ind.TotalBenefitN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalNPV1Stat1Property(NPV1Stat1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            sPropertyValue = ind.GetMeanBenefitsProperty(attName);
            sPropertyValue = ind.GetMeanCostsProperty(attName);
            sPropertyValue = ind.GetMedianBenefitsProperty(attName);
            sPropertyValue = ind.GetMedianCostsProperty(attName);
            sPropertyValue = ind.GetVarianceBenefitsProperty(attName);
            sPropertyValue = ind.GetVarianceCostsProperty(attName);
            sPropertyValue = ind.GetStdDevBenefitsProperty(attName);
            sPropertyValue = ind.GetStdDevCostsProperty(attName);
            switch (attName)
            {
                case TCostN:
                    sPropertyValue = ind.TotalCostN.ToString();
                    break;
                case TBenefitN:
                    sPropertyValue = ind.TotalBenefitN.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public void SetTotalNPV1Stat1Attributes(NPV1Stat1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(TCostN, attNameExtension), ind.TotalCostN.ToString("N2", CultureInfo.InvariantCulture));
                if (this.CalcParameters.CurrentElementNodeName.Contains(Input.INPUT_PRICE_TYPES.input.ToString()))
                {
                    //tamincent will be extra in prices
                    ind.SetAmortCosts2Attributes(attNameExtension, ref writer);
                    if (this.CalcParameters.SubApplicationType != Constants.SUBAPPLICATION_TYPES.inputprices)
                    {
                        //no stats for tech inputs, but show totals
                        ind.SetTotalCostsPsandQsAttributes(attNameExtension, ref writer);
                    }
                }
                else
                {
                    ind.SetAmortCosts2Attributes(attNameExtension, ref writer);
                    if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets
                        || this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        ind.SetAmortNets2Attributes(attNameExtension, ref writer);
                    }
                }
            }
            if (bIsBenefitNode || bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(TBenefitN, attNameExtension), ind.TotalBenefitN.ToString("N2", CultureInfo.InvariantCulture));
                //a few extra atts for prices
                ind.SetAmortBenefits2Attributes(attNameExtension, ref writer);
                ind.SetAmortBenefits2PsandQsAttributes(attNameExtension, ref writer);
            }
        }
        //run the analyses for inputs an outputs
        public bool RunAnalyses(NPV1Stock npv1Stock)
        {
            //only used for base input and output analysis
            //the base inputs and outputs were copied to calculators
            //do not use for op.inputs, outcome.outputs
            bool bHasAnalyses = false;
            //set npv1Stock.Total1
            //bHasAnalyses = SetBaseIOAnalyses(npv1Stock);
            return bHasAnalyses;
        }
        //calcs holds the collections needing statistical analysis
        public bool RunAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to npv1stock.Stat1
            if (npv1Stock.Stat1 == null)
            {
                npv1Stock.Stat1 = new NPV1Stat1();
            }
            //don't use npv1Stock totals, use calcs
            SetAnalyses(npv1Stock);
            bHasAnalyses = SetBaseAnalyses(npv1Stock, calcs);
            return bHasAnalyses;
        }
        private bool SetAnalyses(NPV1Stock npv1Stock)
        {
            bool bHasAnalysis = false;
            npv1Stock.Stat1.CalcParameters = npv1Stock.CalcParameters;
            //totals were added to npv1stock, but those totals result 
            //in double counting when calcs are being summed
            //set them to zero
            npv1Stock.Stat1.InitTotalBenefitsProperties();
            npv1Stock.Stat1.InitTotalCostsProperties();
            npv1Stock.Stat1.TotalRAmount = 0;
            //times is already in comp amount
            npv1Stock.Stat1.TotalRCompositionAmount = 0;
            npv1Stock.Stat1.TotalRPrice = 0;
            bHasAnalysis = true;
            return bHasAnalysis;
        }
        
        private bool SetBaseAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iCostN2 = 0;
            int iBenefitN2 = 0;
            List<NPV1Stat1> stats2 = new List<NPV1Stat1>();
            double dbMultiplier = 1;
            //this is the IO pattern: test if the Alt2 Pattern is still needed
            //make sure this does not need  IEnumerable<System.Linq.IGrouping<int, Calculator1>>
            //calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(npv1Stock.GetType()))
                {
                    NPV1Stock stock = (NPV1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Stat1 != null)
                        {
                            //set the calc totals in each observation
                            NPV1Stock observation2Stock = new NPV1Stock(stock.Stat1.CalcParameters, stock.Stat1.CalcParameters.AnalyzerParms.AnalyzerType);
                            observation2Stock.Stat1 = new NPV1Stat1();
                            observation2Stock.Stat1.CalcParameters = new CalculatorParameters(stock.Stat1.CalcParameters);
                            //since Stat1 is new, this method will not accrue totals
                            //but it will set N
                            //calc.Multiplier not used because base calcs used it
                            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Stat1,
                                    dbMultiplier, stock.Stat1);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                stats2.Add(observation2Stock.Stat1);
                                bool bIsCostNode = CalculatorHelpers.IsCostNode(stock.CalcParameters.CurrentElementNodeName);
                                bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(stock.CalcParameters.CurrentElementNodeName);
                                if (bIsCostNode)
                                {
                                    iCostN2++;
                                }
                                else if (bIsBenefitNode)
                                {
                                    iBenefitN2++;
                                }
                                else
                                {
                                    iCostN2++;
                                    iBenefitN2++;
                                }
                            }
                        }
                    }
                }
            }
            if (iCostN2 > 0 || iBenefitN2 > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats2, npv1Stock, iCostN2, iBenefitN2);
            }
            return bHasAnalysis;
        }
        
        public bool AddSubTotalToTotalStock(NPV1Stat1 totStock, double multiplier,
            NPV1Stat1 subTotal)
        {
            bool bHasCalculations = false;
            //all initial totals are added to calculator.Stat1
            if (subTotal != null)
            {
                totStock.TotalAMOC += subTotal.TotalAMOC * multiplier;
                totStock.TotalAMAOH += subTotal.TotalAMAOH * multiplier;
                totStock.TotalAMCAP += subTotal.TotalAMCAP * multiplier;
                totStock.TotalAMINCENT += subTotal.TotalAMINCENT * multiplier;
                totStock.TotalAMTOTAL += subTotal.TotalAMTOTAL * multiplier;
                //benefits
                totStock.TotalAMR += subTotal.TotalAMR * multiplier;
                totStock.TotalAMRINCENT += subTotal.TotalAMRINCENT * multiplier;
                //nets
                totStock.TotalAMNET = totStock.TotalAMR - totStock.TotalAMTOTAL;
                totStock.TotalAMINCENT_NET = totStock.TotalAMRINCENT - totStock.TotalAMINCENT;
                //r ps and qs
                totStock.TotalRAmount += subTotal.TotalRAmount * multiplier;
                totStock.TotalRCompositionAmount += subTotal.TotalRCompositionAmount * multiplier;
                //don't adjust prices by multiplier
                totStock.TotalRPrice += subTotal.TotalRPrice;
                //display the r (ancestors of outs put name in first calc)
                if (!string.IsNullOrEmpty(subTotal.TotalRName))
                {
                    totStock.TotalRName = subTotal.TotalRName;
                    totStock.TotalRUnit = subTotal.TotalRUnit;
                    totStock.TotalRCompositionUnit = subTotal.TotalRCompositionUnit;
                }
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        private bool SetStatsAnalysis(List<NPV1Stat1> stats2, NPV1Stock statStock, 
            int costN, int benN)
        {
            bool bHasTotals = false;
            //set the total observations total
            foreach (var stat in stats2)
            {
                //add each stat to statStock.Stat1
                bHasTotals = AddSubTotalToTotalStock(statStock.Stat1, 1, stat);
            }
            if (costN > 0)
            {
                statStock.Stat1.TotalCostN = costN;
                //set the cost means
                statStock.Stat1.TotalAMOC_MEAN = statStock.Stat1.TotalAMOC / costN;
                statStock.Stat1.TotalAMAOH_MEAN = statStock.Stat1.TotalAMAOH / costN;
                statStock.Stat1.TotalAMCAP_MEAN = statStock.Stat1.TotalAMCAP / costN;
                statStock.Stat1.TotalAMINCENT_MEAN = statStock.Stat1.TotalAMINCENT / costN;
                statStock.Stat1.TotalAMTOTAL_MEAN = statStock.Stat1.TotalAMTOTAL / costN;
                statStock.Stat1.TotalAMNET_MEAN = statStock.Stat1.TotalAMNET / costN;
                //set the median, variance, and standard deviation costs
                SetOCStatistics(statStock, stats2);
                SetAOHStatistics(statStock, stats2);
                SetCAPStatistics(statStock, stats2);
                SetINCENTStatistics(statStock, stats2);
                //total costs
                SetTOTALStatistics(statStock, stats2);
                //net returns (benefits have to be set first)
                SetNETStatistics(statStock, stats2);
            }
            if (benN > 0)
            {
                statStock.Stat1.TotalBenefitN = benN;
                //set the benefit means
                statStock.Stat1.TotalAMR_MEAN = statStock.Stat1.TotalAMR / benN;
                statStock.Stat1.TotalAMRINCENT_MEAN = statStock.Stat1.TotalAMRINCENT / benN;
                statStock.Stat1.TotalAMINCENT_NET_MEAN = statStock.Stat1.TotalAMINCENT_NET / benN;
                //r ps and qs
                statStock.Stat1.TotalRAmount_MEAN = statStock.Stat1.TotalRAmount / benN;
                statStock.Stat1.TotalRCompositionAmount_MEAN = statStock.Stat1.TotalRCompositionAmount / benN;
                //don't adjust prices by multiplier
                statStock.Stat1.TotalRPrice_MEAN = statStock.Stat1.TotalRPrice / benN;
                //benefits
                SetRStatistics(statStock, stats2);
                SetRINCENTStatistics(statStock, stats2);
                SetINCENT_NETStatistics(statStock, stats2);
                //ps and qs
                SetRAmountStatistics(statStock, stats2);
                SetRPriceStatistics(statStock, stats2);
                SetRCompositionAmountStatistics(statStock, stats2);
            }
            return bHasTotals;
        }

        private static void SetOCStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMOC);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMOC- npv1Stock.Stat1.TotalAMOC_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMOC_MED = (stat.TotalAMOC+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMOC_MED = stat.TotalAMOC;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMOC;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMOC_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMOC_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMOC_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMOC_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMOC_VAR2 = 0;
                npv1Stock.Stat1.TotalAMOC_SD = 0;
            }
        }
        private static void SetAOHStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMAOH);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMAOH- npv1Stock.Stat1.TotalAMAOH_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMAOH_MED = (stat.TotalAMAOH+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMAOH_MED = stat.TotalAMAOH;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMAOH;
            }

            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMAOH_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMAOH_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMAOH_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMAOH_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMAOH_VAR2 = 0;
                npv1Stock.Stat1.TotalAMAOH_SD = 0;
            }
        }
        private static void SetCAPStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMCAP);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMCAP- npv1Stock.Stat1.TotalAMCAP_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMCAP_MED = (stat.TotalAMCAP+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMCAP_MED = stat.TotalAMCAP;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMCAP;
            }

            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMCAP_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMCAP_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMCAP_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMCAP_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMCAP_VAR2 = 0;
                npv1Stock.Stat1.TotalAMCAP_SD = 0;
            }
        }
        private static void SetINCENTStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMINCENT);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMINCENT- npv1Stock.Stat1.TotalAMINCENT_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMINCENT_MED = (stat.TotalAMINCENT+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMINCENT_MED = stat.TotalAMINCENT;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMINCENT;
            }

            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMINCENT_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMINCENT_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMINCENT_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMINCENT_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMINCENT_VAR2 = 0;
                npv1Stock.Stat1.TotalAMINCENT_SD = 0;
            }
        }
        private static void SetTOTALStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMTOTAL);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMTOTAL- npv1Stock.Stat1.TotalAMTOTAL_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMTOTAL_MED = (stat.TotalAMTOTAL+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMTOTAL_MED = stat.TotalAMTOTAL;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMTOTAL;
            }

            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMTOTAL_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMTOTAL_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMTOTAL_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMTOTAL_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMTOTAL_VAR2 = 0;
                npv1Stock.Stat1.TotalAMTOTAL_SD = 0;
            }
        }
        private static void SetNETStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMNET);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMNET- npv1Stock.Stat1.TotalAMNET_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMNET_MED = (stat.TotalAMNET+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMNET_MED = stat.TotalAMNET;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMNET;
            }

            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMNET_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMNET_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMNET_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMNET_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMNET_VAR2 = 0;
                npv1Stock.Stat1.TotalAMNET_SD = 0;
            }
        }
        private static void SetRStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMR);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMR- npv1Stock.Stat1.TotalAMR_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMR_MED = (stat.TotalAMR+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMR_MED = stat.TotalAMR;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMR;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalBenefitN - 1));
                npv1Stock.Stat1.TotalAMR_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMR_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMR_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMR_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMR_VAR2 = 0;
                npv1Stock.Stat1.TotalAMR_SD = 0;
            }
        }
        
        private static void SetRINCENTStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMRINCENT);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMRINCENT- npv1Stock.Stat1.TotalAMRINCENT_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMRINCENT_MED = (stat.TotalAMRINCENT+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMRINCENT_MED = stat.TotalAMRINCENT;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMRINCENT;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalBenefitN - 1));
                npv1Stock.Stat1.TotalAMRINCENT_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMRINCENT_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMRINCENT_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMRINCENT_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMRINCENT_VAR2 = 0;
                npv1Stock.Stat1.TotalAMRINCENT_SD = 0;
            }
        }
        private static void SetINCENT_NETStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAMINCENT_NET);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAMINCENT_NET- npv1Stock.Stat1.TotalAMINCENT_NET_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalAMINCENT_NET_MED = (stat.TotalAMINCENT_NET+ dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalAMINCENT_NET_MED = stat.TotalAMINCENT_NET;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAMINCENT_NET;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalCostN - 1));
                npv1Stock.Stat1.TotalAMINCENT_NET_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalAMINCENT_NET_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalAMINCENT_NET_SD = Math.Sqrt(npv1Stock.Stat1.TotalAMINCENT_NET_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalAMINCENT_NET_VAR2 = 0;
                npv1Stock.Stat1.TotalAMINCENT_NET_SD = 0;
            }
        }
        private static void SetRAmountStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalRAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalRAmount - npv1Stock.Stat1.TotalRAmount_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalRAmount_MED = (stat.TotalRAmount + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalRAmount_MED = stat.TotalRAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalRAmount;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalBenefitN - 1));
                npv1Stock.Stat1.TotalRAmount_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalRAmount_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalRAmount_SD = Math.Sqrt(npv1Stock.Stat1.TotalRAmount_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalRAmount_VAR2 = 0;
                npv1Stock.Stat1.TotalRAmount_SD = 0;
            }
        }
        private static void SetRPriceStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalRPrice);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalRPrice - npv1Stock.Stat1.TotalRPrice_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalRPrice_MED = (stat.TotalRPrice + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalRPrice_MED = stat.TotalRPrice;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalRPrice;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalBenefitN - 1));
                npv1Stock.Stat1.TotalRPrice_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalRPrice_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalRPrice_SD = Math.Sqrt(npv1Stock.Stat1.TotalRPrice_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalRPrice_VAR2 = 0;
                npv1Stock.Stat1.TotalRPrice_SD = 0;
            }
        }
        private static void SetRCompositionAmountStatistics(NPV1Stock npv1Stock, List<NPV1Stat1> stats)
        {
            //reorder for median
            IEnumerable<NPV1Stat1> stat2s = stats.OrderByDescending(s => s.TotalRCompositionAmount);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double db_MEDQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (NPV1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalRCompositionAmount - npv1Stock.Stat1.TotalRCompositionAmount_MEAN), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > db_MEDQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        npv1Stock.Stat1.TotalRCompositionAmount_MED = (stat.TotalRCompositionAmount + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        npv1Stock.Stat1.TotalRCompositionAmount_MED = stat.TotalRCompositionAmount;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalRCompositionAmount;
            }
            //don't divide by zero
            if (npv1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (npv1Stock.Stat1.TotalBenefitN - 1));
                npv1Stock.Stat1.TotalRCompositionAmount_VAR2 = dbMemberSquaredTotalQ1 * dbCount;
                if (npv1Stock.Stat1.TotalRCompositionAmount_MEAN != 0)
                {
                    //sample standard deviation
                    npv1Stock.Stat1.TotalRCompositionAmount_SD = Math.Sqrt(npv1Stock.Stat1.TotalRCompositionAmount_VAR2);
                }
            }
            else
            {
                npv1Stock.Stat1.TotalRCompositionAmount_VAR2 = 0;
                npv1Stock.Stat1.TotalRCompositionAmount_SD = 0;
            }
        }
    }
}
