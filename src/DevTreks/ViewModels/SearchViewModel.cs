using DevTreks.Data;
using DevTreks.Helpers;
using DevTreks.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;
namespace DevTreks.ViewModels
{
    /// <summary>
    ///Purpose:		View Model class for filling in search views
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	
    /// </summary>
    /// NOTES       The constant strings are search parameters sent back in the 
    ///             request. Two additional search parameters are also sent back, 
    ///             but they are added to the partialUriPattern that is being 
    ///             searched. These are the uri.urinodename (service drop down list)
    ///             and the uri.network.networkid (network drop down list).
    public class SearchViewModel 
    {
        public SearchViewModel(ContentURI initialURI)
        {
            //finish the contenturi with route, httpcontext, ..., params
            //then pass _initialURI to finish appsettings
            this.SearchManagerData = new SearchManager();
            this.SearchManagerData.SearchResult = new ContentURI();
            _initialURI = initialURI;
        }
        private ContentURI _initialURI { get; set; }
        public SearchManager SearchManagerData { get; private set; }
        public const string NETWORK_FILTER_TYPE    = "networkfiltertype";
        public const string SERVICE_ID              = "serviceid";
        private const string TYPE_ID                = "typeid";
        private const string KEYWORDS               = "keywords";
        public string ViewName = ViewDataHelper.SEARCH_VIEW;
        public string Title = AppHelper.GetResource("DEVTREKS_TITLE");
        public string Description = AppHelper.GetResource("DEVTREKS_GOAL");
        public async Task<bool> SetViewAsync(
            RouteData route, HttpContext context,
            ISearchService searchService, IMemberService memberService,
            string partialUriPattern)
        {
            bool bHasSet = false;
            string sViewName = this.ViewName;
            string sTitle = this.Title;
            bool bIsInitView = false;
            string sController = string.Empty;
            string sAction = string.Empty;
            string sContenturipattern = string.Empty;
            ViewDataHelper.GetRouteValues(route, out sController,
                out sAction, out sContenturipattern);
            //?? can get and post actions can be differentiated by this parameter
            if (partialUriPattern == string.Empty)
            {
                partialUriPattern = sContenturipattern;
            }
            ViewDataHelper.GetViews(route, context, sController,
                out bIsInitView, ref sViewName, ref sTitle);
            this.ViewName = sViewName;
            this.Title = sTitle;
            bHasSet = await SetSearchModel(context, sController, sAction, bIsInitView,
                partialUriPattern, searchService, memberService);
            bool bIsNetworkingSearch = IsNetworkingSearch();
            bHasSet = await SetSearchDataForAllUsers(context, sController, searchService,
                    bIsNetworkingSearch);
            if (!HtmlMemberExtensions.UserIsLoggedIn(context))
            {
                bHasSet = await SetSearchDataForPublicMember(searchService,
                    bIsNetworkingSearch);
            }
            else
            {
                bHasSet = await SetSearchDataForLoggedinMember(searchService,
                    bIsNetworkingSearch);
            }
            //set the restful addresses
            DataHelpers.SetFullURIs(this.SearchManagerData.SearchResult);
            //publish error messages
            AppHelper.PublishErrorMessage(this.SearchManagerData.SearchResult);
            return bHasSet;
        }
       
