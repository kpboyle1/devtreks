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
    ///Purpose:		The operation/component class is used by most 
    ///             operation and component calculators.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class OperationComponent : CostBenefitCalculator
    {
        public OperationComponent() 
        {
            Init();
        }
        //derived constants
        public const string ISPRICELIST = DataAppHelpers.Prices.ISPRICELIST;
        public const string WEIGHT = DataAppHelpers.Prices.WEIGHT;
        public const string EFFECTIVE_LIFE = DataAppHelpers.Prices.EFFECTIVE_LIFE;
        public const string INITIAL_VALUE = DataAppHelpers.Prices.INITIAL_VALUE;
        public const string SALVAGE_VALUE = DataAppHelpers.Prices.SALVAGE_VALUE;
        public const string ENDOFPERIOD_DATE = DataAppHelpers.Prices.ENDOFPERIOD_DATE;
        public const string OPERATION_ID = DataAppHelpers.Prices.OPERATION_ID;
        public const string OPERATION_GROUP_ID = DataAppHelpers.Prices.OPERATION_GROUP_ID;
        public const string OPERATION_GROUP_NAME = DataAppHelpers.Prices.OPERATION_GROUP_NAME;
        public const string COMPONENT_ID = DataAppHelpers.Prices.COMPONENT_ID;
        public const string COMPONENT_GROUP_ID = DataAppHelpers.Prices.COMPONENT_GROUP_ID;
        public const string COMPONENT_GROUP_NAME = DataAppHelpers.Prices.COMPONENT_GROUP_NAME;
        /// </summary>
        public enum OPERATION_PRICE_TYPES
        {
            operationtype = 0,
            operationgroup = 1,
            operation = 2,
            operationinput = 3
        }
        /// <summary>
        /// Type of component price node being used.
        /// see operation technology comment
        /// </summary>
        public enum COMPONENT_PRICE_TYPES
        {
            componenttype = 0,
            componentgroup = 1,
            component = 2,
            componentinput = 3
        }
        //properties
        public double Amount { get; set; }
        public string Unit { get; set; }
        public double Weight { get; set; }
        public double EffectiveLife { get; set; }
        public double SalvageValue { get; set; }
        public double IncentiveRate { get; set; }
        public double IncentiveAmount { get; set; }
        public int TimePeriodId { get; set; }
        //used in calculations but not part of schema
        public TimePeriod.ANNUITY_TYPES AnnuityType { get; set; }
        public int AnnuityCount { get; set; }
        public Local Local { get; set; }
        public List<Input> Inputs { get; set; }
        public List<Calculator1> Calculators { get; set; }
        //linked calculators/analyzers
        public XElement XmlDocElement { get; set; }

        public void Init()
        {
            this.InitSharedObjectProperties();
            //init anything that will cause an exception when called
            this.Local = new Local();
            this.Inputs = new List<Input>();
            this.Calculators = new List<Calculator1>();
            this.Amount = 0;
            this.Unit = string.Empty;
            this.Weight =0;
            this.EffectiveLife = 0;
            this.SalvageValue = 0;
            this.IncentiveAmount = 0;
            this.IncentiveRate = 0;
            this.TimePeriodId = 0;
            this.AnnuityType = TimePeriod.ANNUITY_TYPES.none;
        }
        //set the class properties using the XElement
        public void SetOperationComponentProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes in base element
            this.SetCalculatorProperties(currentCalculationsElement);
            this.SetSharedObjectProperties(currentElement);
            this.SetTotalCostsProperties(currentElement);
            this.AnnuityType = TimePeriod.GetAnnuityType(currentElement);
            if (currentElement.Name.LocalName
                != DataAppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString()
                && currentElement.Name.LocalName
                != DataAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                this.Amount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.AMOUNT);
                this.Date = CalculatorHelpers.GetAttributeDate(currentElement,
                    ENDOFPERIOD_DATE);
                this.Unit = CalculatorHelpers.GetAttribute(currentElement,
                    DataAppHelpers.General.UNIT);
                this.Weight = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.WEIGHT);
                this.EffectiveLife = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.EFFECTIVE_LIFE);
                this.SalvageValue = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Prices.SALVAGE_VALUE);
                this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.INCENTIVE_AMOUNT);
                this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.General.INCENTIVE_RATE);
                if (this.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets.ToString())
                {
                    this.TimePeriodId = CalculatorHelpers.GetAttributeInt(currentElement,
                        DataAppHelpers.Economics1.BUDGETSYSTEM_TO_TIME_ID);
                }
                else if (this.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments.ToString())
                {
                    this.TimePeriodId = CalculatorHelpers.GetAttributeInt(currentElement,
                        DataAppHelpers.Economics1.COSTSYSTEM_TO_TIME_ID);
                }
            }
            this.AnnuityCount = 0;
            this.Local = new Local();
            string sCurrentNodeURIPattern
               = CalculatorHelpers.MakeNewURIPatternFromElement(
               calcParameters.ExtensionDocToCalcURI.URIPattern, currentElement);
            this.Local = CalculatorHelpers.GetLocal(sCurrentNodeURIPattern, calcParameters,
                currentCalculationsElement, currentElement);
            this.XmlDocElement = currentCalculationsElement;
        }

        //copy constructor
        public OperationComponent(CalculatorParameters calcParameters, 
            OperationComponent operationorcomponent)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes
            this.CopyCalculatorProperties(operationorcomponent);
            this.CopySharedObjectProperties(operationorcomponent);
            this.Amount = operationorcomponent.Amount;
            this.Unit = operationorcomponent.Unit;
            this.Weight = operationorcomponent.Weight;
            this.EffectiveLife = operationorcomponent.EffectiveLife;
            this.SalvageValue = operationorcomponent.SalvageValue;
            this.IncentiveAmount = operationorcomponent.IncentiveAmount;
            this.IncentiveRate = operationorcomponent.IncentiveRate;
            this.TimePeriodId = operationorcomponent.TimePeriodId;
            this.Type = operationorcomponent.Type;
            this.ErrorMessage = operationorcomponent.ErrorMessage;
            this.AnnuityType = operationorcomponent.AnnuityType;
            this.AnnuityCount = operationorcomponent.AnnuityCount;
            //better to set in base
            this.CopyTotalCostsProperties(operationorcomponent);
            //calculators are always app-specific and must be copied subsequently
            this.Calculators = new List<Calculator1>();
            if (operationorcomponent.Local == null)
                operationorcomponent.Local = new Local();
            this.Local = new Local(calcParameters, operationorcomponent.Local);
            if (operationorcomponent.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(operationorcomponent.XmlDocElement);
            }
        }

        //set the XElement parameter's attributes using this class
        public void SetOperationComponentAttributes(
            CalculatorParameters calcParameters, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //version 1.4.5 requires setting calculator atts separately (so specific calc can be used)
            //serialize the current element (no need to remove atts) 
            this.SetSharedObjectAttributes(string.Empty, ref currentElement);
            this.SetTotalCostsAttributes(string.Empty, ref currentElement);
            if (currentElement.Name.LocalName
                != DataAppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString()
                && currentElement.Name.LocalName
                != DataAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, ENDOFPERIOD_DATE,
                   this.Date.ToString("s"), RuleHelpers.GeneralRules.DATE,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.AMOUNT,
                    this.Amount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.General.UNIT,
                   this.Unit, RuleHelpers.GeneralRules.STRING,
                   calcParameters.StepNumber, ref currentElement,  updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.WEIGHT,
                   this.Weight.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.EFFECTIVE_LIFE,
                   this.EffectiveLife.ToString("f2"), RuleHelpers.GeneralRules.FLOAT,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                   calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.SALVAGE_VALUE,
                   this.SalvageValue.ToString("f2"), RuleHelpers.GeneralRules.DECIMAL,
                   calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_AMOUNT,
                    this.IncentiveAmount.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_RATE,
                    this.IncentiveRate.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                    calcParameters.StepNumber, ref currentElement, updates);
            }
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
                || calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                if (this.Local != null)
                {
                    this.Local.SetLocalAttributes(calcParameters,
                        ref currentElement, updates);
                }
            }
            //xmldoc atts are handled in the calculator helpers
        }
        //serialize regular properties
        public void SetNewOperationComponentAttributes(
            ref XElement elementNeedingAttributes)
        {
            CalculatorHelpers.SetAttributeDateS(elementNeedingAttributes,
               ENDOFPERIOD_DATE, this.Date);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.AMOUNT, this.Amount);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
                DataAppHelpers.General.UNIT, this.Unit);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.WEIGHT, this.Weight);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.EFFECTIVE_LIFE, this.EffectiveLife);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.SALVAGE_VALUE, this.SalvageValue);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.INCENTIVE_AMOUNT, this.IncentiveAmount);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.General.INCENTIVE_RATE, this.IncentiveRate);
            CalculatorHelpers.SetAttribute(elementNeedingAttributes,
              DataAppHelpers.Economics1.ANNUITY_TYPE, this.AnnuityType.ToString());
        }
        public void SetNewOperationComponentAttributes(
            string attNameExt, ref XmlWriter writer)
        {
            //object only (base object sets name, date (eopdate)
            this.SetSharedObjectAttributes(attNameExt, ref writer);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.AMOUNT, attNameExt), this.Amount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.UNIT, attNameExt), this.Unit);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.WEIGHT, attNameExt), this.Weight.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.EFFECTIVE_LIFE, attNameExt), this.EffectiveLife.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.SALVAGE_VALUE, attNameExt), this.SalvageValue.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_AMOUNT, attNameExt), this.IncentiveAmount.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_RATE, attNameExt), this.IncentiveRate.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ANNUITY_TYPE, attNameExt), this.AnnuityType.ToString());
        }
    }
    
}
