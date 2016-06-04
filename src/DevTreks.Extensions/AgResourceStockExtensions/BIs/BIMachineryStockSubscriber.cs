using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		This class derives from the CalculatorContracts.BudgetInvestmentCalculator 
    ///             class. It subscribes to the RunCalculation event
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2014, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class BIMachineryStockSubscriber : BudgetInvestmentCalculator
    {
        //constructors
        //constructor sets class (base) properties
        public BIMachineryStockSubscriber(CalculatorParameters calcParameters)
            : base(calcParameters) 
        {
            //this calls the constructors which also inherit from bicalculator for base params
            if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                {
                    this.BIM2Calculator = new BIMachinery2StockCalculator(calcParameters);
                }
                else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    this.BIM2aCalculator = new BIMachinery2aStockCalculator(calcParameters);
                }
                else
                {
                    this.BIM1Calculator = new BIMachineryStockCalculator(calcParameters);
                }
            }
            else if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
            {
                this.BIIP1Calculator = new BIIrrPowerStockCalculator(calcParameters);
            }
            else if (calcParameters.RelatedCalculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
            {
                this.BIGC1Calculator = new BIGeneralCapitalStockCalculator(calcParameters);
            }
        }

        //properties
        //stateful calculator being run by this extension
        public BIMachineryStockCalculator BIM1Calculator { get; set; }
        public BIMachinery2StockCalculator BIM2Calculator { get; set; }
        public BIMachinery2aStockCalculator BIM2aCalculator { get; set; }
        public BIIrrPowerStockCalculator BIIP1Calculator { get; set; }
        public BIGeneralCapitalStockCalculator BIGC1Calculator { get; set; }
        
        //methods
        //subscribe to the events raised by the base class
        public bool RunCalculator()
        {
            bool bHasAnalysis = false;
            //subscribe to the events raised by the publisher delegate
            this.SetAncestorObjects += AddAncestorObjects;
            this.RunCalculation += AddCalculations;
            //run the calculation (raising the events
            //from the base event publisher for each node)
            bHasAnalysis = this.StreamAndSaveCalculation();
            return bHasAnalysis;
        }
        //define the actions to take when the event is raised
        public void AddAncestorObjects(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement statElement = new XElement(e.CurrentElement);
            //run the stats and add them to statelement
            AddAncestorObjects(statElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            e.CurrentElement = new XElement(statElement);
        }
        //define the actions to take when the event is raised
        public void AddCalculations(object sender, CustomEventArgs e)
        {
            //pass a byref xelement from the publisher's data
            XElement statElement = null;
            XElement linkedViewElement = null;
            if (e.CurrentElement != null)
                statElement = new XElement(e.CurrentElement);
            if (e.LinkedViewElement != null)
                linkedViewElement = new XElement(e.LinkedViewElement);
            //run the stats and add them to statelement
            e.HasCalculations = RunBudgetInvestmentCalculation(
                ref statElement, ref linkedViewElement);
            if (e.HasCalculations)
            {
                //pass the new statelement back to the publisher
                //by setting the CalculatedElement property of CustomEventArgs
                if (statElement != null)
                    e.CurrentElement = new XElement(statElement);
                if (linkedViewElement != null)
                    e.LinkedViewElement = new XElement(linkedViewElement);
            }
        }
        
        private bool RunBudgetInvestmentCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            CalculatorHelpers.CALCULATOR_TYPES eCalculatorType
                = CalculatorHelpers.GetCalculatorType(this.GCCalculatorParams.CalculatorType);
            switch (eCalculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.budget:
                    if (this.GCCalculatorParams.RelatedCalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                    {
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                        {
                            bHasCalculations
                                = BIM2Calculator.AddStock2CalculationsToCurrentElement(
                                    ref currentCalculationsElement, ref currentElement);
                        }
                        else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                        {
                            bHasCalculations
                                = BIM2aCalculator.AddStock2aCalculationsToCurrentElement(
                                    ref currentCalculationsElement, ref currentElement);
                        }
                        else
                        {
                            bHasCalculations
                                = BIM1Calculator.AddStock1CalculationsToCurrentElement(
                                    ref currentCalculationsElement, ref currentElement);
                        }
                    }
                    else if (this.GCCalculatorParams.RelatedCalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                    {
                        bHasCalculations
                            = BIIP1Calculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    else if (this.GCCalculatorParams.RelatedCalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                    {
                        bHasCalculations
                            = BIGC1Calculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.investment:
                    if (this.GCCalculatorParams.RelatedCalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                    {
                        if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString())
                        {
                            bHasCalculations
                                = BIM2Calculator.AddStock2CalculationsToCurrentElement(
                                    ref currentCalculationsElement, ref currentElement);
                        }
                        else if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                            == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                        {
                            bHasCalculations
                                = BIM2aCalculator.AddStock2aCalculationsToCurrentElement(
                                    ref currentCalculationsElement, ref currentElement);
                        }
                        else
                        {
                            bHasCalculations
                                = BIM1Calculator.AddStock1CalculationsToCurrentElement(
                                    ref currentCalculationsElement, ref currentElement);
                        }
                    }
                    else if (this.GCCalculatorParams.RelatedCalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                    {
                        bHasCalculations
                            = BIIP1Calculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    else if (this.GCCalculatorParams.RelatedCalculatorType
                        == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                    {
                        bHasCalculations
                            = BIGC1Calculator.AddCalculationsToCurrentElement(
                                ref currentCalculationsElement, ref currentElement);
                    }
                    break;
                default:
                    break;
            }
            bHasCalculations = TransferErrors(bHasCalculations);
            //some calcparams have to be passed back to this.calcparams
            UpdateCalculatorParams(currentElement.Name.LocalName);
            return bHasCalculations;
        }
         
        private void AddAncestorObjects(XElement currentElement)
        {
            XElement currentCalculationsElement = null;
            currentCalculationsElement
                    = CalculatorHelpers.GetCalculator(this.GCCalculatorParams, 
                    currentElement);
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //need the tp.amount multiplier
                //set the stateful ancestor object
                //last time period date's are used in some calculations
                DateTime dtLastTimePeriodDate = new DateTime(1000, 12, 1);
                bool bHasLastDate = false;
                if (this.GCCalculatorParams.ParentTimePeriod != null)
                {
                    bHasLastDate = true;
                    dtLastTimePeriodDate = this.GCCalculatorParams.ParentTimePeriod.Date;
                }
                this.GCCalculatorParams.ParentTimePeriod = new TimePeriod();
                //deserialize xml to object, just need the amount, maybe date
                this.GCCalculatorParams.ParentTimePeriod.SetTimePeriodProperties(
                    this.GCCalculatorParams, currentCalculationsElement, currentElement);
                //copy some of the last tps properties to current tp 
                if (!bHasLastDate) dtLastTimePeriodDate
                    = this.GCCalculatorParams.ParentTimePeriod.Date;
                this.GCCalculatorParams.ParentTimePeriod.LastPeriodDate
                    = dtLastTimePeriodDate;
                //all calculators need it
                if (this.BIM1Calculator != null)
                {
                    this.BIM1Calculator.GCCalculatorParams.ParentTimePeriod
                        = this.GCCalculatorParams.ParentTimePeriod;
                }
                else if (this.BIM2Calculator != null)
                {
                    this.BIM2Calculator.GCCalculatorParams.ParentTimePeriod
                        = this.GCCalculatorParams.ParentTimePeriod;
                }
                else if (this.BIM2aCalculator != null)
                {
                    this.BIM2aCalculator.GCCalculatorParams.ParentTimePeriod
                        = this.GCCalculatorParams.ParentTimePeriod;
                }
                else if (this.BIIP1Calculator != null)
                {
                    this.BIIP1Calculator.GCCalculatorParams.ParentTimePeriod
                        = this.GCCalculatorParams.ParentTimePeriod;
                }
                else if (this.BIGC1Calculator != null)
                {
                    this.BIGC1Calculator.GCCalculatorParams.ParentTimePeriod
                        = this.GCCalculatorParams.ParentTimePeriod;
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //need the oc.amount multiplier
                this.GCCalculatorParams.ParentOperationComponent
                 = new OperationComponent();
                this.GCCalculatorParams.ParentOperationComponent
                    .SetOperationComponentProperties(
                    this.GCCalculatorParams, currentCalculationsElement,
                    currentElement);
                //init the total costs to zero
                this.GCCalculatorParams.ParentOperationComponent.InitTotalCostsProperties();
                //all calculators need it
                if (this.BIM1Calculator != null)
                {
                    this.BIM1Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.BIM2Calculator != null)
                {
                    this.BIM2Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.BIM2aCalculator != null)
                {
                    this.BIM2aCalculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.BIIP1Calculator != null)
                {
                    this.BIIP1Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
                else if (this.BIGC1Calculator != null)
                {
                    this.BIGC1Calculator.GCCalculatorParams.ParentOperationComponent
                        = this.GCCalculatorParams.ParentOperationComponent;
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //need the outcome.amount multiplier
                this.GCCalculatorParams.ParentOutcome
                 = new Outcome();
                this.GCCalculatorParams.ParentOutcome
                    .SetOutcomeProperties(
                    this.GCCalculatorParams, currentCalculationsElement,
                    currentElement);
                //init the total benefits to zero
                this.GCCalculatorParams.ParentOutcome.InitTotalBenefitsProperties();
                //all calculators need it
                if (this.BIM1Calculator != null)
                {
                    this.BIM1Calculator.GCCalculatorParams.ParentOutcome
                        = this.GCCalculatorParams.ParentOutcome;
                }
                else if (this.BIM2Calculator != null)
                {
                    this.BIM2Calculator.GCCalculatorParams.ParentOutcome
                        = this.GCCalculatorParams.ParentOutcome;
                }
                else if (this.BIM2aCalculator != null)
                {
                    this.BIM2aCalculator.GCCalculatorParams.ParentOutcome
                        = this.GCCalculatorParams.ParentOutcome;
                }
                else if (this.BIIP1Calculator != null)
                {
                    this.BIIP1Calculator.GCCalculatorParams.ParentOutcome
                        = this.GCCalculatorParams.ParentOutcome;
                }
                else if (this.BIGC1Calculator != null)
                {
                    this.BIGC1Calculator.GCCalculatorParams.ParentOutcome
                        = this.GCCalculatorParams.ParentOutcome;
                }
            }
        }
        public static double GetMultiplierForTimePeriod(TimePeriod tp)
        {
            //ops, comps, budgets and investments use the times multiplier
            double multiplier = 1;
            if (tp != null)
            {
                multiplier = tp.Amount;
                if (multiplier == 0)
                {
                    multiplier = 1;
                }
            }
            return multiplier;
        }
        private bool TransferErrors(bool hasCalculations)
        {
            bool bHasGoodCalculations = hasCalculations;
            //the bicalculators have their own set of calcs and analysis params
            if (this.BIM1Calculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += BIM1Calculator.GCCalculatorParams.ErrorMessage;
            }
            if (this.BIM2Calculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.BIM2Calculator.GCCalculatorParams.ErrorMessage;
            }
            if (this.BIM2aCalculator != null)
            {
                this.GCCalculatorParams.ErrorMessage += this.BIM2aCalculator.GCCalculatorParams.ErrorMessage;
            }
            if (!string.IsNullOrEmpty(this.GCCalculatorParams.ErrorMessage))
            {
                this.GCCalculatorParams.ErrorMessage += this.GCCalculatorParams.ErrorMessage;
                bHasGoodCalculations = false;
            }
            return bHasGoodCalculations;
        }
        private void UpdateCalculatorParams(string currentNodeName)
        {
            //pass back NeedsXmlDocOnly in calcparams
            //note that 0.8.8 did not end up using Use Same Calculator Pack in Children
            //for resources 02 or resources 02a analyses
            //but keep this pattern for future use
            if (this.GCCalculatorParams.NeedsCalculators
                && (this.GCCalculatorParams.StartingDocToCalcNodeName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || this.GCCalculatorParams.StartingDocToCalcNodeName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString()))
            {
                if (this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString()
                    || this.GCCalculatorParams.AnalyzerParms.AnalyzerType
                    == ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString())
                {
                    if (currentNodeName
                        == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                        || currentNodeName
                        == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                    {
                        if (this.BIM2Calculator != null)
                        {
                            //unless NeedsXmlDocOnly = false
                            //won't be able to insert a new calculator in children 
                            //(note that resource02 replace base npv budget calcor
                            //and resources2a children replace the base resources 01 calcor 
                            //passed into them because they find relatedcalctype=agmachinery)
                            //this.GCCalculatorParams.NeedsXmlDocOnly
                            //    = this.BIM2Calculator.GCCalculatorParams.NeedsXmlDocOnly;
                        }
                    }
                }
            }
        }
        
    }
}
