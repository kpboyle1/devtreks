using System.Resources;
using System.Reflection;

namespace DevTreks.Resources
{
    /// <summary>
    ///Purpose:		Return localized resources to user interfaces
    ///Name:		DevTreksResources.cs
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///Reference:	https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
    /// </summary>
    public sealed class DevTreksResources
    {
        //private constructor restricts an instance of this class from being created. Thread safe.
        private DevTreksResources()
        {
            //Startup.cs localizes resources on current thread
        }
        //init resource manager		
        private static ResourceManager DevTreksResMngr = new ResourceManager(typeof(DevTreksResources).Namespace 
            + ".DevTreksResources", Assembly.GetAssembly(typeof(DevTreksResources)));
        
        //return a localized string to the client for the DevTreks views
        public static string GetDevTreksString(string resourceName)
        {
            string sResourceValue = DevTreksResMngr.GetString(resourceName);
            return sResourceValue;
        }
    }
}
