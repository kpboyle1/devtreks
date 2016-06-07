using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountToNetwork
    {
        public AccountToNetwork() { }
        public AccountToNetwork(bool init)
        {
            this.PKId = 0;
            this.IsDefaultNetwork = false;
            this.DefaultGetDataFromType = string.Empty;
            this.DefaultStoreDataAtType = string.Empty;
            this.NetworkRole = string.Empty;
            this.AccountId = 0;
            this.Account = new Account();
            this.NetworkId = 0;
            this.Network = new Network();
        }
        public AccountToNetwork(AccountToNetwork copyAccountToNetwork)
        {
            this.PKId = copyAccountToNetwork.PKId;
            this.IsDefaultNetwork = copyAccountToNetwork.IsDefaultNetwork;
            this.DefaultGetDataFromType = copyAccountToNetwork.DefaultGetDataFromType;
            this.DefaultStoreDataAtType = copyAccountToNetwork.DefaultStoreDataAtType;
            this.NetworkRole = copyAccountToNetwork.NetworkRole;

            this.AccountId = copyAccountToNetwork.AccountId;
            this.Account = new Account();
            this.NetworkId = copyAccountToNetwork.NetworkId;
            this.Network = new Network();
        }
        public int PKId { get; set; }
        public bool IsDefaultNetwork { get; set; }
        public string DefaultGetDataFromType { get; set; }
        public string DefaultStoreDataAtType { get; set; }
        public string NetworkRole { get; set; }
        public int AccountId { get; set; }
        public int NetworkId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Network Network { get; set; }
    }
}
