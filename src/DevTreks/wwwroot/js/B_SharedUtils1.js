/*
    Name:SharedUtils.js
    Author: www.devtreks.org
    Date: 2016, May
    Purpose: enums, constants, methods shared across script objects
    Patterns: n/a -general helper functions, constants, enums ...
    no constructor
*/
///<reference path="jquery-1.5.1.js" />
//global variables
var SharedUtils = new Object();
SharedUtils.URIBASE = "http://www.devtreks.org/";
SharedUtils.XML_DEVDOCS = null;         
SharedUtils.INTERVAL_ID = 0;
SharedUtils.NUMBER_TO_DISPLAY = 1;
SharedUtils.NEW_WINDOW;
SharedUtils.DisplayErrorElId = "spanDisplayError";
SharedUtils.DisplayProgressId = "spanGettingData";
SharedUtils.SelectionsId = "spanSelections";
SharedUtils.TabId = null;
SharedUtils.BACK = "back";
SharedUtils.NEXT = "next";
SharedUtils.START_ROW = "startrow";
SharedUtils.OLDSTART_ROW = "oldstartrow";
SharedUtils.PARENTSTART_ROW = "parentstartrow";
SharedUtils.FILE_UPLOAD_PARAMS = "fileUploadParams1";
//either query strings or form element names 
//can be used to send ajax requests to server
SharedUtils.CONTENT_PATTERN = "cp";
SharedUtils.CLIENT_ACTION = "ca";
SharedUtils.URIPatternDelimiter = "/";
SharedUtils.QuestionDelimiter = "?";
SharedUtils.AndDelimiter = "&";
SharedUtils.EqualDelimiter = "=";
SharedUtils.NONE = "none";
//html 5 attributes supporting unobtrusive js
SharedUtils.DATA_CP = "data-contenturipattern";
SharedUtils.DATA_CA = "data-clientaction";
SharedUtils.DATA_PARAMS = "data-params";
SharedUtils.DATA_ID1 = "data-id1";
SharedUtils.DATA_ATTNAME = "data-attname";
SharedUtils.DATA_UP1 = "data-uripattern1";
SharedUtils.DATA_NODENAME = "data-nodename";
SharedUtils.DATA_UNITID = "data-unitid";
SharedUtils.DATA_FORMPARAM = "data-formparam";
SharedUtils.CLP = "colp";
//same as webconfig
SharedUtils.DefaultWebResourcesRelPath = "../../../../../../../resources/";
//one less because controller is added
SharedUtils.DefaultPOSTRelPath = "../../../../../../";
SharedUtils.HistoryArgs = "";
SharedUtils.UPDATE_PANEL_TYPES_none              = 0;
SharedUtils.UPDATE_PANEL_TYPES_search            = 1;
SharedUtils.UPDATE_PANEL_TYPES_preview           = 2;
SharedUtils.UPDATE_PANEL_TYPES_edit              = 3;
SharedUtils.UPDATE_PANEL_TYPES_select            = 4;
SharedUtils.UPDATE_PANEL_TYPES_pack              = 5;
SharedUtils.UPDATE_PANEL_TYPES_linkedviews       = 6;
SharedUtils.UPDATE_PANEL_TYPES_member            = 7;
SharedUtils.ONREQUESTCOMPLETED_xml              = 0;
SharedUtils.ONREQUESTCOMPLETED_xmlxslt          = 1;
SharedUtils.ONREQUESTCOMPLETED_xmltext          = 2;
////constants, enums
//client action
SharedUtils.CLIENTACTION_postrequest         = 0;
SharedUtils.CLIENTACTION_closeelement        = 1;
SharedUtils.CLIENTACTION_openwindow          = 2;
SharedUtils.CLIENTACTION_changeurl           = 3;
SharedUtils.CLIENTACTION_loaddoc             = 4;
SharedUtils.CLIENTACTION_discardselects	     = 5;
SharedUtils.CLIENTACTION_addtopack           = 6;
SharedUtils.CLIENTACTION_downloaddoc         = 7;
SharedUtils.CLIENTACTION_prepaddin           = 8;
//what main action should the server take to respond with a new view?
SharedUtils.SERVERACTION_none        = 0;
SharedUtils.SERVERACTION_search      = 1;
SharedUtils.SERVERACTION_preview     = 2;
SharedUtils.SERVERACTION_edit        = 3;
SharedUtils.SERVERACTION_select      = 4;
SharedUtils.SERVERACTION_pack        = 5;
SharedUtils.SERVERACTION_linkedviews = 6;
SharedUtils.SERVERACTION_member      = 7;
//what subaction should the server take to respond with a new view?
SharedUtils.SERVERSUBACTION_none                    = 0;
SharedUtils.SERVERSUBACTION_respondwithhtml         = 1;
SharedUtils.SERVERSUBACTION_searchbynetwork         = 2;
SharedUtils.SERVERSUBACTION_searchbyservice         = 3;
SharedUtils.SERVERSUBACTION_submitedits             = 4;
SharedUtils.SERVERSUBACTION_closeedits              = 5;
SharedUtils.SERVERSUBACTION_adddefaults             = 6;
SharedUtils.SERVERSUBACTION_uploadfile              = 7;
SharedUtils.SERVERSUBACTION_downloadfile            = 8;
SharedUtils.SERVERSUBACTION_saveselects             = 9;
SharedUtils.SERVERSUBACTION_savenewselects          = 10;
SharedUtils.SERVERSUBACTION_respondwithxml          = 11;
SharedUtils.SERVERSUBACTION_makepackage             = 12;
SharedUtils.SERVERSUBACTION_runaddin                = 13;
SharedUtils.SERVERSUBACTION_respondwithlist         = 14;
SharedUtils.SERVERSUBACTION_submitlistedits         = 15;
SharedUtils.SERVERSUBACTION_respondwithnewxhtml     = 16;
SharedUtils.SERVERSUBACTION_respondwithform         = 17;
SharedUtils.SERVERSUBACTION_submitformedits         = 18;
SharedUtils.SERVERSUBACTION_buildtempdoc            = 19;
//type of data being returned in response
SharedUtils.RESPONSETYPE_xml             = 0;
SharedUtils.RESPONSETYPE_xslt            = 1;
SharedUtils.RESPONSETYPE_html            = 2;
SharedUtils.RESPONSETYPE_text            = 3;
SharedUtils.RESPONSETYPE_json            = 4;
//full contenturipattern parts
SharedUtils.URIPATTERNPART_controller      = 1;
SharedUtils.URIPATTERNPART_action          = 2;
SharedUtils.URIPATTERNPART_network         = 3;
SharedUtils.URIPATTERNPART_node            = 4;
SharedUtils.URIPATTERNPART_name            = 5;
SharedUtils.URIPATTERNPART_id              = 6;
SharedUtils.URIPATTERNPART_fileextension   = 7;
SharedUtils.URIPATTERNPART_subaction       = 8;
SharedUtils.URIPATTERNPART_subactionview   = 9;
SharedUtils.URIPATTERNPART_variable        = 10;
        
