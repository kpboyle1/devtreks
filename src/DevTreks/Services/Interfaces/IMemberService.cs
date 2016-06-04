using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace DevTreks.Services
{
    /// <summary>
    ///Purpose:		Member interface for log-in, registration, member management.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public interface IMemberService : IDisposable
    {
        //link (but no foreign key) between aspnetidentity and member
        Task<bool> InsertNewMemberAsync(string aspNetUserId, string email);

        Task<Network> GetNetworkByIdAsync(ContentURI uri, int id);
        Task<Network> GetNetworkByPartialNameAsync(ContentURI uri, string networkPartName);
        
        //vs2013 security and models
        Task<bool> SetDefaultClubAndMemberAsync(ContentURI uri, string userName);
        //set uri.uriclub (the club that owns the uri)
        Task<Account> GetClubByIdAsync(ContentURI uri, int accountId);
        //get the first club fully authorized to use this service with this member
        Task<AccountToMember> GetAuthorizedClubByServiceAndMemberAsync(
            ContentURI uri, int serviceId, int memberId);
        //return a member's list of clubs and a default club
        Task<List<AccountToMember>> GetClubsByMemberAsync(ContentURI uri, int memberId);
        //return a club's list of members
        Task<List<AccountToMember>> GetMemberByClubAsync(ContentURI uri, int accountId);
        //return a specific member of a club
        Task<AccountToMember> GetMemberByClubAsync(ContentURI uri, int accountId, int memberId);
        //return a club's list of networks, including the default
        Task<List<AccountToNetwork>> GetNetworkByClubAsync(ContentURI uri, int accountId);
        //return a club's list of locals, including the default
        Task<List<AccountToLocal>> GetLocalsByClubAsync(ContentURI uri, int accountId);
        //return a club's list of addins, including the default
        Task<List<AccountToAddIn>> GetAddInsByClubAsync(ContentURI uri, int accountId);
        //return a club's list of services, owned and subscribed
        Task<List<AccountToService>> GetServiceByClubAsync(ContentURI uri, int accountId);
        //return a service's list of subscribed clubs 
        //(allowing coordinators to set authorizationlevels for subscribing clubs)
        Task<List<AccountToService>> GetSubscribedClubsByServiceAsync(ContentURI uri, int serviceId);
        //gatekeeper
        bool ContentCanBeSelectedAndEdited(ContentURI uri);
        //deprecated in 2.0.0 in favor of uniform EF form edits
        ////insert a new club for a given member
        //Task<int> InsertNewClubForMemberAsync(ContentURI uri, IDictionary<string, string> updates);
        //set uri.urimember.clubdefault (club displayed in members panel)
        Task<bool> SetMemberClubDefaultAsync(ContentURI uri, bool isInitView, int accountId);
        //simple list item linq to sql updates for commons apps
        Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
           IDictionary<string, string> colUpdates);
        Task<List<AccountToAudit>> GetClubLast5EditedItemsAsync(ContentURI uri, int clubId);
        //set clubinuse.Credit for payment management (revenues and costs)
        Task<bool> SetClubCreditAsync(AccountToMember loggedInMember);
        //future preview
        Task<List<AccountToPayment>> GetPaymentsByClubAsync(int accountId);
    }
}
