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

        public int PKId { get; set; }
        public string ResourceNum { get; set; }
        public string ResourceTagNameForApps { get; set; }
        public string ResourceName { get; set; }
        public string ResourceFileName { get; set; }
        public string ResourceShortDesc { get; set; }
        public string ResourceLongDesc { get; set; }
        public DateTime ResourceLastChangedDate { get; set; }
        public string ResourceGeneralType { get; set; }
        public string ResourceMimeType { get; set; }
        public string ResourceXml { get; set; }
        public byte[] ResourceBinary { get; set; }
        public int ResourcePackId { get; set; }

        public virtual ICollection<LinkedViewToResource> LinkedViewToResource { get; set; }
        public virtual ResourcePack ResourcePack { get; set; }
        [NotMapped]
        public string ResourcePath { get; set; }
    }
}
