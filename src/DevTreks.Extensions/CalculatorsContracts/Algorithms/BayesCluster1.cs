using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Extensions.Algorithms
{
    // <summary>
    ///Purpose:		Simple bayes clustering algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	MSDN. McCaffrey. January, 2012
    public class BayesCluster1
    {
        public BayesCluster1()
        {
            //initiate algo

        }
        private CalculatorParameters _params { get; set; }
        static Random random = null;

        public void RunAlgorithm(double[,] problemData)
        {
            try
            {
                Console.WriteLine("\nBegin data clustering using Naive Bayes demo\n");
                random = new Random(6); // seed of 6 gives a nice demo
                //A tuple is a data structure that has a specific number and sequence of values
                //An example of a tuple is a data structure with three elements (known as a 3-tuple or triple) that is used to store an identifier such as a person's name in the first element, a year in the second element, and the person's income for that year in the third element. The .NET Framework directly supports tuples with one to seven elements. In addition, you can create tuples of eight or more elements by nesting tuple objects in the Rest property of a Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> object.

                //Tuples are commonly used in four ways:

                //To represent a single set of data. For example, a tuple can represent a database record, and its components can represent individual fields of the record.

                //To provide easy access to, and manipulation of, a data set.

                //To return multiple values from a method without using out parameters (in C#) or ByRef parameters (in Visual Basic).

                //To pass multiple values to a method through a single parameter. For example, the Thread.Start(Object) method has a single parameter that lets you supply one value to the method that the thread executes at startup time. If you supply a Tuple<T1, T2, T3> object as the method argument, you can supply the thread’s startup routine with three items of data.


                int numTuples = 8;
                Console.WriteLine("Generating " + numTuples + " tuples of location-income-politics data");
                string[] rawData = MakeData(numTuples);  // random data

                Console.WriteLine("\nRaw data in string form is:\n");
                ShowData(rawData, numTuples);
                Console.WriteLine("");

                string[] attNames = new string[] { "Location", "Income", "Politics" };

                string[][] attValues = new string[attNames.Length][]; // 3 attributes = location, income, politics
                attValues[0] = new string[] { "Urban", "Suburban", "Rural" }; // Location
                attValues[1] = new string[] { "Low", "Medium", "High", "VeryHigh" }; // Income
                attValues[2] = new string[] { "Liberal", "Conservative" }; // Politics

                Console.WriteLine("Loading raw data into memory\n");
                string[][] tuples = LoadData(rawData, attValues);
                //ShowData(tuples, numTuples);
                //Console.WriteLine("");

                Console.WriteLine("Converting raw data to int form and storing in memory\n");
                int[][] tuplesAsInt = TuplesToInts(tuples, attValues);

                Console.WriteLine("Data in int form is:\n");
                ShowData(tuplesAsInt, numTuples);
                Console.WriteLine("");

                int numClusters = 3;
                int numTrials = 20;  // times to sample to get good seed indexes (different tuples)
                Console.WriteLine("\nSetting numClusters to " + numClusters);

                Console.WriteLine("\nInitializing Naive Bayes joint counts with Laplacian smoothing");
                int[][][] jointCounts = InitJointCounts(tuplesAsInt, attValues, numClusters); // inits with 1 in all cells for Laplacian
                int[] clusterCounts = new int[numClusters];  // implicitly initialized to 0

                Console.WriteLine("\nBegin clustering data using Naive Bayes (with equal priors)");
                int[] clustering = Cluster(tuplesAsInt, numClusters, numTrials, jointCounts, clusterCounts, true); // equal priors = true
                Console.WriteLine("Clustering complete");

                Console.WriteLine("\nClustering in internal form is:\n");
                for (int i = 0; i < clustering.Length; ++i)
                    Console.Write(clustering[i] + " ");
                Console.WriteLine("\n");

                Console.WriteLine("Raw data displayed in clusters is:\n");
                DisplayClustered(tuplesAsInt, numClusters, clustering, attValues);

                //Console.WriteLine("Refining clustering (using non equal priors)");
                //Refine(20, tuplesAsInt, clustering, jointCounts, clusterCounts, false);  // 20 attempts. equal priors is false use computed P(ci)s based on counts)

                //Console.WriteLine("\nRefined clustering in internal form is:\n");
                //for (int i = 0; i < clustering.Length; ++i)
                //  Console.Write(clustering[i] + " ");
                //Console.WriteLine("\n");

                //Console.WriteLine("Raw data displayed in (refined) clusters is:\n");
                //DisplayClustered(tuplesAsInt, numClusters, clustering, attValues);

                Console.WriteLine("\nEnd demo\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        } // Main

        static int[] Cluster(int[][] tuplesAsInt, int numClusters, int numTrials, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
        {
            // return a good clustering
            //
            // get numClusters good indexes, use to assign one tuple to each cluster
            // loop each tuple
            //  if tuple has already been assigned to a cluster then continue
            //  compute probs that tuple belongs to each cluster
            //  determine cluster k' that has greatest probability
            //  assign tuple to k'
            // end each tuple

            // consider computing and returning (as an out parameter) the average of the greatest probabilities as a confidence metric
            // this would allow you to call Cluster multiple times and use the clustering that gave the greatest confidence value

            // create clustering array
            int numRows = tuplesAsInt.Length;  // number of tuples
            int[] clustering = new int[numRows];  // the result is a clustering. one cell per tuple, value in cell is the cluster (-1 for unassigned)
            for (int i = 0; i < clustering.Length; ++i)
                clustering[i] = -1;

            // seed clustering[] with 'good' indexes
            int[] goodIndexes = GetGoodIndexes(tuplesAsInt, numClusters, numTrials);  // get indexes of tuples that are different from each other (numTrials attempts)

            Console.Write("Seed indexes = ");
            for (int i = 0; i < goodIndexes.Length; ++i)
                Console.Write(goodIndexes[i] + " ");
            Console.WriteLine("");

            for (int i = 0; i < goodIndexes.Length; ++i)  // assign seed tuples to clusters
            {
                int idx = goodIndexes[i];
                clustering[idx] = i;
            }

            // update joint counts
            for (int i = 0; i < goodIndexes.Length; ++i)  // i also represents a cluster
            {
                int idx = goodIndexes[i];
                for (int j = 0; j < tuplesAsInt[idx].Length; ++j)
                {
                    int v = tuplesAsInt[idx][j];
                    ++jointCounts[j][v][i];     // very tricky indexing; need diagrams
                }
            }

            // update cluster counts
            for (int i = 0; i < clusterCounts.Length; ++i)
                ++clusterCounts[i];

            // main loop
            // here we always iterate from [0], in order. this gives more weight to early tuples. consider walking thru tuples in random or scrambled order
            for (int i = 0; i < tuplesAsInt.Length; ++i)
            {
                if (clustering[i] != -1) continue;  // if value (a cluster id) is not -1 (like 0 or 2) then the tuple has been assigned so skip

                double[] allProbabilities = AllProbabilities(tuplesAsInt[i], jointCounts, clusterCounts, equalPriors);  // probs that curr tuple belongs to each possible cluster. equal priors
                int c = IndexOfLargestProbability(allProbabilities);  // the cluster that has greatest probability

                clustering[i] = c;  // assign tuple i to cluster c

                // update joint counts
                for (int j = 0; j < tuplesAsInt[i].Length; ++j)
                {
                    int v = tuplesAsInt[i][j];
                    ++jointCounts[j][v][c];     // very tricky; need diagrams
                }

                // update clusterCounts
                ++clusterCounts[c];

                // validate
                //Validate(jointCounts, clustering, clusterCounts, numClusters);

            } // main loop

            return clustering;
        } // Cluster

        static void DisplayClustered(int[][] tuplesAsInt, int numClusters, int[] clustering, string[][] attValues)
        {
            // show the data as strings, clustered
            Console.WriteLine("--------------------------------------");
            for (int k = 0; k < numClusters; ++k) // display by cluster
            {
                for (int i = 0; i < tuplesAsInt.Length; ++i)
                {
                    if (clustering[i] == k)
                    {
                        for (int j = 0; j < tuplesAsInt[i].Length; ++j)
                        {
                            int v = tuplesAsInt[i][j];
                            string s = attValues[j][v];
                            Console.Write(s.ToString().PadRight(12) + " ");
                        }
                        Console.WriteLine("");
                    }
                }
                Console.WriteLine("--------------------------------------");
            } // k
        }

        static void Refine(int numTrials, int[][] tuplesAsInt, int[] clustering, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
        {
            // after one pass of Cluster, call this method to try and improve the clustering
            // select a random tuple, unassign it from its cluster, reassign (numTrials times)
            for (int n = 0; n < numTrials; ++n)
            {
                int randomTupleIndex = GetTupleIndex(tuplesAsInt, clustering, clusterCounts);
                if (randomTupleIndex == -1)
                    throw new Exception("Failed to get valid randomTupleIndex in Refine");

                Unassign(randomTupleIndex, clustering, tuplesAsInt, jointCounts, clusterCounts);

                double[] allProbabilities = AllProbabilities(tuplesAsInt[randomTupleIndex], jointCounts, clusterCounts, equalPriors);  // probs that tuple at randomTupleIndex belongs to each possible cluster. use priors?
                int c = IndexOfLargestProbability(allProbabilities);  // the cluster that has greatest probability

                clustering[randomTupleIndex] = c;  // assign tuple i to cluster c

                // update joint counts
                for (int j = 0; j < tuplesAsInt[randomTupleIndex].Length; ++j)
                {
                    int v = tuplesAsInt[randomTupleIndex][j];
                    ++jointCounts[j][v][c];     // very tricky; need diagrams
                }

                // update clusterCounts
                ++clusterCounts[c];
            }
        }

        static int GetTupleIndex(int[][] tuplesAsInt, int[] clustering, int[] clusterCounts)
        {
            // helper for Refine
            // get the index of a random tuple, where the tuple is not the only member of a cluster
            // or, in other words, index of a tuple that is part of a cluster with 2 or more tuples.
            int sanityCount = 0;
            while (sanityCount < 10000)
            {
                ++sanityCount;
                int ri = random.Next(0, tuplesAsInt.Length); // a candidate index of a tuple
                int c = clustering[ri];                      // cluster that the tuple is assigned to
                if (clusterCounts[c] > 1)                    // the cluster has 2 or more tuples assigned
                    return ri;
            }
            return -1; //error
        }

        static void Unassign(int tupleIndex, int[] clustering, int[][] tuplesAsInt, int[][][] jointCounts, int[] clusterCounts)
        {
            // helper for refine
            // remove a tuple from clustering by modifying clustering[], jointCounts[][][], and clusterCounts[]
            int c = clustering[tupleIndex];  // curr tuple that tupleIndex is assigned to
            clustering[tupleIndex] = -1;     // unassign

            for (int i = 0; i < tuplesAsInt[tupleIndex].Length; ++i)  // i is an attribute (or a tupleAsInt col)
            {
                int v = tuplesAsInt[tupleIndex][i];  // v is an attribute value
                --jointCounts[i][v][c];              // undo the joint count
            }

            --clusterCounts[c];  // undo the cluster count
        }

        // -----------------------------------------------------------------------------------------------------------------------------------

        static int IndexOfLargestProbability(double[] allProbabilities)
        {
            double largestP = 0.0;
            int idx = 0;

            for (int i = 0; i < allProbabilities.Length; ++i)
            {
                if (allProbabilities[i] > largestP)
                {
                    largestP = allProbabilities[i]; idx = i;
                }
            }
            return idx;
        }

        static double[] AllProbabilities(int[] tupleAsInt, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
        {
            // returns an array of probs that the tuple belongs to each cluster
            // calls AllPartialProbs which in turn calls PartialProbability
            // the prob that a tuple belongs to some cluster c is the partial for c / sum of partials for all clusters
            // equalPriors is passed along. if true, all P(ci) are equal. if false P(ci) = count(ci) / total count.
            int numClusters = clusterCounts.Length;
            double[] result = new double[numClusters];

            double[] partialProbs = AllPartialProbs(tupleAsInt, jointCounts, clusterCounts, equalPriors);  // compute and store all partials
            double sumOfPartials = 0.0;
            for (int i = 0; i < partialProbs.Length; ++i)
                sumOfPartials += partialProbs[i];

            for (int i = 0; i < result.Length; ++i)
                result[i] = partialProbs[i] / sumOfPartials;

            //Console.WriteLine("All probs = " + result[0].ToString("F4") + " " + result[1].ToString("F4") + " " + result[2].ToString("F4"));
            //Console.ReadLine();

            return result;
        }

        static double[] AllPartialProbs(int[] tupleAsInt, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
        {
            // compute Partial Probabilities of tupleAsInt for all clusters
            int numClusters = clusterCounts.Length;
            double[] result = new double[numClusters]; // one PartialProbability for each cluster
            for (int c = 0; c < result.Length; ++c)
            {
                double p = PartialProbability(tupleAsInt, jointCounts, clusterCounts, c, equalPriors);  // compute 
                result[c] = p;  // store
            }
            //Console.WriteLine("Partials probs = " + result[0].ToString("F4") + " " + result[1].ToString("F4") + " " + result[2].ToString("F4"));
            //Console.ReadLine();
            return result;
        }

        static double PartialProbability(int[] tupleAsInt, int[][][] jointCounts, int[] clusterCounts, int c, bool equalPriors)
        {
            // returns the partial probability that a tuple with int values in tupleAsInt belongs to cluster c (by using the values in jointCounts and clusterCounts)
            // ex. Suppose tupleAsInt is (1,2,0) == (suburban,high,liberal) and suppose c = 1 (assume republican if hidden labels are  democrat(0) or republican(1) or independent(2))
            // we want the PP that the tuple belongs to c1 which is
            // P(c1 | suburban) * P(c1 | high) * P(c1 | liberal) * P(c1)  which is IN THEORY
            // count(c1 & suburban) / count(c1)  *  count(c1 & high) / count(c1)  *  count(c1 & liberal) / count(c1)  *  [ count(c1) / [count(c0) + count(c1) + count(c2)] ]
            // but with Laplacian smoothing (all joint counts have 1 added to prevent a zero-out) and the calculation is:
            // count(c1 & suburban) / count(c1)+3  *  count(c1 & high) / count(c1)+3  *  count(c1 & liberal) / count(c1)+3  *  [ count(c1) / [count(c0) + count(c1) + count(c2)] ], where the 3 is numClusters
            // if equal priors is true, the last term becomes just 1 / numclusters (e.g., 1/3) but since all the same the term just drops out.

            int totalCount = 0;  // count(c0) + count(c1) + count(c2) 
            for (int i = 0; i < clusterCounts.Length; ++i)
                totalCount += clusterCounts[i];  // total number of tuples that have been assigned to a cluster

            int numClusters = clusterCounts.Length;
            int clusterCount = clusterCounts[c] + numClusters;  // number of tuples assigned to cluster c plus the Laplacian factor. ex: count(c1) alone plus 3

            double[] probs = new double[jointCounts.Length + 1]; // there is one prob for each attribute (location,income,political) plus 1 for the cluster prob
            for (int i = 0; i < probs.Length - 1; ++i)  // (all except the last prob) -- the joint probabilities
            {
                int j = tupleAsInt[i];  // i is each attribute type
                double p = jointCounts[i][j][c] / (1.0 * clusterCount);  // tricky. need a diagram to understand this.
                //Console.WriteLine("partial " + i + " = " + jointCounts[i][j][c] + " over " + clusterCount);
                //Console.ReadLine();
                probs[i] = p;
            }

            //Console.WriteLine("last partial = " + clusterCounts[c] + " over " + totalCount);
            //Console.ReadLine();

            // P(c). take care of the last prob.
            double clusterP = 0.0;
            if (equalPriors == true)
                clusterP = 1.0 / numClusters;
            else
                clusterP = clusterCounts[c] / (1.0 * totalCount);

            probs[probs.Length - 1] = clusterP;

            double result = 1.0; // now multiply the probs together

            for (int i = 0; i < probs.Length; ++i)
                result *= probs[i];

            return result;
        } // PartialProbability

        // -----------------------------------------------------------------------------------------------------------------------------------

        static int[][][] InitJointCounts(int[][] tuplesAsInt, string[][] attValues, int numClusters)
        {
            // allocate joint counts and set all cells to 1 for Laplacian smoothing (a joint count of 0 is bad)
            // [attribute][attValue][cluster]. ex: [1][0][2] holds the count of income(1) equal to lown (0) AND cluster c(2)

            int[][][] result = new int[attValues.Length][][]; // allocate
            for (int i = 0; i < result.Length; ++i)
            {
                int numCells = attValues[i].Length;
                result[i] = new int[numCells][];
            }

            for (int i = 0; i < result.Length; ++i)
            {
                for (int j = 0; j < result[i].Length; ++j)
                {
                    result[i][j] = new int[numClusters];
                }
            }

            for (int i = 0; i < result.Length; ++i)         // assign -1 to each cell
                for (int j = 0; j < result[i].Length; ++j)
                    for (int k = 0; k < result[i][j].Length; ++k)
                        result[i][j][k] = 1;

            return result;

        } // InitJointCounts

        static void ShowJointCounts(int[][][] jointCounts)
        {
            for (int i = 0; i < jointCounts.Length; ++i)
            {
                for (int j = 0; j < jointCounts[i].Length; ++j)
                {
                    for (int k = 0; k < jointCounts[i][j].Length; ++k)
                    {
                        Console.Write(jointCounts[i][j][k] + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }
        }


        // ------------------------------------------------------------------------------------------------------------------

        static int Delta(int[][] tuplesAsInt, int a, int b)
        {
            // helper for GetRandomIndexes
            // number of positions where tuplesAsInt[a] and tuplesAsInt[b] differ
            int numCols = tuplesAsInt[0].Length;
            int ct = 0;
            for (int j = 0; j < numCols; ++j)
                if (tuplesAsInt[a][j] != tuplesAsInt[b][j])
                    ++ct;
            return ct;
        }

        static int TotalDelta(int[][] tuplesAsInt, int[] indexes)
        {
            // helper for GetRandomIndexes
            // total number of positions where tuples specified in indexes[] differ, computed a pair at a time
            int numCols = tuplesAsInt[0].Length;
            int ct = 0;
            for (int i = 0; i < indexes.Length - 1; ++i)
            {
                for (int j = i + 1; j < indexes.Length; ++j)
                {
                    int a = indexes[i];
                    int b = indexes[j];
                    ct += Delta(tuplesAsInt, a, b);
                }
            }
            return ct;
        }

        static int[] GetRandomIndexes(int n, int maxIndex)
        {
            // get n distinct indexes between [0,maxIndex) // [inclusive,exclusive)
            // assumes a global (class-scope) random object exists
            Dictionary<int, bool> d = new Dictionary<int, bool>();  // key is an index, bool is true if already used (dummy)

            int[] result = new int[n];
            int ct = 0;
            int sanityCount = 0;  // to prevent an infinite loop
            while (ct < n && sanityCount < 10000)
            {
                ++sanityCount;
                int idx = random.Next(0, maxIndex);
                if (d.ContainsKey(idx) == false)
                {
                    result[ct] = idx;
                    d[idx] = true;  // dummy
                    ++ct;
                }
            }
            return result;
        } // GetRandomIndexes

        static int[] GetGoodIndexes(int[][] tuplesAsInt, int n, int numTrials)
        {
            // calls GetRandomIndexes
            // get n 'good' indexes -- where the total delta for all tuples of the indexes is high (we want tuples that are different)
            int[] result = new int[n];
            int numRows = tuplesAsInt.Length;
            int highestDelta = 0;

            for (int i = 0; i < numTrials; ++i)
            {
                int[] randomIndexes = GetRandomIndexes(n, numRows);
                int totalDelta = TotalDelta(tuplesAsInt, randomIndexes);
                if (totalDelta > highestDelta)
                {
                    highestDelta = totalDelta;
                    Array.Copy(randomIndexes, result, result.Length);
                }
            }
            return result;
        }

        // ------------------------------------------------------------------------------------------------------------------

        static string[] MakeData(int numRows)
        {
            string[] result = new string[numRows];
            for (int i = 0; i < numRows; ++i)
            {
                string party = MakeParty();  // hidden label is a political party affiliation
                string location = MakeLocation(party);  // urban, suburban, rural
                string income = MakeIncome(party);      // low, medium, high, veryhigh
                string political = MakePolitical(party);  // liberal, conservative
                string s = location + "," + income + "," + political;
                result[i] = s;
            }
            return result;
        } // MakeData

        static void ShowData(string[] rawData, int top)
        {
            for (int i = 0; i < top; ++i)
                Console.WriteLine(rawData[i]);
        }

        static string MakeParty() // hidden label
        {
            int r = random.Next(0, 100); // [0,100) => [0,99]
            if (r >= 0 && r <= 9)        // 10% independent
                return "independent";
            else if (r >= 10 && r <= 59) // 50% democart
                return "democrat";
            else
                return "republican";       // 40% republican
        }

        static string MakeLocation(string party) // where a person lives 
        {
            int r = random.Next(0, 100); // [0,100) => [0,99]
            if (party == "independent")
            {
                if (r >= 0 && r <= 19) return "Urban";  // 20%
                else if (r >= 20 && r <= 49) return "Suburban"; // 30%
                else return "Rural"; // 50%
            }
            else if (party == "democrat")
            {
                if (r >= 0 && r <= 69) return "Urban";  // 70%
                else if (r >= 70 && r <= 89) return "Suburban"; // 20%
                else return "Rural";  // 10%
            }
            else // republican
            {
                if (r >= 0 && r <= 19) return "Urban";  // 20%
                else if (r >= 20 && r <= 69) return "Suburban"; // 50%
                else return "Rural";  // 30%
            }
        } // MakeLocation

        static string MakeIncome(string party)
        {
            int r = random.Next(0, 100); // [0,100) => [0,99]
            if (party == "independent")
            {
                if (r >= 0 && r <= 19) return "Low";          // 20%
                else if (r >= 20 && r <= 49) return "Medium"; // 30%
                else if (r >= 50 && r <= 79) return "High";   // 30%
                else return "VeryHigh";                       // 20%
            }
            else if (party == "democrat")
            {
                if (r >= 0 && r <= 59) return "Low";          // 60%
                else if (r >= 60 && r <= 79) return "Medium"; // 20%
                else if (r >= 80 && r <= 89) return "High";   // 10%
                else return "VeryHigh";                       // 10%
            }
            else // republican
            {
                if (r >= 0 && r <= 39) return "Low";          // 40%
                else if (r >= 40 && r <= 59) return "Medium"; // 10%
                else if (r >= 60 && r <= 69) return "High";   // 10%
                else return "VeryHigh";                       // 40%
            }
        } // MakeIncome

        static string MakePolitical(string party)
        {
            int r = random.Next(0, 100); // [0,100) => [0,99]
            if (party == "independent")
            {
                if (r >= 0 && r <= 49) return "Conservative";   // 50%
                else return "Liberal";                          // 50%
            }
            else if (party == "democrat")
            {
                if (r >= 0 && r <= 19) return "Conservative";   // 20%
                else return "Liberal";                          // 80%
            }
            else // republican
            {
                if (r >= 0 && r <= 69) return "Conservative";   // 70%
                else return "Liberal";                          // 30%
            }
        } // MakePolitical

        static string[][] LoadData(string[] rawData, string[][] attValues)
        {
            // iterate thru raw data, parse on commas, load into memory
            int numRows = rawData.Length;
            int numCols = attValues.Length;

            string[][] result = new string[numRows][]; // allocate
            for (int i = 0; i < numRows; ++i)
                result[i] = new string[numCols];

            // read, parse, store
            for (int i = 0; i < rawData.Length; ++i)
            {
                string[] tokens = rawData[i].Split(',');
                for (int j = 0; j < tokens.Length; ++j)
                    result[i][j] = tokens[j];
            }

            return result;
        } // LoadData

        static void ShowData(string[][] tuples, int top)
        {
            for (int i = 0; i < top; ++i)
            {
                for (int j = 0; j < tuples[i].Length; ++j)
                {
                    Console.Write(tuples[i][j] + " ");
                }
                Console.WriteLine("");
            }
        } // ShowData

        static void ShowData(int[][] tuples, int top)
        {
            for (int i = 0; i < top; ++i)
            {
                for (int j = 0; j < tuples[i].Length; ++j)
                {
                    Console.Write(tuples[i][j] + " ");
                }
                Console.WriteLine("");
            }
        } // ShowData

        static int[][] TuplesToInts(string[][] tuples, string[][] attValues) // converts a matrix of tuples in string form to int form
        {
            // create an array of lookup table thing. [0] = attribute (e.g., location), key = "rural", then value = 2
            Dictionary<string, int>[] lookup = new Dictionary<string, int>[attValues.Length];  // one dictionary for each attribute
            for (int i = 0; i < attValues.Length; ++i)  // each attribute
            {
                lookup[i] = new Dictionary<string, int>();
                for (int j = 0; j < attValues[i].Length; ++j) // each value of curr attribute
                    lookup[i].Add(attValues[i][j], j); //
            }
            // scan tuples and convert using the lookup
            int numRows = tuples.Length;
            int numCols = attValues.Length;
            int[][] result = new int[numRows][]; // allocate
            for (int i = 0; i < numRows; ++i)
                result[i] = new int[numCols];

            for (int i = 0; i < numRows; ++i) // each row/tuple
            {
                for (int j = 0; j < numCols; ++j) // each col/attribute
                {
                    string v = tuples[i][j]; // eg, "rural"
                    int attAsInt = lookup[j][v]; // then 2. j is the tuple column which is also the attribute
                    result[i][j] = attAsInt;
                }
            }
            return result;
        } // TuplesToInts
    }
}
//original McCaffrey reference
//using System;
//using System.Collections.Generic;

//// INBIAC - Iterative Naive Bayesian Inference Agglomerative Clustering
//// Program to demonstrate data clustering using the INBIAC algorithm.
//// The basic idea is to seed each of m clusters with a single data tuple.
//// Then, each remaining tuple is assigned to the cluster that has the greatest
//// probability, where probability is computed using Naive Bayes inference.

//namespace ClusteringBayesian
//{
//  class ClusteringBayesianProgram
//  {
//    static Random random = null;

//    static void Main(string[] args)
//    {
//      try
//      {
//        Console.WriteLine("\nBegin data clustering using Naive Bayes demo\n");
//        random = new Random(6); // seed of 6 gives a nice demo

//        int numTuples = 8;
//        Console.WriteLine("Generating " + numTuples + " tuples of location-income-politics data");
//        string[] rawData = MakeData(numTuples);  // random data

//        Console.WriteLine("\nRaw data in string form is:\n");
//        ShowData(rawData, numTuples);
//        Console.WriteLine("");

//        string[] attNames = new string[] { "Location", "Income", "Politics" };

//        string[][] attValues = new string[attNames.Length][]; // 3 attributes = location, income, politics
//        attValues[0] = new string[] { "Urban", "Suburban", "Rural" }; // Location
//        attValues[1] = new string[] { "Low", "Medium", "High", "VeryHigh" }; // Income
//        attValues[2] = new string[] { "Liberal", "Conservative" }; // Politics

//        Console.WriteLine("Loading raw data into memory\n");
//        string[][] tuples = LoadData(rawData, attValues);
//        //ShowData(tuples, numTuples);
//        //Console.WriteLine("");
        
//        Console.WriteLine("Converting raw data to int form and storing in memory\n");
//        int[][] tuplesAsInt = TuplesToInts(tuples, attValues);

//        Console.WriteLine("Data in int form is:\n");
//        ShowData(tuplesAsInt, numTuples);
//        Console.WriteLine("");

//        int numClusters = 3;
//        int numTrials = 20;  // times to sample to get good seed indexes (different tuples)
//        Console.WriteLine("\nSetting numClusters to " + numClusters);

//        Console.WriteLine("\nInitializing Naive Bayes joint counts with Laplacian smoothing");
//        int[][][] jointCounts = InitJointCounts(tuplesAsInt, attValues, numClusters); // inits with 1 in all cells for Laplacian
//        int[] clusterCounts = new int[numClusters];  // implicitly initialized to 0

//        Console.WriteLine("\nBegin clustering data using Naive Bayes (with equal priors)");
//        int[] clustering = Cluster(tuplesAsInt, numClusters, numTrials, jointCounts, clusterCounts, true); // equal priors = true
//        Console.WriteLine("Clustering complete");

//        Console.WriteLine("\nClustering in internal form is:\n");
//        for (int i = 0; i < clustering.Length; ++i)
//          Console.Write(clustering[i] + " ");
//        Console.WriteLine("\n");

//        Console.WriteLine("Raw data displayed in clusters is:\n");
//        DisplayClustered(tuplesAsInt, numClusters, clustering, attValues);

//        //Console.WriteLine("Refining clustering (using non equal priors)");
//        //Refine(20, tuplesAsInt, clustering, jointCounts, clusterCounts, false);  // 20 attempts. equal priors is false use computed P(ci)s based on counts)

//        //Console.WriteLine("\nRefined clustering in internal form is:\n");
//        //for (int i = 0; i < clustering.Length; ++i)
//        //  Console.Write(clustering[i] + " ");
//        //Console.WriteLine("\n");

//        //Console.WriteLine("Raw data displayed in (refined) clusters is:\n");
//        //DisplayClustered(tuplesAsInt, numClusters, clustering, attValues);

//        Console.WriteLine("\nEnd demo\n");
//        Console.ReadLine();
//      }
//      catch (Exception ex)
//      {
//        Console.WriteLine(ex.Message);
//        Console.ReadLine();
//      }
//    } // Main

//    static int[] Cluster(int[][] tuplesAsInt, int numClusters, int numTrials, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
//    {
//      // return a good clustering
//      //
//      // get numClusters good indexes, use to assign one tuple to each cluster
//      // loop each tuple
//      //  if tuple has already been assigned to a cluster then continue
//      //  compute probs that tuple belongs to each cluster
//      //  determine cluster k' that has greatest probability
//      //  assign tuple to k'
//      // end each tuple

//      // consider computing and returning (as an out parameter) the average of the greatest probabilities as a confidence metric
//      // this would allow you to call Cluster multiple times and use the clustering that gave the greatest confidence value

//      // create clustering array
//      int numRows = tuplesAsInt.Length;  // number of tuples
//      int[] clustering = new int[numRows];  // the result is a clustering. one cell per tuple, value in cell is the cluster (-1 for unassigned)
//      for (int i = 0; i < clustering.Length; ++i)
//        clustering[i] = -1;

//      // seed clustering[] with 'good' indexes
//      int[] goodIndexes = GetGoodIndexes(tuplesAsInt, numClusters, numTrials);  // get indexes of tuples that are different from each other (numTrials attempts)

//      Console.Write("Seed indexes = ");
//      for (int i = 0; i < goodIndexes.Length; ++i)
//        Console.Write(goodIndexes[i] + " ");
//      Console.WriteLine("");

//      for (int i = 0; i < goodIndexes.Length; ++i)  // assign seed tuples to clusters
//      {
//        int idx = goodIndexes[i];
//        clustering[idx] = i;
//      }

//      // update joint counts
//      for (int i = 0; i < goodIndexes.Length; ++i)  // i also represents a cluster
//      {
//        int idx = goodIndexes[i];
//        for (int j = 0; j < tuplesAsInt[idx].Length; ++j)
//        {
//          int v = tuplesAsInt[idx][j];
//          ++jointCounts[j][v][i];     // very tricky indexing; need diagrams
//        }
//      }

//      // update cluster counts
//      for (int i = 0; i < clusterCounts.Length; ++i)
//        ++clusterCounts[i];

//      // main loop
//      // here we always iterate from [0], in order. this gives more weight to early tuples. consider walking thru tuples in random or scrambled order
//      for (int i = 0; i < tuplesAsInt.Length; ++i)
//      {
//        if (clustering[i] != -1) continue;  // if value (a cluster id) is not -1 (like 0 or 2) then the tuple has been assigned so skip

//        double[] allProbabilities = AllProbabilities(tuplesAsInt[i], jointCounts, clusterCounts, equalPriors);  // probs that curr tuple belongs to each possible cluster. equal priors
//        int c = IndexOfLargestProbability(allProbabilities);  // the cluster that has greatest probability

//        clustering[i] = c;  // assign tuple i to cluster c

//        // update joint counts
//        for (int j = 0; j < tuplesAsInt[i].Length; ++j)
//        {
//          int v = tuplesAsInt[i][j];
//          ++jointCounts[j][v][c];     // very tricky; need diagrams
//        }

//        // update clusterCounts
//        ++clusterCounts[c];

//        // validate
//        //Validate(jointCounts, clustering, clusterCounts, numClusters);

//      } // main loop

//      return clustering;
//    } // Cluster

//    static void DisplayClustered(int[][] tuplesAsInt, int numClusters, int[] clustering, string[][] attValues)
//    {
//      // show the data as strings, clustered
//      Console.WriteLine("--------------------------------------");
//      for (int k = 0; k < numClusters; ++k) // display by cluster
//      {
//        for (int i = 0; i < tuplesAsInt.Length; ++i)
//        {
//          if (clustering[i] == k)
//          {
//            for (int j = 0; j < tuplesAsInt[i].Length; ++j)
//            {
//              int v = tuplesAsInt[i][j];
//              string s = attValues[j][v];
//              Console.Write(s.ToString().PadRight(12) + " ");
//            }
//            Console.WriteLine("");
//          }
//        }
//        Console.WriteLine("--------------------------------------");
//      } // k
//    }

//    static void Refine(int numTrials, int[][] tuplesAsInt, int[] clustering, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
//    {
//      // after one pass of Cluster, call this method to try and improve the clustering
//      // select a random tuple, unassign it from its cluster, reassign (numTrials times)
//      for (int n = 0; n < numTrials; ++n)
//      {
//        int randomTupleIndex = GetTupleIndex(tuplesAsInt, clustering, clusterCounts);
//        if (randomTupleIndex == -1)
//          throw new Exception("Failed to get valid randomTupleIndex in Refine");

//        Unassign(randomTupleIndex, clustering, tuplesAsInt, jointCounts, clusterCounts);

//        double[] allProbabilities = AllProbabilities(tuplesAsInt[randomTupleIndex], jointCounts, clusterCounts, equalPriors);  // probs that tuple at randomTupleIndex belongs to each possible cluster. use priors?
//        int c = IndexOfLargestProbability(allProbabilities);  // the cluster that has greatest probability

//        clustering[randomTupleIndex] = c;  // assign tuple i to cluster c

//        // update joint counts
//        for (int j = 0; j < tuplesAsInt[randomTupleIndex].Length; ++j)
//        {
//          int v = tuplesAsInt[randomTupleIndex][j];
//          ++jointCounts[j][v][c];     // very tricky; need diagrams
//        }

//        // update clusterCounts
//        ++clusterCounts[c];
//      }
//    }

//    static int GetTupleIndex(int[][] tuplesAsInt, int[] clustering, int[] clusterCounts)
//    {
//      // helper for Refine
//      // get the index of a random tuple, where the tuple is not the only member of a cluster
//      // or, in other words, index of a tuple that is part of a cluster with 2 or more tuples.
//      int sanityCount = 0;
//      while (sanityCount < 10000)
//      {
//        ++sanityCount;
//        int ri = random.Next(0, tuplesAsInt.Length); // a candidate index of a tuple
//        int c = clustering[ri];                      // cluster that the tuple is assigned to
//        if (clusterCounts[c] > 1)                    // the cluster has 2 or more tuples assigned
//          return ri;
//      }
//      return -1; //error
//    }

//    static void Unassign(int tupleIndex, int[] clustering, int[][] tuplesAsInt, int[][][] jointCounts, int[] clusterCounts)
//    {
//      // helper for refine
//      // remove a tuple from clustering by modifying clustering[], jointCounts[][][], and clusterCounts[]
//      int c = clustering[tupleIndex];  // curr tuple that tupleIndex is assigned to
//      clustering[tupleIndex] = -1;     // unassign

//      for (int i = 0; i < tuplesAsInt[tupleIndex].Length; ++i)  // i is an attribute (or a tupleAsInt col)
//      {
//        int v = tuplesAsInt[tupleIndex][i];  // v is an attribute value
//        --jointCounts[i][v][c];              // undo the joint count
//      }

//      --clusterCounts[c];  // undo the cluster count
//    }

//    // -----------------------------------------------------------------------------------------------------------------------------------

//    static int IndexOfLargestProbability(double[] allProbabilities)
//    {
//      double largestP = 0.0;
//      int idx = 0;

//      for (int i = 0; i < allProbabilities.Length; ++i)
//      {
//        if (allProbabilities[i] > largestP)
//        {
//          largestP = allProbabilities[i]; idx = i;
//        }
//      }
//      return idx;
//    }

//    static double[] AllProbabilities(int[] tupleAsInt, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
//    {
//      // returns an array of probs that the tuple belongs to each cluster
//      // calls AllPartialProbs which in turn calls PartialProbability
//      // the prob that a tuple belongs to some cluster c is the partial for c / sum of partials for all clusters
//      // equalPriors is passed along. if true, all P(ci) are equal. if false P(ci) = count(ci) / total count.
//      int numClusters = clusterCounts.Length;
//      double[] result = new double[numClusters];

//      double[] partialProbs = AllPartialProbs(tupleAsInt, jointCounts, clusterCounts, equalPriors);  // compute and store all partials
//      double sumOfPartials = 0.0;
//      for (int i = 0; i < partialProbs.Length; ++i)
//        sumOfPartials += partialProbs[i];

//      for (int i = 0; i < result.Length; ++i)
//        result[i] = partialProbs[i] / sumOfPartials;

//      //Console.WriteLine("All probs = " + result[0].ToString("F4") + " " + result[1].ToString("F4") + " " + result[2].ToString("F4"));
//      //Console.ReadLine();

//      return result;
//    }

//    static double[] AllPartialProbs(int[] tupleAsInt, int[][][] jointCounts, int[] clusterCounts, bool equalPriors)
//    {
//      // compute Partial Probabilities of tupleAsInt for all clusters
//      int numClusters = clusterCounts.Length;
//      double[] result = new double[numClusters]; // one PartialProbability for each cluster
//      for (int c = 0; c < result.Length; ++c)
//      {
//        double p = PartialProbability(tupleAsInt, jointCounts, clusterCounts, c, equalPriors);  // compute 
//        result[c] = p;  // store
//      }
//      //Console.WriteLine("Partials probs = " + result[0].ToString("F4") + " " + result[1].ToString("F4") + " " + result[2].ToString("F4"));
//      //Console.ReadLine();
//      return result;
//    }

//    static double PartialProbability(int[] tupleAsInt, int[][][] jointCounts, int[] clusterCounts, int c, bool equalPriors)
//    {
//      // returns the partial probability that a tuple with int values in tupleAsInt belongs to cluster c (by using the values in jointCounts and clusterCounts)
//      // ex. Suppose tupleAsInt is (1,2,0) == (suburban,high,liberal) and suppose c = 1 (assume republican if hidden labels are  democrat(0) or republican(1) or independent(2))
//      // we want the PP that the tuple belongs to c1 which is
//      // P(c1 | suburban) * P(c1 | high) * P(c1 | liberal) * P(c1)  which is IN THEORY
//      // count(c1 & suburban) / count(c1)  *  count(c1 & high) / count(c1)  *  count(c1 & liberal) / count(c1)  *  [ count(c1) / [count(c0) + count(c1) + count(c2)] ]
//      // but with Laplacian smoothing (all joint counts have 1 added to prevent a zero-out) and the calculation is:
//      // count(c1 & suburban) / count(c1)+3  *  count(c1 & high) / count(c1)+3  *  count(c1 & liberal) / count(c1)+3  *  [ count(c1) / [count(c0) + count(c1) + count(c2)] ], where the 3 is numClusters
//      // if equal priors is true, the last term becomes just 1 / numclusters (e.g., 1/3) but since all the same the term just drops out.

//      int totalCount = 0;  // count(c0) + count(c1) + count(c2) 
//      for (int i = 0; i < clusterCounts.Length; ++i)
//        totalCount += clusterCounts[i];  // total number of tuples that have been assigned to a cluster

//      int numClusters = clusterCounts.Length;
//      int clusterCount = clusterCounts[c] + numClusters;  // number of tuples assigned to cluster c plus the Laplacian factor. ex: count(c1) alone plus 3

//      double[] probs = new double[jointCounts.Length + 1]; // there is one prob for each attribute (location,income,political) plus 1 for the cluster prob
//      for (int i = 0; i < probs.Length - 1; ++i)  // (all except the last prob) -- the joint probabilities
//      {
//        int j = tupleAsInt[i];  // i is each attribute type
//        double p = jointCounts[i][j][c] / (1.0 * clusterCount);  // tricky. need a diagram to understand this.
//        //Console.WriteLine("partial " + i + " = " + jointCounts[i][j][c] + " over " + clusterCount);
//        //Console.ReadLine();
//        probs[i] = p;
//      }

//      //Console.WriteLine("last partial = " + clusterCounts[c] + " over " + totalCount);
//      //Console.ReadLine();

//      // P(c). take care of the last prob.
//      double clusterP = 0.0;
//      if (equalPriors == true)
//        clusterP = 1.0 / numClusters;
//      else
//        clusterP = clusterCounts[c] / (1.0 * totalCount); 
  
//      probs[probs.Length - 1] = clusterP;

//      double result = 1.0; // now multiply the probs together

//      for (int i = 0; i < probs.Length; ++i)
//        result *= probs[i];

//      return result;
//    } // PartialProbability

//    // -----------------------------------------------------------------------------------------------------------------------------------

//    static int[][][] InitJointCounts(int[][] tuplesAsInt, string[][] attValues, int numClusters)
//    {
//      // allocate joint counts and set all cells to 1 for Laplacian smoothing (a joint count of 0 is bad)
//      // [attribute][attValue][cluster]. ex: [1][0][2] holds the count of income(1) equal to lown (0) AND cluster c(2)

//      int[][][] result = new int[attValues.Length][][]; // allocate
//      for (int i = 0; i < result.Length; ++i)
//      {
//        int numCells = attValues[i].Length;
//        result[i] = new int[numCells][];
//      }

//      for (int i = 0; i < result.Length; ++i)
//      {
//        for (int j = 0; j < result[i].Length; ++j)
//        {
//          result[i][j] = new int[numClusters];
//        }
//      }

//      for (int i = 0; i < result.Length; ++i)         // assign -1 to each cell
//        for (int j = 0; j < result[i].Length; ++j)
//          for (int k = 0; k < result[i][j].Length; ++k)
//            result[i][j][k] = 1;

//      return result;

//    } // InitJointCounts

//    static void ShowJointCounts(int[][][] jointCounts)
//    {
//      for (int i = 0; i < jointCounts.Length; ++i)
//      {
//        for (int j = 0; j < jointCounts[i].Length; ++j)
//        {
//          for (int k = 0; k < jointCounts[i][j].Length; ++k)
//          {
//            Console.Write(jointCounts[i][j][k] + " ");
//          }
//          Console.WriteLine("");
//        }
//        Console.WriteLine("");
//      }
//    }


//    // ------------------------------------------------------------------------------------------------------------------

//    static int Delta(int[][] tuplesAsInt, int a, int b)
//    {
//      // helper for GetRandomIndexes
//      // number of positions where tuplesAsInt[a] and tuplesAsInt[b] differ
//      int numCols = tuplesAsInt[0].Length;
//      int ct = 0;
//      for (int j = 0; j < numCols; ++j)
//        if (tuplesAsInt[a][j] != tuplesAsInt[b][j])
//          ++ct;
//      return ct;
//    }

//    static int TotalDelta(int[][] tuplesAsInt, int[] indexes)
//    {
//      // helper for GetRandomIndexes
//      // total number of positions where tuples specified in indexes[] differ, computed a pair at a time
//      int numCols = tuplesAsInt[0].Length;
//      int ct = 0;
//      for (int i = 0; i < indexes.Length - 1; ++i)
//      {
//        for (int j = i + 1; j < indexes.Length; ++j)
//        {
//          int a = indexes[i];
//          int b = indexes[j];
//          ct += Delta(tuplesAsInt, a, b);
//        }
//      }
//      return ct;
//    }

//    static int[] GetRandomIndexes(int n, int maxIndex)
//    {
//      // get n distinct indexes between [0,maxIndex) // [inclusive,exclusive)
//      // assumes a global (class-scope) random object exists
//      Dictionary<int, bool> d = new Dictionary<int, bool>();  // key is an index, bool is true if already used (dummy)

//      int[] result = new int[n];
//      int ct = 0;
//      int sanityCount = 0;  // to prevent an infinite loop
//      while (ct < n && sanityCount < 10000)
//      {
//        ++sanityCount;
//        int idx = random.Next(0, maxIndex);
//        if (d.ContainsKey(idx) == false)
//        {
//          result[ct] = idx;
//          d[idx] = true;  // dummy
//          ++ct;
//        }
//      }
//      return result;
//    } // GetRandomIndexes

//    static int[] GetGoodIndexes(int[][] tuplesAsInt, int n, int numTrials)
//    {
//      // calls GetRandomIndexes
//      // get n 'good' indexes -- where the total delta for all tuples of the indexes is high (we want tuples that are different)
//      int[] result = new int[n];
//      int numRows = tuplesAsInt.Length;
//      int highestDelta = 0;

//      for (int i = 0; i < numTrials; ++i)
//      {
//        int[] randomIndexes = GetRandomIndexes(n, numRows);
//        int totalDelta = TotalDelta(tuplesAsInt, randomIndexes);
//        if (totalDelta > highestDelta)
//        {
//          highestDelta = totalDelta;
//          Array.Copy(randomIndexes, result, result.Length);
//        }
//      }
//      return result;
//    }

//    // ------------------------------------------------------------------------------------------------------------------

//    static string[] MakeData(int numRows)
//    {
//      string[] result = new string[numRows];
//      for (int i = 0; i < numRows; ++i)
//      {
//        string party = MakeParty();  // hidden label is a political party affiliation
//        string location = MakeLocation(party);  // urban, suburban, rural
//        string income = MakeIncome(party);      // low, medium, high, veryhigh
//        string political = MakePolitical(party);  // liberal, conservative
//        string s = location + "," + income + "," + political;
//        result[i] = s;
//      }
//      return result;
//    } // MakeData

//    static void ShowData(string[] rawData, int top)
//    {
//      for (int i = 0; i < top; ++i)
//        Console.WriteLine(rawData[i]);
//    }

//    static string MakeParty() // hidden label
//    {
//      int r = random.Next(0, 100); // [0,100) => [0,99]
//      if (r >= 0 && r <= 9)        // 10% independent
//        return "independent";
//      else if (r >= 10 && r <= 59) // 50% democart
//        return "democrat";
//      else
//        return "republican";       // 40% republican
//    }

//    static string MakeLocation(string party) // where a person lives 
//    {
//      int r = random.Next(0, 100); // [0,100) => [0,99]
//      if (party == "independent")
//      {
//        if (r >= 0 && r <= 19) return "Urban";  // 20%
//        else if (r >= 20 && r <= 49) return "Suburban"; // 30%
//        else return "Rural"; // 50%
//      }
//      else if (party == "democrat")
//      {
//        if (r >= 0 && r <= 69) return "Urban";  // 70%
//        else if (r >= 70 && r <= 89) return "Suburban"; // 20%
//        else return "Rural";  // 10%
//      }
//      else // republican
//      {
//        if (r >= 0 && r <= 19) return "Urban";  // 20%
//        else if (r >= 20 && r <= 69) return "Suburban"; // 50%
//        else return "Rural";  // 30%
//      }
//    } // MakeLocation

//    static string MakeIncome(string party)
//    {
//      int r = random.Next(0, 100); // [0,100) => [0,99]
//      if (party == "independent")
//      {
//        if (r >= 0 && r <= 19) return "Low";          // 20%
//        else if (r >= 20 && r <= 49) return "Medium"; // 30%
//        else if (r >= 50 && r <= 79) return "High";   // 30%
//        else return "VeryHigh";                       // 20%
//      }
//      else if (party == "democrat")
//      {
//        if (r >= 0 && r <= 59) return "Low";          // 60%
//        else if (r >= 60 && r <= 79) return "Medium"; // 20%
//        else if (r >= 80 && r <= 89) return "High";   // 10%
//        else return "VeryHigh";                       // 10%
//      }
//      else // republican
//      {
//        if (r >= 0 && r <= 39) return "Low";          // 40%
//        else if (r >= 40 && r <= 59) return "Medium"; // 10%
//        else if (r >= 60 && r <= 69) return "High";   // 10%
//        else return "VeryHigh";                       // 40%
//      }
//    } // MakeIncome

//    static string MakePolitical(string party)
//    {
//      int r = random.Next(0, 100); // [0,100) => [0,99]
//      if (party == "independent")
//      {
//        if (r >= 0 && r <= 49) return "Conservative";   // 50%
//        else return "Liberal";                          // 50%
//      }
//      else if (party == "democrat")
//      {
//        if (r >= 0 && r <= 19) return "Conservative";   // 20%
//        else return "Liberal";                          // 80%
//      }
//      else // republican
//      {
//        if (r >= 0 && r <= 69) return "Conservative";   // 70%
//        else return "Liberal";                          // 30%
//      }
//    } // MakePolitical

//    static string[][] LoadData(string[] rawData, string[][] attValues)
//    {
//      // iterate thru raw data, parse on commas, load into memory
//      int numRows = rawData.Length;
//      int numCols = attValues.Length;

//      string[][] result = new string[numRows][]; // allocate
//      for (int i = 0; i < numRows; ++i)
//        result[i] = new string[numCols];

//      // read, parse, store
//      for (int i = 0; i < rawData.Length; ++i)
//      {
//        string[] tokens = rawData[i].Split(',');
//        for (int j = 0; j < tokens.Length; ++j)
//          result[i][j] = tokens[j];
//      }

//      return result;
//    } // LoadData

//    static void ShowData(string[][] tuples, int top)
//    {
//      for (int i = 0; i < top; ++i)
//      {
//        for (int j = 0; j < tuples[i].Length; ++j)
//        {
//          Console.Write(tuples[i][j] + " ");
//        }
//        Console.WriteLine("");
//      }
//    } // ShowData

//    static void ShowData(int[][] tuples, int top)
//    {
//      for (int i = 0; i < top; ++i)
//      {
//        for (int j = 0; j < tuples[i].Length; ++j)
//        {
//          Console.Write(tuples[i][j] + " ");
//        }
//        Console.WriteLine("");
//      }
//    } // ShowData

//    static int[][] TuplesToInts(string[][] tuples, string[][] attValues) // converts a matrix of tuples in string form to int form
//    {
//      // create an array of lookup table thing. [0] = attribute (e.g., location), key = "rural", then value = 2
//      Dictionary<string, int>[] lookup = new Dictionary<string, int>[attValues.Length];  // one dictionary for each attribute
//      for (int i = 0; i < attValues.Length; ++i)  // each attribute
//      {
//        lookup[i] = new Dictionary<string, int>();
//        for (int j = 0; j < attValues[i].Length; ++j) // each value of curr attribute
//          lookup[i].Add(attValues[i][j], j); //
//      }
//      // scan tuples and convert using the lookup
//      int numRows = tuples.Length;
//      int numCols = attValues.Length;
//      int[][] result = new int[numRows][]; // allocate
//      for (int i = 0; i < numRows; ++i)
//        result[i] = new int[numCols];

//      for (int i = 0; i < numRows; ++i) // each row/tuple
//      {
//        for (int j = 0; j < numCols; ++j) // each col/attribute
//        {
//          string v = tuples[i][j]; // eg, "rural"
//          int attAsInt = lookup[j][v]; // then 2. j is the tuple column which is also the attribute
//          result[i][j] = attAsInt;
//        }
//      }
//      return result;
//    } // TuplesToInts

//  } // class ClusteringBayesianProgram
//} // ns