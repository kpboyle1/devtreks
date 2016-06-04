using DevTreks.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.AppHelpers
{
    /// <summary>
    ///Purpose:		Support class holding constants, enums, and common methods 
    ///             for devpacks (custom and virtual uris)
    ///Author:		www.devtreks.org
    ///Date:		2016, March
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///NOTE :   1. This class is used to either hold custom documents, such as budgets,
    ///         or, in the case of virtual uris, uri addresses and edits to the virtual uri.
    /// </summary>
    public static class DevPacks
    {
        //base table schema (default insertions prior to join table insertion)
        public const string DEVPACKBASEEDIT_SCHEMA = "DevPackBaseEdits.xml";
        //join table edits schema (join table insertions and edits)
        public const string DEVPACKJOINEDIT_SCHEMA = "DevPackJoinEdits.xml";
        //attribute defining a recursive node's parentid (devpacks are recursive to support custom docs)
        private const string DEVPACKGROUPID = "DevPackGroupId";
       
        //refactor: won't be used in version 1.0
        //tells DevPackFull.xslt to show init instructions
        private const string DEVPACK_START_XML = "<root><devpackstart></devpackstart></root>";
        //xml elements found in join table schemas (i.e. servicebase in all apps)
        public enum DEVPACKS_TYPES
        {
            servicebase                 = 0,
            devpacktype                 = 1,
            devpackgroup                = 2,
            devpack                     = 3,
            devpackpart                 = 4,
            devpackresourcepack         = 5
        }
        public enum DEVPACKS_BASE_TYPES
        {
            devpackbase         = 0,
            devpackpartbase     = 1
        }
        public static Dictionary<string, string> GetDevPackType(ContentURI uri)
        {
            Dictionary<string, string> colTypes = new Dictionary<string, string>();
            if (uri.URIModels.DevPackType != null)
            {
                foreach (var type in uri.URIModels.DevPackType)
                {
                    //note that on the client the key becomes the option's value
                    colTypes.Add(type.PKId.ToString(), type.Name);
                }
            }
            return colTypes;
        }
        public static void SetAppSearchView(string currentNodeName,
            int currentId, ContentURI uri)
        {
            //tells what to display and how to display it
            if (uri.URIMember.ClubInUse.PrivateAuthorizationLevel == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
            {
                uri.URIDataManager.EditViewEditType = Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
                //selections for node insertions
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
            if (currentNodeName == DEVPACKS_TYPES.devpackresourcepack.ToString())
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
                uri.URIDataManager.ChildrenNodeName = DEVPACKS_TYPES.devpackgroup.ToString();
            }
            else if (currentNodeName == DEVPACKS_TYPES.devpackgroup.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = DEVPACKS_TYPES.devpack.ToString();
            }
            else if (currentNodeName == DEVPACKS_TYPES.devpack.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = DEVPACKS_TYPES.devpackpart.ToString();
            }
            else if (currentNodeName == DEVPACKS_TYPES.devpackpart.ToString())
            {
                uri.URIDataManager.ChildrenNodeName = DEVPACKS_TYPES.devpackresourcepack.ToString();
            }
            else
            {
                uri.URIDataManager.ChildrenNodeName = string.Empty;
            }
        }
        public static void AddBaseDevPackToXml(XElement devpackClassToDevPack,
            DevPack devPack)
        {
            if (devPack != null)
            {
                devpackClassToDevPack.SetAttributeValue("DevPackClassAndPackName", devPack.DevPackName);
                devpackClassToDevPack.SetAttributeValue("DevPackDocStatus", devPack.DevPackDocStatus);
                devpackClassToDevPack.SetAttributeValue("DevPackNum", devPack.DevPackNum);
                devpackClassToDevPack.SetAttributeValue("DevPackLastChangedDate", devPack.DevPackLastChangedDate);
                devpackClassToDevPack.SetAttributeValue("DevPackDesc", devPack.DevPackDesc);
                devpackClassToDevPack.SetAttributeValue("DevPackKeywords", devPack.DevPackKeywords);
                EditHelpers.EditModelHelper.AddXmlAttributeToDoc(devPack.DevPackMetaDataXml, devpackClassToDevPack);
            }
        }
        public static void AddBaseDevPackToXml(XElement devpackToDevPackPart,
            DevPackPart devPackPart)
        {
            if (devPackPart != null)
            {
                devpackToDevPackPart.SetAttributeValue("DevPackClassAndPackName", devPackPart.DevPackPartName);
                devpackToDevPackPart.SetAttributeValue("DevPackDocStatus", devPackPart.DevPackPartNum);
                devpackToDevPackPart.SetAttributeValue("DevPackNum", devPackPart.DevPackPartLastChangedDate);
                devpackToDevPackPart.SetAttributeValue("DevPackDesc", devPackPart.DevPackPartDesc);
                devpackToDevPackPart.SetAttributeValue("DevPackKeywords", devPackPart.DevPackPartKeywords);
                devpackToDevPackPart.SetAttributeValue("DevPackLastChangedDate", devPackPart.DevPackPartFileName);
                //don't include the xmldoc in the base doc
                //EditHelpers.EditModelHelper.AddXmlAttributeToDoc(devPackPart.DevPackPartXmlDoc, devpackToDevPackPart);
            }
        }
        public static void AddBaseDevPackToXml(XElement devpackPartToResourcePack,
           ResourcePack resourcePack)
        {
            if (resourcePack != null)
            {
                devpackPartToResourcePack.SetAttributeValue("ResourcePackName", resourcePack.ResourcePackName);
                devpackPartToResourcePack.SetAttributeValue("ResourcePackDesc", resourcePack.ResourcePackDesc);
            }
        }
        
        public static void GetUpdateDevPackBaseQueryParams(ContentURI uri, 
            bool isMetaData, bool isJoinId,
            out string qryName, out string attName)
        {
            qryName = "0UpdateDevPackBaseXml";
            attName = string.Empty;
            if (uri.URINodeName == DEVPACKS_BASE_TYPES.devpackbase.ToString())
            {
                if (isMetaData == true)
                {
                    attName = "DevPackMetaDataXml";
                }
            }
            else if (uri.URINodeName == DEVPACKS_BASE_TYPES.devpackpartbase.ToString())
            {
                attName = "DevPackPartXmlDoc";
                if (isJoinId)
                {
                    qryName = "0UpdateDevPackBaseXmlFromJoinId";
                }
            }
        }
        public static void GetUpdateDevPackJoinQueryParams(ContentURI uri,
            out string qryName)
        {
            qryName = "0UpdateDevPackJoinXml";
        }
        public static string GetDevPackJoinQueryName(ContentURI uri)
        {
            string sQryName = string.Empty;
            if (uri.URINodeName == DEVPACKS_TYPES.devpackpart.ToString()
                || uri.URINodeName == DEVPACKS_TYPES.devpack.ToString())
            {
                //get devpacktodevpackpart.xmldoc field
                //holding instruction nodes telling what to do with associated linked view id base
                sQryName = "0GetDevPackJoinXml";
            }
            return sQryName;
        }
        public static void GetDevPackBaseQueryName(ContentURI uri, bool isMetaData,
            out string qryName, out string attName)
        {
            qryName = string.Empty;
            attName = string.Empty;
            //return metadata as part of the schema-generated xmldoc
            if (uri.URINodeName == DEVPACKS_TYPES.devpackpart.ToString())
            {
                //get devpackpart.xmldoc field
                //holding instruction nodes telling what to do with associated linkedview base
                qryName = "0GetDevPackBaseXml";
                if (isMetaData == true)
                {
                    attName = "DevPackPartMetaDataXml";
                }
            }
        }
        public static void GetChildForeignKeyName(string parentNodeName,
            out string childForeignKeyName, out string baseForeignKeyName)
        {
            childForeignKeyName = string.Empty;
            baseForeignKeyName = string.Empty;
            if (parentNodeName == Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
            {
                childForeignKeyName = Agreement.SERVICE_ID;
            }
            else if (parentNodeName == DevPacks.DEVPACKS_TYPES.devpackgroup.ToString())
            {
                childForeignKeyName = "DevPackGroupId";
            }
            else if (parentNodeName == DevPacks.DEVPACKS_TYPES.devpack.ToString())
            {
                childForeignKeyName = "DevPackClassToDevPackId";
                //many to many table joins to another many to many table
                baseForeignKeyName = "DevPackId";
            }
            else if (parentNodeName == DevPacks.DEVPACKS_TYPES.devpackpart.ToString())
            {
                childForeignKeyName = "DevPackToDevPackPartId";
                //many to many table joins to another many to many table
                baseForeignKeyName = "DevPackPartId";
            }
            else if (parentNodeName == DevPacks.DEVPACKS_TYPES.devpackresourcepack.ToString())
            {
                //shouldn't be using resources app
            }
        }
        public static void GetParentOfRecursiveNodesId(
            EditHelpers.EditHelper.ArgumentsEdits addsArguments,
            out string recursiveParentKeyName, out string recursiveParentId)
        {
            recursiveParentKeyName = string.Empty;
            recursiveParentId = string.Empty;
            if (addsArguments.URIToEdit.URIDataManager.Ancestors != null)
            {
                ContentURI groupURI = Helpers.LinqHelpers.GetContentURIByNodeName(
                    addsArguments.URIToEdit.URIDataManager.Ancestors, 
                    DEVPACKS_TYPES.devpackgroup.ToString());
                if (groupURI != null)
                {
                    recursiveParentKeyName = DEVPACKGROUPID;
                    recursiveParentId = groupURI.URIId.ToString();
                }
            }
        }
        public static async Task<string> GetDevPackBaseIdFromJoinIdAsync(
            ContentURI uri, string connect, string currentNodeName, string currentId)
        {
            string sBaseTableId = string.Empty;
            Helpers.SqlIOAsync sqlIO = new Helpers.SqlIOAsync(uri);
            SqlParameter[] colPrams = 
			{ 
				sqlIO.MakeInParam("@Id",                    SqlDbType.Int, 4, currentId),
				sqlIO.MakeInParam("@NodeName",              SqlDbType.NVarChar, 25, currentNodeName),
                sqlIO.MakeOutParam("@BaseTableId",          SqlDbType.Int, 4)
			};
            string sQryName = "0GetDevPackBaseTableId";
            int iNotUsed = await sqlIO.RunProcIntAsync(sQryName, colPrams);
            if (colPrams[2].Value != System.DBNull.Value)
            {
                sBaseTableId = colPrams[2].Value.ToString();
            }
            sqlIO.Dispose();
            return sBaseTableId;
        }

        public static string GetDeleteBaseandJoinQry()
        {
            return "0DeleteDevPacks";
        }
    }
}
