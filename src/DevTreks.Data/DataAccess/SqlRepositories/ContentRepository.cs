using DevTreks.Data.DataAccess;
using DevTreks.Data.EditHelpers;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using GenHelpers = DevTreks.Data.Helpers.GeneralHelpers;

namespace DevTreks.Data.SqlRepositories
{
    /// <summary>
    ///Purpose:		Principal repository for accessing DevTreks content
    ///Author:		www.devtreks.org
    ///Date:		2016, August
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class ContentRepository : IContentRepositoryEF, IDisposable
    {
        //2.0.0 changes
        //this uri is only used to configure datacontext
        //most class methods include a complete uri, with full route, context, and data settings
        public ContentRepository(ContentURI uri)
        {
            //example for potential network-specific dbs, 
            //but must be set from route params during uri construction on UI layer
            //if (string.IsNullOrEmpty(Helpers.AppSettings.GetConnection(uri)))
            //{
            //    if (uri.URINetwork != null)
            //    {
            //        uri.URIDataManager.DefaultConnection
            //            = uri.URINetwork.WebConnection;
            //    }
            //}
            //pass the generic config settings to a dbcontextoptionsbldr
            var builder = new DbContextOptionsBuilder<DevTreksContext>();
            builder.UseSqlServer(Helpers.AppSettings.GetConnection(uri));
            _dataContext = new DevTreksContext(builder.Options);

            //retain for testing file uploads
            //_dataContext.BinaryMaxLength = GenHelpers.ConvertMBStorageToInt(
            //    uri.URIDataManager.FileSizeDBStorageValidation);

            //Do not use _sqlIOAsync(uri) here. Pass a method parameter uri to 
            //a new SqlIOAsync(uri). That uri contains more that just config settings 
            //and is often needed to build the full data models.
        }
        //EF data access (supplemented with direct calls to sqlio for sqlclient)
        DevTreksContext _dataContext { get; set; }
        
        /// <summary>
        ///get current uri's ancestors
        /// </summary>
        /// <returns>List of ContentURI (ancestors of current uri)</returns>
        public async Task<List<ContentURI>> GetAncestorsAsync(
            ContentURI uri)
        {
            //get the ancestor records from the db as a string array 
            //(these sps are better than alternatives)
            string sAncestorURIPatternArray = string.Empty;
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            List<ContentURI> colURIs = new List<ContentURI>();
            if (uri.URIDataManager.AppType == GenHelpers.APPLICATION_TYPES.accounts
                || uri.URIDataManager.AppType == GenHelpers.APPLICATION_TYPES.members
                || uri.URIDataManager.AppType == GenHelpers.APPLICATION_TYPES.networks
                || uri.URIDataManager.AppType == GenHelpers.APPLICATION_TYPES.locals
                || uri.URIDataManager.AppType == GenHelpers.APPLICATION_TYPES.addins)
            {
                sAncestorURIPatternArray = await oContentHelper.GetAdminAncestorsAsync(uri);
            }
            else
            {
                sAncestorURIPatternArray = await oContentHelper.GetURIAncestorsAsync(uri);
            }
            //fill an List with the array
            colURIs = FillContentList(uri, sAncestorURIPatternArray);
            return colURIs;
        }
        public async Task<List<ContentURI>> GetAgreementAncestorsAndSetServiceAsync(
            ContentURI uri)
        {
            //get the ancestor records from the db as a string array 
            string sAncestorURIPatternArray = string.Empty;
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            List<ContentURI> colURIs = new List<ContentURI>();
            sAncestorURIPatternArray = await oAgreementHelper.GetAncestorsAndSetServiceAsync(uri);
            //fill an List with the array
            colURIs = FillContentList(uri, sAncestorURIPatternArray);
            return colURIs;
        }
        public async Task<List<ContentURI>> GetAgreementAncestorsAndAuthorizationsAsync(
            ContentURI uri, int clubOrMemberId)
        {
            //get the ancestor records from the db as a string array 
            string sAncestorURIPatternArray = string.Empty;
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            List<ContentURI> colURIs = new List<ContentURI>();
            sAncestorURIPatternArray 
                = await oAgreementHelper.GetAncestorsAndAuthorizationsAsync(uri, clubOrMemberId);
            //fill a List with the array
            colURIs = FillContentList(uri, sAncestorURIPatternArray);
            return colURIs;
        }
        private List<ContentURI> FillContentList(ContentURI uri,
           string ancestorNameArray)
        {
            List<ContentURI> colAncestors = new List<ContentURI>();
            if (string.IsNullOrEmpty(ancestorNameArray) == false)
            {
                //ancestor name array is a delimited string of ContentURI.URIPatterns
                string[] arrAncestorURIs = ancestorNameArray.Split(GenHelpers.PARAMETER_DELIMITERS);
                if (arrAncestorURIs != null)
                {
                    int i = 0;
                    int iAncestorNamesLength = arrAncestorURIs.Length;
                    string sAncestorURI = string.Empty;
                    for (i = 0; i < iAncestorNamesLength; i++)
                    {
                        sAncestorURI = arrAncestorURIs[i];
                        ContentURI ancestor = new ContentURI();
                        ancestor.ChangeURIPatternNoDbHit(sAncestorURI);
                        bool bNeedsAncestors = false;
                        Helpers.ContentHelper.UpdateNewURIArgs(uri, ancestor, bNeedsAncestors);
                        colAncestors.Add(ancestor);
                    }
                }
            }
            return colAncestors;
        }
        public async Task<List<GeoRegion>> GetGeoRegionsAsync(ContentURI uri)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<GeoRegion> colGeoRegions = await oClubHelper.GetGeoRegionsAsync(uri);
            //152 uses ef only to edit content
            //var q = await _dataContext.GeoRegions
            //    .OrderBy(g => g.GeoRegionName)
            //    .ToListAsync();
            return colGeoRegions;
        }
        public async Task<List<AccountClass>> GetClubGroupsAsync(ContentURI uri)
        {
            AppHelpers.Accounts oClubHelper = new AppHelpers.Accounts();
            List<AccountClass> colClubGroups = await oClubHelper.GetClubGroupsAsync(uri);
            //152 uses ef only to edit content
            //List<AccountClass> q = await _dataContext.AccountClass.ToListAsync();
            return colClubGroups;
        }
        public async Task<List<MemberClass>> GetMemberGroupsAsync(ContentURI uri)
        {
            AppHelpers.Members oMemberHelper = new AppHelpers.Members();
            List<MemberClass> colMemberGroups = await oMemberHelper.GetMemberGroupsAsync(uri);
            return colMemberGroups;
        }
        public async Task<bool> SetServiceAsync(ContentURI uri,
            bool isBaseService)
        {
            bool bHasSet = false;
            AppHelpers.Agreement oAgreementHelper = new AppHelpers.Agreement();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader services = await oAgreementHelper.GetServiceAsync(sqlIO, uri, isBaseService);
            uri.URIService = oAgreementHelper.FillServiceObject(services);
            sqlIO.Dispose();
            bHasSet = true;
            return bHasSet;
        }
        public async Task<bool> SetServiceAndChangeApplicationAsync(ContentURI uri, int serviceId)
        {
            bool bNeedsNewApp = false;
            bool bHasSet = false;
            DevTreks.Data.AppHelpers.Agreement oAgreement = new AppHelpers.Agreement();
            if (uri.URINodeName.Equals(
                AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()))
            {
                //db search needed to figure the application/subapplication when a service nodename clicked
                //uses accounttoservice.pkid to return serviceId = service.pkid
                if (uri.URIId != 0)
                {
                    bNeedsNewApp = true;
                    bHasSet = await oAgreement.ChangeApplicationAndServiceAsync(
                        uri, serviceId, bNeedsNewApp);
                }
                else
                {
                    //this a services search, set the service to default
                    uri.URIService = new AccountToService();
                    uri.URIService.Service = new Service();
                    uri.URIService.Service.ServiceClassId = (int)Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.agreements;
                    uri.URIService.IsSelected = true;
                }
            }
            else if (uri.URINodeName.Equals(
                AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()))
            {
                if (uri.URIDataManager.ServerSubActionType
                    == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                    || uri.URIDataManager.ServerSubActionType
                    == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
                {
                    //set the service but don't change the app
                    bHasSet = await oAgreement.ChangeApplicationAndServiceAsync(uri, 
                        serviceId, bNeedsNewApp);
                }
                else
                {
                    //db search needed to figure the application/subapplication when a service nodename clicked from within an app
                    bHasSet = await oAgreement.ChangeApplicationAndServiceFromBaseServiceAsync(uri);
                }
            }
            else
            {
                //set the service but don't change the app
                bHasSet = await oAgreement.ChangeApplicationAndServiceAsync(
                    uri, serviceId, bNeedsNewApp);
            }
            return bHasSet;
        }
        public async Task<bool> SetClubByServiceAsync(ContentURI serviceURI)
        {
            AppHelpers.Accounts oAccountHelper = new AppHelpers.Accounts();
            bool bHasSet = await oAccountHelper.SetClubOwnerByServiceAsync(serviceURI);
            return bHasSet;
        }
        //get the uri.uriservice.networkcategories filtered by a network and supapptype list of categories
        public async Task<List<ContentURI>> GetNetworkCategoriesAsync(ContentURI serviceURI)
        {
            DevTreks.Data.AppHelpers.Agreement oAgreement = new AppHelpers.Agreement();
            List<ContentURI> colCategories = await oAgreement.GetNetworkCategoriesAsync(serviceURI);
            return colCategories;
        }

        /// <summary>
        ///Returns the children of parentURI
        /// </summary>
        /// <returns>List of ContentURI (children of current uri)</returns>
        public async Task<List<ContentURI>> GetChildrenAsync(ContentURI parentURI,
            ContentURI uri)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            ContentURI childrenURIParent = new ContentURI();
            SqlDataReader childrenReader = await oContentHelper.GetURIChildrenAsync(sqlIO, parentURI, uri,
                childrenURIParent);
            //fill an List with the qry results
            int iNewRowCount = uri.URIDataManager.RowCount;
            List<ContentURI> colURIs
                = Helpers.ContentHelper.FillChildrenListandSetStartRow(
                    childrenURIParent, uri, childrenReader);
            uri.URIDataManager.RowCount = iNewRowCount;
            sqlIO.Dispose();
            return colURIs;
        }
        public async Task<bool> CopyResourceToPackageAsync(
            ContentURI uri, string resourceRelFilePaths,
            int arrayPos, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied 
                = await Helpers.PackageIO.CopyResourceToPackageAsync(
                    uri, resourceRelFilePaths, arrayPos, rootDirectory, newDirectory,
                    parentFileName, zipArgs);
            return bHasCopied;
        }
        public async Task<bool> CopyRelatedDataToPackageAsync(
            ContentURI uri, string currentFilePath, string packageName,
            string fileType, string tempPackageRootDirectory, bool needsAllRelatedData,
            IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = await Helpers.PackageIO.CopyRelatedDataToPackageAsync(
                uri, currentFilePath, packageName,
                fileType, tempPackageRootDirectory, needsAllRelatedData, zipArgs);
            return bHasCopied;
        }
        public async Task<bool> PackageFilesAsync(ContentURI uri, string packageFilePathName,
            string packageType, string digitalSignatureType,
            IDictionary<string, string> zipArgs)
        {
            Helpers.PackageIO packIO = new Helpers.PackageIO();
            //zip archive does not have async methods
            bool bHasSet = packIO.PackageFiles(uri, packageFilePathName,
                packageType, digitalSignatureType, zipArgs);
            return bHasSet;
        }

