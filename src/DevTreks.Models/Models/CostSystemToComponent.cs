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
        public int ComponentId { get; set; }

        public virtual ICollection<CostSystemToInput> CostSystemToInput { get; set; }
        public virtual Component Component { get; set; }
        public virtual CostSystemToTime CostSystemToTime { get; set; }
    }
}
