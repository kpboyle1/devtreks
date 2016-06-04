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
    ///             NPV1Stock.Change1
    ///             The class tracks annual changes in totals.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///</summary>
    public class NPV1Change1 : NPV1Stock
    {
        //calls the base-class version, and initializes the base class properties.
        public NPV1Change1()
            : base()
        {
            //subprice object
            InitTotalNPV1Change1Properties(this);
        }
        //copy constructor
        public NPV1Change1(NPV1Change1 calculator)
            : base(calculator)
        {
            CopyTotalNPV1Change1Properties(this, calculator);
        }
        //note that display properties, such as name, description, unit are in parent NPV1Stock
        //calculator properties

        //All Totals must come from base CostBenefitCalculator and stay consistent across apps
        //time period total
        //public double TotalAMOC { get; set; }
        //total change from last time period
        public double TotalAMOCAmountChange { get; set; }
        //percent change from last time period
        public double TotalAMOCPercentChange { get; set; }
        //total change from base lcc or lcb calculator
        public double TotalAMOCBaseChange { get; set; }
        //percent change from base lcc or lcb calculator
        public double TotalAMOCBasePercentChange { get; set; }

        //public double TotalAMAOH { get; set; }
        public double TotalAMAOHAmountChange { get; set; }
        public double TotalAMAOHPercentChange { get; set; }
        public double TotalAMAOHBaseChange { get; set; }
        public double TotalAMAOHBasePercentChange { get; set; }

        //public double TotalAMCAP { get; set; }
        public double TotalAMCAPAmountChange { get; set; }
        public double TotalAMCAPPercentChange { get; set; }
        public double TotalAMCAPBaseChange { get; set; }
        public double TotalAMCAPBasePercentChange { get; set; }

        //total cost
        //public double TotalAMTOTAL { get; set; }
        public double TotalAmountChange { get; set; }
        public double TotalPercentChange { get; set; }
        public double TotalBaseChange { get; set; }
        public double TotalBasePercentChange { get; set; }

        //total incentive adjusted costs 
        //public double TotalAMINCENT { get; set; }
        public double TotalAMIncentAmountChange { get; set; }
        public double TotalAMIncentPercentChange { get; set; }
        public double TotalAMIncentBaseChange { get; set; }
        public double TotalAMIncentBasePercentChange { get; set; }

        //net returns
        //public double TotalAMNET { get; set; }
        public double TotalAMNETAmountChange { get; set; }
        public double TotalAMNETPercentChange { get; set; }
        public double TotalAMNETBaseChange { get; set; }
        public double TotalAMNETBasePercentChange { get; set; }

        //private const string TAMOC = "TAMOC";
        private const string cTotalAMOCAmountChange = "TAMOCAmountChange";
        private const string cTotalAMOCPercentChange = "TAMOCPercentChange";
        private const string cTotalAMOCBaseChange = "TAMOCBaseChange";
        private const string cTotalAMOCBasePercentChange = "TAMOCBasePercentChange";

        //private const string TAMAOH = "TAMAOH";
        private const string cTotalAMAOHAmountChange = "TAMAOHAmountChange";
        private const string cTotalAMAOHPercentChange = "TAMAOHPercentChange";
        private const string cTotalAMAOHBaseChange = "TAMAOHBaseChange";
        private const string cTotalAMAOHBasePercentChange = "TAMAOHBasePercentChange";

        //private const string TAMCAP = "TAMCAP";
        private const string cTotalAMCAPAmountChange = "TAMCAPAmountChange";
        private const string cTotalAMCAPPercentChange = "TAMCAPPercentChange";
        private const string cTotalAMCAPBaseChange = "TAMCAPBaseChange";
        private const string cTotalAMCAPBasePercentChange = "TAMCAPBasePercentChange";

        //private const string TAMTOTAL = "TAMTOTAL";
        private const string cTotalAmountChange = "TAMAmountChange";
        private const string cTotalPercentChange = "TAMPercentChange";
        private const string cTotalBaseChange = "TAMBaseChange";
        private const string cTotalBasePercentChange = "TAMBasePercentChange";

        //private const string TAMINCENT = "TAMINCENT";
        private const string cTotalAMIncentAmountChange = "TAMIncentAmountChange";
        private const string cTotalAMIncentPercentChange = "TAMIncentPercentChange";
        private const string cTotalAMIncentBaseChange = "TAMIncentBaseChange";
        private const string cTotalAMIncentBasePercentChange = "TAMIncentBasePercentChange";

        //private const string TAMNET = "TAMNET";
        private const string TAMNETAmountChange = "TAMNETAmountChange";
        private const string TAMNETPercentChange = "TAMNETPercentChange";
        private const string TAMNETBaseChange = "TAMNETBaseChange";
        private const string TAMNETBasePercentChange = "TAMNETBasePercentChange";

        //benefits
        //totals, including initbens, salvageval, replacement, and subcosts
        //public double TotalAMR { get; set; }
        public double TotalAMRAmountChange { get; set; }
        public double TotalAMRPercentChange { get; set; }
        public double TotalAMRBaseChange { get; set; }
        public double TotalAMRBasePercentChange { get; set; }
        //total output amount (note this must be output.amount * output.compositionamount)
        //public double TotalRAmount { get; set; }
        public double TotalRAmountChange { get; set; }
        public double TotalRAmountPercentChange { get; set; }
        public double TotalRAmountBaseChange { get; set; }
        public double TotalRAmountBasePercentChange { get; set; }
        //total incentive adjusted benefits
        //public double TotalAMRINCENT { get; set; }
        public double TotalAMRIncentAmountChange { get; set; }
        public double TotalAMRIncentPercentChange { get; set; }
        public double TotalAMRIncentBaseChange { get; set; }
        public double TotalAMRIncentBasePercentChange { get; set; }
        //net incentive adjusted benefits
        //public double TotalRPrice { get; set; }
        public double TotalRPriceAmountChange { get; set; }
        public double TotalRPricePercentChange { get; set; }
        public double TotalRPriceBaseChange { get; set; }
        public double TotalRPriceBasePercentChange { get; set; }

        //private const string TAMR = "TAMR";
        private const string cTotalAMRAmountChange = "TAMRAmountChange";
        private const string cTotalAMRPercentChange = "TAMRPercentChange";
        private const string cTotalAMRBaseChange = "TAMRBaseChange";
        private const string cTotalAMRBasePercentChange = "TAMRBasePercentChange";

        //private const string TRAmount = "TRAmount";
        private const string cTotalRAmountChange = "TRAmountChange";
        private const string cTotalRAmountPercentChange = "TRAmountPercentChange";
        private const string cTotalRAmountBaseChange = "TRAmountBaseChange";
        private const string cTotalRAmountBasePercentChange = "TRAmountBasePercentChange";

        //private const string TAMRINCENT = "TAMRINCENT";
        private const string cTotalAMRIncentAmountChange = "TAMRIncentAmountChange";
        private const string cTotalAMRIncentPercentChange = "TAMRIncentPercentChange";
        private const string cTotalAMRIncentBaseChange = "TAMRIncentBaseChange";
        private const string cTotalAMRIncentBasePercentChange = "TAMRIncentBasePercentChange";

        //private const string TRPrice = "TRPrice";
        private const string cTotalRPriceAmountChange = "TRPriceAmountChange";
        private const string cTotalRPricePercentChange = "TRPricePercentChange";
        private const string cTotalRPriceBaseChange = "TRPriceBaseChange";
        private const string cTotalRPriceBasePercentChange = "TRPriceBasePercentChange";

        public void InitTotalNPV1Change1Properties(NPV1Change1 ind)
        {
            ind.ErrorMessage = string.Empty;
            //includes summary data
            ind.InitTotalBenefitsProperties();
            ind.InitTotalCostsProperties();


            ind.TotalAMOC = 0;
            ind.TotalAMOCAmountChange = 0;
            ind.TotalAMOCPercentChange = 0;
            ind.TotalAMOCBaseChange = 0;
            ind.TotalAMOCBasePercentChange = 0;

            ind.TotalAMAOH = 0;
            ind.TotalAMAOHAmountChange = 0;
            ind.TotalAMAOHPercentChange = 0;
            ind.TotalAMAOHBaseChange = 0;
            ind.TotalAMAOHBasePercentChange = 0;

            ind.TotalAMCAP = 0;
            ind.TotalAMCAPAmountChange = 0;
            ind.TotalAMCAPPercentChange = 0;
            ind.TotalAMCAPBaseChange = 0;
            ind.TotalAMCAPBasePercentChange = 0;

            ind.TotalAMTOTAL = 0;
            ind.TotalAmountChange = 0;
            ind.TotalPercentChange = 0;
            ind.TotalBaseChange = 0;
            ind.TotalBasePercentChange = 0;

            ind.TotalAMINCENT = 0;
            ind.TotalAMIncentAmountChange = 0;
            ind.TotalAMIncentPercentChange = 0;
            ind.TotalAMIncentBaseChange = 0;
            ind.TotalAMIncentBasePercentChange = 0;

            ind.TotalAMNET = 0;
            ind.TotalAMNETAmountChange = 0;
            ind.TotalAMNETPercentChange = 0;
            ind.TotalAMNETBaseChange = 0;
            ind.TotalAMNETBasePercentChange = 0;

            ind.TotalAMR = 0;
            ind.TotalAMRAmountChange = 0;
            ind.TotalAMRPercentChange = 0;
            ind.TotalAMRBaseChange = 0;
            ind.TotalAMRBasePercentChange = 0;

            ind.TotalRAmount = 0;
            ind.TotalRAmountChange = 0;
            ind.TotalRAmountPercentChange = 0;
            ind.TotalRAmountBaseChange = 0;
            ind.TotalRAmountBasePercentChange = 0;

            ind.TotalAMRINCENT = 0;
            ind.TotalAMRIncentAmountChange = 0;
            ind.TotalAMRIncentPercentChange = 0;
            ind.TotalAMRIncentBaseChange = 0;
            ind.TotalAMRIncentBasePercentChange = 0;

            ind.TotalRPrice = 0;
            ind.TotalRPriceAmountChange = 0;
            ind.TotalRPricePercentChange = 0;
            ind.TotalRPriceBaseChange = 0;
            ind.TotalRPriceBasePercentChange = 0;
            ind.CalcParameters = new CalculatorParameters();
        }

        public void CopyTotalNPV1Change1Properties(NPV1Change1 ind,
            NPV1Change1 calculator)
        {
            if (calculator != null)
            {
                //inits with standard cb totals
                ind.CopyCalculatorProperties(calculator);
                ind.CopyTotalBenefitsProperties(calculator);
                ind.CopyTotalCostsProperties(calculator);

                ind.ErrorMessage = calculator.ErrorMessage;
                ind.TotalAMOC = calculator.TotalAMOC;
                ind.TotalAMOCAmountChange = calculator.TotalAMOCAmountChange;
                ind.TotalAMOCPercentChange = calculator.TotalAMOCPercentChange;
                ind.TotalAMOCBaseChange = calculator.TotalAMOCBaseChange;
                ind.TotalAMOCBasePercentChange = calculator.TotalAMOCBasePercentChange;

                ind.TotalAMAOH = calculator.TotalAMAOH;
                ind.TotalAMAOHAmountChange = calculator.TotalAMAOHAmountChange;
                ind.TotalAMAOHPercentChange = calculator.TotalAMAOHPercentChange;
                ind.TotalAMAOHBaseChange = calculator.TotalAMAOHBaseChange;
                ind.TotalAMAOHBasePercentChange = calculator.TotalAMAOHBasePercentChange;

                ind.TotalAMCAP = calculator.TotalAMCAP;
                ind.TotalAMCAPAmountChange = calculator.TotalAMCAPAmountChange;
                ind.TotalAMCAPPercentChange = calculator.TotalAMCAPPercentChange;
                ind.TotalAMCAPBaseChange = calculator.TotalAMCAPBaseChange;
                ind.TotalAMCAPBasePercentChange = calculator.TotalAMCAPBasePercentChange;

                ind.TotalAMTOTAL = calculator.TotalAMTOTAL;
                ind.TotalAmountChange = calculator.TotalAmountChange;
                ind.TotalPercentChange = calculator.TotalPercentChange;
                ind.TotalBaseChange = calculator.TotalBaseChange;
                ind.TotalBasePercentChange = calculator.TotalBasePercentChange;

                ind.TotalAMINCENT = calculator.TotalAMINCENT;
                ind.TotalAMIncentAmountChange = calculator.TotalAMIncentAmountChange;
                ind.TotalAMIncentPercentChange = calculator.TotalAMIncentPercentChange;
                ind.TotalAMIncentBaseChange = calculator.TotalAMIncentBaseChange;
                ind.TotalAMIncentBasePercentChange = calculator.TotalAMIncentBasePercentChange;

                ind.TotalAMNET = calculator.TotalAMNET;
                ind.TotalAMNETAmountChange = calculator.TotalAMNETAmountChange;
                ind.TotalAMNETPercentChange = calculator.TotalAMNETPercentChange;
                ind.TotalAMNETBaseChange = calculator.TotalAMNETBaseChange;
                ind.TotalAMNETBasePercentChange = calculator.TotalAMNETBasePercentChange;

                ind.TotalAMR = calculator.TotalAMR;
                ind.TotalAMRAmountChange = calculator.TotalAMRAmountChange;
                ind.TotalAMRPercentChange = calculator.TotalAMRPercentChange;
                ind.TotalAMRBaseChange = calculator.TotalAMRBaseChange;
                ind.TotalAMRBasePercentChange = calculator.TotalAMRBasePercentChange;

                ind.TotalRAmount = calculator.TotalRAmount;
                ind.TotalRAmountChange = calculator.TotalRAmountChange;
                ind.TotalRAmountPercentChange = calculator.TotalRAmountPercentChange;
                ind.TotalRAmountBaseChange = calculator.TotalRAmountBaseChange;
                ind.TotalRAmountBasePercentChange = calculator.TotalRAmountBasePercentChange;

                ind.TotalAMRINCENT = calculator.TotalAMRINCENT;
                ind.TotalAMRIncentAmountChange = calculator.TotalAMRIncentAmountChange;
                ind.TotalAMRIncentPercentChange = calculator.TotalAMRIncentPercentChange;
                ind.TotalAMRIncentBaseChange = calculator.TotalAMRIncentBaseChange;
                ind.TotalAMRIncentBasePercentChange = calculator.TotalAMRIncentBasePercentChange;

                ind.TotalRPrice = calculator.TotalRPrice;
                ind.TotalRPriceAmountChange = calculator.TotalRPriceAmountChange;
                ind.TotalRPricePercentChange = calculator.TotalRPricePercentChange;
                ind.TotalRPriceBaseChange = calculator.TotalRPriceBaseChange;
                ind.TotalRPriceBasePercentChange = calculator.TotalRPriceBasePercentChange;
                if (calculator.CalcParameters == null)
                    calculator.CalcParameters = new CalculatorParameters();
                if (ind.CalcParameters == null)
                    ind.CalcParameters = new CalculatorParameters();
                ind.CalcParameters = new CalculatorParameters(calculator.CalcParameters);
            }
        }
        public void CopyTotalNPV1Change1RProperties(NPV1Change1 calculator)
        {
            if (calculator != null)
            {
                this.CopyTotalBenefitsPsandQsProperties(calculator);
            }
        }
        public void SetTotalNPV1Change1Properties(NPV1Change1 ind,
            string attNameExtension, XElement calculator)
        {
            ind.SetTotalBenefitsSummaryProperties(attNameExtension, calculator);
            ind.SetTotalCostsSummaryProperties(attNameExtension, calculator);

            ind.TotalAMOC = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMOC, attNameExtension));
            ind.TotalAMOCAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCAmountChange, attNameExtension));
            ind.TotalAMOCPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCPercentChange, attNameExtension));
            ind.TotalAMOCBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCBaseChange, attNameExtension));
            ind.TotalAMOCBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMOCBasePercentChange, attNameExtension));

            ind.TotalAMAOH = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMAOH, attNameExtension));
            ind.TotalAMAOHAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHAmountChange, attNameExtension));
            ind.TotalAMAOHPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHPercentChange, attNameExtension));
            ind.TotalAMAOHBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHBaseChange, attNameExtension));
            ind.TotalAMAOHBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMAOHBasePercentChange, attNameExtension));

            ind.TotalAMCAP = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMCAP, attNameExtension));
            ind.TotalAMCAPAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPAmountChange, attNameExtension));
            ind.TotalAMCAPPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPPercentChange, attNameExtension));
            ind.TotalAMCAPBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPBaseChange, attNameExtension));
            ind.TotalAMCAPBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMCAPBasePercentChange, attNameExtension));

            ind.TotalAMTOTAL = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMTOTAL, attNameExtension));
            ind.TotalAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAmountChange, attNameExtension));
            ind.TotalPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalPercentChange, attNameExtension));
            ind.TotalBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalBaseChange, attNameExtension));
            ind.TotalBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalBasePercentChange, attNameExtension));

            ind.TotalAMINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMINCENT, attNameExtension));
            ind.TotalAMIncentAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentAmountChange, attNameExtension));
            ind.TotalAMIncentPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentPercentChange, attNameExtension));
            ind.TotalAMIncentBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentBaseChange, attNameExtension));
            ind.TotalAMIncentBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMIncentBasePercentChange, attNameExtension));

            ind.TotalAMNET = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNET, attNameExtension));
            ind.TotalAMNETAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETAmountChange, attNameExtension));
            ind.TotalAMNETPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETPercentChange, attNameExtension));
            ind.TotalAMNETBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETBaseChange, attNameExtension));
            ind.TotalAMNETBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMNETBasePercentChange, attNameExtension));

            ind.TotalAMR = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMR, attNameExtension));
            ind.TotalAMRAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRAmountChange, attNameExtension));
            ind.TotalAMRPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRPercentChange, attNameExtension));
            ind.TotalAMRBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRBaseChange, attNameExtension));
            ind.TotalAMRBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRBasePercentChange, attNameExtension));

            ind.TotalRAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TRAmount, attNameExtension));
            ind.TotalRAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountChange, attNameExtension));
            ind.TotalRAmountPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountPercentChange, attNameExtension));
            ind.TotalRAmountBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountBaseChange, attNameExtension));
            ind.TotalRAmountBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRAmountBasePercentChange, attNameExtension));

            ind.TotalAMRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TAMRINCENT, attNameExtension));
            ind.TotalAMRIncentAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentAmountChange, attNameExtension));
            ind.TotalAMRIncentPercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentPercentChange, attNameExtension));
            ind.TotalAMRIncentBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentBaseChange, attNameExtension));
            ind.TotalAMRIncentBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalAMRIncentBasePercentChange, attNameExtension));

            ind.TotalRPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(TRPrice, attNameExtension));
            ind.TotalRPriceAmountChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceAmountChange, attNameExtension));
            ind.TotalRPricePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPricePercentChange, attNameExtension));
            ind.TotalRPriceBaseChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceBaseChange, attNameExtension));
            ind.TotalRPriceBasePercentChange = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cTotalRPriceBasePercentChange, attNameExtension));
        }

        public void SetTotalNPV1Change1Property(NPV1Change1 ind,
            string attDateame, string attValue)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case TAMOC:
                    ind.TotalAMOC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCAmountChange:
                    ind.TotalAMOCAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCPercentChange:
                    ind.TotalAMOCPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCBaseChange:
                    ind.TotalAMOCBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMOCBasePercentChange:
                    ind.TotalAMOCBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH:
                    ind.TotalAMAOH = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHAmountChange:
                    ind.TotalAMAOHAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHPercentChange:
                    ind.TotalAMAOHPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHBaseChange:
                    ind.TotalAMAOHBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMAOHBasePercentChange:
                    ind.TotalAMAOHBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP:
                    ind.TotalAMCAP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPAmountChange:
                    ind.TotalAMCAPAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPPercentChange:
                    ind.TotalAMCAPPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPBaseChange:
                    ind.TotalAMCAPBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMCAPBasePercentChange:
                    ind.TotalAMCAPBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL:
                    ind.TotalAMTOTAL = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAmountChange:
                    ind.TotalAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalPercentChange:
                    ind.TotalPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalBaseChange:
                    ind.TotalBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalBasePercentChange:
                    ind.TotalBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT:
                    ind.TotalAMINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentAmountChange:
                    ind.TotalAMIncentAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentPercentChange:
                    ind.TotalAMIncentPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentBaseChange:
                    ind.TotalAMIncentBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMIncentBasePercentChange:
                    ind.TotalAMIncentBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNET:
                    ind.TotalAMNET = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETAmountChange:
                    ind.TotalAMNETAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETPercentChange:
                    ind.TotalAMNETPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETBaseChange:
                    ind.TotalAMNETBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNETBasePercentChange:
                    ind.TotalAMNETBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR:
                    ind.TotalAMR = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRAmountChange:
                    ind.TotalAMRAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRPercentChange:
                    ind.TotalAMRPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRBaseChange:
                    ind.TotalAMRBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRBasePercentChange:
                    ind.TotalAMRBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRAmount:
                    ind.TotalRAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountChange:
                    ind.TotalRAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountPercentChange:
                    ind.TotalRAmountPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountBaseChange:
                    ind.TotalRAmountBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRAmountBasePercentChange:
                    ind.TotalRAmountBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT:
                    ind.TotalAMRINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentAmountChange:
                    ind.TotalAMRIncentAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentPercentChange:
                    ind.TotalAMRIncentPercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentBaseChange:
                    ind.TotalAMRIncentBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalAMRIncentBasePercentChange:
                    ind.TotalAMRIncentBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice:
                    ind.TotalRPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceAmountChange:
                    ind.TotalRPriceAmountChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPricePercentChange:
                    ind.TotalRPricePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceBaseChange:
                    ind.TotalRPriceBaseChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTotalRPriceBasePercentChange:
                    ind.TotalRPriceBasePercentChange = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }

        public string GetTotalNPV1Change1Property(NPV1Change1 ind, string attDateame)
        {
            string sPropertyValue = string.Empty;
            switch (attDateame)
            {
                case TAMOC:
                    sPropertyValue = ind.TotalAMOC.ToString();
                    break;
                case cTotalAMOCAmountChange:
                    sPropertyValue = ind.TotalAMOCAmountChange.ToString();
                    break;
                case cTotalAMOCPercentChange:
                    sPropertyValue = ind.TotalAMOCPercentChange.ToString();
                    break;
                case cTotalAMOCBaseChange:
                    sPropertyValue = ind.TotalAMOCBaseChange.ToString();
                    break;
                case cTotalAMOCBasePercentChange:
                    sPropertyValue = ind.TotalAMOCBasePercentChange.ToString();
                    break;
                case TAMAOH:
                    sPropertyValue = ind.TotalAMAOH.ToString();
                    break;
                case cTotalAMAOHAmountChange:
                    sPropertyValue = ind.TotalAMAOHAmountChange.ToString();
                    break;
                case cTotalAMAOHPercentChange:
                    sPropertyValue = ind.TotalAMAOHPercentChange.ToString();
                    break;
                case cTotalAMAOHBaseChange:
                    sPropertyValue = ind.TotalAMAOHBaseChange.ToString();
                    break;
                case cTotalAMAOHBasePercentChange:
                    sPropertyValue = ind.TotalAMAOHBasePercentChange.ToString();
                    break;
                case TAMCAP:
                    sPropertyValue = ind.TotalAMCAP.ToString();
                    break;
                case cTotalAMCAPAmountChange:
                    sPropertyValue = ind.TotalAMCAPAmountChange.ToString();
                    break;
                case cTotalAMCAPPercentChange:
                    sPropertyValue = ind.TotalAMCAPPercentChange.ToString();
                    break;
                case cTotalAMCAPBaseChange:
                    sPropertyValue = ind.TotalAMCAPBaseChange.ToString();
                    break;
                case cTotalAMCAPBasePercentChange:
                    sPropertyValue = ind.TotalAMCAPBasePercentChange.ToString();
                    break;
                case TAMTOTAL:
                    sPropertyValue = ind.TotalAMTOTAL.ToString();
                    break;
                case cTotalAmountChange:
                    sPropertyValue = ind.TotalAmountChange.ToString();
                    break;
                case cTotalPercentChange:
                    sPropertyValue = ind.TotalPercentChange.ToString();
                    break;
                case cTotalBaseChange:
                    sPropertyValue = ind.TotalBaseChange.ToString();
                    break;
                case cTotalBasePercentChange:
                    sPropertyValue = ind.TotalBasePercentChange.ToString();
                    break;
                case TAMINCENT:
                    sPropertyValue = ind.TotalAMINCENT.ToString();
                    break;
                case cTotalAMIncentAmountChange:
                    sPropertyValue = ind.TotalAMIncentAmountChange.ToString();
                    break;
                case cTotalAMIncentPercentChange:
                    sPropertyValue = ind.TotalAMIncentPercentChange.ToString();
                    break;
                case cTotalAMIncentBaseChange:
                    sPropertyValue = ind.TotalAMIncentBaseChange.ToString();
                    break;
                case cTotalAMIncentBasePercentChange:
                    sPropertyValue = ind.TotalAMIncentBasePercentChange.ToString();
                    break;
                case TAMNET:
                    sPropertyValue = ind.TotalAMNET.ToString();
                    break;
                case TAMNETAmountChange:
                    sPropertyValue = ind.TotalAMNETAmountChange.ToString();
                    break;
                case TAMNETPercentChange:
                    sPropertyValue = ind.TotalAMNETPercentChange.ToString();
                    break;
                case TAMNETBaseChange:
                    sPropertyValue = ind.TotalAMNETBaseChange.ToString();
                    break;
                case TAMNETBasePercentChange:
                    sPropertyValue = ind.TotalAMNETBasePercentChange.ToString();
                    break;
                case TAMR:
                    sPropertyValue = ind.TotalAMR.ToString();
                    break;
                case cTotalAMRAmountChange:
                    sPropertyValue = ind.TotalAMRAmountChange.ToString();
                    break;
                case cTotalAMRPercentChange:
                    sPropertyValue = ind.TotalAMRPercentChange.ToString();
                    break;
                case cTotalAMRBaseChange:
                    sPropertyValue = ind.TotalAMRBaseChange.ToString();
                    break;
                case cTotalAMRBasePercentChange:
                    sPropertyValue = ind.TotalAMRBasePercentChange.ToString();
                    break;
                case TRAmount:
                    sPropertyValue = ind.TotalRAmount.ToString();
                    break;
                case cTotalRAmountChange:
                    sPropertyValue = ind.TotalRAmountChange.ToString();
                    break;
                case cTotalRAmountPercentChange:
                    sPropertyValue = ind.TotalRAmountPercentChange.ToString();
                    break;
                case cTotalRAmountBaseChange:
                    sPropertyValue = ind.TotalRAmountBaseChange.ToString();
                    break;
                case cTotalRAmountBasePercentChange:
                    sPropertyValue = ind.TotalRAmountBasePercentChange.ToString();
                    break;
                case TAMRINCENT:
                    sPropertyValue = ind.TotalAMRINCENT.ToString();
                    break;
                case cTotalAMRIncentAmountChange:
                    sPropertyValue = ind.TotalAMRIncentAmountChange.ToString();
                    break;
                case cTotalAMRIncentPercentChange:
                    sPropertyValue = ind.TotalAMRIncentPercentChange.ToString();
                    break;
                case cTotalAMRIncentBaseChange:
                    sPropertyValue = ind.TotalAMRIncentBaseChange.ToString();
                    break;
                case cTotalAMRIncentBasePercentChange:
                    sPropertyValue = ind.TotalAMRIncentBasePercentChange.ToString();
                    break;
                case TRPrice:
                    sPropertyValue = ind.TotalRPrice.ToString();
                    break;
                case cTotalRPriceAmountChange:
                    sPropertyValue = ind.TotalRPriceAmountChange.ToString();
                    break;
                case cTotalRPricePercentChange:
                    sPropertyValue = ind.TotalRPricePercentChange.ToString();
                    break;
                case cTotalRPriceBaseChange:
                    sPropertyValue = ind.TotalRPriceBaseChange.ToString();
                    break;
                case cTotalRPriceBasePercentChange:
                    sPropertyValue = ind.TotalRPriceBasePercentChange.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }

        public virtual void SetTotalNPV1Change1Attributes(NPV1Change1 ind,
            string attNameExtension, ref XElement calculator)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMOC, attNameExtension), ind.TotalAMOC);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCAmountChange, attNameExtension), ind.TotalAMOCAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCPercentChange, attNameExtension), ind.TotalAMOCPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCBaseChange, attNameExtension), ind.TotalAMOCBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMOCBasePercentChange, attNameExtension), ind.TotalAMOCBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMAOH, attNameExtension), ind.TotalAMAOH);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHAmountChange, attNameExtension), ind.TotalAMAOHAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHPercentChange, attNameExtension), ind.TotalAMAOHPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHBaseChange, attNameExtension), ind.TotalAMAOHBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMAOHBasePercentChange, attNameExtension), ind.TotalAMAOHBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMCAP, attNameExtension), ind.TotalAMCAP);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPAmountChange, attNameExtension), ind.TotalAMCAPAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPPercentChange, attNameExtension), ind.TotalAMCAPPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPBaseChange, attNameExtension), ind.TotalAMCAPBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMCAPBasePercentChange, attNameExtension), ind.TotalAMCAPBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMTOTAL, attNameExtension), ind.TotalAMTOTAL);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAmountChange, attNameExtension), ind.TotalAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalPercentChange, attNameExtension), ind.TotalPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalBaseChange, attNameExtension), ind.TotalBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalBasePercentChange, attNameExtension), ind.TotalBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMINCENT, attNameExtension), ind.TotalAMINCENT);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentAmountChange, attNameExtension), ind.TotalAMIncentAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentPercentChange, attNameExtension), ind.TotalAMIncentPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentBaseChange, attNameExtension), ind.TotalAMIncentBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMIncentBasePercentChange, attNameExtension), ind.TotalAMIncentBasePercentChange);
            }
            if (bIsBenefitNode || bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMR, attNameExtension), ind.TotalAMR);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRAmountChange, attNameExtension), ind.TotalAMRAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRPercentChange, attNameExtension), ind.TotalAMRPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRBaseChange, attNameExtension), ind.TotalAMRBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRBasePercentChange, attNameExtension), ind.TotalAMRBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TRAmount, attNameExtension), ind.TotalRAmount);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountChange, attNameExtension), ind.TotalRAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountPercentChange, attNameExtension), ind.TotalRAmountPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountBaseChange, attNameExtension), ind.TotalRAmountBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRAmountBasePercentChange, attNameExtension), ind.TotalRAmountBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMRINCENT, attNameExtension), ind.TotalAMRINCENT);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentAmountChange, attNameExtension), ind.TotalAMRIncentAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentPercentChange, attNameExtension), ind.TotalAMRIncentPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentBaseChange, attNameExtension), ind.TotalAMRIncentBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalAMRIncentBasePercentChange, attNameExtension), ind.TotalAMRIncentBasePercentChange);
            }
            if (bIsBoth)
            {
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNET, attNameExtension), ind.TotalAMNET);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETAmountChange, attNameExtension), ind.TotalAMNETAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETPercentChange, attNameExtension), ind.TotalAMNETPercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETBaseChange, attNameExtension), ind.TotalAMNETBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TAMNETBasePercentChange, attNameExtension), ind.TotalAMNETBasePercentChange);

                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(TRPrice, attNameExtension), ind.TotalRPrice);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPriceAmountChange, attNameExtension), ind.TotalRPriceAmountChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPricePercentChange, attNameExtension), ind.TotalRPricePercentChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPriceBaseChange, attNameExtension), ind.TotalRPriceBaseChange);
                CalculatorHelpers.SetAttributeDoubleF3(calculator,
                        string.Concat(cTotalRPriceBasePercentChange, attNameExtension), ind.TotalRPriceBasePercentChange);
            }
        }

        public void SetTotalNPV1Change1Attributes(NPV1Change1 ind,
            string attNameExtension, ref XmlWriter writer)
        {
            bool bIsCostNode = CalculatorHelpers.IsCostNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBenefitNode = CalculatorHelpers.IsBenefitNode(this.CalcParameters.CurrentElementNodeName);
            bool bIsBoth = (bIsBenefitNode == false && bIsCostNode == false) ? true : false;
            if (bIsCostNode || bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(TAMOC, attNameExtension), ind.TotalAMOC.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCAmountChange, attNameExtension), ind.TotalAMOCAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCPercentChange, attNameExtension), ind.TotalAMOCPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCBaseChange, attNameExtension), ind.TotalAMOCBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMOCBasePercentChange, attNameExtension), ind.TotalAMOCBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMAOH, attNameExtension), ind.TotalAMAOH.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHAmountChange, attNameExtension), ind.TotalAMAOHAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHPercentChange, attNameExtension), ind.TotalAMAOHPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHBaseChange, attNameExtension), ind.TotalAMAOHBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMAOHBasePercentChange, attNameExtension), ind.TotalAMAOHBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMCAP, attNameExtension), ind.TotalAMCAP.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPAmountChange, attNameExtension), ind.TotalAMCAPAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPPercentChange, attNameExtension), ind.TotalAMCAPPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPBaseChange, attNameExtension), ind.TotalAMCAPBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMCAPBasePercentChange, attNameExtension), ind.TotalAMCAPBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                   string.Concat(TAMTOTAL, attNameExtension), ind.TotalAMTOTAL.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAmountChange, attNameExtension), ind.TotalAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalPercentChange, attNameExtension), ind.TotalPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalBaseChange, attNameExtension), ind.TotalBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalBasePercentChange, attNameExtension), ind.TotalBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMINCENT, attNameExtension), ind.TotalAMINCENT.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentAmountChange, attNameExtension), ind.TotalAMIncentAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentPercentChange, attNameExtension), ind.TotalAMIncentPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentBaseChange, attNameExtension), ind.TotalAMIncentBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMIncentBasePercentChange, attNameExtension), ind.TotalAMIncentBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsBenefitNode || bIsBoth)
            {

                writer.WriteAttributeString(
                    string.Concat(TAMR, attNameExtension), ind.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRAmountChange, attNameExtension), ind.TotalAMRAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRPercentChange, attNameExtension), ind.TotalAMRPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRBaseChange, attNameExtension), ind.TotalAMRBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRBasePercentChange, attNameExtension), ind.TotalAMRBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TRAmount, attNameExtension), ind.TotalRAmount.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountChange, attNameExtension), ind.TotalRAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountPercentChange, attNameExtension), ind.TotalRAmountPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountBaseChange, attNameExtension), ind.TotalRAmountBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRAmountBasePercentChange, attNameExtension), ind.TotalRAmountBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TAMRINCENT, attNameExtension), ind.TotalAMRINCENT.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentAmountChange, attNameExtension), ind.TotalAMRIncentAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentPercentChange, attNameExtension), ind.TotalAMRIncentPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentBaseChange, attNameExtension), ind.TotalAMRIncentBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalAMRIncentBasePercentChange, attNameExtension), ind.TotalAMRIncentBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));

                writer.WriteAttributeString(
                    string.Concat(TRPrice, attNameExtension), ind.TotalRPrice.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPriceAmountChange, attNameExtension), ind.TotalRPriceAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPricePercentChange, attNameExtension), ind.TotalRPricePercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPriceBaseChange, attNameExtension), ind.TotalRPriceBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(cTotalRPriceBasePercentChange, attNameExtension), ind.TotalRPriceBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));
            }
            if (bIsBoth)
            {
                writer.WriteAttributeString(
                    string.Concat(TAMNET, attNameExtension), ind.TotalAMNET.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETAmountChange, attNameExtension), ind.TotalAMNETAmountChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETPercentChange, attNameExtension), ind.TotalAMNETPercentChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETBaseChange, attNameExtension), ind.TotalAMNETBaseChange.ToString("N2", CultureInfo.InvariantCulture));
                writer.WriteAttributeString(
                        string.Concat(TAMNETBasePercentChange, attNameExtension), ind.TotalAMNETBasePercentChange.ToString("N2", CultureInfo.InvariantCulture));
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
        //calcs holds the collections needing change analysis
        public bool RunAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            bool bHasAnalyses = false;
            //don't use npv1Stock totals, use calcs
            SetAnalyses(npv1Stock);
            //set calculated changestocks
            List<NPV1Stock> changeStocks = new List<NPV1Stock>();
            if (npv1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices
                || npv1Stock.CalcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                changeStocks = SetIOAnalyses(npv1Stock, calcs);
            }
            else
            {
                if (npv1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Input.INPUT_PRICE_TYPES.input.ToString())
                    || npv1Stock.CalcParameters.CurrentElementNodeName
                    .Contains(Output.OUTPUT_PRICE_TYPES.output.ToString()))
                {
                    //i/os only need totals, no changes
                    changeStocks = SetTotals(npv1Stock, calcs);
                }
                else
                {
                    changeStocks = SetAnalyses(npv1Stock, calcs);
                }
            }
            //add the changestocks to parent stock
            if (changeStocks != null)
            {
                bHasAnalyses = AddChangeStocksToBaseStock(npv1Stock, changeStocks);
                //npv1Stock must still add the members of change1s to the right ParentElement.Calculators
            }
            return bHasAnalyses;
        }
        private bool SetAnalyses(NPV1Stock npv1Stock)
        {
            bool bHasAnalysis = false;
            npv1Stock.Change1.CalcParameters = npv1Stock.CalcParameters;
            //totals were added to npv1stock, but those totals result 
            //in double counting when calcs are being summed
            //set them to zero
            npv1Stock.Change1.InitTotalBenefitsProperties();
            npv1Stock.Change1.InitTotalCostsProperties();
            npv1Stock.Change1.TotalRAmount = 0;
            //times is already in comp amount
            npv1Stock.Change1.TotalRCompositionAmount = 0;
            npv1Stock.Change1.TotalRPrice = 0;
            bHasAnalysis = true;
            return bHasAnalysis;
        }
       
        private List<NPV1Stock> SetTotals(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            List<NPV1Stock> changeStocks = new List<NPV1Stock>();
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
                        if (stock.Change1 != null)
                        {
                            //calc holds an input or output stock
                            //add that stock to npv1stock (some analyses will need to use subprices too)
                            bHasTotals = AddSubTotalToTotalStock(npv1Stock.Change1, npv1Stock.Multiplier, stock.Change1);
                            if (bHasTotals)
                            {
                                changeStocks.Add(npv1Stock);
                            }
                        }
                    }
                }
            }
            return changeStocks;
        }
        private List<NPV1Stock> SetAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            List<NPV1Stock> changeStocks = new List<NPV1Stock>();
            bool bHasTotals = false;
            //only the totStocks are used in results
            //calcs holds a collection of npv1stocks; stats run on calcs for every node
            //so budget holds tp stats; tp holds op/comp/outcome stats; 
            //but op/comp/outcome holds N number of observations defined by alternative2 property
            //object model is calc.Change1 for costs and 2s for benefits
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
                NPV1Stock observationStock = new NPV1Stock(npv1Stock.Change1.CalcParameters, npv1Stock.Change1.CalcParameters.AnalyzerParms.AnalyzerType);
                observationStock.Change1 = new NPV1Change1();
                observationStock.Change1.CalcParameters = new CalculatorParameters(npv1Stock.Change1.CalcParameters);
                foreach (Calculator1 calc in calcbyalt)
                {
                    if (calc.GetType().Equals(npv1Stock.GetType()))
                    {
                        NPV1Stock stock = (NPV1Stock)calc;
                        if (stock != null)
                        {
                            if (stock.Change1 != null)
                            {
                                NPV1Stock observation2Stock = new NPV1Stock(stock.Change1.CalcParameters, stock.Change1.CalcParameters.AnalyzerParms.AnalyzerType);
                                bHasTotals = SetObservationStock(changeStocks, calc,
                                    stock, observation2Stock);
                                if (bHasTotals)
                                {
                                    //add to the stats collection
                                    changeStocks.Add(observation2Stock);
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
            }
            if (iCostN > 0 || iBenefitN > 0)
            {
                bHasTotals = SetChangesAnalysis(changeStocks, npv1Stock, iCostN, iBenefitN);
            }
            return changeStocks;
        }
        private List<NPV1Stock> SetIOAnalyses(NPV1Stock npv1Stock, List<Calculator1> calcs)
        {
            List<NPV1Stock> changeStocks = new List<NPV1Stock>();
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
                        //stock.Change1 holds the initial substock/price totals
                        if (stock.Change1 != null)
                        {
                            NPV1Stock observation2Stock = new NPV1Stock(stock.Change1.CalcParameters, stock.Change1.CalcParameters.AnalyzerParms.AnalyzerType);
                            bHasTotals = SetObservationStock(changeStocks, calc, 
                                stock, observation2Stock);
                            if (bHasTotals)
                            {
                                //add to the stats collection
                                changeStocks.Add(observation2Stock);
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
                bHasTotals = SetChangesAnalysis(changeStocks, npv1Stock, iCostN2, iBenefitN2);
            }
            return changeStocks;
        }
        private bool SetObservationStock(List<NPV1Stock> changeStocks,
            Calculator1 calc, NPV1Stock stock, NPV1Stock observation2Stock)
        {
            bool bHasTotals = false;
            //set the calc totals in each time period
            //add a new stock for this new time period
            observation2Stock.Change1 = new NPV1Change1();
            observation2Stock.Id = stock.Id;
            observation2Stock.Change1.Id = stock.Id;
            //copy some stock props to progress1
            BINPV1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock.Change1);
            //copy progress1 properties
            //to parent stock so that general SetAnalyzerAtts can be set
            BINPV1StockAnalyzer.CopyBaseElementProperties(stock, observation2Stock);
            if (stock.Change1.CalcParameters == null)
                stock.Change1.CalcParameters = new CalculatorParameters();
            observation2Stock.Change1.CalcParameters = new CalculatorParameters(stock.CalcParameters);
            observation2Stock.CalcParameters = new CalculatorParameters(stock.CalcParameters);
            //stock.Change1.CalcParameters.CurrentElementNodeName = stock.CalcParameters.CurrentElementNodeName;
            //at oc and outcome level no aggregating by year, id or alt
            //calc.Multiplier not used because base calcs used it
            double dbMultiplier = 1;
            bHasTotals = AddSubTotalToTotalStock(observation2Stock.Change1,
                dbMultiplier, stock.Change1);
            return bHasTotals;
        }
        private bool SetChangesAnalysis(List<NPV1Stock> changeStocks, NPV1Stock npv1Stock,
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
                    bHasTotals = AddSubTotalToTotalStock(npv1Stock.Change1, 1, stat.Change1);
                }
                else
                {
                    if (!bHasCurrents)
                    {
                        //no changes?, straight totals needed
                        bHasTotals = AddSubTotalToTotalStock(npv1Stock.Change1, 1, stat.Change1);
                    }
                }
            }
            if (costN > 0)
            {
                //if any changestock has this property, it's trying to compare antecedents, rather than siblings
                if (bHasCurrents)
                {
                    //budgets uses antecendent, rather than sibling, comparators
                    SetAMOCBudgetChanges(npv1Stock, changeStocks);
                    SetAMAOHBudgetChanges(npv1Stock, changeStocks);
                    SetAMCAPBudgetChanges(npv1Stock, changeStocks);
                    SetTOTALBudgetChanges(npv1Stock, changeStocks);
                    SetAMIncentBudgetChanges(npv1Stock, changeStocks);
                    SetNetBudgetChanges(npv1Stock, changeStocks);
                    SetRPriceBudgetChanges(npv1Stock, changeStocks);
                }
                else
                {
                    //set change numbers
                    SetAMOCChanges(npv1Stock, changeStocks);
                    SetAMAOHChanges(npv1Stock, changeStocks);
                    SetAMCAPChanges(npv1Stock, changeStocks);
                    SetTOTALChanges(npv1Stock, changeStocks);
                    SetAMIncentChanges(npv1Stock, changeStocks);
                    SetNetChanges(npv1Stock, changeStocks);
                    SetRPriceChanges(npv1Stock, changeStocks);
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
                    SetRBudgetChanges(npv1Stock, changeStocks);
                    SetRAmountBudgetChanges(npv1Stock, changeStocks);
                    SetRPriceBudgetChanges(npv1Stock, changeStocks);
                    SetAMRIncentBudgetChanges(npv1Stock, changeStocks);
                }
                else
                {
                    SetRChanges(npv1Stock, changeStocks);
                    SetRAmountChanges(npv1Stock, changeStocks);
                    SetRPriceChanges(npv1Stock, changeStocks);
                    SetAMRIncentChanges(npv1Stock, changeStocks);
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

        public bool CopyTotalToChangeStock(NPV1Change1 totStock, NPV1Total1 subTotal)
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
       
        public bool AddSubTotalToTotalStock(NPV1Change1 totStock, double multiplier,
            NPV1Change1 subTotal)
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

        
        
        
        public static void ChangeSubTotalByMultipliers(NPV1Change1 subTotal, double multiplier)
        {
            if (multiplier == 0)
                multiplier = 1;
            subTotal.TotalAMOC = subTotal.TotalAMOC * multiplier;
            subTotal.TotalAMAOH = subTotal.TotalAMAOH * multiplier;
            subTotal.TotalAMCAP = subTotal.TotalAMCAP * multiplier;
            subTotal.TotalAMTOTAL = subTotal.TotalAMTOTAL * multiplier;
            subTotal.TotalAMINCENT = subTotal.TotalAMINCENT * multiplier;
            subTotal.TotalAMNET = subTotal.TotalAMNET * multiplier;
            subTotal.TotalAMR = subTotal.TotalAMR * multiplier;
            subTotal.TotalRAmount = subTotal.TotalRAmount * multiplier;
            subTotal.TotalAMRINCENT = subTotal.TotalAMRINCENT * multiplier;
            subTotal.TotalRPrice = subTotal.TotalRPrice * multiplier;
            
        }
        private static void SetAMOCBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMOCBaseChange = stat.Change1.TotalAMOC - benchmark.Change1.TotalAMOC; ;
                        stat.Change1.TotalAMOCBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMOCBaseChange, benchmark.Change1.TotalAMOC);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMOCAmountChange
                            = stat.Change1.TotalAMOC - xminus1.Change1.TotalAMOC;
                        stat.Change1.TotalAMOCPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMOCAmountChange, xminus1.Change1.TotalAMOC);
                    }
                }
            }
        }
        private static NPV1Stock GetChangeStockByLabel(NPV1Stock actual, List<int> ids,
            List<NPV1Stock> changeStocks, string changeType)
        {
            //won't get a true planned cumulative if the last actual can't find a matching planned
            //that's why benchmark planned must be displayed too
            NPV1Stock plannedMatch = null;
            //make sure the aggLabel can be matched in planned sequence
            if (changeStocks.Any(p => p.Label == actual.Label
                && p.ChangeType == changeType))
            {
                int iIndex = 1;
                foreach (NPV1Stock change in changeStocks)
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
            NPV1Stock change, List<NPV1Stock> changeStocks, string changeType)
        {
            bool bHasMatch = false;
            bool bStart = false;
            foreach (NPV1Stock rp in changeStocks)
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
        private static void SetAMAOHBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMAOHBaseChange = stat.Change1.TotalAMAOH - benchmark.Change1.TotalAMAOH; ;
                        stat.Change1.TotalAMAOHBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMAOHBaseChange, benchmark.Change1.TotalAMAOH);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMAOHAmountChange
                            = stat.Change1.TotalAMAOH - xminus1.Change1.TotalAMAOH;
                        stat.Change1.TotalAMAOHPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMAOHAmountChange, xminus1.Change1.TotalAMAOH);
                    }
                }
            }
        }
        private static void SetAMCAPBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMCAPBaseChange = stat.Change1.TotalAMCAP - benchmark.Change1.TotalAMCAP; ;
                        stat.Change1.TotalAMCAPBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMCAPBaseChange, benchmark.Change1.TotalAMCAP);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMCAPAmountChange
                            = stat.Change1.TotalAMCAP - xminus1.Change1.TotalAMCAP;
                        stat.Change1.TotalAMCAPPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMCAPAmountChange, xminus1.Change1.TotalAMCAP);
                    }
                }
            }
        }
        private static void SetTOTALBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalBaseChange = stat.Change1.TotalAMTOTAL - benchmark.Change1.TotalAMTOTAL; ;
                        stat.Change1.TotalBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalBaseChange, benchmark.Change1.TotalAMTOTAL);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAmountChange
                            = stat.Change1.TotalAMTOTAL - xminus1.Change1.TotalAMTOTAL;
                        stat.Change1.TotalPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAmountChange, xminus1.Change1.TotalAMTOTAL);
                    }
                }
            }
        }
        private static void SetAMIncentBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMIncentBaseChange = stat.Change1.TotalAMINCENT - benchmark.Change1.TotalAMINCENT; ;
                        stat.Change1.TotalAMIncentBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMIncentBaseChange, benchmark.Change1.TotalAMINCENT);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMIncentAmountChange
                            = stat.Change1.TotalAMINCENT - xminus1.Change1.TotalAMINCENT;
                        stat.Change1.TotalAMIncentPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMIncentAmountChange, xminus1.Change1.TotalAMINCENT);
                    }
                }
            }
        }
        private static void SetNetBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMNETBaseChange = stat.Change1.TotalAMNET - benchmark.Change1.TotalAMNET; ;
                        stat.Change1.TotalAMNETBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMNETBaseChange, benchmark.Change1.TotalAMNET);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMNETAmountChange
                            = stat.Change1.TotalAMNET - xminus1.Change1.TotalAMNET;
                        stat.Change1.TotalAMNETPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMNETAmountChange, xminus1.Change1.TotalAMNET);
                    }
                }
            }
        }
        private static void SetRBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMRBaseChange = stat.Change1.TotalAMR - benchmark.Change1.TotalAMR; ;
                        stat.Change1.TotalAMRBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMRBaseChange, benchmark.Change1.TotalAMR);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMRAmountChange
                            = stat.Change1.TotalAMR - xminus1.Change1.TotalAMR;
                        stat.Change1.TotalAMRPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMRAmountChange, xminus1.Change1.TotalAMR);
                    }
                }
            }
        }
        private static void SetRAmountBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalRAmountBaseChange = stat.Change1.TotalRAmount - benchmark.Change1.TotalRAmount; ;
                        stat.Change1.TotalRAmountBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRAmountBaseChange, benchmark.Change1.TotalRAmount);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalRAmountChange
                            = stat.Change1.TotalRAmount - xminus1.Change1.TotalRAmount;
                        stat.Change1.TotalRAmountPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRAmountChange, xminus1.Change1.TotalRAmount);
                    }
                }
            }
        }
        private static void SetAMRIncentBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalAMRIncentBaseChange = stat.Change1.TotalAMRINCENT - benchmark.Change1.TotalAMRINCENT; ;
                        stat.Change1.TotalAMRIncentBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMRIncentBaseChange, benchmark.Change1.TotalAMRINCENT);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalAMRIncentAmountChange
                            = stat.Change1.TotalAMRINCENT - xminus1.Change1.TotalAMRINCENT;
                        stat.Change1.TotalAMRIncentPercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalAMRIncentAmountChange, xminus1.Change1.TotalAMRINCENT);
                    }
                }
            }
        }
        private static void SetRPriceBudgetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            List<int> baseIds = new List<int>();
            List<int> xMinus1Ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                //actual compared to base comparator and xminus1 comparator
                if (stat.ChangeType == CHANGE_TYPES.current.ToString())
                {
                    //set the base change using benchmark tt
                    NPV1Stock benchmark = GetChangeStockByLabel(
                        stat, baseIds, changeStocks, Calculator1.CHANGE_TYPES.baseline.ToString());
                    if (benchmark != null)
                    {
                        stat.Change1.TotalRPriceBaseChange = stat.Change1.TotalRPrice - benchmark.Change1.TotalRPrice; ;
                        stat.Change1.TotalRPriceBasePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRPriceBaseChange, benchmark.Change1.TotalRPrice);
                    }
                    //set the xminus change using partialtarget tt
                    NPV1Stock xminus1 = GetChangeStockByLabel(
                        stat, xMinus1Ids, changeStocks, Calculator1.CHANGE_TYPES.xminus1.ToString());
                    if (xminus1 != null)
                    {
                        stat.Change1.TotalRPriceAmountChange
                            = stat.Change1.TotalRPrice - xminus1.Change1.TotalRPrice;
                        stat.Change1.TotalRPricePercentChange
                            = CalculatorHelpers.GetPercent(stat.Change1.TotalRPriceAmountChange, xminus1.Change1.TotalRPrice);
                    }
                }
            }
        }
        private static void SetAMOCChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMOC = 0;
            double dbLastTotalAMOC = 0;
            int i = 0;
            List<int> ids = new List<int>();
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    //no changes to display with first member
                    dbBaseTotalAMOC = stat.Change1.TotalAMOC;
                }
                else
                {
                    if (dbLastTotalAMOC != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMOCAmountChange = stat.Change1.TotalAMOC - dbLastTotalAMOC;
                        stat.Change1.TotalAMOCPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMOCAmountChange, dbLastTotalAMOC);
                    }
                    dbLastTotalAMOC = stat.Change1.TotalAMOC;

                    stat.Change1.TotalAMOCBaseChange = stat.Change1.TotalAMOC - dbBaseTotalAMOC;
                    stat.Change1.TotalAMOCBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMOCBaseChange, dbBaseTotalAMOC);
                }
                i++;
            }
        }
        private static void SetAMAOHChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMAOH = 0;
            double dbLastTotalAMAOH = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMAOH = stat.Change1.TotalAMAOH;
                }
                else
                {
                    if (dbLastTotalAMAOH != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMAOHAmountChange = stat.Change1.TotalAMAOH - dbLastTotalAMAOH;
                        stat.Change1.TotalAMAOHPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMAOHAmountChange, dbLastTotalAMAOH);
                    }
                    dbLastTotalAMAOH = stat.Change1.TotalAMAOH;
                    stat.Change1.TotalAMAOHBaseChange = stat.Change1.TotalAMAOH - dbBaseTotalAMAOH;
                    stat.Change1.TotalAMAOHBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMAOHBaseChange, dbBaseTotalAMAOH);
                }
                i++;
            }
        }
        private static void SetAMCAPChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMCAP = 0;
            double dbLastTotalAMCAP = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMCAP = stat.Change1.TotalAMCAP;
                }
                else
                {
                    if (dbLastTotalAMCAP != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMCAPAmountChange = stat.Change1.TotalAMCAP - dbLastTotalAMCAP;
                        stat.Change1.TotalAMCAPPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMCAPAmountChange, dbLastTotalAMCAP);
                    }
                    dbLastTotalAMCAP = stat.Change1.TotalAMCAP;
                    stat.Change1.TotalAMCAPBaseChange = stat.Change1.TotalAMCAP - dbBaseTotalAMCAP;
                    stat.Change1.TotalAMCAPBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMCAPBaseChange, dbBaseTotalAMCAP);
                }
                i++;
            }
        }
        private static void SetTOTALChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMTOTAL = 0;
            double dbLastTotalAMTOTAL = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMTOTAL = stat.Change1.TotalAMTOTAL;
                }
                else
                {
                    //set the annual change numbers
                    if (dbLastTotalAMTOTAL != 0)
                    {
                        stat.Change1.TotalAmountChange = stat.Change1.TotalAMTOTAL - dbLastTotalAMTOTAL;
                        stat.Change1.TotalPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAmountChange, dbLastTotalAMTOTAL);
                    }
                    dbLastTotalAMTOTAL = stat.Change1.TotalAMTOTAL;
                    stat.Change1.TotalBaseChange = stat.Change1.TotalAMTOTAL - dbBaseTotalAMTOTAL;
                    stat.Change1.TotalBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalBaseChange, dbBaseTotalAMTOTAL);
                }
                i++;
            }
        }
        private static void SetAMIncentChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMINCENT = 0;
            double dbLastTotalAMINCENT = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMINCENT = stat.Change1.TotalAMINCENT;
                }
                else
                {
                    if (dbLastTotalAMINCENT != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMIncentAmountChange = stat.Change1.TotalAMINCENT - dbLastTotalAMINCENT;
                        stat.Change1.TotalAMIncentPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMIncentAmountChange, dbLastTotalAMINCENT);
                    }
                    dbLastTotalAMINCENT = stat.Change1.TotalAMINCENT;
                    stat.Change1.TotalAMIncentBaseChange = stat.Change1.TotalAMINCENT - dbBaseTotalAMINCENT;
                    stat.Change1.TotalAMIncentBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMIncentBaseChange, dbBaseTotalAMINCENT);
                }
                i++;
            }
        }
        private static void SetNetChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMNET = 0;
            double dbLastTotalAMNET = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMNET = stat.Change1.TotalAMNET;
                }
                else
                {
                    if (dbLastTotalAMNET != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMNETAmountChange = stat.Change1.TotalAMNET - dbLastTotalAMNET;
                        stat.Change1.TotalAMNETPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMNETAmountChange, dbLastTotalAMNET);
                    }
                    dbLastTotalAMNET = stat.Change1.TotalAMNET;
                    stat.Change1.TotalAMNETBaseChange = stat.Change1.TotalAMNET - dbBaseTotalAMNET;
                    stat.Change1.TotalAMNETBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMNETBaseChange, dbBaseTotalAMNET);
                }
                i++;
            }
        }
        private static void SetRChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMR = 0;
            double dbLastTotalAMR = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMR = stat.Change1.TotalAMR;
                }
                else
                {
                    if (dbLastTotalAMR != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMRAmountChange = stat.Change1.TotalAMR - dbLastTotalAMR;
                        stat.Change1.TotalAMRPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMRAmountChange, dbLastTotalAMR);
                    }
                    dbLastTotalAMR = stat.Change1.TotalAMR;
                    stat.Change1.TotalAMRBaseChange = stat.Change1.TotalAMR - dbBaseTotalAMR;
                    stat.Change1.TotalAMRBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMRBaseChange, dbBaseTotalAMR);
                }
                i++;
            }
        }
        private static void SetRAmountChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalRAmount = 0;
            double dbLastTotalRAmount = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalRAmount = stat.Change1.TotalRAmount;
                }
                else
                {
                    if (dbLastTotalRAmount != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalRAmountChange = stat.Change1.TotalRAmount - dbLastTotalRAmount;
                        stat.Change1.TotalRAmountPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRAmountChange, dbLastTotalRAmount);
                    }
                    dbLastTotalRAmount = stat.Change1.TotalRAmount;
                    //set the change from first tp numbers
                    stat.Change1.TotalRAmountBaseChange = stat.Change1.TotalRAmount - dbBaseTotalRAmount;
                    stat.Change1.TotalRAmountBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRAmountBaseChange, dbBaseTotalRAmount);
                }
                i++;
            }
        }
        private static void SetAMRIncentChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalAMRINCENT = 0;
            double dbLastTotalAMRINCENT = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalAMRINCENT = stat.Change1.TotalAMRINCENT;
                }
                else
                {
                    if (dbLastTotalAMRINCENT != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalAMRIncentAmountChange = stat.Change1.TotalAMRINCENT - dbLastTotalAMRINCENT;
                        stat.Change1.TotalAMRIncentPercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMRIncentAmountChange, dbLastTotalAMRINCENT);
                    }
                    dbLastTotalAMRINCENT = stat.Change1.TotalAMRINCENT;
                    stat.Change1.TotalAMRIncentBaseChange = stat.Change1.TotalAMRINCENT - dbBaseTotalAMRINCENT;
                    stat.Change1.TotalAMRIncentBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalAMRIncentBaseChange, dbBaseTotalAMRINCENT);
                }
                i++;
            }
        }
        private static void SetRPriceChanges(NPV1Stock npv1Stock, List<NPV1Stock> changeStocks)
        {
            double dbBaseTotalRPrice = 0;
            double dbLastTotalRPrice = 0;
            int i = 0;
            foreach (NPV1Stock stat in changeStocks)
            {
                if (i == 0)
                {
                    dbBaseTotalRPrice = stat.Change1.TotalRPrice;
                }
                else
                {
                    if (dbLastTotalRPrice != 0)
                    {
                        //set the annual change numbers
                        stat.Change1.TotalRPriceAmountChange = stat.Change1.TotalRPrice - dbLastTotalRPrice;
                        stat.Change1.TotalRPricePercentChange
                            = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRPriceAmountChange, dbLastTotalRPrice);
                    }
                    dbLastTotalRPrice = stat.Change1.TotalRPrice;
                    stat.Change1.TotalRPriceBaseChange = stat.Change1.TotalRPrice - dbBaseTotalRPrice;
                    stat.Change1.TotalRPriceBasePercentChange
                        = CalculatorHelpers.GetPercentUseIndex(i, stat.Change1.TotalRPriceBaseChange, dbBaseTotalRPrice);
                }
                i++;
            }
        }
        private bool AddChangeStocksToBaseStock(NPV1Stock npv1Stock,
            List<NPV1Stock> changeStocks)
        {
            bool bHasAnalyses = false;
            npv1Stock.Stocks = new List<NPV1Stock>();
            foreach (NPV1Stock changeStock in changeStocks)
            {
                //add it to the list
                npv1Stock.Stocks.Add(changeStock);
                bHasAnalyses = true;
            }
            return bHasAnalyses;
        }
    }
}