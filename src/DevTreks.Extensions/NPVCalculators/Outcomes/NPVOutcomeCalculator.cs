using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run general outcome calculations
    ///Author:		www.devtreks.org
    ///Date:		2012, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    //NOTES         1. Carry out calculations by deserializing currentCalculationsElement 
    //              and currentElement into an AddInViews.BaseObject and using the object
    //              to run the calculations.
    //              2. Serialize the object's new calculations back to 
    //              currentCalculationsElement and currentElement, and fill in 
    //              the updates collection with any db fields that have changed. 
    /// </summary>
    public class NPVOutcomeCalculator
    {
        //additional timeliness penalty object (adds optional costs to base object)
        //public TimelinessOpComp1 TimelinessOutcome1 { get; set; }
        public bool SetOutcomeCalculations(
            CalculatorHelpers.CALCULATOR_TYPES calculatorType,
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            //this.TimelinessOutcome1 = new TimelinessOpComp1();
            switch (calculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.outcome:
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
                == Outcome.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                OutcomeGroup outcomeGroup
                    = new OutcomeGroup();
                //deserialize xml to object
                outcomeGroup.SetOutcomeGroupProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                bHasCalculations = SetOutcomeGroupCalculations(
                    calcParameters, outcomeGroup);
                //serialize object back to xml
                outcomeGroup.SetOutcomeGroupAttributes(
                    calcParameters, ref currentElement, updates);
                if (outcomeGroup != null)
                {
                    //locals only for calculator
                    outcomeGroup.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(Outcome.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                BICalculator biCalculator = new BICalculator();
                bHasCalculations
                    = biCalculator.SetOutcomeCalculations(
                    calcParameters, calcParameters.ParentOutcome);
                //serialize object back to xml
                calcParameters.ParentOutcome.SetOutcomeAttributes(
                    calcParameters, ref currentElement, updates);
                if (calcParameters.ParentOutcome.Local != null)
                {
                    //locals only for calculator
                    calcParameters.ParentOutcome.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                } 
            }
            else if (currentElement.Name.LocalName.EndsWith(Output.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                Output output = new Output();
                //deserialize xml to object
                output.SetOutputProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                BICalculator biCalculator = new BICalculator();
                bHasCalculations
                    = biCalculator.SetOutputCalculations(
                    calcParameters, output, ref currentCalculationsElement);
                //serialize object back to xml
                output.SetOutputAttributes(
                    calcParameters, ref currentElement, updates);
                //and set the totals
                output.SetTotalBenefitsAttributes(string.Empty, ref currentElement);
                if (output.Local != null)
                {
                    //locals only for calculator
                    output.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                }
            }
            return bHasCalculations;
        }
        private bool SetOutcomeGroupCalculations(
            CalculatorParameters calcParameters,
            OutcomeGroup outcomeGroup)
        {
            bool bHasCalculations = false;
            outcomeGroup.TotalR += calcParameters.ParentTimePeriod.TotalR;
            outcomeGroup.TotalRINCENT += calcParameters.ParentTimePeriod.TotalRINCENT;
            outcomeGroup.TotalAMR += calcParameters.ParentTimePeriod.TotalAMR;
            outcomeGroup.TotalAMRINCENT += calcParameters.ParentTimePeriod.TotalAMRINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
        public void AdjustJointOutputCalculations(
            Output output, ref XElement currentCalculationsElement,
            CalculatorParameters calcParameters)
        {
            //transfer changes found in calcParams.ParentOutcome.Outputs[]
            //to this output, prior to discounting
            if (calcParameters.ParentOutcome.Outputs != null)
            {
                int iOutputId = output.Id;
                Output adjustedOutput
                    = calcParameters.ParentOutcome.Outputs
                    .FirstOrDefault(i => i.Id == iOutputId
                    && i.Type == CostBenefitCalculator.TYPE_NEWCALCS);
                if (adjustedOutput != null)
                {
                    //replace output
                    output.CopyOutput(adjustedOutput);
                    //output = new Output(adjustedOutput);
                    if (adjustedOutput.XmlDocElement != null)
                    {
                        //adjustments can change fuel amounts, AOHAmounts ...
                        output.XmlDocElement
                            = new XElement(adjustedOutput.XmlDocElement);
                        string sId = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                            Calculator1.cId);
                        currentCalculationsElement
                            = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                            adjustedOutput.XmlDocElement, Calculator1.cId, sId);
                    }
                }
            }
        }
        //set rules, interest charges, and price adjustments for combinations of outputs
        public void SetJointOutputCalculations(CalculatorParameters calcParameters)
        {
            //this is called during the SetAncestors event on opcomps
            if (calcParameters.ParentOutcome.Outputs != null)
            {
                //run any health care cost calculators
                List<HealthBenefit1Calculator> hcOutputs
                    = GetHealthCareOutputs(calcParameters);
                if (hcOutputs != null)
                {
                    if (hcOutputs.Count > 0)
                    {
                        //set any joint calculations needed
                        //none needed yet

                        //serialize all the new calculations
                        SerializeHealthCareOutputCalculations(calcParameters, hcOutputs);
                    }
                }
            }
        }
        private List<HealthBenefit1Calculator> GetHealthCareOutputs(CalculatorParameters calcParameters)
        {
            List<HealthBenefit1Calculator> hcOutputs = new List<HealthBenefit1Calculator>();
            foreach (Output output in calcParameters.ParentOutcome.Outputs)
            {
                if (output.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                    && output.XmlDocElement != null)
                {
                    //0.9.0 stays consistent across all apps by using standard calc patterns
                    if (output.XmlDocElement.HasElements)
                    {
                        XElement oOutputElement = new XElement(output.XmlDocElement);
                        //set the output atts
                        Output.SetOutputAllAttributes(output, ref oOutputElement);
                        string sCalculatorType
                            = CalculatorHelpers.GetCalculatorType(oOutputElement);
                        XElement oHealthCareOutputElement = GetHealthCareCalculator(calcParameters,
                            sCalculatorType, oOutputElement);
                        if (oHealthCareOutputElement != null && oOutputElement != null)
                        {
                            string sErrorMsg = string.Empty;
                            bool bHasThisCalculations = false;
                            sCalculatorType
                                = CalculatorHelpers.GetCalculatorType(oHealthCareOutputElement);
                            if (sCalculatorType
                                == HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString())
                            {
                                string sAttNameExtension = string.Empty;
                                HealthBenefit1Calculator hcOutput = new HealthBenefit1Calculator();
                                //serialize in same manner as OutputHCStockCalculator
                                hcOutput.SetOutputProperties(calcParameters,
                                    oHealthCareOutputElement, oOutputElement);
                                hcOutput.SetHealthBenefit1Properties(oHealthCareOutputElement);
                                OutputHCStockCalculator hcCalculator = new OutputHCStockCalculator();
                                bHasThisCalculations = hcCalculator.RunHCStockCalculations(hcOutput,
                                    calcParameters);
                                //store the new calculations
                                hcOutput.XmlDocElement = oHealthCareOutputElement;
                                calcParameters.ErrorMessage = sErrorMsg;
                                //they need to find one another when serializing
                                output.CalculatorId = hcOutput.CalculatorId;
                                hcOutputs.Add(hcOutput);
                            }
                        }
                    }
                }
            }
            return hcOutputs;
        }
        private XElement GetHealthCareCalculator(CalculatorParameters calcParameters,
             string calculatorType, XElement outputElement)
        {
            XElement oCalculationsElement = null;
            if (outputElement.Name.LocalName
                    == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                if (calculatorType
                    == HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString())
                {
                    oCalculationsElement = outputElement;
                }
            }
            else
            {
                oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(outputElement,
                    Calculator1.cCalculatorType, HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString());
                if (oCalculationsElement == null)
                {
                }
            }
            return oCalculationsElement;
        }
        private void SerializeHealthCareOutputCalculations(CalculatorParameters calcParameters,
            List<HealthBenefit1Calculator> hcOutputs)
        {
            if (calcParameters.ParentOutcome.Outputs != null
                && hcOutputs != null)
            {
                foreach (Output output in calcParameters.ParentOutcome.Outputs)
                {
                    if (output.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                        && output.XmlDocElement != null)
                    {
                        //this was set up when serialized
                        HealthBenefit1Calculator hcOutput = hcOutputs.FirstOrDefault(
                            f => f.CalculatorId == output.CalculatorId);
                        if (hcOutput != null)
                        {
                            if (hcOutput.XmlDocElement != null)
                            {
                                XElement oHealtCareOutputElement = new XElement(hcOutput.XmlDocElement);
                                XElement oOutputElement = new XElement(output.XmlDocElement);
                                //serialize object back to xml using standard MachCalc1 pattern
                                if (hcOutput.CalculatorType
                                    == HCCalculatorHelper.CALCULATOR_TYPES.hcbenefit1.ToString())
                                {
                                    string sAttNameExtension = string.Empty;
                                    hcOutput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                                        ref oHealtCareOutputElement);
                                    hcOutput.SetNewOutputAttributes(calcParameters, ref oHealtCareOutputElement);
                                    hcOutput.SetHealthBenefit1Attributes(sAttNameExtension,
                                        ref oHealtCareOutputElement);
                                }
                                //mark this linkedview as edited (GetCalculator uses it later)
                                oHealtCareOutputElement.SetAttributeValue(CostBenefitStatistic01.TYPE_NEWCALCS,
                                    "true");
                                //update output agmach linkedview with new calcs
                                oOutputElement = new XElement(output.XmlDocElement);
                                CalculatorHelpers.ReplaceElementInDocument(oHealtCareOutputElement,
                                    ref oOutputElement);
                                //update output with new prices and amounts
                                output.XmlDocElement = oOutputElement;
                                output.Price = hcOutput.Price;
                                //tells calculators to swap out output being calculated with this one
                                output.Type = CostBenefitCalculator.TYPE_NEWCALCS;
                            }
                        }
                    }
                }
            }
        }
    }
}
