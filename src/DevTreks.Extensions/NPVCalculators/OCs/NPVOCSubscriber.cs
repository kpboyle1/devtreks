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
    ///Purpose:		This class derives from the CalculatorContracts.OperationComponentCalculator 
    ///             class. It subscribes to the RunCalculation and SetAncestorObjects events 
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2012, February
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    public class NPVOCSubscriber : OperationComponentCalculator
    {
        //constructors
        public NPVOCSubscriber() { }
        //constructor sets class (base) properties
        public NPVOCSubscriber(CalculatorParameters calcParameters,
            CalculatorHelpers.CALCULATOR_TYPES calculatorType)
            : base(calcParameters)
        {
            this.CalculatorType = calculatorType;
        }

        //properties
        //calculators specific to this extension
        public CalculatorHelpers.CALCULATOR_TYPES CalculatorType { get; set; }
        //parameters needed by customdocs, in particular 
        //(this.GCCalculatorParams has the parameters passed back by 
        //the event publisher, still need original ones)
        public CalculatorParameters NPVCalculatorParams { get; set; }

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
            e.HasCalculations = RunOperationComponentCalculation(
                ref statElement, ref linkedViewElement);
            //pass the new statelement back to the publisher
            //by setting the CalculatedElement property of CustomEventArgs
            if (statElement != null)
                e.CurrentElement = new XElement(statElement);
            if (linkedViewElement != null)
                e.LinkedViewElement = new XElement(linkedViewElement);
        }
        //descendents need properties from ancestor objects (i.e. realrate)
        //to run their calculations, the ancestors are kept statefully in this.GCCalculatorParams
        private void AddAncestorObjects(XElement currentElement)
        {
            if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                XElement currentCalculationsElement
                   = CalculatorHelpers.GetCalculator(this.GCCalculatorParams,
                   currentElement);
                SetAncestorProperties(currentElement, currentCalculationsElement);
                //run joint input calculations (i.e. fuel amount, fuel cost)
                //these will be copied to descendent input nodes as calculations are run
                //on those nodes
                OCCalculator ocCalculator = new OCCalculator();
                ocCalculator.SetJointInputCalculations(
                    this.GCCalculatorParams);
            }
        }

        public void SetAncestorProperties(XElement currentElement,
            XElement currentCalculationsElement)
        {
            this.GCCalculatorParams.ParentOperationComponent
                = new OperationComponent();
            this.GCCalculatorParams.ParentOperationComponent
                .SetOperationComponentProperties(
                this.GCCalculatorParams, currentCalculationsElement,
                currentElement);
            //init the total costs to zero
            this.GCCalculatorParams.ParentOperationComponent.InitTotalCostsProperties();
            //add streamed inputs so that join calcs can be run
            SetAncestorOperationComponent(currentCalculationsElement,
                currentElement);
            //if needed, can hold cumulative totals for group nodes
            this.GCCalculatorParams.ParentBudgetInvestment
                = new BudgetInvestment();
            this.GCCalculatorParams.ParentBudgetInvestment.Local = new Local();
            this.GCCalculatorParams.ParentBudgetInvestment.Local.NominalRate
                = this.GCCalculatorParams.ParentOperationComponent.Local.NominalRate;
            this.GCCalculatorParams.ParentBudgetInvestment.Local.InflationRate
                = this.GCCalculatorParams.ParentOperationComponent.Local.InflationRate;
            this.GCCalculatorParams.ParentBudgetInvestment.Local.RealRate
                = this.GCCalculatorParams.ParentOperationComponent.Local.RealRate;
            this.GCCalculatorParams.ParentBudgetInvestment.InitEOPDate
                = this.GCCalculatorParams.ParentOperationComponent.Date;
            //as with budgetinvestment calculator; holds cumulative totals
            this.GCCalculatorParams.ParentTimePeriod = new TimePeriod();
            //this is the end of period date used to discount
            this.GCCalculatorParams.ParentTimePeriod.Date
                = this.GCCalculatorParams.ParentOperationComponent.Date;
            //ops/comps always discount (tps have option to turn it off)
            this.GCCalculatorParams.ParentTimePeriod.IsDiscounted = true;
        }
        //stateful ancestor stores input calculations that have to be set 
        //using all of an operation/component's inputs (unlike the 
        //single node calculations run above)
        private void SetAncestorOperationComponent(XElement currentCalculationsElement,
            XElement currentElement)
        {
            this.GCCalculatorParams.ParentOperationComponent.Inputs
                = new List<Input>();
            //use streaming techniques to process children
            XElement rootInputs
                = this.GCCalculatorParams.GetInputForOpOrCompNode(
                currentElement);
            if (rootInputs != null)
            {
                if (rootInputs.HasElements)
                {
                    //although inputs can have multiple xmldocs as children
                    //they have to be structured here using <root><linkedview/>
                    XElement inputXmlDoc = null;
                    foreach (XElement input in rootInputs.Elements())
                    {
                        //need a root of input
                        inputXmlDoc = new XElement(input.Name.LocalName);
                        if (input.Elements(Constants.ROOT_PATH).Any())
                        {
                            foreach (XElement xmlDoc in input.Elements(Constants.ROOT_PATH))
                            {
                                inputXmlDoc.Add(xmlDoc);
                            }
                        }
                        Input oInput = new Input();
                        oInput.SetInputProperties(this.GCCalculatorParams,
                            inputXmlDoc, input);
                        this.GCCalculatorParams.ParentOperationComponent
                            .Inputs.Add(oInput);
                        inputXmlDoc = null;
                    }

                }
            }
        }

        private bool RunOperationComponentCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            //no if (needsCalculation) because calcs are run at all node levels
            //once the calculator is initiated
            CalculatorHelpers.CALCULATOR_TYPES eCalculatorType
                = CalculatorHelpers.GetCalculatorType(this.GCCalculatorParams.CalculatorType);
            OCCalculator ocCalculator = new OCCalculator();
            switch (eCalculatorType)
            {
                //these two apps are expected to need separate calcs in the future
                case CalculatorHelpers.CALCULATOR_TYPES.operation:
                    bHasCalculations
                        = ocCalculator.SetOperationComponentCalculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, this.GCCalculatorParams.Updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.operation2:
                    bHasCalculations
                        = ocCalculator.SetOperationComponentCalculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, this.GCCalculatorParams.Updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.component:
                    bHasCalculations
                        = ocCalculator.SetOperationComponentCalculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, this.GCCalculatorParams.Updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.component2:
                    bHasCalculations
                        = ocCalculator.SetOperationComponentCalculations(
                            eCalculatorType, this.GCCalculatorParams, ref currentCalculationsElement,
                            ref currentElement, this.GCCalculatorParams.Updates);
                    break;
                default:
                    break;
            }
            return bHasCalculations;
        }
    }
}
