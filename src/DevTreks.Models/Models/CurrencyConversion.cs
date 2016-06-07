using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CurrencyConversion
    {
        public int PKId { get; set; }
        public int Currency1Id { get; set; }
        public int Currency2Id { get; set; }
        public float CurrencyConversionFactor { get; set; }
        public string CurrencyToName { get; set; }
        public string CurrencyFromName { get; set; }
        public int CurrencyClassId { get; set; }

        public virtual Currency Currency1 { get; set; }
    }
}
