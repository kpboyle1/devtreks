using DevTreks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GenHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Data
{
    /// <summary>
    ///Purpose:		Principal interface for searching DevTreks 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public interface ISearchRepositoryEF
    {
        Task<Network> GetNetworkByIdAsync(ContentURI uri, int id);
        Task<Network> GetNetworkByPartialNameAsync(ContentURI uri, string partName);
        //get the networks available for the uri initiating a search (i.e. devtreks.org/agtreks)
        Task<List<Network>> GetNetworkAsync(ContentURI uri);
        //get the networks available for a registered user using a networkfiltertype
        Task<List<Network>> GetNetworkForLoggedinMemberAsync(SearchManager searcher);
        //get the service groups available for the uri initiating a search (i.e. devtreks.org/agtreks)
        Task<List<ServiceClass>> GetServiceGroupsAsync(ContentURI uri);
        //get the network groups available
        Task<List<NetworkClass>> GetNetworkGroupsAsync(ContentURI uri);
        //get the search types available
        Task<List<SearchManager.SearchType>> GetSearchTypesByNetworkIdAndServiceGroupIdAsync(
            ContentURI uri, GenHelpers.SUBAPPLICATION_TYPES subAppType, 
            int networkId, int serviceGroupId);
        //get the search results (stored in a grouped, paginated list)
        Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetSearchAsync(SearchManager searcher);
        //get services that are related (same servicegroupid) to the current search results 
        Task<List<AccountToService>> GetRelatedServiceAsync(SearchManager searcher);

        void Dispose();
    }
}
