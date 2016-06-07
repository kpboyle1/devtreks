using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class Network
    {
        public Network()
        {
            AccountToNetwork = new HashSet<AccountToNetwork>();
            Service = new HashSet<Service>();
        }
        public Network(bool init)
        {
            this.PKId = 0;
            this.NetworkName = string.Empty;
            this.NetworkURIPartName = string.Empty;
            this.NetworkDesc = string.Empty;
            this.AdminConnection = string.Empty;
            this.WebFileSystemPath = string.Empty;
            this.WebConnection = string.Empty;
            this.WebDbPath = string.Empty;
            this.NetworkClassId = 0;
            this.NetworkClass = new NetworkClass();
            this.GeoRegionId = 0;
            this.IsDefault = false;
            this.URIFull = string.Empty;
            this.XmlConnection = string.Empty;
            this.AccountToNetwork = new List<AccountToNetwork>();
            this.Service = new List<Service>();
        }

        public Network(Network network)
        {
            this.PKId = network.PKId;
            this.NetworkName = network.NetworkName;
            this.NetworkURIPartName = network.NetworkURIPartName;
            this.NetworkDesc = network.NetworkDesc;
            this.AdminConnection = network.AdminConnection;
            this.WebFileSystemPath = network.WebFileSystemPath;
            this.WebConnection = network.WebConnection;
            this.WebDbPath = network.WebDbPath;
            this.NetworkClassId = network.NetworkClassId;
            this.NetworkClass = new NetworkClass();
            this.GeoRegionId = network.GeoRegionId;
            this.IsDefault = network.IsDefault;
            this.URIFull = network.URIFull;
            this.XmlConnection = network.XmlConnection;
            this.AccountToNetwork = new List<AccountToNetwork>();
            this.Service = new List<Service>();
        }
        public int PKId { get; set; }
        public string NetworkName { get; set; }
        public string NetworkURIPartName { get; set; }
        public string NetworkDesc { get; set; }
        public string AdminConnection { get; set; }
        public string WebFileSystemPath { get; set; }
        public string WebConnection { get; set; }
        public string WebDbPath { get; set; }
        public int NetworkClassId { get; set; }
        public int GeoRegionId { get; set; }

        public virtual ICollection<AccountToNetwork> AccountToNetwork { get; set; }
        public virtual ICollection<Service> Service { get; set; }
        public virtual GeoRegion GeoRegion { get; set; }
        public virtual NetworkClass NetworkClass { get; set; }
        [NotMapped]
        public virtual bool IsDefault { get; set; }
        [NotMapped]
        public string URIFull { get; set; }
        [NotMapped] //being phased out in beta 0.8.2
        public string XmlConnection { get; set; }
    }
}
