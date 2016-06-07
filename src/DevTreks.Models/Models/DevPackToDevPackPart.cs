using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackToDevPackPart
    {
        public DevPackToDevPackPart()
        {
            DevPackPartToResourcePack = new HashSet<DevPackPartToResourcePack>();
            LinkedViewToDevPackPartJoin = new HashSet<LinkedViewToDevPackPartJoin>();
        }

        public int PKId { get; set; }
        public string DevPackToDevPackPartSortLabel { get; set; }
        public string DevPackToDevPackPartName { get; set; }
        public string DevPackToDevPackPartDesc { get; set; }
        public string DevPackToDevPackPartFileExtensionType { get; set; }
        public int DevPackClassToDevPackId { get; set; }
        public int DevPackPartId { get; set; }
        //public int? DevPackClassToDevPackId { get; set; }
        //public int? DevPackPartId { get; set; }

        public virtual ICollection<DevPackPartToResourcePack> DevPackPartToResourcePack { get; set; }
        public virtual ICollection<LinkedViewToDevPackPartJoin> LinkedViewToDevPackPartJoin { get; set; }
        public virtual DevPackClassToDevPack DevPackClassToDevPack { get; set; }
        public virtual DevPackPart DevPackPart { get; set; }
    }
}
