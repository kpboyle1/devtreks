using System;

namespace DevTreks.Models
{
    public class General
    {
        public const string NONE = "none";
        public static DateTime GetDateShortNow()
        {
            DateTime oToday = DateTime.Parse(DateTime.Today.ToShortDateString());
            return oToday;
        }
    }
}
