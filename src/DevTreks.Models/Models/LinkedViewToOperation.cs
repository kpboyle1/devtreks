using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class LinkedViewToOperation
    {
        public int PKId { get; set; }
        public bool IsDefaultLinkedView { get; set; }
        public int LinkedViewId { get; set; }
        public string LinkedViewName { get; set; }
        public int LinkingNodeId { get; set; }
        public string LinkingXmlDoc { get; set; }

        public virtual LinkedView LinkedView { get; set; }
        public virtual Operation Operation { get; set; }
    }
}
