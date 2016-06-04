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
    ///             Costs: LCA1Stock.Total1.SubP1Stock.SubStock1s.SubPrice1s
    ///             Benefits: LCA1Stock.Total1.SubP2Stock.SubStock2s.SubPrice1s
    ///             The class aggregates lcas.
    ///Author:		www.devtreks.org
    ///Date:		2013, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class LCA1Total1 : LCA1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public LCA1Total1()
            : base()
        {
            //subprice object
            InitTotalLCA1Total1Properties(this);
        }
        //copy constructor
        public LCA1Total1(LCA1Total1 calculator)
            : base (calculator)
        {
            CopyTotalLCA1Total1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent LCA1Stock
        //calculator properties
        public double TotalOCCost { get; set; }
        public double TotalAOHCost { get; set; }
        public double TotalCAPCost { get; set; }
        //total lcc cost
        public double TotalLCCCost { get; set; }
        //total eaa cost (equiv ann annuity)
        public double TotalEAACost { get; set; }
        //total per unit costs
        public double TotalUnitCost { get; set; }

        //options and salvage value taken from other capital inputs
        private const string cTotalOCCost = "TOCCost";
        private const string cTotalAOHCost = "TAOHCost";
        private const string cTotalCAPCost = "TCAPCost";
        private const string cTotalLCCCost = "TLCCCost";
        private const string cTotalEAACost = "TEAACost";
        private const string cTotalUnitCost = "TUnitCost";

        //benefits
        //totals, including initbens, salvageval, replacement, and subcosts
        public double TotalRBenefit { get; set; }
        //total lcb benefit
        public double TotalLCBBenefit { get; set; }
        //total eaa benefit (equiv ann annuity)
        public double TotalREAABenefit { get; set; }
        //total per unit benefits
        public double TotalRUnitBenefit { get; set; }

        //options and salvage value taken from other capital inputs
        private const string cTotalRBenefit = "TRBenefit";
        private const string cTotalLCBBenefit = "TLCBBenefit";
        private const string cTotalREAABenefit = "TREAABenefit";
        private const string cTotalRUnitBenefit = "TRUnitBenefit";
        
        public void InitTotalLCA1Total1Properties(LCA1Total1 ind)
        {
            ind.ErrorMessage = string.Empty;
            ind.TotalOCCost = 0;
            ind.TotalAOHCost = 0;
            ind.TotalCAPCost = 0;
            ind.TotalLCCCost = 0;
            ind.TotalEAACost = 0;
            ind.TotalUnitCost = 0;
            ind.TotalRBenefit = 0;
            ind.TotalLCBBenefit = 0;
            ind.TotalREAABenefit = 0;
            ind.TotalRUnitBenefit = 0;
            ind.CalcParameters = new CalculatorParameters();
            ind.SubP1Stock = new SubPrice1Stock();
            ind.SubP2Stock = new SubPrice2Stock();
        }
       
        public void CopyTotalLCA1Total1Properties(LCA1Total1 ind,
            LCA1Total1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalOCCost = calculator.TotalOCCost;
            ind.TotalAOHCost = calculator.TotalAOHCost;
            ind.TotalCAPCost = calculator.TotalCAPCost;
            ind.TotalLCCCost = calculator.TotalLCCCost;
            ind.TotalEAACost = calculator.TotalEAACost;
            ind.TotalUnitCost = calculator.TotalUnitCost;
            ind.TotalRBenefit = calculator.TotalRBenefit;
            ind.TotalLCBBenefit = calculator.TotalLCBBenefit;
            ind.TotalREAABenefit = calculator.TotalREAABenefit;
            ind.TotalRUnitBenefit = calculator.TotalRUnitBenefit;
            if (calculator.CalcParameters == null)
                calculator.CalcParameters = new CalculatorParameters();
            if (ind.CalcParameters == null)
                ind.CalcParameters = new CalculatorParameters();
            ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            if (calculator.SubP1Stock == null)
                calculator.SubP1Stock = new SubPrice1Stock();
            if (ind.SubP1Stock == null)
                ind.SubP1Stock = new SubPrice1Stock();
            ind.SubP1Stock.CopyTotalSubPrice1StocksProperties(calculator.SubP1Stock);
            if (calculator.SubP2Stock == null)
                calculator.SubP2Stock = new SubPrice2Stock();
            if (ind.SubP2Stock == null)
                ind.SubP2Stock = new SubPrice2Stock();
            ind.SubP2Stock.CopyTotalSubPrice2StocksProperties(calculator.SubP2Stock);
        }

        public void SetTotalLCA1Total1Properties(LCA1Total1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalOCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCCost, attNameExtension));
            ind.TotalAOHCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHCost, attNameExtension));
            ind.TotalCAPCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPCost, attNameExtension));
            ind.TotalLCCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCCost, attNameExtension));
            ind.TotalEAACost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAACost, attNameExtension));
            ind.TotalUnitCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitCost, attNameExtension));
            ind.TotalRBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRBenefit, attNameExtension));
            ind.TotalLCBBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBBenefit, attNameExtension));
            ind.TotalREAABenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAABenefit, attNameExtension));
            ind.TotalRUnitBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitBenefit, attNameExtension));
        }
    
        public void SetTotalLCA1Total1Property(LCA1Total1 ind,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cTotalOCCost:
                    ind.TotalOCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHCost:
                    ind.TotalAOHCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPCost:
                    ind.TotalCAPCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCCost:
                    ind.TotalLCCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAACost:
                    ind.TotalEAACost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitCost:
                    ind.TotalUnitCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRBenefit:
                    ind.TotalRBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBBenefit:
                    ind.TotalLCBBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAABenefit:
                    ind.TotalREAABenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitBenefit:
                    ind.TotalRUnitBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
      
        public string GetTotalLCA1Total1Property(LCA1Total1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalOCCost:
                    sPropertyValue = ind.TotalOCCost.ToString();
                    break;
                case cTotalAOHCost:
                    sPropertyValue = ind.TotalAOHCost.ToString();
                    break;
                case cTotalCAPCost:
                    sPropertyValue = ind.TotalCAPCost.ToString();
                    break;
                case cTotalLCCCost:
                    sPropertyValue = ind.TotalLCCCost.ToString();
                    break;
                case cTotalEAACost:
                    sPropertyValue = ind.TotalEAACost.ToString();
                    break;
                case cTotalUnitCost:
                    sPropertyValue = ind.TotalUnitCost.ToString();
                    break;
                case cTotalRBenefit:
                    sPropertyValue = ind.TotalRBenefit.ToString();
                    break;
                case cTotalLCBBenefit:
                    sPropertyValue = ind.TotalLCBBenefit.ToString();
                    break;
                case cTotalREAABenefit:
                    sPropertyValue = ind.TotalREAABenefit.ToString();
                    break;
                case cTotalRUnitBenefit:
                    sPropertyValue = ind.TotalRUnitBenefit.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        
        public virtual void SetTotalLCA1Total1Attributes(LCA1Total1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost);
            }
            if (bIsBenefitNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit);
                CalculatorHelpers.SetAttributeDoubleF2(calculator,
                     string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit);
            }
        }
        
        public void SetTotalLCA1Total1Attributes(LCA1Total1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                       string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost.ToString("f2"));
                writer.WriteAttributeString(
                       string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost.ToString("f2"));
                writer.WriteAttributeString(
                      string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost.ToString("f2"));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost.ToString("f2"));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost.ToString("f2"));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost.ToString("f2"));
            }
            if (bIsBenefitNode || bIsBoth)
            {
                writer.WriteAttributeString(
                      string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit.ToString("f2"));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit.ToString("f2"));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit.ToString("f2"));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit.ToString("f2"));
            }
        }
        //run the analyses for inputs an outputs
        public bool RunAnalyses(LCA1Stock lca1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (
            bHasAnalyses = SetAnalyses(lca1Stock);
            return bHasAnalyses;
        }
        //run the analyes for everything else 
        //descendentstock holds input and output stock totals and calculators
        public bool RunAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to lca1stock.Total1
            if (lca1Stock.Total1 == null)
            {
                lca1Stock.Total1 = new LCA1Total1();
            }
            //need one property set
            lca1Stock.Total1.SubApplicationType = lca1Stock.CalcParameters.SubApplicationType.ToString();
            bHasAnalyses = SetAnalyses(lca1Stock, calcs);
            return bHasAnalyses;
        }
        private bool SetAnalyses(LCA1Stock lca1Stock)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            bool adjustTotals = true;
            //only the totStocks are used to store numerical results
            //calcprops and analyzerprops stored in lca1stock
            lca1Stock.Total1 = new LCA1Total1();
            //need one property set
            lca1Stock.Total1.SubApplicationType = lca1Stock.CalcParameters.SubApplicationType.ToString();
            //these are the lcc and lcb calculations
            //the initial aggregation must have serialized them correctly as lcc or lcb calcors
            //costs
            foreach (SubPrice1 ind in lca1Stock.SubP1Stock.SubPrice1s)
            {
                if (ind.CalculatorType
                    == LCA1CalculatorHelper.CALCULATOR_TYPES.buildcost1.ToString())
                {
                    LCC1Calculator lccInput = (LCC1Calculator)ind;
                    //ind.SubPrice1s holds the subprices collection (which must also be totaled)
                    bHasTotals = AddCostToTotalStock(lca1Stock.Total1, lca1Stock.Multiplier,
                        lccInput, adjustTotals);
                    //stock needs some calculator properties (date)
                    BILCA1StockAnalyzer.CopyBaseElementProperties(lccInput.LCCInput, lca1Stock);
                    lca1Stock.Date = lccInput.LCCInput.Date;
                    if (bHasTotals)
                    {
                        bHasAnalysis = true;
                    }
                }
            }
            //benefits
            foreach (SubPrice1 ind in lca1Stock.SubP2Stock.SubPrice1s)
            {
                if (ind.CalculatorType
                    == LCA1CalculatorHelper.CALCULATOR_TYPES.buildbenefit1.ToString())
                {
                    LCB1Calculator lcbOutput = (LCB1Calculator)ind;
                    //ind.SubPrice1s holds the subprices collection (which must also be totaled)
                    bHasTotals = AddBenefitToTotalStock(lca1Stock.Total1, lca1Stock.Multiplier,
                        lcbOutput, adjustTotals);
                    //stock needs some calculator properties (date)
                    BILCA1StockAnalyzer.CopyBaseElementProperties(lcbOutput.LCBOutput, lca1Stock);
                    //lca1Stock.Date = lcbOutput.LCBOutput.Date;
                    if (bHasTotals)
                    {
                        bHasAnalysis = true;
                    }
                }
            }
            return bHasAnalysis;
        }
        private bool SetAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of lca1stocks for each input and output
            //object model is calc.Total1.SubPrice1Stocks
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(lca1Stock.GetType()))
                {
                    LCA1Stock stock = (LCA1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Total1 != null)
                        {
                            //calc holds an input or output stock
                            //add that stock to lca1stock (some analyses will need to use subprices too)
                            bHasTotals = AddSubTotalToTotalStock(lca1Stock.Total1, stock.Multiplier, stock.Total1);
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
        public bool AddCostToTotalStock(LCA1Total1 totStock, double multiplier,
            LCC1Calculator lccInput, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (input.times)
                //don't change per hour building costs, only total costs
                if (totStock.SubApplicationType
                        == Constants.SUBAPPLICATION_TYPES.inputprices.ToString())
                {
                    //i.e. lccInput.cost = lccInput.cost * multiplier (1 in stock analyzers)
                    ChangeInputByMultiplier(lccInput, multiplier);
                }
                else
                {
                    //i.e. lccInput.cost = lccInput.cost * multiplier * input.ocamount
                    //multiplier = input.times * oc.amount * tp.amount
                    ChangeInputByInputMultipliers(lccInput, multiplier);
                }
            }
            //multiplier adjusted costs
            totStock.TotalOCCost += lccInput.OCTotalCost;
            totStock.TotalAOHCost += lccInput.AOHTotalCost;
            totStock.TotalCAPCost += lccInput.CAPTotalCost;
            totStock.TotalLCCCost += lccInput.LCCTotalCost;
            totStock.TotalEAACost += lccInput.EAATotalCost;
            //unit cost is derived from totstock
            totStock.TotalUnitCost = totStock.TotalLCCCost / lccInput.PerUnitAmount;
            //subcosts
            AddSubCosts(totStock, lccInput);
            bHasCalculations = true;
            return bHasCalculations;
        }

        private void AddSubCosts(LCA1Total1 totStock, LCC1Calculator lccInput)
        {
            foreach (SubPrice1 subprice in lccInput.SubPrice1s)
            {
                AddSubCost(totStock, subprice, lccInput);
            }
        }
        private void AddSubCost(LCA1Total1 totStock, SubPrice1 subprice, LCC1Calculator lccInput)
        {
            //make sure that each subprice has a corresponding stock
            totStock.SubP1Stock.AddSubPrice1ToStocks(subprice);
            //add cumulative totals (material, equipment, labor)
            foreach (SubPrice1Stock stock in totStock.SubP1Stock.SubPrice1Stocks)
            {
                if ((stock.TotalSubP1Label == subprice.SubPLabel
                    && subprice.SubPLabel != string.Empty))
                {
                    stock.TotalSubP1Total += subprice.SubPTotal;
                    stock.TotalSubP1Price += subprice.SubPPrice;
                    stock.TotalSubP1Amount += subprice.SubPAmount;
                    stock.TotalSubP1TotalPerUnit = stock.TotalSubP1Total / lccInput.PerUnitAmount;
                }
            }
        }

        public bool AddBenefitToTotalStock(LCA1Total1 totStock, double multiplier,
            LCB1Calculator lcbOutput, bool adjustTotals)
        {
            bool bHasCalculations = false;
            //inheriting classes usually run this class, but adjust their own totals
            if (adjustTotals)
            {
                //multipliers (output.times, out.compositionamount)
                //i.e. buildCostOutput.cost = output.amount * multiplier
                ChangeOutputByOutputMultipliers(lcbOutput, multiplier);
            }
            //multiplier adjusted benefits
            totStock.TotalRBenefit += lcbOutput.RTotalBenefit;
            totStock.TotalLCBBenefit += lcbOutput.LCBTotalBenefit;
            totStock.TotalREAABenefit += lcbOutput.EAATotalBenefit;
            //unit benefit is derived from totstock
            totStock.TotalRUnitBenefit = totStock.TotalLCBBenefit / lcbOutput.PerUnitAmount;
            //subbenefits
            AddSubBenefits(totStock, lcbOutput);
            bHasCalculations = true;
            return bHasCalculations;
        }
        private void AddSubBenefits(LCA1Total1 totStock, LCB1Calculator lcbOutput)
        {
            foreach (SubPrice1 subprice in lcbOutput.SubPrice1s)
            {
                AddSubBenefit(totStock, subprice, lcbOutput);
            }
        }
        private void AddSubBenefit(LCA1Total1 totStock, SubPrice1 subprice, LCB1Calculator lcbOutput)
        {
            //make sure that each subprice has a corresponding stock2
            //stock2 distinguishes benefits from costs in budget elements
            totStock.SubP2Stock.AddSubPrice2ToStocks(subprice);
            //add cumulative totals (rentals, commodities, sales)
            foreach (SubPrice2Stock stock in totStock.SubP2Stock.SubPrice2Stocks)
            {
                if ((stock.TotalSubP2Label == subprice.SubPLabel
                    && subprice.SubPLabel != string.Empty))
                {
                    stock.TotalSubP2Total += subprice.SubPTotal;
                    stock.TotalSubP2Price += subprice.SubPPrice;
                    stock.TotalSubP2Amount += subprice.SubPAmount;
                    stock.TotalSubP2TotalPerUnit = stock.TotalSubP2Total / lcbOutput.PerUnitAmount;
                }
            }
        }
        public bool AddSubTotalToTotalStock(LCA1Total1 totStock, double multiplier,
            LCA1Total1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            //multiplier adjusted costs
            totStock.TotalOCCost += subTotal.TotalOCCost;
            totStock.TotalAOHCost += subTotal.TotalAOHCost;
            totStock.TotalCAPCost += subTotal.TotalCAPCost;
            totStock.TotalLCCCost += subTotal.TotalLCCCost;
            totStock.TotalEAACost += subTotal.TotalEAACost;
            totStock.TotalUnitCost += subTotal.TotalUnitCost;
            totStock.TotalRBenefit += subTotal.TotalRBenefit;
            totStock.TotalLCBBenefit += subTotal.TotalLCBBenefit;
            totStock.TotalREAABenefit += subTotal.TotalREAABenefit;
            totStock.TotalRUnitBenefit += subTotal.TotalRUnitBenefit;
            //cost subtotals
            AddSubStock1Totals(totStock, subTotal);
            //benefit subtotals
            AddSubStock2Totals(totStock, subTotal);
            bHasCalculations = true;
            return bHasCalculations;
        }

        private void AddSubStock1Totals(LCA1Total1 totStock, LCA1Total1 subTotal)
        {
            //cost object model: total1.SubPrice1Stocks
            if (subTotal.SubP1Stock.SubPrice1Stocks != null)
            {
                foreach (SubPrice1Stock subpstock in subTotal.SubP1Stock.SubPrice1Stocks)
                {
                    //make sure that each substock has a corresponding stock
                    //can't use totStock.SubP1Stock.AddPrice1ToStock cause stocks are needed
                    totStock.AddSubStock1ToTotalStocks(subpstock);
                    //add cumulative totals (material, equipment, labor)
                    foreach (SubPrice1Stock stock in totStock.SubP1Stock.SubPrice1Stocks)
                    {
                        if ((stock.TotalSubP1Label == subpstock.TotalSubP1Label
                            && subpstock.TotalSubP1Label != string.Empty))
                        {
                            stock.TotalSubP1TotalPerUnit += subpstock.TotalSubP1TotalPerUnit;
                            stock.TotalSubP1Total += subpstock.TotalSubP1Total;
                            stock.TotalSubP1Price += subpstock.TotalSubP1Price;
                            stock.TotalSubP1Amount += subpstock.TotalSubP1Amount;
                        }
                    }
                }
            }
        }
        private static void CopySubStock1Totals(LCA1Total1 totStock, LCA1Total1 subTotal)
        {
            //cost object model: total1.SubPrice1Stocks
            if (subTotal.SubP1Stock.SubPrice1Stocks != null)
            {
                foreach (SubPrice1Stock subpstock in subTotal.SubP1Stock.SubPrice1Stocks)
                {
                    //make sure that each substock has a corresponding stock
                    //can't use totStock.SubP1Stock.AddPrice1ToStock cause stocks are needed
                    totStock.AddSubStock1ToTotalStocks(subpstock);
                    //add cumulative totals (material, equipment, labor)
                    foreach (SubPrice1Stock stock in totStock.SubP1Stock.SubPrice1Stocks)
                    {
                        if ((stock.TotalSubP1Label == subpstock.TotalSubP1Label
                            && subpstock.TotalSubP1Label != string.Empty))
                        {
                            stock.TotalSubP1TotalPerUnit += subpstock.TotalSubP1TotalPerUnit;
                            stock.TotalSubP1Total += subpstock.TotalSubP1Total;
                            stock.TotalSubP1Price += subpstock.TotalSubP1Price;
                            stock.TotalSubP1Amount += subpstock.TotalSubP1Amount;
                        }
                    }
                }
            }
        }
        private void AddSubStock2Totals(LCA1Total1 totStock, LCA1Total1 subTotal)
        {
            //benefits object model: total1.SubPrice2Stocks
            if (subTotal.SubP2Stock.SubPrice2Stocks != null)
            {
                foreach (SubPrice2Stock subpstock in subTotal.SubP2Stock.SubPrice2Stocks)
                {
                    //make sure that each substock has a corresponding stock
                    totStock.AddSubStock2ToTotalStocks(subpstock);
                    //add cumulative totals (material, equipment, labor)
                    foreach (SubPrice2Stock stock in totStock.SubP2Stock.SubPrice2Stocks)
                    {
                        if ((stock.TotalSubP2Label == subpstock.TotalSubP2Label
                            && subpstock.TotalSubP2Label != string.Empty))
                        {
                            stock.TotalSubP2TotalPerUnit += subpstock.TotalSubP2TotalPerUnit;
                            stock.TotalSubP2Total += subpstock.TotalSubP2Total;
                            stock.TotalSubP2Price += subpstock.TotalSubP2Price;
                            stock.TotalSubP2Amount += subpstock.TotalSubP2Amount;
                        }
                    }
                }
            }
        }
        private void CopySubStock2Totals(LCA1Total1 totStock, LCA1Total1 subTotal)
        {
            //benefits object model: total1.SubPrice2Stocks
            if (subTotal.SubP2Stock.SubPrice2Stocks != null)
            {
                foreach (SubPrice2Stock subpstock in subTotal.SubP2Stock.SubPrice2Stocks)
                {
                    //make sure that each substock has a corresponding stock
                    totStock.AddSubStock2ToTotalStocks(subpstock);
                    //add cumulative totals (material, equipment, labor)
                    foreach (SubPrice2Stock stock in totStock.SubP2Stock.SubPrice2Stocks)
                    {
                        if ((stock.TotalSubP2Label == subpstock.TotalSubP2Label
                            && subpstock.TotalSubP2Label != string.Empty))
                        {
                            stock.TotalSubP2TotalPerUnit += subpstock.TotalSubP2TotalPerUnit;
                            stock.TotalSubP2Total += subpstock.TotalSubP2Total;
                            stock.TotalSubP2Price += subpstock.TotalSubP2Price;
                            stock.TotalSubP2Amount += subpstock.TotalSubP2Amount;
                        }
                    }
                }
            }
        }
        public static void ChangeInputByMultiplier(LCC1Calculator lccInput,
            double multiplier)
        {
            lccInput.OCTotalCost = lccInput.OCTotalCost * multiplier;
            lccInput.AOHTotalCost = lccInput.AOHTotalCost * multiplier;
            lccInput.CAPTotalCost = lccInput.CAPTotalCost * multiplier;
            //multiplicative, so ok to multiply; but final number is summation
            lccInput.LCCTotalCost = lccInput.LCCTotalCost * multiplier;
            lccInput.EAATotalCost = lccInput.EAATotalCost * multiplier;
            //unit is derived from total
            lccInput.UnitTotalCost = lccInput.LCCTotalCost / lccInput.PerUnitAmount;
            //subcosts
            foreach (SubPrice1 subprice in lccInput.SubPrice1s)
            {
                subprice.SubPTotal = subprice.SubPTotal * multiplier;
                subprice.SubPTotalPerUnit = subprice.SubPTotalPerUnit / lccInput.PerUnitAmount;
            }
        }
        public static void ChangeInputByInputMultipliers(LCC1Calculator lccInput,
            double multiplier)
        {
            //total cost already was multiplied by amount now needs to be multiplied by times
            lccInput.OCTotalCost = lccInput.OCTotalCost * lccInput.LCCInput.OCAmount * multiplier;
            lccInput.AOHTotalCost = lccInput.AOHTotalCost * lccInput.LCCInput.AOHAmount * multiplier;
            lccInput.CAPTotalCost = lccInput.CAPTotalCost * lccInput.LCCInput.CAPAmount * multiplier;
            //recalculate total costs
            lccInput.LCCTotalCost = lccInput.OCTotalCost
                + lccInput.AOHTotalCost + lccInput.CAPTotalCost;
            lccInput.EAATotalCost = GeneralRules.CalculateEquivalentAnnualAnnuity(lccInput.LCCTotalCost,
                lccInput.ServiceLifeYears, lccInput.LCCInput.Local.RealRate, lccInput.LCCInput.Local.NominalRate);
            lccInput.UnitTotalCost = lccInput.LCCTotalCost / lccInput.PerUnitAmount;
            //subcosts can use all three price amounts
            foreach (SubPrice1 subprice in lccInput.SubPrice1s)
            {
                subprice.SubPTotal = GetMultipliedTotal(lccInput, subprice.SubPTotal, subprice.SubPType, multiplier);
                subprice.SubPTotalPerUnit = subprice.SubPTotal / lccInput.PerUnitAmount;
                //display the multiplier-adjusted quantity of each subprice1
                //this number can be used directly in statistical aggregations
                subprice.SubPAmount = GetMultipliedTotal(lccInput, subprice.SubPAmount, subprice.SubPType, multiplier);
            }
        }
        private static double GetMultipliedTotal(LCC1Calculator lccInput,
            double total, string priceType, double multiplier)
        {
            double dbMultipliedTotal = total;
            if (priceType == SubPrices.PRICE_TYPES.oc.ToString())
            {
                dbMultipliedTotal = total * lccInput.LCCInput.OCAmount * multiplier;
            }
            else if (priceType == SubPrices.PRICE_TYPES.aoh.ToString())
            {
                dbMultipliedTotal = total * lccInput.LCCInput.AOHAmount * multiplier;
            }
            else if (priceType == SubPrices.PRICE_TYPES.cap.ToString())
            {
                dbMultipliedTotal = total * lccInput.LCCInput.CAPAmount * multiplier;
            }
            else
            {
                dbMultipliedTotal = total * multiplier;
            }
            return dbMultipliedTotal;
        }
        
        public static void ChangeOutputByOutputMultipliers(LCB1Calculator lcbOutput,
            double multiplier)
        {
            //lcbOutput.Amount is included to stay consistent with how input multipliers are used
            lcbOutput.RTotalBenefit = lcbOutput.RTotalBenefit * lcbOutput.LCBOutput.Amount * multiplier;
            //multiplicative, so ok to multiply; but final number is summation
            lcbOutput.LCBTotalBenefit = lcbOutput.LCBTotalBenefit * lcbOutput.LCBOutput.Amount * multiplier;
            lcbOutput.EAATotalBenefit = lcbOutput.EAATotalBenefit * lcbOutput.LCBOutput.Amount * multiplier;
            //unit totals are derived
            lcbOutput.UnitTotalBenefit = lcbOutput.LCBTotalBenefit / lcbOutput.PerUnitAmount;
            //subbenefits
            foreach (SubPrice1 subprice in lcbOutput.SubPrice1s)
            {
                subprice.SubPTotal = GetMultipliedTotal(lcbOutput, subprice.SubPTotal, subprice.SubPType, multiplier);
                subprice.SubPTotalPerUnit = subprice.SubPTotal / lcbOutput.PerUnitAmount;
                //display the multiplier-adjusted quantity of each subprice1
                //this number can be used directly in statistical aggregations
                subprice.SubPAmount = GetMultipliedTotal(lcbOutput, subprice.SubPAmount, subprice.SubPType, multiplier);
            }
        }
        private static double GetMultipliedTotal(LCB1Calculator lcbOutput,
            double total, string priceType, double multiplier)
        {
            double dbMultipliedTotal = total;
            if (priceType == SubPrices.PRICE_TYPES.rev.ToString())
            {
                dbMultipliedTotal = total * lcbOutput.LCBOutput.Amount * multiplier;
            }
            else
            {
                //base table uses all three
                dbMultipliedTotal = total * lcbOutput.LCBOutput.Amount * multiplier;
            }
            return dbMultipliedTotal;
        }

        public static void ChangeSubTotalByMultipliers(LCA1Total1 subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
            subTotal.TotalOCCost = subTotal.TotalOCCost * multiplier;
            subTotal.TotalAOHCost = subTotal.TotalAOHCost * multiplier;
            subTotal.TotalCAPCost = subTotal.TotalCAPCost * multiplier;
            subTotal.TotalLCCCost = subTotal.TotalLCCCost * multiplier;
            subTotal.TotalEAACost = subTotal.TotalEAACost * multiplier;
            subTotal.TotalUnitCost = subTotal.TotalUnitCost * multiplier;
            subTotal.TotalRBenefit = subTotal.TotalRBenefit * multiplier;
            subTotal.TotalLCBBenefit = subTotal.TotalLCBBenefit * multiplier;
            subTotal.TotalREAABenefit = subTotal.TotalREAABenefit * multiplier;
            subTotal.TotalRUnitBenefit = subTotal.TotalRUnitBenefit * multiplier;
            if (subTotal.SubP1Stock.SubPrice1Stocks != null)
            {
                foreach (SubPrice1Stock stock in subTotal.SubP1Stock.SubPrice1Stocks)
                {
                    stock.TotalSubP1TotalPerUnit = stock.TotalSubP1TotalPerUnit * multiplier;
                    stock.TotalSubP1Total = stock.TotalSubP1Total * multiplier;
                    //do not change price or amount 
                }
            }
            if (subTotal.SubP2Stock.SubPrice2Stocks != null)
            {
                foreach (SubPrice2Stock stock in subTotal.SubP2Stock.SubPrice2Stocks)
                {
                    stock.TotalSubP2TotalPerUnit = stock.TotalSubP2TotalPerUnit * multiplier;
                    stock.TotalSubP2Total = stock.TotalSubP2Total * multiplier;
                    //do not change price or amount 
                }
            }
        }
    }
    public static class LCA1Total1Extensions
    {
        public static void AddSubStock1ToTotalStocks(this LCA1Total1 baseStat, SubPrice1Stock substock)
        {
            //make sure that each subprice has a corresponding stock
            if (baseStat.SubP1Stock.SubPrice1Stocks == null)
            {
                baseStat.SubP1Stock.SubPrice1Stocks = new List<SubPrice1Stock>();
            }
            if (!baseStat.SubP1Stock.SubPrice1Stocks
                .Any(s => s.TotalSubP1Label == substock.TotalSubP1Label))
            {
                if (substock.TotalSubP1Label != string.Empty)
                {
                    SubPrice1Stock stock = new SubPrice1Stock();
                    stock.TotalSubP1Label = substock.TotalSubP1Label;
                    stock.TotalSubP1Name = substock.TotalSubP1Name;
                    stock.TotalSubP1Unit = substock.TotalSubP1Unit;
                    stock.TotalSubP1Description = substock.TotalSubP1Description;
                    baseStat.SubP1Stock.SubPrice1Stocks.Add(stock);
                }
            }
            else
            {
                //update the identifiers in case they have changed
                SubPrice1Stock stock = baseStat.SubP1Stock.SubPrice1Stocks
                    .FirstOrDefault(s => s.TotalSubP1Label == substock.TotalSubP1Label);
                if (stock != null)
                {
                    stock.TotalSubP1Label = substock.TotalSubP1Label;
                    stock.TotalSubP1Name = substock.TotalSubP1Name;
                    stock.TotalSubP1Unit = substock.TotalSubP1Unit;
                    stock.TotalSubP1Description = substock.TotalSubP1Description;
                }
            }
        }
        public static void AddSubStock2ToTotalStocks(this LCA1Total1 baseStat, SubPrice2Stock substock)
        {
            //make sure that each subprice has a corresponding stock
            if (baseStat.SubP2Stock.SubPrice2Stocks == null)
            {
                baseStat.SubP2Stock.SubPrice2Stocks = new List<SubPrice2Stock>();
            }
            if (!baseStat.SubP2Stock.SubPrice2Stocks
                .Any(s => s.TotalSubP2Label == substock.TotalSubP2Label))
            {
                if (substock.TotalSubP2Label != string.Empty)
                {
                    SubPrice2Stock stock = new SubPrice2Stock();
                    stock.TotalSubP2Label = substock.TotalSubP2Label;
                    stock.TotalSubP2Name = substock.TotalSubP2Name;
                    stock.TotalSubP2Unit = substock.TotalSubP2Unit;
                    stock.TotalSubP2Description = substock.TotalSubP2Description;
                    baseStat.SubP2Stock.SubPrice2Stocks.Add(stock);
                }
            }
            else
            {
                //update the identifiers in case they have changed
                SubPrice2Stock stock = baseStat.SubP2Stock.SubPrice2Stocks
                    .FirstOrDefault(s => s.TotalSubP2Label == substock.TotalSubP2Label);
                if (stock != null)
                {
                    stock.TotalSubP2Label = substock.TotalSubP2Label;
                    stock.TotalSubP2Name = substock.TotalSubP2Name;
                    stock.TotalSubP2Unit = substock.TotalSubP2Unit;
                    stock.TotalSubP2Description = substock.TotalSubP2Description;
                }
            }
        }

    }
}