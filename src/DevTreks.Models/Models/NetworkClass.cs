using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class NetworkClass
    {
        public NetworkClass()
        {
            Network = new HashSet<Network>();
        }
        public NetworkClass(bool init)
        {
            this.PKId = 0;
            this.NetworkClassLabel = string.Empty;
            this.NetworkClassName = string.Empty;
            this.NetworkClassDesc = string.Empty;
            this.NetworkClassControllerName = string.Empty;
            this.NetworkClassUserLanguage = string.Empty;
            this.Network = new List<Network>();
            this.IsSelected = false;
        }
        public NetworkClass(NetworkClass networkGroup)
        {
            this.PKId = networkGroup.PKId;
            this.NetworkClassLabel = networkGroup.NetworkClassLabel;
            this.NetworkClassName = networkGroup.NetworkClassName;
            this.NetworkClassDesc = networkGroup.NetworkClassDesc;
            this.NetworkClassControllerName = networkGroup.NetworkClassControllerName;
            this.NetworkClassUserLanguage = networkGroup.NetworkClassUserLanguage;
            this.Network = new List<Network>();
            //non db
            this.IsSelected = networkGroup.IsSelected;
        }
        public int PKId { get; set; }
        public string NetworkClassLabel { get; set; }
        public string NetworkClassName { get; set; }
        public string NetworkClassDesc { get; set; }
        public string NetworkClassControllerName { get; set; }
        public string NetworkClassUserLanguage { get; set; }

        public virtual ICollection<Network> Network { get; set; }
        [NotMapped]
        public virtual bool IsSelected { get; set; }
    }
}
