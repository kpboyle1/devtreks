using DevTreks.Helpers;
using DevTreks.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.IO;
using DevTreks.Data;
using DataAllHelpers = DevTreks.Data.Helpers;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers.GeneralHelpers;
using Exceptions = DevTreks.Exceptions;
using DevTreks.Services;

namespace DevTreks.ViewModels
{
    /// <summary>
    ///Purpose:		ViewModel class for loading, displaying and editing DevTreks content uris
    ///Author:		www.devtreks.org
    ///Date:		2016, June
    ///References:	
    /// </summary>
    public class ContentViewModel 
    {
        public ContentViewModel(ContentURI initialURI)
        {
            //pass the appsettings configs to initialURI 
            //then finish the contenturi with route, httpcontext, ..., params
            //pass initialURI.to finish off ContentURI
            _initialURI = initialURI;
            //public uri is completed and passed to all services and repositories
            ContentURIData = new ContentURI();
        }
        private ContentURI _initialURI { get; set; }
        public ContentURI ContentURIData { get; set; }

        public string ViewName = ViewDataHelper.CONTENT_VIEW;
        public string Title = AppHelper.GetResource("DEVTREKS_TITLE");
        public string Description = AppHelper.GetResource("DEVTREKS_GOAL");
        public async Task<bool> SetViewAsync(
            RouteData route, HttpContext context,
            IContentService contentService, IMemberService memberService,
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
            ViewDataHelper.GetViews(route, context, sController,
                out bIsInitView, ref sViewName, ref sTitle);
            bHasSet = await SetContentModelAsync(context, sController, sAction,
                bIsInitView, partialUriPattern, contentService, memberService);
            //fill the content model
            bHasSet = await SetContentDataForAllMembersAsync(contentService, memberService, bIsInitView);
            //set any missing images
            await SetContentURIResourceAsync(contentService);
            //set the state of the docs found in the file paths (many views rely on docpaths)
            bHasSet = await SetURIState(contentService);
            //run the server asynch subaction (everything above must be run synch and sequential)
            await RunServerSubActionAsync(contentService, memberService, context);
            //set the restful addresses
            DataHelpers.SetFullURIs(ContentURIData);
            ////set the schema path (uri.schemapath)
            //SetSchemaPath();
            //see if the view should be changed (error, message ...)
            ViewDataHelper.CheckForNewViews(ContentURIData, ref sViewName);
            this.ViewName = sViewName;
            //seo
            SetSearchEngineParams(sTitle);
            //insert audit trail (could use iAuditId for errmsg)
            int iAuditItemId = await InsertAuditTrailAsync(contentService);
            //publish error messages
            AppHelper.PublishErrorMessage(ContentURIData);
            //2.0.0: copy extensions dlls
            //refactor needed: by either getting dnx 
            //or build/publishOptions to copy extension dlls to the needed paths
            await CopyExtensions(ContentURIData);
            return bHasSet;
        }
        private void SetSearchEngineParams(string title)
        {
            //seo
            if (!string.IsNullOrEmpty(ContentURIData.URIName))
            {
                this.Title = ContentURIData.URIName;
            }
            else
            {
                this.Title = title;
            }
            if (ContentURIData.URIDataManager != null)
            {
                if (!string.IsNullOrEmpty(ContentURIData.URIDataManager.Description))
                {
                    this.Description = ContentURIData.URIDataManager.Description;
                }
                else
                {
                    if (ContentURIData.URINodeName == DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        if (ContentURIData.URIService != null)
                        {
                            if (ContentURIData.URIService.Service != null)
                            {
                                if (!string.IsNullOrEmpty(ContentURIData.URIService.Service.ServiceDesc))
                                {
                                    ContentURIData.URIDataManager.Description = ContentURIData.URIService.Service.ServiceDesc;
                                    this.Description = ContentURIData.URIService.Service.ServiceDesc;

                                }
                            }
                        }
                    }
                }
            }
        }
        private async Task<bool> SetContentModelAsync(HttpContext context, string controller, 
            string action, bool isInitView, string partialUriPattern,
            Services.IContentService contentService, Services.IMemberService memberService)
        {
            bool bHasSet = false;
            //2.0.0 moved localization to Startup.cs: SetUserLanguage(context);
            //the order that the content params get set is important
            //set general content properties
            ViewDataHelper vwDataHelper = new ViewDataHelper();
            //this also copies _initialURI.URI.DataManager to ContentURI
            //and then completes each of the static.AppPaths properties
            ContentURIData =
                vwDataHelper.SetInitialModelProperties(_initialURI, context, controller,
                    action, partialUriPattern);
            //2.0.0 contenturi does not init network in constructor, do it here
            await SetNetworkAsync(contentService, memberService);
            //set up initial club and member
            MemberHelper oMemberHelper = new MemberHelper();
            bHasSet = await oMemberHelper.SetClubAndMemberAsync(context, isInitView, memberService,
                ContentURIData);
            //set the initial service and, if necessary, change the app
            bHasSet = await SetServiceAndChangeAppAsync(contentService);
            //set 'rows to return' params sent in the request body
            SetStartRowArgs();
            return bHasSet;
        }
        private async Task SetNetworkAsync(Services.IContentService contentService, 
            Services.IMemberService memberService)
        {
            //actually uses the memberservice to get the network
            ContentURIData.URINetwork = await contentService.GetNetworkAsync(
                memberService, ContentURIData,
                ContentURIData.URINetworkPartName);
        }
        private async Task<bool> SetServiceAndChangeAppAsync(Services.IContentService contentService)
        {
            bool bHasSet = false;
            if (ContentURIData.URINodeName.StartsWith(
                DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()))
            {
                if (ContentURIData.URINodeName != 
                    DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
                {
                    //service nodes can cause apps to change 
                    bHasSet = await contentService.SetServiceAndChangeAppAsync(ContentURIData,
                        ContentURIData.URIId);
                }
            }
            else
            {
                if (ContentURIData.URIDataManager.FormInput != null
                    && ContentURIData.URIDataManager.ServerSubActionType ==
                    DataHelpers.SERVER_SUBACTION_TYPES.searchbyservice)
                {
                    //if the search is using a serviceid filter, set the service
                    string sServiceId 
                        = DataHelpers.GetFormValue(ContentURIData, 
                        SearchViewModel.SERVICE_ID);
                    int iServiceId = 0;
                    int.TryParse(sServiceId, out iServiceId);
                    if (iServiceId != 0)
                    {
                        //app won't change but service filter will be set
                        bHasSet = await contentService.SetServiceAndChangeAppAsync(ContentURIData,
                            iServiceId);
                    }
                }
            }
            return bHasSet;
        }
        public void SetStartRowArgs()
        {
            int iStartRow = 0;
            string sIsForward = "1";
            int iPageSize = DevTreks.Data.Helpers.AppSettings.GetPageSize(ContentURIData);
            int iParentRow = 0;
            ViewDataHelper.GetRowArgs(ContentURIData.URIDataManager.FormInput,
                iPageSize, out iStartRow, out sIsForward, out iParentRow);
            ContentURIData.URIDataManager.StartRow = iStartRow;
            ContentURIData.URIDataManager.IsForward = sIsForward;
            ContentURIData.URIDataManager.PageSize = iPageSize;
            ContentURIData.URIDataManager.ParentStartRow = iParentRow;
        }

        private async Task<bool> SetContentDataForAllMembersAsync(Services.IContentService contentService, 
            Services.IMemberService memberService, bool isInitView)
        {
            bool bHasSet = false;
            ContentURIData.URIDataManager.Ancestors = new List<ContentURI>();
            ContentURIData.URIClub.ClubDocFullPath = string.Empty;
            ContentURIData.URIMember.MemberDocFullPath = string.Empty;
            bool bIsTempDoc = IsTempDoc();
            if (!bIsTempDoc)
            {
                bHasSet = await contentService.SetContentModelAndAncestorsAsync(memberService, ContentURIData, isInitView);
                if (ContentURIData.URIDataManager.ServerActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.select)
                {
                    //adddefaults calls getchildren (from runserversubaction() after insertion)
                    if (ContentURIData.URIDataManager.ServerSubActionType
                        != DataHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                    {
                        //set the children uri, as well as additional navigation/display params
                        await SetChildrenAsync(contentService);
                    }
                }
                else if (ContentURIData.URIDataManager.ServerActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
                {
                    //set the children uri, as well as additional navigation/display params
                    await SetChildrenAsync(contentService);
                }
                else if (ContentURIData.URIDataManager.ServerActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.preview)
                {
                    //162 stopped using EF for preview panel and started using children uris
                    //display allows multimedia content which looks better
                    //set the children uri, as well as additional navigation/display params
                    await SetChildrenAsync(contentService);
                }
                else
                {
                    //set additional navigation/display params
                    SetChildrenNavigationParams();
                }
            }
            else
            {
                await SetTempDocContentAsync(contentService, memberService);
            }
            return bHasSet;
        }
        private bool IsTempDoc()
        {
            bool bIsTempDoc = true;
            if (ContentURIData.URIPattern != string.Empty
                && ContentURIData.URIFileExtensionType
                != DataHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                bIsTempDoc = false;
            }
            else
            {
                if (ContentURIData.URIDataManager.ServerActionType
                   == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.select
                    && ContentURIData.URIId == 0)
                {
                    //networkgroups, clubgroups, ... init with a zero param in select panel
                    bIsTempDoc = false;
                }
            }
            return bIsTempDoc;
        }
        private void SetChildrenNavigationParams()
        {
            DataHelpers.SetAppSearchView(ContentURIData.URIId, 
                ContentURIData.URINodeName, ContentURIData);
        }
        private async Task SetChildrenAsync(Services.IContentService contentService)
        {
            ContentURI grandParentURI = new ContentURI();
            ContentURI parentURI = new ContentURI();
            GetParentURIs(out grandParentURI, out parentURI);
            ContentURIData.URIDataManager.Children = 
                await contentService.GetChildrenAsync(parentURI, ContentURIData);
            AddParentsToChildren(grandParentURI, parentURI);
        }
        private void GetParentURIs(out ContentURI grandParentURI,
            out ContentURI parentURI)
        {
            //this sets up the top header link which causes navigation
            //'backwards' to ancestors
            grandParentURI = null;
            parentURI = null;
            if (ContentURIData.URIDataManager.Ancestors != null)
            {
                int iIndex = ContentURIData.URIDataManager.Ancestors.Count;
                if (ContentURIData.URINodeName == DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                {
                    //skip the service ancestor or they'll need to click twice
                    grandParentURI = (iIndex >= 3) ? ContentURIData.URIDataManager.Ancestors[iIndex - 3] : null;
                    parentURI = (iIndex >= 2) ? ContentURIData.URIDataManager.Ancestors[iIndex - 2] : null;
                }
                else
                {
                    grandParentURI = (iIndex >= 2) ? ContentURIData.URIDataManager.Ancestors[iIndex - 2] : null;
                    parentURI = (iIndex >= 1) ? ContentURIData.URIDataManager.Ancestors[iIndex - 1] : null;
                }
            }
            if (parentURI == null)
            {
                parentURI = new ContentURI(ContentURIData);
            }
            if (grandParentURI == null)
            {
                if (ContentURIData.URIDataManager.AppType
                        == DataHelpers.APPLICATION_TYPES.addins
                    || ContentURIData.URIDataManager.AppType
                        == DataHelpers.APPLICATION_TYPES.networks
                    || ContentURIData.URIDataManager.AppType
                        == DataHelpers.APPLICATION_TYPES.locals
                    || ContentURIData.URIDataManager.AppType
                        == DataHelpers.APPLICATION_TYPES.members
                    || ContentURIData.URIDataManager.AppType
                        == DataHelpers.APPLICATION_TYPES.accounts)
                {
                    grandParentURI = parentURI;
                }
                else
                {
                    grandParentURI = ContentURIData;
                }
            }
        }
        private void AddParentsToChildren(ContentURI grandParentURI,
            ContentURI parentURI)
        {
            if (grandParentURI != null)
            {
                bool bNeedsToBeInChildrenList = 
                    DevTreks.Data.Helpers.ContentHelper.NeedsToBeInChildrenList(ContentURIData);
                if (bNeedsToBeInChildrenList)
                {
                    //these nodes have no children, but instead of showing a meaningless empty list 
                    //show them with their siblings as children, but give them an asterisk to distinguish them from siblings
                    HighLightNodeClickedInSiblingList();
                    if (parentURI != null)
                        ContentURIData.URIDataManager.Children.Insert(0, parentURI);
                    if (grandParentURI != null)
                        ContentURIData.URIDataManager.Children.Insert(0, grandParentURI);
                }
                else
                {
                    //this is the node that was clicked to generate the children
                    //show it's name as the header link
                    if (ContentURIData != null)
                        ContentURIData.URIDataManager.Children.Insert(0, ContentURIData);
                    //this will be the parameters used by the header link to navigate
                    //'backwards' to ancestors
                    if (parentURI != null)
                        ContentURIData.URIDataManager.Children.Insert(0, parentURI);
                }
            }
        }
        private void HighLightNodeClickedInSiblingList()
        {
            //should be able to use generics list manipulation to do the selection
            foreach (ContentURI child in ContentURIData.URIDataManager.Children)
            {
                if (child.URIPattern.Equals(ContentURIData.URIPattern))
                {
                    child.URIName += "*";
                }
            }
        }
        //all resource state management is handled here
        //earlier versions allowed resources to be saved using various methods
        //which allowed stylesheets to set state which 160 eliminated
        public async Task SetContentURIResourceAsync(Services.IContentService contentService)
        {
            
            if (ContentURIData.URIDataManager.ServerSubActionType
                == DataHelpers.SERVER_SUBACTION_TYPES.respondwithhtml)
            {
                if (ContentURIData.URIDataManager.ServerActionType 
                    == DataHelpers.SERVER_ACTION_TYPES.preview
                    || ContentURIData.URIDataManager.ServerActionType
                        == DataHelpers.SERVER_ACTION_TYPES.linkedviews)
                {
                    //152 linked views no longer automatically add images to storage
                    //is they don't exist, need to add them to file system for display
                    bool bIsSet = false;
                    if (ContentURIData.URIMember == null)
                        ContentURIData.URIMember = new AccountToMember();
                    if (ContentURIData.URIMember.ClubInUse == null)
                        ContentURIData.URIMember.ClubInUse = new DevTreks.Models.Account();
                    if (ContentURIData.URIDataManager.LinkedView == null)
                        ContentURIData.URIDataManager.LinkedView = new List<IGrouping<int, ContentURI>>();
                    if (ContentURIData.URIDataManager.LinkedView.Count > 0)
                    {
                        foreach (var urigroup in ContentURIData.URIDataManager.LinkedView)
                        {
                            foreach (var uri in urigroup)
                            {
                                if (uri.URIDataManager.Resource != null)
                                {
                                    foreach (var resourceuri in uri.URIDataManager.Resource)
                                    {
                                        //no difference for now -need to show the images and download resource command
                                        //on new localhosts
                                        if (ContentURIData.URIMember.ClubInUse.PrivateAuthorizationLevel
                                            == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                                        {
                                            bIsSet = await contentService.SetPathAndStoreResourceAsync(resourceuri);
                                        }
                                        else
                                        {
                                            bIsSet = await contentService.SetPathAndStoreResourceAsync(resourceuri);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (ContentURIData.URIDataManager.Children != null)
                    {
                        //161 moved from EF.GetDevTreksContent to GetChildren() (no EF)
                        foreach (var uri in ContentURIData.URIDataManager.Children)
                        {
                            if (uri.URIDataManager.Resource != null)
                            {
                                foreach (var resourceuri in uri.URIDataManager.Resource)
                                {
                                    if (ContentURIData.URIMember.ClubInUse.PrivateAuthorizationLevel
                                        == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                                    {
                                        bIsSet = await contentService.SetPathAndStoreResourceAsync(resourceuri);
                                    }
                                    else
                                    {
                                        bIsSet = contentService.SetPathAndStoreResource(resourceuri);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task SetResourceAsync(Services.IContentService contentService)
        {
            if (ContentURIData.URIDataManager.ServerSubActionType
                == DataHelpers.SERVER_SUBACTION_TYPES.respondwithhtml)
            {
                string sURIPattern = ContentURIData.URIPattern;
                if (sURIPattern.EndsWith(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    || sURIPattern.EndsWith(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
                    || sURIPattern.EndsWith(DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                    || sURIPattern.EndsWith(DataAppHelpers.Resources.RESOURCES_TYPES.resourcegroup.ToString())
                    || sURIPattern.EndsWith(DataAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString()))
                {
                    //set image uris for client caching in browser
                    bool bNeedsFullPath = false;
                    bool bNeedsOneRecord = false;
                    ContentURIData.URIDataManager.Resource =
                       await contentService.GetResourceAsync(ContentURIData,
                        bNeedsOneRecord, bNeedsFullPath, 
                        DevTreks.Data.AppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcetype,
                        DataAppHelpers.Resources.GENERAL_RESOURCE_TYPES.image.ToString());
                }
            }
        }
        private async Task SetTempDocContentAsync(Services.IContentService contentService, 
            Services.IMemberService memberService)
        {
            bool bIsAdminApp
                = DataHelpers.IsAdminApp(ContentURIData.URIDataManager.AppType);
            if (bIsAdminApp)
            {
                DevTreks.Data.Helpers.AppSettings.SetTempDocPathandFileName(ContentURIData);
                ContentURIData.URIDataManager.EditViewEditType
                    = DataHelpers.VIEW_EDIT_TYPES.full;
                //children of base nodes will be AgTreks, BuildTreks ... 
                //children of join nodes will be clubs, members, networks ...
                await SetChildrenAsync(contentService);
            }
            else
            {
                //calcs need linkedviews to run
               bool bHasSet = await contentService.SetContentModelForTempDocsAsync(memberService, ContentURIData);
            }
        }
        public async Task<bool> SetURIState(Services.IContentService contentService)
        {
            bool bHasSet = false;
            bool bNeedsDocState = NeedsDocState(ContentURIData);
            if (bNeedsDocState)
            {
                bHasSet = await contentService.SetURIStateAsync(ContentURIData);
            }
            return bHasSet;
        }
        private static bool NeedsDocState(ContentURI uri)
        {
            bool bNeedsDocState = true;
            if (uri.URIDataManager.ServerActionType
                == DataHelpers.SERVER_ACTION_TYPES.preview)
            {
                //v161 moved from EF Models to URI.URIDataManager.Children
                bNeedsDocState = false;
            }
            if (bNeedsDocState == true
                && uri.URIDataManager.ServerActionType
                == DataHelpers.SERVER_ACTION_TYPES.select)
            {
                bNeedsDocState = false;
            }
            if (bNeedsDocState == true
                && (uri.URIDataManager.ServerSubActionType
                == DataHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.SERVER_SUBACTION_TYPES.submitlistedits
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.SERVER_SUBACTION_TYPES.respondwithform
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.SERVER_SUBACTION_TYPES.submitformedits))
            {
                bNeedsDocState = false;
            }
            //condition is not included
            if (uri.URIPattern == string.Empty
                || uri.URIId == 0)
            {
                bNeedsDocState = false;
            }
            return bNeedsDocState;
        }
        //need a class level cancellation token source
        private CancellationTokenSource _cts;
        private async Task RunServerSubActionAsync(Services.IContentService contentService,
            Services.IMemberService memberService, HttpContext context)
        {
            bool bHasSet = false;
            if (ContentURIData.URIDataManager.FormInput != null)
            {
                //central "event" handler for all edits 
                //usually derived from a clientside onclick event that sends a 
                //SERVER_SUBACTION_TYPES form element in request body
                //custom docs (selectedLinkedViewURI) run when UseSelectedLinkedView = true
                ContentURI selectedLinkedViewURI = new ContentURI();
                switch (ContentURIData.URIDataManager.ServerSubActionType)
                {
                    case DataHelpers.SERVER_SUBACTION_TYPES.submitedits:
                        //deletions
                        StringDictionary colDeletes = new StringDictionary();
                        //updates
                        IDictionary<string, string> colUpdates = new Dictionary<string, string>();
                        DevTreks.Services.Helpers.EditHelper.GetEditParameters(
                            ContentURIData, ref colDeletes, ref colUpdates);
                        if (colDeletes.Count == 0 && colUpdates.Count == 0)
                        {
                            ContentURIData.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "CONTENTVIEW_MAKEEDIT");
                            //fill in the ui collections again
                            ContentURIData.URIDataManager.ServerSubActionType
                                = DataHelpers.SERVER_SUBACTION_TYPES.respondwithhtml;
                            bHasSet = await contentService.GetDevTrekContentAsync(ContentURIData);
                            return;
                        }
                        if (ContentURIData.URIDataManager.UseSelectedLinkedView
                            == false)
                        {
                            //use sqlxml for updates
                            bool bIsOkToSave = await contentService.UpdateAsync(ContentURIData,
                                colDeletes, colUpdates);
                        }
                        else
                        {
                            selectedLinkedViewURI =
                                DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(ContentURIData);
                            await contentService.EnsureEditSecondDocExistsAsync(selectedLinkedViewURI);
                            if (DevTreks.Data.Helpers.FileStorageIO.URIAbsoluteExists(
                                selectedLinkedViewURI, selectedLinkedViewURI.URIClub.ClubDocFullPath))
                            {
                                XElement root = await DevTreks.Data.Helpers.FileStorageIO.LoadXmlElementAsync(
                                    selectedLinkedViewURI, selectedLinkedViewURI.URIClub.ClubDocFullPath);
                                bHasSet = await contentService.UpdateAsync(selectedLinkedViewURI,
                                    colDeletes, colUpdates, root);
                                //set selectedlv html view
                                await SetSelectLinkedViewHtml(contentService);
                            }
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.submitlistedits:
                        colDeletes = new StringDictionary();
                        colUpdates = new Dictionary<string, string>();
                        DevTreks.Services.Helpers.EditHelper.GetEditParameters(
                            ContentURIData, ref colDeletes, ref colUpdates);
                        if (ContentURIData.URIDataManager.SubActionView
                            == DataHelpers.SUBACTION_VIEWS.categories.ToString())
                        {
                            //inserting a new category, or editing an existing category
                            bHasSet = await contentService.UpdateCategoriesAsync(ContentURIData,
                                colDeletes, colUpdates);
                        }
                        else if (ContentURIData.URIDataManager.SubActionView
                            == DataHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                        {
                            //editing an existing linked view join in a list (default lv)
                            bHasSet = await contentService.UpdateLinkedViewAsync(ContentURIData,
                                colDeletes, colUpdates);
                        }
                        else
                        {
                            if (colDeletes.Count == 0 && colUpdates.Count == 0)
                            {
                                ContentURIData.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                    string.Empty, "CONTENTVIEW_MAKEEDIT2");
                                return;
                            }
                            //use linq to sql objects for updates (commons apps simple list edits)
                            bHasSet = await memberService.UpdateAsync(ContentURIData,
                                colDeletes, colUpdates);
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.saveselects:
                        DevTreks.Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS eSelectionOption
                            = DevTreks.Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS.allancestors;
                        //see if a linked view is being used
                        if (ContentURIData.URIDataManager.UseSelectedLinkedView
                            == false)
                        {
                            bHasSet = await contentService.AddSelectionsAsync(ContentURIData,
                                eSelectionOption);
                        }
                        else
                        {
                            selectedLinkedViewURI =
                                DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(ContentURIData);
                            bHasSet = await contentService.AddSelectionsAsync(selectedLinkedViewURI,
                                eSelectionOption);
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.buildtempdoc:
                        //this is a build new temp doc subview
                        DevTreks.Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS eSelectionOption2
                            = DevTreks.Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS.allancestors;
                        //see if a linked view is being used
                        if (ContentURIData.URIDataManager.UseSelectedLinkedView
                            == false)
                        {
                            bHasSet = await contentService.AddSelectionsAsync(ContentURIData,
                                eSelectionOption2);
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.adddefaults:
                        if (ContentURIData.URIDataManager.UseSelectedLinkedView
                            == false)
                        {
                            await AddLocalsAsync(ContentURIData, memberService);
                            bHasSet = await contentService.AddDefaultNodesAsync(memberService, ContentURIData);
                        }
                        else
                        {
                            selectedLinkedViewURI =
                                DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(ContentURIData);
                            await AddLocalsAsync(selectedLinkedViewURI, memberService);
                            bHasSet = await contentService.AddDefaultNodesAsync(memberService, selectedLinkedViewURI);
                            //set selectedlv html view
                            await SetSelectLinkedViewHtml(contentService);
                        }
                        if (ContentURIData.URIDataManager.ServerActionType
                            == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.select)
                        {
                            //set the children uri, including the newly added default node
                            await SetChildrenAsync(contentService);
                            //don't show the default node as a selection pending
                            ContentURIData.URIDataManager.SelectedList = string.Empty;
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.cancel:
                        //cancel the action raised in runaddin
                        if (_cts != null) _cts.Cancel();
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.runaddin:
                        //keep for future use; same with progress reporting
                        _cts = new CancellationTokenSource();
                        if (ContentURIData.URIDataManager.UseSelectedLinkedView
                            == false)
                        {
                            string sSelectedLinkedViewAddInHostName =
                                DevTreks.Data.Helpers.LinqHelpers.SelectedLinkedViewAddInHostName(
                                ContentURIData.URIDataManager.LinkedView);
                            if (!string.IsNullOrEmpty(sSelectedLinkedViewAddInHostName))
                            {
                                bool bIsOKToRun = Services.Helpers.AddInRunHelper.IsOkToRunAddIn(
                                    ContentURIData, sSelectedLinkedViewAddInHostName);
                                if (bIsOKToRun)
                                {
                                    bHasSet = await contentService.RunAddInAsync(
                                        ContentURIData, _cts.Token);
                                    await DisplaySelectLinkedViewHtml(ContentURIData);
                                }
                                else
                                {
                                    //try to display third doc alone
                                    await DisplaySelectViewHtml(ContentURIData);
                                }
                            }
                            else
                            {
                                //let this fall through without generating any error messages
                            }
                        }
                        else
                        {
                            selectedLinkedViewURI =
                                DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(ContentURIData);
                            string sSelectedLinkedViewAddInHostName =
                                DevTreks.Data.Helpers.LinqHelpers.SelectedLinkedViewAddInHostName(
                                selectedLinkedViewURI.URIDataManager.LinkedView);
                            if (!string.IsNullOrEmpty(sSelectedLinkedViewAddInHostName))
                            {
                                bool bIsOKToRun = Services.Helpers.AddInRunHelper.IsOkToRunAddIn(
                                    selectedLinkedViewURI, sSelectedLinkedViewAddInHostName);
                                if (bIsOKToRun)
                                {
                                    bHasSet = await contentService.RunAddInAsync(
                                        selectedLinkedViewURI, _cts.Token);
                                    if (bHasSet)
                                    {
                                        //turn off defaults if they are on
                                        ContentURIData.URIDataManager.UseDefaultAddIn = false;
                                        ContentURIData.URIDataManager.UseDefaultLocal = false;
                                        await DisplaySelectLinkedViewHtml(selectedLinkedViewURI);
                                    }
                                }
                                else
                                {
                                    //try to display third doc alone
                                    await DisplaySelectViewHtml(selectedLinkedViewURI);
                                }
                            }
                            else
                            {
                                //for devpacks without calculators
                                await DisplaySelectViewHtml(selectedLinkedViewURI);
                                return;
                            }
                        }
                        if (bHasSet)
                        {
                            if (!ContentURIData.ErrorMessage.Contains(AppHelper.GetErrorMessage("DISPLAYHELPER_DONTNEEDHTMLDOC")))
                            {
                                ContentURIData.ErrorMessage = string.Empty;
                            }
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.uploadfile:
                        //2.0.0
                        if (context.Request.HasFormContentType)
                        {
                            if (context.Request.Form.Files.Count > 0)
                            {
                                IOHelper oFileUploadHelper = new IOHelper();
                                await oFileUploadHelper.UploadFileAsync(memberService, contentService,
                                    context, ContentURIData);
                            }
                        }
                        else
                        {
                            ContentURIData.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "CONTENTVIEW_NOFILEUPLOAD");
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.downloadfile:
                        //this is the main way that the uri's xml and html is saved in filesystem
                        //requires specific user subaction (to improve scalability, its not 
                        //automatically retrieved after each edit is made)
                        bHasSet = await contentService.SaveURIFirstDocAsync(ContentURIData);
                        if (bHasSet)
                        {
                            if (!ContentURIData.ErrorMessage.Contains(Exceptions.DevTreksErrors.GetMessage("CONTENTSERVICE_HASNEWXMLDOC")))
                            {
                                //only concerned with the base doc at this stage, hide other error messages
                                ContentURIData.ErrorMessage = string.Empty;
                            }
                        }
                        //generate the calculator html to begin running calcs
                        DisplayURIHelper displayURI2 = new DisplayURIHelper();
                        await displayURI2.DisplayURIAsync(ContentURIData, DataHelpers.DOC_STATE_NUMBER.seconddoc);
                        if (ContentURIData.URIDataManager.ServerActionType
                            == DataHelpers.SERVER_ACTION_TYPES.linkedviews)
                        {
                            //open the default calculator
                            ContentURIData.URIDataManager.ServerSubActionType
                                = DataHelpers.SERVER_SUBACTION_TYPES.runaddin;
                        }
                        break;
                    case DataHelpers.SERVER_SUBACTION_TYPES.makepackage:
                        IOHelper oPackageHelper = new IOHelper();
                        await oPackageHelper.MakeZipPackage(contentService, memberService,
                            context, ContentURIData);
                        break;
                    default:
                        //set pending selections (i.e. for action buttons)
                        DevTreks.Services.Helpers.EditHelper.SetSelects(ContentURIData);
                        //set selectedlv html view
                        await SetSelectLinkedViewHtml(contentService);
                        break;
                }
                if (selectedLinkedViewURI != null)
                {
                    if (selectedLinkedViewURI.URIId != 0)
                    {
                        //this should not be a += message -use selectedLV only
                        ContentURIData.ErrorMessage = selectedLinkedViewURI.ErrorMessage;
                    }
                }
            }
        }
       
        //potential cancel code
        //private static async Task UntilCompletionOrCancellation(
        //    Task asyncOp, CancellationToken ct)
        //{
        //    var tcs = new TaskCompletionSource<bool>();
        //    using (ct.Register(() => tcs.TrySetResult(true)))
        //        await Task.WhenAny(asyncOp, tcs.Task);
        //    //return asyncOp;
        //}
        private async Task SetSelectLinkedViewHtml(Services.IContentService contentService)
        {
            if (ContentURIData.URIDataManager.UseSelectedLinkedView == true)
            {
                if (ContentURIData.URIDataManager.AppType != DataHelpers.APPLICATION_TYPES.locals)
                {
                    DisplayURIHelper displayURI = new DisplayURIHelper();
                    //this also transfers error messages to contenturidata
                    await displayURI.DisplayURIAsync(ContentURIData, DataHelpers.DOC_STATE_NUMBER.thirddoc);
                }
            }
        }
     
        private async Task DisplaySelectLinkedViewHtml(ContentURI uri)
        {
           //160 moved all html state management to ContentViewModel
           //generate html views from xml calcs
           DisplayURIHelper displayURI = new DisplayURIHelper();
            //always generate new second docs (save messages are dynamic)
            await displayURI.DisplayURIAsync(uri, DataHelpers.DOC_STATE_NUMBER.seconddoc);
            //only generate the html views after calcs run (not during save -unneeded extra work)
            if (uri.URIDataManager.TempDocSaveMethod
                != DevTreks.Data.Helpers.AddInHelper.SAVECALCS_METHOD.calcs.ToString()
                && uri.URIDataManager.TempDocSaveMethod
                != DevTreks.Data.Helpers.AddInHelper.SAVECALCS_METHOD.analyses.ToString())
            {
                if (uri.URIDataManager.AppType != DataHelpers.APPLICATION_TYPES.locals)
                {
                    await displayURI.DisplayURIAsync(uri, DataHelpers.DOC_STATE_NUMBER.thirddoc);
                }
            }
        }
        private async Task DisplaySelectViewHtml(ContentURI uri)
        {
            DisplayURIHelper displayURI = new DisplayURIHelper();
            await displayURI.DisplayURIAsync(uri, DataHelpers.DOC_STATE_NUMBER.seconddoc);
            if (uri.URIDataManager.ClientActionType == DataHelpers.CLIENTACTION_TYPES.postrequest
                || uri.URIDataManager.ClientActionType == DataHelpers.CLIENTACTION_TYPES.prepaddin)
            {
                if (uri.URIDataManager.AppType != DataHelpers.APPLICATION_TYPES.locals)
                {
                    //they are generating a different view
                    await displayURI.DisplayURIAsync(uri, DataHelpers.DOC_STATE_NUMBER.thirddoc);
                }
            }
        }
        private async Task AddLocalsAsync(ContentURI uri, 
            Services.IMemberService memberService)
        {
            if (uri.URIDataManager.AppType
                == DataHelpers.APPLICATION_TYPES.prices
                || uri.URIDataManager.AppType
                == DataHelpers.APPLICATION_TYPES.economics1)
            {
                //locals come from clubinuse.locals
                uri.URIMember.ClubInUse.AccountToLocal
                    = await memberService.GetLocalsByClubAsync(
                        uri, ContentURIData.URIMember.ClubInUse.PKId);
            }
        }
        private string GetClientArgument(HttpContext context)
        {
            string sClientArg1 = string.Empty;
            //2.0.0
            string[] arrClientArg1 
                = context.Request.Form.FirstOrDefault(k => k.Key == "clientArg1").Value;
            //string[] arrClientArg1 = context.Request.Form.GetValues("clientArg1");
            if (arrClientArg1 != null)
            {
                //clientarg1 is used instead of iserror in script in case an extra stateful client param is needed (i.e. el partialUriPattern)
                //all error messages start with standard "Error;" which client will check
                sClientArg1 = (arrClientArg1.Length >= 0) ? arrClientArg1[0] : string.Empty;
            }
            return sClientArg1;
        }
        private async Task<int> InsertAuditTrailAsync(Services.IContentService contentService)
        {
            //keep this simple until proven need for more auditing arises
            int iAuditItemId = await contentService.InsertAuditItemAsync(ContentURIData);
            return iAuditItemId;
        }
        private async Task CopyExtensions(ContentURI uri)
        {

            //2.0.0 workaround for post build scripts and buildOptions copy not working
            //refactor by getting dnx to copy automatically like any referenced dll
            //probably better than post build scripts for cross platform use
            //BUT, it requires running a release build in Visual Studio, logging in,
            //and previewing data on the Preview panel
            if (ContentURIData.URIMember.ClubInUse.PrivateAuthorizationLevel
                == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
            {
                bool bHasCopied = false;
                //2 debugs 
                string sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\AgBudgetingCalculators\\bin\\Debug\\net461\\AgBudgetingCalculators.dll");
                string sToExtension = Path.GetFullPath("wwwroot\\Extensions\\AgBudgetingCalculators.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\AgBudgetingCalculators\\bin\\Debug\\net461\\AgBudgetingCalculators.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\AgBudgetingCalculators.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\AgResourceStockExtensions\\bin\\Debug\\net461\\AgResourceStockExtensions.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\AgResourceStockExtensions.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\AgResourceStockExtensions\\bin\\Debug\\net461\\AgResourceStockExtensions.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\AgResourceStockExtensions.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\FoodNutrition\\bin\\Debug\\net461\\FoodNutrition.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\FoodNutrition.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\FoodNutrition\\bin\\Debug\\net461\\FoodNutrition.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\FoodNutrition.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\HealthCare\\bin\\Debug\\net461\\HealthCare.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\HealthCare.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\HealthCare\\bin\\Debug\\net461\\HealthCare.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\HealthCare.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\Jace\\bin\\Debug\\net461\\Jace.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\Jace.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\Jace\\bin\\Debug\\net461\\Jace.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\Jace.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\LCA1\\bin\\Debug\\net461\\LCA1.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\LCA1.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\LCA1\\bin\\Debug\\net461\\LCA1.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\LCA1.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\ME2\\bin\\Debug\\net461\\ME2.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\ME2.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\ME2\\bin\\Debug\\net461\\ME2.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\ME2.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\MN1\\bin\\Debug\\net461\\MN1.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\MN1.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\MN1\\bin\\Debug\\net461\\MN1.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\MN1.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPV1\\bin\\Debug\\net461\\NPV1.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPV1.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPV1\\bin\\Debug\\net461\\NPV1.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPV1.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPVCalculators\\bin\\Debug\\net461\\NPVCalculators.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPVCalculators.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPVCalculators\\bin\\Debug\\net461\\NPVCalculators.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPVCalculators.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPVCalculators\\bin\\Debug\\net461\\NPVCalculators.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPVCalculators.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPVCalculators\\bin\\Debug\\net461\\NPVCalculators.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPVCalculators.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);

                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\Numerics\\bin\\Debug\\net461\\Numerics.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\Numerics.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\Numerics\\bin\\Debug\\net461\\Numerics.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\Numerics.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);

                
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\SB1\\bin\\Debug\\net461\\SB1.dll");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\SB1.dll");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);
                sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\SB1\\bin\\Debug\\net461\\SB1.pdb");
                sToExtension = Path.GetFullPath("wwwroot\\Extensions\\SB1.pdb");
                bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                    uri, sFromExtension, sToExtension);


                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\AgBudgetingCalculators\\bin\\Release\\net461\\AgBudgetingCalculators.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\AgBudgetingCalculators.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\AgResourceStockExtensions\\bin\\Release\\net461\\AgResourceStockExtensions.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\AgResourceStockExtensions.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\FoodNutrition\\bin\\Release\\net461\\FoodNutrition.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\FoodNutrition.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\HealthCare\\bin\\Release\\net461\\HealthCare.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\HealthCare.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\Jace\\bin\\Release\\net461\\Jace.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\Jace.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\LCA1\\bin\\Release\\net461\\LCA1.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\LCA1.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\ME2\\bin\\Release\\net461\\ME2.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\ME2.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\MN1\\bin\\Release\\net461\\MN1.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\MN1.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPVCalculators\\bin\\Release\\net461\\NPVCalculators.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPVCalculators.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPVCalculators\\bin\\Release\\net461\\NPVCalculators.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPVCalculators.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\NPV1\\bin\\Release\\net461\\NPV1.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\NPV1.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\Numerics\\bin\\Release\\net461\\Numerics.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\Numerics.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);
                ////1 release
                //sFromExtension = Path.GetFullPath("..\\DevTreks.Extensions\\SB1\\bin\\Release\\net461\\SB1.dll");
                //sToExtension = Path.GetFullPath("wwwroot\\Extensions\\SB1.dll");
                //bHasCopied = await DataAllHelpers.FileStorageIO.CopyNewerFiles(
                //    uri, sFromExtension, sToExtension);

            }
        }
    }
}
