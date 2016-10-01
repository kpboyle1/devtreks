using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevTreks.Models;
using DataGeneralHelpers = DevTreks.Data.Helpers.GeneralHelpers;
using DataHelpers = DevTreks.Data.Helpers;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The ExtensionContentURI class is a class used by all extensions. 
    ///             Its purpose is to keep the programming in the extensions similar 
    ///             to the programming in the rest of DevTreks (which is ContentURI 
    ///             centric). ContentURIs also serve to package most of the parameters 
    ///             used throughout DevTreks. It also allows additional display 
    ///             instructions to be passed back from extensions to the 
    ///             DevTreks view layer. The plan is to give extension builders 
    ///             greater control over how their extensions are viewed and displayed.
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///Notes
    ///</summary>
    public class ExtensionContentURI 
    {
        public ExtensionContentURI() 
        {
            //common name = db key Name
            this.URIName = DataGeneralHelpers.NONE;
            //unique id of the uri = db key PKId
            this.URIId = 0;
            //network of the uri (see DevTreks.Data.Network notes)
            this.URINetworkPartName = string.Empty;
            //nodename = db table
            this.URINodeName = DataGeneralHelpers.NONE;
            //mostly used by tempdocs, or by analyzers to find base calculations
            this.URIFileExtensionType = string.Empty;
            this.URIPattern = DataGeneralHelpers.MakeURIPattern(this.URIName,
                this.URIId.ToString(), this.URINetworkPartName.ToString(),
                this.URINodeName, this.URIFileExtensionType);
            //representational state transfer uri (i.e. the Internet address of this uri)
            this.URIFull = string.Empty;
            //club that owns the uri
            this.URIClub = new ExtensionClub();
            //logged-in member or anonymous
            this.URIMember = new ExtensionMember();
            //the uri's network, including data connections
            this.URINetwork = new ExtensionNetwork();
            //the uri's service (commons apps have no services)
            this.URIService = new ExtensionService();
            //navigation, state management, and display management
            this.URIDataManager = new DataManager();
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
        public ExtensionContentURI(ExtensionContentURI uri)
        {
            //make a copy of uri
            this.URIName = uri.URIName;
            this.URIId = uri.URIId;
            this.URINetworkPartName = uri.URINetworkPartName;
            this.URINodeName = uri.URINodeName;
            this.URIFileExtensionType = uri.URIFileExtensionType;
            this.URIPattern = uri.URIPattern;
            this.URIFull = uri.URIFull;
            this.URIClub = new ExtensionClub(uri.URIClub);
            this.URIMember = new ExtensionMember(uri.URIMember);
            this.URINetwork = new ExtensionNetwork(uri.URINetwork);
            this.URIService = new ExtensionService(uri.URIService);
            this.URIDataManager
                = new ExtensionContentURI.DataManager(uri.URIDataManager);
            this.ErrorMessage = string.Empty;
            this.Message = uri.Message;
            this.Json = uri.Json;
            this.SchemaPath = uri.SchemaPath;
        }
        public ExtensionContentURI(DevTreks.Data.ContentURI uri) 
        {
            //make a copy of uri
            this.URIName = uri.URIName;
            this.URIId = uri.URIId;
            this.URINetworkPartName = uri.URINetworkPartName;
            this.URINodeName = uri.URINodeName;
            this.URIFileExtensionType = uri.URIFileExtensionType;
            this.URIPattern = uri.URIPattern;
            this.URIFull = uri.URIFull;
            this.URIClub = new ExtensionClub(uri.URIClub);
            this.URIMember = new ExtensionMember(uri.URIMember);
            this.URINetwork = new ExtensionNetwork(uri.URINetwork);
            this.URIService = new ExtensionService(uri.URIService);
            this.URIDataManager 
                = new ExtensionContentURI.DataManager(uri.URIDataManager);
            this.URIDataManager.InitialDocToCalcURI = uri;
            this.ErrorMessage = string.Empty;
            this.Message = uri.Message;
            this.Json = uri.Json;
            this.SchemaPath = uri.SchemaPath;
        }
        
        public string URIName { get; set; }
        public int URIId { get; set; }
        //internally (on server and to machines) this can be urinetworkid,
        //externally (what humans see) it should be the networkpartname
        public string URINetworkPartName { get; set; }
        public string URINodeName { get; set; }
        public string URIFileExtensionType { get; set; }
        //delimited string sent in route pattern: {networkuripartname}/{nodename}/{commonname}/{id}/{fileextensiontype},
        public string URIPattern { get; set; }
        //full uri www.devtreks.org/agtreks/linkedviews/{networkuripartname}/{nodename}/{commonname}/{id}/{fileextensiontype},
        public string URIFull { get; set; }

        //owner of the uri
        public ExtensionClub URIClub { get; set; }
        //member using the uri (including clubinuse.authorization level for using uri)
        //if not logged-in, URIMember is anonymous and given id=0
        public ExtensionMember URIMember { get; set; }
        //network containing the uri (holding data connections)
        public ExtensionNetwork URINetwork { get; set; }
        //service, if applicable, containing the uri 
        //(commons objects are not part of service agreements -they provide 
        //networking services but not formal story or number services)
        public ExtensionService URIService { get; set; }
        //parameters needed to load and display the uri
        public DataManager URIDataManager { get; set; }

        //error handling
        public string ErrorMessage { get; set; }
        //in lieu of responding with new views, respond only with a string message
        public string Message { get; set; }
        //respond with javascript (i.e. stop showing progress and signal success)
        public string Json { get; set; }
        //path to the xml schema defining the data returned by the uri
        public string SchemaPath { get; set; }

        /// <summary>
        ///Purpose:		Manage params needed to load and display uri content
        ///Author:		www.devtreks.org
        ///Date:		2016, March
        ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
        /// </summary>
        public class DataManager
        {
            public DataManager()
            {
                this.ContentURIPattern = string.Empty;
                this.InitialDocToCalcURI = new Data.ContentURI();
                this.ControllerName = string.Empty;
                this.ClientActionType = string.Empty;
                this.ServerActionType = string.Empty;
                this.ServerSubActionType = string.Empty;
                this.Variable = string.Empty;
                this.FormInput = null;
                this.SelectionsNodeNeededName = string.Empty;
                this.SelectedList = string.Empty;
                this.SelectionsURIPattern = string.Empty;
                this.SelectionsNodeURIPattern = string.Empty;
                this.SelectionsAttributeName = string.Empty;
                this.CalcParams = string.Empty;
                this.DocStatus = string.Empty;
                this.AppType = string.Empty;
                this.SubAppType = string.Empty;

                this.ParentURIPattern = string.Empty;
                this.ChildrenNodeName = string.Empty;
                this.ParentStartRow = 0;
                this.StartRow = 0;
                this.IsForward = "1";
                this.PageSize = 0;
                this.RowCount = 0;

                this.UpdatePanelType = string.Empty;
                this.ParentPanelType = string.Empty;
                this.ChildrenPanelType = string.Empty;

                this.PreviewViewEditType = string.Empty;
                this.EditViewEditType = string.Empty;
                this.SelectViewEditType = string.Empty;
                this.Label = string.Empty;
                this.AttributeName = string.Empty;
                this.Date = DataGeneralHelpers.GetDateShortNow();
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
                this.PlatformType = DataHelpers.FileStorageIO.PLATFORM_TYPES.none;

                this.Resource = null;
                this.IsMainStylesheet = false;
                this.IsMainImage = false;
                
                this.ExtensionObjectNamespace = string.Empty;
                this.Description = string.Empty;
                this.LongDescription = string.Empty;
                this.NeedsFullView = false;
                this.NeedsSummaryView = false;
            }
            public DataManager(DataManager mngr)
            {
                this.ContentURIPattern = mngr.ContentURIPattern;
                this.InitialDocToCalcURI = mngr.InitialDocToCalcURI;
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

                //avoid copying collections, should not be needed in calcs
                //CopyExtensionContentURILinkedView(mngr, this);

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

                //avoid copying collections, should not be needed in calcs
                //this.Resource = Helpers.LinqHelpers.CopyContentURIs(mngr.Resource);
                this.IsMainStylesheet = mngr.IsMainStylesheet;
                this.IsMainImage = mngr.IsMainImage;
                
                this.ExtensionObjectNamespace = mngr.ExtensionObjectNamespace;
                this.Description = mngr.Description;
                this.LongDescription = mngr.LongDescription;
                this.NeedsFullView = mngr.NeedsFullView;
                this.NeedsSummaryView = mngr.NeedsSummaryView;
            }

            public DataManager(DevTreks.Data.ContentURI.DataManager mngr)
            {
                this.ContentURIPattern = mngr.ContentURIPattern;
                //this must be set in calling procedure
                //this.InitialDocToCalcURI = mngr.Initial;
                this.ControllerName = mngr.ControllerName;
                this.ClientActionType = mngr.ClientActionType.ToString();
                this.ServerActionType = mngr.ServerActionType.ToString();
                this.ServerSubActionType = mngr.ServerSubActionType.ToString();
                this.SelectionsNodeNeededName = mngr.SelectionsNodeNeededName;
                this.SelectedList = mngr.SelectedList;
                this.SelectionsURIPattern = mngr.SelectionsURIPattern;
                this.SelectionsNodeURIPattern = mngr.SelectionsNodeURIPattern;
                this.SelectionsAttributeName = mngr.SelectionsAttributeName;
                this.CalcParams = mngr.CalcParams;
                this.DocStatus = mngr.DocStatus.ToString();
                this.AppType = mngr.AppType.ToString();
                this.SubAppType = mngr.SubAppType.ToString();

                this.ParentURIPattern = mngr.ParentURIPattern;
                this.ChildrenNodeName = mngr.ChildrenNodeName;

                this.ParentStartRow = mngr.ParentStartRow;
                this.StartRow = mngr.StartRow;
                this.IsForward = mngr.IsForward;
                this.PageSize = mngr.PageSize;
                this.RowCount = mngr.RowCount;

                this.UpdatePanelType = mngr.UpdatePanelType.ToString();
                this.ParentPanelType = mngr.ParentPanelType.ToString();
                this.ChildrenPanelType = mngr.ChildrenPanelType.ToString();

                this.PreviewViewEditType = mngr.PreviewViewEditType.ToString();
                this.EditViewEditType = mngr.EditViewEditType.ToString();
                this.SelectViewEditType = mngr.SelectViewEditType.ToString();
                this.Label = mngr.Label;
                this.AttributeName = mngr.AttributeName;
                this.UserLanguage = mngr.UserLanguage;
                this.Date = mngr.Date;
                //avoid copying collections, should not be needed in calcs
                //CopyExtensionContentURILinkedView(mngr, this);
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

                //avoid copying collections, should not be needed in calcs
                //this.Resource = CopyContentURIs(mngr.Resource);
                this.IsMainStylesheet = mngr.IsMainStylesheet;
                this.IsMainImage = mngr.IsMainImage;
                
                this.ExtensionObjectNamespace = mngr.ExtensionObjectNamespace;
                this.Description = mngr.Description;
                this.LongDescription = mngr.LongDescription;
                this.NeedsFullView = mngr.NeedsFullView;
                this.NeedsSummaryView = mngr.NeedsSummaryView;
            }
            
            public string ContentURIPattern { get; set; }
            public DevTreks.Data.ContentURI InitialDocToCalcURI { get; set; }
            public string ControllerName { get; set; }
            //client-side instruction usually sent in an ajax request
            //client-side instruction usually sent in an ajax request
            public string ClientActionType { get; set; }
            //search, preview, select, edit, pack, view
            public string ServerActionType { get; set; }
            //replaces typical postback event handlers (i.e. login, logout, calculate ...)
            public string ServerSubActionType { get; set; }
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
            public string DocStatus { get; set; }

            //app properties
            public string AppType { get; set; }
            public string SubAppType { get; set; }

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
            public string UpdatePanelType { get; set; }
            public string ParentPanelType { get; set; }
            public string ChildrenPanelType { get; set; }
            public string PreviewViewEditType { get; set; }
            public string EditViewEditType { get; set; }
            public string SelectViewEditType { get; set; }
            //misc
            public string UserLanguage { get; set; }
            public string Label { get; set; }
            public string AttributeName { get; set; }
            //last edited date 
            public DateTime Date { get; set; }

            //ancestors of the uri
            public IList<ExtensionContentURI> Ancestors { get; set; }
            //children of the uri
            public IList<ExtensionContentURI> Children { get; set; }

            //Berners-Lee also advises that a site's uris should return "linked data"
            //the semantic web aspects of LinkedView will evolve
            //linked views of the uri: calculations, analyses, and other new views
            public IList<System.Linq.IGrouping<int, ExtensionContentURI>> LinkedView { get; set; }
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

            //in the case of join tables, such as linkedviewtooperation, 
            //this will be the base table id, such as linkedviewid
            public int BaseId { get; set; }
            //tempdocs usually store addin calcs temporarily (no db relations)
            public string TempDocURIPattern { get; set; }
            //the node to calc is usually different than the doctocalc when using full tempdocs 
            public string TempDocNodeToCalcURIPattern { get; set; }
            public string TempDocPath { get; set; }
            public string TempDocSaveMethod { get; set; }
            public string MiscDocPath { get; set; }
            //2.0.0 paths are set using appsettings
            //all web and file paths are derived from these
            public string DefaultRootFullFilePath { get; set; }
            public string DefaultRootWebStoragePath { get; set; }
            public string DefaultWebDomain { get; set; }
            //paths used to load resources such as images and stylesheets
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
            public DataHelpers.FileStorageIO.PLATFORM_TYPES PlatformType { get; set; }
            //resources (images, stylesheets) of the uri 
            public IList<ExtensionContentURI> Resource { get; set; }
            //main stylesheet used to transform uri
            public bool IsMainStylesheet { get; set; }
            //main image to show with uri 
            public bool IsMainImage { get; set; }
            
            //stylesheet extension objects
            public string ExtensionObjectNamespace { get; set; }
            //also used for a resource element's alt and long description attributes
            public string Description { get; set; }
            public string LongDescription { get; set; }
            //instructions for displaying calcultor and calculated results
            public bool NeedsSummaryView { get; set; }
            public bool NeedsFullView { get; set; }
            
        }
    }
}
