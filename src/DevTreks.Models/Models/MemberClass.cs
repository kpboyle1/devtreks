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
        public MemberClass(bool init)
        {
            this.PKId = 0;
            this.MemberClassNum = string.Empty;
            this.MemberClassName = string.Empty;
            this.MemberClassDesc = string.Empty;
            this.Member = new List<Member>();
        }
        public MemberClass(MemberClass memberGroup)
        {
            this.PKId = memberGroup.PKId;
            this.MemberClassNum = memberGroup.MemberClassNum;
            this.MemberClassName = memberGroup.MemberClassName;
            this.MemberClassDesc = memberGroup.MemberClassDesc;
            this.Member = new List<Member>();
        }
        public int PKId { get; set; }
        public string MemberClassDesc { get; set; }
        public string MemberClassName { get; set; }
        public string MemberClassNum { get; set; }

        public virtual ICollection<Member> Member { get; set; }
    }
}
