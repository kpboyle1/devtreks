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
        public ResourcePack(bool init)
        {
            this.PKId = 0;
            this.ResourcePackNum = General.NONE;
            this.ResourcePackName = General.NONE;
            this.ResourcePackDesc = General.NONE;
            this.ResourcePackKeywords = General.NONE;
            this.ResourcePackDocStatus = 0;
            this.ResourcePackLastChangedDate = General.GetDateShortNow();
            this.ResourcePackMetaDataXml = General.NONE;
            this.ResourceClassId = 0;
            this.ResourceClass = new ResourceClass();
            this.DevPackPartToResourcePack = new List<DevPackPartToResourcePack>();
            this.LinkedViewToResourcePack = new List<LinkedViewToResourcePack>();
            this.Resource = new List<Resource>();
        }
        public ResourcePack(ResourcePack rp)
        {
            this.PKId = rp.PKId;
            this.ResourcePackNum = rp.ResourcePackNum;
            this.ResourcePackName = rp.ResourcePackName;
            this.ResourcePackDesc = rp.ResourcePackDesc;
            this.ResourcePackKeywords = rp.ResourcePackKeywords;
            this.ResourcePackDocStatus = rp.ResourcePackDocStatus;
            this.ResourcePackLastChangedDate = rp.ResourcePackLastChangedDate;
            this.ResourcePackMetaDataXml = rp.ResourcePackMetaDataXml;
            this.ResourceClassId = rp.ResourceClassId;
            this.ResourceClass = new ResourceClass();
            this.DevPackPartToResourcePack = new List<DevPackPartToResourcePack>();
            this.LinkedViewToResourcePack = new List<LinkedViewToResourcePack>();
            this.Resource = new List<Resource>();
        }
        public int PKId { get; set; }
        public int ResourceClassId { get; set; }
        public string ResourcePackDesc { get; set; }
        public short ResourcePackDocStatus { get; set; }
        public string ResourcePackKeywords { get; set; }
        public DateTime ResourcePackLastChangedDate { get; set; }
        public string ResourcePackName { get; set; }
        public string ResourcePackNum { get; set; }
        public string ResourcePackMetaDataXml { get; set; }

        public virtual ICollection<DevPackPartToResourcePack> DevPackPartToResourcePack { get; set; }
        public virtual ICollection<LinkedViewToResourcePack> LinkedViewToResourcePack { get; set; }
        public virtual ICollection<Resource> Resource { get; set; }
        public virtual ResourceClass ResourceClass { get; set; }
    }
}
