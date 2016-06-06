using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Errors = DevTreks.Exceptions.DevTreksErrors;

namespace DevTreks.Extensions.StatsAnalyzers
{
    /// <summary>
    ///Purpose:		Optimization algorithm for selecting least cost combinations
    ///             of power inputs and implements. In general, optimizes power input 
    ///             (horsepower) and nonpower input (field capacity or speed, width, 
    ///             draft/soil factor) at least cost. In general, imposes two constraints: 
    ///             minimum horsepower and minimum field capacity.
    ///Author:		www.devtreks.org
    ///Date:		2014, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// Note:    this optimization is known as simulated annealing
    public class AI01Annealing
    {
        public AI01Annealing() { }
        public Dictionary<Machinery1Input, Machinery1Input> BestMachineryCombos { get; set; }
        public string ErrorMessage = string.Empty;
        private static Random random { get; set; }
        private List<Machinery1Input> PowerInputs = new List<Machinery1Input>();
        private List<Machinery1Input> NonPowerInputs = new List<Machinery1Input>();

        public void RunOptimization(
            TimelinessOpComp1 initialTimelinessOCStock, List<Machinery1Input> machineryInputs)
        {
            BestMachineryCombos = new Dictionary<Machinery1Input, Machinery1Input>();
            try
            {
                random = new Random(0);
                this.ErrorMessage = string.Empty;

                this.PowerInputs = machineryInputs.FindAll(m => m.FuelCost > 0);
                this.NonPowerInputs = machineryInputs.FindAll(m => m.FuelCost <= 0);
                if (this.PowerInputs.Count > 0 && this.NonPowerInputs.Count > 0)
                {
                    // total cost index for problem definition
                    double[][] problemData = MakeProblemData(initialTimelinessOCStock);
                    //state represents a solution int array where the index is the nonpowerimplement and the value is the powerimplement
                    //(powerinput - implement combos that are not possible must have a zero total cost)
                    int[] state = RandomState(problemData);
                    double energy = Energy(state, problemData);
                    //best state is the least cost vector of 1 power input and 1 non power inputs
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
                    MakeBestMachineryCombos(bestState, problemData);
                }
                else
                {
                    this.ErrorMessage = Errors.GetMessage("RESOURCESTOCKS_NOPOWERORNONPOWERINPUTS");
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = string.Concat(Errors.GetMessage("RESOURCESTOCKS_NOSIMULATEDANNEALING"), 
                    ex.ToString());
            }
        }

        private double[][] MakeProblemData(TimelinessOpComp1 initialTimelinessOCStock)
        {
            // the problem matrix
            //powerInputs are the first member of array (w)
            double[][] result = new double[this.PowerInputs.Count][];
            //could also declare as: type double[,] = 

            //nonpowerinputs are the second array member (t)
            for (int w = 0; w < result.Length; ++w)
            {
                result[w] = new double[this.NonPowerInputs.Count];
            }
            //add the total cost value for each array index 
            int i = 0;
            int j = 0;
            double dbTotalCost = 0;
            foreach (var powerinput in this.PowerInputs)
            {
                foreach (var nonpowerinput in this.NonPowerInputs)
                {
                    dbTotalCost = TotalMachineryCost(initialTimelinessOCStock, powerinput, nonpowerinput);
                    result[i][j] = dbTotalCost;
                    j++;
                }
                j = 0;
                i++;
            }
            //example array
            //result[0][0] = 7.55; result[0][1] = 3.89; result[0][2] = 22.00;
            return result;
        }
        private static double TotalMachineryCost(TimelinessOpComp1 initialTimelinessOCStock, 
            Machinery1Input powerinput, Machinery1Input nonpowerinput)
        {
            double totalCost = 0;
            if (nonpowerinput.Constants.HPPTOMax > powerinput.Constants.HPPTOMax)
            {
                //powerinput - implement combos that are not possible must have a zero total cost
                //zero total cost tells best state to keep looking for a different combo
                totalCost = 0;
            }
            else
            {
                //stay consistent with other object structures used
                List<Machinery1Input> machInputs = new List<Machinery1Input>();
                machInputs.Add(powerinput);
                machInputs.Add(nonpowerinput);
                //total costs are operating costs plus allocated overhead plus timeliness
                //on a per acre or per hectare basis
                //need to run oc costs so that the power input and non power have same field capacity
                totalCost = OpCompCostPerUnitArea(machInputs)
                    + TimelinessCostPerUnitArea(initialTimelinessOCStock, machInputs);
            }
            return totalCost;
        }
        private static double OpCompCostPerUnitArea(List<Machinery1Input> machInputs)
        {
            //adjust the machinery so that implement and power input are in synch
            //for this op or comp
            double ocandAOHCost = 0;
            OCCalculator ocCalculator = new OCCalculator();
            ocCalculator.SetJointInputCalculations(machInputs);
            foreach (Machinery1Input machinput in machInputs)
            {
                ocandAOHCost += (machinput.OCAmount * machinput.OCPrice)
                    + (machinput.AOHAmount * machinput.AOHPrice);
            }
            return ocandAOHCost;
        }
        private static double TimelinessCostPerUnitArea(TimelinessOpComp1 initialTimelinessOCStock,
            List<Machinery1Input> machInputs)
        {
            //run new TimelinessCost penalty calculations using this combination of machinery
            double timelinessCostPerUnitArea = 0;
            double powerInputOCAmount = machInputs.FirstOrDefault().OCAmount;
            if (powerInputOCAmount != initialTimelinessOCStock.FieldCapacity)
            {
                TimelinessOpComp1 timelinessCalculator
                    = new TimelinessOpComp1(initialTimelinessOCStock);
                //date only changes when npvcalcor is run
                timelinessCalculator.AddCalculationsProperties(powerInputOCAmount, initialTimelinessOCStock.PlannedStartDate);
                timelinessCostPerUnitArea = powerInputOCAmount * timelinessCalculator.TimelinessPenaltyCostPerHour;
            }
            else
            {
                timelinessCostPerUnitArea = powerInputOCAmount * initialTimelinessOCStock.TimelinessPenaltyCostPerHour;
            }
            return timelinessCostPerUnitArea;
        }
        private static int[] RandomState(double[][] problemData)
        {
            int powerInputs = problemData.Length;
            int nonPowerInputs = problemData[0].Length;
            int[] state = new int[nonPowerInputs];
            int w = 0;
            for (int t = 0; t < nonPowerInputs; ++t)
            {
                // pick random powerInput
                w = random.Next(0, powerInputs);
                // make sure powerInput can pull implement
                while (problemData[w][t] == 0.0) 
                {
                    ++w;
                    if (w > powerInputs - 1) w = 0;
                }
                state[t] = w;
            }
            return state;
        }

        private static int[] AdjacentState(int[] currState, double[][] problemData)
        {
            int powerInputs = problemData.Length;
            int nonPowerInputs = problemData[0].Length;
            int[] state = new int[nonPowerInputs];
            // pick a random implement
            int implement = random.Next(0, nonPowerInputs);
            // might not be able to do implement
            int powerInput = random.Next(0, powerInputs);
            // make sure powerInput can do implement
            while (problemData[powerInput][implement] == 0.0) 
            {
                ++powerInput;
                if (powerInput > powerInputs - 1) powerInput = 0;
            }
            currState.CopyTo(state, 0);
            // this could be the same as the original state
            state[implement] = powerInput; 
            return state;
        }

        private static double Energy(int[] state, double[][] problemData)
        {
            //energy is totalcost
            double result = 0.0;
            int powerInput = 0;
            double totalCost = 0;
            //t = nonpowerinputs
            for (int t = 0; t < state.Length; ++t)
            {
                powerInput = state[t];
                totalCost = problemData[powerInput][t];
                result += totalCost;
            }

            //// 3.50 hour penalty when a powerInput pulls more than one implement
            //int powerInputs = problemData.Length;
            //int[] numJobs = new int[powerInputs];
            //for (int t = 0; t < state.Length; ++t)
            //{
            //    powerInput = state[t];
            //    ++numJobs[powerInput];
            //    if (numJobs[powerInput] > 1) result += 3.50;
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
        private void MakeBestMachineryCombos(int[] state, double[][] problemData)
        {
            //implement
            for (int t = 0; t < state.Length; ++t)
            {
                //powerInput
                int w = state[t];
                //cost = problemData[w][t].ToString("F2") 
                //add the corresponding nonpower inputs and power inputs
                //note that the unique key has to be the implement
                Machinery1Input np = this.NonPowerInputs.ElementAtOrDefault(t);
                Machinery1Input p = this.PowerInputs.ElementAtOrDefault(w);
                if (!this.BestMachineryCombos.ContainsKey(this.NonPowerInputs.ElementAtOrDefault(t)))
                {
                    this.BestMachineryCombos.Add(this.NonPowerInputs.ElementAtOrDefault(t), this.PowerInputs.ElementAtOrDefault(w));
                }

            }
        }
        private static void Display(int[] vector)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            Console.WriteLine("");
        }
    }
}