function GetContentURIPatternPart(contentURIPattern, uriPartIntEnum)
{
    //returns a substring from a uripattern
    var sURIPatternPart = "";
    if (contentURIPattern && uriPartIntEnum) {
        if (isNaN(uriPartIntEnum) == false) {
            //uripart enum is a one based index, but get substring uses a zero index
            var iZeroBasedIndex = uriPartIntEnum - 1;
            sURIPatternPart = GetSubStringWithDelimiter(contentURIPattern, SharedUtils.URIPatternDelimiter, iZeroBasedIndex);
        }
    }
    return sURIPatternPart;
}
function ChangeContentURIPatternPart(contentURIPattern, uriPartIntEnum, newPart) {
    var sNewContentURIPattern = contentURIPattern;
    if (contentURIPattern && uriPartIntEnum && newPart) {
        //replaces part of the contenturipattern
        var sURIPatternPart = GetContentURIPatternPart(contentURIPattern, uriPartIntEnum);
        if (sURIPatternPart != undefined && sURIPatternPart != null
            && isNaN(uriPartIntEnum) == false) {
            var iZeroBasedIndex = uriPartIntEnum - 1;
            sNewContentURIPattern = ReplaceArrayMember(contentURIPattern, SharedUtils.URIPatternDelimiter, iZeroBasedIndex, newPart);
        }
    }
    return sNewContentURIPattern;
}
function MakeContentURIPattern(controller, serveraction, network, 
    node, name, id, fileextension, subaction, subactionview, variable){
    var contenturipattern = controller;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += serveraction;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += network;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += node;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += name;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += id;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += fileextension;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += subaction;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += subactionview;
    contenturipattern += SharedUtils.URIPatternDelimiter;
    contenturipattern += variable;
    return contenturipattern;
}
function MakeContentURIPatternUsingURIPattern(controller, action, 
    uriPattern, subActionType, subActionView, variable)
{
    var sContentURIPattern = "";
    if (controller)
    {
        sContentURIPattern = controller;
        sContentURIPattern += SharedUtils.URIPatternDelimiter;
        sContentURIPattern += action;
        sContentURIPattern += SharedUtils.URIPatternDelimiter;
        sContentURIPattern += MakePartialContentURIPattern(uriPattern,
            subActionType, subActionView, variable);
    }
    return sContentURIPattern;
}
function MakePartialContentURIPattern(uriPattern, subActionType, 
    subActionView, variable)
{
    var sContentURIPattern = "";
    if (uriPattern && subActionType)
    {
        sContentURIPattern = uriPattern;
        var iURIPatternLength = GetArrayLength(uriPattern);
        if (iURIPatternLength == 4)
        {
            //existing route doesn't include a filextension,
            //add one so that the route has fixed params
            //crops/input/fert/123/none/respondwithlist/listview
            sContentURIPattern += SharedUtils.URIPatternDelimiter;
            sContentURIPattern += SharedUtils.NONE;
        }
        sContentURIPattern += SharedUtils.URIPatternDelimiter;
        sContentURIPattern += subActionType;
        sContentURIPattern += SharedUtils.URIPatternDelimiter;
        sContentURIPattern += subActionView;
        sContentURIPattern += SharedUtils.URIPatternDelimiter;
        sContentURIPattern += variable;
    }
    return sContentURIPattern;
}
function SetClientURIPatternParams(presenter, contentURIPattern){
    presenter.controller = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_controller);
    presenter.serveraction = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_action);
    presenter.network = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_network);
    presenter.node = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_node);
    presenter.name = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_name);
    presenter.id = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_id);
    presenter.fileextension = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_fileextension);
    presenter.subaction = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_subaction);
    presenter.subactionview = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_subactionview);
    presenter.variable = GetContentURIPatternPart(contentURIPattern, SharedUtils.URIPATTERNPART_variable);
}
function MakeHistoryArgs(fileName, clientaction, serveraction, subaction, params){
    var sHistoryArgs = fileName + "?" + clientaction + "?" + serveraction + "?" + subaction + "?" + params;
    SharedUtils.HistoryArgs = sHistoryArgs;
    return sHistoryArgs;
}
function GetProgressId(clientaction){
    var sProgressId = SharedUtils.DisplayProgressId;
    if (clientaction == SharedUtils.CLIENTACTION_downloaddoc){
        //change where the progess bar goes
        sProgressId = "spanMakingPack";
    }
    return sProgressId;
}
function GetResponseType(responseType){
    var sResponseType = "html";
     switch(responseType){
        case SharedUtils.RESPONSETYPE_xml:
            sResponseType = "xml";
            break;
       case SharedUtils.RESPONSETYPE_text:
            sResponseType = "text";
            break;
        case SharedUtils.RESPONSETYPE_html:
            sResponseType = "html";
            break;
       case SharedUtils.RESPONSETYPE_json:
            sResponseType = "json";
            break;
        case SharedUtils.RESPONSETYPE_xslt:
            sResponseType = "xml";
            break;
        default:
            sResponseType = "html";
            break;
    }
    return sResponseType;
}
function ConfirmPassword(){
    var oPasswordBox = GetOneElement('MemberPassword');
    if ($(oPasswordBox).length > 0) {
        var sPassword = RemoveLastAsterisk($(oPasswordBox).val());
        var oConfirmPasswordBox = GetOneElement('lblConfirmPassword');
        if ($(oConfirmPasswordBox).length > 0) {
            //add the asterisk so it will be validated again on server
            IsUpdated(oConfirmPasswordBox);
            var sConfirmedPassword = RemoveLastAsterisk($(oConfirmPasswordBox).val());
            var oSpan = GetOneElement('spanNotConfirmed');
            if ($(oSpan).length > 0) {
                var oEditButton;
                if (sConfirmedPassword == sPassword){
                    $(oSpan).hide();
                    oEditButton = GetOneElement('submitedits');
                    if ($(oEditButton).length > 0){
                        $(oEditButton).parent().show();
                    }
                }
                else {
	                $(oSpan).show();
	                oEditButton = GetOneElement('submitedits');
                    if ($(oEditButton).length > 0){
                        $(oEditButton).parent().hide();
                    }
                }
            }
        }
    }
}
function SetAttributeValue(elId, attName, attValue){
    var oEl = (typeof(elId) == "object") ? elId : GetOneElement(elId);
    if ($(oEl).length > 0) {
        $(oEl).attr(attName, attValue);
    }
}
function GetAttributeValue(elId, attName){
    var sAttValue;
    var oEl = (typeof(elId) == "object") ? elId : GetOneElement(elId);
    if ($(oEl).length > 0) {
        sAttValue = $(oEl).attr(attName);
    }
    return sAttValue;
}
function SetPropertyValue(elId, attName, attValue) {
    var oEl = (typeof (elId) == "object") ? elId : GetOneElement(elId);
    if ($(oEl).length > 0) {
        $(oEl).prop(attName, attValue);
    }
}
function GetPropertyValue(elId, attName) {
    var sAttValue;
    var oEl = (typeof (elId) == "object") ? elId : GetOneElement(elId);
    if ($(oEl).length > 0) {
        sAttValue = $(oEl).prop(attName);
    }
    return sAttValue;
}
function Focus(elId) {
    var oEl = (typeof(elId) == "object") ? elId : GetOneElement(elId);
    if ($(oEl).length > 0) {
        $(oEl).focus();
    }
}
function IsErrorMessage(response){
    var bIsErrorMessage = false;
    //error messages start with constant string
    var regError = new RegExp("^Error:");
    bIsErrorMessage = regError.test(response);
    return bIsErrorMessage;
}
function StartsWith(startsWithText, fullText){
    var bStartsWith = false;
    var regExText = "^" + startsWithText;
    //error messages start with constant string
    var regError = new RegExp(regExText);
    bStartsWith = regError.test(fullText);
    return bStartsWith;
}
function EndsWith(endsWithText, fullText) {
    var bEndsWith = false;
    var regExText = endsWithText + "$";
    //error messages start with constant string
    var regError = new RegExp(regExText);
    bEndsWith = regError.test(fullText);
    return bEndsWith;
}
function AddMessageHtml(msg, isError){
    //this needs a postback for the html to display; no postback? preformat the span
    //put all messages, including errors in Home.Master.spanDisplayError
    var spanError = $("#" + SharedUtils.DisplayErrorElId);
    var spanError = GetOneElement(SharedUtils.DisplayErrorElId);
    if ($(spanError).length > 0) {
        var sColor = (isError) ? 'Grey' : 'Black';
        if ($(spanError).length > 0) {
            $(spanError).html('<br /><br /><span style=\"color:' + sColor + '\;"><strong>' + msg + '</strong></span><br /><br />');
            $(spanError).show();
        }
    }
    spanError = GetOneElement(SharedUtils.DisplayErrorElId + "2");
    if ($(spanError).length > 0) {
    }
}
function AppendMessageHtml(parentNode, msg, isError){
    var oParentNode = (typeof(parentNode) == "object") ? parentNode : GetOneElement(parentNode);
    var sColor = (isError) ? 'Red' : 'Black';
    if ($(oParentNode).length > 0) {
        $(oParentNode).html('<br /><span style=\"color:' + sColor + '\;">' + msg + '</span>');
        $(oParentNode).show();
    }
}
function HasServerErrorMessage() {
    var hasErrorMsg = false;
    spanError = $("#" + SharedUtils.DisplayErrorElId + "2");
    if ($(spanError).length > 0) {
        //refactor: not being used and not debugged
        var errorMsg = $(spanError).innerHTML;
        if (errorMsg) {
            //default has "Error:"
            if (errorMsg.length > 10) {
                hasErrorMsg = true;
            }
        }
    }
    return hasErrorMsg;
}
function ClearErrorMessage(){
    //errors returned in response
    var spanError = GetOneElement(SharedUtils.DisplayErrorElId);
    if ($(spanError).length > 0) {
        $(spanError).attr("id", "none");
        $(spanError).html("");
        $(spanError).hide();
    }
    spanError = GetOneElement(SharedUtils.DisplayErrorElId + "2");
    //$("#" + SharedUtils.DisplayErrorElId + "2");
    if ($(spanError).length > 0) {
        $(spanError).attr("id", "none");
        $(spanError).html("");
        $(spanError).hide();
    } 
}
function GetSelectValue(serverSidePrefix, selectBoxId){
    var oSelectBox; var sSelectValue;
    oSelectBox = $("select#" + selectBoxId);
    if (oSelectBox.length > 0) {
        sSelectValue = oSelectBox.val();
    }
    if (sSelectValue == null || sSelectValue == ""){
        if (serverSidePrefix != null){
            sSelectValue = $("select#" + serverSidePrefix + selectBoxId).val();
        }
    }
    return sSelectValue;
}
function GetURIPattern(currentName, currentId, networkPartName, currentNodeName, 
    fileExtensionType, subAction, subActionView, linkedViewId){
    var sURIPattern;
    sURIPattern = networkPartName + SharedUtils.URIPatternDelimiter + currentNodeName + SharedUtils.URIPatternDelimiter + currentName + SharedUtils.URIPatternDelimiter + currentId;
    if (fileExtensionType != undefined && fileExtensionType != null) {
        sURIPattern += SharedUtils.URIPatternDelimiter + fileExtensionType;
    }
    if (subAction != undefined && subAction != null) {
        sURIPattern += SharedUtils.URIPatternDelimiter + subAction;
    }
    if (subActionView != undefined && subActionView != null) {
        sURIPattern += SharedUtils.URIPatternDelimiter + subActionView;
    }
    if (linkedViewId != undefined && linkedViewId != null) {
        sURIPattern += SharedUtils.URIPatternDelimiter + linkedViewId;
    }
    return sURIPattern;
}
function GetControllerPattern(controller, controlleraction, currentName, currentId, networkPartName, currentNodeName){
    var sURIPattern;
    sURIPattern = controller + SharedUtils.URIPatternDelimiter + controlleraction + SharedUtils.URIPatternDelimiter + networkPartName + SharedUtils.URIPatternDelimiter + currentNodeName + SharedUtils.URIPatternDelimiter + currentName + SharedUtils.URIPatternDelimiter + currentId;
    return sURIPattern;
}
function IsMatch(regx, response){
    var bIsMatch = false;
    var regError = new RegExp(regx);
    bIsMatch = regError.test(response);
    return bIsMatch;
}
function HasSubString(fullString, subString){
    var hasstring = false;
    if (fullString != undefined && fullString != null
        && subString != undefined && subString != null) {
        var lfullString = fullString.toLowerCase();
        var lsubString = subString.toLowerCase();
        var index = lfullString.indexOf(lsubString);
        if (index >= 0){
            hasstring = true;
        }
    }
    return hasstring;
}
function GetRadioValue(radioName) {
    var sValue = "";
    var oCheckBox = $("input[name='" + radioName + "']:checked");
    if (oCheckBox.length > 0) {
        sValue = oCheckBox.val();
        return sValue;
    }
}
function ReplaceString(stringToReplace, replaceString, withString){
    var regReplace = new RegExp(replaceString, "g");
    var sNewString = stringToReplace.replace(regReplace, withString);
    return sNewString;
}
function LoadAjaxLinkContent(displayElementId){
    //switch tabs
    var oTabChanger = new ChangeMvcTabView.ViewChanger(event);
}
function DisplayAsynchFeedback(span, msg) {
    //show the page loader
    $.mobile.loading('show');
}

