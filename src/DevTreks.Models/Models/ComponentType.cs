using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class ComponentType
    {
        public ComponentType()
        {
            ComponentClass = new HashSet<ComponentClass>();
        }
        public ComponentType(bool init)
        {
            this.PKId = 0;
            this.Label = General.NONE;
            this.Name = General.NONE;
            this.NetworkId = 0;
            this.ServiceClassId = 0;
            this.ComponentClass = new List<ComponentClass>();
        }
        public ComponentType(ComponentType rt)
        {
            this.PKId = rt.PKId;
            this.Label = rt.Label;
            this.Name = rt.Name;
            this.NetworkId = rt.NetworkId;
            this.ServiceClassId = rt.ServiceClassId;
            this.ComponentClass = new List<ComponentClass>();
        }
        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<ComponentClass> ComponentClass { get; set; }
    }
}
