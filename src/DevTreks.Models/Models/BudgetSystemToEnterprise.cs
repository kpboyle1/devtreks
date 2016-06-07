using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class BudgetSystemToEnterprise
    {
        public BudgetSystemToEnterprise()
        {
            BudgetSystemToTime = new HashSet<BudgetSystemToTime>();
            LinkedViewToBudgetSystemToEnterprise = new HashSet<LinkedViewToBudgetSystemToEnterprise>();
        }
        
        public int PKId { get; set; }
        public string Num { get; set; }
        public string Num2 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InitialValue { get; set; }
        public decimal SalvageValue { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int GeoCodeId { get; set; }
        public int DataSourceId { get; set; }
        public int CurrencyClassId { get; set; }
        public int UnitClassId { get; set; }
        public int BudgetSystemId { get; set; }

        public virtual ICollection<BudgetSystemToTime> BudgetSystemToTime { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystemToEnterprise> LinkedViewToBudgetSystemToEnterprise { get; set; }
        public virtual BudgetSystem BudgetSystem { get; set; }
    }
}
