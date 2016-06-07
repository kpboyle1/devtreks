using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutcomeType
    {
        public OutcomeType()
        {
            OutcomeClass = new HashSet<OutcomeClass>();
        }

        public int PKId { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int NetworkId { get; set; }
        public int ServiceClassId { get; set; }

        public virtual ICollection<OutcomeClass> OutcomeClass { get; set; }
    }
}