        public async Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetLinkedViewAsync(
            ContentURI uri)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader linkedviews = await oContentHelper.GetLinkedViewPageAsync(sqlIO, uri);
            //fill an List with the linked views
            //even if linkedviews.hasrows == false, still want a uri.uridatamanager.linkedviews.count = 0
            List<ContentURI> colURIs = new List<ContentURI>();
            bool bNeedsDefaultRecords = false;
            if (uri.URIDataManager.AppType
                != Helpers.GeneralHelpers.APPLICATION_TYPES.resources)
            {
                if (uri.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    //recursive lists need more ways to navigate
                    if (uri.URIDataManager.SubActionView
                        == Helpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                    {
                        //paginated linkedviews
                        colURIs = FillLinkedViewList(uri, linkedviews);
                    }
                    else
                    {
                        if (uri.URIDataManager.IsSelectedLinkedView == false)
                        {
                            //non-paginated, recursive children
                            colURIs = FillChildrenViewsList(uri, linkedviews, bNeedsDefaultRecords);
                        }
                        else
                        {
                            //paginated linkedviews
                            colURIs = FillLinkedViewList(uri, linkedviews);
                        }

                    }
                }
                else
                {
                    //paginated linkedviews
                    colURIs = FillLinkedViewList(uri, linkedviews);
                }
            }
            else
            {
                if (uri.URINodeName
                    == AppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                {
                    //a8b: regular paginated linkedviews
                    colURIs = FillLinkedViewList(uri, linkedviews);
                }
                else
                {
                    colURIs = FillOtherViewsList(uri, linkedviews);
                }
            }
            sqlIO.Dispose();
            Helpers.LinqHelpers.AdjustForDefaultLinkedView(uri, colURIs);
            //group the list(queryGroupURIs in an List<IGrouping<int, ContentURI>>) 
            //using their unique parentid property (for display using a parent/children grouping)
            IEnumerable<System.Linq.IGrouping<int, ContentURI>>
                qryGroupURIs =
                from parent in colURIs
                group parent by ContentURI.GetGroupingParentId(parent.URIDataManager.ParentURIPattern)
                    into parents
                    select parents;
            return qryGroupURIs;
        }
        public async Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetLinkedViewForAnalysesAsync(
            ContentURI uri)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader linkedviews = await oContentHelper.GetLinkedViewForAnalysesPageAsync(sqlIO, uri);
            //fill an List with the linked views
            //even if linkedviews.hasrows == false, still want a uri.uridatamanager.linkedviews.count = 0
            List<ContentURI> colURIs = new List<ContentURI>();
            if (uri.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                //non-paginated, recursive children
                colURIs = FillLinkedViewList(uri, linkedviews);
            }
            sqlIO.Dispose();
            //group the list(queryGroupURIs in an List<IGrouping<int, ContentURI>>) 
            //using their unique parentid property (for display using a parent/children grouping)
            IEnumerable<System.Linq.IGrouping<int, ContentURI>>
                qryGroupURIs =
                from parent in colURIs
                group parent by ContentURI.GetGroupingParentId(parent.URIDataManager.ParentURIPattern)
                    into parents
                    select parents;
            return qryGroupURIs;
        }
        public async Task<bool> SaveURIFirstDocAsync(ContentURI uri)
        {
            bool bHasSaved = false;
            uri.ErrorMessage = string.Empty;
            switch (uri.URIDataManager.SubAppType)
            {
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.clubs:
                    AppHelpers.AccountModelHelper clubHelper
                        = new AppHelpers.AccountModelHelper(_dataContext, uri);
                    //fill in the data transfer object (uri.ResourceClass)
                    bHasSaved = await clubHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.members:
                    AppHelpers.MemberModelHelper memberHelper
                        = new AppHelpers.MemberModelHelper(_dataContext, uri);
                    bHasSaved = await memberHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.networks:
                    AppHelpers.NetworkModelHelper networkHelper
                        = new AppHelpers.NetworkModelHelper(_dataContext, uri);
                    bHasSaved = await networkHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.addins:
                    AppHelpers.AddInsModelHelper addinHelper
                        = new AppHelpers.AddInsModelHelper(_dataContext, uri);
                    bHasSaved = await addinHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.locals:
                    AppHelpers.LocalsModelHelper localHelper
                        = new AppHelpers.LocalsModelHelper(_dataContext, uri);
                    bHasSaved = await localHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.agreements:
                    AppHelpers.ServiceModelHelper servicesHelper
                        = new AppHelpers.ServiceModelHelper(_dataContext, uri);
                    bHasSaved = await servicesHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.resources:
                    AppHelpers.ResourceModelHelper resourceHelper
                        = new AppHelpers.ResourceModelHelper(_dataContext, uri);
                    bHasSaved = await resourceHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                    AppHelpers.InputModelHelper inputHelper
                        = new AppHelpers.InputModelHelper(_dataContext, uri);
                    bHasSaved = await inputHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                    AppHelpers.OutputModelHelper outputHelper
                        = new AppHelpers.OutputModelHelper(_dataContext, uri);
                    bHasSaved = await outputHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                    AppHelpers.OutcomeModelHelper outcomeHelper
                        = new AppHelpers.OutcomeModelHelper(_dataContext, uri);
                    bHasSaved = await outcomeHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                    AppHelpers.OperationModelHelper operationHelper
                        = new AppHelpers.OperationModelHelper(_dataContext, uri);
                    bHasSaved = await operationHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                    AppHelpers.ComponentModelHelper componentHelper
                        = new AppHelpers.ComponentModelHelper(_dataContext, uri);
                    bHasSaved = await componentHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                    AppHelpers.BudgetsModelHelper budgetHelper
                        = new AppHelpers.BudgetsModelHelper(_dataContext, uri);
                    bHasSaved = await budgetHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                    AppHelpers.InvestmentsModelHelper investmentHelper
                        = new AppHelpers.InvestmentsModelHelper(_dataContext, uri);
                    bHasSaved = await investmentHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.linkedviews:
                    AppHelpers.LinkedViewModelHelper linkedViewHelper
                        = new AppHelpers.LinkedViewModelHelper(_dataContext, uri);
                    bHasSaved = await linkedViewHelper.SaveURIFirstDocAsync();
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.devpacks:
                    AppHelpers.DevPackModelHelper devPackHelper
                        = new AppHelpers.DevPackModelHelper(_dataContext, uri);
                    bHasSaved = await devPackHelper.SaveURIFirstDocAsync();
                    break;
                default:
                    break;
            }
            return bHasSaved;
        }
        
