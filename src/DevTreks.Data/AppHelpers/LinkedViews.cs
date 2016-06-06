using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for economics analyses, calculations, custom linkedviews, 
    ///             and other xml apps.
    ///Author:		www.devtreks.org
    ///Date:		2016, May
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE:        1. This class has two primary roles:
    ///                 a. Linking apps to calculators, analyzers 
    ///                     and story-tellers (i.e. ebook packagers).
    ///                 b. Holding stand-alone xml docs that serve as explanations 
    ///                     accompanying calculations, analyses and stories. 
    /// </summary>
    public class LinkedViews
    {
        public LinkedViews()
        {
        }
        
        //attribute for linking (linkedviews) an economics node to a linked view node
        //(i.e. for calcs, analyses, images, explanations ..)
        public const string LINKEDVIEWBASEID = "LinkedViewId";
        //default linkedview attribute when opening a calculation, analysis, or other linked view
        public const string DEFAULTLINKEDVIEWID = "DefaultLinkedViewId";

        public const string ISDEFAULTLINKEDVIEWID = "IsDefaultLinkedView";
        public const string LINKEDVIEWNAME = "LinkedViewName";
        //node being linked property name (uniform for all apps)
        public const string LINKINGNODEID = "LinkingNodeId";
        //linked view property being updated
        public const string LINKINGXMLDOC = "LinkingXmlDoc";

        //each linkedview can be a what-if scenario (i.e. high vs. low yield)
        public const string WHATIF_TAG_NAME = "WhatIfTagName";
        public const string ATWHATIF_TAG_NAME = "@WhatIfTagName";
        
        //calcparam form element names
        public const string TEMPDOCURI = "tempdocuri";
        public const string DOCTOCALCURI = "doctocalcuri";
        public const string CALCDOCURI = "calcdocuri";
        public const string LINKEDVIEWSURI = "linkedviewsuri";
        public const string USEDEFAULT = "usedefault";
        public const string SELECTSLINKEDVIEWSID = "selectslinkedviewsid";
        public const string LINKEDVIEWSID = "linkedViewId";
        //standard custom node attributes
        public const string TITLE = "Title";
        public const string HREF = "Href";
        public const string cDateLASTMODIFIED = "DateLastModified";
        
        public enum LINKEDVIEWS_TYPES
        {
            servicebase             = 0,
            linkedviewtype          = 1,
            linkedviewgroup         = 2,
            linkedviewpack          = 3,
            linkedview              = 4,
            linkedviewresourcepack  = 5
        }
        public static Dictionary<string, string> GetLinkedViewType(ContentURI uri)
        {
            Dictionary<string, string> colTypes = new Dictionary<string, string>();
            if (uri.URIModels.LinkedViewType != null)
            {
                foreach (var type in uri.URIModels.LinkedViewType)
                {
                    //note that on the client the key becomes the option's value
                    colTypes.Add(type.PKId.ToString(), type.Name);
                }
            }
            return colTypes;
        }
        public static async Task<Dictionary<int, string>> GetDefaultAddInOrLocalId(ContentURI uri, 
            DataAccess.DevTreksContext context)
        {
            Dictionary<int, string> addins = new Dictionary<int, string>();
            int addInId = 0;
            string addInName = string.Empty;
            if (uri.URIMember != null)
            {
                if (uri.URIDataManager.UseDefaultLocal)
                {
                    var defaultAddIn = await context.AccountToLocal
                        .Where(ca => ca.LinkingNodeId == uri.URIMember.AccountId
                        && ca.IsDefaultLinkedView == true)
                        .FirstOrDefaultAsync();
                    if (defaultAddIn != null)
                    {
                        addInId = defaultAddIn.LinkedViewId;
                        addInName = defaultAddIn.LinkedViewName;
                    }
                }
                else
                {
                    var defaultAddIn = await context.AccountToAddIn
                        .Where(ca => ca.LinkingNodeId == uri.URIMember.AccountId
                        && ca.IsDefaultLinkedView == true)
                        .FirstOrDefaultAsync();
                    if (defaultAddIn != null)
                    {
                        addInId = defaultAddIn.LinkedViewId;
                        addInName = defaultAddIn.LinkedViewName;
                    }
                }
            }
            addins.Add(addInId, addInName);
            return addins;
        }
        public static void AddBaseResourcePackToXml(XElement linkedviewresourcepack,
            LinkedViewToResourcePack linkedViewToResourcePack)
        {
            if (linkedViewToResourcePack.ResourcePack != null)
            {
                linkedviewresourcepack.SetAttributeValue("ResourcePackName", linkedViewToResourcePack.ResourcePack.ResourcePackName);
                linkedviewresourcepack.SetAttributeValue("ResourcePackDesc", linkedViewToResourcePack.ResourcePack.ResourcePackDesc);
            }
        }
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
            {
                uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                //checkboxes for node insertions
                uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
            }
            else
            {
                uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.print;
                uri.URIDataManager.SelectViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.part;
            }
            //link backwards
            uri.URIDataManager.ParentPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
            //link forwards
            if (currentNodeName == LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.none;
            }
            else
            {
                uri.URIDataManager.ChildrenPanelType = Helpers.GeneralHelpers.UPDATE_PANEL_TYPES.select;
            }
            //tell ui about children node name (for adding to/selecting from tocs)
            if (currentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                || currentNodeName == Agreement.AGREEMENT_TYPES.service.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = LINKEDVIEWS_TYPES.linkedviewgroup.ToString();
            }
            else if (currentNodeName == LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = LINKEDVIEWS_TYPES.linkedviewpack.ToString();
            }
            else if (currentNodeName == LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = LINKEDVIEWS_TYPES.linkedview.ToString();
            }
            else if (currentNodeName == LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString();
            }
            else if (currentNodeName == LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
            else
            {
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
        }
       
        public async Task<bool> UpdateDefaultLinkedViewAsync(ContentURI uri,
            int newIsDefaultId)
        {
            bool bHasUpdated = false;
            string sNodeName = string.Empty;
            int iLinkingNodeId = 0;
            int iLinkedViewId = 0;
            GetEditLinkedViewParams(uri,
                newIsDefaultId, out sNodeName, out iLinkingNodeId, out iLinkedViewId);
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
            { 
                sqlIO.MakeInParam("@NodeName",		                    SqlDbType.NVarChar, 25, sNodeName),
                sqlIO.MakeInParam("@LinkingNodeId",                     SqlDbType.Int, 4, iLinkingNodeId),
                sqlIO.MakeInParam("@NewIsDefaultLinkedViewJoinId",      SqlDbType.Int, 4, newIsDefaultId)
            };
            int iResult = await sqlIO.RunProcIntAsync(
                "0UpdateLinkedViewIsDefault", colPrams);
            sqlIO.Dispose();
            if (iResult == 1) bHasUpdated = true;
            if (bHasUpdated)
            {
                //update the linkedviews collection for display
                string sLinkedViewURIPattern = ContentURI.ChangeURIPatternPart(uri.URIPattern, ContentURI.URIPATTERNPART.id, 
                    newIsDefaultId.ToString());
                Helpers.LinqHelpers.SetLinkedViewIsDefaultView(uri.URIDataManager.LinkedView, 
                    sLinkedViewURIPattern);
            }
            return bHasUpdated;
        }
        public static void GetEditLinkedViewParams(ContentURI uri,
            int linkedViewJoinId, out string nodeName, 
            out int linkingNodeId, out int linkedViewId)
        {
            nodeName = uri.URINodeName;
            linkingNodeId = uri.URIId;
            linkedViewId = 0;
            ContentURI linkedView = Helpers.LinqHelpers.GetLinkedView(uri.URIDataManager.LinkedView, 
                linkedViewJoinId);
            if (linkedView != null)
            {
                linkedViewId = linkedView.URIDataManager.BaseId;
            }
            else
            {
                //uri.ErrorMessage =
            }
            if (linkedViewJoinId == 0
                || linkedViewId == 0)
            {
                //uri.ErrorMessage =
            }
        }
        public static void GetChildForeignKeyName(string parentNodeName,
            out string childForeignKeyName)
        {
            childForeignKeyName = string.Empty;
            if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                childForeignKeyName = Agreement.SERVICE_ID;
            }
            else if (parentNodeName == LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                childForeignKeyName = "LinkedViewGroupId";
            }
            else if (parentNodeName == LINKEDVIEWS_TYPES.linkedviewpack.ToString())
            {
                childForeignKeyName = "LinkedViewPackId";
            }
            else if (parentNodeName == LINKEDVIEWS_TYPES.linkedview.ToString())
            {
                childForeignKeyName = "LinkedViewId";
            }
            else if (parentNodeName == LINKEDVIEWS_TYPES.linkedviewresourcepack.ToString())
            {
                //should be using media app
            }
        }
        public static void GetUpdateLinkedViewBaseQueryParams(ContentURI uri,
            bool isMetaData, bool isJoinId,
            out string qryName, out string attName)
        {
            qryName = "0UpdateLinkedViewBaseXml";
            attName = string.Empty;
            if (isMetaData == true)
            {
                attName = "LinkedViewPackMetaDataXml";
            }
        }
        public static void GetLinkedViewBaseQueryName(ContentURI uri, bool isMetaData,
            out string qryName, out string attName)
        {
            qryName = string.Empty;
            attName = string.Empty;
            if (isMetaData == true)
            {
                attName = "LinkedViewPackMetaDataXml";
            }
            if (uri.URINodeName != LINKEDVIEWS_TYPES.linkedviewgroup.ToString())
            {
                qryName = "0GetLinkedViewBaseXml";
            }
        }
        public static bool IsBaseLinkedView(ContentURI uri)
        {
            bool bIsBaseLV = false;
            //if the clubdocpath is not being stored in a regular /linkedviews/restofpath path
            //it can't be a base linkedview
            string sDelimiter
                = Helpers.FileStorageIO.GetDelimiterForFileStorage(uri.URIClub.ClubDocFullPath);
            string sLVFileNameDirectory =
                        string.Concat(sDelimiter,
                        Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.linkedviews.ToString(),
                        sDelimiter);
            if (uri.URIClub.ClubDocFullPath.Contains(sLVFileNameDirectory))
            {
                bIsBaseLV = true;
            }
            return bIsBaseLV;
        }
        /// <summary>
        /// Get the calculations node out of the xmldoc attribute 
        /// field of the current element
        /// </summary>
        public static void GetLinkedViewFromXmlDocAttribute(bool useWhatIfOnly,
            string whatIfTagName, XElement parentElement,
            out XElement linkedViewElement)
        {
            linkedViewElement = null;
            if (parentElement.HasElements)
            {
                string sCalcNodeQry = string.Empty;
                if (whatIfTagName != string.Empty
                    && whatIfTagName != Helpers.GeneralHelpers.NONE)
                {
                    //the linkedview pattern uses whatIfTagName to determine which descendent xmldoc linkedview node
                    //to load - if more than one node, it chooses the first (i.e. don't use the same calculator 
                    //twice on the same node)
                    sCalcNodeQry = Helpers.AddInHelper.GetAddInNodeQryFromParentNode(
                       parentElement.CreateNavigator(), ATWHATIF_TAG_NAME,
                       whatIfTagName);
                    linkedViewElement = parentElement.XPathSelectElement(sCalcNodeQry);
                }
                if (linkedViewElement == null && useWhatIfOnly == false)
                {
                    //use defaultlinkedviewid when no whatiftagnames used
                    string sDefaultLinkedViewId
                        = EditHelpers.XmlLinq.GetElementAttributeValue(parentElement, DEFAULTLINKEDVIEWID);
                    linkedViewElement = EditHelpers.XmlLinq.GetElement(parentElement,
                        LINKEDVIEWS_TYPES.linkedview.ToString(), sDefaultLinkedViewId);
                }
                if (linkedViewElement == null && useWhatIfOnly == false)
                {
                    //one more try - legacy calcs may still be on hand
                    //return the first linkedview
                    XPathNavigator navCalcs = parentElement.CreateNavigator();
                    bool bHasMoved = navCalcs.MoveToFirstChild();
                    if (navCalcs.LocalName == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        bHasMoved = navCalcs.MoveToFirstChild();
                        if (bHasMoved)
                        {
                            linkedViewElement
                                = EditHelpers.XmlLinq.ConvertNavigatorToElement(navCalcs);
                        }
                    }
                    else
                    {
                        if (bHasMoved) navCalcs.MoveToParent();
                    }
                }
            }
        }
        public static void GetNewLinkedViewDocPaths(ContentURI docToCalc, string fileName,
            out string newViewFullMemberDocPath, out string newViewFullClubDocPath)
        {
            //all views are saved in the same paths
            newViewFullMemberDocPath = string.Empty;
            newViewFullClubDocPath = string.Empty;
            string sOldFileName = string.Empty;
            fileName = Helpers.ContentHelper.FixFilePathLength(docToCalc, fileName);
            if (!string.IsNullOrEmpty(docToCalc.URIMember.MemberDocFullPath))
            {
                sOldFileName = Path.GetFileName(docToCalc.URIMember.MemberDocFullPath);
                newViewFullMemberDocPath = docToCalc.URIMember.MemberDocFullPath.Replace(
                    sOldFileName, fileName);
            }
            if (!string.IsNullOrEmpty(docToCalc.URIClub.ClubDocFullPath))
            {
                sOldFileName = Path.GetFileName(docToCalc.URIClub.ClubDocFullPath);
                newViewFullClubDocPath = docToCalc.URIClub.ClubDocFullPath.Replace(
                    sOldFileName, fileName);
            }
        }
        public static string GetNewLinkedViewURIPatternForFileName(string currentURIPattern,
            string calcDocURIPattern)
        {
            string sNewViewURIPattern = string.Empty;
            string sNewViewId = ContentURI.GetURIPatternPart(calcDocURIPattern,
                ContentURI.URIPATTERNPART.id);
            string sOldId = ContentURI.GetURIPatternPart(currentURIPattern,
                ContentURI.URIPATTERNPART.id);
            sNewViewURIPattern = currentURIPattern.Replace(sOldId, sNewViewId);
            return sNewViewURIPattern;
        }
        public static void SaveTempDocTotalsToLinkedViewPaths(ContentURI docToCalcURI,
            ContentURI calcDocURI)
        {
            //full docs identified by fileextensiontype = full
            string sTotalsFilePath = Helpers.GeneralHelpers.AddDocExtensionToPath(
                docToCalcURI.URIClub.ClubDocFullPath,
                Helpers.GeneralHelpers.FILENAME_EXTENSIONS.full);
            //save newly calculated files
            Helpers.FileStorageIO.CopyURIs(docToCalcURI, docToCalcURI.URIDataManager.TempDocPath,
                sTotalsFilePath);
            //delete any old doctocalc html views of the addin 
            //but keep the calcdoc views
            bool bIncludeCalcDocs = false;
            Helpers.AddInHelper.DeleteOldAddInHtmlFiles(docToCalcURI,
                sTotalsFilePath, calcDocURI.URIPattern, bIncludeCalcDocs);
        }
        
        public void SaveSummaryDoc(ContentURI docToCalcURI,
            ContentURI calcDocURI, XmlElement parentNode,
            XmlDocument baseDoc, XPathNavigator navigator,
            string directoryPath, IDictionary<string, string> childrenLinkedView)
        {
            bool bIsDeepClone = false;
            //create the partial document that will be saved on this recursion
            XmlDocument oSummaryDoc = new XmlDocument();
            //clone the base doc from the root; append this to it's last child node and import these nodes into summarydoc
            XmlNode oClonedBaseNode = baseDoc.DocumentElement.CloneNode(true);
            //import this into the new summary doc
            XmlNode oNodeToInsert = oSummaryDoc.ImportNode(oClonedBaseNode, true);
            //append it to the summary document
            oSummaryDoc.AppendChild(oNodeToInsert);
            if (docToCalcURI.URIDataManager.AppType
                == DevTreks.Data.Helpers.GeneralHelpers.APPLICATION_TYPES.economics1
                && docToCalcURI.URINodeName.EndsWith("group"))
            {
                //append the time period shallow cloned nodes as well
                int iDepth = 2;
                EditHelpers.XmlIO.AppendChildren(navigator, bIsDeepClone,
                    ref parentNode, iDepth);
            }
            else
            {
                //append the parentelement's children to the parent element (the children nodes hold the summary totals to display)
                EditHelpers.XmlIO.AppendChildrenOrAnnuitiesOnly(navigator,
                    bIsDeepClone, docToCalcURI.URIDataManager.TempDocPath,
                    baseDoc, ref parentNode);
            }
            //import and append the parent element to the last child in the inserted node
            bIsDeepClone = true;
            EditHelpers.XmlIO.AppendNodeToLastDocNode(parentNode, bIsDeepClone,
                oSummaryDoc.FirstChild, ref oSummaryDoc);
            string sCacheKey = string.Empty;
            if (sCacheKey != string.Empty)
            {
                //caching needs consideration again (i.e. a couple of hours when actively editing)
            }
            else
            {
                Helpers.GeneralHelpers.FILENAME_EXTENSIONS eFileExtType 
                    = Helpers.GeneralHelpers.FILENAME_EXTENSIONS.none;
                string sSummaryFilePath = GetSaveTotalsPath(
                    docToCalcURI, calcDocURI,
                    directoryPath, parentNode, eFileExtType);
                string sChildLinkedViewPattern = calcDocURI.URIPattern;
                string sSummaryOldFilePath = sSummaryFilePath;
                //if new linkedviews were inserted change file names to the new ids
                GetLinkedViewPath(childrenLinkedView, ref sSummaryFilePath, ref sChildLinkedViewPattern);
                if (sSummaryFilePath.Equals(sSummaryOldFilePath))
                {
                    //v1.3.2 addition
                    //childrenLinkedView did not contain childLinkedView needed for the correct
                    //addin path (happens when UseInDesc = true but UpdateDesc = false)
                    //try to obtain the child lv id using the calcdocuri.fileextensiontype
                    XmlElement xmlNodeToInsert = (XmlElement)oNodeToInsert;
                    GetLinkedViewPath(xmlNodeToInsert, calcDocURI, ref sSummaryFilePath, ref sChildLinkedViewPattern);
                }
                //save summary data to disk file
                string sErrorMsg = string.Empty;
                Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
                fileStorageIO.SaveXmlInURI(docToCalcURI,
                    oSummaryDoc, sSummaryFilePath,
                    out sErrorMsg);
                docToCalcURI.ErrorMessage += sErrorMsg;
                //delete any old html views of the doctocalc
                //but keep the calcdocs -they won't change
                bool bIncludeCalcDocs = false;
                Helpers.AddInHelper.DeleteOldAddInHtmlFiles(docToCalcURI,
                    sSummaryFilePath, sChildLinkedViewPattern, bIncludeCalcDocs);
            }
        }
        /// <summary>
        /// clone a group node to a base doc
        /// </summary>
        /// <param name="totalsDoc"></param>
        /// <param name="baseDoc"></param>
        public static void CloneGroupNode(XmlDocument totalsDoc, ref XmlDocument baseDoc)
        {
            //get the node to insert
            if (totalsDoc.DocumentElement.HasChildNodes == true)
            {
                XmlNode oNodeToInsert = baseDoc.ImportNode(totalsDoc.DocumentElement.FirstChild, false);
                //append it to the base document
                baseDoc.DocumentElement.AppendChild(oNodeToInsert);
            }
        }
        /// <summary>
        /// Save the remaining full docs: budget/investment docs, timeperiod docs
        /// </summary>
        public async Task SaveFullDoc(ContentURI docToCalcURI,
            ContentURI calcDocURI, XmlElement parentNode, XmlDocument baseDoc,
            XPathNavigator navigator, string directoryPath, 
            IDictionary<string, string> childrenLinkedView)
        {
            //each child node has the full doc being saved
            if (navigator.HasChildren == true)
            {
                bool bIsDeepClone = true;
                //162 ran into bugs with parallel tasks and eliminated
                //async and parallel processing
                //List<Task> tasks = new List<Task>();
                //List<Stream> sourceStreams = new List<Stream>();
                XPathNodeIterator oChildrenIterator = navigator.SelectChildren(XPathNodeType.Element);
                try
                {
                    while (oChildrenIterator.MoveNext())
                    {
                        //get the child node holding the full document (child nodes) being saved
                        XmlElement oChildElement = (XmlElement)((IHasXmlNode)oChildrenIterator.Current).GetNode().CloneNode(bIsDeepClone);
                        if (oChildElement != null)
                        {
                            //create the full document that will be saved on this recursion
                            XmlDocument oFullDoc = new XmlDocument();
                            XmlNode oClonedBaseNode = baseDoc.DocumentElement.CloneNode(true);
                            //import this into the new summary doc
                            XmlNode oNodeToInsert = oFullDoc.ImportNode(oClonedBaseNode, true);
                            //append it to the summary document
                            oFullDoc.AppendChild(oNodeToInsert);
                            //append the child element to the last child in the inserted node
                            EditHelpers.XmlIO.AppendNodeToLastDocNode(oChildElement,
                                bIsDeepClone, oFullDoc.FirstChild, ref oFullDoc);
                            if (oChildElement.LocalName
                                != Helpers.GeneralHelpers.ROOT_PATH)
                            {
                                if (docToCalcURI.URINodeName != oChildElement.Name)
                                {
                                    //save this full document; done with the full fileextensiontype
                                    Helpers.GeneralHelpers.FILENAME_EXTENSIONS eFileExtType
                                        = Helpers.GeneralHelpers.FILENAME_EXTENSIONS.full;
                                    string sTotalsFilePath = GetSaveTotalsPath(
                                        docToCalcURI, calcDocURI, directoryPath,
                                        oChildElement, eFileExtType);
                                    string sChildLinkedViewPattern = calcDocURI.URIPattern;
                                    string sTotalsOldFilePath = sTotalsFilePath;
                                    if (childrenLinkedView != null)
                                    {
                                        //if new linkedviews were inserted change file names to the new ids
                                        GetLinkedViewPath(childrenLinkedView, ref sTotalsFilePath, ref sChildLinkedViewPattern);
                                    }
                                    if (sTotalsFilePath.Equals(sTotalsOldFilePath))
                                    {
                                        //v1.2.0 addition
                                        //childrenLinkedView did not contain childLinkedView needed for the correct
                                        //addin path (happens when UseInDesc = true but UpdateDesc = false)
                                        //try to obtain the child lv id using the calcdocuri.fileextensiontype
                                        GetLinkedViewPath(oChildElement, calcDocURI, ref sTotalsFilePath, ref sChildLinkedViewPattern);
                                    }
                                    //ok to process file here for now for parallel processing
                                    Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
                                    //add stream.WriteAsynch() to streams
                                    await fileStorageIO.SaveFileAsync(docToCalcURI,
                                        oFullDoc, sTotalsFilePath);
                                    //Task taskSaveFile = fileStorageIO.SaveFilesAsync(oFullDoc, sTotalsFilePath,
                                    //    sourceStreams);
                                    //tasks.Add(taskSaveFile);
                                    //delete any old html views of the doctocalc
                                    //but keep the calcdocs -they won't change
                                    bool bIncludeCalcDocs = false;
                                    Helpers.AddInHelper.DeleteOldAddInHtmlFiles(docToCalcURI,
                                        sTotalsFilePath, sChildLinkedViewPattern, bIncludeCalcDocs);
                                }
                            }
                        }
                    }
                    //await Task.WhenAll(tasks);
                }
                catch
                {
                    docToCalcURI.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.GetMessage("FILESTORAGE_FILENOSAVEXMLSUM");
                }
                //finally
                //{
                //    //close the file streams
                //    foreach (Stream sourceStream in sourceStreams)
                //    {
                //        sourceStream.Close();
                //    }
                //}
            }
        }

        
        private static string GetSaveTotalsPath(
            ContentURI docToCalcURI, ContentURI calcDocURI,
            string directoryPath, XmlElement currentElement,
            Helpers.GeneralHelpers.FILENAME_EXTENSIONS fileExtType)
        {
            string sId = currentElement.GetAttribute(AppHelpers.Calculator.cId);
            string sCurrentDirectoryName = string.Concat(currentElement.LocalName,
                Helpers.GeneralHelpers.FILENAME_DELIMITER, sId);
            string sAncestorPath = string.Empty;
            Helpers.GeneralHelpers.GetStartSubstring(directoryPath,
               sCurrentDirectoryName, out sAncestorPath);
            string sTotalsFilePath = string.Empty;
            string sDelimiter
                = Helpers.FileStorageIO.GetDelimiterForFileStorage(directoryPath);
            if (sAncestorPath.Equals(directoryPath) == true)
            {
                string sExistingDirectory = string.Concat(sCurrentDirectoryName, sDelimiter);
                if (directoryPath.EndsWith(sExistingDirectory) == false)
                {
                    //it's a subdirectory of directoryPath
                    sAncestorPath = GetCurrentDirectoryPath(directoryPath, sCurrentDirectoryName);
                }
            }
            Helpers.FileStorageIO.DirectoryCreate(docToCalcURI, sAncestorPath);
            //calculated totals always use the calcDocURI pattern in name
            string sAncestorFilePath = string.Concat(sAncestorPath,
                Helpers.ContentHelper.MakeStandardFileNameFromURIPattern(calcDocURI));
            bool bNeedsFullDocs = NeedsFullDocs(currentElement.LocalName);
            if (bNeedsFullDocs)
            {
                if (fileExtType != Helpers.GeneralHelpers.FILENAME_EXTENSIONS.none)
                {
                    sTotalsFilePath = Helpers.GeneralHelpers.AddDocExtensionToPath(sAncestorFilePath,
                        fileExtType);
                }
                else
                {
                    sTotalsFilePath = Helpers.GeneralHelpers.AddDocExtensionToPath(sAncestorFilePath,
                        Helpers.GeneralHelpers.FILENAME_EXTENSIONS.none);
                }
            }
            else
            {
                sTotalsFilePath = Helpers.GeneralHelpers.AddDocExtensionToPath(sAncestorFilePath,
                    Helpers.GeneralHelpers.FILENAME_EXTENSIONS.none);
            }
            return sTotalsFilePath;
        }
        public static string GetCurrentDirectoryPath(string directoryPath,
            string currentDirectoryName)
        {
            string sCurrentDirectoryPath = string.Empty;
            string sDelimiter 
                = Helpers.FileStorageIO.GetDelimiterForFileStorage(directoryPath);
            //it's a subdirectory of directoryPath
            //append currentdirectory to parentdirectory
            string sParentPath = string.Empty;
            string sParentFileName = string.Empty;
            Helpers.GeneralHelpers.GetParentPathandLastSubstring(directoryPath,
                sDelimiter, out sParentPath, out sParentFileName);
            if (sParentPath.EndsWith(sDelimiter))
            {
                sCurrentDirectoryPath = string.Concat(sParentPath, currentDirectoryName, sDelimiter);
            }
            else
            {
                sCurrentDirectoryPath = Helpers.GeneralHelpers.MakeString(sParentPath, sDelimiter,
                    currentDirectoryName, sDelimiter, string.Empty, string.Empty);
            }
            return sCurrentDirectoryPath;
        }
        private static bool NeedsFullDocs(string currentNodeName)
        {
            bool bNeedsFullDocs = false;
            if (currentNodeName == Economics1.BUDGET_TYPES.budgetgroup.ToString()
               || currentNodeName == Economics1.INVESTMENT_TYPES.investmentgroup.ToString()
               || currentNodeName == Economics1.BUDGET_TYPES.budget.ToString()
               || currentNodeName == Economics1.INVESTMENT_TYPES.investment.ToString()
               || currentNodeName == Prices.INPUT_PRICE_TYPES.inputgroup.ToString()
               || currentNodeName == Prices.OUTPUT_PRICE_TYPES.outputgroup.ToString()
               || currentNodeName == Prices.OUTCOME_PRICE_TYPES.outcomegroup.ToString()
               || currentNodeName == Prices.OPERATION_PRICE_TYPES.operationgroup.ToString()
               || currentNodeName == Prices.COMPONENT_PRICE_TYPES.componentgroup.ToString())
            {
                bNeedsFullDocs = true;
            }
            return bNeedsFullDocs;
        }
        public static bool IsLinkedViewXmlDoc(string keyURIPattern,
            ContentURI docToCalcURI)
        {
            bool bIsLinkedViewXmlDoc = true;
            string sNodeName
                = ContentURI.GetURIPatternPart(keyURIPattern,
                ContentURI.URIPATTERNPART.node);
            //these xmldocs are stored with the appnode and have 
            //a db relational tie to the base input, output, operation,
            //or component xmldoc (and that relational tie is important)
            bIsLinkedViewXmlDoc = 
                IsLinkedViewXmlDoc(keyURIPattern, docToCalcURI.URIDataManager.SubAppType);
            return bIsLinkedViewXmlDoc;
        }
        public static bool IsLinkedViewXmlDoc(string currentNodeURIPattern,
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subAppType)
        {
            bool bIsLinkedViewXmlDoc = true;
            string sNodeName
                = ContentURI.GetURIPatternPart(currentNodeURIPattern,
                ContentURI.URIPATTERNPART.node);
            //these xmldocs are stored with the appnode and have 
            //a db relational tie to the base input, output, operation,
            //or component xmldoc (and that relational tie is important)
            if (subAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.budgets
                && (sNodeName == Economics1.BUDGET_TYPES.budgetoperation.ToString()
                || sNodeName == Economics1.BUDGET_TYPES.budgetoutcome.ToString()
                || sNodeName == Economics1.BUDGET_TYPES.budgetoutput.ToString()
                || sNodeName == Economics1.BUDGET_TYPES.budgetinput.ToString()))
            {
                //these will still be updated but will use the standard 
                //sqlxml pattern
                bIsLinkedViewXmlDoc = false;
            }
            else if (subAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.investments
                && (sNodeName == Economics1.INVESTMENT_TYPES.investmentcomponent.ToString()
                || sNodeName == Economics1.INVESTMENT_TYPES.investmentoutcome.ToString()
                || sNodeName == Economics1.INVESTMENT_TYPES.investmentoutput.ToString()
                || sNodeName == Economics1.INVESTMENT_TYPES.investmentinput.ToString()))
            {
                bIsLinkedViewXmlDoc = false;
            }
            else if (subAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.outcomeprices
                && (sNodeName == Prices.OUTCOME_PRICE_TYPES.outcomeoutput.ToString()))
            {
                bIsLinkedViewXmlDoc = false;
            }
            else if (subAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.operationprices
                && (sNodeName == Prices.OPERATION_PRICE_TYPES.operationinput.ToString()))
            {
                bIsLinkedViewXmlDoc = false;
            }
            else if (subAppType
                == Helpers.GeneralHelpers.SUBAPPLICATION_TYPES.componentprices
                && (sNodeName == Prices.COMPONENT_PRICE_TYPES.componentinput.ToString()))
            {
                bIsLinkedViewXmlDoc = false;
            }
            return bIsLinkedViewXmlDoc;
        }
        public static string GetFullXhtmlDocPath(ContentURI uri,
            string xmlDocPath)
        {
            //used by displayhelper to determine whether or not
            //the fullxml doc needs conversion to xhtml (for packaging)
            string sFullTotalsFilePath = string.Empty;
            if (uri.URIDataManager.TempDocSaveMethod
                == Helpers.AddInHelper.SAVECALCS_METHOD.calcs.ToString())
            {
                sFullTotalsFilePath
                    = Helpers.GeneralHelpers.AddDocExtensionToPath(xmlDocPath,
                       Helpers.GeneralHelpers.FILENAME_EXTENSIONS.full);
                if (!Helpers.FileStorageIO.URIAbsoluteExists(uri,
                    sFullTotalsFilePath))
                {
                    sFullTotalsFilePath = string.Empty;
                }
            }
            return sFullTotalsFilePath;
        }
        public static string GetLinkedViewStartParams(bool needsSingleQuote,
            ContentURI docToCalcURI, string selectionBoxName,
            ref string calcDocURIPattern, ref string docToCalcURIPattern,
            ref string selectedLinkedViewURIPattern)
        {
            string sTempDocURI = string.Empty;
            string sLinkedViewURIPattern = string.Empty;
            ContentURI selectedViewURI =
                Helpers.LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
            if (selectedViewURI != null)
            {
                //docToCalcURI is usually a linkedviewpack
                //selectedViewURI is usually a custom doc or subfolder
                selectedLinkedViewURIPattern = selectedViewURI.URIPattern;
                docToCalcURIPattern = docToCalcURI.URIPattern;
                //calcdocuri is in selectedviewuri.lvs collection
                ContentURI selectedViewAddInURI = Helpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(selectedViewURI);
                if (selectedViewAddInURI != null)
                {
                    //2.0.0 condition needed for 2nd drop down
                    calcDocURIPattern = (calcDocURIPattern != Helpers.GeneralHelpers.NONE) ?
                        selectedViewAddInURI.URIPattern : calcDocURIPattern; 
                }
                if (selectionBoxName == string.Empty)
                {
                    sTempDocURI = (string.IsNullOrEmpty(selectedViewURI.URIDataManager.TempDocURIPattern)) ?
                        string.Empty : selectedViewURI.URIDataManager.TempDocURIPattern;
                }
                //2.0.0 condition added to stay consistent with next condition and with devpack drop down selections calcparams
                if (docToCalcURI.URIDataManager.UseSelectedLinkedView
                    && (!string.IsNullOrEmpty(docToCalcURI.URIDataManager.ParentURIPattern))
                    && (docToCalcURI.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                    || docToCalcURI.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks))
                {
                    //doctocalcuri is already selected view, it needs to ref its parent
                    selectedLinkedViewURIPattern = docToCalcURI.URIPattern;
                    docToCalcURIPattern = docToCalcURI.URIDataManager.ParentURIPattern;
                    if (selectionBoxName == string.Empty)
                    {
                        //selections must change views, but other actions maintain the same view
                        sTempDocURI = (string.IsNullOrEmpty(docToCalcURI.URIDataManager.TempDocURIPattern)) ?
                            string.Empty : docToCalcURI.URIDataManager.TempDocURIPattern;
                    }
                }
            }
            else
            {
                if (docToCalcURI.URIDataManager.UseSelectedLinkedView
                    && (!string.IsNullOrEmpty(docToCalcURI.URIDataManager.ParentURIPattern))
                    && (docToCalcURI.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.linkedviews
                    || docToCalcURI.URIDataManager.AppType == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks))
                {
                    //doctocalcuri is already selected view, it needs to ref its parent
                    selectedLinkedViewURIPattern = docToCalcURI.URIPattern;
                    docToCalcURIPattern = docToCalcURI.URIDataManager.ParentURIPattern;
                    if (selectionBoxName == string.Empty)
                    {
                        //selections must change views, but other actions maintain the same view
                        sTempDocURI = (string.IsNullOrEmpty(docToCalcURI.URIDataManager.TempDocURIPattern)) ?
                            string.Empty : docToCalcURI.URIDataManager.TempDocURIPattern;
                    }
                }
                else
                {
                    docToCalcURIPattern = docToCalcURI.URIPattern;
                    if (selectionBoxName == string.Empty)
                    {
                        //selections must change views, but other actions maintain the same view
                        sTempDocURI = (string.IsNullOrEmpty(docToCalcURI.URIDataManager.TempDocURIPattern)) ?
                            string.Empty : docToCalcURI.URIDataManager.TempDocURIPattern;
                    }
                }
            }
            ContentURI selectedAddInURI =
                Helpers.LinqHelpers.GetLinkedViewIsSelectedAddIn(docToCalcURI);
            if (selectedAddInURI != null)
            {
                //2.0.0 condition needed for 2nd drop down
                calcDocURIPattern = (calcDocURIPattern != Helpers.GeneralHelpers.NONE) ? 
                    selectedAddInURI.URIPattern : calcDocURIPattern;
                if (selectionBoxName == string.Empty
                    && sTempDocURI == string.Empty)
                {
                    //should have been set above, but the same tempdocuri is used by all
                    sTempDocURI = (string.IsNullOrEmpty(selectedAddInURI.URIDataManager.TempDocURIPattern)) ?
                        string.Empty : selectedAddInURI.URIDataManager.TempDocURIPattern;
                }
            }
            //check for tempdocs
            GetLinkedViewTempDocs(docToCalcURI, ref docToCalcURIPattern,
                ref sLinkedViewURIPattern);
            string sDefaultAddInAppType = GetDefaultAddInAppType(docToCalcURI);
            string sLinkedViewParams = string.Empty;
            if (string.IsNullOrEmpty(sLinkedViewURIPattern))
            {
                sLinkedViewParams = GetLinkedViewStartParams(needsSingleQuote,
                   selectionBoxName, docToCalcURIPattern, calcDocURIPattern,
                   sTempDocURI, selectedLinkedViewURIPattern, sDefaultAddInAppType);
            }
            else
            {
                sLinkedViewParams = GetLinkedViewStartParams(needsSingleQuote,
                    selectionBoxName, docToCalcURIPattern, calcDocURIPattern,
                    sTempDocURI, sLinkedViewURIPattern, sDefaultAddInAppType);
            }
            return sLinkedViewParams;
        }

        public static void GetLinkedViewTempDocs(ContentURI docToCalcURI,
            ref string docToCalcURIPattern, ref string linkedViewsURIPattern)
        {
            if (docToCalcURI.URIFileExtensionType
                == Helpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
            {
                //can't run addins for tempdocs without these params
                //set nodetocalc
                if (!string.IsNullOrEmpty(
                    docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern))
                {
                    docToCalcURIPattern
                        = docToCalcURI.URIDataManager.TempDocNodeToCalcURIPattern;
                }
                linkedViewsURIPattern = GetLinkedViewURIPattern(docToCalcURI);
            }
        }
        public static string GetLinkedViewURIPattern(ContentURI docToCalcURI)
        {
            string sLinkedViewURIPattern = string.Empty;
            //set calcdoc (has good dbkey which can retrieve linkedviews)
            //tempdocs always use linkedviewpack (parent of any linkedview)
            //which is stored in docToCalcURI.URIDataManager.Resource
            //or any linkedview.ParentId
            ContentURI linkedViewsURI = Helpers.LinqHelpers.GetContentURIByNodeName(
                docToCalcURI.URIDataManager.Resource,
                AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedviewpack.ToString());
            if (linkedViewsURI != null)
            {
                sLinkedViewURIPattern = linkedViewsURI.URIPattern;
            }
            return sLinkedViewURIPattern;
        }
        public static string GetDefaultAddInStartParam(ContentURI docToCalcURI)
        {
            string sDefaultAddInAppType = GetDefaultAddInAppType(docToCalcURI); ;
            string sAddInStartParam = string.Concat("&usedefault=",
                sDefaultAddInAppType);
            return sAddInStartParam;
        }
        private static string GetDefaultAddInAppType(ContentURI uri)
        {
            string sDefaultAddInAppType = string.Empty;
            if (uri.URIDataManager.UseDefaultAddIn)
            {
                sDefaultAddInAppType
                    = Helpers.GeneralHelpers.APPLICATION_TYPES.addins.ToString();
            }
            else if (uri.URIDataManager.UseDefaultLocal)
            {
                sDefaultAddInAppType
                    = Helpers.GeneralHelpers.APPLICATION_TYPES.locals.ToString();
            }
            return sDefaultAddInAppType;
        }
        public static string GetLinkedViewStartParams(bool needsSingleQuote,
            string selectionBoxName, string docToCalcURI, string calcDocURI,
            string tempDocURI, string linkedViewsURI, string defaultAddInAppType)
        {
            string sSingleQuote = (needsSingleQuote) ? "'" : string.Empty;
            string sLinkedViewParams = string.Concat(sSingleQuote,
                GetLinkedViewStartParams(selectionBoxName, docToCalcURI,
                    calcDocURI, tempDocURI, linkedViewsURI,
                    defaultAddInAppType), sSingleQuote);
            return sLinkedViewParams;
        }
        public static string GetLinkedViewStartParamsFromForm(ContentURI docToCalcURI)
        {
            string sLinkedViewParams = string.Empty;
            if (docToCalcURI.URIDataManager.FormInput != null)
            {
                string sCalcDocURI = Helpers.GeneralHelpers.GetFormValue(docToCalcURI, CALCDOCURI);
                string sDocToCalcURI = Helpers.GeneralHelpers.GetFormValue(docToCalcURI, DOCTOCALCURI);
                string sLinkedViewURI = Helpers.GeneralHelpers.GetFormValue(docToCalcURI, LINKEDVIEWSURI);
                //tempdocs need an extra param to retrieve db linkedviews
                sLinkedViewParams = string.Concat(
                    Helpers.GeneralHelpers.MakeFormElement(CALCDOCURI, sCalcDocURI),
                    Helpers.GeneralHelpers.MakeFormElement(DOCTOCALCURI, sDocToCalcURI),
                    Helpers.GeneralHelpers.MakeFormElement(TEMPDOCURI, string.Empty),
                    Helpers.GeneralHelpers.MakeFormElement(LINKEDVIEWSURI, sLinkedViewURI));
            }
            return sLinkedViewParams;
        }

        public static string GetLinkedViewStartParams(string selectionBoxName,
            string docToCalcURI, string calcDocURI, string tempDocURI,
            string linkedViewsURI, string defaultAddInAppType)
        {
            string sLinkedViewParams = string.Empty;
            StringBuilder linkedParamsBlder = new StringBuilder();
            //2.0.0 added the NONE condition for better flexiblity in excluding calcparams
            if (selectionBoxName == string.Empty)
            {
                if (calcDocURI != string.Empty && calcDocURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(CALCDOCURI, calcDocURI));
                if (docToCalcURI != string.Empty && docToCalcURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(DOCTOCALCURI, docToCalcURI));
                if (tempDocURI != string.Empty && tempDocURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(TEMPDOCURI, tempDocURI));
                if (linkedViewsURI != string.Empty && linkedViewsURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(LINKEDVIEWSURI, linkedViewsURI));
                if (defaultAddInAppType != string.Empty && defaultAddInAppType != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(USEDEFAULT, defaultAddInAppType));
            }
            else
            {
                //client script expects &selectslinkedviewsid= to be last param
                if (calcDocURI != string.Empty && calcDocURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(CALCDOCURI, calcDocURI));
                if (docToCalcURI != string.Empty && docToCalcURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(DOCTOCALCURI, docToCalcURI));
                if (tempDocURI != string.Empty && tempDocURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(TEMPDOCURI, tempDocURI));
                if (linkedViewsURI != string.Empty && linkedViewsURI != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(LINKEDVIEWSURI, linkedViewsURI));
                if (defaultAddInAppType != string.Empty && defaultAddInAppType != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(USEDEFAULT, defaultAddInAppType));
                if (defaultAddInAppType != string.Empty && defaultAddInAppType != Helpers.GeneralHelpers.NONE)
                    linkedParamsBlder.Append(Helpers.GeneralHelpers.MakeFormElement(SELECTSLINKEDVIEWSID, defaultAddInAppType));
            }
            sLinkedViewParams = linkedParamsBlder.ToString();
            return sLinkedViewParams;
        }
        public static string GetLinkedViewIdStartParam(string linkedViewId)
        {
            string sLinkedViewIdFormElement = string.Empty;
            if (!string.IsNullOrEmpty(linkedViewId))
            {
                //doctocalcuri is a standard uri that can display a list of children linkedviews
                //linkedViewId is an id of one of its linkedviews (displays one of the children linkedviews)
                sLinkedViewIdFormElement = Helpers.GeneralHelpers.MakeFormElement(LINKEDVIEWSID, LINKEDVIEWSID);
            }
            return sLinkedViewIdFormElement;
        }
        public static string AddLinkedViewForSelectionBoxes(string selectionBoxName)
        {
            //selection box onclick events use selectionbox form params to init state
            string sLinkedViewParams = string.Concat("&selectslinkedviewsid=",
                selectionBoxName);
            return sLinkedViewParams;
        }
        public static string AddLinkedViewDefaultParam(string calcParams,
            bool useDefaultAddin)
        {
            string sLinkedViewDefaultParam = string.Empty;
            if (useDefaultAddin)
            {
                sLinkedViewDefaultParam = string.Concat(calcParams,
                    Helpers.GeneralHelpers.MakeFormElement(USEDEFAULT, Helpers.GeneralHelpers.APPLICATION_TYPES.addins.ToString()));
            }
            else
            {
                sLinkedViewDefaultParam = string.Concat(calcParams,
                    Helpers.GeneralHelpers.MakeFormElement(USEDEFAULT, Helpers.GeneralHelpers.APPLICATION_TYPES.locals.ToString()));
            }
            return sLinkedViewDefaultParam;
        }

        private static void UpdateLinkedViewListMember(ContentURI linkedview,
            ref XElement linkedViewEl)
        {
            //minimal attribute list for a stateful linkedview
            linkedViewEl.SetAttributeValue(Calculator.cId, linkedview.URIId.ToString());
            linkedViewEl.SetAttributeValue(Calculator.cName, linkedview.URIName);
            linkedViewEl.SetAttributeValue("NetworkPartName", linkedview.URINetworkPartName);
            linkedViewEl.SetAttributeValue("NodeName", linkedview.URINodeName);
            if (linkedview.URIFileExtensionType != string.Empty)
                linkedViewEl.SetAttributeValue(Calculator.cFileExtensionType, linkedview.URIFileExtensionType);
            linkedViewEl.SetAttributeValue("URIPattern", linkedview.URIPattern);
            linkedViewEl.SetAttributeValue("URIFull", linkedview.URIFull);
            linkedViewEl.SetAttributeValue("ParentURIPattern", linkedview.URIDataManager.ParentURIPattern);
            if (linkedview.URIClub.ClubDocFullPath != string.Empty)
                linkedViewEl.SetAttributeValue("ClubDocFullPath", linkedview.URIClub.ClubDocFullPath);
            linkedViewEl.SetAttributeValue("AddInName", linkedview.URIDataManager.AddInName);
            linkedViewEl.SetAttributeValue("HostName", linkedview.URIDataManager.HostName);
            ContentURI stylesheetURI
                = Helpers.LinqHelpers.GetContentURIListIsMainStylesheet(
                linkedview.URIDataManager.Resource);
            if (stylesheetURI != null)
            {
                if (Path.GetFileName(stylesheetURI.URIDataManager.FileSystemPath) != string.Empty)
                    linkedViewEl.SetAttributeValue("StylesheetResourceFileName",
                        Path.GetFileName(stylesheetURI.URIDataManager.FileSystemPath));
                if (stylesheetURI.URIPattern != string.Empty)
                    linkedViewEl.SetAttributeValue("StylesheetResourceURI",
                        stylesheetURI.URIPattern);
                if (stylesheetURI.URIDataManager.FileSystemPath != string.Empty)
                    linkedViewEl.SetAttributeValue("StylesheetResourceDocPath",
                        stylesheetURI.URIDataManager.FileSystemPath);
            }
        }
        public async Task<string> AddLinkedViewAsync(ContentURI uri,
            IDictionary<string, string> newLinkedView, bool isSelections, string oldInsertedIds)
        {
            string sInsertedIdsArray = string.Empty;
            //the insertedIdsArray will be used to update the associated xmldocs
            //once updated, those will be inserted using UpdateLinkedViewXmlDocs
            string sArgDelimiter = Helpers.GeneralHelpers.PARAMETER_DELIMITER;
            string sPropDelimiter = Helpers.GeneralHelpers.STRING_DELIMITER;
            string sSelectedViewsArray = GetSelectionsArray(ref newLinkedView, 
                isSelections, sArgDelimiter, sPropDelimiter);
            if (sSelectedViewsArray != string.Empty)
            {
                Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                SqlParameter[] colPrams = 
			    { 
			        sqlIO.MakeInParam("@SelectedViewsArray",    SqlDbType.NChar, 4000, sSelectedViewsArray),
                    sqlIO.MakeInParam("@ArgDelimiter",          SqlDbType.NChar, 1, sArgDelimiter),
                    sqlIO.MakeInParam("@PropDelimiter",         SqlDbType.NChar, 1, sPropDelimiter),
                    sqlIO.MakeOutParam("@InsertedIdsArray",     SqlDbType.NChar, 2000)
			    };
                string sQryName = "0InsertLinkedViews";
                int iNotUsed = await sqlIO.RunProcIntAsync(sQryName, colPrams);
                if (colPrams[3].Value != System.DBNull.Value)
                {
                    sInsertedIdsArray = colPrams[3].Value.ToString() + oldInsertedIds;
                }
                sqlIO.Dispose();
            }
            //recurse to the next 31
            if (newLinkedView.Count > 0)
            {
                sInsertedIdsArray = await AddLinkedViewAsync(uri, newLinkedView, isSelections, sInsertedIdsArray);
            }
            return sInsertedIdsArray;
        }
        private string GetSelectionsArray(ref IDictionary<string, string> newLinkedView, 
            bool isSelections, string argDelimiter, string propDelimiter)
        {
            string sSelectedViewsArray = string.Empty;
            //selections use the linkedviews as unique keys
            //while other inserts (i.e. descendants) use the linkingnodes as unique keys
            if (newLinkedView != null)
            {
                if (newLinkedView.Count > 0)
                {
                    string sLinkingNodeURIPattern = string.Empty;
                    string sLinkingNodeName = string.Empty;
                    string sLinkingNodeId = string.Empty;
                    string sSelectedURIPattern = string.Empty;
                    string sSelectedNodeName = string.Empty;
                    string sSelectedLiewId = string.Empty;
                    int i = 0;
                    List<string> keysToRemove = new List<string>();
                    foreach (KeyValuePair<string, string> kvp in newLinkedView)
                    {
                        //sql server has a 32 limit on recursive sps
                        if (i <= 29)
                        {
                            if (isSelections)
                            {
                                sSelectedURIPattern = kvp.Key;
                                if (!string.IsNullOrEmpty(sSelectedURIPattern))
                                {
                                    sSelectedNodeName = ContentURI.GetURIPatternPart(sSelectedURIPattern,
                                        ContentURI.URIPATTERNPART.node);
                                    if (sSelectedNodeName
                                        == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                    {
                                        sSelectedLiewId = ContentURI.GetURIPatternPart(sSelectedURIPattern,
                                            ContentURI.URIPATTERNPART.id);
                                        sLinkingNodeURIPattern = kvp.Value;
                                        if (!string.IsNullOrEmpty(sLinkingNodeURIPattern))
                                        {
                                            sLinkingNodeName = ContentURI.GetURIPatternPart(sLinkingNodeURIPattern,
                                                ContentURI.URIPATTERNPART.node);
                                            sLinkingNodeId = ContentURI.GetURIPatternPart(sLinkingNodeURIPattern,
                                                ContentURI.URIPATTERNPART.id);
                                            //numbers and nodes are parsed using delimiters
                                            sSelectedViewsArray += string.Concat(sLinkingNodeName,
                                                propDelimiter, sLinkingNodeId, argDelimiter,
                                                sSelectedNodeName, propDelimiter, sSelectedLiewId, argDelimiter);
                                        }
                                    }
                                    keysToRemove.Add(sSelectedURIPattern);
                                }
                            }
                            else
                            {
                                sLinkingNodeURIPattern = kvp.Key;
                                if (!string.IsNullOrEmpty(sLinkingNodeURIPattern))
                                {
                                    sLinkingNodeName = ContentURI.GetURIPatternPart(sLinkingNodeURIPattern,
                                        ContentURI.URIPATTERNPART.node);
                                    sLinkingNodeId = ContentURI.GetURIPatternPart(sLinkingNodeURIPattern,
                                        ContentURI.URIPATTERNPART.id);
                                    sSelectedURIPattern = kvp.Value;
                                    if (!string.IsNullOrEmpty(sSelectedURIPattern))
                                    {
                                        sSelectedNodeName = ContentURI.GetURIPatternPart(sSelectedURIPattern,
                                            ContentURI.URIPATTERNPART.node);
                                        if (sSelectedNodeName
                                            == AppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                                        {
                                            sSelectedLiewId = ContentURI.GetURIPatternPart(sSelectedURIPattern,
                                                ContentURI.URIPATTERNPART.id);
                                            //numbers and nodes are parsed using delimiters
                                            sSelectedViewsArray += string.Concat(sLinkingNodeName,
                                                propDelimiter, sLinkingNodeId, argDelimiter,
                                                sSelectedNodeName, propDelimiter, sSelectedLiewId, argDelimiter);
                                        }
                                    }
                                    keysToRemove.Add(sLinkingNodeURIPattern);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                        i++;
                    }
                    //remove used keys
                    foreach (string key in keysToRemove)
                    {
                        newLinkedView.Remove(key);
                    }
                }
            }
            return sSelectedViewsArray;
        }
        public void GetNewLinkedView(IDictionary<string, string> linkedViewsInserted,
            string insertedIdsArray, ref IDictionary<string, string> newLinkedViewInserted)
        {
            string[] arrLinkedView = insertedIdsArray.Split(Helpers.GeneralHelpers.PARAMETER_DELIMITERS);
            string[] arrLinkedViews = { };
            if (arrLinkedView != null)
            {
                int i = 0;
                string sLinkedView = string.Empty;
                string sLinkingNodeId = string.Empty;
                string sLinkingNodeId2 = string.Empty;
                string sLinkingNodeURIPattern = string.Empty;
                string sLinkingNodeName = string.Empty;
                string sNewLinkedViewJoinId = string.Empty;
                string sSelectedURIPattern = string.Empty;
                string sNewSelectedURIPattern = string.Empty;
                for (i = 0; i < arrLinkedView.Count(); i++)
                {
                    sLinkedView = arrLinkedView[i].Trim();
                    if (!string.IsNullOrEmpty(sLinkedView))
                    {
                        arrLinkedView = sLinkedView.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                        if (arrLinkedView != null)
                        {
                            if (arrLinkedView.Length > 1)
                            {
                                sLinkingNodeId = arrLinkedView[0];
                                sNewLinkedViewJoinId = arrLinkedView[1];
                                foreach (KeyValuePair<string, string> kvp in linkedViewsInserted)
                                {
                                    sLinkingNodeURIPattern = kvp.Key;
                                    if (!string.IsNullOrEmpty(sLinkingNodeURIPattern))
                                    {
                                        sLinkingNodeId2 = ContentURI.GetURIPatternPart(sLinkingNodeURIPattern,
                                            ContentURI.URIPATTERNPART.id);
                                        if (sLinkingNodeId.Equals(sLinkingNodeId2))
                                        {
                                            //update the id of the value field
                                            sSelectedURIPattern = kvp.Value;
                                            if (!string.IsNullOrEmpty(sSelectedURIPattern))
                                            {
                                                sNewSelectedURIPattern =
                                                    ContentURI.ChangeURIPatternPart(sSelectedURIPattern,
                                                    ContentURI.URIPATTERNPART.id, sNewLinkedViewJoinId);
                                                if (!newLinkedViewInserted.ContainsKey(sLinkingNodeURIPattern))
                                                {
                                                    newLinkedViewInserted.Add(sLinkingNodeURIPattern, sNewSelectedURIPattern);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void GetLinkedViewPath(XmlElement childElement, ContentURI calcDocURI,
            ref string newCalculatedDocPath, ref string childLinkedViewPattern)
        {
            string sChildLinkedViewId = EditHelpers.XmlLinq.GetFirstChildLinkedViewIdUsingAttribute(
                childElement, AppHelpers.Calculator.cFileExtensionType, calcDocURI.URIFileExtensionType);
            if (!string.IsNullOrEmpty(sChildLinkedViewId))
            {
                string sNewSelectedURIPattern = ContentURI.ChangeURIPatternPart(
                    calcDocURI.URIPattern, ContentURI.URIPATTERNPART.id, sChildLinkedViewId);
                string sOldFileName = Path.GetFileNameWithoutExtension(newCalculatedDocPath);
                string sNewFileName = Helpers.ContentHelper
                    .MakeStandardFileNameFromURIPattern(sNewSelectedURIPattern);
                newCalculatedDocPath = newCalculatedDocPath
                    .Replace(sOldFileName, sNewFileName);
                newCalculatedDocPath = AddFullExtensionToPath(sOldFileName, newCalculatedDocPath);
                childLinkedViewPattern = sNewSelectedURIPattern;
            }
        }
        public static string AddFullExtensionToPath(string oldFileName, string newCalculatedDocPath)
        {
            string sNewCalculatedDocPath = newCalculatedDocPath;
            if (oldFileName.EndsWith(Helpers.GeneralHelpers.FILENAME_EXTENSIONS.full.ToString()))
            {
                if (!newCalculatedDocPath.EndsWith(Helpers.GeneralHelpers.FILENAME_EXTENSIONS.full.ToString()))
                {
                    sNewCalculatedDocPath = Helpers.GeneralHelpers.AddDocExtensionToPath(newCalculatedDocPath,
                        Helpers.GeneralHelpers.FILENAME_EXTENSIONS.full);
                }
            }
            return sNewCalculatedDocPath;
        }
        public void GetLinkedViewPath(IDictionary<string, string> childrenLinkedView,
            ref string newCalculatedDocPath, ref string childLinkedViewPattern)
        {
            //linked lists is the GetNewLinkedView() dictionary saved as a string
            if (childrenLinkedView != null
                && !string.IsNullOrEmpty(newCalculatedDocPath))
            {
                string sDirPath = string.Empty;
                string sLinkingNodeURIPattern = string.Empty;
                string sLinkingNodeName = string.Empty;
                string sLinkingNodeId = string.Empty;
                string sNewSelectedURIPattern = string.Empty;
                string sOldFileName = Path.GetFileNameWithoutExtension(newCalculatedDocPath);
                string sNewFileName = string.Empty;
                foreach (KeyValuePair<string, string> kvp in childrenLinkedView)
                {
                    sLinkingNodeURIPattern = kvp.Key;
                    if (!string.IsNullOrEmpty(sLinkingNodeURIPattern))
                    {
                        sLinkingNodeId = ContentURI.GetURIPatternPart(
                            sLinkingNodeURIPattern, ContentURI.URIPATTERNPART.id);
                        sLinkingNodeName = ContentURI.GetURIPatternPart(
                            sLinkingNodeURIPattern, ContentURI.URIPATTERNPART.node);
                        if (!string.IsNullOrEmpty(sLinkingNodeId)
                            && !string.IsNullOrEmpty(sLinkingNodeName))
                        {
                            string sDelimiter = Helpers.FileStorageIO.GetDelimiterForFileStorage(newCalculatedDocPath);
                            sDirPath = string.Concat(sLinkingNodeName,
                                Helpers.GeneralHelpers.FILENAME_DELIMITER,
                                sLinkingNodeId, sDelimiter);
                            string sNewCalculatedDocDir 
                                = Helpers.FileStorageIO.GetDirectoryName(newCalculatedDocPath);
                            if (!sNewCalculatedDocDir.EndsWith(sDelimiter))
                            {
                                sNewCalculatedDocDir = string.Concat(sNewCalculatedDocDir, sDelimiter);
                            }
                            if (sNewCalculatedDocDir.EndsWith(sDirPath))
                            {
                                sNewSelectedURIPattern = kvp.Value;
                                sNewFileName = Helpers.ContentHelper
                                    .MakeStandardFileNameFromURIPattern(sNewSelectedURIPattern);
                                newCalculatedDocPath = newCalculatedDocPath
                                    .Replace(sOldFileName, sNewFileName);
                                newCalculatedDocPath = AddFullExtensionToPath(sOldFileName, newCalculatedDocPath);
                                childLinkedViewPattern = sNewSelectedURIPattern;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public static bool DeleteLinkedViewFromLinkingNode(ContentURI uri,
            EditHelpers.EditHelper.ArgumentsEdits argumentsEdits,
            XElement root)
        {
            bool bIsDeleted = false;  
            XNamespace y0 = EditHelpers.XmlLinq.GetNamespaceForNode(
                root, argumentsEdits.URIToAdd.URINodeName);
            bIsDeleted = EditHelpers.XmlLinq.DeleteLinkedViewUsingURIToAddandURIToEdit(
                y0, root, argumentsEdits);
            return bIsDeleted;
        }
        private static string GetLinkedViewDeletionQry(
            EditHelpers.EditHelper.ArgumentsEdits argumentsEdits)
        {
            string sQryName = string.Empty;
            switch (argumentsEdits.URIToEdit.URIDataManager.AppType)
            {
                case Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks:
                    DevPacks.GetUpdateDevPackJoinQueryParams(argumentsEdits.URIToEdit,
                        out sQryName);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.economics1:
                    sQryName
                        = Economics1.GetUpdateEconomics1QueryName(argumentsEdits.URIToEdit);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.prices:
                    sQryName 
                        = Prices.GetUpdatePricesQueryName(argumentsEdits.URIToEdit);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.locals:
                    sQryName
                        = Locals.GetUpdateLocalQueryName(argumentsEdits.URIToEdit);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.resources:
                    sQryName
                        = Resources.GetUpdateResourceQueryName(argumentsEdits.URIToEdit);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.addins:
                    //addins don't store xmldocs
                    break;
                default:
                    break;
            }
            return sQryName;
        }
    }
}
