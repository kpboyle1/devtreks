using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DevTreks.Data.EditHelpers
{
    /// <summary>
    ///Purpose:		Xml node editing class
    ///Author:		www.devtreks.org
    ///Date:		2016, April
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class EditHelper
    {
        public EditHelper() { }
        public const string DELETE = "delete";
        private const string PARENTID = "ParentId";
        private const string LAST_CHANGED_cDate = "LastChangedDate";
        private const string IS_DEFAULT = "IsDefault";
        
        
        public static EditHelper.ArgumentsEdits MakeArgumentEdits(ContentURI uri,
           IDictionary<string, string> colUpdates)
        {
            EditHelper.ArgumentsEdits oArgumentEdits = new EditHelper.ArgumentsEdits();
            oArgumentEdits.ParentURIPattern = uri.URIPattern;
            oArgumentEdits.IsDbEdit = Helpers.GeneralHelpers.IsDbEdit(uri);
            oArgumentEdits.NeedsBaseIds = false;
            oArgumentEdits.URIToEdit = new ContentURI(uri);
            return oArgumentEdits;
        }
 
        public static void SetUpdateSchemaProperties(
            bool isDeletes, EditHelper.ArgumentsEdits argumentsEdits)
        {
            //tells apps to use edit schemas
            argumentsEdits.NeedsFullSchema = true;
            argumentsEdits.IsUpdateGram = true;
            if (argumentsEdits.URIToEdit.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                if (argumentsEdits.URIToEdit.URIDataManager.SubActionView
                    == Helpers.GeneralHelpers.SUBACTION_VIEWS.categories.ToString())
                {
                    argumentsEdits.IsUpdateGram = true;
                }
                else
                {
                    //want full schema because because it allows
                    //both the base and join tables to be updated with
                    //one updategram
                    argumentsEdits.IsUpdateGram = false;
                }
            }
            else if (argumentsEdits.URIToEdit.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                //except when running addins for customdocs
                if (argumentsEdits.URIToEdit.URIDataManager.ServerSubActionType
                    != Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                {
                    //want single node schemas because they allow
                    //both the base and join tables to be updated with
                    //one updategram
                    argumentsEdits.IsUpdateGram = false;
                    argumentsEdits.NeedsFullSchema = false;
                }
            }
        }
        
        public async Task<bool> MakeDeletionsAsync(
            ContentURI uri, EditHelper.ArgumentsEdits argumentsEdits,
            StringDictionary colDeletes, XElement devTrekLinqRoot)
        {
            bool bIsOkToSave = false;
            if (colDeletes != null)
            {
                string sAction = string.Empty;
                foreach (string sKeyName in colDeletes.Keys)
                {
                    bIsOkToSave = false;
                    //1. sKeyName is a 'searchname;delete' delimited string
                    String[] arrDeleteParams
                        = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                    if (arrDeleteParams.Length > 1)
                    {
                        sAction = string.Empty;
                        sAction = arrDeleteParams[1];
                        if (sAction.ToLower().StartsWith(DELETE))
                        {
                            //convention: URIToAdd is each node being inserted or deleted
                            //URIToEdit is base node (node getting insertions or node holding document)
                            argumentsEdits.URIToAdd.URIPattern = string.Empty;
                            argumentsEdits.URIToAdd.URIPattern = arrDeleteParams[0];
                            Helpers.GeneralHelpers.SetURIParams(argumentsEdits.URIToAdd);
                            bool bHasSet = await ChangeSpecialAttributeValuesAsync(
                                uri, argumentsEdits, devTrekLinqRoot);
                            //delete this node and prepare regular updategram
                            bIsOkToSave = Delete(uri, argumentsEdits,
                                devTrekLinqRoot);
                        }
                    }
                }
            }
            return bIsOkToSave;
        }
        
        private bool Delete(ContentURI uri,
            EditHelper.ArgumentsEdits argumentsEdits,
            XElement root)
        {
            bool bIsDeleted = false;
            XNamespace y0 = XmlLinq.GetNamespaceForNode(
                root, argumentsEdits.URIToAdd.URINodeName);
            if (y0 != null)
            {
                bIsDeleted = XmlLinq.DeleteElementUsingURIToAdd(y0, root,
                    argumentsEdits);
                //updategrams don't need the prefix/namespace
                argumentsEdits.URIToAdd.URINodeName
                    = argumentsEdits.URIToAdd.URINodeName.Replace(
                    Helpers.GeneralHelpers.NAMESPACE_DB_ABBREV_COLON,
                    string.Empty);
            }
            else
            {
                bIsDeleted = XmlLinq.DeleteElementUsingURIToAdd(y0, root,
                        argumentsEdits);
            }
            return bIsDeleted;
        }
        
        public async Task<bool> DeleteBase(ContentURI uri,
            EditHelper.ArgumentsEdits argumentsEdits,
            XElement root)
        {
            bool bIsDeleted = false;
            //definitely delete the join record (argumentEdits.URIToAdd.URIId)
            //and check to see if descendent base table records must also be deleted
            //(they need to be deleted if their related join is the top join record)
            bIsDeleted = await DeleteJoinAndCheckBaseAsync(uri, argumentsEdits.URIToAdd, argumentsEdits.IsDbEdit);
            if (bIsDeleted)
            {
                //delete the node from the xmldoc being edited
                bIsDeleted = DeleteJoinNode(argumentsEdits,
                    root);
            }
            return bIsDeleted;
        }

        public async Task<bool> DeleteJoinAndCheckBaseAsync(
            ContentURI uri, ContentURI deletionURI, bool isDbEdit)
        {
            //this should be moved to where the sqlio can be disposed normally
            bool bIsDeleted = false;
            if (isDbEdit)
            {
                string sQry = GetDeleteBaseandJoinQry(uri);
                Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
                if (!string.IsNullOrEmpty(sQry))
                {
                    SqlParameter[] colPrams = 
                    { 
                        sqlIO.MakeInParam("@Id",                SqlDbType.Int, 4, deletionURI.URIId),
                        sqlIO.MakeInParam("@NodeName",          SqlDbType.NVarChar, 25, deletionURI.URINodeName),
                        sqlIO.MakeInParam("@NetworkPartName",   SqlDbType.NVarChar, 20, deletionURI.URINetworkPartName)
                    };
                    int iIsCompleted = await sqlIO.RunProcIntAsync(
                        sQry, colPrams);
                    sqlIO.Dispose();
                    if (iIsCompleted == 1) bIsDeleted = true;
                }
            }
            return bIsDeleted;
        }
        private static string GetDeleteBaseandJoinQry(ContentURI uriToEdit)
        {
            string sQry = string.Empty;
            //if base nodes are deleted, they will cascade delete join records
            //if base nodes are not deleted, only the join record is deleted
            if (uriToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                //devpack and devpackpart base nodes might need to be deleted
                sQry = AppHelpers.DevPacks.GetDeleteBaseandJoinQry();
            }
            else if (uriToEdit.URIDataManager.AppType
                    == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                //service and incentive base nodes might need to be deleted
                sQry = AppHelpers.Agreement.GetDeleteBaseandJoinQry();
            }
            return sQry;
        }
        public bool DeleteJoinNode(EditHelper.ArgumentsEdits argumentsEdits,
            XElement root)
        {
            bool bIsDeleted = false;
            XNamespace y0 = XmlLinq.GetNamespaceForNode(
                root, argumentsEdits.URIToAdd.URINodeName);
            bIsDeleted = XmlLinq.DeleteElementUsingURIToAdd(y0, root, argumentsEdits);
            return bIsDeleted;
        }
        public async Task<bool> JoinIdsAreMatches(ContentURI uri,
            EditHelper.ArgumentsEdits argumentsEdits,
            string baseNodeName, string baseId)
        {
            bool bJoinIdsMatch = false;
            string sTopJoinId = string.Empty;
            if (uri.URIDataManager.AppType
                == Helpers.GeneralHelpers.APPLICATION_TYPES.members)
            {
                AppHelpers.Members member = new AppHelpers.Members();
                sTopJoinId = await member.GetTopClubToMemberJoinIdAsync(argumentsEdits.URIToAdd,
                    argumentsEdits.URIToAdd.URIDataManager.DefaultConnection,
                    baseNodeName, baseId, uri.URIMember.ClubInUse.PKId);
            }
            if (sTopJoinId.Equals(argumentsEdits.URIToAdd.URIId.ToString()))
            {
                bJoinIdsMatch = true;
            }
            return bJoinIdsMatch;
        }
        public async Task MakeUpdatesAsync(
            ContentURI uri, EditHelper.ArgumentsEdits argumentsEdits,
            IDictionary<string, string> colUpdates, XElement root)
        {
            if (colUpdates != null)
            {
                string sURIPattern = string.Empty;
                string sAttName = string.Empty;
                string sDataType = string.Empty;
                string sSize = string.Empty;
                bool bNeedsUpdate = true;
                foreach (string sKeyName in colUpdates.Keys)
                {
                    //1. sKeyName should be a 'uripattern;attname;datatype;size' delimited string
                    string[] arrUpdateParams
                        = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                    argumentsEdits.URIToAdd.URIPattern = string.Empty;
                    argumentsEdits.EditAttName = string.Empty;
                    argumentsEdits.EditAttValue = string.Empty;
                    GetStandardEditNameParams(arrUpdateParams,
                        out sURIPattern, out sAttName, out sDataType, out sSize);
                    if (!string.IsNullOrEmpty(sAttName))
                    {
                        //convention: URIToAdd is each node being inserted or deleted
                        //URIToAdd is base node (node getting insertions, node holding document)
                        argumentsEdits.URIToAdd.URIPattern = sURIPattern;
                        Helpers.GeneralHelpers.SetURIParams(argumentsEdits.URIToAdd);
                        argumentsEdits.EditAttName = sAttName;
                        argumentsEdits.EditAttValue = colUpdates[sKeyName].ToString();
                        //optional params
                        argumentsEdits.EditDataType = sDataType;
                        argumentsEdits.EditSize = sSize;
                        bNeedsUpdate = await ChangeSpecialAttributeValuesAsync(
                            uri, argumentsEdits, root);
                        if (bNeedsUpdate)
                        {
                            if (string.IsNullOrEmpty(uri.ErrorMessage))
                            {
                                //update the root element
                                Update(uri, argumentsEdits, root);
                            }
                        }
                        sURIPattern = string.Empty;
                        sAttName = string.Empty;
                        sDataType = string.Empty;
                        sSize = string.Empty;
                    }
                }
            }
        }
        public async Task<bool> ChangeSpecialAttributeValuesAsync(
            ContentURI uri, EditHelper.ArgumentsEdits argumentsEdits, XElement root)
        {
            bool bNeedsUpdate = true;
            //set dynamically (note that uploaded files have file names that aren't used here
            if (argumentsEdits.EditAttName.ToLower().Contains(
                IS_DEFAULT.ToLower()))
            {
                //grouped radios have a false uripattern in the name
                //and an id as the value
                if (argumentsEdits.EditAttValue.Length > 0)
                {
                    //update both current IsDefault setting and new setting at one time
                    bNeedsUpdate = await UpdateConcurrentSettingsAsync(uri,
                        argumentsEdits);
                }
            }
            
            return bNeedsUpdate;
        }
        
        private void UpdateXmlDocField(ContentURI uri,
            EditHelper.ArgumentsEdits argumentsEdits,
            IDictionary<string, string> colUpdates,
            XElement root)
        {
            //see if they are updating the corresponding xmldoc as well
            string sStepNumber = string.Empty;
            string sDbKeyName = GetDbKeyNameForAddIn(
                argumentsEdits.URIToAdd.URIPattern,
                Helpers.GeneralHelpers.ROOT_PATH,
                RuleHelpers.GeneralRules.XML, sStepNumber);
            //delete calculator files
            string sClubDocPath = (!string.IsNullOrEmpty(
                argumentsEdits.URIToAdd.URIClub.ClubDocFullPath)
                ? argumentsEdits.URIToAdd.URIClub.ClubDocFullPath
                : uri.URIClub.ClubDocFullPath);
            Helpers.FileStorageIO.DeleteURIsContainingSubstring(uri,
                sClubDocPath,
               string.Concat(Helpers.GeneralHelpers.FILENAME_DELIMITER,
               Helpers.GeneralHelpers.ADDIN.ToLower()));
            //delete calculated files
            IEnumerable linkedViewDocs = XmlLinq.GetChildrenLinkedView(
                root, argumentsEdits.URIToAdd.URINodeName, 
                argumentsEdits.URIToAdd.URIId.ToString());
            if (linkedViewDocs != null)
            {
                string sId = string.Empty;
                foreach (XElement linkedViewDoc in linkedViewDocs)
                {
                    sId = XmlLinq.GetAttributeValue(linkedViewDoc, AppHelpers.Calculator.cId);
                    if (!string.IsNullOrEmpty(sId))
                    {
                        Helpers.FileStorageIO.DeleteURIsContainingSubstring(
                            uri, sClubDocPath, sId);
                    }
                }
            }
        }
        private bool Update(ContentURI uri, 
            EditHelper.ArgumentsEdits argumentsEdits,
            XElement root)
        {
            bool bIsUpdated = false;
            //validate the user's input (editattvalue)
            RuleHelpers.GeneralRules.ValidateXSDInput(argumentsEdits);
            //update the node, if it changed, and return result; 
            XNamespace y0 = XmlLinq.GetNamespaceForNode(
                root, argumentsEdits.URIToAdd.URINodeName);
            if (y0 != null)
            {
                bIsUpdated = XmlLinq.UpdateElementUsingURIToAdd(y0, root,
                    argumentsEdits);
                //updategrams don't need the prefix/namespace
                argumentsEdits.URIToAdd.URINodeName
                    = argumentsEdits.URIToAdd.URINodeName.Replace(
                    Helpers.GeneralHelpers.NAMESPACE_DB_ABBREV_COLON, string.Empty);
            }
            else
            {
                bIsUpdated = XmlLinq.UpdateElementUsingURIToAdd(y0, root,
                    argumentsEdits);
            }
            return bIsUpdated;
        }

        public static string GetRecursiveParentKeyName()
        {
            //standard recursive relational key name
            string sForeignKeyName = PARENTID;
            return sForeignKeyName;
        }
        public static void GetParentsOfRecursiveNodesIds(
            EditHelper.ArgumentsEdits addsArguments,
            Helpers.GeneralHelpers.APPLICATION_TYPES appType,
            out string recursiveParentKeyName, 
            out string recursiveParentId)
        {
            recursiveParentKeyName = string.Empty;
            recursiveParentId = string.Empty;
            switch (appType)
            {
                case Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks:
                    AppHelpers.DevPacks.GetParentOfRecursiveNodesId(addsArguments,
                        out recursiveParentKeyName, out recursiveParentId);
                    break;
                default:
                    //don't render any ui
                    break;
            }
        }
        public static string GetForeignKeyName(Helpers.GeneralHelpers.APPLICATION_TYPES appType,
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES subAppType, string parentNodeName,
            bool isBaseKey)
        {
            string sForeignKeyName = string.Empty;
            string sChildForeignName = string.Empty;
            string sBaseForeignKeyName = string.Empty;
            switch (appType)
            {
                case Helpers.GeneralHelpers.APPLICATION_TYPES.accounts:
                    AppHelpers.Accounts.GetChildForeignKeyName(parentNodeName,
                        out sForeignKeyName);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.members:
                    AppHelpers.Members.GetChildForeignKeyName(parentNodeName,
                        out sChildForeignName, out sBaseForeignKeyName);
                    if (isBaseKey == true)
                    {
                        sForeignKeyName = sBaseForeignKeyName;
                    }
                    else
                    {
                        sForeignKeyName = sChildForeignName;
                    }
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.networks:
                    AppHelpers.Networks.GetChildForeignKeyName(parentNodeName,
                        out sChildForeignName, out sBaseForeignKeyName);
                    if (isBaseKey == true)
                    {
                        sForeignKeyName = sBaseForeignKeyName;
                    }
                    else
                    {
                        sForeignKeyName = sChildForeignName;
                    }
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.locals:
                    AppHelpers.Locals.GetChildForeignKeyName(parentNodeName,
                        out sChildForeignName, out sBaseForeignKeyName);
                    if (isBaseKey == true)
                    {
                        sForeignKeyName = sBaseForeignKeyName;
                    }
                    else
                    {
                        sForeignKeyName = sChildForeignName;
                    }
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.addins:
                    AppHelpers.AddIns.GetChildForeignKeyName(parentNodeName,
                        out sChildForeignName, out sBaseForeignKeyName);
                    if (isBaseKey == true)
                    {
                        sForeignKeyName = sBaseForeignKeyName;
                    }
                    else
                    {
                        sForeignKeyName = sChildForeignName;
                    }
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.agreements:
                    AppHelpers.Agreement.GetChildForeignKeyNames(parentNodeName,
                       out sChildForeignName, out sBaseForeignKeyName);
                    if (isBaseKey == true)
                    {
                        sForeignKeyName = sBaseForeignKeyName;
                    }
                    else
                    {
                        sForeignKeyName = sChildForeignName;
                    }
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.prices:
                    AppHelpers.Prices.GetChildForeignKeyName(subAppType, parentNodeName,
                        out sForeignKeyName);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.economics1:
                    AppHelpers.Economics1.GetChildForeignKeyName(subAppType, parentNodeName,
                        out sForeignKeyName);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks:
                    AppHelpers.DevPacks.GetChildForeignKeyName(parentNodeName,
                       out sChildForeignName, out sBaseForeignKeyName);
                    if (isBaseKey == true)
                    {
                        sForeignKeyName = sBaseForeignKeyName;
                    }
                    else
                    {
                        sForeignKeyName = sChildForeignName;
                    }
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.linkedviews:
                    AppHelpers.LinkedViews.GetChildForeignKeyName(parentNodeName,
                         out sForeignKeyName);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.resources:
                    AppHelpers.Resources.GetChildForeignKeyName(parentNodeName,
                         out sForeignKeyName);
                    break;
                default:
                    //don't render any ui
                    break;
            }
            return sForeignKeyName;
        }

        public static string GetBaseSchema(Helpers.GeneralHelpers.APPLICATION_TYPES appType)
        {
            string sBaseSchemaName = string.Empty;
            if (appType == Helpers.GeneralHelpers.APPLICATION_TYPES.agreements)
            {
                sBaseSchemaName = AppHelpers.Agreement.AGREEMENTSBASE_SCHEMA;
            }
            else if (appType == Helpers.GeneralHelpers.APPLICATION_TYPES.devpacks)
            {
                sBaseSchemaName = AppHelpers.DevPacks.DEVPACKBASEEDIT_SCHEMA;
            }
            return sBaseSchemaName;
        }
        private async Task<bool> UpdateConcurrentSettingsAsync( 
            ContentURI uri, EditHelper.ArgumentsEdits argumentsEdits)
        {
            bool bNeedsUpdate = true;
            int iNewIsDefaultId = 0;
            bool bIsUpdated = false;
            switch (argumentsEdits.URIToEdit.URIDataManager.AppType)
            {
                case Helpers.GeneralHelpers.APPLICATION_TYPES.members:
                    AppHelpers.Members oMember = new AppHelpers.Members();
                    iNewIsDefaultId = Helpers.GeneralHelpers.ConvertStringToInt(argumentsEdits.EditAttValue);
                    bIsUpdated = await oMember.UpdateDefaultClubForLoggedinMemberAsync(argumentsEdits.URIToAdd,
                        uri.URIMember.MemberId, iNewIsDefaultId);
                    break;
                case Helpers.GeneralHelpers.APPLICATION_TYPES.networks:
                    AppHelpers.Networks oNetwork = new AppHelpers.Networks();
                    iNewIsDefaultId = Helpers.GeneralHelpers.ConvertStringToInt(argumentsEdits.EditAttValue);
                    bIsUpdated = await oNetwork.UpdateDefaultNetworkForLoggedinMemberAsync(
                        argumentsEdits.URIToAdd, uri.URIId, iNewIsDefaultId);
                    break;
                default:
                    if (uri.URIDataManager.SubActionView
                        == Helpers.GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                    {
                        iNewIsDefaultId = Helpers.GeneralHelpers.ConvertStringToInt(argumentsEdits.EditAttValue);
                        AppHelpers.LinkedViews oLinkedView = new AppHelpers.LinkedViews();
                        bIsUpdated = await oLinkedView.UpdateDefaultLinkedViewAsync(uri, iNewIsDefaultId);
                    }
                    else if (argumentsEdits.URIToEdit.URIDataManager.AppType
                        == Helpers.GeneralHelpers.APPLICATION_TYPES.addins
                        || argumentsEdits.URIToEdit.URIDataManager.AppType
                        == Helpers.GeneralHelpers.APPLICATION_TYPES.locals)
                    {
                        //2.0.0 made the addin and local edit pattern the same as the rest
                        iNewIsDefaultId = Helpers.GeneralHelpers.ConvertStringToInt(argumentsEdits.EditAttValue);
                        AppHelpers.LinkedViews oLinkedView = new AppHelpers.LinkedViews();
                        bIsUpdated = await oLinkedView.UpdateDefaultLinkedViewAsync(uri, iNewIsDefaultId);
                    }
                    break;
            }
            if (bIsUpdated)
            {
                bNeedsUpdate = false;
            }
            
            return bNeedsUpdate;
        }
        public static string MakeStandardEditName(string uriPattern, string attName,
            string dataType, string size)
        {
            //attributes in all stylesheets use this pattern for edits
            string[] arrEditName = { uriPattern, Helpers.GeneralHelpers.STRING_DELIMITER, attName, Helpers.GeneralHelpers.STRING_DELIMITER, 
                                   dataType, Helpers.GeneralHelpers.STRING_DELIMITER, size};
            string sEditName = Helpers.GeneralHelpers.MakeString(arrEditName);
            return sEditName;
        }
        public static string MakeStandardDeleteName(string uriPattern)
        {
            //nodes in all stylesheets use this pattern for deletes
            string sDeleteName = string.Concat(uriPattern, Helpers.GeneralHelpers.STRING_DELIMITER, 
                Data.EditHelpers.EditHelper.DELETE);
            return sDeleteName;
        }
        public static void GetStandardEditNameParams(string[] arrEditName, 
            out string uriPattern, out string attName, out string dataType, 
            out string size)
        {
            uriPattern = string.Empty;
            attName = string.Empty;
            dataType = string.Empty;
            size = string.Empty;
            //attributes in all stylesheets use this pattern for edits
            if (arrEditName != null)
            {
                uriPattern = arrEditName[0];
                if (arrEditName.Length >= 2) attName = arrEditName[1];
                if (arrEditName.Length >= 3) dataType = arrEditName[2];
                if (arrEditName.Length >= 4) size = arrEditName[3];
                if (size.StartsWith("step"))
                    size = string.Empty;
            }
        }
        
        public static void SaveEdits(XElement updatedDoc, ContentURI uri)
        {
            //make sure the file and directory were not deleted
            if (Helpers.FileStorageIO.DirectoryCreate(uri,
                uri.URIClub.ClubDocFullPath))
            {
                string sErrorMsg = string.Empty;
                Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
                fileStorageIO.SaveXmlInURI(uri,
                    updatedDoc.CreateReader(), uri.URIClub.ClubDocFullPath,
                    out sErrorMsg);
                uri.ErrorMessage += sErrorMsg;
            }
        }
        public static void SaveEdits(XmlDocument updatedDoc, ContentURI uri)
        {
            string sErrorMsg = string.Empty;
            Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
            fileStorageIO.SaveXmlInURI(uri, 
                updatedDoc, uri.URIClub.ClubDocFullPath,
                out sErrorMsg);
            uri.ErrorMessage += sErrorMsg;
        }
        //160 should make this obsolete
        public static void DeleteEditDocs(ContentURI uri)
        {
            //forces a new db doc to be generated on next visit (i.e. after selects, defaultadds)
            Helpers.FileStorageIO.DeleteURI(uri, uri.URIClub.ClubDocFullPath);
            if (uri.URIClub.ClubDocFullPath.Equals(uri.URIMember.MemberDocFullPath) == false)
            {
                Helpers.FileStorageIO.DeleteURI(uri, uri.URIMember.MemberDocFullPath);
            }
        }
        public static string GetDbKeyName(string uriPattern, string attName,
            string dataType, string size)
        {
            string sDbKeyName = string.Concat(uriPattern,
                Helpers.GeneralHelpers.STRING_DELIMITER, attName, Helpers.GeneralHelpers.STRING_DELIMITER,
                dataType, Helpers.GeneralHelpers.STRING_DELIMITER, size);
            return sDbKeyName;
        }
        public static string GetDbKeyNameForAddInFromUpdates(string uriPattern, string attName,
            string dataType, IDictionary<string, string> updates)
        {
            string sDbStartKeyName = GetDbKeyNameForAddIn(uriPattern, 
                attName, dataType, string.Empty);
            //when not sure of the step delimiter, use the starting string
            foreach (KeyValuePair<string, string> kvp in updates)
            {
                if (kvp.Key.StartsWith(sDbStartKeyName))
                {
                    sDbStartKeyName = kvp.Key;
                    break;
                }
            }
            return sDbStartKeyName;
        }
        public static string GetDbKeyNameForAddIn(string uriPattern, string attName,
            string dataType, string stepNumber)
        {
            string sDbKeyName = string.Empty;
            if (!string.IsNullOrEmpty(stepNumber))
            {
                //stepnumber is used to get rid of updates members that come from
                //running the same step more than once
                sDbKeyName = string.Concat(uriPattern,
                    Helpers.GeneralHelpers.STRING_DELIMITER, attName, Helpers.GeneralHelpers.STRING_DELIMITER,
                    dataType, Helpers.GeneralHelpers.STRING_DELIMITER, stepNumber);
            }
            else
            {
                sDbKeyName = string.Concat(uriPattern,
                    Helpers.GeneralHelpers.STRING_DELIMITER, attName, Helpers.GeneralHelpers.STRING_DELIMITER,
                    dataType);
            }
            return sDbKeyName;
        }
        public static void DeleteSubDirectoriesAndResource(ContentURI uri, 
            StringDictionary colDeletes)
        {
            if (colDeletes != null)
            {
                //delete the corresponding filesystem files to prevent 
                //them from being orphans
                //orphans have the potential to interfere with filesystem-based analyses
                string sAction = string.Empty;
                string sURcIdeletedURIPattern = string.Empty;
                foreach (string sKeyName in colDeletes.Keys)
                {
                    //1. sKeyName is a 'searchname;delete' delimited string
                    String[] arrDeleteParams = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                    if (arrDeleteParams.Length > 1)
                    {
                        sAction = string.Empty;
                        sAction = arrDeleteParams[1];
                        if (sAction.ToLower().Equals(DELETE))
                        {
                            sURcIdeletedURIPattern = string.Empty;
                            sURcIdeletedURIPattern = arrDeleteParams[0];
                            DeleteSubDirectoryAndResource(uri, sURcIdeletedURIPattern);
                        }
                    }
                }
            }
        }
        private static void DeleteSubDirectoryAndResource(ContentURI uri,
            string uriDeletedPattern)
        {
            //argumentsEdits.URIToAdd.URIPattern contains the 
            //standard subdirectory naming convention: node_id
            //it has to be either an ancestor or descendent of
            //uri.uriclub.docpath
            //check to see if it is an ancestor or self:
            string sDeletedNodeName 
                = ContentURI.GetURIPatternPart(uriDeletedPattern, ContentURI.URIPATTERNPART.node);
            string sDeletedNodeId
                = ContentURI.GetURIPatternPart(uriDeletedPattern, ContentURI.URIPATTERNPART.id);
            string sDirectoryToDelete
                = Helpers.AppSettings.GetStandardFileDirectoryName(
                    sDeletedNodeName, sDeletedNodeId);
            DeleteSubDirectory(uri, uriDeletedPattern, sDirectoryToDelete);
            //note: apptype resources deletes the resources in repository.deletecollection()
        }
        private static void DeleteSubDirectory(ContentURI uri,
            string uriDeletedPattern, string directoryToDelete)
        {   
            //convert the directoryname to a directory path
            if (uri.URIClub.ClubDocFullPath.Contains(directoryToDelete))
            {
                bool bNeedsNameOnly = false;
                directoryToDelete = Helpers.GeneralHelpers.GetDirectoryPathOrName(
                    directoryToDelete, uri.URIClub.ClubDocFullPath, bNeedsNameOnly);
            }
            else
            {
                directoryToDelete = GetDescendentDirectory(uri,
                    directoryToDelete, uri.URIClub.ClubDocFullPath);
            }
            if (!string.IsNullOrEmpty(directoryToDelete))
            {
                //hierarchical data means subdirs must always be deleted (db cascading deletes)
                bool bIncludeSubDirs = true;
                bool bIsDeleted = Helpers.FileStorageIO.DeleteDirectory(uri,
                    directoryToDelete, bIncludeSubDirs);
            }
           
        }
        private static string GetDescendentDirectory(ContentURI uri,
            string descendentDirectoryName, string docPath)
        {
            string sDescDirToDelete = string.Empty;
            if (Helpers.FileStorageIO.DirectoryExists(uri, docPath))
            {
                sDescDirToDelete 
                    = Helpers.FileStorageIO.GetDescendentDirectory(uri,
                        docPath, descendentDirectoryName);
            }
            return sDescDirToDelete;
        }
       
        
        public static bool UpdateNewFileNames(ContentURI uri,
            IDictionary<string, string> updates, XElement devTrekLinqRoot)
        {
            //if uri.uriname changes, so to will the associated filename
            //before saving the file, switch to the new file name
            //move or delete the old files to prevent them from being orphans
            //orphans have the potential to interfere with filesystem-based analyses
            ContentURI updatedURI = new ContentURI();
            string sOldName = string.Empty;
            bool bNeedsNewFileNames = false;
            string[] arrUpdateParams = null;
            string sOldURIPattern = string.Empty;
            string sAttName = string.Empty;
            string sDataType = string.Empty;
            string sSize = string.Empty;
            string sNewName = string.Empty;
            foreach (string sKeyName in updates.Keys.AsParallel())
            {
                arrUpdateParams = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                if (arrUpdateParams != null)
                {
                    GetStandardEditNameParams(arrUpdateParams, out sOldURIPattern,
                        out sAttName, out sDataType, out sSize);
                    if (sAttName == AppHelpers.Calculator.cName)
                    {
                        updatedURI = ContentURI.ConvertShortURIPattern(sOldURIPattern, uri);
                        //names have been changed and filenames need to be changed
                        sOldName = updatedURI.URIName;
                        sNewName = updates[sKeyName];
                        if (sNewName != sOldName
                            && (!string.IsNullOrEmpty(sNewName)))
                        {
                            updatedURI.URIName = sNewName;
                            updatedURI.URIPattern = Helpers.GeneralHelpers.MakeURIPattern(
                                updatedURI.URIName, updatedURI.URIId.ToString(), 
                                updatedURI.URINetworkPartName, updatedURI.URINodeName, 
                                updatedURI.URIFileExtensionType);
                            //uri is built using db names that haven't been updated yet
                            //update to latest names available and delete old files (no orphans)
                            UpdateNamesAndPaths(uri, updatedURI, sOldName);
                        }
                    }
                }
            }
            return bNeedsNewFileNames;
        }
        private static void UpdateNamesAndPaths(ContentURI uri,
            ContentURI updatedURI, string oldName)
        {
            if (uri.URIClub.ClubDocFullPath != string.Empty)
            {
                //the subfolder holding the old file will be:
                string sUpdatedDirectory = Helpers.AppSettings.GetStandardFileDirectoryName(
                    updatedURI.URINodeName, updatedURI.URIId.ToString());
                //search for the directory using uri.URIClub.ClubDocFullPath
                bool bNeedsNameOnly = false;
                string sUpdateDirectoryPath = Helpers.GeneralHelpers.GetDirectoryPathOrName(
                    sUpdatedDirectory, uri.URIClub.ClubDocFullPath, bNeedsNameOnly);
                if (string.IsNullOrEmpty(sUpdateDirectoryPath))
                {
                    sUpdateDirectoryPath = GetDescendentDirectory(uri,
                        sUpdatedDirectory, uri.URIClub.ClubDocFullPath);
                }
                if (Helpers.FileStorageIO.DirectoryExists(uri,
                    sUpdateDirectoryPath))
                {
                    updatedURI.URIClub.ClubDocFullPath = sUpdateDirectoryPath;
                    string sNewFileName 
                        = Helpers.ContentHelper.MakeStandardFileNameFromURIPattern(
                        updatedURI);
                    string sNewURIName
                        = Helpers.GeneralHelpers.GetSubstringFromFront(
                            sNewFileName, Helpers.GeneralHelpers.FILENAME_DELIMITERS,
                            1);
                    string sOldFileName = string.Concat(sNewFileName.Replace(
                        sNewURIName, oldName), Helpers.GeneralHelpers.EXTENSION_XML);
                    string sOldFilePath = string.Concat(sUpdateDirectoryPath,
                        Helpers.GeneralHelpers.FILE_PATH_DELIMITER, sOldFileName);
                    if (Helpers.FileStorageIO.URIAbsoluteExists(uri,
                        sOldFilePath))
                    {
                        string sNewFilePath = string.Concat(sUpdateDirectoryPath,
                            Helpers.GeneralHelpers.FILE_PATH_DELIMITER, 
                            sNewFileName, Helpers.GeneralHelpers.EXTENSION_XML);
                        Helpers.FileStorageIO.MoveURIs(uri,
                            sOldFilePath, sNewFilePath);
                        Helpers.FileStorageIO.DeleteURIsWithChangedNames(uri,
                            sNewFilePath,
                            sOldFileName);
                        if (uri.URIClub.ClubDocFullPath
                            == sOldFilePath)
                        {
                            uri.URIClub.ClubDocFullPath = sNewFilePath;
                            //160 deprecated separate file storage for guests
                            //guest path
                            uri.URIMember.MemberDocFullPath = uri.URIClub.ClubDocFullPath;
                        }
                    }
                }
            }
        }
        /// <summary>
        ///Purpose:		Organize the arguments used to edit documents.
        ///Name:		ArgumentsEdits.cs
        ///Author:		www.devtreks.org
        ///Date:		2010, October
        ///Reference:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
        /// </summary>
        public class ArgumentsEdits
        {
            public ArgumentsEdits()
            {
                this.URIToEdit = new ContentURI();
                this.URIToAdd = new ContentURI();
                this.URIToAddOriginalURIPattern = string.Empty;
                this.IsAncestorBeingAdded = false;
                this.EditAttName = string.Empty;
                this.EditAttValue = string.Empty;
                this.EditDataType = string.Empty;
                this.EditSize = string.Empty;
                this.ParentURIPattern = string.Empty;
                this.ParentQry = string.Empty;
                
                this.DevTrekSchemaName = string.Empty;
                this.DevTrekNamespace = string.Empty;
                this.DevTrekNodeName = string.Empty;
                this.DevTrekNodeId = string.Empty;

                this.NeedsBaseIds = false;
                this.IsDbEdit = false;
                this.NeedsFullSchema = false;
                this.IsUpdateGram = false;
                this.ReturnIds = string.Empty;
                this.SelectionOption = AddHelperLinq.SELECTION_OPTIONS.none;
                this.NumberToAdd = "1";
                this.SelectionsToAdd = new List<ContentURI>();
                //updategram management
                this.EditRootElement = null;
                this.EditWriter = null;

                this.RndGenerator = null;
                this.IsCustomNodeBeingAdded = false;

                this.ErrorMessage = string.Empty;
            }
            public ArgumentsEdits(ArgumentsEdits edits)
            {
                this.URIToEdit = new ContentURI(edits.URIToEdit);
                this.URIToAdd = new ContentURI(edits.URIToAdd);
                this.URIToAddOriginalURIPattern = edits.URIToAddOriginalURIPattern;
                this.IsAncestorBeingAdded = edits.IsAncestorBeingAdded;
                this.EditAttName = edits.EditAttName;
                this.EditAttValue = edits.EditAttValue;
                this.EditDataType = edits.EditDataType;
                this.EditSize = edits.EditSize;
                this.ParentURIPattern = edits.ParentURIPattern;
                this.ParentQry = edits.ParentQry;

                this.DevTrekSchemaName = edits.DevTrekSchemaName;
                this.DevTrekNamespace = edits.DevTrekNamespace;
                this.DevTrekNodeName = edits.DevTrekNodeName;
                this.DevTrekNodeId = edits.DevTrekNodeId;

                this.NeedsBaseIds = edits.NeedsBaseIds;
                this.IsDbEdit = edits.IsDbEdit;
                this.NeedsFullSchema = edits.NeedsFullSchema;
                this.IsUpdateGram = edits.IsUpdateGram;
                this.ReturnIds = edits.ReturnIds;
                this.SelectionOption = edits.SelectionOption;
                this.NumberToAdd = edits.NumberToAdd;
                this.SelectionsToAdd = Helpers.LinqHelpers.CopyContentURIs(edits.SelectionsToAdd).ToList();
                //updategram management
                this.EditRootElement = (edits.EditRootElement != null) ?
                    new XElement(edits.EditRootElement) : null;
                this.EditWriter = null;

                this.RndGenerator = edits.RndGenerator;
                this.IsCustomNodeBeingAdded = edits.IsCustomNodeBeingAdded;

                this.ErrorMessage = edits.ErrorMessage;
            }
            public ContentURI URIToEdit{ get; set; }
            public ContentURI URIToAdd{ get; set; }
            public string URIToAddOriginalURIPattern { get; set; }
            public bool IsAncestorBeingAdded { get; set; }
            public string EditAttName{ get; set; }
            public string EditAttValue{ get; set; }
            public string EditDataType{ get; set; }
            public string EditSize{ get; set; }
            public string ParentURIPattern { get; set; }
            public string ParentQry { get; set; }
            
            public string DevTrekSchemaName{ get; set; }
            public string DevTrekNamespace{ get; set; }
            public string DevTrekNodeName{ get; set; }
            public string DevTrekNodeId{ get; set; }

            public bool NeedsBaseIds{ get; set; }
            public bool IsDbEdit{ get; set; }
            public bool NeedsFullSchema{ get; set; }
            public bool IsUpdateGram{ get; set; }
            public string ReturnIds { get; set; }
            public AddHelperLinq.SELECTION_OPTIONS SelectionOption { get; set; }
            public string NumberToAdd { get; set; }
            public List<ContentURI> SelectionsToAdd { get; set; }

            //updategram management
            public XElement EditRootElement { get; set; }
            public XmlWriter EditWriter { get; set; }

            //random number generator 
            //(sameids can't be reliably randomized with new Randoms)
            public Random RndGenerator  { get; set; }
            //customdoc node insertions
            public bool IsCustomNodeBeingAdded { get; set; }

            public string ErrorMessage{ get; set; }
        }
    }
}
