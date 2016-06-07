using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class OutputClass
    {
        public OutputClass()
        {
            LinkedViewToOutputClass = new HashSet<LinkedViewToOutputClass>();
            Output = new HashSet<Output>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<LinkedViewToOutputClass> LinkedViewToOutputClass { get; set; }
        public virtual ICollection<Output> Output { get; set; }
        public virtual Service Service { get; set; }
        public virtual OutputType OutputType { get; set; }
    }
}
