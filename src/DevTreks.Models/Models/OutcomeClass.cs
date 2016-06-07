using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutcomeClass
    {
        public OutcomeClass()
        {
            LinkedViewToOutcomeClass = new HashSet<LinkedViewToOutcomeClass>();
            Outcome = new HashSet<Outcome>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<LinkedViewToOutcomeClass> LinkedViewToOutcomeClass { get; set; }
        public virtual ICollection<Outcome> Outcome { get; set; }
        public virtual Service Service { get; set; }
        public virtual OutcomeType OutcomeType { get; set; }
    }
}