        private async Task<bool> SetSearchModel(HttpContext context, string controller,
            string action, bool isInitView, string partialUriPattern, ISearchService searchService, 
            IMemberService memberService)
        {
            //the order that the search params get set is important
            //set general content properties
            this.SearchManagerData.SearchResult = new ContentURI();
            ViewDataHelper vwDataHelper = new ViewDataHelper();
            this.SearchManagerData.SearchResult =
                vwDataHelper.SetInitialModelProperties(_initialURI, context, controller,
                action, partialUriPattern);

            ////2.0.0 contenturi does not init network in constructor, do it here
            //now it's done below with SetNetwork
            //await SetNetworkAsync(searchService, memberService);

            //set up initial club and member
            MemberHelper oMemberHelper = new MemberHelper();
            bool bHasSet = await oMemberHelper.SetClubAndMemberAsync(context, isInitView, memberService,
                this.SearchManagerData.SearchResult);
            //set the service and, if necessary, change the app
            bHasSet = await SetService(context);
            //set 'rows to return' params sent in the request body
            SetStartRowArgs();
            SetOtherSearchArgs(context);
            SetNetworkType(context);
            return bHasSet;
        }
        public void SetStartRowArgs()
        {
            int iStartRow = 0;
            string sIsForward = "1";
            int iPageSize = DevTreks.Data.Helpers.AppSettings.GetPageSize(this.SearchManagerData.SearchResult);
            //used with content not with search
            int iParentRow = 0;
            ViewDataHelper.GetRowArgs(this.SearchManagerData.SearchResult.URIDataManager.FormInput,
                iPageSize, out iStartRow, out sIsForward, out iParentRow);
            this.SearchManagerData.StartRow = iStartRow;
            this.SearchManagerData.IsForward = sIsForward;
            this.SearchManagerData.PageSize = iPageSize;
        }
        private async Task SetNetworkAsync(Services.ISearchService searchService,
            Services.IMemberService memberService)
        {
            //if a network has been selected as a filter, it was passed to uri.UriNetwork constructor (not uri.URINetworkPartName)
            //but it didn't do a db hit so only partial info on hand
            if (this.SearchManagerData.SearchResult.URINetwork.PKId 
                == 0)
            {
                this.SearchManagerData.SearchResult.URINetwork
                = await memberService.GetNetworkByPartialNameAsync(
                    this.SearchManagerData.SearchResult,
                    this.SearchManagerData.SearchResult.URINetwork.NetworkURIPartName);
            }
            else
            {
                this.SearchManagerData.SearchResult.URINetwork
                = await memberService.GetNetworkByPartialNameAsync(
                    this.SearchManagerData.SearchResult,
                    this.SearchManagerData.SearchResult.URINetwork.PKId.ToString());
            }
        }
        private void SetNetworkType(HttpContext context)
        {
            if (this.SearchManagerData.SearchResult.URIDataManager.FormInput != null)
            {
                //1. the search page's network drop down list selection was added 
                //by the client to the partialUriPattern used to build 
                //this.SearchManagerData.SearchResult, so
                //SearchResult.URINetworkName has been set already

                //2. Private searches can carry out service agreement searches
                //as well as groups of networks searches using the networktype 
                //parameter
                SetNetworkTypeFromForm(context);
            }
            else
            {
                this.SearchManagerData.NetworkType
                    = DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none;
            }
        }
        private void SetNetworkTypeFromForm(HttpContext context)
        {
            if (context.Request != null)
            {
                string sNetworkType
                    = DataHelpers.GetFormValue(this.SearchManagerData.SearchResult, NETWORK_FILTER_TYPE);
                //public searches don't have a network filter type
                this.SearchManagerData.NetworkType
                    = DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none;
                if (string.IsNullOrEmpty(sNetworkType))
                {
                    if (this.SearchManagerData.SearchResult.URIMember.ClubInUse.PKId
                        != 0)
                    {
                        //default search for logged in member is through myagreement
                        this.SearchManagerData.NetworkType
                            = DataAppHelpers.Networks.NETWORK_FILTER_TYPES.myagreement;
                    }
                }
                else
                {
                    this.SearchManagerData.NetworkType 
                        = (DataAppHelpers.Networks.NETWORK_FILTER_TYPES)Enum.Parse(typeof(DataAppHelpers.Networks.NETWORK_FILTER_TYPES), sNetworkType);
                    //= DataAppHelpers.Networks.GetNetworkFilter(sNetworkType);
                }
            }
            else
            {
                this.SearchManagerData.NetworkType
                    = DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none;
            }
        }
        
