using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for members 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        Known issues
    ///             1. 2.0.0 switched to aspnet 1.0 security and login.
    ///             When a new Member is inserted, they are given the same UserName and Email.
    ///             That matches aspnet.Identity.User and works well with these sql queries.
    ///             Don't allow edits to either property (unless queries changed). Aspnet 
    ///             does not currently allow them to manually change email or password and 
    ///             neither can the member model.
    ///             1. Member can only be deleted from the base members table 
    ///             by a custom action of a DevTreks administrator. 
    ///             2. Founding members of clubs can't change their memberrole 
    ///             from 'coordinator', and can't delete themselves from the 
    ///             club they founded. 
    public class Members
    {
        public Members()
        {
        }
        public const string FORMEL_CONFIRM_PASSWORD = "lblConfirmPassword";
        //member model
        public const string MEMBER_USERNAME = "UserName";
        public const string MEMBER_EMAIL = "MemberEmail";
        public const string MEMBER_ROLE = "MemberRole";
        public const string MEMBER_URL = "MemberUrl";
        //accountomembermodel
        public const string ISDEFAULTCLUB = "IsDefaultClub";
        public const string AUTHORIZATION_LEVEL = "AuthorizationLevel";
        
        public enum MEMBER_TYPES
        {
            //account to accounttomember view 
            //the members belonging to a specific club
            memberaccountgroup  = 1,
            member              = 2
        }
        public enum MEMBER_BASE_TYPES
        {
            //memberclass to member
            memberbasegroup     = 1,
            memberbase          = 2
        }
       
        public enum MEMBER_ROLE_TYPES
        {
            none            = 0,
            coordinator     = 1,
            contributor     = 2
        }
        public static Dictionary<string, string> GetMemberRolesDictionary()
        {
            Dictionary<string, string> roles = new Dictionary<string, string>();
            roles.Add(MEMBER_ROLE_TYPES.none.ToString(), MEMBER_ROLE_TYPES.none.ToString());
            roles.Add(MEMBER_ROLE_TYPES.coordinator.ToString(), MEMBER_ROLE_TYPES.coordinator.ToString());
            roles.Add(MEMBER_ROLE_TYPES.contributor.ToString(), MEMBER_ROLE_TYPES.contributor.ToString());
            return roles;
        }
        
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == MEMBER_BASE_TYPES.memberbasegroup.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel 
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (uri.URIId == 0)
                {
                    //no link backwards (showing groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                    uri.URIDataManager.ChildrenNodeName = MEMBER_BASE_TYPES.memberbasegroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = MEMBER_BASE_TYPES.memberbase.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == MEMBER_TYPES.memberaccountgroup.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel 
                        == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
                //link forwards
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (uri.URIId == 0)
                {
                    //no link backwards (showing groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
                    uri.URIDataManager.ChildrenNodeName = MEMBER_TYPES.memberaccountgroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = MEMBER_TYPES.member.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == MEMBER_TYPES.member.ToString()
                || currentNodeName == MEMBER_BASE_TYPES.memberbase.ToString())
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel 
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                else
                {
                    uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
                }
                //link backwards
                uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                //link forwards because it still will show group-item in toc
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                if (currentNodeName == MEMBER_TYPES.member.ToString())
                {
                    //no empty tocs
                    uri.URIDataManager.ChildrenNodeName = MEMBER_TYPES.member.ToString();
                }
                else
                {
                    //no empty tocs
                    uri.URIDataManager.ChildrenNodeName = MEMBER_BASE_TYPES.memberbase.ToString();
                }

            }
        }
        public static MEMBER_ROLE_TYPES GetMemberRole(string memberType)
        {
            MEMBER_ROLE_TYPES eMemberRole = MEMBER_ROLE_TYPES.contributor;
            if (memberType == MEMBER_ROLE_TYPES.coordinator.ToString())
            {
                eMemberRole = MEMBER_ROLE_TYPES.coordinator;
            }
            else if (memberType == MEMBER_ROLE_TYPES.contributor.ToString())
            {
                eMemberRole = MEMBER_ROLE_TYPES.contributor;
            }
            return eMemberRole;
        }
        public static void GetChildForeignKeyName(string parentNodeName,
            out string parentForeignKeyName, out string baseForeignKeyName)
        {
            parentForeignKeyName = string.Empty;
            baseForeignKeyName = string.Empty;
            if (parentNodeName 
                == MEMBER_TYPES.memberaccountgroup.ToString())
            {
                parentForeignKeyName = "AccountId";
                baseForeignKeyName = "MemberId";
            }
            //else can insert members into base table during initial registration
        }
        public static void ChangeAttributesForInsertion(XElement insertedMember)
        {
            //set members-specific attributes
            insertedMember.SetAttributeValue(MEMBER_ROLE, 
                MEMBER_ROLE_TYPES.contributor.ToString());
        }
        //refactor: remove
        public static void SetMemberDbAttributes(XPathNavigator insertedMember)
        {
            //set members-specific db attributes
            EditHelpers.XPathIO.SetAttributeValue(insertedMember, MEMBER_ROLE,
                MEMBER_ROLE_TYPES.contributor.ToString());
        }
        public async Task<List<MemberClass>> GetMemberGroupsAsync(ContentURI uri)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader memberGroups
                = await sqlIO.RunProcAsync("0GetMemberGroups");
            List<MemberClass> colMemberGroups = new List<MemberClass>();
            if (memberGroups != null)
            {
                //build a related service list to return to the client
                while (memberGroups.Read())
                {
                    MemberClass newMemberGroup = new MemberClass();
                    newMemberGroup.PKId = memberGroups.GetInt32(0);
                    newMemberGroup.MemberClassNum = memberGroups.GetString(1);
                    newMemberGroup.MemberClassName = memberGroups.GetString(2);
                    newMemberGroup.MemberClassDesc = memberGroups.GetString(3);
                    newMemberGroup.Member = new List<Member>();
                    colMemberGroups.Add(newMemberGroup);
                }
            }
            sqlIO.Dispose();
            return colMemberGroups;
        }
        public async Task<bool> SetDefaultClubAndMemberAsync( 
            ContentURI uri, string userName)
        {
            bool bHasDefaults = false;
            if (string.IsNullOrEmpty(userName) 
                || userName == Helpers.GeneralHelpers.NONE)
            {
                //set default objects
                uri.URIMember = new AccountToMember(true);
                uri.URIMember.ClubDefault = new Account(true);
                uri.URIMember.Member = new Member(true);
                return false;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@UserName", SqlDbType.NVarChar, 128, userName)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync("0GetDefaultClubAndMember", 
                colPrams);
            if (dataReader != null)
            {
                //discrepency in field order between cloud and local db
                Helpers.FileStorageIO.PLATFORM_TYPES ePlatform = uri.URIDataManager.PlatformType;
                using (dataReader)
                {
                    int i = 0;
                    while (await dataReader.ReadAsync())
                    {
                        if (i == 0)
                        {
                            uri.URIMember = new AccountToMember();
                            uri.URIMember.ClubDefault = new Account();
                            uri.URIMember.Member = new Member();
                            //order is Account, Member, AccountToMember
                            //set default club
                            //Account.PKId	
                            uri.URIMember.ClubDefault.PKId = dataReader.GetInt32(0);
                            //Account.AccountName	
                            uri.URIMember.ClubDefault.AccountName = dataReader.GetString(1);
                            //Account.AccountDesc	
                            uri.URIMember.ClubDefault.AccountDesc = dataReader.GetString(2);
                            //Account.AccountLongDesc	
                            uri.URIMember.ClubDefault.AccountLongDesc = dataReader.GetString(3);
                            //Account.AccountEmail	
                            uri.URIMember.ClubDefault.AccountEmail = dataReader.GetString(4);
                            //Account.AccountURI	
                            uri.URIMember.ClubDefault.AccountURI = dataReader.GetString(5);
                            //Account.AccountClassId	
                            uri.URIMember.ClubDefault.AccountClassId = dataReader.GetInt32(6);
                            //Account.GeoRegionId	
                            uri.URIMember.ClubDefault.GeoRegionId = dataReader.GetInt32(7);
                            //not in db
                            uri.URIMember.ClubDefault.ClubDocFullPath = string.Empty;
                            //can this be set now? or after service is known?
                            uri.URIMember.ClubDefault.PrivateAuthorizationLevel
                                = AccountHelper.AUTHORIZATION_LEVELS.none;
                            uri.URIMember.ClubDefault.NetCost = 0;
                            uri.URIMember.ClubDefault.TotalCost = 0;
                            uri.URIMember.ClubDefault.URIFull = string.Empty;

                            //set member
                            //Member.PKId	
                            uri.URIMember.Member.PKId = dataReader.GetInt32(8);
                            if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
                            {
                                //Member.MemberEmail	
                                uri.URIMember.Member.MemberEmail = dataReader.GetString(9);
                                //Member.UserName	
                                uri.URIMember.Member.UserName = dataReader.GetString(10);
                            }
                            else
                            {
                                //Member.UserName	
                                uri.URIMember.Member.UserName = dataReader.GetString(9);
                                //Member.MemberEmail	
                                uri.URIMember.Member.MemberEmail = dataReader.GetString(10);
                            }
                            //Member.MemberJoinedDate	
                            uri.URIMember.Member.MemberJoinedDate = dataReader.GetDateTime(11);
                            //Member.MemberLastChangedDate	
                            uri.URIMember.Member.MemberLastChangedDate = dataReader.GetDateTime(12);
                            //Member.MemberFirstName	
                            uri.URIMember.Member.MemberFirstName = dataReader.GetString(13);
                            //Member.MemberLastName	
                            uri.URIMember.Member.MemberLastName = dataReader.GetString(14);
                            //Member.MemberDesc	
                            uri.URIMember.Member.MemberDesc = dataReader.GetString(15);
                            //Member.MemberOrganization	
                            uri.URIMember.Member.MemberOrganization = dataReader.GetString(16);
                            //Member.MemberAddress1	
                            uri.URIMember.Member.MemberAddress1 = dataReader.GetString(17);
                            //Member.MemberAddress2	
                            uri.URIMember.Member.MemberAddress2 = dataReader.GetString(18);
                            //Member.MemberCity	
                            uri.URIMember.Member.MemberCity = dataReader.GetString(19);
                            //Member.MemberState	
                            uri.URIMember.Member.MemberState = dataReader.GetString(20);
                            //Member.MemberCountry	
                            uri.URIMember.Member.MemberCountry = dataReader.GetString(21);
                            //Member.MemberZip	
                            uri.URIMember.Member.MemberZip = dataReader.GetString(22);
                            //Member.MemberPhone	
                            uri.URIMember.Member.MemberPhone = dataReader.GetString(23);
                            //Member.MemberPhone2	
                            uri.URIMember.Member.MemberPhone2 = dataReader.GetString(24);
                            //Member.MemberFax	
                            uri.URIMember.Member.MemberFax = dataReader.GetString(25);
                            //Member.MemberUrl	
                            uri.URIMember.Member.MemberUrl = dataReader.GetString(26);
                            //Member.MemberClassId	
                            uri.URIMember.Member.MemberClassId = dataReader.GetInt32(27);
                            //Member.GeoRegionId	
                            uri.URIMember.Member.GeoRegionId = dataReader.GetInt32(28);
                            //Member.AspNetUserId	
                            uri.URIMember.Member.AspNetUserId = dataReader.GetString(29);

                            //set accounttomember
                            //AccountToMember.PKId	
                            uri.URIMember.PKId = dataReader.GetInt32(30);
                            //AccountToMember.IsDefaultClub	
                            uri.URIMember.IsDefaultClub = dataReader.GetBoolean(31);
                            //AccountToMember.MemberRole	
                            uri.URIMember.MemberRole = dataReader.GetString(32);
                            //AccountToMember.AccountId	
                            uri.URIMember.AccountId = dataReader.GetInt32(33);
                            //AccountToMember.MemberId
                            uri.URIMember.MemberId = dataReader.GetInt32(34);
                            //not part of db
                            uri.URIMember.AuthorizationLevel = (int)AccountHelper.AUTHORIZATION_LEVELS.none;
                            uri.URIMember.MemberDocFullPath = string.Empty;
                            uri.URIMember.URIFull = string.Empty;
                            bHasDefaults = true;
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                //set default objects
                uri.URIMember = new AccountToMember(true);
                uri.URIMember.ClubDefault = new Account(true);
                uri.URIMember.Member = new Member(true);
            }
            if (!bHasDefaults)
            {
                bHasDefaults = await SetDefaultMemberAsync(uri, userName);
            }
            sqlIO.Dispose();
            return bHasDefaults;
        }
        public async Task<List<AccountToMember>> GetClubByServiceAndMemberAsync(
            ContentURI uri, int serviceId, int memberId)
        {
            List<AccountToMember> clubs = new List<AccountToMember>();
            if (memberId == 0
                || serviceId == 0)
            {
                AccountToMember club = new AccountToMember(true);
                //return blank member and service if not a real member or a real service
                club.ClubDefault = new Account(true);
                club.Member = new Member(true);
                clubs.Add(club);
                return clubs;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@MemberId", SqlDbType.Int, 4, memberId),
                sqlIO.MakeInParam("@ServiceId", SqlDbType.Int, 4, serviceId)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync( 
                "0GetClubByServiceAndMemberIds", colPrams);
            if (dataReader != null)
            {
                //discrepency in field order between cloud and local db
                Helpers.FileStorageIO.PLATFORM_TYPES ePlatform = uri.URIDataManager.PlatformType;
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        AccountToMember club = new AccountToMember();
                        club.ClubInUse = new Account();
                        club.Member = new Member();
                        //order is Account, Member, AccountToMember
                        //set club in use
                        //Account.PKId	
                        club.ClubInUse.PKId = dataReader.GetInt32(0);
                        //Account.AccountName	
                        club.ClubInUse.AccountName = dataReader.GetString(1);
                        //Account.AccountDesc	
                        club.ClubInUse.AccountDesc = dataReader.GetString(2);
                        //Account.AccountLongDesc	
                        club.ClubInUse.AccountLongDesc = dataReader.GetString(3);
                        //Account.AccountEmail	
                        club.ClubInUse.AccountEmail = dataReader.GetString(4);
                        //Account.AccountURI	
                        club.ClubInUse.AccountURI = dataReader.GetString(5);
                        //Account.AccountClassId	
                        club.ClubInUse.AccountClassId = dataReader.GetInt32(6);
                        //Account.GeoRegionId	
                        club.ClubInUse.GeoRegionId = dataReader.GetInt32(7);
                        //not in db
                        club.ClubInUse.ClubDocFullPath = string.Empty;
                        club.ClubInUse.PrivateAuthorizationLevel
                            = AccountHelper.AUTHORIZATION_LEVELS.none;
                        club.ClubInUse.NetCost = 0;
                        club.ClubInUse.TotalCost = 0;
                        club.ClubInUse.URIFull = string.Empty;

                        //set member	
                        club.Member.PKId = dataReader.GetInt32(8);
                        if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
                        {
                            //Member.MemberEmail	
                            club.Member.MemberEmail = dataReader.GetString(9);
                            //Member.UserName	
                            club.Member.UserName = dataReader.GetString(10);
                        }
                        else
                        {
                            //Member.UserName	
                            club.Member.UserName = dataReader.GetString(9);
                            //Member.MemberEmail	
                            club.Member.MemberEmail = dataReader.GetString(10);
                        }
                        club.Member.MemberJoinedDate = dataReader.GetDateTime(11);
                        club.Member.MemberLastChangedDate = dataReader.GetDateTime(12);
                        club.Member.MemberFirstName = dataReader.GetString(13);
                        club.Member.MemberLastName = dataReader.GetString(14);
                        club.Member.MemberDesc = dataReader.GetString(15);
                        club.Member.MemberOrganization = dataReader.GetString(16);
                        club.Member.MemberAddress1 = dataReader.GetString(17);
                        club.Member.MemberAddress2 = dataReader.GetString(18);
                        club.Member.MemberCity = dataReader.GetString(19);
                        club.Member.MemberState = dataReader.GetString(20);
                        club.Member.MemberCountry = dataReader.GetString(21);
                        club.Member.MemberZip = dataReader.GetString(22);
                        club.Member.MemberPhone = dataReader.GetString(23);
                        club.Member.MemberPhone2 = dataReader.GetString(24);
                        club.Member.MemberFax = dataReader.GetString(25);
                        club.Member.MemberUrl = dataReader.GetString(26);
                        club.Member.MemberClassId = dataReader.GetInt32(27);
                        club.Member.GeoRegionId = dataReader.GetInt32(28);
                        club.Member.AspNetUserId = dataReader.GetString(29);

                        //set accounttomember
                        //AccountToMember.PKId	
                        club.PKId = dataReader.GetInt32(30);
                        club.IsDefaultClub = dataReader.GetBoolean(31);
                        club.MemberRole = dataReader.GetString(32);
                        club.AccountId = dataReader.GetInt32(33);
                        club.MemberId = dataReader.GetInt32(34);
                        //not part of db
                        club.AuthorizationLevel = (int)AccountHelper.AUTHORIZATION_LEVELS.none;
                        club.MemberDocFullPath = string.Empty;
                        club.URIFull = string.Empty;
                        //the authorization level comes from this join
                        AccountToService service = new AccountToService();
                        service.PKId = dataReader.GetInt32(35);
                        service.Name = dataReader.GetString(36);
                        service.Amount1 = dataReader.GetInt32(37);
                        service.Status = dataReader.GetString(38);
                        service.LastChangedDate = dataReader.GetDateTime(39);
                        service.AuthorizationLevel = dataReader.GetInt16(40);
                        service.StartDate = dataReader.GetDateTime(41);
                        service.EndDate = dataReader.GetDateTime(42);
                        service.LastChangedDate = dataReader.GetDateTime(43);
                        service.IsOwner = dataReader.GetBoolean(44);
                        service.AccountId = dataReader.GetInt32(45);
                        service.ServiceId = dataReader.GetInt32(46);
                        //this is used to set ClubInUseAuthorizationLevel
                        club.ClubInUse.PrivateAuthorizationLevel
                            = AccountHelper.GetAuthorizationLevel(service.AuthorizationLevel);
                        clubs.Add(club);
                    }
                }
            }
            else
            {
                //set default objects
                AccountToMember club = new AccountToMember(true);
                //return blank member and service if not a real member or a real service
                club.ClubInUse = new Account(true);
                club.Member = new Member(true);
                clubs.Add(club);
            }
            sqlIO.Dispose();
            return clubs;
        }
        private async Task<bool> SetDefaultMemberAsync(
            ContentURI uri, string userName)
        {
            bool bHasMember = false;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@UserName", SqlDbType.NVarChar, 128, userName)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync("0GetDefaultMember",
                colPrams);
            if (dataReader != null)
            {
                //discrepency in field order between cloud and local db
                Helpers.FileStorageIO.PLATFORM_TYPES ePlatform = uri.URIDataManager.PlatformType;
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        uri.URIMember = new AccountToMember(true);
                        uri.URIMember.ClubDefault = new Account(true);
                        uri.URIMember.Member = new Member(true);

                        //set member
                        //Member.PKId	
                        uri.URIMember.Member.PKId = dataReader.GetInt32(0);
                        if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
                        {
                            //Member.MemberEmail	
                            uri.URIMember.Member.MemberEmail = dataReader.GetString(1);
                            //Member.UserName	
                            uri.URIMember.Member.UserName = dataReader.GetString(2);
                        }
                        else
                        {
                            //Member.UserName	
                            uri.URIMember.Member.UserName = dataReader.GetString(1);
                            //Member.MemberEmail	
                            uri.URIMember.Member.MemberEmail = dataReader.GetString(2);
                        }
                        //Member.MemberJoinedDate	
                        uri.URIMember.Member.MemberJoinedDate = dataReader.GetDateTime(3);
                        //Member.MemberLastChangedDate	
                        uri.URIMember.Member.MemberLastChangedDate = dataReader.GetDateTime(4);
                        //Member.MemberFirstName	
                        uri.URIMember.Member.MemberFirstName = dataReader.GetString(5);
                        //Member.MemberLastName	
                        uri.URIMember.Member.MemberLastName = dataReader.GetString(6);
                        //Member.MemberDesc	
                        uri.URIMember.Member.MemberDesc = dataReader.GetString(7);
                        //Member.MemberOrganization	
                        uri.URIMember.Member.MemberOrganization = dataReader.GetString(8);
                        //Member.MemberAddress1	
                        uri.URIMember.Member.MemberAddress1 = dataReader.GetString(9);
                        //Member.MemberAddress2	
                        uri.URIMember.Member.MemberAddress2 = dataReader.GetString(10);
                        //Member.MemberCity	
                        uri.URIMember.Member.MemberCity = dataReader.GetString(11);
                        //Member.MemberState	
                        uri.URIMember.Member.MemberState = dataReader.GetString(12);
                        //Member.MemberCountry	
                        uri.URIMember.Member.MemberCountry = dataReader.GetString(13);
                        //Member.MemberZip	
                        uri.URIMember.Member.MemberZip = dataReader.GetString(14);
                        //Member.MemberPhone	
                        uri.URIMember.Member.MemberPhone = dataReader.GetString(15);
                        //Member.MemberPhone2	
                        uri.URIMember.Member.MemberPhone2 = dataReader.GetString(16);
                        //Member.MemberFax	
                        uri.URIMember.Member.MemberFax = dataReader.GetString(17);
                        //Member.MemberUrl	
                        uri.URIMember.Member.MemberUrl = dataReader.GetString(18);
                        //Member.MemberClassId	
                        uri.URIMember.Member.MemberClassId = dataReader.GetInt32(19);
                        //Member.GeoRegionId	
                        uri.URIMember.Member.GeoRegionId = dataReader.GetInt32(20);
                        //Member.AspNetUserId	
                        uri.URIMember.Member.AspNetUserId = dataReader.GetString(21);
                        bHasMember = true;
                    }
                }
            }
            else
            {
                //set default objects
                uri.URIMember = new AccountToMember(true);
                uri.URIMember.ClubDefault = new Account(true);
                uri.URIMember.Member = new Member(true);
            }
            sqlIO.Dispose();
            return bHasMember;
        }
        public async Task<bool> UpdateDefaultClubForLoggedinMemberAsync(ContentURI uri,
           int memberId, int newIsDefaultAccountId)
        {
            bool bHasUpdated = false;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
            { 
                sqlIO.MakeInParam("@MemberId",                     SqlDbType.Int, 4, memberId),
                sqlIO.MakeInParam("@NewIsDefaultAccountId",       SqlDbType.Int, 4, newIsDefaultAccountId)
            };
            int iResult = await sqlIO.RunProcIntAsync( 
                "0UpdateAccountIsDefault", colPrams);
            sqlIO.Dispose();
            if (iResult == 0) bHasUpdated = true;
            return bHasUpdated;
        }
        public async Task<string> GetTopClubToMemberJoinIdAsync(ContentURI uriToEdit,
            string connect, string baseNodeName, string baseId, int accountId)
        {
            //top joins mean the base record can be edited (they were both
            //inserted by the same owner at the same time)
            string sTopJoinTableId = string.Empty;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uriToEdit);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@MemberId",          SqlDbType.Int, 4, baseId),
                sqlIO.MakeInParam("@AccountId",         SqlDbType.Int, 4, accountId),
				sqlIO.MakeInParam("@NodeName",          SqlDbType.NVarChar, 25, baseNodeName),
                sqlIO.MakeOutParam("@TopJoinTableId",   SqlDbType.Int, 4)
			};
            string sQryName = "0GetMemberTopJoinTableId";
            int iNotUsed = await sqlIO.RunProcIntAsync(sQryName, colPrams);
            if (colPrams[3].Value != System.DBNull.Value)
            {
                sTopJoinTableId = colPrams[3].Value.ToString();
            }
            sqlIO.Dispose();
            return sTopJoinTableId;
        }
    }
}
