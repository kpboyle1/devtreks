using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Unit
    {
        public Unit()
        {
            UnitConversion = new HashSet<UnitConversion>();
        }

        public int PKId { get; set; }
        public string UnitNameAbbrev { get; set; }
        public string UnitName { get; set; }
        public int UnitClassId { get; set; }

        public virtual ICollection<UnitConversion> UnitConversion { get; set; }
    }
}
