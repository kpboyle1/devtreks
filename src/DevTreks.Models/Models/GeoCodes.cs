using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class GeoCodes
    {
        public GeoCodes() { }
        public GeoCodes(bool init)
        {
            this.PKId = 0;
            this.ParentId = 0;
            this.GeoCodeNameId = string.Empty;
            this.GeoCodeParentNameId = string.Empty;
            this.GeoName = string.Empty;
            this.GeoDesc = string.Empty;
            this.NodeType = string.Empty;
            this.TocPath = string.Empty;
            this.TocParentPath = string.Empty;
            this.URI = string.Empty;

            this.GeoCodes1 = new List<GeoCodes>();
            this.GeoCode1 = new GeoCodes();
        }
        public int PKId { get; set; }
        public string GeoCodeNameId { get; set; }
        public string GeoCodeParentNameId { get; set; }
        public string GeoDesc { get; set; }
        public string GeoName { get; set; }
        public string NodeType { get; set; }
        public int ParentId { get; set; }
        public string TocParentPath { get; set; }
        public string TocPath { get; set; }
        public string URI { get; set; }

        public virtual GeoCodes GeoCode1 { get; set; }
        public virtual ICollection<GeoCodes> GeoCodes1 { get; set; }
        //public virtual ICollection<GeoCodes> InverseParent { get; set; }
    }
}
