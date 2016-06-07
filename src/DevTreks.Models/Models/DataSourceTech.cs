using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DataSourceTech
    {
        public int PKId { get; set; }
        public string DSTechName { get; set; }
        public string DSTechDesc { get; set; }
        public string DSTechURL { get; set; }
        public int AccountId { get; set; }
        public int GeoCodeId { get; set; }
    }
}
