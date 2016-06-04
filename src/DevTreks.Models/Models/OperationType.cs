using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OperationType
    {
        public OperationType()
        {
            OperationClass = new HashSet<OperationClass>();
        }
        public OperationType(bool init)
        {
            this.PKId = 0;
            this.Label = General.NONE;
            this.Name = General.NONE;
            this.NetworkId = 0;
            this.ServiceClassId = 0;
            this.OperationClass = new List<OperationClass>();
        }
        public OperationType(OperationType rt)
        {
            this.PKId = rt.PKId;
            this.Label = rt.Label;
            this.Name = rt.Name;
            this.NetworkId = rt.NetworkId;
            this.ServiceClassId = rt.ServiceClassId;
            this.OperationClass = new List<OperationClass>();
        }
        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<OperationClass> OperationClass { get; set; }
    }
}
