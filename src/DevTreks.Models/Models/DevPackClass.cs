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

        public int PKId { get; set; }
        public string DevPackClassNum { get; set; }
        public string DevPackClassName { get; set; }
        public string DevPackClassDesc { get; set; }
        public int TypeId { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<DevPackClassToDevPack> DevPackClassToDevPack { get; set; }
        public virtual Service Service { get; set; }
        public virtual DevPackType DevPackType { get; set; }
    }
}
