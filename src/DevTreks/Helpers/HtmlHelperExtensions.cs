using DevTreks.Data.Helpers;
using DevTreks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Net;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using DevTreks.Data.RuleHelpers;
using DevTreks.Data.EditHelpers;
using DataAppHelpers = DevTreks.Data.AppHelpers;
using DevTreks.Models;

namespace DevTreks.Helpers
{
    /// <summary>
    ///Purpose:		static Html extensions for presentation layer xhtml manipulation
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	www.devtreks.org
    /// </summary>
    public static class HtmlHelperExtensions
    {
        public const string CANCEL_EDITS = "btnCancelAllDoc";
        public static HtmlString MakeAddDefaultNodeButton(this IHtmlHelper helper,
            ContentURI model, string label, string nodeToAddName)
        {
            //htmlstring will display &amp;
            string adddefaultparams = string.Concat("&parentnode=", model.URIPattern, "&defaultnode=",
               StylesheetHelper.GetURIPattern("00Default", "1", model.URINetworkPartName,
               nodeToAddName, string.Empty));
            StringBuilder result = new StringBuilder();
            result.AppendLine(helper.DivStart(string.Empty, "ui-grid-a").ToString());
            result.AppendLine(helper.DivStart(string.Empty, "ui-block-a").ToString());
            result.AppendLine(StylesheetHelper.MakeDevTreksButton(string.Concat("adddefault_", nodeToAddName), "SubmitButton1Enabled150",
                label, model.URIDataManager.ContentURIPattern, string.Empty,
                GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(), GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults.ToString(),
                GeneralHelpers.NONE, @adddefaultparams));
            result.AppendLine(helper.DivEnd().ToString());
            result.AppendLine(helper.DivStart(string.Empty, "ui-block-b").ToString());
            result.AppendLine(helper.InputMobile(GeneralHelpers.VIEW_EDIT_TYPES.full, "NumberToAdd", "Input35", "text", string.Empty,
                string.Concat("adddefault_", @model.URIPattern), "0", "true", "true", string.Empty, string.Empty).ToString());
            result.AppendLine(helper.DivEnd().ToString());
            result.AppendLine(helper.DivEnd().ToString());
            return new HtmlString(result.ToString());
        }
        public static HtmlString MakeDefaultClubButtons(this IHtmlHelper helper,
            ContentURI model, string searchurl)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, string.Empty,
                   "controlgroup", "horizontal"));
                //2.0.0: eliminated because members can't add themselves to existing clubs
                //a club coordinator must add them to the club using standard club management
                //result.WriteLine(StylesheetHelper.MakeGetSelectionsLink("selectexisting1", "#",
                //    "GetSelectsLink", "Select Club", "spanSelectionFiles",
                //    model.URIDataManager.ContentURIPattern, searchurl,
                //    DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString(), string.Empty, string.Empty));
                //2.0.0 simplification for inserting new club by member (std EF add default pattern)
                //this can't be std. WriteTo 
                result.WriteLine(MakeAddDefaultNodeButton(helper, model, "Add New Club",
                    DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString()));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeServiceAgreementButtons(this IHtmlHelper helper,
            ContentURI model, string searchurl)
        {
            using (StringWriter result = new StringWriter())
            {
                //2.0.0 simplification for selecting services and adding new services
                string adddefaultparams = string.Concat("&parentnode=", model.URIPattern, "&defaultnode=",
               StylesheetHelper.GetURIPattern("00Default", "1", model.URINetworkPartName,
                    DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString(), string.Empty));
                result.WriteLine(helper.DivStart(string.Empty, "ui-grid-a"));
                result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                result.WriteLine(StylesheetHelper.MakeDevTreksButton(string.Concat("adddefault_", DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString()), 
                    "SubmitButton1Enabled150", "Add New Service", model.URIDataManager.ContentURIPattern, string.Empty,
                    GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(), 
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults.ToString(),
                    GeneralHelpers.NONE, adddefaultparams));
                string selectName = string.Concat("adddefault_", searchurl);
                result.WriteLine(helper.LabelHidden("lblServiceGroup", "Service", "ui-hidden-accessible"));
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                result.Write(helper.SelectStart(
                    GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "lblServiceGroup", selectName, string.Empty));
                bool bIsSelected = false;
                Dictionary<string, string> servicegroups = GeneralHelpers.GetSubAppTypeDictionary();
                foreach (KeyValuePair<string, string> kvp in servicegroups)
                {
                    result.Write(helper.Option(kvp.Value, kvp.Key, bIsSelected));
                }
                result.Write(helper.SelectEnd());
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                result.WriteLine(StylesheetHelper.MakeGetSelectionsLink("selectexisting1", "#",
                    "GetSelectsLink", "Select Existing Service", "spanSelectionFiles",
                    model.URIDataManager.ContentURIPattern, searchurl,
                DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString(), string.Empty, string.Empty));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeSelectList(this IHtmlHelper helper,
            string searchurl, string value, string propertyName,
            string dataType, string length, string label,
            GeneralHelpers.VIEW_EDIT_TYPES viewType, string className,
            Dictionary<string, string> options)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.LabelStrong(propertyName, label));
                MakeSelectList(helper, searchurl, value, propertyName,
                    dataType, length, viewType, className, options)
                    .WriteTo(result, HtmlEncoder.Default);
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeSelectList(this IHtmlHelper helper,
            string searchurl, string value, string propertyName,
            string dataType, string length,
            GeneralHelpers.VIEW_EDIT_TYPES viewType, string className,
            Dictionary<string, string> options)
        {
            using (StringWriter result = new StringWriter())
            {
                if (viewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
                {
                    result.WriteLine(helper.SelectUpdateStart(viewType, searchurl, value, propertyName,
                        dataType, length, className, string.Empty));
                    bool bIsSelected = false;
                    foreach (KeyValuePair<string, string> kvp in options)
                    {
                        bIsSelected = false;
                        if (kvp.Key == value)
                        {
                            bIsSelected = true;
                        }
                        //counterintuitive but the dictionary's key is the unique value for the options
                        result.WriteLine(helper.Option(kvp.Value, kvp.Key, bIsSelected));
                    }
                    result.WriteLine(helper.SelectEnd());
                }
                else
                {
                    bool bIsSelected = false;
                    bool bHasSelection = false;
                    result.WriteLine(helper.SelectStart(viewType, propertyName,
                        propertyName, string.Empty));
                    foreach (KeyValuePair<string, string> kvp in options)
                    {
                        bIsSelected = false;
                        if (kvp.Key == value)
                        {
                            bIsSelected = true;
                            bHasSelection = true;
                        }
                        if (bIsSelected)
                        {
                            result.WriteLine(helper.Option(kvp.Value, kvp.Key, bIsSelected));
                        }
                    }
                    result.WriteLine(helper.SelectEnd());
                    if (!bHasSelection)
                    {
                        result.WriteLine(string.Concat("   ", value));
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeName(this IHtmlHelper helper,
            string searchurl, string value, string propertyName,
            GeneralHelpers.VIEW_EDIT_TYPES viewType, bool canDelete)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.LabelHidden(propertyName,
                    "Name", "ui-hidden-accessible"));
                result.WriteLine(helper.InputTextUpdate(viewType,
                    searchurl, value, propertyName, GeneralRules.STRING,
                    "75", "Input350Bold", string.Empty));
                if (canDelete)
                {
                    result.WriteLine(helper.FieldsetStart(string.Empty,
                        string.Empty, "controlgroup", "horizontal", "true"));
                    //don't include a legend
                    MakeDeleteOptions(helper, result, searchurl);
                    result.WriteLine(helper.FieldsetEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString MakeDeleteOptionsWithFieldSet(this IHtmlHelper helper,
            string searchurl, string name)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty,
                    "controlgroup", "horizontal", "true"));
                result.WriteLine("<legend>" + name + "</legend>");
                helper.MakeDeleteOptions(result, searchurl);
                result.WriteLine(helper.FieldsetEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static void MakeDeleteOptions(this IHtmlHelper helper,
            StringWriter result, string searchurl)
        {
            result.WriteLine(helper.Input(string.Concat(searchurl,
                ";Delete"), string.Empty, "radio", string.Empty,
                string.Concat(searchurl, ";Delete"), "Delete"));
            result.WriteLine(helper.LabelRegular(string.Concat(searchurl,
                ";Delete"), "D"));
            result.WriteLine(helper.Input(string.Concat(searchurl,
                ";Delete2"), string.Empty, "radio", string.Empty,
                string.Concat(searchurl, ";Delete"), string.Empty));
            result.WriteLine(helper.LabelRegular(string.Concat(searchurl,
                ";Delete2"), "U"));
        }
        public static HtmlString MakeRadioTrueFalseOption(this IHtmlHelper helper,
            string id, string name, string value, bool isChecked, string label)
        {
            using (StringWriter result = new StringWriter())
            {
                if (isChecked == true)
                {
                    result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        id, string.Empty, "radio", name, value, isChecked));
                    result.WriteLine(helper.LabelRegular(id, label));
                }
                else
                {
                    isChecked = false;
                    result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        id, string.Empty, "radio", name, value, isChecked));
                    result.WriteLine(helper.LabelRegular(id, label));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeRadioTrueFalseBothOptions(this IHtmlHelper helper,
            string idName, string name, bool value)
        {
            bool bIsChecked = false;
            string sValue = string.Empty;
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, string.Empty,
                    "fieldcontain", string.Empty));
                result.WriteLine(helper.FieldsetStart(string.Empty,
                    string.Empty, "controlgroup", "horizontal", "true"));
                string id = string.Concat(@idName, "1");
                if (value == true)
                {
                    bIsChecked = true;
                    sValue = "1";
                    result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        id, string.Empty, "radio", name, sValue, bIsChecked));
                    result.WriteLine(helper.LabelRegular(id, "T"));

                    id = string.Concat(@idName, "0");
                    bIsChecked = false;
                    sValue = "0";
                    result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        id, string.Empty, "radio", name, sValue, bIsChecked));
                    result.WriteLine(helper.LabelRegular(id, "F"));
                }
                else
                {
                    bIsChecked = false;
                    sValue = "1";
                    result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        id, string.Empty, "radio", name, sValue, bIsChecked));
                    result.WriteLine(helper.LabelRegular(@id, "T"));
                    id = string.Concat(@idName, "0");
                    bIsChecked = true;
                    sValue = "0";
                    result.WriteLine(helper.LabelRegular(@id, "F"));
                }
                result.WriteLine(helper.FieldsetEnd());
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }

        public static HtmlString MakeTextArea(this IHtmlHelper helper,
            string searchurl, string value, string propertyName,
            string label, GeneralHelpers.VIEW_EDIT_TYPES viewType,
            string className, string dataLength)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.LabelStrong(propertyName, label));
                if (viewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
                {
                    string sHtmlName = EditHelper.MakeStandardEditName(searchurl,
                        propertyName, GeneralRules.STRING, dataLength);
                    //use jquery default style
                    result.WriteLine(helper.TextArea(propertyName,
                        sHtmlName, string.Empty, value, false));
                }
                else
                {
                    result.WriteLine(helper.TextArea(propertyName,
                        string.Empty, string.Empty, value, true));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakePreviewLinks(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                //regular links that search engines can index
                string sSearchHref = model.URIFull.Replace(
                    GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                    GeneralHelpers.SERVER_ACTION_TYPES.search.ToString());
                string sViewsHref = model.URIFull.Replace(
                    GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                    GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString());
                string sSelectHref = model.URIFull.Replace(
                    GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                    GeneralHelpers.SERVER_ACTION_TYPES.select.ToString());
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                result.WriteLine(helper.Span(string.Empty, string.Empty,
                    string.Concat(AppHelper.GetResource("IRIS_SEARCH"), " : ")));
                result.WriteLine(helper.Span(string.Empty, string.Empty,
                    string.Concat("&lt; &nbsp;",
                    helper.Link("id", sSearchHref, string.Empty,
                    AppHelper.GetResource("IRI_SEARCH")))));
                result.WriteLine(helper.Span(string.Empty, string.Empty,
                    string.Concat("&lt; &nbsp;",
                    helper.Link("id", sViewsHref, string.Empty,
                    AppHelper.GetResource("IRI_VIEWS")), "&lt; &nbsp;")));
                result.WriteLine(helper.Span(string.Empty, string.Empty,
                   string.Concat("&nbsp;",
                   helper.Link("id", sSelectHref, string.Empty,
                   AppHelper.GetResource("IRI_SELECT")), "&lt; &nbsp;")));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeHorizStartAtRowButtons(this IHtmlHelper helper,
            ContentURI uri, int startRow, int pageSize, int rowCount,
            int parentStartRow, bool isSearch)
        {
            using (StringWriter result = new StringWriter())
            {
                string sClass1 = "SearchButton1Enabled";
                string sClass2 = "SearchButton1Enabled";
                if (isSearch)
                {
                    HtmlExtensions.GetSearchStartAtRowButtonsClass(startRow,
                        pageSize, rowCount, out sClass1, out sClass2);
                }
                else
                {
                    sClass1 = "Button1Enabled";
                    sClass2 = "Button1Enabled";
                    HtmlExtensions.GetStartAtRowButtonsClass(startRow,
                        pageSize, rowCount, out sClass1, out sClass2);
                }
                bool bNeedsRowNav = false;
                //2.0.0 refactored to 1000
                if (rowCount > pageSize)
                {
                    bNeedsRowNav = true;
                }
                int i = (int)uri.URIDataManager.ServerActionType;
                string sSubActionType = GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString();
                if (uri.URIDataManager.ServerActionType
                    == GeneralHelpers.SERVER_ACTION_TYPES.edit)
                {
                    if (uri.URIDataManager.UseSelectedLinkedView)
                    {
                        //they are editing custom docs. When paginating only, none are selected.
                        //this clue will tell GetCalcDoc to set UseSelectedView = true
                        sSubActionType = GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml.ToString();
                    }
                    else
                    {
                        //switch from the edit action to the pagination action
                        if (uri.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits
                            || uri.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.savenewselects
                            || uri.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects
                            || uri.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults)
                        {
                            sSubActionType = GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString();
                        }
                        else if (uri.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.submitformedits)
                        {
                            sSubActionType = GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithform.ToString();
                        }
                        else if (uri.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
                        {
                            sSubActionType = GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString();
                        }
                    }
                }
                if (bNeedsRowNav)
                {
                    result.WriteLine(helper.DivStart(string.Empty, "ui-grid-c"));
                    int iForwardOrBackVar = 0;
                    //clients init with full uri patterns held in id of buttons clicked
                    string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        uri.URIDataManager.ControllerName,
                        uri.URIDataManager.ServerActionType.ToString(),
                        uri.URIPattern, sSubActionType,
                        uri.URIDataManager.SubActionView, iForwardOrBackVar.ToString());
                    //custom doc navigation needs extraparams
                    string sSelectParams = StylesheetHelper.SetSelectedLinkedViewParams(uri, false);
                    //go back instructions
                    bool bIsDisabled = false;
                    bIsDisabled = (sClass1 == "Button1Disabled") ? true : false;
                    string sTitle = AppHelper.GetResource("GO_BACK") + pageSize;
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                    result.WriteLine(helper.InputUnobtrusiveMobile(
                        id: string.Concat(sTitle, i),
                        classAttribute: sClass1,
                        type: "button",
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: sSelectParams,
                        value: AppHelper.GetResource("GO_BACK"),
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "arrow-l",
                        dataIconPos: "left"));
                    result.WriteLine(helper.DivEnd());
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-b").ToString());
                    result.Write("<br/>");
                    result.Write(helper.Raw("&nbsp;"));
                    result.Write(string.Concat(AppHelper.GetResource("GOTOROW"),
                        @AppHelper.GetResource("OF"), " ", rowCount.ToString()));
                    result.Write("<br/>");
                    result.WriteLine(helper.DivEnd().ToString());
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-c"));
                    sClass1 = "Input35";
                    result.WriteLine(helper.InputMobile(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        string.Concat(HtmlExtensions.START_ROW, i), sClass1,
                        "text", string.Empty, string.Concat(HtmlExtensions.START_ROW, i),
                        startRow.ToString(), "true", "true",
                        string.Empty, string.Empty));

                    //go forward instructions
                    bIsDisabled = (sClass2 == "Button1Disabled") ? true : false;
                    sTitle = AppHelper.GetResource("GO_FORWARD") + pageSize;
                    iForwardOrBackVar = 1;
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(uri.URIDataManager.ControllerName,
                        uri.URIDataManager.ServerActionType.ToString(), uri.URIPattern,
                        sSubActionType, uri.URIDataManager.SubActionView, iForwardOrBackVar.ToString());
                    result.WriteLine(helper.DivEnd());
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-d"));
                    result.WriteLine(helper.InputUnobtrusiveMobile(
                        id: string.Concat(sTitle, i),
                        classAttribute: sClass2,
                        type: "button",
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: sSelectParams,
                        value: AppHelper.GetResource("GO_FORWARD"),
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "arrow-r",
                        dataIconPos: "right"));
                    result.WriteLine(helper.DivEnd());
                    //end of block divider
                    result.WriteLine(helper.DivEnd());
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    //hidden els
                    result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        string.Concat(HtmlExtensions.OLDSTART_ROW, i), string.Empty, "hidden",
                        string.Empty, string.Concat(HtmlExtensions.OLDSTART_ROW, i), startRow.ToString()));
                    result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        string.Concat(HtmlExtensions.PARENTSTART_ROW, i), string.Empty, "hidden",
                        string.Empty, string.Concat(HtmlExtensions.PARENTSTART_ROW, i),
                        parentStartRow.ToString()));
                    result.WriteLine(helper.DivEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeSynchDocLinks(this IHtmlHelper helper,
            ContentURI uri)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty,
                    "ui-grid-b"));
                string sJavascriptMethodForDevTrekFull = string.Empty;
                string sContentURIPattern = string.Empty;
                bool bIsAdminApp = GeneralHelpers.IsAdminApp(uri.URIDataManager.AppType);
                if (uri.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.preview)
                {
                    //last panel 
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        uri.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.search.ToString(),
                        uri.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);

                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                    result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReloadSearch",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("RELOAD_SEARCH"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty,
                        dataRole: "button",
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "arrow-l",
                        dataIconPos: "left"));
                    result.WriteLine(helper.DivEnd());
                    if (!bIsAdminApp)
                    {
                        //load in views panel allowed default addins to be loaded (for quickly linking views)
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            uri.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                            uri.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                        result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkLinkedViewLoadAddins",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("LINKEDVIEWS_SYNCH"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: string.Empty,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "arrow-r",
                            dataIconPos: "right"));
                        result.WriteLine(helper.DivEnd());
                    }
                    else
                    {
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                        result.WriteLine(helper.DivEnd());
                    }
                    //next panel
                    //select view needs rowargs
                    string sExtraParams = StylesheetHelper.SetRowArgs(uri.URIDataManager.StartRow,
                        uri.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        uri.URIDataManager.ParentStartRow);
                    //load in views panel allowed default addins to be loaded (for quickly linking views)
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        uri.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                        uri.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-c"));
                    result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkLoadSelect",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("LOAD_SELECT"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: sExtraParams,
                        dataRole: "button",
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "arrow-r",
                        dataIconPos: "right"));
                    result.WriteLine(helper.DivEnd());
                }
                else if (uri.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.select)
                {
                    //both preview and edit views need rowargs
                    string sExtraParams = StylesheetHelper.SetRowArgs(uri.URIDataManager.StartRow,
                        uri.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        uri.URIDataManager.ParentStartRow);
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        uri.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                        uri.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                    result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReloadPreview",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("RELOAD_PREVIEW"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: sExtraParams,
                        dataRole: "button",
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "arrow-l",
                        dataIconPos: "left"));
                    result.WriteLine(helper.DivEnd());
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                    result.WriteLine(helper.DivEnd());
                    //don't allow a synch next when selections are pending (or can't put selection anywhere)
                    if (uri.URIDataManager.SelectionsNodeNeededName == string.Empty)
                    {
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            uri.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                            uri.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-c"));
                        result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkLoadEdit",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("LOAD_EDIT"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: sExtraParams,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "arrow-r",
                            dataIconPos: "right"));
                        result.WriteLine(helper.DivEnd());
                    }
                    else
                    {
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-c"));
                        result.WriteLine(helper.DivEnd());
                    }
                }
                else if (uri.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.linkedviews)
                {
                    if (uri.URIFileExtensionType
                        != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        //link to a select view (to see context of this uri) and add to pack view
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            uri.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                            uri.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                        result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReloadSelect",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("RELOAD_SELECT"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: string.Empty,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "arrow-l",
                            dataIconPos: "left"));
                        result.WriteLine(helper.DivEnd());
                    }
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                    result.WriteLine(helper.DivEnd());
                    if (FileStorageIO.URIAbsoluteExists(uri, uri.URIClub.ClubDocFullPath))
                    {
                        //save in pack
                        //linkedviews use calcparams to package calcs and analyses
                        bool bNeedsSingleQuotes = true;
                        string sSelectedLinkedViewURI = string.Empty;
                        string sDocToCalcURI = string.Empty;
                        string sCalcDocURI = string.Empty;
                        string sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                            bNeedsSingleQuotes, uri, string.Empty, ref sCalcDocURI, ref sDocToCalcURI,
                            ref sSelectedLinkedViewURI);
                        string sMainXmlDocPath = IOHelper.GetPackageMainDocPath(uri);
                        sCalcParams = sCalcParams.EndsWith("'")
                            ? sCalcParams.Remove(sCalcParams.Length - 1) : sCalcParams;
                        sCalcParams += string.Concat(GeneralHelpers.FORMELEMENT_DELIMITER,
                            IOHelper.URI_XMLDOC_PATH, "=", sMainXmlDocPath, "'");
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            uri.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.pack.ToString(),
                            uri.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-c"));
                        result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkLoadPack",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("ADD_TO_PACK2"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.addtopack.ToString(),
                            extraParams: sCalcParams,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "arrow-r",
                            dataIconPos: "right"));
                        result.WriteLine(helper.DivEnd());
                    }
                }
                else
                {
                    if (uri.URIFileExtensionType
                        != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        string sExtraParams = StylesheetHelper.SetRowArgs(uri.URIDataManager.StartRow,
                            uri.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                            uri.URIDataManager.ParentStartRow);
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            uri.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                            uri.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                        result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReloadSelect2",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("RELOAD_SELECT"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: sExtraParams,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "arrow-l",
                            dataIconPos: "left"));
                        result.WriteLine(helper.DivEnd());
                    }
                    result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                    result.WriteLine(helper.DivEnd());
                    string sURIPattern = uri.URIPattern;
                    if (uri.URIPattern.EndsWith(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        || uri.URIPattern.EndsWith(DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString()))
                    {
                        if (uri.URIDataManager.Ancestors.Count >= 7)
                        {
                            //linkedviewpack ancestor
                            ContentURI ancestorURI = uri.URIDataManager.Ancestors[6];
                            if (ancestorURI != null)
                            {
                                sURIPattern = ancestorURI.URIPattern;
                            }
                        }
                    }
                    //save in pack
                    if (ContentHelper.NodeCanHaveFile(uri.URIClub.ClubDocFullPath))
                    {
                        //160 deprecated separate file storage for guests
                        string sMainXmlDocPath = uri.URIClub.ClubDocFullPath;
                        //switch from filepath delimited to webpath delimiter (because of // issues)
                        sMainXmlDocPath = sMainXmlDocPath.Replace(
                            GeneralHelpers.FILE_PATH_DELIMITER, GeneralHelpers.WEBFILE_PATH_DELIMITER);
                        string sCalcParams = string.Concat("'", GeneralHelpers.FORMELEMENT_DELIMITER,
                            IOHelper.URI_XMLDOC_PATH, "=", sMainXmlDocPath, "'");
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            uri.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.pack.ToString(),
                            sURIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.DivStart(string.Empty, "ui-block-c"));
                        result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReLoadPack",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("ADD_TO_PACK"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.addtopack.ToString(),
                            extraParams: sCalcParams,
                            dataRole: "button",
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: "arrow-r",
                            dataIconPos: "right"));
                        result.WriteLine(helper.DivEnd());
                    }
                }
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeMetaDocUpload(this IHtmlHelper helper,
           ContentURI model, string searchurl, string contenturipattern,
            string nodeid, string xml, string propertyName, GeneralHelpers.VIEW_EDIT_TYPES viewType,
            string cellNumber, string fileExtType)
        {
            using (StringWriter result = new StringWriter())
            {
                string sMetaDataId = string.Concat("metadata_", nodeid, viewType.ToString());
                result.WriteLine(helper.DivStart("divMetaUpload", string.Empty));
                helper.InputUnobtrusiveMobile(string.Concat("btnMetaData_", nodeid), "ShowElement", "button",
                   sMetaDataId, "Toggle Authorship", "true", "true", string.Empty, string.Empty)
                   .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.DivEnd());
                result.Write("<br/>");
                result.Write("<br/>");
                result.WriteLine(helper.DivStart(sMetaDataId, "display:none"));

                if (fileExtType != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                    && viewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
                {
                    string buttonid = string.Concat("btnDisplayFileUpload_", nodeid);
                    string formparam = EditHelper.MakeStandardEditName(searchurl,
                        propertyName, GeneralRules.XML, "2000");
                    string spanid = string.Concat("spanMetaFileUpload_", nodeid);
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    result.Write(StylesheetHelper.MakeUploadButton(buttonid, "UploadFile", "Show MetaData Doc Upload",
                        spanid, contenturipattern, searchurl, string.Empty, string.Empty, formparam));
                    result.WriteLine(helper.DivEnd());

                }
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString MakeFileUpload(this IHtmlHelper helper,
            string searchurl, string contenturipattern,
            string nodeid, string fileName, string propertyName,
            GeneralHelpers.VIEW_EDIT_TYPES viewType,
            string cellNumber, string networkId, string fileExtType,
            string nodeName, string dataType, string dataSize)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                if (fileExtType != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                    && viewType == GeneralHelpers.VIEW_EDIT_TYPES.full)
                {
                    string searchfilenameurl = StylesheetHelper
                        .GetURIPattern(fileName, nodeid,
                        networkId, nodeName, fileExtType);
                    string buttonid = string.Concat("btnUpload_", nodeid);
                    string formparam = EditHelper
                        .MakeStandardEditName(searchfilenameurl,
                        propertyName, dataType, dataSize);
                    string spanid = string.Concat("spanFileUpload_", nodeid);
                    result.WriteLine(helper.SpanStart(string.Empty, string.Empty));
                    result.Write(StylesheetHelper
                        .MakeUploadButton(buttonid,
                            "UploadFile", "Show File Upload", spanid,
                            contenturipattern, searchurl, string.Empty,
                            string.Empty, formparam));
                    result.WriteLine(helper.SpanEnd());
                    result.WriteLine(helper.Span(spanid, string.Empty,
                        string.Empty));
                }
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString WriteViewLinks(this IHtmlHelper helper,
            string searchurl, string contenturipattern,
            string calcParams, string nodeName, string oldId)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                StylesheetHelper styleHelper = new StylesheetHelper();
                result.WriteLine(styleHelper
                    .WriteViewLinks(searchurl, contenturipattern,
                    calcParams, nodeName, oldId));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteCurrentVersion(this IHtmlHelper helper)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(
                    "Current version: DevTreks.2.0.0, August 31, 2016");
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteURI(this IHtmlHelper helper,
            string uriFull)
        {
            using (StringWriter result = new StringWriter())
            {
                string[] arrURIParts = uriFull.Split(GeneralHelpers.WEBFILE_PATH_DELIMITERS);
                if (arrURIParts != null)
                {
                    string sSubURI = string.Empty;
                    for (int i = 0; i < arrURIParts.Length; i++)
                    {
                        if (i <= 4)
                        {
                            sSubURI += string.Concat(arrURIParts[i], GeneralHelpers.WEBFILE_PATH_DELIMITER);
                            if (i == 4)
                            {
                                result.WriteLine(sSubURI);
                                result.Write("<br/>");
                                sSubURI = string.Empty;
                            }
                        }
                        else
                        {
                            if (i <= 10)
                            {
                                sSubURI += string.Concat(arrURIParts[i], GeneralHelpers.WEBFILE_PATH_DELIMITER);
                            }
                        }
                    }
                    result.WriteLine(sSubURI);
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeDisplayURIMenu(this IHtmlHelper helper,
            ContentURI uri)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, "ui-body ui-body-b", "controlgroup", "horizontal"));
                string sContentURIPattern = string.Empty;
                string sStartSubActionView = uri.URIDataManager.SubActionView;
                string sDataIcon = string.Empty;
                //go back to stepzero when display is changed (need to change formels)
                //GeneralHelpers.AddFormInput(uri, DevTreks.Data.Helpers.AddInHelper.STEPLAST, "stepzero");
                //make a mobile view button
                string sSubActionView = GeneralHelpers.SUBACTION_VIEWS.graph.ToString();
                if (sStartSubActionView == sSubActionView
                    || sStartSubActionView == string.Empty
                    || sStartSubActionView == GeneralHelpers.NONE)
                {
                    sDataIcon = "check";
                }
                sContentURIPattern = GeneralHelpers
                    .MakeContentURIPattern(
                    uri.URIDataManager.ControllerName,
                    uri.URIDataManager.ServerActionType.ToString(),
                    uri.URIPattern,
                    uri.URIDataManager.ServerSubActionType.ToString(),
                    sSubActionView,
                    uri.URIDataManager.Variable);
                //this style tells jquery to change to stepzero as well
                string sClass1 = "ChangeView";
                helper.InputUnobtrusiveMobile(
                    string.Concat(GeneralHelpers.SUBACTION_VIEWS.graph.ToString(), uri.URIId),
                    sClass1, "button", sContentURIPattern,
                    GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    uri.URIDataManager.CalcParams,
                    AppHelper.GetResource("MEDIA"), "true", "true", sDataIcon, string.Empty)
                    .WriteTo(result, HtmlEncoder.Default);
                sDataIcon = string.Empty;
                //make a mobile view button
                sSubActionView = GeneralHelpers.SUBACTION_VIEWS.mobile.ToString();
                if (sStartSubActionView == sSubActionView)
                {
                    sDataIcon = "check";
                }
                sContentURIPattern = GeneralHelpers
                    .MakeContentURIPattern(
                    uri.URIDataManager.ControllerName,
                    uri.URIDataManager.ServerActionType.ToString(),
                    uri.URIPattern,
                    uri.URIDataManager.ServerSubActionType.ToString(),
                    sSubActionView,
                    uri.URIDataManager.Variable);
                helper.InputUnobtrusiveMobile(
                    string.Concat(GeneralHelpers.SUBACTION_VIEWS.mobile.ToString(), uri.URIId),
                    sClass1, "button", sContentURIPattern,
                    GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    uri.URIDataManager.CalcParams,
                    AppHelper.GetResource("MOBILE"), "true", "true",
                    sDataIcon, string.Empty)
                    .WriteTo(result, HtmlEncoder.Default);
                //make a full view button
                sDataIcon = string.Empty;
                sSubActionView = GeneralHelpers.SUBACTION_VIEWS.full.ToString();
                if (sStartSubActionView == sSubActionView)
                {
                    sDataIcon = "check";
                }
                sContentURIPattern = GeneralHelpers
                    .MakeContentURIPattern(
                    uri.URIDataManager.ControllerName,
                    uri.URIDataManager.ServerActionType.ToString(),
                    uri.URIPattern,
                    uri.URIDataManager.ServerSubActionType.ToString(),
                    sSubActionView,
                    uri.URIDataManager.Variable);
                helper.InputUnobtrusiveMobile(
                    string.Concat(GeneralHelpers.SUBACTION_VIEWS.full.ToString(), uri.URIId),
                    sClass1, "button", sContentURIPattern,
                    GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    uri.URIDataManager.CalcParams,
                    AppHelper.GetResource("FULL"), "true", "true",
                    sDataIcon, string.Empty)
                    .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString NetworkList(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.Write(helper.DivStart(string.Empty, string.Empty));
                result.Write(helper.LabelHidden("lstNetworks",
                    AppHelper.GetResource("SEARCH_NETWORK"), "ui-hidden-accessible"));
                result.Write(helper.SelectStart(
                    GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "lstNetworks", "lstNetworks", string.Empty));
                result.Write(helper.Option(
                    AppHelper.GetResource("NETWORKSEARCH_ALLNETWORKS"), "0", false));
                if (model.SearchManagerData.Network != null)
                {
                    foreach (Network network in model.SearchManagerData.Network)
                    {
                        result.Write(helper.Option(
                            network.NetworkName, network.PKId.ToString(),
                            network.IsDefault));
                    }
                }
                result.Write(helper.SelectEnd());
                result.Write(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString NetworkFilterSelectList(
            this IHtmlHelper helper, DevTreks.ViewModels.SearchViewModel model,
            DataAppHelpers.Networks.NETWORK_FILTER_TYPES networkFilterType,
            string selectListId, string selectCssClass)
        {
            using (StringWriter result = new StringWriter())
            {
                //default search is through own service agreement
                result.Write(helper.DivStart(string.Empty, string.Empty));
                result.Write(helper.LabelHidden("lstNetworkFilter",
                    AppHelper.GetResource("SEARCH_NETWORK"),
                    "ui-hidden-accessible"));
                result.Write(helper.SelectStart(
                    GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "lstNetworkFilter", "lstNetworkFilter", string.Empty));
                bool bIsSelected = (networkFilterType == DataAppHelpers.Networks.NETWORK_FILTER_TYPES.myagreement
                    || networkFilterType == DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none) ? true : false;
                result.Write(helper.Option(AppHelper.GetResource("NETWORKSEARCH_SERVICES"),
                    DataAppHelpers.Networks.NETWORK_FILTER_TYPES.myagreement.ToString(), bIsSelected));
                bIsSelected = (networkFilterType == DataAppHelpers.Networks.NETWORK_FILTER_TYPES.mynetworks) ? true : false;
                result.Write(helper.Option(AppHelper.GetResource("NETWORKSEARCH_NETWORKS"),
                   DataAppHelpers.Networks.NETWORK_FILTER_TYPES.mynetworks.ToString(), bIsSelected));
                bIsSelected = (networkFilterType == DataAppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworksbygroup) ? true : false;
                result.Write(helper.Option(AppHelper.GetResource("NETWORKSEARCH_ALLNETWORKSBYGROUP"),
                    DataAppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworksbygroup.ToString(), bIsSelected));
                bIsSelected = (networkFilterType == DataAppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworks) ? true : false;
                result.Write(helper.Option(AppHelper.GetResource("NETWORKSEARCH_ALLNETWORKS"),
                    DataAppHelpers.Networks.NETWORK_FILTER_TYPES.allnetworks.ToString(), bIsSelected));
                result.Write(helper.SelectEnd());
                result.Write(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString CategoryList(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.SearchManagerData.SearchTypes != null)
                {
                    result.Write(helper.LabelHidden("lstSearchTypeFilter",
                        AppHelper.GetResource("SEARCH_CATEGORY"), "ui-hidden-accessible"));
                    result.Write(helper.SelectStart(
                        GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lstSearchTypeFilter", "lstSearchTypeFilter",
                        string.Empty));
                    //category used to classify data
                    bool bIsAdminApp = GeneralHelpers.IsAdminApp(
                        model.SearchManagerData.SearchResult.URIDataManager.AppType);
                    if (!bIsAdminApp)
                    {
                        foreach (SearchManager.SearchType searchType
                            in model.SearchManagerData.SearchTypes)
                        {
                            if (searchType.IsSelected == false)
                            {
                                result.Write(helper.Option(searchType.Name,
                                    searchType.Id.ToString(), false));
                            }
                            else
                            {
                                result.Write(helper.Option(searchType.Name,
                                    searchType.Id.ToString(), true));
                            }
                        }
                    }
                    else
                    {
                        result.Write(helper.Option("All Categories",
                                    "0", true));
                    }
                    result.Write(helper.SelectEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeSearchResultLinks(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.SearchManagerData.SearchResults != null)
                {
                    int iSearchCount = 0;
                    string sParentId = string.Empty;
                    string sParentName = string.Empty;
                    string sParentNodeName = string.Empty;
                    string sParentNetwork = string.Empty;
                    string sParentFileExtType = string.Empty;
                    string sContentURIPattern = string.Empty;
                    string sImgHTML = string.Empty;
                    foreach (var uriGroup in model.SearchManagerData.SearchResults)
                    {
                        int i = 0;
                        result.Write(helper.ULStart(
                            "ulSearch", string.Empty,
                            "listview", "true",
                            "a"));
                        foreach (DevTreks.Data.ContentURI uri in uriGroup)
                        {
                            string sId = string.Empty;
                            DevTreks.Data.ContentURI.GetURIParams(uri.URIDataManager.ParentURIPattern,
                                out sParentName, out sParentId, out sParentNetwork,
                                out sParentNodeName, out sParentFileExtType);
                            if (i == 0)
                            {
                                sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                    model.SearchManagerData.SearchResult.URIDataManager.ControllerName,
                                    GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                                    uri.URIDataManager.ParentURIPattern,
                                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                                sId = string.Concat(sParentNodeName, sParentId);
                                result.Write(helper.LIStart("list-divider"));
                                result.Write(helper.StrongStart());
                                result.Write(helper.LinkUnobtrusiveMobile(id: sId,
                                    href: "#",
                                    classAttribute: "JSLink",
                                    text: sParentName,
                                    contenturipattern: sContentURIPattern,
                                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                    extraParams: string.Empty,
                                    dataRole: "button",
                                    dataMini: "true",
                                    dataInline: string.Empty,
                                    dataIcon: "arrow-r",
                                    dataIconPos: "right"));
                                result.Write(helper.StrongEnd());
                                result.Write(helper.LIEnd());
                            }
                            sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                model.SearchManagerData.SearchResult.URIDataManager.ControllerName,
                                GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                                uri.URIPattern,
                                GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                                GeneralHelpers.NONE, GeneralHelpers.NONE);
                            sId = string.Concat(uri.URINodeName, uri.URIId.ToString());
                            result.Write(helper.LIStart(string.Empty));
                            //search results look askew when images included in mobile *@
                            result.Write(helper.LinkUnobtrusiveMobile(id: sId,
                                href: "#",
                                classAttribute: "JSLink",
                                text: string.Concat("<p>", "<strong>", uri.URIName, "</strong>", "</p>", "<p>", @uri.URIDataManager.Description, "</p>"),
                                contenturipattern: sContentURIPattern,
                                clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                extraParams: string.Empty,
                                dataRole: string.Empty,
                                dataMini: "true",
                                dataInline: "true",
                                dataIcon: "arrow-r",
                                dataIconPos: "right"));
                            result.Write(helper.LIEnd());
                            i++;
                            iSearchCount = i;
                        }
                        result.Write(helper.ULEnd());
                    }
                    if (iSearchCount == 0)
                    {
                        result.Write("<br/>");
                        result.Write(
                            @AppHelper.GetResource("NO_SEARCH_RESULTS"));
                        result.Write("<br/>");
                        result.Write("<br/>");
                    }
                }
                else
                {
                    result.Write(helper.H3(
                        @AppHelper.GetResource("SEARCH_DO"), string.Empty));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString RelatedServiceList(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.Write(helper.DivStart(string.Empty, "ui-field-contain"));
                result.Write(helper.LabelRegular("lstRelatedServices",
                    AppHelper.GetResource("SERVICE_FILTER")));
                result.Write(helper.SelectStart(
                        GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lstRelatedService", "lstRelatedServices",
                        string.Empty));
                result.Write(helper.Option(AppHelper.GetResource("SERVICES_USE_ALL"),
                    "0", false));
                if (model.SearchManagerData.RelatedService != null)
                {
                    foreach (AccountToService service in model.SearchManagerData.RelatedService)
                    {
                        result.Write(helper.Option(service.Service.ServiceName,
                            service.PKId.ToString(), service.IsSelected));
                    }
                }
                result.Write(helper.SelectEnd());
                result.Write(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString LoadURILinks(this IHtmlHelper helper,
            ContentURI uri)
        {
            using (StringWriter result = new StringWriter())
            {
                result.Write("<br/>");
                result.Write(helper.DivStart(string.Empty,
                    "footer-docs", "footer", "b"));
                if (uri.URIDataManager.ServerActionType
                    == GeneralHelpers.SERVER_ACTION_TYPES.preview)
                {
                    result.Write("<p>");
                    result.Write(helper.StrongStart());
                    result.Write(@AppHelper.GetResource("IRI_DEVTREKS"));
                    result.Write(":");
                    result.Write(helper.StrongEnd());
                    result.Write("<br/>");
                    result.Write(uri.URIFull);
                    result.Write("</p>");
                    //seo
                    @HtmlHelperExtensions.MakePreviewLinks(helper, uri);
                }
                else
                {
                    result.Write("<p>");
                    result.Write(helper.StrongStart());
                    result.Write(@AppHelper.GetResource("IRIS_SEARCH"));
                    result.Write(":");
                    result.Write(helper.StrongEnd());
                    result.Write("<br/>");
                    result.Write(uri.URIFull);
                    result.Write("</p>");
                }
                result.Write(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteLoadURILink(this IHtmlHelper helper,
            ContentURI model, AccountToMember loggedInMember)
        {
            using (StringWriter result = new StringWriter())
            {
                if (loggedInMember != null)
                {
                    if (!string.IsNullOrEmpty(model.URIMember.URIFull))
                    {
                        var oldnetworkgroup = GeneralHelpers.GetSubstringFromFront(model.URIMember.URIFull,
                           GeneralHelpers.WEBFILE_PATH_DELIMITERS, 4);
                        var newnetworkgroup = GeneralHelpers.GetSubstringFromFront(model.URIDataManager.WebPath,
                           GeneralHelpers.WEBFILE_PATH_DELIMITERS, 4);
                        //var newnetworkgroup = GeneralHelpers.GetSubstringFromFront(this.Context.Request.Url.AbsoluteUri,
                        //   GeneralHelpers.WEBFILE_PATH_DELIMITERS, 4);
                        var oldhost = GeneralHelpers.GetSubstringFromFront(model.URIMember.URIFull,
                           GeneralHelpers.WEBFILE_PATH_DELIMITERS, 3);
                        //development server can have a different host from domain host
                        var newhost = GeneralHelpers.GetSubstringFromFront(model.URIDataManager.DefaultWebDomain,
                           GeneralHelpers.WEBFILE_PATH_DELIMITERS, 3);
                        var action = GeneralHelpers.GetSubstringFromFront(model.URIMember.URIFull,
                           GeneralHelpers.WEBFILE_PATH_DELIMITERS, 5);
                        if (!string.IsNullOrEmpty(oldnetworkgroup)
                            && !string.IsNullOrEmpty(oldhost)
                            && !string.IsNullOrEmpty(action))
                        {
                            if (!string.IsNullOrEmpty(newnetworkgroup)
                                && !string.IsNullOrEmpty(newhost))
                            {
                                var sNewRequestPath = model.URIMember.URIFull
                                .Replace(action, GeneralHelpers.SERVER_ACTION_TYPES.member.ToString());
                                sNewRequestPath = sNewRequestPath.Replace(oldnetworkgroup, newnetworkgroup);
                                //can't use www.devtreks.org in localhost or for development web server testing
                                sNewRequestPath = sNewRequestPath.Replace(oldhost, newhost);
                                result.Write(helper.LinkMobile("reloadmember",
                                    sNewRequestPath, string.Empty, AppHelper.GetResource("LOAD_URI"),
                                    "button", "true", "true", "refresh", "left"));
                            }
                            else
                            {
                                var sNewRequestPath = model.URIMember.URIFull;
                                result.Write(helper.LinkMobile("reloadmember",
                                   sNewRequestPath, string.Empty, AppHelper.GetResource("LOAD_URI"),
                                   "button", "true", "true", "refresh", "left"));
                            }
                        }
                        else
                        {
                            result.Write(DevTreks.Exceptions.DevTreksErrors.GetMessage("MEMBERVIEW_BADURIFULL"));
                        }
                    }
                    else
                    {
                        result.Write(DevTreks.Exceptions.DevTreksErrors.GetMessage("MEMBERVIEW_BADURIFULL"));
                    }
                }
                else
                {
                    @result.Write(DevTreks.Exceptions.DevTreksErrors.GetMessage("MEMBERVIEW_BADURIFULL"));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteAccountToMemberCount(this IHtmlHelper helper,
            ContentURI model, AccountToMember member)
        {
            using (StringWriter result = new StringWriter())
            {
                if (member.ClubDefault.AccountToMember != null)
                {
                    result.Write(member.ClubDefault.AccountToMember.Count);
                }
                else
                {
                    result.Write(DevTreks.Exceptions.DevTreksErrors.GetMessage("MEMBERVIEW_BADDEFAULTCLUB"));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteOwnedServiceCount(this IHtmlHelper helper,
            ContentURI uri, AccountToMember member)
        {
            using (StringWriter result = new StringWriter())
            {
                int iOwnedServiceCount = 0;
                if (member.ClubDefault.AccountToService != null)
                {
                    foreach (AccountToService ownservice in member.ClubDefault.AccountToService)
                    {
                        if (ownservice.IsOwner == true)
                        {
                            iOwnedServiceCount += 1;
                        }
                    }
                    result.Write(iOwnedServiceCount);
                }
                else
                {
                    result.Write(DevTreks.Exceptions.DevTreksErrors.GetMessage("MEMBERVIEW_BADDEFAULTCLUB"));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteSubscribedServiceCount(this IHtmlHelper helper,
            ContentURI uri, AccountToMember member)
        {
            using (StringWriter result = new StringWriter())
            {
                int iSubscribedServiceCount = 0;
                if (member.ClubDefault.AccountToService != null)
                {
                    foreach (AccountToService subservice in member.ClubDefault.AccountToService)
                    {
                        if (subservice.IsOwner == false)
                        {
                            iSubscribedServiceCount += 1;
                        }
                    }
                    result.Write(iSubscribedServiceCount);
                }
                else
                {
                    result.Write(DevTreks.Exceptions.DevTreksErrors.GetMessage("MEMBERVIEW_BADDEFAULTCLUB"));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeMemberLink(this IHtmlHelper helper,
            ContentURI model, AccountToMember member,
            string title, bool isBase)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(member.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(member.URIFull, 3);
                }
                string sMemberId = member.PKId.ToString();
                string sMemberNodeName = DataAppHelpers.Members.MEMBER_TYPES.member.ToString();
                if (isBase)
                {
                    sMemberId = member.MemberId.ToString();
                    sMemberNodeName = DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString();
                }
                string sMemberURIPattern = GeneralHelpers.MakeURIPattern(member.Member.MemberLastName,
                    sMemberId, sNetworkPartName, sMemberNodeName, string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sMemberURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("member", member.MemberId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: title,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: "button",
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeClubLink(this IHtmlHelper helper,
            ContentURI model, Account club, string clubName)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(club.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(club.URIFull, 3);
                }
                string sClubURIPattern = GeneralHelpers.MakeURIPattern(club.AccountName,
                    club.PKId.ToString(), sNetworkPartName,
                    DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString(), string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sClubURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("club", club.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: clubName,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeClubPaymentHandlingLink(this IHtmlHelper helper,
            ContentURI model, Account club)
        {
            using (StringWriter result = new StringWriter())
            {
                if (club.PKId == 0)
                {
                    result.Write(AppHelper.GetResource("CLUBDEFAULT_NOT_SELECTED"));
                }
                else
                {
                    //model state sets networkpartname when full uripaths are set
                    string sNetworkPartName = model.URINetworkPartName;
                    if (!string.IsNullOrEmpty(club.URIFull))
                    {
                        sNetworkPartName = ContentURI.GetFullURIPathPart(club.URIFull, 3);
                    }
                    string sClubURIPattern = GeneralHelpers.MakeURIPattern(club.AccountName,
                        club.PKId.ToString(), sNetworkPartName,
                       DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString(), string.Empty);
                    string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                        sClubURIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("club", club.PKId.ToString()),
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("CLUB_PAYMENT_HANDLING"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty,
                        dataRole: string.Empty,
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: string.Empty,
                        dataIconPos: string.Empty));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeClubPaymentHistoryLink(this IHtmlHelper helper,
            ContentURI model, AccountToMember loggedInMember)
        {
            using (StringWriter result = new StringWriter())
            {
                if (loggedInMember.ClubDefault.PKId == 0)
                {
                    //don't make a link but don't need another error message
                }
                else
                {
                    //model state sets networkpartname when full uripaths are set
                    string sNetworkPartName = model.URINetworkPartName;
                    if (!string.IsNullOrEmpty(loggedInMember.URIFull))
                    {
                        sNetworkPartName
                         = ContentURI.GetFullURIPathPart(loggedInMember.URIFull, 3);
                    }
                    string sMemberURIPattern = GeneralHelpers.MakeURIPattern(loggedInMember.Member.MemberLastName,
                        loggedInMember.MemberId.ToString(), sNetworkPartName,
                       DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString(), string.Empty);
                    //which list will be loaded?
                    string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       model.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                       sMemberURIPattern,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString(),
                       AccountToPayment.PaymentList, GeneralHelpers.NONE);
                    result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("memberclubs", loggedInMember.PKId.ToString()),
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("CLUB_PAYMENT_HISTORY"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty,
                        dataRole: string.Empty,
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: string.Empty,
                        dataIconPos: string.Empty));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeAccountToMemberLink(this IHtmlHelper helper,
            ContentURI model, Account currentClub)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(currentClub.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(currentClub.URIFull, 3);
                }
                string sMemberURIPattern = GeneralHelpers.MakeURIPattern(currentClub.AccountName,
                    currentClub.PKId.ToString(), sNetworkPartName,
                    DataAppHelpers.Members.MEMBER_TYPES.memberaccountgroup.ToString(), string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sMemberURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("clubmember", currentClub.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: AppHelper.GetResource("CLUB_MEMBERS_VIEW"),
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeClubNetworkLink(this IHtmlHelper helper,
            ContentURI model, Account currentClub)
        {
            using (StringWriter result = new StringWriter())
            {
                string sNetworkName = string.Empty;
                if (currentClub.AccountToNetwork != null)
                {
                    if (currentClub.AccountToNetwork.Count > 0)
                    {
                        AccountToNetwork defaultNetwork = currentClub.AccountToNetwork.FirstOrDefault(
                            n => n.IsDefaultNetwork == true && n.PKId != 0);
                        if (defaultNetwork != null)
                        {
                            sNetworkName = defaultNetwork.Network.NetworkName;
                        }
                        else
                        {
                            sNetworkName = AppHelper.GetResource("NETWORKS_NOT_SELECTEDYET");
                        }
                    }
                    else
                    {
                        sNetworkName = AppHelper.GetResource("NETWORKS_NOT_SELECTEDYET");
                    }
                }
                else
                {
                    sNetworkName = AppHelper.GetResource("NETWORKS_NOT_SELECTEDYET");
                }
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(currentClub.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(currentClub.URIFull, 3);
                }
                string sNetworkURIPattern = GeneralHelpers.MakeURIPattern(currentClub.AccountName,
                    currentClub.PKId.ToString(), sNetworkPartName,
                    DataAppHelpers.Networks.NETWORK_TYPES.networkaccountgroup.ToString(), string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sNetworkURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("network", currentClub.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: sNetworkName,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeClubLocalsLink(this IHtmlHelper helper,
            ContentURI model, Account currentClub)
        {
            using (StringWriter result = new StringWriter())
            {
                string sLocalsName = string.Empty;
                if (currentClub.AccountToLocal != null)
                {
                    if (currentClub.AccountToLocal.Count > 0)
                    {
                        AccountToLocal defaultLocal = currentClub.AccountToLocal.FirstOrDefault(
                            l => l.IsDefaultLinkedView == true && l.PKId != 0);
                        if (defaultLocal != null)
                        {
                            sLocalsName = defaultLocal.LocalName;
                        }
                        else
                        {
                            sLocalsName = AppHelper.GetResource("LOCALS_NOT_SELECTED");
                        }
                    }
                    else
                    {
                        sLocalsName = AppHelper.GetResource("LOCALS_NOT_SELECTED");
                    }
                }
                else
                {
                    sLocalsName = AppHelper.GetResource("LOCALS_NOT_SELECTED");
                }
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(currentClub.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(currentClub.URIFull, 3);
                }
                string sLocalURIPattern = GeneralHelpers.MakeURIPattern(currentClub.AccountName,
                    currentClub.PKId.ToString(), sNetworkPartName,
                    DataAppHelpers.Locals.LOCAL_TYPES.localaccountgroup.ToString(), string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sLocalURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("local", currentClub.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: sLocalsName,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeClubAddInsLink(this IHtmlHelper helper,
            ContentURI model, Account currentClub)
        {
            using (StringWriter result = new StringWriter())
            {
                string sDefaultAddinName = string.Empty;
                if (currentClub.AccountToAddIn != null)
                {
                    if (currentClub.AccountToAddIn.Count > 0)
                    {
                        AccountToAddIn defaultAddIn = currentClub.AccountToAddIn.FirstOrDefault(
                            a => a.IsDefaultLinkedView == true && a.PKId != 0);
                        if (defaultAddIn != null)
                        {
                            sDefaultAddinName = defaultAddIn.LinkedView.LinkedViewName;
                        }
                        else
                        {
                            sDefaultAddinName = AppHelper.GetResource("ADDINS_NOT_SELECTED");
                        }
                    }
                    else
                    {
                        sDefaultAddinName = AppHelper.GetResource("ADDINS_NOT_SELECTED");
                    }
                }
                else
                {
                    sDefaultAddinName = AppHelper.GetResource("ADDINS_NOT_SELECTED");
                }
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(currentClub.URIFull))
                {
                    sNetworkPartName
                        = ContentURI.GetFullURIPathPart(currentClub.URIFull, 3);
                }
                string sAddInURIPattern = GeneralHelpers.MakeURIPattern(currentClub.AccountName,
                    currentClub.PKId.ToString(), sNetworkPartName,
                    DataAppHelpers.AddIns.ADDIN_TYPES.addinaccountgroup.ToString(), string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sAddInURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("addin", currentClub.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: sDefaultAddinName,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeSubscribedClubsLink(this IHtmlHelper helper,
            ContentURI model, Account currentClub)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(currentClub.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(currentClub.URIFull, 3);
                }
                string sClubURIPattern = GeneralHelpers.MakeURIPattern(currentClub.AccountName,
                    currentClub.PKId.ToString(), sNetworkPartName,
                    DataAppHelpers.Accounts.ACCOUNT_TYPES.account.ToString(), string.Empty);
                //which list will be loaded?
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sClubURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString(),
                    GeneralHelpers.SUBACTION_VIEWS.services.ToString(), GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("subscribedclubs", currentClub.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: AppHelper.GetResource("CLUB_SUBSCRIBED_CLUBS_VIEW"),
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }

        public static HtmlString MakeServiceAgreementLink(this IHtmlHelper helper,
            ContentURI model, AccountToMember loggedInMember, string title)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(loggedInMember.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(loggedInMember.URIFull, 3);
                }
                string sNodeName
                    = DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString();
                string sServiceURIPattern = GeneralHelpers.MakeURIPattern(
                    loggedInMember.ClubDefault.AccountName,
                    loggedInMember.ClubDefault.PKId.ToString(),
                    sNetworkPartName, sNodeName, string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sServiceURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("service", loggedInMember.ClubDefault.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: title,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeServiceLink(this IHtmlHelper helper,
            ContentURI model, AccountToService service, bool needsGroup, string title)
        {
            using (StringWriter result = new StringWriter())
            {
                //networkpartnames can be numbers or names, 
                //the service model uses networkid
                string sNetworkPartName = service.Service.NetworkId.ToString();
                //accounttoservice.pkid
                string sId = service.PKId.ToString();
                string sNodeName = DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString();
                string sName = service.Name;
                if (needsGroup == true)
                {
                    sNetworkPartName = model.URINetworkPartName;
                    sId = service.AccountId.ToString();
                    sNodeName = DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString();
                    sName = model.URIName;
                }
                string sServiceURIPattern = GeneralHelpers.MakeURIPattern(sName,
                    sId, sNetworkPartName, sNodeName, string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                    sServiceURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("service", sId),
                    href: "#",
                    classAttribute: "JSLink",
                    text: title,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeMemberClubsLink(this IHtmlHelper helper,
            ContentURI model, AccountToMember loggedInMember)
        {
            using (StringWriter result = new StringWriter())
            {
                //2.0.0 refactor -same ui as everything else (no separate lists)
                if (loggedInMember.Member.AccountToMember != null)
                {
                    if (loggedInMember.Member.AccountToMember.Count > 0)
                    {
                        //model state sets networkpartname when full uripaths are set
                        string sNetworkPartName = model.URINetworkPartName;
                        if (!string.IsNullOrEmpty(loggedInMember.URIFull))
                        {
                            sNetworkPartName
                                = ContentURI.GetFullURIPathPart(loggedInMember.URIFull, 3);
                        }
                        //convenient to use memberbasegroup for MembersModelHelper -similar pattern to accounttonetworks ...
                        string sMemberURIPattern = GeneralHelpers.MakeURIPattern(loggedInMember.Member.MemberLastName,
                            loggedInMember.MemberId.ToString(), sNetworkPartName,
                           DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbasegroup.ToString(), string.Empty);
                        string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                            sMemberURIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.Write(helper.LinkUnobtrusiveMobile(
                            id: string.Concat("memberclubs", loggedInMember.PKId.ToString()),
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("MEMBER_CLUBS_EDIT"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: string.Empty,
                            dataRole: string.Empty,
                            dataMini: "true",
                            dataInline: "true",
                            dataIcon: string.Empty,
                            dataIconPos: string.Empty));
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString MakeClubInsertNewLink(this IHtmlHelper helper,
            ContentURI model, AccountToMember loggedInMember)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full uripaths are set
                string sNetworkPartName = model.URINetworkPartName;
                if (!string.IsNullOrEmpty(loggedInMember.URIFull))
                {
                    sNetworkPartName = ContentURI.GetFullURIPathPart(loggedInMember.URIFull, 3);
                }
                string sMemberURIPattern = GeneralHelpers.MakeURIPattern(loggedInMember.Member.MemberLastName,
                        loggedInMember.MemberId.ToString(), sNetworkPartName,
                        DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString(), string.Empty);
                //which form will be loaded?
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sMemberURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithform.ToString(),
                    Account.MemberClubList, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(
                    id: string.Concat("insertnewclub", loggedInMember.MemberId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: AppHelper.GetResource("MEMBER_INSERT_NEWCLUB_VIEW"),
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString DisplayRESTAddress(this IHtmlHelper helper,
            ContentURI model, AccountToAudit audit)
        {
            using (StringWriter result = new StringWriter())
            {
                string sURIPattern = ContentURI.GetURIPatternFromContentURIPattern(audit.EditedDocURI);
                string sAuthority = audit.EditedDocFullPath.Replace(sURIPattern, string.Empty);
                result.Write(sAuthority);
                //put spaces in so uri displays in list properly
                result.Write(" {linebreak}");
                result.Write("<br/>");
                result.Write(sURIPattern);
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeURILink(this IHtmlHelper helper,
            ContentURI model, AccountToAudit audit, string title)
        {
            using (StringWriter result = new StringWriter())
            {
                string sContentURIPattern =
            ContentURI.ChangeContentURIPatternPart(audit.EditedDocURI,
            ContentURI.CONTENTURIPATTERNPART.subaction, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("editeduris", audit.PKId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: "View uri",
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: string.Empty,
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString SearchBody1(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model,
            Microsoft.AspNetCore.Http.HttpContext context)
        {
            //HtmlTextWriter htw;
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(
                    string.Empty, "ui-field-contain").ToString());
                result.WriteLine(helper.FieldsetStart(
                    string.Empty, string.Empty, "controlgroup",
                    string.Empty, string.Empty).ToString());
                result.WriteLine(helper.Legend(
                    "Keyword, Service, Network, Category").ToString());
                result.WriteLine(helper.LabelHidden(
                    "keywords", AppHelper.GetResource("SEARCH_KEYWORDS"),
                    "ui-hidden-accessible").ToString());
                result.WriteLine(helper.InputMobile(GeneralHelpers.VIEW_EDIT_TYPES.full,
                    "keywords", string.Empty, "text", string.Empty,
                    "keywords", model.SearchManagerData.Keywords, "true",
                    "true", string.Empty, string.Empty).ToString());
                result.WriteLine(helper.LabelHidden(
                    "lstServiceGroupFilter", AppHelper.GetResource("SEARCH_SERVICE"),
                    "ui-hidden-accessible").ToString());
                if (model.SearchManagerData.ServiceGroups != null)
                {
                    //service groups = subapplications (i.e. inputs, outputs ...)
                    result.WriteLine(helper.SelectStart(GeneralHelpers.VIEW_EDIT_TYPES.full,
                        "lstServiceGroupFilter", "lstServiceGroupFilter",
                        string.Empty).ToString());
                    foreach (ServiceClass serviceGroup
                        in model.SearchManagerData.ServiceGroups)
                    {
                        if (serviceGroup.IsSelected == false)
                        {
                            result.WriteLine(helper.Option(serviceGroup.ServiceClassName,
                                serviceGroup.ServiceClassNum, false).ToString());
                        }
                        else
                        {
                            result.WriteLine(helper.Option(serviceGroup.ServiceClassName,
                                serviceGroup.ServiceClassNum, true).ToString());
                        }
                    }
                    result.WriteLine(helper.SelectEnd());
                }
                if (model.SearchManagerData.Network != null
                    && model.SearchManagerData.SearchResult.URIDataManager.AppType
                    != GeneralHelpers.APPLICATION_TYPES.networks)
                {
                    if (model.SearchManagerData.SearchResult.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.accounts
                        || model.SearchManagerData.SearchResult.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.members)
                    {
                        HtmlHelperExtensions.NetworkList(helper, model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                    else
                    {
                        if (!HtmlMemberExtensions.UserIsLoggedIn(context))
                        {
                            HtmlHelperExtensions.NetworkList(helper, model)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                        else
                        {
                            HtmlHelperExtensions.NetworkFilterSelectList(
                                helper, model,
                                model.SearchManagerData.NetworkType,
                                "lstNetworkFilter", "Select225")
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                    }
                }
                int iSearchVar = -1;
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.SearchManagerData.SearchResult.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.search.ToString(),
                    model.SearchManagerData.SearchResult.URIPattern,
                    model.SearchManagerData.SearchResult.URIDataManager.ServerSubActionType.ToString(),
                    model.SearchManagerData.SearchResult.URIDataManager.SubActionView,
                    iSearchVar.ToString());
                result.Write(HtmlHelperExtensions.CategoryList(helper, model));
                result.WriteLine(helper.InputUnobtrusiveMobile(id: "startsearch",
                    classAttribute: "Search1Enabled",
                    type: "button",
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    value: @AppHelper.GetResource("SEARCH_START"),
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: "search",
                    dataIconPos: "left"));
                result.WriteLine(helper.FieldsetEnd());
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString SearchBody2(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model)
        {
            using (StringWriter result = new StringWriter())
            {
                model.SearchManagerData.SearchResult.URIDataManager.ServerActionType
                    = GeneralHelpers.SERVER_ACTION_TYPES.search;
                model.SearchManagerData.SearchResult.UpdateContentURIPattern();
                MakeHorizStartAtRowButtons(helper,
                    model.SearchManagerData.SearchResult,
                    model.SearchManagerData.StartRow,
                    model.SearchManagerData.PageSize,
                    model.SearchManagerData.RowCount,
                    model.SearchManagerData.SearchResult.URIDataManager.ParentStartRow,
                    true).
                    WriteTo(result, HtmlEncoder.Default);
                //2.0.0 deprecated -takes up unnecessary space
                //result.Write(HtmlHelperExtensions.FinishPagination(helper, model,
                //    model.SearchManagerData.SearchResult.URINetwork.NetworkName));
                if (!(string.IsNullOrEmpty(model.SearchManagerData.SearchResult.ErrorMessage)))
                {
                    result.WriteLine(helper.DivStart(
                        string.Empty, string.Empty).ToString());
                    result.WriteLine(helper.SpanError(
                        model.SearchManagerData.SearchResult.ErrorMessage));
                    model.SearchManagerData.SearchResult.ErrorMessage = string.Empty;
                    result.WriteLine(helper.DivEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString SearchBody3(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model,
            Microsoft.AspNetCore.Http.HttpContext context)
        {
            using (StringWriter result = new StringWriter())
            {
                //list of search results
                result.WriteLine(helper.DivStart(
                    "subnav", string.Empty).ToString());
                result.Write(HtmlHelperExtensions.MakeSearchResultLinks(helper, model));
                result.WriteLine(helper.DivEnd());
                //individual network filter
                if (HtmlMemberExtensions.UserIsLoggedIn(context))
                {
                    if (model.SearchManagerData.Network != null
                        && model.SearchManagerData.SearchResult.URIDataManager.AppType
                        != GeneralHelpers.APPLICATION_TYPES.networks
                        && model.SearchManagerData.SearchResult.URIDataManager.AppType
                        != GeneralHelpers.APPLICATION_TYPES.members
                        && model.SearchManagerData.SearchResult.URIDataManager.AppType
                        != GeneralHelpers.APPLICATION_TYPES.accounts)
                    {
                        result.Write(HtmlHelperExtensions.NetworkList(
                            helper, model));
                    }
                }
                //related service filter
                if (model.SearchManagerData.RelatedService != null)
                {
                    result.Write(HtmlHelperExtensions.RelatedServiceList(
                        helper, model));
                }
                result.WriteLine(helper.DivStart(
                   string.Empty, "footer-docs", "footer",
                   "a", string.Empty, string.Empty, "true").ToString());
                if (model.SearchManagerData.SearchResult.URIDataManager.ServerSubActionType
                    != GeneralHelpers.SERVER_SUBACTION_TYPES.uploadfile)
                {
                    result.Write(HtmlHelperExtensions.SearchFooter(helper, model));
                }
                int iNetworkId = (model.SearchManagerData.SearchResult.URINetwork != null)
                    ? model.SearchManagerData.SearchResult.URINetwork.PKId : 0;
                int iServiceId = (model.SearchManagerData.SearchResult.URIService != null)
                    ? model.SearchManagerData.SearchResult.URIService.PKId : 0;
                //hidden input elements for resetting searchs
                result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full, "oldNetworkId", string.Empty, "hidden",
                    string.Empty, "oldNetworkId", iNetworkId.ToString()));
                result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full, "oldNetworkType", string.Empty, "hidden",
                    string.Empty, "oldNetworkType", model.SearchManagerData.NetworkType.ToString()));
                result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full, "oldNodeName", string.Empty, "hidden",
                    string.Empty, "oldNodeName", model.SearchManagerData.SearchResult.URINodeName));
                result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full, "oldServiceId", string.Empty, "hidden",
                    string.Empty, "oldServiceId", iServiceId.ToString()));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString SearchFooter(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(
                    "NetworkGroups", "ui-field-contain").ToString());
                if (model.SearchManagerData.NetworkGroups != null
                    && model.SearchManagerData.SearchResult.URIDataManager.AppType
                    != GeneralHelpers.APPLICATION_TYPES.networks)
                {
                    result.WriteLine(helper.LabelStrong(
                        "networkgrouplinks", "Switch to Network Group:"));
                    foreach (NetworkClass networkgroup in model.SearchManagerData.NetworkGroups)
                    {
                        if (networkgroup.IsSelected == false)
                        {
                            result.Write("<span>");
                            HtmlHelperExtensions
                                .MakeNetworkGroupLink(helper, model,
                                networkgroup).WriteTo(result, HtmlEncoder.Default);
                            result.Write("</span>");
                        }
                        else
                        {
                            result.Write(helper.Span(
                                "editlink", string.Empty,
                                networkgroup.NetworkClassName)); }
                    }
                }
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeNetworkGroupLink(this IHtmlHelper helper,
            DevTreks.ViewModels.SearchViewModel model, NetworkClass networkGroup)
        {
            using (StringWriter result = new StringWriter())
            {
                NetworkClass existingNetworkGroup
                    = model.SearchManagerData.NetworkGroups.FirstOrDefault(
                        n => n.IsSelected == true);
                if (existingNetworkGroup != null
                    && model.SearchManagerData.SearchResult.URIFull != string.Empty)
                {
                    {
                        string uriPattern = GeneralHelpers
                            .MakeURIPattern("none", "0", "none",
                            model.SearchManagerData.SearchResult.URINodeName,
                            string.Empty);
                        helper.ActionLink(networkGroup.NetworkClassName,
                            GeneralHelpers.SERVER_ACTION_TYPES.search.ToString(),
                            routeValues:
                            new
                            {
                                controller = networkGroup.NetworkClassControllerName,
                                contenturipattern = uriPattern
                            },
                            htmlAttributes:
                            new
                            {
                                data_role = "button",
                                data_mini = "true",
                                data_inline = "true"
                            }).WriteTo(result, HtmlEncoder.Default);
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MemberNotLoggedIn(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(
                    string.Empty, string.Empty, "navbar", string.Empty));
                result.WriteLine(helper.ULStart(string.Empty, string.Empty));
                result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                helper.ActionLink(AppHelper.GetResource("JOININ_JOININ"),
                    "Register", "Account", routeValues: null,
                    htmlAttributes: new { id = "registerLink", data_role = "button", data_mini = "true", data_inline = "true" }
                    ).WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.LIEnd());
                result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                helper.ActionLink(AppHelper.GetResource("LOGIN"),
                    "Login", "Account", routeValues: null,
                    htmlAttributes: new { id = "loginLink", data_role = "button", data_mini = "true", data_inline = "true" }
                    ).WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.LIEnd());
                result.WriteLine(helper.ULEnd());
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MemberLoggedIn(this IHtmlHelper helper,
            ContentURI model, Microsoft.AspNetCore.Http.HttpContext context)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart("divEditAccount", string.Empty));
                if (model.URIMember != null && HtmlMemberExtensions.UserIsLoggedIn(context))
                {
                    result.Write("<br/>");
                    result.Write("<br/>");
                    result.Write("<span>");
                    result.WriteLine(WriteLoadURILink(helper,
                        model, model.URIMember));
                    result.Write("<br/>");
                    result.Write(AppHelper.GetResource("RELOAD_MEMBER"));
                    if (model.URIMember.ClubDefault != null)
                    {
                        result.Write(string.Concat("for ",
                            model.URIMember.Member.UserName));
                        result.Write(string.Concat(" in ",
                            model.URIMember.ClubDefault.AccountName));
                    }
                    result.Write("</span>");
                    result.Write("<br/>");
                    result.Write("<br/>");
                }
                result.WriteLine(helper.DivStart(string.Empty, string.Empty,
                    "navbar", string.Empty));
                result.WriteLine(helper.ULStart(string.Empty, string.Empty));
                result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                //note this is not a form postback and does not use AccountController.LogOff
                //it uses custom action AccountController.LogOut
                helper.ActionLink(AppHelper.GetResource("LOGOUT"),
                    "LogOut", "Account",
                    routeValues: null,
                    htmlAttributes: new { data_role = "button", data_mini = "true", data_inline = "true", id = "logoffLink" })
                    .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.LIEnd());
                result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                helper.ActionLink(AppHelper.GetResource("LOGIN_CHANGE"),
                    "Index", "Manage",
                    routeValues: null,
                    htmlAttributes: new { @class = "username", title = "Manage", data_role = "button", data_mini = "true", data_inline = "true", id = "loginChange" })
                    .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.LIEnd());
                result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                bool bIsBaseMember2 = true;
                HtmlHelperExtensions.MakeMemberLink(helper,
                    model, model.URIMember, AppHelper.GetResource("MEMBERSHIP_EDIT"),
                    bIsBaseMember2).WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.LIEnd());
                result.WriteLine(helper.ULEnd());
                result.WriteLine(helper.DivEnd());
                if (ContentURI.GetContentURIPatternPart(
                    model.URIDataManager.ContentURIPattern,
                    ContentURI.CONTENTURIPATTERNPART.action.ToString())
                    == "member" && HtmlMemberExtensions.UserIsLoggedIn(context))
                {
                    result.WriteLine(helper
                        .ULStart(string.Empty, string.Empty,
                        "listview", "true", string.Empty));
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write("<strong>");
                    result.Write(AppHelper.GetResource("CLUB_CURRENT"));
                    result.Write("</strong>");
                    result.WriteLine(helper.LIEnd());
                    string sClubName = string.Empty;
                    if (model.URIMember.ClubDefault.PKId == 0)
                    {
                        sClubName = AppHelper.GetResource("CLUBDEFAULT_NOT_SELECTED");
                    }
                    else
                    {
                        sClubName = model.URIMember.ClubDefault.AccountName;
                    }
                    result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                    HtmlHelperExtensions.MakeClubLink(helper, model,
                        model.URIMember.ClubDefault, sClubName)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    //2.0.0 don't show unless fully implemented
                    //result.WriteLine(helper.LIStart(string.Empty));
                    //helper.ActionLink(AppHelper.GetResource("CLUB_PAYMENT_HANDLING"), "payments", "member")
                    //    .WriteTo(result, HtmlEncoder.Default);
                    //result.WriteLine(helper.LIEnd());
                    //result.WriteLine(helper.LIStart(string.Empty));
                    //HtmlHelperExtensions.MakeClubPaymentHistoryLink(helper,
                    //    model, model.URIMember)
                    //    .WriteTo(result, HtmlEncoder.Default);
                    //result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("NETWORK_DEFAULT"));
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeClubNetworkLink(helper, model,
                        model.URIMember.ClubDefault)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.WriteLine(AppHelper.GetResource("LOCAL_DEFAULT"));
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeClubLocalsLink(helper, model,
                        model.URIMember.ClubDefault)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("ADDIN_DEFAULT"));
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeClubAddInsLink(helper, model,
                        model.URIMember.ClubDefault)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("CLUB_MEMBERS"));
                    result.Write("&nbsp;");
                    HtmlHelperExtensions.WriteAccountToMemberCount(helper, model, model.URIMember)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeAccountToMemberLink(helper, model,
                        model.URIMember.ClubDefault)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    int i = 0;
                    if (model.URIMember.ClubDefault.AccountToMember
                        != null)
                    {
                        foreach (AccountToMember member
                            in model.URIMember.ClubDefault.AccountToMember)
                        {
                            //limit display to 5 members, rest can be viewed by opening link
                            if (i == 5)
                            {
                                result.WriteLine(helper.LIStart(string.Empty));
                                HtmlHelperExtensions.MakeAccountToMemberLink(
                                    helper, model, model.URIMember.ClubDefault)
                                   .WriteTo(result, HtmlEncoder.Default);
                                result.WriteLine(helper.LIEnd());
                                break;
                            }
                            else
                            {
                                result.WriteLine(helper.LIStart(string.Empty));
                                string sMemberName = string.Concat(member.Member.MemberLastName,
                                    ", ", member.Member.MemberFirstName, ", ", member.MemberRole.ToString(), ", ", member.Member.MemberEmail);
                                HtmlHelperExtensions.MakeMemberLink(helper,
                                    model, member, sMemberName, false)
                                    .WriteTo(result, HtmlEncoder.Default);
                                result.WriteLine(helper.LIEnd());
                                i++;
                            }
                        }
                    }
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("CLUB_SERVICES"));
                    HtmlHelperExtensions.WriteOwnedServiceCount(helper, model,
                        model.URIMember).WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeServiceAgreementLink(
                       helper, model, model.URIMember,
                       AppHelper.GetResource("CLUB_AGREEMENT_VIEW"))
                       .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    i = 0;
                    bool bNeedsGroup = false;
                    if (model.URIMember.ClubDefault.AccountToService
                        != null)
                    {
                        foreach (AccountToService service
                            in model.URIMember.ClubDefault.AccountToService)
                        {
                            //limit display to 5 owned services, rest can be viewed by opening link
                            if (i == 5)
                            {
                                result.WriteLine(helper.LIStart(string.Empty));
                                bNeedsGroup = true;
                                HtmlHelperExtensions.MakeServiceLink(
                                    helper, model, service, bNeedsGroup,
                                    AppHelper.GetResource("SERVICES_VIEW_REMAINING"))
                                    .WriteTo(result, HtmlEncoder.Default);
                                result.WriteLine(helper.LIEnd());
                                break;
                            }
                            else
                            {
                                if (service.IsOwner)
                                {
                                    bNeedsGroup = false;
                                    result.WriteLine(helper.LIStart(string.Empty));
                                    HtmlHelperExtensions.MakeServiceLink(
                                        helper, model, service, bNeedsGroup,
                                        string.Concat(service.Name, " : ",
                                        service.Service.ServiceDesc))
                                        .WriteTo(result, HtmlEncoder.Default);
                                    result.WriteLine(helper.LIEnd());
                                    i++;
                                }
                            }
                        }
                    }
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeSubscribedClubsLink(
                        helper, model, model.URIMember.ClubDefault)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("CLUB_SERVICES2"));
                    HtmlHelperExtensions.WriteSubscribedServiceCount(
                        helper, model, model.URIMember)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    i = 0;
                    if (model.URIMember.ClubDefault.AccountToService
                        != null)
                    {
                        foreach (AccountToService service
                            in model.URIMember.ClubDefault.AccountToService)
                        {
                            //limit display to 5 subscribed services, rest can be viewed by opening link
                            if (i == 5)
                            {
                                result.WriteLine(helper.LIStart(string.Empty));
                                bNeedsGroup = true;
                                HtmlHelperExtensions.MakeServiceLink(
                                    helper, model, service, bNeedsGroup,
                                    AppHelper.GetResource("SERVICES_VIEW_REMAINING"))
                                    .WriteTo(result, HtmlEncoder.Default);
                                result.WriteLine(helper.LIEnd());
                                break;
                            }
                            else
                            {
                                if (service.IsOwner == false)
                                {
                                    bNeedsGroup = false;
                                    result.WriteLine(helper.LIStart(string.Empty));
                                    HtmlHelperExtensions.MakeServiceLink(
                                            helper, model, service, bNeedsGroup,
                                            string.Concat(service.Name, " : ",
                                            service.Service.ServiceDesc))
                                        .WriteTo(result, HtmlEncoder.Default);
                                    result.WriteLine(helper.LIEnd());
                                    i++;
                                }
                            }
                        }
                    }
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("MEMBER_CLUBS"));
                    result.WriteLine(helper.LIEnd());
                    i = 0;
                    if (model.URIMember.Member.AccountToMember != null)
                    {
                        foreach (AccountToMember club in model.URIMember.Member.AccountToMember)
                        {
                            sClubName = string.Concat(club.ClubDefault.AccountName, " : ");
                            if (club.IsDefaultClub)
                            {
                                sClubName += AppHelper.GetResource("CLUB_ISDEFAULT");
                            }
                            else
                            {
                                sClubName += AppHelper.GetResource("CLUB_ISNOTDEFAULT");
                            }
                            result.WriteLine(helper.LIStart(string.Empty));
                            HtmlHelperExtensions.MakeClubLink(helper, model,
                                club.ClubDefault, sClubName)
                                .WriteTo(result, HtmlEncoder.Default);
                            result.WriteLine(helper.LIEnd());
                            i++;
                            if (i == 5) break;
                        }
                    }
                    result.WriteLine(helper.LIStart(string.Empty));
                    HtmlHelperExtensions.MakeMemberClubsLink(
                        helper, model, model.URIMember)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.LIEnd());
                    result.WriteLine(helper.LIStart("list-divider"));
                    result.Write(AppHelper.GetResource("CLUB_LASTEDITS"));
                    result.WriteLine(helper.LIEnd());
                    i = 0;
                    string sUriName = string.Empty;
                    if (model.URIMember.ClubDefault.AccountToAudit != null)
                    {
                        if (model.URIMember.ClubDefault.AccountToAudit.Count == 0)
                        {
                            sUriName = AppHelper.GetResource("URIS_NOTEDITED");
                        }
                        else
                        {
                            int j = 0;
                            string uri = string.Empty;
                            foreach (AccountToAudit audit in model.URIMember.ClubDefault.AccountToAudit)
                            {
                                //limit display to 5 edits, this release doesn't 
                                //include additional audit trail features (may need another app)
                                if (j == 5)
                                {
                                    break;
                                }
                                else
                                {
                                    if (audit.EditedDocURI != uri)
                                    {
                                        result.WriteLine(helper.LIStart(string.Empty));
                                        HtmlHelperExtensions.DisplayRESTAddress(
                                            helper, model, audit)
                                            .WriteTo(result, HtmlEncoder.Default);
                                        result.WriteLine(helper.LIEnd());
                                        result.WriteLine(helper.LIStart(string.Empty));
                                        HtmlHelperExtensions.MakeURILink(
                                            helper, model, audit, sUriName)
                                            .WriteTo(result, HtmlEncoder.Default);
                                        result.WriteLine(helper.LIEnd());
                                        j++;
                                    }
                                    uri = audit.EditedDocURI;
                                }
                            }
                        }
                    }
                    result.WriteLine(helper.ULEnd());
                }
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.PStart());
                result.WriteLine(helper.StrongStart());
                result.Write("URI:");
                result.WriteLine(helper.StrongEnd());
                result.Write("<br/>");
                HtmlHelperExtensions.WriteURI(helper,
                    model.URIMember.URIFull)
                    .WriteTo(result, HtmlEncoder.Default);
                result.Write("<br/>");
                result.WriteLine(helper.PEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString AncestorLinks(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIDataManager.Ancestors != null)
                {
                    int i = 0;
                    //edit and read views keep the existing ancestors intact 
                    //(for ease of moving between children and parent nodes)
                    bool bHasCurrentURI = false;
                    string sStatefulAncestorParams
                        = StylesheetHelper.GetStatefulAncestorParams(model, ref bHasCurrentURI);
                    foreach (DevTreks.Data.ContentURI ancestor in model.URIDataManager.Ancestors)
                    {
                        if (i != 0)
                        {
                            result.Write(">");
                        }
                        if (ancestor.URIId == 0)
                        {
                            result.Write(ancestor.URIName);
                        }
                        else
                        {
                            string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                model.URIDataManager.ControllerName,
                                model.URIDataManager.ServerActionType.ToString(),
                                ancestor.URIPattern,
                                GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                                GeneralHelpers.NONE, GeneralHelpers.NONE);
                            result.WriteLine(helper.LinkUnobtrusive(id: string.Concat("ancestor", i.ToString()),
                                href: "#",
                                classAttribute: "JSLink",
                                text: ancestor.URIName,
                                contenturipattern: sContentURIPattern,
                                clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                extraParams: sStatefulAncestorParams));
                        }
                        i++;
                    }
                    if (model.URIDataManager.UpdatePanelType ==
                        GeneralHelpers.UPDATE_PANEL_TYPES.edit
                        && model.URINodeName != DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString())
                    {
                        if (bHasCurrentURI == false)
                        {
                            //the edit panel needs a final link 
                            //(can navigate among related nodes)
                            if (i != 0)
                            {
                                result.Write(">");
                                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                model.URIDataManager.ControllerName,
                                model.URIDataManager.ServerActionType.ToString(),
                                model.URIPattern,
                                model.URIDataManager.ServerSubActionType.ToString(),
                                GeneralHelpers.NONE, GeneralHelpers.NONE);
                                result.WriteLine(helper.LinkUnobtrusive(id: string.Concat("ancestor2", i.ToString()),
                                   href: "#",
                                   classAttribute: "JSLink",
                                   text: model.URIName,
                                   contenturipattern: sContentURIPattern,
                                   clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                   extraParams: sStatefulAncestorParams));
                            }
                        }
                    }
                    else
                    {
                        //the remaining panels don't need the last link
                        if (i != 0)
                        {
                            result.Write(">");
                            result.Write(model.URIName);
                        }
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString StartEditPanel(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.SpanStart("spanMoveDevTrekEdits", "SearchButtons"));
                if (model.URIDataManager.ServerActionType
                    != GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
                {
                    bool bIsSearch = false;
                    int iPageSizeEdits = GeneralHelpers.ConvertStringToInt(model.URIDataManager.PageSizeEdits);
                    MakeHorizStartAtRowButtons(helper,
                        model, model.URIDataManager.StartRow, iPageSizeEdits,
                        model.URIDataManager.RowCount, model.URIDataManager.ParentStartRow,
                        bIsSearch)
                        .WriteTo(result, HtmlEncoder.Default);
                }
                else
                {
                    //for 0.8.5 linked panel does not paginate html views
                    //but it will paginate the linkedviews in the selects lists
                    if (model.URIDataManager.AppType
                       == GeneralHelpers.APPLICATION_TYPES.resources
                       && model.URINodeName
                       != DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                    {
                        bool bIsSearch = false;
                        MakeHorizStartAtRowButtons(helper,
                            model, model.URIDataManager.StartRow, model.URIDataManager.PageSize,
                            model.URIDataManager.RowCount, model.URIDataManager.ParentStartRow,
                            bIsSearch)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                }
                result.WriteLine(helper.SpanEnd());
                //edit Model menu
                string sClass2 = string.Empty;
                if (model.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.edit)
                {
                    result.WriteLine(helper.DivStart("divEditsSection", sClass2));
                    if (model.URIDataManager.EditViewEditType
                        == GeneralHelpers.VIEW_EDIT_TYPES.full)
                    {
                        helper.EditButtons(model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                }
                else if (model.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.linkedviews)
                {
                    result.WriteLine(helper.DivStart("divLinkedViewSection", sClass2));
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString FeedbackSpan(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (string.IsNullOrEmpty(model.ErrorMessage))
                {
                    //span for giving feedback after the client submits edits, inserts selections ...
                    switch (model.URIDataManager.ServerSubActionType)
                    {
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("SELECTS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.buildtempdoc:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                                string.Concat(AppHelper.GetResource("SELECTS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.savenewselects:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("SELECTS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("DEFAULTINSERTS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("EDITS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("EDITS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.submitformedits:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("EDITS_SUCCESS"),
                            " Edited at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        case GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile:
                            result.Write("<br/>");
                            result.WriteLine(helper.Span("spanEditsFeedback", "ContentNoHighlight",
                            string.Concat(AppHelper.GetResource("EDITS_SUCCESSSAVEFILE"),
                            " Saved at: ", GeneralHelpers.GetDateUniversalNow())));
                            break;
                        default:
                            //need subserveraction
                            break;
                    }

                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString EditButtons(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.Write(helper.DivStart("divEditButtons",
                    "ui-body ui-body-b", "controlgroup", "horizontal"));
                bool bNeedsSingleQuote = false;
                string sSelectParams = StylesheetHelper.SetSelectedLinkedViewParams(model, bNeedsSingleQuote);
                string sSubAction = GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits.ToString();
                if (model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                    || model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
                {
                    //utility for simple list edits using entity framework
                    sSubAction = GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits.ToString();
                }
                else if (model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithform
                    || model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.submitformedits)
                {
                    //utility for simple form edits using entity framework
                    sSubAction = GeneralHelpers.SERVER_SUBACTION_TYPES.submitformedits.ToString();
                }
                bool bHasCurrentURI = false;
                string sStatefulAncestorParams
                    = StylesheetHelper.GetStatefulAncestorParams(model, ref bHasCurrentURI);
                DevTreks.Services.IMemberService memberService = new DevTreks.Services.MemberService(model);
                bool bIsOkToDisplay = memberService.ContentCanBeSelectedAndEdited(model);
                if (bIsOkToDisplay)
                {
                    //edits use pagination and need rowargs
                    string sPageParams = StylesheetHelper.SetRowArgs(model.URIDataManager.StartRow,
                        model.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        model.URIDataManager.ParentStartRow);
                    string sContentURIPattern = string.Empty;
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                        model.URIPattern,
                        sSubAction,
                        model.URIDataManager.SubActionView,
                        model.URIDataManager.Variable);
                    string sClass1 = "SubmitButton1Enabled150";
                    //make a submit edits button
                    result.WriteLine(helper.InputUnobtrusiveMobile(GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits.ToString(),
                        sClass1, "button", sContentURIPattern,
                        GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        string.Concat(sStatefulAncestorParams, sSelectParams, sPageParams),
                        AppHelper.GetResource("SUBMIT_EDITS"), "true", "true", string.Empty, string.Empty));
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       model.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                       model.URIPattern,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.closeedits.ToString(),
                       model.URIDataManager.SubActionView,
                       model.URIDataManager.Variable);
                    //mobile needs specific jquery action
                    sClass1 = "ResetForm";
                    //make a cancel edits button
                    result.WriteLine(helper.InputMobile(GeneralHelpers.VIEW_EDIT_TYPES.full, string.Concat(HtmlExtensions.CANCEL_EDITS, model.URIId.ToString()),
                        sClass1, "reset", string.Empty, HtmlExtensions.CANCEL_EDITS, AppHelper.GetResource("CANCEL_EDITS"),
                        "true", "true", string.Empty, string.Empty));
                    sClass1 = "SubmitButton1Enabled";
                    result.WriteLine(helper.InputUnobtrusiveMobile(GeneralHelpers.SERVER_SUBACTION_TYPES.closeedits.ToString(),
                        sClass1, "button", sContentURIPattern, GeneralHelpers.CLIENTACTION_TYPES.closeelement.ToString(),
                        string.Empty, AppHelper.GetResource("CLOSE_EDITS"), "true", "true", string.Empty, string.Empty));
                    //173 deprecated stateful file system file management for noncustom docs
                    if (!model.URIDataManager.UseSelectedLinkedView)
                    {
                        //173 deprecated state mngt in views
                    }
                }
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString EditModels(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                helper.MakeSynchDocLinks(model)
                    .WriteTo(result, HtmlEncoder.Default);
                if (!(string.IsNullOrEmpty(model.ErrorMessage)))
                {
                    result.WriteLine(helper.DivEnd());
                    result.WriteLine(helper.SpanError(model.ErrorMessage));
                    result.WriteLine(helper.DivEnd());
                }
                result.WriteLine(helper.FormStart("frmEdit", string.Empty));
                if (model.URIDataManager.SubActionView
                    != AddHelperLinq.SELECTION_OPTIONS.buildnewdocview.ToString())
                {
                    helper.StartEditPanel(model)
                        .WriteTo(result, HtmlEncoder.Default);
                }
                else
                {
                    result.WriteLine(helper.DivStart("divEditsSection", string.Empty));
                }
                if (model.URIDataManager.ServerSubActionType ==
                    GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects
                    && model.URIDataManager.SubActionView
                    == AddHelperLinq.SELECTION_OPTIONS.buildnewdocview.ToString())
                {
                    if (model.URIDataManager.SelectedList == string.Empty)
                    {
                        DevTreks.Services.Helpers.EditHelper.SetSelectsList(model);
                    }
                }
                else
                {
                    if (model.URIDataManager.UseSelectedLinkedView == false)
                    {
                        helper.Partial("AdminModelViews/_DisplayModels", model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                    else
                    {
                        bool bHasGoodSelection = helper.WriteLinkedViewSelections2(result,
                            model);
                        //keep this 100% consistent with LinkedView panel
                        if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews
                            || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                        {
                            ContentURI selectedViewURI =
                                DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(model);
                            helper.DisplayURI(selectedViewURI,
                                GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                .WriteTo(result, HtmlEncoder.Default);
                            model.ErrorMessage += selectedViewURI.ErrorMessage;
                        }
                        else
                        {
                            helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                        //write any error messages generated while trying to display the linkedviews
                        if (!string.IsNullOrEmpty(model.ErrorMessage))
                        {
                            helper.PError(model.ErrorMessage)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                    }
                }
                result.WriteLine(helper.PStart());
                result.WriteLine(string.Concat("<strong>", model.ErrorMessage, "</strong>"));
                result.WriteLine(helper.PEnd());
                //div started in _startpanel
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.FormEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString SynchDocLinks(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart("synchlinksmenu", string.Empty, "navbar", string.Empty));
                result.WriteLine(helper.ULStart(string.Empty, string.Empty));
                string sJavascriptMethodForDevTrekFull = string.Empty;
                string sContentURIPattern = string.Empty;
                bool bIsAdminApp = GeneralHelpers.IsAdminApp(model.URIDataManager.AppType);

                if (model.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.preview)
                {
                    //last panel 
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.search.ToString(),
                        model.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                    result.WriteLine(helper.LinkUnobtrusive(id: "lnkReloadSearch",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("RELOAD_SEARCH"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty));
                    result.WriteLine(helper.LIEnd());
                    if (!bIsAdminApp)
                    {
                        //load in views panel allowed default addins to be loaded (for quickly linking views)
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                            model.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                        result.WriteLine(helper.LinkUnobtrusive(id: "lnkLinkedViewLoadAddins",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("LINKEDVIEWS_SYNCH"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: string.Empty));
                        result.WriteLine(helper.LIEnd());
                    }
                    else
                    {
                    }
                    //next panel
                    //select view needs rowargs
                    string sExtraParams = StylesheetHelper.SetRowArgs(model.URIDataManager.StartRow,
                        model.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        model.URIDataManager.ParentStartRow);
                    //load in views panel allowed default addins to be loaded (for quickly linking views)
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                        model.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                    result.WriteLine(helper.LinkUnobtrusive(id: "lnkLoadSelect",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("LOAD_SELECT"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: sExtraParams));
                    result.WriteLine(helper.LIEnd());
                }
                else if (model.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.select)
                {
                    //both preview and edit views need rowargs
                    string sExtraParams = StylesheetHelper.SetRowArgs(model.URIDataManager.StartRow,
                        model.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        model.URIDataManager.ParentStartRow);
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.preview.ToString(),
                        model.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                    result.WriteLine(helper.LinkUnobtrusive(id: "lnkReloadPreview",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("RELOAD_PREVIEW"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: sExtraParams));
                    result.WriteLine(helper.LIEnd());
                    //don't allow a synch next when selections are pending (or can't put selection anywhere)
                    if (model.URIDataManager.SelectionsNodeNeededName == string.Empty)
                    {
                        DevTreks.Services.IMemberService memberService
                            = new DevTreks.Services.MemberService(model);
                        bool bIsOkToDisplay = memberService.ContentCanBeSelectedAndEdited(model);
                        memberService.Dispose();
                        if (bIsOkToDisplay)
                        {
                            sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                model.URIDataManager.ControllerName,
                                GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                                model.URIPattern,
                                GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                                GeneralHelpers.NONE, GeneralHelpers.NONE);
                            result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                            result.WriteLine(helper.LinkUnobtrusive(id: "lnkLoadEdit",
                                href: "#",
                                classAttribute: "JSLink",
                                text: AppHelper.GetResource("LOAD_EDIT"),
                                contenturipattern: sContentURIPattern,
                                clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                extraParams: sExtraParams));
                            result.WriteLine(helper.LIEnd());
                        }
                    }
                }
                else if (model.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.linkedviews)
                {
                    if (model.URIFileExtensionType
                        != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        //link to a select view (to see context of this model) and add to pack view
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                            model.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                        result.WriteLine(helper.LinkUnobtrusive(id: "lnkReloadSelect",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("RELOAD_SELECT"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: string.Empty));
                        result.WriteLine(helper.LIEnd());
                    }
                    if (FileStorageIO.URIAbsoluteExists(model, model.URIClub.ClubDocFullPath))
                    {
                        //save in pack
                        //linkedviews use calcparams to package calcs and analyses
                        bool bNeedsSingleQuotes = true;
                        string sSelectedLinkedViewURI = string.Empty;
                        string sDocToCalcURI = string.Empty;
                        string sCalcDocURI = string.Empty;
                        string sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                            bNeedsSingleQuotes, model, string.Empty, ref sCalcDocURI, ref sDocToCalcURI,
                            ref sSelectedLinkedViewURI);
                        string sMainXmlDocPath = IOHelper.GetPackageMainDocPath(model);
                        sCalcParams = sCalcParams.EndsWith("'")
                            ? sCalcParams.Remove(sCalcParams.Length - 1) : sCalcParams;
                        sCalcParams += string.Concat(GeneralHelpers.FORMELEMENT_DELIMITER,
                            IOHelper.URI_XMLDOC_PATH, "=", sMainXmlDocPath, "'");
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.pack.ToString(),
                            model.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                        result.WriteLine(helper.LinkUnobtrusive(id: "lnkLoadPack",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("ADD_TO_PACK2"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.addtopack.ToString(),
                            extraParams: sCalcParams));
                        result.WriteLine(helper.LIEnd());
                    }
                }
                else
                {
                    if (model.URIFileExtensionType
                        != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        string sExtraParams = StylesheetHelper.SetRowArgs(model.URIDataManager.StartRow,
                            model.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                            model.URIDataManager.ParentStartRow);
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                            model.URIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                        result.WriteLine(helper.LinkUnobtrusive(id: "lnkReloadSelect",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("RELOAD_SELECT"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: sExtraParams));
                        result.WriteLine(helper.LIEnd());
                    }

                    string sURIPattern = model.URIPattern;
                    if (model.URIPattern.EndsWith(DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        || model.URIPattern.EndsWith(DataAppHelpers.DevPacks.DEVPACKS_TYPES.devpackpart.ToString()))
                    {
                        if (model.URIDataManager.Ancestors.Count >= 7)
                        {
                            //linkedviewpack ancestor
                            ContentURI ancestorURI = model.URIDataManager.Ancestors[6];
                            if (ancestorURI != null)
                            {
                                sURIPattern = ancestorURI.URIPattern;
                            }
                        }
                    }
                    //save in pack
                    if (ContentHelper.NodeCanHaveFile(model.URIClub.ClubDocFullPath))
                    {
                        string sMainXmlDocPath = model.URIClub.ClubDocFullPath;
                        sMainXmlDocPath = sMainXmlDocPath.Replace(
                            GeneralHelpers.FILE_PATH_DELIMITER, GeneralHelpers.WEBFILE_PATH_DELIMITER);
                        string sCalcParams = string.Concat("'", GeneralHelpers.FORMELEMENT_DELIMITER,
                            IOHelper.URI_XMLDOC_PATH, "=", sMainXmlDocPath, "'");
                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            GeneralHelpers.SERVER_ACTION_TYPES.pack.ToString(),
                            sURIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                        result.WriteLine(helper.LIStart(string.Empty, string.Empty));
                        result.WriteLine(helper.LinkUnobtrusive(id: "lnkReLoadPack",
                            href: "#",
                            classAttribute: "JSLink",
                            text: AppHelper.GetResource("ADD_TO_PACK"),
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.addtopack.ToString(),
                            extraParams: sCalcParams));
                        result.WriteLine(helper.LIEnd());
                    }
                }
                result.WriteLine(helper.ULEnd());
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }

        public static HtmlString Edit(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.Write(MakeSynchDocLinks(helper, model));
                if (!(string.IsNullOrEmpty(model.ErrorMessage)))
                {
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    result.WriteLine(helper.SpanError(model.ErrorMessage));
                    result.WriteLine(helper.DivEnd());
                }
                result.WriteLine(helper.FormStart("frmEdit", string.Empty));
                if (model.URIDataManager.SubActionView
                    != AddHelperLinq.SELECTION_OPTIONS.buildnewdocview.ToString())
                {
                    //includes a starting div that must be closed
                    helper.StartEditPanel(model)
                       .WriteTo(result, HtmlEncoder.Default);
                }
                else
                {
                    result.WriteLine(helper.DivStart("divEditsSection", string.Empty));
                }
                if (model.URIDataManager.ServerSubActionType ==
                    GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects
                    && model.URIDataManager.SubActionView
                    == AddHelperLinq.SELECTION_OPTIONS.buildnewdocview.ToString())
                {
                    if (model.URIDataManager.SelectedList == string.Empty)
                    {
                        DevTreks.Services.Helpers.EditHelper.SetSelectsList(model);
                    }
                    if (model.URIDataManager.SelectedList != string.Empty)
                    {
                        //no temp doc builds -extra level of complexity
                    }
                    else
                    {
                        model.ErrorMessage = AppHelper.GetErrorMessage("DISPLAYHELPER_NOSELECTEDFILES");
                        helper.PError(model.ErrorMessage)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                }
                else
                {
                    if (model.URIDataManager.UseSelectedLinkedView == false)
                    {
                        //first uri
                        helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.firstdoc)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                    else
                    {
                        //uri is based on doctocalc (addinhelper checks for changed linkedviews when starting addins)
                        result.Write("<br/>");
                        bool bHasGoodSelection = helper.WriteLinkedViewSelections2(result,
                            model);
                        if (!model.URIDataManager.UseSelectedLinkedView)
                        {
                            //display the linked view (usually in edits panel)
                            helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                        else
                        {
                            //keep this 100% consistent with LinkedView panel
                            if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews
                                    || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                            {
                                ContentURI selectedViewURI =
                                    DevTreks.Data.Helpers.LinqHelpers.GetLinkedViewIsSelectedView(model);
                                helper.DisplayURI(selectedViewURI, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                    .WriteTo(result, HtmlEncoder.Default);
                                model.ErrorMessage += selectedViewURI.ErrorMessage;
                            }
                            else
                            {
                                helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                    .WriteTo(result, HtmlEncoder.Default);
                            }
                        }
                    }
                }
                //write any error messages generated while trying to display the linkedviews
                if (!string.IsNullOrEmpty(model.ErrorMessage))
                {
                    helper.PError(model.ErrorMessage)
                        .WriteTo(result, HtmlEncoder.Default);
                }
                //div started in _startpanel
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.FormEnd());
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString EditLists(this IHtmlHelper helper,
            ContentURI model, Microsoft.AspNetCore.Http.HttpContext context)
        {
            using (StringWriter result = new StringWriter())
            {
                helper.MakeSynchDocLinks(model)
                    .WriteTo(result, HtmlEncoder.Default);
                if (!(string.IsNullOrEmpty(model.ErrorMessage)))
                {
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    result.WriteLine(helper.SpanError(model.ErrorMessage));
                    model.ErrorMessage = string.Empty;
                    result.WriteLine(helper.DivEnd());
                }
                helper.FeedbackSpan(model)
                    .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.FormStart("frmEdit", string.Empty));
                //display the lists using the writer
                helper.DisplayListView(model)
                   .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.FormEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString BuildDocToCalcButton(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                    == DevTreks.Models.AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    ContentURI calcDocURI
                        = LinqHelpers.GetLinkedViewIsSelectedAddIn(model);
                    if (calcDocURI != null)
                    {
                        if (AddInHelper.CanRunBaseDoc(model, calcDocURI.URIDataManager.HostName))
                        {
                            string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                               model.URIDataManager.ControllerName,
                               GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                               model.URIPattern,
                               GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile.ToString(),
                               model.URIDataManager.SubActionView,
                               model.URIDataManager.Variable);
                            string sClientAction = GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();

                            //inits off of selection box name (i.e. gets option)
                            //and automatically opens addin after generating doc
                            string sCalcParams 
                                = DataAppHelpers.LinkedViews.AddLinkedViewForSelectionBoxes(
                                    DevTreks.Services.Helpers.AddInStateHelper.NAME_FORADDINVIEW);
                            ////if it needs pagination use this with isAddInSelections = true
                            //bool bIsAddInSelections = true;
                            //StylesheetHelper.MakeViewsCalcParams(Model, bIsAddInSelections, ref sCalcParams);
                            result.WriteLine(helper.LinkUnobtrusiveMobile(
                                id: string.Concat(GeneralHelpers.SERVER_SUBACTION_TYPES.downloadfile.ToString(), "2"),
                                href: "#",
                                classAttribute: "JSLink",
                                text: AppHelper.GetResource("SAVE_FILEFORCALCS"),
                                contenturipattern: sContentURIPattern,
                                clientaction: sClientAction,
                                extraParams: sCalcParams,
                                dataRole: "button",
                                dataMini: "true",
                                dataInline: "true",
                                dataIcon: "gear",
                                dataIconPos: "right"));
                        }
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeDocToCalcSuccessMsg(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.ErrorMessage.Contains(AppHelper.GetErrorMessage("CONTENTSERVICE_HASNEWXMLDOC")))
                {
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    //display good xml base doc message
                    result.WriteLine(helper.SpanError(AppHelper.GetErrorMessage("CONTENTSERVICE_HASNEWXMLDOC")));
                    result.WriteLine(helper.DivEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeDownloadDataLink(this IHtmlHelper helper,
            ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                var sURIFull = AppSettings.ConvertPathFileandWeb(model, model.URIClub.ClubDocFullPath);
                if (!string.IsNullOrEmpty(sURIFull))
                {
                    result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/Dataset"));
                    result.WriteLine(helper.SpanItemStart("name"));
                    result.Write(string.Concat(@AppHelper.GetResource("DATASET"), ":"));
                    if (!string.IsNullOrEmpty(sURIFull))
                    {
                        result.WriteLine(helper.LinkItemStart(string.Concat("story", model.URIId), "distribution", sURIFull));
                        result.Write(string.Concat(model.URIName, " IRI"));
                        result.WriteLine(helper.LinkEnd());
                    }
                    result.WriteLine(helper.SpanEnd());
                    result.WriteLine(helper.SpanItemStart("description"));
                    result.Write(model.URIDataManager.Description);
                    result.WriteLine(helper.SpanEnd());
                    result.WriteLine(helper.DivEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString LinkedViews(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIDataManager.ServerActionType
                    == GeneralHelpers.SERVER_ACTION_TYPES.linkedviews
                    && model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithxml)
                {
                    GeneralHelpers.DOC_STATE_NUMBER eDocState
                        = GeneralHelpers.DOC_STATE_NUMBER.firstdoc;
                    if (model.URIDataManager.UseSelectedLinkedView == false)
                    {
                        eDocState = GeneralHelpers.DOC_STATE_NUMBER.firstdoc;
                    }
                    else
                    {
                        eDocState = GeneralHelpers.DOC_STATE_NUMBER.thirddoc;
                    }
                    string sDocToReadPath = AddInHelper.GetDevTrekPath(model, eDocState);
                    if (FileStorageIO.URIAbsoluteExists(model, sDocToReadPath))
                    {
                        FileStorageIO oFileStorageIO = new FileStorageIO();
                        string sErrorMsg = string.Empty;
                        string sSelectList = oFileStorageIO.ReadText(model, sDocToReadPath, out sErrorMsg);
                    }
                }
                else
                {
                    helper.MakeSynchDocLinks(model)
                        .WriteTo(result, HtmlEncoder.Default);
                    if (!(string.IsNullOrEmpty(model.ErrorMessage))
                        && (!model.ErrorMessage.Contains(AppHelper.GetErrorMessage("DISPLAYHELPER_DONTNEEDHTMLDOC")))
                        && (!model.ErrorMessage.Contains(AppHelper.GetErrorMessage("CONTENTSERVICE_HASNEWXMLDOC")))
                        && (!model.ErrorMessage.Contains(AppHelper.GetErrorMessage("DISPLAYHELPER_NOCALCULATIONS")))
                        && (model.URIDataManager.SubActionView
                        != GeneralHelpers.SUBACTION_VIEWS.graph.ToString()))
                    {
                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                        result.Write(helper.SpanError(model.ErrorMessage));
                        model.ErrorMessage = string.Empty;
                        result.WriteLine(helper.DivEnd());
                    }
                    result.WriteLine(helper.FormStart("frmLinkedViews", string.Empty));
                    helper.StartEditPanel(model)
                        .WriteTo(result, HtmlEncoder.Default);
                    DisplayURIHelper displayURI = new DisplayURIHelper();
                    //model is based on doctocalc (addinhelper checks for changed linkedviews when starting addins)
                    bool bHasGoodSelection = true;
                    if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                        == DevTreks.Models.AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                    {
                        result.WriteLine(helper.DivStart(string.Empty, string.Empty,
                            "controlgroup", "horizontal"));
                        helper.EditLinkedViewLink(model)
                            .WriteTo(result, HtmlEncoder.Default);
                        helper.BuildDocToCalcButton(model)
                            .WriteTo(result, HtmlEncoder.Default);
                        result.WriteLine(helper.DivEnd());
                        helper.MakeDocToCalcSuccessMsg(model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    if (string.IsNullOrEmpty(model.URIDataManager.SubActionView))
                    {
                        //default view is graph and must be set
                        model.URIDataManager.SubActionView
                            = GeneralHelpers.SUBACTION_VIEWS.graph.ToString();
                    }
                    bHasGoodSelection = helper.WriteLinkedViewSelections2(result, model);
                    result.WriteLine(helper.DivEnd());
                    if (model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.locals
                        && model.URIDataManager.UseSelectedLinkedView == false)
                    {
                        //switch views menu (has to come after WriteVS2 to get the calc params)
                        //gets displayed at top
                        helper.MakeDisplayURIMenu(model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                    else
                    {
                        //v188
                        if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                        {
                            helper.MakeDisplayURIMenu(model)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                    }
                    if (model.URIDataManager.SubActionView
                        != GeneralHelpers.SUBACTION_VIEWS.graph.ToString()
                        || model.URIDataManager.UseSelectedLinkedView)
                    {
                        if (!model.URIDataManager.UseSelectedLinkedView)
                        {
                            if (bHasGoodSelection == true)
                            {
                                //addins (ui is calculator or analyzer)
                                //must always be dynamic because calcparams are dynamic (tempdoc paths)
                                helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                                    .WriteTo(result, HtmlEncoder.Default);
                                //copy any missing html views from tempdocs to regular docs
                                displayURI.SaveHTMLForAddIn(model, GeneralHelpers.DOC_STATE_NUMBER.seconddoc);
                            }
                            if (model.URIDataManager.HasNewXml == false)
                            {
                                if (model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.locals)
                                {
                                    helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                        .WriteTo(result, HtmlEncoder.Default);
                                    helper.MakeDownloadDataLink(model)
                                        .WriteTo(result, HtmlEncoder.Default);
                                }
                            }
                        }
                        else
                        {
                            if (bHasGoodSelection)
                            {
                                if ((model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews
                                    || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                                    && model.URIDataManager.SubActionView
                                    != GeneralHelpers.SUBACTION_VIEWS.graph.ToString())
                                {
                                    ContentURI selectedViewURI =
                                        LinqHelpers.GetLinkedViewIsSelectedView(model);
                                    //addins are associated with selectedViewURI
                                    helper.DisplayURI(selectedViewURI, GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                                    .WriteTo(result, HtmlEncoder.Default);
                                    //write any error messages generated while trying to display the linkedviews
                                    if (!string.IsNullOrEmpty(selectedViewURI.ErrorMessage))
                                    {
                                        //selected lvs don't always need calcdocs
                                        if (!model.URIDataManager.UseSelectedLinkedView)
                                        {
                                            selectedViewURI.ErrorMessage = string.Empty;
                                        }
                                        else
                                        {
                                            selectedViewURI.ErrorMessage = string.Empty;
                                        }
                                    }
                                    //copy any missing html views from tempdocs to regular docs
                                    displayURI.SaveHTMLForAddIn(selectedViewURI, GeneralHelpers.DOC_STATE_NUMBER.seconddoc);
                                    if (model.URIDataManager.HasNewXml == false)
                                    {
                                        if (model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.locals)
                                        {
                                            helper.DisplayURI(selectedViewURI, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                                .WriteTo(result, HtmlEncoder.Default);
                                            helper.MakeDownloadDataLink(selectedViewURI)
                                                .WriteTo(result, HtmlEncoder.Default);
                                        }
                                    }
                                    if (!model.ErrorMessage.Contains(AppHelper.GetErrorMessage("DISPLAYHELPER_DONTNEEDHTMLDOC")))
                                    {
                                        model.ErrorMessage += selectedViewURI.ErrorMessage;
                                    }
                                    else
                                    {
                                        //just need a 1 line message
                                        model.ErrorMessage = AppHelper.GetErrorMessage("DISPLAYHELPER_DONTNEEDHTMLDOC");
                                    }
                                }
                                else
                                {
                                    //no second docs when stories being viewed or edited
                                    if (!model.URIDataManager.UseSelectedLinkedView)
                                    {
                                        //addins are associated with model
                                        helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.seconddoc)
                                            .WriteTo(result, HtmlEncoder.Default);
                                        //write any error messages generated while trying to display the linkedviews
                                        if (!string.IsNullOrEmpty(model.ErrorMessage))
                                        {
                                            helper.PError(model.ErrorMessage)
                                                 .WriteTo(result, HtmlEncoder.Default);
                                            model.ErrorMessage = string.Empty;
                                        }
                                        //copy any missing html views from tempdocs to regular docs
                                        displayURI.SaveHTMLForAddIn(model, GeneralHelpers.DOC_STATE_NUMBER.seconddoc);
                                    }
                                    if (model.URIDataManager.HasNewXml == false
                                        && model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.devpacks)
                                    {
                                        if (model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.locals)
                                        {
                                            helper.DisplayURI(model, GeneralHelpers.DOC_STATE_NUMBER.thirddoc)
                                                .WriteTo(result, HtmlEncoder.Default);
                                            helper.MakeDownloadDataLink(model)
                                                .WriteTo(result, HtmlEncoder.Default);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //write any error messages generated while trying to display the linkedviews
                    //descriptions mean its not an error (i.e. no third doc)
                    if (!string.IsNullOrEmpty(model.ErrorMessage)
                        && model.URIDataManager.SubActionView
                            != GeneralHelpers.SUBACTION_VIEWS.graph.ToString())
                    {
                        if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.locals
                            && model.URIDataManager.ServerSubActionType == GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin)
                        {
                            model.ErrorMessage = string.Empty;
                        }
                        else
                        {
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.PStart());
                            result.WriteLine(helper.SpanError(model.ErrorMessage));
                            result.WriteLine(helper.PEnd());
                            model.ErrorMessage = string.Empty;
                            result.WriteLine(helper.DivEnd());
                        }
                    }
                    //v188 added devpacks conditions
                    if (model.URIDataManager.SubActionView
                        == GeneralHelpers.SUBACTION_VIEWS.graph.ToString()
                        && (!model.URIDataManager.UseSelectedLinkedView
                        || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks))
                    {
                        
                        string sMURI = string.Empty;
                        string sMURIAlt = string.Empty;
                        if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews
                            || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                        {
                            ContentURI selectedViewURI = null;
                            if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                            {
                                selectedViewURI =
                                    LinqHelpers.GetLinkedViewIsSelectedView(model);
                            }
                            else
                            {
                                selectedViewURI =
                                    LinqHelpers.GetLinkedViewIsFirst(model);
                            }
                            if (selectedViewURI != null)
                            {
                                //2.0.2 : use MediaURL for displaying lv images ((because doctocalc image is the same for whole list))
                                //by using MediaURL to set lv.Resource.IsMainImage
                                DataAppHelpers.Resources.SetDefaultResourceURI(model, selectedViewURI);
                                ContentURI resourceURI = LinqHelpers.GetContentURIListIsMainImage(
                                    selectedViewURI.URIDataManager.Resource);
                                if (resourceURI == null)
                                    resourceURI = new ContentURI();
                                sMURI = resourceURI.URIDataManager.FileSystemPath;
                                sMURIAlt = resourceURI.URIDataManager.Description;
                            }
                        }
                        else
                        {
                            //2.0.2 : use MediaURL for displaying lv images (not doctocalc image)
                            //by using MediaURL to set lv.Resource.IsMainImage
                            ContentURI linkedViewURI =
                                LinqHelpers.GetLinkedViewIsSelectedAddIn(model);
                            DataAppHelpers.Resources.SetDefaultResourceURI(model, linkedViewURI);
                            ContentURI resourceURI = LinqHelpers.GetContentURIListIsMainImage(
                                linkedViewURI.URIDataManager.Resource);
                            if (resourceURI == null)
                                resourceURI = new ContentURI();
                            sMURI = resourceURI.URIDataManager.FileSystemPath;
                            sMURIAlt = resourceURI.URIDataManager.Description;
                        }
                        if (string.IsNullOrEmpty(sMURI))
                        {
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.PStart());
                            result.WriteLine(helper.SpanError(AppHelper.GetErrorMessage("DISPLAYHELPER_NOMEDIA")));
                            result.WriteLine(helper.PEnd());
                            model.ErrorMessage = string.Empty;
                            result.WriteLine(helper.DivEnd());
                        }
                        else
                        {
                            if (!sMURI.Contains(DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                                || !sMURI.StartsWith("http"))
                            {
                                result.WriteLine(helper.PStart());
                                result.WriteLine(helper.SpanError(
                                    AppHelper.GetErrorMessage("DISPLAYHELPER_BADMEDIAURL")));
                                result.WriteLine(helper.PEnd());
                                model.ErrorMessage = string.Empty;
                            }
                            else
                            {
                                if (model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.linkedviews
                                    || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                                {
                                    ContentURI selectedViewURI =
                                        LinqHelpers.GetLinkedViewIsSelectedView(model);
                                    if (selectedViewURI != null)
                                    {
                                        helper.MakeDownloadDataLink(selectedViewURI)
                                            .WriteTo(result, HtmlEncoder.Default);
                                    }
                                }
                                else
                                {
                                    helper.MakeDownloadDataLink(model)
                                        .WriteTo(result, HtmlEncoder.Default);
                                }
                                result.WriteLine("<br/>");
                                DisplayMedia(helper, result, sMURI, sMURIAlt, model);
                            }
                        }
                    }
                }
                //div started in _startpanel
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.FormEnd());
                return new HtmlString(result.ToString());
            }
        }
        private static void DisplayMedia(this IHtmlHelper helper, StringWriter result, string muri, 
            string muriAlt, ContentURI model)
        {
            string[] arrURLs = muri.Split(GeneralHelpers.STRING_DELIMITERS);
            if (arrURLs != null)
            {
                string sMediaURI = string.Empty;
                string resourceFileName = string.Empty;
                string sMediaName = string.Empty;
                for (int i = 0; i < arrURLs.Length; i++)
                {
                    sMediaURI = GeneralHelpers.GetSubstring(
                        muri, GeneralHelpers.STRING_DELIMITERS, i).Trim();
                    if (model.URIDataManager.ServerActionType
                        == GeneralHelpers.SERVER_ACTION_TYPES.preview)
                    {
                        //show 1 image or video on the Preview panel
                        if (!DataAppHelpers.Resources.IsImage(sMediaURI)
                            && !DataAppHelpers.Resources.IsVideo(sMediaURI))
                        {
                            sMediaURI = string.Empty;
                        }
                    }
                    if (!string.IsNullOrEmpty(sMediaURI))
                    {
                        if (!sMediaURI.Contains(DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString())
                            || !sMediaURI.StartsWith("http"))
                        {
                            int err = i + 1;
                            result.WriteLine(helper.PStart());
                            result.WriteLine(helper.SpanError(
                                string.Concat("Error with Media URL", @err, ":")));
                            result.WriteLine(helper.PEnd());
                            result.WriteLine(helper.PStart());
                            result.WriteLine(helper.SpanError(
                                AppHelper.GetErrorMessage("DISPLAYHELPER_BADMEDIAURL")));
                            result.WriteLine(helper.PEnd());
                            model.ErrorMessage = string.Empty;
                        }
                        resourceFileName = Path.GetFileName(sMediaURI);
                        sMediaName = Path.GetFileNameWithoutExtension(sMediaURI);
                        if (DataAppHelpers.Resources.IsVideo(sMediaURI))
                        {
                            string sThumbnailURI = StylesheetHelper.GetImagesUrl("devtreks-logo.jpg");
                            result.WriteLine(helper.DivItemStart(string.Empty, "video", "none", "http://schema.org/VideoObject"));
                            result.WriteLine(helper.SpanStart(string.Empty, string.Empty));
                            result.Write(string.Concat("Video:"));
                            result.WriteLine(helper.SpanItemStart("name"));
                            result.Write(sMediaName);
                            result.WriteLine(helper.SpanEnd());
                            result.WriteLine(helper.SpanEnd());
                            result.Write("<br/>");
                            result.WriteLine(helper.MetaItem(string.Empty, "thumbnailUrl", sThumbnailURI));
                            result.WriteLine(helper.MetaItem(string.Empty, "contentUrl", sMediaURI));
                            result.WriteLine(helper.VideoItemStart("controls", sThumbnailURI, "300", "300", "none"));
                            result.WriteLine(helper.SourceItem(sMediaURI, "video/mp4"));
                            result.WriteLine(helper.VideoEnd());
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.SpanItemStart("description"));
                            //don't use the same Calc.Description for multiple urls
                            result.Write(string.Empty);
                            result.WriteLine(helper.SpanEnd());
                            result.WriteLine(helper.DivEnd());
                            result.WriteLine(helper.DivEnd());
                        }
                        else if (DataAppHelpers.Resources.IsImage(sMediaURI))
                        {
                            result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/ImageObject"));
                            result.WriteLine(helper.SpanItemStart("name"));
                            result.Write(sMediaName);
                            result.WriteLine(helper.SpanEnd());
                            result.Write("<br/>");
                            result.Write(helper.Image(string.Concat("linkedviewimage_",
                                model.URIId.ToString()), sMediaURI,
                                muriAlt, "100%", "100%", string.Empty));
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.SpanItemStart("description"));
                            result.Write(string.Empty);
                            result.WriteLine(helper.SpanEnd());
                            result.WriteLine(helper.DivEnd());
                            result.WriteLine(helper.DivEnd());
                        }
                        else if (DataAppHelpers.Resources.IsStory(sMediaURI))
                        {
                            result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/TechArticle"));
                            result.WriteLine(helper.SpanItemStart("name"));
                            result.Write(string.Concat(AppHelper.GetResource("TECH_STORY"), ": ", sMediaName));
                            result.WriteLine(helper.SpanEnd());
                            result.Write("<br/>");
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.SpanItemStart("description"));
                            result.Write(string.Empty);
                            result.WriteLine(helper.SpanEnd());
                            result.WriteLine(helper.DivEnd());
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.LinkItemStart(string.Concat("story", model.URIId), "sameAs", sMediaURI));
                            result.Write(resourceFileName);
                            result.WriteLine(helper.LinkEnd());
                            result.WriteLine(helper.DivEnd());
                            result.WriteLine(helper.DivEnd());
                        }
                        else
                        {
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.SpanStart(string.Empty, string.Empty));
                            result.Write(sMediaName);
                            result.WriteLine(helper.SpanEnd());
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.SpanStart(string.Empty, string.Empty));
                            result.WriteLine(helper.SpanEnd());
                            result.WriteLine(helper.DivEnd());
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.LinkItemStart(string.Concat("media", model.URIId), "sameAs", sMediaURI));
                            result.Write(resourceFileName);
                            result.WriteLine(helper.LinkEnd());
                            result.WriteLine(helper.DivEnd());
                            result.WriteLine(helper.DivEnd());
                        }
                        if (model.URIDataManager.ServerActionType
                            == GeneralHelpers.SERVER_ACTION_TYPES.preview)
                        {
                            if (model.URIDataManager.AppType 
                                != GeneralHelpers.APPLICATION_TYPES.linkedviews)
                            {
                                result.WriteLine(muriAlt);
                            }
                            //don't display more than 1 image or video on the Preview panel
                            break;
                        }
                        else
                        {
                            result.WriteLine(helper.LinkMobile(string.Concat("resourcelink", model.URIId), sMediaURI,
                                string.Empty, AppHelper.GetResource("DOWNLOAD_RESOURCE"),
                                "button", "true", "true", string.Empty,
                                string.Empty));
                        }
                    }
                }
            }
        }
        public static HtmlString DisplayPreviews(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, "ui-corner-all custom-corners"));
                result.WriteLine(helper.DivStart(string.Empty, "ui-body ui-body-a"));
                result.WriteLine(helper.H4(model.URIName, string.Empty));
                result.WriteLine(helper.DivEnd());
                //show the meta data (and thumbnails) describing the children
                if (model.URIDataManager.Children == null)
                {
                    model.URIDataManager.Children = new List<ContentURI>();
                }
                if (model.URIDataManager.Children.Count > 2)
                {
                    int i = 1;
                    foreach (var child in model.URIDataManager.Children)
                    {
                        if (i <= 2)
                        {
                            //show the Model at top of list
                            if (child.URIPattern == model.URIPattern)
                            {
                                result.WriteLine(helper.DivStart(string.Empty, "ui-body ui-body-a"));
                                helper.DisplayChildCell(model, model)
                                .WriteTo(result, HtmlEncoder.Default);
                                result.WriteLine(helper.DivEnd());
                            }
                        }
                        else
                        {
                            //display the models children
                            result.WriteLine(helper.DivStart(string.Empty, "ui-body ui-body-a"));
                            helper.DisplayChildCell(model, child)
                                .WriteTo(result, HtmlEncoder.Default);
                            result.WriteLine(helper.DivEnd());
                        }
                        i++;
                    }
                }
                if (model.URIDataManager.Children.Count > 1)
                {
                }
                else
                {
                    result.WriteLine(helper.DivStart(string.Empty, "ui-corner-all custom-corners"));
                    result.Write(AppHelper.GetResource("RECORDS_NOCHILDREN"));
                    result.Write(" . ");
                    result.Write(AppHelper.GetResource("RECORDS_CHECKSTARTROW"));
                    result.WriteLine(helper.DivEnd());
                }
                result.WriteLine(helper.DivEnd());

                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString DisplayChildCell(this IHtmlHelper helper,
           ContentURI model, ContentURI child)
        {
            using (StringWriter result = new StringWriter())
            {
                ContentURI resourceURI = LinqHelpers.GetContentURIListIsMainImage(
                    child.URIDataManager.Resource);
                //write the image
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                if (DataAppHelpers.Resources.IsResourceImage(resourceURI))
                {
                    result.Write(helper.Image(string.Concat("uriimage_", child.URIId.ToString()), resourceURI.URIDataManager.FileSystemPath,
                        resourceURI.URIDataManager.Description, "50%", "50%", string.Empty));
                }
                else
                {
                    result.Write("<br/>");
                }
                result.WriteLine(helper.DivEnd());
                //write the name and description
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                result.WriteLine(helper.StrongStart());
                result.Write(child.URIName);
                result.WriteLine(helper.StrongEnd());
                result.Write(" : ");
                result.Write(child.URIDataManager.Description);
                result.WriteLine(helper.DivEnd());
                string sURIPatternForClickArgument = child.URIPattern;
                string sContentURIPattern = string.Empty;
                result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty, "controlgroup", "horizontal", "true"));
                sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                    sURIPatternForClickArgument,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.WriteLine(helper.LinkUnobtrusiveMobile(
                    id: string.Concat(GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(), "_", child.URIId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: AppHelper.GetResource("LOAD_SELECT"),
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: "button",
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: "arrow-r",
                    dataIconPos: "right"));
                result.WriteLine(helper.FieldsetEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString DisplayLinkedView(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                //show the meta data (and thumbnails) describing the 
                //available linked views (i.e. calculations, analyses, stories, media)
                if (model.URIDataManager.LinkedView != null)
                {
                    foreach (var linkedviewparent in model.URIDataManager.LinkedView)
                    {
                        int i = 1;
                        result.WriteLine(helper.DivStart(string.Empty, "ui-corner-all custom-corners"));
                        foreach (ContentURI linkedview in linkedviewparent)
                        {
                            if (i == 1)
                            {
                                result.WriteLine(helper.DivStart(string.Empty, "ui-bar ui-bar-a"));
                                result.WriteLine(helper.H4(ContentURI.GetURIPatternPart(linkedview.URIDataManager.ParentURIPattern,
                                ContentURI.URIPATTERNPART.name), string.Empty));
                                result.WriteLine(helper.DivEnd());
                            }
                            result.WriteLine(helper.DivStart(string.Empty, "ui-body ui-body-a"));
                            helper.DisplayLinkedViewCell(model, linkedview)
                                .WriteTo(result, HtmlEncoder.Default);
                            result.WriteLine(helper.DivEnd());
                            i++;
                        }
                        result.WriteLine(helper.DivEnd());
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString DisplayLinkedViewCell(this IHtmlHelper helper,
           ContentURI model, ContentURI linkedview)
        {
            using (StringWriter result = new StringWriter())
            {
                //2.0.2 : use MediaURL for displaying lv images (because doctocalc image is the same for whole list)
                //by using MediaURL to set lv.Resource.IsMainImage
                DataAppHelpers.Resources.SetDefaultResourceURI(model, linkedview);
                ContentURI resourceURI = LinqHelpers.GetContentURIListIsMainImage(
                    linkedview.URIDataManager.Resource);
                string sMURI = string.Empty;
                string sMURIAlt = string.Empty;
                if (resourceURI == null)
                    resourceURI = new ContentURI();
                sMURI = resourceURI.URIDataManager.FileSystemPath;
                sMURIAlt = resourceURI.URIDataManager.Description;
                if (model.URIDataManager.AppType
                   != GeneralHelpers.APPLICATION_TYPES.resources)
                {
                    //write the image
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    if (DataAppHelpers.Resources.IsResourceImage(resourceURI))
                    {
                        //2.0.2 changes
                        DisplayMedia(helper, result, sMURI, sMURIAlt, model);
                        //result.Write(helper.Image(string.Concat("linkedviewimage_",
                        //     linkedview.URIId.ToString()), resourceURI.URIDataManager.FileSystemPath,
                        //     resourceURI.URIDataManager.Description, "50%", "50%", string.Empty));
                    }
                    else
                    {
                        result.Write("<br/>");
                    }
                    result.WriteLine(helper.DivEnd());
                    var sURIFull = string.Empty;
                    var sNewURIPattern = string.Empty;
                    var sCURIPattern = string.Empty;
                    //note no fileext for devpackpart, so no link
                    if (!string.IsNullOrEmpty(model.URIFull)
                        && !string.IsNullOrEmpty(linkedview.URIFileExtensionType))
                    {
                        var sNetworkGroup = ContentURI.GetFullURIPathPart(model.URIFull, 1);
                        sNewURIPattern = ContentURI.ChangeURIPatternPart(model.URIPattern,
                            ContentURI.URIPATTERNPART.fileExtension, linkedview.URIFileExtensionType);
                        sCURIPattern = GeneralHelpers.MakePartialContentURIPattern(sNewURIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString(), GeneralHelpers.SUBACTION_VIEWS.full.ToString(), string.Empty);
                        sURIFull = GeneralHelpers.MakeFullURI(model, sNetworkGroup, GeneralHelpers.SERVER_ACTION_TYPES.linkedviews,
                            sCURIPattern);
                    }
                    if ((!string.IsNullOrEmpty(linkedview.URIDataManager.AddInName)
                        && linkedview.URIDataManager.AddInName != GeneralHelpers.NONE)
                        || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                    {
                        result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/Dataset"));
                        result.WriteLine(helper.SpanItemStart("name"));
                        result.Write(string.Concat(@AppHelper.GetResource("DATASET"), ":", linkedview.URIName));
                        result.WriteLine(helper.SpanEnd());
                        result.Write("<br/>");
                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                        result.WriteLine(helper.SpanItemStart("description"));
                        result.Write(linkedview.URIDataManager.Description);
                        result.WriteLine(helper.SpanEnd());
                        if (!string.IsNullOrEmpty(sURIFull))
                        {
                            result.WriteLine(helper.SpanStart(string.Empty, string.Empty));
                            result.WriteLine(helper.LinkItemStart(string.Concat("calc", linkedview.URIId), "sameAs", sURIFull));
                            result.Write(@AppHelper.GetResource("IRI_VIEWS"));
                            result.WriteLine(helper.LinkEnd());
                            result.WriteLine(helper.SpanEnd());
                        }
                        result.WriteLine(helper.DivEnd());
                        result.WriteLine(helper.DivEnd());
                    }
                    else
                    {
                        result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/TechArticle"));
                        result.WriteLine(helper.SpanItemStart("name"));
                        result.Write(string.Concat(@AppHelper.GetResource("TECH_STORY"), ": ", linkedview.URIName));
                        result.WriteLine(helper.SpanEnd());
                        result.Write("<br/>");
                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                        result.WriteLine(helper.SpanItemStart("description"));
                        result.Write(linkedview.URIDataManager.Description);
                        result.WriteLine(helper.SpanEnd());
                        if (!string.IsNullOrEmpty(sURIFull))
                        {
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            result.WriteLine(helper.LinkItemStart(string.Concat("story", linkedview.URIId), "sameAs", sURIFull));
                            result.Write(@AppHelper.GetResource("IRI_VIEWS"));
                            result.WriteLine(helper.LinkEnd());
                            result.WriteLine(helper.DivEnd());
                        }
                        result.WriteLine(helper.DivEnd());
                        result.WriteLine(helper.DivEnd());
                    }
                }
                bool bNeedsSingleQuote = false;
                string sCalcParams = string.Empty;
                string sURIPatternForClickArgument = model.URIPattern;
                bool bIsEditList = false;
                bool bIsAddInSelections = true;
                string sSelectionBoxName = DevTreks.Services.Helpers.AddInStateHelper.GetSelectLinkedViewBoxName(
                    bIsEditList, bIsAddInSelections);
                string sSelectedViewURIPattern = (model.URIFileExtensionType
                    == GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    ? DataAppHelpers.LinkedViews.GetLinkedViewURIPattern(model) 
                    : linkedview.URIPattern;
                if (model.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.linkedviews
                    || (model.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.resources
                    && model.URINodeName != DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString()))
                {
                    //parenturipattern accounts for recursive linkedviewpack parents (uri.uripattern could be an earlier ancestor)
                    //in the case of resources and linkedviews, its just a faster way to get 
                    //to the resource (i.e. image) or linkedview (i.e. calculator)
                    sURIPatternForClickArgument = (!string.IsNullOrEmpty(linkedview.URIDataManager.ParentURIPattern))
                        ? linkedview.URIDataManager.ParentURIPattern : model.URIPattern;
                }
                else if (model.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    if (linkedview.URINodeName
                        == DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        //stories associated with devpack 
                        //(uri.URIPattern is same as parentURIPattern in the else clause)
                        sURIPatternForClickArgument = model.URIPattern;
                    }
                    else
                    {
                        //devpacks use parents, same as linkedviews and resources
                        sURIPatternForClickArgument = (!string.IsNullOrEmpty(linkedview.URIDataManager.ParentURIPattern))
                            ? linkedview.URIDataManager.ParentURIPattern : model.URIPattern;
                    }
                }
                else
                {
                    //no recursion to deal with
                    sURIPatternForClickArgument = model.URIPattern;
                }
                if (linkedview.URINodeName
                    == DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                {
                    if (AddInHelper.IsAddIn(linkedview))
                    {
                        //uri is parent doctocalc; linkedview is now calcdoc
                        sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                            bNeedsSingleQuote, string.Empty, sURIPatternForClickArgument,
                            linkedview.URIPattern, string.Empty, string.Empty, string.Empty);
                    }
                    else
                    {
                        //uri is parent doctocalc; linkedview is now selectedlinkedview
                        sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(
                            bNeedsSingleQuote, string.Empty, sURIPatternForClickArgument,
                            string.Empty, string.Empty, sSelectedViewURIPattern, string.Empty);
                    }
                }
                else
                {
                    sCalcParams = DataAppHelpers.LinkedViews.GetLinkedViewStartParams(bNeedsSingleQuote,
                        sSelectionBoxName, sURIPatternForClickArgument,
                        string.Empty, string.Empty, sSelectedViewURIPattern, string.Empty);
                }
                string sContentURIPattern = string.Empty;
                string sClientAction = string.Empty;
                //v170 added devpack condition otherwise nothing loads (it thinks its a story, not a calculated result)
                if (AddInHelper.IsAddIn(linkedview)
                    || model.URIDataManager.AppType == GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    //run as addins 
                    DevTreks.Services.Helpers.AddInRunHelper.SetHostInitParams(linkedview.URIDataManager.HostName, ref sCalcParams);
                    sClientAction = GeneralHelpers.CLIENTACTION_TYPES.prepaddin.ToString();
                    string sSubAction = GeneralHelpers.SUBACTION_VIEWS.graph.ToString();
                    if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                           == DevTreks.Models.AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                    {
                        //easier to run calculators and analyzers in full view
                        sSubAction = GeneralHelpers.SUBACTION_VIEWS.full.ToString();
                    }
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       model.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                       sURIPatternForClickArgument,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString(),
                       sSubAction, GeneralHelpers.NONE);
                }
                else
                {
                    //run as a regular postbacks
                    sClientAction = GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       model.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                       sURIPatternForClickArgument,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                       GeneralHelpers.NONE, GeneralHelpers.NONE);
                }
                bool bHasFieldset = false;
                if (model.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.resources)
                {
                    //v160 can download from this page
                    if (!string.IsNullOrEmpty(linkedview.URIDataManager.FileSystemPath))
                    {
                        string resourceFileName = Path.GetFileName(linkedview.URIDataManager.FileSystemPath);
                        if (!string.IsNullOrEmpty(resourceFileName)
                            && resourceFileName != GeneralHelpers.NONE)
                        {
                            string resourceurl = StylesheetHelper.GetURIPattern(resourceFileName, linkedview.URIId.ToString(),
                                model.URINetworkPartName, DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString(),
                                string.Empty);
                            if (!string.IsNullOrEmpty(resourceurl))
                            {
                                string resourcePackId = ContentURI.GetURIPatternPart(linkedview.URIDataManager.ParentURIPattern,
                                        ContentURI.URIPATTERNPART.id);
                                string resourceparam = DataAppHelpers.Resources.GetResourceFilePath(
                                    model, false, model.URIDataManager.ServerSubActionType.ToString(),
                                    model.URINetworkPartName, resourcePackId, linkedview.URIId.ToString(),
                                    resourceFileName);
                                if (!string.IsNullOrEmpty(resourceparam))
                                {
                                    //v171
                                    if (DataAppHelpers.Resources.IsResourceVideo(linkedview))
                                    {
                                        string sThumbnailURI = StylesheetHelper.GetImagesUrl(model.URIDataManager.DefaultWebDomain,
                                            "devtreks-logo.jpg");
                                        result.WriteLine(helper.DivItemStart(string.Empty, "video", "none", "http://schema.org/VideoObject"));
                                        result.WriteLine(helper.SpanStart(string.Empty, string.Empty));
                                        result.Write(string.Concat("Video:"));
                                        result.WriteLine(helper.SpanItemStart("name"));
                                        result.Write(linkedview.URIName);
                                        result.WriteLine(helper.SpanEnd());
                                        result.WriteLine(helper.SpanEnd());
                                        result.Write("<br/>");
                                        result.WriteLine(helper.MetaItem(string.Empty, "thumbnailUrl", sThumbnailURI));
                                        result.WriteLine(helper.MetaItem(string.Empty, "contentUrl", resourceparam));
                                        result.WriteLine(helper.VideoItemStart("controls", sThumbnailURI, "300", "300", "none"));
                                        result.WriteLine(helper.SourceItem(resourceparam, "video/mp4"));
                                        result.WriteLine(helper.VideoEnd());
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.SpanItemStart("description"));
                                        result.Write(linkedview.URIDataManager.Description);
                                        result.WriteLine(helper.SpanEnd());
                                        result.WriteLine(helper.DivEnd());
                                        result.WriteLine(helper.DivEnd());
                                    }
                                    else if (DataAppHelpers.Resources.IsResourceImage(linkedview))
                                    {
                                        result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/ImageObject"));
                                        result.WriteLine(helper.SpanItemStart("name"));
                                        result.Write(linkedview.URIName);
                                        result.WriteLine(helper.SpanEnd());
                                        result.Write("<br/>");
                                        result.Write(helper.Image(string.Concat("linkedviewimage_",
                                             linkedview.URIId.ToString()), resourceURI.URIDataManager.FileSystemPath,
                                             resourceURI.URIDataManager.Description, "50%", "50%", string.Empty));
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.SpanItemStart("description"));
                                        result.Write(linkedview.URIDataManager.Description);
                                        result.WriteLine(helper.SpanEnd());
                                        result.WriteLine(helper.DivEnd());
                                        result.WriteLine(helper.DivEnd());
                                    }
                                    else if (DataAppHelpers.Resources.IsResourceStory(linkedview))
                                    {
                                        result.WriteLine(helper.DivItemStart(string.Empty, string.Empty, "none", "http://schema.org/TechArticle"));
                                        result.WriteLine(helper.SpanItemStart("name"));
                                        result.Write(linkedview.URIName);
                                        result.WriteLine(helper.SpanEnd());
                                        result.Write("<br/>");
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.SpanItemStart("description"));
                                        result.Write(linkedview.URIDataManager.Description);
                                        result.WriteLine(helper.SpanEnd());
                                        result.WriteLine(helper.DivEnd());
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.LinkItemStart(string.Concat("story", linkedview.URIId), "sameAs", resourceparam));
                                        result.Write(resourceFileName);
                                        result.WriteLine(helper.LinkEnd());
                                        result.WriteLine(helper.DivEnd());
                                        result.WriteLine(helper.DivEnd());
                                    }
                                    else
                                    {
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.SpanItemStart("name"));
                                        result.Write(linkedview.URIName);
                                        result.WriteLine(helper.SpanEnd());
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.SpanItemStart("description"));
                                        result.Write(linkedview.URIDataManager.Description);
                                        result.WriteLine(helper.SpanEnd());
                                        result.WriteLine(helper.DivEnd());
                                        result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                                        result.WriteLine(helper.LinkItemStart(string.Concat("media", linkedview.URIId), "sameAs", resourceparam));
                                        result.Write(resourceFileName);
                                        result.WriteLine(helper.LinkEnd());
                                        result.WriteLine(helper.DivEnd());
                                        result.WriteLine(helper.DivEnd());
                                    }
                                    bHasFieldset = true;
                                    result.Write(helper.FieldsetStart(string.Empty, string.Empty,
                                        "controlgroup", "horizontal", "true"));
                                    result.Write(helper.LinkMobile(string.Concat("resourcelink", linkedview.URIId),
                                        resourceparam, string.Empty, AppHelper.GetResource("DOWNLOAD_RESOURCE"),
                                        "button", "true", "true", string.Empty, string.Empty));
                                }
                            }
                        }
                    }
                }
                if (bHasFieldset == false)
                {
                    result.Write(helper.FieldsetStart(string.Empty, string.Empty,
                        "controlgroup", "horizontal", "true"));
                }
                result.Write(helper.LinkUnobtrusiveMobile(
                    id: string.Concat(GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                        "_", linkedview.URIId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: AppHelper.GetResource("LINKEDVIEWS_SYNCH"),
                    contenturipattern: sContentURIPattern,
                    clientaction: sClientAction,
                    extraParams: sCalcParams,
                    dataRole: "button",
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: "arrow-r",
                    dataIconPos: "right"));
                string sBaseLinkedViewURIPattern = linkedview.URIPattern;
                if (model.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.devpacks)
                {
                    if (linkedview.URINodeName
                        == DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                    {
                        //stories associated with devpack 
                        if (linkedview.URIDataManager.BaseId != 0)
                        {
                            //linkedview has a join table uripattern make it a base uripattern
                            sBaseLinkedViewURIPattern = ContentURI.ChangeURIPatternPart(
                                linkedview.URIPattern, ContentURI.URIPATTERNPART.id,
                                linkedview.URIDataManager.BaseId.ToString());
                        }
                    }
                    else
                    {
                        sBaseLinkedViewURIPattern = linkedview.URIPattern;
                    }
                }
                else
                {
                    if (linkedview.URIDataManager.BaseId != 0)
                    {
                        //linkedview has a join table uripattern make it a base uripattern
                        sBaseLinkedViewURIPattern = ContentURI.ChangeURIPatternPart(
                            linkedview.URIPattern, ContentURI.URIPATTERNPART.id,
                            linkedview.URIDataManager.BaseId.ToString());
                    }
                }
                sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                    sBaseLinkedViewURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusiveMobile(
                        id: string.Concat(GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                        "_", linkedview.URIId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: AppHelper.GetResource("LOAD_SELECT"),
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty,
                    dataRole: "button",
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: "arrow-r",
                    dataIconPos: "right"));
                result.Write(helper.FieldsetEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString DisplaySelectPanel(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.Write(helper.FormStart("frmSelect", string.Empty));
                bool bIsSearch = false;
                helper.MakeHorizStartAtRowButtons(
                    model, model.URIDataManager.StartRow, model.URIDataManager.PageSize,
                    model.URIDataManager.RowCount, model.URIDataManager.ParentStartRow,
                    bIsSearch)
                    .WriteTo(result, HtmlEncoder.Default);

                //selections
                if (model.URIDataManager.SelectViewEditType
                    != GeneralHelpers.VIEW_EDIT_TYPES.none
                    && model.URIMember.MemberDocFullPath != string.Empty)
                {
                    //retain selections state when navigating
                    if (model.URIDataManager.SelectedList != string.Empty)
                    {
                        //selections menu
                        helper.WriteSelectionsMenu(model).WriteTo(result, HtmlEncoder.Default);
                        //selections
                        helper.WriteSelections(model).WriteTo(result, HtmlEncoder.Default);
                    }
                    else if (model.URIDataManager.SelectionsNodeNeededName != string.Empty)
                    {
                        if (model.URIDataManager.SelectViewEditType
                            == GeneralHelpers.VIEW_EDIT_TYPES.part)
                        {
                            //selections menu
                            helper.WriteSelectionsMenu(model).WriteTo(result, HtmlEncoder.Default);
                            //selections
                            helper.WriteSelections(model).WriteTo(result, HtmlEncoder.Default);
                        }
                    }
                }
                result.Write("<br/>");
                result.Write(helper.ULStart(string.Empty, string.Empty,
                    "listview", string.Empty, "a"));
                helper.WriteChildrenLinks(model).WriteTo(result, HtmlEncoder.Default);
                result.Write(helper.ULEnd());
                result.Write(helper.FormEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteChildrenLinks(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                string sClass1 = string.Empty;
                //put the search names in an array
                if (model.URIDataManager.Children != null)
                {
                    if (model.URIDataManager.Children.Count > 0)
                    {
                        int i = 0;
                        //first param is used for backward's navigation to parent's parent
                        string sGrandParentFileName = string.Empty;
                        string sURIPattern = string.Empty;
                        string sExtraParams = string.Empty;
                        string sCommonName = string.Empty;
                        bool bIsFolder = true;
                        bool bCanNavigate = true;
                        //string sTocItemIndicator = ">&nbsp;";
                        string sTocSeparator = string.Empty;
                        bool bIsFirst = true;
                        string sClass = string.Empty;
                        bool bIsSelected = false;
                        foreach (ContentURI child in model.URIDataManager.Children)
                        {
                            sURIPattern = child.URIPattern;
                            if (sURIPattern != string.Empty)
                            {
                                sCommonName = child.URIName;
                                if (i == 0)
                                {
                                    //first param used to navigate back to parent of toc
                                    sGrandParentFileName = sURIPattern;
                                }
                                else if (i == 1)
                                {
                                    sClass1 = string.Empty;
                                    result.Write(helper.LIStart("list-divider"));
                                    if (model.URIDataManager.ParentPanelType !=
                                        GeneralHelpers.UPDATE_PANEL_TYPES.none)
                                    {
                                        //return to the parent start row
                                        sExtraParams = StylesheetHelper.SetRowArgs(0,
                                            model.URIDataManager.ParentStartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                                            0);
                                        string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                            model.URIDataManager.ControllerName,
                                            GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                                            sGrandParentFileName,
                                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                                            GeneralHelpers.NONE, GeneralHelpers.NONE);
                                        result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("selectchild", i.ToString()),
                                            href: "#",
                                            classAttribute: "JSLink",
                                            text: sCommonName,
                                            contenturipattern: sContentURIPattern,
                                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                            extraParams: sExtraParams,
                                            dataRole: "button",
                                            dataMini: "true",
                                            dataInline: "false",
                                            dataIcon: "arrow-u",
                                            dataIconPos: "left"));
                                    }
                                    else
                                    {
                                        result.Write(sCommonName);
                                    }
                                    result.Write(helper.LIEnd());
                                }
                                else
                                {
                                    bIsFolder = StylesheetHelper.IsFolder(model, child);
                                    bCanNavigate = (bIsFolder) ? true : false;
                                    //sTocItemIndicator = (bIsFolder) ? ">&nbsp;" : "-&nbsp;";
                                    StylesheetHelper.GetTocSeparator(model, child, ref sTocSeparator, ref bIsFirst);
                                    if ((!string.IsNullOrEmpty(sTocSeparator))
                                        && bIsFirst == true)
                                    {
                                        result.Write("<br/>");
                                        result.Write(helper.StrongStart());
                                        result.Write("<i>");
                                        result.Write(sTocSeparator);
                                        result.Write("</i>");
                                        result.Write(helper.StrongEnd());
                                        bIsFirst = false;
                                    }
                                    if (sCommonName.EndsWith("*"))
                                    {
                                        bIsSelected = true;

                                    }
                                    sClass1 = string.Empty;
                                    result.Write(helper.LIStart(string.Empty));
                                    helper.WriteListItem(model, bCanNavigate,
                                        bIsFolder, bIsSelected, sCommonName,
                                        sClass1, sURIPattern, i, child)
                                    .WriteTo(result, HtmlEncoder.Default);
                                    result.Write(helper.LIEnd());
                                }
                                bIsSelected = false;
                                i++;
                            }
                        }
                    }
                    if (model.URIDataManager.Children.Count > 2)
                    {
                    }
                    else
                    {
                        result.Write(helper.LIStart(string.Empty));
                        result.Write(AppHelper.GetResource("RECORDS_NOCHILDREN"));
                        result.Write(" . ");
                        result.Write(AppHelper.GetResource("RECORDS_CHECKSTARTROW"));
                        result.Write(helper.LIEnd());
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteListItem(this IHtmlHelper helper,
            ContentURI model, bool canNavigate, bool isFolder, bool isSelected,
            string commonName, string classAtt, string uriPattern, int i,
            ContentURI child)
        {
            using (StringWriter result = new StringWriter())
            {
                if (StylesheetHelper.IsListItemOnly(model, isFolder))
                {
                    //italicized and can't be selected
                    result.Write(helper.PStart());
                    result.Write("<i>");
                    result.Write("&nbsp; -&nbsp;");
                    result.Write(commonName);
                    result.Write("</i>");
                    result.Write(helper.PEnd());
                }
                else
                {
                    //init the parent start row
                    string sExtraParams = string.Empty;
                    if (isFolder)
                    {
                        sExtraParams = StylesheetHelper.SetRowArgs(0, 0,
                            "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                            model.URIDataManager.StartRow);
                    }
                    else
                    {
                        //return to the start row
                        sExtraParams = StylesheetHelper.SetRowArgs(0,
                            model.URIDataManager.StartRow, "-1",
                            DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                            model.URIDataManager.ParentStartRow);
                    }
                    string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                        uriPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                        GeneralHelpers.NONE, GeneralHelpers.NONE);
                    string sInstructions = string.Empty;
                    bool bHasInstructions = false;
                    bool bIsValidForSelection = StylesheetHelper.IsValidForSelection(
                          model, child, ref bHasInstructions, ref sInstructions);
                    if (i == 2)
                    {
                        if (!string.IsNullOrEmpty(sInstructions))
                        {
                            result.Write(sInstructions);
                            result.Write("<br/>");
                        }
                    }
                    helper.WriteChildSelectListItem(model,
                            bIsValidForSelection, model, child, classAtt)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.Write(helper.PStart());
                    if (canNavigate)
                    {
                        result.Write(helper.LinkUnobtrusiveMobile(id: string.Concat("selectchild", i.ToString()),
                            href: "#",
                            classAttribute: "JSLink",
                            text: string.Empty,//commonName,
                            contenturipattern: sContentURIPattern,
                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                            extraParams: sExtraParams,
                            dataRole: "button",
                            dataMini: "false",
                            dataInline: "true",
                            dataIcon: "arrow-d",
                            dataIconPos: "left"));
                    }
                    if (!bIsValidForSelection)
                    {
                        result.Write(helper.SpanStart(string.Empty, string.Empty));
                        result.Write("&nbsp;");
                        result.Write(string.Concat(commonName, ":"));
                        result.Write(helper.SpanEnd());
                    }
                    result.Write(helper.SpanStart(string.Empty, string.Empty));
                    result.Write("&nbsp; &nbsp;");
                    result.Write(child.URIDataManager.Description);
                    result.Write(helper.SpanEnd());
                    //model is used to get networkgroup
                    if (!string.IsNullOrEmpty(model.URIFull))
                    {
                        var sNGroup = ContentURI.GetFullURIPathPart(model.URIFull, 1);
                        //uripattern if for child
                        var sNewURIFull = GeneralHelpers.MakeFullURI(model, sNGroup, GeneralHelpers.SERVER_ACTION_TYPES.preview,
                            uriPattern);
                        result.Write(helper.SpanStart(string.Empty, string.Empty));
                        result.Write("&nbsp; (&lt; &nbsp;");
                        result.Write(helper.Link(string.Empty, sNewURIFull, string.Empty,
                            "preview IRI", string.Empty));
                        result.Write(helper.SpanEnd());
                    }
                    result.Write(helper.PEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteChildSelectListItem(this IHtmlHelper helper,
            ContentURI model, bool isValidForSelection,
            ContentURI uri, ContentURI child, string listItemClass)
        {
            using (StringWriter result = new StringWriter())
            {
                if (isValidForSelection)
                {
                    string sParentURIPattern = (!string.IsNullOrEmpty(child.URIDataManager.ParentURIPattern))
                        ? child.URIDataManager.ParentURIPattern : uri.URIDataManager.ParentURIPattern;
                    if (sParentURIPattern == string.Empty
                        || sParentURIPattern.Equals(child.URIPattern))
                    {
                        //set when ancestors are set
                        sParentURIPattern = uri.URIDataManager.ParentURIPattern;
                        if (sParentURIPattern == string.Empty
                            || sParentURIPattern.Equals(child.URIPattern))
                        {
                            //use ancestors to set parent
                            LinqHelpers.SetParentURIPatternFromAncestors(uri);
                            sParentURIPattern = uri.URIDataManager.ParentURIPattern;
                        }
                    }
                    //tempdoc selections are used for filenames, 
                    //and filenames use standard validation rules
                    ResourceRules.ValidateURIPatternScriptArgument(
                        ref sParentURIPattern);
                    string sChildURIPattern = child.URIPattern;
                    ResourceRules.ValidateURIPatternScriptArgument(
                        ref sChildURIPattern);
                    string sChildParentURIPatterns
                        = AddHelperLinq.MakeChildParentURIPattern(
                        sChildURIPattern, sParentURIPattern);
                    result.Write(helper.InputMobile(GeneralHelpers.VIEW_EDIT_TYPES.full, sChildParentURIPatterns,
                        "select", "checkbox", string.Empty, sChildParentURIPatterns, string.Empty,
                        "true", "true", string.Empty, string.Empty));
                    result.Write(helper.LabelStrong(sChildParentURIPatterns, child.URIName));
                }
                return new HtmlString(result.ToString());
            }
        }
        //DisplayHelper also has versions of the following 3 methods. Investigate consistency.
        public static HtmlString EditLinkedViewLink(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIDataManager.AppType
                    != GeneralHelpers.APPLICATION_TYPES.linkedviews)
                {
                    if (model.URIDataManager.UseSelectedLinkedView == true
                        && model.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.devpacks)
                    {
                        //use the addins belonging to the selectedview (Get Addins For View means exactly that)
                        ContentURI selectedLinkedViewURI
                            = LinqHelpers.GetLinkedViewIsSelectedView(model);
                        if (selectedLinkedViewURI != null)
                        {
                            if (selectedLinkedViewURI.URINodeName
                                != DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                            {
                                //they can edit linked views for selectedLinkedView
                                helper.WriteSelectNewLinkedView(selectedLinkedViewURI)
                                    .WriteTo(result, HtmlEncoder.Default);
                            }
                        }
                        else
                        {
                            //they can edit linked views for Model
                            helper.WriteSelectNewLinkedView(model)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                    }
                    else
                    {
                        if (model.URIDataManager.AppType
                            != GeneralHelpers.APPLICATION_TYPES.accounts
                            && model.URIDataManager.AppType
                            != GeneralHelpers.APPLICATION_TYPES.agreements
                            && model.URIDataManager.AppType
                            != GeneralHelpers.APPLICATION_TYPES.members
                            && model.URIDataManager.AppType
                            != GeneralHelpers.APPLICATION_TYPES.networks)
                        {
                            //they can edit linked views for Model
                            helper.WriteSelectNewLinkedView(model)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                    }
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteSelectNewLinkedView(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                == DevTreks.Models.AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    StylesheetHelper.WriteEditLinkedViewListLink(result, model);
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString FileUpload(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                string sControllerPath = string.Concat(
                    model.URIDataManager.DefaultWebDomain,
                    model.URIDataManager.ControllerName);
                result.WriteLine(helper.DivStart("divFileUploadParent", "display:none"));
                result.WriteLine(helper.DivStart("divFileUpload", string.Empty));
                result.WriteLine(helper.FormStart("frmUploadFile", sControllerPath,
                    "post", "multipart/form-data"));
                result.Write("<br/>");
                result.WriteLine(helper.LabelStrong("fileUpload", "File Upload"));
                result.Write("<br/>");
                result.Write("<br/>");
                result.WriteLine(helper.Input("fileUpload", "Input100PerCent", "file",
                    string.Empty, string.Empty, string.Empty));
                result.Write("<br/>");
                result.Write("<br/>");
                helper.InputUnobtrusiveMobile("btnFileUpload", "UploadFile", "submit",
                   string.Empty, string.Empty, string.Empty, AppHelper.GetResource("FILE_UPLOAD1"),
                   "true", "true", string.Empty, string.Empty)
                   .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.SpanStart("spanFileUpload", string.Empty));
                result.Write(AppHelper.GetResource("FILE_UPLOAD"));
                result.WriteLine(helper.SpanEnd());
                result.Write("<br/>");
                result.Write("<br/>");
                result.WriteLine(helper.SpanStart("spanFileUploadMessage", string.Empty));
                result.WriteLine(helper.SpanEnd());
                result.WriteLine(helper.FormEnd());
                result.WriteLine(helper.DivEnd());
                //http://stackoverflow.com/questions/166221/how-can-i-upload-files-asynchronously?rq=1
                //the src attribute causes 2 hits to controller, stackoverflow shows example with no src attribute
                //file uploads are targeted to the iframe src to simulate asynch upload
                result.WriteLine(helper.IFrameStart("frameFileUpload",
                    "frameFileUpload", "Hide1", ""));
                //result.WriteLine(helper.IFrameStart("frameFileUpload",
                //    "frameFileUpload", "Hide1", "#"));
                result.WriteLine(helper.IFrameEnd());
                result.WriteLine(helper.SpanStart("iframeresponse", string.Empty));
                result.Write("The file has been uploaded.");
                result.WriteLine(helper.SpanEnd());
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString Resource(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                string searchurl = StylesheetHelper.GetURIPattern(model.URIModels.Resource.ResourceName, model.URIModels.Resource.PKId.ToString(),
                    model.URINetworkPartName, DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString(), string.Empty);
                bool bCanDelete = AppHelper.CanDeleteNode(
                   model.URINodeName, DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString(),
                   model.URIDataManager.ServerActionType, model.URIMember);
                //get rid of any pagination instructions (or moves row after file upload)
                string sContentURIPatternNoPage = StylesheetHelper.GetContentURIPatternNoVar(model);
                MakeName(helper, @searchurl, model.URIModels.Resource.ResourceName,
                    "ResourceName", model.URIDataManager.EditViewEditType, bCanDelete)
                    .WriteTo(result, HtmlEncoder.Default);
                string resourceurl = StylesheetHelper.GetURIPattern(model.URIModels.Resource.ResourceFileName,
                    model.URIModels.Resource.PKId.ToString(), model.URINetworkPartName,
                    DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString(), string.Empty);
                string resourceparam = model.URIModels.Resource.ResourcePath;
                //2.0.0 deprecated - paths are set when model is built
                //StylesheetHelper styleHelper = new StylesheetHelper();
                //string resourceparam = styleHelper.GetResourceUrlByResource(Model, @resourceurl, @Model.URIModels.Resource.ResourcePackId.ToString(), @Model.URIDataManager.ServerSubActionType.ToString());
                if (!string.IsNullOrEmpty(resourceparam)
                    && model.URIModels.Resource.ResourceFileName != GeneralHelpers.NONE)
                {
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    if (model.URIModels.Resource.ResourceGeneralType
                        == DataAppHelpers.Resources.GENERAL_RESOURCE_TYPES.image.ToString())
                    {
                        if (model.URIDataManager.ServerActionType == GeneralHelpers.SERVER_ACTION_TYPES.linkedviews)
                        {
                            result.WriteLine(helper.Image(string.Concat("image", model.URIModels.Resource.PKId),
                                @resourceparam, model.URIModels.Resource.ResourceShortDesc,
                                "100%", "100%", string.Empty));
                        }
                        else
                        {
                            result.WriteLine(helper.Image(string.Concat("image", model.URIModels.Resource.PKId),
                                @resourceparam, model.URIModels.Resource.ResourceShortDesc,
                                "100%", "100%", string.Empty));
                        }
                    }
                    else if (model.URIModels.Resource.ResourceGeneralType
                        == DataAppHelpers.Resources.GENERAL_RESOURCE_TYPES.video.ToString())
                    {
                        result.WriteLine(helper.LabelItalic("resourcelink",
                            "Click button to download resource. Some browsers start playing the video immediately."));
                        result.Write("<br/>");
                        result.WriteLine(helper.LinkMobile(string.Concat("resourcelink",
                            model.URIModels.Resource.PKId), @resourceparam, string.Empty,
                            string.Concat("Download ", model.URIModels.Resource.ResourceFileName), "button",
                            "true", "true", string.Empty, string.Empty));
                        result.Write("<br/>");
                    }
                    else
                    {
                        result.WriteLine(helper.LabelItalic("resourcelink",
                            "Click button to download resource."));
                        result.Write("<br/>");
                        result.WriteLine(helper.LinkMobile(string.Concat("resourcelink",
                        model.URIModels.Resource.PKId), @resourceparam, string.Empty,
                        string.Concat("Download ", model.URIModels.Resource.ResourceFileName),
                        "button", "true", "true", string.Empty, string.Empty));
                        result.Write("<br/>");
                    }
                    result.WriteLine(helper.DivEnd());
                }
                result.WriteLine(helper.DivStart(string.Empty, "ui-grid-a"));
                result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                result.WriteLine(helper.LabelStrong("ResourceNum", "Label "));
                result.WriteLine(helper.InputTextUpdate(model.URIDataManager.EditViewEditType, searchurl,
                    model.URIModels.Resource.ResourceNum, "ResourceNum", GeneralRules.STRING, "15",
                    "Input75Center", string.Empty));
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                result.WriteLine(helper.LabelStrong("ResourceTagNameForApps", "  Tag Name "));
                result.WriteLine(helper.InputTextUpdate(model.URIDataManager.EditViewEditType, searchurl,
                    model.URIModels.Resource.ResourceTagNameForApps, "ResourceTagNameForApps", GeneralRules.STRING, "25",
                    "Input75Center", string.Empty));
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivStart(string.Empty, "ui-grid-a"));
                result.WriteLine(helper.DivStart(string.Empty, "ui-block-a"));
                result.WriteLine(helper.LabelStrong("ResourceLastChangedDate", "  Last Changed "));
                result.WriteLine(helper.InputTextUpdate(model.URIDataManager.EditViewEditType, searchurl,
                    model.URIModels.Resource.ResourceLastChangedDate.ToShortDateString(), "ResourceLastChangedDate",
                     GeneralRules.SHORTDATE, "8", "Input75Center", string.Empty));
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivStart(string.Empty, "ui-block-b"));
                result.WriteLine(helper.DivEnd());
                result.WriteLine(helper.DivEnd());
                helper.MakeTextArea(searchurl, model.URIModels.Resource.ResourceShortDesc, "ResourceShortDesc",
                    "Description", model.URIDataManager.EditViewEditType, "Text75H100PCW", "255")
                    .WriteTo(result, HtmlEncoder.Default);
                helper.MakeTextArea(searchurl, model.URIModels.Resource.ResourceLongDesc, "ResourceLongDesc",
                    "Long Description", model.URIDataManager.EditViewEditType, "Text200H100PCW", "1000")
                    .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                result.WriteLine(helper.LabelStrong("ResourceMimeType", "Mimetype  "));
                result.WriteLine(helper.InputTextUpdate(GeneralHelpers.VIEW_EDIT_TYPES.part, searchurl,
                    model.URIModels.Resource.ResourceMimeType, "ResourceMimeType",
                     GeneralRules.STRING, "75", "Input150", string.Empty));
                result.WriteLine(helper.DivEnd());
                helper.MakeSelectList(searchurl, model.URIModels.Resource.ResourceGeneralType,
                    "ResourceGeneralType", GeneralRules.STRING, "15", "GeneralType",
                    model.URIDataManager.EditViewEditType, "Select225",
                    DataAppHelpers.Resources.GetResourceGeneralType())
                    .WriteTo(result, HtmlEncoder.Default);
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                result.WriteLine(helper.LabelStrong("ResourceFileName", "FileName"));
                result.WriteLine(helper.InputTextUpdate(GeneralHelpers.VIEW_EDIT_TYPES.part, searchurl,
                model.URIModels.Resource.ResourceFileName, "ResourceFileName",
                     GeneralRules.STRING, "75", "Input150", string.Empty));
                result.WriteLine(helper.DivEnd());
                helper.MakeFileUpload(searchurl, sContentURIPatternNoPage,
                    model.URIModels.Resource.PKId.ToString(),
                    model.URIModels.Resource.ResourceFileName, "ResourceBinary", model.URIDataManager.EditViewEditType,
                    "4", model.URINetworkPartName, model.URIFileExtensionType, DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString(),
                    GeneralRules.BINARY, "60400")
                    .WriteTo(result, HtmlEncoder.Default);
                helper.WriteViewLinks(searchurl, model.URIDataManager.ContentURIPattern, model.URIDataManager.CalcParams,
                    DataAppHelpers.Resources.RESOURCES_TYPES.resource.ToString(), "oldid")
                    .WriteTo(result, HtmlEncoder.Default);
                return new HtmlString(result.ToString());
            }
        }
        private static bool WriteLinkedViewSelections2(this IHtmlHelper helper,
           StringWriter result, ContentURI docToCalcURI)
        {
            bool bHasGoodSelection = false;
            //uri is based on doctocalc (addinhelper checks for changed linkedviews when starting addins)
            bool bIsAddInSelections = false;
            //select list showing available story or tempdoc views 
            bool bHasGoodSelection1 = false;
            bool bHasStoryViews = LinqHelpers.HasLinkedViewIsSelectedView(docToCalcURI);
            if (bHasStoryViews)
            {
                result.WriteLine(helper.DivStart(string.Empty, string.Empty,
                    "controlgroup", "horizontal"));
                bHasGoodSelection1 = helper.WriteLinkedViewSelections(result,
                    docToCalcURI, bIsAddInSelections);
                result.WriteLine(helper.DivEnd());
            }
            bool bHasGoodSelection2 = false;
            if (docToCalcURI.URIDataManager.ServerActionType
                != GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                bIsAddInSelections = true;
                result.WriteLine(helper.DivStart(string.Empty, string.Empty,
                    "controlgroup", "horizontal"));
                //select list showing available views that use addins
                bHasGoodSelection2 = helper.WriteLinkedViewSelections(result,
                    docToCalcURI, bIsAddInSelections);
                result.WriteLine(helper.DivEnd());
            }
            if (bHasGoodSelection1 || bHasGoodSelection2)
            {
                bHasGoodSelection = true;
            }
            return bHasGoodSelection;
        }
        public static bool WriteLinkedViewSelections(this IHtmlHelper helper,
           StringWriter result, ContentURI docToCalcURI, bool isAddInSelections)
        {
            bool bHasGoodSelection = false;
            //can be found near top of views panel and when custom doc edits, edit panel
            bool bIsEditList = false;
            if (docToCalcURI.URIDataManager.ServerActionType
                == GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                bIsEditList = true;
                result.Write("<br/>");
            }
            //enables user to change views without going back to another panel
            //addinhelper checks for selections before deciding which linkedviewuri to run
            string sSelectionBoxName = DevTreks.Services.Helpers.AddInStateHelper
                .GetSelectLinkedViewBoxName(bIsEditList, isAddInSelections);
            if (docToCalcURI.URIDataManager.UseSelectedLinkedView == true
                && isAddInSelections == true)
            {
                //use the addins belonging to the selectedview (Get Addins For View means exactly that)
                ContentURI selectedLinkedViewURI
                    = LinqHelpers.GetLinkedViewIsSelectedView(docToCalcURI);
                if (selectedLinkedViewURI != null
                    && (docToCalcURI.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.devpacks
                    || docToCalcURI.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.linkedviews))
                {
                    //addins are associated with selectedLinkedViewURI
                    bHasGoodSelection = helper.MakeViewSelections(result, selectedLinkedViewURI,
                        sSelectionBoxName, GeneralHelpers.VIEW_EDIT_TYPES.full,
                        isAddInSelections);
                }
                else
                {
                    //addins are associated with docToCalcURI
                    bHasGoodSelection = helper.MakeViewSelections(result, docToCalcURI,
                        sSelectionBoxName, GeneralHelpers.VIEW_EDIT_TYPES.full,
                        isAddInSelections);
                }
            }
            else
            {
                //addins are associated with docToCalcURI
                bHasGoodSelection = helper.MakeViewSelections(result, docToCalcURI,
                    sSelectionBoxName, GeneralHelpers.VIEW_EDIT_TYPES.full,
                    isAddInSelections);
            }
            //uri is doctocalc; uri.uridatamanager.linkedviews contains linkedview.isselected = true
            string sCalcDocURIPattern = string.Empty;
            string sCalcParams = string.Empty;
            //inits off of selection box name (i.e. gets option)
            sCalcParams = DataAppHelpers.LinkedViews
                .AddLinkedViewForSelectionBoxes(sSelectionBoxName);
            StylesheetHelper.MakeViewsCalcParams(docToCalcURI, isAddInSelections, ref sCalcParams);
            //store the calc params (swith views will use them)
            docToCalcURI.URIDataManager.CalcParams = sCalcParams;
            string sContentURIPattern = string.Empty;
            string sClientAction = string.Empty;
            //string sMethod = string.Empty;
            if (docToCalcURI.URIDataManager.ServerActionType
                == GeneralHelpers.SERVER_ACTION_TYPES.edit)
            {
                //return the custom doc for editing (respondwithnewxhtml)
                sClientAction = GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();
                sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                   docToCalcURI.URIDataManager.ControllerName,
                   GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                   docToCalcURI.URIPattern,
                   GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithnewxhtml.ToString(),
                   docToCalcURI.URIDataManager.SubActionView, GeneralHelpers.NONE);
            }
            else
            {
                if (isAddInSelections == false)
                {
                    //return the linked view
                    sClientAction = GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       docToCalcURI.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                       docToCalcURI.URIPattern,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                       docToCalcURI.URIDataManager.SubActionView, GeneralHelpers.NONE);
                }
                else
                {
                    //start the addin
                    sClientAction = GeneralHelpers.CLIENTACTION_TYPES.prepaddin.ToString();
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       docToCalcURI.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.linkedviews.ToString(),
                       docToCalcURI.URIPattern,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.runaddin.ToString(),
                       docToCalcURI.URIDataManager.SubActionView, GeneralHelpers.NONE);
                }
            }
            if (isAddInSelections == false
                && bHasGoodSelection)
            {
                result.WriteLine(helper.LinkUnobtrusiveMobile("getlinkedview", "#", "JSLink",
                    AppHelper.GetResource("VIEW_GET"), sContentURIPattern, sClientAction,
                    sCalcParams, "button", "false", "false", string.Empty, string.Empty));
            }
            else
            {
                if (bHasGoodSelection)
                {
                    if (docToCalcURI.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.linkedviews
                        || docToCalcURI.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.devpacks
                        || docToCalcURI.URIFileExtensionType
                        == GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                    {
                        //no text: AppHelper.GetResource("VIEW_GETADDIN")
                        result.WriteLine(helper.LinkUnobtrusiveMobile("getlinkedaddin", "#", "JSLink",
                            AppHelper.GetResource("VIEW_GET"), sContentURIPattern, sClientAction,
                            sCalcParams, "button", "false", "true", string.Empty, string.Empty));
                    }
                    else
                    {
                        //no text: AppHelper.GetResource("VIEW_GETADDINPARENT")
                        result.WriteLine(helper.LinkUnobtrusiveMobile("getlinkedaddinparent", "#", "JSLink",
                            AppHelper.GetResource("VIEW_GET"), sContentURIPattern, sClientAction,
                            sCalcParams, "button", "false", "true", string.Empty, string.Empty));
                    }
                }
            }
            result.Write("<br/>");
            return bHasGoodSelection;
        }
        public static bool MakeViewSelections(this IHtmlHelper helper,
           StringWriter result, ContentURI docToCalcURI,
           string selectionBoxName, GeneralHelpers.VIEW_EDIT_TYPES viewEditType,
           bool isAddInSelections)
        {
            bool bHasAtLeastOneSelection = false;
            string sMethod = string.Empty;
            result.WriteLine(helper.SelectStart(viewEditType, selectionBoxName, selectionBoxName,
                string.Empty));
            bool bIsSelected = false;
            if (docToCalcURI.URIDataManager.LinkedView != null)
            {
                string sTrimmedOptionName = string.Empty;
                int iNameLength = 25;
                foreach (var linkedviewparent in docToCalcURI.URIDataManager.LinkedView)
                {
                    foreach (ContentURI linkedview in linkedviewparent)
                    {
                        bIsSelected = false;
                        int iTrimLength = (linkedview.URIName.Length >= iNameLength) ?
                            iNameLength : linkedview.URIName.Length;
                        sTrimmedOptionName = linkedview.URIName.Substring(0, iTrimLength);
                        sTrimmedOptionName = StylesheetHelper.MakeUniformStringLength(sTrimmedOptionName, iNameLength);
                        if (isAddInSelections == false)
                        {
                            if (linkedview.URIDataManager.IsSelectedLinkedView == true)
                            {
                                bIsSelected = true;
                            }
                            else
                            {
                                if (linkedview.URIDataManager.IsSelectedLinkedAddIn == true
                                    && bIsSelected == false)
                                {
                                    bIsSelected = true;
                                }
                            }
                        }
                        else
                        {
                            if (linkedview.URIDataManager.IsSelectedLinkedAddIn == true)
                            {
                                bIsSelected = true;
                            }
                        }
                        bool bIsAddIn = false;
                        if (AddInHelper.IsAddIn(linkedview))
                        {
                            bIsAddIn = true;
                        }
                        if (isAddInSelections)
                        {
                            if (bIsAddIn)
                            {
                                result.WriteLine(helper.Option(sTrimmedOptionName,
                                    linkedview.URIPattern, bIsSelected));
                                bHasAtLeastOneSelection = true;
                            }
                        }
                        else
                        {
                            if (bIsAddIn == false)
                            {
                                result.WriteLine(helper.Option(sTrimmedOptionName,
                                    linkedview.URIPattern, bIsSelected));
                                bHasAtLeastOneSelection = true;
                            }
                        }
                    }
                }
            }
            if (bHasAtLeastOneSelection == false)
            {
                if (isAddInSelections == true)
                {
                    result.WriteLine(helper.Option(AppHelper.GetResource("LINKEDVIEW_OPEN"),
                       "0", true));
                }
                else
                {
                    result.WriteLine(helper.Option(AppHelper.GetResource("LINKEDVIEWS_NOTAVAILABLE"),
                       "0", true));
                }
            }
            else
            {
                result.WriteLine(AppHelper.GetResource("LINKEDVIEWS_NONE"));
            }
            result.WriteLine(helper.SelectEnd());
            return bHasAtLeastOneSelection;
        }
        public static HtmlString DisplayURI(this IHtmlHelper helper,
            ContentURI uri, GeneralHelpers.DOC_STATE_NUMBER displayDocType)
        {
            using (StringWriter result = new StringWriter())
            {
                string sDocToReadPath
                = AddInHelper.GetDevTrekPath(uri, displayDocType);
                string sFilePathToXhtmlDoc = AppSettings.GetXhtmlDocPath(
                    uri, displayDocType, sDocToReadPath,
                    DataAppHelpers.Resources.FILEEXTENSION_TYPES.html,
                    GeneralHelpers.GetViewEditType(uri, displayDocType));

                if (FileStorageIO.URIAbsoluteExists(uri, sFilePathToXhtmlDoc))
                {

                    //write the fragment html doc to the writer (async doesn't work here)
                    FileStorageIO oFileStorageIO = new FileStorageIO();
                    oFileStorageIO.SaveHtmlURIToWriter(uri, result, sFilePathToXhtmlDoc);
                }
                else
                {
                    DisplayURIHelper.SetNoHtmlDocErrorMsg(uri, displayDocType);
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeXhtmlHeader(this IHtmlHelper helper,
            ContentURI uri, string pathToHeaderFiles)
        {
            using (StringWriter result = new StringWriter())
            {
                MakeXhtmlHeader(result, uri, pathToHeaderFiles);
                return new HtmlString(result.ToString());
            }
        }
        
        public static void MakeXhtmlHeader(StringWriter result, ContentURI uri,
            string pathToHeaderFiles)
        {
            result.WriteLine("<!doctype html public>");
            result.WriteLine("<html>");
            result.WriteLine("<head>");
            result.WriteLine("<title>");
            result.WriteLine(uri.URIName);
            result.WriteLine("</title>");
            result.WriteLine(HtmlExtensions.Meta(string.Empty,
                string.Empty, "utf-8").ToString());
            result.WriteLine(HtmlExtensions.Meta("robots",
                "all", "utf-8").ToString());
            result.WriteLine(HtmlExtensions.Meta("description",
                "Members of DevTreks use social budgeting to improve lives and livelihoods.",
                string.Empty).ToString());
            result.WriteLine(HtmlExtensions.Meta("keywords",
                "social budgeting; mobile budgeting; economic development; microeconomics; economics; budgets; costs; benefits; profits; operating budgets; capital budgets; cost benefit analysis; input prices; output prices; operations; components; outcomes; technology assessment; resources conservation; ",
                string.Empty).ToString());
            //2.0.0: use the minified css and js files generated from gulp deployment 
            //(current files have to be copied after inetpub deployment)
            result.WriteLine(HtmlExtensions.LinkHead("stylesheet", string.Concat(pathToHeaderFiles, "css/site.min.css")).ToString());
            result.WriteLine(HtmlExtensions.ScriptsHead(string.Concat(pathToHeaderFiles, "js/site.min.js")).ToString());
            //deprecated
            //result.WriteLine(HtmlExtensions.LinkHead("stylesheet",
            //    "text/css", string.Concat(pathToHeaderFiles, "stylesheets/Site1.css")).ToString());
            //result.WriteLine(HtmlExtensions.LinkHead("stylesheet",
            //    "text/css", string.Concat(pathToHeaderFiles, "stylesheets/desert2.css")).ToString());
            //result.WriteLine(HtmlExtensions.LinkHead("stylesheet",
            //    "text/css", string.Concat(pathToHeaderFiles, "stylesheets/jquery.mobile.structure-1.4.5.css")).ToString());
            result.WriteLine("</head>");
            result.WriteLine("<body>");
        }

        
        public static HtmlString MakeXhtmlFooter(this IHtmlHelper helper)
        {
            using (StringWriter result = new StringWriter())
            {
                MakeXhtmlFooter(result);
                return new HtmlString(result.ToString());
            }
        }
        public static void MakeXhtmlFooter(StringWriter result)
        {
            //footer
            result.WriteLine("</body>");
            result.WriteLine("</html>");
        }
        public static HtmlString MakeGetConstantsButtons(
            string calcParams, string cpForClose, string cpForCalculate)
        {
            using (StringWriter result = new StringWriter())
            {
                string sClass1 = "SubmitButton1Enabled150";
                //make a submit edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                       id: "getstartingselections",
                       classAttribute: sClass1,
                       type: "button",
                       contenturipattern: cpForCalculate,
                       clientaction: GeneralHelpers.CLIENTACTION_TYPES.prepaddin.ToString(),
                       extraParams: calcParams,
                       value: AppHelper.GetResource("GET_NEW_SELECTIONS"),
                       dataMini: "true",
                       dataInline: "true",
                       dataIcon: string.Empty,
                       dataIconPos: string.Empty));
                sClass1 = "ResetForm";
                //make a cancel edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                        id: CANCEL_EDITS,
                        classAttribute: sClass1,
                        type: "reset",
                        contenturipattern: string.Empty,
                        clientaction: string.Empty,
                        extraParams: string.Empty,
                        value: AppHelper.GetResource("CANCEL_EDITS"),
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: string.Empty,
                        dataIconPos: string.Empty));
                sClass1 = "SubmitButton1Enabled";
                //make a close edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                        id: "closeedits3",
                        classAttribute: sClass1,
                        type: "button",
                        contenturipattern: cpForClose,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.closeelement.ToString(),
                        extraParams: string.Empty,
                        value: AppHelper.GetResource("CLOSE_EDITS"),
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: string.Empty,
                        dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeCalculateButtons(
            string calcParams, bool isAnalysis, string cpForClose,
            string cpForCalculate)
        {
            using (StringWriter result = new StringWriter())
            {
                string sClass1 = "SubmitButton1Enabled150";
                //bool bIsDisabled = false;
                string sName = (isAnalysis) ? AppHelper.GetResource("ANALYZE")
                    : AppHelper.GetResource("CALCULATE");
                string sToolTip = (isAnalysis) ? AppHelper.GetResource("ANALYZE_TT")
                    : AppHelper.GetResource("CALCULATE_TT");
                //make a submit edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                       id: "calculate",
                       classAttribute: sClass1,
                       type: "button",
                       contenturipattern: cpForCalculate,
                       clientaction: GeneralHelpers.CLIENTACTION_TYPES.prepaddin.ToString(),
                       extraParams: calcParams,
                       value: sName,
                       dataMini: "true",
                       dataInline: "true",
                       dataIcon: string.Empty,
                       dataIconPos: string.Empty));
                sClass1 = "ResetForm";
                //make a cancel edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                        id: CANCEL_EDITS,
                        classAttribute: sClass1,
                        type: "reset",
                        contenturipattern: string.Empty,
                        clientaction: string.Empty,
                        extraParams: string.Empty,
                        value: AppHelper.GetResource("CANCEL_EDITS"),
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: string.Empty,
                        dataIconPos: string.Empty));
                sClass1 = "SubmitButton1Enabled";
                //make a close edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                        id: "closeedits2",
                        classAttribute: sClass1,
                        type: "button",
                        contenturipattern: cpForClose,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.closeelement.ToString(),
                        extraParams: string.Empty,
                        value: AppHelper.GetResource("CLOSE_EDITS"),
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: string.Empty,
                        dataIconPos: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString DisplayListView(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                //edit model menu
                string sClass2 = string.Empty;
                result.WriteLine(helper.DivStart("divEditsSection", sClass2));
                if (model.URIDataManager.UpdatePanelType
                    == GeneralHelpers.UPDATE_PANEL_TYPES.edit)
                {
                    if (model.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.accounts
                        || model.URIDataManager.AppType
                        == GeneralHelpers.APPLICATION_TYPES.agreements)
                    {
                        if (model.URIDataManager.SubActionView
                            == GeneralHelpers.SUBACTION_VIEWS.categories.ToString())
                        {
                            string sClubNetworkRole = Services.MemberService.GetNetworkRoleByClub(model);
                            if (sClubNetworkRole == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString()
                                && model.URIMember.MemberRole == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                            {
                                helper.WriteEditViewMenu(model)
                                    .WriteTo(result, HtmlEncoder.Default);
                            }
                            //display editable list of categories available for this network/supbapptype
                            helper.WriteNetworkCategories(model, sClubNetworkRole)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                        else
                        {
                            //write the club's owned services
                            helper.WriteClubServiceLinks(model)
                                .WriteTo(result, HtmlEncoder.Default); 
                        }
                        if (!string.IsNullOrEmpty(model.ErrorMessage))
                        {
                            helper.PError(model.ErrorMessage)
                                .WriteTo(result, HtmlEncoder.Default);
                            result.WriteLine(helper.DivEnd());
                            return null;
                        }
                    }
                }
                if (model.URIDataManager.AppType
                    == GeneralHelpers.APPLICATION_TYPES.members)
                {
                    if (model.URIDataManager.SubActionView
                        == AccountToPayment.PaymentList)
                    {
                        //2.0.0 refactor
                        result.Write("Subscription-based service payments are not available yet.");
                        //display list of model.member.defaultclub.payments 
                        //HtmlMemberExtensions.WriteDefaultClubPayments(result, model);
                    }
                    else
                    {
                        helper.WriteEditViewMenu(model)
                            .WriteTo(result, HtmlEncoder.Default);
                        //display list of model.member.clubs with edit capability on IsDefaultClub
                        helper.WriteMemberClubLinks(model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                }
                else if (model.URIDataManager.AppType
                    != GeneralHelpers.APPLICATION_TYPES.accounts
                    && model.URIDataManager.AppType
                    != GeneralHelpers.APPLICATION_TYPES.members)
                {
                    if (model.URIDataManager.SubActionView
                        == GeneralHelpers.SUBACTION_VIEWS.services.ToString())
                    {
                        if (model.URIService.Service.SubscribedClubs != null)
                        {
                            if (model.URIService.Service.SubscribedClubs.Count > 0)
                            {
                                helper.WriteEditViewMenu(model)
                                    .WriteTo(result, HtmlEncoder.Default);
                                //display list of model.modelservice.subscribedclubs
                                //with edit capability on authorizationlevel 
                                helper.WriteServiceClubLinks(model)
                                    .WriteTo(result, HtmlEncoder.Default);
                            }
                            else
                            {
                                model.ErrorMessage = AppHelper.GetErrorMessage("DISPLAYHELPER_NOSUBSCRIPTIONS");
                            }
                        }
                        else
                        {
                            model.ErrorMessage = AppHelper.GetErrorMessage("DISPLAYHELPER_NOSUBSCRIPTIONS");
                        }
                    }
                    else if (model.URIDataManager.SubActionView
                        == GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString())
                    {
                        if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                            == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                        {
                            helper.WriteEditViewMenu(model)
                                .WriteTo(result, HtmlEncoder.Default);
                        }
                        //display list of linkedviews
                        helper.WriteLinkedView(model)
                           .WriteTo(result, HtmlEncoder.Default);
                    }
                }
                if (!string.IsNullOrEmpty(model.ErrorMessage))
                {
                    result.Write("<br />");
                    helper.PError(model.ErrorMessage)
                        .WriteTo(result, HtmlEncoder.Default);
                }
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteLinkedView(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.StrongStart());
                result.Write(AppHelper.GetResource("LINKEDVIEWS_LIST"));
                result.Write(helper.StrongEnd());
                string sContentURIPattern = string.Empty;
                ContentURI nodeToCalcURI = new ContentURI(model);
                if (model.URIFileExtensionType
                    == GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                    && (!string.IsNullOrEmpty(model.URIDataManager.TempDocNodeToCalcURIPattern)))
                {
                    nodeToCalcURI
                        = LinqHelpers.GetLinkedViewURIByURIPattern(model,
                        model.URIDataManager.TempDocNodeToCalcURIPattern);
                    if (nodeToCalcURI == null)
                    {
                        nodeToCalcURI = ContentURI.ConvertShortURIPattern(model.URIDataManager.TempDocNodeToCalcURIPattern, model.URINetwork);
                    }
                }
                if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                    == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty, 
                        "controlgroup", "horizontal"));
                    //link to select new linkedview
                    helper.WriteSelectNewLinkedViewLink(
                        model, nodeToCalcURI, 
                        DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString())
                        .WriteTo(result, HtmlEncoder.Default);
                    //link to reload model edit panel
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       model.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                       model.URIPattern,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                       GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReloadEditPanel",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("EDIT_URI"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty,
                        dataRole: "button",
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "back",
                        dataIconPos: "left"));
                    //devpacks have to be opened from preview or edits panels - they use a parent-children lv pattern
                    if (model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.devpacks)
                    {
                        //link to open in views panel
                        nodeToCalcURI.URIDataManager.SubActionView
                            = GeneralHelpers.SUBACTION_VIEWS.mobile.ToString();
                  //move this here
                        StylesheetHelper.WriteOpenInLinkedViewPanelLink(
                            result, nodeToCalcURI, model, DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                    }
                    result.WriteLine(helper.DivEnd());
                    //which list will be loaded?
                    if (model.URIDataManager.AppType
                        != GeneralHelpers.APPLICATION_TYPES.addins)
                    {
                        //write the add default addin/locals buttons
                        helper.WriteAddDefaultLinkedView(model)
                            .WriteTo(result, HtmlEncoder.Default);
                    }
                }
                else
                {
                    //link to reload model edit panel
                    sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                       model.URIDataManager.ControllerName,
                       GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                       model.URIPattern,
                       GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                       GeneralHelpers.NONE, GeneralHelpers.NONE);
                    result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkReloadEditPanel",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("EDIT_URI"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty,
                        dataRole: "button",
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "back",
                        dataIconPos: "left"));
                    if (model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.devpacks)
                    {
                        //link to open in views panel
                        nodeToCalcURI.URIDataManager.SubActionView
                            = GeneralHelpers.SUBACTION_VIEWS.mobile.ToString();
                    //move here
                        StylesheetHelper.WriteOpenInLinkedViewPanelLink(
                            result, nodeToCalcURI, model, DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                    }
                }
                if (model.URIDataManager.LinkedView != null)
                {
                    if (model.URIDataManager.LinkedView.Count > 0)
                    {
                        {
                            string sFormElementName = string.Empty;
                            //linkedview join schema uses parent node name
                            string sParentNodeLinkedViewPattern = string.Empty;
                            string sBaseLinkedViewURIPattern = string.Empty;
                            string sFormIdId = string.Empty;
                            string sId1 = string.Empty;
                            result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                            //this is what causes the radios to change
                            result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty,
                                string.Empty, string.Empty, "true"));
                            foreach (var linkedviewparent in model.URIDataManager.LinkedView)
                            {
                                foreach (ContentURI linkedview in linkedviewparent)
                                {
                                    //tempdocs use the same state management as devpacks and stand-alone linked views, 
                                    //but should not be listed here
                                    if (linkedview.URIFileExtensionType
                                        != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString())
                                    {
                                        result.Write("<br />");
                                        sFormIdId = string.Concat("LinkedViewName", linkedview.URIId.ToString());
                                        result.WriteLine(helper.LabelRegular(
                                            sFormIdId, string.Concat("Linked View Id : ", linkedview.URIId.ToString())));
                                        //other form els follow normal edit patterns
                                        sFormElementName = EditHelper.MakeStandardEditName(
                                            linkedview.URIPattern, DataAppHelpers.LinkedViews.LINKEDVIEWNAME, GeneralRules.STRING,
                                            GeneralRules.NAME_SIZE);
                                        result.WriteLine(helper.InputTextUpdate(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                            sFormElementName, linkedview.URIName, sFormIdId, GeneralRules.STRING, "150",
                                            "Input300", string.Empty));
                                        //radios need the same name so use parentpattern (note that the repository 
                                        //updates isdefaults separately from other updates because of this pattern)
                                        sParentNodeLinkedViewPattern = ContentURI.ChangeURIPatternPart(linkedview.URIPattern,
                                            ContentURI.URIPATTERNPART.node, model.URINodeName);
                                        sFormElementName = EditHelper.MakeStandardEditName(
                                            model.URIPattern, DataAppHelpers.LinkedViews.ISDEFAULTLINKEDVIEWID, "boolean", "1");
                                        //the linkedview.modelid radio option value is used to change isdefault
                                        sFormIdId = string.Concat("DefaultLV", linkedview.URIId.ToString());
                                        result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                            sFormIdId, string.Empty, "radio", sFormElementName,
                                            linkedview.URIId.ToString(), linkedview.URIDataManager.IsDefault));
                                        result.WriteLine(helper.LabelRegular(
                                            sFormIdId, "Is Default"));
                                        result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty,
                                            "controlgroup", "horizontal", "true"));
                                        if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                                            == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                                        {
                                            sId1 = string.Concat("LinkedView", linkedview.URIId, "1");
                                            sFormElementName = EditHelper.MakeStandardDeleteName(linkedview.URIPattern);
                                            result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                                sId1, string.Empty, "radio", sFormElementName,
                                                EditHelper.DELETE, false));
                                            result.WriteLine(helper.LabelRegular(sId1, "DEL"));
                                            sId1 = string.Concat("LinkedView", linkedview.URIId, "2");
                                            result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                                sId1, string.Empty, "radio", sFormElementName,
                                                string.Empty, false));
                                            result.WriteLine(helper.LabelRegular(
                                                sId1, "UNDEL"));
                                        }
                                        //link to open in selects panel
                                        sBaseLinkedViewURIPattern = ContentURI.ChangeURIPatternPart(linkedview.URIPattern,
                                            ContentURI.URIPATTERNPART.id, linkedview.URIDataManager.BaseId.ToString());
                                        sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                                           model.URIDataManager.ControllerName,
                                           GeneralHelpers.SERVER_ACTION_TYPES.select.ToString(),
                                           sBaseLinkedViewURIPattern,
                                           GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                                           model.URIDataManager.SubActionView, model.URIDataManager.Variable);
                                        result.WriteLine(helper.LinkUnobtrusiveMobile(
                                            id: string.Concat("lnkView", nodeToCalcURI.URIId.ToString()),
                                            href: "#",
                                            classAttribute: "JSLink",
                                            text: AppHelper.GetResource("VIEW"),
                                            contenturipattern: sContentURIPattern,
                                            clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                            extraParams: string.Empty,
                                            dataRole: "button",
                                            dataMini: "true",
                                            dataInline: "true",
                                            dataIcon: string.Empty,
                                            dataIconPos: string.Empty));
                                        if (model.URIFileExtensionType
                                            != GeneralHelpers.FILENAME_EXTENSIONS.temp.ToString()
                                            && model.URIDataManager.AppType != GeneralHelpers.APPLICATION_TYPES.devpacks)
                                        {
                                            //link to open in views panel
                                            model.URIDataManager.SubActionView
                                                = GeneralHelpers.SUBACTION_VIEWS.mobile.ToString();
                                            StylesheetHelper.WriteOpenInLinkedViewPanelLink(
                                                result, model, linkedview, DataAppHelpers.LinkedViews.LINKEDVIEWS_TYPES.linkedview.ToString());
                                        }
                                        result.WriteLine(helper.FieldsetEnd());
                                    }
                                }
                            }
                            result.WriteLine(helper.FieldsetEnd());
                            result.WriteLine(helper.DivEnd());
                        }
                    }
                    else
                    {
                        model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "DISPLAYHELPER_NOLINKEDVIEWS");
                    }
                }
                else
                {
                    model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "DISPLAYHELPER_NOLINKEDVIEWS");
                }
                result.Write("<br />");
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteNetworkCategories(this IHtmlHelper helper,
           ContentURI model, string clubNetworkRole)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.StrongStart());
                result.Write(AppHelper.GetResource("MEMHELPER_CURRENTSERVICE"));
                result.Write(model.URIService.Service.ServiceName);
                result.WriteLine(helper.StrongEnd());
                result.Write("<br/>");
                result.Write("<br/>");
                //2.0.0 link to reload services agreement
                helper.MakeServiceAgreementLink(model,
                    AppHelper.GetResource("CLUB_AGREEMENT_VIEW"))
                    .WriteTo(result, HtmlEncoder.Default);
                result.Write("<br />");
                result.Write("<br />");
                result.Write(HtmlExtensions.StrongStart());
                result.Write(AppHelper.GetResource("NETWORK_CATEGORIES"));
                result.Write(HtmlExtensions.StrongEnd());
                result.Write("<br />");
                result.Write("<br />");
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                if (clubNetworkRole == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString()
                    && model.URIMember.MemberRole == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                {
                    //keep the adddefault pattern mostly intact
                    string sCategoryNodeName = GeneralHelpers.GetCategoryNodeName(model.URIService.Service.ServiceClassId);
                    string sDefaultURIPattern = GeneralHelpers.MakeURIPattern(
                        "00Default", model.URIService.Service.ServiceClassId.ToString(),
                        model.URIService.Service.NetworkId.ToString(), sCategoryNodeName, string.Empty);
                    string sDefaultParams = GeneralHelpers.GetDefaultParams(
                        model.URIPattern, sDefaultURIPattern);
                    //serversubactiontype combined with sDefaultParams will know what to do
                    string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                        model.URIPattern,
                        GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits.ToString(),
                        model.URIDataManager.SubActionView, GeneralHelpers.NONE);
                    //make number to add text box
                    string sTextId = string.Concat(AddHelperLinq.SELECT_EXISTING_PARAMS.adddefault_,
                        model.URIPattern);
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                    result.WriteLine(helper.InputUnobtrusiveMobile(id: "btnAddDefaultCategory",
                        classAttribute: "SubmitButton1Enabled150",
                        type: "button",
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        value: AppHelper.GetResource("ADD_NEWCATEGORY"),
                        extraParams: sDefaultParams,
                        dataMini: "true",
                        dataInline: "false",
                        dataIcon: "plus",
                        dataIconPos: "right"));
                    result.WriteLine(helper.LabelRegular(sTextId, "Number to Add"));
                    result.WriteLine(helper.Input(GeneralHelpers.VIEW_EDIT_TYPES.full.ToString(),
                        string.Empty, "text", string.Empty, sTextId, "1"));
                    result.WriteLine(helper.DivEnd());
                }
                else
                {
                    result.Write(HtmlExtensions.StrongStart());
                    result.Write(AppHelper.GetResource("NETWORK_CATEGORIES_ADMINONLY"));
                    result.Write(HtmlExtensions.StrongEnd());
                    result.Write("<br />");
                    result.Write("<br />");
                }
                if (model.URIService != null)
                {
                    if (model.URIDataManager.NetworkCategories != null)
                    {
                        if (model.URIDataManager.NetworkCategories.Count > 0)
                        {
                            {
                                string sId = string.Empty;
                                string sCategoryURIPattern = string.Empty;
                                foreach (ContentURI category in model.URIDataManager.NetworkCategories)
                                {
                                    sCategoryURIPattern = GeneralHelpers.MakeURIPattern(category.URIName,
                                        category.URIId.ToString(), category.URINetworkPartName,
                                        GeneralHelpers.GetCategoryNodeName(model.URIService.Service.ServiceClassId),
                                        string.Empty);
                                    result.WriteLine(helper.DivStart(string.Empty, string.Empty, 
                                        "fieldcontain", string.Empty));
                                    if (category.URIService.Service.NetworkId == 0
                                        && category.URIService.Service.ServiceClassId == 0)
                                    {
                                        //static, non-editable DevTreks-wide categories (i.e. all networks)
                                        result.Write("<p>");
                                        result.Write(string.Concat("Id:  ", category.URIId, ", ", "Id:  ", category.URIDataManager.Label));
                                        result.Write("<br />");
                                        result.Write(category.URIName);
                                        result.Write("</p>");
                                    }
                                    else
                                    {
                                        if (clubNetworkRole == DataAppHelpers.Members.MEMBER_ROLE_TYPES.coordinator.ToString())
                                        {
                                            result.WriteLine(helper.DivStart(string.Empty, string.Empty, 
                                                "fieldcontain", string.Empty));
                                            result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty,
                                                "controlgroup", "horizontal", "true"));
                                            string sId1 = string.Concat(sId, "1");
                                            sId = EditHelper.MakeStandardDeleteName(sCategoryURIPattern);
                                            result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                                sId1, string.Empty, "radio", sId,
                                                EditHelper.DELETE, false));
                                            result.WriteLine(helper.LabelRegular(sId1, "D"));
                                            sId1 = string.Concat(sId, "2");
                                            result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                                sId1, string.Empty, "radio", sId,
                                                string.Empty, false));
                                            result.WriteLine(helper.LabelRegular(
                                                sId1, "U"));
                                            result.WriteLine(helper.FieldsetEnd());
                                            result.WriteLine(helper.DivEnd());
                                        }
                                        sId = EditHelper.MakeStandardEditName(
                                            sCategoryURIPattern, DataAppHelpers.General.LABEL3, GeneralRules.STRING,
                                            GeneralRules.LABEL_SIZE);
                                        result.WriteLine(helper.LabelRegular(
                                            sId, string.Concat("Label for Id ", category.URIId)));
                                        result.WriteLine(helper.InputTextUpdate(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                            sId, category.URIDataManager.Label, sId, GeneralRules.STRING, GeneralRules.LABEL_SIZE,
                                            string.Empty, string.Empty));
                                        sId = EditHelper.MakeStandardEditName(
                                            sCategoryURIPattern, DataAppHelpers.Calculator.cName, GeneralRules.STRING,
                                            GeneralRules.NAME_SIZE);
                                        result.WriteLine(helper.LabelRegular(
                                            sId, "Name"));
                                        result.WriteLine(helper.InputTextUpdate(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                            sId, category.URIName, sId, GeneralRules.STRING, GeneralRules.LABEL_SIZE,
                                            string.Empty, string.Empty));
                                    }
                                    result.WriteLine(helper.DivEnd());
                                }
                            }
                        }
                        else
                        {
                            model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                string.Empty, "MEMHELPER_NOCATEGORIES");
                        }
                    }
                    else
                    {
                        model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MEMHELPER_NOCATEGORIES");
                    }
                }
                else
                {
                    model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MEMHELPER_NOSERVICE");
                }
                result.WriteLine(helper.DivEnd());
                result.Write("<br />");
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteSelectNewLinkedViewLink(this IHtmlHelper helper,
           ContentURI model, ContentURI nodeToCalcURI, string nodeNeededName)
        {
            using (StringWriter result = new StringWriter())
            {
                string sAttName = string.Empty;
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    model.URIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects.ToString(),
                    GeneralHelpers.SUBACTION_VIEWS.linkedviewslist.ToString(), GeneralHelpers.NONE);
                HtmlExtensions.LinkPlusUnobtrusiveMobile(
                    id: string.Concat(nodeToCalcURI.URIId.ToString(), "selectnewview"),
                    href: "#",
                    classAttribute: "GetSelectsLink",
                    text: AppHelper.GetResource("LINKEDVIEWS_NEW"),
                    spanId: "spanSelectionFiles",
                    contenturipattern: sContentURIPattern,
                    nodeURIPattern: nodeToCalcURI.URIPattern,
                    nodeName: nodeNeededName,
                    attributeName: sAttName,
                    extraParams: string.Empty,
                    dataRole: "button",
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: "search",
                    dataIconPos: "left")
                .WriteTo(result, HtmlEncoder.Default);
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString WriteEditViewMenu(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                bool bNeedsSingleQuote = false;
                string sSelectParams = StylesheetHelper.SetSelectedLinkedViewParams(model, bNeedsSingleQuote);
                string sSubAction = GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits.ToString();
                if (model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist
                    || model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits)
                {
                    //utility for simple list edits using ling to sql
                    sSubAction = GeneralHelpers.SERVER_SUBACTION_TYPES.submitlistedits.ToString();
                }
                else if (model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithform
                    || model.URIDataManager.ServerSubActionType
                    == GeneralHelpers.SERVER_SUBACTION_TYPES.submitformedits)
                {
                    //utility for simple form edits using ling to sql
                    sSubAction = GeneralHelpers.SERVER_SUBACTION_TYPES.submitformedits.ToString();
                }
                bool bHasCurrentURI = false;
                string sStatefulAncestorParams
                    = StylesheetHelper.GetStatefulAncestorParams(model, ref bHasCurrentURI);
                Services.IMemberService memberService = new Services.MemberService(model);
                bool bIsOkToDisplay = memberService.ContentCanBeSelectedAndEdited(model);
                memberService.Dispose();
                if (bIsOkToDisplay)
                {
                    //edits use pagination and need rowargs
                    string sPageParams = StylesheetHelper.SetRowArgs(model.URIDataManager.StartRow,
                        model.URIDataManager.StartRow, "-1", DataAppHelpers.Networks.NETWORK_FILTER_TYPES.none,
                        model.URIDataManager.ParentStartRow);
                    string sCPForSubmit = GeneralHelpers.MakeContentURIPattern(
                        model.URIDataManager.ControllerName,
                        GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                        model.URIPattern, sSubAction, model.URIDataManager.SubActionView,
                        model.URIDataManager.Variable);
                    string sCPForClose = GeneralHelpers.MakeContentURIPattern(
                         model.URIDataManager.ControllerName,
                         GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                         model.URIPattern, GeneralHelpers.SERVER_SUBACTION_TYPES.closeedits.ToString(),
                         model.URIDataManager.SubActionView,
                         model.URIDataManager.Variable);
                    helper.EditButtons(model, string.Concat(sStatefulAncestorParams, 
                        sSelectParams, sPageParams), sCPForClose, sCPForSubmit)
                        .WriteTo(result, HtmlEncoder.Default);
                }
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString WriteAddDefaultLinkedView(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                {
                    //linkedviews use calcparams to package calcs and analyses
                    bool bUseDefaultAddIn = true;
                    string sCalcParams = DataAppHelpers.LinkedViews.AddLinkedViewDefaultParam(string.Empty,
                        bUseDefaultAddIn);
                    string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                           model.URIDataManager.ControllerName,
                           GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                           model.URIPattern,
                           GeneralHelpers.SERVER_SUBACTION_TYPES.adddefaults.ToString(),
                           model.URIDataManager.SubActionView, model.URIDataManager.Variable);
                    result.WriteLine(helper.DivStart(string.Empty, string.Empty, 
                        "controlgroup", "horizontal"));
                    result.WriteLine(helper.LinkUnobtrusiveMobile(id: "lnkLoadDefaultAddIn",
                        href: "#",
                        classAttribute: "JSLink",
                        text: AppHelper.GetResource("ADD_DEFAULTADDIN"),
                        contenturipattern: sContentURIPattern,
                        clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                        extraParams: string.Empty,
                        dataRole: "button",
                        dataMini: "true",
                        dataInline: "true",
                        dataIcon: "plus",
                        dataIconPos: "right"));
                    result.WriteLine(helper.DivEnd());
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteClubServiceLinks(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                if (model.URIClub.AccountToService != null)
                {
                    result.Write(HtmlExtensions.StrongStart());
                    result.Write(AppHelper.GetResource("SERVICE_SELECT"));
                    result.Write(HtmlExtensions.StrongEnd());
                    result.Write("<br />");
                    result.Write("<br />");
                    result.WriteLine(helper.DivStart("spanHistoryToc", string.Empty, 
                        "controlgroup", "horizontal"));
                    //service click displays that service's subscribed clubs
                    helper.WriteServiceLinks(model)
                        .WriteTo(result, HtmlEncoder.Default);
                    result.WriteLine(helper.DivEnd());
                    result.Write("<br />");
                }
                else
                {
                    model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                        string.Empty, "MEMHELPER_NOSERVICES");
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteServiceLinks(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                int i = 0;
                bool bHasAService = false;
                foreach (AccountToService service in model.URIClub.AccountToService)
                {
                    if (service.IsOwner == true
                        && service.Name.StartsWith("00") == false)
                    {
                        bHasAService = true;
                        string sServiceURIPattern = GeneralHelpers.MakeURIPattern(
                            service.Service.ServiceName, service.ServiceId.ToString(),
                            model.URINetworkPartName,
                            DataAppHelpers.Agreement.AGREEMENT_BASE_TYPES.servicebase.ToString(),
                            string.Empty);
                        string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                            model.URIDataManager.ControllerName,
                            model.URIDataManager.ServerActionType.ToString(),
                            sServiceURIPattern,
                            GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithlist.ToString(),
                            model.URIDataManager.SubActionView, GeneralHelpers.NONE);
                            result.WriteLine(helper.LinkUnobtrusiveMobile(
                                id: string.Concat("subscribedservice", service.PKId),
                                href: "#",
                                classAttribute: "JSLink",
                                text: service.Service.ServiceName,
                                contenturipattern: sContentURIPattern,
                                clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                                extraParams: string.Empty,
                                dataRole: "button",
                                dataMini: "true",
                                dataInline: "true",
                                dataIcon: "grid",
                                dataIconPos: "left"));
                        i++;
                    }
                }
                if (bHasAService == false)
                {
                    model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                       string.Empty, "MEMHELPER_NOSERVICES2");
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteServiceClubLinks(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                //allows club coordinators to edit a service's subscribed clubs' authorizationlevels
                //(i.e. will they be allowed to edit context service?)
                //might be simplistic, but times have changed
                result.Write(HtmlExtensions.StrongStart());
                result.Write(AppHelper.GetResource("CLUB_SUBSCRIBERS"));
                result.Write(":&nbsp;");
                result.Write(model.URIService.Service.ServiceName);
                result.Write("&nbsp;:&nbsp;");
                result.Write(HtmlExtensions.StrongEnd());
                result.Write("<br />");
                result.Write("<br />");
                result.WriteLine(helper.ULStart(string.Empty, "listview", "b"));
                //service click displays that service's subscribed clubs
                bool bIsSelected = false;
                foreach (AccountToService subscribedClub in model.URIService.Service.SubscribedClubs)
                {
                    bIsSelected = false;
                    result.WriteLine(helper.LIStart(string.Empty));
                    result.Write(HtmlExtensions.StrongStart());
                    result.Write(subscribedClub.Account.AccountName);
                    result.Write(HtmlExtensions.StrongEnd());
                    result.Write("<br />");
                    result.Write(subscribedClub.Account.AccountEmail);
                    result.Write("&nbsp;&nbsp;&nbsp;");
                    result.Write(subscribedClub.Account.AccountDesc);
                    result.Write("<br />");
                    string sLabel = AppHelper.GetResource("CLUB_AUTHORIZATION_LEVEL");
                    if (model.URIMember.ClubInUse.PrivateAuthorizationLevel
                        == AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                    {
                        if (bIsSelected)
                        {
                            result.Write(HtmlExtensions.StrongStart());
                            sLabel += " ***";
                            result.Write(HtmlExtensions.StrongEnd());
                        }
                        string sURIPattern = GeneralHelpers.MakeURIPattern(subscribedClub.Name,
                            subscribedClub.PKId.ToString(), model.URINetworkPartName,
                            DataAppHelpers.Agreement.AGREEMENT_TYPES.service.ToString(), string.Empty);
                        string sFormElementName = EditHelper.MakeStandardEditName(
                            sURIPattern, DataAppHelpers.Members.AUTHORIZATION_LEVEL, "smallint", "2");
                        result.WriteLine(helper.LabelRegular(
                            sFormElementName, sLabel));
                        result.WriteLine(helper.SelectStart(GeneralHelpers.VIEW_EDIT_TYPES.full, 
                            string.Concat(DataAppHelpers.Members.AUTHORIZATION_LEVEL, subscribedClub.PKId.ToString()),
                            sFormElementName, "Select250"));
                        bIsSelected = (subscribedClub.AuthorizationLevel
                            == (int)AccountHelper.AUTHORIZATION_LEVELS.fulledits)
                            ? true : false;
                        result.WriteLine(helper.Option(AppHelper.GetResource("CLUB_CAN_EDIT"),
                          "1", bIsSelected));
                        result.WriteLine(helper.Option(AppHelper.GetResource("CLUB_CANNOT_EDIT"),
                          "2", bIsSelected));
                        result.WriteLine(helper.SelectEnd());
                    }
                    else
                    {
                        result.Write(HtmlExtensions.StrongStart());
                        string sAuthoriz = model.URIMember.ClubInUse.PrivateAuthorizationLevel.ToString();
                        result.Write(string.Concat(AppHelper.GetResource("AUTHORIZATION_NO1"),
                            sAuthoriz, AppHelper.GetResource("AUTHORIZATION_NO2")));
                        result.Write(HtmlExtensions.StrongEnd());
                    }
                    result.WriteLine(helper.LIEnd());
                    result.Write("<br />");
                }
                result.WriteLine(helper.ULEnd());
                result.Write("<br />");
                result.Write("<br />");
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString MakeServiceAgreementLink(this IHtmlHelper helper,
           ContentURI model, string title)
        {
            using (StringWriter result = new StringWriter())
            {
                //model state sets networkpartname when full modelpaths are set
                string sNetworkPartName
                    = ContentURI.GetFullURIPathPart(model.URIMember.URIFull, 3);
                string sNodeName
                    = DataAppHelpers.Agreement.AGREEMENT_TYPES.serviceaccount.ToString();
                string sServiceURIPattern = GeneralHelpers.MakeURIPattern(
                    model.URIMember.ClubDefault.AccountName,
                    model.URIMember.ClubDefault.PKId.ToString(),
                    sNetworkPartName, sNodeName, string.Empty);
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sServiceURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.respondwithhtml.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                result.Write(helper.LinkUnobtrusive(
                    id: string.Concat("subscribedservice", model.URIId.ToString()),
                    href: "#",
                    classAttribute: "JSLink",
                    text: title,
                    contenturipattern: sContentURIPattern,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: string.Empty));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteMemberClubLinks(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                //allows loggedin member to change isdefault club
                result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty,
                    "controlgroup", string.Empty, "true"));
                result.WriteLine(helper.Legend(AppHelper.GetResource("MEMBER_CLUBSDEFAULT")));
                if (model.URIMember.Member.AccountToMember != null)
                {
                    string sURIPattern = string.Empty;
                    string sFormElementName = string.Empty;
                    if (model.URIMember.Member.AccountToMember.Count > 0)
                    {
                        sURIPattern = GeneralHelpers.MakeURIPattern(model.URIMember.Member.MemberLastName,
                                model.URIMember.MemberId.ToString(), model.URINetworkPartName,
                                DataAppHelpers.Members.MEMBER_BASE_TYPES.memberbase.ToString(), string.Empty);
                        sFormElementName = EditHelper.MakeStandardEditName(
                            sURIPattern, DataAppHelpers.Members.ISDEFAULTCLUB, "boolean", "1");
                        string sFormId = string.Empty;
                        foreach (AccountToMember memberClub in model.URIMember.Member.AccountToMember)
                        {
                            sFormId = string.Concat("Club", memberClub.PKId.ToString());
                            //member can always edit default club option (only app
                            //where model.clubinuse.authorizationlevel doesn't matter)
                            helper.MakeCurrentClubOption(model, sURIPattern, sFormId,
                               sFormElementName, memberClub)
                               .WriteTo(result, HtmlEncoder.Default);
                            result.WriteLine(helper.LabelRegular(
                                    sFormId, memberClub.ClubDefault.AccountName));
                        }
                    }
                    else
                    {
                        model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MEMBERHELPER_NOTMEMBER");
                    }
                }
                else
                {
                    model.ErrorMessage = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "MEMBERHELPER_NOTMEMBER");
                }
                result.WriteLine(helper.FieldsetEnd());
                result.Write("<br />");
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeCurrentClubOption(this IHtmlHelper helper,
           ContentURI model, string modelPattern, string formId, string formElementName,
           AccountToMember club)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                    formId, string.Empty, "radio", formElementName,
                    club.AccountId.ToString(), club.IsDefaultClub));
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString EditButtons(this IHtmlHelper helper,
           ContentURI model, string calcParams, string cpForClose, string cpForSubmit)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, 
                    "ui-body ui-body-b", "controlgroup", "horizontal"));
                string sClass1 = "SubmitButton1Enabled150";
                //original included disable
                bool bIsDisabled = false;
                //make a submit edits button
                result.WriteLine(helper.InputUnobtrusiveMobile(
                    id: GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits.ToString(),
                    classAttribute: sClass1,
                    type: "button",
                    contenturipattern: cpForSubmit,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                    extraParams: calcParams,
                    value: AppHelper.GetResource("SUBMIT_EDITS"),
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                //mobile requires jquery separate action
                sClass1 = "ResetForm";
                //make a cancel edits button
                result.Write(HtmlExtensions.InputUnobtrusiveMobile(
                    id: CANCEL_EDITS,
                    classAttribute: sClass1,
                    type: "reset",
                    contenturipattern: string.Empty,
                    clientaction: string.Empty,
                    extraParams: string.Empty,
                    value: AppHelper.GetResource("CANCEL_EDITS"),
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                //make a close edits button
                sClass1 = "SubmitButton1Enabled";
                result.WriteLine(helper.InputUnobtrusiveMobile(
                    id: GeneralHelpers.SERVER_SUBACTION_TYPES.closeedits.ToString(),
                    classAttribute: sClass1,
                    type: "button",
                    contenturipattern: cpForClose,
                    clientaction: GeneralHelpers.CLIENTACTION_TYPES.closeelement.ToString(),
                    extraParams: string.Empty,
                    value: AppHelper.GetResource("CLOSE_EDITS"),
                    dataMini: "true",
                    dataInline: "true",
                    dataIcon: string.Empty,
                    dataIconPos: string.Empty));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeHtml(
           ContentURI model, XmlReader reader, GeneralHelpers.DOC_STATE_NUMBER displayDocType,
           IDictionary<string, string> styleParams)
        {
            using (StringWriter result = new StringWriter())
            {
                XmlReader stylesheetReader = null;
                //create the XsltSettings object with document() enabled and script disabled.
                XsltSettings oXsltSettings = new XsltSettings(true, false);
                oXsltSettings.EnableDocumentFunction = true;
                XslCompiledTransform oXslt = new XslCompiledTransform();
                XmlUrlResolver oResolver = new XmlUrlResolver();
                oResolver.Credentials = CredentialCache.DefaultCredentials;
                //create an XsltArgumentList.
                Object extensionObject = null;
                ContentURI stylesheetURI = new ContentURI();
                StylesheetHelper styleHelper = new StylesheetHelper();
                stylesheetReader = styleHelper.GetStyleSheet(model, displayDocType,
                    ref extensionObject, ref stylesheetURI);
                bool bHasLoadedStyle = false;
                XsltArgumentList oXslArgList = new XsltArgumentList();
                if (stylesheetReader != null
                    && string.IsNullOrEmpty(model.ErrorMessage))
                {
                    using (stylesheetReader)
                    {
                        oXslt.Load(stylesheetReader, oXsltSettings, oResolver);
                        bHasLoadedStyle = true;
                    }
                }
                else if (string.IsNullOrEmpty(model.ErrorMessage))
                {
                    if (stylesheetURI != null)
                    {
                        if (stylesheetURI.URIDataManager.FileSystemPath != string.Empty)
                        {
                            oXslt.Load(stylesheetURI.URIDataManager.FileSystemPath, oXsltSettings,
                                oResolver);
                            bHasLoadedStyle = true;
                        }
                        else
                        {
                            model.ErrorMessage
                                = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                                    string.Empty, "STYLEHELPER_NOSTYLE");
                            return new HtmlString(model.ErrorMessage);
                        }
                    }
                }
                if (bHasLoadedStyle
                    && string.IsNullOrEmpty(model.ErrorMessage))
                {
                    // add args
                    if (styleParams != null)
                    {
                        if (extensionObject != null)
                        {
                            oXslArgList.AddExtensionObject(
                                stylesheetURI.URIDataManager.ExtensionObjectNamespace,
                                extensionObject);
                        }
                        foreach (KeyValuePair<string, string> kvp in styleParams)
                        {
                            if (kvp.Value == null)
                            {
                                oXslArgList.AddParam(kvp.Key, "", GeneralHelpers.NONE);
                            }
                            else
                            {
                                oXslArgList.AddParam(kvp.Key, "", kvp.Value);
                            }
                        }
                    }
                    try
                    {
                        //writer can't be init with a StringWriter or can't use xslt html output method
                        //try using existing xml output method in stylesheets
                        //after all Stylesheet functions are refactored to produce good html
                        oXslt.Transform(reader, oXslArgList, result);
                    }
                    catch (Exception x)
                    {
                        string sErrorCheck = x.ToString();
                        model.ErrorMessage
                           = string.Concat(sErrorCheck, DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                           string.Empty, "STYLEHELPER_BADXMLORHTML"));
                    }
                    //help the GC out;
                    oXslArgList.Clear();
                }
                else
                {
                    model.ErrorMessage
                            = DevTreks.Exceptions.DevTreksErrors.MakeStandardErrorMsg(
                            string.Empty, "STYLEHELPER_NOSTYLE");
                }
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteSelectionsMenu(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                bool bDisableButtons = (model.URIDataManager.SelectedList == string.Empty)
                    ? true : false;
                bool bNeedsSingleQuote = false;
                string sSelectionsURIPattern = model.URIDataManager.SelectionsURIPattern;
                if (string.IsNullOrEmpty(sSelectionsURIPattern))
                {
                    sSelectionsURIPattern = model.URIPattern;
                }
                string sSelectParams = StylesheetHelper.SetSelectedLinkedViewParams(model, bNeedsSingleQuote);
                string sClientAction = GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString();
                string sContentURIPattern = GeneralHelpers.MakeContentURIPattern(
                    model.URIDataManager.ControllerName,
                    GeneralHelpers.SERVER_ACTION_TYPES.edit.ToString(),
                    sSelectionsURIPattern,
                    GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects.ToString(),
                    GeneralHelpers.NONE, GeneralHelpers.NONE);
                helper.MakeSelectionButtons(model, bDisableButtons, sContentURIPattern,
                    sClientAction, sSelectParams)
                    .WriteTo(result, HtmlEncoder.Default);
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString MakeSelectionButtons(this IHtmlHelper helper,
            ContentURI model, bool disableButtons,
            string contenturipattern, string clientaction, string extraparams)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty,
                    "ui-body ui-body-b", "controlgroup", "horizontal"));
                string sClass1 = "GetSelections";
                result.WriteLine(helper.InputUnobtrusiveMobile(
                       id: GeneralHelpers.SERVER_SUBACTION_TYPES.submitedits.ToString(),
                       classAttribute: sClass1,
                       type: "button",
                       contenturipattern: string.Empty,
                       clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                       extraParams: string.Empty,
                       value: AppHelper.GetResource("SELECT"),
                       dataMini: "true",
                       dataInline: "true",
                       dataIcon: string.Empty,
                       dataIconPos: string.Empty));
                sClass1 = "DeleteSelections";
                result.WriteLine(helper.InputUnobtrusiveMobile(
                       id: HtmlExtensions.DISCARD_SELECTS,
                       classAttribute: sClass1,
                       type: "button",
                       contenturipattern: string.Empty,
                       clientaction: GeneralHelpers.CLIENTACTION_TYPES.postrequest.ToString(),
                       extraParams: string.Empty,
                       value: AppHelper.GetResource("DISCARD"),
                       dataMini: "true",
                       dataInline: "true",
                       dataIcon: string.Empty,
                       dataIconPos: string.Empty));

                sClass1 = "SubmitButton1Enabled150";
                result.WriteLine(helper.InputUnobtrusiveMobile(
                       id: GeneralHelpers.SERVER_SUBACTION_TYPES.saveselects.ToString(),
                       classAttribute: sClass1,
                       type: "button",
                       contenturipattern: contenturipattern,
                       clientaction: clientaction,
                       extraParams: extraparams,
                       value: AppHelper.GetResource("SAVE"),
                       dataMini: "true",
                       dataInline: "true",
                       dataIcon: string.Empty,
                       dataIconPos: string.Empty));
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString WriteSelections(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                result.WriteLine(helper.DivStart(string.Empty, string.Empty));
                result.WriteLine(helper.StrongStart());
                result.Write(AppHelper.GetResource("SELECTED"));
                string sDisplayName = (model.URIDataManager.SelectedList == string.Empty) ?
                    StylesheetHelper.GetDisplayNameForSelections(model) :
                    StylesheetHelper.GetDisplayNameForSelections(model, model.URIDataManager.SelectedList);
                result.Write(sDisplayName);
                result.Write(":");
                result.WriteLine(helper.StrongEnd());
                string sClass = string.Empty;
                result.WriteLine(helper.FieldsetStart(string.Empty, string.Empty, 
                    "controlgroup", "false", "true"));
                result.WriteLine(helper.SpanStart("spanSelections", sClass));
                if (!string.IsNullOrEmpty(model.URIDataManager.SelectedList))
                {
                    string[] arrSelections = model.URIDataManager
                        .SelectedList.Split(GeneralHelpers.PARAMETER_DELIMITERS);
                    int i = 0;
                    if (arrSelections != null)
                    {
                        int iLength = arrSelections.Length;
                        string sSelectedChildParentURIPatterns = string.Empty;
                        string sDisplayCommonName = string.Empty;
                        for (i = 0; i < iLength; i++)
                        {
                            sSelectedChildParentURIPatterns = arrSelections[i];
                            result.WriteLine(helper.InputCheckBox(GeneralHelpers.VIEW_EDIT_TYPES.full,
                                sSelectedChildParentURIPatterns, "selectionpending", "checkbox",
                                sSelectedChildParentURIPatterns, "true", true));
                            sDisplayCommonName = ContentURI.GetURIPatternPart(
                                sSelectedChildParentURIPatterns, ContentURI.URIPATTERNPART.name);
                            result.WriteLine(helper.LabelRegular(sSelectedChildParentURIPatterns, 
                                sDisplayCommonName));
                        }
                    }
                }
                result.WriteLine(helper.SpanEnd());
                result.WriteLine(helper.FieldsetEnd());
                result.WriteLine(helper.DivEnd());
                return new HtmlString(result.ToString());
            }
        }
        
        public static HtmlString Select2(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                return new HtmlString(result.ToString());
            }
        }
        public static HtmlString Select3(this IHtmlHelper helper,
           ContentURI model)
        {
            using (StringWriter result = new StringWriter())
            {
                return new HtmlString(result.ToString());
            }
        }
    }
}
