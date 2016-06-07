using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountToPayment
    {
        
        public AccountToPayment() { }
        public AccountToPayment(bool init)
        {
            this.PKId = 0;
            this.PaymentDue = 0;
            this.PaymentDueDate = General.GetDateShortNow();
            this.PaymentPaid = 0;
            this.PaymentPaidDate = General.GetDateShortNow();
            this.CreditDue = 0;
            this.CreditDueDate = General.GetDateShortNow();
            this.CreditPaid = 0;
            this.CreditPaidDate = General.GetDateShortNow();
            this.AccountToServiceId = 0;
            this.AccountToService = new AccountToService();
        }
        public AccountToPayment(AccountToPayment copyAccountToIncentive)
        {
            this.PKId = copyAccountToIncentive.PKId;
            this.PaymentDue = copyAccountToIncentive.PaymentDue;
            this.PaymentDueDate = copyAccountToIncentive.PaymentDueDate;
            this.PaymentPaid = copyAccountToIncentive.PaymentPaid;
            this.PaymentPaidDate = copyAccountToIncentive.PaymentPaidDate;
            this.CreditDue = copyAccountToIncentive.CreditDue;
            this.CreditDueDate = copyAccountToIncentive.CreditDueDate;
            this.CreditPaid = copyAccountToIncentive.CreditPaid;
            this.CreditPaidDate = copyAccountToIncentive.CreditPaidDate;
            this.AccountToServiceId = copyAccountToIncentive.AccountToServiceId;
            this.AccountToService = new AccountToService();
        }
        public int PKId { get; set; }
        public decimal PaymentDue { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal PaymentPaid { get; set; }
        public DateTime PaymentPaidDate { get; set; }
        public decimal CreditDue { get; set; }
        public DateTime CreditDueDate { get; set; }
        public decimal CreditPaid { get; set; }
        public DateTime CreditPaidDate { get; set; }
        public int AccountToServiceId { get; set; }

        public virtual AccountToService AccountToService { get; set; }
        //clues respondwithlist about the specific list to load and display
        public const string PaymentList = "clubpaymentlist";
    }
}
