using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackPartToResourcePack
    {
        public int PKId { get; set; }
        public string SortLabel { get; set; }
        public int ResourcePackId { get; set; }
        public int DevPackToDevPackPartId { get; set; }

        public virtual DevPackToDevPackPart DevPackToDevPackPart { get; set; }
        public virtual ResourcePack ResourcePack { get; set; }
    }
}
