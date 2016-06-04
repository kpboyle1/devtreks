using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Services
{
    /// <summary>
    ///Purpose:		Search engine interface for accessing all DevTreks content.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    ///             
    public interface ISearchService : IDisposable
    {
        Task<Network> GetNetworkByIdAsync(ContentURI uri, int id);
        Task<Network> GetNetworkByPartialNameAsync(ContentURI uri, string networkPartName);
        Task<List<Network>> GetNetworkByNetworkGroupIdAsync(
            SearchManager searcher, int networkid);
        Task<List<Network>> GetNetworkForLoggedinMemberAsync(
            SearchManager searcher, int networkid);
        Task<List<ServiceClass>> GetServiceGroupsAsync(SearchManager searcher,
           int serviceGroupIsSelectedId);
        Task<List<NetworkClass>> GetNetworkGroupsAsync(SearchManager searcher,
            string networkGroupIsSelectedName);
        Task<List<SearchManager.SearchType>> GetSearchTypesByNetworkIdAndServiceGroupIdAsync(
            SearchManager searcher, DataHelpers.SUBAPPLICATION_TYPES subAppType, int typeId, 
            int networkId, int serviceGroupId);
        Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetSearchAsync(SearchManager searcher);
        Task<List<AccountToService>> GetRelatedServiceAsync(SearchManager searcher);
    }
}
