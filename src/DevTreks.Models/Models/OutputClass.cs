using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutputClass
    {
        public OutputClass()
        {
            LinkedViewToOutputClass = new HashSet<LinkedViewToOutputClass>();
            Output = new HashSet<Output>();
        }
        public OutputClass(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.DocStatus = 0;
            this.ServiceId = 0;
            this.Service = new Service();
            this.TypeId = 0;
            this.OutputType = new OutputType();
            this.Output = new List<Output>();
            this.LinkedViewToOutputClass = new List<LinkedViewToOutputClass>();
        }
        public OutputClass(OutputClass outputClass)
        {
            this.PKId = outputClass.PKId;
            this.Num = outputClass.Num;
            this.Name = outputClass.Name;
            this.Description = outputClass.Description;
            this.DocStatus = outputClass.DocStatus;
            this.ServiceId = outputClass.ServiceId;
            this.Service = new Service();
            this.TypeId = outputClass.TypeId;
            this.OutputType = new OutputType();
            this.Output = new List<Output>();
            this.LinkedViewToOutputClass = new List<LinkedViewToOutputClass>();
        }
        public int PKId { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<LinkedViewToOutputClass> LinkedViewToOutputClass { get; set; }
        public virtual ICollection<Output> Output { get; set; }
        public virtual Service Service { get; set; }
        public virtual OutputType OutputType { get; set; }
    }
}
