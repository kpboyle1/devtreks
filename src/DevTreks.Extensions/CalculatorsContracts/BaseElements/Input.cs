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
    ///Purpose:		The input class is a base class used by most input calculators
    ///             (including input calculations carried out in other calculators)
    ///Author:		www.devtreks.org
    ///Date:		2014, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class Input : CostBenefitCalculator
    {
        public Input() 
        {
            Init();
        }
        //derived constants
        public const string INPUT_DATE = DataAppHelpers.Prices.INPUT_DATE;
        public const string OC_AMOUNT = DataAppHelpers.Prices.OC_AMOUNT;
        public const string OC_PRICE = DataAppHelpers.Prices.OC_PRICE;
        public const string OC_UNIT = DataAppHelpers.Prices.OC_UNIT;
        public const string AOH_AMOUNT = DataAppHelpers.Prices.AOH_AMOUNT;
        public const string AOH_PRICE = DataAppHelpers.Prices.AOH_PRICE;
        public const string AOH_UNIT = DataAppHelpers.Prices.AOH_UNIT;
        public const string CAP_AMOUNT = DataAppHelpers.Prices.CAP_AMOUNT;
        public const string CAP_PRICE = DataAppHelpers.Prices.CAP_PRICE;
        public const string CAP_UNIT = DataAppHelpers.Prices.CAP_UNIT;
        public const string USE_AOH = DataAppHelpers.Prices.USE_AOH;
        public const string INPUT_TIMES = DataAppHelpers.Prices.INPUT_TIMES;
        public const string INPUT_ID = DataAppHelpers.Prices.INPUT_ID;
        public const string INPUT_GROUP_ID = DataAppHelpers.Prices.INPUT_GROUP_ID;
        public const string INPUT_GROUP_NAME = DataAppHelpers.Prices.INPUT_GROUP_NAME;
        /// <summary>
        /// Type of input price node being used.
        /// </summary>
        public enum INPUT_PRICE_TYPES
        {
            inputtype = 0,
            inputgroup = 1,
            input = 2,
            inputseries = 3
        }
        public static INPUT_PRICE_TYPES ConvertNodeNameStringToEnum(string nodeName)
        {
            INPUT_PRICE_TYPES eNodeName
                = (!string.IsNullOrEmpty(nodeName))
                ? (INPUT_PRICE_TYPES)Enum.Parse(typeof(INPUT_PRICE_TYPES), nodeName)
                : INPUT_PRICE_TYPES.input;
            return eNodeName;
        }
        //properties
        public double OCAmount { get; set; }
        public double OCPrice { get; set; }
        public string OCUnit { get; set; }
        public double AOHAmount { get; set; }
        public double AOHPrice { get; set; }
        public string AOHUnit { get; set; }
        public double CAPAmount { get; set; }
        public double CAPPrice { get; set; }
        public string CAPUnit { get; set; }
        public int InputGroupId { get; set; }
        public string InputGroupName { get; set; }
        //join side
        public double IncentiveRate { get; set; }
        public double IncentiveAmount { get; set; }
        public double Times { get; set; }
        public bool UseAOH { get; set; }
        public int OpCompId { get; set; }
        //misc calculators
        //used in calculations but not part of schema
        public TimePeriod.ANNUITY_TYPES AnnuityType { get; set; }
        public Local Local { get; set; }
        //linked calculators/analyzers
        public XElement XmlDocElement { get; set; }
        //collections
        public List<Input> Inputs { get; set; }
        public List<Calculator1> Calculators { get; set; }
        public void Init()
        {
            this.InitSharedObjectProperties();
            //init anything that will cause an exception when called
            this.Local = new Local();
            this.Inputs = new List<Input>();
            this.Calculators = new List<Calculator1>();
            this.OCAmount = 0;
            this.OCPrice = 0;
            this.OCUnit = string.Empty;
            this.AOHAmount = 0;
            this.AOHPrice = 0;
            this.AOHUnit = string.Empty;
            this.CAPAmount = 0;
            this.CAPPrice = 0;
            this.CAPUnit = string.Empty;
            this.IncentiveAmount = 0;
            this.IncentiveRate = 0;
            this.AnnuityType = TimePeriod.ANNUITY_TYPES.none;
            this.InputGroupId = 0;
            this.InputGroupName = string.Empty;
            this.Times = 0;
            this.UseAOH = false;
            this.OpCompId = 0;
        }
        //set the class properties using the XElement
        public void SetInputProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes in base element
            this.SetCalculatorProperties(currentCalculationsElement);
            this.SetSharedObjectProperties(currentElement);
            this.SetTotalCostsProperties(currentElement);
            this.AnnuityType = TimePeriod.GetAnnuityType(currentElement);
            //input calcs can be run from group nodes
            if (currentElement.Name.LocalName
                != DataAppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                //set this object's specific properties
                this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                    DataAppHelpers.Prices.INPUT_DATE);
                this.OCAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.OC_AMOUNT);
                this.OCPrice = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.OC_PRICE);
                this.OCUnit = CalculatorHelpers.GetAttribute(currentElement,
                    DataAppHelpers.Prices.OC_UNIT);
                this.AOHAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.AOH_AMOUNT);
                this.AOHPrice = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.AOH_PRICE);
                this.AOHUnit = CalculatorHelpers.GetAttribute(currentElement,
                    DataAppHelpers.Prices.AOH_UNIT);
                this.CAPAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.CAP_AMOUNT);
                this.CAPPrice = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.CAP_PRICE);
                this.CAPUnit = CalculatorHelpers.GetAttribute(currentElement,
                    DataAppHelpers.Prices.CAP_UNIT);
                this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.INCENTIVE_AMOUNT);
                this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.INCENTIVE_RATE);
                this.InputGroupId = CalculatorHelpers.GetAttributeInt(currentElement,
                    DataAppHelpers.Prices.INPUT_GROUP_ID);
                this.InputGroupName = CalculatorHelpers.GetAttribute(currentElement,
                    DataAppHelpers.Prices.INPUT_GROUP_NAME);
                this.Times = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.INPUT_TIMES);
                this.UseAOH = CalculatorHelpers.GetAttributeBool(currentElement,
                    DataAppHelpers.Prices.USE_AOH);
                this.Local = new Local();
                string sCurrentNodeURIPattern
                    = CalculatorHelpers.MakeNewURIPatternFromElement(
                    calcParameters.ExtensionDocToCalcURI.URIPattern, currentElement);
                this.Local = CalculatorHelpers.GetLocal(sCurrentNodeURIPattern, calcParameters,
                    currentCalculationsElement, currentElement);
            }
            else
            {
                //set this object's properties using calculator attributes
                this.Date = CalculatorHelpers.GetAttributeDate(currentCalculationsElement,
                   INPUT_DATE);
                this.OCAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.OC_AMOUNT);
                this.OCPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.OC_PRICE);
                this.OCUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    DataAppHelpers.Prices.OC_UNIT);
                this.AOHAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.AOH_AMOUNT);
                this.AOHPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.AOH_PRICE);
                this.AOHUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    DataAppHelpers.Prices.AOH_UNIT);
                this.CAPAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.CAP_AMOUNT);
                this.CAPPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.CAP_PRICE);
                this.CAPUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                    DataAppHelpers.Prices.CAP_UNIT);
                this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.General.INCENTIVE_AMOUNT);
                this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.General.INCENTIVE_RATE);
                this.Times = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement,
                    DataAppHelpers.Prices.INPUT_TIMES);
                this.UseAOH = CalculatorHelpers.GetAttributeBool(currentCalculationsElement,
                    DataAppHelpers.Prices.USE_AOH);
                //use the calculator params to set locals (they can be changed in calcor)
                this.Local = new Local();
                this.Local.SetLocalProperties(calcParameters,
                    currentCalculationsElement, currentElement);
            }
            //joint input calcs can change this element
            this.XmlDocElement = currentCalculationsElement;
        }
        
        public void CopyInput(Input input)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes
            this.CopyCalculatorProperties(input);
            this.CopySharedObjectProperties(input);
            this.OCAmount = input.OCAmount;
            this.OCPrice = input.OCPrice;
            this.OCUnit = input.OCUnit;
            this.AOHAmount = input.AOHAmount;
            this.AOHPrice = input.AOHPrice;
            this.AOHUnit = input.AOHUnit;
            this.CAPAmount = input.CAPAmount;
            this.CAPPrice = input.CAPPrice;
            this.CAPUnit = input.CAPUnit;
            this.IncentiveAmount = input.IncentiveAmount;
            this.IncentiveRate = input.IncentiveRate;
            this.Type = input.Type;
            this.ErrorMessage = input.ErrorMessage;
            this.AnnuityType = input.AnnuityType;
            this.InputGroupId = input.InputGroupId;
            this.InputGroupName = input.InputGroupName;
            this.Times = input.Times;
            this.UseAOH = input.UseAOH;
            this.OpCompId = input.OpCompId;
            //better to set in base
            this.CopyTotalCostsProperties(input);
            //calculators are always app-specific and must be copied subsequently
            this.Calculators = new List<Calculator1>();
            if (input.Local == null)
                input.Local = new Local();
            this.Local = new Local(input.Local);
            if (input.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(input.XmlDocElement);
            }
        }
        //copy constructor
        public Input(Input input)
        {
            this.CopyInput(input);
            if (input.Local == null)
                input.Local = new Local();
            this.Local = new Local(input.Local);
        }
        //copy constructor
        public Input(CalculatorParameters calcParameters, Input input)
        {
            this.CopyInput(input);
            if (input.Local == null)
                input.Local = new Local();
            this.Local = new Local(calcParameters, input.Local);
        }
        public void SetInputProperties(CalculatorParameters calcParameters, Input input)
        {
            this.CopyInput(input);
            if (input.Local == null)
                input.Local = new Local();
            this.Local = new Local(calcParameters, input.Local);
        }
       
        public void SetInputAttributes(CalculatorParameters calcParameters,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            //version 1.4.5 requires setting calculator atts separately (so specific calc can be used)
            //(note that inputdate has to be handled in shared objects
            this.SetSharedObjectAttributes(string.Empty, ref currentElement);
            //172: inputs have unique date property
            CalculatorHelpers.SetAttributeDateS(currentElement, INPUT_DATE, this.Date);
            if (currentElement.Name.LocalName
                != DataAppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.OC_AMOUNT,
                    this.OCAmount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.OC_PRICE,
                    this.OCPrice.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.OC_UNIT,
                    this.OCUnit, RuleHelpers.GeneralRules.STRING,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.AOH_AMOUNT,
                    this.AOHAmount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.AOH_PRICE,
                    this.AOHPrice.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.AOH_UNIT,
                    this.AOHUnit, RuleHelpers.GeneralRules.STRING,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.CAP_AMOUNT,
                    this.CAPAmount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.CAP_PRICE,
                    this.CAPPrice.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.CAP_UNIT,
                    this.CAPUnit, RuleHelpers.GeneralRules.STRING,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_AMOUNT,
                    this.IncentiveAmount.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_RATE,
                    this.IncentiveRate.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.INPUT_DATE,
                   this.Date.ToString("s"), RuleHelpers.GeneralRules.DATE,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.INPUT_TIMES,
                    this.Times.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.USE_AOH,
                    this.UseAOH.ToString(), RuleHelpers.GeneralRules.BOOLEAN,
                    calcParameters.StepNumber, ref currentElement, updates);
                if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    if (this.Local == null)
                        this.Local = new Local();
                    this.Local.SetLocalAttributes(calcParameters,
                        ref currentElement, updates);
                }
            }
            //xmldoc atts are handled in the calculator helpers
        }
        //serialize regular input properties
        public void SetNewInputAttributes(CalculatorParameters calcParameters,
            ref XElement elementNeedingAttributes)
        {
            CalculatorHelpers.SetAttributeDateS(elementNeedingAttributes,
               DataAppHelpers.Prices.INPUT_DATE, this.Date);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.OC_AMOUNT, this.OCAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.OC_PRICE, this.OCPrice);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.OC_UNIT, this.OCUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.AOH_AMOUNT, this.AOHAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.AOH_PRICE, this.AOHPrice);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.AOH_UNIT, this.AOHUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.CAP_AMOUNT, this.CAPAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.CAP_PRICE, this.CAPPrice);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.CAP_UNIT, this.CAPUnit);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              DataAppHelpers.Economics1.ANNUITY_TYPE, this.AnnuityType.ToString());
            if (this.Local != null)
            {
                //locals only for calculator
                this.Local.SetLocalAttributesForCalculator(calcParameters,
                    ref elementNeedingAttributes);
            }
        }
        public static void SetInputAllAttributes(Input input,
            ref XElement elementNeedingAttributes)
        {
            CalculatorHelpers.SetAttributeInt(elementNeedingAttributes,
              Calculator1.cId, input.Id);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              Calculator1.cName, input.Name.ToString());
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              Calculator1.cDescription, input.Description.ToString());
            CalculatorHelpers.SetAttributeDateS(elementNeedingAttributes,
               DataAppHelpers.Prices.INPUT_DATE, input.Date);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.OC_AMOUNT, input.OCAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.OC_PRICE, input.OCPrice);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.OC_UNIT, input.OCUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.AOH_AMOUNT, input.AOHAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.AOH_PRICE, input.AOHPrice);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.AOH_UNIT, input.AOHUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.CAP_AMOUNT, input.CAPAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.CAP_PRICE, input.CAPPrice);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.Prices.CAP_UNIT, input.CAPUnit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
              DataAppHelpers.General.INCENTIVE_AMOUNT, input.IncentiveAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
              DataAppHelpers.General.INCENTIVE_RATE, input.IncentiveRate);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
              DataAppHelpers.Prices.INPUT_TIMES, input.Times);
            CalculatorHelpers.SetAttributeBool(elementNeedingAttributes,
              DataAppHelpers.Prices.USE_AOH, input.UseAOH);
            CalculatorHelpers.SetAttributeInt(elementNeedingAttributes,
              DataAppHelpers.Prices.INPUT_GROUP_ID, input.InputGroupId);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              DataAppHelpers.Prices.INPUT_GROUP_NAME, input.InputGroupName.ToString());
        }
        public void SetNewInputAttributes(string attNameExt, 
            ref XmlWriter writer)
        {
            //object only
            this.SetSharedObjectAttributes(attNameExt, ref writer);
            writer.WriteAttributeString(
                 string.Concat(DataAppHelpers.Prices.INPUT_DATE, attNameExt), this.Date.ToShortDateString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.OC_AMOUNT, attNameExt), this.OCAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.OC_PRICE, attNameExt), this.OCPrice.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.OC_UNIT, attNameExt), this.OCUnit);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.AOH_AMOUNT, attNameExt), this.AOHAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.AOH_PRICE, attNameExt), this.AOHPrice.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.AOH_UNIT, attNameExt), this.AOHUnit);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.CAP_AMOUNT, attNameExt), this.CAPAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.CAP_PRICE, attNameExt), this.CAPPrice.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.CAP_UNIT, attNameExt), this.CAPUnit);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_AMOUNT, attNameExt), this.IncentiveAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_RATE, attNameExt), this.IncentiveRate.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.INPUT_TIMES, attNameExt), this.Times.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.USE_AOH, attNameExt), this.UseAOH.ToString());
        }
    }
}
