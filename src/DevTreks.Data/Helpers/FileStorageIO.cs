using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities storing files on cloud and web server file storage systems
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class FileStorageIO
    {
        /// <summary>
        /// Platform where DevTreks is deployed
        /// </summary>
        public enum PLATFORM_TYPES
        {
            webserver = 0,
            azure = 1,
            amazon = 2
        }
        public static PLATFORM_TYPES GetPlatformType(ContentURI uri)
        {
            string sWebRoot = uri.URIDataManager.DefaultRootWebStoragePath;
            PLATFORM_TYPES ePlatform = PLATFORM_TYPES.azure;
            ePlatform = GetPlatformType(sWebRoot);
            return ePlatform;
        }
        public static PLATFORM_TYPES GetPlatformType(string url)
        {
            PLATFORM_TYPES ePlatform = PLATFORM_TYPES.azure;
            if (url.Contains(".blob."))
            {
                ePlatform = PLATFORM_TYPES.azure;
            }
            else if (url.Contains("127.0.0.1"))
            {
                ePlatform = PLATFORM_TYPES.azure;
            }
            else if (url.Contains("www.devtreks.org"))
            {
                ePlatform = PLATFORM_TYPES.azure;
            }
            else if (url.Contains("localhost"))
            {
                ePlatform = PLATFORM_TYPES.webserver;
            }
            else
            {
                //if it's not azure or local its a deployed web site
                ePlatform = PLATFORM_TYPES.webserver;
            }
            return ePlatform;
        }
        public static bool IsFileSystemFile(string url)
        {
            bool bIsFileSystem = true;
            if (url.StartsWith("http"))
            {
                return false;
            }
            return bIsFileSystem;
        }
        public static string GetDelimiterForFileStorage(string uri)
        {
            string sDelimiter = Helpers.GeneralHelpers.FILE_PATH_DELIMITER;
            if (!Path.IsPathRooted(uri))
            {
                sDelimiter = Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER;
            }
            else
            {
                if (uri.Contains(Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER))
                {
                    sDelimiter = Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITER;
                }
            }
            return sDelimiter;
        }
        public static char[] GetCDelimiterForFileStorage(string uri)
        {
            char[] cDelimiter = Helpers.GeneralHelpers.FILE_PATH_DELIMITERS;
            if (!Path.IsPathRooted(uri))
            {
                cDelimiter = Helpers.GeneralHelpers.WEBFILE_PATH_DELIMITERS;
            }
            return cDelimiter;
        }
        public static string GetDelimiterForFileStorage(ContentURI uri)
        {
            string sPathDelimiter = GeneralHelpers.FILE_PATH_DELIMITER;
            PLATFORM_TYPES ePlatform = GetPlatformType(uri);
            if (ePlatform == PLATFORM_TYPES.azure)
            {
                sPathDelimiter = GeneralHelpers.WEBFILE_PATH_DELIMITER;
            }
            return sPathDelimiter;
        }
        public static bool URIAbsoluteExists(ContentURI uri, string fullURIPath)
        {
            bool bURIExists = false;
            if (!Path.HasExtension(fullURIPath))
            {
                return false;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                if (File.Exists(fullURIPath))
                {
                    bURIExists = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.webserver)
                {
                    //176 supported dataurl with calculators
                    //this opens the file -which could interfere with the next step that also opens the file
                    //so just return the error when the file is not opened twice
                    //bURIExists = WebServerFileIO.Exists(fullURIPath);
                    bURIExists = true;
                }
                else if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    //this also opens remote files and won't scale
                    //full uri paths had to be retrieved from cloud or web storage
                    bURIExists = azureIO.BlobExists(fullURIPath);
                }
            }
            return bURIExists;
        }
        public static bool FileExists(ContentURI uri, string fullURIPath)
        {
            bool bURIExists = false;
            if (!Path.HasExtension(fullURIPath))
            {
                return false;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                if (File.Exists(fullURIPath))
                {
                    bURIExists = true;
                }
            }
            else if (!fullURIPath.StartsWith("http"))
            {
                if (File.Exists(fullURIPath))
                {
                    bURIExists = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(fullURIPath);
                if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.webserver)
                {
                    //176 supported dataurl with calculators
                    //this opens the file -which could interfere with the next step that also opens the file
                    //so just return the error when the file is opened not twice
                    //bURIExists = WebServerFileIO.Exists(fullURIPath);
                    bURIExists = true;
                }
                else if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    //this also opens remote files and won't scale
                    //full uri paths had to be retrieved from cloud or web storage
                    bURIExists = azureIO.BlobExists(fullURIPath);
                }
            }
            return bURIExists;
        }
        public static bool FileExists(string fullURIPath)
        {
            bool bURIExists = false;
            if (!Path.HasExtension(fullURIPath))
            {
                return false;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                if (File.Exists(fullURIPath))
                {
                    bURIExists = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(fullURIPath);
                if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.webserver)
                {
                    //176 supported dataurl with calculators
                    //this opens the file -which could interfere with the next step that also opens the file
                    //so just return the error when the file is opened not twice
                    //bURIExists = WebServerFileIO.Exists(fullURIPath);
                    bURIExists = true;
                }
                else if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    //can't init storage with filepath alone
                    //let it fail when retrieved is it doesn't exist
                    //possibly scalable replacement for URIAbsoluteExists
                    bURIExists = true;
                }
            }
            return bURIExists;
        }
        public static bool DirectoryExists(ContentURI uri, string fullURIPath)
        {
            bool bDirectoryExists = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                string sDirectoryName = Path.GetDirectoryName(fullURIPath);
                if (Directory.Exists(sDirectoryName) == true)
                {
                    bDirectoryExists = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    //directories are part of blob name -they'll be built when blob is saved
                    bDirectoryExists = true;
                }
            }
            return bDirectoryExists;
        }
        public static bool DirectoryCreate(ContentURI uri, string fullURIPath)
        {
            bool bDirectoryExists = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                string sDirectoryName = Path.GetDirectoryName(fullURIPath);
                if (Directory.Exists(sDirectoryName) == false)
                {
                    Directory.CreateDirectory(sDirectoryName);
                    bDirectoryExists = true;
                }
                else
                {
                    bDirectoryExists = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    //directories are part of blob name -they'll be built when blob is saved
                    bDirectoryExists = true;
                }
            }
            
            return bDirectoryExists;
        }

        public static string GetDirectoryName(string fullURIPath)
        {
            string sDirectoryName = string.Empty;
            if (Path.IsPathRooted(fullURIPath))
            {
                sDirectoryName = Path.GetDirectoryName(fullURIPath);
            }
            else
            {
                if (fullURIPath.StartsWith("http"))
                {
                    //directories are part of blob name 
                    if (Path.HasExtension(fullURIPath))
                    {
                        sDirectoryName = AzureIOAsync.GetDirectoryName(fullURIPath);
                    }
                    else
                    {
                        //it is a directory name
                        sDirectoryName = fullURIPath;
                    }
                    //blob directories end with a path delimiter
                    if (!sDirectoryName.EndsWith(GeneralHelpers.WEBFILE_PATH_DELIMITER))
                    {
                        sDirectoryName = string.Concat(sDirectoryName, GeneralHelpers.WEBFILE_PATH_DELIMITER);
                    }
                }
            }
            return sDirectoryName;
        }
        public static bool DeleteAndReplaceDirectory(ContentURI uri, string fullURIPath, 
            bool includeSubDirs)
        {
            bool bDirectoryIsDeleted = DeleteDirectory(uri, fullURIPath, includeSubDirs);
            DirectoryCreate(uri, fullURIPath);
            return bDirectoryIsDeleted;
        }
        public static bool DeleteDirectory(ContentURI uri, string fullURIPath, 
            bool includeSubDirs)
        {
            bool bDirectoryIsDeleted = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                bDirectoryIsDeleted = FileIO.DeleteDirectory(uri, fullURIPath, includeSubDirs);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (fullURIPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bDirectoryIsDeleted = azureIO.DeleteDirectory(uri, fullURIPath, includeSubDirs);
                }
            }
            return bDirectoryIsDeleted;
        }
        public static string GetDescendentDirectory(ContentURI uri, 
            string directoryPath, string directoryPattern)
        {
            string sDescDir = string.Empty;
            if (Path.IsPathRooted(directoryPath))
            {
                sDescDir = FileIO.GetDescendentDirectory(directoryPath, directoryPattern);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (directoryPath.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    sDescDir = azureIO.GetDescendentDirectory(directoryPath, directoryPattern);
                }
            }
            return sDescDir;
        }
        public static void CopyDirectories(ContentURI uri, 
            string fromDirectory, string toDirectory, 
            bool copySubDirs, bool needsNewSubDirectories)
        {
            PLATFORM_TYPES ePlatform = GetPlatformType(uri);
            if (Path.IsPathRooted(fromDirectory))
            {
                if (Path.IsPathRooted(toDirectory))
                {
                    FileIO.CopyDirectories(fromDirectory, toDirectory, 
                        copySubDirs, needsNewSubDirectories);
                }
                else
                {
                    if (ePlatform == PLATFORM_TYPES.azure)
                    {
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        azureIO.CopyDirectories(uri, 
                            fromDirectory, toDirectory,
                            copySubDirs, needsNewSubDirectories);
                    }
                }
            }
            else
            {
                if (fromDirectory.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    azureIO.CopyDirectories(uri, 
                        fromDirectory, toDirectory,
                            copySubDirs, needsNewSubDirectories);
                }
            }
        }
        public static async Task<bool> CopyDirectoriesAsync(ContentURI uri,
            string fromDirectory, string toDirectory, 
            bool copySubDirs, bool needsNewSubDirectories)
        {
            bool bHasCopied = false;
            PLATFORM_TYPES ePlatform = GetPlatformType(uri);
            if (Path.IsPathRooted(fromDirectory))
            {
                if (Path.IsPathRooted(toDirectory))
                {
                    bHasCopied = await FileIO.CopyDirectoriesAsync(
                        uri, fromDirectory, toDirectory,
                        copySubDirs, needsNewSubDirectories);
                }
                else
                {
                    if (ePlatform == PLATFORM_TYPES.azure)
                    {
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        bHasCopied = await azureIO.CopyDirectoriesAsync(
                            uri, fromDirectory, toDirectory,
                            copySubDirs, needsNewSubDirectories);
                    }
                }
            }
            else
            {
                if (fromDirectory.StartsWith("http")
                    && ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bHasCopied = await azureIO.CopyDirectoriesAsync(
                        uri, fromDirectory, toDirectory,
                        copySubDirs, needsNewSubDirectories);
                }
            }
            return bHasCopied;
        }
        
        public static string AddDirectoryToDirectoryPath(string fromDirectoryPath, string toDirectoryPath)
        {
            string sFullDirectoryPath = toDirectoryPath;
            //deal with when absoluteuripath ends with a delimiter or file name
            if (Path.HasExtension(toDirectoryPath))
            {
                sFullDirectoryPath = GetDirectoryName(toDirectoryPath);
            }
            char[] cDelimiter = FileStorageIO.GetCDelimiterForFileStorage(fromDirectoryPath);
            string sDelimiter = FileStorageIO.GetDelimiterForFileStorage(fromDirectoryPath);
            string sLastDirectoryName = string.Empty;
            if (fromDirectoryPath.EndsWith(sDelimiter))
            {
                sLastDirectoryName = GeneralHelpers.GetSubstringFromEnd(
                   fromDirectoryPath, cDelimiter, 2);
            }
            else
            {
                sLastDirectoryName = GeneralHelpers.GetSubstringFromEnd(
                    fromDirectoryPath, cDelimiter, 1);
            }
            string sToDelimiter = FileStorageIO.GetDelimiterForFileStorage(toDirectoryPath);
            string sLastDirectoryDelimited = string.Concat(sToDelimiter,
                sLastDirectoryName, sToDelimiter);
            if (!sFullDirectoryPath.EndsWith(sLastDirectoryDelimited))
            {
                sFullDirectoryPath = Path.Combine(sFullDirectoryPath, sLastDirectoryName);
                if (!sFullDirectoryPath.EndsWith(sToDelimiter))
                {
                    sFullDirectoryPath = string.Concat(sFullDirectoryPath, sToDelimiter);
                }
            }
            return sFullDirectoryPath;
        }
        

        public async Task<string> SaveBinaryStreamInURIAsync(ContentURI uri,
            Stream stream, string fullURIPath)
        {
            string sErrorMsg = string.Empty;
            bool bHasSaved = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                //bool 
                bHasSaved = await FileIO.WriteBinaryBlobFileAsync(fullURIPath, stream);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    //string
                    fullURIPath = await azureIO.SaveResourceURLInCloudAsync(fullURIPath, stream);
                    if (!string.IsNullOrEmpty(fullURIPath))
                    {
                        bHasSaved = true;
                    }
                }
            }
            if (!bHasSaved)
            {
                sErrorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEPOSTEDFILE");
            }
            return sErrorMsg;
        }
        public async Task<string> SaveXmlInURIAsync(ContentURI uri, 
            XmlReader reader, string fullURIPath)
        {
            string sErrorMsg = string.Empty;
            bool bFileHasSaved = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                if (GeneralHelpers.IsXmlFileExt(fullURIPath))
                {
                    XmlFileIO xmlIO = new XmlFileIO();
                    bFileHasSaved = await xmlIO.WriteFileXmlAsync(
                        uri, reader, fullURIPath);
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    //azure asynch is different than file system
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = await azureIO.WriteFileXmlAsync(reader, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                sErrorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEXML");
            }
            return sErrorMsg;
        }
        
        public bool SaveXmlInURI(ContentURI uri, XmlReader reader,
            string fullURIPath, out string errorMsg)
        {
            bool bFileHasSaved = false;
            errorMsg = string.Empty;
            if (Path.IsPathRooted(fullURIPath))
            {
                if (GeneralHelpers.IsXmlFileExt(fullURIPath))
                {
                    XmlFileIO xmlIO = new XmlFileIO();
                    xmlIO.WriteFileXml(uri, reader, fullURIPath, out errorMsg);
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        bFileHasSaved = true;
                    }
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = azureIO.SaveXmlInURI(reader, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                errorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEXML");
            }
            return bFileHasSaved;
        }
        public async Task SaveFilesAsync(ContentURI uri, XmlDocument doc,
            string fullURIPath, List<Stream> sourceStreams)
        {
            if (Path.IsPathRooted(fullURIPath))
            {
                //the xml does not need to be .xml (it could be .opf)
                XmlFileIO xmlIO = new XmlFileIO();
                await xmlIO.SaveFilesAsync(doc, fullURIPath, 
                    sourceStreams);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    await azureIO.SaveBlobsAsync(doc, fullURIPath,
                        sourceStreams);
                }
            }
        }
        public async Task SaveFileAsync(ContentURI uri, XmlDocument doc,
            string fullURIPath)
        {
            if (Path.IsPathRooted(fullURIPath))
            {
                //the xml does not need to be .xml (it could be .opf)
                XmlFileIO xmlIO = new XmlFileIO();
                await xmlIO.SaveFileAsync(doc, fullURIPath);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    await azureIO.SaveBlobAsync(doc, fullURIPath);
                }
            }
        }
        public bool SaveXmlInURI(ContentURI uri, XmlDocument doc,
            string fullURIPath, out string errorMsg)
        {
            bool bFileHasSaved = false;
            errorMsg = string.Empty;
            if (Path.IsPathRooted(fullURIPath))
            {
                //the xml does not need to be .xml (it could be .opf)
                XmlFileIO xmlIO = new XmlFileIO();
                xmlIO.WriteFileXml(uri, doc, fullURIPath, out errorMsg);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    bFileHasSaved = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = azureIO.SaveStringInURI(doc.OuterXml, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                errorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEXML");
            }
            return bFileHasSaved;
        }
        public async Task SaveHtmlTextURIAsync(ContentURI uri, StringWriter writer,
            string fullURIPath)
        {
            bool bFileHasSaved = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                await fileIO.WriteHtmlTextFileAsync(fullURIPath, writer);
                bFileHasSaved = true;
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    //azure asynch not the same as filesystem
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    await azureIO.WriteHtmlTextFileAsync(writer, fullURIPath);
                    bFileHasSaved = true;
                }
            }
            if (!bFileHasSaved)
            {
                uri.ErrorMessage = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEHTMLORTEXT");
            }
        }
        public string SaveTextURI(ContentURI uri, StringWriter writer,
            string fullURIPath)
        {
            bool bFileHasSaved = false;
            string sErrorMsg = string.Empty;
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                bFileHasSaved = fileIO.WriteTextFile(fullURIPath, writer);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    //azure async not the same as filesystem
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = azureIO.SaveXmlWriterInURI(writer, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                sErrorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEHTMLORTEXT");
            }
            return sErrorMsg;
        }
        public async Task<string> SaveTextURIAsync(ContentURI uri,
            StringWriter writer, string fullURIPath)
        {
            bool bFileHasSaved = false;
            string sErrorMsg = string.Empty;
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                bFileHasSaved = await fileIO.WriteTextFileAsync(fullURIPath, writer);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    //azure asynch not the same as filesystem
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = await azureIO.SaveXmlWriterInURIAsync(writer, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                sErrorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEHTMLORTEXT");
            }
            return sErrorMsg;
        }
        public bool SaveHtmlTextURI(ContentURI uri, StringWriter writer,
            string fullURIPath)
        {
            bool bFileHasSaved = false;
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                bFileHasSaved = fileIO.WriteHtmlTextFile(fullURIPath, writer);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = azureIO.SaveHtmlWriterInURI(writer, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                uri.ErrorMessage = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEHTMLORTEXT");
            }
            return bFileHasSaved;
        }
        
        public bool SaveTextURI(ContentURI uri, string fullURIPath, string text, 
            out string errorMsg)
        {
            bool bFileHasSaved = false;
            errorMsg = string.Empty;
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                fileIO.WriteTextFile(fullURIPath, text);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    bFileHasSaved = true;
                }
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    //don't use https urls or won't work)
                    string sFilePath
                        = AppSettings.ConvertPathFileandWeb(uri, fullURIPath);
                    if (Path.IsPathRooted(sFilePath))
                    {
                        FileIO fileIO = new FileIO();
                        fileIO.WriteTextFile(sFilePath, text);
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            bFileHasSaved = true;
                        }
                    }
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    bFileHasSaved = azureIO.SaveStringInURI(text, fullURIPath);
                }
            }
            if (!bFileHasSaved)
            {
                errorMsg = DevTreks.Exceptions.Errors.GetMessage("FILESTORAGE_FILENOSAVEHTMLORTEXT");
            }
            return bFileHasSaved;
        }

        //public async Task SaveHtmlURIToWriterAsync(ContentURI uri,
        //    XmlWriter writer, string fullURIPath)
        //{
        //    //reduce memory use of html strings
        //    string sErrorMsg = string.Empty;
        //    await writer.WriteRawAsync(await ReadTextAsync(uri, fullURIPath));
        //    uri.ErrorMessage += sErrorMsg;
        //}
        //public void SaveHtmlURIToWriter(ContentURI uri,
        //    XmlWriter writer, string fullURIPath)
        //{
        //    //reduce memory use of html strings
        //    string sErrorMsg = string.Empty;
        //    writer.WriteRaw(ReadText(uri, fullURIPath, out sErrorMsg));
        //    uri.ErrorMessage += sErrorMsg;
        //}
        public async Task SaveHtmlURIToWriterAsync(ContentURI uri,
            StringWriter writer, string fullURIPath)
        {
            //reduce memory use of html strings
            string sErrorMsg = string.Empty;
            await writer.WriteAsync(await ReadTextAsync(uri, fullURIPath));
            uri.ErrorMessage += sErrorMsg;
        }
        public void SaveHtmlURIToWriter(ContentURI uri,
            StringWriter writer, string fullURIPath)
        {
            //reduce memory use of html strings
            string sErrorMsg = string.Empty;
            writer.Write(ReadText(uri, fullURIPath, out sErrorMsg));
            uri.ErrorMessage += sErrorMsg;
        }
        public string ReadText(ContentURI uri,
            string fullURIPath, out string errorMsg)
        {
            errorMsg = string.Empty;
            string sTextString = string.Empty;
            if (URIAbsoluteExists(uri, fullURIPath) == false)
            {
                return sTextString;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                sTextString = fileIO.ReadText(uri, fullURIPath);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                //the webserver code is not debugged
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    WebServerFileIO webIO = new WebServerFileIO();
                    sTextString = webIO.ReadText(fullURIPath);
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    sTextString =  azureIO.SaveCloudFileInString(fullURIPath);
                }
            }
            return sTextString;
        }
        public async Task<List<string>> ReadLinesAsync(ContentURI uri, 
            string fullURIPath)
        {
            List<string> lines = new List<string>();
            if (URIAbsoluteExists(uri, fullURIPath) == false)
            {
                return lines;
            }
            PLATFORM_TYPES ePlatform = GetPlatformType(uri);
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                lines = await fileIO.ReadLinesAsync(fullURIPath);
            }
            else
            {
                //176 starting using dataurl prop on localhost
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    WebServerFileIO wsfileIO = new WebServerFileIO();
                    lines = await wsfileIO.ReadLinesAsync(fullURIPath);
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    lines = await azureIO.ReadLinesAsync(fullURIPath);
                }
            }
            return lines;
        }

        public async Task<string> InvokeHttpRequestResponseServiceAsync(string fullURIPath)
        {
            string sResponse = string.Empty;
            //use configure await to make sure the response is returned to algo
            //return await WebServerFileIO.InvokeHttpRequestResponseServiceAsync().ConfigureAwait(false);
            return sResponse;
            //if (URIAbsoluteExists(fullURIPath) == false)
            //{
            //    return lines;
            //}
            //PLATFORM_TYPES ePlatform = GetPlatformType();
            //if (Path.IsPathRooted(fullURIPath))
            //{
            //    FileIO fileIO = new FileIO();
            //    lines = await fileIO.ReadLinesAsync(fullURIPath);
            //}
            //else
            //{
            //    //176 starting using dataurl prop on localhost
            //    if (ePlatform == PLATFORM_TYPES.webserver)
            //    {
            //        WebServerFileIO wsfileIO = new WebServerFileIO();
            //        lines = await wsfileIO.ReadLinesAsync(fullURIPath);
            //    }
            //    else if (ePlatform == PLATFORM_TYPES.azure)
            //    {
            //        AzureIOAsync azureIO = new AzureIOAsync(uri);
            //        lines = await azureIO.ReadLinesAsync(fullURIPath);
            //    }
            //}
        }
        public async Task<string> InvokeHttpRequestResponseService(
            ContentURI uri, string baseURL, string apiKey,
            string inputFileLocation, string outputFileLocation, string script)
        {
            string sResponseMsg = string.Empty;
            PLATFORM_TYPES ePlatform = GetPlatformType(baseURL);
            if (Path.IsPathRooted(inputFileLocation))
            {
                //use configure await to make sure the response is returned to algo
                sResponseMsg = await WebServerFileIO.InvokeHttpRequestResponseService(baseURL, apiKey,
                        inputFileLocation, outputFileLocation, script).ConfigureAwait(false);
            }
            else
            {
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    //use configure await to make sure the response is returned to algo
                    sResponseMsg = await WebServerFileIO.InvokeHttpRequestResponseService(baseURL, apiKey,
                        inputFileLocation, outputFileLocation, script).ConfigureAwait(false);
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    sResponseMsg = await azureIO.InvokeHttpRequestResponseService(baseURL, apiKey,
                        inputFileLocation, outputFileLocation, script).ConfigureAwait(false);
                }
            }
            return sResponseMsg;
        }
        public async Task<string> InvokeHttpRequestResponseService2(
            ContentURI uri, string baseURL, string apiKey,
            string inputBlob1Location, string inputBlob2Location,
            string outputBlob1Location, string outputBlob2Location)
        {
            string sResponseMsg = string.Empty;
            PLATFORM_TYPES ePlatform = GetPlatformType(baseURL);
            if (Path.IsPathRooted(inputBlob1Location))
            {
                //use configure await to make sure the response is returned to algo
                sResponseMsg = await WebServerFileIO.InvokeHttpRequestResponseService2(baseURL, apiKey,
                        inputBlob1Location, inputBlob2Location, outputBlob1Location, outputBlob2Location).ConfigureAwait(false);
            }
            else
            {
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    //use configure await to make sure the response is returned to algo
                    sResponseMsg = await WebServerFileIO.InvokeHttpRequestResponseService2(baseURL, apiKey,
                        inputBlob1Location, inputBlob2Location, outputBlob1Location, outputBlob2Location).ConfigureAwait(false);
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    sResponseMsg = await azureIO.InvokeHttpRequestResponseService2(baseURL, apiKey,
                        inputBlob1Location, inputBlob2Location, outputBlob1Location, outputBlob2Location).ConfigureAwait(false);
                }
            }
            return sResponseMsg;
        }
        public List<string> ReadLines(ContentURI uri, 
            string fullURIPath, out string errorMsg)
        {
            errorMsg = string.Empty;
            List<string> lines = new List<string>();
            if (URIAbsoluteExists(uri, fullURIPath) == false)
            {
                return lines;
            }
            PLATFORM_TYPES ePlatform = GetPlatformType(uri);
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                lines = fileIO.ReadLines(fullURIPath);
            }
            else
            {
                //176 starting using dataurl prop on localhost
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    WebServerFileIO wsfileIO = new WebServerFileIO();
                    lines = wsfileIO.ReadLines(fullURIPath);
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    lines = azureIO.ReadLines(fullURIPath);
                }
            }
            return lines;
        }
        public async Task<string> ReadTextAsync(ContentURI uri, 
            string fullURIPath)
        {
            string sTextString = string.Empty;
            if (URIAbsoluteExists(uri, fullURIPath) == false)
            {
                return sTextString;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                FileIO fileIO = new FileIO();
                sTextString = await fileIO.ReadTextAsync(uri, fullURIPath);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.webserver)
                {
                    //this is not debugged
                    WebServerFileIO webIO = new WebServerFileIO();
                    sTextString = await webIO.ReadTextAsync(fullURIPath);
                }
                else if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    sTextString = await azureIO.ReadTextAsync(fullURIPath);
                }
            }
            return sTextString;
        }
        public static XElement LoadXmlElement(ContentURI uri, 
            string fullURIPath)
        {
            XElement el = null;
            //correctly retrieves blob or filesys reader
            XmlReader reader = GetXmlReader(uri, fullURIPath);
            if (reader != null)
            {
                using (reader)
                {
                    el = XElement.Load(reader);
                }
            }
            return el;
        }
        public static XmlReader GetXmlReader(ContentURI uri, string fullURIPath)
        {
            XmlReader reader = null;
            if (URIAbsoluteExists(uri, fullURIPath)
                == false)
            {
                return reader;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                reader = XmlFileIO.GetXmlFromFile(fullURIPath);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(fullURIPath);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    reader = azureIO.GetXmlFromURI(fullURIPath);
                }
            }
            return reader;
        }

        public static async Task<XmlReader> GetXmlReaderAsync(ContentURI uri, 
            string fullURIPath)
        {
            XmlReader reader = null;
            if (URIAbsoluteExists(uri, fullURIPath) == false)
            {
                return reader;
            }
            if (Path.IsPathRooted(fullURIPath))
            {
                //this reader can be read async
                reader = XmlFileIO.GetXmlFromFileAsync(fullURIPath);
            }
            else
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    reader = await azureIO.GetXmlFromURIAsync(fullURIPath);
                }
            }
            return reader;
        }
        public static async Task<XElement> LoadXmlElementAsync(ContentURI uri,
            string fullURIPath)
        {
            XElement el = null;
            //correctly retrieves blob or filesys reader
            XmlReader reader = await GetXmlReaderAsync(uri, fullURIPath);
            if (reader != null)
            {
                using (reader)
                {
                    el = XElement.Load(reader);
                }
            }
            return el;
        }
        //deprecated in 160 in favor of async
        public static void CopyURIs(ContentURI uri, 
            string fromURIPath, string toURIPath)
        {
            //some files need to be copied from root web content
            if (URIAbsoluteExists(uri, fromURIPath) == true
                && fromURIPath.Equals(toURIPath) == false
                && (!string.IsNullOrEmpty(toURIPath)))
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (Path.IsPathRooted(fromURIPath))
                {
                    if (Path.IsPathRooted(toURIPath))
                    {
                        FileIO.CopyFiles(uri, fromURIPath, toURIPath);
                    }
                    else
                    {
                        if (ePlatform == PLATFORM_TYPES.azure)
                        {
                            AzureIOAsync azureIO = new AzureIOAsync(uri);
                            string sBlobFullURI = string.Empty;
                            azureIO.SaveCloudFile(toURIPath, fromURIPath, out sBlobFullURI);
                        }
                    }
                }
                else
                {
                    if (Path.IsPathRooted(toURIPath))
                    {
                        if (ePlatform == PLATFORM_TYPES.azure)
                        {
                            //2.0.0 debugs azure blob storage using localhost
                            ePlatform = GetPlatformType(fromURIPath);
                            if (ePlatform == PLATFORM_TYPES.azure)
                            {
                                AzureIOAsync azureIO = new AzureIOAsync(uri);
                                azureIO.SaveCloudFileInFileSystem(fromURIPath, toURIPath);
                            }
                            else
                            {
                                if (ePlatform == PLATFORM_TYPES.webserver)
                                {
                                    WebServerFileIO webIO = new WebServerFileIO();
                                    webIO.CopyWebFileToFileSystemAsync(fromURIPath, toURIPath);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ePlatform == PLATFORM_TYPES.azure)
                        {
                            AzureIOAsync azureIO = new AzureIOAsync(uri);
                            azureIO.CopyBlob(uri, fromURIPath, toURIPath);
                        }
                    }
                }
            }
        }
        public static async Task<bool> CopyURIsAsync(ContentURI uri, 
            string fromURIPath, string toURIPath)
        {
            bool bHasCopied = false;
            //2.0.0 refactored from URIAbsoluteExists(fromURIPath) because azure uses localhost to debug
            if (FileExists(uri, fromURIPath) == true
                && fromURIPath.Equals(toURIPath) == false
                && (!string.IsNullOrEmpty(toURIPath)))
            {
                PLATFORM_TYPES ePlatform = GetPlatformType(uri);
                if (Path.IsPathRooted(fromURIPath))
                {
                    if (Path.IsPathRooted(toURIPath))
                    {
                        bHasCopied = await FileIO.CopyFilesAsync(
                            uri, fromURIPath, toURIPath);
                    }
                    else
                    {
                        if (ePlatform == PLATFORM_TYPES.azure)
                        {
                            //2.0.0 debugs azure blob storage using localhost
                            //note topath and frompath are reversed below
                            ePlatform = GetPlatformType(toURIPath);
                            if (ePlatform == PLATFORM_TYPES.azure)
                            {
                                AzureIOAsync azureIO = new AzureIOAsync(uri);
                                bHasCopied = await azureIO.SaveFileinCloudAsync(fromURIPath, toURIPath);
                            }
                            else
                            {
                                if (ePlatform == PLATFORM_TYPES.webserver)
                                {
                                    WebServerFileIO webIO = new WebServerFileIO();
                                    bHasCopied = await webIO.CopyWebFileToFileSystemAsync(toURIPath, fromURIPath);
                                }
                            }
                        }
                        else
                        {
                            //web server doesn't handle https and should use filesystem
                        }
                    }
                }
                else
                {
                    if (Path.IsPathRooted(toURIPath))
                    {
                        if (ePlatform == PLATFORM_TYPES.azure)
                        {
                            //2.0.0 debugs azure blob storage using localhost
                            ePlatform = GetPlatformType(fromURIPath);
                            if (ePlatform == PLATFORM_TYPES.azure)
                            {
                                AzureIOAsync azureIO = new AzureIOAsync(uri);
                                bHasCopied = await azureIO.SaveCloudFileAsync(fromURIPath, toURIPath);
                            }
                            else
                            {
                                if (ePlatform == PLATFORM_TYPES.webserver)
                                {
                                    WebServerFileIO webIO = new WebServerFileIO();
                                    bHasCopied = await webIO.CopyWebFileToFileSystemAsync(fromURIPath, toURIPath);
                                }
                            }
                        }
                        else
                        {
                            //web server doesn't handle https and should use filesystem

                        }
                    }
                    else
                    {
                        if (ePlatform == PLATFORM_TYPES.azure)
                        {
                            AzureIOAsync azureIO = new AzureIOAsync(uri);
                            bHasCopied = await azureIO.CopyBlobAsync(
                                uri, fromURIPath, toURIPath);
                        }
                        else
                        {
                            //web server doesn't handle https and should use filesystem

                        }
                    }
                }
            }
            return bHasCopied;
        }
        public static bool DeleteURI(ContentURI uri, string deleteURIPath)
        {
            bool bIsDeleted = false;
            if (URIAbsoluteExists(uri, deleteURIPath) == true
                && (!string.IsNullOrEmpty(deleteURIPath)))
            {
                if (Path.IsPathRooted(deleteURIPath))
                {
                    FileIO.DeleteFile(uri, deleteURIPath);
                    bIsDeleted = true;
                }
                else
                {
                    PLATFORM_TYPES ePlatform 
                        = GetPlatformType(uri);
                    if (ePlatform == PLATFORM_TYPES.azure)
                    {
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        bIsDeleted = azureIO.DeleteBlob(deleteURIPath);
                    }
                }
            }
            return bIsDeleted;
        }
        public static bool DeleteURIsContainingSubstring(ContentURI uri, 
            string changedURIPath, string subString)
        {
            bool bIsDeleted = false;
            if (URIAbsoluteExists(uri, changedURIPath))
            {
                //if one is rooted the other is too (no mix between blob and file system storage)
                if (Path.IsPathRooted(changedURIPath))
                {
                    FileIO.DeleteDirectoryFilesContainingSubstring(
                        uri, changedURIPath, subString);
                    bIsDeleted = true;
                }
                else
                {
                    PLATFORM_TYPES ePlatform
                        = GetPlatformType(uri);
                    if (ePlatform == PLATFORM_TYPES.azure)
                    {
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        bIsDeleted = azureIO.DeleteBlob(changedURIPath);
                    }
                }
            }
            return bIsDeleted;
        }
        public static void DeleteURIsWithChangedNames(ContentURI uri, 
            string changedURIPath, string oldURIName)
        {
            //if one is rooted the other is too (no mix between blob and file system storage)
            if (Path.IsPathRooted(changedURIPath))
            {
                FileIO.DeleteFilesWithChangedNames(changedURIPath, oldURIName);
            }
            else
            {
                PLATFORM_TYPES ePlatform
                    = GetPlatformType(uri);
                if (ePlatform == PLATFORM_TYPES.azure)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    azureIO.DeleteBlob(changedURIPath);
                }
            }
        }
        public static void MoveURIs(ContentURI uri, string fromFile, string toFile)
        {
            if (URIAbsoluteExists(uri, fromFile) == true
                && fromFile.Equals(toFile) == false
                && (!string.IsNullOrEmpty(fromFile))
                && (!string.IsNullOrEmpty(toFile)))
            {
                if (Path.IsPathRooted(fromFile))
                {
                    if (Path.IsPathRooted(toFile))
                    {
                        FileIO.MoveFiles(uri, fromFile, toFile);
                    }
                    else
                    {
                        //file storage to blob storage
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        azureIO.MoveFileToBlob(fromFile, toFile);
                        //delete the file
                        FileIO.DeleteFile(uri, fromFile);
                    }
                }
                else
                {
                    PLATFORM_TYPES ePlatform
                        = GetPlatformType(uri);
                    if (ePlatform == PLATFORM_TYPES.azure)
                    {
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        azureIO.MoveBlob(fromFile, toFile);
                    }
                }
            }
        }
        public static bool File1IsNewer(ContentURI uri, 
            string file1Path, string file2Path)
        {
            bool bXmlIsNewer = false;
            if (FileStorageIO.URIAbsoluteExists(uri, file1Path)
                && FileStorageIO.URIAbsoluteExists(uri, file2Path))
            {
                DateTime xmlFileTime 
                    = GetLastWriteTimeUtc(uri, file1Path);
                DateTime xhtmlFileTime 
                    = GetLastWriteTimeUtc(uri, file2Path);
                if (xmlFileTime > xhtmlFileTime)
                {
                    //rule 3: if the xmldoc is older than than the html, make a new html
                    bXmlIsNewer = true;
                }
            }
            return bXmlIsNewer;
        }
        public static DateTime GetLastWriteTimeUtc(ContentURI uri, 
            string fileURIPath)
        {
            DateTime date = GeneralHelpers.GetDateShortNow();
            if (URIAbsoluteExists(uri, fileURIPath) == true)
            {
                if (Path.IsPathRooted(fileURIPath))
                {
                    date = File.GetLastWriteTimeUtc(fileURIPath);
                }
                else
                {
                    PLATFORM_TYPES ePlatform
                        = GetPlatformType(uri);
                    if (ePlatform == PLATFORM_TYPES.azure)
                    {
                        AzureIOAsync azureIO = new AzureIOAsync(uri);
                        CloudBlockBlob blob = azureIO.GetBlob(fileURIPath);
                        if (blob != null)
                        {
                            //have to fetch the attributes prior to reading them
                            blob.FetchAttributes();
                            DateTimeOffset blobdate = blob.Properties.LastModified.Value;
                            //165 fix
                            date = blobdate.UtcDateTime;
                        }
                    }
                }
            }
            return date;
        }
        public static string GetResourceURIPath(ContentURI uri, string existingURIPath)
        {
            string sURIPath = string.Empty;
            string sErrorMsg = string.Empty;
            sURIPath = GetResourceURIPath(uri, 
                existingURIPath, ref sErrorMsg);
            uri.ErrorMessage = sErrorMsg;
            return sURIPath;
        }
        public static string GetResourceURIPath(ContentURI uri,
            string existingURIPath, ref string errorMsg)
        {
            string sURIPath = string.Empty;
            PLATFORM_TYPES ePlatform
                    = GetPlatformType(uri);
            if (ePlatform == PLATFORM_TYPES.azure)
            {
                AzureIOAsync azureIO = new AzureIOAsync(uri);
                sURIPath = azureIO.GetResourceURIPath(existingURIPath, ref errorMsg);
            }
            else
            {
                if (URIAbsoluteExists(uri, existingURIPath))
                {
                    sURIPath = existingURIPath;
                }
                else
                {
                    errorMsg
                        = DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
                        string.Empty, "RESOURCES_NOFILE");
                }
            }
            return sURIPath;
        }
        //public static string GetResourceURIPath(string existingURIPath, ref string errorMsg)
        //{
        //    string sURIPath = string.Empty;
        //    PLATFORM_TYPES ePlatform
        //            = GetPlatformType(existingURIPath);
        //    if (ePlatform == PLATFORM_TYPES.azure)
        //    {
        //        AzureIOAsync azureIO = new AzureIOAsync(uri);
        //        sURIPath = azureIO.GetResourceURIPath(existingURIPath, ref errorMsg);
        //    }
        //    else
        //    {
        //        if (URIAbsoluteExists(existingURIPath))
        //        {
        //            sURIPath = existingURIPath;
        //        }
        //        else
        //        {
        //            errorMsg
        //                = DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
        //                string.Empty, "RESOURCES_NOFILE");
        //        }
        //    }
        //    return sURIPath;
        //}
    }
}
