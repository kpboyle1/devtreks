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
        public IncentiveClass(bool init)
        {
            this.PKId = 0;
            this.IncentiveClassNum = string.Empty;
            this.IncentiveClassName = string.Empty;
            this.IncentiveClassDesc = string.Empty;
            this.Incentive = new List<Incentive>();
        }
        public IncentiveClass(IncentiveClass copyIncentiveClass)
        {
            this.PKId = copyIncentiveClass.PKId;
            this.IncentiveClassNum = copyIncentiveClass.IncentiveClassNum;
            this.IncentiveClassName = copyIncentiveClass.IncentiveClassName;
            this.IncentiveClassDesc = copyIncentiveClass.IncentiveClassDesc;
            this.Incentive = new List<Incentive>();
        }
        public int PKId { get; set; }
        public string IncentiveClassDesc { get; set; }
        public string IncentiveClassName { get; set; }
        public string IncentiveClassNum { get; set; }

        public virtual ICollection<Incentive> Incentive { get; set; }
    }
}
