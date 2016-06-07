using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountClass
    {
        public AccountClass()
        {
            Account = new HashSet<Account>();
        }
        public AccountClass(bool init)
        {
            this.PKId = 0;
            this.AccountClassNum = string.Empty;
            this.AccountClassName = string.Empty;
            this.AccountClassDesc = string.Empty;
            this.Account = new List<Account>();
        }
        public AccountClass(AccountClass copyAccountClass)
        {
            this.PKId = copyAccountClass.PKId;
            this.AccountClassNum = copyAccountClass.AccountClassNum;
            this.AccountClassName = copyAccountClass.AccountClassName;
            this.AccountClassDesc = copyAccountClass.AccountClassDesc;
            this.Account = new List<Account>();
        }
        public int PKId { get; set; }
        public string AccountClassNum { get; set; }
        public string AccountClassName { get; set; }
        public string AccountClassDesc { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
