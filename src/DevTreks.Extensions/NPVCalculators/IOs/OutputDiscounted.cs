using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Support life cycle output calculations. 
    ///Author:		www.devtreks.org
    ///Date:		2012, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class OutputDiscounted : Output
    {
        //calculator properties
        public bool IsDiscounted { get; set; }
        public DateTime EndOfPeriodDate { get; set; }

        public void SetOutputDiscountedProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //set the base output properties 
            SetOutputProperties(calcParameters, currentCalculationsElement,
                currentElement);
            this.CompositionAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                COMPOSITION_AMOUNT);
            this.CompositionUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                COMPOSITION_UNIT);
            this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                Constants.INCENTIVE_AMOUNT);
            this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                Constants.INCENTIVE_RATE);
            //set the calculator properties
            this.IsDiscounted = CalculatorHelpers.GetAttributeBool(currentCalculationsElement,
                BudgetInvestment.ISDISCOUNTED);
            this.EndOfPeriodDate = CalculatorHelpers.GetAttributeDate(currentCalculationsElement,
                OperationComponent.ENDOFPERIOD_DATE);
            
        }
        //copy constructor
        public void SetOutputDiscountedProperties(CalculatorParameters calcParameters, 
            OutputDiscounted outputDiscounted)
        {
            //set the base output properties
            this.SetOutputProperties(calcParameters, outputDiscounted);
            //set this properties
            this.IsDiscounted = outputDiscounted.IsDiscounted;
            this.EndOfPeriodDate = outputDiscounted.EndOfPeriodDate;
        }
        //conversion method
        public static Output ConvertDiscountedOutput(CalculatorParameters calcParameters, 
            OutputDiscounted outputDiscounted)
        {
            Output output = new Output();
            output.SetOutputProperties(calcParameters, outputDiscounted);
            //only the calcs in the base output.total1 ... are needed in outputDiscounted
            return output;
        }
        
        public void SetOutputDiscountedAttributes(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //set the base output's new calculations
            this.SetOutputAttributes(calcParameters, ref currentElement, updates);
            //use the same attributes within the calculator as well
            this.SetNewOutputAttributes(calcParameters, ref currentCalculationsElement);
            //and the totals
            this.SetTotalBenefitsAttributes(string.Empty, ref currentCalculationsElement);
            CalculatorHelpers.SetAttributeBool(currentCalculationsElement,
               BudgetInvestment.ISDISCOUNTED, this.IsDiscounted);
            CalculatorHelpers.SetAttributeDateS(currentCalculationsElement,
                OperationComponent.ENDOFPERIOD_DATE, this.EndOfPeriodDate);
        }
    }
}
