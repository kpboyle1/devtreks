using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystemToComponent
    {
        public CostSystemToComponent()
        {
            CostSystemToInput = new HashSet<CostSystemToInput>();
        }
        public CostSystemToComponent(bool init)
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
            ComponentId = 0;
            Component = new Component();
            CostSystemToInput = new List<CostSystemToInput>();
        }
        public CostSystemToComponent(CostSystemToComponent rt)
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
            ComponentId = rt.ComponentId;
            Component = new Component();
            CostSystemToInput = new List<CostSystemToInput>();
        }
        public int PKId { get; set; }
        public float Amount { get; set; }
        public int ComponentId { get; set; }
        public int CostSystemToTimeId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public float EffectiveLife { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public float ResourceWeight { get; set; }
        public decimal SalvageValue { get; set; }
        public string Unit { get; set; }

        public virtual ICollection<CostSystemToInput> CostSystemToInput { get; set; }
        public virtual Component Component { get; set; }
        public virtual CostSystemToTime CostSystemToTime { get; set; }
    }
}
