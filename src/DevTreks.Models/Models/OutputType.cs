using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutputType
    {
        public OutputType()
        {
            OutputClass = new HashSet<OutputClass>();
        }
        public OutputType(bool init)
        {
            this.PKId = 0;
            this.Label = General.NONE;
            this.Name = General.NONE;
            this.NetworkId = 0;
            this.ServiceClassId = 0;
            this.OutputClass = new List<OutputClass>();
        }
        public OutputType(OutputType rt)
        {
            this.PKId = rt.PKId;
            this.Label = rt.Label;
            this.Name = rt.Name;
            this.NetworkId = rt.NetworkId;
            this.ServiceClassId = rt.ServiceClassId;
            this.OutputClass = new List<OutputClass>();
        }
        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<OutputClass> OutputClass { get; set; }
    }
}
