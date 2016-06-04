using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		Math utilities used by the DoStepsAddInView's extension calculators.
    ///Author:		www.devtreks.org
    ///Date:		2015, January
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class MathHelpers
    {
        public enum CORRELATION_TYPES
        {
            none = 0,
            pearson = 1,
            spearman = 2
        }
        public static CORRELATION_TYPES GetCorrelationType(string calcType)
        {
            CORRELATION_TYPES eCorrelationType = CORRELATION_TYPES.pearson;
            if (calcType == CORRELATION_TYPES.pearson.ToString())
            {
                eCorrelationType = CORRELATION_TYPES.pearson;
            }
            else if (calcType == CORRELATION_TYPES.spearman.ToString())
            {
                eCorrelationType = CORRELATION_TYPES.spearman;
            }
            return eCorrelationType;
        }
        public static double DiscountFactor(double decimalAnnualRate, double periods)
        {
            double dbDiscountFactor = (1 / (System.Math.Pow((1 + decimalAnnualRate), periods)));
            return dbDiscountFactor;
        }
        public static double DiscountFactorE(double decimalAnnualRate, double periods)
        {
            double dbDiscountFactor = System.Math.Pow((-1 * decimalAnnualRate), periods);
            double dbDiscountEFactor = (1 - System.Math.Pow(System.Math.E, dbDiscountFactor)) / decimalAnnualRate;
            return dbDiscountFactor;
        }
    }
}
