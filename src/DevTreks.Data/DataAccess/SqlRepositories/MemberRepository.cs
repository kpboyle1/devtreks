using DevTreks.Data.DataAccess;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GenHelpers = DevTreks.Data.Helpers.GeneralHelpers;
namespace DevTreks.Data.SqlRepositories
{
    /// <summary>
    ///Purpose:		data access for club, member, and commons models
    ///Author:		www.devtreks.org
    ///Date:		2016, July
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class MemberRepository : IMemberRepositoryEF, IDisposable
    {    
        //2.0.0 changes
        //this uri is only used to init datacontext
        //most class methods include a complete uri, with full route, config, and context settings
        //sqlio uses that uri to get db connection
        public MemberRepository(ContentURI uri)
        {
            //pass the generic config settings to a dbcontextoptionsbldr
            var builder = new DbContextOptionsBuilder<DevTreksContext>();
            builder.UseSqlServer(Helpers.AppSettings.GetConnection(uri));
            _dataContext = new DevTreksContext(builder.Options);
        }
        //EF data access (supplemented with direct calls to sqlio for sqlclient)
        DevTreksContext _dataContext { get; set; }

        public async Task<bool> InsertNewMemberAsync(
            string aspNetUserId, string email)
        {
            bool bHasAdded = false;
            if (!string.IsNullOrEmpty(email))
            {
                Member member = _dataContext.Member
                    .FirstOrDefault(u => u.AspNetUserId == aspNetUserId);
                //check if member already exists
                if (member == null)
                {
                    //insert email and id into the member table
                    //they can not edit email or user name because
                    //those must be unique email and are used to init member and club
                    Member newMember
                        = new Member(email, aspNetUserId);
                    _dataContext.Member.Add(newMember);
                    await _dataContext.SaveChangesAsync();
                    //bHasAdded = true;
                    if (newMember.PKId != 0)
                    {
                        bHasAdded = await InsertNewClubForMemberAsync(newMember.PKId, email);
                    }
                }
                else
                {
                    bHasAdded = true;
                }
            }
            return bHasAdded;
        }
        public async Task<bool> InsertNewClubForMemberAsync(int memberId, string accountEmail)
        {
            bool bHasAdded = false;
            if (memberId != 0)
            {
                string sAccountName = GetNameFromEmail(accountEmail, memberId);
                var newAccount = new Account
                {
                    AccountName = sAccountName,
                    AccountEmail = accountEmail,
                    AccountURI = string.Empty,
                    AccountDesc = string.Empty,
                    AccountLongDesc = string.Empty,
                    AccountClassId = 1,
                    GeoRegionId = 1,
                    GeoRegion = null,
                    AccountClass = null,
                    AccountToAddIn = null,
                    AccountToAudit = null,
                    AccountToCredit = null,
                    AccountToLocal = null,
                    AccountToMember = null,
                    AccountToNetwork = null,
                    AccountToPayment = null,
                    AccountToService = null
                };
                //join table (each member can join one or more accounts (clubs))
                var newAccountToMember = new AccountToMember
                {
                    MemberId = memberId,
                    //each member is the coordinator of their own club
                    MemberRole = AppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString(),
                    IsDefaultClub = true,
                    //next step after insertion switches the default club to this club
                    //IsDefaultClub = false,
                    ClubDefault = null,
                    Member = null
                };
                try
                {
                    _dataContext.Account.Add(newAccount);
                    await _dataContext.SaveChangesAsync();
                    //foreign key
                    newAccountToMember.AccountId = newAccount.PKId;
                    _dataContext.AccountToMember.Add(newAccountToMember);
                    await _dataContext.SaveChangesAsync();
                    //_dataContext.Entry(newAccount).State = EntityState.Added;
                    //await _dataContext.SaveChangesAsync();
                    //foreign key
                    //newAccountToMember.AccountId = newAccount.PKId;
                    //_dataContext.Entry(newAccountToMember).State = EntityState.Added;
                    //await _dataContext.SaveChangesAsync();
                    bHasAdded = true;
                }
                catch
                {
                    throw;
                }
            }
            return bHasAdded;
        }
        private string GetNameFromEmail(string accountEmail, int memberId)
        {
            string sAccountName = string.Concat("memberId= ", memberId.ToString());
            int iNameLength = accountEmail.IndexOf(GenHelpers.PARAMETER_DELIMITER);
            if (iNameLength <= 1 && accountEmail.Length >= 5)
            {
                iNameLength = 5;
            }
            sAccountName = accountEmail.Substring(0, iNameLength);
            return sAccountName;
        }
        //deprecated in 2.0.0 in favor of uniform EF form edits
        //public async Task<int> InsertNewClubForMemberAsync(ContentURI uri, string accountName,
        //    string accountEmail, int accountGroupId, int accountGroupGeoRegionId)
        //{
        //    int iAccountId = 0;
        //    if (uri.URIMember != null)
        //    {
        //        if (uri.URIMember.Member != null)
        //        {
        //            if (uri.URIMember.Member.PKId != 0)
        //            {
        //                var newAccount = new Account
        //                {
        //                    AccountName = accountName,
        //                    AccountEmail = accountEmail,
        //                    AccountURI = string.Empty,
        //                    AccountDesc = string.Empty,
        //                    AccountLongDesc = string.Empty,
        //                    AccountClassId = accountGroupId,
        //                    GeoRegionId = accountGroupGeoRegionId,
        //                    GeoRegion = null,
        //                    AccountClass = null,
        //                    AccountToAddIn = null,
        //                    AccountToAudit = null,
        //                    AccountToCredit = null,
        //                    AccountToLocal = null,
        //                    AccountToMember = null,
        //                    AccountToNetwork = null,
        //                    AccountToPayment = null,
        //                    AccountToService = null
        //                };
        //                //join table (each member can join one or more accounts (clubs))
        //                var newAccountToMember = new AccountToMember
        //                {
        //                    MemberId = uri.URIMember.Member.PKId,
        //                    //each member is the coordinator of their own club
        //                    MemberRole = AppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString(),
        //                    //next step after insertion switches the default club to this club
        //                    IsDefaultClub = false,
        //                    ClubDefault = null,
        //                    Member = null
        //                };
        //                try
        //                {
        //                    _dataContext.Entry(newAccount).State = EntityState.Added;
        //                    await _dataContext.SaveChangesAsync();
        //                    newAccountToMember.AccountId = newAccount.PKId;
        //                    _dataContext.Entry(newAccountToMember).State = EntityState.Added;
        //                    await _dataContext.SaveChangesAsync();
        //                    iAccountId = newAccount.PKId;
        //                    //160
        //                    newAccountToMember.Member = new Member(uri.URIMember.Member);
        //                    uri.URIMember = newAccountToMember;
        //                }
        //                catch
        //                {
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    return iAccountId;
        //}
        public async Task<Network> GetNetworkByIdAsync(ContentURI uri, int id)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            Network network = await oNetworkHelper.GetNetworkAsync(uri, id.ToString());
            //152 uses ef only to edit content
            //network = await _dataContext.Network.SingleOrDefaultAsync(x => x.PKId == id);
            return network;
        }
        public async Task<Network> GetNetworkByPartialNameAsync(ContentURI uri, string partName)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            Network network = await oNetworkHelper.GetNetworkAsync(uri, partName);
            //152 uses ef only to edit content
            //network = await _dataContext.Network
            //    .FirstOrDefaultAsync(n => n.NetworkURIPartName == partName);
            return network;
        }
        public async Task<bool> SetDefaultClubAndMemberAsync(ContentURI uri, string userName)
        {
            //first query run for every uri
            //_repository is too slow during start up; so load it async
            AppHelpers.Members oMemberHelper = new AppHelpers.Members();
            bool bHasDefault = await oMemberHelper.SetDefaultClubAndMemberAsync(
                uri, userName);
            return bHasDefault;
        }
        public async Task<Account> GetClubByIdAsync(ContentURI uri, int accountId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            Account club = await oClubHelper.GetClubByIdAsync(uri, accountId); ;
            return club;
        }
        public async Task<List<AccountToMember>> GetClubToMemberByMemberIdAsync(
            ContentURI uri, int memberId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<AccountToMember> colClubToMember 
                = await oClubHelper.GetClubToMemberByMemberIdAsync(uri, memberId);
            return colClubToMember;
        }
        public async Task<AccountToMember> GetDefaultClubByMemberAsync(
            ContentURI uri, int memberId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            AccountToMember defaultClub = new AccountToMember(true);
            List<AccountToMember> colClubToMember
                = await oClubHelper.GetClubToMemberByMemberIdAsync(uri, memberId);
            if (colClubToMember != null)
            {
                defaultClub = colClubToMember.FirstOrDefault(m => m.IsDefaultClub == true);
                if (defaultClub != null)
                {
                    defaultClub.ClubDefault = await GetClubByIdAsync(uri, defaultClub.AccountId);
                }
            }
            return defaultClub;
        }
        public async Task<List<AccountToNetwork>> GetNetworkByClubAsync(
            ContentURI uri, int accountId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<AccountToNetwork> colClubNetwork 
                = await oClubHelper.GetNetworkByClubIdAsync(uri, accountId);
            return colClubNetwork;
        }
        
        public async Task<List<AccountToMember>> GetMemberByClubAsync(
            ContentURI uri, int accountId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<AccountToMember> colClubToMember 
                = await oClubHelper.GetMemberByClubIdAsync(uri, accountId);
            return colClubToMember;
        }
         
        public async Task<AccountToMember> GetMemberByClubAsync(
            ContentURI uri, int accountId, int memberId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            AccountToMember currentMember = new AccountToMember(true);
            List<AccountToMember> colClubToMember
                = await oClubHelper.GetMemberByClubIdAsync(uri, accountId);
            if (colClubToMember != null)
            {
                currentMember = colClubToMember
                    .FirstOrDefault(cm => cm.AccountId == accountId && cm.MemberId == memberId);
            }
            return currentMember;
        }
        public async Task<List<AccountToLocal>> GetLocalsByClubAsync(
            ContentURI uri, int accountId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<AccountToLocal> colClubLocals 
                = await oClubHelper.GetLocalsByClubIdAsync(uri, accountId);
            return colClubLocals;
        }
        public async Task<List<AccountToAddIn>> GetAddInsByClubAsync(
            ContentURI uri, int accountId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<AccountToAddIn> colClubAddIns 
                = await oClubHelper.GetAddInsByClubIdAsync(uri, accountId);
            return colClubAddIns;
        }
        public async Task<List<AccountToService>> GetServiceByClubAsync(
            ContentURI uri, int accountId)
        {
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            //the sp also sets the owning club id
            List<AccountToService> colClubService
                = await oAgreementHelper.GetServiceByClubIdAsync(uri, accountId);
            return colClubService;
        }
        
        public async Task<List<AccountToMember>> GetClubsByServiceAndMemberAsync(
            ContentURI uri, int serviceId, int memberId)
        {
            //this sets club.clubinuse and club.Member
            AppHelpers.Members oMemberHelper = new AppHelpers.Members();
            List<AccountToMember> clubs = await oMemberHelper
                .GetClubByServiceAndMemberAsync(
                uri, serviceId, memberId);
            return clubs;
        }
        /// <summary>
        /// Returns a queryable list of clubs subscribed to a service
        /// </summary>
        public async Task<List<AccountToService>> GetSubscribedClubsByServiceAsync(
            ContentURI uri, int serviceId)
        {
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            //the sp also sets the owning club id
            bool bIsOwner = false;
            List<AccountToService> colClubService
                = await oAgreementHelper
                .GetServiceByServiceIdAsync(uri, serviceId, bIsOwner);
            return colClubService;
        }
        
        public async Task<int> GetDevTreksGroupIdsFromRegionIdAsync(ContentURI uri,
            int accountGeoRegionId, int memberGeoRegionId)
        {
            int iAccountGroupId = 0;
            //can't use datacontext because this is used prior to club and member insertion
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] prams =
			{
				sqlIO.MakeInParam("@AccountGroupGeoRegionId",      SqlDbType.Int, 4, accountGeoRegionId),
                sqlIO.MakeInParam("@MemberGroupGeoRegionId",       SqlDbType.Int, 4, memberGeoRegionId),
                sqlIO.MakeOutParam("@AccountGroupId",	            SqlDbType.Int, 4), 
                sqlIO.MakeOutParam("@MemberGroupId",	            SqlDbType.Int, 4)
			};
            // run the stored procedure (and close the sqlIO connection)
            int iNotUsed = await sqlIO.RunProcIntAsync(
                "0GetDevTreksGroupIdsFromGeoRegionId", prams);
            if (prams[2].Value != System.DBNull.Value)
            {
                iAccountGroupId = (int)prams[2].Value;
            }
            if (prams[3].Value != System.DBNull.Value)
            {
                int memberGroupId = (int)prams[3].Value;
            }
            sqlIO.Dispose();
            return iAccountGroupId;
        }
        public async Task<int> GetUserNameCountForMemberAsync(string userName)
        {
            int iUserNameCount = 
                await _dataContext.Member.CountAsync(m => m.UserName == userName);
            return iUserNameCount;
        }
        public async Task<int> GetEmailCountForMemberAsync(string eMailAddress)
        {
            int iEmailCount = await _dataContext.Member.CountAsync(m => m.MemberEmail == eMailAddress);
            return iEmailCount;
        }
        public async Task<int> GetEmailCountForClubAsync(string eMailAddress)
        {
            int iEmailCount = await _dataContext.Account.CountAsync(c => c.AccountEmail == eMailAddress);
            return iEmailCount;
        }
        public async Task<int> GetClubIdByEmailAsync(string eMailAddress)
        {
            int iAccountId = 0;
            if (await _dataContext.Account.AnyAsync(
                c => c.AccountEmail == eMailAddress))
            {
                Account account = await _dataContext.Account.FirstOrDefaultAsync(
                    c => c.AccountEmail == eMailAddress);
                if (account != null)
                {
                    iAccountId = account.PKId;
                }
            }
            return iAccountId;
        }
        public async Task<int> GetClubToMemberCountAsync(int clubId)
        {
            int iMemberCount = await _dataContext.AccountToMember.CountAsync(m => m.AccountId == clubId);
            return iMemberCount;
        }
        public async Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
          IDictionary<string, string> colUpdates)
        {
            //utility for editing simple, specialized, lists using linq to sql (_dataContext)
            bool bIsUpdated = false;
            //standard DevTreks db edit authorization check
            bool bHasAuthorityToEdit = Helpers.GeneralHelpers.IsDbEdit(uri);
            string sURIPattern = string.Empty;
            int iId = 0;
            ContentURI updateURI = new ContentURI();
            string sEditAttName = string.Empty;
            string sDataType = string.Empty;
            string sSize = string.Empty;
            string sEditAttValue = string.Empty;
            foreach (string sKeyName in colUpdates.Keys)
            {
                //sKeyName should be a 'uripattern;attname;datatype;size' delimited string
                string[] arrUpdateParams
                    = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                sURIPattern = string.Empty;
                sEditAttName = string.Empty;
                sDataType = string.Empty;
                sSize = string.Empty;
                sEditAttValue = string.Empty;
                iId = 0;
                EditHelpers.EditHelper.GetStandardEditNameParams(arrUpdateParams,
                    out sURIPattern, out sEditAttName, out sDataType, out sSize);
                //this method only edits two fields
                if (sEditAttName == AppHelpers.Members.AUTHORIZATION_LEVEL
                    || sEditAttName == AppHelpers.Members.ISDEFAULTCLUB)
                {
                    updateURI.URIPattern = sURIPattern;
                    Helpers.GeneralHelpers.SetURIParams(updateURI);
                    iId = updateURI.URIId;
                    sEditAttValue = colUpdates[sKeyName].ToString();
                    if (!string.IsNullOrEmpty(sEditAttValue))
                    {
                        //_dataContext.ObjectTrackingEnabled = true;
                        if (uri.URIDataManager.AppType
                            == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements
                            && bHasAuthorityToEdit
                            && uri.URIDataManager.SubActionView
                                == Helpers.GeneralHelpers.SUBACTION_VIEWS.services.ToString())
                        {
                            //this is an accounttoservice authorization update
                            bIsUpdated = await UpdateAuthorizationLevelAsync(uri, sEditAttName, sEditAttValue, 
                                iId);
                        }
                        //deprecated in 2.0.0 in favor of uniform EF form edits
                        //else if (uri.URIDataManager.AppType
                        //    == Helpers.GeneralHelpers.APPLICATION_TYPES.members
                        //    && uri.URIDataManager.SubActionView
                        //    == Account.MemberClubList)
                        //{
                        //    //this is an accounttomember isdefaultclub update
                        //    //the memberid is used to update all of the clubs 
                        //    //(not just the change)
                        //    //editattvalue has the accountid of the new club
                        //    iId = Helpers.GeneralHelpers.ConvertStringToInt(sEditAttValue);
                        //    bIsUpdated = await UpdateDefaultClubAsync(uri, sEditAttName, iId);
                        //}

                    }
                }
            }
            return bIsUpdated;
        }
       private async Task<bool> UpdateAuthorizationLevelAsync(ContentURI uri, string editAttName, 
            string editAttValue, int id)
        {
            bool bIsUpdated = false;
            int iAuthorizationLevel = (int)AccountHelper.AUTHORIZATION_LEVELS.viewonly;
            int iAccountId = 0;
            //submit the changes to the database.
            try
            {
                //update the database and db model
                AccountToService existingAC = await _dataContext.AccountToService.SingleOrDefaultAsync(x => x.PKId == id);
                if (existingAC != null)
                {
                    //tell the tracker to change the state is unchanged
                    _dataContext.Entry(existingAC).State = EntityState.Unchanged;
                    //execute the query, and make edit changes
                    if (editAttName.Contains(AppHelpers.Members.AUTHORIZATION_LEVEL))
                    {
                        iAuthorizationLevel
                            = Helpers.GeneralHelpers.ConvertStringToInt(editAttValue);
                        existingAC.AuthorizationLevel = (short)iAuthorizationLevel;
                        iAccountId = existingAC.AccountId;
                        existingAC.Account = null;
                        existingAC.Service = null;
                        _dataContext.Entry(existingAC).State = EntityState.Modified;
                        //state is now modified and can be updated
                        await _dataContext.SaveChangesAsync();
                    }
                    bIsUpdated = true;
                    UpdateListsAsync(uri, iAccountId, iAuthorizationLevel, id);
                }
                
            }
            catch (Exception e)
            {
               uri. ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    e.InnerException.ToString(), "ERROR_INTRO");
            }
            return bIsUpdated;
        }
        //deprecated in 2.0.0 in favor of uniform EF form edits
        //public async Task<bool> UpdateDefaultClubAsync(ContentURI uri, string editAttName,
        //    int id)
        //{
        //    bool bIsUpdated = false;
        //    int iAuthorizationLevel = (int)AccountHelper.AUTHORIZATION_LEVELS.viewonly;
        //    int iAccountId = 0;
        //    try
        //    {
        //        //update the database and db model
        //        IList<AccountToMember> members = await _dataContext.AccountToMember
        //                .Where(cm => cm.MemberId == uri.URIMember.MemberId)
        //                .ToListAsync();
        //        //execute the query, and make edit changes
        //        bool bIsDefaultClub = false;
        //        if (members != null)
        //        {
        //            foreach (AccountToMember cm in members)
        //            {
        //                bIsDefaultClub = false;
        //                AccountToMember existingAM = await _dataContext.AccountToMember.SingleOrDefaultAsync(x => x.PKId == cm.PKId);
        //                if (existingAM != null)
        //                {
        //                    //tell the tracker to change the state is unchanged
        //                    _dataContext.Entry(existingAM).State = EntityState.Unchanged;
        //                    if (cm.AccountId == id)
        //                    {
        //                        if (editAttName == AppHelpers.Members.ISDEFAULTCLUB)
        //                        {
        //                            bIsDefaultClub = true;
        //                            existingAM.IsDefaultClub = bIsDefaultClub;
        //                            //don't set these to null or changes uri
        //                            //existingAM.ClubDefault = null;
        //                            //existingAM.Member = null;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        existingAM.IsDefaultClub = bIsDefaultClub;
        //                        existingAM.ClubDefault = null;
        //                        existingAM.Member = null;
        //                    }
        //                    _dataContext.Entry(existingAM).State = EntityState.Modified;
        //                }
        //            }
        //            await _dataContext.SaveChangesAsync();
        //            bIsUpdated = true;
        //            UpdateListsAsync(uri, iAccountId, iAuthorizationLevel, id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
        //            e.InnerException.ToString(), "ERROR_INTRO");
        //    }
        //    return bIsUpdated;
        //}
        private void UpdateListsAsync(ContentURI uri, int accountId, int authorizationLevel, 
            int id)
        {
            if (uri.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                //update the DevTreks model
                if (uri.URIService != null)
                {
                    if (uri.URIService.Service != null)
                    {
                        if (uri.URIService.Service.SubscribedClubs != null)
                        {
                            if (uri.URIService.Service.SubscribedClubs.Count > 0)
                            {
                                AccountToService oSubscribedClub
                                    = uri.URIService.Service.SubscribedClubs.FirstOrDefault(
                                        c => c.PKId == id);
                                if (oSubscribedClub != null)
                                {
                                    oSubscribedClub.AuthorizationLevel
                                        = (short)authorizationLevel;
                                }
                            }
                        }
                    }
                }
            }
            else if (uri.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                if (uri.URIMember.Member != null)
                {
                    if (uri.URIMember.Member.AccountToMember != null)
                    {
                        foreach (AccountToMember memberClub in uri.URIMember.Member.AccountToMember)
                        {
                            if (memberClub.AccountId == id)
                            {
                                memberClub.IsDefaultClub = true;
                            }
                            else
                            {
                                memberClub.IsDefaultClub = false;
                            }
                        }
                    }
                }
            }
        }
        
        public async Task<bool> UpdateSubscribedMemberCountAsync(ContentURI uri,
            int currentClubMemberCount, int accountId, int joinServiceId,
            int memberCount)
        {
            //service agreement revenues are derived from subscribing clubs member count 
            //(fees are charged per club member)
            bool bHasUpdated = false;
            if (uri.URIMember.ClubInUse != null
                && uri.URIMember.ClubDefault != null
                && joinServiceId != 0 && accountId != 0)
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                  == AccountHelper.AUTHORIZATION_LEVELS.fulledits
                    && uri.URIMember.ClubDefault.PKId == accountId)
                {
                    if (currentClubMemberCount != memberCount)
                    {
                        //update the database and db model
                        try
                        {
                            //update the database and db model
                            AccountToService existingAC = 
                                await _dataContext.AccountToService.SingleOrDefaultAsync(x => x.PKId == joinServiceId);
                            if (existingAC != null)
                            {
                                //tell the tracker to change the state is unchanged
                                _dataContext.Entry(existingAC).State = EntityState.Unchanged;
                                //execute the query, and make edit changes
                                existingAC.Amount1 = currentClubMemberCount;
                                existingAC.LastChangedDate = Helpers.GeneralHelpers.GetDateShortNow();
                                _dataContext.Entry(existingAC).State = EntityState.Modified;
                                //state is now modified and can be updated
                                await _dataContext.SaveChangesAsync();
                                bHasUpdated = true;
                            }
                        }
                        catch (Exception e)
                        {
                            uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                e.InnerException.ToString(), "MEMBERREPOSIT_UPcDateAGREEMENTAMOUNT");
                        }
                    }
                }
            }
            return bHasUpdated;
        }
        
        public async Task<List<AccountToCredit>> GetCreditsByClubAsync(int accountId)
        {
            var q = await _dataContext.AccountToCredit
                .Where(cr => cr.AccountId == accountId)
                .ToListAsync();
            return q;
        }
        
        //update clubdefault.Credit
        public async Task<bool> UpdatePaymentHandlingAsync(AccountToMember loggedInMember,
            string cardNumberHash, string salt)
        {
            bool bIsUpdated = false;
            if (loggedInMember.ClubDefault != null)
            {
                if (loggedInMember.ClubDefault.PKId != 0
                    && loggedInMember.ClubDefault.AccountToCredit != null)
                {
                    bool bNeedsInsert = false;
                    if (loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().PKId == 0
                        || loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().AccountId == 0)
                    {
                        bNeedsInsert = true;
                    }
                    if (!bNeedsInsert)
                    {
                        try
                        {
                            AccountToCredit existingCR = 
                                await _dataContext.AccountToCredit.SingleOrDefaultAsync(x => x.PKId == loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().PKId);
                            if (_dataContext.Entry(existingCR) != null) {
                                //tell the tracker to change the state is unchanged
                                _dataContext.Entry(existingCR).State = EntityState.Unchanged;
                                //2.0.0 changes
                                //update the properties to the new values
                                var currentObjContext = _dataContext.Entry(existingCR);
                                var aToC = loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault();
                                var currentObjContent = new AccountToCredit(aToC);
                                //var existingCRProps = currentObjContext.Metadata.GetProperties();
                                //_dataContext.Entry(existingCR).CurrentValues.SetValues(loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault()); 
                                _dataContext.Entry(existingCR).State = EntityState.Modified;
                                //state is now modified and can be updated
                                await _dataContext.SaveChangesAsync();
                            }
                            bIsUpdated = true;
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    else
                    {
                        AccountToCredit newAccountToCredit 
                                = new AccountToCredit(loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault());
                        newAccountToCredit.AccountId = loggedInMember.AccountId;
                        newAccountToCredit.Account = null;
                        newAccountToCredit.CardFullNumber = cardNumberHash;
                        newAccountToCredit.CardNumberSalt = salt;
                        try
                        {
                            _dataContext.Entry(newAccountToCredit).State = EntityState.Added;
                            await _dataContext.SaveChangesAsync();
                            bIsUpdated = true;
                            loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().PKId = newAccountToCredit.PKId;
                            loggedInMember.ClubDefault.AccountToCredit.FirstOrDefault().CardNumberSalt = newAccountToCredit.CardNumberSalt;
                        }
                        catch
                        {
                            throw;
                        }
                    }

                }
            }
            return bIsUpdated;
        }
        
        public async Task<List<AccountToPayment>> GetPaymentHistoryByClubAsync(int accountId)
        {
            var q = await _dataContext.AccountToPayment
                .Where(a => a.AccountToService.AccountId == accountId)
                .ToListAsync();
            return q;
        }
        public async Task<List<AccountToAudit>> GetClubLastEditedItemsAsync(
            ContentURI uri, int accountId)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            int iNumber = 5;
            List<AccountToAudit> colClubAudits
                = await oClubHelper.GetAuditsByClubIdAsync(uri, accountId, iNumber);
            return colClubAudits;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources.
                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }
            }
        }
    }
}
