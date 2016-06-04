using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevTreks.Extensions
{
    public static class ContractHelpers
    {
        /// <summary>
        /// Which step in an extension needs to be carried out?
        /// </summary>
        public enum EXTENSION_STEPS
        {
            stepzero    = 0,
            stepone     = 1,
            steptwo     = 2,
            stepthree   = 3,
            stepfour    = 4,
            stepfive    = 5,
            stepsix     = 6,
            stepseven   = 7,
            stepeight   = 8
        }
        public static EXTENSION_STEPS GetEnumStepNumber(string step)
        {
            EXTENSION_STEPS eStepNumber
                = (!string.IsNullOrEmpty(step))
                ? (EXTENSION_STEPS)Enum.Parse(
                typeof(EXTENSION_STEPS), step)
                : EXTENSION_STEPS.stepzero;
            return eStepNumber;
        }
    }
}
