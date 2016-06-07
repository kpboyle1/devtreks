using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class ResourcePack
    {
        public ResourcePack()
        {
            DevPackPartToResourcePack = new HashSet<DevPackPartToResourcePack>();
            LinkedViewToResourcePack = new HashSet<LinkedViewToResourcePack>();
            Resource = new HashSet<Resource>();
        }

        public int PKId { get; set; }
        public string ResourcePackNum { get; set; }
        public string ResourcePackName { get; set; }
        public string ResourcePackDesc { get; set; }
        public string ResourcePackKeywords { get; set; }
        public short ResourcePackDocStatus { get; set; }
        public DateTime ResourcePackLastChangedDate { get; set; }
        public string ResourcePackMetaDataXml { get; set; }
        public int ResourceClassId { get; set; }

        public virtual ICollection<DevPackPartToResourcePack> DevPackPartToResourcePack { get; set; }
        public virtual ICollection<LinkedViewToResourcePack> LinkedViewToResourcePack { get; set; }
        public virtual ICollection<Resource> Resource { get; set; }
        public virtual ResourceClass ResourceClass { get; set; }
    }
}
