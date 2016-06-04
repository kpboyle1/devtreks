using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GenHelpers = DevTreks.Data.Helpers.GeneralHelpers;
using SearchHelpers = DevTreks.Data.Helpers.SearchHelper;
namespace DevTreks.Data.SqlRepositories
{
    /// <summary>
    ///Purpose:		Principal repository for searching DevTreks 
    ///             search results. 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class SearchRepository : ISearchRepositoryEF, IDisposable
    {
        //2.0.0 changes
        //SqlClient data access (not EF and no datacontext construction)
        public SearchRepository(ContentURI uri)
        {
            //uri is for consistency with ContentRep and MemberRep
            //sqlio is newed and disposed in each method call
        }
        public async Task<Network> GetNetworkByIdAsync(ContentURI uri, int id)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            Network network = await oNetworkHelper.GetNetworkAsync(uri, id.ToString());
            return network;
        }
        public async Task<Network> GetNetworkByPartialNameAsync(ContentURI uri, string partName)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            Network network = await oNetworkHelper.GetNetworkAsync(uri, partName);
            return network;
        }
        
        public async Task<List<Network>> GetNetworkAsync(ContentURI uri)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            List<Network> networks
                = await oNetworkHelper.GetNetworkAsync(uri);
            return networks;
        }
        public async Task<List<Network>> GetNetworkForLoggedinMemberAsync(SearchManager searcher)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(searcher.SearchResult);
            SqlDataReader networks = await oNetworkHelper.GetNetworkForLoggedinMemberAsync(sqlIO, 
                searcher);
            List<Network> colNetwork = new List<Network>();
            if (networks != null)
            {
                int iClubNetworkPKId = 0;
                //build a network list to return to the client
                //small inmemory data set so not async
                while (networks.Read())
                {
                    Network network = new Network();
                    iClubNetworkPKId = networks.GetInt32(0);
                    network.PKId = networks.GetInt32(1);
                    network.NetworkName = networks.GetString(2);
                    network.NetworkURIPartName = networks.GetString(3);
                    network.NetworkDesc = networks.GetString(4);
                    network.AdminConnection = networks.GetString(5);
                    network.WebFileSystemPath = networks.GetString(6);
                    network.WebConnection = networks.GetString(7);
                    network.WebDbPath = networks.GetString(8);
                    network.NetworkClassId = networks.GetInt32(9);
                    network.NetworkClass = new NetworkClass();
                    network.NetworkClass.PKId = network.NetworkClassId;
                    network.NetworkClass.NetworkClassName = networks.GetString(10);
                    //version 1.5.2 standardized on this approach
                    network.AdminConnection = Helpers.AppSettings.GetConnection(searcher.SearchResult);
                    network.WebConnection = Helpers.AppSettings.GetConnection(searcher.SearchResult);
                    colNetwork.Add(network);
                }
            }
            sqlIO.Dispose();
            return colNetwork;
        }

        public async Task<List<ServiceClass>> GetServiceGroupsAsync(ContentURI uri)
        {
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            List<ServiceClass> servicegroups
                = await oAgreementHelper.GetServiceGroupsAsync(uri);
            //152 uses ef only to edit content
            //List<ServiceClass> servicegroups
            //    = await _dataContext.ServiceClass.ToListAsync();
            return servicegroups;
        }
        public async Task<List<NetworkClass>> GetNetworkGroupsAsync(ContentURI uri)
        {
            AppHelpers.Networks oNetworkHelper = new AppHelpers.Networks();
            List<NetworkClass> networkgroups
                = await oNetworkHelper.GetNetworkGroupsAsync(uri);
            return networkgroups;
        }
        public async Task<List<SearchManager.SearchType>> GetSearchTypesByNetworkIdAndServiceGroupIdAsync(
            ContentURI uri, GenHelpers.SUBAPPLICATION_TYPES subAppType, 
            int networkId, int serviceGroupId)
        {
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            List<SearchManager.SearchType> colSearchTypes 
                = await oAgreementHelper.GetTypesAsync(uri, networkId, serviceGroupId);
            return colSearchTypes;
        }
        public async Task<List<AccountToService>> GetRelatedServiceAsync(SearchManager searcher)
        {
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            List<AccountToService> colService = await oAgreementHelper.GetServiceAsync(searcher);
            ////fill an IList with the search results
            //colService = await oAgreementHelper.FillClubServiceListAsync(services);
            return colService;
        }
        public async Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetSearchAsync(
            SearchManager searcher)
        {
            //get the searcher records from the db
            SearchHelpers oSearchHelper = new SearchHelpers();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(searcher.SearchResult);
            SqlDataReader searchresults = await oSearchHelper.GetSearchRecordsAsync(
                sqlIO, searcher);
            int iNewRowCount = searcher.RowCount;
            //fill an List with the search results
            List<ContentURI> colURIs = SearchHelpers.FillSearchList(
                searcher.SearchResult, searchresults);
            sqlIO.Dispose();
            searcher.RowCount = iNewRowCount;
            //group the list(queryGroupURIs) in an IEnumerable<IGrouping<int, ContentURI>>) 
            //using their unique parentid property (for display using a parent/children grouping)
            IEnumerable<System.Linq.IGrouping<int, ContentURI>> qryGroupURIs =
                from parent in colURIs
                group parent by ContentURI.GetGroupingParentId(parent.URIDataManager.ParentURIPattern)
                    into parents
                    select parents;
            return qryGroupURIs;
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
                //sql sps dispose themselves
            }
        }
    }
}
