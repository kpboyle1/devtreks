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
        public ComponentClass(bool init)
        {
            this.PKId = 0;
            this.Num = General.NONE;
            this.Name = General.NONE;
            this.Description = General.NONE;
            this.DocStatus = 0;
            this.ServiceId = 0;
            this.Service = new Service();
            this.TypeId = 0;
            this.ComponentType = new ComponentType();
            this.Component = new List<Component>();
            this.LinkedViewToComponentClass = new List<LinkedViewToComponentClass>();
        }
        public ComponentClass(ComponentClass componentClass)
        {
            this.PKId = componentClass.PKId;
            this.Num = componentClass.Num;
            this.Name = componentClass.Name;
            this.Description = componentClass.Description;
            this.DocStatus = componentClass.DocStatus;
            this.ServiceId = componentClass.ServiceId;
            this.Service = new Service();
            this.TypeId = componentClass.TypeId;
            this.ComponentType = new ComponentType();
            this.Component = new List<Component>();
            this.LinkedViewToComponentClass = new List<LinkedViewToComponentClass>();
        }
        public int PKId { get; set; }
        public string Description { get; set; }
        public short DocStatus { get; set; }
        public string Name { get; set; }
        public string Num { get; set; }
        public bool PriceListYorN { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<Component> Component { get; set; }
        public virtual ICollection<LinkedViewToComponentClass> LinkedViewToComponentClass { get; set; }
        public virtual Service Service { get; set; }
        public virtual ComponentType ComponentType { get; set; }
    }
}
