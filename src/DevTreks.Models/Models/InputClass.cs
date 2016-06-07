using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class InputClass
    {
        public InputClass()
        {
            Input = new HashSet<Input>();
            LinkedViewToInputClass = new HashSet<LinkedViewToInputClass>();
        }

        public int PKId { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public short DocStatus { get; set; }
        public int ServiceId { get; set; }

        public virtual ICollection<Input> Input { get; set; }
        public virtual ICollection<LinkedViewToInputClass> LinkedViewToInputClass { get; set; }
        public virtual Service Service { get; set; }
        public virtual InputType InputType { get; set; }
    }
}
