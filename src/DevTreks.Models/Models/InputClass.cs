using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class InputClass
    {
        public InputClass()
        {
            Input = new HashSet<Input>();
            LinkedViewToInputClass = new HashSet<LinkedViewToInputClass>();
        }
        public InputClass(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.DocStatus = 0;
            this.ServiceId = 0;
            this.Service = new Service();
            this.TypeId = 0;
            this.InputType = new InputType();
            this.Input = new List<Input>();
            this.LinkedViewToInputClass = new List<LinkedViewToInputClass>();
        }
        public InputClass(InputClass inputClass)
        {
            this.PKId = inputClass.PKId;
            this.Num = inputClass.Num;
            this.Name = inputClass.Name;
            this.Description = inputClass.Description;
            this.DocStatus = inputClass.DocStatus;
            this.ServiceId = inputClass.ServiceId;
            this.Service = new Service();
            this.TypeId = inputClass.TypeId;
            this.InputType = new InputType();
            this.Input = new List<Input>();
            this.LinkedViewToInputClass = new List<LinkedViewToInputClass>();
        }
        public int PKId { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<Input> Input { get; set; }
        public virtual ICollection<LinkedViewToInputClass> LinkedViewToInputClass { get; set; }
        public virtual Service Service { get; set; }
        public virtual InputType InputType { get; set; }
    }
}
