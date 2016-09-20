using DevTreks.Helpers;
using DevTreks.Models;
using DevTreks.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Data = DevTreks.Data;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DataHelpers = DevTreks.Data.Helpers;
using EditHelpers = DevTreks.Data.EditHelpers;
using Exceptions = DevTreks.Exceptions;
using RuleHelpers = DevTreks.Data.RuleHelpers;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		assist uploading/downloading files, 
    ///             zipping and packaging of files
    ///Author:		www.devtreks.org
    ///Date:		2016, July
    ///References:	www.devtreks.org
    /// </summary>
    public class IOHelper
    {
        public IOHelper() { }
        //keep in synch with LoadFile1.displayFileUploadClick javascript
        private const string FILE_UPLOAD_PARAMS         = "fileUploadParams1";
        //used in ViewDataHelper to init uri (and paths for file deletions)
        public const string FILE_UPLOAD_URI             = "fileSelectedFileName1";
        //keep in synch with SharedUtils.js
        //what is the doc path to the uri being added to package
        public const string URI_XMLDOC_PATH = "uridocpath";
        //keep in synch with OnClickPresenter1.setPackageParams javascript
        private const string PACKAGE_TYPE               = "packagetype";
        private const string SIGNATURE_TYPE             = "digitalsignaturetype";
        private const string ZIP_ARGS                   = "zipargs";
        private const string PACKAGE_NAME               = "DevTreksPackage";
        private const string ZIP_FILES                  = "zipfiles";
        private const string ZIP_DOCPATHS               = "zipdocpaths";
        private const string FILE_TYPE                  = "filetype";
        private const string INCLUDE_RELATED_DATA_TYPE  = "relateddatatype";

        public enum RELATED_DATA_TYPES
        {
            //don't include related data
            no      = 0,
            //include sibling files and children subfolder contents
            yes     = 1
        }
        public async Task<bool> UploadFileAsync(IMemberService memberService, 
            IContentService contentService, HttpContext context, 
            Data.ContentURI uri)
        {
            bool bIsUploaded = false;
            string sFileSize = string.Empty;
            string sExistingFileName = string.Empty;
            Data.ContentURI oFileUploadURI = null;
            string sErrorMsg = string.Empty;
            try
            {
                //uri is the node associated with the upload. oFileURI is used in case 
                //future versions change this relation (i.e. because of ui feedback); 
                //Old versions of uploaded files are deleted (using uri.URIClub.docpath).
                //If the old files need to be archived, add a subfolder (i.e. named archives)
                //to oFileURI.URIClub.DocPath and move, rather than delete, the old files.
                bool bOkToInit = InitUploadFile(context, uri, out oFileUploadURI, out sFileSize,
                    out sExistingFileName);
                if (bOkToInit)
                {
                    bIsUploaded = await SaveFileAsync(contentService, context,
                        oFileUploadURI, sFileSize, sExistingFileName);
                    uri.ErrorMessage = oFileUploadURI.ErrorMessage;
                }
                else
                {
                    uri.ErrorMessage = AppHelper.GetErrorMessage("IOHELPER_NOSAVE");
                }
                //add short feedback message to long description
                if (!string.IsNullOrEmpty(uri.ErrorMessage))
                {
                    string sErrorMessage = string.Concat(uri.ErrorMessage,
                        DataHelpers.GeneralHelpers.GetDateUniversalNow());
                    //no error messages over 100 chars stored in db.
                    sErrorMessage = sErrorMessage.Substring(0, 100);
                    await contentService.UpdateURIResourceFileUploadMsgAsync(oFileUploadURI, sErrorMessage);
                }
                else
                {
                    string sSuccessMessage = string.Concat(AppHelper.GetErrorMessage("IOHELPER_YESSAVE"),
                        DataHelpers.GeneralHelpers.GetDateUniversalNow());
                    await contentService.UpdateURIResourceFileUploadMsgAsync(oFileUploadURI, sSuccessMessage);
                }
            }
            catch (Exception x)
            {
                sErrorMsg = string.Concat(AppHelper.GetErrorMessage("IOHELPER_NOSAVE"), 
                    DataHelpers.GeneralHelpers.GetDateUniversalNow(), x.ToString());
                if (x.InnerException != null)
                {
                    sErrorMsg += x.InnerException.ToString();
                }
                //no error messages over 100 chars stored in db.
                sErrorMsg = sErrorMsg.Substring(0, 100);
            }
            if (!string.IsNullOrEmpty(sErrorMsg))
            {
                await contentService.UpdateURIResourceFileUploadMsgAsync(oFileUploadURI, sErrorMsg);
            }
            return bIsUploaded;
        }
        
        public bool InitUploadFile(HttpContext context, Data.ContentURI uri,
            out Data.ContentURI fileUploadURI, out string size, out string existingFileName)
        {
            bool bInitIsOk = false;
            fileUploadURI = null;
            size = string.Empty;
            existingFileName = string.Empty;
            string sFileUploadParams = ViewDataHelper.GetRequestParam(context, FILE_UPLOAD_PARAMS);
            if (string.IsNullOrEmpty(sFileUploadParams) == false)
            {
                //initialize fileUploadURI
                bInitIsOk = GetFileUploadParams(uri, sFileUploadParams, out fileUploadURI,
                    out size, out existingFileName);
            }
            else
            {
                uri.ErrorMessage = AppHelper.GetErrorMessage("IOHELPER_NOSAVELOCATION");
            }
            return bInitIsOk;
        }
        private bool GetFileUploadParams(Data.ContentURI uri, string fileUploadParams,
            out Data.ContentURI fileUploadURI, out string size, 
            out string existingFileName)
        {
            fileUploadURI = null;
            size = string.Empty;
            existingFileName = string.Empty;
            bool bIsOkToUpload = false;
            //1. formUploadParams is a std 'uripattern;attname;datatype;size' delimited string
            string[] arrUploadFileParams = fileUploadParams.Split(
                DataHelpers.GeneralHelpers.STRING_DELIMITERS);
            if (arrUploadFileParams.Length > 3)
            {
                string sURIPattern = string.Empty;
                string sAttName = string.Empty;
                string sDataType = string.Empty;
                int iBaseId = 0;
                EditHelpers.EditHelper.GetStandardEditNameParams(arrUploadFileParams,
                    out sURIPattern, out sAttName, out sDataType, out size);
                fileUploadURI = new Data.ContentURI(uri);
                fileUploadURI.ChangeURIPattern(sURIPattern);
                DataHelpers.GeneralHelpers.SetApps(fileUploadURI);
                if (uri.URIClub != null)
                {
                    //linked views file updates check for base linkedview or join
                    //they use clubdocpath to distinguish between the two
                    fileUploadURI.URIClub = new Account(uri.URIClub);
                }
                if (uri.URIDataManager.AppType 
                    == DevTreks.Data.Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    //sURIPattern has the basedocid being updated 
                    //(uri.uripattern is the joinid)
                    iBaseId = fileUploadURI.URIId;
                }
                 //don't use fileUploadURI.URIName because it changes from filename to name
                existingFileName = Data.ContentURI.GetURIPatternPart(sURIPattern,
                   Data.ContentURI.URIPATTERNPART.name);
                fileUploadURI.URIDataManager.AttributeName = sAttName;
                if (uri.URIDataManager.AppType 
                    == DevTreks.Data.Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    fileUploadURI.URIDataManager.BaseId = iBaseId;
                }
                //need parent to make resource file path
                fileUploadURI.URIDataManager.ParentURIPattern = uri.URIPattern;
                bIsOkToUpload = true;
            }
            else
            {
                uri.ErrorMessage = AppHelper.GetErrorMessage("IOHELPER_NOSAVELOCATION2");
            }
            return bIsOkToUpload;
        }
        
        public async Task<bool> SaveFileAsync(IContentService contentService,
            HttpContext context, Data.ContentURI fileUploadURI,
            string fileSize, string existingFileName)
        {
            bool bHasSaved = false;
            //2.0.0 changes
            IFormFileCollection colFiles
                = context.Request.Form.Files;
            if (colFiles.Count > 0)
            {
                int i = 0;
                foreach (var formFile in colFiles)
                {
                    //version 1.0 security limits file uploads to 1
                    if (i == 0)
                    {
                        if (formFile.Length > 0)
                        {
                            char[] formDelims = new char[] { '=' };
                            string sFilePath = DataHelpers.GeneralHelpers.GetSubstring(
                               formFile.ContentDisposition, formDelims, 2);
                            formDelims = new char[] { '"', '/' };
                            sFilePath = sFilePath.TrimStart(formDelims);
                            formDelims = new char[] { '/', '"' };
                            string sUploadFilePath = sFilePath.TrimEnd(formDelims);
                            //string sUploadFilePath = colFiles[sKeyName].FileName;
                            string sNewFileName = Path.GetFileName(sUploadFilePath);
                            if (fileUploadURI.URIDataManager.AppType == DataHelpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
                            {
                                //devpack part xml docs use standard file naming conventions (easier to find)
                                sNewFileName = string.Concat(
                                    DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(fileUploadURI),
                                    DataHelpers.GeneralHelpers.EXTENSION_XML);
                            }
                            else
                            {
                                //attname = FileName tells validation to keep .ext
                                RuleHelpers.GeneralRules.ValidateXSDInput(
                                    DataAppHelpers.General.FILE_NAME,
                                    ref sNewFileName, RuleHelpers.GeneralRules.STRING,
                                    RuleHelpers.GeneralRules.NAME_SIZE);
                            }
                            int iFileLength = (int)formFile.Length;
                            //verify this file length against the length allowed in ResourceRules
                            bool bIsSizeOK 
                                = RuleHelpers.ResourceRules.VerifyFileLength(
                                    fileUploadURI, iFileLength);
                            if (bIsSizeOK)
                            {
                                //derive the mimetype form the file extension to save the file in db
                                string sMimeType = DataAppHelpers.Resources.GetMimeTypeFromFileExt(fileUploadURI,
                                    Path.GetExtension(sUploadFilePath));
                                if (string.IsNullOrEmpty(sMimeType))
                                {
                                    fileUploadURI.ErrorMessage = AppHelper.GetErrorMessage("IOHELPER_BADFILETYPE");
                                    return false;
                                }
                                //2.0.0
                                using (Stream oUploadFileStream = formFile.OpenReadStream())
                                {
                                    if (fileUploadURI.URINodeName.StartsWith(
                                        DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
                                    {
                                        //save in table resource or resourcepack
                                        bHasSaved = await contentService.SaveURIResourceFileAsync(fileUploadURI, sNewFileName,
                                            iFileLength, sMimeType, oUploadFileStream);
                                        bHasSaved = true;
                                    }
                                    else
                                    {
                                        //it's an xml doc; can be saved in a number of different tables
                                        XmlReader oReader = null;
                                        DataHelpers.XmlFileIO oXmlFileIO = new DataHelpers.XmlFileIO();
                                        oXmlFileIO.GetXmlFromFile(oUploadFileStream, out oReader);
                                        oXmlFileIO = null;
                                        bool bIsMetaData = IsMetaData(fileUploadURI);
                                        //these are always base table updates (addins handle join tables)
                                        bHasSaved = await contentService.SaveURISecondBaseDocAsync(fileUploadURI, bIsMetaData,
                                            sNewFileName, oReader);
                                        if (fileUploadURI.ErrorMessage == string.Empty)
                                        {
                                            bHasSaved = true;
                                        }
                                        else
                                        {
                                            bHasSaved = false;
                                        }
                                    }
                                }
                                if (bHasSaved)
                                {
                                    //this deletes the blob and resource which was just saved
                                    if (sNewFileName != existingFileName
                                        && existingFileName != DataHelpers.GeneralHelpers.GetResource("FILE_NONE")
                                        && existingFileName != DataHelpers.GeneralHelpers.NONE)
                                    {
                                        if (fileUploadURI.URIDataManager.AppType
                                            == DataHelpers.GeneralHelpers.APPLICATION_TYPES.resources)
                                        {
                                            //0.9.0: the resource directory is deleted on the dataaccess layer before the resource is saved
                                        }
                                        else
                                        {
                                            //delete the existing base linkedview or devpackpart
                                            DeleteExistingBaseFile(fileUploadURI,
                                                sNewFileName, existingFileName);
                                        }
                                    }
                                    //delete the fileuploaduri docs
                                    if (DataHelpers.FileStorageIO.URIAbsoluteExists(fileUploadURI,
                                        fileUploadURI.URIClub.ClubDocFullPath))
                                    {
                                        DataHelpers.FileStorageIO.DeleteURI(fileUploadURI,
                                            fileUploadURI.URIClub.ClubDocFullPath);
                                    }
                                }
                                else
                                {
                                    if (fileUploadURI.ErrorMessage == string.Empty)
                                    {
                                        fileUploadURI.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                            string.Empty, "IOHELPER_CANTSAVE");
                                    }
                                }
                            }
                            else
                            {
                                fileUploadURI.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                    string.Empty, "IOHELPER_TOOBIG");
                            }
                        }
                    }
                    i++;
                }
            }
            return bHasSaved;
        }

    private bool IsMetaData(Data.ContentURI uri)
        {
            bool bIsMetaData = false;
            int iIndex = uri.URIDataManager.AttributeName.ToLower().IndexOf("metadata");
            if (iIndex > 0)
            {
                bIsMetaData = true;
            }
            return bIsMetaData;
        }
        private static async Task DeleteExistingResourceFile(IContentService contentService,
            Data.ContentURI fileUploadURI, string newFileName, string existingFileName)
        {
            //delimited string resourceurl;altdesc;resourceId
            string sResourceParams = string.Empty;
            bool bNeedsFullPath = true;
            bool bNeedsOneRecord = true;
            //for resource nodes, this uses urid to get the resourceurl
            sResourceParams = await contentService.GetResourceURLsAsync(fileUploadURI,
                bNeedsFullPath, bNeedsOneRecord,
                DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.filename, string.Empty);
            if (sResourceParams != string.Empty)
            {
                string sResourceURL = DataHelpers.GeneralHelpers.GetSubstringFromFront(sResourceParams, DataHelpers.GeneralHelpers.STRING_DELIMITERS,
                    1);
                //substitute old file name (note that old filename not name must be passed here)
                sResourceURL = sResourceURL.Replace(newFileName, existingFileName);
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(fileUploadURI, sResourceURL))
                {
                    DataHelpers.FileStorageIO.DeleteURI(fileUploadURI, sResourceURL);
                }
            }
        }
        private static void DeleteExistingBaseFile(Data.ContentURI fileUploadURI, 
            string newFileName, string existingFileName)
        {
            //substitute old base file name (note that old filename not name must be passed here)
            string sSelectedViewURIPattern = DataHelpers.GeneralHelpers.MakeURIPattern(
                fileUploadURI.URIName, fileUploadURI.URIId.ToString(),
                fileUploadURI.URINetworkPartName, fileUploadURI.URINodeName,
                DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.selected.ToString());
            string sSelectedViewClubPath = fileUploadURI.URIClub.ClubDocFullPath.Replace(
                DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(fileUploadURI),
                DataHelpers.ContentHelper.MakeStandardFileNameFromURIPattern(sSelectedViewURIPattern));
            if (DataHelpers.FileStorageIO.URIAbsoluteExists(fileUploadURI, sSelectedViewClubPath))
            {
                DataHelpers.FileStorageIO.DeleteURI(fileUploadURI, sSelectedViewClubPath);
            }
        }
        public async Task MakeZipPackage(IContentService contentService, 
            IMemberService memberService, HttpContext context, 
            Data.ContentURI uri)
        {
            //need a key-value collection for packages (to establish relationship between files and resources)
            //string.empty value: string is filename and empty value mean it's a root package member
            //string with a value: string is filename (packagepart) and value is one of it's related resources
            IDictionary<string, string> colZipArgs = new Dictionary<string, string>();
            string sInitPackageFilePath = await InitMakeZipPackageAsync(contentService, memberService,
                context, uri, colZipArgs);
            if (!string.IsNullOrEmpty(sInitPackageFilePath))
            {
                await SendZipFileLinkAsResponseAsync(contentService, context, uri, 
                   sInitPackageFilePath, colZipArgs);
            }
        }
        private async Task<string> InitMakeZipPackageAsync(IContentService contentService, 
            IMemberService memberService, HttpContext context, 
            Data.ContentURI uri, IDictionary<string, string> zipArgs)
        {
            string sInitPackageFilePath = string.Empty;
            bool bInitIsOk = false;
            bool bIsPackageDirectory = true;
            DataHelpers.FileStorageIO.PLATFORM_TYPES ePlatform
                = uri.URIDataManager.PlatformType;
            //resources go in this path
            string sRootPackageDirectory = string.Empty;
            //content files go in this directory because
            //rel paths within htmldocs will allow images to load
            string sContentPackageDirectory = string.Empty;
            sRootPackageDirectory = DataHelpers.AppSettings.GetTempWebDirectory(uri,
                bIsPackageDirectory, DataHelpers.GeneralHelpers.Get2RandomInteger());
            sContentPackageDirectory = sRootPackageDirectory;
            string sPackageURIPattern = string.Empty;
            string sPackageName = string.Empty;
            //zip args follow ziplibrary protocols
            string sZipArgs = DataHelpers.GeneralHelpers.GetFormValue(uri, ZIP_ARGS);
            if (sZipArgs != null)
            {
                string[] arrZipArgs = sZipArgs.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS);
                if (arrZipArgs != null)
                {
                    if (arrZipArgs.Length > 0)
                    {
                        //this param is sent for the sake of click arg consistency
                        sPackageURIPattern = arrZipArgs[0];
                        if (string.IsNullOrEmpty(sPackageURIPattern) == false)
                        {
                            //add a random number to the file name to avoid overwriting previous downloads
                            sPackageName = string.Concat(PACKAGE_NAME,
                                DataHelpers.GeneralHelpers.FILENAME_DELIMITER,
                                DataHelpers.GeneralHelpers.GetRandomInteger(0).ToString(), ".zip");
                            sInitPackageFilePath = string.Concat(sRootPackageDirectory,
                                DataHelpers.GeneralHelpers.FILE_PATH_DELIMITER, sPackageName);
                            //add the html header files (.css, scripts) to the zipArgs 
                            //uses the same /scripts and /stylesheets subfolders as server 
                            //(clients and servers use consistent relative paths to all resources)
                            await StylesheetHelper.AddHtmlHeaderFilesToPackageAsync(uri,
                                sContentPackageDirectory, zipArgs);
                        }
                        else
                        {
                            uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "IOHELPER_NONAME");
                            return sInitPackageFilePath;
                        }
                    }
                    else
                    {
                        uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "IOHELPER_NOINSTRUCTS");
                        return sInitPackageFilePath;
                    }
                }
            }
            bInitIsOk = await FinishMakeZipPackageAsync(contentService, memberService,
                context, uri, zipArgs, sPackageName,
                sRootPackageDirectory, sContentPackageDirectory, ePlatform);
            return sInitPackageFilePath;
        }
        private async Task<bool> FinishMakeZipPackageAsync(IContentService contentService, IMemberService memberService,
            HttpContext context, Data.ContentURI uri, IDictionary<string, string> zipArgs,
            string packageName, string rootPackageDirectory, string contentPackageDirectory,
            DataHelpers.FileStorageIO.PLATFORM_TYPES platform)
        {
            bool bInitIsOk = false;
            //list of standard devtreks uripatterns@uridocpaths (delimited strings)
            string sZipFiles = DataHelpers.GeneralHelpers.GetFormValue(uri, ZIP_FILES);
            //the docpaths point to the file system folders containing the data to package
            //no db hits should be required to get those files (except for their related resources)
            string sZipDocPaths = DataHelpers.GeneralHelpers.GetFormValue(uri, ZIP_DOCPATHS);
            //zip xml or xhtml files?
            string sFileType = GetFileType(uri);
            if (string.IsNullOrEmpty(sZipFiles) == false)
            {
                //zip files should use the ZipLibrary's protocols 
                //for ZipLibrary-specific uri (i.e. -p key would be related to a password value)
                //the rest of the files use a logical name-value relation 
                //(file-related resource)

                //in order to unzip into one directory, move files from 
                //all of the existing directories into one random directory 
                string[] arrZipFiles = sZipFiles.Split(
                    DataHelpers.GeneralHelpers.STRING_DELIMITERS);
                string[] arrZipDocPaths = sZipDocPaths.Split(
                    DataHelpers.GeneralHelpers.STRING_DELIMITERS);
                int i = 0;
                string sURIPattern = string.Empty;
                string sURIXmlDocPath = string.Empty;
                string sNewXmlFilePath = string.Empty;
                string sSelectFileNameURIPattern = string.Empty;
                List<Data.ContentURI> zipFileURIs = new List<Data.ContentURI>();
                for (i = 0; i < arrZipFiles.Length; i++)
                {
                    sURIPattern = arrZipFiles[i];
                    if (string.IsNullOrEmpty(sURIPattern) == false)
                    {
                        sURIXmlDocPath = (arrZipDocPaths.Length > i) ? arrZipDocPaths[i] : string.Empty;
                        //get rid of the "*" last char (added to distinguish this 
                        //checklist from the stateful selections-made checklist)
                        if (sURIPattern.EndsWith("*")) sURIPattern =
                            sURIPattern.Remove(sURIPattern.Length - 1);
                        //file paths (uri.URIClub.ClubDocPath) have to be 
                        //derived from each sURIPattern
                        //only args that change are related to sURIPattern
                        //(not postbacktype which is needed later)
                        bool bNeedsFullContent = true;
                        bool bIsFileNameURIPattern = false;
                        //because of the performance consequences, packages can't 
                        //convert xml to html; they can only package what's already 
                        //found in the xmlfilepath
                        Data.ContentURI oZipFileURI = SetZipFileState(context,
                            contentService, memberService, uri, sURIPattern,
                            sURIXmlDocPath, bIsFileNameURIPattern, bNeedsFullContent);
                        //at present, only read views, with 'view only' authorization
                        SetZipFileDisplayState(oZipFileURI);
                        sSelectFileNameURIPattern = Path.GetFileName(oZipFileURI.URIClub.ClubDocFullPath);
                        //this has to be contentpackagedir or images won't load
                        sNewXmlFilePath = string.Concat(contentPackageDirectory,
                                DataHelpers.GeneralHelpers.FILE_PATH_DELIMITER, sSelectFileNameURIPattern);
                        if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, 
                            oZipFileURI.URIClub.ClubDocFullPath))
                        {
                            //always copy the xml file
                            bInitIsOk = await DataHelpers.FileStorageIO.CopyURIsAsync(uri, 
                                oZipFileURI.URIClub.ClubDocFullPath,
                                sNewXmlFilePath);
                            //the html can't be reliably copied because it's name could differ from xml
                            //instead copyrelateddata will copy sibling html
                            DataHelpers.GeneralHelpers.AddToList(
                                sNewXmlFilePath, packageName, zipArgs);
                            await CopyRelatedDataToPackageAsync(uri, contentService,
                                oZipFileURI.URIClub.ClubDocFullPath, packageName, sFileType,
                                sNewXmlFilePath, zipArgs);
                            AddZipFileURIToList(oZipFileURI, zipFileURIs);
                        }
                        else if (DataHelpers.FileStorageIO.DirectoryExists(uri, oZipFileURI.URIClub.ClubDocFullPath))
                        {
                            //copy files from the directory into the package
                            await CopyRelatedDataToPackageAsync(uri, contentService,
                                oZipFileURI.URIClub.ClubDocFullPath, 
                                packageName, sFileType, sNewXmlFilePath, zipArgs);
                        }
                        else
                        {
                            uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "IOHELPER_CANTBUILD");
                        }
                    }
                }
                //needs to come after copy relateddata and use rootdir
                //this doesn't need to be async
                await SetZipArgsResourceAsync(contentService, memberService,
                    context, uri, rootPackageDirectory,
                    zipFileURIs, sNewXmlFilePath, zipArgs);
            }
            else
            {
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "IOHELPER_CANTZIP");
            }
            if (zipArgs.Count > 0 && uri.ErrorMessage == string.Empty)
                bInitIsOk = true;
            return bInitIsOk;
        }
        
        private static string GetFileType(Data.ContentURI uri)
        {
            string sFileType 
                = DataHelpers.GeneralHelpers.GetFormValue(uri, FILE_TYPE);
            //convention is to store html with html extension
            if (string.IsNullOrEmpty(sFileType))
            {
                sFileType = DataAppHelpers.Resources.FILEEXTENSION_TYPES.html.ToString();
            }
            else if (sFileType == DataAppHelpers.Resources.FILEEXTENSION_TYPES.xhtml.ToString())
            {
                sFileType = DataAppHelpers.Resources.FILEEXTENSION_TYPES.html.ToString();
            }
            if (!sFileType.StartsWith(DataHelpers.GeneralHelpers.FILEEXTENSION_DELIMITER))
            {
                sFileType = string.Concat(
                    DataHelpers.GeneralHelpers.FILEEXTENSION_DELIMITER, sFileType);
            }
            return sFileType;
        }
        private Data.ContentURI SetZipFileState(HttpContext context,
            IContentService contentService, IMemberService memberService,
            Data.ContentURI uri, string contentUriPattern, string uriXmlDocPath,
            bool isFileNameURIPattern, bool needsFullContent)
        {
            Data.ContentURI oZipFileURI = new Data.ContentURI(uri);
            if (isFileNameURIPattern)
            {
                //filename uripatterns are different than std uripatterns
                //filename uripatterns don't use contenturipatterns (so param is a misnomer)
                oZipFileURI.ChangeURIPatternFromFileName(contentUriPattern);
            }
            else
            {
                oZipFileURI.ChangeContentURIPattern(contentUriPattern);
            }
            DataAppHelpers.Resources.AddParentURIPropertiesToResourceURI(uri, oZipFileURI);
            DataHelpers.GeneralHelpers.SetApps(oZipFileURI);
            if (oZipFileURI.URIPattern != string.Empty
                && oZipFileURI.URIFileExtensionType
                != DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                && needsFullContent)
            {
                oZipFileURI.URIClub.ClubDocFullPath = uriXmlDocPath;
                oZipFileURI.URIMember.MemberDocFullPath = uriXmlDocPath;
                if (!DataHelpers.FileStorageIO.URIAbsoluteExists(uri, uriXmlDocPath))
                {
                    //no error message here -it will use directory path to copy files


                    //160 eliminated this because of performance
                    ////avoid this because of performance hit
                    //IContentService contentservice = new Contis entService(oZipFileURI);
                    //bool bIsSaved = await contentservice.SaveURIFirstDocAsync(oZipFileURI);
                    //contentService.Dispose();
                    //if (!DataHelpers.FileStorageIO.URIAbsoluteExists(uriXmlDocPath))
                    //{
                    //    oZipFileURI.URIClub.ClubDocFullPath = string.Empty;
                    //    oZipFileURI.URIMember.MemberDocFullPath = string.Empty;
                    //}
                }
            }
            else
            {
                if (oZipFileURI.URIFileExtensionType
                    == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                {
                    //temp docs are stand-alone file system docs (no ancestors)
                    //this doesn't change 
                    oZipFileURI.URIDataManager.TempDocPath = uriXmlDocPath;
                    //these can change to zipdirectory path
                    oZipFileURI.URIClub.ClubDocFullPath = uriXmlDocPath;
                    oZipFileURI.URIMember.MemberDocFullPath = uriXmlDocPath;
                }
            }
            return oZipFileURI;
        }
        private static void SetZipFileDisplayState(Data.ContentURI zipFileURI)
        {
            //at this stage of development, they only need to read, 
            //not edit, the packaged files on the client
            zipFileURI.URIMember.ClubInUse.PrivateAuthorizationLevel 
                = AccountHelper.AUTHORIZATION_LEVELS.viewonly;
            //read view
            zipFileURI.URIDataManager.UpdatePanelType =
                DataHelpers.GeneralHelpers.UPDATE_PANEL_TYPES.preview;
        }
        private static void AddZipFileURIToList(Data.ContentURI zipFileURI,
            List<Data.ContentURI> zipFileURIs)
        {
            if (!zipFileURIs.Any(
                u => u.URIPattern == zipFileURI.URIPattern))
            {
                zipFileURIs.Add(zipFileURI);
            }
        }
        private static Data.ContentURI GetZipFileFromList(List<Data.ContentURI> zipFileURIs,
            string fileNameURIPattern)
        {
            Data.ContentURI newURI = new Data.ContentURI();
            DataHelpers.GeneralHelpers.SetURIParamsFromFileName(
                newURI, fileNameURIPattern);
            //don't use the urifileextension (some = selected which wasn't added to zipfileuris)
            newURI.URIPattern = Data.ContentURI.ChangeURIPatternPart(newURI.URIPattern, Data.ContentURI.URIPATTERNPART.fileExtension, DataHelpers.GeneralHelpers.NONE);
            Data.ContentURI zipFileURI
                = DataHelpers.LinqHelpers.GetContentURIByURIPattern(zipFileURIs,
                newURI.URIPattern);
            return zipFileURI;
        }
        private async Task SetResourceAsync(IContentService contentService, HttpContext context,
            Data.ContentURI uri, string rootPackageDirectory, string newFullXHtmlFilePath,
            IDictionary<string, string> zipArgs)
        {
            string sResourceRelFilePaths = string.Empty;
            if (uri.URIFileExtensionType !=
                DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //get any resource files (images, video, stylesheets) used by this file
                bool bNeedsFullPath = true;
                bool bNeedsOneRecord = false;
                sResourceRelFilePaths = await contentService.GetResourceURLsAsync(uri, bNeedsFullPath, bNeedsOneRecord,
                    DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.storyuri, string.Empty);
            }
            else
            {
                //will have to parse the xml doc to get the good db resourceids
                bool bNeedsFullPath = true;
                bool bNeedsOneRecord = false;
                sResourceRelFilePaths = await contentService.GetResourceURLsAsync(uri, bNeedsFullPath, bNeedsOneRecord,
                    DataAppHelpers.Resources.RESOURCES_GETBY_TYPES.tempdocs,
                    uri.URIName);
            }
            //add them to newZipArgs using the existing uri's found in the html docs (use sResourceRelFilePaths as is)
            //sResourceRelFilePaths is an array (PARAMETER_DELIMITER) of arrays (STRING_DELIMITER)
            //the second array is: relresourcepath;altdescription;resourceuripattern
            if (string.IsNullOrEmpty(sResourceRelFilePaths) == false)
            {
                //2.0.0 changes
                string sPhysicalFilePath = DataHelpers.AppSettings.ConvertPathFileandWeb(uri, newFullXHtmlFilePath);
                bool bIsSet = await contentService.CopyResourceToPackageAsync(
                    uri, sResourceRelFilePaths, 0,
                    sPhysicalFilePath, rootPackageDirectory,
                    Path.GetFileName(newFullXHtmlFilePath), zipArgs);
            }
        }
        private async Task CopyRelatedDataToPackageAsync(Data.ContentURI uri,
            IContentService contentService, string oldFullXHtmlFilePath,
            string packageName, string fileType, string newFullXHtmlFilePath, 
            IDictionary<string, string> zipArgs)
        {
            //include children data?
            string sRelatedDataType 
                = DataHelpers.GeneralHelpers.GetFormValue(uri, INCLUDE_RELATED_DATA_TYPE);
            //include sibling and children data
            bool bNeedsAllRelatedData = false;
            if (sRelatedDataType == RELATED_DATA_TYPES.yes.ToString())
            {
                //sibling files are included even if sRelatedDataType == no
                //this goes into subfolders
                bNeedsAllRelatedData = true;
               
            }
            await contentService.CopyRelatedDataToPackageAsync(
                uri, oldFullXHtmlFilePath, packageName, 
                fileType, newFullXHtmlFilePath, bNeedsAllRelatedData,
                zipArgs);
        }
        private async Task SetZipArgsResourceAsync(IContentService contentService,
            IMemberService memberService, HttpContext context,
            Data.ContentURI uri, string rootPackageDirectory,
            List<Data.ContentURI> zipFileURIs, string newFullXHtmlFilePath,
            IDictionary<string, string> zipArgs)
        {
            if (zipFileURIs != null)
            {
                IDictionary<string, string> resourceArgs = new Dictionary<string, string>();
                foreach (Data.ContentURI zipuri in zipFileURIs)
                {
                    //refactor: remember that ebooks need to play and read in order 
                    if (zipuri.URIFileExtensionType 
                        == DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        //tempfiles have to do some digging
                        await AddResourceArgsForTempsAsync(contentService, memberService,
                            context, uri, rootPackageDirectory,
                            newFullXHtmlFilePath, zipuri, resourceArgs);
                    }
                    else
                    {
                        await SetResourceAsync(contentService, context, zipuri, rootPackageDirectory,
                            newFullXHtmlFilePath, resourceArgs);
                    }
                }
                AddResourceToZipArgs(resourceArgs, ref zipArgs);
            }
        }
        
        private async Task AddResourceArgsForTempsAsync(IContentService contentService,
            IMemberService memberService, HttpContext context,
            Data.ContentURI uri, string rootPackageDirectory,
            string newFullXHtmlFilePath, Data.ContentURI oZipFileURI, 
            IDictionary<string, string> resourceArgs)
        {
            string sKey = string.Empty;
            string sValue = string.Empty;
            bool bNeedsFullContent = false;
            Data.ContentURI oTempFileURI = null;
            bool bIsFileNameURIPattern = false;
            string[] arrSelectsList
                = DataHelpers.GeneralHelpers.GetTempDocFilePathsSelectsList(uri,
                oZipFileURI.URIDataManager.TempDocPath);
            if (arrSelectsList != null)
            {
                int iArrayLength = arrSelectsList.Length;
                if (iArrayLength > 0)
                {
                    int i = 0;
                    string sChildURIPattern = string.Empty;
                    string sParentURIPattern = string.Empty;
                    for (i = 0; i < iArrayLength; i++)
                    {
                        sChildURIPattern = string.Empty;
                        sParentURIPattern = string.Empty;
                        //selection = childuripattern;parenturipattern (cuts db hits)
                        EditHelpers.AddHelperLinq.GetChildParentURIPatterns(
                            arrSelectsList[i], out sChildURIPattern, out sParentURIPattern);
                        if (!string.IsNullOrEmpty(sChildURIPattern))
                        {
                            oTempFileURI = SetZipFileState(context, contentService,
                                memberService, uri, sChildURIPattern, string.Empty,
                                bIsFileNameURIPattern, bNeedsFullContent);
                            await SetResourceAsync(contentService, context, oTempFileURI, rootPackageDirectory,
                                newFullXHtmlFilePath, resourceArgs);
                        }
                    }
                }
            }
        }
        private static void AddResourceToZipArgs(
            IDictionary<string, string> resourceArgs,
            ref IDictionary<string, string> zipArgs)
        {
            string sKey = string.Empty;
            string sValue = string.Empty;
            foreach (KeyValuePair<string, string> kvp in resourceArgs)
            {
                sKey = kvp.Key;
                sValue = kvp.Value;
                if (!zipArgs.ContainsKey(sKey))
                {
                    zipArgs.Add(sKey, sValue);
                }
            }
        }
        public async Task SendZipFileLinkAsResponseAsync(IContentService contentService, 
            HttpContext context, Data.ContentURI uri,
            string initPackageFilePath, IDictionary<string, string> zipArgs)
        {
            string sErrorMessage = string.Empty;
            //package type?
            string sPackageType = DataHelpers.GeneralHelpers.GetFormValue(uri, PACKAGE_TYPE);
            //digital signature type?
            string sDigitalSignatureType = DataHelpers.GeneralHelpers.GetFormValue(uri, SIGNATURE_TYPE);
            //make a package
            bool bHasSet = await contentService.PackageFilesAsync(uri, initPackageFilePath,
                sPackageType, sDigitalSignatureType, zipArgs);
            //if error
            if (string.IsNullOrEmpty(uri.ErrorMessage)
                && bHasSet)
            {
                string sFileName = string.Empty;
                string sFileSize = string.Empty;
                DataHelpers.FileIO oFileIO = new DataHelpers.FileIO();
                oFileIO.GetFileNameAndSize(initPackageFilePath,
                    out sFileName, out sFileSize);
                oFileIO = null;
                if (!string.IsNullOrEmpty(sFileName))
                {
                    string sDownloadFilePath = string.Empty;
                    DataHelpers.FileStorageIO.PLATFORM_TYPES ePlatform
                        = uri.URIDataManager.PlatformType;
                    if (ePlatform == DataHelpers.FileStorageIO.PLATFORM_TYPES.webserver)
                    {
                        sDownloadFilePath = DataHelpers.AppSettings.SwitchFullandRelTempWebPaths(
                            uri, initPackageFilePath, uri.URIDataManager.DefaultWebDomain);
                        if (string.IsNullOrEmpty(sDownloadFilePath))
                        {
                            uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "IOHELPER_CANTZIP2");
                        }
                    }
                    else if (ePlatform == DataHelpers.FileStorageIO.PLATFORM_TYPES.azure)
                    {
                        sDownloadFilePath = DataHelpers.AppSettings.GetCloudTempBlobURI(
                            uri, sFileName);
                        //put the package in a blob in the temporary container
                        DataHelpers.FileStorageIO fsIO = new DataHelpers.FileStorageIO();
                        //2.0.0 uses same filesystem storage on azure and web and the files MUST be moved to 
                        //prevent exceeding filesystem limit.
                        DataHelpers.FileStorageIO.MoveURIs(
                            uri, initPackageFilePath, sDownloadFilePath);
                        bool bIsSaved = false;
                        if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, sDownloadFilePath))
                        {
                            bIsSaved = true;
                        }
                        //2.0.0 the files have been deleted but can't delete the tempdirectory
                        //because generates an error message about being used in other process
                        //will be recycled in azure when new site deployed
                        //manually deleted on web servers but better to keep testing why this doesn't work
                        //bool bIncludeSubDirs = true;
                        //DataHelpers.FileStorageIO.DeleteDirectory(uri, initPackageFilePath, bIncludeSubDirs);
                        if (!bIsSaved)
                        {
                            uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "BLOB_CANTSAVEZIPBLOB");
                        }
                    }
                    string sText = AppHelper.GetResource("DOWNLOAD_ZIP");
                    if (uri.URIDataManager.ServerSubActionType == 
                        DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.makepackage)
                    {
                        sText = AppHelper.GetResource("DOWNLOAD_PACKAGE");
                    }
                    string sDownloadPackLink = string.Empty;
                    using (StringWriter writer = new StringWriter())
                    {
                        //this builds a mobile download file link (w/o need for form postback)
                        HtmlExtensions.LinkMobile(
                            id: "lnkDownloadPack",
                            href: sDownloadFilePath,
                            classAttribute: string.Empty,
                            text: sText,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "check",
                            dataIconPos: "left")
                            .WriteTo(writer, HtmlEncoder.Default);
                        sDownloadPackLink = writer.ToString();
                    }
                    string[] arrHtml = { sDownloadPackLink, " ", sFileName, " (", sFileSize, " bytes)" };
                    uri.Message = System.Net.WebUtility.HtmlDecode(
                        DataHelpers.GeneralHelpers.MakeString(arrHtml));
                }
                else
                {
                    uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "IOHELPER_CANTFINDZIP");
                }
            }
            else
            {
                uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    sErrorMessage, "IOHELPER_CANTZIP2");
            }
        }
        public static string GetPackageMainDocPath(Data.ContentURI uri)
        {
            string sMainXmlDocPath = string.Empty;
            Data.ContentURI selectedLinkedViewURI
                = DataHelpers.LinqHelpers.GetLinkedViewIsSelectedView(uri);
            if (uri.URIDataManager.UseSelectedLinkedView == true
                && uri.URINodeName == DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                //they are running a stand alone calculator, analyzer ...
                //the tempdocs hold the temporary calculations (not the main docs)
                if (selectedLinkedViewURI != null)
                {
                    if (DataHelpers.FileStorageIO.URIAbsoluteExists(uri, 
                        selectedLinkedViewURI.URIDataManager.TempDocPath))
                    {
                        sMainXmlDocPath = selectedLinkedViewURI.URIDataManager.TempDocPath;
                    }
                }
            }
            if (sMainXmlDocPath == string.Empty)
            {
                //check for devpacks
                if (selectedLinkedViewURI != null)
                {
                    //160 deprecated separate file storage for guests
                    sMainXmlDocPath = selectedLinkedViewURI.URIClub.ClubDocFullPath;
                }
            }
            if (sMainXmlDocPath == string.Empty)
            {
                sMainXmlDocPath = uri.URIClub.ClubDocFullPath;
            }
            if (sMainXmlDocPath != string.Empty)
            {
                //switch from filepath delimited to webpath delimiter (because of // issues)
                sMainXmlDocPath = sMainXmlDocPath.Replace(
                    DataHelpers.GeneralHelpers.FILE_PATH_DELIMITER,
                    DataHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITER);
            }
            return sMainXmlDocPath;
        }
    }
}
