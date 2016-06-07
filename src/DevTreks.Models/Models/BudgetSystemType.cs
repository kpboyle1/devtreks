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

        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<BudgetSystem> BudgetSystem { get; set; }
    }
}
