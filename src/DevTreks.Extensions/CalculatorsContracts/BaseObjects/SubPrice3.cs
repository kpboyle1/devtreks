using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Add subcosts or subbenefits to DevTreks input or output elements. 
    ///             Typical examples include contingencies, energy, garbage, water, 
    ///             repair in capital inputs. Environmental impact examples include 
    ///             carbon and SO2 which are both traded in markets and may have 'prices'.
    ///             Environmental factors include garbage and food contaminants for food 
    ///             nutrition.
    ///Date:		2013, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        1. Uses _ColIndex to distinguish collection members in xml attributes
    ///             2. Uses the B and C parameters for risk analysis
    ///             3. Objects inheriting from this class (StockAnalysis( should include 
    ///             a SensitivityAnalysisLabel to idenfity the single cost to vary, 
    ///             an optional second property identifying P, Q, or EscRate to vary, 
    ///             and a third property identifying the range to examine.
    ///            
    /// </summary>   
    public class SubPrice3 : CostBenefitCalculator
    {
        //constructor
        public SubPrice3()
        {
            InitSubPrice3sProperties();
        }
        //copy constructors
        public SubPrice3(SubPrice3 calculator)
        {
            CopySubPrice3sProperties(calculator);
        }
        //indicators can be costs or revenues
        //by using the Ind1bAmount and Ind2bAmount as prices
        public enum PRICE_TYPE
        {
            none    = 0,
            rev     = 1,
            oc      = 2,
            aoh     = 3,
            cap     = 4
        }
        //up to five alternatives can be analyzed per element
        //(i.e. with 20 subprices / 5 = 4 subprices per alt)
        //lowest alt type is considered base (usually alt one)
        public enum ALTERNATIVE_TYPE
        {
            none    = 0,
            one     = 1,
            two     = 2,
            three   = 3,
            four    = 4,
            five    = 5,
            six     = 6
        }
        //risk analysis distribution
        //triangular: default is most likely, b is min, c is max
        //normal: default is mean, b is sd
        //uniform: default is min, b is max
        public enum DISTRIBUTION_TYPE
        {
            none            = 0,
            uniform         = 1,
            normal          = 2,
            triangular      = 3
        }
        //Phase 01: generate risk adjusted totals (final numbers)
        public enum RISKANALYSIS_TYPE
        {
            none = 0,
            mcc01 = 1,
            mcc02 = 2,
            mcc03 = 3,
            mcc04 = 4,
            mcc05 = 5
        }
        public static RISKANALYSIS_TYPE GetRiskType(string mandeType)
        {
            //note that the default in the stylesheet must correspond to this default
            RISKANALYSIS_TYPE eRISKANALYSIS_TYPE = RISKANALYSIS_TYPE.none;
            if (mandeType == RISKANALYSIS_TYPE.mcc01.ToString())
            {
                eRISKANALYSIS_TYPE = RISKANALYSIS_TYPE.mcc01;
            }
            else if (mandeType == RISKANALYSIS_TYPE.mcc02.ToString())
            {
                eRISKANALYSIS_TYPE = RISKANALYSIS_TYPE.mcc02;
            }
            else if (mandeType == RISKANALYSIS_TYPE.mcc03.ToString())
            {
                eRISKANALYSIS_TYPE = RISKANALYSIS_TYPE.mcc03;
            }
            else if (mandeType == RISKANALYSIS_TYPE.mcc04.ToString())
            {
                eRISKANALYSIS_TYPE = RISKANALYSIS_TYPE.mcc04;
            }
            else if (mandeType == RISKANALYSIS_TYPE.mcc05.ToString())
            {
                eRISKANALYSIS_TYPE = RISKANALYSIS_TYPE.mcc05;
            }
            return eRISKANALYSIS_TYPE;
        }
        public enum EVALUATION_TYPE
        {
            none = 0,
            //bc ratio or bc probability ratio (look at bc textbook)
            benefitcostratio = 1,
            //6.1 net benefits minus net costs (displayed summarily) of LCC(base) - LCC(alt1)
            netsavings = 2,
            //6.2 savings to investment ration
            sir = 3,
            //6.3 adjusted internal rate of return
            airr = 4,
            //6.4 simple payback and discounted payback
            payback = 5
        }
        public static EVALUATION_TYPE GetDecisionType(string mandeType)
        {
            //note that the default in the stylesheet must correspond to this default
            EVALUATION_TYPE eEVALUATION_TYPE = EVALUATION_TYPE.none;
            if (mandeType == EVALUATION_TYPE.benefitcostratio.ToString())
            {
                eEVALUATION_TYPE = EVALUATION_TYPE.benefitcostratio;
            }
            else if (mandeType == EVALUATION_TYPE.netsavings.ToString())
            {
                eEVALUATION_TYPE = EVALUATION_TYPE.netsavings;
            }
            else if (mandeType == EVALUATION_TYPE.sir.ToString())
            {
                eEVALUATION_TYPE = EVALUATION_TYPE.sir;
            }
            else if (mandeType == EVALUATION_TYPE.airr.ToString())
            {
                eEVALUATION_TYPE = EVALUATION_TYPE.airr;
            }
            else if (mandeType == EVALUATION_TYPE.payback.ToString())
            {
                eEVALUATION_TYPE = EVALUATION_TYPE.payback;
            }
            return eEVALUATION_TYPE;
        }
        //table 7.8 in NIST reference
        public enum DECISION_TYPE
        {
            none = 0,
            //acceptorreject
            acceptorreject = 1,
            //6.1 net benefits minus net costs (displayed summarily) of LCC(base) - LCC(alt1)
            efficiency = 2,
            //6.2 savings to investment ration
            systemselection = 3,
            //6.3 adjusted internal rate of return
            interdependentcombos = 4,
            //6.4 simple payback and discounted payback
            independentcombos = 5
        }
        //list of indicators 
        public List<SubPrice3> SubPrice3s = new List<SubPrice3>();
        //maximum limit for reasonable serialization
        private int MaximumNumberOfSubPrice3s = 20;

        //name
        public string SubPName { get; set; }
        //description
        public string SubPDescription { get; set; }
        //aggregation label
        public string SubPLabel { get; set; }
        //PRICE_TYPES enum
        public string SubPType { get; set; }
        //ALT_TYPES enum
        public string SubPAltType { get; set; }
        //NIST 135 lump sum fempUPV index 
        public double SubPFactor { get; set; }
        //planning construction phase fempUPV index
        public double SubPYears { get; set; }
        //amount
        public double SubPAmount { get; set; }
        //unit
        public string SubPUnit { get; set; }
        //price
        public double SubPPrice { get; set; }
        //escalation rate
        public double SubPEscRate { get; set; }
        //uniform, geometric, linear ...
        public string SubPEscType { get; set; }
        //total subcost (calculated)
        public double SubPTotal { get; set; }
        //total per unit subcost (unit amount passed in by parent) 
        //(calculated)
        public double SubPTotalPerUnit { get; set; }
//sensitivity analysis properties
        //distribution
        public string SubPDistType { get; set; }
        //amount B
        public double SubPAmountB { get; set; }
        //price B
        public double SubPPriceB { get; set; }
        //rate B
        public double SubPEscRateB { get; set; }
        //total B
        public double SubPTotalB { get; set; }
        //amount C
        public double SubPAmountC { get; set; }
        //price C
        public double SubPPriceC { get; set; }
        //rate C
        public double SubPEscRateC { get; set; }
        //total C
        public double SubPTotalC { get; set; }

        //minimum decision support properties
        public string SubPRiskType { get; set; }
        //better to display multiple decision criteria (B/C, net savings, AIRR)
        //public string SubPDecType { get; set; }
        //risk and decision support total
        public string SubPDecTotal { get; set; }
        //decision support total percent difference from base (alt0)
        public string SubPDecPerCentTotal { get; set; }
        //probability of total (i.e. Cost = $100 with 80% probability)
        public string SubPDecProbTotal { get; set; }
        //Accepted ALT_TYPES (for updating input.price or output.price)
        public string SubPAltAcceptType { get; set; }
        

        public const string cSubPName = "SubPName";
        private const string cSubPDescription = "SubPDescription";
        private const string cSubPLabel = "SubPLabel";
        private const string cSubPType = "SubPType";
        private const string cSubPAltType = "SubPAltType";
        private const string cSubPFactor = "SubPFactor";
        private const string cSubPYears = "SubPYears";
        private const string cSubPAmount = "SubPAmount";
        private const string cSubPUnit = "SubPUnit";
        private const string cSubPPrice = "SubPPrice";
        private const string cSubPEscRate = "SubPEscRate";
        private const string cSubPEscType = "SubPEscType";
        private const string cSubPTotal = "SubPTotal";
        private const string cSubPTotalPerUnit = "SubPTotalPerUnit";
        private const string cSubPDistType = "SubPDistType";
        private const string cSubPAmountB = "SubPAmountB";
        private const string cSubPPriceB = "SubPPriceB";
        private const string cSubPEscRateB = "SubPEscRateB";
        private const string cSubPTotalB = "SubPTotalB";
        private const string cSubPAmountC = "SubPAmountC";
        private const string cSubPPriceC = "SubPPriceC";
        private const string cSubPEscRateC = "SubPEscRateC";
        private const string cSubPTotalC = "SubPTotalC";

        public virtual void InitSubPrice3sProperties()
        {
            if (this.SubPrice3s == null)
            {
                this.SubPrice3s = new List<SubPrice3>();
            }
            foreach (SubPrice3 subP in this.SubPrice3s)
            {
                InitSubPrice3Properties(subP);
            }
        }
        private void InitSubPrice3Properties(SubPrice3 subP)
        {
            subP.SubPName = string.Empty;
            subP.SubPDescription = string.Empty;
            subP.SubPLabel = string.Empty;
            subP.SubPType = PRICE_TYPE.none.ToString();
            subP.SubPAltType = ALTERNATIVE_TYPE.none.ToString();
            subP.SubPFactor = 0;
            subP.SubPYears = 0;
            subP.SubPAmount = 0;
            subP.SubPUnit = string.Empty;
            subP.SubPPrice = 0;
            subP.SubPEscRate = 0;
            subP.SubPEscType = string.Empty;
            subP.SubPTotal = 0;
            subP.SubPTotalPerUnit = 0;
            subP.SubPFactor = 0;
            subP.SubPDistType = DISTRIBUTION_TYPE.none.ToString();
            subP.SubPAmountB = 0;
            subP.SubPPriceB = 0;
            subP.SubPEscRateB = 0;
            subP.SubPTotalB = 0;
            subP.SubPAmountC = 0;
            subP.SubPPriceC = 0;
            subP.SubPEscRateC = 0;
            subP.SubPTotalC = 0;
        }
        public virtual void CopySubPrice3sProperties(
            SubPrice3 calculator)
        {
            if (calculator.SubPrice3s != null)
            {
                if (this.SubPrice3s == null)
                {
                    this.SubPrice3s = new List<SubPrice3>();
                }
                foreach (SubPrice3 calculatorInd in calculator.SubPrice3s)
                {
                    SubPrice3 subP = new SubPrice3();
                    CopySubPrice3Properties(subP, calculatorInd);
                    this.SubPrice3s.Add(subP);
                }
            }
        }
        private void CopySubPrice3Properties(
            SubPrice3 subP, SubPrice3 calculator)
        {
            subP.SubPName = calculator.SubPName;
            subP.SubPDescription = calculator.SubPDescription;
            subP.SubPLabel = calculator.SubPLabel;
            subP.SubPType = calculator.SubPType;
            subP.SubPAltType = calculator.SubPAltType;
            subP.SubPFactor = calculator.SubPFactor;
            subP.SubPYears = calculator.SubPYears;
            subP.SubPAmount = calculator.SubPAmount;
            subP.SubPUnit = calculator.SubPUnit;
            subP.SubPPrice = calculator.SubPPrice;
            subP.SubPEscRate = calculator.SubPEscRate;
            subP.SubPEscType = calculator.SubPEscType;
            subP.SubPTotal = calculator.SubPTotal;
            subP.SubPTotalPerUnit = calculator.SubPTotalPerUnit;
            subP.SubPDistType = calculator.SubPDistType;
            subP.SubPAmountB = calculator.SubPAmountB;
            subP.SubPPriceB = calculator.SubPPriceB;
            subP.SubPEscRateB = calculator.SubPEscRateB;
            subP.SubPTotalB = calculator.SubPTotalB;
            subP.SubPAmountC = calculator.SubPAmountC;
            subP.SubPPriceC = calculator.SubPPriceC;
            subP.SubPEscRateC = calculator.SubPEscRateC;
            subP.SubPTotalC = calculator.SubPTotalC;
        }
        public virtual void SetSubPrice3sProperties(XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name 
            //this.SetCalculatorProperties(calculator);
            if (this.SubPrice3s == null)
            {
                this.SubPrice3s = new List<SubPrice3>();
            }
            int i = 1;
            //standard attname used throughout DevTreks
            string sAttNameExtension = string.Empty;
            //don't make unnecessary collection members
            string sHasAttribute = string.Empty;
            for (i = 1; i < this.MaximumNumberOfSubPrice3s; i++)
            {
                sAttNameExtension = i.ToString();
                sHasAttribute = CalculatorHelpers.GetAttribute(calculator,
                    string.Concat(cSubPName, sAttNameExtension));
                if (!string.IsNullOrEmpty(sHasAttribute))
                {
                    SubPrice3 subP1 = new SubPrice3();
                    SetSubPrice3Properties(subP1, sAttNameExtension, calculator);
                    this.SubPrice3s.Add(subP1);
                }
                sHasAttribute = string.Empty;
            }
        }
        private void SetSubPrice3Properties(SubPrice3 subP, string attNameExtension,
            XElement calculator)
        {
            //set this object's properties
            subP.SubPName = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPName, attNameExtension));
            subP.SubPDescription = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPDescription, attNameExtension));
            subP.SubPLabel = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPLabel, attNameExtension));
            subP.SubPAltType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPAltType, attNameExtension));
            subP.SubPType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPType, attNameExtension));
            subP.SubPAmount = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPAmount, attNameExtension));
            subP.SubPUnit = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPUnit, attNameExtension));
            subP.SubPPrice = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPPrice, attNameExtension));
            subP.SubPFactor = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPFactor, attNameExtension));
            subP.SubPYears = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPYears, attNameExtension));
            subP.SubPEscRate = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPEscRate, attNameExtension));
            subP.SubPEscType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPEscType, attNameExtension));
            subP.SubPTotal = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPTotal, attNameExtension));
            subP.SubPTotalPerUnit = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPTotalPerUnit, attNameExtension));
            subP.SubPDistType = CalculatorHelpers.GetAttribute(calculator,
               string.Concat(cSubPDistType, attNameExtension));
            subP.SubPAmountB = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPAmountB, attNameExtension));
            subP.SubPPriceB = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPPriceB, attNameExtension));
            subP.SubPEscRateB = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPEscRateB, attNameExtension));
            subP.SubPTotalB = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPTotalB, attNameExtension));
            subP.SubPAmountC = CalculatorHelpers.GetAttributeDouble(calculator,
              string.Concat(cSubPAmountC, attNameExtension));
            subP.SubPPriceC = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPPriceC, attNameExtension));
            subP.SubPEscRateC = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPEscRateC, attNameExtension));
            subP.SubPTotalC = CalculatorHelpers.GetAttributeDouble(calculator,
               string.Concat(cSubPTotalC, attNameExtension));
        }
        public virtual void SetSubPrice3sProperty(string attName,
           string attValue, int colIndex)
        {
            if (this.SubPrice3s == null)
            {
                this.SubPrice3s = new List<SubPrice3>();
            }
            if (this.SubPrice3s.Count < (colIndex + 1))
            {
                SubPrice3 subP1 = new SubPrice3();
                this.SubPrice3s.Insert(colIndex, subP1);
            }
            SubPrice3 subP = this.SubPrice3s.ElementAt(colIndex);
            if (subP != null)
            {
                SetSubPrice3Property(subP, attName, attValue);
            }
        }
        private void SetSubPrice3Property(SubPrice3 subP,
            string attName, string attValue)
        {
            switch (attName)
            {
                case cSubPName:
                    subP.SubPName = attValue;
                    break;
                case cSubPDescription:
                    subP.SubPDescription = attValue;
                    break;
                case cSubPLabel:
                    subP.SubPLabel = attValue;
                    break;
                case cSubPType:
                    subP.SubPType = attValue;
                    break;
                case cSubPAltType:
                    subP.SubPAltType = attValue;
                    break;
                case cSubPFactor:
                    subP.SubPFactor = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPYears:
                    subP.SubPYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmount:
                    subP.SubPAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPUnit:
                    subP.SubPUnit = attValue;
                    break;
                case cSubPPrice:
                    subP.SubPPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRate:
                    subP.SubPEscRate = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscType:
                    subP.SubPEscType = attValue;
                    break;
                case cSubPTotal:
                    subP.SubPTotal = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalPerUnit:
                    subP.SubPTotalPerUnit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPDistType:
                    subP.SubPDistType = attValue;
                    break;
                case cSubPAmountB:
                    subP.SubPAmountB = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPPriceB:
                    subP.SubPPriceB = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRateB:
                    subP.SubPEscRateB = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalB:
                    subP.SubPTotalB = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPAmountC:
                    subP.SubPAmountC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPPriceC:
                    subP.SubPPriceC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPEscRateC:
                    subP.SubPEscRateC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cSubPTotalC:
                    subP.SubPTotalC = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public virtual string GetSubPrice3sProperty(string attName, int colIndex)
        {
            string sPropertyValue = string.Empty;
            if (this.SubPrice3s.Count >= (colIndex + 1))
            {
                SubPrice3 subP = this.SubPrice3s.ElementAt(colIndex);
                if (subP != null)
                {
                    sPropertyValue = GetSubPrice3Property(subP, attName);
                }
            }
            return sPropertyValue;
        }
        private string GetSubPrice3Property(SubPrice3 subP, string attName)
        {
            string sPropertyValue = string.Empty;
            switch (attName)
            {
                case cSubPName:
                    sPropertyValue = subP.SubPName;
                    break;
                case cSubPDescription:
                    sPropertyValue = subP.SubPDescription;
                    break;
                case cSubPLabel:
                    sPropertyValue = subP.SubPLabel;
                    break;
                case cSubPType:
                    sPropertyValue = subP.SubPType;
                    break;
                case cSubPAltType:
                    sPropertyValue = subP.SubPAltType;
                    break;
                case cSubPFactor:
                    sPropertyValue = subP.SubPFactor.ToString();
                    break;
                case cSubPYears:
                    sPropertyValue = subP.SubPYears.ToString();
                    break;
                case cSubPAmount:
                    sPropertyValue = subP.SubPAmount.ToString();
                    break;
                case cSubPUnit:
                    sPropertyValue = subP.SubPUnit.ToString();
                    break;
                case cSubPPrice:
                    sPropertyValue = subP.SubPPrice.ToString();
                    break;
                case cSubPEscRate:
                    sPropertyValue = subP.SubPEscRate.ToString();
                    break;
                case cSubPEscType:
                    sPropertyValue = subP.SubPEscType;
                    break;
                case cSubPTotal:
                    sPropertyValue = subP.SubPTotal.ToString();
                    break;
                case cSubPTotalPerUnit:
                    sPropertyValue = subP.SubPTotalPerUnit.ToString();
                    break;
                case cSubPDistType:
                    sPropertyValue = subP.SubPDistType.ToString();
                    break;
                case cSubPAmountB:
                    sPropertyValue = subP.SubPAmountB.ToString();
                    break;
                case cSubPPriceB:
                    sPropertyValue = subP.SubPPriceB.ToString();
                    break;
                case cSubPEscRateB:
                    sPropertyValue = subP.SubPEscRateB.ToString();
                    break;
                case cSubPTotalB:
                    sPropertyValue = subP.SubPTotalB.ToString();
                    break;
                case cSubPAmountC:
                    sPropertyValue = subP.SubPAmountC.ToString();
                    break;
                case cSubPPriceC:
                    sPropertyValue = subP.SubPPriceC.ToString();
                    break;
                case cSubPEscRateC:
                    sPropertyValue = subP.SubPEscRateC.ToString();
                    break;
                case cSubPTotalC:
                    sPropertyValue = subP.SubPTotalC.ToString();
                    break;
                default:
                    break;
            }
            return sPropertyValue;
        }
        public virtual void SetSubPrice3sAttributes(ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
            if (this.SubPrice3s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice3 subP in this.SubPrice3s)
                {
                    sAttNameExtension = i.ToString();
                    SetSubPrice3Attributes(subP, sAttNameExtension,
                        ref calculator);
                    i++;
                }
            }
        }
        private void SetSubPrice3Attributes(SubPrice3 subP, string attNameExtension,
            ref XElement calculator)
        {
            //remember that the calculator inheriting from this class must set id and name atts
            //and remove unwanted old atts i.e. this.SetCalculatorAttributes(calculator);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPName, attNameExtension), subP.SubPName);
            CalculatorHelpers.SetAttribute(calculator,
                     string.Concat(cSubPDescription, attNameExtension), subP.SubPDescription);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPLabel, attNameExtension), subP.SubPLabel);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPType, attNameExtension), subP.SubPType);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPAltType, attNameExtension), subP.SubPAltType);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPFactor, attNameExtension), subP.SubPFactor);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPYears, attNameExtension), subP.SubPYears);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPAmount, attNameExtension), subP.SubPAmount);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPUnit, attNameExtension), subP.SubPUnit);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPPrice, attNameExtension), subP.SubPPrice);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPEscRate, attNameExtension), subP.SubPEscRate);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPEscType, attNameExtension), subP.SubPEscType);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPTotal, attNameExtension), subP.SubPTotal);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPTotalPerUnit, attNameExtension), subP.SubPTotalPerUnit);
            CalculatorHelpers.SetAttribute(calculator,
                 string.Concat(cSubPDistType, attNameExtension), subP.SubPDistType);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPAmountB, attNameExtension), subP.SubPAmountB);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPPriceB, attNameExtension), subP.SubPPriceB);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPEscRateB, attNameExtension), subP.SubPEscRateB);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPTotalB, attNameExtension), subP.SubPTotalB);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPAmountC, attNameExtension), subP.SubPAmountC);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPPriceC, attNameExtension), subP.SubPPriceC);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPEscRateC, attNameExtension), subP.SubPEscRateC);
            CalculatorHelpers.SetAttributeDoubleF2(calculator,
                 string.Concat(cSubPTotalC, attNameExtension), subP.SubPTotalC);
        }
        public virtual void SetSubPrice3sAttributes(ref XmlWriter writer)
        {
            if (this.SubPrice3s != null)
            {
                int i = 1;
                string sAttNameExtension = string.Empty;
                foreach (SubPrice3 subP in this.SubPrice3s)
                {
                    sAttNameExtension = i.ToString();
                    SetSubPrice3Attributes(subP, sAttNameExtension,
                        ref writer);
                    i++;
                }
            }
        }
        public virtual void SetSubPrice3Attributes(SubPrice3 subP, string attNameExtension,
           ref XmlWriter writer)
        {
            writer.WriteAttributeString(
                string.Concat(cSubPName, attNameExtension), subP.SubPName);
            writer.WriteAttributeString(
                     string.Concat(cSubPDescription, attNameExtension), subP.SubPDescription);
            writer.WriteAttributeString(
                 string.Concat(cSubPLabel, attNameExtension), subP.SubPLabel);
            writer.WriteAttributeString(
                 string.Concat(cSubPType, attNameExtension), subP.SubPType);
            writer.WriteAttributeString(
                 string.Concat(cSubPAltType, attNameExtension), subP.SubPAltType);
            writer.WriteAttributeString(
                 string.Concat(cSubPFactor, attNameExtension), subP.SubPFactor.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPYears, attNameExtension), subP.SubPYears.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPAmount, attNameExtension), subP.SubPAmount.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPUnit, attNameExtension), subP.SubPUnit);
            writer.WriteAttributeString(
                 string.Concat(cSubPPrice, attNameExtension), subP.SubPPrice.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPEscRate, attNameExtension), subP.SubPEscRate.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPEscType, attNameExtension), subP.SubPEscType);
            writer.WriteAttributeString(
                 string.Concat(cSubPTotal, attNameExtension), subP.SubPTotal.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPTotalPerUnit, attNameExtension), subP.SubPTotalPerUnit.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPDistType, attNameExtension), subP.SubPDistType);
            writer.WriteAttributeString(
                 string.Concat(cSubPAmountB, attNameExtension), subP.SubPAmountB.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPPriceB, attNameExtension), subP.SubPPriceB.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPEscRateB, attNameExtension), subP.SubPEscRateB.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPTotalB, attNameExtension), subP.SubPTotalB.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPAmountC, attNameExtension), subP.SubPAmountC.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPPriceC, attNameExtension), subP.SubPPriceC.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPEscRateC, attNameExtension), subP.SubPEscRateC.ToString());
            writer.WriteAttributeString(
                 string.Concat(cSubPTotalC, attNameExtension), subP.SubPTotalC.ToString());
        }

        //run the calculations
        public bool RunCalculations()
        {
            bool bHasCalculations = false;
            SetCalculations();
            //run other calcs
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool SetCalculations()
        {
            bool bHasCalcs = false;
            foreach (SubPrice3 subP in this.SubPrice3s)
            {
                //subP.IndTotal = GetTotal(subP.IndMathType,
                //    subP.Ind1aAmount, subP.Ind2aAmount);
            }
            bHasCalcs = true;
            return bHasCalcs;
        }
        public static double GetTotal(string mathType, double Ind1aAmount, double Ind2aAmount)
        {
            double dbTotal = 0;
            //if (mathType == QMATH_TYPE.Q1_divide_Q2.ToString())
            //{
            //    if (Ind2aAmount == 0)
            //    {
            //        return -1;
            //    }
            //    dbTotal = Ind1aAmount / Ind2aAmount;
            //}
            //else if (mathType == QMATH_TYPE.Q1_multiply_Q2.ToString())
            //{
            //    dbTotal = Ind1aAmount * Ind2aAmount;
            //}
            //else if (mathType == QMATH_TYPE.Q1_add_Q2.ToString())
            //{
            //    dbTotal = Ind1aAmount + Ind2aAmount;
            //}
            //else if (mathType == QMATH_TYPE.Q1_subtract_Q2.ToString())
            //{
            //    dbTotal = Ind1aAmount - Ind2aAmount;
            //}
            //else
            //{
            //    //default is q1
            //    dbTotal = Ind1aAmount;
            //}
            return dbTotal;
        }
    }
}
