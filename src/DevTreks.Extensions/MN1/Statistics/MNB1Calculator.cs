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
    ///Purpose:		Serialize and deserialize a food nutrition benefit calculator.
    ///             This calculator is used with outputs to calculate benefits.
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. Extends the base object MNB1Calculator object
    ///</summary>
    public class MNB1Calculator : MNSR1
    {
        public MNB1Calculator()
            : base()
        {
            //health care cost object
            InitMNB1Properties();
        }
        //copy constructor
        public MNB1Calculator(MNB1Calculator lca1Calc)
            : base(lca1Calc)
        {
            CopyMNB1Properties(lca1Calc);
        }
        
        //need to store locals and update parent output.trev
        public Output MNBOutput { get; set; }

        public virtual void InitMNB1Properties()
        {
            //avoid null references to properties
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.InitMNSR1Properties();
            this.MNBOutput = new Output();
        }

        public virtual void CopyMNB1Properties(
            MNB1Calculator calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            this.CopyMNSR1Properties(calculator);
            this.MNBOutput = new Output(calculator.MNBOutput);
        }
        //set the class properties using the XElement
        public virtual void SetMNB1Properties(XElement calculator,
            XElement currentElement)
        {
            this.SetCalculatorProperties(calculator);
            //need the aggregating params (label, groupid, typeid(
            this.SetSharedObjectProperties(currentElement);
            this.SetMNSR1Properties(calculator);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetMNB1Property(string attName,
            string attValue, int colIndex)
        {
            this.SetMNSR1Property(attName, attValue);
        }
       
        public void SetMNB1Attributes(string attNameExtension,
            ref XElement calculator)
        {
            //must remove old unwanted attributes
            if (calculator != null)
            {
                //do not remove atts here, they were removed in prior this.LCCOutput.SetOutputAtts
                //and now include good locals
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(attNameExtension, ref calculator);
            }
            this.SetMNSR1Attributes(attNameExtension, ref calculator);
        }
        public virtual void SetMNB1Attributes(string attNameExtension,
           ref XmlWriter writer)
        {
            //note must first use use either setanalyzeratts or SetCalculatorAttributes(attNameExtension, ref writer);
            this.SetMNSR1Attributes(attNameExtension, ref writer);
        }

        public bool SetMNB1Calculations(
            MN1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement calculator,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            string sErrorMessage = string.Empty;
            InitMNB1Properties();
            //deserialize xml to object
            //set the base output properties (including local)
            this.MNBOutput.SetOutputProperties(calcParameters,
                calculator, currentElement);
            this.SetMNB1Properties(calculator, currentElement);
            bHasCalculations = RunMNB1Calculations(calcParameters);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                //serialize object back to xml and fill in updates list
                //locals come from output
                this.MNBOutput.SetOutputAttributes(calcParameters,
                    ref currentElement, calcParameters.Updates);
            }
            else
            {
                //no db updates outside base output
                this.MNBOutput.SetOutputAttributes(calcParameters,
                    ref currentElement, null);
            }
            this.SetMNB1Attributes(string.Empty, ref calculator);
            //set the totals into calculator
            this.MNBOutput.SetNewOutputAttributes(calcParameters, ref calculator);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            calcParameters.ErrorMessage = sErrorMessage;
            return bHasCalculations;
        }
        public bool RunMNB1Calculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //first figure quantities
            UpdateBaseOutputUnitPrices(calcParameters);
            //then figure whether output.amount or compositionamount should be used as a multiplier
            double multiplier = GetMultiplierForMNSR1();
            bHasCalculations = this.RunMNSR1Calculations(calcParameters, multiplier);
            return bHasCalculations;
        }
        private void UpdateBaseOutputUnitPrices(
             CalculatorParameters calcParameters)
        {
            //check illegal divisors
            this.ContainerSizeInSSUnits = (this.ContainerSizeInSSUnits == 0)
                ? -1 : this.ContainerSizeInSSUnits;
            this.TypicalServingsPerContainer = this.ContainerSizeInSSUnits / this.TypicalServingSize;
            this.ActualServingsPerContainer = this.ContainerSizeInSSUnits / this.ActualServingSize;
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                //tech analysis can change from base
                //Actual serving size has to be 1 unit of hh measure
                this.MNBOutput.Amount = 1;
                this.MNBOutput.CompositionAmount = 1;
            }
            //calculate Price as a unit cost per serving (not per each unit of serving or ContainerSizeInSSUnits)
            this.MNBOutput.Price = this.ContainerPrice / this.ActualServingsPerContainer;
            //serving size is the unit cost 
            this.MNBOutput.Unit = string.Concat(this.ActualServingSize, " ", this.ServingSizeUnit);

            //old way
            ////calculate cost per actual serving
            //this.ServingCost = this.MNBOutput.Price * this.MNBOutput.Amount;
            //this.MNBOutput.TotalR = this.ServingCost;

            //new techniques to test and document
            //also test for base outputs and inputs only
            //benefits need to track the package price using calculator.props
            this.MNBOutput.CompositionUnit = this.ContainerUnit;
            //if (this.MNBOutput.CompositionAmount > 0)
            //comp amount defaults to 1 for good npv totals
            if (this.MNBOutput.CompositionAmount > 1)
            {
                this.MNBOutput.Price = this.ContainerPrice;
                this.TotalR = this.ContainerPrice * this.MNBOutput.CompositionAmount;
                this.ServingCost = this.TotalR;
                //do not use the amount to calculate food nutrients or benefits
                this.MNBOutput.Amount = 1;
            }
            else
            {
                //calculate cost per actual serving
                //note that this can change when the output is added elsewhere
                this.ServingCost = this.MNBOutput.Price * this.MNBOutput.Amount;
                this.MNBOutput.TotalR = this.ServingCost;
            }
        }
        public double GetMultiplierForMNSR1()
        {
            double multiplier = 1;
            if (this.MNBOutput.CompositionAmount > 1)
            {
                //cap budget can use container size for nutrient values
                multiplier = this.ActualServingsPerContainer * this.MNBOutput.CompositionAmount;
            }
            else
            {
                //op budget can use amount for nutrient values
                multiplier = this.MNBOutput.Amount;
            }
            return multiplier;
        }
    }
}
