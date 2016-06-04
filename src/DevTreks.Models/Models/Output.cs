using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Output
    {
        public Output()
        {
            LinkedViewToOutput = new HashSet<LinkedViewToOutput>();
            OutputSeries = new HashSet<OutputSeries>();
        }
        public Output(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.OutputUnit1 = General.NONE;
            this.OutputPrice1 = 0;
            this.OutputAmount1 = 1;

            this.OutputDate = General.GetDateShortNow();
            this.OutputLastChangedDate = General.GetDateShortNow();
            this.RatingClassId = 0;
            this.RealRateId = 0;
            this.NominalRateId = 0;
            this.DataSourceId = 0;
            this.GeoCodeId = 0;
            this.CurrencyClassId = 0;
            this.UnitClassId = 0;

            this.OutputClassId = 0;
            this.OutputClass = new OutputClass();
            this.LinkedViewToOutput = new List<LinkedViewToOutput>();
            this.OutputSeries = new List<OutputSeries>();
        }
        public Output(Output rp)
        {
            this.PKId = rp.PKId;
            this.Num = rp.Num;
            this.Name = rp.Name;
            this.Description = rp.Description;
            this.OutputUnit1 = rp.OutputUnit1;
            this.OutputPrice1 = rp.OutputPrice1;
            this.OutputAmount1 = rp.OutputAmount1;

            this.OutputDate = rp.OutputDate;
            this.OutputLastChangedDate = rp.OutputLastChangedDate;
            this.RatingClassId = rp.RatingClassId;
            this.RealRateId = rp.RealRateId;
            this.NominalRateId = rp.NominalRateId;
            this.DataSourceId = rp.DataSourceId;
            this.GeoCodeId = rp.GeoCodeId;
            this.CurrencyClassId = rp.CurrencyClassId;
            this.UnitClassId = rp.UnitClassId;

            this.OutputClassId = rp.OutputClassId;
            this.OutputClass = new OutputClass();
            this.LinkedViewToOutput = new List<LinkedViewToOutput>();
            this.OutputSeries = new List<OutputSeries>();
        }
        public int PKId { get; set; }
        public int CurrencyClassId { get; set; }
        public int DataSourceId { get; set; }
        public string Description { get; set; }
        public int GeoCodeId { get; set; }
        public string Name { get; set; }
        public int NominalRateId { get; set; }
        public string Num { get; set; }
        public float OutputAmount1 { get; set; }
        public int OutputClassId { get; set; }
        public DateTime OutputDate { get; set; }
        public DateTime OutputLastChangedDate { get; set; }
        public decimal OutputPrice1 { get; set; }
        public string OutputUnit1 { get; set; }
        public int RatingClassId { get; set; }
        public int RealRateId { get; set; }
        public int UnitClassId { get; set; }

        public virtual ICollection<LinkedViewToOutput> LinkedViewToOutput { get; set; }
        public virtual ICollection<OutputSeries> OutputSeries { get; set; }
        public virtual OutputClass OutputClass { get; set; }
    }
}
