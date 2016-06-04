using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

using DevTreksAppHelpers = DevTreks.Data.AppHelpers;
using DevTreksHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The CostBenefitCalculator class is a base class used 
    ///             by most cost and benefit calculators/analyzers to hold 
    ///             cost and benefit totals. The virtual methods are meant 
    ///             to be overridden because some analyses, due to file size 
    ///             and performance issues, need to limit the properties 
    ///             used in an object and subsequently deserialized to 
    ///             an xelement's attributes.
    ///Author:		www.devtreks.org
    ///Date:		2013, December
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Derived classes usually extend this class with additional 
    ///             cost and benefit properties, such as statistical properties.
    ///             2. Price calculators focus on the Price and Amount properties.
    ///             3. Data definition documents will become available during 
    ///             late-stage testing. In the meantime, R = Benefits, 
    ///             OC = Operating Costs, AOH = Allocated Overhead Costs, 
    ///             CAP = Stock (i.e. Capital) Costs, INCENT = Incentive-adjusted 
    ///             benefits, costs, and nets, AM = Annual (amortized).
    ///</summary>            
    public class CostBenefitCalculator : Calculator1
    {
        public CostBenefitCalculator() 
        {
            InitSharedObjectProperties();
            InitTotalBenefitsProperties();
            InitTotalCostsProperties();
        }
        //copy constructor
        public CostBenefitCalculator(CostBenefitCalculator calculator) 
        {
            CopyCalculatorProperties(calculator);
            CopyTotalBenefitsProperties(calculator);
            CopyTotalCostsProperties(calculator);
        }

        //benefits
        public const string TRPrice = "TRPrice";
        public const string TRAmount = "TRAmount";
        public const string TRName = "TRName";
        public const string TRUnit = "TRUnit";
        public const string TRCompositionAmount = "TRCompositionAmount";
        public const string TRCompositionUnit = "TRCompositionUnit";
        public const string TR = "TR";
        public const string TR_INT = "TR_INT";
        public const string TRINCENT = "TRINCENT";
        public const string TAMR = "TAMR";
        public const string TAMR_INT = "TAMR_INT";
        public const string TAMRINCENT = "TAMRINCENT";

        //costs
        public const string TOCAmount = "TOCAmount";
        public const string TOCPrice = "TOCPrice";
        public const string TOCUnit = "TOCUnit";
        public const string TOC = "TOC";
        public const string TOC_INT = "TOC_INT";
        public const string TOCName = "TOCName";

        public const string TAOHAmount = "TAOHAmount";
        public const string TAOHPrice = "TAOHPrice";
        public const string TAOHUnit = "TAOHUnit";
        public const string TAOH = "TAOH";
        public const string TAOH_INT = "TAOH_INT";
        public const string TCAPAmount = "TCAPAmount";
        public const string TCAPPrice = "TCAPPrice";
        public const string TCAPUnit = "TCAPUnit";
        public const string TCAP = "TCAP";
        public const string TCAP_INT = "TCAP_INT";
        //incentives
        public const string TINCENT = "TINCENT";
        //amortized costs
        public const string TAMOC = "TAMOC";
        public const string TAMAOH = "TAMAOH";
        public const string TAMCAP = "TAMCAP";
        public const string TAMOC_INT = "TAMOC_INT";
        public const string TAMAOH_INT = "TAMAOH_INT";
        public const string TAMCAP_INT = "TAMCAP_INT";
        public const string TAMOC_NET = "TAMOC_NET";
        public const string TAMAOH_NET = "TAMAOH_NET";
        public const string TAMAOH_NET2 = "TAMAOH_NET2";
        public const string TAMCAP_NET = "TAMCAP_NET";
        public const string TAMINCENT = "TAMINCENT";
        public const string TAMINCENT_NET = "TAMINCENT_NET";
        //oc + aoh + cap
        public const string TAMTOTAL = "TAMTOTAL";
        //tr - oc + aoh + cap
        public const string TAMNET = "TAMNET";
        
        //ancestors setting calcs for descendants sometimes use this attribute
        public const string TYPE_NEWCALCS = "NewCalculations";

        public double TotalRPrice { get; set; }
        public double TotalRAmount { get; set; }
        public string TotalRUnit { get; set; }
        public string TotalRName { get; set; }
        public double TotalRCompositionAmount { get; set; }
        public string TotalRCompositionUnit { get; set; }
        public double TotalR { get; set; }
        public double TotalR_INT { get; set; }
        public double TotalRINCENT { get; set; }
        public double TotalAMR { get; set; }
        public double TotalAMR_INT { get; set; }
        public double TotalAMRINCENT { get; set; }

        public string TotalOCUnit { get; set; }
        public double TotalOCPrice { get; set; }
        public double TotalOCAmount { get; set; }
        public double TotalOC { get; set; }
        public string TotalOCName { get; set; }
        public string TotalAOHUnit { get; set; }
        public double TotalAOHPrice { get; set; }
        public double TotalAOHAmount { get; set; }
        public double TotalAOH { get; set; }
        public string TotalCAPUnit { get; set; }
        public double TotalCAPPrice { get; set; }
        public double TotalCAPAmount { get; set; }
        public double TotalCAP { get; set; }
        public double TotalOC_INT { get; set; }
        public double TotalAOH_INT { get; set; }
        public double TotalCAP_INT { get; set; }

        public double TotalINCENT { get; set; }

        public double TotalAMOC { get; set; }
        public double TotalAMAOH { get; set; }
        public double TotalAMCAP { get; set; }
        public double TotalAMOC_INT { get; set; }
        public double TotalAMAOH_INT { get; set; }
        public double TotalAMCAP_INT { get; set; }
        public double TotalAMOC_NET { get; set; }
        public double TotalAMAOH_NET { get; set; }
        public double TotalAMAOH_NET2 { get; set; }
        public double TotalAMCAP_NET { get; set; }
        public double TotalAMINCENT { get; set; }
        public double TotalAMINCENT_NET { get; set; }
        public double TotalAnnuities { get; set; }
        //oc + aoh + cap
        public double TotalAMTOTAL { get; set; }
        public double TotalAMTOTAL_MEAN { get; set; }
        public double TotalAMTOTAL_MED { get; set; }
        public double TotalAMTOTAL_VAR2 { get; set; }
        public double TotalAMTOTAL_SD { get; set; }
        //tr - oc + aoh + cap
        public double TotalAMNET { get; set; }
        public double TotalAMNET_MEAN { get; set; }
        public double TotalAMNET_MED { get; set; }
        public double TotalAMNET_VAR2 { get; set; }
        public double TotalAMNET_SD { get; set; }

        public static bool NameIsAStatistic(string attName)
        {
            bool bNameIsAStatistic = false;
            //all totals and derived statistics must follow 
            //the convention of starting their attribute name with "T"
            //so that no unnecessary properties are mathematically analyzed
            if (attName.StartsWith("T"))
            {
                //refactor: override this method in the CE comparative analysis class
                //and use that method, remove this from here
                //ce comparative analyses need two more total atts that are not stats
                if (attName.Equals(CostBenefitStatistic01.TRName)
                    || attName.Equals(CostBenefitStatistic01.TRUnit))
                {
                    bNameIsAStatistic = false;
                }
                else
                {
                    bNameIsAStatistic = true;
                }
            }
            return bNameIsAStatistic;
        }
        
        public virtual void CopyTotalCostsProperties(
            CostBenefitCalculator calculator)
        {
            this.TotalOCUnit = calculator.TotalOCUnit;
            this.TotalOCAmount = calculator.TotalOCAmount;
            this.TotalOCPrice = calculator.TotalOCPrice;
            this.TotalOC = calculator.TotalOC;
            this.TotalOC_INT = calculator.TotalOC_INT;
            this.TotalOCName = calculator.TotalOCName;
            this.TotalAOHUnit = calculator.TotalAOHUnit;
            this.TotalAOHAmount = calculator.TotalAOHAmount;
            this.TotalAOHPrice = calculator.TotalAOHPrice;
            this.TotalAOH = calculator.TotalAOH;
            this.TotalAOH_INT = calculator.TotalAOH_INT;
            this.TotalCAPUnit = calculator.TotalCAPUnit;
            this.TotalCAPAmount = calculator.TotalCAPAmount;
            this.TotalCAPPrice = calculator.TotalCAPPrice;
            this.TotalCAP = calculator.TotalCAP;
            this.TotalCAP_INT = calculator.TotalCAP_INT;

            this.TotalINCENT = calculator.TotalINCENT;

            this.TotalAMOC = calculator.TotalAMOC;
            this.TotalAMOC_INT = calculator.TotalAMOC_INT;
            this.TotalAMOC_NET = calculator.TotalAMOC_NET;
            this.TotalAMAOH = calculator.TotalAMAOH;
            this.TotalAMAOH_INT = calculator.TotalAMAOH_INT;
            this.TotalAMAOH_NET = calculator.TotalAMAOH_NET;
            this.TotalAMAOH_NET2 = calculator.TotalAMAOH_NET2;
            this.TotalAMCAP = calculator.TotalAMCAP;
            this.TotalAMCAP_INT = calculator.TotalAMCAP_INT;
            this.TotalAMCAP_NET = calculator.TotalAMCAP_NET;
            this.TotalAMINCENT = calculator.TotalAMINCENT;
            this.TotalAMINCENT_NET = calculator.TotalAMINCENT_NET;
            this.TotalAMNET = calculator.TotalAMNET;
            this.TotalAMTOTAL = calculator.TotalAMTOTAL;
            this.TotalAnnuities = calculator.TotalAnnuities;
        }
        public virtual void InitTotalCostsProperties()
        {
            //init totals to zero prior to running calculations
            this.TotalOCUnit = string.Empty;
            this.TotalOCAmount = 0;
            this.TotalOCPrice = 0;
            this.TotalOC = 0;
            this.TotalOC_INT = 0;
            this.TotalOCName = string.Empty;
            this.TotalAOHUnit = string.Empty;
            this.TotalAOHAmount = 0;
            this.TotalAOHPrice = 0;
            this.TotalAOH = 0;
            this.TotalAOH_INT = 0;
            this.TotalCAPUnit = string.Empty;
            this.TotalCAPAmount = 0;
            this.TotalCAPPrice = 0;
            this.TotalCAP = 0;
            this.TotalCAP_INT = 0;
            this.TotalINCENT = 0;

            this.TotalAMOC = 0;
            this.TotalAMOC_INT = 0;
            this.TotalAMOC_NET = 0;
            this.TotalAMAOH = 0;
            this.TotalAMAOH_INT = 0;
            this.TotalAMAOH_NET = 0;
            this.TotalAMAOH_NET2 = 0;
            this.TotalAMCAP = 0;
            this.TotalAMCAP_INT = 0;
            this.TotalAMCAP_NET = 0;
            this.TotalAMINCENT = 0;
            this.TotalAMINCENT_NET = 0;
            this.TotalAMNET = 0;
            this.TotalAMTOTAL = 0;
        }
        public virtual void SetTotalCostsProperties(XElement calculator)
        {
            this.TotalOC = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC);
            this.TotalOC_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TOC_INT);
            this.TotalAOH = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH);
            this.TotalAOH_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOH_INT);
            this.TotalCAP = CalculatorHelpers.GetAttributeDouble(calculator,
                 TCAP);
            this.TotalCAP_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAP_INT);
            this.TotalINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                TINCENT);

            this.TotalAMOC = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC);
            this.TotalAMOC_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_INT);
            this.TotalAMOC_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC_NET);
            this.TotalAMAOH = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH);
            this.TotalAMAOH_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_INT);
            this.TotalAMAOH_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET);
            this.TotalAMAOH_NET2 = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH_NET2);
            this.TotalAMCAP = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP);
            this.TotalAMCAP_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_INT);
            this.TotalAMCAP_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP_NET);
            this.TotalAMINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT);
            this.TotalAMINCENT_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT_NET);
            this.TotalAMNET = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMNET);
            this.TotalAMTOTAL = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMTOTAL);
        }
        public virtual void SetTotalCostsPsandQsProperties(XElement calculator)
        {
            //price analysis but not tech analysis
            this.TotalOCUnit = CalculatorHelpers.GetAttribute(calculator,
                TOCUnit);
            this.TotalOCAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCAmount);
            this.TotalOCPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TOCPrice);
            this.TotalOCName = CalculatorHelpers.GetAttribute(calculator,
               TOCName);
            this.TotalAOHUnit = CalculatorHelpers.GetAttribute(calculator,
                TAOHUnit);
            this.TotalAOHAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHAmount);
            this.TotalAOHPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TAOHPrice);
            this.TotalCAPUnit = CalculatorHelpers.GetAttribute(calculator,
                TCAPUnit);
            this.TotalCAPAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPAmount);
            this.TotalCAPPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TCAPPrice);
        }
        public virtual void SetTotalCostsSummaryProperties(XElement calculator)
        {
            this.TotalAMOC = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMOC);
            this.TotalAMAOH = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMAOH);
            this.TotalAMCAP = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMCAP);
            this.TotalAMINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMINCENT);
            this.TotalAMTOTAL = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMTOTAL);
        }
        public virtual void SetTotalCostsProperties(string attNameExtension, XElement calculator)
        {
            this.TotalOC = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TOC, attNameExtension));
            this.TotalOC_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TOC_INT, attNameExtension));
            this.TotalAOH = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAOH, attNameExtension));
            this.TotalAOH_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAOH_INT, attNameExtension));
            this.TotalCAP = CalculatorHelpers.GetAttributeDouble(calculator,
                 string.Concat(TCAP, attNameExtension));
            this.TotalCAP_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TCAP_INT, attNameExtension));

            this.TotalINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TINCENT, attNameExtension));

            this.TotalAMOC = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMOC, attNameExtension));
            this.TotalAMOC_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMOC_INT, attNameExtension));
            this.TotalAMOC_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMOC_NET, attNameExtension));
            this.TotalAMAOH = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMAOH, attNameExtension));
            this.TotalAMAOH_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMAOH_INT, attNameExtension));
            this.TotalAMAOH_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMAOH_NET, attNameExtension));
            this.TotalAMAOH_NET2 = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMAOH_NET2, attNameExtension));
            this.TotalAMCAP = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMCAP, attNameExtension));
            this.TotalAMCAP_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMCAP_INT, attNameExtension));
            this.TotalAMCAP_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMCAP_NET, attNameExtension));
            this.TotalAMINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMINCENT, attNameExtension));
            this.TotalAMINCENT_NET = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMINCENT_NET, attNameExtension));
            this.TotalAMNET = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMNET, attNameExtension));
            this.TotalAMTOTAL = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMTOTAL, attNameExtension));
        }
        public virtual void SetTotalCostsPsandQsProperties(string attNameExtension, XElement calculator)
        {
            //price analysis
            this.TotalOCUnit = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TOCUnit, attNameExtension));
            this.TotalOCAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TOCAmount, attNameExtension));
            this.TotalOCPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TOCPrice, attNameExtension));
            this.TotalOCName = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TOCName, attNameExtension));
            this.TotalAOHUnit = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TAOHUnit, attNameExtension));
            this.TotalAOHAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAOHAmount, attNameExtension));
            this.TotalAOHPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAOHPrice, attNameExtension));
            this.TotalCAPUnit = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TCAPUnit, attNameExtension));
            this.TotalCAPAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TCAPAmount, attNameExtension));
            this.TotalCAPPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TCAPPrice, attNameExtension));
        }
        public virtual void SetTotalCostsSummaryProperties(string attNameExtension, XElement calculator)
        {
            this.TotalAMOC = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMOC, attNameExtension));
            this.TotalAMAOH = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMAOH, attNameExtension));
            this.TotalAMCAP = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMCAP, attNameExtension));
            this.TotalAMINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMINCENT, attNameExtension));
            this.TotalAMTOTAL = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMTOTAL, attNameExtension));
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetTotalCostsProperty(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TOCUnit:
                    this.TotalOCUnit = attValue;
                    break;
                case TOCAmount:
                    this.TotalOCAmount
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCPrice:
                    this.TotalOCPrice
                       = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC:
                    this.TotalOC
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOC_INT:
                    this.TotalOC_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TOCName:
                    this.TotalOCName = attValue;
                    break;
                case TAOHUnit:
                    this.TotalAOHUnit = attValue;
                    break;
                case TAOHAmount:
                    this.TotalAOHAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOHPrice:
                    this.TotalAOHPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH:
                    this.TotalAOH = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAOH_INT:
                    this.TotalAOH_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPUnit:
                    this.TotalCAPUnit = attValue;
                    break;
                case TCAPAmount:
                    this.TotalCAPAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAPPrice:
                    this.TotalCAPPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP:
                    this.TotalCAP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TCAP_INT:
                    this.TotalCAP_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TINCENT:
                    this.TotalINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;

                case TAMOC:
                    this.TotalAMOC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_INT:
                    this.TotalAMOC_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMOC_NET:
                    this.TotalAMOC_NET = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH:
                    this.TotalAMAOH
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_INT:
                    this.TotalAMAOH_INT
                        = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET:
                    this.TotalAMAOH_NET
                       = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMAOH_NET2:
                    this.TotalAMAOH_NET2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP:
                    this.TotalAMCAP = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_INT:
                    this.TotalAMCAP_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMCAP_NET:
                    this.TotalAMCAP_NET = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT:
                    this.TotalAMINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMINCENT_NET:
                    this.TotalAMINCENT_NET = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMNET:
                    this.TotalAMNET = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMTOTAL:
                    this.TotalAMTOTAL = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalCostsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TOCUnit:
                    sPropertyValue = this.TotalOCUnit;
                    break;
                case TOCAmount:
                    sPropertyValue = this.TotalOCAmount.ToString();
                    break;
                case TOCPrice:
                    sPropertyValue = this.TotalOCPrice.ToString();
                    break;
                case TOC:
                    sPropertyValue = this.TotalOC.ToString();
                    break;
                case TOC_INT:
                    sPropertyValue = this.TotalOC_INT.ToString();
                    break;
                case TOCName:
                    sPropertyValue = this.TotalOCName;
                    break;
                case TAOHUnit:
                    sPropertyValue = this.TotalAOHUnit;
                    break;
                case TAOHAmount:
                    sPropertyValue = this.TotalAOHAmount.ToString();
                    break;
                case TAOHPrice:
                    sPropertyValue = this.TotalAOHPrice.ToString();
                    break;
                case TAOH:
                    sPropertyValue = this.TotalAOH.ToString();
                    break;
                case TAOH_INT:
                    sPropertyValue = this.TotalAOH_INT.ToString();
                    break;
                case TCAPUnit:
                    sPropertyValue = this.TotalCAPUnit;
                    break;
                case TCAPAmount:
                    sPropertyValue = this.TotalCAPAmount.ToString();
                    break;
                case TCAPPrice:
                    sPropertyValue = this.TotalCAPPrice.ToString();
                    break;
                case TCAP:
                    sPropertyValue = this.TotalCAP.ToString();
                    break;
                case TCAP_INT:
                    sPropertyValue = this.TotalCAP_INT.ToString();
                    break;

                case TINCENT:
                    sPropertyValue = this.TotalINCENT.ToString();
                    break;

                case TAMOC:
                    sPropertyValue = this.TotalAMOC.ToString();
                    break;
                case TAMOC_INT:
                    sPropertyValue = this.TotalAMOC_INT.ToString();
                    break;
                case TAMOC_NET:
                    sPropertyValue = this.TotalAMOC_NET.ToString();
                    break;
                case TAMAOH:
                    sPropertyValue = this.TotalAMAOH.ToString();
                    break;
                case TAMAOH_INT:
                    sPropertyValue = this.TotalAMAOH_INT.ToString();
                    break;
                case TAMAOH_NET:
                    sPropertyValue = this.TotalAMAOH_NET.ToString();
                    break;
                case TAMAOH_NET2:
                    sPropertyValue = this.TotalAMAOH_NET2.ToString();
                    break;
                case TAMCAP:
                    sPropertyValue = this.TotalAMCAP.ToString();
                    break;
                case TAMCAP_INT:
                    sPropertyValue = this.TotalAMCAP_INT.ToString();
                    break;
                case TAMCAP_NET:
                    sPropertyValue = this.TotalAMCAP_NET.ToString();
                    break;
                case TAMINCENT:
                    sPropertyValue = this.TotalAMINCENT.ToString();
                    break;
                case TAMINCENT_NET:
                    sPropertyValue = this.TotalAMINCENT_NET.ToString();
                    break;
                case TAMNET:
                    sPropertyValue = this.TotalAMNET.ToString();
                    break;
                case TAMTOTAL:
                    sPropertyValue = this.TotalAMTOTAL.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalCostsAttributes(string attNameExtension, 
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TOC, attNameExtension),
                this.TotalOC);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TOC_INT, attNameExtension),
                this.TotalOC_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAOH, attNameExtension),
                this.TotalAOH);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAOH_INT, attNameExtension),
                this.TotalAOH_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TCAP, attNameExtension),
                this.TotalCAP);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TCAP_INT, attNameExtension),
                this.TotalCAP_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TINCENT, attNameExtension),
                this.TotalINCENT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMOC, attNameExtension),
                this.TotalAMOC);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMOC_INT, attNameExtension),
                this.TotalAMOC_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMOC_NET, attNameExtension),
                this.TotalAMOC_NET);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMAOH, attNameExtension),
                this.TotalAMAOH);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMAOH_INT, attNameExtension),
                this.TotalAMAOH_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMAOH_NET, attNameExtension),
                this.TotalAMAOH_NET);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMAOH_NET2, attNameExtension),
                this.TotalAMAOH_NET2);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMCAP, attNameExtension),
                this.TotalAMCAP);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMCAP_INT, attNameExtension),
                this.TotalAMCAP_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMCAP_NET, attNameExtension),
                this.TotalAMCAP_NET);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMINCENT, attNameExtension),
                this.TotalAMINCENT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMINCENT_NET, attNameExtension),
                this.TotalAMINCENT_NET);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMNET, attNameExtension),
                this.TotalAMNET);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMTOTAL, attNameExtension),
                this.TotalAMTOTAL);
        }
        public virtual void SetTotalCostsPsandQsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //price analysis
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(TOCUnit, attNameExtension),
                 this.TotalOCUnit);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TOCAmount, attNameExtension),
                this.TotalOCAmount);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TOCPrice, attNameExtension),
                this.TotalOCPrice);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(TOCName, attNameExtension),
                this.TotalOCName);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(TAOHUnit, attNameExtension),
                this.TotalAOHUnit);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAOHAmount, attNameExtension),
                this.TotalAOHAmount);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAOHPrice, attNameExtension),
                this.TotalAOHPrice);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(TCAPUnit, attNameExtension),
                this.TotalCAPUnit);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TCAPAmount, attNameExtension),
                this.TotalCAPAmount);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TCAPPrice, attNameExtension),
                this.TotalCAPPrice);
        }
        public virtual void SetTotalCostsSummaryAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMOC, attNameExtension),
                this.TotalAMOC);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMAOH, attNameExtension),
                this.TotalAMAOH);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMCAP, attNameExtension),
                this.TotalAMCAP);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMINCENT, attNameExtension),
                this.TotalAMINCENT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMTOTAL, attNameExtension),
                this.TotalAMTOTAL);
        }
        public virtual void SetTotalCostsSummaryNetsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMINCENT_NET, attNameExtension),
                this.TotalAMINCENT_NET);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMNET, attNameExtension),
                this.TotalAMNET);
        }
        public virtual void SetTotalCostsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TOC, attNameExtension),
                this.TotalOC.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOC_INT, attNameExtension),
                this.TotalOC_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH, attNameExtension),
                this.TotalAOH.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOH_INT, attNameExtension),
                this.TotalAOH_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP, attNameExtension),
                this.TotalCAP.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAP_INT, attNameExtension),
                this.TotalCAP_INT.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TINCENT, attNameExtension),
                this.TotalINCENT.ToString("N2", CultureInfo.InvariantCulture));

            writer.WriteAttributeString(
                string.Concat(TAMOC, attNameExtension),
                this.TotalAMOC.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_INT, attNameExtension),
                this.TotalAMOC_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMOC_NET, attNameExtension),
                this.TotalAMOC_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH, attNameExtension),
                this.TotalAMAOH.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_INT, attNameExtension),
                this.TotalAMAOH_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET, attNameExtension),
                this.TotalAMAOH_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH_NET2, attNameExtension),
                this.TotalAMAOH_NET2.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP, attNameExtension),
                this.TotalAMCAP.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_INT, attNameExtension),
                this.TotalAMCAP_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP_NET, attNameExtension),
                this.TotalAMCAP_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT, attNameExtension),
                this.TotalAMINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET, attNameExtension),
                this.TotalAMINCENT_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMNET, attNameExtension),
                this.TotalAMNET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMTOTAL, attNameExtension),
                this.TotalAMTOTAL.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void SetTotalCostsPsandQsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            //price analysis
            writer.WriteAttributeString(
                string.Concat(TOCUnit, attNameExtension),
                this.TotalOCUnit);
            writer.WriteAttributeString(
                string.Concat(TOCAmount, attNameExtension),
                this.TotalOCAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice, attNameExtension),
                this.TotalOCPrice.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TOCName, attNameExtension),
               this.TotalOCName);
            writer.WriteAttributeString(
                string.Concat(TAOHUnit, attNameExtension),
                this.TotalAOHUnit);
            writer.WriteAttributeString(
                string.Concat(TAOHAmount, attNameExtension),
                this.TotalAOHAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAOHPrice, attNameExtension),
                this.TotalAOHPrice.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPUnit, attNameExtension),
                this.TotalCAPUnit);
            writer.WriteAttributeString(
                string.Concat(TCAPAmount, attNameExtension),
                this.TotalCAPAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TCAPPrice, attNameExtension),
                this.TotalCAPPrice.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void SetTotalOCPricePsandQsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            //limited price analysis 
            writer.WriteAttributeString(
               string.Concat(TOCName, attNameExtension),
               this.TotalOCName);
            writer.WriteAttributeString(
                string.Concat(TOCUnit, attNameExtension),
                this.TotalOCUnit);
            writer.WriteAttributeString(
                string.Concat(TOCAmount, attNameExtension),
                this.TotalOCAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TOCPrice, attNameExtension),
                this.TotalOCPrice.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void SetTotalCostsSummaryAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TAMOC, attNameExtension),
                this.TotalAMOC.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMAOH, attNameExtension),
                this.TotalAMAOH.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMCAP, attNameExtension),
                this.TotalAMCAP.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMINCENT, attNameExtension),
                this.TotalAMINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMTOTAL, attNameExtension),
                this.TotalAMTOTAL.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void SetTotalCostsSummaryNetsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TAMINCENT_NET, attNameExtension),
                this.TotalAMINCENT_NET.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMNET, attNameExtension),
                this.TotalAMNET.ToString("N2", CultureInfo.InvariantCulture));
            //watch NET2
        }
        public virtual void CopyTotalBenefitsProperties(
            CostBenefitCalculator calculator)
        {
            this.TotalRPrice = calculator.TotalRPrice;
            this.TotalRAmount = calculator.TotalRAmount;
            this.TotalRUnit = calculator.TotalRUnit;
            this.TotalRName = calculator.TotalRName;
            this.TotalRCompositionAmount = calculator.TotalRCompositionAmount;
            this.TotalRCompositionUnit = calculator.TotalRCompositionUnit;
            this.TotalR = calculator.TotalR;
            this.TotalR_INT = calculator.TotalR_INT;
            this.TotalRINCENT = calculator.TotalRINCENT;
            this.TotalAMR = calculator.TotalAMR;
            this.TotalAMR_INT = calculator.TotalAMR_INT;
            this.TotalAMRINCENT = calculator.TotalAMRINCENT;
        }
        public virtual void InitTotalBenefitsProperties()
        {
            this.TotalRPrice = 0;
            this.TotalRAmount = 0;
            this.TotalRUnit = string.Empty;
            this.TotalRName = string.Empty;
            this.TotalRCompositionAmount = 0;
            this.TotalRCompositionUnit = string.Empty;
            this.TotalR = 0;
            this.TotalR_INT = 0;
            this.TotalRINCENT = 0;
            this.TotalAMR = 0;
            this.TotalAMR_INT = 0;
            this.TotalAMRINCENT = 0;
            this.TotalAnnuities = 0;
        }
        public virtual void SetTotalBenefitsProperties(XElement calculator)
        {
            this.TotalR = CalculatorHelpers.GetAttributeDouble(calculator,
                TR);
            this.TotalR_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TR_INT);
            this.TotalRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                TRINCENT);
            this.TotalAMR = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR);
            this.TotalAMR_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR_INT);
            this.TotalAMRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT);
            //v145 added output ps and qs to base npvcalcs
            SetTotalBenefitsPsandQsProperties(calculator);
        }
        public virtual void SetTotalBenefitsPsandQsProperties(XElement calculator)
        {
            //price analysis and cost effectiveness analysis
            this.TotalRAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount);
            this.TotalRPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                TRPrice);
            this.TotalRUnit = CalculatorHelpers.GetAttribute(calculator,
                TRUnit);
            this.TotalRName = CalculatorHelpers.GetAttribute(calculator,
                TRName);
            this.TotalRCompositionAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TRCompositionAmount);
            this.TotalRCompositionUnit = CalculatorHelpers.GetAttribute(calculator,
                TRCompositionUnit);
        }
        public virtual void CopyTotalBenefitsPsandQsProperties(CostBenefitCalculator calculator)
        {
            //price analysis and cost effectiveness analysis
            this.TotalRAmount = calculator.TotalRAmount;
            this.TotalRPrice = calculator.TotalRPrice;
            this.TotalRUnit = calculator.TotalRUnit;
            this.TotalRName = calculator.TotalRName;
            this.TotalRCompositionAmount = calculator.TotalRCompositionAmount;
            this.TotalRCompositionUnit = calculator.TotalRCompositionUnit;
        }
        public virtual void SetTotalBenefitsSummaryProperties(XElement calculator)
        {
            this.TotalRAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                TRAmount);
            this.TotalAMR = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMR);
            this.TotalAMRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                TAMRINCENT);
        }
        public virtual void SetTotalBenefitsProperties(string attNameExtension, XElement calculator)
        {
            this.TotalR = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TR, attNameExtension));
            this.TotalR_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TR_INT, attNameExtension));
            this.TotalRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TRINCENT, attNameExtension));
            this.TotalAMR = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMR, attNameExtension));
            this.TotalAMR_INT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMR_INT, attNameExtension));
            this.TotalAMRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMRINCENT, attNameExtension));
            //v145 added output ps and qs to base npvcalcs
            SetTotalBenefitsPsandQsProperties(attNameExtension, calculator);
        }
        public virtual void SetTotalBenefitsPsandQsProperties(string attNameExtension, XElement calculator)
        {
            this.TotalRAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TRAmount, attNameExtension));
            this.TotalRPrice = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TRPrice, attNameExtension));
            this.TotalRUnit = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TRUnit, attNameExtension));
            this.TotalRName = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TRName, attNameExtension));
            this.TotalRCompositionAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TRCompositionAmount, attNameExtension));
            this.TotalRCompositionUnit = CalculatorHelpers.GetAttribute(calculator,
                string.Concat(TRCompositionUnit, attNameExtension));
        }
        public virtual void SetTotalBenefitsSummaryProperties(string attNameExtension, XElement calculator)
        {
            this.TotalRAmount = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TRAmount, attNameExtension));
            this.TotalAMR = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMR, attNameExtension));
            this.TotalAMRINCENT = CalculatorHelpers.GetAttributeDouble(calculator,
                string.Concat(TAMRINCENT, attNameExtension));
        }
        public virtual void SetTotalBenefitsProperty(string attName,
            string attValue)
        {
            switch (attName)
            {
                case TRAmount:
                    this.TotalRAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRPrice:
                    this.TotalRPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRUnit:
                    this.TotalRUnit = attValue;
                    break;
                case TRName:
                    this.TotalRName = attValue;
                    break;
                case TRCompositionAmount:
                    this.TotalRCompositionAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRCompositionUnit:
                    this.TotalRCompositionUnit = attValue;
                    break;
                case TR:
                    this.TotalR = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR_INT:
                    this.TotalR_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TRINCENT:
                    this.TotalRINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR:
                    this.TotalAMR = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMR_INT:
                    this.TotalAMR_INT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TAMRINCENT:
                    this.TotalAMRINCENT = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetTotalBenefitsProperty(string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case TRAmount:
                    sPropertyValue = this.TotalRAmount.ToString();
                    break;
                case TRPrice:
                    sPropertyValue = this.TotalRPrice.ToString();
                    break;
                case TRUnit:
                    sPropertyValue = this.TotalRUnit;
                    break;
                case TRName:
                    sPropertyValue = this.TotalRName.ToString();
                    break;
                case TRCompositionAmount:
                    sPropertyValue = this.TotalRCompositionAmount.ToString();
                    break;
                case TRCompositionUnit:
                    sPropertyValue = this.TotalRCompositionUnit;
                    break;
                case TR:
                    sPropertyValue = this.TotalR.ToString();
                    break;
                case TR_INT:
                    sPropertyValue = this.TotalR_INT.ToString();
                    break;
                case TRINCENT:
                    sPropertyValue = this.TotalRINCENT.ToString();
                    break;
                case TAMR:
                    sPropertyValue = this.TotalAMR.ToString();
                    break;
                case TAMR_INT:
                    sPropertyValue = this.TotalAMR_INT.ToString();
                    break;
                case TAMRINCENT:
                    sPropertyValue = this.TotalAMRINCENT.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetTotalBenefitsAttributes(string attNameExtension, 
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TR, attNameExtension),
                this.TotalR);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TR_INT, attNameExtension),
                this.TotalR_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TRINCENT, attNameExtension),
                this.TotalRINCENT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMR, attNameExtension),
                this.TotalAMR);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMR_INT, attNameExtension),
                this.TotalAMR_INT);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMRINCENT, attNameExtension),
                this.TotalAMRINCENT);
        }
        public virtual void SetTotalBenefitsSummaryAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMR, attNameExtension),
                this.TotalAMR);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TAMRINCENT, attNameExtension),
                this.TotalAMRINCENT);
        }
        public virtual void SetTotalBenefitsPsandQsAttributes(string attNameExtension,
            ref XElement calculator)
        {
            //benefit totals
            CalculatorHelpers.SetAttributeDoubleN3(calculator,
                string.Concat(TRAmount, attNameExtension),
                this.TotalRAmount);
            CalculatorHelpers.SetAttributeDoubleN2(calculator,
                string.Concat(TRPrice, attNameExtension),
                this.TotalRPrice);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(TRUnit, attNameExtension),
                this.TotalRUnit);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(TRName, attNameExtension),
                this.TotalRName);
            CalculatorHelpers.SetAttributeDoubleN3(calculator,
                string.Concat(TRCompositionAmount, attNameExtension),
                this.TotalRCompositionAmount);
            CalculatorHelpers.SetAttribute(calculator,
                string.Concat(TRCompositionUnit, attNameExtension),
                this.TotalRCompositionUnit);
        }
        
        public virtual void SetTotalBenefitsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TR, attNameExtension),
                this.TotalR.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TR_INT, attNameExtension),
                this.TotalR_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRINCENT, attNameExtension),
                this.TotalRINCENT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMR, attNameExtension),
                this.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
               string.Concat(TAMR_INT, attNameExtension),
               this.TotalAMR_INT.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT, attNameExtension),
                this.TotalAMRINCENT.ToString("N2", CultureInfo.InvariantCulture));
        }
        public virtual void SetTotalBenefitsPsandQsAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TRAmount, attNameExtension),
                this.TotalRAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRPrice, attNameExtension),
                this.TotalRPrice.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRUnit, attNameExtension),
                this.TotalRUnit);
            writer.WriteAttributeString(
                string.Concat(TRName, attNameExtension),
                this.TotalRName);
            writer.WriteAttributeString(
                string.Concat(TRCompositionAmount, attNameExtension),
                this.TotalRCompositionAmount.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TRCompositionUnit, attNameExtension),
                this.TotalRCompositionUnit);
        }
        public virtual void SetTotalBenefitsSummaryAttributes(string attNameExtension,
            ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(TAMR, attNameExtension),
                this.TotalAMR.ToString("N2", CultureInfo.InvariantCulture));
            writer.WriteAttributeString(
                string.Concat(TAMRINCENT, attNameExtension),
                this.TotalAMRINCENT.ToString("N2", CultureInfo.InvariantCulture));
        }
        public static void ConvertAttributeNameToStatisticName(
            string nodeName, ref string attName)
        {
            //input and output nodes store their calculations in non statistical attributes
            //use those to set the values of the statistical attributes
            //as more calculators get built, this may need further evaluation
            if (nodeName.EndsWith(DevTreksAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                if (attName.Equals(DevTreksAppHelpers.Prices.OUTPUT_AMOUNT1))
                {
                    attName = TRAmount;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.OUTPUT_PRICE1))
                {
                    attName = TRPrice;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.COMPOSITION_AMOUNT))
                {
                    attName = TRCompositionAmount;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.COMPOSITION_UNIT))
                {
                    attName = TRCompositionUnit;
                }
                else if (attName.Equals(TRAmount))
                {
                    //don't double count the statistic
                    //i.e. calculators should not use these properties
                    attName = string.Empty;
                }
                else if (attName.Equals(TRPrice))
                {
                    attName = string.Empty;
                }
                else if (attName.Equals(TRCompositionAmount))
                {
                    attName = string.Empty;
                }
                else if (attName.Equals(TRCompositionUnit))
                {
                    attName = string.Empty;
                }
            }
            else if (nodeName.EndsWith(DevTreksAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
            {
                if (attName.Equals(DevTreksAppHelpers.Prices.OC_AMOUNT))
                {
                    attName = TOCAmount;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.OC_PRICE))
                {
                    attName = TOCPrice;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.AOH_AMOUNT))
                {
                    attName = TAOHAmount;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.AOH_PRICE))
                {
                    attName = TAOHPrice;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.CAP_AMOUNT))
                {
                    attName = TCAPAmount;
                }
                else if (attName.Equals(DevTreksAppHelpers.Prices.CAP_PRICE))
                {
                    attName = TCAPPrice;
                }
                else if (attName.Equals(TOCAmount))
                {
                    //don't double count the statistic
                    //i.e. calculators should not use these properties
                    attName = string.Empty;
                }
                else if (attName.Equals(TOCPrice))
                {
                    attName = string.Empty;
                }
                else if (attName.Equals(TAOHAmount))
                {
                    attName = string.Empty;
                }
                else if (attName.Equals(TAOHPrice))
                {
                    attName = string.Empty;
                }
                else if (attName.Equals(TCAPAmount))
                {
                    attName = string.Empty;
                }
                else if (attName.Equals(TCAPPrice))
                {
                    attName = string.Empty;
                }
            }
        }
       
    }
}
