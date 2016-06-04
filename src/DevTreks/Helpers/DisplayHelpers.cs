using System.Text.RegularExpressions;

using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		static utility methods for displaying stylesheets
    ///Author:		www.devtreks.org
    ///Date:		2011, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public sealed class DisplayHelpers
    {
        private DisplayHelpers()
        {
            //no constructor: exposes static methods 
        }
        public static int GetIntValue(string value)
        {
            int iValue = (isNumber(value))
                ? DataHelpers.ConvertStringToInt(value) : 0;
            return iValue;
        }
        public static double GetDoubleValue(string value)
        {
            double dbValue = (isNumber(value))
                ? DataHelpers.ConvertStringToDouble(value) : 0;
            return dbValue;
        }
        public static bool isNumber(string test)
        {
            bool bIsNumber = false;
            if (test != string.Empty && test != null)
            {
                bool bIsNotNumber = ValidateNonNumber(test);
                if (bIsNotNumber == false)
                {
                    bIsNumber = true;
                }
            }
            return bIsNumber;
        }
        public static bool ValidateNonNumber(string sNumber)
        {
            bool bIsNotNumber = true;
            //has a non-number? (except for decimal, negative sign, and comma)
            string sCleanNumber = sNumber;
            sCleanNumber = Replace(",", sNumber);
            sCleanNumber = Replace(".", sCleanNumber);
            sCleanNumber = Replace("-", sCleanNumber);
            string sExp = @"(\D)";
            Regex regExpression = new Regex(sExp);
            bIsNotNumber = regExpression.IsMatch(sCleanNumber);
            return bIsNotNumber;
        }
        public static string Replace(string toReplace, string inputValue)
        {
            string sNewValue = inputValue.Replace(toReplace, "");
            return sNewValue;
        }
    }
}