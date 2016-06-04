using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Simple optimization algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA example 5
    ///</summary>
    public class SimulatedAnnealing1 : Calculator1
    {
        public SimulatedAnnealing1(double currTemp, 
            double alpha, double penalty, int maxIteration, CalculatorParameters calcParams)
            : base()
        {
            _currtemp = currTemp;
            _alpha = alpha;
            _penalty = penalty;
            _maxiteration = maxIteration;
            _params = calcParams;
        }
        private CalculatorParameters _params { get; set; }
        //object initiation
        private double _currtemp { get; set; }
        private double _alpha { get; set; }
        private double _penalty { get; set; }
        private int _maxiteration { get; set; }
        //output
        public double BestEnergy { get; set; }
        
        //params
        private static Random random;

        /// <summary>
        /// Run the simulated annealing algo
        /// </summary>
        /// <param name="problemData">matrix of energy requirements (hour to complete task)</param>
        /// <param name="currTemp">initial temperature from which cooldown starts</param>
        /// <param name="alpha">cooling rate</param>
        /// <param name="penalty">a penalty when a worker has more than 1 task</param>
        /// <param name="maxIteration">number of iterations</param>
        public async Task<bool> RunAlgorithmAsync(List<List<double>> data)
        {
            bool bHasCalculation = false;
            try
            {
                random = new Random(0);
                //this algorith uses standard arrays
                double[,] problemData = Shared.GetDoubleArray(data);
                int[] state = RandomState(problemData);
                double energy = Energy(state, problemData, _penalty);
                int[] bestState = state;
                double bestEnergy = energy;
                int[] adjState;
                double adjEnergy;

                int iteration = 0;
                while (iteration < _maxiteration && _currtemp > 0.0001)
                {
                    adjState = AdjacentState(state, problemData);
                    adjEnergy = Energy(adjState, problemData, _penalty);

                    if (adjEnergy < bestEnergy)
                    {
                        bestState = adjState;
                        bestEnergy = adjEnergy;
                    }

                    double p = random.NextDouble(); // [0, 1.0)
                    if (AcceptanceProb(energy, adjEnergy, _currtemp) > p)
                    {
                        state = adjState;
                        energy = adjEnergy;
                    }

                    _currtemp = _currtemp * _alpha; // cool down; annealing schedule
                    ++iteration;
                } // while
                this.BestEnergy = bestEnergy;
                if (this.MathResult.ToLower().StartsWith("http"))
                {
                    string sError = string.Empty;
                    bool bHasSaved = CalculatorHelpers.SaveTextInURI(_params.ExtensionDocToCalcURI,
                        Interpret(bestState, problemData), this.MathResult, out sError);
                    if (!string.IsNullOrEmpty(sError))
                    {
                        this.MathResult += sError;
                    }
                }
                else
                {
                    this.MathResult = Interpret(bestState, problemData);
                }
                bHasCalculation = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return bHasCalculation;
        }

        private static int[] RandomState(double[,] problemData)
        {
            //random workers who can complete task; size of int equals number of tasks
            int numWorkers = problemData.GetUpperBound(0) + 1;
            int numTasks = problemData.GetUpperBound(1) + 1;
            int[] state = new int[numTasks];
            for (int t = 0; t < numTasks; ++t)
            {
                int w = random.Next(0, numWorkers); // pick random worker
                int iLoop = 0;
                while (problemData[w, t] == 0.0) // make sure worker can do task
                {
                    //try the next worker
                    ++w;
                    //don't exceed the number of workers
                    if (w > numWorkers - 1) w = 0;
                    //don't allow vectors with all zeros to run all day
                    if (iLoop > numWorkers) break;
                    iLoop++;
                }
                state[t] = w;
            }
            return state;
        }

        private static int[] AdjacentState(int[] currState, double[,] problemData)
        {
            int numWorkers = problemData.GetUpperBound(0) + 1;
            int numTasks = problemData.GetUpperBound(1) + 1;
            int[] state = new int[numTasks];

            int task = random.Next(0, numTasks); // pick a random task
            int iLoop = 0;
            int worker = random.Next(0, numWorkers); // might not be able to do task
            while (problemData[worker, task] == 0.0) // make sure worker can do task
            {
                ++worker;
                if (worker > numWorkers - 1) worker = 0;
                //don't allow vectors with all zeros to run all day
                if (iLoop > numWorkers) break;
                iLoop++;
            }
            currState.CopyTo(state, 0);
            state[task] = worker; // this could be the same as the original state
            return state;
        }

        private static double Energy(int[] state, double[,] problemData, double penalty)
        {
            // enery is cost
            double result = 0.0;
            for (int t = 0; t < state.Length; ++t)
            {
                int worker = state[t];
                double time = problemData[worker,t];
                result += time;
            }

            // 3.50 hour penalty when a worker does more than one task
            int numWorkers = problemData.Length;
            int[] numJobs = new int[numWorkers];
            for (int t = 0; t < state.Length; ++t)
            {
                int worker = state[t];
                ++numJobs[worker];
                if (numJobs[worker] > 1) result += penalty;
            }
            return result;
        }

        private static double AcceptanceProb(double energy, double adjEnergy, double currTemp)
        {
            if (adjEnergy < energy)
                return 1.0;
            else
                return Math.Exp((energy - adjEnergy) / currTemp); //exp((e − e') / T)
        }

        private static string Interpret(int[] state, double[,] problemData)
        {
            StringBuilder sb = new StringBuilder();
            for (int t = 0; t < state.Length; ++t) // task
            {
                int w = state[t]; // worker
                sb.AppendLine("Row [" + t + "] assigned to Column " + w + ", " + problemData[w, t].ToString("F2") + " total energy");
            }
            return sb.ToString();
        }
    }
}


