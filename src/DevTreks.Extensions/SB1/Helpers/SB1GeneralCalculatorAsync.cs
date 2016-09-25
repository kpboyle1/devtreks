using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevTreks.Extensions
{
    ///<summary>
    ///Purpose:		Base calculator class used to runt calculator and 
    ///             analyzer tasks in one consistent manner. 
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTES        1.  Runcs calculators asyncronously. 
    ///             They are both compute and i/o bound because of dataurls.
    ///             In addition, they must to process cancel and progress async tasks.       
    /// </summary>
    public class SB1GeneralCalculatorAsync
    {
        //allow base class to perform initialization tasks when 
        //instances of a derived class are created
        protected SB1GeneralCalculatorAsync() { }
        protected SB1GeneralCalculatorAsync(CalculatorParameters calcParameters) 
        {
            this.GCCalculatorParams = new CalculatorParameters(calcParameters);
            BISB1CalculatorAsync = new BISB1StockCalculatorAsync(calcParameters);
        }
        //standard calculator parameters
        public CalculatorParameters GCCalculatorParams { get; set; }
        //stateful full model
        public BISB1StockCalculatorAsync BISB1CalculatorAsync { get; set; }
        //if true, saves the totals using new object model
        public bool HasTotals { get; set; }

        public async Task<bool> RunCalculationsAndSetUpdatesAsync(XElement currentElement)
        {
            bool bHasCalculations = false;
            if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.basic)
            {
                //182 added synchronous Wait because the calcs were completed after the calc file was saved
                //without Wait() the async returns before UI can display a result
                RunBasicCalculationsAndSetUpdatesAsync(currentElement).Wait();
                bHasCalculations = true;
            }
            else if (this.GCCalculatorParams.RunCalculatorType
                == CalculatorHelpers.RUN_CALCULATOR_TYPES.analyzeobjects)
            {
                RunBasicAnalysisAndSetUpdates(currentElement).Wait();
                bHasCalculations = true;
            }
            //other extensions may need more options
            //else if (this.GCCalculatorParams.RunCalculatorType
            //    == CalculatorHelpers.RUN_CALCULATOR_TYPES.iotechnology)
            //{
            //    bHasCalculations
            //        = RunIOTechCalculationsAndSetUpdates(currentElement);
            //}
            return bHasCalculations;
        }
       
        private async Task<bool> RunBasicCalculationsAndSetUpdatesAsync(XElement currentElement)
        {
            bool bHasCalculations = false;
            if (!currentElement.HasAttributes)
                return true;
            //1. set parameters needed by updates collection
            this.GCCalculatorParams.CurrentElementNodeName
                = currentElement.Name.LocalName;
            this.GCCalculatorParams.CurrentElementURIPattern
                = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                currentElement);
            //2. don't run calcs on ancestors
            bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
               this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
            if (bIsSelfOrDescendentNode)
            {
                //3. get the calculator to use 
                //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
                XElement linkedViewElement = null;
                linkedViewElement = CalculatorHelpers.GetCalculator(
                    this.GCCalculatorParams, currentElement);
                //some apps, such as locals, work differently 
                CalculatorHelpers.AdjustSpecialtyLinkedViewElements(currentElement, linkedViewElement, this.GCCalculatorParams);
                //4. Set bool to update base node attributes in db
                this.GCCalculatorParams.AttributeNeedsDbUpdate
                    = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
                //5. raise event to carry out calculations 
                //v180: no children xml doc changes when Overwrite = false and UseSameCalc = false
                bool bNeedsLVUpdate = CalculatorHelpers.NeedsLVUpdate(this.GCCalculatorParams);
                if (bNeedsLVUpdate)
                {
                     bHasCalculations 
                        = await RunCalculationAsync(currentElement, linkedViewElement);
                }
                else
                {
                    //but if it's a child it might still need to be displayed
                    //will only be displayed by setting CalculatorId or AnalyzerType in currentEl
                    if (CalculatorHelpers.IsSelfOrChildNode(this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName))
                    {
                        CalculatorHelpers.SetCalculatorId(linkedViewElement, currentElement);
                    }
                }
                if (bHasCalculations)
                {
                    //v182 added resetting because async starts next node (i.e. parent) and these change before the method is finished
                    //1. set parameters needed by updates collection
                    //this.GCCalculatorParams.CurrentElementNodeName
                    //    = currentElement.Name.LocalName;
                    //this.GCCalculatorParams.CurrentElementURIPattern
                    //    = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                    //    currentElement);
                    //6. 100% Rules: don't allow analyzers to db update descendent calculators
                    CalculatorHelpers.ChangeLinkedViewCalculator(currentElement, linkedViewElement, this.GCCalculatorParams);
                    //7. replace the this.GCCalculatorParams.LinkedViewElement when 
                    //the originating doctocalcuri node is processed
                    bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
                        currentElement, linkedViewElement);
                    //8. v180 SetXmlDocAttributes only set db updates (xmldoc lvs are automatically changed by RunCalcAsync)
                    CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                        linkedViewElement, currentElement, this.GCCalculatorParams.Updates);
                }
            }
            else
            {
                //basic calculators don't need full collections that include ancestobrs
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        //doesn't run analysis yet; just fills in stateful model for object analysis
        private async Task<bool> RunBasicAnalysisAndSetUpdates(XElement currentElement)
        {
            bool bHasCalculations = false;
            if (!currentElement.HasAttributes)
                return true;
            //1. set parameters needed by updates collection
            this.GCCalculatorParams.CurrentElementNodeName
                = currentElement.Name.LocalName;
            this.GCCalculatorParams.CurrentElementURIPattern
                = CalculatorHelpers.MakeNewURIPatternFromElement(this.GCCalculatorParams.ExtensionDocToCalcURI.URIPattern,
                currentElement);
            //2. don't run calcs on ancestors
            bool bIsSelfOrDescendentNode = CalculatorHelpers.IsSelfOrDescendentNode(
               this.GCCalculatorParams, this.GCCalculatorParams.CurrentElementNodeName);
            if (bIsSelfOrDescendentNode)
            {
                //3. get the calculator to use 
                //(this.GCCalculatorParams.CalculationEl, or currentElement.xmldoc)
                XElement linkedViewElement = null;
                linkedViewElement = CalculatorHelpers.GetCalculator(
                    this.GCCalculatorParams, currentElement);
                //4. Set bool to update base node attributes in db
                this.GCCalculatorParams.AttributeNeedsDbUpdate
                    = CalculatorHelpers.NeedsUpdateAttribute(this.GCCalculatorParams);
                //5. raise event to carry out analysis (no tasks, just property manipulation)
                bHasCalculations = await SetAnalysisModel(currentElement, linkedViewElement);
                if (bHasCalculations)
                {
                    //6. allow analyzers to db update descendent analyzers
                    CalculatorHelpers.ChangeLinkedViewCalculatorForAnalysis(this.GCCalculatorParams, currentElement, ref linkedViewElement);
                    //7. replace the this.GCCalculatorParams.LinkedViewElement when 
                    //the originating doctocalcuri node is processed
                    bool bHasReplacedCalculator = CalculatorHelpers.ReplaceCalculations(this.GCCalculatorParams,
                        currentElement, linkedViewElement);
                    //8. SetXmlDocAttributes
                    CalculatorHelpers.SetXmlDocUpdates(this.GCCalculatorParams,
                        linkedViewElement, ref currentElement,
                        this.GCCalculatorParams.Updates);
                }
            }
            else
            {
                XElement linkedViewElement = null;
                bHasCalculations = await SetAnalysisModel(currentElement, linkedViewElement);
                //always return true, so no error msg is generated
                bHasCalculations = true;
            }
            return bHasCalculations;
        }
        
        //allows derived classes to override the method 
        protected virtual async Task<bool> RunCalculationAsync(XElement currentElement, XElement linkedViewElement)
        {
            bool bHasCalculations = false;
            SB1CalculatorHelper.CALCULATOR_TYPES eCalculatorType
                = SB1CalculatorHelper.GetCalculatorType(
                this.GCCalculatorParams.CalculatorType);
            switch (eCalculatorType)
            {
                case SB1CalculatorHelper.CALCULATOR_TYPES.sb101:
                    //serialize, run calcs, and deserialize
                    SBC1Calculator sbc1 = new SBC1Calculator();
                    bHasCalculations = await sbc1.SetSB1C1CalculationsAsync(eCalculatorType, this.GCCalculatorParams,
                        linkedViewElement, currentElement);
                    break;
                case SB1CalculatorHelper.CALCULATOR_TYPES.sb102:
                    //serialize, run calcs, and deserialize
                    SBB1Calculator sbb1 = new SBB1Calculator();
                    bHasCalculations = await sbb1.SetSB1B1CalculationsAsync(eCalculatorType, this.GCCalculatorParams,
                        linkedViewElement, currentElement);
                    break;
                default:
                    //should be running an analysis
                    break;
            }
            return bHasCalculations;
        }
        private async Task<bool> SetAnalysisModel(XElement currentElement,
                XElement linkedViewElement)
        {
            bool bHasCalculations = false;
            
            if (this.GCCalculatorParams.RelatedCalculatorsType
               == SB1CalculatorHelper.CALCULATOR_TYPES.sb01.ToString())
            {
                bHasCalculations
                    = AddCalculationsToCurrentElement(
                        linkedViewElement, currentElement);
            }
            return bHasCalculations;
        }
        //process any of the nodes in BISB1Calculator (inputs, outputs, ops, outc, bud, invest)
        //by adding them to a corresponding object model
        //this appears inherently syncronous - no computations and no i/o
        public bool AddCalculationsToCurrentElement(
            XElement currentCalculationsElement, XElement currentElement)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == Input.INPUT_PRICE_TYPES.inputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BISB1CalculatorAsync.SetInputGroupSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == Output.OUTPUT_PRICE_TYPES.outputgroup.ToString())
            {
                //the input group can be used to insert calculators into 
                //descendant inputs and run totals for each input
                bHasCalculations = BISB1CalculatorAsync.SetOutputGroupSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.inputprices)
                {
                    //bimachinerystockcalculator handles calculations
                    bHasCalculations = BISB1CalculatorAsync.SetInputSB1Calculations(
                        currentCalculationsElement, currentElement);
                }
                else
                {
                    //resource stock calcs come from calculator results
                    bHasCalculations = BISB1CalculatorAsync.SetTechInputSB1Calculations(
                        currentCalculationsElement, currentElement);
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                if (this.GCCalculatorParams.SubApplicationType == Constants.SUBAPPLICATION_TYPES.outputprices)
                {
                    //bimachinerystockcalculator handles calculations
                    bHasCalculations = BISB1CalculatorAsync.SetOutputSB1Calculations(
                        currentCalculationsElement, currentElement);
                }
                else
                {
                    bHasCalculations = BISB1CalculatorAsync.SetTechOutputSB1Calculations(
                            currentCalculationsElement, currentElement);
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Input.INPUT_PRICE_TYPES.inputseries.ToString()))
            {
                bHasCalculations = BISB1CalculatorAsync.SetInputSeriesSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Output.OUTPUT_PRICE_TYPES.outputseries.ToString()))
            {
                bHasCalculations = BISB1CalculatorAsync.SetOutputSeriesSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || currentElement.Name.LocalName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                //the object model uses a BudgetGroup but all that's needed is id and name 
                bHasCalculations = BISB1CalculatorAsync.SetOCGroupSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BISB1CalculatorAsync.SetOpOrCompSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                //the operation group can be used to insert calculators into 
                //descendant operations and run totals for each operation
                bHasCalculations = BISB1CalculatorAsync.SetOutcomeGroupSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                //bimachinerystockcalculator handles calculations
                bHasCalculations = BISB1CalculatorAsync.SetOutcomeSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgetgroup.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmentgroup.ToString())
            {
                bHasCalculations = BISB1CalculatorAsync.SetBIGroupSB1Calculations(
                   currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budget.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investment.ToString())
            {
                bHasCalculations = BISB1CalculatorAsync.SetBISB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            else if (currentElement.Name.LocalName
                == BudgetInvestment.BUDGET_TYPES.budgettimeperiod.ToString()
                || currentElement.Name.LocalName
                == BudgetInvestment.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                bHasCalculations = BISB1CalculatorAsync.SetTimePeriodSB1Calculations(
                    currentCalculationsElement, currentElement);
            }
            return bHasCalculations;
        }
       
    }
}
