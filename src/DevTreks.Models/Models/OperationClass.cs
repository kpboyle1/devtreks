using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OperationClass
    {
        public OperationClass()
        {
            LinkedViewToOperationClass = new HashSet<LinkedViewToOperationClass>();
            Operation = new HashSet<Operation>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool PriceListYorN { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<LinkedViewToOperationClass> LinkedViewToOperationClass { get; set; }
        public virtual ICollection<Operation> Operation { get; set; }
        public virtual Service Service { get; set; }
        public virtual OperationType OperationType { get; set; }
    }
}
