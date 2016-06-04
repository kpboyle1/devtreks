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
    ///             NPV1Stock.Total1
    ///             The class aggregates npv totals. Less data to analyze than stats.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///NOTES:       Provides aggregated NPV totals
    ///</summary>
    public class NPV1Total1 : NPV1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public NPV1Total1()
            : base()
        {
            //subprice object
            InitTotalNPV1Total1Properties(this);
        }
        //copy constructor
        public NPV1Total1(NPV1Total1 calculator)
            : base (calculator)
        {
            CopyTotalNPV1Total1Properties(this, calculator);
        }
        
        public void InitTotalNPV1Total1Properties(NPV1Total1 ind)
        {
            ind.ErrorMessage = string.Empty;
            //includes summary data
            ind.InitTotalBenefitsProperties();
            ind.InitTotalCostsProperties();
            ind.CalcParameters = new CalculatorParameters();
        }
       
        public void CopyTotalNPV1Total1Properties(NPV1Total1 ind,
            NPV1Stock calculator)
        {
            if (calculator != null)
            {
                ind.ErrorMessage = calculator.ErrorMessage;
                //includes summary data
                ind.CopyCalculatorProperties(calculator);
                ind.CopyTotalBenefitsProperties(calculator);
                ind.CopyTotalCostsProperties(calculator);
                if (calculator.CalcParameters == null)
                    calculator.CalcParameters = new CalculatorParameters();
                if (ind.CalcParameters == null)
                    ind.CalcParameters = new CalculatorParameters();
                ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            }
        }
        public void CopyTotalNPV1Total1RProperties(NPV1Total1 calculator)
        {
            if (calculator != null)
            {
                this.CopyTotalBenefitsPsandQsProperties(calculator);
            }
        }
        public void SetTotalNPV1Total1Properties(NPV1Total1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.SetTotalBenefitsProperties(attNameExtension, calculator);
            ind.SetTotalCostsProperties(attNameExtension, calculator);
        }
        
        public void SetTotalNPV1Total1Property(NPV1Total1 ind,
            string attName, string attValue)
        {
            ind.SetTotalBenefitsProperty(attName, attValue);
            ind.SetTotalCostsProperty(attName, attValue);
        }
      
        public string GetTotalNPV1Total1Property(NPV1Total1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            ind.GetTotalBenefitsProperty(attName);
            ind.GetTotalCostsProperty(attName);
            return sPropertyValue;
        }

        public void SetTotalNPV1Total1Attributes(NPV1Total1 ind, string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                ind.SetTotalCostsSummaryAttributes(attNameExtension, ref writer);
                if (this.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    //limited ps and qs
                    ind.SetTotalOCPricePsandQsAttributes(attNameExtension, ref writer);
                }
                else
                {
                    if (this.CalcParameters.SubApplicationType != Constants.SUBAPPLICATION_TYPES.componentprices
                        && this.CalcParameters.SubApplicationType != Constants.SUBAPPLICATION_TYPES.operationprices)
                    {
                        ind.SetTotalCostsSummaryNetsAttributes(attNameExtension, ref writer);
                    }
                    if (this.CalcParameters.CurrentElementNodeName.Contains(Input.INPUT_PRICE_TYPES.input.ToString()))
                    {
                        //limited ps and qs
                        ind.SetTotalCostsPsandQsAttributes(attNameExtension, ref writer);
                    }
                }
            }
            if (bIsBenefitNode || bIsBoth)
            {
                ind.SetTotalBenefitsSummaryAttributes(attNameExtension, ref writer);
                ind.SetTotalBenefitsPsandQsAttributes(attNameExtension, ref writer);
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
            bHasAnalyses = SetBaseIOAnalyses(npv1Stock);
            return bHasAnalyses;
        }
        //current analyzer does not need this and isn't used
        private bool SetBaseIOAnalyses(NPV1Stock npv1Stock)
        {
            bool bHasAnalysis = false;
            //initial calculators are added to stock.Total1, stock.Stat1 ...
            if (npv1Stock.Total1 != null)
            {
                //will double count first member, so skip sending first member of collections here
                npv1Stock.Total1.TotalAMOC += npv1Stock.Total1.TotalOC;
                npv1Stock.Total1.TotalAMAOH += npv1Stock.Total1.TotalAOH;
                npv1Stock.Total1.TotalAMCAP += npv1Stock.Total1.TotalCAP;
                //this is not plus equal
                npv1Stock.Total1.TotalAMTOTAL += npv1Stock.Total1.TotalAMOC + npv1Stock.Total1.TotalAMAOH + npv1Stock.Total1.TotalAMCAP;
                //when machinery, lca, or meaningful unit amounts are on hand (no input.Times in base Inputs)
                npv1Stock.Total1.TotalOCAmount += npv1Stock.Total1.TotalOCAmount;
                npv1Stock.Total1.TotalOCPrice += npv1Stock.Total1.TotalOCPrice;
                //benefits
                npv1Stock.Total1.TotalAMR += npv1Stock.Total1.TotalR;
                //cost effectiveness analysis (no output.compositionamount in base Outputs)
                npv1Stock.Total1.TotalRAmount += npv1Stock.Total1.TotalRAmount;
                npv1Stock.Total1.TotalRPrice += npv1Stock.Total1.TotalRPrice;
                npv1Stock.Total1.TotalRCompositionAmount += npv1Stock.Total1.TotalRCompositionAmount;
            }
            bHasAnalysis = true;
            return bHasAnalysis;
        }
        //run the analyses for everything else 
        //keep this for future use in case more data needs to be taken
        //out of input and output calcs
        public bool RunAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //don't use npv1Stock totals, use calcs
            SetAnalyses(npv1Stock);
            //consistent pattern followed by LCA, NPV, M&E
            bHasAnalyses = SetAnalyses(npv1Stock, calcs);
            return bHasAnalyses;
        }
        private bool SetAnalyses(NPV1Stock npv1Stock)
        {
            bool bHasAnalysis = false;
            npv1Stock.Total1.CalcParameters = npv1Stock.CalcParameters;
            //totals were added to npv1stock, but those totals result 
            //in double counting when calcs are being summed
            //set them to zero
            npv1Stock.Total1.InitTotalBenefitsProperties();
            npv1Stock.Total1.InitTotalCostsProperties();
            npv1Stock.Total1.TotalOCAmount = 0;
            npv1Stock.Total1.TotalOCPrice = 0;
            npv1Stock.Total1.TotalRAmount = 0;
            //times is already in comp amount
            npv1Stock.Total1.TotalRCompositionAmount = 0;
            npv1Stock.Total1.TotalRPrice = 0;
            bHasAnalysis = true;
            return bHasAnalysis;
        }
        private bool SetAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of npv1stocks for base element
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(npv1Stock.GetType()))
                {
                    NPV1Stock stock = (NPV1Stock)calc;
                    if (stock != null)
                    {
                        //make sure it already has a total1 (set by SetBaseIO, or during InitTotal1)
                        if (stock.Total1 != null)
                        {
                            npv1Stock.Total1.CalcParameters = new CalculatorParameters(stock.Total1.CalcParameters);
                            //npv1Stock.Total1.CalcParameters = npv1Stock.CalcParameters;
                            //these totals were multiplied in base npv calcs
                            //don't use calc.Multiplier again
                            double iMultiplier = 1;
                            //these need to be multiplied because arbitrary ocstocks are being aggregated
                            //into time periods; the timeperiod totals can't be used
                            AddSubTotalToTotalStock(npv1Stock.Total1,
                                iMultiplier, stock.Total1);
                            bHasTotals = true;
                            if (bHasTotals)
                            {
                                bHasAnalysis = true;
                            }
                        }
                    }
                }
            }
            return bHasAnalysis;
        }
        public bool AddSubTotalToTotalStock(NPV1Total1 totStock, double multiplier,
           NPV1Total1 stock)
        {
            bool bHasCalculations = false;
            //costs
            totStock.TotalAMOC += stock.TotalAMOC * multiplier;
            totStock.TotalAMAOH += stock.TotalAMAOH * multiplier;
            totStock.TotalAMCAP += stock.TotalAMCAP * multiplier;
            totStock.TotalAMINCENT += stock.TotalAMINCENT * multiplier;
            totStock.TotalAMTOTAL += stock.TotalAMTOTAL * multiplier;
            //these are only used with inputprices but no harm in setting them for other apps
            totStock.TotalOCAmount += stock.TotalOCAmount * multiplier;
            totStock.TotalOCPrice += stock.TotalOCPrice;
            totStock.TotalOCName = stock.TotalOCName;
            totStock.TotalOCUnit = stock.TotalOCUnit;
            //benefits
            totStock.TotalAMR += stock.TotalAMR * multiplier;
            totStock.TotalAMRINCENT += stock.TotalAMRINCENT * multiplier;
            //totals are on always on hand (because BIAnalyzer sets them for the initial output)
            //cost effectiveness analysis 
            totStock.TotalRAmount += stock.TotalRAmount * multiplier;
            //compos is independent of ramount and can be used to study head of cattle ...
            totStock.TotalRCompositionAmount += stock.TotalRCompositionAmount * multiplier;
            //don't adjust prices by multiplier
            totStock.TotalRPrice += stock.TotalRPrice;
            //display the r (ancestors of outs put name in first calc)
            if (!string.IsNullOrEmpty(stock.TotalRName))
            {
                totStock.TotalRName = stock.TotalRName;
                totStock.TotalRUnit = stock.TotalRUnit;
                totStock.TotalRCompositionUnit = stock.TotalRCompositionUnit;
            }
            //nets
            totStock.TotalAMNET = totStock.TotalAMR - totStock.TotalAMTOTAL;
            totStock.TotalAMINCENT_NET = totStock.TotalAMRINCENT - totStock.TotalAMINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
    }
}