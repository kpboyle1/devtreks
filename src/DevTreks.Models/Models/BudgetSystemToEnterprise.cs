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
        public BudgetSystemToEnterprise(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Num2 = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            InitialValue = 0;
            SalvageValue = 0;
            LastChangedDate = General.GetDateShortNow();
            RatingClassId = 0;
            RealRateId = 0;
            NominalRateId = 0;
            GeoCodeId = 0;
            DataSourceId = 0;
            CurrencyClassId = 0;
            UnitClassId = 0;
            BudgetSystemId = 0;
            BudgetSystem = new BudgetSystem();
            BudgetSystemToTime = new List<BudgetSystemToTime>();
            LinkedViewToBudgetSystemToEnterprise = new List<LinkedViewToBudgetSystemToEnterprise>();
        }
        public BudgetSystemToEnterprise(BudgetSystemToEnterprise rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Num2 = rt.Num2;
            Name = rt.Name;
            Description = rt.Description;
            InitialValue = rt.InitialValue;
            SalvageValue = rt.SalvageValue;
            LastChangedDate = rt.LastChangedDate;
            RatingClassId = rt.RatingClassId;
            RealRateId = rt.RealRateId;
            NominalRateId = rt.NominalRateId;
            GeoCodeId = rt.GeoCodeId;
            DataSourceId = rt.DataSourceId;
            CurrencyClassId = rt.CurrencyClassId;
            UnitClassId = rt.UnitClassId;
            BudgetSystemId = rt.BudgetSystemId;
            BudgetSystem = new BudgetSystem();
            BudgetSystemToTime = new List<BudgetSystemToTime>();
            LinkedViewToBudgetSystemToEnterprise = new List<LinkedViewToBudgetSystemToEnterprise>();
        }
        public int PKId { get; set; }
        public int BudgetSystemId { get; set; }
        public int CurrencyClassId { get; set; }
        public int DataSourceId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public decimal InitialValue { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public string Num2 { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public decimal SalvageValue { get; set; }
        public int UnitClassId { get; set; }

        public virtual ICollection<BudgetSystemToTime> BudgetSystemToTime { get; set; }
        public virtual ICollection<LinkedViewToBudgetSystemToEnterprise> LinkedViewToBudgetSystemToEnterprise { get; set; }
        public virtual BudgetSystem BudgetSystem { get; set; }
    }
}
