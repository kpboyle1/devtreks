using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystemToOutput
    {
        public CostSystemToOutput() { }
        public CostSystemToOutput(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            IncentiveRate = 0;
            IncentiveAmount = 0;
            OutputCompositionAmount = 0;
            OutputCompositionUnit = General.NONE;
            OutputAmount1 = 0;
            OutputTimes = 0;
            OutputDate = General.GetDateShortNow();
            RatingClassId = 0;
            RealRateId = 0;
            NominalRateId = 0;
            GeoCodeId = 0;
            CostSystemToOutcomeId = 0;
            CostSystemToOutcome = new CostSystemToOutcome();
            OutputId = 0;
            OutputSeries = new OutputSeries();
        }
        public CostSystemToOutput(CostSystemToOutput rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            IncentiveRate = rt.IncentiveRate;
            IncentiveAmount = rt.IncentiveAmount;
            OutputCompositionAmount = rt.OutputCompositionAmount;
            OutputCompositionUnit = rt.OutputCompositionUnit;
            OutputAmount1 = rt.OutputAmount1;
            OutputTimes = rt.OutputTimes;
            OutputDate = rt.OutputDate;
            RatingClassId = rt.RatingClassId;
            RealRateId = rt.RealRateId;
            NominalRateId = rt.NominalRateId;
            GeoCodeId = rt.GeoCodeId;
            CostSystemToOutcomeId = rt.CostSystemToOutcomeId;
            CostSystemToOutcome = new CostSystemToOutcome();
            OutputId = rt.OutputId;
            OutputSeries = new OutputSeries();
        }
        public int PKId { get; set; }
        public int CostSystemToOutcomeId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public float OutputAmount1 { get; set; }
        public float OutputCompositionAmount { get; set; }
        public string OutputCompositionUnit { get; set; }
        public DateTime OutputDate { get; set; }
        public int OutputId { get; set; }
        public float OutputTimes { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }

        public virtual CostSystemToOutcome CostSystemToOutcome { get; set; }
        public virtual OutputSeries OutputSeries { get; set; }
    }
}