function ClearInterval(spanId, displayElementId){
    if (SharedUtils.INTERVAL_ID != null){
        var iIntervalId = parseInt(SharedUtils.INTERVAL_ID);
        window.clearInterval(iIntervalId);
        SharedUtils.INTERVAL_ID = 0;
    }
    //hide the page loader
    $.mobile.loading('hide');
    if (displayElementId) {
        FadeIn(displayElementId);
    }
}
function ShowCollapsedDiv(editURIPattern) {
    //show collapsed content after edits are made to the content
    //the editURIpattern is a short pattern
    var sId = GetContentURIPatternPart(editURIPattern, 4);
    if (sId) {
        var sCollapsedDivId = SharedUtils.CLP + sId;
        SetAttributeValue(sCollapsedDivId, "data-collapsed", "false");
    }
}
function FadeIn(displayElementId) {
    var sId = '#' + displayElementId;
    $(sId).fadeIn('normal');
}
function FadeOut(displayElementId) {
    var sId = '#' + displayElementId;
    $(sId).fadeOut('normal');
}
function ResetOldInterValId(elId){
    var oHiddenIntervalEl = GetOneElement(elId);
    if ($(oHiddenIntervalEl).length > 0) {
        if (oHiddenIntervalEl.value.length > 0) {
            var iIntervalId = parseInt(oHiddenIntervalEl.value);
            SharedUtils.INTERVAL_ID = iIntervalId;
        }
    }
}
function GetDisplayElementName(updatePanelType, subaction){
    //where do server responses get displayed?
    var sDisplayElName = null;
    subaction = isNaN(subaction) ? GetServerSubAction(subaction) : subaction;
    switch(updatePanelType){
        case SharedUtils.UPDATE_PANEL_TYPES_search:
            sDisplayElName = "divsearch";
            break;
        case SharedUtils.UPDATE_PANEL_TYPES_preview:
            sDisplayElName = "divpreview";
            break;
        case SharedUtils.UPDATE_PANEL_TYPES_select:
            sDisplayElName = "divselect";
            break;
        case SharedUtils.UPDATE_PANEL_TYPES_edit:
            sDisplayElName = "divedit";
            break;
        case SharedUtils.UPDATE_PANEL_TYPES_linkedviews:
            sDisplayElName = "divlinkedviews";
            break;
        case SharedUtils.UPDATE_PANEL_TYPES_pack:
            if (subaction == SharedUtils.SERVERSUBACTION_makepackage) {
                sDisplayElName = "divDownloadPackLink";
            }
            else {
                sDisplayElName = "divpack";
            }
            break;
        case SharedUtils.UPDATE_PANEL_TYPES_member:
            sDisplayElName = "divmember";
            break;
        default:
            break;
    }
    return sDisplayElName;
}

function GetEventElementValue(e){
    var value = 0;
    if (e.currentTarget){
        value = e.currentTarget.value;
    }
    else {
        if (e.target){
            value = e.target.value;
        }
    }
    return value;
}
function GetIdFromEvent(e){
    var id = 0;
    if (e.currentTarget){
        id = e.currentTarget.id;
    }
    else {
        if (e.target){
            id = e.target.id;
        }
    }
    return id;
}
function GetClassFromEvent(e){
    var classAtt = "";
    if (e){
        if (e.currentTarget){
            classAtt = e.currentTarget.className;
        }
        else {
            if (e.target){
                classAtt = e.target.className;
            }
        }
    }
    return classAtt;
}
function GetURIPatternFromEvent(e){
    var uriPattern = "";
    var uri = "";
    if (e.currentTarget){
        if (e.currentTarget.pathname){
            uriPattern = e.currentTarget.pathname;
        }
        else if (e.currentTarget.search){
            uriPattern = GetLastSubstring(e.currentTarget.search, SharedUtils.EqualDelimiter);
        }
        else {
            uri = window.location.href;
            if (uri != null){
                //by convention, uripatterns don't include the controller or action
                uriPattern = GetLastSubstringUseIndex(uri, SharedUtils.URIPatternDelimiter, 5);
            }
        }
    }
    else {
        if (e.target.pathname){
            uriPattern = e.target.pathname;
        }
        else if (e.target.search){
            uriPattern = GetLastSubstring(e.target.search, SharedUtils.EqualDelimiter);
        }
        else {
            uri = window.location.href;
            if (uri != null){
                //by convention, uripatterns don't include the controller or action
                uriPattern = GetLastSubstringUseIndex(uri, SharedUtils.URIPatternDelimiter, 5);
            }
        }
    }
    return uriPattern;
}
function GetServerActionFromLocation(){
    var action = SharedUtils.SERVERACTION_none;
    var uri = window.location.href;
    //by convention, uripatterns don't include the controller or action
    if (uri != null){
        action = GetSubStringWithDelimiter(uri, SharedUtils.URIPatternDelimiter, 4);
            if (action != null) {
            action = GetServerAction(action);
        }
    }
    return action;
}

