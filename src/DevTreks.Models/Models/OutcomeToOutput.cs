using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutcomeToOutput
    {
        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float IncentiveRate { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float OutputCompositionAmount { get; set; }
        public string OutputCompositionUnit { get; set; }
        public float OutputAmount1 { get; set; }
        public float OutputTimes { get; set; }
        public DateTime OutputDate { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int GeoCodeId { get; set; }
        public int OutcomeId { get; set; }
        public int OutputId { get; set; }

        public virtual Outcome Outcome { get; set; }
        public virtual OutputSeries OutputSeries { get; set; }
    }
}
