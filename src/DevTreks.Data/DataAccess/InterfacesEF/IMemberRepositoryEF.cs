using DevTreks.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace DevTreks.Data
{
    /// <summary>
    ///Purpose:		Principal interface for social networking. 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public interface IMemberRepositoryEF
    {
        Task<bool> InsertNewMemberAsync(string aspNetUserId, string email);
        Task<Network> GetNetworkByIdAsync(ContentURI uri, int id);
        Task<Network> GetNetworkByPartialNameAsync(ContentURI uri, string partName);
        //stored procedure used to initialize member and default club
        Task<bool> SetDefaultClubAndMemberAsync(ContentURI uri, string userName);
        
        //return a default member and club groupid found in respective regions 
        Task<int> GetDevTreksGroupIdsFromRegionIdAsync(ContentURI uri,
            int accountGeoRegionId, int memberGeoRegionId);
        
        //return an iqueryable list of club to members
        Task<List<AccountToMember>> GetClubToMemberByMemberIdAsync(ContentURI uri, int memberId);
        //return a base club Async(no joins)
        Task<Account> GetClubByIdAsync(ContentURI uri, int accountId);
        //return an iqueryable list of networks for a given club
        Task<List<AccountToNetwork>> GetNetworkByClubAsync(ContentURI uri, int accountId);
        //return an ilist of members for a given club
        Task<List<AccountToMember>> GetMemberByClubAsync(ContentURI uri, int accountId);
        //return a specific member of a club
        Task<AccountToMember> GetMemberByClubAsync(ContentURI uri, int accountId, int memberId);
        //return an iqueryable list of locals for a given club
        Task<List<AccountToLocal>> GetLocalsByClubAsync(ContentURI uri, int accountId);
        //return an iqueryable list of addins for a given club
        Task<List<AccountToAddIn>> GetAddInsByClubAsync(ContentURI uri, int accountId);
        //return an iqueryable list of services owned by the club or subscribed to by the club
        Task<List<AccountToService>> GetServiceByClubAsync(ContentURI uri, int accountId);
        //return clubs with subscriptions to this service (and check authorization levels for editing)
       Task<List<AccountToMember>> GetClubsByServiceAndMemberAsync(ContentURI uri, 
           int serviceId, int memberId);
        //return a service's list of subscribed clubs
        Task<List<AccountToService>> GetSubscribedClubsByServiceAsync(
            ContentURI uri, int serviceId);
        //deprecated in 2.0.0 in favor of uniform EF form edits
        //Task<int>  InsertNewClubForMemberAsync(ContentURI uri, string accountName,
        //    string accountEmail, int accountGroupId,
        //    int accountGroupGeoRegionId);
        Task<int>  GetUserNameCountForMemberAsync(string userName);
        Task<int>  GetEmailCountForMemberAsync(string eMailAddress);
        Task<int>  GetEmailCountForClubAsync(string eMailAddress);
        Task<int>  GetClubIdByEmailAsync(string eMailAddress);
        Task<int>  GetClubToMemberCountAsync(int clubId);
        //simple list item updates for commons apps
        Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates);
        //deprecated in 2.0.0 in favor of uniform EF form edits
        //Task<bool> UpdateDefaultClubAsync(ContentURI uri, string editAttName,
        //    int id);
        
        Task<bool> UpdateSubscribedMemberCountAsync(ContentURI uri, int currentClubMemberCount,
            int accountId, int joinServiceId, int memberCount);
        Task<List<AccountToAudit>> GetClubLastEditedItemsAsync(
            ContentURI uri, int clubId);
        //manage payments/credits
        Task<List<AccountToCredit>> GetCreditsByClubAsync(int accountId);
        Task<bool> UpdatePaymentHandlingAsync(AccountToMember member, string cardNumberHash, string salt);
        Task<List<AccountToPayment>> GetPaymentHistoryByClubAsync(
            int accountId);

        void Dispose();
    }
}
