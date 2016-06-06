using DevTreks.Data;
using DevTreks.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Extensions.Configuration;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers;
using EditHelpers = DevTreks.Data.EditHelpers;
using DevTreks.Exceptions;

namespace DevTreks.Services.Helpers
{
    /// <summary>
    ///Purpose:		manage extension state
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148 
    ///
    ///NOTE:    Basic State Management:
    ///         doctocalcuri is the uri initiating calculations or analyses
    ///         doctotocalcuri.uridatamanager.linkedviews = collection of uris for 
    ///         carrying out, or explaining, calcs
    ///         calcdocuri is the selected linkedview addin uri (ui is a calculator or analyzer)
    ///         selectedlinkedviewuri is the selected linkedview uri (ui is a custom\virtual doc or addin result)
    ///         
    ///         calcdocuri.URIDataManager.TempDocPath = temporary path to store addin instructions
    ///         doctocalcuri.URIDataManager.TempDocPath = temporary path to store addin results
    ///         selectedlinkedviewuri.URIDataManager.TempDocPath = temporary path to store xmldoc attribute or file system addin result
    ///         
    ///         calcdocuri.URIDataManager.Resource[].IsMainStylesheet = stylesheet used to display calcdocuri
    ///         doctocalcuri.URIDataManager.Resource[].IsMainStylesheet = stylesheet used to display doctocalcuri
    ///         selectedlinkedviewuri.URIDataManager.Resource[].IsMainStylesheet = stylesheet used to display selectedlinkedviewuri
    /// </summary>
    public class AddInStateHelper
    {
        public AddInStateHelper(){ }
        //hard-coded element names in onclickpresenter.addSelectedLinkedViewToFormEls()
        public const string NAME_FORADDINVIEW = "lstAddinLinkedView";
        private const string NAME_FORVIEWS = "lstViewLinkedView";
        private const string NAME_FOREDITS = "lstEditLinkedView";

