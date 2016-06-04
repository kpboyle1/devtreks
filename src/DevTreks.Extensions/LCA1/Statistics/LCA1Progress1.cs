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
    ///             The class measures planned vs actual progress.
    ///Author:		www.devtreks.org
    ///Date:		2014, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class LCA1Progress1 : LCA1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public LCA1Progress1()
            : base()
        {
            //subprice object
            InitTotalLCA1Progress1Properties(this);
        }
        //copy constructor
        public LCA1Progress1(LCA1Progress1 calculator)
            : base(calculator)
        {
            CopyTotalLCA1Progress1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent LCA1Stock
        //calculator properties
        //totals names must be consistent with Total1
        //planned period
        public double TotalOCCost { get; set; }
        //planned full (sum of all planning periods)
        public double TotalOCPFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalOCPCTotal { get; set; }
        //actual period
        public double TotalOCAPTotal { get; set; }
        //actual cumulative 
        public double TotalOCACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalOCAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalOCACChange { get; set; }
        //planned period
        public double TotalOCPPPercent { get; set; }
        //planned cumulative
        public double TotalOCPCPercent { get; set; }
        //planned full
        public double TotalOCPFPercent { get; set; }

        public double TotalAOHCost { get; set; }
        public double TotalAOHPFTotal { get; set; }
        public double TotalAOHPCTotal { get; set; }
        public double TotalAOHAPTotal { get; set; }
        public double TotalAOHACTotal { get; set; }
        public double TotalAOHAPChange { get; set; }
        public double TotalAOHACChange { get; set; }
        public double TotalAOHPPPercent { get; set; }
        public double TotalAOHPCPercent { get; set; }
        public double TotalAOHPFPercent { get; set; }

        public double TotalCAPCost { get; set; }
        public double TotalCAPPFTotal { get; set; }
        public double TotalCAPPCTotal { get; set; }
        public double TotalCAPAPTotal { get; set; }
        public double TotalCAPACTotal { get; set; }
        public double TotalCAPAPChange { get; set; }
        public double TotalCAPACChange { get; set; }
        public double TotalCAPPPPercent { get; set; }
        public double TotalCAPPCPercent { get; set; }
        public double TotalCAPPFPercent { get; set; }

        //total lcc cost
        public double TotalLCCCost { get; set; }
        public double TotalLCCPFTotal { get; set; }
        public double TotalLCCPCTotal { get; set; }
        public double TotalLCCAPTotal { get; set; }
        public double TotalLCCACTotal { get; set; }
        public double TotalLCCAPChange { get; set; }
        public double TotalLCCACChange { get; set; }
        public double TotalLCCPPPercent { get; set; }
        public double TotalLCCPCPercent { get; set; }
        public double TotalLCCPFPercent { get; set; }

        //total eaa cost (equiv ann annuity)
        public double TotalEAACost { get; set; }
        public double TotalEAAPFTotal { get; set; }
        public double TotalEAAPCTotal { get; set; }
        public double TotalEAAAPTotal { get; set; }
        public double TotalEAAACTotal { get; set; }
        public double TotalEAAAPChange { get; set; }
        public double TotalEAAACChange { get; set; }
        public double TotalEAAPPPercent { get; set; }
        public double TotalEAAPCPercent { get; set; }
        public double TotalEAAPFPercent { get; set; }

        //total per unit costs
        public double TotalUnitCost { get; set; }
        public double TotalUnitPFTotal { get; set; }
        public double TotalUnitPCTotal { get; set; }
        public double TotalUnitAPTotal { get; set; }
        public double TotalUnitACTotal { get; set; }
        public double TotalUnitAPChange { get; set; }
        public double TotalUnitACChange { get; set; }
        public double TotalUnitPPPercent { get; set; }
        public double TotalUnitPCPercent { get; set; }
        public double TotalUnitPFPercent { get; set; }

        private const string cTotalOCCost = "TOCCost";
        private const string cTotalOCPFTotal = "TOCPFTotal";
        private const string cTotalOCPCTotal = "TOCPCTotal";
        private const string cTotalOCAPTotal = "TOCAPTotal";
        private const string cTotalOCACTotal = "TOCACTotal";
        private const string cTotalOCAPChange = "TOCAPChange";
        private const string cTotalOCACChange = "TOCACChange";
        private const string cTotalOCPPPercent = "TOCPPPercent";
        private const string cTotalOCPCPercent = "TOCPCPercent";
        private const string cTotalOCPFPercent = "TOCPFPercent";

        private const string cTotalAOHCost = "TAOHCost";
        private const string cTotalAOHPFTotal = "TAOHPFTotal";
        private const string cTotalAOHPCTotal = "TAOHPCTotal";
        private const string cTotalAOHAPTotal = "TAOHAPTotal";
        private const string cTotalAOHACTotal = "TAOHACTotal";
        private const string cTotalAOHAPChange = "TAOHAPChange";
        private const string cTotalAOHACChange = "TAOHACChange";
        private const string cTotalAOHPPPercent = "TAOHPPPercent";
        private const string cTotalAOHPCPercent = "TAOHPCPercent";
        private const string cTotalAOHPFPercent = "TAOHPFPercent";

        private const string cTotalCAPCost = "TCAPCost";
        private const string cTotalCAPPFTotal = "TCAPPFTotal";
        private const string cTotalCAPPCTotal = "TCAPPCTotal";
        private const string cTotalCAPAPTotal = "TCAPAPTotal";
        private const string cTotalCAPACTotal = "TCAPACTotal";
        private const string cTotalCAPAPChange = "TCAPAPChange";
        private const string cTotalCAPACChange = "TCAPACChange";
        private const string cTotalCAPPPPercent = "TCAPPPPercent";
        private const string cTotalCAPPCPercent = "TCAPPCPercent";
        private const string cTotalCAPPFPercent = "TCAPPFPercent";

        private const string cTotalLCCCost = "TLCCCost";
        private const string cTotalLCCPFTotal = "TLCCPFTotal";
        private const string cTotalLCCPCTotal = "TLCCPCTotal";
        private const string cTotalLCCAPTotal = "TLCCAPTotal";
        private const string cTotalLCCACTotal = "TLCCACTotal";
        private const string cTotalLCCAPChange = "TLCCAPChange";
        private const string cTotalLCCACChange = "TLCCACChange";
        private const string cTotalLCCPPPercent = "TLCCPPPercent";
        private const string cTotalLCCPCPercent = "TLCCPCPercent";
        private const string cTotalLCCPFPercent = "TLCCPFPercent";

        private const string cTotalEAACost = "TEAACost";
        private const string cTotalEAAPFTotal = "TEAAPFTotal";
        private const string cTotalEAAPCTotal = "TEAAPCTotal";
        private const string cTotalEAAAPTotal = "TEAAAPTotal";
        private const string cTotalEAAACTotal = "TEAAACTotal";
        private const string cTotalEAAAPChange = "TEAAAPChange";
        private const string cTotalEAAACChange = "TEAAACChange";
        private const string cTotalEAAPPPercent = "TEAAPPPercent";
        private const string cTotalEAAPCPercent = "TEAAPCPercent";
        private const string cTotalEAAPFPercent = "TEAAPFPercent";

        private const string cTotalUnitCost = "TUnitCost";
        private const string cTotalUnitPFTotal = "TUnitPFTotal";
        private const string cTotalUnitPCTotal = "TUnitPCTotal";
        private const string cTotalUnitAPTotal = "TUnitAPTotal";
        private const string cTotalUnitACTotal = "TUnitACTotal";
        private const string cTotalUnitAPChange = "TUnitAPChange";
        private const string cTotalUnitACChange = "TUnitACChange";
        private const string cTotalUnitPPPercent = "TUnitPPPercent";
        private const string cTotalUnitPCPercent = "TUnitPCPercent";
        private const string cTotalUnitPFPercent = "TUnitPFPercent";
        //benefits
        //totals, including initbens, salvageval, replacement, and subcosts
        public double TotalRBenefit { get; set; }
        public double TotalRPFTotal { get; set; }
        public double TotalRPCTotal { get; set; }
        public double TotalRAPTotal { get; set; }
        public double TotalRACTotal { get; set; }
        public double TotalRAPChange { get; set; }
        public double TotalRACChange { get; set; }
        public double TotalRPPPercent { get; set; }
        public double TotalRPCPercent { get; set; }
        public double TotalRPFPercent { get; set; }
        //total lcb benefit
        public double TotalLCBBenefit { get; set; }
        public double TotalLCBPFTotal { get; set; }
        public double TotalLCBPCTotal { get; set; }
        public double TotalLCBAPTotal { get; set; }
        public double TotalLCBACTotal { get; set; }
        public double TotalLCBAPChange { get; set; }
        public double TotalLCBACChange { get; set; }
        public double TotalLCBPPPercent { get; set; }
        public double TotalLCBPCPercent { get; set; }
        public double TotalLCBPFPercent { get; set; }
        //total eaa benefit (equiv ann annuity)
        public double TotalREAABenefit { get; set; }
        public double TotalREAAPFTotal { get; set; }
        public double TotalREAAPCTotal { get; set; }
        public double TotalREAAAPTotal { get; set; }
        public double TotalREAAACTotal { get; set; }
        public double TotalREAAAPChange { get; set; }
        public double TotalREAAACChange { get; set; }
        public double TotalREAAPPPercent { get; set; }
        public double TotalREAAPCPercent { get; set; }
        public double TotalREAAPFPercent { get; set; }
        //total per unit benefits
        public double TotalRUnitBenefit { get; set; }
        public double TotalRUnitPFTotal { get; set; }
        public double TotalRUnitPCTotal { get; set; }
        public double TotalRUnitAPTotal { get; set; }
        public double TotalRUnitACTotal { get; set; }
        public double TotalRUnitAPChange { get; set; }
        public double TotalRUnitACChange { get; set; }
        public double TotalRUnitPPPercent { get; set; }
        public double TotalRUnitPCPercent { get; set; }
        public double TotalRUnitPFPercent { get; set; }

        //options and salvage value taken from other capital inputs
        private const string cTotalRBenefit = "TRBenefit";
        private const string cTotalRPFTotal = "TRPFTotal";
        private const string cTotalRPCTotal = "TRPCTotal";
        private const string cTotalRAPTotal = "TRAPTotal";
        private const string cTotalRACTotal = "TRACTotal";
        private const string cTotalRAPChange = "TRAPChange";
        private const string cTotalRACChange = "TRACChange";
        private const string cTotalRPPPercent = "TRPPPercent";
        private const string cTotalRPCPercent = "TRPCPercent";
        private const string cTotalRPFPercent = "TRPFPercent";

        private const string cTotalLCBBenefit = "TLCBBenefit";
        private const string cTotalLCBPFTotal = "TLCBPFTotal";
        private const string cTotalLCBPCTotal = "TLCBPCTotal";
        private const string cTotalLCBAPTotal = "TLCBAPTotal";
        private const string cTotalLCBACTotal = "TLCBACTotal";
        private const string cTotalLCBAPChange = "TLCBAPChange";
        private const string cTotalLCBACChange = "TLCBACChange";
        private const string cTotalLCBPPPercent = "TLCBPPPercent";
        private const string cTotalLCBPCPercent = "TLCBPCPercent";
        private const string cTotalLCBPFPercent = "TLCBPFPercent";

        private const string cTotalREAABenefit = "TREAABenefit";
        private const string cTotalREAAPFTotal = "TREAAPFTotal";
        private const string cTotalREAAPCTotal = "TREAAPCTotal";
        private const string cTotalREAAAPTotal = "TREAAAPTotal";
        private const string cTotalREAAACTotal = "TREAAACTotal";
        private const string cTotalREAAAPChange = "TREAAAPChange";
        private const string cTotalREAAACChange = "TREAAACChange";
        private const string cTotalREAAPPPercent = "TREAAPPPercent";
        private const string cTotalREAAPCPercent = "TREAAPCPercent";
        private const string cTotalREAAPFPercent = "TREAAPFPercent";

        private const string cTotalRUnitBenefit = "TRUnitBenefit";
        private const string cTotalRUnitPFTotal = "TRUnitPFTotal";
        private const string cTotalRUnitPCTotal = "TRUnitPCTotal";
        private const string cTotalRUnitAPTotal = "TRUnitAPTotal";
        private const string cTotalRUnitACTotal = "TRUnitACTotal";
        private const string cTotalRUnitAPChange = "TRUnitAPChange";
        private const string cTotalRUnitACChange = "TRUnitACChange";
        private const string cTotalRUnitPPPercent = "TRUnitPPPercent";
        private const string cTotalRUnitPCPercent = "TRUnitPCPercent";
        private const string cTotalRUnitPFPercent = "TRUnitPFPercent";

        public void InitTotalLCA1Progress1Properties(LCA1Progress1 ind)
        {
            ind.ErrorMessage = string.Empty;

            ind.TotalOCCost = 0;
            ind.TotalOCPFTotal = 0;
            ind.TotalOCPCTotal = 0;
            ind.TotalOCAPTotal = 0;
            ind.TotalOCACTotal = 0;
            ind.TotalOCAPChange = 0;
            ind.TotalOCACChange = 0;
            ind.TotalOCPPPercent = 0;
            ind.TotalOCPCPercent = 0;
            ind.TotalOCPFPercent = 0;

            ind.TotalAOHCost = 0;
            ind.TotalAOHPFTotal = 0;
            ind.TotalAOHPCTotal = 0;
            ind.TotalAOHAPTotal = 0;
            ind.TotalAOHACTotal = 0;
            ind.TotalAOHAPChange = 0;
            ind.TotalAOHACChange = 0;
            ind.TotalAOHPPPercent = 0;
            ind.TotalAOHPCPercent = 0;
            ind.TotalAOHPFPercent = 0;

            ind.TotalCAPCost = 0;
            ind.TotalCAPPFTotal = 0;
            ind.TotalCAPPCTotal = 0;
            ind.TotalCAPAPTotal = 0;
            ind.TotalCAPACTotal = 0;
            ind.TotalCAPAPChange = 0;
            ind.TotalCAPACChange = 0;
            ind.TotalCAPPPPercent = 0;
            ind.TotalCAPPCPercent = 0;
            ind.TotalCAPPFPercent = 0;

            ind.TotalLCCCost = 0;
            ind.TotalLCCPFTotal = 0;
            ind.TotalLCCPCTotal = 0;
            ind.TotalLCCAPTotal = 0;
            ind.TotalLCCACTotal = 0;
            ind.TotalLCCAPChange = 0;
            ind.TotalLCCACChange = 0;
            ind.TotalLCCPPPercent = 0;
            ind.TotalLCCPCPercent = 0;
            ind.TotalLCCPFPercent = 0;

            ind.TotalEAACost = 0;
            ind.TotalEAAPFTotal = 0;
            ind.TotalEAAPCTotal = 0;
            ind.TotalEAAAPTotal = 0;
            ind.TotalEAAACTotal = 0;
            ind.TotalEAAAPChange = 0;
            ind.TotalEAAACChange = 0;
            ind.TotalEAAPPPercent = 0;
            ind.TotalEAAPCPercent = 0;
            ind.TotalEAAPFPercent = 0;

            ind.TotalUnitCost = 0;
            ind.TotalUnitPFTotal = 0;
            ind.TotalUnitPCTotal = 0;
            ind.TotalUnitAPTotal = 0;
            ind.TotalUnitACTotal = 0;
            ind.TotalUnitAPChange = 0;
            ind.TotalUnitACChange = 0;
            ind.TotalUnitPPPercent = 0;
            ind.TotalUnitPCPercent = 0;
            ind.TotalUnitPFPercent = 0;

            ind.TotalRBenefit = 0;
            ind.TotalRPFTotal = 0;
            ind.TotalRPCTotal = 0;
            ind.TotalRAPTotal = 0;
            ind.TotalRACTotal = 0;
            ind.TotalRAPChange = 0;
            ind.TotalRACChange = 0;
            ind.TotalRPPPercent = 0;
            ind.TotalRPCPercent = 0;
            ind.TotalRPFPercent = 0;

            ind.TotalLCBBenefit = 0;
            ind.TotalLCBPFTotal = 0;
            ind.TotalLCBPCTotal = 0;
            ind.TotalLCBAPTotal = 0;
            ind.TotalLCBACTotal = 0;
            ind.TotalLCBAPChange = 0;
            ind.TotalLCBACChange = 0;
            ind.TotalLCBPPPercent = 0;
            ind.TotalLCBPCPercent = 0;
            ind.TotalLCBPFPercent = 0;

            ind.TotalREAABenefit = 0;
            ind.TotalREAAPFTotal = 0;
            ind.TotalREAAPCTotal = 0;
            ind.TotalREAAAPTotal = 0;
            ind.TotalREAAACTotal = 0;
            ind.TotalREAAAPChange = 0;
            ind.TotalREAAACChange = 0;
            ind.TotalREAAPPPercent = 0;
            ind.TotalREAAPCPercent = 0;
            ind.TotalREAAPFPercent = 0;

            ind.TotalRUnitBenefit = 0;
            ind.TotalRUnitPFTotal = 0;
            ind.TotalRUnitPCTotal = 0;
            ind.TotalRUnitAPTotal = 0;
            ind.TotalRUnitACTotal = 0;
            ind.TotalRUnitAPChange = 0;
            ind.TotalRUnitACChange = 0;
            ind.TotalRUnitPPPercent = 0;
            ind.TotalRUnitPCPercent = 0;
            ind.TotalRUnitPFPercent = 0;
            ind.CalcParameters = new CalculatorParameters();
            ind.SubP1Stock = new SubPrice1Stock();
            ind.SubP2Stock = new SubPrice2Stock();
        }

        public void CopyTotalLCA1Progress1Properties(LCA1Progress1 ind,
            LCA1Progress1 calculator)
        {
            ind.ErrorMessage = calculator.ErrorMessage;
            ind.TotalOCCost = calculator.TotalOCCost;
            ind.TotalOCPFTotal = calculator.TotalOCPFTotal;
            ind.TotalOCPCTotal = calculator.TotalOCPCTotal;
            ind.TotalOCAPTotal = calculator.TotalOCAPTotal;
            ind.TotalOCACTotal = calculator.TotalOCACTotal;
            ind.TotalOCAPChange = calculator.TotalOCAPChange;
            ind.TotalOCACChange = calculator.TotalOCACChange;
            ind.TotalOCPPPercent = calculator.TotalOCPPPercent;
            ind.TotalOCPCPercent = calculator.TotalOCPCPercent;
            ind.TotalOCPFPercent = calculator.TotalOCPFPercent;

            ind.TotalAOHCost = calculator.TotalAOHCost;
            ind.TotalAOHPFTotal = calculator.TotalAOHPFTotal;
            ind.TotalAOHPCTotal = calculator.TotalAOHPCTotal;
            ind.TotalAOHAPTotal = calculator.TotalAOHAPTotal;
            ind.TotalAOHACTotal = calculator.TotalAOHACTotal;
            ind.TotalAOHAPChange = calculator.TotalAOHAPChange;
            ind.TotalAOHACChange = calculator.TotalAOHACChange;
            ind.TotalAOHPPPercent = calculator.TotalAOHPPPercent;
            ind.TotalAOHPCPercent = calculator.TotalAOHPCPercent;
            ind.TotalAOHPFPercent = calculator.TotalAOHPFPercent;

            ind.TotalCAPCost = calculator.TotalCAPCost;
            ind.TotalCAPPFTotal = calculator.TotalCAPPFTotal;
            ind.TotalCAPPCTotal = calculator.TotalCAPPCTotal;
            ind.TotalCAPAPTotal = calculator.TotalCAPAPTotal;
            ind.TotalCAPACTotal = calculator.TotalCAPACTotal;
            ind.TotalCAPAPChange = calculator.TotalCAPAPChange;
            ind.TotalCAPACChange = calculator.TotalCAPACChange;
            ind.TotalCAPPPPercent = calculator.TotalCAPPPPercent;
            ind.TotalCAPPCPercent = calculator.TotalCAPPCPercent;
            ind.TotalCAPPFPercent = calculator.TotalCAPPFPercent;

            ind.TotalLCCCost = calculator.TotalLCCCost;
            ind.TotalLCCPFTotal = calculator.TotalLCCPFTotal;
            ind.TotalLCCPCTotal = calculator.TotalLCCPCTotal;
            ind.TotalLCCAPTotal = calculator.TotalLCCAPTotal;
            ind.TotalLCCACTotal = calculator.TotalLCCACTotal;
            ind.TotalLCCAPChange = calculator.TotalLCCAPChange;
            ind.TotalLCCACChange = calculator.TotalLCCACChange;
            ind.TotalLCCPPPercent = calculator.TotalLCCPPPercent;
            ind.TotalLCCPCPercent = calculator.TotalLCCPCPercent;
            ind.TotalLCCPFPercent = calculator.TotalLCCPFPercent;

            ind.TotalEAACost = calculator.TotalEAACost;
            ind.TotalEAAPFTotal = calculator.TotalEAAPFTotal;
            ind.TotalEAAPCTotal = calculator.TotalEAAPCTotal;
            ind.TotalEAAAPTotal = calculator.TotalEAAAPTotal;
            ind.TotalEAAACTotal = calculator.TotalEAAACTotal;
            ind.TotalEAAAPChange = calculator.TotalEAAAPChange;
            ind.TotalEAAACChange = calculator.TotalEAAACChange;
            ind.TotalEAAPPPercent = calculator.TotalEAAPPPercent;
            ind.TotalEAAPCPercent = calculator.TotalEAAPCPercent;
            ind.TotalEAAPFPercent = calculator.TotalEAAPFPercent;

            ind.TotalUnitCost = calculator.TotalUnitCost;
            ind.TotalUnitPFTotal = calculator.TotalUnitPFTotal;
            ind.TotalUnitPCTotal = calculator.TotalUnitPCTotal;
            ind.TotalUnitAPTotal = calculator.TotalUnitAPTotal;
            ind.TotalUnitACTotal = calculator.TotalUnitACTotal;
            ind.TotalUnitAPChange = calculator.TotalUnitAPChange;
            ind.TotalUnitACChange = calculator.TotalUnitACChange;
            ind.TotalUnitPPPercent = calculator.TotalUnitPPPercent;
            ind.TotalUnitPCPercent = calculator.TotalUnitPCPercent;
            ind.TotalUnitPFPercent = calculator.TotalUnitPFPercent;

            ind.TotalRBenefit = calculator.TotalRBenefit;
            ind.TotalRPFTotal = calculator.TotalRPFTotal;
            ind.TotalRPCTotal = calculator.TotalRPCTotal;
            ind.TotalRAPTotal = calculator.TotalRAPTotal;
            ind.TotalRACTotal = calculator.TotalRACTotal;
            ind.TotalRAPChange = calculator.TotalRAPChange;
            ind.TotalRACChange = calculator.TotalRACChange;
            ind.TotalRPPPercent = calculator.TotalRPPPercent;
            ind.TotalRPCPercent = calculator.TotalRPCPercent;
            ind.TotalRPFPercent = calculator.TotalRPFPercent;

            ind.TotalLCBBenefit = calculator.TotalLCBBenefit;
            ind.TotalLCBPFTotal = calculator.TotalLCBPFTotal;
            ind.TotalLCBPCTotal = calculator.TotalLCBPCTotal;
            ind.TotalLCBAPTotal = calculator.TotalLCBAPTotal;
            ind.TotalLCBACTotal = calculator.TotalLCBACTotal;
            ind.TotalLCBAPChange = calculator.TotalLCBAPChange;
            ind.TotalLCBACChange = calculator.TotalLCBACChange;
            ind.TotalLCBPPPercent = calculator.TotalLCBPPPercent;
            ind.TotalLCBPCPercent = calculator.TotalLCBPCPercent;
            ind.TotalLCBPFPercent = calculator.TotalLCBPFPercent;

            ind.TotalREAABenefit = calculator.TotalREAABenefit;
            ind.TotalREAAPFTotal = calculator.TotalREAAPFTotal;
            ind.TotalREAAPCTotal = calculator.TotalREAAPCTotal;
            ind.TotalREAAAPTotal = calculator.TotalREAAAPTotal;
            ind.TotalREAAACTotal = calculator.TotalREAAACTotal;
            ind.TotalREAAAPChange = calculator.TotalREAAAPChange;
            ind.TotalREAAACChange = calculator.TotalREAAACChange;
            ind.TotalREAAPPPercent = calculator.TotalREAAPPPercent;
            ind.TotalREAAPCPercent = calculator.TotalREAAPCPercent;
            ind.TotalREAAPFPercent = calculator.TotalREAAPFPercent;

            ind.TotalRUnitBenefit = calculator.TotalRUnitBenefit;
            ind.TotalRUnitPFTotal = calculator.TotalRUnitPFTotal;
            ind.TotalRUnitPCTotal = calculator.TotalRUnitPCTotal;
            ind.TotalRUnitAPTotal = calculator.TotalRUnitAPTotal;
            ind.TotalRUnitACTotal = calculator.TotalRUnitACTotal;
            ind.TotalRUnitAPChange = calculator.TotalRUnitAPChange;
            ind.TotalRUnitACChange = calculator.TotalRUnitACChange;
            ind.TotalRUnitPPPercent = calculator.TotalRUnitPPPercent;
            ind.TotalRUnitPCPercent = calculator.TotalRUnitPCPercent;
            ind.TotalRUnitPFPercent = calculator.TotalRUnitPFPercent;
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

        public void SetTotalLCA1Progress1Properties(LCA1Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.TotalOCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCCost, attNameExtension));
            ind.TotalOCPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCPFTotal, attNameExtension));
            ind.TotalOCPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCPCTotal, attNameExtension));
            ind.TotalOCAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCAPTotal, attNameExtension));
            ind.TotalOCACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCACTotal, attNameExtension));
            ind.TotalOCAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCAPChange, attNameExtension));
            ind.TotalOCACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCACChange, attNameExtension));
            ind.TotalOCPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCPPPercent, attNameExtension));
            ind.TotalOCPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCPCPercent, attNameExtension));
            ind.TotalOCPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalOCPFPercent, attNameExtension));

            ind.TotalAOHCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHCost, attNameExtension));
            ind.TotalAOHPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHPFTotal, attNameExtension));
            ind.TotalAOHPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHPCTotal, attNameExtension));
            ind.TotalAOHAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHAPTotal, attNameExtension));
            ind.TotalAOHACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHACTotal, attNameExtension));
            ind.TotalAOHAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHAPChange, attNameExtension));
            ind.TotalAOHACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHACChange, attNameExtension));
            ind.TotalAOHPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHPPPercent, attNameExtension));
            ind.TotalAOHPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHPCPercent, attNameExtension));
            ind.TotalAOHPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAOHPFPercent, attNameExtension));

            ind.TotalCAPCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPCost, attNameExtension));
            ind.TotalCAPPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPPFTotal, attNameExtension));
            ind.TotalCAPPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPPCTotal, attNameExtension));
            ind.TotalCAPAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPAPTotal, attNameExtension));
            ind.TotalCAPACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPACTotal, attNameExtension));
            ind.TotalCAPAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPAPChange, attNameExtension));
            ind.TotalCAPACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPACChange, attNameExtension));
            ind.TotalCAPPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPPPPercent, attNameExtension));
            ind.TotalCAPPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPPCPercent, attNameExtension));
            ind.TotalCAPPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalCAPPFPercent, attNameExtension));

            ind.TotalLCCCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCCost, attNameExtension));
            ind.TotalLCCPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCPFTotal, attNameExtension));
            ind.TotalLCCPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCPCTotal, attNameExtension));
            ind.TotalLCCAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCAPTotal, attNameExtension));
            ind.TotalLCCACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCACTotal, attNameExtension));
            ind.TotalLCCAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCAPChange, attNameExtension));
            ind.TotalLCCACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCACChange, attNameExtension));
            ind.TotalLCCPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCPPPercent, attNameExtension));
            ind.TotalLCCPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCPCPercent, attNameExtension));
            ind.TotalLCCPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCCPFPercent, attNameExtension));

            ind.TotalEAACost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAACost, attNameExtension));
            ind.TotalEAAPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAPFTotal, attNameExtension));
            ind.TotalEAAPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAPCTotal, attNameExtension));
            ind.TotalEAAAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAAPTotal, attNameExtension));
            ind.TotalEAAACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAACTotal, attNameExtension));
            ind.TotalEAAAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAAPChange, attNameExtension));
            ind.TotalEAAACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAACChange, attNameExtension));
            ind.TotalEAAPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAPPPercent, attNameExtension));
            ind.TotalEAAPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAPCPercent, attNameExtension));
            ind.TotalEAAPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalEAAPFPercent, attNameExtension));

            ind.TotalUnitCost = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitCost, attNameExtension));
            ind.TotalUnitPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitPFTotal, attNameExtension));
            ind.TotalUnitPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitPCTotal, attNameExtension));
            ind.TotalUnitAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitAPTotal, attNameExtension));
            ind.TotalUnitACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitACTotal, attNameExtension));
            ind.TotalUnitAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitAPChange, attNameExtension));
            ind.TotalUnitACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitACChange, attNameExtension));
            ind.TotalUnitPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitPPPercent, attNameExtension));
            ind.TotalUnitPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitPCPercent, attNameExtension));
            ind.TotalUnitPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalUnitPFPercent, attNameExtension));

            ind.TotalRBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRBenefit, attNameExtension));
            ind.TotalRPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPFTotal, attNameExtension));
            ind.TotalRPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPCTotal, attNameExtension));
            ind.TotalRAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAPTotal, attNameExtension));
            ind.TotalRACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRACTotal, attNameExtension));
            ind.TotalRAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAPChange, attNameExtension));
            ind.TotalRACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRACChange, attNameExtension));
            ind.TotalRPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPPPercent, attNameExtension));
            ind.TotalRPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPCPercent, attNameExtension));
            ind.TotalRPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPFPercent, attNameExtension));

            ind.TotalLCBBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBBenefit, attNameExtension));
            ind.TotalLCBPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBPFTotal, attNameExtension));
            ind.TotalLCBPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBPCTotal, attNameExtension));
            ind.TotalLCBAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBAPTotal, attNameExtension));
            ind.TotalLCBACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBACTotal, attNameExtension));
            ind.TotalLCBAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBAPChange, attNameExtension));
            ind.TotalLCBACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBACChange, attNameExtension));
            ind.TotalLCBPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBPPPercent, attNameExtension));
            ind.TotalLCBPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBPCPercent, attNameExtension));
            ind.TotalLCBPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalLCBPFPercent, attNameExtension));

            ind.TotalREAABenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAABenefit, attNameExtension));
            ind.TotalREAAPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAPFTotal, attNameExtension));
            ind.TotalREAAPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAPCTotal, attNameExtension));
            ind.TotalREAAAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAAPTotal, attNameExtension));
            ind.TotalREAAACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAACTotal, attNameExtension));
            ind.TotalREAAAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAAPChange, attNameExtension));
            ind.TotalREAAACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAACChange, attNameExtension));
            ind.TotalREAAPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAPPPercent, attNameExtension));
            ind.TotalREAAPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAPCPercent, attNameExtension));
            ind.TotalREAAPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalREAAPFPercent, attNameExtension));

            ind.TotalRUnitBenefit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitBenefit, attNameExtension));
            ind.TotalRUnitPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitPFTotal, attNameExtension));
            ind.TotalRUnitPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitPCTotal, attNameExtension));
            ind.TotalRUnitAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitAPTotal, attNameExtension));
            ind.TotalRUnitACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitACTotal, attNameExtension));
            ind.TotalRUnitAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitAPChange, attNameExtension));
            ind.TotalRUnitACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitACChange, attNameExtension));
            ind.TotalRUnitPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitPPPercent, attNameExtension));
            ind.TotalRUnitPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitPCPercent, attNameExtension));
            ind.TotalRUnitPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRUnitPFPercent, attNameExtension));
        }

        public void SetTotalLCA1Progress1Property(LCA1Progress1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalOCCost:
                    ind.TotalOCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCPFTotal:
                    ind.TotalOCPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCPCTotal:
                    ind.TotalOCPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCAPTotal:
                    ind.TotalOCAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCACTotal:
                    ind.TotalOCACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCAPChange:
                    ind.TotalOCAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCACChange:
                    ind.TotalOCACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCPPPercent:
                    ind.TotalOCPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCPCPercent:
                    ind.TotalOCPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalOCPFPercent:
                    ind.TotalOCPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHCost:
                    ind.TotalAOHCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHPFTotal:
                    ind.TotalAOHPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHPCTotal:
                    ind.TotalAOHPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHAPTotal:
                    ind.TotalAOHAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHACTotal:
                    ind.TotalAOHACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHAPChange:
                    ind.TotalAOHAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHACChange:
                    ind.TotalAOHACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHPPPercent:
                    ind.TotalAOHPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHPCPercent:
                    ind.TotalAOHPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAOHPFPercent:
                    ind.TotalAOHPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPCost:
                    ind.TotalCAPCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPPFTotal:
                    ind.TotalCAPPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPPCTotal:
                    ind.TotalCAPPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPAPTotal:
                    ind.TotalCAPAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPACTotal:
                    ind.TotalCAPACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPAPChange:
                    ind.TotalCAPAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPACChange:
                    ind.TotalCAPACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPPPPercent:
                    ind.TotalCAPPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPPCPercent:
                    ind.TotalCAPPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalCAPPFPercent:
                    ind.TotalCAPPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCCost:
                    ind.TotalLCCCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCPFTotal:
                    ind.TotalLCCPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCPCTotal:
                    ind.TotalLCCPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCAPTotal:
                    ind.TotalLCCAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCACTotal:
                    ind.TotalLCCACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCAPChange:
                    ind.TotalLCCAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCACChange:
                    ind.TotalLCCACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCPPPercent:
                    ind.TotalLCCPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCPCPercent:
                    ind.TotalLCCPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCCPFPercent:
                    ind.TotalLCCPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAACost:
                    ind.TotalEAACost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAPFTotal:
                    ind.TotalEAAPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAPCTotal:
                    ind.TotalEAAPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAAPTotal:
                    ind.TotalEAAAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAACTotal:
                    ind.TotalEAAACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAAPChange:
                    ind.TotalEAAAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalEAAACChange:
                    sPropertyValue = ind.TotalEAAACChange.ToString();
                    break;
                case cTotalEAAPPPercent:
                    sPropertyValue = ind.TotalEAAPPPercent.ToString();
                    break;
                case cTotalEAAPCPercent:
                    sPropertyValue = ind.TotalEAAPCPercent.ToString();
                    break;
                case cTotalEAAPFPercent:
                    sPropertyValue = ind.TotalEAAPFPercent.ToString();
                    break;
                case cTotalUnitCost:
                    ind.TotalUnitCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitPFTotal:
                    ind.TotalUnitPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitPCTotal:
                    ind.TotalUnitPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitAPTotal:
                    ind.TotalUnitAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitACTotal:
                    ind.TotalUnitACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitAPChange:
                    ind.TotalUnitAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitACChange:
                    ind.TotalUnitACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitPPPercent:
                    ind.TotalUnitPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitPCPercent:
                    ind.TotalUnitPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalUnitPFPercent:
                    ind.TotalUnitPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRBenefit:
                    ind.TotalRBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPFTotal:
                    ind.TotalRPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPCTotal:
                    ind.TotalRPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAPTotal:
                    ind.TotalRAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRACTotal:
                    ind.TotalRACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAPChange:
                    ind.TotalRAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRACChange:
                    ind.TotalRACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPPPercent:
                    ind.TotalRPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPCPercent:
                    ind.TotalRPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPFPercent:
                    ind.TotalRPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBBenefit:
                    ind.TotalLCBBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBPFTotal:
                    ind.TotalLCBPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBPCTotal:
                    ind.TotalLCBPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBAPTotal:
                    ind.TotalLCBAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBACTotal:
                    ind.TotalLCBACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBAPChange:
                    ind.TotalLCBAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBACChange:
                    ind.TotalLCBACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBPPPercent:
                    ind.TotalLCBPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBPCPercent:
                    ind.TotalLCBPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalLCBPFPercent:
                    ind.TotalLCBPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAABenefit:
                    ind.TotalREAABenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAPFTotal:
                    ind.TotalREAAPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAPCTotal:
                    ind.TotalREAAPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAAPTotal:
                    ind.TotalREAAAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAACTotal:
                    ind.TotalREAAACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAAPChange:
                    ind.TotalREAAAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAACChange:
                    ind.TotalREAAACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAPPPercent:
                    ind.TotalREAAPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAPCPercent:
                    ind.TotalREAAPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalREAAPFPercent:
                    ind.TotalREAAPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitBenefit:
                    ind.TotalRUnitBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitPFTotal:
                    ind.TotalRUnitPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitPCTotal:
                    ind.TotalRUnitPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitAPTotal:
                    ind.TotalRUnitAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitACTotal:
                    ind.TotalRUnitACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitAPChange:
                    ind.TotalRUnitAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitACChange:
                    ind.TotalRUnitACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitPPPercent:
                    ind.TotalRUnitPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitPCPercent:
                    ind.TotalRUnitPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRUnitPFPercent:
                    ind.TotalRUnitPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalLCA1Progress1Property(LCA1Progress1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case cTotalOCCost:
                    sPropertyValue = ind.TotalOCCost.ToString();
                    break;
                case cTotalOCPFTotal:
                    sPropertyValue = ind.TotalOCPFTotal.ToString();
                    break;
                case cTotalOCPCTotal:
                    sPropertyValue = ind.TotalOCPCTotal.ToString();
                    break;
                case cTotalOCAPTotal:
                    sPropertyValue = ind.TotalOCAPTotal.ToString();
                    break;
                case cTotalOCACTotal:
                    sPropertyValue = ind.TotalOCACTotal.ToString();
                    break;
                case cTotalOCAPChange:
                    sPropertyValue = ind.TotalOCAPChange.ToString();
                    break;
                case cTotalOCACChange:
                    sPropertyValue = ind.TotalOCACChange.ToString();
                    break;
                case cTotalOCPPPercent:
                    sPropertyValue = ind.TotalOCPPPercent.ToString();
                    break;
                case cTotalOCPCPercent:
                    sPropertyValue = ind.TotalOCPCPercent.ToString();
                    break;
                case cTotalOCPFPercent:
                    sPropertyValue = ind.TotalOCPFPercent.ToString();
                    break;
                case cTotalAOHCost:
                    sPropertyValue = ind.TotalAOHCost.ToString();
                    break;
                case cTotalAOHPFTotal:
                    sPropertyValue = ind.TotalAOHPFTotal.ToString();
                    break;
                case cTotalAOHPCTotal:
                    sPropertyValue = ind.TotalAOHPCTotal.ToString();
                    break;
                case cTotalAOHAPTotal:
                    sPropertyValue = ind.TotalAOHAPTotal.ToString();
                    break;
                case cTotalAOHACTotal:
                    sPropertyValue = ind.TotalAOHACTotal.ToString();
                    break;
                case cTotalAOHAPChange:
                    sPropertyValue = ind.TotalAOHAPChange.ToString();
                    break;
                case cTotalAOHACChange:
                    sPropertyValue = ind.TotalAOHACChange.ToString();
                    break;
                case cTotalAOHPPPercent:
                    sPropertyValue = ind.TotalAOHPPPercent.ToString();
                    break;
                case cTotalAOHPCPercent:
                    sPropertyValue = ind.TotalAOHPCPercent.ToString();
                    break;
                case cTotalAOHPFPercent:
                    sPropertyValue = ind.TotalAOHPFPercent.ToString();
                    break;
                case cTotalCAPCost:
                    sPropertyValue = ind.TotalCAPCost.ToString();
                    break;
                case cTotalCAPPFTotal:
                    sPropertyValue = ind.TotalCAPPFTotal.ToString();
                    break;
                case cTotalCAPPCTotal:
                    sPropertyValue = ind.TotalCAPPCTotal.ToString();
                    break;
                case cTotalCAPAPTotal:
                    sPropertyValue = ind.TotalCAPAPTotal.ToString();
                    break;
                case cTotalCAPACTotal:
                    sPropertyValue = ind.TotalCAPACTotal.ToString();
                    break;
                case cTotalCAPAPChange:
                    sPropertyValue = ind.TotalCAPAPChange.ToString();
                    break;
                case cTotalCAPACChange:
                    sPropertyValue = ind.TotalCAPACChange.ToString();
                    break;
                case cTotalCAPPPPercent:
                    sPropertyValue = ind.TotalCAPPPPercent.ToString();
                    break;
                case cTotalCAPPCPercent:
                    sPropertyValue = ind.TotalCAPPCPercent.ToString();
                    break;
                case cTotalCAPPFPercent:
                    sPropertyValue = ind.TotalCAPPFPercent.ToString();
                    break;
                case cTotalLCCCost:
                    sPropertyValue = ind.TotalLCCCost.ToString();
                    break;
                case cTotalLCCPFTotal:
                    sPropertyValue = ind.TotalLCCPFTotal.ToString();
                    break;
                case cTotalLCCPCTotal:
                    sPropertyValue = ind.TotalLCCPCTotal.ToString();
                    break;
                case cTotalLCCAPTotal:
                    sPropertyValue = ind.TotalLCCAPTotal.ToString();
                    break;
                case cTotalLCCACTotal:
                    sPropertyValue = ind.TotalLCCACTotal.ToString();
                    break;
                case cTotalLCCAPChange:
                    sPropertyValue = ind.TotalLCCAPChange.ToString();
                    break;
                case cTotalLCCACChange:
                    sPropertyValue = ind.TotalLCCACChange.ToString();
                    break;
                case cTotalLCCPPPercent:
                    sPropertyValue = ind.TotalLCCPPPercent.ToString();
                    break;
                case cTotalLCCPCPercent:
                    sPropertyValue = ind.TotalLCCPCPercent.ToString();
                    break;
                case cTotalLCCPFPercent:
                    sPropertyValue = ind.TotalLCCPFPercent.ToString();
                    break;
                case cTotalEAACost:
                    sPropertyValue = ind.TotalEAACost.ToString();
                    break;
                case cTotalEAAPFTotal:
                    sPropertyValue = ind.TotalEAAPFTotal.ToString();
                    break;
                case cTotalEAAPCTotal:
                    sPropertyValue = ind.TotalEAAPCTotal.ToString();
                    break;
                case cTotalEAAAPTotal:
                    sPropertyValue = ind.TotalEAAAPTotal.ToString();
                    break;
                case cTotalEAAACTotal:
                    sPropertyValue = ind.TotalEAAACTotal.ToString();
                    break;
                case cTotalEAAAPChange:
                    sPropertyValue = ind.TotalEAAAPChange.ToString();
                    break;
                case cTotalEAAACChange:
                    sPropertyValue = ind.TotalEAAACChange.ToString();
                    break;
                case cTotalEAAPPPercent:
                    sPropertyValue = ind.TotalEAAPPPercent.ToString();
                    break;
                case cTotalEAAPCPercent:
                    sPropertyValue = ind.TotalEAAPCPercent.ToString();
                    break;
                case cTotalEAAPFPercent:
                    sPropertyValue = ind.TotalEAAPFPercent.ToString();
                    break;
                case cTotalUnitCost:
                    sPropertyValue = ind.TotalUnitCost.ToString();
                    break;
                case cTotalUnitPFTotal:
                    sPropertyValue = ind.TotalUnitPFTotal.ToString();
                    break;
                case cTotalUnitPCTotal:
                    sPropertyValue = ind.TotalUnitPCTotal.ToString();
                    break;
                case cTotalUnitAPTotal:
                    sPropertyValue = ind.TotalUnitAPTotal.ToString();
                    break;
                case cTotalUnitACTotal:
                    sPropertyValue = ind.TotalUnitACTotal.ToString();
                    break;
                case cTotalUnitAPChange:
                    sPropertyValue = ind.TotalUnitAPChange.ToString();
                    break;
                case cTotalUnitACChange:
                    sPropertyValue = ind.TotalUnitACChange.ToString();
                    break;
                case cTotalUnitPPPercent:
                    sPropertyValue = ind.TotalUnitPPPercent.ToString();
                    break;
                case cTotalUnitPCPercent:
                    sPropertyValue = ind.TotalUnitPCPercent.ToString();
                    break;
                case cTotalUnitPFPercent:
                    sPropertyValue = ind.TotalUnitPFPercent.ToString();
                    break;
                case cTotalRBenefit:
                    sPropertyValue = ind.TotalRBenefit.ToString();
                    break;
                case cTotalRPFTotal:
                    sPropertyValue = ind.TotalRPFTotal.ToString();
                    break;
                case cTotalRPCTotal:
                    sPropertyValue = ind.TotalRPCTotal.ToString();
                    break;
                case cTotalRAPTotal:
                    sPropertyValue = ind.TotalRAPTotal.ToString();
                    break;
                case cTotalRACTotal:
                    sPropertyValue = ind.TotalRACTotal.ToString();
                    break;
                case cTotalRAPChange:
                    sPropertyValue = ind.TotalRAPChange.ToString();
                    break;
                case cTotalRACChange:
                    sPropertyValue = ind.TotalRACChange.ToString();
                    break;
                case cTotalRPPPercent:
                    sPropertyValue = ind.TotalRPPPercent.ToString();
                    break;
                case cTotalRPCPercent:
                    sPropertyValue = ind.TotalRPCPercent.ToString();
                    break;
                case cTotalRPFPercent:
                    sPropertyValue = ind.TotalRPFPercent.ToString();
                    break;
                case cTotalLCBBenefit:
                    sPropertyValue = ind.TotalLCBBenefit.ToString();
                    break;
                case cTotalLCBPFTotal:
                    sPropertyValue = ind.TotalLCBPFTotal.ToString();
                    break;
                case cTotalLCBPCTotal:
                    sPropertyValue = ind.TotalLCBPCTotal.ToString();
                    break;
                case cTotalLCBAPTotal:
                    sPropertyValue = ind.TotalLCBAPTotal.ToString();
                    break;
                case cTotalLCBACTotal:
                    sPropertyValue = ind.TotalLCBACTotal.ToString();
                    break;
                case cTotalLCBAPChange:
                    sPropertyValue = ind.TotalLCBAPChange.ToString();
                    break;
                case cTotalLCBACChange:
                    sPropertyValue = ind.TotalLCBACChange.ToString();
                    break;
                case cTotalLCBPPPercent:
                    sPropertyValue = ind.TotalLCBPPPercent.ToString();
                    break;
                case cTotalLCBPCPercent:
                    sPropertyValue = ind.TotalLCBPCPercent.ToString();
                    break;
                case cTotalLCBPFPercent:
                    sPropertyValue = ind.TotalLCBPFPercent.ToString();
                    break;
                case cTotalREAABenefit:
                    sPropertyValue = ind.TotalREAABenefit.ToString();
                    break;
                case cTotalREAAPFTotal:
                    sPropertyValue = ind.TotalREAAPFTotal.ToString();
                    break;
                case cTotalREAAPCTotal:
                    sPropertyValue = ind.TotalREAAPCTotal.ToString();
                    break;
                case cTotalREAAAPTotal:
                    sPropertyValue = ind.TotalREAAAPTotal.ToString();
                    break;
                case cTotalREAAACTotal:
                    sPropertyValue = ind.TotalREAAACTotal.ToString();
                    break;
                case cTotalREAAAPChange:
                    sPropertyValue = ind.TotalREAAAPChange.ToString();
                    break;
                case cTotalREAAACChange:
                    sPropertyValue = ind.TotalREAAACChange.ToString();
                    break;
                case cTotalREAAPPPercent:
                    sPropertyValue = ind.TotalREAAPPPercent.ToString();
                    break;
                case cTotalREAAPCPercent:
                    sPropertyValue = ind.TotalREAAPCPercent.ToString();
                    break;
                case cTotalREAAPFPercent:
                    sPropertyValue = ind.TotalREAAPFPercent.ToString();
                    break;
                case cTotalRUnitBenefit:
                    sPropertyValue = ind.TotalRUnitBenefit.ToString();
                    break;
                case cTotalRUnitPFTotal:
                    sPropertyValue = ind.TotalRUnitPFTotal.ToString();
                    break;
                case cTotalRUnitPCTotal:
                    sPropertyValue = ind.TotalRUnitPCTotal.ToString();
                    break;
                case cTotalRUnitAPTotal:
                    sPropertyValue = ind.TotalRUnitAPTotal.ToString();
                    break;
                case cTotalRUnitACTotal:
                    sPropertyValue = ind.TotalRUnitACTotal.ToString();
                    break;
                case cTotalRUnitAPChange:
                    sPropertyValue = ind.TotalRUnitAPChange.ToString();
                    break;
                case cTotalRUnitACChange:
                    sPropertyValue = ind.TotalRUnitACChange.ToString();
                    break;
                case cTotalRUnitPPPercent:
                    sPropertyValue = ind.TotalRUnitPPPercent.ToString();
                    break;
                case cTotalRUnitPCPercent:
                    sPropertyValue = ind.TotalRUnitPCPercent.ToString();
                    break;
                case cTotalRUnitPFPercent:
                    sPropertyValue = ind.TotalRUnitPFPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalLCA1Progress1Attributes(LCA1Progress1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsTotalNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsTotalNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCPFTotal, attNameExtension), ind.TotalOCPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCPCTotal, attNameExtension), ind.TotalOCPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCAPTotal, attNameExtension), ind.TotalOCAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCACTotal, attNameExtension), ind.TotalOCACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalOCAPChange, attNameExtension), ind.TotalOCAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalOCACChange, attNameExtension), ind.TotalOCACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalOCPPPercent, attNameExtension), ind.TotalOCPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalOCPCPercent, attNameExtension), ind.TotalOCPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalOCPFPercent, attNameExtension), ind.TotalOCPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHPFTotal, attNameExtension), ind.TotalAOHPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHPCTotal, attNameExtension), ind.TotalAOHPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHAPTotal, attNameExtension), ind.TotalAOHAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHACTotal, attNameExtension), ind.TotalAOHACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAOHAPChange, attNameExtension), ind.TotalAOHAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAOHACChange, attNameExtension), ind.TotalAOHACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAOHPPPercent, attNameExtension), ind.TotalAOHPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAOHPCPercent, attNameExtension), ind.TotalAOHPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAOHPFPercent, attNameExtension), ind.TotalAOHPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPPFTotal, attNameExtension), ind.TotalCAPPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPPCTotal, attNameExtension), ind.TotalCAPPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPAPTotal, attNameExtension), ind.TotalCAPAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPACTotal, attNameExtension), ind.TotalCAPACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalCAPAPChange, attNameExtension), ind.TotalCAPAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalCAPACChange, attNameExtension), ind.TotalCAPACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalCAPPPPercent, attNameExtension), ind.TotalCAPPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalCAPPCPercent, attNameExtension), ind.TotalCAPPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalCAPPFPercent, attNameExtension), ind.TotalCAPPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCPFTotal, attNameExtension), ind.TotalLCCPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCPCTotal, attNameExtension), ind.TotalLCCPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCAPTotal, attNameExtension), ind.TotalLCCAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCACTotal, attNameExtension), ind.TotalLCCACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCCAPChange, attNameExtension), ind.TotalLCCAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCCACChange, attNameExtension), ind.TotalLCCACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCCPPPercent, attNameExtension), ind.TotalLCCPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCCPCPercent, attNameExtension), ind.TotalLCCPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCCPFPercent, attNameExtension), ind.TotalLCCPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAPFTotal, attNameExtension), ind.TotalEAAPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAPCTotal, attNameExtension), ind.TotalEAAPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAAPTotal, attNameExtension), ind.TotalEAAAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAACTotal, attNameExtension), ind.TotalEAAACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalEAAAPChange, attNameExtension), ind.TotalEAAAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalEAAACChange, attNameExtension), ind.TotalEAAACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalEAAPPPercent, attNameExtension), ind.TotalEAAPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalEAAPCPercent, attNameExtension), ind.TotalEAAPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalEAAPFPercent, attNameExtension), ind.TotalEAAPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitPFTotal, attNameExtension), ind.TotalUnitPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitPCTotal, attNameExtension), ind.TotalUnitPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitAPTotal, attNameExtension), ind.TotalUnitAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalUnitACTotal, attNameExtension), ind.TotalUnitACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalUnitAPChange, attNameExtension), ind.TotalUnitAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalUnitACChange, attNameExtension), ind.TotalUnitACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalUnitPPPercent, attNameExtension), ind.TotalUnitPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalUnitPCPercent, attNameExtension), ind.TotalUnitPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalUnitPFPercent, attNameExtension), ind.TotalUnitPFPercent);
            }
            if (bIsTotalNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPFTotal, attNameExtension), ind.TotalRPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPCTotal, attNameExtension), ind.TotalRPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAPTotal, attNameExtension), ind.TotalRAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRACTotal, attNameExtension), ind.TotalRACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRAPChange, attNameExtension), ind.TotalRAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRACChange, attNameExtension), ind.TotalRACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPPPercent, attNameExtension), ind.TotalRPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPCPercent, attNameExtension), ind.TotalRPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPFPercent, attNameExtension), ind.TotalRPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBPFTotal, attNameExtension), ind.TotalLCBPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBPCTotal, attNameExtension), ind.TotalLCBPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBAPTotal, attNameExtension), ind.TotalLCBAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBACTotal, attNameExtension), ind.TotalLCBACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalLCBAPChange, attNameExtension), ind.TotalLCBAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCBACChange, attNameExtension), ind.TotalLCBACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCBPPPercent, attNameExtension), ind.TotalLCBPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCBPCPercent, attNameExtension), ind.TotalLCBPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalLCBPFPercent, attNameExtension), ind.TotalLCBPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAPFTotal, attNameExtension), ind.TotalREAAPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAPCTotal, attNameExtension), ind.TotalREAAPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAAPTotal, attNameExtension), ind.TotalREAAAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAACTotal, attNameExtension), ind.TotalREAAACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalREAAAPChange, attNameExtension), ind.TotalREAAAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalREAAACChange, attNameExtension), ind.TotalREAAACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalREAAPPPercent, attNameExtension), ind.TotalREAAPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalREAAPCPercent, attNameExtension), ind.TotalREAAPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalREAAPFPercent, attNameExtension), ind.TotalREAAPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitPFTotal, attNameExtension), ind.TotalRUnitPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitPCTotal, attNameExtension), ind.TotalRUnitPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitAPTotal, attNameExtension), ind.TotalRUnitAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitACTotal, attNameExtension), ind.TotalRUnitACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRUnitAPChange, attNameExtension), ind.TotalRUnitAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRUnitACChange, attNameExtension), ind.TotalRUnitACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRUnitPPPercent, attNameExtension), ind.TotalRUnitPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRUnitPCPercent, attNameExtension), ind.TotalRUnitPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRUnitPFPercent, attNameExtension), ind.TotalRUnitPFPercent);
            }
        }

        public void SetTotalLCA1Progress1Attributes(LCA1Progress1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsTotalNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsTotalNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(cTotalOCCost, attNameExtension), ind.TotalOCCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCPFTotal, attNameExtension), ind.TotalOCPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCPCTotal, attNameExtension), ind.TotalOCPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCAPTotal, attNameExtension), ind.TotalOCAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCACTotal, attNameExtension), ind.TotalOCACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalOCAPChange, attNameExtension), ind.TotalOCAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalOCACChange, attNameExtension), ind.TotalOCACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalOCPPPercent, attNameExtension), ind.TotalOCPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalOCPCPercent, attNameExtension), ind.TotalOCPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalOCPFPercent, attNameExtension), ind.TotalOCPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalAOHCost, attNameExtension), ind.TotalAOHCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHPFTotal, attNameExtension), ind.TotalAOHPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHPCTotal, attNameExtension), ind.TotalAOHPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHAPTotal, attNameExtension), ind.TotalAOHAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHACTotal, attNameExtension), ind.TotalAOHACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAOHAPChange, attNameExtension), ind.TotalAOHAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAOHACChange, attNameExtension), ind.TotalAOHACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAOHPPPercent, attNameExtension), ind.TotalAOHPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAOHPCPercent, attNameExtension), ind.TotalAOHPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAOHPFPercent, attNameExtension), ind.TotalAOHPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalCAPCost, attNameExtension), ind.TotalCAPCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPPFTotal, attNameExtension), ind.TotalCAPPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPPCTotal, attNameExtension), ind.TotalCAPPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPAPTotal, attNameExtension), ind.TotalCAPAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPACTotal, attNameExtension), ind.TotalCAPACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalCAPAPChange, attNameExtension), ind.TotalCAPAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalCAPACChange, attNameExtension), ind.TotalCAPACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalCAPPPPercent, attNameExtension), ind.TotalCAPPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalCAPPCPercent, attNameExtension), ind.TotalCAPPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalCAPPFPercent, attNameExtension), ind.TotalCAPPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                   string.Concat(cTotalLCCCost, attNameExtension), ind.TotalLCCCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCPFTotal, attNameExtension), ind.TotalLCCPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCPCTotal, attNameExtension), ind.TotalLCCPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCAPTotal, attNameExtension), ind.TotalLCCAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCCACTotal, attNameExtension), ind.TotalLCCACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCCAPChange, attNameExtension), ind.TotalLCCAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCCACChange, attNameExtension), ind.TotalLCCACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCCPPPercent, attNameExtension), ind.TotalLCCPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCCPCPercent, attNameExtension), ind.TotalLCCPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCCPFPercent, attNameExtension), ind.TotalLCCPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalEAACost, attNameExtension), ind.TotalEAACost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAPFTotal, attNameExtension), ind.TotalEAAPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAPCTotal, attNameExtension), ind.TotalEAAPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAAPTotal, attNameExtension), ind.TotalEAAAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAACTotal, attNameExtension), ind.TotalEAAACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalEAAAPChange, attNameExtension), ind.TotalEAAAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalEAAACChange, attNameExtension), ind.TotalEAAACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalEAAPPPercent, attNameExtension), ind.TotalEAAPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalEAAPCPercent, attNameExtension), ind.TotalEAAPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalEAAPFPercent, attNameExtension), ind.TotalEAAPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalUnitCost, attNameExtension), ind.TotalUnitCost.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitPFTotal, attNameExtension), ind.TotalUnitPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitPCTotal, attNameExtension), ind.TotalUnitPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitAPTotal, attNameExtension), ind.TotalUnitAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitACTotal, attNameExtension), ind.TotalUnitACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalUnitAPChange, attNameExtension), ind.TotalUnitAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalUnitACChange, attNameExtension), ind.TotalUnitACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalUnitPPPercent, attNameExtension), ind.TotalUnitPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalUnitPCPercent, attNameExtension), ind.TotalUnitPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalUnitPFPercent, attNameExtension), ind.TotalUnitPFPercent.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsTotalNode || bIsBoth)
            {

                writer.WriteAttributeString(
                    string.Concat(cTotalRBenefit, attNameExtension), ind.TotalRBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPFTotal, attNameExtension), ind.TotalRPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPCTotal, attNameExtension), ind.TotalRPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAPTotal, attNameExtension), ind.TotalRAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRACTotal, attNameExtension), ind.TotalRACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAPChange, attNameExtension), ind.TotalRAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRACChange, attNameExtension), ind.TotalRACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPPPercent, attNameExtension), ind.TotalRPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPCPercent, attNameExtension), ind.TotalRPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPFPercent, attNameExtension), ind.TotalRPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalLCBBenefit, attNameExtension), ind.TotalLCBBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBPFTotal, attNameExtension), ind.TotalLCBPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBPCTotal, attNameExtension), ind.TotalLCBPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBAPTotal, attNameExtension), ind.TotalLCBAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBACTotal, attNameExtension), ind.TotalLCBACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalLCBAPChange, attNameExtension), ind.TotalLCBAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCBACChange, attNameExtension), ind.TotalLCBACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCBPPPercent, attNameExtension), ind.TotalLCBPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCBPCPercent, attNameExtension), ind.TotalLCBPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalLCBPFPercent, attNameExtension), ind.TotalLCBPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalREAABenefit, attNameExtension), ind.TotalREAABenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAPFTotal, attNameExtension), ind.TotalREAAPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAPCTotal, attNameExtension), ind.TotalREAAPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAAPTotal, attNameExtension), ind.TotalREAAAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAACTotal, attNameExtension), ind.TotalREAAACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalREAAAPChange, attNameExtension), ind.TotalREAAAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalREAAACChange, attNameExtension), ind.TotalREAAACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalREAAPPPercent, attNameExtension), ind.TotalREAAPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalREAAPCPercent, attNameExtension), ind.TotalREAAPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalREAAPFPercent, attNameExtension), ind.TotalREAAPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(cTotalRUnitBenefit, attNameExtension), ind.TotalRUnitBenefit.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitPFTotal, attNameExtension), ind.TotalRUnitPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitPCTotal, attNameExtension), ind.TotalRUnitPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitAPTotal, attNameExtension), ind.TotalRUnitAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitACTotal, attNameExtension), ind.TotalRUnitACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRUnitAPChange, attNameExtension), ind.TotalRUnitAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRUnitACChange, attNameExtension), ind.TotalRUnitACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRUnitPPPercent, attNameExtension), ind.TotalRUnitPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRUnitPCPercent, attNameExtension), ind.TotalRUnitPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRUnitPFPercent, attNameExtension), ind.TotalRUnitPFPercent.ToString("N2", CultureInfo.InvariantCulture));
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
                lca1Stock.Progress1 = new LCA1Progress1();
                //need one property set
                lca1Stock.Progress1.SubApplicationType = lca1Stock.CalcParameters.SubApplicationType.ToString();
                CopyTotalToProgressStock(lca1Stock.Progress1, lca1Stock.Total1);
            }
            return bHasAnalyses;
        }
        //calcs holds the collections needing progress analysis
        public bool RunAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //set calculated changestocks
            List<LCA1Stock> progressStocks = new List<LCA1Stock>();
            if (lca1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || lca1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                progressStocks = SetIOAnalyses(lca1Stock, calcs);
            }
            else
            {
                if (lca1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || lca1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no progress
                    progressStocks = SetTotals(lca1Stock, calcs);
                }
                else if (lca1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || lca1Stock.CalcParameters.CurrentElementNodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //tps with currentnodename set only need nets (inputs minus outputs)
                    //note that only sb1stock is used (not progressStocks)
                    progressStocks = SetTotals(lca1Stock, calcs);
                }
                else
                {
                    progressStocks = SetAnalyses(lca1Stock, calcs);
                }
            }
            //add the progresstocks to parent stock
            if (progressStocks != null)
            {
                bHasAnalyses = AddProgsToBaseStock(lca1Stock, progressStocks);
                //lca1Stock must still add the members of progress1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        
        private List<LCA1Stock> SetTotals(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            List<LCA1Stock> progressStocks = new List<LCA1Stock>();
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
                        if (stock.Progress1 != null)
                        {
                            //calc holds an input or output stock
                            //add that stock to lca1stock (some analyses will need to use subprices too)
                            bHasTotals = AddSubTotalToTotalStock(lca1Stock.Progress1, stock.Multiplier, stock.Progress1);
                            //at tp level could also run nets in the future
                            if (bHasTotals)
                            {
                                progressStocks.Add(lca1Stock);
                            }
                        }
                    }
                }
            }
            return progressStocks;
        }
        private List<LCA1Stock> SetAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            List<LCA1Stock> progressStocks = new List<LCA1Stock>();
            bool bHasTotals = false;
            //set N
            int iCostN = 0;
            int iTotalN = 0;
            //set the calc totals in each observation
            LCA1Stock observationStock = new LCA1Stock();
            observationStock.Progress1 = new LCA1Progress1();
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(lca1Stock.GetType()))
                {
                    LCA1Stock stock = (LCA1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                            LCA1Stock observation2Stock = new LCA1Stock();
                            //stocks need some props set
                            stock.CalcParameters.CurrentElementNodeName 
                                = lca1Stock.CalcParameters.CurrentElementNodeName;
                            //168 need calc.Mults not agg.Mults
                            //stock.Multiplier = lca1Stock.Multiplier;
                            bHasTotals = SetObservationStock(progressStocks, calc,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                progressStocks.Add(observation2Stock);
                                //N is determined from the cost SubP1Stock
                                if (observation2Stock.Progress1.SubP1Stock != null)
                                {
                                    if (observation2Stock.Progress1.SubP1Stock.SubPrice1Stocks != null)
                                    {
                                        if (observation2Stock.Progress1.SubP1Stock.SubPrice1Stocks.Count > 0)
                                        {
                                            iCostN++;
                                        }
                                    }
                                }
                                //and from the benefit SubP2Stock
                                if (observation2Stock.Progress1.SubP2Stock != null)
                                {
                                    if (observation2Stock.Progress1.SubP2Stock.SubPrice2Stocks != null)
                                    {
                                        if (observation2Stock.Progress1.SubP2Stock.SubPrice2Stocks.Count > 0)
                                        {
                                            iTotalN++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (iCostN > 0 || iTotalN > 0)
            {
                bHasTotals = SetProgressAnalysis(progressStocks, lca1Stock, iCostN, iTotalN);
            }
            return progressStocks;
        }
        private List<LCA1Stock> SetIOAnalyses(LCA1Stock lca1Stock, List<Calculator1> calcs)
        {
            List<LCA1Stock> progressStocks = new List<LCA1Stock>();
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iCostN2 = 0;
            int iTotalN2 = 0;
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(lca1Stock.GetType()))
                {
                    LCA1Stock stock = (LCA1Stock)calc;
                    if (stock != null)
                    {
                        //stock.Progress1 holds the initial substock/price totals
                        if (stock.Progress1 != null)
                        {
                            LCA1Stock observation2Stock = new LCA1Stock();
                            stock.Multiplier = lca1Stock.Multiplier;
                            bHasTotals = SetObservationStock(progressStocks, calc,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                progressStocks.Add(observation2Stock);
                                //N is determined from the cost SubP1Stock
                                if (observation2Stock.Progress1.SubP1Stock != null)
                                {
                                    if (observation2Stock.Progress1.SubP1Stock.SubPrice1Stocks != null)
                                    {
                                        if (observation2Stock.Progress1.SubP1Stock.SubPrice1Stocks.Count > 0)
                                        {
                                            iCostN2++;
                                        }
                                    }
                                }
                                //and from the benefit SubP2Stock
                                if (observation2Stock.Progress1.SubP2Stock != null)
                                {
                                    if (observation2Stock.Progress1.SubP2Stock.SubPrice2Stocks != null)
                                    {
                                        if (observation2Stock.Progress1.SubP2Stock.SubPrice2Stocks.Count > 0)
                                        {
                                            iTotalN2++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (iCostN2 > 0 || iTotalN2 > 0)
            {
                bHasTotals = SetProgressAnalysis(progressStocks, lca1Stock, iCostN2, iTotalN2);
            }
            return progressStocks;
        }
    
        private bool SetObservationStock(List<LCA1Stock> progressStocks,
            Calculator1 calc, LCA1Stock stock, LCA1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Progress1 = new LCA1Progress1();
            observation2Stock.Id = stock.Id;
            observation2Stock.Progress1.Id = stock.Id;
            //copy some stock props to progress1
            BILCA1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock.Progress1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BILCA1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Progress1.CalcParameters == null)
                stock.Progress1.CalcParameters = new CalculatorParameters();
            stock.Progress1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //at oc and outcome level no aggregating by targettype, need schedule variances)
            bHasTotals = SetSubTotalFromTotalStock(observation2Stock.Progress1,
                stock.Multiplier, stock.Progress1);
            return bHasTotals;
        }
        private bool SetProgressAnalysis(List<LCA1Stock> progressStocks, LCA1Stock lca1Stock,
            int costN, int benN)
        {
            bool bHasTotals = false;
            if (costN > 0)
            {
                //set progress numbers
                SetOCProgress(lca1Stock, progressStocks);
                SetAOHProgress(lca1Stock, progressStocks);
                SetCAPProgress(lca1Stock, progressStocks);
                SetLCCProgress(lca1Stock, progressStocks);
                SetEAAProgress(lca1Stock, progressStocks);
                SetUnitProgress(lca1Stock, progressStocks);
            }
            if (benN > 0)
            {
                //benefits
                SetRProgress(lca1Stock, progressStocks);
                SetLCBProgress(lca1Stock, progressStocks);
                SetREAAProgress(lca1Stock, progressStocks);
                SetRUnitProgress(lca1Stock, progressStocks);
            }
            //add cumulative totals to parent lcastock1 (ocgroup)
            AddCumulative1SubStocks(progressStocks, lca1Stock);
            bHasTotals = AddCumulative1Calcs(progressStocks, lca1Stock);
            return bHasTotals;
        }
        private bool AddCumulative1Calcs(List<LCA1Stock> progressStocks, LCA1Stock lca1Stock)
        {
            bool bHasTotals = false;
            //could be all planned, all actual
            LCA1Stock cumChange = progressStocks.LastOrDefault();
            //or both
            if (progressStocks.Any(p => p.TargetType == TARGET_TYPES.benchmark.ToString()))
            {
                if (progressStocks.Any(p2 => p2.TargetType == TARGET_TYPES.actual.ToString()))
                {
                    //the last actual holds the cumulatives and that's all that is needed
                    cumChange = progressStocks.LastOrDefault(p => p.TargetType == TARGET_TYPES.actual.ToString());
                }
            }
            if (cumChange != null)
            {
                if (!string.IsNullOrEmpty(cumChange.TargetType))
                {
                    if (cumChange.Progress1 != null)
                    {
                        AddCumulativeTotals(lca1Stock,
                            cumChange.Progress1);
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private void AddCumulative1SubStocks(List<LCA1Stock> progressStocks, LCA1Stock lca1Stock)
        {
            bool bHasBoth = false;
            //add actual subtotals
            if (progressStocks.Any(p => p.TargetType == TARGET_TYPES.benchmark.ToString()))
            {
                if (progressStocks.Any(p2 => p2.TargetType == TARGET_TYPES.actual.ToString()))
                {
                    bHasBoth = true;
                }
            }
            foreach (var stat in progressStocks)
            {
                if (bHasBoth)
                {
                    //the actual hold the cumulatives and that's all that can be displayed
                    if (stat.TargetType == TARGET_TYPES.actual.ToString())
                    {
                        //actual cost subtotals
                        AddSubStock1Totals(lca1Stock.Progress1, stat.Progress1);
                        //actual benefit subtotals
                        AddSubStock2Totals(lca1Stock.Progress1, stat.Progress1);
                    }
                }
                else
                {
                    //actual cost subtotals
                    AddSubStock1Totals(lca1Stock.Progress1, stat.Progress1);
                    //actual benefit subtotals
                    AddSubStock2Totals(lca1Stock.Progress1, stat.Progress1);
                }
            }
        }
        public static bool AddCumulative1Calcs(LCA1Stock totalsStock, List<Calculator1> calcs)
        {
            bool bHasTotals = false;
            LCA1Stock stock = new LCA1Stock();
            if (calcs != null)
            {
                foreach (var calc in calcs)
                {
                    if (calc.GetType().Equals(stock.GetType()))
                    {
                        //convert basecalc to stock
                        stock = (LCA1Stock)calc;
                    }
                }
                //last stock in a series always has cumulatives (planned, actual, or both)
                if (!string.IsNullOrEmpty(stock.TargetType))
                {
                    if (stock.Progress1 != null)
                    {
                        AddCumulativeTotals(totalsStock, stock.Progress1);
                        //actual cost subtotals
                        CopySubStock1Totals(totalsStock.Progress1, stock.Progress1);
                        //actual benefit subtotals
                        CopySubStock2Totals(totalsStock.Progress1, stock.Progress1);
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private static void AddCumulativeTotals(LCA1Stock lca1Stock,
            LCA1Progress1 cumulativeTotal)
        {
            if (lca1Stock.Progress1 == null)
            {
                lca1Stock.Progress1 = new LCA1Progress1();
            }
            if (cumulativeTotal.TargetType == TARGET_TYPES.benchmark.ToString())
            {
                lca1Stock.Progress1.TotalOCCost = cumulativeTotal.TotalOCPCTotal;
                lca1Stock.Progress1.TotalAOHCost = cumulativeTotal.TotalAOHPCTotal;
                lca1Stock.Progress1.TotalCAPCost = cumulativeTotal.TotalCAPPCTotal;
                lca1Stock.Progress1.TotalLCCCost = cumulativeTotal.TotalLCCPCTotal;
                lca1Stock.Progress1.TotalEAACost = cumulativeTotal.TotalEAAPCTotal;
                lca1Stock.Progress1.TotalUnitCost = cumulativeTotal.TotalUnitPCTotal;
                lca1Stock.Progress1.TotalRBenefit = cumulativeTotal.TotalRPCTotal;
                lca1Stock.Progress1.TotalLCBBenefit = cumulativeTotal.TotalLCBPCTotal;
                lca1Stock.Progress1.TotalREAABenefit = cumulativeTotal.TotalREAAPCTotal;
                lca1Stock.Progress1.TotalRUnitBenefit = cumulativeTotal.TotalRUnitPCTotal;
                //lca1Stock.Progress1.TotalOCCost = cumulativeTotal.TotalOCPFTotal;
            }
            else
            {
                lca1Stock.Progress1.TotalOCCost = cumulativeTotal.TotalOCACTotal;
                lca1Stock.Progress1.TotalAOHCost = cumulativeTotal.TotalAOHACTotal;
                lca1Stock.Progress1.TotalCAPCost = cumulativeTotal.TotalCAPACTotal;
                lca1Stock.Progress1.TotalLCCCost = cumulativeTotal.TotalLCCACTotal;
                lca1Stock.Progress1.TotalEAACost = cumulativeTotal.TotalEAAACTotal;
                lca1Stock.Progress1.TotalUnitCost = cumulativeTotal.TotalUnitACTotal;
                lca1Stock.Progress1.TotalRBenefit = cumulativeTotal.TotalRACTotal;
                lca1Stock.Progress1.TotalLCBBenefit = cumulativeTotal.TotalLCBACTotal;
                lca1Stock.Progress1.TotalREAABenefit = cumulativeTotal.TotalREAAACTotal;
                lca1Stock.Progress1.TotalRUnitBenefit = cumulativeTotal.TotalRUnitACTotal;
            }
            //subtotals for actuals already added
        }
        //private static void AddCumulativeTotals(LCA1Stock lca1Stock,
        //    LCA1Progress1 cumulativeTotal)
        //{
        //    if (lca1Stock.Progress1 == null)
        //    {
        //        lca1Stock.Progress1 = new LCA1Progress1();
        //    }
        //    lca1Stock.Progress1.TotalOCCost = cumulativeTotal.TotalOCCost;
        //    lca1Stock.Progress1.TotalOCACChange = cumulativeTotal.TotalOCACChange;
        //    lca1Stock.Progress1.TotalOCACTotal = cumulativeTotal.TotalOCACTotal;
        //    lca1Stock.Progress1.TotalOCAPChange = cumulativeTotal.TotalOCAPChange;
        //    lca1Stock.Progress1.TotalOCAPTotal = cumulativeTotal.TotalOCAPTotal;
        //    lca1Stock.Progress1.TotalOCPCTotal = cumulativeTotal.TotalOCPCTotal;
        //    lca1Stock.Progress1.TotalOCPFTotal = cumulativeTotal.TotalOCPFTotal;
        //    lca1Stock.Progress1.TotalOCPPPercent = cumulativeTotal.TotalOCPPPercent;
        //    lca1Stock.Progress1.TotalOCPCPercent = cumulativeTotal.TotalOCPCPercent;
        //    lca1Stock.Progress1.TotalOCPFPercent = cumulativeTotal.TotalOCPFPercent;

        //    lca1Stock.Progress1.TotalAOHCost = cumulativeTotal.TotalAOHCost;
        //    lca1Stock.Progress1.TotalAOHACChange = cumulativeTotal.TotalAOHACChange;
        //    lca1Stock.Progress1.TotalAOHACTotal = cumulativeTotal.TotalAOHACTotal;
        //    lca1Stock.Progress1.TotalAOHAPChange = cumulativeTotal.TotalAOHAPChange;
        //    lca1Stock.Progress1.TotalAOHAPTotal = cumulativeTotal.TotalAOHAPTotal;
        //    lca1Stock.Progress1.TotalAOHPCTotal = cumulativeTotal.TotalAOHPCTotal;
        //    lca1Stock.Progress1.TotalAOHPFTotal = cumulativeTotal.TotalAOHPFTotal;
        //    lca1Stock.Progress1.TotalAOHPPPercent = cumulativeTotal.TotalAOHPPPercent;
        //    lca1Stock.Progress1.TotalAOHPCPercent = cumulativeTotal.TotalAOHPCPercent;
        //    lca1Stock.Progress1.TotalAOHPFPercent = cumulativeTotal.TotalAOHPFPercent;

        //    lca1Stock.Progress1.TotalCAPCost = cumulativeTotal.TotalCAPCost;
        //    lca1Stock.Progress1.TotalCAPACChange = cumulativeTotal.TotalCAPACChange;
        //    lca1Stock.Progress1.TotalCAPACTotal = cumulativeTotal.TotalCAPACTotal;
        //    lca1Stock.Progress1.TotalCAPAPChange = cumulativeTotal.TotalCAPAPChange;
        //    lca1Stock.Progress1.TotalCAPAPTotal = cumulativeTotal.TotalCAPAPTotal;
        //    lca1Stock.Progress1.TotalCAPPCTotal = cumulativeTotal.TotalCAPPCTotal;
        //    lca1Stock.Progress1.TotalCAPPFTotal = cumulativeTotal.TotalCAPPFTotal;
        //    lca1Stock.Progress1.TotalCAPPPPercent = cumulativeTotal.TotalCAPPPPercent;
        //    lca1Stock.Progress1.TotalCAPPCPercent = cumulativeTotal.TotalCAPPCPercent;
        //    lca1Stock.Progress1.TotalCAPPFPercent = cumulativeTotal.TotalCAPPFPercent;

        //    lca1Stock.Progress1.TotalLCCCost = cumulativeTotal.TotalLCCCost;
        //    lca1Stock.Progress1.TotalLCCACChange = cumulativeTotal.TotalLCCACChange;
        //    lca1Stock.Progress1.TotalLCCACTotal = cumulativeTotal.TotalLCCACTotal;
        //    lca1Stock.Progress1.TotalLCCAPChange = cumulativeTotal.TotalLCCAPChange;
        //    lca1Stock.Progress1.TotalLCCAPTotal = cumulativeTotal.TotalLCCAPTotal;
        //    lca1Stock.Progress1.TotalLCCPCTotal = cumulativeTotal.TotalLCCPCTotal;
        //    lca1Stock.Progress1.TotalLCCPFTotal = cumulativeTotal.TotalLCCPFTotal;
        //    lca1Stock.Progress1.TotalLCCPPPercent = cumulativeTotal.TotalLCCPPPercent;
        //    lca1Stock.Progress1.TotalLCCPCPercent = cumulativeTotal.TotalLCCPCPercent;
        //    lca1Stock.Progress1.TotalLCCPFPercent = cumulativeTotal.TotalLCCPFPercent;

        //    lca1Stock.Progress1.TotalEAACost = cumulativeTotal.TotalEAACost;
        //    lca1Stock.Progress1.TotalEAAACChange = cumulativeTotal.TotalEAAACChange;
        //    lca1Stock.Progress1.TotalEAAACTotal = cumulativeTotal.TotalEAAACTotal;
        //    lca1Stock.Progress1.TotalEAAAPChange = cumulativeTotal.TotalEAAAPChange;
        //    lca1Stock.Progress1.TotalEAAAPTotal = cumulativeTotal.TotalEAAAPTotal;
        //    lca1Stock.Progress1.TotalEAAPCTotal = cumulativeTotal.TotalEAAPCTotal;
        //    lca1Stock.Progress1.TotalEAAPFTotal = cumulativeTotal.TotalEAAPFTotal;
        //    lca1Stock.Progress1.TotalEAAPPPercent = cumulativeTotal.TotalEAAPPPercent;
        //    lca1Stock.Progress1.TotalEAAPCPercent = cumulativeTotal.TotalEAAPCPercent;
        //    lca1Stock.Progress1.TotalEAAPFPercent = cumulativeTotal.TotalEAAPFPercent;

        //    lca1Stock.Progress1.TotalUnitCost = cumulativeTotal.TotalUnitCost;
        //    lca1Stock.Progress1.TotalUnitACChange = cumulativeTotal.TotalUnitACChange;
        //    lca1Stock.Progress1.TotalUnitACTotal = cumulativeTotal.TotalUnitACTotal;
        //    lca1Stock.Progress1.TotalUnitAPChange = cumulativeTotal.TotalUnitAPChange;
        //    lca1Stock.Progress1.TotalUnitAPTotal = cumulativeTotal.TotalUnitAPTotal;
        //    lca1Stock.Progress1.TotalUnitPCTotal = cumulativeTotal.TotalUnitPCTotal;
        //    lca1Stock.Progress1.TotalUnitPFTotal = cumulativeTotal.TotalUnitPFTotal;
        //    lca1Stock.Progress1.TotalUnitPPPercent = cumulativeTotal.TotalUnitPPPercent;
        //    lca1Stock.Progress1.TotalUnitPCPercent = cumulativeTotal.TotalUnitPCPercent;
        //    lca1Stock.Progress1.TotalUnitPFPercent = cumulativeTotal.TotalUnitPFPercent;

        //    lca1Stock.Progress1.TotalRBenefit = cumulativeTotal.TotalRBenefit;
        //    lca1Stock.Progress1.TotalRACChange = cumulativeTotal.TotalRACChange;
        //    lca1Stock.Progress1.TotalRACTotal = cumulativeTotal.TotalRACTotal;
        //    lca1Stock.Progress1.TotalRAPChange = cumulativeTotal.TotalRAPChange;
        //    lca1Stock.Progress1.TotalRAPTotal = cumulativeTotal.TotalRAPTotal;
        //    lca1Stock.Progress1.TotalRPCTotal = cumulativeTotal.TotalRPCTotal;
        //    lca1Stock.Progress1.TotalRPFTotal = cumulativeTotal.TotalRPFTotal;
        //    lca1Stock.Progress1.TotalRPPPercent = cumulativeTotal.TotalRPPPercent;
        //    lca1Stock.Progress1.TotalRPCPercent = cumulativeTotal.TotalRPCPercent;
        //    lca1Stock.Progress1.TotalRPFPercent = cumulativeTotal.TotalRPFPercent;

        //    lca1Stock.Progress1.TotalLCBBenefit = cumulativeTotal.TotalLCBBenefit;
        //    lca1Stock.Progress1.TotalLCBACChange = cumulativeTotal.TotalLCBACChange;
        //    lca1Stock.Progress1.TotalLCBACTotal = cumulativeTotal.TotalLCBACTotal;
        //    lca1Stock.Progress1.TotalLCBAPChange = cumulativeTotal.TotalLCBAPChange;
        //    lca1Stock.Progress1.TotalLCBAPTotal = cumulativeTotal.TotalLCBAPTotal;
        //    lca1Stock.Progress1.TotalLCBPCTotal = cumulativeTotal.TotalLCBPCTotal;
        //    lca1Stock.Progress1.TotalLCBPFTotal = cumulativeTotal.TotalLCBPFTotal;
        //    lca1Stock.Progress1.TotalLCBPPPercent = cumulativeTotal.TotalLCBPPPercent;
        //    lca1Stock.Progress1.TotalLCBPCPercent = cumulativeTotal.TotalLCBPCPercent;
        //    lca1Stock.Progress1.TotalLCBPFPercent = cumulativeTotal.TotalLCBPFPercent;

        //    lca1Stock.Progress1.TotalREAABenefit = cumulativeTotal.TotalREAABenefit;
        //    lca1Stock.Progress1.TotalREAAACChange = cumulativeTotal.TotalREAAACChange;
        //    lca1Stock.Progress1.TotalREAAACTotal = cumulativeTotal.TotalREAAACTotal;
        //    lca1Stock.Progress1.TotalREAAAPChange = cumulativeTotal.TotalREAAAPChange;
        //    lca1Stock.Progress1.TotalREAAAPTotal = cumulativeTotal.TotalREAAAPTotal;
        //    lca1Stock.Progress1.TotalREAAPCTotal = cumulativeTotal.TotalREAAPCTotal;
        //    lca1Stock.Progress1.TotalREAAPFTotal = cumulativeTotal.TotalREAAPFTotal;
        //    lca1Stock.Progress1.TotalREAAPPPercent = cumulativeTotal.TotalREAAPPPercent;
        //    lca1Stock.Progress1.TotalREAAPCPercent = cumulativeTotal.TotalREAAPCPercent;
        //    lca1Stock.Progress1.TotalREAAPFPercent = cumulativeTotal.TotalREAAPFPercent;

        //    lca1Stock.Progress1.TotalRUnitBenefit = cumulativeTotal.TotalRUnitBenefit;
        //    lca1Stock.Progress1.TotalRUnitACChange = cumulativeTotal.TotalRUnitACChange;
        //    lca1Stock.Progress1.TotalRUnitACTotal = cumulativeTotal.TotalRUnitACTotal;
        //    lca1Stock.Progress1.TotalRUnitAPChange = cumulativeTotal.TotalRUnitAPChange;
        //    lca1Stock.Progress1.TotalRUnitAPTotal = cumulativeTotal.TotalRUnitAPTotal;
        //    lca1Stock.Progress1.TotalRUnitPCTotal = cumulativeTotal.TotalRUnitPCTotal;
        //    lca1Stock.Progress1.TotalRUnitPFTotal = cumulativeTotal.TotalRUnitPFTotal;
        //    lca1Stock.Progress1.TotalRUnitPPPercent = cumulativeTotal.TotalRUnitPPPercent;
        //    lca1Stock.Progress1.TotalRUnitPCPercent = cumulativeTotal.TotalRUnitPCPercent;
        //    lca1Stock.Progress1.TotalRUnitPFPercent = cumulativeTotal.TotalRUnitPFPercent;
        //    //subtotals for actuals already added
        //}
        public bool CopyTotalToProgressStock(LCA1Progress1 totStock, LCA1Total1 subTotal)
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
        private void CopySubStock1Totals(LCA1Progress1 totStock, LCA1Total1 subTotal)
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
        private static void CopySubStock1Totals(LCA1Progress1 totStock, LCA1Progress1 subTotal)
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
        private void CopySubStock2Totals(LCA1Progress1 totStock, LCA1Total1 subTotal)
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
        private static void CopySubStock2Totals(LCA1Progress1 totStock, LCA1Progress1 subTotal)
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
        public bool AddSubTotalToTotalStock(LCA1Progress1 totStock, double multiplier,
            LCA1Progress1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            //inputs and outputs need cumulative totals 
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
        public bool SetSubTotalFromTotalStock(LCA1Progress1 totStock, double multiplier,
            LCA1Progress1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            //only inputs and outputs need +-
            //the SetOCProgress set cumulative totals (not here)
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
            AddSubStock1Totals(totStock, subTotal);
            //benefit subtotals
            AddSubStock2Totals(totStock, subTotal);
            bHasCalculations = true;
            return bHasCalculations;
        }
        
        private void AddSubStock1Totals(LCA1Progress1 totStock, LCA1Progress1 subTotal)
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
        private void AddSubStock2Totals(LCA1Progress1 totStock, LCA1Progress1 subTotal)
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

        private static void ChangeSubTotalByMultipliers(LCA1Progress1 subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
            //adjust TotalOCCost -that's the only number used in SetOCProgress
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
                    //do not progress price or amount 
                }
            }
            if (subTotal.SubP2Stock.SubPrice2Stocks != null)
            {
                foreach (SubPrice2Stock stock in subTotal.SubP2Stock.SubPrice2Stocks)
                {
                    stock.TotalSubP2TotalPerUnit = stock.TotalSubP2TotalPerUnit * multiplier;
                    stock.TotalSubP2Total = stock.TotalSubP2Total * multiplier;
                    //do not progress price or amount 
                }
            }
        }
        
        
        private static void SetOCProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalOCCost
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalOCCost;
                    //planned cumulative
                    planned.Progress1.TotalOCPCTotal = dbPlannedTotalCost;
                    
                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalOCPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalOCCost;
                    actual.Progress1.TotalOCACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalOCAPTotal = actual.Progress1.TotalOCCost;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalOCPCTotal = planned.Progress1.TotalOCPCTotal;
                        //set actual.planned period
                        //TotalOCCost is always planned period and TotalOCAPTotal is actual period
                        actual.Progress1.TotalOCCost = planned.Progress1.TotalOCCost;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalOCPFTotal = planned.Progress1.TotalOCPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalOCAPChange = actual.Progress1.TotalOCAPTotal - actual.Progress1.TotalOCCost;
                    //cumulative change
                    actual.Progress1.TotalOCACChange = actual.Progress1.TotalOCACTotal - actual.Progress1.TotalOCPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalOCPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalOCAPTotal, actual.Progress1.TotalOCCost);
                    actual.Progress1.TotalOCPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalOCACTotal, actual.Progress1.TotalOCPCTotal);
                    actual.Progress1.TotalOCPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalOCACTotal, actual.Progress1.TotalOCPFTotal);
                }
            }
        }
        private static LCA1Stock GetProgressStockByLabel(LCA1Stock actual, List<int> ids,
            List<LCA1Stock> progressStocks, string targetType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            LCA1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (progressStocks.Any(p => p.Label == actual.Label
                && p.TargetType == targetType))
            {
                int iIndex = 1;
                foreach (LCA1Stock planned in progressStocks)
                {
                    if (planned.TargetType == targetType)
                    {
                        if (actual.Label == planned.Label)
                        {
                            //make sure it hasn't already been used (2 or more els with same Labels)
                            if (!ids.Any(i => i == iIndex))
                            {
                                plannedMatch = planned;
                                //index based check is ok
                                ids.Add(iIndex);
                                //break the for loop
                                break;
                            }
                            else
                            {
                                bool bHasMatch = HasProgressMatchByLabel(actual.Label, planned,
                                    progressStocks, targetType);
                                if (!bHasMatch)
                                {
                                    //if no match use the last one (i.e. input series with 1 bm and 20 actuals)
                                   plannedMatch = progressStocks.LastOrDefault(p => p.Label == actual.Label
                                        && p.TargetType == targetType);
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
        private static bool HasProgressMatchByLabel(string aggLabel,
            LCA1Stock planned, List<LCA1Stock> progressStocks, string targetType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (LCA1Stock rp in progressStocks)
            {
                if (rp.TargetType == targetType)
                {
                    if (bStart)
                    {
                        if (aggLabel == planned.Label)
                        {
                            bHasMatch = true;
                            break;
                        }
                    }
                    if (rp.Id == planned.Id)
                    {
                        bStart = true;
                    }
                }
            }
            return bHasMatch;
        }
        private static void SetAOHProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalOCCost
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAOHCost;
                    //planned cumulative
                    planned.Progress1.TotalAOHPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAOHPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAOHCost;
                    actual.Progress1.TotalAOHACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAOHAPTotal = actual.Progress1.TotalAOHCost;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAOHPCTotal = planned.Progress1.TotalAOHPCTotal;
                        //set actual.planned period
                        //TotalAOHCost is always planned period and TotalAOHAPTotal is actual period
                        actual.Progress1.TotalAOHCost = planned.Progress1.TotalAOHCost;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAOHPFTotal = planned.Progress1.TotalAOHPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAOHAPChange = actual.Progress1.TotalAOHAPTotal - actual.Progress1.TotalAOHCost;
                    //cumulative change
                    actual.Progress1.TotalAOHACChange = actual.Progress1.TotalAOHACTotal - actual.Progress1.TotalAOHPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAOHPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAOHAPTotal, actual.Progress1.TotalAOHCost);
                    actual.Progress1.TotalAOHPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAOHACTotal, actual.Progress1.TotalAOHPCTotal);
                    actual.Progress1.TotalAOHPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAOHACTotal, actual.Progress1.TotalAOHPFTotal);
                }
            }
        }
        private static void SetCAPProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalCAPCost
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalCAPCost;
                    //planned cumulative
                    planned.Progress1.TotalCAPPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalCAPPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalCAPCost;
                    actual.Progress1.TotalCAPACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalCAPAPTotal = actual.Progress1.TotalCAPCost;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalCAPPCTotal = planned.Progress1.TotalCAPPCTotal;
                        //set actual.planned period
                        //TotalCAPCost is always planned period and TotalCAPAPTotal is actual period
                        actual.Progress1.TotalCAPCost = planned.Progress1.TotalCAPCost;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalCAPPFTotal = planned.Progress1.TotalCAPPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalCAPAPChange = actual.Progress1.TotalCAPAPTotal - actual.Progress1.TotalCAPCost;
                    //cumulative change
                    actual.Progress1.TotalCAPACChange = actual.Progress1.TotalCAPACTotal - actual.Progress1.TotalCAPPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalCAPPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalCAPAPTotal, actual.Progress1.TotalCAPCost);
                    actual.Progress1.TotalCAPPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalCAPACTotal, actual.Progress1.TotalCAPPCTotal);
                    actual.Progress1.TotalCAPPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalCAPACTotal, actual.Progress1.TotalCAPPFTotal);
                }
            }
        }
        private static void SetLCCProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalLCCCost
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalLCCCost;
                    //planned cumulative
                    planned.Progress1.TotalLCCPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalLCCPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalLCCCost;
                    actual.Progress1.TotalLCCACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalLCCAPTotal = actual.Progress1.TotalLCCCost;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalLCCPCTotal = planned.Progress1.TotalLCCPCTotal;
                        //set actual.planned period
                        //TotalLCCCost is always planned period and TotalLCCAPTotal is actual period
                        actual.Progress1.TotalLCCCost = planned.Progress1.TotalLCCCost;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalLCCPFTotal = planned.Progress1.TotalLCCPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalLCCAPChange = actual.Progress1.TotalLCCAPTotal - actual.Progress1.TotalLCCCost;
                    //cumulative change
                    actual.Progress1.TotalLCCACChange = actual.Progress1.TotalLCCACTotal - actual.Progress1.TotalLCCPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalLCCPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalLCCAPTotal, actual.Progress1.TotalLCCCost);
                    actual.Progress1.TotalLCCPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalLCCACTotal, actual.Progress1.TotalLCCPCTotal);
                    actual.Progress1.TotalLCCPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalLCCACTotal, actual.Progress1.TotalLCCPFTotal);
                }
            }
        }
        private static void SetEAAProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalLCCCost
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalEAACost;
                    //planned cumulative
                    planned.Progress1.TotalEAAPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalEAAPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalEAACost;
                    actual.Progress1.TotalEAAACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalEAAAPTotal = actual.Progress1.TotalEAACost;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalEAAPCTotal = planned.Progress1.TotalEAAPCTotal;
                        //set actual.planned period
                        //TotalEAACost is always planned period and TotalEAAAPTotal is actual period
                        actual.Progress1.TotalEAACost = planned.Progress1.TotalEAACost;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalEAAPFTotal = planned.Progress1.TotalEAAPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalEAAAPChange = actual.Progress1.TotalEAAAPTotal - actual.Progress1.TotalEAACost;
                    //cumulative change
                    actual.Progress1.TotalEAAACChange = actual.Progress1.TotalEAAACTotal - actual.Progress1.TotalEAAPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalEAAPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalEAAAPTotal, actual.Progress1.TotalEAACost);
                    actual.Progress1.TotalEAAPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalEAAACTotal, actual.Progress1.TotalEAAPCTotal);
                    actual.Progress1.TotalEAAPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalEAAACTotal, actual.Progress1.TotalEAAPFTotal);
                }
            }
        }
        private static void SetUnitProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalUnitCost
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalUnitCost;
                    //planned cumulative
                    planned.Progress1.TotalUnitPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalUnitPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalUnitCost;
                    actual.Progress1.TotalUnitACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalUnitAPTotal = actual.Progress1.TotalUnitCost;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalUnitPCTotal = planned.Progress1.TotalUnitPCTotal;
                        //set actual.planned period
                        //TotalUnitCost is always planned period and TotalUnitAPTotal is actual period
                        actual.Progress1.TotalUnitCost = planned.Progress1.TotalUnitCost;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalUnitPFTotal = planned.Progress1.TotalUnitPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalUnitAPChange = actual.Progress1.TotalUnitAPTotal - actual.Progress1.TotalUnitCost;
                    //cumulative change
                    actual.Progress1.TotalUnitACChange = actual.Progress1.TotalUnitACTotal - actual.Progress1.TotalUnitPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalUnitPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalUnitAPTotal, actual.Progress1.TotalUnitCost);
                    actual.Progress1.TotalUnitPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalUnitACTotal, actual.Progress1.TotalUnitPCTotal);
                    actual.Progress1.TotalUnitPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalUnitACTotal, actual.Progress1.TotalUnitPFTotal);
                }
            }
        }
        private static void SetRProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalRBenefit
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalRBenefit;
                    //planned cumulative
                    planned.Progress1.TotalRPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalRPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalRBenefit;
                    actual.Progress1.TotalRACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalRAPTotal = actual.Progress1.TotalRBenefit;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalRPCTotal = planned.Progress1.TotalRPCTotal;
                        //set actual.planned period
                        //TotalRBenefit is always planned period and TotalRAPTotal is actual period
                        actual.Progress1.TotalRBenefit = planned.Progress1.TotalRBenefit;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalRPFTotal = planned.Progress1.TotalRPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalRAPChange = actual.Progress1.TotalRAPTotal - actual.Progress1.TotalRBenefit;
                    //cumulative change
                    actual.Progress1.TotalRACChange = actual.Progress1.TotalRACTotal - actual.Progress1.TotalRPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalRPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRAPTotal, actual.Progress1.TotalRBenefit);
                    actual.Progress1.TotalRPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRACTotal, actual.Progress1.TotalRPCTotal);
                    actual.Progress1.TotalRPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalRACTotal, actual.Progress1.TotalRPFTotal);
                }
            }
        }
        private static void SetLCBProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalLCBBenefit
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalLCBBenefit;
                    //planned cumulative
                    planned.Progress1.TotalLCBPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalLCBPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalLCBBenefit;
                    actual.Progress1.TotalLCBACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalLCBAPTotal = actual.Progress1.TotalLCBBenefit;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalLCBPCTotal = planned.Progress1.TotalLCBPCTotal;
                        //set actual.planned period
                        //TotalLCBBenefit is always planned period and TotalLCBAPTotal is actual period
                        actual.Progress1.TotalLCBBenefit = planned.Progress1.TotalLCBBenefit;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalLCBPFTotal = planned.Progress1.TotalLCBPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalLCBAPChange = actual.Progress1.TotalLCBAPTotal - actual.Progress1.TotalLCBBenefit;
                    //cumulative change
                    actual.Progress1.TotalLCBACChange = actual.Progress1.TotalLCBACTotal - actual.Progress1.TotalLCBPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalLCBPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalLCBAPTotal, actual.Progress1.TotalLCBBenefit);
                    actual.Progress1.TotalLCBPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalLCBACTotal, actual.Progress1.TotalLCBPCTotal);
                    actual.Progress1.TotalLCBPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalLCBACTotal, actual.Progress1.TotalLCBPFTotal);
                }
            }
        }
        private static void SetREAAProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalREAABenefit
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalREAABenefit;
                    //planned cumulative
                    planned.Progress1.TotalREAAPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalREAAPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalREAABenefit;
                    actual.Progress1.TotalREAAACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalREAAAPTotal = actual.Progress1.TotalREAABenefit;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalREAAPCTotal = planned.Progress1.TotalREAAPCTotal;
                        //set actual.planned period
                        //TotalREAABenefit is always planned period and TotalREAAAPTotal is actual period
                        actual.Progress1.TotalREAABenefit = planned.Progress1.TotalREAABenefit;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalREAAPFTotal = planned.Progress1.TotalREAAPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalREAAAPChange = actual.Progress1.TotalREAAAPTotal - actual.Progress1.TotalREAABenefit;
                    //cumulative change
                    actual.Progress1.TotalREAAACChange = actual.Progress1.TotalREAAACTotal - actual.Progress1.TotalREAAPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalREAAPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalREAAAPTotal, actual.Progress1.TotalREAABenefit);
                    actual.Progress1.TotalREAAPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalREAAACTotal, actual.Progress1.TotalREAAPCTotal);
                    actual.Progress1.TotalREAAPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalREAAACTotal, actual.Progress1.TotalREAAPFTotal);
                }
            }
        }
        private static void SetRUnitProgress(LCA1Stock lca1Stock, List<LCA1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalRUnitBenefit
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalRUnitBenefit;
                    //planned cumulative
                    planned.Progress1.TotalRUnitPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (LCA1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalRUnitPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (LCA1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalRUnitBenefit;
                    actual.Progress1.TotalRUnitACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalRUnitAPTotal = actual.Progress1.TotalRUnitBenefit;
                    //set the corresponding planned totals
                    LCA1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalRUnitPCTotal = planned.Progress1.TotalRUnitPCTotal;
                        //set actual.planned period
                        //TotalRUnitBenefit is always planned period and TotalRUnitAPTotal is actual period
                        actual.Progress1.TotalRUnitBenefit = planned.Progress1.TotalRUnitBenefit;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalRUnitPFTotal = planned.Progress1.TotalRUnitPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalRUnitAPChange = actual.Progress1.TotalRUnitAPTotal - actual.Progress1.TotalRUnitBenefit;
                    //cumulative change
                    actual.Progress1.TotalRUnitACChange = actual.Progress1.TotalRUnitACTotal - actual.Progress1.TotalRUnitPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalRUnitPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRUnitAPTotal, actual.Progress1.TotalRUnitBenefit);
                    actual.Progress1.TotalRUnitPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRUnitACTotal, actual.Progress1.TotalRUnitPCTotal);
                    actual.Progress1.TotalRUnitPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalRUnitACTotal, actual.Progress1.TotalRUnitPFTotal);
                }
            }
        }
        private bool AddProgsToBaseStock(LCA1Stock lca1Stock,
            List<LCA1Stock> progressStocks)
        {
            bool bHasAnalyses = false;
            lca1Stock.Stocks = new List<LCA1Stock>();
            foreach (LCA1Stock actual in progressStocks)
            {
                lca1Stock.Stocks.Add(actual);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
    public static class LCA1Progress1Extensions
    {
        public static void AddSubStock1ToTotalStocks(this LCA1Progress1 baseStat, SubPrice1Stock substock)
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
                //update the identifiers in case they have progressd
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
        public static void AddSubStock2ToTotalStocks(this LCA1Progress1 baseStat, SubPrice2Stock substock)
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
                //update the identifiers in case they have progressd
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
