using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class UnitConversion
    {
        public int PKId { get; set; }
        public int Unit1Id { get; set; }
        public int Unit2Id { get; set; }
        public float UnitConversionFactor { get; set; }
        public string UnitToName { get; set; }
        public bool IsBestConversion { get; set; }
        public int UnitToClassId { get; set; }

        public virtual Unit Unit1 { get; set; }
    }
}