        public static string GetConnection()
        {
            string sConnect = string.Empty;
            var builder = new ConfigurationBuilder();
            return sConnect;

        }
        public static bool NeedsDocToCalcFullState(ContentURI docToCalcURI)
        {
            bool bNeedsDocToCalcFullState = false;
            if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
            {
                //won't load EF because another condition put on EF.Gets
                bNeedsDocToCalcFullState = true;
            }
            else if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.preview)
            {
                //160 uses GetChildren instead of GetDevTreksContent to avoid loading EF
                bNeedsDocToCalcFullState = false;
            }
            else if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                //edit needs EF models except when editing stories
                bNeedsDocToCalcFullState = true;
            }
            return bNeedsDocToCalcFullState;
        }
        public async Task<bool> SetDocToCalcFullStateAsync(IContentService contentService, 
            ContentURI docToCalcURI)
        {
            bool bHasSet = false;
            //file paths are set in Devtreks.Data.Helpers.ContentHelper.SetFilePaths() 
            //these Set... methods put xml files in those paths
            if (docToCalcURI.URIClub.ClubDocFullPath == string.Empty)
            {
                docToCalcURI.ErrorMessage
                    = DevTreksErrors.MakeStandardErrorMsg(
                            docToCalcURI.ErrorMessage, "ADDINSTATE_NOINITDOC");
                return bHasSet;
            }
            
            ContentURI selectedLinkedViewURI = new ContentURI();
            ContentURI calcDocURI = new ContentURI();
            bool bIsAuthorizedToEditClub 
                = (docToCalcURI.URIMember.ClubInUse.PrivateAuthorizationLevel 
                == AccountHelper.AUTHORIZATION_LEVELS.fulledits) 
                ? true : false;
            //160 changed to edit only panel because EF loads slowly first time
            if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit
                && docToCalcURI.URIDataManager.UseSelectedLinkedView == false)
            {
                //uses EF when edit panel is opened
                bHasSet = await contentService.GetDevTrekContentAsync(docToCalcURI);
            }
            if (NeedsFullAddInState(docToCalcURI)
                && docToCalcURI.ErrorMessage == string.Empty)
            {
                //step 2. update uri.linkedviews.isselectedview props to those of uri
                DataHelpers.LinqHelpers.UpdateLinkedViewAddInAndSelectedView(docToCalcURI);

                //step 3. set addin state (ui is a calculator or analyzer)
                selectedLinkedViewURI
                    = GetSelectedLinkedViewURIForAddInState(docToCalcURI, bIsAuthorizedToEditClub);
                bool bHasSelectedView = false;
                if (selectedLinkedViewURI != null)
                {
                    bHasSelectedView = true;
                    //addins are associated with selectedViewURI
                    calcDocURI = await SetSelectedAddInStateAsync(contentService, 
                        selectedLinkedViewURI, bIsAuthorizedToEditClub);
                }
                else
                {
                    //addins are associated with doctocalcuri
                    calcDocURI = await SetSelectedAddInStateAsync(contentService, 
                        docToCalcURI, bIsAuthorizedToEditClub);
                }
                
                //step 4. set third doc state (xmldoc attribute or file system addin result)
                //build new base doc and store in standard URIClub.DocPath (not AddIn-adjusted path)
                if (docToCalcURI.URIDataManager.ServerSubActionType
                    != DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile)
                {
                    if (selectedLinkedViewURI != null)
                    {
                        //addins are associated with selectedViewURI
                        bHasSet = await SetSelectedLinkedViewState(contentService, 
                            bHasSelectedView, selectedLinkedViewURI, calcDocURI,
                            docToCalcURI, bIsAuthorizedToEditClub);
                    }
                    else
                    {
                        //addins are associated with doctocalcuri
                        bHasSet = await SetSelectedLinkedViewState(contentService, 
                            bHasSelectedView, docToCalcURI, calcDocURI,
                            docToCalcURI, bIsAuthorizedToEditClub);
                    }
                }
                //step 5. set temp doc state (tempdocs will always be displayed before other stateful docs
                //because if they exist, it means an addin is being run, or a tempdoc is being loaded)
                //these have to set after addin and selected view states are set
                if (selectedLinkedViewURI == null)
                {
                    await SetTempDocsState(contentService, docToCalcURI);
                }
                else
                {
                    await SetTempDocsState(contentService, selectedLinkedViewURI);
                }
            }
            DataHelpers.AddInStylesheetHelper oAddInStylesheetHelper
                = new DataHelpers.AddInStylesheetHelper();
            if (NeedsFullAddInState(docToCalcURI))
            {
                //set doctocalcuri stylesheet state (needed when building html file system files)
                await contentService.SetStylesheetStateAsync(
                    docToCalcURI, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.firstdoc);
                //set calcDocURI and selectLinkedViewURI stylesheet state
                //note that this will also be called in AddInRunHelper after
                //an addin is run (addins can set stylesheet attributes)
                //default is to show summary view of third docs
                docToCalcURI.URIDataManager.NeedsSummaryView = true;
                if (selectedLinkedViewURI == null)
                {
                    await contentService.SetLinkedViewStateAsync(
                        docToCalcURI, docToCalcURI);
                }
                else
                {
                    //default is to show summary view of third docs
                    selectedLinkedViewURI.URIDataManager.NeedsSummaryView = true;
                    await contentService.SetLinkedViewStateAsync(
                        selectedLinkedViewURI, docToCalcURI);
                }
            }
            else
            {
                if (docToCalcURI.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile)
                {
                    //only need html for first doc with savexml/html is clicked
                    await contentService.SetStylesheetStateAsync(
                        docToCalcURI, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.firstdoc);
                }
            }
            if (selectedLinkedViewURI != null)
                docToCalcURI.ErrorMessage += selectedLinkedViewURI.ErrorMessage;
            if (string.IsNullOrEmpty(docToCalcURI.ErrorMessage))
                bHasSet = true;
            return bHasSet;
        }
        private static bool NeedsFullAddInState(ContentURI docToCalcURI)
        {
            //0.8.8a linked views always need addinstate
            bool bNeedsAddInState = false;
            if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
            {
                bNeedsAddInState = true;
            }
            else if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit
                && docToCalcURI.URIDataManager.UseSelectedLinkedView)
            {
                //edit panel only needs addinstate when devpack custom docs being edited
                //stories being edited in edits panel
                bNeedsAddInState = true;
            }
            return bNeedsAddInState;
        }
        public async Task<bool> SetDocToCalcStateAsync(IContentService contentService,
            ContentURI docToCalcURI, bool isAuthorizedToEditClub)
        {
            bool bHasSet = false;
            //160 changed to edit only panel because EF loads slowly first time
            if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                //uses EF when edit panel is opened
                bHasSet = await contentService.GetDevTrekContentAsync(docToCalcURI);
            }
            else
            {
                //but only generates a new xml doc when linked views start
                bool bNeedsNewDoc = NeedsNewDocToCalc(docToCalcURI, isAuthorizedToEditClub);
                if (bNeedsNewDoc)
                {
                    //refactored: 0.8.7 required manual building of documents
                    //the file.exists has to be modified for 
                    if (!DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                        docToCalcURI.URIClub.ClubDocFullPath))
                    {
                        bHasSet = await contentService.SaveURIFirstDocAsync(docToCalcURI);
                    }
                }
            }
            return bHasSet;
        }
        private bool NeedsNewDocToCalc(ContentURI docToCalcURI,
            bool isAuthorizedToEditClub)
        {
            bool bNeedsNewDoc = false;
            //1. needs a newdb doc when an addin is first loaded into views panel
            //2. that dbdoc will be saved in selectedviewpath when the addin is saved
            //3. tempdocs copy the selectedviewpath or the newdb doc to temdoctocalcpath under same conditions
            if (docToCalcURI.URIFileExtensionType
                != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                bool bAuthorizedClubNeedsDbDoc
                    = NeedsNewDbDoc(docToCalcURI, isAuthorizedToEditClub);
                //note that selectedlinkedviews init here too and delete URIClub.ClubDocFullPath
                if (bAuthorizedClubNeedsDbDoc)
                {
                    //always need a new doc when starting a calculator -otherwise its up 
                    //to the use to save the file after edits
                    if (docToCalcURI.URIDataManager.ServerSubActionType
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                    {
                        bNeedsNewDoc = true;
                    }
                }
            }
            return bNeedsNewDoc;
        }
        
        
        public static bool NeedsNewDbDoc(ContentURI uri, bool isAuthorizedToEditClub)
        {
            //owner always retrieve the the doc when an extension is started
            //guests can retrieve if the owner forgot to do so (didn't run calculations)
            bool bAuthorizedClubNeedsDbDoc = false;
            if (uri.URIDataManager.ServerSubActionType 
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile)
            {
                //serversubaction will handle generating the document
            }
            else
            {
                //the owner never saved the edits, show an error message (to guests and the owner)
                if (isAuthorizedToEditClub == true)
                {
                    if (!DataHelpers.FileStorageIO.URIAbsoluteExists(uri, 
                        uri.URIClub.ClubDocFullPath))
                    {
                        if (!string.IsNullOrEmpty(uri.URIClub.ClubDocFullPath))
                        {
                            bAuthorizedClubNeedsDbDoc = true;
                        }
                    }
                }
                bool bIsOkToRunAddIn = DataHelpers.AddInHelper.IsOkToRunExtension(
                       uri);
                if (isAuthorizedToEditClub == true && bIsOkToRunAddIn == false)
                {
                    //generate the xml docs needed for linked views and packages
                    bAuthorizedClubNeedsDbDoc = true;
                }
            }
            return bAuthorizedClubNeedsDbDoc;
        }

        public async Task<ContentURI> SetSelectedAddInStateAsync(IContentService contentService, 
            ContentURI docToCalcURI, bool isAuthorizedToEditClub)
        {
            ContentURI calcDocURI = new ContentURI();
            if (docToCalcURI != null
                && (docToCalcURI.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin
                || docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews))
            {
                calcDocURI = GetCalcDocURIForAddInState(docToCalcURI);
                if (calcDocURI != null
                    && docToCalcURI.ErrorMessage == string.Empty)
                {
                    AdjustCalcDocURIPaths(docToCalcURI, calcDocURI);
                    bool bFileExists =  DataHelpers.FileStorageIO.URIAbsoluteExists(
                        docToCalcURI, calcDocURI.URIClub.ClubDocFullPath);
                    if (((isAuthorizedToEditClub == true)
                        && (docToCalcURI.URIFileExtensionType
                        != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()))
                        || bFileExists == false)
                    {
                        //get state when calculator is first initializing, but not after
                        bool bIsNotFirstStep = Services.Helpers.AddInRunHelper.IsOkToRunAddIn(
                            docToCalcURI, calcDocURI.URIDataManager.HostName);
                        if ((!bIsNotFirstStep)
                            || bFileExists == false)
                        {
                            XmlReader linkedViewReader = await contentService.GetURISecondDocAsync(docToCalcURI, calcDocURI);
                            if (linkedViewReader == null)
                            {
                                linkedViewReader = await contentService.GetURISecondBaseDocAsync(calcDocURI);
                            }
                            bool bHasLV = false;
                            if (linkedViewReader != null)
                            {
                                using (linkedViewReader)
                                {
                                    //linkedviewreader could be derived from string.empty
                                    XmlNodeType xType = linkedViewReader.MoveToContent();
                                    if (xType == XmlNodeType.Element)
                                    {
                                        await WriteReaderToFileAsync(docToCalcURI, linkedViewReader, calcDocURI.URIClub.ClubDocFullPath);
                                        bHasLV = true;
                                    }
                                }
                                if (!bHasLV)
                                {
                                    XmlReader linkedViewReader2 = await contentService.GetURISecondBaseDocAsync(calcDocURI);
                                    XmlNodeType xType = linkedViewReader2.MoveToContent();
                                    if (xType == XmlNodeType.Element)
                                    {
                                        await WriteReaderToFileAsync(docToCalcURI, linkedViewReader2, calcDocURI.URIClub.ClubDocFullPath);
                                    }
                                }
                            }
                            else
                            {
                                //tempdocs and others will init with a temp calcor 
                                //when tempdocs are set (EnsureLinkedViewExists)
                            }
                        }
                    }
                }
            }
            return calcDocURI;
        }
        
        public static ContentURI GetCalcDocURIForAddInState(ContentURI docToCalcURI)
        {
            ContentURI calcDocURI = new ContentURI();
            calcDocURI 
                = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI != null)
            {
                if (calcDocURI.URIClub.ClubDocFullPath == string.Empty)
                {
                    docToCalcURI.ErrorMessage
                        = DevTreksErrors.MakeStandardErrorMsg(
                            docToCalcURI.ErrorMessage, "ADDINSTATE_NOADDINDOC");
                }
            }
            return calcDocURI;
        }
        private static void AdjustCalcDocURIPaths(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //0.8.7: always adjust caldocpath or it ends up with selectedviewpath too
            if (calcDocURI.URIClub.ClubDocFullPath != string.Empty)
            {
                //save with 'addin' fileextension
                string sAddInURIPattern = GetAddInURIPattern(calcDocURI);
                calcDocURI.URIClub.ClubDocFullPath = calcDocURI.URIClub.ClubDocFullPath.Replace(
                    Path.GetFileNameWithoutExtension(calcDocURI.URIClub.ClubDocFullPath),
                    DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(sAddInURIPattern));
                //160 deprecated separate file storage for guests
                calcDocURI.URIMember.MemberDocFullPath = calcDocURI.URIClub.ClubDocFullPath;
            }
        }
        
        public static string GetAddInURIPattern(ContentURI docToCalcURI)
        {
            //doctocalcuri ensures 1 doc, and xmldoc field, holds all calcdocs 
            string sAddInURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(
                 docToCalcURI.URIName, docToCalcURI.URIId.ToString(),
                 docToCalcURI.URINetworkPartName, docToCalcURI.URINodeName,
                 DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.addin.ToString());
            return sAddInURIPattern;
        }
        public async Task<bool> SetSelectedLinkedViewState(IContentService contentService,
            bool hasSelectedView, ContentURI selectedViewURI, ContentURI calcDocURI, 
            ContentURI docToCalcURI, bool isAuthorizedToEditClub)
        {
            bool bHasSet = false;
            if (docToCalcURI.URIFileExtensionType
                != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //set selected view paths
                bHasSet = await SetSelectedViewPaths(contentService, 
                    selectedViewURI, calcDocURI, isAuthorizedToEditClub);
                //set stand alone addin view
                SetStandAloneViews(hasSelectedView, selectedViewURI, calcDocURI);
                if (selectedViewURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks
                    && selectedViewURI.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                {
                    if (calcDocURI != null)
                    {
                        bool bIsOkToRunAddIn = Services.Helpers.AddInRunHelper.IsOkToRunAddIn(
                            docToCalcURI, calcDocURI.URIDataManager.HostName);
                        DevTreks.Data.Helpers.LinqHelpers
                            .AddAncestorsToLinkedView(docToCalcURI, selectedViewURI);
                        if (bIsOkToRunAddIn)
                        {
                            //add devpackparts to filesystem, if not currently stored 
                            //i.e. before running a calculation or analysis on Treatment 01
                            //make sure each of its devpackpart custom docs are already stored in file system
                            //alternative is to open each individual devpackpart and run individual calcs
                            bHasSet = await SetSelectedViewDevPackPart(contentService, 
                                selectedViewURI);
                        }
                    }
                }
            }
            else
            {
                selectedViewURI.URIClub.ClubDocFullPath = docToCalcURI.URIClub.ClubDocFullPath;
                selectedViewURI.URIMember.MemberDocFullPath = docToCalcURI.URIClub.ClubDocFullPath;
            }
            return bHasSet;
        }
        public void SetStandAloneViews(bool hasSelectedView,
            ContentURI selectedViewURI, ContentURI calcDocURI)
        {
            if (selectedViewURI.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                && calcDocURI != null
                && selectedViewURI.URIDataManager.ServerSubActionType
                == DevTreks.Data.Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
            {
                if (hasSelectedView == true)
                {
                    //anonymous or authorized users can run calculators against sample documents
                    selectedViewURI.URIDataManager.EditViewEditType
                        = DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                    calcDocURI.URIDataManager.EditViewEditType
                        = DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                }
                else
                {
                    //force them to click so that a good selected view can be used to run calcs
                    selectedViewURI.URIDataManager.EditViewEditType
                        = DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                    calcDocURI.URIDataManager.EditViewEditType
                        = DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                }
            }
        }
        public static ContentURI GetSelectedLinkedViewURIForAddInState(
            ContentURI docToCalcURI, bool isAuthorizedToEditClub)
        {
            ContentURI selectedLinkedViewURI = null;
            if (docToCalcURI.URIDataManager.UseSelectedLinkedView)
            {
                //set third doc state (xmldoc attribute or file system addin result)
                //uses custom docs
                selectedLinkedViewURI =
                   DataHelpers.LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
            }
            return selectedLinkedViewURI;
        }
        public async Task<bool> SetSelectedViewPaths(IContentService contentService, 
            ContentURI selectedLinkedViewURI, ContentURI calcDocURI, bool isAuthorizedToEditClub)
        {
            bool bHasSet = false;
            if (selectedLinkedViewURI.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin
                || selectedLinkedViewURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
            {
                bool bNeedsToInitDocToCalc
                    = NeedsToInitDoctoCalcTempDocState(selectedLinkedViewURI);
                if (bNeedsToInitDocToCalc
                    || calcDocURI == null)
                {
                    if (selectedLinkedViewURI.URIDataManager.UseSelectedLinkedView
                        == true)
                    {
                        bHasSet = await SetSelectedViewPath(contentService, 
                            selectedLinkedViewURI, isAuthorizedToEditClub);
                    }
                    else
                    {
                        SetAddInResultPaths(calcDocURI, selectedLinkedViewURI, isAuthorizedToEditClub);
                    }
                }
                else
                {
                    //v170
                    if ((selectedLinkedViewURI.URIDataManager.UseSelectedLinkedView
                        == true && selectedLinkedViewURI.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                        && selectedLinkedViewURI.URIDataManager.ServerSubActionType != DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                    {
                        bHasSet = await SetSelectedViewPath(contentService,
                            selectedLinkedViewURI, isAuthorizedToEditClub);
                    }
                    else
                    {
                        SetAddInResultPaths(calcDocURI, selectedLinkedViewURI, isAuthorizedToEditClub);
                    }
                }
            }
            else
            {
                if (selectedLinkedViewURI.URIDataManager.UseSelectedLinkedView
                    == true)
                {
                    bHasSet = await SetSelectedViewPath(contentService, 
                        selectedLinkedViewURI, isAuthorizedToEditClub);
                }
                //else
                //use existing selectedview paths
            }
            return bHasSet;
        }
        private async Task<bool> SetSelectedViewPath(IContentService contentService,
            ContentURI selectedLinkedViewURI, bool isAuthorizedToEditClub)
        {
            bool bHasSet = false;
            string sStartClubDocFullPath = selectedLinkedViewURI.URIClub.ClubDocFullPath;
            if (!string.IsNullOrEmpty(selectedLinkedViewURI.URIClub.ClubDocFullPath))
            {
                //base linkedview and devpack docs (these may need to be retrieved from db)
                //these stay in original owner paths, so new uri is needed
                string sSelectedViewURIPattern = GetSelectedViewURIPattern(selectedLinkedViewURI);
                selectedLinkedViewURI.URIClub.ClubDocFullPath = selectedLinkedViewURI.URIClub.ClubDocFullPath.Replace(
                    Path.GetFileNameWithoutExtension(selectedLinkedViewURI.URIClub.ClubDocFullPath),
                    DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(sSelectedViewURIPattern));
                DataHelpers.ContentHelper.SetMemberPath(selectedLinkedViewURI);
                if (isAuthorizedToEditClub
                    || (!DataHelpers.FileStorageIO.URIAbsoluteExists(selectedLinkedViewURI,
                        selectedLinkedViewURI.URIClub.ClubDocFullPath)))
                {
                    //retrieve the base doc
                    //somewhat inefficient, but reliable, and convenient for packaging
                    XmlReader linkedViewReader = null;
                    linkedViewReader = await GetSelectedLinkedViewReaderAsync(contentService, 
                        selectedLinkedViewURI);
                    if (linkedViewReader != null)
                    {
                        //linkedviewreader could be derived from string.empty
                        XmlNodeType xType = linkedViewReader.MoveToContent();
                        if (xType == XmlNodeType.Element)
                        {
                            //save the reader for viewing
                            await WriteReaderToFileAsync(selectedLinkedViewURI,
                                linkedViewReader, selectedLinkedViewURI.URIClub.ClubDocFullPath);
                        }
                    }
                    else
                    {
                        //0.8.7 use the starting doc (doctocalc) as a miscdoc and copy to misctemppath durig settempdocs
                        //especially used with devpacks so devpack can be used to find descendent linkedview calculators
                        selectedLinkedViewURI.URIDataManager.MiscDocPath = sStartClubDocFullPath;
                    }
                }
            }
            else
            {
                selectedLinkedViewURI.ErrorMessage
                    = DevTreksErrors.MakeStandardErrorMsg(
                        selectedLinkedViewURI.ErrorMessage, "ADDINSTATE_NOSELECTEDVIEWDOC");
            }
            bHasSet = true;
            return bHasSet;
        }
        private void SetAddInResultPaths(ContentURI calcDocURI, ContentURI selectedLinkedViewURI,
            bool isAuthorizedToEdit)
        {
            //use an addin result path
            if (calcDocURI == null)
            {
                calcDocURI = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(selectedLinkedViewURI);
            }
            if (calcDocURI != null)
            {
                string sSelectedViewFileName
                    = DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(calcDocURI);
                sSelectedViewFileName
                    = DataHelpers.ContentHelper.FixFilePathLength(selectedLinkedViewURI, sSelectedViewFileName);
                string sDbDocToCalcPath = selectedLinkedViewURI.URIClub.ClubDocFullPath;
                //when the addinresults are saved, a new db doc is generated in order to get any new or updated linkedviews
                //rule 1. calcs being run for first time always use doctocalc.URIClub.ClubDocFullPath
                bool bNeedsDocToCalcTempDocState
                    = NeedsToInitDoctoCalcTempDocState(selectedLinkedViewURI);
                //NOTE owner should display a full doc, if it exists, but its the display logic's job to handle
                //here rule 2 always applies
                //rule 2. store/display using a regular addin result file
                string sSummaryDocPath = sDbDocToCalcPath.Replace(
                    Path.GetFileNameWithoutExtension(sDbDocToCalcPath), sSelectedViewFileName);
                selectedLinkedViewURI.URIClub.ClubDocFullPath = sSummaryDocPath;
                DataHelpers.ContentHelper.SetMemberPath(selectedLinkedViewURI);
                //rules 2 or 4. if the addin results doesn't exist, or owner running the first calc, 
                //copy the dbdoctocalc to its path 
                //(note that when saved, the method SaveURIFirstDoc must use the original sDbDocPath)
                if (!DataHelpers.FileStorageIO.URIAbsoluteExists(selectedLinkedViewURI, 
                    selectedLinkedViewURI.URIClub.ClubDocFullPath)
                    || bNeedsDocToCalcTempDocState)
                {
                    if (DataHelpers.FileStorageIO.URIAbsoluteExists(selectedLinkedViewURI, 
                        sDbDocToCalcPath))
                    {
                        //0.9.1: potential to overwrite a calculation, better to store directly in tempdoc
                        //settempdocstate will move to real tempdocpath when set
                        selectedLinkedViewURI.URIDataManager.TempDocPath = sDbDocToCalcPath;
                    }
                }
            }
        }
        
        private async Task<bool> ResetDocToCalcStateAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI selectedLinkedViewURI)
        {
            bool bHasSet = false;
            if (selectedLinkedViewURI.URIDataManager.UseSelectedLinkedView == true)
            {
                //2.0.0 added the linkedviews app to the condition because no reason for 
                //selectedviewuri to have the same uridatamanager.lv -the props won't be the same
                if ((docToCalcURI.URIDataManager.AppType
                   == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks
                   || docToCalcURI.URIDataManager.AppType
                   == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews)
                   && selectedLinkedViewURI.URINodeName
                   == DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    //selectedlinked is a story for doctocalcuri
                }
                else
                {
                    selectedLinkedViewURI.URIDataManager.LinkedView
                        = await contentService.GetLinkedViewAsync(selectedLinkedViewURI);
                    if (docToCalcURI.URIDataManager.UseDefaultAddIn)
                    {
                        //doctocalcuri has default addins that are needed by selectedviewuri
                        DataHelpers.LinqHelpers.AddDefaultAddIns1ToLinkedView1(selectedLinkedViewURI.URIDataManager.LinkedView,
                            docToCalcURI.URIDataManager.LinkedView);
                    }
                }
            }
            bHasSet = true;
            return bHasSet;
        }
        private async Task<XmlReader> GetSelectedLinkedViewReaderAsync(IContentService contentService, 
            ContentURI docToCalcURI)
        {
            XmlReader linkedViewReader = null;
            //init the linked view xmldoc field (customdocs only stored in parts table)
            if (docToCalcURI.URINodeName
                != DevTreks.Data.AppHelpers.DevPacks.DEVPACKS_TYPES.devpackgroup.ToString()
                && docToCalcURI.URINodeName
                != DevTreks.Data.AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                linkedViewReader = await contentService.GetURISecondBaseDocAsync(docToCalcURI);
            }
            else
            {
                //linkedviewgroups and packs don't have xml doc attribute base fields
                //the addins have to be run to retrieve the calcd file system doc
            }
            return linkedViewReader;
        }
        private async Task<bool> SetSelectedViewDevPackPart(IContentService contentService,
            ContentURI selectedViewURI)
        {
            bool bHasSet = false;
            if (selectedViewURI.URIDataManager.LinkedView != null
                && selectedViewURI.URIClub.ClubDocFullPath != string.Empty
                && selectedViewURI.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
            {
                if (selectedViewURI.URIDataManager.LinkedView == null)
                {
                    //set state for selectedViewURI (need current linkedviews out of db)
                    bHasSet = await SetDevPackDocToCalcStateAsync(contentService, selectedViewURI);
                }
                //build path to custom doc
                string sPathDelimiter 
                    = DataHelpers.FileStorageIO.GetDelimiterForFileStorage(selectedViewURI.URIClub.ClubDocFullPath);
                string sClubDirectoryName = DataHelpers.FileStorageIO.GetDirectoryName(
                    selectedViewURI.URIClub.ClubDocFullPath);
                if (!sClubDirectoryName.EndsWith(sPathDelimiter))
                {
                    sClubDirectoryName = string.Concat(sClubDirectoryName, sPathDelimiter);
                }
                string sParentSubFolderName = string.Empty;
                string sSubFolderName = string.Empty;
                string sClubDocPath = string.Empty;
                string sMemberDocPath = string.Empty;
                foreach (var linkedviewparent in selectedViewURI.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        if (linkedview.URINodeName
                            == DevTreks.Data.AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                        {
                            //more convenient than clicking on each one
                            linkedview.URIDataManager.AppType = selectedViewURI.URIDataManager.AppType;
                            linkedview.URIDataManager.SubAppType = selectedViewURI.URIDataManager.SubAppType;
                            XmlReader linkedViewReader = null;
                            //get filepath to devpackpart customdoc
                            sParentSubFolderName = string.Concat(DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString(),
                               DataHelpers.GeneralHelpers.FILENAME_DELIMITER, 
                               linkedviewparent.Key.ToString(), sPathDelimiter);
                            sSubFolderName = string.Concat(linkedview.URINodeName,
                                DataHelpers.GeneralHelpers.FILENAME_DELIMITER, linkedview.URIId.ToString(),
                                sPathDelimiter);
                            //can't be sure how many recursive nodes are in front of sParentSubFolderName
                            //requirement is to start from parent before running calcs on ancestors
                            //of parent (i.e. run SubTreatment01, then Treatment 01, Then Treatments 1 to 10)
                            if (sClubDirectoryName.EndsWith(sParentSubFolderName))
                            {
                                sClubDocPath = string.Concat(sClubDirectoryName, sSubFolderName,
                                    DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(GetSelectedViewURIPattern(linkedview)),
                                    DataHelpers.GeneralHelpers.EXTENSION_XML);
                                //if it doesn't exist, hit db and store it
                                if (!DataHelpers.FileStorageIO.URIAbsoluteExists(
                                    selectedViewURI, sClubDocPath))
                                {
                                    linkedViewReader 
                                        = await GetSelectedLinkedViewReaderAsync(
                                        contentService, linkedview);
                                    if (linkedViewReader != null)
                                    {
                                        //linkedviewreader could be derived from string.empty
                                        XmlNodeType xType = linkedViewReader.MoveToContent();
                                        if (xType == XmlNodeType.Element)
                                        {
                                            using (linkedViewReader)
                                            {
                                                //save the reader for viewing
                                                await WriteReaderToFileAsync(selectedViewURI,
                                                    linkedViewReader, sClubDocPath);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            bHasSet = true;
            return bHasSet;
        }
        private async Task<bool> SetDevPackDocToCalcStateAsync(IContentService contentService, 
            ContentURI selectedViewURI)
        {
            bool bHasSet = false;
            //selecteviewuri.club.docpath points to the addinresults file
            //state is also needed for the original doctocalc 
            //(need current linkedviews)
            string sClubAddInResult = selectedViewURI.URIClub.ClubDocFullPath;
            string sMemberAddInResult = selectedViewURI.URIMember.MemberDocFullPath;
            string sClubDocToCalcFilePath = DevTreks.Data.Helpers.ContentHelper
                .MakeNewFilePathFromURIPattern(selectedViewURI.URIPattern,
                sClubAddInResult);
            string sMemberDocToCalcFilePath = DevTreks.Data.Helpers.ContentHelper
                .MakeNewFilePathFromURIPattern(selectedViewURI.URIPattern,
                sMemberAddInResult);
            selectedViewURI.URIClub.ClubDocFullPath
                = sClubDocToCalcFilePath;
            selectedViewURI.URIMember.MemberDocFullPath
                = sMemberDocToCalcFilePath;
            //need the selectedViewURI's doctocalc (need the current nodes in db)
            bool bIsAuthorized = true;
            bHasSet = await SetDocToCalcStateAsync(contentService, selectedViewURI, bIsAuthorized);
            selectedViewURI.URIClub.ClubDocFullPath
                = sClubAddInResult;
            selectedViewURI.URIMember.MemberDocFullPath
                = sMemberAddInResult;
            return bHasSet;
        }
        private static string GetSelectedViewURIPattern(ContentURI uri)
        {
            string sSelectedViewURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(
               uri.URIName, uri.URIId.ToString(),
               uri.URINetworkPartName, uri.URINodeName,
               DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString());
            return sSelectedViewURIPattern;
        }
        private static bool SetFullDocPathsForDisplay(ContentURI docToCalcURI,
            ContentURI linkedAddInViewURI)
        {
            bool bHasState = false;
            //if the full calculations exist, use them
            string sFullURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(
                linkedAddInViewURI.URIName, linkedAddInViewURI.URIId.ToString(),
                linkedAddInViewURI.URINetworkPartName,
                linkedAddInViewURI.URINodeName,
                DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.full.ToString());
            string sFullDocPath = linkedAddInViewURI.URIClub.ClubDocFullPath.Replace(
                Path.GetFileNameWithoutExtension(linkedAddInViewURI.URIClub.ClubDocFullPath), 
                DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(sFullURIPattern));
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, sFullDocPath))
            {
                docToCalcURI.URIClub.ClubDocFullPath = sFullDocPath;
                //160 deprecated separate file storage for guests
                docToCalcURI.URIMember.MemberDocFullPath = sFullDocPath;
                bHasState = true;
            }
            else
            {
                //process regular docs
                bHasState = false;
            }
            return bHasState;
        }
        private static bool NeedsInitialDbDoc(ContentURI calcDocURI)
        {
            bool bNeedsInitialDbDoc = false;
            if (DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString().Equals(
                calcDocURI.URIDataManager.HostName.ToLower())
                || DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
                calcDocURI.URIDataManager.HostName.ToLower()))
            {
                bNeedsInitialDbDoc = DevTreks.Extensions.DoStepsHost.NeedsInitialDbDoc(
                   calcDocURI);
            }
            return bNeedsInitialDbDoc;
        }
        private static bool NeedsNewDocToCalc(ContentURI calcDocURI,
            ContentURI docToCalcURI)
        {
            bool bNeedsNewDocToCalc = false;
            if (DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString().Equals(
               calcDocURI.URIDataManager.HostName.ToLower())
                || DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
               calcDocURI.URIDataManager.HostName.ToLower()))
            {
                bNeedsNewDocToCalc =
                    DevTreks.Extensions.DoStepsHost.NeedsNewDocToCalc(
                    calcDocURI, docToCalcURI);
            }
            return bNeedsNewDocToCalc;
        }
        private static async Task<XElement> GetNewCalcDocVersionAsync(IContentService contentService, 
            XElement linkedView, ContentURI calcDocURI)
        {
            //calcDocURI.URIDataManager.AttributeName is used to store version number
            string version = string.Empty;
            XElement baseLinkedViewXmlDoc = null;
            //use the extension host to get the existing calculatortype name
            if (DataHelpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString().Equals(
                calcDocURI.URIDataManager.HostName.ToLower())
                || DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
               calcDocURI.URIDataManager.HostName.ToLower()))
            {
                bool bUseBaseId = false;
                string sVersion = DevTreks.Extensions.DoStepsHost.GetCalcDocVersion(calcDocURI,
                    linkedView, bUseBaseId);
                //must check the db base calculator
                XmlReader linkedViewReader = await contentService.GetURISecondBaseDocAsync(calcDocURI);
                if (linkedViewReader != null)
                {
                    using (linkedViewReader)
                    {
                        baseLinkedViewXmlDoc = XElement.Load(linkedViewReader);
                    }
                    if (baseLinkedViewXmlDoc != null)
                    {
                        bUseBaseId = true;
                        version = DevTreks.Extensions.DoStepsHost
                            .GetCalcDocVersion(calcDocURI, baseLinkedViewXmlDoc,
                                bUseBaseId);
                        //store the version in calcdoc temporarily
                        calcDocURI.URIDataManager.AttributeName = version;
                    }
                }
                if (sVersion.Equals(version))
                {
                    baseLinkedViewXmlDoc = null;
                }
            }
            return baseLinkedViewXmlDoc;
        }
        
        private async Task WriteReaderToFileAsync(ContentURI uri, 
            XmlReader readerToSave, string saveDocPath)
        {
            string sErrorMsg = string.Empty;
            DataHelpers.FileStorageIO fileStorageIO = new DataHelpers.FileStorageIO();
            sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(uri,
                readerToSave, saveDocPath);
            uri.ErrorMessage += sErrorMsg;
        }
        public async Task<bool> SetTempDocsState(IContentService contentService, 
            ContentURI docToCalcURI)
        {
            bool bHasSet = false;
            string sSelectedLinkedViewAddInHostName =
                DevTreks.Data.Helpers.LinqHelpers.SelectedLinkedViewAddInHostName(
                docToCalcURI.URIDataManager.LinkedView);
            if (!string.IsNullOrEmpty(sSelectedLinkedViewAddInHostName))
            {
                bool bIsOKToRun = false;
                if (docToCalcURI.URIDataManager.ServerActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
                {
                    bIsOKToRun = true;
                }
                if (!bIsOKToRun)
                {
                    bIsOKToRun = Services.Helpers.AddInRunHelper.IsOkToRunAddIn(
                        docToCalcURI, sSelectedLinkedViewAddInHostName);
                    if (!bIsOKToRun)
                    {
                        bIsOKToRun = NeedsAddInState(docToCalcURI);
                    }
                }
                if (bIsOKToRun)
                {
                    ContentURI calcDocURI = new ContentURI();
                    //addins save state between steps using tempdocs
                    //the file for the calculation\analysis options chosen in the addin
                    string sTempCalcDocPath = string.Empty;
                    //the file holding the calculation results
                    string sTempDocToCalcPath = string.Empty;
                    //2. holds randomid used to build two tempdocs
                    List<string> init = new List<string>();
                    string sTempDocURIPattern = string.Empty;
                    string sSaveMethod = string.Empty;
                    calcDocURI = await SetStateManagementURIsAsync(contentService,
                        docToCalcURI, init);
                    GetInitParams(docToCalcURI, out sTempDocURIPattern, out sSaveMethod);
                    if (string.IsNullOrEmpty(sTempDocURIPattern))
                    {
                        List<string> tempDocs = await SetTempDocsPathsandFileNamesAsync(
                            contentService, docToCalcURI, calcDocURI);
                        sTempDocURIPattern = tempDocs.ElementAtOrDefault(0);
                        sTempCalcDocPath = tempDocs.ElementAtOrDefault(1);
                        sTempDocToCalcPath = tempDocs.ElementAtOrDefault(2);
                    }
                    else
                    {
                        //init the temppaths
                        SetTempDocsPathsandFileNames(docToCalcURI,
                            calcDocURI, sTempDocURIPattern, 
                            out sTempCalcDocPath, out sTempDocToCalcPath);
                        if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                            docToCalcURI, sTempCalcDocPath) == false
                            || DataHelpers.FileStorageIO.URIAbsoluteExists(
                                docToCalcURI, sTempDocToCalcPath) == false)
                        {
                            //init the state of the temppaths
                            List<string> tempDocs = await SetTempDocsPathsandFileNamesAsync(
                                contentService, docToCalcURI, calcDocURI);
                            sTempDocURIPattern = tempDocs.ElementAtOrDefault(0);
                            sTempCalcDocPath = tempDocs.ElementAtOrDefault(1);
                            sTempDocToCalcPath = tempDocs.ElementAtOrDefault(2);
                        }
                    }
                    if (sTempCalcDocPath == string.Empty)
                    {
                        //don't show the calcdoc
                        calcDocURI.URIDataManager.TempDocURIPattern = string.Empty;
                        calcDocURI.URIDataManager.TempDocPath = string.Empty;
                        docToCalcURI.URIDataManager.TempDocURIPattern = string.Empty;
                        docToCalcURI.URIDataManager.TempDocPath = string.Empty;
                    }
                    else
                    {
                        //set stateful tempdoc params
                        docToCalcURI.URIDataManager.TempDocURIPattern = sTempDocURIPattern;
                        docToCalcURI.URIDataManager.TempDocPath = sTempDocToCalcPath;
                        docToCalcURI.URIDataManager.TempDocSaveMethod = sSaveMethod;
                        calcDocURI.URIDataManager.TempDocURIPattern = sTempDocURIPattern;
                        calcDocURI.URIDataManager.TempDocPath = sTempCalcDocPath;
                    }
                }
                bHasSet = true;
            }
            return bHasSet;
        }
        private static bool NeedsAddInState(ContentURI docToCalcURI)
        {
            bool bNeedsAddInState = false;
            //if nothing has been saved, init a new addin
            ContentURI calcDocURI =
                DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI != null)
            {
                string sAddInURIPattern = GetAddInURIPattern(calcDocURI);
                string sCalcDocClubPath = calcDocURI.URIClub.ClubDocFullPath.Replace(
                    Path.GetFileNameWithoutExtension(calcDocURI.URIClub.ClubDocFullPath),
                    DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(sAddInURIPattern));
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, sCalcDocClubPath)
                    == false)
                {
                    bNeedsAddInState = true;
                }
                else
                {
                    //make sure sCalcDocClubPath holds this particular addin
                    XElement oCalcNodes = DevTreks.Data.Helpers.FileStorageIO.LoadXmlElement(
                        docToCalcURI, sCalcDocClubPath);
                    XElement linkedViewNode = GetLinkedView(calcDocURI, oCalcNodes);
                    if (linkedViewNode == null)
                    {
                        bNeedsAddInState = true;
                    }
                }
            }
            return bNeedsAddInState;
        }
        private static async Task<ContentURI> SetStateManagementURIsAsync(IContentService contentService, 
            ContentURI docToCalcURI, List<string> init)
        {
            //retrieve the selected linkedview
            ContentURI calcDocURI =
                DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI == null)
            {
                //doctocalc.linkedviews holds calculator/analysis params
                calcDocURI = await GetCalcDocAsync(contentService, docToCalcURI);
            }
            return calcDocURI;
        }
        private void GetInitParams(ContentURI docToCalcURI, out string tempDocURIPattern, 
            out string saveMethod)
        {
            //holds random id used to build two tempdoc paths for next two params
            tempDocURIPattern = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI,
                DataAppHelpers.LinkedViews.TEMPDOCURI);
            //save method (node-only or db atts too)
            saveMethod
                = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, DataAppHelpers.General.SAVE_METHOD);
            if (string.IsNullOrEmpty(saveMethod)) saveMethod = DevTreks.Data.Helpers.AddInHelper.SAVECALCS_METHOD.none.ToString();
        }
        private async Task<string> SetCalcDocTempStateAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, string tempDocToCalcPath, string tempCalcDocPath)
        {
            string sTempCalcDocPath = tempCalcDocPath;
            bool bHasLinkedView = false;
            if (calcDocURI != null
                && DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI, calcDocURI.URIClub.ClubDocFullPath))
            {
                XElement oCalcNodes = null;
                XmlReader oLinkedViewReader
                    = await DataHelpers.FileStorageIO.GetXmlReaderAsync(docToCalcURI, calcDocURI.URIClub.ClubDocFullPath);
                if (oLinkedViewReader != null)
                {
                    using (oLinkedViewReader)
                    {
                        oCalcNodes = XElement.Load(oLinkedViewReader);
                    }
                }
                //calcDocURIPattern points to the linked view node to use in oCalcNodes
                //make sure the linkedview node being used exists
                bHasLinkedView = await EnsureLinkedViewExistsAsync(contentService, docToCalcURI,
                    calcDocURI, tempDocToCalcPath, oCalcNodes);
                //use the whole document so that the update is straightforward
                if (bHasLinkedView)
                {
                    if (oCalcNodes.HasElements)
                    {
                        string sErrorMsg = string.Empty;
                        DataHelpers.FileStorageIO fileStorageIO = new DataHelpers.FileStorageIO();
                        sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(
                            docToCalcURI, oCalcNodes.CreateReader(),
                            sTempCalcDocPath);
                        docToCalcURI.ErrorMessage += sErrorMsg;
                    }
                }
                else
                {
                    oCalcNodes = new XElement(DataHelpers.GeneralHelpers.ROOT_PATH);
                    bHasLinkedView = await EnsureLinkedViewExistsAsync(contentService, 
                        docToCalcURI, calcDocURI, tempDocToCalcPath, oCalcNodes);
                    //use the whole document so that the update is straightforward
                    if (bHasLinkedView)
                    {
                        if (oCalcNodes.HasElements)
                        {
                            string sErrorMsg = string.Empty;
                            DataHelpers.FileStorageIO fileStorageIO = new DataHelpers.FileStorageIO();
                            sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(docToCalcURI,
                                oCalcNodes.CreateReader(), sTempCalcDocPath);
                            docToCalcURI.ErrorMessage += sErrorMsg;
                        }
                    }
                }
            }
            else
            {
                XElement oCalcNodes = new XElement(DataHelpers.GeneralHelpers.ROOT_PATH);
                bHasLinkedView = await EnsureLinkedViewExistsAsync(contentService, 
                    docToCalcURI, calcDocURI, tempDocToCalcPath, oCalcNodes);
                //use the whole document so that the update is straightforward
                if (bHasLinkedView)
                {
                    {
                        string sErrorMsg = string.Empty;
                        DataHelpers.FileStorageIO fileStorageIO = new DataHelpers.FileStorageIO();
                        sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(docToCalcURI,
                            oCalcNodes.CreateReader(), sTempCalcDocPath);
                        docToCalcURI.ErrorMessage += sErrorMsg;
                    }
                }
            }
            if (bHasLinkedView == false)
            {
                docToCalcURI.ErrorMessage = DevTreksErrors.MakeStandardErrorMsg(
                    docToCalcURI.ErrorMessage, "ADDINSTATE_NOADDINDOC");
                sTempCalcDocPath = string.Empty;
                if (calcDocURI != null)
                {
                    calcDocURI.URIDataManager.TempDocPath = string.Empty;
                }
            }
            else
            {
                //if it exists, copy the corresponding html fragment file
                DevTreks.Helpers.DisplayURIHelper.CopyHtmlDocsToTempDocPaths(
                    calcDocURI, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc,
                    sTempCalcDocPath);
            }
            return sTempCalcDocPath;
        }
        
        private async Task<bool> EnsureLinkedViewExistsAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI, string tempDocToCalcPath,
            XElement linkedViews)
        {
            bool bLinkedLinkedViewExists = true;
            //stand-alone calculators don't need to be checked
            if (docToCalcURI.URIDataManager.AppType
                != DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews)
            {
                XElement linkedView = GetLinkedView(calcDocURI, linkedViews);
                if (linkedView != null)
                {
                    //check whether the host has updated the calculator/analyzer
                    //they do so by changing the Version (1.0 to 1.2)
                    string sVersion = string.Empty;
                    XElement baseLinkedViewXmlDoc = null;
                    baseLinkedViewXmlDoc = await GetNewCalcDocVersionAsync(contentService, 
                        linkedView, calcDocURI);
                    sVersion = calcDocURI.URIDataManager.AttributeName;
                    if (baseLinkedViewXmlDoc != null)
                    {
                        bool bHasNewVersion = AddUpdatedAttributes(calcDocURI,
                            linkedViews, baseLinkedViewXmlDoc, sVersion);
                        if (bHasNewVersion)
                        {
                            //the calling procedure saves in calcdoc.tempdocpath
                        }
                    }
                }
                else
                {
                    //check to make sure this is actually an addin
                    bool bHasSet = await contentService.SetAddInNamesAsync(calcDocURI);
                    if (calcDocURI.URIDataManager == null)
                        calcDocURI.URIDataManager = new ContentURI.DataManager();
                    if (DataHelpers.AddInHelper.IsAddIn(calcDocURI.URIDataManager.AddInName) == true
                        && DataHelpers.AddInHelper.IsAddIn(calcDocURI.URIDataManager.HostName) == true)
                    {
                        XmlReader oBaseCalcNodes = await contentService.GetURISecondBaseDocAsync(
                            calcDocURI);
                        if (oBaseCalcNodes != null)
                        {
                            using (oBaseCalcNodes)
                            {
                                oBaseCalcNodes.MoveToContent();
                                if (oBaseCalcNodes.NodeType == XmlNodeType.Element)
                                {
                                    //and add it to oCalcNodes
                                    XElement baseLinkedViewRoot = XElement.Load(oBaseCalcNodes);
                                    if (baseLinkedViewRoot.HasElements)
                                    {
                                        bLinkedLinkedViewExists
                                            = EditHelpers.XmlLinq.AddBaseLinkedViewToExistingXmlDoc(
                                            calcDocURI, baseLinkedViewRoot, linkedViews);
                                        docToCalcURI.ErrorMessage = calcDocURI.ErrorMessage;
                                    }
                                }
                                else
                                {
                                    bLinkedLinkedViewExists = false;
                                    docToCalcURI.ErrorMessage
                                        = DevTreksErrors.MakeStandardErrorMsg(
                                            docToCalcURI.ErrorMessage, "ADDINSTATE_NOADDINDOC2");
                                }
                            }
                        }
                        else
                        {
                            bLinkedLinkedViewExists = false;
                            docToCalcURI.ErrorMessage
                                = DevTreksErrors.MakeStandardErrorMsg(
                                    docToCalcURI.ErrorMessage, "ADDINSTATE_NOADDINDOC2");
                        }
                    }
                    else
                    {
                        bLinkedLinkedViewExists = false;
                    }
                }
                if (!string.IsNullOrEmpty(docToCalcURI.ErrorMessage))
                {
                    bLinkedLinkedViewExists = false;
                }
            }
            return bLinkedLinkedViewExists;
        }
        private bool AddUpdatedAttributes(ContentURI calcDocURI,
            XElement linkedViews, XElement baseLinkedViewXmlDoc,
            string newVersion)
        {
            bool bHasNewVersion = false;
            //don't overwrite existing calculation attributes
            //just append missing, new attributes or new attribute values
            //(the calculators get rid of outdated attributes 
            //when they are serialized after calcs run)
            if (baseLinkedViewXmlDoc != null)
            {
                if (baseLinkedViewXmlDoc.HasElements)
                {
                    if (baseLinkedViewXmlDoc
                        .Elements(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()).Any())
                    {
                        if (!string.IsNullOrEmpty(newVersion))
                        {
                            //update to the new attributes
                            string sDefaultBaseId = "1";
                            EditHelpers.XmlLinq.AddAttributesThatAreMissing(
                                DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                                sDefaultBaseId, baseLinkedViewXmlDoc,
                                calcDocURI.URIId.ToString(), linkedViews);
                            if (!string.IsNullOrEmpty(newVersion))
                            {
                                bHasNewVersion = EditHelpers.XmlLinq
                                    .SetAttributeValue(linkedViews,
                                    DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                                    calcDocURI.URIId.ToString(), DevTreks.Data.AppHelpers.Calculator.cVersion,
                                    newVersion);
                            }
                        }

                    }
                }
            }
            return bHasNewVersion;
        }
        private static XElement GetLinkedView(ContentURI calcDocURI,
            XElement linkedViews)
        {
            XElement linkedView = EditHelpers.XmlLinq.GetElement(
                linkedViews, DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                calcDocURI.URIId.ToString());
            return linkedView;
        }
        private bool AddLinkedViewFromDocToCalc(ContentURI docToCalcURI,
            ContentURI calcDocURI, string tempDocToCalcPath,
            ref XElement calcDoc)
        {
            bool bLinkedLinkedViewExists = false;
            string sNodeName = docToCalcURI.URINodeName;
            string sId = docToCalcURI.URIId.ToString();
            if (!string.IsNullOrEmpty(
                docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern))
            {
                sNodeName = ContentURI.GetURIPatternPart(
                    docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern,
                    ContentURI.URIPATTERNPART.node);
                sId = ContentURI.GetURIPatternPart(
                    docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern,
                    ContentURI.URIPATTERNPART.id);
            }
            string sDocToCalcXmlDocRootQry
                = DevTreks.Data.EditHelpers.XmlIO.MakeXmlDocRootQry(
                sNodeName, DataHelpers.GeneralHelpers.AT_ID, sId);
            XPathNavigator navLinkedViewNodes = EditHelpers.XPathIO.GetElement(
                docToCalcURI, string.Empty, string.Empty, tempDocToCalcPath,
                sDocToCalcXmlDocRootQry);
            if (navLinkedViewNodes != null)
            {
                string sRootQry = EditHelpers.XmlIO.MakeXPathAbbreviatedQry(
                    DataHelpers.GeneralHelpers.ROOT_PATH, string.Empty, string.Empty);
                bLinkedLinkedViewExists = EditHelpers.XPathIO.ReplaceElement(
                    calcDoc.CreateNavigator(), sRootQry, navLinkedViewNodes,
                    string.Empty, string.Empty);
            }
            return bLinkedLinkedViewExists;
        }
        private void SetDocToCalcTempState(ContentURI calcDocURI, ContentURI docToCalcURI,
            string tempDocToCalcPath)
        {
            bool bNeedsDocToCalcTempDocState = false;
            bNeedsDocToCalcTempDocState
                = NeedsToInitDoctoCalcTempDocState(docToCalcURI);
            if (bNeedsDocToCalcTempDocState)
            {
                //0.9.1: analyzers use docToCalcURI.TempDocPath to temporarily point to base calc doc
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, docToCalcURI.URIDataManager.TempDocPath))
                {
                    DevTreks.Data.Helpers.FileStorageIO.CopyURIs(
                        docToCalcURI, docToCalcURI.URIDataManager.TempDocPath,
                        tempDocToCalcPath);
                    docToCalcURI.URIDataManager.TempDocPath = tempDocToCalcPath;
                }
                else
                {
                    DevTreks.Data.Helpers.FileStorageIO.CopyURIs(
                        docToCalcURI, docToCalcURI.URIClub.ClubDocFullPath,
                        tempDocToCalcPath);
                }
                //0.8.7 devpacks: if it exists, copy the miscdoc to a new temp miscdoc 
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, docToCalcURI.URIDataManager.MiscDocPath))
                {
                    //better to use localcache for calcs rather than http uris (i.e. base devpack doctocalc)
                    DevTreks.Data.Helpers.FileStorageIO.CopyURIs(
                        docToCalcURI, docToCalcURI.URIDataManager.MiscDocPath,
                        DataHelpers.AppSettings.GetMiscDocURI(tempDocToCalcPath));
                }
                //if it exists, copy the corresponding html fragment file
                DevTreks.Helpers.DisplayURIHelper.CopyHtmlDocsToTempDocPaths(
                    docToCalcURI, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc,
                    tempDocToCalcPath);
            }
        }
        
        
        private static void SetTempDocToCalcTempState(ContentURI calcDocURI,
            ContentURI docToCalcURI, string tempCalcDocPath, string tempDocToCalcPath)
        {
            //if the temp doc exists, display it
            if (!DataHelpers.FileStorageIO.URIAbsoluteExists(
                docToCalcURI, docToCalcURI.URIClub.ClubDocFullPath))
            {
                string sBaseTempDocPath = string.Empty;
                bool bNeedsBaseDoc = true;
                if (DataHelpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
                    calcDocURI.URIDataManager.HostName.ToLower()))
                {
                    //analyzers need to find base calculations before being run
                    //use existing patterns to find base calculators
                    string sErrorMsg = string.Empty;
                    calcDocURI.URIDataManager.TempDocPath = tempCalcDocPath;
                    string sFileExtensionType = DataHelpers.AddInHelper.GetFileExtensionType(
                        calcDocURI, ref sErrorMsg);
                    if (sFileExtensionType != calcDocURI.URIFileExtensionType)
                    {
                        string sBaseCalculatorDocURIPattern
                            = DataHelpers.AddInHelper.GetBaseCalculatorDocURIPattern(docToCalcURI,
                            sFileExtensionType);
                        IList<string> tempUrisToAnalyze = new List<string>();
                        DataHelpers.AddInHelper.FillSummaryDocURIs(docToCalcURI, calcDocURI,
                            sBaseCalculatorDocURIPattern, ref tempUrisToAnalyze, ref sErrorMsg);
                        if (tempUrisToAnalyze.Count > 0)
                        {
                            sBaseTempDocPath = tempUrisToAnalyze[0];
                        }
                    }
                    else
                    {
                        //hasn't set base calculation type in analyzer yet
                        bNeedsBaseDoc = false;
                    }
                }
                else
                {
                    //base calculators can use the base temp doc to run new calculations
                    //copy the base temp doc into the new temp doc path
                    //not could also use doctocalc.uridatamanager.tempdocuripattern to set base path
                    sBaseTempDocPath = docToCalcURI.URIClub.ClubDocFullPath.Replace(
                        Path.GetFileNameWithoutExtension(docToCalcURI.URIClub.ClubDocFullPath),
                        DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(docToCalcURI.URIPattern));
                }
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, sBaseTempDocPath))
                {
                    DataHelpers.FileStorageIO.CopyURIs(
                        docToCalcURI, sBaseTempDocPath, docToCalcURI.URIClub.ClubDocFullPath);
                }
                else
                {
                    if (bNeedsBaseDoc)
                    {
                        docToCalcURI.ErrorMessage = DevTreksErrors.MakeStandardErrorMsg(
                               string.Empty, "ADDINHELPER_NOBASECALCS");
                    }
                }
            }
        }
        private static bool IsPriceOrBudgetGroupNode(string uriNodeName)
        {
            bool bIsPriceOrBudgetGroupNode = false;
            //group nodes have the potential to generate very large files that need careful memory management
            if (uriNodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetgroup.ToString()
                || uriNodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentgroup.ToString()
                || uriNodeName == DataAppHelpers.Prices.INPUT_PRICE_TYPES.inputgroup.ToString()
                || uriNodeName == DataAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString()
                || uriNodeName == DataAppHelpers.Prices.OPERATION_PRICE_TYPES.operationgroup.ToString()
                || uriNodeName == DataAppHelpers.Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString()
                || uriNodeName == DataAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString())
            {
                bIsPriceOrBudgetGroupNode = true;
            }
            return bIsPriceOrBudgetGroupNode;
        }
        private static bool NeedsToInitDoctoCalcTempDocState(ContentURI docToCalcURI)
        {
            //does the calculation/analysis need to start with a new db doc (i.e. step zero)?
            bool bNeedsToInitDoctoCalcTempDocState = false;
            ContentURI calcDocURI =
                DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI != null)
            {
                bool bIsOKToRun = Services.Helpers.AddInRunHelper.IsOkToRunAddIn(
                    docToCalcURI, calcDocURI.URIDataManager.HostName);
                if (bIsOKToRun)
                {
                    bNeedsToInitDoctoCalcTempDocState
                        = NeedsNewDocToCalc(calcDocURI, docToCalcURI);
                }
            }
            return bNeedsToInitDoctoCalcTempDocState;
        }
        public static async Task<ContentURI> GetCalcDocAsync(IContentService contentService, 
            ContentURI docToCalcURI)
        {
            ContentURI calcDocURI = null;
            //addin
            string sCalcDocURIPattern = string.Empty;
            //view
            string sSelectedViewURIPattern = string.Empty;
            //indeterminate (from within one stylesheet selection list)
            string sSelectedURIPattern = string.Empty;
            //check for a solo &linkedviewid= form element that identifies a doctocalcuri.LinkedView member
            SetSelectedViewsListId(docToCalcURI, ref sSelectedViewURIPattern);
            if (string.IsNullOrEmpty(sSelectedViewURIPattern))
            {
                //check form els first
                SetSelectedViewsFromFormEls(docToCalcURI,
                    ref sCalcDocURIPattern, ref sSelectedViewURIPattern);
                //check selection box form els
                SetSelectedViews(docToCalcURI,
                    ref sCalcDocURIPattern, ref sSelectedViewURIPattern);
                //check doctocalc.fileextensiontype
                if (string.IsNullOrEmpty(sCalcDocURIPattern)
                    && string.IsNullOrEmpty(sSelectedViewURIPattern))
                {
                    SetSelectedViewsFromFileExt(docToCalcURI,
                        ref sCalcDocURIPattern, ref sSelectedViewURIPattern);
                }
            }
            if (docToCalcURI.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                || docToCalcURI.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                //custom docs can init with additional form els
                if (string.IsNullOrEmpty(sSelectedViewURIPattern))
                {
                    //check stylesheet form els
                    SetSelectedViewFromStylesheet(docToCalcURI,
                        ref sSelectedViewURIPattern);
                }
            }
            //set the calcDocURI
            calcDocURI = await SetLinkedViewURIsAsync(contentService, 
                docToCalcURI, sCalcDocURIPattern,
                sSelectedViewURIPattern, sSelectedURIPattern);
            return calcDocURI;
        }
        private static async Task<ContentURI> SetLinkedViewURIsAsync(IContentService contentService,
            ContentURI docToCalcURI, string selectedAddInURIPattern, 
            string selectedViewURIPattern, string selectedURIPattern)
        {
            bool bHasSet = false;
            ContentURI calcDocURI = null;
            if (!string.IsNullOrEmpty(selectedAddInURIPattern))
            {
                if (docToCalcURI.URIDataManager.ServerSubActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                {
                    calcDocURI = GetCalcDoc(docToCalcURI, selectedAddInURIPattern);
                }
                else if (docToCalcURI.URIDataManager.ServerSubActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile
                    && docToCalcURI.URIDataManager.ServerActionType 
                    == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
                {
                    calcDocURI = GetCalcDoc(docToCalcURI, selectedAddInURIPattern);
                }
            }
            //set the selected view
            if (!string.IsNullOrEmpty(selectedViewURIPattern))
            {
                bHasSet = await SetLinkedViewAsync(contentService,
                    docToCalcURI, calcDocURI, selectedViewURIPattern,
                    selectedAddInURIPattern);
            }
            if (!string.IsNullOrEmpty(selectedURIPattern))
            {
                //try to set either one
                calcDocURI = GetCalcDoc(docToCalcURI, selectedURIPattern);
                if (calcDocURI == null)
                {
                    bHasSet = await SetLinkedViewAsync(contentService, 
                        docToCalcURI, calcDocURI, selectedURIPattern, string.Empty);
                }
            }
            if (calcDocURI == null 
                && (!string.IsNullOrEmpty(selectedViewURIPattern)))
            {
                //selectedviewuri has its own collection of linkedviews containing a default lv
                calcDocURI = GetCalcDocForSelectedViewURI(docToCalcURI, selectedAddInURIPattern);
            }
            return calcDocURI;
        }
        private static void SetSelectedViews(ContentURI docToCalcURI,
            ref string selectedAddInURIPattern, ref string selectedViewURIPattern)
        {
            bool bIsEditList = false;
            bool bIsAddInList = true;
            string sSelectionBoxName = string.Empty;
            string sSelectedViewURIPattern = string.Empty;
            if (docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                //selections can be made from edits panel when editing custom docs
                bIsAddInList = false;
                bIsEditList = true;
                sSelectionBoxName = GetSelectLinkedViewBoxName(
                    bIsEditList, bIsAddInList);
                //see if a selection was made from the get views list at the top of the edits panel
                sSelectedViewURIPattern = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, sSelectionBoxName);
                if (!string.IsNullOrEmpty(sSelectedViewURIPattern))
                {
                    //this is the custom doctocalc (isselectedview = true)
                    //selection from the Get View selection list in the edits panel
                    selectedViewURIPattern = sSelectedViewURIPattern.Replace("*", string.Empty);
                }
            }
            else
            {
                //always go with what's on hand before trying further
                if (string.IsNullOrEmpty(selectedAddInURIPattern)
                    || docToCalcURI.URIFileExtensionType
                    == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                {
                    sSelectionBoxName = GetSelectLinkedViewBoxName(
                        bIsEditList, bIsAddInList);
                    //see if a selection was made from the addin list at the top of the views panel
                    string sCalcDocURIPattern = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, sSelectionBoxName);
                    if (!string.IsNullOrEmpty(sCalcDocURIPattern))
                    {
                        selectedAddInURIPattern = sCalcDocURIPattern.Replace("*", string.Empty);
                    }
                }
                if (string.IsNullOrEmpty(selectedViewURIPattern)
                    || docToCalcURI.URIFileExtensionType
                    == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                {
                    bIsEditList = false;
                    bIsAddInList = false;
                    sSelectionBoxName = GetSelectLinkedViewBoxName(
                        bIsEditList, bIsAddInList);
                    //see if a selection was made from the addin list at the top of the views panel
                    sSelectedViewURIPattern = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, sSelectionBoxName);
                    if (!string.IsNullOrEmpty(sSelectedViewURIPattern))
                    {
                        //this is the custom doctocalc (isselectedview = true)
                        //selection from the Get View selection list in the views panel
                        selectedViewURIPattern = sSelectedViewURIPattern.Replace("*", string.Empty);
                    }
                }
            }
        }
        private static void SetSelectedViewsFromFileExt(ContentURI docToCalcURI,
            ref string selectedAddInURIPattern, ref string selectedViewURIPattern)
        {
            if (docToCalcURI.URIFileExtensionType == DataHelpers.GeneralHelpers.NONE)
            {
                selectedViewURIPattern = DataHelpers.LinqHelpers.GetLinkedViewURIPatternByFileExtension(
                    docToCalcURI.URIDataManager.LinkedView, docToCalcURI.URIFileExtensionType);
            }
            else
            {
                selectedAddInURIPattern = DataHelpers.LinqHelpers.GetLinkedViewURIPatternByFileExtension(
                    docToCalcURI.URIDataManager.LinkedView, docToCalcURI.URIFileExtensionType);
            }
        }
        private static void SetSelectedViewFromStylesheet(ContentURI docToCalcURI,
            ref string selectedURIPattern)
        {
            //4. see if a selection was made from within an economics document
            string sSelectionBoxName = GetSelectLinkedViewBoxNameForStylesheet(
                docToCalcURI.URIPattern);
            string sSelectedURIPattern 
                = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, sSelectionBoxName);
            if (!string.IsNullOrEmpty(sSelectedURIPattern))
            {
                selectedURIPattern = sSelectedURIPattern.Replace("*", string.Empty);
            }
        }
        private static void SetSelectedViewsListId(ContentURI docToCalcURI,
            ref string selectedViewURIPattern)
        {
            //check address (a solo &linkedviewid= form element for simplicity)
            string sSelectedViewURIPattern = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI,
                DataAppHelpers.LinkedViews.LINKEDVIEWSID);
            if (sSelectedViewURIPattern == null)
                sSelectedViewURIPattern = string.Empty;
            selectedViewURIPattern = string.Empty;
        }
        public static void SetSelectedViewsFromFormEls(ContentURI docToCalcURI,
            ref string selectedAddInURIPattern, ref string selectedViewURIPattern)
        {
            //see if an addin is being run by using calcdocuri form element
            selectedAddInURIPattern 
                = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI, 
                DataAppHelpers.LinkedViews.CALCDOCURI);
            //check to see if a custom doc is being used
            selectedViewURIPattern
               = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI,
               DataAppHelpers.LinkedViews.LINKEDVIEWSURI);
        }
        public static bool HasSelectedViewsFromFormEls(ContentURI uri)
        {
            bool bHasSelectedViewsFromFormEls = false;
            string sSelectedViewURIPattern 
                = DataHelpers.GeneralHelpers.GetFormValue(uri,
                DataAppHelpers.LinkedViews.DOCTOCALCURI);
            if (!string.IsNullOrEmpty(sSelectedViewURIPattern)) 
                bHasSelectedViewsFromFormEls = true;
            if (!bHasSelectedViewsFromFormEls)
            {
                if ((uri.URIDataManager.AppType != DataHelpers.GeneralHelpers.APPLICATION_TYPES.accounts
                    && uri.URIDataManager.AppType != DataHelpers.GeneralHelpers.APPLICATION_TYPES.agreements
                    && uri.URIDataManager.AppType != DataHelpers.GeneralHelpers.APPLICATION_TYPES.members
                    && uri.URIDataManager.AppType != DataHelpers.GeneralHelpers.APPLICATION_TYPES.networks)
                    && (uri.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                    || uri.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits
                    || uri.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml))
                {
                    //version 6a edits linked views in edit panel
                    bHasSelectedViewsFromFormEls = true;
                }
            }
            return bHasSelectedViewsFromFormEls;
        }
        private static ContentURI GetCalcDoc(ContentURI docToCalcURI,
            string selectedAddInURIPattern)
        {
            ContentURI calcDocURI = null;
            string sLinkedViewId = ContentURI.GetURIPatternPart(selectedAddInURIPattern,
                ContentURI.URIPATTERNPART.id);
            //set the doctocalc.uridatamanager.linkedviews[].isselectedaddin
            bool bHasSelectedLinkedView =
                DataHelpers.LinqHelpers.SetLinkedViewIsSelectedAddIn(docToCalcURI,
                DataHelpers.GeneralHelpers.ConvertStringToInt(sLinkedViewId));
            if (bHasSelectedLinkedView)
            {
                //retrieve the selected addin
                calcDocURI = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            }
            else
            {
                //retrieve the default
                calcDocURI
                    = DataHelpers.LinqHelpers.GetLinkedViewIsDefaultAddIn(docToCalcURI.URIDataManager.LinkedView);
            }
            return calcDocURI;
        }
        private static ContentURI GetCalcDocForSelectedViewURI(ContentURI docToCalcURI,
            string selectedAddInURIPattern)
        {
            ContentURI calcDocURI = null;
            ContentURI selectedLinkedViewURI 
                = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
            if (selectedLinkedViewURI != null)
            {
                bool bHasSelectedLinkedView = false;
                if (!string.IsNullOrEmpty(selectedAddInURIPattern))
                {
                    string sLinkedViewId = ContentURI.GetURIPatternPart(selectedAddInURIPattern,
                        ContentURI.URIPATTERNPART.id);
                    //set the doctocalc.uridatamanager.linkedviews[].isselectedaddin
                    bHasSelectedLinkedView =
                        DataHelpers.LinqHelpers.SetLinkedViewIsSelectedAddIn(selectedLinkedViewURI,
                        DataHelpers.GeneralHelpers.ConvertStringToInt(sLinkedViewId));
                }
                if (bHasSelectedLinkedView)
                {
                    //retrieve the selected addin
                    calcDocURI = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(selectedLinkedViewURI);
                }
                else
                {
                    //set one to one
                    bool bHasCalcDoc = DataHelpers.LinqHelpers.SetLinkedViewIsDefaultOrFirstAddIn(selectedLinkedViewURI);
                    if (bHasCalcDoc)
                    {
                        //retrieve the default
                        calcDocURI
                            = DataHelpers.LinqHelpers.GetLinkedViewIsDefaultAddIn(selectedLinkedViewURI.URIDataManager.LinkedView);
                    }
                }
            }
            return calcDocURI;
        }
        private static async Task<bool> SetLinkedViewAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI,
            string selectedViewURIPattern, string selectedAddInURIPattern)
        {
            bool bHasSet = false;
            //set the doctocalc.uridatamanager.linkedviews[].isselectedview 
            //which is the doc (or subfolder) being calculated, analyzed, or storytold
            bool bHasSelectedViewURIPattern
                = DataHelpers.LinqHelpers.SetLinkedViewIsSelectedView(
                    docToCalcURI, selectedViewURIPattern);
            if (bHasSelectedViewURIPattern == false
                && calcDocURI == null && (!string.IsNullOrEmpty(selectedAddInURIPattern)))
            {
                //see if the addin param was used to store the linked view 
                //(i.e. stories linked to db budgets)
                bHasSelectedViewURIPattern
                    = DataHelpers.LinqHelpers.SetLinkedViewIsSelectedView(
                    docToCalcURI, selectedAddInURIPattern);
            }
            //see if they are only paginating, rather than selecting something
            if (bHasSelectedViewURIPattern == false)
            {
                if (docToCalcURI.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml)
                {
                    //this serversubaction works when custom docs being edited in edits panel
                    //but only being paginated, none have been selected yet
                    docToCalcURI.URIDataManager.UseSelectedLinkedView = true;
                    //use the first linked view in collection when navigating (to display something)
                    DataHelpers.LinqHelpers.SetLinkedViewUseFirst(docToCalcURI);
                }
            }
            //flag tells views and edits panels to use selectedLinkedView (xmldoc attribute field)
            if (bHasSelectedViewURIPattern)
            {
                if (docToCalcURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                    || docToCalcURI.URIDataManager.AppType
                    == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    ////addins are associated with selectedViewURIPattern
                    docToCalcURI.URIDataManager.UseSelectedLinkedView = true;
                    //update the useselectedlinkedview with some of doctocalcuri's properties
                    DataHelpers.LinqHelpers.SetLinkedViewUseSelectedView(docToCalcURI);
                    //devpacks use addins for selectedviews, not parents,
                    //so additional state is needed for selectedlinkedview
                    bHasSet = await ResetLinkedViewStateAsync(contentService,
                        docToCalcURI, calcDocURI, selectedViewURIPattern,
                        selectedAddInURIPattern);
                    //note that some of doctocalcuri's properties (i.e. authorizationlevels)
                    //have not been set yet, DataHelpers.LinqHelpers.UpdateLinkedViewIsSelectedView(docToCalcURI) 
                    //must be run after all of those have been set
                }
                else
                {
                    //linkedviews panel uses runaddin
                    //edit and linkedviews panel use respondwithhtml to init
                    //edit panel uses submit edits to edit custom docs
                    if (docToCalcURI.URIDataManager.ServerSubActionType
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin
                        || docToCalcURI.URIDataManager.ServerSubActionType
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml
                        || docToCalcURI.URIDataManager.ServerSubActionType
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits
                        || docToCalcURI.URIDataManager.ServerSubActionType
                        == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml)
                    {
                        //addins are associated with doctocalcuri
                        //running addins or running related stories
                        docToCalcURI.URIDataManager.UseSelectedLinkedView = true;
                        //update the useselectedlinkedview with some of doctocalcuri's properties
                        DataHelpers.LinqHelpers.SetLinkedViewUseSelectedView(docToCalcURI);
                        //note that some of doctocalcuri's properties (i.e. authorizationlevels)
                        //have not been set yet, DataHelpers.LinqHelpers.UpdateLinkedViewIsSelectedView(docToCalcURI) 
                        //must be run after all of those have been set
                    }
                }
            }
            bHasSet = true;
            return bHasSet;
        }
        private static async Task<bool> ResetLinkedViewStateAsync(IContentService contentService, 
            ContentURI docToCalcURI, ContentURI calcDocURI,
            string selectedViewURIPattern, string selectedAddInURIPattern)
        {
            bool bHasSet = false;
            ContentURI selectedLinkedViewURI 
                = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
            if (selectedLinkedViewURI != null)
            {
                AddInStateHelper stateHelper = new AddInStateHelper();
                bHasSet = await stateHelper.ResetDocToCalcStateAsync(contentService,
                    docToCalcURI, selectedLinkedViewURI);
                //calcdoc should only be set during runaddin actions (need the set view link to return stories)
                if (docToCalcURI.URIDataManager.ServerSubActionType
                    == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                {
                    bool bHasLinkedViewAddIn = false;
                    if (!string.IsNullOrEmpty(selectedAddInURIPattern))
                    {
                        string sId = ContentURI.GetURIPatternPart(selectedAddInURIPattern, ContentURI.URIPATTERNPART.id);
                        int iId = DataHelpers.GeneralHelpers.ConvertStringToInt(sId);
                        bHasLinkedViewAddIn = DataHelpers.LinqHelpers.SetLinkedViewIsSelectedAddIn(selectedLinkedViewURI, iId);
                    }
                    if (!bHasLinkedViewAddIn)
                    {
                        bHasLinkedViewAddIn = DataHelpers.LinqHelpers.SetLinkedViewIsDefaultOrFirstAddIn(selectedLinkedViewURI);
                    }
                }
                docToCalcURI.ErrorMessage += selectedLinkedViewURI.ErrorMessage;
            }
            return bHasSet;
        }
        public static string GetSelectLinkedViewBoxName(
            bool isEditLinkedViewBox, bool isAddInBox)
        {
            string sSelectionBoxName = string.Empty;
            if (isAddInBox)
            {
                sSelectionBoxName = NAME_FORADDINVIEW;
            }
            else if (isEditLinkedViewBox)
            {
                sSelectionBoxName = NAME_FOREDITS;
            }
            else
            {
                //hard-coded in onclickpresenter.addSelectedLinkedViewToFormEls()
                sSelectionBoxName = NAME_FORVIEWS;
            }
            return sSelectionBoxName;
        }
        public static string GetSelectLinkedViewBoxNameForStylesheet(
            string nodeToCalcURIPattern)
        {
            string sSelectionBoxName = string.Concat(nodeToCalcURIPattern,
                ";DefaultDevDocId;int;4");
            return sSelectionBoxName;
        }
        private async Task<List<string>> SetTempDocsPathsandFileNamesAsync(IContentService contentService,
            ContentURI docToCalcURI, ContentURI calcDocURI)
        {
            List<string> tempDocs = new List<string>();
            string tempDocURIPattern = string.Empty;
            string tempCalcDocPath = string.Empty;
            string tempDocToCalcPath = string.Empty;
            if (docToCalcURI.URIDataManager.TempDocURIPattern
                == string.Empty)
            {
                DataHelpers.AppSettings.GetTempDocURIPattern(calcDocURI, out 
                    tempDocURIPattern);
            }
            else
            {
                tempDocURIPattern = docToCalcURI.URIDataManager.TempDocURIPattern;
            }
            //copy files into tempdocpaths
            SetTempDocsPathsandFileNames(docToCalcURI, calcDocURI,
                tempDocURIPattern, out tempCalcDocPath, out tempDocToCalcPath);
            //overwrite if they are in the middle of running calcs
            if (!DataHelpers.FileStorageIO.URIAbsoluteExists(
                docToCalcURI, tempCalcDocPath))
            {
                tempCalcDocPath = await SetCalcDocTempStateAsync(contentService, 
                    docToCalcURI, calcDocURI, tempDocToCalcPath, tempCalcDocPath);
            }
            if (tempCalcDocPath != string.Empty)
            {
                if (!DataHelpers.FileStorageIO.URIAbsoluteExists(
                    docToCalcURI, tempDocToCalcPath))
                {
                    if (docToCalcURI.URIFileExtensionType
                        != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        SetDocToCalcTempState(calcDocURI, docToCalcURI, tempDocToCalcPath);
                    }
                    else
                    {
                        SetTempDocToCalcTempState(calcDocURI, docToCalcURI,
                            tempCalcDocPath, tempDocToCalcPath);
                    }
                }
            }
            if (tempCalcDocPath == string.Empty)
            {
                //done with addin helper
                //needs a linked linkedview displayed normally
                //not an addin linkedview
                //or an error message displayed 
                tempDocURIPattern = string.Empty;
                tempCalcDocPath = string.Empty;
                tempDocToCalcPath = string.Empty;
            }
            tempDocs.Add(tempDocURIPattern);
            tempDocs.Add(tempCalcDocPath);
            tempDocs.Add(tempDocToCalcPath);
            return tempDocs;
        }
        private void SetTempDocsPathsandFileNames(ContentURI docToCalcURI,
            ContentURI calcDocURI, string tempDocURIPattern, out string tempCalcDocPath,
            out string tempDocToCalcPath)
        {
            tempCalcDocPath = string.Empty;
            tempDocToCalcPath = string.Empty;
            //set the tempsubdirectory where calcs will be save temporarily (tempdocuri)
            if (docToCalcURI.URIDataManager.TempDocURIPattern == string.Empty)
                docToCalcURI.URIDataManager.TempDocURIPattern = tempDocURIPattern;
            string sNewURIPattern 
                = DataHelpers.AppSettings.GetTempDocToCalcURIPattern(docToCalcURI, 
                tempDocURIPattern);
            //holds any calculation made to doctocalc
            if (docToCalcURI.URIFileExtensionType
                != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //use a new doctocalcpath in the temp directory
                tempDocToCalcPath = DataHelpers.AppSettings.GetTempDocPath(docToCalcURI,
                    sNewURIPattern);
            }
            else
            {
                //use the existing doctocalcpath -it's in temp directory
                tempDocToCalcPath = docToCalcURI.URIClub.ClubDocFullPath;
            }
            //holds any edit made to calcdoc
            string sTempCalcDocURIPattern = calcDocURI.URIPattern;
            sTempCalcDocURIPattern = ContentURI.ChangeURIPatternPart(sTempCalcDocURIPattern, ContentURI.URIPATTERNPART.fileExtension,
                DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString());
            tempCalcDocPath
                = tempDocToCalcPath.Replace(Path.GetFileNameWithoutExtension(tempDocToCalcPath),
                DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(sTempCalcDocURIPattern));
        }
        public static void GetSelectedViewsURIPatterns(ContentURI docToCalcURI,
            ref string selectedViewURIPattern, ref string selectedAddInViewURIPattern)
        {
            ContentURI selectedViewURI
                = DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
            if (selectedViewURI != null) selectedViewURIPattern = selectedViewURI.URIPattern;
            ContentURI selectedAddInViewURI
                = DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (selectedAddInViewURI != null) selectedAddInViewURIPattern = selectedAddInViewURI.URIPattern;
        }
        public static string GetUpdatesFilePath(string tempDocPath)
        {
            string sUpdatesDocPath = tempDocPath.Replace(".xml", "_updates.xml");
            return sUpdatesDocPath;
        }
    }
}
