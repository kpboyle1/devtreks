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
        public int CostSystemId { get; set; }

        public virtual ICollection<CostSystemToTime> CostSystemToTime { get; set; }
        public virtual ICollection<LinkedViewToCostSystemToPractice> LinkedViewToCostSystemToPractice { get; set; }
        public virtual CostSystem CostSystem { get; set; }
    }
}
