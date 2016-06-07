using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class IncentiveClass
    {
        public IncentiveClass()
        {
            Incentive = new HashSet<Incentive>();
        }

        public int PKId { get; set; }
        public string IncentiveClassNum { get; set; }
        public string IncentiveClassName { get; set; }
        public string IncentiveClassDesc { get; set; }

        public virtual ICollection<Incentive> Incentive { get; set; }
    }
}
