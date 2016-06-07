using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class CurrencyClass
    {
        public CurrencyClass()
        {
            Currency = new HashSet<Currency>();
        }

        public int PKId { get; set; }
        public string CurrencyClassName { get; set; }
        public string CurrencyClassAbbrev { get; set; }
        public string CurrencyClassDesc { get; set; }

        public virtual ICollection<Currency> Currency { get; set; }
    }
}
