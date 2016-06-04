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
        public LinkedViewPack(bool init)
        {
            PKId = 0;
            LinkedViewPackNum = General.NONE;
            LinkedViewPackName = General.NONE;
            LinkedViewPackDesc = General.NONE;
            LinkedViewPackKeywords = General.NONE;
            LinkedViewPackDocStatus = 0;
            LinkedViewPackLastChangedDate = General.GetDateShortNow();
            LinkedViewPackMetaDataXml = string.Empty;
            LinkedViewClassId = 0;
            LinkedViewClass = new LinkedViewClass();
            LinkedView = new List<LinkedView>();
        }
        public LinkedViewPack(LinkedViewPack rt)
        {
            PKId = rt.PKId;
            LinkedViewPackNum = rt.LinkedViewPackNum;
            LinkedViewPackName = rt.LinkedViewPackName;
            LinkedViewPackDesc = rt.LinkedViewPackDesc;
            LinkedViewPackKeywords = rt.LinkedViewPackKeywords;
            LinkedViewPackDocStatus = rt.LinkedViewPackDocStatus;
            LinkedViewPackLastChangedDate = rt.LinkedViewPackLastChangedDate;
            LinkedViewPackMetaDataXml = rt.LinkedViewPackMetaDataXml;
            LinkedViewClassId = rt.LinkedViewClassId;
            LinkedViewClass = new LinkedViewClass();
            LinkedView = new List<LinkedView>();
        }
        public int PKId { get; set; }
        public int LinkedViewClassId { get; set; }
        public string LinkedViewPackDesc { get; set; }
        public short LinkedViewPackDocStatus { get; set; }
        public string LinkedViewPackKeywords { get; set; }
        public DateTime LinkedViewPackLastChangedDate { get; set; }
        public string LinkedViewPackName { get; set; }
        public string LinkedViewPackNum { get; set; }
        public string LinkedViewPackMetaDataXml { get; set; }

        public virtual ICollection<LinkedView> LinkedView { get; set; }
        public virtual LinkedViewClass LinkedViewClass { get; set; }
    }
}
