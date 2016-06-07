using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class ComponentClass
    {
        public ComponentClass()
        {
            Component = new HashSet<Component>();
            LinkedViewToComponentClass = new HashSet<LinkedViewToComponentClass>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool PriceListYorN { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<Component> Component { get; set; }
        public virtual ICollection<LinkedViewToComponentClass> LinkedViewToComponentClass { get; set; }
        public virtual Service Service { get; set; }
        public virtual ComponentType ComponentType { get; set; }
    }
}
