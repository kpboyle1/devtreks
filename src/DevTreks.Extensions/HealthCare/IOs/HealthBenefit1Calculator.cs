using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Errors = DevTreks.Exceptions.DevTreksErrors;


namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Serialize and deserialize a health care benefit object with
    ///             properties derived from basic qualitative and quantitative 
    ///             indicators
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    public class HealthBenefit1Calculator : Output
    {
        public HealthBenefit1Calculator()
            : base()
        {
            //health care benefit object
            InitHealthBenefit1Properties();
        }
        //copy constructor
        public HealthBenefit1Calculator(HealthBenefit1Calculator healthBenefit1)
            : base(healthBenefit1)
        {
            CopyHealthBenefit1Properties(healthBenefit1);
        }
        
        //calculator properties
        //demographic properties
        public Demog1 Demographics { get; set; }
        //output.outputprice
        public double OutputCost { get; set; }
        //AdjustedBenefit = BenefitAdjustment * OutputCost
        public double BenefitAdjustment { get; set; }
        public double AdjustedBenefit { get; set; }
        //for cost effectiveness analysis (i.e. cost per stroke averted)
        public string OutputEffect1Name { get; set; }
        public string OutputEffect1Unit { get; set; }
        public double OutputEffect1Amount { get; set; }
        public double OutputEffect1Price { get; set; }
        public double OutputEffect1Cost { get; set; }
        //improvement in HRQol dimension
        public int PhysicalHealthRating { get; set; }
        public int EmotionalHealthRating { get; set; }
        public int SocialHealthRating { get; set; }
        public int EconomicHealthRating { get; set; }
        public int HealthCareDeliveryRating { get; set; }
        
        //HRQol weight before treatment
        public int BeforeQOLRating { get; set; }
        //HRQol weight after treatment
        public int AfterQOLRating { get; set; }
        //duration of before treatment years
        public double BeforeYears { get; set; }
        //duration of after treatment years
        public double AfterYears { get; set; }
        //probability of duration of after treatment years
        public double AfterYearsProb { get; set; }
        //fewer after treatment years in exchange for improving from the before treatment state?
        public double TimeTradeoffYears { get; set; }
        //optional equity multiplier
        public double EquityMultiplier { get; set; }
        //explanation of output ratings
        public string BenefitAssessment { get; set; }
        //average of five health quality dimensions
        public double AverageBenefitRating { get; set; }
        public double QALY { get; set; }
        public double ICERQALY { get; set; }
        public double TTOQALY { get; set; }
        //if offered, will complete a survey going into health benefits and costs in more detail
        public bool WillDoSurvey { get; set; }

        private const string cOutputCost = "OutputCost";
        private const string cBenefitAdjustment = "BenefitAdjustment";
        private const string cAdjustedBenefit = "AdjustedBenefit";
        private const string cOutputEffect1Name = "OutputEffect1Name";
        private const string cOutputEffect1Unit = "OutputEffect1Unit";
        private const string cOutputEffect1Amount = "OutputEffect1Amount";
        private const string cOutputEffect1Price = "OutputEffect1Price";
        private const string cOutputEffect1Cost = "OutputEffect1Cost";
        private const string cPhysicalHealthRating = "PhysicalHealthRating";
        private const string cEmotionalHealthRating = "EmotionalHealthRating";
        private const string cSocialHealthRating = "SocialHealthRating";
        private const string cEconomicHealthRating = "EconomicHealthRating";
        private const string cHealthCareDeliveryRating = "HealthCareDeliveryRating";
        private const string cBeforeQOLRating = "BeforeQOLRating";
        private const string cAfterQOLRating = "AfterQOLRating";
        private const string cBeforeYears = "BeforeYears";
        private const string cAfterYears = "AfterYears";
        private const string cAfterYearsProb = "AfterYearsProb";
        private const string cTimeTradeoffYears = "TimeTradeoffYears";
        private const string cEquityMultiplier = "EquityMultiplier";
        private const string cBenefitAssessment = "BenefitAssessment";
        private const string cAverageBenefitRating = "AverageBenefitRating";
        private const string cQALY = "QALY";
        private const string cICERQALY = "ICERQALY";
        private const string cTTOQALY = "TTOQALY";
        private const string cWillDoSurvey = "WillDoSurvey";

        public virtual void InitHealthBenefit1Properties()
        {
            Demog1 Demographics = new Demog1();
            Demographics.InitDemog1Properties();
            //avoid null references to properties
            this.OutputCost = 0;
            this.BenefitAdjustment = 0;
            this.AdjustedBenefit = 0;
            this.OutputEffect1Name = string.Empty;
            this.OutputEffect1Unit = string.Empty;
            this.OutputEffect1Amount = 0;
            this.OutputEffect1Price = 0;
            this.OutputEffect1Cost = 0;
            this.AverageBenefitRating = 0;
            this.PhysicalHealthRating = 0;
            this.EmotionalHealthRating = 0;
            this.SocialHealthRating = 0;
            this.EconomicHealthRating = 0;
            this.HealthCareDeliveryRating = 0;
            this.BeforeQOLRating = 0;
            this.AfterQOLRating = 0;
            this.BeforeYears = 0;
            this.AfterYears = 0;
            this.AfterYearsProb = 0;
            this.TimeTradeoffYears = 0;
            this.EquityMultiplier = 0;
            this.BenefitAssessment = string.Empty;
            this.QALY = 0;
            this.ICERQALY = 0;
            this.TTOQALY = 0;
            this.WillDoSurvey = false;
        }

        public virtual void CopyHealthBenefit1Properties(
            HealthBenefit1Calculator calculator)
        {
            this.Demographics.CopyDemog1Properties(calculator.Demographics);
            this.OutputCost = calculator.OutputCost;
            this.BenefitAdjustment = calculator.BenefitAdjustment;
            this.AdjustedBenefit = calculator.AdjustedBenefit;
            this.OutputEffect1Name = calculator.OutputEffect1Name;
            this.OutputEffect1Unit = calculator.OutputEffect1Unit;
            this.OutputEffect1Amount = calculator.OutputEffect1Amount;
            this.OutputEffect1Price = calculator.OutputEffect1Price;
            this.OutputEffect1Cost = calculator.OutputEffect1Cost;
            this.AverageBenefitRating = calculator.AverageBenefitRating;
            this.PhysicalHealthRating = calculator.PhysicalHealthRating;
            this.EmotionalHealthRating = calculator.EmotionalHealthRating;
            this.SocialHealthRating = calculator.SocialHealthRating;
            this.EconomicHealthRating = calculator.EconomicHealthRating;
            this.HealthCareDeliveryRating = calculator.HealthCareDeliveryRating;
            this.BeforeQOLRating = calculator.BeforeQOLRating;
            this.AfterQOLRating = calculator.AfterQOLRating;
            this.BeforeYears = calculator.BeforeYears;
            this.AfterYears = calculator.AfterYears;
            this.AfterYearsProb = calculator.AfterYearsProb;
            this.TimeTradeoffYears = calculator.TimeTradeoffYears;
            this.EquityMultiplier = calculator.EquityMultiplier; 
            this.BenefitAssessment = calculator.BenefitAssessment;
            this.QALY = calculator.QALY;
            this.ICERQALY = calculator.ICERQALY;
            this.TTOQALY = calculator.TTOQALY;
            this.WillDoSurvey = calculator.WillDoSurvey;
        }
        public virtual void SetHealthBenefit1Properties(CalculatorParameters calcParameters,
            XElement calculator, XElement currentElement)
        {
            SetOutputProperties(calcParameters, calculator,
                currentElement);
            SetHealthBenefit1Properties(calculator);
        }
        //set the class properties using the XElement
        public virtual void SetHealthBenefit1Properties(XElement currentCalculationsElement)
        {
            if (Demographics == null) Demographics = new Demog1();
            Demographics.SetDemog1Properties(currentCalculationsElement);
            //don't set any input properties; each calculator should set what's needed separately
            this.OutputCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputCost);
            this.BenefitAdjustment = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBenefitAdjustment);
            this.AdjustedBenefit = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAdjustedBenefit);
            this.OutputEffect1Name = CalculatorHelpers.GetAttribute(currentCalculationsElement, cOutputEffect1Name);
            this.OutputEffect1Unit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cOutputEffect1Unit);
            this.OutputEffect1Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputEffect1Amount);
            this.OutputEffect1Price = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputEffect1Price);
            this.OutputEffect1Cost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputEffect1Cost);
            this.AverageBenefitRating = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAverageBenefitRating);
            this.PhysicalHealthRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cPhysicalHealthRating);
            this.EmotionalHealthRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cEmotionalHealthRating);
            this.SocialHealthRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cSocialHealthRating);
            this.EconomicHealthRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cEconomicHealthRating);
            this.HealthCareDeliveryRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cHealthCareDeliveryRating);
            this.BeforeQOLRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cBeforeQOLRating);
            this.AfterQOLRating = CalculatorHelpers.GetAttributeInt(currentCalculationsElement, cAfterQOLRating);
            this.BeforeYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cBeforeYears);
            this.AfterYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAfterYears);
            this.AfterYearsProb = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAfterYearsProb);
            this.TimeTradeoffYears = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimeTradeoffYears);
            this.EquityMultiplier = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cEquityMultiplier);
            this.BenefitAssessment = CalculatorHelpers.GetAttribute(currentCalculationsElement, cBenefitAssessment);
            this.QALY = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cQALY);
            this.ICERQALY = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cICERQALY);
            this.TTOQALY = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTTOQALY);
            this.WillDoSurvey = CalculatorHelpers.GetAttributeBool(currentCalculationsElement, cWillDoSurvey);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetHealthBenefit1Properties(string attName,
            string attValue)
        {
            if (Demographics == null) Demographics = new Demog1();
            Demographics.SetDemog1Properties(attName, attValue);
            switch (attName)
            {
                case cOutputCost:
                    this.OutputCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cBenefitAdjustment:
                    this.BenefitAdjustment = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAdjustedBenefit:
                    this.AdjustedBenefit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOutputEffect1Name:
                    this.OutputEffect1Name = attValue;
                    break;
                case cOutputEffect1Unit:
                    this.OutputEffect1Unit = attValue;
                    break;
                case cOutputEffect1Amount:
                    this.OutputEffect1Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOutputEffect1Price:
                    this.OutputEffect1Price = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOutputEffect1Cost:
                    this.OutputEffect1Cost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAverageBenefitRating:
                    this.AverageBenefitRating = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cPhysicalHealthRating:
                    this.PhysicalHealthRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cEmotionalHealthRating:
                    this.EmotionalHealthRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cSocialHealthRating:
                    this.SocialHealthRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cEconomicHealthRating:
                    this.EconomicHealthRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cHealthCareDeliveryRating:
                    this.HealthCareDeliveryRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBeforeQOLRating:
                    this.BeforeQOLRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cAfterQOLRating:
                    this.AfterQOLRating = CalculatorHelpers.ConvertStringToInt(attValue);
                    break;
                case cBeforeYears:
                    this.BeforeYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAfterYears:
                    this.AfterYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAfterYearsProb:
                    this.AfterYearsProb = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTimeTradeoffYears:
                    this.TimeTradeoffYears = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cEquityMultiplier:
                    this.EquityMultiplier = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cBenefitAssessment:
                    this.BenefitAssessment = attValue;
                    break;
                case cQALY:
                    this.QALY = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cICERQALY:
                    this.ICERQALY = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTTOQALY:
                    this.TTOQALY = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWillDoSurvey:
                    this.WillDoSurvey = CalculatorHelpers.ConvertStringToBool(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetHealthBenefit1Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            if (Demographics == null) Demographics = new Demog1();
            Demographics.SetDemog1Attributes(attNameExtension, ref currentCalculationsElement);
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputCost, attNameExtension), this.OutputCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cBenefitAdjustment, attNameExtension), this.BenefitAdjustment);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAdjustedBenefit, attNameExtension), this.AdjustedBenefit);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cOutputEffect1Name, attNameExtension), this.OutputEffect1Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cOutputEffect1Unit, attNameExtension), this.OutputEffect1Unit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputEffect1Amount, attNameExtension), this.OutputEffect1Amount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputEffect1Price, attNameExtension), this.OutputEffect1Price);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputEffect1Cost, attNameExtension), this.OutputEffect1Cost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAverageBenefitRating, attNameExtension), this.AverageBenefitRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               string.Concat(cPhysicalHealthRating, attNameExtension), this.PhysicalHealthRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               string.Concat(cEmotionalHealthRating, attNameExtension), this.EmotionalHealthRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               string.Concat(cSocialHealthRating, attNameExtension), this.SocialHealthRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               string.Concat(cEconomicHealthRating, attNameExtension), this.EconomicHealthRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               string.Concat(cHealthCareDeliveryRating, attNameExtension), this.HealthCareDeliveryRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
               string.Concat(cBeforeQOLRating, attNameExtension), this.BeforeQOLRating);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
              string.Concat(cAfterQOLRating, attNameExtension), this.AfterQOLRating);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(cBeforeYears, attNameExtension), this.BeforeYears);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(cAfterYears, attNameExtension), this.AfterYears);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(cAfterYearsProb, attNameExtension), this.AfterYearsProb);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
              string.Concat(cTimeTradeoffYears, attNameExtension), this.TimeTradeoffYears);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
               string.Concat(cEquityMultiplier, attNameExtension), this.EquityMultiplier);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               string.Concat(cBenefitAssessment, attNameExtension), this.BenefitAssessment);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cQALY, attNameExtension), this.QALY);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cICERQALY, attNameExtension), this.ICERQALY);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
               string.Concat(cTTOQALY, attNameExtension), this.TTOQALY);
            CalculatorHelpers.SetAttributeBool(currentCalculationsElement,
               string.Concat(cWillDoSurvey, attNameExtension), this.WillDoSurvey);
        }
        public virtual void SetHealthBenefit1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            if (Demographics == null) Demographics = new Demog1();
            Demographics.SetDemog1Attributes(attNameExtension, ref writer);
            writer.WriteAttributeString(string.Concat(cOutputCost, attNameExtension), this.OutputCost.ToString());
            writer.WriteAttributeString(string.Concat(cBenefitAdjustment, attNameExtension), this.BenefitAdjustment.ToString());
            writer.WriteAttributeString(string.Concat(cAdjustedBenefit, attNameExtension), this.AdjustedBenefit.ToString());
            writer.WriteAttributeString(string.Concat(cOutputEffect1Name, attNameExtension), this.OutputEffect1Name);
            writer.WriteAttributeString(string.Concat(cOutputEffect1Unit, attNameExtension), this.OutputEffect1Unit);
            writer.WriteAttributeString(string.Concat(cOutputEffect1Amount, attNameExtension), this.OutputEffect1Amount.ToString());
            writer.WriteAttributeString(string.Concat(cOutputEffect1Price, attNameExtension), this.OutputEffect1Price.ToString());
            writer.WriteAttributeString(string.Concat(cOutputEffect1Cost, attNameExtension), this.OutputEffect1Cost.ToString());
            writer.WriteAttributeString(string.Concat(cAverageBenefitRating, attNameExtension), this.AverageBenefitRating.ToString());
            writer.WriteAttributeString(string.Concat(cPhysicalHealthRating, attNameExtension), this.PhysicalHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(cEmotionalHealthRating, attNameExtension), this.EmotionalHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(cSocialHealthRating, attNameExtension), this.SocialHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(cEconomicHealthRating, attNameExtension), this.EconomicHealthRating.ToString());
            writer.WriteAttributeString(string.Concat(cHealthCareDeliveryRating, attNameExtension), this.HealthCareDeliveryRating.ToString());
            writer.WriteAttributeString(string.Concat(cBeforeQOLRating, attNameExtension), this.BeforeQOLRating.ToString());
            writer.WriteAttributeString(string.Concat(cAfterQOLRating, attNameExtension), this.AfterQOLRating.ToString());
            writer.WriteAttributeString(string.Concat(cBeforeYears, attNameExtension), this.BeforeYears.ToString());
            writer.WriteAttributeString(string.Concat(cAfterYears, attNameExtension), this.AfterYears.ToString());
            writer.WriteAttributeString(string.Concat(cAfterYearsProb, attNameExtension), this.AfterYearsProb.ToString());
            writer.WriteAttributeString(string.Concat(cTimeTradeoffYears, attNameExtension), this.TimeTradeoffYears.ToString());
            writer.WriteAttributeString(string.Concat(cEquityMultiplier, attNameExtension), this.EquityMultiplier.ToString());
            writer.WriteAttributeString(string.Concat(cBenefitAssessment, attNameExtension), this.BenefitAssessment.ToString());
            writer.WriteAttributeString(string.Concat(cQALY, attNameExtension), this.QALY.ToString());
            writer.WriteAttributeString(string.Concat(cICERQALY, attNameExtension), this.ICERQALY.ToString());
            writer.WriteAttributeString(string.Concat(cTTOQALY, attNameExtension), this.TTOQALY.ToString());
            writer.WriteAttributeString(string.Concat(cWillDoSurvey, attNameExtension), this.WillDoSurvey.ToString());
        }
        public bool RunHCCalculations(CalculatorParameters calcParameters,
            HealthBenefit1Calculator hcBenefit1)
        {
            bool bHasCalculations = false;
            if (hcBenefit1 != null)
            {
                FixSelections(hcBenefit1);
                //calculate the adjusted benefit
                hcBenefit1.AdjustedBenefit = hcBenefit1.OutputCost * (hcBenefit1.BenefitAdjustment / 100);
                
                //set the base input properties
                //remember not to set amounts or the calcs have to be rerun in npv calcors
                hcBenefit1.Price = hcBenefit1.AdjustedBenefit;
                hcBenefit1.Unit = (hcBenefit1.Unit != string.Empty && hcBenefit1.Unit != Constants.NONE)
                    ? hcBenefit1.Unit : Constants.EACH;
                //set the bcrating (equally weighted)
                SetAverageBenefitRating(hcBenefit1);
                bHasCalculations = true;
            }
            else
            {
                calcParameters.ErrorMessage = Errors.MakeStandardErrorMsg("CALCULATORS_WRONG_ONE");
            }
            return bHasCalculations;
        }
        private void FixSelections(HealthBenefit1Calculator hcBenefit1)
        {
            if (hcBenefit1.Demographics == null)
            {
                hcBenefit1.Demographics = new Demog1();
            }
            Demog1.FixSelections(hcBenefit1.Demographics);
        }
        private void SetAverageBenefitRating(HealthBenefit1Calculator hcBenefit1)
        {
            //five dimension benefit rating
            SetBenefitRating(hcBenefit1);
            //effects
            hcBenefit1.OutputEffect1Cost = hcBenefit1.OutputEffect1Amount * hcBenefit1.OutputEffect1Price;

            //these need to be discounted by real rate and before or after years
            //conversion to double
            double onehundred = 100;
            double dbBeforeQOLRating = (hcBenefit1.BeforeQOLRating / onehundred);
            double dbAfterQOLRating = (hcBenefit1.AfterQOLRating / onehundred);
            //discounted QALY
            double dbAfterDiscountFactor = MathHelpers.DiscountFactor(hcBenefit1.Local.RealRate, hcBenefit1.AfterYears);
            hcBenefit1.QALY = dbAfterQOLRating * hcBenefit1.AfterYears * dbAfterDiscountFactor;
            //discounted QALY
            double dbBeforeDiscountFactor = MathHelpers.DiscountFactor(hcBenefit1.Local.RealRate, hcBenefit1.BeforeYears);
            double dbBeforeQALY = dbBeforeQOLRating * hcBenefit1.BeforeYears * dbBeforeDiscountFactor;
            if (hcBenefit1.EquityMultiplier > 0)
            {
                dbBeforeQALY = dbBeforeQALY * (hcBenefit1.EquityMultiplier / onehundred);
            }
            if (hcBenefit1.AfterYearsProb > 0 && hcBenefit1.AfterYearsProb < onehundred)
            {
                hcBenefit1.QALY = (hcBenefit1.QALY * (hcBenefit1.AfterYearsProb / onehundred))
                    + (dbBeforeQALY * ((onehundred - hcBenefit1.AfterYearsProb) / onehundred));
            }
            if (hcBenefit1.EquityMultiplier > 0)
            {
                hcBenefit1.QALY = hcBenefit1.QALY * (hcBenefit1.EquityMultiplier / onehundred);
            }
            //incremental QALY gain
            hcBenefit1.ICERQALY = hcBenefit1.QALY - dbBeforeQALY;

            //ttoqaly (qaly weight * after years * discount factor)
            hcBenefit1.TTOQALY = (hcBenefit1.TimeTradeoffYears / hcBenefit1.BeforeYears) * hcBenefit1.AfterYears * dbAfterDiscountFactor;
            if (hcBenefit1.EquityMultiplier > 0)
            {
                hcBenefit1.TTOQALY = hcBenefit1.TTOQALY * (hcBenefit1.EquityMultiplier / onehundred);
            }
        }
        private void SetBenefitRating(HealthBenefit1Calculator hcBenefit1)
        {
            double dbBCRating = 0;
            int iDivisor = 0;
            if (hcBenefit1.PhysicalHealthRating > 0)
            {
                dbBCRating += hcBenefit1.PhysicalHealthRating;
                iDivisor += 1;
            }
            if (hcBenefit1.EmotionalHealthRating > 0)
            {
                dbBCRating += hcBenefit1.EmotionalHealthRating;
                iDivisor += 1;
            }
            if (hcBenefit1.SocialHealthRating > 0)
            {
                dbBCRating += hcBenefit1.SocialHealthRating;
                iDivisor += 1;
            }
            if (hcBenefit1.EconomicHealthRating > 0)
            {
                dbBCRating += hcBenefit1.EconomicHealthRating;
                iDivisor += 1;
            }
            if (hcBenefit1.HealthCareDeliveryRating > 0)
            {
                dbBCRating += hcBenefit1.HealthCareDeliveryRating;
                iDivisor += 1;
            }
            if (iDivisor == 0)
            {
                hcBenefit1.AverageBenefitRating = 0;
            }
            else
            {
                hcBenefit1.AverageBenefitRating = dbBCRating / iDivisor;
            }
        }
    }
}
