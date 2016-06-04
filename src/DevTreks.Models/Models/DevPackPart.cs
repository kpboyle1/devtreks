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
        public DevPackPart(bool init)
        {
            PKId = 0;
            DevPackPartNum = General.NONE;
            DevPackPartName = General.NONE;
            DevPackPartDesc = General.NONE;
            DevPackPartKeywords = General.NONE;
            DevPackPartLastChangedDate = General.GetDateShortNow();
            DevPackPartFileName = General.NONE;
            DevPackPartXmlDoc = string.Empty;
            DevPackPartVirtualURIPattern = General.NONE;

            DevPackToDevPackPart = new List<DevPackToDevPackPart>();
        }
        public DevPackPart(DevPackPart rt)
        {
            PKId = rt.PKId;
            DevPackPartNum = rt.DevPackPartNum;
            DevPackPartName = rt.DevPackPartName;
            DevPackPartDesc = rt.DevPackPartDesc;
            DevPackPartKeywords = rt.DevPackPartKeywords;
            DevPackPartLastChangedDate = rt.DevPackPartLastChangedDate;
            DevPackPartFileName = rt.DevPackPartFileName;
            DevPackPartXmlDoc = rt.DevPackPartXmlDoc;
            DevPackPartVirtualURIPattern = rt.DevPackPartVirtualURIPattern;

            DevPackToDevPackPart = new List<DevPackToDevPackPart>();
        }
        public int PKId { get; set; }
        public string DevPackPartDesc { get; set; }
        public string DevPackPartFileName { get; set; }
        public string DevPackPartKeywords { get; set; }
        public DateTime DevPackPartLastChangedDate { get; set; }
        public string DevPackPartName { get; set; }
        public string DevPackPartNum { get; set; }
        public string DevPackPartXmlDoc { get; set; }
        public string DevPackPartVirtualURIPattern { get; set; }


        public virtual ICollection<DevPackToDevPackPart> DevPackToDevPackPart { get; set; }
    }
}
