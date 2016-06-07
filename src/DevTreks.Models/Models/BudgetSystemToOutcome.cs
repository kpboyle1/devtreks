using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystemToOutcome
    {
        public BudgetSystemToOutcome()
        {
            BudgetSystemToOutput = new HashSet<BudgetSystemToOutput>();
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
        public int BudgetSystemToTimeId { get; set; }
        public int OutcomeId { get; set; }

        public virtual ICollection<BudgetSystemToOutput> BudgetSystemToOutput { get; set; }
        public virtual BudgetSystemToTime BudgetSystemToTime { get; set; }
        public virtual Outcome Outcome { get; set; }
    }
}