function GetServerAction(serveraction) {
    var eServerAction;
    if (isNaN(serveraction)) {
        if (serveraction == "SharedUtils.SERVERACTION_search"
            || serveraction == "search"){
            eServerAction = SharedUtils.SERVERACTION_search;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_preview"
            || serveraction == "preview"){
            eServerAction = SharedUtils.SERVERACTION_preview;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_edit"
            || serveraction == "edit"){
            eServerAction = SharedUtils.SERVERACTION_edit;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_select"
            || serveraction == "select"){
            eServerAction = SharedUtils.SERVERACTION_select;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_pack"
            || serveraction == "pack"){
            eServerAction = SharedUtils.SERVERACTION_pack;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_member"
            || serveraction == "member"){
            eServerAction = SharedUtils.SERVERACTION_member;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_linkedviews"
            || serveraction == "linkedviews"){
            eServerAction = SharedUtils.SERVERACTION_linkedviews;
        }
        else if (serveraction == "SharedUtils.SERVERACTION_preview"
            || serveraction == "preview"){
            eServerAction = SharedUtils.SERVERACTION_preview;
        }
    }
    else {
        eServerAction = parseInt(serveraction);
    }
    return eServerAction;
}
function GetServerSubAction(subaction) {
    //cross browser support
    var eServerSubAction;
    if (isNaN(subaction)) {
        if (subaction == "SharedUtils.SERVERSUBACTION_none"
            || subaction == "none"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_none;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_respondwithhtml"
            || subaction == "respondwithhtml"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_respondwithhtml;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_searchbynetwork"
            || subaction == "searchbynetwork"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_searchbynetwork;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_searchbyservice"
            || subaction == "searchbyservice"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_searchbyservice;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_submitedits"
            || subaction == "submitedits"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_submitedits;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_closeedits"
            || subaction == "closeedits"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_closeedits;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_adddefaults"
            || subaction == "adddefaults"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_adddefaults;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_uploadfile"
            || subaction == "uploadfile"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_uploadfile;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_downloadfile"
            || subaction == "downloadfile"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_downloadfile;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_saveselects"
            || subaction == "saveselects"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_saveselects;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_buildtempdoc"
            || subaction == "buildtempdoc") {
            eServerSubAction = SharedUtils.SERVERSUBACTION_buildtempdoc;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_savenewselects"
            || subaction == "savenewselects"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_savenewselects;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_respondwithxml"
            || subaction == "respondwithxml"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_respondwithxml;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_makepackage"
            || subaction == "makepackage"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_makepackage;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_runaddin"
            || subaction == "runaddin"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_runaddin;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_respondwithlist"
            || subaction == "respondwithlist"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_respondwithlist;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_submitlistedits"
            || subaction == "submitlistedits"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_submitlistedits;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_respondwithnewxhtml"
            || subaction == "respondwithnewxhtml"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_respondwithnewxhtml;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_respondwithform"
            || subaction == "respondwithform"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_respondwithform;
        }
        else if (subaction == "SharedUtils.SERVERSUBACTION_submitformedits"
            || subaction == "submitformedits"){
            eServerSubAction = SharedUtils.SERVERSUBACTION_submitformedits;
        }
    }
    else {
        eServerSubAction = parseInt(subaction);
    }
    return eServerSubAction;
}
function GetClientAction(clientaction) {
    var eClientAction;
    if (isNaN(clientaction)) {
        if (clientaction == "SharedUtils.CLIENTACTION_postrequest"
            || clientaction == "postrequest"){
            eClientAction = SharedUtils.CLIENTACTION_postrequest;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_closeelement"
            || clientaction == "closeelement"){
            eClientAction = SharedUtils.CLIENTACTION_closeelement;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_changeurl"
            || clientaction == "changeurl"){
            eClientAction = SharedUtils.CLIENTACTION_changeurl;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_openwindow"
            || clientaction == "openwindow"){
            eClientAction = SharedUtils.CLIENTACTION_openwindow;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_loaddoc"
            || clientaction == "loaddoc"){
            eClientAction = SharedUtils.CLIENTACTION_loaddoc;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_addtopack"
            || clientaction == "addtopack"){
            eClientAction = SharedUtils.CLIENTACTION_addtopack;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_downloaddoc"
            || clientaction == "downloaddoc"){
            eClientAction = SharedUtils.CLIENTACTION_downloaddoc;
        }
        else if (clientaction == "SharedUtils.CLIENTACTION_prepaddin"
            || clientaction == "prepaddin"){
            eClientAction = SharedUtils.CLIENTACTION_prepaddin;
        }
    }
    else {
        eClientAction = parseInt(clientaction);
    }
    return eClientAction;
}
function GetLastSubstring(fullString, delimiter){
    var sSubString = "";
    var iStartIndex = fullString.lastIndexOf(delimiter);
    if (iStartIndex > 0) {
        sSubString = fullString.substring(iStartIndex + 1);
    }
    return sSubString;
}
function GetLastSubstringUseIndex(fullString, delimiter, index){
    var arrSubs = fullString.split(delimiter);
    var sSubString = "";
    if (arrSubs) {
        if (arrSubs.length > index){
            for(var i = index; i < arrSubs.length; i++) {
                if (sSubString != "")
                    sSubString += delimiter;
                    sSubString += arrSubs[i];
            }
        }
    }
    return sSubString;
}
function GetLastChar(fullstring){
    var lastChar = "";
    if (fullstring != undefined && fullstring != null) {
        var indexToStart = fullstring.length - 1;
        lastChar = fullstring.substring(indexToStart, fullstring.length);
    }
    return lastChar;
}
function GetSubStringWithDelimiter(fullString, delimiter, index){
    var sSubString = "";
    if (fullString) {
        var arrSubs = fullString.split(delimiter);
        if (arrSubs) {
            if (arrSubs.length > index){
                var firstChar = fullString.substring(0, 1);
                if (firstChar != delimiter){
                    sSubString = arrSubs[index];
                }
                else {
                    sSubString = arrSubs[index + 1];
                }
            }
        }
    }
    return sSubString;
}
function ReplaceArrayMember(fullString, delimiter, index, newPart) {
    var replacedArray = "";
    if (fullString) {
        var arrSubs = fullString.split(delimiter);
        if (arrSubs) {
            if (arrSubs.length > index) {
                var firstChar = fullString.substring(0, 1);
                if (firstChar == delimiter) {
                    index = index + 1;
                }
                for (var i = 0; i < arrSubs.length; i++) {
                    if (replacedArray != "")
                        replacedArray += delimiter;
                    if (i != index) {
                        replacedArray += arrSubs[i];
                    }
                    else {
                        replacedArray += newPart;
                    }
                }
            }
        }
    }
    return replacedArray;
}
function GetOneElement(divId){
    var el = null;
    if (divId != undefined && divId != null) {
        //has to return a jquery object
        try {
            el = (typeof(divId) == "object") ? divId : $("#" + divId.replace(/:/g, "\\:").replace(/\./g, "\\."));
	    }catch(ex){
	        var el1 = document.getElementById(divId);
	        el = $(el1);
	    }
	}
    return el;
}
function DisplayElementsByName(elementName, needsDisplay){
    var lstEls = ("#" + elementName);
    if ($(lstEls).length > 0) {
        $(lstEls).each(function (i) {
            $(this).toggle();
        });
    }
}
function DisplayElementByName(divId){
    var divEl = (typeof(divId) == "object") ? divId : GetOneElement(divId);
    if ($(divEl).length > 0) {
        $(divEl).toggle();
    }
}
function DisplayDivById(divId){
    var divEl = (typeof(divId) == "object") ? divId : GetOneElement(divId);
    if ($(divEl).length > 0) {
        $(divEl).toggle();
    }
    else {
        var selector = 'div[id=' + divId + ']';
        if ($(divEl).length > 0) {
            $(divEl).toggle();
        }
    }
}
function GetResource(resourceId, resourceFilePath, id, resourceName){
    var lnkEl = (typeof(resourceId) == "object") ? resourceId : GetOneElement(resourceId);
    if ($(lnkEl).length > 0) {
        lnkEl.href = resourceFilePath + id + "\\" + resourceName;
    }
}
function ShowThumbnail(imageid) {
    var image1;  var image2; var width = 30; var sqdimension = 100;
    image1 = GetOneElement(imageid);
    image2 = GetOneElement(imageid);
    if ($(image1)) {
        if (typeof ($(image1).width) == "function") {
            if ($(image1).attr("width")) {
                width = $(image1).attr("width");
            }
        }
        else if (typeof ($(image1).width) == "number") {
            width = $(image1).width;
        }
        if (width == 30) {
            sqdimension = 250;
        }
        else if (width == 250) {
            sqdimension = 30;
        }
        else if (width == 100) {
            sqdimension = 95;
        }
        else if (width == 95) {
            sqdimension = 100;
        }
        else {
            if (width >= 250) {
                sqdimension = width / 2;
            }
            else {
                sqdimension = width * 2;
            }
        }
        if (sqdimension == 95) {
            $(image2).animate({
                width: (sqdimension + "%"),
                height: (sqdimension + "%")
            }, 0);
        }
        else {
            $(image2).animate({
                width: (sqdimension + "px"),
                height: (sqdimension + "px")
            }, 0);
        }
    }
}

