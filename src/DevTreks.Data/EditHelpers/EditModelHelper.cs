using DevTreks.Data.RuleHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
//using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DevTreks.Data.EditHelpers
{
    /// <summary>
    ///Purpose:		CLR object editing, and model deserialization, class
    ///Author:		www.devtreks.org
    ///Date:		2016, May    
    ///References:	
    /// </summary>
    public class EditModelHelper
    {
        public bool MakeDeletionCollection(ContentURI uri,
            EditHelper.ArgumentsEdits argumentsEdits, StringDictionary colDeletes)
        {
            bool bIsOkToSave = false;
            //a delete query is run separately for each deletion, but why?
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
                        if (sAction.ToLower().Equals(EditHelper.DELETE))
                        {
                            ContentURI deleteURI = new ContentURI();
                            //convention: argumentsEdits.SelectionsToAdd is each node being inserted or deleted
                            //URIToEdit is base node (node getting insertions or node holding document)
                            deleteURI.URIPattern = string.Empty;
                            deleteURI.URIPattern = arrDeleteParams[0];
                            Helpers.GeneralHelpers.SetURIParams(deleteURI);
                            deleteURI.URIDataManager = new ContentURI.DataManager(uri.URIDataManager);
                            if (argumentsEdits.SelectionsToAdd == null)
                                argumentsEdits.SelectionsToAdd = new List<ContentURI>();
                            argumentsEdits.SelectionsToAdd.Add(deleteURI);
                        }
                    }
                }
            }
            if (uri.ErrorMessage == string.Empty
                && argumentsEdits.SelectionsToAdd != null)
            {
                if (argumentsEdits.SelectionsToAdd.Count > 0)
                {
                    bIsOkToSave = true;
                }
            }
            return bIsOkToSave;
        }
        public bool MakeEditsCollection(ContentURI uri, EditHelper.ArgumentsEdits argumentsEdits,
            List<EditHelper.ArgumentsEdits> edits, IDictionary<string, string> colUpdates)
        {
            bool bIsOkToSave = false;
            if (colUpdates != null)
            {
                string sURIPattern = string.Empty;
                string sAttName = string.Empty;
                string sDataType = string.Empty;
                string sSize = string.Empty;
                EditHelper editHelper = new EditHelper();
                //added to argEdits.SelectionsToAdd collection so that the objects needing to be updated are stateful
                foreach (string sKeyName in colUpdates.Keys)
                {
                    //1. sKeyName should be a 'uripattern;attname;datatype;size' delimited string
                    string[] arrUpdateParams
                        = sKeyName.Split(Helpers.GeneralHelpers.STRING_DELIMITERS);
                    argumentsEdits.URIToAdd.URIPattern = string.Empty;
                    argumentsEdits.EditAttName = string.Empty;
                    argumentsEdits.EditAttValue = string.Empty;
                    EditHelper.GetStandardEditNameParams(arrUpdateParams,
                        out sURIPattern, out sAttName, out sDataType, out sSize);
                    if (!string.IsNullOrEmpty(sAttName))
                    {
                        //convention: URIToAdd is each node being inserted or deleted
                        //URIToAdd is base node (node getting insertions, node holding document)
                        EditHelper.ArgumentsEdits newEdit 
                            = new EditHelper.ArgumentsEdits(argumentsEdits);
                        newEdit.URIToAdd.URIPattern = sURIPattern;
                        Helpers.GeneralHelpers.SetURIParams(newEdit.URIToAdd);
                        newEdit.URIToAdd.URIDataManager 
                            = new ContentURI.DataManager(uri.URIDataManager);
                        newEdit.EditAttName = sAttName;
                        newEdit.EditAttValue = colUpdates[sKeyName].ToString();
                        newEdit.EditDataType = sDataType;
                        newEdit.EditSize = sSize;
                        //update the edits collection
                        edits.Add(newEdit);
                        bIsOkToSave = true;
                        sURIPattern = string.Empty;
                        sAttName = string.Empty;
                        sDataType = string.Empty;
                        sSize = string.Empty;
                    }
                    else
                    {
                        uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "UPcDate_BADPROPERTY");
                    }
                }
            }
            return bIsOkToSave;
        }
        
        public static void UpdateDbEntityProperty(EntityEntry entry, string propertyName,
            string propertyValue, string dataType, ref string errorMsg)
        {
            //entry.CurrentValues[propertyName] can have null value for LinkingXmlDoc
            if (propertyValue != string.Empty)
            {
                //GeneralRule datatypes are all lower case
                if (dataType.ToLower() == GeneralRules.BOOLEAN)
                {
                    bool bValue = Helpers.GeneralHelpers.ConvertStringToBool(propertyValue);
                    //entry.CurrentValues[propertyName] = dtValue;
                    entry.Property(propertyName).CurrentValue = bValue;
                }
                else if (dataType.ToLower() == GeneralRules.BINARY)
                {
                    //binaries get uploaded using the file upload utility
                }
                else if (dataType.ToLower() == GeneralRules.DATE)
                {
                    DateTime dtValue = Helpers.GeneralHelpers.ConvertStringToDate(propertyValue);
                    entry.Property(propertyName).CurrentValue = dtValue;
                }
                else if (dataType.ToLower() == GeneralRules.SHORTDATE)
                {
                    DateTime dtValue = Helpers.GeneralHelpers.ConvertStringToShortDate(propertyValue);
                    entry.Property(propertyName).CurrentValue = dtValue;
                }
                else if (dataType.ToLower() == GeneralRules.DECIMAL)
                {
                    decimal dValue = Helpers.GeneralHelpers.ConvertStringToDecimal(propertyValue);
                    entry.Property(propertyName).CurrentValue = dValue;
                }
                else if (dataType.ToLower() == GeneralRules.FLOAT)
                {
                    float fValue = Helpers.GeneralHelpers.ConvertStringToFloat(propertyValue);
                    entry.Property(propertyName).CurrentValue = fValue;
                }
                else if (dataType.ToLower() == GeneralRules.DOUBLE)
                {
                    double dblValue = Helpers.GeneralHelpers.ConvertStringToDouble(propertyValue);
                    entry.Property(propertyName).CurrentValue = dblValue;
                }
                else if (dataType.ToLower() == GeneralRules.INTEGER)
                {
                    int iValue = Helpers.GeneralHelpers.ConvertStringToInt(propertyValue);
                    entry.Property(propertyName).CurrentValue = iValue;
                }
                else if (dataType.ToLower() == GeneralRules.LONG)
                {
                    long lValue = Helpers.GeneralHelpers.ConvertStringToLong(propertyValue);
                    entry.Property(propertyName).CurrentValue = lValue;
                }
                else if (dataType.ToLower() == GeneralRules.SHORTINTEGER)
                {
                    short sValue = Helpers.GeneralHelpers.ConvertStringToShort(propertyValue);
                    entry.Property(propertyName).CurrentValue = sValue;
                }
                else if (dataType.ToLower() == GeneralRules.XML)
                {
                    //treated by ef as a string
                    entry.Property(propertyName).CurrentValue = propertyValue;
                }
                else
                {
                    //string is default
                    entry.Property(propertyName).CurrentValue = propertyValue;
                }
            }
            else
            {
                errorMsg +=
                    DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Concat("Property = ", propertyValue), "UPcDate_BADPROPERTY");
            }
        }
        public static Helpers.GeneralHelpers.SUBAPPLICATION_TYPES GetSubAppTypeForEdit(ContentURI uri)
        {
            Helpers.GeneralHelpers.SUBAPPLICATION_TYPES apptype = uri.URIDataManager.SubAppType;
            //category edits have an apptype of agreement, but categories need ServiceClassId to find app for edits
            if (uri.URINodeName == AppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString()
                && uri.URIDataManager.SubActionView 
                == Helpers.GeneralHelpers.SUBACTION_VIEWS.categories.ToString())
            {
                if (uri.URIService != null)
                {
                    if (uri.URIService.Service != null)
                    {
                        ContentURI tempuri = new ContentURI();
                        Helpers.GeneralHelpers.SetAppTypes(uri.URIService.Service.ServiceClassId, tempuri);
                        apptype = tempuri.URIDataManager.SubAppType;
                    }

                }
            }
            return apptype;
        }
        public static void SetAttributes(
           Dictionary<string, string> propValues, XElement node)
        {
            string sPropertyName = string.Empty;
            string sValue = string.Empty;
            string sId = SetIdAttribute(propValues, node);
            if (!string.IsNullOrEmpty(sId))
            {
                foreach (var property in propValues)
                {
                    sPropertyName = property.Key;
                    sValue = property.Value;
                    if (sPropertyName != AppHelpers.Calculator.cPKId)
                    {
                        bool bIsXmlProperty = IsXmlProperty(sPropertyName);
                        if ((!bIsXmlProperty)
                            && (!IsBinaryProperty(sPropertyName)))
                        {
                            node.SetAttributeValue(sPropertyName, sValue);
                        }
                        else
                        {
                            if (bIsXmlProperty)
                            {
                                AddXmlAttributeToDoc(sValue, node);
                            }
                        }
                    }
                }
            }
        }
        public static string SetIdAttribute(
            Dictionary<string, string> propValues, XElement node)
        {
            string sId = string.Empty;
            propValues.TryGetValue(AppHelpers.Calculator.cPKId, out sId);
            if (!string.IsNullOrEmpty(sId))
            {
                node.SetAttributeValue(AppHelpers.Calculator.cId, sId);
            }
            return sId;
        }
        
        public static bool IsXmlProperty(string propertyName)
        {
            bool bIsXmlProperty = false;
            if (propertyName.ToLower().Contains(AppHelpers.Resources.FILEEXTENSION_TYPES.xml.ToString().ToLower()))
            {
                bIsXmlProperty = true;
            }
            return bIsXmlProperty;
        }
        public static bool IsBinaryProperty(string propertyName)
        {
            bool bIsBinaryProperty = false;
            if (propertyName.ToLower().Contains(Helpers.GeneralHelpers.BINARY.ToLower()))
            {
                bIsBinaryProperty = true;
            }
            return bIsBinaryProperty;
        }
       
        public static void AddXmlAttributeToDoc(string xmldoc, XElement el)
        {
            //all linked view get added as childrent of current element
            //handle basexmldocs that are atts in the basedoc calls, not here
            if (!string.IsNullOrEmpty(xmldoc))
            {
                //ef allows xml to be strings, so filter out strings
                if (xmldoc != Helpers.GeneralHelpers.NONE)
                {
                    if (xmldoc.StartsWith(Helpers.GeneralHelpers.ROOT_START_NODE)
                        || xmldoc.StartsWith(Helpers.GeneralHelpers.METADATA_ELEMENT_STARTNAME))
                    {
                        XElement linkedView = XElement.Parse(xmldoc);
                        if (linkedView != null)
                        {
                            //lvs must has a root node or a metadata element
                            if (linkedView.Name.LocalName == Helpers.GeneralHelpers.ROOT_PATH
                                || linkedView.Name.LocalName == Helpers.GeneralHelpers.METADATA_ELEMENT_NAME)
                            {
                                //and they must contain content
                                if (linkedView.HasElements)
                                {
                                    el.Add(linkedView);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static async Task<bool> SaveStandardContentXmlDocAsync(ContentURI uri, XElement root)
        {
            //asynch because base xmldoc is not used by calling methods
            bool bHasGoodXml = false;
            if ((!string.IsNullOrEmpty(uri.URIClub.ClubDocFullPath)))
            {
                if (root != null)
                {
                    //standard content rules
                    if (root.Name == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        if (root.HasElements)
                        {
                            string sErrorMsg = string.Empty;
                            Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
                            sErrorMsg = await fileStorageIO.SaveXmlInURIAsync(uri,
                                root.CreateReader(), uri.URIClub.ClubDocFullPath);
                            if (string.IsNullOrEmpty(sErrorMsg))
                            {
                                bHasGoodXml = true;
                            }
                        }
                    }
                }
            }
            if (!bHasGoodXml)
            {
                //add an error message
                uri.ErrorMessage += DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "MODELHELPERS_BADXMLCONTENT");
            }
            return bHasGoodXml;
        }
        public static bool SaveStandardContentXmlDoc(ContentURI uri, XElement root)
        {
            bool bHasGoodXml = false;
            if ((!string.IsNullOrEmpty(uri.URIClub.ClubDocFullPath)))
            {
                if (root != null)
                {
                    //standard content rules
                    if (root.Name == Helpers.GeneralHelpers.ROOT_PATH)
                    {
                        if (root.HasElements)
                        {
                            string sErrorMsg = string.Empty;
                            Helpers.FileStorageIO fileStorageIO = new Helpers.FileStorageIO();
                            bHasGoodXml = fileStorageIO.SaveXmlInURI(uri,
                                root.CreateReader(), uri.URIClub.ClubDocFullPath,
                                out sErrorMsg);
                            uri.ErrorMessage += sErrorMsg;
                        }
                    }
                }
            }
            else
            {
                //add an error message
                uri.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                    string.Empty, "MODELHELPERS_BADXMLCONTENT");
            }
            return bHasGoodXml;
        }
        public static void AddLinkedViewStandardElement(XElement node,
            List<Microsoft.EntityFrameworkCore.Metadata.IProperty> vals)
        {
            var sProperty = vals.Find(t => t.Name == "LinkingXmlDoc");
            if (sProperty != null)
            {
                string sValue = sProperty.ValueGenerated.ToString();
                if (!string.IsNullOrEmpty(sValue))
                {
                    AddXmlAttributeToDoc(sValue, node);
                }
            }
        }
        public static void AddLinkedViewBaseElement(XElement node,
            List<Microsoft.EntityFrameworkCore.Metadata.IProperty> vals)
        {
            var sProperty = vals.Find(t => t.Name == "LinkedViewXml");
            if (sProperty != null)
            {
                string sValue = sProperty.ValueGenerated.ToString();
                if (!string.IsNullOrEmpty(sValue))
                {
                    AddXmlAttributeToDoc(sValue, node);
                }
            }
        }
        
    }
}
