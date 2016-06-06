using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTreks.Models;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General constants, enums and utilities
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public static class GeneralHelpers
    {
        public const string CSV_DELIMITER = ",";
        public static char[] CSV_DELIMITERS = new char[] { ',' };
        public const string SPACE_DELIMITER = " ";
        public static char[] SPACE_DELIMITERS = new char[] { ' ' };
        public const string STRING_DELIMITER = ";";
        public const string STRING_DELIMITER_POST = "; ";
        public static char[] STRING_DELIMITERS = new char[] { ';' };
        public const string FILENAME_DELIMITER = "_";
        public static char[] FILENAME_DELIMITERS = new char[] { '_' };
        public const string FILE_PATH_DELIMITER = @"\";
        public static char[] FILE_PATH_DELIMITERS = FILE_PATH_DELIMITER.ToCharArray();
        public const string WEBFILE_PATH_DELIMITER = "/";
        public static char[] WEBFILE_PATH_DELIMITERS = new char[] { '/' };
        public const string PARAMETER_DELIMITER = "@";
        public static char[] PARAMETER_DELIMITERS = new char[] { '@' };
        public const string FORMELEMENT_DELIMITER = "&";
        public static char[] FORMELEMENT_DELIMITERS = new char[] { '&' };
        //2.0.0 refactor: all html is encoded using htmlextensions (can't include amp;)
        public const string FORMELEMENT_HTMLDELIMITER = "&";
        //public const string FORMELEMENT_HTMLDELIMITER = "&amp;";
        public const string EQUALS = "=";
        public const string QUESTION = "?";
        public const string FILEEXTENSION_DELIMITER = ".";
        public static char[] FILEEXTENSION_DELIMITERS = new char[] { '.' };
        //The newline character, \u000A.
        public static char[] LINE_DELIMITERS = new char[] { '\n' };
        public const string LINE_DELIMITER = "\n";
        //The tab return char for asynch utf 8 encoded text
        public static char[] TAB_DELIMITERS = new char[] { '\r' };
        public const string TAB_DELIMITER = "\r";
        public const string NAMESPACE_ABBREV = "devtreks";
        public const string ROOT_PATH = "root";
        public const string ROOT_NODE = "<root></root>";
        public const string ROOT_START_NODE = "<root>";
        public const string ROOT_END_NODE = "</root>";
        public const string METADATA_ELEMENT_NAME = "metadata";
        public const string METADATA_ELEMENT_STARTNAME = "<metadata";
        public const string BINARY = "binary";
        public const string AT_ID = "@Id";
        public const string NAMESPACE_DB_ABBREV = "y0";
        public const string NAMESPACE_DB_ABBREV_COLON = "y0:";
        public const string AT_ACCOUNTID = "@AccountId";
        public const string EXTENSION_XML = ".xml";
        public const string EXTENSION_MISC = "_misc";
        public const string EXTENSION_CSV = ".csv";
        public const string EXTENSION_XSLT = ".xslt";
        public const string EXTENSION_FULL_XML = "_full.xml";
        public const string ADDIN = "Addin";
        public const string VALUE = "Value";
        public const string XPATH_START = "//";
        public const string NONE = "none";
        public const string EACH = "each";
        public const string XMLNS = "xmlns";
        public const string CLP = "colp";
        /// <summary>
        /// DevTreks apps
        /// </summary>
        public enum APPLICATION_TYPES
        {
            none        = 0,
            accounts    = 1,
            members     = 2,
            agreements  = 3,
            addins      = 4,
            networks    = 5,
            locals      = 6,
            prices      = 7,
            economics1  = 8,
            linkedviews     = 9,
            resources   = 10,
            devpacks    = 11
        }
        /// <summary>
        /// Subapplications
        /// </summary>
        public enum SUBAPPLICATION_TYPES
        {
            none            = 0,
            clubs           = 1,
            members         = 2,
            agreements      = 3,
            addins          = 4,
            networks        = 5,
            locals          = 6,
            inputprices     = 100,
            outputprices    = 200,
            outcomeprices   = 300,
            operationprices = 400,
            componentprices = 500,
            budgets         = 600,
            investments     = 700,
            devpacks        = 800,
            linkedviews     = 900,
            resources       = 1000
        }
        /// <summary>
        /// 5th, optional, parameter to standard searchnames
        /// i.e. Feeds_1234_1_inputgroup_temp.xml
        /// </summary>
        public enum FILENAME_EXTENSIONS
        {
            none        = 0,
            //econnode groups holding full calculations
            full        = 1,
            //tempdocs generally built by anonymous users
            temp        = 2,
            //addins
            addin       = 3,
            //selected views (custom docs)
            selected    = 4,
            //select lists stored in tempdocs that lists stateful docs copied to tempdoc path
            selectlist  = 5
            //custom FILENAME_EXTENSIONS are used by addins to find other addin results
        }

        /// <summary>
        /// Status of document
        /// </summary>
        public enum DOCS_STATUS
        {
            //don't change donotdisplay; hard coded integer used in search queries
            donotdisplay    = 0,
            approved        = 1,
            underreview     = 2,
            underrevision   = 3,
            notreviewed     = 4
        }
        /// <summary>
        /// How to display a document (none = don't display)
        /// </summary>
        public enum VIEW_EDIT_TYPES
        {
            none    = 0,
            print   = 1,
            part    = 2,
            full    = 3
        }
        //each serversubaction can have 'specialty' views
        public enum SUBACTION_VIEWS
        {
            none                    = 0,
            mobile                  = 1,
            summary                 = 2,
            full                    = 3,
            graph                   = 4,
            csv                     = 5,
            xml                     = 6,
            tsv                     = 7,
            //clues respondwithlist about the specific list to load
            //clubs that have subscribed to an owned service
            services                = 8,
            //categories used to classify the data associated with an owned service
            categories              = 9,
            //clues respondwithlist about the specific list to load
            //linkedviews to current node (uri.URIDataManager.SubActionView)
            linkedviewslist         = 10,
            //same pattern for linkedview xmldocs updates
            linkedviewsxmldocs      = 11

        }
        public enum DOC_STATE_NUMBER
        {
            none = 0,
            //schema-generated, relational data
            firstdoc = 1,
            //addins stored as xmldoc att fields
            seconddoc = 2,
            //non-addin xmldoc att fields or file system analysis docs, other linked view docs
            thirddoc = 3
        }
        /// <summary>
        /// The action in the MVC route pattern (devtreks.org/search/crops/input/Alfalfa/4567)
        /// </summary>
        public enum SERVER_ACTION_TYPES
        {
            //MVC and route pattern actions
            none        = 0,
            search      = 1,
            preview     = 2,
            edit        = 3,
            select      = 4,
            pack        = 5,
            linkedviews = 6,
            //club panel inits by member
            member      = 7
        }
        /// <summary>
        /// not part of route patterns because it complicates the uri
        /// mainly sent by ajax-related client requests in form els (subserveraction param)
        /// (a RESTful API for machines needs more thought)
        /// </summary>
        public enum SERVER_SUBACTION_TYPES
        {
            none                    = 0,
            respondwithhtml         = 1,
            searchbynetwork         = 2,
            searchbyservice         = 3,
            submitedits             = 4,
            closeedits              = 5,
            adddefaults             = 6,
            uploadfile              = 7,
            downloadfile            = 8,
            saveselects             = 9,
            savenewselects          = 10,
            respondwithxml          = 11,
            makepackage             = 12,
            runaddin                = 13,
            respondwithlist         = 14,
            submitlistedits         = 15,
            respondwithnewxhtml     = 16,
            respondwithform         = 17,
            submitformedits         = 18,
            buildtempdoc            = 19,
            cancel                  = 20
        }
        public enum CLIENTACTION_TYPES
        {
            postrequest         = 0,
            closeelement        = 1,
            openwindow          = 2,
            changeurl           = 3,
            loaddoc             = 4,
            discardselects      = 5,
            addtopack           = 6,
            downloaddoc         = 7,
            prepaddin           = 8
        }
        //at present, it is convenient to keep these the same as SERVER_ACTION_TYPES
        public enum UPDATE_PANEL_TYPES
        {
            none        = 0,
            search      = 1,
            preview     = 2,
            edit        = 3,
            select      = 4,
            pack        = 5,
            linkedviews = 6,
            member      = 7
        }
        
        public static DOCS_STATUS GetDocStatus(int docStatus)
        {
            DOCS_STATUS eDocStatus = DOCS_STATUS.notreviewed;
            if (docStatus < 5)
            {
                eDocStatus = (DOCS_STATUS)docStatus;
            }
            else
            {
                eDocStatus = DOCS_STATUS.notreviewed;
            }
            return eDocStatus;
        }
        public static DOCS_STATUS GetDocsStatus(string docStatus)
        {
            DOCS_STATUS eDocStatus = DOCS_STATUS.notreviewed;
            if (docStatus == DOCS_STATUS.approved.ToString())
            {
                eDocStatus = DOCS_STATUS.approved;
            }
            else if (docStatus == DOCS_STATUS.donotdisplay.ToString())
            {
                eDocStatus = DOCS_STATUS.donotdisplay;
            }
            else if (docStatus == DOCS_STATUS.notreviewed.ToString())
            {
                eDocStatus = DOCS_STATUS.notreviewed;
            }
            else if (docStatus == DOCS_STATUS.underreview.ToString())
            {
                eDocStatus = DOCS_STATUS.underreview;
            }
            else if (docStatus == DOCS_STATUS.underrevision.ToString())
            {
                eDocStatus = DOCS_STATUS.underrevision;
            }
            return eDocStatus;
        }
        public static Dictionary<string, string> GetDocStatusDictionary()
        {
            Dictionary<string, string> docstats = new Dictionary<string, string>();
            int iValue = (int)DOCS_STATUS.donotdisplay;
            docstats.Add(iValue.ToString(), DOCS_STATUS.donotdisplay.ToString());
            iValue = (int)DOCS_STATUS.notreviewed;
            docstats.Add(iValue.ToString(), DOCS_STATUS.notreviewed.ToString());
            iValue = (int)DOCS_STATUS.underreview;
            docstats.Add(iValue.ToString(), DOCS_STATUS.underreview.ToString());
            iValue = (int)DOCS_STATUS.underrevision;
            docstats.Add(iValue.ToString(), DOCS_STATUS.underrevision.ToString());
            iValue = (int)DOCS_STATUS.approved;
            docstats.Add(iValue.ToString(), DOCS_STATUS.approved.ToString());
            return docstats;
        }
        public static Dictionary<string, string> GetSubAppTypeDictionary()
        {
            Dictionary<string, string> subapps = new Dictionary<string, string>();
            int iValue = (int)SUBAPPLICATION_TYPES.budgets;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.budgets.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.componentprices;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.componentprices.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.devpacks;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.devpacks.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.inputprices;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.inputprices.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.investments;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.investments.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.linkedviews;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.linkedviews.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.operationprices;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.operationprices.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.outcomeprices;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.outcomeprices.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.outputprices;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.outputprices.ToString());
            iValue = (int)SUBAPPLICATION_TYPES.resources;
            subapps.Add(iValue.ToString(), SUBAPPLICATION_TYPES.resources.ToString());
            return subapps;
        }
        public static SERVER_ACTION_TYPES GetServerAction(string serverAction)
        {
            SERVER_ACTION_TYPES eSERVERACTION = SERVER_ACTION_TYPES.none;
            if (serverAction == SERVER_ACTION_TYPES.edit.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.edit;
            }
            else if (serverAction == SERVER_ACTION_TYPES.linkedviews.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.linkedviews;
            }
            else if (serverAction == SERVER_ACTION_TYPES.member.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.member;
            }
            else if (serverAction == SERVER_ACTION_TYPES.pack.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.pack;
            }
            else if (serverAction == SERVER_ACTION_TYPES.preview.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.preview;
            }
            else if (serverAction == SERVER_ACTION_TYPES.search.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.search;
            }
            else if (serverAction == SERVER_ACTION_TYPES.select.ToString())
            {
                eSERVERACTION = SERVER_ACTION_TYPES.select;
            }
            return eSERVERACTION;
        }
        public static SERVER_SUBACTION_TYPES GetServerSubAction(string serverSubAction)
        {
            //default is always respond with html
            SERVER_SUBACTION_TYPES eSERVERSUBACTION = SERVER_SUBACTION_TYPES.respondwithhtml;
            if (serverSubAction == SERVER_SUBACTION_TYPES.adddefaults.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.adddefaults;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.closeedits.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.closeedits;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.downloadfile.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.downloadfile;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.makepackage.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.makepackage;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.respondwithform.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.respondwithform;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.respondwithhtml.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.respondwithhtml;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.respondwithlist.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.respondwithlist;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.respondwithnewxhtml.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.respondwithnewxhtml;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.respondwithxml.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.respondwithxml;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.runaddin.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.runaddin;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.savenewselects.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.savenewselects;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.saveselects.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.saveselects;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.buildtempdoc.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.buildtempdoc;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.searchbynetwork.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.searchbynetwork;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.searchbyservice.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.searchbyservice;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.submitedits.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.submitedits;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.submitformedits.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.submitformedits;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.submitlistedits.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.submitlistedits;
            }
            else if (serverSubAction == SERVER_SUBACTION_TYPES.uploadfile.ToString())
            {
                eSERVERSUBACTION = SERVER_SUBACTION_TYPES.uploadfile;
            }
            return eSERVERSUBACTION;
        }
        public static SUBACTION_VIEWS GetSubActionView(string subActionView)
        {
            SUBACTION_VIEWS eSubActionView = SUBACTION_VIEWS.none;
            if (subActionView == SUBACTION_VIEWS.categories.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.categories;
            }
            else if (subActionView == SUBACTION_VIEWS.csv.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.csv;
            }
            else if (subActionView == SUBACTION_VIEWS.full.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.full;
            }
            else if (subActionView == SUBACTION_VIEWS.graph.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.graph;
            }
            else if (subActionView == SUBACTION_VIEWS.linkedviewslist.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.linkedviewslist;
            }
            else if (subActionView == SUBACTION_VIEWS.linkedviewsxmldocs.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.linkedviewsxmldocs;
            }
            else if (subActionView == SUBACTION_VIEWS.mobile.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.mobile;
            }
            else if (subActionView == SUBACTION_VIEWS.services.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.services;
            }
            else if (subActionView == SUBACTION_VIEWS.summary.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.summary;
            }
            else if (subActionView == SUBACTION_VIEWS.tsv.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.tsv;
            }
            else if (subActionView == SUBACTION_VIEWS.xml.ToString())
            {
                eSubActionView = SUBACTION_VIEWS.xml;
            }
            return eSubActionView;
        }
        //2.0.0 copied from AppHelper to compile
        /// <summary>
        /// This method retrieves localized resource strings.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns>localized string</returns>
        public static string GetResource(string resourceName)
        {
            string sResourceValue
                = DevTreks.Resources.DevTreksResources.GetDevTreksString(resourceName);
            return sResourceValue;
        }
        
        
        public static bool IsXmlFileExt(string fileName)
        {
            bool bIsXmlFile = false;
            if (fileName.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xhtml.ToString())
                || fileName.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xml.ToString())
                || fileName.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xslt.ToString())
                || fileName.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xsl.ToString())
                || fileName.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xsd.ToString()))
            {
                bIsXmlFile = true;
            }
            return bIsXmlFile;
        }
        public static bool IsXmlType(string generalResourceType)
        {
            bool bIsXmlFile = false;
            if (generalResourceType.Equals(AppHelpers.Resources.GENERAL_RESOURCE_TYPES.stylesheet1.ToString())
                || generalResourceType.Equals(AppHelpers.Resources.GENERAL_RESOURCE_TYPES.stylesheet2.ToString())
                || generalResourceType.Equals(AppHelpers.Resources.GENERAL_RESOURCE_TYPES.xml.ToString())
                || generalResourceType.Equals(AppHelpers.Resources.GENERAL_RESOURCE_TYPES.idpf_ebook.ToString())
            )
            {
                bIsXmlFile = true;
            }
            return bIsXmlFile;
        }
        public static bool IsXmlMimeType(string mimetype)
        {
            bool bIsXmlFile = false;
            if (mimetype.Contains(AppHelpers.Resources.FILEEXTENSION_TYPES.xhtml.ToString())
                || mimetype.Contains(AppHelpers.Resources.FILEEXTENSION_TYPES.xml.ToString())
                || mimetype.Contains(AppHelpers.Resources.FILEEXTENSION_TYPES.xslt.ToString())
                || mimetype.Contains(AppHelpers.Resources.FILEEXTENSION_TYPES.xsl.ToString())
                || mimetype.Contains(AppHelpers.Resources.FILEEXTENSION_TYPES.xsd.ToString()))
            {
                bIsXmlFile = true;
            }
            return bIsXmlFile;
        }
        public static void SetURIParams(ContentURI uri)
        {
            if (string.IsNullOrEmpty(uri.URIPattern) == false)
            {
                TrimURIPattern(uri);
                string[] arrNameIds = uri.URIPattern.Split(WEBFILE_PATH_DELIMITERS);
                if (arrNameIds != null)
                {
                    int iNumber = 0;
                    int iArrayLength = arrNameIds.Length;
                    if (iArrayLength > 0)
                    {
                        uri.URINetworkPartName = arrNameIds[0];
                    }
                    else
                    {
                        //full uripatterns 
                        uri.URIPattern = MakeURIPatternStart();
                        SetURIParams(uri);
                    }
                    if (iArrayLength > 1)
                    {
                        uri.URINodeName = arrNameIds[1];
                    }
                    else
                    {
                        uri.URIPattern = MakeURIPatternStart();
                        SetURIParams(uri);
                    }
                    if (iArrayLength > 2)
                    {
                        //not necessary to keeps names js compatible until they are used in js
                        string sName = arrNameIds[2];
                        uri.URIName = sName;
                    }
                    else
                    {
                        uri.URIPattern = MakeURIPatternStart();
                        SetURIParams(uri);
                    }
                    if (iArrayLength > 3)
                    {
                        string sId = arrNameIds[3].Replace("*", string.Empty);
                        uri.URIId = int.TryParse(sId, out iNumber) ? iNumber : 0;
                    }
                    else
                    {
                        uri.URIPattern = MakeURIPatternStart();
                        SetURIParams(uri);
                    }
                    if (iArrayLength > 4)
                    {
                        uri.URIFileExtensionType = arrNameIds[4];
                    }
                    else
                    {
                        uri.URIFileExtensionType = NONE;
                    }
                }
                else
                {
                    uri.URIPattern = MakeURIPatternStart();
                    SetURIParams(uri);
                }
            }
            else
            {
                uri.URIPattern = MakeURIPatternStart();
                SetURIParams(uri);
            }
        }
        
        public static void SetContentURIParams(ContentURI uri)
        {
            if (string.IsNullOrEmpty(uri.URIDataManager.ContentURIPattern) == false)
            {
                TrimURIPattern(uri);
                String[] arrNameIds = uri.URIDataManager.ContentURIPattern.Split(WEBFILE_PATH_DELIMITERS);
                if (arrNameIds != null)
                {
                    int iNumber = 0;
                    int iArrayLength = arrNameIds.Length;
                    if (iArrayLength > 0)
                    {
                        uri.URIDataManager.ControllerName = arrNameIds[0];
                    }
                    if (iArrayLength > 1)
                    {
                        uri.URIDataManager.ServerActionType 
                            = GetServerAction(arrNameIds[1]);
                    }
                    if (iArrayLength > 2)
                    {
                        uri.URINetworkPartName = arrNameIds[2];
                    }
                    if (iArrayLength > 3)
                    {
                        uri.URINodeName = arrNameIds[3];
                    }
                    if (iArrayLength > 4)
                    {
                        uri.URIName = arrNameIds[4];
                    }
                    if (iArrayLength > 5)
                    {
                        string sId = arrNameIds[5].Replace("*", string.Empty);
                        uri.URIId = int.TryParse(sId, out iNumber) ? iNumber : 0;
                    }
                    if (iArrayLength > 6)
                    {
                        uri.URIFileExtensionType = arrNameIds[6];
                    }
                    if (iArrayLength > 7)
                    {
                        uri.URIDataManager.ServerSubActionType = GetServerSubAction(arrNameIds[7]);
                    }
                    if (iArrayLength > 8)
                    {
                        uri.URIDataManager.SubActionView = arrNameIds[8];
                    }
                    if (iArrayLength > 9)
                    {
                        uri.URIDataManager.Variable = arrNameIds[9];
                    }
                    uri.URIPattern = MakeURIPattern(uri.URIName, uri.URIId.ToString(),
                        uri.URINetworkPartName, uri.URINodeName, uri.URIFileExtensionType);
                }
                else
                {
                    uri.URIPattern = MakeURIPatternStart();
                    SetURIParams(uri);
                }
            }
            else
            {
                uri.URIPattern = MakeURIPatternStart();
                SetURIParams(uri);
            }
        }
        public static void SetURIParamsFromFileName(ContentURI uri,
            string fileNameURIPattern)
        {
            if (string.IsNullOrEmpty(fileNameURIPattern) == false)
            {
                String[] arrNameIds = fileNameURIPattern.Split(FILENAME_DELIMITERS);
                if (arrNameIds != null)
                {
                    int iNumber = 0;
                    int iArrayLength = arrNameIds.Length;
                    if (iArrayLength > 0)
                    {
                        uri.URIName = arrNameIds[0];
                    }
                    else
                    {
                        uri.URIName = NONE;
                    }
                    if (iArrayLength > 1)
                    {
                        uri.URIId = int.TryParse(arrNameIds[1], out iNumber) ? iNumber : 1;
                    }
                    else
                    {
                        uri.URIId = 0;
                    }
                    if (iArrayLength > 2)
                    {
                        uri.URINetworkPartName = arrNameIds[2];
                    }
                    else
                    {
                        uri.URINetworkPartName = NONE;
                    }
                    if (iArrayLength > 3)
                    {
                        uri.URINodeName = arrNameIds[3];
                    }
                    else
                    {
                        uri.URINodeName = NONE;
                    }
                    if (iArrayLength > 4)
                    {
                        uri.URIFileExtensionType = arrNameIds[4];
                    }
                    else
                    {
                        uri.URIFileExtensionType = NONE;
                    }
                    uri.URIPattern = MakeURIPattern(uri.URIName, uri.URIId.ToString(),
                        uri.URINetworkPartName, uri.URINodeName, uri.URIFileExtensionType);
                }
                else
                {
                    uri.URIPattern = MakeURIPatternStart();
                    SetURIParams(uri);
                }
            }
            else
            {
                uri.URIPattern = MakeURIPatternStart();
                SetURIParams(uri);
            }
        }
        
        public static void TrimURIPattern(ContentURI uri)
        {
            if (uri.URIPattern.EndsWith(WEBFILE_PATH_DELIMITER))
            {
                uri.URIPattern = uri.URIPattern.TrimEnd(WEBFILE_PATH_DELIMITERS);
            }
            if (uri.URIDataManager != null)
            {
                if (uri.URIDataManager.ContentURIPattern.EndsWith(WEBFILE_PATH_DELIMITER))
                {
                    uri.URIDataManager.ContentURIPattern = uri.URIDataManager.ContentURIPattern.TrimEnd(WEBFILE_PATH_DELIMITERS);
                }
            }
        }
        public static void GetSubAppType(string subAppType, out SUBAPPLICATION_TYPES subApplicationEnum)
        {
            subApplicationEnum = SUBAPPLICATION_TYPES.none;
            //subapptypes enum (int) used for search searches
            if (subAppType != string.Empty)
            {
                subApplicationEnum = (SUBAPPLICATION_TYPES)Enum.Parse(typeof(SUBAPPLICATION_TYPES), subAppType);
            }
        }
        public static int GetSubAppType(string subAppType)
        {
            SUBAPPLICATION_TYPES eSubAppType = SUBAPPLICATION_TYPES.none;
            //subapptypes enum (int) used for search searches
            int iServiceGroupId = 0;
            if (subAppType != string.Empty)
            {
                GetSubAppType(subAppType, out eSubAppType);
            }
            iServiceGroupId = Convert.ToInt32(eSubAppType);
            return iServiceGroupId;
        }
        public static void ChangeAppTypeFromNodeName(ContentURI uriToChange,
            string nodeName)
        {
            APPLICATION_TYPES eAppType = uriToChange.URIDataManager.AppType;
            SUBAPPLICATION_TYPES eSubAppType = uriToChange.URIDataManager.SubAppType;
            GetAppTypesFromNodeName(nodeName, out eAppType, out eSubAppType);
            if (eAppType != APPLICATION_TYPES.none)
            {
                uriToChange.URIDataManager.AppType = eAppType;
            }
            if (eSubAppType != SUBAPPLICATION_TYPES.none)
            {
                uriToChange.URIDataManager.SubAppType = eSubAppType;
            }
        }
        public static void GetAppTypesFromNodeName(string currentNodeName,
            out APPLICATION_TYPES appType, out SUBAPPLICATION_TYPES subAppType)
        {
            appType = APPLICATION_TYPES.none;
            subAppType = SUBAPPLICATION_TYPES.none;
            if (currentNodeName.StartsWith(AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString()))
            {
                appType = APPLICATION_TYPES.accounts;
                subAppType = SUBAPPLICATION_TYPES.clubs;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Members.MEMBER_TYPES.member.ToString()))
            {
                appType = APPLICATION_TYPES.members;
                subAppType = SUBAPPLICATION_TYPES.members;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Networks.NETWORK_TYPES.network.ToString()))
            {
                appType = APPLICATION_TYPES.networks;
                subAppType = SUBAPPLICATION_TYPES.networks;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Locals.LOCAL_TYPES.local.ToString()))
            {
                appType = APPLICATION_TYPES.locals;
                subAppType = SUBAPPLICATION_TYPES.locals;
            }
            else if (currentNodeName.StartsWith(AppHelpers.AddIns.ADDIN_TYPES.addin.ToString()))
            {
                appType = APPLICATION_TYPES.addins;
                subAppType = SUBAPPLICATION_TYPES.addins;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()))
            {
                //first node in an agreement is an account node - already used by account app, so second node is where the search param is used
                appType = APPLICATION_TYPES.agreements;
                //for now no contract types to use
                subAppType = SUBAPPLICATION_TYPES.agreements;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                appType = APPLICATION_TYPES.economics1;
                subAppType = SUBAPPLICATION_TYPES.budgets;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString()))
            {
                appType = APPLICATION_TYPES.economics1;
                subAppType = SUBAPPLICATION_TYPES.investments;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.outcomeprices;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.operationprices;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.componentprices;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.inputprices;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.outputprices;
            }
            else if (currentNodeName.StartsWith(AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                || currentNodeName.EndsWith("packs"))
            {
                appType = APPLICATION_TYPES.devpacks;
                subAppType = SUBAPPLICATION_TYPES.devpacks;
            }
            else if (currentNodeName.StartsWith(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                appType = APPLICATION_TYPES.linkedviews;
                subAppType = SUBAPPLICATION_TYPES.linkedviews;
            }
            else if (currentNodeName.StartsWith(AppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
            {
                appType = APPLICATION_TYPES.resources;
                subAppType = SUBAPPLICATION_TYPES.resources;
            }
        }
        public static SUBAPPLICATION_TYPES GetSubAppTypeFromNode(
            string currentNodeName)
        {
            string sSubAppType = GetSubAppTypeFromNodeName(currentNodeName);
            SUBAPPLICATION_TYPES eSubAppType
                = SUBAPPLICATION_TYPES.none;
            GetSubAppType(sSubAppType, out eSubAppType);
            return eSubAppType;
        }
        public static string GetSubAppTypeFromNodeName(string nodeName)
        {
            string sSubAppType = string.Empty;
            if (nodeName.StartsWith(AppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.budgets.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.investments.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.outcomeprices.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.operationprices.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.componentprices.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.outputprices.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.inputprices.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Locals.LOCAL_TYPES.local.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.locals.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.devpacks.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.linkedviews.ToString();
            }
            else if (nodeName.StartsWith(AppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
            {
                sSubAppType = SUBAPPLICATION_TYPES.resources.ToString();
            }
            return sSubAppType;
        }
        public static void SetApps(ContentURI uri)
        {
            string sCurrentNodeName = (string.IsNullOrEmpty(uri.URINodeName) == false)
                ? uri.URINodeName : string.Empty;
            if (sCurrentNodeName.StartsWith(AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.accounts;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.clubs;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Members.MEMBER_TYPES.member.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.members;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.members;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Networks.NETWORK_TYPES.network.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.networks;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.networks;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Locals.LOCAL_TYPES.local.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.locals;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.locals;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.AddIns.ADDIN_TYPES.addin.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.addins;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.addins;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                || sCurrentNodeName.StartsWith(AppHelpers.Agreement.AGREEMENT_TYPES.incentive.ToString())
                || sCurrentNodeName.StartsWith(APPLICATION_TYPES.agreements.ToString()))
            {
                //first node in an agreement is an account node - already used by account app, so second node is where the search param is used
                uri.URIDataManager.AppType = APPLICATION_TYPES.agreements;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.agreements;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.economics1;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.budgets;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.economics1;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.investments;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.outcomeprices;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.operationprices;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.componentprices;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.inputprices;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.outputprices;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                || sCurrentNodeName.EndsWith("packs"))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.devpacks;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.devpacks;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.linkedviews;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.linkedviews;
            }
            else if (sCurrentNodeName.StartsWith(AppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.resources;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.resources;
            }
            //set servicegroupid from subapptype
            if (uri.URIService == null)
                uri.URIService = new AccountToService();
            if (uri.URIService.Service == null)
                uri.URIService.Service = new Service();
            if (uri.URIDataManager.SubAppType != SUBAPPLICATION_TYPES.none)
            {
                uri.URIService.Service.ServiceClassId = GetSubAppType(uri.URIDataManager.SubAppType.ToString());
            }
            else
            {
                uri.URIDataManager.AppType = APPLICATION_TYPES.linkedviews;
                uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.linkedviews;
                uri.URIService.Service.ServiceClassId = GetSubAppType(uri.URIDataManager.SubAppType.ToString());
            }
        }
        
        public static void GetApps(string nodeName, out APPLICATION_TYPES appType,
            out SUBAPPLICATION_TYPES subAppType)
        {
            appType = APPLICATION_TYPES.none;
            subAppType = SUBAPPLICATION_TYPES.none;
            if (nodeName.StartsWith(AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString()))
            {
                appType = APPLICATION_TYPES.accounts;
                subAppType = SUBAPPLICATION_TYPES.clubs;
            }
            else if (nodeName.StartsWith(AppHelpers.Members.MEMBER_TYPES.member.ToString()))
            {
                appType = APPLICATION_TYPES.members;
                subAppType = SUBAPPLICATION_TYPES.members;
            }
            else if (nodeName.StartsWith(AppHelpers.Networks.NETWORK_TYPES.network.ToString()))
            {
                appType = APPLICATION_TYPES.networks;
                subAppType = SUBAPPLICATION_TYPES.networks;
            }
            else if (nodeName.StartsWith(AppHelpers.Locals.LOCAL_TYPES.local.ToString()))
            {
                appType = APPLICATION_TYPES.locals;
                subAppType = SUBAPPLICATION_TYPES.locals;
            }
            else if (nodeName.StartsWith(AppHelpers.AddIns.ADDIN_TYPES.addin.ToString()))
            {
                appType = APPLICATION_TYPES.addins;
                subAppType = SUBAPPLICATION_TYPES.addins;
            }
            else if (nodeName.StartsWith(AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                || nodeName.StartsWith(AppHelpers.Agreement.AGREEMENT_TYPES.incentive.ToString())
                || nodeName.StartsWith(APPLICATION_TYPES.agreements.ToString()))
            {
                //first node in an agreement is an account node - already used by account app, so second node is where the search param is used
                appType = APPLICATION_TYPES.agreements;
                subAppType = SUBAPPLICATION_TYPES.agreements;
            }
            else if (nodeName.StartsWith(AppHelpers.Economics1.BUDGET_TYPES.budget.ToString()))
            {
                appType = APPLICATION_TYPES.economics1;
                subAppType = SUBAPPLICATION_TYPES.budgets;
            }
            else if (nodeName.StartsWith(AppHelpers.Economics1.INVESTMENT_TYPES.investment.ToString()))
            {
                appType = APPLICATION_TYPES.economics1;
                subAppType = SUBAPPLICATION_TYPES.investments;
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.outcomeprices;
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.operationprices;
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.componentprices;
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.inputprices;
            }
            else if (nodeName.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
            {
                appType = APPLICATION_TYPES.prices;
                subAppType = SUBAPPLICATION_TYPES.outputprices;
            }
            else if (nodeName.StartsWith(AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
                || nodeName.EndsWith("packs"))
            {
                appType = APPLICATION_TYPES.devpacks;
                subAppType = SUBAPPLICATION_TYPES.devpacks;
            }
            else if (nodeName.StartsWith(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString()))
            {
                appType = APPLICATION_TYPES.linkedviews;
                subAppType = SUBAPPLICATION_TYPES.linkedviews;
            }
            else if (nodeName.StartsWith(AppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
            {
                appType = APPLICATION_TYPES.resources;
                subAppType = SUBAPPLICATION_TYPES.resources;
            }
        }
        public static APPLICATION_TYPES GetAppTypeFromURIPattern(string uriPattern)
        {
            string sNodeName = ContentURI.GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.node);
            APPLICATION_TYPES eAppType = APPLICATION_TYPES.none;
            SUBAPPLICATION_TYPES eSubAppType = SUBAPPLICATION_TYPES.none;
            GetAppTypesFromNodeName(sNodeName, out eAppType, out eSubAppType);
            return eAppType;
        }
        public static SUBAPPLICATION_TYPES GetSubAppTypeFromURIPattern(string uriPattern)
        {
            string sNodeName = ContentURI.GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.node);
            APPLICATION_TYPES eAppType = APPLICATION_TYPES.none;
            SUBAPPLICATION_TYPES eSubAppType = SUBAPPLICATION_TYPES.none;
            GetAppTypesFromNodeName(sNodeName, out eAppType, out eSubAppType);
            return eSubAppType;
        }
        public static void SetAppTypes(int serviceGroupId, ContentURI uri)
        {
            switch (serviceGroupId)
            {
                case 1:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.accounts;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.clubs;
                    break;
                case 2:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.members;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.members;
                    break;
                case 3:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.agreements;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.agreements;
                    break;
                case 4:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.addins;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.addins;
                    break;
                case 5:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.networks;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.networks;
                    break;
                case 6:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.locals;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.locals;
                    break;
                case 100:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.inputprices;
                    break;
                case 200:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.outputprices;
                    break;
                case 300:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.outcomeprices;
                    break;
                case 400:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.operationprices;
                    break;
                case 500:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.prices;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.componentprices;
                    break;
                case 600:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.economics1;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.budgets;
                    break;
                case 700:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.economics1;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.investments;
                    break;
                case 800:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.devpacks;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.devpacks;
                    break;
                case 900:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.linkedviews;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.linkedviews;
                    break;
                case 1000:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.resources;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.resources;
                    break;
                default:
                    uri.URIDataManager.AppType = APPLICATION_TYPES.none;
                    uri.URIDataManager.SubAppType = SUBAPPLICATION_TYPES.none;
                    break;
            }
        }
        public static string GetCategoryNodeName(int serviceGroupId)
        {
            string sCategoryNodeName = string.Empty;
            //all categories handled alike
            switch (serviceGroupId)
            {
                case 100:
                    sCategoryNodeName = AppHelpers.Prices.INPUT_PRICE_TYPES.inputtype.ToString();
                    break;
                case 200:
                    sCategoryNodeName = AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputtype.ToString();
                    break;
                case 300:
                    sCategoryNodeName = AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcometype.ToString();
                    break;
                case 400:
                    sCategoryNodeName = AppHelpers.Prices.OPERATION_PRICE_TYPES.operationtype.ToString();
                    break;
                case 500:
                    sCategoryNodeName = AppHelpers.Prices.COMPONENT_PRICE_TYPES.componenttype.ToString();
                    break;
                case 600:
                    sCategoryNodeName = AppHelpers.Economics1.BUDGET_TYPES.budgettype.ToString();
                    break;
                case 700:
                    sCategoryNodeName = AppHelpers.Economics1.INVESTMENT_TYPES.investmenttype.ToString();
                    break;
                case 800:
                    sCategoryNodeName = AppHelpers.DevPacks.DEVPACKS_TYPES.devpacktype.ToString();
                    break;
                case 900:
                    sCategoryNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewtype.ToString();
                    break;
                case 1000:
                    sCategoryNodeName = AppHelpers.Resources.RESOURCES_TYPES.resourcetype.ToString();
                    break;
                default:
                    break;
            }
            return sCategoryNodeName;
        }
        public static void SetAppSearchView(int uriId, string uriNodeName,
            ContentURI uri)
        {
            //provides more fine-tuned control over navigation and display by
            //setting node-specific properties
            if (uri.URIFileExtensionType == FILENAME_EXTENSIONS.temp.ToString()
                && uri.URIDataManager.ServerActionType == SERVER_ACTION_TYPES.edit)
            {
                //temps and edits can always be edited
                uri.URIDataManager.EditViewEditType
                = VIEW_EDIT_TYPES.full;
            }
            else
            {
                uri.URIDataManager.PreviewViewEditType
                    = VIEW_EDIT_TYPES.print;
                uri.URIDataManager.ChildrenNodeName = uriNodeName;
                switch (uri.URIDataManager.AppType)
                {
                    case APPLICATION_TYPES.accounts:
                        AppHelpers.Accounts.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.members:
                        AppHelpers.Members.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.networks:
                        AppHelpers.Networks.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.agreements:
                        AppHelpers.Agreement.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.locals:
                        AppHelpers.Locals.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.addins:
                        AppHelpers.AddIns.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.prices:
                        AppHelpers.Prices.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.economics1:
                        AppHelpers.Economics1.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.devpacks:
                        AppHelpers.DevPacks.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.linkedviews:
                        AppHelpers.LinkedViews.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    case APPLICATION_TYPES.resources:
                        AppHelpers.Resources.SetAppSearchView(uriNodeName,
                            uriId, uri);
                        break;
                    default:
                        //defaults are printview
                        break;
                }
            }
        }
        public static string GetStartingNodeNameFromServiceGroup(int serviceGroupId)
        {
            SUBAPPLICATION_TYPES eSubAppType = (SUBAPPLICATION_TYPES)serviceGroupId;
            string sStartNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
            switch (eSubAppType)
            {
                case SUBAPPLICATION_TYPES.clubs:
                    sStartNodeName = AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString();
                    break;
                case SUBAPPLICATION_TYPES.members:
                    sStartNodeName = AppHelpers.Members.MEMBER_TYPES.member.ToString();
                    break;
                case SUBAPPLICATION_TYPES.agreements:
                    sStartNodeName = AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString();
                    break;
                case SUBAPPLICATION_TYPES.budgets:
                    sStartNodeName = AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString();
                    break;
                case SUBAPPLICATION_TYPES.componentprices:
                    sStartNodeName = AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString();
                    break;
                case SUBAPPLICATION_TYPES.linkedviews:
                    sStartNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    break;
                case SUBAPPLICATION_TYPES.devpacks:
                    sStartNodeName = AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString();
                    break;
                case SUBAPPLICATION_TYPES.inputprices:
                    sStartNodeName = AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString();
                    break;
                case SUBAPPLICATION_TYPES.investments:
                    sStartNodeName = AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString();
                    break;
                case SUBAPPLICATION_TYPES.locals:
                    sStartNodeName = AppHelpers.Locals.LOCAL_TYPES.local.ToString();
                    break;
                case SUBAPPLICATION_TYPES.addins:
                    sStartNodeName = AppHelpers.AddIns.ADDIN_TYPES.addin.ToString();
                    break;
                case SUBAPPLICATION_TYPES.networks:
                    sStartNodeName = AppHelpers.Networks.NETWORK_TYPES.network.ToString();
                    break;
                case SUBAPPLICATION_TYPES.outcomeprices:
                    sStartNodeName = AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString();
                    break;
                case SUBAPPLICATION_TYPES.operationprices:
                    sStartNodeName = AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString();
                    break;
                case SUBAPPLICATION_TYPES.outputprices:
                    sStartNodeName = AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString();
                    break;
                case SUBAPPLICATION_TYPES.resources:
                    sStartNodeName = AppHelpers.Resources.RESOURCES_TYPES.resourcepack.ToString();
                    break;
                default:
                    sStartNodeName = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString();
                    break;
            }
            return sStartNodeName;
        }
        public static void AddToList(string key, string value, 
            IDictionary<string, string> list)
        {
            if (key != string.Empty
                && value != string.Empty)
            {
                if (list.ContainsKey(key) == false)
                {
                    list.Add(key, value);
                }
                else
                {
                    //overwrite the value of the existing list item
                    list[key] = value;
                }
            }
        }
        public static int ConvertMBStorageToInt(string megaByte)
        {
            int iKBStorage = 0;
            //default storage is .5MB and don't exceed 100MB
            double dbMB = ConvertStringToDouble(megaByte);
            if (dbMB > 100)
                dbMB = 100;
            if (dbMB <= 0)
                dbMB = 0.5;
            double MaxDBStorage = dbMB * 1024000;
            iKBStorage = Convert.ToInt32(MaxDBStorage);
            return iKBStorage;
        }
        public static short ConvertInt32Int16(int value)
        {
            short iShort = Convert.ToInt16(value);
            return iShort;
        }
        public static bool ConvertStringToBool(string stringNumber)
        {
            bool bIsTrueOrFalse = false;
            if (stringNumber == "0")
            {
                bIsTrueOrFalse = false;
            }
            else if (stringNumber == "1")
            {
                bIsTrueOrFalse = true;
            }
            else if (stringNumber == "no")
            {
                bIsTrueOrFalse = false;
            }
            else if (stringNumber == "yes")
            {
                bIsTrueOrFalse = true;
            }
            else if (!string.IsNullOrEmpty(stringNumber))
            {
                bool.TryParse(stringNumber, out bIsTrueOrFalse);
            }
            return bIsTrueOrFalse;
        }
        
        public static int ConvertStringToInt(string stringNumber)
        {
            int iNumber = 0;
            if (stringNumber != string.Empty && stringNumber != null)
            {
                //some integers have 1.0000 which must be converted to double
                if (stringNumber.Contains(FILEEXTENSION_DELIMITER))
                {
                    stringNumber = GetParsedString(0,
                        FILEEXTENSION_DELIMITERS, stringNumber);
                }
                int.TryParse(stringNumber, out iNumber);
            }
            return iNumber;
        }
        public static DateTime ConvertStringToDate(string stringDate)
        {
            DateTime dtDate = DateTime.Today;
            if (stringDate != string.Empty && stringDate != null)
            {
                bool bHasParsed = DateTime.TryParse(stringDate, out dtDate);
                if (!bHasParsed)
                {
                    dtDate = GetDateShortNow();
                }
                //range conditions
                DateTime oldestDate = new DateTime(1000, 1, 1);
                DateTime newestData = new DateTime(3000, 1, 1);
                if (dtDate < oldestDate || dtDate > newestData)
                {
                    dtDate = GetDateShortNow();
                }
            }
            return dtDate;
        }
        public static DateTime ConvertStringToShortDate(string stringDate)
        {
            DateTime dtDate = DateTime.Today;
            if (stringDate != string.Empty && stringDate != null)
            {
                bool bHasParsed = DateTime.TryParse(stringDate, out dtDate);
                if (!bHasParsed)
                {
                    dtDate = GetDateShortNow();
                }
                string shortDate = dtDate.ToShortDateString();
                dtDate = DateTime.Parse(shortDate);
                //range conditions
                DateTime oldestDate = new DateTime(1000, 1, 1);
                DateTime newestData = new DateTime(3000, 1, 1);
                if (dtDate < oldestDate || dtDate > newestData)
                {
                    dtDate = GetDateShortNow();
                }
            }
            return dtDate;
        }
        public static decimal ConvertStringToDecimal(string stringNumber)
        {
            decimal dcNumber = 0;
            if (stringNumber != string.Empty && stringNumber != null)
            {
                decimal.TryParse(stringNumber, out dcNumber);
                stringNumber = dcNumber.ToString("f2");
                decimal.TryParse(stringNumber, out dcNumber);
                //NumberFormatInfo nfi = new NumberFormatInfo();
                //nfi.CurrencyDecimalDigits = 2;
                //bool bHasParsed = decimal.TryParse(stringNumber, NumberStyles.Any, nfi, 
                //    out dcNumber);
            }
            return dcNumber;
        }
        public static double ConvertStringToDouble(string stringNumber)
        {
            double dbNumber = 0;
            if (stringNumber != string.Empty && stringNumber != null)
            {
                double.TryParse(stringNumber, out dbNumber);
                //format general rule that no double in DevTreks can 
                //have greater than 4 decimals (comparisons sometimes
                //use numbers like .05000000001
                stringNumber = dbNumber.ToString("f4");
                double.TryParse(stringNumber, out dbNumber);
                //NumberFormatInfo nfi = new NumberFormatInfo();
                //nfi.NumberDecimalDigits = 4;
                //double.TryParse(stringNumber, NumberStyles.Any, nfi, 
                //    out dbNumber);
            }
            return dbNumber;
        }
        public static float ConvertStringToFloat(string stringNumber)
        {
            float fNumber = 0;
            if (stringNumber != string.Empty && stringNumber != null)
            {
                float.TryParse(stringNumber, out fNumber);
                stringNumber = fNumber.ToString("f4");
                float.TryParse(stringNumber, out fNumber);
            }
            return fNumber;
        }
        public static long ConvertStringToLong(string stringNumber)
        {
            long lNumber = 0;
            if (stringNumber != string.Empty && stringNumber != null)
            {
                long.TryParse(stringNumber, out lNumber);
            }
            return lNumber;
        }
        public static short ConvertStringToShort(string stringNumber)
        {
            short iNumber = 0;
            if (stringNumber != string.Empty && stringNumber != null)
            {
                short.TryParse(stringNumber, out iNumber);
            }
            return iNumber;
        }
        /// <summary>
        /// Convert.ToBoolean doesn't handle sql booleans (0=false, 1=true)
        /// </summary>
        /// <param name="valueZeroOrOne"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(string valueZeroOrOne)
        {
            bool bIsTrue = false;
            if (string.IsNullOrEmpty(valueZeroOrOne))
            {
                bIsTrue = false;
            }
            else if (valueZeroOrOne == "0")
            {
                bIsTrue = false;
            }
            else
            {
                bIsTrue = true;
            }
            return bIsTrue;
        }

        
        public static string MakeString(string param1, string param2, string param3,
            string param4, string param5, string param6)
        {
            string sString = string.Empty;
            StringBuilder oString = new StringBuilder();
            if (param1 != string.Empty) oString.Append(param1);
            if (param2 != string.Empty) oString.Append(param2);
            if (param3 != string.Empty) oString.Append(param3);
            if (param4 != string.Empty) oString.Append(param4);
            if (param5 != string.Empty) oString.Append(param5);
            if (param6 != string.Empty) oString.Append(param6);
            sString = oString.ToString();
            return sString;
        }
        public static string MakeString(string[] stringArray)
        {
            StringBuilder oStringBldr = new StringBuilder();
            if (stringArray != null)
            {
                int iArrayLength = stringArray.Length;
                if (iArrayLength > 0)
                {
                    int i = 0;
                    for (i = 0; i < iArrayLength; i++)
                    {
                        oStringBldr.Append(stringArray[i]);
                    }
                }
            }
            return oStringBldr.ToString();
        }
        public static string MakeStringWithDelimiter(string[] stringArray,
            string delimiter)
        {
            StringBuilder oStringBldr = new StringBuilder();
            if (stringArray != null)
            {
                int iArrayLength = stringArray.Length;
                if (iArrayLength > 0)
                {
                    int i = 0;
                    for (i = 0; i < iArrayLength; i++)
                    {
                        oStringBldr.Append(stringArray[i]);
                        if (i < iArrayLength - 1)
                        {
                            oStringBldr.Append(delimiter);
                        }
                    }
                }
            }
            return oStringBldr.ToString();
        }
        public static void GetStringFromStringArray(string[] array, string pathDelimiter, out string joinedArray)
        {
            joinedArray = array.ToString();
        }
        public static string GetParsedString(int i, char[] delimiter,
            string name)
        {
            string sParsedName = name;
            string[] arrLabels = name.Split(delimiter);
            if (arrLabels.Count() >= (i + 1))
            {
                sParsedName = arrLabels[i];
            }
            return sParsedName;
        }
        public static string GetSubstring(string delimitedString,
           char[] delimiter, int pos)
        {
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    //zero based array but 1 based count
                    if (pos < arrSubStrings.Length)
                    {
                        sSubString = arrSubStrings[pos];
                    }
                }
            }
            return sSubString;
        }
        public static string GetSubstringFromEnd(string delimitedString,
            char[] delimiter, int posFromArrayEnd)
        {
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    //adjust for zero-based index
                    int iPosFromArrayFront = arrSubStrings.Length - posFromArrayEnd;
                    if (iPosFromArrayFront >= 0)
                    {
                        sSubString = arrSubStrings[iPosFromArrayFront];
                    }
                }
            }
            return sSubString;
        }
        public static string GetStringFromFront(string delimitedString,
            char[] delimiter, string delimiter2, int posFromArrayFront)
        {
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    //make sure its not too long
                    int iPosFromArrayFront = arrSubStrings.Length - posFromArrayFront;
                    if (iPosFromArrayFront >= 0)
                    {
                        //posFromArrayFront is a 1-based index
                        for (int i = posFromArrayFront; i < arrSubStrings.Length; i++)
                        {
                            if (i == iPosFromArrayFront)
                            {
                                sSubString = arrSubStrings[i];
                            }
                            else
                            {
                                sSubString += string.Concat(delimiter2, arrSubStrings[i]);
                            }
                        }
                    }
                }
            }
            return sSubString;
        }
        public static string GetStringFromEnd(string delimitedString,
            char[] delimiter, string delimiter2, int posFromArrayEnd)
        {
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    //adjust for zero-based index
                    int iPosFromArrayFront = arrSubStrings.Length - posFromArrayEnd;
                    if (iPosFromArrayFront >= 0)
                    {
                        for (int i = iPosFromArrayFront; i < arrSubStrings.Length; i++)
                        {
                            if (i == iPosFromArrayFront)
                            {
                                sSubString = arrSubStrings[i];
                            }
                            else
                            {
                                sSubString += string.Concat(delimiter2, arrSubStrings[i]);
                            }
                        }
                    }
                }
            }
            return sSubString;
        }
        public static string GetStringFromEnd2(string delimitedString,
            char[] delimiter, string delimiter2, int posFromArrayFrontZeroIndex)
        {
            //gets rid of starting http//domain
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    if (posFromArrayFrontZeroIndex >= 0)
                    {
                        for (int i = posFromArrayFrontZeroIndex; i < arrSubStrings.Length; i++)
                        {
                            if (i == posFromArrayFrontZeroIndex)
                            {
                                sSubString = arrSubStrings[i];
                            }
                            else
                            {
                                sSubString += string.Concat(delimiter2, arrSubStrings[i]);
                            }
                        }
                    }
                }
            }
            return sSubString;
        }
        public static string GetSubstringFromFront(string delimitedString,
            char[] delimiter, string delimiter2, int posFromFrontZeroBasedIndex)
        {
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    if (arrSubStrings.Length >= posFromFrontZeroBasedIndex)
                    {
                        for (int i = 0; i < posFromFrontZeroBasedIndex; i++)
                        {
                            if (i == 0)
                            {
                                sSubString = arrSubStrings[i];
                            }
                            else
                            {
                                sSubString += string.Concat(delimiter2, arrSubStrings[i]);
                            }
                        }
                    }
                }
            }
            return sSubString;
        }
        public static string GetSubstringFromFront(string delimitedString,
            char[] delimiter, int posFromFrontOneBasedIndex)
        {
            string sSubString = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                string[] arrSubStrings = delimitedString.Split(delimiter);
                if (arrSubStrings != null)
                {
                    if (arrSubStrings.Length >= posFromFrontOneBasedIndex)
                    {
                        int iAdjustForZeroIndex = (posFromFrontOneBasedIndex == 0) ? posFromFrontOneBasedIndex : posFromFrontOneBasedIndex - 1;
                        sSubString = arrSubStrings[iAdjustForZeroIndex];
                    }
                }
            }
            return sSubString;
        }
        public static string GetSubstringFromFront(string delimitedString,
            string delimiter, int posFromFrontOneBasedIndex)
        {
            string sSubString = string.Empty;
            char cDelimiter;
            bool bHasChar = char.TryParse(delimiter, out cDelimiter);
            if (bHasChar)
            {
                char[] arrDelimiter = { cDelimiter };
                string[] arrSubStrings = delimitedString.Split(arrDelimiter);
                if (arrSubStrings != null)
                {
                    if (arrSubStrings.Length >= posFromFrontOneBasedIndex)
                    {
                        int iAdjustForZeroIndex = (posFromFrontOneBasedIndex == 0) ? posFromFrontOneBasedIndex : posFromFrontOneBasedIndex - 1;
                        sSubString = arrSubStrings[iAdjustForZeroIndex];
                    }
                }
            }
            return sSubString;
        }
        public static List<string> GetLinesFromUTF8Encoding(string utf8String)
        {
            List<string> lines = new List<string>();
            //utf8 uses tabreturn and newline
            if (!string.IsNullOrEmpty(utf8String)
                && utf8String.IndexOf(LINE_DELIMITER) > 0)
            {
                //get rid of tab return chars
                string sNoTabs = utf8String.Replace(TAB_DELIMITER, string.Empty);
                string[] flines = sNoTabs.Split(LINE_DELIMITERS);
                lines = new List<string>(flines.ToArray());
            }
            return lines;
        }
        public static List<string> GetLines(string stringlines)
        {
            List<string> lines = new List<string>();
            if (!string.IsNullOrEmpty(stringlines)
                && (stringlines != NONE))
            {
                if (stringlines.IndexOf(LINE_DELIMITER) > 0)
                {
                    //get rid of tab return chars
                    string sNoTabs = stringlines.Replace(TAB_DELIMITER, string.Empty);
                    string[] flines = sNoTabs.Split(LINE_DELIMITERS);
                    lines = new List<string>(flines.ToArray());
                }
                else if (stringlines.IndexOf(TAB_DELIMITER) > 0)
                {
                    //get rid of line return chars
                    string sNoTabs = stringlines.Replace(LINE_DELIMITER, string.Empty);
                    string[] flines = sNoTabs.Split(TAB_DELIMITERS);
                    lines = new List<string>(flines.ToArray());
                }
            }
            return lines;
        }
        
        public static string MakeURIPatternStart()
        {
            string sURIPatternStart = MakeURIPattern("none", "0", "all", AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString(),
                FILENAME_EXTENSIONS.temp.ToString());
            return sURIPatternStart;
        }
        
        public static string MakePartialContentURIPattern(string currentName,
            string currentId, string networkName, string currentNodeName,
            string fileExtension, string subActionType, 
            string subActionView, string variable)
        {
            string sContentURIPattern = string.Empty;
            string sURIPattern = MakeURIPattern(currentName, currentId, 
                networkName, currentNodeName, fileExtension);
            sContentURIPattern = MakePartialContentURIPattern(sURIPattern,
                subActionType, subActionView, variable);
            return sContentURIPattern;
        }
        public static string MakeURIPattern(string currentName,
            string currentId, string networkName, string currentNodeName,
            string fileExtension)
        {
            //returns: {networkname}/{nodename}/{commonname}/{id}/{fileextensiontype}
            //make sure not using the edited filename
            if (currentName == null) currentName = NONE;
            string sURIName = currentName.Replace("*", "");
            //and validate name
            RuleHelpers.ResourceRules.ValidateScriptArgument(ref currentName);
            StringBuilder oStrBldr = new StringBuilder();
            //{networkname}
            oStrBldr.Append(networkName);
            oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            oStrBldr.Append(currentNodeName);
            oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            //don't make names js compatible until they are actually used in js 
            oStrBldr.Append(currentName);
            oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            oStrBldr.Append(currentId);
            if (string.IsNullOrEmpty(fileExtension))
            {
                fileExtension = NONE;
            }
            oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            oStrBldr.Append(fileExtension);
            return oStrBldr.ToString();
        }
        public static string MakePartialContentURIPattern(string uriPattern,
            string subActionType, string subActionView,
            string variable)
        {
            string sQryParams = string.Empty;
            if (uriPattern != string.Empty
                && subActionType != SERVER_SUBACTION_TYPES.none.ToString())
            {
                StringBuilder bldr = new StringBuilder();
                bldr.Append(uriPattern);
                int iURIPatternLength = GetArrayLength(uriPattern);
                if (iURIPatternLength == 4)
                {
                    //existing route doesn't include a filextension,
                    //add one so that the route has fixed params
                    //crops/input/fert/123/none/respondwithlist/listview
                    bldr.Append(WEBFILE_PATH_DELIMITER);
                    bldr.Append(NONE);
                }
                bldr.Append(WEBFILE_PATH_DELIMITER);
                bldr.Append(subActionType.ToString());
                bldr.Append(WEBFILE_PATH_DELIMITER);
                bldr.Append(subActionView);
                bldr.Append(WEBFILE_PATH_DELIMITER);
                bldr.Append(variable);
                sQryParams = bldr.ToString();
            }
            return sQryParams;
        }
        public static string MakeContentURIPatternFull(
            ContentURI uri, string controller,
            string action, string uriPattern, string subActionType,
            string subActionView = "", string variable = "")
        {
            string sContentURIPattern = string.Empty;
            if (!string.IsNullOrEmpty(controller))
            {
                StringBuilder bldr = new StringBuilder();
                bldr.Append(uri.URIDataManager.DefaultWebDomain);
                bldr.Append(MakeContentURIPattern(controller,
                    action, uriPattern, subActionType,
                    subActionView, variable));
                sContentURIPattern = bldr.ToString();
            }
            return sContentURIPattern;
        }
        public static string MakeContentURIPattern(
            string controller,
            string action, string uriPattern, string subActionType,
            string subActionView, string variable)
        {
            string sContentURIPattern = string.Empty;
            if (!string.IsNullOrEmpty(controller))
            {
                StringBuilder bldr = new StringBuilder();
                bldr.Append(controller);
                bldr.Append(WEBFILE_PATH_DELIMITER);
                bldr.Append(action);
                bldr.Append(WEBFILE_PATH_DELIMITER);
                bldr.Append(MakePartialContentURIPattern(uriPattern,
                    subActionType, subActionView, variable));
                sContentURIPattern = bldr.ToString();
            }
            return sContentURIPattern;
        }
        public static int GetArrayLength(string array)
        {
            int iArrayLength = 0;
            if (!string.IsNullOrEmpty(array))
            {
                string[] arr = array.Split(WEBFILE_PATH_DELIMITERS);
                iArrayLength = arr.Length;
            }
            return iArrayLength;
        }
        public static void SetFullURIs(ContentURI uri)
        {
            //tell client what uri (address in address bar) can be used to return here
            string sNetworkGroupName = (uri.URINetwork.NetworkClass != null)
                ? uri.URINetwork.NetworkClass.NetworkClassName: NONE;
            string sNetworkPartName = uri.URINetworkPartName;
            //make sure that all uris are unique
            //refer to DataAccess.Network for an explanation
            string sAdminNetworkGroupName = GetDefaultNetworkGroupName();
            string sAdminNetworkPartName = GetDefaultNetworkPartName();
            string sURIPattern = string.Empty;
            if (uri.URIDataManager.AppType == APPLICATION_TYPES.accounts
                || uri.URIDataManager.AppType == APPLICATION_TYPES.addins
                || uri.URIDataManager.AppType == APPLICATION_TYPES.locals
                || uri.URIDataManager.AppType == APPLICATION_TYPES.members)
            {
                //note that networks app doesn't use admins
                sURIPattern = MakeURIPattern(uri.URIName,
                    uri.URIId.ToString(), sAdminNetworkPartName,
                    uri.URINodeName, string.Empty);
                uri.URIFull = MakeFullURI(uri,
                    sAdminNetworkGroupName,
                    uri.URIDataManager.ServerActionType,
                    sURIPattern);
                if (!string.IsNullOrEmpty(sAdminNetworkGroupName))
                {
                    //2.0.0: switch to the correct networkgroup when building 
                    //links during html construction
                    uri.URIDataManager.ControllerName = sAdminNetworkGroupName;
                }
            }
            else
            {
                //services apps
                uri.URIFull = MakeFullURI(uri,
                    sNetworkGroupName,
                    uri.URIDataManager.ServerActionType,
                    uri.URIPattern);
                if (!string.IsNullOrEmpty(sNetworkGroupName))
                {
                    //2.0.0: switch to the correct networkgroup when building 
                    //links during html construction (i.e. don't run agtreks calculators from greentreks)
                    uri.URIDataManager.ControllerName = sNetworkGroupName;
                }
            }
            string sName = Helpers.GeneralHelpers.NONE;
            if (uri.URIMember.Member != null) 
            {
                sName = (string.IsNullOrEmpty(uri.URIMember.Member.MemberLastName))
                    ? Helpers.GeneralHelpers.NONE : uri.URIMember.Member.MemberLastName;
            }
            sURIPattern = MakeURIPattern(sName,
                uri.URIMember.MemberId.ToString(), sAdminNetworkPartName,
                AppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString(), string.Empty);
            uri.URIMember.URIFull = MakeFullURI(uri,
                sAdminNetworkGroupName, SERVER_ACTION_TYPES.member,
                sURIPattern);
            sName = Helpers.GeneralHelpers.NONE;
            if (uri.URIMember.ClubInUse != null)
            {
                sName = (string.IsNullOrEmpty(uri.URIMember.ClubInUse.AccountName))
                    ? Helpers.GeneralHelpers.NONE : uri.URIMember.ClubInUse.AccountName;

                sURIPattern = MakeURIPattern(sName,
                    uri.URIMember.ClubInUse.PKId.ToString(), sAdminNetworkPartName,
                    AppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString(), string.Empty);
                uri.URIMember.ClubInUse.URIFull = MakeFullURI(uri,
                    sAdminNetworkGroupName, SERVER_ACTION_TYPES.member,
                    sURIPattern);
            }
            sName = Helpers.GeneralHelpers.NONE;
            if (uri.URIMember.ClubDefault != null)
            {
                sName = (string.IsNullOrEmpty(uri.URIMember.ClubDefault.AccountName))
                    ? Helpers.GeneralHelpers.NONE : uri.URIMember.ClubDefault.AccountName;
                sURIPattern = MakeURIPattern(sName,
                    uri.URIMember.ClubDefault.PKId.ToString(), sAdminNetworkPartName,
                    AppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString(), string.Empty);
                uri.URIMember.ClubDefault.URIFull = MakeFullURI(uri,
                    sAdminNetworkGroupName, SERVER_ACTION_TYPES.member,
                    sURIPattern);
            }
            sURIPattern = MakeURIPattern(uri.URIClub.AccountName,
                uri.URIClub.PKId.ToString(), sAdminNetworkPartName,
                AppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString(), string.Empty);
            uri.URIClub.URIFull = MakeFullURI(uri,
                sAdminNetworkGroupName, SERVER_ACTION_TYPES.member,
                sURIPattern);
            //note that networks app doesn't use admins
            sURIPattern = MakeURIPattern(uri.URINetwork.NetworkName,
                uri.URINetwork.PKId.ToString(), sNetworkPartName,
                AppHelpers.Networks.NETWORK_TYPES.network.ToString(), string.Empty);
            uri.URINetwork.URIFull = MakeFullURI(uri,
                sNetworkGroupName, uri.URIDataManager.ServerActionType,
                sURIPattern);
        }
        
        public static string MakeFullURI(ContentURI uri, string networkGroupName,
           SERVER_ACTION_TYPES serverActionType, string uriPattern)
        {
            string sFullURI = string.Empty;
            StringBuilder oStrBldr = new StringBuilder();
            oStrBldr.Append(uri.URIDataManager.DefaultWebDomain);
            //oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            oStrBldr.Append(networkGroupName);
            oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            oStrBldr.Append(serverActionType.ToString());
            oStrBldr.Append(WEBFILE_PATH_DELIMITER);
            oStrBldr.Append(uriPattern);
            sFullURI = oStrBldr.ToString();
            return sFullURI;
        }
        //public static string GetContentURIPatternFromFullURI(
        //    ContentURI uri, string fullURIPath)
        //{
        //    string sContentURIPattern = string.Empty;
        //    if (!string.IsNullOrEmpty(fullURIPath))
        //    {
        //        sContentURIPattern = fullURIPath.Replace(GetWWWDEVTREKS(uri), string.Empty);
        //        if (sContentURIPattern.StartsWith(WEBFILE_PATH_DELIMITER))
        //        {
        //            sContentURIPattern 
        //                = sContentURIPattern.TrimStart(WEBFILE_PATH_DELIMITERS);
        //        }
        //    }
        //    return sContentURIPattern;
        //}
        
        //public static void ReplaceCommonNameWithRandomName(string uriorfilePattern, 
        //    ref string filePathName)
        //{
        //    ContentURI uri = ContentURI.ConvertShortURIPattern(uriorfilePattern);
        //    int iIndex = uriorfilePattern.IndexOf(WEBFILE_PATH_DELIMITER);
        //    if (iIndex == -1)
        //    {
        //        //filename to convert to typical uripattern
        //        uri.ChangeURIPatternFromFileName(uriorfilePattern);
        //    }
        //    if (uri.URIName != string.Empty)
        //    {
        //        filePathName = filePathName.Replace(uri.URIName, 
        //            string.Concat(uri.URIName, GetRandomInteger(0).ToString()));
        //    }
        //}
        
        public static void GetDefaultNetworkSettings(out int networkId,
            out string networkName, out string networkPartName,
            out int networkGroupId, out string networkGroupName)
        {
            //ok to hardcode since default connection strings also hardcoded
            //commontreks is default controller
            //allnetworks is default network
            networkId = 0;
            networkName = "All Network";
            networkPartName = GetDefaultNetworkPartName();
            networkGroupId = 1;
            networkGroupName = GetDefaultNetworkGroupName();
        }
        public static string GetDefaultNetworkGroupName()
        {
            string sNetworkGroupName = "commontreks";
            return sNetworkGroupName;
        }
        public static string GetDefaultNetworkGroupName(ContentURI uri)
        {
            string sNetworkGroupName = uri.URIDataManager.ContentURIName;
            return sNetworkGroupName;
        }
        public static string GetDefaultNetworkPartName()
        {
            string sNetworkPartName = "allnetworks";
            return sNetworkPartName;
        }
        
        public static DateTime GetDateShortNow()
        {
            DateTime oToday = DateTime.Parse(DateTime.Today.ToShortDateString());
            return oToday;
        }
        public static DateTime GetDateSortOld()
        {
            DateTime oToday = DateTime.Parse("1901-12-31");
            return oToday;
        }
        public static DateTime GetDateUniversalNow()
        {
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToUniversalTime().ToString());
            return dtNow;
        }
        public static string ChangeFileNameInPath(string startingFilePath, string newFileName)
        {
            string sNewFilePath = string.Empty;
            if (!string.IsNullOrEmpty(startingFilePath))
            {
                string sFileName = Path.GetFileName(startingFilePath);
                if (!string.IsNullOrEmpty(sFileName))
                {
                    sNewFilePath = startingFilePath.Replace(sFileName, newFileName);
                }
            }
            return sNewFilePath;
        }
        
        public static string RemoveStartingDelimiter(string path)
        {
            string sNewPath = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith(FILE_PATH_DELIMITER))
                {
                    sNewPath = path.Substring(1);
                }
                else if (path.StartsWith(WEBFILE_PATH_DELIMITER))
                {
                    sNewPath = path.Substring(1);
                }
            }
            return sNewPath;
        }
        
        
        public static int GetRandomInteger(Random rnd)
        {
            int iRandomInt = 0;
            //if the optional seed = 0 use a time dependent seed
            //the new Random() produces the same number if called in quick succession
            //avoid by passing the new rnd at the start of the loop 
            iRandomInt = rnd.Next();
            return iRandomInt;
        }
        public static int Get2RandomInteger()
        {
            int iRandomInt = 0;
            //if the optional seed = 0 use a time dependent seed
            //the new Random() produces the same number if called in quick succession
            //avoid by passing the i for the loop as the optional seed
            Random randObj = new Random();
            iRandomInt = randObj.Next();
            iRandomInt += GetRandomInteger(randObj);
            return iRandomInt;
        }
        public static int GetRandomInteger(int optionalSeed)
        {
            int iRandomInt = 0;
            //if the optional seed = 0 use a time dependent seed
            //the new Random() produces the same number if called in quick succession
            //avoid by passing the i for the loop as the optional seed
            Random randObj = (optionalSeed != 0) ? new Random(optionalSeed) : new Random();
            iRandomInt = randObj.Next();
            return iRandomInt;
        }
        public static Guid GetGuid()
        {
            Guid gwid = Guid.NewGuid();
            return gwid;
        }
        public static void MakeCollectionFromStandardSelectsArray(
            ref IDictionary<string, string> uriPatterns,
            string uriPatternArray)
        {
            string[] arrNames = uriPatternArray.Split(PARAMETER_DELIMITERS);
            int i = 0;
            if (arrNames != null)
            {
                int iLength = arrNames.Length;
                string sChildURIPattern = string.Empty;
                string sParentURIPattern = string.Empty;
                string sValue = string.Empty;
                string sSearchName = string.Empty;
                for (i = 0; i < iLength; i++)
                {
                    sSearchName = arrNames[i];
                    EditHelpers.AddHelperLinq.GetChildParentURIPatterns
                        (sSearchName, out sChildURIPattern, out sParentURIPattern);
                    sValue = ContentURI.GetURIPatternPart(sChildURIPattern, 
                        ContentURI.URIPATTERNPART.name);
                    if (!uriPatterns.ContainsKey(sSearchName))
                    {
                        uriPatterns.Add(sSearchName, sValue);
                    }
                }
            }
        }
        public static string MakeDelimitedStringFromDictionary(
            IDictionary<string, string> uriPatterns)
        {
            StringBuilder arrString = new StringBuilder();
            if (uriPatterns != null)
            {
                //standard uripattern dictionary in devtreks
                foreach (KeyValuePair<string, string> kvp in uriPatterns)
                {
                    arrString.Append(kvp.Key);
                    arrString.Append(STRING_DELIMITERS);
                    arrString.Append(kvp.Value);
                    arrString.Append(PARAMETER_DELIMITERS);
                }
            }
            return arrString.ToString();
        }
        public static IDictionary<string, string> MakeDictionaryFromDelimitedString(
             string dictionary)
        {
            IDictionary<string, string> uriPatterns = null;
            if (!string.IsNullOrEmpty(dictionary))
            {
                string[] arrNames = dictionary.Split(PARAMETER_DELIMITERS);
                int i = 0;
                if (arrNames != null)
                {
                    uriPatterns = new Dictionary<string, string>();
                    int iLength = arrNames.Length;
                    string sFirstURIPattern = string.Empty;
                    string sSecondURIPattern = string.Empty;
                    string sNames = string.Empty;
                    for (i = 0; i < iLength; i++)
                    {
                        sNames = arrNames[i];
                        if (!string.IsNullOrEmpty(sNames))
                        {
                            EditHelpers.AddHelperLinq.GetChildParentURIPatterns
                                (sNames, out sFirstURIPattern, out sSecondURIPattern);
                            if (!string.IsNullOrEmpty(sFirstURIPattern)
                                && !string.IsNullOrEmpty(sSecondURIPattern))
                            {
                                uriPatterns.Add(sFirstURIPattern, sSecondURIPattern);
                            }
                        }
                    }
                }
            }
            return uriPatterns;
        }
        public static string GetDelimitedSubstring(string fullString, char[] delimiter,
            int substringDelimiterPosition)
        {
            string sSubstring = string.Empty;
            //plus one because delimiter not wanted
            if (fullString != string.Empty)
            {
                string[] aSubStrings = fullString.Split(delimiter);
                if (aSubStrings.Length > substringDelimiterPosition) sSubstring = aSubStrings[substringDelimiterPosition];
            }
            return sSubstring;
        }
        public static string GetStringFromStream(Stream streamBody)
        {
            string sArgs = string.Empty;
            if (streamBody != null)
            {
                if (streamBody.CanRead)
                {
                    StringBuilder oStringBuilder = new StringBuilder();
                    int iStreamLength = Convert.ToInt32(streamBody.Length);
                    int iReadLength = 0;
                    byte[] arrBody = new byte[iStreamLength];
                    iReadLength = streamBody.Read(arrBody, 0, iStreamLength);
                    streamBody.Dispose();
                    //streamBody.Close();
                    //encode the byte to ascii chars
                    ASCIIEncoding AE = new ASCIIEncoding();
                    char[] arrChars = AE.GetChars(arrBody);
                    //encode the byte to unicode chars
                    //UnicodeEncoding UC = new UnicodeEncoding();
                    //char[] arrChars = UC.GetChars(arrBody);
                    int x = 0;
                    int iCharLength = arrChars.Length;
                    for (x = 0; x < iCharLength; x++)
                    {
                        oStringBuilder.Append(arrChars[x].ToString());
                    }
                    sArgs = oStringBuilder.ToString();
                }
            }
            return sArgs;
        }
        public static string GetDirectoryPathOrName(string searchName,
            string docPath, bool needsNameOnly)
        {
            string sDirectoryPath = string.Empty;
            string sDelimiter = FileStorageIO.GetDelimiterForFileStorage(docPath);
            int iStartIndex
                = docPath.IndexOf(searchName);
            if (iStartIndex != -1)
            {
                int iEndIndex
                    = docPath.IndexOf(sDelimiter, iStartIndex);
                if (iEndIndex != -1)
                {
                    if (needsNameOnly)
                    {
                        sDirectoryPath = docPath.Substring(
                            iStartIndex, iEndIndex - iStartIndex);
                    }
                    else
                    {
                        sDirectoryPath = docPath.Substring(
                           0, iEndIndex);
                    }
                }
            }
            return sDirectoryPath;
        }
        public static string GetLastSubString(string delimitedString, string delimiter)
        {
            string sSubstring = string.Empty;
            if (delimitedString != string.Empty && delimitedString != null)
            {
                int iParamIndex = delimitedString.LastIndexOf(delimiter);
                if (iParamIndex > 0)
                {
                    int iSubstringLength = delimitedString.Length - iParamIndex - 1;
                    sSubstring = delimitedString.Substring(iParamIndex + 1, iSubstringLength);
                }
                else
                {
                    sSubstring = delimitedString;
                }
            }
            return sSubstring;
        }

        public static APPLICATION_TYPES ConvertAppTypeStringToEnum(string appType)
        {
            APPLICATION_TYPES eAppType = (appType != string.Empty && appType != null) ? (APPLICATION_TYPES)Enum.Parse(typeof(APPLICATION_TYPES), appType) : APPLICATION_TYPES.none;
            return eAppType;
        }
        public static SUBAPPLICATION_TYPES ConvertSubAppTypeStringToEnum(string subAppType)
        {
            SUBAPPLICATION_TYPES eSubAppType = (subAppType != string.Empty && subAppType != null) ? (SUBAPPLICATION_TYPES)Enum.Parse(typeof(SUBAPPLICATION_TYPES), subAppType) : SUBAPPLICATION_TYPES.none;
            return eSubAppType;
        }
        public static bool IsSubApp(string currentId)
        {
            bool bIsSubApp = false;
            int iURIId = (currentId != string.Empty) ? Convert.ToInt32(currentId) : 0;
            foreach (int i in Enum.GetValues(typeof(SUBAPPLICATION_TYPES)))
            {
                if (i == iURIId)
                {
                    bIsSubApp = true;
                    return bIsSubApp;
                }
            }
            return bIsSubApp;
        }
        public static string GetSubString(int startIndex, string delimitedString, string delimiter)
        {
            string sSubstring = string.Empty;
            if (!string.IsNullOrEmpty(delimitedString))
            {
                int iParamIndex = delimitedString.IndexOf(delimiter);
                if (iParamIndex > 0
                    && startIndex < iParamIndex)
                {
                    sSubstring = delimitedString.Substring(startIndex, iParamIndex);
                }
                else
                {
                    sSubstring = delimitedString;
                }
            }
            return sSubstring;
        }
        public static bool IsAdminApp(APPLICATION_TYPES appType)
        {
            bool bIsAdminApp = false;
            if (appType == APPLICATION_TYPES.accounts
                || appType == APPLICATION_TYPES.addins
                || appType == APPLICATION_TYPES.agreements
                || appType == APPLICATION_TYPES.members
                || appType == APPLICATION_TYPES.networks
                || appType == APPLICATION_TYPES.locals)
            {
                bIsAdminApp = true;
            }
            return bIsAdminApp;
        }
        public static bool IsEditSubServerAction(ContentURI uri)
        {
             bool bIsEdit = false;
             if (uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.adddefaults
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.submitedits
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.submitlistedits
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.submitformedits
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.runaddin
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.savenewselects
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.saveselects
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.buildtempdoc
                     || uri.URIDataManager.ServerSubActionType
                     == SERVER_SUBACTION_TYPES.uploadfile)
             {
                 bIsEdit = true;
             }
             return bIsEdit;
        }
        public static bool IsDbEdit(ContentURI uri)
        {
            bool bIsDbInsert = false;
            //refactor: see if it can be kept this simple
            if (uri.URIFileExtensionType == FILENAME_EXTENSIONS.temp.ToString())
            {
                bIsDbInsert = false;
            }
            else if (uri.URIPattern.EndsWith(FILENAME_EXTENSIONS.temp.ToString()))
            {
                bIsDbInsert = false;
            }
            else
            {
                //keep it this simple here -if needed, enforce additional rules at app level 
                if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel ==
                    AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    bIsDbInsert = true;
                }
            }
            return bIsDbInsert;
        }
        
        public static string MakeNodeXPath(string nodeName, string attributeName, string attributeValue)
        {
            string sXPath = string.Empty;
            StringBuilder oString = new StringBuilder();
            if (nodeName != string.Empty)
            {
                oString.Append(XPATH_START);
                oString.Append(nodeName);
                oString.Append("[");
                oString.Append(attributeName);
                oString.Append("='");
                oString.Append(attributeValue);
                oString.Append("']");
            }
            sXPath = oString.ToString();
            return sXPath;
        }
        public static string MakeNodeXPathWithXmlDoc(string nodeName, string attributeName,
            string attributeValue)
        {
            string sXPath = string.Empty;
            StringBuilder oString = new StringBuilder();
            if (nodeName != string.Empty)
            {
                oString.Append(XPATH_START);
                oString.Append(nodeName);
                oString.Append("[");
                oString.Append(attributeName);
                oString.Append("='");
                oString.Append(attributeValue);
                oString.Append("']");
                oString.Append(WEBFILE_PATH_DELIMITER);
                oString.Append(ROOT_PATH);
            }
            sXPath = oString.ToString();
            return sXPath;
        }
        public static void GetParentPathandLastSubstring(string currentPath, string pathDelimiter, out string pathParent, out string lastSubstring)
        {
            pathParent = string.Empty;
            lastSubstring = string.Empty;
            int iLastIndex = -1;
            int iLength = -1;
            iLastIndex = currentPath.LastIndexOf(pathDelimiter);
            if (iLastIndex > 0)
            {
                pathParent = currentPath.Substring(0, iLastIndex);
                iLength = currentPath.Length;
                int iNewLastIndex = iLastIndex + 1;
                if (iNewLastIndex < iLength)
                {
                    lastSubstring = currentPath.Substring(iNewLastIndex);
                }
            }
        }
        
        public static ContentURI GetParentAncestor(ContentURI uri)
        {
            int iCount = uri.URIDataManager.Ancestors.Count;
            ContentURI oParentURI = (iCount >= 1) ? uri.URIDataManager.Ancestors[iCount - 1] : null;
            return oParentURI;
        }
        public static void GetParentIdAndNodeName(ContentURI uri, out int parentId, out string parentNodename)
        {
            parentId = 0;
            parentNodename = string.Empty;
            parentNodename = ContentURI.GetURIPatternPart(uri.URIDataManager.ParentURIPattern,
                ContentURI.URIPATTERNPART.node);
            string sParentId = ContentURI.GetURIPatternPart(uri.URIDataManager.ParentURIPattern,
                ContentURI.URIPATTERNPART.id);
            parentId = Helpers.GeneralHelpers.ConvertStringToInt(sParentId);
            if (parentId <= 0 || string.IsNullOrEmpty(parentNodename))
            {
                uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "INSERT_NOPARENT");
            }
        }
        public static string AddDocExtensionToPath(string docPath,
            FILENAME_EXTENSIONS fileExtType)
        {
            string sDocPathWithExtension = string.Empty;
            //2.0.0 added null or empty condition
            if (fileExtType == FILENAME_EXTENSIONS.none
                || string.IsNullOrEmpty(fileExtType.ToString()))
            {
                if (docPath.EndsWith(EXTENSION_XML) == false)
                {
                    sDocPathWithExtension = string.Concat(docPath, EXTENSION_XML);
                }
            }
            else
            {
                string sExt = string.Concat(FILENAME_DELIMITER, fileExtType.ToString(), EXTENSION_XML);
                if (docPath.EndsWith(EXTENSION_XML) == false)
                {
                    sDocPathWithExtension = string.Concat(docPath, sExt);
                }
                else
                {
                    sDocPathWithExtension = docPath.Replace(EXTENSION_XML, sExt);
                }
            }
            return sDocPathWithExtension;
        }
        /// <summary>
        /// Retrieve a starting full sub string from a full string 
        /// based on the name passed in
        /// </summary>
        /// <param name="fullString"></param>
        /// <param name="nameToFind"></param>
        /// <param name="fullSubString"></param>
        public static void GetStartSubstring(string fullString,
            string nameToFind, out string fullSubString)
        {
            fullSubString = string.Empty;
            //typically used to retrieve ancestor file paths 
            //nameToFile = directory of current uri, string.concat(uri.URINodeName, "_", uri.URIId)
            int iStartIndex = fullString.LastIndexOf(nameToFind);
            if (iStartIndex >= 0)
            {
                //add the name and the path delimiter
                int iFullIndex = iStartIndex + nameToFind.Length + 1;
                fullSubString = fullString.Substring(0, iFullIndex);
            }
            else
            {
                //return the full string
                fullSubString = fullString;
            }
        }
        public static void GetSubstringsSeparateLast(string fullString,
            string pathDelimiter, out string newFullString, 
            out string lastSubstring)
        {
            newFullString = string.Empty;
            lastSubstring = string.Empty;
            int iLastIndex = -1;
            int iLength = -1;
            iLastIndex = fullString.LastIndexOf(pathDelimiter);
            if (iLastIndex > 0)
            {
                newFullString = fullString.Substring(0, iLastIndex);
                iLength = fullString.Length;
                int iNewLastIndex = iLastIndex + 1;
                if (iNewLastIndex < iLength)
                {
                    lastSubstring = fullString.Substring(iNewLastIndex);
                }
            }
        }
    
        public static void CopyDirectories(ContentURI uri, 
            string fromFilePath, string toFilePath, bool needsNewSubDirectories)
        {
            string sFromFilePath = string.Empty;
            string sFromFileName = string.Empty;
            string sDelimiter = (Path.IsPathRooted(fromFilePath)) ?
            FILE_PATH_DELIMITER : WEBFILE_PATH_DELIMITER;
            GetParentPathandLastSubstring(fromFilePath, sDelimiter,
                out sFromFilePath, out sFromFileName);
            string sToFilePath = string.Empty;
            string sToFileName = string.Empty;
            GetParentPathandLastSubstring(toFilePath, sDelimiter,
                out sToFilePath, out sToFileName);
            bool bCopySubDirectories = true;
            FileStorageIO.CopyDirectories(uri, sFromFilePath, sToFilePath, 
                bCopySubDirectories, needsNewSubDirectories);
        }
        public static async Task<bool> CopyDirectoriesAsync(
            ContentURI uri, string fromFilePath,
            string toFilePath, bool needsNewSubDirectories)
        {
            bool bHasCopied = false;

            string sFromFilePath = FileStorageIO.GetDirectoryName(fromFilePath);
            string sToFilePath = FileStorageIO.GetDirectoryName(toFilePath); 
            bool bCopySubDirectories = true;
            bHasCopied = await FileStorageIO.CopyDirectoriesAsync(
                uri, sFromFilePath, sToFilePath,
                bCopySubDirectories, needsNewSubDirectories);
            return bHasCopied;
        }
        public static string ReplaceFormElementParam(string formElParams,
            string formElParamName, string replaceParamValue)
        {
            string sNewFormElParams = formElParams;
            string sFormElemParam = GetFormElementParam(formElParams,
                formElParamName, string.Empty);
            if (!string.IsNullOrEmpty(sFormElemParam))
            {
                string sOldFormEl = MakeFormElement(formElParamName, sFormElemParam);
                string sNewFormEl = MakeFormElement(formElParamName, replaceParamValue);
                sNewFormElParams = formElParams.Replace(sOldFormEl, sNewFormEl);
            }
            return sNewFormElParams;
        }
        public static string GetFormElementParam(string formElParams,
            string formElParamName, string defaultParamValue)
        {
            string sFormElParam = string.Empty;
            if (!string.IsNullOrEmpty(formElParams))
            {
                string sFormElName = string.Concat(formElParamName, "=");
                int iStartIndex
                    = formElParams.IndexOf(sFormElName);
                if (iStartIndex > -1)
                {
                    sFormElParam = GetFormValue(sFormElName, formElParams, iStartIndex);
                }
                else
                {
                    //search using formElParamName only (i.e. dbkey name delimited strings)
                    iStartIndex = formElParams.IndexOf(formElParamName);
                    if (iStartIndex > -1)
                    {
                        iStartIndex = formElParams.IndexOf("=", iStartIndex);
                        sFormElParam = GetFormValue("=", formElParams, iStartIndex);
                    }
                }
            }
            if (sFormElParam.Length < 1)
            {
                sFormElParam = defaultParamValue;
            }
            return sFormElParam;
        }
        private static string GetFormValue(string formElParamName, string formElParams, 
            int startIndex)
        {
            string sFormElParam = string.Empty;
            startIndex += formElParamName.Length;
            int iLastIndex = formElParams.IndexOf("&", startIndex);
            if (iLastIndex > -1)
            {
                sFormElParam = formElParams.Substring(startIndex, iLastIndex - startIndex);
            }
            else
            {
                sFormElParam = formElParams.Substring(startIndex);
            }
            return sFormElParam;
        }
        public static string GetFormValue(ContentURI uri, string name)
        {
            string sValue = string.Empty;
            sValue = GetFormValue(uri.URIDataManager.FormInput, name);
            return sValue;
        }
        public static string GetFormValue(Dictionary<string, string> formParams, 
            string name)
        {
            string sValue = string.Empty;
            if (formParams != null)
            {
                formParams.TryGetValue(name, out sValue);
                //sValue = uri.URIDataManager.FormInput[name];
                if (sValue == "undefined") sValue = string.Empty;
                if (sValue == null) sValue = string.Empty;
                if (sValue.EndsWith("*")) sValue =
                    sValue.Remove(sValue.Length - 1);
            }
            return sValue;
        }
        public static void AddNVCollectionToFormInput(ContentURI uri,
            NameValueCollection postBackParams)
        {
            if (postBackParams != null)
            {
                if (uri.URIDataManager.FormInput == null)
                {
                    uri.URIDataManager.FormInput = new Dictionary<string, string>();
                }
                string sValue = string.Empty;
                foreach (string key in postBackParams)
                {
                    sValue = postBackParams[key];
                    if (sValue != null)
                        uri.URIDataManager.FormInput.Add(key, sValue);
                }
            }
        }
        
        public static void AddFormInput(ContentURI uri,
            string formInputName, string formInputValue)
        {
            if (uri.URIDataManager.FormInput != null)
            {
                uri.URIDataManager.FormInput.Add(formInputName, formInputValue);
            }
        }
        public static string GetTerminalNode(ContentURI uri)
        {
            //used by tempdocs to get detailed view of tempdoc
            string sTerminalNode = uri.URINodeName;
            if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.budgets)
            {
                sTerminalNode 
                    = AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.investments)
            {
                sTerminalNode
                    = AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.outcomeprices)
            {
                sTerminalNode
                    = AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.operationprices)
            {
                sTerminalNode
                    = AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.componentprices)
            {
                sTerminalNode
                    = AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.inputprices)
            {
                sTerminalNode
                    = AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.outputprices)
            {
                sTerminalNode
                    = AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.linkedviews)
            {
                sTerminalNode
                    = AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.devpacks)
            {
                sTerminalNode
                    = AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString();
            }
            else if (uri.URIDataManager.SubAppType
                == SUBAPPLICATION_TYPES.resources)
            {
                sTerminalNode
                    = AppHelpers.Resources.RESOURCES_TYPES.resource.ToString();
            }
            return sTerminalNode;
        }
        public static bool IsTerminalFolder(string folderName)
        {
            bool bIsTerminalNode = false;
            string sFolderNodeName = GetSubstringFromFront(folderName,
                FILENAME_DELIMITERS, 1);
            //note that price analysis should generally use the input or output node -that document contains price series
            if (sFolderNodeName.Equals(AppHelpers.Economics1.BUDGET_TYPES.budgettimeperiod.ToString())
                || sFolderNodeName.Equals(AppHelpers.Economics1.INVESTMENT_TYPES.investmenttimeperiod.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.OUTCOME_PRICE_TYPES.outcome.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.OPERATION_PRICE_TYPES.operation.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.COMPONENT_PRICE_TYPES.component.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString())
                || sFolderNodeName.Equals(AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
                || sFolderNodeName.Equals(AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                || sFolderNodeName.Equals(AppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString()))
            {
                bIsTerminalNode = true;
            }
            return bIsTerminalNode;
        }
        public static void ChangeFileExtension(
            AppHelpers.Resources.FILEEXTENSION_TYPES newFileExtType, 
            ref string filePath)
        {
            string sOldExtension = Path.GetExtension(filePath);
            if (sOldExtension != string.Empty)
            {
                filePath = filePath.Replace(sOldExtension, 
                    string.Concat(".", newFileExtType.ToString()));
            }
            else
            {
                if (!filePath.EndsWith("."))
                {
                    filePath += string.Concat(".", newFileExtType.ToString());
                }
                else
                {
                    filePath += newFileExtType.ToString();
                }
            }
        }
        
        public static string GetViewEditType(ContentURI uri,
            DOC_STATE_NUMBER displayDocType)
        {
            string sViewEditType = string.Empty;
            if (uri.URIDataManager.UpdatePanelType ==
                UPDATE_PANEL_TYPES.preview)
            {
                sViewEditType = uri.URIDataManager.PreviewViewEditType.ToString();
            }
            else if (uri.URIDataManager.UpdatePanelType ==
                UPDATE_PANEL_TYPES.linkedviews)
            {
                if (displayDocType == DOC_STATE_NUMBER.seconddoc
                    && uri.URIDataManager.ServerSubActionType
                    == SERVER_SUBACTION_TYPES.runaddin)
                {
                    sViewEditType = uri.URIDataManager.EditViewEditType.ToString();
                }
                else
                {
                    if (displayDocType == DOC_STATE_NUMBER.seconddoc)
                    {
                        //0.8.7 when this is full it saves a click to init calc buttons
                        sViewEditType = uri.URIDataManager.EditViewEditType.ToString();
                    }
                    else
                    {
                        //only print views in thirddocs
                        sViewEditType = VIEW_EDIT_TYPES.print.ToString();
                    }
                }
            }
            else if (uri.URIDataManager.UpdatePanelType ==
                UPDATE_PANEL_TYPES.select)
            {
                sViewEditType = uri.URIDataManager.SelectViewEditType.ToString();
            }
            else
            {
                sViewEditType = uri.URIDataManager.EditViewEditType.ToString();
            }
            return sViewEditType;
        }
        public static void SaveTempDocsSelectsList(ContentURI uri)
        {
            //selectionlist is parameter delimited string of
            //childuripattern;parenturipattern strings
            if (uri.URIFileExtensionType
                == FILENAME_EXTENSIONS.temp.ToString()
                && (!(string.IsNullOrEmpty(uri.URIDataManager.SelectedList))))
            {
                //tempdocs need to be able to identify the resources associated 
                //with the nodes added when packaged
                //save the selects list in the tempdoc path
                string sSelectsListPath
                    = GetSelectListFullFilePath(uri.URIMember.MemberDocFullPath);
                FileStorageIO oFileStorageIO = new FileStorageIO();
                string sErrorMsg = string.Empty;
                oFileStorageIO.SaveTextURI(uri, sSelectsListPath, uri.URIDataManager.SelectedList, 
                    out sErrorMsg);
                uri.ErrorMessage = sErrorMsg;
            }
        }
        public static string[] GetTempDocFilePathsSelectsList(
            ContentURI uri, string tempDocsFilePath)
        {
            string sSelectsListPath
                = GetSelectListFullFilePath(tempDocsFilePath);
            FileStorageIO oFileStorageIO = new FileStorageIO();
            string sErrorMsg = string.Empty;
            string sSelectList = oFileStorageIO.ReadText(
                uri, sSelectsListPath, out sErrorMsg);
            string[] arrSelectList = { };
            if (!string.IsNullOrEmpty(sSelectList))
            {
                arrSelectList = sSelectList.Split(PARAMETER_DELIMITERS);
            }
            return arrSelectList;
        }
        //public static string[] GetTempDocsSelectsList(
        //    ContentURI uri, string tempDocPath)
        //{
        //    FileStorageIO oFileStorageIO = new FileStorageIO();
        //    string sErrorMsg = string.Empty;
        //    string sSelectList = oFileStorageIO.ReadText(
        //        uri, tempDocPath, out sErrorMsg);
        //    string[] arrSelectList = { };
        //    if (!string.IsNullOrEmpty(sSelectList))
        //    {
        //        arrSelectList = sSelectList.Split(PARAMETER_DELIMITERS);
        //    }
        //    return arrSelectList;
        //}
        private static string GetSelectListFullFilePath(string tempDocsFilePath)
        {
            //selects lists are used with tempdocs
            //tempdocs use memberpaths
            string sSelectListFileName = string.Concat(FILENAME_EXTENSIONS.selectlist.ToString(),
                FILEEXTENSION_DELIMITER, AppHelpers.Resources.FILEEXTENSION_TYPES.txt.ToString());
            string sSelectsListPath = tempDocsFilePath.Replace(
                Path.GetFileName(tempDocsFilePath), sSelectListFileName);
            return sSelectsListPath;
        }
        public static bool IsRootChild(string nodeName)
        {
            bool bIsRootChild = false;
            if (nodeName.EndsWith("group")
                || nodeName
                == AppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString()
                || nodeName 
                == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                //first child of all root elements in devtreks xmldocs 
                //are always either grouping elements
                //or, in the case of serviceagreements, serviceaccount elements
                bIsRootChild = true;
            }
            return bIsRootChild;
        }
        public static string MakeFormElement(string name, string value)
        {
            string sFormElement = string.Empty;
            sFormElement = string.Concat(FORMELEMENT_DELIMITER,
                name, EQUALS, value);
            return sFormElement;
        }
        public static string MakeFormHTMLElement(string name, string value)
        {
            string sFormElement = string.Empty;
            sFormElement = string.Concat(FORMELEMENT_HTMLDELIMITER,
                name, EQUALS, value);
            return sFormElement;
        }
        public static string MakeDataViewFormParam(SERVER_SUBACTION_TYPES subActionType, 
            string listName)
        {
            string sListViewParam = string.Empty;
            sListViewParam = MakeFormElement("subaction", subActionType.ToString());
            sListViewParam += MakeFormElement("subactionview", listName);
            return sListViewParam;
        }
        

        public static string GetDataViewFormView(SERVER_SUBACTION_TYPES subActionType, ContentURI uri)
        {
            string sListViewValue = string.Empty;
            sListViewValue = GetFormValue(uri, subActionType.ToString());
            return sListViewValue;
        }
        public static string GetDefaultParams(string parentNodeURIPattern, 
            string defaultNodeURIPattern)
        {
            string sDefaultParams = string.Concat(
                MakeFormHTMLElement(EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.parentnode.ToString(), parentNodeURIPattern),
                MakeFormHTMLElement(EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.defaultnode.ToString(), defaultNodeURIPattern));
            return sDefaultParams;
        }
        public static CLIENTACTION_TYPES GetClientActionType(string calculatorType)
        {
            CLIENTACTION_TYPES eCLIENTACTION_TYPES
                = (!string.IsNullOrEmpty(calculatorType))
                ? (CLIENTACTION_TYPES)Enum.Parse(
                typeof(CLIENTACTION_TYPES), calculatorType)
                : CLIENTACTION_TYPES.postrequest;
            return eCLIENTACTION_TYPES;
        }
        public static UPDATE_PANEL_TYPES GetUpdatePanelType(string updatePanelType)
        {
            UPDATE_PANEL_TYPES eCLIENTACTION_TYPES
                = (!string.IsNullOrEmpty(updatePanelType))
                ? (UPDATE_PANEL_TYPES)Enum.Parse(
                typeof(UPDATE_PANEL_TYPES), updatePanelType)
                : UPDATE_PANEL_TYPES.none;
            return eCLIENTACTION_TYPES;
        }
        public static SERVER_ACTION_TYPES GetServerActionType(string serverActionType)
        {
            SERVER_ACTION_TYPES eSERVER_ACTION_TYPES
                = (!string.IsNullOrEmpty(serverActionType))
                ? (SERVER_ACTION_TYPES)Enum.Parse(
                typeof(SERVER_ACTION_TYPES), serverActionType)
                : SERVER_ACTION_TYPES.none;
            return eSERVER_ACTION_TYPES;
        }
        public static SERVER_SUBACTION_TYPES GetServerSubActionActionType(
            string serverSubActionType)
        {
            SERVER_SUBACTION_TYPES eSERVER_SUBACTION_TYPES
                = (!string.IsNullOrEmpty(serverSubActionType))
                ? (SERVER_SUBACTION_TYPES)Enum.Parse(
                typeof(SERVER_SUBACTION_TYPES), serverSubActionType)
                : SERVER_SUBACTION_TYPES.none;
            return eSERVER_SUBACTION_TYPES;
        }
        public static APPLICATION_TYPES GetApplicationType(string applicationType)
        {
            APPLICATION_TYPES eAPPLICATION_TYPES
                = (!string.IsNullOrEmpty(applicationType))
                ? (APPLICATION_TYPES)Enum.Parse(
                typeof(APPLICATION_TYPES), applicationType)
                : APPLICATION_TYPES.none;
            return eAPPLICATION_TYPES;
        }
        public static SUBAPPLICATION_TYPES GetSubApplicationType(string subApplicationType)
        {
            SUBAPPLICATION_TYPES eSUBAPPLICATION_TYPES
                = (!string.IsNullOrEmpty(subApplicationType))
                ? (SUBAPPLICATION_TYPES)Enum.Parse(
                typeof(SUBAPPLICATION_TYPES), subApplicationType)
                : SUBAPPLICATION_TYPES.none;
            return eSUBAPPLICATION_TYPES;
        }
        public static VIEW_EDIT_TYPES GetViewEditType(string viewEditType)
        {
            VIEW_EDIT_TYPES eVIEW_EDIT_TYPES
                = (!string.IsNullOrEmpty(viewEditType))
                ? (VIEW_EDIT_TYPES)Enum.Parse(
                typeof(VIEW_EDIT_TYPES), viewEditType)
                : VIEW_EDIT_TYPES.none;
            return eVIEW_EDIT_TYPES;
        }
        public static DOCS_STATUS GetDocStatus(string docStatus)
        {
            DOCS_STATUS eDOCS_STATUS
                = (!string.IsNullOrEmpty(docStatus))
                ? (DOCS_STATUS)Enum.Parse(
                typeof(DOCS_STATUS), docStatus)
                : DOCS_STATUS.notreviewed;
            return eDOCS_STATUS;
        }
        public static bool IsGroupingNode(string nodeName)
        {
            bool bIsGroupingNode = false;
            if (nodeName == AppHelpers.Economics1.BUDGET_TYPES.budgetoutcomes.ToString()
                || nodeName == AppHelpers.Economics1.BUDGET_TYPES.budgetoperations.ToString()
                || nodeName == AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponents.ToString())
            {
                bIsGroupingNode = true;
            }
            return bIsGroupingNode;
        }
    }
}