function ShowThumbnailInNewWindow(imageId){
    //hold for reference
    var oThumbnail = (typeof(imageId) == "object") ? imageId : GetOneElement(imageId);
    if ($(oThumbnail).length > 0) {
        //blow it up so they can see whether or not to open and look at
        if (!SharedUtils.NEW_WINDOW || SharedUtils.NEW_WINDOW.closed){
            //show them in one window
            SharedUtils.NEW_WINDOW = window.open("", "", "");
            SharedUtils.NEW_WINDOW.moveTo(0,0);
        }
        if (SharedUtils.NEW_WINDOW) {
            var sHtml = '<html><head></head><body><div align="center">';
            //rules needed on image size - need a standard thumbnail and full image
            //other media need accomodation
            sHtml += '<img src="' + oThumbnail.src + '" alt="" height="500" width="500" />';
            sHtml += '</div></body></html>';
            var oDoc = SharedUtils.NEW_WINDOW.document;
            oDoc.write(sHtml);;
            oDoc.close();
        }
    }
    return true;
}
function DoNamesMatch(firstName, secondName){
    var sName = (typeof(secondName) == "object") ? secondName.toString() : secondName;
    var bIsMatch = IsMatch(firstName, sName);
    return bIsMatch;
}
function HighlightLinkClicked(id){
    //id is composed of nodename+id
    var oLink = GetOneElement(id);
    if ($(oLink).length > 0) {
        var hiliteText;
        if ($(oLink).text().length > 0) {
            hiliteText = $(oLink).text().replace($(oLink).text(), $(oLink).text() + "*");
            $(oLink).text(hiliteText);
        }
    }
}
function HighlightElementClicked(linkClickedURIPattern){
    //id is composed of nodename+id
    var sNodeName = GetSubStringWithDelimiter(linkClickedURIPattern, 
        SharedUtils.URIPatternDelimiter, 1);
    var sElementId = sNodeName + GetSubStringWithDelimiter(linkClickedURIPattern, 
        SharedUtils.URIPatternDelimiter, 3);
    var oLink = GetOneElement(sElementId);
    if ($(oLink).length > 0) {
        var hiliteText;
        if ($(oLink).text().length > 0) {
            hiliteText = $(oLink).text().replace($(oLink).text(), $(oLink).text() + "*");
            $(oLink).text(hiliteText);
        }
        else {
        }
    }
}
function IsJQSelectionUpdated() {
    var select = $(this);
    if (select != null) {
        select = select[0];
        if (select != null) {
            var iItem = select.selectedIndex;
            if (select.options != null) {
                var oForm = new Form.FormFilter();
                var bHasAsterisk = oForm.lastCharIsMatch(select.options[iItem].value);
                if (!bHasAsterisk) {
                    select.options[iItem].value = select.options[iItem].value + "*";
                }
            }
            //special updates
            IsUnitUpdated(select);
        }
    }
}
function IsSelectionUpdated(){
    var iItem = this.selectedIndex;
    if (this.options != null) {
        this.options[iItem].value = this.options[iItem].value + "*";
    }
}
function IsUpdated(input){
    if (input != null) {
        if ($(input).val().length > 0) {
            var sNewValue = $(input).val() + "*";
            $(input).prop("value", sNewValue);
        }
    }
}
function IsJQUpdated() {
    var input = $(this);
    if (input != null) {
        if ($(input).length > 0) {
            var sOldValue = $(input).val();
            if (sOldValue == "") {
                sOldValue = SharedUtils.NONE;
            }
            var oForm = new Form.FormFilter();
            var bHasAsterisk = oForm.lastCharIsMatch(sOldValue);
            if (!bHasAsterisk) {
                var sNewValue = sOldValue + "*";
                if ($(input).prop("type") == "checkbox") {
                    if ($(input).prop("checked")) {
                        if (($(input).prop("name") != "RememberMe")){
                            //checkbox props change but atts don't, so need to dynamically change them for xml 
                            $(input).prop("value", "true*");
                        }
                    }
                    else {
                        $(input).prop("value", "false*");
                    }
                }
                else if (($(input).prop("name") == "UserName")
                    || ($(input).prop("name") == "Email")
                    || EndsWith("Password", $(input).prop("name"))) {
                    //aspnet identity uses client validation
                }
                else {
                    $(input).prop("value", sNewValue);
                }
            }
        }
    }
}
function IsFileNameUpdated(inputBox){
    var sFileName = inputBox.value;
    if (sFileName) {
        var iFileNameLength = sFileName.length;
        if (iFileNameLength > 75) sFileName = ShortenName(sFileName, 75);
        iStartFileExt =  sFileName.lastIndexOf(".");
        var bNeedsErrorMsg = false;
        if (iStartFileExt > -1){
            var sFileExt = sFileName.substr(iStartFileExt).toLowerCase();
            if (sFileExt) {
                if (IsMatch(".xslt$", sFileExt) || IsMatch(".xsl$", sFileExt)
                    || IsMatch(".xml$", sFileExt) || IsMatch(".xhtml$", sFileExt)
                    || IsMatch(".html$", sFileExt) || IsMatch(".htm$", sFileExt)
                    || IsMatch(".txt$", sFileExt) || IsMatch(".csv$", sFileExt) 
                 ){
                    //ok to update on server
                    $(inputBox).val() = sFileName + "*";
                    return;
                }
                else {
                    bNeedsErrorMsg = true;
                }
            }
        }
        else {
            bNeedsErrorMsg = true;
        }
    }
    if (bNeedsErrorMsg) {
        AppendMessageHtml(inputBox.parentNode, "A file name must be less than 75 characters in length and end with an allowed file name extension (i.e. .xml, .xslt, .svg).", bNeedsErrorMsg);
        $(inputBox).focus();
    }
}
function ShortenName(name, maxLength){
    var sNewName = name.slice(0, maxLength);
    return sNewName;
}

