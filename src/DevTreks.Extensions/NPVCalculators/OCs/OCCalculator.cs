using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Run general component and operation calculations
    ///Author:		www.devtreks.org
    ///Date:		2014, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    //NOTES         1. Carry out calculations by deserializing currentCalculationsElement 
    //              and currentElement into an AddInViews.BaseObject and using the object
    //              to run the calculations.
    //              2. Serialize the object's new calculations back to 
    //              currentCalculationsElement and currentElement, and fill in 
    //              the updates collection with any db fields that have changed. 
    //              3. New calculations need to be run on any input or output 
    //              with a changed amount (i.e. machinery and food nutrition)
    /// </summary>
    public class OCCalculator
    {
        //additional timeliness penalty object (adds optional costs to base object)
        public TimelinessOpComp1 TimelinessOC1 { get; set; }

        public bool SetOperationComponentCalculations(
            CalculatorHelpers.CALCULATOR_TYPES calculatorType, 
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            //0.8.8 store any calculator specific properties
            this.TimelinessOC1 = new TimelinessOpComp1();
            switch (calculatorType)
            {
                case CalculatorHelpers.CALCULATOR_TYPES.operation:
                    bHasCalculations = AddNPVCalculationsToCurrentElement(calcParameters,
                        ref currentCalculationsElement, ref currentElement,
                        updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.operation2:
                    bHasCalculations = AddNPVAndTimelinessCalculationsToCurrentElement(calcParameters,
                        ref currentCalculationsElement, ref currentElement,
                        updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.component:
                    bHasCalculations = AddNPVCalculationsToCurrentElement(calcParameters,
                        ref currentCalculationsElement, ref currentElement,
                        updates);
                    break;
                case CalculatorHelpers.CALCULATOR_TYPES.component2:
                    bHasCalculations = AddNPVAndTimelinessCalculationsToCurrentElement(calcParameters,
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
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || currentElement.Name.LocalName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                OperationComponentGroup operationGroup
                    = new OperationComponentGroup();
                //deserialize xml to object
                operationGroup.SetOperationComponentGroupProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                bHasCalculations = SetOperationComponentGroupCalculations(
                    calcParameters, operationGroup);
                //serialize object back to xml
                operationGroup.SetOperationComponentGroupAttributes(
                    calcParameters, ref currentElement, updates);
                if (operationGroup.Local != null)
                {
                    //locals only for calculator
                    operationGroup.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                }
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                BICalculator biCalculator = new BICalculator();
                bHasCalculations
                    = biCalculator.SetOperationComponentCalculations(
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
            else if (currentElement.Name.LocalName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                Input input = new Input();
                //deserialize xml to object
                input.SetInputProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                BICalculator biCalculator = new BICalculator();
                bHasCalculations
                    = biCalculator.SetInputCalculations(
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
        private bool AddNPVAndTimelinessCalculationsToCurrentElement(
            CalculatorParameters calcParameters, ref XElement currentCalculationsElement,
            ref XElement currentElement, IDictionary<string, string> updates)
        {
            bool bHasCalculations = false;
            if (currentElement.Name.LocalName
                == OperationComponent.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || currentElement.Name.LocalName
                == OperationComponent.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                //set the timeliness penalty
                this.TimelinessOC1.SetTimelinessOC1Properties(currentCalculationsElement);
                OperationComponentGroup operationGroup
                    = new OperationComponentGroup();
                //deserialize xml to object
                operationGroup.SetOperationComponentGroupProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                bHasCalculations = SetOperationComponentGroupCalculations(
                    calcParameters, operationGroup);
                //serialize object back to xml
                operationGroup.SetOperationComponentGroupAttributes(
                    calcParameters, ref currentElement, updates);
                if (operationGroup.Local != null)
                {
                    //locals only for calculator
                    operationGroup.Local.SetLocalAttributesForCalculator(calcParameters,
                        ref currentCalculationsElement);
                }
                //add the timeliness penalty calculations
                AddSchedulingCalculations(calcParameters, currentElement,
                    currentCalculationsElement);
            }
            else if (currentElement.Name.LocalName
                .EndsWith(OperationComponent.OPERATION_PRICE_TYPES.operation.ToString())
                || currentElement.Name.LocalName
                .EndsWith(OperationComponent.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                //set the timeliness penalty
                this.TimelinessOC1.SetTimelinessOC1Properties(currentCalculationsElement);
                //reset oc.amount to opcomp.amount (not the stored linkedview amout)
                this.TimelinessOC1.SetTimelinessBaseProperties(currentElement);
                BICalculator biCalculator = new BICalculator();
                bHasCalculations
                    = biCalculator.SetOperationComponentCalculations(
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
                //add the timeliness penalty calculations
                AddSchedulingCalculations(calcParameters, currentElement,
                    currentCalculationsElement);
            }
            else if (currentElement.Name.LocalName.EndsWith(Input.INPUT_PRICE_TYPES.input.ToString()))
            {
                Input input = new Input();
                //deserialize xml to object
                input.SetInputProperties(
                    calcParameters, currentCalculationsElement, currentElement);
                BICalculator biCalculator = new BICalculator();
                bHasCalculations
                    = biCalculator.SetInputCalculations(
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
        private bool SetOperationComponentGroupCalculations(
            CalculatorParameters calcParameters, 
            OperationComponentGroup operationOrComponentGroup)
        {
            bool bHasCalculations = false;
            operationOrComponentGroup.TotalOC += calcParameters.ParentTimePeriod.TotalOC;
            operationOrComponentGroup.TotalAOH += calcParameters.ParentTimePeriod.TotalAOH;
            operationOrComponentGroup.TotalCAP += calcParameters.ParentTimePeriod.TotalCAP;
            operationOrComponentGroup.TotalINCENT += calcParameters.ParentTimePeriod.TotalINCENT;
            operationOrComponentGroup.TotalAMOC += calcParameters.ParentTimePeriod.TotalAMOC;
            operationOrComponentGroup.TotalAMAOH += calcParameters.ParentTimePeriod.TotalAMAOH;
            operationOrComponentGroup.TotalAMCAP += calcParameters.ParentTimePeriod.TotalAMCAP;
            operationOrComponentGroup.TotalAMINCENT += calcParameters.ParentTimePeriod.TotalAMINCENT;
            bHasCalculations = true;
            return bHasCalculations;
        }
        private bool AddSchedulingCalculations(CalculatorParameters calcParameters,
            XElement currentElement, XElement currentCalculationsElement)
        {
            //set base opcomp props needed in calculation
            this.TimelinessOC1.SetTimelinessBaseProperties(currentElement);
            //retrieve the first inputocamount that has an input.linkedview with a fuel cost > 0
            double dbPowerInputOCAmount = 0;
            DateTime inputDate = this.TimelinessOC1.PlannedStartDate;
            //the input dates are the actual dates the operation takes place, the opcomp date is 
            //used for discounting
            CalculatorHelpers.GetPowerCalculatorInputVars(calcParameters,
                currentElement, ref inputDate, out dbPowerInputOCAmount);
            //run the final calculations using the object
            bool bHasCalculations = this.TimelinessOC1.AddCalculationsProperties(dbPowerInputOCAmount, inputDate);
            //put the calculator specific props back into the calculator
            this.TimelinessOC1.SetTimelinessOC1Attributes(string.Empty, currentCalculationsElement);
            //need to display some base atts with calculated results
            this.TimelinessOC1.SetTimelinessBaseAttributes(string.Empty, currentCalculationsElement);
            return bHasCalculations;
        }
        //calcParams.ParentOperationComponent.Inputs holds changes made to this input 
        //to reflect the combination of inputs (i.e. fuel price) 
        //found in this operation or component
        public void AdjustJointInputCalculations(
            Input input, ref XElement currentCalculationsElement,
            CalculatorParameters calcParameters)
        {
            //transfer changes found in calcParams.ParentOperationComponent.Inputs[]
            //to this input, prior to discounting
            if (calcParameters.ParentOperationComponent.Inputs != null)
            {
                int iInputId = input.Id;
                Input adjustedInput
                    = calcParameters.ParentOperationComponent.Inputs
                    .FirstOrDefault(i => i.Id == iInputId
                    && i.Type == CostBenefitCalculator.TYPE_NEWCALCS);
                if (adjustedInput != null)
                {
                    //replace input
                    input.CopyInput(adjustedInput);
                    if (adjustedInput.XmlDocElement != null)
                    {
                        //adjustments can change fuel amounts, AOHAmounts ...
                        input.XmlDocElement 
                            = new XElement(adjustedInput.XmlDocElement);
                        string sId = CalculatorHelpers.GetAttribute(currentCalculationsElement,
                            Calculator1.cId);
                        currentCalculationsElement
                            = CalculatorHelpers.GetChildLinkedViewUsingAttribute(
                            adjustedInput.XmlDocElement, Calculator1.cId, sId);
                    }
                }
            }
        }
        //set rules, interest charges, and price adjustments for combinations of inputs
        public void SetJointInputCalculations(CalculatorParameters calcParameters)
        {
            //this is called during the SetAncestors event on opcomps
            if (calcParameters.ParentOperationComponent.Inputs != null)
            {
                //run any machinery calculators
                List<Machinery1Input> machineryInputs
                    = GetMachineryInputs(calcParameters);
                if (machineryInputs != null)
                {
                    if (machineryInputs.Count > 0)
                    {
                        //set any joint calculations needed
                        SetAgMachineryInputs(calcParameters, machineryInputs);
                        //serialize all the new calculations
                        SerializeMachineryInputCalculations(calcParameters, machineryInputs);
                    }
                }
                //deprecated in v165 -can change input.ocamounts
                ////run any food calculators (they set OCAmount == Actual serving size)
                //List<Input> foodInputs
                //    = GetFamilyBudgetInputs(calcParameters);
                //if (foodInputs != null)
                //{
                //    if (foodInputs.Count > 0)
                //    {
                //        //set any joint calculations needed
                //        //none needed yet

                //        //serialize all the new calculations
                //        SerializeFamilyBudgetInputCalculations(calcParameters, foodInputs);
                //    }
                //}
                
                //Calculators that only change prices don't need to be rerun
                //the prices are always taken from base inputs and outputs:
                //Life cycle calculators
                
            }
        }
        private List<Machinery1Input> GetMachineryInputs(CalculatorParameters calcParameters)
        {
            List<Machinery1Input> machineryInputs = new List<Machinery1Input>();
            foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
            {
                if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                    && input.XmlDocElement != null)
                {
                    //0.9.0 stays consistent across all apps by using standard calc patterns
                    if (input.XmlDocElement.HasElements)
                    {
                        string sCalculatorType = string.Empty;
                        //Calculator1 calc = new Calculator1();
                        //calc.InitCalculatorProperties();
                        //calc.SetCalculatorProperties(input.XmlDocElement);
                        //XElement oInputElement = new XElement(input.XmlDocElement);
                        ////set the input atts
                        //Input.SetInputAllAttributes(input, ref oInputElement);
                        //string sCalculatorType
                        //    = CalculatorHelpers.GetCalculatorType(oInputElement);
                        XElement oMachInputElement = GetMachineryCalculator(calcParameters,
                            sCalculatorType, input.XmlDocElement);
                        if (oMachInputElement != null)
                        {
                            string sErrorMsg = string.Empty;
                            bool bHasThisCalculations = false;
                            sCalculatorType
                                = CalculatorHelpers.GetCalculatorType(oMachInputElement);
                            XElement oInputElement = new XElement(input.XmlDocElement.Name.LocalName);
                            input.SetNewInputAttributes(calcParameters, ref oInputElement);
                            if (sCalculatorType
                                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                            {
                                Machinery1Input machineryInput = new Machinery1Input();
                                machineryInput.SetMachinery1InputProperties(calcParameters,
                                    oMachInputElement, oInputElement);
                                bHasThisCalculations = Machinery1InputCalculator.SetAgMachineryCalculations(
                                    calcParameters, AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery,
                                    machineryInput, oMachInputElement, ref sErrorMsg);
                                //store the new calculations
                                machineryInput.XmlDocElement = oMachInputElement;
                                calcParameters.ErrorMessage = sErrorMsg;
                                //they need to find one another when serializing
                                input.CalculatorId = machineryInput.CalculatorId;
                                machineryInputs.Add(machineryInput);
                            }
                            else if (sCalculatorType
                                == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                            {
                                IrrigationPower1Input irrigationPowerInput = new IrrigationPower1Input();
                                irrigationPowerInput.SetIrrigationPower1InputProperties(calcParameters,
                                    oMachInputElement, oInputElement);
                                bHasThisCalculations = Machinery1InputCalculator.SetIrrPowerCalculations(
                                    calcParameters, AgBudgetingHelpers.CALCULATOR_TYPES.irrpower,
                                    irrigationPowerInput, ref sErrorMsg);
                                //store the new calculations
                                irrigationPowerInput.XmlDocElement = oMachInputElement;
                                calcParameters.ErrorMessage = sErrorMsg;
                                //they need to find one another when serializing
                                input.CalculatorId = irrigationPowerInput.CalculatorId;
                                machineryInputs.Add(irrigationPowerInput);
                            }
                            else if (sCalculatorType
                                == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                            {
                                GeneralCapital1Input capitalInput = new GeneralCapital1Input();
                                capitalInput.SetGeneralCapital1InputProperties(calcParameters,
                                        oMachInputElement, oInputElement);
                                bHasThisCalculations = Machinery1InputCalculator.SetGenCapitalCalculations(calcParameters,
                                    AgBudgetingHelpers.CALCULATOR_TYPES.gencapital, capitalInput, ref sErrorMsg);
                                //store the new calculations
                                capitalInput.XmlDocElement = oMachInputElement;
                                calcParameters.ErrorMessage = sErrorMsg;
                                //they need to find one another when serializing
                                input.CalculatorId = capitalInput.CalculatorId;
                                machineryInputs.Add(capitalInput);
                            }
                        }
                    }
                }
            }
            return machineryInputs;
        }
        private List<Input> GetFamilyBudgetInputs(CalculatorParameters calcParameters)
        {
            List<Input> foodInputs = new List<Input>();
            foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
            {
                if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                    && input.XmlDocElement != null)
                {
                    //0.9.0 stays consistent across all apps by using standard calc patterns
                    if (input.XmlDocElement.HasElements)
                    {
                        XElement oInputElement = new XElement(input.XmlDocElement);
                        //set the input atts
                        Input.SetInputAllAttributes(input, ref oInputElement);
                        string sCalculatorType
                            = CalculatorHelpers.GetCalculatorType(oInputElement);
                        XElement oFoodInputElement = GetFamilyBudgetCalculator(calcParameters,
                            sCalculatorType, oInputElement);
                        if (oFoodInputElement != null && oInputElement != null)
                        {
                            string sErrorMsg = string.Empty;
                            bool bHasThisCalculations = false;
                            sCalculatorType
                                = CalculatorHelpers.GetCalculatorType(oFoodInputElement);
                            string sAttNameExtension = string.Empty;
                            if (sCalculatorType
                                == FNCalculatorHelper.CALCULATOR_TYPES.foodfactUSA1.ToString())
                            {
                                FoodFactCalculator foodFactInput = new FoodFactCalculator();
                                //serialize in same manner as IOFNStockCalculator
                                foodFactInput.SetFoodFactProperties(calcParameters,
                                    oFoodInputElement, oInputElement);
                                //foodFactInput.SetInputProperties(calcParameters,
                                //    oFoodInputElement, oInputElement);
                                //foodFactInput.SetFoodFactProperties(oFoodInputElement);
                                IOFNStockCalculator fnCalculator = new IOFNStockCalculator();
                                bHasThisCalculations = fnCalculator.RunFNStockCalculations(foodFactInput,
                                    calcParameters);
                                //store the new calculations
                                foodFactInput.XmlDocElement = oFoodInputElement;
                                calcParameters.ErrorMessage = sErrorMsg;
                                //they need to find one another when serializing
                                input.CalculatorId = foodFactInput.CalculatorId;
                                foodInputs.Add(foodFactInput);
                            }
                            else if (sCalculatorType
                                == FNCalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                            {
                                FNSR01Calculator srInput = new FNSR01Calculator();
                                //serialize in same manner as IOFNStockCalculator
                                srInput.SetFNSR01Properties(calcParameters,
                                    oFoodInputElement, oInputElement);
                                //srInput.SetInputProperties(calcParameters,
                                //    oFoodInputElement, oInputElement);
                                //srInput.SetFNSR01Properties(oFoodInputElement);
                                IOFNSR01StockCalculator fnCalculator = new IOFNSR01StockCalculator();
                                bHasThisCalculations = fnCalculator.RunFNSR01StockCalculations(srInput,
                                    calcParameters);
                                //store the new calculations
                                srInput.XmlDocElement = oFoodInputElement;
                                calcParameters.ErrorMessage = sErrorMsg;
                                //they need to find one another when serializing
                                input.CalculatorId = srInput.CalculatorId;
                                foodInputs.Add(srInput);
                            }
                        }
                    }
                }
            }
            return foodInputs;
        }
        //retain in case hc calcors change amounts (prices and units only in v1.1)
        //private List<HealthCareCost1Calculator> GetHealthCareInputs(CalculatorParameters calcParameters)
        //{
        //    List<HealthCareCost1Calculator> hcInputs = new List<HealthCareCost1Calculator>();
        //    foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
        //    {
        //        if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
        //            && input.XmlDocElement != null)
        //        {
        //            //0.9.0 stays consistent across all apps by using standard calc patterns
        //            if (input.XmlDocElement.HasElements)
        //            {
        //                XElement oInputElement = new XElement(input.XmlDocElement);
        //                //set the input atts
        //                Input.SetInputAllAttributes(input, ref oInputElement);
        //                string sCalculatorType
        //                    = CalculatorHelpers.GetCalculatorType(oInputElement);
        //                XElement oHealthCareInputElement = GetHealthCareCalculator(calcParameters,
        //                    sCalculatorType, oInputElement);
        //                if (oHealthCareInputElement != null && oInputElement != null)
        //                {
        //                    string sErrorMsg = string.Empty;
        //                    bool bHasThisCalculations = false;
        //                    sCalculatorType
        //                        = CalculatorHelpers.GetCalculatorType(oHealthCareInputElement);
        //                    if (sCalculatorType
        //                        == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
        //                    {
        //                        string sAttNameExtension = string.Empty;
        //                        HealthCareCost1Calculator hcInput = new HealthCareCost1Calculator();
        //                        //serialize in same manner as InputHCStockCalculator
        //                        hcInput.SetInputProperties(calcParameters,
        //                            oHealthCareInputElement, oInputElement);
        //                        hcInput.SetHealthCareCost1Properties(oHealthCareInputElement);
        //                        InputHCStockCalculator hcCalculator = new InputHCStockCalculator();
        //                        bHasThisCalculations = hcCalculator.RunHCStockCalculations(hcInput,
        //                            calcParameters);
        //                        //store the new calculations
        //                        hcInput.XmlDocElement = oHealthCareInputElement;
        //                        calcParameters.ErrorMessage = sErrorMsg;
        //                        //they need to find one another when serializing
        //                        input.CalculatorId = hcInput.CalculatorId;
        //                        hcInputs.Add(hcInput);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return hcInputs;
        //}

        //retain in case lcc calcors change amounts (prices only in v1.1)
        //private List<LCC1Calculator> GetLifeCycleInputs(CalculatorParameters calcParameters)
        //{
        //    List<LCC1Calculator> lcInputs = new List<LCC1Calculator>();
        //    foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
        //    {
        //        if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
        //            && input.XmlDocElement != null)
        //        {
        //            //0.9.0 stays consistent across all apps by using standard calc patterns
        //            if (input.XmlDocElement.HasElements)
        //            {
        //                XElement oInputElement = new XElement(input.XmlDocElement);
        //                //set the input atts
        //                Input.SetInputAllAttributes(input, ref oInputElement);
        //                string sCalculatorType
        //                    = CalculatorHelpers.GetCalculatorType(oInputElement);
        //                XElement oLifeCycleInputElement = GetLifeCycleCalculator(calcParameters,
        //                    sCalculatorType, oInputElement);
        //                if (oLifeCycleInputElement != null && oInputElement != null)
        //                {
        //                    string sErrorMsg = string.Empty;
        //                    bool bHasThisCalculations = false;
        //                    sCalculatorType
        //                        = CalculatorHelpers.GetCalculatorType(oLifeCycleInputElement);
        //                    if (sCalculatorType
        //                        == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
        //                    {
        //                        string sAttNameExtension = string.Empty;
        //                        LCC1Calculator lcInput = new LCC1Calculator();
        //                        //serialize in same manner as InputHCStockCalculator
        //                        lcInput.SetInputProperties(calcParameters,
        //                            oLifeCycleInputElement, oInputElement);
        //                        lcInput.SetLCC1CalculatorProperties(oLifeCycleInputElement);
        //                        InputBuildStockCalculator lcCalculator = new InputBuildStockCalculator();
        //                        bHasThisCalculations = lcCalculator.RunBuildStockCalculations(lcInput,
        //                            calcParameters);
        //                        //store the new calculations
        //                        lcInput.XmlDocElement = oLifeCycleInputElement;
        //                        calcParameters.ErrorMessage = sErrorMsg;
        //                        //they need to find one another when serializing
        //                        input.CalculatorId = lcInput.CalculatorId;
        //                        lcInputs.Add(lcInput);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return lcInputs;
        //}
        private XElement GetMachineryCalculator(CalculatorParameters calcParameters, 
            string calculatorType, XElement inputElement)
        {
            XElement oCalculationsElement = null;
            if (inputElement.Name.LocalName
                    == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                if (calculatorType
                    == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                {
                    oCalculationsElement = inputElement;
                }
                else if (calculatorType
                    == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                {
                    oCalculationsElement = inputElement;
                }
                else if (calculatorType
                    == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                {
                    oCalculationsElement = inputElement;
                }
            }
            else
            {
                oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(inputElement,
                    Calculator1.cCalculatorType, AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString());
                if (oCalculationsElement == null)
                {
                    oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(inputElement,
                        Calculator1.cCalculatorType, AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString());
                }
                if (oCalculationsElement == null)
                {
                    oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(inputElement,
                        Calculator1.cCalculatorType, AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString());
                }
                //when multiple agmach are used will need the getally
                //or use new attribute in base calcors AlternativeType='accepted'
                //oCalculationsElement
                //    = CalculatorHelpers.GetAllyCalculator(
                //    calcParameters, inputElement,
                //    AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString());
            }
            return oCalculationsElement;
        }
        private XElement GetFamilyBudgetCalculator(CalculatorParameters calcParameters,
            string calculatorType, XElement inputElement)
        {
            XElement oCalculationsElement = null;
            if (inputElement.Name.LocalName
                    == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                if (calculatorType
                    == FNCalculatorHelper.CALCULATOR_TYPES.foodfactUSA1.ToString())
                {
                    oCalculationsElement = inputElement;
                }
                else if (calculatorType
                    == FNCalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                {
                    oCalculationsElement = inputElement;
                }
            }
            else
            {
                oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(inputElement,
                    Calculator1.cCalculatorType, FNCalculatorHelper.CALCULATOR_TYPES.foodfactUSA1.ToString());
                if (oCalculationsElement == null)
                {
                    oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(inputElement,
                    Calculator1.cCalculatorType, FNCalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString());
                }
            }
            return oCalculationsElement;
        }
        private XElement GetHealthCareCalculator(CalculatorParameters calcParameters,
            string calculatorType, XElement inputElement)
        {
            XElement oCalculationsElement = null;
            if (inputElement.Name.LocalName
                    == Constants.LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                if (calculatorType
                    == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                {
                    oCalculationsElement = inputElement;
                }
            }
            else
            {
                oCalculationsElement = CalculatorHelpers.GetChildLinkedViewUsingAttribute(inputElement,
                    Calculator1.cCalculatorType, HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString());
                if (oCalculationsElement == null)
                {
                }
            }
            return oCalculationsElement;
        }
        
        private void SetAgMachineryInputs(CalculatorParameters calcParameters,
            List<Machinery1Input> machineryInputs)
        {
            List<Machinery1Input> agmachineryInputs
                = machineryInputs.FindAll(mi => mi.CalculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString());
            if (agmachineryInputs != null)
            {
                if (agmachineryInputs.Count > 0)
                {
                    //always readjust all machinery inputs
                    SetAgMachineryInputCalculations(agmachineryInputs);
                }
            }
        }
        private void SerializeMachineryInputCalculations(CalculatorParameters calcParameters,
            List<Machinery1Input> machineryInputs)
        {
            if (calcParameters.ParentOperationComponent.Inputs != null
                && machineryInputs != null)
            {
                foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
                {
                    if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                        && input.XmlDocElement != null)
                    {
                        //this was set up when serialized
                        Machinery1Input machineryInput = machineryInputs.FirstOrDefault(
                            m => m.CalculatorId == input.CalculatorId);
                        if (machineryInput != null)
                        {
                            if (machineryInput.XmlDocElement != null)
                            {
                                XElement oMachInputElement = new XElement(machineryInput.XmlDocElement);
                                XElement oInputElement = new XElement(input.XmlDocElement);
                                //serialize object back to xml using standard MachCalc1 pattern
                                if (machineryInput.CalculatorType
                                    == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
                                {
                                    machineryInput.SetMachinery1InputAttributes(calcParameters, ref oMachInputElement,
                                        ref oInputElement);
                                }
                                else if (machineryInput.CalculatorType
                                    == AgBudgetingHelpers.CALCULATOR_TYPES.irrpower.ToString())
                                {
                                    IrrigationPower1Input irrPowerInput = (IrrigationPower1Input)machineryInput;
                                    irrPowerInput.SetIrrigationPower1Attributes(calcParameters, ref oMachInputElement,
                                        ref oInputElement);
                                }
                                else if (machineryInput.CalculatorType
                                    == AgBudgetingHelpers.CALCULATOR_TYPES.gencapital.ToString())
                                {
                                    GeneralCapital1Input genCapitalInput = (GeneralCapital1Input)machineryInput;
                                    genCapitalInput.SetGeneralCapital1InputAttributes(calcParameters, ref oMachInputElement,
                                        ref oInputElement);
                                }
                                //mark this linkedview as edited (GetCalculator uses it later)
                                oMachInputElement.SetAttributeValue(CostBenefitStatistic01.TYPE_NEWCALCS,
                                    "true");
                                //update input agmach linkedview with new calcs
                                oInputElement = new XElement(input.XmlDocElement);
                                CalculatorHelpers.ReplaceElementInDocument(oMachInputElement,
                                    ref oInputElement);
                                //update input with new prices and amounts
                                input.XmlDocElement = oInputElement;
                                input.OCPrice = machineryInput.OCPrice;
                                input.OCAmount = machineryInput.OCAmount;
                                //note that the joint input calcs don't change
                                //aohprice, capamount, or capprice 
                                //(if they do in future, add those changes here)
                                input.AOHAmount = machineryInput.AOHAmount;
                                //tells calculators to swap out input being calculated with this one
                                input.Type = CostBenefitCalculator.TYPE_NEWCALCS;
                            }
                        }
                    }
                }
            }
        }
        private void SerializeFamilyBudgetInputCalculations(CalculatorParameters calcParameters,
            List<Input> foodInputs)
        {
            if (calcParameters.ParentOperationComponent.Inputs != null
                && foodInputs != null)
            {
                foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
                {
                    if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                        && input.XmlDocElement != null)
                    {
                        //this was set up when serialized
                        Input foodInput = foodInputs.FirstOrDefault(
                            f => f.CalculatorId == input.CalculatorId);
                        if (foodInput != null)
                        {
                            if (foodInput.XmlDocElement != null)
                            {
                                XElement oFoodInputElement = new XElement(foodInput.XmlDocElement);
                                XElement oInputElement = new XElement(input.XmlDocElement);
                                //serialize object back to xml using standard MachCalc1 pattern
                                string sAttNameExtension = string.Empty;
                                foodInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                                    ref oFoodInputElement);
                                foodInput.SetNewInputAttributes(calcParameters, ref oFoodInputElement);
                                if (foodInput.CalculatorType
                                    == FNCalculatorHelper.CALCULATOR_TYPES.foodfactUSA1.ToString())
                                {
                                    FoodFactCalculator ffInput = (FoodFactCalculator)foodInput;
                                    ffInput.SetFoodFactAttributes(sAttNameExtension,
                                        ref oFoodInputElement);
                                }
                                else if (foodInput.CalculatorType
                                    == FNCalculatorHelper.CALCULATOR_TYPES.foodnutSR01.ToString())
                                {
                                    FNSR01Calculator srInput = (FNSR01Calculator)foodInput;
                                    srInput.SetFNSR01Attributes(sAttNameExtension,
                                        ref oFoodInputElement);
                                }
                                //mark this linkedview as edited (GetCalculator uses it later)
                                oFoodInputElement.SetAttributeValue(CostBenefitStatistic01.TYPE_NEWCALCS,
                                    "true");
                                //update input agmach linkedview with new calcs
                                oInputElement = new XElement(input.XmlDocElement);
                                CalculatorHelpers.ReplaceElementInDocument(oFoodInputElement,
                                    ref oInputElement);
                                //update input with new prices and amounts
                                input.XmlDocElement = oInputElement;
                                input.OCPrice = foodInput.OCPrice;
                                input.OCAmount = foodInput.OCAmount;
                                if (!string.IsNullOrEmpty(foodInput.OCUnit))
                                {
                                    input.OCUnit = foodInput.OCUnit;
                                }
                                //no aoh changes yet in food calculators
                                input.CAPPrice = foodInput.CAPPrice;
                                //tells calculators to swap out input being calculated with this one
                                input.Type = CostBenefitCalculator.TYPE_NEWCALCS;
                            }
                        }
                    }
                }
            }
        }
        private void SerializeHealthCareInputCalculations(CalculatorParameters calcParameters,
            List<HealthCareCost1Calculator> hcInputs)
        {
            if (calcParameters.ParentOperationComponent.Inputs != null
                && hcInputs != null)
            {
                foreach (Input input in calcParameters.ParentOperationComponent.Inputs)
                {
                    if (input.AnnuityType == TimePeriod.ANNUITY_TYPES.none
                        && input.XmlDocElement != null)
                    {
                        //this was set up when serialized
                        HealthCareCost1Calculator hcInput = hcInputs.FirstOrDefault(
                            f => f.CalculatorId == input.CalculatorId);
                        if (hcInput != null)
                        {
                            if (hcInput.XmlDocElement != null)
                            {
                                XElement oHealtCareInputElement = new XElement(hcInput.XmlDocElement);
                                XElement oInputElement = new XElement(input.XmlDocElement);
                                //serialize object back to xml using standard MachCalc1 pattern
                                if (hcInput.CalculatorType
                                    == HCCalculatorHelper.CALCULATOR_TYPES.healthcost1.ToString())
                                {
                                    string sAttNameExtension = string.Empty;
                                    hcInput.SetAndRemoveCalculatorAttributes(sAttNameExtension,
                                        ref oHealtCareInputElement);
                                    hcInput.SetNewInputAttributes(calcParameters, ref oHealtCareInputElement);
                                    hcInput.SetHealthCareCost1Attributes(sAttNameExtension,
                                        ref oHealtCareInputElement);
                                }
                                //mark this linkedview as edited (GetCalculator uses it later)
                                oHealtCareInputElement.SetAttributeValue(CostBenefitStatistic01.TYPE_NEWCALCS,
                                    "true");
                                //update input agmach linkedview with new calcs
                                oInputElement = new XElement(input.XmlDocElement);
                                CalculatorHelpers.ReplaceElementInDocument(oHealtCareInputElement,
                                    ref oInputElement);
                                //update input with new prices and amounts
                                input.XmlDocElement = oInputElement;
                                input.OCPrice = hcInput.OCPrice;
                                input.OCAmount = hcInput.OCAmount;
                                //no aoh changes yet in food calculators
                                input.CAPPrice = hcInput.CAPPrice;
                                //tells calculators to swap out input being calculated with this one
                                input.Type = CostBenefitCalculator.TYPE_NEWCALCS;
                            }
                        }
                    }
                }
            }
        }
 
        //mach selection and scheduling analyzers use this section
        public void SetJointInputCalculations(List<Machinery1Input> machineryInputs)
        {
            //if no adjustments were made this list will be null
            if (machineryInputs != null)
            {
                SetAgMachineryInputCalculations(machineryInputs);
            }
        }
        private void SetAgMachineryInputCalculations(List<Machinery1Input> machineryInputs)
        {
            //fix capital inputs so they are synchronized (i.e. same amounts) 
            //for this component
            bool bHasMoreThanOneCapitalInput = false;
            //rule: two power inputs being used in one operation means 
            //they are independent (i.e. a tractor is not pulling a non-powered implement)
            bool bHasMoreThanOnePowerInput = false;
            double dbLowestCapacity = 0;
            int iHighestEquivPTO = 0;
            int iHPPTOMax = 0;
            //implement calculator price rules
            if (machineryInputs.Count > 0)
            {
                GetAgMachinerySynchCapitalInputVars(machineryInputs, ref dbLowestCapacity,
                    ref iHighestEquivPTO, ref iHPPTOMax,
                    ref bHasMoreThanOneCapitalInput, ref bHasMoreThanOnePowerInput);
                bool bNeedsToAdjustInput = false;
                //make sure they have power equipment calculations before going further
                if (dbLowestCapacity != 0 & iHPPTOMax != 0)
                {
                    foreach (Machinery1Input machInput in machineryInputs)
                    {
                        //adjust any existing prices built from calculators so they now reflect the combination of inputs (i.e. fuel price) in this operation or component
                        bNeedsToAdjustInput = SetNewAgMachineryCalculations(machInput, bHasMoreThanOneCapitalInput,
                            bHasMoreThanOnePowerInput, dbLowestCapacity,
                            iHighestEquivPTO, iHPPTOMax);
                    }
                }
            }
            else
            {
                //no joint calcs to adjust, use the new machinery calculation that were 
                //run when the machinputs collection was built
            }
        }
        /// <summary>
        /// Get the variables needed to synchronize and update capital input amounts 
        /// for this specific combination of inputs
        /// </summary>
        public void GetAgMachinerySynchCapitalInputVars(List<Machinery1Input> machineryInputs,
            ref double lowestCapacity, ref int highestEquivPTO, ref int maxPTO,
            ref bool hasMoreThanOneCapitalInput, ref bool hasMoreThanOnePowerInput)
        {
            int iCapitalInputCount = 0;
            int iPowerInputCount = 0;
            if (machineryInputs != null)
            {
                foreach (Machinery1Input machInput in machineryInputs)
                {
                    //Rule 1. A capital input is any input with a CAPrice not equal to zero
                    if (machInput.CAPPrice != 0)
                    {
                        //Rule 2. Do not compute calculated prices for any capital input being expensed (with an amount != 0)
                        if (machInput.CAPAmount == 0)
                        {
                            iCapitalInputCount = iCapitalInputCount + 1;
                            if (iCapitalInputCount > 1) hasMoreThanOneCapitalInput = true;
                            GetAgMachineryCalculatorVars(machInput, 
                                ref lowestCapacity, ref highestEquivPTO, ref maxPTO,
                                ref iPowerInputCount);
                            if (iPowerInputCount > 1) hasMoreThanOnePowerInput = true;
                        }
                    }
                }
            }
        }

        private void GetAgMachineryCalculatorVars(Machinery1Input machInput, 
            ref double lowestCapacity, ref int highestEquivPTO, ref int maxPTO,
            ref int powerInputCount)
        {
            int iHighestEquivPTO = 0;
            int iMaxPTO = 0;
            double dbFuelCost = 0;
            //set the lowest equivalent pto hp, and set the service capacity from that same item
            GetFuelVars(machInput, AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString(),
                out iHighestEquivPTO, out iMaxPTO, out dbFuelCost);
            //see the comments above about naming convention changes that effect ocamount
            double currentOCAmount = GetOCAmount(machInput);
            GetCapacityandFuelProperties(machInput.Constants.ServiceCapacity, iHighestEquivPTO, iMaxPTO, dbFuelCost,
                ref lowestCapacity, ref highestEquivPTO, ref maxPTO,
                ref powerInputCount);
        }
        private static void GetCapacityandFuelProperties(double currentLowestCapacity,
            int currentLowestEquivPTO, int currentMaxPTO, double fuelCost,
            ref double lowestCapacity, ref int highestEquivPTO, ref int maxPTO,
            ref int powerInputCount)
        {
            //set the power input count
            if (fuelCost > 0)
            {
                //if more than one power input, don't reset any of the variables 
                //for these inputs - assume the power inputs are being used independently
                powerInputCount += 1;
                //but if the power input has associated nonpower inputs, the maxptohp comes from power input
                if (maxPTO <= 0)
                {
                    maxPTO = currentMaxPTO;
                }
                //NOTE: this is not usually used (one power input per opcomp is the norm)
                //fuel calcs use two ways to use maxpto : straight multipler or divisor multiplier
                //the fuel multiplier is equivptohp / maxptohp so highest cost comes with lowest maxptohp
                if (currentMaxPTO > 0 && currentMaxPTO < maxPTO)
                {
                    maxPTO = currentMaxPTO;
                }
            }
            else
            {
                //comes from implement 
                if (lowestCapacity <= 0)
                {
                    lowestCapacity = currentLowestCapacity;
                }
                //highest timeliness penalties come from lowest capacity
                //timeliness critical in determining full machinery costs
                if (currentLowestCapacity > 0 && currentLowestCapacity < lowestCapacity)
                {
                    lowestCapacity = currentLowestCapacity;
                }
                //the fuel multiplier is equivptohp / maxptohp so highest cost comes with highestequivptohp
                if (highestEquivPTO <= 0)
                {
                    highestEquivPTO = currentLowestEquivPTO;
                }
                if (currentLowestEquivPTO > 0 && currentLowestEquivPTO > highestEquivPTO)
                {
                    highestEquivPTO = currentLowestEquivPTO;
                }
            }
        }
        private static double GetOCAmount(Machinery1Input machInput)
        {
            //see the comments above about naming convention changes that effect ocamount
            double currentOCAmount = (machInput.Constants.ServiceCapacity != 0)
                ? machInput.Constants.ServiceCapacity : machInput.OCAmount;
            return currentOCAmount;
        }
        /// <summary>
        /// Get the fuel vars to be be used by all of the inputs in an operation
        /// </summary>
        private void GetFuelVars(Machinery1Input machInput, string calculatorType, 
            out int equivPTOHP, out int maxPTOHP, out double fuelCost)
        {
            equivPTOHP = 0;
            maxPTOHP = 0;
            fuelCost = 0;
            if (calculatorType
                == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString())
            {
                equivPTOHP = machInput.Constants.HPPTOEquiv;
                maxPTOHP = machInput.Constants.HPPTOMax;
                fuelCost = machInput.FuelCost;
            }
        }
        /// <summary>
        /// Some calculated prices need to be adjusted (i.e. fuel) based on the specific operation or component they are now part of
        /// </summary>
        public bool SetNewAgMachineryCalculations(Machinery1Input machInput,
            bool hasMoreThanOneCapitalInput, bool hasMoreThanOnePowerInput, double lowestCapacity,
            int highestEquivPTO, int maxPTO)
        {
            bool bHasNewCalcs = false;
            if (machInput.CAPPrice != 0)
            {
                if (hasMoreThanOneCapitalInput == true
                    && hasMoreThanOnePowerInput == false)
                {
                    AdjustMachineryInputAmounts(machInput, lowestCapacity, highestEquivPTO, maxPTO);
                    bHasNewCalcs = true;
                }
                else
                {
                    if (hasMoreThanOneCapitalInput == false)
                    {
                        //aoh amount was not set in input calculator
                        //set it now
                        SetAmounts(machInput, machInput.OCAmount);
                        bHasNewCalcs = true;
                    }
                }
            }
            return bHasNewCalcs;
        }
        /// <summary>
        /// Returns true if any parameters have changed
        /// </summary>
        private bool NeedsNewCalculation(Machinery1Input machInput,
            double lowestCapacity, int highestEquivPTO, int maxPTO)
        {
            bool bNeedsNewCalculation = false;
            //see if any changes are needed
            if (machInput.OCAmount != lowestCapacity
                || machInput.AOHAmount != lowestCapacity)
            {
                return true;
            }
            int iHighestEquivPTO = 0;
            int iMaxPTO = 0;
            double dbFuelCost = 0;
            //see if this differs from lowest joint params
            GetFuelVars(machInput, AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery.ToString(),
                out iHighestEquivPTO, out iMaxPTO, out dbFuelCost);
            if (highestEquivPTO != iHighestEquivPTO
                || maxPTO != iMaxPTO)
            {
                return true;
            }
            return bNeedsNewCalculation;
        }
        /// <summary>
        /// Adjust any machinery input amounts to account for operations/components
        /// </summary>
        private void AdjustMachineryInputAmounts(Machinery1Input machInput,
            double lowestCapacity, int highestEquivPTO, int maxPTO)
        {
            //set the amounts
            SetAmounts(machInput, lowestCapacity);
            //change the fuel consumption, fuel cost, and ocprice attributes
            ChangeFuelAtts(machInput, lowestCapacity, highestEquivPTO, maxPTO);
        }
        private void SetAmounts(Machinery1Input machInput, double lowestCapacity)
        {
            //set the amounts
            //note probably want width, speed and fieldeff synchronized because they go into this number
            machInput.OCAmount = lowestCapacity;
            machInput.AOHAmount = lowestCapacity;
            //tells calculators to swap out input being calculated with this one
            machInput.Type = CostBenefitCalculator.TYPE_NEWCALCS;
        }
        private void ChangeFuelAtts(Machinery1Input machInput,
            double lowestCapacity, int highestEquivPTO, int maxPTO)
        {
            GeneralRules.UNIT_TYPES eUnitType
                = GeneralRules.UNIT_TYPES.none;
            if (machInput.Local.UnitGroupId != 0)
            {
                eUnitType = GeneralRules.GetUnitsEnum(machInput.Local.UnitGroupId);
            }
            //set the new service capacity
            machInput.Constants.ServiceCapacity = lowestCapacity;
            //keep hppto props synchronized for transparent calculations
            machInput.Constants.HPPTOMax = maxPTO;
            machInput.Constants.HPPTOEquiv = highestEquivPTO;
            if (machInput.FuelCost > 0 && machInput.Constants.FuelType != string.Empty
                && machInput.Constants.FuelType != Constants.NONE)
            {
                double dbOldFuelCost = machInput.FuelCost;
                double dbNewFuelCost = 0;
                //if it has a fuel cost it is a power driven tractor
                ChangeFuelAmounts(machInput,
                    highestEquivPTO, maxPTO, eUnitType, ref dbNewFuelCost);
                if (dbOldFuelCost != dbNewFuelCost)
                {
                    //calculate and set the ocprice: all costs are per hour
                    //since oc.Amount is acres/hr, the final multiplication
                    //gives cost per acre
                    machInput.OCPrice = machInput.LubeOilCost + machInput.RepairCost
                        + machInput.LaborCost + dbNewFuelCost;
                    //neither capital recovery or thi cost were changed, so don't change aohprice
                }
            }
        }
        public void ChangeFuelAmounts(Machinery1Input machInput,
            int highestEquivPTO, int maxPTO, GeneralRules.UNIT_TYPES unitType,
            ref double newFuelCostHr)
        {
            double dbFuelAmount = 0;
            newFuelCostHr = GeneralRules.FuelCostHr(unitType, machInput.Constants.FuelType,
                maxPTO, highestEquivPTO, machInput.Constants.PriceGas, machInput.Constants.PriceDiesel,
                machInput.Constants.PriceLP, ref dbFuelAmount);
            //do not use input.times as a multiplier here
            //the parent element uses it to set final costs
            //and resource stock analyzers use it as a multiplier
            //that changes these in their analyses (so no double counting)
            machInput.FuelCost = newFuelCostHr;
            machInput.FuelAmount = dbFuelAmount;
        }
       
    }
}