        public async Task<bool> SetService(HttpContext context)
        {
            //the search page's service drop down list selection is 
            //added to the partialUriPattern being searched
            bool bHasSet = false;
            if (this.SearchManagerData.SearchResult.URINodeName.StartsWith(
                DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()))
            {
                //service nodes can cause apps to change 
                ContentService contentService = new ContentService(this.SearchManagerData.SearchResult);
                bHasSet = await contentService.SetServiceAndChangeAppAsync(this.SearchManagerData.SearchResult,
                    this.SearchManagerData.SearchResult.URIId);
                contentService.Dispose();
            }
            else
            {
                if (this.SearchManagerData.SearchResult.URIDataManager.FormInput != null
                    && this.SearchManagerData.SearchResult.URIDataManager.ServerSubActionType ==
                    DataHelpers.SERVER_SUBACTION_TYPES.searchbyservice)
                {
                    bHasSet = await SetServiceFilter(context);
                }
            }
            return bHasSet;
        }
        private async Task<bool> SetServiceFilter(HttpContext context)
        {
            bool bHasSet = false;
            //if the search is using a serviceid filter, set the service
            string sServiceId
                = DataHelpers.GetFormValue(this.SearchManagerData.SearchResult, SERVICE_ID);
            if (string.IsNullOrEmpty(sServiceId))
            {
                //look in the querystring
                sServiceId = context.Request.Query[SERVICE_ID].ToString();
                //sServiceId = context.Request.QueryString[SERVICE_ID];
            }
            int iServiceId = 0;
            int.TryParse(sServiceId, out iServiceId);
            if (iServiceId != 0)
            {
                ContentService contentService = new ContentService(this.SearchManagerData.SearchResult);
                //app won't change but service filter will be set
                bHasSet = await contentService.SetServiceAndChangeAppAsync(this.SearchManagerData.SearchResult,
                    iServiceId);
                contentService.Dispose();
            }
            return bHasSet;
        }
        private void SetOtherSearchArgs(HttpContext context)
        {
            if (this.SearchManagerData.SearchResult.URIDataManager.FormInput != null)
            {
                SetSearchType(context);
                SetKeywords(context);
            }
            else
            {
                this.SearchManagerData.TypeId = 0;
                this.SearchManagerData.Keywords = "none";
            }
        }
        private void SetSearchType(HttpContext context)
        {
            string sTypeId
                    = DataHelpers.GetFormValue(this.SearchManagerData.SearchResult, TYPE_ID);
            if (string.IsNullOrEmpty(sTypeId))
            {
                //look in the querystring
                sTypeId = context.Request.Query[TYPE_ID].ToString();
                //sTypeId = context.Request.QueryString[TYPE_ID];
            }
            //init with all categories
            int iTypeId = 0;
            int.TryParse(sTypeId, out iTypeId);
            this.SearchManagerData.TypeId = iTypeId;
        }
        private void SetKeywords(HttpContext context)
        {
            this.SearchManagerData.Keywords
                    = DataHelpers.GetFormValue(this.SearchManagerData.SearchResult, KEYWORDS);
            if (string.IsNullOrEmpty(this.SearchManagerData.Keywords))
                if (this.SearchManagerData.SearchResult != null)
                {
                    if (!string.IsNullOrEmpty(this.SearchManagerData.SearchResult.URIName))
                    {
                        this.SearchManagerData.Keywords = this.SearchManagerData.SearchResult.URIName;
                    }
                    else
                    {
                        this.SearchManagerData.Keywords = DataHelpers.NONE;
                    }
                }
                
        }
        private bool IsNetworkingSearch()
        {
            bool bIsNetworkingSearch = false;
            if (this.SearchManagerData.SearchResult.URIDataManager.AppType ==
                    DataHelpers.APPLICATION_TYPES.accounts
                || this.SearchManagerData.SearchResult.URIDataManager.AppType ==
                    DataHelpers.APPLICATION_TYPES.members
                || this.SearchManagerData.SearchResult.URIDataManager.AppType ==
                    DataHelpers.APPLICATION_TYPES.networks)
            {
                bIsNetworkingSearch = true;
            }
            return bIsNetworkingSearch;
        }
        public async Task<bool> SetSearchDataForAllUsers(HttpContext context,
            string controller, ISearchService searchService, bool isAdminSearch)
        {
            bool bHasSet = false;
            //set servicegroup properties
            this.SearchManagerData.ServiceGroups = await searchService.GetServiceGroupsAsync(
                this.SearchManagerData, this.SearchManagerData.SearchResult.URIService.Service.ServiceClassId);
            //set network group properties
            this.SearchManagerData.NetworkGroups = await searchService.GetNetworkGroupsAsync(
                this.SearchManagerData, controller);
            bool bIsAdminApp = DataHelpers.IsAdminApp(this.SearchManagerData.SearchResult.URIDataManager.AppType);
            if (!bIsAdminApp)
            {
                //these apps use the searchtype properties
                int iServiceGroupId = (int) this.SearchManagerData.SearchResult.URIDataManager.SubAppType;
                this.SearchManagerData.SearchTypes 
                    = await searchService.GetSearchTypesByNetworkIdAndServiceGroupIdAsync(
                    this.SearchManagerData, this.SearchManagerData.SearchResult.URIDataManager.SubAppType, 
                    this.SearchManagerData.TypeId, this.SearchManagerData.SearchResult.URINetwork.PKId,
                    iServiceGroupId);
            }
            bHasSet = true;
            return bHasSet;
        }
        public async Task<bool> SetSearchDataForPublicMember(ISearchService searchService, 
            bool isAdminSearch)
        {
            bool bHasSet = false;
            //set network properties
            this.SearchManagerData.Network = await searchService.GetNetworkByNetworkGroupIdAsync(
               this.SearchManagerData, this.SearchManagerData.SearchResult.URINetwork.PKId);
            if (!isAdminSearch)
            {
                //set related services
                this.SearchManagerData.RelatedService 
                    = await searchService.GetRelatedServiceAsync(this.SearchManagerData);
            }
            //this has to be in this order
            //set searchresult properties (cast used on IEnumberable)
            this.SearchManagerData.SearchResults =
                await searchService.GetSearchAsync(this.SearchManagerData);
            bHasSet = true;
            return bHasSet;
        }
        public async Task<bool> SetSearchDataForLoggedinMember(ISearchService searchService, bool isAdminSearch)
        {
            bool bHasSet = false;
            //set network properties
            this.SearchManagerData.Network = await searchService.GetNetworkForLoggedinMemberAsync(
                this.SearchManagerData, this.SearchManagerData.SearchResult.URINetwork.PKId);
            if (!isAdminSearch)
            {
                //set related services
                this.SearchManagerData.RelatedService = await searchService.GetRelatedServiceAsync(
                    this.SearchManagerData);
            }
            //this has to be in this order
            //set searchresult properties
            this.SearchManagerData.SearchResults
                = await searchService.GetSearchAsync(this.SearchManagerData);
            bHasSet = true;
            return bHasSet;
        }
        
    }
}
