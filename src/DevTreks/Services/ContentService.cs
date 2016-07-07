using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers;
using EditHelpers = DevTreks.Data.EditHelpers;

namespace DevTreks.Services
{
    /// <summary>
    ///Purpose:		Service layer for managing DevTreks content 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///Besides building and editing models, these services build a new uri 
    ///from route params, set uri.Data.from config options,
    ///set additional uri.from httpcontext, 
    ///allow for network-specific connections in uri.Network constructor, 
    ///and pass uri to the repository in its constructor
    /// </summary>
    public class ContentService : IContentService
    {
        //abstract di (but dependent on uri in each method call)
        IContentRepositoryEF _repository { get; set; }
        //repository is dependent on uri for config, context, and rest of state mangt
        private ContentURI uri { get; set; }
        //2.0.0 changes
        //the content repository has config options passed in uri
        //service method calls need uris with more params (route, httpcontext)
        public ContentService(ContentURI uri)
            : this(new DevTreks.Data.SqlRepositories.ContentRepository(uri))
        {}
        private ContentService(IContentRepositoryEF rep)
        {
            //needed config options passed from Startup
            _repository = rep;
            if (_repository == null)
                throw new InvalidOperationException(
                    DevTreks.Exceptions.DevTreksErrors.GetMessage("REPOSITORY_NOTNULL"));
        }
        public ContentURI GetURI(string name, int id, string networkPartName,
            string nodeName, string fileExtensionType)
        {
            ContentURI uri = new ContentURI(name, id, networkPartName,
                nodeName, fileExtensionType);
            return uri;
        }
        public async Task<Network> GetNetworkAsync(IMemberService memberService,
            ContentURI uri, int id)
        {
            Network n = new Network(true);
            n = await memberService.GetNetworkByIdAsync(uri, id);
            return n;
        }
        public async Task<Network> GetNetworkAsync(IMemberService memberService,
            ContentURI uri, string networkPartName)
        {
            Network n = new Network(true);
            //searches by id or part name
            n = await memberService.GetNetworkByPartialNameAsync(uri, networkPartName);
            return n;
        }
        public async Task<bool> SetServiceAndChangeAppAsync(ContentURI uri, int serviceId)
        {
            bool bHasSet = await _repository.SetServiceAndChangeApplicationAsync(uri, serviceId);
            return bHasSet;
        }
        public async Task<List<ContentURI>> GetAncestorsAsync(ContentURI uri)
        {
            List<ContentURI> colURIs = await _repository.GetAncestorsAsync(uri);
            return colURIs;
        }

