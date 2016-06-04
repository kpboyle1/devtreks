using DevTreks.Models;
using System.Collections.Generic;

namespace DevTreks.Data
{
    /// <summary>
    ///Purpose:		Search model
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///
    ///NOTES:       
    /// </summary>
    public class SearchManager
    {
        public SearchManager()
        {
            this.SearchResult = new ContentURI();
            this.Keywords = Helpers.GeneralHelpers.NONE;
            this.TypeId = 0;
            this.NetworkType = DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.none;
            this.UserLanguage = "en-us";
            this.StartRow = 0;
            this.IsForward = "1";
            this.PageSize = 0;
            this.RowCount = 0;
            this.NetworkGroups = new List<NetworkClass>();
            this.ServiceGroups = new List<ServiceClass>();
            this.Network = new List<Network>();
            this.RelatedService = new List<AccountToService>();
        }
        //model for search headers
        //filter the search results by a network groups i.e. agtreks for ag, buildtreks for construction
        //and offer access to other groups
        public List<NetworkClass> NetworkGroups { get; set; }
        public NetworkClass NetworkGroupSelected { get; set; }

        //filter the search results by one of the principle servicegroups in any networkgroup
        //servicegroups = subapplications (i.e. operations)
        public List<ServiceClass> ServiceGroups { get; set; }
        public ServiceClass ServiceGroupSelected { get; set; }

        //filter the search results by one or more networks and offer access to other networks
        public List<Network> Network { get; set; }
        //the network selected has connection strings
        public Network NetworkSelected { get; set; }

        //services related to a search result (i.e. inputs offered by other clubs)
        public List<AccountToService> RelatedService { get; set; }
        public AccountToService ServiceSelected { get; set; }

        //filter the search results by the services meta "type" (i.e. linkedviews.devdoctype)
        public List<SearchType> SearchTypes { get; set; }
        public SearchType SearchTypeSelected { get; set; }

        //keywords search param (analagous to ContentURI.URICommonName)
        public string Keywords { get; set; }
        //linkedviews, devpacks and resources filter lists by type 
        //(every subapp has a 'type' table that can be used for this purpose)
        public int TypeId { get; set; }

        //registered users can filter results by their networks and service agreement
        public AppHelpers.Networks.NETWORK_FILTER_TYPES NetworkType { get; set; }

        //a search returns a grouped list of ContentURIs (grouped by a parentid)
        public IEnumerable<System.Linq.IGrouping<int, ContentURI>> SearchResults { get; set; }
        public ContentURI SearchResult { get; set; }
        public int StartRow { get; set; }
        public string IsForward { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        
        //registered users can filter results by currentlanguage (selected in browser)
        public string UserLanguage { get; set; }

        //abtraction of the concrete Type classes (i.e. ResourceType)
        public class SearchType
        {
            public int Id { get; set; }
            public string Label { get; set; }
            public string Name { get; set; }
            public int NetworkId { get; set; }
            public int ServiceClassId { get; set; }
            //needed to init the view
            public bool IsSelected { get; set; }
        }

    }
}