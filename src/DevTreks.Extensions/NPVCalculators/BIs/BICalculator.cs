using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Run net present value calculations for operating and capital budgets
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    // NOTES        1. Although both operating and capital budgets both use the same 
    //              net present value calculations in this calculator, future 
    //              calculators are expected to target the two subapps individually.
    //              2. Carry out calculations by deserializing currentCalculationsElement 
    //              and currentElement into an AddInViews.BaseObject and using the object
    //              to run the calculations
    //              3. Serialize the object's new calculations back to 
    //              currentCalculationsElement and currentElement, and fill in 
    //              the updates collection with any db fields that have changed
    /// </summary>
    public class BICalculator
    {
        //constants used by this calculator
        public const string ZERO = "0";
        public const double DOUBLE_YEAR = 365.25;
        public const int INT_SEED = 123561278;
        public const string SPACE = " ";
        public const string PREPRODCAPRECOVERY = "Preproduction Capital Recovery";
        public const string PREPRODCAPRECOVERY_ABB = "Preprod. Cap. Recov.";
        public const string CAP_RECOVERY_I_ONLY = "Cap. Rec. I only";
        public const string CAP_RECOVERY_PANDI = "Cap. Recov. P&I";
        public const string UNIFORMSERIES = "uniform";
        public const string LINEARSERIES = "linear";
        public const string GEOMETRICSERIES = "geometric";
        public const string OVER = "Over ";
        public const string PLUS = "Plus ";
        public const string MOREPERIODS = " More Periods";
        public const string CAPRECOVERY = "Capital Recovery";

        public bool SetBudgetAndInvestmentCalculations(
            CalculatorHelpers.CALCULATOR_TYPES calculatorType, CalculatorParameters calcParameters, 
            ref XElement currentCalculationsElement, ref XElement currentElement, 
            IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            //use the same basic pattern in all of the initial calculators
            switch (calculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.budget:
                    bHasCalculations = AddNPVCalculationsToCurrentElement(calcParameters,
                        ref currentCalculationsElement, ref currentElement,
                        updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.investment:
                    //npv ok, but so too are newer techniques
                    bHasCalculations = AddNPVCalculationsToCurrentElement(calcParameters,
                        ref currentCalculationsElement, ref currentElement,
                        updates);
                    break;
                default:
                    break;
            }
            return bHasCalculations;
        }
        private bool AddNPVCalculationsToCurrentElement(
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString())
            {
                //uses calcParams.ParentBudgetInvestmentGroup for calculations
                bHasCalculations = SetBudgetInvestmentGroupCalculations(
                    calcParameters);
                //serialize object back to xml
                calcParameters.ParentBudgetInvestmentGroup.SetBudgetInvestmentGroupAttributes(
                    calcParameters, ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentBudgetInvestmentGroup.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
                if (calcParameters.ParentBudgetInvestmentGroup.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentBudgetInvestmentGroup.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                //uses calcParams.ParentBudgetInvestmentGroup for calculations
                bHasCalculations = SetBudgetInvestmentGroupCalculations(
                    calcParameters);
                //serialize object back to xml
                calcParameters.ParentBudgetInvestmentGroup.SetBudgetInvestmentGroupAttributes(
                    calcParameters, ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentBudgetInvestmentGroup.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
                if (calcParameters.ParentBudgetInvestmentGroup.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentBudgetInvestmentGroup.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString())
            {
                //uses calcParams.ParentBudget for calculations
                bHasCalculations = SetBudgetInvestmentCalculations(
                    calcParameters);
                //serialize object back to xml
                calcParameters.ParentBudgetInvestment.SetBudgetInvestmentAttributes(
                    calcParameters, ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentBudgetInvestment.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
                if (calcParameters.ParentBudgetInvestment.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentBudgetInvestment.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                //uses calcParams.ParentInvestment for calculations
                bHasCalculations = SetBudgetInvestmentCalculations(
                    calcParameters);
                //serialize object back to xml
                calcParameters.ParentBudgetInvestment.SetBudgetInvestmentAttributes(
                    calcParameters, ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentBudgetInvestment.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
                if (calcParameters.ParentBudgetInvestment.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentBudgetInvestment.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString())
            {
                //uses calcParams.ParentTimePeriod for calculations
                bHasCalculations = SetBudgetInvestmentTimePeriodCalculations(
                    calcParameters);
                //serialize object back to xml
                calcParameters.ParentTimePeriod.SetTimePeriodAttributes(
                    calcParameters, ref currentCalculationsElement,
                    ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentTimePeriod.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                //uses calcParams.ParentTimePeriod for calculations
                bHasCalculations = SetBudgetInvestmentTimePeriodCalculations(
                    calcParameters);
                //serialize object back to xml
                calcParameters.ParentTimePeriod.SetTimePeriodAttributes(
                    calcParameters, ref currentCalculationsElement,
                    ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentTimePeriod.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //uses calcParams.ParentOutcome for calculations
                bHasCalculations = SetOutcomeCalculations(
                    calcParameters, calcParameters.ParentOutcome);
                //serialize object back to xml
                calcParameters.ParentOutcome.SetOutcomeAttributes(
                    calcParameters, ref currentElement, updates);
                //version 1.4.5
                calcParameters.ParentOutcome.SetTotalBenefitsPsandQsAttributes(
                    string.Empty, ref currentElement);
                if (calcParameters.ParentOutcome.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentOutcome.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                Output output = new Output();
                //deserialize xml to object
                output.SetOutputProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                bHasCalculations = SetOutputCalculations(
                    calcParameters, output, ref currentCalculationsElement);
                //serialize object back to xml
                output.SetOutputAttributes(
                    calcParameters, ref currentElement, updates);
                output.SetTotalBenefitsAttributes(string.Empty, ref currentElement);
                if (output.Local != null)
                {
                    //locals only for calculator
                    output.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                //uses calcParams.ParentOperation for calculations
                bHasCalculations = SetOperationComponentCalculations(
                    calcParameters, calcParameters.ParentOperationComponent);
                //serialize object back to xml
                calcParameters.ParentOperationComponent.SetOperationComponentAttributes(
                    calcParameters, ref currentElement, updates);
                if (calcParameters.ParentOperationComponent.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentOperationComponent.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //uses calcParams.ParentComponent for calculations
                bHasCalculations = SetOperationComponentCalculations(
                    calcParameters, calcParameters.ParentOperationComponent);
                //serialize object back to xml
                calcParameters.ParentOperationComponent.SetOperationComponentAttributes(
                    calcParameters, ref currentElement, updates);
                if (calcParameters.ParentOperationComponent.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentOperationComponent.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                Input input = new Input();
                //deserialize xml to object
                input.SetInputProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                bHasCalculations = SetInputCalculations(
                    calcParameters, input, ref currentCalculationsElement);
                //serialize object back to xml
                input.SetInputAttributes(
                    calcParameters, ref currentElement, updates);
                //and set the totals
                input.SetTotalCostsAttributes(string.Empty, ref currentElement);
                if (input.Local != null)
                {
                    //locals only for calculator
                    input.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                }
            }
            return bHasCalculations;
        }
        private bool SetBudgetInvestmentGroupCalculations(
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //no reason, in current version, to have cumulative group totals
            //ParentGroupTotals.TotalOC += budgetOrInvestmentGroup.TotalOC;
            //limited to single group totals which were accumlated 
            //in calcParameters.ParentBudgetInvestmentGroup when 
            //SetBudgetInvestmentCalculations was run
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool SetBudgetInvestmentCalculations(
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            
            //refactor: current version does not clean up annuities
            //previous field work suggests its more confusing than helpful
            //CleanUpAnnuities(calcParameters, ref totalsCalcs);
            //set the object's calculations (nonamortized calculations)
            CalculateBudgetInvestment(ref calcParameters);
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool SetBudgetInvestmentTimePeriodCalculations(
            CalculatorParameters calcParameters)
        {
            bool bHasCalculations = false;
            //set preproduction annuities
            SetPreProductionAnnuity(calcParameters);
            //add annuities found in TimePeriod.AnnEquivs to this time period
            AddTimePeriodAnnuities(calcParameters);
            //add the annuities to this timeperiod's totals
            AddAnnuitiesToTimePeriodTotals(calcParameters);
            //set the object's calculations 
            CalculateTimePeriod(ref calcParameters);
            //add this timeperiod's totals to the corresponding member of the 
            //ParentBudgetInvestment.TimePeriods collection
            AddTimePeriodTotalsToTimePeriodsCollection(calcParameters);
            //set cumulative totals based on the calcs above
            calcParameters.ParentBudgetInvestment.TotalOC_INT += calcParameters.ParentTimePeriod.TotalOC_INT;
            calcParameters.ParentBudgetInvestment.TotalAOH_INT += calcParameters.ParentTimePeriod.TotalAOH_INT;
            calcParameters.ParentBudgetInvestment.TotalCAP_INT += calcParameters.ParentTimePeriod.TotalCAP_INT;
            calcParameters.ParentBudgetInvestment.TotalOC += calcParameters.ParentTimePeriod.TotalOC;
            calcParameters.ParentBudgetInvestment.TotalAOH += calcParameters.ParentTimePeriod.TotalAOH;
            calcParameters.ParentBudgetInvestment.TotalCAP += calcParameters.ParentTimePeriod.TotalCAP;
            calcParameters.ParentBudgetInvestment.TotalINCENT += calcParameters.ParentTimePeriod.TotalINCENT;
            calcParameters.ParentBudgetInvestment.TotalR += calcParameters.ParentTimePeriod.TotalR;
            calcParameters.ParentBudgetInvestment.TotalR_INT += calcParameters.ParentTimePeriod.TotalR_INT;
            calcParameters.ParentBudgetInvestment.TotalRINCENT += calcParameters.ParentTimePeriod.TotalRINCENT;

            calcParameters.ParentBudgetInvestment.TotalAMR_INT += calcParameters.ParentTimePeriod.TotalAMR_INT;
            calcParameters.ParentBudgetInvestment.TotalAMOC_INT += calcParameters.ParentTimePeriod.TotalAMOC_INT;
            calcParameters.ParentBudgetInvestment.TotalAMAOH_INT += calcParameters.ParentTimePeriod.TotalAMAOH_INT;
            calcParameters.ParentBudgetInvestment.TotalAMCAP_INT += calcParameters.ParentTimePeriod.TotalAMCAP_INT;
            calcParameters.ParentBudgetInvestment.TotalAMR += calcParameters.ParentTimePeriod.TotalAMR;
            calcParameters.ParentBudgetInvestment.TotalAMOC += calcParameters.ParentTimePeriod.TotalAMOC;
            calcParameters.ParentBudgetInvestment.TotalAMAOH += calcParameters.ParentTimePeriod.TotalAMAOH;
            calcParameters.ParentBudgetInvestment.TotalAMCAP += calcParameters.ParentTimePeriod.TotalAMCAP;
            calcParameters.ParentBudgetInvestment.TotalAMINCENT += calcParameters.ParentTimePeriod.TotalAMINCENT;
            calcParameters.ParentBudgetInvestment.TotalAMRINCENT += calcParameters.ParentTimePeriod.TotalAMRINCENT;
            
            //160 moved set tp amort nets and totals into CalculateTimePeriod and eliminated NET2
            //double dbNetTotal = calcParameters.ParentTimePeriod.TotalAMR - calcParameters.ParentTimePeriod.TotalAMOC;
           
            calcParameters.ParentBudgetInvestment.TotalAMOC_NET += calcParameters.ParentTimePeriod.TotalAMOC_NET;
            calcParameters.ParentBudgetInvestment.TotalAMAOH_NET += calcParameters.ParentTimePeriod.TotalAMAOH_NET;
            //calcParameters.ParentBudgetInvestment.TotalAMAOH_NET2 += calcParameters.ParentTimePeriod.TotalAMAOH_NET2;
            calcParameters.ParentBudgetInvestment.TotalAMCAP_NET += calcParameters.ParentTimePeriod.TotalAMCAP_NET;

            calcParameters.ParentBudgetInvestment.TotalRAmount += calcParameters.ParentTimePeriod.TotalRAmount;
            calcParameters.ParentBudgetInvestment.TotalRCompositionAmount += calcParameters.ParentTimePeriod.TotalRCompositionAmount;
            calcParameters.ParentBudgetInvestment.TotalRPrice += calcParameters.ParentTimePeriod.TotalRPrice;
            calcParameters.ParentBudgetInvestment.TotalRName = calcParameters.ParentTimePeriod.TotalRName;
            calcParameters.ParentBudgetInvestment.TotalRUnit = calcParameters.ParentTimePeriod.TotalRUnit;
            calcParameters.ParentBudgetInvestment.TotalRCompositionUnit = calcParameters.ParentTimePeriod.TotalRCompositionUnit;
            //if this timeperiod has growthperiods > 1 add another timeperiod
            //and internally handle cumulative totals
            AddGrowthPeriods(calcParameters);
            //total oc + aoh + cap costs
            calcParameters.ParentBudgetInvestment.TotalAMTOTAL 
                = calcParameters.ParentBudgetInvestment.TotalAMOC + calcParameters.ParentBudgetInvestment.TotalAMAOH + calcParameters.ParentBudgetInvestment.TotalAMCAP;
            //total tr - oc + aoh + cap costs
            //must set tp nets first -they are not cumulative
            //total tr - oc + aoh + cap costs
            calcParameters.ParentBudgetInvestment.TotalAMNET = calcParameters.ParentBudgetInvestment.TotalAMR - calcParameters.ParentBudgetInvestment.TotalAMTOTAL;
            calcParameters.ParentBudgetInvestment.TotalAMINCENT_NET = calcParameters.ParentBudgetInvestment.TotalAMRINCENT - calcParameters.ParentBudgetInvestment.TotalAMINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetOutcomeCalculations(
            CalculatorParameters calcParameters, Outcome currentOutcome)
        {
            bool bHasCalculations = false;
            //set the object's calculations (nonamortized calculations)
            CalculateOutcomePrice(ref calcParameters);
            //reset totals based on the new calculations
            calcParameters.ParentTimePeriod.TotalR_INT += calcParameters.ParentOutcome.TotalR_INT;
            calcParameters.ParentTimePeriod.TotalR += calcParameters.ParentOutcome.TotalR;
            calcParameters.ParentTimePeriod.TotalRINCENT += calcParameters.ParentOutcome.TotalRINCENT;
            //output ps and qs are aggregated for ce analysis
            calcParameters.ParentTimePeriod.TotalRAmount += calcParameters.ParentOutcome.TotalRAmount;
            calcParameters.ParentTimePeriod.TotalRCompositionAmount += calcParameters.ParentOutcome.TotalRCompositionAmount;
            calcParameters.ParentTimePeriod.TotalRPrice += calcParameters.ParentOutcome.TotalRPrice;
            calcParameters.ParentTimePeriod.TotalRName = calcParameters.ParentOutcome.TotalRName;
            calcParameters.ParentTimePeriod.TotalRUnit = calcParameters.ParentOutcome.TotalRUnit;
            calcParameters.ParentTimePeriod.TotalRCompositionUnit = calcParameters.ParentOutcome.TotalRCompositionUnit;
            //set up new annuities and set the attributes of the cash flows 
            //for annuals column
            if ((currentOutcome.AnnuityType != TimePeriod.ANNUITY_TYPES.preproduction)
                && (currentOutcome.AnnuityType != TimePeriod.ANNUITY_TYPES.regular))
            {
                //NOTE: regular annuity cash flows at comp level, 
                //without amortized principal, (incentives and compamount handled above)
                CalculateOutcomeAmortizedBen(calcParameters);
            }
            //reset totals based on the new calculations
            //all annuities set at comp level not input
            calcParameters.ParentTimePeriod.TotalAMR_INT += calcParameters.ParentOutcome.TotalAMR_INT;
            calcParameters.ParentTimePeriod.TotalAMR += calcParameters.ParentOutcome.TotalAMR;
            calcParameters.ParentTimePeriod.TotalAMRINCENT += calcParameters.ParentOutcome.TotalAMRINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetOutputCalculations(CalculatorParameters calcParameters,
            Output output, ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            double dbOutPrice1Total = 0;
            double dbOutPriceInt1Total = 0;
            //don't process previously calculated nodes
            bool bIsAnnuity = TimePeriod.IsAnnuity(calcParameters.ParentTimePeriod);
            if (!bIsAnnuity)
            {
                if (currentCalculationsElement != null)
                {
                    //adjust output's prices and amounts if joint calculations had to be run
                    //when ParentOutcome was set
                    NPVOutcomeCalculator outcomeCalculator = new NPVOutcomeCalculator();
                    outcomeCalculator.AdjustJointOutputCalculations(
                        output, ref currentCalculationsElement, calcParameters);
                }
                CalculateOutputPrice(output, ref calcParameters);
            }

            dbOutPrice1Total = output.TotalR;
            dbOutPriceInt1Total = output.TotalR_INT;
            //version 1.4.5 requires outputs to use same pattern as inputs, and add interest back in when displaying output totals
            calcParameters.ParentOutcome.TotalR += dbOutPrice1Total; 
            calcParameters.ParentOutcome.TotalR_INT += dbOutPriceInt1Total;
            calcParameters.ParentOutcome.TotalRINCENT += output.TotalRINCENT;
            //unlike input.Times, TotalRCompositionAmount is tracked and can be analyzed separately
            calcParameters.ParentOutcome.TotalRAmount += output.Amount;
            //v145a left out the output.times
            calcParameters.ParentOutcome.TotalRCompositionAmount += (output.CompositionAmount * output.Times);
            calcParameters.ParentOutcome.TotalRPrice += output.Price;
            //keep for c/e analysis
            calcParameters.ParentOutcome.TotalRName = output.Name;
            calcParameters.ParentOutcome.TotalRUnit = output.Unit;
            calcParameters.ParentOutcome.TotalRCompositionUnit = output.CompositionUnit;
            //note that calculators should not be setting separate values for output.TotalRAmount ...
            //standard pattern uses multipliers (Outcome.Amount) to change values
            bHasCalculations = true;
            return bHasCalculations;
        }
        
        public bool SetOperationComponentCalculations(
            CalculatorParameters calcParameters, OperationComponent currentOpComp)
        {
            bool bHasCalculations = false;
            //set the object's calculations (nonamortized calculations)
            CalculateOperationComponentPrice(ref calcParameters);
            //reset totals based on the new calculations
            calcParameters.ParentTimePeriod.TotalOC_INT += calcParameters.ParentOperationComponent.TotalOC_INT;
            calcParameters.ParentTimePeriod.TotalAOH_INT += calcParameters.ParentOperationComponent.TotalAOH_INT;
            calcParameters.ParentTimePeriod.TotalCAP_INT += calcParameters.ParentOperationComponent.TotalCAP_INT;
            calcParameters.ParentTimePeriod.TotalOC += calcParameters.ParentOperationComponent.TotalOC;
            calcParameters.ParentTimePeriod.TotalAOH += calcParameters.ParentOperationComponent.TotalAOH;
            calcParameters.ParentTimePeriod.TotalCAP += calcParameters.ParentOperationComponent.TotalCAP;
            calcParameters.ParentTimePeriod.TotalINCENT += calcParameters.ParentOperationComponent.TotalINCENT;
            //set up new annuities and set the attributes of the cash flows 
            //for annuals column
            if ((currentOpComp.AnnuityType != TimePeriod.ANNUITY_TYPES.preproduction)
                && (currentOpComp.AnnuityType != TimePeriod.ANNUITY_TYPES.regular))
            {
                //NOTE: regular annuity cash flows at comp level, 
                //without amortized principal, (incentives and compamount handled above)
                CalculateComponentAmortizedCost(calcParameters);
            }
            //reset totals based on the new calculations
            //all annuities set at comp level not input
            calcParameters.ParentTimePeriod.TotalAMOC_INT += calcParameters.ParentOperationComponent.TotalAMOC_INT;
            calcParameters.ParentTimePeriod.TotalAMAOH_INT += calcParameters.ParentOperationComponent.TotalAMAOH_INT;
            calcParameters.ParentTimePeriod.TotalAMCAP_INT += calcParameters.ParentOperationComponent.TotalAMCAP_INT;
            calcParameters.ParentTimePeriod.TotalAMOC += calcParameters.ParentOperationComponent.TotalAMOC;
            calcParameters.ParentTimePeriod.TotalAMAOH += calcParameters.ParentOperationComponent.TotalAMAOH;
            calcParameters.ParentTimePeriod.TotalAMCAP += calcParameters.ParentOperationComponent.TotalAMCAP;
            calcParameters.ParentTimePeriod.TotalAMINCENT += calcParameters.ParentOperationComponent.TotalAMINCENT;
            //total oc + aoh + cap costs
            calcParameters.ParentTimePeriod.TotalAMTOTAL += calcParameters.ParentOperationComponent.TotalAMTOTAL;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool SetInputCalculations(CalculatorParameters calcParameters, 
            Input input, ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            //all annuities handled separately, in addaes (regular) or addgrowthps (preprod) to doc
            if ((input.AnnuityType != TimePeriod.ANNUITY_TYPES.preproduction)
                && (input.AnnuityType != TimePeriod.ANNUITY_TYPES.regular))
            {
                //adjust input's prices and amounts if joint calculations had to be run
                //when ParentOperationComponent was set
                OCCalculator ocCalculator = new OCCalculator();
                ocCalculator.AdjustJointInputCalculations(
                    input, ref currentCalculationsElement, calcParameters);
                //calculate the input price
                CalculateInputPrice(input, ref calcParameters);
            }
            //Rule 1. Regular 1 year budgets; keep running sums to pass back byref to parent node
            calcParameters.ParentOperationComponent.TotalOC_INT += input.TotalOC_INT;
            calcParameters.ParentOperationComponent.TotalAOH_INT += input.TotalAOH_INT;
            calcParameters.ParentOperationComponent.TotalCAP_INT += input.TotalCAP_INT;
            calcParameters.ParentOperationComponent.TotalOC += input.TotalOC;
            calcParameters.ParentOperationComponent.TotalAOH += input.TotalAOH;
            calcParameters.ParentOperationComponent.TotalCAP += input.TotalCAP;
            calcParameters.ParentOperationComponent.TotalINCENT += input.TotalINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
        //set the parameters needed to figure interest and preproduction costs
        public void SetPreProductionCalculations(
            CalculatorParameters calcParameters, XElement currentElement)
        {
            int iCount = 0;
            bool bCommonRefPt = false;
            int iGrowthPeriods = 0;
            int iTotalPeriods = 0;
            int iPreProdPeriods = 0;
            int iProdPeriods = 0;
            bool bRetVal = false;
            DateTime dtInitEOPDate = new DateTime(1000, 12, 1);
            DateTime dtEOPDate = new DateTime(1000, 12, 1);
            DateTime dtLastEOPDate = new DateTime(1000, 12, 1);
            DateTime dtStartInitEOPDate = dtInitEOPDate;
            bool bIsSameTimePeriod = false;
            if (calcParameters.ParentBudgetInvestment.TimePeriods != null)
            {
                if (calcParameters.ParentBudgetInvestment.TimePeriods.Count > 0)
                {
                    foreach (TimePeriod tp in calcParameters.ParentBudgetInvestment.TimePeriods)
                    {
                        if (tp.Name != Constants.ROOT_PATH)
                        {
                            bIsSameTimePeriod = false;
                            bCommonRefPt = tp.IsCommonReference;
                            iGrowthPeriods = tp.GrowthPeriods;
                            if ((iGrowthPeriods < 1)) iGrowthPeriods = 1;
                            dtEOPDate = tp.Date;
                            //don't count the period if it has the same date as the previous period
                            if (dtEOPDate.Equals(dtLastEOPDate)) bIsSameTimePeriod = true;
                            //icount does not include growth periods
                            if (bIsSameTimePeriod == false)
                            {
                                iCount = iCount + 1;
                            }
                            //set the common reference point based on the first true point returned
                            if (bCommonRefPt == true)
                            {
                                //if the point has not yet been set, set it
                                if (bRetVal != true)
                                {
                                    dtInitEOPDate = dtEOPDate;
                                    //preproduction tps do not include the current year
                                    iPreProdPeriods = (iCount + iTotalPeriods) - 1;
                                    bRetVal = true;
                                }
                            }
                            if (iGrowthPeriods > 1)
                            {
                                //keep track of growth separately and keep it after preprod is calculated
                                iTotalPeriods = iTotalPeriods + iGrowthPeriods;
                            }
                            dtLastEOPDate = dtEOPDate;
                        }
                    }
                    if (dtInitEOPDate.Equals(dtStartInitEOPDate))
                    {
                        dtInitEOPDate = dtEOPDate;
                    }
                }
            }
            iProdPeriods = iCount + iTotalPeriods - iPreProdPeriods;
            calcParameters.ParentBudgetInvestment.InitEOPDate = dtInitEOPDate;
            calcParameters.ParentBudgetInvestment.PreProdPeriods = iPreProdPeriods;
            calcParameters.ParentBudgetInvestment.ProdPeriods = iProdPeriods;
        }
        private void SetPreProductionAnnuity(CalculatorParameters calcParameters)
        {
            //set time parameters needed by subsequent calculations
            double dbTotalOpAmortTotal = 0;
            //Rule1. calculate preproduction costs and add appropriate nodes to document
            if (((calcParameters.ParentTimePeriod.Date 
                == calcParameters.ParentBudgetInvestment.InitEOPDate)
                && (calcParameters.ParentBudgetInvestment.PreProdPeriods != 0)
                && (calcParameters.ParentBudgetInvestment.ProdPeriods >= 1)))
            {
                //amortize the discounted net present value of the investment
                dbTotalOpAmortTotal = ((calcParameters.ParentBudgetInvestment.TotalOC + calcParameters.ParentBudgetInvestment.TotalAOH +
                    calcParameters.ParentBudgetInvestment.TotalCAP - calcParameters.ParentBudgetInvestment.TotalAnnuities) - calcParameters.ParentBudgetInvestment.TotalR)
                    / (1 + calcParameters.ParentBudgetInvestment.Local.RealRate);
                double dbTotalIncentOpAmortTotal = ((calcParameters.ParentBudgetInvestment.TotalINCENT
                    - calcParameters.ParentBudgetInvestment.TotalAnnuities) - calcParameters.ParentBudgetInvestment.TotalRINCENT)
                    / (1 + calcParameters.ParentBudgetInvestment.Local.RealRate);
                //double dbTotalIncentOpAmortTotal = ((calcParameters.ParentBudgetInvestment.TotalINCENT 
                //    - calcParameters.ParentBudgetInvestment.TotalAnnuities) - calcParameters.ParentBudgetInvestment.TotalR) 
                //    / (1 + calcParameters.ParentBudgetInvestment.Local.RealRate);
                //amortize this sum and add appropriate nodes to the annequiv collection
                double dbPreProdCost = CalculatePreProductionAmortizedCost(
                    dbTotalOpAmortTotal, calcParameters.ParentBudgetInvestment.Local.RealRate,
                    calcParameters.ParentBudgetInvestment.ProdPeriods, calcParameters.ParentBudgetInvestment.SalvageValue);
                double dbPreProdIncentCost = CalculatePreProductionAmortizedCost(
                   dbTotalIncentOpAmortTotal, calcParameters.ParentBudgetInvestment.Local.RealRate,
                   calcParameters.ParentBudgetInvestment.ProdPeriods, calcParameters.ParentBudgetInvestment.SalvageValue);
                //double dbPreProdIncentCost = CalculatePreProductionAmortizedCost(
                //    calcParameters.ParentTimePeriod.TotalAMINCENT, calcParameters.ParentBudgetInvestment.Local.RealRate, 
                //    calcParameters.ParentBudgetInvestment.ProdPeriods, calcParameters.ParentBudgetInvestment.SalvageValue);
                string sTimePeriodNodeName 
                    = (calcParameters.SubApplicationType 
                    == Constants.SUBAPPLICATION_TYPES.budgets) 
                    ? BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString() 
                    : BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                //now add a new comp and input node that will be added to all subsequent timeperiods
                AddAnnualEquivalents1(ref calcParameters, 
                    TimePeriod.ANNUITY_TYPES.preproduction, 
                    sTimePeriodNodeName, calcParameters.ParentTimePeriod.Id, 
                    calcParameters.ParentBudgetInvestment.ProdPeriods, 
                    calcParameters.ParentTimePeriod.Date, 1, dbTotalOpAmortTotal, 
                    dbPreProdCost, dbTotalIncentOpAmortTotal, dbPreProdIncentCost);
                dbTotalOpAmortTotal = 0;
                RemoveExistingAnnualEqivalents(ref calcParameters);
            }
        }
        /// <summary>
        /// Get rid of any ae preproduction nodes (prior to new ones being added)
        /// </summary>
        private void RemoveExistingAnnualEqivalents(ref CalculatorParameters calcParameters)
        {
            if (calcParameters.ParentTimePeriod.OperationComponents != null)
            {
                if (calcParameters.ParentTimePeriod.OperationComponents
                    .Any(c => c.AnnuityType != TimePeriod.ANNUITY_TYPES.none))
                {
                    calcParameters.ParentTimePeriod.OperationComponents
                    .RemoveAll(c => c.AnnuityType != TimePeriod.ANNUITY_TYPES.none);
                }
            }
        }
        private void AddTimePeriodAnnuities(CalculatorParameters calcParameters)
        {
            //insert the annuity collection into node; let it pass through tree so that accruals are ok
            if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
            {
                bool bIsAnnuity = TimePeriod.IsAnnuity(calcParameters.ParentTimePeriod);
                if (((calcParameters.ParentBudgetInvestment.AnnEquivalents.Count > 0)
                    && (!bIsAnnuity)))
                {
                    bool bInsertAllAEs = false;
                    AddTimePeriodAnnualEquivalents(calcParameters,
                        bInsertAllAEs);
                }
            }
        }
        /// <summary>
        /// Calculate preproduction costs.
        /// Output is a new annuity node, with amortized preproduction costs, that will get added to each 
        /// subsequent time period
        /// </summary>
        private double CalculatePreProductionAmortizedCost(double amortOpTotal,
            double realRate, int prodPeriods, double salvVal)
        {
            double dbPreProductionCost = 0;
            double dbCapitalRecoveryFactor = 0;
            double dbDiscountedSalvVal = 0;

            if (prodPeriods < 1)
            {
                prodPeriods = 1;
            }

            //discount the SalvVal
            dbDiscountedSalvVal = salvVal / (System.Math.Pow((1 + realRate), prodPeriods));

            //CompPriceTotals store interest charge; also adjusted and discounted prices
            //so the amortization occurs over these adjusted numbers using a real rate
            if (prodPeriods == 1)
            {
                //preproduction costs are discounted back to the beginning of year
                //need to use discounting even in 1 year
                dbPreProductionCost = amortOpTotal;
                dbCapitalRecoveryFactor
                    = (GeneralRules.CalculateCapitalRecoveryFactor(prodPeriods, 0, realRate));
                dbPreProductionCost = (((amortOpTotal) - dbDiscountedSalvVal) 
                    * dbCapitalRecoveryFactor);
            }
            else
            {
                //note: nom interest is taken out when practice passed in
                //inflation added back in to the annuity using CalcDiscountedAnnuity in CalcTotal
                dbCapitalRecoveryFactor
                    = (GeneralRules.CalculateCapitalRecoveryFactor(prodPeriods, 0, realRate));
                dbPreProductionCost = (((amortOpTotal) - dbDiscountedSalvVal) 
                    * dbCapitalRecoveryFactor);
            }
            return dbPreProductionCost;
        }
        private void InitTimperiodTotalsForAnnuities(CalculatorParameters calcParameters)
        {
            //reset parent tps totals to zero (finished using them for growth periods)
            calcParameters.ParentTimePeriod.TotalOC_INT = 0;
            calcParameters.ParentTimePeriod.TotalAOH_INT = 0;
            calcParameters.ParentTimePeriod.TotalCAP_INT = 0;
            calcParameters.ParentTimePeriod.TotalOC = 0;
            calcParameters.ParentTimePeriod.TotalAOH = 0;
            calcParameters.ParentTimePeriod.TotalCAP = 0;
            calcParameters.ParentTimePeriod.TotalINCENT = 0;
            calcParameters.ParentTimePeriod.TotalAMOC_INT = 0;
            calcParameters.ParentTimePeriod.TotalAMAOH_INT = 0;
            calcParameters.ParentTimePeriod.TotalAMCAP_INT = 0;
            calcParameters.ParentTimePeriod.TotalAMOC = 0;
            calcParameters.ParentTimePeriod.TotalAMAOH = 0;
            calcParameters.ParentTimePeriod.TotalAMCAP = 0;
            calcParameters.ParentTimePeriod.TotalAMINCENT = 0;
        }
        //add the calculated annuities to this timeperiod's totals
        private void AddAnnuitiesToTimePeriodTotals(CalculatorParameters calcParameters)
        {
            //the Outcomes collection holds the calculated 
            //(i.e. discounted) annuities (the AnnEquivs holds the non calculated ones(
            if (calcParameters.ParentTimePeriod.Outcomes != null)
            {
                if (calcParameters.ParentTimePeriod.Outcomes.Count > 0)
                {
                    foreach (Outcome outcomeAnnuity
                        in calcParameters.ParentTimePeriod.Outcomes)
                    {
                        //reset totals based on the new calculations
                        //all annuities set at outcome level not output
                        calcParameters.ParentTimePeriod.TotalR_INT += outcomeAnnuity.TotalR_INT;
                        calcParameters.ParentTimePeriod.TotalR += outcomeAnnuity.TotalR;
                        calcParameters.ParentTimePeriod.TotalRINCENT += outcomeAnnuity.TotalRINCENT;
                        calcParameters.ParentTimePeriod.TotalAMR_INT += outcomeAnnuity.TotalAMR_INT;
                        calcParameters.ParentTimePeriod.TotalAMR += outcomeAnnuity.TotalAMR;
                        calcParameters.ParentTimePeriod.TotalAMRINCENT += outcomeAnnuity.TotalAMRINCENT;
                    }
                }
            }
            //the OperationComponents collection holds the calculated 
            //(i.e. discounted) annuities (the AnnEquivs holds the non calculated ones(
            if (calcParameters.ParentTimePeriod.OperationComponents != null)
            {
                if (calcParameters.ParentTimePeriod.OperationComponents.Count > 0)
                {
                    foreach (OperationComponent opCompAnnuity
                        in calcParameters.ParentTimePeriod.OperationComponents)
                    {
                        //reset totals based on the new calculations
                        //all annuities set at comp level not input
                        calcParameters.ParentTimePeriod.TotalOC_INT += opCompAnnuity.TotalOC_INT;
                        calcParameters.ParentTimePeriod.TotalAOH_INT += opCompAnnuity.TotalAOH_INT;
                        calcParameters.ParentTimePeriod.TotalCAP_INT += opCompAnnuity.TotalCAP_INT;
                        calcParameters.ParentTimePeriod.TotalOC += opCompAnnuity.TotalOC;
                        calcParameters.ParentTimePeriod.TotalAOH += opCompAnnuity.TotalAOH;
                        calcParameters.ParentTimePeriod.TotalCAP += opCompAnnuity.TotalCAP;
                        calcParameters.ParentTimePeriod.TotalINCENT += opCompAnnuity.TotalINCENT;
                        calcParameters.ParentTimePeriod.TotalAMOC_INT += opCompAnnuity.TotalAMOC_INT;
                        calcParameters.ParentTimePeriod.TotalAMAOH_INT += opCompAnnuity.TotalAMAOH_INT;
                        calcParameters.ParentTimePeriod.TotalAMCAP_INT += opCompAnnuity.TotalAMCAP_INT;
                        calcParameters.ParentTimePeriod.TotalAMOC += opCompAnnuity.TotalAMOC;
                        calcParameters.ParentTimePeriod.TotalAMAOH += opCompAnnuity.TotalAMAOH;
                        calcParameters.ParentTimePeriod.TotalAMCAP += opCompAnnuity.TotalAMCAP;
                        calcParameters.ParentTimePeriod.TotalAMINCENT += opCompAnnuity.TotalAMINCENT;
                    }
                }
            }
        }
        //add the calculated annuities to this timeperiod's totals
        private void AddAnnuitiesToBudgetTotals(CalculatorParameters calcParameters)
        {
            //all annuities set at comp/outcome level not input or output
            calcParameters.ParentBudgetInvestment.TotalR_INT += calcParameters.ParentTimePeriod.TotalR_INT;
            calcParameters.ParentBudgetInvestment.TotalR += calcParameters.ParentTimePeriod.TotalR;
            calcParameters.ParentBudgetInvestment.TotalRINCENT += calcParameters.ParentTimePeriod.TotalRINCENT;
            calcParameters.ParentBudgetInvestment.TotalAMR += calcParameters.ParentTimePeriod.TotalAMR;
            calcParameters.ParentBudgetInvestment.TotalAMRINCENT += calcParameters.ParentTimePeriod.TotalAMRINCENT;

            calcParameters.ParentBudgetInvestment.TotalOC_INT += calcParameters.ParentTimePeriod.TotalOC_INT;
            calcParameters.ParentBudgetInvestment.TotalAOH_INT += calcParameters.ParentTimePeriod.TotalAOH_INT;
            calcParameters.ParentBudgetInvestment.TotalCAP_INT += calcParameters.ParentTimePeriod.TotalCAP_INT;
            calcParameters.ParentBudgetInvestment.TotalOC += calcParameters.ParentTimePeriod.TotalOC;
            calcParameters.ParentBudgetInvestment.TotalAOH += calcParameters.ParentTimePeriod.TotalAOH;
            calcParameters.ParentBudgetInvestment.TotalCAP += calcParameters.ParentTimePeriod.TotalCAP;
            calcParameters.ParentBudgetInvestment.TotalINCENT += calcParameters.ParentTimePeriod.TotalINCENT;
            calcParameters.ParentBudgetInvestment.TotalAMOC_INT += calcParameters.ParentTimePeriod.TotalAMOC_INT;
            calcParameters.ParentBudgetInvestment.TotalAMAOH_INT += calcParameters.ParentTimePeriod.TotalAMAOH_INT;
            calcParameters.ParentBudgetInvestment.TotalAMCAP_INT += calcParameters.ParentTimePeriod.TotalAMCAP_INT;
            calcParameters.ParentBudgetInvestment.TotalAMOC += calcParameters.ParentTimePeriod.TotalAMOC;
            calcParameters.ParentBudgetInvestment.TotalAMAOH += calcParameters.ParentTimePeriod.TotalAMAOH;
            calcParameters.ParentBudgetInvestment.TotalAMCAP += calcParameters.ParentTimePeriod.TotalAMCAP;
            calcParameters.ParentBudgetInvestment.TotalAMINCENT += calcParameters.ParentTimePeriod.TotalAMINCENT;
            //ce analysis (cost per unit labor, cost per unit output ...)
            calcParameters.ParentBudgetInvestment.TotalRAmount += calcParameters.ParentTimePeriod.TotalRAmount;
            calcParameters.ParentBudgetInvestment.TotalRCompositionAmount += calcParameters.ParentTimePeriod.TotalRCompositionAmount;
            calcParameters.ParentBudgetInvestment.TotalRPrice += calcParameters.ParentTimePeriod.TotalRPrice;
            calcParameters.ParentBudgetInvestment.TotalRName = calcParameters.ParentTimePeriod.TotalRName;
            calcParameters.ParentBudgetInvestment.TotalRUnit = calcParameters.ParentTimePeriod.TotalRUnit;
            calcParameters.ParentBudgetInvestment.TotalRCompositionUnit = calcParameters.ParentTimePeriod.TotalRCompositionUnit;

            calcParameters.ParentBudgetInvestment.TotalAMTOTAL
                += calcParameters.ParentBudgetInvestment.TotalAMOC + calcParameters.ParentBudgetInvestment.TotalAMAOH + calcParameters.ParentBudgetInvestment.TotalAMCAP;
            calcParameters.ParentBudgetInvestment.TotalAMNET
                += calcParameters.ParentBudgetInvestment.TotalAMR - calcParameters.ParentBudgetInvestment.TotalAMTOTAL;
            calcParameters.ParentBudgetInvestment.TotalAMINCENT_NET
               += calcParameters.ParentBudgetInvestment.TotalAMRINCENT - calcParameters.ParentBudgetInvestment.TotalAMINCENT;
        }
        private double GetAmount(CalculatorParameters calcParameters,
            OperationComponent operationOrComponent)
        {
            double dbAmount = 0;
            dbAmount = operationOrComponent.Amount;
            //regular annuities calculated net of incentives, so don't adjust
            if ((operationOrComponent.AnnuityType == TimePeriod.ANNUITY_TYPES.regular)
                || (operationOrComponent.AnnuityType == TimePeriod.ANNUITY_TYPES.preproduction))
            {
                //handled when first amortized
                dbAmount = 1;
            }
            return dbAmount;
        }
        private double GetAmount(CalculatorParameters calcParameters,
            Outcome outcome)
        {
            double dbAmount = 0;
            dbAmount = outcome.Amount;
            //regular annuities calculated net of incentives, so don't adjust
            if ((outcome.AnnuityType == TimePeriod.ANNUITY_TYPES.regular)
                || (outcome.AnnuityType == TimePeriod.ANNUITY_TYPES.preproduction))
            {
                //handled when first amortized
                dbAmount = 1;
            }
            return dbAmount;
        }
        public void CalculateBudgetInvestment(
            ref CalculatorParameters calcParameters)
        {
            //cumulative regular annuities that will be removed from preproduction expenses
            calcParameters.ParentBudgetInvestmentGroup.TotalAnnuities = 0;
            //cumulative totals
            calcParameters.ParentBudgetInvestmentGroup.TotalOC += calcParameters.ParentBudgetInvestment.TotalOC;
            calcParameters.ParentBudgetInvestmentGroup.TotalAOH += calcParameters.ParentBudgetInvestment.TotalAOH;
            calcParameters.ParentBudgetInvestmentGroup.TotalCAP += calcParameters.ParentBudgetInvestment.TotalCAP;
            calcParameters.ParentBudgetInvestmentGroup.TotalINCENT += calcParameters.ParentBudgetInvestment.TotalINCENT;
            calcParameters.ParentBudgetInvestmentGroup.TotalR += calcParameters.ParentBudgetInvestment.TotalR;
            calcParameters.ParentBudgetInvestmentGroup.TotalRINCENT += calcParameters.ParentBudgetInvestment.TotalRINCENT;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMR += calcParameters.ParentBudgetInvestment.TotalAMR;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMRINCENT += calcParameters.ParentBudgetInvestment.TotalAMRINCENT;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMOC += calcParameters.ParentBudgetInvestment.TotalAMOC;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMAOH += calcParameters.ParentBudgetInvestment.TotalAMAOH;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMCAP += calcParameters.ParentBudgetInvestment.TotalAMCAP;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMAOH_NET += calcParameters.ParentBudgetInvestment.TotalAMAOH_NET;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMINCENT += calcParameters.ParentBudgetInvestment.TotalAMINCENT;

            calcParameters.ParentBudgetInvestmentGroup.TotalRAmount += calcParameters.ParentBudgetInvestment.TotalRAmount;
            calcParameters.ParentBudgetInvestmentGroup.TotalRCompositionAmount += calcParameters.ParentBudgetInvestment.TotalRCompositionAmount;
            calcParameters.ParentBudgetInvestmentGroup.TotalRPrice += calcParameters.ParentBudgetInvestment.TotalRPrice;
            calcParameters.ParentBudgetInvestmentGroup.TotalRName = calcParameters.ParentBudgetInvestment.TotalRName;
            calcParameters.ParentBudgetInvestmentGroup.TotalRUnit = calcParameters.ParentBudgetInvestment.TotalRUnit;
            calcParameters.ParentBudgetInvestmentGroup.TotalRCompositionUnit = calcParameters.ParentBudgetInvestment.TotalRCompositionUnit;

            double dbNetTotal = calcParameters.ParentBudgetInvestment.TotalAMR - calcParameters.ParentBudgetInvestment.TotalAMOC;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMOC_NET += dbNetTotal;
            dbNetTotal -= calcParameters.ParentBudgetInvestment.TotalAMAOH;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMAOH_NET += dbNetTotal;
            //calcParameters.ParentBudgetInvestmentGroup.TotalAMAOH_NET2 += dbNetTotal;
            dbNetTotal -= calcParameters.ParentBudgetInvestment.TotalAMCAP;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMCAP_NET += dbNetTotal;
            //set the real equivalent annual annuity equation 10.13
            double dbEAA = 0;
            if ((calcParameters.ParentBudgetInvestment.PreProdPeriods + calcParameters.ParentBudgetInvestment.ProdPeriods) > 1)
            {
                dbEAA = GeneralRules.CalculateEquivalentAnnualAnnuity((calcParameters.ParentBudgetInvestment.TotalR
                        - calcParameters.ParentBudgetInvestment.TotalOC - calcParameters.ParentBudgetInvestment.TotalAOH - calcParameters.ParentBudgetInvestment.TotalCAP),
                    (calcParameters.ParentBudgetInvestment.PreProdPeriods + calcParameters.ParentBudgetInvestment.ProdPeriods),
                    calcParameters.ParentBudgetInvestment.Local.RealRate, 0);
            }
            else
            {
                dbEAA = calcParameters.ParentBudgetInvestment.TotalR - calcParameters.ParentBudgetInvestment.TotalOC - calcParameters.ParentBudgetInvestment.TotalAOH
                    - calcParameters.ParentBudgetInvestment.TotalCAP;
            }
            calcParameters.ParentBudgetInvestment.InvestmentEAA = dbEAA;

            //set nets
            calcParameters.ParentBudgetInvestmentGroup.TotalAMTOTAL
                = calcParameters.ParentBudgetInvestmentGroup.TotalAMOC + calcParameters.ParentBudgetInvestmentGroup.TotalAMAOH + calcParameters.ParentBudgetInvestmentGroup.TotalAMCAP;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMNET = calcParameters.ParentBudgetInvestmentGroup.TotalAMR - calcParameters.ParentBudgetInvestmentGroup.TotalAMTOTAL;
            calcParameters.ParentBudgetInvestmentGroup.TotalAMINCENT_NET = calcParameters.ParentBudgetInvestmentGroup.TotalAMRINCENT - calcParameters.ParentBudgetInvestmentGroup.TotalAMINCENT;
        }
        public void CalculateTimePeriod(
            ref CalculatorParameters calcParameters)
        {
            //set attributes of this time period
            calcParameters.ParentTimePeriod.TotalOC = calcParameters.ParentTimePeriod.TotalOC * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAOH = calcParameters.ParentTimePeriod.TotalAOH * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalCAP = calcParameters.ParentTimePeriod.TotalCAP * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalR = calcParameters.ParentTimePeriod.TotalR * calcParameters.ParentTimePeriod.Amount;
            //set running sum for incentive variable
            calcParameters.ParentTimePeriod.TotalINCENT = calcParameters.ParentTimePeriod.TotalINCENT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalRINCENT = calcParameters.ParentTimePeriod.TotalRINCENT * calcParameters.ParentTimePeriod.Amount;
            //set the sum at this node for incentive variable, it will be based on cumulative incentive-adjusted totals
            double dbIncentiveAdjustedSum = 0;
            CalculateIncentiveForTimePeriod(calcParameters.ParentTimePeriod, calcParameters,
                ref dbIncentiveAdjustedSum);

            calcParameters.ParentTimePeriod.TotalOC_INT = calcParameters.ParentTimePeriod.TotalOC_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAOH_INT = calcParameters.ParentTimePeriod.TotalAOH_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalCAP_INT = calcParameters.ParentTimePeriod.TotalCAP_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalR_INT = calcParameters.ParentTimePeriod.TotalR_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMR = calcParameters.ParentTimePeriod.TotalAMR * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMR_INT = calcParameters.ParentTimePeriod.TotalAMR_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMRINCENT = calcParameters.ParentTimePeriod.TotalAMRINCENT * calcParameters.ParentTimePeriod.Amount;
            //keep it simple and clear to end users
            calcParameters.ParentTimePeriod.TotalAMOC = calcParameters.ParentTimePeriod.TotalAMOC * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMAOH = calcParameters.ParentTimePeriod.TotalAMAOH * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMCAP = calcParameters.ParentTimePeriod.TotalAMCAP * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMINCENT = calcParameters.ParentTimePeriod.TotalAMINCENT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMOC_INT = calcParameters.ParentTimePeriod.TotalAMOC_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMAOH_INT = calcParameters.ParentTimePeriod.TotalAMAOH_INT * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalAMCAP_INT = calcParameters.ParentTimePeriod.TotalAMCAP_INT * calcParameters.ParentTimePeriod.Amount;

            //don't multiply name, units, or prices
            calcParameters.ParentTimePeriod.TotalRAmount = calcParameters.ParentTimePeriod.TotalRAmount * calcParameters.ParentTimePeriod.Amount;
            calcParameters.ParentTimePeriod.TotalRCompositionAmount = calcParameters.ParentTimePeriod.TotalRCompositionAmount * calcParameters.ParentTimePeriod.Amount;
            

            //160 refactored totals and nets for time period
            calcParameters.ParentTimePeriod.TotalAMOC_NET =
                   calcParameters.ParentTimePeriod.TotalAMR - calcParameters.ParentTimePeriod.TotalAMOC;
            calcParameters.ParentTimePeriod.TotalAMAOH_NET =
                calcParameters.ParentTimePeriod.TotalAMOC_NET - calcParameters.ParentTimePeriod.TotalAMAOH;
            calcParameters.ParentTimePeriod.TotalAMCAP_NET =
                calcParameters.ParentTimePeriod.TotalAMAOH_NET - calcParameters.ParentTimePeriod.TotalAMCAP;
            calcParameters.ParentTimePeriod.TotalAMTOTAL
               = calcParameters.ParentTimePeriod.TotalAMOC + calcParameters.ParentTimePeriod.TotalAMAOH + calcParameters.ParentTimePeriod.TotalAMCAP;
            calcParameters.ParentTimePeriod.TotalAMNET = calcParameters.ParentTimePeriod.TotalAMR - calcParameters.ParentTimePeriod.TotalAMTOTAL;
            calcParameters.ParentTimePeriod.TotalAMINCENT_NET = calcParameters.ParentTimePeriod.TotalAMRINCENT - calcParameters.ParentTimePeriod.TotalAMINCENT;
        
        }
        //set rules, interest charges, and price adjustments for operations and components
        public void CalculateOperationComponentPrice(
            ref CalculatorParameters calcParameters)
        {
            //variables needed for calculations
            double dbTempSum = 0;
            double dbAmount = GetAmount(calcParameters, 
                calcParameters.ParentOperationComponent);
            //Rule 1. regular sums; discounted at input level
            //same attributes in investments and budgets
            dbTempSum = ((calcParameters.ParentOperationComponent.TotalOC_INT + calcParameters.ParentOperationComponent.TotalOC) * dbAmount);
            calcParameters.ParentOperationComponent.TotalOC = dbTempSum;
            //need to display full interest
            calcParameters.ParentOperationComponent.TotalOC_INT = calcParameters.ParentOperationComponent.TotalOC_INT * dbAmount;
            dbTempSum = ((calcParameters.ParentOperationComponent.TotalAOH_INT + calcParameters.ParentOperationComponent.TotalAOH) * dbAmount);
            calcParameters.ParentOperationComponent.TotalAOH = dbTempSum;
            //need to display full interest
            calcParameters.ParentOperationComponent.TotalAOH_INT = calcParameters.ParentOperationComponent.TotalAOH_INT * dbAmount;
            dbTempSum = ((calcParameters.ParentOperationComponent.TotalCAP_INT + calcParameters.ParentOperationComponent.TotalCAP) * dbAmount);
            calcParameters.ParentOperationComponent.TotalCAP = dbTempSum;
            //need to display full interest
            calcParameters.ParentOperationComponent.TotalCAP_INT = calcParameters.ParentOperationComponent.TotalCAP_INT * dbAmount;
            //set running sum for incentive variable
            calcParameters.ParentOperationComponent.TotalINCENT = calcParameters.ParentOperationComponent.TotalINCENT * dbAmount;
            //set the sum at this node for incentive variable, it will be based on cumulative incentive-adjusted totals
            double dbIncentiveAdjustedSum = 0;
            CalculateIncentiveForComponentOperation(calcParameters.ParentOperationComponent,
                calcParameters, ref dbIncentiveAdjustedSum);
        }
        /// <summary>
        /// Amortizes each type of cost separately. Makes display of cost data more understandable.
        /// Adds this annuity as a line item to future time periods.
        /// Reference year is used to adjust salvage values.
        /// </summary>
        public void CalculateComponentAmortizedCost(CalculatorParameters calcParameters)
        {
            double dbSalvVal = 0;
            double dbCRF = 0;
            double dbOCCompAmortCost = 0;
            double dbAOHCompAmortCost = 0;
            double dbCAPCompAmortCost = 0;
            double dbIncentCompAmortCost = 0;
            double dbOCTotal = 0;
            double dbAOHTotal = 0;
            double dbCAPTotal = 0;
            double dbIncentTotal = 0;
            double dbLastTimeDays = 0;
            double dbRemovePrincipal = 0;

            //Rule1. Show regular totals in annual column for 1 year budgets
            if ((calcParameters.ParentOperationComponent.EffectiveLife == 1) 
                && (calcParameters.ParentBudgetInvestment.PreProdPeriods == 0))
            {
                //remember, these are discounted costs, interest has to be added back in to get original
                calcParameters.ParentOperationComponent.TotalAMOC = calcParameters.ParentOperationComponent.TotalOC;
                calcParameters.ParentOperationComponent.TotalAMAOH = calcParameters.ParentOperationComponent.TotalAOH;
                calcParameters.ParentOperationComponent.TotalAMCAP = calcParameters.ParentOperationComponent.TotalCAP;
                calcParameters.ParentOperationComponent.TotalAMOC_INT = calcParameters.ParentOperationComponent.TotalOC_INT;
                calcParameters.ParentOperationComponent.TotalAMAOH_INT = calcParameters.ParentOperationComponent.TotalAOH_INT;
                calcParameters.ParentOperationComponent.TotalAMCAP_INT = calcParameters.ParentOperationComponent.TotalCAP_INT;
                //incentives
                calcParameters.ParentOperationComponent.TotalAMINCENT = calcParameters.ParentOperationComponent.TotalINCENT;
            }
            else if ((calcParameters.ParentOperationComponent.EffectiveLife == 1) 
                && (calcParameters.ParentBudgetInvestment.PreProdPeriods != 0))
            {
                //Rule2. don't show preproduction costs prior to common ref point, but include totals column in ann column after
                if ((calcParameters.ParentBudgetInvestment.ProdPeriods >= 1) 
                    && (calcParameters.ParentTimePeriod.Date < calcParameters.ParentBudgetInvestment.InitEOPDate))
                {
                    calcParameters.ParentOperationComponent.TotalAMOC = 0;
                    calcParameters.ParentOperationComponent.TotalAMAOH = 0;
                    calcParameters.ParentOperationComponent.TotalAMCAP = 0;
                    calcParameters.ParentOperationComponent.TotalAMOC_INT = 0;
                    calcParameters.ParentOperationComponent.TotalAMAOH_INT = 0;
                    calcParameters.ParentOperationComponent.TotalAMCAP_INT = 0;
                    //incentives
                    calcParameters.ParentOperationComponent.TotalAMINCENT = 0;
                }
                else
                {
                    //and preproduction component annuity is already in the collection
                    calcParameters.ParentOperationComponent.TotalAMOC = calcParameters.ParentOperationComponent.TotalOC;
                    calcParameters.ParentOperationComponent.TotalAMAOH = calcParameters.ParentOperationComponent.TotalAOH;
                    calcParameters.ParentOperationComponent.TotalAMCAP = calcParameters.ParentOperationComponent.TotalCAP;
                    calcParameters.ParentOperationComponent.TotalAMOC_INT = calcParameters.ParentOperationComponent.TotalOC_INT;
                    calcParameters.ParentOperationComponent.TotalAMAOH_INT = calcParameters.ParentOperationComponent.TotalAOH_INT;
                    calcParameters.ParentOperationComponent.TotalAMCAP_INT = calcParameters.ParentOperationComponent.TotalCAP_INT;
                    //incentives
                    calcParameters.ParentOperationComponent.TotalAMINCENT = calcParameters.ParentOperationComponent.TotalINCENT;
                }
            }
            else if ((calcParameters.ParentOperationComponent.EffectiveLife != 1))
            {
                if (calcParameters.ParentOperationComponent.EffectiveLife < 1)
                {
                    dbCRF = GeneralRules.CalculateFractionalPaymentFactor(
                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                        calcParameters.ParentOperationComponent.EffectiveLife,
                        CalculatorHelpers.ConvertStringToInt(calcParameters.ParentOperationComponent.EffectiveLife.ToString()), 
                        DOUBLE_YEAR, ref dbLastTimeDays);
                }
                else
                {
                    dbCRF = GeneralRules.CalculateCapitalRecoveryFactor(
                        calcParameters.ParentOperationComponent.EffectiveLife, 0, 
                        calcParameters.ParentBudgetInvestment.Local.RealRate);
                }
                //amortize and set each cost
                if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
                {
                    dbSalvVal = calcParameters.ParentOperationComponent.SalvageValue / 
                        (System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate), 
                        calcParameters.ParentOperationComponent.EffectiveLife));
                    dbOCTotal = calcParameters.ParentOperationComponent.TotalOC;
                    dbOCCompAmortCost = (dbOCTotal * dbCRF);

                    dbAOHTotal = calcParameters.ParentOperationComponent.TotalAOH;
                    //always take the salvage value out of aoh only
                    dbAOHCompAmortCost = ((dbAOHTotal - dbSalvVal) * dbCRF);

                    dbCAPTotal = calcParameters.ParentOperationComponent.TotalCAP;
                    dbCAPCompAmortCost = (dbCAPTotal * dbCRF);
                    //incentives handled separately
                    dbIncentTotal = calcParameters.ParentOperationComponent.TotalINCENT;
                    if (dbIncentTotal != 0)
                    {
                        //different than AOH because no subsequent periods added to finish off incentive
                        dbIncentCompAmortCost = ((dbIncentTotal - dbSalvVal) * dbCRF);
                    }
                }
                else
                {
                    //added for operation/component price application
                    dbSalvVal = calcParameters.ParentOperationComponent.SalvageValue / 
                        (System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate), 
                        calcParameters.ParentOperationComponent.EffectiveLife));
                    dbOCTotal = calcParameters.ParentOperationComponent.TotalOC;
                    dbAOHTotal = calcParameters.ParentOperationComponent.TotalAOH;
                    dbCAPTotal = calcParameters.ParentOperationComponent.TotalCAP;
                    dbAOHCompAmortCost = (((dbOCTotal + dbAOHTotal + dbCAPTotal - dbSalvVal) 
                        * dbCRF));
                    dbIncentTotal = calcParameters.ParentOperationComponent.TotalINCENT;
                    if (dbIncentTotal != 0)
                    {
                        dbIncentCompAmortCost = ((dbIncentTotal - dbSalvVal) * dbCRF);
                    }
                }
                calcParameters.ParentOperationComponent.TotalAMOC = dbOCCompAmortCost;
                calcParameters.ParentOperationComponent.TotalAMAOH = dbAOHCompAmortCost;
                calcParameters.ParentOperationComponent.TotalAMCAP = dbCAPCompAmortCost;
                calcParameters.ParentOperationComponent.TotalAMOC_INT = 0;
                calcParameters.ParentOperationComponent.TotalAMAOH_INT = 0;
                calcParameters.ParentOperationComponent.TotalAMCAP_INT = 0;
                //Rule3. Always show annuities in annual column of any budget 
                //or investment and handle consistently in time periods thereafter
                if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets
                    || calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    //the first year discounted real interest will actually show up in next time period
                    //that allows the correct profit to be shown in the year the annuity ocdbs
                    //see the dairy cow replacement examples
                    //note annuities will only be accrued in the aoh cost section
                    if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
                    {
                        AddAnnualEquivalents1(ref calcParameters,
                            TimePeriod.ANNUITY_TYPES.regular, calcParameters.CurrentElementNodeName,
                            calcParameters.ParentTimePeriod.Id, calcParameters.ParentBudgetInvestment.ProdPeriods,
                            calcParameters.ParentTimePeriod.Date, 1, (dbOCTotal + dbAOHTotal + dbCAPTotal),
                            (dbOCCompAmortCost + dbAOHCompAmortCost + dbCAPCompAmortCost),
                            dbIncentTotal, dbIncentCompAmortCost);
                    }
                    //now remove this principal from preproduction annuity total
                    if (calcParameters.ParentBudgetInvestment.PreProdPeriods != 0)
                    {
                        dbRemovePrincipal = (dbOCTotal + dbAOHTotal + dbCAPTotal);
                        //regular annuities are not included in the sum of the preproduction costs
                        RemoveAnnualEquivalents(dbRemovePrincipal, calcParameters);
                    }
                }
                else
                {
                }
                //incentives
                calcParameters.ParentOperationComponent.TotalAMINCENT = dbIncentCompAmortCost;
            }
            //totals
            calcParameters.ParentOperationComponent.TotalAMTOTAL
                = calcParameters.ParentOperationComponent.TotalAMOC + calcParameters.ParentOperationComponent.TotalAMAOH + calcParameters.ParentOperationComponent.TotalAMCAP;
        }
        //set rules, interest charges, and price adjustments for outcomes
        public void CalculateOutcomePrice(
            ref CalculatorParameters calcParameters)
        {
            //variables needed for calculations
            double dbAmount = GetAmount(calcParameters,
                calcParameters.ParentOutcome);
            //Rule 1. regular sums; discounted at output level
            //totals handled same as inputs (needs to be added here)
            //same attributes in investments and budgets
            calcParameters.ParentOutcome.TotalR = (calcParameters.ParentOutcome.TotalR + calcParameters.ParentOutcome.TotalR_INT) * dbAmount;
            //at this state the totalr matches toc... calcs
            //need to display full interest
            calcParameters.ParentOutcome.TotalR_INT = calcParameters.ParentOutcome.TotalR_INT * dbAmount;
            //set running sum for incentive variable
            calcParameters.ParentOutcome.TotalRINCENT = calcParameters.ParentOutcome.TotalRINCENT * dbAmount;
            //set the sum at this node for incentive variable, it will be based on cumulative incentive-adjusted totals
            double dbIncentiveAdjustedSum = 0;
            CalculateIncentiveForOutcome(calcParameters.ParentOutcome,
                calcParameters, ref dbIncentiveAdjustedSum);
        }

        /// <summary>
        /// Amortizes each type of beneift separately. Makes display of benefit data more understandable.
        /// Adds this annuity as a line item to future time periods.
        /// Reference year is used to adjust salvage values.
        /// </summary>
        public void CalculateOutcomeAmortizedBen(CalculatorParameters calcParameters)
        {
            double dbSalvVal = 0;
            double dbCRF = 0;
            double dbROutcomeAmortBen = 0;
            double dbIncentOutcomeAmortBen = 0;
            double dbRTotal = 0;
            double dbIncentTotal = 0;
            double dbLastTimeDays = 0;
            double dbRemovePrincipal = 0;

            //Rule1. Show regular totals in annual column for 1 year budgets
            if ((calcParameters.ParentOutcome.EffectiveLife == 1)
                && (calcParameters.ParentBudgetInvestment.PreProdPeriods == 0))
            {
                //remember, these are discounted costs, interest has to be added back in to get original
                calcParameters.ParentOutcome.TotalAMR = calcParameters.ParentOutcome.TotalR;
                calcParameters.ParentOutcome.TotalAMR_INT = calcParameters.ParentOutcome.TotalR_INT;
                //incentives
                calcParameters.ParentOutcome.TotalAMRINCENT = calcParameters.ParentOutcome.TotalRINCENT;
            }
            else if ((calcParameters.ParentOutcome.EffectiveLife == 1)
                && (calcParameters.ParentBudgetInvestment.PreProdPeriods != 0))
            {
                //Rule2. don't show preproduction costs prior to common ref point, but include totals column in ann column after
                if ((calcParameters.ParentBudgetInvestment.ProdPeriods >= 1)
                    && (calcParameters.ParentTimePeriod.Date < calcParameters.ParentBudgetInvestment.InitEOPDate))
                {
                    calcParameters.ParentOutcome.TotalAMR = 0;
                    calcParameters.ParentOutcome.TotalAMR_INT = 0;
                    //incentives
                    calcParameters.ParentOutcome.TotalAMRINCENT = 0;
                }
                else
                {
                    //and preproduction net annuity is already in the opcomp collection
                    calcParameters.ParentOutcome.TotalAMR = calcParameters.ParentOutcome.TotalR;
                    //incentives
                    calcParameters.ParentOutcome.TotalAMRINCENT = calcParameters.ParentOutcome.TotalRINCENT;
                }
            }
            else if ((calcParameters.ParentOutcome.EffectiveLife != 1))
            {
                if (calcParameters.ParentOutcome.EffectiveLife < 1)
                {
                    dbCRF = GeneralRules.CalculateFractionalPaymentFactor(
                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                        calcParameters.ParentOutcome.EffectiveLife,
                        CalculatorHelpers.ConvertStringToInt(calcParameters.ParentOutcome.EffectiveLife.ToString()),
                        DOUBLE_YEAR, ref dbLastTimeDays);
                }
                else
                {
                    dbCRF = GeneralRules.CalculateCapitalRecoveryFactor(
                        calcParameters.ParentOutcome.EffectiveLife, 0,
                        calcParameters.ParentBudgetInvestment.Local.RealRate);
                }
                //amortize and set each cost
                if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
                {
                    dbSalvVal = calcParameters.ParentOutcome.SalvageValue /
                        (System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate),
                        calcParameters.ParentOutcome.EffectiveLife));
                    dbRTotal = calcParameters.ParentOutcome.TotalR;
                    dbROutcomeAmortBen = ((dbRTotal - dbSalvVal) * dbCRF);
                    //incentives handled separately
                    dbIncentTotal = calcParameters.ParentOutcome.TotalRINCENT;
                    if (dbIncentTotal != 0)
                    {
                        //different than AOH because no subsequent periods added to finish off incentive
                        dbIncentOutcomeAmortBen = ((dbIncentTotal - dbSalvVal) * dbCRF);
                    }
                }
                else
                {
                    //added for operation/component price application
                    dbSalvVal = calcParameters.ParentOutcome.SalvageValue /
                        (System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate),
                        calcParameters.ParentOutcome.EffectiveLife));
                    dbRTotal = calcParameters.ParentOutcome.TotalR;
                    dbROutcomeAmortBen = (((dbRTotal - dbSalvVal) * dbCRF));
                    dbIncentTotal = calcParameters.ParentOutcome.TotalRINCENT;
                    if (dbIncentTotal != 0)
                    {
                        dbIncentOutcomeAmortBen = ((dbIncentTotal - dbSalvVal) * dbCRF);
                    }
                }
                calcParameters.ParentOutcome.TotalAMR = dbROutcomeAmortBen;
                calcParameters.ParentOutcome.TotalAMR_INT = 0;
                //Rule3. Always show annuities in annual column of any budget 
                //or investment and handle consistently in time periods thereafter
                if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets
                    || calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
                {
                    //the first year discounted real interest will actually show up in next time period
                    //that allows the correct profit to be shown in the year the annuity ocdbs
                    if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
                    {
                        AddAnnualEquivalents1(ref calcParameters,
                            TimePeriod.ANNUITY_TYPES.regular, calcParameters.CurrentElementNodeName,
                            calcParameters.ParentTimePeriod.Id, calcParameters.ParentBudgetInvestment.ProdPeriods,
                            calcParameters.ParentTimePeriod.Date, 1, dbRTotal, dbROutcomeAmortBen,
                            dbIncentTotal, dbIncentOutcomeAmortBen);
                    }
                    //now remove this principal from preproduction annuity total
                    if (calcParameters.ParentBudgetInvestment.PreProdPeriods != 0)
                    {
                        dbRemovePrincipal = dbRTotal;
                        //regular annuities are not included in the sum of the preproduction costs
                        RemoveAnnualEquivalents(dbRemovePrincipal, calcParameters);
                    }
                }
                //incentives
                calcParameters.ParentOutcome.TotalAMRINCENT = dbIncentOutcomeAmortBen;
            }
        }
        /// <summary>
        /// Remove annual equivalents from budget and capital investment documents.
        /// Action is taken after the sum of their discounted cash flows equals their starting principal
        /// Purpose is to keep the totals and annuals columns in synchronization.
        /// </summary>
        public void RemoveAnnualEquivalents(double total, CalculatorParameters calcParameters)
        {
            //this sum is subtracted from preproduction expenses, 
            //so it needs to be a positive number
            if (total < 0) total = total * -1;
            calcParameters.ParentBudgetInvestment.TotalAnnuities += total;
        }
        
        //set rules, interest charges, and price adjustments for inputs
        public double CalculateInputPrice(
            Input input, ref CalculatorParameters calcParameters)
        {
            double dbCalcInputPrice = 0;
            System.TimeSpan spanDays;
            double dbSpanDays = 0;
            double dbDaysToEOP = 0;
            double dbNomRate = calcParameters.ParentBudgetInvestment.Local.NominalRate;
            double dbNomDiscountFactor = 0;
            double dbDiscountFactor = 0;
            double dbNomMonths = 0;
            double dbRealYrs = 0;
            double dbTimePeriods = 0;
            double dbRegionMult = 1;
            double dbEnterpriseAmount = 0;
            double dbEntPractAOHFactor = 0;
            double dbInputPrice = 0;
            bool bDiscountYorN = true;
            //will calculate interest based on operation/component date (end of period date)
            DateTime dtEOPDate = calcParameters.ParentOperationComponent.Date;
            DateTime dtInputDate;
            double dbDaysFromCRPToEOP = 0;
            double dbDaysPerPeriod = 0;
            double dbRealInterestDays = 0;
            double dbInterest = 0;
            double dbInputandInterestTotal = 0;
            double dbTimes = 0;
            double dbValue = 0;
            double dbOCAmount = 0;
            double dbAOHAmount = 0;
            double dbCAPAmount = 0;
            bool bUseAOH = false;

            //initialize variables
            dbInterest = 0;
            if (calcParameters.ParentTimePeriod != null)
            {
                bDiscountYorN = calcParameters.ParentTimePeriod.IsDiscounted;
                //will calculate interest based on common reference point,
                //but set to tp eop date to init
                dtEOPDate = calcParameters.ParentTimePeriod.Date;
            }
            //input DateTime
            dtInputDate = input.Date;
            dbTimes = input.Times;
            bUseAOH = input.UseAOH;
            if (bUseAOH == true && calcParameters.ParentTimePeriod != null)
            {

                dbEnterpriseAmount = calcParameters.ParentTimePeriod.Amount;
                if (dbEnterpriseAmount == 0)
                {
                    dbEnterpriseAmount = 1;
                }
                dbEntPractAOHFactor = calcParameters.ParentTimePeriod.OverheadFactor 
                    / dbEnterpriseAmount;
            }
            else
            {
                dbEntPractAOHFactor = 1;
            }
            //oc price
            dbInputPrice = input.OCPrice;
            dbInputPrice = dbInputPrice * dbRegionMult;
            dbValue = dbInputPrice * dbEntPractAOHFactor;
            //NOTE: price is non-discounted, INT holds the discounted amount, and always subtracted out at comp level in display totals
            input.OCPrice = dbValue;
            //aoh price - don't adjust annuities
            dbInputPrice = input.AOHPrice;
            dbInputPrice = dbInputPrice * dbRegionMult;
            dbValue = dbInputPrice * dbEntPractAOHFactor;
            input.AOHPrice = dbValue;
            //cap price
            dbInputPrice = input.CAPPrice;
            dbInputPrice = dbInputPrice * dbRegionMult;
            dbValue = dbInputPrice * dbEntPractAOHFactor;
            input.CAPPrice = dbValue;

            //Step 2: calculate a with-in year interest charge on inputs using a nominal interest rate
            //keep annuities out of here = they don't have w/i year interest
            spanDays = dtEOPDate - dtInputDate;
            dbDaysToEOP = spanDays.Days;
            if (dbDaysToEOP < 0)
            {
                dbDaysToEOP = -1 * (dbDaysToEOP);
            }
            //nominal discount factor used to calculate w/i period nominal interest charge
            dbNomMonths = dbDaysToEOP / 30;
            if (bDiscountYorN == true)
            {
                dbNomDiscountFactor
                    = GeneralRules.CalculateDiscountFactorByMonths(dbNomRate, dbNomMonths);
            }
            else
            {
                dbNomDiscountFactor = 1;
                dbNomRate = 0;
            }
            //oc price
            dbOCAmount = input.OCAmount;
            //set it for display
            input.OCAmount = dbOCAmount;
            dbCalcInputPrice = (input.OCPrice * dbOCAmount) * dbTimes;
            dbInterest = (dbCalcInputPrice * dbNomDiscountFactor) - dbCalcInputPrice;
            input.TotalOC_INT = dbInterest;
            input.TotalOC = dbCalcInputPrice;
            //aoh price
            dbAOHAmount = input.AOHAmount;
            //set it for display
            input.AOHAmount = dbAOHAmount;
            dbCalcInputPrice = (input.AOHPrice * dbAOHAmount) * dbTimes;
            dbInterest = (dbCalcInputPrice * dbNomDiscountFactor) - dbCalcInputPrice;
            dbValue = dbInterest;
            input.TotalAOH_INT = dbValue;
            dbValue = dbCalcInputPrice;
            input.TotalAOH = dbValue;

            //cap price
            dbCAPAmount = input.CAPAmount;
            //set it for display
            input.CAPAmount = dbCAPAmount;
            dbCalcInputPrice = (input.CAPPrice * dbCAPAmount) * dbTimes;
            dbInterest = ((dbCalcInputPrice * dbNomDiscountFactor) - dbCalcInputPrice);
            dbValue = dbInterest;
            input.TotalCAP_INT = dbValue;
            dbValue = dbCalcInputPrice;
            input.TotalCAP = dbValue;
            //Step 3: if this input is a preproduction cost, add interest to it from its end of period DateTime
            //to 1 time period before the common reference point time period, using a real rate
            //add one year of interest at nominal rate for production year (p.10-9)
            //now all preproductive costs have been adjusted to the end of the common ref point (first productive year)
            if (calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.budgets
                || calcParameters.SubApplicationType == Constants.SUBAPPLICATION_TYPES.investments)
            {
                if ((dtEOPDate <= calcParameters.ParentBudgetInvestment.InitEOPDate))
                {
                    //a) How many time periods in the preproduction period?
                    spanDays = dtEOPDate - calcParameters.ParentBudgetInvestment.InitEOPDate;
                    dbSpanDays = spanDays.Days;
                    dbTimePeriods = dbSpanDays / DOUBLE_YEAR;
                    //b) How many days from this input's end of period to the common reference point DateTime?
                    if (dbTimePeriods != 0)
                    {
                        spanDays = dtEOPDate - calcParameters.ParentBudgetInvestment.InitEOPDate;
                        dbDaysFromCRPToEOP = spanDays.Days;
                        if (dbDaysFromCRPToEOP < 0)
                        {
                            dbDaysFromCRPToEOP = -1 * dbDaysFromCRPToEOP;
                        }
                        //c) How many days in a time period?
                        dbDaysPerPeriod = dbDaysFromCRPToEOP / dbTimePeriods;
                        if (dbDaysPerPeriod < 0)
                        {
                            dbDaysPerPeriod = -1 * dbDaysPerPeriod;
                        }
                        //d) Subtract out the productive year, and the preproductive year that already has a nominal
                        // interest charge
                        dbRealInterestDays = (dbDaysFromCRPToEOP - dbDaysPerPeriod);
                        if (dbRealInterestDays < 0)
                        {
                            dbRealInterestDays = -1 * (dbRealInterestDays);
                        }
                        //e) accrue interest on these days using an annual discounting formula
                        dbRealYrs = dbRealInterestDays / DOUBLE_YEAR;
                        if (bDiscountYorN == true)
                        {
                            dbDiscountFactor
                                = System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate), dbRealYrs);
                        }
                        else
                        {
                            dbDiscountFactor = 1;
                        }
                        //oc price
                        dbInterest = input.TotalOC_INT;
                        dbInputandInterestTotal = input.TotalOC + dbInterest;
                        dbInterest = dbInterest + ((dbInputandInterestTotal * dbDiscountFactor) - dbInputandInterestTotal);
                        dbValue = dbInterest;
                        input.TotalOC_INT = dbValue;
                        //aoh price
                        dbInterest = input.TotalAOH_INT;
                        dbInputandInterestTotal = input.TotalAOH + dbInterest;
                        dbInterest = dbInterest + ((dbInputandInterestTotal * dbDiscountFactor)
                            - dbInputandInterestTotal);
                        dbValue = dbInterest;
                        input.TotalAOH_INT = dbValue;
                        //cap price
                        dbInterest = input.TotalCAP_INT;
                        dbInputandInterestTotal = input.TotalCAP + dbInterest;
                        dbInterest = dbInterest + ((dbInputandInterestTotal * dbDiscountFactor)
                            - dbInputandInterestTotal);
                        dbValue = dbInterest;
                        input.TotalCAP_INT = dbValue;
                        //add in the nominal interest
                        //oc price
                        dbInterest = input.TotalOC_INT;
                        dbInputandInterestTotal = input.TotalOC + dbInterest;
                        dbInterest = dbInterest + (dbInputandInterestTotal * dbNomRate);
                        dbValue = dbInterest;
                        input.TotalOC_INT = dbValue;
                        //aoh price
                        dbInterest = input.TotalAOH_INT;
                        dbInputandInterestTotal = input.TotalAOH + dbInterest;
                        dbInterest = dbInterest + (dbInputandInterestTotal * dbNomRate);
                        dbValue = dbInterest;
                        input.TotalAOH_INT = dbValue;
                        //cap price
                        dbInterest = input.TotalCAP_INT;
                        dbInputandInterestTotal = input.TotalCAP + dbInterest;
                        dbInterest = dbInterest + (dbInputandInterestTotal * dbNomRate);
                        dbValue = dbInterest;
                        input.TotalCAP_INT = dbValue;
                    }
                }
                //Step 4. if (this input comes after the common reference point, discount its interest back to the
                //common reference point. Discount its totals back to the common point using real rates
                if ((dtInputDate > calcParameters.ParentBudgetInvestment.InitEOPDate)
                    && (bDiscountYorN == true))
                {
                    //a) discount its total back to crp and put this in interest
                    //means that the interest charge is used to derive discounted totals
                    //This should be a negative number
                    spanDays = calcParameters.ParentBudgetInvestment.InitEOPDate - dtEOPDate;
                    dbDaysFromCRPToEOP = spanDays.Days;
                    if (dbDaysFromCRPToEOP < 0)
                    {
                        dbDaysFromCRPToEOP = -1 * dbDaysFromCRPToEOP;
                    }
                    dbRealYrs = dbDaysFromCRPToEOP / DOUBLE_YEAR;
                    if (bDiscountYorN == true)
                    {
                        dbDiscountFactor = 1 / System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate), dbRealYrs);
                    }
                    else
                    {
                        dbDiscountFactor = 1;
                    }
                    //note: add nominal interest to the input cost for within period costs
                    dbInputandInterestTotal = input.TotalOC + input.TotalOC_INT;
                    //discounted total input between period real cost
                    dbInterest = dbInputandInterestTotal - (dbInputandInterestTotal * dbDiscountFactor);
                    dbValue = -1 * dbInterest;
                    input.TotalOC_INT = dbValue;
                    //aoh
                    dbInputandInterestTotal = input.TotalAOH + input.TotalAOH_INT;
                    dbInterest = dbInputandInterestTotal - (dbInputandInterestTotal * dbDiscountFactor);
                    dbValue = -1 * dbInterest;
                    input.TotalAOH_INT = dbValue;
                    //cap
                    dbInputandInterestTotal = input.TotalCAP + input.TotalCAP_INT;
                    dbInterest = dbInputandInterestTotal - (dbInputandInterestTotal * dbDiscountFactor);
                    dbValue = -1 * dbInterest;
                    input.TotalCAP_INT = dbValue;
                }
            }
            double dbIncentiveAdjustedSum = 0;
            CalculateIncentiveForInput(input, calcParameters,
                ref dbIncentiveAdjustedSum);
            return dbCalcInputPrice;
        }
        private void CalculateIncentiveForInput(Input input,
            CalculatorParameters calcParameters, ref double incentiveAdjustedSum)
        {
            double dbTotalCost = 0;
            incentiveAdjustedSum = 0;
            //input totals do not include discount interest
            dbTotalCost = input.TotalOC + input.TotalOC_INT + input.TotalAOH
                + input.TotalAOH_INT + input.TotalCAP + input.TotalCAP_INT;
            //calculate incentives for this node
            //see rules in Example 5c, Operation, Incentives
            //incentive-adjusted total costs (positive incentive decreases costs)
            incentiveAdjustedSum = dbTotalCost - input.IncentiveAmount -
                (dbTotalCost * input.IncentiveRate);
            input.TotalINCENT = incentiveAdjustedSum;
        }
        private void CalculateIncentiveForOutput(Output output,
            CalculatorParameters calcParameters, ref double incentiveAdjustedSum)
        {
            double dbTotalRevenue = 0;
            incentiveAdjustedSum = 0;
            //input totals do not include discount interest
            dbTotalRevenue
                = output.TotalR + output.TotalR_INT;
            //calculate incentives for this node
            //see rules in Example 5c, Operation, Incentives
            //incentive-adjusted total revenues (positive incentive increases revenues)
            incentiveAdjustedSum = dbTotalRevenue + output.IncentiveAmount +
                (dbTotalRevenue * output.IncentiveRate);
            output.TotalRINCENT = incentiveAdjustedSum;
        }
        /// <summary>
        /// Calculate input, operation/component, and time period incentives based on incentive rates and amounts
        /// </summary>
        private void CalculateIncentiveForComponentOperation(OperationComponent opComp,
            CalculatorParameters calcParameters, ref double incentiveAdjustedSum)
        {
            double dbTotalCost = 0;
            incentiveAdjustedSum = 0;
            //TINCENT is the cumulative, incentive-adjusted, cost totals to this point
            dbTotalCost = opComp.TotalINCENT;
                
            //calculate incentives for this node
            //see rules in Example 5c, Operation, Incentives
            //incentive-adjusted total costs (positive incentive decreases costs)
            incentiveAdjustedSum = dbTotalCost - opComp.IncentiveAmount -
                (dbTotalCost * opComp.IncentiveRate);
            opComp.TotalINCENT = incentiveAdjustedSum;
        }
        /// <summary>
        /// Calculate outcome incentives based on incentive rates and amounts
        /// </summary>
        private void CalculateIncentiveForOutcome(Outcome outcome,
            CalculatorParameters calcParameters, ref double incentiveAdjustedSum)
        {
            double dbTotalBenefit = 0;
            incentiveAdjustedSum = 0;
            //TRINCENT is the cumulative, incentive-adjusted, benefit totals to this point
            dbTotalBenefit = outcome.TotalRINCENT;

            //calculate incentives for this node
            incentiveAdjustedSum = dbTotalBenefit + outcome.IncentiveAmount +
                (dbTotalBenefit * outcome.IncentiveRate);
            outcome.TotalRINCENT = incentiveAdjustedSum;
        }
        private void CalculateIncentiveForTimePeriod(TimePeriod tp,
            CalculatorParameters calcParameters, ref double incentiveAdjustedSum)
        {
            double dbTotalCost = 0;
            double dbTotalRevenue = 0;
            incentiveAdjustedSum = 0;
            //TINCENT is the cumulative, incentive-adjusted, cost totals to this point
            dbTotalCost = tp.TotalINCENT;
            //TRINCENT is the cumulative, incentive-adjusted, revenue totals to this point
            dbTotalRevenue = tp.TotalRINCENT;

            //calculate incentives for this node
            //see rules in Example 5c, Operation, Incentives
            //incentive-adjusted total costs (positive incentive decreases costs)
            incentiveAdjustedSum = dbTotalCost - tp.IncentiveAmount -
                (dbTotalCost * tp.IncentiveRate);
            tp.TotalINCENT = incentiveAdjustedSum;

            //incentive-adjusted total revenues (positive incentive increases revenues)
            incentiveAdjustedSum = dbTotalRevenue + tp.IncentiveAmount +
                (dbTotalRevenue * tp.IncentiveRate);
            tp.TotalRINCENT = incentiveAdjustedSum;
        }
        
        /// <summary>
        /// Set rules, interest charges, and price adjustments for each output included in a budget or capital investment
        /// </summary>
        public double CalculateOutputPrice( 
            Output output, ref CalculatorParameters calcParameters)
        {
            double dbCalcOutputPrice = 0;
            double dbDaysToEOP = 0;
            double dbNomRate = calcParameters.ParentBudgetInvestment.Local.NominalRate;
            double dbNomDiscountFactor = 0;
            double dbDiscountFactor = 0;
            double dbNomMonths = 0;
            double dbRealYrs = 0;
            double dbRegionMult = 1;
            double dbTimePeriods = 0;
            double dbOutputPrice = 0;
            bool bDiscountYorN = true;
            DateTime dtEOPDate;
            DateTime dtOutputDate;
            double dbDaysFromCRPToEOP = 0;
            double dbDaysPerPeriod = 0;
            double dbRealInterestDays = 0;
            double dbInterest = 0;
            double dbInputandInterestTotal = 0;
            double dbValue = 0;
            System.TimeSpan spanDays;
            double dbSpanDays = 0;
            double dbOutAmount = 0;
            double dbCompositionAmount = 0;

            //inititalize variables
            dbInterest = 0;
            bDiscountYorN = calcParameters.ParentTimePeriod.IsDiscounted;
            //common reference point DateTime is dtInitEOPDate
            //end of period DateTime (for all periods
            dtEOPDate = calcParameters.ParentTimePeriod.Date;
            dtOutputDate = output.Date;
            //Step 1: calculate a with-in year interest charge on Outputs using a nominal interest rate
            //   but do not accrue interest on any Output that is part of an operation or component that will
            //   be amortized; because that capital recovery factor will provide an opportunity interest charge
            //   for the Output
            spanDays = dtEOPDate - dtOutputDate;
            dbDaysToEOP = spanDays.Days;
            if (dbDaysToEOP < 0)
            {
                dbDaysToEOP = -1 * (dbDaysToEOP);
            }
            //nominal discount factor used to calculate w/i period nominal interest charge
            dbNomMonths = dbDaysToEOP / 30;
            if (bDiscountYorN == true)
            {
                dbNomDiscountFactor = GeneralRules.CalculateDiscountFactorByMonths(dbNomRate, dbNomMonths);
            }
            else
            {
                dbNomDiscountFactor = 1;
                dbNomRate = 0;
            }
            //output price
            dbOutputPrice = output.Price;
            dbOutputPrice = dbOutputPrice * dbRegionMult;
            dbOutAmount = output.Amount;
            //set it for display
            output.Amount = dbOutAmount;
            dbCompositionAmount = output.CompositionAmount;
            //set it for display
            output.CompositionAmount = dbCompositionAmount;
            //v145 bug left out output.times
            dbCalcOutputPrice = ((dbOutputPrice * dbOutAmount) * dbCompositionAmount * output.Times);
            dbInterest = ((dbCalcOutputPrice * dbNomDiscountFactor) - dbCalcOutputPrice);
            //set the prices
            dbValue = dbCalcOutputPrice;
            output.TotalR = dbValue;
            dbValue = dbInterest;
            output.TotalR_INT = dbValue;

            //Step 2: if (this Output is a preproduction revenue, add interest to it from it's end of period DateTime
            //to 1 time period before the common reference point time period, using a real rate
            //this step is only used to add real interest between annual time periods (not monthly)
            if (dtEOPDate <= calcParameters.ParentBudgetInvestment.InitEOPDate)
            {
                //a) How many annual time periods in the preproduction period?
                spanDays = dtEOPDate - calcParameters.ParentBudgetInvestment.InitEOPDate;
                dbSpanDays = spanDays.Days;
                dbTimePeriods = dbSpanDays / DOUBLE_YEAR;
                //b) How many days from this Output's end of period to the common reference point DateTime?
                if (dbTimePeriods != 0)
                {
                    spanDays = dtEOPDate - calcParameters.ParentBudgetInvestment.InitEOPDate;
                    dbDaysFromCRPToEOP = spanDays.Days;
                    if (dbDaysFromCRPToEOP < 0)
                    {
                        dbDaysFromCRPToEOP = -1 * (dbDaysFromCRPToEOP);
                    }
                    //c) How many days in a time period?
                    dbDaysPerPeriod = dbDaysFromCRPToEOP / dbTimePeriods;
                    if (dbDaysPerPeriod < 0)
                    {
                        dbDaysPerPeriod = -1 * (dbDaysPerPeriod);
                    }
                    //d) Subtract out the preproductive year that already has a nominal
                    //interest charge
                    dbRealInterestDays = dbDaysFromCRPToEOP - dbDaysPerPeriod;
                    if (dbRealInterestDays < 0)
                    {
                        dbRealInterestDays = -1 * dbRealInterestDays;
                    }
                    //e) accrue interest on these days using an annual discounting formula, if the periods are at least annual
                    dbRealYrs = dbRealInterestDays / DOUBLE_YEAR;
                    if (bDiscountYorN == true)
                    {
                        dbDiscountFactor = (System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate), dbRealYrs));
                    }
                    else
                    {
                        dbDiscountFactor = 1;
                    }
                    //output price
                    dbInterest = output.TotalR_INT;
                    dbInputandInterestTotal = output.TotalR + dbInterest;
                    dbInterest = dbInterest + ((dbInputandInterestTotal * dbDiscountFactor) 
                        - dbInputandInterestTotal);
                    dbValue = dbInterest;
                    //added 0.9.7 to be consistent with original calcs
                    dbInputandInterestTotal = output.TotalR + dbInterest;
                    output.TotalR_INT = dbValue;
                    //f) add nominal interest for the productive year, so that all prices are at common reference point
                    dbInterest = dbInterest + (dbInputandInterestTotal * dbNomRate);
                    dbValue = dbInterest;
                    output.TotalR_INT = dbValue;
                }
            }
            //Step 3. if this Output comes after the common reference point, discount the interest back to the
            //common reference point. Discount the prices back to the common point and redo the totals
            if ((dtOutputDate > calcParameters.ParentBudgetInvestment.InitEOPDate) 
                && (bDiscountYorN))
            {
                //a) first discount it's interest, using end of period DateTime
                spanDays = calcParameters.ParentBudgetInvestment.InitEOPDate - dtEOPDate;
                dbDaysFromCRPToEOP = spanDays.Days;
                if (dbDaysFromCRPToEOP < 0)
                {
                    dbDaysFromCRPToEOP = -1 * (dbDaysFromCRPToEOP);
                }
                dbRealYrs = dbDaysFromCRPToEOP / DOUBLE_YEAR;
                if (bDiscountYorN == true)
                {
                    dbDiscountFactor = 1 / (System.Math.Pow((1 + calcParameters.ParentBudgetInvestment.Local.RealRate), dbRealYrs));
                }
                else
                {
                    dbDiscountFactor = 1;
                }
                //discounted total output cost (nominal wi period)
                dbInputandInterestTotal = output.TotalR + output.TotalR_INT;
                //discount this total back to the reference point using real discount factor
                dbInterest = -1 * (dbInputandInterestTotal - (dbInputandInterestTotal * dbDiscountFactor));
                //no need to add the wi period nominal interest - the real interest was based on a total that included that interest
                dbValue = dbInterest;
                output.TotalR_INT = dbValue;
            }
            double dbIncentiveAdjustedSum = 0;
            CalculateIncentiveForOutput(output,
                calcParameters, ref dbIncentiveAdjustedSum);
            return dbCalcOutputPrice;
        }
        
        /// <summary>
        ///  Add ann equiv nodes each time an annuity is formed in analyses; store only in aoh section
        ///  Includes preprod annuity and regular complife>1 annuities.
        ///  Output is an addition to the collection of annuities that get added to subsequent time periods
        ///  the annuity added.
        /// </summary>
        public void AddAnnualEquivalents1(ref CalculatorParameters calcParameters,
            TimePeriod.ANNUITY_TYPES annuityType, string nodeName, 
            int timePeriodId, int prodPeriods, DateTime timeDate, 
            int iStartAnnuityCount, double tAOH, double tAMAOH, double tINCENT, 
            double tAMINCENT)
        {
            string sName = string.Empty;
            //Rules on adding
            //preprods: no interest included year0, and regular p&I thereafter
            //regs: don't go through here in year0, I only in year 1, and regular p&I thereafter
            //a) create new objects
            //set the properties of a new operationcomponent object
            OperationComponent annOpComponent = new OperationComponent();
            annOpComponent.TimePeriodId = timePeriodId;
            annOpComponent.AnnuityType = annuityType;
            annOpComponent.Id = CalculatorHelpers.GetRandomInteger(calcParameters); 
            if (nodeName.EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()) 
                || nodeName.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                annOpComponent.Name = string.Concat(
                    calcParameters.ParentOperationComponent.Name, "-", CAPRECOVERY);
                annOpComponent.EffectiveLife = calcParameters.ParentOperationComponent.EffectiveLife;
                annOpComponent.AnnuityCount = iStartAnnuityCount;
                annOpComponent.SalvageValue = calcParameters.ParentOperationComponent.SalvageValue;
                annOpComponent.Unit = BudgetInvestment.ANNUITY;
            }
            else if (nodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString() 
                || nodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                annOpComponent.Name = PREPRODCAPRECOVERY;
                annOpComponent.EffectiveLife = (double) prodPeriods;
                annOpComponent.AnnuityCount = iStartAnnuityCount;
                annOpComponent.SalvageValue = 0;
                annOpComponent.Unit = BudgetInvestment.ANNUITY;
            }
            //interest set each time annuity is added to a tp;
            //first year is interest only; last year can be fractional and includes salvage value
            annOpComponent.TotalAMOC = 0;
            annOpComponent.TotalAMAOH = tAMAOH;
            annOpComponent.TotalAMCAP = 0;
            annOpComponent.TotalAMOC_INT = 0;
            annOpComponent.TotalAMAOH_INT = 0; //set each time added to tp
            annOpComponent.TotalAMCAP_INT = 0;
            annOpComponent.TotalAMINCENT = tAMINCENT;
            //annuities never added to regular totals columns rule enforced
            annOpComponent.TotalOC = 0;
            annOpComponent.TotalAOH = 0;
            annOpComponent.TotalCAP = 0;
            annOpComponent.TotalINCENT = 0;
            annOpComponent.Amount = tAOH;
            annOpComponent.Date = timeDate;
            annOpComponent.IncentiveRate = 0;
            annOpComponent.IncentiveAmount = 0;

            //add an input object to opcomp
            Input annInput = new Input();
            annInput.Id = CalculatorHelpers.GetRandomInteger(calcParameters);
            annInput.AnnuityType = annuityType;
            if (nodeName.EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString())
                || nodeName.EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                annInput.OpCompId = annOpComponent.Id;
                annInput.Name = annOpComponent.Name;
            }
            else if (nodeName == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || nodeName == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                annInput.Name = PREPRODCAPRECOVERY_ABB;
            }
            annInput.AOHAmount = 1;
            //the sum being amortized - for display only - inputs not used with annuities except for display
            annInput.AOHPrice = tAOH;
            annInput.TotalINCENT = tINCENT;
            annInput.AOHUnit = BudgetInvestment.ANNUITY;
            annInput.TotalAMAOH = tAMAOH;
            annInput.TotalAMAOH_INT = 0;
            annInput.TotalAMINCENT = tAMINCENT;
            annInput.TotalOC = 0;
            annInput.TotalAOH = 0;
            annInput.TotalCAP = 0;
            annInput.TotalOC_INT = 0;
            annInput.TotalAOH_INT = 0;
            annInput.TotalCAP_INT = 0;

            annInput.TotalAMOC = 0;
            annInput.TotalAMCAP = 0;
            annInput.CAPAmount = 0;
            annInput.CAPPrice = 0;
            annInput.CAPUnit = BudgetInvestment.ANNUITY;
            annInput.OCAmount = 0;
            annInput.OCPrice = 0;
            annInput.OCUnit = BudgetInvestment.ANNUITY;
            annInput.UseAOH = false;
            annInput.Times = 0;
            annInput.Date = timeDate;
            annInput.InputGroupId = (int) annuityType;
            annInput.InputGroupName = BudgetInvestment.ANNUITY;
            annInput.TypeId = (int) annuityType;
            annInput.Name = BudgetInvestment.ANNUITY;
            if (annOpComponent.Inputs == null)
                annOpComponent.Inputs = new List<Input>();
            annOpComponent.Inputs.Add(annInput);
            //add it to annequivs collection
            if (calcParameters.ParentBudgetInvestment.AnnEquivalents == null)
                calcParameters.ParentBudgetInvestment.AnnEquivalents = new List<OperationComponent>();
            calcParameters.ParentBudgetInvestment.AnnEquivalents.Add(annOpComponent);
        }
        /// <summary>
        /// Run calculations on TimePeriod.AnnEquivs and add the calculations to 
        /// TimePeriod.OperationComponents collection.
        /// </summary>
        public bool AddTimePeriodAnnualEquivalents(
            CalculatorParameters calcParameters, bool insertAllAEs)
        {
            bool bIsAdded = false;
            //are these two variables really needed below?
            double dbTimeAmort2Total = 0;
            double dbTimeAmort2IntTotal = 0;
            int iTimePeriodDays = 0;
            bool bDiscountYorN = true;
            int iTimeId = INT_SEED;
            TimePeriod.GROWTH_SERIES_TYPES eGrowthType 
                = TimePeriod.GROWTH_SERIES_TYPES.uniform;
            string sSeriesType = string.Empty;
            bool bRetVal = false;
            double dbTimeAmount = 0;
            string sName = string.Empty;
            System.TimeSpan spanTimePeriodDays;
            if ((calcParameters.ParentTimePeriod.Date 
                != calcParameters.ParentTimePeriod.LastPeriodDate))
            {
                //initialize last tps totals to days per time period for monthly/annual
                spanTimePeriodDays = calcParameters.ParentTimePeriod.Date 
                    - calcParameters.ParentTimePeriod.LastPeriodDate;
                iTimePeriodDays = spanTimePeriodDays.Days;
                if (iTimePeriodDays < 0) iTimePeriodDays = iTimePeriodDays * -1;
            }
            else
            {
                //default is annual and in year 1
                iTimePeriodDays = (int) DOUBLE_YEAR;
            }
            dbTimeAmort2Total = 0;
            dbTimeAmort2IntTotal = 0;
            bDiscountYorN = calcParameters.ParentTimePeriod.IsDiscounted;
            eGrowthType = (TimePeriod.GROWTH_SERIES_TYPES) calcParameters.ParentTimePeriod.GrowthTypeId;
            dbTimeAmount = calcParameters.ParentTimePeriod.Amount;
            iTimeId = CalculatorHelpers.GetRandomInteger(calcParameters);
            sSeriesType = TimePeriod.GROWTH_SERIES_TYPES.uniform.ToString();
            bRetVal = AddAnnualEquivalents2(calcParameters, 
                calcParameters.ParentTimePeriod.Date, iTimeId, 
                iTimePeriodDays, ref dbTimeAmort2Total,
                ref dbTimeAmort2IntTotal, iTimePeriodDays,
                bDiscountYorN, insertAllAEs);
            if (bRetVal == true)
            {
                bIsAdded = true;
            }
            else
            {
                bIsAdded = false;
                return bIsAdded;
            }
            if (insertAllAEs == true)
            {
                //refactor: if annuities need to be 'cleaned up', put this back in
                //this is not equivalent to using the old newTimeElement
                //double dbTempSum = 0;
                //won't be passing through display totals so do accruals here
                //Rule: left over annuities only get added to amortaoh
                //calcParameters.ParentTimePeriod.Id = iTimeId;
                //calcParameters.ParentTimePeriod.Name = CalculatorHelpers.ANNUITY;
                //calcParameters.ParentTimePeriod.IsDiscounted = bDiscountYorN;
                //calcParameters.ParentTimePeriod.GrowthPeriods = 1;
                //calcParameters.ParentTimePeriod.GrowthTypeId = 1;
                //calcParameters.ParentTimePeriod.IsCommonReference = true;
                //calcParameters.ParentTimePeriod.OverheadFactor = 1;
                //calcParameters.ParentTimePeriod.IncentiveAmount = 0;
                //calcParameters.ParentTimePeriod.IncentiveRate = 0;
                //sName = string.Concat(calcParameters.ParentTimePeriod.Name2, SPACE, " Remaining Annuities");
                //calcParameters.ParentTimePeriod.Name2 = sName;
                //calcParameters.ParentTimePeriod.Amount = dbTimeAmount;
                ////calcParameters.ParentTimePeriod. = eTimeElement.GetAttribute("BudgetSystemToPracticeId").ToString());
                //calcParameters.ParentTimePeriod.Unit = CalculatorHelpers.ANNUITY;
                //calcParameters.ParentTimePeriod.TotalOC = 0;
                //calcParameters.ParentTimePeriod.TotalAOH = 0;
                //calcParameters.ParentTimePeriod.TotalCAP = 0;
                //calcParameters.ParentTimePeriod.TotalINCENT = 0;
                //calcParameters.ParentTimePeriod.TotalOC_INT = 0;
                //calcParameters.ParentTimePeriod.TotalAOH_INT = 0;
                //calcParameters.ParentTimePeriod.TotalCAP_INT = 0;
                //calcParameters.ParentTimePeriod.TotalR = 0;
                //calcParameters.ParentTimePeriod.TotalR_INT = 0;
                //calcParameters.ParentTimePeriod.TotalAMR = 0;
                //calcParameters.ParentTimePeriod.TotalAMOC = 0;
                //dbTempSum = dbTimeAmort2Total * dbTimeAmount;
                //calcParameters.ParentTimePeriod.TotalAMAOH = dbTimeAmort2Total * dbTimeAmount;
                //calcParameters.ParentTimePeriod.TotalAMCAP = 0;
                ////holds annuity interest sums alone
                //dbTempSum = dbTimeAmort2IntTotal * dbTimeAmount;
                //calcParameters.ParentTimePeriod.TotalAMAOH_INT = dbTimeAmort2IntTotal * dbTimeAmount;
                ////1 year budgets; budgets with no annuities; time periods prior to common ref pt
                //double dbTimeNetAmortTotal = dbTimeAmort2Total * dbTimeAmount;
                //calcParameters.ParentTimePeriod.TotalAMAOH_NET = dbTimeNetAmortTotal;
                //calcParameters.ParentBudgetInvestment.TotalAnnuities += dbTimeNetAmortTotal;
                ////should be handled similar to totalNetAmortTotal, adequate for now
                //calcParameters.ParentTimePeriod.TotalAMINCENT = 0;
                ////set totals properties here as well
                //calcParameters.ParentBudgetInvestment.TotalAMAOH += (dbTimeAmort2Total * dbTimeAmount);
            }
            return bIsAdded;
        }
        public bool AddAnnualEquivalents2(CalculatorParameters calcParameters,
            DateTime endOfGrowthPeriodDate, int iTimeId, double initDiscountPeriod, 
            ref double timeAmort2Total, ref double timeAmort2IntTotal, int timePeriodDays, 
            bool discountYorN, bool insertAllAEs)
        {
            bool bIsAdded = false;
            //int iCount = 0;
            int iStart = 0;
            double dbEffLife = 0;
            double dbAEPrincipal = 0;
            double dbAEInterest = 0;
            double dbAEDiscPrincipal = 0;
            //160 added incentive totals
            double dbAEPrincipal2 = 0;
            double dbAEInterest2 = 0;
            double dbAEDiscPrincipal2 = 0;
            double dbDiscountFactor = 0;
            double dbFractDiscFactor = 0;
            string sName = string.Empty;
            string sCompChangeName = string.Empty;
            string sSeriesType = string.Empty;
            double dbSalvValue = 0;
            double dbLastTimeDays = 0;
            double dbAOHAmount = 0;
            int iEffLife = 0;
            bool bUseFractYr = false;
            System.TimeSpan spanDays;
            System.TimeSpan spanLeapYearDay = new TimeSpan(1, 0, 0, 0);
            if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
            {
                if (calcParameters.ParentBudgetInvestment.AnnEquivalents.Count > 0)
                {
                    List<int> removeUsedUpAnnEquivIds = new List<int>();
                    foreach (OperationComponent opcomp
                        in calcParameters.ParentBudgetInvestment.AnnEquivalents)
                    {
                        int iCount = 0;
                        int i = 0;
                        int iAnnuityCounter = 0;
                        //reinit back to starting end of growth date
                        DateTime dtEndOfGrowthPeriodDate = endOfGrowthPeriodDate;
                        dbAEPrincipal = opcomp.TotalAMAOH + opcomp.TotalAMAOH_INT;
                        dbAEPrincipal2 = opcomp.TotalAMINCENT;
                        dbEffLife = opcomp.EffectiveLife;
                        dbSalvValue = opcomp.SalvageValue;
                        sName = opcomp.Name;
                        iStart = opcomp.AnnuityCount;
                        dbAOHAmount = opcomp.Amount;
                        if (iStart > dbEffLife)
                        {
                            //can only be a fractional year, 
                            //otherwise it would have been removed using Remove function
                            iCount = iStart;
                            //but use a fractional year
                            bUseFractYr = true;
                            iEffLife = iStart;
                        }
                        else
                        {
                            //handle clean up years
                            iCount = (int)dbEffLife;
                            if (iCount != dbEffLife)
                            {
                                //it's been rounded, use fractional year
                                bUseFractYr = true;
                            }
                        }
                        //Rule 1. rounding down not allowed 
                        //- will always use that last fractional year
                        if (dbEffLife > iCount)
                        {
                            iCount = iCount + 1;
                            //might be changed in next if clause
                        }
                        //keep track of salvage value years
                        iEffLife = iCount;
                        //handle regular annuities
                        if (insertAllAEs != true)
                        {
                            if (iCount != iStart)
                            {
                                bUseFractYr = false;
                            }
                            iCount = iStart;
                        }
                        dbFractDiscFactor = GeneralRules.CalculateFractionalPaymentFactor(
                            calcParameters.ParentBudgetInvestment.Local.RealRate,
                            dbEffLife, iCount, initDiscountPeriod, ref dbLastTimeDays);
                        //regular annuities start at calccompamortcost; 
                        //because of that they need 1 period added
                        if ((opcomp.AnnuityType == TimePeriod.ANNUITY_TYPES.regular)
                            && (insertAllAEs == true))
                        {
                            if (iStart > dbEffLife)
                            {
                                //this is a fraction so use an adjusted DateTime
                                int iLastTimeDays = (int)dbLastTimeDays;
                                spanDays = new TimeSpan(iLastTimeDays, 0, 0, 0);
                                DateTime dtEndOfGrowthPeriodSpan
                                    = dtEndOfGrowthPeriodDate + spanDays;
                                dtEndOfGrowthPeriodDate = dtEndOfGrowthPeriodSpan;
                            }
                            else
                            {
                                //clean ups DateTimes
                                int iInitDiscountPeriod = (int)initDiscountPeriod;
                                spanDays = new TimeSpan(iInitDiscountPeriod, 0, 0, 0);
                                dtEndOfGrowthPeriodDate = dtEndOfGrowthPeriodDate + spanDays;
                            }
                            //handle leap years
                            dtEndOfGrowthPeriodDate = AdjustDateForLeapYear(dtEndOfGrowthPeriodDate);
                        }
                        for (i = iStart; i <= iCount; i++)
                        {
                            //initalize the name that will be changed
                            sCompChangeName = sName;
                            if (discountYorN == true)
                            {
                                //Rule 2. Use compounded monthly or annual interest charges
                                if (initDiscountPeriod <= 31)
                                {
                                    //use monthly compounding
                                    dbDiscountFactor
                                        = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                                        calcParameters.ParentBudgetInvestment.InitEOPDate,
                                        dtEndOfGrowthPeriodDate, true);
                                }
                                else
                                {
                                    //use annual compounding
                                    dbDiscountFactor = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                                        calcParameters.ParentBudgetInvestment.InitEOPDate,
                                        dtEndOfGrowthPeriodDate, false);
                                }
                            }
                            else
                            {
                                //don't discount
                                dbDiscountFactor = 1;
                            }
                            //Rule 3. if last time period is a fraction, use fractional discount; this is often equal to one
                            if ((i == iCount) && (bUseFractYr == true))
                            {
                                dbAEPrincipal = dbAEPrincipal * dbFractDiscFactor;
                                dbAEPrincipal2 = dbAEPrincipal2 * dbFractDiscFactor;
                                //now subtract normal discounted interest from this principal
                                if (initDiscountPeriod <= 31)
                                {
                                    dbDiscountFactor
                                        = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                                        calcParameters.ParentBudgetInvestment.InitEOPDate,
                                        dtEndOfGrowthPeriodDate, true);
                                }
                                else if (initDiscountPeriod > 31)
                                {
                                    dbDiscountFactor
                                        = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                                        calcParameters.ParentBudgetInvestment.InitEOPDate,
                                        dtEndOfGrowthPeriodDate, false);
                                }
                            }
                            //discount the annuity correctly and set its principal and interest
                            dbAEInterest = (dbAEPrincipal - (dbAEPrincipal * dbDiscountFactor)) * -1;
                            dbAEDiscPrincipal = dbAEPrincipal + dbAEInterest;
                            dbAEInterest2 = (dbAEPrincipal2 - (dbAEPrincipal2 * dbDiscountFactor)) * -1;
                            dbAEDiscPrincipal2 = dbAEPrincipal2 + dbAEInterest2;
                            //Rule 4. keep principal out of the first tp reg annuity -it's already included as part of the previous time period
                            if ((i == 1)
                                && (opcomp.AnnuityType == TimePeriod.ANNUITY_TYPES.regular))
                            {
                                //discount the annuity correctly and set it's principal and interest
                                dbAEInterest = (dbAEPrincipal - (dbAEPrincipal * dbDiscountFactor)) * -1;
                                dbAEDiscPrincipal = dbAEInterest;
                                dbAEInterest2 = (dbAEPrincipal2 - (dbAEPrincipal2 * dbDiscountFactor)) * -1;
                                dbAEDiscPrincipal2 = dbAEInterest2;
                            }
                            else if ((i == 1)
                                && (opcomp.AnnuityType == TimePeriod.ANNUITY_TYPES.preproduction))
                            {
                                //keep interest out of first full production year
                                dbAEInterest = 0;
                                dbAEDiscPrincipal = dbAEPrincipal;
                                dbAEDiscPrincipal2 = dbAEPrincipal2;
                            }
                            sCompChangeName = sCompChangeName.Replace(CAPRECOVERY, "") + " Period" + " - " + i;
                            //add the calculations to the TimePeriod.OpComps collection 
                            AddAnnualEquivalentsChildren(ref calcParameters,
                                calcParameters.SubApplicationType.ToString(), opcomp, 
                                sSeriesType, sCompChangeName, iTimeId,
                                dtEndOfGrowthPeriodDate, dbAEPrincipal, dbAEInterest,
                                dbAEDiscPrincipal, dbAEDiscPrincipal2, i, dbEffLife, dbSalvValue, dbAOHAmount,
                                insertAllAEs);
                            //accrue both the int and the principal in normal counters
                            timeAmort2IntTotal += dbAEInterest;
                            timeAmort2Total += dbAEDiscPrincipal;
                            //Rule 5. add discounted salvage values for each ae that has been added
                            if ((i == iEffLife)
                                && (opcomp.AnnuityType == TimePeriod.ANNUITY_TYPES.regular))
                            {
                                if (initDiscountPeriod <= 31)
                                {
                                    //use monthly compounding
                                    dbDiscountFactor
                                        = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                                        calcParameters.ParentBudgetInvestment.InitEOPDate,
                                        dtEndOfGrowthPeriodDate, true);
                                }
                                else
                                {
                                    //use annual compounding
                                    dbDiscountFactor
                                        = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                        calcParameters.ParentBudgetInvestment.Local.RealRate,
                                        calcParameters.ParentBudgetInvestment.InitEOPDate,
                                        dtEndOfGrowthPeriodDate, false);
                                }
                                sCompChangeName = sCompChangeName.Replace(CAPRECOVERY, "") + " Salvage Value";
                                dbAEPrincipal = dbSalvValue;
                                dbAEInterest = (dbAEPrincipal - (dbSalvValue * dbDiscountFactor)) * -1;
                                dbAEDiscPrincipal = dbAEPrincipal + dbAEInterest;
                                //salvage value is the same
                                dbAEDiscPrincipal2 = dbAEPrincipal;
                                AddAnnualEquivalentsChildren(ref calcParameters,
                                    calcParameters.SubApplicationType.ToString(), opcomp, 
                                    sSeriesType, sCompChangeName, iTimeId,
                                    dtEndOfGrowthPeriodDate, dbAEPrincipal, dbAEInterest,
                                    dbAEDiscPrincipal, dbAEDiscPrincipal2, i, dbEffLife, dbSalvValue, dbAOHAmount,
                                    insertAllAEs);
                                //accrue the discounted salvage value in normal counters
                                timeAmort2IntTotal += dbAEInterest;
                                timeAmort2Total += dbAEDiscPrincipal;
                            }
                            //increment counters for next pass through
                            iAnnuityCounter = opcomp.AnnuityCount + 1;
                            opcomp.AnnuityCount = iAnnuityCounter;
                            //no need to remove regular annuity op/comp nodes (causes error in for loop)
                            if (opcomp.AnnuityType != TimePeriod.ANNUITY_TYPES.regular)
                            {
                                //keep track of annuities that are used up and need to be removed
                                AddIdToRemovals(ref calcParameters,
                                    opcomp, calcParameters.SubApplicationType.ToString(),
                                    ref removeUsedUpAnnEquivIds);
                            }
                            if ((i != iCount))
                            {
                                //don't accrue when exiting for loop
                                if ((i == (iCount - 1)))
                                {
                                    //last period is next and it may be less than a year
                                    int iLastTimeDays = (int)dbLastTimeDays;
                                    spanDays = new TimeSpan(iLastTimeDays, 0, 0, 0);
                                    dtEndOfGrowthPeriodDate = dtEndOfGrowthPeriodDate + spanDays;
                                }
                                else
                                {
                                    spanDays = new TimeSpan(timePeriodDays, 0, 0, 0);
                                    //finish leap years
                                    dtEndOfGrowthPeriodDate = dtEndOfGrowthPeriodDate + spanDays;
                                    dtEndOfGrowthPeriodDate = AdjustDateForLeapYear(dtEndOfGrowthPeriodDate);
                                }
                            }
                        }
                    }
                    bIsAdded = true;
                    //remove annuities from xmlAnnequivs when they are used up
                    //i.e. in the case of a preproduction annuity, when production periods
                    //are finished
                    RemoveAEsFromXMLAnnEquiv(ref calcParameters,
                        calcParameters.SubApplicationType.ToString(), removeUsedUpAnnEquivIds);
                }
            }
            else
            {
                bIsAdded = false;
            }
            return bIsAdded;
        }
        /// <summary>
        /// Adjust end of year date so that it always ends on the 31st
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime AdjustDateForLeapYear(DateTime date)
        {
            DateTime newDate = date;
            if (date.Day == 30)
            {
                newDate = date.AddDays(1.00);
            }
            else if (date.Day == 1)
            {
                newDate = date.AddDays(-1.00);
            }
            return newDate;
        }
        //add annuity ids to removal list if they have already been fully discounted
        public void AddIdToRemovals(ref CalculatorParameters calcParameters,
            OperationComponent opcomp, string subApplicationType, 
            ref List<int> removeUsedUpAnnEquivIds)
        {
            int iCompAEId = 0;
            int iStart = 0;
            double dbEffLife = 0;
            int iCount = 0;
            iCompAEId = opcomp.Id;
            iStart = opcomp.AnnuityCount;
            dbEffLife = opcomp.EffectiveLife;
            iCount = CalculatorHelpers.ConvertStringToInt(dbEffLife.ToString());
            //Rule 1. rounding down not allowed - will always use that last fractional year
            if (dbEffLife > iCount)
            {
                iCount = iCount + 1;
            }
            //if not finished w/ ae exit
            if (dbEffLife > CalculatorHelpers.ConvertStringToDouble(iStart.ToString()))
            {
                return;
            }
            if (iStart > iCount)
            {
                //the annuity has been fully discounted in the cash flows
                //a) add the id to the removal list
                removeUsedUpAnnEquivIds.Add(iCompAEId);
            }
        }
        /// <summary>
        /// Purpose:   remove ann equiv nodes after the sum of their discounted cash flows equals their starting principal
        ///				keeps the totals and annuals columns in synch
        ///	Output is a collection of annuities that still get added to subsequent time periods
        /// </summary>
        public void RemoveAEsFromXMLAnnEquiv(ref CalculatorParameters calcParameters,
            string subApplicationType, List<int> removeUsedUpAnnEquivIds)
        {
            if (calcParameters.ParentBudgetInvestment.AnnEquivalents != null)
            {
                foreach (int OpCompId in removeUsedUpAnnEquivIds)
                {
                    OperationComponent removeOpComp
                        = calcParameters.ParentBudgetInvestment.AnnEquivalents.FirstOrDefault(
                            c => c.Id == OpCompId);
                    if (removeOpComp != null)
                    {
                        //remove any annuities that have been fully discounted in the cash flows
                        calcParameters.ParentBudgetInvestment.AnnEquivalents.Remove(removeOpComp);
                    }
                }
            }
        }
        
        /// <summary>
        /// Add child nodes to an annual equivalent node to clean up time period nodes
        /// </summary>
        public void AddAnnualEquivalentsChildren(ref CalculatorParameters calcParameters,
            string subApplicationType, OperationComponent opcomp, string seriesType,
            string compName, int timeId, DateTime endOfGrowthPeriodDate, double aePrincipal,
            double aeInterest, double aeDiscPrincipal, double aeDiscPrincipal2, int annCounter, double annuityCount,
            double salvValue, double aohAmount, bool insertAllAEs)
        {
            OperationComponent OC = new OperationComponent(calcParameters, opcomp);
            Input input = new Input();
            OC.TimePeriodId = timeId;
            OC.AnnuityType = TimePeriod.ANNUITY_TYPES.regular;
            OC.Id = CalculatorHelpers.GetRandomInteger(calcParameters);
            OC.Name = compName;
            OC.Label = BudgetInvestment.ANNUITY;
            OC.EffectiveLife = annuityCount;
            OC.SalvageValue = salvValue;
            OC.Unit = BudgetInvestment.ANNUITY;
            //better to do the annual column here
            OC.TotalAMAOH = aeDiscPrincipal;
            OC.TotalAMINCENT = aeDiscPrincipal2;
            OC.TotalAMOC = 0;
            OC.TotalAMCAP = 0;
            OC.TotalAMAOH_INT = aeInterest;
            OC.TotalAMOC_INT = 0;
            OC.TotalAMCAP_INT = 0;
            //annuities never added to regular totals columns rule enforced
            OC.TotalOC = 0;
            OC.TotalAOH = 0;
            OC.TotalCAP = 0;
            OC.TotalOC_INT = 0;
            OC.TotalAOH_INT = 0;
            OC.TotalCAP_INT = 0;
            OC.Amount = 1;
            //don't change this DateTime when it gets added to tps, needed in calcinputprice
            OC.Date = endOfGrowthPeriodDate;
            OC.IncentiveRate = 0;
            OC.IncentiveAmount = 0;
            //set the input properties
            input.Id = CalculatorHelpers.GetRandomInteger(calcParameters);
            input.OpCompId = OC.Id;
            input.AnnuityType = OC.AnnuityType;
            if ((annCounter == 1)
                && (OC.AnnuityType == TimePeriod.ANNUITY_TYPES.regular))
            {
                input.Name = CAP_RECOVERY_I_ONLY;
            }
            else
            {
                input.Name = CAP_RECOVERY_PANDI;
            }
            input.Label = BudgetInvestment.ANNUITY;
            //displays the preproduction sum being amortized
            input.AOHAmount = aohAmount;
            //the sum being amortized - for display only
            input.AOHPrice = aePrincipal;
            input.AOHUnit = BudgetInvestment.ANNUITY;
            //it will get summed correctly from input on up through tree
            input.Date = endOfGrowthPeriodDate;
            input.TotalOC = 0;
            input.TotalAOH = 0;
            input.TotalCAP = 0;
            input.TotalOC_INT = 0;
            input.TotalAOH_INT = 0;
            input.TotalCAP_INT = 0;
            input.TotalAMOC_INT = 0;
            input.TotalAMAOH_INT = aeInterest;
            input.TotalAMCAP_INT = 0;
            input.TotalAMOC = 0;
            input.TotalAMAOH = aeDiscPrincipal;
            input.TotalAMCAP = 0;
            input.CAPAmount = 0;
            input.CAPPrice = 0;
            input.OCAmount = 0;
            input.OCPrice = 0;
            input.OCUnit = BudgetInvestment.ANNUITY;
            input.CAPUnit = BudgetInvestment.ANNUITY;
            input.UseAOH = false;
            input.Times = 0;
            input.InputGroupId = (int) TimePeriod.ANNUITY_TYPES.regular;
            input.InputGroupName = seriesType;
            input.TypeId = (int) TimePeriod.ANNUITY_TYPES.regular;
            input.Name = BudgetInvestment.ANNUITY;
            if (OC.Inputs == null)
                OC.Inputs = new List<Input>();
            OC.Inputs.Add(input);
            //note that the calculations get serialized to xml using 
            //the TimePeriod.OpComps collection (not the AnnEquiv collection)
            if (calcParameters.ParentTimePeriod.OperationComponents == null)
                calcParameters.ParentTimePeriod.OperationComponents = new List<OperationComponent>();
            //cumulative totals are added using AddAnnuitiesToTimePeriodTotals
            calcParameters.ParentTimePeriod.OperationComponents.Add(OC);
        }
        private void AddTimePeriodTotalsToTimePeriodsCollection(
            CalculatorParameters calcParameters)
        {
            //add this timeperiod's totals to the corresponding member of the 
            //ParentBudgetInvestment.TimePeriods collection
            //(a currentTimePeriod sometimes uses other timeperiod calcs)
            BudgetInvestment.AddTimePeriodTotalsToTimePeriodsCollection(
                calcParameters, calcParameters.ParentTimePeriod);
        }
        private void AddGrowthPeriods(CalculatorParameters calcParameters)
        {
            //if this timeperiod has growthperiods > 1 add another timeperiod
            //to ParentBudgetInvestment.TimePeriods with the growth period
            //calculations stored in the the new TimePeriod.Outputs and 
            //TimePeriod.OpComps collections 
            if (calcParameters.ParentTimePeriod.GrowthPeriods > 1)
            {
                //reset calcParameters.ParentTimePeriod so that 
                //cumulative totals can be added using regular SetTotals functions
                int iCurrentTpIndex = BudgetInvestment.GetTimePeriodIndex(
                    calcParameters, calcParameters.ParentTimePeriod);
                calcParameters.ParentTimePeriod = new TimePeriod(calcParameters.ParentTimePeriod);
                bool bRetVal = AddNewGrowthPeriods(calcParameters);
                //insert the time series node right after this one
                if (bRetVal == true)
                {
                    calcParameters.ParentBudgetInvestment.TimePeriods
                        .Insert(iCurrentTpIndex + 1, calcParameters.ParentTimePeriod);
                    //reset calcParameters.ParentTimePeriod to original tp
                    calcParameters.ParentTimePeriod 
                        = calcParameters.ParentBudgetInvestment.TimePeriods[iCurrentTpIndex];
                }
            }
        }
        /// <summary>
        /// Add a growth period for adding any growth series into operating and capital budgets
        /// Output is a new node added to document with correct uniform series sums.
        /// </summary>
        public bool AddNewGrowthPeriods(CalculatorParameters calcParameters)
        {
            bool bIsAdded = false;
            double dbTimePrice1Total = 0;
            double dbTimePrice2Total = 0;
            double dbTimePrice3Total = 0;
            double dbTimePrice4Total = 0;
            double dbTimeAmort1Total = 0;
            double dbTimeAmort2Total = 0;
            double dbTimeAmort3Total = 0;
            double dbTimeAmort4Total = 0;
            double dbTimeAmort1GPTotal = 0;
            double dbTimeAmort2GPTotal = 0;
            double dbTimeAmort3GPTotal = 0;
            double dbTimeAmort4GPTotal = 0;
            double dbTimeAmort1IntTotal = 0;
            double dbTimeAmort2IntTotal = 0;
            double dbTimeAmort3IntTotal = 0;
            double dbTimeAmort4IntTotal = 0;
            double dbTimePrice1GPTotal = 0;
            double dbTimePrice2GPTotal = 0;
            double dbTimePrice3GPTotal = 0;
            double dbTimePrice4GPTotal = 0;
            double dbTimeInterest1GPTotal = 0;
            double dbTimeInterest2GPTotal = 0;
            double dbTimeInterest3GPTotal = 0;
            double dbTimeInterest4GPTotal = 0;
            double dbLastTimePrice1Total = 0;
            double dbLastTimePrice2Total = 0;
            double dbLastTimePrice3Total = 0;
            double dbLastTimePrice4Total = 0;
            double dbLastTimeAmort1Total = 0;
            double dbLastTimeAmort2Total = 0;
            double dbLastTimeAmort3Total = 0;
            double dbLastTimeAmort4Total = 0;
            int iTimePeriodDays = 0;
            System.TimeSpan spanTimePeriodDays;
            System.TimeSpan spanLeapYearDay = new TimeSpan(1, 0, 0, 0);
            double dbDiscountFactor = 1;
            double dbFractDiscFactor = 1;
            DateTime dtEOGrowthPeriodDate;
            bool bDiscountYorN;
            int i = 0;
            int iCount = 0;
            string sName;
            int iGrowthPeriods = 0;
            int iTimeId = 0;
            //set to timedate in case they are running this from one time period
            DateTime dtLastDate;
            TimePeriod.GROWTH_SERIES_TYPES eGrowthType;
            string sSeriesType = string.Empty;
            double dbGradientP1Factor = 0;
            double dbGradientP2Factor = 0;
            double dbGradientP3Factor = 0;
            double dbGradientP4Factor = 0;
            double dbInitDiscountPeriod = 0;
            System.TimeSpan spanDiscountDays;
            double dbPrice1Adjustment = 0;
            double dbPrice2Adjustment = 0;
            double dbPrice3Adjustment = 0;
            double dbPrice4Adjustment = 0;
            int iAnnuityCounter;
            //initialize the dbrent tps variables being summed over growth periods
            //interest added to each total so that the starting sum is not a discounted starting sum -nested series need this
            //the time element contains discounted sums - subtract out the interest so that growth is based strictly on principal
            dbTimePrice1Total = calcParameters.ParentTimePeriod.TotalOC - calcParameters.ParentTimePeriod.TotalOC_INT;
            dbTimePrice2Total = calcParameters.ParentTimePeriod.TotalAOH - calcParameters.ParentTimePeriod.TotalAOH_INT;
            dbTimePrice3Total = calcParameters.ParentTimePeriod.TotalCAP - calcParameters.ParentTimePeriod.TotalCAP_INT;
            dbTimePrice4Total = calcParameters.ParentTimePeriod.TotalR - calcParameters.ParentTimePeriod.TotalR_INT;
            dbTimeAmort1Total = calcParameters.ParentTimePeriod.TotalAMOC - calcParameters.ParentTimePeriod.TotalAMOC_INT;
            dbTimeAmort2Total = calcParameters.ParentTimePeriod.TotalAMAOH - calcParameters.ParentTimePeriod.TotalAMAOH_INT;
            dbTimeAmort3Total = calcParameters.ParentTimePeriod.TotalAMCAP - calcParameters.ParentTimePeriod.TotalAMCAP_INT;
            //version 0.9.8: TAMR is just equal to TR + TR_INT; keep it uniform with TR by using TR_INT, no TAMR_INT
            dbTimeAmort4Total = calcParameters.ParentTimePeriod.TotalAMR - calcParameters.ParentTimePeriod.TotalR_INT;
            dbTimePrice1GPTotal = 0;
            dbTimePrice2GPTotal = 0;
            dbTimePrice3GPTotal = 0;
            dbTimePrice4GPTotal = 0;
            bDiscountYorN = calcParameters.ParentTimePeriod.IsDiscounted;
            eGrowthType = (TimePeriod.GROWTH_SERIES_TYPES)calcParameters.ParentTimePeriod.GrowthTypeId;

            //Rule 4a. remove first year newly amortized regular annuities from totals column, in this case year 1 aes
            iAnnuityCounter = 1;
            AdjustGrowthPeriodAnnuities(calcParameters, iAnnuityCounter,
                ref dbPrice1Adjustment, ref dbPrice2Adjustment, ref dbPrice3Adjustment, ref dbPrice4Adjustment);
            dbTimePrice1Total = dbTimePrice1Total - dbPrice1Adjustment;
            dbTimePrice2Total = dbTimePrice2Total - dbPrice2Adjustment;
            dbTimePrice3Total = dbTimePrice3Total - dbPrice3Adjustment;
            dbTimePrice4Total = dbTimePrice4Total - dbPrice4Adjustment;
            //the only difference between the two columns at this point is any annuities in column 2
            //so remove them
            dbTimeAmort2Total = dbTimeAmort2Total - (dbTimeAmort2Total - dbTimePrice2Total);

            //initialize last tps totals for gradient series
            TimePeriod lastTimePeriod = BudgetInvestment.GetLastTimePeriod(
                calcParameters, calcParameters.ParentTimePeriod);
            //handle no last period if this is a Time Period document
            if (lastTimePeriod != null)
            {
                dtLastDate = lastTimePeriod.Date;
                //base changes just on principal, not discounted principal
                dbLastTimePrice1Total = lastTimePeriod.TotalOC - lastTimePeriod.TotalOC_INT;
                dbLastTimePrice2Total = lastTimePeriod.TotalAOH - lastTimePeriod.TotalAOH_INT;
                dbLastTimePrice3Total = lastTimePeriod.TotalCAP- lastTimePeriod.TotalCAP_INT;
                dbLastTimePrice4Total = lastTimePeriod.TotalR - lastTimePeriod.TotalR_INT;
                dbLastTimeAmort1Total = lastTimePeriod.TotalAMOC - lastTimePeriod.TotalAMOC_INT;
                dbLastTimeAmort2Total = lastTimePeriod.TotalAMAOH - lastTimePeriod.TotalAMAOH_INT;
                dbLastTimeAmort3Total = lastTimePeriod.TotalAMCAP - lastTimePeriod.TotalAMCAP_INT;
                dbLastTimeAmort4Total = lastTimePeriod.TotalAMR - lastTimePeriod.TotalAMR_INT;
            }
            else
            {
                dtLastDate = calcParameters.ParentTimePeriod.Date;
            }
            //Rule 4b. remove first year newly amortized regular annuities, in this case year 2 aes
            dbPrice1Adjustment = 0;
            dbPrice2Adjustment = 0;
            dbPrice3Adjustment = 0;
            dbPrice4Adjustment = 0;
            iAnnuityCounter = 2;
            AdjustGrowthPeriodAnnuities(calcParameters, iAnnuityCounter,
                ref dbPrice1Adjustment, ref dbPrice2Adjustment, ref dbPrice3Adjustment, ref dbPrice4Adjustment);
            dbLastTimePrice1Total = dbLastTimePrice1Total - dbPrice1Adjustment;
            dbLastTimePrice2Total = dbLastTimePrice2Total - dbPrice2Adjustment;
            dbLastTimePrice3Total = dbLastTimePrice3Total - dbPrice3Adjustment;
            dbLastTimePrice4Total = dbLastTimePrice4Total - dbPrice4Adjustment;
            if (eGrowthType == TimePeriod.GROWTH_SERIES_TYPES.uniform)
            {
                //Rule 1. make it a uniform series
                sSeriesType = TimePeriod.GROWTH_SERIES_TYPES.uniform.ToString();
                dbGradientP1Factor = 1;
                dbGradientP2Factor = 1;
                dbGradientP3Factor = 1;
                dbGradientP4Factor = 1;
            }
            else if (eGrowthType == TimePeriod.GROWTH_SERIES_TYPES.linear)
            {
                sSeriesType = TimePeriod.GROWTH_SERIES_TYPES.linear.ToString();
                //Rule 2. initial gradient sum is the difference between totals for last 2 periods
                //sum this difference to each time period's uniform series and multiple by the iCount
                dbGradientP1Factor = (dbTimePrice1Total - dbLastTimePrice1Total);
                dbGradientP2Factor = (dbTimePrice2Total - dbLastTimePrice2Total);
                dbGradientP3Factor = (dbTimePrice3Total - dbLastTimePrice3Total);
                dbGradientP4Factor = (dbTimePrice4Total - dbLastTimePrice4Total);
            }
            else if (eGrowthType == TimePeriod.GROWTH_SERIES_TYPES.geometric)
            {
                sSeriesType = TimePeriod.GROWTH_SERIES_TYPES.geometric.ToString();
                //Rule 3. initial gradient percentage is the percentage difference between totals for last 2 periods
                //multiply each subs period's uniform series by 1 + percent
                //no zeros allowed
                if (dbLastTimePrice1Total == 0) dbLastTimePrice1Total = 1;
                if (dbLastTimePrice2Total == 0) dbLastTimePrice2Total = 1;
                if (dbLastTimePrice3Total == 0) dbLastTimePrice3Total = 1;
                if (dbLastTimePrice4Total == 0) dbLastTimePrice4Total = 1;
                dbGradientP1Factor = 1 + ((dbTimePrice1Total - dbLastTimePrice1Total) / dbLastTimePrice1Total);
                dbGradientP2Factor = 1 + ((dbTimePrice2Total - dbLastTimePrice2Total) / dbLastTimePrice2Total);
                dbGradientP3Factor = 1 + ((dbTimePrice3Total - dbLastTimePrice3Total) / dbLastTimePrice3Total);
                dbGradientP4Factor = 1 + ((dbTimePrice4Total - dbLastTimePrice4Total) / dbLastTimePrice4Total);
            }

            //Rule 5. last time period accurately reflects number of days in each growth time period
            spanTimePeriodDays = calcParameters.ParentTimePeriod.Date - dtLastDate;
            dtEOGrowthPeriodDate = calcParameters.ParentTimePeriod.Date + spanTimePeriodDays;
            spanDiscountDays = dtEOGrowthPeriodDate - calcParameters.ParentBudgetInvestment.InitEOPDate;
            dbInitDiscountPeriod = spanDiscountDays.Days;
            iGrowthPeriods = calcParameters.ParentTimePeriod.GrowthPeriods;
            //leap year
            dtEOGrowthPeriodDate = AdjustDateForLeapYear(dtEOGrowthPeriodDate);
            calcParameters.ParentTimePeriod.Id = CalculatorHelpers.GetRandomInteger(calcParameters);
            calcParameters.ParentTimePeriod.AnnuityType = TimePeriod.ANNUITY_TYPES.regular;
            calcParameters.ParentTimePeriod.IsDiscounted = bDiscountYorN;
            calcParameters.ParentTimePeriod.GrowthPeriods = 0;
            calcParameters.ParentTimePeriod.GrowthTypeId = 0;
            calcParameters.ParentTimePeriod.IsCommonReference = false;
            calcParameters.ParentTimePeriod.OverheadFactor = 1;
            calcParameters.ParentTimePeriod.IncentiveAmount = 0;
            calcParameters.ParentTimePeriod.IncentiveRate = 0;
            calcParameters.ParentTimePeriod.Name2 = string.Concat(calcParameters.ParentTimePeriod.Name2,
                    SPACE, OVER, SPACE, iGrowthPeriods, MOREPERIODS);
            calcParameters.ParentTimePeriod.Unit = sSeriesType;
            calcParameters.ParentTimePeriod.Label = BudgetInvestment.ANNUITY;
            for (i = 1; i <= iGrowthPeriods; i++)
            {
                iCount = iCount + 1;
                if (bDiscountYorN == true)
                {
                    if (dbInitDiscountPeriod <= 31)
                    {
                        //use monthly compounding
                        dbDiscountFactor 
                            = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                                calcParameters.ParentBudgetInvestment.Local.RealRate, 
                                calcParameters.ParentBudgetInvestment.InitEOPDate, 
                                dtEOGrowthPeriodDate, true);
                    }
                    else
                    {
                        //use annual compounding
                        dbDiscountFactor 
                            = GeneralRules.CalculateDiscountFactorByMonthsorYears(
                            calcParameters.ParentBudgetInvestment.Local.RealRate, 
                            calcParameters.ParentBudgetInvestment.InitEOPDate, 
                            dtEOGrowthPeriodDate, false);
                    }
                }
                else
                {
                    //don't discount
                    dbDiscountFactor = 1;
                }
                //use fractional during last time periods; this is often equal to one
                if ((i == iGrowthPeriods))
                {
                    if ((dbFractDiscFactor > 0.9990) && (dbFractDiscFactor <= 1.00))
                    {
                        //its equivalent to a 1; use the existing discount factor
                    }
                    else
                    {
                        dbDiscountFactor = dbFractDiscFactor;
                    }
                }
                if (eGrowthType == TimePeriod.GROWTH_SERIES_TYPES.uniform)
                {
                    dbTimeInterest1GPTotal = (((dbTimePrice1Total * dbGradientP1Factor) - ((dbTimePrice1Total * dbGradientP1Factor) * dbDiscountFactor)) * -1);
                    dbTimeInterest2GPTotal = (((dbTimePrice2Total * dbGradientP2Factor) - ((dbTimePrice2Total * dbGradientP2Factor) * dbDiscountFactor)) * -1);
                    dbTimeInterest3GPTotal = (((dbTimePrice3Total * dbGradientP3Factor) - ((dbTimePrice3Total * dbGradientP3Factor) * dbDiscountFactor)) * -1);
                    dbTimeInterest4GPTotal = (((dbTimePrice4Total * dbGradientP4Factor) - ((dbTimePrice4Total * dbGradientP4Factor) * dbDiscountFactor)) * -1);
                    dbTimePrice1GPTotal = (dbTimePrice1Total * dbGradientP1Factor);
                    dbTimePrice2GPTotal = (dbTimePrice2Total * dbGradientP2Factor);
                    dbTimePrice3GPTotal = (dbTimePrice3Total * dbGradientP3Factor);
                    dbTimePrice4GPTotal = (dbTimePrice4Total * dbGradientP4Factor);
                    dbTimeAmort1IntTotal = (((dbTimeAmort1Total * dbGradientP1Factor) - ((dbTimeAmort1Total * dbGradientP1Factor) * dbDiscountFactor)) * -1);
                    dbTimeAmort1GPTotal = (dbTimeAmort1Total * dbGradientP1Factor);
                    dbTimeAmort2IntTotal = (((dbTimeAmort2Total * dbGradientP2Factor) - ((dbTimeAmort2Total * dbGradientP2Factor) * dbDiscountFactor)) * -1);
                    dbTimeAmort2GPTotal = (dbTimeAmort2Total * dbGradientP2Factor);
                    dbTimeAmort3IntTotal = (((dbTimeAmort3Total * dbGradientP3Factor) - ((dbTimeAmort3Total * dbGradientP3Factor) * dbDiscountFactor)) * -1);
                    dbTimeAmort3GPTotal = (dbTimeAmort3Total * dbGradientP3Factor);
                    dbTimeAmort4IntTotal = (((dbTimeAmort4Total * dbGradientP4Factor) - ((dbTimeAmort4Total * dbGradientP4Factor) * dbDiscountFactor)) * -1);
                    dbTimeAmort4GPTotal = (dbTimeAmort4Total * dbGradientP4Factor);
                }
                else if (eGrowthType == TimePeriod.GROWTH_SERIES_TYPES.geometric)
                {
                    dbTimeInterest1GPTotal = (((dbTimePrice1Total * (System.Math.Pow(dbGradientP1Factor, (iCount - 1)))) - ((dbTimePrice1Total * (System.Math.Pow(dbGradientP1Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeInterest2GPTotal = (((dbTimePrice2Total * (System.Math.Pow(dbGradientP2Factor, (iCount - 1)))) - ((dbTimePrice2Total * (System.Math.Pow(dbGradientP2Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeInterest3GPTotal = (((dbTimePrice3Total * (System.Math.Pow(dbGradientP3Factor, (iCount - 1)))) - ((dbTimePrice3Total * (System.Math.Pow(dbGradientP3Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeInterest4GPTotal = (((dbTimePrice4Total * (System.Math.Pow(dbGradientP4Factor, (iCount - 1)))) - ((dbTimePrice4Total * (System.Math.Pow(dbGradientP4Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimePrice1GPTotal = (dbTimePrice1Total * (System.Math.Pow(dbGradientP1Factor, (iCount - 1))));
                    dbTimePrice2GPTotal = (dbTimePrice2Total * (System.Math.Pow(dbGradientP2Factor, (iCount - 1))));
                    dbTimePrice3GPTotal = (dbTimePrice3Total * (System.Math.Pow(dbGradientP3Factor, (iCount - 1))));
                    dbTimePrice4GPTotal = (dbTimePrice4Total * (System.Math.Pow(dbGradientP4Factor, (iCount - 1))));
                    dbTimeAmort1IntTotal = (((dbTimeAmort1Total * (System.Math.Pow(dbGradientP1Factor, (iCount - 1)))) - ((dbTimeAmort1Total * (System.Math.Pow(dbGradientP1Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeAmort1GPTotal = (dbTimeAmort1Total * (System.Math.Pow(dbGradientP1Factor, (iCount - 1))));
                    dbTimeAmort2IntTotal = (((dbTimeAmort2Total * (System.Math.Pow(dbGradientP2Factor, (iCount - 1)))) - ((dbTimeAmort2Total * (System.Math.Pow(dbGradientP2Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeAmort2GPTotal = (dbTimeAmort2Total * (System.Math.Pow(dbGradientP2Factor, (iCount - 1))));
                    dbTimeAmort3IntTotal = (((dbTimeAmort3Total * (System.Math.Pow(dbGradientP3Factor, (iCount - 1)))) - ((dbTimeAmort3Total * (System.Math.Pow(dbGradientP3Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeAmort3GPTotal = (dbTimeAmort3Total * (System.Math.Pow(dbGradientP3Factor, (iCount - 1))));
                    dbTimeAmort4IntTotal = (((dbTimeAmort4Total * (System.Math.Pow(dbGradientP4Factor, (iCount - 1)))) - ((dbTimeAmort4Total * (System.Math.Pow(dbGradientP4Factor, (iCount - 1)))) * dbDiscountFactor)) * -1);
                    dbTimeAmort4GPTotal = (dbTimeAmort4Total * (System.Math.Pow(dbGradientP4Factor, (iCount - 1))));
                }
                else if (eGrowthType == TimePeriod.GROWTH_SERIES_TYPES.linear)
                {
                    dbTimeInterest1GPTotal = (((dbTimePrice1Total + (iCount * dbGradientP1Factor)) - ((dbTimePrice1Total + (iCount * dbGradientP1Factor)) * dbDiscountFactor)) * -1);
                    dbTimeInterest2GPTotal = (((dbTimePrice2Total + (iCount * dbGradientP2Factor)) - ((dbTimePrice2Total + (iCount * dbGradientP2Factor)) * dbDiscountFactor)) * -1);
                    dbTimeInterest3GPTotal = (((dbTimePrice3Total + (iCount * dbGradientP3Factor)) - ((dbTimePrice3Total + (iCount * dbGradientP3Factor)) * dbDiscountFactor)) * -1);
                    dbTimeInterest4GPTotal = (((dbTimePrice4Total + (iCount * dbGradientP4Factor)) - ((dbTimePrice4Total + (iCount * dbGradientP4Factor)) * dbDiscountFactor)) * -1);
                    dbTimePrice1GPTotal = (dbTimePrice1Total + (iCount * dbGradientP1Factor));
                    dbTimePrice2GPTotal = (dbTimePrice2Total + (iCount * dbGradientP2Factor));
                    dbTimePrice3GPTotal = (dbTimePrice3Total + (iCount * dbGradientP3Factor));
                    dbTimePrice4GPTotal = (dbTimePrice4Total + (iCount * dbGradientP4Factor));
                    //the only thing in annuity column that should be discounted are any annuities held in col aoh
                    dbTimeAmort1IntTotal = (((dbTimeAmort1Total + (iCount * dbGradientP1Factor)) - ((dbTimeAmort1Total + (iCount * dbGradientP1Factor)) * dbDiscountFactor)) * -1);
                    dbTimeAmort1GPTotal = ((dbTimeAmort1Total + (iCount * dbGradientP1Factor)));
                    dbTimeAmort2IntTotal = (((dbTimeAmort2Total + (iCount * dbGradientP2Factor)) - ((dbTimeAmort2Total + (iCount * dbGradientP2Factor)) * dbDiscountFactor)) * -1);
                    dbTimeAmort2GPTotal = (dbTimeAmort2Total + (iCount * dbGradientP2Factor));
                    dbTimeAmort3IntTotal = (((dbTimeAmort3Total + (iCount * dbGradientP3Factor)) - ((dbTimeAmort3Total + (iCount * dbGradientP3Factor)) * dbDiscountFactor)) * -1);
                    dbTimeAmort3GPTotal = (dbTimeAmort3Total + (iCount * dbGradientP3Factor));
                    dbTimeAmort4IntTotal = (((dbTimeAmort4Total + (iCount * dbGradientP4Factor)) - ((dbTimeAmort4Total + (iCount * dbGradientP4Factor)) * dbDiscountFactor)) * -1);
                    dbTimeAmort4GPTotal = (dbTimeAmort4Total + (iCount * dbGradientP4Factor));
                }
                sName = "Period " + i;
                AddGrowthOutputChildren(calcParameters,
                    sSeriesType, sName, iTimeId,
                    dtEOGrowthPeriodDate, dbTimePrice4GPTotal, dbTimeInterest4GPTotal,
                    dbTimeAmort4GPTotal, dbTimeAmort4IntTotal);
                AddGrowthInputChildren(calcParameters,
                    sSeriesType, sName, iTimeId,
                    dtEOGrowthPeriodDate, dbTimePrice1GPTotal, dbTimeInterest1GPTotal, dbTimePrice2GPTotal, dbTimeInterest2GPTotal, dbTimePrice3GPTotal,
                    dbTimeInterest3GPTotal, dbTimePrice4GPTotal, dbTimeInterest4GPTotal,
                    dbTimeAmort1GPTotal, dbTimeAmort1IntTotal, dbTimeAmort2GPTotal, dbTimeAmort2IntTotal, dbTimeAmort3GPTotal, dbTimeAmort3IntTotal);
                //insert a collection of annuities for each tp
                bool bInsertAllAEs = false;
                AddAnnualEquivalents2(calcParameters, dtEOGrowthPeriodDate, 
                    iTimeId, dbInitDiscountPeriod, ref dbTimeAmort2GPTotal, 
                    ref dbTimeAmort2IntTotal, iTimePeriodDays,
                    bDiscountYorN, bInsertAllAEs);
                dtEOGrowthPeriodDate = dtEOGrowthPeriodDate + spanTimePeriodDays;
                //leap year
                dtEOGrowthPeriodDate = AdjustDateForLeapYear(dtEOGrowthPeriodDate);
                //refactor? alpha 4b used integers for growth periods so no fractional calcs
                //if ((i == (iGrowthPeriods - 1)))
                //{
                //    //adjust the last time period to account for fractional time periods
                //    //dbFractDiscFactor
                //    //    = GeneralRules.CalculateFractionalPaymentFactor(
                //    //    calcParameters.ParentBudgetInvestment.Local.RealRate,
                //    //    iGrowthPeriods, iGrowthPeriods, dbInitDiscountPeriod, ref dbLastTimeDays);
                //    //spanLastTimeDays = new TimeSpan(GenHelpers.ConvertStringToInt(dbLastTimeDays.ToString()), 0, 0, 0);
                //    //dtEOGrowthPeriodDate = dtEOGrowthPeriodDate + spanLastTimeDays;
                //}
                //else
                //{
                //    //keep accruing the growth period DateTime used when discounting
                //    dtEOGrowthPeriodDate = dtEOGrowthPeriodDate + spanTimePeriodDays;
                //    //leap year
                //    dtEOGrowthPeriodDate = AdjustDateForLeapYear(dtEOGrowthPeriodDate);
                //}
            }
            dtEOGrowthPeriodDate = dtEOGrowthPeriodDate - spanTimePeriodDays;
            calcParameters.ParentTimePeriod.Date = dtEOGrowthPeriodDate;
            calcParameters.ParentTimePeriod.Name = string.Concat(PLUS, "- ", iGrowthPeriods.ToString());
            //add cumulative totals
            InitTimperiodTotalsForAnnuities(calcParameters);
            AddAnnuitiesToTimePeriodTotals(calcParameters);
            AddAnnuitiesToBudgetTotals(calcParameters);
            bIsAdded = true;
            return bIsAdded;
        }
        /// <summary>
        /// Removes newly amortized annual equivalent principal from growth 
        /// //(i.e. don't add a cow costing $1050 thats being amortized, to the growth flows)
        /// Output is the sum of the principals to be removed and the ref parameters
        /// </summary>
        public double AdjustGrowthPeriodAnnuities(CalculatorParameters calcParameters,
            int count, ref double price1Adjustment, ref double price2Adjustment,
            ref double price3Adjustment, ref double price4Adjustment)
        {
            double dbAdjustedAnnuity = 0;
            int iAnnuityCounter = 0;
            if (calcParameters.ParentBudgetInvestment.AnnEquivalents == null)
            {
                return dbAdjustedAnnuity;
            }
            foreach (OperationComponent opComp 
                in calcParameters.ParentBudgetInvestment.AnnEquivalents)
            {
                iAnnuityCounter = opComp.AnnuityCount;
                if ((iAnnuityCounter == count))
                {
                    //add this annuity's amortized amount to the value being returned
                    Input firstInput = (opComp.Inputs != null) ?
                        opComp.Inputs.FirstOrDefault() : null;
                    if (firstInput != null)
                    {
                        price1Adjustment += firstInput.TotalOC + firstInput.TotalOC_INT;
                        price2Adjustment += firstInput.TotalAOH + firstInput.TotalAOH_INT;
                        price3Adjustment += firstInput.TotalCAP + firstInput.TotalCAP_INT;
                        price4Adjustment += firstInput.TotalR + firstInput.TotalR_INT;
                    }
                }
            }
            return dbAdjustedAnnuity;
        }
        /// <summary>
        /// Add child nodes to new growth period nodes.
        /// The growth period gets 1 component or operation, output, and input node added
        /// </summary>
        public void AddGrowthInputChildren(CalculatorParameters calcParameters,
            string seriesType, string compName, int timeId,
            DateTime endOfGrowthPeriodDate, double timePrice1GPTotal,
            double timeInterest1GPTotal, double timePrice2GPTotal,
            double timeInterest2GPTotal, double timePrice3GPTotal,
            double timeInterest3GPTotal, double timePrice4GPTotal,
            double timeInterest4GPTotal, double timeAmort1GPTotal,
            double timeAmort1IntTotal, double timeAmort2GPTotal,
            double timeAmort2IntTotal, double timeAmort3GPTotal,
            double timeAmort3IntTotal)
        {
            //use ancestor objects for accumulating totals
            calcParameters.ParentOperationComponent = new OperationComponent();
            Input newInput = new Input();
            int iCompAEId = INT_SEED;
            int iInputAEId = INT_SEED;
            iInputAEId = CalculatorHelpers.GetRandomInteger(calcParameters);
            iCompAEId = CalculatorHelpers.GetRandomInteger(calcParameters);
            calcParameters.ParentOperationComponent.AnnuityType = TimePeriod.ANNUITY_TYPES.regular;
            calcParameters.ParentOperationComponent.Id = iCompAEId;
            calcParameters.ParentOperationComponent.Name = compName;
            calcParameters.ParentOperationComponent.EffectiveLife = 1;
            calcParameters.ParentOperationComponent.SalvageValue = 0;
            calcParameters.ParentOperationComponent.Unit = seriesType;
            //note: interest has already been added to the totals for display
            calcParameters.ParentOperationComponent.TotalAMOC = timeAmort1GPTotal + timeAmort1IntTotal;
            calcParameters.ParentOperationComponent.TotalAMAOH = timeAmort2GPTotal + timeAmort2IntTotal;
            calcParameters.ParentOperationComponent.TotalAMCAP = timeAmort3GPTotal + timeAmort3IntTotal;
            calcParameters.ParentOperationComponent.TotalAMOC_INT = timeAmort1IntTotal;
            calcParameters.ParentOperationComponent.TotalAMAOH_INT = timeAmort2IntTotal;
            calcParameters.ParentOperationComponent.TotalAMCAP_INT = timeAmort3IntTotal;
            //note: interest has already been added to the totals for display
            calcParameters.ParentOperationComponent.TotalOC = timePrice1GPTotal + timeInterest1GPTotal;
            calcParameters.ParentOperationComponent.TotalAOH = timePrice2GPTotal + timeInterest2GPTotal;
            calcParameters.ParentOperationComponent.TotalCAP = timePrice3GPTotal + timeInterest3GPTotal;
            calcParameters.ParentOperationComponent.TotalOC_INT = timeInterest1GPTotal;
            calcParameters.ParentOperationComponent.TotalAOH_INT = timeInterest2GPTotal;
            calcParameters.ParentOperationComponent.TotalCAP_INT = timeInterest3GPTotal;
            calcParameters.ParentOperationComponent.Amount = 1;
            calcParameters.ParentOperationComponent.Date = endOfGrowthPeriodDate;
            calcParameters.ParentOperationComponent.IncentiveRate = 0;
            calcParameters.ParentOperationComponent.IncentiveAmount = 0;
            //set the attributes
            iInputAEId = CalculatorHelpers.GetRandomInteger(calcParameters);
            newInput.AnnuityType = TimePeriod.ANNUITY_TYPES.regular;
            newInput.Id = iInputAEId;
            newInput.Name = Errors.MakeStandardErrorMsg("CALCULATOR_SEELAST_PERIOD");
            newInput.Label = BudgetInvestment.ANNUITY;
            newInput.AOHAmount = 1;
            //the sum being amortized - for display only
            newInput.AOHPrice = timePrice2GPTotal;
            newInput.AOHUnit = seriesType;
            newInput.Date = endOfGrowthPeriodDate;
            newInput.TotalAMOC = timeAmort1GPTotal;
            newInput.TotalAMAOH = timeAmort2GPTotal;
            newInput.TotalAMCAP = timeAmort3GPTotal;
            newInput.TotalAMOC_INT = timeAmort1IntTotal;
            newInput.TotalAMAOH_INT = timeAmort2IntTotal;
            newInput.TotalAMCAP_INT = timeAmort3IntTotal;
            //note: interest has already been added to the totals for display
            newInput.TotalOC = timePrice1GPTotal;
            newInput.TotalAOH = timePrice2GPTotal;
            newInput.TotalCAP = timePrice3GPTotal;
            newInput.TotalOC_INT = timeInterest1GPTotal;
            newInput.TotalAOH_INT = timeInterest2GPTotal;
            newInput.TotalCAP_INT = timeInterest3GPTotal;
            newInput.CAPAmount = 0;
            newInput.CAPPrice = timePrice3GPTotal;
            newInput.OCAmount = 0;
            newInput.OCPrice = timePrice1GPTotal;
            newInput.OCUnit = seriesType;
            newInput.CAPUnit = seriesType;
            newInput.UseAOH = false;
            newInput.Times = 1;
            newInput.InputGroupId = (int)TimePeriod.ANNUITY_TYPES.regular;
            newInput.InputGroupName = seriesType;
            newInput.TypeId = (int)TimePeriod.ANNUITY_TYPES.regular;
            newInput.Name = seriesType;
            if (calcParameters.ParentOperationComponent.Inputs == null)
                calcParameters.ParentOperationComponent.Inputs = new List<Input>();
            //opcomp already has totals
            calcParameters.ParentOperationComponent.Inputs.Add(newInput);
            if (calcParameters.ParentTimePeriod.OperationComponents == null)
                calcParameters.ParentTimePeriod.OperationComponents
                    = new List<OperationComponent>();
            //add cumulative totals
            calcParameters.ParentTimePeriod.OperationComponents.Add(calcParameters.ParentOperationComponent);
        }
        /// <summary>
        /// Add child nodes to new growth period nodes.
        /// The growth period gets 1 component, operation, outcome, output, and input node added
        /// </summary>
        public void AddGrowthOutputChildren(CalculatorParameters calcParameters,
            string seriesType, string compName, int timeId,
            DateTime endOfGrowthPeriodDate, double timePrice4GPTotal,
            double timeInterest4GPTotal, double timeAmort4GPTotal,
            double timeAmort4IntTotal)
        {
            //use ancestor objects for accumulating totals
            calcParameters.ParentOutcome = new Outcome();
            Output newOutput = new Output();
            int iCompAEId = INT_SEED;
            int iOutputAEId = INT_SEED;
            iOutputAEId = CalculatorHelpers.GetRandomInteger(calcParameters);
            iCompAEId = CalculatorHelpers.GetRandomInteger(calcParameters);
            calcParameters.ParentOutcome.AnnuityType = TimePeriod.ANNUITY_TYPES.regular;
            calcParameters.ParentOutcome.Id = iCompAEId;
            calcParameters.ParentOutcome.Name = compName;
            calcParameters.ParentOutcome.EffectiveLife = 1;
            calcParameters.ParentOutcome.SalvageValue = 0;
            calcParameters.ParentOutcome.Unit = seriesType;
            //note: interest has already been added to the totals for display
            calcParameters.ParentOutcome.TotalAMR = timeAmort4GPTotal + timeAmort4IntTotal;
            calcParameters.ParentOutcome.TotalAMR_INT = timeAmort4IntTotal;
            //note: interest has already been added to the totals for display
            calcParameters.ParentOutcome.TotalR = timePrice4GPTotal + timeInterest4GPTotal;
            calcParameters.ParentOutcome.TotalR_INT = timeInterest4GPTotal;
            calcParameters.ParentOutcome.Amount = 1;
            calcParameters.ParentOutcome.Date = endOfGrowthPeriodDate;
            calcParameters.ParentOutcome.IncentiveRate = 0;
            calcParameters.ParentOutcome.IncentiveAmount = 0;
            //set the attributes
            iOutputAEId = CalculatorHelpers.GetRandomInteger(calcParameters);
            newOutput.AnnuityType = TimePeriod.ANNUITY_TYPES.regular;
            newOutput.Id = iOutputAEId;
            newOutput.Name = Errors.MakeStandardErrorMsg("CALCULATOR_SEELAST_PERIOD");
            newOutput.Label = BudgetInvestment.ANNUITY;
            newOutput.Date = endOfGrowthPeriodDate;
            newOutput.TotalAMR = timeAmort4GPTotal;
            newOutput.TotalAMR_INT = timeAmort4IntTotal;
            //note: interest has already been added to the totals for display
            newOutput.TotalR = timePrice4GPTotal;
            newOutput.TotalR_INT = timeInterest4GPTotal;
            newOutput.Amount = 1;
            newOutput.CompositionAmount = 1;
            newOutput.Times = 1;
            newOutput.Price = timePrice4GPTotal;
            newOutput.Unit = seriesType;
            newOutput.OutputGroupId = (int)TimePeriod.ANNUITY_TYPES.regular;
            newOutput.OutputGroupName = seriesType;
            newOutput.TypeId = (int)TimePeriod.ANNUITY_TYPES.regular;
            newOutput.Name = seriesType;
            if (calcParameters.ParentOutcome.Outputs == null)
                calcParameters.ParentOutcome.Outputs = new List<Output>();
            //opcomp already has totals
            calcParameters.ParentOutcome.Outputs.Add(newOutput);
            if (calcParameters.ParentTimePeriod.Outcomes == null)
                calcParameters.ParentTimePeriod.Outcomes
                    = new List<Outcome>();
            //add cumulative totals
            //SetOutcomeCalculations(calcParameters, calcParameters.ParentOutcome);
            calcParameters.ParentTimePeriod.Outcomes.Add(calcParameters.ParentOutcome);
        }
        
        private void CleanUpAnnuities(CalculatorParameters calcParameters)
        {
            //bool bInsertAllAE2s = true;
            TimePeriod newTimePeriod = new TimePeriod();
            bool bRetVal = false;
            //refactor: debug annuities
            //bool bRetVal = AddTimePeriodAnnualEquivalents(calcParameters,
            //    ref newTimePeriod, null, bInsertAllAE2s);
            //append the new node, which cleans up any dangling annuities, to the end of the child nodes list
            if (bRetVal == true)
            {
                if (calcParameters.ParentBudgetInvestment.TimePeriods == null)
                    calcParameters.ParentBudgetInvestment.TimePeriods = new List<TimePeriod>();
                calcParameters.ParentBudgetInvestment.TimePeriods.Add(newTimePeriod);
            }
        }
    }
}
