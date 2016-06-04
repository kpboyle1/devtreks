using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Input
    {
        public Input()
        {
            InputSeries = new HashSet<InputSeries>();
            LinkedViewToInput = new HashSet<LinkedViewToInput>();
        }
        public Input(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.InputUnit1 = General.NONE;
            this.InputPrice1 = 0;
            this.InputPrice1Amount = 1;
            this.InputUnit2 = General.NONE;
            this.InputPrice2 = 0;
            this.InputUnit3 = General.NONE;
            this.InputPrice3 = 1;

            this.InputDate = General.GetDateShortNow();
            this.InputLastChangedDate = General.GetDateShortNow();
            this.RatingClassId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.DataSourceId = 0;
            this.GeoCodeId = 0;
            this.CurrencyClassId = 0;
            this.UnitClassId = 0;

            this.InputClassId = 0;
            this.InputClass = new InputClass();
            this.LinkedViewToInput = new List<LinkedViewToInput>();
            this.InputSeries = new List<InputSeries>();
        }
        public Input(Input rp)
        {
            this.PKId = rp.PKId;
            this.Num = rp.Num;
            this.Name = rp.Name;
            this.Description = rp.Description;
            this.InputUnit1 = rp.InputUnit1;
            this.InputPrice1 = rp.InputPrice1;
            this.InputPrice1Amount = rp.InputPrice1Amount;
            this.InputUnit2 = rp.InputUnit2;
            this.InputPrice2 = rp.InputPrice2;
            this.InputUnit3 = rp.InputUnit3;
            this.InputPrice3 = rp.InputPrice3;

            this.InputDate = rp.InputDate;
            this.InputLastChangedDate = rp.InputLastChangedDate;
            this.RatingClassId = rp.RatingClassId;
            this.RealRateId = rp.RealRateId;
            this.NominalRateId = rp.NominalRateId;
            this.DataSourceId = rp.DataSourceId;
            this.GeoCodeId = rp.GeoCodeId;
            this.CurrencyClassId = rp.CurrencyClassId;
            this.UnitClassId = rp.UnitClassId;

            this.InputClassId = rp.InputClassId;
            this.InputClass = new InputClass();
            this.LinkedViewToInput = new List<LinkedViewToInput>();
            this.InputSeries = new List<InputSeries>();
        }
        public int PKId { get; set; }
        public int CurrencyClassId { get; set; }
        public int DataSourceId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public int InputClassId { get; set; }
        public DateTime InputDate { get; set; }
        public DateTime InputLastChangedDate { get; set; }
        public decimal InputPrice1 { get; set; }
        public float InputPrice1Amount { get; set; }
        public decimal InputPrice2 { get; set; }
        public decimal InputPrice3 { get; set; }
        public string InputUnit1 { get; set; }
        public string InputUnit2 { get; set; }
        public string InputUnit3 { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int UnitClassId { get; set; }

        public virtual ICollection<InputSeries> InputSeries { get; set; }
        public virtual ICollection<LinkedViewToInput> LinkedViewToInput { get; set; }
        public virtual InputClass InputClass { get; set; }
    }
}
