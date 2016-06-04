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
    ///             Costs: LCA1Stock.Stat1.SubP1Stock.SubStock1s.SubPrice1s
    ///             Benefits: LCA1Stock.Stat1.SubP2Stock.SubStock2s.SubPrice1s
    ///             The class statistically analyzes lcas.
    ///Author:		www.devtreks.org
    ///Date:		2013, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class LCA1Stat1 : LCA1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public LCA1Stat1()
            : base()
        {
            //subprice object
            InitTotalLCA1Stat1Properties(this);
        }
        //copy constructor
        public LCA1Stat1(LCA1Stat1 calculator)
            : base(calculator)
        {
            CopyTotalLCA1Stat1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent SubPrice1Stock
        //calculator properties
        //number of cost observations
        public double TotalCostN { get; set; }
        //totals names must be consistent with Total1
        public double TotalOCCost { get; set; }
        public double TotalOCMean { get; set; }
        public double TotalOCMedian { get; set; }
        public double TotalOCVariance { get; set; }
        public double TotalOCStandDev { get; set; }

        public double TotalAOHCost { get; set; }
        public double TotalAOHMean { get; set; }
        public double TotalAOHMedian { get; set; }
        public double TotalAOHVariance { get; set; }
        public double TotalAOHStandDev { get; set; }

        public double TotalCAPCost { get; set; }
        public double TotalCAPMean { get; set; }
        public double TotalCAPMedian { get; set; }
        public double TotalCAPVariance { get; set; }
        public double TotalCAPStandDev { get; set; }

        //total lcc cost
        public double TotalLCCCost { get; set; }
        public double TotalLCCMean { get; set; }
        public double TotalLCCMedian { get; set; }
        public double TotalLCCVariance { get; set; }
        public double TotalLCCStandDev { get; set; }

        //total eaa cost (equiv ann annuity)
        public double TotalEAACost { get; set; }
        public double TotalEAAMean { get; set; }
        public double TotalEAAMedian { get; set; }
        public double TotalEAAVariance { get; set; }
        public double TotalEAAStandDev { get; set; }

        //total per unit costs
        public double TotalUnitCost { get; set; }
        public double TotalUnitMean { get; set; }
        public double TotalUnitMedian { get; set; }
        public double TotalUnitVariance { get; set; }
        public double TotalUnitStandDev { get; set; }

        private const string cTotalCostN = "TCostN";

        private const string cTotalOCCost = "TOCCost";
        private const string cTotalOCMean = "TOCMean";
        private const string cTotalOCMedian = "TOCMedian";
        private const string cTotalOCVariance = "TOCVariance";
        private const string cTotalOCStandDev = "TOCStandDev";

        private const string cTotalAOHCost = "TAOHCost";
        private const string cTotalAOHMean = "TAOHMean";
        private const string cTotalAOHMedian = "TAOHMedian";
        private const string cTotalAOHVariance = "TAOHVariance";
        private const string cTotalAOHStandDev = "TAOHStandDev";

         private const string cTotalCAPCost = "TCAPCost";
        private const string cTotalCAPMean = "TCAPMean";
        private const string cTotalCAPMedian = "TCAPMedian";
        private const string cTotalCAPVariance = "TCAPVariance";
        private const string cTotalCAPStandDev = "TCAPStandDev";

        private const string cTotalLCCCost = "TLCCCost";
        private const string cTotalLCCMean = "TLCCMean";
        private const string cTotalLCCMedian = "TLCCMedian";
        private const string cTotalLCCVariance = "TLCCVariance";
        private const string cTotalLCCStandDev = "TLCCStandDev";

        private const string cTotalEAACost = "TEAACost";
        private const string cTotalEAAMean = "TEAAMean";
        private const string cTotalEAAMedian = "TEAAMedian";
        private const string cTotalEAAVariance = "TEAAVariance";
        private const string cTotalEAAStandDev = "TEAAStandDev";

        private const string cTotalUnitCost = "TUnitCost";
        private const string cTotalUnitMean = "TUnitMean";
        private const string cTotalUnitMedian = "TUnitMedian";
        private const string cTotalUnitVariance = "TUnitVariance";
        private const string cTotalUnitStandDev = "TUnitStandDev";

        //benefits
        //number of benefit observations
        public double TotalBenefitN { get; set; }
        //totals, including initbens, salvageval, replacement, and subcosts
        public double TotalRBenefit { get; set; }
        public double TotalRMean { get; set; }
        public double TotalRMedian { get; set; }
        public double TotalRVariance { get; set; }
        public double TotalRStandDev { get; set; }
        //total lcb benefit
        public double TotalLCBBenefit { get; set; }
        public double TotalLCBMean { get; set; }
        public double TotalLCBMedian { get; set; }
        public double TotalLCBVariance { get; set; }
        public double TotalLCBStandDev { get; set; }
        //total eaa benefit (equiv ann annuity)
        public double TotalREAABenefit { get; set; }
        public double TotalREAAMean { get; set; }
        public double TotalREAAMedian { get; set; }
        public double TotalREAAVariance { get; set; }
        public double TotalREAAStandDev { get; set; }
        //total per unit benefits
        public double TotalRUnitBenefit { get; set; }
        public double TotalRUnitMean { get; set; }
        public double TotalRUnitMedian { get; set; }
        public double TotalRUnitVariance { get; set; }
        public double TotalRUnitStandDev { get; set; }

        //options and salvage value taken from other capital inputs
        private const string cTotalBenefitN = "TBenefitN";
        private const string cTotalRBenefit = "TRBenefit";
        private const string cTotalRMean = "TRMean";
        private const string cTotalRMedian = "TRMedian";
        private const string cTotalRVariance = "TRVariance";
        private const string cTotalRStandDev = "TRStandDev";

        private const string cTotalLCBBenefit = "TLCBBenefit";
        private const string cTotalLCBMean = "TLCBMean";
        private const string cTotalLCBMedian = "TLCBMedian";
        private const string cTotalLCBVariance = "TLCBVariance";
        private const string cTotalLCBStandDev = "TLCBStandDev";

        private const string cTotalREAABenefit = "TREAABenefit";
        private const string cTotalREAAMean = "TREAAMean";
        private const string cTotalREAAMedian = "TREAAMedian";
        private const string cTotalREAAVariance = "TREAAVariance";
        private const string cTotalREAAStandDev = "TREAAStandDev";

        private const string cTotalRUnitBenefit = "TRUnitBenefit";
        private const string cTotalRUnitMean = "TRUnitMean";
        private const string cTotalRUnitMedian = "TRUnitMedian";
        private const string cTotalRUnitVariance = "TRUnitVariance";
        private const string cTotalRUnitStandDev = "TRUnitStandDev";

        public void InitTotalLCA1Stat1Properties(LCA1Stat1 ind)
        {
            ind.ErrorMessage = string.Empty;
        
            ind.TotalCostN = 0;
            ind.TotalOCCost = 0;
            ind.TotalOCMean = 0;
            ind.TotalOCMedian = 0;
            ind.TotalOCVariance = 0;
            ind.TotalOCStandDev = 0;

            ind.TotalAOHCost = 0;
            ind.TotalAOHMean = 0;
            ind.TotalAOHMedian = 0;
            ind.TotalAOHVariance = 0;
            ind.TotalAOHStandDev = 0;

            ind.TotalCAPCost = 0;
            ind.TotalCAPMean = 0;
            ind.TotalCAPMedian = 0;
            ind.TotalCAPVariance = 0;
            ind.TotalCAPStandDev = 0;

            ind.TotalLCCCost = 0;
            ind.TotalLCCMean = 0;
            ind.TotalLCCMedian = 0;
            ind.TotalLCCVariance = 0;
            ind.TotalLCCStandDev = 0;

            ind.TotalEAACost = 0;
            ind.TotalEAAMean = 0;
            ind.TotalEAAMedian = 0;
            ind.TotalEAAVariance = 0;
            ind.TotalEAAStandDev = 0;

            ind.TotalUnitCost = 0;
            ind.TotalUnitMean = 0;
            ind.TotalUnitMedian = 0;
            ind.TotalUnitVariance = 0;
            ind.TotalUnitStandDev = 0;

            ind.TotalBenefitN = 0;
            ind.TotalRBenefit = 0;
            ind.TotalRMean = 0;
            ind.TotalRMedian = 0;
            ind.TotalRVariance = 0;
            ind.TotalRStandDev = 0;

            ind.TotalLCBBenefit = 0;
            ind.TotalLCBMean = 0;
            ind.TotalLCBMedian = 0;
            ind.TotalLCBVariance = 0;
            ind.TotalLCBStandDev = 0;

            ind.TotalREAABenefit = 0;
            ind.TotalREAAMean = 0;
            ind.TotalREAAMedian = 0;
            ind.TotalREAAVariance = 0;
            ind.TotalREAAStandDev = 0;

            ind.TotalRUnitBenefit = 0;
            ind.TotalRUnitMean = 0;
            ind.TotalRUnitMedian = 0;
            ind.TotalRUnitVariance = 0;
            ind.TotalRUnitStandDev = 0;
            ind.CalcParameters = new CalculatorParameters();
            ind.SubP1Stock = new SubPrice1Stock();
            ind.SubP2Stock = new SubPrice2Stock();
        }

        public void CopyTotalLCA1Stat1Properties(LCA1Stat1 ind,
            LCA1Stat1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalCostN = calculator.TotalCostN;
            ind.TotalOCCost = calculator.TotalOCCost;
            ind.TotalOCMean = calculator.TotalOCMean;
            ind.TotalOCMedian = calculator.TotalOCMedian;
            ind.TotalOCVariance = calculator.TotalOCVariance;
            ind.TotalOCStandDev = calculator.TotalOCStandDev;

            ind.TotalAOHCost = calculator.TotalAOHCost;
            ind.TotalAOHMean = calculator.TotalAOHMean;
            ind.TotalAOHMedian = calculator.TotalAOHMedian;
            ind.TotalAOHVariance = calculator.TotalAOHVariance;
            ind.TotalAOHStandDev = calculator.TotalAOHStandDev;

            ind.TotalCAPCost = calculator.TotalCAPCost;
            ind.TotalCAPMean = calculator.TotalCAPMean;
            ind.TotalCAPMedian = calculator.TotalCAPMedian;
            ind.TotalCAPVariance = calculator.TotalCAPVariance;
            ind.TotalCAPStandDev = calculator.TotalCAPStandDev;

            ind.TotalLCCCost = calculator.TotalLCCCost;
            ind.TotalLCCMean = calculator.TotalLCCMean;
            ind.TotalLCCMedian = calculator.TotalLCCMedian;
            ind.TotalLCCVariance = calculator.TotalLCCVariance;
            ind.TotalLCCStandDev = calculator.TotalLCCStandDev;

            ind.TotalEAACost = calculator.TotalEAACost;
            ind.TotalEAAMean = calculator.TotalEAAMean;
            ind.TotalEAAMedian = calculator.TotalEAAMedian;
            ind.TotalEAAVariance = calculator.TotalEAAVariance;
            ind.TotalEAAStandDev = calculator.TotalEAAStandDev;

            ind.TotalUnitCost = calculator.TotalUnitCost;
            ind.TotalUnitMean = calculator.TotalUnitMean;
            ind.TotalUnitMedian = calculator.TotalUnitMedian;
            ind.TotalUnitVariance = calculator.TotalUnitVariance;
            ind.TotalUnitStandDev = calculator.TotalUnitStandDev;

            ind.TotalBenefitN = calculator.TotalBenefitN;
            ind.TotalRBenefit = calculator.TotalRBenefit;
            ind.TotalRMean = calculator.TotalRMean;
            ind.TotalRMedian = calculator.TotalRMedian;
            ind.TotalRVariance = calculator.TotalRVariance;
            ind.TotalRStandDev = calculator.TotalRStandDev;

            ind.TotalLCBBenefit = calculator.TotalLCBBenefit;
            ind.TotalLCBMean = calculator.TotalLCBMean;
            ind.TotalLCBMedian = calculator.TotalLCBMedian;
            ind.TotalLCBVariance = calculator.TotalLCBVariance;
            ind.TotalLCBStandDev = calculator.TotalLCBStandDev;

            ind.TotalREAABenefit = calculator.TotalREAABenefit;
            ind.TotalREAAMean = calculator.TotalREAAMean;
            ind.TotalREAAMedian = calculator.TotalREAAMedian;
            ind.TotalREAAVariance = calculator.TotalREAAVariance;
            ind.TotalREAAStandDev = calculator.TotalREAAStandDev;

            ind.TotalRUnitBenefit = calculator.TotalRUnitBenefit;
            ind.TotalRUnitMean = calculator.TotalRUnitMean;
            ind.TotalRUnitMedian = calculator.TotalRUnitMedian;
            ind.TotalRUnitVariance = calculator.TotalRUnitVariance;
            ind.TotalRUnitStandDev = calculator.TotalRUnitStandDev;
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

        public void SetTotalLCA1Stat1Properties(LCA1Stat1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalCostN = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCostN, attNameExtension));
            ind.TotalOCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCCost, attNameExtension));
            ind.TotalOCMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCMean, attNameExtension));
            ind.TotalOCMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCMedian, attNameExtension));
            ind.TotalOCVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCVariance, attNameExtension));
            ind.TotalOCStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCStandDev, attNameExtension));

            ind.TotalAOHCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHCost, attNameExtension));
            ind.TotalAOHMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHMean, attNameExtension));
            ind.TotalAOHMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHMedian, attNameExtension));
            ind.TotalAOHVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHVariance, attNameExtension));
            ind.TotalAOHStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHStandDev, attNameExtension));

            ind.TotalCAPCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPCost, attNameExtension));
            ind.TotalCAPMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPMean, attNameExtension));
            ind.TotalCAPMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPMedian, attNameExtension));
            ind.TotalCAPVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPVariance, attNameExtension));
            ind.TotalCAPStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPStandDev, attNameExtension));

            ind.TotalLCCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCCost, attNameExtension));
            ind.TotalLCCMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCMean, attNameExtension));
            ind.TotalLCCMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCMedian, attNameExtension));
            ind.TotalLCCVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCVariance, attNameExtension));
            ind.TotalLCCStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCStandDev, attNameExtension));

            ind.TotalEAACost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAACost, attNameExtension));
            ind.TotalEAAMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAMean, attNameExtension));
            ind.TotalEAAMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAMedian, attNameExtension));
            ind.TotalEAAVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAVariance, attNameExtension));
            ind.TotalEAAStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAStandDev, attNameExtension));

            ind.TotalUnitCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitCost, attNameExtension));
            ind.TotalUnitMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitMean, attNameExtension));
            ind.TotalUnitMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitMedian, attNameExtension));
            ind.TotalUnitVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitVariance, attNameExtension));
            ind.TotalUnitStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitStandDev, attNameExtension));

            ind.TotalBenefitN = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalBenefitN, attNameExtension));
            ind.TotalRBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRBenefit, attNameExtension));
            ind.TotalRMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRMean, attNameExtension));
            ind.TotalRMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRMedian, attNameExtension));
            ind.TotalRVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRVariance, attNameExtension));
            ind.TotalRStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRStandDev, attNameExtension));

            ind.TotalLCBBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBBenefit, attNameExtension));
            ind.TotalLCBMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBMean, attNameExtension));
            ind.TotalLCBMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBMedian, attNameExtension));
            ind.TotalLCBVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBVariance, attNameExtension));
            ind.TotalLCBStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBStandDev, attNameExtension));

            ind.TotalREAABenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAABenefit, attNameExtension));
            ind.TotalREAAMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAMean, attNameExtension));
            ind.TotalREAAMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAMedian, attNameExtension));
            ind.TotalREAAVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAVariance, attNameExtension));
            ind.TotalREAAStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAStandDev, attNameExtension));

            ind.TotalRUnitBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitBenefit, attNameExtension));
            ind.TotalRUnitMean = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitMean, attNameExtension));
            ind.TotalRUnitMedian = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitMedian, attNameExtension));
            ind.TotalRUnitVariance = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitVariance, attNameExtension));
            ind.TotalRUnitStandDev = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitStandDev, attNameExtension));
        }

        public void SetTotalLCA1Stat1Property(LCA1Stat1 ind,
            string attName, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalCostN:
                    ind.TotalCostN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCCost:
                    ind.TotalOCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCMean:
                    ind.TotalOCMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCMedian:
                    ind.TotalOCMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCVariance:
                    ind.TotalOCVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCStandDev:
                    ind.TotalOCStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHCost:
                    ind.TotalAOHCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHMean:
                    ind.TotalAOHMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHMedian:
                    ind.TotalAOHMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHVariance:
                    ind.TotalAOHVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHStandDev:
                    ind.TotalAOHStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPCost:
                    ind.TotalCAPCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPMean:
                    ind.TotalCAPMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPMedian:
                    ind.TotalCAPMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPVariance:
                    ind.TotalCAPVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPStandDev:
                    ind.TotalCAPStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCCost:
                    ind.TotalLCCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCMean:
                    ind.TotalLCCMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCMedian:
                    ind.TotalLCCMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCVariance:
                    ind.TotalLCCVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCStandDev:
                    ind.TotalLCCStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAACost:
                    ind.TotalEAACost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAMean:
                    ind.TotalEAAMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAMedian:
                    ind.TotalEAAMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAVariance:
                    ind.TotalEAAVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAStandDev:
                    ind.TotalEAAStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitCost:
                    ind.TotalUnitCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitMean:
                    ind.TotalUnitMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitMedian:
                    ind.TotalUnitMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitVariance:
                    ind.TotalUnitVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitStandDev:
                    ind.TotalUnitStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalBenefitN:
                    ind.TotalBenefitN = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRBenefit:
                    ind.TotalRBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRMean:
                    ind.TotalRMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRMedian:
                    ind.TotalRMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRVariance:
                    ind.TotalRVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRStandDev:
                    ind.TotalRStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBBenefit:
                    ind.TotalLCBBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBMean:
                    ind.TotalLCBMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBMedian:
                    ind.TotalLCBMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBVariance:
                    ind.TotalLCBVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBStandDev:
                    ind.TotalLCBStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAABenefit:
                    ind.TotalREAABenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAMean:
                    ind.TotalREAAMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAMedian:
                    ind.TotalREAAMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAVariance:
                    ind.TotalREAAVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAStandDev:
                    ind.TotalREAAStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitBenefit:
                    ind.TotalRUnitBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitMean:
                    ind.TotalRUnitMean = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitMedian:
                    ind.TotalRUnitMedian = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitVariance:
                    ind.TotalRUnitVariance = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitStandDev:
                    ind.TotalRUnitStandDev = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalLCA1Stat1Property(LCA1Stat1 ind, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cTotalCostN:
                    sPropertyValue = ind.TotalCostN.ToString();
                    break;
                case cTotalOCCost:
                    sPropertyValue = ind.TotalOCCost.ToString();
                    break;
                case cTotalOCMean:
                    sPropertyValue = ind.TotalOCMean.ToString();
                    break;
                case cTotalOCMedian:
                    sPropertyValue = ind.TotalOCMedian.ToString();
                    break;
                case cTotalOCVariance:
                    sPropertyValue = ind.TotalOCVariance.ToString();
                    break;
                case cTotalOCStandDev:
                    sPropertyValue = ind.TotalOCStandDev.ToString();
                    break;
                case cTotalAOHCost:
                    sPropertyValue = ind.TotalAOHCost.ToString();
                    break;
                case cTotalAOHMean:
                    sPropertyValue = ind.TotalAOHMean.ToString();
                    break;
                case cTotalAOHMedian:
                    sPropertyValue = ind.TotalAOHMedian.ToString();
                    break;
                case cTotalAOHVariance:
                    sPropertyValue = ind.TotalAOHVariance.ToString();
                    break;
                case cTotalAOHStandDev:
                    sPropertyValue = ind.TotalAOHStandDev.ToString();
                    break;
                case cTotalCAPCost:
                    sPropertyValue = ind.TotalCAPCost.ToString();
                    break;
                case cTotalCAPMean:
                    sPropertyValue = ind.TotalCAPMean.ToString();
                    break;
                case cTotalCAPMedian:
                    sPropertyValue = ind.TotalCAPMedian.ToString();
                    break;
                case cTotalCAPVariance:
                    sPropertyValue = ind.TotalCAPVariance.ToString();
                    break;
                case cTotalCAPStandDev:
                    sPropertyValue = ind.TotalCAPStandDev.ToString();
                    break;
                case cTotalLCCCost:
                    sPropertyValue = ind.TotalLCCCost.ToString();
                    break;
                case cTotalLCCMean:
                    sPropertyValue = ind.TotalLCCMean.ToString();
                    break;
                case cTotalLCCMedian:
                    sPropertyValue = ind.TotalLCCMedian.ToString();
                    break;
                case cTotalLCCVariance:
                    sPropertyValue = ind.TotalLCCVariance.ToString();
                    break;
                case cTotalLCCStandDev:
                    sPropertyValue = ind.TotalLCCStandDev.ToString();
                    break;
                case cTotalEAACost:
                    sPropertyValue = ind.TotalEAACost.ToString();
                    break;
                case cTotalEAAMean:
                    sPropertyValue = ind.TotalEAAMean.ToString();
                    break;
                case cTotalEAAMedian:
                    sPropertyValue = ind.TotalEAAMedian.ToString();
                    break;
                case cTotalEAAVariance:
                    sPropertyValue = ind.TotalEAAVariance.ToString();
                    break;
                case cTotalEAAStandDev:
                    sPropertyValue = ind.TotalEAAStandDev.ToString();
                    break;
                case cTotalUnitCost:
                    sPropertyValue = ind.TotalUnitCost.ToString();
                    break;
                case cTotalUnitMean:
                    sPropertyValue = ind.TotalUnitMean.ToString();
                    break;
                case cTotalUnitMedian:
                    sPropertyValue = ind.TotalUnitMedian.ToString();
                    break;
                case cTotalUnitVariance:
                    sPropertyValue = ind.TotalUnitVariance.ToString();
                    break;
                case cTotalUnitStandDev:
                    sPropertyValue = ind.TotalUnitStandDev.ToString();
                    break;
                case cTotalBenefitN:
                    sPropertyValue = ind.TotalBenefitN.ToString();
                    break;
                case cTotalRBenefit:
                    sPropertyValue = ind.TotalRBenefit.ToString();
                    break;
                case cTotalRMean:
                    sPropertyValue = ind.TotalRMean.ToString();
                    break;
                case cTotalRMedian:
                    sPropertyValue = ind.TotalRMedian.ToString();
                    break;
                case cTotalRVariance:
                    sPropertyValue = ind.TotalRVariance.ToString();
                    break;
                case cTotalRStandDev:
                    sPropertyValue = ind.TotalRStandDev.ToString();
                    break;
                case cTotalLCBBenefit:
                    sPropertyValue = ind.TotalLCBBenefit.ToString();
                    break;
                case cTotalLCBMean:
                    sPropertyValue = ind.TotalLCBMean.ToString();
                    break;
                case cTotalLCBMedian:
                    sPropertyValue = ind.TotalLCBMedian.ToString();
                    break;
                case cTotalLCBVariance:
                    sPropertyValue = ind.TotalLCBVariance.ToString();
                    break;
                case cTotalLCBStandDev:
                    sPropertyValue = ind.TotalLCBStandDev.ToString();
                    break;
                case cTotalREAABenefit:
                    sPropertyValue = ind.TotalREAABenefit.ToString();
                    break;
                case cTotalREAAMean:
                    sPropertyValue = ind.TotalREAAMean.ToString();
                    break;
                case cTotalREAAMedian:
                    sPropertyValue = ind.TotalREAAMedian.ToString();
                    break;
                case cTotalREAAVariance:
                    sPropertyValue = ind.TotalREAAVariance.ToString();
                    break;
                case cTotalREAAStandDev:
                    sPropertyValue = ind.TotalREAAStandDev.ToString();
                    break;
                case cTotalRUnitBenefit:
                    sPropertyValue = ind.TotalRUnitBenefit.ToString();
                    break;
                case cTotalRUnitMean:
                    sPropertyValue = ind.TotalRUnitMean.ToString();
                    break;
                case cTotalRUnitMedian:
                    sPropertyValue = ind.TotalRUnitMedian.ToString();
                    break;
                case cTotalRUnitVariance:
                    sPropertyValue = ind.TotalRUnitVariance.ToString();
                    break;
                case cTotalRUnitStandDev:
                    sPropertyValue = ind.TotalRUnitStandDev.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalLCA1Stat1Attributes(LCA1Stat1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCostN, attNameExtension), ind.TotalCostN);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCMean, attNameExtension), ind.TotalOCMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCMedian, attNameExtension), ind.TotalOCMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCVariance, attNameExtension), ind.TotalOCVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCStandDev, attNameExtension), ind.TotalOCStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHMean, attNameExtension), ind.TotalAOHMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHMedian, attNameExtension), ind.TotalAOHMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHVariance, attNameExtension), ind.TotalAOHVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHStandDev, attNameExtension), ind.TotalAOHStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPMean, attNameExtension), ind.TotalCAPMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPMedian, attNameExtension), ind.TotalCAPMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPVariance, attNameExtension), ind.TotalCAPVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPStandDev, attNameExtension), ind.TotalCAPStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCMean, attNameExtension), ind.TotalLCCMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCMedian, attNameExtension), ind.TotalLCCMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCVariance, attNameExtension), ind.TotalLCCVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCStandDev, attNameExtension), ind.TotalLCCStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAMean, attNameExtension), ind.TotalEAAMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAMedian, attNameExtension), ind.TotalEAAMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAVariance, attNameExtension), ind.TotalEAAVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAStandDev, attNameExtension), ind.TotalEAAStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitMean, attNameExtension), ind.TotalUnitMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitMedian, attNameExtension), ind.TotalUnitMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitVariance, attNameExtension), ind.TotalUnitVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitStandDev, attNameExtension), ind.TotalUnitStandDev);
            }
            if (bIsBenefitNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalBenefitN, attNameExtension), ind.TotalBenefitN);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRMean, attNameExtension), ind.TotalRMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRMedian, attNameExtension), ind.TotalRMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRVariance, attNameExtension), ind.TotalRVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRStandDev, attNameExtension), ind.TotalRStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBMean, attNameExtension), ind.TotalLCBMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBMedian, attNameExtension), ind.TotalLCBMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBVariance, attNameExtension), ind.TotalLCBVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBStandDev, attNameExtension), ind.TotalLCBStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAMean, attNameExtension), ind.TotalREAAMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAMedian, attNameExtension), ind.TotalREAAMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAVariance, attNameExtension), ind.TotalREAAVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAStandDev, attNameExtension), ind.TotalREAAStandDev);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitMean, attNameExtension), ind.TotalRUnitMean);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitMedian, attNameExtension), ind.TotalRUnitMedian);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitVariance, attNameExtension), ind.TotalRUnitVariance);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitStandDev, attNameExtension), ind.TotalRUnitStandDev);
            }
        }

        public void SetTotalLCA1Stat1Attributes(LCA1Stat1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                   string.Concat(cTotalCostN, attNameExtension), ind.TotalCostN.ToString());
                writer.WriteAttributeString(
                    string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCMean, attNameExtension), ind.TotalOCMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCMedian, attNameExtension), ind.TotalOCMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCVariance, attNameExtension), ind.TotalOCVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCStandDev, attNameExtension), ind.TotalOCStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHMean, attNameExtension), ind.TotalAOHMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHMedian, attNameExtension), ind.TotalAOHMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHVariance, attNameExtension), ind.TotalAOHVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHStandDev, attNameExtension), ind.TotalAOHStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPMean, attNameExtension), ind.TotalCAPMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPMedian, attNameExtension), ind.TotalCAPMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPVariance, attNameExtension), ind.TotalCAPVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPStandDev, attNameExtension), ind.TotalCAPStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                   string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCMean, attNameExtension), ind.TotalLCCMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCMedian, attNameExtension), ind.TotalLCCMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCVariance, attNameExtension), ind.TotalLCCVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCStandDev, attNameExtension), ind.TotalLCCStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAMean, attNameExtension), ind.TotalEAAMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAMedian, attNameExtension), ind.TotalEAAMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAVariance, attNameExtension), ind.TotalEAAVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAStandDev, attNameExtension), ind.TotalEAAStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitMean, attNameExtension), ind.TotalUnitMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitMedian, attNameExtension), ind.TotalUnitMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitVariance, attNameExtension), ind.TotalUnitVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitStandDev, attNameExtension), ind.TotalUnitStandDev.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsBenefitNode || bIsBoth)
            {
                writer.WriteAttributeString(
                   string.Concat(cTotalBenefitN, attNameExtension), ind.TotalBenefitN.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                    string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRMean, attNameExtension), ind.TotalRMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRMedian, attNameExtension), ind.TotalRMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRVariance, attNameExtension), ind.TotalRVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRStandDev, attNameExtension), ind.TotalRStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBMean, attNameExtension), ind.TotalLCBMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBMedian, attNameExtension), ind.TotalLCBMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBVariance, attNameExtension), ind.TotalLCBVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBStandDev, attNameExtension), ind.TotalLCBStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAMean, attNameExtension), ind.TotalREAAMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAMedian, attNameExtension), ind.TotalREAAMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAVariance, attNameExtension), ind.TotalREAAVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAStandDev, attNameExtension), ind.TotalREAAStandDev.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitMean, attNameExtension), ind.TotalRUnitMean.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitMedian, attNameExtension), ind.TotalRUnitMedian.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitVariance, attNameExtension), ind.TotalRUnitVariance.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitStandDev, attNameExtension), ind.TotalRUnitStandDev.ToString("N2", CultureInfo.InvariantCulture));
            }
        }
        //run the analyses for inputs an outputs
        public bool RunAnalyses(LCA1Stock lca1Stock)
        {
            bool bHasAnalyses = false;
            //add totals to lca stocks (use Total1 object to avoid duplication)
            LCA1Total1 total = new LCA1Total1();
            //this adds the totals to lca1stock.total1 (not to total)
            bHasAnalyses = total.RunAnalyses(lca1Stock);
            if (lca1Stock.Total1 != null)
            {
                //copy at least the stock and substock totals from total1 to stat1
                //subprices only if needed in future analyses
                lca1Stock.Stat1 = new LCA1Stat1();
                //need one property set
                lca1Stock.Stat1.SubApplicationType = lca1Stock.CalcParameters.SubApplicationType.ToString();
                CopyTotalToStatStock(lca1Stock.Stat1, lca1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing statistical analysis
        public bool RunAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //add totals to lca1stock.Stat1
            if (lca1Stock.Stat1 == null)
            {
                lca1Stock.Stat1 = new LCA1Stat1();
            }
            //need some properties set
            lca1Stock.Stat1.SubApplicationType = lca1Stock.CalcParameters.SubApplicationType.ToString();
            if (lca1Stock.Stat1.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices.ToString()
                || lca1Stock.Stat1.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices.ToString())
            {
                //inputs and outputs calcs are calculated as separate observations (mean input price for similar inputs is meaningfull)
                bHasAnalyses = SetIOAnalyses(lca1Stock, calcs);
            }
            else
            {
                //inputs and outputs are not calculated as separate observation (mean input price for dissimilar inputs is meaningless)
                bHasAnalyses = SetAnalyses(lca1Stock, calcs);
            }
            return bHasAnalyses;
        }
        private bool SetAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of lca1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Stat1.SubPrice1Stocks for costs and 2s for benefits
            //set N
            int iCostN = 0;
            int iBenefitN = 0;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation; alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            //stats lists holds observation.stat1 collection with totals
            List<LCA1Stat1> stats = new List<LCA1Stat1>();
            foreach (var calcbyalt in calcsByAlt2)
            {
                //set the calc totals in each observation
                LCA1Stock observationStock = new LCA1Stock();
                observationStock.Stat1 = new LCA1Stat1();
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(lca1Stock.GetType()))
                    {
                        LCA1Stock stock = (LCA1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Stat1 != null)
                            {
                                //set each observation's totals
                                bHasTotals = AddSubTotalToTotalStock(observationStock.Stat1, stock.Multiplier,
                                    stock.Stat1);
                            }
                        }
                    }
                }
                //add to the stats collection
                stats.Add(observationStock.Stat1);
                //N is determined from the cost SubP1Stock
                if (observationStock.Stat1.SubP1Stock != null)
                {
                    if (observationStock.Stat1.SubP1Stock.SubPrice1Stocks != null)
                    {
                        if (observationStock.Stat1.SubP1Stock.SubPrice1Stocks.Count > 0)
                        {
                            iCostN++;
                        }
                    }
                }
                //and from the benefit SubP2Stock
                if (observationStock.Stat1.SubP2Stock != null)
                {
                    if (observationStock.Stat1.SubP2Stock.SubPrice2Stocks != null)
                    {
                        if (observationStock.Stat1.SubP2Stock.SubPrice2Stocks.Count > 0)
                        {
                            iBenefitN++;
                        }
                    }
                }
            }
            if (iCostN > 0 || iBenefitN > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats, lca1Stock, iCostN, iBenefitN);
            }
            return bHasAnalysis;
        }
        private bool SetIOAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalysis = false;
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iCostN2 = 0;
            int iBenefitN2 = 0;
            List<LCA1Stat1> stats2 = new List<LCA1Stat1>();
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(lca1Stock.GetType()))
                {
                    LCA1Stock stock = (LCA1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Stat1 != null)
                        {
                            //set the calc totals in each observation
                            LCA1Stock observation2Stock = new LCA1Stock();
                            observation2Stock.Stat1 = new LCA1Stat1();
                            //set each observation's totals
                            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Stat1,
                                lca1Stock.Multiplier, stock.Stat1);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                stats2.Add(observation2Stock.Stat1);
                                //N is determined from the cost SubP1Stock
                                if (observation2Stock.Stat1.SubP1Stock != null)
                                {
                                    if (observation2Stock.Stat1.SubP1Stock.SubPrice1Stocks != null)
                                    {
                                        if (observation2Stock.Stat1.SubP1Stock.SubPrice1Stocks.Count > 0)
                                        {
                                            iCostN2++;
                                        }
                                    }
                                }
                                //and from the benefit SubP2Stock
                                if (observation2Stock.Stat1.SubP2Stock != null)
                                {
                                    if (observation2Stock.Stat1.SubP2Stock.SubPrice2Stocks != null)
                                    {
                                        if (observation2Stock.Stat1.SubP2Stock.SubPrice2Stocks.Count > 0)
                                        {
                                            iBenefitN2++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (iCostN2 > 0 || iBenefitN2 > 0)
            {
                bHasAnalysis = true;
                bHasTotals = SetStatsAnalysis(stats2, lca1Stock, iCostN2, iBenefitN2);
            }
            return bHasAnalysis;
        }
        private bool SetStatsAnalysis(List<LCA1Stat1> stats2, LCA1Stock statStock, 
            int costN, int benN)
        {
            bool bHasTotals = false;
            //set the total observations total
            foreach (var stat in stats2)
            {
                bHasTotals = AddSubTotalToTotalStock(statStock.Stat1, 1, stat);
            }
            if (costN > 0)
            {
                statStock.Stat1.TotalCostN = costN;
                //set the cost means
                statStock.Stat1.TotalOCMean = statStock.Stat1.TotalOCCost / costN;
                statStock.Stat1.TotalAOHMean = statStock.Stat1.TotalAOHCost / costN;
                statStock.Stat1.TotalCAPMean = statStock.Stat1.TotalCAPCost / costN;
                statStock.Stat1.TotalLCCMean = statStock.Stat1.TotalLCCCost / costN;
                statStock.Stat1.TotalEAAMean = statStock.Stat1.TotalEAACost / costN;
                statStock.Stat1.TotalUnitMean = statStock.Stat1.TotalUnitCost / costN;
                //set the median, variance, and standard deviation costs
                SetOCStatistics(statStock, stats2);
                SetAOHStatistics(statStock, stats2);
                SetCAPStatistics(statStock, stats2);
                SetLCCStatistics(statStock, stats2);
                SetEAAStatistics(statStock, stats2);
                SetUnitStatistics(statStock, stats2);
            }
            if (benN > 0)
            {
                statStock.Stat1.TotalBenefitN = benN;
                //set the benefit means
                statStock.Stat1.TotalRMean = statStock.Stat1.TotalRBenefit / benN;
                statStock.Stat1.TotalLCBMean = statStock.Stat1.TotalLCBBenefit / benN;
                statStock.Stat1.TotalREAAMean = statStock.Stat1.TotalREAABenefit / benN;
                statStock.Stat1.TotalRUnitMean = statStock.Stat1.TotalRUnitBenefit / benN;
                //benefits
                SetRStatistics(statStock, stats2);
                SetLCBStatistics(statStock, stats2);
                SetREAAStatistics(statStock, stats2);
                SetRUnitStatistics(statStock, stats2);
            }
            return bHasTotals;
        }

        
        
        public bool CopyTotalToStatStock(LCA1Stat1 totStock, LCA1Total1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier adjusted costs
            totStock.TotalOCCost = subTotal.TotalOCCost;
            totStock.TotalAOHCost = subTotal.TotalAOHCost;
            totStock.TotalCAPCost = subTotal.TotalCAPCost;
            totStock.TotalLCCCost = subTotal.TotalLCCCost;
            totStock.TotalEAACost = subTotal.TotalEAACost;
            totStock.TotalUnitCost = subTotal.TotalUnitCost;
            totStock.TotalRBenefit = subTotal.TotalRBenefit;
            totStock.TotalLCBBenefit = subTotal.TotalLCBBenefit;
            totStock.TotalREAABenefit = subTotal.TotalREAABenefit;
            totStock.TotalRUnitBenefit = subTotal.TotalRUnitBenefit;
            //cost subtotals
            CopySubStock1Totals(totStock, subTotal);
            //benefit subtotals
            CopySubStock2Totals(totStock, subTotal);
            bHasCalculations = true;
            return bHasCalculations;
        }
        private void CopySubStock1Totals(LCA1Stat1 totStock, LCA1Total1 subTotal)
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
        private void CopySubStock2Totals(LCA1Stat1 totStock, LCA1Total1 subTotal)
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
        public bool AddSubTotalToTotalStock(LCA1Stat1 totStock, double multiplier,
            LCA1Stat1 subTotal)
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
        
        private void AddSubStock1Totals(LCA1Stat1 totStock, LCA1Stat1 subTotal)
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
        private void AddSubStock2Totals(LCA1Stat1 totStock, LCA1Stat1 subTotal)
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
        

        public static void ChangeSubTotalByMultipliers(LCA1Stat1 subTotal, double multiplier)
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
        private static void SetOCStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalOCCost);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalOCCost - lca1Stock.Stat1.TotalOCMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalOCMedian = (stat.TotalOCCost + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalOCMedian = stat.TotalOCCost;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalOCCost;
            }

            //don't divide by zero
            if (lca1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalCostN - 1));
                lca1Stock.Stat1.TotalOCVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalOCMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalOCStandDev = Math.Sqrt(lca1Stock.Stat1.TotalOCVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalOCVariance = 0;
                lca1Stock.Stat1.TotalOCStandDev = 0;
            }
        }
        private static void SetAOHStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalAOHCost);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalAOHCost - lca1Stock.Stat1.TotalAOHMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalAOHMedian = (stat.TotalAOHCost + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalAOHMedian = stat.TotalAOHCost;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalAOHCost;
            }

            //don't divide by zero
            if (lca1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalCostN - 1));
                lca1Stock.Stat1.TotalAOHVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalAOHMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalAOHStandDev = Math.Sqrt(lca1Stock.Stat1.TotalAOHVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalAOHVariance = 0;
                lca1Stock.Stat1.TotalAOHStandDev = 0;
            }
        }
        private static void SetCAPStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalCAPCost);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalCAPCost - lca1Stock.Stat1.TotalCAPMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalCAPMedian = (stat.TotalCAPCost + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalCAPMedian = stat.TotalCAPCost;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalCAPCost;
            }

            //don't divide by zero
            if (lca1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalCostN - 1));
                lca1Stock.Stat1.TotalCAPVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalCAPMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalCAPStandDev = Math.Sqrt(lca1Stock.Stat1.TotalCAPVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalCAPVariance = 0;
                lca1Stock.Stat1.TotalCAPStandDev = 0;
            }
        }
        private static void SetLCCStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalLCCCost);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalLCCCost - lca1Stock.Stat1.TotalLCCMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalLCCMedian = (stat.TotalLCCCost + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalLCCMedian = stat.TotalLCCCost;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalLCCCost;
            }

            //don't divide by zero
            if (lca1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalCostN - 1));
                lca1Stock.Stat1.TotalLCCVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalLCCMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalLCCStandDev = Math.Sqrt(lca1Stock.Stat1.TotalLCCVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalLCCVariance = 0;
                lca1Stock.Stat1.TotalLCCStandDev = 0;
            }
        }
        private static void SetEAAStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalEAACost);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalEAACost - lca1Stock.Stat1.TotalEAAMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalEAAMedian = (stat.TotalEAACost + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalEAAMedian = stat.TotalEAACost;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalEAACost;
            }

            //don't divide by zero
            if (lca1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalCostN - 1));
                lca1Stock.Stat1.TotalEAAVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalEAAMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalEAAStandDev = Math.Sqrt(lca1Stock.Stat1.TotalEAAVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalEAAVariance = 0;
                lca1Stock.Stat1.TotalEAAStandDev = 0;
            }
        }
        private static void SetUnitStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalUnitCost);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalUnitCost - lca1Stock.Stat1.TotalUnitMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalUnitMedian = (stat.TotalUnitCost + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalUnitMedian = stat.TotalUnitCost;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalUnitCost;
            }

            //don't divide by zero
            if (lca1Stock.Stat1.TotalCostN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalCostN - 1));
                lca1Stock.Stat1.TotalUnitVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalUnitMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalUnitStandDev = Math.Sqrt(lca1Stock.Stat1.TotalUnitVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalUnitVariance = 0;
                lca1Stock.Stat1.TotalUnitStandDev = 0;
            }
        }
        private static void SetRStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalRBenefit);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalRBenefit - lca1Stock.Stat1.TotalRMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalRMedian = (stat.TotalRBenefit + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalRMedian = stat.TotalRBenefit;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalRBenefit;
            }
            //don't divide by zero
            if (lca1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalBenefitN - 1));
                lca1Stock.Stat1.TotalRVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalRMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalRStandDev = Math.Sqrt(lca1Stock.Stat1.TotalRVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalRVariance = 0;
                lca1Stock.Stat1.TotalRStandDev = 0;
            }
        }
        private static void SetLCBStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalLCBBenefit);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalLCBBenefit - lca1Stock.Stat1.TotalLCBMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalLCBMedian = (stat.TotalLCBBenefit + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalLCBMedian = stat.TotalLCBBenefit;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalLCBBenefit;
            }
            //don't divide by zero
            if (lca1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalBenefitN - 1));
                lca1Stock.Stat1.TotalLCBVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalLCBMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalLCBStandDev = Math.Sqrt(lca1Stock.Stat1.TotalLCBVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalLCBVariance = 0;
                lca1Stock.Stat1.TotalLCBStandDev = 0;
            }
        }
        private static void SetREAAStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalREAABenefit);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalREAABenefit - lca1Stock.Stat1.TotalREAAMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalREAAMedian = (stat.TotalREAABenefit + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalREAAMedian = stat.TotalREAABenefit;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalREAABenefit;
            }
            //don't divide by zero
            if (lca1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalBenefitN - 1));
                lca1Stock.Stat1.TotalREAAVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalREAAMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalREAAStandDev = Math.Sqrt(lca1Stock.Stat1.TotalREAAVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalREAAVariance = 0;
                lca1Stock.Stat1.TotalREAAStandDev = 0;
            }
        }
        private static void SetRUnitStatistics(LCA1Stock lca1Stock, List<LCA1Stat1> stats)
        {
            //reorder for median
            IEnumerable<LCA1Stat1> stat2s = stats.OrderByDescending(s => s.TotalRUnitBenefit);
            double j = 1;
            //variance
            double dbMemberSquaredTotalQ1 = 0;
            double dbMemberSquaredQ1 = 0;
            dbMemberSquaredTotalQ1 = 0;
            double dbMedianQ1 = stat2s.Count() / 2;
            double dbRemainderQ1 = Math.IEEERemainder(stat2s.Count(), 2);
            double dbLastTotalQ1 = 0;
            foreach (LCA1Stat1 stat in stat2s)
            {
                dbMemberSquaredQ1 = Math.Pow((stat.TotalRUnitBenefit - lca1Stock.Stat1.TotalRUnitMean), 2);
                dbMemberSquaredTotalQ1 += dbMemberSquaredQ1;
                if (j > dbMedianQ1 && j != 0)
                {
                    if (dbRemainderQ1 == 0)
                    {
                        //divide the middle two numbers
                        lca1Stock.Stat1.TotalRUnitMedian = (stat.TotalRUnitBenefit + dbLastTotalQ1) / 2;
                    }
                    else
                    {
                        //use the middle number
                        lca1Stock.Stat1.TotalRUnitMedian = stat.TotalRUnitBenefit;
                    }
                    j = 0;
                }
                if (j != 0)
                {
                    j = j + 1;
                }
                dbLastTotalQ1 = stat.TotalRUnitBenefit;
            }
            //don't divide by zero
            if (lca1Stock.Stat1.TotalBenefitN > 1)
            {
                //sample variance
                double dbCount = (1 / (lca1Stock.Stat1.TotalBenefitN - 1));
                lca1Stock.Stat1.TotalRUnitVariance = dbMemberSquaredTotalQ1 * dbCount;
                if (lca1Stock.Stat1.TotalRUnitMean != 0)
                {
                    //sample standard deviation
                    lca1Stock.Stat1.TotalRUnitStandDev = Math.Sqrt(lca1Stock.Stat1.TotalRUnitVariance);
                }
            }
            else
            {
                lca1Stock.Stat1.TotalRUnitVariance = 0;
                lca1Stock.Stat1.TotalRUnitStandDev = 0;
            }
        }
    }
    public static class LCA1Stat1Extensions
    {
        public static void AddSubStock1ToTotalStocks(this LCA1Stat1 baseStat, SubPrice1Stock substock)
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
        public static void AddSubStock2ToTotalStocks(this LCA1Stat1 baseStat, SubPrice2Stock substock)
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
