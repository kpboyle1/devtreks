using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class AccountToService
    {
        public AccountToService()
        {
            AccountToIncentive = new HashSet<AccountToIncentive>();
            AccountToPayment = new HashSet<AccountToPayment>();
        }
        public AccountToService(bool init)
        {
            this.PKId = 0;
            this.Name = string.Empty;
            this.Amount1 = 0;
            this.Status = string.Empty;
            this.StatusDate = General.GetDateShortNow();
            this.AuthorizationLevel = 0;
            this.StartDate = General.GetDateShortNow();
            this.EndDate = General.GetDateShortNow();
            this.LastChangedDate = General.GetDateShortNow();
            this.IsOwner = false;
            this.AccountId = 0;
            this.Account = new Account();
            this.ServiceId = 0;
            this.Service = new Service();
            this.AccountToIncentive = new List<AccountToIncentive>();
            this.AccountToPayment = new List<AccountToPayment>();

            this.IsSelected = false;
            this.OwningClubId = 0;
        }

        public AccountToService(AccountToService service)
        {
            this.PKId = service.PKId;
            this.Name = service.Name;
            this.Amount1 = service.Amount1;
            this.Status = service.Status;
            this.StatusDate = service.StatusDate;
            this.AuthorizationLevel = service.AuthorizationLevel;
            this.StartDate = service.StartDate;
            this.EndDate = service.EndDate;
            this.LastChangedDate = service.LastChangedDate;
            this.IsOwner = service.IsOwner;
            this.AccountId = service.AccountId;
            this.Account = new Account();
            this.ServiceId = service.ServiceId;
            this.Service = new Service();
            this.AccountToIncentive = new List<AccountToIncentive>();
            this.AccountToPayment = new List<AccountToPayment>();
            //non db
            this.IsSelected = service.IsSelected;
            this.OwningClubId = service.OwningClubId;
        }
        public int PKId { get; set; }
        public string Name { get; set; }
        public int Amount1 { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public short AuthorizationLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public bool IsOwner { get; set; }
        public int AccountId { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<AccountToIncentive> AccountToIncentive { get; set; }
        public virtual ICollection<AccountToPayment> AccountToPayment { get; set; }
        public virtual Account Account { get; set; }
        public virtual Service Service { get; set; }
        [NotMapped]
        public virtual bool IsSelected { get; set; }
        [NotMapped]
        public virtual int OwningClubId { get; set; }
        [NotMapped]
        public virtual decimal HostServiceFee { get; set; }
        [NotMapped]
        public virtual double HostServiceRate { get; set; }
        [NotMapped]
        public decimal TotalCost { get; set; }
        [NotMapped]
        public decimal NetCost { get; set; }
    }
}
