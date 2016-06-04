using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Serialize and deserialize a food nutrition cost object.
    ///             This calculator is used with inputs to calculate costs.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Extends the base object MNSR1 object
    ///</summary>
    public class MNC1Calculator : MNSR1
    {
        public MNC1Calculator()
            : base()
        {
            //health care cost object
            InitMNC1Properties();
        }
        //copy constructor
        public MNC1Calculator(MNC1Calculator lca1Calc)
            : base(lca1Calc)
        {
            CopyMNC1Properties(lca1Calc);
        }

        //need to store locals and update parent input.ocprice, aohprice, capprice
        public Input MNCInput { get; set; }

        public virtual void InitMNC1Properties()
        {
            //avoid null references to properties
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.InitMNSR1Properties();
            this.MNCInput = new Input();
        }

        public virtual void CopyMNC1Properties(
            MNC1Calculator calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            this.CopyMNSR1Properties(calculator);
            this.MNCInput = new Input(calculator.MNCInput);
        }
        //set the class properties using the XElement
        public virtual void SetMNC1Properties(XElement calculator,
            XElement currentElement)
        {
            this.SetCalculatorProperties(calculator);
            //need the aggregating params (label, groupid, typeid and Date for sorting)
            this.SetSharedObjectProperties(currentElement);
            this.SetMNSR1Properties(calculator);
        }
        
        //attname and attvalue generally passed in from a reader
        public virtual void SetMNC1Property(string attName,
            string attValue)
        {
            this.SetMNSR1Property(attName, attValue);
            
        }
        
        public void SetMNC1Attributes(string attNameExtension,
            ref XElement calculator)
        {
            //must remove old unwanted attributes
            if (calculator != null)
            {
                //do not remove atts here, they were removed in prior this.MNCInput.SetInputAtts
                //and now include good locals
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(attNameExtension, ref calculator);
            }
            this.SetMNSR1Attributes(attNameExtension, ref calculator);
            
        }
        public virtual void SetMNC1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //note must first use use either setanalyzeratts or SetCalculatorAttributes(attNameExtension, ref writer);
            this.SetMNSR1Attributes(attNameExtension, ref writer);
        }

        public bool SetMNC1Calculations(
            MN1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement calculator,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            string sErrorMessage = string.Empty;
            InitMNC1Properties();
            //deserialize xml to object
            //set the base input properties (updates base input prices)
            //locals come from input
            this.MNCInput.SetInputProperties(calcParameters,
                calculator, currentElement);
            //set the calculator
            this.SetMNC1Properties(calculator, currentElement);
            bHasCalculations = RunMNC1Calculations(calcParameters);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                //serialize object back to xml and fill in updates list
                //this also removes atts
                this.MNCInput.SetInputAttributes(calcParameters,
                    ref currentElement, calcParameters.Updates);
            }
            else
            {
                //no db updates outside base output
                this.MNCInput.SetInputAttributes(calcParameters,
                    ref currentElement, null);
            }
            //this sets and removes some atts
            this.SetMNC1Attributes(string.Empty, ref calculator);
            //set the totals into calculator
            this.MNCInput.SetNewInputAttributes(calcParameters, ref calculator);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            calcParameters.ErrorMessage = sErrorMessage;
            return bHasCalculations;
        }
        public bool RunMNC1Calculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //first figure quantities
            UpdateBaseInputUnitPrices(calcParameters);
            //then figure whether ocamount or capamount should be used as a multiplier
            //times calcd by stock.multiplier
            double multiplier = GetMultiplierForMNSR1();
            //run cost calcs
            bHasCalculations = this.RunMNSR1Calculations(calcParameters, multiplier);
            
            return bHasCalculations;
        }
        private void UpdateBaseInputUnitPrices(
             CalculatorParameters calcParameters)
        {
            //is being able to change ins and outs in tech elements scalable?? double check

            //check illegal divisors
            this.ContainerSizeInSSUnits = (this.ContainerSizeInSSUnits == 0)
                ? -1 : this.ContainerSizeInSSUnits;
            this.TypicalServingsPerContainer = this.ContainerSizeInSSUnits / this.TypicalServingSize;
            this.ActualServingsPerContainer = this.ContainerSizeInSSUnits / this.ActualServingSize;
            //Actual serving size has to be 1 unit of hh measure
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                //tech analysis can change from base
                this.MNCInput.OCAmount = 1;
            }
            //calculate OCPrice as a unit cost per serving (not per each unit of serving or ContainerSizeInSSUnits)
            this.MNCInput.OCPrice = this.ContainerPrice / this.ActualServingsPerContainer;
            //serving size is the unit cost 
            this.MNCInput.OCUnit = string.Concat(this.ActualServingSize, " ", this.ServingSizeUnit);
            //transfer capprice (benefits need to track the package price using calculator.props)
            this.MNCInput.CAPPrice = this.ContainerPrice;
            this.MNCInput.CAPUnit = this.ContainerUnit;
            if (this.MNCInput.CAPAmount > 0)
            {
                this.ServingCost = this.MNCInput.CAPPrice * this.MNCInput.CAPAmount;
                this.MNCInput.TotalCAP = this.ServingCost;
            }
            else
            {
                //calculate cost per actual serving
                //note that this can change when the input is added elsewhere
                this.ServingCost = this.MNCInput.OCPrice * this.MNCInput.OCAmount;
                this.MNCInput.TotalOC = this.ServingCost;
            }
            
        }
        public double GetMultiplierForMNSR1()
        {
            double multiplier = 1;
            if (this.MNCInput.CAPAmount > 0)
            {
                //cap budget can use container size for nutrient values
                multiplier = this.ActualServingsPerContainer * this.MNCInput.CAPAmount;
            }
            else if (this.MNCInput.CAPAmount <= 0)
            {
                //op budget can use ocamount for nutrient values
                multiplier = this.MNCInput.OCAmount;
            }
            return multiplier;
        }
    }
}
