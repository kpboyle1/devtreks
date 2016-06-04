using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackClassToDevPack
    {
        public DevPackClassToDevPack()
        {
            DevPackToDevPackPart = new HashSet<DevPackToDevPackPart>();
            LinkedViewToDevPackJoin = new HashSet<LinkedViewToDevPackJoin>();
        }
        public DevPackClassToDevPack(bool init)
        {
            PKId = 0;
            DevPackClassAndPackSortLabel = General.NONE;
            DevPackClassAndPackName = General.NONE;
            DevPackClassAndPackDesc = General.NONE;
            DevPackClassAndPackFileExtensionType = General.NONE;

            DevPackId = 0;
            DevPack = new DevPack();
            DevPackClassId = 0;
            DevPackClass = new DevPackClass();
            ParentId = null;
            DevPackClassToDevPack2 = new DevPackClassToDevPack();

            DevPackToDevPackPart = new List<DevPackToDevPackPart>();
            DevPackClassToDevPack1 = new List<DevPackClassToDevPack>();
            LinkedViewToDevPackJoin = new List<LinkedViewToDevPackJoin>();
        }
        public DevPackClassToDevPack(DevPackClassToDevPack rt)
        {
            PKId = rt.PKId;
            DevPackClassAndPackSortLabel = rt.DevPackClassAndPackSortLabel;
            DevPackClassAndPackName = rt.DevPackClassAndPackName;
            DevPackClassAndPackDesc = rt.DevPackClassAndPackDesc;
            DevPackClassAndPackFileExtensionType = rt.DevPackClassAndPackFileExtensionType;

            DevPackId = rt.DevPackId;
            DevPack = new DevPack();
            DevPackClassId = rt.DevPackClassId;
            DevPackClass = new DevPackClass();
            ParentId = rt.ParentId;
            DevPackClassToDevPack2 = new DevPackClassToDevPack();

            DevPackToDevPackPart = new List<DevPackToDevPackPart>();
            DevPackClassToDevPack1 = new List<DevPackClassToDevPack>();
            LinkedViewToDevPackJoin = new List<LinkedViewToDevPackJoin>();
        }
        public int PKId { get; set; }
        public string DevPackClassAndPackDesc { get; set; }
        public string DevPackClassAndPackFileExtensionType { get; set; }
        public string DevPackClassAndPackName { get; set; }
        public string DevPackClassAndPackSortLabel { get; set; }
        public int DevPackClassId { get; set; }
        public int DevPackId { get; set; }
        public Nullable<int> ParentId { get; set; }

        public virtual ICollection<DevPackToDevPackPart> DevPackToDevPackPart { get; set; }
        public virtual ICollection<LinkedViewToDevPackJoin> LinkedViewToDevPackJoin { get; set; }
        public virtual DevPackClass DevPackClass { get; set; }
        public virtual DevPack DevPack { get; set; }
        public virtual DevPackClassToDevPack DevPackClassToDevPack2 { get; set; }
        public virtual ICollection<DevPackClassToDevPack> DevPackClassToDevPack1 { get; set; }
    }
}
