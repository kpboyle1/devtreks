using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Serialize and deserialize a monitoring and evaluation indicators object with
    ///             properties derived from base indicators object (which extends base CostBenefitCalculator)
    ///             This calculator can be used with any element (input, outcome), but the 
    ///             stylesheets must be element specific.
    ///Author:		www.devtreks.org
    ///Date:		2016, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Extends the base object ME2Indicator object
    ///</summary>
    public class ME2Calculator : ME2Indicator
    {
        public ME2Calculator(CalculatorParameters calcParams)
            : base()
        {
            //calcParams
            this.CalcParameters = calcParams;
            //indicators
            InitME2Properties();
        }
        //copy constructor
        public ME2Calculator(ME2Calculator me2Calc)
            : base(me2Calc)
        {
            CopyME2Properties(me2Calc);
        }

        public virtual void InitME2Properties()
        {
            //this includes targettype and alterntype
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.InitME2IndicatorsProperties();
        }

        public virtual void CopyME2Properties(
            ME2Calculator calculator)
        {
            //this includes targettype and alterntype
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            this.CopyME2IndicatorsProperties(calculator);
        }
        //set the class properties using the XElement
        public virtual void SetME2Properties(XElement currentCalculationsElement,
            XElement currentElement)
        {
            //this includes targettype and alterntype
            this.SetCalculatorProperties(currentCalculationsElement);
            //need the aggregating params (label, groupid, typeid(
            this.SetSharedObjectProperties(currentElement);
            this.SetME2IndicatorsProperties(currentCalculationsElement);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetME2Property(string attName,
            string attValue, int colIndex)
        {
            this.SetME2IndicatorsProperty(attName, attValue, colIndex);
        }
        public void SetME2Attributes(string attNameExtension,
            ref XElement currentCalculationsElement)
        {
            //most calculators use this type of pattern, which removes unwanted old atts
            //SetInputAttributes(calcParameters, ref calculator, ref currentElement, updates);
            //this calculator does not change the base element, so that pattern isn't needed
            //but still must remove old unwanted attributes
            if (currentCalculationsElement != null)
            {
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(string.Empty, ref currentCalculationsElement);
            }
            this.SetME2IndicatorsAttributes(ref currentCalculationsElement);
        }
        public virtual void SetME2Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //this includes targettype and alterntype
            this.SetCalculatorAttributes(string.Empty, ref writer);
            this.SetME2IndicatorsAttributes(ref writer);
        }
        
        public bool SetME2Calculations(
            ME2CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement, 
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            string sErrorMessage = string.Empty;
            //deserialize xml to object
            this.SetME2Properties(currentCalculationsElement, currentElement);
            bHasCalculations = RunME2Calculations(calcParameters, ref sErrorMessage);
            //serialize object back to xml and fill in updates list
            this.SetME2Attributes(string.Empty, ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            calcParameters.ErrorMessage = sErrorMessage;
            return bHasCalculations;
        }
        public bool RunME2Calculations(CalculatorParameters calcParameters, ref string errorMsg)
        {
            bool bHasCalculations = false;
            //run other calcs
            bHasCalculations = this.RunCalculations(calcParameters);
            return bHasCalculations;
        }
    }
}

