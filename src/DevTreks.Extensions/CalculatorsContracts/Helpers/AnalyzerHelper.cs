using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Helper functions for analyzers
    ///Author:		www.devtreks.org
    ///Date:		2015, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class AnalyzerHelper
    {
        //constructor
        public AnalyzerHelper() { }
        public AnalyzerHelper(CalculatorParameters calcParameters)
        {
            this.CalcParams = new CalculatorParameters(calcParameters);
        }

        //properties
        public CalculatorParameters CalcParams { get; set; }
        public const string ANALYZER_TYPE =  Calculator1.cAnalyzerType;
        public const string FILESTOANALYZE_EXTENSION_TYPE = Calculator1.cFilesToAnalyzeExtensionType;

        //enums
        /// <summary>
        /// comparison options for statistical analysis
        /// </summary>
        public enum COMPARISON_OPTIONS
        {
            none = 0,
            compareonly = 1
        }
        /// <summary>
        /// grouping options for statistical analysis
        /// </summary>
        public enum AGGREGATION_OPTIONS
        {
            none = 0,
            labels = 1,
            types = 2,
            groups = 3
        }
        /// <summary>
        /// Yes means limit analysis to subfolders; 
        /// no means carry out analysis using descendants with a 'filetoanalyzeextensiontype'
        /// that are terminal files (last descendant should be the first calcd doc)
        /// in the case of devpacks, no means carry out analysis using any descendant with a 'filetoanalyzeextensiontype'
        /// this latter feature needs tweaking
        /// </summary>
        public enum SUBFOLDER_OPTIONS
        {
            no = 0,
            yes = 1
        }
        public enum ANALYZER_GENERAL_TYPES
        {
            none            = 0,
            //mean, sd
            basic           = 1,
            //cost per unit output
            outputbased     = 2,
            //corn output per unit labor
            inputbased      = 3
        }
        public enum NODE_ANALYZER_TYPES
        {
            new_node        = 1,
            existing_node   = 2,
            comparison_node = 3
        }

        //methods
        public static COMPARISON_OPTIONS GetComparisonType(
            XElement calculationsElement)
        {
            COMPARISON_OPTIONS eType = COMPARISON_OPTIONS.none;
            string sType = CalculatorHelpers.GetAttribute(calculationsElement, 
                Calculator1.cOption1);
            if (sType == COMPARISON_OPTIONS.none.ToString())
            {
                eType = COMPARISON_OPTIONS.none;
            }
            else if (sType == COMPARISON_OPTIONS.compareonly.ToString())
            {
                eType = COMPARISON_OPTIONS.compareonly;
            }
            return eType;
        }
        public static AGGREGATION_OPTIONS GetAggregationType(XElement calculationsElement)
        {
            AGGREGATION_OPTIONS eType = AGGREGATION_OPTIONS.none;
            string sType = CalculatorHelpers.GetAttribute(calculationsElement,
                Calculator1.cOption2);
            if (sType == AGGREGATION_OPTIONS.none.ToString())
            {
                eType = AGGREGATION_OPTIONS.none;
            }
            else if (sType == AGGREGATION_OPTIONS.groups.ToString())
            {
                eType = AGGREGATION_OPTIONS.groups;
            }
            else if (sType == AGGREGATION_OPTIONS.labels.ToString())
            {
                eType = AGGREGATION_OPTIONS.labels;
            }
            else if (sType == AGGREGATION_OPTIONS.types.ToString())
            {
                eType = AGGREGATION_OPTIONS.types;
            }
            return eType;
        }
        public static SUBFOLDER_OPTIONS GetSubFolderType(XElement calculationsElement)
        {
            SUBFOLDER_OPTIONS eType = SUBFOLDER_OPTIONS.no;
            string sType = CalculatorHelpers.GetAttribute(calculationsElement,
                Calculator1.cOption4);
            if (sType == SUBFOLDER_OPTIONS.no.ToString())
            {
                eType = SUBFOLDER_OPTIONS.no;
            }
            else if (sType == SUBFOLDER_OPTIONS.yes.ToString())
            {
                eType = SUBFOLDER_OPTIONS.yes;
            }
            return eType;
        }
        public static bool GetDisplayFullViewOption(XElement calculationsElement)
        {
            //default is no (meaning don't display grandchildren ... elements)
            bool bNeedsFullView = CalculatorHelpers.GetAttributeBool(calculationsElement,
                Calculator1.cOption5);
            return bNeedsFullView;
        }
        public static ANALYZER_GENERAL_TYPES GetAnalyzerType(
            string analyzerType, ref string errorMsg)
        {
            ANALYZER_GENERAL_TYPES eAnalyzerType = ANALYZER_GENERAL_TYPES.none;
            if (analyzerType == ANALYZER_GENERAL_TYPES.basic.ToString())
            {
                eAnalyzerType = ANALYZER_GENERAL_TYPES.basic;
            }
            else if (analyzerType == ANALYZER_GENERAL_TYPES.inputbased.ToString())
            {
                eAnalyzerType = ANALYZER_GENERAL_TYPES.inputbased;
            }
            else if (analyzerType == ANALYZER_GENERAL_TYPES.outputbased.ToString())
            {
                eAnalyzerType = ANALYZER_GENERAL_TYPES.outputbased;
            }
            if (eAnalyzerType == ANALYZER_GENERAL_TYPES.none)
            {
                errorMsg = Errors.MakeStandardErrorMsg("ANALYSES_CHOOSEONE");
            }
            return eAnalyzerType;
        }
        //set up benefit and cost collections that can then be 
        //analyzed by basic statistical analyzers (mean, sd, median)
        public virtual void SetBasicCollections(XElement currentElement)
        {
            //need to parse an attribute name in a componly analysis
            this.CalcParams.AnalyzerParms.FilePositionIndex = 0;
            //set the benefits and costs collections 
            if (this.CalcParams.AnalyzerParms.NodeName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                this.CalcParams.AnalyzerParms.BenefitObjectsCount += 1;
                this.CalcParams.AnalyzerParms.NodePositionIndex
                    = this.CalcParams.AnalyzerParms.BenefitObjectsCount - 1;
                CostBenefitStatistic01 baseStat = new CostBenefitStatistic01();
                baseStat.SetCalculatorProperties(currentElement);
                baseStat.SetStatisticGeneralProperties(currentElement);
                //set the "All" column (holds sum of all outputs)
                AddBenefitStatisticsToDictionary(
                   this.CalcParams.AnalyzerParms.FilePositionIndex,
                   this.CalcParams.AnalyzerParms.NodePositionIndex, baseStat);
            }
            else
            {
                this.CalcParams.AnalyzerParms.CostObjectsCount += 1;
                this.CalcParams.AnalyzerParms.NodePositionIndex
                    = this.CalcParams.AnalyzerParms.CostObjectsCount - 1;
                CostBenefitStatistic01 baseStat = new CostBenefitStatistic01();
                baseStat.SetCalculatorProperties(currentElement);
                baseStat.SetStatisticGeneralProperties(currentElement);
                //need a new coststat object for each output object
                AddCostStatisticsToDictionary(
                    this.CalcParams.AnalyzerParms.FilePositionIndex,
                    this.CalcParams.AnalyzerParms.NodePositionIndex, baseStat);
            }
        }
        //set up benefit and cost collections that can then be 
        //analyzed by effectiveness analyzers (cost per unit output)
        public virtual void SetOutputBasedCollections(XElement currentElement)
        {
            //need to parse an attribute name in a componly analysis
            this.CalcParams.AnalyzerParms.FilePositionIndex = 0;
            //set the benefits and costs collections (basestats.count = outputs.count)
            if (this.CalcParams.AnalyzerParms.NodeName
               .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //two attributes are used to set the benefits and costs collections
                //the benefits count must be set before the costs collection, 
                //so the order that the nodes get streamed is critical
                if (this.CalcParams.AnalyzerParms.BenefitObjectsCount == 0)
                {
                    this.CalcParams.AnalyzerParms.BenefitObjectsCount += 1;
                    this.CalcParams.AnalyzerParms.NodePositionIndex
                        = this.CalcParams.AnalyzerParms.BenefitObjectsCount - 1;
                    //first output is the "All" column holding summation of outputs
                    CostBenefitStatistic01 baseAllStat = new CostBenefitStatistic01();
                    baseAllStat.SetCalculatorProperties(currentElement);
                    //the all column's stats will be a summation of outputs and 
                    //get set after the Benefits collection has been filled
                    baseAllStat.SetStatisticGeneralProperties(currentElement);
                    //set the "All" column (holds sum of all outputs)
                    AddBenefitStatisticsToDictionary(
                       this.CalcParams.AnalyzerParms.FilePositionIndex, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                       baseAllStat);
                }
                //this is not a conditional clause
                //set the first output column
                this.CalcParams.AnalyzerParms.BenefitObjectsCount += 1;
                this.CalcParams.AnalyzerParms.NodePositionIndex
                    = this.CalcParams.AnalyzerParms.BenefitObjectsCount - 1;
                CostBenefitStatistic01 baseStat = new CostBenefitStatistic01();
                //set totals
                baseStat.SetCalculatorProperties(currentElement);
                //set statistics
                baseStat.SetStatisticGeneralProperties(currentElement);
                baseStat.SetMeanBenefitsProperties(currentElement);
                baseStat.SetMedianBenefitsProperties(currentElement);
                baseStat.SetNBenefitsProperties(currentElement);
                baseStat.SetStdDevBenefitsProperties(currentElement);
                //convert app names to stat names
                SetNameProperties(baseStat, currentElement);
                //set the "All" column (holds sum of all outputs)
                AddBenefitStatisticsToDictionary(
                   this.CalcParams.AnalyzerParms.FilePositionIndex, this.CalcParams.AnalyzerParms.NodePositionIndex, 
                   baseStat);
            }
            else
            {
                this.CalcParams.AnalyzerParms.CostObjectsCount += 1;
                this.CalcParams.AnalyzerParms.NodePositionIndex
                    = this.CalcParams.AnalyzerParms.CostObjectsCount - 1;
                //ce analyses use the benefitsobject count to set
                //the zero-based number of nodes
                for (int i = 0; i <= this.CalcParams.AnalyzerParms.BenefitObjectsCount - 1; i++)
                {
                    CostBenefitStatistic01 baseStat = new CostBenefitStatistic01();
                    baseStat.SetCalculatorProperties(currentElement);
                    baseStat.SetStatisticGeneralProperties(currentElement);
                    //need a new coststat object for each output object
                    AddCostStatisticsToDictionary(
                        this.CalcParams.AnalyzerParms.FilePositionIndex, i, baseStat);
                }
            }
        }
        
        public virtual void AddBenefitStatisticsToDictionary(
            int filePosition, int nodePosition, CostBenefitStatistic01 baseStat)
        {
            if (filePosition < 0 || nodePosition < 0)
            {
                this.CalcParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return;
            }
            if (this.CalcParams.AnalyzerParms.BenefitStatistics == null)
                this.CalcParams.AnalyzerParms.BenefitStatistics
                = new Dictionary<int, List<CostBenefitStatistic01>>();
            if (this.CalcParams.AnalyzerParms.BenefitStatistics.ContainsKey(filePosition))
            {
                if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition].Count <= i)
                        {
                            this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition]
                                .Add(new CostBenefitStatistic01());
                        }
                    }
                    this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                        = baseStat;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<CostBenefitStatistic01> baseStats
                    = new List<CostBenefitStatistic01>();
                KeyValuePair<int, List<CostBenefitStatistic01>> newStat
                    = new KeyValuePair<int, List<CostBenefitStatistic01>>(
                        filePosition, baseStats);
                this.CalcParams.AnalyzerParms.BenefitStatistics.Add(newStat);
                AddBenefitStatisticsToDictionary(filePosition, nodePosition,
                    baseStat);
            }
        }
        public virtual void AddCostStatisticsToDictionary(
            int filePosition, int nodePosition, CostBenefitStatistic01 baseStat)
        {
            if (filePosition < 0 || nodePosition < 0)
            {
                this.CalcParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return;
            }
            if (this.CalcParams.AnalyzerParms.CostStatistics == null)
                this.CalcParams.AnalyzerParms.CostStatistics
                = new Dictionary<int, List<CostBenefitStatistic01>>();
            if (this.CalcParams.AnalyzerParms.CostStatistics.ContainsKey(filePosition))
            {
                if (this.CalcParams.AnalyzerParms.CostStatistics[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (this.CalcParams.AnalyzerParms.CostStatistics[filePosition].Count <= i)
                        {
                            this.CalcParams.AnalyzerParms.CostStatistics[filePosition]
                                .Add(new CostBenefitStatistic01());
                        }
                    }
                    this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                        = baseStat;
                }
            }
            else
            {
                //add the missing dictionary entry
                List<CostBenefitStatistic01> baseStats
                    = new List<CostBenefitStatistic01>();
                KeyValuePair<int, List<CostBenefitStatistic01>> newStat
                    = new KeyValuePair<int, List<CostBenefitStatistic01>>(
                        filePosition, baseStats);
                this.CalcParams.AnalyzerParms.CostStatistics.Add(newStat);
                AddCostStatisticsToDictionary(filePosition, nodePosition,
                    baseStat);
            }
        }
        public virtual void SetBenefitsMemberProperty(int filePosition, int nodePosition,
            string attName, string attValue, string nodeName)
        {
            if (filePosition < 0 || nodePosition < 0)
            {
                this.CalcParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return;
            }
            if (this.CalcParams.AnalyzerParms.BenefitStatistics.ContainsKey(filePosition))
            {
                if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition].Count <= i)
                        {
                            this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition]
                                .Add(new CostBenefitStatistic01());
                        }
                    }
                    SetBenefitStatisticProperty(filePosition, nodePosition,
                        attName, attValue, nodeName);
                }
                else
                {
                    AddBenefitStatisticsToDictionary(
                        filePosition, nodePosition, new CostBenefitStatistic01());
                    SetBenefitsMemberProperty(filePosition, nodePosition,
                        attName, attValue, nodeName);
                }
            }
            else
            {
                AddBenefitStatisticsToDictionary(
                    filePosition, nodePosition, new CostBenefitStatistic01());
                SetBenefitsMemberProperty(filePosition, nodePosition,
                    attName, attValue, nodeName);
            }
        }
        public virtual void SetBenefitStatisticProperty(int filePosition, int nodePosition,
            string attName, string attValue, string nodeName)
        {
            if (attName.Contains(CostBenefitStatistic01.MEAN))
            {
                this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                    .SetMeanBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.SD))
            {
                this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                    .SetStdDevBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.VAR2))
            {
                this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                    .SetVarianceBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.MED))
            {
                this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                    .SetMedianBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.N))
            {
                this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                    .SetNBenefitsProperties(attName, attValue);
            }
            else
            {
                if (CostBenefitCalculator.NameIsAStatistic(attName))
                {
                    this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                        .SetTotalBenefitsProperty(attName, attValue);
                }
                else
                {
                    //late concession
                    if (attName.Equals(CostBenefitStatistic01.TRName)
                        || attName.Equals(CostBenefitStatistic01.TRUnit))
                    {
                        this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                            .SetTotalBenefitsProperty(attName, attValue);
                    }
                    //not needed with most collections when they are 
                    //initialized correctly
                    //this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                    //    .SetCalculatorProperties(attName, attValue);
                }
            }
        }
        public virtual void SetBenefitStatisticPropertyForCosts(int filePosition, int nodePosition,
            string attName, string attValue, string nodeName)
        {
            if (attName.Contains(CostBenefitStatistic01.MEAN))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetMeanBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.SD))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetStdDevBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.VAR2))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetVarianceBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.MED))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetMedianBenefitsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.N))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetNBenefitsProperties(attName, attValue);
            }
            else
            {
                if (CostBenefitCalculator.NameIsAStatistic(attName))
                {
                    this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                        .SetTotalBenefitsProperty(attName, attValue);
                }
                else
                {
                    //late concession
                    if (attName.Equals(CostBenefitStatistic01.TRName)
                        || attName.Equals(CostBenefitStatistic01.TRUnit))
                    {
                        this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                            .SetTotalBenefitsProperty(attName, attValue);
                    }
                    //should not be needed if stat object init correctly
                    //this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    //    .SetCalculatorProperties(attName, attValue);
                }
            }
        }
        public virtual void SetCostsMemberProperty(int filePosition, int nodePosition,
            string attName, string attValue, string nodeName)
        {
            if (filePosition < 0 || nodePosition < 0)
            {
                this.CalcParams.ErrorMessage
                    = Errors.MakeStandardErrorMsg("ANALYSES_INDEX_OUTOFBOUNDS");
                return;
            }
            if (this.CalcParams.AnalyzerParms.CostStatistics.ContainsKey(filePosition))
            {
                if (this.CalcParams.AnalyzerParms.CostStatistics[filePosition] != null)
                {
                    for (int i = 0; i <= nodePosition; i++)
                    {
                        if (this.CalcParams.AnalyzerParms.CostStatistics[filePosition].Count <= i)
                        {
                            this.CalcParams.AnalyzerParms.CostStatistics[filePosition]
                                .Add(new CostBenefitStatistic01());
                        }
                    }
                    SetCostStatisticProperty(filePosition, nodePosition,
                        attName, attValue, nodeName);
                    if (NeedsBenefitStatsWithCosts(nodeName))
                    {
                        //use the costs collection to set a benefits property
                        SetBenefitStatisticPropertyForCosts(filePosition, nodePosition,
                            attName, attValue, nodeName);
                    }
                }
                else
                {
                    AddCostStatisticsToDictionary(
                        filePosition, nodePosition, new CostBenefitStatistic01());
                    SetCostsMemberProperty(filePosition, nodePosition,
                        attName, attValue, nodeName);
                }
            }
            else
            {
                AddCostStatisticsToDictionary(
                    filePosition, nodePosition, new CostBenefitStatistic01());
                SetCostsMemberProperty(filePosition, nodePosition,
                    attName, attValue, nodeName);
            }
        }
        public virtual void SetCostStatisticProperty(int filePosition, int nodePosition,
            string attName, string attValue, string nodeName)
        {
            if (attName.Contains(CostBenefitStatistic01.MEAN))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetMeanCostsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.SD))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetStdDevCostsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.VAR2))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetVarianceCostsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.MED))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetMedianCostsProperties(attName, attValue);
            }
            else if (attName.Contains(CostBenefitStatistic01.N))
            {
                this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    .SetNCostsProperties(attName, attValue);
            }
            else
            {
                //set base statistic props
                if (CostBenefitCalculator.NameIsAStatistic(attName))
                {
                    this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                        .SetTotalCostsProperty(attName, attValue);
                }
                else
                {
                    //names written separately (unlike setbenstatprop)
                    ////most collections were initialized with the base descriptors
                    ////they need
                    //this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                    //    .SetCalculatorProperties(attName, attValue);
                }
            }
        }
        public static bool NeedsBenefitStatsWithCosts(string nodeName)
        {
            bool bNeedsStats = false;
            if (nodeName.StartsWith(BudgetInvestment.BUDGET_TYPES.budget.ToString())
                || nodeName.StartsWith(BudgetInvestment.INVESTMENT_TYPES.investment.ToString()))
            {
                bNeedsStats = true;
            }
            return bNeedsStats;
        }
        public virtual string GetStatisticalPropertyValue(int filePosition,
            int nodePosition, string attName, string nodeName, string statType)
        {
            string sPropertyValue = string.Empty;
            string sExtendedAttName = string.Concat(attName, 
                Constants.FILENAME_DELIMITER, statType);
            if (nodeName.EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                if (this.CalcParams.AnalyzerParms.BenefitStatistics.ContainsKey(filePosition))
                {
                    if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition] != null)
                    {
                        if (this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition] != null)
                        {
                            if (statType == CostBenefitStatistic01.MEAN)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                                    .GetMeanBenefitsProperty(sExtendedAttName);
                            }
                            else if (statType == CostBenefitStatistic01.SD)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                                    .GetStdDevBenefitsProperty(sExtendedAttName);
                            }
                            else if (statType == CostBenefitStatistic01.VAR2)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                                    .GetVarianceBenefitsProperty(sExtendedAttName);
                            }
                            else if (statType == CostBenefitStatistic01.MED)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                                    .GetMedianBenefitsProperty(sExtendedAttName);
                            }
                            else if (statType == CostBenefitStatistic01.N)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                                    .GetNBenefitsProperty(sExtendedAttName);
                            }
                            else
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.BenefitStatistics[filePosition][nodePosition]
                                    .GetTotalBenefitsProperty(attName);
                            }
                        }
                    }
                }
            }
            else 
            {
                //try both collections
                if (this.CalcParams.AnalyzerParms.CostStatistics.ContainsKey(filePosition))
                {
                    if (this.CalcParams.AnalyzerParms.CostStatistics[filePosition] != null)
                    {
                        if (this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition] != null)
                        {
                            if (statType == CostBenefitStatistic01.N)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetNCostsProperty(sExtendedAttName);
                                if (string.IsNullOrEmpty(sPropertyValue))
                                {
                                    sPropertyValue
                                        = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetNBenefitsProperty(sExtendedAttName);
                                }
                            }
                            else if (statType == CostBenefitStatistic01.MEAN)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetMeanCostsProperty(sExtendedAttName);
                                if (string.IsNullOrEmpty(sPropertyValue))
                                {
                                    sPropertyValue
                                        = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetMeanBenefitsProperty(sExtendedAttName);
                                }
                            }
                            else if (statType == CostBenefitStatistic01.SD)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetStdDevCostsProperty(sExtendedAttName);
                                if (string.IsNullOrEmpty(sPropertyValue))
                                {
                                    sPropertyValue
                                        = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetStdDevBenefitsProperty(sExtendedAttName);
                                }
                            }
                            else if (statType == CostBenefitStatistic01.VAR2)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetVarianceCostsProperty(sExtendedAttName);
                                if (string.IsNullOrEmpty(sPropertyValue))
                                {
                                    sPropertyValue
                                        = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetVarianceBenefitsProperty(sExtendedAttName);
                                }
                            }
                            else if (statType == CostBenefitStatistic01.MED)
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetMedianCostsProperty(sExtendedAttName);
                                if (string.IsNullOrEmpty(sPropertyValue))
                                {
                                    sPropertyValue
                                        = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetMedianBenefitsProperty(sExtendedAttName);
                                }
                            }
                            else
                            {
                                sPropertyValue
                                    = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetTotalCostsProperty(attName);
                                if (string.IsNullOrEmpty(sPropertyValue))
                                {
                                    sPropertyValue
                                        = this.CalcParams.AnalyzerParms.CostStatistics[filePosition][nodePosition]
                                    .GetTotalBenefitsProperty(attName);
                                }
                            }

                        }
                    }
                }
            }
            return sPropertyValue;
        }
        private void SetNameProperties(CostBenefitStatistic01 stat,
            XElement currentElement)
        {
            if (this.CalcParams.AnalyzerParms.AnalyzerGeneralType
                == ANALYZER_GENERAL_TYPES.outputbased)
            {
                //need to set the existing name and unit to a stat name and unit
                //(if it hasn't already been done)
                string sName = CalculatorHelpers
                    .GetAttribute(currentElement, CostBenefitStatistic01.TRName);
                if (string.IsNullOrEmpty(sName))
                {
                    //analyzers sometimes use more than one analyzer
                    //so the first may or may not have converted the name already
                    sName = CalculatorHelpers
                        .GetAttribute(currentElement, Calculator1.cName);
                }
                stat.TotalRName = sName;
                string sUnit = CalculatorHelpers
                    .GetAttribute(currentElement, CostBenefitStatistic01.TRUnit);
                if (string.IsNullOrEmpty(sUnit))
                {
                    sUnit = CalculatorHelpers
                        .GetAttribute(currentElement, Constants.UNIT);
                    if (string.IsNullOrEmpty(sUnit))
                    {
                        sUnit = CalculatorHelpers
                        .GetAttribute(currentElement, Output.OUTPUT_BASE_UNIT);
                    }
                }
                stat.TotalRUnit = sUnit;
            }
        }
    }
}
