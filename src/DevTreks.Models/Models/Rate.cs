using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Rate
    {
        public int PKId { get; set; }
        public string RateEnum { get; set; }
        public string RateName { get; set; }
        public float RateValue { get; set; }
        public DateTime RateDate { get; set; }
        public int RateClassId { get; set; }

        public virtual RateClass RateClass { get; set; }
    }
}
