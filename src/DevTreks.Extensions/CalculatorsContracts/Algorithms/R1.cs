using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Extensions.Algorithms
{
    /// <summary>
    ///Purpose:		Simple AML r project algorithm
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	CTA example 8
    ///</summary>
    public class R1 : Calculator1
    {

        public R1(string[] mathTerms, string[] colNames, string[] depColNames,
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
        public async Task<bool> RunAlgorithmAsync(string inputFilePath, string rFile,
            System.Threading.CancellationToken ctk)
        {
            bool bHasCalcs = false;
            try
            {
                //label, date, learningstep, population, q6
                if (string.IsNullOrEmpty(inputFilePath))
                {
                    this.ErrorMessage ="The R project dataset has not been uploaded.";
                }
                if (string.IsNullOrEmpty(rFile))
                {
                    this.ErrorMessage += "The R project script file URL has not been added to the Joint Data.";
                }
                string sError = string.Empty;
                string sBaseURL =
                    "https://ussouthcentral.services.azureml.net/workspaces/d454361ecdcb4ec4b03fa1aec5a7c0e2/services/9f23e23f1a724d408126276db8b6017f/jobs";

                string sApiKey =
                    "4xs+lIxGYOLEM5u/SrDpSg7CmrWDOIJZzL+dm2igKGfzqToyWTafpSM0v+0J74ZqF1hxsgD7TgJAtYTChOhMHQ==";

                //convert the script file to the script string expected by the algorithm
                List<string> rlines = new List<string>();
                rlines = CalculatorHelpers.ReadLines(_params.ExtensionDocToCalcURI, rFile, out sError);
                this.ErrorMessage += sError;
                if (rlines == null)
                {
                    this.ErrorMessage += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                    return bHasCalcs;
                }
                if (rlines.Count == 0)
                {
                    this.ErrorMessage += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                    return bHasCalcs;
                }
                StringBuilder sbR = new StringBuilder();
                for (int i = 0; i < rlines.Count(); i++)
                {
                    sbR.AppendLine(rlines[i]);
                }
                string rScript = sbR.ToString();

                //web server expects to store in temp/randomid/name.csv
                //web service stores in temp blob
                string sOutputDataURL = CalculatorHelpers.GetTempContainerPath("Routput.csv");

                //web service expects urls that start with container names
                //regular rproject file must be stored in JDataURL
                string sInputContainerPath = CalculatorHelpers.GetContainerPathFromFullURIPath("resources", inputFilePath);

                //async wait so that results can be stored in output file location and parsed into string lines
                SetResponse(sBaseURL, sApiKey, sInputContainerPath, sOutputDataURL, rScript).Wait();
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
                        return bHasCalcs;
                    }
                    if (lines.Count == 0)
                    {
                        this.ErrorMessage += string.Concat(" ", Errors.MakeStandardErrorMsg("DATAURL_BAD"));
                        return bHasCalcs;
                    }
                    sb = new StringBuilder();
                    sb.AppendLine("r project results");
                    //dep var has to be in the R project 1st column
                    string sLine = string.Concat("first variable:  ", _colNames[0]);
                    string[] line = new List<string>().ToArray();
                    for (int i = 0; i < lines.Count(); i++)
                    {
                        line = lines[i].Split(Constants.CSV_DELIMITERS);
                        //lineout[1] = CalculatorHelpers.ConvertStringToDouble(line[0]).ToString("N4", CultureInfo.InvariantCulture);
                        sb.AppendLine(Shared.GetLine(line, false));
                    }
                    //the last line has the ci for the QTM
                    this.QTPredicted = CalculatorHelpers.ConvertStringToDouble(line[0]);
                    this.QTL = CalculatorHelpers.ConvertStringToDouble(line[1]);
                    this.QTU = CalculatorHelpers.ConvertStringToDouble(line[2]);
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
                    this.ErrorMessage += "The calculations could not be run using the web service.";
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage += ex.Message;
            };
            return bHasCalcs;
        }
        public async Task SetResponse(string baseURL, string apiKey,
            string inputBlobLocation, string outputBlobLocation, string rScript)
        {
            _response = await CalculatorHelpers.InvokeHttpRequestResponseService(
                _params.ExtensionDocToCalcURI, baseURL, apiKey,
                inputBlobLocation, outputBlobLocation, rScript).ConfigureAwait(false);
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
