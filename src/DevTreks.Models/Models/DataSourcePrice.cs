using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DataSourcePrice
    {
        public int PKId { get; set; }
        public string DSPriceName { get; set; }
        public string DSPriceDesc { get; set; }
        public string DSPriceURL { get; set; }
        public int AccountId { get; set; }
        public int GeoCodeId { get; set; }
    }
}
