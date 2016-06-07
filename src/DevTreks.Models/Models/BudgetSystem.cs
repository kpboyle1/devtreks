using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystem
    {
        public BudgetSystem()
        {
            BudgetSystemToEnterprise = new HashSet<BudgetSystemToEnterprise>();
            LinkedViewToBudgetSystem = new HashSet<LinkedViewToBudgetSystem>();
        }
        
        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<BudgetSystemToEnterprise> BudgetSystemToEnterprise { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystem> LinkedViewToBudgetSystem { get; set; }
        public virtual Service Service { get; set; }
        public virtual BudgetSystemType BudgetSystemType { get; set; }
        //public virtual BudgetSystemType Type { get; set; }
    }
}
