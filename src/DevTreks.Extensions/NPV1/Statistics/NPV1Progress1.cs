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
    ///             NPV1Stock.Progress1
    ///             The class measures planned vs actual progress.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class NPV1Progress1 : NPV1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public NPV1Progress1()
            : base()
        {
            //subprice object
            InitTotalNPV1Progress1Properties(this);
        }
        //copy constructor
        public NPV1Progress1(NPV1Progress1 calculator)
            : base(calculator)
        {
            CopyTotalNPV1Progress1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent NPV1Stock
        //calculator properties
        //All Totals must come from base CostBenefitCalculator and stay consistent across apps
        //planned period
        //public double TotalAMOC { get; set; }
        //planned full (sum of all planning periods)
        public double TotalAMOCPFTotal { get; set; }
        //planned cumulative (sum of planning periods through corresponding actual period)
        public double TotalAMOCPCTotal { get; set; }
        //actual period
        public double TotalAMOCAPTotal { get; set; }
        //actual cumulative 
        public double TotalAMOCACTotal { get; set; }
        //change in actual period amount (actual amount - planned period amount
        public double TotalAMOCAPChange { get; set; }
        //change in actual cumulative amount (actual amount - planned cumulative amount
        public double TotalAMOCACChange { get; set; }
        //planned period
        public double TotalAMOCPPPercent { get; set; }
        //planned cumulative
        public double TotalAMOCPCPercent { get; set; }
        //planned full
        public double TotalAMOCPFPercent { get; set; }

        //public double TotalAMAOH { get; set; }
        public double TotalAMAOHPFTotal { get; set; }
        public double TotalAMAOHPCTotal { get; set; }
        public double TotalAMAOHAPTotal { get; set; }
        public double TotalAMAOHACTotal { get; set; }
        public double TotalAMAOHAPChange { get; set; }
        public double TotalAMAOHACChange { get; set; }
        public double TotalAMAOHPPPercent { get; set; }
        public double TotalAMAOHPCPercent { get; set; }
        public double TotalAMAOHPFPercent { get; set; }

        //public double TotalAMCAP { get; set; }
        public double TotalAMCAPPFTotal { get; set; }
        public double TotalAMCAPPCTotal { get; set; }
        public double TotalAMCAPAPTotal { get; set; }
        public double TotalAMCAPACTotal { get; set; }
        public double TotalAMCAPAPChange { get; set; }
        public double TotalAMCAPACChange { get; set; }
        public double TotalAMCAPPPPercent { get; set; }
        public double TotalAMCAPPCPercent { get; set; }
        public double TotalAMCAPPFPercent { get; set; }

        //total cost
        //public double TotalAMTOTAL { get; set; }
        public double TotalAMPFTotal { get; set; }
        public double TotalAMPCTotal { get; set; }
        public double TotalAMAPTotal { get; set; }
        public double TotalAMACTotal { get; set; }
        public double TotalAMAPChange { get; set; }
        public double TotalAMACChange { get; set; }
        public double TotalAMPPPercent { get; set; }
        public double TotalAMPCPercent { get; set; }
        public double TotalAMPFPercent { get; set; }

        //total incentive adjusted cost 
        //public double TotalAMINCENT { get; set; }
        public double TotalAMIncentPFTotal { get; set; }
        public double TotalAMIncentPCTotal { get; set; }
        public double TotalAMIncentAPTotal { get; set; }
        public double TotalAMIncentACTotal { get; set; }
        public double TotalAMIncentAPChange { get; set; }
        public double TotalAMIncentACChange { get; set; }
        public double TotalAMIncentPPPercent { get; set; }
        public double TotalAMIncentPCPercent { get; set; }
        public double TotalAMIncentPFPercent { get; set; }

        //net returns
        //public double TotalAMNET { get; set; }
        public double TotalAMNETPFTotal { get; set; }
        public double TotalAMNETPCTotal { get; set; }
        public double TotalAMNETAPTotal { get; set; }
        public double TotalAMNETACTotal { get; set; }
        public double TotalAMNETAPChange { get; set; }
        public double TotalAMNETACChange { get; set; }
        public double TotalAMNETPPPercent { get; set; }
        public double TotalAMNETPCPercent { get; set; }
        public double TotalAMNETPFPercent { get; set; }

        //private const string TAMOC = "TAMOC";
        private const string cTotalAMOCPFTotal = "TAMOCPFTotal";
        private const string cTotalAMOCPCTotal = "TAMOCPCTotal";
        private const string cTotalAMOCAPTotal = "TAMOCAPTotal";
        private const string cTotalAMOCACTotal = "TAMOCACTotal";
        private const string cTotalAMOCAPChange = "TAMOCAPChange";
        private const string cTotalAMOCACChange = "TAMOCACChange";
        private const string cTotalAMOCPPPercent = "TAMOCPPPercent";
        private const string cTotalAMOCPCPercent = "TAMOCPCPercent";
        private const string cTotalAMOCPFPercent = "TAMOCPFPercent";

        //private const string TAMAOH = "TAMAOH";
        private const string cTotalAMAOHPFTotal = "TAMAOHPFTotal";
        private const string cTotalAMAOHPCTotal = "TAMAOHPCTotal";
        private const string cTotalAMAOHAPTotal = "TAMAOHAPTotal";
        private const string cTotalAMAOHACTotal = "TAMAOHACTotal";
        private const string cTotalAMAOHAPChange = "TAMAOHAPChange";
        private const string cTotalAMAOHACChange = "TAMAOHACChange";
        private const string cTotalAMAOHPPPercent = "TAMAOHPPPercent";
        private const string cTotalAMAOHPCPercent = "TAMAOHPCPercent";
        private const string cTotalAMAOHPFPercent = "TAMAOHPFPercent";

        //private const string TAMCAP = "TAMCAP";
        private const string cTotalAMCAPPFTotal = "TAMCAPPFTotal";
        private const string cTotalAMCAPPCTotal = "TAMCAPPCTotal";
        private const string cTotalAMCAPAPTotal = "TAMCAPAPTotal";
        private const string cTotalAMCAPACTotal = "TAMCAPACTotal";
        private const string cTotalAMCAPAPChange = "TAMCAPAPChange";
        private const string cTotalAMCAPACChange = "TAMCAPACChange";
        private const string cTotalAMCAPPPPercent = "TAMCAPPPPercent";
        private const string cTotalAMCAPPCPercent = "TAMCAPPCPercent";
        private const string cTotalAMCAPPFPercent = "TAMCAPPFPercent";

        //private const string TAMTOTAL = "TAMTOTAL";
        private const string cTotalAMPFTotal = "TAMPFTotal";
        private const string cTotalAMPCTotal = "TAMPCTotal";
        private const string cTotalAMAPTotal = "TAMAPTotal";
        private const string cTotalAMACTotal = "TAMACTotal";
        private const string cTotalAMAPChange = "TAMAPChange";
        private const string cTotalAMACChange = "TAMACChange";
        private const string cTotalAMPPPercent = "TAMPPPercent";
        private const string cTotalAMPCPercent = "TAMPCPercent";
        private const string cTotalAMPFPercent = "TAMPFPercent";

        //private const string TAMINCENT = "TAMINCENT";
        private const string cTotalAMIncentPFTotal = "TAMIncentPFTotal";
        private const string cTotalAMIncentPCTotal = "TAMIncentPCTotal";
        private const string cTotalAMIncentAPTotal = "TAMIncentAPTotal";
        private const string cTotalAMIncentACTotal = "TAMIncentACTotal";
        private const string cTotalAMIncentAPChange = "TAMIncentAPChange";
        private const string cTotalAMIncentACChange = "TAMIncentACChange";
        private const string cTotalAMIncentPPPercent = "TAMIncentPPPercent";
        private const string cTotalAMIncentPCPercent = "TAMIncentPCPercent";
        private const string cTotalAMIncentPFPercent = "TAMIncentPFPercent";

        //private const string TAMNET = "TAMNET";
        private const string TAMNETPFTotal = "TAMNETPFTotal";
        private const string TAMNETPCTotal = "TAMNETPCTotal";
        private const string TAMNETAPTotal = "TAMNETAPTotal";
        private const string TAMNETACTotal = "TAMNETACTotal";
        private const string TAMNETAPChange = "TAMNETAPChange";
        private const string TAMNETACChange = "TAMNETACChange";
        private const string TAMNETPPPercent = "TAMNETPPPercent";
        private const string TAMNETPCPercent = "TAMNETPCPercent";
        private const string TAMNETPFPercent = "TAMNETPFPercent";
        //benefits
        //totals, including initbens, salvageval, replacement, and subcosts
        //public double TotalAMR { get; set; }
        public double TotalAMRPFTotal { get; set; }
        public double TotalAMRPCTotal { get; set; }
        public double TotalAMRAPTotal { get; set; }
        public double TotalAMRACTotal { get; set; }
        public double TotalAMRAPChange { get; set; }
        public double TotalAMRACChange { get; set; }
        public double TotalAMRPPPercent { get; set; }
        public double TotalAMRPCPercent { get; set; }
        public double TotalAMRPFPercent { get; set; }
        //output amount
        //public double TotalRAmount { get; set; }
        public double TotalRAmountPFTotal { get; set; }
        public double TotalRAmountPCTotal { get; set; }
        public double TotalRAmountAPTotal { get; set; }
        public double TotalRAmountACTotal { get; set; }
        public double TotalRAmountAPChange { get; set; }
        public double TotalRAmountACChange { get; set; }
        public double TotalRAmountPPPercent { get; set; }
        public double TotalRAmountPCPercent { get; set; }
        public double TotalRAmountPFPercent { get; set; }
        //total incentive adjusted benefits
        //public double TotalAMRINCENT { get; set; }
        public double TotalAMRIncentPFTotal { get; set; }
        public double TotalAMRIncentPCTotal { get; set; }
        public double TotalAMRIncentAPTotal { get; set; }
        public double TotalAMRIncentACTotal { get; set; }
        public double TotalAMRIncentAPChange { get; set; }
        public double TotalAMRIncentACChange { get; set; }
        public double TotalAMRIncentPPPercent { get; set; }
        public double TotalAMRIncentPCPercent { get; set; }
        public double TotalAMRIncentPFPercent { get; set; }
        //new incentive adjusted returns
        //public double TotalRPrice { get; set; }
        public double TotalRPricePFTotal { get; set; }
        public double TotalRPricePCTotal { get; set; }
        public double TotalRPriceAPTotal { get; set; }
        public double TotalRPriceACTotal { get; set; }
        public double TotalRPriceAPChange { get; set; }
        public double TotalRPriceACChange { get; set; }
        public double TotalRPricePPPercent { get; set; }
        public double TotalRPricePCPercent { get; set; }
        public double TotalRPricePFPercent { get; set; }

        //options and salvage value taken from other capital inputs
        //private const string TAMR = "TRBenefit";
        private const string cTotalAMRPFTotal = "TAMRPFTotal";
        private const string cTotalAMRPCTotal = "TAMRPCTotal";
        private const string cTotalAMRAPTotal = "TAMRAPTotal";
        private const string cTotalAMRACTotal = "TAMRACTotal";
        private const string cTotalAMRAPChange = "TAMRAPChange";
        private const string cTotalAMRACChange = "TAMRACChange";
        private const string cTotalAMRPPPercent = "TAMRPPPercent";
        private const string cTotalAMRPCPercent = "TAMRPCPercent";
        private const string cTotalAMRPFPercent = "TAMRPFPercent";

        //private const string TRAmount = "TRAmount";
        private const string cTotalRAmountPFTotal = "TRAmountPFTotal";
        private const string cTotalRAmountPCTotal = "TRAmountPCTotal";
        private const string cTotalRAmountAPTotal = "TRAmountAPTotal";
        private const string cTotalRAmountACTotal = "TRAmountACTotal";
        private const string cTotalRAmountAPChange = "TRAmountAPChange";
        private const string cTotalRAmountACChange = "TRAmountACChange";
        private const string cTotalRAmountPPPercent = "TRAmountPPPercent";
        private const string cTotalRAmountPCPercent = "TRAmountPCPercent";
        private const string cTotalRAmountPFPercent = "TRAmountPFPercent";

        //private const string TAMRINCENT = "TAMRINCENT";
        private const string cTotalAMRIncentPFTotal = "TAMRIncentPFTotal";
        private const string cTotalAMRIncentPCTotal = "TAMRIncentPCTotal";
        private const string cTotalAMRIncentAPTotal = "TAMRIncentAPTotal";
        private const string cTotalAMRIncentACTotal = "TAMRIncentACTotal";
        private const string cTotalAMRIncentAPChange = "TAMRIncentAPChange";
        private const string cTotalAMRIncentACChange = "TAMRIncentACChange";
        private const string cTotalAMRIncentPPPercent = "TAMRIncentPPPercent";
        private const string cTotalAMRIncentPCPercent = "TAMRIncentPCPercent";
        private const string cTotalAMRIncentPFPercent = "TAMRIncentPFPercent";

        //private const string TRPrice = "TRPrice";
        private const string cTotalRPricePFTotal = "TRPricePFTotal";
        private const string cTotalRPricePCTotal = "TRPricePCTotal";
        private const string cTotalRPriceAPTotal = "TRPriceAPTotal";
        private const string cTotalRPriceACTotal = "TRPriceACTotal";
        private const string cTotalRPriceAPChange = "TRPriceAPChange";
        private const string cTotalRPriceACChange = "TRPriceACChange";
        private const string cTotalRPricePPPercent = "TRPricePPPercent";
        private const string cTotalRPricePCPercent = "TRPricePCPercent";
        private const string cTotalRPricePFPercent = "TRPricePFPercent";

        public void InitTotalNPV1Progress1Properties(NPV1Progress1 ind)
        {
            ind.ErrorMessage = string.Empty;
            //includes summary data
            ind.InitTotalBenefitsProperties();
            ind.InitTotalCostsProperties();

            ind.TotalAMOC = 0;
            ind.TotalAMOCPFTotal = 0;
            ind.TotalAMOCPCTotal = 0;
            ind.TotalAMOCAPTotal = 0;
            ind.TotalAMOCACTotal = 0;
            ind.TotalAMOCAPChange = 0;
            ind.TotalAMOCACChange = 0;
            ind.TotalAMOCPPPercent = 0;
            ind.TotalAMOCPCPercent = 0;
            ind.TotalAMOCPFPercent = 0;

            ind.TotalAMAOH = 0;
            ind.TotalAMAOHPFTotal = 0;
            ind.TotalAMAOHPCTotal = 0;
            ind.TotalAMAOHAPTotal = 0;
            ind.TotalAMAOHACTotal = 0;
            ind.TotalAMAOHAPChange = 0;
            ind.TotalAMAOHACChange = 0;
            ind.TotalAMAOHPPPercent = 0;
            ind.TotalAMAOHPCPercent = 0;
            ind.TotalAMAOHPFPercent = 0;

            ind.TotalAMCAP = 0;
            ind.TotalAMCAPPFTotal = 0;
            ind.TotalAMCAPPCTotal = 0;
            ind.TotalAMCAPAPTotal = 0;
            ind.TotalAMCAPACTotal = 0;
            ind.TotalAMCAPAPChange = 0;
            ind.TotalAMCAPACChange = 0;
            ind.TotalAMCAPPPPercent = 0;
            ind.TotalAMCAPPCPercent = 0;
            ind.TotalAMCAPPFPercent = 0;

            ind.TotalAMTOTAL = 0;
            ind.TotalAMPFTotal = 0;
            ind.TotalAMPCTotal = 0;
            ind.TotalAMAPTotal = 0;
            ind.TotalAMACTotal = 0;
            ind.TotalAMAPChange = 0;
            ind.TotalAMACChange = 0;
            ind.TotalAMPPPercent = 0;
            ind.TotalAMPCPercent = 0;
            ind.TotalAMPFPercent = 0;

            ind.TotalAMINCENT = 0;
            ind.TotalAMIncentPFTotal = 0;
            ind.TotalAMIncentPCTotal = 0;
            ind.TotalAMIncentAPTotal = 0;
            ind.TotalAMIncentACTotal = 0;
            ind.TotalAMIncentAPChange = 0;
            ind.TotalAMIncentACChange = 0;
            ind.TotalAMIncentPPPercent = 0;
            ind.TotalAMIncentPCPercent = 0;
            ind.TotalAMIncentPFPercent = 0;

            ind.TotalAMNET = 0;
            ind.TotalAMNETPFTotal = 0;
            ind.TotalAMNETPCTotal = 0;
            ind.TotalAMNETAPTotal = 0;
            ind.TotalAMNETACTotal = 0;
            ind.TotalAMNETAPChange = 0;
            ind.TotalAMNETACChange = 0;
            ind.TotalAMNETPPPercent = 0;
            ind.TotalAMNETPCPercent = 0;
            ind.TotalAMNETPFPercent = 0;

            ind.TotalAMR = 0;
            ind.TotalAMRPFTotal = 0;
            ind.TotalAMRPCTotal = 0;
            ind.TotalAMRAPTotal = 0;
            ind.TotalAMRACTotal = 0;
            ind.TotalAMRAPChange = 0;
            ind.TotalAMRACChange = 0;
            ind.TotalAMRPPPercent = 0;
            ind.TotalAMRPCPercent = 0;
            ind.TotalAMRPFPercent = 0;

            ind.TotalRAmount = 0;
            ind.TotalRAmountPFTotal = 0;
            ind.TotalRAmountPCTotal = 0;
            ind.TotalRAmountAPTotal = 0;
            ind.TotalRAmountACTotal = 0;
            ind.TotalRAmountAPChange = 0;
            ind.TotalRAmountACChange = 0;
            ind.TotalRAmountPPPercent = 0;
            ind.TotalRAmountPCPercent = 0;
            ind.TotalRAmountPFPercent = 0;

            ind.TotalAMRINCENT = 0;
            ind.TotalAMRIncentPFTotal = 0;
            ind.TotalAMRIncentPCTotal = 0;
            ind.TotalAMRIncentAPTotal = 0;
            ind.TotalAMRIncentACTotal = 0;
            ind.TotalAMRIncentAPChange = 0;
            ind.TotalAMRIncentACChange = 0;
            ind.TotalAMRIncentPPPercent = 0;
            ind.TotalAMRIncentPCPercent = 0;
            ind.TotalAMRIncentPFPercent = 0;

            ind.TotalRPrice = 0;
            ind.TotalRPricePFTotal = 0;
            ind.TotalRPricePCTotal = 0;
            ind.TotalRPriceAPTotal = 0;
            ind.TotalRPriceACTotal = 0;
            ind.TotalRPriceAPChange = 0;
            ind.TotalRPriceACChange = 0;
            ind.TotalRPricePPPercent = 0;
            ind.TotalRPricePCPercent = 0;
            ind.TotalRPricePFPercent = 0;
            ind.CalcParameters = new CalculatorParameters();
        }

        public void CopyTotalNPV1Progress1Properties(NPV1Progress1 ind,
            NPV1Progress1 calculator)
        {
            if (calculator != null)
            {
                //inits with standard cb totals
                ind.CopyCalculatorProperties(calculator);
                ind.CopyTotalBenefitsProperties(calculator);
                ind.CopyTotalCostsProperties(calculator);

                ind.ErrorMessage = calculator.ErrorMessage;
                ind.TotalAMOC = calculator.TotalAMOC;
                ind.TotalAMOCPFTotal = calculator.TotalAMOCPFTotal;
                ind.TotalAMOCPCTotal = calculator.TotalAMOCPCTotal;
                ind.TotalAMOCAPTotal = calculator.TotalAMOCAPTotal;
                ind.TotalAMOCACTotal = calculator.TotalAMOCACTotal;
                ind.TotalAMOCAPChange = calculator.TotalAMOCAPChange;
                ind.TotalAMOCACChange = calculator.TotalAMOCACChange;
                ind.TotalAMOCPPPercent = calculator.TotalAMOCPPPercent;
                ind.TotalAMOCPCPercent = calculator.TotalAMOCPCPercent;
                ind.TotalAMOCPFPercent = calculator.TotalAMOCPFPercent;

                ind.TotalAMAOH = calculator.TotalAMAOH;
                ind.TotalAMAOHPFTotal = calculator.TotalAMAOHPFTotal;
                ind.TotalAMAOHPCTotal = calculator.TotalAMAOHPCTotal;
                ind.TotalAMAOHAPTotal = calculator.TotalAMAOHAPTotal;
                ind.TotalAMAOHACTotal = calculator.TotalAMAOHACTotal;
                ind.TotalAMAOHAPChange = calculator.TotalAMAOHAPChange;
                ind.TotalAMAOHACChange = calculator.TotalAMAOHACChange;
                ind.TotalAMAOHPPPercent = calculator.TotalAMAOHPPPercent;
                ind.TotalAMAOHPCPercent = calculator.TotalAMAOHPCPercent;
                ind.TotalAMAOHPFPercent = calculator.TotalAMAOHPFPercent;

                ind.TotalAMCAP = calculator.TotalAMCAP;
                ind.TotalAMCAPPFTotal = calculator.TotalAMCAPPFTotal;
                ind.TotalAMCAPPCTotal = calculator.TotalAMCAPPCTotal;
                ind.TotalAMCAPAPTotal = calculator.TotalAMCAPAPTotal;
                ind.TotalAMCAPACTotal = calculator.TotalAMCAPACTotal;
                ind.TotalAMCAPAPChange = calculator.TotalAMCAPAPChange;
                ind.TotalAMCAPACChange = calculator.TotalAMCAPACChange;
                ind.TotalAMCAPPPPercent = calculator.TotalAMCAPPPPercent;
                ind.TotalAMCAPPCPercent = calculator.TotalAMCAPPCPercent;
                ind.TotalAMCAPPFPercent = calculator.TotalAMCAPPFPercent;

                ind.TotalAMTOTAL = calculator.TotalAMTOTAL;
                ind.TotalAMPFTotal = calculator.TotalAMPFTotal;
                ind.TotalAMPCTotal = calculator.TotalAMPCTotal;
                ind.TotalAMAPTotal = calculator.TotalAMAPTotal;
                ind.TotalAMACTotal = calculator.TotalAMACTotal;
                ind.TotalAMAPChange = calculator.TotalAMAPChange;
                ind.TotalAMACChange = calculator.TotalAMACChange;
                ind.TotalAMPPPercent = calculator.TotalAMPPPercent;
                ind.TotalAMPCPercent = calculator.TotalAMPCPercent;
                ind.TotalAMPFPercent = calculator.TotalAMPFPercent;

                ind.TotalAMINCENT = calculator.TotalAMINCENT;
                ind.TotalAMIncentPFTotal = calculator.TotalAMIncentPFTotal;
                ind.TotalAMIncentPCTotal = calculator.TotalAMIncentPCTotal;
                ind.TotalAMIncentAPTotal = calculator.TotalAMIncentAPTotal;
                ind.TotalAMIncentACTotal = calculator.TotalAMIncentACTotal;
                ind.TotalAMIncentAPChange = calculator.TotalAMIncentAPChange;
                ind.TotalAMIncentACChange = calculator.TotalAMIncentACChange;
                ind.TotalAMIncentPPPercent = calculator.TotalAMIncentPPPercent;
                ind.TotalAMIncentPCPercent = calculator.TotalAMIncentPCPercent;
                ind.TotalAMIncentPFPercent = calculator.TotalAMIncentPFPercent;

                ind.TotalAMNET = calculator.TotalAMNET;
                ind.TotalAMNETPFTotal = calculator.TotalAMNETPFTotal;
                ind.TotalAMNETPCTotal = calculator.TotalAMNETPCTotal;
                ind.TotalAMNETAPTotal = calculator.TotalAMNETAPTotal;
                ind.TotalAMNETACTotal = calculator.TotalAMNETACTotal;
                ind.TotalAMNETAPChange = calculator.TotalAMNETAPChange;
                ind.TotalAMNETACChange = calculator.TotalAMNETACChange;
                ind.TotalAMNETPPPercent = calculator.TotalAMNETPPPercent;
                ind.TotalAMNETPCPercent = calculator.TotalAMNETPCPercent;
                ind.TotalAMNETPFPercent = calculator.TotalAMNETPFPercent;

                ind.TotalAMR = calculator.TotalAMR;
                ind.TotalAMRPFTotal = calculator.TotalAMRPFTotal;
                ind.TotalAMRPCTotal = calculator.TotalAMRPCTotal;
                ind.TotalAMRAPTotal = calculator.TotalAMRAPTotal;
                ind.TotalAMRACTotal = calculator.TotalAMRACTotal;
                ind.TotalAMRAPChange = calculator.TotalAMRAPChange;
                ind.TotalAMRACChange = calculator.TotalAMRACChange;
                ind.TotalAMRPPPercent = calculator.TotalAMRPPPercent;
                ind.TotalAMRPCPercent = calculator.TotalAMRPCPercent;
                ind.TotalAMRPFPercent = calculator.TotalAMRPFPercent;

                ind.TotalRAmount = calculator.TotalRAmount;
                ind.TotalRAmountPFTotal = calculator.TotalRAmountPFTotal;
                ind.TotalRAmountPCTotal = calculator.TotalRAmountPCTotal;
                ind.TotalRAmountAPTotal = calculator.TotalRAmountAPTotal;
                ind.TotalRAmountACTotal = calculator.TotalRAmountACTotal;
                ind.TotalRAmountAPChange = calculator.TotalRAmountAPChange;
                ind.TotalRAmountACChange = calculator.TotalRAmountACChange;
                ind.TotalRAmountPPPercent = calculator.TotalRAmountPPPercent;
                ind.TotalRAmountPCPercent = calculator.TotalRAmountPCPercent;
                ind.TotalRAmountPFPercent = calculator.TotalRAmountPFPercent;

                ind.TotalAMRINCENT = calculator.TotalAMRINCENT;
                ind.TotalAMRIncentPFTotal = calculator.TotalAMRIncentPFTotal;
                ind.TotalAMRIncentPCTotal = calculator.TotalAMRIncentPCTotal;
                ind.TotalAMRIncentAPTotal = calculator.TotalAMRIncentAPTotal;
                ind.TotalAMRIncentACTotal = calculator.TotalAMRIncentACTotal;
                ind.TotalAMRIncentAPChange = calculator.TotalAMRIncentAPChange;
                ind.TotalAMRIncentACChange = calculator.TotalAMRIncentACChange;
                ind.TotalAMRIncentPPPercent = calculator.TotalAMRIncentPPPercent;
                ind.TotalAMRIncentPCPercent = calculator.TotalAMRIncentPCPercent;
                ind.TotalAMRIncentPFPercent = calculator.TotalAMRIncentPFPercent;

                ind.TotalRPrice = calculator.TotalRPrice;
                ind.TotalRPricePFTotal = calculator.TotalRPricePFTotal;
                ind.TotalRPricePCTotal = calculator.TotalRPricePCTotal;
                ind.TotalRPriceAPTotal = calculator.TotalRPriceAPTotal;
                ind.TotalRPriceACTotal = calculator.TotalRPriceACTotal;
                ind.TotalRPriceAPChange = calculator.TotalRPriceAPChange;
                ind.TotalRPriceACChange = calculator.TotalRPriceACChange;
                ind.TotalRPricePPPercent = calculator.TotalRPricePPPercent;
                ind.TotalRPricePCPercent = calculator.TotalRPricePCPercent;
                ind.TotalRPricePFPercent = calculator.TotalRPricePFPercent;
                if (calculator.CalcParameters == null)
                    calculator.CalcParameters = new CalculatorParameters();
                if (ind.CalcParameters == null)
                    ind.CalcParameters = new CalculatorParameters();
                ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            }
        }
        public void CopyTotalNPV1Progress1RProperties(NPV1Progress1 calculator)
        {
            if (calculator != null)
            {
                this.CopyTotalBenefitsPsandQsProperties(calculator);
            }
        }
        public void SetTotalNPV1Progress1Properties(NPV1Progress1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.SetTotalBenefitsSummaryProperties(attNameExtension, calculator);
            ind.SetTotalCostsSummaryProperties(attNameExtension, calculator);

            ind.TotalAMOC = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMOC, attNameExtension));
            ind.TotalAMOCPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCPFTotal, attNameExtension));
            ind.TotalAMOCPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCPCTotal, attNameExtension));
            ind.TotalAMOCAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCAPTotal, attNameExtension));
            ind.TotalAMOCACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCACTotal, attNameExtension));
            ind.TotalAMOCAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCAPChange, attNameExtension));
            ind.TotalAMOCACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCACChange, attNameExtension));
            ind.TotalAMOCPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCPPPercent, attNameExtension));
            ind.TotalAMOCPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCPCPercent, attNameExtension));
            ind.TotalAMOCPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCPFPercent, attNameExtension));

            ind.TotalAMAOH = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMAOH, attNameExtension));
            ind.TotalAMAOHPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHPFTotal, attNameExtension));
            ind.TotalAMAOHPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHPCTotal, attNameExtension));
            ind.TotalAMAOHAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHAPTotal, attNameExtension));
            ind.TotalAMAOHACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHACTotal, attNameExtension));
            ind.TotalAMAOHAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHAPChange, attNameExtension));
            ind.TotalAMAOHACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHACChange, attNameExtension));
            ind.TotalAMAOHPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHPPPercent, attNameExtension));
            ind.TotalAMAOHPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHPCPercent, attNameExtension));
            ind.TotalAMAOHPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHPFPercent, attNameExtension));

            ind.TotalAMCAP = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMCAP, attNameExtension));
            ind.TotalAMCAPPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPPFTotal, attNameExtension));
            ind.TotalAMCAPPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPPCTotal, attNameExtension));
            ind.TotalAMCAPAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPAPTotal, attNameExtension));
            ind.TotalAMCAPACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPACTotal, attNameExtension));
            ind.TotalAMCAPAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPAPChange, attNameExtension));
            ind.TotalAMCAPACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPACChange, attNameExtension));
            ind.TotalAMCAPPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPPPPercent, attNameExtension));
            ind.TotalAMCAPPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPPCPercent, attNameExtension));
            ind.TotalAMCAPPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPPFPercent, attNameExtension));

            ind.TotalAMTOTAL = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMTOTAL, attNameExtension));
            ind.TotalAMPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMPFTotal, attNameExtension));
            ind.TotalAMPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMPCTotal, attNameExtension));
            ind.TotalAMAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAPTotal, attNameExtension));
            ind.TotalAMACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMACTotal, attNameExtension));
            ind.TotalAMAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAPChange, attNameExtension));
            ind.TotalAMACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMACChange, attNameExtension));
            ind.TotalAMPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMPPPercent, attNameExtension));
            ind.TotalAMPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMPCPercent, attNameExtension));
            ind.TotalAMPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMPFPercent, attNameExtension));

            ind.TotalAMINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMINCENT, attNameExtension));
            ind.TotalAMIncentPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentPFTotal, attNameExtension));
            ind.TotalAMIncentPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentPCTotal, attNameExtension));
            ind.TotalAMIncentAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentAPTotal, attNameExtension));
            ind.TotalAMIncentACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentACTotal, attNameExtension));
            ind.TotalAMIncentAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentAPChange, attNameExtension));
            ind.TotalAMIncentACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentACChange, attNameExtension));
            ind.TotalAMIncentPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentPPPercent, attNameExtension));
            ind.TotalAMIncentPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentPCPercent, attNameExtension));
            ind.TotalAMIncentPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentPFPercent, attNameExtension));

            ind.TotalAMNET = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNET, attNameExtension));
            ind.TotalAMNETPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETPFTotal, attNameExtension));
            ind.TotalAMNETPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETPCTotal, attNameExtension));
            ind.TotalAMNETAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETAPTotal, attNameExtension));
            ind.TotalAMNETACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETACTotal, attNameExtension));
            ind.TotalAMNETAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETAPChange, attNameExtension));
            ind.TotalAMNETACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETACChange, attNameExtension));
            ind.TotalAMNETPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETPPPercent, attNameExtension));
            ind.TotalAMNETPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETPCPercent, attNameExtension));
            ind.TotalAMNETPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETPFPercent, attNameExtension));

            ind.TotalAMR = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMR, attNameExtension));
            ind.TotalAMRPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRPFTotal, attNameExtension));
            ind.TotalAMRPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRPCTotal, attNameExtension));
            ind.TotalAMRAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRAPTotal, attNameExtension));
            ind.TotalAMRACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRACTotal, attNameExtension));
            ind.TotalAMRAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRAPChange, attNameExtension));
            ind.TotalAMRACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRACChange, attNameExtension));
            ind.TotalAMRPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRPPPercent, attNameExtension));
            ind.TotalAMRPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRPCPercent, attNameExtension));
            ind.TotalAMRPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRPFPercent, attNameExtension));

            ind.TotalRAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TRAmount, attNameExtension));
            ind.TotalRAmountPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountPFTotal, attNameExtension));
            ind.TotalRAmountPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountPCTotal, attNameExtension));
            ind.TotalRAmountAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountAPTotal, attNameExtension));
            ind.TotalRAmountACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountACTotal, attNameExtension));
            ind.TotalRAmountAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountAPChange, attNameExtension));
            ind.TotalRAmountACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountACChange, attNameExtension));
            ind.TotalRAmountPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountPPPercent, attNameExtension));
            ind.TotalRAmountPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountPCPercent, attNameExtension));
            ind.TotalRAmountPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountPFPercent, attNameExtension));

            ind.TotalAMRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMRINCENT, attNameExtension));
            ind.TotalAMRIncentPFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentPFTotal, attNameExtension));
            ind.TotalAMRIncentPCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentPCTotal, attNameExtension));
            ind.TotalAMRIncentAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentAPTotal, attNameExtension));
            ind.TotalAMRIncentACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentACTotal, attNameExtension));
            ind.TotalAMRIncentAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentAPChange, attNameExtension));
            ind.TotalAMRIncentACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentACChange, attNameExtension));
            ind.TotalAMRIncentPPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentPPPercent, attNameExtension));
            ind.TotalAMRIncentPCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentPCPercent, attNameExtension));
            ind.TotalAMRIncentPFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentPFPercent, attNameExtension));

            ind.TotalRPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TRPrice, attNameExtension));
            ind.TotalRPricePFTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPricePFTotal, attNameExtension));
            ind.TotalRPricePCTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPricePCTotal, attNameExtension));
            ind.TotalRPriceAPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceAPTotal, attNameExtension));
            ind.TotalRPriceACTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceACTotal, attNameExtension));
            ind.TotalRPriceAPChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceAPChange, attNameExtension));
            ind.TotalRPriceACChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceACChange, attNameExtension));
            ind.TotalRPricePPPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPricePPPercent, attNameExtension));
            ind.TotalRPricePCPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPricePCPercent, attNameExtension));
            ind.TotalRPricePFPercent = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPricePFPercent, attNameExtension));
        }
        public void SetTotalNPV1Progress1Property(NPV1Progress1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case TAMOC:
                    ind.TotalAMOC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCPFTotal:
                    ind.TotalAMOCPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCPCTotal:
                    ind.TotalAMOCPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCAPTotal:
                    ind.TotalAMOCAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCACTotal:
                    ind.TotalAMOCACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCAPChange:
                    ind.TotalAMOCAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCACChange:
                    ind.TotalAMOCACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCPPPercent:
                    ind.TotalAMOCPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCPCPercent:
                    ind.TotalAMOCPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCPFPercent:
                    ind.TotalAMOCPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH:
                    ind.TotalAMAOH = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHPFTotal:
                    ind.TotalAMAOHPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHPCTotal:
                    ind.TotalAMAOHPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHAPTotal:
                    ind.TotalAMAOHAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHACTotal:
                    ind.TotalAMAOHACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHAPChange:
                    ind.TotalAMAOHAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHACChange:
                    ind.TotalAMAOHACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHPPPercent:
                    ind.TotalAMAOHPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHPCPercent:
                    ind.TotalAMAOHPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHPFPercent:
                    ind.TotalAMAOHPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP:
                    ind.TotalAMCAP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPPFTotal:
                    ind.TotalAMCAPPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPPCTotal:
                    ind.TotalAMCAPPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPAPTotal:
                    ind.TotalAMCAPAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPACTotal:
                    ind.TotalAMCAPACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPAPChange:
                    ind.TotalAMCAPAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPACChange:
                    ind.TotalAMCAPACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPPPPercent:
                    ind.TotalAMCAPPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPPCPercent:
                    ind.TotalAMCAPPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPPFPercent:
                    ind.TotalAMCAPPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL:
                    ind.TotalAMTOTAL = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMPFTotal:
                    ind.TotalAMPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMPCTotal:
                    ind.TotalAMPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAPTotal:
                    ind.TotalAMAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMACTotal:
                    ind.TotalAMACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAPChange:
                    ind.TotalAMAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMACChange:
                    ind.TotalAMACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMPPPercent:
                    ind.TotalAMPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMPCPercent:
                    ind.TotalAMPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMPFPercent:
                    ind.TotalAMPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT:
                    ind.TotalAMINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentPFTotal:
                    ind.TotalAMIncentPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentPCTotal:
                    ind.TotalAMIncentPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentAPTotal:
                    ind.TotalAMIncentAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentACTotal:
                    ind.TotalAMIncentACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentAPChange:
                    ind.TotalAMIncentAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentACChange:
                    sPropertyValue = ind.TotalAMIncentACChange.ToString();
                    break;
                case cTotalAMIncentPPPercent:
                    sPropertyValue = ind.TotalAMIncentPPPercent.ToString();
                    break;
                case cTotalAMIncentPCPercent:
                    sPropertyValue = ind.TotalAMIncentPCPercent.ToString();
                    break;
                case cTotalAMIncentPFPercent:
                    sPropertyValue = ind.TotalAMIncentPFPercent.ToString();
                    break;
                case TAMNET:
                    ind.TotalAMNET = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETPFTotal:
                    ind.TotalAMNETPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETPCTotal:
                    ind.TotalAMNETPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETAPTotal:
                    ind.TotalAMNETAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETACTotal:
                    ind.TotalAMNETACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETAPChange:
                    ind.TotalAMNETAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETACChange:
                    ind.TotalAMNETACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETPPPercent:
                    ind.TotalAMNETPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETPCPercent:
                    ind.TotalAMNETPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETPFPercent:
                    ind.TotalAMNETPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR:
                    ind.TotalAMR = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRPFTotal:
                    ind.TotalAMRPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRPCTotal:
                    ind.TotalAMRPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRAPTotal:
                    ind.TotalAMRAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRACTotal:
                    ind.TotalAMRACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRAPChange:
                    ind.TotalAMRAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRACChange:
                    ind.TotalAMRACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRPPPercent:
                    ind.TotalAMRPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRPCPercent:
                    ind.TotalAMRPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRPFPercent:
                    ind.TotalAMRPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRAmount:
                    ind.TotalRAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountPFTotal:
                    ind.TotalRAmountPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountPCTotal:
                    ind.TotalRAmountPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountAPTotal:
                    ind.TotalRAmountAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountACTotal:
                    ind.TotalRAmountACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountAPChange:
                    ind.TotalRAmountAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountACChange:
                    ind.TotalRAmountACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountPPPercent:
                    ind.TotalRAmountPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountPCPercent:
                    ind.TotalRAmountPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountPFPercent:
                    ind.TotalRAmountPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT:
                    ind.TotalAMRINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentPFTotal:
                    ind.TotalAMRIncentPFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentPCTotal:
                    ind.TotalAMRIncentPCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentAPTotal:
                    ind.TotalAMRIncentAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentACTotal:
                    ind.TotalAMRIncentACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentAPChange:
                    ind.TotalAMRIncentAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentACChange:
                    ind.TotalAMRIncentACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentPPPercent:
                    ind.TotalAMRIncentPPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentPCPercent:
                    ind.TotalAMRIncentPCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentPFPercent:
                    ind.TotalAMRIncentPFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice:
                    ind.TotalRPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPricePFTotal:
                    ind.TotalRPricePFTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPricePCTotal:
                    ind.TotalRPricePCTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceAPTotal:
                    ind.TotalRPriceAPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceACTotal:
                    ind.TotalRPriceACTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceAPChange:
                    ind.TotalRPriceAPChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceACChange:
                    ind.TotalRPriceACChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPricePPPercent:
                    ind.TotalRPricePPPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPricePCPercent:
                    ind.TotalRPricePCPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPricePFPercent:
                    ind.TotalRPricePFPercent = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalNPV1Progress1Property(NPV1Progress1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case TAMOC:
                    sPropertyValue = ind.TotalAMOC.ToString();
                    break;
                case cTotalAMOCPFTotal:
                    sPropertyValue = ind.TotalAMOCPFTotal.ToString();
                    break;
                case cTotalAMOCPCTotal:
                    sPropertyValue = ind.TotalAMOCPCTotal.ToString();
                    break;
                case cTotalAMOCAPTotal:
                    sPropertyValue = ind.TotalAMOCAPTotal.ToString();
                    break;
                case cTotalAMOCACTotal:
                    sPropertyValue = ind.TotalAMOCACTotal.ToString();
                    break;
                case cTotalAMOCAPChange:
                    sPropertyValue = ind.TotalAMOCAPChange.ToString();
                    break;
                case cTotalAMOCACChange:
                    sPropertyValue = ind.TotalAMOCACChange.ToString();
                    break;
                case cTotalAMOCPPPercent:
                    sPropertyValue = ind.TotalAMOCPPPercent.ToString();
                    break;
                case cTotalAMOCPCPercent:
                    sPropertyValue = ind.TotalAMOCPCPercent.ToString();
                    break;
                case cTotalAMOCPFPercent:
                    sPropertyValue = ind.TotalAMOCPFPercent.ToString();
                    break;
                case TAMAOH:
                    sPropertyValue = ind.TotalAMAOH.ToString();
                    break;
                case cTotalAMAOHPFTotal:
                    sPropertyValue = ind.TotalAMAOHPFTotal.ToString();
                    break;
                case cTotalAMAOHPCTotal:
                    sPropertyValue = ind.TotalAMAOHPCTotal.ToString();
                    break;
                case cTotalAMAOHAPTotal:
                    sPropertyValue = ind.TotalAMAOHAPTotal.ToString();
                    break;
                case cTotalAMAOHACTotal:
                    sPropertyValue = ind.TotalAMAOHACTotal.ToString();
                    break;
                case cTotalAMAOHAPChange:
                    sPropertyValue = ind.TotalAMAOHAPChange.ToString();
                    break;
                case cTotalAMAOHACChange:
                    sPropertyValue = ind.TotalAMAOHACChange.ToString();
                    break;
                case cTotalAMAOHPPPercent:
                    sPropertyValue = ind.TotalAMAOHPPPercent.ToString();
                    break;
                case cTotalAMAOHPCPercent:
                    sPropertyValue = ind.TotalAMAOHPCPercent.ToString();
                    break;
                case cTotalAMAOHPFPercent:
                    sPropertyValue = ind.TotalAMAOHPFPercent.ToString();
                    break;
                case TAMCAP:
                    sPropertyValue = ind.TotalAMCAP.ToString();
                    break;
                case cTotalAMCAPPFTotal:
                    sPropertyValue = ind.TotalAMCAPPFTotal.ToString();
                    break;
                case cTotalAMCAPPCTotal:
                    sPropertyValue = ind.TotalAMCAPPCTotal.ToString();
                    break;
                case cTotalAMCAPAPTotal:
                    sPropertyValue = ind.TotalAMCAPAPTotal.ToString();
                    break;
                case cTotalAMCAPACTotal:
                    sPropertyValue = ind.TotalAMCAPACTotal.ToString();
                    break;
                case cTotalAMCAPAPChange:
                    sPropertyValue = ind.TotalAMCAPAPChange.ToString();
                    break;
                case cTotalAMCAPACChange:
                    sPropertyValue = ind.TotalAMCAPACChange.ToString();
                    break;
                case cTotalAMCAPPPPercent:
                    sPropertyValue = ind.TotalAMCAPPPPercent.ToString();
                    break;
                case cTotalAMCAPPCPercent:
                    sPropertyValue = ind.TotalAMCAPPCPercent.ToString();
                    break;
                case cTotalAMCAPPFPercent:
                    sPropertyValue = ind.TotalAMCAPPFPercent.ToString();
                    break;
                case TAMTOTAL:
                    sPropertyValue = ind.TotalAMTOTAL.ToString();
                    break;
                case cTotalAMPFTotal:
                    sPropertyValue = ind.TotalAMPFTotal.ToString();
                    break;
                case cTotalAMPCTotal:
                    sPropertyValue = ind.TotalAMPCTotal.ToString();
                    break;
                case cTotalAMAPTotal:
                    sPropertyValue = ind.TotalAMAPTotal.ToString();
                    break;
                case cTotalAMACTotal:
                    sPropertyValue = ind.TotalAMACTotal.ToString();
                    break;
                case cTotalAMAPChange:
                    sPropertyValue = ind.TotalAMAPChange.ToString();
                    break;
                case cTotalAMACChange:
                    sPropertyValue = ind.TotalAMACChange.ToString();
                    break;
                case cTotalAMPPPercent:
                    sPropertyValue = ind.TotalAMPPPercent.ToString();
                    break;
                case cTotalAMPCPercent:
                    sPropertyValue = ind.TotalAMPCPercent.ToString();
                    break;
                case cTotalAMPFPercent:
                    sPropertyValue = ind.TotalAMPFPercent.ToString();
                    break;
                case TAMINCENT:
                    sPropertyValue = ind.TotalAMINCENT.ToString();
                    break;
                case cTotalAMIncentPFTotal:
                    sPropertyValue = ind.TotalAMIncentPFTotal.ToString();
                    break;
                case cTotalAMIncentPCTotal:
                    sPropertyValue = ind.TotalAMIncentPCTotal.ToString();
                    break;
                case cTotalAMIncentAPTotal:
                    sPropertyValue = ind.TotalAMIncentAPTotal.ToString();
                    break;
                case cTotalAMIncentACTotal:
                    sPropertyValue = ind.TotalAMIncentACTotal.ToString();
                    break;
                case cTotalAMIncentAPChange:
                    sPropertyValue = ind.TotalAMIncentAPChange.ToString();
                    break;
                case cTotalAMIncentACChange:
                    sPropertyValue = ind.TotalAMIncentACChange.ToString();
                    break;
                case cTotalAMIncentPPPercent:
                    sPropertyValue = ind.TotalAMIncentPPPercent.ToString();
                    break;
                case cTotalAMIncentPCPercent:
                    sPropertyValue = ind.TotalAMIncentPCPercent.ToString();
                    break;
                case cTotalAMIncentPFPercent:
                    sPropertyValue = ind.TotalAMIncentPFPercent.ToString();
                    break;
                case TAMNET:
                    sPropertyValue = ind.TotalAMNET.ToString();
                    break;
                case TAMNETPFTotal:
                    sPropertyValue = ind.TotalAMNETPFTotal.ToString();
                    break;
                case TAMNETPCTotal:
                    sPropertyValue = ind.TotalAMNETPCTotal.ToString();
                    break;
                case TAMNETAPTotal:
                    sPropertyValue = ind.TotalAMNETAPTotal.ToString();
                    break;
                case TAMNETACTotal:
                    sPropertyValue = ind.TotalAMNETACTotal.ToString();
                    break;
                case TAMNETAPChange:
                    sPropertyValue = ind.TotalAMNETAPChange.ToString();
                    break;
                case TAMNETACChange:
                    sPropertyValue = ind.TotalAMNETACChange.ToString();
                    break;
                case TAMNETPPPercent:
                    sPropertyValue = ind.TotalAMNETPPPercent.ToString();
                    break;
                case TAMNETPCPercent:
                    sPropertyValue = ind.TotalAMNETPCPercent.ToString();
                    break;
                case TAMNETPFPercent:
                    sPropertyValue = ind.TotalAMNETPFPercent.ToString();
                    break;
                case TAMR:
                    sPropertyValue = ind.TotalAMR.ToString();
                    break;
                case cTotalAMRPFTotal:
                    sPropertyValue = ind.TotalAMRPFTotal.ToString();
                    break;
                case cTotalAMRPCTotal:
                    sPropertyValue = ind.TotalAMRPCTotal.ToString();
                    break;
                case cTotalAMRAPTotal:
                    sPropertyValue = ind.TotalAMRAPTotal.ToString();
                    break;
                case cTotalAMRACTotal:
                    sPropertyValue = ind.TotalAMRACTotal.ToString();
                    break;
                case cTotalAMRAPChange:
                    sPropertyValue = ind.TotalAMRAPChange.ToString();
                    break;
                case cTotalAMRACChange:
                    sPropertyValue = ind.TotalAMRACChange.ToString();
                    break;
                case cTotalAMRPPPercent:
                    sPropertyValue = ind.TotalAMRPPPercent.ToString();
                    break;
                case cTotalAMRPCPercent:
                    sPropertyValue = ind.TotalAMRPCPercent.ToString();
                    break;
                case cTotalAMRPFPercent:
                    sPropertyValue = ind.TotalAMRPFPercent.ToString();
                    break;
                case TRAmount:
                    sPropertyValue = ind.TotalRAmount.ToString();
                    break;
                case cTotalRAmountPFTotal:
                    sPropertyValue = ind.TotalRAmountPFTotal.ToString();
                    break;
                case cTotalRAmountPCTotal:
                    sPropertyValue = ind.TotalRAmountPCTotal.ToString();
                    break;
                case cTotalRAmountAPTotal:
                    sPropertyValue = ind.TotalRAmountAPTotal.ToString();
                    break;
                case cTotalRAmountACTotal:
                    sPropertyValue = ind.TotalRAmountACTotal.ToString();
                    break;
                case cTotalRAmountAPChange:
                    sPropertyValue = ind.TotalRAmountAPChange.ToString();
                    break;
                case cTotalRAmountACChange:
                    sPropertyValue = ind.TotalRAmountACChange.ToString();
                    break;
                case cTotalRAmountPPPercent:
                    sPropertyValue = ind.TotalRAmountPPPercent.ToString();
                    break;
                case cTotalRAmountPCPercent:
                    sPropertyValue = ind.TotalRAmountPCPercent.ToString();
                    break;
                case cTotalRAmountPFPercent:
                    sPropertyValue = ind.TotalRAmountPFPercent.ToString();
                    break;
                case TAMRINCENT:
                    sPropertyValue = ind.TotalAMRINCENT.ToString();
                    break;
                case cTotalAMRIncentPFTotal:
                    sPropertyValue = ind.TotalAMRIncentPFTotal.ToString();
                    break;
                case cTotalAMRIncentPCTotal:
                    sPropertyValue = ind.TotalAMRIncentPCTotal.ToString();
                    break;
                case cTotalAMRIncentAPTotal:
                    sPropertyValue = ind.TotalAMRIncentAPTotal.ToString();
                    break;
                case cTotalAMRIncentACTotal:
                    sPropertyValue = ind.TotalAMRIncentACTotal.ToString();
                    break;
                case cTotalAMRIncentAPChange:
                    sPropertyValue = ind.TotalAMRIncentAPChange.ToString();
                    break;
                case cTotalAMRIncentACChange:
                    sPropertyValue = ind.TotalAMRIncentACChange.ToString();
                    break;
                case cTotalAMRIncentPPPercent:
                    sPropertyValue = ind.TotalAMRIncentPPPercent.ToString();
                    break;
                case cTotalAMRIncentPCPercent:
                    sPropertyValue = ind.TotalAMRIncentPCPercent.ToString();
                    break;
                case cTotalAMRIncentPFPercent:
                    sPropertyValue = ind.TotalAMRIncentPFPercent.ToString();
                    break;
                case TRPrice:
                    sPropertyValue = ind.TotalRPrice.ToString();
                    break;
                case cTotalRPricePFTotal:
                    sPropertyValue = ind.TotalRPricePFTotal.ToString();
                    break;
                case cTotalRPricePCTotal:
                    sPropertyValue = ind.TotalRPricePCTotal.ToString();
                    break;
                case cTotalRPriceAPTotal:
                    sPropertyValue = ind.TotalRPriceAPTotal.ToString();
                    break;
                case cTotalRPriceACTotal:
                    sPropertyValue = ind.TotalRPriceACTotal.ToString();
                    break;
                case cTotalRPriceAPChange:
                    sPropertyValue = ind.TotalRPriceAPChange.ToString();
                    break;
                case cTotalRPriceACChange:
                    sPropertyValue = ind.TotalRPriceACChange.ToString();
                    break;
                case cTotalRPricePPPercent:
                    sPropertyValue = ind.TotalRPricePPPercent.ToString();
                    break;
                case cTotalRPricePCPercent:
                    sPropertyValue = ind.TotalRPricePCPercent.ToString();
                    break;
                case cTotalRPricePFPercent:
                    sPropertyValue = ind.TotalRPricePFPercent.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalNPV1Progress1Attributes(NPV1Progress1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsTotalNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsTotalNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMOC, attNameExtension), ind.TotalAMOC);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCPFTotal, attNameExtension), ind.TotalAMOCPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCPCTotal, attNameExtension), ind.TotalAMOCPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCAPTotal, attNameExtension), ind.TotalAMOCAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCACTotal, attNameExtension), ind.TotalAMOCACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCAPChange, attNameExtension), ind.TotalAMOCAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMOCACChange, attNameExtension), ind.TotalAMOCACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMOCPPPercent, attNameExtension), ind.TotalAMOCPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMOCPCPercent, attNameExtension), ind.TotalAMOCPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMOCPFPercent, attNameExtension), ind.TotalAMOCPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMAOH, attNameExtension), ind.TotalAMAOH);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHPFTotal, attNameExtension), ind.TotalAMAOHPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHPCTotal, attNameExtension), ind.TotalAMAOHPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHAPTotal, attNameExtension), ind.TotalAMAOHAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHACTotal, attNameExtension), ind.TotalAMAOHACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHAPChange, attNameExtension), ind.TotalAMAOHAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMAOHACChange, attNameExtension), ind.TotalAMAOHACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMAOHPPPercent, attNameExtension), ind.TotalAMAOHPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMAOHPCPercent, attNameExtension), ind.TotalAMAOHPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMAOHPFPercent, attNameExtension), ind.TotalAMAOHPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMCAP, attNameExtension), ind.TotalAMCAP);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPPFTotal, attNameExtension), ind.TotalAMCAPPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPPCTotal, attNameExtension), ind.TotalAMCAPPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPAPTotal, attNameExtension), ind.TotalAMCAPAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPACTotal, attNameExtension), ind.TotalAMCAPACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPAPChange, attNameExtension), ind.TotalAMCAPAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMCAPACChange, attNameExtension), ind.TotalAMCAPACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMCAPPPPercent, attNameExtension), ind.TotalAMCAPPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMCAPPCPercent, attNameExtension), ind.TotalAMCAPPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMCAPPFPercent, attNameExtension), ind.TotalAMCAPPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMTOTAL, attNameExtension), ind.TotalAMTOTAL);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMPFTotal, attNameExtension), ind.TotalAMPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMPCTotal, attNameExtension), ind.TotalAMPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAPTotal, attNameExtension), ind.TotalAMAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMACTotal, attNameExtension), ind.TotalAMACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAPChange, attNameExtension), ind.TotalAMAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMACChange, attNameExtension), ind.TotalAMACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMPPPercent, attNameExtension), ind.TotalAMPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMPCPercent, attNameExtension), ind.TotalAMPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMPFPercent, attNameExtension), ind.TotalAMPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMINCENT, attNameExtension), ind.TotalAMINCENT);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentPFTotal, attNameExtension), ind.TotalAMIncentPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentPCTotal, attNameExtension), ind.TotalAMIncentPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentAPTotal, attNameExtension), ind.TotalAMIncentAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentACTotal, attNameExtension), ind.TotalAMIncentACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentAPChange, attNameExtension), ind.TotalAMIncentAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMIncentACChange, attNameExtension), ind.TotalAMIncentACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMIncentPPPercent, attNameExtension), ind.TotalAMIncentPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMIncentPCPercent, attNameExtension), ind.TotalAMIncentPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMIncentPFPercent, attNameExtension), ind.TotalAMIncentPFPercent);
            }
            if (bIsTotalNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMR, attNameExtension), ind.TotalAMR);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRPFTotal, attNameExtension), ind.TotalAMRPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRPCTotal, attNameExtension), ind.TotalAMRPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRAPTotal, attNameExtension), ind.TotalAMRAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRACTotal, attNameExtension), ind.TotalAMRACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRAPChange, attNameExtension), ind.TotalAMRAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRACChange, attNameExtension), ind.TotalAMRACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRPPPercent, attNameExtension), ind.TotalAMRPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRPCPercent, attNameExtension), ind.TotalAMRPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRPFPercent, attNameExtension), ind.TotalAMRPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TRAmount, attNameExtension), ind.TotalRAmount);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountPFTotal, attNameExtension), ind.TotalRAmountPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountPCTotal, attNameExtension), ind.TotalRAmountPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountAPTotal, attNameExtension), ind.TotalRAmountAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountACTotal, attNameExtension), ind.TotalRAmountACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountAPChange, attNameExtension), ind.TotalRAmountAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRAmountACChange, attNameExtension), ind.TotalRAmountACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRAmountPPPercent, attNameExtension), ind.TotalRAmountPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRAmountPCPercent, attNameExtension), ind.TotalRAmountPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRAmountPFPercent, attNameExtension), ind.TotalRAmountPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMRINCENT, attNameExtension), ind.TotalAMRINCENT);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentPFTotal, attNameExtension), ind.TotalAMRIncentPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentPCTotal, attNameExtension), ind.TotalAMRIncentPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentAPTotal, attNameExtension), ind.TotalAMRIncentAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentACTotal, attNameExtension), ind.TotalAMRIncentACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentAPChange, attNameExtension), ind.TotalAMRIncentAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRIncentACChange, attNameExtension), ind.TotalAMRIncentACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRIncentPPPercent, attNameExtension), ind.TotalAMRIncentPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRIncentPCPercent, attNameExtension), ind.TotalAMRIncentPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalAMRIncentPFPercent, attNameExtension), ind.TotalAMRIncentPFPercent);
            }
            if (bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNET, attNameExtension), ind.TotalAMNET);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETPFTotal, attNameExtension), ind.TotalAMNETPFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETPCTotal, attNameExtension), ind.TotalAMNETPCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETAPTotal, attNameExtension), ind.TotalAMNETAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETACTotal, attNameExtension), ind.TotalAMNETACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(TAMNETAPChange, attNameExtension), ind.TotalAMNETAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(TAMNETACChange, attNameExtension), ind.TotalAMNETACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(TAMNETPPPercent, attNameExtension), ind.TotalAMNETPPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(TAMNETPCPercent, attNameExtension), ind.TotalAMNETPCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(TAMNETPFPercent, attNameExtension), ind.TotalAMNETPFPercent);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TRPrice, attNameExtension), ind.TotalRPrice);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPricePFTotal, attNameExtension), ind.TotalRPricePFTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPricePCTotal, attNameExtension), ind.TotalRPricePCTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPriceAPTotal, attNameExtension), ind.TotalRPriceAPTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPriceACTotal, attNameExtension), ind.TotalRPriceACTotal);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPriceAPChange, attNameExtension), ind.TotalRPriceAPChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPriceACChange, attNameExtension), ind.TotalRPriceACChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPricePPPercent, attNameExtension), ind.TotalRPricePPPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPricePCPercent, attNameExtension), ind.TotalRPricePCPercent);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                       string.Concat(cTotalRPricePFPercent, attNameExtension), ind.TotalRPricePFPercent);
            }
        }

        public void SetTotalNPV1Progress1Attributes(NPV1Progress1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsTotalNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsTotalNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(TAMOC, attNameExtension), ind.TotalAMOC.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCPFTotal, attNameExtension), ind.TotalAMOCPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCPCTotal, attNameExtension), ind.TotalAMOCPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCAPTotal, attNameExtension), ind.TotalAMOCAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCACTotal, attNameExtension), ind.TotalAMOCACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCAPChange, attNameExtension), ind.TotalAMOCAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMOCACChange, attNameExtension), ind.TotalAMOCACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMOCPPPercent, attNameExtension), ind.TotalAMOCPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMOCPCPercent, attNameExtension), ind.TotalAMOCPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMOCPFPercent, attNameExtension), ind.TotalAMOCPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMAOH, attNameExtension), ind.TotalAMAOH.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHPFTotal, attNameExtension), ind.TotalAMAOHPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHPCTotal, attNameExtension), ind.TotalAMAOHPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHAPTotal, attNameExtension), ind.TotalAMAOHAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHACTotal, attNameExtension), ind.TotalAMAOHACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHAPChange, attNameExtension), ind.TotalAMAOHAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMAOHACChange, attNameExtension), ind.TotalAMAOHACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMAOHPPPercent, attNameExtension), ind.TotalAMAOHPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMAOHPCPercent, attNameExtension), ind.TotalAMAOHPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMAOHPFPercent, attNameExtension), ind.TotalAMAOHPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMCAP, attNameExtension), ind.TotalAMCAP.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPPFTotal, attNameExtension), ind.TotalAMCAPPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPPCTotal, attNameExtension), ind.TotalAMCAPPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPAPTotal, attNameExtension), ind.TotalAMCAPAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPACTotal, attNameExtension), ind.TotalAMCAPACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPAPChange, attNameExtension), ind.TotalAMCAPAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMCAPACChange, attNameExtension), ind.TotalAMCAPACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMCAPPPPercent, attNameExtension), ind.TotalAMCAPPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMCAPPCPercent, attNameExtension), ind.TotalAMCAPPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMCAPPFPercent, attNameExtension), ind.TotalAMCAPPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                   string.Concat(TAMTOTAL, attNameExtension), ind.TotalAMTOTAL.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMPFTotal, attNameExtension), ind.TotalAMPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMPCTotal, attNameExtension), ind.TotalAMPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAPTotal, attNameExtension), ind.TotalAMAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMACTotal, attNameExtension), ind.TotalAMACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMAPChange, attNameExtension), ind.TotalAMAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMACChange, attNameExtension), ind.TotalAMACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMPPPercent, attNameExtension), ind.TotalAMPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMPCPercent, attNameExtension), ind.TotalAMPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMPFPercent, attNameExtension), ind.TotalAMPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMINCENT, attNameExtension), ind.TotalAMINCENT.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentPFTotal, attNameExtension), ind.TotalAMIncentPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentPCTotal, attNameExtension), ind.TotalAMIncentPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentAPTotal, attNameExtension), ind.TotalAMIncentAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentACTotal, attNameExtension), ind.TotalAMIncentACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentAPChange, attNameExtension), ind.TotalAMIncentAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMIncentACChange, attNameExtension), ind.TotalAMIncentACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMIncentPPPercent, attNameExtension), ind.TotalAMIncentPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMIncentPCPercent, attNameExtension), ind.TotalAMIncentPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMIncentPFPercent, attNameExtension), ind.TotalAMIncentPFPercent.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsTotalNode || bIsBoth)
            {

                writer.WriteAttributeString(
                    string.Concat(TAMR, attNameExtension), ind.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRPFTotal, attNameExtension), ind.TotalAMRPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRPCTotal, attNameExtension), ind.TotalAMRPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRAPTotal, attNameExtension), ind.TotalAMRAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRACTotal, attNameExtension), ind.TotalAMRACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRAPChange, attNameExtension), ind.TotalAMRAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRACChange, attNameExtension), ind.TotalAMRACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRPPPercent, attNameExtension), ind.TotalAMRPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRPCPercent, attNameExtension), ind.TotalAMRPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRPFPercent, attNameExtension), ind.TotalAMRPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TRAmount, attNameExtension), ind.TotalRAmount.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountPFTotal, attNameExtension), ind.TotalRAmountPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountPCTotal, attNameExtension), ind.TotalRAmountPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountAPTotal, attNameExtension), ind.TotalRAmountAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountACTotal, attNameExtension), ind.TotalRAmountACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountAPChange, attNameExtension), ind.TotalRAmountAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRAmountACChange, attNameExtension), ind.TotalRAmountACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRAmountPPPercent, attNameExtension), ind.TotalRAmountPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRAmountPCPercent, attNameExtension), ind.TotalRAmountPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRAmountPFPercent, attNameExtension), ind.TotalRAmountPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMRINCENT, attNameExtension), ind.TotalAMRINCENT.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentPFTotal, attNameExtension), ind.TotalAMRIncentPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentPCTotal, attNameExtension), ind.TotalAMRIncentPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentAPTotal, attNameExtension), ind.TotalAMRIncentAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentACTotal, attNameExtension), ind.TotalAMRIncentACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentAPChange, attNameExtension), ind.TotalAMRIncentAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRIncentACChange, attNameExtension), ind.TotalAMRIncentACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRIncentPPPercent, attNameExtension), ind.TotalAMRIncentPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRIncentPCPercent, attNameExtension), ind.TotalAMRIncentPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalAMRIncentPFPercent, attNameExtension), ind.TotalAMRIncentPFPercent.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TRPrice, attNameExtension), ind.TotalRPrice.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPricePFTotal, attNameExtension), ind.TotalRPricePFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPricePCTotal, attNameExtension), ind.TotalRPricePCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPriceAPTotal, attNameExtension), ind.TotalRPriceAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPriceACTotal, attNameExtension), ind.TotalRPriceACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPriceAPChange, attNameExtension), ind.TotalRPriceAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPriceACChange, attNameExtension), ind.TotalRPriceACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPricePPPercent, attNameExtension), ind.TotalRPricePPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPricePCPercent, attNameExtension), ind.TotalRPricePCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(cTotalRPricePFPercent, attNameExtension), ind.TotalRPricePFPercent.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(TAMNET, attNameExtension), ind.TotalAMNET.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETPFTotal, attNameExtension), ind.TotalAMNETPFTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETPCTotal, attNameExtension), ind.TotalAMNETPCTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETAPTotal, attNameExtension), ind.TotalAMNETAPTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETACTotal, attNameExtension), ind.TotalAMNETACTotal.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETAPChange, attNameExtension), ind.TotalAMNETAPChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(TAMNETACChange, attNameExtension), ind.TotalAMNETACChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(TAMNETPPPercent, attNameExtension), ind.TotalAMNETPPPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(TAMNETPCPercent, attNameExtension), ind.TotalAMNETPCPercent.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                       string.Concat(TAMNETPFPercent, attNameExtension), ind.TotalAMNETPFPercent.ToString("N2", CultureInfo.InvariantCulture));
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
        //calcs holds the collections needing progress analysis
        public bool RunAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //don't use npv1Stock totals, use calcs
            SetAnalyses(npv1Stock);
            //set calculated changestocks
            List<NPV1Stock> progressStocks = new List<NPV1Stock>();
            if (npv1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || npv1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                progressStocks = SetIOAnalyses(npv1Stock, calcs);
            }
            else
            {
                if (npv1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || npv1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no progress
                    progressStocks = SetTotals(npv1Stock, calcs);
                }
                else
                {
                    progressStocks = SetAnalyses(npv1Stock, calcs);
                }
            }
            //add the progresstocks to parent stock
            if (progressStocks != null)
            {
                bHasAnalyses = AddProgsToBaseStock(npv1Stock, progressStocks);
                //npv1Stock must still add the members of progress1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        private bool SetAnalyses(NPV1Stock npv1Stock)
        {
            bool bHasAnalysis = false;
            npv1Stock.Progress1.CalcParameters = npv1Stock.CalcParameters;
            //totals were added to npv1stock, but those totals result 
            //in double counting when calcs are being summed
            //set them to zero
            npv1Stock.Progress1.InitTotalBenefitsProperties();
            npv1Stock.Progress1.InitTotalCostsProperties();
            npv1Stock.Progress1.TotalRAmount = 0;
            //times is already in comp amount
            npv1Stock.Progress1.TotalRCompositionAmount = 0;
            npv1Stock.Progress1.TotalRPrice = 0;
            bHasAnalysis = true;
            return bHasAnalysis;
        }
        
        private List<NPV1Stock> SetTotals(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            List<NPV1Stock> progressStocks = new List<NPV1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of npv1stocks for each input and output
            //object model is calc.Total1
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(npv1Stock.GetType()))
                {
                    NPV1Stock stock = (NPV1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                            //calc holds an input or output stock
                            //add that stock to npv1stock (some analyses will need to use subprices too)
                            bHasTotals = AddSubTotalToTotalStock(npv1Stock.Progress1, npv1Stock.Multiplier, stock.Progress1);
                            if (bHasTotals)
                            {
                                progressStocks.Add(npv1Stock);
                            }
                        }
                    }
                }
            }
            return progressStocks;
        }
        private List<NPV1Stock> SetAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            List<NPV1Stock> progressStocks = new List<NPV1Stock>();
            bool bHasTotals = false;
            //set N
            int iCostN = 0;
            int iBenefitN = 0;
            //set the calc totals in each observation
            NPV1Stock observationStock = new NPV1Stock(npv1Stock.Progress1.CalcParameters, npv1Stock.Progress1.CalcParameters.AnalyzerParms.AnalyzerType);
            observationStock.Progress1 = new NPV1Progress1();
            observationStock.Progress1.CalcParameters = new CalculatorParameters(npv1Stock.Progress1.CalcParameters);
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(npv1Stock.GetType()))
                {
                    NPV1Stock stock = (NPV1Stock)calc;
                    if (stock != null)
                    {
                        if (stock.Progress1 != null)
                        {
                            NPV1Stock observation2Stock = new NPV1Stock(stock.Progress1.CalcParameters, stock.Progress1.CalcParameters.AnalyzerParms.AnalyzerType);
                            ////stocks need some props set
                            //stock.CalcParameters.CurrentElementNodeName 
                            //    = npv1Stock.CalcParameters.CurrentElementNodeName;
                            bHasTotals = SetObservationStock(progressStocks, calc,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                progressStocks.Add(observation2Stock);
                                bool bIsCostNode = CalculatorHelpers.IsCostNode(stock.CalcParameters.CurrentElementNodeName);
                                bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(stock.CalcParameters.CurrentElementNodeName);
                                if (bIsCostNode)
                                {
                                    iCostN++;
                                }
                                else if (bIsBenefitNode)
                                {
                                    iBenefitN++;
                                }
                                else
                                {
                                    iCostN++;
                                    iBenefitN++;
                                }
                            }
                        }
                    }
                }
            }
            if (iCostN > 0 || iBenefitN > 0)
            {
                bHasTotals = SetProgressAnalysis(progressStocks, npv1Stock, iCostN, iBenefitN);
            }
            return progressStocks;
        }
        private List<NPV1Stock> SetIOAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            List<NPV1Stock> progressStocks = new List<NPV1Stock>();
            bool bHasTotals = false;
            //set the calc totals in each observation
            int iCostN2 = 0;
            int iBenefitN2 = 0;
            //inputs and outputs use calc for each observation (not the Grouping.Alternative)
            foreach (Calculator1 calc in calcs)
            {
                if (calc.GetType().Equals(npv1Stock.GetType()))
                {
                    NPV1Stock stock = (NPV1Stock)calc;
                    if (stock != null)
                    {
                        //stock.Progress1 holds the initial substock/price totals
                        if (stock.Progress1 != null)
                        {
                            NPV1Stock observation2Stock = new NPV1Stock(stock.Progress1.CalcParameters, stock.Progress1.CalcParameters.AnalyzerParms.AnalyzerType);
                            bHasTotals = SetObservationStock(progressStocks, calc,
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                progressStocks.Add(observation2Stock);
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
                bHasTotals = SetProgressAnalysis(progressStocks, npv1Stock, iCostN2, iBenefitN2);
            }
            return progressStocks;
        }
    
        private bool SetObservationStock(List<NPV1Stock> progressStocks,
            Calculator1 calc, NPV1Stock stock, NPV1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Progress1 = new NPV1Progress1();
            observation2Stock.Id = stock.Id;
            observation2Stock.Progress1.Id = stock.Id;
            //copy some stock props to progress1
            BINPV1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock.Progress1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BINPV1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Progress1.CalcParameters == null)
                stock.Progress1.CalcParameters = new CalculatorParameters();
            observation2Stock.Progress1.CalcParameters = new CalculatorParameters(stock.CalcParameters);
            observation2Stock.CalcParameters = new CalculatorParameters(stock.CalcParameters);
            //at oc and outcome level no aggregating by targettype, need schedule variances)
            //calc.Multiplier not used because base calcs used it
            double dbMultiplier = 1;
            bHasTotals = SetSubTotalFromTotalStock(observation2Stock.Progress1,
                dbMultiplier, stock.Progress1);
            return bHasTotals;
        }
        private bool SetProgressAnalysis(List<NPV1Stock> progressStocks, NPV1Stock npv1Stock,
            int costN, int benN)
        {
            bool bHasTotals = false;
            if (costN > 0)
            {
                //set progress numbers
                SetAMOCProgress(npv1Stock, progressStocks);
                SetAMAOHProgress(npv1Stock, progressStocks);
                SetAMCAPProgress(npv1Stock, progressStocks);
                SetAMProgress(npv1Stock, progressStocks);
                SetAMIncentProgress(npv1Stock, progressStocks);
                SetAMNetProgress(npv1Stock, progressStocks);
            }
            if (benN > 0)
            {
                //benefits
                SetRProgress(npv1Stock, progressStocks);
                SetRAmountProgress(npv1Stock, progressStocks);
                SetAMRIncentProgress(npv1Stock, progressStocks);
                SetRPriceProgress(npv1Stock, progressStocks);
            }
            //add cumulative totals to parent npvstock1 (ocgroup)
            AddCumulative1SubStocks(progressStocks, npv1Stock);
            bHasTotals = AddCumulative1Calcs(progressStocks, npv1Stock);
            return bHasTotals;
        }
        private bool AddCumulative1Calcs(List<NPV1Stock> progressStocks, NPV1Stock npv1Stock)
        {
            bool bHasTotals = false;
            //could be all planned, all actual
            NPV1Stock cumChange = progressStocks.LastOrDefault();
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
                        AddCumulativeTotals(npv1Stock,
                            cumChange.Progress1);
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private void AddCumulative1SubStocks(List<NPV1Stock> progressStocks, NPV1Stock npv1Stock)
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
                        ////actual cost subtotals
                        //AddSubStock1Totals(npv1Stock.Progress1, stat.Progress1);
                        ////actual benefit subtotals
                        //AddSubStock2Totals(npv1Stock.Progress1, stat.Progress1);
                    }
                }
                else
                {
                    ////actual cost subtotals
                    //AddSubStock1Totals(npv1Stock.Progress1, stat.Progress1);
                    ////actual benefit subtotals
                    //AddSubStock2Totals(npv1Stock.Progress1, stat.Progress1);
                }
            }
        }
        public static bool AddCumulative1Calcs(NPV1Stock totalsStock, List<Calculator1> calcs)
        {
            bool bHasTotals = false;
            NPV1Stock stock = new NPV1Stock(totalsStock.CalcParameters, totalsStock.CalcParameters.AnalyzerParms.AnalyzerType);
            if (calcs != null)
            {
                foreach (var calc in calcs)
                {
                    if (calc.GetType().Equals(stock.GetType()))
                    {
                        //convert basecalc to stock
                        stock = (NPV1Stock)calc;
                    }
                }
                //last stock in a series always has cumulatives (planned, actual, or both)
                if (!string.IsNullOrEmpty(stock.TargetType))
                {
                    if (stock.Progress1 != null)
                    {
                        AddCumulativeTotals(totalsStock, stock.Progress1);
                        ////actual cost subtotals
                        //CopySubStock1Totals(totalsStock.Progress1, stock.Progress1);
                        ////actual benefit subtotals
                        //CopySubStock2Totals(totalsStock.Progress1, stock.Progress1);
                        bHasTotals = true;
                    }
                }
            }
            return bHasTotals;
        }
        private static void AddCumulativeTotals(NPV1Stock npv1Stock,
            NPV1Progress1 cumulativeTotal)
        {
            if (npv1Stock.Progress1 == null)
            {
                npv1Stock.Progress1 = new NPV1Progress1();
            }
            npv1Stock.Progress1.TotalAMOC = cumulativeTotal.TotalAMOC;
            npv1Stock.Progress1.TotalAMOCACChange = cumulativeTotal.TotalAMOCACChange;
            npv1Stock.Progress1.TotalAMOCACTotal = cumulativeTotal.TotalAMOCACTotal;
            npv1Stock.Progress1.TotalAMOCAPChange = cumulativeTotal.TotalAMOCAPChange;
            npv1Stock.Progress1.TotalAMOCAPTotal = cumulativeTotal.TotalAMOCAPTotal;
            npv1Stock.Progress1.TotalAMOCPCTotal = cumulativeTotal.TotalAMOCPCTotal;
            npv1Stock.Progress1.TotalAMOCPFTotal = cumulativeTotal.TotalAMOCPFTotal;
            npv1Stock.Progress1.TotalAMOCPPPercent = cumulativeTotal.TotalAMOCPPPercent;
            npv1Stock.Progress1.TotalAMOCPCPercent = cumulativeTotal.TotalAMOCPCPercent;
            npv1Stock.Progress1.TotalAMOCPFPercent = cumulativeTotal.TotalAMOCPFPercent;

            npv1Stock.Progress1.TotalAMAOH = cumulativeTotal.TotalAMAOH;
            npv1Stock.Progress1.TotalAMAOHACChange = cumulativeTotal.TotalAMAOHACChange;
            npv1Stock.Progress1.TotalAMAOHACTotal = cumulativeTotal.TotalAMAOHACTotal;
            npv1Stock.Progress1.TotalAMAOHAPChange = cumulativeTotal.TotalAMAOHAPChange;
            npv1Stock.Progress1.TotalAMAOHAPTotal = cumulativeTotal.TotalAMAOHAPTotal;
            npv1Stock.Progress1.TotalAMAOHPCTotal = cumulativeTotal.TotalAMAOHPCTotal;
            npv1Stock.Progress1.TotalAMAOHPFTotal = cumulativeTotal.TotalAMAOHPFTotal;
            npv1Stock.Progress1.TotalAMAOHPPPercent = cumulativeTotal.TotalAMAOHPPPercent;
            npv1Stock.Progress1.TotalAMAOHPCPercent = cumulativeTotal.TotalAMAOHPCPercent;
            npv1Stock.Progress1.TotalAMAOHPFPercent = cumulativeTotal.TotalAMAOHPFPercent;

            npv1Stock.Progress1.TotalAMCAP = cumulativeTotal.TotalAMCAP;
            npv1Stock.Progress1.TotalAMCAPACChange = cumulativeTotal.TotalAMCAPACChange;
            npv1Stock.Progress1.TotalAMCAPACTotal = cumulativeTotal.TotalAMCAPACTotal;
            npv1Stock.Progress1.TotalAMCAPAPChange = cumulativeTotal.TotalAMCAPAPChange;
            npv1Stock.Progress1.TotalAMCAPAPTotal = cumulativeTotal.TotalAMCAPAPTotal;
            npv1Stock.Progress1.TotalAMCAPPCTotal = cumulativeTotal.TotalAMCAPPCTotal;
            npv1Stock.Progress1.TotalAMCAPPFTotal = cumulativeTotal.TotalAMCAPPFTotal;
            npv1Stock.Progress1.TotalAMCAPPPPercent = cumulativeTotal.TotalAMCAPPPPercent;
            npv1Stock.Progress1.TotalAMCAPPCPercent = cumulativeTotal.TotalAMCAPPCPercent;
            npv1Stock.Progress1.TotalAMCAPPFPercent = cumulativeTotal.TotalAMCAPPFPercent;

            npv1Stock.Progress1.TotalAMTOTAL = cumulativeTotal.TotalAMTOTAL;
            npv1Stock.Progress1.TotalAMACChange = cumulativeTotal.TotalAMACChange;
            npv1Stock.Progress1.TotalAMACTotal = cumulativeTotal.TotalAMACTotal;
            npv1Stock.Progress1.TotalAMAPChange = cumulativeTotal.TotalAMAPChange;
            npv1Stock.Progress1.TotalAMAPTotal = cumulativeTotal.TotalAMAPTotal;
            npv1Stock.Progress1.TotalAMPCTotal = cumulativeTotal.TotalAMPCTotal;
            npv1Stock.Progress1.TotalAMPFTotal = cumulativeTotal.TotalAMPFTotal;
            npv1Stock.Progress1.TotalAMPPPercent = cumulativeTotal.TotalAMPPPercent;
            npv1Stock.Progress1.TotalAMPCPercent = cumulativeTotal.TotalAMPCPercent;
            npv1Stock.Progress1.TotalAMPFPercent = cumulativeTotal.TotalAMPFPercent;

            npv1Stock.Progress1.TotalAMINCENT = cumulativeTotal.TotalAMINCENT;
            npv1Stock.Progress1.TotalAMIncentACChange = cumulativeTotal.TotalAMIncentACChange;
            npv1Stock.Progress1.TotalAMIncentACTotal = cumulativeTotal.TotalAMIncentACTotal;
            npv1Stock.Progress1.TotalAMIncentAPChange = cumulativeTotal.TotalAMIncentAPChange;
            npv1Stock.Progress1.TotalAMIncentAPTotal = cumulativeTotal.TotalAMIncentAPTotal;
            npv1Stock.Progress1.TotalAMIncentPCTotal = cumulativeTotal.TotalAMIncentPCTotal;
            npv1Stock.Progress1.TotalAMIncentPFTotal = cumulativeTotal.TotalAMIncentPFTotal;
            npv1Stock.Progress1.TotalAMIncentPPPercent = cumulativeTotal.TotalAMIncentPPPercent;
            npv1Stock.Progress1.TotalAMIncentPCPercent = cumulativeTotal.TotalAMIncentPCPercent;
            npv1Stock.Progress1.TotalAMIncentPFPercent = cumulativeTotal.TotalAMIncentPFPercent;

            npv1Stock.Progress1.TotalAMNET = cumulativeTotal.TotalAMNET;
            npv1Stock.Progress1.TotalAMNETACChange = cumulativeTotal.TotalAMNETACChange;
            npv1Stock.Progress1.TotalAMNETACTotal = cumulativeTotal.TotalAMNETACTotal;
            npv1Stock.Progress1.TotalAMNETAPChange = cumulativeTotal.TotalAMNETAPChange;
            npv1Stock.Progress1.TotalAMNETAPTotal = cumulativeTotal.TotalAMNETAPTotal;
            npv1Stock.Progress1.TotalAMNETPCTotal = cumulativeTotal.TotalAMNETPCTotal;
            npv1Stock.Progress1.TotalAMNETPFTotal = cumulativeTotal.TotalAMNETPFTotal;
            npv1Stock.Progress1.TotalAMNETPPPercent = cumulativeTotal.TotalAMNETPPPercent;
            npv1Stock.Progress1.TotalAMNETPCPercent = cumulativeTotal.TotalAMNETPCPercent;
            npv1Stock.Progress1.TotalAMNETPFPercent = cumulativeTotal.TotalAMNETPFPercent;

            npv1Stock.Progress1.TotalAMR = cumulativeTotal.TotalAMR;
            npv1Stock.Progress1.TotalAMRACChange = cumulativeTotal.TotalAMRACChange;
            npv1Stock.Progress1.TotalAMRACTotal = cumulativeTotal.TotalAMRACTotal;
            npv1Stock.Progress1.TotalAMRAPChange = cumulativeTotal.TotalAMRAPChange;
            npv1Stock.Progress1.TotalAMRAPTotal = cumulativeTotal.TotalAMRAPTotal;
            npv1Stock.Progress1.TotalAMRPCTotal = cumulativeTotal.TotalAMRPCTotal;
            npv1Stock.Progress1.TotalAMRPFTotal = cumulativeTotal.TotalAMRPFTotal;
            npv1Stock.Progress1.TotalAMRPPPercent = cumulativeTotal.TotalAMRPPPercent;
            npv1Stock.Progress1.TotalAMRPCPercent = cumulativeTotal.TotalAMRPCPercent;
            npv1Stock.Progress1.TotalAMRPFPercent = cumulativeTotal.TotalAMRPFPercent;

            npv1Stock.Progress1.TotalRAmount = cumulativeTotal.TotalRAmount;
            npv1Stock.Progress1.TotalRAmountACChange = cumulativeTotal.TotalRAmountACChange;
            npv1Stock.Progress1.TotalRAmountACTotal = cumulativeTotal.TotalRAmountACTotal;
            npv1Stock.Progress1.TotalRAmountAPChange = cumulativeTotal.TotalRAmountAPChange;
            npv1Stock.Progress1.TotalRAmountAPTotal = cumulativeTotal.TotalRAmountAPTotal;
            npv1Stock.Progress1.TotalRAmountPCTotal = cumulativeTotal.TotalRAmountPCTotal;
            npv1Stock.Progress1.TotalRAmountPFTotal = cumulativeTotal.TotalRAmountPFTotal;
            npv1Stock.Progress1.TotalRAmountPPPercent = cumulativeTotal.TotalRAmountPPPercent;
            npv1Stock.Progress1.TotalRAmountPCPercent = cumulativeTotal.TotalRAmountPCPercent;
            npv1Stock.Progress1.TotalRAmountPFPercent = cumulativeTotal.TotalRAmountPFPercent;

            npv1Stock.Progress1.TotalAMRINCENT = cumulativeTotal.TotalAMRINCENT;
            npv1Stock.Progress1.TotalAMRIncentACChange = cumulativeTotal.TotalAMRIncentACChange;
            npv1Stock.Progress1.TotalAMRIncentACTotal = cumulativeTotal.TotalAMRIncentACTotal;
            npv1Stock.Progress1.TotalAMRIncentAPChange = cumulativeTotal.TotalAMRIncentAPChange;
            npv1Stock.Progress1.TotalAMRIncentAPTotal = cumulativeTotal.TotalAMRIncentAPTotal;
            npv1Stock.Progress1.TotalAMRIncentPCTotal = cumulativeTotal.TotalAMRIncentPCTotal;
            npv1Stock.Progress1.TotalAMRIncentPFTotal = cumulativeTotal.TotalAMRIncentPFTotal;
            npv1Stock.Progress1.TotalAMRIncentPPPercent = cumulativeTotal.TotalAMRIncentPPPercent;
            npv1Stock.Progress1.TotalAMRIncentPCPercent = cumulativeTotal.TotalAMRIncentPCPercent;
            npv1Stock.Progress1.TotalAMRIncentPFPercent = cumulativeTotal.TotalAMRIncentPFPercent;

            npv1Stock.Progress1.TotalRPrice = cumulativeTotal.TotalRPrice;
            npv1Stock.Progress1.TotalRPriceACChange = cumulativeTotal.TotalRPriceACChange;
            npv1Stock.Progress1.TotalRPriceACTotal = cumulativeTotal.TotalRPriceACTotal;
            npv1Stock.Progress1.TotalRPriceAPChange = cumulativeTotal.TotalRPriceAPChange;
            npv1Stock.Progress1.TotalRPriceAPTotal = cumulativeTotal.TotalRPriceAPTotal;
            npv1Stock.Progress1.TotalRPricePCTotal = cumulativeTotal.TotalRPricePCTotal;
            npv1Stock.Progress1.TotalRPricePFTotal = cumulativeTotal.TotalRPricePFTotal;
            npv1Stock.Progress1.TotalRPricePPPercent = cumulativeTotal.TotalRPricePPPercent;
            npv1Stock.Progress1.TotalRPricePCPercent = cumulativeTotal.TotalRPricePCPercent;
            npv1Stock.Progress1.TotalRPricePFPercent = cumulativeTotal.TotalRPricePFPercent;
            //subtotals for actuals already added
        }
        public bool CopyTotalToProgressStock(NPV1Progress1 totStock, NPV1Total1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier adjusted costs
            //totStock.TotalAMOC = subTotal.TotalAMOC;
            //totStock.TotalAMAOH = subTotal.TotalAMAOH;
            //totStock.TotalAMCAP = subTotal.TotalAMCAP;
            //totStock.TotalAMTOTAL = subTotal.TotalAMTOTAL;
            //totStock.TotalAMINCENT = subTotal.TotalAMINCENT;
            //totStock.TotalAMNET = subTotal.TotalAMNET;
            //totStock.TotalAMR = subTotal.TotalAMR;
            //totStock.TotalRAmount = subTotal.TotalRAmount;
            //totStock.TotalAMRINCENT = subTotal.TotalAMRINCENT;
            //totStock.TotalRPrice = subTotal.TotalRPrice;
            bHasCalculations = true;
            return bHasCalculations;
        }
        
        public bool AddSubTotalToTotalStock(NPV1Progress1 totStock, double multiplier,
            NPV1Progress1 subTotal)
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
                //totStock.TotalRCompositionAmount += subTotal.TotalRCompositionAmount * multiplier;
                //don't adjust prices by multiplier
                totStock.TotalRPrice += subTotal.TotalRPrice;
                //display the r (ancestors of outs put name in first calc)
                if (!string.IsNullOrEmpty(subTotal.TotalRName))
                {
                    totStock.TotalRName = subTotal.TotalRName;
                    totStock.TotalRUnit = subTotal.TotalRUnit;
                }
                bHasCalculations = true;
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetSubTotalFromTotalStock(NPV1Progress1 totStock, double multiplier,
            NPV1Progress1 subTotal)
        {
            bool bHasCalculations = false;
            //multiplier = input.times * oc.amount * tp.amount
            ChangeSubTotalByMultipliers(subTotal, multiplier);
            //only inputs and outputs need +-
            //the SetAMOCProgress set cumulative totals (not here)
            totStock.TotalAMOC = subTotal.TotalAMOC;
            totStock.TotalAMAOH = subTotal.TotalAMAOH;
            totStock.TotalAMCAP = subTotal.TotalAMCAP;
            totStock.TotalAMTOTAL = subTotal.TotalAMOC + subTotal.TotalAMAOH + subTotal.TotalAMCAP;
            totStock.TotalAMINCENT = subTotal.TotalAMINCENT;
            totStock.TotalAMR = subTotal.TotalAMR;
            totStock.TotalRAmount = subTotal.TotalRAmount;
            totStock.TotalAMRINCENT = subTotal.TotalAMRINCENT;
            totStock.TotalRPrice = subTotal.TotalRPrice;
            totStock.TotalAMNET = totStock.TotalAMR - totStock.TotalAMTOTAL;
            totStock.TotalAMINCENT_NET = totStock.TotalAMRINCENT - totStock.TotalAMINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
        
       
        private static void ChangeSubTotalByMultipliers(NPV1Progress1 subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
            //adjust TotalAMOC -that's the only number used in SetAMOCProgress
            subTotal.TotalAMOC = subTotal.TotalAMOC * multiplier;
            subTotal.TotalAMAOH = subTotal.TotalAMAOH * multiplier;
            subTotal.TotalAMCAP = subTotal.TotalAMCAP * multiplier;
            subTotal.TotalAMTOTAL = subTotal.TotalAMTOTAL * multiplier;
            subTotal.TotalAMINCENT = subTotal.TotalAMINCENT * multiplier;
            subTotal.TotalAMNET = subTotal.TotalAMNET * multiplier;
            subTotal.TotalAMR = subTotal.TotalAMR * multiplier;
            subTotal.TotalRAmount = subTotal.TotalRAmount * multiplier;
            subTotal.TotalAMRINCENT = subTotal.TotalAMRINCENT * multiplier;
            subTotal.TotalRPrice = subTotal.TotalRPrice;
        }
        
        private static void SetAMOCProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMOC
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAMOC;
                    //planned cumulative
                    planned.Progress1.TotalAMOCPCTotal = dbPlannedTotalCost;
                    
                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMOCPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAMOC;
                    actual.Progress1.TotalAMOCACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMOCAPTotal = actual.Progress1.TotalAMOC;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMOCPCTotal = planned.Progress1.TotalAMOCPCTotal;
                        //set actual.planned period
                        //TotalAMOC is always planned period and TotalAMOCAPTotal is actual period
                        actual.Progress1.TotalAMOC = planned.Progress1.TotalAMOC;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMOCPFTotal = planned.Progress1.TotalAMOCPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMOCAPChange = actual.Progress1.TotalAMOCAPTotal - actual.Progress1.TotalAMOC;
                    //cumulative change
                    actual.Progress1.TotalAMOCACChange = actual.Progress1.TotalAMOCACTotal - actual.Progress1.TotalAMOCPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMOCPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMOCAPTotal, actual.Progress1.TotalAMOC);
                    actual.Progress1.TotalAMOCPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMOCACTotal, actual.Progress1.TotalAMOCPCTotal);
                    actual.Progress1.TotalAMOCPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMOCACTotal, actual.Progress1.TotalAMOCPFTotal);
                }
            }
        }
        private static NPV1Stock GetProgressStockByLabel(NPV1Stock actual, List<int> ids,
            List<NPV1Stock> progressStocks, string targetType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            NPV1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (progressStocks.Any(p => p.Label == actual.Label
                && p.TargetType == targetType))
            {
                int iIndex = 1;
                foreach (NPV1Stock planned in progressStocks)
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
            NPV1Stock planned, List<NPV1Stock> progressStocks, string targetType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (NPV1Stock rp in progressStocks)
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
        private static void SetAMAOHProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMOC
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAMAOH;
                    //planned cumulative
                    planned.Progress1.TotalAMAOHPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMAOHPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAMAOH;
                    actual.Progress1.TotalAMAOHACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMAOHAPTotal = actual.Progress1.TotalAMAOH;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMAOHPCTotal = planned.Progress1.TotalAMAOHPCTotal;
                        //set actual.planned period
                        //TotalAMAOH is always planned period and TotalAMAOHAPTotal is actual period
                        actual.Progress1.TotalAMAOH = planned.Progress1.TotalAMAOH;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMAOHPFTotal = planned.Progress1.TotalAMAOHPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMAOHAPChange = actual.Progress1.TotalAMAOHAPTotal - actual.Progress1.TotalAMAOH;
                    //cumulative change
                    actual.Progress1.TotalAMAOHACChange = actual.Progress1.TotalAMAOHACTotal - actual.Progress1.TotalAMAOHPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMAOHPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMAOHAPTotal, actual.Progress1.TotalAMAOH);
                    actual.Progress1.TotalAMAOHPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMAOHACTotal, actual.Progress1.TotalAMAOHPCTotal);
                    actual.Progress1.TotalAMAOHPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMAOHACTotal, actual.Progress1.TotalAMAOHPFTotal);
                }
            }
        }
        private static void SetAMCAPProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMCAP
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAMCAP;
                    //planned cumulative
                    planned.Progress1.TotalAMCAPPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMCAPPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAMCAP;
                    actual.Progress1.TotalAMCAPACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMCAPAPTotal = actual.Progress1.TotalAMCAP;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMCAPPCTotal = planned.Progress1.TotalAMCAPPCTotal;
                        //set actual.planned period
                        //TotalAMCAP is always planned period and TotalAMCAPAPTotal is actual period
                        actual.Progress1.TotalAMCAP = planned.Progress1.TotalAMCAP;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMCAPPFTotal = planned.Progress1.TotalAMCAPPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMCAPAPChange = actual.Progress1.TotalAMCAPAPTotal - actual.Progress1.TotalAMCAP;
                    //cumulative change
                    actual.Progress1.TotalAMCAPACChange = actual.Progress1.TotalAMCAPACTotal - actual.Progress1.TotalAMCAPPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMCAPPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMCAPAPTotal, actual.Progress1.TotalAMCAP);
                    actual.Progress1.TotalAMCAPPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMCAPACTotal, actual.Progress1.TotalAMCAPPCTotal);
                    actual.Progress1.TotalAMCAPPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMCAPACTotal, actual.Progress1.TotalAMCAPPFTotal);
                }
            }
        }
        private static void SetAMProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMTOTAL
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAMTOTAL;
                    //planned cumulative
                    planned.Progress1.TotalAMPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAMTOTAL;
                    actual.Progress1.TotalAMACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMAPTotal = actual.Progress1.TotalAMTOTAL;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMPCTotal = planned.Progress1.TotalAMPCTotal;
                        //set actual.planned period
                        //TotalAMTOTAL is always planned period and TotalAMAPTotal is actual period
                        actual.Progress1.TotalAMTOTAL = planned.Progress1.TotalAMTOTAL;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMPFTotal = planned.Progress1.TotalAMPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMAPChange = actual.Progress1.TotalAMAPTotal - actual.Progress1.TotalAMTOTAL;
                    //cumulative change
                    actual.Progress1.TotalAMACChange = actual.Progress1.TotalAMACTotal - actual.Progress1.TotalAMPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMAPTotal, actual.Progress1.TotalAMTOTAL);
                    actual.Progress1.TotalAMPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMACTotal, actual.Progress1.TotalAMPCTotal);
                    actual.Progress1.TotalAMPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMACTotal, actual.Progress1.TotalAMPFTotal);
                }
            }
        }
        private static void SetAMIncentProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMTOTAL
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAMINCENT;
                    //planned cumulative
                    planned.Progress1.TotalAMIncentPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMIncentPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAMINCENT;
                    actual.Progress1.TotalAMIncentACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMIncentAPTotal = actual.Progress1.TotalAMINCENT;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMIncentPCTotal = planned.Progress1.TotalAMIncentPCTotal;
                        //set actual.planned period
                        //TotalAMINCENT is always planned period and TotalAMIncentAPTotal is actual period
                        actual.Progress1.TotalAMINCENT = planned.Progress1.TotalAMINCENT;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMIncentPFTotal = planned.Progress1.TotalAMIncentPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMIncentAPChange = actual.Progress1.TotalAMIncentAPTotal - actual.Progress1.TotalAMINCENT;
                    //cumulative change
                    actual.Progress1.TotalAMIncentACChange = actual.Progress1.TotalAMIncentACTotal - actual.Progress1.TotalAMIncentPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMIncentPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMIncentAPTotal, actual.Progress1.TotalAMINCENT);
                    actual.Progress1.TotalAMIncentPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMIncentACTotal, actual.Progress1.TotalAMIncentPCTotal);
                    actual.Progress1.TotalAMIncentPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMIncentACTotal, actual.Progress1.TotalAMIncentPFTotal);
                }
            }
        }
        private static void SetAMNetProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalCost = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMNET
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalCost += planned.Progress1.TotalAMNET;
                    //planned cumulative
                    planned.Progress1.TotalAMNETPCTotal = dbPlannedTotalCost;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMNETPFTotal = dbPlannedTotalCost;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalCost = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalCost += actual.Progress1.TotalAMNET;
                    actual.Progress1.TotalAMNETACTotal = dbActualTotalCost;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMNETAPTotal = actual.Progress1.TotalAMNET;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMNETPCTotal = planned.Progress1.TotalAMNETPCTotal;
                        //set actual.planned period
                        //TotalAMNET is always planned period and TotalAMNETAPTotal is actual period
                        actual.Progress1.TotalAMNET = planned.Progress1.TotalAMNET;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMNETPFTotal = planned.Progress1.TotalAMNETPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMNETAPChange = actual.Progress1.TotalAMNETAPTotal - actual.Progress1.TotalAMNET;
                    //cumulative change
                    actual.Progress1.TotalAMNETACChange = actual.Progress1.TotalAMNETACTotal - actual.Progress1.TotalAMNETPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMNETPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMNETAPTotal, actual.Progress1.TotalAMNET);
                    actual.Progress1.TotalAMNETPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMNETACTotal, actual.Progress1.TotalAMNETPCTotal);
                    actual.Progress1.TotalAMNETPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMNETACTotal, actual.Progress1.TotalAMNETPFTotal);
                }
            }
        }
        private static void SetRProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMR
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalAMR;
                    //planned cumulative
                    planned.Progress1.TotalAMRPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMRPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalAMR;
                    actual.Progress1.TotalAMRACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMRAPTotal = actual.Progress1.TotalAMR;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMRPCTotal = planned.Progress1.TotalAMRPCTotal;
                        //set actual.planned period
                        //TotalAMR is always planned period and TotalAMRAPTotal is actual period
                        actual.Progress1.TotalAMR = planned.Progress1.TotalAMR;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMRPFTotal = planned.Progress1.TotalAMRPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMRAPChange = actual.Progress1.TotalAMRAPTotal - actual.Progress1.TotalAMR;
                    //cumulative change
                    actual.Progress1.TotalAMRACChange = actual.Progress1.TotalAMRACTotal - actual.Progress1.TotalAMRPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMRPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMRAPTotal, actual.Progress1.TotalAMR);
                    actual.Progress1.TotalAMRPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMRACTotal, actual.Progress1.TotalAMRPCTotal);
                    actual.Progress1.TotalAMRPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMRACTotal, actual.Progress1.TotalAMRPFTotal);
                }
            }
        }
        private static void SetRAmountProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalRAmount
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalRAmount;
                    //planned cumulative
                    planned.Progress1.TotalRAmountPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalRAmountPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalRAmount;
                    actual.Progress1.TotalRAmountACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalRAmountAPTotal = actual.Progress1.TotalRAmount;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalRAmountPCTotal = planned.Progress1.TotalRAmountPCTotal;
                        //set actual.planned period
                        //TotalRAmount is always planned period and TotalRAmountAPTotal is actual period
                        actual.Progress1.TotalRAmount = planned.Progress1.TotalRAmount;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalRAmountPFTotal = planned.Progress1.TotalRAmountPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalRAmountAPChange = actual.Progress1.TotalRAmountAPTotal - actual.Progress1.TotalRAmount;
                    //cumulative change
                    actual.Progress1.TotalRAmountACChange = actual.Progress1.TotalRAmountACTotal - actual.Progress1.TotalRAmountPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalRAmountPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRAmountAPTotal, actual.Progress1.TotalRAmount);
                    actual.Progress1.TotalRAmountPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRAmountACTotal, actual.Progress1.TotalRAmountPCTotal);
                    actual.Progress1.TotalRAmountPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalRAmountACTotal, actual.Progress1.TotalRAmountPFTotal);
                }
            }
        }
        private static void SetAMRIncentProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalAMRINCENT
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalAMRINCENT;
                    //planned cumulative
                    planned.Progress1.TotalAMRIncentPCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalAMRIncentPFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalAMRINCENT;
                    actual.Progress1.TotalAMRIncentACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalAMRIncentAPTotal = actual.Progress1.TotalAMRINCENT;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalAMRIncentPCTotal = planned.Progress1.TotalAMRIncentPCTotal;
                        //set actual.planned period
                        //TotalAMRINCENT is always planned period and TotalAMRIncentAPTotal is actual period
                        actual.Progress1.TotalAMRINCENT = planned.Progress1.TotalAMRINCENT;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalAMRIncentPFTotal = planned.Progress1.TotalAMRIncentPFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalAMRIncentAPChange = actual.Progress1.TotalAMRIncentAPTotal - actual.Progress1.TotalAMRINCENT;
                    //cumulative change
                    actual.Progress1.TotalAMRIncentACChange = actual.Progress1.TotalAMRIncentACTotal - actual.Progress1.TotalAMRIncentPCTotal;
                    //set planned period percent
                    actual.Progress1.TotalAMRIncentPPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMRIncentAPTotal, actual.Progress1.TotalAMRINCENT);
                    actual.Progress1.TotalAMRIncentPCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMRIncentACTotal, actual.Progress1.TotalAMRIncentPCTotal);
                    actual.Progress1.TotalAMRIncentPFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalAMRIncentACTotal, actual.Progress1.TotalAMRIncentPFTotal);
                }
            }
        }
        private static void SetRPriceProgress(NPV1Stock npv1Stock, List<NPV1Stock> progressStocks)
        {
            //the progressStats actual members are used to store the results
            //the planned member results get added cumulatively to actual members
            double dbPlannedTotalBenefit = 0;
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned period = planned.Progress1.TotalRPrice
                    //this means that the planned that get sent with actuals
                    //can also be set correctly again
                    dbPlannedTotalBenefit += planned.Progress1.TotalRPrice;
                    //planned cumulative
                    planned.Progress1.TotalRPricePCTotal = dbPlannedTotalBenefit;

                }
            }
            foreach (NPV1Stock planned in progressStocks)
            {
                if (planned.TargetType == TARGET_TYPES.benchmark.ToString())
                {
                    //planned full 
                    planned.Progress1.TotalRPricePFTotal = dbPlannedTotalBenefit;

                }
            }
            //set the actuals
            List<int> ids = new List<int>();
            double dbActualTotalBenefit = 0;
            foreach (NPV1Stock actual in progressStocks)
            {
                if (actual.TargetType == TARGET_TYPES.actual.ToString())
                {
                    //set actual cumulative
                    dbActualTotalBenefit += actual.Progress1.TotalRPrice;
                    actual.Progress1.TotalRPriceACTotal = dbActualTotalBenefit;
                    //set actual period using last actual total
                    actual.Progress1.TotalRPriceAPTotal = actual.Progress1.TotalRPrice;
                    //set the corresponding planned totals
                    NPV1Stock planned = GetProgressStockByLabel(
                        actual, ids, progressStocks, Calculator1.TARGET_TYPES.benchmark.ToString());
                    if (planned != null)
                    {
                        //set actual.planned cumulative
                        actual.Progress1.TotalRPricePCTotal = planned.Progress1.TotalRPricePCTotal;
                        //set actual.planned period
                        //TotalRPrice is always planned period and TotalRPriceAPTotal is actual period
                        actual.Progress1.TotalRPrice = planned.Progress1.TotalRPrice;
                        //the planned fulltotal to the planned full total
                        actual.Progress1.TotalRPricePFTotal = planned.Progress1.TotalRPricePFTotal;
                    }
                    //set the variances
                    //partial period change
                    actual.Progress1.TotalRPriceAPChange = actual.Progress1.TotalRPriceAPTotal - actual.Progress1.TotalRPrice;
                    //cumulative change
                    actual.Progress1.TotalRPriceACChange = actual.Progress1.TotalRPriceACTotal - actual.Progress1.TotalRPricePCTotal;
                    //set planned period percent
                    actual.Progress1.TotalRPricePPPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRPriceAPTotal, actual.Progress1.TotalRPrice);
                    actual.Progress1.TotalRPricePCPercent
                        = CalculatorHelpers.GetPercent(actual.Progress1.TotalRPriceACTotal, actual.Progress1.TotalRPricePCTotal);
                    actual.Progress1.TotalRPricePFPercent
                         = CalculatorHelpers.GetPercent(actual.Progress1.TotalRPriceACTotal, actual.Progress1.TotalRPricePFTotal);
                }
            }
        }
        private bool AddProgsToBaseStock(NPV1Stock npv1Stock,
            List<NPV1Stock> progressStocks)
        {
            bool bHasAnalyses = false;
            npv1Stock.Stocks = new List<NPV1Stock>();
            foreach (NPV1Stock actual in progressStocks)
            {
                npv1Stock.Stocks.Add(actual);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
    
}
