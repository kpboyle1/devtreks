using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;
using SearchHelpers = DevTreks.Data.Helpers.SearchHelper;

namespace DevTreks.Services
{
    /// <summary>
    ///Purpose:		Search engine service for accessing DevTreks URIs.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class SearchService : ISearchService
    {
        ISearchRepositoryEF _repository { get; set; }
        //2.0.0 changes
        public SearchService(ContentURI uri)
            : this(new DevTreks.Data.SqlRepositories.SearchRepository(uri))
        { }
        private SearchService(ISearchRepositoryEF rep)
        {
            _repository = rep;
            if (_repository == null)
                throw new InvalidOperationException(
                    DevTreks.Exceptions.DevTreksErrors.GetMessage("REPOSITORY_NOTNULL"));
        }
        public async Task<Network> GetNetworkByIdAsync(ContentURI uri, int id)
        {
            //use a filter to get the network from the repository
            Network n = await _repository.GetNetworkByIdAsync(uri, id);
            return n;
        }
        public async Task<Network> GetNetworkByPartialNameAsync(
            ContentURI uri, string networkPartName)
        {
            //use a filter to get the network from the repository
            int iNetworkId = 1;
            bool bIsNetworkId = int.TryParse(networkPartName, out iNetworkId);
            Network n = new Network();
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
        public async Task<List<Network>> GetNetworkByNetworkGroupIdAsync(
            SearchManager searcher, int networkid)
        {
            List<Network> colNetwork 
                = await _repository.GetNetworkAsync(searcher.SearchResult);
            List<Network> colNetworkByGroupId = new List<Network>();
            if (colNetwork != null)
            {
                searcher.NetworkSelected
                        = await SetSelectedNetworkAsync(searcher, 
                        networkid, colNetwork);
                //return only the networks that are part of the network group
                if (searcher.NetworkGroupSelected != null)
                {
                    colNetwork.RemoveAll(n => n.NetworkClassId != searcher.NetworkGroupSelected.PKId);
                }
                //make sure network selected is in list
                if (searcher.NetworkSelected != null)
                {
                    if (!colNetwork.Any(n => n.PKId == searcher.NetworkSelected.PKId))
                    {
                        colNetwork.Add(searcher.NetworkSelected);
                    }
                }
            }
            return colNetwork;
        }

        public async Task<List<Network>> GetNetworkForLoggedinMemberAsync(
            SearchManager searcher, int networkid)
        {
            List<Network> colNetwork = new List<Network>();
            //myagreement searches don't allow network filters (queries don't use NetworkId)
            if (searcher.NetworkType
                != DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.myagreement)
            {
                colNetwork 
                    = await _repository.GetNetworkForLoggedinMemberAsync(searcher);
                searcher.NetworkSelected
                    = await SetSelectedNetworkAsync(searcher, networkid, colNetwork);
            }
            else
            {
                colNetwork = new List<Network>();
                searcher.NetworkSelected
                    = await SetSelectedNetworkAsync(searcher, networkid, colNetwork);

            }
            return colNetwork;
        }
        private async Task<Network> SetSelectedNetworkAsync(
            SearchManager searcher, int networkid, List<Network> networks)
        {
            Network networkSelected = new Network(true);
            if (networkid != 0
                && searcher.NetworkType
                == DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworks)
            {
                //switch to an allnetworks search
                networkSelected 
                    = await _repository.GetNetworkByPartialNameAsync(
                        searcher.SearchResult, "0");
            }
            else if (networkid == 0
                && (searcher.NetworkType
                == DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.mynetworks
                || searcher.NetworkType
                == DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworksbygroup))
            {
                //take the first network (this searches each network individually)
                if (networks.Count > 0)
                {
                    networkSelected = networks[0];
                }
            }
            else
            {
                networkSelected = networks.FirstOrDefault(
                       n => n.PKId == networkid);
            }
            if (networkSelected != null)
            {
                networkSelected.IsDefault = true;
            }
            else
            {
                //all networks == 0
                if (networkid != 0 && networks != null)
                {
                    //take the top one
                    if (networks.Count > 0)
                    {
                        networkSelected = networks[0];
                        networkSelected.IsDefault = true;
                    }
                }
            }
            if (networkSelected == null)
            {
                networkSelected = new Network(true);
            }
            return networkSelected;
        }
        public async Task<List<ServiceClass>> GetServiceGroupsAsync(SearchManager searcher, 
            int serviceGroupIsSelectedId)
        {
            List<ServiceClass> colServiceGroups 
                = await _repository.GetServiceGroupsAsync(searcher.SearchResult);
            if (colServiceGroups != null)
            {
                //commons 'services': clubs, members, networks, locals, and services
                SearchHelpers.AddServiceGroupToList(colServiceGroups,
                    DataHelpers.SUBAPPLICATION_TYPES.agreements);
                SearchHelpers.AddServiceGroupToList(colServiceGroups,
                    DataHelpers.SUBAPPLICATION_TYPES.networks);
                SearchHelpers.AddServiceGroupToList(colServiceGroups,
                    DataHelpers.SUBAPPLICATION_TYPES.clubs);
                SearchHelpers.AddServiceGroupToList(colServiceGroups,
                    DataHelpers.SUBAPPLICATION_TYPES.members);
                ServiceClass serviceGroupSelected = colServiceGroups.FirstOrDefault(
                    sg => sg.PKId == serviceGroupIsSelectedId);
                if (serviceGroupSelected != null)
                {
                    serviceGroupSelected.IsSelected = true;
                }
                else
                {
                    //take the top one
                    if (colServiceGroups.Count > 0)
                        serviceGroupSelected = colServiceGroups[0];
                    if (serviceGroupSelected != null)
                        serviceGroupSelected.IsSelected = true;
                }
                if (serviceGroupSelected == null)
                {
                    serviceGroupSelected = new ServiceClass(true);
                }
                searcher.ServiceGroupSelected = serviceGroupSelected;
            }
            return colServiceGroups;
        }

        public async Task<List<NetworkClass>> GetNetworkGroupsAsync(SearchManager searcher,
            string networkGroupIsSelectedName)
        {
            List<NetworkClass> colNetworkGroups 
                = await _repository.GetNetworkGroupsAsync(searcher.SearchResult);
            NetworkClass networkGroupSelected = colNetworkGroups.FirstOrDefault(
                ng => ng.NetworkClassControllerName == networkGroupIsSelectedName);
            if (networkGroupSelected != null)
            {
                networkGroupSelected.IsSelected = true;
            }
            else
            {
                //take the top one
                if (colNetworkGroups.Count > 0)
                    networkGroupSelected = colNetworkGroups[0];
                if (networkGroupSelected != null)
                    networkGroupSelected.IsSelected = true;
            }
            if (networkGroupSelected == null)
            {
                networkGroupSelected = new NetworkClass(true);
            }
            searcher.NetworkGroupSelected = networkGroupSelected;
            return colNetworkGroups;
        }
        public async Task<List<SearchManager.SearchType>> GetSearchTypesByNetworkIdAndServiceGroupIdAsync(
            SearchManager searcher, DataHelpers.SUBAPPLICATION_TYPES subAppType, 
            int typeId, int networkId, int serviceGroupId)
        {
            SearchManager.SearchType searchTypeSelected = new SearchManager.SearchType();
            List<SearchManager.SearchType> colSearchTypes = await _repository
                .GetSearchTypesByNetworkIdAndServiceGroupIdAsync(
                searcher.SearchResult, subAppType, networkId, serviceGroupId);
            searchTypeSelected = colSearchTypes.FirstOrDefault(
                st => st.Id == typeId);
            if (searchTypeSelected != null)
            {
                searchTypeSelected.IsSelected = true;
            }
            else
            {
                if (colSearchTypes.Count > 0)
                    searchTypeSelected = colSearchTypes[0];
                if (searchTypeSelected != null)
                    searchTypeSelected.IsSelected = true;
            }
            if (searchTypeSelected == null)
            {
                searchTypeSelected = new SearchManager.SearchType();
            }
            searcher.SearchTypeSelected = searchTypeSelected;
            return colSearchTypes;
        }
        public async Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetSearchAsync(
            SearchManager searcher)
        {
            //always returns PAGE_SIZE count, groups search results by parentid
            IEnumerable<System.Linq.IGrouping<int, ContentURI>> groupedURIs =
                await _repository.GetSearchAsync(searcher);
            return groupedURIs;
        }
        public async Task<List<AccountToService>> GetRelatedServiceAsync(
            SearchManager searcher)
        {
            List<AccountToService> colService = new List<AccountToService>();
            AccountToService serviceSelected = new AccountToService(true);
            if (searcher.NetworkType
                != DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworks
                && searcher.NetworkSelected.PKId != 0)
            {
                colService = await _repository.GetRelatedServiceAsync(searcher);
            }
            serviceSelected = colService.FirstOrDefault(
                s => s.PKId == searcher.SearchResult.URIService.PKId);
            if (serviceSelected != null)
            {
                serviceSelected.IsSelected = true;
            }
            else
            {
                if (searcher.SearchResult.URIService != null)
                {
                    serviceSelected = searcher.SearchResult.URIService;
                }
                else
                {
                    if (colService.Count > 0)
                        serviceSelected = colService[0];
                }
                if (serviceSelected != null)
                    serviceSelected.IsSelected = true;
            }
            if (serviceSelected == null)
            {
                serviceSelected = new AccountToService(true);
            }
            searcher.ServiceSelected = serviceSelected;
            return colService;
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
