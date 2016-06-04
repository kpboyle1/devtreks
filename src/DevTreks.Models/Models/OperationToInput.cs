using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OperationToInput
    {
        public OperationToInput() { }
        public OperationToInput(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.IncentiveRate = 0;
            this.IncentiveAmount = 0;
            this.InputPrice1Amount = 1;
            this.InputPrice2Amount = 0;
            this.InputPrice3Amount = 0;
            this.InputTimes = 1;
            this.InputDate = General.GetDateShortNow();
            this.InputUseAOHOnly = false;

            this.RatingClassId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.GeoCodeId = 0;

            this.InputId = 0;
            this.InputSeries = new InputSeries();
            this.OperationId = 0;
            this.Operation = new Operation();
        }
        public OperationToInput(OperationToInput rp)
        {
            this.PKId = rp.PKId;
            this.Num = rp.Num;
            this.Name = rp.Name;
            this.Description = rp.Description;
            this.IncentiveRate = rp.IncentiveRate;
            this.IncentiveAmount = rp.IncentiveAmount;
            this.InputPrice1Amount = rp.InputPrice1Amount;
            this.InputPrice2Amount = rp.InputPrice2Amount;
            this.InputPrice3Amount = rp.InputPrice3Amount;
            this.InputTimes = rp.InputTimes;
            this.InputDate = rp.InputDate;
            this.InputUseAOHOnly = rp.InputUseAOHOnly;

            this.RatingClassId = rp.RatingClassId;
            this.RealRateId = rp.RealRateId;
            this.NominalRateId = rp.NominalRateId;
            this.GeoCodeId = rp.GeoCodeId;

            this.InputId = rp.InputId;
            this.InputSeries = new InputSeries();
            this.OperationId = rp.OperationId;
            this.Operation = new Operation();
        }
        public int PKId { get; set; }
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
        public int OperationId { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }

        public virtual InputSeries InputSeries { get; set; }
        public virtual Operation Operation { get; set; }
    }
}
