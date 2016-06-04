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
        public BudgetSystemToTime(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            Date = General.GetDateShortNow();
            DiscountYorN = true;
            GrowthPeriods = 0;
            GrowthTypeId = 0;
            CommonRefYorN = true;
            EnterpriseName = General.NONE;
            EnterpriseUnit = General.NONE;
            EnterpriseAmount = 1;
            AOHFactor = 0;
            IncentiveAmount = 0;
            IncentiveRate = 0;
            LastChangedDate = General.GetDateShortNow();
            BudgetSystemToEnterpriseId = 0;
            BudgetSystemToEnterprise = new BudgetSystemToEnterprise();
            BudgetSystemToOperation = new List<BudgetSystemToOperation>();
            BudgetSystemToOutcome = new List<BudgetSystemToOutcome>();
            LinkedViewToBudgetSystemToTime = new List<LinkedViewToBudgetSystemToTime>();
        }
        public BudgetSystemToTime(BudgetSystemToTime rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            Date = rt.Date;
            DiscountYorN = rt.DiscountYorN;
            GrowthPeriods = rt.GrowthPeriods;
            GrowthTypeId = rt.GrowthTypeId;
            CommonRefYorN = rt.CommonRefYorN;
            EnterpriseName = rt.EnterpriseName;
            EnterpriseUnit = rt.EnterpriseUnit;
            EnterpriseAmount = rt.EnterpriseAmount;
            AOHFactor = rt.AOHFactor;
            IncentiveAmount = rt.IncentiveAmount;
            IncentiveRate = rt.IncentiveRate;
            LastChangedDate = rt.LastChangedDate;
            BudgetSystemToEnterpriseId = rt.BudgetSystemToEnterpriseId;
            BudgetSystemToEnterprise = new BudgetSystemToEnterprise();
            BudgetSystemToOperation = new List<BudgetSystemToOperation>();
            BudgetSystemToOutcome = new List<BudgetSystemToOutcome>();
            LinkedViewToBudgetSystemToTime = new List<LinkedViewToBudgetSystemToTime>();
        }
        public int PKId { get; set; }
        public float AOHFactor { get; set; }
        public int BudgetSystemToEnterpriseId { get; set; }
        public bool CommonRefYorN { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool DiscountYorN { get; set; }
        public float EnterpriseAmount { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseUnit { get; set; }
        public int GrowthPeriods { get; set; }
        public short GrowthTypeId { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }

        public virtual ICollection<BudgetSystemToOperation> BudgetSystemToOperation { get; set; }
        public virtual ICollection<BudgetSystemToOutcome> BudgetSystemToOutcome { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystemToTime> LinkedViewToBudgetSystemToTime { get; set; }
        public virtual BudgetSystemToEnterprise BudgetSystemToEnterprise { get; set; }
    }
}
