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
    ///Purpose:		The timeperiod class is used by most economics calculators.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class TimePeriod : CostBenefitCalculator
    {
        public TimePeriod() 
        {
            Init();
        }

        /// <summary>
        /// type of annuity used in estimates
        /// </summary>
        public enum ANNUITY_TYPES
        {
            none            = 0,
            regular         = 1,
            preproduction   = 2
        }
        /// <summary>
        /// type of growth series to extend profit and investment flows into the future
        /// </summary>
        public enum GROWTH_SERIES_TYPES
        {
            none            = 0,
            uniform         = 1,
            linear          = 2,
            geometric       = 3
        }
        public static ANNUITY_TYPES GetAnnuityType(XElement el)
        {
            ANNUITY_TYPES eAnnuityType = ANNUITY_TYPES.none;
            string sAnnuityType = CalculatorHelpers.GetAttribute(el,
                DataAppHelpers.Economics1.ANNUITY_TYPE);
            if (!string.IsNullOrEmpty(sAnnuityType))
            {
                if (sAnnuityType == ANNUITY_TYPES.preproduction.ToString())
                {
                    eAnnuityType = ANNUITY_TYPES.preproduction;
                }
                else if (sAnnuityType == ANNUITY_TYPES.regular.ToString())
                {
                    eAnnuityType = ANNUITY_TYPES.regular;
                }
            }
            return eAnnuityType;
        }
        //properties
        public string Name2 { get; set; }
        public bool IsDiscounted { get; set; }
        public bool IsCommonReference { get; set; }
        public int GrowthPeriods { get; set; }
        public int GrowthTypeId { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public double OverheadFactor { get; set; }
        public double IncentiveRate { get; set; }
        public double IncentiveAmount { get; set; }
        public DateTime LastPeriodDate { get; set; }
        public List<Outcome> Outcomes { get; set; }
        public List<Calculator1> Calculators { get; set; }
        //annuities are kept in stateful collections
        //so that they can be added to subsequent time periods
        //i.e. a preproduction annuity calculated in year 3, gets added to year 4, 5, ...
        public TimePeriod.ANNUITY_TYPES AnnuityType { get; set; }
        public List<OperationComponent> OperationComponents { get; set; }
        
        //linked calculators/analyzers
        public XElement XmlDocElement { get; set; }
        public void Init()
        {
            this.InitSharedObjectProperties();
            //init anything that will cause an exception when called
            this.Outcomes = new List<Outcome>();
            this.OperationComponents = new List<OperationComponent>();
            this.Calculators = new List<Calculator1>();
            this.Name2 = string.Empty;
            this.Amount = 0;
            this.IsDiscounted = false;
            this.IsCommonReference = false;
            this.GrowthPeriods = 0;
            this.GrowthTypeId = 0;
            this.OverheadFactor = 0;
            this.IncentiveAmount = 0;
            this.IncentiveRate = 0;
            this.LastPeriodDate = CalculatorHelpers.GetDateShortNow();
            this.AnnuityType = ANNUITY_TYPES.none;
        }
        //set the class properties using the XElement
        public void SetTimePeriodProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes in base element
            this.SetCalculatorProperties(currentCalculationsElement);
            this.SetSharedObjectProperties(currentElement);
            this.SetTotalBenefitsProperties(currentElement);
            this.SetTotalCostsProperties(currentElement);
            if (currentElement.Name.LocalName.StartsWith(
                DataAppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                this.Amount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Economics1.BUDGET_TPENTERPRISE_AMOUNT);
            }
            else
            {
                this.Amount = CalculatorHelpers.GetAttributeDouble(currentElement,
                    DataAppHelpers.Economics1.INVEST_TPENTERPRISE_AMOUNT);
            }
            this.Unit = CalculatorHelpers.GetAttribute(currentElement,
                DataAppHelpers.General.UNIT);
            this.IsDiscounted = CalculatorHelpers.GetAttributeBool(currentElement,
               DataAppHelpers.Economics1.ISDISCOUNTED);
            this.IsCommonReference = CalculatorHelpers.GetAttributeBool(currentElement,
                DataAppHelpers.Economics1.ISCOMMON_REFERENCE);
            this.GrowthPeriods = CalculatorHelpers.GetAttributeInt(currentElement,
                DataAppHelpers.Economics1.GROWTH_PERIODS);
            this.GrowthTypeId = CalculatorHelpers.GetAttributeInt(currentElement,
                DataAppHelpers.Economics1.GROWTH_TYPE_ID);
            this.OverheadFactor = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.Economics1.OVERHEAD_FACTOR);
            this.IncentiveAmount = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.General.INCENTIVE_AMOUNT);
            this.IncentiveRate = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.General.INCENTIVE_RATE);
            this.AnnuityType = GetAnnuityType(currentElement);
            this.XmlDocElement = currentCalculationsElement;
        }

        //copy constructor
        public void CopyTimePeriod(TimePeriod timePeriod)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes
            this.CopyCalculatorProperties(timePeriod);
            this.CopySharedObjectProperties(timePeriod);
            this.Amount = timePeriod.Amount;
            this.Unit = timePeriod.Unit;
            this.IsDiscounted = timePeriod.IsDiscounted;
            this.IsCommonReference = timePeriod.IsCommonReference;
            this.GrowthPeriods = timePeriod.GrowthPeriods;
            this.GrowthTypeId = timePeriod.GrowthTypeId;
            this.OverheadFactor = timePeriod.OverheadFactor;
            this.IncentiveAmount = timePeriod.IncentiveAmount;
            this.IncentiveRate = timePeriod.IncentiveRate;
            this.LastPeriodDate = timePeriod.LastPeriodDate;
            this.Type = timePeriod.Type;
            this.AnnuityType = timePeriod.AnnuityType;
            //better to set in base
            this.CopyTotalBenefitsProperties(timePeriod);
            this.CopyTotalCostsProperties(timePeriod);
            //calculators are always app-specific and must be copied subsequently
            this.Calculators = new List<Calculator1>();
            this.ErrorMessage = timePeriod.ErrorMessage;
            if (timePeriod.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(timePeriod.XmlDocElement);
            }
        }
        //copy constructors
        public TimePeriod(TimePeriod timePeriod)
        {
            this.CopyTimePeriod(timePeriod);
        }
        public TimePeriod(CalculatorParameters calcParameters, TimePeriod timePeriod)
        {
            //calcParams same pattern as other constructors
            this.CopyTimePeriod(timePeriod);
        }

        //set the XElement parameter's attributes using this class
        //note: the only base element that includes calculator in args
        public void SetTimePeriodAttributes(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //version 1.4.5 requires setting calculator atts separately (so specific calc can be used)
            //serialize the current element 
            SetSharedObjectAttributes(string.Empty, ref currentElement);
            this.SetTotalBenefitsAttributes(string.Empty, ref currentElement);
            this.SetTotalCostsAttributes(string.Empty, ref currentElement);
            if (currentElement.Name.LocalName.StartsWith(
                DataAppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.BUDGET_TPENTERPRISE_AMOUNT,
                    this.Amount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
            }
            else
            {
                DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                    calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.INVEST_TPENTERPRISE_AMOUNT,
                    this.Amount.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                    calcParameters.StepNumber, ref currentElement, updates);
            }
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, Calculator1.cDate,
                this.Date.ToString("s"), RuleHelpers.GeneralRules.DATE,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.General.UNIT,
                this.Unit, RuleHelpers.GeneralRules.STRING,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.ISDISCOUNTED,
                this.IsDiscounted.ToString(), RuleHelpers.GeneralRules.BOOLEAN,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.ISCOMMON_REFERENCE,
                this.IsCommonReference.ToString(), RuleHelpers.GeneralRules.BOOLEAN,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.GROWTH_PERIODS,
                this.GrowthPeriods.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.GROWTH_TYPE_ID,
                this.GrowthTypeId.ToString(), RuleHelpers.GeneralRules.INTEGER,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Economics1.OVERHEAD_FACTOR,
                this.OverheadFactor.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_AMOUNT,
                this.IncentiveAmount.ToString("f3"), RuleHelpers.GeneralRules.DECIMAL,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.General.INCENTIVE_RATE,
                this.IncentiveRate.ToString("f3"), RuleHelpers.GeneralRules.FLOAT,
                calcParameters.StepNumber, ref currentElement, updates);
            //any output annuities on hand are serialized using:
            AddOutcomesToTimePeriod(calcParameters,
                this.Outcomes, currentCalculationsElement, ref currentElement);
            //any opcomp annuities on hand are serialized using:
            AddOpCompsToTimePeriod(calcParameters,
                this.OperationComponents, currentCalculationsElement, ref currentElement);
            //xmldoc atts are handled in the calculator helpers
        }
        
        //growth periods and other new tps that don't affect db 
        public void SetNewTimePeriodAttributes(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            CalculatorHelpers.SetAttribute(currentElement,
               DataAppHelpers.General.NAME2, this.Name2);
            if (currentElement.Name.LocalName.StartsWith(
                DataAppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                    DataAppHelpers.Economics1.BUDGET_TPENTERPRISE_AMOUNT, this.Amount);
            }
            else
            {
                CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                    DataAppHelpers.Economics1.INVEST_TPENTERPRISE_AMOUNT, this.Amount);
            }
            CalculatorHelpers.SetAttributeDateS(currentElement,
               Calculator1.cDate, this.Date);
            CalculatorHelpers.SetAttribute(currentElement,
               DataAppHelpers.General.UNIT, this.Unit);
            CalculatorHelpers.SetAttributeBool(currentElement,
               DataAppHelpers.Economics1.ISDISCOUNTED, this.IsDiscounted);
            CalculatorHelpers.SetAttributeBool(currentElement,
               DataAppHelpers.Economics1.ISCOMMON_REFERENCE, this.IsCommonReference);
            CalculatorHelpers.SetAttributeInt(currentElement,
              DataAppHelpers.Economics1.GROWTH_PERIODS, this.GrowthPeriods);
            CalculatorHelpers.SetAttributeInt(currentElement,
               DataAppHelpers.Economics1.GROWTH_TYPE_ID, this.GrowthTypeId);
            CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                DataAppHelpers.Economics1.OVERHEAD_FACTOR, this.OverheadFactor);
            CalculatorHelpers.SetAttributeDoubleF4(currentElement,
                DataAppHelpers.General.INCENTIVE_AMOUNT, this.IncentiveAmount);
            CalculatorHelpers.SetAttributeDoubleF4(currentElement,
               DataAppHelpers.General.INCENTIVE_RATE, this.IncentiveRate);
            CalculatorHelpers.SetAttribute(currentElement,
               DataAppHelpers.Economics1.ANNUITY_TYPE, this.AnnuityType.ToString());
            //any outcome annuities on hand are serialized using:
            AddOutcomesToTimePeriod(calcParameters,
                this.Outcomes, currentCalculationsElement, ref currentElement);
            //any opcomp annuities on hand are serialized using:
            AddOpCompsToTimePeriod(calcParameters,
                this.OperationComponents, currentCalculationsElement, ref currentElement);
        }
        public void SetNewTPBudgetAttributes(
            string attNameExt, ref XmlWriter writer)
        {
            //object only
            this.SetSharedObjectAttributes(attNameExt, ref writer);
            //could be either one
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.BUDGET_TPENTERPRISE_AMOUNT, attNameExt), this.Amount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.BUDGET_TPENTERPRISE_NAME, attNameExt), this.Name2);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.UNIT, attNameExt), this.Unit);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ISDISCOUNTED, attNameExt), this.IsDiscounted.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ISCOMMON_REFERENCE, attNameExt), this.IsCommonReference.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.GROWTH_PERIODS, attNameExt), this.GrowthPeriods.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.GROWTH_TYPE_ID, attNameExt), this.GrowthTypeId.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.OVERHEAD_FACTOR, attNameExt), this.OverheadFactor.ToString("F3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_AMOUNT, attNameExt), this.IncentiveAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_RATE, attNameExt), this.IncentiveRate.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ANNUITY_TYPE, attNameExt), this.AnnuityType.ToString());
        }
        public void SetNewTPInvestmentAttributes(
            string attNameExt, ref XmlWriter writer)
        {
            //object only
            this.SetSharedObjectAttributes(attNameExt, ref writer);
            //could be either one
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.INVEST_TPENTERPRISE_AMOUNT, attNameExt), this.Amount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.INVEST_TPENTERPRISE_NAME, attNameExt), this.Name2);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.UNIT, attNameExt), this.Unit);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ISDISCOUNTED, attNameExt), this.IsDiscounted.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ISCOMMON_REFERENCE, attNameExt), this.IsCommonReference.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.GROWTH_PERIODS, attNameExt), this.GrowthPeriods.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.GROWTH_TYPE_ID, attNameExt), this.GrowthTypeId.ToString());
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.OVERHEAD_FACTOR, attNameExt), this.OverheadFactor.ToString("F3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_AMOUNT, attNameExt), this.IncentiveAmount.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.General.INCENTIVE_RATE, attNameExt), this.IncentiveRate.ToString("f3"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Economics1.ANNUITY_TYPE, attNameExt), this.AnnuityType.ToString());
        }
        public static void AddOutcomesToTimePeriod(
            CalculatorParameters calcParameters, List<Outcome> outcomes,
            XElement currentCalculator, ref XElement currentElement)
        {
            if (outcomes != null)
            {
                //no db atts for annuities
                IDictionary<string, string> updates = null;
                if (calcParameters.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    foreach (Outcome outcome in outcomes)
                    {
                        XElement outcomeEl =
                            new XElement(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString());
                        //this must come after setcalculator attributes or id = 0
                        outcome.SetSharedObjectAttributes(string.Empty, ref outcomeEl);
                        outcome.SetNewOutcomeAttributes(ref outcomeEl);
                        outcome.SetTotalBenefitsAttributes(string.Empty, ref outcomeEl);
                        //serialize and add children outputs to outcomeEl
                        SerializeChildrenOutputs(outcome, calcParameters,
                            currentCalculator, ref outcomeEl, updates);
                        if (currentElement.Elements(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString()).Any() == false)
                        {
                            currentElement.Add(new XElement(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString()));
                        }
                        currentElement.Element(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString()).Add(outcomeEl);
                    }
                }
                else if (calcParameters.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    foreach (Outcome outcome in outcomes)
                    {
                        XElement outcomeEl =
                            new XElement(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString());
                        outcome.SetSharedObjectAttributes(string.Empty, ref outcomeEl);
                        outcome.SetNewOutcomeAttributes(ref outcomeEl);
                        outcome.SetTotalBenefitsAttributes(string.Empty, ref outcomeEl);
                        //serialize and add children outputs to outcomeEl
                        SerializeChildrenOutputs(outcome, calcParameters,
                            currentCalculator, ref outcomeEl, updates);
                        if (currentElement.Elements(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString()).Any() == false)
                        {
                            currentElement.Add(new XElement(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString()));
                        }
                        currentElement.Element(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcomes.ToString()).Add(outcomeEl);
                    }
                }
            }
        }
        private static void SerializeChildrenOutputs(Outcome outcome,
            CalculatorParameters calcParameters, XElement currentCalculator,
            ref XElement outcomeEl, IDictionary<string, string> updates)
        {
            //serialize and add childrent inputs to operationEl
            if (outcome.Outputs != null)
            {
                foreach (Output output in outcome.Outputs)
                {
                    XElement outputEl = null;
                    if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        outputEl
                        = new XElement(BudgetInvestment.BUDGET_TYPES.budgetoutput.ToString());
                    }
                    else if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        outputEl
                        = new XElement(BudgetInvestment.INVESTMENT_TYPES.investmentoutput.ToString());
                    }
                    output.SetSharedObjectAttributes(string.Empty, ref outputEl);
                    output.SetNewOutputAttributes(calcParameters, ref outputEl);
                    output.SetTotalBenefitsAttributes(string.Empty, ref outputEl);
                    outcomeEl.Add(outputEl);
                }
            }
        }
        
        public static void AddOpCompsToTimePeriod(
            CalculatorParameters calcParameters, List<OperationComponent> opComps, 
            XElement currentCalculator, ref XElement currentElement)        
        {
            if (opComps != null)
            {
                //no db atts for annuities
                IDictionary<string, string> updates = null;
                if (calcParameters.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    foreach (OperationComponent operation in opComps)
                    {
                        XElement operationEl =
                            new XElement(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString());
                        operation.SetSharedObjectAttributes(string.Empty, ref operationEl);
                        operation.SetNewOperationComponentAttributes(ref operationEl);
                        operation.SetTotalCostsAttributes(string.Empty, ref operationEl);
                        //serialize and add children inputs to operationEl
                        SerializeChildrenInputs(operation, calcParameters,
                            currentCalculator, ref operationEl, updates);
                        if (currentElement.Elements(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString()).Any() == false)
                        {
                            currentElement.Add(new XElement(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString()));
                        }
                        currentElement.Element(DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString()).Add(operationEl);
                    }
                }
                else if (calcParameters.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    foreach (OperationComponent component in opComps)
                    {
                        XElement componentEl =
                            new XElement(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString());
                        component.SetSharedObjectAttributes(string.Empty, ref componentEl);
                        component.SetNewOperationComponentAttributes(ref componentEl);
                        component.SetTotalCostsAttributes(string.Empty, ref componentEl);
                        //serialize and add children inputs to operationEl
                        SerializeChildrenInputs(component, calcParameters,
                            currentCalculator, ref componentEl, updates);
                        if (currentElement.Elements(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString()).Any() == false)
                        {
                            currentElement.Add(new XElement(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString()));
                        }
                        currentElement.Element(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString()).Add(componentEl);
                    }
                }
            }
        }
        private static void SerializeChildrenInputs(OperationComponent opComp,
            CalculatorParameters calcParameters, XElement currentCalculator,
            ref XElement opCompEl, IDictionary<string, string> updates)
        {
            //serialize and add children inputs to operationEl
            if (opComp.Inputs != null)
            {
                foreach (Input input in opComp.Inputs)
                {
                    XElement inputEl = null;
                    if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets)
                    {
                        inputEl
                        = new XElement(BudgetInvestment.BUDGET_TYPES.budgetinput.ToString());
                    }
                    else if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
                    {
                        inputEl
                        = new XElement(BudgetInvestment.INVESTMENT_TYPES.investmentinput.ToString());
                    }
                    input.SetSharedObjectAttributes(string.Empty, ref inputEl);
                    input.SetNewInputAttributes(calcParameters, ref inputEl);
                    input.SetTotalCostsAttributes(string.Empty, ref inputEl);
                    opCompEl.Add(inputEl);
                }
            }
        }
        public static List<OperationComponent> CopyOperationComponent(
            CalculatorParameters calcParameters, List<OperationComponent> opComps)
        {
            List<OperationComponent> copyOpComps = new List<OperationComponent>();
            if (opComps != null)
            {
                foreach (OperationComponent copyService in opComps)
                {
                    copyOpComps.Add(new OperationComponent(calcParameters, copyService));
                }
            }
            return copyOpComps;
        }
        public static bool IsAnnuity(XElement currentElement)
        {
            bool bIsAnnuity = false;
            ANNUITY_TYPES eAnnuityType = GetAnnuityType(currentElement);
            string sAnnuityLabel = CalculatorHelpers.GetAttribute(currentElement, 
                Calculator1.cLabel);
            if (eAnnuityType != ANNUITY_TYPES.none
                || sAnnuityLabel == DataAppHelpers.Economics1.ANNUITY)
            {
                bIsAnnuity = true;
            }
            return bIsAnnuity;
        }
        public static bool IsAnnuity(TimePeriod tp)
        {
            bool bIsAnnuity = false;
            if (tp.AnnuityType != ANNUITY_TYPES.none
                || tp.Label == DataAppHelpers.Economics1.ANNUITY)
            {
                bIsAnnuity = true;
            }
            return bIsAnnuity;
        }
        
    }
}
