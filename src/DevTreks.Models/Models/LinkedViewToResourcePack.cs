using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class LinkedViewToResourcePack
    {
        public int PKId { get; set; }
        public string SortLabel { get; set; }
        public int ResourcePackId { get; set; }
        public int LinkedViewId { get; set; }

        public virtual LinkedView LinkedView { get; set; }
        public virtual ResourcePack ResourcePack { get; set; }
    }
}
