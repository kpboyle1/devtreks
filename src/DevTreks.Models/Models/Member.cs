using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class Member
    {
        public Member()
        {
            AccountToMember = new HashSet<AccountToMember>();
        }
        public Member(bool init)
        {
            this.PKId = 0;
            this.UserName = string.Empty;
            this.AspNetUserId = string.Empty;
            this.MemberEmail = string.Empty;
            this.MemberJoinedDate = General.GetDateShortNow();
            this.MemberLastChangedDate = General.GetDateShortNow();
            this.MemberFirstName = string.Empty;
            this.MemberLastName = string.Empty;
            this.MemberDesc = string.Empty;
            this.MemberOrganization = string.Empty;
            this.MemberAddress1 = string.Empty;
            this.MemberAddress2 = string.Empty;
            this.MemberCity = string.Empty;
            this.MemberState = string.Empty;
            this.MemberCountry = string.Empty;
            this.MemberZip = string.Empty;
            this.MemberPhone = string.Empty;
            this.MemberPhone2 = string.Empty;
            this.MemberFax = string.Empty;
            this.MemberUrl = string.Empty;
            this.MemberClassId = 0;
            this.MemberClass = new MemberClass();
            this.GeoRegionId = 0;
            this.GeoRegion = new GeoRegion();
            this.AccountToMember = new List<AccountToMember>();
        }
        //used to insert new members using their aspnetuser
        public Member(string eMail, string aspNetUserId)
        {
            //the user name is used as a sql param in Data.AppHelpers.Member
            //to init the member and club. Do not allow edits.
            this.UserName = eMail;
            this.AspNetUserId = aspNetUserId;
            this.MemberEmail = eMail;
            this.MemberJoinedDate = General.GetDateShortNow();
            this.MemberLastChangedDate = General.GetDateShortNow();
            this.MemberFirstName = General.NONE;
            this.MemberLastName = General.NONE;
            this.MemberDesc = General.NONE;
            this.MemberOrganization = General.NONE;
            this.MemberAddress1 = General.NONE;
            this.MemberAddress2 = General.NONE;
            this.MemberCity = General.NONE;
            this.MemberState = General.NONE;
            this.MemberCountry = General.NONE;
            this.MemberZip = General.NONE;
            this.MemberPhone = General.NONE;
            this.MemberPhone2 = General.NONE;
            this.MemberFax = General.NONE;
            this.MemberUrl = General.NONE;
            this.MemberClassId = 1;
            this.GeoRegionId = 1;
        }
        public Member(Member member)
        {
            this.PKId = member.PKId;
            this.UserName = member.UserName;
            this.AspNetUserId = member.AspNetUserId;
            this.MemberEmail = member.MemberEmail;
            this.MemberJoinedDate = member.MemberJoinedDate;
            this.MemberLastChangedDate = member.MemberLastChangedDate;
            this.MemberFirstName = member.MemberFirstName;
            this.MemberLastName = member.MemberLastName;
            this.MemberDesc = member.MemberDesc;
            this.MemberOrganization = member.MemberOrganization;
            this.MemberAddress1 = member.MemberAddress1;
            this.MemberAddress2 = member.MemberAddress2;
            this.MemberCity = member.MemberCity;
            this.MemberState = member.MemberState;
            this.MemberCountry = member.MemberCountry;
            this.MemberZip = member.MemberZip;
            this.MemberPhone = member.MemberPhone;
            this.MemberPhone2 = member.MemberPhone2;
            this.MemberFax = member.MemberFax;
            this.MemberUrl = member.MemberUrl;
            this.MemberClassId = member.MemberClassId;
            this.MemberClass = new MemberClass();
            this.GeoRegionId = member.GeoRegionId;
            this.GeoRegion = new GeoRegion();
            this.AccountToMember = new List<AccountToMember>();
        }
        public int PKId { get; set; }
        public string UserName { get; set; }
        public string MemberEmail { get; set; }
        public DateTime MemberJoinedDate { get; set; }
        public DateTime MemberLastChangedDate { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberDesc { get; set; }
        public string MemberOrganization { get; set; }
        public string MemberAddress1 { get; set; }
        public string MemberAddress2 { get; set; }
        public string MemberCity { get; set; }
        public string MemberState { get; set; }
        public string MemberCountry { get; set; }
        public string MemberZip { get; set; }
        public string MemberPhone { get; set; }
        public string MemberPhone2 { get; set; }
        public string MemberFax { get; set; }
        public string MemberUrl { get; set; }
        public int MemberClassId { get; set; }
        public int GeoRegionId { get; set; }
        public string AspNetUserId { get; set; }

        public virtual ICollection<AccountToMember> AccountToMember { get; set; }
        public virtual GeoRegion GeoRegion { get; set; }
        public virtual MemberClass MemberClass { get; set; }
    }
}
