using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountToCredit
    {
        public AccountToCredit() { }
        public AccountToCredit(bool init)
        {
            this.PKId = 0;
            this.CardFullName = string.Empty;
            this.CardFullNumber = string.Empty;
            this.CardNumberSalt = string.Empty;
            this.CardShortNumber = string.Empty;
            this.CardType = string.Empty;
            this.CardEndMonth = string.Empty;
            this.CardEndYear = string.Empty;
            this.CardState = string.Empty;
            this.AccountId = 0;
            this.Account = new Account();
        }
        public AccountToCredit(AccountToCredit copyAccountToCredit)
        {
            this.PKId = copyAccountToCredit.PKId;
            this.CardFullName = copyAccountToCredit.CardFullName;
            this.CardFullNumber = copyAccountToCredit.CardFullNumber;
            this.CardNumberSalt = copyAccountToCredit.CardNumberSalt;
            this.CardShortNumber = copyAccountToCredit.CardShortNumber;
            this.CardType = copyAccountToCredit.CardType;
            this.CardEndMonth = copyAccountToCredit.CardEndMonth;
            this.CardEndYear = copyAccountToCredit.CardEndYear;
            this.CardState = copyAccountToCredit.CardState;
            this.AccountId = copyAccountToCredit.AccountId;
            this.Account = new Account();
        }
        public int PKId { get; set; }
        public string CardFullName { get; set; }
        public string CardFullNumber { get; set; }
        public string CardNumberSalt { get; set; }
        public string CardShortNumber { get; set; }
        public string CardType { get; set; }
        public string CardEndMonth { get; set; }
        public string CardEndYear { get; set; }
        public string CardState { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
