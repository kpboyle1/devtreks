﻿using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class LinkedViewToOutput
    {
        public int PKId { get; set; }
        public string LinkedViewName { get; set; }
        public int LinkingNodeId { get; set; }
        public int LinkedViewId { get; set; }
        public bool IsDefaultLinkedView { get; set; }
        public string LinkingXmlDoc { get; set; }

        public virtual LinkedView LinkedView { get; set; }
        public virtual Output Output { get; set; }
    }
}
