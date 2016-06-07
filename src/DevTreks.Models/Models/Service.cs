using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class Service
    {
        public Service()
        {
            AccountToService = new HashSet<AccountToService>();
            BudgetSystem = new HashSet<BudgetSystem>();
            ComponentClass = new HashSet<ComponentClass>();
            CostSystem = new HashSet<CostSystem>();
            DevPackClass = new HashSet<DevPackClass>();
            InputClass = new HashSet<InputClass>();
            LinkedViewClass = new HashSet<LinkedViewClass>();
            OperationClass = new HashSet<OperationClass>();
            OutcomeClass = new HashSet<OutcomeClass>();
            OutputClass = new HashSet<OutputClass>();
            ResourceClass = new HashSet<ResourceClass>();
        }
        public Service(bool init)
        {
            this.PKId = 0;
            this.ServiceNum = string.Empty;
            this.ServiceName = string.Empty;
            this.ServiceDesc = string.Empty;
            this.ServicePrice1 = 0;
            this.ServiceUnit1 = string.Empty;
            this.ServiceCurrency1 = string.Empty;
            this.NetworkId = 0;
            this.Network = new Network();
            this.ServiceClassId = 0;
            this.ServiceClass = new ServiceClass();
            this.AccountToService = new List<AccountToService>();
            this.BudgetSystem = new List<BudgetSystem>();
            this.ComponentClass = new List<ComponentClass>();
            this.CostSystem = new List<CostSystem>();
            this.DevPackClass = new List<DevPackClass>();
            this.InputClass = new List<InputClass>();
            this.LinkedViewClass = new List<LinkedViewClass>();
            this.OperationClass = new List<OperationClass>();
            this.OutcomeClass = new List<OutcomeClass>();
            this.OutputClass = new List<OutputClass>();
            this.ResourceClass = new List<ResourceClass>();
        }
        public int PKId { get; set; }
        public string ServiceNum { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDesc { get; set; }
        public decimal ServicePrice1 { get; set; }
        public string ServiceUnit1 { get; set; }
        public string ServiceCurrency1 { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<AccountToService> AccountToService { get; set; }
        public virtual ICollection<BudgetSystem> BudgetSystem { get; set; }
        public virtual ICollection<ComponentClass> ComponentClass { get; set; }
        public virtual ICollection<CostSystem> CostSystem { get; set; }
        public virtual ICollection<DevPackClass> DevPackClass { get; set; }
        public virtual ICollection<InputClass> InputClass { get; set; }
        public virtual ICollection<LinkedViewClass> LinkedViewClass { get; set; }
        public virtual ICollection<OperationClass> OperationClass { get; set; }
        public virtual ICollection<OutcomeClass> OutcomeClass { get; set; }
        public virtual ICollection<OutputClass> OutputClass { get; set; }
        public virtual ICollection<ResourceClass> ResourceClass { get; set; }
        public virtual Network Network { get; set; }
        public virtual ServiceClass ServiceClass { get; set; }
        [NotMapped]
        public virtual bool IsSelected { get; set; }
        //clubs that have subscriptions to this service
        [NotMapped]
        public IList<AccountToService> SubscribedClubs { get; set; }
    }
}
