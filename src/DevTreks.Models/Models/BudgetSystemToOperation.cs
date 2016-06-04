using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystemToOperation
    {
        public BudgetSystemToOperation()
        {
            BudgetSystemToInput = new HashSet<BudgetSystemToInput>();
        }
        public BudgetSystemToOperation(bool init)
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
            BudgetSystemToTimeId = 0;
            BudgetSystemToTime = new BudgetSystemToTime();
            OperationId = 0;
            Operation = new Operation();
            BudgetSystemToInput = new List<BudgetSystemToInput>();
        }
        public BudgetSystemToOperation(BudgetSystemToOperation rt)
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
            BudgetSystemToTimeId = rt.BudgetSystemToTimeId;
            BudgetSystemToTime = new BudgetSystemToTime();
            OperationId = rt.OperationId;
            Operation = new Operation();
            BudgetSystemToInput = new List<BudgetSystemToInput>();
        }
        public int PKId { get; set; }
        public float Amount { get; set; }
        public int BudgetSystemToTimeId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public float EffectiveLife { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int OperationId { get; set; }
        public float ResourceWeight { get; set; }
        public decimal SalvageValue { get; set; }
        public string Unit { get; set; }

        public virtual ICollection<BudgetSystemToInput> BudgetSystemToInput { get; set; }
        public virtual BudgetSystemToTime BudgetSystemToTime { get; set; }
        public virtual Operation Operation { get; set; }
    }
}
