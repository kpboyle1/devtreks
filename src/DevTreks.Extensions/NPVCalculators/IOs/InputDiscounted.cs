using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Support life cycle input calculations. 
    ///Author:		www.devtreks.org
    ///Date:		2012, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class InputDiscounted : Input
    {
        //calculator properties
        public bool IsDiscounted { get; set; }
        public DateTime EndOfPeriodDate { get; set; }
        public double EffectiveLife { get; set; }
        public double SalvageValue { get; set; }

        public void SetInputDiscountedProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //set the base input properties 
            SetInputProperties(calcParameters, currentCalculationsElement,
                currentElement);
            //set the calculator properties
            this.IsDiscounted = CalculatorHelpers.GetAttributeBool(currentCalculationsElement,
                BudgetInvestment.ISDISCOUNTED);
            this.EndOfPeriodDate = CalculatorHelpers.GetAttributeDate(currentCalculationsElement,
                OperationComponent.ENDOFPERIOD_DATE);
            this.EffectiveLife = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                OperationComponent.EFFECTIVE_LIFE);
            this.SalvageValue = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                OperationComponent.SALVAGE_VALUE);
            this.Times = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                Input.INPUT_TIMES);
            this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                Constants.INCENTIVE_AMOUNT);
            this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                Constants.INCENTIVE_RATE);
        }
        //copy constructor
        public void SetInputDiscountedProperties(CalculatorParameters calcParameters, InputDiscounted inputDiscounted)
        {
            //set the base input properties
            this.SetInputProperties(calcParameters, inputDiscounted);
            this.IsDiscounted = inputDiscounted.IsDiscounted;
            this.EndOfPeriodDate = inputDiscounted.EndOfPeriodDate;
            this.EffectiveLife = inputDiscounted.EffectiveLife;
            this.SalvageValue = inputDiscounted.SalvageValue;
            this.Times = inputDiscounted.Times;
        }
        //conversion method
        public static Input ConvertDiscountedInput(CalculatorParameters calcParameters, InputDiscounted inputDiscounted)
        {
            Input input = new Input();
            input.SetInputProperties(calcParameters, inputDiscounted);
            //only the calcs in the base input.total1 ... are needed in inputDiscounted
            return input;
        }
        public void SetInputDiscountedAttributes(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //set the base input's new calculations
            this.SetInputAttributes(calcParameters, ref currentElement, updates);
            //use the same attributes within the calculator as well
            this.SetNewInputAttributes(calcParameters, ref currentCalculationsElement);
            //and the totals
            this.SetTotalCostsAttributes(string.Empty, ref currentCalculationsElement);
            CalculatorHelpers.SetAttributeBool(currentCalculationsElement,
                BudgetInvestment.ISDISCOUNTED, this.IsDiscounted);
            CalculatorHelpers.SetAttributeDateS(currentCalculationsElement,
                OperationComponent.ENDOFPERIOD_DATE, this.EndOfPeriodDate);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                OperationComponent.EFFECTIVE_LIFE, this.EffectiveLife);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                OperationComponent.SALVAGE_VALUE, this.SalvageValue);
            CalculatorHelpers.SetAttributeDoubleF2(currentCalculationsElement,
                Input.INPUT_TIMES, this.Times);
        }
    }
}
