using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class GeoCodes
    {
        public int PKId { get; set; }
        public int ParentId { get; set; }
        public string GeoCodeNameId { get; set; }
        public string GeoCodeParentNameId { get; set; }
        public string GeoName { get; set; }
        public string GeoDesc { get; set; }
        public string NodeType { get; set; }
        public string TocPath { get; set; }
        public string TocParentPath { get; set; }
        public string URI { get; set; }
        public virtual GeoCodes GeoCode1 { get; set; }
        public virtual ICollection<GeoCodes> GeoCodes1 { get; set; }
        //public virtual GeoCodes Parent { get; set; }
        //public virtual ICollection<GeoCodes> InverseParent { get; set; }
    }
}
