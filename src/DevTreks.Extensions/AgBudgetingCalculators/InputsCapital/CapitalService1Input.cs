using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Support capital service calculations
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>      
    public class CapitalService1Input : Machinery1Input
    {
        private const string cServiceCapacityUnits = "ServiceCapacityUnits";
        private const string cCapitalRecoveryCost = "CapitalRecoveryCost";
        private const string cTaxesHousingInsuranceCost = "TaxesHousingInsuranceCost";
        private const string cOptionForCapacity = "OptionForCapacity";
        //private const string cOptionForInflation = "OptionForInflation";
        //private const string cOptionForTime = "OptionForTime";
        private const string cOptionForFuel = "OptionForFuel";
        private const string cLaborAmountAdj = "LaborAmountAdj";
        //set inherited capservice properties
        private const string cDiscountFactor = "DiscountFactor";
        private const string cServiceUnitPrice = "ServiceUnitPrice";
        private const string cServiceEnhanceAmount = "ServiceEnhanceAmount";
        private const string cServiceEnhanceTimePeriod = "ServiceEnhanceTimePeriod";
        private const string cOC1Name = "OC1Name";
        private const string cOC2Name = "OC2Name";
        private const string cOC3Name = "OC3Name";
        private const string cOC4Name = "OC4Name";
        private const string cAOH1Name = "AOH1Name";
        private const string cAOH2Name = "AOH2Name";
        private const string cOC1Amount = "OC1Amount";
        private const string cOC2Amount = "OC2Amount";
        private const string cOC3Amount = "OC3Amount";
        private const string cOC4Amount = "OC4Amount";
        private const string cAOH1Amount = "AOH1Amount";
        private const string cAOH2Amount = "AOH2Amount";
        private const string cOC1Change = "OC1Change";
        private const string cOC2Change = "OC2Change";
        private const string cOC3Change = "OC3Change";
        private const string cOC4Change = "OC4Change";
        private const string cAOH1Change = "AOH1Change";
        private const string cAOH2Change = "AOH2Change";
        private const string cOC1Cost = "OC1Cost";
        private const string cOC2Cost = "OC2Cost";
        private const string cOC3Cost = "OC3Cost";
        private const string cOC4Cost = "OC4Cost";
        private const string cOCTotalCost = "OCTotalCost";
        private const string cAOH1Cost = "AOH1Cost";
        private const string cAOH2Cost = "AOH2Cost";
        private const string cAOHTotalCost = "AOHTotalCost";
        private const string cOutputsSum = "OutputsSum";
        private const string cOutputName = "OutputName";
        private const string cOutputUnits = "OutputUnits";
        private const string cOutputBeginAmount = "OutputBeginAmount";
        private const string cOutputBeginGrowthType = "OutputBeginGrowthType";
        private const string cOutputBeginChange = "OutputBeginChange";
        private const string cOutputEndGrowthType = "OutputEndGrowthType";
        private const string cOutputEndChange = "OutputEndChange";
        private const string cUseOnlyTimePeriod = "UseOnlyTimePeriod";
        private const string cOutputPeakTimePeriod = "OutputPeakTimePeriod";
        private const string cGrowthType = "GrowthType";
        private const string cAccOutputEndYrT = "AccOutputEndYrT";
        private const string cDiscOutputBeginYrT = "DiscOutputBeginYrT";
        private const string cAccCostEndYrT = "AccCostEndYrT";

        public double DiscountFactor { get; set; }
        public double ServiceUnitPrice { get; set; }
        public double ServiceEnhanceAmount { get; set; }
        public int ServiceEnhanceTimePeriod { get; set; }
        public string OC1Name { get; set; }
        public string OC2Name { get; set; }
        public string OC3Name { get; set; }
        public string OC4Name { get; set; }
        public string AOH1Name { get; set; }
        public string AOH2Name { get; set; }
        public double OC1Amount { get; set; }
        public double OC2Amount { get; set; }
        public double OC3Amount { get; set; }
        public double OC4Amount { get; set; }
        public double AOH1Amount { get; set; }
        public double AOH2Amount { get; set; }
        public double OC1Change { get; set; }
        public double OC2Change { get; set; }
        public double OC3Change { get; set; }
        public double OC4Change { get; set; }
        public double AOH1Change { get; set; }
        public double AOH2Change { get; set; }
        public double OC1Cost { get; set; }
        public double OC2Cost { get; set; }
        public double OC3Cost { get; set; }
        public double OC4Cost { get; set; }
        public double OCTotalCost { get; set; }
        public double AOH1Cost { get; set; }
        public double AOH2Cost { get; set; }
        public double AOHTotalCost { get; set; }
        public double OutputsSum { get; set; }
        public string OutputName { get; set; }
        public string OutputUnits { get; set; }
        public double OutputBeginAmount { get; set; }
        public int OutputBeginGrowthType { get; set; }
        public double OutputBeginChange { get; set; }
        public int OutputEndGrowthType { get; set; }
        public double OutputEndChange { get; set; }
        public int UseOnlyTimePeriod { get; set; }
        public int OutputPeakTimePeriod { get; set; }
        public int GrowthType { get; set; }
        public double AccOutputEndYrT { get; set; }
        public double DiscOutputBeginYrT { get; set; }
        public double AccCostEndYrT { get; set; }
        //time periods used to calculate time dependent costs
        public TimePeriodCS[] TimePeriods;

        public void SetCapitalService1InputProperties(CalculatorParameters calcParameters,
            XElement currentCalculationsElement, XElement currentElement)
        {
            //set the base input properties
            SetMachinery1InputProperties(calcParameters,
                currentCalculationsElement, currentElement);
            SetProperties(currentCalculationsElement);
        }
        public void SetCapitalService1InputProperties(CalculatorParameters calcParameters, 
            Machinery1Input machinery1Input, XElement calculationsElement)
        {
            //set this properties
            SetProperties(calculationsElement);
            //set the base input properties
            CopyMachinery1InputProperties(calcParameters, machinery1Input);
        }
        
        private void SetProperties(XElement calculationsElement)
        {
            //set this properties
            this.DiscountFactor = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cDiscountFactor);
            this.ServiceUnitPrice = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cServiceUnitPrice);
            this.ServiceEnhanceAmount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cServiceEnhanceAmount);
            this.ServiceEnhanceTimePeriod = CalculatorHelpers.GetAttributeInt(calculationsElement,
               cServiceEnhanceTimePeriod);
            this.OC1Name = CalculatorHelpers.GetAttribute(calculationsElement,
               cOC1Name);
            this.OC2Name = CalculatorHelpers.GetAttribute(calculationsElement,
               cOC2Name);
            this.OC3Name = CalculatorHelpers.GetAttribute(calculationsElement,
               cOC3Name);
            this.OC4Name = CalculatorHelpers.GetAttribute(calculationsElement,
               cOC4Name);
            this.AOH1Name = CalculatorHelpers.GetAttribute(calculationsElement,
               cAOH1Name);
            this.AOH2Name = CalculatorHelpers.GetAttribute(calculationsElement,
               cAOH2Name);
            this.OC1Amount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC1Amount);
            this.OC2Amount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC2Amount);
            this.OC3Amount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC3Amount);
            this.OC4Amount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC4Amount);
            this.AOH1Amount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOH1Amount);
            this.AOH2Amount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOH2Amount);
            this.OC1Change = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC1Change);
            this.OC2Change = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC2Change);
            this.OC3Change = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC3Change);
            this.OC4Change = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC4Change);
            this.AOH1Change = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOH1Change);
            this.AOH2Change = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOH2Change);
            this.OC1Cost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC1Cost);
            this.OC2Cost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC2Cost);
            this.OC3Cost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC3Cost);
            this.OC4Cost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOC4Cost);
            this.OCTotalCost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOCTotalCost);
            this.AOH1Cost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOH1Cost);
            this.AOH2Cost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOH2Cost);
            this.AOHTotalCost = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAOHTotalCost);
            this.OutputsSum = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOutputsSum);
            this.OutputName = CalculatorHelpers.GetAttribute(calculationsElement,
               cOutputName);
            this.OutputUnits = CalculatorHelpers.GetAttribute(calculationsElement,
               cOutputUnits);
            this.OutputBeginAmount = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOutputBeginAmount);
            this.OutputBeginGrowthType = CalculatorHelpers.GetAttributeInt(calculationsElement,
               cOutputBeginGrowthType);
            this.OutputBeginChange = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOutputBeginChange);
            this.OutputEndGrowthType = CalculatorHelpers.GetAttributeInt(calculationsElement,
               cOutputEndGrowthType);
            this.OutputEndChange = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cOutputEndChange);
            this.UseOnlyTimePeriod = CalculatorHelpers.GetAttributeInt(calculationsElement,
              cUseOnlyTimePeriod);
            this.OutputPeakTimePeriod = CalculatorHelpers.GetAttributeInt(calculationsElement,
               cOutputPeakTimePeriod);
            this.GrowthType = CalculatorHelpers.GetAttributeInt(calculationsElement,
               cGrowthType);
            this.AccOutputEndYrT = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAccOutputEndYrT);
            this.DiscOutputBeginYrT = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cDiscOutputBeginYrT);
            this.AccCostEndYrT = CalculatorHelpers.GetAttributeDouble(calculationsElement,
               cAccCostEndYrT);
        }

        public void SetCapitalService1InputAttributes(CalculatorParameters calcParameters,
            ref XElement currentCalculationsElement, ref XElement currentElement,
            IDictionary<string, string> updates)
        {
            //set the base attributes
            SetMachinery1InputAttributes(calcParameters,
                ref currentCalculationsElement, ref currentElement,
                updates);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cDiscountFactor, this.DiscountFactor);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cServiceUnitPrice, this.ServiceUnitPrice);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cServiceEnhanceAmount, this.ServiceEnhanceAmount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cServiceEnhanceTimePeriod, this.ServiceEnhanceTimePeriod);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cOC1Name, this.OC1Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cOC2Name, this.OC2Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cOC3Name, this.OC3Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cOC4Name, this.OC4Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cAOH1Name, this.AOH1Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cAOH2Name, this.AOH2Name);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC1Amount, this.OC1Amount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC2Amount, this.OC2Amount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC3Amount, this.OC3Amount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC4Amount, this.OC4Amount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAOH1Amount, this.AOH1Amount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAOH2Amount, this.AOH2Amount);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC1Change, this.OC1Change);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC2Change, this.OC2Change);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC3Change, this.OC3Change);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC4Change, this.OC4Change);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAOH1Change, this.AOH1Change);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAOH2Change, this.AOH2Change);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC1Cost, this.OC1Cost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC2Cost, this.OC2Cost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC3Cost, this.OC3Cost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOC4Cost, this.OC4Cost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOCTotalCost, this.OCTotalCost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAOH1Cost, this.AOH1Cost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAOH2Cost, this.AOH2Cost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
               cAOHTotalCost, this.AOHTotalCost);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOutputsSum, this.OutputsSum);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                cOutputName, this.Name);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
               cOutputUnits, this.OutputUnits);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOutputBeginAmount, this.OutputBeginAmount);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                cOutputBeginGrowthType, this.OutputBeginGrowthType);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOutputBeginChange, this.OutputBeginChange);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                cOutputEndGrowthType, this.OutputEndGrowthType);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cOutputEndChange, this.OutputEndChange);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                cUseOnlyTimePeriod, this.UseOnlyTimePeriod);
            CalculatorHelpers.SetAttributeInt(currentCalculationsElement,
                cOutputPeakTimePeriod, this.OutputPeakTimePeriod);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAccOutputEndYrT, this.AccOutputEndYrT);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cDiscOutputBeginYrT, this.DiscOutputBeginYrT);
            CalculatorHelpers.SetAttributeDoubleF4(currentCalculationsElement,
                cAccCostEndYrT, this.AccCostEndYrT);
        }
        
    }
    public class TimePeriodCS : CapitalService1Input
    {
        //inits with cap services input
        public TimePeriodCS(CalculatorParameters calcParameters, CapitalService1Input capServiceInput) 
        {
            this.Constants = new Machinery1Constant();
            this.Constants.SetMachinery1ConstantProperties(capServiceInput.Constants);
            this.Local = new Local(calcParameters, capServiceInput.Local);
            //set inherited mach1 properties
            this.ListPriceAdj = capServiceInput.ListPriceAdj;
            this.SalvageValue = capServiceInput.SalvageValue;
            this.FuelAmount = capServiceInput.FuelAmount;
            this.FuelUnit = capServiceInput.FuelUnit;
            this.FuelCost = capServiceInput.FuelCost;
            this.LubeOilCost = capServiceInput.LubeOilCost;
            this.RepairCost = capServiceInput.RepairCost;
            this.LaborCost = capServiceInput.LaborCost;
            this.LaborAmount = capServiceInput.LaborAmount;
            this.ServiceUnits = capServiceInput.ServiceUnits;
            this.ServiceCapacityUnits = capServiceInput.ServiceCapacityUnits;
            this.CapitalRecoveryCost = capServiceInput.CapitalRecoveryCost;
            this.TaxesHousingInsuranceCost = capServiceInput.TaxesHousingInsuranceCost;
            this.OptionForCapacity = capServiceInput.OptionForCapacity;
            this.OptionForInflation = capServiceInput.OptionForInflation;
            this.OptionForTime = capServiceInput.OptionForTime;
            this.OptionForFuel = capServiceInput.OptionForFuel;
            this.LaborAmountAdj = capServiceInput.LaborAmountAdj;
            //set inherited capservice properties
            this.DiscountFactor = capServiceInput.DiscountFactor;
            this.ServiceUnitPrice = capServiceInput.ServiceUnitPrice;
            this.ServiceEnhanceAmount = capServiceInput.ServiceEnhanceAmount;
            this.ServiceEnhanceTimePeriod = capServiceInput.ServiceEnhanceTimePeriod;
            this.OC1Name = capServiceInput.OC1Name;
            this.OC2Name = capServiceInput.OC2Name;
            this.OC3Name = capServiceInput.OC3Name;
            this.OC4Name = capServiceInput.OC4Name;
            this.AOH1Name = capServiceInput.AOH1Name;
            this.AOH2Name = capServiceInput.AOH2Name;
            this.OC1Amount = capServiceInput.OC1Amount;
            this.OC2Amount = capServiceInput.OC2Amount;
            this.OC3Amount = capServiceInput.OC3Amount;
            this.OC4Amount = capServiceInput.OC4Amount;
            this.AOH1Amount = capServiceInput.AOH1Amount;
            this.AOH2Amount = capServiceInput.AOH2Amount;
            this.OC1Change = capServiceInput.OC1Change;
            this.OC2Change = capServiceInput.OC2Change;
            this.OC3Change = capServiceInput.OC3Change;
            this.OC4Change = capServiceInput.OC4Change;
            this.AOH1Change = capServiceInput.AOH1Change;
            this.AOH2Change = capServiceInput.AOH2Change;
            this.OC1Cost = capServiceInput.OC1Cost;
            this.OC2Cost = capServiceInput.OC2Cost;
            this.OC3Cost = capServiceInput.OC3Cost;
            this.OC4Cost = capServiceInput.OC4Cost;
            this.OCTotalCost = capServiceInput.OCTotalCost;
            this.AOH1Cost = capServiceInput.AOH1Cost;
            this.AOH2Cost = capServiceInput.AOH2Cost;
            this.AOHTotalCost = capServiceInput.AOHTotalCost;
            this.OutputsSum = capServiceInput.OutputsSum;
            this.OutputName = capServiceInput.OutputName;
            this.OutputUnits = capServiceInput.OutputUnits;
            this.OutputBeginAmount = capServiceInput.OutputBeginAmount;
            this.OutputBeginGrowthType = capServiceInput.OutputBeginGrowthType;
            this.OutputBeginChange = capServiceInput.OutputBeginChange;
            this.OutputEndGrowthType = capServiceInput.OutputEndGrowthType;
            this.OutputEndChange = capServiceInput.OutputEndChange;
            this.UseOnlyTimePeriod = capServiceInput.UseOnlyTimePeriod;
            this.OutputPeakTimePeriod = capServiceInput.OutputPeakTimePeriod;
            this.GrowthType = capServiceInput.GrowthType;
            this.AccOutputEndYrT = capServiceInput.AccOutputEndYrT;
            this.DiscOutputBeginYrT = capServiceInput.DiscOutputBeginYrT;
            this.AccCostEndYrT = capServiceInput.AccCostEndYrT;

        }
        /// <summary>
        /// Calculate costs that vary over time
        /// </summary>
        public static void CalculateCostsOverTime(CalculatorParameters calcParameters, 
            AgBudgetingHelpers.CALCULATOR_TYPES calculatorType,
            ref CapitalService1Input capitalServicesInput, int i,
            GeneralRules.GROWTH_SERIES_TYPES growthType, GeneralRules.TIME_TYPES timeType,
            GeneralRules.INFLATION_TYPES inflationType, double realRate,
            double nominalRate, double inflationInit, double inflationRate,
            int startingPeriods, int plannedUsePeriods, double RF1, double RF2,
            double gradientFactor, double valueToDiscount,
            out int timePeriods, out int timeUsed, out int remainingPeriods,
            out double sumCosts, out double sumOC3UsedCosts, out double sumOutputs)
        {
            timePeriods = 0;
            timeUsed = 0;
            remainingPeriods = 0;
            sumCosts = 0;
            string sQry = string.Empty;
            int iUsefulLifePeriods = 0;
            int iAccumPeriods = 0;
            int iNewStartingPeriod = 0;
            double dbCostEndYrT = 0;
            double dbDiscCostBeginYrT = 0;

            double dbCostYrTMinus1 = 0;
            double dbSumAccCostEndYrT = 0;
            double dbDiscOutputBeginYrT = 0;
            double dbServiceUnitPrice = 0;
            double dbServiceEnhanceAmount = 0;
            int iServiceEnhanceTP = 0;
            double dbAnnuityFactor = 0;
            double dbFirstYearRate = 0;
            double dbStoredAccCost = 0;
            double dbTempSum = 0;
            //output variables
            double dbOutputBeginAmount = 0;
            GeneralRules.GROWTH_SERIES_TYPES eOutputBeginGrowthType;
            GeneralRules.GROWTH_SERIES_TYPES eOutputEndGrowthType;
            int iUseOnlyTimePeriod = 0;
            int iOutputPeakTimePeriod = 0;

            double dbGradientOutputBeginFactor = 0;
            double dbGradientOutputEndFactor = 0;

            //init variables for this loop
            dbSumAccCostEndYrT = 0;
            iNewStartingPeriod = 0;
            sumCosts = 0;
            sumOC3UsedCosts = 0;
            sumOutputs = 0;
            dbCostYrTMinus1 = 0;
            dbCostEndYrT = 0;
            //Rule 3. if it is used equipment, OC3Cost will always be repairs
            //init time variables
            iUsefulLifePeriods = capitalServicesInput.Constants.UsefulLifeHrs;
            if (startingPeriods > 1)
            {
                //first time period
                timeUsed = (startingPeriods / plannedUsePeriods);
                remainingPeriods = (iUsefulLifePeriods - startingPeriods);
            }
            if (remainingPeriods == 0) remainingPeriods = 1;
            if (iUsefulLifePeriods == 0) iUsefulLifePeriods = 1;
            if ((timeType == GeneralRules.TIME_TYPES.costsvary)
                || (timeType == GeneralRules.TIME_TYPES.costsandoutputsvary))
            {
                timePeriods = (iUsefulLifePeriods / plannedUsePeriods);
                iAccumPeriods = plannedUsePeriods;
            }
            else if (timeType == GeneralRules.TIME_TYPES.costdonotvary)
            {
                //set time periods to one
                timePeriods = 1;
                iAccumPeriods = iUsefulLifePeriods;
            }
            if (timePeriods == 0)
            {
                timePeriods = 1;
            }
            else if (timePeriods < 0)
            {
                //negative numbers mean you entered bad data
                return;
            }

            //initialize variables
            dbServiceUnitPrice = capitalServicesInput.ServiceUnitPrice;
            dbServiceEnhanceAmount = capitalServicesInput.ServiceEnhanceAmount;
            iServiceEnhanceTP = capitalServicesInput.ServiceEnhanceTimePeriod;

            //initialize output variables
            dbOutputBeginAmount = capitalServicesInput.OutputBeginAmount;
            eOutputBeginGrowthType = (GeneralRules.GROWTH_SERIES_TYPES)capitalServicesInput.OutputBeginGrowthType;
            eOutputEndGrowthType = (GeneralRules.GROWTH_SERIES_TYPES)capitalServicesInput.OutputEndGrowthType;
            iUseOnlyTimePeriod = capitalServicesInput.UseOnlyTimePeriod;
            iOutputPeakTimePeriod = capitalServicesInput.OutputPeakTimePeriod;
            //get the gradient factors for outputs
            GetGradientFactor(eOutputBeginGrowthType,
                capitalServicesInput.OutputBeginChange, dbOutputBeginAmount,
                out dbGradientOutputBeginFactor);
            if (i == 1)
            {
                //add a time period array to capServiceInput to store calculations
                AddTimePeriods(calcParameters, timePeriods, GeneralRules.MAX_TIME_PERIODS,
                    ref capitalServicesInput);
            }
            if (valueToDiscount != 0)
            {
                //Step 3. loop through and adjust the costs and add the sums and adjustments to the collection
                for (int t = 1; t <= timePeriods; t++)
                {
                    if (t > GeneralRules.MAX_TIME_PERIODS) break;
                    TimePeriodCS currentTimePeriod
                        = capitalServicesInput.TimePeriods
                        .FirstOrDefault(tp => tp.Id == t);
                    if (currentTimePeriod == null) break;
                    if (i == 1)
                    {
                        if (t == iServiceEnhanceTP)
                        {
                            dbServiceEnhanceAmount = capitalServicesInput.ServiceEnhanceAmount;
                            currentTimePeriod.ServiceEnhanceAmount = dbServiceEnhanceAmount;
                        }
                        else
                        {
                            currentTimePeriod.ServiceEnhanceAmount = 0;
                        }
                    }
                    //Step 5. Cumulated Costs end year t
                    //adjust the initial list price for this year's inflation, if any
                    valueToDiscount = valueToDiscount * (1 + inflationRate);
                    //see equation 5.10b for numerator, these are cumulative costs -see page 5-17
                    if ((i == 3) && (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery))
                    {
                        dbCostEndYrT = ((RF1 * valueToDiscount) * System.Math.Pow((iAccumPeriods / 1000.00), RF2));
                    }
                    else
                    {
                        dbCostEndYrT = dbCostEndYrT + valueToDiscount;
                    }
                    dbSumAccCostEndYrT = dbSumAccCostEndYrT + dbCostEndYrT;

                    //Step 6. get the discounted sums and factors back from calcs component byref
                    //if option is inflation all years make sure to increase dbCostEndYrT by this year's inflation
                    CapitalServicesDiscFactor(timeType, inflationType, t,
                        inflationRate, realRate, nominalRate,
                        ref dbCostYrTMinus1, ref dbFirstYearRate,
                        ref dbAnnuityFactor, timePeriods);
                    if (dbAnnuityFactor < 0)
                    {
                        dbAnnuityFactor = dbAnnuityFactor * -1;
                    }
                    //Step 7. Adjust costs for this time period
                    //final equation 5.18;
                    //discounted, gradient-adjusted costs, at the beginning of the dbrent year
                    //equals cumlative, inflated costs at the end of the year minus cumulative, inflated costs from the previous year
                    //this could be enhanced similar to AAEA repair coeffs to account for cumulative use
                    if (growthType == GeneralRules.GROWTH_SERIES_TYPES.uniform
                        || growthType == GeneralRules.GROWTH_SERIES_TYPES.none)
                    {
                        if ((i == 1) || (i == 2) || (i == 3) || (i == 4) || (i == 5))
                        {
                            dbDiscCostBeginYrT = ((dbCostEndYrT - dbCostYrTMinus1) + (gradientFactor)) / dbAnnuityFactor;
                        }
                        else if (i == 6)
                        {
                            dbDiscCostBeginYrT = ((dbCostEndYrT - dbCostYrTMinus1) + (gradientFactor)) / dbAnnuityFactor;
                            //uniform, beginning amount
                            dbDiscOutputBeginYrT = (dbOutputBeginAmount + (dbGradientOutputBeginFactor)) / dbAnnuityFactor;
                        }
                    }
                    else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.geometric)
                    {
                        if ((i == 1) || (i == 2) || (i == 3) || (i == 4) || (i == 5))
                        {
                            //final equation
                            dbDiscCostBeginYrT = ((dbCostEndYrT - dbCostYrTMinus1) * System.Math.Pow(gradientFactor, (t - 1))) / dbAnnuityFactor;
                        }
                        else if (i == 6)
                        {
                            dbDiscCostBeginYrT = ((dbCostEndYrT - dbCostYrTMinus1) * System.Math.Pow(gradientFactor, (t - 1))) / dbAnnuityFactor;
                            dbDiscOutputBeginYrT = (dbOutputBeginAmount * System.Math.Pow(dbGradientOutputBeginFactor, (t - 1))) / dbAnnuityFactor;
                        }
                    }
                    else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.linear)
                    {
                        if ((i == 1) || (i == 2) || (i == 3) || (i == 4) || (i == 5))
                        {
                            //final equation
                            dbDiscCostBeginYrT = ((dbCostEndYrT - dbCostYrTMinus1) + (gradientFactor * t)) / dbAnnuityFactor;
                        }
                        else if (i == 6)
                        {
                            dbDiscCostBeginYrT = ((dbCostEndYrT - dbCostYrTMinus1) + (gradientFactor * t)) / dbAnnuityFactor;
                            dbDiscOutputBeginYrT = (dbOutputBeginAmount + (dbGradientOutputBeginFactor * t)) / dbAnnuityFactor;
                        }
                    }

                    //keep running sums; final sum will often be converted into an annuity when an average cost is desired
                    sumCosts += dbDiscCostBeginYrT;
                    if ((t == iOutputPeakTimePeriod) && (i == 6))
                    {
                        //gradient changes after peak output
                        GetGradientFactor(eOutputEndGrowthType,
                            capitalServicesInput.OutputEndChange, dbOutputBeginAmount,
                            out dbGradientOutputEndFactor);
                    }
                    //if it's used, store the costs at end of the starting hours
                    if (timeUsed > 1)
                    {
                        if ((t == timeUsed) || (timePeriods == 1))
                        {
                            //Rule enforced that if starting periods > then oc3 is repairs of some nature
                            if ((i == 3) && (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery))
                            {
                                //cumulative costs in current period -based on accum hrs and initial list price
                                sumOC3UsedCosts = dbCostEndYrT;
                            }
                            else if ((i == 3) && (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.capitalservices))
                            {
                                //cumulative repair costs
                                sumOC3UsedCosts = sumCosts;
                            }
                        }
                    }

                    //accumulating service units (v. 1.1 only allowed some costs to vary - why?)
                    if ((i == 1) || (i == 2) || (i == 3) || (i == 4) || (i == 5) || (i == 6))
                    {
                        iNewStartingPeriod = iNewStartingPeriod + plannedUsePeriods;
                        if (timeType != GeneralRules.TIME_TYPES.costdonotvary)
                        {
                            iAccumPeriods = iAccumPeriods + plannedUsePeriods;
                            iUsefulLifePeriods = iUsefulLifePeriods - plannedUsePeriods;
                        }
                        if (iUsefulLifePeriods == 0) iUsefulLifePeriods = 1;
                    }

                    //if inflation is used only for the first year's costs
                    if ((inflationType == GeneralRules.INFLATION_TYPES.inflationyesfirst)
                        && (timePeriods > 1))
                        inflationRate = 0;
                    //set the element's cost totals
                    SetTimeCosts(i, dbCostEndYrT, dbCostYrTMinus1, gradientFactor,
                        dbDiscCostBeginYrT, sumCosts, iNewStartingPeriod, iAccumPeriods,
                        iUsefulLifePeriods, dbAnnuityFactor, ref currentTimePeriod);
                    //set summary variables for calling method
                    if (i == 6)
                    {
                        //outputs handled on last pass
                        sumOutputs += dbDiscOutputBeginYrT;
                        //its actually beginning of year, mistake was using inflated costs to set this property up
                        currentTimePeriod.AccOutputEndYrT = sumOutputs;
                        //unit cost is just cost / the following number each year
                        currentTimePeriod.DiscOutputBeginYrT = dbDiscOutputBeginYrT;
                        //change each period, not just beginning
                        dbTempSum = dbOutputBeginAmount - dbDiscOutputBeginYrT;
                        currentTimePeriod.OutputBeginChange = dbTempSum;
                    }
                    //set accumulating costs
                    dbStoredAccCost = currentTimePeriod.AccCostEndYrT;
                    dbTempSum = dbStoredAccCost + dbSumAccCostEndYrT;
                    currentTimePeriod.AccCostEndYrT = dbTempSum;

                    //the end of this year has already been adjusted for inflation; this filters out the inflation added in the calc function
                    if ((i == 3) && (calculatorType == AgBudgetingHelpers.CALCULATOR_TYPES.agmachinery))
                    {
                        dbCostYrTMinus1 = ((RF1 * valueToDiscount) * System.Math.Pow((iNewStartingPeriod / 1000.00), RF2));
                    }
                    else
                    {
                        dbCostYrTMinus1 = dbCostEndYrT;
                    }
                }//for t loop
            }
        }
        /// <summary>
        /// Get a gradient factor for use with growth series
        /// </summary>
        public static void GetGradientFactor(
            GeneralRules.GROWTH_SERIES_TYPES growthType, double costChangeAmount,
            double costAmount, out double gradientFactor)
        {
            gradientFactor = 0;
            if (growthType == GeneralRules.GROWTH_SERIES_TYPES.uniform)
            {
                gradientFactor = costChangeAmount;
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.linear)
            {
                //this is not correct
                gradientFactor = costChangeAmount;
            }
            else if (growthType == GeneralRules.GROWTH_SERIES_TYPES.geometric)
            {
                if (costChangeAmount == 0)
                {
                    gradientFactor = 1;
                }
                else
                {
                    gradientFactor = 1 + (costChangeAmount / costAmount);
                }
            }
        }

        public static void AddTimePeriods(CalculatorParameters calcParameters, 
            int numberToAppend, int maxTimePeriods,
            ref CapitalService1Input capitalServicesInput)
        {
            List<TimePeriodCS> lstTPs = new List<TimePeriodCS>();
            for (int t = 1; t <= numberToAppend; t++)
            {
                if (t > maxTimePeriods) break;
                TimePeriodCS tp = new TimePeriodCS(calcParameters, capitalServicesInput);
                tp.Id = t;
                tp.Name = "Period " + t.ToString();
                lstTPs.Add(tp);
            }
            //convert the list to an array.
            TimePeriodCS[] timeperiods = lstTPs.ToArray();
            //set the array
            capitalServicesInput.TimePeriods = timeperiods;
        }
        /// <summary>
        /// Purpose:       calculate generic capital services costs;
        /// References:    AAEA Equation 5.18 and 5.10b; AAEA Appendix 6A and 10A and Chapter 2
        /// </summary>
        public static void CapitalServicesDiscFactor(GeneralRules.TIME_TYPES timeType,
            GeneralRules.INFLATION_TYPES inflationType, int t,
            double inflationRate, double realRate, double nomRate,
            ref double infCostEndYrTMinus1, ref double firstYrRate,
            ref double annuityFactor, int timePeriods)
        {
            annuityFactor = 1;

            //see equation 5.18 for sngAnnuityFactor and inflation options
            if (inflationType == GeneralRules.INFLATION_TYPES.inflationyesfirst)
            {
                if (t == 1)
                {
                    annuityFactor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
                    firstYrRate = annuityFactor;
                }
                else
                {
                    //need to include the first year inflated discount rate
                    annuityFactor = (System.Math.Pow((1 + inflationRate), (t - 1)) * System.Math.Pow((1 + realRate), (t - 1))) * (firstYrRate);
                }
            }
            else if (inflationType == GeneralRules.INFLATION_TYPES.inflationyesall)
            {
                //don't make any special provisions for first year
                infCostEndYrTMinus1 = infCostEndYrTMinus1 * (1 + inflationRate);
                annuityFactor = (System.Math.Pow((1 + inflationRate), t) * System.Math.Pow((1 + realRate), t));
            }
            else if (inflationType == GeneralRules.INFLATION_TYPES.inflationno)
            {
                annuityFactor = System.Math.Pow((1 + realRate), t);
            }

            //account for option to not vary costs over time
            if (timePeriods == 1)
            {
                annuityFactor = 1;
            }
            return;
        }
        /// <summary>
        /// set time dependent costs
        /// </summary>
        public static void SetTimeCosts(int iCostVariable, double costEndYrT,
            double costYrTMinus1, double gradientFactor, double discCostBeginYrT,
            double sumCosts, int newStartingPeriod, int accumPeriods,
            int usefulLifePeriods, double annuityFactor, ref TimePeriodCS timePeriod)
        {
            double dbStoredTotal = 0;
            double dbAmount = (costEndYrT - costYrTMinus1);
            double dbCost = 0;
            switch (iCostVariable)
            {
                case 1:
                    timePeriod.OC1Amount = dbAmount;
                    //gradient factors stored in change parameter
                    timePeriod.OC1Change = gradientFactor;
                    timePeriod.OC1Cost = discCostBeginYrT;
                    timePeriod.OCTotalCost = sumCosts;
                    //same for each capital service element (for all six costs)
                    timePeriod.Constants.StartingHrs = newStartingPeriod;
                    timePeriod.Constants.PlannedUseHrs = accumPeriods;
                    timePeriod.Constants.UsefulLifeHrs = usefulLifePeriods;
                    timePeriod.DiscountFactor = annuityFactor;
                    break;
                case 2:
                    timePeriod.OC2Amount = dbAmount;
                    timePeriod.OC2Change = gradientFactor;
                    timePeriod.OC2Cost = discCostBeginYrT;
                    dbStoredTotal = timePeriod.OCTotalCost;
                    dbCost = (dbStoredTotal + sumCosts);
                    timePeriod.OCTotalCost = dbCost;
                    break;
                case 3:
                    timePeriod.OC3Amount = dbAmount;
                    timePeriod.OC3Change = gradientFactor;
                    timePeriod.OC3Cost = discCostBeginYrT;
                    dbStoredTotal = timePeriod.OCTotalCost;
                    dbCost = (dbStoredTotal + sumCosts);
                    timePeriod.OCTotalCost = dbCost;
                    break;
                case 4:
                    timePeriod.OC4Amount = dbAmount;
                    timePeriod.OC4Change = gradientFactor;
                    timePeriod.OC4Cost = discCostBeginYrT;
                    dbStoredTotal = timePeriod.OCTotalCost;
                    dbCost = (dbStoredTotal + sumCosts);
                    timePeriod.OCTotalCost = dbCost;
                    break;
                case 5:
                    timePeriod.AOH1Amount = dbAmount;
                    timePeriod.AOH1Change = gradientFactor;
                    timePeriod.AOH1Cost = discCostBeginYrT;
                    timePeriod.AOHTotalCost = sumCosts;
                    break;
                case 6:
                    timePeriod.AOH2Amount = dbAmount;
                    timePeriod.AOH2Change = gradientFactor;
                    timePeriod.AOH2Cost = discCostBeginYrT;
                    dbStoredTotal = timePeriod.AOHTotalCost;
                    dbCost = (dbStoredTotal + sumCosts);
                    timePeriod.AOHTotalCost = dbCost;
                    break;
                default:
                    break;
            }
        }
    }
}
