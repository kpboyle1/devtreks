using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
//azure machine learning web services
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities for Azure cloud storage
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///NOTE:        uriPath below can be the absolute URI to the blob, 
    ///             or a relative URI beginning with the container name.
    ///             Don't use catch-trys -these should be debugged to the 
    ///             point that they only effect performance -allow the 
    ///             exceptions to be passed to UI, recorded, and displayed
    /// </summary>
    public class AzureIOAsync
    {
        public AzureIOAsync(ContentURI uri)
        {
            //2.0.0: StorageAccount now uses StorageConnectionString set in Startup.cs
            _uri = uri;
        }
        private ContentURI _uri { get; set; }
        private CloudStorageAccount _storageAccount
        {
            get
            {
                //2.0.0 changes
                string account = _uri.URIDataManager.StorageConnection;
                if (account == "UseDevelopmentStorage=true")
                {
                    return CloudStorageAccount.DevelopmentStorageAccount;
                }
                CloudStorageAccount StorageAccount 
                    = CloudStorageAccount.Parse(account);
                return StorageAccount;
            }
        }
        async public Task InitBlobStorage()
        {
            //2.0.0
            var account = _storageAccount;
            var client = account.CreateCloudBlobClient();
            CloudBlobContainer resContainer = client.GetContainerReference(_uri.URIDataManager.ResourceURIName);
            //Create the "resources" container if it doesn't already exist.
            if (await resContainer.CreateIfNotExistsAsync())
            {
                // Enable public access on the newly created container
                await resContainer.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
            CloudBlobContainer cContainer = client.GetContainerReference(_uri.URIDataManager.ContentURIName);
            //Create the "container" container if it doesn't already exist.
            if (await cContainer.CreateIfNotExistsAsync())
            {
                // Enable public access on the newly created "images" container
                await cContainer.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
            CloudBlobContainer tempContainer = client.GetContainerReference(_uri.URIDataManager.TempDocsURIName);
            //Create the "temp" container if it doesn't already exist.
            if (await tempContainer.CreateIfNotExistsAsync())
            {
                // Enable public access on the newly created container
                await tempContainer.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
        }
        public CloudBlobClient GetBlobClient()
        {
            //2.0.0
            var account = _storageAccount;
            ////get a handle on storage account, create a blob service client and 
            ////get container proxy
            return account.CreateCloudBlobClient();
        }
        
        //this is the exact save pattern as the filesystem storage in Resources.GetAndSaveResourceURLInFiles
        public async Task<string> GetAndSaveResourceURLInCloudAsync( 
            ContentURI uri, string resourceURIPath)
        {
            string sResourceWebPath = string.Empty;
            //see if it can be retrieved from the db and stored in the proper blob
            if (!string.IsNullOrEmpty(resourceURIPath))
            {
                bool bNeedsWebFullPath = true;
                sResourceWebPath = AppHelpers.Resources.GetRootedResourcePath(uri,
                    resourceURIPath, bNeedsWebFullPath,
                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
                if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                    sResourceWebPath) == false)
                {
                    //store the resource in a blob
                    sResourceWebPath 
                        = await SaveResourceURLInCloudAsync(uri, sResourceWebPath);
                }
            }
            return sResourceWebPath;
        }
        public async Task<string> SaveResourceURLInCloudAsync( 
            ContentURI uri, string resourceURIPath)
        {
            string sResourceWebPath = string.Empty;
            //see if it can be retrieved from the db and stored in the proper blob
            if (!string.IsNullOrEmpty(resourceURIPath))
            {
                bool bNeedsWebFullPath = true;
                //this is needed because can call this method directly
                sResourceWebPath = AppHelpers.Resources.GetRootedResourcePath(uri,
                    resourceURIPath, bNeedsWebFullPath,
                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
                //get a reference to a blob
                //blob is named resourceuripath: crops/resourcepack_123/resource_456/cloud.png
                CloudBlockBlob blob = GetBlob(resourceURIPath);
                if (blob != null)
                {
                    //save the resource in the blob
                    await StoreResourceInCloudBobByIdAsync(blob, uri);
                    sResourceWebPath = blob.Uri.AbsoluteUri;
                }
            }
            return sResourceWebPath;
        }
        public string GetAndSaveResourceURLInCloud( 
            ContentURI uri, string resourceURIPath)
        {
            string sResourceWebPath = string.Empty;
            //see if it can be retrieved from the db and stored in the proper blob
            if (!string.IsNullOrEmpty(resourceURIPath))
            {
                bool bNeedsWebFullPath = true;
                sResourceWebPath = AppHelpers.Resources.GetRootedResourcePath(uri,
                    resourceURIPath, bNeedsWebFullPath,
                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
                if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                    sResourceWebPath) == false)
                {
                    //store the resource in a blob
                    sResourceWebPath = SaveResourceURLInCloud(uri, sResourceWebPath);
                }
            }
            return sResourceWebPath;
        }
        public string SaveResourceURLInCloud( 
            ContentURI uri, string resourceURIPath)
        {
            string sResourceWebPath = string.Empty;
            //see if it can be retrieved from the db and stored in the proper blob
            if (!string.IsNullOrEmpty(resourceURIPath))
            {
                bool bNeedsWebFullPath = true;
                //this is needed because can call this method directly
                sResourceWebPath = AppHelpers.Resources.GetRootedResourcePath(uri,
                    resourceURIPath, bNeedsWebFullPath,
                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
                //get a reference to a blob
                //blob is named resourceuripath: crops/resourcepack_123/resource_456/cloud.png
                CloudBlockBlob blob = GetBlob(resourceURIPath);
                if (blob != null)
                {
                    //save the resource in the blob
                    StoreResourceInCloudBobById(blob, uri);
                    sResourceWebPath = blob.Uri.AbsoluteUri;
                }
            }
            return sResourceWebPath;
        }
        
        public void StoreResourceInCloudBobById(
            CloudBlockBlob blob, ContentURI uri)
        {
            if (uri.URIName == Helpers.GeneralHelpers.GetResource("FILE_NONE")
                || uri.URIName == Helpers.GeneralHelpers.NONE)
            {
                //don't run the query if no resource is on hand
                return;
            }
            bool bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(uri.URIName);
            if (!bIsXmlDoc)
                bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(blob.Uri.AbsoluteUri);
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
		    {
			    sqlIO.MakeInParam("@PKId",              SqlDbType.Int, 4, uri.URIId),
                sqlIO.MakeInParam("@IsXmlDoc",          SqlDbType.Bit, 1, bIsXmlDoc)
		    };
            //don't run sequential in cloud - process all of the bytes at once
            SqlDataReader oDataReader = sqlIO.RunProc("0GetResourceByResourceId", oPrams);
            if (oDataReader != null && (!oDataReader.IsClosed))
            {
                SaveBitsInCloudBlob(oDataReader, blob);
            }
            sqlIO.Dispose();
        }
        private void SaveBitsInCloudBlob(SqlDataReader dataReader,
            CloudBlockBlob blob)
        {
            if (dataReader != null && (!dataReader.IsClosed))
            {
                using (dataReader)
                {
                    //check whether the dataReader timed out (downloading video to localhost during debugging)
                    while (dataReader.Read())
                    {
                        string sResourceMimeType = string.Empty;
                        //sequential data has to be read in order (once an index is passed, can't return to it)
                        //get the media type
                        if (dataReader.IsDBNull(0) == false)
                        {
                            sResourceMimeType = dataReader.GetString(0);
                        }
                        //update the filename
                        if (dataReader.IsDBNull(1) == false)
                        {
                            string sFileName = dataReader.GetString(1);
                            if (!string.IsNullOrEmpty(sFileName))
                            {
                                string sOldFileName = Helpers.GeneralHelpers.GetLastSubString(blob.Name,
                                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
                                if (!sOldFileName.Equals(sFileName)
                                    && !string.IsNullOrEmpty(sOldFileName))
                                {
                                    string sNewBlobName = blob.Name.Replace(sOldFileName, sFileName);
                                    blob = GetBlob(sNewBlobName);
                                }
                            }
                        }
                        if (dataReader.IsDBNull(2) == false)
                        {
                            blob.Properties.ContentType = sResourceMimeType;
                            bool bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(blob.Name);
                            if (!bIsXmlDoc)
                                bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(blob.Uri.AbsoluteUri);
                            //these two methods could be wrapped in the pattern used in FileStorageIO
                            if (bIsXmlDoc)
                            {
                                SqlXml oSqlXml = dataReader.GetSqlXml(2);
                                if (oSqlXml.IsNull == false)
                                {
                                    XmlReader oReader = oSqlXml.CreateReader();
                                    Helpers.XmlFileIO oXmlFileIO = new Helpers.XmlFileIO();
                                    using (oReader)
                                    {
                                        string sURIPath = UploadXmlReaderBlob(blob, oReader);
                                    }
                                }
                            }
                            else
                            {
                                string sURI = UploadBinaryBlobStream(blob, dataReader, 2);
                            }
                        }
                    }
                }
            }
        }
        public string UploadXmlReaderBlob(CloudBlockBlob blob,
            XmlReader reader)
        {
            string sURI = string.Empty;
            //upload a reader as text
            if (reader.NodeType != XmlNodeType.Element)
            {
                reader.MoveToContent();
            }
            if (reader.NodeType == XmlNodeType.Element)
            {
                blob.UploadText(reader.ReadOuterXml());
                sURI = blob.Uri.AbsoluteUri;
            }
            return sURI;
        }
        public async Task<string> UploadBinaryBlobStreamAsync(CloudBlockBlob blob,
            SqlDataReader sqlReader, int columnIndex)
        {
            string sURI = string.Empty;
            bool bIsSaved = false;
            //try
            //{
            //don't use using here or only one record can be processed by reader 
            Stream strm = sqlReader.GetSqlBytes(columnIndex).Stream;
            using (strm)
            {
                await blob.UploadFromStreamAsync(strm);
            }
            //errors get kicked back to controller
            bIsSaved = true;
            //}
            //catch (StorageException x)
            //{
            //    bIsSaved = false;
            //}
            if (bIsSaved)
            {
                sURI = blob.Uri.AbsoluteUri;
            }
            return sURI;
        }
        public string UploadBinaryBlobStream(CloudBlockBlob blob,
            SqlDataReader sqlReader, int columnIndex)
        {
            string sURI = string.Empty;
            bool bIsSaved = UploadBinaryBlobStream(blob, sqlReader.GetSqlBytes(columnIndex).Stream);
            if (bIsSaved)
            {
                sURI = blob.Uri.AbsoluteUri;
            }
            return sURI;
        }
        public async Task<string> SaveResourceURLInCloudAsync(string resourceURIPath, 
            Stream postedFileStream)
        {
            string sResourceWebPath = string.Empty;
            //see if it can be retrieved from the db and stored in the proper blob
            if (!string.IsNullOrEmpty(resourceURIPath))
            {
                //get a reference to a blob
                //blob is named resourceuripath: crops/resourcepack_123/resource_456/cloud.png
                CloudBlockBlob blockBlob = GetBlob(resourceURIPath);
                if (blockBlob != null)
                {
                    //the using on the posted is done earlier
                    //save the resource in the blob
                    await blockBlob.UploadFromStreamAsync(postedFileStream);
                    //regular way is relative path
                    sResourceWebPath = blockBlob.Uri.AbsoluteUri;
                }
            }
            return sResourceWebPath;
        }
        public string GetResourceURIPath(string existingURIPath, ref string errorMsg)
        {
            string sURIPath = string.Empty;
            //try to get the blob out of storage
            //blob is named either 
            //resourceuripath: crops/resourcepack_123/resource_456/cloud.png
            //or http://wwww.this.org/resources/crops/...
            CloudBlockBlob blob = GetBlob(existingURIPath);
            if (blob != null)
            {
                sURIPath = blob.Uri.AbsoluteUri;
            }
            else
            {
                errorMsg
                    = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "RESOURCES_NOBLOB");
            }
            return sURIPath;
        }
        public string GetContentURIPath(ContentURI uri, string existingURIPath)
        {
            string sURIPath = string.Empty;
            FileStorageIO.PLATFORM_TYPES ePlatform
                    = uri.URIDataManager.PlatformType;
            if (ePlatform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                //try to get the blob out of storage
                //blob is named either 
                //contenturipath: club_148/economics/budgets/servicebase_1819 ...
                //or a:/club_148/economics/budgets/servicebase_1819 ...
                //or http://wwww.this.org/content/crops/...
                CloudBlockBlob blob = GetBlob(existingURIPath);
                if (blob != null)
                {
                    sURIPath = blob.Uri.AbsoluteUri;
                }
                else
                {
                    uri.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "RESOURCES_NOBLOB");
                }
            }
            else
            {
                if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                    existingURIPath))
                {
                    sURIPath = existingURIPath;
                }
                else
                {
                    uri.ErrorMessage
                        = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "RESOURCES_NOFILE");
                }
            }
            return sURIPath;
        }
        
        //transition to async
        public bool BlobExists(string fullBlobURI)
        {
            bool bBlobExists = false;
            if (!string.IsNullOrEmpty(fullBlobURI))
            {
                //containername is always iindex = 4 (1-based) in http://hostname/containername
                CloudBlobContainer container = GetContainer(fullBlobURI);
                if (container != null)
                {
                    //blobname is always the full string after container name
                    string sBlobName = GetBlobRelativeName(container.Name, fullBlobURI);
                    if (!string.IsNullOrEmpty(sBlobName))
                    {
                        //a references to the blob is not the blob itself
                        CloudBlockBlob blob = container.GetBlockBlobReference(sBlobName);
                        //this makes an http request and should be async
                        if (blob.Exists())
                        {
                            return true;
                        }
                    }
                }
            }
            return bBlobExists;
        }
        public async Task<bool> BlobExistsAsync(string fullBlobURI)
        {
            bool bBlobExists = false;
            if (!string.IsNullOrEmpty(fullBlobURI))
            {
                //containername is always iindex = 4 (1-based) in http://hostname/containername
                CloudBlobContainer container = GetContainer(fullBlobURI);
                if (container != null)
                {
                    //blobname is always the full string after container name
                    string sBlobName = GetBlobRelativeName(container.Name, fullBlobURI);
                    if (!string.IsNullOrEmpty(sBlobName))
                    {
                        //a references to the blob is not the blob itself
                        CloudBlockBlob blob = container.GetBlockBlobReference(sBlobName);
                        //this makes an http request
                        if (await blob.ExistsAsync())
                        {
                            return true;
                        }
                    }
                }
            }
            return bBlobExists;
        }

        public async Task<bool> BlobExistsAsync(CloudBlobClient blobClient,
            string blobName, string containerName)
        {
            bool bBlobExists = false;
            if (blobClient != null
                && (!string.IsNullOrEmpty(blobName)))
            {
                CloudBlobContainer container
                    = blobClient.GetContainerReference(containerName);
                if (container != null)
                {
                    string sBlobName = GetBlobRelativeName(containerName, blobName);
                    CloudBlockBlob blob = container.GetBlockBlobReference(sBlobName);
                    if (await blob.ExistsAsync())
                    {
                        return true;
                    }
                }
            }
            return bBlobExists;
        }
        //use one method for getting any blob
        //bloburi has to be full uri
        public CloudBlockBlob GetBlob(string blobURI)
        {
            CloudBlockBlob cBlob = null;
            if (!string.IsNullOrEmpty(blobURI))
            {
                CloudBlobContainer container = GetContainer(blobURI);
                //containername is always iindex = 4 (1-based) in http://hostname/containername
                if (container != null)
                {
                    if (container.Exists())
                    {
                        //blobname is always the full string after container name
                        string sBlobName = GetBlobRelativeName(container.Name, blobURI);
                        if (!string.IsNullOrEmpty(sBlobName))
                        {
                            //a references to the blob is not the blob itself
                            cBlob = container.GetBlockBlobReference(sBlobName);
                        }
                    }
                    else
                    {
                        //2.0.0 refactor: moved here from Global.cs
                        //suggests that these methods should be async
                        Task wait = InitBlobStorage();
                        //finish with same method (don't call the same method in case of endless loop)
                        container = GetContainer(blobURI);
                        if (container != null)
                        {
                            if (container.Exists())
                            {
                                string sBlobName = GetBlobRelativeName(container.Name, blobURI);
                                if (!string.IsNullOrEmpty(sBlobName))
                                {
                                    cBlob = container.GetBlockBlobReference(sBlobName);
                                }
                            }
                        }
                    }
                }
            }
            return cBlob;
        }
        public CloudBlobContainer GetContainer(string blobURI)
        {
            CloudBlobContainer container = null;
            CloudBlobClient cBlobClient = GetBlobClient();
            if (cBlobClient != null)
            {
                string sContainerName = GetContainerNameFull(blobURI);
                if (!string.IsNullOrEmpty(sContainerName))
                {
                    container = cBlobClient.GetContainerReference(sContainerName);
                }
                else
                {
                    sContainerName = GetContainerNameRelative(blobURI);
                    if (!string.IsNullOrEmpty(sContainerName))
                    {
                        container = cBlobClient.GetContainerReference(sContainerName);
                    }
                }
            }
            return container;
        }
        public static string GetContainerNameFull(string fullURIPath)
        {
            string sContainerName = string.Empty;
            if (fullURIPath.StartsWith("http"))
            {
                //containerName is always iindex = 4 (0-based) in http://hostname/containername/blobname
                sContainerName = GeneralHelpers.GetSubstringFromFront(fullURIPath,
                    GeneralHelpers.WEBFILE_PATH_DELIMITERS, 4);
                if (sContainerName == "devstoreaccount1")
                {
                    //development emulator uses devstoreaccount1/commontreks
                    sContainerName = GeneralHelpers.GetSubstringFromFront(fullURIPath,
                        GeneralHelpers.WEBFILE_PATH_DELIMITERS, 5);
                }
            }
            return sContainerName;
        }
        public static string GetContainerNameRelative(string relURIPath)
        {
            string sContainerName = string.Empty;
            if (Path.IsPathRooted(relURIPath))
            {
                //containerName is always iindex = 2 (0-based) in C:\containername
                sContainerName = GeneralHelpers.GetSubstringFromFront(relURIPath,
                    GeneralHelpers.FILE_PATH_DELIMITER, 2); ;
            }
            return sContainerName;
        }
        public static string GetBlobRelativeName(string containerName, string blobFullOrRelName)
        {
            //blobs can be retrieved either with absolute paths or relative paths that include container name
            string sRelBlobName = string.Empty;
            bool bIsWebPath
                = blobFullOrRelName.StartsWith("http");
            if (!bIsWebPath)
            {
                sRelBlobName = blobFullOrRelName;
            }
            else
            {
                //blobname is always iindex = 4 (0-based) in https://hostname/containername/blobname
                int iIndex = blobFullOrRelName.IndexOf(containerName);
                if (iIndex > 0)
                {
                    //substring +1 is web path delimiter
                    sRelBlobName = blobFullOrRelName.Substring(iIndex + containerName.Length + 1);
                }
            }
            return sRelBlobName;
        }
        //retain for large blobs (more documentation can be found on azure)
        //public CloudBlobClient GetBlobClient(string uriPath)
        //{
        //    var account = StorageUtils.StorageAccount;
        //    var client = account.CreateCloudBlobClient();
        //    return client;
        //}
        //public bool SetBlobClientForLargeFileUpload(string uriPath)
        //{
        //    bool bHasClientSettings = false;
        //    //set client just like blob (use storage credentials)
        //    CloudBlobClient client = GetBlobClient(uriPath);

        //    //stay with the defaults until testing proves they need to be changed
        //    //4MB is the default
        //    //version 1.5.2 not supported
        //    //client.WriteBlockSizeInBytes = 4;

        //    //system threads taken from system pool
        //    client.ParallelOperationThreadCount = 4;

        //    //this will break blobs up automatically in WriteBlockSize after this size
        //    client.SingleBlobUploadThresholdInBytes = 32;

        //    if (client != null)
        //    {
        //        bHasClientSettings = true;
        //    }
        //    return bHasClientSettings;
        //}
        //2.0.0 deprecated: can use the same file system path on azure and filesystem
        //as long as it references appsettings.DefaultRootFullFilePath
        public string GetLocalResourceDirectoryPath()
        {
            string sLocalRootPath = string.Empty; 
            //Azure App Service Local Cache
            //https://azure.microsoft.com/en-us/documentation/articles/app-service-local-cache/
            //use D:\Home on azure
            //D:\Home will point to the local cache.D:\local will continue pointing to the temporary VM specific storage.


            //deprecated: azure production gives resource too long message
            //https://azure.microsoft.com/en-us/documentation/articles/best-practices-caching/
            //Azure Websites also comes with a Temp folder. The path is defined in the environment variable %TEMP% and accessible using Path.GetTempPath
            //remember to delete files and that other apps on same server could interfere
            //string sLocalRootPath = Path.GetTempPath();

            //documentation indicates azure cache could also be used
            //LocalResource cache = RoleEnvironment.GetLocalResource("LocalDriveCache");
            //string sLocalRootPath = cache.RootPath;
            return sLocalRootPath;
        }
       
        //copy an existing blob to a new blob
        public bool CopyBlob(ContentURI uri,
            string fromURIPath, string toURIPath)
        {
            bool bHasCopied = false;
            if (FileStorageIO.URIAbsoluteExists(uri, fromURIPath))
            {
                CloudBlockBlob fromblob = GetBlob(fromURIPath);
                CloudBlockBlob toblob = GetBlob(toURIPath);
                if ((fromblob != null) && (toblob != null))
                {
                    //2.0.0
                    string sResult = toblob.StartCopy(fromblob, null, null);
                    bHasCopied = true;
                }
            }
            return bHasCopied;
        }
        public async Task<bool> CopyBlobAsync(ContentURI uri,
            string fromURIPath, string toURIPath)
        {
            bool bHasCopied = false;
            if (FileStorageIO.URIAbsoluteExists(uri, fromURIPath))
            {
                CloudBlockBlob fromblob = GetBlob(fromURIPath);
                CloudBlockBlob toblob = GetBlob(toURIPath);
                if ((fromblob != null) && (toblob != null))
                {
                    //2.0.0
                    string sResult = await toblob.StartCopyAsync(fromblob, CancellationToken.None);
                    bHasCopied = true;

                }
            }
            return bHasCopied;
        }
        public bool MoveFileToBlob(
            string fromFilePath, string toURIPath)
        {
            bool bHasCopied = false;
            //calling procedure checks existence of fromfile
            CloudBlockBlob toblob = GetBlob(toURIPath);
            if (toblob != null)
            {
                bHasCopied = CopyFileToBlob(toblob, fromFilePath);
            }
            return bHasCopied;
        }
        public bool MoveBlob(
            string fromURIPath, string toURIPath)
        {
            bool bHasCopied = false;
            //calling procedure checks existence of fromfile
            CloudBlockBlob fromblob = GetBlob(fromURIPath);
            CloudBlockBlob toblob = GetBlob(toURIPath);
            if ((fromblob != null) && (toblob != null))
            {
                //2.0.0
                string sResult = toblob.StartCopy(fromblob, null, null);
                //toblob.StartCopyFromBlob(fromblob, null, null, null);
                fromblob.Delete();
                bHasCopied = true;
            }
            return bHasCopied;
        }
        public async Task<bool> WriteFileXmlAsync(XmlReader reader, string filePath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(filePath))
            {
                CloudBlockBlob blob = GetBlob(filePath);
                if (blob != null)
                {
                    using (reader)
                    {
                        reader.MoveToContent();
                        bHasSaved = await SaveTextBlobAsync(blob, reader.ReadOuterXml());
                    }
                }
            }
            return bHasSaved;
        }
        public bool SaveXmlInURI(XmlReader reader, string fullURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    UploadXmlReaderBlob(blob, reader);
                    bHasSaved = true;
                }
            }
            return bHasSaved;
        }
        public bool SaveXmlWriterInURI(StringWriter writer, string fullURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    bHasSaved = UploadStringWriterBlob(blob, writer);
                }
            }
            return bHasSaved;
        }
        public async Task<bool> SaveXmlWriterInURIAsync(StringWriter writer, string fullURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    bHasSaved = await UploadStringWriterBlobAsync(blob, writer);
                }
            }
            return bHasSaved;
        }
        public async Task<bool> WriteHtmlTextFileAsync(StringWriter writer, string fullURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    bHasSaved = await SaveHtmlWriterBlobAsync(blob, writer);
                }
            }
            return bHasSaved;
        }
        public bool SaveHtmlWriterInURI(StringWriter writer, string fullURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    UploadHtmlWriterBlob(blob, writer);
                    bHasSaved = true;
                }
            }
            return bHasSaved;
        }
        public bool SaveStringInURI(string content, string fullURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    UploadStringBlob(blob, content);
                    bHasSaved = true;
                }
            }
            return bHasSaved;
        }
        public bool CopyFileToBlob(CloudBlockBlob blob,
            string fullFilePath)
        {
            bool bHasWrote = false;
            using (var fileStream = File.OpenRead(fullFilePath))
            {
                blob.UploadFromStream(fileStream);
            } 
            //or
            //blob.UploadFromFile(fullFilePath, FileMode.Read or Open, null, null, null);
            bHasWrote = true;
            return bHasWrote;
        }
        public async Task<bool> CopyFileToBlobAsync(CloudBlockBlob blob,
            string fullFilePath)
        {
            bool bHasWrote = false;
            using (var fileStream = File.OpenRead(fullFilePath))
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
            //or
            //blob.UploadFromFile(fullFilePath, FileMode.Read or Open, null, null, null);
            bHasWrote = true;
            return bHasWrote;
        }
        public bool UploadSqlStringBlob(CloudBlockBlob blob,
            SqlDataReader sqlReader, int columnIndex)
        {
            bool bHasWrote = false;
            if (sqlReader == null || blob == null)
            {
                return false;
            }
            //upload a sqlstring as text
            using (sqlReader)
            {

                //calling procedure closes sqlreader
                blob.UploadText(sqlReader.GetSqlString(columnIndex).ToString());
                bHasWrote = true;
            }
            return bHasWrote;
        }
        public bool UploadStringBlob(CloudBlockBlob blob,
            string content)
        {
            bool bHasWrote = false;
            if (string.IsNullOrEmpty(content) || blob == null)
            {
                return false;
            }
            //upload a string
            blob.UploadText(content);
            bHasWrote = true;
            return bHasWrote;
        }
        public async Task<bool> SaveHtmlWriterBlobAsync(CloudBlockBlob blob,
            StringWriter writer)
        {
            //upload a writer containing html (avoid memory issues with html strings)
            if (writer == null || blob == null)
            {
                return false;
            }
            bool bHasSaved = false;
            using (writer)
            {
                //2.0.0 changes (same decoding as FileIO.WriteHtmlTextFileAsync())
                byte[] encodedText = Encoding.UTF8.GetBytes(
                    System.Net.WebUtility.HtmlDecode(writer.ToString()));
                if (encodedText.Length > 0)
                {
                    using (CloudBlobStream stream = blob.OpenWrite())
                    {
                        if (stream.CanWrite)
                        {
                            await stream.WriteAsync(encodedText, 0, encodedText.Length);
                            bHasSaved = true;
                        }
                    }
                }
            }
            return bHasSaved;
        }
        public async Task<bool> SaveHtmlTextBlobAsync(CloudBlockBlob blob,
           string text)
        {
            if (string.IsNullOrEmpty(text) || blob == null)
            {
                return false;
            }
            //upload a writer containing html (avoid memory issues with html strings)
            bool bHasSaved = false;
            //2.0.0 changes
            //calling procedure closes writer
            byte[] encodedText = Encoding.UTF8.GetBytes(
                System.Net.WebUtility.HtmlDecode(text));
            if (encodedText.Length > 0)
            {
                using (CloudBlobStream stream = blob.OpenWrite())
                {
                    if (stream.CanWrite)
                    {
                        await stream.WriteAsync(encodedText, 0, encodedText.Length);
                        bHasSaved = true;
                    }
                }
            }
            return bHasSaved;
        }
        
        public async Task<bool> SaveTextBlobAsync(CloudBlockBlob blob,
           string text)
        {
            //upload a writer containing html (avoid memory issues with html strings)
            bool bHasSaved = false;
            if (string.IsNullOrEmpty(text) || blob == null)
            {
                return false;
            }
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            if (encodedText.Length > 0)
            {
                using (CloudBlobStream stream = blob.OpenWrite())
                {
                    if (stream.CanWrite)
                    {
                        await stream.WriteAsync(encodedText, 0, encodedText.Length);
                        bHasSaved = true;
                    }
                }
            }
            return bHasSaved;
        }
        public bool UploadHtmlWriterBlob(CloudBlockBlob blob,
            StringWriter writer)
        {
            bool bHasWrote = false;
            if (blob == null)
            {
                return false;
            }
            //upload a writer containing html (avoid memory issues with html strings)
            using (writer)
            {
                //2.0.0 changes
                blob.UploadText(System.Net.WebUtility.HtmlDecode(
                    writer.GetStringBuilder().ToString()));
                bHasWrote = true;
            }
            return bHasWrote;
        }
        public bool UploadStringWriterBlob(CloudBlockBlob blob,
           StringWriter writer)
        {
            bool bHasWrote = false;
            if (blob == null)
            {
                return false;
            }
            //upload a writer containing html (avoid memory issues with html strings)
            using (writer)
            {
                blob.UploadText(writer.GetStringBuilder().ToString());
                bHasWrote = true;
            }
            return bHasWrote;
        }
        public async Task<bool> UploadStringWriterBlobAsync(CloudBlockBlob blob,
            StringWriter writer)
        {
            bool bHasWrote = false;
            if (blob == null)
            {
                return false;
            }
            //upload a writer containing html (avoid memory issues with html strings)
            using (writer)
            {
                //calling procedure closes writer
                await blob.UploadTextAsync(writer.GetStringBuilder().ToString());
                bHasWrote = true;
            }
            return bHasWrote;
        }
        public async Task<string> UploadXmlReaderBlobAsync(CloudBlockBlob blob,
            XmlReader reader)
        {
            string sURI = string.Empty;
            if (blob == null)
            {
                return sURI;
            }
            //upload a reader as text
            if (reader.NodeType != XmlNodeType.Element)
            {
                reader.MoveToContent();
            }
            if (reader.NodeType == XmlNodeType.Element)
            {
                //calling procedure closes reader
                await blob.UploadTextAsync(reader.ReadOuterXml());
                sURI = blob.Uri.AbsoluteUri;
            }
            return sURI;
        }

        public async Task<bool> UploadBinaryBlobStreamAsync(CloudBlockBlob blob,
            Stream stream)
        {
            bool bHasWrote = false;
            if (stream == null)
            {
                return false;
            }
            if (blob == null)
            {
                return false;
            }
            //upload stream using (stream)
            {
                await blob.UploadFromStreamAsync(stream);
                bHasWrote = true;
            }
            return bHasWrote;
        }
        public bool UploadBinaryBlobStream(CloudBlockBlob blob,
            Stream stream)
        {
            bool bHasWrote = false;
            if (stream == null)
            {
                return false;
            }
            if (blob == null)
            {
                return false;
            }
            //upload stream
            using (stream)
            {
                blob.UploadFromStream(stream);
                bHasWrote = true;
            }
            return bHasWrote;
        }
        public async Task StoreResourceInCloudBobByIdAsync( 
            CloudBlockBlob blob, ContentURI uri)
        {
            if (uri.URIName == Helpers.GeneralHelpers.GetResource("FILE_NONE")
                || uri.URIName == Helpers.GeneralHelpers.NONE)
            {
                //don't run the query if no resource is on hand
                return;
            }
            if (blob == null)
            {
                return;
            }
            bool bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(uri.URIName);
            if (!bIsXmlDoc)
                bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(blob.Uri.AbsoluteUri);
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
		    {
			    sqlIO.MakeInParam("@PKId",              SqlDbType.Int, 4, uri.URIId),
                sqlIO.MakeInParam("@IsXmlDoc",          SqlDbType.Bit, 1, bIsXmlDoc)
		    };
            //don't run sequential in cloud - process all of the bytes at once
            SqlDataReader oDataReader = await sqlIO.RunProcAsync( 
                "0GetResourceByResourceId",
                oPrams);
            if (oDataReader != null && (!oDataReader.IsClosed))
            {
                await SaveBitsInCloudBlobAsync(oDataReader, blob);
            }
            sqlIO.Dispose();
        }
        private async Task SaveBitsInCloudBlobAsync(SqlDataReader dataReader,
            CloudBlockBlob blob)
        {
            if (dataReader != null && (!dataReader.IsClosed))
            {
                //called locally so ok to close
                using (dataReader)
                {
                    //check whether the dataReader timed out (downloading video to localhost during debugging)
                    while (dataReader.Read())
                    {
                        string sResourceMimeType = string.Empty;
                        //sequential data has to be read in order (once an index is passed, can't return to it)
                        //get the media type
                        if (dataReader.IsDBNull(0) == false)
                        {
                            sResourceMimeType = dataReader.GetString(0);
                        }
                        //update the filename
                        if (dataReader.IsDBNull(1) == false)
                        {
                            string sFileName = dataReader.GetString(1);
                            if (!string.IsNullOrEmpty(sFileName))
                            {
                                string sOldFileName = Helpers.GeneralHelpers.GetLastSubString(blob.Name,
                                    Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
                                if (!sOldFileName.Equals(sFileName)
                                    && !string.IsNullOrEmpty(sOldFileName))
                                {
                                    string sNewBlobName = blob.Name.Replace(sOldFileName, sFileName);
                                    blob = GetBlob(sNewBlobName);
                                }
                            }
                        }
                        int iColIndex = 2;
                        if (dataReader.IsDBNull(iColIndex) == false)
                        {
                            blob.Properties.ContentType = sResourceMimeType;
                            bool bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(blob.Name);
                            if (!bIsXmlDoc)
                                bIsXmlDoc = Helpers.GeneralHelpers.IsXmlFileExt(blob.Uri.AbsoluteUri);
                            //these two methods could be wrapped in the pattern used in FileStorageIO
                            if (bIsXmlDoc)
                            {
                                SqlXml oSqlXml = dataReader.GetSqlXml(iColIndex);
                                if (oSqlXml.IsNull == false)
                                {
                                    XmlReader oReader = oSqlXml.CreateReader();
                                    Helpers.XmlFileIO oXmlFileIO = new Helpers.XmlFileIO();
                                    using (oReader)
                                    {
                                        string sURIPath = await UploadXmlReaderBlobAsync(blob, oReader);
                                    }
                                }
                            }
                            else
                            {
                                //will use try/catch to save
                                string sURI = await UploadBinaryBlobStreamAsync(blob, dataReader, iColIndex);
                            }
                        }
                    }
                }
            }
        }
        
        public async Task SaveBlobsAsync(XmlDocument doc,
           string fullURIPath, List<Stream> sourceStreams)
        {
            //do not close this stream; the closing procedure closes parallel streams
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    byte[] encodedText = Encoding.UTF8.GetBytes(doc.OuterXml);
                    if (encodedText.Length > 0)
                    {
                        //openwrite overwrites previous content (not append)
                        CloudBlobStream stream = blob.OpenWrite();
                        if (stream.CanWrite)
                        {
                            //stream.BlockSize = 4096;
                            //stream.Seek(0, SeekOrigin.End);
                            await stream.WriteAsync(encodedText, 0, encodedText.Length);
                            sourceStreams.Add(stream);
                        }
                    }
                }
            }
        }
        public async Task SaveBlobAsync(XmlDocument doc, string fullURIPath)
        {
            //do not close this stream; the closing procedure closes parallel streams
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    byte[] encodedText = Encoding.UTF8.GetBytes(doc.OuterXml);
                    if (encodedText.Length > 0)
                    {
                        //openwrite overwrites previous content (not append)
                        CloudBlobStream stream = blob.OpenWrite();
                        if (stream.CanWrite)
                        {
                            using (stream)
                            {
                                //stream.BlockSize = 4096;
                                //stream.Seek(0, SeekOrigin.End);
                                await stream.WriteAsync(encodedText, 0, encodedText.Length);
                            }
                        }
                    } 
                }
            }
        }
        
        //keep around: don't use GetBytes -doesn't reliably get all bytes see FileIO reference for SqlDataClient
        //doesn't work well in cloud (keeps retrieving small amount of bytes)
        //maybe a candidate for a worker process, but the small bytes will 
        //still be a problem
        //public bool WriteBinaryBlobFileBySequence(CloudBlockBlob blob,
        //    SqlDataReader sqlReader, int columnIndex)
        //{
        //    bool bHasWrote = false;
        //    if (blob == null)
        //    {
        //        return false;
        //    }
        //    //create the writer for data.
        //    CloudBlobStream oBlobStreamWriter = blob.OpenWrite();
        //    using (oBlobStreamWriter)
        //    {
        //        //size of the buffer.
        //        int iBufferSize = 100;
        //        //blob byte[] buffer to be filled by GetBytes.
        //        byte[] outBytes = new byte[iBufferSize];
        //        //the bytes returned from GetBytes.
        //        long lRetVal = 0;
        //        //the starting position in the BLOB output.
        //        long lStartIndex = 0;
        //        //read bytes into outByte[] and retain the number of bytes returned.
        //        lRetVal = sqlReader.GetBytes(columnIndex, lStartIndex, outBytes, 0, iBufferSize);
        //        //continue while there are bytes beyond the size of the buffer.
        //        while (lRetVal == iBufferSize
        //            && lRetVal > 0)
        //        {
        //            oBlobStreamWriter.Write(outBytes, 0, (int)lRetVal);
        //            //184: deprecated because it left out last byte
        //            //oBlobStreamWriter.Write(outBytes, 0, (int)lRetVal - 1);
        //            //oBlobStreamWriter.Flush();
        //            //reposition start index to end of last buffer and fill buffer.
        //            lStartIndex += iBufferSize;
        //            lRetVal = sqlReader.GetBytes(columnIndex, lStartIndex, outBytes, 0, iBufferSize);
        //        }
        //        if (lRetVal > 0)
        //        {
        //            //write the remaining buffer.
        //            oBlobStreamWriter.Write(outBytes, 0, (int)lRetVal);
        //            //184: deprecated because it left out last byte
        //            //oBlobStreamWriter.Write(outBytes, 0, (int)lRetVal - 1);
        //        }
        //        bHasWrote = true;
        //    }
        //    return bHasWrote;
        //}
        public void SaveCloudFile(string blobURIPath, string uploadFullFilePath,
            out string blobFullURI)
        {
            bool bHasSaved = false;
            blobFullURI = blobURIPath;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    bHasSaved = CopyFileToBlob(blob, uploadFullFilePath);
                    //System.Diagnostics.Trace.TraceInformation("Saved tempfile = '{0}' to blob storage as '{1}'", bHasSaved, blob.Name);
                    if (bHasSaved)
                    {
                        blobFullURI = blob.Uri.AbsoluteUri;
                    }
                }
            }
        }
        public async Task<bool> SaveCloudFileAsync(string blobURIPath, string uploadFullFilePath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    if (Path.IsPathRooted(uploadFullFilePath))
                    {
                        //2.0.0 debugs using localhost and packages using tempdir
                        bHasSaved = await SaveBlobInFileSystemAsync(blob, uploadFullFilePath);
                    }
                }
            }
            return bHasSaved;
        }
        public async Task<bool> SaveFileinCloudAsync(string uploadFullFilePath, string blobURIPath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    if (Path.IsPathRooted(uploadFullFilePath))
                    {
                        //2.0.0 debugs using localhost and packages using tempdir
                        bHasSaved = await CopyFileToBlobAsync(blob, uploadFullFilePath);
                    }
                }
            }
            return bHasSaved;
        }
        public void SaveCloudFileInFileSystem(string blobURIPath, string fullFilePath)
        {
            //bool bHasSaved = false;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                blob.DownloadToFile(fullFilePath, FileMode.Create);
                //or
                //// Save blob contents to a file.
                //using (var fileStream = System.IO.File.OpenWrite(fullFilePath))
                //{
                //    blob.DownloadToStream(fileStream);
                //} 
                //or test download
                //string text;
                //using (var memoryStream = new MemoryStream())
                //{
                //    blockBlob2.DownloadToStream(memoryStream);
                //    text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                //}
                //if needed for larger blobs
                //DownloadBlockBlobToFileSystem(resourcesURIPath, fullFilePath);
                //System.Diagnostics.Trace.TraceInformation("Saved blobfile = '{0}' to file storage as '{1}'", bHasSaved, blob.Name);
            }
        }
        public async Task<bool> SaveCloudFileInFileSystemAsync(string blobURIPath, string fullFilePath)
        {
            bool bHasSaved = false;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                bHasSaved = await SaveBlobInFileSystemAsync(blob, fullFilePath);
            }
            return bHasSaved;
        }
        public async Task<bool> SaveBlobInFileSystemAsync(CloudBlockBlob blob, string fullFilePath)
        {
            bool bHasSaved = false;
            if (blob != null)
            {
                await blob.DownloadToFileAsync(fullFilePath, FileMode.Create);
                bHasSaved = true;
            }
            return bHasSaved;
        }
        public async Task<string> ReadTextAsync(string blobURIPath)
        {
            string sContent = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    //v180 changed this from blob.OpenRead
                    using (Stream stream = await blob.OpenReadAsync())
                    {
                        if (stream.CanRead)
                        {
                            byte[] buffer = new byte[stream.Length];
                            //byte[] buffer = new byte[0x1000];
                            int numRead;
                            while ((numRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                            {
                                string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                                sb.Append(text);
                            }
                            sContent = sb.ToString();
                            //or
                            //byte[] buffer;
                            //UTF8Encoding utf8encode = new UTF8Encoding();
                            //buffer = new byte[stream.Length];
                            //await stream.ReadAsync(buffer, 0, (int)stream.Length);
                            //sContent = utf8encode.GetString(buffer);
                        }
                    }
                }
            }
            return sContent;
        }
        public async Task<List<string>> ReadLinesAsync(string blobURIPath)
        {
            List<string> lines = new List<string>();
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                string sFile = await ReadTextAsync(blobURIPath);
                lines = GeneralHelpers.GetLinesFromUTF8Encoding(sFile);
            }
            return lines;
        }
        public byte[] DownloadFile(string fullURIPath)
        {
            byte[] data = { };
            if (!string.IsNullOrEmpty(fullURIPath))
            {
                CloudBlockBlob blob = GetBlob(fullURIPath);
                if (blob != null)
                {
                    // Read content 
                    using (MemoryStream ms = new MemoryStream())
                    {
                        blob.DownloadToStream(ms);
                        data = ms.ToArray();
                    }
                }
            }
            return data;
        }
        
        public List<string> ReadLines(string blobURIPath)
        {
            List<string> lines = new List<string>();
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    using (Stream stream = blob.OpenRead())
                    {
                        //may also want to download whole blob and then manipulate using stream
                        //blob.DownloadToStream(stream);
                        if (stream.CanRead)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    lines.Add(reader.ReadLine());
                                }
                            }
                        }
                    }
                }
            }
            return lines;
        }
        public string SaveCloudFileInString(string blobURIPath)
        {
            string sContent = string.Empty;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob == null)
                {
                    return sContent;
                }
                sContent = blob.DownloadText();
            }
            return sContent;
        }
        public XmlReader GetXmlFromURI(string blobURIPath)
        {
            XmlReader reader = null;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    //do not use a using with the memory stream or it closes before it can be used in reader
                    //when reader closes it will dipose of the memory stream
                    var memoryStream = new MemoryStream();
                    blob.DownloadToStream(memoryStream);
                    memoryStream.Position = 0;
                    XmlReaderSettings oSettings = new XmlReaderSettings();
                    oSettings.ConformanceLevel = ConformanceLevel.Document;
                    oSettings.IgnoreWhitespace = true;
                    oSettings.IgnoreComments = true;
                    reader = XmlReader.Create(memoryStream, oSettings);
                }
            }
            return reader;
        }
        public async Task<XmlReader> GetXmlFromURIAsync(string blobURIPath)
        {
            XmlReader reader = null;
            if (!string.IsNullOrEmpty(blobURIPath))
            {
                CloudBlockBlob blob = GetBlob(blobURIPath);
                if (blob != null)
                {
                    //do not use a using with the memory stream or it closes before it can be used in reader
                    //when reader closes it will dipose of the memory stream
                    //using (var memoryStream = new MemoryStream())
                    //{
                    var memoryStream = new MemoryStream();
                    //better performance that blob.OpenRead
                    await blob.DownloadToStreamAsync(memoryStream);
                    memoryStream.Position = 0;
                    XmlReaderSettings oSettings = new XmlReaderSettings();
                    oSettings.ConformanceLevel = ConformanceLevel.Document;
                    oSettings.IgnoreWhitespace = true;
                    oSettings.IgnoreComments = true;
                    //this reader can now be read asynch
                    oSettings.Async = true;
                    reader = XmlReader.Create(memoryStream, oSettings);
                    //can also use the bytes
                    //text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            return reader;
        }
        public void SaveCloudTempFile(string tempURIPath, string fullFilePath,
            out string blobFullURI)
        {
            bool bHasSaved = false;
            blobFullURI = string.Empty;
            if (!string.IsNullOrEmpty(tempURIPath))
            {
                CloudBlockBlob blob = GetBlob(tempURIPath);
                if (blob == null)
                {
                    return;
                }
                bHasSaved = CopyFileToBlob(blob, fullFilePath);
                if (bHasSaved)
                {
                    blobFullURI = blob.Uri.AbsoluteUri;
                }
            }
        }
        public bool DeleteBlob(string fullURIPath)
        {
            bool bIsDeleted = false;
            CloudBlockBlob blob = GetBlob(fullURIPath);
            if (blob != null)
            {
                blob.Delete();
                bIsDeleted = true;
            }
            return bIsDeleted;
        }
        public bool DeleteDirectory(ContentURI uri, string fullURIPath, bool includeSubDirs)
        {
            bool bDirsAreDeleted = false;
            bool bIsFlat = (includeSubDirs) ? false : true;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(fullURIPath, bIsFlat);
            if (blobs != null)
            {
                bDirsAreDeleted = DeleteDirectories(uri, blobs, includeSubDirs);
            }
            return bDirsAreDeleted;
        }
        private bool DeleteDirectories(ContentURI uri, IEnumerable<IListBlobItem> blobs,
            bool includeSubDirs)
        {
            bool bDirsAreDeleted = false;
            if (blobs != null)
            {
                AzureIOAsync azureIO = new AzureIOAsync(uri);
                foreach (var blobOrBlobDir in blobs)
                {
                    if (Path.HasExtension(blobOrBlobDir.Uri.AbsoluteUri))
                    {
                        bDirsAreDeleted = DeleteBlob(blobOrBlobDir.Uri.AbsoluteUri);
                    }
                    else
                    {
                        if (includeSubDirs)
                        {
                            bDirsAreDeleted = DeleteDirectory(uri, blobOrBlobDir.Uri.AbsoluteUri, includeSubDirs);
                        }
                    }
                }
            }
            return bDirsAreDeleted;
        }
        public IEnumerable<IListBlobItem> GetChildDirectories(string directoryPath)
        {
            IEnumerable<IListBlobItem> dirs = null;
            bool bIsFlat = false;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(directoryPath, bIsFlat);
            if (blobs != null)
            {
                //directories end with web path delimiters
                dirs = blobs;
                //blobs.Where(b => (b.Uri.AbsoluteUri.EndsWith(GeneralHelpers.WEBFILE_PATH_DELIMITER)));
            }
            return dirs;
        }
        public string GetDescendentDirectory(string directoryPath, string directoryPattern)
        {
            string sDescDir = string.Empty;
            bool bIsFlat = false;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(directoryPath, bIsFlat);
            if (blobs != null)
            {
                sDescDir = GetDescendentDirectory(blobs, directoryPath, directoryPattern);
            }
            return sDescDir;
        }
        private string GetDescendentDirectory(IEnumerable<IListBlobItem> blobs,
            string startDirectoryPath, string directoryPattern)
        {
            string sDescDir = string.Empty;
            if (blobs != null)
            {
                foreach (var blobOrBlobDir in blobs)
                {
                    //must only look through directories
                    if (!Path.HasExtension(blobOrBlobDir.Uri.AbsoluteUri))
                    {
                        string sDelimiter = FileStorageIO.GetDelimiterForFileStorage(blobOrBlobDir.Uri.AbsoluteUri);
                        string sDirToCheck = directoryPattern;
                        if (!directoryPattern.EndsWith(sDelimiter))
                        {
                            //get rid of last delimiter
                            sDirToCheck = string.Concat(directoryPattern, sDelimiter);
                        }
                        if (blobOrBlobDir.Uri.AbsoluteUri.EndsWith(sDirToCheck))
                        {
                            sDescDir = blobOrBlobDir.Uri.AbsoluteUri;
                            break;
                        }
                        else
                        {
                            sDescDir = GetDescendentDirectory(blobOrBlobDir.Uri.AbsoluteUri, directoryPattern);
                        }
                    }
                }
            }
            return sDescDir;
        }
        public void GetTerminalNodeAzureFiles(
            ContentURI uri, IList<string> urisToAnalyze,
            string docToCalcURIPattern, string docToCalcPath, string fileExtension,
            ref int i, ref IDictionary<string, string> fileOrFolderPaths)
        {
            string sDirectory = FileStorageIO.GetDirectoryName(docToCalcPath);
            string sDirectoryName = Helpers.GeneralHelpers.GetSubstringFromEnd(sDirectory,
                GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
            if (GeneralHelpers.IsTerminalFolder(sDirectoryName))
            {
                //verify this folder exists in db
                if (urisToAnalyze.Contains(sDirectoryName))
                {
                    //188 doesn't require new calcs for inputs or outputs
                    if (fileExtension.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                        || fileExtension.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
                    {
                        AddNewestIOFile(uri, sDirectory, ref i, ref fileOrFolderPaths);
                    }
                    else
                    {
                        int z = fileOrFolderPaths.Count;
                        AddNewestFileWithFileExtension(uri, sDirectory, fileExtension,
                             ref i, ref fileOrFolderPaths);
                        if (z == fileOrFolderPaths.Count)
                        {
                            //188 allows default uploaded files to be used directly w/o running NPV calculation
                            AddNewestFileWithFileExtension(uri, sDirectory,
                                DevTreks.Data.Helpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString(),
                                ref i, ref fileOrFolderPaths);
                        }
                    }
                }
                IEnumerable<IListBlobItem> dirs = GetChildDirectories(docToCalcPath);
                if (dirs != null)
                {
                    foreach (var folder in dirs)
                    {
                        if (!Path.HasExtension(folder.Uri.AbsoluteUri))
                        {
                            string sSubDirectory = FileStorageIO.GetDirectoryName(folder.Uri.AbsoluteUri);
                            string sSubDirectoryName = Helpers.GeneralHelpers.GetSubstringFromEnd(sSubDirectory,
                                GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
                            if (GeneralHelpers.IsTerminalFolder(sSubDirectoryName))
                            {
                                //recursion will catch the files in this folder
                                //devpacks are recursive
                                if (sSubDirectoryName.StartsWith(
                                    AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString()))
                                {
                                    //recurse
                                    GetTerminalNodeAzureFiles(uri,
                                        urisToAnalyze, docToCalcURIPattern,
                                        folder.Uri.AbsoluteUri, fileExtension,
                                        ref i, ref fileOrFolderPaths);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                IEnumerable<IListBlobItem> dirs = GetChildDirectories(docToCalcPath);
                if (dirs != null)
                {
                    foreach (var folder in dirs)
                    {
                        if (!Path.HasExtension(folder.Uri.AbsoluteUri))
                        {
                            string sSubDirectory = FileStorageIO.GetDirectoryName(folder.Uri.AbsoluteUri);
                            string sSubDirectoryName = Helpers.GeneralHelpers.GetSubstringFromEnd(sSubDirectory,
                                GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
                            if (GeneralHelpers.IsTerminalFolder(sSubDirectoryName))
                            {
                                string sNodeName
                                    = ContentURI.GetURIPatternPart(docToCalcURIPattern, ContentURI.URIPATTERNPART.node);
                                if (!sNodeName.StartsWith(AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString()))
                                {
                                    //verify this folder exists in db
                                    if (urisToAnalyze.Contains(sSubDirectoryName))
                                    {
                                        //188 doesn't require new calcs for inputs or outputs
                                        if (fileExtension.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                                            || fileExtension.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
                                        {
                                            AddNewestIOFile(uri, 
                                                folder.Uri.AbsoluteUri, ref i, ref fileOrFolderPaths);
                                        }
                                        else
                                        {
                                            int z = fileOrFolderPaths.Count;
                                            AddNewestFileWithFileExtension(uri,
                                                folder.Uri.AbsoluteUri, fileExtension,
                                                ref i, ref fileOrFolderPaths);
                                            if (z == fileOrFolderPaths.Count)
                                            {
                                                //188 allows default uploaded files to be used directly w/o running NPV calculation
                                                AddNewestFileWithFileExtension(uri,
                                                    folder.Uri.AbsoluteUri,
                                                    DevTreks.Data.Helpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString(),
                                                    ref i, ref fileOrFolderPaths);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //devpacks are recursive
                                    GetTerminalNodeAzureFiles(uri, urisToAnalyze, docToCalcURIPattern,
                                        folder.Uri.AbsoluteUri, fileExtension,
                                        ref i, ref fileOrFolderPaths);
                                }
                            }
                            else
                            {
                                //recurse
                                GetTerminalNodeAzureFiles(uri, urisToAnalyze, docToCalcURIPattern,
                                    folder.Uri.AbsoluteUri, fileExtension,
                                    ref i, ref fileOrFolderPaths);
                            }
                        }
                    }
                }
            }
        }
        public void AddNewestFileWithFileExtension(
            ContentURI uri, string dirURIPath,
            string fileExtension, ref int i, ref IDictionary<string, string> lstFilePaths)
        {
            bool bIsFlat = false;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(dirURIPath, bIsFlat);
            CloudBlockBlob newestBlob = null;
            if (blobs != null)
            {
                bool bIsNewerFile = false;
                foreach (var blob in blobs)
                {
                    if (Path.GetFileNameWithoutExtension(blob.Uri.AbsoluteUri).EndsWith(fileExtension)
                        && Path.GetExtension(blob.Uri.AbsoluteUri) == Helpers.GeneralHelpers.EXTENSION_XML)
                    {
                        //rule 1: analyzers can only use calculator data
                        if (Path.GetFileName(blob.Uri.AbsoluteUri).StartsWith(GeneralHelpers.ADDIN))
                        {
                            //rule2: use only the latest file calculated with that fileextension
                            //if this folder had more than one file with this extension, 
                            //it could mean that an old calculation, 
                            //from a previous calculator, was not deleted properly
                            i++;
                            if (newestBlob != null)
                            {
                                bIsNewerFile
                                    = FileStorageIO.File1IsNewer(
                                        uri, blob.Uri.AbsoluteUri, newestBlob.Uri.AbsoluteUri);
                                if (bIsNewerFile)
                                {
                                    newestBlob = (CloudBlockBlob)blob;
                                }
                            }
                            else
                            {
                                newestBlob = (CloudBlockBlob)blob;
                            }
                        }
                    }
                }
                if (newestBlob != null)
                {
                    AddBlobToList(newestBlob, i, ref lstFilePaths);
                }
            }
        }
        public void AddNewestIOFile(
            ContentURI uri, string dirURIPath,
            ref int i, ref IDictionary<string, string> lstFilePaths)
        {
            bool bIsFlat = false;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(dirURIPath, bIsFlat);
            CloudBlockBlob newestBlob = null;
            if (blobs != null)
            {
                bool bIsNewerFile = false;
                foreach (var blob in blobs)
                {
                    if (Path.GetExtension(blob.Uri.AbsoluteUri) == Helpers.GeneralHelpers.EXTENSION_XML)
                    {
                        //rule2: use only the latest file calculated with that fileextension
                        //if this folder had more than one file with this extension, 
                        //it could mean that an old calculation, 
                        //from a previous calculator, was not deleted properly
                        i++;
                        if (newestBlob != null)
                        {
                            bIsNewerFile
                                = FileStorageIO.File1IsNewer(uri,
                                    blob.Uri.AbsoluteUri, newestBlob.Uri.AbsoluteUri);
                            if (bIsNewerFile)
                            {
                                newestBlob = (CloudBlockBlob)blob;
                            }
                        }
                        else
                        {
                            newestBlob = (CloudBlockBlob)blob;
                        }
                    }
                }
                if (newestBlob != null)
                {
                    AddBlobToList(newestBlob, i, ref lstFilePaths);
                }
            }
        }
        private void AddBlobToList(CloudBlockBlob blob,
            int i, ref IDictionary<string, string> lstFilePaths)
        {
            if (lstFilePaths.ContainsKey(i.ToString()) == false
                && blob != null)
            {
                lstFilePaths.Add(i.ToString(), blob.Uri.AbsoluteUri);
            }
        }
        public void GetFirstSubFolderAzureFiles(
            ContentURI uri, IList<string> urisToAnalyze,
            string docToCalcPath, string fileExtension,
            ref IDictionary<string, string> fileOrFolderPaths)
        {
            int i = 0;
            IEnumerable<IListBlobItem> dirs = GetChildDirectories(docToCalcPath);
            if (dirs != null)
            {
                foreach (var folder in dirs)
                {
                    string sSubDirectory = FileStorageIO.GetDirectoryName(folder.Uri.AbsoluteUri);
                    string sSubDirectoryName = Helpers.GeneralHelpers.GetSubstringFromEnd(sSubDirectory,
                        GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
                    //verify this folder exists in db
                    if (urisToAnalyze.Contains(sSubDirectoryName))
                    {
                        //188 doesn't require new calcs for inputs or outputs
                        if (fileExtension.StartsWith(AppHelpers.Prices.INPUT_PRICE_TYPES.input.ToString())
                            || fileExtension.StartsWith(AppHelpers.Prices.OUTPUT_PRICE_TYPES.output.ToString()))
                        {
                            AddNewestIOFile(uri, 
                                folder.Uri.AbsoluteUri, ref i, ref fileOrFolderPaths);
                        }
                        else
                        {
                            AddNewestFileWithFileExtension(uri, 
                                folder.Uri.AbsoluteUri, fileExtension,
                                ref i, ref fileOrFolderPaths);
                        }
                    }
                }
            }
        }
        public void DeleteOldAddInAzureFiles(ContentURI uri,
            string docToCalcPath, string URIdeletedURIPattern)
        {
            string sURIdeletedId = ContentURI.GetURIPatternPart(
                URIdeletedURIPattern, ContentURI.URIPATTERNPART.id);
            string sFileToDeleteId
                = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                sURIdeletedId, Helpers.GeneralHelpers.FILENAME_DELIMITER);
            List<string> filesToDelete = new List<string>();
            bool bIsFlat = true;
            IEnumerable<IListBlobItem> blobs = ListBlobs(docToCalcPath, bIsFlat);
            if (blobs != null)
            {
                foreach (var blob in blobs)
                {
                    if (!blob.Uri.AbsoluteUri.Equals(docToCalcPath))
                    {
                        if (Path.HasExtension(blob.Uri.AbsoluteUri))
                        {
                            if (Path.GetFileName(blob.Uri.AbsoluteUri).Contains(sFileToDeleteId))
                            {
                                filesToDelete.Add(blob.Uri.AbsoluteUri);
                            }
                        }
                    }
                }
            }
            foreach (string filetodelete in filesToDelete)
            {
                FileStorageIO.DeleteURI(uri, filetodelete);
            }
        }

        public void DeleteOldAddInHtmlAzureFiles(ContentURI uri,
            string docToCalcPath, string URIdeletedURIPattern, bool includeCalcDocs)
        {
            string sURIdeletedId = ContentURI.GetURIPatternPart(
                URIdeletedURIPattern, ContentURI.URIPATTERNPART.id);
            string sFileToDeleteId
                = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                sURIdeletedId, Helpers.GeneralHelpers.FILENAME_DELIMITER);
            List<string> filesToDelete = new List<string>();
            bool bIsFlat = true;
            IEnumerable<IListBlobItem> blobs = ListBlobs(docToCalcPath, bIsFlat);
            if (blobs != null)
            {
                foreach (var blob in blobs)
                {
                    if (Path.GetFileName(blob.Uri.AbsoluteUri).Contains(sFileToDeleteId))
                    {
                        if (Path.GetFileName(blob.Uri.AbsoluteUri).EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.frag.ToString())
                            || Path.GetFileName(blob.Uri.AbsoluteUri).EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.html.ToString()))
                        {
                            if (includeCalcDocs == false)
                            {
                                string sAddIn = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                                    Data.AppHelpers.AddIns.ADDIN_TYPES.addin.ToString());
                                if (!Path.GetFileName(blob.Uri.AbsoluteUri).StartsWith(sAddIn))
                                {
                                    filesToDelete.Add(blob.Uri.AbsoluteUri);
                                }
                            }
                            else
                            {
                                filesToDelete.Add(blob.Uri.AbsoluteUri);
                            }

                        }
                    }
                }
            }
            foreach (string filetodelete in filesToDelete)
            {
                FileStorageIO.DeleteURI(uri, filetodelete);
            }
        }
        
        public IEnumerable<IListBlobItem> ListBlobs(string uriPath, bool isFlat)
        {
            IEnumerable<IListBlobItem> blobs = null;
            string sRelativeBlobPath = GetRelativeBlobPathFromFullPath(uriPath);
            string sDirectory = GetDirectoryName(sRelativeBlobPath);
            CloudBlobClient blobClient = GetBlobClient();
            CloudBlobContainer container = GetContainer(uriPath);
            if (container != null)
            {
                CloudBlobDirectory blobDir = container.GetDirectoryReference(sDirectory);
                if (blobDir == null)
                {
                    return blobs;
                }
                if (isFlat)
                {
                    //List blobs in this blob directory using a flat listing.
                    //BlobRequestOptions options = new BlobRequestOptions();
                    //options.UseFlatBlobListing = true;
                    //blobs = blobDir.ListBlobs(options);
                    bool bUseFlatListing = true;
                    BlobRequestOptions options = new BlobRequestOptions();
                    blobs = blobDir.ListBlobs(bUseFlatListing, BlobListingDetails.None, options);
                }
                else
                {
                    blobs = blobDir.ListBlobs();
                }
            }
            return blobs;
        }
        public static string GetRelativeBlobPathFromFullPath(string uriPath)
        {
            string sContainerName = GetContainerNameFull(uriPath);
            if (string.IsNullOrEmpty(sContainerName))
            {
                sContainerName = GetContainerNameRelative(uriPath);
            }
            string sRelBlobName = GetBlobRelativeName(sContainerName, uriPath);
            return sRelBlobName;
        }
        public static string GetDirectoryName(string blobPath)
        {
            string sFileName = Path.GetFileName(blobPath);
            string sDirectory = blobPath;
            if (!string.IsNullOrEmpty(sFileName))
            {
                sDirectory = blobPath.Replace(sFileName, string.Empty);
            }
            return sDirectory;
        }
        
        public void CopyDirectories(ContentURI uri,
            string fromDirectory, string toDirectory, 
            bool copySubDirs, bool needsNewSubDirectories)
        {
            bool bIsFlat = (copySubDirs) ? false : true;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(fromDirectory, bIsFlat);
            if (blobs != null)
            {
                CopyDirectories(uri, blobs, toDirectory, copySubDirs, needsNewSubDirectories);
            }
        }
        public void CopyDirectories(ContentURI uri,
            IEnumerable<IListBlobItem> blobs, string toDirectory, 
            bool copySubDirs, bool needsNewSubDirectories)
        {
            if (blobs != null)
            {
                //toDirectory has to end in a delimited file path or gets the wrong directory
                string sToDirectory = FileStorageIO.GetDirectoryName(toDirectory);
                FileStorageIO.DirectoryCreate(uri, sToDirectory);
                string sFullFilePath = string.Empty;
                foreach (var blobOrBlobDir in blobs)
                {
                    if (Path.HasExtension(blobOrBlobDir.Uri.AbsoluteUri))
                    {
                        //create the path to the new copy of the file.
                        sFullFilePath = Path.Combine(sToDirectory,
                            Path.GetFileName(blobOrBlobDir.Uri.AbsoluteUri));
                        //copy the file.
                        if (!FileStorageIO.URIAbsoluteExists(uri, 
                            sFullFilePath))
                        {
                            FileStorageIO.CopyURIs(uri,
                                blobOrBlobDir.Uri.AbsoluteUri, sFullFilePath);
                        }
                    }
                    else
                    {
                        if (copySubDirs)
                        {
                            string sFullDirectoryPath = sToDirectory;
                            //packages generally don't want subdirectories because of 
                            //need for consistent relpath to associated resources
                            if (needsNewSubDirectories)
                            {
                                //add the subdirectory to the directory.
                                sFullDirectoryPath = FileStorageIO.AddDirectoryToDirectoryPath(
                                    blobOrBlobDir.Uri.AbsoluteUri, sFullDirectoryPath);
                            }
                            //copy the subdirectories.
                            CopyDirectories(uri, blobOrBlobDir.Uri.AbsoluteUri, sFullDirectoryPath,
                                copySubDirs, needsNewSubDirectories);
                        }
                    }
                }
            }
        }
        public async Task<bool> CopyDirectoriesAsync(
            ContentURI uri, string fromDirectory,
            string toDirectory, bool copySubDirs, bool needsNewSubDirectories)
        {
            bool bHasCopied = false;
            bool bIsFlat = (copySubDirs) ? false : true;
            //get the blobs in from directory and the subdirs
            IEnumerable<IListBlobItem> blobs = ListBlobs(fromDirectory, bIsFlat);
            if (blobs != null)
            {
              bHasCopied = await CopyDirectoriesAsync(uri,
                  blobs, toDirectory, copySubDirs, needsNewSubDirectories);
            }
            return bHasCopied;
        }
        public async Task<bool> CopyDirectoriesAsync(
            ContentURI uri, IEnumerable<IListBlobItem> blobs,
            string toDirectory, bool copySubDirs, bool needsNewSubDirectories)
        {
            bool bHasCopied = false;
            if (blobs != null)
            {
                //toDirectory has to end in a delimited file path or gets the wrong directory
                //recursion will keep cutting off the last dir without this condition
                string sToDirectory = toDirectory;
                if (Path.HasExtension(toDirectory))
                {
                    //get the dir from the file path
                    sToDirectory = FileStorageIO.GetDirectoryName(toDirectory);
                }
                FileStorageIO.DirectoryCreate(uri, sToDirectory);
                string sFullFilePath = string.Empty;
                foreach (var blobOrBlobDir in blobs)
                {
                    if (Path.HasExtension(blobOrBlobDir.Uri.AbsoluteUri))
                    {
                        //create the path to the new copy of the file.
                        sFullFilePath = Path.Combine(sToDirectory,
                            Path.GetFileName(blobOrBlobDir.Uri.AbsoluteUri));
                        //copy the file.
                        if (!FileStorageIO.URIAbsoluteExists(uri,
                            sFullFilePath))
                        {
                            bHasCopied 
                                = await FileStorageIO.CopyURIsAsync(
                                    uri, blobOrBlobDir.Uri.AbsoluteUri, sFullFilePath);
                        }
                    }
                    else
                    {
                        if (copySubDirs)
                        {
                            string sFullDirectoryPath = sToDirectory;
                            //packages generally don't want subdirectories because of 
                            //need for consistent relpath to associated resources
                            if (needsNewSubDirectories)
                            {
                                //add the subdirectory to the directory.
                                sFullDirectoryPath = FileStorageIO.AddDirectoryToDirectoryPath(
                                    blobOrBlobDir.Uri.AbsoluteUri, sFullDirectoryPath);
                            }
                            //copy the subdirectories.
                            bHasCopied = await CopyDirectoriesAsync(
                                uri, blobOrBlobDir.Uri.AbsoluteUri, sFullDirectoryPath,
                                copySubDirs, needsNewSubDirectories);
                        }
                    }
                }
            }
            return bHasCopied;
        }
        public void StartCopy(CloudStorageAccount srcAccount, string srcContainerName, string srcBlobName,
            CloudStorageAccount destAccount, string destContainerName, string destBlobName)
        {
            var srcClient = srcAccount.CreateCloudBlobClient();
            var srcContainer = srcClient.GetContainerReference(srcContainerName);
            var srcBlob = srcContainer.GetPageBlobReference(srcBlobName);


            var destClient = destAccount.CreateCloudBlobClient();
            var destContainer = destClient.GetContainerReference(destContainerName);
            var destBlob = destContainer.GetPageBlobReference(destBlobName);

            var readPolicy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(1)
            };
            var srcUri = string.Format("{0}{1}", srcBlob.Uri.AbsoluteUri, srcBlob.GetSharedAccessSignature(readPolicy));
            
            //2.0.0
            string sResult = destBlob.StartCopy(new Uri(srcUri), null, null);
            //destBlob.StartCopyFromBlob(new Uri(srcUri));
            // destBlob.StartCopyFromBlob(srcBlob);
        }
        //160 deprecated for async
        public bool CopyRelatedDataToCloudServerPackage(
            ContentURI uri, string uriPath,
            string packageName, string fileType, string newFilePath,
            bool needsAllRelatedData, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            bool bIsFlat = false;
            IEnumerable<IListBlobItem> blobs = ListBlobs(uriPath, bIsFlat);
            if (blobs != null)
            {
                //filter the blobs by fileType (i.e. .xml) or directory name only blobs
                IEnumerable<IListBlobItem> fileTypeBlobs = blobs.Where(b =>
                    (Path.GetExtension(b.Uri.AbsoluteUri) == fileType
                    || Path.GetFileName(b.Uri.AbsoluteUri) == string.Empty));
                if (fileTypeBlobs != null)
                {
                    FileStorageIO.DirectoryCreate(uri, newFilePath);
                    //add siblings for everything
                    AddSiblingFiles(uri, fileTypeBlobs, uriPath, packageName,
                        newFilePath, zipArgs);
                    bHasCopied = true;
                    if (needsAllRelatedData)
                    {
                        //copy children subfolder data 
                        //html subdirectories get put in root path so they can find resources
                        bool bNeedsNewSubDirectories =
                            (fileType.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xml.ToString()))
                            ? true : false;
                        bool bCopySubDirectories = true;
                        //note azure uses filtered files while the web server copies everything
                        CopyDirectories(uri, fileTypeBlobs, newFilePath, bCopySubDirectories, bNeedsNewSubDirectories);
                        //add them to zipargs
                        PackageIO.AddChildrenFilesToZipArgsAsync(newFilePath, packageName, fileType, zipArgs);
                    }
                }
            }
            return bHasCopied;
        }
        //160 deprecated for async
        private void AddSiblingFiles(ContentURI uri,
            IEnumerable<IListBlobItem> blobs, string currentFilePath, 
            string packageName, string newFilePath,
            IDictionary<string, string> zipArgs)
        {
            string sPackageFilePath = string.Empty;
            if (blobs != null)
            {
                string sFileExtension = string.Empty;
                //toDirectory has to end in a delimited file path or gets the wrong directory
                string sToDirectory = FileStorageIO.GetDirectoryName(newFilePath);
                foreach (var blob in blobs)
                {
                    //current file is already in the package
                    if (!blob.Uri.AbsoluteUri.Contains(currentFilePath))
                    {
                        if (Path.HasExtension(blob.Uri.AbsoluteUri))
                        {
                            sPackageFilePath = Path.Combine(sToDirectory, Path.GetFileName(blob.Uri.AbsoluteUri));
                            FileStorageIO.CopyURIs(uri, blob.Uri.AbsoluteUri, sPackageFilePath);
                            if (Helpers.FileStorageIO.URIAbsoluteExists(
                                uri, sPackageFilePath))
                            {
                                if (!zipArgs.ContainsKey(sPackageFilePath))
                                {
                                    zipArgs.Add(sPackageFilePath, packageName);
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task<bool> CopyRelatedDataToCloudServerPackageAsync(
            ContentURI uri, string uriPath,
            string packageName, string fileType, string newFilePath,
            bool needsAllRelatedData, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            bool bIsFlat = false;
            IEnumerable<IListBlobItem> blobs = ListBlobs(uriPath, bIsFlat);
            if (blobs != null)
            {
                string sFileType = fileType;
                if (fileType == Helpers.GeneralHelpers.EXTENSION_CSV)
                {
                    //xml files are used to build text files but text is not stored statefully
                    sFileType = Helpers.GeneralHelpers.EXTENSION_XML;
                }
                //filter the blobs by fileType (i.e. .xml) or directory name only blobs
                IEnumerable<IListBlobItem> fileTypeBlobs = blobs.Where(b =>
                    (Path.GetExtension(b.Uri.AbsoluteUri) == sFileType
                    || Path.GetFileName(b.Uri.AbsoluteUri) == string.Empty));
                if (fileTypeBlobs != null)
                {
                    FileStorageIO.DirectoryCreate(uri, newFilePath);
                    //add siblings for everything
                    bHasCopied = await AddSiblingFilesAsync(
                        uri, fileTypeBlobs, uriPath, packageName, fileType,
                        newFilePath, zipArgs);
                    bHasCopied = true;
                    if (needsAllRelatedData)
                    {
                        //copy children subfolder data 
                        //html subdirectories get put in root path so they can find resources
                        bool bNeedsNewSubDirectories =
                            (fileType.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xml.ToString()))
                            ? true : false;
                        bool bCopySubDirectories = true;
                        //note azure uses filtered files while the web server copies everything
                        await CopyDirectoriesAsync(uri, 
                            fileTypeBlobs, newFilePath, bCopySubDirectories, bNeedsNewSubDirectories);
                        //add them to zipargs
                        await PackageIO.AddChildrenFilesToZipArgsAsync(newFilePath, packageName, fileType, zipArgs);
                    }
                }
            }
            return bHasCopied;
        }
        private async Task<bool> AddSiblingFilesAsync(
            ContentURI uri, IEnumerable<IListBlobItem> blobs,
            string currentFilePath, string packageName, string fileType, string newFilePath,
            IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            string sPackageFilePath = string.Empty;
            if (blobs != null)
            {
                string sFileExtension = string.Empty;
                //toDirectory has to end in a delimited file path or gets the wrong directory
                string sToDirectory = FileStorageIO.GetDirectoryName(newFilePath);
                string sNoCalcDocExt = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
                    Helpers.GeneralHelpers.FILENAME_EXTENSIONS.addin.ToString());
                foreach (var blob in blobs)
                {
                    if (fileType == Helpers.GeneralHelpers.EXTENSION_CSV)
                    {
                        bHasCopied = AddSiblingCSVFiles(uri,
                            blob, currentFilePath, sNoCalcDocExt,
                            packageName, fileType, sToDirectory, zipArgs);
                    }
                    else
                    {
                        //current file is already in the package
                        if (!blob.Uri.AbsoluteUri.Contains(currentFilePath))
                        {
                            if (Path.HasExtension(blob.Uri.AbsoluteUri))
                            {
                                //v174 exclude calcdocs because too many unneeded docs in package
                                if (!blob.Uri.AbsoluteUri.Contains(sNoCalcDocExt))
                                {
                                    sPackageFilePath = Path.Combine(sToDirectory, Path.GetFileName(blob.Uri.AbsoluteUri));
                                    bHasCopied = await FileStorageIO.CopyURIsAsync(
                                        uri, blob.Uri.AbsoluteUri, sPackageFilePath);
                                    if (Helpers.FileStorageIO.URIAbsoluteExists(
                                        uri, sPackageFilePath))
                                    {
                                        if (!zipArgs.ContainsKey(sPackageFilePath))
                                        {
                                            zipArgs.Add(sPackageFilePath, packageName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bHasCopied;
        }
        private bool AddSiblingCSVFiles(ContentURI uri, IListBlobItem blob,
            string currentFilePath, string noCalcDocExt, string packageName, string fileType,
            string toDirectory, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            string sPackageFilePath = string.Empty;
            //convert xml files
            if (Path.HasExtension(blob.Uri.AbsoluteUri))
            {
                //v174 exclude calcdocs because too many unneeded docs in package
                if (!blob.Uri.AbsoluteUri.Contains(noCalcDocExt))
                {
                    //to csv
                    string sCSVFileName = Path.GetFileName(blob.Uri.AbsoluteUri).Replace(Helpers.GeneralHelpers.EXTENSION_XML,
                        Helpers.GeneralHelpers.EXTENSION_CSV);
                    sPackageFilePath = Path.Combine(toDirectory, sCSVFileName);
                    ContentURI docToCalcURI = new ContentURI(uri);
                    docToCalcURI.URIDataManager.TempDocPath = sPackageFilePath;
                    ObservationTextBuilder csvBuilder = new ObservationTextBuilder();
                    IDictionary<string, string> fileOrFolderPaths = new Dictionary<string, string>();
                    fileOrFolderPaths.Add("1", blob.Uri.AbsoluteUri);
                    bHasCopied = csvBuilder.StreamAndSaveObservation(docToCalcURI, fileOrFolderPaths);
                    if (Helpers.FileStorageIO.URIAbsoluteExists(
                        uri, sPackageFilePath))
                    {
                        if (!zipArgs.ContainsKey(sPackageFilePath))
                        {
                            zipArgs.Add(sPackageFilePath, packageName);
                        }
                    }
                    if (!string.IsNullOrEmpty(docToCalcURI.ErrorMessage))
                    {
                        uri.ErrorMessage += docToCalcURI.ErrorMessage;
                    }
                }
            }
            return bHasCopied;
        }
        //azure machine learning R project
        public async Task<string> InvokeHttpRequestResponseService(ContentURI uri,
            string baseURL, string apiKey, string inputBlobLocation, 
            string outputBlobLocation, string script)
        {
            string sResponse = string.Empty;
            string BaseUrl = baseURL;
            string InputFileLocation = inputBlobLocation; 
            string OutputFileLocation = outputBlobLocation; 
            string APIKey = apiKey;
            //2.0.0 refactor: get the connection string from AzureIO (get connected service)
            string sStorageConnectionString = uri.URIDataManager.StorageConnection;
            // set a time out for polling status
            const int TimeOutInMilliseconds = 120 * 1000; // Set a timeout of 2 minutes

            using (HttpClient client = new HttpClient())
            {
                BatchScoreRequest request = new BatchScoreRequest()
                {
                    Input = new AzureBlobDataReference()
                    {
                        ConnectionString = sStorageConnectionString,
                        RelativeLocation = InputFileLocation
                    },
                    GlobalParameters = new Dictionary<string, string>() {
                        { "inputblobpath", InputFileLocation },
                        { "outputcsvblobpath", OutputFileLocation },
                        { "script1", script },
                     }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIKey);
                
                var response = await client.PostAsJsonAsync(BaseUrl, request).ConfigureAwait(false);
                string jobId = await response.Content.ReadAsAsync<string>().ConfigureAwait(false);
                //2.0.0 tests
                //var postContent = new Dictionary<string, string>() {
                //        { "inputblobpath", InputFileLocation },
                //        { "outputcsvblobpath", OutputFileLocation },
                //        { "script1", script },
                //    };
                //var content = new FormUrlEncodedContent(postContent);
                //var response = await client.PostAsync(BaseUrl, content).ConfigureAwait(false);
                //string jobId = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                string jobLocation = BaseUrl + "/" + jobId + "?api-version=2.0";
                Stopwatch watch = Stopwatch.StartNew();
                bool done = false;
                while (!done)
                {
                    response = await client.GetAsync(jobLocation).ConfigureAwait(false);
                    BatchScoreStatus status = await response.Content.ReadAsAsync<BatchScoreStatus>().ConfigureAwait(false);
                    //2.0.0 tests
                    //BatchScoreStatus status = await response.Content.ReadAsAsync<BatchScoreStatus>().ConfigureAwait(false);
                    ////var statusString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    ////bug introduced in order to compile
                    ////BatchScoreStatus status = new BatchScoreStatus();
                    //status.StatusCode = statusString;
                    if (watch.ElapsedMilliseconds > TimeOutInMilliseconds)
                    {
                        done = true;
                        //sbResponse.AppendLine("Timed out. Deleting the job ...");
                        sResponse = "Timed out. Deleting the job ...";
                        await client.DeleteAsync(jobLocation).ConfigureAwait(false); ;
                    }
                    switch (status.StatusCode)
                    {
                        case BatchScoreStatusCode.NotStarted:
                            //sbResponse.AppendLine("Not started...");
                            break;
                        case BatchScoreStatusCode.Running:
                            //sbResponse.AppendLine("Running...");
                            break;
                        case BatchScoreStatusCode.Failed:
                            //sbResponse.AppendLine("Failed!");
                            //sbResponse.AppendLine(string.Format("Error details: {0}", status.Details));
                            sResponse = string.Format("Error details: {0}", status.Details);
                            done = true;
                            break;
                        case BatchScoreStatusCode.Cancelled:
                            //sbResponse.AppendLine("Cancelled!");
                            sResponse = "Cancelled";
                            done = true;
                            break;
                        case BatchScoreStatusCode.Finished:
                            done = true;
                            //The word success tells calling procedure that output file can be processed
                            sResponse = string.Concat("Success with status code: ", response.StatusCode);
                            break;
                    }
                    if (!done)
                    {
                        Thread.Sleep(1000); // Wait one second
                    }
                }
            }
            return sResponse;
        }
        //azure machine learning algos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="apiKey"></param>
        /// <param name="inputBlob1Location">dataset to train model</param>
        /// <param name="inputBlob2Location">dataset to score</param>
        /// <param name="outputBlob1Location">score results</param>
        /// <param name="outputBlob2Location">model results</param>
        /// <param name="script"></param>
        /// <returns></returns>
        public async Task<string> InvokeHttpRequestResponseService2(string baseURL, string apiKey,
            string inputBlob1Location, string inputBlob2Location,
            string outputBlob1Location, string outputBlob2Location)
        {
            string sResponse = string.Empty;
            string BaseUrl = baseURL;
            //string InputFile1Location = inputBlob1Location;
            //string OutputFile1Location = outputBlob1Location;
            string APIKey = apiKey;

            // set a time out for polling status
            const int TimeOutInMilliseconds = 120 * 1000; // Set a timeout of 2 minutes

            using (HttpClient client = new HttpClient())
            {
                BatchExecutionRequest request = new BatchExecutionRequest()
                {
                    GlobalParameters = new Dictionary<string, string>() {
                    { "inputdata2", inputBlob2Location },
                    { "outputdata2", outputBlob2Location },
                    { "outputdata1", outputBlob1Location },
                    { "inputdata1", inputBlob1Location },
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIKey);

                //2.0.0 changes
                //var response = await client.PostAsJsonAsync(BaseUrl + "?api-version=2.0", request).ConfigureAwait(false);
                var postContent = new Dictionary<string, string>() {
                    { "inputdata2", inputBlob2Location },
                    { "outputdata2", outputBlob2Location },
                    { "outputdata1", outputBlob1Location },
                    { "inputdata1", inputBlob1Location },
                    };
                var content = new FormUrlEncodedContent(postContent);
                var response = await client.PostAsync(BaseUrl + "?api-version=2.0", content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    sResponse = await WriteFailedResponse(response);
                }

                //2.0.0 changes
                //string jobId = await response.Content.ReadAsAsync<string>();
                string jobId = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(string.Format("Job ID: {0}", jobId));


                // start the job
                //Console.WriteLine("Starting the job...");
                response = await client.PostAsync(BaseUrl + "/" + jobId + "/start?api-version=2.0", null);
                if (!response.IsSuccessStatusCode)
                {
                    sResponse = await WriteFailedResponse(response);
                    return sResponse;
                }

                ////var response = await client.PostAsJsonAsync(BaseUrl, request).ConfigureAwait(false);
                //string jobId = await response.Content.ReadAsAsync<string>().ConfigureAwait(false);
                string jobLocation = BaseUrl + "/" + jobId + "?api-version=2.0";
                Stopwatch watch = Stopwatch.StartNew();
                bool done = false;
                while (!done)
                {
                    //Console.WriteLine("Checking the job status...");
                    response = await client.GetAsync(jobLocation);
                    if (!response.IsSuccessStatusCode)
                    {
                        sResponse = await WriteFailedResponse(response);
                    }
                    //2.0.0 changes
                    //BatchScoreStatus status = await response.Content.ReadAsAsync<BatchScoreStatus>();
                    var statusString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //bug introduced in order to compile
                    BatchScoreStatus status = new BatchScoreStatus();
                    if (watch.ElapsedMilliseconds > TimeOutInMilliseconds)
                    {
                        done = true;
                        sResponse = string.Format("Timed out. Deleting job {0} ...", jobId);
                        //Console.WriteLine(string.Format("Timed out. Deleting job {0} ...", jobId));
                        await client.DeleteAsync(jobLocation);
                    }
                    switch (status.StatusCode)
                    {
                        case BatchScoreStatusCode.NotStarted:
                            //Console.WriteLine(string.Format("Job {0} not yet started...", jobId));
                            break;
                        case BatchScoreStatusCode.Running:
                            //Console.WriteLine(string.Format("Job {0} running...", jobId));
                            break;
                        case BatchScoreStatusCode.Failed:
                            //Console.WriteLine(string.Format("Job {0} failed!", jobId));
                            //Console.WriteLine(string.Format("Error details: {0}", status.Details));
                            sResponse = string.Format("Error details: {0}", status.Details);
                            done = true;
                            break;
                        case BatchScoreStatusCode.Cancelled:
                            //Console.WriteLine(string.Format("Job {0} cancelled!", jobId));
                            done = true;
                            break;
                        case BatchScoreStatusCode.Finished:
                            done = true;
                            //Console.WriteLine(string.Format("Job {0} finished!", jobId));
                            sResponse = string.Concat("Success with status code: ", response.StatusCode);
                            //sResponse += await response.Content.ReadAsStringAsync();
                            break;
                    }

                    if (!done)
                    {
                        //2.0.0 change
                        //Thread.Sleep(1000); // Wait one second
                    }
                }
            }
            return sResponse;
        }
        async Task<string> WriteFailedResponse(HttpResponseMessage response)
        {
            string sError = string.Format("The request failed with status code: {0}", response.StatusCode);

            // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
            sError += response.Headers.ToString();

            sError += await response.Content.ReadAsStringAsync();
            return sError;
        }

    }
   
    public class AzureBlobDataReference
    {
        // Storage connection string used for regular blobs. It has the following format:
        // DefaultEndpointsProtocol=https;AccountName=ACCOUNT_NAME;AccountKey=ACCOUNT_KEY
        // It's not used for shared access signature blobs.
        public string ConnectionString { get; set; }

        // Relative uri for the blob, used for regular blobs as well as shared access 
        // signature blobs.
        public string RelativeLocation { get; set; }

        // Base url, only used for shared access signature blobs.
        public string BaseLocation { get; set; }

        // Shared access signature, only used for shared access signature blobs.
        public string SasBlobToken { get; set; }
    }

    public enum BatchScoreStatusCode
    {
        NotStarted,
        Running,
        Failed,
        Cancelled,
        Finished
    }

    public class BatchScoreStatus
    {
        // Status code for the batch scoring job
        public BatchScoreStatusCode StatusCode { get; set; }


        // Locations for the potential multiple batch scoring outputs
        public IDictionary<string, AzureBlobDataReference> Results { get; set; }

        // Error details, if any
        public string Details { get; set; }
    }

    public class BatchScoreRequest
    {
        public AzureBlobDataReference Input { get; set; }
        public IDictionary<string, string> GlobalParameters { get; set; }
    }
    public class BatchExecutionRequest
    {
        public AzureBlobDataReference Input { get; set; }
        public IDictionary<string, string> GlobalParameters { get; set; }

        // Locations for the potential multiple batch scoring outputs
        public IDictionary<string, AzureBlobDataReference> Output { get; set; }
    }

}
