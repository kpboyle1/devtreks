using DevTreks.Data;
using DevTreks.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DataHelpers = DevTreks.Data.Helpers;
using EditHelpers = DevTreks.Data.EditHelpers;

namespace DevTreks.Services.Helpers
{
    /// <summary>
    ///Purpose:		assist editing DevTreks content
    ///Author:		www.devtreks.org
    ///Date:		2014, June
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    /// </summary>
    public class EditHelper
    {
        public EditHelper() { }
        //standard html val for checkbox checked
        private const string SELECTED = "true";
        public static void GetEditParameters(ContentURI uri,
            ref StringDictionary deletes, ref IDictionary<string, string> updates)
        {
            //Get the form keys and then insert into an appropriate name value collection for editing
            string sAction = string.Empty;
            if (uri.URIDataManager.FormInput != null)
            {
                string sKeyName = string.Empty;
                foreach (KeyValuePair<string, string> kvp in uri.URIDataManager.FormInput)
                {
                    sKeyName = kvp.Key;
                    sAction = string.Empty;
                    //verify actual edits and not ui instructions (i.e. getting new views)
                    int iIndex = sKeyName.LastIndexOf(DataHelpers.GeneralHelpers.STRING_DELIMITER);
                    if (iIndex > 0)
                    {
                        sAction = kvp.Value;
                        if (sAction.EndsWith("*"))
                        {
                            //get rid of the javascript-inserted stars
                            sAction = sAction.TrimEnd(new char[] { '*' });
                            //math expressions replace plus + chars with -- because they get removed by either jquery or browser
                            if (sKeyName.ToLower().Contains(DevTreks.Data.RuleHelpers.GeneralRules.MATHEXPRESS))
                            {
                                //this conversion takes place using javascript Form.encodeFormElement
                                sAction = sAction.Replace("--", "+");
                            }
                            //calculator joint data prop replace plus + chars with -- because they get removed by either jquery or browser
                            if (sKeyName.ToLower().Contains(DevTreks.Data.RuleHelpers.GeneralRules.JOINTDATA))
                            {
                                //this conversion takes place using javascript Form.encodeFormElement
                                sAction = sAction.Replace("--", "<-");
                            }
                        }
                        if (!string.IsNullOrEmpty(sAction))
                        {
                            if (sAction.ToLower().StartsWith(EditHelpers.EditHelper.DELETE))
                            {
                                deletes.Add(sKeyName, sAction);
                            }
                            else
                            {
                                if (!sKeyName.ToLower().EndsWith(EditHelpers.EditHelper.DELETE))
                                {
                                    updates.Add(sKeyName, sAction);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static bool IsSameSubApp(ContentURI uri, string keyName)
        {
            //only one subapp can be edited at a time (i.e. linkedviews calculators 
            //should not interfere with edits being made)
            bool bIsSameSubApp = false;
            string sNodeName = DataHelpers.GeneralHelpers.GetSubstringFromFront(keyName,
                DataHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
            DataHelpers.GeneralHelpers.APPLICATION_TYPES eAppType 
                = DataHelpers.GeneralHelpers.APPLICATION_TYPES.none;
            DataHelpers.GeneralHelpers.SUBAPPLICATION_TYPES eSubAppType 
                = DataHelpers.GeneralHelpers.SUBAPPLICATION_TYPES.none;
            DataHelpers.GeneralHelpers.GetAppTypesFromNodeName(sNodeName,
                out eAppType, out eSubAppType);
            if (uri.URIDataManager.SubAppType == eSubAppType)
            {
                bIsSameSubApp = true;
            }
            return bIsSameSubApp;
        }
        public async Task<bool> UpdateAddInParamsAsync(ContentURI docToCalcURI, ContentURI calcDocURI)
        {
            bool bHasUpdatedParams = false;
            StringDictionary colDeletes = new StringDictionary();
            IDictionary<string, string> colUpdates = new Dictionary<string, string>();
            GetEditParameters(calcDocURI, ref colDeletes, ref colUpdates);
            if (colDeletes.Count > 0 || colUpdates.Count > 0)
            {
                //uri is always a linkedviewpack, calcDocURIPattern is always a linkedview,
                //so always same apps (i.e. no need for MakeNewURI())
                ContentURI oTempDocURI = new ContentURI(calcDocURI);
                //tempCalcDocPath has a random id and can't be used
                //calcDocURIPattern needs _temp suffix, so updates 
                //know that it is not a db update
                oTempDocURI.URIFileExtensionType 
                    = DataHelpers.GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString();
                oTempDocURI.UpdateURIPattern();
                oTempDocURI.URIClub.ClubDocFullPath 
                    = calcDocURI.URIDataManager.TempDocPath;
                oTempDocURI.URIMember.MemberDocFullPath 
                    = calcDocURI.URIDataManager.TempDocPath;
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(docToCalcURI,
                    oTempDocURI.URIClub.ClubDocFullPath))
                {
                    IContentService contentService = new ContentService(docToCalcURI);
                    XElement calcDocXml = await DevTreks.Data.Helpers.FileStorageIO.LoadXmlElementAsync(
                        docToCalcURI, oTempDocURI.URIClub.ClubDocFullPath);
                    bool bIsOkToSave = await contentService.UpdateAsync(oTempDocURI,
                       colDeletes, colUpdates, calcDocXml);
                    contentService.Dispose();
                }
                bHasUpdatedParams = true;
            }
            return bHasUpdatedParams;
        }
        public static void SetSelectsList(ContentURI uri)
        {
            if (uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects
                || uri.URIDataManager.ServerActionType
                == DataHelpers.GeneralHelpers.SERVER_ACTION_TYPES.select
                || uri.URIDataManager.ServerSubActionType
                == DataHelpers.GeneralHelpers.SERVER_SUBACTION_TYPES.buildtempdoc)
            {
                string sSelectionsMade = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    DevTreks.Data.EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectionsmade.ToString());
                if (!string.IsNullOrEmpty(sSelectionsMade))
                {
                    //build new doc stores selections made in hidden form element
                    uri.URIDataManager.SelectedList = sSelectionsMade;
                }
                else
                {
                    StringBuilder oSelects = new StringBuilder();
                    string sAction = string.Empty;
                    string sKeyName = string.Empty;
                    //sKeyName is a uripattern;parenturipattern delimited string
                    //parenturipattern is used during xml node insertion
                    foreach (KeyValuePair<string, string> kvp in uri.URIDataManager.FormInput)
                    {
                        sKeyName = kvp.Key;
                        sAction = string.Empty;
                        //the only checklist with names not ending in "*" are the selections list
                        if (sKeyName.EndsWith("*") == false)
                        {
                            sAction = kvp.Value;
                            string[] arrValues = sAction.Split(DataHelpers.GeneralHelpers.STRING_DELIMITERS);
                            //value tells what action to take
                            int i = 0;
                            int iLength = arrValues.Length;
                            //only one nodetype is ever allowed in the selects list
                            string sNodeTypeAllowedName = string.Empty;
                            for (i = 0; i < iLength; i++)
                            {
                                sAction = arrValues[i];
                                if (sAction == SELECTED)
                                {
                                    //simple delimited string
                                    if (oSelects.Length == 0)
                                    {
                                        oSelects.Append(sKeyName);
                                        //first selection determines nodetype allowed in selects list
                                        sNodeTypeAllowedName = DataHelpers.GeneralHelpers.GetSubstringFromFront(
                                            sKeyName, DataHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
                                        if (string.IsNullOrEmpty(uri.URIDataManager.SelectionsNodeNeededName))
                                        {
                                            uri.URIDataManager.SelectionsNodeNeededName = sNodeTypeAllowedName;
                                        }
                                    }
                                    else
                                    {
                                        sNodeTypeAllowedName = DataHelpers.GeneralHelpers.GetSubstringFromFront(
                                            sKeyName, DataHelpers.GeneralHelpers.WEBFILE_PATH_DELIMITERS, 2);
                                        if (uri.URIDataManager.SelectionsNodeNeededName.EndsWith(sNodeTypeAllowedName))
                                        {
                                            oSelects.Append(DataHelpers.GeneralHelpers.PARAMETER_DELIMITER);
                                            oSelects.Append(sKeyName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    uri.URIDataManager.SelectedList = oSelects.ToString();
                }
            }
        }
        
        public static void GetSelectOptions(ContentURI uri,
            out EditHelpers.AddHelperLinq.SELECTION_OPTIONS selectionOption,
            out string selectedAncestor)
        {
            selectionOption = EditHelpers.AddHelperLinq.SELECTION_OPTIONS.none;
            selectedAncestor = string.Empty;
            //use all ancestors, or just the one selected?
            string sBuildOption =
                DataHelpers.GeneralHelpers.GetFormValue(uri, 
                EditHelpers.AddHelperLinq.SELECTION_OPTIONS.rdobuildselects.ToString());
            SetSelectsNodeName(uri);
            if (!string.IsNullOrEmpty(sBuildOption))
            {
                selectionOption =
                    (EditHelpers.AddHelperLinq.SELECTION_OPTIONS)Enum.Parse(
                    typeof(EditHelpers.AddHelperLinq.SELECTION_OPTIONS), sBuildOption);
                selectedAncestor = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    EditHelpers.AddHelperLinq.SELECTION_OPTIONS.rdoselects.ToString());
            }
            else
            {
                //check to make sure that build new doc radio selections aren't available
                selectedAncestor = DataHelpers.GeneralHelpers.GetFormValue(uri,
                    EditHelpers.AddHelperLinq.SELECTION_OPTIONS.rdoselects.ToString());
                if (string.IsNullOrEmpty(selectedAncestor))
                {
                    if (uri.URIDataManager.SelectionsNodeURIPattern
                        != string.Empty)
                    {
                        selectionOption
                           = EditHelpers.AddHelperLinq.SELECTION_OPTIONS.selectedparent;
                    }
                }
            }

        }
        
        public static void SetSelectsNodeName(ContentURI uri)
        {
            //parent file and node of selections (select_click adds params to form els)
            string sSelectsNodeFileName = DataHelpers.GeneralHelpers.GetFormValue(uri, 
                EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.selectsnodeuripattern.ToString());
            if (string.IsNullOrEmpty(sSelectsNodeFileName)) sSelectsNodeFileName = string.Empty;
            uri.URIDataManager.SelectionsNodeURIPattern = sSelectsNodeFileName;
        }
        public static void SetSelectsURIPattern(ContentURI uri)
        {
            //parent file and node of selections (select_click adds params to form els)
            string sSelectsURIPattern = DataHelpers.GeneralHelpers.GetFormValue(uri, 
                EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.selectsuripattern.ToString());
            if (string.IsNullOrEmpty(sSelectsURIPattern)) sSelectsURIPattern = string.Empty;
            uri.URIDataManager.SelectionsURIPattern = sSelectsURIPattern;
        }
        
        public static void SetSelectsCalcParams(ContentURI uri)
        {
            //parent file and node of selections (select_click adds params to form els)
            string sSelectsCalcParams = DataHelpers.GeneralHelpers.GetFormValue(uri, 
                EditHelpers.AddHelperLinq.SELECT_EXISTING_PARAMS.selectscalcparams.ToString());
            if (string.IsNullOrEmpty(sSelectsCalcParams)) sSelectsCalcParams = string.Empty;
            uri.URIDataManager.CalcParams = sSelectsCalcParams;
        }
        public static void SetSelects(ContentURI uri)
        {
            //keep selections stateful on client
            SetSelectsList(uri);
            SetSelectsNodeName(uri);
            SetSelectsURIPattern(uri);
            //selected view
            SetSelectsCalcParams(uri);
        }
        public static void ClearSelects(ContentURI uri)
        {
            //clear the selections (they've already been saved and displayed)
            uri.URIDataManager.SelectionsAttributeName = string.Empty;
            uri.URIDataManager.SelectionsURIPattern = string.Empty;
            uri.URIDataManager.SelectedList = string.Empty;
            uri.URIDataManager.SelectionsNodeURIPattern = string.Empty;
            uri.URIDataManager.SelectionsNodeNeededName = string.Empty;
        }

        public static void MakeTempDocParms(ContentURI uri)
        {
            DataHelpers.AppSettings.SetTempDocPathandFileName(uri);
            //set the associated params needed for display
            DataHelpers.GeneralHelpers.SetURIParams(uri);
            //tempdocs can always be viewed and edited
            uri.URIDataManager.EditViewEditType 
                = DevTreks.Data.Helpers.GeneralHelpers.VIEW_EDIT_TYPES.full;
        }
        public static XElement GetEditLinqDoc(ContentURI uri)
        {
            //used for transformations and building new docs
            XElement oDevTrekDoc = null;
            if (uri.URIDataManager.UseSelectedLinkedView == false)
            {
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    uri, uri.URIClub.ClubDocFullPath))
                {
                    oDevTrekDoc = DevTreks.Data.Helpers.FileStorageIO.LoadXmlElement(
                        uri, uri.URIClub.ClubDocFullPath);
                }
                else
                {
                    //files should already be on hand
                    uri.ErrorMessage = DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
                        string.Empty, "EDITHELPER_NOEDITDOC");
                }
            }
            else
            {
                //uri is selectedLinkedViewURI
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    uri, uri.URIClub.ClubDocFullPath))
                {
                    oDevTrekDoc = DevTreks.Data.Helpers.FileStorageIO.LoadXmlElement(
                        uri, uri.URIClub.ClubDocFullPath);
                    //set the selectedlinkedview properties needed for editing
                    SetLinkedViewPropertiesForEdit(uri, oDevTrekDoc);
                }
                else
                {
                    //files should already be on hand
                    uri.ErrorMessage = DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
                        string.Empty, "EDITHELPER_NOSELECTEDLV");
                }
            }
            return oDevTrekDoc;
        }
        public static async Task<XmlDocument> GetEditDocAsync(ContentURI uri)
        {
            XmlDocument oDevTrekDoc = null;
            if (uri.URIDataManager.UseSelectedLinkedView == false)
            {
                if (DataHelpers.FileStorageIO.URIAbsoluteExists(
                    uri, uri.URIClub.ClubDocFullPath))
                {
                    oDevTrekDoc = new XmlDocument();
                    XmlReader reader = DataHelpers.FileStorageIO.GetXmlReader(uri,
                        uri.URIClub.ClubDocFullPath);
                    if (reader != null)
                    {
                        using (reader)
                        {
                            oDevTrekDoc.Load(reader);
                        }
                    }
                }
                else
                {
                    //files should already be on hand
                    uri.ErrorMessage = DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
                        string.Empty, "EDITHELPER_NOEDITDOC");
                }
            }
            else
            {
                //uri is selectedLinkedViewURI
                IContentService contentService = new ContentService(uri);
                XmlReader oSelectedViewReader = await contentService.GetURISecondBaseDocAsync(uri);
                contentService.Dispose();
                if (oSelectedViewReader != null)
                {
                    using (oSelectedViewReader)
                    {
                        oDevTrekDoc = new XmlDocument();
                        oDevTrekDoc.Load(oSelectedViewReader);
                    }
                }
                if (oDevTrekDoc == null)
                {
                    //database xmldoc fields should already be on hand
                    uri.ErrorMessage = DevTreks.Exceptions.Errors.MakeStandardErrorMsg(
                        string.Empty, "EDITHELPER_NOSELECTEDLV");
                }
            }
            return oDevTrekDoc;
        }
        public async Task SaveSelectedLinkedViewAsync(ContentURI selectedLinkedViewURI,
            XElement devTrekDoc)
        {
            //addins save their own file system state (in AddInRunHelper)
            if (selectedLinkedViewURI.URIDataManager.UseSelectedLinkedView
                && selectedLinkedViewURI.URIDataManager.ServerSubActionType
                != DevTreks.Data.Helpers.GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
            {
                //save in the file system 
                DevTreks.Data.EditHelpers.EditHelper.SaveEdits(devTrekDoc, 
                    selectedLinkedViewURI);
                //save the custom doc in the db (if it is being edited
                if (selectedLinkedViewURI.URIDataManager.ServerActionType
                    == DevTreks.Data.Helpers.GeneralHelpers.SERVER_ACTION_TYPES.edit)
                {
                    XmlReader oEditedDocReader
                        = await DataHelpers.FileStorageIO.GetXmlReaderAsync(selectedLinkedViewURI, 
                            selectedLinkedViewURI.URIClub.ClubDocFullPath);
                    if (oEditedDocReader != null)
                    {
                        using (oEditedDocReader)
                        {
                            //reset linkedview to original properties prior to edits
                            SetLinkedViewPropertiesAfterEdit(selectedLinkedViewURI);
                            IContentService contentService = new ContentService(selectedLinkedViewURI);
                            bool bIsMetaData = false;
                            bool bIsSaved = await contentService.SaveURISecondBaseDocAsync(
                                selectedLinkedViewURI, bIsMetaData,
                                Path.GetFileName(selectedLinkedViewURI.URIClub.ClubDocFullPath),
                                oEditedDocReader);
                            contentService.Dispose();
                        }
                    }
                }
            }
            else
            {
                DevTreks.Data.EditHelpers.EditHelper.SaveEdits(
                    devTrekDoc, selectedLinkedViewURI);
            }
        }
        private static void SetLinkedViewPropertiesForEdit(
            ContentURI selectedLinkedViewURI, XElement selectedViewDoc)
        {
            AccountHelper.AUTHORIZATION_LEVELS eInitialAuthorization
                    = selectedLinkedViewURI.URIMember.ClubInUse.PrivateAuthorizationLevel;
            //turn off db editing
            selectedLinkedViewURI.URIMember.ClubInUse.PrivateAuthorizationLevel =
                AccountHelper.AUTHORIZATION_LEVELS.viewonly;
            if (selectedViewDoc != null)
            {
                //prior to editing, change uri's app and subapp type 
                //to the custom doc being edited
                string sAppNodeName
                    = DevTreks.Data.EditHelpers.XmlLinq.GetRootElementFirstChildNodeName(
                    selectedViewDoc);
                DataHelpers.GeneralHelpers.ChangeAppTypeFromNodeName(
                    selectedLinkedViewURI, sAppNodeName);
            }
        }
        private static void SetLinkedViewPropertiesAfterEdit(
            ContentURI selectedLinkedViewURI)
        {
            //put initial authorization back
            selectedLinkedViewURI.URIMember.ClubInUse.PrivateAuthorizationLevel
                = AccountHelper.AUTHORIZATION_LEVELS.fulledits;
            //switch back from customdoc app to original devpack or linkedview
            if (!string.IsNullOrEmpty(
                selectedLinkedViewURI.URIDataManager.ParentURIPattern))
            {
                string sAppNodeName
                    = ContentURI.GetURIPatternPart(
                        selectedLinkedViewURI.URIDataManager.ParentURIPattern,
                        ContentURI.URIPATTERNPART.node);
                DataHelpers.GeneralHelpers.ChangeAppTypeFromNodeName(
                    selectedLinkedViewURI, sAppNodeName);
            }
            else
            {
                string sAppNodeName
                    = ContentURI.GetURIPatternPart(
                        selectedLinkedViewURI.URIPattern, ContentURI.URIPATTERNPART.node);
                DataHelpers.GeneralHelpers.ChangeAppTypeFromNodeName(
                    selectedLinkedViewURI, sAppNodeName);
            }
        }
    }
}
