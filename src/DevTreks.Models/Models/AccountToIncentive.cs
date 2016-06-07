using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class AccountToIncentive
    {
        public AccountToIncentive() { }
        public AccountToIncentive(bool init)
        {
            this.PKId = 0;
            this.AccountToServiceId = 0;
            this.IncentiveId = 0;
            this.AccountToService = new AccountToService();
            this.Incentive = new Incentive();
        }
        public AccountToIncentive(AccountToIncentive copyAccountToIncentive)
        {
            this.PKId = copyAccountToIncentive.PKId;
            this.AccountToServiceId = copyAccountToIncentive.AccountToServiceId;
            this.IncentiveId = copyAccountToIncentive.IncentiveId;

            this.AccountToService = new AccountToService();
            this.Incentive = new Incentive();
        }
        public int PKId { get; set; }
        public int AccountToServiceId { get; set; }
        public int IncentiveId { get; set; }

        public virtual AccountToService AccountToService { get; set; }
        public virtual Incentive Incentive { get; set; }
        [NotMapped]
        public decimal TotalCost { get; set; }
        [NotMapped]
        public decimal NetCost { get; set; }
    }
}
