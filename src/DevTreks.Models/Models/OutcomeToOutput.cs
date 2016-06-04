using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutcomeToOutput
    {
        public OutcomeToOutput() { }
        public OutcomeToOutput(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.IncentiveRate = 0;
            this.IncentiveAmount = 0;
            this.OutputCompositionAmount = 1;
            this.OutputCompositionUnit = General.NONE;
            this.OutputAmount1 = 0;
            this.OutputTimes = 1;
            this.OutputDate = General.GetDateShortNow();

            this.RatingClassId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.GeoCodeId = 0;

            this.OutputId = 0;
            this.OutputSeries = new OutputSeries();
            this.OutcomeId = 0;
            this.Outcome = new Outcome();
        }
        public OutcomeToOutput(OutcomeToOutput rp)
        {
            this.PKId = rp.PKId;
            this.Num = rp.Num;
            this.Name = rp.Name;
            this.Description = rp.Description;
            this.IncentiveRate = rp.IncentiveRate;
            this.IncentiveAmount = rp.IncentiveAmount;
            this.OutputCompositionAmount = rp.OutputCompositionAmount;
            this.OutputCompositionUnit = rp.OutputCompositionUnit;
            this.OutputAmount1 = rp.OutputAmount1;
            this.OutputTimes = rp.OutputTimes;
            this.OutputDate = rp.OutputDate;

            this.RatingClassId = rp.RatingClassId;
            this.RealRateId = rp.RealRateId;
            this.NominalRateId = rp.NominalRateId;
            this.GeoCodeId = rp.GeoCodeId;

            this.OutputId = rp.OutputId;
            this.OutputSeries = new OutputSeries();
            this.OutcomeId = rp.OutcomeId;
            this.Outcome = new Outcome();
        }
        public int PKId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public int OutcomeId { get; set; }
        public float OutputAmount1 { get; set; }
        public float OutputCompositionAmount { get; set; }
        public string OutputCompositionUnit { get; set; }
        public DateTime OutputDate { get; set; }
        public int OutputId { get; set; }
        public float OutputTimes { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }

        public virtual Outcome Outcome { get; set; }
        public virtual OutputSeries OutputSeries { get; set; }
    }
}
