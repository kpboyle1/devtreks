using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:	    webserver (http://localhost) file managment utilities
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	
    ///Notes:
    /// </summary>
    public class WebServerFileIO
    {
        public WebServerFileIO()
        {
            //each instance holds its own state
        }
        public static bool Exists(string URIPath)
        {
            bool bExists = false;
            if (!string.IsNullOrEmpty(URIPath))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URIPath);
                if (request != null)
                {
                    //this returns a 404 error msg if not found
                    //this should not be used with large files
                    //2.0.0 refactor
                    if (request.HaveResponse)
                    {
                        bExists = true;
                    }

                }
            }
            return bExists;
        }
        
        public List<string> ReadLines(string dataURL)
        {
            List<string> lines = new List<string>();
            if (!string.IsNullOrEmpty(dataURL))
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(dataURL);
                    if (request != null)
                    {
                        // Create a response object.
                        //this returns a 404 error msg if not found
                        //localhost:44300 should debug using fileserver files; or http://localhost urls
                        using (WebResponse response = request.GetResponse())
                        {
                            if (response != null)
                            {
                                using (StreamReader stream =
                                   new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                                {
                                    while (!stream.EndOfStream)
                                    {
                                        lines.Add(stream.ReadLine());
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    //dataurl has to be http or https (has to come from resource base element url)
                    if (x.Message.Contains("404"))
                    {
                        //don't return messages
                    }
                    return null;
                }
            }
            return lines;
        }
        public async Task<List<string>> ReadLinesAsync(string dataURL)
        {
            List<string> lines = new List<string>();
            string sFile = await ReadTextAsync(dataURL);
            lines = GeneralHelpers.GetLinesFromUTF8Encoding(sFile);
            return lines;
        }
        public async Task<List<string>> ReadLinesAsync2(string dataURL)
        {
            //use HttpClient as an async alternative
            List<string> lines = new List<string>();
            if (!string.IsNullOrEmpty(dataURL))
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetStreamAsync(dataURL).ConfigureAwait(false);
                        if (response != null)
                        {
                            using (StreamReader stream =
                               new StreamReader(response, Encoding.UTF8))
                            {
                                while (!stream.EndOfStream)
                                {
                                    lines.Add(stream.ReadLine());
                                }
                            }
                        }
                    }
                }
                catch (Exception x)
                {
                    //dataurl has to be http or https (has to come from resource base element url)
                    if (x.Message.Contains("404"))
                    {
                        //don't return messages
                    }
                    return null;
                }
            }
            return lines;
        }
        public string ReadText(string dataURL)
        {
            string sText = string.Empty;
            if (!string.IsNullOrEmpty(dataURL))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(dataURL);
                if (request != null)
                {
                    // Create a response object.
                    //this returns a 404 error msg if not found
                    //localhost:44300 should debug using fileserver files; or http://localhost urls
                    using (WebResponse response = request.GetResponse())
                    {
                        //could also check request.haveresponse
                        if (response != null)
                        {
                            using (StreamReader stream =
                               new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                sText = stream.ReadToEnd();
                            }
                        }
                    }
                }
            }
            return sText;
        }
        /// <summary>
        /// Same ReadTextAsync pattern in FileIO and AzureIO
        /// </summary>
        /// <param name="dataURL"></param>
        /// <returns></returns>
        public async Task<string> ReadTextAsync(string dataURL)
        {
            string sFile = string.Empty;
            //2.0.0 may not be cross platform
            //consider refactor by converting webfile path to filesys path
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(dataURL);
                //could also check request.haveresponse
                if (response != null)
                {
                    //retrieve the website contents from the HttpResponseMessage. 
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    //standard protocol in ReadTextAsync
                    sFile = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }
            }
            return sFile;
        }
        public async Task<bool> CopyWebFileToFileSystemAsync(string webFileURL, string fileSysPath)
        {
            //2.0.0 uses localhost to debug azure blob storage
            bool bHasCopied = false;
            if (Path.IsPathRooted(fileSysPath))
            {
                string sWebFileString = await ReadTextAsync(webFileURL);
                if (!string.IsNullOrEmpty(sWebFileString))
                {
                    FileIO fileIO = new FileIO();
                    bHasCopied = await fileIO.WriteTextFileAsync(fileSysPath, sWebFileString);
                }
            }
            return bHasCopied;
        }

        public static async Task<string> InvokeHttpRequestResponseService(string baseURL, string apiKey,
            string inputFileLocation, string outputFileLocation, string script)
        {
            //like azure invoke... this stores response in output file
            //web service reads from azure storage and writes to azure storage
            //must have preloaded a good input file in rproject format for this to work
            string sResponse = string.Empty;
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    GlobalParameters = new Dictionary<string, string>() {
                        { "inputblobpath", inputFileLocation },
                        { "outputcsvblobpath", outputFileLocation },
                        { "script1", script },
                    }
                };
                //const string apiKey = "abc123"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri(baseURL);
                //client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/d454361ecdcb4ec4b03fa1aec5a7c0e2/services/9f23e23f1a724d408126276db8b6017f/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                //2.0.0 change: 
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);
                ////not debugged
                ////http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
                //var postContent = new Dictionary<string, string>() {
                //        { "inputblobpath", inputFileLocation },
                //        { "outputcsvblobpath", outputFileLocation },
                //        { "script1", script },
                //    };
                //var content = new FormUrlEncodedContent(postContent);
                //HttpResponseMessage response = await client.PostAsync(baseURL, content).ConfigureAwait(false);
                

                if (response.IsSuccessStatusCode)
                {
                    //the web service stores the result file in blob storage 
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //sResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    sResponse = string.Concat("Success with status code: ", response.StatusCode);
                }
                else
                {
                    sResponse = string.Concat("Failed with status code: ", response.StatusCode);
                }
            }
            return sResponse;
        }
        public static async Task<string> InvokeHttpRequestResponseService2(string baseURL, string apiKey,
            string inputBlob1Location, string inputBlob2Location,
            string outputBlob1Location, string outputBlob2Location)
        {
            //like azure invoke... this stores response in output file
            //web service reads from azure storage and writes to azure storage
            //must have preloaded a good input file in rproject format for this to work
            string sResponse = string.Empty;
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    GlobalParameters = new Dictionary<string, string>() {
                        { "inputdata2", inputBlob2Location },
                        { "outputdata2", outputBlob2Location },
                        { "outputdata1", outputBlob1Location },
                        { "inputdata1", inputBlob1Location },
                    }
                };
                //const string apiKey = "abc123"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri(baseURL);
                //client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/d454361ecdcb4ec4b03fa1aec5a7c0e2/services/9f23e23f1a724d408126276db8b6017f/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                //2.0.0 change: HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);
                //not debugged
                //http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
                var postContent = new Dictionary<string, string>() {
                        { "inputdata2", inputBlob2Location },
                        { "outputdata2", outputBlob2Location },
                        { "outputdata1", outputBlob1Location },
                        { "inputdata1", inputBlob1Location },
                };
                var content = new FormUrlEncodedContent(postContent);
                HttpResponseMessage response = await client.PostAsync(baseURL, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    //the web service stores the result file in blob storage 
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //sResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    sResponse = string.Concat("Success with status code: ", response.StatusCode);
                }
                else
                {
                    sResponse = string.Concat("Failed with status code: ", response.StatusCode);
                }
            }
            return sResponse;
        }
        public static async Task<bool> ClientCreate(StatScript statScript)
        {
            bool bIsSuccess = false;
            // HTTP POST example
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(statScript);

            // Post statscript
            Uri address = new Uri(string.Concat(statScript.DefaultWebDomain, "api/statscript"));
            try
            {
                //create controller actionresult says this only returns a url 
                //to the created statscript referenced in Location Header
                //needs the configure or returns without the results
                HttpResponseMessage response =
                    await client.PostAsync(address,
                    new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                //can also use .PostAsJson(address, statScript) but requires Microsoft.AspNet.WebApi.Client.5.2.3 package

                // Check that response was successful or throw exception
                response.EnsureSuccessStatusCode();

                //the response body contains the json string result
                string statResult = JsonConvert.SerializeObject(response);
                if (!string.IsNullOrEmpty(statResult))
                {
                    string body = await response.Content.ReadAsStringAsync();
                    StatScript deserializedScript = JsonConvert.DeserializeObject<StatScript>(body);
                    statScript.StatisticalResult = deserializedScript.StatisticalResult;
                    if (!string.IsNullOrEmpty(statScript.StatisticalResult))
                    {
                        bIsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                statScript.ErrorMessage = ex.Message.ToString();
            }
            
            return bIsSuccess;
        }

    }

    public class ScoreData
    {

        public Dictionary<string, string> FeatureVector { get; set; }

        public Dictionary<string, string> GlobalParameters { get; set; }

    }

    public class ScoreRequest
    {

        public string Id { get; set; }

        public ScoreData Instance { get; set; }

    }
    public class StatScript
    {
        public StatScript()
        {
            this.Key = string.Empty;
            this.Name = string.Empty;
            this.DateCompleted = string.Empty;
            this.DataURL = string.Empty;
            this.ScriptURL = string.Empty;
            this.OutputURL = string.Empty;
            this.StatType = string.Empty;
            this.RExecutablePath = string.Empty;
            this.PyExecutablePath = string.Empty;
            this.DefaultRootFullFilePath = string.Empty;
            this.DefaultRootWebStoragePath = string.Empty;
            this.DefaultWebDomain = string.Empty;
            this.StatisticalResult = string.Empty;
            this.IsComplete = false;
            this.IsDevelopment = false;
            this.ErrorMessage = string.Empty;
        }
        public StatScript(StatScript statScript)
        {
            this.Key = statScript.Key;
            this.Name = statScript.Name;
            this.DateCompleted = statScript.DateCompleted;
            this.DataURL = statScript.DataURL;
            this.ScriptURL = statScript.ScriptURL;
            this.OutputURL = statScript.OutputURL;
            this.StatType = statScript.StatType;
            this.RExecutablePath = statScript.RExecutablePath;
            this.PyExecutablePath = statScript.PyExecutablePath;
            this.DefaultRootFullFilePath = statScript.DefaultRootFullFilePath;
            this.DefaultRootWebStoragePath = statScript.DefaultRootWebStoragePath;
            this.DefaultWebDomain = statScript.DefaultWebDomain;
            this.StatisticalResult = statScript.StatisticalResult;
            this.IsComplete = statScript.IsComplete;
            this.IsDevelopment = statScript.IsDevelopment;
            this.ErrorMessage = statScript.ErrorMessage;
        }
        public enum STAT_TYPE
        {
            none = 0,
            r = 1,
            py = 2,
            aml = 3
        }
        //first 1 prop set by api
        public string Key { get; set; }
        //these 4 properties are set by client and sent as POCO object
        public string Name { get; set; }
        public string DateCompleted { get; set; }
        public string DataURL { get; set; }
        public string ScriptURL { get; set; }
        public string OutputURL { get; set; }
        //the client sends this to host
        public string StatType { get; set; }
        //the host sets these 4 properties using di from appsettings
        public string RExecutablePath { get; set; }
        public string PyExecutablePath { get; set; }
        public string DefaultRootFullFilePath { get; set; }
        public string DefaultRootWebStoragePath { get; set; }
        public string DefaultWebDomain { get; set; }
        public string StatisticalResult { get; set; }
        //set by api
        public bool IsComplete { get; set; }
        public bool IsDevelopment { get; set; }
        public string ErrorMessage { get; set; }
        public static STAT_TYPE GetStatType(string executablepath)
        {
            STAT_TYPE eStatType = STAT_TYPE.none;
            if (executablepath.Contains("python"))
            {
                eStatType = STAT_TYPE.py;
            }
            else
            {
                eStatType = STAT_TYPE.r;
            }
            //aml addressed when subalgo 4 is debugged
            return eStatType;
        }
        public static StatScript GetStatScript(bool isPyScript, string scriptFilePath, 
            string inputFilePath)
        {
            //used to test the post http (create) controller action in web api
            //client in DevTreks posts directly to create controller and doesn't use this at all
            StatScript testStat = new StatScript();
            testStat.Key = Guid.NewGuid().ToString();
            testStat.Name = "GetStatScript";
            
            //make sure these exist
            testStat.DataURL = inputFilePath;
            testStat.ScriptURL = scriptFilePath;
            if (isPyScript)
            {
                //py script
                testStat.StatType = StatScript.STAT_TYPE.py.ToString();
            }
            else
            {
                //r script
                testStat.StatType = StatScript.STAT_TYPE.r.ToString();
            }
            testStat.IsComplete = false;
            return testStat;
        }
    }
}
