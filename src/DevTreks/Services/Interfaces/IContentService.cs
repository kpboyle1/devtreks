using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DataEditHelpers = DevTreks.Data.EditHelpers;

namespace DevTreks.Services
{
    /// <summary>
    ///Purpose:		Content management interface for loading, displaying, 
    ///             and interacting with, all DevTreks content.
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    public interface IContentService : IDisposable
    {
        
        Task<Network> GetNetworkAsync(IMemberService memberService, 
            ContentURI uri, int id);
        Task<Network> GetNetworkAsync(IMemberService memberService, 
            ContentURI uri, string networkPartName);
        Task<bool> SetContentModelAndAncestorsAsync(IMemberService memberService, 
            ContentURI uri, bool isInitView);
        //get/set the ancestors of the contenturi
        Task<List<DevTreks.Data.ContentURI>>
            GetAncestorsAsync(ContentURI uri);
        //get/set views that are linked to the contenturi (calculators, analyzers, stories)
        Task<List<System.Linq.IGrouping<int, ContentURI>>>
            GetLinkedViewAsync(ContentURI uri);
        Task<bool> SetLinkedViewAsync(IMemberService memberService, 
            ContentURI uri, bool setLinkedViewMembers);
        //return the contenturi's children
        Task<List<DevTreks.Data.ContentURI>>
            GetChildrenAsync(ContentURI parentURI, ContentURI uri);
        DevTreks.Data.ContentURI GetURI(string name, int id, string networkPartName,
           string nodeName, string fileExtensionType);
        Task<List<GeoRegion>> GetGeoRegionsAsync(ContentURI uri);
        //get a list of available clubgroups 
        Task<List<AccountClass>> GetClubGroupsAsync(ContentURI uri);
        //get a list of available member groups for this georegion
        Task<List<MemberClass>> GetMemberGroupsAsync(ContentURI uri);
        //service related interfaces
        //return the ancestors of a service (to which the contenturi belongs)
        Task<List<DevTreks.Data.ContentURI>>
            GetAgreementAncestorsAndSetServiceAsync(ContentURI uri);
        Task<bool> SetServiceAndChangeAppAsync(ContentURI uri, int serviceId);
        //return the ancestors of a service and the authorization level of the current member.clubinuse
        Task<List<DevTreks.Data.ContentURI>>
            GetAgreementAncestorsByServiceAsync(ContentURI uri, int clubOrMemberId);
        //set the service of the contenturi
        Task<bool> SetServiceAsync(ContentURI uri, bool isBaseService);
        //set the club that owns the service (and uri)
        Task<bool> SetURIOwnerClubFromServiceAsync(ContentURI uri);
        //set uri.uriservice.networkcategories used to classify content
        Task<bool> SetNetworkCategoriesAsync(ContentURI uri);

        //manage packages
        Task<bool> CopyResourceToPackageAsync(ContentURI uri, string resourceRelFilePaths,
            int arrayPos, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs);
        Task<bool> CopyRelatedDataToPackageAsync(ContentURI uri, string currentFilePath, string packageName,
            string fileType, string tempPackageRootDirectory, bool needsAllRelatedData,
            IDictionary<string, string> zipArgs);
        Task<bool> PackageFilesAsync(ContentURI uri, string packageFilePathName,
            string packageType, string digitalSignatureType,
            IDictionary<string, string> zipArgs);
        //set state of data transfer objects
        Task<bool> GetDevTrekContentAsync(ContentURI uri);
        //set the state of the data associated with a contenturi
        Task<bool> SetURIStateAsync(ContentURI uri);
        //the views layer will use the xml doc saved in file system from these three methods
        //relational xml docs Async(metadata)
        Task<bool> SaveURIFirstDocAsync(ContentURI uri);
        //whole xmldocs stored in xmlfields in db
        Task<XmlReader> GetURISecondDocAsync(ContentURI docToCalcURI, ContentURI calcDocURI);
        //base field linkedviews or devpackparts (i.e. base calculators, custom xmldocs)
        Task<XmlReader> GetURISecondBaseDocAsync(ContentURI uri);

        //set tempdoc state Async(version 0.9 allows tempdocs to run addins)
        Task<bool> SetContentModelForTempDocsAsync(IMemberService memberService, 
            ContentURI uri);

        //calculators and analyzers use addins
        Task<bool> RunAddInAsync(ContentURI docToCalcURI, CancellationToken cancellationToken);
        Task<bool> SetAddInNamesAsync(ContentURI calcDocURI);
        Task<bool> SetURIAddInNamesAsync(ContentURI uri);

        //primary interfaces for updating and deleting contenturis (using xml data editing methods)
        Task<bool> EnsureEditFirstDocExistsAsync(ContentURI uri);
        Task<bool> EnsureEditSecondDocExistsAsync(ContentURI uri);
        //edits and deletions
        //ef
        Task<bool> UpdateContentAsync(ContentURI uri, StringDictionary colDeletes,
           IDictionary<string, string> colUpdates);
        Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates);
        //xmldoc
        Task<bool> UpdateAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates, XElement devTrekLinqRoot);
        //sqlio
        Task<bool> UpdateLinkedViewAsync(ContentURI uri, StringDictionary colDeletes,
           IDictionary<string, string> colUpdates);
        Task<bool> UpdateCategoriesAsync(ContentURI uri, StringDictionary colDeletes,
            IDictionary<string, string> colUpdates);
        //primary interfaces for inserting contenturis (using xml data editing methods)
        //ef and sqlxml
        Task<bool> AddSelectionsAsync(ContentURI uri,
           DataEditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
           string selectedAncestorURIPattern, XElement devTrekLinqRoot,
           string numberToAdd);
        Task<bool> AddSelectionsAsync(ContentURI uri, 
            DataEditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption);
        Task<bool> AddDefaultNodesAsync(IMemberService memberService, ContentURI uri);
        //sqlio
        Task<string> AddLinkedViewAsync(ContentURI uri, IDictionary<string, string> newLinkedView,
            bool isSelections);
        //save uri docs
        Task<bool> SaveURISecondDocAsync(ContentURI uri, XmlReader reader);
        Task<bool> SaveURISecondBaseDocAsync(ContentURI uri, bool isMetaData,
            string fileName, XmlReader reader);
        Task<bool> SaveURIResourceFileAsync(ContentURI uri, string fileName, int fileLength,
            string mimeType, Stream postedFileStream);

        //resources management
        Task<bool> SetPathAndStoreResourceAsync(ContentURI resourceuri);
        bool SetPathAndStoreResource(ContentURI resourceuri);
        //return the resources (i.e. images, stylesheets) associated with the contenturi
        Task<List<ContentURI>> GetResourceAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            DevTreks.Data.AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam);
        Task<string> GetResourceURLsAsync(ContentURI uri,
            bool needsOneRecord, bool needsFullPath,
            DevTreks.Data.AppHelpers.Resources.RESOURCES_GETBY_TYPES getResourceByType,
            string getResourceByParam);
        Task<bool> UpdateURIResourceFileUploadMsgAsync(ContentURI fileUploadURI, string message);
        //stylesheets
        Task SetStylesheetStateAsync(ContentURI docToCalcURI, 
            DevTreks.Data.Helpers.GeneralHelpers.DOC_STATE_NUMBER docState);
        Task SetLinkedViewStateAsync(ContentURI docToCalcURI, ContentURI uri);
        Task<string> GetLinkedListsArrayAsync(ContentURI docToCalcURI, ContentURI calcDocURI);
        //simple audit trail
        Task<int> InsertAuditItemAsync(ContentURI uri);
        
    }
}
