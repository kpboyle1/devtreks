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
        public BudgetSystem(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            Date = General.GetDateShortNow();
            LastChangedDate = General.GetDateShortNow();
            DocStatus = 0;
            BudgetSystemToEnterprise = new List<BudgetSystemToEnterprise>();
            LinkedViewToBudgetSystem = new List<LinkedViewToBudgetSystem>();
            TypeId = 0;
            BudgetSystemType = new BudgetSystemType();
            ServiceId = 0;
            Service = new Service();
        }
        public BudgetSystem(BudgetSystem rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            Date = rt.Date;
            LastChangedDate = rt.LastChangedDate;
            DocStatus = rt.DocStatus;
            BudgetSystemToEnterprise = new List<BudgetSystemToEnterprise>();
            LinkedViewToBudgetSystem = new List<LinkedViewToBudgetSystem>();
            TypeId = rt.TypeId;
            BudgetSystemType = new BudgetSystemType();
            ServiceId = rt.ServiceId;
            Service = new Service();
        }
        public int PKId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<BudgetSystemToEnterprise> BudgetSystemToEnterprise { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystem> LinkedViewToBudgetSystem { get; set; }
        public virtual Service Service { get; set; }
        public virtual BudgetSystemType BudgetSystemType { get; set; }
    }
}
