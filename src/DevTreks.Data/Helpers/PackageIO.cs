using System.Collections.Generic;
//using System.IO.Packaging;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DevTreks.Data.Helpers
{
    public class PackageIO
    {
        /// <summary>  
        ///Purpose:		utilites for making ebook packages (openxml, idpf ...)
        ///Author:		www.devtreks.org
        ///Date:		2016, May
        ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
        ///NOTES        
        ///             1. The current version does not organize the ebooks. Some early 
        ///             experimentation showed how to do it, but its not considered 
        ///             as critical as getting the existing features debugged and performant.
        /// </summary>
  
        public enum DIGITAL_SIGNATURE_TYPES
        {
            none    = 0,
            all     = 1
        }
        //css and scripts shared by all html files
        public const string HEADERS_KEYNAME = "headers";
        //thes uris are taken from a sdk example
        private const string _openxmPackageRelationshipType = "http://schemas.openxmlformats.org/package/2007/relationships/sdk-samples/htmx/root-html";
        private const string _openXmResourceRelationshipType = "http://schemas.openxmlformats.org/package/2007/relationships/sdk-samples/htmx/required-resource";
        //part name of the signatureOrigin relationship.
        private const string _openxmlDigitalSignatureUri = "/package/services/digital-signature/_rels/origin.psdsor.rels";
        //current directory
        private string CurrentDirectory { get; set; }

        public static string GetPackageContentRoot(string rootPackageDirectory)
        {
            //0.8.7 switched from relative paths to full web paths
            string sContentPackageDirectory = rootPackageDirectory;
            return sContentPackageDirectory;
        }
        public bool PackageFiles(ContentURI uri, string packageFilePathName,
            string packageType, string digitalSignatureType,
            IDictionary<string, string> args)
        {
            bool bIsZipped = false;
            //add the error msg to the uri
            string sErrorMsg = string.Empty;
            ZipIO zipIO = new ZipIO();
            if (packageType == AppHelpers.Resources.PACKAGE_TYPES.plainzip.ToString())
            {
                //160 went to 100% zip until need arises for more
                bIsZipped = zipIO.ZipFiles(uri, packageFilePathName, args);
                //delete those files (don't delete them in the zipIO because of
                //error message about deleting the parent directory contains open files
                string sKey = string.Empty;
                string sValue = string.Empty;
                foreach (KeyValuePair<string, string> kvp in args)
                {
                    sKey = kvp.Key;
                    sValue = kvp.Value;
                    if (Path.IsPathRooted(sKey))
                    {
                        FileIO.DeleteFile(uri, sKey);
                    }
                }
            }
            //else if (packageType == AppHelpers.Resources.PACKAGE_TYPES.openxml.ToString())
            //{
            //    bIsZipped = await PackageOpenXmlFiles(packageFilePathName,
            //        digitalSignatureType, args);
            //}
            //else if (packageType == AppHelpers.Resources.PACKAGE_TYPES.idpf.ToString())
            //{
            //    //create and add the idpf-specific files (add to args for zipping)
            //    EPub epub = new EPub();
            //    sErrorMsg = await epub.PackageIDPFFiles(uri, packageFilePathName,
            //        args);
            //    //zip the package
            //    sErrorMsg = await zipIO.ZipFiles(packageFilePathName,
            //        args);
            //}
            //else if (packageType == AppHelpers.Resources.PACKAGE_TYPES.nimas.ToString())
            //{
            //    PackageNIMASFiles(arrZipArgs, out sErrorMessage);
            //}
            else
            {
                //160 went to 100% zip until need arises for more
                bIsZipped = zipIO.ZipFiles(uri, packageFilePathName, args);
                ////default is idpf
                //PackageIDPFFiles(arrZipArgs, out sErrorMessage);
            }
            return bIsZipped;
        }
        public bool PackageOpenXmlFiles(string packageFilePathName,
            string digitalSignatureType, IDictionary<string, string> args,
            out string errorMsg)
        {
            bool bIsZipped = false;
            errorMsg = string.Empty;
            CheckForPackageFileErrors(packageFilePathName, args, ref errorMsg);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return bIsZipped;
            }
            //package parts have a value with the filename of packageFilePathName
            string sPackagePartValue = Path.GetFileName(packageFilePathName);
            //files are found in this directory and it's subfolders
            CurrentDirectory = FileStorageIO.GetDirectoryName(packageFilePathName);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(packageFilePathName));
            //using (Package oPack = Package.Open(Path.GetFileName(packageFilePathName),
            //    FileMode.Create))
            //{
            //    //make a collection of std css and script files to relate to html files
            //    List<Uri> colHeaderUris = new List<Uri>();
            //    AddHtmlHeadersToPackage(oPack, args, colHeaderUris,
            //        errorMsg);
            //    string sKey = string.Empty;
            //    string sValue = string.Empty;
            //    //parallel didn't work here
            //    foreach (KeyValuePair<string, string> kvp in args)
            //    {
            //        sKey = kvp.Key;
            //        sValue = kvp.Value;
            //        if (sValue == sPackagePartValue)
            //        {
            //            if (string.IsNullOrEmpty(sKey) == false
            //                && Helpers.FileStorageIO.URIAbsoluteExists(sKey))
            //            {
            //                //these are all package parts
            //                PackagePart oPackPart = null;
            //                bool bIsPackagePart = AddPackagePart(oPack, sKey,
            //                    out oPackPart, errorMsg);
            //                if (bIsPackagePart)
            //                {
            //                    AddPackageRelationship(oPack, oPackPart.Uri);
            //                    //relate to std DevTreks headers
            //                    RelateHtmlHeadersToPart(oPack, oPackPart,
            //                        colHeaderUris, errorMsg);
            //                    //add resources and relationships
            //                    AddandRelateResource(sKey, args, oPack,
            //                        oPackPart, errorMsg);
            //                }
            //            }
            //        }
            //    }
            //    //save all of the content
            //    oPack.Flush();
            //    if (digitalSignatureType == DIGITAL_SIGNATURE_TYPES.all.ToString())
            //    {
            //        //digitally sign all the parts and relationships in the package.
            //        SignAllParts(oPack);
            //    }
            //    bIsZipped = true;
            //}//closes, saves, and disposes of Package
            return bIsZipped;
        }
        public void CheckForPackageFileErrors(string packageFilePathName,
            IDictionary<string, string> args, ref string errorMsg)
        {
            if (args.Count < 2)
                errorMsg = "Please select files or directories to zip";
            if (!packageFilePathName.EndsWith(".zip"))
                errorMsg = "The package file must end with .zip";
        }
        //private bool AddPackagePart(Package pack, 
        //    string partFullPath, out PackagePart packPart, string errorMsg)
        //{
        //    bool bIsPackagePart = false;
        //    packPart = null;
        //    //convert system path and file names to Part URIs (they are relative to package root)
        //    //convert the absolute path of partFullPath into a path relative to the package root (getcurrentdirectory)
        //    string sRelPartPath = GeneralHelpers.ConvertAbsPathToRelPath(CurrentDirectory, partFullPath);
        //    //string sRelPartPath = GeneralHelpers.ConvertAbsPathToRelPath(Directory.GetCurrentDirectory(), partFullPath);
        //    Uri uriPart = PackUriHelper.CreatePartUri(new Uri(sRelPartPath, UriKind.Relative));
        //    if (pack.PartExists(uriPart) == false)
        //    {
        //        if (Helpers.FileStorageIO.URIAbsoluteExists(partFullPath))
        //        {
        //            //set the media type used in createpart()
        //            string sMediaType = AppHelpers.Resources.GetMimeTypeFromFileExt(Path.GetExtension(partFullPath).ToLower(), ref errorMsg);
        //            //for now, allow the package to be built skipping over some file types
        //            errorMsg = string.Empty;
        //            // Use normal compression unless the data that has
        //            // already been compressed or is an "octet-stream".
        //            CompressionOption oCompression = CompressionOption.Normal;
        //            if (sMediaType.StartsWith("image")
        //                || sMediaType.StartsWith("video")
        //                || sMediaType.StartsWith("audio")
        //                || sMediaType.StartsWith("application/octet-stream"))
        //                oCompression = CompressionOption.NotCompressed;
        //            //add the document part to the package
        //            packPart = pack.CreatePart(uriPart, sMediaType, oCompression);
        //            //copy the data to the document part
        //            using (FileStream fileStream = new FileStream(partFullPath,
        //                FileMode.Open, FileAccess.Read))
        //            {
        //                FileIO.CopyStream(fileStream, packPart.GetStream());
        //            }
        //            bIsPackagePart = true;
        //        }
        //    }
        //    return bIsPackagePart;
        //}
        //private void AddPackageRelationship(Package pack,
        //    Uri partUri)
        //{
        //    //add a package relationship to the document part
        //    pack.CreateRelationship(partUri, TargetMode.Internal, _openxmPackageRelationshipType);
        //}
        //private static void AddResourceRelationship(Package pack,
        //    Uri partUri, Uri resourceUri)
        //{
        //    //add a resource relationship to the document part at the packlevel?
        //    //resources are always in subfolders
        //    string sNewURI = @".." + resourceUri.ToString();
        //    pack.CreateRelationship(new Uri(sNewURI, UriKind.Relative), TargetMode.Internal,
        //        partUri.ToString());
        //}
        //private static void AddResourceRelationship(PackagePart packPart,
        //    Uri resourceUri)
        //{
        //    //add a resource relationship to the document part at the part level?
        //    //resources are always in subfolders
        //    string sNewURI = @".." + resourceUri.ToString();
        //    packPart.CreateRelationship(new Uri(sNewURI, UriKind.Relative), TargetMode.Internal,
        //        _openXmResourceRelationshipType);
        //}
        //private void AddHtmlHeadersToPackage(Package pack,
        //    IDictionary<string, string> args, List<Uri> headerUris,
        //    string errorMsg)
        //{
        //    string sKey = string.Empty;
        //    string sValue = string.Empty;
        //    bool bIsPackagePart = false;
        //    //parallel didn't work here: Parallel.ForEach(args, kvp =>
        //    foreach (KeyValuePair<string, string> kvp in args)
        //    {
        //        sKey = kvp.Key;
        //        sValue = kvp.Value;
        //        if (sValue == HEADERS_KEYNAME)
        //        {
        //            if (string.IsNullOrEmpty(sKey) == false
        //                && Helpers.FileStorageIO.URIAbsoluteExists(sKey))
        //            {
        //                PackagePart oHeaderPart = null;
        //                //add the headers to the package
        //                bIsPackagePart = AddPackagePart(pack, sKey,
        //                    out oHeaderPart, errorMsg);
        //                if (bIsPackagePart)
        //                {
        //                    headerUris.Add(oHeaderPart.Uri);
        //                }
        //            }
        //        }
        //    }
        //}
        //private void RelateHtmlHeadersToPart(Package pack, PackagePart packPart,
        //    List<Uri> headerUris, string errorMsg)
        //{
        //    //only relate to htm and html files
        //    int iIndex = packPart.Uri.ToString().ToLower().IndexOf(".htm");
        //    if (iIndex > 0)
        //    {
        //        if (headerUris.Count > 0)
        //        {
        //            foreach (Uri uriHeader in headerUris)
        //            {
        //                //relate to the partUri
        //                AddResourceRelationship(packPart, uriHeader);
        //            }
        //        }
        //    }
        //}
        //private void AddandRelateResource(string packagePartFilePath,
        //    IDictionary<string, string> args, Package pack, 
        //    PackagePart packPart, string errorMsg)
        //{
        //    string sResourceValue = Path.GetFileName(packagePartFilePath);
        //    string sKey = string.Empty;
        //    string sValue = string.Empty;
        //    bool bIsPackagePart = false;
        //    //parellel didnt' work here: Parallel.ForEach(args, kvp =>
        //    foreach (KeyValuePair<string, string> kvp in args)
        //    {
        //        sKey = kvp.Key;
        //        sValue = kvp.Value;
        //        if (sValue == sResourceValue)
        //        {
        //            if (string.IsNullOrEmpty(sKey) == false
        //                && Helpers.FileStorageIO.URIAbsoluteExists(sKey))
        //            {
        //                PackagePart oResourcePart = null;
        //                //add the resources to the package
        //                bIsPackagePart = AddPackagePart(pack, sKey,
        //                    out oResourcePart, errorMsg);
        //                if (bIsPackagePart)
        //                {
        //                    //relate the resource to the part
        //                    AddResourceRelationship(packPart, oResourcePart.Uri);
        //                }
        //            }
        //        }
        //    }
        //}
        //// ------------------------ ValidateSignatures ------------------------
        ///// <summary>
        /////   Validates all the digital signatures of a given package.</summary>
        ///// <param name="package">
        /////   The package for validating digital signatures.</param>
        ///// <returns>
        /////   true if all digital signatures are valid; otherwise false if the
        /////   package is unsigned or any of the signatures are invalid.</returns>
        //private bool ValidateSignatures(Package package)
        //{
        //    if (package == null)
        //        throw new ArgumentNullException("ValidateSignatures(package)");

        //    // Create a PackageDigitalSignatureManager for the given Package.
        //    PackageDigitalSignatureManager dsm =
        //        new PackageDigitalSignatureManager(package);

        //    // Check to see if the package contains any signatures.
        //    if (!dsm.IsSigned)
        //        return false;   // The package is not signed.

        //    // Verify that all signatures are valid.
        //    VerifyResult result = dsm.VerifySignatures(false);
        //    if (result != VerifyResult.Success)
        //        return false;   // One or more digital signatures are invalid.

        //    // else if (result == VerifyResult.Success)
        //    return true;        // All signatures are valid.

        //}// end:ValidateSignatures()
        //private static void SignAllParts(Package pack)
        //{
        //    if (pack == null)
        //        throw new ArgumentNullException("SignAllParts(pack)");
        //    // Create the DigitalSignature Manager
        //    PackageDigitalSignatureManager dsm =
        //        new PackageDigitalSignatureManager(pack);
        //    dsm.CertificateOption =
        //        CertificateEmbeddingOption.InSignaturePart;
        //    // Create a list of all the part URIs in the pack to sign
        //    // (GetParts() also includes PackageRelationship parts).
        //    System.Collections.Generic.List<Uri> toSign =
        //        new System.Collections.Generic.List<Uri>();
        //    foreach (PackagePart packagePart in pack.GetParts())
        //    {
        //        // Add all package parts to the list for signing.
        //        toSign.Add(packagePart.Uri);
        //    }

        //    // Add the URI for SignatureOrigin PackageRelationship part.
        //    // The SignatureOrigin relationship is created when Sign() is called.
        //    // Signing the SignatureOrigin relationship disables counter-signatures.
        //    Uri uriPartSignatureOriginRelationship = PackUriHelper.CreatePartUri(
        //        new Uri(_openxmlDigitalSignatureUri, UriKind.Relative));
        //    toSign.Add(uriPartSignatureOriginRelationship);
        //    try
        //    {
        //        //use a testing certificate (need certificates for registered users?)
        //        string sCertificateName = "CN=kpboyle";
        //        X509Certificate2 cert = GetCertificate(sCertificateName);
        //        //this signs all parts and
        //        dsm.Sign(toSign, cert);
        //    }

        //    // If there are no certificates or the SmartCard manager is
        //    // not running, catch the exception and show an error message.
        //    catch (CryptographicException ex)
        //    {
              
        //    }

        //}// end:SignAllParts()
        private static X509Certificate2 GetCertificate(string certificateName)
        {
            X509Certificate2 certForRegisteredUser = null;
            //open the X.509 "Current User" store in read only mode.
            //need to open the registered user store (create one for all registered users?)
            X509Store certStore = new X509Store(StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            //place all certificates in an X509Certificate2Collection object.
            X509Certificate2Collection certCollection = certStore.Certificates;
            //find certificateName in the collection
            foreach (X509Certificate2 cert2 in certCollection)
            {
                if (string.IsNullOrEmpty(certificateName) == false)
                {
                    if (cert2.Subject == certificateName)
                    {
                        certForRegisteredUser = cert2;
                        break;
                    }
                }
                else
                {
                    //use last for testing
                    certForRegisteredUser = cert2;
                }
            }
            if (certForRegisteredUser == null)
            {
                throw new CryptographicException("The X.509 certificate could not be found.");
            }
            //close the certStore.
            certStore.Close();
            //return the certificate
            return certForRegisteredUser;
        }
       
        public static async Task<bool> CopyResourceToPackageAsync(
            ContentURI uri, string resourceRelFilePaths,
            int arrayPos, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            string[] arrResourceParams = resourceRelFilePaths.Split(GeneralHelpers.PARAMETER_DELIMITERS);
            string sResourceParams = string.Empty;
            string sResourceRelURI = string.Empty;
            string sResourceFullURI = string.Empty;
            //refactor: parallel didn't work with this loop - keep testing
            //Parallel.For(0, arrResourceParams.Length, i =>
            for (int i = 0; i < arrResourceParams.Length; i++)
            {
                sResourceParams = arrResourceParams[i];
                if (string.IsNullOrEmpty(sResourceParams) == false)
                {
                    //the array holds relative paths; convert to full path for file.copy and package
                    sResourceRelURI = GeneralHelpers.GetDelimitedSubstring(sResourceParams,
                        GeneralHelpers.STRING_DELIMITERS, arrayPos);
                    //remove the rel directories
                    string sBaseResourceURI = sResourceRelURI.Replace("../", string.Empty);
                    if ((!sBaseResourceURI.StartsWith(GeneralHelpers.WEBFILE_PATH_DELIMITER))
                        && (!rootDirectory.EndsWith(GeneralHelpers.FILE_PATH_DELIMITER)))
                    {
                        sBaseResourceURI = string.Concat(GeneralHelpers.WEBFILE_PATH_DELIMITER,
                            sBaseResourceURI);
                    }
                    FileStorageIO.PLATFORM_TYPES ePlatform
                        = uri.URIDataManager.PlatformType;
                    if (ePlatform == FileStorageIO.PLATFORM_TYPES.webserver)
                    {
                        bHasCopied = await CopyWebServerResourceToPackageAsync(
                            uri, rootDirectory, newDirectory,
                            parentFileName, zipArgs, sBaseResourceURI);
                    }
                    else if (ePlatform == FileStorageIO.PLATFORM_TYPES.azure)
                    {
                        bHasCopied = await CopyAzureResourceToPackageAsync(
                            uri, rootDirectory, newDirectory,
                            parentFileName, zipArgs, sBaseResourceURI);
                    }
                }
            }
            return bHasCopied;
        }
        public static async Task<bool> CopyWebServerResourceToPackageAsync(
            ContentURI uri, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs, string baseResourceFullURI)
        {
            bool bHasCopied = false;
            string sPackageNewURI = string.Empty;
            //convert the full web uri to an equivalent filepath 
            string sFileName = Path.GetFileName(baseResourceFullURI);
            //remove the http:\\www.devtreks.org\resources
            string sWebPath = string.Format("{0}{1}{2}",
                uri.URIDataManager.DefaultRootWebStoragePath,
                uri.URIDataManager.ResourceURIName,
                GeneralHelpers.WEBFILE_PATH_DELIMITER);
            string sResourceFilePath = baseResourceFullURI.Replace(sWebPath, string.Empty);
            sResourceFilePath = sResourceFilePath.Replace(
                GeneralHelpers.WEBFILE_PATH_DELIMITER, GeneralHelpers.FILE_PATH_DELIMITER);
            string sResourceFullURI = string.Concat(rootDirectory,
                uri.URIDataManager.ResourceURIName,
                GeneralHelpers.FILE_PATH_DELIMITER, sResourceFilePath);
            if (!string.IsNullOrEmpty(baseResourceFullURI))
            {
                sPackageNewURI = string.Concat(Path.Combine(newDirectory,
                    uri.URIDataManager.ResourceURIName),
                    GeneralHelpers.FILE_PATH_DELIMITER,
                    sFileName);
                Helpers.FileStorageIO.DirectoryCreate(uri, sPackageNewURI);
                //don't add the same file to the zip args (or the string array becomes out of synch)
                if (Helpers.FileStorageIO.URIAbsoluteExists(uri, sPackageNewURI) == false)
                {
                    await Helpers.FileStorageIO.CopyURIsAsync(uri, sResourceFullURI, sPackageNewURI);
                    //parentFileNameKey tells package how to relate resource to parentfile
                    if (!zipArgs.ContainsKey(sPackageNewURI))
                    {
                        zipArgs.Add(sPackageNewURI, parentFileName);
                    }
                    //using streams to copy: see openxml/Packaging sample (write example)
                }
            }
            return bHasCopied;
        }
        public static async Task<bool> CopyAzureResourceToPackageAsync(
            ContentURI uri, string rootDirectory, string newDirectory,
            string parentFileName, IDictionary<string, string> zipArgs, string baseResourceFullURI)
        {
            bool bHasCopied = false;
            string sPackageNewURI = string.Empty;
            if (!string.IsNullOrEmpty(baseResourceFullURI))
            {
                //get the filename
                string sFileName = Path.GetFileName(baseResourceFullURI);
                sPackageNewURI = string.Concat(Path.Combine(newDirectory,
                    uri.URIDataManager.ResourceURIName),
                    GeneralHelpers.FILE_PATH_DELIMITER,
                    sFileName);
                Helpers.FileStorageIO.DirectoryCreate(uri, sPackageNewURI);
                //don't add the same file to the zip args (or the string array becomes out of synch)
                if (Helpers.FileStorageIO.URIAbsoluteExists(uri, sPackageNewURI) == false)
                {
                    AzureIOAsync azureIO = new AzureIOAsync(uri);
                    string sBlobURIPath = string.Empty;
                    azureIO.SaveCloudFileInFileSystem(baseResourceFullURI, 
                        sPackageNewURI);
                    azureIO = null;
                    //parentFileNameKey tells package how to relate resource to parentfile
                    if (!zipArgs.ContainsKey(sPackageNewURI))
                    {
                        zipArgs.Add(sPackageNewURI, parentFileName);
                    }
                }
            }
            return bHasCopied;
        }
        public static string MakeResourcePath(string currentResourceFullURI, 
            string newPackageDirectory)
        {
            string sNewPackagePath = string.Empty;
            string sErrorMsg = string.Empty;
            string sMimeType = AppHelpers.Resources.GetMimeTypeFromFileExt(Path.GetExtension(currentResourceFullURI).ToLower(), 
                ref sErrorMsg);
            string sPackageSubfolderName = GetPackageSubFolderName(currentResourceFullURI, sMimeType);
            sNewPackagePath = string.Concat(newPackageDirectory, 
                GeneralHelpers.FILE_PATH_DELIMITER, sPackageSubfolderName, 
                GeneralHelpers.FILE_PATH_DELIMITER, 
                Path.GetFileName(currentResourceFullURI), string.Empty);
            return sNewPackagePath;
        }
        private static string GetPackageSubFolderName(string currentResourceFullURI, 
            string mimeType)
        {
            string sPackageSubfolderName = "images";
            int iIndex = mimeType.IndexOf("image");
            if (iIndex < 0)
            {
                iIndex = mimeType.IndexOf("video");
                if (iIndex > 0)
                {
                    sPackageSubfolderName = "videos";
                }
                else
                {
                    iIndex = mimeType.IndexOf("audio");
                    if (iIndex > 0)
                    {
                        sPackageSubfolderName = "audios";
                    }
                    else
                    {
                        iIndex = mimeType.IndexOf("audio");
                        if (iIndex > 0)
                        {
                            sPackageSubfolderName = "audios";
                        }
                        else
                        {
                            iIndex = currentResourceFullURI.IndexOf(".xsl");
                            if (iIndex > 0)
                            {
                                sPackageSubfolderName = "stylesheets";
                            }
                            else
                            {
                                sPackageSubfolderName = "misc";
                            }
                        }
                    }
                }
            }
            return sPackageSubfolderName;
        }
        public static async Task<bool> CopyRelatedDataToPackageAsync(
            ContentURI uri, string currentFilePath,
            string packageName, string fileType, string newFilePath,
            bool needsAllRelatedData, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            FileStorageIO.PLATFORM_TYPES platform
                = uri.URIDataManager.PlatformType;
            if (platform == FileStorageIO.PLATFORM_TYPES.webserver)
            {
                bHasCopied = await CopyRelatedDataToWebServerPackageAsync(
                    uri, currentFilePath, packageName, fileType, newFilePath,
                    needsAllRelatedData, zipArgs);
            }
            else if (platform == FileStorageIO.PLATFORM_TYPES.azure)
            {
                AzureIOAsync azureIO = new AzureIOAsync(uri);
                bHasCopied = await azureIO.CopyRelatedDataToCloudServerPackageAsync(
                    uri, currentFilePath,
                    packageName, fileType, newFilePath, needsAllRelatedData, zipArgs);
            }
            return bHasCopied;
        }
        public static async Task<bool> CopyRelatedDataToWebServerPackageAsync(
            ContentURI uri, string currentFilePath,
            string packageName, string fileType, string newFilePath, 
            bool needsAllRelatedData, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            DirectoryInfo oldDir = null;
            if (Path.HasExtension(currentFilePath))
            {
                //if it has an extension it needs the parent directory
                oldDir = new DirectoryInfo(
                    Path.GetDirectoryName(currentFilePath));
            }
            else
            {
                oldDir = new DirectoryInfo(currentFilePath);
            }
            DirectoryInfo newDir = null;
            if (Path.HasExtension(newFilePath))
            {
                //if it has an extension it needs the parent directory
                newDir = new DirectoryInfo(
                    Path.GetDirectoryName(newFilePath));
            }
            else
            {
                newDir = new DirectoryInfo(newFilePath);
            }
            //add siblings for everything
            bHasCopied = await AddSiblingFilesAsync(
                uri, oldDir, currentFilePath, packageName, 
                fileType, newDir.FullName, zipArgs);
            if (needsAllRelatedData)
            {
                //copy children subfolder data 
                //html subdirectories get put in root path so they can find resources
                bool bNeedsNewSubDirectories = 
                    (fileType.EndsWith(AppHelpers.Resources.FILEEXTENSION_TYPES.xml.ToString())) 
                    ? true : false;
                bool bHasCopiedDirs 
                    = await GeneralHelpers.CopyDirectoriesAsync(
                        uri, currentFilePath,
                    newFilePath, bNeedsNewSubDirectories);
                //add them to zipargs
                bool bHasAdded = await AddChildrenFilesToZipArgsAsync(newDir, packageName, fileType, zipArgs);
            }
            return bHasCopied;
        }
        
        private static async Task<bool> AddSiblingFilesAsync(
            ContentURI uri, DirectoryInfo folder,
            string currentFilePath, string packageName, string fileType,
            string newFilePath, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            string sPackageFilePath = string.Empty;
            if (folder != null)
            {
                string sNoCalcDocExt = string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER, 
                    Helpers.GeneralHelpers.FILENAME_EXTENSIONS.addin.ToString());
                FileInfo[] files = folder.GetFiles();
                if (files != null)
                {
                    string sFileExtension = string.Empty;
                    foreach (FileInfo file in files)
                    {
                        if (fileType == Helpers.GeneralHelpers.EXTENSION_CSV)
                        {
                            bHasCopied = AddSiblingCSVFilesAsync(
                                uri, file, currentFilePath, sNoCalcDocExt,
                                packageName, fileType, newFilePath, zipArgs);
                        }
                        else
                        {
                            //current file is already in the package
                            if (!file.FullName.Equals(currentFilePath))
                            {
                                //package either all html or all xml files
                                if (Path.GetExtension(file.FullName).Equals(fileType))
                                {
                                    //v174 exclude calcdocs because too much unneeded docs in package
                                    if (!file.Name.Contains(sNoCalcDocExt))
                                    {
                                        sPackageFilePath = Path.Combine(newFilePath, Path.GetFileName(file.FullName));
                                        bHasCopied = await FileStorageIO.CopyURIsAsync(
                                            uri, file.FullName, sPackageFilePath);
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
            }
            return bHasCopied;
        }
        private static bool AddSiblingCSVFilesAsync(
            ContentURI uri, FileInfo file,
            string currentFilePath, string noCalcDocExt, string packageName, string fileType,
            string newFilePath, IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            string sPackageFilePath = string.Empty;
            //convert xml files
            if (Path.GetExtension(file.FullName).Equals(Helpers.GeneralHelpers.EXTENSION_XML))
            {
                //v174 exclude calcdocs because too much unneeded docs in package
                if (!file.Name.Contains(noCalcDocExt))
                {
                    //to csv
                    string sCSVFileName = Path.GetFileName(file.FullName).Replace(Helpers.GeneralHelpers.EXTENSION_XML, 
                        Helpers.GeneralHelpers.EXTENSION_CSV);
                    sPackageFilePath = Path.Combine(newFilePath, sCSVFileName);
                    ContentURI docToCalcURI = new ContentURI();
                    docToCalcURI.URIDataManager.TempDocPath = sPackageFilePath;
                    ObservationTextBuilder csvBuilder = new ObservationTextBuilder();
                    IDictionary<string, string> fileOrFolderPaths = new Dictionary<string, string>();
                    fileOrFolderPaths.Add("1", file.FullName);
                    bHasCopied = csvBuilder.StreamAndSaveObservation(docToCalcURI, fileOrFolderPaths);
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
            return bHasCopied;
        }
        public static async Task<bool> AddChildrenFilesToZipArgsAsync(string newFilePath,
            string packageName, string fileType,
            IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            DirectoryInfo newDir = null;
            if (Path.HasExtension(newFilePath))
            {
                //if it has an extension it needs the parent directory
                newDir = new DirectoryInfo(
                    Path.GetDirectoryName(newFilePath));
            }
            else
            {
                newDir = new DirectoryInfo(newFilePath);
            }
            bHasCopied = await AddChildrenFilesToZipArgsAsync(newDir, packageName, fileType, zipArgs);
            return bHasCopied;
        }
        public static async Task<bool> AddChildrenFilesToZipArgsAsync(
            DirectoryInfo dir, string packageName, string fileType,
            IDictionary<string, string> zipArgs)
        {
            bool bHasCopied = false;
            //add any new files added to root directory
            foreach (FileInfo file in dir.GetFiles())
            {
                if (!zipArgs.ContainsKey(file.FullName))
                {
                    //package either all html or all xml files
                    if (Path.GetExtension(file.FullName).Equals(fileType))
                    {
                        zipArgs.Add(file.FullName, packageName);
                    }
                }
            }
            //files have alread been copied into 
            DirectoryInfo[] dirs = dir.GetDirectories();
            //add any new subfolders added
            foreach (DirectoryInfo folder in dirs)
            {
                if (!folder.FullName.EndsWith("stylesheets")
                    && !folder.FullName.EndsWith("scripts"))
                {
                    FileInfo[] files = folder.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (!zipArgs.ContainsKey(file.FullName))
                        {
                            //package either all html or all xml files
                            if (Path.GetExtension(file.FullName).Equals(fileType))
                            {
                                zipArgs.Add(file.FullName, packageName);
                            }
                        }
                    }
                    //check for further subfolders
                    bHasCopied = await AddChildrenFilesToZipArgsAsync(folder, packageName, fileType, zipArgs);
                }
            }
            return bHasCopied;
        }
    }
}
