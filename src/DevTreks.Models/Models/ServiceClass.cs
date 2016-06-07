using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class ServiceClass
    {
        public ServiceClass()
        {
            Service = new HashSet<Service>();
        }
        public ServiceClass(bool init)
        {
            this.PKId = 0;
            this.ServiceClassNum = string.Empty;
            this.ServiceClassName = string.Empty;
            this.ServiceClassDesc = string.Empty;
            this.Service = new List<Service>();
            //nondb
            this.IsSelected = false;
        }
        public ServiceClass(ServiceClass serviceGroup)
        {
            this.PKId = serviceGroup.PKId;
            this.ServiceClassNum = serviceGroup.ServiceClassNum;
            this.ServiceClassName = serviceGroup.ServiceClassName;
            this.ServiceClassDesc = serviceGroup.ServiceClassDesc;
            this.Service = new List<Service>();
            //nondb
            this.IsSelected = serviceGroup.IsSelected;
        }
        public int PKId { get; set; }
        public string ServiceClassNum { get; set; }
        public string ServiceClassName { get; set; }
        public string ServiceClassDesc { get; set; }

        public virtual ICollection<Service> Service { get; set; }
        [NotMapped]
        public virtual bool IsSelected { get; set; }
    }
}
