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
        public OperationClass(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.DocStatus = 0;
            this.ServiceId = 0;
            this.Service = new Service();
            this.TypeId = 0;
            this.OperationType = new OperationType();
            this.Operation = new List<Operation>();
            this.LinkedViewToOperationClass = new List<LinkedViewToOperationClass>();
        }
        public OperationClass(OperationClass operationClass)
        {
            this.PKId = operationClass.PKId;
            this.Num = operationClass.Num;
            this.Name = operationClass.Name;
            this.Description = operationClass.Description;
            this.DocStatus = operationClass.DocStatus;
            this.ServiceId = operationClass.ServiceId;
            this.Service = new Service();
            this.TypeId = operationClass.TypeId;
            this.OperationType = new OperationType();
            this.Operation = new List<Operation>();
            this.LinkedViewToOperationClass = new List<LinkedViewToOperationClass>();
        }
        public int PKId { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public bool PriceListYorN { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<LinkedViewToOperationClass> LinkedViewToOperationClass { get; set; }
        public virtual ICollection<Operation> Operation { get; set; }
        public virtual Service Service { get; set; }
        public virtual OperationType OperationType { get; set; }
    }
}
