using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTreks.Models
{
    public partial class AccountToMember
    {
        public AccountToMember() { }
        public AccountToMember(bool init)
        {
            this.PKId = 0;
            this.MemberRole = string.Empty;
            this.IsDefaultClub = false;
            this.AccountId = 0;
            this.ClubDefault = new Account();
            this.MemberId = 0;
            this.Member = new Member();
            //nondb
            this.ClubInUse = new Account();
            this.AuthorizationLevel = 0;
            this.MemberDocFullPath = string.Empty;
            this.URIFull = string.Empty;
        }
        public AccountToMember(AccountToMember clubMember)
        {
            this.PKId = clubMember.PKId;
            this.MemberRole = clubMember.MemberRole;
            this.IsDefaultClub = clubMember.IsDefaultClub;
            this.AccountId = clubMember.AccountId;
            this.ClubDefault = new Account();
            this.MemberId = clubMember.MemberId;
            this.Member = new Member();
            //nondb
            this.ClubInUse = new Account();
            this.AuthorizationLevel = clubMember.AuthorizationLevel;
            this.MemberDocFullPath = (clubMember.MemberDocFullPath == null)
                ? string.Empty : clubMember.MemberDocFullPath;
            this.URIFull = (clubMember.URIFull == null)
                ? string.Empty : clubMember.URIFull;
        }
        public int PKId { get; set; }
        public bool IsDefaultClub { get; set; }
        public string MemberRole { get; set; }
        public int AccountId { get; set; }
        public int MemberId { get; set; }

        public virtual Account ClubDefault { get; set; }
        public virtual Member Member { get; set; }
        [NotMapped]
        public virtual Account ClubInUse { get; set; }
        [NotMapped]
        public virtual int AuthorizationLevel { get; set; }
        [NotMapped]
        public virtual string MemberDocFullPath { get; set; }
        [NotMapped]
        public string URIFull { get; set; }
    }
}
