using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Run scripting language algorithms
    ///Author:		www.devtreks.org
    ///Date:		2016, November
    ///References:	CTA 1, 2, and 3 references
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
                    if (_subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm1.ToString())
                    {
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
                    }
                    sb.AppendLine("python results");
                }
                else 
                {
                    if (_subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm1.ToString())
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
                    }
                    //r is default
                    sb.AppendLine("r results");
                }
                string sLastLine = string.Empty;
                //2.0.2: algo 2 subalgo2 is r or algo 3 subalgo2 Python; subalgo 2 is virtual machine
                if (_subalgorithm == Calculator1.MATH_SUBTYPES.subalgorithm2.ToString())
                {
                   
                    //run on remote servers that have the DevTreksStatsApi WebApi app deployed
                    string sStatType = Data.Helpers.StatScript.STAT_TYPE.r.ToString();
                    if (_algorithm == Calculator1.MATH_TYPES.algorithm3.ToString())
                    {
                        //python is significantly slower than R
                        sStatType = Data.Helpers.StatScript.STAT_TYPE.py.ToString();
                    }
                    else if (_algorithm == Calculator1.MATH_TYPES.algorithm6.ToString())
                    {
                        //julia has not been tested in 2.0.2
                        sStatType = Data.Helpers.StatScript.STAT_TYPE.julia.ToString();
                    }

                    DevTreks.Data.Helpers.StatScript statScript
                       = DevTreks.Data.Helpers.StatScript.GetStatScript(
                            sStatType, scriptFilePath, inputFilePath);
                    string sPlatformType = CalculatorHelpers.GetAppSettingString(
                        this._params.ExtensionDocToCalcURI, "PlatformType");
                    if (sPlatformType.Contains("azure"))
                    {
                        //webapi web domain
                        if (inputFilePath.Contains("localhost"))
                        {
                            statScript.DefaultWebDomain = "http://localhost:5000/";
                        }
                        else
                        {
                            statScript.DefaultWebDomain = "http://devtreksapi1.southcentralus.cloudapp.azure.com/";
                        }
                    }
                    else
                    {
                        if (inputFilePath.Contains("localhost"))
                        {
                            statScript.DefaultWebDomain = "http://localhost:5000/";
                        }
                        else
                        {
                            //run tests on cloud webapi site too
                            statScript.DefaultWebDomain = "http://devtreksapi1.southcentralus.cloudapp.azure.com/";
                        }
                    }
                    //use a console app to post to a webapi CreateClient controller action
                    bool bIsSuccess = await CalculatorHelpers.ClientCreate(statScript);
                    if (bIsSuccess)
                    {
                        if ((!string.IsNullOrEmpty(statScript.StatisticalResult)))
                        {
                            List<string> lines = CalculatorHelpers
                                .GetLinesFromUTF8Encoding(statScript.StatisticalResult);
                            if (lines != null)
                            {
                                if (lines.Count > 0)
                                {
                                    //store the result in the MathResult (or in the MathResult.URL below)
                                    sb.Append(statScript.StatisticalResult);
                                    sLastLine = lines.Last();
                                    if (string.IsNullOrEmpty(sLastLine))
                                    {
                                        int iSecondToLast = lines.Count - 2;
                                        sLastLine = lines[iSecondToLast];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(statScript.StatisticalResult)))
                            {
                                this.MathResult += statScript.ErrorMessage;
                            }
                            else
                            {
                                this.MathResult += "The remote server returned a successful response header but failed to generate the statistical results.";
                            }
                        }
                    }
                }
                else
                {
                    //default subalgo1 runs statpackages on the same server
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
                }
                process.WaitForExit();
            }
            return sLastLine;
        }
    }
}
