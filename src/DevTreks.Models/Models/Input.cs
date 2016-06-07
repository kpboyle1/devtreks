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

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime InputDate { get; set; }
        public string InputUnit1 { get; set; }
        public decimal InputPrice1 { get; set; }
        public float InputPrice1Amount { get; set; }
        public string InputUnit2 { get; set; }
        public decimal InputPrice2 { get; set; }
        public string InputUnit3 { get; set; }
        public decimal InputPrice3 { get; set; }
        public DateTime InputLastChangedDate { get; set; }
        public int RealRateId { get; set; }
        public int NominalRateId { get; set; }
        public int GeoCodeId { get; set; }
        public int DataSourceId { get; set; }
        public int CurrencyClassId { get; set; }
        public int UnitClassId { get; set; }
        public int RatingClassId { get; set; }
        public int InputClassId { get; set; }

        public virtual ICollection<InputSeries> InputSeries { get; set; }
        public virtual ICollection<LinkedViewToInput> LinkedViewToInput { get; set; }
        public virtual InputClass InputClass { get; set; }
    }
}
