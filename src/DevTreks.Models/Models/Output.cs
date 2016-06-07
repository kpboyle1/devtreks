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

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OutputDate { get; set; }
        public string OutputUnit1 { get; set; }
        public float OutputAmount1 { get; set; }
        public decimal OutputPrice1 { get; set; }
        public DateTime OutputLastChangedDate { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int GeoCodeId { get; set; }
        public int DataSourceId { get; set; }
        public int CurrencyClassId { get; set; }
        public int UnitClassId { get; set; }
        public int RatingClassId { get; set; }
        public int OutputClassId { get; set; }

        public virtual ICollection<LinkedViewToOutput> LinkedViewToOutput { get; set; }
        public virtual ICollection<OutputSeries> OutputSeries { get; set; }
        public virtual OutputClass OutputClass { get; set; }
    }
}
