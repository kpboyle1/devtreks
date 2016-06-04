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
    ///Purpose:		This class derives from the CalculatorContracts.OutcomeCalculator 
    ///             class. It subscribes to the RunCalculation and SetAncestorObjects events 
    ///             raised by that class. It runs calculations on the nodes 
    ///             returned by that class, and returns a calculated xelement 
    ///             to the publisher for saving.
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        1. 
    /// </summary>
    /// 
    public class NPVOutcomeSubscriber : OutcomeCalculator
    {
        //constructors
        public NPVOutcomeSubscriber() { }
        //constructor sets class (base) properties
        public NPVOutcomeSubscriber(CalculatorParameters calcParameters,
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
            e.HasCalculations = RunOutcomeCalculation(
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
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                XElement currentCalculationsElement
                   = CalculatorHelpers.GetCalculator(this.GCCalculatorParams,
                   currentElement);
                SetAncestorProperties(currentElement, currentCalculationsElement);
                //run joint output calculations (i.e. fuel amount, fuel cost)
                //these will be copied to descendent output nodes as calculations are run
                //on those nodes
                NPVOutcomeCalculator outcomeCalculator = new NPVOutcomeCalculator();
                outcomeCalculator.SetJointOutputCalculations(
                    this.GCCalculatorParams);
            }
        }

        public void SetAncestorProperties(XElement currentElement,
            XElement currentCalculationsElement)
        {
            this.GCCalculatorParams.ParentOutcome
                = new Outcome();
            this.GCCalculatorParams.ParentOutcome
                .SetOutcomeProperties(
                this.GCCalculatorParams, currentCalculationsElement,
                currentElement);
            //init the total costs to zero
            this.GCCalculatorParams.ParentOutcome.InitTotalCostsProperties();
            //add streamed outputs so that join calcs can be run
            SetAncestorOutcome(currentCalculationsElement,
                currentElement);
            //if needed, can hold cumulative totals for group nodes
            this.GCCalculatorParams.ParentBudgetInvestment
                = new BudgetInvestment();
            this.GCCalculatorParams.ParentBudgetInvestment.Local = new Local();
            this.GCCalculatorParams.ParentBudgetInvestment.Local.NominalRate
                = this.GCCalculatorParams.ParentOutcome.Local.NominalRate;
            this.GCCalculatorParams.ParentBudgetInvestment.Local.InflationRate
                = this.GCCalculatorParams.ParentOutcome.Local.InflationRate;
            this.GCCalculatorParams.ParentBudgetInvestment.Local.RealRate
                = this.GCCalculatorParams.ParentOutcome.Local.RealRate;
            this.GCCalculatorParams.ParentBudgetInvestment.InitEOPDate
                = this.GCCalculatorParams.ParentOutcome.Date;
            //as with budgetinvestment calculator; holds cumulative totals
            this.GCCalculatorParams.ParentTimePeriod = new TimePeriod();
            //this is the end of period date used to discount
            this.GCCalculatorParams.ParentTimePeriod.Date
                = this.GCCalculatorParams.ParentOutcome.Date;
            //ops/comps always discount (tps have option to turn it off)
            this.GCCalculatorParams.ParentTimePeriod.IsDiscounted = true;
        }
        //stateful ancestor stores output calculations that have to be set 
        //using all of an outcome's outputs (unlike the 
        //single node calculations run above)
        private void SetAncestorOutcome(XElement currentCalculationsElement,
            XElement currentElement)
        {
            this.GCCalculatorParams.ParentOutcome.Outputs
                = new List<Output>();
            //use streaming techniques to process children
            XElement rootOutputs
                = this.GCCalculatorParams.GetOutputForOutcomeNode(
                currentElement);
            if (rootOutputs != null)
            {
                if (rootOutputs.HasElements)
                {
                    //although outputs can have multiple xmldocs as children
                    //they have to be structured here using <root><linkedview/>
                    XElement outputXmlDoc = null;
                    foreach (XElement output in rootOutputs.Elements())
                    {
                        //need a root of output
                        outputXmlDoc = new XElement(output.Name.LocalName);
                        if (output.Elements(Constants.ROOT_PATH).Any())
                        {
                            foreach (XElement xmlDoc in output.Elements(Constants.ROOT_PATH))
                            {
                                outputXmlDoc.Add(xmlDoc);
                            }
                        }
                        Output oOutput = new Output();
                        oOutput.SetOutputProperties(this.GCCalculatorParams,
                            outputXmlDoc, output);
                        this.GCCalculatorParams.ParentOutcome
                            .Outputs.Add(oOutput);
                        outputXmlDoc = null;
                    }

                }
            }
        }

        private bool RunOutcomeCalculation(ref XElement currentElement,
            ref XElement currentCalculationsElement)
        {
            bool bHasCalculations = false;
            //no if (needsCalculation) because calcs are run at all node levels
            //once the calculator is initiated
            CalculatorHelpers.CALCULATOR_TYPES eCalculatorType
                = CalculatorHelpers.GetCalculatorType(this.GCCalculatorParams.CalculatorType);
            NPVOutcomeCalculator ocCalculator = new NPVOutcomeCalculator();
            switch (eCalculatorType)
            {
                //these two apps are expected to need separate calcs in the future
                case CalculatorHelpers.CALCULATOR_TYPES.outcome:
                    bHasCalculations
                        = ocCalculator.SetOutcomeCalculations(
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