//keep input oc and aoh synchronized
function IsPriceUpdated(inputBox){
    var sSelectedValue = $(inputBox).val();
    if ($(inputBox).length > 0 && sSelectedValue != undefined && sSelectedValue != null) {
        var sPriceBoxName = $(inputBox).attr("name");
        if (sPriceBoxName != undefined && sPriceBoxName != null) {
            var sNewPriceBoxName;
            var re = /OCPrice/gi;
            sNewPriceBoxName = sPriceBoxName.replace(re, "AOHPrice");
            var newPriceBox = $("input[name='" + sNewPriceBoxName + "']");
            if (newPriceBox.length > 0) {
                newPriceBox.val(sSelectedValue);
            }
        }
    }
}
function CloseDiv(serveraction, subaction){
    var isClosed = false;
    var elToClose;
    if (subaction == SharedUtils.SERVERSUBACTION_closeedits
        || subaction == "closeedits"){
        if (serveraction == SharedUtils.SERVERACTION_edit
            || serveraction == "edit"){
            elToClose = GetOneElement("divEditsSection");
            if ($(elToClose).length > 0){
                $(elToClose).html = "";
                $(elToClose).hide();
                return true;
            }
        }
        else if (serveraction == SharedUtils.SERVERACTION_linkedviews
            || serveraction == "linkedviews"){
            elToClose = GetOneElement("divLinkedViewsSection");
            if ($(elToClose).length > 0) {
                $(elToClose).html = "";
                $(elToClose).hide();
                return true;
            }
        }
    }
}
//use the client to fill the unit lists
function SwitchUnits(selectToFill, unitGroupId) {
    var sUnitGroupId = unitGroupId.replace(/"/g, "");
    if (unitGroupId == null) sUnitGroupId = "1";
    if (sUnitGroupId != "1" && sUnitGroupId != "1001") sUnitGroupId = "1";
    if ($(selectToFill).length > 0) {
        var sSelectedValue = $(selectToFill).val();
        var oChildren = $(selectToFill).children("option");
        if ($(oChildren).length <= 1) {
            var oSelectToGet = $('#divUnits').find("select[id='" + sUnitGroupId + "']");
            if ($(oSelectToGet).length > 0) {
                $(selectToFill).empty();
                //append the options
                var childrenUnits = $(oSelectToGet).children("option").clone(true);
                if ($(childrenUnits).length > 0) {
                    $(selectToFill).append($(childrenUnits));
                    if (sSelectedValue != "" && sSelectedValue != "undefined" & sSelectedValue != null) {
                        $(selectToFill).children("option[value='" + sSelectedValue + "']").attr("selected", "selected");
                    }
                }
            }
        }
    }
}

function ChangeSelection(selectionListId, selectedValue){
    var selectionListEl = $("select#" + selectionListId);
    if (selectionListEl != null && selectionListEl != "undefined") {
        if (selectionListEl.length > 0) {
            selectionListEl.children("option[value='" + selectedValue + "']").prop("selected", "selected");
        }
    }
}
function IsUnitUpdated(selectBox){
    //this refers to the element (select) that raised the event
    var $oSelectUnits = $(selectBox);
    if ($oSelectUnits.length > 0) {
        if ($oSelectUnits.attr("id") != undefined && $oSelectUnits.attr("id") != null){
            if ($oSelectUnits.attr("id").toLowerCase().indexOf("ocunit") != -1){
                var sSelectedValue = $oSelectUnits.val();
                var oForm = new Form.FormFilter();
                var bHasAsterisk = oForm.lastCharIsMatch(sSelectedValue);
                if (!bHasAsterisk) {
                    sSelectedValue = sSelectedValue + "*";
                    IsSelectionUpdated();
                }
                var sUnitBoxName = $oSelectUnits.attr("name");
                if (sUnitBoxName != undefined && sUnitBoxName != null) {
                    var sNewUnitBoxName;
                    var re = /OCUnit/gi;
                    sNewUnitBoxName = sUnitBoxName.replace(re, "AOHUnit");
                    var newUnitBox = $("input[name='" + sNewUnitBoxName + "']");
                    if (newUnitBox.length > 0){
                        newUnitBox.val(sSelectedValue);
                    }
                }
            }
        }
    }
}
function MakeCheckBoxString(checkBoxName, checkBoxValue) {
    var sCheckBoxString = " <input class='selectionpending' id=" + MakeValue(checkBoxName) + " name=" + MakeValue(checkBoxName) + " type='checkbox' value=" + MakeValue(checkBoxValue) + " checked='checked' />";
    var sCheckBoxString = sCheckBoxString + "<label for=" + MakeValue(checkBoxName) + ">" + GetSubStringWithDelimiter(checkBoxName, SharedUtils.URIPatternDelimiter, 2) + "</label>";
    return sCheckBoxString;
}
function AppendInputElement(parentElement, inputtype, cssClass, id, onclick, inputvalue, isSelected){
    $(parentElement).append("<input id='" + id + "' type='" + inputtype + "' />");
    var oAppendedInput = $(parentElement).find("input[id='" + id + "']");
    if ($(oAppendedInput).length > 0) {
        $(oAppendedInput).attr('name', id);
        $(oAppendedInput).attr('class', cssClass);
        $(oAppendedInput).prop('value', inputvalue);
        if (onclick.length > 0){
            $(oAppendedInput).attr('onclick', onclick);
        }
        if (isSelected == true){
            if (inputtype == "checkbox") {
                $(oAppendedInput).attr('checked', 'checked');
            }
        }
    }
}
function MakeInputString(type, cssClass, id, onclick, value, isSelected){
    var sInputString = " <input id=" + MakeValue(id) + " name=" + MakeValue(id) + " type=" + MakeValue(type) + " class=" + MakeValue(cssClass) ;
    if (onclick.length > 0){
        sInputString += " onclick=" + MakeValue(onclick);
    }
    if (isSelected == true){
        if (type == "checkbox") {
            sInputString += " checked=" + MakeValue("checked");
        }
    }
    sInputString += " value=" + MakeValue(value) + " />";
    return sInputString;
}
function MakeAnchorString(href, cssClass, id, onclick, value){
    var sAnchorString = " <a id=" + MakeValue(id) + " name=" + MakeValue(id) + " href=" + MakeValue(href) + " class=" + MakeValue(cssClass) + " onclick=" + MakeValue2(onclick) + " >" + value + "</a>";
    return sAnchorString;
}
function MakeAnchorUnobtrusive(href, cssClass, id, onclick, value, datacp, dataca, dataparams) {
    var sAnchorString = " <a id=" + MakeValue(id) + " name=" + MakeValue(id) + " href=" + MakeValue(href) + " class=" + MakeValue(cssClass) + " onclick=" + MakeValue2(onclick) + " " + SharedUtils.DATA_CP + "=" + MakeValue2(datacp) + " " + SharedUtils.DATA_CA + "=" + MakeValue2(dataca) + " " + SharedUtils.DATA_PARAMS + "=" + MakeValue2(dataparams) + " >" + value + "</a>";
    return sAnchorString;
}
function MakeValue(value){
    var sValue = "'" + value + "'";
    return sValue;
}
function MakeValue2(value){
    var sValue = '"' + value + '"';
    return sValue;
}
function MakeImage(id, src, height, width){
    var sInputString = " <img id=" + MakeValue(id) + " name=" + MakeValue(id) + " src=" + MakeValue(src) + " height=" + MakeValue(height) + " width=" + MakeValue(width) + " />";
    return sInputString;
}
function AddFileToList(parentId, contenturipattern, uriBase, params){
    var $oParentOfList =  $("#" + parentId);
    if ($oParentOfList.length > 0) {
        var sCommonName = GetContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_name);
        var sId = GetContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_id);
        var sNewContentURIPattern = ChangeContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_action, "linkedviews");
        sNewContentURIPattern = ChangeContentURIPatternPart(sNewContentURIPattern, SharedUtils.URIPATTERNPART_subaction, "runaddin");
        //make sure they can return to the same uris 
        var sURI = MakeDevTreksURI(contenturipattern, uriBase);
        //make sure the uri's can be displayed under views tab
        //append a checkbox, a common name, a link to view it, and a uri for unique identification
        var sHtml = "<li>";
        //buildPostBody uses last char of checklist names to distinguish selections from checklists like this one
        sHtml += MakeCheckBoxString(contenturipattern + "*", "SelectedFile");
        sHtml += "   ";
        sHtml += MakeAnchorUnobtrusive("#", "JSLink", "package" + sId, "", " Display Under Views Tab ", sNewContentURIPattern, "prepaddin", params);
        sHtml += "<br />";
        sHtml += sURI;
        sHtml += "</li>";
        $oParentOfList.html($oParentOfList.html() + sHtml);
    }
}

