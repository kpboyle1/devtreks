using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:     front-end zip utility to new system.compression library
    ///Author:		www.devtreks.org
    ///Date:		2016, July
    ///References:	MSDN: When you create a new entry, the file is compressed and added to the zip package. The CreateEntry method enables you to specify a directory hierarchy when adding the entry. You include the relative path of the new entry within the zip package. For example, creating a new entry with a relative path of AddedFolder\NewFile.txt creates a compressed text file in a directory named AddedFolder.
    ///If you reference the System.IO.Compression.FileSystem assembly in your project, you can access 
    ///three extension methods(from the ZipFileExtensions class) for the ZipArchive class: 
    ///CreateEntryFromFile, CreateEntryFromFile, and ExtractToDirectory.These extension methods enable 
    ///you to compress and decompress the contents of the entry to a file.The System.IO.Compression.FileSystem assembly is not available for Windows 8.x Store apps.In Windows 8.x Store apps, you can compress and decompress files by using the DeflateStream or GZipStream class, or you can use the Windows Runtime types Compressor and Decompressor.
    /// </summary>
    public class ZipIO
    {
        public bool ZipFiles(ContentURI uri, string zipFilePath,
            IDictionary<string, string> args)
        {
            bool bIsZipped = false;
            string errorMsg = string.Empty;
            PackageIO packIO = new PackageIO();
            packIO.CheckForPackageFileErrors(zipFilePath, args,
                ref errorMsg);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return bIsZipped;
            }
            //filesystem
            //don't want the temp directory info in the file path, disrupts subfolders (i.e. in packages)
            Directory.SetCurrentDirectory(Path.GetDirectoryName(zipFilePath));
            string sCurrentDirectory = FileStorageIO.GetDirectoryName(zipFilePath);
            //init the zip with the zip's file name
            using (FileStream zipToOpen = new FileStream(Path.GetFileName(zipFilePath), FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    string[] arrValues = { };
                    string sRelPartPath = string.Empty;
                    string sKey = string.Empty;
                    string sValue = string.Empty;
                    foreach (KeyValuePair<string, string> kvp in args)
                    {
                        sKey = kvp.Key;
                        sValue = kvp.Value;
                        if (string.IsNullOrEmpty(sKey) == false
                            && Helpers.FileStorageIO.URIAbsoluteExists(
                                uri, sKey))
                        {
                            //convert the absolute path of partFullPath into a path relative to the package root (getcurrentdirectory)
                            sRelPartPath = AppSettings.ConvertAbsPathToRelPath(sCurrentDirectory, sKey);
                            //will add files to the same directory as setcurrentdirectory
                            //2.0.0 change required adding a reference to System.IO.Compression.FileSystem
                            archive.CreateEntryFromFile(sKey, sRelPartPath);
                        }
                    }
                    bIsZipped = true;
                }
            }
            return bIsZipped;
        }
        
    }
    
}
