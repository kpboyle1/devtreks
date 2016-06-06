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
    ///             draft/soil factor) at least cost. 
    ///             
    ///             Initial set up doesn't work. Has to restrict t to feasible opcomps, while
    ///             this algorithm ends up pairing a unique field cultivate with a best chisel plow.
    ///            
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// Note:    this optimization is known as simulated annealing
    public class AI02Annealing
    {
        public AI02Annealing() { }
        public string ErrorMessage = string.Empty;
        private static Random random { get; set; }
        //feasible opcomps (workers or field operations)
        List<TimelinessOpComp1> WOpComps = new List<TimelinessOpComp1>();
        //unique opcomps (unique tasks)
        List<TimelinessOpComp1> TOpComps = new List<TimelinessOpComp1>();
        //least cost collections of TP.OpComps (with each OpComp holding best OpComp.TimelinessOpComp)
        public List<TimelinessTimePeriod1> BestTimelinessTimePeriods = new List<TimelinessTimePeriod1>();

        public void RunOptimization(
            List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            InitCollections(timelinessTimePeriods);
            if (this.ErrorMessage == string.Empty)
            {
                try
                {
                    //initial collections of TP.OpComps (with each OpComp holding feasible OpComp.TimelinessOpComps)
                    random = new Random(0);
                    // total cost index for problem definition
                    double[][] problemData = MakeProblemData(timelinessTimePeriods);
                    //state represents a solution int array where the first index is the feasibleOpComp 
                    //and the second index is the uniqueOpComp
                    //(uniqueOpComp - feasibleOpComp combos that are not possible must have a zero total cost)
                    int[] state = RandomState(problemData);
                    double energy = Energy(state, problemData);
                    //best state is the least cost vector of 1 unique opcomp and 1 feasible opcomp
                    int[] bestState = state;
                    double bestEnergy = energy;
                    int[] adjState;
                    double adjEnergy;

                    int iteration = 0;
                    //the higher these are, the longer the algorithm runs 
                    //the longer the algo runs the more optimal the solution
                    int maxIteration = 1000000;
                    double currTemp = 10000.0;
                    // cooling rate
                    double alpha = 0.995;

                    while (iteration < maxIteration && currTemp > 0.0001)
                    {
                        //adjacent state doesn't allow nonfeasible selections (w,t = 0)
                        adjState = AdjacentState(state, problemData);
                        adjEnergy = Energy(adjState, problemData);

                        if (adjEnergy < bestEnergy)
                        {
                            bestState = adjState;
                            bestEnergy = adjEnergy;
                        }
                        // [0, 1.0)
                        double p = random.NextDouble();
                        if (AcceptanceProb(energy, adjEnergy, currTemp) > p)
                        {
                            state = adjState;
                            energy = adjEnergy;
                        }
                        // cool down; annealing schedule
                        currTemp = currTemp * alpha;
                        ++iteration;
                    }
                    //temperature has cooled to (almost) zero at final iteration 
                    MakeBestMachineryCombos(timelinessTimePeriods, bestState, problemData);
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = string.Concat(Errors.GetMessage("RESOURCESTOCKS_NOSIMULATEDANNEALING"),
                        ex.ToString());
                }
            }
        }
        private void InitCollections(List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            if (timelinessTimePeriods != null)
            {
                if (timelinessTimePeriods.Count > 0)
                {
                    //make the optimization array of uniqueopcomp, feasibleopcomp
                    MakeOrderedTOCs(timelinessTimePeriods);
                    if (this.TOpComps == null || this.WOpComps == null)
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
        private double[][] MakeProblemData(List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            //feasible opcomps are first member of array (w)
            double[][] result = new double[this.WOpComps.Count][];
            //could also declare as: type double[,] = 
            int w = 0;
            //unique opcomps are the second array member (t)
            for (w = 0; w < result.Length; ++w)
            {
                result[w] = new double[this.TOpComps.Count];
            }
            //the problem matrix
            w = 0;
            int t = 0;
            double dbTotalCost = 0;
            foreach (var feasibleOC in this.WOpComps)
            {
                //any feasible oc that is not a child of the unique oc has a cost of zero
                foreach (var uniqueOC in this.TOpComps)
                {
                    dbTotalCost = TotalMachineryCost(uniqueOC, feasibleOC);
                    result[w][t] = dbTotalCost;
                    t++;
                }
                t = 0;
                w++;
            }
            //example array (last member can't be chosen)
            //result[0][0] = 7.55; result[0][1] = 3.89; result[0][2] = 0;
            return result;
        }
        private TimelinessOpComp1[][] MakeDynamicCostData(List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            //feasible opcomps are first member of array (w)
            TimelinessOpComp1[][] result = new TimelinessOpComp1[this.WOpComps.Count][];
            //could also declare as: type double[,] = 
            int w = 0;
            //unique opcomps are the second array member (t)
            for (w = 0; w < result.Length; ++w)
            {
                result[w] = new TimelinessOpComp1[this.TOpComps.Count];
            }
            //the problem matrix
            w = 0;
            int t = 0;
            double dbTotalCost = 0;
            foreach (var feasibleOC in this.WOpComps)
            {
                //any feasible oc that is not a child of the unique oc has a cost of zero
                foreach (var uniqueOC in this.TOpComps)
                {
                    //dbTotalCost = TotalMachineryCost(uniqueOC, feasibleOC);
                    result[w][t] = feasibleOC;
                        //MakeDynamicCostMember(result[w][t],
                        //w, t, uniqueOC, feasibleOC);
                    t++;
                }
                t = 0;
                w++;
            }
            //example array (last member can't be chosen)
            //result[0][0] = 7.55; result[0][1] = 3.89; result[0][2] = 0;
            return result;
        }
        private TimelinessOpComp1 MakeDynamicCostMember(TimelinessOpComp1 previousArrayMember,
            int currentW, int currentT, TimelinessOpComp1 uniqueOC, TimelinessOpComp1 feasibleOC)
        {
            //adjust date by previous array member
            TimelinessOpComp1 adjustedArrayMember = null;
            //TimelinessOpComp1 previousArrayMember = null;
            return adjustedArrayMember;
        }
        private void MakeOrderedTOCs(
            List<TimelinessTimePeriod1> timelinessTimePeriods)
        {
            //make ascending date order collections
            IOrderedEnumerable<TimelinessTimePeriod1> ascendingTPs
                = timelinessTimePeriods.OrderBy(tp => tp.Date);
            List<TimelinessOpComp1> allTOCS = new List<TimelinessOpComp1>();
            foreach (TimelinessTimePeriod1 tp in ascendingTPs)
            {
                //ascending order by startdate
                IOrderedEnumerable<TimelinessOpComp1> ascendingOCs
                    = tp.TimelinessOpComps.OrderBy(oc => oc.PlannedStartDate);
                foreach (TimelinessOpComp1 oc in ascendingOCs)
                {
                    
                    //ascending order by startdate
                    IOrderedEnumerable<TimelinessOpComp1> ascendingFOCs
                        = oc.TimelinessOpComps.OrderBy(woc => woc.PlannedStartDate);
                    if (ascendingFOCs.Count() == 0)
                    {
                        //add it to the unique collection (will get zero total costs in problem data)
                        TOpComps.Add(oc);
                    }
                    else
                    {
                        //add it to the unique collection
                        TOpComps.Add(oc);
                        foreach (TimelinessOpComp1 foc in ascendingFOCs)
                        {
                            //add it to the feasible oc collection
                            //note that is only feasible for the parent unique opcomp
                            // i.e. planting.feasibleplantingcombos
                            WOpComps.Add(foc);
                        }
                    }
                }
            }
        }
        private static double TotalMachineryCost(TimelinessOpComp1 uniqueTOC, TimelinessOpComp1 feasibleTOC)
        {
            double totalCost = 0;
            //these are total costs (multiplied by input.ocamount or aohamount, inputtimes
            //and oc.amount) not cost per unit area totals
            foreach (var foc in uniqueTOC.TimelinessOpComps)
            {
                //note: foc.CalculatorId = parentTOC.Id and foc.Id is randomid
                if (feasibleTOC.Id == foc.Id)
                {
                    //this feasibletoc belongs to this uniqueToc
                    totalCost = feasibleTOC.TotalOC + feasibleTOC.TotalAOH
                        + feasibleTOC.TimelinessPenaltyCost;
                    break;
                }
                else
                {
                    //never allow a zero cost oc in best state
                    totalCost = 0;
                }
            }
            
            return totalCost;
        }

        private static int[] RandomState(double[][] problemData)
        {
            int feasibleOpComps = problemData.Length;
            int uniqueOpComps = problemData[0].Length;
            int[] state = new int[uniqueOpComps];
            int w = 0;
            int i = 0;
            for (int t = 0; t < uniqueOpComps; ++t)
            {
                //init break counter
                i = 0;
                // pick random powerInput
                w = random.Next(0, feasibleOpComps);
                // make sure powerInput can pull implement
                while (problemData[w][t] == 0.0)
                {
                    ++w;
                    if (w > feasibleOpComps - 1)
                    {
                        w = 0;
                        i++;
                        if (i > 1)
                        {
                            //no endless loops
                            break;
                        }
                    }
                }
                state[t] = w;
            }
            return state;
        }

        private int[] AdjacentState(int[] currState, double[][] problemData)
        {
            int feasibleOpComps = problemData.Length;
            int uniqueOpComps = problemData[0].Length;
            int[] state = new int[uniqueOpComps];
            // pick a random implement
            int uniqueOpComp = random.Next(0, uniqueOpComps);
            // might not be able to do implement
            int feasibleOpComp = random.Next(0, feasibleOpComps);
            int i = 0;
            // make sure feasibleOpComp doesn't overlap with uniqueOpComp
            while (HasOverlappingTimeOrMachinery(problemData, feasibleOpComp, uniqueOpComp))
            {
                ++feasibleOpComp;
                if (feasibleOpComp > feasibleOpComps - 1)
                {
                    feasibleOpComp = 0;
                    i++;
                    if (i > 1)
                    {
                        //no endless loops
                        break;
                    }
                }
            }
            currState.CopyTo(state, 0);
            // this could be the same as the original state
            state[uniqueOpComp] = feasibleOpComp;
            return state;
        }
        private bool HasOverlappingTimeOrMachinery(double[][] problemData,
            int feasibleIndex, int uniqueIndex)
        {
            bool bHasOverlap = false;
            //rule 1: zero costs means the two aren't feasible
            if (problemData[feasibleIndex][uniqueIndex] == 0)
            {
                return true;
            }
            //rule 2: if they are feasible, check to see if the dates overlap
            if (this.TOpComps[uniqueIndex].ProbableFinishDate >
                this.WOpComps[feasibleIndex].PlannedStartDate)
            {
                bHasOverlap = true;
            }
            else
            {
                bHasOverlap = false;
            }
            if (bHasOverlap == false)
            {
                //rule 3: dynamically adjust date and costs

                //Machinery1Input uniqueMachInput = (Machinery1Input)this.TOpComps[uniqueIndex];
                //bHasOverlap = BIMachinery2aStockCalculator.HasPowerInput(machlocInput, sameBOCs);
                //if (bHasOverlap == false)
                //{
                //    bHasOverlap = BIMachinery2aStockCalculator.HasNonPowerInput(machlocInput, sameBOCs);
                //}
                //else
                //{
                //    return true;
                //}
            }
            //rule 3: machinery
            return bHasOverlap;
        }
        private static double Energy(int[] state, double[][] problemData)
        {
            //energy is totalcost
            double result = 0.0;
            int feasibleOpComp = 0;
            double totalCost = 0;
            //t = nonpowerinputs
            for (int t = 0; t < state.Length; ++t)
            {
                feasibleOpComp = state[t];
                totalCost = problemData[feasibleOpComp][t];
                result += totalCost;
            }

            //// 3.50 hour penalty when a feasibleOpComp pulls more than one implement
            //int feasibleOpComps = problemData.Length;
            //int[] numJobs = new int[feasibleOpComps];
            //for (int t = 0; t < state.Length; ++t)
            //{
            //    feasibleOpComp = state[t];
            //    ++numJobs[feasibleOpComp];
            //    if (numJobs[feasibleOpComp] > 1) result += 3.50;
            //}
            return result;
        }
        
        private static double AcceptanceProb(double energy, double adjEnergy, double currTemp)
        {
            if (adjEnergy < energy)
            {
                return 1.0;
            }
            else
            {
                //exp((e − e') / T)
                return Math.Exp((energy - adjEnergy) / currTemp);
            }
        }

        private static void Display(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    Console.Write(matrix[i][j].ToString("F2") + " ");
                Console.WriteLine("");
            }
        }
        private void MakeBestMachineryCombos(List<TimelinessTimePeriod1> timelinessTimePeriods,
            int[] state, double[][] problemData)
        {
            List<TimelinessOpComp1> uniqueTimelinessOpComps = new List<TimelinessOpComp1>();
            //best unique opcomp
            for (int t = 0; t < state.Length; ++t)
            {
                //feasible opcomp
                int w = state[t];
                TimelinessOpComp1 newBestOC = this.WOpComps.ElementAtOrDefault(w);
                TimelinessOpComp1 newUniqueOC = this.TOpComps.ElementAtOrDefault(t);
                newUniqueOC.TimelinessOpComps = new List<TimelinessOpComp1>();
                //match to parent uniqueOC
                newUniqueOC.TimelinessOpComps.Add(newBestOC);
                uniqueTimelinessOpComps.Add(newUniqueOC);
            }
            //build a stateful new collection of best ocs
            //refactor: they have to go with correct tp, so use a subroutine to add
            foreach (TimelinessTimePeriod1 tp in timelinessTimePeriods)
            {
                //add them to end of collection
                this.BestTimelinessTimePeriods.Add(tp);
                //make sure this is a byref that changes internal collection
                TimelinessTimePeriod1 newTP = this.BestTimelinessTimePeriods
                    .ElementAtOrDefault(this.BestTimelinessTimePeriods.Count - 1);
                //start a new list
                newTP.TimelinessOpComps = new List<TimelinessOpComp1>();
                foreach (TimelinessOpComp1 bestoc in uniqueTimelinessOpComps)
                {
                    //add it to end of collection (each besttoc has one member of besttoc.tocs that is best)
                    newTP.TimelinessOpComps.Add(bestoc);
                }
            }
        }
        //private void MakeBestMachineryCombos(List<TimelinessTimePeriod1> timelinessTimePeriods, 
        //    int[] state, double[][] problemData)
        //{
        //    //feasible opcomp
        //    for (int t = 0; t < state.Length; ++t)
        //    {
        //        //unique opcomp
        //        //int w = state[t];
        //        //int w2 = 0;
        //        int t2 = 0;
        //        //build a brand new collection, because the existing collection
        //        //must be kept intact in case there is a next time period to optimize and synchronize
        //        foreach (TimelinessTimePeriod1 tp in timelinessTimePeriods)
        //        {
        //            //add it to end of collection
        //            this.BestTimelinessTimePeriods.Add(tp);
        //            //make sure this is a byref that changes internal collection
        //            TimelinessTimePeriod1 newTP = this.BestTimelinessTimePeriods
        //                .ElementAtOrDefault(this.BestTimelinessTimePeriods.Count - 1);
        //            //start a new list
        //            newTP.TimelinessOpComps = new List<TimelinessOpComp1>();
        //            //keep this container collection but change its children collection
        //            foreach (TimelinessOpComp1 oc in tp.TimelinessOpComps)
        //            {
        //                //add it to end of collection
        //                newTP.TimelinessOpComps.Add(oc);
        //                //make sure this is a byref that changes internal collection
        //                TimelinessOpComp1 newOC = newTP.TimelinessOpComps
        //                    .ElementAtOrDefault(newTP.TimelinessOpComps.Count - 1);
        //                //start a new list
        //                newOC.TimelinessOpComps = new List<TimelinessOpComp1>();
        //                foreach (TimelinessOpComp1 foc in oc.TimelinessOpComps)
        //                {
        //                    if (t2 == t)
        //                    {
        //                        //add the best machinery combo
        //                        newOC.TimelinessOpComps.Add(foc);
        //                        break;
        //                    }
        //                    t2++;
        //                }
        //                t2 = 0;
        //            }

        //            //keep all of the tp properties except for the collection of:
        //            //tp.TimelinessOpComps = new List<TimelinessOpComp1>();
        //            //this.BestTimelinessTimePeriods.Add(tp);

        //            ////keep this container collection but change its children collection
        //            //foreach (TimelinessOpComp1 oc in tp.TimelinessOpComps)
        //            //{
        //            //    //keep all of the oc properties except for the collection of:
        //            //    oc.tim = new List<TimelinessOpComp1>();
        //            //    TimelinessOpComp1 bestOpComp = new TimelinessOpComp1();
        //            //    bestOpComp.IimelinessOpComps = new List<TimelinessOpComp1>();
        //            //    foreach (TimelinessOpComp1 foc in oc.IimelinessOpComps)
        //            //    {
        //            //        if (t2 == t)
        //            //        {
        //            //            bestOpComp = new TimelinessOpComp1(foc);
        //            //            break;
        //            //        }
        //            //        t2++;
        //            //    }
        //            //    t2 = 0;
        //            //    if (w2 == w)
        //            //    {
        //            //        tp.TimelinessOpComps.Add(bestOpComp);
        //            //        break;
        //            //    }
        //            //    w2++;
        //            //}
        //            ////add the new tp with its best machinery combos
        //            //this.BestTimelinessTimePeriods.Add(tp);
        //        }
        //        //cost = problemData[w][t].ToString("F2") 
        //        //add the corresponding nonpower inputs and power inputs
        //        //note that the unique key has to be the feasibleOpComp
        //        //Machinery1Input np = this.NonPowerInputs.ElementAtOrDefault(t);
        //        //Machinery1Input p = this.PowerInputs.ElementAtOrDefault(w);
        //        //if (!this.BestMachineryCombos.ContainsKey(this.NonPowerInputs.ElementAtOrDefault(t)))
        //        //{
        //        //    this.BestMachineryCombos.Add(this.NonPowerInputs.ElementAtOrDefault(t), this.PowerInputs.ElementAtOrDefault(w));
        //        //}

        //    }
        //}
        private static void Display(int[] vector)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            Console.WriteLine("");
        }
    }
}
