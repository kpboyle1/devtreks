using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPack
    {
        public DevPack()
        {
            DevPackClassToDevPack = new HashSet<DevPackClassToDevPack>();
        }
        public DevPack(bool init)
        {
            PKId = 0;
            DevPackNum = General.NONE;
            DevPackName = General.NONE;
            DevPackDesc = General.NONE;
            DevPackDocStatus = 0;
            DevPackKeywords = General.NONE;
            DevPackLastChangedDate = General.GetDateShortNow();
            DevPackMetaDataXml = string.Empty;

            DevPackClassToDevPack = new List<DevPackClassToDevPack>();
        }
        public DevPack(DevPack rt)
        {
            PKId = rt.PKId;
            DevPackNum = rt.DevPackNum;
            DevPackName = rt.DevPackName;
            DevPackDesc = rt.DevPackDesc;
            DevPackDocStatus = rt.DevPackDocStatus;
            DevPackKeywords = rt.DevPackKeywords;
            DevPackLastChangedDate = rt.DevPackLastChangedDate;
            DevPackMetaDataXml = rt.DevPackMetaDataXml;

            DevPackClassToDevPack = new List<DevPackClassToDevPack>();
        }
        public int PKId { get; set; }
        public string DevPackDesc { get; set; }
        public short DevPackDocStatus { get; set; }
        public string DevPackKeywords { get; set; }
        public DateTime DevPackLastChangedDate { get; set; }
        public string DevPackName { get; set; }
        public string DevPackNum { get; set; }
        public string DevPackMetaDataXml { get; set; }

        public virtual ICollection<DevPackClassToDevPack> DevPackClassToDevPack { get; set; }
    }
}
