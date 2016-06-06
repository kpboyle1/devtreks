using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions.StatsAnalyzers
{
    /// <summary>
    ///Purpose:		Optimization algorithm for selecting least cost combinations
    ///             of uniqueOpComps and feasibleOpComps. In general, optimizes power input 
    ///             (horsepower) and nonpower input (field capacity or speed, width, 
    ///             field efficiency) at least cost. 
    ///             
    ///            This is an alternative to the annealing calculators, because the  
    ///            combinations are generally less than 20^5. A planting operation 
    ///            is limited to a small number of feasible planting operations. 
    ///            That makes normal looping, rather than ai algorithms, feasible.
    ///            
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// Note:    this optimization is known as simulated annealing
    public class Mach1SelectSchedule1
    {
        public Mach1SelectSchedule1() { }
        public string ErrorMessage = string.Empty;
        private static Random random { get; set; }
        //least cost combinations of opcomps (object array = beststate array = [1][4][20][15][10][7])
        //the index corresponds to the InitialOpComps array so BestOpComps[3] is for InitialOpComps[3]
        //each feasible member of the list has new properties set during a loop
        List<TimelinessOpComp1> InitialOpComps = new List<TimelinessOpComp1>();
        List<TimelinessOpComp1> CurrentStateOpComps = new List<TimelinessOpComp1>();
        List<TimelinessOpComp1> BestOpComps = new List<TimelinessOpComp1>();
        //least cost collections of TP.OpComps (with each OpComp holding best OpComp.TimelinessOpComp)
        //calling procedures run the optimization and then use this as the result
        public List<TimelinessTimePeriod1> BestTimelinessTimePeriods = new List<TimelinessTimePeriod1>();

        public void RunOptimization(List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            //prepare the initial, ordered collections of opcomps
            InitCollections(timelinessTimePeriods);
            if (this.ErrorMessage == string.Empty)
            {
                try
                {
                    //state holds feasible opcomps for each uniqueopcomp 
                    //[1][4][20][15][10][7] = means the first unique opcomp
                    //uses the feasible opcomp with an index of 1 
                    //= this.InitialOpComps[0].FeasibleOpComps[1]
                    int u = 0;
                    int f = 0;
                    
                    //init this.CurrentStateOpComps total costs properties 
                    double totalCurrentStateCost = SetTotalCostsForCurrentState();
                    //init this.BestOpComps total costs properties 
                    SetBestState();
                    double totalBestStateCost = SetTotalCostsForBestState();
                    //all feasible combinations are looped through 
                    //each time a new feasible is checked, a new timeliness cost 
                    //is added to it when its date must be changed 
                    //best state is least cost for the current full array
                    u = 0;
                    bool bHasNewFixedUniqueOpComp = false;
                    foreach (var uniqueOpComp in this.InitialOpComps)
                    {
                        if (uniqueOpComp.TimelinessOpComps != null)
                        {
                            f = 0;
                            foreach (var feasibleOpComp in uniqueOpComp.TimelinessOpComps)
                            {
                                //add feasibleopcomp to currentopcomps
                                totalCurrentStateCost = SetTotalCostsForCurrentState(u, f);
                                totalBestStateCost = SetTotalCostsForBestState();
                                if (totalCurrentStateCost < totalBestStateCost && totalCurrentStateCost > 0)
                                {
                                    //new least cost combination
                                    //copy this.CurrentStateOpComps to this.BestOpComps
                                    SetBestState();
                                    bHasNewFixedUniqueOpComp = true;
                                }
                                if (bHasNewFixedUniqueOpComp)
                                {
                                    //every time the best state is changed
                                    //recheck all combos, keeping the new feasibleOpComp fixed 
                                    ChangeBestStateUsingFixedUniqueOpComp(u);
                                }
                                bHasNewFixedUniqueOpComp = false;
                                f++;
                            }
                        }
                        u++;
                    }
                    //set the solution collection
                    MakeBestMachineryCombos(timelinessTimePeriods);
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = string.Concat(Errors.GetMessage("RESOURCESTOCKS_NOMACH1SELECTSCHEDULE"),
                        ex.ToString());
                }
            }
        }
        private void ChangeBestStateUsingFixedUniqueOpComp(int u)
        {
            double totalCurrentStateCost = 0;
            double totalBestStateCost = 0;
            int u2 = 0;
            int f2 = 0;
            u2 = 0;
            //combine it with every other possible combination [0][0][0][0][0][1] ...
            //to see if it affects timeliness penalties
            foreach (var uniqueOpComp2 in this.InitialOpComps)
            {
                f2 = 0;
                foreach (var feasibleOpComp2 in uniqueOpComp2.TimelinessOpComps)
                {
                    //hold the uniqueopcomp fixed and examine everything else
                    if (u2 != u)
                    {
                        totalCurrentStateCost = SetTotalCostsForCurrentState(u2, f2);
                        totalBestStateCost = SetTotalCostsForBestState();
                        if (totalCurrentStateCost < totalBestStateCost && totalCurrentStateCost > 0)
                        {
                            //new least cost combination
                            //copy this.CurrentStateOpComps to this.BestOpComps
                            SetBestState();
                        }
                    }
                    f2++;
                }
                u2++;
            }
        }
        private TimelinessOpComp1 CopyTimelinessOpComp(TimelinessOpComp1 existingTimelinessOpComp)
        {
            TimelinessOpComp1 newTimelinessOpComp = null;
            if (existingTimelinessOpComp != null)
            {
                //the properties of newTOC can be safely changed
                newTimelinessOpComp = new TimelinessOpComp1(existingTimelinessOpComp);
                newTimelinessOpComp.CopyCalculatorProperties(existingTimelinessOpComp);
                newTimelinessOpComp.XmlDocElement = existingTimelinessOpComp.XmlDocElement;
                newTimelinessOpComp.CopyTotalCostsProperties(existingTimelinessOpComp);
                newTimelinessOpComp.CopyTimelinessOC1Properties(existingTimelinessOpComp);
                //these are filled dynamically (after ordering, optimization)
                newTimelinessOpComp.TimelinessOpComps = new List<TimelinessOpComp1>();
                //inputs shouldn't be changed, only the parent timelinessopcomp is changed
                newTimelinessOpComp.Inputs = new List<Input>();
                if (existingTimelinessOpComp.Inputs != null)
                {
                    foreach (var input in existingTimelinessOpComp.Inputs)
                    {
                        //this is byref, so don't change the inputs
                        newTimelinessOpComp.Inputs.Add(input);
                    }
                }
            }
            return newTimelinessOpComp;
        }
        private void InitCollections(List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            if (timelinessTimePeriods != null)
            {
                if (timelinessTimePeriods.Count > 0)
                {
                    //make the optimization array of uniqueopcomp, feasibleopcomp
                    SetOrderedTOCs(timelinessTimePeriods);
                    if (this.InitialOpComps == null)
                    {
                        this.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS_NOPOWERORNONPOWERINPUTS");
                    }
                }
                else
                {
                    this.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS_NOPOWERORNONPOWERINPUTS");
                }
            }
            else
            {
                this.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS_NOPOWERORNONPOWERINPUTS");
            }
        }
        private void SetOrderedTOCs(
            List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            List<TimelinessOpComp1> opCompsDateOrdered = new List<TimelinessOpComp1>();
            //make ascending date order collections
            IOrderedEnumerable<TimelinessTimePeriod1> ascendingTPs
                = timelinessTimePeriods.OrderBy(tp => tp.Date);
            //get all of the tocs available in all tps
            List<TimelinessOpComp1> allTOCS = new List<TimelinessOpComp1>();
            foreach (TimelinessTimePeriod1 tp in ascendingTPs)
            {
                foreach (TimelinessOpComp1 aoc in tp.TimelinessOpComps)
                {
                    allTOCS.Add(aoc);
                }
                
            }
            //order them by ascending startdate (this becomes the required order)
            IOrderedEnumerable<TimelinessOpComp1> ascendingOCs 
                = allTOCS.OrderBy(oc => oc.PlannedStartDate);
            foreach (TimelinessOpComp1 oc in ascendingOCs)
            {
                //new collections are needed because the members
                //will be changed anytime an actual date has to be changed
                TimelinessOpComp1 newUniqueTimelinessOpComp
                    = CopyTimelinessOpComp(oc);
                TimelinessOpComp1 newUnique2TimelinessOpComp
                    = CopyTimelinessOpComp(oc);
                //ascending order by finishdate (start date is the same)
                if (oc.TimelinessOpComps != null)
                {
                    IOrderedEnumerable<TimelinessOpComp1> ascendingFOCs
                        = oc.TimelinessOpComps.OrderBy(woc => woc.ProbableFinishDate);
                    int i = 0;
                    foreach (TimelinessOpComp1 foc in ascendingFOCs)
                    {
                        //add the feasible opcomp to the unique opcomp
                        oc.TimelinessOpComps.Add(foc);
                        if (i == 0)
                        {
                            //add it to the optimization collections
                            TimelinessOpComp1 newFeasibleTimelinessOpComp
                                = CopyTimelinessOpComp(foc);
                            newUniqueTimelinessOpComp.TimelinessOpComps.Add(newFeasibleTimelinessOpComp);
                            TimelinessOpComp1 newFeasible2TimelinessOpComp
                                = CopyTimelinessOpComp(foc);
                            newUnique2TimelinessOpComp.TimelinessOpComps.Add(newFeasible2TimelinessOpComp);
                        }
                        i++;
                    }
                }
                //add it to the unique collections
                this.InitialOpComps.Add(oc);
                this.BestOpComps.Add(newUniqueTimelinessOpComp);
                this.CurrentStateOpComps.Add(newUnique2TimelinessOpComp);
            }
        }
 
        private double SetTotalCostsForCurrentState(int uniqueOpCompIndex, int feasibleOpCompIndex)
        {
            double totalCost = 0;
            //reset current state to the current best state
            ReSetCurrentState();
            if (this.InitialOpComps[uniqueOpCompIndex] != null)
            {
                TimelinessOpComp1 newFeasibleTimelinessOpComp = CopyTimelinessOpComp(
                    this.InitialOpComps[uniqueOpCompIndex].TimelinessOpComps[feasibleOpCompIndex]);
                if (newFeasibleTimelinessOpComp != null)
                {
                    //clear the current feasible opcomp
                    this.CurrentStateOpComps[uniqueOpCompIndex].TimelinessOpComps.Clear();
                    //add new one
                    this.CurrentStateOpComps[uniqueOpCompIndex].TimelinessOpComps.Add(newFeasibleTimelinessOpComp);
                    totalCost = SetTotalCostsForCurrentState();
                }
            }
            return totalCost;
        }
        private void ReSetCurrentState()
        {
            this.CurrentStateOpComps.Clear();
            //current state always starts with beststate and then sees
            //if changing the current feasible opcomp lowers best state
            foreach (TimelinessOpComp1 oc in this.BestOpComps)
            {
                //new collections are needed because the members
                //will be changed anytime an actual date has to be changed
                TimelinessOpComp1 newUniqueTimelinessOpComp
                    = CopyTimelinessOpComp(oc);
                //ascending order by finishdate (start date is the same)
                if (oc.TimelinessOpComps != null)
                {
                    int i = 0;
                    foreach (TimelinessOpComp1 foc in oc.TimelinessOpComps)
                    {
                        if (i == 0)
                        {
                            TimelinessOpComp1 newFeasibleTimelinessOpComp
                                = CopyTimelinessOpComp(foc);
                            newUniqueTimelinessOpComp.TimelinessOpComps.Add(newFeasibleTimelinessOpComp);
                        }
                        i++;
                    }
                }
                //add it to the currentstate collections
                this.CurrentStateOpComps.Add(newUniqueTimelinessOpComp);
            }
        }
        
        
        private double SetTotalCostsForCurrentState()
        {
            double totalCost = 0;
            int i = 0;
            DateTime lastFinishDate = CalculatorHelpers.GetDateShortNow();
            foreach (var uniqueOpComp in this.CurrentStateOpComps)
            {
                if (uniqueOpComp.TimelinessOpComps != null)
                {
                    foreach (var feasibleOpComp in uniqueOpComp.TimelinessOpComps)
                    {
                        if (i == 0)
                        {
                            //no dates need to be adjusted for first uniqueopcom
                            totalCost = TotalMachineryCost(feasibleOpComp);
                        }
                        else
                        {
                            //if needed, reset timeliness penalty
                            if (lastFinishDate > feasibleOpComp.ActualStartDate)
                            {
                                TimelinessOpComp1.SetTimelinessPenaltyNewActualDate(
                                    feasibleOpComp, lastFinishDate);
                            }
                            else if (feasibleOpComp.PlannedStartDate != feasibleOpComp.ActualStartDate)
                            {
                                //see if the planned start date can be done earlier
                                if (feasibleOpComp.PlannedStartDate > lastFinishDate)
                                {
                                    TimelinessOpComp1.SetTimelinessPenaltyNewActualDate(
                                        feasibleOpComp, feasibleOpComp.PlannedStartDate);
                                }
                                else
                                {
                                    //switch to a date somewhere between existing actual 
                                    //and planned
                                    if (lastFinishDate > feasibleOpComp.PlannedStartDate
                                        && lastFinishDate < feasibleOpComp.ActualStartDate)
                                    {
                                        TimelinessOpComp1.SetTimelinessPenaltyNewActualDate(
                                            feasibleOpComp, lastFinishDate);
                                    }
                                }
                            }
                            totalCost += TotalMachineryCost(feasibleOpComp);
                        }
                        lastFinishDate = feasibleOpComp.ProbableFinishDate;
                    }
                    i++;
                }
            }
            return totalCost;
        }

        private double SetTotalCostsForBestState()
        {
            double totalCost = 0;
            //no adjustments are made in bestcomps (they were done in base.CurrentOpComps)
            foreach (var uniqueOpComp in this.BestOpComps)
            {
                if (uniqueOpComp.TimelinessOpComps != null)
                {
                    foreach (var feasibleOpComp in uniqueOpComp.TimelinessOpComps)
                    {
                        totalCost += TotalMachineryCost(feasibleOpComp);
                    }
                }
            }
            return totalCost;
        }
        private static double TotalMachineryCost(TimelinessOpComp1 feasibleTOC)
        {
           double totalCost = feasibleTOC.TotalOC + feasibleTOC.TotalAOH
                        + feasibleTOC.TimelinessPenaltyCost;
            return totalCost;
        }
        
        private void SetBestState()
        {
            this.BestOpComps.Clear();
            foreach (TimelinessOpComp1 oc in this.CurrentStateOpComps)
            {
                //new collections are needed because the members
                //will be changed anytime an actual date has to be changed
                TimelinessOpComp1 newUniqueTimelinessOpComp
                    = CopyTimelinessOpComp(oc);
                //ascending order by finishdate (start date is the same)
                if (oc.TimelinessOpComps != null)
                {
                    foreach (TimelinessOpComp1 foc in oc.TimelinessOpComps)
                    {
                        TimelinessOpComp1 newFeasibleTimelinessOpComp
                            = CopyTimelinessOpComp(foc);
                        newUniqueTimelinessOpComp.TimelinessOpComps.Add(newFeasibleTimelinessOpComp);
                    }
                }
                //add it to the best collections
                this.BestOpComps.Add(newUniqueTimelinessOpComp);
            }
        }

        private TimelinessOpComp1 GetLeastCostOpComp(TimelinessOpComp1 uniqueOpComp)
        {
            TimelinessOpComp1 leastCostTOC = null;
            //loop through collection, selecting lowest cost opcomps
            double dbBestCost = 0;
            double dbThisTotalCost = 0;
            int iBestId = 0;
            dbBestCost = 0;
            dbThisTotalCost = 0;
            //the collections have unique, random ids
            iBestId = 0;
            int i = 0;
            foreach (var feasibleOpComp in uniqueOpComp.TimelinessOpComps)
            {
                dbThisTotalCost = TotalMachineryCost(feasibleOpComp);
                if (i == 0)
                {
                    dbBestCost = dbThisTotalCost;
                    iBestId = feasibleOpComp.Id;
                }
                else
                {
                    //has to be less cost to change to a potentially later finish date
                    //zero means its not a feasible opcomp
                    if (dbThisTotalCost < dbBestCost && dbThisTotalCost >= 0)
                    {
                        dbBestCost = dbThisTotalCost;
                        iBestId = feasibleOpComp.Id;
                    }
                }
                i++;
            }
            //return the best opcomp
            leastCostTOC = RunNoCostLeastCostRules(uniqueOpComp, iBestId);
            if (leastCostTOC != null)
            {
                return leastCostTOC;
            }
            //else
            //{

            //    return leastCostTOC;
            //    //leastCostTOC = RunMoreCostLeastCostRules(uniqueOpComp, iBestId);
            //}
            //null means no easy solution, run annealizing optimization with dynamic prices
            return leastCostTOC;
        }
        private TimelinessOpComp1 RunNoCostLeastCostRules(TimelinessOpComp1 uniqueOpComp, int bestId)
        {
            TimelinessOpComp1 leastCostTOC = uniqueOpComp.TimelinessOpComps
                .FirstOrDefault(boc => boc.Id == bestId);
            if (leastCostTOC == null)
            {
                return leastCostTOC;
            }
            //Rule 1. If the best opcomp's StartDate is later than all of this.BestComps
            //finish dates, no overlap, it's the best, so return it
            bool bHasOverlap = this.BestOpComps
                .Any(ocb => ocb.ProbableFinishDate > leastCostTOC.PlannedStartDate);
            if (bHasOverlap == false)
            {
                return leastCostTOC;
            }
            //Rule 2. If all of the best opcomp's inputs are different than the overlapping 
            //opcomps, no overlap, it's the best, so return it
            bool bHasSameEquipment = HasSameEquipment(leastCostTOC);
            if (bHasSameEquipment == false)
            {
                return leastCostTOC;
            }
            return null;
        }
        private TimelinessOpComp1 RunMoreCostLeastCostRules(TimelinessOpComp1 uniqueOpComp, int bestId)
        {
            //these rules start changing start and finish dates, with potential penalties
            TimelinessOpComp1 leastCostTOC = uniqueOpComp.TimelinessOpComps
               .FirstOrDefault(boc => boc.Id == bestId);
            if (leastCostTOC == null)
            {
                return leastCostTOC;
            }
            //Rule 3. If the leastCostToc total costs, including the remaining BestCostCombos, 
            //don't increase when the actual date is moved ahead to overcome overlap, it's the best
            bool bCostIsMoreWithNewActualDate = CostIncreasesWithNewDate(leastCostTOC);
            if (bCostIsMoreWithNewActualDate == false)
            {
                //leastCostTOC has reset penalty and dates to new date
                return leastCostTOC;
            }
            //Rule 4. If all of the best opcomp's inputs are different than the overlapping 
            //opcomps, no overlap, it's the best, so return it


            return leastCostTOC;
        }
        private bool CostIncreasesWithNewDate(TimelinessOpComp1 leastCostTOC)
        {
            //Rule 3. If the leastCostToc total costs don't increase when 
            //the actual date is moved ahead to overcome overlap, it's the best
            bool bCostIsMoreWithNewActualDate = true;
            TimelinessOpComp1 dateOpComp = this.BestOpComps.Last(ocb => ocb.ProbableFinishDate
                   > leastCostTOC.PlannedStartDate);
            if (dateOpComp != null)
            {
                TimelinessOpComp1 newDateOpComp = new TimelinessOpComp1(leastCostTOC);
                bCostIsMoreWithNewActualDate = TimelinessOpComp1.TimelinessPenaltyIncreasesUsingNewDate(
                    newDateOpComp, dateOpComp.ProbableFinishDate);
            }
            if (bCostIsMoreWithNewActualDate == false)
            {
                //keep the existing least cost, but reset penalty and dates
                if (dateOpComp != null)
                {
                    TimelinessOpComp1.SetTimelinessPenaltyNewActualDate(leastCostTOC,
                        dateOpComp.ProbableFinishDate);
                }
            }
            return bCostIsMoreWithNewActualDate;
        }

        private bool HasSameEquipment(TimelinessOpComp1 leastCostTOC)
        {
            bool bHasSameEquipment = true;
            foreach (var boc in this.BestOpComps)
            {
                //these are mach1inputts that inherit from a base input
                if (boc.Inputs != null)
                {
                    if (leastCostTOC.Inputs != null)
                    {
                        List<Machinery1Input> sameBOCs = new List<Machinery1Input>();
                        foreach (var bocinput in boc.Inputs)
                        {
                            Machinery1Input machInput = (Machinery1Input)bocinput;
                            sameBOCs.Add(machInput);
                        }
                        foreach (var locinput in leastCostTOC.Inputs)
                        {
                            Machinery1Input machlocInput = (Machinery1Input)locinput;
                            bHasSameEquipment = BIMachinery2aStockCalculator.HasPowerInput(machlocInput, sameBOCs);
                            if (bHasSameEquipment == false)
                            {
                                bHasSameEquipment = BIMachinery2aStockCalculator.HasNonPowerInput(machlocInput, sameBOCs);
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return bHasSameEquipment;
        }
        private bool CostIsMoreWithNewActualDate(TimelinessOpComp1 leastCostTOC)
        {
            bool bCostIsMoreWithNewActualDate = true;
            foreach (var boc in this.BestOpComps)
            {
                //these are mach1inputs that inherit from a base input
                if (boc.Inputs != null)
                {
                    if (leastCostTOC.Inputs != null)
                    {
                        List<Machinery1Input> sameBOCs = new List<Machinery1Input>();
                        foreach (var bocinput in boc.Inputs)
                        {
                            Machinery1Input machInput = (Machinery1Input)bocinput;
                            sameBOCs.Add(machInput);
                        }
                        foreach (var locinput in leastCostTOC.Inputs)
                        {
                            Machinery1Input machlocInput = (Machinery1Input)locinput;
                            bCostIsMoreWithNewActualDate = BIMachinery2aStockCalculator.HasPowerInput(machlocInput, sameBOCs);
                            if (bCostIsMoreWithNewActualDate == false)
                            {
                                bCostIsMoreWithNewActualDate = BIMachinery2aStockCalculator.HasNonPowerInput(machlocInput, sameBOCs);
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return bCostIsMoreWithNewActualDate;
        }
        private void MakeBestMachineryCombos(List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            //build a stateful new collection of best ocs
            //refactor: they have to go with correct tp, so use a subroutine to add
            foreach (TimelinessTimePeriod1 tp in timelinessTimePeriods)
            {
                //add new tps to end of collection
                TimelinessTimePeriod1 newTP = GetNewTimePeriod(tp);
                //start a new list (must now use InitTimePeriods to match opcomp to timeperiod)
                newTP.TimelinessOpComps = new List<TimelinessOpComp1>();
                foreach (TimelinessOpComp1 bestoc in this.BestOpComps)
                {
                    //add it to the correct timeperiod 
                    if (tp.TimelinessOpComps.FirstOrDefault(toc => toc.Id == bestoc.Id) != null)
                    {
                        //add it to end of collection (each besttoc has one member of besttoc.tocs that is best)
                        newTP.TimelinessOpComps.Add(bestoc);
                    }
                }
                //add the best opcomps to the stateful besttp collection
                this.BestTimelinessTimePeriods.Add(newTP);
            }
        }
        private TimelinessTimePeriod1 GetNewTimePeriod(TimelinessTimePeriod1 tp)
        {
            TimelinessTimePeriod1 newTP = new TimelinessTimePeriod1(tp);
            newTP.CopyCalculatorProperties(tp);
            newTP.CopyTotalBenefitsProperties(tp);
            newTP.CopyTotalCostsProperties(tp);
            return newTP;
        }
    }
}