function GetArrayLength(array){
    var arrayLength = 0;
    if (array!=undefined && array!=null)
    {
        var arr = array.split(SharedUtils.URIPatternDelimiter);
        arrayLength = arr.length;
    }
    return arrayLength;
}
function MakeDevTreksURI(contenturipattern, uriBase){
    var sURI = uriBase;
    var sNewLocationPath = ChangeContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_action, "preview");
    sURI += sNewLocationPath;
    return sURI;
}
function GetDevPackClickArgument(fileName, clientaction, serveraction, 
    subaction, params) {
    var sArgument;
    if (fileName == null) {
        sArgument = "javascript:devTreksOnClick('', " + clientaction + ", " + serveraction + ", " + subaction;
    }
    else {
        sArgument = "javascript:devTreksOnClick('" + fileName + "', " + clientaction + ", " + serveraction + ", " + subaction;
    }
    if (arguments.length == 5 && params != undefined && params != null)
    {
        sArgument += ", '" + params + "');return false;";
    }
    else
    {
       sArgument += ");return false;";
    }
    return sArgument;
}
function GetJavascriptFunctionArgument(functionName) {
    var sArgument;
    sArgument = "javascript:" + functionName + "();return false;";
    return sArgument;
}
function StoreResources(webResponseURLsElement){
    //resources need caching/handling when devpackpart or devpackdoc filename is being loaded
    var oResourceURLsElement = (typeof(webResponseURLsElement) == "object") ? webResponseURLsElement : GetOneElement(webResponseURLsElement);
    if (oResourceURLsElement){
        //holds delimited urls with alts: resources/network_{networkuripartname}/resourcepack_{id}/resource_{id}/{resourcefilename};altdescription@resources/network_123/resourcepack_42/resource_456/picture1.png;altdescription
        if (document.images) {
            var sResourceURLsElement = oResourceURLsElement.value;
            if (sResourceURLsElement) {
                //is this allowed in web responses?
                var arrResourceURLs = sResourceURLsElement.split("@");
                if (arrResourceURLs) {
                    var sURLwithAltandName;
                    var sName;
                    var sURL;
                    var sDescription;
                    var length = arrResourceURLs.length;
                    for(var i = 0; i < length; i++) {
                        sURLwithAltandName = arrResourceURLs[i];
                        if (sURLwithAltandName) {
                            sName = GetSubStringWithDelimiter(sURLwithAltandName, ";", 2);
                            if (sName) {
                                if (document.images.namedItem(sName)
                                    || document.images[sName]){
                                    //if the first image is already cached, exit StoreResources
                                    alert("StoreResources: cached");
                                    break;
                                }
                                sURL = GetSubStringWithDelimiter(sURLwithAltandName, ";", 0);
                                var oImage = new Image(100, 100);
                                sDescription = GetSubStringWithDelimiter(sURLwithAltandName, ";", 1);
                                //fetch the image from the server and store it in document.images cache
                                oImage.src = sURL;
                                alert("StoreResources: notcached " + sURL);
                                oImage.alt = sDescription;
                            }
                        }
                    }
                }
            }
        }
    }
}
function GetResourceParam(index, imageParams){
    var sParam;
    if (imageParams) {
        var iIndex = parseInt(index);
        var arrResourceURLs = imageParams.split(";");
        if (arrResourceURLs) {
            if (arrResourceURLs.length > iIndex) {
                sParam = arrResourceURLs[iIndex];
            }
        }
    }
    return sParam;
}
function VerifyIsNotEmpty(elId, msgEl, msg){
    var bHasContent = false;
    var $oUploadFileEl = GetOneElement(elId);
    if ($oUploadFileEl.length > 0) {
        if ($oUploadFileEl.val().length > 0){
            bHasContent = true;
        }
        else {
            if (msgEl.length > 0){
                msgEl.html(msg);
                $oUploadFileEl.focus();
            }
        }
    }
    return bHasContent;
}
function RemoveElsByName(name, id){
    var oParentNode;
    var oEls = document.getElementsByName(name);
    if (oEls){
        if (oEls.length > 0){
            for (var i = 0; i < oEls.length; i++) {
                oInputEl = oEls[i];
                oParentNode = oInputEl.parentNode;
                if (oParentNode) {
                    oParentNode.removeChild(oInputEl);
                }
            }
        }
    }
    if (!oParentNode) oParentNode = GetOneElement(id);
    return oParentNode;
}
function HideFileEls(){
    var oEls = document.getElementsByTagName("input");
    if (oEls){
        if (oEls.length > 0){
            for (var i = 0; i < oEls.length; i++) {
                oEl = oEls[i];
                if (oEl) {
                    if (oEl.type == "file") {
                        oEl.style.display = "none";
                    }
                }
            }
        }
    }
}
function ChangeChildrenIds(parentNode, idExtension){
    if ($(parentNode).children().length > 0){
        $.each($(parentNode).children(), function () {
            if (!EndsWith(idExtension, $(this).attr("id"))) {
                $(this).attr("id", $(this).attr("id") + idExtension);
                $(this).attr("name", $(this).attr("id"));
                //recurse
                ChangeChildrenIds(this, idExtension)
            }
        });
    }
}
function AddChildFirstChildIds(parentNodeId, delimiter) {
    var $oParentNode = (typeof(parentNodeId) == "object") ? $(parentNodeId) : $("#" + parentNodeId);
    var sValues = "";
    var i = 0;
    $oParentNode.children("li").each(function (i) {
        if ($(this).children("input:checked").length > 0) {
            sValues += $(this).children(":first").attr("id") + delimiter;
        }
    });
    return sValues;
}
function AddChildSecondChildOnClickParams(parentNodeId, delimiter) {
    var $oParentNode = (typeof(parentNodeId) == "object") ? $(parentNodeId) : $("#" + parentNodeId);
    var sValues = ""; var sAnchorOuterHTML; var sOnClick = ""; 
    var sDocPath = ""; var i = 0; var arrOnClicks = null; 
    var iEndPathIndex;
    $oParentNode.children("li").each(function (i) {
        if ($(this).children("input:checked").length > 0) {
            sDocPath = "";
            sAnchorOuterHTML = $(this).children("a:first").attr(SharedUtils.DATA_PARAMS);
            if (sAnchorOuterHTML.length > 0){
                arrOnClicks = sAnchorOuterHTML.split("uridocpath=");
                if (arrOnClicks) {
                    if (arrOnClicks.length > 0) {
                        iEndPathIndex = arrOnClicks[1].indexOf(".xml") + 4;
                        sDocPath = arrOnClicks[1].substring(0, parseInt(iEndPathIndex));
                    }
                }
            }
            if (sDocPath.length > 0){
                sValues += sDocPath + delimiter;
            }
            else {
                sValues += "none" + delimiter;
            }
        }
    });
    return sValues;
}

function AppendNewChildLink(parentNode, tagName, id, name, href, cssClass, value, onclick){
    var oEl = document.createElement(tagName);
    if (id != null) oEl.setAttribute("id", id);
    if (name != null) oEl.setAttribute("name", name);
    if (href != null) oEl.setAttribute("href", href);
    if (cssClass != null) oEl.setAttribute("class", cssClass);
    if (onclick != null) oEl.setAttribute("onclick", onclick);
    if (value != null) {
        var oText = document.createTextNode(value);
        oEl.appendChild(oText);
    }
    parentNode.appendChild(oEl);
}
function AppendBreakToNode(parentNode){
    var oEl = document.createElement("br");
    parentNode.appendChild(oEl);
}

