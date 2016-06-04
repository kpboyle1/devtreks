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
        public CostSystemToTime(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            Date = General.GetDateShortNow();
            DiscountYorN = true;
            GrowthPeriods = 0;
            GrowthTypeId = 0;
            CommonRefYorN = true;
            PracticeName = General.NONE;
            PracticeUnit = General.NONE;
            PracticeAmount = 1;
            AOHFactor = 0;
            IncentiveAmount = 0;
            IncentiveRate = 0;
            LastChangedDate = General.GetDateShortNow();
            CostSystemToPracticeId = 0;
            CostSystemToPractice = new CostSystemToPractice();
            CostSystemToComponent = new List<CostSystemToComponent>();
            CostSystemToOutcome = new List<CostSystemToOutcome>();
            LinkedViewToCostSystemToTime = new List<LinkedViewToCostSystemToTime>();
        }
        public CostSystemToTime(CostSystemToTime rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            Date = rt.Date;
            DiscountYorN = rt.DiscountYorN;
            GrowthPeriods = rt.GrowthPeriods;
            GrowthTypeId = rt.GrowthTypeId;
            CommonRefYorN = rt.CommonRefYorN;
            PracticeName = rt.PracticeName;
            PracticeUnit = rt.PracticeUnit;
            PracticeAmount = rt.PracticeAmount;
            AOHFactor = rt.AOHFactor;
            IncentiveAmount = rt.IncentiveAmount;
            IncentiveRate = rt.IncentiveRate;
            LastChangedDate = rt.LastChangedDate;
            CostSystemToPracticeId = rt.CostSystemToPracticeId;
            CostSystemToPractice = new CostSystemToPractice();
            CostSystemToComponent = new List<CostSystemToComponent>();
            CostSystemToOutcome = new List<CostSystemToOutcome>();
            LinkedViewToCostSystemToTime = new List<LinkedViewToCostSystemToTime>();
        }
        public int PKId { get; set; }
        public float AOHFactor { get; set; }
        public bool CommonRefYorN { get; set; }
        public int CostSystemToPracticeId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool DiscountYorN { get; set; }
        public int GrowthPeriods { get; set; }
        public short GrowthTypeId { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public float PracticeAmount { get; set; }
        public string PracticeName { get; set; }
        public string PracticeUnit { get; set; }

        public virtual ICollection<CostSystemToComponent> CostSystemToComponent { get; set; }
        public virtual ICollection<CostSystemToOutcome> CostSystemToOutcome { get; set; }
        public virtual ICollection<LinkedViewToCostSystemToTime> LinkedViewToCostSystemToTime { get; set; }
        public virtual CostSystemToPractice CostSystemToPractice { get; set; }
    }
}
