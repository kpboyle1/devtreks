using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystemToOutcome
    {
        public CostSystemToOutcome()
        {
            CostSystemToOutput = new HashSet<CostSystemToOutput>();
        }
        public CostSystemToOutcome(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            ResourceWeight = 0;
            Amount = 0;
            Unit = General.NONE;
            EffectiveLife = 0;
            SalvageValue = 0;
            IncentiveAmount = 0;
            IncentiveRate = 0;
            Date = General.GetDateShortNow();
            CostSystemToTimeId = 0;
            CostSystemToTime = new CostSystemToTime();
            OutcomeId = 0;
            Outcome = new Outcome();
            CostSystemToOutput = new List<CostSystemToOutput>();
        }
        public CostSystemToOutcome(CostSystemToOutcome rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            ResourceWeight = rt.ResourceWeight;
            Amount = rt.Amount;
            Unit = rt.Unit;
            EffectiveLife = rt.EffectiveLife;
            SalvageValue = rt.SalvageValue;
            IncentiveAmount = rt.IncentiveAmount;
            IncentiveRate = rt.IncentiveRate;
            Date = rt.Date;
            CostSystemToTimeId = rt.CostSystemToTimeId;
            CostSystemToTime = new CostSystemToTime();
            OutcomeId = rt.OutcomeId;
            Outcome = new Outcome();
            CostSystemToOutput = new List<CostSystemToOutput>();
        }
        public int PKId { get; set; }
        public float Amount { get; set; }
        public int CostSystemToTimeId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public float EffectiveLife { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int OutcomeId { get; set; }
        public float ResourceWeight { get; set; }
        public decimal SalvageValue { get; set; }
        public string Unit { get; set; }

        public virtual ICollection<CostSystemToOutput> CostSystemToOutput { get; set; }
        public virtual CostSystemToTime CostSystemToTime { get; set; }
        public virtual Outcome Outcome { get; set; }
    }
}
