using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run cost effectiveness statistics for standard DevTreks 
    ///             price and technology nodes
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. This class assumes that outputs are processed by the
    ///             event publisher before any other nodes. This assumption 
    ///             works for the selected publisher's streaming techniques.
    ///             2. Please review AnalyzerParameters notes for an explanation 
    ///             of how the fileposition and nodeposition indexes work 
    ///             with the benefits and costs collections. The dictionaries 
    ///             work similar to the statistical analyzers, except they hold 
    ///             object collections corresponding to the number of 
    ///             outputs or inputs in the type of effectiveness analysis.
    ///             3. The work process is to:
    ///             a. Complete a statistical analysis using StatsAnalyzer1. 
    ///             b. Use the resultant statistical analysis xml document to 
    ///             fill in the cb collections using each statistics node. 
    ///             The Benefits dictionary's object collection holds 
    ///             an All (summation) object in position 0, with each output 
    ///             in the rest of the collection. These outputs become 
    ///             the divisors used in the effectiveness analyses. 
    ///             c. The Costs dictionary holds an All object in position 0
    ///             (holding summations) with the same number of objects as 
    ///             the outputs collection. Each of these objects holds an 
    ///             effectiveness statistic calculated using a cost value 
    ///             as a numerator and an output value as a divisor.
    ///             d. Both dictionaries have one member in fileposition = 0 
    ///             for aggregate analysis, and file number of members for 
    ///             comparative analysis.
    ///             e. Once the statistics have been added to the collections, 
    ///             they are deserialized to an xelement which is then added 
    ///             to an analysis document. These types of object collections 
    ///             are the preferred data source for statistical analyses.
    /// </summary>
    public class CostEffectivenessAnalyzer1 : AnalyzerHelper
    {
        //constructors
        public CostEffectivenessAnalyzer1() { }
        //constructor sets class properties
        public CostEffectivenessAnalyzer1(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
        }

        //properties
        public ARSAnalyzerHelper.ANALYZER_TYPES AnalyzerType { get; set; }

        //methods
        public bool SetCostEffectivenessCalculation(
            ref XElement currentElement)
        {
            bool bHasStatistics = false;
            if (currentElement.HasAttributes)
            {
                this.CalcParams.AnalyzerParms.NodeName = currentElement.Name.LocalName;
                //comparisons need to check an attribute's filenumber 
                //set up the this.analyzerparams.benefitsstats and costsstats collections
                this.SetOutputBasedCollections(currentElement);
                if (currentElement.Name.LocalName
                    != BudgetInvestment.BUDGET_TYPES.budgetoutcome.ToString()
                    || this.CalcParams.AnalyzerParms.ComparisonType
                    == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                {
                    //use streaming techniques to reduce memory footprint
                    using (XmlReader currentElReader
                        = currentElement.CreateReader())
                    {
                        currentElReader.MoveToContent();
                        if (currentElReader.MoveToFirstAttribute())
                        {
                            this.CalcParams.AnalyzerParms.ObservationAttributeName = currentElReader.Name;
                            this.CalcParams.AnalyzerParms.ObservationAttributeValue = currentElReader.Value;
                            //set the ce statistic
                            SetBaseStatisticCalculation();
                            while (currentElReader.MoveToNextAttribute())
                            {
                                this.CalcParams.AnalyzerParms.ObservationAttributeName = currentElReader.Name;
                                this.CalcParams.AnalyzerParms.ObservationAttributeValue = currentElReader.Value;
                                //set the ce statistic
                                SetBaseStatisticCalculation();
                            }
                        }
                    }
                }
                //deserialize cost and benefits collections to currentelement
                MakeCostEffectivenessElement(ref currentElement);
            }
            else
            {
                if (currentElement.Name.LocalName
                    == BudgetInvestment.BUDGET_TYPES.budgetoutcomes.ToString())
                {
                    //deserialize benefits collection to outputs/output node
                    MakeBenefitsElement(ref currentElement);
                }
            }
            //reset collections and counters
            ClearCollections();
            if (string.IsNullOrEmpty(this.CalcParams.ErrorMessage))
                bHasStatistics = true;
            return bHasStatistics;
        }
        private void SetBaseStatisticCalculation()
        {
            //fix and set any indexing found in attribute name
            PrepareComparison();
            if (this.CalcParams.AnalyzerParms.ObservationAttributeName 
                != string.Empty)
            {
                if (this.CalcParams.AnalyzerParms.NodeName
                    .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    SetBenefitCalculation();
                }
                else
                {
                    SetCostCalculation();
                }
            }
        }
        private void PrepareComparison()
        {
            if (this.CalcParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                this.CalcParams.AnalyzerParms.FilePositionIndex = 0;
                string sBaseAttName = string.Empty;
                string sFilePosition = string.Empty;
                CalculatorHelpers.GetSubstringsSeparateLast(this.CalcParams.AnalyzerParms.ObservationAttributeName,
                    Constants.FILENAME_DELIMITER, out sBaseAttName, out sFilePosition);
                //don't process any attribute that is not a comparison (base ids already handled)
                this.CalcParams.AnalyzerParms.ObservationAttributeName = string.Empty;
                if (!string.IsNullOrEmpty(sBaseAttName)
                    && !string.IsNullOrEmpty(sFilePosition))
                {
                    bool bIsANumber = CalculatorHelpers.ValidateIsNumber(sFilePosition);
                    if (bIsANumber)
                    {
                        //when serialized, the existing single index name will be 
                        //converted to a double indexed name with 
                        //_0_1 = first output, second file
                        this.CalcParams.AnalyzerParms.ObservationAttributeName = sBaseAttName;
                        this.CalcParams.AnalyzerParms.FilePositionIndex
                            = CalculatorHelpers.ConvertStringToInt(sFilePosition);
                    }
                }
            }
        }
        private void SetBenefitCalculation()
        {
            if (CEStatistic.NeedsCEStatistic(
                this.CalcParams.AnalyzerParms.ObservationAttributeName))
            {
                //no additional calculations are needed to 
                //set benefit properties
                this.SetBenefitsMemberProperty(
                    this.CalcParams.AnalyzerParms.FilePositionIndex,
                    this.CalcParams.AnalyzerParms.NodePositionIndex,
                    this.CalcParams.AnalyzerParms.ObservationAttributeName,
                    this.CalcParams.AnalyzerParms.ObservationAttributeValue,
                    this.CalcParams.AnalyzerParms.NodeName);
            }
            else
            {
                if (this.CalcParams.AnalyzerParms.ObservationAttributeName.Equals(
                    CostBenefitStatistic01.TRName)
                    || this.CalcParams.AnalyzerParms.ObservationAttributeName.Equals(
                    CostBenefitStatistic01.TRUnit))
                {
                    this.SetBenefitsMemberProperty(
                        this.CalcParams.AnalyzerParms.FilePositionIndex,
                        this.CalcParams.AnalyzerParms.NodePositionIndex,
                        this.CalcParams.AnalyzerParms.ObservationAttributeName,
                        this.CalcParams.AnalyzerParms.ObservationAttributeValue,
                        this.CalcParams.AnalyzerParms.NodeName);
                }
            }
        }
        private void SetCostCalculation()
        {
            if (CEStatistic.NeedsCEStatistic(
                this.CalcParams.AnalyzerParms.ObservationAttributeName))
            {
                if (this.CalcParams.AnalyzerParms.ComparisonType 
                    != COMPARISON_OPTIONS.compareonly)
                {
                    SetEffectivenessCalculations();
                }
                else
                {
                    SetEffectivenessComparativeCalculations();
                }
            }
        }
        
        private void SetEffectivenessCalculations()
        {
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.BenefitStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    string sNumerator = this.CalcParams.AnalyzerParms.ObservationAttributeValue;
                    foreach (CostBenefitStatistic01 benefitStat in kvp.Value)
                    {
                        //carry out the ce cost statistic
                        string sCostPerOutput
                            = CalculateEffectivenessValue(benefitStat, sNumerator);
                        //set the property of the corresponding coststat member
                        //i.e. if benefit is file 2, output 2; use the same index
                        //to find the coststat object whose stat value must be set
                        this.SetCostsMemberProperty(
                            iFilePosition, iNodeCount,
                            this.CalcParams.AnalyzerParms.ObservationAttributeName, 
                            sCostPerOutput, this.CalcParams.AnalyzerParms.NodeName);
                        iNodeCount += 1;
                    }
                }
            }
        }
        private void SetEffectivenessComparativeCalculations()
        {
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iNodeCount = 0;
                if (this.CalcParams.AnalyzerParms.BenefitStatistics.ContainsKey(
                    this.CalcParams.AnalyzerParms.FilePositionIndex))
                {
                    List<CostBenefitStatistic01> benefitStats 
                        = this.CalcParams.AnalyzerParms.BenefitStatistics[
                        this.CalcParams.AnalyzerParms.FilePositionIndex];
                    if (benefitStats != null)
                    {
                        iNodeCount = 0;
                        string sNumerator = this.CalcParams.AnalyzerParms.ObservationAttributeValue;
                        foreach (CostBenefitStatistic01 benefitStat in benefitStats)
                        {
                            //carry out the ce cost statistic
                            string sCostPerOutput
                                = CalculateEffectivenessValue(benefitStat, sNumerator);
                            //set the property of the corresponding coststat member
                            //i.e. if benefit is file 2, output 2; use the same index
                            //to find the coststat object whose stat value must be set
                            this.SetCostsMemberProperty(
                                this.CalcParams.AnalyzerParms.FilePositionIndex, iNodeCount,
                                this.CalcParams.AnalyzerParms.ObservationAttributeName,
                                sCostPerOutput, this.CalcParams.AnalyzerParms.NodeName);
                            iNodeCount += 1;
                        }
                    }
                }
            }
        }
        private string CalculateEffectivenessValue(CostBenefitStatistic01 benefitStat, 
            string numerator)
        {
            //these calculations can be fine-tuned, as needed
            string sCalculatedValue = string.Empty;
            if (AttributeNeedsCalculation())
            {
                string sDivisor = GetDivisor(benefitStat);
                sCalculatedValue = GetEffectivenessValue(numerator,
                    sDivisor);
            }
            else
            {
                sCalculatedValue = numerator;
            }
            return sCalculatedValue;
        }
        private bool AttributeNeedsCalculation()
        {
            bool bNeedsCalc = true;
            if (this.CalcParams.AnalyzerParms.ObservationAttributeName
                .Equals(CostBenefitStatistic01.TAMR_N)
                || this.CalcParams.AnalyzerParms.ObservationAttributeName
                .Equals(CostBenefitStatistic01.TAMOC_N))
            {
                //observations don't need calculation, but need to be included
                //in stats
                bNeedsCalc = false;
            }
            return bNeedsCalc;
        }
        private string GetDivisor(CostBenefitStatistic01 benefitStat)
        {
            string sDivisor = string.Empty;
            //keep the initial analyzers simple; other analyzers can get as complex as needed
            if (this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.effectiveness01a
                || this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.effectiveness02a)
            {
                if (NeedsTotals())
                {
                    //numerator is total
                    sDivisor = benefitStat.TotalRAmount.ToString();
                }
                else
                {
                    //numerator is mean
                    sDivisor = benefitStat.TotalRAmount_MEAN.ToString();
                }
            }
            else
            {
                if (NeedsTotals())
                {
                    //numerator is total
                    sDivisor = benefitStat.TotalAMR.ToString();
                }
                else
                {
                    //numerator is mean
                    sDivisor = benefitStat.TotalAMR_MEAN.ToString();
                }
            }
            //the std deviation stat needs more thought
            return sDivisor;
        }
        private bool NeedsTotals()
        {
            bool bNeedsTotals = false;
            //divisor is a total not a mean
            switch (this.CalcParams.AnalyzerParms.ObservationAttributeName)
            {
                case CostBenefitStatistic01.TAMOC:
                    bNeedsTotals = true;
                    break;
                case CostBenefitStatistic01.TAMAOH:
                   bNeedsTotals = true;
                   break;
                case CostBenefitStatistic01.TAMCAP:
                   bNeedsTotals = true;
                   break;
                default:
                    bNeedsTotals = false;
                    break;
            }
            return bNeedsTotals;
        }
       
        private string GetEffectivenessValue(string numerator, 
            string divisor)
        {
            string sRatio = string.Empty;
            double dbNumerator = (string.IsNullOrEmpty(numerator))
                ? 0 : CalculatorHelpers.ConvertStringToDouble(numerator);
            double dbDivisor = (string.IsNullOrEmpty(divisor))
                 ? 0 : CalculatorHelpers.ConvertStringToDouble(divisor);
            if (dbDivisor == 0)
            {
                sRatio = "0";
            }
            else
            {
                double dbRatio = dbNumerator / dbDivisor;
                sRatio = dbRatio.ToString("f4");
            }
            return sRatio;
        }
        private void MakeCostEffectivenessElement(ref XElement currentElement)
        {
            if (!this.CalcParams.AnalyzerParms.NodeName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                //the all member is a summation of siblings
                FixCostsAllMember();
                //make a cost element from the costs collection
                MakeCECostElement(ref currentElement);
                //benefits elements handled with outputs node
            }
        }
        private void FixCostsAllMember()
        {
            //the first member of the costs collection needs to be 
            //a summation of the sibling members
            if (this.CalcParams.AnalyzerParms.CostStatistics != null)
            {
                int iNodeCount = 0;
                int iFilePosition = 0;
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.CostStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    CostBenefitStatistic01 sumCosts = new CostBenefitStatistic01();
                    foreach (CostBenefitStatistic01 costStat in kvp.Value)
                    {
                        //the "All" column is nothing more than a summation of 
                        //index columns (and should not be interpreted differently)
                        if (iNodeCount == 0)
                        {
                            //need the properties 
                            sumCosts = new CostBenefitStatistic01(costStat);
                            //but init the numbers at 0
                            sumCosts.InitMeanBenefitsProperties();
                            sumCosts.InitMeanCostsProperties();
                            sumCosts.InitNBenefitsProperties();
                            sumCosts.InitNCostsProperties();
                            sumCosts.InitTotalBenefitsProperties();
                            sumCosts.InitTotalCostsProperties();
                            sumCosts.InitStdDevBenefitsProperties();
                            sumCosts.InitStdDevCostsProperties();
                            sumCosts.TotalRName = CostBenefitStatistic01.ALL;
                            sumCosts.TotalRUnit = CostBenefitStatistic01.ALL;
                            sumCosts.TotalOCUnit = CostBenefitStatistic01.ALL;
                        }
                        else
                        {
                            sumCosts.TotalObservations += sumCosts.TotalObservations;
                            sumCosts.TotalAMOC_N += costStat.TotalAMOC_N;
                            sumCosts.TotalAMOC += costStat.TotalAMOC;
                            sumCosts.TotalAMOC_MEAN += costStat.TotalAMOC_MEAN;
                            sumCosts.TotalAMOC_SD += costStat.TotalAMOC_SD;
                            sumCosts.TotalAMAOH += costStat.TotalAMAOH;
                            sumCosts.TotalAMAOH_MEAN += costStat.TotalAMAOH_MEAN;
                            sumCosts.TotalAMAOH_SD += costStat.TotalAMAOH_SD;
                            sumCosts.TotalAMCAP += costStat.TotalAMCAP;
                            sumCosts.TotalAMCAP_MEAN += costStat.TotalAMCAP_MEAN;
                            sumCosts.TotalAMCAP_SD += costStat.TotalAMCAP_SD;
                            sumCosts.TotalAMOC_NET += costStat.TotalAMOC_NET;
                            sumCosts.TotalAMOC_NET_MEAN += costStat.TotalAMOC_NET_MEAN;
                            sumCosts.TotalAMOC_NET_SD += costStat.TotalAMOC_NET_SD;
                            sumCosts.TotalAMAOH_NET += costStat.TotalAMAOH_NET;
                            sumCosts.TotalAMAOH_NET_MEAN += costStat.TotalAMAOH_NET_MEAN;
                            sumCosts.TotalAMAOH_NET_SD += costStat.TotalAMAOH_NET_SD;
                            sumCosts.TotalAMAOH_NET2 += costStat.TotalAMAOH_NET2;
                            sumCosts.TotalAMAOH_NET2_MEAN += costStat.TotalAMAOH_NET2_MEAN;
                            sumCosts.TotalAMAOH_NET2_SD += costStat.TotalAMAOH_NET2_SD;
                            sumCosts.TotalAMCAP_NET += costStat.TotalAMCAP_NET;
                            sumCosts.TotalAMCAP_NET_MEAN += costStat.TotalAMCAP_NET_MEAN;
                            sumCosts.TotalAMCAP_NET_SD += costStat.TotalAMCAP_NET_SD;
                            sumCosts.TotalAMINCENT_NET += costStat.TotalAMINCENT_NET;
                            sumCosts.TotalAMINCENT_NET_MEAN += costStat.TotalAMINCENT_NET_MEAN;
                            sumCosts.TotalAMINCENT_NET_SD += costStat.TotalAMINCENT_NET_SD;
                        }
                        iNodeCount += 1;
                    }
                    //replace the existing member
                    this.AddCostStatisticsToDictionary(
                       iFilePosition, 0, sumCosts);
                }
            }
        }
        public void MakeBenefitsElement(ref XElement outputsElement)
        {
            FixBenefitsAllMember();
            //remove the children outputs
            outputsElement.RemoveAll();
            //use BenefitsStats to build a new element and add to outputsEl
            XElement benefitsElement
                = new XElement(BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString());
            //XElement benefitsElement 
            //    = new XElement(Output.OUTPUT_PRICE_TYPES.output.ToString());
            MakeCEBenefitElement(ref benefitsElement);
            //use a randomly generated id
            if (this.CalcParams.RndGenerator == null)
                this.CalcParams.RndGenerator = new Random();
            string sNewId
                = CalculatorHelpers.GetRandomInteger(this.CalcParams.RndGenerator).ToString();
            CalculatorHelpers.SetAttribute(benefitsElement,
                Calculator1.cId, sNewId);
            //add new child output node
            outputsElement.Add(benefitsElement);
        }
        private void FixBenefitsAllMember()
        {
            //the first member of the benefits collection needs to be 
            //a summation of the sibling members
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iNodeCount = 0;
                int iFilePosition = 0;
                int iObservations = 0;
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.BenefitStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    CostBenefitStatistic01 sumBenefits = new CostBenefitStatistic01();
                    foreach (CostBenefitStatistic01 benefitStat in kvp.Value)
                    {
                        //the "All" columns are nothing more than a summation of 
                        //index columns (and should not be interpreted differently)
                        if (iNodeCount == 0)
                        {
                            sumBenefits = new CostBenefitStatistic01(benefitStat);
                            sumBenefits.InitMeanBenefitsProperties();
                            sumBenefits.InitNBenefitsProperties();
                            sumBenefits.InitStdDevBenefitsProperties();
                            sumBenefits.InitTotalBenefitsProperties();
                            sumBenefits.TotalRName = CostBenefitStatistic01.ALL;
                            sumBenefits.TotalRUnit = CostBenefitStatistic01.ALL;
                        }
                        else
                        {
                            sumBenefits.TotalObservations += benefitStat.TotalObservations;
                            sumBenefits.TotalAMR_N += benefitStat.TotalAMR_N;
                            sumBenefits.TotalAMR_MEAN += benefitStat.TotalAMR_MEAN;
                            sumBenefits.TotalRAmount_MEAN += benefitStat.TotalRAmount_MEAN;
                            //keep it simple; just need good sample analyzers at this point
                            sumBenefits.TotalAMR_SD += benefitStat.TotalAMR_SD;
                            sumBenefits.TotalRAmount_SD += benefitStat.TotalRAmount_SD;
                        }
                        iNodeCount += 1;
                        iObservations += 1;
                    }
                    //replace the existing member
                    this.AddBenefitStatisticsToDictionary(
                       iFilePosition, 0, sumBenefits);
                }
            }
        }
        private void MakeCEBenefitElement(ref XElement benefitsElement)
        {
            //write the new attributes using a writer
            XmlWriter writer = benefitsElement.CreateWriter();
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.BenefitStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 benefitStat in kvp.Value)
                    {
                        if (iNodeCount == 0 && iFilePosition == 0)
                        {
                            //set the general attributes of the node (id, name)
                            sAttNameExtension = string.Empty;
                            benefitStat.SetCalculatorAttributes(sAttNameExtension,
                                ref writer);
                            benefitStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                            //set the column counter
                            writer.WriteAttributeString(CEStatistic.OUTPUTS,
                                kvp.Value.Count.ToString());
                            if (this.CalcParams.AnalyzerParms.ComparisonType
                                == COMPARISON_OPTIONS.compareonly)
                            {
                                //first cols display summary of all comparisons
                                //and use single indexes (comparisons use double indexes)
                                MakeCEBenefitSummaryElement(ref writer);
                            }
                        }
                        sAttNameExtension = GetAttributeNameExtension(iNodeCount,
                            iFilePosition);
                        if (!string.IsNullOrEmpty(sAttNameExtension))
                            benefitStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                        //limit attributes to those needed or file size balloons
                        benefitStat.SetAmortBenefitsAttributes(sAttNameExtension,
                            ref writer);
                        iNodeCount += 1;
                    }
                }
                writer.Flush();
                writer.Close();
            }
        }
        private void MakeCEBenefitSummaryElement(ref XmlWriter writer)
        {
            //the first member of the benefits collection needs to be 
            //a summation of the sibling members
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iNodeCount = 0;
                int iFilePosition = 0;
                CostBenefitStatistic01 sumAllComparison = null;
                List<CostBenefitStatistic01> summaryBenefits 
                    = new List<CostBenefitStatistic01>();
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.BenefitStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 benefitStat in kvp.Value)
                    {
                        if (iFilePosition == 0)
                        {
                            summaryBenefits.Add(new CostBenefitStatistic01());
                        }
                        sumAllComparison = summaryBenefits[iNodeCount];
                        //the "All" columns are nothing more than a summation of 
                        //index columns (and should not be interpreted differently)
                        sumAllComparison.TotalRName = CostBenefitStatistic01.ALL;
                        sumAllComparison.TotalRUnit = CostBenefitStatistic01.ALL; 
                        sumAllComparison.TotalAMR_N += benefitStat.TotalAMR_N;
                        sumAllComparison.TotalAMR_MEAN += benefitStat.TotalAMR_MEAN;
                        sumAllComparison.TotalRAmount_MEAN += benefitStat.TotalRAmount_MEAN;
                        //keep it simple; just need good sample analyzers at this point
                        sumAllComparison.TotalAMR_SD += benefitStat.TotalAMR_SD;
                        sumAllComparison.TotalRAmount_SD += benefitStat.TotalRAmount_SD;
                        iNodeCount += 1;
                    }
                }
                MakeCEBenefitSummaryElement(summaryBenefits, ref writer);
            }
        }
        private void MakeCEBenefitSummaryElement(List<CostBenefitStatistic01> summaryBenefits,
            ref XmlWriter writer)
        {
            //write the summarybenefits collection to the benefits element
            //write the new attributes using a writer
            int iNodeCount = 0;
            string sAttNameExtension = string.Empty;
            foreach (CostBenefitStatistic01 benefitStat in summaryBenefits)
            {
                sAttNameExtension = GetAttributeNameExtension(iNodeCount);
                if (iNodeCount == 0)
                {
                    //trname, trunit get set with setAmort
                }
                if (!string.IsNullOrEmpty(sAttNameExtension))
                    benefitStat.SetStatisticGeneralAttributes(sAttNameExtension,
                        ref writer);
                //limit attributes to those needed or file size balloons
                benefitStat.SetAmortBenefitsAttributes(sAttNameExtension,
                    ref writer);
                iNodeCount += 1;
            }
        }
        //this should be seldom used because it uses more attributes than needed
        private void MakeAllBenefitElement(ref XElement benefitsElement)
        {
            //remove atts
            benefitsElement.RemoveAttributes();
            //write the new attributes using a writer
            XmlWriter writer = benefitsElement.CreateWriter();
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.BenefitStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 benefitStat in kvp.Value)
                    {
                        if (iNodeCount == 0 && iFilePosition == 0)
                        {
                            //set the column counter
                            writer.WriteAttributeString(CEStatistic.OUTPUTS,
                                kvp.Value.Count.ToString());
                            //set the general attributes of the node (id, name)
                            sAttNameExtension = string.Empty;
                            benefitStat.SetCalculatorAttributes(sAttNameExtension,
                                ref writer);
                            benefitStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                        }
                        sAttNameExtension
                            = this.GetAttributeNameExtension(iNodeCount,
                            iFilePosition);
                        if (!string.IsNullOrEmpty(sAttNameExtension))
                            benefitStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                        benefitStat.SetTotalBenefitsAttributes(sAttNameExtension,
                            ref writer);
                        benefitStat.SetMeanBenefitsAttributes(sAttNameExtension,
                           ref writer);
                        benefitStat.SetStdDevBenefitsAttributes(sAttNameExtension,
                           ref writer);
                        iNodeCount += 1;
                    }
                }
                writer.Flush();
                writer.Close();
            }
        }
        
        private void MakeCECostElement(ref XElement currentElement)
        {
            //remove atts
            currentElement.RemoveAttributes();
            //write the new attributes using a writer
            XmlWriter writer = currentElement.CreateWriter();
            if (this.CalcParams.AnalyzerParms.CostStatistics != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                //the indexes in CECostStats are the same as CEBenefitsStats
                //so benefit and cost columns will line up
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.CostStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 costStat in kvp.Value)
                    {
                        if (iNodeCount == 0 && iFilePosition == 0)
                        {
                            //set the general attributes of the node (id, name)
                            sAttNameExtension = string.Empty;
                            costStat.SetCalculatorAttributes(sAttNameExtension,
                                ref writer);
                            costStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                            if (this.CalcParams.AnalyzerParms.ComparisonType
                                == COMPARISON_OPTIONS.compareonly)
                            {
                                //first cols display summary of all comparisons
                                //and use single indexes (comparisons use double indexes)
                                MakeCECostSummaryElement(ref writer);
                            }
                        }
                        sAttNameExtension
                            = this.GetAttributeNameExtension(iNodeCount,
                            iFilePosition);
                        //deserialize basestats into current element
                        SetNameAttributes(iNodeCount, iFilePosition,
                            ref writer);
                        if (!string.IsNullOrEmpty(sAttNameExtension))
                            costStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                        //limit attributes to those needed or file size balloons
                        costStat.SetAmortCostsAttributes(sAttNameExtension,
                            ref writer);
                        if (NeedsBenefitStatsWithCosts(this.CalcParams.AnalyzerParms.NodeName))
                        {
                            costStat.SetAmortBenefitsAttributes(sAttNameExtension,
                                ref writer);
                        }
                        iNodeCount += 1;
                    }
                }
                writer.Flush();
                writer.Close();
            }
        }
        private void MakeCECostSummaryElement(ref XmlWriter writer)
        {
            //the first member of the benefits collection needs to be 
            //a summation of the sibling members
            if (this.CalcParams.AnalyzerParms.CostStatistics != null)
            {
                int iNodeCount = 0;
                int iFilePosition = 0;
                CostBenefitStatistic01 sumAllComparison = null;
                List<CostBenefitStatistic01> summaryCosts
                    = new List<CostBenefitStatistic01>();
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.CostStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 costStat in kvp.Value)
                    {
                        if (iFilePosition == 0)
                        {
                            summaryCosts.Add(new CostBenefitStatistic01());
                        }
                        sumAllComparison = summaryCosts[iNodeCount];
                        //the "All" column is nothing more than a summation of 
                        //index columns (and should not be interpreted differently)
                        //the all column has 
                        sumAllComparison.TotalRName = CostBenefitStatistic01.ALL;
                        sumAllComparison.TotalRUnit = CostBenefitStatistic01.ALL; 
                       
                        sumAllComparison.TotalAMOC_N += costStat.TotalAMOC_N;
                        sumAllComparison.TotalAMOC += costStat.TotalAMOC;
                        sumAllComparison.TotalAMOC_MEAN += costStat.TotalAMOC_MEAN;
                        sumAllComparison.TotalAMOC_SD += costStat.TotalAMOC_SD;
                        sumAllComparison.TotalAMAOH += costStat.TotalAMAOH;
                        sumAllComparison.TotalAMAOH_MEAN += costStat.TotalAMAOH_MEAN;
                        sumAllComparison.TotalAMAOH_SD += costStat.TotalAMAOH_SD;
                        sumAllComparison.TotalAMCAP += costStat.TotalAMCAP;
                        sumAllComparison.TotalAMCAP_MEAN += costStat.TotalAMCAP_MEAN;
                        sumAllComparison.TotalAMCAP_SD += costStat.TotalAMCAP_SD;
                        sumAllComparison.TotalAMOC_NET += costStat.TotalAMOC_NET;
                        sumAllComparison.TotalAMOC_NET_MEAN += costStat.TotalAMOC_NET_MEAN;
                        sumAllComparison.TotalAMOC_NET_SD += costStat.TotalAMOC_NET_SD;
                        sumAllComparison.TotalAMAOH_NET += costStat.TotalAMAOH_NET;
                        sumAllComparison.TotalAMAOH_NET_MEAN += costStat.TotalAMAOH_NET_MEAN;
                        sumAllComparison.TotalAMAOH_NET_SD += costStat.TotalAMAOH_NET_SD;
                        sumAllComparison.TotalAMAOH_NET2 += costStat.TotalAMAOH_NET2;
                        sumAllComparison.TotalAMAOH_NET2_MEAN += costStat.TotalAMAOH_NET2_MEAN;
                        sumAllComparison.TotalAMAOH_NET2_SD += costStat.TotalAMAOH_NET2_SD;
                        sumAllComparison.TotalAMCAP_NET += costStat.TotalAMCAP_NET;
                        sumAllComparison.TotalAMCAP_NET_MEAN += costStat.TotalAMCAP_NET_MEAN;
                        sumAllComparison.TotalAMCAP_NET_SD += costStat.TotalAMCAP_NET_SD;
                        sumAllComparison.TotalAMINCENT_NET += costStat.TotalAMINCENT_NET;
                        sumAllComparison.TotalAMINCENT_NET_MEAN += costStat.TotalAMINCENT_NET_MEAN;
                        sumAllComparison.TotalAMINCENT_NET_SD += costStat.TotalAMINCENT_NET_SD;
                        if (NeedsBenefitStatsWithCosts(this.CalcParams.AnalyzerParms.NodeName))
                        {
                            sumAllComparison.TotalAMR += costStat.TotalAMR;
                            sumAllComparison.TotalAMR_N += costStat.TotalAMR_N;
                            sumAllComparison.TotalAMR_MEAN += costStat.TotalAMR_MEAN;
                            //keep it simple; just need good sample analyzers at this point
                            sumAllComparison.TotalAMR_SD += costStat.TotalAMR_SD;

                            sumAllComparison.TotalRPrice += costStat.TotalRPrice;
                            sumAllComparison.TotalRPrice_MEAN += costStat.TotalRPrice_MEAN;
                            sumAllComparison.TotalRPrice_SD += costStat.TotalRPrice_SD;
                            sumAllComparison.TotalRCompositionAmount += costStat.TotalRCompositionAmount;
                            sumAllComparison.TotalRCompositionAmount_MEAN += costStat.TotalRCompositionAmount_MEAN;
                            sumAllComparison.TotalRCompositionAmount_SD += costStat.TotalRCompositionAmount_SD;
                            sumAllComparison.TotalRAmount += costStat.TotalRAmount;
                            sumAllComparison.TotalRAmount_MEAN += costStat.TotalRAmount_MEAN;
                            sumAllComparison.TotalRAmount_SD += costStat.TotalRAmount_SD;
                        }
                        iNodeCount += 1;
                    }
                }
                MakeCECostSummaryElement(summaryCosts, ref writer);
            }
        }
        private void MakeCECostSummaryElement(List<CostBenefitStatistic01> summaryCosts,
            ref XmlWriter writer)
        {
            //write the summarybenefits collection to the benefits element
            //write the new attributes using a writer
            int iNodeCount = 0;
            string sAttNameExtension = string.Empty;
            foreach (CostBenefitStatistic01 costStat in summaryCosts)
            {
                sAttNameExtension = GetAttributeNameExtension(iNodeCount);
                if (iNodeCount == 0)
                {
                    //names and units get set with setamort
                }
                if (!string.IsNullOrEmpty(sAttNameExtension))
                    costStat.SetStatisticGeneralAttributes(sAttNameExtension,
                        ref writer);
                //limit attributes to those needed or file size balloons
                costStat.SetAmortCostsAttributes(sAttNameExtension,
                    ref writer);
                if ((!this.CalcParams.AnalyzerParms.NodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                    && (!this.CalcParams.AnalyzerParms.NodeName.EndsWith( OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
                    && (!this.CalcParams.AnalyzerParms.NodeName.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())))
                {
                    costStat.SetAmortBenefitsAttributes(sAttNameExtension,
                        ref writer);
                }
                iNodeCount += 1;
            }
        }
        //this should be seldom used because it uses more attributes than needed
        private void MakeAllCostElement(ref XElement currentElement)
        {
            //remove atts
            currentElement.RemoveAttributes();
            //write the new attributes using a writer
            XmlWriter writer = currentElement.CreateWriter();
            if (this.CalcParams.AnalyzerParms.CostStatistics != null)
            {
                int iFilePosition = 0;
                int iNodeCount = 0;
                string sAttNameExtension = string.Empty;
                //the indexes in CECostStats are the same as CEBenefitsStats
                //so benefit and cost columns will line up
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.CostStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 costStat in kvp.Value)
                    {
                        if (iNodeCount == 0 && iFilePosition == 0)
                        {
                            //set the general attributes of the node (id, name)
                            sAttNameExtension = string.Empty;
                            costStat.SetCalculatorAttributes(sAttNameExtension,
                                ref writer);
                            costStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                        }
                        sAttNameExtension = GetAttributeNameExtension(iNodeCount,
                            iFilePosition);
                        //deserialize basestats into current element
                        SetNameAttributes(iNodeCount, iFilePosition, 
                            ref writer);
                        if (!string.IsNullOrEmpty(sAttNameExtension))
                            costStat.SetStatisticGeneralAttributes(sAttNameExtension,
                                ref writer);
                        costStat.SetTotalCostsAttributes(sAttNameExtension,
                            ref writer);
                        costStat.SetMeanCostsAttributes(sAttNameExtension,
                            ref writer);
                        costStat.SetStdDevCostsAttributes(sAttNameExtension,
                            ref writer);
                        if ((!this.CalcParams.AnalyzerParms.NodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                            && (!this.CalcParams.AnalyzerParms.NodeName.EndsWith( OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
                            && (!this.CalcParams.AnalyzerParms.NodeName.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())))
                        {
                            costStat.SetTotalBenefitsAttributes(sAttNameExtension,
                                ref writer);
                            costStat.SetMeanBenefitsAttributes(sAttNameExtension,
                               ref writer);
                            costStat.SetStdDevBenefitsAttributes(sAttNameExtension,
                               ref writer);
                        }
                        iNodeCount += 1;
                    }
                }
                writer.Flush();
                writer.Close();
            }
        }
        
        private void SetNameAttributes(int nodePosition, 
            int filePosition, ref XmlWriter writer)
        {
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                if (this.CalcParams.AnalyzerParms.BenefitStatistics.Count > filePosition)
                {
                    if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition].Count
                        > nodePosition)
                    {
                        string sAttNameExtension = string.Empty;
                        //note this does not include _AMOUNT_ in att name
                        if (this.CalcParams.AnalyzerParms.ComparisonType
                            == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                        {
                            sAttNameExtension = string.Concat(
                                Constants.FILENAME_DELIMITER, nodePosition.ToString(),
                                Constants.FILENAME_DELIMITER, filePosition.ToString());
                        }
                        else
                        {
                            sAttNameExtension = string.Concat(
                                Constants.FILENAME_DELIMITER, nodePosition.ToString());
                        }
                        //each node needs attributes to identify and set columns
                        if (nodePosition == 0 && filePosition == 0)
                        {
                            writer.WriteAttributeString(CEStatistic.OUTPUTS,
                                this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition].Count.ToString());
                        }
                        //the stats analyzer needs to set these attributes
                        writer.WriteAttributeString(string.Concat(CostBenefitStatistic01.TRName, sAttNameExtension),
                            this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition].TotalRName);
                        writer.WriteAttributeString(string.Concat(CostBenefitStatistic01.TRUnit, sAttNameExtension),
                            this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition].TotalRUnit);
                    }
                }
            }
        }
        public string GetAttributeNameExtension(int nodeCount)
        {
            string sAttNameExtension = string.Empty;
            string sDataDefAmountOrRevenue = CEStatistic.AMOUNT;
            if (this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.effectiveness01b
                || this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.effectiveness02b)
            {
                if (this.CalcParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sDataDefAmountOrRevenue = CEStatistic.BENEFIT;
                }
                else if (this.CalcParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sDataDefAmountOrRevenue = CEStatistic.REVENUE;
                }
            }
            //indexed attribute name is used to display the xml in columns
            if (this.CalcParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                //output index needed
                sAttNameExtension = string.Concat(
                    Constants.FILENAME_DELIMITER, sDataDefAmountOrRevenue,
                    Constants.FILENAME_DELIMITER, nodeCount.ToString());
            }
            else
            {
                sAttNameExtension = string.Empty;
            }
            return sAttNameExtension;
        }
        public string GetAttributeNameExtension(int nodeCount,
            int fileNumber)
        {
            string sAttNameExtension = string.Empty;
            string sDataDefAmountOrRevenue = CEStatistic.AMOUNT;
            if (this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.effectiveness01b
                || this.AnalyzerType
                == ARSAnalyzerHelper.ANALYZER_TYPES.effectiveness02b)
            {
                if (this.CalcParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    sDataDefAmountOrRevenue = CEStatistic.BENEFIT;
                }
                else if (this.CalcParams.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    sDataDefAmountOrRevenue = CEStatistic.REVENUE;
                }
            }
            //indexed attribute name is used to display the xml in columns
            if (this.CalcParams.AnalyzerParms.ComparisonType
                != AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                //output index needed
                sAttNameExtension = string.Concat(
                    Constants.FILENAME_DELIMITER, sDataDefAmountOrRevenue,
                    Constants.FILENAME_DELIMITER, nodeCount.ToString());
            }
            else
            {
                //output and file number index needed (zero-based)
                //_0_0 = 1st output, 1st file; AMOUNT_3_8 = 4th output, 9th file
                sAttNameExtension = string.Concat(
                    Constants.FILENAME_DELIMITER, sDataDefAmountOrRevenue,
                    Constants.FILENAME_DELIMITER, nodeCount.ToString(),
                    Constants.FILENAME_DELIMITER, fileNumber.ToString());
            }
            return sAttNameExtension;
        }
        private void ClearCollections()
        {
            //outputs change each time a time period is processed
            if (this.CalcParams.AnalyzerParms.NodeName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || this.CalcParams.AnalyzerParms.NodeName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //this analyzer does not use cumulative totals
                //the underlying calculator that generated the 
                //numbers is assumed to have already done that
                //the analyzer just runs numbers node by node
                //clear stats collections
                this.CalcParams.AnalyzerParms.BenefitObjectsCount = 0;
                this.CalcParams.AnalyzerParms.CostObjectsCount = 0;
                this.CalcParams.AnalyzerParms.NodePositionIndex = 0;
                //remember that only one stat file is being processed
                //the attribute name holds the file position
                this.CalcParams.AnalyzerParms.FilePositionIndex = 0;
                if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
                {
                    this.CalcParams.AnalyzerParms.BenefitStatistics.Clear();
                    this.CalcParams.AnalyzerParms.BenefitStatistics = null;
                }
                if (this.CalcParams.AnalyzerParms.CostStatistics != null)
                {
                    this.CalcParams.AnalyzerParms.CostStatistics.Clear();
                    this.CalcParams.AnalyzerParms.CostStatistics = null;
                }
            }
            else
            {
                //costs collections get reset with each node
                this.CalcParams.AnalyzerParms.CostObjectsCount = 0;
                this.CalcParams.AnalyzerParms.NodePositionIndex = 0;
                //remember that only one stat file is being processed
                //the attribute name holds the file position
                this.CalcParams.AnalyzerParms.FilePositionIndex = 0;
                if (this.CalcParams.AnalyzerParms.CostStatistics != null)
                {
                    this.CalcParams.AnalyzerParms.CostStatistics.Clear();
                    this.CalcParams.AnalyzerParms.CostStatistics = null;
                }
            }
        }
        
    }
}
