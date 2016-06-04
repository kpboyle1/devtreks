using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		This class extends the base OperationComponentClass with 
    ///             labor scheduling and timeliness penalty parameters. These 
    ///             parameters are used in calculators and resource stock analyzers 
    ///             to schedule operations and to penalize late operations.
    ///Author:		www.devtreks.org
    ///Date:		2014, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    public class TimelinessOpComp1 : OperationComponent
    {
        public TimelinessOpComp1()
            : base()
        {
            //no nulls
            InitTimelinessOC1Properties();
        }
        //copy constructor
        public TimelinessOpComp1(TimelinessOpComp1 npvOC)
            : base()
        {
            CopyTimelinessOC1Properties(npvOC);
        }
        //scheduling and selection optimization collection
        //each member is a feasible combination of power and nopower input for that opcomp
        //the opcomp has a new timeliness penalty for that combo of inputs
        public List<TimelinessOpComp1> TimelinessOpComps { get; set; }

        //planned start date for operation
        public DateTime PlannedStartDate { get; set; }
        //actual start date for operation (scheduling analyzers optimize by changing dates)
        public DateTime ActualStartDate { get; set; }
        //labor available to carry out operation (hours per day)
        public double LaborAvailable { get; set; }
        //workday probability percentage (probablility that work conditions, 
        //such as weather, will allow work to be completed)
        public double WorkdayProbability { get; set; }
        //per day output percentage loss due to untimely operations
        public double TimelinessPenalty1 { get; set; }
        //number of days from start date when timeliness penalty 1 starts
        public double TimelinessPenaltyDaysFromStart1 { get; set; }
        //additional per day output percentage loss due to untimely operations
        public double TimelinessPenalty2 { get; set; }
        //additional number of days from start date when timeliness penalty 2 starts
        public double TimelinessPenaltyDaysFromStart2 { get; set; }
        //total number of days that operation or component must be completed 
        //(i.e. forces operation to choose larger equipment)
        public double WorkdaysLimit { get; set; }
        //Input OC amount
        //(i.e. field capacity or hrs per acre for power input (has to be inverted in calcs))
        public double FieldCapacity { get; set; }
        //calculated results
        //(i.e. acres covered per day)
        public double AreaCovered { get; set; }
        //(i.e. field days needed to cover total area)
        public double FieldDays { get; set; }
        //name of output for timeliness penalty
        public string OutputName { get; set; }
        //unit of output for timeliness penalty
        public string OutputUnit { get; set; }
        //price of output for timeliness penalty
        public double OutputPrice { get; set; }
        //yield of output for timeliness penalty
        public double OutputYield { get; set; }
        //composition amount out output for timeliness penalty
        public double CompositionAmount { get; set; }
        //unit of composition for timeliness penalty
        public string CompositionUnit { get; set; }
        //times harvested
        public double OutputTimes { get; set; }
        //probable Field Days Needed
        public double ProbableFieldDays { get; set; }
        //probable date that operation will be finished (uses workdayprobability to calc)
        public DateTime ProbableFinishDate { get; set; }
        //timeliness penalty cost (see calcs below)
        public double TimelinessPenaltyCost { get; set; }
        //occost and aohcost are per hour
        public double TimelinessPenaltyCostPerHour { get; set; }
        //total revenue used to compute penaltu
        //uses TotalR from base calculator

        public const string cPlannedStartDate = "PlannedStartDate";
        public const string cActualStartDate = "ActualStartDate";
        public const string cLaborAvailable = "LaborAvailable";
        public const string cWorkdayProbability = "WorkdayProbability";
        public const string cTimelinessPenalty1 = "TimelinessPenalty1";
        public const string cTimelinessPenaltyDaysFromStart1 = "TimelinessPenaltyDaysFromStart1";
        public const string cTimelinessPenalty2 = "TimelinessPenalty2";
        public const string cTimelinessPenaltyDaysFromStart2 = "TimelinessPenaltyDaysFromStart2";
        public const string cWorkdaysLimit = "WorkdaysLimit";
        public const string cFieldCapacity = "FieldCapacity";
        public const string cAreaCovered = "AreaCovered";
        public const string cFieldDays = "FieldDays";
        public const string cOutputName = "OutputName";
        public const string cOutputUnit = "OutputUnit";
        public const string cOutputPrice = "OutputPrice";
        public const string cOutputYield = "OutputYield";
        public const string cCompositionUnit = "CompositionUnit";
        public const string cCompositionAmount = "CompositionAmount";
        public const string cOutputTimes = "OutputTimes";
        //composition names are in output constants
        public const string cProbableFieldDays = "ProbableFieldDays";
        public const string cProbableFinishDate = "ProbableFinishDate";
        public const string cTimelinessPenaltyCost = "TimelinessPenaltyCost";
        public const string cTimelinessPenaltyCostPerHour = "TimelinessPenaltyCostPerHour";

        public virtual void InitTimelinessOC1Properties()
        {
            //avoid null references to properties
            this.PlannedStartDate = CalculatorHelpers.GetDateShortNow();
            this.ActualStartDate = CalculatorHelpers.GetDateShortNow();
            this.LaborAvailable = 0;
            this.WorkdayProbability = 100;
            this.TimelinessPenalty1 = 0;
            this.TimelinessPenaltyDaysFromStart1 = 0;
            this.TimelinessPenalty2 = 0;
            this.TimelinessPenaltyDaysFromStart2 = 0;
            this.WorkdaysLimit = 0;
            //base props
            this.Amount = 0;
            this.Unit = string.Empty;
            //base input prop
            this.FieldCapacity = 0;
            //calculated results
            this.AreaCovered = 0;
            this.FieldDays = 0;

            this.OutputName = string.Empty;
            this.OutputUnit = string.Empty;
            this.OutputPrice = 0;
            this.OutputYield = 0;
            this.CompositionAmount = 1;
            this.CompositionUnit = "each";
            this.OutputTimes = 1;
            this.ProbableFieldDays = 0;
            this.ProbableFinishDate = CalculatorHelpers.GetDateShortNow();
            this.TimelinessPenaltyCost = 0;
            this.TimelinessPenaltyCostPerHour = 0;
            this.TotalR = 0;
        }
        public virtual void CopyTimelinessOC1Properties(
           TimelinessOpComp1 calculator)
        {
            this.PlannedStartDate = calculator.PlannedStartDate;
            this.ActualStartDate = calculator.ActualStartDate;
            this.LaborAvailable = calculator.LaborAvailable;
            this.WorkdayProbability = calculator.WorkdayProbability;
            this.TimelinessPenalty1 = calculator.TimelinessPenalty1;
            this.TimelinessPenaltyDaysFromStart1 = calculator.TimelinessPenaltyDaysFromStart1;
            this.TimelinessPenalty2 = calculator.TimelinessPenalty2;
            this.TimelinessPenaltyDaysFromStart2 = calculator.TimelinessPenaltyDaysFromStart2;
            this.WorkdaysLimit = calculator.WorkdaysLimit;
            this.Amount = calculator.Amount;
            this.Unit = calculator.Unit;
            this.FieldCapacity = calculator.FieldCapacity;
            this.AreaCovered = calculator.AreaCovered;
            this.FieldDays = calculator.FieldDays;
            this.OutputName = calculator.OutputName;
            this.OutputUnit = calculator.OutputUnit;
            this.OutputPrice = calculator.OutputPrice;
            this.OutputYield = calculator.OutputYield;
            this.CompositionAmount = calculator.CompositionAmount;
            this.CompositionUnit = calculator.CompositionUnit;
            this.OutputTimes = calculator.OutputTimes;
            this.ProbableFieldDays = calculator.ProbableFieldDays;
            this.ProbableFinishDate = calculator.ProbableFinishDate;
            this.TimelinessPenaltyCost = calculator.TimelinessPenaltyCost;
            this.TimelinessPenaltyCostPerHour = calculator.TimelinessPenaltyCostPerHour;
            this.TotalR = calculator.TotalR;
        }
        //set the class properties using the XElement
        public virtual void SetTimelinessOC1Properties(XElement currentCalculationsElement)
        {
            this.PlannedStartDate = CalculatorHelpers.GetAttributeDate(currentCalculationsElement, cPlannedStartDate);
            this.ActualStartDate = CalculatorHelpers.GetAttributeDate(currentCalculationsElement, cActualStartDate);
            this.LaborAvailable = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cLaborAvailable);
            this.WorkdayProbability = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cWorkdayProbability);
            this.TimelinessPenalty1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimelinessPenalty1);
            this.TimelinessPenaltyDaysFromStart1 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimelinessPenaltyDaysFromStart1);
            this.TimelinessPenalty2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimelinessPenalty2);
            this.TimelinessPenaltyDaysFromStart2 = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimelinessPenaltyDaysFromStart2);
            this.WorkdaysLimit = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cWorkdaysLimit);
            this.Amount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, Constants.AMOUNT);
            this.Unit = CalculatorHelpers.GetAttribute(currentCalculationsElement, Constants.UNIT);
            this.FieldCapacity = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFieldCapacity);
            this.AreaCovered = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cAreaCovered);
            this.FieldDays = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cFieldDays);

            this.OutputName = CalculatorHelpers.GetAttribute(currentCalculationsElement, cOutputName);
            this.OutputUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cOutputUnit);
            this.OutputPrice = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputPrice);
            this.OutputYield = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputYield);
            this.CompositionUnit = CalculatorHelpers.GetAttribute(currentCalculationsElement, cCompositionUnit);
            this.CompositionAmount = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cCompositionAmount);
            this.OutputTimes = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cOutputTimes);
            this.ProbableFieldDays = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cProbableFieldDays);
            this.ProbableFinishDate = CalculatorHelpers.GetAttributeDate(currentCalculationsElement, cProbableFinishDate);
            this.TimelinessPenaltyCost = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimelinessPenaltyCost);
            this.TimelinessPenaltyCostPerHour = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, cTimelinessPenaltyCostPerHour);
            this.TotalR = CalculatorHelpers.GetAttributeDouble(currentCalculationsElement, CostBenefitCalculator.TR);
        }
        public virtual void SetTimelinessOC1Properties(string attName,
            string attValue)
        {
            switch (attName)
            {
                case cPlannedStartDate:
                    this.PlannedStartDate = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cActualStartDate:
                    this.ActualStartDate = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cLaborAvailable:
                    this.LaborAvailable = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWorkdayProbability:
                    this.WorkdayProbability = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTimelinessPenalty1:
                    this.TimelinessPenalty1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTimelinessPenaltyDaysFromStart1:
                    this.TimelinessPenaltyDaysFromStart1 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTimelinessPenalty2:
                    this.TimelinessPenalty2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTimelinessPenaltyDaysFromStart2:
                    this.TimelinessPenaltyDaysFromStart2 = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cWorkdaysLimit:
                    this.WorkdaysLimit = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case Constants.AMOUNT:
                    this.Amount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case Constants.UNIT:
                    this.Unit = attValue;
                    break;
                case cFieldCapacity:
                    this.FieldCapacity = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cAreaCovered:
                    this.AreaCovered = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cFieldDays:
                    this.FieldDays = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOutputName:
                    this.OutputName = attValue;
                    break;
                case cOutputUnit:
                    this.OutputUnit = attValue;
                    break;
                case cOutputPrice:
                    this.OutputPrice = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOutputYield:
                    this.OutputYield = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cCompositionUnit:
                    this.CompositionUnit = attValue;
                    break;
                case cCompositionAmount:
                    this.CompositionAmount = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cOutputTimes:
                    this.OutputTimes = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProbableFieldDays:
                    this.ProbableFieldDays = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cProbableFinishDate:
                    this.ProbableFinishDate = CalculatorHelpers.ConvertStringToDate(attValue);
                    break;
                case cTimelinessPenaltyCost:
                    this.TimelinessPenaltyCost = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case cTimelinessPenaltyCostPerHour:
                    this.TimelinessPenaltyCostPerHour = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                case TR:
                    this.TotalR = CalculatorHelpers.ConvertStringToDouble(attValue);
                    break;
                default:
                    break;
            }
        }
        public void SetTimelinessBaseProperties(XElement currentElement)
        {
            //need some base props to be displayed in calculator
            //opcomp.amount 
            this.Amount = CalculatorHelpers.GetAttributeDouble(currentElement, Constants.AMOUNT);
            //opcomp.unit
            this.Unit = CalculatorHelpers.GetAttribute(currentElement, Constants.UNIT);
        }
        public bool AddCalculationsProperties(double powerInputOCAmount, DateTime opCompDate)
        {
            bool bHasCalculations = false;
            //adjust startdate to same year
            AddStartingProperties(powerInputOCAmount, opCompDate);
            //163 started using outcome.amount in tp calcs
            double dbTotalOutputBenefit = this.Amount * this.OutputPrice * this.OutputYield * this.CompositionAmount * this.OutputTimes;
            this.TotalR = dbTotalOutputBenefit;
            bHasCalculations = AddTimelinessPenaltyProperties();
            return bHasCalculations;
        }
        private void AddStartingProperties(double powerInputOCAmount, DateTime opCompDate)
        {
            //adjust startdate to same year
            if (this.PlannedStartDate.Year != opCompDate.Year)
            {
                this.PlannedStartDate = new DateTime(opCompDate.Year, this.PlannedStartDate.Month, this.PlannedStartDate.Day);
            }
            //actual start date can only be changed programmatically in bianalyzers
            //using SetTimelinessPenaltyNewActualDate
            this.ActualStartDate = this.PlannedStartDate;
            //convert hrs. per acre to acres per hour
            this.FieldCapacity = (powerInputOCAmount != 0) ? (1 / powerInputOCAmount) : 0;
            //area covered is total acreage (5 acres per hour * 10 hours available = 50 acres per day)
            this.AreaCovered = this.FieldCapacity * this.LaborAvailable;
        }
        private bool AddTimelinessPenaltyProperties()
        {
            bool bHasCalculations = false;
            //actual start date can only be changed programmatically in bianalyzers
            //using SetTimelinessPenaltyNewActualDate
            this.ActualStartDate = this.PlannedStartDate;
            this.FieldDays = (this.AreaCovered != 0) ? (this.Amount / this.AreaCovered) : 0;
            //the number they enter whether 10 or .01 is the probability
            double dbWorkdayProbablity = this.WorkdayProbability / 100;
            dbWorkdayProbablity = (dbWorkdayProbablity != 0) ? dbWorkdayProbablity : -1;
            this.ProbableFieldDays = this.FieldDays / dbWorkdayProbablity;
            this.ProbableFinishDate = this.ActualStartDate.AddDays(this.ProbableFieldDays);
            TimeSpan tsPlannedvsActualPenaltyDays = this.PlannedStartDate - this.ActualStartDate;
            int iDays = tsPlannedvsActualPenaltyDays.Days;
            if (iDays < 0)
            {
                if (this.PlannedStartDate < this.ActualStartDate)
                {
                    iDays = iDays * -1;
                }
            }
            else if (iDays > 0)
            {
                if (this.PlannedStartDate > this.ActualStartDate)
                {
                    iDays = iDays * -1;
                }
            }
            double dbPenaltyDays = (iDays + this.ProbableFieldDays) - this.TimelinessPenaltyDaysFromStart1;
            //updated 163 to use totalr
            // double dbTotalOutputBenefit = this.Amount * this.OutputPrice * this.OutputYield * this.CompositionAmount * this.OutputTimes;
            //this.TotalR = dbTotalOutputBenefit;
            if (dbPenaltyDays > 0)
            {
                //the number they enter whether 10 or .01 is the penalty
                double dbTimelinessPenalty = this.TimelinessPenalty1 / 100;
                //penalty runs for all days
                this.TimelinessPenaltyCost = this.TotalR * dbTimelinessPenalty * dbPenaltyDays;
                if (this.TimelinessPenaltyCost > this.TotalR)
                {
                    this.TimelinessPenaltyCost = this.TotalR;
                }
                //add additional penalty (i.e. 65 - (30 + 30))
                dbPenaltyDays = (iDays + this.ProbableFieldDays) - (this.TimelinessPenaltyDaysFromStart1 + this.TimelinessPenaltyDaysFromStart2);
                if (dbPenaltyDays > 0)
                {
                    //additional penalty
                    //i.e. (1) / 100 = .01 penalty
                    dbTimelinessPenalty = (this.TimelinessPenalty2) / 100;
                    //additional penalty runs only for additional days
                    this.TimelinessPenaltyCost = this.TimelinessPenaltyCost + (this.TotalR * dbTimelinessPenalty * dbPenaltyDays);
                    if (this.TimelinessPenaltyCost > this.TotalR)
                    {
                        this.TimelinessPenaltyCost = this.TotalR;
                    }
                }
            }
            else
            {
                this.TimelinessPenaltyCost = 0;
            }
            if (this.FieldCapacity == 0)
            {
                this.FieldCapacity = -1;
            }
            //1000 / (100 hours = .2 hrs/acre * 500 acres)
            double dbTotalHours = (1 / this.FieldCapacity) * this.Amount;
            if (dbTotalHours > 0)
            {
                this.TimelinessPenaltyCostPerHour = this.TimelinessPenaltyCost / dbTotalHours;
            }
            else
            {
                this.TimelinessPenaltyCostPerHour = 0;
            }
            bHasCalculations = true;
            return bHasCalculations;
        }
        public bool ReSetParentTimePeriodProperties(double powerInputOCAmount, DateTime opCompDate,
            double timePeriodAmount, List<Outcome> outcomes)
        {
            //called when a new combo of inputs with new powerocamount are built
            bool bHasCalculations = false;
            AddStartingProperties(powerInputOCAmount, opCompDate);
            bHasCalculations = ReSetParentTimePeriodProperties(timePeriodAmount, outcomes);
            return bHasCalculations;
        }
        public bool ReSetParentTimePeriodProperties(double timePeriodAmount,
            List<Outcome> outcomes)
        {
            //called in regular stock2 calculations
            bool bHasCalculations = false;
            //reset the acreage by the output amount (1 acre * 500 acres)
            //note that budgets should list per acre operations and opcomp.amount should be reset to 1 
            if (timePeriodAmount <= 0)
            {
                timePeriodAmount = 1;
            }
            //set the initial output props
            this.TotalR = this.Amount * this.OutputPrice * this.OutputYield * this.CompositionAmount * this.OutputTimes;
            //change the outputproperties from the speculative opcomp.output to the real output
            this.ResetOutcomeProperties(outcomes);
            this.TotalR = this.TotalR * timePeriodAmount;
            this.Amount = this.Amount * timePeriodAmount;
            //reset all of the calculations
            bHasCalculations = AddTimelinessPenaltyProperties();
            return bHasCalculations;
        }

        //163 started using tr to calc penalty
        private bool ResetOutcomeProperties(List<Outcome> outcomes)
        {
            bool bHasNewOutputs = false;
            TimelinessOpComp1 tempCalcs = new TimelinessOpComp1();
            if (outcomes != null)
            {
                if (outcomes.Count > 0)
                {
                    foreach (Outcome outcome in outcomes)
                    {
                        if (outcome.Outputs != null)
                        {
                            foreach (Output output in outcome.Outputs)
                            {
                                //don't process when machin calcs have not beeen completed
                                if (this.FieldCapacity != 0)
                                {
                                    //either name must be contained in the other
                                    if (output.Name.ToLower().IndexOf(this.OutputName.ToLower()) >= 0
                                        || this.OutputName.ToLower().IndexOf(output.Name.ToLower()) >= 0)
                                    {
                                        if (output.CompositionAmount <= 0)
                                        {
                                            output.CompositionAmount = 1;
                                        }
                                        if (output.Times <= 0)
                                        {
                                            output.Times = 1;
                                        }
                                        //use the one with the highest revenue
                                        output.TotalR = outcome.Amount * output.CompositionAmount * output.Times * output.Amount * output.Price;
                                        if (output.TotalR > tempCalcs.TotalR)
                                        {
                                            tempCalcs.OutputName = output.Name;
                                            tempCalcs.OutputPrice = output.Price;
                                            tempCalcs.OutputUnit = output.Unit;
                                            tempCalcs.OutputYield = output.Amount;
                                            tempCalcs.CompositionAmount = output.CompositionAmount;
                                            tempCalcs.CompositionUnit = output.CompositionUnit;
                                            tempCalcs.OutputTimes = output.Times;
                                            tempCalcs.TotalR = output.TotalR;
                                            bHasNewOutputs = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //use first output 
                    if (!bHasNewOutputs)
                    {
                        if (outcomes.FirstOrDefault().Outputs != null)
                        {
                            Output output = outcomes.FirstOrDefault().Outputs.FirstOrDefault();
                            this.OutputName = output.Name;
                            this.OutputPrice = output.Price;
                            this.OutputUnit = output.Unit;
                            this.OutputYield = output.Amount;
                            this.CompositionAmount = output.CompositionAmount;
                            this.CompositionUnit = output.CompositionUnit;
                            this.OutputTimes = output.Times;
                            this.TotalR = output.TotalR;
                            bHasNewOutputs = true;
                        }
                    }
                    else
                    {
                        //switch to this tps output
                        this.OutputName = tempCalcs.OutputName;
                        this.OutputPrice = tempCalcs.OutputPrice;
                        this.OutputUnit = tempCalcs.OutputUnit;
                        this.OutputYield = tempCalcs.OutputYield;
                        this.CompositionAmount = tempCalcs.CompositionAmount;
                        this.CompositionUnit = tempCalcs.CompositionUnit;
                        this.OutputTimes = tempCalcs.OutputTimes;
                        this.TotalR = tempCalcs.TotalR;
                    }
                }
            }
            return bHasNewOutputs;
        }
        
        public void SetTimelinessOC1Attributes(string attNameExtension,
           XElement currentCalculationsElement)
        {
            //don't set any input attributes; each calculator should set what's needed separately
            CalculatorHelpers.SetAttributeDateS(currentCalculationsElement,
                string.Concat(cPlannedStartDate, attNameExtension), this.PlannedStartDate);
            CalculatorHelpers.SetAttributeDateS(currentCalculationsElement,
                string.Concat(cActualStartDate, attNameExtension), this.ActualStartDate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cLaborAvailable, attNameExtension), this.LaborAvailable);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cWorkdayProbability, attNameExtension), this.WorkdayProbability);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTimelinessPenalty1, attNameExtension), this.TimelinessPenalty1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTimelinessPenaltyDaysFromStart1, attNameExtension), this.TimelinessPenaltyDaysFromStart1);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTimelinessPenalty2, attNameExtension), this.TimelinessPenalty2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTimelinessPenaltyDaysFromStart2, attNameExtension), this.TimelinessPenaltyDaysFromStart2);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cWorkdaysLimit, attNameExtension), this.WorkdaysLimit);

            //child power input ocamount
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFieldCapacity, attNameExtension), this.FieldCapacity);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cAreaCovered, attNameExtension), this.AreaCovered);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cFieldDays, attNameExtension), this.FieldDays);

            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cOutputName, attNameExtension), this.OutputName);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cOutputUnit, attNameExtension), this.OutputUnit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputPrice, attNameExtension), this.OutputPrice);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputYield, attNameExtension), this.OutputYield);
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(cCompositionUnit, attNameExtension), this.CompositionUnit);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cCompositionAmount, attNameExtension), this.CompositionAmount);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cOutputTimes, attNameExtension), this.OutputTimes);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cProbableFieldDays, attNameExtension), this.ProbableFieldDays);
            CalculatorHelpers.SetAttributeDateS(currentCalculationsElement,
                string.Concat(cProbableFinishDate, attNameExtension), this.ProbableFinishDate);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTimelinessPenaltyCost, attNameExtension), this.TimelinessPenaltyCost);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(cTimelinessPenaltyCostPerHour, attNameExtension), this.TimelinessPenaltyCostPerHour);
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(CostBenefitCalculator.TR, attNameExtension), this.TotalR);
        }
        public virtual void SetTimelinessOC1Attributes(string attNameExtension,
           XmlWriter writer)
        {
            writer.WriteAttributeString(string.Concat(cPlannedStartDate, attNameExtension), this.PlannedStartDate.ToString());
            writer.WriteAttributeString(string.Concat(cActualStartDate, attNameExtension), this.ActualStartDate.ToString());
            writer.WriteAttributeString(string.Concat(cLaborAvailable, attNameExtension), this.LaborAvailable.ToString());
            writer.WriteAttributeString(string.Concat(cWorkdayProbability, attNameExtension), this.WorkdayProbability.ToString());
            writer.WriteAttributeString(string.Concat(cTimelinessPenalty1, attNameExtension), this.TimelinessPenalty1.ToString());
            writer.WriteAttributeString(string.Concat(cTimelinessPenaltyDaysFromStart1, attNameExtension), this.TimelinessPenaltyDaysFromStart1.ToString());
            writer.WriteAttributeString(string.Concat(cTimelinessPenalty2, attNameExtension), this.TimelinessPenalty2.ToString());
            writer.WriteAttributeString(string.Concat(cTimelinessPenaltyDaysFromStart2, attNameExtension), this.TimelinessPenaltyDaysFromStart2.ToString());
            writer.WriteAttributeString(string.Concat(cWorkdaysLimit, attNameExtension), this.WorkdaysLimit.ToString());

            writer.WriteAttributeString(string.Concat(cFieldCapacity, attNameExtension), this.FieldCapacity.ToString());
            writer.WriteAttributeString(string.Concat(cAreaCovered, attNameExtension), this.AreaCovered.ToString());
            writer.WriteAttributeString(string.Concat(cFieldDays, attNameExtension), this.FieldDays.ToString());

            writer.WriteAttributeString(string.Concat(cOutputName, attNameExtension), this.OutputName);
            writer.WriteAttributeString(string.Concat(cOutputUnit, attNameExtension), this.OutputUnit);
            writer.WriteAttributeString(string.Concat(cOutputPrice, attNameExtension), this.OutputPrice.ToString());
            writer.WriteAttributeString(string.Concat(cOutputYield, attNameExtension), this.OutputYield.ToString());
            writer.WriteAttributeString(string.Concat(cCompositionUnit, attNameExtension), this.CompositionUnit);
            writer.WriteAttributeString(string.Concat(cCompositionAmount, attNameExtension), this.CompositionAmount.ToString());
            writer.WriteAttributeString(string.Concat(cOutputTimes, attNameExtension), this.OutputTimes.ToString());
            writer.WriteAttributeString(string.Concat(cProbableFieldDays, attNameExtension), this.ProbableFieldDays.ToString());
            writer.WriteAttributeString(string.Concat(cProbableFinishDate, attNameExtension), this.ProbableFinishDate.ToString());
            writer.WriteAttributeString(string.Concat(cTimelinessPenaltyCost, attNameExtension), this.TimelinessPenaltyCost.ToString());
            writer.WriteAttributeString(string.Concat(cTimelinessPenaltyCostPerHour, attNameExtension), this.TimelinessPenaltyCostPerHour.ToString());
            writer.WriteAttributeString(string.Concat(CostBenefitCalculator.TR, attNameExtension), this.TotalR.ToString());
        }
        public void SetTimelinessBaseAttributes(string attNameExtension,
           XElement currentCalculationsElement)
        {
            //need some base props to be displayed in calculator
            //opcomp.amount 
            CalculatorHelpers.SetAttributeDoubleF3(currentCalculationsElement,
                string.Concat(Constants.AMOUNT, attNameExtension), this.Amount);
            //opcomp.unit
            CalculatorHelpers.SetAttribute(currentCalculationsElement,
                string.Concat(Constants.UNIT, attNameExtension), this.Unit);
        }
        public static bool TimelinessPenaltyIncreasesUsingNewDate(TimelinessOpComp1 tocToChange,
            DateTime newActualDate)
        {
            bool bCostIncreases = false;
            double startingPenaltyCost = tocToChange.TimelinessPenaltyCost;
            //returns the date adjusted toc to calling procedure, so send in a new one
            SetTimelinessPenaltyNewActualDate(tocToChange, newActualDate);
            if (tocToChange.TimelinessPenaltyCost > startingPenaltyCost)
            {
                bCostIncreases = true;
            }
            return bCostIncreases;
        }
        public static void SetTimelinessPenaltyNewActualDate(TimelinessOpComp1 tocToChange,
            DateTime newActualDate)
        {
            tocToChange.ActualStartDate = newActualDate;
            tocToChange.FieldDays = (tocToChange.AreaCovered != 0) ? (tocToChange.Amount / tocToChange.AreaCovered) : 0;
            //the number they enter whether 10 or .01 is the probability
            double dbWorkdayProbablity = tocToChange.WorkdayProbability / 100;
            dbWorkdayProbablity = (dbWorkdayProbablity != 0) ? dbWorkdayProbablity : -1;
            tocToChange.ProbableFieldDays = tocToChange.FieldDays / dbWorkdayProbablity;
            tocToChange.ProbableFinishDate = tocToChange.ActualStartDate.AddDays(tocToChange.ProbableFieldDays);
            //penalty only if actual start date is later than planned
            TimeSpan tsPlannedvsActualPenaltyDays = tocToChange.PlannedStartDate - tocToChange.ActualStartDate;
            int iDays = tsPlannedvsActualPenaltyDays.Days;
            if (iDays < 0)
            {
                if (tocToChange.PlannedStartDate < tocToChange.ActualStartDate)
                {
                    iDays = iDays * -1;
                }
            }
            else if (iDays > 0)
            {
                if (tocToChange.PlannedStartDate > tocToChange.ActualStartDate)
                {
                    iDays = iDays * -1;
                }
            }

            double dbPenaltyDays = (iDays + tocToChange.ProbableFieldDays) - tocToChange.TimelinessPenaltyDaysFromStart1;
            double dbTotalOutputBenefit = tocToChange.Amount * tocToChange.OutputPrice * tocToChange.OutputYield * tocToChange.CompositionAmount * tocToChange.OutputTimes;
            tocToChange.TotalR = dbTotalOutputBenefit;
            if (dbPenaltyDays > 0)
            {
                //the number they enter whether 10 or .01 is the penalty
                double dbTimelinessPenalty = tocToChange.TimelinessPenalty1 / 100;
                //penalty runs for all days
                tocToChange.TimelinessPenaltyCost = dbTotalOutputBenefit * dbTimelinessPenalty * dbPenaltyDays;
                if (tocToChange.TimelinessPenaltyCost > dbTotalOutputBenefit)
                {
                    tocToChange.TimelinessPenaltyCost = dbTotalOutputBenefit;
                }
                //add additional penalty (i.e. 65 - (30 + 30))
                dbPenaltyDays = (iDays + tocToChange.ProbableFieldDays) - (tocToChange.TimelinessPenaltyDaysFromStart1 + tocToChange.TimelinessPenaltyDaysFromStart2);
                if (dbPenaltyDays > 0)
                {
                    //additional penalty
                    //i.e. (1) / 100 = .01 penalty
                    dbTimelinessPenalty = (tocToChange.TimelinessPenalty2) / 100;
                    //additional penalty runs only for additional days
                    tocToChange.TimelinessPenaltyCost = tocToChange.TimelinessPenaltyCost + (dbTotalOutputBenefit * dbTimelinessPenalty * dbPenaltyDays);
                    if (tocToChange.TimelinessPenaltyCost > dbTotalOutputBenefit)
                    {
                        tocToChange.TimelinessPenaltyCost = dbTotalOutputBenefit;
                    }
                }
            }
            else
            {
                tocToChange.TimelinessPenaltyCost = 0;
            }
            if (tocToChange.FieldCapacity == 0)
            {
                tocToChange.FieldCapacity = -1;
            }
            //1000 / (100 hours = .2 hrs/acre * 500 acres)
            double dbTotalHours = (1 / tocToChange.FieldCapacity) * tocToChange.Amount;
            if (dbTotalHours > 0)
            {
                tocToChange.TimelinessPenaltyCostPerHour = tocToChange.TimelinessPenaltyCost / dbTotalHours;
            }
            else
            {
                tocToChange.TimelinessPenaltyCostPerHour = 0;
            }
        }
    }
}
