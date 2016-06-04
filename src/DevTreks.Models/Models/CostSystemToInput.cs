using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CostSystemToInput
    {
        public CostSystemToInput() { }
        public CostSystemToInput(bool init)
        {
            PKId = 0;
            Num = General.NONE;
            Name = General.NONE;
            Description = General.NONE;
            IncentiveRate = 0;
            IncentiveAmount = 0;
            InputPrice1Amount = 0;
            InputPrice2Amount = 0;
            InputPrice3Amount = 0;
            InputTimes = 1;
            InputDate = General.GetDateShortNow();
            InputUseAOHOnly = false;
            RatingClassId = 0;
            RealRateId = 0;
            GeoCodeId = 0;
            NominalRateId = 0;

            CostSystemToComponentId = 0;
            CostSystemToComponent = new CostSystemToComponent(); ;
            InputId = 0;
            InputSeries = new InputSeries();
        }
        public CostSystemToInput(CostSystemToInput rt)
        {
            PKId = rt.PKId;
            Num = rt.Num;
            Name = rt.Name;
            Description = rt.Description;
            IncentiveRate = rt.IncentiveRate;
            IncentiveAmount = rt.IncentiveAmount;
            InputPrice1Amount = rt.InputPrice1Amount;
            InputPrice2Amount = rt.InputPrice2Amount;
            InputPrice3Amount = rt.InputPrice3Amount;
            InputTimes = rt.InputTimes;
            InputDate = rt.InputDate;
            InputUseAOHOnly = rt.InputUseAOHOnly;
            RatingClassId = rt.RatingClassId;
            RealRateId = rt.RealRateId;
            GeoCodeId = rt.GeoCodeId;
            NominalRateId = rt.NominalRateId;

            CostSystemToComponentId = rt.CostSystemToComponentId;
            CostSystemToComponent = new CostSystemToComponent();
            InputId = rt.InputId;
            InputSeries = new InputSeries();
        }
        public int PKId { get; set; }
        public int CostSystemToComponentId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public decimal IncentiveAmount { get; set; }
        public float IncentiveRate { get; set; }
        public DateTime InputDate { get; set; }
        public int InputId { get; set; }
        public float InputPrice1Amount { get; set; }
        public float InputPrice2Amount { get; set; }
        public float InputPrice3Amount { get; set; }
        public float InputTimes { get; set; }
        public bool InputUseAOHOnly { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }

        public virtual CostSystemToComponent CostSystemToComponent { get; set; }
        public virtual InputSeries InputSeries { get; set; }
    }
}
