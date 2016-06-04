using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Neural network classification algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA example 7
    ///</summary>
    public class NeuralNetwork1 : Calculator1
    {
        public NeuralNetwork1(List<double> qs, int maxIteration, CalculatorParameters calcParams)
            : base()
        {
            if (qs == null)
                qs = new List<double>();
            _qs = qs;
            _params = calcParams;
        }
        private CalculatorParameters _params { get; set; }
        //qs in indicators being calculated
        private List<double> _qs { get; set; }
        //limit on number of input vars
        private int maxInputs = 9;
        //limit on number of output values
        private int maxOutputs = 10;

        private int rows = 0;
        //the last col is output 
        private int cols = 0;
        private double[] outs = new double[] { };
        private int inputVarCount = 0;
        private int outputValCount = 0;
        //neural network dimensions
        //first dimension = number of inputs
        private int nnI { get; set; }
        //hidden = inputs + 1
        private int nnH { get; set; }
        //outputs = Distinct(outputvector) i.e. 3 distinct colors possible
        private int nn0 { get; set; }
        private static Random rnd = null;
        //output
        public double QTPredicted { get; set; }
        public double QTL { get; set; }
        public string QTLUnit { get; set; }

        public async Task<bool> RunAlgorithmAsync(List<List<double>> data)
        {
            bool bHasCalculation = false;
            try
            {
                //add the qs to end of list for running in algo
                data.Add(_qs);
                //this algorith uses standard arrays
                double[,] problemData = Shared.GetDoubleArray(data);
                StringBuilder sb = new StringBuilder();
                //set the rank params needed for the needed matrixes
                rows = problemData.GetUpperBound(0) + 1;
                cols = problemData.GetUpperBound(1) + 1;
                outs = GetDistinctOutputValues(problemData, cols);
                inputVarCount = cols - 1;
                if (inputVarCount > maxInputs)
                {
                    //no: need to catch it before the data is sent here
                }
                outputValCount = outs.Count();

                rnd = new Random(159); // 159 makes 'good' output

                double[][] trainMatrix = null;
                double[][] testMatrix = null;
                //Generating train and test matrices using an 80%-20% split
                MakeTrainAndTest(problemData, out trainMatrix, out testMatrix);

                sb.AppendLine("\nFirst few rows of training matrix, using an 80%-20% split, are:");
                //First few rows of training matrix are
                Helpers.ShowMatrix(sb, trainMatrix, inputVarCount, 5);

                //the first and last params are legit; not sure of the correct basis for 2nd
                NeuralNetwork nn = new NeuralNetwork(inputVarCount, inputVarCount + 1, outputValCount);

                //Training to find best neural network weights using PSO with cross entropy error
                double[] bestWeights = nn.Train(sb, trainMatrix);
                sb.AppendLine("\nBest weights found:");
                Helpers.ShowVector(sb, bestWeights, 2, true);


                //Loading best weights into neural network
                nn.SetWeights(bestWeights);

                sb.AppendLine("\nExamples of the neural network accuracy:\n");
                //Analyzing the neural network accuracy on the test data
                double accuracy = nn.Test(sb, testMatrix);
                this.QTL = accuracy;
                this.QTLUnit = "percent accuracy";
                sb.AppendLine("Prediction accuracy = " + accuracy.ToString("F4"));
                this.QTPredicted = nn.QTPredicted;
                sb.AppendLine("Predicted QT = " + this.QTPredicted.ToString("F4"));
                if (this.MathResult.ToLower().StartsWith("http"))
                {
                    string sError = string.Empty;
                    bool bHasSaved = CalculatorHelpers.SaveTextInURI(
                        _params.ExtensionDocToCalcURI, sb.ToString(), this.MathResult, out sError);
                    if (!string.IsNullOrEmpty(sError))
                    {
                        this.MathResult += sError;
                    }
                }
                else
                {
                    this.MathResult = sb.ToString();
                }
                bHasCalculation = true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return bHasCalculation;
        }

        private void MakeTrainAndTest(double[,] problemData,
            out double[][] trainMatrix, out double[][] testMatrix)
        {
            int numTrain = (int)(0.80 * rows);
            int numTest = rows - numTrain;

            double[][] allData = new double[rows][];  
            for (int i = 0; i < allData.Length; ++i)
            {
                //CTA example uses 4 inputvars and 3 outputvals: (x0, x1, x2, x3), (y0, y1, y2)
                allData[i] = new double[inputVarCount + outputValCount];               
            }
            //get min and max input value for normalizing data
            double max = -1000000;
            double min = 1000000;
            ////skip the first row of zeros
            //for (int row = 1; row < rows; ++row)
            for (int row = 0; row < rows; ++row)
            {
                //inputs (cols - 1)
                for (int col = 0; col < (cols - 1); ++col)
                {
                    //determine max input value
                    if (problemData[row, col] > max)
                    {
                        max = problemData[row, col];
                    }
                    //determine min input value
                    if (problemData[row, col] < min)
                    {
                        min = problemData[row, col];
                    }
                }
            }
            double slope = 2 / (max - min);
            double intercept = 1 - (slope * max);
            for (int row = 0; row < rows; ++row)
            {
                //inputs (cols - 1)
                for (int col = 0; col < (cols - 1); ++col)
                {
                    allData[row][col] = problemData[row, col];
                }
                
                //normalize data to values between -1 and 1
                
                //inputs (cols - 1)
                for (int i = 0; i < (cols - 1); ++i)
                {
                    // scale input data to [-1.0, +1.0]
                    allData[row][i] = (slope * allData[row][i]) + intercept;  
                }
                //outputs
                int iOutIndex = 0;
                for (int i = 0; i < outs.Count(); i++)
                {
                    if (problemData[row, cols - 1] == outs[i])
                    {
                        //only one member of output vector can be one {1, 0, 0}
                        iOutIndex = i;
                        break;
                    }
                }
                for (int j = inputVarCount; j < (inputVarCount + outputValCount); j++)
                {
                    if ((j - inputVarCount)  == iOutIndex)
                    {
                        allData[row][j] = 1.0;
                    }
                    else
                    {
                        //all the rest must be zero
                        allData[row][j] = 0.0;
                    }
                }
            }

            Helpers.ShuffleRows(allData);

            trainMatrix = Helpers.MakeMatrix(numTrain, (inputVarCount + outputValCount));
            testMatrix = Helpers.MakeMatrix(numTest, (inputVarCount + outputValCount));
            for (int i = 0; i < numTrain; ++i)
            {
                allData[i].CopyTo(trainMatrix[i], 0);
            }
            for (int i = 0; i < numTest; ++i)
            {
                //adds remaining 20 out of 100 (starts
                allData[i + numTrain].CopyTo(testMatrix[i], 0);
            }
        } // MakeTrainAndTest
        //get a vector of possible output values 
        private double[] GetDistinctOutputValues(double[,] problemData, int cols)
        {
            List<double> lstOuts = new List<double>();
            double output = 0;
            for (int i = 0; i <= problemData.GetUpperBound(0); i++)
            {
                //the first col is the output variable
                output = problemData[i, 0];
                //if last col
                //output = problemData[i, cols - 1];
                if (!lstOuts.Contains(output)
                    && lstOuts.Count() <= maxOutputs)
                {
                    lstOuts.Add(output);
                }
            }
            //sort them
            lstOuts.Sort();
            return lstOuts.ToArray();
        }
        // --------------------------------------------------------------------------------------------

    } // class NeuralClassificationProgram

    // ==============================================================================================

    class NeuralNetwork
    {
        private int numInput;
        private int numHidden;
        private int numOutput;

        private double[] inputs;
        private double[][] ihWeights; // input-to-hidden
        private double[] ihSums;
        private double[] ihBiases;
        private double[] ihOutputs;
        private double[][] hoWeights;  // hidden-to-output
        private double[] hoSums;
        private double[] hoBiases;
        private double[] outputs;
        //passed back to original object to fill in same prop
        public double QTPredicted { get; set; }
        static Random rnd = null;

        public NeuralNetwork(int numInput, int numHidden, int numOutput)
        {
            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            inputs = new double[numInput];
            ihWeights = Helpers.MakeMatrix(numInput, numHidden);
            ihSums = new double[numHidden];
            ihBiases = new double[numHidden];
            ihOutputs = new double[numHidden];
            hoWeights = Helpers.MakeMatrix(numHidden, numOutput);
            hoSums = new double[numOutput];
            hoBiases = new double[numOutput];
            outputs = new double[numOutput];

            rnd = new Random(0);
        }

        public void SetWeights(double[] weights)
        {
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("The weights array length: " + weights.Length + " does not match the total number of weights and biases: " + numWeights);

            int k = 0; // points into weights param

            for (int i = 0; i < numInput; ++i)
                for (int j = 0; j < numHidden; ++j)
                    ihWeights[i][j] = weights[k++];

            for (int i = 0; i < numHidden; ++i)
                ihBiases[i] = weights[k++];

            for (int i = 0; i < numHidden; ++i)
                for (int j = 0; j < numOutput; ++j)
                    hoWeights[i][j] = weights[k++];

            for (int i = 0; i < numOutput; ++i)
                hoBiases[i] = weights[k++];
        }

        public double[] ComputeOutputs(double[] currInputs)
        {
            if (inputs.Length != numInput)
                throw new Exception("Inputs array length " + inputs.Length + " does not match NN numInput value " + numInput);

            for (int i = 0; i < numHidden; ++i)
                this.ihSums[i] = 0.0;
            //for (int i = 0; i < numHidden; ++i)
            //  this.ihOutputs[i] = 0.0;
            for (int i = 0; i < numOutput; ++i)
                this.hoSums[i] = 0.0;

            for (int i = 0; i < currInputs.Length; ++i) // copy
                this.inputs[i] = currInputs[i];

            for (int j = 0; j < numHidden; ++j)  // compute input-to-hidden sums
                for (int i = 0; i < numInput; ++i)
                    ihSums[j] += this.inputs[i] * ihWeights[i][j];

            for (int i = 0; i < numHidden; ++i)  // add biases to input-to-hidden sums
                ihSums[i] += ihBiases[i];

            for (int i = 0; i < numHidden; ++i)   // determine input-to-hidden output
                ihOutputs[i] = SigmoidFunction(ihSums[i]);


            for (int j = 0; j < numOutput; ++j)   // compute hidden-to-output sums
                for (int i = 0; i < numHidden; ++i)
                    hoSums[j] += ihOutputs[i] * hoWeights[i][j];
            for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
                hoSums[i] += hoBiases[i];

            double[] result = Softmax(hoSums);

            result.CopyTo(this.outputs, 0);
            return result;
        } // ComputeOutputs


        private static double SigmoidFunction(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            else return 1.0 / (1.0 + Math.Exp(-x));
        }

        private static double[] Softmax(double[] hoSums)
        {
            // determine max
            double max = hoSums[0];
            for (int i = 0; i < hoSums.Length; ++i)
                if (hoSums[i] > max) max = hoSums[i];

            // determine scaling factor (sum of exp(eachval - max)
            double scale = 0.0;
            for (int i = 0; i < hoSums.Length; ++i)
                scale += Math.Exp(hoSums[i] - max);

            double[] result = new double[hoSums.Length];
            for (int i = 0; i < hoSums.Length; ++i)
                result[i] = Math.Exp(hoSums[i] - max) / scale;

            return result;
        }
        // seek and return the best weights
        public double[] Train(StringBuilder sb, double[][] trainMatrix) 
        {
            int numWeights = (this.numInput * this.numHidden) + (this.numHidden * this.numOutput) + this.numHidden + this.numOutput;
            //double[] currWeights = new double[numWeights];

            // use PSO to seek best weights
            int numberParticles = 10;
            int numberIterations = 500;
            int iteration = 0;
            int Dim = numWeights; // number of values to solve for
            double minX = -5.0; // for each weight
            double maxX = 5.0;

            Particle[] swarm = new Particle[numberParticles];
            double[] bestGlobalPosition = new double[Dim]; // best solution found by any particle in the swarm. implicit initialization to all 0.0
            double bestGlobalFitness = double.MaxValue; // smaller values better

            double minV = -0.1 * maxX;  // velocities
            double maxV = 0.1 * maxX;

            for (int i = 0; i < swarm.Length; ++i) // initialize each Particle in the swarm with random positions and velocities
            {
                double[] randomPosition = new double[Dim];
                for (int j = 0; j < randomPosition.Length; ++j)
                {
                    double lo = minX;
                    double hi = maxX;
                    randomPosition[j] = (hi - lo) * rnd.NextDouble() + lo;
                }

                double fitness = CrossEntropy(trainMatrix, randomPosition); // smaller values better
                double[] randomVelocity = new double[Dim];

                for (int j = 0; j < randomVelocity.Length; ++j)
                {
                    double lo = -1.0 * Math.Abs(maxX - minX);
                    double hi = Math.Abs(maxX - minX);
                    randomVelocity[j] = (hi - lo) * rnd.NextDouble() + lo;
                }
                swarm[i] = new Particle(randomPosition, fitness, randomVelocity, randomPosition, fitness);

                // does current Particle have global best position/solution?
                if (swarm[i].fitness < bestGlobalFitness)
                {
                    bestGlobalFitness = swarm[i].fitness;
                    swarm[i].position.CopyTo(bestGlobalPosition, 0);
                }
            } // initialization

            double w = 0.729; // inertia weight.
            double c1 = 1.49445; // cognitive/local weight
            double c2 = 1.49445; // social/global weight
            double r1, r2; // cognitive and social randomizations

            //Console.WriteLine("Entering main PSO weight estimation processing loop");
            while (iteration < numberIterations)
            {
                ++iteration;
                double[] newVelocity = new double[Dim];
                double[] newPosition = new double[Dim];
                double newFitness;

                for (int i = 0; i < swarm.Length; ++i) // each Particle
                {
                    Particle currP = swarm[i];

                    for (int j = 0; j < currP.velocity.Length; ++j) // each x value of the velocity
                    {
                        r1 = rnd.NextDouble();
                        r2 = rnd.NextDouble();

                        newVelocity[j] = (w * currP.velocity[j]) +
                          (c1 * r1 * (currP.bestPosition[j] - currP.position[j])) +
                          (c2 * r2 * (bestGlobalPosition[j] - currP.position[j])); // new velocity depends on old velocity, best position of parrticle, and best position of any particle

                        if (newVelocity[j] < minV)
                            newVelocity[j] = minV;
                        else if (newVelocity[j] > maxV)
                            newVelocity[j] = maxV;     // crude way to keep velocity in range
                    }

                    newVelocity.CopyTo(currP.velocity, 0);

                    for (int j = 0; j < currP.position.Length; ++j)
                    {
                        newPosition[j] = currP.position[j] + newVelocity[j];  // compute new position
                        if (newPosition[j] < minX)
                            newPosition[j] = minX;
                        else if (newPosition[j] > maxX)
                            newPosition[j] = maxX;
                    }

                    newPosition.CopyTo(currP.position, 0);

                    newFitness = CrossEntropy(trainMatrix, newPosition);  // compute error of the new position
                    currP.fitness = newFitness;

                    if (newFitness < currP.bestFitness) // new particle best?
                    {
                        newPosition.CopyTo(currP.bestPosition, 0);
                        currP.bestFitness = newFitness;
                    }

                    if (newFitness < bestGlobalFitness) // new global best?
                    {
                        newPosition.CopyTo(bestGlobalPosition, 0);
                        bestGlobalFitness = newFitness;
                    }

                } // each Particle

                ////Console.WriteLine(swarm[0].ToString());
                //Console.ReadLine();

            } // while

            //Console.WriteLine("Processing complete");
            sb.AppendLine("Final best (smallest) cross entropy error = ");
            sb.AppendLine(bestGlobalFitness.ToString("F4"));

            return bestGlobalPosition;

        } // Train

        private double CrossEntropy(double[][] trainData, double[] weights) // (sum) Cross Entropy
        {
            // how good (cross entropy) are weights? CrossEntropy is error so smaller values are better
            this.SetWeights(weights); // load the weights and biases to examine

            double sce = 0.0; // sum of cross entropy

            for (int i = 0; i < trainData.Length; ++i) // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)  where the parens are not really there
            {
                double[] currInputs = new double[numInput];
                for (int j = 0; j < currInputs.Count(); j++)
                {
                    currInputs[j] = trainData[i][j]; 
                }
                double[] currExpected = new double[numOutput];
                for (int j = 0; j < currExpected.Count(); j++)
                {
                    currExpected[j] = trainData[i][j + numInput];
                }
                double[] currOutputs = this.ComputeOutputs(currInputs); // run the jnputs through the neural network


                // compute ln of each nn output (and the sum)
                double currSum = 0.0;
                for (int j = 0; j < currOutputs.Length; ++j)
                {
                    if (currExpected[j] != 0.0)
                        currSum += currExpected[j] * Math.Log(currOutputs[j]);
                }
                sce += currSum; // accumulate
            }
            return -sce;
        } // CrossEntropy

        public double Test(StringBuilder sb, double[][] testMatrix) // returns the accuracy (percent correct predictions)
        {
            // assumes that weights have been set using SetWeights
            int numCorrect = 0;
            int numWrong = 0;
            this.QTPredicted = 0;

            for (int i = 0; i < testMatrix.Length; ++i) // walk thru each test case. looks like (6.9 3.2 5.7 2.3) (0 0 1)  where the parens are not really there
            {
                double[] currInputs = new double[numInput];
                for (int j = 0; j < currInputs.Count(); j++)
                {
                    currInputs[j] = testMatrix[i][j];
                }
                double[] currOutputs = new double[numOutput];
                for (int j = 0; j < currOutputs.Count(); j++)
                {
                    currOutputs[j] = testMatrix[i][j + (numInput)];
                }
                double[] currPredicted = this.ComputeOutputs(currInputs); // outputs are in softmax form -- each between 0.0, 1.0 representing a prob and summing to 1.0

                // use winner-takes all -- highest prob of the prediction
                int indexOfLargest = Helpers.IndexOfLargest(currPredicted);

                if (i <= 3) // just a few for demo purposes
                {
                    sb.AppendLine("-----------------------------------");
                    sb.Append("Input:     ");
                    Helpers.ShowVector(sb, currInputs, 2, true);
                    sb.Append("Output:    ");
                    Helpers.ShowVector(sb, currOutputs, 1, false);

                    for (int j = 0; j < currOutputs.Count(); j++)
                    {
                        if (currOutputs[j] == 1.0)
                        {
                            sb.AppendLine(string.Concat(" (output ",  j+1, ")"));
                            //possible go have more than one with 1 why?
                            break;
                        }
                        else
                        {
                            //nothing
                        }
                    }
                    sb.Append("Predicted: ");
                    Helpers.ShowVector(sb, currPredicted, 1, false);
                    for (int j = 0; j < currOutputs.Count(); j++)
                    {
                        if (indexOfLargest == j)
                        {
                            sb.AppendLine(string.Concat(" (output ",  j+1, ")"));
                            if (i == 0)
                            {
                                this.QTPredicted = indexOfLargest;
                            }
                        }
                        else
                        {
                            //nothing
                        }
                    }
                    if (currOutputs[indexOfLargest] == 1)
                        sb.AppendLine("correct");
                    else
                        sb.AppendLine("wrong");
                    sb.AppendLine("-----------------------------------");
                }
                if (currOutputs[indexOfLargest] == 1)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            sb.AppendLine(". . .");

            double percentCorrect = (numCorrect * 1.0) / (numCorrect + numWrong);
            sb.AppendLine("\nCorrect = " + numCorrect);
            sb.AppendLine("Wrong = " + numWrong);

            return percentCorrect;
        } // Test

    } // class NeuralNetwork


    // ==============================================================================================

    public class Helpers
    {
        static Random rnd = new Random(0);

        public static double[][] MakeMatrix(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        public static void ShuffleRows(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; ++i)
            {
                int r = rnd.Next(i, matrix.Length);
                double[] tmp = matrix[r];
                matrix[r] = matrix[i];
                matrix[i] = tmp;
            }
        }

        public static int IndexOfLargest(double[] vector)
        {
            int indexOfLargest = 0;
            double maxVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > maxVal)
                {
                    maxVal = vector[i];
                    indexOfLargest = i;
                }
            }
            return indexOfLargest;
        }

        public static void ShowVector(StringBuilder sb, double[] vector, int decimals, bool newLine)
        {
            string fmt = "F" + decimals;
            for (int i = 0; i < vector.Length; ++i)
            {
                if (i > 0 && i % 12 == 0)
                    sb.AppendLine("");
                if (vector[i] >= 0.0) sb.Append(" ");
                sb.Append(vector[i].ToString(fmt) + " ");
            }
            if (newLine == true) sb.AppendLine("");
        }

        public static void ShowMatrix(StringBuilder sb, double[][] matrix, int inVarCount, int numRows)
        {
            int ct = 0;
            if (numRows == -1) numRows = int.MaxValue;
            for (int i = 0; i < matrix.Length && ct < numRows; ++i)
            {
                for (int j = 0; j < matrix[0].Length; ++j)
                {
                    if (matrix[i][j] >= 0.0) sb.Append(" ");
                    if (j == inVarCount) sb.AppendLine("-> ");
                    sb.Append(matrix[i][j].ToString("F2") + " ");
                }
                sb.AppendLine("");
                ++ct;
            }
            sb.AppendLine("");
        }

    } // class Helpers

    // ==============================================================================================

    public class Particle
    {
        public double[] position; // equivalent to x-Values and/or solution
        public double fitness;
        public double[] velocity;

        public double[] bestPosition; // best position found so far by this Particle
        public double bestFitness;

        public Particle(double[] position, double fitness, double[] velocity, double[] bestPosition, double bestFitness)
        {
            this.position = new double[position.Length];
            position.CopyTo(this.position, 0);
            this.fitness = fitness;
            this.velocity = new double[velocity.Length];
            velocity.CopyTo(this.velocity, 0);
            this.bestPosition = new double[bestPosition.Length];
            bestPosition.CopyTo(this.bestPosition, 0);
            this.bestFitness = bestFitness;
        }

        public override string ToString()
        {
            string s = "";
            s += "==========================\n";
            s += "Position: ";
            for (int i = 0; i < this.position.Length; ++i)
                s += this.position[i].ToString("F2") + " ";
            s += "\n";
            s += "Fitness = " + this.fitness.ToString("F4") + "\n";
            s += "Velocity: ";
            for (int i = 0; i < this.velocity.Length; ++i)
                s += this.velocity[i].ToString("F2") + " ";
            s += "\n";
            s += "Best Position: ";
            for (int i = 0; i < this.bestPosition.Length; ++i)
                s += this.bestPosition[i].ToString("F2") + " ";
            s += "\n";
            s += "Best Fitness = " + this.bestFitness.ToString("F4") + "\n";
            s += "==========================\n";
            return s;
        }

    }
}

