using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class LinkedViewPack
    {
        public LinkedViewPack()
        {
            LinkedView = new HashSet<LinkedView>();
        }

        public int PKId { get; set; }
        public string LinkedViewPackNum { get; set; }
        public string LinkedViewPackName { get; set; }
        public string LinkedViewPackDesc { get; set; }
        public string LinkedViewPackKeywords { get; set; }
        public short LinkedViewPackDocStatus { get; set; }
        public DateTime LinkedViewPackLastChangedDate { get; set; }
        public string LinkedViewPackMetaDataXml { get; set; }
        public int LinkedViewClassId { get; set; }

        public virtual ICollection<LinkedView> LinkedView { get; set; }
        public virtual LinkedViewClass LinkedViewClass { get; set; }
    }
}
