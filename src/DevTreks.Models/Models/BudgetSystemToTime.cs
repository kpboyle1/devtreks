using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystemToTime
    {
        public BudgetSystemToTime()
        {
            BudgetSystemToOperation = new HashSet<BudgetSystemToOperation>();
            BudgetSystemToOutcome = new HashSet<BudgetSystemToOutcome>();
            LinkedViewToBudgetSystemToTime = new HashSet<LinkedViewToBudgetSystemToTime>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool DiscountYorN { get; set; }
        public int GrowthPeriods { get; set; }
        public short GrowthTypeId { get; set; }
        public bool CommonRefYorN { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseUnit { get; set; }
        public float EnterpriseAmount { get; set; }
        public float AOHFactor { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int BudgetSystemToEnterpriseId { get; set; }

        public virtual ICollection<BudgetSystemToOperation> BudgetSystemToOperation { get; set; }
        public virtual ICollection<BudgetSystemToOutcome> BudgetSystemToOutcome { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystemToTime> LinkedViewToBudgetSystemToTime { get; set; }
        public virtual BudgetSystemToEnterprise BudgetSystemToEnterprise { get; set; }
    }
}
