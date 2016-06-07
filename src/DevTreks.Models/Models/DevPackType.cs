using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class DevPackType
    {
        public DevPackType()
        {
            DevPackClass = new HashSet<DevPackClass>();
        }

        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<DevPackClass> DevPackClass { get; set; }
    }
}
