using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class MemberClass
    {
        public MemberClass()
        {
            Member = new HashSet<Member>();
        }

        public int PKId { get; set; }
        public string MemberClassNum { get; set; }
        public string MemberClassName { get; set; }
        public string MemberClassDesc { get; set; }

        public virtual ICollection<Member> Member { get; set; }
    }
}