        public async Task<bool> GetDevTrekContentAsync(ContentURI uri, bool saveInFileSystemContent)
        {
            bool bHasSaved = false;
            uri.ErrorMessage = string.Empty;
            switch (uri.URIDataManager.SubAppType)
            {
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.clubs:
                    AppHelpers.AccountModelHelper clubHelper
                        = new AppHelpers.AccountModelHelper(_dataContext, uri);
                    //fill in the data transfer object (uri.ResourceClass)
                    bHasSaved = await clubHelper.SetURIAccount(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.members:
                    AppHelpers.MemberModelHelper memberHelper
                        = new AppHelpers.MemberModelHelper(_dataContext, uri);
                    bHasSaved = await memberHelper.SetURIMember(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.networks:
                    AppHelpers.NetworkModelHelper networkHelper
                        = new AppHelpers.NetworkModelHelper(_dataContext, uri);
                    bHasSaved = await networkHelper.SetURINetwork(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.addins:
                    AppHelpers.AddInsModelHelper addinHelper
                        = new AppHelpers.AddInsModelHelper(_dataContext, uri);
                    bHasSaved = await addinHelper.SetURIAddIns(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.locals:
                    AppHelpers.LocalsModelHelper localHelper
                        = new AppHelpers.LocalsModelHelper(_dataContext, uri);
                    bHasSaved = await localHelper.SetURILocals(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.agreements:
                    AppHelpers.ServiceModelHelper servicesHelper
                        = new AppHelpers.ServiceModelHelper(_dataContext, uri);
                    bHasSaved = await servicesHelper.SetURIService(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.resources:
                    AppHelpers.ResourceModelHelper resourceHelper
                        = new AppHelpers.ResourceModelHelper(_dataContext, uri);
                    bHasSaved = await resourceHelper.SetURIResource(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                    AppHelpers.InputModelHelper inputHelper
                        = new AppHelpers.InputModelHelper(_dataContext, uri);
                    bHasSaved = await inputHelper.SetURIInput(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                    AppHelpers.OutputModelHelper outputHelper
                        = new AppHelpers.OutputModelHelper(_dataContext, uri);
                    bHasSaved = await outputHelper.SetURIOutput(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                    AppHelpers.OutcomeModelHelper outcomeHelper
                        = new AppHelpers.OutcomeModelHelper(_dataContext, uri);
                    bHasSaved = await outcomeHelper.SetURIOutcome(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                    AppHelpers.OperationModelHelper operationHelper
                        = new AppHelpers.OperationModelHelper(_dataContext, uri);
                    bHasSaved = await operationHelper.SetURIOperation(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                    AppHelpers.ComponentModelHelper componentHelper
                        = new AppHelpers.ComponentModelHelper(_dataContext, uri);
                    bHasSaved = await componentHelper.SetURIComponent(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                    AppHelpers.BudgetsModelHelper budgetHelper
                        = new AppHelpers.BudgetsModelHelper(_dataContext, uri);
                    bHasSaved = await budgetHelper.SetURIBudgets(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                    AppHelpers.InvestmentsModelHelper investmentHelper
                        = new AppHelpers.InvestmentsModelHelper(_dataContext, uri);
                    bHasSaved = await investmentHelper.SetURIInvestments(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.linkedviews:
                    AppHelpers.LinkedViewModelHelper linkedviewHelper
                        = new AppHelpers.LinkedViewModelHelper(_dataContext, uri);
                    bHasSaved = await linkedviewHelper.SetURILinkedView(uri, saveInFileSystemContent);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.devpacks:
                    AppHelpers.DevPackModelHelper devpackHelper
                        = new AppHelpers.DevPackModelHelper(_dataContext, uri);
                    bHasSaved = await devpackHelper.SetURIDevPack(uri, saveInFileSystemContent);
                    break;
                default:
                    break;
            }
            return bHasSaved;
        }
        public async Task<XmlReader> GetURISecondDocAsync(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            XmlReader reader =  await oContentHelper.GetURISecondDocAsync(docToCalcURI, calcDocURI);
            return reader;
        }
        public async Task<XmlReader> GetURISecondBaseDocAsync(ContentURI uri)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            XmlReader reader = await oContentHelper.GetURISecondBaseDocAsync(uri);
            return reader;
        }
        public async Task<bool> SetAddInNamesAsync(ContentURI calcDocURI)
        {
            bool bHasSet = false;
            //the addin/extension and host typenames are attributes/fields 
            //in a linkedview node/table (the containing element of the tempdoc)
            if (Helpers.AddInHelper.IsAddIn(calcDocURI))
            {
                //use calcDocURI.URIDataManager.HostName && AddinTypeName;
                bHasSet = true;
            }
            else
            {
                //retrieve from the db
                bHasSet = await SetURIAddInNamesAsync(calcDocURI);
            }
            if (string.IsNullOrEmpty(calcDocURI.URIDataManager.AddInName)
                || (calcDocURI.URIDataManager.AddInName.EndsWith(Helpers.GeneralHelpers.NONE)))
            {
                calcDocURI.URIDataManager.AddInName = string.Empty;
            }
            if (string.IsNullOrEmpty(calcDocURI.URIDataManager.HostName)
                || (calcDocURI.URIDataManager.HostName.EndsWith(Helpers.GeneralHelpers.NONE)))
            {
                calcDocURI.URIDataManager.HostName = string.Empty;
            }
            return bHasSet;
        }
        public async Task<bool> SetURIAddInNamesAsync(ContentURI uri)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            bool bHasSet = await oContentHelper.SetURIAddInNames(uri);
            return bHasSet;
        }
        //entity framework updates
        public async Task<bool> UpdateAsync(ContentURI uri, IDictionary<string, string> colUpdates,
            StringDictionary colDeletes)
        {
            bool bIsOkToSave = false;
            //create object for holding most local arguments for node being edited
            //by convention, oArgumentsEdits.URIToEdit = uri
            //oArgumentsEdits.URIToAdd changes with the node being edited
            EditHelper.ArgumentsEdits oArgumentsEdits
                = EditHelper.MakeArgumentEdits(uri, colUpdates);
            oArgumentsEdits.URIToAdd = new ContentURI(uri);
            EditModelHelper oEditModelHelper = new EditModelHelper();
            //process edits first or they could be deleted w/ edit failure
            if (colUpdates.Count > 0)
            {
                List<EditHelper.ArgumentsEdits> edits = new List<EditHelper.ArgumentsEdits>();
                bIsOkToSave = oEditModelHelper.MakeEditsCollection(uri, oArgumentsEdits,
                    edits, colUpdates);
                if (bIsOkToSave)
                {
                    //some edits take place in more than one app
                    bool bHasUpdates = await UpdateSpecialEdits(uri, edits);
                    if (edits.Count > 0)
                    {
                        bIsOkToSave = await UpdateCollectionAsync(uri, edits);
                    }
                }
            }
            if (colDeletes.Count > 0)
            {
                bIsOkToSave = oEditModelHelper.MakeDeletionCollection(uri, oArgumentsEdits,
                    colDeletes);
                if (bIsOkToSave)
                {
                    bIsOkToSave = await DeleteCollectionAsync(uri, oArgumentsEdits);
                }
            }
            return bIsOkToSave;
        }
        private async Task<bool> UpdateSpecialEdits(ContentURI uri, List<EditHelper.ArgumentsEdits> edits)
        {
            EditHelper editHelper = new EditHelper();
            bool bNeedsMoreEdits = true;
            int iIndex = 0;
            bool bHasEdits = false;
            //keep track of the edits already made which must be removed
            List<int> editIdsToRemove = new List<int>();
            foreach (var edit in edits)
            {
                //only one special att per edits collection is possible
                bNeedsMoreEdits = await editHelper.ChangeSpecialAttributeValuesAsync(
                    uri, edit, null);
                if (!bNeedsMoreEdits
                    && edits.Count == 1)
                {
                    //get a new collection when the IsDefault is the only property being changed
                    bool bSaveInFileSystem = false;
                    editIdsToRemove.Add(iIndex);
                    bHasEdits = await GetDevTrekContentAsync(uri, bSaveInFileSystem);
                    break;
                }
                if (!bNeedsMoreEdits)
                {
                    editIdsToRemove.Add(iIndex);
                    bHasEdits = true;
                }
                iIndex += 1;
            }
            if (editIdsToRemove.Count() > 0)
            {
                //the special edits can't be handled correctly by regular update pattern
                //so remove them
                foreach (var index in editIdsToRemove)
                {
                    edits.RemoveAt(index);
                }
                bHasEdits = true;
            }
            return bHasEdits;
        }
        private async Task<bool> DeleteCollectionAsync(ContentURI uri, EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bIsDeleted = false;
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subapptype
                = EditHelpers.EditModelHelper.GetSubAppTypeForEdit(uri);
            uri.ErrorMessage = string.Empty;
            switch (subapptype)
            {
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.clubs:
                    AppHelpers.AccountModelHelper clubHelper
                        = new AppHelpers.AccountModelHelper(_dataContext, uri);
                    //fill in the data transfer object (uri.ResourceClass)
                    bIsDeleted = await clubHelper.DeleteAccount(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.agreements:
                    AppHelpers.ServiceModelHelper agreementHelper
                        = new AppHelpers.ServiceModelHelper(_dataContext, uri);
                    bIsDeleted = await agreementHelper.DeleteService(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.members:
                    AppHelpers.MemberModelHelper memberHelper
                        = new AppHelpers.MemberModelHelper(_dataContext, uri);
                    bIsDeleted = await memberHelper.DeleteMember(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.addins:
                    AppHelpers.AddInsModelHelper addinHelper
                        = new AppHelpers.AddInsModelHelper(_dataContext, uri);
                    bIsDeleted = await addinHelper.DeleteAddIns(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.locals:
                    AppHelpers.LocalsModelHelper localHelper
                        = new AppHelpers.LocalsModelHelper(_dataContext, uri);
                    bIsDeleted = await localHelper.DeleteLocals(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.networks:
                    AppHelpers.NetworkModelHelper networkHelper
                        = new AppHelpers.NetworkModelHelper(_dataContext, uri);
                    bIsDeleted = await networkHelper.DeleteNetwork(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.resources:
                    //resources must be deleted from directories too
                    //get their paths before deleted the db records
                    AppHelpers.Resources resources = new AppHelpers.Resources();
                    await resources.AddResourceURLsToDeleteCollectionAsync(uri, argumentsEdits);
                    AppHelpers.ResourceModelHelper resourceHelper
                        = new AppHelpers.ResourceModelHelper(_dataContext, uri);
                    bIsDeleted = await resourceHelper.DeleteResource(argumentsEdits);
                    if (bIsDeleted)
                    {
                        //delete the file and blob resources
                        Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                            = Helpers.FileStorageIO.GetPlatformType(uri);
                        AppHelpers.Resources.DeleteResource(uri, ePlatform, argumentsEdits);
                    }
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                    AppHelpers.InputModelHelper inputHelper
                        = new AppHelpers.InputModelHelper(_dataContext, uri);
                    bIsDeleted = await inputHelper.DeleteInput(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                    AppHelpers.OutputModelHelper outputHelper
                        = new AppHelpers.OutputModelHelper(_dataContext, uri);
                    bIsDeleted = await outputHelper.DeleteOutput(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                    AppHelpers.OutcomeModelHelper outcomeHelper
                        = new AppHelpers.OutcomeModelHelper(_dataContext, uri);
                    bIsDeleted = await outcomeHelper.DeleteOutcome(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                    AppHelpers.OperationModelHelper operationHelper
                        = new AppHelpers.OperationModelHelper(_dataContext, uri);
                    bIsDeleted = await operationHelper.DeleteOperation(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                    AppHelpers.ComponentModelHelper componentHelper
                        = new AppHelpers.ComponentModelHelper(_dataContext, uri);
                    bIsDeleted = await componentHelper.DeleteComponent(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                    AppHelpers.BudgetsModelHelper budgetHelper
                        = new AppHelpers.BudgetsModelHelper(_dataContext, uri);
                    bIsDeleted = await budgetHelper.DeleteBudgets(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                    AppHelpers.InvestmentsModelHelper investmentHelper
                        = new AppHelpers.InvestmentsModelHelper(_dataContext, uri);
                    bIsDeleted = await investmentHelper.DeleteInvestments(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.linkedviews:
                    AppHelpers.LinkedViewModelHelper linkedviewHelper
                        = new AppHelpers.LinkedViewModelHelper(_dataContext, uri);
                    bIsDeleted = await linkedviewHelper.DeleteLinkedView(argumentsEdits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.devpacks:
                    AppHelpers.DevPackModelHelper devpackHelper
                        = new AppHelpers.DevPackModelHelper(_dataContext, uri);
                    bIsDeleted = await devpackHelper.DeleteDevPack(argumentsEdits);
                    break;
                default:
                    uri.ErrorMessage = string.IsNullOrEmpty(uri.ErrorMessage) ? Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "CONTENTREP_BADAPP") : uri.ErrorMessage;
                    break;
            }
            return bIsDeleted;
        }
        private async Task<bool> UpdateCollectionAsync(ContentURI uri, List<EditHelper.ArgumentsEdits> edits)
        {
            bool bIsUpdated = false;
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subapptype 
                = EditHelpers.EditModelHelper.GetSubAppTypeForEdit(uri);
            uri.ErrorMessage = string.Empty;
            switch (subapptype)
            {
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.clubs:
                    AppHelpers.AccountModelHelper clubHelper
                        = new AppHelpers.AccountModelHelper(_dataContext, uri);
                    //fill in the data transfer object (uri.AccountClass)
                    bIsUpdated = await clubHelper.UpdateAccount(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.agreements:
                    AppHelpers.ServiceModelHelper agreementHelper
                        = new AppHelpers.ServiceModelHelper(_dataContext, uri);
                    bIsUpdated = await agreementHelper.UpdateService(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.members:
                    AppHelpers.MemberModelHelper memberHelper
                        = new AppHelpers.MemberModelHelper(_dataContext, uri);
                    bIsUpdated = await memberHelper.UpdateMember(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.networks:
                    AppHelpers.NetworkModelHelper networkHelper
                        = new AppHelpers.NetworkModelHelper(_dataContext, uri);
                    bIsUpdated = await networkHelper.UpdateNetwork(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.addins:
                    AppHelpers.AddInsModelHelper addinHelper
                        = new AppHelpers.AddInsModelHelper(_dataContext, uri);
                    bIsUpdated = await addinHelper.UpdateAddIns(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.locals:
                    AppHelpers.LocalsModelHelper localHelper
                        = new AppHelpers.LocalsModelHelper(_dataContext, uri);
                    bIsUpdated = await localHelper.UpdateLocals(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.resources:
                    AppHelpers.ResourceModelHelper resourceHelper
                        = new AppHelpers.ResourceModelHelper(_dataContext, uri);
                    bIsUpdated = await resourceHelper.UpdateResource(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                    AppHelpers.InputModelHelper inputHelper
                        = new AppHelpers.InputModelHelper(_dataContext, uri);
                    bIsUpdated = await inputHelper.UpdateInput(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                    AppHelpers.OutputModelHelper outputHelper
                        = new AppHelpers.OutputModelHelper(_dataContext, uri);
                    bIsUpdated = await outputHelper.UpdateOutput(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                    AppHelpers.OutcomeModelHelper outcomeHelper
                        = new AppHelpers.OutcomeModelHelper(_dataContext, uri);
                    bIsUpdated = await outcomeHelper.UpdateOutcome(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                    AppHelpers.OperationModelHelper operationHelper
                        = new AppHelpers.OperationModelHelper(_dataContext, uri);
                    bIsUpdated = await operationHelper.UpdateOperation(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                    AppHelpers.ComponentModelHelper componentHelper
                        = new AppHelpers.ComponentModelHelper(_dataContext, uri);
                    bIsUpdated = await componentHelper.UpdateComponent(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                    AppHelpers.BudgetsModelHelper budgetHelper
                        = new AppHelpers.BudgetsModelHelper(_dataContext, uri);
                    bIsUpdated = await budgetHelper.UpdateBudgets(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                    AppHelpers.InvestmentsModelHelper investmentHelper
                        = new AppHelpers.InvestmentsModelHelper(_dataContext, uri);
                    bIsUpdated = await investmentHelper.UpdateInvestments(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.linkedviews:
                    AppHelpers.LinkedViewModelHelper linkedViewHelper
                        = new AppHelpers.LinkedViewModelHelper(_dataContext, uri);
                    bIsUpdated = await linkedViewHelper.UpdateLinkedView(edits);
                    break;
                case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.devpacks:
                    AppHelpers.DevPackModelHelper devpackelper
                        = new AppHelpers.DevPackModelHelper(_dataContext, uri);
                    bIsUpdated = await devpackelper.UpdateDevPack(edits);
                    break;
                default:
                    uri.ErrorMessage = string.IsNullOrEmpty(uri.ErrorMessage) ? Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "CONTENTREP_BADAPP") : uri.ErrorMessage;
                    break;
            }
            return bIsUpdated;
        }
        //primary xml document edit interfaces
        public async Task<bool> UpdateAsync(bool isDeletes, ContentURI uri,
            IDictionary<string, string> colUpdates,
            StringDictionary colDeletes, XElement devTrekRoot)
        {
            bool bIsOkToSave = false;

            //figure out how to run this async Task.Run???

            //create object for holding most local arguments for node being edited
            //by convention, oArgumentsEdits.URIToEdit = uri
            //oArgumentsEdits.URIToAdd changes with the node being edited
            EditHelper.ArgumentsEdits oArgumentsEdits
                = EditHelper.MakeArgumentEdits(uri, colUpdates);
                oArgumentsEdits.URIToAdd = new ContentURI(uri);
            //EditHelper.SetUpdateSchemaProperties(isDeletes, oArgumentsEdits);
            EditHelper oEditHelper = new EditHelper();
            //stringwriter allows xmlwriter.writeraw() to work properly
            if (isDeletes)
            {
                bIsOkToSave = await oEditHelper.MakeDeletionsAsync(uri, oArgumentsEdits,
                    colDeletes, devTrekRoot);
            }
            else
            {
                await oEditHelper.MakeUpdatesAsync(uri, oArgumentsEdits,
                    colUpdates, devTrekRoot);
            }
            if (string.IsNullOrEmpty(uri.ErrorMessage))
            {
                bIsOkToSave = true;
            }
            //using clauses not used
            if (oArgumentsEdits.EditWriter != null)
                oArgumentsEdits.EditWriter.Dispose();
            return bIsOkToSave;
        }
        public async Task<bool> AddSelectionsAsync(ContentURI uri,
            EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
            string selectedAncestorURIPattern, string numberToAdd)
        {

            bool bIsOkToSave = false;
            EditHelpers.AddHelperLinq oAddHelper = new EditHelpers.AddHelperLinq();

            //step 1. organize the arguments that will be used to add selection
            //by convention, oArgumentsEdits.URIToEdit is the node receving new children
            //oArgumentsEdits.URIToAdd is the node being added
            EditHelper.ArgumentsEdits oArgumentsAdds
                = EditHelpers.AddHelperLinq.MakeArgumentAdds(uri,
                selectionOption, selectedAncestorURIPattern, numberToAdd);

            //step 2. prepare a list of the selections to be inserted
            EditHelpers.AddHelperLinq.SetSelectionsToAdd(uri,
                oArgumentsAdds);

            //step 3. add the selections to the db
            if (uri.ErrorMessage == string.Empty)
            {
                if (oArgumentsAdds.SelectionOption
                    == EditHelpers.AddHelperLinq.SELECTION_OPTIONS.allancestors)
                {
                    int i = 0;
                    foreach (ContentURI selectedURI in oArgumentsAdds.SelectionsToAdd)
                    {
                        //insertions get their parent params from addsArguments.URIToEdit
                        //each selection needs to reset addsArguments.URIToEdit to its parent
                        ContentURI.ChangeURIPattern(oArgumentsAdds.URIToEdit,
                            selectedURI.URIDataManager.ParentURIPattern);
                        if (i == 0)
                            break;
                    }
                }
                //add the selection
                if (oArgumentsAdds.SelectionsToAdd.Count < 1)
                {
                    uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "EDIT_NOURISTOEDIT");
                    return false;
                }
                uri.ErrorMessage = string.Empty;
                switch (uri.URIDataManager.SubAppType)
                {
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.clubs:
                        AppHelpers.AccountModelHelper clubHelper
                            = new AppHelpers.AccountModelHelper(_dataContext, uri);
                        //fill in the data transfer object (uri.ResourceClass)
                        bIsOkToSave = await clubHelper.AddAccount(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.agreements:
                        AppHelpers.ServiceModelHelper agreementHelper
                            = new AppHelpers.ServiceModelHelper(_dataContext, uri);
                        bIsOkToSave = await agreementHelper.AddService(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.members:
                        AppHelpers.MemberModelHelper memberHelper
                            = new AppHelpers.MemberModelHelper(_dataContext, uri);
                        bIsOkToSave = await memberHelper.AddMember(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.networks:
                        AppHelpers.NetworkModelHelper networkHelper
                            = new AppHelpers.NetworkModelHelper(_dataContext, uri);
                        bIsOkToSave = await networkHelper.AddNetwork(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.addins:
                        AppHelpers.AddInsModelHelper addinHelper
                            = new AppHelpers.AddInsModelHelper(_dataContext, uri);
                        bIsOkToSave = await addinHelper.AddAddIns(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.locals:
                        AppHelpers.LocalsModelHelper localHelper
                            = new AppHelpers.LocalsModelHelper(_dataContext, uri);
                        bIsOkToSave = await localHelper.AddLocals(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.resources:
                        AppHelpers.ResourceModelHelper resourceHelper
                            = new AppHelpers.ResourceModelHelper(_dataContext, uri);
                        bIsOkToSave = await resourceHelper.AddResource(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.inputprices:
                        AppHelpers.InputModelHelper inputHelper
                            = new AppHelpers.InputModelHelper(_dataContext, uri);
                        bIsOkToSave = await inputHelper.AddInput(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outputprices:
                        AppHelpers.OutputModelHelper outputHelper
                            = new AppHelpers.OutputModelHelper(_dataContext, uri);
                        bIsOkToSave = await outputHelper.AddOutput(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices:
                        AppHelpers.OutcomeModelHelper outcomeHelper
                            = new AppHelpers.OutcomeModelHelper(_dataContext, uri);
                        bIsOkToSave = await outcomeHelper.AddOutcome(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices:
                        AppHelpers.OperationModelHelper operationHelper
                            = new AppHelpers.OperationModelHelper(_dataContext, uri);
                        bIsOkToSave = await operationHelper.AddOperation(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices:
                        AppHelpers.ComponentModelHelper componentHelper
                            = new AppHelpers.ComponentModelHelper(_dataContext, uri);
                        bIsOkToSave = await componentHelper.AddComponent(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets:
                        AppHelpers.BudgetsModelHelper budgetHelper
                            = new AppHelpers.BudgetsModelHelper(_dataContext, uri);
                        bIsOkToSave = await budgetHelper.AddBudgets(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments:
                        AppHelpers.InvestmentsModelHelper investmentHelper
                            = new AppHelpers.InvestmentsModelHelper(_dataContext, uri);
                        bIsOkToSave = await investmentHelper.AddInvestments(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.linkedviews:
                        AppHelpers.LinkedViewModelHelper linkedViewHelper
                            = new AppHelpers.LinkedViewModelHelper(_dataContext, uri);
                        bIsOkToSave = await linkedViewHelper.AddLinkedView(oArgumentsAdds);
                        break;
                    case Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.devpacks:
                        AppHelpers.DevPackModelHelper devpackHelper
                            = new AppHelpers.DevPackModelHelper(_dataContext, uri);
                        bIsOkToSave = await devpackHelper.AddDevPack(oArgumentsAdds);
                        break;
                    default:
                        uri.ErrorMessage = string.IsNullOrEmpty(uri.ErrorMessage) ? Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "CONTENTREP_BADAPP") : uri.ErrorMessage;
                        break;
                }
            }
            return bIsOkToSave;
        }
        public async Task<bool> AddSelectionsAsync(ContentURI uri,
            EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
            string selectedAncestorURIPattern,
            XElement root, string numberToAdd)
        {

            bool bIsOkToSave = false;
            bool bStepIsOK = false;
            EditHelpers.AddHelperLinq oAddHelper = new EditHelpers.AddHelperLinq();

            //figure out how to run this async Task.Run???

            //step 1. organize the arguments that will be used to add selection
            //by convention, oArgumentsEdits.URIToEdit is the node receving new children
            //oArgumentsEdits.URIToAdd is the node being added
            EditHelper.ArgumentsEdits oArgumentsAdds
                = EditHelpers.AddHelperLinq.MakeArgumentAdds(uri,
                selectionOption, selectedAncestorURIPattern, numberToAdd);

            //step 2. prepare a list of the selections to be inserted
            EditHelpers.AddHelperLinq.SetSelectionsToAdd(uri,
                oArgumentsAdds);

            //step 3. add the ancestors needed by each selection
            bStepIsOK = await oAddHelper.AddAncestorsToSelectionsAsync(uri, oArgumentsAdds,
                root);

            //step 4. add the ancestors to the linq xelement
            if (uri.ErrorMessage == string.Empty)
            {
                oAddHelper.AddAncestorsToLinqDoc(uri, oArgumentsAdds,
                    root);
            }

            //step 5. add the selections to linq doc and db
            if (uri.ErrorMessage == string.Empty)
            {
                //insert the selections into the root element doc only
                oAddHelper.AddNewSelectionsToLinqDoc(uri, oArgumentsAdds,
                    root);
            }
            if (uri.ErrorMessage == string.Empty)
            {
                //retain the original uripattern if it existed, 
                //otherwise switch to a uripattern set by an ancestor 
                //(to get the right stylesheet)
                EditHelpers.AddHelperLinq.SetTempURIForDisplay(uri,
                    oArgumentsAdds, root);
                bIsOkToSave = true;
            }
            return bIsOkToSave;
        }
        public async Task<string> AddLinkedViewAsync(ContentURI uri,
            IDictionary<string, string> newLinkedView,
            bool isSelections, string insertedIdsArray)
        {
            DevTreks.Data.AppHelpers.LinkedViews oLinkedView = new AppHelpers.LinkedViews();
            string sInsertedIdsArray = await oLinkedView.AddLinkedViewAsync(
                uri, newLinkedView, isSelections, insertedIdsArray);
            return sInsertedIdsArray;
        }
        public async Task<bool> AddCategoryAsync(ContentURI uri, int serviceGroupId,
            int networkId, int numberToAdd)
        {
            bool bIsOkToSave = false;
            DevTreks.Data.AppHelpers.Agreement oAgreement = new AppHelpers.Agreement();
            bIsOkToSave = await oAgreement.AddNetworkCategoriesAsync(
                uri, serviceGroupId, networkId, numberToAdd);
            return bIsOkToSave;
        }
        public async Task<bool> SaveURISecondDocAsync(ContentURI uri, XmlReader reader)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            //join table updates
            bool bIsSaved = await oContentHelper.SaveURISecondDocAsync(uri,
                reader);
            if (bIsSaved == false)
            {
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "SQLCONTENT_NOSAVE");
            }
            return bIsSaved;
        }
        public async Task<bool> SaveURISecondBaseDocAsync(ContentURI uri, bool isMetaData,
            string fileName, XmlReader reader)
        {
            Helpers.ContentHelper oContentHelper = new Helpers.ContentHelper();
            //base table updates
            bool bIsSaved = await oContentHelper.SaveURISecondBaseDocAsync(uri,
                isMetaData, fileName, reader);
            if (bIsSaved == false)
            {
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "SQLCONTENT_NOSAVE");
            }
            return bIsSaved;
        }
        public async Task<bool> SaveURIResourceFileAsync(ContentURI uri, string fileName, int fileLength,
            string mimeType, Stream postedFileStream)
        {
            AppHelpers.Resources oResource = new AppHelpers.Resources();
            bool bIsSaved = await oResource.SaveURIResourceFileAsync(uri,
                fileName, fileLength, mimeType, postedFileStream);
            if (bIsSaved == false)
            {
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "SQLCONTENT_NOSAVE2");
            }
            return bIsSaved;
        }
        public async Task<bool> UpdateURIResourceFileUploadMsgAsync(ContentURI fileUploadURI, string message)
        {
            AppHelpers.Resources oResource = new AppHelpers.Resources();
            bool bIsUpdated = await oResource.UpdateURIResourceFileUploadMsgAsync(
                fileUploadURI, message);
            return bIsUpdated;
        }
        private List<ContentURI> FillLinkedViewList(ContentURI uri,
            SqlDataReader linkedViewsReader)
        {
            List<ContentURI> colLinkedView = new List<ContentURI>();
            if (linkedViewsReader != null && (!linkedViewsReader.IsClosed))
            {
                if (linkedViewsReader.HasRows)
                {
                    //build a contenturi list to return to the client
                    string sName = string.Empty;
                    using (linkedViewsReader)
                    {
                        while (linkedViewsReader.Read())
                        {
                            //don't include newly added default nodes in search results 
                            //inputs the exception because many ferts start with 00
                            sName = linkedViewsReader.GetString(1);
                            ContentURI oLinkedViewURI = ContentURI.GetContentURI
                            (
                                uri
                                //id
                                ,linkedViewsReader.GetInt32(0)
                                //label
                                , string.Empty
                                //name
                                , sName
                                //description
                                , linkedViewsReader.GetString(2)
                                //file extension type (used by analyzers to find base calculations)
                                , linkedViewsReader.GetString(3)
                                //connection (networkid handles)
                                , string.Empty
                                //media resource uri to display
                                , linkedViewsReader.GetString(11)
                                //media resource alt to display
                                , linkedViewsReader.GetString(12)
                                //parentURIPattern
                                , linkedViewsReader.GetString(8)
                                //docStatus
                                , (Helpers.GeneralHelpers.DOCS_STATUS)linkedViewsReader.GetInt16(9)
                                //networkshortname
                                , linkedViewsReader.GetString(10)
                                //nodename
                                , linkedViewsReader.GetString(13)
                                , uri.URIDataManager.DefaultRootWebStoragePath
                            );
                            oLinkedViewURI.URIDataManager.AddInName = linkedViewsReader.GetString(4);
                            oLinkedViewURI.URIDataManager.HostName = linkedViewsReader.GetString(5);
                            oLinkedViewURI.URIDataManager.BaseId = linkedViewsReader.GetInt32(6);
                            oLinkedViewURI.URIDataManager.IsDefault = linkedViewsReader.GetBoolean(7);
                            Helpers.ContentHelper.SetLinkedViewPropsFromParent(oLinkedViewURI, uri);
                            //need to adjust networkid because only qry only returns networkpartname
                            if (uri.URINetwork != null && oLinkedViewURI.URINetwork != null)
                            {
                                oLinkedViewURI.URINetwork.PKId = uri.URINetwork.PKId;
                            }
                            colLinkedView.Add(oLinkedViewURI);
                        }
                    }
                }
            }
            return colLinkedView;
        }
        private List<ContentURI> FillOtherViewsList(ContentURI uri,
            SqlDataReader linkedViewsReader)
        {
            List<ContentURI> colLinkedView = new List<ContentURI>();
            if (linkedViewsReader != null && (!linkedViewsReader.IsClosed))
            {
                if (linkedViewsReader.HasRows)
                {
                    //build a contenturi list to return to the client
                    using (linkedViewsReader)
                    {
                        int i = 0;
                        string sName = string.Empty;
                        while (linkedViewsReader.Read())
                        {
                            //don't include newly added default nodes in search results 
                            sName = linkedViewsReader.GetString(2);
                            ContentURI oLinkedViewURI = ContentURI.GetContentURI
                            (
                                uri
                                //id
                                ,linkedViewsReader.GetInt32(0)
                                //label
                                , linkedViewsReader.GetString(1)
                                //name
                                , sName
                                //description
                                , linkedViewsReader.GetString(3)
                                //fileextensiontype
                                , linkedViewsReader.GetString(4)
                                //connection, (networkid handles)
                                , string.Empty
                                //media resource uri to display
                                , linkedViewsReader.GetString(8)
                                //media resource alt to display
                                , linkedViewsReader.GetString(9)
                                //parentURIPattern
                                , linkedViewsReader.GetString(6)
                                //docStatus
                                , (Helpers.GeneralHelpers.DOCS_STATUS)linkedViewsReader.GetInt16(7)
                                //networkpartname
                                , uri.URINetworkPartName
                                , linkedViewsReader.GetString(5)
                                , uri.URIDataManager.DefaultRootWebStoragePath
                            );
                            //need to adjust networkid because only qry only returns networkpartname
                            if (uri.URINetwork != null && oLinkedViewURI.URINetwork != null)
                            {
                                oLinkedViewURI.URINetwork.PKId = uri.URINetwork.PKId;
                            }
                            colLinkedView.Add(oLinkedViewURI);
                            i++;
                        }
                    }
                }
            }
            return colLinkedView;
        }
        public List<ContentURI> FillChildrenViewsList(
            ContentURI uri, SqlDataReader linkedViewsReader, bool needsDefaultRecords)
        {
            List<ContentURI> colLinkedView = new List<ContentURI>();
            if (linkedViewsReader != null && (!linkedViewsReader.IsClosed))
            {
                if (linkedViewsReader.HasRows)
                {
                    string sDefaultPrefix = "00";
                    if (needsDefaultRecords)
                    {
                        //not allowed in names by DevTreks.RuleHelper
                        sDefaultPrefix = Helpers.GeneralHelpers.FILENAME_DELIMITER;
                    }
                    //build a contenturi list to return to the client
                    using (linkedViewsReader)
                    {
                        bool bHasDefault = false;
                        string sName = string.Empty;
                        while (linkedViewsReader.Read())
                        {
                            //don't include newly added default nodes in search results 
                            sName = linkedViewsReader.GetString(1);
                            //the rowcount and pagination have to be adjusted as well
                            if (!sName.StartsWith(sDefaultPrefix))
                            {
                                ContentURI oLinkedViewURI = ContentURI.GetContentURI
                                (
                                    uri
                                    //id
                                    ,linkedViewsReader.GetInt32(0)
                                    //label
                                    , linkedViewsReader.GetString(1)
                                    //name
                                    , sName
                                    //description
                                    , linkedViewsReader.GetString(2)
                                    //fileextensiontype
                                    , linkedViewsReader.GetString(3)
                                    //connection, (networkid handles)
                                    , string.Empty
                                    //media resource uri to display
                                    , string.Empty
                                    //media resource alt to display
                                    , string.Empty
                                    //parentURIPattern
                                    , linkedViewsReader.GetString(7)
                                    //docStatus
                                    , (Helpers.GeneralHelpers.DOCS_STATUS)linkedViewsReader.GetInt16(8)
                                    //networkpartname
                                    , linkedViewsReader.GetString(9)
                                    , linkedViewsReader.GetString(10)
                                    , uri.URIDataManager.DefaultRootWebStoragePath
                                );
                                oLinkedViewURI.URIDataManager.AddInName = linkedViewsReader.GetString(4);
                                oLinkedViewURI.URIDataManager.HostName = linkedViewsReader.GetString(5);
                                oLinkedViewURI.URIDataManager.BaseId = linkedViewsReader.GetInt32(6);
                                if (!bHasDefault)
                                {
                                    //custom docs have arbitrary defaults
                                    oLinkedViewURI.URIDataManager.IsDefault = true;
                                    bHasDefault = true;
                                }
                                Helpers.ContentHelper.SetLinkedViewPropsFromParent(oLinkedViewURI, uri);
                                //need to adjust networkid because only qry only returns networkpartname
                                if (uri.URINetwork != null && oLinkedViewURI.URINetwork != null)
                                {
                                    oLinkedViewURI.URINetwork.PKId = uri.URINetwork.PKId;
                                }
                                colLinkedView.Add(oLinkedViewURI);
                            }
                            else
                            {
                                uri.URIDataManager.RowCount -= 1;
                            }
                        }
                    }
                }
            }
            return colLinkedView;
        }
        public async Task<List<ContentURI>> GetResourceAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            //end users may not be able to resolve errors generated by resource retrieval
            //trap the common ones here
            if (uri.URIId == 0)
            {
                //an uploaded file that is bad (not formatted as xml), or a bad filename, could be the cause
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "SQLCONTENT_NORESOURCES");
                return new List<ContentURI>();
            }
            AppHelpers.Resources oResourceHelper = new AppHelpers.Resources();
            //the results returned from resources qry are ok for web or cloud
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlDataReader resources = await oResourceHelper.GetResourceAsync(sqlIO, uri,
                needsOneRecord, needsFullPath, getResourceByType, getResourceByParam);
            //fill an List with the resources (distinguishes cloud from web)
            List<ContentURI> colURIs = await oResourceHelper.FillResourceListAsync(uri, resources);
            sqlIO.Dispose();
            return colURIs;
        }
        public string GetResourceURLs(ContentURI uri,
           bool needsOneRecord, bool needsFullPath,
           AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
           string getResourceByParam)
        {
            //return a delimited string containing enough information to display, or load, the resource
            string sResourceURLs = string.Empty;
            if (uri.URIId == 0)
            {
                //an uploaded file that is bad (not formatted as xml), or a bad filename, could be the cause
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "SQLCONTENT_NORESOURCES");
                return sResourceURLs;
            }
            AppHelpers.Resources oResourceHelper = new AppHelpers.Resources();
            Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = Helpers.FileStorageIO.GetPlatformType(uri);
            if (getResourceByType
                == AppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcepackid)
            {
                if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
                {
                    //this saves it in a blob
                    sResourceURLs = oResourceHelper.GetAndSaveResourceURLInCloudByResourcePackId(
                        uri, getResourceByParam);
                }
                else
                {
                    //this doesn't save anything it just makes a string
                    sResourceURLs = oResourceHelper.GetAndSaveResourceURLInFilesByResourcePackId(
                        uri, getResourceByParam);
                }
            }
            else if (getResourceByType
                == AppHelpers.Resources.RESOURCES_GETBY_TYPES.tempdocs)
            {
                //tempdocs deprecated
                //this appears to be cloud compatible
                //sResourceURLs = oResourceHelper.GetResourceURLsForTempDocs(uri);
            }
            else
            {
                //works for cloud or web
                sResourceURLs = oResourceHelper.GetResourceURLs(uri,
                    needsOneRecord, needsFullPath, getResourceByType, getResourceByParam);
            }
            return sResourceURLs;
        }
        public async Task<string> GetResourceURLsAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam)
        {
            //return a delimited string containing enough information to display, or load, the resource
            string sResourceURLs = string.Empty;
            if (uri.URIId == 0)
            {
                //an uploaded file that is bad (not formatted as xml), or a bad filename, could be the cause
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "SQLCONTENT_NORESOURCES");
                return sResourceURLs;
            }
            AppHelpers.Resources oResourceHelper = new AppHelpers.Resources();
            Helpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = Helpers.FileStorageIO.GetPlatformType(uri);
            if (getResourceByType
                == AppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcepackid)
            {
                if (ePlatform == Helpers.FileStorageIO.PLATFORM_TYPES.azure)
                {
                    //this saves it in a blob
                    sResourceURLs = oResourceHelper.GetAndSaveResourceURLInCloudByResourcePackId(uri,
                        getResourceByParam);
                }
                else
                {
                    //this doesn't save anything it just makes a string
                    sResourceURLs = await oResourceHelper.GetAndSaveResourceURLInFilesByResourcePackIdAsync(
                        uri, getResourceByParam);
                }
            }
            else if (getResourceByType
                == AppHelpers.Resources.RESOURCES_GETBY_TYPES.tempdocs)
            {
                //tempdocs deprecated
                ////this appears to be cloud compatible
                //sResourceURLs = await oResourceHelper.GetResourceURLsForTempDocs(uri);
            }
            else
            {
                //works for cloud or web
                sResourceURLs = await oResourceHelper.GetResourceURLsAsync(uri,
                    needsOneRecord, needsFullPath, getResourceByType, getResourceByParam);
            }
            return sResourceURLs;
        }
        public bool SetPathAndStoreResource(ContentURI resourceuri)
        {
            AppHelpers.Resources oResourceHelper = new AppHelpers.Resources();
            bool bNeedsFullPath = false;
            //gets the file system path and only stores resource if it doesn't exist
            oResourceHelper.GetPathAndStoreResource(
                resourceuri, resourceuri.URIDataManager.FileSystemPath, bNeedsFullPath);
            bool bIsSet = true;
            return bIsSet;
        }
        public async Task<bool> SetPathAndStoreResourceAsync(ContentURI resourceuri)
        {
            AppHelpers.Resources oResourceHelper = new AppHelpers.Resources();
            bool bNeedsFullPath = false;
            //gets the file system path and only stores resource if it doesn't exist
            await oResourceHelper.GetPathAndStoreResourceAsync(
                resourceuri, resourceuri.URIDataManager.FileSystemPath, bNeedsFullPath);
            bool bIsSet = true;
            return bIsSet;
        }
        public async Task<int> InsertNewAuditItemAsync(ContentURI uri,
            string serverSubAction, int memberId,
            int clubInUseId, string editedDocURIPattern, string editedDocFullPath,
            string clubInUseAuthorizationLevel, string memberRole,
            DateTime dtToday, int ownerClubId)
        {
            int iAuditItemId = 0;
            AppHelpers.Accounts oAccountHelper = new AppHelpers.Accounts();
            var newAuditItem = new AccountToAudit
            {
                AccountId = ownerClubId,
                ClubInUseAuthorizationLevel = clubInUseAuthorizationLevel,
                ClubInUseId = clubInUseId,
                EditDate = dtToday,
                EditedDocFullPath = editedDocFullPath,
                EditedDocURI = editedDocURIPattern,
                MemberId = memberId,
                MemberRole = memberRole,
                ServerSubAction = serverSubAction
            };
            iAuditItemId = await oAccountHelper.InsertNewAuditItemAsync(uri, newAuditItem);
            return iAuditItemId;
        }


       //162 moved these here to avoid having to create new repositories
        public async Task GetStylesheetParametersForAddInAsync(ContentURI docToCalcURI,
           IDictionary<string, string> styleParams)
        {
            ContentURI calcDocURI =
                Helpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI != null)
            {
                await GetStandardStylesheetParametersForAddInsAsync(docToCalcURI, calcDocURI,
                       styleParams);
                //add standard stylesheet params needed by each host
                string sAddInTypeName = string.Empty;
                string sHostTypeName = string.Empty;
                bool bHasSet = await SetAddInNamesAsync(calcDocURI);
                if (calcDocURI.URIDataManager.HostName.ToLower().Equals(Helpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString())
                    || calcDocURI.URIDataManager.HostName.ToLower().Equals(Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString()))
                {
                    //this had been in host before moving this to data access layer
                    Helpers.AddInStylesheetHelper.GetStylesheetParametersForHost(docToCalcURI, ref styleParams);
                }
            }
            else
            {
                //2.0.0: stories need linked media resources added 
                calcDocURI = Helpers.LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
                if (calcDocURI != null)
                {
                    if (docToCalcURI.URIDataManager.Resource == null)
                    {
                        docToCalcURI.URIDataManager.Resource = new List<ContentURI>();
                    }
                    docToCalcURI.URIDataManager.Resource.Add(calcDocURI);
                    bool bNeedsFullPath = false;
                    bool bNeedsOneRecord = false;
                    //2.0.2 switched to calcdocuri from doctocalcuri because when linked to 
                    //content can't retrieve resources using doctocalcuri
                    string sResourceURLs = await GetResourceURLsAsync(calcDocURI, bNeedsOneRecord, bNeedsFullPath,
                        AppHelpers.Resources.RESOURCES_GETBY_TYPES.storyuri, string.Empty);
                    styleParams.Add("linkedListsArray", sResourceURLs);
                }
            }
        }

        public async Task GetStandardStylesheetParametersForAddInsAsync(
            ContentURI docToCalcURI, ContentURI calcDocURI,
            IDictionary<string, string> styleParams)
        {
            string sDefaultDevDocId = calcDocURI.URIId.ToString();
            ContentURI defaultDevDocURI = Helpers.LinqHelpers.GetLinkedViewIsDefaultAddIn(
                docToCalcURI.URIDataManager.LinkedView);
            if (defaultDevDocURI != null)
            {
                sDefaultDevDocId = ContentURI.GetURIPatternPart(
                    defaultDevDocURI.URIPattern, ContentURI.URIPATTERNPART.id);
            }
            styleParams.Add("defaultLinkedViewId", sDefaultDevDocId);
            styleParams.Add("saveMethod", docToCalcURI.URIDataManager.TempDocSaveMethod);
            styleParams.Add("linkedListsArray",
            await GetLinkedListsArrayAsync(docToCalcURI, calcDocURI));
        }
        public async Task<string> GetLinkedListsArrayAsync(ContentURI docToCalcURI,
           ContentURI calcDocURI)
        {
            string sLinkedListsArray = string.Empty;
            //see if it is stored in form elements
            sLinkedListsArray = calcDocURI.URIDataManager.LinkedLists;
            bool bNeedsLists = false;
            if (Helpers.AddInHelper.HOSTS.extensioncalculatorsteps.ToString().Equals(
                calcDocURI.URIDataManager.HostName.ToLower())
                || Helpers.AddInHelper.HOSTS.extensionanalyzersteps.ToString().Equals(
                calcDocURI.URIDataManager.HostName.ToLower()))
            {
                //this had been in host before moving this to data access layer
                bNeedsLists = Helpers.AddInStylesheetHelper.NeedsLinkedLists(docToCalcURI, calcDocURI);
            }
            if (bNeedsLists)
            {
                //see if it is stored in form elements
                sLinkedListsArray = calcDocURI.URIDataManager.LinkedLists;
                if (string.IsNullOrEmpty(sLinkedListsArray))
                {
                    //retrieve from db (storing in hidden form element can lead to confusion)
                    sLinkedListsArray
                        = await GetResourceURIArraysForLinkedListsAsync(calcDocURI);
                    //the linkedlists are associated with calcdocuri
                    calcDocURI.URIDataManager.LinkedLists = sLinkedListsArray;
                }
            }
            return sLinkedListsArray;
        }

        public async Task<string> GetResourceURIArraysForLinkedListsAsync(
            ContentURI calcDocURI)
        {
            int iCalcDocId = calcDocURI.URIId;
            if (calcDocURI.URIDataManager.BaseId != 0)
            {
                iCalcDocId = calcDocURI.URIDataManager.BaseId;
            }
            string sBaseCalcDocURIPattern = ContentURI.ChangeURIPatternPart(calcDocURI.URIPattern,
                ContentURI.URIPATTERNPART.id, iCalcDocId.ToString());
            string sResourceURIArrays = string.Empty;
            bool bNeedsOneRecord = false;
            sResourceURIArrays = await GetResourceUrlsAsync(calcDocURI, sBaseCalcDocURIPattern,
                    AppHelpers.Resources.GENERAL_RESOURCE_TYPES.linkedlists.ToString(),
                    bNeedsOneRecord, Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString());
            return sResourceURIArrays;
        }

        public async Task<string> GetResourceUrlsAsync(
            ContentURI calcDocURI, string devTrekURIPattern, string resourceType,
            bool needsOneRecord, string serverSubActionType)
        {
            //note: this is replicated in Web.Stylesheethelper
            //returns: relwebpath1;altdesc1;uripattern@relwebpath2;altdesc2;uripattern
            string sResourceUrls = string.Empty;
            ContentURI uri = ContentURI.ConvertShortURIPattern(devTrekURIPattern, calcDocURI);
            //packages use same relpath to resources
            uri.URIDataManager.ServerSubActionType
                = (serverSubActionType != string.Empty)
                ? (Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES)Enum.Parse(typeof(Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES),
                serverSubActionType) : Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.none;
            //get full paths
            bool bNeedsFullPath = true;
            sResourceUrls = await GetResourceURLsAsync(uri,
                needsOneRecord, bNeedsFullPath,
                AppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcetype,
                resourceType);
            return sResourceUrls;
        }

        public async Task SetLinkedViewStateAsync(ContentURI docToCalcURI,
            ContentURI baseDocToCalcURI)
        {
            //set calcdocURI (i.e. stats analysis) stylesheet state
            await SetStylesheetStateAsync(docToCalcURI, Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc);
            //set thirddoc (addin result)
            if (baseDocToCalcURI.URIFileExtensionType
                != Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //v1.2.0
                if (baseDocToCalcURI.URIDataManager.NeedsSummaryView == true
                    || baseDocToCalcURI.URIDataManager.NeedsFullView == true)
                {
                    await SetStylesheetStateAsync(docToCalcURI,
                        Helpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc);
                }
            }
            else
            {
                //tempdocs will always be the same as doctocalc ss
                if (Helpers.LinqHelpers.HasMainStylesheet(
                    baseDocToCalcURI.URIDataManager.Resource)
                    && (!Helpers.LinqHelpers.HasMainStylesheet(
                    docToCalcURI.URIDataManager.Resource)))
                {
                    Helpers.LinqHelpers.AddResource2ToResource1(
                        docToCalcURI, baseDocToCalcURI);
                }
                else
                {
                    await SetStylesheetStateAsync(docToCalcURI,
                        Helpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc);
                }
            }
        }
        public async Task SetStylesheetStateAsync(ContentURI docToCalcURI,
            Helpers.GeneralHelpers.DOC_STATE_NUMBER docState)
        {
            bool bHasStylesheet = false;
            //add a stylesheetURI to docstateuri.URIDataManager.Resource[]
            //where docstateuri is doctocalcuri for firstdoc
            //calcdocuri for seconddoc and selectedviewuri for thirddoc
            if (docState
                == Helpers.GeneralHelpers.DOC_STATE_NUMBER.firstdoc)
            {
                //these are used for packaging, not for displaying
                await SetDocToCalcStylesheetURIAsync(docToCalcURI);
            }
            else if (docState
                == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
            {
                //try finding a stylesheet name in calcdocuri xmldoc
                //if not found, use standard getresourcebylabel pattern
                bHasStylesheet
                    = await SetSelectedAddInStylesheetURIAsync(docToCalcURI, docState);
            }
            else if (docState
                == Helpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
            {
                //try finding a stylesheet name in calcdocuri xmldoc 
                //(using stylesheet2name attribute)
                //if not found, use standard bylabel patterns
                bHasStylesheet
                    = await SetSelectedViewStylesheetURIAsync(docToCalcURI, docState);
                if (!bHasStylesheet)
                {
                    //treat the thirddoc like a first doc (i.e. static stylesheets)
                    //doctocalcuri already holds a static stylesheet resource
                }
            }
        }
        public async Task SetTempDocStylesheetURIAsync(ContentURI uri)
        {
            if (uri.URIFileExtensionType
                == Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //started with a blank doc so stylesheet has not been set yet
                await SetStylesheetStateAsync(uri, Helpers.GeneralHelpers.DOC_STATE_NUMBER.firstdoc);
            }
        }
        public async Task SetDocToCalcStylesheetURIAsync(ContentURI docToCalcURI)
        {
            //refactored: 0.8.5 only builds these during downloadfile actions
            //they are only saved for packaging (not for display
            if (docToCalcURI.URIDataManager.ServerSubActionType
                == Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile)
            {
                ContentURI stylesheetURI = new ContentURI(docToCalcURI);
                string sRelativeFilePath = string.Empty;
                //potential refactor: store stylesheets in db like second and third displaydoctypes
                //for now use "en" instead of uri.URIDataManager.UserLanguage
                Helpers.AddInStylesheetHelper.GetStaticStyleSheetPath(docToCalcURI,
                    string.Concat("en", Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER),
                    out sRelativeFilePath);
                string sStylesheetPath = Helpers.AddInStylesheetHelper.StylesheetFullPath(docToCalcURI, sRelativeFilePath);
                Helpers.AddInStylesheetHelper.GetNetworkStylesheet(docToCalcURI, ref sStylesheetPath);
                stylesheetURI.URIDataManager.FileSystemPath
                    = sStylesheetPath;
                stylesheetURI.URIDataManager.ExtensionObjectNamespace
                    = docToCalcURI.URIDataManager.ExtensionObjectNamespace;
                await AddStylesheetURIToResourceAsync(docToCalcURI, stylesheetURI);
            }
        }
        public async Task<bool> SetSelectedAddInStylesheetURIAsync(ContentURI docToCalcURI, 
            Helpers.GeneralHelpers.DOC_STATE_NUMBER docState)
        {
            bool bHasStylesheet = false;
            ContentURI calcDocURI =
                Helpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (calcDocURI != null)
            {
                bool bNeedsStylesheet = true;
                bHasStylesheet = await SetStylesheetURIByNameAsync(
                    bNeedsStylesheet, docToCalcURI, calcDocURI, docState);
                if ((!bHasStylesheet)
                    && docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                {
                    bHasStylesheet = await SetStylesheetURIByLabelAsync(calcDocURI);
                }
            }
            return bHasStylesheet;
        }

        public async Task<bool> SetSelectedViewStylesheetURIAsync( 
            ContentURI selectedLinkedViewURI, Helpers.GeneralHelpers.DOC_STATE_NUMBER docState)
        {
            bool bHasStylesheet = false;
            //first try using a calcdocuri's second stylesheetname attribute
            bHasStylesheet = await SetSelectedAddInStylesheetURIAsync(selectedLinkedViewURI, docState);
            if (!bHasStylesheet)
            {
                //this is strictly a check to see if the last method could save the ss properly
                bHasStylesheet = Helpers.LinqHelpers.HasMainStylesheet(
                    selectedLinkedViewURI.URIDataManager.Resource);
                if (!bHasStylesheet)
                {
                    //last try using the label
                    bHasStylesheet = await SetStylesheetURIByLabelAsync(selectedLinkedViewURI);
                }
            }
            return bHasStylesheet;
        }
        private async Task AddStylesheetURIToResourceAsync(ContentURI displayURI, ContentURI stylesheetURI)
        {
            if (stylesheetURI == null)
            {
                displayURI.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "STYLEHELPER_NOSTYLE");
            }
            else if (!Helpers.FileStorageIO.URIAbsoluteExists(stylesheetURI,
                stylesheetURI.URIDataManager.FileSystemPath))
            {
                displayURI.ErrorMessage
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "STYLEHELPER_NOSTYLE");
            }
            else
            {
                string sStylesheetURIPattern = stylesheetURI.URIPattern;
                string sStylesheetPath = stylesheetURI.URIDataManager.FileSystemPath;
                string sAltDesc = stylesheetURI.URIDataManager.Description;
                string sStylesheetExtensionObjectNameSpace
                    = stylesheetURI.URIDataManager.ExtensionObjectNamespace;
                sStylesheetURIPattern = await displayURI.URIDataManager.AddResourceToResourceListAsync(
                    displayURI, sStylesheetURIPattern, sAltDesc, sStylesheetPath,
                    sStylesheetExtensionObjectNameSpace);
                stylesheetURI.ChangeURIPattern(sStylesheetURIPattern);
            }
        }

        private async Task<bool> SetStylesheetURIByLabelAsync(ContentURI docToDisplayURI)
        {
            bool bHasStylesheet = false;
            //necessary when the linked view can't store
            //the name of the stylesheet it needs in its internal xml (i.e. custom docs,
            //explanatory linkedview stories).
            docToDisplayURI.URIDataManager.Label
                = AppHelpers.Resources.GENERAL_RESOURCE_TYPES.stylesheet1.ToString();
            string sResourceURLs = await GetResourceURLsByLabelAsync(docToDisplayURI);
            if (string.IsNullOrEmpty(sResourceURLs))
            {
                if (docToDisplayURI.URIDataManager.AppType == GenHelpers.APPLICATION_TYPES.devpacks)
                {
                    //try using the ss2 label (v170)
                    docToDisplayURI.URIDataManager.Label
                        = AppHelpers.Resources.GENERAL_RESOURCE_TYPES.stylesheet2.ToString();
                    sResourceURLs = await GetResourceURLsByLabelAsync(docToDisplayURI);
                }
            }
            if (sResourceURLs != string.Empty)
            {
                ContentURI stylesheetURI = new ContentURI();
                Helpers.AddInStylesheetHelper.SetStylesheetURIParams(docToDisplayURI, ref stylesheetURI,
                    sResourceURLs, docToDisplayURI.URIDataManager.Label);
                string sStylesheetURIPattern = string.Empty;
                sStylesheetURIPattern = await docToDisplayURI.URIDataManager.AddResourceToResourceListAsync(
                    docToDisplayURI, sStylesheetURIPattern,
                    stylesheetURI.URIDataManager.Description,
                    stylesheetURI.URIDataManager.FileSystemPath,
                    stylesheetURI.URIDataManager.ExtensionObjectNamespace);
                bHasStylesheet = true;
            }
            return bHasStylesheet;
        }
        private async Task<string> GetResourceURLsByLabelAsync(ContentURI docToDisplayURI)
        {
            string sResourceURLs = string.Empty;
            bool bNeedsOneResource = true;
            bool bNeedsFullPath = true;
            //this should be one db hit, except for initial retrieval
            //if the urls are not found in filesystem, they are retrieved from db and stored
            sResourceURLs = await GetResourceURLsAsync(docToDisplayURI,
               bNeedsOneResource, bNeedsFullPath,
               AppHelpers.Resources.RESOURCES_GETBY_TYPES.resourcetype,
               docToDisplayURI.URIDataManager.Label);
            return sResourceURLs;
        }
        public async Task<bool> SetStylesheetURIByNameAsync(bool needsStylesheet,
            ContentURI docToCalcURI, ContentURI calcDocURI,
            Helpers.GeneralHelpers.DOC_STATE_NUMBER docState)
        {
            bool bHasStylesheet = false;
            //these linked views store stylesheet filenames as attributes in the xml
            if (Helpers.FileStorageIO.URIAbsoluteExists(calcDocURI,
                calcDocURI.URIDataManager.TempDocPath))
            {
                //need an editable xmldocument
                XElement rootCalcDoc = await Helpers.FileStorageIO.LoadXmlElementAsync(
                    calcDocURI, calcDocURI.URIDataManager.TempDocPath);
                XElement calcEl = EditHelpers.XmlLinq.GetDescendantUsingURIPattern(
                    rootCalcDoc, calcDocURI.URIPattern);
                if (calcEl == null)
                {
                    //may not have saved a calculation yet (id = 1)
                    //get the first one
                    string sOneURIPattern
                        = calcDocURI.URIPattern.Replace(
                        calcDocURI.URIId.ToString(), "1");
                    calcEl = EditHelpers.XmlLinq.GetDescendantUsingURIPattern(
                        rootCalcDoc, sOneURIPattern);
                }
                if (calcEl != null)
                {
                    //if the calcdoc has stylesheet params, it needs a dynamic ss
                    //calcdoc can hold two stylesheet names 
                    //-one for the calc doc and one for the selectedview (the results)
                    string sStylesheetAttributeName =
                        (docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                        ? AppHelpers.Calculator.cStylesheetResourceFileName : AppHelpers.Calculator.cStylesheet2ResourceFileName;
                    string sStylesheetName = EditHelpers.XmlLinq.GetAttributeValue(
                        calcEl, sStylesheetAttributeName);
                    if (!string.IsNullOrEmpty(sStylesheetName))
                    {
                        if (docState
                            == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                        {
                            bHasStylesheet
                                = Helpers.LinqHelpers.HasMainStylesheetWithSameFileName(
                                calcDocURI.URIDataManager.Resource, sStylesheetName);
                        }
                        else if (docState
                            == Helpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                        {
                            bHasStylesheet
                                = Helpers.LinqHelpers.HasMainStylesheetWithSameFileName(
                                docToCalcURI.URIDataManager.Resource, sStylesheetName);
                        }
                        bHasStylesheet = await SaveLinkedViewStylesheetParamsAsync( 
                            bHasStylesheet, docToCalcURI, calcDocURI, rootCalcDoc,
                            calcEl, sStylesheetName, docState);
                    }
                    else
                    {
                        //not possible - the calcdoc needs to have a name set or it could be any of multiple
                    }
                }
                else
                {
                    calcDocURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "ADDINSTYLE_NOSTYLESHEET");
                }
            }
            //else: informational linkedview
            return bHasStylesheet;
        }
        private async Task<bool> SaveLinkedViewStylesheetParamsAsync( 
            bool hasStylesheet, ContentURI docToCalcURI, ContentURI calcDocURI,
            XElement rootCalcDoc, XElement calcEl, string stylesheetName,
            Helpers.GeneralHelpers.DOC_STATE_NUMBER docState)
        {
            bool bHasStylesheet = hasStylesheet;
            //addins can set a stylesheet name the extension object it needs
            string sStylesheetExtensionObjectNameSpaceAttName
                = (docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                ? AppHelpers.Calculator.cStylesheetObjectNS
                : AppHelpers.Calculator.cStylesheet2ObjectNS;
            string sStylesheetExtensionObjectNameSpace
                = EditHelpers.XmlLinq.GetAttributeValue(
                calcEl, sStylesheetExtensionObjectNameSpaceAttName);
            if (!hasStylesheet)
            {
                string sResourceURLArray = string.Empty;
                string sStylesheetPath = string.Empty;
                string sStylesheetURIPattern = string.Empty;
                string sAltDesc = string.Empty;
                bool bNeedsFullPath = true;
                bool bNeedsOneRecord = true;
                stylesheetName = Helpers.AddInStylesheetHelper.GetViewsPanelStylesheetName(docState, docToCalcURI, stylesheetName);
                sResourceURLArray = await GetResourceURLsAsync(calcDocURI,
                    bNeedsOneRecord, bNeedsFullPath,
                    AppHelpers.Resources.RESOURCES_GETBY_TYPES.filename,
                    stylesheetName);
                if (!string.IsNullOrEmpty(sResourceURLArray))
                {
                    int iPositioninArray = 0;
                    //parameter delimited string holding relativepath;altdesc;resourceuri
                    AppHelpers.Resources.GetResourceIdsForResourceFilePaths(
                        sResourceURLArray, iPositioninArray, out sStylesheetPath,
                        out sAltDesc, out sStylesheetURIPattern);
                    if (!string.IsNullOrEmpty(sStylesheetPath))
                    {
                        if (docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                        {
                            //set the resource used to retrieve stylesheet to transform calcDocURI
                            sStylesheetURIPattern = await calcDocURI.URIDataManager.AddResourceToResourceListAsync(
                                calcDocURI, sStylesheetURIPattern, sAltDesc,
                                sStylesheetPath, sStylesheetExtensionObjectNameSpace);
                            if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                                sStylesheetPath))
                            {
                                bHasStylesheet = true;
                            }
                            else
                            {
                                //retrieve from db and store (in case its a new localhost deployment)
                                ContentURI oResourceuri = Helpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                                    calcDocURI.URIDataManager.Resource);
                                if (oResourceuri != null)
                                    bHasStylesheet = await SetPathAndStoreResourceAsync(oResourceuri);
                            }
                        }
                        else if (docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                        {
                            //set the resource used to retrieve stylesheet to transform calcDocURI
                            sStylesheetURIPattern = await docToCalcURI.URIDataManager.AddResourceToResourceListAsync(
                                docToCalcURI, sStylesheetURIPattern, sAltDesc,
                                sStylesheetPath, sStylesheetExtensionObjectNameSpace);
                            if (Helpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                                sStylesheetPath))
                            {
                                bHasStylesheet = true;
                            }
                            else
                            {
                                //retrieve from db and store (in case its a new localhost deployment)
                                ContentURI oResourceuri = Helpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                                    docToCalcURI.URIDataManager.Resource);
                                if (oResourceuri != null)
                                    bHasStylesheet = await SetPathAndStoreResourceAsync(oResourceuri);
                            }
                        }
                    }
                }
            }
            else
            {
                //the extension object namespace may not have been set properly
                //until the addin was actually run
                if (docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                {
                    if (calcDocURI.URIDataManager.Resource != null)
                    {
                        ContentURI oStylesheetURI
                            = Helpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                            calcDocURI.URIDataManager.Resource);
                        if (oStylesheetURI != null)
                        {
                            oStylesheetURI.URIDataManager.ExtensionObjectNamespace
                                = sStylesheetExtensionObjectNameSpace;
                        }
                    }
                }
                else if (docState == Helpers.GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                {
                    if (docToCalcURI.URIDataManager.Resource != null)
                    {
                        ContentURI oStylesheetURI
                            = Helpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                            docToCalcURI.URIDataManager.Resource);
                        if (oStylesheetURI != null)
                        {
                            oStylesheetURI.URIDataManager.ExtensionObjectNamespace
                                = sStylesheetExtensionObjectNameSpace;
                        }
                    }
                }
            }
            return bHasStylesheet;
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
                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }
            }
        }
    }
}
