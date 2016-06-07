using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPack
    {
        public DevPack()
        {
            DevPackClassToDevPack = new HashSet<DevPackClassToDevPack>();
        }

        public int PKId { get; set; }
        public string DevPackNum { get; set; }
        public string DevPackName { get; set; }
        public string DevPackDesc { get; set; }
        public short DevPackDocStatus { get; set; }
        public string DevPackKeywords { get; set; }
        public DateTime DevPackLastChangedDate { get; set; }
        public string DevPackMetaDataXml { get; set; }

        public virtual ICollection<DevPackClassToDevPack> DevPackClassToDevPack { get; set; }
    }
}
