using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Currency
    {
        public Currency()
        {
            CurrencyConversion = new HashSet<CurrencyConversion>();
        }

        public int PKId { get; set; }
        public DateTime CurrencyDate { get; set; }
        public int CurrencyClassId { get; set; }

        public virtual ICollection<CurrencyConversion> CurrencyConversion { get; set; }
        public virtual CurrencyClass CurrencyClass { get; set; }
    }
}
