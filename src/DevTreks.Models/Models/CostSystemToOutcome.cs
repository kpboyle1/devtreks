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

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float ResourceWeight { get; set; }
        public float Amount { get; set; }
        public string Unit { get; set; }
        public float EffectiveLife { get; set; }
        public decimal SalvageValue { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime Date { get; set; }
        public int CostSystemToTimeId { get; set; }
        public int OutcomeId { get; set; }

        public virtual ICollection<CostSystemToOutput> CostSystemToOutput { get; set; }
        public virtual CostSystemToTime CostSystemToTime { get; set; }
        public virtual Outcome Outcome { get; set; }
    }
}
