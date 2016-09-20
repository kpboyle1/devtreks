using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Run scripting language algorithms
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA examples 2 and 3
    ///</summary>
    public class Script1 : Calculator1
    {

        public Script1(string[] mathTerms, string[] colNames, string[] depColNames,
            double[] qs, string algorithm, string subAlgorithm,
            CalculatorParameters calcParams)
            : base()
        {
            _colNames = colNames;
            _depColNames = depColNames;
            _mathTerms = mathTerms;
            _algorithm = algorithm;
            _subalgorithm = subAlgorithm;
            //estimators
            //add an intercept to qs 
            _qs = new double[qs.Count() + 1];
            //1 * b0 = b0
            _qs[0] = 1;
            qs.CopyTo(_qs, 1);
            _params = calcParams;
        }
        private CalculatorParameters _params { get; set; }
        //all of the the dependent and independent var column names
        private string[] _colNames { get; set; }
        //all of the the dependent var column names including intercept
        private string[] _depColNames { get; set; }
        //corresponding Ix.Qx names (1 less count because no dependent var)
        private string[] _mathTerms { get; set; }
        //corresponding Qx amounts
        private double[] _qs { get; set; }

        private string _algorithm { get; set; }
        private string _subalgorithm { get; set; }

        //output
        public double QTSlope { get; set; }
        //qTM = predicted y
        public double QTPredicted { get; set; }
        //lower ci
        public double QTL { get; set; }
        public string QTLUnit { get; set; }
        //upper ci
        public double QTU { get; set; }
        public string QTUUnit { get; set; }

        //running this truly async returns to UI w/o saving final calcs or an endless wait
        public async Task<bool> RunAlgorithmAsync(string inputFilePath, string scriptFilePath, 
            System.Threading.CancellationToken ctk)
        {
            bool bHasCalcs = false;
            try
            {
                this.ErrorMessage =string.Empty;
                if (string.IsNullOrEmpty(inputFilePath) || (!inputFilePath.EndsWith(".csv")))
                {
                    this.ErrorMessage ="The dataset file URL has not been added to the Data URL. The file must be stored in a Resource and use a csv file extension.";
                }
                if (string.IsNullOrEmpty(scriptFilePath) || (!scriptFilePath.EndsWith(".txt")))
                {
                    this.ErrorMessage += "The script file URL has not been added to the Joint Data.The file must be stored in a Resource and use a txt file extension.";
                }
                //unblock after debug
                if (!string.IsNullOrEmpty(this.ErrorMessage))
                {
                    return bHasCalcs;
                }
                string sError = string.Empty;

                StringBuilder sb = new StringBuilder();
                //get the path to the script executable
                string sScriptExecutable = CalculatorHelpers.GetAppSettingString(
                    this._params.ExtensionDocToCalcURI, "RExecutable");
                if (_algorithm == Calculator1.MATH_TYPES.algorithm3.ToString())
                {
                    //python must be installed to automatically run 
                    sScriptExecutable = CalculatorHelpers.GetAppSettingString(
                        this._params.ExtensionDocToCalcURI, "PyExecutable");
                    //python scripts must be run by executable as '.pyw' files
                    //save the 'txt' file as a 'pyw' file in temp path
                    //has to be done each time because can't be sure when scriptfile changed last
                    if (!scriptFilePath.EndsWith(".pyw"))
                    {
                        string sPyScript = CalculatorHelpers.ReadText(_params.ExtensionDocToCalcURI, scriptFilePath, out sError);
                        if (!string.IsNullOrEmpty(sPyScript))
                        {
                            string sFileName = Path.GetFileName(scriptFilePath);
                            string sPyScriptFileName = sFileName.Replace(".txt", ".pyw");
                            bool bIsLocalCache = false;
                            string sPyPath = CalculatorHelpers.GetTempDocsPath(_params.ExtensionDocToCalcURI, bIsLocalCache, sPyScriptFileName);
                            bool bHasFile = CalculatorHelpers.SaveTextInURI(_params.ExtensionDocToCalcURI, sPyScript, sPyPath, out sError);
                            scriptFilePath = sPyPath;
                        }
                    }
                    sb.AppendLine("python results");
                }
                else 
                {
                    //check for 2.0.2 -R Open can run from a url
                    //rscript.exe can't run from a url 
                    if (scriptFilePath.StartsWith("http"))
                    {
                        //convert it to a filesystem path
                        //make sure that both localhost and localhost:44300 have a copy of the file 
                        string sRFilePath = CalculatorHelpers.ConvertFullURIToFilePath(
                             this._params.ExtensionDocToCalcURI, scriptFilePath);
                        scriptFilePath = sRFilePath;
                    }
                    //r is default
                    sb.AppendLine("r results");
                }
                string sLastLine = string.Empty;
                //subalgo2 is r subalgo3 is Python
                if (_subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm2.ToString()
                    || _subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm3.ToString())
                {
                    inputFilePath = @"C:\DevTreks\src\DevTreks\wwwroot\resources\network_carbon\resourcepack_526\resource_1771\Regress1.csv";
                    scriptFilePath = @"C:\DevTreks\src\DevTreks\wwwroot\resources\network_carbon\resourcepack_526\resource_1765\R1Web.txt";
                    string sOutputURL = @"C:\DevTreks\src\DevTreks\wwwroot\resources\temp\2146500643\out1.csv";
                    bool bHasCsvFile = await CalculatorHelpers.RunStatScriptAPIClient(
                        sb, scriptFilePath, inputFilePath, sOutputURL);
                    if (bHasCsvFile)
                    {
                        //process the sOutputURL csv file

                    }
                }
                else
                {
                    sLastLine = RunScript(sb, sScriptExecutable, scriptFilePath, inputFilePath);
                }
                if (this.MathResult.ToLower().StartsWith("http"))
                {
                    sError = string.Empty;
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
                bHasCalcs = true;
                //last line of string should have the QTM vars
                if (!string.IsNullOrEmpty(sLastLine))
                {
                    string[] vars = sLastLine.Split(Constants.CSV_DELIMITERS);
                    bool bHasVars = false;
                    if (vars != null)
                    {
                        if (vars.Count() > 1)
                        {
                            bHasVars = true;
                        }
                        if (!bHasVars)
                        {
                            //try space delimited
                            vars = sLastLine.Split(' ');
                            bHasVars = true;
                        }
                        if (vars != null)
                        {
                            //row count may be in first pos
                            int iPos = vars.Count() - 3;
                            if (vars[iPos] != null)
                                this.QTPredicted = CalculatorHelpers.ConvertStringToDouble(vars[iPos]);
                            iPos = vars.Count() - 2;
                            if (vars[iPos] != null)
                                this.QTL = CalculatorHelpers.ConvertStringToDouble(vars[iPos]);
                            iPos = vars.Count() - 1;
                            if (vars[iPos] != null)
                                this.QTU = CalculatorHelpers.ConvertStringToDouble(vars[iPos]);
                        }
                    }
                }
                else
                {
                    this.MathResult = "The script did not run successfully. Please check the dataset and script. Verify their urls.";
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage += ex.Message;
            }
            return bHasCalcs;
        }
        private string RunScript(StringBuilder sb, string scriptExecutable, 
            string scriptFilePath, string inputFilePath)
        {
            //run the excecutable as a console app
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = scriptExecutable;
            start.RedirectStandardOutput = true;
            start.UseShellExecute = false;
            start.Arguments = string.Format("{0} {1}", scriptFilePath, inputFilePath);
            start.CreateNoWindow = true;
            string sLastLine = string.Empty;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        sLastLine = reader.ReadLine();
                        sb.AppendLine(sLastLine);
                    }

                    //sb.Append(reader.ReadToEnd());
                    //retain for writing to file
                    //using (StreamWriter sw = new StreamWriter(output3))
                    //{
                    //    sb.Append(reader.ReadToEnd());
                    //    sw.Write(sb.ToString());
                    //}
                }
                process.WaitForExit();
            }
            return sLastLine;
        }
    }
}
