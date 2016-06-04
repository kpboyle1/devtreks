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
        public DevPackToDevPackPart(bool init)
        {
            PKId = 0;
            DevPackToDevPackPartSortLabel = General.NONE;
            DevPackToDevPackPartName = General.NONE;
            DevPackToDevPackPartDesc = General.NONE;
            DevPackToDevPackPartFileExtensionType = General.NONE;
            DevPackClassToDevPackId = 0;
            DevPackClassToDevPack = new DevPackClassToDevPack();
            DevPackPartId = 0;
            DevPackPart = new DevPackPart();
            DevPackPartToResourcePack = new List<DevPackPartToResourcePack>();
            LinkedViewToDevPackPartJoin = new List<LinkedViewToDevPackPartJoin>();
        }
        public DevPackToDevPackPart(DevPackToDevPackPart rt)
        {
            PKId = rt.PKId;
            DevPackToDevPackPartSortLabel = rt.DevPackToDevPackPartSortLabel;
            DevPackToDevPackPartName = rt.DevPackToDevPackPartName;
            DevPackToDevPackPartDesc = rt.DevPackToDevPackPartDesc;
            DevPackToDevPackPartFileExtensionType = rt.DevPackToDevPackPartFileExtensionType;
            DevPackClassToDevPackId = rt.DevPackClassToDevPackId;
            DevPackClassToDevPack = new DevPackClassToDevPack();
            DevPackPartId = rt.DevPackPartId;
            DevPackPart = new DevPackPart();
            DevPackPartToResourcePack = new List<DevPackPartToResourcePack>();
            LinkedViewToDevPackPartJoin = new List<LinkedViewToDevPackPartJoin>();
        }
        public int PKId { get; set; }
        public int DevPackClassToDevPackId { get; set; }
        public int DevPackPartId { get; set; }
        public string DevPackToDevPackPartDesc { get; set; }
        public string DevPackToDevPackPartFileExtensionType { get; set; }
        public string DevPackToDevPackPartName { get; set; }
        public string DevPackToDevPackPartSortLabel { get; set; }

        public virtual ICollection<DevPackPartToResourcePack> DevPackPartToResourcePack { get; set; }
        public virtual ICollection<LinkedViewToDevPackPartJoin> LinkedViewToDevPackPartJoin { get; set; }
        public virtual DevPackClassToDevPack DevPackClassToDevPack { get; set; }
        public virtual DevPackPart DevPackPart { get; set; }
    }
}
