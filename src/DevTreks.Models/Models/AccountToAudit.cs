using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountToAudit
    {
        public AccountToAudit() { }
        public AccountToAudit(bool init)
        {
            this.PKId = 0;
            this.MemberId = 0;
            this.MemberRole = string.Empty;
            this.ClubInUseId = 0;
            this.ClubInUseAuthorizationLevel = string.Empty;
            this.EditedDocURI = string.Empty;
            this.EditedDocFullPath = string.Empty;
            this.ServerSubAction = string.Empty;
            this.EditDate = General.GetDateShortNow();
            this.AccountId = 0;
            this.Account = new Account();
        }
        public AccountToAudit(AccountToAudit copyAccountToAddIn)
        {
            this.PKId = copyAccountToAddIn.PKId;
            this.MemberId = copyAccountToAddIn.MemberId;
            this.MemberRole = copyAccountToAddIn.MemberRole;
            this.ClubInUseId = copyAccountToAddIn.ClubInUseId;
            this.ClubInUseAuthorizationLevel = copyAccountToAddIn.ClubInUseAuthorizationLevel;
            this.EditedDocURI = copyAccountToAddIn.EditedDocURI;
            this.EditedDocFullPath = copyAccountToAddIn.EditedDocFullPath;
            this.ServerSubAction = copyAccountToAddIn.ServerSubAction;
            this.EditDate = copyAccountToAddIn.EditDate;
            this.AccountId = copyAccountToAddIn.AccountId;
            this.Account = new Account();
        }
        public int PKId { get; set; }
        public int MemberId { get; set; }
        public string MemberRole { get; set; }
        public int ClubInUseId { get; set; }
        public string ClubInUseAuthorizationLevel { get; set; }
        public string EditedDocURI { get; set; }
        public string EditedDocFullPath { get; set; }
        public string ServerSubAction { get; set; }
        public DateTime EditDate { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
