using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Serialize and deserialize a Stock cost object.
    ///             This calculator is used with inputs to calculate costs.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1.  Extends the base object SB1Base object
    ///</summary>
    public class SBC1Calculator : SB1Base
    {
        public SBC1Calculator()
            : base()
        {
            //health care cost object
            InitSB1C1Properties();
        }
        //copy constructor
        public SBC1Calculator(SBC1Calculator lca1Calc)
            : base(lca1Calc)
        {
            CopySB1C1Properties(lca1Calc);
        }

        //need to store locals and update parent input.ocprice, aohprice, capprice
        public Input SB1CInput { get; set; }

        public virtual void InitSB1C1Properties()
        {
            //avoid null references to properties
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.InitSB1BaseProperties();
            this.SB1CInput = new Input();
        }

        public virtual void CopySB1C1Properties(
            SBC1Calculator calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            this.CopySB1BaseProperties(calculator);
            this.SB1CInput = new Input(calculator.SB1CInput);
        }
        //set the class properties using the XElement
        public virtual void SetSB1C1Properties(XElement calculator,
            XElement currentElement)
        {
            this.SetCalculatorProperties(calculator);
            //need the aggregating params (label, groupid, typeid and Date for sorting)
            this.SetSharedObjectProperties(currentElement);
            this.SetSB1BaseProperties(calculator);
        }
        
        //attname and attvalue generally passed in from a reader
        public virtual void SetSB1C1Property(string attName,
            string attValue)
        {
            this.SetSB1BaseProperty(attName, attValue);
            
        }
        
        public void SetSB1C1Attributes(string attNameExtension,
            XElement calculator)
        {
            //must remove old unwanted attributes
            if (calculator != null)
            {
                //do not remove atts here, they were removed in prior this.SB1CInput.SetInputAtts
                //and now include good locals
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(attNameExtension, calculator);
            }
            this.SetSB1BaseAttributes(attNameExtension, calculator);
            
        }
        
        public virtual void SetSB1C1Attributes(string attNameExtension,
           XmlWriter writer)
        {
            //note must first use use either setanalyzeratts or SetCalculatorAttributes(attNameExtension, ref writer);
            this.SetSB1BaseAttributes(attNameExtension, writer);
        }
        public virtual async Task SetSB1B1AttributesAsync(string attNameExtension,
           XmlWriter writer)
        {
            //note must first use use either setanalyzeratts or SetCalculatorAttributes(attNameExtension, ref writer);
            await this.SetSB1BaseAttributesAsync(attNameExtension, writer);
        }
        public bool SetSB1C1Calculations(
            SB1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, XElement calculator,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            InitSB1C1Properties();
            //deserialize xml to object
            //set the base input properties 
            //locals come from input
            this.SB1CInput.SetInputProperties(calcParameters,
                calculator, currentElement);
            //set the calculator
            this.SetSB1C1Properties(calculator, currentElement);
            bHasCalculations = RunSB1C1Calculations(calcParameters);
            //serialize object back to xml and fill in updates list
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                this.SB1CInput.SetInputAttributes(calcParameters,
                    ref currentElement, calcParameters.Updates);
            }
            else
            {

            }
            //this sets and removes some atts
            this.SetSB1C1Attributes(string.Empty, calculator);
            //set the totals into calculator
            this.SB1CInput.SetNewInputAttributes(calcParameters, ref calculator);
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            return bHasCalculations;
        }
        public bool RunSB1C1Calculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //first figure quantities
            UpdateBaseInputUnitAmount(calcParameters);
            //output.amount, compositionamount and times calcd by stock.multiplier (in Totals)
            double multiplier = 1;
            bHasCalculations = this.RunSB1BaseCalculations(calcParameters, multiplier);
            return bHasCalculations;
        }
        public async Task<bool> SetSB1C1CalculationsAsync(
            SB1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, XElement calculator,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            InitSB1C1Properties();
            //deserialize xml to object
            //set the base input properties 
            //locals come from input
            this.SB1CInput.SetInputProperties(calcParameters,
                calculator, currentElement);
            //set the calculator
            this.SetSB1C1Properties(calculator, currentElement);
            bHasCalculations = await RunSB1C1CalculationsAsync(calcParameters);
            //serialize object back to xml and fill in updates list
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                this.SB1CInput.SetInputAttributes(calcParameters,
                    ref currentElement, calcParameters.Updates);
            }
            //this sets and removes some atts
            this.SetSB1C1Attributes(string.Empty, calculator);
            //set the totals into calculator
            this.SB1CInput.SetNewInputAttributes(calcParameters, ref calculator);
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            return bHasCalculations;
        }
        
        public async Task<bool> RunSB1C1CalculationsAsync(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //first figure quantities
            //UpdateBaseInputUnitAmount(calcParameters);
            //output.amount, compositionamount and times calcd by stock.multiplier (in Totals)
            double multiplier = 1;
            bHasCalculations = await this.RunSB1BaseCalculationsAsync(calcParameters, multiplier);
            //v186 update quantities after calcs run
            UpdateBaseInputUnitAmount(calcParameters);
            return bHasCalculations;
        }
        private void UpdateBaseInputUnitAmount(
             CalculatorParameters calcParameters)
        {
            //BaseIO indicators update base inputs and outputs
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
            {
                if (this.SB1BaseIO == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.ocprice.ToString())
                {
                    this.SB1CInput.OCPrice = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.aohprice.ToString())
                {
                    this.SB1CInput.AOHPrice = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.capprice.ToString())
                {
                    this.SB1CInput.CAPPrice = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1CInput.OCAmount = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1CInput.Times = this.SB1TMAmount20;
                }
            }
            else
            {
                //but not join fields
            }
           
        }
    }
}
