
/*
    Name:Select1.js
    Author: www.devtreks.org
    Date: 2014, June
    Purpose: handle client selections
*/

function getSelections(spanName){
    var colCheckBox = $("input:checked");
    if ($(colCheckBox)) {
        var oSpanSelections = $("#" + spanName);
        if (oSpanSelections.length > 0) {
            var i = 0; var sURIPattern = ""; var sHtml = "";
            var sValue = "";
            $(colCheckBox).each(function (i) {
                sValue = $(this).val();
                sValue = RemoveLastAsterisk(sValue);
                if (sValue == "true") {
                    sURIPattern = $(this).attr("id");
                    sHtml += MakeCheckBoxString(sURIPattern, "true");
                    $(this).prop("value", "");
                    $(this).prop("checked", "false");
                    //$(this).removeProp("checked");
                }
            });
            oSpanSelections.html("");
            oSpanSelections.html(sHtml);
            if (sHtml.length > 1){
                DisableButton(SharedUtils.CLIENTACTION_discardselects, false);
                DisableButton(SharedUtils.SERVERSUBACTION_saveselects, false);
            }
            else {
                DisableButton(SharedUtils.CLIENTACTION_discardselects, true);
                DisableButton(SharedUtils.SERVERSUBACTION_saveselects, true);
            }
        }
    }
}
function DisableButton(buttonId, disabled){
    var oButtonl; var sButtonId;
    if (buttonId == SharedUtils.CLIENTACTION_discardselects){
        sButtonId = "discardselects";
    }
    else if (buttonId == SharedUtils.SERVERSUBACTION_saveselects){
        sButtonId = "saveselects";
    }
    oButton = $("#" + sButtonId);
    if (oButton.length > 0) {
        if (disabled){
            $(oButton).prop("disabled", "disabled");
       }
       else {
            $(oButton).removeProp("disabled");
       }
    }
}
function deleteSelections(spanName){
    var oSpanSelections = $("#" + spanName);
    if ($(oSpanSelections).length > 0) {
        oSpanSelections.html("");
        DisableButton(SharedUtils.CLIENTACTION_discardselects, true);
        DisableButton(SharedUtils.SERVERSUBACTION_saveselects, true);
    }
    ResetSelections();
}
function ResetSelections() {
    var colCheckBox = $("input:checked");
    if ($(colCheckBox)) {
        var i = 0;
        $(colCheckBox).each(function (i) {
            $(this).removeProp("checked");
        });
    }
}
function getSelectionFiles(spanName, contenturipattern, nodeuripattern, nodeNameNeeded, attributeName, params){
    var oSpanSelectFiles = $("#" + spanName);
    if (!$(oSpanSelectFiles).length > 0) {
        //they haven't opened an edit view yet, making selections using select view
        MakeSpanSelectionFiles(spanName);
        oSpanSelectFiles = $("#" + spanName);
    }
    if (oSpanSelectFiles.length > 0) {
        var spanSelectFileHtml;
        spanSelectFileHtml = GetCancelSelectionElementHtml("selectscancel", "", "Cancel Selections");
        spanSelectFileHtml += GetSelectionElementHtml("selectsuripattern", "File for Adding Selections:", contenturipattern);
        spanSelectFileHtml += GetSelectionElementHtml("selectsnodeuripattern", "Node For Appending Selections :", nodeuripattern);
        spanSelectFileHtml += GetSelectionElementHtml("selectionsnodeneededname", "Node For Making Selections:", nodeNameNeeded);
        if (arguments.length >= 5 && attributeName != null && attributeName != "") {
            spanSelectFileHtml += GetSelectionElementHtml("selectsattributename", "Attribute For Setting URI:", attributeName);
        }
        if (arguments.length >= 6 && params != null && params != "") {
            spanSelectFileHtml += GetSelectionElementHtml("selectscalcparams", "Linked Views Parameters:", params);
        }
        $(oSpanSelectFiles).html(spanSelectFileHtml);
        deleteSelections("spanSelections");
        var sSearchNodeNeeded = MakeSearchNodeNeededName(nodeNameNeeded);
        ChangeSearchElements(sSearchNodeNeeded);
        //if the search panel is open, change the node and network being searched
        ChangeSelection("lstNetworkFilter", "myagreement");
        var sStartSearchURIPattern = MakeStartSearchURIPattern(contenturipattern, sSearchNodeNeeded);
        devTreksOnClick(sStartSearchURIPattern, SharedUtils.CLIENTACTION_postrequest, params);
    }
}
function MakeSelectContentURIPattern(uripattern, serveraction, subaction){
    var contenturipattern = "";
    var controller = GetSubStringWithDelimiter(window.location.href, SharedUtils.URIPatternDelimiter, 3);
    contenturipattern = MakeContentURIPatternUsingURIPattern(controller, serveraction, uripattern, 
        subaction, subaction, SharedUtils.NONE);
    return contenturipattern;
}
function GetCancelSelectionElementHtml(newId, newLabel, newValue){
    var sOnClick = GetJavascriptFunctionArgument("CancelSelections");
    var selectElHtml;
    selectElHtml = "<label>" + newLabel + "</label>";
    selectElHtml += "<br />";
    selectElHtml += MakeInputString("button", "SubmitButton1Enabled225", newId, sOnClick, newValue, "false");
    selectElHtml += "<br />";
    return selectElHtml;
}
function GetSelectionElementHtml(newId, newLabel, newValue){
    var selectElHtml;
    selectElHtml = "<label>" + newLabel + "</label>";
    selectElHtml += "<br />";
    selectElHtml += MakeInputString("text", "Input500NoEdit", newId, "", newValue, "false");
    selectElHtml += "<br />";
    return selectElHtml;
}
function GetSelections(serveraction) {
    var selections = "";
    if (serveraction == SharedUtils.SERVERACTION_select) {
        var colSelectionsPending = $(".selectionpending");
        var sValue = "";
        var sId = "";
        var oForm = new Form.FormFilter();
        //add the selections pending to the form elements
        if ($(colSelectionsPending)) {
            $(colSelectionsPending).each(function (i) {
                sValue = $(this).val();
                sId = $(this).attr("id");
                selections += oForm.addFormElement(sId, sValue);
            });
        }
        //add a form el for the node for making selections
        var selectsNode = GetOneElement("selectionsnodeneededname");
        if ($(selectsNode)) {
            sValue = $(selectsNode).val();
            if (sValue) {
                if (sValue.length > 1) {
                    selections += oForm.addFormElement("selectionsnodeneededname", sValue);
                }
            }
        }
    }
    return selections;
}
function MakeCancelSelectionElement(spanParent, newId, newLabel, newValue){
    var oSelectionEl = $("#" + newId);
    if ($(oSelectionEl).length > 0) {
        //already on hand
    }
    else {
        var sOnClick = GetJavascriptFunctionArgument("CancelSelections");
        $(spanParent).append("<br />");
        AppendInputElement(spanParent, "button", "SubmitButton1Enabled225", newId, sOnClick, newValue, "false");
        $(spanParent).append("<br />");
        $(spanParent).append("<br />");
    }
}
function CancelSelections(){
    var $oSpanSelections = $("#spanSelectionFiles");
    if ($oSpanSelections.length > 0) {
        $oSpanSelections.html("");
    }
}
function MakeSelectionElement(spanParent, newId, newLabel, newValue){
    var oSelectionEl = $("#" + newId);
    if ($(oSelectionEl).length > 0) {
        $(oSelectionEl).prop("value", newValue);
    }
    else {
        $(spanParent).append("<label>" + newLabel + "</label>");
        $(spanParent).append("<br />");
        //jquery 1.4.1 .append no longer handles a text input's value attribute
        AppendInputElement(spanParent, "text", "Input500NoEdit", newId, "", newValue, "false");
        $(spanParent).append("<br />");
    }
}
function MakeSpanSelectionFiles(spanName){
    var oEditPanelDiv = $("#divedit");
    if ($(oEditPanelDiv).length > 0) {
        var sHtml = "<span id=" + MakeValue(spanName) + "></span>";
        $(oEditPanelDiv).append(sHtml);
    }
}
function ChangeSearchElements(nodeNameNeeded){
    var subAppType = GetSubAppTypeFromNodeName(nodeNameNeeded);
    ChangeSelection("lstServiceGroupFilter", subAppType);
}
function MakeStartSearchURIPattern(contenturipattern, searchNodeNeeded) {
    var sStartSearchURIPattern = contenturipattern;
    if (searchNodeNeeded) {
        sStartSearchURIPattern = ChangeContentURIPatternPart(contenturipattern, SharedUtils.URIPATTERNPART_action, "search");
        sStartSearchURIPattern = ChangeContentURIPatternPart(sStartSearchURIPattern, SharedUtils.URIPATTERNPART_subaction, "respondwithhtml");
        sStartSearchURIPattern = ChangeContentURIPatternPart(sStartSearchURIPattern, SharedUtils.URIPATTERNPART_node, searchNodeNeeded);
        sStartSearchURIPattern = ChangeContentURIPatternPart(sStartSearchURIPattern, SharedUtils.URIPATTERNPART_name, SharedUtils.NONE);
        sStartSearchURIPattern = ChangeContentURIPatternPart(sStartSearchURIPattern, SharedUtils.URIPATTERNPART_id, "0");
    }
     return sStartSearchURIPattern;
}
function MakeSearchNodeNeededName(nodeNameNeeded){
    var sSearchNodeNeeded = nodeNameNeeded;
    //selections for linkedviews and resourcepacks need adjusting for good searches
    if (nodeNameNeeded == "linkedviewresourcepack" || nodeNameNeeded == "devpackresourcepack") {
        sSearchNodeNeeded = "resourcepack";
    }
    return sSearchNodeNeeded;
}
