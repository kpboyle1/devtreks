using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using GenHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Data
{
    /// <summary>
    ///Purpose:		URI model for loading and editing DevTreks content (i.e. 
    ///             generic collections, xml/xthml docs, images ...)
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    /// NOTES       URIs can be discovered and displayed using the contenturipattern
    ///             route value and the MVC controller and action. 
    ///             This class also uses a DataManager object to store 
    ///             the parameters needed to load, display, and manipulate the uri.
    public class ContentURI
    {
        //The URI patterns (see routes in Startup.cs) are:
        //The MVC route pattern is: controller/action/{contenturipattern}
        //i.e. agtreks/edit/crops/input/10-10-10 Fertilizer/12345/temp 
        //contenturipattern is a variable route with params:
        //"{controller}/{action}/{networkname}/{nodename}/{commonname}/{id}/",
        //"{filenameextensiontype}/{subaction}/{subactionview}/",
        //"{*variable}"
        public static void GetContentURIParams(string contentURIPattern,
            out string controller, out string action,
            out string networkName, out string nodeName, out string name, 
            out string id, out string fileNameExtensionType, out string subAction,
            out string subActionView, out string variable)
        {
            controller = string.Empty;
            action = string.Empty;
            networkName = string.Empty;
            nodeName = string.Empty;
            name = string.Empty;
            id = string.Empty;
            fileNameExtensionType = string.Empty;
            subAction = string.Empty;
            subActionView = string.Empty;
            variable = string.Empty;
            if (string.IsNullOrEmpty(contentURIPattern))
                return;
            string[] routeParams 
                = contentURIPattern.Split(GenHelpers.WEBFILE_PATH_DELIMITERS);
            if (routeParams != null)
            {
                int i = 0;
                for (i = 0; i < routeParams.Length; i++)
                {
                    if (i == 0)
                    {
                        controller = routeParams[i];
                        controller = controller.Replace("*", string.Empty);
                    }
                    else if (i == 1)
                    {
                        action = routeParams[i];
                        action = action.Replace("*", string.Empty);
                    }
                    else if (i == 2)
                    {
                        networkName = routeParams[i];
                        networkName = networkName.Replace("*", string.Empty);
                    }
                    else if (i == 3)
                    {
                        nodeName = routeParams[i];
                        nodeName = nodeName.Replace("*", string.Empty);
                    }
                    else if (i == 4)
                    {
                        name = routeParams[i];
                    }
                    else if (i == 5)
                    {
                        id = routeParams[i];
                        id = id.Replace("*", string.Empty);
                    }
                    else if (i == 6)
                    {
                        fileNameExtensionType = routeParams[i];
                        fileNameExtensionType = fileNameExtensionType.Replace("*", string.Empty);
                    }
                    else if (i == 7)
                    {
                        subAction = routeParams[i];
                    }
                    else if (i == 8)
                    {
                        subActionView = routeParams[i];
                    }
                    else if (i == 9)
                    {
                        variable = routeParams[i];
                    }
                }
            }
        }
        public static void GetURIParams(string uriPattern,
            out string name, out string id, out string networkName,
            out string nodeName, out string fileNameExtension)
        {
            name = GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.name);
            id = GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.id);
            networkName = GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.network);
            nodeName = GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.node);
            fileNameExtension = GetURIPatternPart(uriPattern, ContentURI.URIPATTERNPART.fileExtension);
        }
        public static ContentURI ConvertShortURIPattern(string uriPattern)
        {
            string networkName = string.Empty;
            string nodeName = string.Empty;
            string name = string.Empty;
            string id = string.Empty;
            string fileNameExtensionType = string.Empty;
            GetURIParams(uriPattern, out name, out id, out networkName,
                out nodeName, out fileNameExtensionType);
            //db hit for network
            ContentURI uri = new ContentURI(name, 
                GenHelpers.ConvertStringToInt(id), 
                networkName, nodeName, fileNameExtensionType);
            return uri;
        }
        public static ContentURI ConvertShortURIPattern(string uriPattern, Network network)
        {
            string networkName = string.Empty;
            string nodeName = string.Empty;
            string name = string.Empty;
            string id = string.Empty;
            string fileNameExtensionType = string.Empty;
            //no db hit
            ContentURI uri = new ContentURI(uriPattern, network);
            return uri;
        }
        public static ContentURI ConvertShortURIPattern(string uriPattern, ContentURI initURI)
        {
            string networkName = string.Empty;
            string nodeName = string.Empty;
            string name = string.Empty;
            string id = string.Empty;
            string fileNameExtensionType = string.Empty;
            //no db hit
            ContentURI uri = new ContentURI(uriPattern, initURI.URINetwork);
            uri.URIDataManager = new DataManager(initURI.URIDataManager);
            return uri;
        }
        public static string GetURIPatternFromContentURIPattern(string contenturipattern)
        {
            string sURIPattern = string.Empty;
            int iURIPatternLength = GenHelpers.GetArrayLength(contenturipattern);
            if (iURIPatternLength <= 5)
            {
                //dealing with uripattern
                sURIPattern = contenturipattern;
            }
            else
            {
                //dealing with contenturipattern
                string sId = GetContentURIPatternPart(contenturipattern, CONTENTURIPATTERNPART.id);
                string sName = GetContentURIPatternPart(contenturipattern, CONTENTURIPATTERNPART.name);
                string sNetwork = GetContentURIPatternPart(contenturipattern, CONTENTURIPATTERNPART.network);
                string sNode = GetContentURIPatternPart(contenturipattern, CONTENTURIPATTERNPART.node);
                string sFileExt = GetContentURIPatternPart(contenturipattern, CONTENTURIPATTERNPART.fileExtension);
                sURIPattern = GenHelpers.MakeURIPattern(sName, sId, sNetwork, sNode, sFileExt);
            }
            return sURIPattern;
        }
        public ContentURI()
        {
            //common name = db key Name
            this.URIName = GenHelpers.NONE;
            //unique id of the uri = db key PKId
            this.URIId = 0;
            //network of the uri (see DevTreks.Data.Network notes)
            this.URINetworkPartName = string.Empty;
            //nodename = db table
            this.URINodeName = GenHelpers.NONE;
            //mostly used by tempdocs, or by analyzers to find base calculations
            this.URIFileExtensionType = GenHelpers.NONE;
            this.URIPattern = GenHelpers.MakeURIPattern(this.URIName,
                this.URIId.ToString(), this.URINetworkPartName.ToString(),
                this.URINodeName, this.URIFileExtensionType);
            //representational state transfer uri (i.e. the Internet address of this uri)
            this.URIFull = string.Empty;
            //club that owns the uri
            this.URIClub = new Account(true);
            //logged-in member or anonymous
            this.URIMember = new AccountToMember(true);
            //the uri's network, including data connections
            this.URINetwork = new Network();
            //the uri's service (commons apps have no services)
            this.URIService = new AccountToService(true);
            //navigation, state management, and display management
            this.URIDataManager = new ContentURI.DataManager();
            this.URIModels = new ContentURI.Models();
            //error message
            this.ErrorMessage = string.Empty;
            //when an html document is not needed
            this.Message = string.Empty;
            //when javascript needs to be returned to client (i.e. post success);
            this.Json = string.Empty;
            //define the data returned by a uri using a link to an xml schema
            //(machines can find the schema using standard urn conventions, 
            //but let humans download them as well)
            this.SchemaPath = string.Empty;
        }
        //route values 
        public ContentURI(string contenturipattern)
        {
            if (string.IsNullOrEmpty(contenturipattern))
                return;
            string controller = string.Empty;
            string serverAction = string.Empty;
            string networkName = string.Empty;
            string nodeName = string.Empty;
            string name = string.Empty;
            string id = string.Empty;
            string fileNameExtensionType = string.Empty;
            string subAction = string.Empty;
            string subActionView = string.Empty;
            string variable = string.Empty;
            GetContentURIParams(contenturipattern,
                out controller, out serverAction,
                out networkName, out nodeName, out name,
                out id, out fileNameExtensionType, out subAction,
                out subActionView, out variable);
            //common name = db key Name
            this.URIName = name;
            //unique id of the uri = db key PKId
            this.URIId = GenHelpers.ConvertStringToInt(id);
            //network of the uri (see DevTreks.Data.Network notes)
            this.URINetworkPartName = networkName;
            //nodename = db table
            this.URINodeName = nodeName;
            //mostly used by tempdocs, or by analyzers to find base calculations
            this.URIFileExtensionType = fileNameExtensionType;
            this.URIPattern = GenHelpers.MakeURIPattern(this.URIName,
                this.URIId.ToString(), this.URINetworkPartName.ToString(),
                this.URINodeName, this.URIFileExtensionType);
            this.URIDataManager = new ContentURI.DataManager();
            this.URIModels = new ContentURI.Models();

            this.URIDataManager.ContentURIPattern = contenturipattern;
            //note that this.URINetwork.NetworkGroupName can be different than controller
            this.URIDataManager.ControllerName = controller;
            //client request submits this using an int enum
            this.URIDataManager.ServerActionType
                = (!string.IsNullOrEmpty(serverAction))
                ? (GenHelpers.SERVER_ACTION_TYPES)Enum.Parse(typeof(GenHelpers.SERVER_ACTION_TYPES), serverAction)
                : GenHelpers.GetServerAction(serverAction);
            this.URIDataManager.ServerSubActionType 
                = (!string.IsNullOrEmpty(subAction))
                ? (GenHelpers.SERVER_SUBACTION_TYPES)Enum.Parse(typeof(GenHelpers.SERVER_SUBACTION_TYPES), subAction)
                : GenHelpers.GetServerSubAction(subAction);
            this.URIDataManager.SubActionView = subActionView;

            //variable params that changes based on subaction 
            this.URIDataManager.Variable = variable;

            this.URIClub = new Account(true);
            this.URIMember = new AccountToMember(true);
            //the network constructor handles if this.URINetworkPartName is networkid
            //this constructor uses a db hit
            AppHelpers.Networks networkHelper = new AppHelpers.Networks();
            this.URINetwork 
                = networkHelper.GetDefaultNetwork(this.URINetworkPartName);
            FixURIPatternForDisplay();
            this.URIService = new AccountToService(true);
            this.URIService.Service = new Service(true);
            //set specific properties needed
            GenHelpers.SetApps(this);
            this.URIFull = string.Empty;
            this.ErrorMessage = string.Empty;
            this.Message = string.Empty;
            this.Json = string.Empty;
            this.SchemaPath = string.Empty;
        }
        public static ContentURI GetContentURIFromFullURL(string fullURL)
        {
            //h ttps://www.devtreks.org/commontreks/preview/commons/resourcepack/Ag Production Analysis 1/464/none 
            string sURINoHttp = GenHelpers.GetStringFromEnd2(fullURL,
                    GenHelpers.WEBFILE_PATH_DELIMITERS, GenHelpers.WEBFILE_PATH_DELIMITER, 3);
            ContentURI uri = new ContentURI(sURINoHttp);
            return uri;
        }
        public static string GetFullContentURIFromPartial(
            string partialContentUriPattern, string controller, string action)
        {
            //non-ajax and ajaxlink requests put the controller and 
            //action found in the routes collection (partialuri does 
            //not include those params)
            StringBuilder bldr = new StringBuilder();
            bldr.Append(controller);
            bldr.Append(GenHelpers.WEBFILE_PATH_DELIMITER);
            bldr.Append(action);
            bldr.Append(GenHelpers.WEBFILE_PATH_DELIMITER);
            bldr.Append(partialContentUriPattern);
            string sContentURIPattern = bldr.ToString();
            return sContentURIPattern;
        }
        //substrings in uripattern
        public enum URIPATTERNPART
        {
            //use 1 based index 
            network         = 1,
            node            = 2,
            name            = 3,
            id              = 4,
            fileExtension   = 5
        }
        private static int GetURIPATTERNPART(URIPATTERNPART uriPart)
        {
            int iPart = 0;
            if (uriPart == URIPATTERNPART.network)
            {
                iPart = 1;
            }
            else if (uriPart == URIPATTERNPART.node)
            {
                iPart = 2;
            }
            else if (uriPart == URIPATTERNPART.name)
            {
                iPart = 3;
            }
            else if (uriPart == URIPATTERNPART.id)
            {
                iPart = 4;
            }
            else if (uriPart == URIPATTERNPART.fileExtension)
            {
                iPart = 5;
            }
            return iPart;
        }
        //substrings in contenturipattern
        public enum CONTENTURIPATTERNPART
        {
            //use 1 based index 
            controller      = 1,
            action          = 2,
            network         = 3,
            node            = 4,
            name            = 5,
            id              = 6,
            fileExtension   = 7,
            subaction       = 8,
            subactionview   = 9,
            variable        = 10
        }
        private static int GetCONTENTURIPATTERNPART(CONTENTURIPATTERNPART uriPart)
        {
            int iPart = 0;
            if (uriPart == CONTENTURIPATTERNPART.controller)
            {
                iPart = 1;
            }
            else if (uriPart == CONTENTURIPATTERNPART.action)
            {
                iPart = 2;
            }
            else if (uriPart == CONTENTURIPATTERNPART.network)
            {
                iPart = 3;
            }
            else if (uriPart == CONTENTURIPATTERNPART.node)
            {
                iPart = 4;
            }
            else if (uriPart == CONTENTURIPATTERNPART.name)
            {
                iPart = 5;
            }
            else if (uriPart == CONTENTURIPATTERNPART.id)
            {
                iPart = 6;
            }
            else if (uriPart == CONTENTURIPATTERNPART.fileExtension)
            {
                iPart = 7;
            }
            else if (uriPart == CONTENTURIPATTERNPART.subaction)
            {
                iPart = 8;
            }
            else if (uriPart == CONTENTURIPATTERNPART.subactionview)
            {
                iPart = 9;
            }
            else if (uriPart == CONTENTURIPATTERNPART.variable)
            {
                iPart = 10;
            }
            return iPart;
        }
        public static string GetURIPatternPart(string uriPattern,
            URIPATTERNPART uriPart)
        {
            //returns a substring from a uripattern
            string sURIPatternPart = string.Empty;
            int iOneBasedIndex = GetURIPATTERNPART(uriPart);
            sURIPatternPart = GenHelpers.GetSubstringFromFront(uriPattern,
                GenHelpers.WEBFILE_PATH_DELIMITERS, iOneBasedIndex);
            return sURIPatternPart;
        }
        public static string GetContentURIPatternPart(string uriPattern,
            CONTENTURIPATTERNPART uriPart)
        {
            //returns a substring from a uripattern
            string sURIPatternPart = string.Empty;
            int iOneBasedIndex = GetCONTENTURIPATTERNPART(uriPart);
            sURIPatternPart = GenHelpers.GetSubstringFromFront(uriPattern,
                GenHelpers.WEBFILE_PATH_DELIMITERS, iOneBasedIndex);
            return sURIPatternPart;
        }
        public static string GetURIPatternPart(string uriPattern, string patternPartName)
        {
            string sURIPatternPart = string.Empty;
            if (patternPartName == URIPATTERNPART.fileExtension.ToString())
            {
                sURIPatternPart = GetURIPatternPart(uriPattern, URIPATTERNPART.fileExtension);
            }
            else if (patternPartName == URIPATTERNPART.id.ToString())
            {
                sURIPatternPart = GetURIPatternPart(uriPattern, URIPATTERNPART.id);
            }
            else if (patternPartName == URIPATTERNPART.name.ToString())
            {
                sURIPatternPart = GetURIPatternPart(uriPattern, URIPATTERNPART.name);
            }
            else if (patternPartName == URIPATTERNPART.network.ToString())
            {
                sURIPatternPart = GetURIPatternPart(uriPattern, URIPATTERNPART.network);
            }
            else if (patternPartName == URIPATTERNPART.node.ToString())
            {
                sURIPatternPart = GetURIPatternPart(uriPattern, URIPATTERNPART.node);
            }
            return sURIPatternPart;
        }
        public static string GetContentURIPatternPart(string uriPattern, string patternPartName)
        {
            string sURIPatternPart = string.Empty;
            if (patternPartName == CONTENTURIPATTERNPART.controller.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.controller);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.action.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.action);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.network.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.network);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.node.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.node);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.name.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.name);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.id.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.id);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.fileExtension.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.fileExtension);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.subaction.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.subaction);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.subactionview.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.subactionview);
            }
            else if (patternPartName == CONTENTURIPATTERNPART.variable.ToString())
            {
                sURIPatternPart = GetContentURIPatternPart(uriPattern, CONTENTURIPATTERNPART.variable);
            }
            return sURIPatternPart;
        }
        public static string GetFullURIPathPart(string uriFull,
            int zeroBasedIndex)
        {
            string sPathPart = string.Empty;
            string sURINoHttp = string.Empty;
            if (uriFull != null)
            {
                //try both
                sURINoHttp = uriFull.Replace("http://", string.Empty);
                if (uriFull.StartsWith("https"))
                {
                    sURINoHttp = uriFull.Replace("https://", string.Empty);
                }
            }
            //zerobasedindex has to match the positions found in GetFullURIParams(...)
            //but the substring utility uses a onebasedindex
            sPathPart = GenHelpers.GetSubstringFromFront(sURINoHttp,
                GenHelpers.WEBFILE_PATH_DELIMITERS, zeroBasedIndex + 1);
            return sPathPart;
        }
        public static int GetGroupingParentId(string parentURIPattern)
        {
            int iParentId = GenHelpers.ConvertStringToInt(
                GetURIPatternPart(parentURIPattern, URIPATTERNPART.id));
            return iParentId;
        }
        public ContentURI(string name, int id, string networkPartName,
           string nodeName, string fileNameExtensionType)
        {
            //validate the name before adding it to uripattern (legacy names
            //could contain invalid chars)
            RuleHelpers.ResourceRules.ValidateScriptArgument(ref name);
            //basic uri pattern
            this.URIName = name;
            this.URIId = id;
            this.URINetworkPartName = networkPartName;
            this.URINodeName = nodeName;
            this.URIFileExtensionType = fileNameExtensionType;
            this.URIPattern = GenHelpers.MakeURIPattern(this.URIName,
                    this.URIId.ToString(), this.URINetworkPartName,
                    this.URINodeName, this.URIFileExtensionType);
            this.URIClub = new Account(true);
            this.URIMember = new AccountToMember(true);
            //the network constructor handles if this.URINetworkPartName is networkid
            //this constructor uses a db hit
            AppHelpers.Networks networkHelper = new AppHelpers.Networks();
            this.URINetwork = networkHelper.GetDefaultNetwork(this.URINetworkPartName);
            FixURIPatternForDisplay();
            this.URIService = new AccountToService(true);
            this.URIDataManager = new ContentURI.DataManager();
            this.URIModels = new ContentURI.Models();
            this.URIFull = string.Empty;
            this.ErrorMessage = string.Empty;
            this.Message = string.Empty;
            this.Json = string.Empty;
            this.SchemaPath = string.Empty;
        }
        
        public ContentURI(string newURIPattern,
            Network newNetwork)
        {
            this.URIPattern = newURIPattern;
            GenHelpers.SetURIParams(this);
            this.URIClub = new Account(true);
            this.URIMember = new AccountToMember(true);
            //the network constructor handles if this.URINetworkPartName is networkid
            if (newNetwork == null)
            {
                //this constructor uses a db hit
                AppHelpers.Networks networkHelper = new AppHelpers.Networks();
                this.URINetwork = networkHelper.GetDefaultNetwork(this.URINetworkPartName);
            }
            else
            {
                //this constructor does not hit db
                this.URINetwork = new Network(newNetwork);
            }
            FixURIPatternForDisplay();
            this.URIService = new AccountToService(true);
            this.URIDataManager = new ContentURI.DataManager();
            this.URIModels = new ContentURI.Models();
            //set specific properties needed
            GenHelpers.SetApps(this);
            this.URIFull = string.Empty;
            this.ErrorMessage = string.Empty;
            this.Message = string.Empty;
            this.Json = string.Empty;
            this.SchemaPath = string.Empty;
        }
        //used to hold and display uri lists with thumbnails
        //should never run any internal db queries
        public static ContentURI GetContentURI(ContentURI startURI,
            int id, string label, string name,
            string description, string fileNameExtensionType,
            string connection, string resourceUri,
            string resourceAlt, string parentURIPattern,
            GenHelpers.DOCS_STATUS docStatus,
            string networkIdOrPartName, string nodeName,
            string webPath)
        {
            //networkId comes from service.NetworkId for search queries
            //linked view queries will change to uri.URINetwork.Id in FillLinkedViewList
            string sNetworkId = ContentURI.GetURIPatternPart(parentURIPattern, URIPATTERNPART.network);
            int iNetworkId = Helpers.GeneralHelpers.ConvertStringToInt(sNetworkId);
            //2.0.0 init with startURI to get appsettings
            ContentURI uri = new ContentURI(startURI);
            //2.0.0 and force these 2 props to be set during workflows
            //otherwise linkedviews stories and devpacks look for files in 
            //children linkedviews when they should use parent
            uri.URIDataManager.IsSelectedLinkedAddIn = false;
            uri.URIDataManager.IsSelectedLinkedView = false;
            //rest have no 2.0.0 changes
            RuleHelpers.ResourceRules.ValidateScriptArgument(ref name);
            uri.URIName = name.Trim();
            uri.URIId = id;
            //returns string networkpartname
            uri.URINetworkPartName = networkIdOrPartName;
            uri.URINodeName = nodeName.Trim();
            uri.URIFileExtensionType = fileNameExtensionType;
            uri.URIPattern = GenHelpers.MakeURIPattern(uri.URIName,
                uri.URIId.ToString(), uri.URINetworkPartName,
                uri.URINodeName, uri.URIFileExtensionType);
            uri.URIClub = new Account(true);
            uri.URIMember = new AccountToMember(true);
            AppHelpers.Networks networkHelper = new AppHelpers.Networks();
            uri.URINetwork = networkHelper.GetDefaultNetwork(uri.URINetworkPartName);
            uri.URINetwork.PKId = iNetworkId;
            if (string.IsNullOrEmpty(connection))
            {
                //152 eliminated db hit-handle during individual uri retrieval
                //uri.URINetwork = await networkHelper.GetNetworkAsync(sqlIO, uri.URINetworkPartName)
            }
            else
            {
                //use the connection returned for potential multidb use
                uri.URINetwork.AdminConnection = connection;
                uri.URINetwork.WebConnection = connection;
            }
            uri.URIService = new AccountToService(true);
            uri.URIFull = string.Empty;
            uri.ErrorMessage = string.Empty;
            uri.Message = string.Empty;
            uri.Json = string.Empty;
            uri.SchemaPath = string.Empty;
            uri.URIModels = new ContentURI.Models();
            uri.URIDataManager.Label = label;
            uri.URIDataManager.Description = description;
            GenHelpers.SetApps(uri);
            //note that resourceURIPattern is actually a filesystem rel path
            if (resourceUri != string.Empty)
            {
                //just set up the resources list here without hitting db (owner can do it later)
                string sResourceURIPattern = string.Empty;
                sResourceURIPattern = uri.URIDataManager.AddResourceToResourceList(uri,
                    sResourceURIPattern, resourceAlt, resourceUri, string.Empty);
                //don't hit db here; let the owner handle during resource management, not contenturi creation
                //sResourceURIPattern = await uri.URIDataManager.AddResourceToResourceListAsync(uri,
                //    sResourceURIPattern, resourceAlt, resourceUri, string.Empty);
            }
            uri.URIDataManager.ParentURIPattern = parentURIPattern.Trim();
            uri.URIDataManager.DocStatus = docStatus;
            uri.URIDataManager.WebPath = webPath;
            return uri;
        }
        
        //copy constructor
        public ContentURI(ContentURI uri)
        {
            //make a copy of uri
            this.URIName = uri.URIName;
            this.URIId = uri.URIId;
            this.URINetworkPartName = uri.URINetworkPartName;
            this.URINodeName = uri.URINodeName;
            this.URIFileExtensionType = uri.URIFileExtensionType;
            this.URIPattern = uri.URIPattern;
            this.URIFull = uri.URIFull;
            this.URIClub = new Account(uri.URIClub);
            this.URIMember = new AccountToMember(uri.URIMember);
            this.URINetwork = new Network(uri.URINetwork);
            this.URIService = new AccountToService(uri.URIService);
            this.URIDataManager = new ContentURI.DataManager(uri.URIDataManager);
            this.URIModels = new Models();
            this.ErrorMessage = string.Empty;
            this.Message = uri.Message;
            this.Json = uri.Json;
            this.SchemaPath = uri.SchemaPath;
        }
        
        //uri parameters sent by route pattern or by an ajax request
        public string URIName { get; set; }
        public int URIId { get; set; }
        //internally (on server and to machines) this can be urinetworkid,
        //externally (what humans see) it should be the networkpartname
        public string URINetworkPartName { get; set; }
        public string URINodeName { get; set; }
        public string URIFileExtensionType { get; set; }
        //delimited string sent in route pattern: {networkname}/{nodename}/{commonname}/{id}/{fileextensiontype},
        public string URIPattern { get; set; }
        //full uri www.devtreks.org/agtreks/linkedviews/{networkname}/{nodename}/{commonname}/{id}/{fileextensiontype},
        public string URIFull { get; set; }

        //owner of the uri
        public Account URIClub { get; set; }
        //member using the uri (including clubinuse.authorization level for using uri)
        //if not logged-in, URIMember is anonymous and given id=0
        public AccountToMember URIMember { get; set; }
        //network containing the uri (holding data connections)
        public Network URINetwork { get; set; }
        //service, if applicable, containing the uri 
        //(commons objects are not part of service agreements -they provide 
        //networking services but not formal story or number services)
        public AccountToService URIService { get; set; }
        //parameters needed to load and display the uri
        public ContentURI.DataManager URIDataManager { get; set; }
        //models needed to load and display the uri
        public ContentURI.Models URIModels { get; set; }

        //error handling
        public string ErrorMessage { get; set; }
        //in lieu of responding with new views, respond only with a string message
        public string Message { get; set; }
        //respond with javascript (i.e. stop showing progress and signal success)
        public string Json { get; set; }
        //path to the xml schema defining the data returned by the uri
        public string SchemaPath { get; set; }

        

        public void ChangeContentURIPattern(string newContentURIPattern)
        {
            string sOldNetworkName = this.URINetworkPartName;
            int iURIPatternLength = GenHelpers.GetArrayLength(newContentURIPattern);
            if (iURIPatternLength <= 5)
            {
                //dealing with uripattern
                this.ChangeURIPattern(newContentURIPattern);
            }
            else
            {
                this.URIDataManager.ContentURIPattern = newContentURIPattern;
                GenHelpers.SetContentURIParams(this);
                if (sOldNetworkName != this.URINetworkPartName
                    || this.URINetwork.NetworkURIPartName != this.URINetworkPartName)
                {
                    //this constructor uses a db hit
                    AppHelpers.Networks networkHelper = new AppHelpers.Networks();
                    this.URINetwork = networkHelper.GetDefaultNetwork(this.URINetworkPartName);
                }
                FixURIPatternForDisplay();
            }
        }
        //change just the uri pattern properties of this
        public void ChangeURIPattern(string newURIPattern)
        {
            string sOldNetworkName = this.URINetworkPartName;
            this.URIPattern = GetURIPatternFromContentURIPattern(newURIPattern);
            GenHelpers.SetURIParams(this);
            if (sOldNetworkName != this.URINetworkPartName
                || this.URINetwork.NetworkURIPartName != this.URINetworkPartName)
            {
                //this constructor uses a db hit
                AppHelpers.Networks networkHelper = new AppHelpers.Networks();
                this.URINetwork = networkHelper.GetDefaultNetwork(this.URINetworkPartName);
            }
            FixURIPatternForDisplay();
        }
        public void ChangeURIPatternNoDbHit(string newURIPattern)
        {
            this.URIPattern = GetURIPatternFromContentURIPattern(newURIPattern);
            GenHelpers.SetURIParams(this);
        }
        public void ChangeURIPatternUsingList(string newURIPattern, 
            List<ContentURI> uris)
        {
            //check whether new network connections are needed
            Network selectionNetwork
                = Helpers.LinqHelpers.GetSameNetworkFromList(
                newURIPattern, uris);
            if (selectionNetwork == null)
            {
                //change the uri and get new connection properties
                this.ChangeURIPattern(newURIPattern);
            }
            else
            {
                //no need to hit db to get connections
                this.ChangeURIPattern(newURIPattern,
                    selectionNetwork);
            }
        }
        public void ChangeURIPattern(string newURIPattern, 
            Network uriNetwork)
        {
            this.URIPattern = GetURIPatternFromContentURIPattern(newURIPattern);
            GenHelpers.SetURIParams(this);
            //this constructor does not use a db hit
            this.URINetwork = new Network(uriNetwork);
            FixURIPatternForDisplay();
        }
        public static void ChangeURIPattern(ContentURI uri, 
            string newURIPattern)
        {
            uri.ChangeURIPattern(newURIPattern);
            GenHelpers.SetApps(uri);
        }
        public void ChangeURIPatternFromFileName(string fileNameURIPattern)
        {
            //filenames use a different order of uriparams than uripattern
            //fileNames use oContentHelper.MakeStandardFileName() to build
            //a FILENAME_DELIMITED string (needed while packaging 
            //and for finding specific filesystem files)
            GenHelpers.SetURIParamsFromFileName(this, fileNameURIPattern);
            string sOldNetworkName = this.URINetworkPartName;
            if (sOldNetworkName != this.URINetworkPartName
                || this.URINetwork.NetworkURIPartName != this.URINetworkPartName)
            {
                //this constructor uses a db hit
                AppHelpers.Networks networkHelper = new AppHelpers.Networks();
                this.URINetwork = networkHelper.GetDefaultNetwork(this.URINetworkPartName);
            }
            FixURIPatternForDisplay();
        }
        public static string ChangeURIPatternPart(string oldURIPattern,
            URIPATTERNPART partToChange, string partToChangeValue)
        {
            //change one of a uripattern's parts 
            ContentURI newURI = new ContentURI();
            newURI.URIPattern = oldURIPattern;
            GenHelpers.SetURIParams(newURI);
            if (partToChange == URIPATTERNPART.fileExtension)
            {
                newURI.URIFileExtensionType = partToChangeValue;
            }
            else if (partToChange == URIPATTERNPART.id)
            {
                newURI.URIId 
                    = GenHelpers.ConvertStringToInt(partToChangeValue);
            }
            else if (partToChange == URIPATTERNPART.name)
            {
                newURI.URIName = partToChangeValue;
            }
            else if (partToChange == URIPATTERNPART.network)
            {
                newURI.URINetworkPartName = partToChangeValue;
            }
            else if (partToChange == URIPATTERNPART.node)
            {
                newURI.URINodeName = partToChangeValue;
            }
            newURI.UpdateURIPattern();
            return newURI.URIPattern;
        }
        public static string ChangeContentURIPatternPart(string oldURIPattern,
            CONTENTURIPATTERNPART partToChange, string partToChangeValue)
        {
            //change one of a uripattern's parts 
            ContentURI newURI = new ContentURI();
            newURI.URIDataManager.ContentURIPattern = oldURIPattern;
            GenHelpers.SetContentURIParams(newURI);
            if (partToChange == CONTENTURIPATTERNPART.fileExtension)
            {
                newURI.URIFileExtensionType = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.id)
            {
                newURI.URIId
                    = GenHelpers.ConvertStringToInt(partToChangeValue);
            }
            else if (partToChange == CONTENTURIPATTERNPART.name)
            {
                newURI.URIName = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.network)
            {
                newURI.URINetworkPartName = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.node)
            {
                newURI.URINodeName = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.fileExtension)
            {
                newURI.URIFileExtensionType = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.controller)
            {
                newURI.URIDataManager.ControllerName = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.action)
            {
                newURI.URIDataManager.ServerActionType = GenHelpers.GetServerAction(partToChangeValue);
            }
            else if (partToChange == CONTENTURIPATTERNPART.subaction)
            {
                newURI.URIDataManager.ServerSubActionType = GenHelpers.GetServerSubAction(partToChangeValue);
            }
            else if (partToChange == CONTENTURIPATTERNPART.subactionview)
            {
                newURI.URIDataManager.SubActionView = partToChangeValue;
            }
            else if (partToChange == CONTENTURIPATTERNPART.variable)
            {
                newURI.URIDataManager.Variable = partToChangeValue;
            }
            newURI.UpdateContentURIPattern();
            return newURI.URIDataManager.ContentURIPattern;
        }
        //change just the uri pattern properties of this
        public void UpdateURIPattern()
        {
            this.URIPattern = GenHelpers.MakeURIPattern(
                this.URIName, this.URIId.ToString(), this.URINetworkPartName,
                this.URINodeName, this.URIFileExtensionType);
            FixURIPatternForDisplay();
        }
        public void UpdateContentURIPattern()
        {
            this.URIPattern = GenHelpers.MakeURIPattern(
                this.URIName, this.URIId.ToString(), this.URINetworkPartName,
                this.URINodeName, this.URIFileExtensionType);
            this.URIDataManager.ContentURIPattern = GenHelpers.MakeContentURIPattern(
                this.URIDataManager.ControllerName, this.URIDataManager.ServerActionType.ToString(),
                this.URIPattern, this.URIDataManager.ServerSubActionType.ToString(),
                this.URIDataManager.SubActionView, this.URIDataManager.Variable);
            FixURIPatternForDisplay();
        }
        private void FixURIPatternForDisplay()
        {
            //this.URINetworkPartName can be network.networkid or network.networkpartname
            //display to humans as this.URINetworkPartName only
            if (this.URINetwork != null
                && this.URIFileExtensionType
                != GenHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                if (this.URINetworkPartName != this.URINetwork.NetworkURIPartName
                    && this.URINetwork.NetworkURIPartName != string.Empty
                    && this.URINetwork.NetworkURIPartName != null)
                {
                    this.URINetworkPartName = this.URINetwork.NetworkURIPartName;
                    this.URIPattern = GenHelpers.MakeURIPattern(this.URIName,
                        this.URIId.ToString(), this.URINetworkPartName,
                        this.URINodeName, this.URIFileExtensionType);
                }
            }
        }
        /// <summary>
        ///Purpose:		Manage params needed to load and display uri content
        ///Author:		www.devtreks.org
        ///Date:		2016, September
        ///Notes:       2.0.2 added PlatformType to eliminate need for GetPlatformType() functions
        /// </summary>
        public class DataManager
        {
            public DataManager()
            {
                this.ContentURIPattern = string.Empty;
                this.ControllerName = string.Empty;
                this.ClientActionType = GenHelpers.CLIENTACTION_TYPES.postrequest;
                this.ServerActionType = GenHelpers.SERVER_ACTION_TYPES.preview;
                this.ServerSubActionType = GenHelpers.SERVER_SUBACTION_TYPES.respondwithhtml;
                this.SubActionView = GenHelpers.SUBACTION_VIEWS.none.ToString();
                this.Variable = string.Empty;
                this.FormInput = null;
                this.SelectionsNodeNeededName = string.Empty;
                this.SelectedList = string.Empty;
                this.SelectionsURIPattern = string.Empty;
                this.SelectionsNodeURIPattern = string.Empty;
                this.SelectionsAttributeName = string.Empty;
                this.CalcParams = string.Empty;
                this.DocStatus = GenHelpers.DOCS_STATUS.notreviewed;
                this.AppType = GenHelpers.APPLICATION_TYPES.none;
                this.SubAppType = GenHelpers.SUBAPPLICATION_TYPES.none;

                this.ParentURIPattern = string.Empty;
                this.ChildrenNodeName = string.Empty;
                this.ParentStartRow = 0;
                this.StartRow = 0;
                this.IsForward = "1";
                this.PageSize = 0;
                this.RowCount = 0;

                this.UpdatePanelType = GenHelpers.UPDATE_PANEL_TYPES.preview;
                this.ParentPanelType = GenHelpers.UPDATE_PANEL_TYPES.none;
                this.ChildrenPanelType = GenHelpers.UPDATE_PANEL_TYPES.none;

                this.PreviewViewEditType = GenHelpers.VIEW_EDIT_TYPES.print;
                this.EditViewEditType = GenHelpers.VIEW_EDIT_TYPES.print;
                this.SelectViewEditType = GenHelpers.VIEW_EDIT_TYPES.part;
                this.Label = string.Empty;
                this.AttributeName = string.Empty;
                this.Date = GenHelpers.GetDateShortNow();
                this.UserLanguage = "en-us";

                this.LinkedView = null;
                this.StartingDocToCalcURIPattern = string.Empty;
                this.IsSelectedLinkedAddIn = false;
                this.IsSelectedLinkedView = false;
                this.UseSelectedLinkedView = false;
                this.IsDefault = false;
                this.AddInName = string.Empty;
                this.HostName = string.Empty;
                this.UseDefaultAddIn = false;
                this.UseDefaultLocal = false;
                this.LinkedLists = string.Empty;
                this.SubActionView = string.Empty;

                this.BaseId = 0;
                this.TempDocURIPattern = string.Empty;
                this.TempDocNodeToCalcURIPattern = string.Empty;
                this.TempDocPath = string.Empty;
                this.TempDocSaveMethod = string.Empty;

                this.Resource = null;
                this.IsMainStylesheet = false;
                this.IsMainImage = false;
                this.ExtensionObjectNamespace = string.Empty;
                this.Description = string.Empty;
                this.LongDescription = string.Empty;
                this.MiscDocPath = string.Empty;
                
                this.DefaultRootFullFilePath = string.Empty;
                this.DefaultRootWebStoragePath = string.Empty;
                this.DefaultWebDomain = string.Empty;
                this.FileSystemPath = string.Empty;
                this.WebPath = string.Empty;
                this.DefaultConnection = string.Empty;
                this.StorageConnection = string.Empty;
                this.FileSizeValidation = string.Empty;
                this.FileSizeDBStorageValidation = string.Empty;
                this.RExecutable = string.Empty;
                this.PyExecutable = string.Empty;
                this.JuliaExecutable = string.Empty;
                this.HostFeeRate = string.Empty;
                this.ResourceURIName = string.Empty;
                this.ContentURIName = string.Empty;
                this.TempDocsURIName = string.Empty;
                this.ExtensionsPath = string.Empty;
                this.PlatformType = Helpers.FileStorageIO.PLATFORM_TYPES.none;
            }
            public DataManager(DataManager mngr)
            {
                this.ContentURIPattern = mngr.ContentURIPattern;
                this.ControllerName = mngr.ControllerName;
                this.ClientActionType = mngr.ClientActionType;
                this.ServerActionType = mngr.ServerActionType;
                this.ServerSubActionType = mngr.ServerSubActionType;
                this.Variable = mngr.Variable;
                this.FormInput = mngr.FormInput;
                this.SelectionsNodeNeededName = mngr.SelectionsNodeNeededName;
                this.SelectedList = mngr.SelectedList;
                this.SelectionsURIPattern = mngr.SelectionsURIPattern;
                this.SelectionsNodeURIPattern = mngr.SelectionsNodeURIPattern;
                this.SelectionsAttributeName = mngr.SelectionsAttributeName;
                this.CalcParams = mngr.CalcParams;
                this.DocStatus = mngr.DocStatus;
                this.AppType = mngr.AppType;
                this.SubAppType = mngr.SubAppType;
                this.ParentURIPattern = mngr.ParentURIPattern;
                this.ChildrenNodeName = mngr.ChildrenNodeName;
                this.ParentStartRow = mngr.ParentStartRow;
                this.StartRow = mngr.StartRow;
                this.IsForward = mngr.IsForward;
                this.PageSize = mngr.PageSize;
                this.RowCount = mngr.RowCount;

                this.UpdatePanelType = mngr.UpdatePanelType;
                this.ParentPanelType = mngr.ParentPanelType;
                this.ChildrenPanelType = mngr.ChildrenPanelType;

                this.PreviewViewEditType = mngr.PreviewViewEditType;
                this.EditViewEditType = mngr.EditViewEditType;
                this.SelectViewEditType = mngr.SelectViewEditType;
                this.Label = mngr.Label;
                this.AttributeName = mngr.AttributeName;
                this.UserLanguage = mngr.UserLanguage;
                this.Date = mngr.Date;

                this.Ancestors = Helpers.LinqHelpers.CopyContentURIs(mngr.Ancestors);
                this.Children = Helpers.LinqHelpers.CopyContentURIs(mngr.Children);
                CopyLinkedView(mngr, this);
                this.StartingDocToCalcURIPattern = mngr.StartingDocToCalcURIPattern;
                this.IsSelectedLinkedAddIn = mngr.IsSelectedLinkedAddIn;
                this.IsSelectedLinkedView = mngr.IsSelectedLinkedView;
                this.UseSelectedLinkedView = mngr.UseSelectedLinkedView;
                this.IsDefault = mngr.IsDefault;
                this.AddInName = mngr.AddInName;
                this.HostName = mngr.HostName;
                this.UseDefaultAddIn = mngr.UseDefaultAddIn;
                this.UseDefaultLocal = mngr.UseDefaultLocal;
                this.LinkedLists = mngr.LinkedLists;
                this.SubActionView = mngr.SubActionView;
                this.BaseId = mngr.BaseId;
                this.TempDocURIPattern = mngr.TempDocURIPattern;
                this.TempDocNodeToCalcURIPattern = mngr.TempDocNodeToCalcURIPattern;
                this.TempDocPath = mngr.TempDocPath;
                this.TempDocSaveMethod = mngr.TempDocSaveMethod;

                this.Resource = Helpers.LinqHelpers.CopyContentURIs(mngr.Resource);
                this.IsMainStylesheet = mngr.IsMainStylesheet;
                this.IsMainImage = mngr.IsMainImage;
                
                this.ExtensionObjectNamespace = mngr.ExtensionObjectNamespace;
                this.Description = mngr.Description;
                this.LongDescription = mngr.LongDescription;

                this.NetworkCategories = Helpers.LinqHelpers.CopyContentURIs(mngr.NetworkCategories);
                this.MemberGroups = mngr.MemberGroups;
                this.ClubGroups = mngr.ClubGroups;
                this.GeoRegions = mngr.GeoRegions;
                this.MiscDocPath = mngr.MiscDocPath;
                this.DefaultRootFullFilePath = mngr.DefaultRootFullFilePath;
                this.DefaultRootWebStoragePath = mngr.DefaultRootWebStoragePath;
                this.DefaultWebDomain = mngr.DefaultWebDomain;
                this.FileSystemPath = mngr.FileSystemPath;
                this.WebPath = mngr.WebPath;
                this.DefaultConnection = mngr.DefaultConnection;
                this.StorageConnection = mngr.StorageConnection;
                this.FileSizeValidation = mngr.FileSizeValidation;
                this.FileSizeDBStorageValidation = mngr.FileSizeDBStorageValidation;
                this.RExecutable = mngr.RExecutable;
                this.PyExecutable = mngr.PyExecutable;
                this.JuliaExecutable = mngr.JuliaExecutable;
                this.HostFeeRate = mngr.HostFeeRate;
                this.ResourceURIName = mngr.ResourceURIName;
                this.ContentURIName = mngr.ContentURIName;
                this.TempDocsURIName = mngr.TempDocsURIName;
                this.ExtensionsPath = mngr.ExtensionsPath;
                this.PlatformType = mngr.PlatformType;
            }
            public string ContentURIPattern { get; set; }
            public string ControllerName { get; set; }
            //client-side instruction usually sent in an ajax request
            public GenHelpers.CLIENTACTION_TYPES ClientActionType { get; set; }
            //search, preview, select, edit, pack, view
            public GenHelpers.SERVER_ACTION_TYPES ServerActionType { get; set; }
            //replaces typical postback event handlers (i.e. login, logout, calculate ...)
            public GenHelpers.SERVER_SUBACTION_TYPES ServerSubActionType { get; set; }
            //holds misc params that depend on subactiontype
            public string Variable { get; set; }
            //form elements are stored in this collection
            public Dictionary<string, string> FormInput { get; set; }

            //state management when selections are pending
            public string SelectionsNodeNeededName { get; set; }
            public string SelectedList { get; set; }
            public string SelectionsURIPattern { get; set; }
            public string SelectionsNodeURIPattern { get; set; }
            public string SelectionsAttributeName { get; set; }

            public string CalcParams { get; set; }

            //uri status: approved, not reviewed (donotdisplay keeps it out of search engine)
            public GenHelpers.DOCS_STATUS DocStatus { get; set; }

            //app properties
            public GenHelpers.APPLICATION_TYPES AppType { get; set; }
            public GenHelpers.SUBAPPLICATION_TYPES SubAppType { get; set; }

            //navigation/edit properties
            public string ParentURIPattern { get; set; }
            public string ChildrenNodeName { get; set; }
            
            //pagination properties
            public int ParentStartRow { get; set; }
            public int StartRow { get; set; }
            public string IsForward { get; set; }
            public int PageSize { get; set; }
            public int RowCount { get; set; }

            //display params- each ui can be viewed differently
            public GenHelpers.UPDATE_PANEL_TYPES UpdatePanelType { get; set; }
            public GenHelpers.UPDATE_PANEL_TYPES ParentPanelType { get; set; }
            public GenHelpers.UPDATE_PANEL_TYPES ChildrenPanelType { get; set; }
            public GenHelpers.VIEW_EDIT_TYPES PreviewViewEditType { get; set; }
            public GenHelpers.VIEW_EDIT_TYPES EditViewEditType { get; set; }
            public GenHelpers.VIEW_EDIT_TYPES SelectViewEditType { get; set; }
            //misc
            public string UserLanguage { get; set; }
            public string Label { get; set; }
            public string AttributeName { get; set; }
            //last edited date 
            public DateTime Date { get; set; }

            //ancestors of the uri
            public List<ContentURI> Ancestors { get; set; }
            //children of the uri
            public List<ContentURI> Children { get; set; }

            //Berners-Lee also advises that a site's uris should return "linked data"
            //the semantic web aspects of LinkedView will evolve
            //linked views of the uri: calculations, analyses, and other new views
            public List<System.Linq.IGrouping<int, ContentURI>> LinkedView { get; set; }
            //convenient param when running calcs
            public string StartingDocToCalcURIPattern { get; set; }
            //true for a selected linkedview that is the addin to use
            public bool IsSelectedLinkedAddIn { get; set; }
            //true for a selected linkedviews member that is a custom/virtual uri, 
            //usually being used as the doctocalcuri during calculations and analyses
            public bool IsSelectedLinkedView { get; set; }
            //true, when a linkedviews member is a custom/virtual uri that either:
            //  a. is being edited in edits panel
            //  b. is being calculated in views panel (calcdocuri is a selected addin)
            public bool UseSelectedLinkedView { get; set; }
            //true for linkedviews member that is the default
            public bool IsDefault { get; set; }
            //not empty if linkedview is an addin or extension
            public string AddInName { get; set; }
            public string HostName { get; set; }
            //true means use their uri.urimember.clubinuse.addins[].isdefault as linkedview
            public bool UseDefaultAddIn { get; set; }
            //true means use their uri.urimember.clubinuse.locals[].isdefault as linkedview
            public bool UseDefaultLocal { get; set; }
            //lists linked to linkedview (stored in a parameter delimited string array)
            public string LinkedLists { get; set; }
            //misc data (returned from respondwithlist or respondwithform serversubactions
            public string SubActionView { get; set; }
            //true means generate stateful html for packaging (whenever contentservice.SaveURIFirstDoc) is run
            public bool HasNewXml { get; set; }

            //in the case of join tables, such as linkedviewtooperation, 
            //this will be the base table id, such as linkedviewid
            public int BaseId { get; set; }
            //tempdocs usually store addin calcs temporarily (no db relations)
            public string TempDocURIPattern { get; set; }
            //the node to calc is usually different than the doctocalc when using full tempdocs 
            public string TempDocNodeToCalcURIPattern { get; set; }
            public string TempDocPath { get; set; }
            public string TempDocSaveMethod { get; set; }

            //resources (images, stylesheets) of the uri 
            public List<ContentURI> Resource { get; set; }
            //main stylesheet used to transform uri
            public bool IsMainStylesheet { get; set; }
            //main image to show with uri 
            public bool IsMainImage { get; set; }
            
            //stylesheet extension objects
            public string ExtensionObjectNamespace { get; set; }
            //also used for a resource element's alt and long description attributes
            public string Description { get; set; }
            public string LongDescription { get; set; }
            //network categories
            public List<ContentURI> NetworkCategories { get; set; }
            //member groups
            public List<MemberClass> MemberGroups { get; set; }
            public List<AccountClass> ClubGroups { get; set; } 
            //regions
            public List<GeoRegion> GeoRegions { get; set; }
            //display summary, or full, view of calculated results?
            public bool NeedsSummaryView { get; set; }
            public bool NeedsFullView { get; set; }
            public string MiscDocPath { get; set; }
            //2.0.0 paths are set using appsettings
            //all web and file paths are derived from these
            public string DefaultRootFullFilePath { get; set; }
            public string DefaultRootWebStoragePath { get; set; }
            public string DefaultWebDomain { get; set; }
            //derived full paths 
            //set from above 3 config plus subpaths such as uripatterns
            public string FileSystemPath { get; set; }
            public string WebPath { get; set; }
            public string DefaultConnection { get; set; }
            public string StorageConnection { get; set; }
            public string FileSizeValidation { get; set; }
            public string FileSizeDBStorageValidation { get; set; }
            public string PageSizeEdits { get; set; }
            public string RExecutable { get; set; }
            public string PyExecutable { get; set; }
            public string JuliaExecutable { get; set; }
            public string HostFeeRate { get; set; }
            public string ResourceURIName { get; set; }
            public string ContentURIName { get; set; }
            public string TempDocsURIName { get; set; }
            public string ExtensionsPath { get; set; }
            public Helpers.FileStorageIO.PLATFORM_TYPES PlatformType { get; set; }
            public static void CopyLinkedView(DataManager mngrFrom,
                DataManager mngrTo)
            {
                if (mngrFrom.LinkedView != null)
                {
                    mngrTo.LinkedView
                        = new List<System.Linq.IGrouping<int, ContentURI>>();
                    foreach (var linkedviewparent in mngrFrom.LinkedView)
                    {
                        mngrTo.LinkedView.Add(linkedviewparent);
                    }
                }
            }
            //add a resource to an uri's resource list with no db hit
            public string AddResourceToResourceList(
                ContentURI uri, string resourceURIPattern, string resourceAlt,
                string resourcePath, string resourceExtensionObjectNameSpace)
            {
                string sResourceURIPattern = resourceURIPattern;
                ContentURI oResourceURI = new ContentURI();
                DevTreks.Data.Helpers.AppSettings.CopyURIAppSettings(uri, oResourceURI);
                if ((!string.IsNullOrEmpty(sResourceURIPattern))
                    || ((!string.IsNullOrEmpty(resourcePath)
                    && resourcePath != GenHelpers.NONE)))
                {
                    //160 moved resource generation to ContentViewPanel
                    //bool bNeedsResourcePath = false;
                    if (string.IsNullOrEmpty(sResourceURIPattern)
                        && (!string.IsNullOrEmpty(resourcePath)))
                    {
                        //bNeedsResourcePath = true;
                        sResourceURIPattern
                            = AppHelpers.Resources.GetURIPatternFromResourcePath(resourcePath);
                        //this is the preferred way to set uris because it includes parent resourcepackid
                        AppHelpers.Resources.SetURIandParentURIPatternFromResourcePath(
                            resourcePath, oResourceURI);

                    }
                    if (!string.IsNullOrEmpty(sResourceURIPattern))
                    {
                        if (oResourceURI.URIPattern == string.Empty)
                        {
                            oResourceURI.ChangeURIPatternNoDbHit(sResourceURIPattern);
                        }
                        //relative path to resources need contextrp
                        AppHelpers.Resources.AddParentURIPropertiesToResourceURI(uri, oResourceURI);
                        if (oResourceURI.URIId != 0)
                        {
                            //standard storage i/o works with relative paths (later converted to full paths for display)
                            oResourceURI.URIDataManager.FileSystemPath = resourcePath;
                            
                            uri.URIDataManager.FileSystemPath = oResourceURI.URIDataManager.FileSystemPath;
                            uri.URIDataManager.LongDescription = resourceAlt;
                            oResourceURI.URIDataManager.ExtensionObjectNamespace = resourceExtensionObjectNameSpace;
                            oResourceURI.URIDataManager.Description = resourceAlt;
                            AppHelpers.Resources.SetResourceDefaults(uri, oResourceURI,
                                resourceExtensionObjectNameSpace);
                            oResourceURI.URIDataManager.AppType = GenHelpers.APPLICATION_TYPES.resources;
                            oResourceURI.URIDataManager.SubAppType = GenHelpers.SUBAPPLICATION_TYPES.resources;
                            if (uri.URIDataManager.Resource == null)
                                uri.URIDataManager.Resource = new List<ContentURI>();
                            uri.URIDataManager.Resource.Add(oResourceURI);
                        }
                    }
                }
                return sResourceURIPattern;
            }
            //add a resource to an uri's resource list with potential db hit
            public async Task<string> AddResourceToResourceListAsync(
                ContentURI uri, string resourceURIPattern, string resourceAlt,
                string resourcePath, string resourceExtensionObjectNameSpace)
            {
                string sResourceURIPattern = resourceURIPattern;
                if ((!string.IsNullOrEmpty(sResourceURIPattern))
                    || ((!string.IsNullOrEmpty(resourcePath)
                    && resourcePath != GenHelpers.NONE)))
                {
                    bool bNeedsResourcePath = false;
                    if (string.IsNullOrEmpty(sResourceURIPattern)
                        && (!string.IsNullOrEmpty(resourcePath)))
                    {
                        bNeedsResourcePath = true;
                        sResourceURIPattern
                            = AppHelpers.Resources.GetURIPatternFromResourcePath(resourcePath);
                    }
                    if (!string.IsNullOrEmpty(sResourceURIPattern))
                    {
                        ContentURI oResourceURI = new ContentURI();
                        DevTreks.Data.Helpers.AppSettings.CopyURIAppSettings(uri, oResourceURI);
                        oResourceURI.ChangeURIPatternNoDbHit(sResourceURIPattern);
                        //relative path to resources need contextrp
                        AppHelpers.Resources.AddParentURIPropertiesToResourceURI(uri, oResourceURI);
                        if (oResourceURI.URIId != 0)
                        {
                            sResourceURIPattern = oResourceURI.URIPattern;
                            if (resourcePath == string.Empty || bNeedsResourcePath)
                            {
                                bool bNeedsFullPath = false;
                                AppHelpers.Resources oResource = new AppHelpers.Resources();
                                //gets the resource path and only stores resource if it doesn't exist
                                await oResource.GetPathAndStoreResourceAsync(oResourceURI, resourcePath, bNeedsFullPath);
                            }
                            else
                            {
                                oResourceURI.URIDataManager.FileSystemPath = resourcePath;
                            }
                            uri.URIDataManager.FileSystemPath = oResourceURI.URIDataManager.FileSystemPath;
                            uri.URIDataManager.LongDescription = resourceAlt;
                            oResourceURI.URIDataManager.ExtensionObjectNamespace = resourceExtensionObjectNameSpace;
                            oResourceURI.URIDataManager.Description = resourceAlt;
                            AppHelpers.Resources.SetResourceDefaults(uri, oResourceURI,
                                resourceExtensionObjectNameSpace);
                            oResourceURI.URIDataManager.AppType = GenHelpers.APPLICATION_TYPES.resources;
                            oResourceURI.URIDataManager.SubAppType = GenHelpers.SUBAPPLICATION_TYPES.resources;
                            if (uri.URIDataManager.Resource == null)
                                uri.URIDataManager.Resource = new List<ContentURI>();
                            uri.URIDataManager.Resource.Add(oResourceURI);
                        }
                    }
                }
                return sResourceURIPattern;
            }
        }
        /// <summary>
        ///Purpose:		Manage models needed to load and display uri content
        ///Author:		www.devtreks.org
        ///Date:		2016, March
        ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
        /// </summary>
        public class Models
        {
            public Models(){}

            //admin models
            public MemberClass MemberClass { get; set; }
            public Member Member { get; set; }
            public AccountToMember AccountToMember { get; set; }
            public AccountClass AccountClass { get; set; }
            public Account Account { get; set; }
            public NetworkClass NetworkClass { get; set; }
            public Network Network { get; set; }
            public AccountToNetwork AccountToNetwork { get; set; }
            public ServiceClass ServiceClass { get; set; }
            public Service Service { get; set; }
            public AccountToService AccountToService { get; set; }
            public AccountToAddIn AccountToAddIn { get; set; }
            public AccountToLocal AccountToLocal { get; set; }
            public AccountToIncentive AccountToIncentive { get; set; }

            //content models
            public List<ResourceType> ResourceType { get; set; }
            public ResourceClass ResourceClass { get; set; }
            public ResourcePack ResourcePack { get; set; }
            public DevTreks.Models.Resource Resource { get; set; }
            public List<LinkedViewType> LinkedViewType { get; set; }
            public LinkedViewClass LinkedViewClass { get; set; }
            public LinkedViewPack LinkedViewPack { get; set; }
            public LinkedView LinkedView { get; set; }
            public LinkedViewToResourcePack LinkedViewToResourcePack { get; set; }

            public List<DevPackType> DevPackType { get; set; }
            public DevPackClass DevPackClass { get; set; }
            public DevPackClassToDevPack DevPackClassToDevPack { get; set; }
            public DevPackToDevPackPart DevPackToDevPackPart { get; set; }
            public DevPackPartToResourcePack DevPackPartToResourcePack { get; set; }
            public DevPack DevPack { get; set; }
            public DevPackPart DevPackPart { get; set; }

            public List<InputType> InputType { get; set; }
            public InputClass InputClass { get; set; }
            public Input Input { get; set; }
            public InputSeries InputSeries { get; set; }
            public List<OutputType> OutputType { get; set; }
            public OutputClass OutputClass { get; set; }
            public Output Output { get; set; }
            public OutputSeries OutputSeries { get; set; }
            public List<OperationType> OperationType { get; set; }
            public OperationClass OperationClass { get; set; }
            public Operation Operation { get; set; }
            public OperationToInput OperationToInput { get; set; }
            public List<ComponentType> ComponentType { get; set; }
            public ComponentClass ComponentClass { get; set; }
            public Component Component { get; set; }
            public ComponentToInput ComponentToInput { get; set; }
            public List<OutcomeType> OutcomeType { get; set; }
            public OutcomeClass OutcomeClass { get; set; }
            public Outcome Outcome { get; set; }
            public OutcomeToOutput OutcomeToOutput { get; set; }
            public List<BudgetSystemType> BudgetSystemType { get; set; }
            public BudgetSystem BudgetSystem { get; set; }
            public BudgetSystemToEnterprise BudgetSystemToEnterprise { get; set; }
            public BudgetSystemToTime BudgetSystemToTime { get; set; }
            public BudgetSystemToOutput BudgetSystemToOutput { get; set; }
            public BudgetSystemToOperation BudgetSystemToOperation { get; set; }
            public BudgetSystemToOutcome BudgetSystemToOutcome { get; set; }
            public BudgetSystemToInput BudgetSystemToInput { get; set; }
            public List<CostSystemType> CostSystemType { get; set; }
            public CostSystem CostSystem { get; set; }
            public CostSystemToPractice CostSystemToPractice { get; set; }
            public CostSystemToTime CostSystemToTime { get; set; }
            public CostSystemToOutput CostSystemToOutput { get; set; }
            public CostSystemToOutcome CostSystemToOutcome { get; set; }
            public CostSystemToComponent CostSystemToComponent { get; set; }
            public CostSystemToInput CostSystemToInput { get; set; }
        }
    }
}
