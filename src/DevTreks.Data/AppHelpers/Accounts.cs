using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for accounts (clubs)
    ///Author:		www.devtreks.org
    ///Date:		2016, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        Known issues
    ///             1. Clubs can't be deleted by club members, it requires 
    ///                 a custom action on the part of DevTreks administrators. 
    ///                 Instead, members can choose to delete clubs from their 
    ///                 list of active club memberships.
    /// </summary>
    public class Accounts
    {
        public Accounts(){}

        //updates
        public const string ACCOUNT_NAME = "AccountName";
        public const string ACCOUNT_EMAIL = "AccountEmail";
        public const string ACCOUNT_GROUPID = "AccountClassId";

        public enum ACCOUNT_TYPES
        {
            accountgroup    = 1,
            account         = 2
        }
        //160 deprecated separate file storage for guests for performance and data management 
        /// <summary>
        /// Correspond to database roles and used for security
        /// </summary>
        public enum ACCOUNT_GROUPS
        {
            //owners of uris are clubs; nonowners are guests
            //guest   = 1,
            club    = 2
        }
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (currentNodeName == ACCOUNT_TYPES.accountgroup.ToString())
            {
                if (uri.URIMember.MemberRole == Members.MEMBER_ROLE_TYPES.coordinator.ToString())
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
                    uri.URIDataManager.ChildrenNodeName = ACCOUNT_TYPES.accountgroup.ToString();
                }
                else
                {
                    uri.URIDataManager.ChildrenNodeName = ACCOUNT_TYPES.account.ToString();
                    //link backwards (to groups or clubs)
                    uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
                }
            }
            else if (currentNodeName == ACCOUNT_TYPES.account.ToString())
            {
                if (uri.URIMember.MemberRole == Members.MEMBER_ROLE_TYPES.coordinator.ToString())
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
                //no empty tocs
                uri.URIDataManager.ChildrenNodeName = ACCOUNT_TYPES.account.ToString();
            }
        }
        public static void GetChildForeignKeyName(string parentNodeName,
            out string childForeignKeyName)
        {
            childForeignKeyName = string.Empty;
            if (parentNodeName == ACCOUNT_TYPES.accountgroup.ToString())
            {
                childForeignKeyName = "AccountGroupId";
            }
            else if (parentNodeName == ACCOUNT_TYPES.account.ToString())
            {
                childForeignKeyName = "AccountId";
            }
        }
        public async Task<List<GeoRegion>> GetGeoRegionsAsync(ContentURI uri)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader geoRegions
                = await sqlIO.RunProcAsync("0GetGeoRegions");
            List<GeoRegion> colGeoRegions = new List<GeoRegion>();
            if (geoRegions != null)
            {
                using (geoRegions)
                {
                    //build a related service list to return to the client
                    while (await geoRegions.ReadAsync())
                    {
                        GeoRegion newGeoRegion = new GeoRegion();
                        newGeoRegion.PKId = geoRegions.GetInt32(0);
                        newGeoRegion.GeoRegionNum = geoRegions.GetString(1);
                        newGeoRegion.GeoRegionName = geoRegions.GetString(2);
                        newGeoRegion.GeoRegionDesc = geoRegions.GetString(3);
                        colGeoRegions.Add(newGeoRegion);
                    }
                }
            }
            sqlIO.Dispose();
            return colGeoRegions;
        }
        public async Task<List<AccountClass>> GetClubGroupsAsync(ContentURI uri)
        {
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader clubGroups
                = await sqlIO.RunProcAsync("0GetAccountGroups");
            List<AccountClass> colAccountGroups = new List<AccountClass>();
            if (clubGroups != null)
            {
                using (clubGroups)
                {
                    //build a related service list to return to the client
                    while (await clubGroups.ReadAsync())
                    {
                        AccountClass newAccountGroup = new AccountClass();
                        newAccountGroup.PKId = clubGroups.GetInt32(0);
                        newAccountGroup.AccountClassNum = clubGroups.GetString(1);
                        newAccountGroup.AccountClassName = clubGroups.GetString(2);
                        newAccountGroup.AccountClassDesc = clubGroups.GetString(3);
                        newAccountGroup.Account = new List<Account>();
                        colAccountGroups.Add(newAccountGroup);
                    }
                }
            }
            sqlIO.Dispose();
            return colAccountGroups;
        }
        public async Task<Account> GetClubByIdAsync(ContentURI uri, int accountId)
        {
            Account club = new Account(true);
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
			{
				sqlIO.MakeInParam("@AccountId",	                SqlDbType.Int, 4, accountId)
			};
            SqlDataReader dataReader
                = await sqlIO.RunProcAsync("0GetClubById", oPrams);
            if (dataReader != null)
            {
                using (dataReader)
                {
                    int i = 0;
                    //build a related service list to return to the client
                    while (await dataReader.ReadAsync())
                    {
                        if (i == 0)
                        {
                            club.PKId = dataReader.GetInt32(0);
                            club.AccountName = dataReader.GetString(1);
                            club.AccountDesc = dataReader.GetString(2);
                            club.AccountLongDesc = dataReader.GetString(3);
                            club.AccountEmail = dataReader.GetString(4);
                            club.AccountURI = dataReader.GetString(5);
                            club.AccountClassId = dataReader.GetInt32(6);
                            club.GeoRegionId = dataReader.GetInt32(7);
                            //the objects and collecions are set later
                            //not part of db
                            club.ClubDocFullPath = string.Empty;
                            club.PrivateAuthorizationLevel = 0;
                            club.URIFull = string.Empty;
                        }
                        else
                        {
                            break;
                        }
                        i++;
                    }
                }
            }
            sqlIO.Dispose();
            return club;
        }

        public async Task<List<AccountToMember>> GetClubToMemberByMemberIdAsync(
            ContentURI uri, int memberId)
        {
            List<AccountToMember> colClubToMember = new List<AccountToMember>();
            if (memberId == 0)
            {
                //set default objects
                AccountToMember atom = new AccountToMember(true);
                atom.ClubDefault = new Account(true);
                atom.Member = new Member(true);
                colClubToMember.Add(atom);
                return colClubToMember;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@MemberId",	                SqlDbType.Int, 4, memberId)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                "0GetClubMembersByMemberId", colPrams);
            colClubToMember = await FillClubToMemberList(dataReader);
            sqlIO.Dispose();
            return colClubToMember;
        }
        public async Task<List<AccountToNetwork>> GetNetworkByClubIdAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToNetwork> colClubNetwork = new List<AccountToNetwork>();
            if (accountId == 0)
            {
                //set default objects
                AccountToNetwork aton = new AccountToNetwork(true);
                colClubNetwork.Add(aton);
                return colClubNetwork;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@AccountId",	                SqlDbType.Int, 4, accountId)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                "0GetNetworksByClubId", colPrams);
            if (dataReader != null)
            {
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        AccountToNetwork aton = new AccountToNetwork(true);
                        aton.PKId = dataReader.GetInt32(0);
                        aton.IsDefaultNetwork = dataReader.GetBoolean(1);
                        aton.DefaultGetDataFromType = dataReader.GetString(2);
                        aton.DefaultStoreDataAtType = dataReader.GetString(3);
                        aton.NetworkRole = dataReader.GetString(4);
                        aton.AccountId = dataReader.GetInt32(5);
                        aton.NetworkId = dataReader.GetInt32(6);
                        aton.Account = new Account(true);
                        aton.Network = new Network(true);
                        aton.Network.PKId = dataReader.GetInt32(7);
                        aton.Network.NetworkName = dataReader.GetString(8);
                        aton.Network.NetworkURIPartName = dataReader.GetString(9);
                        aton.Network.NetworkDesc = dataReader.GetString(10);
                        aton.Network.AdminConnection = dataReader.GetString(11);
                        aton.Network.WebFileSystemPath = dataReader.GetString(12);
                        aton.Network.WebConnection = dataReader.GetString(13);
                        aton.Network.WebDbPath = dataReader.GetString(14);
                        aton.Network.NetworkClassId = dataReader.GetInt32(15);
                        aton.Network.NetworkClass = new NetworkClass();
                        aton.Network.GeoRegionId = dataReader.GetInt32(16);
                        //rest are not stored in db
                        aton.Network.IsDefault = aton.IsDefaultNetwork;
                        aton.Network.URIFull = string.Empty;
                        aton.Network.XmlConnection = string.Empty;
                        colClubNetwork.Add(aton);
                    }
                }
            }
            else
            {
                //set default objects
                AccountToNetwork aton = new AccountToNetwork(true);
                colClubNetwork.Add(aton);
            }
            sqlIO.Dispose();
            return colClubNetwork;
        }
        public async Task<List<AccountToMember>> GetMemberByClubIdAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToMember> colClubToMember = new List<AccountToMember>();
            if (accountId == 0)
            {
                //set default objects
                AccountToMember aton = new AccountToMember(true);
                colClubToMember.Add(aton);
                return colClubToMember;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@AccountId",	                SqlDbType.Int, 4, accountId)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                "0GetMembersByClubId", colPrams);
            colClubToMember = await FillClubToMemberList(dataReader);
            sqlIO.Dispose();
            return colClubToMember;
        }
        private async Task<List<AccountToMember>> FillClubToMemberList(SqlDataReader dataReader)
        {
            List<AccountToMember> colClubToMember = new List<AccountToMember>();
            if (dataReader != null)
            {
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        AccountToMember atom = new AccountToMember(true);
                        atom.ClubDefault = new Account();
                        atom.Member = new Member();
                        //order is Account, Member, AccountToMember
                        //set default club
                        //Account.PKId	
                        atom.ClubDefault.PKId = dataReader.GetInt32(0);
                        //Account.AccountName	
                        atom.ClubDefault.AccountName = dataReader.GetString(1);
                        //Account.AccountDesc	
                        atom.ClubDefault.AccountDesc = dataReader.GetString(2);
                        //Account.AccountLongDesc	
                        atom.ClubDefault.AccountLongDesc = dataReader.GetString(3);
                        //Account.AccountEmail	
                        atom.ClubDefault.AccountEmail = dataReader.GetString(4);
                        //Account.AccountURI	
                        atom.ClubDefault.AccountURI = dataReader.GetString(5);
                        //Account.AccountClassId	
                        atom.ClubDefault.AccountClassId = dataReader.GetInt32(6);
                        //Account.GeoRegionId	
                        atom.ClubDefault.GeoRegionId = dataReader.GetInt32(7);
                        //not in db
                        atom.ClubDefault.ClubDocFullPath = string.Empty;
                        //can this be set now? or after service is known?
                        atom.ClubDefault.PrivateAuthorizationLevel
                            = AccountHelper.AUTHORIZATION_LEVELS.none;
                        atom.ClubDefault.NetCost = 0;
                        atom.ClubDefault.TotalCost = 0;
                        atom.ClubDefault.URIFull = string.Empty;

                        //set member
                        //Member.PKId	
                        atom.Member.PKId = dataReader.GetInt32(8);
                        //Member.UserName	
                        atom.Member.UserName = dataReader.GetString(9);
                        //Member.MemberEmail	
                        atom.Member.MemberEmail = dataReader.GetString(10);
                        //Member.MemberJoinedDate	
                        atom.Member.MemberJoinedDate = dataReader.GetDateTime(11);
                        //Member.MemberLastChangedDate	
                        atom.Member.MemberLastChangedDate = dataReader.GetDateTime(12);
                        //Member.MemberFirstName	
                        atom.Member.MemberFirstName = dataReader.GetString(13);
                        //Member.MemberLastName	
                        atom.Member.MemberLastName = dataReader.GetString(14);
                        //Member.MemberDesc	
                        atom.Member.MemberDesc = dataReader.GetString(15);
                        //Member.MemberOrganization	
                        atom.Member.MemberOrganization = dataReader.GetString(16);
                        //Member.MemberAddress1	
                        atom.Member.MemberAddress1 = dataReader.GetString(17);
                        //Member.MemberAddress2	
                        atom.Member.MemberAddress2 = dataReader.GetString(18);
                        //Member.MemberCity	
                        atom.Member.MemberCity = dataReader.GetString(19);
                        //Member.MemberState	
                        atom.Member.MemberState = dataReader.GetString(20);
                        //Member.MemberCountry	
                        atom.Member.MemberCountry = dataReader.GetString(21);
                        //Member.MemberZip	
                        atom.Member.MemberZip = dataReader.GetString(22);
                        //Member.MemberPhone	
                        atom.Member.MemberPhone = dataReader.GetString(23);
                        //Member.MemberPhone2	
                        atom.Member.MemberPhone2 = dataReader.GetString(24);
                        //Member.MemberFax	
                        atom.Member.MemberFax = dataReader.GetString(25);
                        //Member.MemberUrl	
                        atom.Member.MemberUrl = dataReader.GetString(26);
                        //Member.MemberClassId	
                        atom.Member.MemberClassId = dataReader.GetInt32(27);
                        //Member.GeoRegionId	
                        atom.Member.GeoRegionId = dataReader.GetInt32(28);
                        //Member.AspNetUserId	
                        atom.Member.AspNetUserId = dataReader.GetString(29);

                        //set accounttomember
                        //AccountToMember.PKId	
                        atom.PKId = dataReader.GetInt32(30);
                        //AccountToMember.IsDefaultClub	
                        atom.IsDefaultClub = dataReader.GetBoolean(31);
                        //AccountToMember.MemberRole	
                        atom.MemberRole = dataReader.GetString(32);
                        //AccountToMember.AccountId	
                        atom.AccountId = dataReader.GetInt32(33);
                        //AccountToMember.MemberId
                        atom.MemberId = dataReader.GetInt32(34);
                        //not part of db
                        atom.AuthorizationLevel = (int)AccountHelper.AUTHORIZATION_LEVELS.none;
                        atom.MemberDocFullPath = string.Empty;
                        atom.URIFull = string.Empty;
                        colClubToMember.Add(atom);
                    }
                }
            }
            else
            {
                //set default objects
                AccountToMember atom = new AccountToMember(true);
                atom.ClubDefault = new Account(true);
                atom.Member = new Member(true);
                colClubToMember.Add(atom);
            } 
            return colClubToMember;
        }
        public async Task<List<AccountToLocal>> GetLocalsByClubIdAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToLocal> colClubLocals = new List<AccountToLocal>();
            if (accountId == 0)
            {
                //set default objects
                AccountToLocal atol = new AccountToLocal(true);
                colClubLocals.Add(atol);
                return colClubLocals;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@AccountId",	                SqlDbType.Int, 4, accountId)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                "0GetLocalsByClubId", colPrams);
            if (dataReader != null)
            {
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        AccountToLocal atol = new AccountToLocal(true);
                        atol.PKId = dataReader.GetInt32(0);
                        atol.LocalName = dataReader.GetString(1);
                        atol.LocalDesc = dataReader.GetString(2);
                        atol.UnitGroupId = dataReader.GetInt32(3);
                        atol.UnitGroup = dataReader.GetString(4);
                        atol.CurrencyGroupId = dataReader.GetInt32(5);
                        atol.CurrencyGroup = dataReader.GetString(6);
                        atol.RealRateId = dataReader.GetInt32(7);
                        atol.RealRate = dataReader.GetFloat(8);
                        atol.NominalRateId = dataReader.GetInt32(9);
                        atol.NominalRate = dataReader.GetFloat(10);
                        atol.DataSourceTechId = dataReader.GetInt32(11);
                        atol.DataSourceTech = dataReader.GetString(12);
                        atol.GeoCodeTechId = dataReader.GetInt32(13);
                        atol.GeoCodeTech = dataReader.GetString(14);
                        atol.DataSourcePriceId = dataReader.GetInt32(15);
                        atol.DataSourcePrice = dataReader.GetString(16);
                        atol.GeoCodePriceId = dataReader.GetInt32(17);
                        atol.GeoCodePrice = dataReader.GetString(18);
                        atol.RatingGroupId = dataReader.GetInt32(19);
                        atol.RatingGroup = dataReader.GetString(20);
                        atol.AccountId = dataReader.GetInt32(21);
                        atol.IsDefaultLinkedView = dataReader.GetBoolean(22);
                        atol.Account = new Account();
                        //2.0.0 deprecated
                        //atol.LinkedViewId = dataReader.GetInt32(22);
                        //atol.LinkingXmlDoc = atol.LinkedViewName;
                        //atol.LinkedView = new LinkedView();
                        colClubLocals.Add(atol);
                    }
                }
            }
            else
            {
                //set default objects
                AccountToLocal aton = new AccountToLocal(true);
                colClubLocals.Add(aton);
            }
            sqlIO.Dispose();
            return colClubLocals;
        }
        public async Task<int> InsertNewAuditItemAsync(
            ContentURI uri, AccountToAudit audit)
        {
            int iId = 0;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
			{
                sqlIO.MakeInParam("@MemberId",	                    SqlDbType.Int, 4, audit.MemberId),
                sqlIO.MakeInParam("@MemberRole",	                SqlDbType.NVarChar, 25, audit.MemberRole),
                sqlIO.MakeInParam("@ClubInUseId",	                SqlDbType.Int, 4, audit.ClubInUseId),
                sqlIO.MakeInParam("@ClubInUseAuthorizationLevel",   SqlDbType.NVarChar, 25, audit.ClubInUseAuthorizationLevel),
                sqlIO.MakeInParam("@EditedDocURI",	                SqlDbType.NVarChar, 300, audit.EditedDocURI),
                sqlIO.MakeInParam("@EditedDocFullPath",	            SqlDbType.NVarChar, 400, audit.EditedDocFullPath),
                sqlIO.MakeInParam("@ServerSubAction",	            SqlDbType.NVarChar, 25, audit.ServerSubAction),
                sqlIO.MakeInParam("@EditDate",	                    SqlDbType.Date, 4, audit.EditDate),
				sqlIO.MakeInParam("@AccountId",	                    SqlDbType.Int, 4, audit.AccountId)
			};
            iId = await sqlIO.RunProcIntAsync("0InsertAudit", oPrams);
            sqlIO.Dispose();
            return iId;
        }
        public async Task<List<AccountToAudit>> GetAuditsByClubIdAsync(
           ContentURI uri, int accountId, int numberRecords)
        {
            List<AccountToAudit> colClubAudits = new List<AccountToAudit>();
            if (accountId == 0)
            {
                //set default objects
                AccountToAudit atoa = new AccountToAudit(true);
                colClubAudits.Add(atoa);
                return colClubAudits;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@AccountId",	                SqlDbType.Int, 4, accountId),
                sqlIO.MakeInParam("@Number",	                SqlDbType.Int, 4, numberRecords)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                "0GetAuditsByClubId", colPrams);
            if (dataReader != null)
            {
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        AccountToAudit atoa = new AccountToAudit(true);
                        atoa.PKId = dataReader.GetInt32(0);
                        atoa.MemberId = dataReader.GetInt32(1);
                        atoa.MemberRole = dataReader.GetString(2);
                        atoa.ClubInUseId = dataReader.GetInt32(3);
                        atoa.ClubInUseAuthorizationLevel = dataReader.GetString(4);
                        atoa.EditedDocURI = dataReader.GetString(5);
                        atoa.EditedDocFullPath = dataReader.GetString(6);
                        atoa.ServerSubAction = dataReader.GetString(7);
                        atoa.EditDate = dataReader.GetDateTime(8);
                        atoa.AccountId = dataReader.GetInt32(9);
                        atoa.Account = new Account();
                        atoa.Account.PKId = dataReader.GetInt32(10);
                        atoa.Account.AccountName = dataReader.GetString(11);
                        atoa.Account.AccountDesc = dataReader.GetString(12);
                        atoa.Account.AccountLongDesc = dataReader.GetString(13);
                        atoa.Account.AccountEmail = dataReader.GetString(14);
                        atoa.Account.AccountURI = dataReader.GetString(15);
                        atoa.Account.AccountClassId = dataReader.GetInt32(16);
                        atoa.Account.GeoRegionId = dataReader.GetInt32(17);
                        //not in db
                        atoa.Account.ClubDocFullPath = string.Empty;
                        atoa.Account.PrivateAuthorizationLevel
                            = AccountHelper.AUTHORIZATION_LEVELS.none;
                        atoa.Account.NetCost = 0;
                        atoa.Account.TotalCost = 0;
                        atoa.Account.URIFull = string.Empty;
                        colClubAudits.Add(atoa);
                    }
                }
            }
            else
            {
                //set default objects
                AccountToAudit aton = new AccountToAudit(true);
                colClubAudits.Add(aton);
            }
            sqlIO.Dispose();
            return colClubAudits;
        }
        public async Task<List<AccountToAddIn>> GetAddInsByClubIdAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToAddIn> colClubAddIns = new List<AccountToAddIn>();
            if (accountId == 0)
            {
                //set default objects
                AccountToAddIn atoa = new AccountToAddIn(true);
                colClubAddIns.Add(atoa);
                return colClubAddIns;
            }
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams =
			{
				sqlIO.MakeInParam("@AccountId",	                SqlDbType.Int, 4, accountId)
			};
            SqlDataReader dataReader = await sqlIO.RunProcAsync(
                "0GetAddInsByClubId", colPrams);
            if (dataReader != null)
            {
                using (dataReader)
                {
                    while (await dataReader.ReadAsync())
                    {
                        AccountToAddIn atoa = new AccountToAddIn(true);
                        atoa.PKId = dataReader.GetInt32(0);
                        atoa.LinkedViewName = dataReader.GetString(1);
                        atoa.LinkingNodeId = dataReader.GetInt32(2);
                        atoa.LinkedViewId = dataReader.GetInt32(3);
                        atoa.IsDefaultLinkedView = dataReader.GetBoolean(4);
                        atoa.Account = new Account(true);
                        atoa.LinkedView = new LinkedView(true);
                        atoa.LinkedView.PKId = dataReader.GetInt32(5);
                        atoa.LinkedView.LinkedViewNum = dataReader.GetString(6);
                        atoa.LinkedView.LinkedViewName = dataReader.GetString(7);
                        atoa.LinkedView.LinkedViewDesc = dataReader.GetString(8);
                        atoa.LinkedView.LinkedViewFileExtensionType = dataReader.GetString(9);
                        atoa.LinkedView.LinkedViewLastChangedDate = dataReader.GetDateTime(10);
                        atoa.LinkedView.LinkedViewFileName = dataReader.GetString(11);
                        atoa.LinkedView.LinkedViewXml = string.Empty;
                        atoa.LinkedView.LinkedViewAddInName = dataReader.GetString(13);
                        atoa.LinkedView.LinkedViewAddInHostName = dataReader.GetString(14);
                        atoa.LinkedView.LinkedViewPackId = dataReader.GetInt32(15);
                        colClubAddIns.Add(atoa);
                    }
                }
            }
            else
            {
                //set default objects
                AccountToAddIn atoa = new AccountToAddIn(true);
                colClubAddIns.Add(atoa);
            }
            sqlIO.Dispose();
            return colClubAddIns;
        }
        public async Task<bool> SetClubOwnerByServiceAsync(ContentURI uri)
        {
            bool bHasSet = true;
            Account newClub = new Account();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
			{
				sqlIO.MakeInParam("@Id",	                SqlDbType.Int, 4, uri.URIService.ServiceId),
                sqlIO.MakeOutParam("@AccountId",	        SqlDbType.Int, 4),
                sqlIO.MakeOutParam("@AccountName",	        SqlDbType.NVarChar, 75),
                sqlIO.MakeOutParam("@AccountGroupId",	    SqlDbType.Int, 4),
                sqlIO.MakeOutParam("@AccountEmail",	        SqlDbType.NVarChar, 255)
			};
            int iNotUsed = await sqlIO.RunProcIntAsync("0GetAccountByService", oPrams);
            if (oPrams[1].Value != System.DBNull.Value)
            {
                newClub.PKId = (int)oPrams[1].Value;
            }
            if (oPrams[2].Value != System.DBNull.Value)
            {
                newClub.AccountName = oPrams[2].Value.ToString();
            }
            if (oPrams[3].Value != System.DBNull.Value)
            {
                int iAccountGroupId = (int)oPrams[3].Value;
                newClub.AccountClassId = iAccountGroupId;
            }
            if (oPrams[4].Value != System.DBNull.Value)
            {
                newClub.AccountEmail = oPrams[4].Value.ToString();
            }
            uri.URIClub = newClub;
            bHasSet = true;
            sqlIO.Dispose();
            return bHasSet;
        }
       
        public static bool IsURIOwningClub(ContentURI uri)
        {
            bool bIsOwningClub = false;
            if (uri.URIFileExtensionType
                == Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //tempdocs are owned by whoever is using them (as of this version)
                bIsOwningClub = true;
            }
            else
            {
                if (uri.URIClub != null)
                {
                    if (uri.URIMember != null)
                    {
                        if (uri.URIMember.ClubInUse != null)
                        {
                            if (uri.URIClub.PKId == uri.URIMember.ClubInUse.PKId)
                            {
                                bIsOwningClub = true;
                            }
                        }
                    }
                }
            }
            return bIsOwningClub;
        }
        public static void GetNewClubEdits(IDictionary<string, string> updates, out string accountName,
            out string accountEmail, out string accountGroupId, out string accountGeoRegionId)
        {
            accountName = string.Empty;
            accountEmail = string.Empty;
            accountGroupId = string.Empty;
            accountGeoRegionId = string.Empty;
            if (updates != null)
            {
                string sURIPattern = string.Empty;
                string sAttName = string.Empty;
                string sAttValue = string.Empty;
                string sDataType = string.Empty;
                string sSize = string.Empty;
                foreach (string sKeyName in updates.Keys.AsParallel())
                {
                    //1. sKeyName should be a 'uripattern;attname;datatype;size' delimited string
                    string[] arrUpdateParams
                        = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                    sURIPattern = string.Empty;
                    sAttName = string.Empty;
                    sAttValue = string.Empty;
                    EditHelpers.EditHelper.GetStandardEditNameParams(arrUpdateParams,
                        out sURIPattern, out sAttName, out sDataType, out sSize);
                    if (!string.IsNullOrEmpty(sAttName))
                    {
                        sAttValue = updates[sKeyName].ToString();
                    }
                    if (sAttName == ACCOUNT_NAME)
                    {
                        accountName = sAttValue;
                    }
                    else if (sAttName == ACCOUNT_EMAIL)
                    {
                        accountEmail = sAttValue;
                    }
                    else if (sAttName == ACCOUNT_GROUPID)
                    {
                        accountGroupId = sAttValue;
                    }
                    else if (sAttName == Locals.GEORREGION_ID)
                    {
                        accountGeoRegionId = sAttValue;
                    }
                }
            }
        }
        
    }
}
