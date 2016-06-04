using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystemToPractice
    {
        public CostSystemToPractice()
        {
            CostSystemToTime = new HashSet<CostSystemToTime>();
            LinkedViewToCostSystemToPractice = new HashSet<LinkedViewToCostSystemToPractice>();
        }
        public CostSystemToPractice(bool init)
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
            CostSystemId = 0;
            CostSystem = new CostSystem();
            CostSystemToTime = new List<CostSystemToTime>();
            LinkedViewToCostSystemToPractice = new List<LinkedViewToCostSystemToPractice>();
        }
        public CostSystemToPractice(CostSystemToPractice rt)
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
            CostSystemId = rt.CostSystemId;
            CostSystem = new CostSystem();
            CostSystemToTime = new List<CostSystemToTime>();
            LinkedViewToCostSystemToPractice = new List<LinkedViewToCostSystemToPractice>();
        }
        public int PKId { get; set; }
        public int CostSystemId { get; set; }
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

        public virtual ICollection<CostSystemToTime> CostSystemToTime { get; set; }
        public virtual ICollection<LinkedViewToCostSystemToPractice> LinkedViewToCostSystemToPractice { get; set; }
        public virtual CostSystem CostSystem { get; set; }
    }
}
