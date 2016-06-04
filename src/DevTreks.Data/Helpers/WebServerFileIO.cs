using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:	    webserver (http://localhost) file managment utilities
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///Notes:
    ///For example, when a URI beginning with http:// or https:// is passed in requestUri, 
    ///an HttpWebRequest is returned by Create. 
    ///If a URI beginning with ftp:// is passed instead, the Create method will return a FileWebRequest instance. 
    ///If a URI beginning with file:// is passed instead, the Create method will return a FileWebRequest instance.
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
                    //using (WebResponse response = request.GetResponse())
                    //{
                    //    //could also check request.haveresponse
                    //    if (response != null)
                    //    {
                    //        bExists = true;
                    //    }
                    //}

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
                            //could also check request.haveresponse
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
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(dataURL);
                //could also check request.haveresponse
                if (response != null)
                {
                    //retrieve the website contents from the HttpResponseMessage. 
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    //standard protocal in ReadTextAsync
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

                //2.0.0 change: HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);
                //not debugged
                //http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
                var postContent = new Dictionary<string, string>() {
                        { "inputblobpath", inputFileLocation },
                        { "outputcsvblobpath", outputFileLocation },
                        { "script1", script },
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
}
