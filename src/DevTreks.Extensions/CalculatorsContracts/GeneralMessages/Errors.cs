using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.Threading;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Retrieve generic errors for all extensions. These error messages 
    ///             are generally passed from extension module to DevTreks to handle.
    ///             Extension authors are responsible for handling errors not 
    ///             found in this project.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
    /// </summary>
    public sealed class Errors
    {
        private Errors()
        {
            //no private instances needed; this class uses static methods only
        }
        //resource manager for localized text.
        private static ResourceManager rm = new ResourceManager(typeof(Errors).Namespace
            + ".GeneralMessages.Messages", Assembly.GetAssembly(typeof(Errors)));
        public static string MakeStandardErrorMsg(string resourceName)
        {
            string sErrorMsg = string.Empty;
            sErrorMsg = GetMessage(resourceName);
            return sErrorMsg;
        }
        public static string MakeStandardErrorMsg(string innerException, string resourceName)
        {
            string sErrorMsg = string.Empty;
            sErrorMsg = GetMessage(resourceName);
            int iKeepLength = (innerException.Length > 100) ? 100 : innerException.Length - 1;
            sErrorMsg += sErrorMsg.Remove(iKeepLength);
            return sErrorMsg;
        }
        /// <summary>
        /// return a localized string exception message to the client
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetMessage(string resourceName)
        {
            //2.0.0 deprecated: InitCurrentResources();
            string sResourceValue = string.Empty;
            sResourceValue = rm.GetString(resourceName);
            if (sResourceValue == null)
            {
                sResourceValue = string.Empty;
            }
            return sResourceValue;
        }
        public static string GetMessage(string resourceName, CultureInfo culture)
        {
            //keep for debugging embedded resources
            string sResourceValue = string.Empty;
            try
            {
                sResourceValue = rm.GetString(resourceName);
            }
            catch (Exception x)
            {
                string sErr = x.Message;
            }
            if (sResourceValue == null)
            {
                sResourceValue = string.Empty;
            }
            return sResourceValue;
        }
        //2.0.0 deprecated moved localization into Startup.cs
        //public static void InitCurrentResources()
        //{
        //    //init localizations to current culture
        //    CultureInfo c = CultureInfo.CurrentCulture;
        //    Thread.CurrentThread.CurrentCulture = c;
        //    Thread.CurrentThread.CurrentUICulture = c;
        //}
    }
}
