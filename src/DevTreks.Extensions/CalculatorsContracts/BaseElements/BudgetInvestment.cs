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
    ///Purpose:		The budget/investment class is used by most 
    ///             budget and investment calculators.
    ///Author:		www.devtreks.org
    ///Date:		2013, November
    ///</summary>
    public class BudgetInvestment : CostBenefitCalculator
    {
        public BudgetInvestment() 
        {
            Init();
        }
        public const string BUDGETSYSTEM_ID = DataAppHelpers.Economics1.BUDGETSYSTEM_ID;
        public const string INVESTMENTSYSTEM_ID = DataAppHelpers.Economics1.INVESTMENTSYSTEM_ID;
        public const string BUDGETSYSTEM_TO_OUTCOME_ID = DataAppHelpers.Economics1.BUDGETSYSTEM_TO_OUTCOME_ID;
        public const string COSTSYSTEM_TO_OUTCOME_ID = DataAppHelpers.Economics1.COSTSYSTEM_TO_OUTCOME_ID;
        public const string BUDGETSYSTEM_TO_OPERATION_ID = DataAppHelpers.Economics1.BUDGETSYSTEM_TO_OPERATION_ID;
        public const string COSTSYSTEM_TO_COMPONENT_ID = DataAppHelpers.Economics1.COSTSYSTEM_TO_COMPONENT_ID;
        public const string BUDGETSYSTEM_TO_TIME_ID = DataAppHelpers.Economics1.BUDGETSYSTEM_TO_TIME_ID;
        public const string COSTSYSTEM_TO_TIME_ID = DataAppHelpers.Economics1.COSTSYSTEM_TO_TIME_ID;

        public const string ISDISCOUNTED = DataAppHelpers.Economics1.ISDISCOUNTED;
        public const string ISCOMMON_REFERENCE = DataAppHelpers.Economics1.ISCOMMON_REFERENCE;
        public const string GROWTH_PERIODS = DataAppHelpers.Economics1.GROWTH_PERIODS;
        public const string GROWTH_TYPE_ID = DataAppHelpers.Economics1.GROWTH_TYPE_ID;
        public const string OVERHEAD_FACTOR = DataAppHelpers.Economics1.OVERHEAD_FACTOR;
        public const string ANNUITY_TYPE = DataAppHelpers.Economics1.ANNUITY_TYPE;
        public const string ANNUITY = DataAppHelpers.Economics1.ANNUITY;
        //string constants
        private const string INVESTMENTEAA = "InvestmentEAA";
        //derived constants
        /// <summary>
        /// Type of budget node being used.
        /// </summary>
        public enum BUDGET_TYPES
        {
            budgettype          = 0,
            budgetgroup         = 1,
            budget              = 2,
            budgettimeperiod    = 3,
            budgetoutcomes      = 4,
            budgetoutcome       = 5,
            budgetoutput        = 6,
            budgetoperations    = 7,
            budgetoperation     = 8,
            budgetinput         = 9
        }
        /// <summary>
        /// Type of investment node being used.
        /// </summary>
        public enum INVESTMENT_TYPES
        {
            investmenttype          = 0,
            investmentgroup         = 1,
            investment              = 2,
            investmenttimeperiod    = 3,
            investmentoutcomes      = 4,
            investmentoutcome       = 5,
            investmentoutput        = 6,
            investmentcomponents    = 7,
            investmentcomponent     = 8,
            investmentinput         = 9
        }
        //properties
        public double InitialValue { get; set; }
        public double SalvageValue { get; set; }
        //time variables needed in calculations
        public DateTime CommonReferencePointDate { get; set; }
        public int PreProdPeriods { get; set; }
        public int ProdPeriods { get; set; }
        //preproduction annuities span all timeperiods
        public DateTime InitEOPDate { get; set; }
        //equivalent annual investment
        public double InvestmentEAA { get; set; }
        public Local Local { get; set; }
        public List<TimePeriod> TimePeriods { get; set; }
        //regular annuities and preproduction annuities that will get discounted
        //and added to each descendent time period
        public List<OperationComponent> AnnEquivalents { get; set; }
        //linked calculators/analyzers
        public List<Calculator1> Calculators { get; set; }
        public XElement XmlDocElement { get; set; }
        public void Init()
        {
            this.InitSharedObjectProperties();
            this.InitialValue = 0;
            this.SalvageValue = 0;
            this.CommonReferencePointDate = CalculatorHelpers.GetDateShortNow();
            this.PreProdPeriods = 0;
            this.ProdPeriods = 0;
            this.InitEOPDate = CalculatorHelpers.GetDateShortNow();
            this.InvestmentEAA = 0;
            //init anything that will cause an exception when called
            this.Local = new Local();
            this.TimePeriods = new List<TimePeriod>();
            this.AnnEquivalents = new List<OperationComponent>();
            this.Calculators = new List<Calculator1>();
        }
        //set the class properties using the XElement
        public void SetBudgetInvestmentProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes in base element
            this.SetCalculatorProperties(currentCalculationsElement);
            this.SetSharedObjectProperties(currentElement);
            this.SetTotalBenefitsProperties(currentElement);
            this.SetTotalCostsProperties(currentElement);
            //this.Id = CalculatorHelpers.GetAttributeInt(currentElement, Calculator1.cId);
            this.InitialValue = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.Prices.INITIAL_VALUE);
            this.SalvageValue = CalculatorHelpers.GetAttributeDouble(currentElement,
                DataAppHelpers.Prices.SALVAGE_VALUE);
            this.InvestmentEAA = CalculatorHelpers.GetAttributeDouble(currentElement,
                INVESTMENTEAA);
            this.Local = new Local();
            //can't use calcParams.CurrentElementURIPattern because also gets set from ancestors
            string sCurrentNodeURIPattern
               = CalculatorHelpers.MakeNewURIPatternFromElement(
               calcParameters.ExtensionDocToCalcURI.URIPattern, currentElement);
            this.Local = CalculatorHelpers.GetLocal(sCurrentNodeURIPattern, calcParameters, 
                currentCalculationsElement, currentElement);
            this.XmlDocElement = currentCalculationsElement;
        }

        //copy constructor
        public BudgetInvestment(CalculatorParameters calcParameters, 
            BudgetInvestment budgetOrInvestment)
        {
            //several extensions store some calculator props in base element (observations, targettype)
            //no harm done in setting them but never set their attributes
            this.CopyCalculatorProperties(budgetOrInvestment);
            this.CopySharedObjectProperties(budgetOrInvestment);
            this.InitialValue = budgetOrInvestment.InitialValue;
            this.SalvageValue = budgetOrInvestment.SalvageValue;
            this.InvestmentEAA = budgetOrInvestment.InvestmentEAA;
            this.Type = budgetOrInvestment.Type;
            //calculators are always app-specific and must be copied subsequently
            this.Calculators = new List<Calculator1>();
            //better to set in base
            this.CopyTotalBenefitsProperties(budgetOrInvestment);
            this.CopyTotalCostsProperties(budgetOrInvestment);
            this.ErrorMessage = budgetOrInvestment.ErrorMessage;
            if (budgetOrInvestment.Local == null)
                budgetOrInvestment.Local = new Local();
            this.Local = new Local(calcParameters, budgetOrInvestment.Local);
            if (budgetOrInvestment.XmlDocElement != null)
            {
                this.XmlDocElement = new XElement(budgetOrInvestment.XmlDocElement);
            }
        }

        //set the XElement parameter's attributes using this class
        public void SetBudgetInvestmentAttributes(CalculatorParameters calcParameters,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            //version 1.4.5 requires setting calculator atts separately (so specific calc can be used)
            //serialize the current element 
            this.SetSharedObjectAttributes(string.Empty, ref currentElement);
            this.SetTotalBenefitsAttributes(string.Empty, ref currentElement);
            this.SetTotalCostsAttributes(string.Empty, ref currentElement);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.INITIAL_VALUE,
                this.InitialValue.ToString("f2"), RuleHelpers.GeneralRules.DECIMAL,
                calcParameters.StepNumber, ref currentElement, updates);
            DataHelpers.AddInHelperLinq.UpdateNewValue(calcParameters.AttributeNeedsDbUpdate,
                calcParameters.CurrentElementURIPattern, DataAppHelpers.Prices.SALVAGE_VALUE,
                this.SalvageValue.ToString("f2"), RuleHelpers.GeneralRules.DECIMAL,
                calcParameters.StepNumber, ref currentElement, updates);
            CalculatorHelpers.SetAttributeDoubleF2(currentElement,
                INVESTMENTEAA, this.InvestmentEAA);
            if (this.Local == null)
                this.Local = new Local();
            this.Local.SetLocalAttributes(calcParameters,
                ref currentElement, updates);
            //xmldoc atts are handled in the calculator helpers
            //any annuities on hand are serialized using 
            AddGrowthPeriodsToBudgetInvestment(calcParameters,
                calcParameters.ParentBudgetInvestment.TimePeriods,
                ref currentElement);
        }
        //serialize regular properties
        public void SetNewBudgetInvestmentAttributes(
            ref XElement elementNeedingAttributes)
        {
            CalculatorHelpers.SetAttributeDoubleF4(elementNeedingAttributes,
                DataAppHelpers.Prices.INITIAL_VALUE, this.InitialValue);
            CalculatorHelpers.SetAttributeDoubleF3(elementNeedingAttributes,
                DataAppHelpers.Prices.SALVAGE_VALUE, this.SalvageValue);
        }
        public void SetNewBIAttributes(
            string attNameExt, ref XmlWriter writer)
        {
            //object only
            this.SetSharedObjectAttributes(attNameExt, ref writer);
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.INITIAL_VALUE, attNameExt), this.InitialValue.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(DataAppHelpers.Prices.SALVAGE_VALUE, attNameExt), this.SalvageValue.ToString("f2"));
            writer.WriteAttributeString(
                  string.Concat(INVESTMENTEAA, attNameExt), this.InvestmentEAA.ToString("f2"));
        }
        public static void AddGrowthPeriodsToBudgetInvestment(
            CalculatorParameters calcParameters, List<TimePeriod> tps,
            ref XElement currentElement)
        {
            if (tps != null)
            {
                //no db atts for annuities
                IDictionary<string, string> updates = null;
                //no need for currentCalculationsElement
                XElement currentCalculator = new XElement(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                int iIndex = 0;
                bool bHasAddedAnnuity = false;
                if (calcParameters.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.budgets)
                {
                    foreach (TimePeriod tp in tps)
                    {
                        if (tp.Unit == TimePeriod.GROWTH_SERIES_TYPES.geometric.ToString()
                            || tp.Unit == TimePeriod.GROWTH_SERIES_TYPES.linear.ToString()
                            || tp.Unit == TimePeriod.GROWTH_SERIES_TYPES.uniform.ToString())
                        {
                            XElement tpEl =
                                new XElement(DataAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString());
                            //id and name must be first
                            tp.SetSharedObjectAttributes(string.Empty, ref tpEl);
                            //serialize and add children outputs and opcomps to tpEl
                            tp.SetNewTimePeriodAttributes(calcParameters,
                                ref currentCalculator, ref tpEl, updates);
                            tp.SetTotalBenefitsAttributes(string.Empty, ref tpEl);
                            tp.SetTotalCostsAttributes(string.Empty, ref tpEl);
                            currentElement.Add(tpEl);
                            bHasAddedAnnuity = true;
                        }
                        else
                        {
                            iIndex += 1;
                        }
                    }
                }
                else if (calcParameters.SubApplicationType
                    == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    foreach (TimePeriod tp in tps)
                    {
                        if (tp.Unit == TimePeriod.GROWTH_SERIES_TYPES.geometric.ToString()
                            || tp.Unit == TimePeriod.GROWTH_SERIES_TYPES.linear.ToString()
                            || tp.Unit == TimePeriod.GROWTH_SERIES_TYPES.uniform.ToString())
                        {
                            XElement tpEl =
                                new XElement(DataAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString());
                            //id and name must be first
                            tp.SetSharedObjectAttributes(string.Empty, ref tpEl);
                            tp.SetNewTimePeriodAttributes(calcParameters,
                                ref currentCalculator, ref tpEl, updates);
                            tp.SetTotalBenefitsAttributes(string.Empty, ref tpEl);
                            tp.SetTotalCostsAttributes(string.Empty, ref tpEl);
                            currentElement.Add(tpEl);
                            bHasAddedAnnuity = true;
                        }
                        else
                        {
                            iIndex += 1;
                        }
                    }
                }
                if (bHasAddedAnnuity)
                {
                    //done with it, remove it
                    calcParameters.ParentBudgetInvestment.TimePeriods
                        .RemoveAt(iIndex);
                }
            }
        }
        public static bool AddTimePeriodTotalsToTimePeriodsCollection(
            CalculatorParameters calcParameters, TimePeriod tp)
        {
            bool bHasAdded = false;
            if (calcParameters.ParentBudgetInvestment.TimePeriods != null)
            {
                if (calcParameters.ParentBudgetInvestment.TimePeriods.Count > 0)
                {
                    bool bHasTimePeriod
                        = calcParameters.ParentBudgetInvestment.TimePeriods.
                            Any(c => c.Id == tp.Id);
                    if (bHasTimePeriod)
                    {
                        int iIndex = GetTimePeriodIndex(calcParameters, tp);
                        //remove the existing tp
                        calcParameters.ParentBudgetInvestment.TimePeriods
                            .RemoveAt(iIndex);
                        //insert the calculated tp at the same index
                        calcParameters.ParentBudgetInvestment.TimePeriods
                            .Insert(iIndex, tp);
                        bHasAdded = true;
                    }
                }
            }
            return bHasAdded;
        }
        public static TimePeriod GetLastTimePeriod(CalculatorParameters calcParameters,
            TimePeriod currentTimePeriod)
        {
            TimePeriod lastTimePeriod = null;
            if (calcParameters.ParentBudgetInvestment.TimePeriods != null)
            {
                if (calcParameters.ParentBudgetInvestment.TimePeriods.Count > 0)
                {
                    bool bHasTimePeriod
                        = calcParameters.ParentBudgetInvestment.TimePeriods.
                            Any(c => c.Id == currentTimePeriod.Id);
                    if (bHasTimePeriod)
                    {
                        int iIndex = GetTimePeriodIndex(calcParameters, currentTimePeriod);
                        //return the previous timeperiod
                        if (iIndex > 0)
                        {
                            lastTimePeriod
                                = calcParameters.ParentBudgetInvestment
                                .TimePeriods[iIndex - 1];
                        }
                    }
                }
            }
            return lastTimePeriod;
        }
        public static int GetTimePeriodIndex(CalculatorParameters calcParameters,
            TimePeriod currentTimePeriod)
        {
            int iIndex = 0;
            if (calcParameters.ParentBudgetInvestment.TimePeriods != null)
            {
                if (calcParameters.ParentBudgetInvestment.TimePeriods.Count > 0)
                {
                    bool bHasTimePeriod
                        = calcParameters.ParentBudgetInvestment.TimePeriods.
                            Any(c => c.Id == currentTimePeriod.Id);
                    if (bHasTimePeriod)
                    {
                        foreach (TimePeriod timePeriod
                            in calcParameters.ParentBudgetInvestment.TimePeriods)
                        {
                            if (timePeriod.Id == currentTimePeriod.Id)
                            {
                                break;
                            }
                            iIndex += 1;
                        }
                    }
                }
            }
            return iIndex;
        }
    }
}
