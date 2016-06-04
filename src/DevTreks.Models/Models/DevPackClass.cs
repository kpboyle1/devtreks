using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackClass
    {
        public DevPackClass()
        {
            DevPackClassToDevPack = new HashSet<DevPackClassToDevPack>();
        }
        public DevPackClass(bool init)
        {
            PKId = 0;
            DevPackClassNum = General.NONE;
            DevPackClassName = General.NONE;
            DevPackClassDesc = General.NONE;
            TypeId = 0;
            DevPackType = new DevPackType();
            ServiceId = 0;
            Service = new Service();
            DevPackClassToDevPack = new List<DevPackClassToDevPack>();
        }
        public DevPackClass(DevPackClass rt)
        {
            PKId = rt.PKId;
            DevPackClassNum = rt.DevPackClassNum;
            DevPackClassName = rt.DevPackClassName;
            DevPackClassDesc = rt.DevPackClassDesc;
            TypeId = rt.TypeId;
            DevPackType = new DevPackType();
            ServiceId = rt.ServiceId;
            Service = new Service();
            DevPackClassToDevPack = new List<DevPackClassToDevPack>();
        }
        public int PKId { get; set; }
        public string DevPackClassDesc { get; set; }
        public string DevPackClassName { get; set; }
        public string DevPackClassNum { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<DevPackClassToDevPack> DevPackClassToDevPack { get; set; }
        public virtual Service Service { get; set; }
        public virtual DevPackType DevPackType { get; set; }
    }
}
