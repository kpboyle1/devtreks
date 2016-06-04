using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Extends BIMachineryStockCalculator with more timeliness stock calculations 
    ///             for operating and capital budgets.
    ///Author:		www.devtreks.org
    ///Date:		2014, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// NOTES       1. Keep MachStock and Mach2Stock collections in synch by using the 
    ///             exact same fileposition index in both -will then have both 
    ///             machinery total costs and timeliness total costs. But allow 
    ///             them to increment consistently.
    ///             Note that the Timeliness collections are used in analyzers that 
    ///             inherit from this class.
    /// </summary>
    public class BIMachinery2StockCalculator : BIMachineryStockCalculator
    {
        public BIMachinery2StockCalculator(CalculatorParameters calcParameters)
            : base(calcParameters)
        {
            //base sets the properties
        }
        private void Init()
        {
            //each member of the dictionary is a separate budget, tp, or opOrComp
            this.Machinery2Stock = new Machinery2Stock();
            this.Machinery2Stock.Machinery2Stocks
                = new Dictionary<int, List<TimelinessOpComp1>>();
        }
        //these objects extend base objects with timeliness penalty properties
        public TimelinessBIGroup1 TimelinessBudgetGroup { get; set; }
        public TimelinessBI1 TimelinessBudget { get; set; }
        public TimelinessTimePeriod1 TimelinessTimePeriod { get; set; }
        public TimelinessOpComp1 TimelinessOpComp { get; set; }

        public bool AddStock2CalculationsToCurrentElement(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //input machinery1 facts are aggregated by each ancestor
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = SetTotalMachinery2StockCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = SetBIMachinery2StockJointCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //set the tp calculations
                bHasCalculations = SetTimePeriodMachinery2StockJointCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                SetOutcome2Collection(ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                 .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                SetOutput2Collection(ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //init the stock2 calculations
                bHasCalculations = SetOpOrCompMachinery2StockJointCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                bHasCalculations = SetInputMachinery2StockJointCalculations(
                    ref currentCalculationsElement, ref currentElement);
            }
            return bHasCalculations;
        }
        public bool SetTotalMachinery2StockJointCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. groups)
            //note that 02a supplements this method with with calculations that use the Timeliness collections
            bool bHasCalculations = false;
            if (currentCalculationsElement == null)
            {
                //budget resource stock analyzers always show budget totals which are stored in
                //currentCalcElement; if null (uses set NeedsChildren to false) set it to 
                //current linkedview
                currentCalculationsElement = this.GCCalculatorParams.LinkedViewElement;
            }
            //set both stock collections
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            bHasCalculations = this.SetTotalMachineryStockCalculations(ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            bHasCalculations = SetTotalMachinery2StockCalculations(ref currentCalculationsElement, ref currentElement);
            //keep the newly set fileposition index
            return bHasCalculations;
        }
        public bool SetTotalMachinery2StockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroup)
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.TimelinessBudgetGroup == null)
            {
                this.TimelinessBudgetGroup = new TimelinessBIGroup1();
            }
            //when this method is called from opcomp or input group
            if (this.TimelinessBudget == null)
            {
                this.TimelinessBudget = new TimelinessBI1();
            }
            if (currentElement.Name.LocalName.EndsWith("group"))
            {
                this.TimelinessBudgetGroup.SetBudgetInvestmentGroupProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            }
            else
            {
                this.TimelinessBudget.SetBudgetInvestmentProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            }
            //init machStock props
            this.Machinery2Stock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            //no multiplier for budget or budget groups
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachinery2StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            this.Machinery2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //add to collection
            if (this.TimelinessBudgetGroup.TimelinessBudgets == null)
            {
                this.TimelinessBudgetGroup.TimelinessBudgets = new List<TimelinessBI1>();
            }
            this.TimelinessBudgetGroup.TimelinessBudgets.Add(this.TimelinessBudget);
            //reset the budget (opcomp) for next collection
            this.TimelinessBudget = null;
            return bHasCalculations;
        }
        public bool SetBIMachinery2StockJointCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroup)
            bool bHasCalculations = false;
            if (currentCalculationsElement == null)
            {
                //budget resource stock analyzers always show budget totals which are stored in
                //currentCalcElement; if null (uses set NeedsChildren to false) set it to 
                //current linkedview
                currentCalculationsElement = this.GCCalculatorParams.LinkedViewElement;
            }
            //Step 1. set both stock collections
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            int iBudgetIndex = this.BudgetStartingFileIndex;
            bHasCalculations = this.SetBIMachineryStockCalculations(ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            this.BudgetStartingFileIndex = iBudgetIndex;
            bHasCalculations = SetBIMachinery2StockCalculations(ref currentCalculationsElement, ref currentElement);
            //keep the newly set fileposition index
            //note that 02a supplements this method with with calculations that use the Timeliness collections
            return bHasCalculations;
        }
        public bool SetBIMachinery2StockCalculations(ref XElement currentCalculationsElement,
           ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroup)
            bool bHasCalculations = false;
            //set the parent budget group for holding collection of budgets
            if (this.TimelinessBudgetGroup == null)
            {
                this.TimelinessBudgetGroup = new TimelinessBIGroup1();
            }
            //when this method is called from opcomp or input group
            if (this.TimelinessBudget == null)
            {
                this.TimelinessBudget = new TimelinessBI1();
            }
            this.TimelinessBudget.SetBudgetInvestmentProperties(this.GCCalculatorParams,
                    currentCalculationsElement, currentElement);
            //init machStock props
            this.Machinery2Stock.SetCalculatorProperties(currentCalculationsElement);
            //the machStock.machstocks dictionary can now be summed to derive totals
            //no multiplier for budget or budget groups
            double dbMultiplier = 1;
            //set the machinery stock totals from machStock collection
            bHasCalculations = SetTotalMachinery2StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change group objects, so don't serialize them)
            string sAttNameExtension = string.Empty;
            ////set new machinery stock totals
            this.Machinery2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.Machinery2Stock.InitTotalMachinery2StockProperties();
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.TimelinessBudgetGroup.TimelinessBudgets == null)
            {
                this.TimelinessBudgetGroup.TimelinessBudgets = new List<TimelinessBI1>();
            }
            this.TimelinessBudgetGroup.TimelinessBudgets.Add(this.TimelinessBudget);
            //reset for next collection
            this.TimelinessBudget = null;
            return bHasCalculations;
        }
        public bool SetTimePeriodMachinery2StockJointCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            //miscellaneous aggregations (i.e. inputgroup, operationgroup)
            bool bHasCalculations = false;
            //standard getally calcs will retrieve resource 01 calculator
            //(because finds relatedcalculatortype=agmachinery)
            //which must not be overwritten if UseSameCalcInChildren = true
            currentCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //CalculatorHelpers.ReplaceAnalyzerCalculator(this.GCCalculatorParams, currentElement,
            //    ARSAnalyzerHelper.ANALYZER_TYPES.resources02a.ToString(),
            //    ref currentCalculationsElement);
            if (currentCalculationsElement == null)
            {
                //either a base resource01 calculation was not run
                //or it was never added to base npv calculator 
                currentCalculationsElement = new XElement(this.GCCalculatorParams.LinkedViewElement);
            }
            //Step 1. set both stock collections
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            int iTimeIndex = this.TimePeriodStartingFileIndex;
            this.SetTimePeriodMachineryStockCalculations(ref currentCalculationsElement, ref currentElement);
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            this.TimePeriodStartingFileIndex = iTimeIndex;
            bHasCalculations = SetTimePeriodMachinery2StockCalculations(ref currentCalculationsElement, ref currentElement);
            //keep the newly set fileposition index
            //note that 02a supplements this method with with calculations that use the Timeliness collections
            return bHasCalculations;
        }
        public bool SetTimePeriodMachinery2StockCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //standard getally calcs will retrieve npv operation calculator
            //which must not be overwritten or replaced in db record which must not be overwritten if UseSameCalcInChildren = true
            //currentCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
            //    Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
            //CalculatorHelpers.ReplaceAnalyzerCalculator(this.GCCalculatorParams, currentElement,
            //    ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString(),
            //    ref currentCalculationsElement);
            if (currentCalculationsElement == null)
            {
                //a base npv calculation could not be found at this node level
                currentCalculationsElement = new XElement(this.GCCalculatorParams.LinkedViewElement);
            }
            //set the parent budget for holding collection of timeperiods
            if (this.TimelinessBudget == null)
            {
                this.TimelinessBudget = new TimelinessBI1();
            }
            //when this method is called from opcomp or input group
            if (this.TimelinessTimePeriod == null)
            {
                this.TimelinessTimePeriod = new TimelinessTimePeriod1();
            }
            //note that the machnpvoc calculator can not change TimePeriod properties
            //but needs properties from the TimePeriod (i.e. Amount)
            this.TimelinessTimePeriod.SetTimePeriodProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init machStock props
            this.Machinery2Stock.SetCalculatorProperties(currentCalculationsElement);
            //don't double count the tp multiplier -each op comp already used it to set penalties
            double dbMultiplier = 1;
            //the machStock.machstocks dictionary can now be summed to derive totals
            bHasCalculations = SetTotalMachinery2StockCalculations(dbMultiplier, currentElement.Name.LocalName);
            //serialize calculator object back to xml
            //(calculator doesn't change tp, so don't serialize it)
            string sAttNameExtension = string.Empty;
            this.Machinery2Stock.SetTotalMachinery2StockAttributes(sAttNameExtension,
                ref currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //reset machstock totals to zero (for next operation)
            this.Machinery2Stock.InitTotalMachinery2StockProperties();
            //reset machStock.machstocks fileposition index
            //next tp's machstock will be inserted in next index
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.TimelinessBudget.TimelinessTimePeriods == null)
            {
                this.TimelinessBudget.TimelinessTimePeriods = new List<TimelinessTimePeriod1>();
            }
            this.TimelinessBudget.TimelinessTimePeriods.Add(this.TimelinessTimePeriod);
            //reset for next collection
            this.TimelinessTimePeriod = null;
            return bHasCalculations;
        }
        public bool SetOutcome2Collection(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //make an output object 
            //note that the parent timeperiod resets timeliness penalties using the calcParams.ParentOutputs collection
            Outcome newOutcome = new Outcome();
            newOutcome.SetOutcomeProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            bHasCalculations = true;
            newOutcome.Outputs = new List<Output>();
            //when this method is called from opcomp or input group
            if (this.TimelinessTimePeriod == null)
            {
                this.TimelinessTimePeriod = new TimelinessTimePeriod1();
            }
            //add it to collection
            if (this.TimelinessTimePeriod.Outcomes == null)
            {
                this.TimelinessTimePeriod.Outcomes = new List<Outcome>();
            }
            //the base subscriber's AddAncestor method sets this.GCCalculatorParams.ParentOutcome property
            if (this.GCCalculatorParams.ParentOutcome != null)
            {
                if (this.GCCalculatorParams.ParentOutcome.Outputs != null)
                {
                    foreach (Output output in this.GCCalculatorParams.ParentOutcome.Outputs)
                    {
                        //no byrefs
                        Output newOutput = new Output(output);
                        newOutcome.Outputs.Add(newOutput);
                    }
                }
            }
            this.TimelinessTimePeriod.Outcomes.Add(newOutcome);
            return bHasCalculations;
        }
        public bool SetOutput2Collection(
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //make an output object 
            //note that the parent timeperiod resets timeliness penalties using the calcParams.ParentOutputs collection
            Output newOutput = new Output();
            newOutput.SetOutputProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            bHasCalculations = true;
            int iOutcomeId = CalculatorHelpers.GetAttributeInt(currentElement, BudgetInvestment.BUDGETSYSTEM_TO_OUTCOME_ID);
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
            {
                iOutcomeId = CalculatorHelpers.GetAttributeInt(currentElement, BudgetInvestment.COSTSYSTEM_TO_OUTCOME_ID);
            }
            //the base subscriber's AddAncestor method sets this.GCCalculatorParams.ParentOutcome property
            if (this.GCCalculatorParams.ParentOutcome != null)
            {
                if (this.GCCalculatorParams.ParentOutcome.Id == iOutcomeId)
                {
                    //add it to collection
                    if (this.GCCalculatorParams.ParentOutcome.Outputs == null)
                    {
                        this.GCCalculatorParams.ParentOutcome.Outputs = new List<Output>();
                    }
                    this.GCCalculatorParams.ParentOutcome.Outputs.Add(newOutput);
                }
            }
            return bHasCalculations;
        }
        
        public bool SetOpOrCompMachinery2StockJointCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
               || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices)
            {
                string sAnalyzerType = CalculatorHelpers.GetAnalyzerType(currentCalculationsElement);
                if (sAnalyzerType != this.GCCalculatorParams.AnalyzerParms.AnalyzerType)
                {
                    //standard getally calcs may retrieve resource 01 calculator
                    //(because finds relatedcalculatortype=agmachinery)
                    //which must not be overwritten if UseSameCalcInChildren = true
                    currentCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                        Calculator1.cAnalyzerType, this.GCCalculatorParams.AnalyzerParms.AnalyzerType);
                    //CalculatorHelpers.ReplaceAnalyzerCalculator(this.GCCalculatorParams, currentElement,
                    //   this.GCCalculatorParams.AnalyzerParms.AnalyzerType,
                    //    ref currentCalculationsElement);
                }
            }
            //step 4. Run both mach1stock and mach2stock calculations keeping both indices synch
            string sCalculatorType = CalculatorHelpers.CALCULATOR_TYPES.operation2.ToString();
            if (currentElement.Name.LocalName.EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                sCalculatorType = CalculatorHelpers.CALCULATOR_TYPES.operation2.ToString();
            }
            else if (currentElement.Name.LocalName.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                sCalculatorType = CalculatorHelpers.CALCULATOR_TYPES.component2.ToString();
            }
            if (currentCalculationsElement == null)
            {
                //either a base resource01 calculation was not run
                //or it was never added to base npv calculator 
                currentCalculationsElement = new XElement(this.GCCalculatorParams.LinkedViewElement);
            }
            //i.e. make sure to use the baseNPV linked view holding base machinery cost and timeliness penalty information
            XElement baseNPVCalculation = CalculatorHelpers.GetChildLinkedViewUsingAttribute(currentElement,
                Calculator1.cCalculatorType, sCalculatorType);
            if (baseNPVCalculation == null)
            {
                //the display will point out bad params if currentCalcEl is wrong
                baseNPVCalculation = currentCalculationsElement;
            }
            ////set machinery cost totals
            //int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            //when this method is called from opcomp or input group
            if (this.TimelinessOpComp == null)
            {
                this.TimelinessOpComp = new TimelinessOpComp1();
            }
            //note that the machnpvoc calculator can not change Operation properties
            //but needs several properties from the Operation (i.e. Id, Amount)
            this.TimelinessOpComp.SetOperationComponentProperties(this.GCCalculatorParams,
                currentCalculationsElement, currentElement);
            //init mach2Stock props
            this.Machinery2Stock.SetCalculatorProperties(currentCalculationsElement);
            this.TimelinessOpComp.SetTimelinessOC1Properties(baseNPVCalculation);
            //baseNPVCalc must be used to set machinery calcs
            this.SetOpOrCompMachineryStockCalculations(ref currentCalculationsElement, ref currentElement);
            //reset opcomp.amount to base (not the stored linkedview amount)
            this.TimelinessOpComp.SetTimelinessBaseProperties(currentElement);
            //set machinery cost totals
            int iFPIndex = this.GCCalculatorParams.AnalyzerParms.FilePositionIndex;
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex = iFPIndex;
            //set timeliness penalty totals
            bHasCalculations = SetOpOrCompMachinery2StockCalculations(ref currentCalculationsElement, ref currentElement);
            //keep the newly set fileposition index
            return bHasCalculations;
        }
        public bool SetOpOrCompMachinery2StockCalculations( 
            ref XElement currentCalculationsElement, ref XElement currentElement)
        {
            //set timeliness cost penalties only
            bool bHasCalculations = false;
            //if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.operationprices
            //    || this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.componentprices)
            //{
            //    //standard getally calcs will retrieve npv operation calculator
            //    //which must not be overwritten or replaced in db record which must not be overwritten 
            //    //if UseSameCalcInChildren = true
            //    CalculatorHelpers.ReplaceAnalyzerCalculator(this.GCCalculatorParams, currentElement,
            //        ARSAnalyzerHelper.ANALYZER_TYPES.resources02.ToString(),
            //        ref currentCalculationsElement);
            //}
            //set the parent tp for holding collection of opcomps
            if (this.TimelinessTimePeriod == null)
            {
                this.TimelinessTimePeriod = new TimelinessTimePeriod1();
            }

            //reset the outputs and rerun the timeliness cost using the outputs, and tp.amount, for this budget
            //parenttp was set during addancestor event
            double dbTPAmount = (this.GCCalculatorParams.ParentTimePeriod != null) 
                ? this.GCCalculatorParams.ParentTimePeriod.Amount : 1;
            this.TimelinessOpComp.ReSetParentTimePeriodProperties(dbTPAmount, this.TimelinessTimePeriod.Outcomes);
            //reset the attributes to real outputs (this is usually true)
            this.TimelinessOpComp.SetTimelinessOC1Attributes(string.Empty, currentCalculationsElement);
            //need to display some base atts with calculated results
            this.TimelinessOpComp.SetTimelinessBaseAttributes(string.Empty, currentCalculationsElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(
                currentCalculationsElement, currentElement);
            //add the machnpvoc to the machStock.machstocks dictionary
            //the count is 1-based, while iNodePosition is 0-based
            //so the count is the correct next index position
            int iNodePosition = this.Machinery2Stock.GetNodePositionCount(
                this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, this.TimelinessOpComp);
            if (iNodePosition < 0)
                iNodePosition = 0;
            bHasCalculations = this.Machinery2Stock
                .AddMachinery2StocksToDictionary(
                this.GCCalculatorParams.AnalyzerParms.FilePositionIndex, iNodePosition,
                this.TimelinessOpComp);
            //keep this consistent with other stock calculators
            //reset machStock.machstocks fileposition index
            //next opOrComp's machstock will be inserted in next index
            this.GCCalculatorParams.AnalyzerParms.FilePositionIndex += 1;
            //add to collection
            if (this.TimelinessTimePeriod.TimelinessOpComps == null)
            {
                this.TimelinessTimePeriod.TimelinessOpComps = new List<TimelinessOpComp1>();
            }
            this.TimelinessTimePeriod.TimelinessOpComps.Add(this.TimelinessOpComp);
            //reset for next collection
            this.TimelinessOpComp = null;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetInputMachinery2StockJointCalculations(ref XElement currentCalculationsElement,
            ref XElement currentElement)
        {
            bool bHasCalculations = false;
            //set the parent opcomp for holding collection of machinputs
            if (this.TimelinessOpComp == null)
            {
                this.TimelinessOpComp = new TimelinessOpComp1();
                this.TimelinessOpComp.TimelinessOpComps = new List<TimelinessOpComp1>();
            }
            //run the base mach1input calcs to set machinery costs
            bHasCalculations = this.SetInputMachineryStockCalculations(ref currentCalculationsElement,
                ref currentElement);
            //set calculatorid (primary way to display calculation attributes)
            CalculatorHelpers.SetCalculatorId(currentCalculationsElement, currentElement);
            //add to collection
            if (this.TimelinessOpComp.Inputs == null)
            {
                this.TimelinessOpComp.Inputs = new List<Input>();
            }
            //note that machinput can be retrieved by converting the input to the 
            //Machinery1Input type (machinput = (Machinery1Input) input)
            if (this.Mach1Input != null)
            {
                this.TimelinessOpComp.Inputs.Add(this.Mach1Input);
            }
            return bHasCalculations;
        }
        public bool SetTotalMachinery2StockCalculations(double multiplier, string currentNodeName)
        {
            bool bHasCalculations = false;
            if (this.Machinery2Stock.Machinery2Stocks != null)
            {
                int iFilePosition = 0;
                string sAttNameExtension = string.Empty;
                foreach (KeyValuePair<int, List<TimelinessOpComp1>> kvp
                    in this.Machinery2Stock.Machinery2Stocks)
                {
                    iFilePosition = kvp.Key;
                    bool bNeedsFilePositionFacts = NeedsFilePosition(iFilePosition, currentNodeName);
                    if (bNeedsFilePositionFacts)
                    {
                        foreach (TimelinessOpComp1 machnpvoc in kvp.Value)
                        {
                            bHasCalculations
                                = AddTimelinessOpComp1ToStock(multiplier, machnpvoc);
                        }
                    }
                }
                if (currentNodeName
                    == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                    || currentNodeName
                    == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
                {
                    //set the stateful budget index 
                    this.BudgetStartingFileIndex = iFilePosition;
                }
                else if (currentNodeName
                    == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                    || currentNodeName
                    == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    //set the stateful time period index 
                    this.TimePeriodStartingFileIndex = iFilePosition;
                }
            }
            return bHasCalculations;
        }
        public bool AddTimelinessOpComp1ToStock(double multiplier, TimelinessOpComp1 machnpvoc)
        {
            bool bHasCalculations = false;
            //this should only be done at tp stock analysis level when the opcomps were correctly
            //adjusted by output collection and by tp.amount (i.e. use opcomp.ReSetParentTimePeriodProperties)
            this.Machinery2Stock.TotalAmount += (machnpvoc.Amount * multiplier);
            this.Machinery2Stock.TotalLaborAvailable += (machnpvoc.LaborAvailable);
            this.Machinery2Stock.TotalWorkdayProbability += (machnpvoc.WorkdayProbability);
            this.Machinery2Stock.TotalTimelinessPenalty1 += (machnpvoc.TimelinessPenalty1);
            this.Machinery2Stock.TotalTimelinessPenaltyDaysFromStart1 += (machnpvoc.TimelinessPenaltyDaysFromStart1);
            this.Machinery2Stock.TotalTimelinessPenalty2 += (machnpvoc.TimelinessPenalty2 * multiplier);
            this.Machinery2Stock.TotalTimelinessPenaltyDaysFromStart2 += (machnpvoc.TimelinessPenaltyDaysFromStart2);
            this.Machinery2Stock.TotalWorkdaysLimit += (machnpvoc.WorkdaysLimit * multiplier);
            this.Machinery2Stock.TotalFieldCapacity += (machnpvoc.FieldCapacity);
            this.Machinery2Stock.TotalAreaCovered += (machnpvoc.AreaCovered * multiplier);
            this.Machinery2Stock.TotalFieldDays += (machnpvoc.FieldDays * multiplier);
            this.Machinery2Stock.TotalOutputPrice += (machnpvoc.OutputPrice);
            this.Machinery2Stock.TotalOutputYield += (machnpvoc.OutputYield);
            this.Machinery2Stock.TotalProbableFieldDays += (machnpvoc.ProbableFieldDays * multiplier);
            this.Machinery2Stock.TotalTimelinessPenaltyCost += (machnpvoc.TimelinessPenaltyCost * multiplier);
            this.Machinery2Stock.TotalTimelinessPenaltyCostPerHour += (machnpvoc.TimelinessPenaltyCostPerHour);
            this.Machinery2Stock.TotalR += (machnpvoc.TotalR);
            //next ancestor needs a stateful machnpvoc that has been adjusted
            //by multiplier: machnpvoc has multiplicative adjustments 
            //i.e. budget adjustement = in.times * op.amount * tp.amount)
            ChangeMachinery2InputByMultiplier(machnpvoc, multiplier);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddTimelinessOpComp1ToStock(Machinery2Stock mach2Stock,
            double multiplier, TimelinessOpComp1 machnpvoc)
        {
            bool bHasCalculations = false;
            //this should only be done at tp stock analysis level when the opcomps were correctly
            //adjusted by output collection and by tp.amount (i.e. use opcomp.ReSetParentTimePeriodProperties)
            mach2Stock.TotalAmount += (machnpvoc.Amount * multiplier);
            mach2Stock.TotalLaborAvailable += (machnpvoc.LaborAvailable);
            mach2Stock.TotalWorkdayProbability += (machnpvoc.WorkdayProbability);
            mach2Stock.TotalTimelinessPenalty1 += (machnpvoc.TimelinessPenalty1);
            mach2Stock.TotalTimelinessPenaltyDaysFromStart1 += (machnpvoc.TimelinessPenaltyDaysFromStart1);
            mach2Stock.TotalTimelinessPenalty2 += (machnpvoc.TimelinessPenalty2 * multiplier);
            mach2Stock.TotalTimelinessPenaltyDaysFromStart2 += (machnpvoc.TimelinessPenaltyDaysFromStart2);
            mach2Stock.TotalWorkdaysLimit += (machnpvoc.WorkdaysLimit * multiplier);
            mach2Stock.TotalFieldCapacity += (machnpvoc.FieldCapacity);
            mach2Stock.TotalAreaCovered += (machnpvoc.AreaCovered * multiplier);
            mach2Stock.TotalFieldDays += (machnpvoc.FieldDays * multiplier);
            mach2Stock.TotalOutputPrice += (machnpvoc.OutputPrice);
            mach2Stock.TotalOutputYield += (machnpvoc.OutputYield);
            mach2Stock.TotalProbableFieldDays += (machnpvoc.ProbableFieldDays * multiplier);
            mach2Stock.TotalTimelinessPenaltyCost += (machnpvoc.TimelinessPenaltyCost * multiplier);
            mach2Stock.TotalTimelinessPenaltyCostPerHour += (machnpvoc.TimelinessPenaltyCostPerHour);
            mach2Stock.TotalR += (machnpvoc.TotalR);
            bHasCalculations = true;
            return bHasCalculations;
        }
        public static bool AddMachinery2StockToStock(Machinery2Stock totalsMach2Stock,
            double multiplier, Machinery2Stock currentMach2Stock)
        {
            bool bHasCalculations = false;
            //this should only be done at tp stock analysis level when the opcomps were correctly
            //adjusted by output collection and by tp.amount (i.e. use opcomp.ReSetParentTimePeriodProperties)
            totalsMach2Stock.TotalAmount += (currentMach2Stock.TotalAmount * multiplier);
            totalsMach2Stock.TotalLaborAvailable += (currentMach2Stock.TotalLaborAvailable);
            totalsMach2Stock.TotalWorkdayProbability += (currentMach2Stock.TotalWorkdayProbability);
            totalsMach2Stock.TotalTimelinessPenalty1 += (currentMach2Stock.TotalTimelinessPenalty1);
            totalsMach2Stock.TotalTimelinessPenaltyDaysFromStart1 += (currentMach2Stock.TotalTimelinessPenaltyDaysFromStart1);
            totalsMach2Stock.TotalTimelinessPenalty2 += (currentMach2Stock.TotalTimelinessPenalty2 * multiplier);
            totalsMach2Stock.TotalTimelinessPenaltyDaysFromStart2 += (currentMach2Stock.TotalTimelinessPenaltyDaysFromStart2);
            totalsMach2Stock.TotalWorkdaysLimit += (currentMach2Stock.TotalWorkdaysLimit * multiplier);
            totalsMach2Stock.TotalFieldCapacity += (currentMach2Stock.TotalFieldCapacity);
            totalsMach2Stock.TotalAreaCovered += (currentMach2Stock.TotalAreaCovered * multiplier);
            totalsMach2Stock.TotalFieldDays += (currentMach2Stock.TotalFieldDays * multiplier);
            totalsMach2Stock.TotalOutputPrice += (currentMach2Stock.TotalOutputPrice);
            totalsMach2Stock.TotalOutputYield += (currentMach2Stock.TotalOutputYield);
            totalsMach2Stock.TotalProbableFieldDays += (currentMach2Stock.TotalProbableFieldDays * multiplier);
            totalsMach2Stock.TotalTimelinessPenaltyCost += (currentMach2Stock.TotalTimelinessPenaltyCost * multiplier);
            totalsMach2Stock.TotalTimelinessPenaltyCostPerHour += (currentMach2Stock.TotalTimelinessPenaltyCostPerHour);
            totalsMach2Stock.TotalR += (currentMach2Stock.TotalR);
            bHasCalculations = true;
            return bHasCalculations;
        }
        //162 eliminated ocs already used tp amounts
        public void ReCalculateTimePeriodTimelinessOpCompCollections()
        {
            //calculators that use the TimelinessTP.TimelinessOpComps collection 
            //call this method to recalculate the tp.timelyopcomps timeliness penalties
            //usually using newly created this.TimelinessTimePeriod.TimelinessOpComps
            if (this.TimelinessTimePeriod != null)
            {
                if (this.TimelinessTimePeriod.TimelinessOpComps != null)
                {
                    double dbTPAmount = (this.GCCalculatorParams.ParentTimePeriod != null)
                        ? this.GCCalculatorParams.ParentTimePeriod.Amount : 1;
                    foreach (var uniqueOpComp in this.TimelinessTimePeriod.TimelinessOpComps)
                    {
                        //feasibles get the new calculations
                        if (uniqueOpComp.TimelinessOpComps != null)
                        {
                            foreach (var feasibleOpComp in uniqueOpComp.TimelinessOpComps)
                            {
                                //feasibles get the new calculations
                                feasibleOpComp.ReSetParentTimePeriodProperties(dbTPAmount,
                                    this.TimelinessTimePeriod.Outcomes);
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.TimelinessBudget != null)
                {

                    if (this.TimelinessBudget.TimelinessTimePeriods != null)
                    {
                        if (this.TimelinessBudget.TimelinessTimePeriods.Last() != null)
                        {
                            if (this.TimelinessBudget.TimelinessTimePeriods.Last().TimelinessOpComps != null)
                            {
                                double dbTPAmount = (this.GCCalculatorParams.ParentTimePeriod != null)
                                    ? this.GCCalculatorParams.ParentTimePeriod.Amount : 1;
                                foreach (var uniqueOpComp in this.TimelinessBudget.TimelinessTimePeriods.Last().TimelinessOpComps)
                                {
                                    //feasibles get the new calculations
                                    if (uniqueOpComp.TimelinessOpComps != null)
                                    {
                                        foreach (var feasibleOpComp in uniqueOpComp.TimelinessOpComps)
                                        {
                                            //feasibles get the new calculations
                                            feasibleOpComp.ReSetParentTimePeriodProperties(dbTPAmount,
                                                this.TimelinessBudget.TimelinessTimePeriods.Last().Outcomes);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private bool NeedsFilePosition(int filePosition, string currentNodeName)
        {
            bool bNeedsFilePositionFacts = true;
            if (currentNodeName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentNodeName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //each opOrComp is a file position member
                //aggregate only using the current file position
                if (this.GCCalculatorParams.AnalyzerParms.FilePositionIndex != filePosition)
                {
                    bNeedsFilePositionFacts = false;
                }
            }
            else if (currentNodeName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentNodeName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                //set the stateful budget index 
                if (this.BudgetStartingFileIndex == 0)
                    return true;
                if (filePosition > this.BudgetStartingFileIndex)
                {
                    bNeedsFilePositionFacts = true;
                }
                else
                {
                    bNeedsFilePositionFacts = false;
                }
            }
            else if (currentNodeName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentNodeName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //set the stateful time period index 
                if (this.TimePeriodStartingFileIndex == 0)
                    return true;
                if (filePosition > this.TimePeriodStartingFileIndex)
                {
                    bNeedsFilePositionFacts = true;
                }
                else
                {
                    bNeedsFilePositionFacts = false;
                }
            }
            return bNeedsFilePositionFacts;
        }
        public static void ChangeMachinery2InputByMultiplier(TimelinessOpComp1 machnpvoc,
            double multiplier)
        {
            //the next ancestor needs to aggregate with whatever multiplier 
            //was used by immediate descendant (i.e. Input.Times for opOrComp, 
            //Operation.Amount for timeperiod, TimePeriod.Amount for budget;
            machnpvoc.Amount = (machnpvoc.Amount * multiplier);
            machnpvoc.LaborAvailable = (machnpvoc.LaborAvailable);
            machnpvoc.WorkdayProbability = (machnpvoc.WorkdayProbability);
            machnpvoc.TimelinessPenalty1 = (machnpvoc.TimelinessPenalty1);
            machnpvoc.TimelinessPenaltyDaysFromStart1 = (machnpvoc.TimelinessPenaltyDaysFromStart1);
            machnpvoc.TimelinessPenalty2 = (machnpvoc.TimelinessPenalty2);
            machnpvoc.TimelinessPenaltyDaysFromStart2 = (machnpvoc.TimelinessPenaltyDaysFromStart2);
            machnpvoc.WorkdaysLimit = (machnpvoc.WorkdaysLimit * multiplier);
            machnpvoc.FieldCapacity = (machnpvoc.FieldCapacity);
            machnpvoc.AreaCovered = (machnpvoc.AreaCovered * multiplier);
            machnpvoc.FieldDays = (machnpvoc.FieldDays * multiplier);
            machnpvoc.OutputPrice = (machnpvoc.OutputPrice);
            machnpvoc.OutputYield = (machnpvoc.OutputYield);
            machnpvoc.ProbableFieldDays = (machnpvoc.ProbableFieldDays * multiplier);
            machnpvoc.TimelinessPenaltyCost = (machnpvoc.TimelinessPenaltyCost * multiplier);
        }
        
        public static void AddStock1TotalsToElement(Machinery1Stock mach1Stock,
            TimelinessOpComp1 currentTOC, ref XElement opCompCalcElement)
        {
            //these have already been adjusted by input multipliers (ocamount, times)
            //and opcomp mulipliers (amount)
            currentTOC.TotalOC = mach1Stock.TotalFuelCost + mach1Stock.TotalLaborCost
                + mach1Stock.TotalRepairCost + mach1Stock.TotalLubeOilCost;
            currentTOC.TotalAOH = mach1Stock.TotalCapitalRecoveryCost
                + mach1Stock.TotalTaxesHousingInsuranceCost;
            CalculatorHelpers.SetAttributeDoubleF2(opCompCalcElement,
                CostBenefitCalculator.TOC, currentTOC.TotalOC);
            CalculatorHelpers.SetAttributeDoubleF2(opCompCalcElement,
                CostBenefitCalculator.TAOH, currentTOC.TotalAOH);
        }
        
    }
}
