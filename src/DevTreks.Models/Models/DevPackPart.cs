using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackPart
    {
        public DevPackPart()
        {
            DevPackToDevPackPart = new HashSet<DevPackToDevPackPart>();
        }

        public int PKId { get; set; }
        public string DevPackPartNum { get; set; }
        public string DevPackPartName { get; set; }
        public string DevPackPartDesc { get; set; }
        public string DevPackPartKeywords { get; set; }
        public DateTime DevPackPartLastChangedDate { get; set; }
        public string DevPackPartFileName { get; set; }
        public string DevPackPartXmlDoc { get; set; }
        public string DevPackPartVirtualURIPattern { get; set; }

        public virtual ICollection<DevPackToDevPackPart> DevPackToDevPackPart { get; set; }
    }
}
