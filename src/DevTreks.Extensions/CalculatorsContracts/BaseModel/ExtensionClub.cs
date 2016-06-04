using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevTreks.Models;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The ExtensionClub class is adapted from the Club class in DevTreks.
    ///Author:		www.devtreks.org
    ///Date:		2011, October
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class ExtensionClub
    {
        public ExtensionClub() 
        {
            //id = db table pkid
            this.AccountId = 0;
            //club name
            this.AccountName = "anonymous";
            //club description
            this.AccountDescription = string.Empty;
            //club long description
            this.AccountLongDescription = string.Empty;
            //club email
            this.AccountEmail = string.Empty;
            //club uri (usually to a linkedview or devpackpart overview)
            this.AccountURI = string.Empty;
            //club group
            this.AccountGroupId = 1;
            //georegion
            this.GeoRegionId = 1;
            //representational state transfer uri (web address of club)
            this.URIFull = string.Empty;
            //true means the club is a particular member's default club
            this.IsDefault = false;
            //path to a contenturi's data
            this.ClubDocFullPath = string.Empty;
            //authorization level for editing a uri
            this.PrivateAuthorizationLevel
                = AccountHelper.AUTHORIZATION_LEVELS.viewonly.ToString();
            //the club's networks
            this.Network = null;
            //the club's locals
            this.Locals = null;
            //the club's members
            this.Member = null;
            //the club's services (owned and subscribed)
            this.Service = null;
        }
        public ExtensionClub(ExtensionClub copyClub)
        {
            this.AccountId = copyClub.AccountId;
            this.AccountName = copyClub.AccountName;
            this.AccountDescription = copyClub.AccountDescription;
            this.AccountLongDescription = copyClub.AccountLongDescription;
            this.AccountEmail = copyClub.AccountEmail;
            this.AccountURI = copyClub.AccountURI;
            this.AccountGroupId = copyClub.AccountGroupId;
            this.GeoRegionId = copyClub.GeoRegionId;
            this.URIFull = copyClub.URIFull;
            this.IsDefault = false;
            this.ClubDocFullPath = copyClub.ClubDocFullPath;
            this.PrivateAuthorizationLevel = copyClub.PrivateAuthorizationLevel.ToString();
        }
        public ExtensionClub(Account copyClub)
        {
            this.AccountId = copyClub.PKId;
            this.AccountName = copyClub.AccountName;
            this.AccountDescription = copyClub.AccountDesc;
            this.AccountLongDescription = copyClub.AccountLongDesc;
            this.AccountEmail = copyClub.AccountEmail;
            this.AccountURI = copyClub.AccountURI;
            this.AccountGroupId = copyClub.AccountClassId;
            this.GeoRegionId = copyClub.GeoRegionId;
            this.URIFull = copyClub.URIFull;
            this.IsDefault = false;
            this.ClubDocFullPath = copyClub.ClubDocFullPath;
            this.PrivateAuthorizationLevel = copyClub.PrivateAuthorizationLevel.ToString();
        }
        public static Account GetClub(ExtensionClub copyClub)
        {
            Account newClub = new Account();
            newClub.PKId = copyClub.AccountId;
            newClub.AccountName = copyClub.AccountName;
            newClub.AccountDesc = copyClub.AccountDescription;
            newClub.AccountLongDesc = copyClub.AccountLongDescription;
            newClub.AccountEmail = copyClub.AccountEmail;
            newClub.AccountURI = copyClub.AccountURI;
            newClub.AccountClassId = copyClub.AccountGroupId;
            newClub.GeoRegionId = copyClub.GeoRegionId;
            newClub.URIFull = copyClub.URIFull;
            //newClub.IsDefault = false;
            newClub.ClubDocFullPath = copyClub.ClubDocFullPath;
            newClub.PrivateAuthorizationLevel 
                = AccountHelper.AUTHORIZATION_LEVELS.none;
            return newClub;
        }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public string AccountLongDescription { get; set; }
        public string AccountEmail { get; set; }
        public string AccountURI { get; set; }
        public int AccountGroupId { get; set; }
        public int GeoRegionId { get; set; }public string URIFull { get; set; }
        public bool IsDefault { get; set; }
        public string ClubDocFullPath { get; set; }
        public string PrivateAuthorizationLevel { get; set; }

        //networks the club belongs to
        public IList<ExtensionNetwork> Network { get; set; }
        //locals the club uses to automatically fill in geocodes, nominalrates ... 
        //when service apps nodes are first inserted
        public IList<Local> Locals { get; set; }
        //members belonging to the club
        public IList<ExtensionMember> Member { get; set; }
        //services belonging to the club, or subscribed to by the club
        public IList<ExtensionService> Service { get; set; }
        public static IList<Account> CopyClubs(IList<Account> clubs)
        {
            IList<Account> copyClubs = new List<Account>();
            if (clubs != null)
            {
                foreach (Account copyClub in clubs)
                {
                    copyClubs.Add(new Account(copyClub));
                }
            }
            return copyClubs;
        }
    }
}
