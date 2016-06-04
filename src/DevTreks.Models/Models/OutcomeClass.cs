using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutcomeClass
    {
        public OutcomeClass()
        {
            LinkedViewToOutcomeClass = new HashSet<LinkedViewToOutcomeClass>();
            Outcome = new HashSet<Outcome>();
        }
        public OutcomeClass(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.DocStatus = 0;
            this.ServiceId = 0;
            this.Service = new Service();
            this.TypeId = 0;
            this.OutcomeType = new OutcomeType();
            this.Outcome = new List<Outcome>();
            this.LinkedViewToOutcomeClass = new List<LinkedViewToOutcomeClass>();
        }
        public OutcomeClass(OutcomeClass operationClass)
        {
            this.PKId = operationClass.PKId;
            this.Num = operationClass.Num;
            this.Name = operationClass.Name;
            this.Description = operationClass.Description;
            this.DocStatus = operationClass.DocStatus;
            this.ServiceId = operationClass.ServiceId;
            this.Service = new Service();
            this.TypeId = operationClass.TypeId;
            this.OutcomeType = new OutcomeType();
            this.Outcome = new List<Outcome>();
            this.LinkedViewToOutcomeClass = new List<LinkedViewToOutcomeClass>();
        }
        public int PKId { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<LinkedViewToOutcomeClass> LinkedViewToOutcomeClass { get; set; }
        public virtual ICollection<Outcome> Outcome { get; set; }
        public virtual Service Service { get; set; }
        public virtual OutcomeType OutcomeType { get; set; }
    }
}
