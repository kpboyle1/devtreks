using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystemToTime
    {
        public CostSystemToTime()
        {
            CostSystemToComponent = new HashSet<CostSystemToComponent>();
            CostSystemToOutcome = new HashSet<CostSystemToOutcome>();
            LinkedViewToCostSystemToTime = new HashSet<LinkedViewToCostSystemToTime>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool DiscountYorN { get; set; }
        public int GrowthPeriods { get; set; }
        public short GrowthTypeId { get; set; }
        public bool CommonRefYorN { get; set; }
        public string PracticeName { get; set; }
        public string PracticeUnit { get; set; }
        public float PracticeAmount { get; set; }
        public float AOHFactor { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int CostSystemToPracticeId { get; set; }

        public virtual ICollection<CostSystemToComponent> CostSystemToComponent { get; set; }
        public virtual ICollection<CostSystemToOutcome> CostSystemToOutcome { get; set; }
        public virtual ICollection<LinkedViewToCostSystemToTime> LinkedViewToCostSystemToTime { get; set; }
        public virtual CostSystemToPractice CostSystemToPractice { get; set; }
    }
}
