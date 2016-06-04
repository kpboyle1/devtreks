using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class GeoRegion
    {
        public GeoRegion()
        {
            Account = new HashSet<Account>();
            Member = new HashSet<Member>();
            Network = new HashSet<Network>();
        }
        public GeoRegion(GeoRegion geoRegion)
        {
            this.PKId = geoRegion.PKId;
            this.GeoRegionNum = geoRegion.GeoRegionNum;
            this.GeoRegionName = geoRegion.GeoRegionName;
            this.GeoRegionDesc = geoRegion.GeoRegionDesc;
        }
        public int PKId { get; set; }
        public string GeoRegionDesc { get; set; }
        public string GeoRegionName { get; set; }
        public string GeoRegionNum { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<Network> Network { get; set; }
    }
}
