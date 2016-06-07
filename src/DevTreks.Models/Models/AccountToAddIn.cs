using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountToAddIn
    {
        public AccountToAddIn() { }
        public AccountToAddIn(bool init)
        {
            this.PKId = 0;
            this.LinkedViewName = string.Empty;
            this.LinkingNodeId = 0;
            this.LinkedViewId = 0;
            this.IsDefaultLinkedView = false;
            this.Account = new Account();
            this.LinkedView = new LinkedView();
        }
        public AccountToAddIn(AccountToAddIn copyAccountToAddIn)
        {
            this.PKId = copyAccountToAddIn.PKId;
            this.LinkedViewName = copyAccountToAddIn.LinkedViewName;
            this.LinkingNodeId = copyAccountToAddIn.LinkingNodeId;
            this.LinkedViewId = copyAccountToAddIn.LinkedViewId;
            this.IsDefaultLinkedView = copyAccountToAddIn.IsDefaultLinkedView;
            this.Account = new Account();
            this.LinkedView = new LinkedView();
        }
        public int PKId { get; set; }
        public string LinkedViewName { get; set; }
        public int LinkingNodeId { get; set; }
        public int LinkedViewId { get; set; }
        public bool IsDefaultLinkedView { get; set; }

        public virtual LinkedView LinkedView { get; set; }
        //stay consistent with rest of admin models
        public virtual Account Account { get; set; }
        //actual
        //public virtual Account LinkingNode { get; set; }
    }
}
