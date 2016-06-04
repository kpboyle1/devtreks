using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run mean and standard deviation statistics for standard 
    ///             DevTreks price and technology nodes.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. In this analyzer, aggregate analyses have one dictionary 
    ///             member with a (0=filepostion, and 0=nodeposition) index, 
    ///             holding statistics for an observation node. Comparative 
    ///             analyses have a file number number of dictionary members 
    ///             with each member holding one statistical object.
    ///             2. The work process is:
    ///             a. Fill in the cb collections from an observation node 
    ///             (a node with attributes holding a delimited string of values). 
    ///             b. Run the stats as the collections are being filled and 
    ///             add them to the collections.  
    ///             c. Once the statistics have been added to the collections, 
    ///             they are deserialized to an xelement which is then added 
    ///             to an analysis document. The object collections are the 
    ///             preferred data source for statistical analyses.
    /// </summary>
    public class StatisticalAnalyzer1 : AnalyzerHelper
    {
        //constructors
        public StatisticalAnalyzer1() { }
        //constructor sets class properties
        public StatisticalAnalyzer1(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
        }

        //properties
         public ARSAnalyzerHelper.ANALYZER_TYPES AnalyzerType { get; set; }

        //methods
        public bool SetStatisticalCalculation(ref XElement currentElement)
        {
            bool bHasStatistics = false;
            if (currentElement.HasAttributes)
            {
                this.CalcParams.AnalyzerParms.NodeName = currentElement.Name.LocalName;
                //init the benefits and costs collections
                this.SetBasicCollections(currentElement);
                //use streaming techniques to reduce memory footprint
                using (XmlReader currentElReader
                    = currentElement.CreateReader())
                {
                    currentElReader.MoveToContent();
                    if (currentElReader.MoveToFirstAttribute())
                    {
                        this.CalcParams.AnalyzerParms.ObservationAttributeName 
                            = currentElReader.Name;
                        this.CalcParams.AnalyzerParms.ObservationAttributeValue 
                            = currentElReader.Value;
                        //add each attribute to a member of the costs
                        //or benefits collections
                        SetBaseStatistics();
                        while (currentElReader.MoveToNextAttribute())
                        {
                            this.CalcParams.AnalyzerParms.ObservationAttributeName 
                                = currentElReader.Name;
                            this.CalcParams.AnalyzerParms.ObservationAttributeValue 
                                = currentElReader.Value;
                            SetBaseStatistics();
                        }
                    }
                }
                //deserialize cost and benefits collections to currentelement
                MakeStatisticalAnalysisElement(ref currentElement);
            }
            //reset collections and counters
            ClearCollections();
            if (string.IsNullOrEmpty(this.CalcParams.ErrorMessage))
                bHasStatistics = true;
            return bHasStatistics;
        }
        private bool NeedsStatistic(string observationValue)
        {
            bool bNeedsStatistic = false;
            if (!string.IsNullOrEmpty(
                this.CalcParams.AnalyzerParms.ObservationAttributeValue))
            {
                //1. double check whether this needs a statistic
                if (CostBenefitCalculator.NameIsAStatistic(
                    this.CalcParams.AnalyzerParms.ObservationAttributeName))
                {
                    //2. if a single observation value is a number
                    //it can generate a statistic
                    bool bIsNumber 
                        = CalculatorHelpers.ValidateIsNumber(observationValue);
                    if (bIsNumber)
                        bNeedsStatistic = true;
                }
            }
            return bNeedsStatistic;
        }
        private void SetBaseStatistics()
        {
            if (CostBenefitCalculator.NameIsAStatistic(
                this.CalcParams.AnalyzerParms.ObservationAttributeName))
            {
                if ((!this.CalcParams.AnalyzerParms.NodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                    && (this.AnalyzerType
                    != ARSAnalyzerHelper.ANALYZER_TYPES.resources01
                    && (!this.AnalyzerType.ToString().StartsWith(ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString()))))
                {
                    SetBasicStatistics();
                }
                else
                {
                    //resource use analyses are in a separate stat analyzer
                }
            }
            else
            {
                if (this.CalcParams.AnalyzerParms.ObservationAttributeName.Equals(
                    CostBenefitStatistic01.TRName)
                    || this.CalcParams.AnalyzerParms.ObservationAttributeName.Equals(
                    CostBenefitStatistic01.TRUnit))
                {
                    //the observation builder aggregates these two attributes
                    //into delimited strings which need to be parsed
                    SetBasicDescriptors();
                }
            }
        }
        private void SetBasicDescriptors()
        {
            //names and units help identify statistics in analyzers
            //that use this as the base analyzer
            int iObservations = 0;
            string sMember = string.Empty;
            this.CalcParams.AnalyzerParms.arrObservations
                = this.CalcParams.AnalyzerParms.ObservationAttributeValue.Split(
                Constants.STRING_DELIMITERS);
            if (this.CalcParams.AnalyzerParms.arrObservations != null)
            {
                string sMemberValue = string.Empty;
                int iFilePosition = 0;
                iObservations = this.CalcParams.AnalyzerParms.arrObservations.Length;
                if (iObservations != 0)
                {
                    int iCount = iObservations - 1;
                    for (int i = 0; i <= iCount; i++)
                    {
                        //sMember = 2.52_1 (second param is fileCount for comparisons)
                        sMember = this.CalcParams.AnalyzerParms.arrObservations[i];
                        GetValues(sMember, out sMemberValue, out iFilePosition);
                        if (this.CalcParams.AnalyzerParms.ComparisonType
                            == AnalyzerHelper.COMPARISON_OPTIONS.none)
                        {
                            //can't have more than one file position
                            //and no more than one descriptor
                            if (i == 0)
                            {
                                //add an attribute for comparing this file (i.e. _1)
                                SetStatisticProperty(0,
                                    this.CalcParams.AnalyzerParms.NodePositionIndex,
                                    this.CalcParams.AnalyzerParms.ObservationAttributeName,
                                    sMemberValue);
                                break;
                            }
                        }
                        else
                        {
                            //add an attribute for comparing this file (i.e. _1)
                            SetStatisticProperty(iFilePosition,
                                this.CalcParams.AnalyzerParms.NodePositionIndex,
                                this.CalcParams.AnalyzerParms.ObservationAttributeName,
                                sMemberValue);
                        }
                    }
                }
            }
        }
        private void SetBasicStatistics()
        {
            double dbTotal = 0;
            double dbMean = 0;
            int iObservations = 0;
            //variance
            double dbMemberSquaredTotal = 0;
            double dbStandardDeviation = 0;
            double dbMedian = 0;
            this.CalcParams.AnalyzerParms.arrObservations
                = this.CalcParams.AnalyzerParms.ObservationAttributeValue.Split(
                Constants.STRING_DELIMITERS);
            if (this.CalcParams.AnalyzerParms.arrObservations != null)
            {
                string sMember = string.Empty;
                int iFilePosition = 0;
                double dbMember = 0;
                double dbMemberSquared = 0;
                dbMemberSquaredTotal = 0;
                iObservations = this.CalcParams.AnalyzerParms.arrObservations.Length;
                if (iObservations != 0)
                {
                    int iCount = iObservations - 1;
                    for (int i = 0; i <= iCount; i++)
                    {
                        //sMember = 2.52_1 (second param is fileCount for comparisons)
                        sMember = this.CalcParams.AnalyzerParms.arrObservations[i];
                        GetValues(sMember, out dbMember, out iFilePosition);
                        if (NeedsStatistic(dbMember.ToString()))
                        {
                            if (this.CalcParams.AnalyzerParms.ComparisonType
                                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
                            {
                                //add an attribute for comparing this file (i.e. _1)
                                //if this attribute already exists (i.e. multiple observations) 
                                //add it to the attvalue already being stored
                                //comparisons are done betweeen files and 
                                //each element being compared can derive from multiple 
                                //observations within a file
                                AddComparisonStatisticProperties(iFilePosition, dbMember);
                            }
                            dbTotal += dbMember;
                            dbMemberSquared = Math.Pow(dbMember, 2);
                            dbMemberSquaredTotal += dbMemberSquared;
                        }
                    }
                    dbMean = dbTotal / iObservations;
                    if (iCount != 0 && iCount != 1 && dbMean != 0)
                    {
                        dbStandardDeviation = GetStandardDeviation(dbTotal,
                            iObservations, dbMemberSquaredTotal);
                    }
                    //alpha 7a switched to summary all columns
                }
            }
            if (this.CalcParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.none)
            {
                //check to make sure that fileposition = 0
                SetStatisticProperties(this.CalcParams.AnalyzerParms.FilePositionIndex,
                    iObservations, dbTotal, dbMean,
                    dbMemberSquaredTotal, dbStandardDeviation,
                    dbMedian);
            }
        }
        private void SetStatisticProperties(int filePosition,
            int observations, double total, double mean,
            double variance, double standardDeviation, double median)
        {
            //set the value of the attribute's corresponding property value
            SetStatisticProperty(filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                this.CalcParams.AnalyzerParms.ObservationAttributeName,
                total.ToString("f3"));
            //set the running sum number of observations for this comparison
            string sPropertyName = string.Concat(
              this.CalcParams.AnalyzerParms.ObservationAttributeName,
              Constants.FILENAME_DELIMITER, CostBenefitStatistic01.N);
            SetStatisticProperty(filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                sPropertyName, observations.ToString());
            //set the running sum mean for this comparison
            sPropertyName = string.Concat(
               this.CalcParams.AnalyzerParms.ObservationAttributeName,
               Constants.FILENAME_DELIMITER, CostBenefitStatistic01.MEAN);
            SetStatisticProperty(filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                sPropertyName, mean.ToString("f3"));
            //set the running sum standard deviation for this comparison
            sPropertyName = string.Concat(
               this.CalcParams.AnalyzerParms.ObservationAttributeName,
               Constants.FILENAME_DELIMITER, CostBenefitStatistic01.VAR2);
            SetStatisticProperty(filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                sPropertyName, variance.ToString("f3"));
            sPropertyName = string.Concat(
               this.CalcParams.AnalyzerParms.ObservationAttributeName,
               Constants.FILENAME_DELIMITER, CostBenefitStatistic01.SD);
            SetStatisticProperty(filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                sPropertyName, standardDeviation.ToString("f3"));
            sPropertyName = string.Concat(
               this.CalcParams.AnalyzerParms.ObservationAttributeName,
               Constants.FILENAME_DELIMITER, CostBenefitStatistic01.MED);
            SetStatisticProperty(filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                sPropertyName, median.ToString("f3"));
        }
        private void AddComparisonStatisticProperties(int filePosition,
            double attNumber)
        {
            if (this.CalcParams.AnalyzerParms.BaseCBStatistic.NameIsNotAStatistic(
                this.CalcParams.AnalyzerParms.ObservationAttributeName))
            {
                //for standard deviation's running sum
                double dbStandardDeviation = 0;
                double dbMemberSquared = Math.Pow(attNumber, 2);
                double dbMemberSquaredTotal = 0;
                //for mean and sd running sum
                //double dbNewValue = attNumber;
                int iObservations = 1;
                double dbMean = attNumber;
                double dbTotal = attNumber;
                double dbMedian = 0;
                //could be another observation for same element within file 
                //get the existing values from the collections, add the new 
                //attnumber to the statistical collection, and set new 
                //statistics (two tillage operations with same label 
                //will have two observations, N=2, and need new mean, sd ... set)
                //note this attribute will never have a statistical suffix
                //in the attribute name (i.e. _MEAN)
                string sAttValue = this.GetStatisticalPropertyValue(
                    filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex,
                    this.CalcParams.AnalyzerParms.ObservationAttributeName,
                    this.CalcParams.AnalyzerParms.NodeName, string.Empty);
                dbTotal = CalculatorHelpers.ConvertStringToDouble(sAttValue)
                    + attNumber;
                //get the new N and MEAN
                if (!string.IsNullOrEmpty(sAttValue))
                {
                    //set the number of observations
                    string sNValue = this.GetStatisticalPropertyValue(
                        filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex,
                        this.CalcParams.AnalyzerParms.ObservationAttributeName,
                        this.CalcParams.AnalyzerParms.NodeName, CostBenefitStatistic01.N);
                    iObservations = CalculatorHelpers.ConvertStringToInt(sNValue) + 1;
                    //set the mean
                    dbMean = dbTotal / iObservations;
                    //set the new variance 
                    string sSumVariance = this.GetStatisticalPropertyValue(
                        filePosition, this.CalcParams.AnalyzerParms.NodePositionIndex,
                        this.CalcParams.AnalyzerParms.ObservationAttributeName,
                        this.CalcParams.AnalyzerParms.NodeName, CostBenefitStatistic01.VAR2);
                    dbMemberSquared = dbMemberSquared 
                        + CalculatorHelpers.ConvertStringToDouble(sSumVariance);
                }
                //get the new variance 
                dbMemberSquaredTotal += dbMemberSquared;
                //get the new standard deviation
                dbStandardDeviation = GetStandardDeviation(dbTotal, iObservations, dbMemberSquaredTotal);
                //set the corresponding property values in the cb collections
                SetStatisticProperties(filePosition, 
                    iObservations, dbTotal, dbMean, dbMemberSquaredTotal,
                    dbStandardDeviation, dbMedian);
            }
        }
        private static double GetStandardDeviation(double total, int observations, double memberSquaredTotal)
        {
            double dbStandardDeviation = 0;
            double dbS1 = (Math.Pow(total, 2)) / observations;
            double dbS2 = memberSquaredTotal - dbS1;
            int iNMinus1 = observations - 1;
            if (dbS2 == 0)
            {
                //single observation 
                dbStandardDeviation = 0;
                return dbStandardDeviation;
            }
            //sample std deviation; population would use iCount as divisor
            double dbS2Count = dbS2 / iNMinus1;
            if (dbS2Count < 0) dbS2Count = 0;
            dbStandardDeviation = Math.Sqrt(dbS2Count);
            return dbStandardDeviation;
        }
        private bool SetNewMean(int observations, int count, double total)
        {
            bool bHasNewMean = false;
            double dbMean = 0;
            string sMeanAttName = string.Concat(Constants.FILENAME_DELIMITER,
                CEStatistic.MEAN);
            bool bHasName = HasName(this.CalcParams.AnalyzerParms.ObservationAttributeName,
                sMeanAttName);
            if (bHasName && observations != 0)
            {
                //mean of the means needed
                dbMean = total / observations;
                SetStatisticProperty(this.CalcParams.AnalyzerParms.FilePositionIndex,
                    0, this.CalcParams.AnalyzerParms.ObservationAttributeName,
                    dbMean.ToString("f3"));
                bHasNewMean = true;
            }
            return bHasNewMean;
        }
        private bool SetNewStandardDeviation(int observations, double variance, double total)
        {
            bool bHasNewSD = false;
            double dbApproxStandardDeviation = 0;
            string sSDAttName = string.Concat(Constants.FILENAME_DELIMITER,
                CEStatistic.SD);
            bool bHasName = HasName(this.CalcParams.AnalyzerParms.ObservationAttributeName,
                sSDAttName);
            if (bHasName)
            {
                if (observations > 1)
                {
                    string sAttValue = string.Empty;
                    //sd derived by taking square root of (variance / n-1)
                    //string sAttValue = CalculatorHelpers.GetAttribute(this.CalcParams.AnalyzerParms.ObservationElement, CEStatistic.OBSERVATIONS);
                    if (!string.IsNullOrEmpty(sAttValue))
                    {
                        string[] arrObservations = sAttValue.Split(Constants.STRING_DELIMITERS);
                        string sMember = string.Empty;
                        int iFilePosition = 0;
                        double dbMember = 0;
                        double dbTotalObservations = 0;
                        int iObservations = arrObservations.Length;
                        if (iObservations != 0)
                        {
                            int iCount = iObservations - 1;
                            for (int i = 0; i <= iCount; i++)
                            {
                                //sMember = 2.52_1 (second param is fileCount for comparisons)
                                sMember = arrObservations[i];
                                if (this.AnalyzerType
                                    != ARSAnalyzerHelper.ANALYZER_TYPES.resources01
                                    && (!this.AnalyzerType.ToString().StartsWith(ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())))
                                {
                                    GetValues(sMember, out dbMember, out iFilePosition);
                                }
                                else
                                {
                                    //resource analyses are in a separate stat analyzer
                                }
                                dbTotalObservations += dbMember;
                            }
                        }
                        int iNMinus1 = (dbTotalObservations > 1)
                            ? Convert.ToInt32(dbTotalObservations) - 1 : 1;
                        dbApproxStandardDeviation
                            = Math.Sqrt((variance / iNMinus1));
                        SetStatisticProperty(this.CalcParams.AnalyzerParms.FilePositionIndex,
                            0, this.CalcParams.AnalyzerParms.ObservationAttributeName,
                            dbApproxStandardDeviation.ToString("f3"));
                        bHasNewSD = true;
                    }
                }
            }
            return bHasNewSD;
        }
        //takes '25_0', and outputs 25 as a double and 0 as an integer
        private bool GetValues(string obsMember, out string memberValue,
            out int filePosition)
        {
            bool bHasFileCount = false;
            memberValue = string.Empty;
            string sValue = obsMember;
            filePosition = 0;
            int iFileExtensionStart
                = obsMember.LastIndexOf(Constants.FILENAME_DELIMITER);
            if (iFileExtensionStart != -1)
            {
                bHasFileCount = true;
                sValue = obsMember.Substring(0, iFileExtensionStart);
                //get rid of the delimiter
                iFileExtensionStart += 1;
                string sFileNumber = obsMember.Substring(iFileExtensionStart,
                    obsMember.Length - iFileExtensionStart);
                filePosition = CalculatorHelpers.ConvertStringToInt(sFileNumber);
            }
            memberValue = sValue;
            return bHasFileCount;
        }
        //takes '25_0', and outputs 25 as a double and 0 as an integer
        private bool GetValues(string obsMember, out double member,
            out int filePosition)
        {
            bool bHasFileCount = false;
            member = 0;
            //string sValue = obsMember;
            filePosition = 0;
            string sMemberValue = string.Empty;
            GetValues(obsMember, out sMemberValue, out filePosition);
            member = CalculatorHelpers.ConvertStringToDouble(sMemberValue);
            return bHasFileCount;
        }
        private bool IsResourceUseString()
        {
            //refactor when new resource use analyzer is built
            //resource use analysis includes resource types and units
            bool bIsString = false;
            bIsString = HasName(this.CalcParams.AnalyzerParms.ObservationAttributeName,
                "Type");
            if (bIsString != true)
            {
                bIsString = HasName(this.CalcParams.AnalyzerParms.ObservationAttributeName,
                    "Unit");
            }
            return bIsString;
        }
        private static bool HasName(string nameToCheck, string substring)
        {
            bool bHasName = false;
            int iIndex = nameToCheck.LastIndexOf(substring);
            if (iIndex > 0)
            {
                bHasName = true;
            }
            return bHasName;
        }
        private void SetStatisticProperty(int filePosition, int nodePosition,
            string attName, string attValue)
        {
            if (this.CalcParams.AnalyzerParms.NodeName.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                this.SetBenefitsMemberProperty(
                    filePosition, nodePosition, attName, attValue,
                    this.CalcParams.AnalyzerParms.NodeName);
            }
            else
            {
                this.SetCostsMemberProperty(
                    filePosition, nodePosition, attName, attValue,
                    this.CalcParams.AnalyzerParms.NodeName);
            }

        }
        private void MakeStatisticalAnalysisElement(ref XElement currentElement)
        {
            if (this.CalcParams.AnalyzerParms.NodeName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                MakeBenefitElement(ref currentElement);
            }
            else
            {
                MakeCostElement(ref currentElement);
            }
        }
        private void MakeBenefitElement(ref XElement benefitsElement)
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
                            //set the general attributes of the node (id, name)
                            benefitStat.SetCalculatorAttributes(string.Empty,
                                ref writer);
                            benefitStat.SetStatisticGeneralAttributes(string.Empty,
                                ref writer);
                            if (this.CalcParams.AnalyzerParms.ComparisonType
                                == COMPARISON_OPTIONS.compareonly)
                            {
                                //first cols display summary of all comparisons
                                //and use single indexes (comparisons use double indexes)
                                MakeBenefitSummaryElement(ref writer);
                            }
                        }
                        sAttNameExtension
                            = GetAttributeNameExtension(iNodeCount,
                            iFilePosition);
                        if (string.IsNullOrEmpty(sAttNameExtension)
                            && iFilePosition > 0)
                        {
                            //collections have been built incorrectly
                        }
                        else
                        {
                            //each analyzer should filter out statistics that 
                            //are not needed in display, or file sizes become very large
                            benefitStat.SetTotalBenefitsAttributes(sAttNameExtension,
                                ref writer);
                            benefitStat.SetNBenefitsAttributes(sAttNameExtension,
                               ref writer);
                            benefitStat.SetMeanBenefitsAttributes(sAttNameExtension,
                               ref writer);
                            benefitStat.SetStdDevBenefitsAttributes(sAttNameExtension,
                               ref writer);
                        }
                        iNodeCount += 1;
                    }
                }
                writer.Flush();
                writer.Close();
            }
        }
        private void MakeBenefitSummaryElement(ref XmlWriter writer)
        {
            //the first member of the benefits collection needs to be 
            //a summation of the sibling members
            if (this.CalcParams.AnalyzerParms.BenefitStatistics != null)
            {
                int iNodeCount = 0;
                int iFilePosition = 0;
                CostBenefitStatistic01 sumAllComparison = new CostBenefitStatistic01();
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.BenefitStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 benefitStat in kvp.Value)
                    {
                        //the "All" column is nothing more than a summation of 
                        //index columns (and should not be interpreted differently)
                        sumAllComparison.TotalRName = CostBenefitStatistic01.ALL;
                        sumAllComparison.TotalRUnit = CostBenefitStatistic01.ALL;
                        
                        sumAllComparison.TotalAMR += benefitStat.TotalAMR;
                        sumAllComparison.TotalAMR_N += benefitStat.TotalAMR_N;
                        sumAllComparison.TotalAMR_MEAN += benefitStat.TotalAMR_MEAN;
                        //keep it simple; just need good sample analyzers at this point
                        sumAllComparison.TotalAMR_SD += benefitStat.TotalAMR_SD;

                        sumAllComparison.TotalRPrice += benefitStat.TotalRPrice;
                        sumAllComparison.TotalRPrice_MEAN += benefitStat.TotalRPrice_MEAN;
                        sumAllComparison.TotalRPrice_SD += benefitStat.TotalRPrice_SD;
                        sumAllComparison.TotalRCompositionAmount += benefitStat.TotalRCompositionAmount;
                        sumAllComparison.TotalRCompositionAmount_MEAN += benefitStat.TotalRCompositionAmount_MEAN;
                        sumAllComparison.TotalRCompositionAmount_SD += benefitStat.TotalRCompositionAmount_SD;
                        sumAllComparison.TotalRAmount += benefitStat.TotalRAmount;
                        sumAllComparison.TotalRAmount_MEAN += benefitStat.TotalRAmount_MEAN;
                        sumAllComparison.TotalRAmount_SD += benefitStat.TotalRAmount_SD;
                        iNodeCount += 1;
                    }
                }
                MakeBenefitSummaryElement(sumAllComparison, ref writer);
            }
        }
        private void MakeBenefitSummaryElement(CostBenefitStatistic01 summaryBenefit,
            ref XmlWriter writer)
        {
            //write the summarybenefits collection to the benefits element
            //write the new attributes using a writer
            //the summary doesn't have an extension
            string sAttNameExtension = string.Empty;
            summaryBenefit.SetTotalBenefitsAttributes(sAttNameExtension,
                ref writer);
            summaryBenefit.SetNBenefitsAttributes(sAttNameExtension,
                ref writer);
            summaryBenefit.SetMeanBenefitsAttributes(sAttNameExtension,
                ref writer);
            summaryBenefit.SetStdDevBenefitsAttributes(sAttNameExtension,
                ref writer);
        }
        private void MakeCostElement(ref XElement currentElement)
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
                            costStat.SetCalculatorAttributes(string.Empty,
                                ref writer);
                            costStat.SetStatisticGeneralAttributes(string.Empty,
                                ref writer);
                            if (this.CalcParams.AnalyzerParms.ComparisonType
                                == COMPARISON_OPTIONS.compareonly)
                            {
                                //first cols display summary of all comparisons
                                //and use single indexes (comparisons use double indexes)
                                MakeCostSummaryElement(ref writer);
                            }
                        }
                        sAttNameExtension
                            = this.GetAttributeNameExtension(iNodeCount,
                            iFilePosition);
                        if (string.IsNullOrEmpty(sAttNameExtension)
                            && iFilePosition > 0)
                        {
                            //collections have been built incorrectly
                        }
                        else
                        {
                            //each analyzer should filter out statistics that 
                            //are not needed in display, or file sizes become very large
                            costStat.SetAmortCostsAttributes(sAttNameExtension,
                                ref writer);
                            if (NeedsBenefitStatsWithCosts(this.CalcParams.AnalyzerParms.NodeName))
                            {
                                costStat.SetAmortBenefitsAttributes(sAttNameExtension,
                                    ref writer);
                            }
                        }
                        iNodeCount += 1;
                    }
                }
                writer.Flush();
                writer.Close();
            }
        }
        private void MakeCostSummaryElement(ref XmlWriter writer)
        {
            //the first member of the benefits collection needs to be 
            //a summation of the sibling members
            if (this.CalcParams.AnalyzerParms.CostStatistics != null)
            {
                int iNodeCount = 0;
                int iFilePosition = 0;
                CostBenefitStatistic01 sumAllComparison = new CostBenefitStatistic01();
                foreach (KeyValuePair<int, List<CostBenefitStatistic01>> kvp
                    in this.CalcParams.AnalyzerParms.CostStatistics)
                {
                    iFilePosition = kvp.Key;
                    iNodeCount = 0;
                    foreach (CostBenefitStatistic01 costStat in kvp.Value)
                    {
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
                MakeCostSummaryElement(sumAllComparison, ref writer);
            }
        }
        private void MakeCostSummaryElement(CostBenefitStatistic01 summaryCost,
            ref XmlWriter writer)
        {
            //write the summarybenefits collection to the benefits element
            //write the new attributes using a writer
            //the summary column doesn't have an extension
            string sAttNameExtension = string.Empty;
            //limit attributes to those needed or file size balloons
            summaryCost.SetAmortCostsAttributes(sAttNameExtension,
                ref writer);
            if ((!this.CalcParams.AnalyzerParms.NodeName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
                && (!this.CalcParams.AnalyzerParms.NodeName.EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
                && (!this.CalcParams.AnalyzerParms.NodeName.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())))
            {
                summaryCost.SetAmortBenefitsAttributes(sAttNameExtension,
                    ref writer);
            }
        }
        public string GetAttributeNameExtension(int nodeCount,
            int fileNumber)
        {
            string sAttNameExtension = string.Empty;
            //indexed attribute name is used to display the xml in columns
            if (this.CalcParams.AnalyzerParms.ComparisonType
                == AnalyzerHelper.COMPARISON_OPTIONS.compareonly)
            {
                sAttNameExtension = string.Concat(
                    Constants.FILENAME_DELIMITER, fileNumber.ToString());
            }
            return sAttNameExtension;
        }
        private void ClearCollections()
        {
            //this analyzer does not use cumulative totals
            //the underlying calculator that generated the 
            //numbers is assumed to have already done that
            //the analyzer just runs numbers node by node
            //clear stats collections
            this.CalcParams.AnalyzerParms.BenefitObjectsCount = 0;
            //watch this: some analyzers may need resetting at operation/component
            this.CalcParams.AnalyzerParms.CostObjectsCount = 0;
            this.CalcParams.AnalyzerParms.NodePositionIndex = 0;
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
    }
}