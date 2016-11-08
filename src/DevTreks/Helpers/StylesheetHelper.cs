using DevTreks.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers;
using EditHelpers = DevTreks.Data.EditHelpers;
using RuleHelpers = DevTreks.Data.RuleHelpers;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		Class for helping stylesheets and chtml pages display data
    ///Author:		www.devtreks.org
    ///Date:		2016, November
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTES        Known issues:
    ///             1. Caching xhtml is not being done yet.
    ///             2. Client side transformations need to be an option.
    /// </summary>
    public class StylesheetHelper
    {
        public StylesheetHelper() { }
        private static HtmlEncoder _enc = HtmlEncoder.Default;
        public enum START_ROW_TYPES
        {
            oldstartrow = 0,
            startrow = 1,
            isforward = 2,
            parentstartrow = 3
        }
        public async Task TransformXmlToXhtmlAsync(StringWriter writer,
            ContentURI uri, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER displayDocType,
            XmlReader reader)
        {
            if (reader != null)
            {
                //keep one uniform set of uri for all stylesheets (i.e. for a uniform display pattern for all documents)
                IDictionary<string, string> lstStyleParams = new Dictionary<string, string>();
                await GetStandardParamsForStylesheetsAsync(uri, displayDocType,
                    lstStyleParams);
                //return a writer containing xhtml to the client
                HtmlHelperExtensions.MakeHtml(uri, reader, displayDocType, 
                    lstStyleParams).WriteTo(writer, HtmlEncoder.Default);
            }
        }
        
        public XmlReader GetStyleSheet(ContentURI uri,
            DataHelpers.GeneralHelpers.DOC_STATE_NUMBER displayDocType,
            ref object extensionObject, ref ContentURI stylesheetURI)
        {
            XmlReader stylesheetReader = null;
            //if stylesheetURI hasn't been set, an error message should have
            //already been generated from the AddInStylesheetHelper module
            if (displayDocType
                == DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.firstdoc)
            {
                stylesheetURI
                    = DataHelpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                        uri.URIDataManager.Resource);
            }
            else if (displayDocType
                == DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
            {
                //calcdocuri
                ContentURI calcDocURI =
                    DataHelpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(uri);
                if (calcDocURI != null)
                {
                    stylesheetURI
                        = DataHelpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                            calcDocURI.URIDataManager.Resource);
                }
            }
            if (displayDocType
                == DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
            {
                if (uri.URIDataManager.UseSelectedLinkedView == true)
                {
                    //selectedlinkedviewuri
                    ContentURI selectedLinkedViewURI =
                        DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(uri);
                    if (selectedLinkedViewURI != null)
                    {
                        stylesheetURI
                            = DataHelpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                                selectedLinkedViewURI.URIDataManager.Resource);
                    }
                    else
                    {
                        stylesheetURI
                        = DataHelpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                        uri.URIDataManager.Resource);
                    }
                }
                else
                {
                    stylesheetURI
                        = DataHelpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                        uri.URIDataManager.Resource);
                }

            }
            if (stylesheetURI != null)
            {
                string sURIPath = string.Empty;
                if (displayDocType
                    == DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.firstdoc)
                {
                    //static stylsheets found in rel path to web root and uploaded in packages
                    sURIPath = stylesheetURI.URIDataManager.FileSystemPath;
                }
                else
                {
                    //stylesheets stored dynamically
                    sURIPath = DataHelpers.FileStorageIO.GetResourceURIPath(stylesheetURI,
                        stylesheetURI.URIDataManager.FileSystemPath);
                }
                if (!string.IsNullOrEmpty(sURIPath))
                {
                    stylesheetReader = GetStylesheetFromCache(uri,
                        sURIPath);
                    if (stylesheetReader == null)
                    {
                        uri.ErrorMessage
                            = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "STYLEHELPER_NOSTYLEFROMCACHE");
                    }
                    GetDisplayObjects(stylesheetURI, ref extensionObject);
                }
                else
                {
                    uri.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "STYLEHELPER_NOSTYLE");
                }
            }
            else
            {
                uri.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "STYLEHELPER_NOSTYLE");
            }
            return stylesheetReader;
        }

        private XmlReader GetStylesheetFromCache(ContentURI uri,
            string stylePath)
        {
            //refactor: when caching is done correctly
            XmlReader reader = DataHelpers.FileStorageIO.GetXmlReader(uri, stylePath);
            return reader;
        }
        public async Task<string> GetResourceUrlArrayAsync(ContentURI resourceURI)
        {
            string sResourceURLArray = string.Empty;
            IContentRepositoryEF contentRepository
                = new DevTreks.Data.SqlRepositories.ContentRepository(resourceURI);
            //resourcepath;resourcealt;resourceuri
            if (resourceURI.URIId != 0
                && resourceURI.URINodeName != DataHelpers.GeneralHelpers.NONE)
            {
                bool bNeedsFullPath = true;
                bool bNeedsOneRecord = false;
                sResourceURLArray = await contentRepository.GetResourceURLsAsync(resourceURI, bNeedsOneRecord, bNeedsFullPath,
                    DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.storyuri,
                    string.Empty);
                contentRepository.Dispose();
            }
            return sResourceURLArray;
        }

        //2.0.0 refactor: resourceURLs added to stylesheet params (linkedlistsarray)
        public string GetResourceUrl2(string resourceFileName,
            string devTrekURIPattern, string linkedViewURIPattern,
            string needsFullPath, string serverSubActionType, string resourceURLs)
        {
            //called by internal db xmldocs (where only an image's file name 
            //and the containing node's uripattern are known)
            bool bNeedsFullPath = (needsFullPath.Equals("true")) ? true : false;
            string sResourceUrl = string.Empty;
            if (string.IsNullOrEmpty(resourceURLs) == false
                && string.IsNullOrEmpty(resourceFileName) == false)
            {
                List<string> resources 
                    = resourceURLs.Split(DataHelpers.GeneralHelpers.PARAMETER_DELIMITERS).ToList();
                if (resources != null)
                {
                    foreach(var resource in resources)
                    {
                        if (resource.ToLower().Contains(resourceFileName.ToLower()))
                        {
                            sResourceUrl = DataHelpers.GeneralHelpers.GetDelimitedSubstring(resource,
                                DataHelpers.GeneralHelpers.STRING_DELIMITERS, 0);
                        }
                    }
                }
            }
            return sResourceUrl;
        }
        
        public string GetMimeType(string resourceFilePath)
        {
            string sErrorMsg = string.Empty;
            string sFileExtension = Path.GetExtension(resourceFilePath);
            string sMimeType = DataAppHelpers.Resources.GetMimeTypeFromFileExt(sFileExtension,
                ref sErrorMsg);
            return sMimeType;
        }
        public async Task<string> GetResourceUrlsAsync(ContentURI model,
            string devTrekURIPattern, string resourceType,
            bool needsOneRecord, string serverSubActionType)
        {
            //returns: relwebpath1;altdesc1;uripattern@relwebpath2;altdesc2;uripattern
            string sResourceUrls = string.Empty;
            ContentURI uri = ContentURI.ConvertShortURIPattern(devTrekURIPattern);
            //packages use same relpath to resources
            uri.URIDataManager.ServerSubActionType = (serverSubActionType != string.Empty)
                ? (DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES)Enum.Parse(typeof(DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES),
                serverSubActionType) : DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.none;
            bool bNeedsFullPath = false;
            IContentRepositoryEF contentRepository
                    = new DevTreks.Data.SqlRepositories.ContentRepository(uri);
            sResourceUrls = await contentRepository.GetResourceURLsAsync(uri,
                needsOneRecord, bNeedsFullPath,
                DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcetype,
                resourceType);
            contentRepository.Dispose();
            return sResourceUrls;
        }
        public async Task<string> GetResourceUrlByResourceAsync(ContentURI model,
            string devTrekURIPattern, string resourcePackId, string serverSubActionType)
        {
            //called from within resources.xslt stylesheet to display images
            string sResourceUrl = string.Empty;
            ContentURI uri = ContentURI.ConvertShortURIPattern(devTrekURIPattern);
            //packages use same relpaths to resources
            uri.URIDataManager.ServerSubActionType = (serverSubActionType != string.Empty)
               ? (DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES)Enum.Parse(typeof(DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES),
               serverSubActionType) : DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.none;
            bool bNeedsOneRecord = true;
            bool bNeedsFullPath = false;
            IContentRepositoryEF contentRepository
                = new DevTreks.Data.SqlRepositories.ContentRepository(uri);
            sResourceUrl = await contentRepository.GetResourceURLsAsync(uri,
                bNeedsOneRecord, bNeedsFullPath,
                DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcepackid,
                resourcePackId);
            contentRepository.Dispose();
            return sResourceUrl;
        }
        public string GetResourceUrlByResource(ContentURI model, string devTrekURIPattern,
            string resourcePackId, string serverSubActionType)
        {
            //called from within resources.xslt stylesheet to display images
            string sResourceUrl = string.Empty;
            ContentURI uri = ContentURI.ConvertShortURIPattern(devTrekURIPattern);
            uri.URIDataManager = new ContentURI.DataManager(model.URIDataManager);
            //packages use same relpaths to resources
            uri.URIDataManager.ServerSubActionType = (serverSubActionType != string.Empty)
               ? (DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES)Enum.Parse(typeof(DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES),
               serverSubActionType) : DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.none;
            bool bNeedsOneRecord = true;
            bool bNeedsFullPath = false;
            IContentRepositoryEF contentRepository
                = new DevTreks.Data.SqlRepositories.ContentRepository(uri);
            sResourceUrl = contentRepository.GetResourceURLs(uri,
                bNeedsOneRecord, bNeedsFullPath,
                DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcepackid,
                resourcePackId);
            contentRepository.Dispose();
            return sResourceUrl;
        }
        public async Task<string> GetResourceURLsByStoryURIandTagName(ContentURI model,
            string devTrekURIPattern, string tagName)
        {
            string sResourceURLs = string.Empty;
            //returns: full path to resource
            bool bNeedsOneRecord = true;
            bool bNeedsFullPath = true;
            ContentURI uri = ContentURI.ConvertShortURIPattern(devTrekURIPattern);
            IContentRepositoryEF contentRepository
                = new DevTreks.Data.SqlRepositories.ContentRepository(uri);
            //should be only one: resourcepath;resourcealt;resourceuri
            sResourceURLs = await contentRepository.GetResourceURLsAsync(uri,
               bNeedsOneRecord, bNeedsFullPath,
               DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.storyuriandtagname,
               tagName);
            contentRepository.Dispose();
            return sResourceURLs;
        }
        public async Task<string> GetResourceURLsByStoryURIandLabel(ContentURI model,
            string devTrekURIPattern, string label)
        {
            string sResourceURLs = string.Empty;
            //returns: full path to resource
            bool bNeedsOneRecord = true;
            bool bNeedsFullPath = true;
            ContentURI uri = ContentURI.ConvertShortURIPattern(devTrekURIPattern);
            IContentRepositoryEF contentRepository
                = new DevTreks.Data.SqlRepositories.ContentRepository(uri);
            sResourceURLs = await contentRepository.GetResourceURLsAsync(uri,
               bNeedsOneRecord, bNeedsFullPath,
               DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.storyuriandlabel,
               label);
            contentRepository.Dispose();
            return sResourceURLs;
        }
        public string GetResourceParam(string arrayPosition, string resourceParams)
        {
            string sResourceParam = string.Empty;
            if (string.IsNullOrEmpty(arrayPosition) == false)
            {
                int iPosition = DataHelpers.GeneralHelpers.ConvertStringToInt(arrayPosition);
                sResourceParam = DataHelpers.GeneralHelpers.GetDelimitedSubstring(resourceParams,
                    DataHelpers.GeneralHelpers.STRING_DELIMITERS, iPosition);
            }
            return sResourceParam;
        }
        
        private async Task GetStandardParamsForStylesheetsAsync(
            ContentURI uri, DataHelpers.GeneralHelpers.DOC_STATE_NUMBER displayDocType,
            IDictionary<string, string> styleParams)
        {
            string sDevTrekFilePath = System.Net.WebUtility.UrlEncode(
                DataHelpers.AddInHelper.GetDevTrekPath(uri, displayDocType));
            styleParams.Add("fullFilePath", sDevTrekFilePath);
            styleParams.Add("contenturipattern", uri.URIDataManager.ContentURIPattern);
            styleParams.Add("clubEmail", uri.URIClub.AccountEmail);
            AddAppSettingsParams(uri, ref styleParams);
            string sPageParams = SetRowArgs(uri.URIDataManager.StartRow,
                uri.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                uri.URIDataManager.ParentStartRow);
            styleParams.Add("pageParams", sPageParams);
            //use the friendlier name in the uris rather than the integer id
            styleParams.Add("networkId", uri.URINetwork.NetworkURIPartName.ToString());
            styleParams.Add("selectionsNodeNeededName", uri.URIDataManager.SelectionsNodeNeededName);
            AddLinkedViewStylesheetParams(uri, displayDocType, sPageParams, ref styleParams);
            styleParams.Add("memberRole", uri.URIMember.MemberRole.ToString());
            string sViewEditType = DataHelpers.GeneralHelpers.GetViewEditType(uri, displayDocType);
            styleParams.Add("viewEditType", sViewEditType);
            styleParams.Add("nodeName", GetCurrentNodeToDisplay(uri));
            styleParams.Add("isURIOwningClub", DataAppHelpers.Accounts.IsURIOwningClub(uri).ToString());
            //packages use same path to images when downloaded onto client
            styleParams.Add("serverSubActionType", uri.URIDataManager.ServerSubActionType.ToString());
            styleParams.Add("serverActionType", uri.URIDataManager.ServerActionType.ToString());
            //add misc parameters
            if (uri.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
            {
                IContentRepositoryEF contentRepository
                    = new DevTreks.Data.SqlRepositories.ContentRepository(uri);
                await contentRepository.GetStylesheetParametersForAddInAsync(uri, styleParams);
                contentRepository.Dispose();
            }
        }
        public static string WriteSelectListForStatus(string searchURL,
            string selectedStatus, string viewEditType)
        {
            string sSelectList = selectedStatus;
            if (viewEditType
                == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
            {
                ContentURI uri = ContentURI.ConvertShortURIPattern(searchURL);
                sSelectList = WriteSelectListForStatus(uri, selectedStatus, viewEditType);
            }
            else
            {
                foreach (KeyValuePair<string, string> kvp in DataHelpers.GeneralHelpers.GetDocStatusDictionary())
                {
                    if (kvp.Key == selectedStatus)
                    {
                        sSelectList = kvp.Value;
                        break;
                    }
                }
            }
            return sSelectList;
        }
        private static string WriteSelectListForStatus(ContentURI uri, string selectedStatusTypeInt,
            string viewEditType)
        {
            string sStatusDocSelectHtml = string.Empty;
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                string sJavascriptMethod = string.Empty;
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                    = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
                string sFormElementName = EditHelpers.EditHelper.MakeStandardEditName(
                    uri.URIPattern, "DocStatus", RuleHelpers.GeneralRules.SHORTINTEGER, "2");
                HtmlExtensions.SelectStart(eViewEditType, string.Concat("DocStatus", uri.URIId),
                    sFormElementName, string.Empty).WriteTo(writer, HtmlEncoder.Default);
                bool bIsSelected = false;
                foreach (KeyValuePair<string, string> kvp in DataHelpers.GeneralHelpers.GetDocStatusDictionary())
                {
                    bIsSelected = false;
                    if (kvp.Key == selectedStatusTypeInt)
                    {
                        bIsSelected = true;
                    }
                    //counterintuitive but the dictionary's key is the unique value for the options
                    HtmlExtensions.Option(kvp.Value, kvp.Key, bIsSelected)
                        .WriteTo(writer, HtmlEncoder.Default);
                }
                HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
                sStatusDocSelectHtml = writer.ToString();
            }
            return sStatusDocSelectHtml;
        }
        private static void AddLinkedViewStylesheetParams(ContentURI uri,
            DataHelpers.GeneralHelpers.DOC_STATE_NUMBER displayDocType,
            string pageParams, ref IDictionary<string, string> styleParams)
        {
            //set the selectedFileURIPattern, calcDocURI, docToCalcURI, tempDocURI, and docToCalcNodeName params
            string sCalcParams = string.Empty;
            string sCalcDocURI = string.Empty;
            string sDocToCalcURIPattern = string.Empty;
            bool bNeedsSingleQuote = false;
            if (uri.URIDataManager.UseSelectedLinkedView == false
                && displayDocType != DataHelpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc
                && (uri.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                || uri.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks))
            {
                //these are regular linkedviews and devpacks
                styleParams.Add("selectedFileURIPattern", uri.URIPattern);
                styleParams.Add("calcDocURI", string.Empty);
                styleParams.Add("docToCalcNodeName", string.Empty);
                styleParams.Add("calcParams", string.Empty);
            }
            else
            {
                sCalcDocURI = string.Empty;
                string sSelectedLinkedViewURI = string.Empty;
                if ((uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks
                    || uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                    || uri.URIFileExtensionType == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    && (!string.IsNullOrEmpty(uri.URIDataManager.ParentURIPattern)))
                {
                    styleParams.Add("selectedFileURIPattern", uri.URIDataManager.ParentURIPattern);
                }
                else
                {
                    styleParams.Add("selectedFileURIPattern", uri.URIPattern);
                }
                sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(bNeedsSingleQuote,
                    uri, string.Empty, ref sCalcDocURI, ref sDocToCalcURIPattern, ref sSelectedLinkedViewURI);
                sCalcParams = string.Concat(pageParams, sCalcParams);
                styleParams.Add("calcDocURI", sCalcDocURI);
                string sDocToCalcNodeName = ContentURI.GetURIPatternPart(
                    sDocToCalcURIPattern, ContentURI.URIPATTERNPART.node);
                styleParams.Add("docToCalcNodeName", sDocToCalcNodeName);
                styleParams.Add("calcParams", sCalcParams);
            }
        }
        public static void SetModelDisplayParameters(ContentURI uri)
        {
            //set the selectedFileURIPattern, calcDocURI, docToCalcURI, tempDocURI, and docToCalcNodeName params
            string sCalcParams = string.Empty;
            string sCalcDocURI = string.Empty;
            string sDocToCalcURIPattern = string.Empty;
            bool bNeedsSingleQuote = false;
            if (uri.URIDataManager.UseSelectedLinkedView == false
                && (uri.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                || uri.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks))
            {
                uri.URIDataManager.CalcParams = string.Empty;
            }
            else
            {
                string sSelectedLinkedViewURI = string.Empty;
                sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(bNeedsSingleQuote,
                    uri, string.Empty, ref sCalcDocURI, ref sDocToCalcURIPattern, ref sSelectedLinkedViewURI);
                uri.URIDataManager.CalcParams = sCalcParams;
            }
        }
        private void AddAppSettingsParams(ContentURI uri,
            ref IDictionary<string, string> styleParams)
        {
            //all stylesheets still include these params, but these params are not used
            styleParams.Add("startRow", "0");
            styleParams.Add("endRow", uri.URIDataManager.RowCount.ToString());
            //if needed, consider using to store conn strings, 
            //but requires changing stylesheets, so better to just add to end of linkedlistsarray
            //styleParams.Add("startRow", uri.URIDataManager.DefaultConnection);
            //styleParams.Add("endRow", uri.URIDataManager.StorageConnection);
        }
        public static string SetEndRow(ContentURI uri)
        {
            string sEndRow = string.Empty;
            int iStartRow = uri.URIDataManager.StartRow;
            int iEndRow = iStartRow + uri.URIDataManager.PageSize;
            uri.URIDataManager.StartRow = iStartRow;
            sEndRow = iEndRow.ToString();
            return sEndRow;
        }

        public static string GetCurrentNodeToDisplay(ContentURI uri)
        {
            string sCurrentNodeToDisplay = uri.URINodeName;
            if (uri.URIFileExtensionType
                == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                sCurrentNodeToDisplay
                    = DataHelpers.GeneralHelpers.GetTerminalNode(uri);
            }
            return sCurrentNodeToDisplay;
        }
        private void GetDisplayObjects(ContentURI stylesheetURI,
            ref Object newObject)
        {
            if (stylesheetURI.URIDataManager.ExtensionObjectNamespace.EndsWith(
                DataAppHelpers.Resources.DISPLAY_NAMESPACE_TYPES.displaycomps.ToString()))
            {
                newObject = new DisplayComparisons();
                stylesheetURI.URIDataManager.ExtensionObjectNamespace = string.Concat("urn:",
                    DataAppHelpers.Resources.DISPLAY_NAMESPACE_TYPES.displaycomps.ToString());
            }
            else
            {
                //default is this.
                newObject = new StylesheetHelper();
                stylesheetURI.URIDataManager.ExtensionObjectNamespace = string.Concat("urn:",
                    DataAppHelpers.Resources.DISPLAY_NAMESPACE_TYPES.displaydevpacks.ToString());
            }
            
        }
        public string WriteGetConstantsButtons(string uriPattern,
           string calcParams)
        {
            //legacy
            return WriteGetConstantsButtons(uriPattern, string.Empty, calcParams);

        }
        public string WriteGetConstantsButtons(string uriPattern,
           string contenturipattern, string calcParams)
        {
            string sXhtml = string.Empty;
            string sContentURIPatternForClose = string.Empty;
            string sContentURIPatternForCalculate = string.Empty;
            GetCalculatorCommands(uriPattern, contenturipattern,
                ref sContentURIPatternForClose, ref sContentURIPatternForCalculate);
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlHelperExtensions.MakeGetConstantsButtons(calcParams,
                    sContentURIPatternForClose, sContentURIPatternForCalculate)
                    .WriteTo(writer, HtmlEncoder.Default);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public string WriteViewLinks(string nodeToCalcURIPattern, string contenturipattern,
            string calcParams, string nodeNeededName, string baseId)
        {
            string sXhtml = string.Empty;
            string sName = string.Empty;
            string sMethod = string.Empty;
            if ((!string.IsNullOrEmpty(nodeToCalcURIPattern))
                && (!string.IsNullOrEmpty(contenturipattern)))
            {
                ContentURI selectedFileURI = new ContentURI(contenturipattern);
                ContentURI nodeToCalcURI = ContentURI.ConvertShortURIPattern(nodeToCalcURIPattern);
                nodeToCalcURI.URIDataManager.ControllerName
                    = selectedFileURI.URIDataManager.ControllerName;
                if (selectedFileURI.URIFileExtensionType
                    == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                {
                    //selectedFileURI holds the actual tempdoc and must init state
                    //nodeToCalcURI has a real db key (baseid) and can be used to establish
                    //linkedviews state
                    nodeToCalcURI.URIFileExtensionType
                        = selectedFileURI.URIFileExtensionType;
                    nodeToCalcURI.UpdateURIPattern();
                    if (!string.IsNullOrEmpty(baseId))
                    {
                        //baseid is the db key id; tempdocs change selections to a randomid
                        nodeToCalcURI.URIDataManager.BaseId
                            = DataHelpers.GeneralHelpers.ConvertStringToInt(baseId);
                    }
                }
                //write to the txtWriter
                using (StringWriter writer = new StringWriter())
                {
                    WriteEditLinkedViewListLink(writer, nodeToCalcURI, selectedFileURI);
                    if (selectedFileURI.URIDataManager.AppType != DataHelpers.GeneralHelpers.APPLICATION_TYPES.addins
                        && selectedFileURI.URIDataManager.AppType != DataHelpers.GeneralHelpers.APPLICATION_TYPES.locals)
                    {
                        WriteOpenInLinkedViewPanelLink(writer,
                            nodeToCalcURI, selectedFileURI, nodeNeededName);
                    }
                    sXhtml = writer.ToString();
                }
            }
            return sXhtml;
        }

        public static void WriteOpenInLinkedViewPanelLink(StringWriter writer,
            ContentURI nodeToCalcURI, ContentURI linkedviewURI,
            string nodeNeededName)
        {
            string sClientAction = string.Empty;
            string sContentURIPattern = string.Empty;
            bool bNeedsSingleQuote = false;
            string sCalcParams = string.Empty;
            if (DataHelpers.AddInHelper.IsAddIn(linkedviewURI)
                && linkedviewURI.URIPattern != nodeToCalcURI.URIPattern)
            {
                //uri is parent doctocalc; linkedview is now calcdoc
                sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                    bNeedsSingleQuote, string.Empty, nodeToCalcURI.URIPattern,
                    linkedviewURI.URIPattern, string.Empty, string.Empty, string.Empty);
                sClientAction = DataHelpers.GeneralHelpers.CLIENTACTION_TYPES.prepaddin.ToString();
                sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                   nodeToCalcURI.URIDataManager.ControllerName,
                   DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                   nodeToCalcURI.URIPattern,
                   DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString(),
                   nodeToCalcURI.URIDataManager.SubActionView,
                   nodeToCalcURI.URIDataManager.Variable);
            }
            else
            {
                if (linkedviewURI.URIFileExtensionType
                    == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                {
                    //tempdocs init with linkedviewURI, but use nodetocalcuri to
                    //retrieve linkedviews
                    sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                        bNeedsSingleQuote, string.Empty, nodeToCalcURI.URIPattern,
                        string.Empty, linkedviewURI.URIPattern, string.Empty, string.Empty);
                    sClientAction = DataHelpers.GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();
                    sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                       nodeToCalcURI.URIDataManager.ControllerName,
                       DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                       linkedviewURI.URIPattern,
                       DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                       nodeToCalcURI.URIDataManager.SubActionView,
                       DataHelpers.GeneralHelpers.NONE);
                }
                else
                {
                    //uri is parent doctocalc; linkedview is now selectedlinkedview
                    sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                        bNeedsSingleQuote, string.Empty, nodeToCalcURI.URIPattern,
                        string.Empty, string.Empty, linkedviewURI.URIPattern, string.Empty);
                    sClientAction = DataHelpers.GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();
                    sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                       nodeToCalcURI.URIDataManager.ControllerName,
                       DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                       nodeToCalcURI.URIPattern,
                       DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                       nodeToCalcURI.URIDataManager.SubActionView,
                       DataHelpers.GeneralHelpers.NONE);
                }
            }
            HtmlExtensions.LinkUnobtrusiveMobile(string.Concat("linktodefaultaddin", nodeToCalcURI.URIId.ToString()),
                "#", "JSLink", AppHelper.GetResource("DISPLAY_OPENINVIEWS"), sContentURIPattern,
                sClientAction, sCalcParams, "button", "true", "true", "arrow-r", "right")
                .WriteTo(writer, HtmlEncoder.Default);
        }
        public static void WriteEditLinkedViewListLink(StringWriter writer,
            ContentURI nodeToCalcURI)
        {
            //which list needs to be edited?
            string sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                nodeToCalcURI.URIDataManager.ControllerName,
                DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                nodeToCalcURI.URIPattern,
                DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString(),
                DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString(),
                DataHelpers.GeneralHelpers.NONE);
            HtmlExtensions.LinkUnobtrusiveMobile(string.Concat(nodeToCalcURI.URIId.ToString(), "editlinkedviews"),
                "#", "JSLink", AppHelper.GetResource("LINKEDVIEWS_EDIT"), sContentURIPattern,
                DataHelpers.GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                string.Empty, "button", "true", "true", "forward", "left")
                .WriteTo(writer, HtmlEncoder.Default);
        }
        public static void WriteEditLinkedViewListLink(StringWriter writer,
            ContentURI nodeToCalcURI, ContentURI selectedFileURI)
        {
            //which list needs to be edited?
            string sCalcParams = string.Empty;
            string sClientAction = string.Empty;
            string sContentURIPattern = string.Empty;
            if (selectedFileURI.URIFileExtensionType
                == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //tempdocs init with selectedFileURI, but use nodetocalcuri to
                //retrieve linkedviews
                bool bNeedsSingleQuote = false;
                string sBaseNodeToCalcURIPattern = nodeToCalcURI.URIPattern;
                if (nodeToCalcURI.URIDataManager.BaseId != 0)
                {
                    sBaseNodeToCalcURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(
                        nodeToCalcURI.URIName, nodeToCalcURI.URIDataManager.BaseId.ToString(),
                        nodeToCalcURI.URINetworkPartName, nodeToCalcURI.URINodeName,
                        nodeToCalcURI.URIFileExtensionType);
                }
                sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                    bNeedsSingleQuote, string.Empty, sBaseNodeToCalcURIPattern,
                    string.Empty, selectedFileURI.URIPattern, string.Empty, string.Empty);
                //tempdocs always init with linkedviewuri
                sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                    selectedFileURI.URIDataManager.ControllerName,
                    DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    selectedFileURI.URIPattern,
                    DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString(),
                    DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString(),
                    selectedFileURI.URIDataManager.Variable);
            }
            else
            {
                //edit using submitlistedits (same pattern as categories)
                sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                    nodeToCalcURI.URIDataManager.ControllerName,
                    DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    nodeToCalcURI.URIPattern,
                    DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString(),
                    DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString(),
                    nodeToCalcURI.URIDataManager.Variable);
            }
            HtmlExtensions.LinkUnobtrusiveMobile(string.Concat(nodeToCalcURI.URIId.ToString(), "editlinkedviews2"), 
                "#", "JSLink", AppHelper.GetResource("LINKEDVIEWS_EDIT"), sContentURIPattern,
                DataHelpers.GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                sCalcParams, "button", "true", "true", "forward", "left")
                .WriteTo(writer, HtmlEncoder.Default);
        }

        private static int GetCols(string nodeNeededName)
        {
            int iCols = 8;
            if (nodeNeededName.EndsWith(DataAppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                iCols = 5;
            }
            else if (nodeNeededName.EndsWith(DataAppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
            {
                iCols = 9;
            }
            return iCols;
        }
        private static int GetTypeId(string nodeNeededName)
        {
            //all categories search
            int iTypeId = 0;
            return iTypeId;
        }

        public static bool MakeViewSelections(StringWriter writer,
            string selectionBoxName, string viewEditType, ContentURI docToCalcURI,
            bool isAddInSelections)
        {
            bool bHasAtLeastOneSelection = false;
            string sMethod = string.Empty;
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
            HtmlExtensions.SelectStart(eViewEditType, selectionBoxName, selectionBoxName,
                string.Empty).WriteTo(writer, HtmlEncoder.Default);
            bool bIsSelected = false;
            if (docToCalcURI.URIDataManager.LinkedView != null)
            {
                string sTrimmedOptionName = string.Empty;
                int iNameLength = 25;
                foreach (var linkedviewparent in docToCalcURI.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        bIsSelected = false;
                        int iTrimLength = (linkedview.URIName.Length >= iNameLength) ?
                            iNameLength : linkedview.URIName.Length;
                        sTrimmedOptionName = linkedview.URIName.Substring(0, iTrimLength);
                        sTrimmedOptionName = MakeUniformStringLength(sTrimmedOptionName, iNameLength);
                        if (isAddInSelections == false)
                        {
                            if (linkedview.URIDataManager.IsSelectedLinkedView == true)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                if (linkedview.URIDataManager.IsSelectedLinkedAddIn == true
                                    && bIsSelected == false)
                                {
                                    bIsSelected = true;
                                }
                            }
                        }
                        else
                        {
                            if (linkedview.URIDataManager.IsSelectedLinkedAddIn == true)
                            {
                                bIsSelected = true;
                            }
                        }
                        bool bIsAddIn = false;
                        if (DataHelpers.AddInHelper.IsAddIn(linkedview))
                        {
                            bIsAddIn = true;
                        }
                        if (isAddInSelections)
                        {
                            if (bIsAddIn)
                            {
                                HtmlExtensions.Option(sTrimmedOptionName,
                                    linkedview.URIPattern, bIsSelected)
                                    .WriteTo(writer, HtmlEncoder.Default);
                                bHasAtLeastOneSelection = true;
                            }
                        }
                        else
                        {
                            if (bIsAddIn == false)
                            {
                                HtmlExtensions.Option(sTrimmedOptionName,
                                    linkedview.URIPattern, bIsSelected)
                                    .WriteTo(writer, HtmlEncoder.Default);
                                bHasAtLeastOneSelection = true;
                            }
                        }
                    }
                }
            }
            if (bHasAtLeastOneSelection == false)
            {
                if (isAddInSelections == true)
                {
                    HtmlExtensions.Option(AppHelper.GetResource("LINKEDVIEW_OPEN"),
                       "0", true).WriteTo(writer, HtmlEncoder.Default);
                }
                else
                {
                    HtmlExtensions.Option(AppHelper.GetResource("LINKEDVIEWS_NOTAVAILABLE"),
                       "0", true).WriteTo(writer, HtmlEncoder.Default);
                }
            }
            else
            {
                HtmlExtensions.LabelRegular(string.Empty, AppHelper.GetResource("LINKEDVIEWS_NONE"))
                    .WriteTo(writer, HtmlEncoder.Default);
            }
            HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
            return bHasAtLeastOneSelection;
        }
        public static string MakeUniformStringLength(string optionName, int nLength)
        {
            string sOptionName = optionName;
            int iNeededLength = nLength - optionName.Length;
            if (iNeededLength > 1)
            {
                for (int i = 0; i < iNeededLength; i++)
                {
                    sOptionName += "-";
                }
            }
            return sOptionName;
        }
        public string WriteSelectListsForLocals(
            string resourcesURIArrays, string linkedViewURIPattern,
            string serverSubActionType, string viewEditType,
            string realRate, string nominalRate, string unitGroupId,
            string currencyGroupId, string realRateId, string nominalRateId,
            string dataSourceId, string geoCodeId, string isPrice)
        {
            string sXhtml = string.Empty;
            //write calculator selects lists for locals shared by all calculators
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                //units
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "unitgroup",
                    unitGroupId, serverSubActionType, viewEditType,
                    "lstUnitGroups", "Unit", ";UnitGroupId;integer;4",
                    writer);
                //currencies
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "currencygroup",
                    currencyGroupId, serverSubActionType, viewEditType,
                    "lstCurrencyGroups", "Currency", ";CurrencyGroupId;integer;4",
                    writer);
                //real rates
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "realrate",
                    realRateId, serverSubActionType, viewEditType,
                    "lstRealRates", "Real Rates", ";RealRateId;integer;4",
                    writer);
                //nominal rates
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "nominalrate",
                    nominalRateId, serverSubActionType, viewEditType,
                    "lstNominalRates", "Nominal Rates", ";NominalRateId;integer;4",
                    writer);
                bool bIsPrice
                    = DataHelpers.GeneralHelpers.ConvertStringToBool(isPrice);
                if (bIsPrice)
                {
                    //data source prices
                    WriteSelectListForNewView(resourcesURIArrays,
                        linkedViewURIPattern, "datasourceprice",
                        dataSourceId, serverSubActionType, viewEditType,
                        "lstDataSourcePrices", "Data Source Price", ";DataSourcePriceId;integer;4",
                        writer);
                    //geocodes prices
                    WriteSelectListForNewView(resourcesURIArrays,
                        linkedViewURIPattern, "geocodeprice",
                        geoCodeId, serverSubActionType, viewEditType,
                        "lstGeoCodePrices", "GeoCode Price", ";GeoCodePriceId;integer;4",
                        writer);
                }
                else
                {
                    //data source tech
                    WriteSelectListForNewView(resourcesURIArrays,
                        linkedViewURIPattern, "datasourcetech",
                        dataSourceId, serverSubActionType, viewEditType,
                        "lstDataSourceTechs", "Data Source Tech", ";DataSourceTechId;integer;4",
                        writer);
                    //geocodes tech
                    WriteSelectListForNewView(resourcesURIArrays,
                        linkedViewURIPattern, "geocodetech",
                        geoCodeId, serverSubActionType, viewEditType,
                        "lstGeoCodeTechs", "GeoCode Tech", ";GeoCodeTechId;integer;4",
                        writer);
                }
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        
        public string WriteSelectListsForLocals(
            string resourcesURIArrays, string linkedViewURIPattern,
            string serverSubActionType, string viewEditType,
            string realRate, string nominalRate, string unitGroupId,
            string currencyGroupId, string realRateId, string nominalRateId,
            string ratingGroupId)
        {
            string sXhtml = string.Empty;
            //write calculator selects lists for locals shared by all calculators
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                //units
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "unitgroup",
                    unitGroupId, serverSubActionType, viewEditType,
                    "lstUnitGroups", "Unit", ";UnitGroupId;integer;4",
                    writer);
                //currencies
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "currencygroup",
                    currencyGroupId, serverSubActionType, viewEditType,
                    "lstCurrencyGroups", "Currency", ";CurrencyGroupId;integer;4",
                    writer);
                //real rates
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "realrate",
                    realRateId, serverSubActionType, viewEditType,
                    "lstRealRates", "Real Rates", ";RealRateId;integer;4",
                    writer);
                //nominal rates
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "nominalrate",
                    nominalRateId, serverSubActionType, viewEditType,
                    "lstNominalRates", "Nominal Rates", ";NominalRateId;integer;4",
                    writer);
                //ratings
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, "ratinggroup",
                    ratingGroupId, serverSubActionType, viewEditType,
                    "lstRatingGroups", "Rating Groups", ";RatingGroupId;integer;4",
                    writer);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }

        public string WriteSelectListForNewView(
            string resourcesURIArrays, string linkedViewURIPattern, string nodeSelectsName, string nodeSelectedId,
           string serverSubActionType, string viewEditType,
           string selectId, string label, string selectListAttributeParams)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                WriteSelectListForNewView(resourcesURIArrays,
                    linkedViewURIPattern, nodeSelectsName, nodeSelectedId,
                    serverSubActionType, viewEditType,
                    selectId, label, selectListAttributeParams,
                    writer);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public string WriteSelectListForNetwork(
            ContentURI uri, string selectedFileURIPattern,
            string fullFilePath, string serviceId, string networkId, string viewEditType)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                WriteSelectListForNetwork(uri, selectedFileURIPattern, fullFilePath,
                    serviceId, networkId, viewEditType, writer);
                sXhtml = writer.ToString();
            }

            return sXhtml;
        }
        private void WriteSelectListForNetwork(ContentURI uri, string nodeURIPattern,
            string fullFilePath, string serviceId, string networkId,
            string viewEditType, StringWriter writer)
        {
            string sNetworkDocPath = GetOwningClubNetworkDocPath(uri, fullFilePath);
            ContentURI networkURI = ContentURI.ConvertShortURIPattern(nodeURIPattern);
            string sURIPath = DataHelpers.FileStorageIO.GetResourceURIPath(networkURI,
                    sNetworkDocPath);
            if (string.IsNullOrEmpty(sURIPath))
            {
                writer.Write(DevTreks.Exceptions.DevTreksErrors
                    .MakeStandardErrorMsg(string.Empty, "NETWORKS_NOTSELECTED"));
            }
            else
            {
                //write the selects list
                MakeNetworkSelects(uri, writer, nodeURIPattern,
                    viewEditType, serviceId, sURIPath,
                    "network", networkId, ";NetworkId;int;4");
            }
        }
        public static string GetOwningClubNetworkDocPath(ContentURI uri,
            string agreementFilePath)
        {
            string sFullNetworkDocPath = string.Empty;
            if (!agreementFilePath.EndsWith(string.Concat(
                DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString(),
                DataHelpers.GeneralHelpers.EXTENSION_XML)))
            {
                //2.0.0 changes
                string sAgreementFilePath = System.Net.WebUtility.UrlEncode(agreementFilePath);
                //string sAgreementFilePath = HttpUtility.UrlDecode(agreementFilePath);
                string sRootFolderPath
                    = AppHelper.GetAdminFolderPath(sAgreementFilePath,
                    DataHelpers.GeneralHelpers.APPLICATION_TYPES.agreements,
                    DataHelpers.GeneralHelpers.APPLICATION_TYPES.networks);
                string sErrorMsg = string.Empty;
                string sFileExtensionType = DataHelpers.GeneralHelpers.EXTENSION_XML;
                IDictionary<string, string> lstFilePaths = new Dictionary<string, string>();
                DataHelpers.XmlFileIO.AddNewestXmlFileWithExtensionToList(
                    uri, sRootFolderPath,
                    sFileExtensionType, lstFilePaths, ref sErrorMsg);
                if (string.IsNullOrEmpty(sErrorMsg))
                {
                    if (lstFilePaths.Count > 0)
                    {
                        //each admin app has only have one xml document
                        KeyValuePair<string, string> kvp
                            = lstFilePaths.ElementAtOrDefault(0);
                        sFullNetworkDocPath = kvp.Value;
                    }
                }
                if (!string.IsNullOrEmpty(sErrorMsg))
                {
                    sErrorMsg = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(string.Empty,
                        "NETWORKS_NOTSELECTED");
                    //the stylesheet looks for a starting "Error" string before display
                    sFullNetworkDocPath = sErrorMsg;
                }
            }
            return sFullNetworkDocPath;
        }
        private static void MakeNetworkSelects(
            ContentURI uri, StringWriter writer,
            string nodeURIPattern, string viewEditType, string selectId,
            string docPath, string nodeSelectsName, string nodeSelectedId,
            string attributeParams)
        {
            string sName = string.Concat(nodeURIPattern, attributeParams);
            ContentURI networkURI = ContentURI.ConvertShortURIPattern(nodeURIPattern);
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
            HtmlExtensions.SelectStart(eViewEditType, string.Concat("SelectNetwork", networkURI.URIId),
                sName, string.Empty).WriteTo(writer, HtmlEncoder.Default);
            string sURIPath = DataHelpers.FileStorageIO.GetResourceURIPath(networkURI,
                    docPath);
            if (!string.IsNullOrEmpty(sURIPath))
            {
                HtmlExtensions.Option(AppHelper.GetResource("NETWORKS_SELECTONE"),
                 "-1", true).WriteTo(writer, HtmlEncoder.Default);
                XPathDocument oDoc = null;
                XmlReader reader = DataHelpers.FileStorageIO.GetXmlReader(uri, sURIPath);
                if (reader != null)
                {
                    using (reader)
                    {
                        oDoc = new XPathDocument(reader);
                    }
                    XPathNavigator navDoc = oDoc.CreateNavigator();
                    // return "//nodename"
                    string sQry
                        = EditHelpers.XmlIO.MakeXPathAbbreviatedQry(nodeSelectsName,
                        string.Empty, string.Empty);
                    XPathNodeIterator itrDoc = navDoc.Select(sQry);
                    XPathNavigator navCurrentNode = null;
                    string sNetworkId = string.Empty;
                    sName = string.Empty;
                    bool bIsSelected = false;
                    while (itrDoc.MoveNext())
                    {
                        bIsSelected = false;
                        navCurrentNode = itrDoc.Current;
                        sNetworkId = EditHelpers.XPathIO.GetAttributeValue(
                            navCurrentNode, DataAppHelpers.Networks.NETWORK_ID);
                        sName = EditHelpers.XPathIO.GetAttributeValue(
                            navCurrentNode, DataAppHelpers.Calculator.cName);
                        if (sNetworkId.Equals(nodeSelectedId))
                        {
                            bIsSelected = true;
                        }
                        HtmlExtensions.Option(sName, sNetworkId, bIsSelected)
                            .WriteTo(writer, HtmlEncoder.Default);
                    }
                    if (itrDoc.CurrentPosition == 0)
                    {
                        HtmlExtensions.Option(AppHelper.GetResource("SELECTEDVIEWS_WRONGDOC"),
                            "0", true).WriteTo(writer, HtmlEncoder.Default);
                    }
                }
            }
            else
            {
                HtmlExtensions.Option(AppHelper.GetResource("SELECTEDVIEWS_NODOC"),
                  "0", true).WriteTo(writer, HtmlEncoder.Default);
            }
            HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
        }

        public string WriteSelectListsForPrices(
            string unitGroupId, string linkedViewURIPattern,
            string serverSubActionType, string viewEditType,
            string priceGas, string priceDiesel, string priceLP, string priceNG,
            string priceElectric, string priceOil, string priceRegularLabor,
            string priceMachineryLabor, string priceSupervisorLabor,
            string taxPercent, string insurePercent, string housingPercent)
        {
            string sXhtml = string.Empty;
            string sUnit = string.Empty;
            decimal dcMultiplier = 0;
            string sSelectListLabel = string.Empty;
            //write calculator selects lists for prices and rates shared by many calculators
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                //price gas
                sUnit = (unitGroupId == "1") ? "liter" : "gallon";
                dcMultiplier = Convert.ToDecimal(0.50);
                sSelectListLabel = string.Concat("Price Gas ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceGas, serverSubActionType, viewEditType,
                    "lstPriceGas", sSelectListLabel, ";PriceGas;double;8",
                    dcMultiplier, writer);
                //price diesel
                sSelectListLabel = string.Concat("Price Diesel ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceDiesel, serverSubActionType, viewEditType,
                    "lstPriceDiesel", sSelectListLabel, ";PriceDiesel;double;8",
                    dcMultiplier, writer);
                //price oil
                sSelectListLabel = string.Concat("Price Oil ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceOil, serverSubActionType, viewEditType,
                    "lstPriceOil", sSelectListLabel, ";PriceOil;double;8",
                    dcMultiplier, writer);
                //price lp
                sSelectListLabel = string.Concat("Price LP ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceLP, serverSubActionType, viewEditType,
                    "lstPriceLP", sSelectListLabel, ";PriceLP;double;8",
                    dcMultiplier, writer);
                //price natural gas
                sUnit = (unitGroupId == "1") ? "m3" : "mcf";
                sSelectListLabel = string.Concat("Price NG ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceNG, serverSubActionType, viewEditType,
                    "lstPriceNG", sSelectListLabel, ";PriceNG;double;8",
                    dcMultiplier, writer);
                //price electric
                sUnit = (unitGroupId == "1") ? "kwh" : "kwh";
                dcMultiplier = Convert.ToDecimal(0.01);
                sSelectListLabel = string.Concat("Price Electric ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceElectric, serverSubActionType, viewEditType,
                    "lstPriceElectric", sSelectListLabel, ";PriceElectric;double;8",
                    dcMultiplier, writer);
                //price regular labor
                sUnit = "hour";
                dcMultiplier = Convert.ToDecimal(1.00);
                sSelectListLabel = string.Concat("Price Regular Labor ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceRegularLabor, serverSubActionType, viewEditType,
                    "lstPriceRegularLabor", sSelectListLabel, ";PriceRegularLabor;double;8",
                    dcMultiplier, writer);
                //price machinery labor
                sSelectListLabel = string.Concat("Price Machinery Labor ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceMachineryLabor, serverSubActionType, viewEditType,
                    "lstPriceMachineryLabor", sSelectListLabel, ";PriceMachineryLabor;double;8",
                    dcMultiplier, writer);
                //price supervisory labor
                sSelectListLabel = string.Concat("Price Supervisor Labor ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    priceSupervisorLabor, serverSubActionType, viewEditType,
                    "lstPriceSupervisorLabor", sSelectListLabel, ";PriceSupervisorLabor;double;8",
                    dcMultiplier, writer);
                //tax percent
                sUnit = "1000 (7.5% per $1000)";
                dcMultiplier = Convert.ToDecimal(0.50);
                sSelectListLabel = string.Concat("Tax Percent ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    taxPercent, serverSubActionType, viewEditType,
                    "lstTaxPercent", sSelectListLabel, ";TaxPercent;double;8",
                    dcMultiplier, writer);
                //insure percent
                sSelectListLabel = string.Concat("Insure Percent ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    insurePercent, serverSubActionType, viewEditType,
                    "lstInsurePercent", sSelectListLabel, ";InsurePercent;double;8",
                    dcMultiplier, writer);
                //housing percent
                sSelectListLabel = string.Concat("Housing Percent ", "(", sUnit, ")");
                WriteSelectListForPriceView(linkedViewURIPattern,
                    housingPercent, serverSubActionType, viewEditType,
                    "lstHousingPercent", sSelectListLabel, ";HousingPercent;double;8",
                    dcMultiplier, writer);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }

        public static string WriteSelectListForTypes(string groupUriPattern,
            string typeDocPath, string serviceBaseId, string selectedTypeId,
            string typeAttributeName, string viewEditType)
        {
            string sXhtml = string.Empty;
            sXhtml = selectedTypeId;
            return sXhtml;

        }
        public static string WriteSelectListForTypes(string groupUriPattern,
            string serviceBaseId, string selectedTypeId,
            string typeAttributeName, string viewEditType)
        {
            string sXhtml = string.Empty;
            sXhtml = selectedTypeId;
            return sXhtml;

        }
        private static void WriteCategorySelectList(string uriPattern, string uriId,
            IList<ContentURI> categories, string serviceBaseId, string selectedTypeId,
            string typeAttributeName, string viewEditType, ref string xhtml)
        {
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                int iSelectedTypeId
                    = DataHelpers.GeneralHelpers.ConvertStringToInt(selectedTypeId);
                WriteSelectListForType(uriPattern, uriId, categories,
                    iSelectedTypeId, typeAttributeName, viewEditType, writer);
                xhtml = writer.ToString();
            }
        }
        public static void WriteSelectListForType(string uriPattern, string uriId,
            IList<ContentURI> categories, int selectedTypeId, string typeAttributeName,
            string viewEditType, StringWriter writer)
        {
            string sCssClass = string.Empty;// "Select250";
            bool bIsSelected = (selectedTypeId == 0) ? true : false;
            if (viewEditType != DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
            {
                foreach (ContentURI category in categories)
                {
                    if (category.URIId == selectedTypeId)
                    {
                        bIsSelected = true;
                    }
                    else
                    {
                        bIsSelected = false;
                    }
                    if (bIsSelected)
                    {
                        HtmlExtensions.LabelRegular(string.Empty, category.URIName)
                            .WriteTo(writer, HtmlEncoder.Default);
                    }
                }
            }
            else
            {
                string sJavascriptMethod = (selectedTypeId == 0) ? string.Empty : string.Empty;
                string sFormElementName = EditHelpers.EditHelper.MakeStandardEditName(
                    uriPattern, typeAttributeName, RuleHelpers.GeneralRules.INTEGER, "4");
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                    = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
                HtmlExtensions.SelectStart(eViewEditType, string.Concat(typeAttributeName, uriId),
                   sFormElementName, sCssClass).WriteTo(writer, HtmlEncoder.Default);
                bIsSelected = false;
                foreach (ContentURI category in categories)
                {
                    if (category.URIId == selectedTypeId)
                    {
                        bIsSelected = true;
                    }
                    else
                    {
                        bIsSelected = false;
                    }
                    HtmlExtensions.Option(category.URIName,
                        category.URIId.ToString(), bIsSelected).WriteTo(writer, HtmlEncoder.Default);
                }
                HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
            }
        }
        public void WriteSelectListForNewView(string resourcesURIArrays,
            string linkedViewURIPattern, string nodeSelectsName, string nodeSelectedId,
            string serverSubActionType, string viewEditType,
            string selectId, string label, string selectListAttributeParams,
            StringWriter writer)
        {
            //each resource list in the resourcesURIArrays is found by using the 
            //the nodeSelectsName param which must correspond to the db key, Resource.TagName
            //that tag name is added as a uri.urifileextensiontype to the uripatterns
            //in resourceURIArrays
            //REST uri is attribute Resource.LOCALS_RESOURCEPACK_URINAME
            //corresponding file path attribute is Resource.LOCALS_RESOURCEPACK_DOCPATH
            string sResourceArray
                = DataAppHelpers.Resources.GetResourceArrayFromResourceArraysByTagName(
                resourcesURIArrays, nodeSelectsName);
            string sFullDocPath = GetResourceParam("0", sResourceArray);
            string sResourceURIPattern = GetResourceParam("2", sResourceArray);
            //2.0.0 addition for appsetting connection strings
            //not a security threat because this is run dynamically -connection strings are not stored statefully
            ContentURI resourceURI = ContentURI.ConvertShortURIPattern(sResourceURIPattern);
            //2.0.4 addition
            resourceURI.URIDataManager.PlatformType = DataHelpers.FileStorageIO.GetPlatformType(sFullDocPath);
            SetConnections(resourceURI, resourcesURIArrays);
            string sURIPath = sFullDocPath;
            //2.0.0 deprecated: path comes originally from this, doesn't need 2 hits to storage.
            //string sURIPath = DataHelpers.FileStorageIO.GetResourceURIPath(resourceURI, sFullDocPath);
            if (string.IsNullOrEmpty(sURIPath))
            {
                writer.Write(DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(label,
                    "STYLEHELPER_NOLINKEDLIST2"));
            }
            else
            {
                //write the selects list
                WriteSelectListForView(resourceURI, sURIPath,
                    linkedViewURIPattern, nodeSelectsName, nodeSelectedId,
                    serverSubActionType, viewEditType,
                    selectId, label, selectListAttributeParams,
                    writer);
            }
        }
        private void SetConnections(ContentURI resourceURI, string resourceURLs)
        {
            string sDefaultConnection = string.Empty;
            string sStorageConnection = string.Empty;
            //2.0.0 added connections using Data.AppHelpers.Resources.AddConnections()
            string[] arrResources = resourceURLs.Split(DataHelpers.GeneralHelpers.PARAMETER_DELIMITERS);
            int iEndPosition = arrResources.Count() - 2;
            if (iEndPosition > 1)
            {
                sDefaultConnection = arrResources[iEndPosition];
                sDefaultConnection = sDefaultConnection.Replace(
                    DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER2, DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                iEndPosition = arrResources.Count() - 1;
                sStorageConnection = arrResources[iEndPosition];
                sStorageConnection = sStorageConnection.Replace(
                    DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER2, DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
            }
            resourceURI.URIDataManager.DefaultConnection = sDefaultConnection;
            resourceURI.URIDataManager.StorageConnection = sStorageConnection;
        }
        public string WriteCalculateButtons(string uriPattern,
           string contenturipattern, string calcParams)
        {
            string sXhtml = string.Empty;
            bool bIsAnalysis = false;
            sXhtml = WriteCalcButtons(bIsAnalysis, uriPattern, contenturipattern, calcParams);
            return sXhtml;
        }

        private string WriteCalcButtons(bool isAnalysis,
            string uriPattern, string contenturipattern, string calcParams)
        {
            string sXhtml = string.Empty;
            string sContentURIPatternForClose = string.Empty;
            string sContentURIPatternForCalculate = string.Empty;
            GetCalculatorCommands(uriPattern, contenturipattern,
                ref sContentURIPatternForClose, ref sContentURIPatternForCalculate);
            using (StringWriter writer = new StringWriter())
            {
                HtmlHelperExtensions.MakeCalculateButtons(calcParams,
                    isAnalysis, sContentURIPatternForClose, sContentURIPatternForCalculate)
                    .WriteTo(writer, HtmlEncoder.Default); 
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }

        private void GetCalculatorCommands(string uriPattern,
            string contenturipattern, ref string cpForClose, ref string cpForCalculate)
        {
            string sController = ContentURI.GetContentURIPatternPart(contenturipattern,
                ContentURI.CONTENTURIPATTERNPART.controller);
            cpForClose = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                sController,
                DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                uriPattern,
                DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.closeedits.ToString(),
                DataHelpers.GeneralHelpers.NONE,
                DataHelpers.GeneralHelpers.NONE);
            string sServerSubActionView = GetStylesheetSubActionView(contenturipattern,
               string.Empty, DataHelpers.GeneralHelpers.NONE);
            cpForCalculate = DataHelpers.GeneralHelpers.MakeContentURIPattern(
               sController,
               DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
               uriPattern,
               DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString(),
               sServerSubActionView,
               DataHelpers.GeneralHelpers.NONE);
        }
        public void WriteSelectListForView(ContentURI resourceURI, 
            string fullDocPath, string linkedViewURIPattern, string nodeSelectsName,
            string nodeSelectedId, string serverSubActionType,
            string viewEditType, string selectId, string label,
            string selectListAttributeParams, StringWriter writer)
        {
            //write the selects list
            HtmlExtensions.LabelRegular(selectId, label)
                .WriteTo(writer, HtmlEncoder.Default);
            MakeNewViewSelects(writer, resourceURI, linkedViewURIPattern,
                viewEditType, selectId, fullDocPath,
                nodeSelectsName, nodeSelectedId, selectListAttributeParams);
        }
        
        private static void MakeNewViewSelects(StringWriter writer, ContentURI resourceURI,
            string linkedViewURIPattern, string viewEditType, string selectId,
            string docPath, string nodeSelectsName, string nodeSelectedId,
            string attributeParams)
        {
            string sName = string.Concat(linkedViewURIPattern, attributeParams);
            //2.0.0 refactor includes connection strings in resourcearrays 
            ContentURI linkedViewURI = ContentURI.ConvertShortURIPattern(linkedViewURIPattern);
            linkedViewURI.URIDataManager.DefaultConnection = resourceURI.URIDataManager.DefaultConnection;
            linkedViewURI.URIDataManager.StorageConnection = resourceURI.URIDataManager.StorageConnection;
            //2.0.4 added PlatformType because it's missing
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
            HtmlExtensions.SelectStart(eViewEditType, string.Concat("SelectNewView", linkedViewURI.URIId),
                sName, string.Empty).WriteTo(writer, HtmlEncoder.Default);
            string sURIPath = docPath;
            if (!string.IsNullOrEmpty(sURIPath))
            {
                XPathDocument oDoc = null;
                XmlReader reader = DataHelpers.FileStorageIO.GetXmlReader(linkedViewURI, sURIPath);
                if (reader != null)
                {
                    using (reader)
                    {
                        oDoc = new XPathDocument(reader);
                    }
                    XPathNavigator navDoc = oDoc.CreateNavigator();
                    // return "//nodename"
                    string sQry
                        = EditHelpers.XmlIO.MakeXPathAbbreviatedQry(nodeSelectsName,
                        string.Empty, string.Empty);
                    XPathNodeIterator itrDoc = navDoc.Select(sQry);
                    XPathNavigator navCurrentNode = null;
                    string sId = string.Empty;
                    bool bIsSelected = false;
                    while (itrDoc.MoveNext())
                    {
                        bIsSelected = false;
                        navCurrentNode = itrDoc.Current;
                        sId = EditHelpers.XPathIO.GetAttributeValue(
                            navCurrentNode, DataAppHelpers.Calculator.cId);
                        sName = EditHelpers.XPathIO.GetAttributeValue(
                            navCurrentNode, DataAppHelpers.Calculator.cName);
                        if (sId.Equals(nodeSelectedId))
                        {
                            bIsSelected = true;
                        }
                        HtmlExtensions.Option(sName, sId, bIsSelected).WriteTo(writer, HtmlEncoder.Default);
                    }
                    if (itrDoc.CurrentPosition == 0)
                    {
                        HtmlExtensions.Option(AppHelper.GetResource("SELECTEDVIEWS_WRONGDOC"),
                            "0", true).WriteTo(writer, HtmlEncoder.Default);
                    }
                }
            }
            else
            {
                HtmlExtensions.Option(AppHelper.GetResource("SELECTEDVIEWS_NODOC"),
                  "0", true).WriteTo(writer, HtmlEncoder.Default);
            }
            HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
        }
        public void WriteSelectListForPriceView(string linkedViewURIPattern,
            string nodeSelectedValue, string serverSubActionType, string viewEditType,
            string selectId, string label, string selectListAttributeParams,
            decimal multiplier, StringWriter writer)
        {
            //write the selects list
            HtmlExtensions.LabelRegular(selectId, label).WriteTo(writer, HtmlEncoder.Default);
            MakeNewPriceViewSelects(writer, linkedViewURIPattern,
                viewEditType, selectId, nodeSelectedValue,
                multiplier, selectListAttributeParams);
        }

        private static void MakeNewPriceViewSelects(StringWriter writer,
            string linkedViewURIPattern, string viewEditType, string selectId,
            string nodeSelectedValue, decimal multiplier, string attributeParams)
        {
            string sName = string.Concat(linkedViewURIPattern, attributeParams);
            string sLVId = ContentURI.GetContentURIPatternPart(linkedViewURIPattern, ContentURI.CONTENTURIPATTERNPART.id);
            DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
            HtmlExtensions.SelectStart(eViewEditType, string.Concat("SelectNewPriceView", sLVId),
                sName, string.Empty).WriteTo(writer, HtmlEncoder.Default);
            int i = 0;
            decimal dcIterationValue = 0;
            string sIterationValue = string.Empty;
            bool bIsSelected = false;
            //compare selection using 2 digit decimal strings
            decimal dcExistingValue = DataHelpers.GeneralHelpers
                .ConvertStringToDecimal(nodeSelectedValue);
            if (dcExistingValue < 1 && attributeParams.Contains("Percent"))
            {
                //the percent lists include one selection (0.5 percent) that is less than 1
                //this finds it when the selection is first made (before the calculations are run)
                double dbExistingValue = DataHelpers.GeneralHelpers
                    .ConvertStringToDouble(nodeSelectedValue);
                if (dbExistingValue == 0.5)
                {
                    //don't convert it yet
                }
                else
                {
                    //calculators change percentages to rates (.001)
                    //note that these are 7.5% per $1000 so have two extra digits in multiplier (1.5 = .0015)
                    double dbRate = DataHelpers.GeneralHelpers
                        .ConvertStringToDouble(nodeSelectedValue) * 1000;
                    //change back to display parameter
                    dcExistingValue = DataHelpers.GeneralHelpers
                        .ConvertStringToDecimal(dbRate.ToString());
                }
            }
            string sExistingValue = dcExistingValue.ToString("f2");
            for (i = 0; i < 25; i++)
            {
                dcIterationValue = i * multiplier;
                //prices and rates should be limited to two decimal places
                sIterationValue = dcIterationValue.ToString("f2");
                bIsSelected = false;
                if (sIterationValue.Equals(sExistingValue))
                {
                    bIsSelected = true;
                }
                HtmlExtensions.Option(sIterationValue,
                    sIterationValue, bIsSelected).WriteTo(writer, HtmlEncoder.Default);
            }
            HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
        }
        public static string WriteOpenInPanelLink(string selectedFileURIPattern,
            string calcParams, string serverActionType, string contenturipattern)
        {
            string sXhtml = string.Empty;
            if (!string.IsNullOrEmpty(selectedFileURIPattern))
            {
                string sOpenInPanelMsg = string.Empty;
                string sController = ContentURI.GetContentURIPatternPart(contenturipattern,
                    ContentURI.CONTENTURIPATTERNPART.controller);
                DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES eServerActionType
                    = DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit;
                if (serverActionType == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString())
                {
                    sOpenInPanelMsg = AppHelper.GetResource("STYLESHEET_OPENLINKEDVIEWS");
                    eServerActionType = DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews;
                }
                else if (serverActionType == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString())
                {
                    sOpenInPanelMsg = AppHelper.GetResource("STYLESHEET_OPENEDITS");
                    eServerActionType = DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit;
                }
                else
                {
                    sOpenInPanelMsg = string.Empty;
                }
                if (sOpenInPanelMsg != string.Empty)
                {
                    string sURIId = ContentURI.GetURIPatternPart(
                        selectedFileURIPattern, ContentURI.URIPATTERNPART.id);
                    string sURINodeName = ContentURI.GetURIPatternPart(
                        selectedFileURIPattern, ContentURI.URIPATTERNPART.node);
                    string sURIFileExtType = ContentURI.GetURIPatternPart(
                        selectedFileURIPattern, ContentURI.URIPATTERNPART.fileExtension);
                    //refactored: 0.8.5 started using respondwithnewxhtml serversubaction as clue
                    //to use linkedviewsuri in uri.linkedviews (i.e. for linkedviews, devpacks, and 
                    //stories linked to other apps)
                    //write to the txtWriter
                    using (StringWriter writer = new StringWriter())
                    {
                        string sContentURIPattern = string.Empty;
                        if (sURINodeName.StartsWith(DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                            || sURINodeName.StartsWith(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            || sURIFileExtType == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                        {
                            calcParams = calcParams.Replace("'", string.Empty);
                            sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                                sController, eServerActionType.ToString(),
                                selectedFileURIPattern,
                                DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml.ToString(),
                                DataHelpers.GeneralHelpers.NONE,
                                DataHelpers.GeneralHelpers.NONE);
                        }
                        else
                        {
                            //SERVER_SUBACTION_TYPES.respondwithnewxhtml so that new calcs can be displayed
                            sContentURIPattern = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                                sController, eServerActionType.ToString(),
                                selectedFileURIPattern,
                                DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml.ToString(),
                                DataHelpers.GeneralHelpers.NONE,
                                DataHelpers.GeneralHelpers.NONE);
                        }
                        HtmlExtensions.LinkUnobtrusiveMobile(string.Concat(serverActionType, sURIId),
                            "#", "JSLink", sOpenInPanelMsg, sContentURIPattern,
                            DataHelpers.GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            calcParams, "button", "true", "true", "forward", "left")
                            .WriteTo(writer, HtmlEncoder.Default);
                        sXhtml = writer.ToString();
                    }
                }
            }
            return sXhtml;
        }

        public static string GetResourceParentURI(string selectedFileURIPattern,
            string calcParams, string calcParamName)
        {
            //called from linkedviews to determine the resources' (i.e. image) parent uri
            string sResourceParentURI = string.Empty;
            sResourceParentURI = DataHelpers.GeneralHelpers.GetFormElementParam(
                calcParams, calcParamName, selectedFileURIPattern);
            return sResourceParentURI;
        }

        public static string WriteFormattedNumber(string attName,
            string attValue, string dataType, string size)
        {
            DevTreks.Data.RuleHelpers.GeneralRules.ValidateXSDInput(attName,
                ref attValue, dataType, size);
            return attValue;
        }
        public static string GetURIPattern(string name,
            string id, string networkid, string nodeName, string fileExtType)
        {
            string sURIPattern = string.Empty;
            sURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(name, id,
                networkid, nodeName, fileExtType);
            return sURIPattern;
        }
        public static string GetImagesUrl(string resourceFileName)
        {
            //2.0.0 the tilde doesn't work
            string sResourcePath = string.Format("{0}/{1}",
                "/images", resourceFileName);
            //string sResourcePath = string.Format("{0}/{1}",
            //    "~/images", resourceFileName);
            return sResourcePath;
        }
        public static string GetImagesUrl(string webDomain, string resourceFileName)
        {
            string sResourcePath = string.Format("{0}{1}/{2}",
                webDomain, "images", resourceFileName);
            return sResourcePath;
        }
        public static string MakeDevTreksLink(string id, string href,
            string classAttribute, string text, string contenturipattern,
            string newURIPattern, string clientAction, string serverAction,
            string serverSubAction, string serverSubActionView,
            string extraParams)
        {
            string sDevTreksLink = string.Empty;
            if (string.IsNullOrEmpty(contenturipattern))
                return string.Empty;
            string sContentURIPattern = GetContentURIPattern(contenturipattern,
                newURIPattern, serverAction, serverSubAction, serverSubActionView);
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlExtensions.LinkUnobtrusiveMobile(id,
                    href, classAttribute, text, sContentURIPattern,
                    clientAction, extraParams, "button", "true", "true", 
                    string.Empty, string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                sDevTreksLink = writer.ToString();
            }
            return sDevTreksLink;
        }
        public static string MakeDevTreksButton(string id,
            string classAttribute, string text, string contenturipattern,
            string newURIPattern, string clientAction, string serverAction,
            string serverSubAction, string serverSubActionView,
            string extraParams)
        {
            string sDevTreksButton = string.Empty;
            if (string.IsNullOrEmpty(contenturipattern))
                return string.Empty;
            serverSubActionView = GetStylesheetSubActionView(contenturipattern,
                serverAction, serverSubActionView);
            string sContentURIPattern = GetContentURIPattern(contenturipattern,
                newURIPattern, serverAction, serverSubAction, serverSubActionView);
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlExtensions.InputUnobtrusiveMobile(id,
                    classAttribute, "button", sContentURIPattern, 
                    clientAction, extraParams, text,
                    "true", "true", "plus", "right")
                    .WriteTo(writer, HtmlEncoder.Default);
                sDevTreksButton = writer.ToString();
            }
            return sDevTreksButton;
        }
        private static string GetStylesheetSubActionView(string contenturipattern,
            string serverAction, string subActionView)
        {
            //v1.1.1 stored info on which view to display in subactionviews. Calc stylesheets have 
            //correct subactionview in contenturipattern but not in serverSubActionView
            string sSubActionView = subActionView;
            if (serverAction == string.Empty)
            {
                serverAction = ContentURI.GetContentURIPatternPart(contenturipattern,
                    ContentURI.CONTENTURIPATTERNPART.action);
            }
            if (serverAction == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString())
            {
                string sServerSubActionView = ContentURI.GetContentURIPatternPart(contenturipattern,
                    ContentURI.CONTENTURIPATTERNPART.subactionview);
                if (!string.IsNullOrEmpty(sServerSubActionView)
                    && sServerSubActionView != DataHelpers.GeneralHelpers.NONE)
                {
                    sSubActionView = sServerSubActionView;
                }
            }
            return sSubActionView;
        }
        public static string GetContentURIPattern(string contenturipattern,
            string newuripattern, string serverAction,
            string serverSubAction, string serverSubActionView)
        {
            string sContentURIPattern = string.Empty;
            if (string.IsNullOrEmpty(contenturipattern))
                return string.Empty;
            string sController = string.Empty;
            string sServerAction = string.Empty;
            string sNetworkName = string.Empty;
            string sNodeName = string.Empty;
            string sName = string.Empty;
            string sId = string.Empty;
            string sFileNameExtensionType = string.Empty;
            string sSubAction = string.Empty;
            string sSubActionView = string.Empty;
            string sVariable = string.Empty;
            ContentURI.GetContentURIParams(contenturipattern,
                out sController, out sServerAction,
                out sNetworkName, out sNodeName, out sName,
                out sId, out sFileNameExtensionType, out sSubAction,
                out sSubActionView, out sVariable);
            string sURIPattern = string.Empty;
            if (string.IsNullOrEmpty(newuripattern))
            {
                sURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(sName,
                     sId, sNetworkName, sNodeName, sFileNameExtensionType);
            }
            else
            {
                sURIPattern = ContentURI.GetURIPatternFromContentURIPattern(newuripattern);
                //sURIPattern = newuripattern;
            }
            sContentURIPattern
                = DataHelpers.GeneralHelpers.MakeContentURIPattern(
                controller: sController,
                action: serverAction,
                uriPattern: sURIPattern,
                subActionType: serverSubAction,
                subActionView: serverSubActionView,
                variable: DataHelpers.GeneralHelpers.NONE);
            return sContentURIPattern;
        }
        public static string MakeGetSelectionsLink(string id, string href,
            string classAttribute, string text, string spanId,
            string contenturipattern, string nodeURIPattern, string nodeName,
            string attributeName, string extraParams)
        {
            string sGetSelectionsLink = string.Empty;
            //change the subaction type (the cup goes into a hidden form element, 
            //but is not used to change the anchor action)
            string sContentURIPattern = ContentURI.ChangeContentURIPatternPart(
               contenturipattern, ContentURI.CONTENTURIPATTERNPART.subaction,
               DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects.ToString());
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlExtensions.LinkPlusUnobtrusiveMobile(
                    id, href, classAttribute, text, spanId, sContentURIPattern,
                    nodeURIPattern, nodeName, attributeName, extraParams,
                    "button", "true", "true", "search", "left")
                    .WriteTo(writer, HtmlEncoder.Default);
                sGetSelectionsLink = writer.ToString();
            }
            return sGetSelectionsLink;
        }
        public static string MakeUploadButton(string id,
           string classAttribute, string text, string spanId,
           string contenturipattern, string nodeURIPattern, string nodeName,
           string attributeName, string extraParams)
        {
            string sButton = string.Empty;
            //change the subaction type
            string sContentURIPattern = ContentURI.ChangeContentURIPatternPart(
               contenturipattern, ContentURI.CONTENTURIPATTERNPART.subaction,
                DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.uploadfile.ToString());
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlExtensions.InputPlusUnobtrusiveMobile(
                    classAttribute, "button", id, text, spanId, sContentURIPattern,
                    nodeURIPattern, nodeName, attributeName, extraParams,
                    "true", "true", string.Empty, string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                sButton = writer.ToString();
            }
            return sButton;
        }
        public static string MakeButtonPlusUnobtrusive(string id,
            string classAttribute, string text, string spanId,
            string contenturipattern, string nodeURIPattern, string nodeName,
            string attributeName, string extraParams)
        {
            string sButton = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlExtensions.InputPlusUnobtrusiveMobile(
                    classAttribute, "button", id, text, spanId, contenturipattern,
                    nodeURIPattern, nodeName, attributeName, extraParams, string.Empty, 
                    string.Empty, string.Empty, string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                sButton = writer.ToString();
            }
            return sButton;
        }
        public static string GetURIPatternPart(string uriPattern, string patternPartName)
        {
            string sURIPatternPart = ContentURI.GetURIPatternPart(uriPattern, patternPartName);
            return sURIPatternPart;
        }
        public static string GetSubString(string delimitedString, string delimiter,
            string oneBasedIndexPos)
        {
            string sSubString = DataHelpers.GeneralHelpers.GetSubstringFromFront(
                delimitedString, delimiter,
                DataHelpers.GeneralHelpers.ConvertStringToInt(oneBasedIndexPos));
            return sSubString;
        }
        public static string MakeDataViewFormParamForLinkedView()
        {
            string sListViewParam = string.Empty;
            sListViewParam = DataHelpers.GeneralHelpers.MakeDataViewFormParam(
                DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist,
                DataHelpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString());
            return sListViewParam;
        }

       
        public static string GetCalcParam(string calcParams,
            string calcParamFormElementName)
        {
            string sCalcParamFormElementValue = string.Empty;
            sCalcParamFormElementValue = DataHelpers.GeneralHelpers
                .GetFormElementParam(calcParams, calcParamFormElementName, string.Empty);
            //whatiftagname
            //relatedcalculatorstype
            return sCalcParamFormElementValue;
        }
        public static string WriteMenuSteps(string integer)
        {
            string sXhtml = string.Empty;
            int iStepNumber = GetStepNumber(integer);
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                HtmlExtensions.DivStart(string.Empty, string.Empty, "navbar", string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.ULStart(string.Empty, string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                WriteMenuStep(writer, "stepzero", string.Empty,
                    "Go to introduction", AppHelper.GetResource("CALCS_INTRO"));
                if (iStepNumber >= 1)
                {
                    WriteMenuStep(writer, "stepone", string.Empty,
                        "Go to step one", AppHelper.GetResource("CALCS_STEP1"));
                }
                if (iStepNumber >= 2)
                {
                    WriteMenuStep(writer, "steptwo", string.Empty,
                        "Go to step two", AppHelper.GetResource("CALCS_STEP2"));
                }
                if (iStepNumber >= 3)
                {
                    WriteMenuStep(writer, "stepthree", string.Empty,
                        "Go to step three", AppHelper.GetResource("CALCS_STEP3"));
                }
                if (iStepNumber >= 4)
                {
                    //finish the top navbar
                    HtmlExtensions.ULEnd()
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.DivEnd()
                       .WriteTo(writer, HtmlEncoder.Default);
                    //start the bottom navbar
                    HtmlExtensions.DivStart(string.Empty, string.Empty, "navbar", string.Empty)
                       .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.ULStart(string.Empty, string.Empty)
                        .WriteTo(writer, HtmlEncoder.Default);
                    WriteMenuStep(writer, "stepfour", string.Empty,
                        "Go to step four", AppHelper.GetResource("CALCS_STEP4"));
                }
                if (iStepNumber >= 5)
                {
                    WriteMenuStep(writer, "stepfive", string.Empty,
                        "Go to step five", AppHelper.GetResource("CALCS_STEP5"));
                }
                if (iStepNumber >= 6)
                {
                    WriteMenuStep(writer, "stepsix", string.Empty,
                        "Go to step six", AppHelper.GetResource("CALCS_STEP6"));
                }
                if (iStepNumber >= 7)
                {
                    WriteMenuStep(writer, "stepseven", string.Empty,
                        "Go to step seven", AppHelper.GetResource("CALCS_STEP7"));
                }
                if (iStepNumber >= 8)
                {
                    WriteMenuStep(writer, "stepeight", string.Empty,
                        "Go to step eight", AppHelper.GetResource("CALCS_STEP8"));
                }
                if (iStepNumber >= 9)
                {
                    WriteMenuStep(writer, "stepnine", string.Empty,
                        "Go to step nine", AppHelper.GetResource("CALCS_STEP9"));
                }
                WriteMenuStep(writer, "steplast", string.Empty,
                    "Get help", AppHelper.GetResource("CALCS_HELP"));
                HtmlExtensions.ULEnd()
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd()
                    .WriteTo(writer, HtmlEncoder.Default);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        

        private static int GetStepNumber(string integer)
        {
            int iInteger = 0;
            if (integer == "1")
            {
                iInteger = 1;
            }
            else if (integer == "2")
            {
                iInteger = 2;
            }
            else if (integer == "3")
            {
                iInteger = 3;
            }
            else if (integer == "4")
            {
                iInteger = 4;
            }
            else if (integer == "5")
            {
                iInteger = 5;
            }
            else if (integer == "6")
            {
                iInteger = 6;
            }
            else if (integer == "7")
            {
                iInteger = 7;
            }
            else if (integer == "8")
            {
                iInteger = 8;
            }
            else if (integer == "9")
            {
                iInteger = 9;
            }
            else if (integer == "10")
            {
                iInteger = 10;
            }
            else if (integer == "11")
            {
                iInteger = 11;
            }
            else if (integer == "12")
            {
                iInteger = 12;
            }
            else if (integer == "13")
            {
                iInteger = 13;
            }
            else if (integer == "14")
            {
                iInteger = 14;
            }
            else if (integer == "15")
            {
                iInteger = 15;
            }
            else if (integer == "16")
            {
                iInteger = 16;
            }
            else if (integer == "17")
            {
                iInteger = 17;
            }
            else if (integer == "18")
            {
                iInteger = 18;
            }
            else if (integer == "19")
            {
                iInteger = 19;
            }
            else if (integer == "20")
            {
                iInteger = 20;
            }
            return iInteger;
        }
        public static void WriteMenuStep(StringWriter writer,
            string stepNumber, string gifFileName,
            string altDescription, string text)
        {
            HtmlExtensions.LIStart(string.Empty, string.Empty)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.LinkMobile(stepNumber, "#",
                string.Empty, text, string.Empty,
                "true", "true", string.Empty, string.Empty)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.LIEnd()
                .WriteTo(writer, HtmlEncoder.Default);
        }
        
        public static string SetRowArgs(
            int oldRow, int startRow, string isForward,
            DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES networkFilterType,
            int parentRow)
        {
            string sRowArgs = string.Empty;
            //stay consistent with Form.js.GetRowArgs()
            sRowArgs = string.Concat(
                DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER, START_ROW_TYPES.oldstartrow.ToString(), "=", oldRow.ToString(),
                DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER, START_ROW_TYPES.startrow.ToString(), "=", startRow.ToString(),
                DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER, START_ROW_TYPES.isforward.ToString(), "=", isForward,
                DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER, DevTreks.ViewModels.SearchViewModel.NETWORK_FILTER_TYPE, "=", networkFilterType.ToString(),
                DataHelpers.GeneralHelpers.FORMELEMENT_DELIMITER, START_ROW_TYPES.parentstartrow.ToString(), "=", parentRow.ToString());
            return sRowArgs;
        }
        public static string WriteStandardCalculatorParams1(string searchurl, string viewEditType,
            string useSameCalculator, string overwrite)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                string sName = string.Empty;
                bool bIsChecked = true;
                string sValue = string.Empty;
                string sText = string.Empty;
                HtmlExtensions.DivStart(string.Empty, string.Empty, "fieldcontain", string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.FieldsetStart(string.Empty, string.Empty, 
                    "controlgroup", "horizontal", "true")
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.Legend(AppHelper.GetResource("USEIN_CHILDS"))
                    .WriteTo(writer, HtmlEncoder.Default);
                sName =
                    (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                    ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, "UseSameCalculator", "boolean", "1")
                    : string.Empty;
                bIsChecked = (useSameCalculator == "1") ? true : false;
                sValue = "1";
                sText = "True";
                HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full, 
                    "lblUseSame1", string.Empty, "radio", sName, sValue, bIsChecked)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.LabelRegular("lblUseSame1", sText)
                    .WriteTo(writer, HtmlEncoder.Default);
                bIsChecked = (useSameCalculator != "1") ? true : false;
                sValue = "0";
                sText = "False";
                HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "lblUseSame2", string.Empty, "radio", sName, sValue, bIsChecked)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.LabelRegular("lblUseSame2", sText)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.FieldsetEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivStart(string.Empty, string.Empty, 
                    "fieldcontain", string.Empty).WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.FieldsetStart(string.Empty, string.Empty, 
                    "controlgroup", "horizontal", "true").WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.Legend(AppHelper.GetResource("OVERWRITE_CHILDS"))
                    .WriteTo(writer, HtmlEncoder.Default);
                sName =
                    (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                    ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, "Overwrite", "boolean", "1")
                    : string.Empty;
                bIsChecked = (overwrite == "1") ? true : false;
                sValue = "1";
                sText = "True";
                HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "lblOverwrite1", string.Empty, "radio", sName, sValue, bIsChecked)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.LabelRegular("lblOverwrite1", sText)
                    .WriteTo(writer, HtmlEncoder.Default);
                bIsChecked = (overwrite != "1") ? true : false;
                sValue = "0";
                sText = "False";
                HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "lblOverwrite2", string.Empty, "radio", sName, sValue, bIsChecked)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.LabelRegular("lblOverwrite2", sText)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.FieldsetEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public static string WriteStandardCalculatorParams2(string searchurl, string viewEditType,
            string whatIfTagName, string relatedCalculatorsType)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                string sName = string.Empty;
                string sValue = string.Empty;
                string sText = string.Empty;
                HtmlExtensions.DivStart(string.Empty, "ui-grid-a")
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivStart(string.Empty, "ui-block-a")
                    .WriteTo(writer, HtmlEncoder.Default);
                if (!string.IsNullOrEmpty(whatIfTagName))
                {
                    sName =
                        (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                        ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, "WhatIfTagName", "string", "25")
                        : string.Empty;
                    HtmlExtensions.LabelRegular("lblWhatIfTagName", "What If Tag")
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.Input("lblWhatIfTagName", string.Empty, "text", string.Empty,
                        sName, whatIfTagName)
                        .WriteTo(writer, HtmlEncoder.Default);
                }
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivStart(string.Empty, "ui-block-b").WriteTo(writer, HtmlEncoder.Default);
                if (!string.IsNullOrEmpty(relatedCalculatorsType))
                {
                    sName =
                        (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                        ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, "RelatedCalculatorsType", "string", "25")
                        : string.Empty;
                    HtmlExtensions.LabelRegular("lblRelatedCalculatorsType", "Related C. Type")
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.Input("lblRelatedCalculatorsType", string.Empty, "text", string.Empty,
                        sName, relatedCalculatorsType)
                        .WriteTo(writer, HtmlEncoder.Default);
                }
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public static string WriteStandardAnalysisParams1(string searchurl, string viewEditType,
            string docToCalcNodeName, string option1, string option2, string option4)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                string sName = string.Empty;
                bool bIsChecked = true;
                string sValue = string.Empty;
                string sText = string.Empty;
                if (!string.IsNullOrEmpty(option1))
                {
                    WriteStandardComparisons(writer,
                        searchurl, viewEditType, docToCalcNodeName, option1);
                }
                if (!string.IsNullOrEmpty(option2))
                {
                    HtmlExtensions.DivStart(string.Empty, string.Empty, 
                        "fieldcontain", string.Empty).WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.FieldsetStart(string.Empty, string.Empty, 
                        "controlgroup", "horizontal", "true").WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.Legend("Aggregate Using:").WriteTo(writer, HtmlEncoder.Default);
                    sName =
                        (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                        ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, DataAppHelpers.Calculator.cOption2, "string", "25")
                        : string.Empty;
                    bIsChecked = (option2 == DataAppHelpers.Calculator.AGGREGATION_OPTIONS.none.ToString()) ? true : false;
                    sValue = DataAppHelpers.Calculator.AGGREGATION_OPTIONS.none.ToString();
                    sText = "None";
                    HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lblAggregate0", string.Empty, "radio", sName, sValue, bIsChecked)
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.LabelRegular("lblAggregate0", sText).WriteTo(writer, HtmlEncoder.Default);
                    bIsChecked = (option2 == DataAppHelpers.Calculator.AGGREGATION_OPTIONS.labels.ToString()) ? true : false;
                    sValue = DataAppHelpers.Calculator.AGGREGATION_OPTIONS.labels.ToString();
                    sText = "Labels";
                    HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lblAggregate1", string.Empty, "radio", sName, sValue, bIsChecked)
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.LabelRegular("lblAggregate1", sText).WriteTo(writer, HtmlEncoder.Default);
                    bIsChecked = (option2 == DataAppHelpers.Calculator.AGGREGATION_OPTIONS.types.ToString()) ? true : false;
                    sValue = DataAppHelpers.Calculator.AGGREGATION_OPTIONS.types.ToString();
                    sText = "Types";
                    HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lblAggregate2", string.Empty, "radio", sName, sValue, bIsChecked)
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.LabelRegular("lblAggregate2", sText).WriteTo(writer, HtmlEncoder.Default);
                    bIsChecked = (option2 == DataAppHelpers.Calculator.AGGREGATION_OPTIONS.groups.ToString()) ? true : false;
                    sValue = DataAppHelpers.Calculator.AGGREGATION_OPTIONS.groups.ToString();
                    sText = "Groups";
                    HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lblAggregate3", string.Empty, "radio", sName, sValue, bIsChecked)
                        .WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.LabelRegular("lblAggregate3", sText).WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.FieldsetEnd().WriteTo(writer, HtmlEncoder.Default);
                    HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                }
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public static string WriteStandardComparisons(string searchurl, string viewEditType,
            string docToCalcNodeName, string option1)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                //write the txtWriter's string
                WriteStandardComparisons(writer,
                        searchurl, viewEditType, docToCalcNodeName, option1);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        private static void WriteStandardComparisons(StringWriter writer,
            string searchurl, string viewEditType, string docToCalcNodeName, string option1)
        {
            string sName = string.Empty;
            bool bIsChecked = true;
            string sValue = string.Empty;
            string sText = string.Empty;
            HtmlExtensions.DivStart(string.Empty, string.Empty, "fieldcontain", string.Empty)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.FieldsetStart(string.Empty, string.Empty, 
                "controlgroup", "horizontal", "true").WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.Legend("Compare Using:").WriteTo(writer, HtmlEncoder.Default);
            sName =
                (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, DataAppHelpers.Calculator.cOption1, "string", "25")
                : string.Empty;
            bIsChecked = (option1 == "none") ? true : false;
            sValue = "none";
            sText = "None";
            HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                "lblCompare1", string.Empty, "radio", sName, sValue, bIsChecked)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.LabelRegular("lblCompare1", sText).WriteTo(writer, HtmlEncoder.Default);
            bIsChecked = (option1 != "none") ? true : false;
            sValue = DataAppHelpers.Calculator.COMPARISON_OPTIONS.compareonly.ToString();
            sText = "Compare Only";
            HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                "lblCompare2", string.Empty, "radio", sName, sValue, bIsChecked)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.LabelRegular("lblCompare2", sText).WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.FieldsetEnd().WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
        }
        public static string WriteStandardAnalysisParams2(string searchurl, string viewEditType, string docToCalcNodeName,
            string option5)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                string sName = string.Empty;
                bool bIsChecked = true;
                string sValue = string.Empty;
                string sText = string.Empty;
                if (string.IsNullOrEmpty(option5))
                    option5 = "no";
                HtmlExtensions.DivStart(string.Empty, string.Empty, "fieldcontain", string.Empty)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.FieldsetStart(string.Empty, string.Empty, 
                    "controlgroup", "horizontal", "true").WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.Legend("Display Full View:").WriteTo(writer, HtmlEncoder.Default);
                sName =
                    (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                    ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, DataAppHelpers.Calculator.cOption5, "string", "10")
                    : string.Empty;
                bIsChecked = (option5 == "yes") ? true : false;
                sValue = "yes";
                sText = "Yes";
                HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "NeedsFullView1", string.Empty, "radio", sName, sValue, bIsChecked)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.LabelRegular("NeedsFullView1", sText)
                    .WriteTo(writer, HtmlEncoder.Default);
                bIsChecked = (option5 != "yes") ? true : false;
                sValue = "no";
                sText = "No";
                HtmlExtensions.InputCheckBox(DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "NeedsFullView2", string.Empty, "radio", sName, sValue, bIsChecked)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.LabelRegular("NeedsFullView2", sText)
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.FieldsetEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public static string WriteAlternatives(string searchurl, string viewEditType,
            string alternativeType, string targetType)
        {
            string sXhtml = string.Empty;
            //write to the txtWriter
            using (StringWriter writer = new StringWriter())
            {
                string sName = string.Empty;
                string sValue = string.Empty;
                string sText = string.Empty;
                HtmlExtensions.DivStart(string.Empty, "ui-grid-a")
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivStart(string.Empty, "ui-block-a")
                    .WriteTo(writer, HtmlEncoder.Default);
                if (string.IsNullOrEmpty(alternativeType))
                {
                    alternativeType = DataAppHelpers.Calculator.ALTERNATIVE_TYPES.none.ToString();
                }
                sName =
                    (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                    ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, DataAppHelpers.Calculator.cAlternativeType, "string", "25")
                    : string.Empty;
                HtmlExtensions.LabelRegular(DataAppHelpers.Calculator.cAlternativeType, 
                    "Altern Type").WriteTo(writer, HtmlEncoder.Default);
                DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES eViewEditType
                    = DataHelpers.GeneralHelpers.GetViewEditType(viewEditType);
                HtmlExtensions.SelectStart(eViewEditType, DataAppHelpers.Calculator.cAlternativeType,
                    sName, string.Empty).WriteTo(writer, HtmlEncoder.Default);
                bool bIsSelected = false;
                foreach (string alttype in DataAppHelpers.Calculator.GetAlternativeTypes())
                {
                    bIsSelected = false;
                    if (alttype == alternativeType)
                    {
                        bIsSelected = true;
                    }
                    HtmlExtensions.Option(alttype, alttype, bIsSelected)
                        .WriteTo(writer, HtmlEncoder.Default);
                }
                HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivStart(string.Empty, "ui-block-b").WriteTo(writer, HtmlEncoder.Default);
                if (string.IsNullOrEmpty(targetType))
                {
                    targetType = DataAppHelpers.Calculator.TARGET_TYPES.none.ToString();
                }
                sName =
                    (viewEditType == DataHelpers.GeneralHelpers.VIEW_EDIT_TYPES.full.ToString())
                    ? EditHelpers.EditHelper.MakeStandardEditName(searchurl, DataAppHelpers.Calculator.cTargetType, "string", "25")
                    : string.Empty;
                HtmlExtensions.LabelRegular(DataAppHelpers.Calculator.cTargetType, "Target Type")
                    .WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.SelectStart(eViewEditType, DataAppHelpers.Calculator.cTargetType,
                    sName, string.Empty).WriteTo(writer, HtmlEncoder.Default);
                bIsSelected = false;
                foreach (string tartype in DataAppHelpers.Calculator.GetTargetTypes())
                {
                    bIsSelected = false;
                    if (tartype == targetType)
                    {
                        bIsSelected = true;
                    }
                    HtmlExtensions.Option(tartype, tartype, bIsSelected)
                        .WriteTo(writer, HtmlEncoder.Default);
                }
                HtmlExtensions.SelectEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
                sXhtml = writer.ToString();
            }
            return sXhtml;
        }
        public static void WriteBlock(StringWriter writer, string blockClass,
            string labelNameA, string labelForA, string inputTextValueA, string inputNameA)
        {
            HtmlExtensions.DivStart(string.Empty, blockClass)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.LabelRegular(labelForA, labelNameA)
                .WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.Input(labelForA, string.Empty, "text", string.Empty,
                inputNameA, inputTextValueA).WriteTo(writer, HtmlEncoder.Default);
            HtmlExtensions.DivEnd().WriteTo(writer, HtmlEncoder.Default);
        }
        public static string GetAddNumberN2(string one, string two, string three, string four)
        {
            string sN2 = string.Empty;
            double db1
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(one);
            double db2
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(two);
            double db3
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(three);
            double db4
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(four);
            double dbSum = db1 + db2 + db3 + db4;
            sN2 = dbSum.ToString("N2", CultureInfo.InvariantCulture);
            return sN2;
        }
        public static string GetSubtractNumberN2(string one, string two, string three, string four)
        {
            string sN2 = string.Empty;
            double db1
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(one);
            double db2
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(two);
            double db3
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(three);
            double db4
                = DataHelpers.GeneralHelpers.ConvertStringToDouble(four);
            double dbSum = db1 - db2 - db3 - db4;
            sN2 = dbSum.ToString("N2", CultureInfo.InvariantCulture);
            return sN2;
        }
       
        
        public static string GetStatefulAncestorParams(ContentURI uri,
            ref bool hasCurrentURI)
        {
            string sStatefulAncestorParams = DataHelpers.GeneralHelpers.GetFormValue(uri, Services.ContentService.HISTORYTOCPARAM);
            //the ancestors don't change on the edit and linkedviews panels so that 
            //users can navigate back and forth between current node and ancestors
            if (uri.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit
                && string.IsNullOrEmpty(sStatefulAncestorParams)
                && uri.URIFileExtensionType
                != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                && uri.URIDataManager.Ancestors != null)
            {
                StringBuilder oHistoryTocParam = new StringBuilder();
                int i = 0;
                oHistoryTocParam.Append("&");
                oHistoryTocParam.Append(Services.ContentService.HISTORYTOCPARAM);
                oHistoryTocParam.Append("=");
                foreach (ContentURI ancestor in uri.URIDataManager.Ancestors)
                {
                    if (i != 0) oHistoryTocParam.Append(DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                    if (hasCurrentURI == false)
                    {
                        hasCurrentURI = (ancestor.URIPattern.Equals(uri.URIPattern))
                            ? true : false;
                        if (hasCurrentURI)
                            ancestor.URIName += "*";
                    }
                    oHistoryTocParam.Append(ancestor.URIPattern);
                    i++;
                }
                if (hasCurrentURI == false)
                {
                    //don't put the service in or goes to the service agreement
                    if (uri.URINodeName
                        != DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                    {
                        //make sure the currenturi is a parameter
                        oHistoryTocParam.Append(DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                        oHistoryTocParam.Append(uri.URIPattern);
                    }
                }
                sStatefulAncestorParams = oHistoryTocParam.ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(sStatefulAncestorParams))
                {
                    sStatefulAncestorParams = string.Concat(
                        DataHelpers.GeneralHelpers.MakeFormElement(
                        Services.ContentService.HISTORYTOCPARAM,
                        sStatefulAncestorParams));
                    hasCurrentURI = true;
                }
            }
            if (sStatefulAncestorParams == null)
                sStatefulAncestorParams = string.Empty;
            return sStatefulAncestorParams;
        }
        public static string GetStatefulLinkedViewAncestorParams(ContentURI uri,
            ref bool hasCurrentURI)
        {
            string sStatefulAncestorParams = DataHelpers.GeneralHelpers.GetFormValue(uri, Services.ContentService.LINKEDVIEWSTOCPARAM);
            //string sStatefulAncestorParams =
            //    uri.URIDataManager.FormInput[Services.Helpers.ContentHelper.LINKEDVIEWSTOCPARAM];
            //the ancestors don't change on the edit and linkedviews panels so that 
            //users can navigate back and forth between current node and ancestors
            if (uri.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews
                && string.IsNullOrEmpty(sStatefulAncestorParams))
            {
                StringBuilder oHistoryTocParam = new StringBuilder();
                int i = 0;
                oHistoryTocParam.Append("&");
                oHistoryTocParam.Append(Services.ContentService.LINKEDVIEWSTOCPARAM);
                oHistoryTocParam.Append("=");
                foreach (ContentURI ancestor in uri.URIDataManager.Ancestors)
                {
                    if (ancestor.URINodeName
                        == DevTreks.Data.AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                    {
                        i = 1;
                    }
                    if (i >= 1)
                    {
                        if (i != 1) oHistoryTocParam.Append(DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                        if (hasCurrentURI == false)
                        {
                            hasCurrentURI = (ancestor.URIPattern.Equals(uri.URIPattern))
                                ? true : false;
                            if (hasCurrentURI)
                                ancestor.URIName += "*";
                        }
                        oHistoryTocParam.Append(ancestor.URIPattern);
                        i++;
                    }
                }
                if (hasCurrentURI == false)
                {
                    //make sure the currenturi is a parameter
                    //add the current uri (never an ancestor)
                    uri.URIDataManager.Ancestors.Add(uri);
                    oHistoryTocParam.Append(DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                    oHistoryTocParam.Append(uri.URIPattern);
                }
                //if it has a selectedLinkedView, add it, but don't have an active link
                ContentURI selectedLinkedView
                    = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedView(uri);
                if (selectedLinkedView != null)
                {
                    uri.URIDataManager.Ancestors.Add(selectedLinkedView);
                    oHistoryTocParam.Append(DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                    oHistoryTocParam.Append(selectedLinkedView.URIPattern);
                }
                sStatefulAncestorParams = oHistoryTocParam.ToString();
            }
            else
            {
                sStatefulAncestorParams = string.Concat(
                    DataHelpers.GeneralHelpers.MakeFormElement(
                    Services.ContentService.LINKEDVIEWSTOCPARAM,
                    sStatefulAncestorParams));
            }
            return sStatefulAncestorParams;
        }
        
        public static bool IsValidForSelection(StringWriter writer,
            ContentURI uri, ContentURI child, ref bool hasInstructions)
        {
            //selections must be made from parent nodes
            bool bIsValidForSelection = ((uri.URINodeName != child.URINodeName)
                && (uri.URINodeName != DataAppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                    && uri.URINodeName != DataAppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()
                    && uri.URINodeName != DataAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
                ? true : false;
            if (uri.URIDataManager.SelectionsNodeNeededName
                != string.Empty)
            {
                if (uri.URIDataManager.SelectionsNodeNeededName
                    != child.URINodeName)
                {
                    if (!(child.URINodeName
                        == DataAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString()
                        && uri.URIDataManager.SelectionsNodeNeededName.EndsWith(
                        DataAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString())))
                    {
                        bIsValidForSelection = false;
                        if (hasInstructions == false)
                        {
                            string sInstructs = string.Concat(AppHelper.GetResource("SELECT_ONLY_NODES"),
                                uri.URIDataManager.SelectionsNodeNeededName);
                            HtmlExtensions.PError(sInstructs)
                                .WriteTo(writer, HtmlEncoder.Default);
                            hasInstructions = true;
                        }
                    }
                }
            }
            return bIsValidForSelection;
        }
        public static bool IsValidForSelection(ContentURI uri,
            ContentURI child, ref bool hasInstructions,
            ref string instructions)
        {
            //selections must be made from parent nodes
            bool bIsValidForSelection = ((uri.URINodeName != child.URINodeName)
                && (uri.URINodeName != DataAppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()
                    && uri.URINodeName != DataAppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()
                    && uri.URINodeName != DataAppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
                ? true : false;
            if (uri.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                if (child.URINodeName
                    == DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString())
                {
                    //ok to select memberbases
                    bIsValidForSelection = true;
                }
            }
            if (uri.URIDataManager.SelectionsNodeNeededName
                != string.Empty)
            {
                if (uri.URIDataManager.SelectionsNodeNeededName
                    != child.URINodeName)
                {
                    if (!(child.URINodeName
                        == DataAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString()
                        && uri.URIDataManager.SelectionsNodeNeededName.EndsWith(
                        DataAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString())))
                    {
                        bIsValidForSelection = false;
                        if (bIsValidForSelection == false)
                        {
                            if (hasInstructions == false)
                            {
                                instructions = string.Concat(
                                    AppHelper.GetResource("SELECT_ONLY_NODES_HEAD"),
                                    "  ", AppHelper.GetResource("SELECT_ONLY_NODES"),
                                    uri.URIDataManager.SelectionsNodeNeededName);
                                hasInstructions = true;
                            }
                        }
                    }
                }
                else
                {
                    bIsValidForSelection = true;
                }
            }
            return bIsValidForSelection;
        }
        
        public static bool IsFolder(ContentURI uri, ContentURI child)
        {
            bool bIsFolder = true;
            if (uri.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString()
                || uri.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString()
                || uri.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString()
                || uri.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString()
                || uri.URINodeName == DataAppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString()
                || uri.URINodeName == DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString()
                || uri.URINodeName == DataAppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString()
                || uri.URINodeName == DataAppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
            {
                //children of these nodes are always displayed as a toc item
                bIsFolder = false;
            }
            else if (uri.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                || uri.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                if (child.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutput.ToString())
                {
                    bIsFolder = false;
                }
                else if (child.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutput.ToString())
                {
                    bIsFolder = false;
                }
            }
            else if (uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.accounts
                || uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.members
                || uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.networks
                || uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.locals
                || uri.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.addins)
            {
                if (uri.URIDataManager.ParentPanelType ==
                    DataHelpers.GeneralHelpers.UPDATE_PANEL_TYPES.none)
                {
                    //groups can be opened
                    bIsFolder = true;
                }
                else
                {
                    //single nodes
                    bIsFolder = false;
                }
            }
            else
            {
                //rest displayed as folder
                bIsFolder = true;
            }
            return bIsFolder;
        }

        public static void GetTocSeparator(ContentURI uri, ContentURI child,
            ref string tocSeparator, ref bool isFirst)
        {
            if (child.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString()
                || child.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
            {
                if (tocSeparator == "Outcome")
                {
                    tocSeparator = string.Empty;
                    isFirst = true;
                }
            }
            else if (child.URINodeName == DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                if (tocSeparator == "DevPack")
                {
                    tocSeparator = string.Empty;
                    isFirst = true;
                }
            }
            if (string.IsNullOrEmpty(tocSeparator)
                || isFirst)
            {
                tocSeparator = GetTocSeparator(tocSeparator, uri, child);
            }
        }
        public static string GetTocSeparator(string existingTocSeparator,
            ContentURI uri, ContentURI child)
        {
            string sTocSeparator = existingTocSeparator;
            if (string.IsNullOrEmpty(existingTocSeparator))
            {
                if (uri.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                    || uri.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                {
                    if (child.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString())
                    {
                        sTocSeparator = "Outcome";
                    }
                    else if (child.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString())
                    {
                        sTocSeparator = "Outcome";
                    }
                    else if (child.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString())
                    {
                        sTocSeparator = "Operation";
                    }
                    else if (child.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString())
                    {
                        sTocSeparator = "Component";
                    }
                }
                else if (uri.URINodeName == DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                {
                    if (child.URINodeName == DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                    {
                        sTocSeparator = "DevPack";
                    }
                    else if (child.URINodeName == DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
                    {
                        sTocSeparator = "DevPackPart";
                    }
                }
            }
            return sTocSeparator;
        }
        public static bool IsListItemOnly(ContentURI uri, bool isFolder)
        {
            bool bIsListItemOnly = false;
            if (uri.URIDataManager.ChildrenPanelType
                == DataHelpers.GeneralHelpers.UPDATE_PANEL_TYPES.none)
            {
                bIsListItemOnly = true;
            }
            if (uri.URINodeName == DataAppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString()
                || uri.URINodeName == DataAppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
            {
                if (!isFolder)
                {
                    bIsListItemOnly = true;
                }
            }
            return bIsListItemOnly;
        }
        
        
        public static string SetSelectedLinkedViewParams(ContentURI uri,
            bool needsSingleQuote)
        {
            string sSelectParams = string.Empty;
            if (uri.URIDataManager.UseSelectedLinkedView == true)
            {
                string sCalcDocURIPattern = string.Empty;
                string sDocToCalcURIPattern = string.Empty;
                string sSelectedLinkedViewURIPattern = string.Empty;
                //calcparams tell select/edit buttons to use linked view doc for edits
                sSelectParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                    needsSingleQuote, uri, string.Empty, ref sCalcDocURIPattern,
                    ref sDocToCalcURIPattern, ref sSelectedLinkedViewURIPattern);
            }
            return sSelectParams;
        }
        
        
       
        public static void MakeViewsCalcParams(ContentURI docToCalcURI,
            bool isAddInSelections, ref string calcParams)
        {
            if (isAddInSelections)
            {
                //addin hasn't been selected yet (standard host inits with stepzero)
                Services.Helpers.AddInRunHelper
                    .SetHostInitParams(string.Empty, ref calcParams);
            }
            AddAdditionalCalcParams(docToCalcURI, isAddInSelections, ref calcParams);
            string sHistoryTocs
                = DataHelpers.GeneralHelpers.GetFormValue(docToCalcURI,
                Services.ContentService.LINKEDVIEWSTOCPARAM);
            if (!string.IsNullOrEmpty(sHistoryTocs))
            {
                calcParams = string.Concat(DataHelpers.GeneralHelpers.MakeFormElement(
                    Services.ContentService.LINKEDVIEWSTOCPARAM,
                    sHistoryTocs), calcParams);
            }
        }
        public static void AddAdditionalCalcParams(ContentURI docToCalcURI, bool isAddInSelections, 
            ref string calcParams)
        {
            if (docToCalcURI.URIFileExtensionType
                == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                || ((docToCalcURI.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks
                || docToCalcURI.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.linkedviews)
                && docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.edit))
            {
                //tempdocs need a full set of calcparams
                //customdocs being edited in edits panel also need calcparams
                bool bNeedsSingleQuotes = false;
                string sCalcDocURI = string.Empty;
                string sDocToCalcURI = string.Empty;
                string sSelectedLinkedViewURI = string.Empty;
                calcParams = string.Concat(DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                    bNeedsSingleQuotes, docToCalcURI, string.Empty,
                    ref sCalcDocURI, ref sDocToCalcURI, ref sSelectedLinkedViewURI), calcParams);
            }
            else if ((docToCalcURI.URIDataManager.AppType
                == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks
                && docToCalcURI.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.linkedviews))
            {
                //2.0.0 refactor added this condition for devpacks
                //it had been part of initial condition and skipped because devpacks not run in edit panel
                //these are the same calcparams sent in from Preview panel when devpack originally loaded in linked views panel
                bool bNeedsSingleQuotes = false;
                //very important that calcdocuri = none so that it is excluded from calcparams
                //need to use the selection made in lstAddinLinkedView (2nd drop down selects)
                string sCalcDocURI = DataHelpers.GeneralHelpers.NONE;
                string sDocToCalcURI = string.Empty;
                string sSelectedLinkedViewURI = string.Empty;
                if (isAddInSelections)
                {
                    calcParams = string.Concat(DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                        bNeedsSingleQuotes, docToCalcURI, string.Empty,
                        ref sCalcDocURI, ref sDocToCalcURI, ref sSelectedLinkedViewURI), calcParams);
                }
                else
                {
                    //don't want any calculator to load; just the original xml doc holding edits
                    //make them select the calculator from the second select list
                }
                //default addin view need a form element for that param
                if (docToCalcURI.URIDataManager.UseDefaultAddIn
                    || docToCalcURI.URIDataManager.UseDefaultLocal)
                {
                    calcParams = string.Concat(
                        DataAppHelpers.LinkedViews.GetDefaultAddInStartParam(docToCalcURI),
                        calcParams);
                }
                else
                {
                    //views use pagination and need rowargs
                    string sPageParams = StylesheetHelper.SetRowArgs(docToCalcURI.URIDataManager.StartRow,
                        docToCalcURI.URIDataManager.StartRow, "-1", DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        docToCalcURI.URIDataManager.ParentStartRow);
                    calcParams = string.Concat(sPageParams, calcParams);
                }
            }
            else
            {
                //default addin view need a form element for that param
                if (docToCalcURI.URIDataManager.UseDefaultAddIn
                    || docToCalcURI.URIDataManager.UseDefaultLocal)
                {
                    calcParams = string.Concat(
                        DataAppHelpers.LinkedViews.GetDefaultAddInStartParam(docToCalcURI),
                        calcParams);
                }
                else
                {
                    //views use pagination and need rowargs
                    string sPageParams = StylesheetHelper.SetRowArgs(docToCalcURI.URIDataManager.StartRow,
                        docToCalcURI.URIDataManager.StartRow, "-1", DevTreks.Data.AppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        docToCalcURI.URIDataManager.ParentStartRow);
                    calcParams = string.Concat(sPageParams, calcParams);
                }
            }
        }
        public static string GetContentURIPatternNoVar(ContentURI model)
        {
            string sContentURIPatternNoPage = model.URIDataManager.ContentURIPattern;
            if (model.URIDataManager.Variable != DataHelpers.GeneralHelpers.NONE)
            {
                if (!string.IsNullOrEmpty(model.URIDataManager.Variable))
                {
                    sContentURIPatternNoPage = ContentURI.ChangeContentURIPatternPart(model.URIDataManager.ContentURIPattern,
                        ContentURI.CONTENTURIPATTERNPART.variable, string.Empty);
                }
            }
            return sContentURIPatternNoPage;
        }
        public static bool IsCollapsed(ContentURI uri, int recordToDisplayId)
        {
            bool bIsCollapsed = true;
            if (uri != null)
            {
                string sAction = string.Empty;
                if (uri.URIDataManager.FormInput != null)
                {
                    string sKeyName = string.Empty;
                    string[] arrUpdateParams = null;
                    string sEditURIPattern = string.Empty;
                    string sAttName = string.Empty;
                    string sDataType = string.Empty;
                    string sSize = string.Empty;
                    string sRecordBeingEditedId = string.Empty;
                    foreach (KeyValuePair<string, string> kvp in uri.URIDataManager.FormInput)
                    {
                        sKeyName = kvp.Key;
                        sAction = string.Empty;
                        sEditURIPattern = string.Empty;
                        sRecordBeingEditedId = string.Empty;
                        //verify actual edits and not ui instructions (i.e. getting new views)
                        int iIndex = sKeyName.LastIndexOf(DataHelpers.GeneralHelpers.STRING_DELIMITER);
                        if (iIndex > 0)
                        {
                            sAction = kvp.Value;
                            if (sAction.EndsWith("*"))
                            {
                                arrUpdateParams = sKeyName.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS);
                                if (arrUpdateParams != null)
                                {
                                    //this is a record that is being edited
                                    EditHelpers.EditHelper.GetStandardEditNameParams(arrUpdateParams,
                                        out sEditURIPattern, out sAttName, out sDataType, out sSize);
                                    sRecordBeingEditedId = ContentURI.GetURIPatternPart(sEditURIPattern,
                                        ContentURI.URIPATTERNPART.id);
                                    if (recordToDisplayId.ToString().Equals(sRecordBeingEditedId))
                                    {
                                        bIsCollapsed = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bIsCollapsed;
        }
        
        
        public static string GetDisplayNameForSelections(ContentURI uri, string selections)
        {
            string sDisplayName = uri.URIDataManager.SelectionsNodeNeededName;
            return sDisplayName;
        }
        public static string GetDisplayNameForSelections(ContentURI uri)
        {
            string sDisplayName = uri.URIDataManager.SelectionsNodeNeededName;
            return sDisplayName;
        }
        public static async Task AddHtmlHeaderFilesToPackageAsync(
           ContentURI uri, string newDirectory, IDictionary<string, string> zipArgs)
        {
            //zipArgs key is unique filepath and value is DataAccess.PackageIO.HEADERS_KEYNAME, 
            //PackageIO and ZIPIO use that value to relate these headers to all html files in packages (if needed)
            //make the stylesheets' paths (absolute paths being used)
            //these must match the MakeFullHtmlFileHeaders paths (relative paths to css)
            //2.0.0: use the minified css and js files 
            //(current files have to be copied after inetpub deployment)
            //css
            await AddHtmlHeaderFileToPackageAsync(uri, DataHelpers.AppSettings.GetWebContentFullPath(uri, "css", "site.min.css"),
                DataHelpers.AppSettings.GetPackageFullPath(uri, newDirectory, "css", "site.min.css"), zipArgs);
            //scripts (need to click on +closed folders to open them on client)
            await AddHtmlHeaderFileToPackageAsync(uri, DataHelpers.AppSettings.GetWebContentFullPath(uri, "js", "site.min.js"),
                DataHelpers.AppSettings.GetPackageFullPath(uri, newDirectory, "js", "site.min.js"), zipArgs);

            //deprecated all of the non min css and js in 2.0.0
            //await AddHtmlHeaderFileToPackageAsync(uri, DataHelpers.AppSettings.GetWebContentFullPath(uri, "css", "Site1.css"),
            //   DataHelpers.AppSettings.GetPackageFullPath(uri, newDirectory, "stylesheets", "Site1.css"), zipArgs);
   
        }
        public static async Task AddHtmlHeaderFileToPackageAsync(
            ContentURI uri, string currentFilePath, string newFilePath,
            IDictionary<string, string> zipArgs)
        {
            //2.0.0 deprecated URIAbsoluteExists(currentFP) because copyuris does the same
            if (DataHelpers.FileStorageIO.DirectoryCreate(uri, newFilePath))
            {
                bool bHasCopied = await DataHelpers.FileStorageIO.CopyURIsAsync(
                    uri, currentFilePath, newFilePath);
                zipArgs.Add(newFilePath, DataHelpers.PackageIO.HEADERS_KEYNAME);
                DataHelpers.GeneralHelpers.AddToList(newFilePath, DataHelpers.PackageIO.HEADERS_KEYNAME,
                    zipArgs);
            }
        }
    }
}
