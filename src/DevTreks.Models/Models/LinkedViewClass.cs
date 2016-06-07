using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class LinkedViewClass
    {
        public LinkedViewClass()
        {
            LinkedViewPack = new HashSet<LinkedViewPack>();
        }

        public int PKId { get; set; }
        public string LinkedViewClassNum { get; set; }
        public string LinkedViewClassName { get; set; }
        public string LinkedViewClassDesc { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }

        public virtual ICollection<LinkedViewPack> LinkedViewPack { get; set; }
        public virtual Service Service { get; set; }
        public virtual LinkedViewType LinkedViewType { get; set; }
    }
}
