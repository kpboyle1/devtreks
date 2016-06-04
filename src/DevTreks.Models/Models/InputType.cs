using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class InputType
    {
        public InputType()
        {
            InputClass = new HashSet<InputClass>();
        }
        public InputType(bool init)
        {
            this.PKId = 0;
            this.Label = General.NONE;
            this.Name = General.NONE;
            this.NetworkId = 0;
            this.ServiceClassId = 0;
            this.InputClass = new List<InputClass>();
        }
        public InputType(InputType rt)
        {
            this.PKId = rt.PKId;
            this.Label = rt.Label;
            this.Name = rt.Name;
            this.NetworkId = rt.NetworkId;
            this.ServiceClassId = rt.ServiceClassId;
            this.InputClass = new List<InputClass>();
        }
        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<InputClass> InputClass { get; set; }
    }
}