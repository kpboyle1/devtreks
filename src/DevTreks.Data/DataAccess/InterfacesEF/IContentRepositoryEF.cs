using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Data
{
    /// <summary>
    ///Purpose:		Principal interface for accessing, building and editing DevTreks 
    ///             content (i.e. uris). 
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public interface IContentRepositoryEF
    {
        //return an iqueryable list of georegions
        Task<List<GeoRegion>> GetGeoRegionsAsync(ContentURI uri);
        //return an iqueryable list of club groups
        Task<List<AccountClass>> GetClubGroupsAsync(ContentURI uri);
        //return an iqueryable list of member groups
        Task<List<MemberClass>> GetMemberGroupsAsync(ContentURI uri);
        //get the ancestors of the current uri (uri.uridatamanger.Ancestors)
        Task<List<ContentURI>> GetAncestorsAsync(ContentURI uri);
        //get the children of the current uri (uri.uridatamanger.Children)
        Task<List<ContentURI>> GetChildrenAsync(ContentURI parentURI, ContentURI uri);

        //get the ancestors of the current uri.URIService or set uri.URIService
        Task<List<ContentURI>> GetAgreementAncestorsAndSetServiceAsync(ContentURI uri);
        //get the ancestors of the current uri.URIService and public (clubid)
        //or private (member.clubinuse) authorization level
        Task<List<ContentURI>> GetAgreementAncestorsAndAuthorizationsAsync(ContentURI uri,
            int clubOrMemberId);
        //set uri.URIService object
        Task<bool> SetServiceAsync(ContentURI uri, bool isBaseService);
        //set uri.URIService object and change application
        Task<bool> SetServiceAndChangeApplicationAsync(ContentURI uri, int serviceId);
        //set the uri.URIService owning club (uri.URIClub)
        Task<bool> SetClubByServiceAsync(ContentURI uri);
        //get the uri.uriservice.networkcategories filtered by a network and supapptype 
        Task<List<ContentURI>> GetNetworkCategoriesAsync(
            ContentURI uri);

        //manage packages
        Task<bool> CopyResourceToPackageAsync(
            ContentURI uri, string resourceRelFilePaths,
            int arrayPos, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs);
        Task<bool> CopyRelatedDataToPackageAsync(
            ContentURI uri, string currentFilePath, string packageName,
            string fileType, string tempPackageRootDirectory, bool needsAllRelatedData,
            IDictionary<string, string> zipArgs);
        Task<bool> PackageFilesAsync(ContentURI uri, string packageFilePathName,
            string packageType, string digitalSignatureType,
            IDictionary<string, string> zipArgs);

        //manage uri content state
        //Entity Framework with data transfer objects
        Task<bool> GetDevTrekContentAsync(ContentURI uri, bool saveInFileSystemContent);
        //relational xml docs
        Task<bool> SaveURIFirstDocAsync(ContentURI uri);
        //whole xmldocs stored in xmlfields in db join tables
        Task<XmlReader> GetURISecondDocAsync(ContentURI docToCalcURI,
            ContentURI calcDocURI);
        //whole xmldocs stored in xmlfields in db base tables
        //(linkedview or devpackpart)
        Task<XmlReader> GetURISecondBaseDocAsync(ContentURI uri);
        //linked views (stored in a grouped, paginated list)
        Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetLinkedViewAsync(
            ContentURI uri);
        //need to verify files being analyzed against db records
        Task<IEnumerable<System.Linq.IGrouping<int, ContentURI>>> GetLinkedViewForAnalysesAsync(
            ContentURI uri);

        //calculators, analyzers, and story-tellers using addins
        Task<bool> SetAddInNamesAsync(ContentURI calcDocURI);
        Task<bool> SetURIAddInNamesAsync(ContentURI uri);

        //primary interfaces for editing uris (using xml data editing methods)
        //sqlxml
        Task<bool> UpdateAsync(bool isDeletes, ContentURI uri,
            IDictionary<string, string> colUpdates, StringDictionary colDeletes,
            XElement devTrekLinqRoot);
        Task<bool> AddSelectionsAsync(ContentURI uri,
           Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
           string selectedAncestorURIPattern, XElement devTrekLinqRoot,
           string numberToAdd);
        //ef
        Task<bool> UpdateAsync(ContentURI uri, IDictionary<string, string> colUpdates, 
            StringDictionary colDeletes);
        Task<bool> AddSelectionsAsync(ContentURI uri,
           Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
           string selectedAncestorURIPattern, string numberToAdd);
        Task<string> AddLinkedViewAsync(ContentURI uri,
            IDictionary<string, string> newLinkedView,
            bool isSelections, string insertedIdsArray);
        Task<bool> AddCategoryAsync(ContentURI uri, int serviceGroupId, int networkId,
            int numberToAdd);

        //save various types of uri content
        Task<bool> SaveURISecondDocAsync(ContentURI uri, XmlReader reader);
        Task<bool> SaveURISecondBaseDocAsync(ContentURI uri, bool isMetaData,
            string fileName, XmlReader reader);
        Task<bool> SaveURIResourceFileAsync(ContentURI uri, string fileName, int fileLength,
            string mimeType, Stream postedFileStream);

        //get the resources (i.e. image, stylesheet, associated data) of the current uri 
        Task<bool> SetPathAndStoreResourceAsync(ContentURI resourceuri);
        bool SetPathAndStoreResource(ContentURI resourceuri);
        Task<List<ContentURI>> GetResourceAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam);
        Task<string> GetResourceURLsAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam);
        string GetResourceURLs(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam);
        Task<bool> UpdateURIResourceFileUploadMsgAsync(ContentURI fileUploadURI, string message);
        //simple audit trail
        Task<int> InsertNewAuditItemAsync(ContentURI uri, string serverSubAction, int memberId,
            int clubInUseId, string editedDocURIPattern, string editedDocFullPath,
            string clubInUseAuthorizationLevel, string memberRole,
            DateTime dtEditDate, int ownerClubId);

        //stylesheets
        Task GetStylesheetParametersForAddInAsync(ContentURI docToCalcURI,
          IDictionary<string, string> styleParams);
        Task SetStylesheetStateAsync(ContentURI docToCalcURI,
            DevTreks.Data.Helpers.GeneralHelpers.DOC_STATE_NUMBER docState);
        Task SetLinkedViewStateAsync(ContentURI docToCalcURI, ContentURI uri);
        Task<string> GetLinkedListsArrayAsync(ContentURI docToCalcURI, ContentURI calcDocURI);
        Task SetTempDocStylesheetURIAsync(ContentURI uri);

        void Dispose();
    }
}
