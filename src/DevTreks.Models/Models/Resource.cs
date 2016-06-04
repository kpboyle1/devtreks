using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class Resource
    {
        public Resource()
        {
            LinkedViewToResource = new HashSet<LinkedViewToResource>();
        }
        public Resource(bool init)
        {
            this.PKId = 0;
            this.ResourceNum = General.NONE;
            this.ResourceTagNameForApps = General.NONE;
            this.ResourceName = General.NONE;
            this.ResourceFileName = General.NONE;
            this.ResourceShortDesc = General.NONE;
            this.ResourceLongDesc = General.NONE;
            this.ResourceLastChangedDate = General.GetDateShortNow();
            this.ResourceGeneralType = General.NONE;
            this.ResourceMimeType = General.NONE;
            this.ResourcePackId = 0;
            this.ResourcePath = General.NONE;
            this.ResourcePack = new ResourcePack();
            this.LinkedViewToResource = new List<LinkedViewToResource>();
            //0.9.0 -not scalable when in model, use stored procs instead
            //this.ResourceXml = General.NONE;
            //this.ResourceBinary = null;
        }
        public Resource(Resource res)
        {
            this.PKId = res.PKId;
            this.ResourceNum = res.ResourceNum;
            this.ResourceTagNameForApps = res.ResourceTagNameForApps;
            this.ResourceName = res.ResourceName;
            this.ResourceFileName = res.ResourceFileName;
            this.ResourceShortDesc = res.ResourceShortDesc;
            this.ResourceLongDesc = res.ResourceLongDesc;
            this.ResourceLastChangedDate = res.ResourceLastChangedDate;
            this.ResourceGeneralType = res.ResourceGeneralType;
            this.ResourceMimeType = res.ResourceMimeType;
            this.ResourcePackId = res.ResourcePackId;
            this.ResourcePath = res.ResourcePath;
            this.ResourcePack = new ResourcePack();
            this.LinkedViewToResource = new List<LinkedViewToResource>();
            //0.9.0 -not scalable when in model, use stored procs instead
            //this.ResourceXml = res.ResourceXml;
            //this.ResourceBinary = res.ResourceBinary;
        }
        public int PKId { get; set; }
        public byte[] ResourceBinary { get; set; }
        public string ResourceFileName { get; set; }
        public string ResourceGeneralType { get; set; }
        public DateTime ResourceLastChangedDate { get; set; }
        public string ResourceLongDesc { get; set; }
        public string ResourceMimeType { get; set; }
        public string ResourceName { get; set; }
        public string ResourceNum { get; set; }
        public int ResourcePackId { get; set; }
        public string ResourceShortDesc { get; set; }
        public string ResourceTagNameForApps { get; set; }

        public virtual ICollection<LinkedViewToResource> LinkedViewToResource { get; set; }
        public virtual ResourcePack ResourcePack { get; set; }
        [NotMapped]
        public string ResourcePath { get; set; }
    }
}
