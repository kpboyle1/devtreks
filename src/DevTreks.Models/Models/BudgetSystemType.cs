using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystemType
    {
        public BudgetSystemType()
        {
            BudgetSystem = new HashSet<BudgetSystem>();
        }
        public BudgetSystemType(bool init)
        {
            this.PKId = 0;
            this.Label = General.NONE;
            this.Name = General.NONE;
            this.NetworkId = 0;
            this.ServiceClassId = 0;
            this.BudgetSystem = new List<BudgetSystem>();
        }
        public BudgetSystemType(BudgetSystemType rt)
        {
            this.PKId = rt.PKId;
            this.Label = rt.Label;
            this.Name = rt.Name;
            this.NetworkId = rt.NetworkId;
            this.ServiceClassId = rt.ServiceClassId;
            this.BudgetSystem = new List<BudgetSystem>();
        }
        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<BudgetSystem> BudgetSystem { get; set; }
    }
}
