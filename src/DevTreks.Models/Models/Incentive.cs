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
        public Incentive(bool init)
        {
            this.PKId = 0;
            this.IncentiveNum = string.Empty;
            this.IncentiveName = string.Empty;
            this.IncentiveDesc = string.Empty;
            this.IncentiveRate1 = 0;
            this.IncentiveAmount1 = 0;
            this.IncentiveClassId = 0;

            this.AccountToIncentive = new List<AccountToIncentive>();
            this.IncentiveClass = new IncentiveClass();
        }
        public Incentive(Incentive copyIncentive)
        {
            this.PKId = copyIncentive.PKId;
            this.IncentiveNum = copyIncentive.IncentiveNum;
            this.IncentiveName = copyIncentive.IncentiveName;
            this.IncentiveDesc = copyIncentive.IncentiveDesc;
            this.IncentiveRate1 = copyIncentive.IncentiveRate1;
            this.IncentiveAmount1 = copyIncentive.IncentiveAmount1;
            this.IncentiveClassId = copyIncentive.IncentiveClassId;

            this.AccountToIncentive = new List<AccountToIncentive>();
            this.IncentiveClass = new IncentiveClass();
        }
        public int PKId { get; set; }
        public decimal IncentiveAmount1 { get; set; }
        public int IncentiveClassId { get; set; }
        public string IncentiveDesc { get; set; }
        public string IncentiveName { get; set; }
        public string IncentiveNum { get; set; }
        public float IncentiveRate1 { get; set; }

        public virtual ICollection<AccountToIncentive> AccountToIncentive { get; set; }
        public virtual IncentiveClass IncentiveClass { get; set; }
    }
}
