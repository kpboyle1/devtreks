using System;
using System.IO;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General path functions
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	2.0.0 moved all path construction
    ///             into this class. These reduce the construction 
    ///             down to a small number of transparent, easy-to-understand, methods.
    /// </summary>
    public static class AppSettings
    {
        //2.0.0: config settings set with ContentURI 
        //in StartUp.cs and passed to controllers
        public enum APPSETTING_TYPES
        {
            none = 0,
            defaultconnection = 1,
            storageconnection = 2,
            defaultrootfullfilepath = 3,
            defaultrootwebstoragepath = 4,
            defaultwebdomain = 5,
            filesizevalidation = 6,
            filesizedbstoragevalidation = 7,
            pagesize = 8,
            pagesizeedits = 9,
            rexecutable = 10,
            pyexecutable = 11,
            hostfeerate = 12,
            resourceuriname = 13,
            contenturiname = 14,
            tempdocsuriname = 15,
            extensions = 16,
            platformtype = 17,
            juliaexecutable = 18
        }
        public static void CopyURIAppSettings(ContentURI fromURI, ContentURI toURI)
        {
            toURI.URIDataManager.DefaultRootFullFilePath = fromURI.URIDataManager.DefaultRootFullFilePath;
            toURI.URIDataManager.DefaultRootWebStoragePath = fromURI.URIDataManager.DefaultRootWebStoragePath;
            toURI.URIDataManager.DefaultWebDomain = fromURI.URIDataManager.DefaultWebDomain;
            toURI.URIDataManager.FileSystemPath = fromURI.URIDataManager.FileSystemPath;
            toURI.URIDataManager.WebPath = fromURI.URIDataManager.WebPath;
            toURI.URIDataManager.DefaultConnection = fromURI.URIDataManager.DefaultConnection;
            toURI.URIDataManager.StorageConnection = fromURI.URIDataManager.StorageConnection;
            toURI.URIDataManager.FileSizeValidation = fromURI.URIDataManager.FileSizeValidation;
            toURI.URIDataManager.FileSizeDBStorageValidation = fromURI.URIDataManager.FileSizeDBStorageValidation;
            toURI.URIDataManager.PageSize = fromURI.URIDataManager.PageSize;
            toURI.URIDataManager.PageSizeEdits = fromURI.URIDataManager.PageSizeEdits;
            toURI.URIDataManager.RExecutable = fromURI.URIDataManager.RExecutable;
            toURI.URIDataManager.PyExecutable = fromURI.URIDataManager.PyExecutable;
            toURI.URIDataManager.JuliaExecutable = fromURI.URIDataManager.JuliaExecutable;
            toURI.URIDataManager.HostFeeRate = fromURI.URIDataManager.HostFeeRate;
            toURI.URIDataManager.ResourceURIName = fromURI.URIDataManager.ResourceURIName;
            toURI.URIDataManager.ContentURIName = fromURI.URIDataManager.ContentURIName;
            toURI.URIDataManager.TempDocsURIName = fromURI.URIDataManager.TempDocsURIName;
            toURI.URIDataManager.ExtensionsPath = fromURI.URIDataManager.ExtensionsPath;
            toURI.URIDataManager.PlatformType = fromURI.URIDataManager.PlatformType;
        }
        public static string GetAppSettingString(ContentURI uri,
            APPSETTING_TYPES appSetting)
        {
            string sAppSettingValue = string.Empty;
            if (appSetting
                == APPSETTING_TYPES.contenturiname)
            {
                sAppSettingValue = uri.URIDataManager.ContentURIName;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultconnection)
            {
                sAppSettingValue = uri.URIDataManager.DefaultConnection;
            }
            else if (appSetting
                == APPSETTING_TYPES.storageconnection)
            {
                sAppSettingValue = uri.URIDataManager.StorageConnection;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultrootfullfilepath)
            {
                sAppSettingValue = uri.URIDataManager.DefaultRootFullFilePath;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultrootwebstoragepath)
            {
                sAppSettingValue = uri.URIDataManager.DefaultRootWebStoragePath;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultwebdomain)
            {
                sAppSettingValue = uri.URIDataManager.DefaultWebDomain;
            }
            else if (appSetting
                == APPSETTING_TYPES.filesizedbstoragevalidation)
            {
                sAppSettingValue = uri.URIDataManager.FileSizeDBStorageValidation;
            }
            else if (appSetting
                == APPSETTING_TYPES.filesizevalidation)
            {
                sAppSettingValue = uri.URIDataManager.FileSizeValidation;
            }
            else if (appSetting
                == APPSETTING_TYPES.hostfeerate)
            {
                sAppSettingValue = uri.URIDataManager.HostFeeRate;
            }
            else if (appSetting
                == APPSETTING_TYPES.pagesize)
            {
                sAppSettingValue = uri.URIDataManager.PageSize.ToString();
            }
            else if (appSetting
                == APPSETTING_TYPES.pyexecutable)
            {
                sAppSettingValue = uri.URIDataManager.PyExecutable;
            }
            else if (appSetting
                == APPSETTING_TYPES.juliaexecutable)
            {
                sAppSettingValue = uri.URIDataManager.JuliaExecutable.ToString();
            }
            else if (appSetting
                == APPSETTING_TYPES.resourceuriname)
            {
                sAppSettingValue = uri.URIDataManager.ResourceURIName;
            }
            else if (appSetting
                == APPSETTING_TYPES.rexecutable)
            {
                sAppSettingValue = uri.URIDataManager.RExecutable;
            }
            else if (appSetting
                == APPSETTING_TYPES.pagesizeedits)
            {
                sAppSettingValue = uri.URIDataManager.PageSizeEdits;
            }
            else if (appSetting
                == APPSETTING_TYPES.tempdocsuriname)
            {
                sAppSettingValue = uri.URIDataManager.TempDocsURIName;
            }
            else if (appSetting
                == APPSETTING_TYPES.extensions)
            {
                sAppSettingValue = uri.URIDataManager.ExtensionsPath;
            }
            else if (appSetting
                == APPSETTING_TYPES.platformtype)
            {
                sAppSettingValue = uri.URIDataManager.PlatformType.ToString();
            }
            else
            {
                sAppSettingValue = APPSETTING_TYPES.none.ToString();
            }
            return sAppSettingValue;
        }
        public static string GetAppSettingString(ContentURI uri,
            string appSetting)
        {
            string sAppSettingValue = string.Empty;
            appSetting = appSetting.ToLower();
            if (appSetting
                == APPSETTING_TYPES.contenturiname.ToString())
            {
                sAppSettingValue = uri.URIDataManager.ContentURIName;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultconnection.ToString())
            {
                sAppSettingValue = uri.URIDataManager.DefaultConnection;
            }
            else if (appSetting
                == APPSETTING_TYPES.storageconnection.ToString())
            {
                sAppSettingValue = uri.URIDataManager.StorageConnection;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultrootfullfilepath.ToString())
            {
                sAppSettingValue = uri.URIDataManager.DefaultRootFullFilePath;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultrootwebstoragepath.ToString())
            {
                sAppSettingValue = uri.URIDataManager.DefaultRootWebStoragePath;
            }
            else if (appSetting
                == APPSETTING_TYPES.defaultwebdomain.ToString())
            {
                sAppSettingValue = uri.URIDataManager.DefaultWebDomain;
            }
            else if (appSetting
                == APPSETTING_TYPES.filesizedbstoragevalidation.ToString())
            {
                sAppSettingValue = uri.URIDataManager.FileSizeDBStorageValidation;
            }
            else if (appSetting
                == APPSETTING_TYPES.filesizevalidation.ToString())
            {
                sAppSettingValue = uri.URIDataManager.FileSizeValidation;
            }
            else if (appSetting
                == APPSETTING_TYPES.hostfeerate.ToString())
            {
                sAppSettingValue = uri.URIDataManager.HostFeeRate;
            }
            else if (appSetting
                == APPSETTING_TYPES.pagesize.ToString())
            {
                sAppSettingValue = uri.URIDataManager.PageSize.ToString();
            }
            else if (appSetting
                == APPSETTING_TYPES.rexecutable.ToString())
            {
                sAppSettingValue = uri.URIDataManager.RExecutable;
            }
            else if (appSetting
                == APPSETTING_TYPES.pyexecutable.ToString())
            {
                sAppSettingValue = uri.URIDataManager.PyExecutable;
            }
            else if (appSetting
                == APPSETTING_TYPES.juliaexecutable.ToString())
            {
                sAppSettingValue = uri.URIDataManager.JuliaExecutable.ToString();
            }
            else if (appSetting
                == APPSETTING_TYPES.resourceuriname.ToString())
            {
                sAppSettingValue = uri.URIDataManager.ResourceURIName;
            }
            else if (appSetting
                == APPSETTING_TYPES.rexecutable.ToString())
            {
                sAppSettingValue = uri.URIDataManager.RExecutable;
            }
            else if (appSetting
                == APPSETTING_TYPES.pagesizeedits.ToString())
            {
                sAppSettingValue = uri.URIDataManager.PageSizeEdits;
            }
            else if (appSetting
                == APPSETTING_TYPES.tempdocsuriname.ToString())
            {
                sAppSettingValue = uri.URIDataManager.TempDocsURIName;
            }
            else if (appSetting
                == APPSETTING_TYPES.extensions.ToString())
            {
                sAppSettingValue = uri.URIDataManager.ExtensionsPath;
            }
            else if (appSetting
                == APPSETTING_TYPES.platformtype.ToString())
            {
                sAppSettingValue = uri.URIDataManager.PlatformType.ToString();
            }
            else
            {
                sAppSettingValue = APPSETTING_TYPES.none.ToString();
            }
            return sAppSettingValue;
        }
        public static string GetConnection(ContentURI uri)
        {
            string sConnect = string.Empty;
            sConnect = GetAppSettingString(uri,
                APPSETTING_TYPES.defaultconnection);
            return sConnect;
        }
        public static string GetandSetPath(ContentURI uri,
            string rootPath, string subFolder1, string fileName,
            string subFolder2 = GeneralHelpers.NONE, string subPath = GeneralHelpers.NONE)
        {
            //examples: 
            //images path in web root
            //    rootPath: http://wwww.devtreks.org,
            //    subFolder1: images,
            //    subFolder2: string.empty
            //    subPath: string.empty,
            //    fileName: devtreks-logo.png);
            //resources 
            //    rootPath: http://wwww.devtreks.org,
            //    subFolder1: resources,
            //    subFolder2: string.empty
            //    subPath: network_carbon/resourcepack_111/resource_222,
            //    fileName: text.csv);
            //xml content
            //    rootPath: http://wwww.devtreks.org,
            //    subFolder1: resources,
            //    subFolder2: commontreks (or, if network storage: network_ag)
            //    subPath: club_372/prices/outputprices/servicebase_245/outputgroup_56/output_7,
            //    fileName: corn.xml);
            string sAppSettingPath = string.Empty;
            //subpaths usually web delimited uripatterns
            string sSubPath = string.Empty;
            if (rootPath.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER) == false
                && rootPath.EndsWith(GeneralHelpers.WEBFILE_PATH_DELIMITER) == false)
            {
                if (Path.IsPathRooted(rootPath))
                {
                    rootPath = string.Concat(rootPath, GeneralHelpers.FILE_PATH_DELIMITER);
                }
                else
                {
                    rootPath = string.Concat(rootPath, GeneralHelpers.WEBFILE_PATH_DELIMITER);
                }
            }
            //subfolderpath2 and subPath are optional
            if (subFolder2 == GeneralHelpers.NONE && subPath
            == GeneralHelpers.NONE)
            {
                if (Path.IsPathRooted(rootPath))
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.WEBFILE_PATH_DELIMITER,
                        GeneralHelpers.FILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}\\{3}",
                        rootPath, subFolder1, fileName);
                    uri.URIDataManager.FileSystemPath = sAppSettingPath;
                }
                else
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.FILE_PATH_DELIMITER,
                        GeneralHelpers.WEBFILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}/{3}",
                        rootPath, subFolder1, fileName);
                    uri.URIDataManager.WebPath = sAppSettingPath;
                }
            }
            else if (subFolder2 != GeneralHelpers.NONE && subPath
                == GeneralHelpers.NONE)
            {
                if (Path.IsPathRooted(rootPath))
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.WEBFILE_PATH_DELIMITER,
                        GeneralHelpers.FILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}\\{3}\\{4}",
                        rootPath, subFolder1, subFolder2, fileName);
                    uri.URIDataManager.FileSystemPath = sAppSettingPath;
                }
                else
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.FILE_PATH_DELIMITER,
                        GeneralHelpers.WEBFILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}/{3}/{4}",
                        rootPath, subFolder1, subFolder2, fileName);
                    uri.URIDataManager.WebPath = sAppSettingPath;
                }
            }
            else if (subFolder2 == GeneralHelpers.NONE
                && subPath != GeneralHelpers.NONE)
            {
                if (Path.IsPathRooted(rootPath))
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.WEBFILE_PATH_DELIMITER,
                        GeneralHelpers.FILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}\\{3}\\{4}",
                        rootPath, subFolder1, subPath, fileName);
                    uri.URIDataManager.FileSystemPath = sAppSettingPath;
                }
                else
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.FILE_PATH_DELIMITER,
                        GeneralHelpers.WEBFILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}/{3}/{4}",
                        rootPath, subFolder1, subPath, fileName);
                    uri.URIDataManager.WebPath = sAppSettingPath;
                }
            }
            else if (subFolder2 != GeneralHelpers.NONE
                && subPath != GeneralHelpers.NONE)
            {
                if (Path.IsPathRooted(rootPath))
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.WEBFILE_PATH_DELIMITER,
                        GeneralHelpers.FILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}\\{3}\\{4}\\{5}",
                        rootPath, subFolder1, subFolder2, subPath, fileName);
                    uri.URIDataManager.FileSystemPath = sAppSettingPath;
                }
                else
                {
                    sSubPath = subPath.Replace(
                        GeneralHelpers.FILE_PATH_DELIMITER,
                        GeneralHelpers.WEBFILE_PATH_DELIMITER);
                    sAppSettingPath = string.Format("{0}{1}/{3}/{4}/{5}",
                        rootPath, subFolder1, subFolder2, subPath, fileName);
                    uri.URIDataManager.WebPath = sAppSettingPath;
                }
            }
            return sAppSettingPath;
        }
        //converts path between file system paths web paths
        public static string ConvertPathFileandWeb(ContentURI uri, string path)
        {
            string sConvertedPath = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                if (Path.IsPathRooted(path))
                {
                    sConvertedPath = path.Replace(
                        uri.URIDataManager.DefaultRootFullFilePath,
                        uri.URIDataManager.DefaultWebDomain);
                    sConvertedPath = sConvertedPath.Replace(
                        GeneralHelpers.FILE_PATH_DELIMITER, GeneralHelpers.WEBFILE_PATH_DELIMITER);
                }
                else
                {
                    sConvertedPath = path.Replace(
                        uri.URIDataManager.DefaultWebDomain,
                        uri.URIDataManager.DefaultRootFullFilePath);
                    sConvertedPath = sConvertedPath.Replace(
                        GeneralHelpers.WEBFILE_PATH_DELIMITER, GeneralHelpers.FILE_PATH_DELIMITER);
                }
            }
            return sConvertedPath;
        }
       
        public static string MakeFilePath(string pathDelimiter, string idDelimiter,
        string networkPath, string networkId, string accountGroup, string accountId,
        string appType, string subAppType, string nodeGroupName, string nodeGroupId,
        string nodeName, string nodeId)
        {
            string sFilePath = string.Empty;
            string sFilePath1 = GeneralHelpers.MakeString(networkPath,
                accountGroup, idDelimiter, accountId, string.Empty, string.Empty);
            string sFilePath2 = string.Empty;
            if (subAppType != string.Empty
                && subAppType != GeneralHelpers.SUBAPPLICATION_TYPES.none.ToString()
                && subAppType != appType)
            {
                sFilePath2 = GeneralHelpers.MakeString(pathDelimiter, appType, pathDelimiter, subAppType, string.Empty, string.Empty);
            }
            else
            {
                sFilePath2 = GeneralHelpers.MakeString(pathDelimiter, appType, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            string sFilePath3 = string.Empty;
            if (nodeName != string.Empty)
            {
                sFilePath3 = GeneralHelpers.MakeString(pathDelimiter, nodeGroupName, idDelimiter, nodeGroupId, pathDelimiter, nodeName);
                string sFilePath4 = string.Empty;
                sFilePath4 = GeneralHelpers.MakeString(idDelimiter, nodeId, string.Empty, string.Empty, string.Empty, string.Empty);
                sFilePath = GeneralHelpers.MakeString(sFilePath1, sFilePath2, sFilePath3, sFilePath4, string.Empty, string.Empty);
            }
            else
            {
                sFilePath3 = GeneralHelpers.MakeString(pathDelimiter, nodeGroupName, idDelimiter, nodeGroupId, string.Empty, string.Empty);
                sFilePath = GeneralHelpers.MakeString(sFilePath1, sFilePath2, sFilePath3, string.Empty, string.Empty, string.Empty);
            }
            return sFilePath;
        }
        //set in ViewDataHelper when uri is configured with appsettings
        public static void SetAbsoluteURLPath(ContentURI uri,
            string absoluteURLPath)
        {
            string sWebPath = absoluteURLPath;
            FileStorageIO.PLATFORM_TYPES ePlatform = FileStorageIO.GetPlatformType(uri);
            if (ePlatform != FileStorageIO.PLATFORM_TYPES.azure)
            {
                //only azure should be using https
                sWebPath = absoluteURLPath.Replace("https", "http");
            }
            uri.URIDataManager.WebPath = sWebPath;
        }
        public static string GetResourceRootPath(ContentURI uri,
            string networkWebFileSystemPath)
        {
            string sPathToContent = string.Empty;
            //networkWebFileSystemName derives from uri.URINetwork.WebFileSystemPath 
            //and is stored in db table Network
            string sContentContainerOrFolderName =
                (string.IsNullOrEmpty(networkWebFileSystemPath)) ?
                uri.URIDataManager.ContentURIName
                : networkWebFileSystemPath;
            FileStorageIO.PLATFORM_TYPES ePlatform = FileStorageIO.GetPlatformType(uri);
            if (ePlatform == FileStorageIO.PLATFORM_TYPES.webserver)
            {
                //store in temp subfolder of resources directory (set permissions once)
                bool bIsAzureStorage = false;
                string sRoot = GetResourceRootPath(uri, bIsAzureStorage);
                if (!sRoot.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER)
                    && (!string.IsNullOrEmpty(sRoot)))
                {
                    sRoot = string.Concat(sRoot, GeneralHelpers.FILE_PATH_DELIMITER);
                }
                sPathToContent = string.Format("{0}{1}{2}",
                    sRoot, sContentContainerOrFolderName,
                    GeneralHelpers.FILE_PATH_DELIMITER);
            }
            else if (ePlatform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                //store in 'commontreks' (or other network containername) container
                sPathToContent = string.Format("{0}{1}{2}",
                    uri.URIDataManager.DefaultRootWebStoragePath,
                    sContentContainerOrFolderName, GeneralHelpers.WEBFILE_PATH_DELIMITER);
            }
            return sPathToContent;
        }
        public static string GetTempWebDirectory(ContentURI uri,
            bool isPackageDirectory, int randomId)
        {
            //if randomid is not enough switch to guid
            string sTempDirPath = string.Empty;
            string sRootDirectory = string.Empty;
            if (isPackageDirectory)
            {
                sRootDirectory = GetTempCacheRootPath(uri);
                sTempDirPath = string.Concat(sRootDirectory, randomId.ToString());
            }
            else
            {
                //0.9.1: multiple servers won't work with local cache; needs blob storage
                sRootDirectory = GetTempRootPath(uri);
                sTempDirPath = string.Concat(sRootDirectory, randomId.ToString());
            }
            FileStorageIO.DirectoryCreate(uri, sTempDirPath);
            return sTempDirPath;
        }
        public static string GetTempCacheRootPath(ContentURI uri)
        {
            string sPathToTempContent = string.Empty;
            //store in temp subfolder of resources directory (set permissions once)
            bool bIsAzureStorage = false;
            string sRoot = GetResourceRootPath(uri, bIsAzureStorage);
            if (!sRoot.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER)
                && (!string.IsNullOrEmpty(sRoot)))
            {
                sRoot = string.Concat(sRoot, GeneralHelpers.FILE_PATH_DELIMITER);
            }
            sPathToTempContent = string.Format("{0}{1}{2}",
                sRoot, uri.URIDataManager.TempDocsURIName,
                GeneralHelpers.FILE_PATH_DELIMITER);
            return sPathToTempContent;
        }
        //2.0.0 deprecated: azure and localhost can both use filesystem 
        //by using appsettings.DefaultRootFullFilePath
        //public static string GetTempCacheRootPath(ContentURI uri)
        //{
        //    string sPathToTempContent = string.Empty;
        //    FileStorageIO.PLATFORM_TYPES ePlatform = FileStorageIO.GetPlatformType(uri);
        //    if (ePlatform == FileStorageIO.PLATFORM_TYPES.webserver)
        //    {
        //        //store in temp subfolder of resources directory (set permissions once)
        //        bool bIsAzureStorage = false;
        //        string sRoot = GetResourceRootPath(uri, bIsAzureStorage);
        //        if (!sRoot.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER)
        //            && (!string.IsNullOrEmpty(sRoot)))
        //        {
        //            sRoot = string.Concat(sRoot, GeneralHelpers.FILE_PATH_DELIMITER);
        //        }
        //        sPathToTempContent = string.Format("{0}{1}{2}",
        //            sRoot, uri.URIDataManager.TempDocsURIName,
        //            GeneralHelpers.FILE_PATH_DELIMITER);
        //    }
        //    else if (ePlatform == FileStorageIO.PLATFORM_TYPES.azure)
        //    {
        //        AzureIOAsync azureIO = new AzureIOAsync(uri);
        //        //store in local temp cache (file system relative to root of virual server)
        //        //note: needs a file path delimiter
        //        sPathToTempContent = string.Format("{0}{1}{2}",
        //            azureIO.GetLocalResourceDirectoryPath(),
        //            uri.URIDataManager.TempDocsURIName,
        //            GeneralHelpers.FILE_PATH_DELIMITER);
        //    }
        //    return sPathToTempContent;
        //}
        //used to build the download package zip file on azure
        public static string GetCloudTempBlobURI(ContentURI uri, string fileName)
        {
            string sTempBlobURI = string.Empty;
            string sDir = GetTempWebDirectory(uri, false, GeneralHelpers.Get2RandomInteger());
            //2.0.0 deprecated guid in name because of azure appserve msg about resource name of blob being too long
            //blob names are limited to 1024 chars
            sTempBlobURI = string.Concat(sDir, GeneralHelpers.WEBFILE_PATH_DELIMITER,
                  GeneralHelpers.Get2RandomInteger().ToString(), GeneralHelpers.WEBFILE_PATH_DELIMITER,
                  fileName);

            //sTempBlobURI = string.Concat(sDir, GeneralHelpers.WEBFILE_PATH_DELIMITER,
            //       Guid.NewGuid().ToString(), GeneralHelpers.WEBFILE_PATH_DELIMITER,
            //       fileName);
            return sTempBlobURI;
        }
        public static string GetResourceRootPath(ContentURI uri, bool needsWebPath)
        {
            //file system full path
            //2.0.0 changes:
            string sPathToResource = string.Format("{0}{1}{2}",
                uri.URIDataManager.DefaultRootFullFilePath,
                uri.URIDataManager.ResourceURIName,
                GeneralHelpers.FILE_PATH_DELIMITER);
            if (needsWebPath)
            {
                //full paths are needed because of blob storage in cloud platforms
                sPathToResource = string.Format("{0}{1}{2}",
                    uri.URIDataManager.DefaultRootWebStoragePath,
                    uri.URIDataManager.ResourceURIName,
                    GeneralHelpers.WEBFILE_PATH_DELIMITER);
            }
            return sPathToResource;
        }
        public static string GetTempRootPath(ContentURI uri)
        {
            string sPathToTempContent = string.Empty;
            FileStorageIO.PLATFORM_TYPES ePlatform = FileStorageIO.GetPlatformType(uri);
            if (ePlatform == FileStorageIO.PLATFORM_TYPES.webserver)
            {
                //store in temp subfolder of resources directory (set permissions once)
                bool bIsAzureStorage = false;
                string sRoot = GetResourceRootPath(uri, bIsAzureStorage);
                if (!sRoot.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER)
                    && (!string.IsNullOrEmpty(sRoot)))
                {
                    sRoot = string.Concat(sRoot, GeneralHelpers.FILE_PATH_DELIMITER);
                }
                sPathToTempContent = string.Format("{0}{1}{2}",
                    sRoot, uri.URIDataManager.TempDocsURIName,
                    GeneralHelpers.FILE_PATH_DELIMITER);
            }
            else if (ePlatform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                //performance will be better when temp files are in same cloud storage as content
                //copying back and forth will be easier
                sPathToTempContent = string.Format("{0}{1}{2}",
                    uri.URIDataManager.DefaultRootWebStoragePath,
                    uri.URIDataManager.TempDocsURIName,
                    GeneralHelpers.WEBFILE_PATH_DELIMITER);
            }
            return sPathToTempContent;
        }
        public static string GetStandardFileDirectoryName(string nodeName,
            string nodeId)
        {
            string sDirectoryName = string.Concat(nodeName, GeneralHelpers.FILENAME_DELIMITER,
                nodeId);
            return sDirectoryName;
        }
        public static string GetStylesheetFullPath(ContentURI uri, string endPath)
        {
            string sRootPath = GetServerRootPath(uri);
            //get rid of starting path delimiter
            //2.0.0 not clear this is needed, but will need to retrieve ss from storage
            //resources/stylesheets
            string sStyleRootPath = string.Concat(uri.URIDataManager.ResourceURIName,
                GeneralHelpers.FILE_PATH_DELIMITER, "stylesheets");
            string sFullPath = string.Format("{0}{1}{2}{3}",
               sRootPath, sStyleRootPath, GeneralHelpers.FILE_PATH_DELIMITER, endPath);
            //this static content is full file system path
            sFullPath = sFullPath.Replace(
                GeneralHelpers.WEBFILE_PATH_DELIMITER,
                GeneralHelpers.FILE_PATH_DELIMITER);
            return sFullPath;
        }
        public static string GetPackageFullPath(ContentURI uri, string pkgEndPath)
        {
            string sRootPath = GetServerRootPath(uri);
            //2.0.0 not clear this is needed, but will need to retrieve packages from storage
            //resources/packages
            string sPackageRoot = string.Concat(uri.URIDataManager.ResourceURIName,
                GeneralHelpers.FILE_PATH_DELIMITER, "packages");
            string sFullPath = string.Format("{0}{1}{2}{3}",
               sRootPath, sPackageRoot, GeneralHelpers.FILE_PATH_DELIMITER, pkgEndPath);
            //this static content is full file system path
            sFullPath = sFullPath.Replace(
                GeneralHelpers.WEBFILE_PATH_DELIMITER,
                GeneralHelpers.FILE_PATH_DELIMITER);
            return sFullPath;
        }
        //gets paths to css, js, images in web root
        public static string GetWebContentFullPath(ContentURI uri,
            string webSubfolder, string filename)
        {
            string sPathToContent = string.Empty;
            FileStorageIO.PLATFORM_TYPES ePlatform = FileStorageIO.GetPlatformType(uri);
            if (ePlatform == FileStorageIO.PLATFORM_TYPES.webserver)
            {
                sPathToContent = string.Format("{0}{1}{2}{3}",
                    uri.URIDataManager.DefaultRootFullFilePath,
                    webSubfolder, GeneralHelpers.FILE_PATH_DELIMITER,
                    filename);
            }
            else if (ePlatform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                //do not use the path to blob storage: DefaultRootWebStoragePath
                sPathToContent = string.Format("{0}{1}{2}{3}",
                    uri.URIDataManager.DefaultWebDomain,
                    webSubfolder, GeneralHelpers.WEBFILE_PATH_DELIMITER,
                    filename);
            }
            return sPathToContent;
        }
        ///<summary>
        /// Builds a package path to resources
        /// </summary>
        /// <param name="endPath">the remaining path to the stylesheet</param>
        public static string GetPackageFullPath(this ContentURI uri, string rootDirectory,
            string resourceSubFolderName, string resourceFile)
        {
            string sPackageFullPath = string.Format("{0}/{1}/{2}",
                rootDirectory,
                resourceSubFolderName, resourceFile);
            if (Path.IsPathRooted(sPackageFullPath))
            {
                sPackageFullPath
                    = sPackageFullPath.Replace(GeneralHelpers.WEBFILE_PATH_DELIMITER,
                    GeneralHelpers.FILE_PATH_DELIMITER);
            }
            else
            {
                sPackageFullPath
                    = sPackageFullPath.Replace(GeneralHelpers.FILE_PATH_DELIMITER,
                    GeneralHelpers.WEBFILE_PATH_DELIMITER);
            }
            return sPackageFullPath;
        }
        public static string GetServerRootPath(ContentURI uri)
        {
            string sServerRootPath = uri.URIDataManager.DefaultRootFullFilePath;
            return sServerRootPath;
        }
        public static int GetPageSize(ContentURI uri)
        {
            string sPageSize = string.Empty;
            int iPageSize = 25;
            iPageSize = (uri.URIDataManager.PageSize <= 0) ?
                iPageSize : uri.URIDataManager.PageSize;
            //edit panel has separate page size
            //linked views panel has to have same pagination or can't switch between panels
            //(i.e. when editing a custom doc that's number 11 in lv -won't find it in edit)
            if (uri.URIDataManager.ServerActionType == GeneralHelpers.SERVER_ACTION_TYPES.edit
                || uri.URIDataManager.ServerActionType == GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
            {
                sPageSize = (!string.IsNullOrEmpty(uri.URIDataManager.PageSizeEdits)) ?
                    uri.URIDataManager.PageSizeEdits : "10";
                iPageSize = GeneralHelpers.ConvertStringToInt(sPageSize);
            }
            return iPageSize;
        }

        public static string GetURIDomain(string urlAuthority,
            ContentURI uri)
        {
            string sURI = uri.URIDataManager.DefaultWebDomain;
            string sURIDomain = string.Concat(@"https://",
                uri.URIFull.Replace(sURI, urlAuthority));
            if (sURI.StartsWith(@"http://"))
            {
                sURIDomain = string.Concat(@"http://",
                uri.URIFull.Replace(sURI, urlAuthority));
            }
            return sURIDomain;
        }

        public static string ConvertAbsPathToRelPath(string rootPath,
            string absolutePath)
        {
            string sRelPathToPackageRoot = string.Empty;
            sRelPathToPackageRoot = absolutePath.Replace(rootPath, string.Empty);
            if (sRelPathToPackageRoot.StartsWith(GeneralHelpers.FILE_PATH_DELIMITER))
            {
                sRelPathToPackageRoot = sRelPathToPackageRoot.Remove(0, 1);
            }
            return sRelPathToPackageRoot;
        }
        public static string SwitchFullandRelTempWebPaths(ContentURI uri,
            string currentWebPath, string webPath)
        {
            string sNewTempPath = string.Empty;
            bool bIsFilePath = false;
            int iIndex = currentWebPath.IndexOf(GeneralHelpers.FILE_PATH_DELIMITER);
            if (iIndex != -1)
            {
                bIsFilePath = true;
            }
            if (bIsFilePath)
            {
                bool bNeedsFullWebPath = true;
                string sRelFilePath = GetResourceRootPath(uri, bNeedsFullWebPath);
                bNeedsFullWebPath = false;
                string sPathToTempDocs = GetResourceRootPath(uri, bNeedsFullWebPath);
                sNewTempPath = currentWebPath.Replace(sPathToTempDocs, sRelFilePath);
                //sNewTempPath = currentWebPath.Replace(ConfigurationManager.ConnectionStrings["DefaultWebResourceFullPath"].ToString(), sRelFilePath);
                sNewTempPath = sNewTempPath.Replace(GeneralHelpers.FILE_PATH_DELIMITER,
                    GeneralHelpers.WEBFILE_PATH_DELIMITER);
            }
            else
            {
                sNewTempPath = Path.GetFullPath(currentWebPath);
            }
            return sNewTempPath;
        }

        public static string GetExtensionsRelPath(ContentURI uri)
        {
            string sPipelinePath = uri.URIDataManager.ExtensionsPath;
            return sPipelinePath;
        }
        public static void SetTempDocPathandFileName(ContentURI uri)
        {
            string sTempURIPattern = string.Empty;
            string sTempURIPath = string.Empty;
            GetTempDocURIPattern(uri, out sTempURIPattern);
            uri.URIDataManager.TempDocURIPattern = sTempURIPattern;
            sTempURIPath = GetTempDocPath(uri, sTempURIPattern);
            uri.ChangeURIPattern(sTempURIPattern);
            uri.URIDataManager.TempDocPath = sTempURIPath;
            uri.URIClub.ClubDocFullPath = sTempURIPath;
            uri.URIMember.MemberDocFullPath = sTempURIPath;
        }
        public static void GetTempDocURIPattern(ContentURI uri,
            out string tempURIPattern)
        {
            tempURIPattern = string.Empty;
            int iId = GeneralHelpers.GetRandomInteger(0);
            //make sure they aren't already using a temp doc
            if (uri.URIFileExtensionType != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //make it reasonably unique so they can carry out edits
                tempURIPattern = GeneralHelpers.MakeURIPattern(uri.URIName,
                    iId.ToString(), uri.URINetworkPartName, uri.URINodeName,
                    GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString());
            }
            else
            {
                tempURIPattern = uri.URIPattern;
            }
        }

        public static string GetTempDocToCalcURIPattern(ContentURI docToCalcURI,
           string tempDocURIPattern)
        {
            string sNewURIPattern = string.Empty;
            string sRandomId = ContentURI.GetURIPatternPart(tempDocURIPattern, ContentURI.URIPATTERNPART.id);
            if (sRandomId != string.Empty)
            {
                string sId = docToCalcURI.URIId.ToString();
                sNewURIPattern = ContentURI.ChangeURIPatternPart(
                    docToCalcURI.URIPattern, ContentURI.URIPATTERNPART.id, sRandomId);
                sNewURIPattern = ContentURI.ChangeURIPatternPart(
                    sNewURIPattern, ContentURI.URIPATTERNPART.fileExtension,
                    GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString());
            }
            return sNewURIPattern;
        }
        public static void SetDocPathandFileNameForTempDocs(ContentURI uri)
        {
            //retain uripattern to find related resources (i.e. stylesheet)
            if (uri.URIDataManager.TempDocURIPattern != string.Empty)
            {
                uri.URIClub.ClubDocFullPath
                    = GetTempDocPath(uri, uri.URIDataManager.TempDocURIPattern);
            }
            else
            {
                uri.URIClub.ClubDocFullPath = GetTempDocPath(uri, uri.URIPattern);
            }
            uri.URIMember.MemberDocFullPath = uri.URIClub.ClubDocFullPath;
        }
        public static string GetXhtmlDocPath(ContentURI uri,
            GeneralHelpers.DOC_STATE_NUMBER displayDocType,
            string xmlDocPath, AppHelpers.Resources.FILEEXTENSION_TYPES fileExtType,
            string viewEditType)
        {
            string sXhtmlDocPath = string.Empty;
            if (string.IsNullOrEmpty(xmlDocPath))
            {
                uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "IOHELPER_CANTBUILD");
                return sXhtmlDocPath;
            }
            //same paths, different file extensions
            sXhtmlDocPath = xmlDocPath;
            GeneralHelpers.ChangeFileExtension(
                fileExtType, ref sXhtmlDocPath);
            //add viewEditType as a filename extension (owners and authorized clubs usually 
            //get full html view while others get print view)
            //and use pagination with the html 
            //and make sure the pagination doesn't interfere with uripatterns
            string sFileName = Path.GetFileNameWithoutExtension(xmlDocPath);
            string sHtmlFileName
                = string.Concat(sFileName,
                GeneralHelpers.FILENAME_DELIMITER, viewEditType);
            if (displayDocType == GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
            {
                //stories are the same in all views
                if (!sFileName.Contains(string.Concat(GeneralHelpers.FILENAME_DELIMITER,
                    GeneralHelpers.FILENAME_EXTENSIONS.selected)))
                {
                    //v1.1.1 uses multiple html views
                    GeneralHelpers.SUBACTION_VIEWS eSubActionView
                        = GeneralHelpers.GetSubActionView(uri.URIDataManager.SubActionView);
                    //do not include graph subaction view -no stateful html doc used
                    if (eSubActionView == GeneralHelpers.SUBACTION_VIEWS.mobile
                        || eSubActionView == GeneralHelpers.SUBACTION_VIEWS.full
                        || eSubActionView == GeneralHelpers.SUBACTION_VIEWS.summary)
                    {
                        //each html view is distinguished by the int representation of enum
                        int iSubActionView = (int)eSubActionView;
                        sHtmlFileName = string.Concat(sFileName,
                            GeneralHelpers.FILENAME_DELIMITER, viewEditType,
                            iSubActionView.ToString());
                    }
                }
            }
            sXhtmlDocPath
                = sXhtmlDocPath.Replace(sFileName, sHtmlFileName);
            //technically not an issue in 2.0.0 servers but not clear about 
            //where this is being deployed so retain for now
            if (sXhtmlDocPath.Length > 260)
            {
                //watch for file length limitation
                int iShortNameLength = sXhtmlDocPath.Length - 257;
                string sNewName = sFileName.Substring(iShortNameLength,
                    sFileName.Length - iShortNameLength);
                sXhtmlDocPath = sXhtmlDocPath.Replace(sFileName, sNewName);
            }
            return sXhtmlDocPath;
        }

        public static string GetTempDocPath(ContentURI uri,
            string tempURIPattern)
        {
            bool bIsLocalCacheDirectory = false;
            string sTempDocPath = GetTempDocPath(uri, bIsLocalCacheDirectory, uri.URIPattern, tempURIPattern);
            return sTempDocPath;

        }
        public static string GetTempDocPath(ContentURI uri, bool isLocalCacheDirectory,
            string uriPattern, string tempURIPattern)
        {
            string sTempDocPath = string.Empty;
            //use uri.URIDataManager.TempDocURI to set subdirectory for holding
            //all related tempdocs (easy to package)
            string sRandomId = ContentURI.GetURIPatternPart(tempURIPattern, ContentURI.URIPATTERNPART.id);
            string sNodeName = ContentURI.GetURIPatternPart(tempURIPattern, ContentURI.URIPATTERNPART.node);
            string sDirectory = string.Empty;
            //ok to put empty uris in a "0" subfolder
            if (sRandomId != string.Empty
                && sNodeName != GeneralHelpers.NONE
                && sRandomId != "0")
            {
                //has a tempdoc to work with
                //uri.uridatamanager.tempdocuri's random urid is always subdir
                sDirectory = GetTempWebDirectory(uri, isLocalCacheDirectory,
                    GeneralHelpers.ConvertStringToInt(sRandomId));
            }
            else
            {
                //may be starting to build a tempdoc
                //need a random directory
                sDirectory = GetTempWebDirectory(uri, isLocalCacheDirectory,
                    GeneralHelpers.Get2RandomInteger());
            }
            string sDelimiter = FileStorageIO.GetDelimiterForFileStorage(sDirectory);
            //don't use uri.uriclubdocpath filename because it may not need to be set
            //for the selectedlinkedviewuri
            string sFileName = string.Concat(
                ContentHelper.MakeStandardFileNameFromURIPattern(uriPattern),
                GeneralHelpers.EXTENSION_XML);
            //make the tempdocpath
            sTempDocPath = string.Concat(sDirectory, sDelimiter,
                ContentHelper.MakeStandardFileNameFromURIPattern(uriPattern),
                GeneralHelpers.EXTENSION_XML);
            return sTempDocPath;
        }
        public static string GetMiscDocURI(string existingURI)
        {
            string sMiscDocURI = string.Empty;
            string sDirectory = FileStorageIO.GetDirectoryName(existingURI);
            string sFileNameNoExt = Path.GetFileNameWithoutExtension(existingURI);
            string sExt = Path.GetExtension(existingURI);
            string sMiscFileName = string.Concat(sFileNameNoExt,
                GeneralHelpers.EXTENSION_MISC, sExt);
            sMiscDocURI = Path.Combine(sDirectory, sMiscFileName);
            return sMiscDocURI;
        }
    }
}
