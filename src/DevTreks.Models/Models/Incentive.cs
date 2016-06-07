using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Incentive
    {
        public Incentive()
        {
            AccountToIncentive = new HashSet<AccountToIncentive>();
        }

        public int PKId { get; set; }
        public string IncentiveNum { get; set; }
        public string IncentiveName { get; set; }
        public string IncentiveDesc { get; set; }
        public float IncentiveRate1 { get; set; }
        public decimal IncentiveAmount1 { get; set; }
        public int IncentiveClassId { get; set; }

        public virtual ICollection<AccountToIncentive> AccountToIncentive { get; set; }
        public virtual IncentiveClass IncentiveClass { get; set; }
    }
}
