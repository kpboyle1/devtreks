using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Algorithm for running R scripts
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA example 5
    ///</summary>
    public class RProject2 : Calculator1
    {

        public RProject2(string[] mathTerms, string[] colNames, string[] depColNames,
            double[] qs, int iterations, CalculatorParameters calcParams)
            : base()
        {
            _colNames = colNames;
            _depColNames = depColNames;
            _mathTerms = mathTerms;
            _iterations = iterations;
            if (_iterations <= 0)
            {
                _iterations = 1000;
            }
            //estimators
            //add an intercept to qs 
            _qs = new double[qs.Count() + 1];
            //1 * b0 = b0
            _qs[0] = 1;
            qs.CopyTo(_qs, 1);
            //init calculated response 
             _response = string.Empty;
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

        private int _iterations { get; set; }
        private string _response { get; set; }

        //output
        //q6 = marginal productivity
        public double QTSlope { get; set; }
        //q6M = predicted y
        public double QTPredicted { get; set; }
        //lower 95% ci
        public double QTL { get; set; }
        public string QTLUnit { get; set; }
        //upper 95% ci
        public double QTU { get; set; }
        public string QTUUnit { get; set; }
        
        //running this truly async returns to UI w/o saving final calcs or an endless wait
        //requires that an r dataset file be uploaded to blob resource prior to this
        //and referenced using jdataurl
        public async Task<bool> RunAlgorithmAsync(string inputFilePath, System.Threading.CancellationToken ctk)
        {
            bool bHasCalcs = false;
            try
            {
                ///label, date, learningstep, population, q6
                if (string.IsNullOrEmpty(inputFilePath))
                {
                    this.ErrorMessage ="The R project dataset has not been uploaded.";
                }
                string sError = string.Empty;
                string sBaseURL =
                    "https://ussouthcentral.services.azureml.net/workspaces/d454361ecdcb4ec4b03fa1aec5a7c0e2/services/9f23e23f1a724d408126276db8b6017f/jobs";
                
                string sApiKey =
                    "4xs+lIxGYOLEM5u/SrDpSg7CmrWDOIJZzL+dm2igKGfzqToyWTafpSM0v+0J74ZqF1hxsgD7TgJAtYTChOhMHQ==";
                //web server expects to store in temp/randomid/name.csv
                //web service stores in temp blob
                string sOutputDataURL = CalculatorHelpers.GetTempContainerPath("Routput.csv");
                
                //web service expects urls that start with container names
                //regular rproject file must be stored in JDataURL
                string sInputContainerPath = CalculatorHelpers.GetContainerPathFromFullURIPath("resources", inputFilePath);
                //async wait so that results can be stored in output file location and parsed into string lines
                SetResponse(sBaseURL, sApiKey, sInputContainerPath, sOutputDataURL).Wait();
                StringBuilder sb = new StringBuilder();
                //if web service successully saved the results, the response will start with Success
                if (_response.StartsWith("Success"))
                {
                    //return the output file contents in a string list of lines
                    //must convert container path to full path
                    string sOutputFullDataURL = string.Concat("https://devtreks1.blob.core.windows.net/", sOutputDataURL);
                    List<string> lines = new List<string>();
                    //azure emulator can't process real Azure URL so this won't work
                    //instead, double check that output url is actually saved
                    lines = CalculatorHelpers.ReadLines(_params.ExtensionDocToCalcURI, sOutputFullDataURL, out sError);
                    this.ErrorMessage += sError;
                    //this results in endless wait
                    //lines = await CalculatorHelpers.ReadLinesAsync(sOutputDataURL);
                    if (lines == null)
                    {
                        this.ErrorMessage += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                    }
                    if (lines.Count == 0)
                    {
                        this.ErrorMessage += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                    }
                    double[] intercepts = new double[] { _depColNames.Count() };
                    sb = new StringBuilder();
                    sb.AppendLine("regression results");
                    //dep var has to be in the 4 column always
                    string sLine = string.Concat("dependent variable:  ", _colNames[3]);
                    string[] line = new List<string>().ToArray();
                    //string[] line2 = new List<string>().ToArray();
                    string[] lineout = new string[5];
                    int j = 0;
                    //how many lines of coefficients?
                    int ciCount = 5 + (_depColNames.Count());
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        if (i == 0)
                        {
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            lineout[0] = "R squared";
                            lineout[1] = CalculatorHelpers.ConvertStringToDouble(line[0]).ToString("N4", CultureInfo.InvariantCulture);
                            lineout[2] = "Adjusted R squared:";
                            lineout[3] = CalculatorHelpers.ConvertStringToDouble(line[1]).ToString("N4", CultureInfo.InvariantCulture);
                            lineout[4] = string.Empty;
                            sb.AppendLine(Shared.GetLine(lineout, false));
                        }
                        else if (i == 1)
                        {
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            lineout[0] = "F statistic:";
                            lineout[1] = CalculatorHelpers.ConvertStringToDouble(line[0]).ToString("N4", CultureInfo.InvariantCulture);

                        }
                        else if (i == 2)
                        {
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            lineout[2] = " with df ";
                            lineout[3] = line[0];
                        }
                        else if (i == 3)
                        {
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            lineout[4] = string.Concat(" and ", CalculatorHelpers.ConvertStringToDouble(line[0]).ToString("N4", CultureInfo.InvariantCulture));
                            sb.AppendLine(Shared.GetLine(lineout, false));
                        }
                        else if (i == 4)
                        {
                            lineout[0] = "coefficient";
                            lineout[1] = "estimate";
                            lineout[2] = "std error";
                            lineout[3] = "t value";
                            lineout[4] = "prob > T";
                            sb.AppendLine(Shared.GetLine(lineout, false));
                            //intercept
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            lineout[0] = "intercept";
                            lineout[1] = CalculatorHelpers.ConvertStringToDouble(line[0]).ToString("N4", CultureInfo.InvariantCulture);
                            //intercepts[j] = CalculatorHelpers.ConvertStringToDouble(line[0]);
                            lineout[2] = CalculatorHelpers.ConvertStringToDouble(line[1]).ToString("N4", CultureInfo.InvariantCulture);
                            lineout[3] = CalculatorHelpers.ConvertStringToDouble(line[2]).ToString("N4", CultureInfo.InvariantCulture);
                            lineout[4] = CalculatorHelpers.ConvertStringToDouble(line[3]).ToString("N4", CultureInfo.InvariantCulture);
                            sb.AppendLine(Shared.GetLine(lineout, false));
                        }
                        else if ((i > 4) && (i < ciCount))
                        {
                            //dep vars
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            if (_depColNames.Count() > (j - 1))
                            {
                                lineout[0] = _depColNames[j];
                                j++;
                            }
                            lineout[1] = CalculatorHelpers.ConvertStringToDouble(line[0]).ToString("N4", CultureInfo.InvariantCulture);
                            //intercepts[j] = CalculatorHelpers.ConvertStringToDouble(line[1]);
                            lineout[2] = CalculatorHelpers.ConvertStringToDouble(line[1]).ToString("N4", CultureInfo.InvariantCulture);
                            lineout[3] = CalculatorHelpers.ConvertStringToDouble(line[2]).ToString("N4", CultureInfo.InvariantCulture);
                            lineout[4] = CalculatorHelpers.ConvertStringToDouble(line[3]).ToString("N4", CultureInfo.InvariantCulture);
                            sb.AppendLine(Shared.GetLine(lineout, false));
                        }
                        else
                        {
                            //set ci from first observation
                            lineout[0] = "estimated QTM";
                            lineout[1] = "upper 95% ci";
                            lineout[2] = "lower 95% ci";
                            lineout[3] = "";
                            lineout[4] = "";
                            sb.AppendLine(Shared.GetLine(lineout, false));
                            line = lines[i].Split(Constants.CSV_DELIMITERS);
                            this.QTPredicted = CalculatorHelpers.ConvertStringToDouble(line[0]);
                            lineout[0] = this.QTPredicted.ToString("N4", CultureInfo.InvariantCulture);
                            this.QTL = CalculatorHelpers.ConvertStringToDouble(line[1]);
                            lineout[1] = this.QTL.ToString("N4", CultureInfo.InvariantCulture);
                            this.QTU = CalculatorHelpers.ConvertStringToDouble(line[2]);
                            lineout[2] = this.QTU.ToString();
                            lineout[4] = "";
                            sb.AppendLine(Shared.GetLine(lineout, false));
                            break;
                        }
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
                    //potential future use: convert to JSON object
                    //BatchResponseStructure responseStruct = 
                    //    JsonConvert.DeserializeObject<BatchResponseStructure>(_response);

                }
                else
                {
                    this.ErrorMessage ="The calculations could not be run using the web service.";
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage =ex.Message;
            };
            return bHasCalcs;
        }

        public async Task SetResponse(
            string baseURL, string apiKey,
            string inputBlobLocation, string outputBlobLocation)
        {
            //_response = await CalculatorHelpers.InvokeHttpRequestResponseServiceR(
            //baseURL, apiKey,
            //    inputBlobLocation, outputBlobLocation).ConfigureAwait(false);
        }

        public class BatchResult
        {
            public String ConnectionString { get; set; }
            public String RelativeLocation { get; set; }
            public String BaseLocation { get; set; }
            public String SasBlobToken { get; set; }
        }
        public class BatchResponseStructure
        {
            public int StatusCode { get; set; }
            public BatchResult Result { get; set; }
            public String Details { get; set; }
            public BatchResponseStructure()
            {
                this.Result = new BatchResult();
            }
        }
    }
}




