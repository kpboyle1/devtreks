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
    ///             The class tracks annual changes in totals.
    ///Author:		www.devtreks.org
    ///Date:		2013, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class LCA1Change1 : LCA1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public LCA1Change1()
            : base()
        {
            //subprice object
            InitTotalLCA1Change1Properties(this);
        }
        //copy constructor
        public LCA1Change1(LCA1Change1 calculator)
            : base(calculator)
        {
            CopyTotalLCA1Change1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent LCA1Stock
        //calculator properties
        
        //totals names must be consistent with Total1
        //time period total
        public double TotalOCCost { get; set; }
        //total change from last time period
        public double TotalOCAmountChange { get; set; }
        //percent change from last time period
        public double TotalOCPercentChange { get; set; }
        //total change from base lcc or lcb calculator
        public double TotalOCBaseChange { get; set; }
        //percent change from base lcc or lcb calculator
        public double TotalOCBasePercentChange { get; set; }

        public double TotalAOHCost { get; set; }
        public double TotalAOHAmountChange { get; set; }
        public double TotalAOHPercentChange { get; set; }
        public double TotalAOHBaseChange { get; set; }
        public double TotalAOHBasePercentChange { get; set; }

        public double TotalCAPCost { get; set; }
        public double TotalCAPAmountChange { get; set; }
        public double TotalCAPPercentChange { get; set; }
        public double TotalCAPBaseChange { get; set; }
        public double TotalCAPBasePercentChange { get; set; }

        //total lcc cost
        public double TotalLCCCost { get; set; }
        public double TotalLCCAmountChange { get; set; }
        public double TotalLCCPercentChange { get; set; }
        public double TotalLCCBaseChange { get; set; }
        public double TotalLCCBasePercentChange { get; set; }

        //total eaa cost (equiv ann annuity)
        public double TotalEAACost { get; set; }
        public double TotalEAAAmountChange { get; set; }
        public double TotalEAAPercentChange { get; set; }
        public double TotalEAABaseChange { get; set; }
        public double TotalEAABasePercentChange { get; set; }

        //total per unit costs
        public double TotalUnitCost { get; set; }
        public double TotalUnitAmountChange { get; set; }
        public double TotalUnitPercentChange { get; set; }
        public double TotalUnitBaseChange { get; set; }
        public double TotalUnitBasePercentChange { get; set; }

        private const string cTotalOCCost = "TOCCost";
        private const string cTotalOCAmountChange = "TOCAmountChange";
        private const string cTotalOCPercentChange = "TOCPercentChange";
        private const string cTotalOCBaseChange = "TOCBaseChange";
        private const string cTotalOCBasePercentChange = "TOCBasePercentChange";

        private const string cTotalAOHCost = "TAOHCost";
        private const string cTotalAOHAmountChange = "TAOHAmountChange";
        private const string cTotalAOHPercentChange = "TAOHPercentChange";
        private const string cTotalAOHBaseChange = "TAOHBaseChange";
        private const string cTotalAOHBasePercentChange = "TAOHBasePercentChange";

        private const string cTotalCAPCost = "TCAPCost";
        private const string cTotalCAPAmountChange = "TCAPAmountChange";
        private const string cTotalCAPPercentChange = "TCAPPercentChange";
        private const string cTotalCAPBaseChange = "TCAPBaseChange";
        private const string cTotalCAPBasePercentChange = "TCAPBasePercentChange";

        private const string cTotalLCCCost = "TLCCCost";
        private const string cTotalLCCAmountChange = "TLCCAmountChange";
        private const string cTotalLCCPercentChange = "TLCCPercentChange";
        private const string cTotalLCCBaseChange = "TLCCBaseChange";
        private const string cTotalLCCBasePercentChange = "TLCCBasePercentChange";

        private const string cTotalEAACost = "TEAACost";
        private const string cTotalEAAAmountChange = "TEAAAmountChange";
        private const string cTotalEAAPercentChange = "TEAAPercentChange";
        private const string cTotalEAABaseChange = "TEAABaseChange";
        private const string cTotalEAABasePercentChange = "TEAABasePercentChange";

        private const string cTotalUnitCost = "TUnitCost";
        private const string cTotalUnitAmountChange = "TUnitAmountChange";
        private const string cTotalUnitPercentChange = "TUnitPercentChange";
        private const string cTotalUnitBaseChange = "TUnitBaseChange";
        private const string cTotalUnitBasePercentChange = "TUnitBasePercentChange";

        //benefits
        //totals, including initbens, salvageval, replacement, and subcosts
        public double TotalRBenefit { get; set; }
        public double TotalRAmountChange { get; set; }
        public double TotalRPercentChange { get; set; }
        public double TotalRBaseChange { get; set; }
        public double TotalRBasePercentChange { get; set; }
        //total lcb benefit
        public double TotalLCBBenefit { get; set; }
        public double TotalLCBAmountChange { get; set; }
        public double TotalLCBPercentChange { get; set; }
        public double TotalLCBBaseChange { get; set; }
        public double TotalLCBBasePercentChange { get; set; }
        //total eaa benefit (equiv ann annuity)
        public double TotalREAABenefit { get; set; }
        public double TotalREAAAmountChange { get; set; }
        public double TotalREAAPercentChange { get; set; }
        public double TotalREAABaseChange { get; set; }
        public double TotalREAABasePercentChange { get; set; }
        //total per unit benefits
        public double TotalRUnitBenefit { get; set; }
        public double TotalRUnitAmountChange { get; set; }
        public double TotalRUnitPercentChange { get; set; }
        public double TotalRUnitBaseChange { get; set; }
        public double TotalRUnitBasePercentChange { get; set; }

        //options and salvage value taken from other capital inputs
        private const string cTotalRBenefit = "TRBenefit";
        private const string cTotalRAmountChange = "TRAmountChange";
        private const string cTotalRPercentChange = "TRPercentChange";
        private const string cTotalRBaseChange = "TRBaseChange";
        private const string cTotalRBasePercentChange = "TRBasePercentChange";

        private const string cTotalLCBBenefit = "TLCBBenefit";
        private const string cTotalLCBAmountChange = "TLCBAmountChange";
        private const string cTotalLCBPercentChange = "TLCBPercentChange";
        private const string cTotalLCBBaseChange = "TLCBBaseChange";
        private const string cTotalLCBBasePercentChange = "TLCBBasePercentChange";

        private const string cTotalREAABenefit = "TREAABenefit";
        private const string cTotalREAAAmountChange = "TREAAAmountChange";
        private const string cTotalREAAPercentChange = "TREAAPercentChange";
        private const string cTotalREAABaseChange = "TREAABaseChange";
        private const string cTotalREAABasePercentChange = "TREAABasePercentChange";

        private const string cTotalRUnitBenefit = "TRUnitBenefit";
        private const string cTotalRUnitAmountChange = "TRUnitAmountChange";
        private const string cTotalRUnitPercentChange = "TRUnitPercentChange";
        private const string cTotalRUnitBaseChange = "TRUnitBaseChange";
        private const string cTotalRUnitBasePercentChange = "TRUnitBasePercentChange";

        public void InitTotalLCA1Change1Properties(LCA1Change1 ind)
        {
            ind.ErrorMessage = string.Empty;

            ind.TotalOCCost = 0;
            ind.TotalOCAmountChange = 0;
            ind.TotalOCPercentChange = 0;
            ind.TotalOCBaseChange = 0;
            ind.TotalOCBasePercentChange = 0;

            ind.TotalAOHCost = 0;
            ind.TotalAOHAmountChange = 0;
            ind.TotalAOHPercentChange = 0;
            ind.TotalAOHBaseChange = 0;
            ind.TotalAOHBasePercentChange = 0;

            ind.TotalCAPCost = 0;
            ind.TotalCAPAmountChange = 0;
            ind.TotalCAPPercentChange = 0;
            ind.TotalCAPBaseChange = 0;
            ind.TotalCAPBasePercentChange = 0;

            ind.TotalLCCCost = 0;
            ind.TotalLCCAmountChange = 0;
            ind.TotalLCCPercentChange = 0;
            ind.TotalLCCBaseChange = 0;
            ind.TotalLCCBasePercentChange = 0;

            ind.TotalEAACost = 0;
            ind.TotalEAAAmountChange = 0;
            ind.TotalEAAPercentChange = 0;
            ind.TotalEAABaseChange = 0;
            ind.TotalEAABasePercentChange = 0;

            ind.TotalUnitCost = 0;
            ind.TotalUnitAmountChange = 0;
            ind.TotalUnitPercentChange = 0;
            ind.TotalUnitBaseChange = 0;
            ind.TotalUnitBasePercentChange = 0;

            ind.TotalRBenefit = 0;
            ind.TotalRAmountChange = 0;
            ind.TotalRPercentChange = 0;
            ind.TotalRBaseChange = 0;
            ind.TotalRBasePercentChange = 0;

            ind.TotalLCBBenefit = 0;
            ind.TotalLCBAmountChange = 0;
            ind.TotalLCBPercentChange = 0;
            ind.TotalLCBBaseChange = 0;
            ind.TotalLCBBasePercentChange = 0;

            ind.TotalREAABenefit = 0;
            ind.TotalREAAAmountChange = 0;
            ind.TotalREAAPercentChange = 0;
            ind.TotalREAABaseChange = 0;
            ind.TotalREAABasePercentChange = 0;

            ind.TotalRUnitBenefit = 0;
            ind.TotalRUnitAmountChange = 0;
            ind.TotalRUnitPercentChange = 0;
            ind.TotalRUnitBaseChange = 0;
            ind.TotalRUnitBasePercentChange = 0;
            ind.CalcParameters = new CalculatorParameters();
            ind.SubP1Stock = new SubPrice1Stock();
            ind.SubP2Stock = new SubPrice2Stock();
        }

        public void CopyTotalLCA1Change1Properties(LCA1Change1 ind,
            LCA1Change1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalOCCost = calculator.TotalOCCost;
            ind.TotalOCAmountChange = calculator.TotalOCAmountChange;
            ind.TotalOCPercentChange = calculator.TotalOCPercentChange;
            ind.TotalOCBaseChange = calculator.TotalOCBaseChange;
            ind.TotalOCBasePercentChange = calculator.TotalOCBasePercentChange;

            ind.TotalAOHCost = calculator.TotalAOHCost;
            ind.TotalAOHAmountChange = calculator.TotalAOHAmountChange;
            ind.TotalAOHPercentChange = calculator.TotalAOHPercentChange;
            ind.TotalAOHBaseChange = calculator.TotalAOHBaseChange;
            ind.TotalAOHBasePercentChange = calculator.TotalAOHBasePercentChange;

            ind.TotalCAPCost = calculator.TotalCAPCost;
            ind.TotalCAPAmountChange = calculator.TotalCAPAmountChange;
            ind.TotalCAPPercentChange = calculator.TotalCAPPercentChange;
            ind.TotalCAPBaseChange = calculator.TotalCAPBaseChange;
            ind.TotalCAPBasePercentChange = calculator.TotalCAPBasePercentChange;

            ind.TotalLCCCost = calculator.TotalLCCCost;
            ind.TotalLCCAmountChange = calculator.TotalLCCAmountChange;
            ind.TotalLCCPercentChange = calculator.TotalLCCPercentChange;
            ind.TotalLCCBaseChange = calculator.TotalLCCBaseChange;
            ind.TotalLCCBasePercentChange = calculator.TotalLCCBasePercentChange;

            ind.TotalEAACost = calculator.TotalEAACost;
            ind.TotalEAAAmountChange = calculator.TotalEAAAmountChange;
            ind.TotalEAAPercentChange = calculator.TotalEAAPercentChange;
            ind.TotalEAABaseChange = calculator.TotalEAABaseChange;
            ind.TotalEAABasePercentChange = calculator.TotalEAABasePercentChange;

            ind.TotalUnitCost = calculator.TotalUnitCost;
            ind.TotalUnitAmountChange = calculator.TotalUnitAmountChange;
            ind.TotalUnitPercentChange = calculator.TotalUnitPercentChange;
            ind.TotalUnitBaseChange = calculator.TotalUnitBaseChange;
            ind.TotalUnitBasePercentChange = calculator.TotalUnitBasePercentChange;

            ind.TotalRBenefit = calculator.TotalRBenefit;
            ind.TotalRAmountChange = calculator.TotalRAmountChange;
            ind.TotalRPercentChange = calculator.TotalRPercentChange;
            ind.TotalRBaseChange = calculator.TotalRBaseChange;
            ind.TotalRBasePercentChange = calculator.TotalRBasePercentChange;

            ind.TotalLCBBenefit = calculator.TotalLCBBenefit;
            ind.TotalLCBAmountChange = calculator.TotalLCBAmountChange;
            ind.TotalLCBPercentChange = calculator.TotalLCBPercentChange;
            ind.TotalLCBBaseChange = calculator.TotalLCBBaseChange;
            ind.TotalLCBBasePercentChange = calculator.TotalLCBBasePercentChange;

            ind.TotalREAABenefit = calculator.TotalREAABenefit;
            ind.TotalREAAAmountChange = calculator.TotalREAAAmountChange;
            ind.TotalREAAPercentChange = calculator.TotalREAAPercentChange;
            ind.TotalREAABaseChange = calculator.TotalREAABaseChange;
            ind.TotalREAABasePercentChange = calculator.TotalREAABasePercentChange;

            ind.TotalRUnitBenefit = calculator.TotalRUnitBenefit;
            ind.TotalRUnitAmountChange = calculator.TotalRUnitAmountChange;
            ind.TotalRUnitPercentChange = calculator.TotalRUnitPercentChange;
            ind.TotalRUnitBaseChange = calculator.TotalRUnitBaseChange;
            ind.TotalRUnitBasePercentChange = calculator.TotalRUnitBasePercentChange;
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

        public void SetTotalLCA1Change1Properties(LCA1Change1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalOCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCCost, attNameExtension));
            ind.TotalOCAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCAmountChange, attNameExtension));
            ind.TotalOCPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCPercentChange, attNameExtension));
            ind.TotalOCBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCBaseChange, attNameExtension));
            ind.TotalOCBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCBasePercentChange, attNameExtension));

            ind.TotalAOHCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHCost, attNameExtension));
            ind.TotalAOHAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHAmountChange, attNameExtension));
            ind.TotalAOHPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHPercentChange, attNameExtension));
            ind.TotalAOHBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHBaseChange, attNameExtension));
            ind.TotalAOHBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHBasePercentChange, attNameExtension));

            ind.TotalCAPCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPCost, attNameExtension));
            ind.TotalCAPAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPAmountChange, attNameExtension));
            ind.TotalCAPPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPPercentChange, attNameExtension));
            ind.TotalCAPBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPBaseChange, attNameExtension));
            ind.TotalCAPBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPBasePercentChange, attNameExtension));

            ind.TotalLCCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCCost, attNameExtension));
            ind.TotalLCCAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCAmountChange, attNameExtension));
            ind.TotalLCCPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCPercentChange, attNameExtension));
            ind.TotalLCCBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCBaseChange, attNameExtension));
            ind.TotalLCCBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCBasePercentChange, attNameExtension));

            ind.TotalEAACost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAACost, attNameExtension));
            ind.TotalEAAAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAAmountChange, attNameExtension));
            ind.TotalEAAPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAPercentChange, attNameExtension));
            ind.TotalEAABaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAABaseChange, attNameExtension));
            ind.TotalEAABasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAABasePercentChange, attNameExtension));

            ind.TotalUnitCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitCost, attNameExtension));
            ind.TotalUnitAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitAmountChange, attNameExtension));
            ind.TotalUnitPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitPercentChange, attNameExtension));
            ind.TotalUnitBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitBaseChange, attNameExtension));
            ind.TotalUnitBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitBasePercentChange, attNameExtension));

            ind.TotalRBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRBenefit, attNameExtension));
            ind.TotalRAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountChange, attNameExtension));
            ind.TotalRPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPercentChange, attNameExtension));
            ind.TotalRBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRBaseChange, attNameExtension));
            ind.TotalRBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRBasePercentChange, attNameExtension));

            ind.TotalLCBBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBBenefit, attNameExtension));
            ind.TotalLCBAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBAmountChange, attNameExtension));
            ind.TotalLCBPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBPercentChange, attNameExtension));
            ind.TotalLCBBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBBaseChange, attNameExtension));
            ind.TotalLCBBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBBasePercentChange, attNameExtension));

            ind.TotalREAABenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAABenefit, attNameExtension));
            ind.TotalREAAAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAAmountChange, attNameExtension));
            ind.TotalREAAPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAPercentChange, attNameExtension));
            ind.TotalREAABaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAABaseChange, attNameExtension));
            ind.TotalREAABasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAABasePercentChange, attNameExtension));

            ind.TotalRUnitBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitBenefit, attNameExtension));
            ind.TotalRUnitAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitAmountChange, attNameExtension));
            ind.TotalRUnitPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitPercentChange, attNameExtension));
            ind.TotalRUnitBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitBaseChange, attNameExtension));
            ind.TotalRUnitBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitBasePercentChange, attNameExtension));
        }

        public void SetTotalLCA1Change1Property(LCA1Change1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalOCCost:
                    ind.TotalOCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCAmountChange:
                    ind.TotalOCAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCPercentChange:
                    ind.TotalOCPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCBaseChange:
                    ind.TotalOCBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCBasePercentChange:
                    ind.TotalOCBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHCost:
                    ind.TotalAOHCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHAmountChange:
                    ind.TotalAOHAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHPercentChange:
                    ind.TotalAOHPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHBaseChange:
                    ind.TotalAOHBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHBasePercentChange:
                    ind.TotalAOHBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPCost:
                    ind.TotalCAPCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPAmountChange:
                    ind.TotalCAPAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPPercentChange:
                    ind.TotalCAPPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPBaseChange:
                    ind.TotalCAPBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPBasePercentChange:
                    ind.TotalCAPBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCCost:
                    ind.TotalLCCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCAmountChange:
                    ind.TotalLCCAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCPercentChange:
                    ind.TotalLCCPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCBaseChange:
                    ind.TotalLCCBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCBasePercentChange:
                    ind.TotalLCCBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAACost:
                    ind.TotalEAACost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAAmountChange:
                    ind.TotalEAAAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAPercentChange:
                    ind.TotalEAAPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAABaseChange:
                    ind.TotalEAABaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAABasePercentChange:
                    ind.TotalEAABasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitCost:
                    ind.TotalUnitCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitAmountChange:
                    ind.TotalUnitAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitPercentChange:
                    ind.TotalUnitPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitBaseChange:
                    ind.TotalUnitBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitBasePercentChange:
                    ind.TotalUnitBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRBenefit:
                    ind.TotalRBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountChange:
                    ind.TotalRAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPercentChange:
                    ind.TotalRPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRBaseChange:
                    ind.TotalRBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRBasePercentChange:
                    ind.TotalRBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBBenefit:
                    ind.TotalLCBBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBAmountChange:
                    ind.TotalLCBAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBPercentChange:
                    ind.TotalLCBPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBBaseChange:
                    ind.TotalLCBBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBBasePercentChange:
                    ind.TotalLCBBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAABenefit:
                    ind.TotalREAABenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAAmountChange:
                    ind.TotalREAAAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAPercentChange:
                    ind.TotalREAAPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAABaseChange:
                    ind.TotalREAABaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAABasePercentChange:
                    ind.TotalREAABasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitBenefit:
                    ind.TotalRUnitBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitAmountChange:
                    ind.TotalRUnitAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitPercentChange:
                    ind.TotalRUnitPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitBaseChange:
                    ind.TotalRUnitBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitBasePercentChange:
                    ind.TotalRUnitBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalLCA1Change1Property(LCA1Change1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalOCCost:
                    sPropertyValue = ind.TotalOCCost.ToString();
                    break;
                case cTotalOCAmountChange:
                    sPropertyValue = ind.TotalOCAmountChange.ToString();
                    break;
                case cTotalOCPercentChange:
                    sPropertyValue = ind.TotalOCPercentChange.ToString();
                    break;
                case cTotalOCBaseChange:
                    sPropertyValue = ind.TotalOCBaseChange.ToString();
                    break;
                case cTotalOCBasePercentChange:
                    sPropertyValue = ind.TotalOCBasePercentChange.ToString();
                    break;
                case cTotalAOHCost:
                    sPropertyValue = ind.TotalAOHCost.ToString();
                    break;
                case cTotalAOHAmountChange:
                    sPropertyValue = ind.TotalAOHAmountChange.ToString();
                    break;
                case cTotalAOHPercentChange:
                    sPropertyValue = ind.TotalAOHPercentChange.ToString();
                    break;
                case cTotalAOHBaseChange:
                    sPropertyValue = ind.TotalAOHBaseChange.ToString();
                    break;
                case cTotalAOHBasePercentChange:
                    sPropertyValue = ind.TotalAOHBasePercentChange.ToString();
                    break;
                case cTotalCAPCost:
                    sPropertyValue = ind.TotalCAPCost.ToString();
                    break;
                case cTotalCAPAmountChange:
                    sPropertyValue = ind.TotalCAPAmountChange.ToString();
                    break;
                case cTotalCAPPercentChange:
                    sPropertyValue = ind.TotalCAPPercentChange.ToString();
                    break;
                case cTotalCAPBaseChange:
                    sPropertyValue = ind.TotalCAPBaseChange.ToString();
                    break;
                case cTotalCAPBasePercentChange:
                    sPropertyValue = ind.TotalCAPBasePercentChange.ToString();
                    break;
                case cTotalLCCCost:
                    sPropertyValue = ind.TotalLCCCost.ToString();
                    break;
                case cTotalLCCAmountChange:
                    sPropertyValue = ind.TotalLCCAmountChange.ToString();
                    break;
                case cTotalLCCPercentChange:
                    sPropertyValue = ind.TotalLCCPercentChange.ToString();
                    break;
                case cTotalLCCBaseChange:
                    sPropertyValue = ind.TotalLCCBaseChange.ToString();
                    break;
                case cTotalLCCBasePercentChange:
                    sPropertyValue = ind.TotalLCCBasePercentChange.ToString();
                    break;
                case cTotalEAACost:
                    sPropertyValue = ind.TotalEAACost.ToString();
                    break;
                case cTotalEAAAmountChange:
                    sPropertyValue = ind.TotalEAAAmountChange.ToString();
                    break;
                case cTotalEAAPercentChange:
                    sPropertyValue = ind.TotalEAAPercentChange.ToString();
                    break;
                case cTotalEAABaseChange:
                    sPropertyValue = ind.TotalEAABaseChange.ToString();
                    break;
                case cTotalEAABasePercentChange:
                    sPropertyValue = ind.TotalEAABasePercentChange.ToString();
                    break;
                case cTotalUnitCost:
                    sPropertyValue = ind.TotalUnitCost.ToString();
                    break;
                case cTotalUnitAmountChange:
                    sPropertyValue = ind.TotalUnitAmountChange.ToString();
                    break;
                case cTotalUnitPercentChange:
                    sPropertyValue = ind.TotalUnitPercentChange.ToString();
                    break;
                case cTotalUnitBaseChange:
                    sPropertyValue = ind.TotalUnitBaseChange.ToString();
                    break;
                case cTotalUnitBasePercentChange:
                    sPropertyValue = ind.TotalUnitBasePercentChange.ToString();
                    break;
                case cTotalRBenefit:
                    sPropertyValue = ind.TotalRBenefit.ToString();
                    break;
                case cTotalRAmountChange:
                    sPropertyValue = ind.TotalRAmountChange.ToString();
                    break;
                case cTotalRPercentChange:
                    sPropertyValue = ind.TotalRPercentChange.ToString();
                    break;
                case cTotalRBaseChange:
                    sPropertyValue = ind.TotalRBaseChange.ToString();
                    break;
                case cTotalRBasePercentChange:
                    sPropertyValue = ind.TotalRBasePercentChange.ToString();
                    break;
                case cTotalLCBBenefit:
                    sPropertyValue = ind.TotalLCBBenefit.ToString();
                    break;
                case cTotalLCBAmountChange:
                    sPropertyValue = ind.TotalLCBAmountChange.ToString();
                    break;
                case cTotalLCBPercentChange:
                    sPropertyValue = ind.TotalLCBPercentChange.ToString();
                    break;
                case cTotalLCBBaseChange:
                    sPropertyValue = ind.TotalLCBBaseChange.ToString();
                    break;
                case cTotalLCBBasePercentChange:
                    sPropertyValue = ind.TotalLCBBasePercentChange.ToString();
                    break;
                case cTotalREAABenefit:
                    sPropertyValue = ind.TotalREAABenefit.ToString();
                    break;
                case cTotalREAAAmountChange:
                    sPropertyValue = ind.TotalREAAAmountChange.ToString();
                    break;
                case cTotalREAAPercentChange:
                    sPropertyValue = ind.TotalREAAPercentChange.ToString();
                    break;
                case cTotalREAABaseChange:
                    sPropertyValue = ind.TotalREAABaseChange.ToString();
                    break;
                case cTotalREAABasePercentChange:
                    sPropertyValue = ind.TotalREAABasePercentChange.ToString();
                    break;
                case cTotalRUnitBenefit:
                    sPropertyValue = ind.TotalRUnitBenefit.ToString();
                    break;
                case cTotalRUnitAmountChange:
                    sPropertyValue = ind.TotalRUnitAmountChange.ToString();
                    break;
                case cTotalRUnitPercentChange:
                    sPropertyValue = ind.TotalRUnitPercentChange.ToString();
                    break;
                case cTotalRUnitBaseChange:
                    sPropertyValue = ind.TotalRUnitBaseChange.ToString();
                    break;
                case cTotalRUnitBasePercentChange:
                    sPropertyValue = ind.TotalRUnitBasePercentChange.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalLCA1Change1Attributes(LCA1Change1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCAmountChange, attNameExtension), ind.TotalOCAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCPercentChange, attNameExtension), ind.TotalOCPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCBaseChange, attNameExtension), ind.TotalOCBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCBasePercentChange, attNameExtension), ind.TotalOCBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHAmountChange, attNameExtension), ind.TotalAOHAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHPercentChange, attNameExtension), ind.TotalAOHPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHBaseChange, attNameExtension), ind.TotalAOHBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHBasePercentChange, attNameExtension), ind.TotalAOHBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPAmountChange, attNameExtension), ind.TotalCAPAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPPercentChange, attNameExtension), ind.TotalCAPPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPBaseChange, attNameExtension), ind.TotalCAPBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPBasePercentChange, attNameExtension), ind.TotalCAPBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCAmountChange, attNameExtension), ind.TotalLCCAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCPercentChange, attNameExtension), ind.TotalLCCPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCBaseChange, attNameExtension), ind.TotalLCCBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCBasePercentChange, attNameExtension), ind.TotalLCCBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAAmountChange, attNameExtension), ind.TotalEAAAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAPercentChange, attNameExtension), ind.TotalEAAPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAABaseChange, attNameExtension), ind.TotalEAABaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAABasePercentChange, attNameExtension), ind.TotalEAABasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitAmountChange, attNameExtension), ind.TotalUnitAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitPercentChange, attNameExtension), ind.TotalUnitPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitBaseChange, attNameExtension), ind.TotalUnitBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitBasePercentChange, attNameExtension), ind.TotalUnitBasePercentChange);
            }
            if (bIsBenefitNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountChange, attNameExtension), ind.TotalRAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPercentChange, attNameExtension), ind.TotalRPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRBaseChange, attNameExtension), ind.TotalRBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRBasePercentChange, attNameExtension), ind.TotalRBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBAmountChange, attNameExtension), ind.TotalLCBAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBPercentChange, attNameExtension), ind.TotalLCBPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBBaseChange, attNameExtension), ind.TotalLCBBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBBasePercentChange, attNameExtension), ind.TotalLCBBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAAmountChange, attNameExtension), ind.TotalREAAAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAPercentChange, attNameExtension), ind.TotalREAAPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAABaseChange, attNameExtension), ind.TotalREAABaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAABasePercentChange, attNameExtension), ind.TotalREAABasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitAmountChange, attNameExtension), ind.TotalRUnitAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitPercentChange, attNameExtension), ind.TotalRUnitPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitBaseChange, attNameExtension), ind.TotalRUnitBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitBasePercentChange, attNameExtension), ind.TotalRUnitBasePercentChange);
            }
        }

        public void SetTotalLCA1Change1Attributes(LCA1Change1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCAmountChange, attNameExtension), ind.TotalOCAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCPercentChange, attNameExtension), ind.TotalOCPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCBaseChange, attNameExtension), ind.TotalOCBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCBasePercentChange, attNameExtension), ind.TotalOCBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHAmountChange, attNameExtension), ind.TotalAOHAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHPercentChange, attNameExtension), ind.TotalAOHPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHBaseChange, attNameExtension), ind.TotalAOHBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHBasePercentChange, attNameExtension), ind.TotalAOHBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPAmountChange, attNameExtension), ind.TotalCAPAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPPercentChange, attNameExtension), ind.TotalCAPPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPBaseChange, attNameExtension), ind.TotalCAPBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPBasePercentChange, attNameExtension), ind.TotalCAPBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                   string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCAmountChange, attNameExtension), ind.TotalLCCAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCPercentChange, attNameExtension), ind.TotalLCCPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCBaseChange, attNameExtension), ind.TotalLCCBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCBasePercentChange, attNameExtension), ind.TotalLCCBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAAmountChange, attNameExtension), ind.TotalEAAAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAPercentChange, attNameExtension), ind.TotalEAAPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAABaseChange, attNameExtension), ind.TotalEAABaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAABasePercentChange, attNameExtension), ind.TotalEAABasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitAmountChange, attNameExtension), ind.TotalUnitAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitPercentChange, attNameExtension), ind.TotalUnitPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitBaseChange, attNameExtension), ind.TotalUnitBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitBasePercentChange, attNameExtension), ind.TotalUnitBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsBenefitNode || bIsBoth)
            {

                writer.WriteAttributeString(
                    string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountChange, attNameExtension), ind.TotalRAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPercentChange, attNameExtension), ind.TotalRPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRBaseChange, attNameExtension), ind.TotalRBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRBasePercentChange, attNameExtension), ind.TotalRBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBAmountChange, attNameExtension), ind.TotalLCBAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBPercentChange, attNameExtension), ind.TotalLCBPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBBaseChange, attNameExtension), ind.TotalLCBBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBBasePercentChange, attNameExtension), ind.TotalLCBBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAAmountChange, attNameExtension), ind.TotalREAAAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAPercentChange, attNameExtension), ind.TotalREAAPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAABaseChange, attNameExtension), ind.TotalREAABaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAABasePercentChange, attNameExtension), ind.TotalREAABasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitAmountChange, attNameExtension), ind.TotalRUnitAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitPercentChange, attNameExtension), ind.TotalRUnitPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitBaseChange, attNameExtension), ind.TotalRUnitBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitBasePercentChange, attNameExtension), ind.TotalRUnitBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));
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
                lca1Stock.Change1 = new LCA1Change1();
                //need one property set
                lca1Stock.Change1.SubApplicationType = lca1Stock.CalcParameters.SubApplicationType.ToString();
                CopyTotalToChangeStock(lca1Stock.Change1, lca1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing change analysis
        public bool RunAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //set calculated changestocks
            List<LCA1Stock> changeStocks = new List<LCA1Stock>();
            if (lca1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || lca1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                changeStocks = SetIOAnalyses(lca1Stock, calcs);
            }
            else
            {
                if (lca1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || lca1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no changes
                    changeStocks = SetTotals(lca1Stock, calcs);
                }
                else if (lca1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || lca1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //tps with currentnodename set only need nets (inputs minus outputs)
                    //note that only sb1stock is used (not changestocks)
                    changeStocks = SetTotals(lca1Stock, calcs);
                }
                else
                {
                    changeStocks = SetAnalyses(lca1Stock, calcs);
                }
            }
            //add the changestocks to parent stock
            if (changeStocks != null)
            {
                bHasAnalyses = AddChangeStocksToBaseStock(lca1Stock, changeStocks);
                //lca1Stock must still add the members of change1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        private List<LCA1Stock> SetTotals(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            List<LCA1Stock> changeStocks = new List<LCA1Stock>();
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
                        if (stock.Change1 != null)
                        {
                            //calc holds an input or output stock
                            //add that stock to lca1stock (some analyses will need to use subprices too)
                            bHasTotals = AddSubTotalToTotalStock(lca1Stock.Change1, stock.Multiplier, stock.Change1);
                            if (bHasTotals)
                            {
                                changeStocks.Add(lca1Stock);
                            }
                        }
                    }
                }
            }
            return changeStocks;
        }
        private List<LCA1Stock> SetAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            List<LCA1Stock> changeStocks = new List<LCA1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of lca1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Change1.SubPrice1Stocks for costs and 2s for benefits
            //set N
            int iCostN = 0;
            int iBenefitN = 0;
            //calcs are aggregated by their alternative2 property
            //so calcs with alt2 = 0 are in first observation; alt2 = 2nd observation
            //put the calc totals in each observation and then run stats on observations (not calcs)
            IEnumerable<System.Linq.IGrouping<int, Calculator1>>
                calcsByAlt2 = calcs.GroupBy(c => c.Alternative2);
            foreach (var calcbyalt in calcsByAlt2)
            {
                //set the calc totals in each observation
                LCA1Stock observationStock = new LCA1Stock();
                observationStock.Change1 = new LCA1Change1();
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(lca1Stock.GetType()))
                    {
                        LCA1Stock stock = (LCA1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Change1 != null)
                            {
                                LCA1Stock observation2Stock = new LCA1Stock();
                                //168 need calc.Mults not agg.Mults
                                //stock.Multiplier = lca1Stock.Multiplier;
                                bHasTotals = SetObservationStock(changeStocks, calc,
                                    stock, observation2Stock);
                                if (bHasTotals)
                                {
                                    //add to the stats collection
                                    changeStocks.Add(observation2Stock);
                                    //N is determined from the cost SubP1Stock
                                    if (observation2Stock.Change1.SubP1Stock != null)
                                    {
                                        if (observation2Stock.Change1.SubP1Stock.SubPrice1Stocks != null)
                                        {
                                            if (observation2Stock.Change1.SubP1Stock.SubPrice1Stocks.Count > 0)
                                            {
                                                iCostN++;
                                            }
                                        }
                                    }
                                    //and from the benefit SubP2Stock
                                    if (observation2Stock.Change1.SubP2Stock != null)
                                    {
                                        if (observation2Stock.Change1.SubP2Stock.SubPrice2Stocks != null)
                                        {
                                            if (observation2Stock.Change1.SubP2Stock.SubPrice2Stocks.Count > 0)
                                            {
                                                iBenefitN++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (iCostN > 0 || iBenefitN > 0)
            {
                bHasTotals = SetChangesAnalysis(changeStocks, lca1Stock, iCostN, iBenefitN);
            }
            return changeStocks;
        }
        private List<LCA1Stock> SetIOAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            List<LCA1Stock> changeStocks = new List<LCA1Stock>();
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iCostN2 = 0;
            int iBenefitN2 = 0;
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(lca1Stock.GetType()))
                {
                    LCA1Stock stock = (LCA1Stock)calc;
                    if (stock != null)
                    {
                        //stock.Change1 holds the initial substock/price totals
                        if (stock.Change1 != null)
                        {
                            LCA1Stock observation2Stock = new LCA1Stock();
                            stock.Multiplier = lca1Stock.Multiplier;
                            bHasTotals = SetObservationStock(changeStocks, calc, 
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                changeStocks.Add(observation2Stock);
                                //N is determined from the cost SubP1Stock
                                if (observation2Stock.Change1.SubP1Stock != null)
                                {
                                    if (observation2Stock.Change1.SubP1Stock.SubPrice1Stocks != null)
                                    {
                                        if (observation2Stock.Change1.SubP1Stock.SubPrice1Stocks.Count > 0)
                                        {
                                            iCostN2++;
                                        }
                                    }
                                }
                                //and from the benefit SubP2Stock
                                if (observation2Stock.Change1.SubP2Stock != null)
                                {
                                    if (observation2Stock.Change1.SubP2Stock.SubPrice2Stocks != null)
                                    {
                                        if (observation2Stock.Change1.SubP2Stock.SubPrice2Stocks.Count > 0)
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
                bHasTotals = SetChangesAnalysis(changeStocks, lca1Stock, iCostN2, iBenefitN2);
            }
            return changeStocks;
        }
        private bool SetObservationStock(List<LCA1Stock> changeStocks,
            Calculator1 calc, LCA1Stock stock, LCA1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Change1 = new LCA1Change1();
            observation2Stock.Id = stock.Id;
            observation2Stock.Change1.Id = stock.Id;
            //copy some stock props to progress1
            BILCA1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock.Change1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BILCA1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Change1.CalcParameters == null)
                stock.Change1.CalcParameters = new CalculatorParameters();
            stock.Change1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //at oc and outcome level no aggregating by year, id or alt
            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Change1,
                stock.Multiplier, stock.Change1);
            return bHasTotals;
        }
        private bool SetChangesAnalysis(List<LCA1Stock> changeStocks, LCA1Stock lca1Stock,
            int costN, int benN)
        {
            bool bHasTotals = false;
            //set the total observations total
            bool bHasCurrents = changeStocks.Any(c => c.ChangeType == Calculator1.CHANGE_TYPES.current.ToString());
            foreach (var stat in changeStocks)
            {
                //only current gets added to parent cumulative totals
                if (stat.ChangeType == Calculator1.CHANGE_TYPES.current.ToString())
                {
                    bHasTotals = AddSubTotalToTotalStock(lca1Stock.Change1, 1, stat.Change1);
                }
                else
                {
                    if (!bHasCurrents)
                    {
                        //no changes?, straight totals needed
                        bHasTotals = AddSubTotalToTotalStock(lca1Stock.Change1, 1, stat.Change1);
                    }
                }
            }
            if (costN > 0)
            {
                //if any changestock has this property, it's trying to compare antecedents, rather than siblings
                if (bHasCurrents)
                {
                    //budgets uses antecendent, rather than sibling, comparators
                    SetOCBudgetChanges(lca1Stock, changeStocks);
                    SetAOHBudgetChanges(lca1Stock, changeStocks);
                    SetCAPBudgetChanges(lca1Stock, changeStocks);
                    SetLCCBudgetChanges(lca1Stock, changeStocks);
                    SetEAABudgetChanges(lca1Stock, changeStocks);
                    SetUnitBudgetChanges(lca1Stock, changeStocks);
                }
                else
                {
                    //set change numbers
                    SetOCChanges(lca1Stock, changeStocks);
                    SetAOHChanges(lca1Stock, changeStocks);
                    SetCAPChanges(lca1Stock, changeStocks);
                    SetLCCChanges(lca1Stock, changeStocks);
                    SetEAAChanges(lca1Stock, changeStocks);
                    SetUnitChanges(lca1Stock, changeStocks);
                }
                bHasTotals = true;
            }
            if (benN > 0)
            {
                //benefits
                //if any changestock has this property, it's trying to compare antecedents, rather than siblings
                if (bHasCurrents)
                {

                    //budgets uses antecendent, rather than sibling, comparators
                    SetRBudgetChanges(lca1Stock, changeStocks);
                    SetLCBBudgetChanges(lca1Stock, changeStocks);
                    SetREAABudgetChanges(lca1Stock, changeStocks);
                    SetRUnitBudgetChanges(lca1Stock, changeStocks);
                }
                else
                {
                    SetRChanges(lca1Stock, changeStocks);
                    SetLCBChanges(lca1Stock, changeStocks);
                    SetREAAChanges(lca1Stock, changeStocks);
                    SetRUnitChanges(lca1Stock, changeStocks);
                }
                bHasTotals = true;
            }
            if (benN > 0 || costN > 0)
            {
                //remove the comparators (only display the actual)
                changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.xminus1.ToString());
                changeStocks.RemoveAll(c => c.ChangeType == Calculator1.CHANGE_TYPES.baseline.ToString());
            }
            return bHasTotals;
        }

        public bool CopyTotalToChangeStock(LCA1Change1 totStock, LCA1Total1 subTotal)
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
        private void CopySubStock1Totals(LCA1Change1 totStock, LCA1Total1 subTotal)
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
        private void CopySubStock2Totals(LCA1Change1 totStock, LCA1Total1 subTotal)
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
        public bool AddSubTotalToTotalStock(LCA1Change1 totStock, double multiplier,
            LCA1Change1 subTotal)
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

        private void AddSubStock1Totals(LCA1Change1 totStock, LCA1Change1 subTotal)
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
        private void AddSubStock2Totals(LCA1Change1 totStock, LCA1Change1 subTotal)
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
        
        public static void ChangeSubTotalByMultipliers(LCA1Change1 subTotal, double multiplier)
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
        private static void SetOCBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalOCBaseChange = stat.Change1.TotalOCCost - benchmark.Change1.TotalOCCost; ;
                        stat.Change1.TotalOCBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalOCBaseChange, benchmark.Change1.TotalOCCost);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalOCAmountChange
                            = stat.Change1.TotalOCCost - xminus1.Change1.TotalOCCost;
                        stat.Change1.TotalOCPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalOCAmountChange, xminus1.Change1.TotalOCCost);
                    }
                }
            }
        }
        private static LCA1Stock GetChangeStockByLabel(LCA1Stock actual, List<int> ids,
            List<LCA1Stock> changeStocks, string changeType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            LCA1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (changeStocks.Any(p => p.Label == actual.Label
                && p.ChangeType == changeType))
            {
                int iIndex = 1;
                foreach (LCA1Stock change in changeStocks)
                {
                    if (change.ChangeType == changeType)
                    {
                        if (actual.Label == change.Label)
                        {
                            //make sure it hasn't already been used (2 or more els with same Labels)
                            if (!ids.Any(i => i == iIndex))
                            {
                                plannedMatch = change;
                                //index based check is ok
                                ids.Add(iIndex);
                                //break the for loop
                                break;
                            }
                            else
                            {
                                //break if no remaining planned has same label
                                bool bHasMatch = HasChangeMatchByLabel(actual.Label, change,
                                    changeStocks, changeType);
                                if (!bHasMatch)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    iIndex++;
                }
            }
            return plannedMatch;
        }
        private static bool HasChangeMatchByLabel(string aggLabel,
            LCA1Stock change, List<LCA1Stock> changeStocks, string changeType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (LCA1Stock rp in changeStocks)
            {
                if (rp.ChangeType == changeType)
                {
                    if (bStart)
                    {
                        if (aggLabel == change.Label)
                        {
                            bHasMatch = true;
                            break;
                        }
                    }
                    if (rp.Id == change.Id)
                    {
                        bStart = true;
                    }
                }
            }
            return bHasMatch;
        }
        private static void SetAOHBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAOHBaseChange = stat.Change1.TotalAOHCost - benchmark.Change1.TotalAOHCost; ;
                        stat.Change1.TotalAOHBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAOHBaseChange, benchmark.Change1.TotalAOHCost);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAOHAmountChange
                            = stat.Change1.TotalAOHCost - xminus1.Change1.TotalAOHCost;
                        stat.Change1.TotalAOHPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAOHAmountChange, xminus1.Change1.TotalAOHCost);
                    }
                }
            }
        }
        private static void SetCAPBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalCAPBaseChange = stat.Change1.TotalCAPCost - benchmark.Change1.TotalCAPCost; ;
                        stat.Change1.TotalCAPBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalCAPBaseChange, benchmark.Change1.TotalCAPCost);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalCAPAmountChange
                            = stat.Change1.TotalCAPCost - xminus1.Change1.TotalCAPCost;
                        stat.Change1.TotalCAPPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalCAPAmountChange, xminus1.Change1.TotalCAPCost);
                    }
                }
            }
        }
        private static void SetLCCBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalLCCBaseChange = stat.Change1.TotalLCCCost - benchmark.Change1.TotalLCCCost; ;
                        stat.Change1.TotalLCCBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalLCCBaseChange, benchmark.Change1.TotalLCCCost);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalLCCAmountChange
                            = stat.Change1.TotalLCCCost - xminus1.Change1.TotalLCCCost;
                        stat.Change1.TotalLCCPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalLCCAmountChange, xminus1.Change1.TotalLCCCost);
                    }
                }
            }
        }
        private static void SetEAABudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalEAABaseChange = stat.Change1.TotalEAACost - benchmark.Change1.TotalEAACost; ;
                        stat.Change1.TotalEAABasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalEAABaseChange, benchmark.Change1.TotalEAACost);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalEAAAmountChange
                            = stat.Change1.TotalEAACost - xminus1.Change1.TotalEAACost;
                        stat.Change1.TotalEAAPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalEAAAmountChange, xminus1.Change1.TotalEAACost);
                    }
                }
            }
        }
        private static void SetUnitBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalUnitBaseChange = stat.Change1.TotalUnitCost - benchmark.Change1.TotalUnitCost; ;
                        stat.Change1.TotalUnitBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalUnitBaseChange, benchmark.Change1.TotalUnitCost);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalUnitAmountChange
                            = stat.Change1.TotalUnitCost - xminus1.Change1.TotalUnitCost;
                        stat.Change1.TotalUnitPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalUnitAmountChange, xminus1.Change1.TotalUnitCost);
                    }
                }
            }
        }
        private static void SetRBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalRBaseChange = stat.Change1.TotalRBenefit - benchmark.Change1.TotalRBenefit; ;
                        stat.Change1.TotalRBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRBaseChange, benchmark.Change1.TotalRBenefit);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalRAmountChange
                            = stat.Change1.TotalRBenefit - xminus1.Change1.TotalRBenefit;
                        stat.Change1.TotalRPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRAmountChange, xminus1.Change1.TotalRBenefit);
                    }
                }
            }
        }
        private static void SetLCBBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalLCBBaseChange = stat.Change1.TotalLCBBenefit - benchmark.Change1.TotalLCBBenefit; ;
                        stat.Change1.TotalLCBBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalLCBBaseChange, benchmark.Change1.TotalLCBBenefit);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalLCBAmountChange
                            = stat.Change1.TotalLCBBenefit - xminus1.Change1.TotalLCBBenefit;
                        stat.Change1.TotalLCBPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalLCBAmountChange, xminus1.Change1.TotalLCBBenefit);
                    }
                }
            }
        }
        private static void SetREAABudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalREAABaseChange = stat.Change1.TotalREAABenefit - benchmark.Change1.TotalREAABenefit; ;
                        stat.Change1.TotalREAABasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalREAABaseChange, benchmark.Change1.TotalREAABenefit);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalREAAAmountChange
                            = stat.Change1.TotalREAABenefit - xminus1.Change1.TotalREAABenefit;
                        stat.Change1.TotalREAAPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalREAAAmountChange, xminus1.Change1.TotalREAABenefit);
                    }
                }
            }
        }
        private static void SetRUnitBudgetChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    LCA1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalRUnitBaseChange = stat.Change1.TotalRUnitBenefit - benchmark.Change1.TotalRUnitBenefit; ;
                        stat.Change1.TotalRUnitBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRUnitBaseChange, benchmark.Change1.TotalRUnitBenefit);
                    }
                    //set the xminus change using partialtarget tt
                    LCA1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalRUnitAmountChange
                            = stat.Change1.TotalRUnitBenefit - xminus1.Change1.TotalRUnitBenefit;
                        stat.Change1.TotalRUnitPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRUnitAmountChange, xminus1.Change1.TotalRUnitBenefit);
                    }
                }
            }
        }
        private static void SetOCChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalOCCost = 0;
            double dbLastTotalOCCost = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTotalOCCost = stat.Change1.TotalOCCost;
                }
                else
                {
                    if (dbLastTotalOCCost != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalOCAmountChange = stat.Change1.TotalOCCost - dbLastTotalOCCost;
                        stat.Change1.TotalOCPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalOCAmountChange, dbLastTotalOCCost);
                    }
                    dbLastTotalOCCost = stat.Change1.TotalOCCost;

                    stat.Change1.TotalOCBaseChange = stat.Change1.TotalOCCost - dbBaseTotalOCCost;
                    stat.Change1.TotalOCBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalOCBaseChange, dbBaseTotalOCCost);
                }
                i++;
            }
        }
        private static void SetAOHChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalAOHCost = 0;
            double dbLastTotalAOHCost = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAOHCost = stat.Change1.TotalAOHCost;
                }
                else
                {
                    if (dbLastTotalAOHCost != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAOHAmountChange = stat.Change1.TotalAOHCost - dbLastTotalAOHCost;
                        stat.Change1.TotalAOHPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAOHAmountChange, dbLastTotalAOHCost);
                    }
                    dbLastTotalAOHCost = stat.Change1.TotalAOHCost;
                    stat.Change1.TotalAOHBaseChange = stat.Change1.TotalAOHCost - dbBaseTotalAOHCost;
                    stat.Change1.TotalAOHBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAOHBaseChange, dbBaseTotalAOHCost);
                }
                i++;
            }
        }
        private static void SetCAPChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalCAPCost = 0;
            double dbLastTotalCAPCost = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalCAPCost = stat.Change1.TotalCAPCost;
                }
                else
                {
                    if (dbLastTotalCAPCost != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalCAPAmountChange = stat.Change1.TotalCAPCost - dbLastTotalCAPCost;
                        stat.Change1.TotalCAPPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalCAPAmountChange, dbLastTotalCAPCost);
                    }
                    dbLastTotalCAPCost = stat.Change1.TotalCAPCost;
                    stat.Change1.TotalCAPBaseChange = stat.Change1.TotalCAPCost - dbBaseTotalCAPCost;
                    stat.Change1.TotalCAPBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalCAPBaseChange, dbBaseTotalCAPCost);
                }
                i++;
            }
        }
        private static void SetLCCChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalLCCCost = 0;
            double dbLastTotalLCCCost = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalLCCCost = stat.Change1.TotalLCCCost;
                }
                else
                {
                    //set the annual change numbers
                    if (dbLastTotalLCCCost != 0)
                    {
                        stat.Change1.TotalLCCAmountChange = stat.Change1.TotalLCCCost - dbLastTotalLCCCost;
                        stat.Change1.TotalLCCPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalLCCAmountChange, dbLastTotalLCCCost);
                    }
                    dbLastTotalLCCCost = stat.Change1.TotalLCCCost;
                    stat.Change1.TotalLCCBaseChange = stat.Change1.TotalLCCCost - dbBaseTotalLCCCost;
                    stat.Change1.TotalLCCBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalLCCBaseChange, dbBaseTotalLCCCost);
                }
                i++;
            }
        }
        private static void SetEAAChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalEAACost = 0;
            double dbLastTotalEAACost = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalEAACost = stat.Change1.TotalEAACost;
                }
                else
                {
                    if (dbLastTotalEAACost != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalEAAAmountChange = stat.Change1.TotalEAACost - dbLastTotalEAACost;
                        stat.Change1.TotalEAAPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalEAAAmountChange, dbLastTotalEAACost);
                    }
                    dbLastTotalEAACost = stat.Change1.TotalEAACost;
                    stat.Change1.TotalEAABaseChange = stat.Change1.TotalEAACost - dbBaseTotalEAACost;
                    stat.Change1.TotalEAABasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalEAABaseChange, dbBaseTotalEAACost);
                }
                i++;
            }
        }
        private static void SetUnitChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalUnitCost = 0;
            double dbLastTotalUnitCost = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalUnitCost = stat.Change1.TotalUnitCost;
                }
                else
                {
                    if (dbLastTotalUnitCost != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalUnitAmountChange = stat.Change1.TotalUnitCost - dbLastTotalUnitCost;
                        stat.Change1.TotalUnitPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalUnitAmountChange, dbLastTotalUnitCost);
                    }
                    dbLastTotalUnitCost = stat.Change1.TotalUnitCost;
                    stat.Change1.TotalUnitBaseChange = stat.Change1.TotalUnitCost - dbBaseTotalUnitCost;
                    stat.Change1.TotalUnitBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalUnitBaseChange, dbBaseTotalUnitCost);
                }
                i++;
            }
        }
        private static void SetRChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalRBenefit = 0;
            double dbLastTotalRBenefit = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalRBenefit = stat.Change1.TotalRBenefit;
                }
                else
                {
                    if (dbLastTotalRBenefit != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalRAmountChange = stat.Change1.TotalRBenefit - dbLastTotalRBenefit;
                        stat.Change1.TotalRPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRAmountChange, dbLastTotalRBenefit);
                    }
                    dbLastTotalRBenefit = stat.Change1.TotalRBenefit;
                    stat.Change1.TotalRBaseChange = stat.Change1.TotalRBenefit - dbBaseTotalRBenefit;
                    stat.Change1.TotalRBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRBaseChange, dbBaseTotalRBenefit);
                }
                i++;
            }
        }
        private static void SetLCBChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalLCBBenefit = 0;
            double dbLastTotalLCBBenefit = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalLCBBenefit = stat.Change1.TotalLCBBenefit;
                }
                else
                {
                    if (dbLastTotalLCBBenefit != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalLCBAmountChange = stat.Change1.TotalLCBBenefit - dbLastTotalLCBBenefit;
                        stat.Change1.TotalLCBPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalLCBAmountChange, dbLastTotalLCBBenefit);
                    }
                    dbLastTotalLCBBenefit = stat.Change1.TotalLCBBenefit;
                    //set the change from first tp numbers
                    stat.Change1.TotalLCBBaseChange = stat.Change1.TotalLCBBenefit - dbBaseTotalLCBBenefit;
                    stat.Change1.TotalLCBBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalLCBBaseChange, dbBaseTotalLCBBenefit);
                }
                i++;
            }
        }
        private static void SetREAAChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalREAABenefit = 0;
            double dbLastTotalREAABenefit = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalREAABenefit = stat.Change1.TotalREAABenefit;
                }
                else
                {
                    if (dbLastTotalREAABenefit != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalREAAAmountChange = stat.Change1.TotalREAABenefit - dbLastTotalREAABenefit;
                        stat.Change1.TotalREAAPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalREAAAmountChange, dbLastTotalREAABenefit);
                    }
                    dbLastTotalREAABenefit = stat.Change1.TotalREAABenefit;
                    stat.Change1.TotalREAABaseChange = stat.Change1.TotalREAABenefit - dbBaseTotalREAABenefit;
                    stat.Change1.TotalREAABasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalREAABaseChange, dbBaseTotalREAABenefit);
                }
                i++;
            }
        }
        private static void SetRUnitChanges(LCA1Stock lca1Stock, List<LCA1Stock> changeStocks)
        {
            double dbBaseTotalRUnitBenefit = 0;
            double dbLastTotalRUnitBenefit = 0;
            int i = 0;
            foreach (LCA1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalRUnitBenefit = stat.Change1.TotalRUnitBenefit;
                }
                else
                {
                    if (dbLastTotalRUnitBenefit != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalRUnitAmountChange = stat.Change1.TotalRUnitBenefit - dbLastTotalRUnitBenefit;
                        stat.Change1.TotalRUnitPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRUnitAmountChange, dbLastTotalRUnitBenefit);
                    }
                    dbLastTotalRUnitBenefit = stat.Change1.TotalRUnitBenefit;
                    stat.Change1.TotalRUnitBaseChange = stat.Change1.TotalRUnitBenefit - dbBaseTotalRUnitBenefit;
                    stat.Change1.TotalRUnitBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRUnitBaseChange, dbBaseTotalRUnitBenefit);
                }
                i++;
            }
        }
        private bool AddChangeStocksToBaseStock(LCA1Stock lca1Stock,
            List<LCA1Stock> changeStocks)
        {
            bool bHasAnalyses = false;
            lca1Stock.Stocks = new List<LCA1Stock>();
            foreach (LCA1Stock changeStock in changeStocks)
            {
                //add it to the list
                lca1Stock.Stocks.Add(changeStock);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
    public static class LCA1Change1Extensions
    {
        public static void AddSubStock1ToTotalStocks(this LCA1Change1 baseStat, SubPrice1Stock substock)
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
        public static void AddSubStock2ToTotalStocks(this LCA1Change1 baseStat, SubPrice2Stock substock)
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