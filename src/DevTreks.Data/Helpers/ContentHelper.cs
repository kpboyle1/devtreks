using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DevTreks.Data.Helpers
{
    /// <summary>
    ///Purpose:		General utilities supporting content model building
    ///Author:		www.devtreks.org
    ///Date:		2014, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class ContentHelper
    {
        public ContentHelper()
        {
        }
        //public const string TEMPDOCS_DIRECTORYNAME = "tempdocs";
        /// <summary>
        ///path accumulates from contract to service to app node
        ///standard filepath rule enforced: 
        ///owning club is club_accountid/
        /// </summary>
        /// <param name="uri"></param>
        public static void SetFilePaths(ContentURI uri)
        {
            string sPathDelimiter 
                = FileStorageIO.GetDelimiterForFileStorage(uri);
            int iAncestorCount = (uri.URIDataManager.Ancestors != null) ?
                uri.URIDataManager.Ancestors.Count : 0;
            if (iAncestorCount > 0
                && uri.URINodeName.StartsWith(AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                == false)
            {
                int i = 0;
                foreach (ContentURI ancestorUri in uri.URIDataManager.Ancestors)
                {
                    if (i == 0)
                    {
                        uri.URIClub.ClubDocFullPath = AppSettings.MakeFilePath(sPathDelimiter,
                            GeneralHelpers.FILENAME_DELIMITER, 
                            AppSettings.GetResourceRootPath(uri, uri.URINetwork.WebFileSystemPath),
                            uri.URINetwork.PKId.ToString(), AppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString(),
                            uri.URIClub.PKId.ToString(), uri.URIDataManager.AppType.ToString(),
                            uri.URIDataManager.SubAppType.ToString(),
                            AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString(),
                            uri.URIService.ServiceId.ToString(), ancestorUri.URINodeName,
                            ancestorUri.URIId.ToString());
                    }
                    else
                    {
                        uri.URIClub.ClubDocFullPath = string.Concat(
                            uri.URIClub.ClubDocFullPath, sPathDelimiter, 
                            ancestorUri.URINodeName, GeneralHelpers.FILENAME_DELIMITER, 
                            ancestorUri.URIId.ToString(), string.Empty);
                    }
                    i++;
                }
                uri.URIClub.ClubDocFullPath = string.Concat(uri.URIClub.ClubDocFullPath, 
                    sPathDelimiter, uri.URINodeName, 
                    GeneralHelpers.FILENAME_DELIMITER, uri.URIId.ToString(), string.Empty);
            }
            else
            {
                if (uri.URINodeName != AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                    && uri.URINodeName != AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                {
                    uri.URIClub.ClubDocFullPath = AppSettings.MakeFilePath(sPathDelimiter,
                        GeneralHelpers.FILENAME_DELIMITER,
                        AppSettings.GetResourceRootPath(uri, uri.URINetwork.WebFileSystemPath),
                        uri.URINetwork.PKId.ToString(), AppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString(),
                        uri.URIClub.PKId.ToString().ToString(), uri.URIDataManager.AppType.ToString(),
                        uri.URIDataManager.SubAppType.ToString(), AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString(),
                        uri.URIService.ServiceId.ToString(), uri.URINodeName, uri.URIId.ToString());
                }
                else
                {
                    uri.URIClub.ClubDocFullPath = AppSettings.MakeFilePath(sPathDelimiter,
                        GeneralHelpers.FILENAME_DELIMITER,
                        AppSettings.GetResourceRootPath(uri, uri.URINetwork.WebFileSystemPath),
                        uri.URINetwork.PKId.ToString(), AppHelpers.Accounts.ACCOUNT_GROUPS.club.ToString(),
                        uri.URIClub.PKId.ToString().ToString(), uri.URIDataManager.AppType.ToString(),
                        uri.URIDataManager.SubAppType.ToString(),
                        AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString(),
                        uri.URIService.ServiceId.ToString(), string.Empty, string.Empty);
                }
            }
            uri.URIClub.ClubDocFullPath = string.Concat(
                uri.URIClub.ClubDocFullPath, sPathDelimiter,
                MakeStandardFileNameFromURIPattern(uri));
            SetMemberPath(uri);
            SetSelectedLinkedViewPaths(uri);
            FixFilePaths(uri);
        }
        public static void SetFilePathsForTempLinkedView(ContentURI uri)
        {
            if (uri.URIDataManager.TempDocURIPattern == string.Empty)
                uri.URIDataManager.TempDocURIPattern = uri.URIPattern;
            if (uri.URIClub.ClubDocFullPath == string.Empty)
            {
                AppSettings.SetTempDocPathandFileName(uri);
                uri.URIDataManager.EditViewEditType
                    = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
            }
            Helpers.LinqHelpers.SetLinkedViewTempDocPaths(
                uri);
        }
        
        public static string MakeStandardFileNameFromURIPattern(ContentURI uri)
        {
            string sFileName = AppHelpers.Calculator.cName;
            if (uri != null)
            {
                //{name}_{id}_{networkname}_{nodename}_{fileextensiontype}
                //the filename needs to use the full uripattern because
                //when files are packaged, they find related resources (such as 
                //calculator stylesheets) from the filename's uripattern (i.e. 
                //a calculator doc stored in a budget path).
                //don't use paginated html or xml -packages and linkedview use full uri content
                //no matter how big
                if (AddInHelper.IsAddIn(uri))
                {
                    //distinguish calculated files from other files
                    sFileName = GeneralHelpers.ADDIN;
                }
                //filename = uriname_uriid_networkname_fileextensiontype
                StringBuilder oStrBldr = new StringBuilder();
                oStrBldr.Append(sFileName);
                oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oStrBldr.Append(uri.URIId);
                oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oStrBldr.Append(uri.URINetworkPartName);
                oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oStrBldr.Append(uri.URINodeName);
                if ((!string.IsNullOrEmpty(uri.URIFileExtensionType))
                    && uri.URIFileExtensionType.ToLower() != Helpers.GeneralHelpers.NONE.ToLower())
                {
                    oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                    oStrBldr.Append(uri.URIFileExtensionType);
                }
                sFileName = oStrBldr.ToString();
                //analyses can exceed windows 260 char limit; so limit the name in filepaths
                sFileName = FixFilePathLength(uri, sFileName);
            }
            return sFileName;
        }
        public static string MakeStandardFileNameFromURIPattern(string uriPattern)
        {
            string sFileName = string.Empty;
            string sId = ContentURI.GetURIPatternPart(uriPattern,
                ContentURI.URIPATTERNPART.id);
            string sNodeName = ContentURI.GetURIPatternPart(uriPattern,
                ContentURI.URIPATTERNPART.node);
            string sNetworkName = ContentURI.GetURIPatternPart(uriPattern,
                ContentURI.URIPATTERNPART.network);
            int iCheckNetworkName = Helpers.GeneralHelpers.ConvertStringToInt(sNetworkName);
            if (iCheckNetworkName != 0)
            {
                ContentURI uri = new ContentURI();
                ContentURI needNetworkNameURI = ContentURI.ConvertShortURIPattern(uriPattern);
                sNetworkName = needNetworkNameURI.URINetworkPartName;
            }
            string sFileExtType = ContentURI.GetURIPatternPart(uriPattern,
                ContentURI.URIPATTERNPART.fileExtension);
            //addins always have a fileextensiontype that is not temp and is not string.empty
            string sName = (!string.IsNullOrEmpty(sFileExtType)
                && sFileExtType != Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                ? GeneralHelpers.ADDIN : AppHelpers.Calculator.cName;
            //filename = uriname_uriid_networkname_fileextensiontype
            StringBuilder oStrBldr = new StringBuilder();
            oStrBldr.Append(sName);
            oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
            oStrBldr.Append(sId);
            oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
            oStrBldr.Append(sNetworkName);
            oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
            oStrBldr.Append(sNodeName);
            if ((!string.IsNullOrEmpty(sFileExtType))
                && sFileExtType.ToLower() != Helpers.GeneralHelpers.NONE.ToLower())
            {
                oStrBldr.Append(Helpers.GeneralHelpers.FILENAME_DELIMITER);
                oStrBldr.Append(sFileExtType);
            }
            sFileName = oStrBldr.ToString();
            return sFileName;
        }
        public static string MakeNewFilePathFromURIPattern(
            string newUriPattern, string existingFilePath)
        {
            string sNewFilePath = string.Empty;
            string sFullFileName = MakeStandardFileNameFromURIPattern(newUriPattern);
            string sExistingFileName
                = Path.GetFileNameWithoutExtension(existingFilePath);
            sNewFilePath = existingFilePath
                .Replace(sExistingFileName, sFullFileName);
            return sNewFilePath;
        }
        public static string FixFilePathLength(ContentURI uri, 
            string newFileNamePattern)
        {
            string sNewFileName = newFileNamePattern;
            string sDelimiter = FileStorageIO.GetDelimiterForFileStorage(uri.URIClub.ClubDocFullPath);
            if (!string.IsNullOrEmpty(uri.URIClub.ClubDocFullPath))
            {
                string sFileExtension = Path.GetExtension(uri.URIClub.ClubDocFullPath);
                int iFileExtLength = (string.IsNullOrEmpty(sFileExtension))
                    ? 4 : 0;
                string sOldFileName = string.Empty;
                if (uri.URIClub.ClubDocFullPath.EndsWith(sDelimiter))
                {
                    sOldFileName = string.Empty;
                }
                else
                {
                    if (Helpers.FileStorageIO.URIAbsoluteExists(
                        uri, uri.URIClub.ClubDocFullPath))
                    {
                        sOldFileName
                            = Path.GetFileNameWithoutExtension(uri.URIClub.ClubDocFullPath);
                    }
                    else if (Helpers.FileStorageIO.DirectoryExists(
                        uri, string.Concat(uri.URIClub.ClubDocFullPath,
                        sDelimiter)))
                    {
                        sOldFileName = string.Empty;
                    }
                    else
                    {
                        sOldFileName
                            = Path.GetFileNameWithoutExtension(uri.URIClub.ClubDocFullPath);
                    }
                }
                string sCommonName
                    = Helpers.GeneralHelpers.GetSubstringFromFront(
                        newFileNamePattern, Helpers.GeneralHelpers.FILENAME_DELIMITERS,
                        1);
                string sNewCommonName = sCommonName;
                RuleHelpers.GeneralRules.ValidateXSDInput(
                    AppHelpers.Calculator.cName, ref sNewCommonName,
                    RuleHelpers.GeneralRules.STRING, RuleHelpers.GeneralRules.NAME_SIZE);
                sNewFileName = sNewFileName.Replace(sCommonName, sNewCommonName);
                string sNewFilePath = string.Empty;
                if (string.IsNullOrEmpty(sOldFileName))
                {
                    if (uri.URIClub.ClubDocFullPath.EndsWith(sDelimiter))
                    {
                        sNewFilePath = string.Concat(uri.URIClub.ClubDocFullPath, sNewFileName);
                    }
                    else
                    {
                        sNewFilePath = string.Concat(uri.URIClub.ClubDocFullPath, 
                            sDelimiter, sNewFileName);
                    }
                }
                else
                {
                    sNewFilePath = uri.URIClub.ClubDocFullPath.Replace(sOldFileName, sNewFileName);
                }
                int iFilePathLength = sNewFilePath.Length + iFileExtLength;
                if (iFilePathLength > 259)
                {
                    //258 accomodates extra letter in guest vs club
                    int iCommonNameAllowedLength
                        = sNewCommonName.Length - (iFilePathLength - 258);
                    //must accomodate common names of "Name" and "AddIn", so 5 chars limit
                    if (iCommonNameAllowedLength <= 0)
                        iCommonNameAllowedLength = 5;
                    if (sNewCommonName.Length > 5)
                    {
                        sNewCommonName = sNewCommonName.Substring(0, iCommonNameAllowedLength);
                    }
                    else
                    {
                        // use sNewCommonName
                    }
                    //names must also comply with generalrules
                    sNewCommonName = sNewCommonName.Trim();
                    sNewFileName = newFileNamePattern.Replace(sCommonName, sNewCommonName);
                }
            }
            return sNewFileName;
        }
        public static string GetFullCalculatorResultsPath(string summaryDocPath)
        {
            //get the full calculated results file using the summary calculated
            //results file
            string sFullCalculatorDocPath = string.Empty;
            //operation.xml to operation_full.xml
            string sFullDocPathSuffix = string.Concat(
                GeneralHelpers.FILENAME_DELIMITER,
                GeneralHelpers.FILENAME_EXTENSIONS.full.ToString(),
                GeneralHelpers.EXTENSION_XML);
            sFullCalculatorDocPath
                = summaryDocPath.Replace(GeneralHelpers.EXTENSION_XML, sFullDocPathSuffix);
            return sFullCalculatorDocPath;

        }
        public static string GetURINameFromStandardFileName(string fileName)
        {
            string sURIName = Helpers.GeneralHelpers.GetSubstringFromFront(fileName,
                Helpers.GeneralHelpers.FILENAME_DELIMITERS, 1);
            return sURIName;
        }
        public static void UpdateDocToCalcPathsUsingNewFileExtensionType(
            ContentURI docToCalcURI, ContentURI calcDocURI, string fileExtensionType)
        {
            //doctopath may not have initialized with this calculator yet, 
            //so don't use doctocalc params
            string sNewURIPattern = GeneralHelpers.MakeURIPattern(calcDocURI.URIName,
                calcDocURI.URIId.ToString(), calcDocURI.URINetworkPartName, 
                calcDocURI.URINodeName, fileExtensionType);
            string sNewFileName
                = MakeStandardFileNameFromURIPattern(sNewURIPattern);
            sNewFileName = FixFilePathLength(docToCalcURI, sNewFileName);
            string sOldFileName = Path.GetFileNameWithoutExtension(
                docToCalcURI.URIClub.ClubDocFullPath);
            docToCalcURI.URIClub.ClubDocFullPath
                = docToCalcURI.URIClub.ClubDocFullPath.Replace(
                sOldFileName, sNewFileName);
            docToCalcURI.URIMember.MemberDocFullPath
                = docToCalcURI.URIMember.MemberDocFullPath.Replace(
                sOldFileName, sNewFileName);
        }
        public static void FixFilePaths(ContentURI uri)
        {
            uri.URIClub.ClubDocFullPath = FixFilePath(uri.URIClub.ClubDocFullPath);
            uri.URIMember.MemberDocFullPath = FixFilePath(uri.URIMember.MemberDocFullPath);
        }
        public static string FixFilePath(string devTrekPath)
        {
            string sFullDevTrekPath = devTrekPath;
            if (!string.IsNullOrEmpty(devTrekPath))
            {
                //string sPartFullPath = devTrekPath.Replace("/", @"\");
                string sXmlExtension = Helpers.GeneralHelpers.EXTENSION_XML;
                if (devTrekPath.EndsWith(sXmlExtension) == false)
                {
                    sFullDevTrekPath = string.Concat(devTrekPath, Helpers.GeneralHelpers.EXTENSION_XML);
                }
            }
            return sFullDevTrekPath;
        }
        public static bool NodeCanHaveFile(string fullFilePath)
        {
            bool bNodeCanHaveFile = true;
            //budgetoperation and investmentcomponent nodes result in filepath
            //lengths that can easily exceed 260 char limit
            //they aren't needed for anything, so don't build, copy, or package them
            if (fullFilePath.ToLower().Contains(AppHelpers.Economics1.BUDGET_TYPES.budgetoperation.ToString()))
            {
                bNodeCanHaveFile = false;
            }
            else if (fullFilePath.ToLower().Contains(AppHelpers.Economics1.INVESTMENT_TYPES.investmentcomponent.ToString()))
            {
                bNodeCanHaveFile = false;
            }
            else if (fullFilePath.ToLower().Contains(AppHelpers.Economics1.BUDGET_TYPES.budgetoutcome.ToString()))
            {
                bNodeCanHaveFile = false;
            }
            else if (fullFilePath.ToLower().Contains(AppHelpers.Economics1.INVESTMENT_TYPES.investmentoutcome.ToString()))
            {
                bNodeCanHaveFile = false;
            }
            return bNodeCanHaveFile;
        }
        public static void SetSelectedLinkedViewPaths(ContentURI uri)
        {
            //all views, whether of uri or selectedlinkedview, get stored in their own path
            if (uri.URIDataManager.LinkedView != null)
            {
                //try to set a selectedlinkedview and an addin paths
                ContentURI selectedLinkedView
                    = LinqHelpers.GetLinkedViewIsSelectedView(uri);
                if (selectedLinkedView != null)
                {
                    SetLinkedViewPaths(uri, selectedLinkedView);
                    FixFilePaths(selectedLinkedView);
                    ContentURI selectedLinkedViewAddIn = LinqHelpers.GetLinkedViewIsSelectedAddIn(selectedLinkedView);
                    if (selectedLinkedViewAddIn != null)
                    {
                        SetLinkedViewPaths(selectedLinkedView, selectedLinkedViewAddIn);
                        FixFilePaths(selectedLinkedViewAddIn);
                    }
                }
                else
                {
                    //try to set an addin path
                    ContentURI selectedLinkedViewAddIn = LinqHelpers.GetLinkedViewIsSelectedAddIn(uri);
                    if (selectedLinkedViewAddIn != null)
                    {
                        SetLinkedViewPaths(uri, selectedLinkedViewAddIn);
                        FixFilePaths(selectedLinkedViewAddIn);
                    }
                }
            }
        }
        
        private static void SetDefaultLinkedViewPaths(ContentURI uri,
            ContentURI defaultLinkedView)
        {
            if (uri.URINodeName ==
                AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString()
                || uri.URINodeName ==
                AppHelpers.DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                string sPathDelimiter
                    = Helpers.FileStorageIO.GetDelimiterForFileStorage(uri.URIClub.ClubDocFullPath);
                //directory
                defaultLinkedView.URIClub.ClubDocFullPath = GeneralHelpers.MakeString(uri.URIClub.ClubDocFullPath,
                    sPathDelimiter, defaultLinkedView.URINodeName,
                    GeneralHelpers.FILENAME_DELIMITER, defaultLinkedView.URIId.ToString(), string.Empty);
                //file path
                defaultLinkedView.URIClub.ClubDocFullPath = string.Concat(
                    defaultLinkedView.URIClub.ClubDocFullPath, sPathDelimiter,
                    MakeStandardFileNameFromURIPattern(defaultLinkedView), GeneralHelpers.EXTENSION_XML);
            }
            else
            {
                //rule is that default views share uri's path 
                //they are the first doc to load when a restful uri is called
                defaultLinkedView.URIClub.ClubDocFullPath = uri.URIClub.ClubDocFullPath;
                SetMemberPath(defaultLinkedView);
            }
        }
        public static void SetLinkedViewPaths(ContentURI uri,
            ContentURI selectedLinkedView)
        {
            string sPathDelimiter
                    = Helpers.FileStorageIO.GetDelimiterForFileStorage(uri.URIClub.ClubDocFullPath);
            string sNewViewFullMemberDocPath = string.Empty;
            string sNewViewFullClubDocPath = string.Empty;
            if (uri.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews
                || uri.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                sNewViewFullClubDocPath = GetFilePathWithNoFileName(uri);
                if (uri.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks
                    && selectedLinkedView.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews)
                {
                    //a linked view linkedview doing the calcs, analyses or telling a story
                    //uses the directory of the devpacks uri
                    //file path
                    selectedLinkedView.URIClub.ClubDocFullPath = string.Concat(
                       sNewViewFullClubDocPath, sPathDelimiter,
                       MakeStandardFileNameFromURIPattern(selectedLinkedView), GeneralHelpers.EXTENSION_XML);
                }
                else 
                {
                    if (uri.URIPattern.Contains(selectedLinkedView.URIDataManager.ParentURIPattern))
                    {
                        //selectedview is a direct child of uri
                        //directory
                        selectedLinkedView.URIClub.ClubDocFullPath = string.Concat(sNewViewFullClubDocPath,
                            sPathDelimiter, selectedLinkedView.URINodeName,
                            GeneralHelpers.FILENAME_DELIMITER, selectedLinkedView.URIId.ToString(), 
                            string.Empty);
                        //file path
                        selectedLinkedView.URIClub.ClubDocFullPath = string.Concat(
                            selectedLinkedView.URIClub.ClubDocFullPath, sPathDelimiter,
                            MakeStandardFileNameFromURIPattern(selectedLinkedView), GeneralHelpers.EXTENSION_XML);

                    }
                    else
                    {
                        if (selectedLinkedView.URIDataManager.ParentURIPattern != string.Empty)
                        {
                            //selectedview is a child of selectedLinkedView.URIDataManager.ParentURIPattern
                            //refactor: test with recursive subfolders that are more than 2 deep (selectedview.ancestors needed)
                            string sParentNodeName = ContentURI.GetURIPatternPart(selectedLinkedView.URIDataManager.ParentURIPattern,
                                ContentURI.URIPATTERNPART.node);
                            string sParentId = ContentURI.GetURIPatternPart(selectedLinkedView.URIDataManager.ParentURIPattern,
                                ContentURI.URIPATTERNPART.id);
                            //directory
                            selectedLinkedView.URIClub.ClubDocFullPath = string.Concat(sNewViewFullClubDocPath,
                                sPathDelimiter, sParentNodeName,
                                GeneralHelpers.FILENAME_DELIMITER, sParentId,
                                sPathDelimiter, selectedLinkedView.URINodeName,
                                GeneralHelpers.FILENAME_DELIMITER, selectedLinkedView.URIId.ToString());
                            //file path
                            selectedLinkedView.URIClub.ClubDocFullPath = string.Concat(
                                selectedLinkedView.URIClub.ClubDocFullPath, sPathDelimiter,
                                MakeStandardFileNameFromURIPattern(selectedLinkedView), GeneralHelpers.EXTENSION_XML);
                        }
                        else
                        {
                            uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "CONTENTHELPER_SELECTHASNOPARENT");
                        }
                    }
                }
            }
            else
            {
                AppHelpers.LinkedViews.GetNewLinkedViewDocPaths(uri, MakeStandardFileNameFromURIPattern(selectedLinkedView),
                    out sNewViewFullMemberDocPath, out sNewViewFullClubDocPath);
                selectedLinkedView.URIClub.ClubDocFullPath = sNewViewFullClubDocPath;
            }
            SetMemberPath(selectedLinkedView);
        }
        private static string GetFilePathWithNoFileName(ContentURI uri)
        {
            string sFilePath = string.Empty;
            string sFileName = Path.GetFileName(uri.URIClub.ClubDocFullPath);
            string sPathDelimiter
                    = Helpers.FileStorageIO.GetDelimiterForFileStorage(uri.URIClub.ClubDocFullPath);
            if (!string.IsNullOrEmpty(sFileName))
            {
                sFileName = string.Concat(sPathDelimiter, sFileName);
                sFilePath = uri.URIClub.ClubDocFullPath.Replace(sFileName, string.Empty);
            }
            return sFilePath;
        }
        public static void SetMemberPath(ContentURI uri)
        {
            //160 deprecated separate file storage for guests
            uri.URIMember.MemberDocFullPath = uri.URIClub.ClubDocFullPath;
        }
        
        public async Task<string> GetAdminAncestorsAsync(ContentURI uri)
        {
            string sAncestorArray = string.Empty;
            //commons ancestors use default network (if not, 
            //keep memberclass, networkclass, and accountclass in synch)
            int iNetworkId = 0;
            string sNetworkName = string.Empty;
            string sNetworkPartName = string.Empty;
            int iNetworkGroupId = 0;
            string sNetworkGroupName = string.Empty;
            GeneralHelpers.GetDefaultNetworkSettings(out iNetworkId, out sNetworkName,
                out sNetworkPartName, out iNetworkGroupId, out sNetworkGroupName);
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
            {
			    sqlIO.MakeInParam("@Id",				    SqlDbType.Int, 4, uri.URIId),
                sqlIO.MakeInParam("@NodeName",		        SqlDbType.NVarChar, 50, uri.URINodeName),
                sqlIO.MakeInParam("@NetworkPartName",		SqlDbType.NVarChar, 20, sNetworkPartName),
                sqlIO.MakeOutParam("@AncestorNameArray",    SqlDbType.NVarChar, 255)
		    };
            string sQry = "0GetAncestorNamesForAdmins";
            int iNotUsed = await sqlIO.RunProcIntAsync(sQry, oPrams);
            if (oPrams[3].Value != System.DBNull.Value)
            {
                sAncestorArray = oPrams[3].Value.ToString();
            }
            sqlIO.Dispose();
            return sAncestorArray;
        }
        public async Task<string> GetURIAncestorsAsync(ContentURI uri)
        {
            string sAncestorArray = string.Empty;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams = GetAncestorParams(uri, sqlIO);
            string sQry = GetQry(uri);
            int iNotUsed = await sqlIO.RunProcIntAsync(sQry, oPrams);
            if (oPrams[4].Value != System.DBNull.Value)
            {
                //return the ancestor array for conversion into an IEnumerable ContentURI list
                sAncestorArray = oPrams[4].Value.ToString();
            }
            if (oPrams[5].Value != System.DBNull.Value)
            {
                //get the uri.URIService base id
                int iServiceId = (int) oPrams[5].Value;
                //view will set the service object for consistency 
                uri.URIService.ServiceId = iServiceId;
            }
            if (oPrams[6].Value != System.DBNull.Value)
            {
                //if needed, fix the uri's name
                string sCurrentNameFromClient = uri.URIName;
                string sNewName = oPrams[6].Value.ToString();
                RuleHelpers.ResourceRules.ValidateScriptArgument(ref sNewName);
                uri.URIName = sNewName;
                //make sure this is the same name sent by the client (i.e. no edit*)
                if (sCurrentNameFromClient.Equals(uri.URIName) == false)
                {
                    uri.URIPattern = uri.URIPattern.Replace(sCurrentNameFromClient, 
                        uri.URIName);
                }

            }
            if (oPrams[7].Value != System.DBNull.Value)
            {
                //set the uri's docstatus
                uri.URIDataManager.DocStatus = GeneralHelpers.GetDocsStatus(oPrams[7].Value.ToString());
            }
            else
            {
                uri.URIDataManager.DocStatus = GeneralHelpers.DOCS_STATUS.notreviewed;
            }
            if (oPrams[8].Value != System.DBNull.Value)
            {
                //v1.7.1 set the uri's description for seo
                uri.URIDataManager.Description = oPrams[8].Value.ToString();
            }
            sqlIO.Dispose();
            return sAncestorArray;
        }
        private static SqlParameter[] GetAncestorParams(ContentURI uri,
            SqlIOAsync data)
        {
            SqlParameter[] oPrams =
            {
		        data.MakeInParam("@Id",				    SqlDbType.Int, 4, uri.URIId),
                data.MakeInParam("@NodeName",		    SqlDbType.NVarChar, 50, uri.URINodeName),
                data.MakeInParam("@NetworkPartName",	SqlDbType.NVarChar, 20, uri.URINetworkPartName),
                data.MakeInParam("@ParamDelimiter",	    SqlDbType.NVarChar, 2, GeneralHelpers.PARAMETER_DELIMITER),
                data.MakeOutParam("@AncestorNameArray", SqlDbType.NVarChar, 2000),
                data.MakeOutParam("@ServiceId",		    SqlDbType.Int, 4),
                data.MakeOutParam("@CurrentName",	    SqlDbType.NVarChar, 75),
                data.MakeOutParam("@DocStatus",		    SqlDbType.Int, 4),
                data.MakeOutParam("@Description",	    SqlDbType.NVarChar, 255),
	        };
            return oPrams;
        }
        private static string GetQry(ContentURI uri)
        {
            string sQry = string.Empty;
            switch (uri.URIDataManager.AppType)
            {
                case GeneralHelpers.APPLICATION_TYPES.economics1:
                    if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
                    {
                        sQry = "0GetAncestorNamesForBudgets";
                    }
                    else
                    {
                        sQry = "0GetAncestorNamesForInvestments";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.prices:
                    if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
                    {
                        sQry = "0GetAncestorNamesForInputs";
                    }
                    else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
                    {
                        sQry = "0GetAncestorNamesForOutputs";
                    }
                    else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
                    {
                        sQry = "0GetAncestorNamesForOutcomes";
                    }
                    else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
                    {
                        sQry = "0GetAncestorNamesForOperations";
                    }
                    else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
                    {
                        sQry = "0GetAncestorNamesForComponents";
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.devpacks:
                    sQry = "0GetAncestorNamesForDevPacks";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    sQry = "0GetAncestorNamesForLinkedViews";
                    break;
                case GeneralHelpers.APPLICATION_TYPES.resources:
                    sQry = "0GetAncestorNamesForResources";
                    break;
                default:
                    sQry = "0GetAncestorNamesForLinkedViews";
                    break;
            }
            return sQry;
        }
        //private static string GetQry(ContentURI uri)
        //{
        //    string sQry = string.Empty;
        //    switch (uri.URIDataManager.AppType)
        //    {
        //        case GeneralHelpers.APPLICATION_TYPES.economics1:
        //            if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.budgets)
        //            {
        //                sQry = "0GetAncestorNamesForBudgets";
        //            }
        //            else
        //            {
        //                sQry = "0GetAncestorNamesForInvestments";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.prices:
        //            if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.inputprices)
        //            {
        //                sQry = "0GetAncestorNamesForInput";
        //            }
        //            else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.outputprices)
        //            {
        //                sQry = "0GetAncestorNamesForOutput";
        //            }
        //            else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices)
        //            {
        //                sQry = "0GetAncestorNamesForOutcome";
        //            }
        //            else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.operationprices)
        //            {
        //                sQry = "0GetAncestorNamesForOperation";
        //            }
        //            else if (uri.URIDataManager.SubAppType == GeneralHelpers.SUBAPPLICATION_TYPES.componentprices)
        //            {
        //                sQry = "0GetAncestorNamesForComponent";
        //            }
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.devpacks:
        //            sQry = "0GetAncestorNamesForDevPack";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.linkedviews:
        //            sQry = "0GetAncestorNamesForLinkedView";
        //            break;
        //        case GeneralHelpers.APPLICATION_TYPES.resources:
        //            sQry = "0GetAncestorNamesForResource";
        //            break;
        //        default:
        //            sQry = "0GetAncestorNamesForLinkedView";
        //            break;
        //    }
        //    return sQry;
        //}

        public async Task<SqlDataReader> GetURIChildrenAsync(SqlIOAsync sqlIO, ContentURI parentURI,
            ContentURI uri, ContentURI childrenParentURI)
        {
            SqlDataReader dataReader = null;
            childrenParentURI = new ContentURI(parentURI);
            //returns Web.ViewDataHelper.PAGE_SIZE records at a time
            bool bIsPageSearch = false;
            string sQry = SearchHelper.GetQry(uri, bIsPageSearch);
            if (sQry != string.Empty && sQry != null)
            {
                //returns pkid, name record list (children of the parent displayed at top of list)
                int iParentId = 0;
                string sParentNodeName = string.Empty;
                //this has to be retested: it changes childrenParentURI
                SetParentSearchNames(parentURI, uri, childrenParentURI,
                    out iParentId, out sParentNodeName);
                SqlParameter[] oPrams = GetChildrenSearchParams(iParentId, sParentNodeName,
                    uri, sqlIO);
                dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
                if (dataReader != null)
                {
                    uri.URIDataManager.RowCount = dataReader.RecordsAffected;
                    if (uri.URIDataManager.RowCount > 0)
                    {
                        if (dataReader.HasRows == false && uri.URIDataManager.StartRow > 0)
                        {
                            //don't return empty children views, it looks like a bug
                            //reset the start row and rerun the qry
                            uri.URIDataManager.StartRow = 0;
                            oPrams = GetChildrenSearchParams(iParentId, sParentNodeName,
                                uri, sqlIO);
                            dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
                            if (dataReader != null)
                            {
                                uri.URIDataManager.RowCount = dataReader.RecordsAffected;
                            }

                        }
                    }
                }
                //initialize parameters needed (uripattern) for the childrens' onclick event 
                //held in dataReader (i.e. childrennodename)
                GeneralHelpers.SetAppSearchView(iParentId, sParentNodeName, uri);
            }
            return dataReader;
        }
        
        private void SetParentSearchNames(ContentURI parentURI,
            ContentURI uri, ContentURI childrenParentURI,
            out int parentId, out string parentNodeName)
        {
            parentId = 0;
            parentNodeName = string.Empty;
            bool bNeedsToBeInChildrenList = NeedsToBeInChildrenList(uri);
            if (bNeedsToBeInChildrenList)
            {
                //parent is the parent of the node clicked 
                //(no reason to advance the toc if a blank toc is the result)
                parentId = parentURI.URIId;
                parentNodeName = parentURI.URINodeName;
            }
            else
            {
                //parent is the node clicked
                parentId = uri.URIId;
                parentNodeName = uri.URINodeName;
                if (parentNodeName
                    == AppHelpers.Agreement.AGREEMENT_TYPES.service.ToString())
                {
                    //currentId is accounttoserviceid, make parentid serviceid
                    parentId = uri.URIService.ServiceId;
                }
                childrenParentURI.URIId = parentId;
                childrenParentURI.URINodeName = parentNodeName;
                childrenParentURI.UpdateURIPattern();
            }
           
        }
        public static bool NeedsToBeInChildrenList(ContentURI uri)
        {
            bool bNeedsToBeInChildrenList = false;
            //161 removed series because its confusing when running calcs
            //if (uri.URINodeName.Equals(AppHelpers.Prices.INPUT_PRICE_TYPES.inputseries.ToString())
            //    || uri.URINodeName.Equals(AppHelpers.Prices.OUTPUT_PRICE_TYPES.outputseries.ToString())
             if (uri.URINodeName.Equals(AppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                || uri.URINodeName.Equals(AppHelpers.Networks.NETWORK_TYPES.network.ToString())
                || uri.URINodeName.Equals(AppHelpers.Networks.NETWORK_BASE_TYPES.networkbase.ToString())
                || uri.URINodeName.Equals(AppHelpers.Locals.LOCAL_TYPES.local.ToString())
                || uri.URINodeName.Equals(AppHelpers.AddIns.ADDIN_TYPES.addin.ToString())
                || uri.URINodeName.Equals(AppHelpers.Members.MEMBER_TYPES.member.ToString())
                || uri.URINodeName.Equals(AppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString())
                || uri.URINodeName.Equals(AppHelpers.Accounts.ACCOUNT_TYPES.account.ToString())
                || uri.URINodeName.Equals(AppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString()))
            {
                //never show an empty list in selects view (these nodes will appear highligted as children in selects view)
                bNeedsToBeInChildrenList = true;
            }
            return bNeedsToBeInChildrenList;
        }
        private static SqlParameter[] GetChildrenSearchParams(int parentId, string parentNodeName,
            ContentURI uri, SqlIOAsync data)
        {
            int iIsForward = GeneralHelpers.ConvertStringToInt(uri.URIDataManager.IsForward);
            if (iIsForward != 1 || iIsForward != 0) iIsForward = 1;
            SqlParameter[] oPrams =
            {
                data.MakeInParam("@AccountId",			SqlDbType.Int, 4, uri.URIClub.PKId),
                data.MakeInParam("@ParentId",			SqlDbType.Int, 4, parentId),
                data.MakeInParam("@CurrentId",			SqlDbType.Int, 4, uri.URIId),
				data.MakeInParam("@CurrentNodeName",	SqlDbType.NChar, 25, parentNodeName),
				data.MakeInParam("@IsForward",			SqlDbType.Bit, 1, iIsForward),
				data.MakeInParam("@StartRow",			SqlDbType.Int, 4, uri.URIDataManager.StartRow),
				data.MakeInParam("@PageSize",			SqlDbType.Int, 4, uri.URIDataManager.PageSize)
            };
            return oPrams;
        }
        
        public static void UpdateNewURIArgs(ContentURI uri, 
            ContentURI newURI, bool needsAncestors)
        {
            //if needed, set the models
            if (newURI.URINetwork == null)
            {
                newURI.URINetwork = new Network(uri.URINetwork);
            }
            else if (newURI.URINetwork.PKId == 0)
            {
                newURI.URINetwork = new Network(uri.URINetwork);
            }
            if (newURI.URIMember == null)
            {
                newURI.URIMember = new AccountToMember(uri.URIMember);
            }
            else if (newURI.URIMember.PKId == 0)
            {
                string sDocPath = newURI.URIMember.MemberDocFullPath;
                newURI.URIMember = new AccountToMember(uri.URIMember);
                if (!string.IsNullOrEmpty(sDocPath)) newURI.URIMember.MemberDocFullPath = sDocPath;
            }
            if (newURI.URIClub == null)
            {
                newURI.URIClub = new Account(uri.URIClub);
            }
            else if (newURI.URIClub.PKId == 0)
            {
                string sDocPath = newURI.URIClub.ClubDocFullPath;
                newURI.URIClub = new Account(uri.URIClub);
                if (!string.IsNullOrEmpty(sDocPath)) newURI.URIClub.ClubDocFullPath = sDocPath;
            }
            
            //set various properties
            Helpers.GeneralHelpers.SetApps(newURI);
            newURI.URIDataManager.ControllerName = uri.URIDataManager.ControllerName;
            newURI.URIDataManager.ClientActionType = uri.URIDataManager.ClientActionType;
            newURI.URIDataManager.ServerActionType = uri.URIDataManager.ServerActionType;
            newURI.URIDataManager.ServerSubActionType = uri.URIDataManager.ServerSubActionType;
            newURI.URIDataManager.SubActionView = uri.URIDataManager.SubActionView;
            newURI.URIDataManager.Variable = uri.URIDataManager.Variable;
            newURI.URIDataManager.UpdatePanelType = uri.URIDataManager.UpdatePanelType;
            newURI.URIDataManager.FormInput = uri.URIDataManager.FormInput;
            //contenturipattern
            newURI.UpdateContentURIPattern();
            //loading and edits
            newURI.URIDataManager.UseSelectedLinkedView 
                = uri.URIDataManager.UseSelectedLinkedView;
            //display
            newURI.URIDataManager.EditViewEditType = uri.URIDataManager.EditViewEditType;
            newURI.URIDataManager.SelectViewEditType = uri.URIDataManager.SelectViewEditType;
            newURI.URIDataManager.PreviewViewEditType = uri.URIDataManager.PreviewViewEditType;
            newURI.URIDataManager.PageSize = uri.URIDataManager.PageSize;
            newURI.URIDataManager.StartRow = uri.URIDataManager.StartRow;
            newURI.URIDataManager.RowCount = uri.URIDataManager.RowCount;
            //copy the appsettings
            Helpers.AppSettings.CopyURIAppSettings(uri, newURI);
        }
        public async Task<XmlReader> GetURISecondDocAsync(ContentURI docToCalcURI, ContentURI calcDocURI)
        {
            //first doc uses sqlxml to generate an xmldoc from the uri.uripattern (relational data)
            //second doc uses sql xml data type to generate an xmldoc from the xmlfield stored in join db tables
            //third docs are analyses, calculations, and explanatory stories carried out from doc 1 or 2 and stored in filesystem
            // or from xmldoc field in base dbtables
            XmlReader reader = null;
            //get the db query name
            string sQryName = string.Empty;
            switch (docToCalcURI.URIDataManager.AppType)
            {
                case GeneralHelpers.APPLICATION_TYPES.economics1:
                    sQryName = AppHelpers.Economics1.GetEconomics1QueryName(docToCalcURI.URIDataManager.SubAppType,
                        docToCalcURI.URINodeName);
                    if (sQryName != string.Empty)
                    {
                        reader = await GetXmlReaderDocAsync(docToCalcURI,
                            sQryName, calcDocURI.URIId);
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.prices:
                    sQryName = AppHelpers.Prices.GetPricesQueryName(docToCalcURI.URIDataManager.SubAppType,
                        docToCalcURI.URINodeName);
                    if (sQryName != string.Empty)
                    {
                        reader = await GetXmlReaderDocAsync(docToCalcURI,
                            sQryName, calcDocURI.URIId);
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    //no join fields for linkedviews
                    break;
                case GeneralHelpers.APPLICATION_TYPES.devpacks:
                    sQryName = AppHelpers.DevPacks.GetDevPackJoinQueryName(docToCalcURI);
                    if (sQryName != string.Empty)
                    {
                        reader = await GetXmlReaderDocAsync(docToCalcURI,
                            sQryName, calcDocURI.URIId);
                    }
                    break;
                case GeneralHelpers.APPLICATION_TYPES.resources:
                    sQryName = "0GetResourceXml";
                    //refactor: get linkedview xml working
                    //sQryName = Resources.GetResourceQueryName(docToCalcURI.URINodeName);
                    reader = await GetXmlReaderDocAsync(docToCalcURI, sQryName);
                    break;
                //case GeneralHelpers.APPLICATION_TYPES.locals:
                //    //2.0.0 deprecated
                //    sQryName = AppHelpers.Locals.GetLocalsJoinQueryName(docToCalcURI);
                //    if (docToCalcURI.URINodeName == AppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString())
                //    {
                //        ContentURI linkedViewAddIn 
                //            = Helpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
                //        if (linkedViewAddIn != null)
                //        {
                //            reader = await GetXmlReaderDocAsync(linkedViewAddIn, sQryName);
                //        }
                //    }
                //    else
                //    {
                //        reader = await GetXmlReaderDocAsync(docToCalcURI, sQryName);
                //    }
                //    break;
                default:
                    break;
            }
            return reader;
        }
        public async Task<XmlReader> GetURISecondBaseDocAsync(ContentURI uri)
        {
            XmlReader reader = null;
            string sQryName = string.Empty;
            //at present, metadata is retrieved via sqlxml
            bool bIsMetaData = false;
            string sAttName = string.Empty;
            if (uri.URIDataManager.AppType 
                == GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                AppHelpers.DevPacks.GetDevPackBaseQueryName(uri,
                    bIsMetaData, out sQryName, out sAttName);
                //get the instructions contained in uri (using join id)
                reader = await GetXmlReaderDocAsync(uri, sQryName, uri.URIId);
            }
            else if (uri.URIDataManager.AppType
                == GeneralHelpers.APPLICATION_TYPES.linkedviews)
            {
                AppHelpers.LinkedViews.GetLinkedViewBaseQueryName(uri, 
                    bIsMetaData, out sQryName, out sAttName);
                //get the instructions contained in the base uri (use baseid)
                reader = await GetXmlReaderDocAsync(uri, sQryName, uri.URIDataManager.BaseId);
            }
            return reader;
        }
        public async Task<bool> SaveURISecondDocAsync(ContentURI uri, XmlReader reader)
        {
            bool bHasUpdated = false;
            if (reader != null & uri.URIPattern != string.Empty)
            {
                string sQryName = string.Empty;
                switch (uri.URIDataManager.AppType)
                {
                    case GeneralHelpers.APPLICATION_TYPES.devpacks:
                        AppHelpers.DevPacks.GetUpdateDevPackJoinQueryParams(uri,
                             out sQryName);
                        break;
                    case GeneralHelpers.APPLICATION_TYPES.economics1:
                        sQryName = AppHelpers.Economics1.GetUpdateEconomics1QueryName(uri);
                        break;
                    case GeneralHelpers.APPLICATION_TYPES.prices:
                        sQryName = AppHelpers.Prices.GetUpdatePricesQueryName(uri);
                        break;
                    case GeneralHelpers.APPLICATION_TYPES.linkedviews:
                        //no join fields for linkedviews
                        break;
                    case GeneralHelpers.APPLICATION_TYPES.resources:
                        sQryName = AppHelpers.Resources.GetUpdateResourceQueryName(uri);
                        break;
                    default:
                        break;
                }
                if (sQryName != string.Empty)
                {
                    //inserts file system docs into xml fields in db 
                    bHasUpdated = await UpdateDevTrekXmlInDbAsync(uri,
                        sQryName, reader);
                }
                else
                {
                    uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "CONTENTHELPER_FILENOUPLOAD");
                }
            }
            return bHasUpdated;
        }
        public async Task<bool> SaveURISecondBaseDocAsync(
            ContentURI uri, bool isMetaData, 
            string fileName, XmlReader reader)
        {
             bool bHasUpdated = false;
             if (reader != null & uri.URIPattern != string.Empty)
             {
                 string sQryName = string.Empty;
                 string sAttName = string.Empty;
                 bool bIsJoinId = true;
                 int iId = 0;
                 if (uri.URIDataManager.AppType ==
                     GeneralHelpers.APPLICATION_TYPES.devpacks)
                 {
                     if (uri.URIDataManager.BaseId != 0)
                     {
                         iId = uri.URIDataManager.BaseId;
                         bIsJoinId = false;
                     }
                     else
                     {
                         //0.8.5 can't update without a base id -qry won't handle join side updates
                         iId = 0;
                         bIsJoinId = true;
                     }
                     if (iId != 0)
                     {
                         AppHelpers.DevPacks.GetUpdateDevPackBaseQueryParams(uri,
                             isMetaData, bIsJoinId, out sQryName, out sAttName);
                     }
                 }
                 else if (uri.URIDataManager.AppType ==
                     GeneralHelpers.APPLICATION_TYPES.linkedviews)
                 {
                     bool bIsBaseLinkedView = AppHelpers.LinkedViews.IsBaseLinkedView(uri);
                     if (!bIsBaseLinkedView)
                     {
                         if (uri.URIDataManager.BaseId != 0)
                         {
                             //0.8.5 allows edits to linked views from stories linked to linkedviews
                             //if the linkedview is not being stored in a linkedview app path
                             //it's being edited from a linked view app and must have a base id
                             //or will overwrite the wrong linkedview
                             iId = uri.URIDataManager.BaseId;
                             bIsJoinId = false;
                         }
                         else
                         {
                             //can't be edited from join side without the base id
                             iId = 0;
                             bIsJoinId = true;
                         }
                     }
                     else
                     {
                         iId = uri.URIId;
                     }
                     if (iId != 0)
                     {
                         AppHelpers.LinkedViews.GetUpdateLinkedViewBaseQueryParams(uri,
                             isMetaData, bIsJoinId, out sQryName, out sAttName);
                     }
                 }
                 if (sQryName != string.Empty && iId != 0)
                 {
                     //inserts file system docs into xml fields in db 
                     bHasUpdated = await UpdateDevTrekBaseXmlInDbAsync(uri,
                         sAttName, iId, sQryName, fileName, reader);
                 }
                 else
                 {
                     uri.ErrorMessage = Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                         string.Empty, "CONTENTHELPER_NOSAVECALCS");
                 }
             }
             return bHasUpdated;
        }
        public async Task<bool> SetURIAddInNames(ContentURI uri)
        {
            bool bHasSet = false;
            string addInTypeName = string.Empty;
            string hostTypeName = string.Empty;
            string sQryName = "0GetLinkedViewsAddInNames";
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
            {
                sqlIO.MakeInParam("@LinkedViewId",      SqlDbType.Int, 4, uri.URIDataManager.BaseId),
                sqlIO.MakeInParam("@NodeName",          SqlDbType.NChar, 25, uri.URINodeName),
                sqlIO.MakeOutParam("@AddInTypeName",    SqlDbType.NVarChar, 255),
                sqlIO.MakeOutParam("@HostTypeName",	    SqlDbType.NVarChar, 255),
            };
            int iNotUsed = await sqlIO.RunProcIntAsync(sQryName, oPrams);
            if (oPrams[2].Value != System.DBNull.Value)
            {
                addInTypeName = oPrams[2].Value.ToString();
            }
            if (oPrams[3].Value != System.DBNull.Value)
            {
                hostTypeName = oPrams[3].Value.ToString();
            }
            if (!string.IsNullOrEmpty(addInTypeName))
            {
                uri.URIDataManager.AddInName = addInTypeName;
                uri.URIDataManager.HostName = hostTypeName;
                bHasSet = true;
            }
            sqlIO.Dispose();
            return bHasSet;
        }
        public async Task<XmlReader> GetXmlReaderDocAsync(ContentURI uri,
            string qry, int currentId)
        {
            XmlReader reader = null;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
            {
                sqlIO.MakeInParam("@PKId",              SqlDbType.Int, 4, currentId),
                sqlIO.MakeInParam("@CurrentNodeName",   SqlDbType.NChar, 25, uri.URINodeName)
            };
            SqlDataReader oDataReader = await sqlIO.RunProcAsync(qry, oPrams);
            if (oDataReader != null && oDataReader.Read()
                && (oDataReader[0] != System.DBNull.Value))
            {
                using (oDataReader)
                {
                    SqlXml sqlValue = oDataReader.GetSqlXml(0);
                    if (sqlValue.IsNull == false
                        && sqlValue.Value != string.Empty)
                    {
                        reader = sqlValue.CreateReader();
                    }
                }
            }
            sqlIO.Dispose();
            return reader;
        }
        public async Task<XmlReader> GetXmlReaderDocAsync(ContentURI uri,
            string qry)
        {
            XmlReader reader = null;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] oPrams =
            {
                sqlIO.MakeInParam("@PKId",              SqlDbType.Int, 4, uri.URIId),
                sqlIO.MakeInParam("@CurrentNodeName",   SqlDbType.NChar, 25, uri.URINodeName),
                sqlIO.MakeInParam("@NetworkId",         SqlDbType.Int, 4, uri.URINetwork.PKId)
            };
            SqlDataReader oDataReader = await sqlIO.RunProcAsync(qry, oPrams);
            if (oDataReader != null && (!oDataReader.IsClosed))
            {
                using (oDataReader)
                {
                    while (oDataReader.Read())
                    {
                        if (oDataReader.FieldCount >= 1)
                        {
                            if (oDataReader.IsDBNull(0) == false)
                            {
                                SqlXml sqlValue = oDataReader.GetSqlXml(0);
                                if (sqlValue.IsNull == false)
                                {
                                    reader = sqlValue.CreateReader();
                                }
                            }
                        }
                    }
                }
            }
            sqlIO.Dispose();
            return reader;
        }
        public async Task<bool> UpdateDevTrekXmlInDbAsync( 
            ContentURI uri, string qry, XmlReader reader)
        {
            bool bHasUpdated = false;
            //store the doc in the database and change the last modified date
            SqlXml sqlValue = new SqlXml(reader);
            if (sqlValue.IsNull == false)
            {
                Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                SqlParameter[] oPrams = GetUpdateXmlParams(uri, sqlIO, sqlValue);
                int iNotUsed = await sqlIO.RunProcIntAsync(qry, oPrams);
                bHasUpdated = true;
                sqlIO.Dispose();
            }
            if (reader != null)
            {
                reader.Dispose();
            }
            return bHasUpdated;
        }
        private static SqlParameter[] GetUpdateXmlParams(ContentURI uri,
            SqlIOAsync sqlIO, SqlXml sqlValue)
        {
            SqlParameter[] oPrams1 =
            {
                sqlIO.MakeInParam("@PKId",					SqlDbType.Int, 4, uri.URIId),
                sqlIO.MakeInParam("@CurrentNodeName",		SqlDbType.NChar, 25, uri.URINodeName),
                sqlIO.MakeInParam("@LastChangedDate",	    SqlDbType.SmallDateTime, 8, GeneralHelpers.GetDateShortNow()),
                sqlIO.MakeInParam("@XmlDoc",			    SqlDbType.Xml, 0, sqlValue)
            };
            return oPrams1;
        }
        public async Task<bool> UpdateDevTrekBaseXmlInDbAsync(
            ContentURI uri, string attName, int id,
            string qry, string fileName, XmlReader reader)
        {
            bool bHasUpdated = false;
            //store the doc in a base table and change the last modified date
            SqlXml sqlValue = new SqlXml(reader);
            if (sqlValue.IsNull == false)
            {
                Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                SqlParameter[] oPrams = GetUpdateXmlParams(uri,
                    id, attName, fileName, sqlIO,
                    sqlValue);
                int iNotUsed = await sqlIO.RunProcIntAsync(qry, oPrams);
                bHasUpdated = true;
                sqlIO.Dispose();
            }
            if (reader != null)
            {
                reader.Dispose();
            }
            return bHasUpdated;
        }
        private static SqlParameter[] GetUpdateXmlParams(
            ContentURI uri, int id, string attName, string fileName,
            SqlIOAsync sqlIO, SqlXml sqlValue)
        {
            SqlParameter[] oPrams1 =
            {
                sqlIO.MakeInParam("@PKId",					SqlDbType.Int, 4, id),
                sqlIO.MakeInParam("@Name",					SqlDbType.NVarChar, 75, fileName),
                sqlIO.MakeInParam("@CurrentNodeName",		SqlDbType.NChar, 25, uri.URINodeName),
                sqlIO.MakeInParam("@LastChangedDate",	    SqlDbType.SmallDateTime, 8, GeneralHelpers.GetDateShortNow()),
                sqlIO.MakeInParam("@AttName",		        SqlDbType.NVarChar, 25, attName),
                sqlIO.MakeInParam("@XmlDoc",			    SqlDbType.Xml, 0, sqlValue)
            };
            return oPrams1;
        }
        public async Task<SqlDataReader> GetLinkedViewPageAsync(SqlIOAsync sqlIO, ContentURI uri)
        {
            SqlDataReader dataReader = null;
            //paginated list of descendent linkedviews
            string sQry = "0GetLinkedViewsPage";
            if (uri.URIDataManager.AppType
                == GeneralHelpers.APPLICATION_TYPES.resources)
            {
                //refactor: replace with real linked views
                if (uri.URINodeName
                    == AppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                {
                    //linked views are associated with resource nodes
                }
                else
                {
                    if (uri.URIDataManager.SubActionView
                        != Helpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                    {
                        //this is a convenient collection for display purposes
                        sQry = "0GetSearchResourcesById";
                    }
                }
            }
            if (uri.URIDataManager.AppType
                == GeneralHelpers.APPLICATION_TYPES.linkedviews)
            {
                sQry = "0GetLinkedViewsForLinkedViews";
            }
            else if (uri.URIDataManager.AppType
                == GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                if (uri.URIDataManager.SubActionView
                    != Helpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                {
                    if (uri.URIDataManager.IsSelectedLinkedView == false)
                    {
                        //doesn't include linked linkedviews because 
                        //they must come from isselectedlinkedview
                        sQry = "0GetLinkedViewsChildrenForDevPacks";
                    }
                    else
                    {
                        //includes linked linkedviews and descendant devpacks and/or devpackparts
                        //descendants included for analyses and for running descendant calculations
                        sQry = "0GetLinkedViewsForDevPackParts";
                    }
                }
            }
            SqlParameter[] oPrams = GetLinkedViewParams(uri, sqlIO);
            dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
            if (dataReader != null)
            {
                uri.URIDataManager.RowCount = dataReader.RecordsAffected;
            }
            return dataReader;
        }
        public async Task<SqlDataReader> GetLinkedViewForAnalysesPageAsync(SqlIOAsync sqlIO, ContentURI uri)
        {
            SqlDataReader dataReader = null;
            //paginated list of descendent linkedviews
            string sQry = string.Empty;
            if (uri.URIDataManager.AppType
                == GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                //include recursive devpackpart descendents
                sQry = "0GetLinkedViewsForDevPackParts";
            }
            SqlParameter[] oPrams = GetLinkedViewParams(uri, sqlIO);
            dataReader = await sqlIO.RunProcAsync(sQry, oPrams);
            if (dataReader != null)
            {
                uri.URIDataManager.RowCount = dataReader.RecordsAffected;
            }
            return dataReader;
        }
        private static SqlParameter[] GetLinkedViewParams(ContentURI uri,
            SqlIOAsync sqlIO)
        {
            int iStartRow = GetLinkedViewStartRow(uri);
            int iIsForward = GeneralHelpers.ConvertStringToInt(uri.URIDataManager.IsForward);
            if (iIsForward != 1 || iIsForward != 0) iIsForward = 1;
            int iPageSize = uri.URIDataManager.PageSize;
            //can edit up to 25 linkedviews
            if (uri.URIDataManager.SubActionView == GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                iPageSize = 25;
            //2.0.0 -calculators and analyzers on the linkedviews panel include all lvs (not 10 and not 25)
            if (uri.URIDataManager.ServerActionType == GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
                iPageSize = 1000;
            if (uri.URIDataManager.UseDefaultAddIn)
            {
                //returns addins for uri.urimember.clubinuse.addins[].isdefault = true
                //subapptype corresponds to linkedview.fileextensiontype
                //and can be used to filter addins
                SqlParameter[] oPrams1 =
                {
                    sqlIO.MakeInParam("@NodeId",		    SqlDbType.Int, 4, uri.URIMember.ClubInUse.PKId),
                    sqlIO.MakeInParam("@NodeName",          SqlDbType.NChar, 25, AppHelpers.AddIns.ADDIN_TYPES.addinaccountgroup.ToString()),
                    sqlIO.MakeInParam("@IsForward",		    SqlDbType.Bit, 1, iIsForward),
                    sqlIO.MakeInParam("@StartRow",			SqlDbType.Int, 4, iStartRow),
                    sqlIO.MakeInParam("@PageSize",			SqlDbType.Int, 4, iPageSize),
                    sqlIO.MakeInParam("@SubAppType",        SqlDbType.NChar, 25, uri.URIDataManager.SubAppType.ToString())
                };
                return oPrams1;
            }
            else if (uri.URIDataManager.UseDefaultLocal)
            {
                //needs addins for uri.urimember.clubinuse.locals[].isdefault = true
                int iAccountId = uri.URIMember.ClubInUse.PKId;
                SqlParameter[] oPrams1 =
                {
                    sqlIO.MakeInParam("@NodeId",		    SqlDbType.Int, 4, iAccountId),
                    sqlIO.MakeInParam("@NodeName",          SqlDbType.NChar, 25, AppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString()),
                    sqlIO.MakeInParam("@IsForward",		    SqlDbType.Bit, 1, iIsForward),
                    sqlIO.MakeInParam("@StartRow",			SqlDbType.Int, 4, iStartRow),
                    sqlIO.MakeInParam("@PageSize",			SqlDbType.Int, 4, iPageSize),
                    sqlIO.MakeInParam("@SubAppType",        SqlDbType.NChar, 25, uri.URIDataManager.SubAppType.ToString())
                };
                return oPrams1;
            }
            else
            {
                SqlParameter[] oPrams1 =
                {
                    sqlIO.MakeInParam("@NodeId",		    SqlDbType.Int, 4, uri.URIId),
                    sqlIO.MakeInParam("@NodeName",          SqlDbType.NChar, 25, uri.URINodeName),
                    sqlIO.MakeInParam("@IsForward",		    SqlDbType.Bit, 1, iIsForward),
                    sqlIO.MakeInParam("@StartRow",			SqlDbType.Int, 4, iStartRow),
                    sqlIO.MakeInParam("@PageSize",			SqlDbType.Int, 4, iPageSize),
                    sqlIO.MakeInParam("@SubAppType",        SqlDbType.NChar, 25, uri.URIDataManager.SubAppType.ToString())
                };
                return oPrams1;
            }
        }
        private static int GetLinkedViewStartRow(ContentURI uri)
        {
            int iStartRow = uri.URIDataManager.StartRow;
            if (uri.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.devpacks
                && uri.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.linkedviews
                && uri.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.resources)
            {
                //assumes that most apps won't need to paginate through linkedviews
                iStartRow = 0;
            }
            return iStartRow;
        }
        private static SqlParameter[] GetLinkedViewForDefaultAddInParams(
            ContentURI uri, SqlIOAsync sqlIO)
        {
            int iIsForward = GeneralHelpers.ConvertStringToInt(uri.URIDataManager.IsForward);
            if (iIsForward != 1 || iIsForward != 0) iIsForward = 1;
            SqlParameter[] oPrams1 =
            {
                sqlIO.MakeInParam("@AccountId",		    SqlDbType.Int, 4, uri.URIMember.ClubInUse.PKId),
                sqlIO.MakeInParam("@SubAppType",        SqlDbType.NChar, 25, uri.URIDataManager.ServerSubActionType.ToString()),
                sqlIO.MakeInParam("@IsForward",		    SqlDbType.Bit, 1, iIsForward),
                sqlIO.MakeInParam("@StartRow",			SqlDbType.Int, 4, uri.URIDataManager.StartRow),
                sqlIO.MakeInParam("@PageSize",			SqlDbType.Int, 4, uri.URIDataManager.PageSize)

            };
            return oPrams1;
        }
        public static List<ContentURI> FillChildrenListandSetStartRow(
            ContentURI parentURI, ContentURI uri, SqlDataReader childrenReaderResults)
        {
            List<ContentURI> colChildren = new List<ContentURI>();
            if (childrenReaderResults != null && (!childrenReaderResults.IsClosed))
            {
                int iStartRow = 0;
                string sFileExtensionType = string.Empty;
                int i = 0;
                bool bNeedsAncestors = false;
                string sName = string.Empty;
                //guests don't need to see default names ("00 Default")
                bool bGuestNeedsRow = true;
                using (childrenReaderResults)
                {
                    while (childrenReaderResults.Read())
                    {
                        sName = childrenReaderResults.GetString(2);
                        if (i == 0)
                        {
                            //the first parameter is a start row parameter (not an id)
                            iStartRow = childrenReaderResults.GetInt32(0);
                        }
                        bGuestNeedsRow = true;
                        if (bGuestNeedsRow)
                        {
                            ContentURI childURI = ContentURI.GetContentURI
                            (
                                uri
                                //id
                                ,childrenReaderResults.GetInt32(1)
                                //label
                                , childrenReaderResults.GetString(3)
                                //name
                                , sName
                                //description 
                                , childrenReaderResults.GetString(4)
                                //file extension type
                                , string.Empty
                                //connection
                                , uri.URINetwork.WebConnection
                                //media resource uri to display
                                , childrenReaderResults.GetString(5)
                                //media resource alt to display
                                , childrenReaderResults.GetString(6)
                                //parentURIPattern
                                , parentURI.URIPattern
                                //docStatus
                                , uri.URIDataManager.DocStatus
                               , uri.URINetworkPartName
                               , (childrenReaderResults.FieldCount == 8) ?
                                    childrenReaderResults.GetString(7) : uri.URIDataManager.ChildrenNodeName
                                , uri.URIDataManager.DefaultRootWebStoragePath
                               );
                            UpdateNewURIArgs(uri, childURI, bNeedsAncestors);
                            colChildren.Add(childURI);
                            i++;
                        }
                        else
                        {
                            uri.URIDataManager.RowCount -= 1;
                        }
                    }
                    //row id is one-based while startrow index is zero-based
                    iStartRow -= 1;
                    uri.URIDataManager.StartRow = iStartRow;
                }
            }
            return colChildren;
        }
        
        public static void SetLinkedViewPropsFromParent(ContentURI selectedLinkedView,
            ContentURI parentURI)
        {
            GeneralHelpers.SetApps(selectedLinkedView);
            selectedLinkedView.URIDataManager.UseDefaultAddIn = parentURI.URIDataManager.UseDefaultAddIn;
            selectedLinkedView.URIDataManager.UseDefaultLocal = parentURI.URIDataManager.UseDefaultLocal;
            //v1.1.1 multiple html views allowed
            selectedLinkedView.URIDataManager.SubActionView = parentURI.URIDataManager.SubActionView;
            if (selectedLinkedView.URIDataManager.ParentURIPattern == string.Empty)
                selectedLinkedView.URIDataManager.ParentURIPattern = parentURI.URIPattern;
        }
    }
}