function DisplayChildNodes(parentNode, nodeToDisplayId) {
    var colChildNodes = parentNode.childNodes;
    if (colChildNodes){
        if (colChildNodes.length > 0){
            var oChildEl;
            for (var i = 0; i < colChildNodes.length; i++) {
                oChildEl = colChildNodes[i];
                if (oChildEl.id) {
                    if (IsMatch(oChildEl.id, nodeToDisplayId)){
                         oChildEl.style.display = "block";
                    }
                    else {
                         oChildEl.style.display = "none";
                    }
                }
            }
        }
    }
}
function GetOnClickFromOuterHTML(outerHTML){
    //parse the onclick="" string (the plus one is for the starting quotation)
    var iStart = outerHTML.indexOf("onclick=");
    var iEnd = outerHTML.indexOf('"', parseInt(iStart) + 9);
    //account for the quotations
    var sOnClickString = outerHTML.substring(parseInt(iStart) + 9, parseInt(iEnd));
    return sOnClickString;
}
function GetEventHandlerParams(id, onClickString){
    //refactor when a better way to pass params to events is found
    var iStart = onClickString.indexOf("javascript:");
    var iEnd = onClickString.indexOf("(", iStart);
    var sHandler = onClickString.substring(parseInt(iStart) + 11, parseInt(iEnd));
    iStart = iEnd;
    iEnd = onClickString.lastIndexOf(")");
    var sParams = onClickString.substring(parseInt(iStart) + 1, parseInt(iEnd));
    if (sParams) {
        //names in uripatterns can include commas
        sParams = CleanUpCommasInNames(sParams, 0);
        //form params can't have commas in names
        sParams = CleanUpUriCommasInNames(sParams);
        //replace the existing argument delimiters
        sParams = ReplaceString(sParams, ",", "?");
        //replace the name placeholders with original chars
        sParams = ReplaceString(sParams, "/", ",");
        //trim off leading and trailing quotes and spaces
        sParams = TrimParams(sParams);
    }
    var arrEventHandlers = new Array(sHandler, sParams);
    return arrEventHandlers;
}
function CleanUpCommasInNames(params, startIndex){
    var sParams = params;
    var iEnd = sParams.indexOf(SharedUtils.URIPatternDelimiter, startIndex);
    //uri= are handled by CleanUpUriCommasInNames
    var iStartURIIndex = sParams.indexOf("uri=");
    if (iStartURIIndex == -1) iStartURIIndex = iEnd + 1;
    if (iEnd > -1 && (iEnd < iStartURIIndex)) {
        //all string arguments, such as uri patterns, are enclosed in single quotes i.e. 'networkname/nodename/commonname/id'
        var iStart = sParams.lastIndexOf("'", iEnd);
        var sName = sParams.substring(parseInt(iStart), parseInt(iEnd))
        //replace commas in name with placeholder
        var sNameNoCommas =  ReplaceString(sName, ",", "/");
        sParams = sParams.replace(sName, sNameNoCommas);
        //start with next argument delimiter
        iStart = sParams.indexOf(",", iEnd);
        if (iStart > -1) {
            sParams = CleanUpCommasInNames(sParams, iStart)
        }
    }
    return sParams;
}
function CleanUpUriCommasInNames(params){
    var sParams = params;
    var iStartIndex;
    //form params uripatterns have uri= in name i.e. calcdocuri=
    iStartIndex = sParams.indexOf("uri=");
    if (iStartIndex > 0) {
        //replace commas in rest of param string (using /g in regex)  with placeholder
        sName = sParams.substring(iStartIndex);
        sNameNoCommas = ReplaceString(sName, ",", "/");
        sParams = sParams.replace(sName, sNameNoCommas);
    }
    return sParams;
}
function TrimStylesheetParams(params) {
    //stylsheets sometimes have to use '' to escape the string
    var sParams = params;
    if (sParams) {
        if (isNaN(sParams)) {
            sParams = sParams.replace(/^'/g, "");
            sParams = sParams.replace(/'$/g, "");
        }
    }
    return sParams;
}
function TrimParams(params){
    var sParams = params;
    var arrParams = params.split("?");
    var i; var sParam; var sNewParam;
    var iParmCount = arrParams.length;
    for (i = 0; i < iParmCount; i++) {
        sParam = arrParams[i];
        if (sParam){
            if (i == 0) sParams = "";
            sNewParam = sParam.replace(/^'/g, "");
            sNewParam = sNewParam.replace(/'$/g, "");
            sNewParam = sNewParam.replace(/^\s/g, "");
            sNewParam = sNewParam.replace(/\s$/g, "");
            //inconsistencies with quote locations
            sNewParam = sNewParam.replace(/^'/g, "");
            sNewParam = sNewParam.replace(/'$/g, "");
            if (i != 0) sParams += "?";
            sParams += sNewParam;
        }
    }
    return sParams;
}
function RemoveLastAsterisk(nametoclean){
    var sNewName = nametoclean;
    var oForm = new Form.FormFilter();
    var bHasAsterisk = oForm.lastCharIsMatch(nametoclean);
    if (bHasAsterisk) {
        sNewName = nametoclean.replace(/\*$/g, "");
        //can select, discard, reselect in select panel
        sNewName = sNewName.replace(/\*$/g, "");
        sNewName = sNewName.replace(/\*$/g, "");
        sNewName = sNewName.replace(/\*$/g, "");
    }
    return sNewName;
}
function ClearPackage(){
    var oParentOfList = GetOneElement("olFileList");
    if ($(oParentOfList).length > 0) {
        $(oParentOfList).html("");
    }
    var spanWithLinks = GetOneElement("divDownloadPackLink");
    if ($(spanWithLinks).length > 0) {
        $(spanWithLinks).html("");
    }
}
function HideOrShowPackageCommands(needsdisplay) {
    if (needsdisplay == false) {
        var oCmd1 = GetOneElement("btnClearPack");
        if ($(oCmd1).length > 0) {
            $(oCmd1).hide();
        }
        var oCmd2 = GetOneElement("btnMakePackage");
        if ($(oCmd2).length > 0) {
            $(oCmd2).hide();
        }
    }
    else {
        var oCmd1 = GetOneElement("btnClearPack");
        if ($(oCmd1).length > 0) {
            $(oCmd1).show();
        }
        var oCmd2 = GetOneElement("btnMakePackage");
        if ($(oCmd2).length > 0) {
            $(oCmd2).show();
        }
    }
}
function GetNodeNameFromSubAppType(subapp)
{
    var sNodeName = "";
    if (StartsWith("budget", subapp))
    {
        sNodeName = "budgettimeperiod";
    }
    else if (StartsWith("investment", subapp))
    {
        sNodeName = "investmenttimeperiod";
    }
    else if (StartsWith("outcome", subapp)) {
        sNodeName = "outcome";
    }
    else if (StartsWith("operation", subapp))
    {
        sNodeName = "operation";
    }
    else if (StartsWith("component", subapp))
    {
        sNodeName = "component";
    }
    else if (StartsWith("output", subapp))
    {
        sNodeName = "output";
    }
    else if (StartsWith("input", subapp))
    {
        sNodeName = "input";
    }
    else if (StartsWith("local", subapp))
    {
        sNodeName = "local";
    }
    else if (StartsWith("devpack", subapp))
    {
        sNodeName = "devpackpart";
    }
    else if (StartsWith("linkedview", subapp))
    {
        sNodeName = "linkedviewpack";
    }
    else if (StartsWith("resource", subapp))
    {
        sNodeName = "resourcepack";
    }
    else if (StartsWith("club", subapp)) {
        sNodeName = "account";
    }
    else if (StartsWith("member", subapp)) {
        sNodeName = "memberbase";
    }
    else if (StartsWith("agreement", subapp)) {
        sNodeName = "service";
    }
    else if (StartsWith("network", subapp)) {
        sNodeName = "networkbase";
    }
    return sNodeName;
}
function GetSubAppTypeFromNodeName(nodeName)
{
    var sSubAppType = "";
    if (StartsWith("budget", nodeName))
    {
        sSubAppType ="budgets";
    }
    else if (StartsWith("investment", nodeName))
    {
        sSubAppType ="investments";
    }
    else if (StartsWith("outcome", nodeName)) {
        sSubAppType = "outcomeprices";
    }
    else if (StartsWith("operation", nodeName))
    {
        sSubAppType ="operationprices";
    }
    else if (StartsWith("component", nodeName))
    {
        sSubAppType ="componentprices";
    }
    else if (StartsWith("output", nodeName))
    {
        sSubAppType ="outputprices";
    }
    else if (StartsWith("input", nodeName))
    {
        sSubAppType ="inputprices";
    }
    else if (StartsWith("local", nodeName))
    {
        sSubAppType ="locals";
    }
    else if (StartsWith("devpack", nodeName))
    {
        sSubAppType ="devpacks";
    }
    else if (StartsWith("linkedview", nodeName))
    {
        sSubAppType ="linkedviews";
    }
    else if (StartsWith("resource", nodeName))
    {
        sSubAppType ="resources";
    }
    else if (StartsWith("account", nodeName)) {
        sSubAppType = "clubs";
    }
    else if (StartsWith("member", nodeName)) {
        sSubAppType = "members";
    }
    else if (StartsWith("service", nodeName)) {
        sSubAppType = "agreements";
    }
    else if (StartsWith("network", nodeName)) {
        sSubAppType = "networks";
    }
    return sSubAppType;
}
function GetFormElementValue(formId, inputId, needsRemoval) {
    var formVal = "";
    var formUpload = $("#" + formId);
    if ($(formUpload)) {
        formUpload = $("#" + formId)[0];
        var oAppendedInput = $(formUpload).find("input[id='" + inputId + "']");
        if ($(oAppendedInput).length > 0) {
            formVal = $(oAppendedInput).val();
            if (needsRemoval) {
                $(oAppendedInput).remove();
            }
        }
    }
    return formVal;
}
function GetPostRelativePath()
{
    var sPathToResources = FixRelativePath(SharedUtils.DefaultPOSTRelPath);
    return sPathToResources;
}
function GetRelativePath() {
    var sPathToResources = FixRelativePath(SharedUtils.DefaultWebResourcesRelPath);
    return sPathToResources;
}
function FixRelativePath(pathToResources) {
    var sPathToResources = pathToResources;
    var iStartIndex = 0;
    //the uri may not include the filextension parameter (i.e. relpath is one less folder)
    //default is length of ten
    //http: //www.agtreks.org/commontreks/preview/commons/resourcepack/Deployment Videos/348/none
    uri = window.location.href;
    if (uri != null) {
        var arrURIPattern = uri.split(SharedUtils.URIPatternDelimiter);
        if (arrURIPattern != null) {
            if (arrURIPattern.length == 11) {
            //http: //localhost:44300/buildtreks/preview/commercial/output/2012 NIST 135 style Example 1/2141223440/none/
            //need one less folder (../)
                iStartIndex = 2;
            }
            else if (arrURIPattern.length == 12) {
                //need two less folders (../../)
                iStartIndex = 5;
            }
        }
    }
    if (iStartIndex > 0) {
        sPathToResources = sPathToResources.substr(iStartIndex);
    }
    return sPathToResources;
}
(function ($) {
    $.fn.toggleDisabled = function () {
        return this.each(function () {
            this.disabled = !this.disabled;
        });
    };
})(jQuery); 