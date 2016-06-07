using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountToAddIn = new HashSet<AccountToAddIn>();
            AccountToAudit = new HashSet<AccountToAudit>();
            AccountToCredit = new HashSet<AccountToCredit>();
            AccountToLocal = new HashSet<AccountToLocal>();
            AccountToMember = new HashSet<AccountToMember>();
            AccountToNetwork = new HashSet<AccountToNetwork>();
            AccountToService = new HashSet<AccountToService>();
        }
        public Account(bool init)
        {
            this.PKId = 0;
            this.AccountName = string.Empty;
            this.AccountDesc = string.Empty;
            this.AccountLongDesc = string.Empty;
            this.AccountEmail = string.Empty;
            this.AccountURI = string.Empty;
            this.AccountClassId = 0;
            this.AccountClass = new AccountClass();
            this.GeoRegionId = 0;
            this.GeoRegion = new GeoRegion();
            this.AccountToMember = new List<AccountToMember>();
            this.AccountToAudit = new List<AccountToAudit>();
            this.AccountToService = new List<AccountToService>();
            this.AccountToCredit = new List<AccountToCredit>();
            this.AccountToAddIn = new List<AccountToAddIn>();
            this.AccountToLocal = new List<AccountToLocal>();
            this.AccountToNetwork = new List<AccountToNetwork>();
            this.AccountToPayment = new List<AccountToPayment>();
            this.ClubDocFullPath = string.Empty;
            this.PrivateAuthorizationLevel = 0;
            this.URIFull = string.Empty;

        }
        public Account(Account club)
        {
            this.PKId = club.PKId;
            this.AccountName = club.AccountName;
            this.AccountDesc = club.AccountDesc;
            this.AccountLongDesc = club.AccountLongDesc;
            this.AccountEmail = club.AccountEmail;
            this.AccountURI = club.AccountURI;
            this.AccountClassId = club.AccountClassId;
            this.GeoRegionId = club.GeoRegionId;

            this.ClubDocFullPath = (club.ClubDocFullPath == null)
                ? string.Empty : club.ClubDocFullPath;
            this.PrivateAuthorizationLevel = club.PrivateAuthorizationLevel;
            this.URIFull = (club.URIFull == null)
                ? string.Empty : club.URIFull;
            //too dangerous; copy them manually
            this.AccountClass = new AccountClass();
            this.GeoRegion = new GeoRegion();
            this.AccountToMember = new List<AccountToMember>();
            this.AccountToAudit = new List<AccountToAudit>();
            this.AccountToService = new List<AccountToService>();
            this.AccountToCredit = new List<AccountToCredit>();
            this.AccountToAddIn = new List<AccountToAddIn>();
            this.AccountToLocal = new List<AccountToLocal>();
            this.AccountToNetwork = new List<AccountToNetwork>();
            this.AccountToPayment = new List<AccountToPayment>();
        }
        public int PKId { get; set; }
        public string AccountName { get; set; }
        public string AccountDesc { get; set; }
        public string AccountLongDesc { get; set; }
        public string AccountEmail { get; set; }
        public string AccountURI { get; set; }
        public int AccountClassId { get; set; }
        public int GeoRegionId { get; set; }

        public virtual ICollection<AccountToAddIn> AccountToAddIn { get; set; }
        public virtual ICollection<AccountToAudit> AccountToAudit { get; set; }
        public virtual ICollection<AccountToCredit> AccountToCredit { get; set; }
        public virtual ICollection<AccountToLocal> AccountToLocal { get; set; }
        public virtual ICollection<AccountToMember> AccountToMember { get; set; }
        public virtual ICollection<AccountToNetwork> AccountToNetwork { get; set; }
        public virtual ICollection<AccountToService> AccountToService { get; set; }
        public virtual AccountClass AccountClass { get; set; }
        public virtual GeoRegion GeoRegion { get; set; }

        [NotMapped]
        public string ClubDocFullPath { get; set; }
        [NotMapped]
        public AccountHelper.AUTHORIZATION_LEVELS PrivateAuthorizationLevel { get; set; }
        [NotMapped]
        public string URIFull { get; set; }
        [NotMapped]
        public virtual ICollection<AccountToPayment> AccountToPayment { get; set; }
        [NotMapped]
        public decimal TotalCost { get; set; }
        [NotMapped]
        public decimal NetCost { get; set; }
        //clues respondwithlist about the specific list to load
        public const string MemberClubList = "memberclublist";
    }
}
