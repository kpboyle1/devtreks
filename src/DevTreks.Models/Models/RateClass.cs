using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class RateClass
    {
        public RateClass()
        {
            Rate = new HashSet<Rate>();
        }

        public int PKId { get; set; }
        public string RateClassName { get; set; }
        public DateTime RateClassYear { get; set; }

        public virtual ICollection<Rate> Rate { get; set; }
    }
}
