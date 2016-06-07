using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystemToInput
    {
        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float IncentiveRate { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float InputPrice1Amount { get; set; }
        public float InputPrice2Amount { get; set; }
        public float InputPrice3Amount { get; set; }
        public float InputTimes { get; set; }
        public DateTime InputDate { get; set; }
        public bool InputUseAOHOnly { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int GeoCodeId { get; set; }
        public int BudgetSystemToOperationId { get; set; }
        public int InputId { get; set; }
        public decimal InputPrice1 { get; set; }
        public int NominalRateId { get; set; }

        public virtual BudgetSystemToOperation BudgetSystemToOperation { get; set; }
        public virtual InputSeries InputSeries { get; set; }
        //actual
        //public virtual InputSeries Input { get; set; }
    }
}
