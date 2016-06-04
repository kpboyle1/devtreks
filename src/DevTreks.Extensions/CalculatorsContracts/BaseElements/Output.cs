using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;

using DataHelpers = DevTreks.Data.Helpers;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataEditHelpers = DevTreks.Data.EditHelpers;
using RuleHelpers = DevTreks.Data.RuleHelpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The output class is a base class used by most output calculators.
    ///             (including output calculations carried out in other calculators)
    ///Author:		www.devtreks.org
    ///Date:		2014, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class Output : CostBenefitCalculator
    {
        public Output() 
        {
            Init();
        }
        //derived constants
        public const string OUTPUT_PRICE = DataAppHelpers.Prices.OUTPUT_PRICE1;
        public const string OUTPUT_AMOUNT = DataAppHelpers.Prices.OUTPUT_AMOUNT1;
        public const string OUTPUT_BASE_UNIT = DataAppHelpers.Prices.OUTPUT_UNIT1;
        public const string COMPOSITION_AMOUNT = DataAppHelpers.Prices.COMPOSITION_AMOUNT;
        public const string COMPOSITION_UNIT = DataAppHelpers.Prices.COMPOSITION_UNIT;
        public const string OUTPUT_TIMES = DataAppHelpers.Prices.OUTPUT_TIMES;
        public const string OUTPUT_ID = DataAppHelpers.Prices.OUTPUT_ID;
        public const string OUTPUT_GROUP_ID = DataAppHelpers.Prices.OUTPUT_GROUP_ID;
        public const string OUTPUT_GROUP_NAME = DataAppHelpers.Prices.OUTPUT_GROUP_NAME;
        public const string OUTPUT_DATE = DataAppHelpers.Prices.OUTPUT_DATE;
        /// <summary>
        /// Type of output price node being used.
        /// </summary>
        public enum OUTPUT_PRICE_TYPES
        {
            outputtype = 0,
            outputgroup = 1,
            output = 2,
            outputseries = 3
        }
        //properties
        //distinguish
        public string OutputName { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public double CompositionAmount { get; set; }
        public string CompositionUnit { get; set; }
        public double Times { get; set; }
        public double IncentiveRate { get; set; }
        public double IncentiveAmount { get; set; }
        public int OutputGroupId { get; set; }
        public string OutputGroupName { get; set; }
        //public string Name { get; set; }
        //misc calculators
        //used in calculations but not part of schema
        public TimePeriod.ANNUITY_TYPES AnnuityType { get; set; }
        public Local Local { get; set; }
        //linked calculators/analyzers
        public XElement XmlDocElement { get; set; }
        //collections
        public List<Output> Outputs { get; set; }
        public List<Calculator1> Calculators { get; set; }
        public void Init()
        {
            this.InitSharedObjectProperties();
            //init anything that will cause an exception when called
            this.Local = new Local();
            this.Outputs = new List<Output>();
            this.Calculators = new List<Calculator1>();
            this.OutputName = string.Empty;
            this.Amount = 0;
            this.Price = 0;
            this.Unit = string.Empty;
            this.CompositionAmount = 0;
            this.CompositionUnit = string.Empty;
            this.Times = 0;
            this.IncentiveAmount = 0;
            this.IncentiveRate = 0;
            this.OutputGroupId = 0;
            this.OutputGroupName = string.Empty;
        }
        //set the class properties using the XElement
        public void SetOutputProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes in base element
            this.SetCalculatorProperties(currentCalculationsElement);
            this.SetSharedObjectProperties(currentElement);
            this.SetTotalBenefitsProperties(currentElement);
            this.AnnuityType = TimePeriod.GetAnnuityType(currentElement);
            if (currentElement.Name.LocalName
                != DataAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                   DataAppHelpers.Prices.OUTPUT_DATE);
                this.Amount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    OUTPUT_AMOUNT);
                this.Price = CalculatorHelpers.GetAttributeDouble(currentElement,
                    OUTPUT_PRICE);
                this.Unit = CalculatorHelpers.GetAttribute(currentElement,
                    OUTPUT_BASE_UNIT);
                this.CompositionAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    COMPOSITION_AMOUNT);
                this.CompositionUnit = CalculatorHelpers.GetAttribute(currentElement,
                    COMPOSITION_UNIT);
                this.Times = CalculatorHelpers.GetAttributeDouble(currentElement,
                    OUTPUT_TIMES);
                this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.INCENTIVE_AMOUNT);
                this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.INCENTIVE_RATE);
                this.OutputGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    DataAppHelpers.Prices.OUTPUT_GROUP_ID);
                this.OutputGroupName = CalculatorHelpers.GetAttribute(currentElement,
                    DataAppHelpers.Prices.OUTPUT_GROUP_NAME);
                this.Local = new Local();
                string sCurrentNodeURIPattern
                   = CalculatorHelpers.MakeNewURIPatternFromElement(
                   calcParameters.ExtensionDocToCalcURI.URIPattern, currentElement);
                this.Local = CalculatorHelpers.GetLocal(sCurrentNodeURIPattern, calcParameters,
                    currentCalculationsElement, currentElement);
            }
            else
            {
                this.Date = CalculatorHelpers.GetAttributeDate(currentCalculationsElement,
                    OUTPUT_DATE);
                this.Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    OUTPUT_AMOUNT);
                this.Price = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    OUTPUT_PRICE);
                this.Unit = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    OUTPUT_BASE_UNIT);
                this.CompositionAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    COMPOSITION_AMOUNT);
                this.CompositionUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    COMPOSITION_UNIT);
                this.Times = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    OUTPUT_TIMES);
                this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.General.INCENTIVE_AMOUNT);
                this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.General.INCENTIVE_RATE);
                //use the calculator params to set locals (they can be changed in calcor)
                this.Local = new Local();
                this.Local.SetLocalProperties(calcParameters,
                    currentCalculationsElement, currentElement);
            }
            //joint output calcs can change this element
            this.XmlDocElement = currentCalculationsElement;
        }
        public void SetOutputProperties(XElement currentElement)
        {
            this.SetSharedObjectProperties(currentElement);
            this.SetTotalBenefitsProperties(currentElement);
            this.AnnuityType = TimePeriod.GetAnnuityType(currentElement);
            this.Amount = CalculatorHelpers.GetAttributeDouble(currentElement,
                OUTPUT_AMOUNT);
            this.Price = CalculatorHelpers.GetAttributeDouble(currentElement,
                OUTPUT_PRICE);
            this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                OUTPUT_DATE);
            this.Unit = CalculatorHelpers.GetAttribute(currentElement,
                OUTPUT_BASE_UNIT);
            this.CompositionAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                COMPOSITION_AMOUNT);
            this.CompositionUnit = CalculatorHelpers.GetAttribute(currentElement,
                COMPOSITION_UNIT);
            this.Times = CalculatorHelpers.GetAttributeDouble(currentElement,
                OUTPUT_TIMES);
            this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.General.INCENTIVE_AMOUNT);
            this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.General.INCENTIVE_RATE);
            this.OutputGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                DataAppHelpers.Prices.OUTPUT_GROUP_ID);
            this.OutputGroupName = CalculatorHelpers.GetAttribute(currentElement,
                DataAppHelpers.Prices.OUTPUT_GROUP_NAME);
        }
        
        public void CopyOutput(Output output)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes
            this.CopyCalculatorProperties(output);
            this.CopySharedObjectProperties(output);
            this.Amount = output.Amount;
            this.Price = output.Price;
            this.Unit = output.Unit;
            this.CompositionAmount = output.CompositionAmount;
            this.CompositionUnit = output.CompositionUnit;
            this.Times = output.Times;
            this.IncentiveAmount = output.IncentiveAmount;
            this.IncentiveRate = output.IncentiveRate;
            this.Type = output.Type;
            this.AnnuityType = output.AnnuityType;
            this.ErrorMessage = output.ErrorMessage;
            this.OutputGroupId = output.OutputGroupId;
            this.OutputGroupName = output.OutputGroupName;
            //better to set in base
            this.CopyTotalBenefitsProperties(output);
            //calculators are always app-specific and must be copied subsequently
            this.Calculators = new List<Calculator1>();
            if (output.Local == null)
                output.Local = new Local();
            this.Local = new Local(output.Local);
            if (output.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(output.XmlDocElement);
            }
        }
        //copy constructors
        public Output(Output output)
        {
            this.CopyOutput(output);
            if (output.Local == null)
                output.Local = new Local();
            this.Local = new Local(output.Local);
        }
        public Output(CalculatorParameters calcParameters, Output output)
        {
            this.CopyOutput(output);
            if (output.Local == null)
                output.Local = new Local();
            this.Local = new Local(calcParameters, output.Local);
        }
        public void SetOutputProperties(CalculatorParameters calcParameters, Output output)
        {
            this.CopyOutput(output);
            if (output.Local == null)
                output.Local = new Local();
            this.Local = new Local(calcParameters, output.Local);
        }
        //set the XElement parameter's attributes using this class
        public void SetOutputAttributes(CalculatorParameters calcParameters,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            //version 1.4.5 requires setting calculator atts separately (so specific calc can be used)
            //serialize the current element (note that outputdate has to be handled in shared objects)
            this.SetSharedObjectProperties(currentElement);
            if (currentElement.Name.LocalName
                != DataAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                     calcParameters.CurrentElementURIPattern, OUTPUT_AMOUNT,
                     this.Amount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                     calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, OUTPUT_PRICE,
                    this.Price.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, OUTPUT_DATE,
                    this.Date.ToString("s"), RuleHelpers.GeneralRules.DATE,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, OUTPUT_BASE_UNIT,
                    this.Unit, RuleHelpers.GeneralRules.STRING,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, COMPOSITION_AMOUNT,
                    this.CompositionAmount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, COMPOSITION_UNIT,
                    this.CompositionUnit, RuleHelpers.GeneralRules.STRING,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, OUTPUT_TIMES,
                    this.Times.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_AMOUNT,
                    this.IncentiveAmount.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_RATE,
                    this.IncentiveRate.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    if (this.Local == null)
                        this.Local = new Local();
                    this.Local.SetLocalAttributes(calcParameters,
                        ref currentElement, updates);
                }
            }
            //xmldoc atts are handled in the calculator helpers
        }
        //some output calculators will also use output attributes within the calculator
        public void SetNewOutputAttributes(CalculatorParameters calcParameters,
            ref XElement elementNeedingAttributes)
        {
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                OUTPUT_AMOUNT, this.Amount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                OUTPUT_PRICE, this.Price);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                OUTPUT_BASE_UNIT, this.Unit);
            CalculatorHelpers.SetAttributeDateS(elementNeedingAttributes,
               OUTPUT_DATE, this.Date);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                COMPOSITION_AMOUNT, this.CompositionAmount);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                COMPOSITION_UNIT, this.CompositionUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                OUTPUT_TIMES, this.Times);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.INCENTIVE_AMOUNT, this.IncentiveAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.INCENTIVE_RATE, this.IncentiveRate);
            //totals
            CalculatorHelpers.SetAttributeDoubleF2(elementNeedingAttributes,
                CostBenefitCalculator.TR, this.TotalR);
            CalculatorHelpers.SetAttributeDoubleF2(elementNeedingAttributes,
                CostBenefitCalculator.TR_INT, this.TotalR_INT);
            CalculatorHelpers.SetAttributeDoubleF2(elementNeedingAttributes,
                CostBenefitCalculator.TRINCENT, this.TotalRINCENT);
            CalculatorHelpers.SetAttributeDoubleF2(elementNeedingAttributes,
                CostBenefitCalculator.TAMR, this.TotalAMR);
            CalculatorHelpers.SetAttributeDoubleF2(elementNeedingAttributes,
                CostBenefitCalculator.TAMR_INT, this.TotalAMR_INT);
            if (this.Local != null)
            {
                //locals only for calculator
                this.Local.SetLocalAttributesForCalculator(calcParameters,
                    ref elementNeedingAttributes);
            }
        }
        
        public static void SetOutputAllAttributes(Output output,
            ref XElement elementNeedingAttributes)
        {
            CalculatorHelpers.SetAttributeInt(elementNeedingAttributes,
              Calculator1.cId, output.Id);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              Calculator1.cName, output.Name.ToString());
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              Calculator1.cDescription, output.Description.ToString());
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                OUTPUT_AMOUNT, output.Amount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                OUTPUT_PRICE, output.Price);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                OUTPUT_BASE_UNIT, output.Unit);
            CalculatorHelpers.SetAttributeDateS(elementNeedingAttributes,
               OUTPUT_DATE, output.Date);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                COMPOSITION_AMOUNT, output.CompositionAmount);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                COMPOSITION_UNIT, output.CompositionUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
               OUTPUT_TIMES, output.Times);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.INCENTIVE_AMOUNT, output.IncentiveAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.INCENTIVE_RATE, output.IncentiveRate);
            CalculatorHelpers.SetAttributeInt(elementNeedingAttributes,
               DataAppHelpers.Prices.OUTPUT_GROUP_ID, output.OutputGroupId);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.OUTPUT_GROUP_NAME, output.OutputGroupName);
        }
        public void SetNewOutputAttributes(
            string attNameExt, ref XmlWriter writer)
        {
            //object only
            this.SetSharedObjectAttributes(attNameExt, ref writer);
            writer.WriteAttributeString(
                  string.Concat(OUTPUT_AMOUNT, attNameExt), this.Amount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(OUTPUT_PRICE, attNameExt), this.Price.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(OUTPUT_BASE_UNIT, attNameExt), this.Unit);
            writer.WriteAttributeString(
                  string.Concat(OUTPUT_DATE, attNameExt), this.Date.ToShortDateString());
            writer.WriteAttributeString(
                  string.Concat(COMPOSITION_AMOUNT, attNameExt), this.CompositionAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(COMPOSITION_UNIT, attNameExt), this.CompositionUnit);
            writer.WriteAttributeString(
                  string.Concat(OUTPUT_TIMES, attNameExt), this.Times.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_AMOUNT, attNameExt), this.IncentiveAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_RATE, attNameExt), this.IncentiveRate.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.OUTPUT_GROUP_ID, attNameExt), this.OutputGroupId.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.OUTPUT_GROUP_NAME, attNameExt), this.OutputGroupName);
            
        }
    }
}
