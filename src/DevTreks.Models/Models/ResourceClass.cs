using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class ResourceClass
    {
        public ResourceClass()
        {
            ResourcePack = new HashSet<ResourcePack>();
        }
        public ResourceClass(bool init)
        {
            this.PKId = 0;
            this.ResourceClassNum = General.NONE;
            this.ResourceClassName = General.NONE;
            this.ResourceClassDesc = General.NONE;
            this.ServiceId = 0;
            this.Service = new Service();
            this.TypeId = 0;
            this.ResourceType = new ResourceType();
            this.ResourcePack = new List<ResourcePack>();
        }
        public ResourceClass(ResourceClass resourceClass)
        {
            this.PKId = resourceClass.PKId;
            this.ResourceClassNum = resourceClass.ResourceClassNum;
            this.ResourceClassName = resourceClass.ResourceClassName;
            this.ResourceClassDesc = resourceClass.ResourceClassDesc;
            this.ServiceId = resourceClass.ServiceId;
            this.Service = new Service();
            this.TypeId = resourceClass.TypeId;
            this.ResourceType = new ResourceType();
            this.ResourcePack = new List<ResourcePack>();
        }
        public int PKId { get; set; }
        public string ResourceClassDesc { get; set; }
        public string ResourceClassName { get; set; }
        public string ResourceClassNum { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<ResourcePack> ResourcePack { get; set; }
        public virtual Service Service { get; set; }
        public virtual ResourceType ResourceType { get; set; }
    }
}
