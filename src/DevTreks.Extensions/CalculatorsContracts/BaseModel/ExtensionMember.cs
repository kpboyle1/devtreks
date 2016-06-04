using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevTreks.Models;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The ExtensionMember class is adapted from the Member class in DevTreks.
    ///Author:		www.devtreks.org
    ///Date:		2011, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class ExtensionMember
    {
        public ExtensionMember() 
        {
            //logsin to default account (i.e. club), but can switch using client ui
            this.MemberRole = string.Empty;
            this.Id = 0;
            this.MemberId = 0;
            this.LastName = string.Empty;
            this.FirstName = string.Empty;
            this.Description = string.Empty;
            this.Email = string.Empty;
            this.MemberGroupId = 1;
            this.GeoRegionId = 1;
            this.URIFull = string.Empty;
            this.MemberDocFullPath = string.Empty;
            this.ErrorMessage = string.Empty;
            this.ClubDefault = new ExtensionClub();
            this.ClubInUse = new ExtensionClub();
        }
        public ExtensionMember(ExtensionMember memberCopy)
        {
            this.MemberRole = memberCopy.MemberRole.ToString();
            this.Id = memberCopy.Id;
            this.MemberId = memberCopy.MemberId;
            this.LastName = memberCopy.LastName;
            this.FirstName = memberCopy.FirstName;
            this.Description = memberCopy.Description;
            this.Email = memberCopy.Email;
            this.MemberGroupId = memberCopy.MemberGroupId;
            this.GeoRegionId = memberCopy.GeoRegionId;
            this.URIFull = memberCopy.URIFull;
            this.MemberDocFullPath = memberCopy.MemberDocFullPath;
            this.ErrorMessage = memberCopy.ErrorMessage;
            this.ClubDefault = new ExtensionClub(memberCopy.ClubDefault);
            this.ClubInUse = new ExtensionClub(memberCopy.ClubInUse);
        }
        public ExtensionMember(AccountToMember memberCopy)
        {
            this.MemberRole = memberCopy.MemberRole.ToString();
            this.Id = memberCopy.PKId;
            this.MemberId = memberCopy.MemberId;
            this.LastName = memberCopy.Member.MemberLastName;
            this.FirstName = memberCopy.Member.MemberFirstName;
            this.Description = memberCopy.Member.MemberDesc;
            this.Email = memberCopy.Member.MemberEmail;
            this.MemberGroupId = memberCopy.Member.MemberClassId;
            this.GeoRegionId = memberCopy.Member.GeoRegionId;
            this.URIFull = memberCopy.URIFull;
            this.MemberDocFullPath = memberCopy.MemberDocFullPath;
            //this.ErrorMessage = memberCopy.Member.ErrorMessage;
            this.ClubDefault = new ExtensionClub(memberCopy.ClubDefault);
            this.ClubInUse = new ExtensionClub(memberCopy.ClubInUse);
        }
        public static AccountToMember GetMember(ExtensionMember memberCopy)
        {
            AccountToMember newMember = new AccountToMember();
            newMember.MemberRole = Data.AppHelpers.Members.MEMBER_ROLE_TYPES.contributor.ToString();
            newMember.PKId = memberCopy.Id;
            newMember.MemberId = memberCopy.MemberId;
            newMember.Member.MemberLastName = memberCopy.LastName;
            newMember.Member.MemberFirstName = memberCopy.FirstName;
            newMember.Member.MemberDesc = memberCopy.Description;
            newMember.Member.MemberEmail = memberCopy.Email;
            newMember.Member.MemberClassId = memberCopy.MemberGroupId;
            newMember.Member.GeoRegionId = memberCopy.GeoRegionId;
            newMember.URIFull = memberCopy.URIFull;
            newMember.MemberDocFullPath = memberCopy.MemberDocFullPath;
            //newMember.ErrorMessage = memberCopy.ErrorMessage;
            newMember.ClubDefault = ExtensionClub.GetClub(memberCopy.ClubDefault);
            newMember.ClubInUse = ExtensionClub.GetClub(memberCopy.ClubInUse);
            return newMember;
        }
        public int Id { get; set; }
        public string MemberRole { get; set; }
        public int MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int MemberGroupId { get; set; }
        public int GeoRegionId { get; set; }
        public string URIFull { get; set; }
        public string MemberDocFullPath { get; set; }
        public string ErrorMessage { get; set; }
        public IList<ExtensionClub> Clubs { get; set; }
        public ExtensionClub ClubDefault { get; set; }
        public ExtensionClub ClubInUse { get; set; }

        public static IList<Member> CopyMember(IList<Member> members)
        {
            IList<Member> copyMembers = new List<Member>();
            if (members != null)
            {
                foreach (Member copyMember in members)
                {
                    copyMembers.Add(new Member(copyMember));
                }
            }
            return copyMembers;
        }
    }
}
