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
    ///Purpose:		Serialize and deserialize a Stock benefit calculator.
    ///             This calculator is used with outputs to calculate benefits.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1.  Extends the base object SBB1Calculator object
    ///</summary>
    public class SBB1Calculator : SB1Base
    {
        public SBB1Calculator()
            : base()
        {
            //health care cost object
            InitSB1B1Properties();
        }
        //copy constructor
        public SBB1Calculator(SBB1Calculator lca1Calc)
            : base(lca1Calc)
        {
            CopySB1B1Properties(lca1Calc);
        }
        
        //need to store locals and update parent output.trev
        public Output SB1BOutput { get; set; }

        public virtual void InitSB1B1Properties()
        {
            //avoid null references to properties
            this.InitCalculatorProperties();
            this.InitSharedObjectProperties();
            this.InitSB1BaseProperties();
            this.SB1BOutput = new Output();
        }

        public virtual void CopySB1B1Properties(
            SBB1Calculator calculator)
        {
            this.CopyCalculatorProperties(calculator);
            this.CopySharedObjectProperties(calculator);
            this.CopySB1BaseProperties(calculator);
            this.SB1BOutput = new Output(calculator.SB1BOutput);
        }
        //set the class properties using the XElement
        public virtual void SetSB1B1Properties(XElement calculator,
            XElement currentElement)
        {
            this.SetCalculatorProperties(calculator);
            //need the aggregating params (label, groupid, typeid(
            this.SetSharedObjectProperties(currentElement);
            this.SetSB1BaseProperties(calculator);
        }
        //attname and attvalue generally passed in from a reader
        public virtual void SetSB1B1Property(string attName,
            string attValue, int colIndex)
        {
            this.SetSB1BaseProperty(attName, attValue);
        }
       
        public void SetSB1B1Attributes(string attNameExtension,
            XElement calculator)
        {
            //must remove old unwanted attributes
            if (calculator != null)
            {
                //do not remove atts here, they were removed in prior this.LCCOutput.SetOutputAtts
                //and now include good locals
                //this also sets the aggregating atts
                this.SetAndRemoveCalculatorAttributes(attNameExtension, ref calculator);
            }
            this.SetSB1BaseAttributes(attNameExtension, calculator);
        }
        public virtual void SetSB1B1Attributes(string attNameExtension,
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
        public bool SetSB1B1Calculations(
           SB1CalculatorHelper.CALCULATOR_TYPES calculatorType,
           CalculatorParameters calcParameters, XElement calculator,
           XElement currentElement)
        {
            bool bHasCalculations = false;
            InitSB1B1Properties();
            //deserialize xml to object
            //set the base output properties (including local)
            this.SB1BOutput.SetOutputProperties(calcParameters,
                calculator, currentElement);
            this.SetSB1B1Properties(calculator, currentElement);
            bHasCalculations = RunSB1B1Calculations(calcParameters);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                //serialize object back to xml and fill in updates list
                this.SB1BOutput.SetOutputAttributes(calcParameters,
                    ref currentElement, calcParameters.Updates);
            }
            else
            {
            }
            this.SetSB1B1Attributes(string.Empty, calculator);
            //set the totals into calculator
            this.SB1BOutput.SetNewOutputAttributes(calcParameters, ref calculator);
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            return bHasCalculations;
        }
        public bool RunSB1B1Calculations(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //first figure quantities
            UpdateBaseOutputUnitPrices(calcParameters);
            //output.amount, compositionamount and times calcd by stock.multiplier (in Totals)
            double multiplier = 1;
            bHasCalculations = this.RunSB1BaseCalculations(calcParameters, multiplier);
            return bHasCalculations;
        }
        public async Task<bool> SetSB1B1CalculationsAsync(
            SB1CalculatorHelper.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, XElement calculator,
            XElement currentElement)
        {
            bool bHasCalculations = false;
            InitSB1B1Properties();
            //deserialize xml to object
            //set the base output properties (including local)
            this.SB1BOutput.SetOutputProperties(calcParameters,
                calculator, currentElement);
            this.SetSB1B1Properties(calculator, currentElement);
            bHasCalculations = await RunSB1B1CalculationsAsync(calcParameters);
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                //serialize object back to xml and fill in updates list
                this.SB1BOutput.SetOutputAttributes(calcParameters,
                    ref currentElement, calcParameters.Updates);
            }
            else
            {
            }
            this.SetSB1B1Attributes(string.Empty, calculator);
            //set the totals into calculator
            this.SB1BOutput.SetNewOutputAttributes(calcParameters, ref calculator);
            CalculatorHelpers.SetCalculatorId(
                calculator, currentElement);
            return bHasCalculations;
        }
        public async Task<bool> RunSB1B1CalculationsAsync(CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //first figure quantities
            //UpdateBaseOutputUnitPrices(calcParameters);
            //output.amount, compositionamount and times calcd by stock.multiplier (in Totals)
            double multiplier = 1;
            bHasCalculations = await this.RunSB1BaseCalculationsAsync(calcParameters, multiplier);
            //v186 update quantities after calcs run
            UpdateBaseOutputUnitPrices(calcParameters);
            return bHasCalculations;
        }
        private void UpdateBaseOutputUnitPrices(
             CalculatorParameters calcParameters)
        {
            //BaseIO indicators update base inputs and outputs
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
            {
                if (this.SB1BaseIO == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.benprice.ToString())
                {
                    this.SB1BOutput.Price = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.quantity.ToString())
                {
                    this.SB1BOutput.Amount = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.times.ToString())
                {
                    this.SB1BOutput.Times = this.SB1TMAmount20;
                }

                if (this.SB1BaseIO == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1ScoreM;
                }
                else if (this.SB1BaseIO1 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount1;
                }
                else if (this.SB1BaseIO2 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount2;
                }
                else if (this.SB1BaseIO3 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount3;
                }
                else if (this.SB1BaseIO4 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount4;
                }
                else if (this.SB1BaseIO5 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount5;
                }
                else if (this.SB1BaseIO6 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount6;
                }
                else if (this.SB1BaseIO7 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount7;
                }
                else if (this.SB1BaseIO8 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount8;
                }
                else if (this.SB1BaseIO9 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount9;
                }
                else if (this.SB1BaseIO10 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount10;
                }
                else if (this.SB1BaseIO11 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount11;
                }
                else if (this.SB1BaseIO12 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount12;
                }
                else if (this.SB1BaseIO13 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount13;
                }
                else if (this.SB1BaseIO14 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount14;
                }
                else if (this.SB1BaseIO15 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount15;
                }
                else if (this.SB1BaseIO16 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount16;
                }
                else if (this.SB1BaseIO17 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount17;
                }
                else if (this.SB1BaseIO18 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount18;
                }
                else if (this.SB1BaseIO19 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount19;
                }
                else if (this.SB1BaseIO20 == BASEIO_TYPES.composquantity.ToString())
                {
                    this.SB1BOutput.CompositionAmount = this.SB1TMAmount20;
                }
            }
            else
            {
                //but not join fields
            }
        }
    }
}
