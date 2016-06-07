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

        public int PKId { get; set; }
        public string DevPackClassAndPackSortLabel { get; set; }
        public string DevPackClassAndPackName { get; set; }
        public string DevPackClassAndPackDesc { get; set; }
        public string DevPackClassAndPackFileExtensionType { get; set; }
        public int DevPackClassId { get; set; }
        public int DevPackId { get; set; }
        public int? ParentId { get; set; }
        //public Nullable<int> ParentId { get; set; }
        public virtual ICollection<DevPackToDevPackPart> DevPackToDevPackPart { get; set; }
        public virtual ICollection<LinkedViewToDevPackJoin> LinkedViewToDevPackJoin { get; set; }
        public virtual DevPackClass DevPackClass { get; set; }
        public virtual DevPack DevPack { get; set; }
        public virtual DevPackClassToDevPack DevPackClassToDevPack2 { get; set; }
        public virtual ICollection<DevPackClassToDevPack> DevPackClassToDevPack1 { get; set; }
        //public virtual DevPackClassToDevPack Parent { get; set; }
        //public virtual ICollection<DevPackClassToDevPack> InverseParent { get; set; }
    }
}
