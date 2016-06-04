using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace DevTreks.Services
{
    /// <summary>
    ///Purpose:		Member and club management services.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///Besides building and editing models, these services build a new _uri 
    ///from route params, set _uri.Data.from config options,
    ///set additional _uri.from httpcontext, 
    ///allow for network-specific connections in _uri.Network constructor, 
    ///and pass _uri to the repository in its constructor
    /// </summary>
    public class MemberService : IMemberService
    {
        IMemberRepositoryEF _repository { get; set; }
        //2.0.0 changes
        public MemberService(ContentURI uri)
            : this(new DevTreks.Data.SqlRepositories.MemberRepository(uri))
        {
            //do not take the shortcut of setting a _uri = uri here
            //pass uris in each method because uri properties are often set 
            //after the constructor
        }
        private MemberService(IMemberRepositoryEF rep)
        {
            _repository = rep;
            if (_repository == null)
                throw new InvalidOperationException(
                    DevTreks.Exceptions.Errors.GetMessage("REPOSITORY_NOTNULL"));
        }
        public async Task<bool> InsertNewMemberAsync(
            string aspNetUserId, string email)
        {
            bool bHasAdded = await _repository
                .InsertNewMemberAsync(aspNetUserId, email);
            return bHasAdded;
        }
        public async Task<Network> GetNetworkByIdAsync(
            ContentURI uri, int id)
        {
            //use a filter to get the network from the repository
            Network n = await _repository.GetNetworkByIdAsync(
                uri, id);
            return n;
        }
        public async Task<Network> GetNetworkByPartialNameAsync(
            ContentURI uri, string networkPartName)
        {
            //use a filter to get the network from the repository
            int iNetworkId = 1;
            bool bIsNetworkId = int.TryParse(networkPartName, out iNetworkId);
            Network n = new Network(true);
            if (bIsNetworkId)
            {
                n = await GetNetworkByIdAsync(uri, iNetworkId);
            }
            else
            {
                n = await _repository.GetNetworkByPartialNameAsync(uri, networkPartName);
            }
            return n;
        }
        
        //152 switched to sps instead of too slow dbcontext
        public async Task<bool> SetDefaultClubAndMemberAsync(ContentURI uri, string userName)
        {
            //check performance using async too
            bool bHasSet = await _repository.SetDefaultClubAndMemberAsync(uri, userName);
            return bHasSet;
        }
        //this has to be async
        public async Task<bool> SetMemberClubDefaultAsync(ContentURI uri, 
            bool isInitView, int accountId)
        {
            bool bHasSet = false;
            //fills in members panel
            uri.URIMember.ClubDefault.AccountToMember = new List<AccountToMember>();
            uri.URIMember.ClubDefault.AccountToNetwork = new List<AccountToNetwork>();
            uri.URIMember.ClubDefault.AccountToLocal = new List<AccountToLocal>();
            uri.URIMember.ClubDefault.AccountToAddIn = new List<AccountToAddIn>();
            uri.URIMember.ClubDefault.AccountToService = new List<AccountToService>();
            uri.URIMember.Member.AccountToMember = new List<AccountToMember>();
            //get enough of the Member and Club models to fill in Member.ascx view
            Task[] tasks = new Task[6];
            tasks[0] = DoOperation0Async(uri, accountId);
            tasks[1] = DoOperation1Async(uri, accountId);
            tasks[2] = DoOperation2Async(uri, accountId);
            tasks[3] = DoOperation3Async(uri, accountId);
            tasks[4] = DoOperation4Async(uri, accountId);
            tasks[5] = DoOperation5Async(uri, accountId);
            // At this point, all tasks are running at the same time.
            // Now, we await them all.
            await Task.WhenAll(tasks);
            bHasSet = true;
            return bHasSet;
        }
        private async Task DoOperation0Async(ContentURI uri, int accountId)
        {
            uri.URIMember.ClubDefault.AccountToMember 
                = await GetMemberByClubAsync(uri, accountId);
        }
        private async Task DoOperation1Async(ContentURI uri, int accountId)
        {
            uri.URIMember.ClubDefault.AccountToLocal 
                = await GetLocalsByClubAsync(uri, accountId);
        }
        private async Task DoOperation2Async(ContentURI uri, int accountId)
        {
            uri.URIMember.ClubDefault.AccountToNetwork 
                = await GetNetworkByClubAsync(uri, accountId);
        }
        private async Task DoOperation3Async(ContentURI uri, int accountId)
        {
            uri.URIMember.ClubDefault.AccountToAddIn 
                = await GetAddInsByClubAsync(uri, accountId);
        }
        private async Task DoOperation4Async(ContentURI uri, int accountId)
        {
            uri.URIMember.ClubDefault.AccountToService 
                = await GetServiceByClubAsync(uri, accountId);
        }
        private async Task DoOperation5Async(ContentURI uri, int accountId)
        {
            uri.URIMember.Member.AccountToMember 
                = await GetClubsByMemberAsync(uri, uri.URIMember.MemberId);
        }
       
        public async Task<Account> GetClubByIdAsync(ContentURI uri, int accountId)
        {
            Account ownerClub = await _repository.GetClubByIdAsync(uri, accountId);
            if (ownerClub == null)
            {
                //set anonymous club settings
                ownerClub = new Account(true);
            }
            return ownerClub;
        }
        /// <summary>
        /// Returns Clubs for a given member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<AccountToMember>> GetClubsByMemberAsync(
            ContentURI uri, int memberId)
        {
            List<AccountToMember> members 
                = await _repository.GetClubToMemberByMemberIdAsync(uri, memberId);
            return members;
        }
        
        /// <summary>
        /// Returns Member for a given club
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<AccountToMember>> GetMemberByClubAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToMember> colMember 
                = await _repository.GetMemberByClubAsync(uri, accountId);
            return colMember;
        }
        
        /// <summary>
        /// Returns a specific member of a given club
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<AccountToMember> GetMemberByClubAsync(
            ContentURI uri, int accountId, int memberId)
        {
            AccountToMember clubMember = new AccountToMember();
            clubMember = await _repository.GetMemberByClubAsync(uri, accountId, memberId);
            if (clubMember == null)
            {
                //default member
                clubMember = new AccountToMember(true);
                //but use the memberid 
                clubMember.MemberId = memberId;
            }
            return clubMember;
        }
        //this determines whether DevTreks internal security allows a subscribed club to edit
        public async Task<AccountToMember> GetAuthorizedClubByServiceAndMemberAsync(
             ContentURI uri, int serviceId, int memberId)
        {
            AccountToMember club = new AccountToMember(true);
            //this sets club.ClubInUse and club.Member if successful
            List<AccountToMember> colClubToMember =
                await _repository.GetClubsByServiceAndMemberAsync(uri, serviceId, memberId);
            if (colClubToMember != null)
            {
                //first try to get a club with the authority to make edits
                club = colClubToMember.FirstOrDefault(c => c.ClubInUse.PrivateAuthorizationLevel
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits);
                if (club == null)
                {
                    //get a clubinuse that doesn't have authority to edit
                    club = colClubToMember.FirstOrDefault();
                }
            }
            return club;
        }
        /// <summary>
        /// Returns Network for a given club
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<AccountToNetwork>> GetNetworkByClubAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToNetwork> colNetwork
                = await _repository.GetNetworkByClubAsync(uri, accountId);
            return colNetwork;
        }
        
        public static string GetNetworkRoleByClub(ContentURI uri)
        {
            //called from client and must be set during async state management
            string sClubNetworkRole = string.Empty;
            if (uri.URIMember != null)
            {
                if (uri.URIMember.ClubInUse != null)
                {
                    if (uri.URIMember.ClubInUse.AccountToNetwork != null)
                    {
                        AccountToNetwork clubNetwork
                                = uri.URIMember.ClubInUse
                                .AccountToNetwork.FirstOrDefault(n => n.NetworkId == uri.URINetwork.PKId);
                        if (clubNetwork != null)
                        {
                            sClubNetworkRole = clubNetwork.NetworkRole;
                        }
                    }
                }
            }
            return sClubNetworkRole;
        }
        public async Task<List<AccountToLocal>> GetLocalsByClubAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToLocal> colLocals 
                = await _repository.GetLocalsByClubAsync(uri, accountId);
            return colLocals;
        }
        public async Task<List<AccountToAddIn>> GetAddInsByClubAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToAddIn> colAddins 
                = await _repository.GetAddInsByClubAsync(uri, accountId);
            return colAddins;
        }

        /// <summary>
        /// Returns owned and subscribed Service for a given club
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<AccountToService>> GetServiceByClubAsync(
            ContentURI uri, int accountId)
        {
            List<AccountToService> colService = 
                await _repository.GetServiceByClubAsync(uri, accountId);
            return colService;
        }
        /// <summary>
        /// Return a service's list of subscribed clubs
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<AccountToService>> GetSubscribedClubsByServiceAsync(
            ContentURI uri, int serviceId)
        {
            List<AccountToService> colSubscribedClubs 
                = await _repository.GetSubscribedClubsByServiceAsync(uri, serviceId);
            return colSubscribedClubs;
        }
       
        public async Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
          IDictionary<string, string> colUpdates)
        {
            bool bIsUpdated = false;
            bIsUpdated = await _repository.UpdateAsync(uri, colDeletes, colUpdates);
            return bIsUpdated;
        }
        
        public bool ContentCanBeSelectedAndEdited(ContentURI uri)
        {
            bool bIsOkToEditAndSelect = false;
            //keep the rules consistent and simple
            if (DevTreks.Data.Helpers.GeneralHelpers.IsAdminApp(uri.URIDataManager.AppType))
            {
                //admin allows selections and some edits
                bIsOkToEditAndSelect = true;
            }
            else
            {
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    bIsOkToEditAndSelect = true;
                }
                else
                {
                    if (uri.URIService != null)
                    {
                        if (uri.URIService.Service.ServicePrice1 > 0)
                        {
                            //see if they have a subscription
                            //that the payment status is current
                            if (uri.URIMember.ClubInUse.PKId != 0
                                && uri.URIService.Status 
                                == DevTreks.Data.AppHelpers.Agreement.SERVICE_STATUS_TYPES.current.ToString())
                            {

                                bIsOkToEditAndSelect = true;
                            }
                        }
                        else
                        {
                            //no price = free service
                            bIsOkToEditAndSelect = true;
                        }
                    }
                }
            }
            return bIsOkToEditAndSelect;
        }
        public async Task<List<AccountToAudit>> GetClubLastEditedItemsAsync(
            ContentURI uri, int clubId, int numberToReturn)
        {
            //refactor: this qry doesn't always work
            //this abbreviated audit item uses the uri.uripattern as the DISTINCT filter
            List<AccountToAudit> colEditItems =
                (from u in await _repository.GetClubLastEditedItemsAsync(uri, clubId)
                 select u).Take(numberToReturn).Distinct().ToList();
            return colEditItems;
        }

        public async Task<List<AccountToAudit>> GetClubLast5EditedItemsAsync(
            ContentURI uri, int clubId)
        {
            //this abbreviated audit item uses the uri.uripattern as the DISTINCT filter
            List<AccountToAudit> colEditItems =
                (from u in await _repository.GetClubLastEditedItemsAsync(uri, clubId)
                 select u).Take(25).ToList();
            return colEditItems;
        }
        public async Task<List<AccountToCredit>> GetCreditsByClubAsync(int accountId)
        {
            //no contenturi in these methods because credits aren't used yet
            List<AccountToCredit> colCredits 
                = (from cr in await _repository.GetCreditsByClubAsync(accountId)
                select cr).ToList();
            return colCredits;
        }
        public async Task<bool> SetClubCreditAsync(AccountToMember loggedInMember)
        {
            bool bHasSet = false;
            List<AccountToCredit> colCredits 
                = await GetCreditsByClubAsync(loggedInMember.ClubDefault.PKId);
            if (colCredits != null)
            {
                //current design uses a one to one relation (one to many used in db for consistency)
                AccountToCredit clubCredit = colCredits.FirstOrDefault();
                if (clubCredit != null)
                {
                    if (loggedInMember.ClubDefault.AccountToCredit == null)
                    {
                        loggedInMember.ClubDefault.AccountToCredit = new List<AccountToCredit>();
                    }
                    loggedInMember.ClubDefault.AccountToCredit.Add(clubCredit);
                    bHasSet = true;
                }
            }
            return bHasSet;
        }
        
        public async Task<List<AccountToPayment>> GetPaymentsByClubAsync(int accountId)
        {
            List<AccountToPayment> colPayments 
                = (from p in await _repository.GetPaymentHistoryByClubAsync(accountId)
                select p).ToList();
            return colPayments;
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
                if (_repository != null)
                {
                    _repository.Dispose();
                }
            }
        }
    }
}
