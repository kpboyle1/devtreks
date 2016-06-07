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

        public int PKId { get; set; }
        public string ResourceClassNum { get; set; }
        public string ResourceClassName { get; set; }
        public string ResourceClassDesc { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<ResourcePack> ResourcePack { get; set; }
        public virtual Service Service { get; set; }
        public virtual ResourceType ResourceType { get; set; }
    }
}
