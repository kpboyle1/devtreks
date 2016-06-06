using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace DevTreks.Exceptions
{
    /// <summary>
    ///Purpose:		generic error handling class
    ///Author:		www.devtreks.org
    ///Date:		2016, June
    ///References:	https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
    /// </summary>
    public sealed class DevTreksErrors
	{
		private DevTreksErrors()
		{
			//no private instances needed; this class uses static methods only
		}
		// static and constant variable declarations
        //error messages returned to clients are displayed in Home.Master.spanDisplayError
        public const string DISPLAY_ERROR_ID = "spanDisplayError";
        //error messages that are generated while writing an html response are displayed in:
        public const string DISPLAY_ERROR_ID2 = "spanDisplayError2";
        public const string ERRORFOLDERNAME = "Errors";
		private readonly static string EXCEPTIONMANAGER_NAME = typeof(DevTreksErrors).Name;
		private const string EXCEPTIONMANAGEMENT_CONFIG_SECTION = "exceptionManagement";
        private readonly static string HTML_BREAK = "<br />";
		// Resource Manager for localized text.
		private static ResourceManager rm = new ResourceManager(typeof(DevTreksErrors).Namespace 
            + ".DevTreksErrors", Assembly.GetAssembly(typeof(DevTreksErrors)));
		//error message delimiters
		private const string STRING_DELIMITER = ";";
		private static char[] STRING_DELIMITERS = new char[] {';'};
		/// <summary>
		/// Static method to publish the exception information.
		/// </summary>
		/// <param name="exception">The exception object whose information should be published.</param>
		/// <param name="errorName">The name of the string resource holding ui error message.</param>
		public static string Publish(Exception exception, string errorName, 
            string defaultErrorRootFullPath)
		{
            int iIndex = -1;
			//see if the error name is a delimited string
            if (errorName != null) iIndex = errorName.IndexOf(STRING_DELIMITER);
			string sFullError = string.Empty;
			if (iIndex != -1) 
			{
				StringBuilder oString = new StringBuilder();
				string[] aMessages = errorName.Split(STRING_DELIMITERS);
				if (aMessages.Length >= 0) 
				{
					oString.Append(GetMessage("TITLE_MESSAGE"));
					oString.Append(aMessages[0]);
					oString.Append(HTML_BREAK);
                    oString.Append(GetMessage("TAB_CLICK_TO_ERASE"));
                    oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 1) 
				{
					oString.Append(GetMessage("TITLE_APPLICATION"));
					oString.Append(aMessages[1]);
					oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 2) 
				{
					oString.Append(GetMessage("TITLE_SUBAPPLICATION"));
					oString.Append(aMessages[2]);
					oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 3) 
				{
					oString.Append(GetMessage("TITLE_NODENAME"));
					oString.Append(aMessages[3]);
					oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 4) 
				{
					oString.Append(GetMessage("TITLE_ID"));
					oString.Append(aMessages[4]);
					oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 5) 
				{
					oString.Append(GetMessage("TITLE_SERVICEID"));
					oString.Append(aMessages[5]);
					oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 6) 
				{
					oString.Append(GetMessage("TITLE_ACCOUNTID"));
					oString.Append(aMessages[6]);
					oString.Append(HTML_BREAK);
				}
				if (aMessages.Length >= 7) 
				{
					oString.Append(GetMessage("TITLE_FULLMSG"));
					string sAdminMsg = aMessages[7];
					//keep 20% of the full error message (don't intimidate the customer too much)
//					int iLength = sAdminMsg.Length;
//					int iKeepLength = (iLength / 50);
//					iLength = iLength - iKeepLength;
//					string sKeepMsg = sAdminMsg.Remove(iKeepLength, iLength);
//					oString.Append(sKeepMsg);
					oString.Append(sAdminMsg);
					oString.Append(HTML_BREAK);
//					sKeepMsg = string.Empty;
					sAdminMsg = string.Empty;
				}
				sFullError = oString.ToString();
			}
			else 
			{
                sFullError = string.Concat(GetMessage("TITLE_MESSAGE"), exception.ToString(), 
                    GetMessage("TAB_CLICK_TO_ERASE"), HTML_BREAK);
			}
            if (Path.IsPathRooted(defaultErrorRootFullPath))
            {
                AppendToErrorLog(sFullError, 2, defaultErrorRootFullPath);
            }
            else
            {
                System.Diagnostics.Trace.TraceInformation("Error '{0}'", sFullError);
            }
			return sFullError;
		}
        public static string Publish(string errorName,
            string defaultErrorRootFullPath)
        {
            int iIndex = -1;
            //see if the error name is a delimited string
            if (errorName != null) iIndex = errorName.IndexOf(STRING_DELIMITER);
            string sFullError = string.Empty;
            if (iIndex != -1)
            {
                StringBuilder oString = new StringBuilder();
                string[] aMessages = errorName.Split(STRING_DELIMITERS);
                if (aMessages.Length >= 0)
                {
                    oString.Append(GetMessage("TITLE_MESSAGE"));
                    oString.Append(aMessages[0]);
                    oString.Append(HTML_BREAK);
                    oString.Append(GetMessage("TAB_CLICK_TO_ERASE"));
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 1)
                {
                    oString.Append(GetMessage("TITLE_APPLICATION"));
                    oString.Append(aMessages[1]);
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 2)
                {
                    oString.Append(GetMessage("TITLE_SUBAPPLICATION"));
                    oString.Append(aMessages[2]);
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 3)
                {
                    oString.Append(GetMessage("TITLE_NODENAME"));
                    oString.Append(aMessages[3]);
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 4)
                {
                    oString.Append(GetMessage("TITLE_ID"));
                    oString.Append(aMessages[4]);
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 5)
                {
                    oString.Append(GetMessage("TITLE_SERVICEID"));
                    oString.Append(aMessages[5]);
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 6)
                {
                    oString.Append(GetMessage("TITLE_ACCOUNTID"));
                    oString.Append(aMessages[6]);
                    oString.Append(HTML_BREAK);
                }
                if (aMessages.Length >= 7)
                {
                    oString.Append(GetMessage("TITLE_FULLMSG"));
                    oString.Append(aMessages[7]);
                    oString.Append(HTML_BREAK);
                }
                sFullError = oString.ToString();
            }
            if (Path.IsPathRooted(defaultErrorRootFullPath))
            {
                AppendToErrorLog(sFullError, 2, defaultErrorRootFullPath);
            }
            else
            {
                System.Diagnostics.Trace.TraceInformation("Error '{0}'", sFullError);
            }
            return sFullError;
        }
        public static string MakeStandardErrorMsg(string resourceName)
        {
            string sErrorMsg = string.Empty;
            sErrorMsg = GetMessage(resourceName);
            return sErrorMsg;
        }
        public static string MakeStandardErrorMsg(string errorMessage, string resourceName)
        {
            string sErrorMsg = string.Empty;
            if (string.IsNullOrEmpty(resourceName) == false)
            {
                //errors should be translated and generated here
                sErrorMsg = string.Concat(GetMessage("TITLE_MESSAGE"), GetMessage(resourceName),
                    errorMessage, GetMessage("TAB_CLICK_TO_ERASE"));
            }
            else
            {
                sErrorMsg = string.Concat(GetMessage("TITLE_MESSAGE"), errorMessage,
                    GetMessage("TAB_CLICK_TO_ERASE"));
            }
            int iKeepLength = (errorMessage.Length > 100) ? 100 : errorMessage.Length - 1;
            if (iKeepLength > 0)
            {
                sErrorMsg += sErrorMsg.Remove(iKeepLength);
            }
            return sErrorMsg;
        }

		/// <summary>
		/// return a localized string exception message to the client
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		public static string GetMessage(string resourceName)
		{
			string sResourceValue = rm.GetString(resourceName);
			return sResourceValue;
		}
        //public static string GetMessage(string resourceName, CultureInfo culture)
        //{
        //    //keep for debugging embedded resources
        //    string sResourceValue = string.Empty;
        //    try
        //    {
        //        sResourceValue = rm.GetString(resourceName);
        //    }
        //    catch (Exception x)
        //    {
        //        string sErr = x.Message;
        //    }
        //    if (sResourceValue == null)
        //    {
        //        sResourceValue = string.Empty;
        //    }
        //    return sResourceValue;
        //}
        /// <summary>
        /// Append a new error message to the log file
        /// </summary>
        /// <param name="errMessage"></param>
        public static void AppendToErrorLog(string errMessage, int recursions, 
            string defaultErrorRootFullPath)
		{
			//relative path to standard write directory
			string sToday = GetTodaysDate();
			StringBuilder oString = new StringBuilder();
			oString.Append(defaultErrorRootFullPath);
			oString.Append(sToday);
			oString.Append(".txt");
			string sLogFile = oString.ToString();
			oString = null;
			try 
			{
                //azure has blobdirectory in name
                if (Path.IsPathRooted(defaultErrorRootFullPath))
                {
                    string sDirectoryName = Path.GetDirectoryName(defaultErrorRootFullPath);
                    if (Directory.Exists(sDirectoryName) == false)
                    {
                        Directory.CreateDirectory(defaultErrorRootFullPath);
                    }
                }
                FileStream oFileStream = null;
                if (File.Exists(sLogFile))
                {
                    oFileStream = new FileStream(sLogFile, FileMode.Append, 
                        FileAccess.Write, FileShare.Write);
                }
                else
                {
                    oFileStream = new FileStream(sLogFile, FileMode.CreateNew, 
                        FileAccess.Write, FileShare.Write);
                }

                if (oFileStream != null)
                {
                    using (oFileStream)
                    {
                        //don't use asynchronous unless log file shows this class is generating errors
                        StreamWriter oLogWriter = new StreamWriter(oFileStream);
                        if (oLogWriter != null)
                        {
                            using (oLogWriter)
                            {
                                LogError(errMessage, oLogWriter);
                            }
                        }
                    }
                }
			}
			catch (Exception x)
			{
				if (recursions == 1) 
				{
                    //162 removed: no reason to recurse 
                    //AppendToErrorLog(errMessage, 1, defaultErrorRootFullPath);
				}
			}
		}
		private static string GetTodaysDate()
		{
			DateTime  dtToday = new DateTime();
			dtToday = (DateTime) DateTime.Today;
			string sToday = dtToday.ToString("yyyy-MM-dd");
			sToday = sToday.Replace("-", "_");
			return sToday;
		}
		private static void LogError(string errMessage, TextWriter logWriter)
		{
			logWriter.Write("\r\nLog Entry : ");
			logWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
				DateTime.Now.ToLongDateString());
			logWriter.WriteLine("  :");
			logWriter.WriteLine("  :{0}", errMessage);
			logWriter.WriteLine ("-------------------------------");
			// Update the underlying file.
			logWriter.Flush(); 
		}

	}
}
