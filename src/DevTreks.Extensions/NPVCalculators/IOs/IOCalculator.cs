using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    // <summary>
    ///Purpose:		Run discounted input and output calculations
    ///Author:		www.devtreks.org
    ///Date:		2011, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    //NOTES         1. Carry out calculations by deserializing currentCalculationsElement 
    //              and currentElement into an AddInViews.BaseObject and using the object
    //              to run the calculations
    //              2. Serialize the object's new calculations back to 
    //              currentCalculationsElement and currentElement, and fill in 
    //              the updates collection with any db fields that have changed
    //              3. All calculations still must be recalibrated against documented results.
    /// </summary>
    public class IOCalculator
    {
        public bool SetInputOutputCalculations(
            CalculatorHelpers.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            switch (calculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.input:
                    InputDiscounted inputDiscounted = new InputDiscounted();
                    //deserialize xml to object
                    inputDiscounted.SetInputDiscountedProperties(
                        calcParameters, currentCalculationsElement, currentElement);
                    //set up and run and the calculations
                    bHasCalculations = RunInputCalculations(inputDiscounted,
                        calcParameters);
                    //serialize object back to xml
                    inputDiscounted.SetInputDiscountedAttributes(
                        calcParameters, ref currentCalculationsElement,
                        ref currentElement, updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.output:
                     OutputDiscounted outputDiscounted = new OutputDiscounted();
                    //deserialize xml to object
                    outputDiscounted.SetOutputDiscountedProperties(
                        calcParameters, currentCalculationsElement, currentElement);
                    //set up and run and the calculations
                    bHasCalculations = RunOutputCalculations(outputDiscounted,
                        calcParameters);
                    //serialize object back to xml
                    outputDiscounted.SetOutputDiscountedAttributes(
                        calcParameters, ref currentCalculationsElement,
                        ref currentElement, updates);
                    break;
                default:
                    break;
            }
            return bHasCalculations;
        }

        private bool RunOutputCalculations(OutputDiscounted outputDiscounted,
            CalculatorParameters calcParameters)
        {
            //this calculator does not do cumulative totals
            bool bHasCalculations = false;
            //outputs are discounted using the BudgetsandInvestments calculator
            //that calculator relies on 'ancestor' objects (ancestor xml nodes)
            //for some calculation parameters
            calcParameters.ParentBudgetInvestment = new BudgetInvestment();
            calcParameters.ParentBudgetInvestment.Local = new Local();
            calcParameters.ParentBudgetInvestment.Local.NominalRate = outputDiscounted.Local.NominalRate;
            calcParameters.ParentBudgetInvestment.Local.InflationRate = outputDiscounted.Local.InflationRate;
            calcParameters.ParentBudgetInvestment.Local.RealRate = outputDiscounted.Local.RealRate;
            calcParameters.ParentBudgetInvestment.InitEOPDate = outputDiscounted.EndOfPeriodDate;
            calcParameters.ParentBudgetInvestment.PreProdPeriods = 0;
            calcParameters.ParentBudgetInvestment.ProdPeriods = 1;
            calcParameters.ParentTimePeriod = new TimePeriod();
            calcParameters.ParentTimePeriod.IsDiscounted = outputDiscounted.IsDiscounted;
            calcParameters.ParentTimePeriod.Date = outputDiscounted.EndOfPeriodDate;
            //convert discountedoutput to an Output object that can be used to run calcs
            Output output = OutputDiscounted.ConvertDiscountedOutput(calcParameters, outputDiscounted);
            XElement oCurrentCalculationElement = null;
            BICalculator biCalculator = new BICalculator();
            bHasCalculations = biCalculator.SetOutputCalculations(
                calcParameters, output, ref oCurrentCalculationElement);
            //transfer the new calculations back to outputDiscounted (via its base output object)
            outputDiscounted.SetOutputProperties(calcParameters, output);
            return bHasCalculations;
        }
        private bool RunInputCalculations(InputDiscounted inputDiscounted,
            CalculatorParameters calcParameters)
        {
            //this calculator does not do cumulative totals
            bool bHasCalculations = false;
            //inputs are discounted using the BudgetsandInvestments calculator
            //that calculator relies on 'ancestor' objects (ancestor xml nodes)
            //for some calculation parameters
            calcParameters.ParentBudgetInvestment = new BudgetInvestment();
            calcParameters.ParentBudgetInvestment.Local = new Local();
            calcParameters.ParentBudgetInvestment.Local.NominalRate = inputDiscounted.Local.NominalRate;
            calcParameters.ParentBudgetInvestment.Local.InflationRate = inputDiscounted.Local.InflationRate;
            calcParameters.ParentBudgetInvestment.Local.RealRate = inputDiscounted.Local.RealRate;
            calcParameters.ParentBudgetInvestment.InitEOPDate = inputDiscounted.EndOfPeriodDate;
            calcParameters.ParentBudgetInvestment.PreProdPeriods = 0;
            calcParameters.ParentBudgetInvestment.ProdPeriods = 1;
            calcParameters.ParentTimePeriod = new TimePeriod();
            calcParameters.ParentTimePeriod.IsDiscounted = inputDiscounted.IsDiscounted;
            calcParameters.ParentTimePeriod.Date = inputDiscounted.EndOfPeriodDate;
            calcParameters.ParentOperationComponent = new OperationComponent();
            calcParameters.ParentOperationComponent.Local = new Local();
            calcParameters.ParentOperationComponent.Local.NominalRate = inputDiscounted.Local.NominalRate;
            calcParameters.ParentOperationComponent.Local.InflationRate = inputDiscounted.Local.InflationRate;
            calcParameters.ParentOperationComponent.Local.RealRate = inputDiscounted.Local.RealRate;
            calcParameters.ParentOperationComponent.Amount = 1;
            calcParameters.ParentOperationComponent.Date = inputDiscounted.EndOfPeriodDate;
            calcParameters.ParentOperationComponent.EffectiveLife = inputDiscounted.EffectiveLife;
            calcParameters.ParentOperationComponent.SalvageValue = inputDiscounted.SalvageValue;
            //convert inputDiscounted to an Input object that can be used to run calcs
            Input input = InputDiscounted.ConvertDiscountedInput(calcParameters, inputDiscounted);
            //adjust base input totals (necessary when aliases aren't used to set them)
            if (input.AOHAmount == 0) input.AOHAmount = input.OCAmount;
            if (input.CAPAmount == 0) input.CAPAmount = 1;
            //run calcs
            XElement oCurrentCalculationElement = null;
            BICalculator biCalculator = new BICalculator();
            bHasCalculations = biCalculator.SetInputCalculations(
                calcParameters, input, ref oCurrentCalculationElement);
            //transfer the new calculations back to inputdiscounted
            inputDiscounted.SetInputProperties(calcParameters, input);
            //see if any amortized totals are needed
            if (inputDiscounted.EffectiveLife != 0
                && inputDiscounted.EffectiveLife != 1)
            {
                //uses calcParams.ParentOperation for calculations
                calcParameters.ParentOperationComponent.TotalOC
                    = input.TotalOC + input.TotalOC_INT;
                calcParameters.ParentOperationComponent.TotalAOH
                    = input.TotalAOH + input.TotalAOH_INT;
                calcParameters.ParentOperationComponent.TotalCAP
                    = input.TotalCAP + input.TotalCAP_INT;
                calcParameters.ParentOperationComponent.TotalINCENT
                    = input.TotalINCENT;
                bHasCalculations
                    = biCalculator.SetOperationComponentCalculations(
                    calcParameters, calcParameters.ParentOperationComponent);
                inputDiscounted.TotalAMOC = calcParameters.ParentOperationComponent.TotalAMOC;
                inputDiscounted.TotalAMOC_INT = calcParameters.ParentOperationComponent.TotalAMOC_INT;
                inputDiscounted.TotalAMAOH = calcParameters.ParentOperationComponent.TotalAMAOH;
                inputDiscounted.TotalAMAOH_INT = calcParameters.ParentOperationComponent.TotalAMAOH_INT;
                inputDiscounted.TotalAMCAP = calcParameters.ParentOperationComponent.TotalAMCAP;
                inputDiscounted.TotalAMCAP_INT = calcParameters.ParentOperationComponent.TotalAMCAP_INT;
                inputDiscounted.TotalAMINCENT = calcParameters.ParentOperationComponent.TotalAMINCENT;
            }
            return bHasCalculations;
        }
    }
}
