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

        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<OperationClass> OperationClass { get; set; }
    }
}