        public async Task<List<System.Linq.IGrouping<int, ContentURI>>> GetLinkedViewAsync(
            ContentURI uri)
        {
            IEnumerable<System.Linq.IGrouping<int, ContentURI>> groupedURIs = 
                await _repository.GetLinkedViewAsync(uri);
            List<System.Linq.IGrouping<int, ContentURI>> colLinkedView
                = groupedURIs.ToList();
            return colLinkedView;
        }
        public async Task<bool> SetLinkedViewAsync(Services.IMemberService memberService,
            ContentURI uri, bool setLinkedViewMembers)
        {
            bool bHasSet = false;
            //always returns PAGE_SIZE count, but groups search results by parentid
            if ((uri.URINodeName
                != DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                && (uri.URIDataManager.ServerActionType
                != DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.select))
            {
                bool bHasSelectedViewsFromFormEls = true;
                if (uri.URIDataManager.ServerActionType
                    == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                {
                    bHasSelectedViewsFromFormEls
                        = Helpers.AddInStateHelper.HasSelectedViewsFromFormEls(uri);
                    if (bHasSelectedViewsFromFormEls)
                    {
                        bHasSet = await SetUseDefaultsAsync(memberService, uri);
                    }
                }
                //no calc params means the view inits with first doc, not linked views
                if (bHasSelectedViewsFromFormEls)
                {
                    //if coming in from edit panel, start row can be greater than zero and needs resetting
                    if (uri.URIDataManager.ServerSubActionType 
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist)
                        uri.URIDataManager.StartRow = 0;
                    if (uri.URIDataManager.UseDefaultAddIn
                        && uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                    {
                        //default addins need two separate groups of linked views (views and addins)
                        bHasSet = await SetDefaultLinkedViewForDevPackAsync(uri);
                    }
                    else
                    {
                        IEnumerable<System.Linq.IGrouping<int, ContentURI>> groupedURIs = 
                            await _repository.GetLinkedViewAsync(uri);
                        uri.URIDataManager.LinkedView = groupedURIs.ToList();
                    }
                }
            }
            if (setLinkedViewMembers)
            {
                await SetLinkedViewMemberStateAsync(uri);
            }
            return bHasSet;
        }
        private async Task<bool> SetUseDefaultsAsync(IMemberService memberService, 
            ContentURI uri)
        {
            bool bHasSet = false;
            //check to see if they are running default linked views
            string sUseDefaultsAppType = DataHelpers.GeneralHelpers.GetFormValue(uri,
                DataAppHelpers.LinkedViews.USEDEFAULT);
            if (!string.IsNullOrEmpty(sUseDefaultsAppType))
            {
                if (sUseDefaultsAppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.addins.ToString())
                {
                    uri.URIDataManager.UseDefaultAddIn = true;
                }
                else if (sUseDefaultsAppType ==
                    DataHelpers.GeneralHelpers.APPLICATION_TYPES.locals.ToString())
                {
                    //and can always change to default local
                    uri.URIDataManager.UseDefaultLocal = true;
                    //need the uri.urimember.clubinuse.locals
                    uri.URIMember.ClubInUse.AccountToLocal
                       = await memberService.GetLocalsByClubAsync(uri, 
                       uri.URIMember.ClubInUse.PKId);
                    bHasSet = true;
                }
            }
            return bHasSet;
        }
        private async Task<bool> SetLinkedViewMemberStateAsync(ContentURI uri)
        {
            bool bHasSet = false;
            if (uri.URIDataManager.LinkedView != null)
            {
                if (uri.URIDataManager.LinkedView.Count > 0)
                {
                    bHasSet = await SetLinkedViewIsSelectedMemberAsync(uri);
                }
            }
            return bHasSet;
        }
        private async Task<bool> SetDefaultLinkedViewForDevPackAsync(ContentURI uri)
        {
            bool bHasSet = false;
            ContentURI newURI = new ContentURI(uri);
            //get the views
            uri.URIDataManager.UseDefaultAddIn = false;
            IEnumerable<System.Linq.IGrouping<int, ContentURI>> groupedURIs = 
                await _repository.GetLinkedViewAsync(uri);
            uri.URIDataManager.LinkedView = groupedURIs.ToList();
            //get the default addins
            uri.URIDataManager.UseDefaultAddIn = true;
            IEnumerable<System.Linq.IGrouping<int, ContentURI>> groupedURI2s = 
                await _repository.GetLinkedViewAsync(newURI);
            newURI.URIDataManager.LinkedView = groupedURI2s.ToList();
            DataHelpers.LinqHelpers.AddLinkedView2ToLinkedView1(uri.URIDataManager.LinkedView,
                newURI.URIDataManager.LinkedView);
            bHasSet = true;
            return bHasSet;
        }
        public async Task<bool> SetContentModelAndAncestorsAsync(IMemberService memberService, 
            ContentURI uri, bool isInitView)
        {
            bool bHasSet = false;
            //service nodes are transitional (to and from applications and contracts)
            if (uri.URINodeName
                != DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //set before ancestors (filepaths get set better)
                bool bSetLinkedViewMembers = NeedsLinkedViewMembers(uri);
                bHasSet = await SetLinkedViewAsync(memberService, uri, bSetLinkedViewMembers);

                //set the uri's ancestors (shows up as header breadcrumbs)
                //and state management properties i.e. file path to xmldocument
                bHasSet = await SetAncestorsAndAuthorizationsAsync(memberService, uri, isInitView);
            }
            else
            {
                bHasSet = await SetAgreementAncestorsAndAuthorizationAsync(memberService, uri);
            }
            //deprecated in 0.8.7
            if (!Path.HasExtension(uri.URIClub.ClubDocFullPath))
            {
                DataHelpers.ContentHelper.FixFilePaths(uri);
            }

            //ancestors for edit/linkedviews view don't change (easier to navigate among related nodes)
            SwitchAncestors(uri);
            return bHasSet;
        }
        private static bool NeedsLinkedViewMembers(ContentURI uri)
        {
            bool bNeedsLinkedViewMembersState = false;
            if (uri.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
            {
                //linkedviews always need linkedviews state
                //respond with newxhml means that a custom doc is being edited in edits panel
                //need the custom doc loaded, not the apps regular model
                bNeedsLinkedViewMembersState = true;
            }
            else if (uri.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                //custom docs can be edited in edit panel and need state
                bNeedsLinkedViewMembersState = true;
            }
            return bNeedsLinkedViewMembersState;
        }
        public async Task<bool> SetContentModelForTempDocsAsync(IMemberService memberService, ContentURI uri)
        {
            bool bHasSet = false;
            if (uri.URINodeName
                != DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()
                && uri.URIId != 0
                && uri.URINodeName != DataHelpers.GeneralHelpers.NONE)
            {
                //set up main subdirectory (stateful tempdocuri param)
                //note that tempdocs init with the same linkedviewuri pattern obtained below
                if (uri.URIDataManager.TempDocURIPattern == string.Empty)
                    uri.URIDataManager.TempDocURIPattern = uri.URIPattern;
                //set the uri's normal tempdocpaths
                DataHelpers.AppSettings.SetDocPathandFileNameForTempDocs(uri);
                uri.URIDataManager.EditViewEditType
                    = DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                //linkedviewsuri is used to set linkedviews state
                //doctocalcuri is used to to identify the node 
                //within tempdoc needing to be calculated
                string sNodeToCalcURI = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    DataAppHelpers.LinkedViews.DOCTOCALCURI);
                //unlike db docs, tempdocs need an extra param to determine node being calculated
                uri.URIDataManager.TempDocNodeToCalcURIPattern = sNodeToCalcURI;
                if (!string.IsNullOrEmpty(sNodeToCalcURI))
                {
                    ContentURI docToCalcURI = ContentURI.ConvertShortURIPattern(sNodeToCalcURI, uri.URINetwork);
                    bool bNeedsAncestors = false;
                    DataHelpers.GeneralHelpers.SetApps(docToCalcURI);
                    DataHelpers.ContentHelper.UpdateNewURIArgs(uri,
                        docToCalcURI, bNeedsAncestors);
                    //prepare for adding stylesheets
                    if (docToCalcURI.URIDataManager.Resource == null)
                        docToCalcURI.URIDataManager.Resource = new List<ContentURI>();
                    //get the linkedviews
                    bool bSetLinkedViewMembers = true;
                    bHasSet = await SetLinkedViewAsync(memberService, docToCalcURI, bSetLinkedViewMembers);
                    //make doctocalcuri the selectedlinkedviewuri (for now)
                    //and add to linkedviews collection
                    docToCalcURI.URIDataManager.IsSelectedLinkedView = true;
                    docToCalcURI.URIDataManager.ParentURIPattern = uri.URIPattern;
                    DataHelpers.LinqHelpers.AddURIToLinkedView(docToCalcURI,
                        docToCalcURI.URIDataManager.LinkedView);
                    //update uri with docToCalcURI.linkedviews
                    DataHelpers.LinqHelpers.CopyLinkedView(docToCalcURI, uri);
                    //set linkedviews member state using uri
                    bHasSet = await SetLinkedViewMemberStateAsync(uri);
                }
                //set the uri.uridatamanager.linkedviews temp paths
                DataHelpers.ContentHelper.SetFilePathsForTempLinkedView(uri);
            }
            else
            {
                DataHelpers.AppSettings.SetTempDocPathandFileName(uri);
                uri.URIDataManager.EditViewEditType
                    = DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
            }
            return bHasSet;
        }
        public async Task<List<ContentURI>> GetAgreementAncestorsAndSetServiceAsync(ContentURI uri)
        {
            List<ContentURI> colURIs =
                await _repository.GetAgreementAncestorsAndSetServiceAsync(uri);
            return colURIs;
        }
        public async Task<List<ContentURI>> GetAgreementAncestorsByServiceAsync(ContentURI uri, int clubOrMemberId)
        {
            //reverse it so it can be easily added to app ancestors
            List<ContentURI> colURIs =
                await _repository.GetAgreementAncestorsAndAuthorizationsAsync(uri, clubOrMemberId);
            //reversed to get correct parent and grandparent
            colURIs.Reverse();
            return colURIs;
        }
        public async Task<bool> SetServiceAsync(ContentURI uri, bool isBaseService)
        {
            bool bHasSet = await _repository.SetServiceAsync(uri, isBaseService);
            return bHasSet;
        }
        public async Task<bool> SetURIOwnerClubFromServiceAsync(ContentURI uri)
        {
            bool bHasSet = await _repository.SetClubByServiceAsync(uri);
            if (uri.URIClub == null)
            {
                //set anonymous club settings
                uri.URIClub = new Account(true);
            }
            return bHasSet;
        }
        public async Task<bool> SetNetworkCategoriesAsync(ContentURI uri)
        {
            //set the network categories (filtered by uri.service.networkid and uri.service.servicegroupid)
            uri.URIDataManager.NetworkCategories
                = await _repository.GetNetworkCategoriesAsync(uri);
            bool bHasSet = true;
            return bHasSet;
        }
        public async Task<List<ContentURI>> GetChildrenAsync(ContentURI parentURI, ContentURI uri)
        {
            List<ContentURI> colURIs 
                = await _repository.GetChildrenAsync(parentURI, uri);
            return colURIs;
        }
        public async Task<bool> CopyResourceToPackageAsync(
            ContentURI uri, string resourceRelFilePaths,
            int arrayPos, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs)
        {
            bool bHasSet = await _repository.CopyResourceToPackageAsync(
                uri, resourceRelFilePaths,
                arrayPos, rootDirectory, newDirectory,
                parentFileName, zipArgs);
            return bHasSet;
        }
        public async Task<bool> CopyRelatedDataToPackageAsync(
            ContentURI uri, string currentFilePath,
            string packageName, string fileType, string tempPackageRootDirectory,
            bool needsAllRelatedData, IDictionary<string, string> zipArgs)
        {
            bool bHasSet = await _repository.CopyRelatedDataToPackageAsync(
                uri, currentFilePath, packageName,
                fileType, tempPackageRootDirectory, needsAllRelatedData, zipArgs);
            return bHasSet;
        }
        public async Task<bool> PackageFilesAsync(ContentURI uri, string packageFilePathName,
            string packageType, string digitalSignatureType,
            IDictionary<string, string> zipArgs)
        {
            bool bHasSet = await _repository.PackageFilesAsync(uri, packageFilePathName,
                packageType, digitalSignatureType, zipArgs);
            return bHasSet;
        }
        public async Task<bool> SetURIStateAsync(ContentURI uri)
        {
            bool bHasSet = false;
            bool bNeedsDocToCalcFullState
                = Helpers.AddInStateHelper.NeedsDocToCalcFullState(uri);
            if (bNeedsDocToCalcFullState)
            {
                Helpers.AddInStateHelper oAddInStateHelper
                    = new Helpers.AddInStateHelper();
                //sets the state of doctocalcuri and doctocalcuri.uridatamanager.linkedviews
                bHasSet = await oAddInStateHelper.SetDocToCalcFullStateAsync(this, uri);
            }
            return bHasSet;
        }
        public async Task<bool> GetDevTrekContentAsync(ContentURI uri)
        {
            bool bHasSet = false;
            //during edits, content is not retrieved until the edits are completed
            //(saves a db hit)
            bool bIsEditSubAction = DataHelpers.GeneralHelpers.IsEditSubServerAction(uri);
            if (!bIsEditSubAction)
            {
                bool bSaveInFileSystemContent = false;
                bHasSet = await _repository.GetDevTrekContentAsync(uri, bSaveInFileSystemContent);
            }
            return bHasSet;
        }
        public async Task<bool> SaveURIFirstDocAsync(ContentURI uri)
        {
            bool bHasSet = await _repository.SaveURIFirstDocAsync(uri);
            //tell the views layer to save stateful html (for packaging)
            if (bHasSet)
            {
                //no message needed for saving base after calc save action
                if (uri.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile)
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.GetMessage("CONTENTSERVICE_HASNEWXMLDOC");
                }
            }
            return bHasSet;
        }
        public async Task<XmlReader> GetURISecondDocAsync(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            XmlReader reader = await _repository.GetURISecondDocAsync(docToCalcURI,
                calcDocURI);
            return reader;
        }
        public async Task<bool> SetPathAndStoreResourceAsync(ContentURI resourceuri)
        {
            bool bIsSet = await _repository.SetPathAndStoreResourceAsync(resourceuri);
            return bIsSet;
        }
        public bool SetPathAndStoreResource(ContentURI resourceuri)
        {
            bool bIsSet = _repository.SetPathAndStoreResource(resourceuri);
            return bIsSet;
        }
        public async Task<XmlReader> GetURISecondBaseDocAsync(ContentURI uri)
        {
             XmlReader reader = await _repository.GetURISecondBaseDocAsync(uri);
             return reader;
        }
        public async Task<bool> SetAddInNamesAsync(ContentURI uri)
        {
            bool bHasSet = await _repository.SetAddInNamesAsync(uri);
            return bHasSet;
        }
        public async Task<bool> SetURIAddInNamesAsync(ContentURI uri)
        {
            bool bHasSet = await _repository.SetURIAddInNamesAsync(uri);
            return bHasSet;
        }
        //primary interfaces for editing contenturis (using xml data editing methods)
        public async Task<bool> EnsureEditFirstDocExistsAsync(ContentURI uri)
        {
            bool bHasSet = false;
            //same conditions as AddInStateHelper
           bool bIsAuthorizedToEditClub 
                = (uri.URIMember.ClubInUse.PrivateAuthorizationLevel 
                == AccountHelper.AUTHORIZATION_LEVELS.fulledits) 
                ? true : false;
            bool bAuthorizedClubNeedsDbDoc
                = Helpers.AddInStateHelper.NeedsNewDbDoc(uri, bIsAuthorizedToEditClub);
            if ((!string.IsNullOrEmpty(uri.URIClub.ClubDocFullPath))
                && bAuthorizedClubNeedsDbDoc
                && DataHelpers.FileStorageIO.URIAbsoluteExists(uri, uri.URIClub.ClubDocFullPath) == false
                && uri.URIFileExtensionType != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                await SaveURIFirstDocAsync(uri);
            }
            return bHasSet;
        }
        
        public async Task<bool> EnsureEditSecondDocExistsAsync(ContentURI uri)
        {
            bool bHasSet = false;
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, uri.URIClub.ClubDocFullPath) == false)
            {
                XmlReader oSecondDocReader = await GetURISecondBaseDocAsync(uri);
                string sErrorMsg = string.Empty;
                DataHelpers.FileStorageIO fileStorageIO = new DataHelpers.FileStorageIO();
                sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(uri, oSecondDocReader, uri.URIClub.ClubDocFullPath);
                uri.ErrorMessage += sErrorMsg;
                DataHelpers.ContentHelper.SetMemberPath(uri);
            }
            return bHasSet;
        }
        public async Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates)
        {
            bool bIsOkToSave = false;
            if (colDeletes.Count > 0
                || colUpdates.Count > 0)
            {
                //0.8.4 adopted 100% ef db edits
                bIsOkToSave = await UpdateContentAsync(uri, colDeletes, colUpdates);
                if (colDeletes.Count > 0)
                {
                    if (bIsOkToSave
                        && uri.URIFileExtensionType
                        != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        //delete associated files and directories
                        EditHelpers.EditHelper.DeleteSubDirectoriesAndResource(
                            uri, colDeletes);
                    }
                }
                if (bIsOkToSave)
                {
                    //user save their own data
                }
            }
            return bIsOkToSave;
        }
        public async Task<bool> UpdateContentAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates)
        {
            bool bIsOkToSave = false;
            if (colDeletes.Count > 0
                || colUpdates.Count > 0)
            {
                bIsOkToSave = await _repository.UpdateAsync(uri, colUpdates, colDeletes);
            }
            return bIsOkToSave;
        }
        public async Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates, XElement devTrekLinqRoot)
        {
            bool bIsOkToSave = false;
            //general pattern:
            //1. validate edits
            //double check the user's input; attribute names use standard naming conventions 
            //across applications (i.e. Name, Amount)
            //2. update attributes/node in xelement (editable)
            //3. if dbedits, make the db edits
            //4. if everything worked, save the xml doc and tell ui ok to signal success
            if (devTrekLinqRoot != null)
            {
                bool bIsDeletes = false;
                if (colUpdates.Count > 0)
                {
                    bIsOkToSave = await _repository.UpdateAsync(bIsDeletes, uri,
                       colUpdates, colDeletes, devTrekLinqRoot);
                }
                if (colDeletes.Count > 0)
                {
                    bIsDeletes = true;
                    bIsOkToSave = await _repository.UpdateAsync(bIsDeletes, uri,
                        colUpdates, colDeletes, devTrekLinqRoot);
                    if (bIsOkToSave
                        && uri.URIFileExtensionType
                        != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        //delete associated files and directories
                        //note: resource storage files are deleted in repository.delectecollection()
                        EditHelpers.EditHelper.DeleteSubDirectoriesAndResource(
                            uri, colDeletes);
                    }
                }
                if (bIsOkToSave)
                {
                    if (uri.URIDataManager.ServerSubActionType
                        != DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                    {
                        if (uri.URIDataManager.UseSelectedLinkedView == false)
                        {
                            EditHelpers.EditHelper.SaveEdits(devTrekLinqRoot, uri);
                            ////see if totals need to be run
                            //Helpers.EditHelper.RunTotals(uri);
                        }
                        else
                        {
                            Helpers.EditHelper oEditHelper = new DevTreks.Services.Helpers.EditHelper();
                            await oEditHelper.SaveSelectedLinkedViewAsync(uri, devTrekLinqRoot);
                        }
                    }
                    else
                    {
                        //addins handle their own state management
                        //just save the tempdoc edits in the tempdoc path
                        await Helpers.AddInRunHelper.SaveUpdatedAddinResultsAsync(uri, devTrekLinqRoot);
                    }
                }
            }
            return bIsOkToSave;
        }
        public async Task<string> AddLinkedViewAsync(ContentURI uri,
            IDictionary<string, string> newLinkedView,
            bool isSelections)
        {
            string sInsertedIdsArray = string.Empty;
            sInsertedIdsArray = await _repository.AddLinkedViewAsync(uri,
                newLinkedView, isSelections, sInsertedIdsArray);

            if (!string.IsNullOrEmpty(sInsertedIdsArray))
            {
                uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "CONTENTSERVICE_NOLINKEDVIEWADD");
            }
            else
            {
                if (uri.URIDataManager.UseDefaultAddIn
                    || uri.URIDataManager.UseDefaultLocal)
                {
                    //now linked successfully to default, need regular linkedviews
                    uri.URIDataManager.UseDefaultAddIn = false;
                    uri.URIDataManager.UseDefaultLocal = false;
                }
                //runaddins is inserting linkedviews for descendents, not uri,
                //so new linkedviews are not needed.
                if (uri.URIDataManager.ServerSubActionType
                    != DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                {
                    //model needs the new linked views for display (standard list of stories and calculators)
                    uri.URIDataManager.LinkedView = await GetLinkedViewAsync(uri);
                }
            }
            return sInsertedIdsArray;
        }
        
        public async Task<bool> UpdateLinkedViewAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates)
        {
            bool bIsOkToSave = false;
            bIsOkToSave = await UpdateContentAsync(uri, colDeletes, colUpdates);
            if (colDeletes.Count > 0)
            {
                if (bIsOkToSave
                    && uri.URIFileExtensionType
                    != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                {
                    //delete old calculated files
                    DataHelpers.AddInHelper.DeleteOldAddInFiles(uri, 
                        uri.URIClub.ClubDocFullPath, colDeletes);
                }
            }
            if (!bIsOkToSave)
            {
                uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "EDITS_NOCANDO");
            }
            else
            {
                //refresh the list of linkedviews in the model
                uri.URIDataManager.LinkedView = await GetLinkedViewAsync(uri);
            }
            return bIsOkToSave;
        }
        public async Task<bool> UpdateCategoriesAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates)
        {
            bool bIsOkToSave = false;
            string sParentURIPattern = string.Empty;
            string sDefaultNodeURIPattern = string.Empty;
            string sNumberToAdd = string.Empty;
            GetAddDefaultParams(uri,
                out sParentURIPattern, out sDefaultNodeURIPattern,
                out sNumberToAdd);
            if (string.IsNullOrEmpty(sParentURIPattern))
            {
                //ef 4.1 updates
                bIsOkToSave = await UpdateContentAsync(uri, colDeletes, colUpdates);
                
                if (bIsOkToSave)
                {
                    //refresh the list of categories in the model
                    bIsOkToSave = await SetNetworkCategoriesAsync(uri);
                }
            }
            else
            {
                //insert a default category
                string sServiceGroupId = ContentURI.GetURIPatternPart(sDefaultNodeURIPattern, ContentURI.URIPATTERNPART.id);
                int iServiceGroupId = DataHelpers.GeneralHelpers.ConvertStringToInt(sServiceGroupId);
                if (iServiceGroupId == 0)
                {
                    //use uri's service
                    iServiceGroupId = uri.URIService.Service.ServiceClassId;
                }
                string sNetworkId = ContentURI.GetURIPatternPart(sDefaultNodeURIPattern, ContentURI.URIPATTERNPART.network);
                int iNetworkId = DataHelpers.GeneralHelpers.ConvertStringToInt(sNetworkId);
                if (iNetworkId == 0)
                {
                    //use uri's service
                    iNetworkId = uri.URIService.Service.NetworkId;
                }
                if (iServiceGroupId == 0 || iNetworkId == 0)
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "CONTSERVICE_CANTADDCATEGORY");
                }
                else
                {
                    int iNumberToAdd = DataHelpers.GeneralHelpers.ConvertStringToInt(sNumberToAdd);
                    //sqlio insertions
                    bIsOkToSave = await _repository.AddCategoryAsync(uri,
                        iServiceGroupId, iNetworkId, iNumberToAdd);
                    if (bIsOkToSave)
                    {
                        //refresh the list of categories in the model
                        bIsOkToSave = await SetNetworkCategoriesAsync(uri);
                    }
                }
            }
            if (!bIsOkToSave)
            {
                uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "EDITS_NOCANDO");
            }
            else
            {
                
            }
            return bIsOkToSave;
        }
        public async Task<bool> AddSelectionsAsync(ContentURI uri,
            EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
            string selectedAncestorURIPattern, XElement devTrekLinqRoot,
            string numberToAdd)
        {
            bool bIsUpdated = false;
            if (uri.URIDataManager.SubActionView !=
                DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
            {
                bIsUpdated = await _repository.AddSelectionsAsync(uri, selectionOption,
                    selectedAncestorURIPattern, devTrekLinqRoot, numberToAdd);
            }
            else
            {
                //sqlxml and devtreklinqroot manipulation not needed
                selectionOption
                    = EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectedparent;
                bIsUpdated = await AddSelectionsAsync(uri, selectionOption);
            }
            return bIsUpdated;
        }
        public async Task<bool> AddSelectionsAsync(ContentURI uri, 
            EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption)
        {
            bool bHasSet = false;
            Helpers.EditHelper.SetSelectsList(uri);

            //this part may need deprecation
            //if selections from custom mydocs are allowed, need the file name param added to the nodename (easy to do, but a lot of potential docs to read and server-consuming)
            if (!string.IsNullOrEmpty(uri.URIDataManager.SelectedList))
            {
                string sSelectedAncestor = string.Empty;
                Helpers.EditHelper.GetSelectOptions(uri, out selectionOption,
                    out sSelectedAncestor);
                string sNumberToAdd = "1";
                if (selectionOption
                    == EditHelpers.AddHelperLinq.SELECTION_OPTIONS.allancestors
                    || selectionOption
                    == EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectedancestor)
                {
                    bHasSet = await AddSelectionsAsync(uri, selectionOption, sSelectedAncestor, sNumberToAdd);
                    DataHelpers.GeneralHelpers.SaveTempDocsSelectsList(uri);
                    await _repository.SetTempDocStylesheetURIAsync(uri);
                    //turn off the subaction view
                    uri.URIDataManager.SubActionView = string.Empty;
                }
                else if (selectionOption
                    == EditHelpers.AddHelperLinq.SELECTION_OPTIONS.none)
                {
                    //tell displaydevtrek to give build new doc options
                }
                else
                {
                    //see if the selection is for a whole node or just an attribute
                    bHasSet = await AddSelectionsAsync(uri, selectionOption,
                        uri.URIDataManager.SelectionsNodeURIPattern, sNumberToAdd);
                    //selections persisted in formelements so selections can continue w/o clicking selects links
                    Helpers.EditHelper.SetSelectsURIPattern(uri);
                    //selected view
                    Helpers.EditHelper.SetSelectsCalcParams(uri);
                    
                }
            }
            //but keep this part
            if (uri.URIDataManager.SubActionView
                == DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
            {
                //display a full list of the linkedviews
                uri.URIDataManager.ServerSubActionType
                    = DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist;
                uri.URIDataManager.SubActionView
                    = DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString();
                //model needs the new linked views for display
                uri.URIDataManager.LinkedView = await GetLinkedViewAsync(uri);
            }
            return bHasSet;
        }
        private async Task<bool> AddSelectionsAsync(ContentURI uri,
            EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
            string selectedAncestorURIPattern, string numberToAdd)
        {
            bool bIsOkToSave = false;
            //use an XElement (for linq) to edit file where selections go
            XElement oDevTrekLinqDoc = XElement.Parse(DataHelpers.GeneralHelpers.ROOT_NODE);
            //tempdocs and custom devpacks are the only docs edited on edits panel
            if (selectionOption
                == EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectedparent
                && (uri.URIFileExtensionType == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                //|| (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks
                //&& uri.URIDataManager.UseSelectedLinkedView == true)
                || uri.URIDataManager.UseSelectedLinkedView == true))
            {
                oDevTrekLinqDoc = Helpers.EditHelper.GetEditLinqDoc(uri);
            }
         
            if (oDevTrekLinqDoc != null)
            {
                bIsOkToSave = false;
                if (!oDevTrekLinqDoc.HasElements)
                {
                    bIsOkToSave = await _repository.AddSelectionsAsync(uri, selectionOption,
                        selectedAncestorURIPattern, numberToAdd);
                }
                else
                {
                    bIsOkToSave = await AddSelectionsAsync(uri, selectionOption,
                        selectedAncestorURIPattern, oDevTrekLinqDoc, numberToAdd);
                    if (selectionOption
                        != EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectedparent)
                    {
                        //make a temp doc and pass back a tempdoc filename
                        Helpers.EditHelper.MakeTempDocParms(uri);
                    }
                    if (bIsOkToSave)
                    {
                        //save in the file system
                        bool IsDbEdit = DataHelpers.GeneralHelpers.IsDbEdit(uri);
                        if (!IsDbEdit)
                        {
                            if (uri.URIDataManager.UseSelectedLinkedView == false)
                            {
                                EditHelpers.EditHelper.SaveEdits(oDevTrekLinqDoc, uri);
                            }
                            else
                            {
                                Helpers.EditHelper oEditHelper = new DevTreks.Services.Helpers.EditHelper();
                                await oEditHelper.SaveSelectedLinkedViewAsync(uri, oDevTrekLinqDoc);
                            }
                        }
                        else
                        {
                            EditHelpers.EditHelper.SaveEdits(oDevTrekLinqDoc, uri);
                        }
                    }
                }
                if (!bIsOkToSave)
                {
                    //error
                    uri.ErrorMessage += DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "CONTENTSERVICE_CANTSAVESELECTS");
                }
            }
            return bIsOkToSave;
        }

        public async Task<bool> AddDefaultNodesAsync(IMemberService memberService, ContentURI uri)
        {
            bool bIsAdded = false;
            string sParentURIPattern = string.Empty;
            string sDefaultNodeURIPattern = string.Empty;
            string sNumberToAdd = string.Empty;
            GetAddDefaultParams(uri,
                out sParentURIPattern, out sDefaultNodeURIPattern,
                out sNumberToAdd);
            bIsAdded = await SetUseDefaultsAsync(memberService, uri);
            if (!string.IsNullOrEmpty(sParentURIPattern))
            {
                EditHelpers.AddHelperLinq.SELECTION_OPTIONS eSelectionOption =
                    EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectedparent;
                if (!string.IsNullOrEmpty(sDefaultNodeURIPattern))
                {
                    //change non-standard form els before taking any action
                    string sNodeName = ContentURI.GetURIPatternPart(
                        sParentURIPattern, ContentURI.URIPATTERNPART.node);
                    if (sNodeName == DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
                    {
                        //if apptype==agreements, sNumberToAdd is a serviceclass table pkid (i.e. serviceclass.pkid =100 Input
                        sDefaultNodeURIPattern =
                            ContentURI.ChangeURIPatternPart(sDefaultNodeURIPattern,
                            ContentURI.URIPATTERNPART.id, sNumberToAdd);
                        sNumberToAdd = "1";
                    }
                    uri.URIDataManager.SelectedList
                        = DevTreks.Data.EditHelpers.AddHelperLinq.MakeChildParentURIPattern(
                        sDefaultNodeURIPattern, sParentURIPattern);
                    bIsAdded = await AddSelectionsAsync(uri, eSelectionOption, sParentURIPattern,
                        sNumberToAdd);
                }
                if (uri.URIDataManager.SubActionView
                        == DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                {
                    //display a full list of the linkedviews
                    uri.URIDataManager.ServerSubActionType
                        = DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist;
                    uri.URIDataManager.SubActionView
                        = DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString();
                    uri.URIDataManager.UseDefaultLocal = false;
                    uri.URIDataManager.UseDefaultAddIn = false;
                    //model needs the new linked views for display
                    uri.URIDataManager.LinkedView = await GetLinkedViewAsync(uri);
                }
            }
            else
            {
                uri.ErrorMessage += DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "CONTENTSERVICE_NOFORMELS");
            }
            return bIsAdded;
        }
        public async Task<bool> SaveURISecondDocAsync(ContentURI uri, XmlReader reader)
        {
            bool bHasSet = await _repository.SaveURISecondDocAsync(uri, reader);
            return bHasSet;
        }
        public async Task<bool> SaveURISecondBaseDocAsync(ContentURI uri, bool isMetaData,
            string fileName, XmlReader reader)
        {
            bool bHasSet = await _repository.SaveURISecondBaseDocAsync(uri, isMetaData,
                fileName, reader);
            return bHasSet;
        }
        public async Task<bool> SaveURIResourceFileAsync(ContentURI uri, string fileName, int fileLength,
            string mimeType, Stream postedFileStream)
        {
            bool bHasSet = await _repository.SaveURIResourceFileAsync(uri, fileName, fileLength,
                mimeType, postedFileStream);
            return bHasSet;
        }
        public async Task<bool> UpdateURIResourceFileUploadMsgAsync(ContentURI fileUploadURI, string message)
        {
            bool bHasSet = await _repository.UpdateURIResourceFileUploadMsgAsync(fileUploadURI, message);
            return bHasSet;
        }
        public async Task<bool> RunAddInAsync(ContentURI docToCalcURI, CancellationToken cancellationToken)
        {
            bool bHasRan = false;
            ContentURI calcDocURI = new ContentURI();
            Helpers.AddInRunHelper oAddInHelper = new Helpers.AddInRunHelper();
            //retrieve the selected linkedview
            calcDocURI =
                DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI == null)
            {
                //doctocalc.linkedviews holds calculator/analysis params
                calcDocURI = await Helpers.AddInStateHelper.GetCalcDocAsync(this, docToCalcURI);
            }
            if (calcDocURI != null)
            {
                if (calcDocURI.URIDataManager.TempDocPath != string.Empty)
                {
                    //update calcDocURI.URIDataManager.TempDocPath with whatever calculation option form elements were sent back
                    Helpers.EditHelper oEdits = new Helpers.EditHelper();
                    bool bHasGoodAddInParams = await oEdits.UpdateAddInParamsAsync(docToCalcURI,
                        calcDocURI);
                    //lstUpdates contains db fields needing to be updated  
                    //(this includes xmldoc attribute fields of descendent nodes such as fuel calcs)
                    //these must be updated so that future analyses (i.e. energy analysis) can find 
                    //the stateful params they need (i.e. fuel consumed) without having to rerun base calculations
                    IDictionary<string, string> lstUpdates =
                        EditHelpers.XPathIO.GetNameValueList(docToCalcURI,
                            Helpers.AddInStateHelper.GetUpdatesFilePath(calcDocURI.URIDataManager.TempDocPath),
                        DataAppHelpers.General.UPDATE_NODE_LIST_NAME);
                    //run the add-in passing the updated params in sTempCalcDocPath
                    //save the resultant calculations in either sTempCalcDocPath (i.e. inputs)
                    //or sTempDocToCalcPath (i.e. budgets)
                    //deprecated with async: bool bSaveTempDocInDb = false;
                    bool bAddInIsCompleted = await oAddInHelper.RunAddInAsync(this, 
                        docToCalcURI, calcDocURI, lstUpdates, cancellationToken);
                    if (bAddInIsCompleted == false)
                    {
                        if (docToCalcURI.ErrorMessage == string.Empty)
                        {
                            docToCalcURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "CONTENTSERVICE_NOCALCSRUN");
                        }
                    }
                    else
                    {
                        bHasRan = true;
                        //1.2 switched from a no byref bool to 'none'
                        if (docToCalcURI.URIDataManager.TempDocSaveMethod
                          != DataHelpers.AddInHelper.SAVECALCS_METHOD.none.ToString())
                        {
                            //160 asynch refactor
                            bool bHasSet = oAddInHelper.SetLinkedViewFileExtensionType(
                                   this, docToCalcURI, calcDocURI, lstUpdates);
                            ////addins load and find one another using uri.urifileextensiontype
                            //bool bHasSet = await oAddInHelper.SetLinkedViewFileExtensionTypeAsync(
                            //    this, docToCalcURI, calcDocURI, lstUpdates);
                            if (docToCalcURI.URIDataManager.TempDocSaveMethod
                                != DataHelpers.AddInHelper.SAVECALCS_METHOD.saveastext.ToString())
                            {
                                bHasRan = await oAddInHelper.UpdateAddInStateAsync(this,
                                    docToCalcURI, calcDocURI, lstUpdates);
                            }
                            else if (docToCalcURI.URIDataManager.TempDocSaveMethod
                                == DataHelpers.AddInHelper.SAVECALCS_METHOD.saveastext.ToString())
                            {
                                oAddInHelper.CopyObservationsTextFile(docToCalcURI, calcDocURI);
                            }
                        }
                        else
                        {
                            //save the update list
                            EditHelpers.XPathIO.SaveNameValueList(docToCalcURI, lstUpdates,
                                Helpers.AddInStateHelper.GetUpdatesFilePath(calcDocURI.URIDataManager.TempDocPath),
                                DataAppHelpers.General.UPDATE_NODE_LIST_NAME);
                        }
                    }
                }
                else
                {
                    docToCalcURI.ErrorMessage += DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "CONTENTSERVICE_NOCALCSRUN2");
                }
            }
            return bHasRan;
        }
       
        public async Task<List<ContentURI>> GetResourceAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            DataAppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            List<ContentURI> colResourceURIs = await _repository.GetResourceAsync(uri, 
                needsOneRecord, needsFullPath, getResourceByType, getResourceByParam);
            return colResourceURIs;
        }
        public async Task<string> GetResourceURLsAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            DataAppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            //return a delimited string containing enough information to display, or load, the resource
            string sResourceURLs = await _repository.GetResourceURLsAsync(uri, needsOneRecord, needsFullPath,
                getResourceByType, getResourceByParam);
            return sResourceURLs;
        }
       
        /// <summary>
        /// Inserts a db audit trail after an edit 
        /// </summary>
        /// <param name="uri"></param>
        public async Task<int> InsertAuditItemAsync(ContentURI uri)
        {
            int iAuditItemId = 0;
            //tempdoc changes don't need auditing
            if (uri.URIClub.PKId != 0)
            {
                bool bIsEditSubServerAction
                    = DataHelpers.GeneralHelpers.IsEditSubServerAction(uri);
                if (bIsEditSubServerAction)
                {
                    if (uri.URIDataManager.ServerSubActionType
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin
                       && (uri.URIDataManager.TempDocSaveMethod
                        == DataHelpers.AddInHelper.SAVECALCS_METHOD.none.ToString()
                        || string.IsNullOrEmpty(uri.URIDataManager.TempDocSaveMethod)))
                    {
                        //anonymous member viewing addin results
                        return 0;
                    }
                    string sServerSubAction = uri.URIDataManager.ServerSubActionType.ToString();
                    int iMemberId = uri.URIMember.MemberId;
                    int iClubInUseId = uri.URIClub.PKId;
                    string sEditedDocURIPattern = (!string.IsNullOrEmpty(uri.URIDataManager.ContentURIPattern))
                        ? uri.URIDataManager.ContentURIPattern : uri.URIPattern;
                    string sEditedDocFullPath = uri.URIFull;
                    string sClubInUseAuthorizationLevel = uri.URIClub.PrivateAuthorizationLevel.ToString();
                    if (uri.URIMember.ClubInUse != null)
                    {
                        iClubInUseId = uri.URIMember.ClubInUse.PKId;
                        sClubInUseAuthorizationLevel = uri.URIMember.ClubInUse.PrivateAuthorizationLevel.ToString();
                    }
                    string sMemberRole = uri.URIMember.MemberRole.ToString();
                    DateTime dtToday = DataHelpers.GeneralHelpers.GetDateShortNow();
                    int iOwnerClubId = uri.URIClub.PKId;
                    iAuditItemId = await _repository.InsertNewAuditItemAsync(
                        uri, sServerSubAction, iMemberId,
                       iClubInUseId, sEditedDocURIPattern, sEditedDocFullPath,
                       sClubInUseAuthorizationLevel, sMemberRole, dtToday, iOwnerClubId);
                }
            }
            return iAuditItemId;
        }
       
        
        public const string HISTORYTOCPARAM = "historytoc";
        public const string LINKEDVIEWSTOCPARAM = "linkedviewstoc";

        public async Task<bool> SetAncestorsAndAuthorizationsAsync(IMemberService memberService, 
            ContentURI uri, bool isInitView)
        {
            bool bHasSet = false;
            switch (uri.URIDataManager.AppType)
            {
                case DataHelpers.GeneralHelpers.APPLICATION_TYPES.accounts:
                    if (uri.URINodeName ==
                        DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString())
                    {
                        uri.URIDataManager.Ancestors =
                            await _repository.GetAncestorsAsync(uri);
                    }
                    SetAdminAncestors(uri);
                    bHasSet = await SetAdminClubOwnerAsync(memberService, uri);
                    //sets uri.urimember.clubinuse and clubinuse.privateauthorizationlevel
                    await SetAdminClubInUseAuthorizationLevelAsync(memberService, uri);
                    await SetURIOwnerServiceListsAsync(memberService, uri);
                    SetAdminFilePaths(uri);
                    uri.URIDataManager.ClubGroups = await GetClubGroupsAsync(uri);
                    uri.URIDataManager.GeoRegions = await GetGeoRegionsAsync(uri);
                    bHasSet = true;
                    break;
                case DataHelpers.GeneralHelpers.APPLICATION_TYPES.members:
                    if (uri.URINodeName ==
                        DataAppHelpers.Members.MEMBER_TYPES.member.ToString()
                        || uri.URINodeName ==
                        DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString())
                    {
                        uri.URIDataManager.Ancestors =
                             await _repository.GetAncestorsAsync(uri);
                    }
                    SetAdminAncestors(uri);
                    bHasSet = await SetAdminClubOwnerAsync(memberService, uri);
                    //sets uri.urimember.clubinuse and clubinuse.privateauthorizationlevel
                    await SetAdminClubInUseAuthorizationLevelAsync(memberService, uri);
                    bHasSet = await SetPaymentHistoryAsync(memberService, uri);
                    await SetURIMemberClubsAsync(memberService, uri);
                    SetAdminFilePaths(uri);
                    uri.URIDataManager.MemberGroups = await GetMemberGroupsAsync(uri);
                    uri.URIDataManager.GeoRegions = await GetGeoRegionsAsync(uri);
                    bHasSet = true;
                    break;
                case DataHelpers.GeneralHelpers.APPLICATION_TYPES.networks:
                    if (uri.URINodeName ==
                        DataAppHelpers.Networks.NETWORK_TYPES.network.ToString()
                        || uri.URINodeName ==
                        DataAppHelpers.Networks.NETWORK_BASE_TYPES.networkbase.ToString())
                    {
                        uri.URIDataManager.Ancestors =
                             await _repository.GetAncestorsAsync(uri);
                    }
                    SetAdminAncestors(uri);
                    bHasSet = await SetAdminClubOwnerAsync(memberService, uri);
                    //sets uri.urimember.clubinuse and clubinuse.privateauthorizationlevel
                    await SetAdminClubInUseAuthorizationLevelAsync(memberService, uri);
                    SetAdminFilePaths(uri);
                    bHasSet = true;
                    break;
                case DataHelpers.GeneralHelpers.APPLICATION_TYPES.locals:
                    if (uri.URINodeName ==
                        DataAppHelpers.Locals.LOCAL_TYPES.local.ToString())
                    {
                        uri.URIDataManager.Ancestors =
                              await _repository.GetAncestorsAsync(uri);
                    }
                    SetAdminAncestors(uri);
                    await SetAdminClubOwnerAsync(memberService, uri);
                    //sets uri.urimember.clubinuse and clubinuse.privateauthorizationlevel
                    await SetAdminClubInUseAuthorizationLevelAsync(memberService, uri);
                    SetAdminFilePaths(uri);
                    //locals use an addin calculator to set locals
                    SetAdminLinkedViewFilePaths(uri);
                    bHasSet = true;
                    break;
                case DataHelpers.GeneralHelpers.APPLICATION_TYPES.addins:
                    if (uri.URINodeName ==
                        DataAppHelpers.AddIns.ADDIN_TYPES.addin.ToString())
                    {
                        uri.URIDataManager.Ancestors =
                              await _repository.GetAncestorsAsync(uri);
                    }
                    SetAdminAncestors(uri);
                    bHasSet = await SetAdminClubOwnerAsync(memberService, uri);
                    //sets uri.urimember.clubinuse and clubinuse.privateauthorizationlevel
                    await SetAdminClubInUseAuthorizationLevelAsync(memberService, uri);
                    SetAdminFilePaths(uri);
                    bHasSet = true;
                    break;
                case DataHelpers.GeneralHelpers.APPLICATION_TYPES.agreements:
                    //also sets uri.uriclub, uri.service, uri.uriclub.services
                    bHasSet = await SetAgreementAncestorsAndAuthorizationAsync(memberService, uri);
                    //sets uri.urimember.clubinuse and clubinuse.privateauthorizationlevel
                    await SetAdminClubInUseAuthorizationLevelAsync(memberService, uri);
                    await SetURIOwnerServiceListsAsync(memberService, uri);
                    await SetServiceClubsAsync(memberService, uri);
                    bHasSet = true;
                    break;
                default:
                    //service apps: also sets club owner, service object, 
                    //and uri.urimember.clubinuse.privateauthorizationlevel
                    bHasSet = await SetServiceAncestorsAsync(memberService, uri, isInitView);
                    break;
            }
            return bHasSet;
        }
        public static void SetAdminAncestors(ContentURI uri)
        {
            ContentURI oldest_ancestor = new ContentURI();
            if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.networks)
            {
                if (uri.URINodeName == DataAppHelpers.Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString()
                    || uri.URINodeName == DataAppHelpers.Networks.NETWORK_BASE_TYPES.networkbase.ToString())
                {
                    oldest_ancestor = new ContentURI("Network Groups", 0,
                        uri.URINetwork.NetworkURIPartName,
                        DataAppHelpers.Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString(),
                        string.Empty);
                }
                else
                {
                    oldest_ancestor = new ContentURI("Clubs", 0,
                        uri.URINetwork.NetworkURIPartName,
                        DataAppHelpers.Networks.NETWORK_TYPES.networkaccountgroup.ToString(),
                        string.Empty);
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.locals)
            {
                oldest_ancestor = new ContentURI("Clubs", 0,
                    uri.URINetwork.NetworkURIPartName,
                    DataAppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString(),
                    string.Empty);
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.addins)
            {
                oldest_ancestor = new ContentURI("Clubs", 0,
                    uri.URINetwork.NetworkURIPartName,
                    DataAppHelpers.AddIns.ADDIN_TYPES.addinaccountgroup.ToString(),
                    string.Empty);
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                if (uri.URINodeName == DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbasegroup.ToString()
                    || uri.URINodeName == DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString())
                {
                    oldest_ancestor = new ContentURI("Member Groups", 0,
                        uri.URINetwork.NetworkURIPartName,
                        DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbasegroup.ToString(),
                        string.Empty);
                }
                else
                {
                    oldest_ancestor = new ContentURI("Clubs", 0,
                        uri.URINetwork.NetworkURIPartName,
                        DataAppHelpers.Members.MEMBER_TYPES.memberaccountgroup.ToString(),
                        string.Empty);
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.accounts)
            {
                oldest_ancestor = new ContentURI("Club Groups", 0,
                    uri.URINetwork.NetworkURIPartName,
                    DataAppHelpers.Accounts.ACCOUNT_TYPES.accountgroup.ToString(),
                    string.Empty);
            }
            DataHelpers.GeneralHelpers.SetApps(oldest_ancestor);
            bool bNeedsAncestors = false;
            DataHelpers.ContentHelper.UpdateNewURIArgs(uri, oldest_ancestor, bNeedsAncestors);
            if (uri.URIDataManager.Ancestors == null)
                uri.URIDataManager.Ancestors = new List<ContentURI>();
            uri.URIDataManager.Ancestors.Insert(0, oldest_ancestor);
        }
        public static void SetAdminFilePaths(ContentURI uri)
        {
            //when the base groups get too large, prevent them from generating an xml file
            bool bNeedsStatefulDocs = AdminAppNeedsStateFulDocs(uri);
            if (bNeedsStatefulDocs)
            {
                string sPathDelimiter = DataHelpers.
                    FileStorageIO.GetDelimiterForFileStorage(uri);
                //clubs, members, locals, networks, agreements
                StringBuilder oFilePath = new StringBuilder();
                oFilePath.Append(DataHelpers.AppSettings.
                    GetResourceRootPath(uri, uri.URINetwork.WebFileSystemPath));
                //Rule 1: owners are clubs, accountid
                oFilePath.Append(DataAppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString());
                oFilePath.Append(DataHelpers.GeneralHelpers.FILENAME_DELIMITER);
                //use the uri.uriclub's id (base admin nodes will be zero)
                oFilePath.Append(uri.URIClub.PKId.ToString());
                oFilePath.Append(sPathDelimiter);
                oFilePath.Append(uri.URIDataManager.AppType.ToString());
                oFilePath.Append(sPathDelimiter);
                oFilePath.Append(uri.URINodeName);
                oFilePath.Append(sPathDelimiter);
                oFilePath.Append(
                    DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(uri));
                uri.URIClub.ClubDocFullPath = oFilePath.ToString();
                //Rule 2: guests = members
                //160 deprecated separate file storage for guests
                uri.URIMember.MemberDocFullPath = uri.URIClub.ClubDocFullPath;
            }
        }
        public static bool AdminAppNeedsStateFulDocs(ContentURI uri)
        {
            bool bNeedsState = true;
            //as long as these are 100% paginated; its ok to generate a stateful doc
            //never show if they are not paginated
            //if (uri.URINodeName == DataAppHelpers.Accounts.ACCOUNT_TYPES.accountgroup.ToString()
            //    || uri.URINodeName == DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbasegroup.ToString()
            //    || uri.URINodeName == DataAppHelpers.Networks.NETWORK_BASE_TYPES.networkbasegroup.ToString()
            //    || uri.URINodeName == DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebasegroup.ToString()
            //    || uri.URINodeName == DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccountgroup.ToString()
            //    || uri.URINodeName == DataAppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString()
            //    || uri.URINodeName == DataAppHelpers.AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
            //{
            //    //don't allow stateful docs
            //    bNeedsState = false;
            //}
            return bNeedsState;
        }
        public static void SetAdminLinkedViewFilePaths(ContentURI uri)
        {
            DataHelpers.ContentHelper.SetSelectedLinkedViewPaths(uri);
        }
        public async Task<bool> SetLinkedViewIsSelectedMemberAsync(ContentURI uri)
        {
            bool bHasSet = false;
            if (uri.URIDataManager.FormInput != null)
            {
                //no need to return the selected linkedview, 
                //just set the isselected property
                await Helpers.AddInStateHelper.GetCalcDocAsync(this, uri);
                bHasSet = true;
            }
            return bHasSet;
        }
        public async Task<bool> SetAdminClubOwnerAsync(IMemberService memberService, ContentURI uri)
        {
            bool bHasSet = false;
            int iAccountId = 0;
            ContentURI owning_ancestor = new ContentURI();
            if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.networks)
            {
                if (uri.URINodeName == DataAppHelpers.Networks.NETWORK_TYPES.network.ToString())
                {
                    if (uri.URIDataManager.Ancestors.Count > 0)
                    {
                        owning_ancestor = uri.URIDataManager.Ancestors.Last();
                        iAccountId = owning_ancestor.URIId;
                    }
                }
                else if (uri.URINodeName == DataAppHelpers.Networks.NETWORK_TYPES.networkaccountgroup.ToString())
                {
                    iAccountId = uri.URIId;
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.locals)
            {
                if (uri.URINodeName == DataAppHelpers.Locals.LOCAL_TYPES.local.ToString())
                {
                    if (uri.URIDataManager.Ancestors.Count > 0)
                    {
                        owning_ancestor = uri.URIDataManager.Ancestors.Last();
                        iAccountId = owning_ancestor.URIId;
                    }
                }
                else if (uri.URINodeName == DataAppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString())
                {
                    iAccountId = uri.URIId;
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.addins)
            {
                if (uri.URINodeName == DataAppHelpers.AddIns.ADDIN_TYPES.addin.ToString())
                {
                    if (uri.URIDataManager.Ancestors.Count > 0)
                    {
                        owning_ancestor = uri.URIDataManager.Ancestors.Last();
                        iAccountId = owning_ancestor.URIId;
                    }
                }
                else if (uri.URINodeName == DataAppHelpers.AddIns.ADDIN_TYPES.addinaccountgroup.ToString())
                {
                    iAccountId = uri.URIId;
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                if (uri.URINodeName == DataAppHelpers.Members.MEMBER_TYPES.member.ToString())
                {
                    if (uri.URIDataManager.Ancestors.Count > 0)
                    {
                        owning_ancestor = uri.URIDataManager.Ancestors.Last();
                        iAccountId = owning_ancestor.URIId;
                    }
                }
                else if (uri.URINodeName == DataAppHelpers.Members.MEMBER_TYPES.memberaccountgroup.ToString())
                {
                    iAccountId = uri.URIId;
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.accounts)
            {
                if (uri.URINodeName == DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString())
                {
                    iAccountId = uri.URIId;
                }
            }
            if (iAccountId != uri.URIMember.ClubInUse.PKId
                && iAccountId != 0)
            {
                uri.URIClub = await memberService.GetClubByIdAsync(uri, iAccountId);
            }
            else
            {
                //this is then used with SetAdminClubInUseAuthoriztion level
                uri.URIClub = new Account(uri.URIMember.ClubInUse);
            }
            bHasSet = true;
            return bHasSet;
        }
        private async Task SetAdminClubInUseAuthorizationLevelAsync(IMemberService memberService, ContentURI uri)
        {
            //authority to edit is always based on uri.urimember.clubinuse.privateauthorizationlevel
            //if this member belongs to uri.uriclub, use the member's role in that club 
            //to set the clubinuse.authorizationlevel
            bool bIsBaseMember = IsBaseMember(uri);
            if (!bIsBaseMember)
            {
                await SetClubMemberAsync(memberService, uri);
            }
            if (uri.URIMember.MemberRole
                == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString())
            {
                if (uri.URIClub.PKId == uri.URIMember.ClubInUse.PKId)
                {
                    uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                        = AccountHelper.AUTHORIZATION_LEVELS.fulledits;
                }
                else
                {
                    if (!bIsBaseMember)
                    {
                        uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                           = AccountHelper.AUTHORIZATION_LEVELS.viewonly;
                    }
                    else
                    {
                        //base members can always edit (but not delete) their own base member info
                        uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                            = AccountHelper.AUTHORIZATION_LEVELS.fulledits;
                    }
                }
            }
            else
            {
                if (bIsBaseMember)
                {
                    //ok to edit their own members data
                    uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                        = AccountHelper.AUTHORIZATION_LEVELS.fulledits;
                }
                else
                {
                    uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                       = AccountHelper.AUTHORIZATION_LEVELS.viewonly;
                }
            }
        }
        private bool IsBaseMember(ContentURI uri)
        {
            bool bIsBaseMember = false;
            if ((uri.URIId == uri.URIMember.MemberId)
                 && (uri.URIMember.ClubInUse.PKId != 0)
                 && (uri.URINodeName
                 == DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString()))
            {
                bIsBaseMember = true;
            }
            return bIsBaseMember;
        }
        private async Task<bool> SetClubMemberAsync(IMemberService memberService, ContentURI uri)
        {
            bool bHasSet = false;
            if (uri.URIClub != null && uri.URIMember != null)
            {
                if (uri.URIMember.ClubInUse != null)
                {
                    if (uri.URIClub.PKId != uri.URIMember.ClubInUse.PKId)
                    {
                        AccountToMember clubMember 
                            = await memberService.GetMemberByClubAsync(uri, 
                            uri.URIClub.PKId, uri.URIMember.MemberId);
                        if (clubMember != null
                            && uri.URIMember.MemberId != 0)
                        {
                            if (clubMember.MemberId != 0)
                            {
                                clubMember.MemberDocFullPath = uri.URIMember.MemberDocFullPath;
                                //160 added this condition because the existing urimember is fine
                                if (uri.URIMember.MemberId != clubMember.MemberId)
                                {
                                    uri.URIMember = new AccountToMember(clubMember);
                                }
                                if (uri.URIClub.PKId != 0)
                                {
                                    uri.URIMember.ClubInUse = new Account(uri.URIClub);
                                }
                            }
                            else
                            {
                                uri.URIMember.MemberRole
                                    = DevTreks.Data.AppHelpers.Members.MEMBER_ROLE_TYPES.none.ToString();
                                //set to anon club
                                uri.URIMember.ClubInUse = new Account();
                            }
                        }
                        else
                        {
                            //not a member of this club means no role
                            uri.URIMember.MemberRole
                                = DevTreks.Data.AppHelpers.Members.MEMBER_ROLE_TYPES.none.ToString();
                            //set to anon club
                            uri.URIMember.ClubInUse = new Account();
                        }
                    }
                }
            }
            bHasSet = true;
            return bHasSet;
        }
        public async Task<bool> SetServiceAncestorsAsync(IMemberService memberService, ContentURI uri, bool isInitView)
        {
            bool bHasSet = false;
            if (uri.URINodeName
                != DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                uri.URIDataManager.Ancestors = await GetAncestorsAsync(uri);
                //set the uri's service
                //refactor: can these be returned with the ancestors to reduce db trips?
                bool bIsBaseService = true;
                bHasSet = await SetServiceAsync(uri, bIsBaseService);
                //change args when they move from contract to apps
                SwitchFromContractToApp(uri);
                //anytime a service is set, must switch uri.urinetworkid and uri.network 
                //to the service's network (or subsequent uri's may be set using bad networkid)
                await SwitchToServiceNetworkAsync(memberService, uri);
            }
            else
            {
                if (uri.URIDataManager.Ancestors == null)
                    uri.URIDataManager.Ancestors = new List<ContentURI>();
                uri.URIDataManager.DocStatus = DataHelpers.GeneralHelpers.DOCS_STATUS.notreviewed;
            }
            //set the uri's owning club
            await SetURIOwnerByServiceAsync(memberService, uri);
            //set the uri.URIClub.ClubDocPath and uri.URIMember.MemberDocFullPath
            DataHelpers.ContentHelper.SetFilePaths(uri);
            //set additional ancestors for this service from the corresponding service agreement
            //also and get the authorization levels from the service
            AccountHelper.AUTHORIZATION_LEVELS ePublicAuthorLevel
                = AccountHelper.AUTHORIZATION_LEVELS.viewonly;
            //privateAuthorLevel = uri.URIMember.ClubInUse.PrivateAuthorizationLevel; is set when db hit
            await SetAgreementAncestorsByServiceAsync(memberService, uri, isInitView);
            //if an anon user, set their privateauthorization = publicauthorization
            SetAnonUserPrivateAuthorization(uri, ePublicAuthorLevel);
            return bHasSet;
        }
        public async Task<bool> SetAgreementAncestorsAndAuthorizationAsync(IMemberService memberService, ContentURI uri)
        {
            bool bHasSet = false;
            //if a service node this also sets the uri.URIService
            uri.URIDataManager.Ancestors =
                await GetAgreementAncestorsAndSetServiceAsync(uri);
            bHasSet = true;
            //anytime a service is set, must switch uri.urinetworkid and uri.network 
            //to the service's network (or subsequent uri's may be set using bad networkid)
            await SwitchToServiceNetworkAsync(memberService, uri);
            //if a service, uses serviceid, if serviceaccount, uses uriid
            await SetAgreementClubOwnerAsync(memberService, uri);
            if (uri.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                SetAdminFilePaths(uri);
            }
            else
            {
                //set owner's agreement authorizations
                SetOwnAgreementClubInUseAuthorizationLevel(uri);
                //refactored: change args when they move from contract to apps
                SwitchFromContractToApp(uri);
                DataHelpers.ContentHelper.SetFilePaths(uri);
            }
            return bHasSet;
        }

        public async Task SwitchToServiceNetworkAsync(IMemberService memberService, 
            ContentURI uri)
        {
            if (uri.URIService != null && uri.URINetwork != null)
            {
                if (uri.URIService.Service.NetworkId != uri.URINetwork.PKId)
                {
                    if (uri.URIService.Service.NetworkId != 0)
                    {
                        uri.URINetwork = await GetNetworkAsync(
                            memberService, uri, uri.URIService.Service.NetworkId);
                        if (uri.URINetwork != null)
                        {
                            string sURIPattern = ContentURI.GetURIPatternFromContentURIPattern(uri.URIDataManager.ContentURIPattern);
                            uri.URINetworkPartName = uri.URINetwork.NetworkURIPartName;
                            string sFileExt = (uri.URIFileExtensionType != null) ? uri.URIFileExtensionType.ToString() : string.Empty;
                            uri.URIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(uri.URIName, uri.URIId.ToString(),
                                uri.URINetworkPartName, uri.URINodeName, sFileExt);
                            uri.URIDataManager.ContentURIPattern = uri.URIDataManager.ContentURIPattern.Replace(sURIPattern,
                                uri.URIPattern);
                        }
                    }
                }
            }
        }
        private async Task SetAgreementClubOwnerAsync(IMemberService memberService, ContentURI uri)
        {
            if (uri.URINodeName
                == DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString())
            {
                uri.URIClub = await memberService.GetClubByIdAsync(uri, uri.URIId);
            }
            else if (uri.URINodeName
                == DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()
                || uri.URINodeName
                == DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                await SetURIOwnerByServiceAsync(memberService, uri);
            }
        }
        //152 deprecated anonymous edits but still allow owners to allows subscribed clubs to edit data
        private async Task SetAgreementAncestorsByServiceAsync(IMemberService memberService, ContentURI uri, bool isInitView)
        {
            IList<ContentURI> colContractAncestors = null;
            //add the service's ancestors, using the owner's agreement
            //find out what the owner authorizes the public to do with the data
            colContractAncestors = await GetAgreementAncestorsByServiceAsync(uri,
                uri.URIClub.PKId);
            if (colContractAncestors != null)
            {
                if (uri.URIMember.MemberId != 0)
                {
                    if (uri.URIMember.ClubInUse.PKId
                        == uri.URIClub.PKId)
                    {
                        //owners always have full authorization
                        //(members don't have individual authorizations)
                        uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                            = AccountHelper.AUTHORIZATION_LEVELS.fulledits;
                    }
                    else
                    {
                        //return a club that has this member with a subscription to this service
                        //and set memberClub.ClubInUse with that club's privateauthorizationlevel
                        AccountToMember memberClub
                            = await memberService.GetAuthorizedClubByServiceAndMemberAsync(
                            uri, uri.URIService.ServiceId, uri.URIMember.MemberId);
                        if (memberClub != null)
                        {
                            if (memberClub.ClubInUse != null)
                            {
                                uri.URIMember.ClubInUse = new Account(memberClub.ClubInUse);
                            }
                        }
                        else
                        {
                            //set it to default (treat their subscribed club as
                            //an anonymous club)
                            uri.URIMember.ClubInUse = new Account();
                        }
                    }
                }
                else
                {
                    //set it to default (treat their subscribed club as
                    //an anonymous club)
                    uri.URIMember.ClubInUse = new Account();
                }
                AddAncestors(colContractAncestors, uri);
            }
        }
        private void SetAnonUserPrivateAuthorization(ContentURI uri,
            AccountHelper.AUTHORIZATION_LEVELS publicAuthorLevel)
        {
            //version 1.5.2 set this to view only always
            //public is MemberId = 0 or ClubInUse.PKId == 0
            if (uri.URIMember.MemberId == 0
                || uri.URIMember.ClubInUse.PKId == 0)
            {
                //public authority to use a service is set by the service's owner 
                //in the owner's service agreement
                uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    = publicAuthorLevel;
            }
        }
        public static void SetOwnAgreementClubInUseAuthorizationLevel(ContentURI uri)
        {
            if (uri.URIClub.PKId == uri.URIMember.ClubInUse.PKId
                && uri.URIMember.MemberRole
                == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString())
            {
                uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    = AccountHelper.AUTHORIZATION_LEVELS.fulledits;
            }
            else
            {
                uri.URIMember.ClubInUse.PrivateAuthorizationLevel
                    = AccountHelper.AUTHORIZATION_LEVELS.viewonly;
            }
        }
        private void AddAncestors(IList<ContentURI> ancestors, ContentURI uri)
        {
            if (uri.URIDataManager.Ancestors == null)
                uri.URIDataManager.Ancestors = new List<ContentURI>();
            foreach (ContentURI ancestor in ancestors)
            {
                //insert the contract nodes before the service nodes
                if (ancestor != null)
                    uri.URIDataManager.Ancestors.Insert(0, ancestor);
            }
        }
        private async Task SetURIOwnerByServiceAsync(IMemberService memberService, ContentURI uri)
        {
            if (uri.URIService.ServiceId != 0)
            {
                //a good serviceid is now on hand, so
                //uri.uriclub and uri.uriservice can now be set 
                if (uri.URIService.OwningClubId != 0)
                {
                    uri.URIClub = await memberService.GetClubByIdAsync(
                        uri, uri.URIService.OwningClubId);
                }
                else
                {
                    await SetURIOwnerClubFromServiceAsync(uri);
                }
            }
        }
        private async Task SetURIOwnerServiceListsAsync(IMemberService memberService, ContentURI uri)
        {
            if (uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
            {
                //see if a service clubs view is needed 
                if (uri.URIDataManager.SubActionView
                    == DataHelpers.GeneralHelpers.SUBACTION_VIEWS.services.ToString())
                {
                    //ui is presenting uri.uriservice.subscribedclubs view
                    //fill in the services for uri.uriclub
                    uri.URIClub.AccountToService
                        = await memberService.GetServiceByClubAsync(
                            uri, uri.URIClub.PKId);
                }
                else if (uri.URIDataManager.SubActionView == DataHelpers.GeneralHelpers.SUBACTION_VIEWS.categories.ToString())
                {
                    //ui is presenting uri.uriservice.categories view
                    //set uri.uriservice.networkcategories and uri.uriservice.servicecategories
                    await SetNetworkCategoriesAsync(uri);
                    //set clubs networks too (160 does not automatically set several members lists)
                    await SetClubNetworkAsync(memberService, uri);
                }
            }
        }
        public async Task SetClubNetworkAsync(IMemberService memberService, ContentURI uri)
        {
            string sClubNetworkRole = DevTreks.Data.AppHelpers.Members.MEMBER_ROLE_TYPES.contributor.ToString();
            if (uri.URIMember.ClubInUse != null)
            {
                bool bNeedsNetwork = false;
                if (uri.URIMember.ClubInUse.AccountToNetwork == null)
                {
                    bNeedsNetwork = true;
                }
                else
                {
                    if (uri.URIMember.ClubInUse.AccountToNetwork.Count == 0)
                    {
                        bNeedsNetwork = true;
                    }
                }
                if (bNeedsNetwork)
                {
                    uri.URIMember.ClubInUse.AccountToNetwork
                        = await memberService.GetNetworkByClubAsync(
                            uri, uri.URIMember.AccountId);
                }
            }
        }
        
        private async Task SetURIMemberClubsAsync(IMemberService memberService, ContentURI uri)
        {
            if (uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
            {
                //see if a members clubs view is needed 
                if (uri.URIDataManager.SubActionView == Account.MemberClubList)
                {
                    //ui is presenting uri.urimember.clubs view
                    //fill in the clubs for uri.urimember
                    uri.URIMember.Member.AccountToMember
                        = await memberService.GetClubsByMemberAsync(
                            uri, uri.URIMember.MemberId);
                }
            }
        }
        private async Task<bool> SetPaymentHistoryAsync(IMemberService memberService, ContentURI uri)
        {
            bool bIsPaymentList = false;
            if (uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
            {
                //see if a paymenthistory view is needed 
                if (uri.URIDataManager.SubActionView == AccountToPayment.PaymentList)
                {
                    bIsPaymentList = true;
                    //note that credit/payments use clubdefault
                    uri.URIMember.ClubDefault.AccountToPayment
                        = await memberService.GetPaymentsByClubAsync(uri.URIMember.ClubDefault.PKId);
                }
            }
            return bIsPaymentList;
        }
        private async Task SetServiceClubsAsync(IMemberService memberService, ContentURI uri)
        {
            if (uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
            {
                //see if a service clubs view is needed 
                if (uri.URIDataManager.SubActionView
                    == DataHelpers.GeneralHelpers.SUBACTION_VIEWS.services.ToString())
                {
                    ///ui is presenting uri.uriservice.subscribedclubs view
                    //fill in the subscribed clubs for uri.uriservice
                    uri.URIService.Service.SubscribedClubs
                        = await memberService.GetSubscribedClubsByServiceAsync(
                            uri, uri.URIService.ServiceId);
                }
            }
        }

        public static void SwitchAncestors(ContentURI uri)
        {
            //ancestors for edit/linkedviews view don't change when ancestor links are clicked
            //(making it  easier to navigate among related nodes)
            string sHistoryTocs = string.Empty;
            if (uri.URIDataManager.UpdatePanelType
                == DataHelpers.GeneralHelpers.UPDATE_PANEL_TYPES.edit)
            {
                sHistoryTocs = DataHelpers.GeneralHelpers.GetFormValue(uri, HISTORYTOCPARAM);
                //sHistoryTocs =
                //    uri.URIDataManager.FormInput[HISTORYTOCPARAM];
            }
            else if (uri.URIDataManager.UpdatePanelType
               == DataHelpers.GeneralHelpers.UPDATE_PANEL_TYPES.linkedviews)
            {
                sHistoryTocs = DataHelpers.GeneralHelpers.GetFormValue(uri, LINKEDVIEWSTOCPARAM);
            }
            if (!string.IsNullOrEmpty(sHistoryTocs))
            {
                string[] arrHistoryToc = sHistoryTocs.Split(DataHelpers.GeneralHelpers.PARAMETER_DELIMITERS);
                int i = 0;
                int iHistoryTocLength = arrHistoryToc.Length;
                string sHistoryURIPattern = string.Empty;
                uri.URIDataManager.Ancestors.Clear();
                for (i = 0; i < iHistoryTocLength; i++)
                {
                    sHistoryURIPattern = arrHistoryToc[i];
                    ContentURI ancestorURI = ContentURI.ConvertShortURIPattern(sHistoryURIPattern, uri.URINetwork);
                    bool bNeedsAncestors = false;
                    DataHelpers.ContentHelper.UpdateNewURIArgs(uri,
                        ancestorURI, bNeedsAncestors);
                    uri.URIDataManager.Ancestors.Add(ancestorURI);
                }
            }
        }
        public static void SwitchFromContractToApp(ContentURI uri)
        {
            if (uri.URINodeName
                == DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
            {
                //service to appgroup docs reference serviceid not accounttoserviceid (as in navigation)
                string sURIPattern = ContentURI.GetURIPatternFromContentURIPattern(uri.URIDataManager.ContentURIPattern);
                uri.URIId = uri.URIService.ServiceId;
                //all apps use servicebase nodename to retrieve file
                uri.URINodeName = DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString();
                uri.URIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(uri.URIName,
                    uri.URIId.ToString(), uri.URINetworkPartName,
                    uri.URINodeName, uri.URIFileExtensionType.ToString());
                uri.URIDataManager.ContentURIPattern = uri.URIDataManager.ContentURIPattern.Replace(sURIPattern,
                    uri.URIPattern);
            }
        }
        public static void GetAddDefaultParams(ContentURI uri,
            out string parentNodeURIPattern, out string defaultNodeURIPattern,
            out string numberToAdd)
        {
            parentNodeURIPattern = string.Empty;
            numberToAdd = string.Empty;
            defaultNodeURIPattern = string.Empty;
            if (uri.URIDataManager.SubActionView
                == DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
            {
                //ef will get the default addin
                parentNodeURIPattern = uri.URIPattern;
                numberToAdd = "0";
                defaultNodeURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(uri.URIName, "0", uri.URINetworkPartName,
                    DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                    string.Empty);
            }
            else
            {
                parentNodeURIPattern
                    = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    DevTreks.Data.EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.parentnode.ToString());
                numberToAdd = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    string.Concat(DevTreks.Data.EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.adddefault_.ToString(),
                    parentNodeURIPattern));
                if (string.IsNullOrEmpty(numberToAdd) || numberToAdd == "0")
                    numberToAdd = "1";
                defaultNodeURIPattern
                    = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    DevTreks.Data.EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.defaultnode.ToString());
            }
        }
        public async Task SetStylesheetStateAsync(ContentURI docToCalcURI,
           DataHelpers.GeneralHelpers.DOC_STATE_NUMBER docState)
        {
            await _repository.SetStylesheetStateAsync(docToCalcURI, docState);
        }
        public async Task SetLinkedViewStateAsync(ContentURI docToCalcURI, ContentURI uri)
        {
            await _repository.SetLinkedViewStateAsync(docToCalcURI, uri);
        }
        public async Task<string> GetLinkedListsArrayAsync(ContentURI docToCalcURI, ContentURI calcDocURI)
        {
            string sLinkedLists = await _repository.GetLinkedListsArrayAsync(docToCalcURI, calcDocURI);
            return sLinkedLists;
        }
        public async Task<List<GeoRegion>> GetGeoRegionsAsync(ContentURI uri)
        {
            List<GeoRegion> geos = await _repository.GetGeoRegionsAsync(uri);
            return geos;
        }
        public async Task<List<AccountClass>> GetClubGroupsAsync(ContentURI uri)
        {
            List<AccountClass> clubgroups = await _repository.GetClubGroupsAsync(uri);
            return clubgroups;
        }

        public async Task<List<MemberClass>> GetMemberGroupsAsync(ContentURI uri)
        {
            List<MemberClass> membergroups = await _repository.GetMemberGroupsAsync(uri);
            return membergroups;
        }
        public void Dispose()
        {
            //the controllers dispose automatically
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
