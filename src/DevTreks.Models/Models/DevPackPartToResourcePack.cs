using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackPartToResourcePack
    {
        public DevPackPartToResourcePack(){ }
        public DevPackPartToResourcePack(bool init)
        {
            PKId = 0;
            SortLabel = General.NONE;
            ResourcePackId = 0;
            ResourcePack = new ResourcePack();
            DevPackToDevPackPartId = 0;
            DevPackToDevPackPart = new DevPackToDevPackPart();
        }
        public DevPackPartToResourcePack(DevPackPartToResourcePack rt)
        {
            PKId = rt.PKId;
            SortLabel = rt.SortLabel;
            ResourcePackId = rt.ResourcePackId;
            ResourcePack = new ResourcePack();
            DevPackToDevPackPartId = rt.DevPackToDevPackPartId;
            DevPackToDevPackPart = new DevPackToDevPackPart();
        }
        public int PKId { get; set; }
        public int DevPackToDevPackPartId { get; set; }
        public int ResourcePackId { get; set; }
        public string SortLabel { get; set; }

        public virtual DevPackToDevPackPart DevPackToDevPackPart { get; set; }
        public virtual ResourcePack ResourcePack { get; set; }
    }
}
